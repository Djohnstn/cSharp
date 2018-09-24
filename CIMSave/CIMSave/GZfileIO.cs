using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;
using System.Runtime.Serialization.Json;

namespace CIMSave
{
    class GZfileIO
    {
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

        // https://stackoverflow.com/questions/19364497/how-to-tell-if-a-byte-array-is-gzipped
        public static bool IsGZip(byte[] arr)
        {
            // byte 1 == 31, byte 2 == 139 GZIP header
            // byte 3 == 8 = deflate file type
            return arr.Length >= 4 && arr[0] == 31 && arr[1] == 139 && arr[2] == 8;
        }

        public static T ReadGZtoJson<T>(string jsonfilename)
        {
            var json = GZfileIO.ReadGZtoString(jsonfilename);

            //var deserializer = new JavaScriptSerializer
            //var deserializer = new DataContractJsonSerializer(typeof(T));
            //return (T)deserializer.ReadObject(json);

            using (var sr = new FileStream(jsonfilename, FileMode.Open, FileAccess.Read))
            {
                var deserializer = new DataContractJsonSerializer(typeof(T));
                return (T)deserializer.ReadObject(sr);
            }
        }


        public static T ReadGZtoPOCO<T>(string filename)
        {
            T result;
            var finfo = new System.IO.FileInfo(filename);
            bool exists = finfo.Exists;
            var len = exists ? finfo.Length : 0;
            if (len <= 1)
            {
                result = default(T);
                // empty file or does not exist
            }
            else if (filename.EndsWith("gz"))
            {
                var deserializer = new DataContractJsonSerializer(typeof(T));
                // probably should read to memory and apply IsGZip() {...} function
                using (Stream fs = File.OpenRead(filename))
                using (var decompress = new GZipStream(fs, CompressionMode.Decompress))
                //using (var sr = new StreamReader(decompress, Encoding.UTF8))
                {
                    //result = sr.ReadToEnd();
                    result = (T)deserializer.ReadObject(decompress);

                }
            }
            else
            {
                var deserializer = new DataContractJsonSerializer(typeof(T));
                using (Stream fs = File.OpenRead(filename))
                //using (var sr = new StreamReader(fs, Encoding.UTF8))
                {
                    result = (T)deserializer.ReadObject(fs);
                    //result = sr.ReadToEnd();
                }
            }
            return result;
        }



    }
}
