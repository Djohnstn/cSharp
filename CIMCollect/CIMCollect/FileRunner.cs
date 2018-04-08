using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace CIMCollect
{
    class FileSetup
    {
        public string IniFileName { get; set; } //        CimCollect-whatnot.ini
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

        private static InfoParts Parts;

        public FileRunner()
        {
        }

        // part 1 - save all file sections for processing all at once into List<FileSetup>() FileSetupList;
        public static void EachFileSection(ref int collections, string server, 
                                            //ref long fileMilliSeconds, 
                                            string filename, 
                                            IniFile.IniFile ini)
        {
            Server = server;
            var iniFileName = ini.TheFile;
            foreach (var section in ini.GetSection("*", "File"))
            {
                if (section.Equals("cimcollect")) continue; // ignore sample section
                collections++;
                var iniCollect = ini.GetValue(section, "cimcollect", "yes");
                if ("0fn".ToLower().Contains((iniCollect + "Y").Substring(0, 1).ToLower())) continue;
                var fileToCopyName = ini.GetValue(section, "File");
                if (!String.IsNullOrWhiteSpace(fileToCopyName)) FileSetupList.Add(new FileSetup()
                {
                    IniFileName = iniFileName,
                    SectionName = section,
                    CommentMark = ini.GetValue(section, "Comment",""),
                    FileMissing = ini.GetValue(section, "Missing","***no file***"),
                    FilePath = fileToCopyName, // ini.GetValue(section, "File",""),
                    TrimOptions = ini.GetValue(section,"Trim","None")
                });
            }
        }


        // part 2 - collect all data
        public static void FinishFileSaver(string saveToFolder)
        {
            Parts = new InfoParts(Server, "File", DateTime.UtcNow);
            SaveToFolder = saveToFolder;
            var sw = new Stopwatch();
            sw.Start();
            foreach (var fileSection in FileSetupList)
            {
                bool result = RunFileQuery(Server, 
                                            fileSection.FilePath, 
                                            fileSection.SectionName, 
                                            fileSection.IniFileName);
            }
            sw.Stop();
        }

        // part 2a
        public static bool RunFileQuery(string server, string filename, string section, string iniName)
        {
            if (String.IsNullOrWhiteSpace(iniName)) return false;
            bool result = false;
            logger.Info($"Begin {filename}::{server}:[{section}]");
            var sw = new Stopwatch();
            sw.Start();
            {
                result = ToFile(SaveToFolder, Server, section, iniName);
            }
            sw.Stop();
            logger.Info($"Finis {filename}::{server}:[{section}] rc={result}; in={sw.ElapsedMilliseconds}ms");
            return result;
        }

        internal static bool ToFile(string saveToFolder, string server, string section, string iniName)
        {
            return false; // ReadFile
        }


        private InfoParts HandleResults(string server, string dataset, string nameid, string filename)
        {
            FileInfo file = new FileInfo(filename);
            if (file.Exists)
            {
                int index = 0;
                using (FileStream fs = file.OpenRead()) // this shares with open files, no crash
                    using (StreamReader sr = new StreamReader(fs))
                {
                    var line = sr.ReadLine();
                    parts.Add(
                                identity: "",
                                index: ++index,
                                name: nameid,
                                type: "string",
                                value: line
                            );
                }
            }


            foreach (var line in System.IO.File.ReadAllText(filename))
            {

            }

            return parts;
        }
        }
    }