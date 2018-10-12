using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Diagnostics;

namespace CIMCollect.SqlClasses
{
    public class BinaryIntegerList
    {

        public enum ListType : byte
        {
            IntegerList = 1,
            RLE = 2,
            SHA = 3
        }

        SortedSet<int> integers = new SortedSet<int>();
        private int lastValue = 0;  // start an RLE with zero
        private List<byte> rle = new List<byte>();
        byte[] returned;

        public int Add(int value)
        {
            integers.Add(value);
            return value;
        }

        public byte[] ToRLE()
        {
            //bool success = false;
            if (integers.Count <= 4)
            {
                // might just return the integer array
                var intarray = integers.ToArray();
                int sizeOfArray = integers.Count * sizeof(int);
                byte[] temp = new byte[sizeOfArray + 1];
                Buffer.BlockCopy(intarray, 0, temp, 0, sizeOfArray);
                // think about this:
                if (!BitConverter.IsLittleEndian) Array.Reverse(temp);
                byte[] returns = new byte[sizeOfArray + 1];
                returns[0] = (byte)ListType.IntegerList;    // 0 = flag raw
                Buffer.BlockCopy(temp, 0, returns, 1, sizeOfArray);
                returned = returns;
                return returns;
            }
            else
            {
                // try compressing the array
                rle.Add((byte)ListType.RLE);   // 1 = flag as RLE
                foreach (int ix in integers)
                {
                    Append(ix);
                }
                HandleRunOfones(0, finish: true);
                if (rle.Count <= 65)
                {
                    returned = rle.ToArray();
                    return returned;
                }
                else
                {   // hash the shorter, denser list of bytes
                    if (rle.Count > integers.Count * sizeof(int))
                    {
                        // return hash of integer array
                        returned = IntListCheckSum();
                    }
                    else
                    {
                        // return hash of RLE byte array
                        returned = ByteListCheckSum(rle);
                    }
                    return returned;
                }
            }
        }

        private byte[] IntListCheckSum()
        {
            int[] intArray = integers.ToArray();
            byte[] result = new byte[intArray.Length * sizeof(int)];
            Buffer.BlockCopy(intArray, 0, result, 0, result.Length);
            var checksum = new byte[65];    // byte 0 is flag as hash = 3
            using (var sha = new SHA512Managed())
            {
                checksum[0] = (byte)ListType.SHA;
                Array.Copy(sha.ComputeHash(result), 0, checksum, 1, 64);
            }
            return checksum;
        }

        private byte[] ByteListCheckSum(List<byte> byteList)
        {
            //int[] intArray = byteList.ToArray();
            var byteArray = byteList.ToArray();
            //byte[] result = new byte[intArray.Length * sizeof(int)];
            //Buffer.BlockCopy(intArray, 0, result, 0, result.Length);
            var checksum = new byte[65];    // byte 0 is flag as hash = 3
            using (var sha = new SHA512Managed())
            {
                checksum[0] = (byte)ListType.SHA;
                Array.Copy(sha.ComputeHash(byteArray), 0, checksum, 1, 64);
            }
            return checksum;
        }

        private void Append(int value)
        {
            if (value <= lastValue) throw new ArgumentOutOfRangeException("New value not greater than last value");

            int diff = value - lastValue;
            HandleRunOfones(diff);
            if (diff > 1)
            {
                byte[] bytes = DiffToBytes(diff);
                rle.AddRange(bytes);
            }
            lastValue = value;
        }

        private int runOfOnes = 0;
        private void HandleRunOfones(int diff, bool finish = false)
        {
            if (diff == 1)
            {
                if (runOfOnes > 19)
                {
                    UpdateRleForOnes(1);
                }
                else
                {
                    runOfOnes++;
                }
            }
            else if (runOfOnes > 0)
            {
                if (finish)
                {
                    UpdateRleForOnes(0);
                }
                else
                {
                    if (runOfOnes == 1)
                    {
                        // not a run, a singleton
                        rle.Add((byte)101);
                        runOfOnes = 0;
                    }
                    else if (runOfOnes > 0 || finish)
                    {
                        UpdateRleForOnes(0);
                    }
                }
            }
        }

        private void UpdateRleForOnes(int newRun)
        {
            byte foo = (byte)(200 + runOfOnes);
            rle.Add(foo);
            runOfOnes = newRun;
        }

        byte[] DiffToBytes(int value)
        {
            //Debug.WriteLine(int.MaxValue);
            byte[] bytes = new byte[2 * sizeof(int) + 1]; // probably not right, but its a start
            int len = 0;
            while (value > 0)
            {
                int x = value % 100;    // peel off lower bits
                value /= 100;           // save upper bits
                if (value <= 0)
                {
                    x += 100;           // x > 99 = end of number
                }
                bytes[len] = (byte)x;
            }
            byte[] returnBytes = new byte[len+1];
            Array.Copy(bytes, returnBytes, len+1);
            return returnBytes;
        }

        public override string ToString()
        {
            if (returned == null)
            {
                var sb = new StringBuilder(integers.Count * 6);
                sb.Append("New:");
                int ix = 0;
                foreach (var value in integers)
                {
                    if (ix++ > 0) sb.Append(",");
                    sb.Append(value.ToString());
                }
                return sb.ToString();
            }
            else
            {
                var type = (ListType)returned[0];
                switch (type)
                {
                    case ListType.IntegerList:
                        int sizeOfArray = returned.Length / sizeof(int);
                        var intarray = new int[sizeOfArray];
                        Buffer.BlockCopy(returned, 1, intarray, 0, returned.Length - 1);
                        //if (!BitConverter.IsLittleEndian) Array.Reverse(temp);
                        var sb = new StringBuilder(intarray.Length * 6);
                        sb.Append("Int:");
                        int ix = 0;
                        foreach (var value in intarray)
                        {
                            if (ix++ > 0) sb.Append(",");
                            sb.Append(value.ToString());
                        }
                        return sb.ToString();
                        break;
                    case ListType.SHA:
                        var bytes = new byte[returned.Length - 1];
                        Buffer.BlockCopy(returned, 1, bytes, 0, bytes.Length);
                        return "SHA:" + BitConverter.ToString(bytes);
                        break;
                    case ListType.RLE:
                        int oldvalue = 0;
                        int thisvalue = 0;
                        var sbr = new StringBuilder(returned.Length * 3);
                        for (int ixr = 1; ixr < returned.Length; ixr++)
                        {
                            int cur = returned[ixr];
                            if (cur > 199)
                            {
                                //sbr.Append(",");
                                thisvalue = oldvalue + cur - 200;
                                if (oldvalue == 0) sbr.Append("1");
                                sbr.Append("..");
                                oldvalue = thisvalue;
                                sbr.Append(oldvalue);
                                thisvalue = 0;
                            }
                            else if (cur > 100)
                            {
                                thisvalue = thisvalue * 100 + cur - 100;
                                oldvalue += thisvalue;
                                sbr.Append(",");
                                sbr.Append(oldvalue);
                                thisvalue = 0;
                            }
                            else
                            {
                                thisvalue = thisvalue * 100 + cur;
                            }

                        }
                        return "RLE:" + sbr.ToString();
                        //return "RLE:" + BitConverter.ToString(returned) + ":" + sbr.ToString();
                        break;
                    default:
                        return "???:" + BitConverter.ToString(returned);
                        break;
                }
            }
        }

    }
}
