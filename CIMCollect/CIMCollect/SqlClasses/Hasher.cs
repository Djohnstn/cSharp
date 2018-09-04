using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Security.Cryptography;

namespace Similarity
{

    // inspired by ..., and now hardly anything like...
    // Damerau-Levenshtein algorithm in C#. 
    // https://gist.github.com/wickedshimmy/449595



    public class Hasher
    {
        /// <summary>
        ///  https://www.c-sharpcorner.com/article/hashing-passwords-in-net-core-with-tips/
        /// </summary>
        /// <param name="hashthis"></param>
        /// <returns></returns>
        public string StringHash(string hashthis)
        {
            // SHA512/SHA256 are disposable by inheritance.  
            using (var sha = SHA512.Create())
            {
                // Send a sample text to hash.  
                var hashedBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(hashthis));
                // Get the hashed string.  
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                // Print the string.   
                Console.WriteLine(hash);
                return hash;
            }
        }

        private float XSimilarity(UInt16[] hash1, UInt16[] hash2)
        {
            Array.Sort(hash1);
            Array.Sort(hash2);
            int sum = 0;
            int words = Math.Max(hash1.Length, hash2.Length);
            int run = Math.Min(hash1.Length, hash2.Length);

            for (var ix = 0; ix < run; ix++)
            {
                sum += (hash1[ix] == hash2[ix]) ? 1 : 0;
            }
            //sum += words - run; // oops, this makes it more similar when it is different!
            var ratio = 1.0f * sum / words;
            Console.WriteLine($" {ratio:N3}");
            return ratio;
        }


        private float XxSimilarity<T>(IList<T> hash1, IList<T> hash2) //where T : class
        {
            var h1 = hash1.ToArray();
            var h2 = hash2.ToArray();
            Array.Sort(h1);
            Array.Sort(h2);
            int sum = 0;
            int words = Math.Max(h1.Length, h2.Length);
            int run = Math.Min(h1.Length, h2.Length);

            for (var ix = 0; ix < run; ix++)
            {
                sum += (h1[ix].Equals(h2[ix])) ? 1 : 0;
            }
            //sum += words - run; // oops, this makes it more similar when it is different!
            var ratio = 1.0f * sum / words;
            Console.WriteLine($" {ratio:N3}");
            return ratio;
        }

        public float Similarity<T>(IList<T> hash1, IList<T> hash2) //where T : class
        {
            // if zero words in both, return 100%
            if (hash1.Count == 0 && hash2.Count == 0) return 100.0f;
            // intersect counts only distinct elements, missing duplicates
            //int match1 = hash1.Intersect(hash2).Count();
            //int match2 = hash2.Intersect(hash1).Count();
            //int matchs = Math.Max(match1, match2);
            int match1 = AllAlike(hash1, hash2);
            int match2 = AllAlike(hash2, hash1);
            int matchs = Math.Max(match1, match2);
            int words = Math.Max(hash1.Count, hash2.Count);
            //int run = Math.Min(hash1.Count, hash2.Count);

            var ratio = 100.0f * matchs / words;
            if (ratio < 10.0 && ratio > 0.0)
            {
                ;   // dummy line for a breakpoint
            }
            return ratio;
        }

        private Dictionary<T, int> HashToDictionary<T>(IList<T> hash2)
        {
            var wordCounters = new Dictionary<T, int>(hash2.Count);
            // update dictionary
            foreach (var x in hash2)
            {
                if (wordCounters.TryGetValue(x, out int value))
                {
                    wordCounters[x] = ++value;
                }
                else
                {
                    wordCounters.Add(x, 1);
                }
            }
            return wordCounters;
        }

        private int AllAlike<T>(IList<T> hash1, IList<T> hash2)
        {
            var matched = 0;
            var wordCounters = HashToDictionary(hash2);

            // find value, decrement matches available
            foreach (var h1 in hash1)
            {
                if (wordCounters.TryGetValue(h1, out int value))
                {
                    if (value > 0)
                    {
                        matched++;
                        wordCounters[h1] = --value;
                    }
                }
            }
            return matched;
        }

        private readonly char[] spacechar = new char[] { ' ' };
        private readonly string[] crlf = new string[] { "\r\n" };
        // "\r\n"

        public UInt16[] UInt16HashSql(string sql)
        {
            string[] words = sql.ToUpperInvariant().Split(spacechar, StringSplitOptions.RemoveEmptyEntries);
            //UInt16[] uints = (from x in words select UInt16Hash(x)).ToArray();
            var uints = new UInt16[words.Length];
            for (var ix = 0; ix<words.Length; ix++)
            {
                uints[ix] = UInt16Hash(words[ix]);
            }
            //var b = string.Join(".", (from x in words select UInt16Hash(x).ToString("x4")).ToArray<string>());
            //Console.Write(b);
            return uints;
        }

