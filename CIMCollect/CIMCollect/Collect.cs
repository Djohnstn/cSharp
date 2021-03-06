﻿using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using IniFile;
using System.Diagnostics;
using System.IO;
using DirectorySecurityList;
using CIMSave;

namespace CIMCollect
{
    class Collect
    {
        public Collect()
        {
            string savefolder = CommandlineParameters._fileSaveFolder;
            SaveToFolder = savefolder;
            if (!System.IO.Directory.Exists(SaveToFolder)) Directory.CreateDirectory(SaveToFolder);
        }


        //private bool FirstFileCall = true;

        public string SaveToFolder { get; set; } = "";

        public void AllConfig()
        {
            int files = 0;
            int collections = 0; 
            var server = Environment.MachineName;
            long fileMilliSeconds = 0;
            long totalMilliSeconds = 0;


            foreach (var filename 
                        in Directory.EnumerateFiles(".", 
                            "CIMCollect-*.txt", 
                            SearchOption.AllDirectories))
            {
                files++;
                fileMilliSeconds = 0;
                var ini = new IniFile.IniFile(filename);
                // check signature on this file, set to NO if you do not want to run the file
                var iniCheck = ini.GetValue("cimcollect", "cimcollect", "NO");
                if (iniCheck.Equals("NO")) continue;
                // if value starts with 0 or NO or No or False or Nyet or Nien, bail out of this file
                if ("0fn".ToLower().Contains((iniCheck + "N").Substring(0, 1).ToLower())) continue;
                //FileRunner.EachFileSection(ref collections, server, ref fileMilliSeconds, filename, ini);
                FileRunner.EachFileSection(ref collections, server, filename, ini);
                EachPowerShellSection(ref collections, server, ref fileMilliSeconds, filename, ini);

                Console.WriteLine($"{Utilities.LogTime()} Processed file {filename}, {collections} collections in {fileMilliSeconds}ms.");
                totalMilliSeconds += fileMilliSeconds;
                fileMilliSeconds = 0;
            }
            FileRunner.FinishFileSaver(SaveToFolder);
            Console.WriteLine($"{Utilities.LogTime()} Processed {files} files, {collections} collections in {totalMilliSeconds}ms.");
            //Utilities.SemiPause("Press any key to exit.", 30);
        }

        internal void FileCollect()
        {
            var settings = Properties.Settings.Default;
            var diskInventory = new DiskInventory(settings, SaveToFolder);
            diskInventory.EachDisk();
        }

        internal void SQLCollect()
        {
            var sql = new MsSqlInventory();
            sql.Inventory(SaveToFolder);
        }

        private void EachPowerShellSection(ref int collections, string server, ref long fileMilliSeconds, string filename, IniFile.IniFile ini)
        {
            foreach (var section in ini.GetSection("*"))
            {
                if (section.Equals("cimcollect")) continue; // ignore sample section
                collections++;
                var iniCollect = ini.GetValue(section, "cimcollect", "yes");
                if ("0fn".ToLower().Contains((iniCollect + "Y").Substring(0, 1).ToLower())) continue;
                var iniName = ini.GetValue(section, "name", "name");
                var s = ini.GetAllValues(section, "s");
                var ps = ini.GetAllValues(section, "powershell");
                var query = ((null == ps) ? "" : String.Join(" ", ps))
                          + ((null == s) ? "" : String.Join(Environment.NewLine, s));
                bool result = false;
                if (!String.IsNullOrWhiteSpace(query))
                {
                    result = RunPowerShellQuery(server, ref fileMilliSeconds, filename, section, iniName, query);
                }
            }
        }

        private bool RunPowerShellQuery(string server, ref long fileMilliSeconds, string filename, string section, string iniName, string query)
        {
            bool result;
            Console.WriteLine($"{Utilities.LogTime()} Begin {filename}::{server}:[{section}]");
            var sw = new Stopwatch();
            sw.Start();
            {
                var pstest = new PSRunner()
                {
                    SaveToFolder = this.SaveToFolder
                };
                result = pstest.ToFile(server, section, iniName, query);
            }
            sw.Stop();
            fileMilliSeconds += sw.ElapsedMilliseconds;
            Console.WriteLine($"{Utilities.LogTime()} Finis {filename}::{server}:[{section}] rc={result}; in={sw.ElapsedMilliseconds}ms");
            return result;
        }


    }
}
