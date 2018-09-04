using System.IO;
using System.Threading.Tasks;
using CIMCollect;

namespace DirectorySecurityList
{
    class DiskInventory
    {
        private CIMCollect.Properties.Settings _settings;
        public DiskInventory(CIMCollect.Properties.Settings settings, string saveToFolder)
        {
            _settings = settings;
        }
        public void EachDisk()
        {

            DriveInfo[] allDrives = DriveInfo.GetDrives();
            // foreach(DriveInfo d in allDrives)
            //{
            //    if (d.IsReady && d.DriveType == DriveType.Fixed)
            //    {
            //        EachFolder(d.RootDirectory, 0, 0);
            //    }
            //}
            // spin a task for each disk drive
            Parallel.ForEach(allDrives, d =>
            {
                if (d.IsReady && d.DriveType == DriveType.Fixed)
                {
                    var igfolders = _settings.IgnoreFolders;
                    var igfilesinfolders = _settings.IgnoreFilesInFolders;
                    var igfiletypes = _settings.IgnoreFilesOfType;
                    var auditfiles = _settings.AuditFilesOfType;
                    var x = new RecurseDirectory(igfolders, igfilesinfolders, igfiletypes, auditfiles);
                    x.RootFolder(d.RootDirectory);
                    x.Save();
                }
            });
        }



    }
}
