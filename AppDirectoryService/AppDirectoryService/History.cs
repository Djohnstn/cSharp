using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AppDirectoryService
{
    public class History
    {

        //private static RecentHistory[] Qhistory(IQueryCollection query,
        //                                        ConnectionInfo connection,
        //                                        HttpContext context)
        public RecentHistory[] QryHistory(IQueryCollection query, ConnectionInfo connection, string ConxString)
        //private static RecentHistory[] Qhistory([FromQuery] string id, [FromQuery] string pc, ConnectionInfo connection)
        {
            try
            {
                var conxIPAddress = connection.RemoteIpAddress.ToString();
                var queryDictionary = query.ToDictionary(
                                _ => _.Key,
                                _ => _.Value,
                                StringComparer.OrdinalIgnoreCase
                                );
                //var queryNames = (from z in sqlHistory.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                //                  where z.StartsWith('@')
                //                  select z)
                //                  .Distinct().ToList()
                //                  ;

                //var param = new SqlParameter();

                string u = SafeTryGetValue(queryDictionary, "winuser");
                string m = SafeTryGetValue(queryDictionary, "pc");
                var x = QryRecent(u, m, ConxString);
                return x; // "result:" + x + ";&ip:" + conxIPAddress;
            }
            catch (Exception ex)
            {
                return new[] { new RecentHistory() { status = ex.Message } };
            }
        }


        // https://www.c-sharpcorner.com/article/net-core-1-0-connecting-sql-server-database/
        const string sqlHistory =
            @"Select    a.ApplicationName
		              , a.ApplicationVersion
		                --, a.WinUserID
		                --, a.PIMSRoleName
		                --, a.PCName
		                --, a.PCOperatingSystem
		                --, a.PIUserID
		                --, a.PIServerName
		                --, a.UTCofAccess
		                --, a.PIMSSDKVersion
		              , a.ServerTime
		                --, a.SQLUserID
		                --, a.IPAddress
		                --, a.id
                from dbo.pimsAccounting a
                where a.ServerTime < sysdatetimeoffset() 
                    and a.ServerTime > DateAdd(Hour, -1, sysdatetimeoffset())
		            and a.pcname = @PC -- 'PCXXXX12345X'
                    and (a.WinUserID = @WINUSER or @WINUSER = '') -- 'domain\\Bob'
                order by a.ServerTime Desc
";


        public RecentHistory[] QryRecent(string qryUser, string qryPc, string ConxString)
        {

            var conxstr = ConxString; // "Data Source=.\\sqlexpress;Initial Catalog=CIMInfo;Integrated Security=True;Pooling=True; Application Name=AppDirectoryService;";
            // sample = "Server=tcp:YourServer,1433;Initial Catalog=YourDatabase;Persist Security Info=True;";
            string results = $"u:{qryUser};pc:{qryPc}";
            var outlist = new List<RecentHistory>();
            using (var connection = new SqlConnection(conxstr))
            {   connection.Open();
                using (var command = new SqlCommand(sqlHistory, connection))
                {
                    command.Parameters.Add("@PC", System.Data.SqlDbType.VarChar).Value = qryPc;
                    command.Parameters.Add("@WINUSER", System.Data.SqlDbType.VarChar).Value = qryUser;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            outlist.Add(new RecentHistory()
                            {
                                appname = reader.GetString(0),
                                pc = reader.GetString(1),
                                when = reader.GetDateTimeOffset(2),
                                status = string.Empty
                            });
                        }
                    }
                }
            }
            return outlist.ToArray(); 
        }

        public InsertedHistory AddHistory(IQueryCollection query, ConnectionInfo connection, string ConxString)
        {
            var result = new InsertedHistory();
            try
            {
                var conxIPAddress = connection.RemoteIpAddress.ToString();
                var queryDictionary = query.ToDictionary(
                                _ => _.Key,
                                _ => _.Value,
                                StringComparer.OrdinalIgnoreCase
                                );

                string AppName = SafeTryGetValue(queryDictionary, "appname");
                string AppVer = SafeTryGetValue(queryDictionary, "appver");
                string WinUser = SafeTryGetValue(queryDictionary, "winuser");

                string PIMSRole = SafeTryGetValue(queryDictionary, "pimsrole");
                string PC = SafeTryGetValue(queryDictionary, "pc");
                string PCOS = SafeTryGetValue(queryDictionary, "pcos");

                string PIUser = SafeTryGetValue(queryDictionary, "piuser");
                string PIServer = SafeTryGetValue(queryDictionary, "piserver");
                string PCDate = SafeTryGetValue(queryDictionary, "pcdate");

                string PIMSSDK = SafeTryGetValue(queryDictionary, "pimssdk");
                //string SQLUSER = SafeTryGetValue(queryDictionary, "PIMSRoleName");
                string IPADDRESS = conxIPAddress; // SafeTryGetValue(queryDictionary, "ipaddress");

                result.status = string.Empty;
                result.updates = UpdateSql(ConxString, AppName, AppVer, WinUser,
                                PIMSRole, PC, PCOS, PIUser, PIServer, PCDate, PIMSSDK, IPADDRESS);
                return result; // "result:" + x + ";&ip:" + conxIPAddress;
            }
            catch (Exception ex)
            {
                result.status = ex.Message;
                result.updates = 0;
                return result;
            }
        }

        private static string SafeTryGetValue(Dictionary<string, StringValues> queryDictionary, string value)
        {
            if (queryDictionary.TryGetValue(value, out StringValues result))
            {
                return result;
            }

            return String.Empty;
        }

        private static SqlParameter TryGetParameter(Dictionary<string, StringValues> queryDictionary, string value)
        {
            var qryparam = value.Substring(1).ToLower();
            if (queryDictionary.TryGetValue(qryparam, out StringValues result))
            {
                var val = result.ToString();
                var param = new SqlParameter(value, System.Data.SqlDbType.NVarChar);
                param.Value = value;
                return param ;
            }
            return null;
        }

        const string sqlAddHistory =
    @"IF NOT EXISTS (SELECT * FROM dbo.pimsAccounting
                     WHERE ApplicationName = @APPNAME 
                     AND WinUserID = @WINUSER
                     AND PCName = @PC
                     AND IPAddress = @IPADDRESS
                     AND ServerTime >= DateAdd(Hour, -1, sysdatetimeoffset()))
      BEGIN
            INSERT INTO dbo.pimsAccounting 
                (ApplicationName, ApplicationVersion, WinUserID, PIMSRoleName, PCName, PCOperatingSystem,
		         PIUserID, PIServerName, UTCofAccess, PIMSSDKVersion, IPAddress)
             VALUES (@APPNAME, @APPVER, @WINUSER, @PIMSROLE, @PC, @OS, 
                     @PIUSER, @PISERVER, @PCDATE, @PIMSSDK, @IPADDRESS)
      END";


        private int UpdateSql(string ConxString, string AppName, string AppVer, string WinUser,
                        string PIMSRole, string PC, string PCOS, string PIUser,
                        string PIServer, string PCDate, string PIMSSDK, string IPADDRESS)
        //public int UpdateSql(string qryUser, string qryPc, string ConxString)
        {

            var conxstr = ConxString; // "Data Source=.\\sqlexpress;Initial Catalog=CIMInfo;Integrated Security=True;Pooling=True; Application Name=AppDirectoryService;";
            // sample = "Server=tcp:YourServer,1433;Initial Catalog=YourDatabase;Persist Security Info=True;";
            string results = $"u:{WinUser};pc:{PC}";
            int updates = 0;
            using (var connection = new SqlConnection(conxstr))
            {
                connection.Open();
                using (var command = new SqlCommand(sqlAddHistory, connection))
                {
                    command.Parameters.Add("@APPNAME", System.Data.SqlDbType.VarChar).Value = AppName;
                    command.Parameters.Add("@APPVER", System.Data.SqlDbType.VarChar).Value = AppVer;
                    command.Parameters.Add("@WINUSER", System.Data.SqlDbType.VarChar).Value = WinUser;
                    command.Parameters.Add("@PIMSROLE", System.Data.SqlDbType.VarChar).Value = PIMSRole;
                    command.Parameters.Add("@PC", System.Data.SqlDbType.VarChar).Value = PC;
                    command.Parameters.Add("@OS", System.Data.SqlDbType.VarChar).Value = PCOS;
                    command.Parameters.Add("@PIUSER", System.Data.SqlDbType.VarChar).Value = PIUser;
                    command.Parameters.Add("@PISERVER", System.Data.SqlDbType.VarChar).Value = PIServer;
                    command.Parameters.Add("@PCDATE", System.Data.SqlDbType.VarChar).Value = PCDate;
                    command.Parameters.Add("@PIMSSDK", System.Data.SqlDbType.VarChar).Value = PIMSSDK;
                    command.Parameters.Add("@IPADDRESS", System.Data.SqlDbType.VarChar).Value = IPADDRESS;
                    updates = command.ExecuteNonQuery();
                }
            }
            return updates;
        }

    }
}
