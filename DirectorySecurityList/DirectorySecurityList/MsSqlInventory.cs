using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIMSave;
using Microsoft.Win32;


namespace DirectorySecurityList
{
    public static class SQLDataReaderExtensions
    {
        public static string SafeGetString(this SqlDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
                return reader.GetString(colIndex);
            return null;
        }
        public static bool SafeGetBool(this SqlDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
            {
                string value = reader[colIndex].ToString().ToUpper();
                if (value.Length > 0)
                {
                    string v = value.Substring(0, 1);
                    //if ("NF0".Contains(v)) return false;    // No False 0 (Zero)
                    if ("YT1".Contains(v)) return true;     // Yes True 1 (One) 
                }
            }
            return false;
        }
        public static int? SafeGetInt32(this SqlDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
            {
                //var objX = typeof(reader[colIndex]);
                var foo = reader[colIndex].ToString();
                //var dotNetType = reader.GetFieldType(colIndex);
                //var sqlType = reader.GetDataTypeName(colIndex);
                Type specificType = reader.GetProviderSpecificFieldType(colIndex);
                if (specificType == typeof(System.Data.SqlTypes.SqlByte)) return (int)reader.GetByte(colIndex);
                if (specificType == typeof(System.Data.SqlTypes.SqlInt16)) return (int)reader.GetInt16(colIndex);
                if (specificType == typeof(System.Data.SqlTypes.SqlInt32)) return (int)reader.GetInt32(colIndex);
                return reader.GetInt32(colIndex);
            }
            return null;
        }
    }

    public class MsSqlTableColumns
    {
        public string Server { get; set; }
        public string Name { get; set; } // instance.catalog.schema.table.column

        public string TABLE_CATALOG { get; set; }
        public string TABLE_SCHEMA { get; set; }
        public string TABLE_NAME { get; set; }
        public string COLUMN_NAME { get; set; }
        public int ORDINAL_POSITION { get; set; }
        public string COLUMN_DEFAULT { get; set; }
        public bool IS_NULLABLE { get; set; }
        public string DATA_TYPE { get; set; }
        public int? CHARACTER_MAXIMUM_LENGTH { get; set; }
        public int? NUMERIC_PRECISION { get; set; }
        public int? NUMERIC_PRECISION_RADIX { get; set; }
        public int? NUMERIC_SCALE { get; set; }
        public int? DATETIME_PRECISION { get; set; }
        public string CHARACTER_SET_CATALOG { get; set; }
        public string CHARACTER_SET_SCHEMA { get; set; }
        public string CHARACTER_SET_NAME { get; set; }
        public string COLLATION_CATALOG { get; set; }
        public string COLLATION_SCHEMA { get; set; }
        public string COLLATION_NAME { get; set; }
        public override string ToString()
        {
            //var nn = IS_NULLABLE ? "" : " NOT NULL";
            //return $"{TABLE_CATALOG}.{TABLE_SCHEMA}.{TABLE_NAME}.{COLUMN_NAME} {DATA_TYPE}({CHARACTER_MAXIMUM_LENGTH}{NUMERIC_PRECISION}){nn};";
            return $"{Name} {DATA_TYPE}({CHARACTER_MAXIMUM_LENGTH}{NUMERIC_PRECISION});";
        }
    }

    public class MsSqlServers
    {
        public string Server { get; set; }
        public string Name { get; set; } // server\instance
        public override string ToString()
        {
            return $"{Server} {Name};";
        }
    }

    public class MsSqlDatabases
    {
        public string Server { get; set; }
        public string Name { get; set; } // instance.catalog

        public string Instance { get; set; }
        public string Database { get; set; }

        public override string ToString()
        {
            return $"{Server} {Name};";
        }
    }

    public class MsSqlTables
    {
        public string Server { get; set; }
        public string Name { get; set; } // instance.catalog.schema.table

        public string Instance { get; set; }
        public string Database { get; set; }
        public string Table { get; set; }
        public int? RecordCount { get; set; }
        public string Status { get; set; }

        public override string ToString()
        {
            return $"{Server} {Name} = {RecordCount}; {Status}";
        }
    }

    public class MsSqlStoredProcedure
    {
        public string Server { get; set; }
        public string Name { get; set; } // instance.catalog.schema.procedure

        public string Instance { get; set; }
        public string Database { get; set; }
        public string Procedure { get; set; }
        public string Hash { get; set; }
        public string Status { get; set; }

        public override string ToString()
        {
            return $"{Server} {Name} {Procedure} = {Hash}; {Status}";
        }
    }

    class MsSqlInventory
    {

        List<MsSqlServers> sqlinstances;
        List<MsSqlDatabases> dblist  = new List<MsSqlDatabases>();
        List<MsSqlTables> tblist  = new List<MsSqlTables>();
        List<MsSqlTableColumns> columns = new List<MsSqlTableColumns>();
        List<MsSqlStoredProcedure> procedures = new List<MsSqlStoredProcedure>();

        RegistryView registryView = Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32;

