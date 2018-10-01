using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIMSave;

namespace DirectorySecurityList
{
    public class InventoryToDatabase
    {

        private const string ACEJsFileName = "Aces.Json.gz";
        private const string ACLJsFileName = "Acls.Json.gz";
        private const string FileJsFileName = "Files.Json.gz";

        //private ACESet _ACESet;
        private ACLSet _ACLSet;

        private CIMDirectoryCollection directories = new CIMDirectoryCollection();

        private string _DiskName = "C";

        public void Save()
        {
            var machine = Environment.MachineName;
            var driveID = _DiskName.Replace(Path.DirectorySeparatorChar.ToString(), "").Replace(":", "");
            var filePath = CommandlineParameters._fileSaveFolder; // GZfileIO.GetSaveFolderName();
            if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);
            var filePrefix = $"{filePath}{Path.DirectorySeparatorChar}{machine}_Disk_{driveID}_";
            {
                var acls = _ACLSet.ToJSON();
                GZfileIO.WriteStringToGZ($"{filePrefix}Acls.js", acls);
                GZfileIO.WriteStringToGZ($"{filePrefix}{ACLJsFileName}", acls);
            }
            {
                var dirs = directories.ToJSON();
                GZfileIO.WriteStringToGZ($"{filePrefix}Files.js", dirs);
                GZfileIO.WriteStringToGZ($"{filePrefix}{FileJsFileName}", dirs);
            }
        }

        public void FindFiles()
        {
            //var gz = new GZfileIO();
            var filePath = CommandlineParameters._fileSaveFolder; //GZfileIO.GetSaveFolderName();
            if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);
            //var dir = Directory.GetDirectoryRoot(filePath);
            var filePrefix = "*_Disk_*_";
            var aclFilePattern = filePrefix + ACLJsFileName;
            foreach (var aclfile in Directory.EnumerateFiles(filePath, aclFilePattern, SearchOption.TopDirectoryOnly))
            {
                var ix = aclfile.IndexOf(ACLJsFileName, StringComparison.InvariantCultureIgnoreCase);
                if (ix > 0)
                {
                    var filefile = aclfile.Remove(ix) + FileJsFileName;
                    if (File.Exists(filefile))
                    {
                        HandleFile(aclfile, filefile);
                    }
                }
            }

        }

        private void HandleFile(string aclfile, string filefile)
        {
            {
                //var aclstring = GZfileIO.ReadGZtoString(aclfile);
                //var filestring = GZfileIO.ReadGZtoString(filefile);
                //_ACESet = ACESet.FromJSON(aclfile);
                //int ix = 0;
                _ACLSet = ACLSet.FromJSON(aclfile);
                _ACLSet.ToDB();
                directories = CIMDirectoryCollection.FromJSON(filefile);
            }
        }

        // see cimsave.findfiles.PartsToDataSet
        private void ACLsToDataSet()
        {
        }

    }
}
