using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIMCollect
{
    public class SqlSynopsis
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
            { "DECLARE",    new TagData("Dcl", true, false) },
            { "DELETE",     new TagData("Del", true, false) },
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
            { "SET",        new TagData("=", false, false) },
            { "UNION",      new TagData("Un", false, false) },
            { "UPDATE",     new TagData("Up", true, false) },
            { "VALUES",     new TagData("V", false, false) },
            { "WITH",       new TagData("Wi", true, false) },
            { "WHERE",      new TagData("Wh", false, false) },
            { "(",          new TagData("", false, false) },            // skip these markers
            { ")",          new TagData("", false, false) },
            { ".",          new TagData("", false, false) },
            { ",",          new TagData("", false, false) },
            { ";",          new TagData("", false, false) }
        };

//        private static readonly Dictionary<string, string> PatternWords = new Dictionary<string, string>
//        {
//            { "BEGIN", "{" },
//            { "DECLARE", "D" },
//            { "ELSE", "El" },
//            { "END", "}" }, //                              (can be a multiple for repeated sequences)
//            { "EXEC", "X" },
//            { "EXECUTE", "X" },
//            { "FROM", "Fr" },
//            { "HAVING", "Ha" },
//            { "IF",
//@"
//If" },
//            { "INSERT", "In" },
//            { "INTO", "to" },
//            { "JOIN", "J" },//                              (multiples can have number of Joins afterwards

//            { "ORDER", "Or" },
//            { "PRINT", "--" },                              // stop collecting
//            { "RETURN", "Re" },
//            { "RAISERROR", "Er" },
//            { "SELECT", "Se" },
//            { "SET", "=" },
//            { "UNION", "Un" },
//            { "UPDATE", "Up" },
//            { "VALUES", "V" },
//            { "WITH", "Wi" },
//            { "WHERE", "Wh" },
//            { "(", "" },            // skip these markers
//            { ")", "" },
//            { ";", "" }

