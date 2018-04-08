using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelloDCOM1;

namespace TestHelloDCOM
{
    class Program
    {
        static void Main(string[] args)
        {
            var obj = (HelloDCOM)Activator.CreateInstance(Type.GetTypeFromProgID("HelloDCOM1.HelloDCOM", "localhost", true));

            //var obj = new HelloDCOM1.HelloDCOM();
            //Console.WriteLine(obj.Computer);
            ///obj.Save($"Hello {DateTime.Now}");
            var ix = 0;
        }
    }
}
