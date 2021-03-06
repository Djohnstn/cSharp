﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
//using Microsoft.VisualBasic.CompilerServices; // not meant to be used by user code.  oh well.
using StringExtensionsIsLike; //StringIsLike;


// https://www.codeproject.com/tips/771772/a-simple-and-efficient-ini-file-reader-in-csharp


namespace IniFile
{
    /// <summary>
    /// A class for reading values by section and key from a standard ".ini" initialization file.
    /// </summary>
    /// <remarks>
    /// Section and key names are not case-sensitive. Values are loaded into a hash table for fast access.
    /// Use <see cref="GetAllValues"/> to read multiple values that share the same section and key.
    /// Sections in the initialization file must have the following form:
    /// <code>
    ///     ; comment line
    ///     [section]
    ///     key=value
    ///     multiline={     // proposal
    ///         value
    ///         value
    ///         }
    /// </code>
    /// </remarks>

    public class IniFile
    {
        private SortedDictionary<string,string> Sections = new SortedDictionary<string, string>();
        private Dictionary<string,string> dictionary = new Dictionary<string, string>();
        
        /// <summary>
        /// Initializes a new instance of the <see cref="IniFile"/> class.
        /// </summary>
        /// <param name="file">The initialization file path.</param>
        /// <param name="commentDelimiter">The comment delimiter string (default value is ";").
        /// </param>
        /// 
        public IniFile(string file, string commentDelimiter = ";")
        {
            CommentDelimiter = commentDelimiter;
            TheFile = file;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IniFile"/> class.
        /// </summary>
        public IniFile()
        {
            CommentDelimiter = ";";
        }

        /// <summary>
        /// The comment delimiter string (default value is ";").
        /// </summary>
        public string CommentDelimiter { get; set; }

        private string theFile = null;

        /// <summary>
        /// The initialization file path.
        /// </summary>
        public string TheFile
        {
            get
            {
                return theFile;
            }
            set
            {
                theFile = null;
                dictionary.Clear();
                if (File.Exists(value))
                {
                    theFile = value;
                    //JDJohnston start
                    string multiValueEnd = "";
                    string multiValueKey = "";
                    //JDJohnston end
                    using (StreamReader sr = new StreamReader(theFile))
                    {
                        string line, section = "";
                        while ((line = sr.ReadLine()) != null)
                        {
                            line = line.Trim();
                            if (line.Length == 0) continue;  // empty line
                            if (!String.IsNullOrEmpty(CommentDelimiter) && line.StartsWith(CommentDelimiter))
                                continue;  // comment

                            if (line.StartsWith("[") && line.Contains("]"))  // [section]
                            {
                                int index = line.IndexOf(']');
                                section = line.Substring(1, index - 1).Trim();
                                Sections[section] = section;    // save section name // jdjohnston
                                continue;
                            }
                            // added by JDJohnston for multi-line values
                            if (!String.IsNullOrWhiteSpace(multiValueEnd))
                            {
                                if (line.StartsWith(multiValueEnd))
                                {   // found end of value section, clear status and go to next line
                                    multiValueEnd = "";
                                    multiValueKey = "";
                                    continue;
                                }
                                else
                                {
                                    line = multiValueKey + "=" + line;  // fake the line to have a key
                                }
                            }
                            else
                            {   // are we starting a multiline section? 
                                if (line.Contains("=[[") && line.EndsWith("[["))
                                {   // if line is "s=[[***[[" then end = "]]***]]"
                                    int index = line.IndexOf('=');
                                    string val = line.Substring(index + 1).Trim();
                                    multiValueKey = line.Substring(0, index).Trim();
                                    multiValueEnd = val.Replace('[',']');
                                    continue;
                                }
                            }
                            // JDJohnston end
                            if (line.Contains("="))  // key=value
                            {
                                int index = line.IndexOf('=');
                                string key = line.Substring(0, index).Trim();
                                string val = line.Substring(index + 1).Trim();
                                string key2 = String.Format("[{0}]{1}", section, key).ToLower();

                                if (val.StartsWith("\"") && val.EndsWith("\""))  // strip quotes
                                    val = val.Substring(1, val.Length - 2);

                                if (dictionary.ContainsKey(key2))  // multiple values can share the same key
                                {
                                    index = 1;
                                    string key3;
                                    while (true)
                                    {
                                        key3 = String.Format("{0}~{1}", key2, ++index);
                                        if (!dictionary.ContainsKey(key3))
                                        {
                                            dictionary.Add(key3, val);
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    dictionary.Add(key2, val);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// GetSection returns an enumerable list of section names matching a pattern, 
        /// </summary>
        /// <param name="pattern">defaults to * all </param>
        /// <returns></returns>
        public IEnumerable<string> GetSection(string pattern = "*")
        {
            foreach (var sectionname in Sections.Keys)
            {
                if (sectionname.IsLike(pattern.ToLower()))
                {
                    yield return sectionname;
                }
            }
        }


        /// <summary>
        /// GetSection returns an enumerable list of section names matching a pattern, containing a key matching a name
        /// </summary>
        /// <param name="pattern">defaults to * all </param>
        /// <returns></returns>
        public IEnumerable<string> GetSection(string pattern = "*", string key = "")
        {
            foreach (var sectionname in Sections.Keys)
            {
                if (sectionname.IsLike(pattern.ToLower()))
                {
                    if (this.TryGetValue(sectionname, key, out string value))
                    {
                        yield return sectionname;
                    }
                }
            }
        }


        // "[section]key"   -> "value1"
        // "[section]key~2" -> "value2"
        // "[section]key~3" -> "value3"
        //private Dictionary<string, string> dictionary = new Dictionary<string, string>();
        private bool TryGetValue(string section, string key, out string value)
        {
            string key2;
            if (section.StartsWith("["))
                key2 = String.Format("{0}{1}", section, key);
            else
                key2 = String.Format("[{0}]{1}", section, key);

            return dictionary.TryGetValue(key2.ToLower(), out value);
        }

        /// <summary>
        /// Gets a string value by section and key.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The value.</returns>
        /// <seealso cref="GetAllValues"/>
        public string GetValue(string section, string key, string defaultValue = "")
        {
            if (!TryGetValue(section, key, out string value))
                return defaultValue;
            return value;
        }

        /// <summary>
        /// Gets a string value by section and key.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <returns>The value.</returns>
        /// <seealso cref="GetValue"/>
        public string this[string section, string key]
        {
            get
            {
                return GetValue(section, key);
            }
        }

        /// <summary>
        /// Gets an integer value by section and key.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="minValue">Optional minimum value to be enforced.</param>
        /// <param name="maxValue">Optional maximum value to be enforced.</param>
        /// <returns>The value.</returns>
        public int GetInteger(string section, string key, int defaultValue = 0,
            int minValue = int.MinValue, int maxValue = int.MaxValue)
        {
            if (!TryGetValue(section, key, out string stringValue))
                return defaultValue;

            if (!int.TryParse(stringValue, out int value))
            {
                if (!double.TryParse(stringValue, out double dvalue))
                    return defaultValue;
                value = (int)dvalue;
            }

            if (value < minValue)
                value = minValue;
            if (value > maxValue)
                value = maxValue;
            return value;
        }

        /// <summary>
        /// Gets a double floating-point value by section and key.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="minValue">Optional minimum value to be enforced.</param>
        /// <param name="maxValue">Optional maximum value to be enforced.</param>
        /// <returns>The value.</returns>
        public double GetDouble(string section, string key, double defaultValue = 0,
            double minValue = double.MinValue, double maxValue = double.MaxValue)
        {
            if (!TryGetValue(section, key, out string stringValue))
                return defaultValue;

            if (!double.TryParse(stringValue, out double value))
                return defaultValue;

            if (value < minValue)
                value = minValue;
            if (value > maxValue)
                value = maxValue;
            return value;
        }

        /// <summary>
        /// Gets a boolean value by section and key.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The value.</returns>
        public bool GetBoolean(string section, string key, bool defaultValue = false)
        {
            if (!TryGetValue(section, key, out string stringValue))
                return defaultValue;

            return (stringValue != "0" && !stringValue.StartsWith("f", true, null));
        }

        /// <summary>
        /// Gets an array of string values by section and key.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <returns>The array of values, or null if none found.</returns>
        /// <seealso cref="GetValue"/>
        public string[] GetAllValues(string section, string key)
        {
            string key2, key3;
            if (section.StartsWith("["))
                key2 = String.Format("{0}{1}", section, key).ToLower();
            else
                key2 = String.Format("[{0}]{1}", section, key).ToLower();

            if (!dictionary.TryGetValue(key2, out string value))
                return null;

            List<string> values = new List<string>
            {
                value
            };
            int index = 1;
            while (true)
            {
                key3 = String.Format("{0}~{1}", key2, ++index);
                if (!dictionary.TryGetValue(key3, out value))
                    break;
                values.Add(value);
            }

            return values.ToArray();
        }
    }
}