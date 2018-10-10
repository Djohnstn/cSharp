using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using StringExtensionsIsLike;
using System.Diagnostics;
using CIMCollect.SqlClasses;

namespace Similarity
{

    public class SqlAbstract
    {


        //SortedDictionary<string, Int32> HashCount = new SortedDictionary<string, Int32>();
        Dictionary<string, Int32> FeatureAbstraction = new Dictionary<string, Int32>(1020);
        public bool HasAscii { get; set; } = false;
        public string Allhash { get; set; } = string.Empty;
        public string Synopsis { get; set; } = string.Empty;

        //private static readonly Dictionary<string, string> WordHashBaseObsolete = new Dictionary<string, string>
        //{
        //    {"(", "A"},
        //    {")", "B"},
        //    {",", "C"},
        //    {";", "D"},
        //    {".", "E"},
        //    {"+", "F"},
        //    {"=", "G"},
        //    {"AND", "H"},
        //    {"AS", "I"},
        //    {"BEGIN", "J"},
        //    {"CREATE", "K"},
        //    {"ELSE", "L"},
        //    {"END", "M"},
        //    {"FROM", "N"},
        //    {"IF", "O"},
        //    {"INT", "P"},
        //    {"IS", "Q"},
        //    {"NCHAR", "R"},
        //    {"NULL", "S"},
        //    {"NVARCHAR", "T"},
        //    {"OR", "U"},
        //    {"PROCEDURE", "V"},
        //    {"SELECT", "W"},
        //    {"SET", "X"},
        //    {"WHERE", "Y"},
        //    {"0", "Z"},
        //    {"'", "AA"},
        //    {"*", "BA"},
        //    {"<", "CA"},
        //    {"<>", "DA"},
        //    {">", "EA"},
        //    {"ACTIONID", "FA"},
        //    {"ACTIVE", "GA"},
        //    {"ADDED", "HA"},
        //    {"ALIAS", "IA"},
        //    {"ASC", "JA"},
        //    {"BETWEEN", "KA"},
        //    {"BIT", "LA"},
        //    {"BLOCK", "MA"},
        //    {"BUG", "NA"},
        //    {"BY", "OA"},
        //    {"CAST", "PA"},
        //    {"CATCH", "QA"},
        //    {"CAUSEID", "RA"},
        //    {"CHARINDEX", "SA"},
        //    {"CLASS", "TA"},
        //    {"CLOSE", "UA"},
        //    {"CODE", "VA"},
        //    {"COMMENT", "WA"},
        //    {"COMMIT", "XA"},
        //    {"CONVERT", "YA"},
        //    {"COS_BALER", "ZA"},
        //    {"COUNT", "AB"},
        //    {"CURRENT", "BB"},
        //    {"CURRENTVALUE", "CB"},
        //    {"CURSOR", "DB"},
        //    {"DATE", "EB"},
        //    {"DATEADD", "FB"},
        //    {"DATECREATED", "GB"},
        //    {"DATEDIFF", "HB"},
        //    {"DATETIME", "IB"},
        //    {"DEALLOCATE", "JB"},
        //    {"DECIMAL", "KB"},
        //    {"DECLARE", "LB"},
        //    {"DEFAULT", "MB"},
        //    {"DELAYTIME", "NB"},
        //    {"DELETE", "OB"},
        //    {"DESC", "PB"},
        //    {"DESCRIPTION", "QB"},
        //    {"DISTINCT", "RB"},
        //    {"DOCUMENTURL", "SB"},
        //    {"ERRORHANDLER", "TB"},
        //    {"EXEC", "UB"},
        //    {"EXECUTE", "VB"},
        //    {"EXISTS", "WB"},
        //    {"EXTENDEDSTATUS", "XB"},
        //    {"FETCH", "YB"},
        //    {"FIX", "ZB"},
        //    {"FLOAT", "AC"},
        //    {"FOR", "BC"},
        //    {"FORMAT", "CC"},
        //    {"FULL_PATH", "DC"},
        //    {"GETDATE", "EC"},
        //    {"GMN", "FC"},
        //    {"GMR", "GC"},
        //    {"GOTO", "HC"},
        //    {"ID", "IC"},
        //    {"IN", "JC"},
        //    {"INNER", "KC"},
        //    {"INSERT", "LC"},
        //    {"INSTRUCTIONS", "MC"},
        //    {"INTO", "NC"},
        //    {"ITEMID", "OC"},
        //    {"JOIN", "PC"},
        //    {"LEFT", "QC"},
        //    {"LEN", "RC"},
        //    {"LIKE", "SC"},
        //    {"LIMIT", "TC"},
        //    {"LINK", "UC"},
        //    {"LOGICAL", "VC"},
        //    {"LOGID", "WC"},
        //    {"MIN", "XC"},
        //    {"MONTH", "YC"},
        //    {"MUST", "ZC"},
        //    {"NEXT", "AD"},
        //    {"NOCOUNT", "BD"},
        //    {"NOLOCK", "CD"},
        //    {"NOT", "DD"},
        //    {"NOW", "ED"},
        //    {"N'Y'", "FD"},
        //    {"OF", "GD"},
        //    {"ON", "HD"},
        //    {"OPEN", "ID"},
        //    {"OPERAND", "JD"},
        //    {"OPERATOR", "KD"},
        //    {"OPERATORID", "LD"},
        //    {"OPTIONS", "MD"},
        //    {"ORDER", "ND"},
        //    {"OUTER", "OD"},
        //    {"OUTPUT", "PD"},
        //    {"PARAMETERLIST", "QD"},
        //    {"PARENTCAUSEID", "RD"},
        //    {"PASSED", "SD"},
        //    {"PRINT", "TD"},
        //    {"PRIORITY", "UD"},
        //    {"PROC", "VD"},
        //    {"RAISERROR", "WD"},
        //    {"REPLACE", "XD"},
        //    {"RESETTIME", "YD"},
        //    {"RETRYCOUNT", "ZD"},
        //    {"RETURN", "AE"},
        //    {"ROLLBACK", "BE"},
        //    {"RTRIM", "CE"},
        //    {"RULEID", "DE"},
        //    {"RULES", "EE"},
        //    {"SAP", "FE"},
        //    {"SEQUENCE", "GE"},
        //    {"SORT", "HE"},
        //    {"SP_EXECUTESQL", "IE"},
        //    {"STATUS", "JE"},
        //    {"STATUSID", "KE"},
        //    {"STOREDPROCEDURE", "LE"},
        //    {"STR", "ME"},
        //    {"TAGNAME", "NE"},
        //    {"THIS", "OE"},
        //    {"TIME", "PE"},
        //    {"TIMESTAMP", "QE"},
        //    {"TOP", "RE"},
        //    {"TRANSACTION", "SE"},
        //    {"TRY", "TE"},
        //    {"TYPE", "UE"},
        //    {"UNION", "VE"},
        //    {"UPDATE", "WE"},
        //    {"USE", "XE"},
        //    {"USERID", "YE"},
        //    {"VALUE", "ZE"},
        //    {"VALUES", "AF"},
        //    {"WHILE", "BF"},
        //    {"WITH", "CF"},
        //    {"--", "DF"},
        //    {"DBO", "EF"},
        //    {"CHAR", "FF"},
        //    {"VARCHAR", "GF"},

