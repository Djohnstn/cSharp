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
            var nn = IS_NULLABLE ? "" : " NOT NULL";
            return $"{TABLE_CATALOG}.{TABLE_SCHEMA}.{TABLE_NAME}.{COLUMN_NAME} {DATA_TYPE}({CHARACTER_MAXIMUM_LENGTH}{NUMERIC_PRECISION}){nn};";
        }
    }


    class MsSqlInventory
    {

        List<string> sqlinstances;
        List<string> dblist  = new List<string>();
        List<string> tblist  = new List<string>();
        List<MsSqlTableColumns> columns = new List<MsSqlTableColumns>();

        RegistryView registryView = Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32;

        public IEnumerable<string> GetSqlInstanceNames()
        {
            using (RegistryKey hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView))
            {
                RegistryKey instanceKey = hklm.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL", false);
                if (instanceKey != null)
                {
                    foreach (var instanceName in instanceKey.GetValueNames())
                    {
                        yield return (Environment.MachineName + @"\" + instanceName);
                    }
                }
            }
        }

        public IEnumerable<string> GetSqlDatabases(string instance)
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
                        yield return (instance + "." + reader[0].ToString());
                    }
                }
            }
        }

        public IEnumerable<string> GetSqlDatabaseTables(string database)
        {
            var db = database.Split('.');
            var connection = $"server={db[0]}; database={db[1]}; Integrated Security=SSPI;";
            using (var con = new SqlConnection(connection))
            {
                con.Open();
                DataTable schema = con.GetSchema("Tables");
                //List<string> TableNames = new List<string>();
                foreach (DataRow row in schema.Rows)
                {
                    yield return $"{db[0]}.{row[0]}.{row[1]}.{row[2]}\t{row[3]}";
                }
            }
        }


        public IEnumerable<MsSqlTableColumns> GetSqlTableColumns(string database)
        {
            const string query = @"SELECT TABLE_CATALOG, TABLE_SCHEMA, TABLE_NAME, COLUMN_NAME, ORDINAL_POSITION, 
                                     COLUMN_DEFAULT, IS_NULLABLE, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, 
                                     NUMERIC_PRECISION, NUMERIC_PRECISION_RADIX, NUMERIC_SCALE,
                                     DATETIME_PRECISION, 
                                     CHARACTER_SET_CATALOG, CHARACTER_SET_SCHEMA, CHARACTER_SET_NAME,
                                     COLLATION_CATALOG, COLLATION_SCHEMA, COLLATION_NAME
                                FROM INFORMATION_SCHEMA.COLUMNS;";
            var db = database.Split('.');
            var connection = $"server={db[0]}; database={db[1]}; Integrated Security=SSPI;";
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

        class MsSqlStoredProcedure
        {

        }

        public IEnumerable<MsSqlTableColumns> GetSqlSoredProcedures(string database)
        {
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
            sqlinstances = GetSqlInstanceNames().ToList<string>();
            sqlinstances.ForEach(d => { dblist.AddRange(GetSqlDatabases(d).ToList<string>()); });
            dblist.ForEach(d => { tblist.AddRange(GetSqlDatabaseTables(d).ToList<string>()); });
            dblist.ForEach(d => { columns.AddRange(GetSqlTableColumns(d).ToList<MsSqlTableColumns>()); });


            var ix = 0;
        }

    }
}
