﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace CIMCollect
{
    class Utilities
    {
        public static void Pause()
        {
            if (Environment.UserInteractive)
            {
                Console.Write("Press Enter to Exit");
                Console.ReadLine();
            }
            else
            {
                Console.Write("Program Exiting");
            }
        }

        public static char SemiPause(string prompt, int timewait)
        {
            if (Environment.UserInteractive)
            {
                return Pause(prompt, timewait);
            }
            return '\0';
        }

        public static char Pause(string prompt, int timewait)
        {
            Console.Write(prompt);
            char keypress = (char)0;
            if (Environment.UserInteractive)
            {
                bool first = true;
                var future = DateTime.Now.AddSeconds(timewait);
                int countdown = 10;
                do
                {
                    while (!Console.KeyAvailable) //Continue if a Key press is not available in the input stream
                    {
                        Thread.Sleep(30);
                        var remain = (future - DateTime.Now).Seconds;
                        if (DateTime.Now > future) return keypress;
                        if ((remain < 9) && (countdown != remain))
                        {
                            countdown = remain;
                            char digit = (char)(countdown + (char)'1');
                            if (!first) Console.Write("\b\b\b");
                            Console.Write($"[{digit}]");
                            first = false;
                        }
                    }
                    keypress = Console.ReadKey(false).KeyChar;
                } while (keypress == (char)0); //exit if anything was pressed        
            }
            return keypress;
        }

        public static string RemoveAfter(string x, string stop)
        {
            if (x.Contains(stop))
            {
                int ix = x.IndexOf(stop);
                return x.Substring(0, ix);
            }
            else
            {
                return x;
            }
        }
        public static string ReduceWhiteSpace(string x)
        {
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);
            var trimmed = regex.Replace(x.Trim(), " ");
            return trimmed;
        }

    }
}
