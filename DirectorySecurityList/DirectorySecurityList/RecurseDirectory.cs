using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Principal;
using System.Security.AccessControl;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using CIMSave;

namespace DirectorySecurityList
{
    public class RecurseDirectory
    {
        //private string[] Builtins = new string[] { "NT AUTHORITY\\", "BUILTIN\\" };

        public int StartedFolders { get; internal set; } = 0;
        public int StartedFiles { get; internal set; } = 0;
        public int IgnoredFolders { get; internal set; } = 0;
        public int IgnoredAuditFiles { get; internal set; } = 0;
        public int FolderPrinted { get; internal set; } = 0;
        public int HashPendAtEnd { get; internal set; } = 0;


        public int HashCallTiny { get; internal set; } = 0;
        public int HashCallSmall { get; internal set; } = 0;
        public int HashCallHuge { get; internal set; } = 0;
        public int HashCallMedium { get; internal set; } = 0;

        public Stopwatch ScanTime { get; internal set; } = new Stopwatch();
        public Stopwatch PendAtEndTime { get; internal set; } = new Stopwatch();
        public Stopwatch HashTimeSmall { get; internal set; } = new Stopwatch();
        public Stopwatch HashTimeMedium { get; internal set; } = new Stopwatch();
        public Stopwatch HashTimeHuge { get; internal set; } = new Stopwatch();

        public bool Verbose { get; set; } = false;

        public HashSet<string> IgnoreFolders = new HashSet<string>
        {
            "adobe",
            "application data",
            "backup",
            "backups",
            "canonbj",
            "debug",
            "desktop",
            "documents",
            "downloads",
            "intel",
            "mcafee",
            "profiles",
            "users",
            "temporary internet files",
            "toshiba",
            "node_modules",
            "preemptive solutions",
            ":\\opt",
            ":\\$getcurrent",
            ":\\config.msi",
            ":\\msocache",
            ":\\$sysreset",
            ":\\$windows.~bt",
            ":\\~windows.~ws",
            ":\\$recycle.bin",

            ":\\documents and settings",
            ":\\perflogs",

            ":\\program files\\adobe",
            ":\\program files\\apple software update",
            ":\\program files\\bonjour", //Bonjour Print Services
            ":\\program files\\bonjour print services",
            ":\\program files\\common files\\microsoft shared",
            ":\\program files\\common files\\skype",
            ":\\program files\\conexant",
            ":\\program files\\cyberlink",
            ":\\program files\\dotnet",
            ":\\program files\\etcher",
            ":\\program files\\garmin",
            ":\\program files\\git",
            ":\\program files\\realtek",
            ":\\program files\\google",
            ":\\program files\\imagewriter",
            ":\\program files\\intel",
            ":\\program files\\installshield installation information",
            ":\\program files\\toshiba",
            ":\\program files\\microsoft office",
            ":\\program files\\microsoft works",
            ":\\program files\\microsoft sql server",
            ":\\program files\\microsoft web tools",
            ":\\program files\\microsoft visual studio",
            ":\\program files\\microsoft visual studio 14.0",
            ":\\program files\\msbuild",
            ":\\program files\\microsoft sdks",
            ":\\program files\\microsoft.net",
            ":\\program files\\mozilla firefox",
            ":\\program files\\mozilla maintenance service",
            ":\\program files\\microsoft silverlight",
            ":\\program files\\nodejs",
            ":\\program files\\netgear",
            ":\\program files\\skype",
            ":\\program files\\synaptics",
            ":\\program files\\reference assemblies\\microsoft",
            ":\\program files\\windows defender",
            ":\\program files\\windows kits",
            ":\\program files\\windows nt",
            ":\\program files\\windowsapps\\microsoft",

            ":\\programdata\\application data",
            ":\\programdata\\temp",
            ":\\programdata\\microsoft",
            ":\\programdata\\microsoft dnx",
            ":\\programdata\\package cache",
            //":\\programdata\\microsoft\\diagnosis",
            ":\\programdata\\microsoft\\windows\\wer",
            //":\\programdata\\microsoft\\windows defender",
            //":\\programdata\\microsoft\\windows\\appRepository\\packages",
            ":\\programdata\\usoprivate",

            ":\\system volume information",
            ":\\recovery",

            ":\\users",
            ":\\users\\all users\\microsoft\\windows\\wer",

            ":\\windows",
            ":\\windows\\apppatch",
            ":\\windows\\inf",
            ":\\windows\\infusedapps",
            ":\\windows\\syswow64\\tasks",
            ":\\windows\\syswow64\\wbem",
            ":\\windows\\systemresources",
            ":\\windows\\winsxs",
            ":\\windows10upgrade"
        };

