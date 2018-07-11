using System;
using System.Security.Cryptography;
using SaveScraps;

namespace AppDirectoryTester1
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var apg = Extensions.AssemblyInfoReader.AppGuid;
            var apfn = Extensions.AssemblyInfoReader.FullName;
            var apkt = Extensions.AssemblyInfoReader.PublicKeyToken;
            var apk = Extensions.AssemblyInfoReader.PublicKey;
            var apver = Extensions.AssemblyInfoReader.Version;
            var y = Extensions.AssemblyInfoReader.Location;
            //var x = Extensions.AssemblyInfo.ProductId;
            var encrypted = EncryptString(apg.ToString(), apkt, apver.ToString(), "a=b&c=d&e=f");
            var decrypted = DecryptString(apg.ToString(), apkt, apver.ToString(), encrypted);
            Console.WriteLine(encrypted);
            //ulong x = 0;
        }

        public static string DecryptString(string key1, byte[] otpkey, string key3, string data)
        {
            if (data.StartsWith("zyy"))
            {
                // zyy is magic cookie for version 1.00
                var untagged = data.Remove(0, 3);
                var bytes = Convert.FromBase64String(untagged);
                var byte2 = DecryptBytestoString(key1, otpkey, key3, bytes);
                return byte2;
            }
            else
            {
                throw new NotImplementedException("data encryption not known");
            }
            //var bytes = EncryptStringtoBytes(key1, otpkey, key3, data);
            //return "q=zyy" + Convert.ToBase64String(bytes);
        }

        public static string EncryptString(string key1, byte[] otpkey, string key3, string data)
        {
            // zyy is magic cookie for version 1.00
            var bytes = EncryptStringtoBytes(key1, otpkey, key3, data);
            return "q=zyy" + Convert.ToBase64String(bytes);
        }

        public static byte[] EncryptStringtoBytes (string key1, byte[] otpkey, string key3, string data)
        {
            var encryptor = new encDec2();
            var b1 = HashBytes(System.Text.Encoding.UTF8.GetBytes(key1));
            var encpart1 = encryptor.EncryptString(data, b1);

            var unixMinute = (DateTimeOffset.UtcNow.ToUnixTimeSeconds() / 60);
            var b2 = OtpHash(otpkey, unixMinute);
            var encpart2 = ConcatenateArrays(encpart1, b2);

            var b3 = HashBytes(System.Text.Encoding.UTF8.GetBytes(key3));
            var encpart3 = encryptor.EncryptBytes(encpart2, b3);
            return encpart3;
        }

        public static uint OtpHash(byte[] key, long stamp)
        {
            byte[] bytestohash = ConcatenateArrays(key, stamp);
            var hashedBytes = HashBytes(bytestohash);
            int offset = (hashedBytes[0] & (byte)0x0f); // get last nibble
            uint otp = BitConverter.ToUInt32(hashedBytes, offset + 1);
            return otp % 100_000_000;
        }

        private static byte[] ConcatenateArrays(byte[] key, long stamp)
        {
            var ustamp = BitConverter.GetBytes(stamp);
            return ConcatenateArrays(key, ustamp);
        }

        private static byte[] ConcatenateArrays(byte[] key, byte[] ustamp)
        {
            var newbytes = new byte[ustamp.Length + key.Length];
            Array.Copy(key, newbytes, key.Length);
            Array.Copy(ustamp, 0, newbytes, key.Length, ustamp.Length);
            return newbytes;
        }

        public static byte[] HashBytes(byte[] bytes)
        {
            UInt64 result = 0x0102030405060708; // bad value but something of file not found
            byte[] hash = BitConverter.GetBytes(result);
            using (var sha = new SHA256Managed())
            {
                hash = sha.ComputeHash(bytes);
                //result = BitConverter.ToUInt64(filehash, 0);
            }
            return hash;
        }


    }
}
