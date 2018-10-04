using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Globalization;
using System.Threading;
using System.IO;
using System.Collections.Concurrent;
using CIMSave;

namespace DirectorySecurityList
{

    [DataContract]
    class CIMFileInfo
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public long Length { get; set; }
        [DataMember]
        public DateTime UtcModified { get; set; }
        [DataMember]
        public string Version { get; set; }
        [DataMember]
        public string Hash { get; set; }
    }

    [DataContract]
    class CIMFileTypeinfo
    {
        [DataMember]
        public string FileType { get; set; }
        [DataMember]
        public int FileCount { get; set; }
        [DataMember]
        public long BytesUsedAll { get; set; }
        [DataMember]
        public long BytesSmallest { get; set; }
        [DataMember]
        public long BytesLargest { get; set; }
        [DataMember]
        public DateTime UtcOldest { get; set; }
        [DataMember]
        public DateTime UtcNewest { get; set; }
        [DataMember]
        public string FileName1 { get; set; }
        [DataMember]
        public string FileName2 { get; set; } = "";
    }

    [DataContract]
    class CIMDirectoryInfo
    {
        [DataMember]
        public string DirectoryName { get; set; }
        [DataMember]
        public bool AclSameAsParent { get; set; }
        [DataMember]
        public bool ErrorAccessing { get; set; }
        [DataMember]
        public int ACLid { get; set; }
        // type name, filetype info
        [DataMember]
        public Dictionary<string, CIMFileTypeinfo> Typelist = new Dictionary<string, CIMFileTypeinfo>();
        // file name, file info
        [DataMember]
        public Dictionary<string, CIMFileInfo> FileList = new Dictionary<string, CIMFileInfo>();

        //public CIMDirectoryInfo()
        //{
        //    Typelist = new Dictionary<string, CIMFileTypeinfo>();
        //    FileList = new Dictionary<string, CIMFileInfo>();
        //}
    }

    [DataContract]
    class CIMDirectoryCollection
    {
        // dictionary name, dictionary info
        [DataMember]
        public ConcurrentDictionary<string, CIMDirectoryInfo> Directories = new ConcurrentDictionary<string, CIMDirectoryInfo>();

        //public CIMDirectoryCollection()
        //{
        //    this.Directories = new Dictionary<string, CIMDirectoryInfo>();
        //}
        public string ToJSON()
        {
            string json;
            var currentCulture = Thread.CurrentThread.CurrentCulture;
            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(CIMDirectoryCollection));
                using (MemoryStream ms = new MemoryStream())
                {
                    js.WriteObject(ms, this);
                    ms.Position = 0;
                    using (StreamReader sr = new StreamReader(ms))
                    {
                        json = sr.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                json = ex.ToString();
                Console.WriteLine(json);
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = currentCulture;
            }
            return json;
        }

        public static CIMDirectoryCollection FromJSON(string jsonfilename)
        {
            return GZfileIO.ReadGZtoPOCO<CIMDirectoryCollection>(jsonfilename);
            //return GZfileIO.ReadGZtoJson<CIMDirectoryCollection>(jsonfilename);
        }

        internal void ToDB(ACLSet aclset)
        {


            throw new NotImplementedException();
        }
    }
}