        public UInt32[] UInt32HashSql(string sql)
        {
            //string[] words = sql.ToUpperInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string[] words = SqlWords(sql);
            //UInt16[] uints = (from x in words select UInt16Hash(x)).ToArray();
            var uints = new UInt32[words.Length];
            for (var ix = 0; ix < words.Length; ix++)
            {
                uints[ix] = UInt32Hash(words[ix]);
            }
            return uints;
        }

        private const int AverageWordLength = 8;

        public string[] SqlWords(string sql)
        {
            string[] words = sql.ToUpperInvariant().Split(spacechar, StringSplitOptions.RemoveEmptyEntries);
            var sb = new StringBuilder(words.Length * AverageWordLength);
            bool inString = false;
            var thisSb = new StringBuilder();
            for (var ix = 0; ix < words.Length; ix++)
            {
                var w = words[ix];
                if (inString)
                {
                    thisSb.Append(" ").Append(w);
                    if (w.EndsWith("'"))
                    {
                        sb.AppendLine(thisSb.ToString());
                        inString = false;
                        w = string.Empty;
                        thisSb.Clear();
                    }
                }
                else
                {
                    if ((w.StartsWith("N'",StringComparison.CurrentCulture) || 
                        w.StartsWith("'", StringComparison.Ordinal)) && 
                        w.EndsWith("'", StringComparison.Ordinal))
                    {
                        sb.AppendLine(w);
                    }
                    else
                    {
                        if ((w.StartsWith("N'", StringComparison.CurrentCulture) || 
                            w.StartsWith("'", StringComparison.Ordinal)))
                        {
                            inString = true;
                            thisSb.Append(w);
                            w = string.Empty;
                        }
                        if (w.Length > 0)
                        {
                            sb.AppendLine(w);
                        }
                    }
                }
            }
            return sb.ToString().Split(crlf, StringSplitOptions.RemoveEmptyEntries);
        }


        // in initial testing, this was not as good as word similarity, when using a sorted dictionary with word counters
        private UInt32[] Phrase(string sql)
        {
            const int slice = 3;
            string[] words = sql.ToUpperInvariant().Split(spacechar, StringSplitOptions.RemoveEmptyEntries);
            var phrasecount = Math.Max(words.Length - slice + 1, 1);
            var phrases = new string[phrasecount];
            if (phrasecount == 1)
            {
                phrases[0] = string.Join(" ", words);
            }
            else
            {
                for (var ix = 0; ix <= words.Length - slice; ix++)
                {   // horrible, I know
                    //phrases[ix] = $"{words[ix]} {words[ix + 1]} {words[ix + 2]} {words[ix + 3]}";
                    phrases[ix] = String.Join(" ", new ArraySegment<string>(words, ix, slice));
                }
            }
            UInt32[] uints = (from x in phrases
                              select UInt32Hash(x)).ToArray<UInt32>();
            //#if DEBUG
            //            string[] a = (from x in phrases
            //                          select UInt32Hash(x).ToString("x8")).ToArray<string>();
            //            var b = string.Join(".", a);
            //            Console.Write(b);
            //#endif
            return uints;
        }

        public T UInt32Hash<T>(string hashthis)
        {
            T result; 
            using (var sha = SHA256.Create())
            {
                // Send a sample text to hash.  
                var hashedBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(hashthis));
                var hash = BitConverter.ToUInt64(hashedBytes, 0);
                result = (T)(object)hash;
            }
            return result;
        }

        public UInt32 UInt32Hash(string hashthis)
        {
            if (string.IsNullOrWhiteSpace(hashthis))
            {
                return UInt32.MaxValue;
            }
            UInt32 result;
            using (var sha = SHA256.Create())
            {
                // Send a sample text to hash.  
                var hashedBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(hashthis));
                var hash = BitConverter.ToUInt32(hashedBytes, 0);
                result = hash;
            }
            return result;
        }

        public UInt16 UInt16Hash(string hashthis)
        {
            // SHA512 is disposable by inheritance.  
            using (var sha512 = SHA512.Create())
            {
                // Send a sample text to hash.  
                var hashedBytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(hashthis));
                var hash = BitConverter.ToUInt16(hashedBytes, 0);
                // Get the hashed string.  
                //var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                // Print the string.   
                //Console.WriteLine(hash);
                return hash;
            }
        }
    }

}
