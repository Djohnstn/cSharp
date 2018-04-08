﻿using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace CIMCollect
{
    public class FileSetup
    {
        public string IniFileName { get; set; } //        CimCollect-whatnot.ini
        public string SectionName { get; set; } //        [Hosts]
        public string FilePath { get; set; }    //        File=%windir%\System32\drivers\etc\hosts.
        public string TrimOptions { get; set; } //        TrimSpace=All
                                                //;TrimSpace=Edge
        public string CommentMark { get; set; } //Comment =#
        public string FileMissing { get; set; }  //Missing = *No Hosts*

    }

    public class FileRunner
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
                if (!String.IsNullOrWhiteSpace(fileToCopyName))
                {
                    FileSetupList.Add(new FileSetup()
                    {
                        IniFileName = iniFileName,
                        SectionName = section,
                        CommentMark = ini.GetValue(section, "Comment", ""),
                        FileMissing = ini.GetValue(section, "Missing", "***no file***"),
                        FilePath = fileToCopyName, // ini.GetValue(section, "File",""),
                        TrimOptions = ini.GetValue(section, "Trim", "None")
                    });
                }
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
                bool result = RunFileQuery(Server, fileSection);
            }
            sw.Stop();
            ToFile();
        }

        // part 2a
        public static bool RunFileQuery(string server, FileSetup fileSetup)
        {
            if (String.IsNullOrWhiteSpace(fileSetup.IniFileName)) return false;
            bool result = false;
            var filename = fileSetup.IniFileName;
            var section = fileSetup.SectionName;
            logger.Info($"Begin {filename}::{server}:[{section}]");
            var sw = new Stopwatch();
            sw.Start();
            {
                result = ProcessFile(Server, section, fileSetup);
            }
            sw.Stop();
            logger.Info($"Finis {filename}::{server}:[{section}] rc={result}; in={sw.ElapsedMilliseconds}ms");
            return result;
        }


        // part 2b
        private static bool ProcessFile(string server, string section, FileSetup fileSetup)
        {
            var nameid = fileSetup.FilePath;
            var iniName = fileSetup.IniFileName;
            var trimOptions = fileSetup.TrimOptions;
            var filenotfound = fileSetup.FileMissing;
            var comment = fileSetup.CommentMark;
            var infoParts = HandleResults(server, section, nameid, iniName, trimOptions, filenotfound, comment);
            Parts.PartsList.AddRange(infoParts.PartsList);
            return true;
        }


        // part 2c
        private static InfoParts HandleResults(string server, string dataset, string nameid, 
            string filename, string trimOptions, string filenotfound, string comment)
        {
            var expandedFileName = Environment.ExpandEnvironmentVariables(nameid);
            FileInfo file = new FileInfo(expandedFileName);
            var simplefilename = file.Name;
            var parts = new InfoParts(server, "file", DateTime.UtcNow);
            int index = 0;
            if (file.Exists)
            {
                using (FileStream fs = file.OpenRead()) // this shares with open files, no crash
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (!sr.EndOfStream)
                    {
                        var line = Utilities.RemoveAfter(sr.ReadLine(), comment);
                        string val;
                        switch (trimOptions.Trim().ToLower())
                        {
                            case "edge":
                                val = line.Trim();
                                break;
                            case "all":
                                val = Utilities.ReduceWhiteSpace(line);
                                break;
                            default:
                                val = line;
                                break;
                        }
                        if (val.Length > 0)
                        {
                            ++index;
                            parts.Add(simplefilename, 1, $"{dataset} {index}", "String", val);
                        }
                    }
                }
                if (parts.PartsList.Count < 1)
                {
                    parts.Add(simplefilename, 1, $"{dataset} {index}", "String", comment);  // add comment line if there was no uncommented data in the file
                }
            }
            else
            {
                parts.Add(simplefilename, 1, $"{dataset} {index}", "String", filenotfound);
            }

            return parts;
        }

        // part 2z
        private static bool ToFile()
        {
            Parts.ToJsonFile(SaveToFolder);
            var count = Parts.PartsList.Count;
            return true; 
        }

    }
}