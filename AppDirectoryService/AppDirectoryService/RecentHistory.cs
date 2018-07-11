using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppDirectoryService
{
    // results of query of recent history of reported apps
    public class RecentHistory
    {
        public string appname;
        public string pc;
        public DateTimeOffset when;
        public string status;
    }

    public class InsertedHistory
    {
        public int updates;
        public string status;
    }

}
