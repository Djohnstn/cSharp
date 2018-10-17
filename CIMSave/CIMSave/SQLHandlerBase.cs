using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Data;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CIMSave
{
    public static class SQLDataRowExtensions
    {
        public static void MaybeAddRow(this DataTable fileDt, DataRow myRow)
        {
            // save previous row
            //if (myRow != null)
            if (myRow?.RowState == DataRowState.Detached)
            {
                myRow.EndEdit();
                fileDt.Rows.Add(myRow);
            }
        }

        // https://code.msdn.microsoft.com/Stored-Procedure-with-6c194514
        /// <summary> 
        /// Creates data table from source data. 
        /// </summary> 
        private static DataTable ToDataTable_Reflection<T>(this IEnumerable<T> source)
        {
            DataTable table = new DataTable();

            //// get properties of T 
            //var binding = BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty;
            //var options = PropertyReflectionOptions.IgnoreEnumerable | PropertyReflectionOptions.IgnoreIndexer;

            //var properties = ReflectionExtensions.GetProperties<T>(binding, options).ToList();

            //// create table schema based on properties 
            //foreach (var property in properties)
            //{
            //    table.Columns.Add(property.Name, property.PropertyType);
            //}

            ////// create table data from T instances 
            //object[] values = new object[properties.Count];

            //foreach (T item in source)
            //{
            //    for (int i = 0; i < properties.Count; i++)
            //    {
            //        values[i] = properties[i].GetValue(item, null);
            //    }

            //    table.Rows.Add(values);
            //}

            return table;
        }

        //https://stackoverflow.com/questions/5595353/how-to-pass-table-value-parameters-to-stored-procedure-from-net-code
        public static DataTable ToDataTable<T>(this IEnumerable<T> source, string columnName)
        {
            //private static DataTable CreateDataTable(IEnumerable<long> ids)
            //{
                DataTable table = new DataTable();
                table.Columns.Add(columnName, typeof(T));
                foreach (T id in source)
                {
                    table.Rows.Add(id);
                }
                return table;
            //}
        }

    }

    public class SQLHandlerBase
    {
        private static string _connectionString;
        private static string _schema;
        private static string _tableprefix;

        public SQLHandlerBase()
        {
            if (_connectionString == null || _connectionString.Length == 0)
            {
                _connectionString = CommandlineParameters.Value("database", "");
                if (_connectionString?.Length < 1)
                {
                    var conxStrings = System.Configuration.ConfigurationManager.ConnectionStrings;
                    _connectionString = conxStrings["database"].ConnectionString;
                }
            }
            if (_schema == null || _schema.Length == 0 || 
                _tableprefix == null || _tableprefix.Length == 0 )
            {

                _schema = CommandlineParameters.Value("schema","dbo").Trim();
                _tableprefix = CommandlineParameters.Value("TablePrefix","CIM_").Trim();
                if (_schema?.Length < 1) {
                    var settings = System.Configuration.ConfigurationManager.AppSettings;
                    //_connectionString = settings["database"].Trim();
                    _schema = settings["schema"].Trim();
                }
                if (_tableprefix?.Length <1)
                {
                    var settings = System.Configuration.ConfigurationManager.AppSettings;
                    _tableprefix = settings["TablePrefix"].Trim();
                }

            }
        }


        public string ConnectionString => _connectionString;
        public string Schema => _schema;
        public string TablePrefix => _tableprefix;

        public T DoQuery<T>(string sqlQuery, T defaultValue, string paramName, string paramValue)
        {
            var p = new List<SqlParameter>
            {
                new SqlParameter($"@{paramName}", paramValue)
            };
            return DoQuery<T>(sqlQuery, defaultValue, p);
        }

        public T DoQuery<T>(string sqlQuery, T defaultValue, List<SqlParameter> paramList)
        {
            T result = defaultValue;
            using (SqlConnection con = new SqlConnection(ConnectionString))
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
                        while (reader.Read())
                        {
                            result = reader.GetFieldValue<T>(0);
                        }
                    }
                }
            }
            return result;
        }


        public T DoQuery<T>(string sqlQuery, T defaultValue)
        {
            T result = defaultValue;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(sqlQuery, con))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = reader.GetFieldValue<T>(0);
                    }

                }
            }
            return result;
        }


        public T2 DoQueryScaler<T0, T1, T2>(string sqlQuery, T2 defaultValue, 
                                            string paramName0, T0 paramValue0,
                                            string paramName, T1 paramValue)
        {
            var p = new List<SqlParameter>
            {
                new SqlParameter($"@{paramName0}", paramValue0),
                new SqlParameter($"@{paramName}", paramValue)
            };
            return DoQueryScaler<T2>(sqlQuery, defaultValue, p);
        }

        public T2 DoQueryScaler<T1, T2>(string sqlQuery, T2 defaultValue, string paramName, T1 paramValue)
        {
            var p = new List<SqlParameter>
            {
                new SqlParameter($"@{paramName}", paramValue)
            };
            return DoQueryScaler<T2>(sqlQuery, defaultValue, p);
        }

        public T DoQueryScaler<T>(string sqlQuery, T defaultValue, List<SqlParameter> paramList)
        {
            T result = defaultValue;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(sqlQuery, con))
                {
                    foreach (var p in paramList)
                    {
                        command.Parameters.Add(p);
                    }
                    object scaler = command.ExecuteScalar();
                    if (scaler == null)
                    {
                        result = defaultValue;
                    }
                    else if (scaler == System.DBNull.Value) //(scaler != null)
                    {
                        result = defaultValue;
                    }
                    else
                    {
                        result = (T)scaler;
                    }
                }
            }
            return result;
        }


        public T DoQueryScaler<T>(string sqlQuery, T defaultValue)
        {
            T result = defaultValue;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(sqlQuery, con))
                {
                    var scaler = command.ExecuteScalar();
                    if (scaler != null)
                    {
                        result = (T)scaler; 
                    }
                }
            }
            return result;
        }

        public int DoQueryNonScaler(string sqlQuery)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(sqlQuery, con))
                {
                    result = command.ExecuteNonQuery();
                }
            }
            return result;
        }


    }

    public class DBMapper<T1, T2>
    {
        // keep up name to ID translation, no need to go back to SQL for this batch run.
        static ConcurrentDictionary<T1, T2> xIDs = new ConcurrentDictionary<T1, T2>();
        static readonly string[] xquerys = new string[]
        {           // try these queries to see if can find the named entity id, or create a named entity id
            "Select id from [%SCHEMA%].[%TABLEPREFIX%%TABLE%] Where %NAMEPARAMETER% = @%NAMEPARAMETER%",
            "Insert into [%SCHEMA%].[%TABLEPREFIX%%TABLE%] (%NAMEPARAMETER%) OUTPUT Inserted.ID Values(@%NAMEPARAMETER%)",
            "Select id from [%SCHEMA%].[%TABLEPREFIX%%TABLE%] Where %NAMEPARAMETER% = @%NAMEPARAMETER%"
        };

        private SQLHandlerBase sqlbase;
        private readonly List<string> InstanceQueries;
        private readonly string NameParameter;
        private readonly T2 DefaultValue;

        public DBMapper(string tablename, T2 defaultvalue, string nameparameter = "name")
        {
            sqlbase = new SQLHandlerBase();
            NameParameter = nameparameter;
            this.DefaultValue = defaultvalue;
            InstanceQueries = (from q in xquerys select Clean(q, tablename, nameparameter)).ToList();
        }
        private string Clean(string cleanthis, string table, string nameparameter)
        {
                return new StringBuilder(cleanthis)
                      .Replace("%SCHEMA%", sqlbase.Schema )
                      .Replace("%TABLEPREFIX%", sqlbase.TablePrefix)
                      .Replace("%TABLE%", table)
                      .Replace("%NAMEPARAMETER%", nameparameter)
                      .ToString();
        }

        public T2 ID(T1 Name)
        {
            if (xIDs.TryGetValue(Name, out T2 idCache))
            {
                return idCache;
            }
            var sqlbase = new SQLHandlerBase();
            foreach (var query in InstanceQueries)
            {
                T2 id = sqlbase.DoQueryScaler<T1,T2>(query, DefaultValue, NameParameter, Name);
                if (id != null && !id.Equals(DefaultValue))
                {
                    xIDs.TryAdd(Name, id); // save value to id for next round of lookups, then return it
                    return id;
                }
            }
            return DefaultValue;
        }
    }

    public class DBMapper2<T0, T1, T2>
    {
        // keep up name to ID translation, no need to go back to SQL for this batch run.
        static ConcurrentDictionary<string, T2> xIDs = new ConcurrentDictionary<string, T2>();
        static readonly string[] xquerys = new string[]
        {           // try these queries to see if can find the named entity id, or create a named entity id
            "Select id from [%SCHEMA%].[%TABLEPREFIX%%TABLE%] Where %NAMEPARAMETER% = @%NAMEPARAMETER% And %NAMEPARAMETER0% = @%NAMEPARAMETER0%",
            "Insert into [%SCHEMA%].[%TABLEPREFIX%%TABLE%] (%NAMEPARAMETER0%,%NAMEPARAMETER%) OUTPUT Inserted.ID Values(@%NAMEPARAMETER0%, @%NAMEPARAMETER%)",
            "Select id from [%SCHEMA%].[%TABLEPREFIX%%TABLE%] Where %NAMEPARAMETER% = @%NAMEPARAMETER% And %NAMEPARAMETER0% = @%NAMEPARAMETER0%",
        };

        private SQLHandlerBase sqlbase;
        private readonly List<string> InstanceQueries;
        private readonly string NameParameter0;
        private readonly string NameParameter;
        private readonly T2 DefaultValue;

        //private bool IndexOnParameter = false;

        public DBMapper2(string tablename, T2 defaultvalue, string nameparameter0 = "name0", string nameparameter = "name")
        {
            sqlbase = new SQLHandlerBase();
            NameParameter0 = nameparameter0;
            NameParameter = nameparameter;
            this.DefaultValue = defaultvalue;
            InstanceQueries = (from q in xquerys select Clean(q, tablename, nameparameter0, nameparameter)).ToList();
        }

        private string Clean(string cleanthis, string table, string nameparameter0, string nameparameter)
        {
            return new StringBuilder(cleanthis)
                  .Replace("%SCHEMA%", sqlbase.Schema)
                  .Replace("%TABLEPREFIX%", sqlbase.TablePrefix)
                  .Replace("%TABLE%", table)
                  .Replace("%NAMEPARAMETER0%", nameparameter0)
                  .Replace("%NAMEPARAMETER%", nameparameter)
                  .ToString();
        }

        const char delim = (char)31; // tab character = 9   // unit separator = 31
        public T2 ID(T0 Name0, T1 Name)
        {
            var compoundName = $"{Name0}{delim}{Name}";     // different type of KeyPair dictionary
            if (xIDs.TryGetValue(compoundName, out T2 idCache))
            {
                return idCache;
            }
            else
            {
                var sqlbase = new SQLHandlerBase();
                foreach (var query in InstanceQueries)
                {
                    T2 id = sqlbase.DoQueryScaler<T0, T1, T2>(query, DefaultValue,
                                                                NameParameter0, Name0,
                                                                NameParameter, Name);
                    if (id != null && !id.Equals(DefaultValue))
                    {
                        xIDs.TryAdd(compoundName, id); // save value to id for next round of lookups, then return it
                        return id;
                    }
                }
            }
            return DefaultValue;
        }
    }


    public class DBTableMaker
    {
        //static readonly string SampleTable =
        //    @"CREATE TABLE [dbo].[CIM_%TABLE%] (
        //        [Id]          INT IDENTITY (1, 1) NOT NULL,
        //        [%PRIME%]     %TYPE%           NOT NULL,
        //        [Description] NVARCHAR (150) NULL,
        //        PRIMARY KEY CLUSTERED ([Id] ASC));";
        //static readonly string SampleIndex =
        //@"CREATE UNIQUE NONCLUSTERED INDEX [ix%TABLE%_%PRIME%] ON [dbo].[CIM_%TABLE%]([%PRIME%] ASC);";

        //private string NewTable = string.Empty;
        //private string NewIndex = string.Empty;
        private readonly string tableName;

        public DBTableMaker()
        {
        }
        public DBTableMaker(string table)
        {
            var b = new SQLHandlerBase();
            this.tableName =  $"[{b.Schema}].[{b.TablePrefix}_{table}]" ;
            ForTable();
        }

        public bool ValidateTable()
        {
            return true;
        }


        //public bool ValidateTable(string tableName, string valueName, string valueType)
        //{
        //    bool result = true;
        //    NewTable = Clean(SampleTable, tableName, valueName, valueType);
        //    NewIndex = Clean(SampleIndex, tableName, valueName, valueType);
        //    var table = Clean("[dbo].[CIM_%TABLE%]", tableName, valueName, valueType);
        //    var index = Clean("[ix%TABLE%_%PRIME%]", tableName, valueName, valueType);
        //    var tableOk = IfExists(table);
        //    if (!tableOk)
        //    {
        //        var handlerBase = new SQLHandlerBase();
        //        var newTable = handlerBase.DoQueryNonScaler(NewTable);
        //        //result = false;
        //    }
        //    var indexOk = IfIndexExists(table, index);
        //    if (!indexOk)
        //    {
        //        var handlerBase = new SQLHandlerBase();
        //        var newIndex = handlerBase.DoQueryNonScaler(NewIndex);
        //    }
        //    return result;
        //}

        public bool IfExists(string objectname)
        {
            //const string IfObjectExists = "select IIF(Object_id('%objectname%') is null, 0, 1)";
            //var qry = IfObjectExists.Replace("%objectname%", objectname);
            bool result = false;
            var sql = $"select IIF(Object_id('{objectname}') is null, 0, 1)";
            var handlerBase = new SQLHandlerBase();
            var rc = handlerBase.DoQueryScaler<int>(sql, -2);
            if (rc == 1) result = true;
            return result;
        }

        //"Select IIF( Object_id('%objectname%') is null, 0, 1)";

        // IndexProperty(Object_Id('MyTable'), 'MyIndex', 'IndexId') 

        private bool IfIndexExists(string objectname, string objectname2)
        {
            //const string IfObjectIndexExists =
            //"Select IIF(IndexProperty(Object_Id('%object%'), '%index%', 'IndexId') is null, 0, 1)";
            //var qry = IfObjectIndexExists
            //            .Replace("%object%", objectname)
            //            .Replace("%index%", objectname2);
            bool result = false;
            var sql = $"Select IIF(IndexProperty(Object_Id('{objectname}'), '{objectname2}', 'IndexId') is null, 0, 1)";
            var handlerBase = new SQLHandlerBase();
            int rc = handlerBase.DoQueryScaler<int>(sql, -2);
            if (rc > 0) result = true;
            return result;
        }

        // COL_LENGTH ( 'table' , 'column' ) 
        private bool IfColumnExists(string tablename, string columnname)
        {
            //const string IfObjectIndexExists =
            //"Select Coalesce(COL_LENGTH('%object%', '%column%'), -1)";
            //var qry = IfObjectIndexExists
            //            .Replace("%object%", tablename)
            //            .Replace("%column%", columnname);
            bool result = false;
            var sql = $"Select Coalesce(COL_LENGTH('{tablename}', '{columnname}'), -1)";
            var handlerBase = new SQLHandlerBase();
            var rc = handlerBase.DoQueryScaler<int>(sql, -2);
            if (rc > -1) result = true;
            return result;
        }

        public bool IfTypeExists(string objectname)
        {
            //const string IfObjectExists = "select IIF(Object_id('%objectname%') is null, 0, 1)";
            //var qry = IfObjectExists.Replace("%objectname%", objectname);
            bool result = false;
            var sql = $"select IIF(Type_id('{objectname}') is null, 0, 1)";
            var handlerBase = new SQLHandlerBase();
            var rc = handlerBase.DoQueryScaler<int>(sql, -2);
            if (rc == 1) result = true;
            return result;
        }

        //private string Clean(string cleanthis, string table, string nameparameter, string datatype)
        //{
        //    return new StringBuilder(cleanthis)
        //          .Replace("%TABLE%", table)
        //          .Replace("%PRIME%", nameparameter)
        //          .Replace("%TYPE%", datatype)
        //          .ToString();
        //}

        public void ForTable()
        {

            if (!IfExists(tableName))
            {
                var sql = $@"CREATE TABLE {tableName} ([Id] INT IDENTITY (1, 1) NOT NULL, PRIMARY KEY CLUSTERED ([Id] ASC));";
                var handlerBase = new SQLHandlerBase();
                handlerBase.DoQueryNonScaler(sql);
            }
        }

        public void ForColumn(string columnName, string datatype, bool nullable, bool indexed)
        {
            if (!IfColumnExists(tableName, columnName)) 
            {
                var nullvalue = nullable ? "NULL" : "";
                var sql = $"Alter Table {tableName} Add {columnName} {datatype} {nullvalue}";
                var handlerBase = new SQLHandlerBase();
                handlerBase.DoQueryNonScaler(sql);
                if (indexed)
                {
                    ForIndex(columnName);
                }
            }
        }

        public void ForIndex(string indexColumn)
        {
            var ixName = ("ix" + tableName + '_' + indexColumn).Replace('[', '_').Replace(']', '_');
            //if (!IfIndexExists(tableName, indexColumn))
            if (!IfIndexExists(tableName, ixName))
            {
                //var sql = $"CREATE UNIQUE NONCLUSTERED INDEX [ix{tableName}_{indexColumn}] " +
                //           $" ON [dbo].[CIM_{tableName}]([{indexColumn}] ASC);";
                var sql = $"CREATE UNIQUE NONCLUSTERED INDEX [{ixName}] " + 
                           $" ON {tableName} ({indexColumn} ASC);";
                var handlerBase = new SQLHandlerBase();
                handlerBase.DoQueryNonScaler(sql);
            }
        }
    }

}