//        };

        private StringBuilder synopsis = new StringBuilder(800);

        //private string lastIfStatement = string.Empty;
        //private int lastIfRepeat = 0;

        //private string thisIfStatement = string.Empty;
        //private int thisIfRepeat = 0;

        //private string lastStatement = string.Empty;
        //private int lastRepeat = 0;
        private StringBuilder longStory = new StringBuilder();
        private const char CarriageReturn = (char)13;

        private string currentStatement = string.Empty;
        private int currentRepeat = 0;

        private bool inBodyOfProcedure = false;
        private bool markOtherWords = false;
        private bool showVariables = true;
        //private readonly bool startingIfBlock = false;
        private bool inIfBlock = false;

        private string newWord2 = string.Empty;
        private string newWord1 = string.Empty;
        private string newWord0 = string.Empty;

        //[Obsolete]
        //private void Append_obsolete (string newWord)
        //{
        //    newWord2 = newWord1;    // short bad stack of words
        //    newWord1 = newWord0;
        //    newWord0 = newWord;

        //    string newValue = string.Empty;
        //    bool major;
        //    bool hide = false;
        //    // if (PatternWords.TryGetValue(newWord, out string value))
        //    if (PatternData.TryGetValue(newWord, out TagData tag ))
        //    {
        //        newValue = tag.Alias;
        //        major = tag.Major;
        //        hide = tag.Hide;
        //        if (newValue.Equals("--"))
        //        {
        //            markOtherWords = false;
        //            showVariables = false;
        //            newValue = string.Empty;
        //        }
        //        else
        //        {
        //            if (inBodyOfProcedure)
        //            {
        //                markOtherWords = true;
        //                showVariables = true;
        //            }
        //        }

        //    }
        //    else
        //    {
        //        major = false;
        //        char firstLetterOfNewWord = newWord.Length > 1 ? newWord[0] : ' ';
        //        if (firstLetterOfNewWord == '@')
        //        {
        //            if (showVariables)
        //            {
        //                newValue = "@";    // are you sure you want this?
        //                if (newWord.Equals(newWord2))
        //                {
        //                    // duplicate variable
        //                    newValue = string.Empty;   // never mind this word
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (markOtherWords)
        //            {
        //                newValue = ".";    // any other words
        //            }
        //            else
        //            {
        //                if (newWord.Equals("AS"))
        //                {
        //                    inBodyOfProcedure = true;
        //                    markOtherWords = true;
        //                }
        //            }
        //        }
        //    }
        //    Debug.WriteLine($"$ {newWord} = '{newValue}'");
        //    if (newValue?.Length > 0)
        //    { 
        //        if (currentStatement.Equals(newValue))
        //        {
        //            currentRepeat++;
        //        }
        //        else
        //        {
        //            if (newWord0.Equals("IF"))
        //            {
        //                startingIfBlock = true;

        //                //inIfBlock = true;
        //                if (thisIfStatement.Equals(lastIfStatement))
        //                {
        //                    if (lastIfStatement.Length > 0)
        //                    {
        //                        lastIfRepeat++;
        //                        thisIfStatement = string.Empty;
        //                    }
        //                }
        //                else
        //                {
        //                    if (lastIfStatement.Length > 0)
        //                    {
        //                        synopsis.Append("[");
        //                        synopsis.Append(lastIfStatement);
        //                        synopsis.Append("]");
        //                        if (lastIfRepeat > 0)
        //                        {
        //                            synopsis.Append(lastIfRepeat);
        //                        }
        //                    }
        //                    lastIfStatement = thisIfStatement;
        //                    lastIfRepeat = thisIfRepeat;
        //                    thisIfStatement = string.Empty;
        //                    thisIfRepeat = 0;
        //                }
        //            }
        //            if (currentStatement.Equals(lastStatement))
        //            {
        //                lastRepeat += currentRepeat;
        //                currentStatement = string.Empty;
        //                currentRepeat = 0;
        //            }
        //            else if (inIfBlock)
        //            {
        //                if (major)
        //                {
        //                    thisIfStatement += lastStatement;
        //                    thisIfStatement += lastRepeat.ToString();
        //                    //inIfBlock = false;
        //                    if (thisIfStatement.Equals(lastIfStatement))
        //                    {
        //                        lastIfRepeat++;
        //                        thisIfStatement = string.Empty;
        //                        thisIfRepeat = 0;
        //                    }
        //                    else
        //                    {
        //                        if (lastIfStatement.Length > 0)
        //                        {
        //                            synopsis.Append(lastIfStatement);
        //                            if (lastIfRepeat > 1) synopsis.Append(lastIfRepeat);
        //                        }
        //                        lastIfStatement = thisIfStatement;
        //                        lastIfRepeat = thisIfRepeat;
        //                    }
        //                }
        //                else
        //                {
        //                    thisIfStatement += lastStatement;
        //                    if (lastRepeat > 1) thisIfStatement += lastRepeat.ToString();
        //                    lastStatement = currentStatement;
        //                    lastRepeat = currentRepeat;
        //                }
        //            }
        //            else
        //            {
        //                if (lastStatement.Equals("If"))
        //                {
        //                    Debug.Print(currentStatement);
        //                }
        //                synopsis.Append(lastStatement);
        //                if (lastRepeat > 1) synopsis.Append(lastRepeat);
        //                lastStatement = currentStatement;
        //                lastRepeat = currentRepeat;
        //            }
        //            currentStatement = newValue;
        //            currentRepeat = 1;
        //        }
        //    }
        //    if (startingIfBlock)
        //    {
        //        inIfBlock = true;
        //        startingIfBlock = false;
        //        if (lastStatement.Length > 0)
        //        {
        //            synopsis.Append(lastStatement);
        //            if (lastRepeat > 1) synopsis.Append(lastRepeat);
        //        }
        //        lastStatement = currentStatement;
        //        lastRepeat = currentRepeat;
        //    }
        //}


        public void Append(string newWord)
        {
            newWord2 = newWord1;    // short bad stack of words
            newWord1 = newWord0;
            newWord0 = newWord;

            string newValue = string.Empty;
            bool major;
            bool hide = false;
            // if (PatternWords.TryGetValue(newWord, out string value))
            if (PatternData.TryGetValue(newWord, out TagData tag))
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
                else if (newWord.Equals("IF") || newWord.Equals("ELSE"))
                {
                    inIfBlock = true;
                }
                else if (newWord.Equals("BEGIN") && inIfBlock)
                {
                    major = false;  // if begin not a problem
                }
                else if (newWord.Equals("END") && inIfBlock)
                {
                    major = false;  // if begin not a problem
                    inIfBlock = false;
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
//#if DEBUG
//            //Debug.WriteLine($"$ {newWord} = '{newValue}'");
//#endif
            if (newValue?.Length > 0 && hide == false)
            {
                if (major)
                {
                    AddToLongStory();
                    //longStory.Append(CarriageReturn);
                    SaveNewValue(CarriageReturn);
                }
                if (currentStatement.Equals(newValue))
                {
                    currentRepeat++;
                }
                else
                {
                    AddToLongStory();
                    SaveNewValue(newValue);
                }
            }
        }

        private void SaveNewValue(char newChar) => SaveNewValue(newChar.ToString());
        private void SaveNewValue(string newValue)
        {
            currentStatement = newValue;
            currentRepeat = 1;
        }

        private void AddToLongStory()
        {
            longStory.Append(currentStatement);
            Debug.WriteLine(currentStatement);
            if (currentRepeat > 1 && currentStatement.Equals("."))
            {
                longStory.Append(currentStatement);
            }
            else if (currentRepeat > 1)
            {
                longStory.Append('(');
                longStory.Append(currentRepeat);
                longStory.Append(')');
            }
        }

        public void Finish()
        {
            AddToLongStory();
            SaveNewValue(CarriageReturn);       // Extra end of lines to force values out
            AddToLongStory();
            SaveNewValue(CarriageReturn);
            AddToLongStory();

            var sentences = longStory.ToString().Split(CarriageReturn);
            //var shortStory = new string[sentences.Length];
            var shortStory = new List<string>(sentences.Length);
            //int shortIx = -1;
            var lastsentence = string.Empty;
            int repeated = 0; 
            for(var ix = 0; ix < sentences.Length; ix++)
            {
                var sentence = sentences[ix];
                //var lastLength = lastsentence.Length;
                int sanity = 999;   // don't loop forever 
                for (var lastLength = lastsentence.Length; 
                        lastLength > 0 && sentence.Length > lastLength && sanity > 0; 
                        sanity--)
                {
                    if (sentence.Substring(0, lastLength) == lastsentence)
                    {
                        repeated++;
                        sentence = sentence.Substring(lastLength);
                    }
                    else
                    {
                        sanity = 0;
                    }
                }
                if (sentence == lastsentence)
                {
                    repeated++;
                }
                else
                {
                    if (lastsentence.Length > 0)
                    {
                        if (repeated == 1)
                        {
                            //shortIx++;
                            //shortStory[shortIx] = lastsentence;
                            shortStory.Add(lastsentence);
                        }
                        else
                        {
                            var sb = new StringBuilder(lastsentence.Length + 10);
                            sb.Append('[');
                            sb.Append(lastsentence);
                            sb.Append(']');
                            sb.Append('(');
                            sb.Append(repeated);
                            sb.Append(')');
                            //shortIx++;
                            //shortStory[shortIx] = sb.ToString();
                            shortStory.Add(sb.ToString());
                        }
                    }
                    lastsentence = sentence;
                    repeated = 1;
                }
            }

            //var lines = shortStory.Where(x => x?.Length > 0).ToList<string>();

            foreach (var line in shortStory)
            {
                if (line?.Length > 0) synopsis.Append(line);
            }
            //shortIx = 0;

        }

        public override string ToString()
        {
            return synopsis.ToString();
        }
    }
}
