using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace CIMSave
{
    public class SQLHandlerBase
    {
        private static string _connectionString;
        private static string _schema;
        private static string _tableprefix;

        public SQLHandlerBase()
        {
            if (_connectionString == null || _connectionString.Length == 0)
            {
                var conxStrings = System.Configuration.ConfigurationManager.ConnectionStrings;
                _connectionString = conxStrings["database"].ConnectionString;
            }
            if (_schema == null || _schema.Length == 0 || 
                _tableprefix == null || _tableprefix.Length == 0 )
            {
                var settings = System.Configuration.ConfigurationManager.AppSettings;
                _connectionString = settings["database"].Trim();
                _schema = settings["schema"].Trim();
                _tableprefix = settings["TablePrefix"].Trim();
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
                    var scaler = command.ExecuteScalar();
                    if (scaler != null)
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
        private readonly List<string> queries;
        private readonly string NameParameter;
        private readonly T2 DefaultValue;

        public DBMapper(string tablename, T2 defaultvalue, string nameparameter = "name")
        {
            sqlbase = new SQLHandlerBase();
            NameParameter = nameparameter;
            this.DefaultValue = defaultvalue;
            queries = (from q in xquerys select Clean(q, tablename, nameparameter)).ToList();
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
            foreach (var query in xquerys)
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

}
