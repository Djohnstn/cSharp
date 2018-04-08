using System;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.InteropServices;

// password for strong name file is in usual place.

// https://www.c-sharpcorner.com/uploadfile/yougerthen/create-dcom-application-from-within-net-environment-part-v/

namespace HelloDCOM
{
    public class Hello
    {
        public string UserID { get {
                return Environment.UserDomainName + Path.DirectorySeparatorChar + Environment.UserName;
            }
        }
        public string Computer { get; } = Environment.MachineName;
        public string Time { get
            {
                return DateTime.UtcNow.ToString("s");
            }
        }
        public void Save(string info)
        {
            var FilePath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + Path.DirectorySeparatorChar + "HelloDCOM.txt";
            using (StreamWriter oFile = new StreamWriter(FilePath))
            {
                XmlSerializer oXmlSerializer = new XmlSerializer(typeof(Hello));
                oXmlSerializer.Serialize(oFile, this);
            }
        }
    }
}
