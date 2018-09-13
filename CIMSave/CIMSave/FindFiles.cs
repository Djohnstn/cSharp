using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using CIMCollect;
using System.Data;
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
        public string SqlConnectionString { get; set; }
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
            Console.Write("Press enter to exit");
            Console.ReadLine();
        }

        private string LogTime() => DateTime.Now.ToString("u");

        private void HandleFile(string filename, string schema)
        {

            Console.WriteLine($"{LogTime()} Processing file {filename}.");

            InfoParts p = InfoParts.FromJsonFile(filename);
            //p.SqlConnectionString = this.SqlConnectionString;
            var p2db = new InfoPartsToDB(SqlConnectionString, TablePrefix);
            p2db.PartsToDataSet(p, schema);
            //Pause();
        }
    }
}
