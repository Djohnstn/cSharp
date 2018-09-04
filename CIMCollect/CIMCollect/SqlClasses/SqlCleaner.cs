using System;
using System.Text;
using StringExtensionsIsLike;

namespace Similarity
{
    public static class SqlCleaner
    {

        //static Regex whitespace = new Regex(@"\s+", RegexOptions.Compiled);

        public static string Clean(string[] sql, bool HideAscii = false)
        {
            string secondsql = FixupSqlLines(sql, HideAscii);
            string lastsql = FixupSqlWords(secondsql);
            return lastsql;
        }

        private static readonly char[] spacechar = new char[] { ' ' };


        // fix up some of the optional words
        private static string FixupSqlWords(string secondsql)
        {
            if (string.IsNullOrEmpty(secondsql))
            {
                return string.Empty;
                //throw new ArgumentException("message", nameof(secondsql));
            }

            var words = secondsql.Split(spacechar, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length > 1)
            {
                if (words[words.Length - 1] == ";") words[words.Length - 1] = ""; // trim a semicolon at end
                if (words[0] == "CREATE" && (words[1] == "PROCEDURE" || words[1] == "PROC") && words[3] == "(") // the parens in ( ) AS are optional
                {
                    var foundAS = Array.IndexOf(words, "AS");
                    if (foundAS > 5 && words[foundAS - 1] == ")")
                    {
                        words[3] = string.Empty;
                        words[foundAS - 1] = string.Empty;
                    }
                    ;
                }
                {
                    var foundDELETE = Array.IndexOf(words, "DELETE");
                    if (foundDELETE > 1 && foundDELETE < words.Length - 1)
                    {
                        if (words[foundDELETE + 1] == "FROM")   // SQL Optional keyword after DELETE
                        {
                            words[foundDELETE + 1] = string.Empty;
                        }
                    }
                }
            }
            var lastsql = string.Join(" ", words);
            var repaired = lastsql.RepairSqlWhitespace();
            return repaired;
        }

        private const char quote = '\'';
        private const char minus = '-';

        private static string FixupSqlInlineComment(string sql)
        {
            if (sql.IndexOf("--", StringComparison.Ordinal) == 0) return sql;
            if (sql.StartsWith("--", StringComparison.Ordinal)) return string.Empty;
            var sb = new StringBuilder(sql.Length);
            bool inQuotes = false;
            var cx = sql; //.ToCharArray();
            var clen = cx.Length;
            var clenm1 = clen - 1;
            for (int ix = 0; ix < cx.Length; ix++)
            {
                var c = cx[ix];
                var cn = ix < clenm1 ? cx[ix + 1] : ' ';
                if (inQuotes)
                {
                    sb.Append(c);
                    if (c.Equals(quote)) inQuotes = false;
                }
                else
                {
                    if (c.Equals(quote))
                    {
                        inQuotes = true;
                        sb.Append(c);
                    }
                    else
                    {
                        if (c.Equals(minus) && cn.Equals(minus))
                        {
                            // found a commend while not in comment mode
                            break;
                        }
                        else
                        {
                            if (Char.IsWhiteSpace(c))
                            {
                                sb.Append(' ');
                            }
                            else
                            {
                                sb.Append(c);
                            }
                        }
                    }
                }
            }
            return sb.ToString(); //.Trim();
        }