        //    {"LOG", "HF"},
        //    {"MAIL", "IF"},
        //    {"AREA", "JF"},
        //    {"MESA", "KF"},
        //    {"ALLOW", "LF"},
        //    {"ITWRS", "MF"},
        //    {"JUNE", "NF"},
        //    {"ETC", "OF"},
        //    {"TO", "PF"},
        //    {"PASS", "QF"},


        //    { "@PARAMETERLIST", "ZF"}
        //};

        static Dictionary<string, string> WordHash = new Dictionary<string, string>(SqlWordDictionary.WordHashBase);

        public float Similarity(string otherAbstract)
        {
            var hash2 = FromString(otherAbstract);

            return Similarity(this.FeatureAbstraction, hash2);
        }

        public float Similarity(Dictionary<string, Int32> hash1, Dictionary<string, Int32> hash2) //where T : class
        {
            // if zero words in both, return 100%
            if (hash1.Count == 0 && hash2.Count == 0) return 1.0f;

            var match1 = AlikeRatio(hash1, hash2);
            var match2 = AlikeRatio(hash2, hash1);
            var matchs = Math.Max(match1, match2);

            var ratio = 1.0f * matchs;
            return ratio;
        }

        private int GetHashValueOrDefault(Dictionary<string, int> x, string key, int defaultvalue)
        {
            if (x.TryGetValue(key, out int value))
            {
                return value;
            }
            else
            {
                return defaultvalue;
            }
        }