        private HashSet<string> IgnoreFilesInFolders = new HashSet<string>
        {
                        ":\\windows"
        };

        private HashSet<string> AuditFilesOfType = new HashSet<string>
        {
                        ".exe",
                        ".dll",
                        ".aspx",
                        ".js"
        };
        private HashSet<string> IgnoreFilesOfType = new HashSet<string>
        {
                        ".bin",
                        ".dmp",
                        ".sys",
                        ".cpl"
        };

        private ACESet _ACESet;
        private ACLSet _ACLSet;

        private ConcurrentBag<Task> driveFileTasks = new ConcurrentBag<Task>();

        private string _DiskName = String.Empty; 

        private CIMDirectoryCollection directories = new CIMDirectoryCollection();

        private void TryAdd(HashSet<string> addlist, StringCollection additem)
        {
            foreach (var line in additem)
            {
                if (!String.IsNullOrWhiteSpace(line))
                {
                    string cleanline = line.Trim().ToLowerInvariant();
                    if (!addlist.Contains(cleanline))
                    {
                        addlist.Add(cleanline);
                    }
                }
            }
        }

        public RecurseDirectory(StringCollection ignoreFolders, 
                                StringCollection ignoreFilesInFolders,
                                StringCollection ignoreFilesOfType, 
                                StringCollection auditFilesOfType)
        {
            TryAdd(IgnoreFolders, ignoreFolders);
            TryAdd(IgnoreFilesInFolders, ignoreFilesInFolders);
            TryAdd(IgnoreFilesOfType, ignoreFilesOfType);
            TryAdd(AuditFilesOfType, auditFilesOfType);
        }

        public void RootFolder(DirectoryInfo rootDirectory)
        {
            _DiskName = rootDirectory.FullName;
            _ACESet = new ACESet();
            _ACLSet = new ACLSet();
            var now = DateTime.Now.ToString("u");
            Console.WriteLine($"{now} : Disk {_DiskName}, Inventory Start.");
            ScanTime.Start();
            int errorCount = EachFolder(rootDirectory, 0, 0);
            if (driveFileTasks.Count > 0)
            {   // anything hashed?
                PendAtEndTime.Start();
                var taskArray = driveFileTasks.Where(x => !x.IsCompleted).ToArray();
                if (taskArray.Length > 0)
                {   // anything still left to hash?
                    Task.WaitAll(taskArray);    // wait for any file scans to finish!
                    HashPendAtEnd = taskArray.Length;
                }
                PendAtEndTime.Stop();
            }
            ScanTime.Stop();
            SummaryStatistics(rootDirectory.FullName, errorCount);

        }
        private void SummaryStatistics(string dirname, int errorCount)
        {
            const string strFormat3 = "#0.000";
            const string strFormat1 = "#0.0";
            var scanms = ScanTime.Elapsed.TotalMilliseconds;
            var pendmillis = PendAtEndTime.Elapsed.TotalMilliseconds.ToString(strFormat1);
            var scansec = ScanTime.Elapsed.TotalSeconds.ToString(strFormat3);
            var hugems = HashTimeHuge.Elapsed.TotalMilliseconds;
            var hugesec = (hugems / 1000.0).ToString(strFormat3);
            var medms = HashTimeMedium.Elapsed.TotalMilliseconds;
            var medsec = (medms / 1000.0).ToString(strFormat3);
            var smallms = HashTimeSmall.Elapsed.TotalMilliseconds;
            var smallsec = (smallms / 1000.0).ToString(strFormat3);
            var hugeper = string.Format("{0:#0.000}", hugems / HashCallHuge);
            var medper = string.Format("{0:#0.000}", medms / HashCallMedium);
            var smallper = string.Format("{0:#0.000}", smallms / HashCallSmall);
            var othersec = string.Format("{0:#0.000}", (scanms - hugems - medms - smallms) / 1000.0);
            var driveID = _DiskName.Replace(Path.DirectorySeparatorChar.ToString(), "");
            var now = DateTime.Now.ToString("u");
            Console.WriteLine($"{now} : Disk: {driveID}, Folders: {StartedFolders}; Files: {StartedFiles};  Scan: {scansec}s;\n" +
                    $"Hash exe: Huge: {HashCallHuge} @ {hugesec}s={hugeper}ms/1; " + 
                    $"Medium: {HashCallMedium} @ {medsec}s={medper}ms/1; " + 
                    $"Small: {HashCallSmall} @ {smallsec}s={smallper}ms/1;\n" + 
                    $"Tiny: {HashCallTiny}; Hashes Pending at scan complete: {HashPendAtEnd}; Pending time: {pendmillis}ms; " +  
                    $"Other: {othersec}s; Errors: {errorCount}.");
        }

