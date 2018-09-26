using System;
using System.Text;
using System.IO;
//using System.IO.Compression;
using System.Data;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

namespace CIMSave
{
    class HashSig : IEquatable<HashSig>     
    {
        private ulong l0;
        private ulong l1;
        private ulong l2;
        private ulong l3;

        private const ulong mask = 0x7fffffff;

        private void MapBytes(byte[] bytes)
        {
            l0 = BitConverter.ToUInt64(bytes, 0);
            l1 = BitConverter.ToUInt64(bytes, 8);
            l2 = BitConverter.ToUInt64(bytes, 16);
            l3 = BitConverter.ToUInt64(bytes, 24);
        }
        private string X(UInt64 v)
        {
            return v.ToString("x16");
        }
        private byte[] HashBytesOfString(string randomString)
        {
            using (var crypt = new System.Security.Cryptography.SHA256Managed())
            {
                return crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            }
        }
        private byte[] HashBytesOfBytes(byte[] bytes)
        {
            using (var crypt = new System.Security.Cryptography.SHA256Managed())
            {
                return crypt.ComputeHash(bytes);
            }
        }
        private byte[] HashDataRow(DataRow row)
        {
            var column = row.Table.Columns;
            var sb = new StringBuilder();
            for (int xcol = 0; xcol < row.ItemArray.Length; xcol++)
            {
                var colName = column[xcol].ColumnName;
                if (xcol < 2 || colName == "id" || colName.StartsWith("_"))
                {   // skip added data columns, including rowid and serverid columns
                }
                else
                {
                    if (row.IsNull(xcol))
                    {
                        sb.Append("\x1a"); // \x1a = ASCII SUB char (Substitute)
                    }
                    else
                    {
                        var colValue = row[xcol];
                        sb.Append(colValue.ToString());
                    }
                    sb.Append('\t');
                }
            }
            var scol = sb.ToString();
            // not sure if this is proper...
            //var bcol = scol.Length < 80 ? HashBytesOfString("\t\t" + scol) : Zip(scol);
            //var hash = HashBytesOfBytes(bcol);
            var hash = HashBytesOfString(scol);
            return hash;
        }
        // Convert an object to a byte array
        private byte[] ObjectToByteArray(Object obj)
        {
            if (obj == null) return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public HashSig(Byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }
            if (bytes.Length != 32)
            {
                throw new ArgumentException("byte array wrong size " + nameof(bytes));
            }
            MapBytes(bytes);
        }

        public HashSig(string randomString)
        {
            if (randomString == null)
            {
                throw new ArgumentNullException(nameof(randomString));
            }
            byte[] bytes = HashBytesOfString(randomString);
            MapBytes(bytes);
        }

        public HashSig(DataRow dataRow)
        {
            if (dataRow == null)
            {
                throw new ArgumentNullException(nameof(dataRow));
            }
            byte[] bytes = HashDataRow(dataRow);
            MapBytes(bytes);
        }

        public HashSig()
        {
            l0 = l1 = l2 = l3 = 0;
            //var zeroArray = new Byte[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            //                        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
            //MapBytes(zeroArray);
        }

        public bool Equals(HashSig h2)
        {
            // Again just optimization
            if (ReferenceEquals(null, h2)) return false;
            if (ReferenceEquals(this, h2)) return true;

            // Actually check the type, should not throw exception from Equals override
            if (h2.GetType() != this.GetType()) return false;

            if (l0 == h2.l0
                 && l1 == h2.l1
                 && l2 == h2.l2
                 && l3 == h2.l3) return true;
            return false;
        }

        public override bool Equals(object h2)
        {
            // Again just optimization
            if (ReferenceEquals(null, h2)) return false;
            if (ReferenceEquals(this, h2)) return true;

            // Actually check the type, should not throw exception from Equals override
            if (h2.GetType() != this.GetType()) return false;

            return Equals((HashSig)h2);
        }

        public override int GetHashCode()
        {
            return (int)((l0 ^ l1 ^ l2 ^ l3) & mask);
        }

        public override string ToString()
        {
            return $"{{{X(l0)}-{X(l1)}-{X(l2)}-{X(l3)}}}";
        }

        //// started from https://stackoverflow.com/questions/7343465/compression-decompression-string-with-c-sharp
        //private static void CopyTo(Stream src, Stream dest)
        //{
        //    byte[] bytes = new byte[4096];
        //    int cnt;
        //    while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
        //    {
        //        dest.Write(bytes, 0, cnt);
        //    }
        //}
        //public static byte[] Zip(string str)
        //{
        //    var bytes = Encoding.UTF8.GetBytes(str);

        //    using (var msi = new MemoryStream(bytes))
        //    using (var mso = new MemoryStream())
        //    {
        //        using (var gs = new GZipStream(mso, CompressionMode.Compress))
        //        {
        //            //msi.CopyTo(gs);
        //            CopyTo(msi, gs);
        //        }
        //        return mso.ToArray();
        //    }
        //}
        //public static string Unzip(byte[] bytes)
        //{
        //    using (var msi = new MemoryStream(bytes))
        //    using (var mso = new MemoryStream())
        //    {
        //        using (var gs = new GZipStream(msi, CompressionMode.Decompress))
        //        {
        //            CopyTo(gs, mso);
        //        }
        //        return Encoding.UTF8.GetString(mso.ToArray());
        //    }
        //}
    }

    class HashTable
    {

        private Dictionary<int, HashSig> IdToHash = new Dictionary<int, HashSig>();

        private Dictionary<HashSig, int> HashToID = new Dictionary<HashSig, int>();

        public bool AddRow(DataRow row, int rownumber)
        {
            var sig = new HashSig(row);
            int id;
            if (row["id"] == System.DBNull.Value)
            {
                //id = rownumber;
                throw new Exception(  "oops id is null in HashTable.add");
            }
            else
            {
                id = row.Field<int>("id");
            }
            return Add(sig, id);
        }

        public bool Add(HashSig hashSig, int id)
        {
            if (!HashToID.ContainsKey(hashSig))
            {
                IdToHash.Add(id, hashSig);
                HashToID.Add(hashSig, id);

                var foo = IdToHash.Keys;
                return true;
            }
            return false;
        }

        public Dictionary<int, HashSig>.KeyCollection Ids() => IdToHash.Keys;

        public Dictionary<HashSig, int>.KeyCollection Hashs() => HashToID.Keys;

        public bool SigExists(HashSig Sig) => HashToID.ContainsKey(Sig);
        public bool IdExists(int id) => IdToHash.ContainsKey(id);

        public int RecordID(HashSig Sig) => HashToID[Sig];
        public HashSig HashValue(int Id) => IdToHash[Id];

        public bool TryGetID(HashSig sig, out int id)
        {
            bool found;
            if (SigExists(sig))
            {
                id = RecordID(sig);
                found = true;
            }
            else
            {
                id = 0;
                found = false;
            }
            return found;
        }

        public bool TryGetHash(int id, out HashSig sig)
        {
            bool found;
            if (IdExists(id))
            {
                sig = HashValue(id);
                found = true;
            }
            else
            {
                sig = null; // or new HashSig() ?
                found = false;
            }
            return found;
        }

        //public IEnumerator GetEnumerator() => ((IEnumerable)IdToHash).GetEnumerator();

    }


}