        private const int AverageSqlLineLength = 64;
        private static string FixupSqlLines(string[] sql, bool HideAscii)
        {
            bool InCommentMode = false;
            var lines = sql; //sql.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            string line;
            var sb = new StringBuilder(sql.Length * AverageSqlLineLength);
            //            for (int ix = 0; ix < lines.Length; ix++)

            foreach (var line0 in lines)
            {
                //var line1 = lines[ix].Trim().ToUpper(System.Globalization.CultureInfo.InvariantCulture);
                //var line1 = line0.Trim().ToUpper(System.Globalization.CultureInfo.InvariantCulture);
                var line1 = line0.Trim().ToUpperInvariant(); // ONLY TOUPPER NEEDED
                bool dataLine = line1.Length > 0;
                if (InCommentMode)
                {
                    var commentEnd = line1.IndexOf("*/", StringComparison.Ordinal);
                    if (commentEnd > -1)
                    {
                        InCommentMode = false;
                        line1 = line1.Remove(0, commentEnd + 2);
                    }
                    else
                    {
                        //line1 = string.Empty;
                        dataLine = false;
                    }
                    line = line1;
                }
                else
                {
                    var ixOfMinus = line1.IndexOf("--", StringComparison.Ordinal);
                    if (ixOfMinus > 0)
                    {
                        line = FixupSqlInlineComment(line1);
                    }
                    else
                    {
                        line = line1;
                    }
                }
                // don't Else this... if you come out of comment mode above, must process this
                if (!InCommentMode && dataLine)
                {
                    // set up a switch statement to allow compiler to optimize for different SQL statements
                    var leftLen = Math.Min(2, line.Length);
                    var linestart = line.Substring(0, leftLen);
                    char lineend = line.Length > 0 ? (char)line[line.Length - 1] : ' ';
                    var lineEndsCloseB = lineend == ']';
                    switch (linestart)
                    {
                        case "--":
                        case "GO":
                            dataLine = false;
                            //line = string.Empty;
                            break;
                        case "/*":
                            //string lineend2 = line.Length > 1 ?
                            //                    string.Concat(line[line.Length - 2], line[line.Length - 1]) : "  ";
                            //var lineEndsCloseComment = lineend2 == "*/";
                            //if (lineEndsCloseComment)
                            if (line.EndsWith("*/", StringComparison.Ordinal))
                            {
                                dataLine = false;
                                //line = string.Empty;
                            }
                            break;
                        case "SE":
                            if (line.StartsWith("SET ANSI_NULLS", StringComparison.InvariantCultureIgnoreCase))
                            {
                                dataLine = false;
                                //line = string.Empty;
                            }
                            else if (line.StartsWith("SET QUOTED_IDENTIFIER", StringComparison.InvariantCultureIgnoreCase))
                            {
                                dataLine = false;
                                //line = string.Empty;
                            }
                            break;
                        case "IF":
                            if (line.StartsWith("IF OBJECT_ID(", StringComparison.InvariantCultureIgnoreCase)
                                    && lineEndsCloseB
                                    && line.Contains("IS NOT NULL DROP PROCEDURE"))
                            {
                                dataLine = false;
                                //line = string.Empty;
                            }
                            break;
                        case "GR":
                            if (line.StartsWith("GRANT EXECUTE ON", StringComparison.InvariantCultureIgnoreCase)
                                    && lineEndsCloseB)
                            {
                                dataLine = false;
                                //line = string.Empty;
                            }
                            else if (line.StartsWith("GRANT ", StringComparison.InvariantCultureIgnoreCase)
                                    && lineEndsCloseB)
                            {
                                dataLine = false;
                                //line = string.Empty;
                            }
                            else if (line.StartsWith("GRANT VIEW DEFINITION ON", StringComparison.InvariantCultureIgnoreCase)
                                    && lineEndsCloseB)
                            {
                                dataLine = false;
                                //line = string.Empty;
                            }
                            break;
                    }
                    //if (line.StartsWith("--")) line = string.Empty; // -- comment line
                    //if (line.StartsWith("/*") && line.EndsWith("*/")) line = string.Empty; // /* comment line */
                    //if (line.Equals("GO")) line = string.Empty; // GO - batch delimiter line
                    //if (line.StartsWith("SET ANSI_NULLS")) line = string.Empty; // parameter line
                    //if (line.Equals("SET ANSI_NULLS OFF")) line = string.Empty; // parameter line
                    //if (line.StartsWith("SET QUOTED_IDENTIFIER")) line = string.Empty; // parameter line
                    //if (line.Equals("SET QUOTED_IDENTIFIER OFF")) line = string.Empty; // parameter line
                    //if (line.StartsWith("IF OBJECT_ID(")
                    //        && line.Contains("IS NOT NULL DROP PROCEDURE")
                    //        && line.EndsWith("]")) line = string.Empty; // /* noise line */
                    //if (line.StartsWith("GRANT EXECUTE ON")
                    //        && line.EndsWith("]")) line = string.Empty; // /* security line */
                    //if (line.StartsWith("GRANT ")
                    //        && line.EndsWith("]")) line = string.Empty; // /* security line */
                    //if (line.StartsWith("GRANT VIEW DEFINITION ON")
                    //        && line.EndsWith("]")) line = string.Empty; // /* security line */

                    if (dataLine)
                    {
                        if (HideAscii)  // pretend these common ASCII definitions are Unicode - might be wrong decision? - jdj 4/2018
                        {
                            if (line.Contains(" CHAR")) line = line.Replace(" CHAR", " NCHAR");
                            if (line.Contains(" VARCHAR")) line = line.Replace(" VARCHAR", " NVARCHAR");
                        }
                        var commentStart = line.IndexOf("/*", StringComparison.Ordinal);
                        if (commentStart > -1)
                        {
                            var commentEnd = line.IndexOf("*/", StringComparison.Ordinal);    // don't do this unless there was a commentstart!
                            if (commentEnd > commentStart)
                            {
                                // '012/*5*/89'
                                //  012s45e
                                // s=3; e=6; 6-3+2
                                line = line.Remove(commentStart, commentEnd - commentStart + 2);
                            }
                            else
                            {
                                line = line.Remove(commentStart);
                                InCommentMode = true;
                            }
                        }
                    }

                }

                //lines[ix] = line;
                if (line.Length > 0 && dataLine)
                {
                    sb.Append(line);
                    sb.Append(" ");
                }
            }
            return sb.ToString();
            //var secondsql = string.Join(" ", lines); //.RepairSqlWhitespace();
            //return secondsql;
        }




    }
}
