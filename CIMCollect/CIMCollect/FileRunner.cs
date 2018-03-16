using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CIMCollect
{
    class FileSetup
    {
        public string SectionName { get; set; } //        [Hosts]
        public string FilePath { get; set; }    //        File=%windir%\System32\drivers\etc\hosts.
        public string TrimOptions { get; set; } //        TrimSpace=All
                                                //;TrimSpace=Edge
        public string CommentMark { get; set; } //Comment =#
        public string FileMissing { get; set; }  //Missing = *No Hosts*

    }

    internal class FileRunner
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static List<FileSetup> FileSetupList = new List<FileSetup>();
        private static string Server = "";
        public static string SaveToFolder { get; set; }

        public FileRunner()
        {
        }

        // save all file sections for processing all at once
        public static void EachFileSection(ref int collections, string server, ref long fileMilliSeconds, string filename, IniFile.IniFile ini)
        {
            Server = server;
            foreach (var section in ini.GetSection("*", "File"))
            {
                if (section.Equals("cimcollect")) continue; // ignore sample section
                collections++;
                var iniCollect = ini.GetValue(section, "cimcollect", "yes");
                if ("0fn".ToLower().Contains((iniCollect + "Y").Substring(0, 1).ToLower())) continue;
                var iniName = ini.GetValue(section, "File");
                FileSetupList.Add(new FileSetup()
                {
                    SectionName = section,
                    CommentMark = ini.GetValue(section, "Comment",""),
                    FileMissing = ini.GetValue(section, "Missing","***no file***"),
                    FilePath = ini.GetValue(section, "File",""),
                    TrimOptions = ini.GetValue(section,"Trim","None")
                });
            }
        }

        public static bool RunFileQuery(string server, ref long fileMilliSeconds, string filename, string section, string iniName)
        {
            if (String.IsNullOrWhiteSpace(iniName)) return false;
            bool result;
            logger.Info($"Begin {filename}::{server}:[{section}]");
            var sw = new Stopwatch();
            sw.Start();
            {
                var thisTest = new FileRunner();
                result = thisTest.ToFile(SaveToFolder, Server, section, iniName);
            }
            sw.Stop();
            fileMilliSeconds += sw.ElapsedMilliseconds;
            logger.Info($"Finis {filename}::{server}:[{section}] rc={result}; in={sw.ElapsedMilliseconds}ms");
            return result;
        }

        public static void FinishFileSaver(string saveToFolder)
        {
            SaveToFolder = saveToFolder;
            var sw = new Stopwatch();
            sw.Start();
            foreach (var fileSection in FileSetupList)
            {
                bool result = FileRunner.RunFileQuery(Server, 
                    fileSection.FilePath, fileSection.SectionName);
            }
            sw.Stop();
        }

        internal bool ToFile(string saveToFolder, string server, string section, string iniName)
        {
            throw new NotImplementedException();
        }
    }
}