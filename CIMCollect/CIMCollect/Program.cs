﻿using NLog;
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
            Test();

            //var collect = new Collect(); //).AllConfig();
            //collect.SQLCollect();
            //collect.FileCollect();
            //collect.AllConfig();
        }

        static void Test()
        {
            var t = new InventoryToDatabase();
            t.FindFiles();
        }
    }

}
