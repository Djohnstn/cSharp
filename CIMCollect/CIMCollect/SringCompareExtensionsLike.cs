using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// http://www.blackbeltcoder.com/Articles/net/implementing-vbs-like-operator-in-c

namespace StringExtensionsIsLike
{
    static class StringCompareExtensions
    {
        /// <summary>
        /// Implement's VB's Like operator logic.
        /// </summary>
        public static bool IsLike(this string s, string pattern)
        {
            // Characters matched so far
            int matched = 0;

            // Loop through pattern string
            for (int i = 0; i < pattern.Length;)
            {
                // Check for end of string
                if (matched > s.Length)
                    return false;

                // Get next pattern character
                char c = pattern[i++];
                if (c == '[') // Character list
                {
                    // Test for exclude character
                    bool exclude = (i < pattern.Length && pattern[i] == '!');
                    if (exclude)
                        i++;
                    // Build character list
                    int j = pattern.IndexOf(']', i);
                    if (j < 0)
                        j = s.Length;
                    HashSet<char> charList = CharListToSet(pattern.Substring(i, j - i));
                    i = j + 1;

                    if (charList.Contains(s[matched]) == exclude)
                        return false;
                    matched++;
                }
                else if (c == '?') // Any single character
                {
                    matched++;
                }
                else if (c == '#') // Any single digit
                {
                    if (!Char.IsDigit(s[matched]))
                        return false;
                    matched++;
                }
                else if (c == '*') // Zero or more characters
                {
                    if (i < pattern.Length)
                    {
                        // Matches all characters until
                        // next character in pattern
                        char next = pattern[i];
                        int j = s.IndexOf(next, matched);
                        if (j < 0)
                            return false;
                        matched = j;
                    }
                    else
                    {
                        // Matches all remaining characters
                        matched = s.Length;
                        break;
                    }
                }
                else // Exact character
                {
                    if (matched >= s.Length || c != s[matched])
                        return false;
                    matched++;
                }
            }
            // Return true if all characters matched
            return (matched == s.Length);
        }

        /// <summary>
        /// Converts a string of characters to a HashSet of characters. If the string
        /// contains character ranges, such as A-Z, all characters in the range are
        /// also added to the returned set of characters.
        /// </summary>
        /// <param name="charList">Character list string</param>
        private static HashSet<char> CharListToSet(string charList)
        {
            HashSet<char> set = new HashSet<char>();

            for (int i = 0; i < charList.Length; i++)
            {
                if ((i + 1) < charList.Length && charList[i + 1] == '-')
                {
                    // Character range
                    char startChar = charList[i++];
                    i++; // Hyphen
                    char endChar = (char)0;
                    if (i < charList.Length)
                        endChar = charList[i++];
                    for (int j = startChar; j <= endChar; j++)
                        set.Add((char)j);
                }
                else set.Add(charList[i]);
            }
            return set;
        }

        // from https://stackoverflow.com/questions/6442421/c-sharp-fastest-way-to-remove-extra-white-spaces?utm_medium=organic&utm_source=google_rich_qa&utm_campaign=google_rich_qa
        // and  https://stackoverflow.com/questions/6442421/c-sharp-fastest-way-to-remove-extra-white-spaces?utm_medium=organic&utm_source=google_rich_qa&utm_campaign=google_rich_qa
        private static string RemoveAllWhiteSpace(this string self)
        {
            return new string(self.Where(c => !Char.IsWhiteSpace(c)).ToArray());
        }

        // modified from web page to add trimming
        // another answer
        public static string RemoveExtraWhitespace(this string str)
        {
            var sb = new StringBuilder(str.Length); // build a sb this size
            int len = 0;
            var prevIsWhitespace = true;
            //char ch;
            foreach (var ch in str)
            {
                var isWhitespace = char.IsWhiteSpace(ch);
                if (prevIsWhitespace && isWhitespace)
                {
                    continue;
                }
                else sb.Append(ch);
                prevIsWhitespace = isWhitespace;
            }
            len = sb.Length;
            if (prevIsWhitespace) len--;    // if last was whte space, back off from it 
            return sb.ToString(0, len);
        }

        // https://stackoverflow.com/questions/4105386/can-maximum-number-of-characters-be-defined-in-c-sharp-format-strings-like-in-c
        public static string MaxLength(this string input, int length)
        {
            if (input == null) return null;
            return input.Substring(0, Math.Min(length, input.Length));
        }

