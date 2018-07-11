using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetQuickChat1
{
    class SmallTOTP
    {
        int _interval = 30;                         // iee standard interval
        int _size = 1000000;                        // six digit values
        string _secret = "75a463c7-f2ee-45d1-97c3-5e8cad582580:" + Environment.MachineName;
        // do something to have a default key of some type




        public SmallTOTP(string Secret, int Interval = 30, int MaxKey = 1000000)
        {
            if (Interval != 0) _interval = Interval;
            if (MaxKey > 100) _size = MaxKey;
            if (Secret.Length > 0) _secret = Secret;

            int test = this.hashStamp(_secret);


        }
        // yes, MD5 is old, but that should give backwards compatability
        private System.Security.Cryptography.MD5CryptoServiceProvider md5Provider =
                new System.Security.Cryptography.MD5CryptoServiceProvider();
        // the database is usually set to Latin1_General_CI_AS which is codepage 1252
        private System.Text.Encoding encoding = System.Text.Encoding.UTF8;

        // based loosely on TOTP / HOTP, using md5 instead of sha-1 to allow for older systems
        // https://en.wikipedia.org/wiki/HMAC-based_One-time_Password_Algorithm#Definition
        private int SQLStringToHash(int timestamp, string sourceString, int modulo = 100000000)
        {
            const int part1 = 0x5c5c5c5c;
            const int part2 = 0x36363636; // is this correct? we are losing some of the bits? not with xor
            byte[] one = BitConverter.GetBytes(timestamp ^ part2);
            byte[] two = encoding.GetBytes(sourceString);
            byte[] three = BitConverter.GetBytes(timestamp ^ part1);
            int length = one.Length + two.Length;
            byte[] sum = new byte[length];
            one.CopyTo(sum, 0);
            two.CopyTo(sum, one.Length);

            var md5BytesA = md5Provider.ComputeHash(sum);
            int length2 = three.Length + md5BytesA.Length;
            byte[] sum2 = new byte[length2];
            three.CopyTo(sum2, 0);
            md5BytesA.CopyTo(sum2, three.Length);

            var md5Bytes = md5Provider.ComputeHash(sum2);

            var nib = md5Bytes[0] % 16;
            var b0 = (nib + 3) % 16;
            var b1 = (nib + 2) % 16;
            var b2 = (nib + 1) % 16;
            var b3 = (nib + 0) % 16;
            var result = BitConverter.ToInt32(new byte[] { md5Bytes[b0], md5Bytes[b1], md5Bytes[b2], md5Bytes[b3] }, 0);
            //var result0 = BitConverter.ToInt32(new byte[] {md5Bytes[15], md5Bytes[14], md5Bytes[13], md5Bytes[12]}, 0);
            //var result1 = BitConverter.ToInt32(new byte[] { md5Bytes[11], md5Bytes[10], md5Bytes[9], md5Bytes[8] }, 0);
            //var result2 = BitConverter.ToInt32(new byte[] { md5Bytes[7], md5Bytes[6], md5Bytes[5], md5Bytes[4] }, 0);
            //var result3 = BitConverter.ToInt32(new byte[] { md5Bytes[3], md5Bytes[2], md5Bytes[1], md5Bytes[0] }, 0);
            //var result = result0 ^ result1 ^ result2 ^ result3;
            int hash = result & 0x7FFFFFFF;
            if (modulo != 0) hash %= modulo;
            return hash;
            //else return (result ^ 0x7FFFFFFF) % modulo;
        }

        //const char asciiTab = '\t';

        private int HOTPTimeStamp(int interval)
        {
            var deltaTime = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt32(deltaTime.TotalSeconds) / interval;
        }

        public int MsgHash()
        {
            int timeStamp1 = HOTPTimeStamp(this._interval);

            return (hashStamp(_secret, timeStamp1));
        }

        // generate hash codes that change every 32 seconds, check intervals before and after in case of a mismatch
        public bool ValidMsgHash(string msgHash)
        {
            int msgHashCode = -1;   // not a valid result
            var hashOk = int.TryParse(msgHash, out msgHashCode);
            if (!hashOk) return false;
            int timeStamp1 = HOTPTimeStamp(this._interval);

            if (hashStamp(_secret, timeStamp1) == msgHashCode) return true;
            if (hashStamp(_secret, timeStamp1 - 1) == msgHashCode) return true; // try previous code
            if (hashStamp(_secret, timeStamp1 + 1) == msgHashCode) return true; // try next code

            return false;
        }

        private int hashStamp(string baseString, int timeStamp = 0)
        {
            if (timeStamp == 0) return SQLStringToHash(this.HOTPTimeStamp(this._interval), baseString, _size);
            else return SQLStringToHash(timeStamp, baseString, _size);
        }
    }
}
