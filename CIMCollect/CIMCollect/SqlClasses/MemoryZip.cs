using System;
using System.Text;
using System.IO;
using System.IO.Compression;

// https://stackoverflow.com/questions/7343465/compression-decompression-string-with-c-sharp
// https://stackoverflow.com/questions/1581694/gzipstream-and-decompression
namespace Similarity
{
    class MemoryZip
    {
        public static string Zip(string value)
        {
            var byteArray = Encoding.UTF8.GetBytes(value);
            string zipB64;
            using (var ms = new MemoryStream())
            {
                using (var sw = new GZipStream(ms, CompressionMode.Compress))
                {
                    sw.Write(byteArray, 0, byteArray.Length);
                    //Close, DO NOT FLUSH cause bytes will go missing...
                    sw.Close();
                }
                //Transform byte[] zip data to string
                var zipBytes = ms.ToArray();
                zipB64 = Convert.ToBase64String(zipBytes);
            }
            return zipB64;
        }

        public static string UnZip(string value)
        {
            var img = value.StartsWith("$GZ:") ? 
                                Convert.FromBase64String(value.Remove(0, 4)) : 
                                Convert.FromBase64String(value);
            using (var to = new MemoryStream())
            {
                using (var from = new MemoryStream(img))
                {
                    using (var compress = new GZipStream(from, CompressionMode.Decompress))
                    {
                        compress.CopyTo(to);
                    }
                    from.Close();
                }
                return Encoding.UTF8.GetString(to.ToArray());
            }
        }

    }
}