        public void Save()
        {
            var machine = Environment.MachineName;
            var driveID = _DiskName.Replace(Path.DirectorySeparatorChar.ToString(), "").Replace(":","");
            var filePath = "DiskInventory";
            if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);
            var filePrefix = $"{filePath}{Path.DirectorySeparatorChar}{machine}_Disk_{driveID}_";
            {
                var acls = _ACLSet.ToJSON();
                GZfileIO.WriteStringToGZ($"{filePrefix}Acls.js", acls);
                GZfileIO.WriteStringToGZ($"{filePrefix}Acls.js.gz", acls);
            }
            {
                var dirs = directories.ToJSON();
                GZfileIO.WriteStringToGZ($"{filePrefix}Files.js", dirs);
                GZfileIO.WriteStringToGZ($"{filePrefix}Files.js.gz", dirs);
            }
        }

        private int EachFolder(System.IO.DirectoryInfo dir, int depth, int parentAcl)
        {
            StartedFolders++;
            int errorCount = 0;
            //var dir = new System.IO.DirectoryInfo(dirName);
            var acl = ACLParse(dir);
            var aclID = acl.Item1;
            bool allInherited = acl.Item3;
            if ((!allInherited) && (acl.Item1 == parentAcl))
            {   // semi-inherited - its the same as the parent.
                allInherited = true;
            }

            if (depth < 2 || !allInherited)
            {
                var dirFullName = dir.FullName;
                var errmsg = acl.Item2;
                FolderPrinted++;
                if (errmsg.Length > 0)
                {
                    if (directories.Directories.TryAdd(dir.FullName,
                        new CIMDirectoryInfo
                        {
                            AclSameAsParent = allInherited,
                            ErrorAccessing = true,
                            ACLid = -1
                        }
                    ))
                    {
                        Console.WriteLine($"[{FolderPrinted}]$ {dir.FullName} :  : {errmsg}");
                    }
                    else
                    {
                        Console.WriteLine($"[{FolderPrinted}]$ {dir.FullName} :  : TryAddAdd Failed {errmsg}");
                    }
                }
                else
                {
                    if (Verbose) {
                        var aclString = _ACLSet.GetACL(aclID).ACLString();
                        Console.WriteLine($"[{FolderPrinted}]$ {dirFullName} \n:{aclID}\t{aclString};");
                    }
                    ////bool found = directories.Directories.TryGetValue(dirFullName, out DirectoryInfo dinfo);
                    if(directories.Directories.TryAdd(dirFullName,
                        new CIMDirectoryInfo
                        {
                            AclSameAsParent = allInherited,
                            ACLid = aclID,
                            ErrorAccessing = false
                        }
                    ))
                    {

                    }
                    else
                    {
                        Console.WriteLine($"[{FolderPrinted}]$ {dirFullName} \n:{aclID}\t: TryAdd failed;");
                    }
                }
            }
            if (ParseWanted(depth, dir))
            {
                // skip files in folders we cant read.
                AllFiles(dir, aclID, allInherited);  // this isnt right place...
            }


            //TODO: check folder depth  
            if (depth > 20) return errorCount;  // stop after this depth in folder search
            try
            {
                var directories = dir.GetDirectories();

                foreach (var subdir in directories)
                {
                    bool parse = ParseWanted(depth, subdir);
                    if (parse)
                    {
                        try
                        {
                            errorCount += EachFolder(subdir, depth + 1, acl.Item1);
                        }
                        catch (Exception ex)
                        {
                            errorCount++;
                            EachDirException(subdir, ex);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                EachDirException(dir, ex);
            }

            return errorCount;
        }

        private static void EachDirException(DirectoryInfo dir, Exception ex)
        {
            string msg = ex.Message;
            if (msg.ToLower().Contains(dir.Name.ToLower()))
            {
                Console.WriteLine($"# :: {ex.Message}.");
            }
            else
            {
                Console.WriteLine($"# :: {dir.Name} :: {ex.Message}.");
            }
        }

        private bool ParseWanted(int depth, System.IO.DirectoryInfo subdir)
        {
            var parse = true;   // look at this folder?
            var attributes = subdir.Attributes;
            bool offline = (attributes & FileAttributes.Offline) == FileAttributes.Offline;
            bool reparse = (attributes & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint;
            if (offline) return false;
            if (reparse) return false;
            var subLower = subdir.Name.ToLower();
            var subLower32 = subLower.Replace(" (x86)", "");
            var subDirPath = subdir.FullName;
            var subdirpath = subDirPath.Remove(0, subDirPath.IndexOf(":")).ToLower();
            var subdirpath32 = subdirpath.Replace(" (x86)", "");
            // ignore these folders
            if (depth < 8)
            {
                if (IgnoreFolders.Any(s => subLower.Equals(s))) parse = false;
                if (IgnoreFolders.Any(s => subdirpath.Equals(s))) parse = false;
                if (IgnoreFolders.Any(s => subLower32.Equals(s))) parse = false;
                if (IgnoreFolders.Any(s => subdirpath32.Equals(s))) parse = false;
                // \\program files\\windowsapps\\microsoft
                if (subdirpath32.Contains(":\\program files\\windows")) parse = false;
                if (subdirpath32.Contains(":\\program files\\microsoft")) parse = false;
                if (subdirpath32.Contains(":\\program files\\windowsapps\\microsoft.")) parse = false;
                if (subdirpath32.Contains(":\\programdata\\windows")) parse = false;
                if (subdirpath32.Contains(":\\programdata\\microsoft")) parse = false;
            }
            if (!parse) IgnoredFolders++;
            return parse;
        }

        public void AllFiles(DirectoryInfo dir, int aclId, bool allInherited)
        {
            bool ignoreThisFolder = IgnoreFilesInFolders.Contains(dir.Name.ToLower());
            if (ignoreThisFolder) return;
            var dirFullName = dir.FullName;
            var dirPath = dirFullName.ToLower();
            ignoreThisFolder = IgnoreFilesInFolders.Contains(dirPath);
            if (ignoreThisFolder) return;
            var dirPathOnDisk = dirPath.Remove(0, dirPath.IndexOf(":"));
            ignoreThisFolder = IgnoreFilesInFolders.Contains(dirPathOnDisk);
            if (ignoreThisFolder) return;
            bool firstPrint = true;

            CIMDirectoryInfo dirinfo;
            if(directories.Directories.TryGetValue(dirFullName, out CIMDirectoryInfo dinfo))
            {
                dirinfo = dinfo;
            }
            else
            {
                dirinfo = null;
            }
            var types = new CIMFileTypeinfo();
            foreach (var file in dir.GetFiles())
            {
                StartedFiles++;
                //TODO: handle file type statistics
                var fileNameOriginal = file.Name;   // original
                //var filename = fileNameOriginal.ToLower();
                var filetype = file.Extension.ToLower();
                var fileLength = file.Length;
                var fileModifyDate = file.LastWriteTimeUtc;

                var attributes = file.Attributes;
                bool offline = (attributes & FileAttributes.Offline) == FileAttributes.Offline;
                bool reparse = (attributes & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint;


                bool auditThisFileType = AuditFilesOfType.Contains(filetype);
                if (auditThisFileType)
                {
                    bool auditThisFile = true;
                    if (offline) auditThisFile = false;
                    if (reparse) auditThisFile = false;
                    //var modifyDate = file.LastWriteTimeUtc; //.ToString("u");
                    var filebytes = file.Length;
                    var version = string.Empty; // eg: 2006-08-22 06:30:07Z
                    var checksum = string.Empty;
                    bool checksumNeeded = false;
                    if (IsExeFile(file))
                    {
                        try
                        {
                            var versionInfo = FileVersionInfo.GetVersionInfo(file.FullName);
                            var description = "" + versionInfo.FileDescription;      // ignore Microsoft?
                            var company = ("" + versionInfo.CompanyName).ToLower();      // ignore Microsoft?
                            var product = ("" + versionInfo.ProductName).ToLower();      // ignore Windows?
                            var copyright = ("" + versionInfo.LegalCopyright).ToLower(); // ignore Microsoft?
                            if (file.FullName.ToLower().Contains("microsoft")) auditThisFile = false;
                            if (company.Contains("microsoft")) auditThisFile = false;
                            if (product.Contains("microsoft")) auditThisFile = false;
                            if (product.Contains("install")) auditThisFile = false;
                            if (product.Contains("setup")) auditThisFile = false;
                            if (copyright.Contains("microsoft")) auditThisFile = false;
                            if (copyright.Contains("james newton-king")) auditThisFile = false;
                            if (description.Contains("json.net")) auditThisFile = false;
                            if (description.Contains("install")) auditThisFile = false;
                            if (description.Contains("setup")) auditThisFile = false;
                            if (description.Contains("cabinet self-extractor")) auditThisFile = false;
                            if (auditThisFile)
                            {
                                var tmpversion = versionInfo.FileVersion;
                                version = ("" + tmpversion).Replace(" ", String.Empty);
                                checksumNeeded = true;
                            }
                            else
                            {
                                IgnoredAuditFiles++;
                            }
                        }
                        catch (Exception ex)
                        {
                            // exe file with invalid data, skip it;
                            Console.WriteLine($"@Skipping version info for File {dirFullName}\\{fileNameOriginal} : {ex.Message}");
                        }
                    }
                    else
                    {   // not an exe file.
                        //version = modifyDate; // eg: 2006-08-22 06:30:07Z
                        //checksum = GetChecksum(file);   // sha256 file hash // dont read files we dont need to // delay action to task
                        checksumNeeded = true;
                    }
                    if (auditThisFile)
                    {
                        if (firstPrint)
                        {
                            firstPrint = false;
                            Console.WriteLine($"@ {dir.FullName}:");
                        }
                        if (Verbose) Console.WriteLine($"{fileNameOriginal.PadRight(34)}; {fileModifyDate.ToString("u")}; {filebytes.ToString().PadLeft(12)}; {version.PadRight(12)}; {checksum};");
                        if (null == dirinfo)
                        {
                            dirinfo = new CIMDirectoryInfo()
                            {
                                ACLid = aclId,
                                AclSameAsParent = allInherited
                            };
                            if (directories.Directories.TryAdd(dirFullName, dirinfo))
                            {

                            }
                            else
                            {
                                Console.WriteLine($"@TryAdd failed for file {fileNameOriginal}");
                            }
                        }
                        if (checksumNeeded)
                        {
                            bool tiny = false;
                            if (tiny = (fileLength <= 4096))
                            {
                                HashCallTiny++;
                            }

                            if (tiny && HashCallTiny > 100)
                            {
                                // handle small files inline if there are many tiny files
                                // don't want to spin off too many Tasks.
                                var thisfileinfo = new CIMFileInfo()
                                {
                                    Name = fileNameOriginal,
                                    Length = fileLength,
                                    Hash = GetChecksum(file),
                                    UtcModified = fileModifyDate,
                                    Version = version
                                };
                                dirinfo.FileList.Add(fileNameOriginal, thisfileinfo);
                            }
                            else
                            {
                                // spin off task for the long running hash function; or all files -- if not too many tiny files
                                var newTask = Task.Run(() =>
                                {
                                    var thisfileinfo = new CIMFileInfo()
                                    {
                                        Name = fileNameOriginal,
                                        Length = fileLength,
                                        Hash = GetChecksum(file),
                                        UtcModified = fileModifyDate,
                                        Version = version
                                    };
                                    dirinfo.FileList.Add(fileNameOriginal, thisfileinfo);
                                }
                                );
                                driveFileTasks.Add(newTask);    // keep up with these tasks
                            }
                        }
                        else
                        {
                            var thisfileinfo = new CIMFileInfo()
                            {
                                Name = fileNameOriginal,
                                Length = fileLength,
                                Hash = checksum,
                                UtcModified = fileModifyDate,
                                Version = version
                            };
                            dirinfo.FileList.Add(fileNameOriginal, thisfileinfo);
                        }
                    }
                }

                bool ignoreThisFileType = IgnoreFilesOfType.Contains(filetype);
                if (!ignoreThisFileType)
                {
                    // handle random files - find newest, oldest by type, size range, average size
                    if (null == dirinfo)
                    {
                        dirinfo = new CIMDirectoryInfo()
                        {
                            ACLid = aclId,
                            AclSameAsParent = allInherited
                        };
                        if (directories.Directories.TryAdd(dirFullName, dirinfo))
                        {

                        }
                        else
                        {
                            Console.WriteLine($"@ directories.Directory.TryAdd {dirFullName} failed.");
                        }
                    }
                    var typeinfo = dirinfo.Typelist.TryGetValue(filetype, out CIMFileTypeinfo tinfo) ?
                                  tinfo : null;
                    if (null == typeinfo)
                    {
                        typeinfo = new CIMFileTypeinfo()
                        {
                            BytesLargest = fileLength,
                            BytesSmallest = fileLength,
                            BytesUsedAll = 0,
                            FileCount = 0,
                            UtcNewest = fileModifyDate,
                            UtcOldest = fileModifyDate,
                            FileType = filetype,
                            FileName1 = fileNameOriginal
                        };
                        dirinfo.Typelist.Add(filetype, typeinfo);
                    }
                    typeinfo.FileCount++;
                    typeinfo.BytesUsedAll += fileLength;
                    if (typeinfo.BytesLargest < fileLength) typeinfo.BytesLargest = fileLength;
                    if (typeinfo.BytesSmallest > fileLength) typeinfo.BytesSmallest = fileLength;
                    if (typeinfo.UtcNewest > fileModifyDate) typeinfo.UtcNewest = fileModifyDate;
                    if (typeinfo.UtcOldest < fileModifyDate) typeinfo.UtcOldest = fileModifyDate;
                    if (typeinfo.FileName1 != fileNameOriginal) typeinfo.FileName2 = fileNameOriginal;
                }

            }
        }
        private bool IsExeFile(string path)
        {
            var twoBytes = new byte[2];
            using (var fileStream = File.Open(path, FileMode.Open))
            {
                fileStream.Read(twoBytes, 0, 2);
            }

            return Encoding.UTF8.GetString(twoBytes) == "MZ";
        }
        private bool IsExeFile(FileInfo file)
        {
            const int SmallestPEFileByteSize = 97; // https://stackoverflow.com/questions/38205729/how-can-an-executable-be-this-small-in-file-size
            if (file.Length < SmallestPEFileByteSize) return false;
            var path = file.FullName;
            var twoBytes = new byte[2];
            using (var fileStream = file.OpenRead()) // File.Open(path, FileMode.Open))
            {
                fileStream.Read(twoBytes, 0, 2);
            }

            return Encoding.UTF8.GetString(twoBytes) == "MZ";
        }

        // http://peterkellner.net/2010/11/24/efficiently-generating-sha256-checksum-for-files-using-csharp/
        // mod by JDJohnston to Fileinfo and base64 string (shorter than hex, exactly same accuracy)
        public string GetChecksum(FileInfo file)
        {
            const int maxAuditFileLength = 10_000_000; // don't read files above this size .. 99% of exe files < 19_000_000
            const int datalength = 4_000_000;       // read entirity of files up to this size
            const int datahalf = datalength / 2;    // process half that at front and end of file if mediumsize
            const long seekto = -datahalf;
            long fileLength = file.Length;
            if (fileLength <= datalength)
            {   // read entirity of small to medium size files
                HashCallSmall++;
                HashTimeSmall.Start();
                //byte[] foo = File.ReadAllBytes(file.FullName);
                using (FileStream stream = file.OpenRead()) // this shares with open files, no crash
                {
                    var sha = new SHA256Managed();
                    byte[] checksum = sha.ComputeHash(stream);
                    HashTimeSmall.Stop();
                    return Convert.ToBase64String(checksum); //.Replace("=", String.Empty);
                }
            }
            else if (fileLength > maxAuditFileLength)
            {   // read first region only of huge files
                HashCallHuge++;
                HashTimeHuge.Start();
                var partialFileBytes = new byte[datalength];
                using (FileStream stream = file.OpenRead())
                {
                    //var filebytesFirst = new byte[datalength];
                    //var filebytesEnd = new byte[datalength];
                    stream.Read(partialFileBytes, 0, datalength);
                    //stream.Read(partialFileBytes, 0, datahalf);
                    //stream.Seek(seekto, SeekOrigin.End);
                    //stream.Read(filebytesEnd, datalength, datalength);
                    //stream.Read(partialFileBytes, datahalf, datahalf);
                    var sha = new SHA256Managed();
                    byte[] checksum = sha.ComputeHash(partialFileBytes);
                    HashTimeHuge.Stop();
                    return Convert.ToBase64String(checksum).Replace("=", "#");  // use # as marked for partial hash
                }
            }
            else
            {   // read first and last region of medium large files
                HashCallMedium++;
                HashTimeMedium.Start();
                var partialFileBytes = new byte[datalength];
                using (FileStream stream = file.OpenRead())
                {
                    //var filebytesFirst = new byte[datalength];
                    //var filebytesEnd = new byte[datalength];
                    //stream.Read(filebytesFirst, 0, datalength);
                    stream.Read(partialFileBytes, 0, datahalf);
                    stream.Seek(seekto, SeekOrigin.End);
                    //stream.Read(filebytesEnd, datalength, datalength);
                    stream.Read(partialFileBytes, datahalf, datahalf);
                    var sha = new SHA256Managed();
                    byte[] checksum = sha.ComputeHash(partialFileBytes);
                    HashTimeMedium.Stop();
                    return Convert.ToBase64String(checksum).Replace("=", "#");  // use # as marked for partial hash
                }
            }
        }

        public Tuple<int, string, bool> ACLParse(System.IO.DirectoryInfo directoryInfo)
        {
            string error = string.Empty;
            int aclId = 0;
            bool allInherited = true;

            try
            {
                var dirSecurity = directoryInfo.GetAccessControl();
                var authRuleColl = dirSecurity.GetAccessRules(true, true, typeof(NTAccount));
                var acl = new ACL();
                foreach (FileSystemAccessRule fsaRule in authRuleColl)
                {
                    if (!fsaRule.IsInherited) allInherited = false;
                    var ace = new ACE(fsaRule);
                    var id = _ACESet.GetId(ace);    // add this ace to the aceset;
                    acl.Add(ace, id);
                }
                acl.Seal();
                aclId = _ACLSet.GetId(acl);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            finally
            {

            }
            return Tuple.Create(aclId, error, allInherited);
        }

    }
}
