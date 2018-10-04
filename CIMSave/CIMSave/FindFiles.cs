using System;
using System.Diagnostics;
using CIMCollect;
//using System.Data.DataSetExtensions;

namespace CIMSave
{
    // https://stackoverflow.com/questions/7574606/left-function-in-c-sharp
    public static class StringExtensions
    {
        public static string Left(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            maxLength = Math.Abs(maxLength);

            return (value.Length <= maxLength
                   ? value
                   : value.Substring(0, maxLength)
                   );
        }
    }

    public static class ByteExtensions
    {
        public static string Left(this byte[] value, int maxLength)
        {
            if (value.Length == 0) return String.Empty;
            maxLength = Math.Abs(maxLength);
            var result = BitConverter.ToString(value).Replace("-", "");
            return (result.Length <= maxLength
                   ? result
                   : result.Substring(0, maxLength)
                   );
        }
    }


    class FindFiles
    {
        //public string SqlConnectionString { get; set; }
        public string TablePrefix { get; set; }

        public int Breakspot { get; set; } = 0;

        public void AllJson(string filePath, string fileMask, string databaseSchema, string tablePrefix)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("message", nameof(filePath));
            }

            if (string.IsNullOrWhiteSpace(fileMask))
            {
                throw new ArgumentException("message", nameof(fileMask));
            }

            if (string.IsNullOrWhiteSpace(databaseSchema))
            {
                throw new ArgumentException("message", nameof(databaseSchema));
            }

            if (string.IsNullOrWhiteSpace(tablePrefix))
            {
                throw new ArgumentException("message", nameof(tablePrefix));
            }

            int files = 0;
            var server = Environment.MachineName;
            TablePrefix = tablePrefix;
            var sw = new Stopwatch();
            Console.WriteLine($"{LogTime()} Processing {filePath}\\{fileMask} files.");
            sw.Start();
            foreach (var filename in System.IO.Directory.EnumerateFiles(filePath, fileMask))
            {
                files++;
                HandleFile(filename, databaseSchema);
            }
            sw.Stop();
            Console.WriteLine($"{LogTime()} Processed {files} dataset files in {sw.ElapsedMilliseconds}ms.");
            Pause();
        }

        private void Pause()
        {
            Utilities.SemiPause("Press any key to exit.", 30);
            //Console.Write("Press enter to exit");
            //Console.ReadLine();
        }

        private string LogTime() => DateTime.Now.ToString("u");

        private readonly string[] possibilities = new string[] { "{\"AsOf\":", "{\"Directories\":", "{\"idList\":" };

        //private void HandleFile(string filename, string schema)
        private void HandleFile(string filename, string schema)
        {

            Console.WriteLine($"{LogTime()} Processing file {filename}.");

            //InfoParts p = InfoParts.FromJsonFile(filename);
            //InfoParts p = GZfileIO.ReadGZtoJson<InfoParts>(filename);

            var header = GZfileIO.Head(filename).Left(64);
            var sniffType = GZfileIO.Sniff(filename, possibilities);
            switch (sniffType)
            {
                case 0: // AsOf -> InfoParts
                    HandleInfoParts(filename, schema);
                    break;
                case 1: // Directories - file inventory
                    {
                        // pull the ACLs into the database
                        var acl = DirectorySecurityList.ACLSet.FromJSON(filename);
                        acl.ToDB();
                        // now can use the ACLs via json ACL id => database ACL id

                        var files = DirectorySecurityList.CIMDirectoryCollection.FromJSON(filename);
                        files.ToDB(acl);
                    }
                    Console.WriteLine($"{LogTime()} Unable to process directory file {filename}.");
                    Console.WriteLine($" >>>{header}<<< ");
                    //throw new NotImplementedException(header);
                    break;
                case 2: // ACL list
                    {
                        var x = DirectorySecurityList.ACLSet.FromJSON(filename);
                        x.ToDB();
                    }
                    //Console.WriteLine($"{LogTime()} Unable to process ACL file {filename}.");
                    //Console.WriteLine($" >>>{header}<<< ");
                    //throw new NotImplementedException(header);
                    break;
                case -1: // unknown
                default:
                    Console.WriteLine($"{LogTime()} Unable to process file {filename}.");
                    Console.WriteLine($" >>>{header}<<< ");
                    break;
            }

            //Pause();
        }

        private void HandleInfoParts(string filename, string schema)
        {
            InfoParts p = GZfileIO.ReadGZtoPOCO<InfoParts>(filename);
            //p.SqlConnectionString = this.SqlConnectionString;
            //var p2db = new InfoPartsToDB(SqlConnectionString, TablePrefix);
            var p2db = new InfoPartsToDB(TablePrefix);
            p2db.PartsToDataSet(p, schema);
        }
    }
}
