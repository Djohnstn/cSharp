using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CIMCollect;
using Microsoft.Win32;
using Similarity;


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
        public string Instance { get; set; }    // instance
        public string Path { get; set; }        // database
        public string Name { get; set; } // schema.table.column : dbo.rules.columnName 

        public string TABLE_CATALOG { get; set; }   // database
        public string TABLE_SCHEMA { get; set; }    // schema
        public string TABLE_NAME { get; set; }      // table
        public string COLUMN_NAME { get; set; }     // column
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
        public static Int32 ToJsonFile(List<MsSqlTableColumns> tableColumns, DateTime inventoryTime, string jsonFolder)
        {
            var className = MethodBase.GetCurrentMethod().DeclaringType.Name;
            int ix = 0;
            if (tableColumns.Count < 1) return ix;
            var server = (from sv in tableColumns select sv).FirstOrDefault();
            InfoParts p = new InfoParts(server.Server, className, inventoryTime);
            foreach (var s in tableColumns)
            {
                ix++;
                //p.Add(s.Instance, s.Path, s.Name, ix, "ORDINAL_POSITION", $"${s.Version.Length}", s.ORDINAL_POSITION);
                p.Add(s.Instance, s.Path, s.Name, ix, "ORDINAL_POSITION", "Int32", s.ORDINAL_POSITION.ToString());
                p.Add(s.Instance, s.Path, s.Name, ix, "COLUMN_DEFAULT", "String", s.COLUMN_DEFAULT);
                p.Add(s.Instance, s.Path, s.Name, ix, "IS_NULLABLE", "Boolean", s.IS_NULLABLE.ToString());
                p.Add(s.Instance, s.Path, s.Name, ix, "DATA_TYPE", "String", s.DATA_TYPE);
                p.Add(s.Instance, s.Path, s.Name, ix, "CHARACTER_MAXIMUM_LENGTH", "Int32", s.CHARACTER_MAXIMUM_LENGTH?.ToString());
                p.Add(s.Instance, s.Path, s.Name, ix, "NUMERIC_PRECISION", "Int32", s.NUMERIC_PRECISION?.ToString());
                p.Add(s.Instance, s.Path, s.Name, ix, "NUMERIC_PRECISION_RADIX", "Int32", s.NUMERIC_PRECISION_RADIX?.ToString());
                p.Add(s.Instance, s.Path, s.Name, ix, "NUMERIC_SCALE", "Int32", s.NUMERIC_SCALE?.ToString());
                p.Add(s.Instance, s.Path, s.Name, ix, "DATETIME_PRECISION", "Int32", s.DATETIME_PRECISION?.ToString());
                p.Add(s.Instance, s.Path, s.Name, ix, "CHARACTER_SET_CATALOG", "String", s.CHARACTER_SET_CATALOG);
                p.Add(s.Instance, s.Path, s.Name, ix, "CHARACTER_SET_SCHEMA", "String", s.CHARACTER_SET_SCHEMA);
                p.Add(s.Instance, s.Path, s.Name, ix, "CHARACTER_SET_NAME", "String", s.CHARACTER_SET_NAME);
                p.Add(s.Instance, s.Path, s.Name, ix, "COLLATION_CATALOG", "String", s.COLLATION_CATALOG);
                p.Add(s.Instance, s.Path, s.Name, ix, "COLLATION_SCHEMA", "String", s.COLLATION_SCHEMA);
                p.Add(s.Instance, s.Path, s.Name, ix, "COLLATION_NAME", "String", s.COLLATION_NAME);
            }
            p.ToJsonFile(jsonFolder);
            return ix;
        }
        public static IEnumerable<MsSqlTableColumns> GetSqlTableColumns(string server, string instance, string database)
        {
            const string query = @"SELECT TABLE_CATALOG, TABLE_SCHEMA, TABLE_NAME, COLUMN_NAME, ORDINAL_POSITION, 
                                     COLUMN_DEFAULT, IS_NULLABLE, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, 
                                     NUMERIC_PRECISION, NUMERIC_PRECISION_RADIX, NUMERIC_SCALE,
                                     DATETIME_PRECISION, 
                                     CHARACTER_SET_CATALOG, CHARACTER_SET_SCHEMA, CHARACTER_SET_NAME,
                                     COLLATION_CATALOG, COLLATION_SCHEMA, COLLATION_NAME
                                FROM INFORMATION_SCHEMA.COLUMNS;";
            //var db = database.Split('.');
            var connection = $"server={server}\\{instance}; database={database}; Integrated Security=SSPI;";
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
                            COLLATION_NAME = reader.SafeGetString(18),
                            Server = server,
                            Instance = instance
                        };
                        r.Path = r.TABLE_CATALOG;
                        r.Name = $"{r.TABLE_SCHEMA}.{r.TABLE_NAME}.{r.COLUMN_NAME}";
                        yield return (r);
                    }
                }
            }
        }


    }

    public class MsSqlServers
    {

        public string Server { get; set; }
        public string Instance { get; set; }    // instance
        public string Path { get; set; }        // nothing
        public string Name { get; set; } // server\instance
        public string Version { get; set; }

        public override string ToString()
        {
            return $"{Server}\\{Instance}.{Name};";
        }

        public static Int32 ToJsonFile(List<MsSqlServers> servers, DateTime inventoryTime, string jsonFolder)
        {
            var className = MethodBase.GetCurrentMethod().DeclaringType.Name;
            int ix = 0;
            if (servers.Count < 1) return ix;
            var server = (from sv in servers select sv).FirstOrDefault();
            InfoParts p = new InfoParts(server.Server, className, inventoryTime);
            foreach (var s in servers)
            {
                ix++;
                p.Add(s.Instance, s.Path, s.Name, ix, "Version" , $"${s.Version.Length}", s.Version);
            }
            p.ToJsonFile(jsonFolder);
            return ix;
        }
        private static readonly RegistryView registryView = Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32;
        public static IEnumerable<MsSqlServers> GetSqlInstanceNames()
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
                            Name = instanceName,
                            Version = GetSqlInstanceVersion(machineName, instanceName)
                        };
                        yield return (value); // (machineName + @"\" + instanceName);
                    }
                }
            }
        }

        public static string GetSqlInstanceVersion(string server, string instance)
        {
            var result = string.Empty;
            var connection = $"server={server}\\{instance}; database=master; Integrated Security=SSPI;";
            try
            {
                using (var con = new SqlConnection(connection))
                {
                    con.Open();
                    string query = "SELECT @@VERSION;";
                    using (var cmd = new SqlCommand(query, con))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result = reader.SafeGetString(0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }


    }

    public class MsSqlDatabases
    {
        public string Server { get; set; }
        public string Instance { get; set; }    // instance
        public string Path { get; set; }        // nothing
        public string Name { get; set; } // database name

        public DateTime CreationDate { get; set; }
        public string FilePath { get; set; }

        public override string ToString()
        {
            return $"{Server}\\{Instance}.{Name};";
        }
        public static Int32 ToJsonFile(List<MsSqlDatabases> databases, DateTime inventoryTime, string jsonFolder)
        {
            var className = MethodBase.GetCurrentMethod().DeclaringType.Name;
            int ix = 0;
            if (databases.Count < 1) return ix;
            var server = (from sv in databases select sv).FirstOrDefault();
            InfoParts p = new InfoParts(server.Server, className, inventoryTime);
            foreach (var s in databases)
            {
                ix++;
                p.Add(s.Instance, s.Path, s.Name, ix, "CreationDate", "DateTime", s.CreationDate.ToString("o"));
                p.Add(s.Instance, s.Path, s.Name, ix, "FilePath", "String", s.FilePath);
            }
            p.ToJsonFile(jsonFolder);
            return ix;
        }
        public static IEnumerable<MsSqlDatabases> GetSqlDatabases(string server, string instance)
        {
            var connection = $"server={server}\\{instance}; database=master; Integrated Security=SSPI;";
            using (var con = new SqlConnection(connection))
            {
                con.Open();
                //using (var cmd = new SqlCommand("Select Name from Sys.Databases;", con))
                // WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb'); "ReportServer" and "ReportServerTempDB
                // WHERE  dbid > 4; Le(owner_sid) > 1       // 'ReportServer', 
                string query = "SELECT * FROM SYS.DATABASES WHERE database_id > 4 AND Name NOT IN ('ReportServerTempDB');";
                using (var cmd = new SqlCommand(query, con))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var result = new MsSqlDatabases()
                        {
                            Server = server,
                            Instance = instance,
                            Name = reader[0].ToString()
                        };
                        //result.Name = $"{result.Database}";
                        yield return (result); // (instance + "." + reader[0].ToString());
                    }
                }
            }
        }


    }

    public class MsSqlTables
    {
        public string Server { get; set; }
        public string Instance { get; set; }    // instance
        public string Path { get; set; }        // database
        public string Name { get; set; } // schema.table

        //public string Instance { get; set; }
        //public string Database { get; set; }
        //public string Table { get; set; }
        public int? RecordCount { get; set; }
        public string TableType { get; set; }
        public string Status { get; set; }

        public override string ToString()
        {
            return $"{Server} {Name} = {RecordCount}; {Status}";
        }
        public static Int32 ToJsonFile(List<MsSqlTables> sqlTables, DateTime inventoryTime, string jsonFolder)
        {
            var className = MethodBase.GetCurrentMethod().DeclaringType.Name;
            int ix = 0;
            if (sqlTables.Count < 1) return ix;
            var server = (from sv in sqlTables select sv).FirstOrDefault();
            InfoParts p = new InfoParts(server.Server, className, inventoryTime);
            foreach (var s in sqlTables)
            {
                ix++;
                p.Add(s.Instance, s.Path, s.Name, ix, "RecordCount", "Int32", s.RecordCount.ToString());
                p.Add(s.Instance, s.Path, s.Name, ix, "TableType", "String", s.TableType);
                p.Add(s.Instance, s.Path, s.Name, ix, "Status", "String", s.Status);
            }
            p.ToJsonFile(jsonFolder);
            return ix;
        }
        public static List<MsSqlTables> GetSqlDatabaseTables(string server, string instance, string database)
        {
            //var db = database.Split('.');
            var tables = new List<MsSqlTables>();
            string tbschema = string.Empty;
            string table = string.Empty;
            var connection = $"server={server}\\{instance}; database={database}; Integrated Security=SSPI;";

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
                            Name = $"{tbschema}.{table}",
                            Instance = instance,
                            TableType = row[3].ToString(),
                            //Table = $"{db[0]}.{row[0]}.{row[1]}.{row[2]}:{row[3]}"
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
                    Instance = instance,
                    Path = database,
                    Name = $"{tbschema}.{table}",
                    Status = ex.Message
                };
                tables.Add(exresult);
            }
            return (tables);// $"{db[0]}.{row[0]}.{row[1]}.{row[2]}\t{row[3]}";
        }
        public static int? SqlRecordCount(SqlConnection con, string database, string schema, string table)
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


    }

    public class MsSqlStoredProcedure
    {
        public string Server { get; set; }
        public string Instance { get; set; }    // instance
        public string Path { get; set; }        // database
        public string Name { get; set; } // schema.procedure

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public Int32 SpLength { get; set; }
        public bool HasASCII { get; set; }      
        public string Hash { get; set; }
        public string Synopsis { get; set; }     // quick list of major patterns of SQL commands
        public string Abstract { get; set; }    // hashes of words, word counts
        public string Status { get; set; }

        public override string ToString()
        {
            return $"{Server}\\{Instance}.{Path}.{Name} = {Hash}; {Status}";
        }
        public static Int32 ToJsonFile(List<MsSqlStoredProcedure> sqlStoredProcedures, DateTime inventoryTime, string jsonFolder)
        {
            var className = MethodBase.GetCurrentMethod().DeclaringType.Name;
            int ix = 0;
            if (sqlStoredProcedures.Count < 1) return ix;
            var server = (from sv in sqlStoredProcedures select sv).FirstOrDefault();
            InfoParts p = new InfoParts(server.Server, className, inventoryTime);
            foreach (var s in sqlStoredProcedures)
            {
                ix++;
                p.Add(s.Instance, s.Path, s.Name, ix, "Created", "DateTime", s.Created.ToString("o"));
                p.Add(s.Instance, s.Path, s.Name, ix, "Modified", "DateTime", s.Modified.ToString("o"));
                p.Add(s.Instance, s.Path, s.Name, ix, "SpLength", "Int32", s.SpLength.ToString());
                p.Add(s.Instance, s.Path, s.Name, ix, "HasASCII", "Boolean", s.HasASCII.ToString());
                p.Add(s.Instance, s.Path, s.Name, ix, "Hash", "String", s.Hash);
                p.Add(s.Instance, s.Path, s.Name, ix, "Synopsis", "String", s.Hash);
                p.Add(s.Instance, s.Path, s.Name, ix, "Abstract", "String", s.Abstract);
                p.Add(s.Instance, s.Path, s.Name, ix, "Status", "String", s.Status);
            }
            p.ToJsonFile(jsonFolder);
            return ix;
        }
        private static readonly string[] crlf = new string[] { "\r\n" };
        public static IEnumerable<MsSqlStoredProcedure> GetSqlStoredProcedures(string server, string instance, string database)
        {

            const string spQuery = @"select SPECIFIC_CATALOG, SPECIFIC_SCHEMA, SPECIFIC_NAME, ROUTINE_TYPE, CREATED, LAST_ALTERED
	            , OBJECT_DEFINITION(OBJECT_ID(SPECIFIC_SCHEMA + '.' + SPECIFIC_NAME)) as USP_TEXT
                from information_schema.routines 
                ";

            //            const string pquery = @"
            //Select o.name, o.crdate, o.refdate, c.colid, c.text
            //from SysObjects o
            //Inner join SysComments c on c.id = o.id
            //Where o.xtype = 'P'
            //order by o.name, c.colid
            //";
            int spNumber = 0;
            var sHash = new SqlAbstract();
            var sw = new Stopwatch();
            sw.Start();
            var db = database.Split('.');
            var connection = $"server={server}\\{instance}; database={database}; Integrated Security=SSPI;";
            using (var con = new SqlConnection(connection))
            {
                con.Open();
                using (var cmd = new SqlCommand(spQuery, con))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        spNumber++;
                        var uspBody = reader.SafeGetString(6);
                        var elapsedMs = sw.ElapsedMilliseconds;
                        if (elapsedMs > 2000)
                        {
                            sw.Restart();
                            var spname = database + "." + reader.SafeGetString(1) + "." + reader.SafeGetString(2);
                            Console.WriteLine($"{Collect.LogTime()} Reading.. MsSqlInventory::SP{spNumber}:[{spname}]");
                            //Console.WriteLine($"{Collect.LogTime()} Completed MsSqlInventory::..:[GetSqlStoredProcedures]");
                        }
                        var rawbase = uspBody.Split(crlf, StringSplitOptions.RemoveEmptyEntries);
                        //var basestring = SqlCleaner.Clean(rawbase, HideAscii: false);
                        //var BaseHasAscii = sHash.AddScript(basestring);
                        //var baseAbstract = sHash.ToString();
                        var baseAbstract = sHash.Abstract(rawbase);
                        var baseHasAscii = sHash.HasAscii;
                        var baseHash = sHash.Allhash;
                        var baseSynopsis = sHash.Synopsis;
                        sHash.Clear();


                        yield return (new MsSqlStoredProcedure()
                        {
                            Server = server,
                            Instance = instance,
                            Path = database,
                            Name = reader.SafeGetString(1) + "." + reader.SafeGetString(2),
                            Created = reader.GetDateTime(4),
                            Modified = reader.GetDateTime(5),
                            Hash = baseHash,
                            Synopsis = baseSynopsis,
                            Abstract = baseAbstract,
                            HasASCII = baseHasAscii
                        });
                    }
                }
            }
            sw.Stop();
        }

    }

    class MsSqlInventory
    {
        readonly DateTime InventoryTime = DateTime.UtcNow;
        List<MsSqlServers> sqlinstances;
        List<MsSqlDatabases> dblist  = new List<MsSqlDatabases>();
        List<MsSqlTables> tblist  = new List<MsSqlTables>();
        List<MsSqlTableColumns> columns = new List<MsSqlTableColumns>();
        List<MsSqlStoredProcedure> procedures = new List<MsSqlStoredProcedure>();





        //public string Get

        // https://stackoverflow.com/questions/1054984/how-can-i-get-column-names-from-a-table-in-sql-server


        public void Inventory(string saveToFolder)
        {
            Console.WriteLine($"{Collect.LogTime()} Begin MsSqlInventory::...");

            sqlinstances = MsSqlServers.GetSqlInstanceNames().ToList<MsSqlServers>();
            Console.WriteLine($"{Collect.LogTime()} Completed MsSqlInventory::..:[GetSqlInstanceNames]");
            sqlinstances.ForEach(d => { dblist
                                        .AddRange(MsSqlDatabases.GetSqlDatabases(d.Server, d.Name)
                                        .ToList<MsSqlDatabases>()); });
            Console.WriteLine($"{Collect.LogTime()} Completed MsSqlInventory::..:[GetSqlDatabases]");
            //dblist.ForEach(d => { tblist.AddRange(GetSqlDatabaseTables(d.Server, d.Instance, d.Database).ToList<MsSqlTables>()); });
            dblist.ForEach(d => { tblist
                                    .AddRange(MsSqlTables.GetSqlDatabaseTables(d.Server, d.Instance, d.Name)); });
            Console.WriteLine($"{Collect.LogTime()} Completed MsSqlInventory::..:[GetSqlDatabaseTables]");
            dblist.ForEach(d => { columns
                                    .AddRange(MsSqlTableColumns.GetSqlTableColumns(d.Server, d.Instance, d.Name)
                                    .ToList<MsSqlTableColumns>()); });
            Console.WriteLine($"{Collect.LogTime()} Completed MsSqlInventory::..:[GetSqlTableColumns]");

            dblist.ForEach(d => { procedures
                                    .AddRange(MsSqlStoredProcedure.GetSqlStoredProcedures(d.Server, d.Instance, d.Name)
                                    .ToList<MsSqlStoredProcedure>()); });
            Console.WriteLine($"{Collect.LogTime()} Completed MsSqlInventory::({procedures.Count()}):[GetSqlStoredProcedures]");

            var InventoryFolder = saveToFolder;
            MsSqlServers.ToJsonFile(sqlinstances, InventoryTime, InventoryFolder);
            MsSqlDatabases.ToJsonFile(dblist, InventoryTime, InventoryFolder);
            MsSqlTables.ToJsonFile(tblist, InventoryTime, InventoryFolder);
            MsSqlTableColumns.ToJsonFile(columns, InventoryTime, InventoryFolder);
            MsSqlStoredProcedure.ToJsonFile(procedures, InventoryTime, InventoryFolder);
            Console.WriteLine($"{Collect.LogTime()} Completed MsSqlInventory::..:[ToJsonFile]");

            //var ix = 0;
        }

    }
}
