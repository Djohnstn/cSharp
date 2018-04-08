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
    public class InfoParts
    {

        [DataMember]
        public string Server { get; set; }      // computer name -> column "Server" / "ServerId"
        [DataMember]
        public string Set { get; set; }         // eg memory    -> table name
        [DataMember]
        public DateTime AsOf { get; set; }         // eg data collection time 

        [DataMember]
        public List<InfoPart> PartsList;

        public string Result { get; set; } = "";

        public InfoParts(string server, string set)
        {
            Server = server;
            Set = set;
            PartsList = new List<InfoPart>();
        }

        public InfoParts(string server, string set, DateTime dateTime)
        {
            Server = server;
            Set = set;
            AsOf = dateTime;
            PartsList = new List<InfoPart>();
        }

        public InfoParts()
        {
            Server = "";
            Set = "";
            PartsList = new List<InfoPart>();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var p in PartsList)
            {
                sb.AppendLine($"[{Set}]://{Server}/{p.Identity}({p.Index})/{p.Name}({p.Type})={p.Value}");
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
                var p = new InfoPart()
                {
                    Identity = identity,
                    Index = index,
                    Name = name,
                    Type = (type.Equals("String")) ? $"${value.Length}" : type,
                    Value = value
                };

                PartsList.Add(p);
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
