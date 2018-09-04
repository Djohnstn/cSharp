using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Threading;
//using System.Management.Automation;
//using System.Management.Automation.Runspaces;
//using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Globalization;

namespace CIMCollect
{
    [DataContract]
    public class InfoPart
    {
        [DataMember]
        public string Identity { get; set; }    // eg part0     -> column "Name" aka Element
        [DataMember]
        public int Index { get; set; }    // eg 1, 2, 3, ... per named element, keeps names from colliding
        [DataMember]
        public string Name { get; set; }        // eg attribute name
        [DataMember]
        public string Type { get; set; }        // eg attribute type (string, int32,...)
        [DataMember]
        public string Value { get; set; }       // attribute value

    }

    [DataContract]
    public class InfoInstance
    {
        [DataMember]
        public string Instance { get; set; }    // eg part0     -> column "Instance" aka Sql Server partition/Instance
        [DataMember]
        public string Path { get; set; }    // eg part0     -> column "Path" aka Database or folder path

        //[DataMember]
        //public List<InfoPart> PartsList;

        public override int GetHashCode()
        {
            var hash = ~this.Instance.GetHashCode();    // bitwise not Instance hash
            hash ^= this.Path.GetHashCode();            // xor with Path hash
            return hash;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is InfoInstance infoInstance))
            {
                return false;
            }

            if (!infoInstance.Instance.Equals(this.Instance))
            {
                return false;
            }

            if (!infoInstance.Path.Equals(this.Path))
            {
                return false;
            }

            return true;
        }


    }

    [DataContract]
    public class InfoParts
    {

        [DataMember]
        public string Server { get; set; }      // computer name -> column "Server" / "ServerId"
        [DataMember]
        public string Set { get; set; }         // eg memory    -> table name
        [DataMember]
        public DateTime AsOf { get; set; }         // eg data collection time 

        //  if Instances and Paths are used, set these flags, maybe not needed
        //[DataMember]
        //public bool InstanceUsed { get; set; } = false;    // InstanceUsed -> column "Instance" aka Sql Server partition/Instance
        //[DataMember]
        //public bool PathUsed { get; set; } = false;   // PathUsed yes/no   -> column "Path" aka Database or folder path

        [DataMember]
        public Dictionary<InfoInstance, List<InfoPart>> Parts;

        public string Result { get; set; } = "";

        public InfoParts(string server, string set)
        {
            Server = server;
            Set = set;
            AsOf = DateTime.UtcNow;
            Parts = new Dictionary<InfoInstance, List<InfoPart>>();
        }

        public InfoParts(string server, string set, DateTime dateTime)
        {
            Server = server;
            Set = set;
            AsOf = dateTime;
            Parts = new Dictionary<InfoInstance, List<InfoPart>>();
        }

        public InfoParts()
        {
            Server = "";
            Set = "";
            AsOf = DateTime.UtcNow;
            Parts = new Dictionary<InfoInstance, List<InfoPart>>();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var id in Parts)
            {
                var kInst = id.Key.Instance.Length > 0 ? $"\\{id.Key.Instance}" : String.Empty;
                var kPath = id.Key.Path.Length > 0 ? $"@{id.Key.Path}" : String.Empty;
                foreach (var p in id.Value )
                {   // pseudo url format [table]://server\instance@directory/object(objectnumber)/attributename(type)=value
                    sb.AppendLine($"[{Set}]://{Server}{kInst}{kPath}/{p.Identity}({p.Index})/{p.Name}({p.Type})={p.Value}");
                }
            }
            return sb.ToString();
        }

        public void Add(string identity, int index, string name, string type, string value)
        {
            if (null == value)
            {
                // bail if sent a null value.
            }
            else
            {
                this.Add(string.Empty, string.Empty, identity, index, name, type, value);
                //var p = new InfoPart()
                //{
                //    //InstanceUsed = false,
                //    //Instance = string.Empty,
                //    //PathUsed = false,
                //    //Path = string.Empty,
                //    Identity = identity,
                //    Index = index,
                //    Name = name,
                //    Type = (type.Equals("String")) ? $"${value.Length}" : type,
                //    Value = value
                //};

                //PartsList.Add(p);
            }
        }


        public void Add(string instance, string path, string identity, int index, string name, string type, string value)
        {
            if (null == value)
            {
                // bail if sent a null value.
            }
            else
            {
                //var tinst = instance ?? string.Empty;
                //var tpath = path ?? string.Empty;
                var infoinst = new InfoInstance()
                {
                    Instance = instance ?? string.Empty,
                    Path = path ?? string.Empty
                };
                var p = new InfoPart()
                {
                    Identity = identity,
                    Index = index,
                    Name = name,
                    Type = (type.Equals("String")) ? $"${value.Length}" : type,
                    Value = value
                };

                if (Parts.TryGetValue(infoinst, out List<InfoPart> partsList))
                {
                    partsList.Add(p);
                }
                else
                {
                    partsList = new List<InfoPart>
                    {
                        p
                    };
                    Parts.Add(infoinst, partsList);

                }

                // might not need these, CimSave should be able to look at object for multiples or for non-null/non-empty
                //if (!InstanceUsed)
                //{
                //    if (!String.IsNullOrWhiteSpace(instance)) InstanceUsed = true;
                //}

                //if (!PathUsed)
                //{
                //    if (!String.IsNullOrWhiteSpace(path)) PathUsed = true;
                //}


            }
        }

        public void ToConsole()
        {
            Console.WriteLine("+--------");
            //foreach (var part in PartsList) { Console.WriteLine(part.ToString()); }
            Console.WriteLine(this.ToString());
            Console.WriteLine("---------");
        }

        public string ToJson()
        {
            string json;
            var currentCulture = Thread.CurrentThread.CurrentCulture;
            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(InfoParts));
                using (MemoryStream ms = new MemoryStream())
                {
                    js.WriteObject(ms, this);
                    ms.Position = 0;
                    using (StreamReader sr = new StreamReader(ms))
                    {
                        json = sr.ReadToEnd();
                        sr.Close();
                    }
                    ms.Close();
                }
            }
            catch (Exception ex)
            {
                json = ex.Message;
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = currentCulture;
            }
            //Console.WriteLine(json);
            return json;
        }

        //public void ToJsonFile() => File.WriteAllText($"{Server}_{Set}.Json", ToJson(), Encoding.UTF8);

        public void ToJsonFile(string SaveToFolder = "")
        {
            CIMSave.GZfileIO.WriteStringToGZ(Path.Combine(SaveToFolder, $"{Server}_{Set}.Json"), ToJson());
            CIMSave.GZfileIO.WriteStringToGZ(Path.Combine(SaveToFolder, $"{Server}_{Set}.Json.gz"), ToJson());
        }

        //public ProcessInfoRequest fromJson(string json)
        //{
        //    ProcessInfoRequest pInfo;
        //    try
        //    {
        //        using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
        //        {
        //            // Deserialization from JSON  
        //            DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(ProcessInfoRequest));
        //            pInfo = (ProcessInfoRequest)deserializer.ReadObject(ms);
        //            //pInfo.Good = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        pInfo = new ProcessInfoRequest()
        //        {
        //            Status = "Fail:" + ex.Message,
        //            Good = false
        //        };
        //    }
        //    return pInfo;
        //}


    }
}