        private float AlikeRatio(Dictionary<string, Int32> hash1, Dictionary<string, Int32> hash2)
        {
            var basewords = hash1.Sum(x => x.Value);
            var otherwords = hash2.Sum(x => x.Value);
            var maxwords = Math.Max(basewords, otherwords);

            var matched = hash1.Sum(x => Math.Min(x.Value, GetHashValueOrDefault(hash2, x.Key, 0)));

            return 1.0f * matched / maxwords;
        }

        public SqlAbstract(bool fullBuild = true)
        {
            HasAscii = false;
            Allhash = string.Empty;
            if (fullBuild)
            {
                Reset();
            }
        }

        public SqlAbstract(string abstracted)
        {
            //Reset();
            FeatureAbstraction = FromString(abstracted);
        }

        public string Abstract(string[] rawSql)
        {
            var basestring = SqlCleaner.Clean(rawSql, HideAscii: false);
            HasAscii = AddScript(basestring);
            return this.ToString();
        }


        private void Reset()
        {
            FeatureAbstraction.Clear();
            //WordHash.Clear();
            if (WordHash.Count == 0)
            {
                WordHash = new Dictionary<string, string>(SqlWordDictionary.WordHashBase);
            }
        }

        public void Clear()
        {
            FeatureAbstraction.Clear();
        }

        private Dictionary<string, Int32> FromString(string abstracted)
        {
            var ahashCount = new Dictionary<string, Int32>();
            var cx = (abstracted + ",").ToCharArray();  // ensure character array ends with a comma
            var len = cx.Length;

            var sb = new StringBuilder(32);
            bool charmode = true;
            int count = 0;
            foreach (var c in cx)
            {
                if (charmode && !(c == ','))
                {   // character mode
                    if (c.Equals('.'))
                    {
                        charmode = false;
                        count = 0;
                    }
                    else
                    {
                        sb.Append(c);
                    }
                }
                else
                {   // number mode
                    if (c.Equals(','))
                    {
                        charmode = true;
                        if (sb.Length > 0)
                        {
                            ahashCount[sb.ToString()] = count;
                            sb.Clear();
                        }
                        count = 0;
                    }
                    else
                    {
                        if (int.TryParse(c.ToString(), out int result))
                        {
                            count = count * 10 + result;
                        }
                    }
                }
            }
            return ahashCount;
        }


        public override string ToString()
        {
            var sb = new StringBuilder();
            var sortedHashCount = new SortedDictionary<string, int>(FeatureAbstraction);
            foreach (var part in sortedHashCount)
            {
                sb.Append(part.Key);
                sb.Append(".");
                var count = part.Value;
                if (count > 99)
                {
                    sb.Append("99");
                }
                else if (count < 1)
                {
                    sb.Append("0");
                }
                else
                {
                    sb.Append(count.ToString());
                }
                sb.Append(",");
            }
            return sb.ToString();
        }

        //public string ToGZ()
        //{
        //    return "$GZ:" + MemoryZip.Zip(this.ToString());
        //}

        //public string FromGZ(string zipped)
        //{
        //    return MemoryZip.UnZip(zipped);
        //}


        public bool AddScript(string sql)
        {
            bool HasAscii = false;
            //var h = new Hasher();
            var synop = new CIMCollect.SqlSynopsis();
            var wordlist = SqlWords(sql);
            foreach (var word in wordlist)
            {
                if (word.Length > 0)
                {
                    if (Add(word)) HasAscii = true;
                }
                synop.Append(word);
            }
            synop.Finish();
            Synopsis = synop.ToString();    // get synopsis
            Debug.WriteLine($"$ {Synopsis}");

            BigAdd(String.Join(" ", wordlist.ToArray()));
            return HasAscii;
        }

        public bool Add(string word)
        {
            bool HasAscii = false;
            if (string.CompareOrdinal(word, "CHAR") == 0 || string.CompareOrdinal(word, "VARCHAR") == 0)
            {
                HasAscii = true;
            }
            var hashed = HashValue(word);
            UpdateHashCount(hashed);
            return HasAscii;
        }

        private void BigAdd(string word)
        {   // this hash is needed at the first of the abstract string, put there by using "!", which is first printed ascii char
            var hashed = BigHash(word, FixupNeeded: false, prefix: "!");
            Allhash = hashed;   // save the hash separately
            UpdateHashCount(hashed);
        }

