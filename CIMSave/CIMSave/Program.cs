using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.IO.Compression;
//using System.IO;
//using System.Security.Cryptography;


namespace CIMSave
{
    class Program
    {
        static void Main(string[] args)
        {
            var settings = new Properties.Settings();
            var connectionString = settings.database;
            var schema = settings.schema;
            var tablePrefix = settings.TablePrefix;

            //string inputfolder = "";

            const string baseMask = "*_*.Json.gz";
            string mask = baseMask;
            CommandlineParameters.Set(args); //, new Properties.Settings());
            foreach (var arg in args)
            {
                //if (arg.StartsWith("-f",StringComparison.InvariantCultureIgnoreCase))
                //{
                //    inputfolder = arg.Remove(0, 2);
                //}
                if (arg.StartsWith("-m", StringComparison.InvariantCultureIgnoreCase))
                {
                    mask = arg.Remove(0, 2);
                    if (mask.Length == 0) mask = baseMask;
                }
            }


            Console.WriteLine($"[{connectionString}].[{schema}].[{tablePrefix}...]");
            //Console.WriteLine($"[{}]");

            var f = new FindFiles(); // { SqlConnectionString = connectionString };
            f.AllJson(CommandlineParameters._fileSaveFolder, mask, schema, tablePrefix);
            //StringCompressDecompress();
        }



    }
}
