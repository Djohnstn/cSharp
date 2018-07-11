using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SaveScraps
{
    class encDec2
    {
        //private String applicationId = ((GuidAttribute)typeof(Program).Assembly.GetCustomAttributes(typeof(GuidAttribute), true)[0]).Value;
        private String applicationId = Extensions.AssemblyInfoReader.AppGuid.ToString();
        private Int32 genHashRounds = 0;
        private const int LineLength = 72;
        private const string Prefix = "\xa\xa1\xfa\xb3\xf\t";
        private const string delim = "\t=\t";
        private const string Suffix = "\t\xd\xd1\xc8\xd9\xd\t";
        private const string JSONprefix = "{'z'='";
        private const string JSONsuffix = "'}";
        private const int minRounds = 10;
        private const int maxBytes = 7000;
        public Byte[] genHash(String metaPassword)
        {   // "22584a87-6d8b-445d-8134-d888914e82bb"
            // "22584a87-6d8b-445d-8134-d888914e82bb" - from program guid
            Byte[] passwordBytes = Encoding.UTF8.GetBytes(metaPassword + applicationId);
            Byte[] savedHash = passwordBytes;
            Int32 firstHashSig = (passwordBytes[passwordBytes.Length - 1] * 65536 +
                    passwordBytes[passwordBytes.Length - 2] * 256 +
                    passwordBytes[passwordBytes.Length - 3]) % 1271;
            bool foundIt = false;
            var shaWorker = SHA256.Create();
            for (Int32 ix = 1; !foundIt && ix < 2200000; ix++)
            {
                savedHash = passwordBytes;
                passwordBytes = shaWorker.ComputeHash(savedHash);
                Int32 hashSig = (passwordBytes[passwordBytes.Length - 1] * 65536 +
                        passwordBytes[passwordBytes.Length - 2] * 256 +
                        passwordBytes[passwordBytes.Length - 3]) % 10000;
                if (hashSig == firstHashSig)
                {
                    genHashRounds = ix;
                    foundIt = true;
                }
            }
            return savedHash;
        }

        public Byte[] theHash;

        //https://stackoverflow.com/questions/8041451/good-aes-initialization-vector-practice

        public byte[] EncryptBytes(byte[] toEncrypt, byte[] encryptionKey)
        {
            if (toEncrypt == null || toEncrypt.Length == 0) throw new ArgumentException("toEncrypt");
            if (encryptionKey == null || encryptionKey.Length == 0) throw new ArgumentException("encryptionKey");
            var toEncryptBytes = toEncrypt;
            using (var provider = new AesCryptoServiceProvider())
            {
                provider.Key = encryptionKey;
                provider.Mode = CipherMode.CBC;
                provider.Padding = PaddingMode.PKCS7;
                provider.GenerateIV();
                using (var encryptor = provider.CreateEncryptor(provider.Key, provider.IV))
                {
                    using (var ms = new MemoryStream())
                    {
                        ms.Write(provider.IV, 0, 16);
                        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            cs.Write(toEncryptBytes, 0, toEncryptBytes.Length);
                            cs.FlushFinalBlock();
                        }
                        return ms.ToArray();
                    }
                }
            }
        }

        public byte[] EncryptString(string toEncrypt, byte[] encryptionKey)
        {
            if (string.IsNullOrEmpty(toEncrypt)) throw new ArgumentException("toEncrypt");
            if (encryptionKey == null || encryptionKey.Length == 0) throw new ArgumentException("encryptionKey");

            var toEncryptBytes = Encoding.UTF8.GetBytes(toEncrypt);
            return EncryptBytes(toEncryptBytes, encryptionKey);
        }

        public byte[] DecryptBytes(byte[] encryptedString, byte[] encryptionKey)
        {
            using (var provider = new AesCryptoServiceProvider())
            {
                provider.Key = encryptionKey;
                provider.Mode = CipherMode.CBC;
                provider.Padding = PaddingMode.PKCS7;
                using (var ms = new MemoryStream(encryptedString))
                {
                    byte[] iv = new byte[16];
                    ms.Read(iv, 0, 16);
                    provider.IV = iv;
                    using (var decryptor = provider.CreateDecryptor())
                    {
                        using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            using (var br = new BinaryReader(cs))
                            {
                                return br.ReadBytes(encryptedString.Length - 16);
                            }
                        }
                    }
                }
            }
        }

        public string DecryptString(byte[] encryptedString, byte[] encryptionKey)
        {
            using (var provider = new AesCryptoServiceProvider())
            {
                provider.Key = encryptionKey;
                provider.Mode = CipherMode.CBC;
                provider.Padding = PaddingMode.PKCS7;
                using (var ms = new MemoryStream(encryptedString))
                {
                    byte[] iv = new byte[16];
                    ms.Read(iv, 0, 16);
                    provider.IV = iv;
                    using (var decryptor = provider.CreateDecryptor())
                    {
                        using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            using (var sr = new StreamReader(cs))
                            {
                                return sr.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }

        public List<string> EncryptToString(string[] toEncrypt, string Key)
        {
            if (toEncrypt == null || toEncrypt.Length == 0) throw new ArgumentException("EncryptToString:toEncrypt");
            if (Key == null || Key.Length == 0) throw new ArgumentException("EncryptToString:Key");
            theHash = genHash(Key);
            // sign the base text
            var flatToEncrypt = Prefix + String.Join(delim, toEncrypt) + Suffix;
            var toEncryptBytes = Encoding.UTF8.GetBytes(flatToEncrypt);
            //var encryptedBytes = EncryptBytes(toEncryptBytes, theHash);
            for (int iencrypt = 0; iencrypt < minRounds || toEncryptBytes.Length < maxBytes; iencrypt++)
            {
                toEncryptBytes = EncryptBytes(toEncryptBytes, theHash);
            }
            // sign the final text
            var result = JSONprefix + Convert.ToBase64String(toEncryptBytes) + JSONsuffix;
            return SplitString(LineLength, result);
        }

        public string[] DecryptToString(string toDecrypt, string Key)
        {
            if (toDecrypt == null) throw new ArgumentException("EncryptToString:toDecrypt:null");
            if (toDecrypt.Length < (Prefix + Suffix + delim).Length) throw new ArgumentException("EncryptToString:toDecrypt:short");
            if (!(toDecrypt.StartsWith(JSONprefix) && toDecrypt.EndsWith(JSONsuffix))) throw new ArgumentException("EncryptToString:toDecrypt:notJSON");
            if (Key == null || Key.Length == 0) throw new ArgumentException("EncryptToString:Key");
            theHash = genHash(Key);
            byte[] secreted = Convert.FromBase64String(
                                    toDecrypt.Substring(JSONprefix.Length,
                                        toDecrypt.Length - JSONprefix.Length - JSONsuffix.Length));
            String decrypted = "";
            // decrypt until you get it right, or give up
            for (int idecrypt = 0; !decrypted.StartsWith(Prefix) && secreted.Length > 16; idecrypt++)
            {
                secreted = DecryptBytes(secreted, theHash);
                try
                {
                    string foo = Encoding.UTF8.GetString(secreted);
                    if (foo.StartsWith(Prefix))
                    {
                        decrypted = foo;
                    }
                }
                catch (Exception)
                {
                }
            }
            var separated = decrypted.Split(new String[] { Prefix, delim, Suffix }, StringSplitOptions.RemoveEmptyEntries);

            return separated;
        }

        /// <summary>
        /// SplitString splits string in to List string of max lengths
        /// </summary>
        /// <param name="chunk">line length</param>
        /// <param name="input">Input string</param>
        /// <returns></returns>
        public List<string> SplitString(int chunk, string input)
        {
            List<string> list = new List<string>();
            int cycles = input.Length / chunk;

            if (input.Length % chunk != 0)
                cycles++;

            for (int ix = 0; ix < cycles; ix++)
            {
                try
                {
                    if (ix < cycles - 1)
                    {
                        list.Add(input.Substring(ix * chunk, chunk));
                    }
                    else
                    {
                        list.Add(input.Substring(ix * chunk));
                    }
                }
                catch
                {
                    list.Add(input.Substring(ix * chunk));
                }
            }
            return list;
        }

    }
}