        private void UpdateHashCount(string hashed)
        {
            if (FeatureAbstraction.TryGetValue(hashed, out int value))
            {
                if (value < 99)
                {
                    FeatureAbstraction[hashed] = value + 1;
                }
            }
            else
            {
                FeatureAbstraction[hashed] = 1;
            }
            //var count = HashCount.GetValueOrDefault(hashed, 0);
            //if (count < 9)
            //{
            //    HashCount[hashed] = count + 1;
            //}
            //else
            //{
            //    ;
            //}
        }


        private string HashValue(string wordparam)
        {
            var code = string.Empty;
            var word = wordparam.Trim(); //.ToUpperInvariant(); // assume uppercase done elsewhere, no need to redo
            if (String.IsNullOrWhiteSpace(word))
            {
                return string.Empty;
            }
            // if hash value is known, use it
            if (WordHash.TryGetValue(word, out string value))
            {
                return value;
            }
            else
            {
                var hashedValue = string.Empty;
                // if word is all alphabetic, fastest to check word length before IsLetter or IsDigit
                if (word.Length < 6 && word.All(char.IsLetter))
                {
                    hashedValue = ShortHash(word);
                }
                else if (word.Length < 9 && word.All(char.IsDigit))
                {   
                    if (word.Length < 5)
                    {
                        hashedValue = word; // just copy small numbers forward as their own hash
                    }
                    else
                    {
                        // 8 digits is less than 26 * 52 ^ 4 = 190_102_016
                        hashedValue = DigitHash(word);
                    }
                }
                else
                {
                    hashedValue = BigHash(word);
                }
                WordHash.Add(word, hashedValue);
                return hashedValue;
            }
        }


        private string ShortHash(string word)
        {
            return (word.Trim());
            // original plan was to mark short words with "$" + word as the hash value. not any more. if needed, bring this code back
            //var sb = new StringBuilder(16);
            //sb.Append("$");
            //sb.Append(word.Trim());
            //return sb.ToString();
        }

        private string DigitHash(string word)
        {
            if (UInt64.TryParse(word, out ulong result))
            {
                return UInt64ToBase62(result, 5, ""); // original plan - mark number hashes with "#" + hash(number)
            }
            else
            {
                return BigHash(word);   // oops, number doesn't parse? really? ok, use bigHash of string!
            }
        }

        private static string UInt64ToBase62(ulong result, int maxHashLen, string prefix = "")
        {
            const string firstChar = "abcdefghijklmnopqrstuvwxyz0123456789";
            const string otherChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            const ulong firstCharLength = 36;
            const ulong otherCharsLength = 62;
            var sb = new StringBuilder(24);
            sb.Append(prefix);
            if (result == 0)
            {
                sb.Append("a"); // value of zero
            }
            result = GetBaseNNDigit(result, firstChar, firstCharLength, sb);
            for (var ix = 0; result > 0 && ix < maxHashLen; ix++)
            {
                result = GetBaseNNDigit(result, otherChars, otherCharsLength, sb);
            }
            return sb.ToString();
        }

        private static ulong GetBaseNNDigit(ulong result, string charset, ulong charsetLength, StringBuilder sb)
        {
            int h1 = Convert.ToInt32(result % charsetLength);
            sb.Append(charset[h1]);
            result /= charsetLength;
            return result;
        }

        private string BigHash(string word, bool FixupNeeded = true, string prefix = "")
        {
            string fixedWord;
            int maxHashLen = 4;
            if (FixupNeeded)
            {
                if ((word.EndsWithDigit() && word.All(c => char.IsLetterOrDigit(c) || c == '@')) || (word.Contains('=') && word.Contains('\'')))
                {
                    fixedWord = FixTrailingDigits(word);
                }
                else
                {
                    fixedWord = word;
                }
                //var result = UInt64Hash(word);            // Expensive SHA hash
                maxHashLen = fixedWord.Length < 8 ? 4 :      // 406,250 hash values
                                                                 //word.Length < 7 ? 4 :
                                                                 //word.Length < 12 ? 5 :
                                                                 //word.Length < 16 ? 6 :
                                                                 //word.Length < 22 ? 6 :
                                 6;                         // 5 = 190,102,016 values
                                                            // 6 is way more
            }
            else
            {
                fixedWord = word;
                maxHashLen = 12;
            }

            var hashValue = UInt64SipHash(fixedWord);           // siphash is 10% of time of SHA hash, still good hash
            return UInt64ToBase62(hashValue, maxHashLen, prefix);
        }

        private string BigSlowHash(string word)
        {
            var result = UInt64Hash(word);            // Expensive SHA hash
            return UInt64ToBase62(result, 12, "!");
        }