        public IEnumerable<MsSqlServers> GetSqlInstanceNames()
        {
            var machineName = Environment.MachineName;
            using (RegistryKey hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView))
            {
                RegistryKey instanceKey = hklm.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL", false);
                if (instanceKey != null)
                {
                    foreach (var instanceName in instanceKey.GetValueNames())
                    {
                        var value = new MsSqlServers()
                        {
                            Server = machineName,
                            Name = $@"{machineName}\{instanceName}"
                        };
                        yield return (value); // (machineName + @"\" + instanceName);
                    }
                }
            }
        }

        public IEnumerable<MsSqlDatabases> GetSqlDatabases(string server, string instance)
        {
            var connection = $"server={instance}; database=master; Integrated Security=SSPI;";
            using (var con = new SqlConnection(connection))
            {
                con.Open();
                //using (var cmd = new SqlCommand("Select Name from Sys.Databases;", con))
                // WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb'); "ReportServer" and "ReportServerTempDB
                // WHERE  dbid > 4; Le(owner_sid) > 1
                string query = "SELECT * FROM SYS.DATABASES WHERE database_id > 4 AND Name NOT IN ('ReportServer', 'ReportServerTempDB');";
                using (var cmd = new SqlCommand(query, con))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var result = new MsSqlDatabases()
                        {
                            Server = server,
                            Instance = instance,
                            Database = reader[0].ToString()
                        };
                        result.Name = $"{instance}.{result.Database}";
                        yield return (result); // (instance + "." + reader[0].ToString());
                    }
                }
            }
        }

        public int? SqlRecordCount(SqlConnection con, string database, string schema, string table)
        {
            int? count = null;
            string query = $"SELECT COUNT(*) FROM {database}.{schema}.{table};";

            try
            {
                using (var cmd = new SqlCommand(query, con))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        count = reader.GetInt32(0);
                    }
                }
            }
            catch (Exception)
            {
                count = -1;
                //throw;
            }

            return (count); 
        }

        public List<MsSqlTables> GetSqlDatabaseTables(string server, string instance, string database)
        {
            //var db = database.Split('.');
            var tables = new List<MsSqlTables>();
            string tbschema = string.Empty;
            string table = string.Empty;
            var connection = $"server={instance}; database={database}; Integrated Security=SSPI;";

            try
            {
                using (var con = new SqlConnection(connection))
                {
                    con.Open();
                    DataTable schema = con.GetSchema("Tables");
                    //List<string> TableNames = new List<string>();
                    foreach (DataRow row in schema.Rows)
                    {
                        // row(0) = database
                        // row(1) = schema
                        tbschema = row[1].ToString();
                        // row(2) = table name
                        table = row[2].ToString();
                        // row(3) = table type 

                        var result = new MsSqlTables()
                        {
                            Server = server,
                            Name = $"{instance}.{database}.{tbschema}.{table}",
                            Instance = instance,
                            Database = database,
                            //Table = $"{db[0]}.{row[0]}.{row[1]}.{row[2]}:{row[3]}"
                            Table = $"{tbschema}.{table}",
                            RecordCount = SqlRecordCount(con, database, tbschema, table)
                        };
                        tables.Add(result);
                    }
                }
            }
            catch (Exception ex)
            {
                var exresult = new MsSqlTables()
                {
                    Server = server,
                    Name = $"{instance}.{database}.{tbschema}.{tbschema}.{table}.$Error",
                    Instance = instance,
                    Database = database,
                    Table = $"{tbschema}.{table}",
                    Status = ex.Message
                };
                tables.Add(exresult);
            }
            return (tables);// $"{db[0]}.{row[0]}.{row[1]}.{row[2]}\t{row[3]}";
        }


        public IEnumerable<MsSqlTableColumns> GetSqlTableColumns(string server, string instance, string database)
        {
            const string query = @"SELECT TABLE_CATALOG, TABLE_SCHEMA, TABLE_NAME, COLUMN_NAME, ORDINAL_POSITION, 
                                     COLUMN_DEFAULT, IS_NULLABLE, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, 
                                     NUMERIC_PRECISION, NUMERIC_PRECISION_RADIX, NUMERIC_SCALE,
                                     DATETIME_PRECISION, 
                                     CHARACTER_SET_CATALOG, CHARACTER_SET_SCHEMA, CHARACTER_SET_NAME,
                                     COLLATION_CATALOG, COLLATION_SCHEMA, COLLATION_NAME
                                FROM INFORMATION_SCHEMA.COLUMNS;";
            //var db = database.Split('.');
            var connection = $"server={instance}; database={database}; Integrated Security=SSPI;";
            using (var con = new SqlConnection(connection))
            {
                con.Open();
                //DataTable schema = con.GetSchema("Tables");
                //List<string> TableNames = new List<string>();
                //string query = "select * from sys.databases where len(owner_sid) > 1;";
                using (var cmd = new SqlCommand(query, con))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var r = new MsSqlTableColumns()
                        {
                            TABLE_CATALOG = reader.SafeGetString(0),
                            TABLE_SCHEMA = reader.SafeGetString(1),
                            TABLE_NAME = reader.SafeGetString(2),
                            COLUMN_NAME = reader.SafeGetString(3),
                            ORDINAL_POSITION = reader.GetInt32(4),
                            COLUMN_DEFAULT = reader.SafeGetString(5),
                            IS_NULLABLE = reader.SafeGetBool(6),
                            DATA_TYPE = reader.SafeGetString(7),
                            CHARACTER_MAXIMUM_LENGTH = reader.SafeGetInt32(8),
                            NUMERIC_PRECISION = reader.SafeGetInt32(9),
                            NUMERIC_PRECISION_RADIX = reader.SafeGetInt32(10),
                            NUMERIC_SCALE = reader.SafeGetInt32(11),
                            DATETIME_PRECISION = reader.SafeGetInt32(12),
                            CHARACTER_SET_CATALOG = reader.SafeGetString(13),
                            CHARACTER_SET_SCHEMA = reader.SafeGetString(14),
                            CHARACTER_SET_NAME = reader.SafeGetString(15),
                            COLLATION_CATALOG = reader.SafeGetString(16),
                            COLLATION_SCHEMA = reader.SafeGetString(17),
                            COLLATION_NAME = reader.SafeGetString(18)
                        };

                        r.Server = server;
                        r.Name = $"{instance}.{r.TABLE_CATALOG}.{r.TABLE_SCHEMA}.{r.TABLE_NAME}.{r.COLUMN_NAME}";

                        yield return (r);
                    }
                }
            }
        }

        public IEnumerable<MsSqlTableColumns> GetSqlStoredProcedures(string server, string instance, string database)
        {

            const string pquery = @"
Select o.name, o.crdate, o.refdate, c.colid, c.text
from SysObjects o
Inner join SysComments c on c.id = o.id
Where o.xtype = 'P'
order by o.name, c.colid
";

            const string query = @"SELECT DISTINCT
	               SCHEMA_NAME(o.schema_id) + '.' + o.name AS Object_Name,
                   o.type_desc,
	               o.create_date,
	               o.is_ms_shipped,
	               o.modify_date,
	               o.principal_id,
	               LEN(m.definition),
	               m.definition
              FROM sys.sql_modules m
              INNER JOIN sys.objects o ON m.object_id = o.object_id";
            var db = database.Split('.');
            var connection = $"server={db[0]}; database={db[1]}; Integrated Security=SSPI;";
            using (var con = new SqlConnection(connection))
            {
                con.Open();
                using (var cmd = new SqlCommand(query, con))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return (new MsSqlTableColumns()
                        {
                            TABLE_CATALOG = reader.SafeGetString(0),
                            TABLE_SCHEMA = reader.SafeGetString(1),
                            TABLE_NAME = reader.SafeGetString(2),
                            COLUMN_NAME = reader.SafeGetString(3),
                            ORDINAL_POSITION = reader.GetInt32(4),
                            COLUMN_DEFAULT = reader.SafeGetString(5),
                            IS_NULLABLE = reader.SafeGetBool(6),
                            DATA_TYPE = reader.SafeGetString(7),
                            CHARACTER_MAXIMUM_LENGTH = reader.SafeGetInt32(8),
                            NUMERIC_PRECISION = reader.SafeGetInt32(9),
                            NUMERIC_PRECISION_RADIX = reader.SafeGetInt32(10),
                            NUMERIC_SCALE = reader.SafeGetInt32(11),
                            DATETIME_PRECISION = reader.SafeGetInt32(12),
                            CHARACTER_SET_CATALOG = reader.SafeGetString(13),
                            CHARACTER_SET_SCHEMA = reader.SafeGetString(14),
                            CHARACTER_SET_NAME = reader.SafeGetString(15),
                            COLLATION_CATALOG = reader.SafeGetString(16),
                            COLLATION_SCHEMA = reader.SafeGetString(17),
                            COLLATION_NAME = reader.SafeGetString(18)
                        });
                    }
                }
            }
        }


        // https://stackoverflow.com/questions/1054984/how-can-i-get-column-names-from-a-table-in-sql-server

        public void Inventory()
        {
            sqlinstances = GetSqlInstanceNames().ToList<MsSqlServers>();
            sqlinstances.ForEach(d => { dblist.AddRange(GetSqlDatabases(d.Server, d.Name).ToList<MsSqlDatabases>()); });
            //dblist.ForEach(d => { tblist.AddRange(GetSqlDatabaseTables(d.Server, d.Instance, d.Database).ToList<MsSqlTables>()); });
            dblist.ForEach(d => { tblist.AddRange(GetSqlDatabaseTables(d.Server, d.Instance, d.Database)); });
            dblist.ForEach(d => { columns.AddRange(GetSqlTableColumns(d.Server, d.Instance, d.Database).ToList<MsSqlTableColumns>()); });

            //dblist.ForEach(d => { procedures.AddRange(GetSqlStoredProcedures(d.Server, d.Instance, d.Database).ToList<MsSqlTableColumns>()); });


            var ix = 0;
        }

    }
}
