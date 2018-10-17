using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectorySecurityList;

namespace CIMCollect
{
    class Program
    {

     /*
     * NLog tutorial suggests to use a static Logger, so here it is
     */
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            CIMSave.CommandlineParameters.Set(args);
            //Test();

            var collect = new Collect(); //).AllConfig();
            collect.SQLCollect();
            collect.FileCollect();
            collect.AllConfig();
            Utilities.SemiPause("Collected all data, press any key to exit.", 30);
        }

        static void Test()
        {
            //var foo = CIMCollect.SqlClasses.SqlWordDictionary.WordHashBase;
            //var t = new InventoryToDatabase();
            //t.FindFiles();
        }
    }

}
