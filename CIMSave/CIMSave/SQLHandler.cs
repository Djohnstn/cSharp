using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.ComponentModel;
using System.Reflection;
using System.Collections.Concurrent;

namespace CIMSave
{

    public static class StringExtensionMethods
    {
        public static bool IsAlphaNum(this string str)
        {
            if (string.IsNullOrEmpty(str)) return false;
            return (str.ToCharArray().All(c => Char.IsLetter(c) || Char.IsNumber(c) || ("_-".IndexOf(c) > -1)));
        }
    }

    public static class Utility
    {
        public static string GetDescriptionFromEnumValue(Enum value)
        {
            return !(value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .SingleOrDefault() is DescriptionAttribute attribute) 
                ? value.ToString() : attribute.Description;
        }

        public static T GetEnumValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum)
                throw new ArgumentException();
            FieldInfo[] fields = type.GetFields();
            var field = fields
                            .SelectMany(f => f.GetCustomAttributes(
                                typeof(DescriptionAttribute), false), (
                                    f, a) => new { Field = f, Att = a })
                            .Where(a => ((DescriptionAttribute)a.Att)
                                .Description == description).SingleOrDefault();
            return field == null ? default(T) : (T)field.Field.GetRawConstantValue();
        }
    }


    class Servers
    {

        // keep up server name to ID translation, no need to go back to SQL for this bach run.
        static ConcurrentDictionary<string, int> xServerIDs = new ConcurrentDictionary<string, int>();
        static readonly string[] xqueryServers = new string[]
        {           // try these queries to see if can find the server id, or create a server id
                    "Select id from [dbo].[Servers] Where NodeName = @server",
                    "Select id from [dbo].[OtherServers] Where ServerName = @server",
                    "Insert into [dbo].[OtherServers] (ServerName) OUTPUT Inserted.ID Values(@server)"
                    //"Insert into [dbo].[OtherServers] (ServerName) Values( @server); Select id from [dbo].[OtherServers] Where ServerName = @server"
        };
        const string ServerParameter = "server";
        public int ID(string serverName)
        {
            if (xServerIDs.TryGetValue(serverName, out int idCache))
            {
                return idCache;
            }
            int factor = 1;
            foreach (var query in xqueryServers)
            {
                var sqlbase = new SQLHandlerBase();
                var serverID = sqlbase.DoQuery<int>(query, -1, ServerParameter, serverName);
                if (serverID > 0)
                {
                    var vServerId = factor * serverID;
                    xServerIDs.TryAdd(serverName, vServerId); // save serverid, then return it
                    return vServerId;
                }
                factor = -1;
            }
            return -1;
        }

    }

    class InstancesXold
    {
        // keep up name to ID translation, no need to go back to SQL for this batch run.
        static ConcurrentDictionary<string, int> xIDs = new ConcurrentDictionary<string, int>();
        static readonly string[] xquerys = new string[]
        {           // try these queries to see if can find the named entity id, or create a named entity id
                    "Select id from [dbo].[CIM__Instances] Where Identity = @name",
                    "Insert into [dbo].[CIM__Instances] (Identity) OUTPUT Inserted.ID Values(@name)"
        };
        const string NameParameter = "name";
        public int ID(string Name)
        {
            if (xIDs.TryGetValue(Name, out int idCache))
            {
                return idCache;
            }
            var sqlbase = new SQLHandlerBase();
            foreach (var query in xquerys)
            {
                var id = sqlbase.DoQuery<int>(query, -1, NameParameter, Name);
                if (id > 0)
                {
                    xIDs.TryAdd(Name, id); // save serverid, then return it
                    return id;
                }
            }
            return -1;
        }
    }

    public class Instances
    {
        private static DBMapper<string, int> map = new DBMapper<string, int>("_Instances", -1, "Name");
        public static int ID(string name) => map.ID(name);
        //public int ID2(string name) => (new DBMapper<string, int>("_Instances", -1, "Identity")).ID(name);
    }
    public class Paths
    {
        private static DBMapper<string, int> map = new DBMapper<string, int>("_Paths", -1, "Name");
        public static int ID(string name) => map.ID(name);
        //public int ID(string name) => (new DBMapper<string, int>("_Paths", -1, "Identity")).ID(name);
    }



    class SQLHandler
    {
        private readonly SQLHandlerBase sQLbase;
        private readonly string _connectionString;
        private string ConnectionString() => _connectionString;
        public static int MaxStringLength { get; } = 2000;

        private string lastSchema;
        private string lastTable;
        private bool lastTableExists = false;

        private List<DataRow> lastTableColumns;

        public int ServerID(string servername) => new Servers().ID(servername);
        //public int InstanceID(string instancename) => new Instances().ID(instancename);
        //public int PathID(string pathname) => new Paths().ID(pathname);
        public int InstanceID(string instancename) => Instances.ID(instancename);
        public int PathID(string pathname) => Paths.ID(pathname);

        public bool TableExists(string schema, string tableName)
        {
            bool exists;
            if (!(String.IsNullOrWhiteSpace(lastSchema) || String.IsNullOrWhiteSpace(lastTable))
                 && lastTable.Equals(tableName) && lastSchema.Equals(schema))
            {
                exists = lastTableExists;
            }
            else
            {
                string query = $@"select * 
                        from sys.tables t
                        inner
                        join sys.schemas s on s.schema_id = t.schema_id
                        where t.type = 'U' 
                            and t.name = '{tableName}' and s.name = '{schema}'";

                List<DataRow> dr = DoQuery(query);
                exists = dr.Count > 0;
                lastTableExists = exists;
                lastSchema = schema;
                lastTable = tableName;
                lastTableColumns = null;
            }
            return exists;
        }

        /// <summary>
        /// find column, determine size of column
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="tableName"></param>
        /// <param name="colName"></param>
        /// <returns>0 for column not found, or length</returns>

        public int ColumnLength(string schema, string tableName, string colName)
        {   // , ty.length, c.*

            //bool exists;
            Int16 length = 0;
            if (!(String.IsNullOrWhiteSpace(lastSchema) || String.IsNullOrWhiteSpace(lastTable))
                && lastTable.Equals(tableName) && lastSchema.Equals(schema))
            {
                if (null == lastTableColumns)
                {
                    string query = $@"select c.name, ty.name as typeName, c.max_length, c.is_nullable 
                            from sys.columns c 
                            inner join sys.tables t on t.object_id = c.object_id
                            inner join sys.schemas s on s.schema_id = t.schema_id 
                            inner join sys.systypes ty on ty.xusertype = c.user_type_id
                            where t.type = 'U' and t.name = '{tableName}' and s.name = '{schema}'";
                    //and c.name = '{colName}'";
                    lastTableColumns = DoQuery(query);
                }
            }
            else
            {
            }

            if (lastTableColumns.Count > 0)
            {
                DataRow dr = lastTableColumns.Find(x => x.Field<string>(0) == colName);
                if (dr != null)
                {
                    var typeName = dr.Field<string>(1);
                    var len = dr.Field<Int16>(2);
                    if (len > 0)  // don't half (max) fields...
                    {
                        if (typeName.Equals("nvarchar") || typeName.Equals("nchar")) len /= 2;
                        length = (Int16)len;
                    }
                }
            }
            return length;
        }


        public string DTtoSelect(DataTable dt, string schema, string tableName, string serverName)
        {
            int serverId = ServerID(serverName);
            //bool pathWhere = false;
            //int pathId = 0;
            //bool instanceWhere = false;
            //int instanceId = 0;
            var sb = new StringBuilder();
            sb.AppendLine("Select ");
            string comma = "";
            foreach (DataColumn col in dt.Columns)
            {
                var colName = col.ColumnName;
                if (colName == "Server") colName = "ServerId";
                if (colName == "Instance") colName = "InstanceId"; //instanceWhere = true; InstanceID = InstanceID(what?)}
                if (colName == "Path") colName = "PathId"; //, ...};
                if (!(colName.StartsWith("_")))
                {
                    //sb.AppendLine($" {comma} [t].[{colName}] ");
                    sb.Append($" {comma} [t].[{colName}] ");
                    if (comma.Length == 0) comma = ",";
                }
            }
            //sb.AppendLine($" FROM [{schema}].[{tableName}] t ");
            //sb.AppendLine($" WHERE t.[ServerId] = {serverId} ");
            sb.Append($" FROM [{schema}].[{tableName}] t ");
            sb.Append($" WHERE t.[ServerId] = {serverId} ");
            return sb.ToString();
        }
        // HashTable
        //public static void DTtoConsole(DataTable dt, Dictionary<int, string> hashList, string dtName)
        public static void DTtoConsole(DataTable dt, HashTable hashList, string dtName)
        {
            Console.WriteLine($"{dtName}:{dt.TableName} rows:{dt.Rows.Count}");
            //var idFound = dt
            string comma = "";
            {
                var sb = new StringBuilder();
                foreach (DataColumn col in dt.Columns)
                {
                    var colName = col.ColumnName.PadRight(6, ' ').Substring(0, 6);
                    sb.Append($"{comma}{colName}~__");
                    if (comma.Length == 0) comma = ",";
                }
                sb.Append($"{comma}_Hash_");
                Console.WriteLine(sb.ToString());
            }
            foreach (DataRow dr in dt.Rows)
            {
                comma = "";
                var sbr = new StringBuilder();
                foreach (var dc in dr.ItemArray)
                {
                    var rawval = ((dc == null) ? "" : dc.ToString());
                    var val = rawval.PadRight(6, ' ').Substring(0, 6);
                    sbr.Append($"{comma}{val}~{rawval.Length.ToString("d2")}");
                    if (comma.Length == 0) comma = ",";
                }
                var rid = dr.Field<int>("id");
                //bool hashFound = hashList.TryGetValue(rid, out string hashString);
                bool hashFound = hashList.TryGetHash(rid, out HashSig hashSig);
                string hashString = hashFound ? hashSig.ToString() : string.Empty;
                sbr.Append($"{comma}{hashString.PadRight(7, ' ').Substring(1, 6)}");
                Console.WriteLine(sbr.ToString());
                sbr.Clear();
            }
        }


        public bool FillDT(DataTable dt, string schema, string tableName, string serverName)
        {
            string sqlQuery = DTtoSelect(dt, schema, tableName, serverName);
            using (SqlConnection con = new SqlConnection(ConnectionString()))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(sqlQuery, con))
                using (SqlDataAdapter da = new SqlDataAdapter(command))
                using (DataSet ds = new DataSet() { EnforceConstraints = false })
                {
                    ds.Tables.Add(dt);
                    da.Fill(dt);
                    //ds.EnforceConstraints = true;
                    ds.Tables.Remove(dt);
                }
                //int updated = dt.Select(null, null, DataViewRowState.ModifiedCurrent).Length;
            }
            foreach (DataRow row in dt.Rows)
            {
                if (row.HasErrors)
                {
                    var rowId = row.Field<int>(1);
                    var colsinError = row.GetColumnsInError();
                    foreach (var col in colsinError)
                    {
                        Console.WriteLine($"Error in {schema}.{tableName} row {rowId} {col.ColumnName}");
                    }
                }
            }
            return true;
        }

        // https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/updating-data-sources-with-dataadapters#example-1
        public int UpdateDA(DataTable dt, string schema, string tableName, string serverName)
        {
            //Console.WriteLine($"Update: [{schema}].[{dt.TableName}] {serverName} rows:{dt.Rows.Count}");
            int recordCount = -1;
            DataTable dtWork = dt.Clone();
            using (SqlConnection con = new SqlConnection(ConnectionString()))
            {
                con.Open();
                string sqlQuery = DTtoSelect(dt, schema, tableName, serverName);
                using (SqlCommand command = new SqlCommand(sqlQuery, con))
                using (SqlDataAdapter da = new SqlDataAdapter(command))
                {
                    //da.SelectCommand = sqlQuery;
                    // https://www.pcreview.co.uk/threads/sqlcommandbuilder-not-setting-my-identity-column.2782244/
                    // http://steptodotnet.blogspot.com/2012/07/how-to-get-identity-field-value-after.html
                    //var foo = da.TableMappings;
                    da.FillSchema(dtWork, SchemaType.Mapped);
                    SqlCommandBuilder builder = new SqlCommandBuilder(da);
                    var deleteCmd = new SqlCommand($"DELETE FROM [{schema}].[{tableName}] WHERE [id] = @id", con)
                    {
                        CommandType = CommandType.Text
                    };
                    deleteCmd.Parameters.Add("@id", SqlDbType.Int, 4, "id");

                    //da.DeleteCommand = builder.GetDeleteCommand();
                    da.DeleteCommand = deleteCmd;
                    da.InsertCommand = builder.GetInsertCommand();
                    int delete2 = (dt.Select(null, null, DataViewRowState.Deleted)).Length;
                    int deleted = da.Update(dt.Select(null, null, DataViewRowState.Deleted));
                    int adde2 = (dt.Select(null, null, DataViewRowState.Added)).Length;
                    int added = da.Update(dt.Select(null, null, DataViewRowState.Added));
                    //da.Update();
                    var recordPlan = delete2 + adde2;
                    recordCount = deleted + added; //  + updated
                    int dtrows = dt.Rows.Count;
                    //if ((recordCount > 0) || (recordPlan > 0) || (dt.Rows.Count > 0))
                    //{
                    Console.WriteLine($"{schema}.{tableName}: rows: {dtrows}. {added} added of {adde2}, {deleted} deleted of {delete2}."); // {updated} updated, 
                    //}
                }

            }
            return recordCount;
        }


        private List<DataRow> DoQuery(string sqlQuery)
        {
            List<DataRow> dr;
            using (SqlConnection con = new SqlConnection(ConnectionString()))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(sqlQuery, con))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    var dt = new DataTable();
                    dt.Load(reader);
                    dr = dt.AsEnumerable().ToList();
                }
            }
            return dr;
        }

        private List<DataRow> DoQuery(string sqlQuery, List<SqlParameter> paramList)
        {
            List<DataRow> dr;
            using (SqlConnection con = new SqlConnection(ConnectionString()))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(sqlQuery, con))
                {
                    foreach (var p in paramList)
                    {
                        command.Parameters.Add(p);
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        var dt = new DataTable();
                        dt.Load(reader);
                        dr = dt.AsEnumerable().ToList();
                    }

                }
            }
            return dr;
        }

        private int DoCommand(string sqlQuery)// string[] { parameters, names})
        {
            int rows = -1;
            if (String.IsNullOrWhiteSpace(sqlQuery))
            {
                rows = 0;
            }
            else
            {
                using (SqlConnection con = new SqlConnection(ConnectionString()))
                {
                    con.Open();

                    using (SqlCommand command = new SqlCommand(sqlQuery, con))
                    {
                        rows = command.ExecuteNonQuery();
                    }
                }
            }
            return rows;
        }


        private string newTableCommand = "";
        private StringBuilder newTableParameters = null;
        private bool firstParameter = true;
        private string newIndexCommand = "";
        private string newTableName = "";
        private string newSchemaName = "";

        public SQLHandler()
        {
            sQLbase = new SQLHandlerBase();
            _connectionString = sQLbase.ConnectionString ?? throw new ArgumentNullException(nameof(_connectionString));
        }

        public bool PrepareTable(string schema, string tableName, int nameLength = 32)
        {
            bool success = false;
            if (!schema.IsAlphaNum()) throw new ArgumentException($"invalid schema name: {schema}");
            if (!tableName.IsAlphaNum()) throw new ArgumentException($"invalid table name: {tableName}");
            if (newTableCommand.Length != 0) throw new ArgumentException($"PepareTable {tableName} before CreateTable for: {newTableCommand}");

            if (TableExists(schema, tableName))
            {   // clear out the table setup if table already exists
                AbandonNewTable();
                success = false;
            }
            else
            {
                string sql1 = $@"Create Table [{schema}].[{tableName}]
                    (
	                    [Id] INT NOT NULL IDENTITY (1, 1) CONSTRAINT PK_{schema}_{tableName}_ID  PRIMARY KEY,
	                    [ServerId] int not null,
	                    [InstanceId] int not null,
	                    [PathId] int not null,
	                    [Name] nvarchar({nameLength}) not null
                    ";
                string sql2 = $"CREATE INDEX [IX_{tableName}_ServerInstancePathName] ON [dbo].[{tableName}] ([ServerId],[InstanceId],[PathId],[Name]);";
                newTableName = tableName;
                newSchemaName = schema;
                newTableCommand = sql1;
                newTableParameters = new StringBuilder();
                firstParameter = false;
                newIndexCommand = sql2;
                success = true;
            }

            return success;
        }

        public bool PrepareTableUpdate(string schema, string tableName)
        {
            bool success = false;
            if (!schema.IsAlphaNum()) throw new ArgumentException($"invalid schema name: {schema}");
            if (!tableName.IsAlphaNum()) throw new ArgumentException($"invalid table name: {tableName}");
            if (newTableCommand.Length != 0) throw new ArgumentException($"PepareTableUpdate {tableName} before CreateTable for: {newTableCommand}");

            if (!TableExists(schema, tableName))
            {   // clear out the table setup if table does not already exists
                AbandonNewTable();
                success = false;
            }
            else
            {
                string sql1 = $"Alter Table [{schema}].[{tableName}] ";
                string sql2 = "";
                newTableName = tableName;
                newSchemaName = schema;
                newTableCommand = sql1;
                newTableParameters = new StringBuilder();
                firstParameter = false;
                newIndexCommand = sql2;
                success = true;
            }

            return success;
        }


        public enum ColumnType
        {
            [Description("?Unknown")]
            ctUnknown = -1,
            [Description("BigInt")]
            ctBigInt = SqlDbType.BigInt,
            [Description("Bit")]
            ctBit = SqlDbType.Bit,
            [Description("Date")]
            ctDate = SqlDbType.Date,
            [Description("DateTime")]
            ctDateTime = SqlDbType.DateTime,
            [Description("DateTime2")]
            ctDateTime2 = SqlDbType.DateTime2,
            [Description("Float")]
            ctFloat = SqlDbType.Float,
            [Description("Int")]
            ctInt = SqlDbType.Int,
            [Description("NChar")]
            ctNChar = SqlDbType.NChar,
            [Description("NVarChar")]
            ctNVarChar = SqlDbType.NVarChar,
            [Description("SmallInt")]
            ctSmallInt = SqlDbType.SmallInt,
            [Description("TinyInt")]
            ctTinyInt = SqlDbType.TinyInt,
            [Description("UniqueIdentifier")]
            ctUniqueIdentifier = SqlDbType.UniqueIdentifier
        }

        public ColumnType ColTypeStringToEnum(string colType)
        {
            var ct = SQLHandler.ColumnType.ctUnknown;
            if (colType.Equals("String")) ct = SQLHandler.ColumnType.ctNVarChar;
            else if (colType.Equals("Byte")) ct = SQLHandler.ColumnType.ctTinyInt;
            else if (colType.Equals("Boolean")) ct = SQLHandler.ColumnType.ctBit;
            else if (colType.Equals("UInt16")) ct = SQLHandler.ColumnType.ctSmallInt;
            else if (colType.Equals("UInt32")) ct = SQLHandler.ColumnType.ctInt;
            else if (colType.Equals("UInt64")) ct = SQLHandler.ColumnType.ctBigInt;
            else if (colType.Equals("Int16")) ct = SQLHandler.ColumnType.ctSmallInt;
            else if (colType.Equals("Int32")) ct = SQLHandler.ColumnType.ctInt;
            else if (colType.Equals("Int64")) ct = SQLHandler.ColumnType.ctBigInt;
            else if (colType.Equals("DateTime")) ct = SQLHandler.ColumnType.ctDateTime2;
            else if (colType.Equals("Date")) ct = SQLHandler.ColumnType.ctDate;
            else if (colType.Equals("DateTimeOld")) ct = SQLHandler.ColumnType.ctDateTime;
            else if (colType.Equals("Single")) ct = SQLHandler.ColumnType.ctFloat;
            else if (colType.Equals("Double")) ct = SQLHandler.ColumnType.ctFloat;
            else if (colType.Equals("GUID")) ct = SQLHandler.ColumnType.ctUniqueIdentifier;
            else if (colType.Equals("Char")) ct = SQLHandler.ColumnType.ctNChar;
            return ct;
        }


        public enum CreateAddAlter
        {
            Create,
            Add,
            Alter
        }

        // ALTER TABLE table_name
        // ADD column_name column-definition;
        // ALTER TABLE employees
        //  ADD last_name VARCHAR(50),
        //        first_name VARCHAR(40);
        //ALTER TABLE table_name
        //  ALTER COLUMN column_name column_type;

        public bool PrepareColumn(string columnName, ColumnType columnType, int columnSize = -1)
        {
            bool success = false;
            if (!columnName.IsAlphaNum()) throw new ArgumentException($"invalid schema name: {columnName}");
            if (newTableParameters == null) throw new ArgumentException($"PrepareColumn before PrepareTble: {newTableName}");
            if (ColumnLength(newSchemaName, newTableName, columnName) != 0)
            {
                //throw new ArgumentException($"Column {columnName} already exists");
                success = false;
            }
            else
            {
                var datatype = columnType.ToString().Remove(0, 2);

                var sql = new StringBuilder();
                sql.Append(firstParameter ? " " : ", ");    // comma needed?
                sql.Append($" [{columnName}] ");
                sql.Append(datatype); // Utility.GetDescriptionFromEnumValue(columnType);
                if (ColumnType.ctNVarChar == columnType)
                {
                    var csize = columnSize < 1 ? 50 : columnSize > MaxStringLength ? MaxStringLength : columnSize;
                    sql.Append($"({csize})");
                }
                sql.Append(" null ");
                newTableParameters.AppendLine(sql.ToString());
                firstParameter = false;
                success = true;
            }
            return success;
        }

        // ALTER TABLE table_name
        // ADD column_name column-definition;
        // ALTER TABLE employees
        //  ADD last_name VARCHAR(50),
        //        first_name VARCHAR(40);
        //ALTER TABLE table_name
        //  ALTER COLUMN column_name column_type;

        public bool AddColumn(string schema, string tableName,
                        string columnName, ColumnType columnType, int columnSize = -1)
        {
            bool success = false;
            if (!columnName.IsAlphaNum()) throw new ArgumentException($"invalid schema name: {columnName}");
            if (newTableParameters != null) throw new ArgumentException($"AddColumn after PrepareTable: {newTableName}");
            if (ColumnLength(schema, tableName, columnName) != 0)
            {
                success = false;
                throw new ArgumentException($"Column {columnName} already exists");
            }
            else
            {
                var datatype = columnType.ToString().Remove(0, 2);

                var sql = new StringBuilder();
                sql.AppendLine($" ALTER TABLE [{schema}].[{tableName}] ");
                sql.Append(" ADD ");
                sql.Append($" [{columnName}] ");
                sql.Append(datatype); // Utility.GetDescriptionFromEnumValue(columnType);
                if (ColumnType.ctNVarChar == columnType)
                {
                    var csize = columnSize < 1 ? 50 : columnSize > MaxStringLength ? MaxStringLength : columnSize;
                    sql.Append($"({csize})");
                }
                sql.Append(" null ");
                var sqlCommand = sql.ToString();
                var result2 = DoCommand(sqlCommand);
                success = (result2 != 0);
            }
            return success;
        }

        // ALTER TABLE table_name
        // ADD column_name column-definition;
        // ALTER TABLE employees
        //  ADD last_name VARCHAR(50),
        //        first_name VARCHAR(40);
        //ALTER TABLE table_name
        //  ALTER COLUMN column_name column_type;

        public bool AlterColumn(string schema, string tableName,
                        string columnName, ColumnType columnType, int columnSize = -1)
        {
            bool success = false;
            if (!columnName.IsAlphaNum()) throw new ArgumentException($"invalid schema name: {columnName}");
            //if (newTableParameters == null) throw new ArgumentException($"PrepareColumn before PrepareTble: {newTableName}");
            if (ColumnLength(schema, tableName, columnName) == 0)
            {
                //throw new ArgumentException($"Column {columnName} does not exists");
                success = false;
            }
            else
            {
                var sql = new StringBuilder();
                sql.AppendLine($" ALTER TABLE [{schema}].[{tableName}] ");
                sql.Append(" ALTER COLUMN ");
                sql.Append($" [{columnName}] ");
                var datatype = columnType.ToString().Remove(0, 2);
                sql.Append(datatype); // Utility.GetDescriptionFromEnumValue(columnType);
                if (ColumnType.ctNVarChar == columnType)
                {
                    var csize = columnSize < 1 ? 50 : columnSize > MaxStringLength ? MaxStringLength : columnSize;
                    sql.Append($"({csize})");
                }
                if (!columnName.Equals("Name")) sql.Append(" null ");
                var sqlCommand = sql.ToString();
                var result2 = DoCommand(sqlCommand);
                success = (result2 != 0);
            }
            return success;
        }


        public bool CreateTable()
        {
            bool success = false;
            if (newTableCommand.Length == 0) throw new ArgumentException($"CreateTable without PrepareTable");
            newTableParameters.Append(" )");    // finish up create table command
            var result1 = DoCommand(newTableCommand + newTableParameters.ToString());
            var result2 = DoCommand(newIndexCommand);
            if (result1 != 0) success = true;
            AbandonNewTable();
            return success;
        }

        public void AbandonNewTable()
        {
            newTableName = "";
            newSchemaName = "";
            newTableCommand = "";
            newTableParameters = null;
            firstParameter = true;
            newIndexCommand = "";
        }

    }
}
