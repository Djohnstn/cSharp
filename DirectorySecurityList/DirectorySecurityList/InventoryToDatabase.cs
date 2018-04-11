using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIMSave;

namespace DirectorySecurityList
{
    class InventoryToDatabase
    {

        private const string ACLJsFileName = "Acls.js.gz";
        private const string FileJsFileName = "Files.js.gz";

        private ACESet _ACESet;
        private ACLSet _ACLSet;

        private CIMDirectoryCollection directories = new CIMDirectoryCollection();

        private string _DiskName = "C";

        public void Save()
        {
            var machine = Environment.MachineName;
            var driveID = _DiskName.Replace(Path.DirectorySeparatorChar.ToString(), "").Replace(":", "");
            var filePath = "DiskInventory";
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
            var filePath = "DiskInventory";
            if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);
            var filePrefix = $"{filePath}{Path.DirectorySeparatorChar}*_Disk_*_";
            foreach(var aclfile in System.IO.Directory.EnumerateFiles(filePrefix + ACLJsFileName))
            {
                var filefile = aclfile.Replace(ACLJsFileName, FileJsFileName);
                if (File.Exists(filefile))
                {
                    HandleFile(aclfile, filefile);
                }
            }

        }

        private void HandleFile(string aclfile, string filefile)
        {
            {
                var aclstring = GZfileIO.ReadGZtoString(aclfile);
                var filestring = GZfileIO.ReadGZtoString(filefile);
                //_ACESet = ACLSet.FromJSON(aclstring);
                //directories = CIMDirectoryCollection.FromJSON(filestring);
            }
        }

        // see cimsave.findfiles.PartsToDataSet
        private void ACLsToDataSet()
        {
        }

    }
}