        public static string NormalizeWhiteSpaceForLoop(string input)
        {
            var src = input.ToCharArray();
            //bool skip = false; // false to keep white space at start
            bool skip = true;    // true  to remove white space at start
            char ch;
            // find last non-whitespace
            int len = Array.FindLastIndex(src, x => !Char.IsWhiteSpace(x));
            // now, work in the middle
            int index = 0;
            for (int i = 0; i < len; i++)
            {
                ch = src[i];
                if (char.IsWhiteSpace(ch))
                {
                    if (skip)
                    {

                    }
                    else
                    {
                        src[index++] = ch;
                        skip = true;
                    }

                }
                else
                {
                    skip = false;
                    src[index++] = ch;
                }
            }
            //// original may be faster, but won't automatically use new unicode whitespace definitions
            return new string(src, 0, index);
        }

        // another answer // customised for SQL 'naturalness'
        public static string RepairSqlWhitespace(this string str)
        {
            var sb = new StringBuilder(str.Length); // build a sb this size
            int len = 0;
            var prevIsWhitespace = true;
            var prevWantsWhitespace = false;
            //char ch;
            foreach (var ch in str)
            {
                if (ch == '[' || ch == ']') continue; // hide these sql characters
                //if (ch.SqlHideThisCharacter()) continue;
                var isWhitespace = char.IsWhiteSpace(ch);
                bool wantsWhiteSpace;
                if (prevIsWhitespace && isWhitespace)
                {
                    continue;
                }
                if (prevWantsWhitespace && !isWhitespace)
                {
                    sb.Append(' '); //  insert extra white space
                    prevIsWhitespace = true;
                    prevWantsWhitespace = false;
                }
                if (wantsWhiteSpace = ch.SqlWantsWhiteSpace())
                {
                    if (!prevIsWhitespace) sb.Append(' ');
                    sb.Append(ch);
                    prevWantsWhitespace = true;
                } // normalise equals;
                else if (isWhitespace) sb.Append(' '); // all whitespace chars should become ' '!
                else sb.Append(ch);
                prevIsWhitespace = isWhitespace;
                prevWantsWhitespace = wantsWhiteSpace;
            }
            len = sb.Length;
            if (prevIsWhitespace) len--;    // if last was whte space, back off from it 
            if (len < 1) return string.Empty;   // nothing to output, clear the string;
            return sb.ToString(0, len); 
        }

        //private static char[] _SqlWantsWhiteSpace = { '=', '(', ')', ',', ';', '+', '.' };
        //private static HashSet<char> _SqlWantsWhiteSpace = 
        //    new HashSet<char> { '=', '(', ')', ',', ';', '+', '.' }; 
        private static bool SqlWantsWhiteSpace (this char ch)
        {
            bool want;
            switch (ch)
            {
                case '=':
                case '(':
                case ')':
                case ',':
                case ';':
                case '+':
                case '.':
                    want = true;
                    break;
                default:
                    want = false;
                    break;
            }
            return want;
            //return (_SqlWantsWhiteSpace.Contains(ch));
        }

        //private static char[] _SqlHideThisCharacter = { '[', ']' };
        //private static HashSet<char> _SqlHideThisCharacter = new HashSet<char> { '[', ']' };
        private static bool SqlHideThisCharacter(this char ch)
        {
            bool want;
            switch (ch)
            {
                case '[':
                case ']':
                    want = true;
                    break;
                default:
                    want = false;
                    break;
            }
            return want;
            //return (_SqlHideThisCharacter.Contains(ch));
        }

        public static bool IsDigits(this string s)
        {
            Boolean value = true;
            foreach (char c in s.ToCharArray())
            {
                value = value && Char.IsDigit(c);
                if (!value) return false;
            }
            return value;
        }

        public static bool EndsWithDigit(this string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            char c = Convert.ToChar( s.Substring(s.Length - 1));
            if (char.IsDigit(c)) return true;
            return false;
        }

        //public static int EndsDigit(string s)
        //{
        //    Boolean value = true;
        //    int digitspot = -1;
        //    var a = s.ToCharArray();
        //    for (int ix = a.Length - 1; ix >= 0; ix--)
        //    {
        //        char c = a[ix];
        //        value = Char.IsDigit(c);
        //        if (!value) return false;
        //    }
        //    return digitspot;
        //}
    }
}
