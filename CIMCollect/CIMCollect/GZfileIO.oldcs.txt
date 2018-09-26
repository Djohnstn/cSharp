using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Diagnostics;
using System.Reflection;

namespace CIMSave
{
    class GZfileIO
    {
        public static string GetSaveFolderName()
        {
            var versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
            var companyName = versionInfo.CompanyName;
            var productName = versionInfo.ProductName;
            string codebase = Assembly.GetExecutingAssembly().CodeBase;

            var savefolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                                        companyName, productName,
                                        Path.GetFileNameWithoutExtension(codebase));
            return savefolder;
        }

        public static long WriteStringToGZ(string filename, string saveThis)
        {
            // for local files, not need to compress is less than 4000 bytes. 
            if (filename.EndsWith("gz"))
            {
                using (Stream fd = File.Create(filename))
                using (Stream csStream = new GZipStream(fd, CompressionMode.Compress))
                {
                    var buffer = Encoding.UTF8.GetBytes(saveThis);
                    csStream.Write(buffer, 0, buffer.Length);
                }
            }
            else
            {
                File.WriteAllText(filename, saveThis);
            }
            var len = new System.IO.FileInfo(filename).Length;
            return len;
        }

        public static string ReadGZtoString(string filename)
        {
            string result;
            var finfo = new System.IO.FileInfo(filename);
            bool exists = finfo.Exists;
            var len = exists ? finfo.Length : 0;
            if (len <= 1)
            {
                result = "";   // empty file or does not exist
            }
            else if (filename.EndsWith("gz"))
            {
                // probably should read to memory and apply IsGZip() {...} function
                using (Stream fs = File.OpenRead(filename))
                using (var decompress = new GZipStream(fs, CompressionMode.Decompress))
                using (var sr = new StreamReader(decompress, Encoding.UTF8))
                {
                    result = sr.ReadToEnd();
                }
            }
            else
            {
                using (Stream fs = File.OpenRead(filename))
                using (var sr = new StreamReader(fs, Encoding.UTF8))
                {
                    result = sr.ReadToEnd();
                }
            }
            return result;
        }

        public static T ReadGZtoJson<T>(string filename)
        {
            var deserializer = new DataContractJsonSerializer(typeof(T));
            T result;
            var finfo = new System.IO.FileInfo(filename);
            bool exists = finfo.Exists;
            var len = exists ? finfo.Length : 0;
            if (len <= 1)
            {
                result = default(T);   // empty file or does not exist
            }
            else if (filename.EndsWith("gz"))
            {
                // probably should read to memory and apply IsGZip() {...} function
                using (Stream fs = File.OpenRead(filename))
                using (var decompress = new GZipStream(fs, CompressionMode.Decompress))
                using (var sr = new StreamReader(decompress, Encoding.UTF8))
                //using (var memoryStream = new MemoryStream(decompress))
                {
                    using (var ms = new MemoryStream())
                    {
                        sr.BaseStream.CopyTo(ms);
                        ms.Position = 0;
                        result = (T)deserializer.ReadObject(ms);
                    }
                }
            }
            else
            {
                using (Stream fs = File.OpenRead(filename))
                using (var sr = new StreamReader(fs, Encoding.UTF8))
                {
                    using (var ms = new MemoryStream())
                    {
                        sr.BaseStream.CopyTo(ms);
                        ms.Position = 0;
                        result = (T)deserializer.ReadObject(ms);
                    }
                }
            }
            return result;
        }
        // https://stackoverflow.com/questions/19364497/how-to-tell-if-a-byte-array-is-gzipped
        public static bool IsGZip(byte[] arr)
        {
            // byte 1 == 31, byte 2 == 139 GZIP header
            // byte 3 == 8 = deflate file type
            return arr.Length >= 4 && arr[0] == 31 && arr[1] == 139 && arr[2] == 8;
        }

    }
}