        public ulong UInt64Hash(string hashthis)
        {
            if (string.IsNullOrWhiteSpace(hashthis))
            {
                return UInt64.MaxValue;
            }
            ulong result;
            using (var sha = SHA256.Create())
            {
                // Send a sample text to hash.  
                var hashedBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(hashthis));
                var hash = BitConverter.ToUInt64(hashedBytes, 0);
                result = hash;
            }
            return result;
        }

        // siphash key random numbers from https://www.random.org/cgi-bin/randbyte?nbytes=16&format=d
        private readonly byte[] siphashKey = {107, 81, 199, 53, 215, 234, 98, 91, 180, 99, 126, 188, 80, 252, 99, 41 };

        public ulong UInt64SipHash(string hashthis)
        {

            if (string.IsNullOrWhiteSpace(hashthis))
            {
                return UInt64.MaxValue;
            }

            ulong result;
            var prf = new SipHash.SipHash(siphashKey);
            var tag = prf.Compute(Encoding.UTF8.GetBytes(hashthis));
            //byte[] bytes = BitConverter.GetBytes(tag);
            //result = BitConverter.ToUInt64(bytes, 0);
            result = (ulong)tag;   // will this crash?
            //using (var sha = SHA256.Create())
            //{
            //    // Send a sample text to hash.  
            //    var hashedBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(hashthis));
            //    var hash = BitConverter.ToUInt64(hashedBytes, 0);
            //    result = hash;
            //}
            return result;
        }

        private static string FixTrailingDigits(string s)
        {
            int ixeq;
            if (s.Length < 3)
            {
                return s;
            }
            char[] c = s.ToCharArray();
            int cend = c.Length - 1;
            if (char.IsDigit(c[cend]) && (char.IsLetter(c[0]) || c[0] == '@'))
            {
                //c[cend] = '9';
                //break;
                // ok, this is a sql variable ending in digits
                for (int ix = cend; ix >= cend - 2; ix--)
                {
                    if (Char.IsDigit(c[ix]))
                    {
                        c[ix] = '9';
                        //break;
                    }
                    else
                    {
                        break;
                    }
                }
                return new string(c);
            }
            else if ((ixeq = s.IndexOf('=')) > 0  && ((c[0] == '\'') || (c[0] == 'N' && c[1] == '\'')))
            {
                // ok, this is a sql string containing = and may contain numbers
                for (int ix = ixeq - 1; ix >= 0; ix--)
                {
                    if (Char.IsWhiteSpace(c[ix]))
                    {
                        continue;
                    }
                    if (Char.IsDigit(c[ix]))
                    {
                        c[ix] = '9';
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
                return new string(c);
            }
            else
            {
                return s;
            }
        }
        private const int AverageWordLength = 8;
        private readonly char[] spacechar = new char[] { ' ' }; 

        private List<string> SqlWords(string sql)
        {
            //string[] words = sql.ToUpperInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string[] words = sql.Split(spacechar, StringSplitOptions.RemoveEmptyEntries);
            //var sb = new StringBuilder(words.Length * AverageWordLength);
            var sList = new List<string>(words.Length);
            bool inString = false;
            var thisSb = new StringBuilder();
            for (var ix = 0; ix < words.Length; ix++)
            {
                var w = words[ix];
                if (inString)
                {
                    thisSb.Append(" ").Append(w);
                    if (w.EndsWith("'", StringComparison.Ordinal))
                    {
                        //sb.AppendLine(thisSb.ToString());
                        sList.Add(thisSb.ToString());
                        inString = false;
                        w = string.Empty;
                        thisSb.Clear();
                    }
                }
                else
                {
                    var wStartsWithNQ = w.StartsWith("N'", StringComparison.Ordinal);
                    var wStartsWithQ = w.StartsWith("'", StringComparison.Ordinal);
                    var wEndsWithQ = w.EndsWith("'", StringComparison.Ordinal);
                    if ((wStartsWithNQ || wStartsWithQ) && wEndsWithQ)
                    {
                        //sb.AppendLine(w);
                        sList.Add(w);
                    }
                    else
                    {
                        if (wStartsWithNQ || wStartsWithQ)
                        {
                            inString = true;
                            thisSb.Append(w);
                            w = string.Empty;
                        }
                        if (w.Length > 0)
                        {
                            //sb.AppendLine(w);
                            sList.Add(w);
                        }
                    }
                }
            }
            return sList;
            //return sb.ToString().Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
        }

    }
}
