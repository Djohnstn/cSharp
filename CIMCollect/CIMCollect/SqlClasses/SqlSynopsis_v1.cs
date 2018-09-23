using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIMCollectZZ
{
    public class SqlSynopsisZZV1
    {
        private class TagData
        {
            public string Alias;
            public bool Major;
            public bool Hide;

            public TagData(string alias, bool major, bool hide)
            {
                this.Alias = alias;
                this.Major = major;
                this.Hide = hide;
            }
        }

        private static readonly Dictionary<string, TagData> PatternData = new Dictionary<string, TagData>()
        {
            { "BEGIN",      new TagData("{", false, false) },
            { "DECLARE",    new TagData("D", true, false) },
            { "ELSE",       new TagData("El", false, false) },
            { "END",        new TagData("}", false, false) }, //                              (can be a multiple for repeated sequences)
            { "EXEC",       new TagData("X", true, false) },
            { "EXECUTE",    new TagData("X", true, false) },
            { "FROM",       new TagData("Fr", false, false) },
            { "HAVING",     new TagData("Ha", false, false) },
            { "IF",         new TagData("If", true, false) },
            { "INSERT",     new TagData("In", true, false) },
            { "INTO",       new TagData("to", false, false) },
            { "JOIN",       new TagData("J", false, false) },//                              (multiples can have number of Joins afterwards

            { "ORDER",      new TagData("Or", false, false) },
            { "PRINT",      new TagData("--", true, true) },                              // stop collecting
            { "RETURN",     new TagData("Re", true, false) },
            { "RAISERROR",  new TagData("Er", true, false) },
            { "SELECT",     new TagData("Se", true, false) },
            { "SET",        new TagData("=", true, false) },
            { "UNION",      new TagData("Un", false, false) },
            { "UPDATE",     new TagData("Up", true, false) },
            { "VALUES",     new TagData("V", false, false) },
            { "WITH",       new TagData("Wi", true, false) },
            { "WHERE",      new TagData("Wh", false, false) },
            { "(",          new TagData("", false, false) },            // skip these markers
            { ")",          new TagData("", false, false) },
            { ";",          new TagData("", false, false) }
        };

        private static readonly Dictionary<string, string> PatternWords = new Dictionary<string, string>
        {
            { "BEGIN", "{" },
            { "DECLARE", "D" },
            { "ELSE", "El" },
            { "END", "}" }, //                              (can be a multiple for repeated sequences)
            { "EXEC", "X" },
            { "EXECUTE", "X" },
            { "FROM", "Fr" },
            { "HAVING", "Ha" },
            { "IF",
@"
If" },
            { "INSERT", "In" },
            { "INTO", "to" },
            { "JOIN", "J" },//                              (multiples can have number of Joins afterwards

            { "ORDER", "Or" },
            { "PRINT", "--" },                              // stop collecting
            { "RETURN", "Re" },
            { "RAISERROR", "Er" },
            { "SELECT", "Se" },
            { "SET", "=" },
            { "UNION", "Un" },
            { "UPDATE", "Up" },
            { "VALUES", "V" },
            { "WITH", "Wi" },
            { "WHERE", "Wh" },
            { "(", "" },            // skip these markers
            { ")", "" },
            { ";", "" }

        };

        private StringBuilder synopsis = new StringBuilder(800);

        private string lastIfStatement = string.Empty;
        private int lastIfRepeat = 0;

        private string thisIfStatement = string.Empty;
        private int thisIfRepeat = 0;

        private string lastStatement = string.Empty;
        private int lastRepeat = 0;

        private string currentStatement = string.Empty;
        private int currentRepeat = 0;

        private bool inBodyOfProcedure = false;
        private bool markOtherWords = false;
        private bool showVariables = true;
        private bool startingIfBlock = false;
        private bool inIfBlock = false;

        private string newWord2 = string.Empty;
        private string newWord1 = string.Empty;
        private string newWord0 = string.Empty;


        public void Append (string newWord)
        {
            newWord2 = newWord1;    // short bad stack of words
            newWord1 = newWord0;
            newWord0 = newWord;

            string newValue = string.Empty;
            bool major;
            bool hide = false;
            // if (PatternWords.TryGetValue(newWord, out string value))
            if (PatternData.TryGetValue(newWord, out TagData tag ))
            {
                newValue = tag.Alias;
                major = tag.Major;
                hide = tag.Hide;
                if (newValue.Equals("--"))
                {
                    markOtherWords = false;
                    showVariables = false;
                    newValue = string.Empty;
                }
                else
                {
                    if (inBodyOfProcedure)
                    {
                        markOtherWords = true;
                        showVariables = true;
                    }
                }

            }
            else
            {
                major = false;
                char firstLetterOfNewWord = newWord.Length > 1 ? newWord[0] : ' ';
                if (firstLetterOfNewWord == '@')
                {
                    if (showVariables)
                    {
                        newValue = "@";    // are you sure you want this?
                        if (newWord.Equals(newWord2))
                        {
                            // duplicate variable
                            newValue = string.Empty;   // never mind this word
                        }
                    }
                }
                else
                {
                    if (markOtherWords)
                    {
                        newValue = ".";    // any other words
                    }
                    else
                    {
                        if (newWord.Equals("AS"))
                        {
                            inBodyOfProcedure = true;
                            markOtherWords = true;
                        }
                    }
                }
            }
            Debug.WriteLine($"$ {newWord} = '{newValue}'");
            if (newValue?.Length > 0)
            { 
                if (currentStatement.Equals(newValue))
                {
                    currentRepeat++;
                }
                else
                {
                    if (newWord0.Equals("IF"))
                    {
                        startingIfBlock = true;

                        //inIfBlock = true;
                        if (thisIfStatement.Equals(lastIfStatement))
                        {
                            if (lastIfStatement.Length > 0)
                            {
                                lastIfRepeat++;
                                thisIfStatement = string.Empty;
                            }
                        }
                        else
                        {
                            if (lastIfStatement.Length > 0)
                            {
                                synopsis.Append("[");
                                synopsis.Append(lastIfStatement);
                                synopsis.Append("]");
                                if (lastIfRepeat > 0)
                                {
                                    synopsis.Append(lastIfRepeat);
                                }
                            }
                            lastIfStatement = thisIfStatement;
                            lastIfRepeat = thisIfRepeat;
                            thisIfStatement = string.Empty;
                            thisIfRepeat = 0;
                        }
                    }
                    if (currentStatement.Equals(lastStatement))
                    {
                        lastRepeat += currentRepeat;
                        currentStatement = string.Empty;
                        currentRepeat = 0;
                    }
                    else if (inIfBlock)
                    {
                        if (major)
                        {
                            thisIfStatement += lastStatement;
                            thisIfStatement += lastRepeat.ToString();
                            //inIfBlock = false;
                            if (thisIfStatement.Equals(lastIfStatement))
                            {
                                lastIfRepeat++;
                                thisIfStatement = string.Empty;
                                thisIfRepeat = 0;
                            }
                            else
                            {
                                if (lastIfStatement.Length > 0)
                                {
                                    synopsis.Append(lastIfStatement);
                                    if (lastIfRepeat > 1) synopsis.Append(lastIfRepeat);
                                }
                                lastIfStatement = thisIfStatement;
                                lastIfRepeat = thisIfRepeat;
                            }
                        }
                        else
                        {
                            thisIfStatement += lastStatement;
                            if (lastRepeat > 1) thisIfStatement += lastRepeat.ToString();
                            lastStatement = currentStatement;
                            lastRepeat = currentRepeat;
                        }
                    }
                    else
                    {
                        if (lastStatement.Equals("If"))
                        {
                            Debug.Print(currentStatement);
                        }
                        synopsis.Append(lastStatement);
                        if (lastRepeat > 1) synopsis.Append(lastRepeat);
                        lastStatement = currentStatement;
                        lastRepeat = currentRepeat;
                    }
                    currentStatement = newValue;
                    currentRepeat = 1;
                }
            }
            if (startingIfBlock)
            {
                inIfBlock = true;
                startingIfBlock = false;
                if (lastStatement.Length > 0)
                {
                    synopsis.Append(lastStatement);
                    if (lastRepeat > 1) synopsis.Append(lastRepeat);
                }
                lastStatement = currentStatement;
                lastRepeat = currentRepeat;
            }
        }

        public void Finish()
        {
            synopsis.Append(this.lastStatement);
            if (lastRepeat > 1) synopsis.Append(this.lastRepeat.ToString());
            synopsis.Append(this.currentStatement);
            if (currentRepeat > 1) synopsis.Append(this.currentRepeat.ToString());
        }

        public override string ToString()
        {

            return synopsis.ToString();
        }
    }
}
