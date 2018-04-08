using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.InteropServices;

namespace HelloDCOM1
{
    [ClassInterface(ClassInterfaceType.None)]
    public class HelloDCOM : System.EnterpriseServices.ServicedComponent, IHelloDCOM
    {
        public string UserID
        {
            get
            {
                return Environment.UserDomainName + Path.DirectorySeparatorChar + Environment.UserName;
            }
        }
        public string Computer { get; } = Environment.MachineName;
        public string Time
        {
            get
            {
                return DateTime.UtcNow.ToString("s");
            }
        }
        public string Info { get; internal set; }
        public void Save(string info)
        {
            this.Info = info;
            var FilePath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + Path.DirectorySeparatorChar + "HelloDCOM.txt";
            using (StreamWriter oFile = new StreamWriter(FilePath))
            {
                XmlSerializer oXmlSerializer = new XmlSerializer(typeof(HelloDCOM));
                oXmlSerializer.Serialize(oFile, this);
            }
        }
    }
}
