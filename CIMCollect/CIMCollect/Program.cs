using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CIMCollect
{
    class Program
    {
        static void Main(string[] args)
        {
            //var pstest = new PSRunner();
            //pstest.Test();
            //Test();
            //pstest.Pause();
            var collect = new Collect();
            collect.AllConfig();
        }



/*
        private static void Test()
        {
            var server = Environment.MachineName;
            var pstest = new PSRunner();

            {
                var result = pstest.ToFile(server, "BIOS", "Manufacturer",
                    @"([wmisearcher]""Select * from win32_bios"").Get()");
               // Console.WriteLine($"BIOS rc={result}");
            }
            {
                var result = pstest.ToFile(server, "PhysicalMemory", "Name",
                    @"([wmisearcher]""Select * from Win32_PhysicalMemory"").Get()");
                //Console.WriteLine($"PhysicalMemory rc={result}");
            }
            {
                var result = pstest.ToFile(server, "Certificate", "FriendlyName",
                    @"Get-ChildItem Cert:\LocalMachine\My | Select FriendlyName , Thumbprint, Issuer, Subject, NotAfter, NotBefore, SerialNumber, Version, Handle");
               // Console.WriteLine($"Certificate rc={result}");
            }
            {
                var result = pstest.ToFile(server, "Service", "Name",
                    @"Get-WMIObject Win32_Service|Select Name, pathname, State,Caption,Description,ErrorControl,DelayedAutoStart,DisplayName,Started,StartMode,Start");
                //Console.WriteLine($"Service rc={result}");
            }
            pstest.Pause();
        }

    */

    }

}
