using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CIMCollect
{
    class Program
    {

     /*
     * NLog tutorial suggests to use a static Logger, so here it is
     */
        private static Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            var collect = new Collect(); //).AllConfig();
            collect.AllConfig();
        }
    }

}
