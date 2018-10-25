using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CIMCollect;

namespace DirectorySecurityList
{
    class DiskInventory
    {
        private CIMCollect.Properties.Settings _settings;
        private readonly string SaveToFolder;
        public DiskInventory(CIMCollect.Properties.Settings settings, string saveToFolder)
        {
            _settings = settings;
            SaveToFolder = saveToFolder;
        }
        public void EachDisk()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            // limit to two disks at a time
            var TwoTasks = new ParallelOptions { MaxDegreeOfParallelism = 2 };
            // spin a task for each disk drive
            Parallel.ForEach(allDrives, TwoTasks, d => NiceInventoryRunner(d));
        }

        private void NiceInventoryRunner(DriveInfo d)
        {
            var prevPriority = Thread.CurrentThread.Priority;
            Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;
            Inventory(d);
            Thread.CurrentThread.Priority = prevPriority;
        }

        private void Inventory(DriveInfo d)
        {
            if (d.IsReady && d.DriveType == DriveType.Fixed)
            {
                var igfolders = _settings.IgnoreFolders;
                var igfilesinfolders = _settings.IgnoreFilesInFolders;
                var igfiletypes = _settings.IgnoreFilesOfType;
                var auditfiles = _settings.AuditFilesOfType;
                var x = new RecurseDirectory(igfolders, igfilesinfolders, igfiletypes, auditfiles, SaveToFolder);
                x.RootFolder(d.RootDirectory);
                x.Save();
            }
        }
    }
}
