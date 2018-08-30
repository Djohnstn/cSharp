using System;
using System.Diagnostics;

namespace DirectorySecurityList
{
    class Program
    {
        static void Main(string[] args)
        {
            Properties.Settings settings = Properties.Settings.Default;

            {
                var sql = new MsSqlInventory();
                sql.Inventory();
            }
            Pause();
            {
                var diskInventory = new DiskInventory(settings);
                diskInventory.EachDisk();
            }

            Pause();
        }

        private static void Pause()
        {
            Console.Write("Done.");
            if (Environment.UserInteractive)
            {
                try
                {
                    if (Debugger.IsAttached)
                    {
                        Console.ReadLine();
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(8000);
                    }
                }
                catch (Exception)
                {
                }
            }
        }
    }


}
