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
    class MsSqlInventory
    {

        List<string> sqlinstances;
        List<string> dblist  = new List<string>();
        List<string> tblist  = new List<string>();
        List<Tuple<string, string, string, string>> columns = new List<string>();

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
                string query = "select * from sys.databases where len(owner_sid) > 1;";
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
                List<string> TableNames = new List<string>();
                foreach (DataRow row in schema.Rows)
                {
                    yield return $"{db[0]}.{row[0]}.{row[1]}.{row[2]}\t{row[3]}";
                }
            }
        }

        public IEnumerable<string> GetSqlTableColumns(string table)
        {
            var db = table.Split('\t');
            var tb = db[0].Split('.');
            var connection = $"server={db[0]}; database={db[1]}; Integrated Security=SSPI;";
            using (var con = new SqlConnection(connection))
            {
                con.Open();
                var sql = "SELECT* from INFORMATION_SCHEMA.COLUMNS";
                DataTable schema = con.GetSchema("Tables");
                List<string> TableNames = new List<string>();
                foreach (DataRow row in schema.Rows)
                {
                    yield return $"{db[0]}.{row[0]}.{row[1]}.{row[2]}\t{row[3]}";
                }
            }
        }


        // https://stackoverflow.com/questions/1054984/how-can-i-get-column-names-from-a-table-in-sql-server

        public void Inventory()
        {
            sqlinstances = GetSqlInstanceNames().ToList<string>();
            sqlinstances.ForEach(d => { dblist.AddRange(GetSqlDatabases(d).ToList<string>()); });
            dblist.ForEach(d => { tblist.AddRange(GetSqlDatabaseTables(d).ToList<string>()); });
            tblist.ForEach(t => { columns.AddRange(GetSqlTableColumns(t).ToList<string>()); });


            var ix = 0;
        }

    }
}
