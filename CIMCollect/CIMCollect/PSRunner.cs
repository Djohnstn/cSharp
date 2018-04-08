using System;
//using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Collections.ObjectModel;
//using System.Runtime.Serialization;
//using System.Runtime.Serialization.Json;
//using System.IO;
//using System.Globalization;


namespace CIMCollect
{

    // https://www.codeproject.com/Articles/18229/How-to-run-PowerShell-scripts-from-C


    class PSRunner
    {


        // InfoParts PartsList;
        public string SaveToFolder { get; set; } = "";

        public PSRunner()
        {

        }



        public void Test()
        { /*
            var server = Environment.MachineName;
            // working - single and multips wmiisearchers
            {
                var result = RunScript(server, "BIOS", "Manufacturer", 
                    @"([wmisearcher]""Select * from win32_bios"").Get()");
                result.ToJsonFile(SaveToFolder);
                Console.WriteLine(result.ToJson());
                Utilities.Pause();
            }
            {
                var result = RunScript(server, "PhysicalMemory", "Name",
                    @"([wmisearcher]""Select * from Win32_PhysicalMemory"").Get()");
                result.ToJsonFile(SaveToFolder);
                Console.WriteLine(result.ToJson());
                Utilities.Pause();
            }
            {
                //var result = RunScript(@"Get-ChildItem Cert:\LocalMachine\My");
                var result = RunScript(server, "Certificate", "FriendlyName", 
                    @"Get-ChildItem Cert:\LocalMachine\My | Select FriendlyName , Thumbprint, Issuer, Subject, NotAfter, NotBefore, SerialNumber, Version, Handle");
                //, @{N='Name'; E={$_.FriendlyName}");
                // FriendlyName, Thumbprint, Issuer, Subject, NotAfter, NotBefore, SerialNumber, Version, Handle
                //Console.WriteLine(result);
                //result.ToConsole();
                result.ToJsonFile(SaveToFolder);
                Console.WriteLine(result.ToJson());
                Utilities.Pause();
            }
            {
                //var result = RunScript(@"Get-ChildItem Cert:\LocalMachine\My");
                var result = RunScript(server, "Service", "Name",
                    @"Get-WMIObject Win32_Service|Select Name, pathname, State,Caption,Description,ErrorControl,DelayedAutoStart,DisplayName,Started,StartMode,Start");
                //, @{N='Name'; E={$_.FriendlyName}");
                //Console.WriteLine(result);
                //result.ToConsole();
                result.ToJsonFile(SaveToFolder);
                Console.WriteLine(result.ToJson());
                Utilities.Pause();
            }
            */
        }

        public bool ToFile(string server, string dataset, string elementid, string script)
        {

            bool rc = false;
            int count = 0;
            try
            {
                var result = RunScript(server, dataset, elementid, script);
                result.ToJsonFile(SaveToFolder);
                count = result.PartsList.Count;
                rc = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                rc = false;
            }
            finally
            {
                var t = DateTime.Now.ToString("u");
                Console.WriteLine($"{t} Final {server} {dataset} result={rc} found={count}");
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
            return rc;
        }

        private string ConvertObjectToString(object obj)
        {
            return obj?.ToString() ?? string.Empty;
        }

        private string ConvertArrayToString<T>(PSProperty prop)
        {
            T[] aryv = (T[])prop.Value;
            string[] result = aryv.Select(x => x.ToString()).ToArray();
            return $"{{\"{string.Join("\",\"", result)}\"}}";
        }

        private void PSPropertyInfoOfTypeToString<T>(StringBuilder sb, PSObject obj)
            where T : PSPropertyInfo
        {
            foreach (T prop in obj.Properties.OfType<T>())
            {

                if (prop.IsGettable)
                {
                    var nam = prop.Name;        // __GENUS ; __CLASS; __SUPERCLASS _RELPATH
                                                //var pstyp = prop.MemberType; // pstype.property
                    var propValTyp = prop.TypeNameOfValue.Replace("System.", ""); // system.int32; system.string
                    string val = prop.Value.ToString();
                    if (!(propValTyp == val.Replace("System.", "")))
                    {
                        sb.AppendLine($"'name':'{nam}';'type':'{propValTyp}';'value':'{val.Trim()}'");
                    }

                }
                //Console.WriteLine(prop.ToString());
                //int ix = 0;
            }
        }


        private void PSPropertyInfoOfTypeToList<T>(InfoParts parts, string nameid, string name, int index, PSObject obj)
            where T : PSPropertyInfo
        {
            //bool indexNeedsUpdate = false;
            foreach (T prop in obj.Properties.OfType<T>())
            {

                if (prop.IsGettable)
                {
                    //indexNeedsUpdate = true;
                    var nam = prop.Name;        // __GENUS ; __CLASS; __SUPERCLASS _RELPATH
                                                //var pstyp = prop.MemberType; // pstype.property
                    if (nam == nameid) continue;  // skip this value, it is being handled differently
                    var propValTyp = prop.TypeNameOfValue.Replace("System.", ""); // system.int32; system.string
                    string val;

                    if (null == prop.Value) continue;
                    else if (propValTyp.Equals("IntPtr")) continue;                  // IntPtr!- remove
                    else if (propValTyp.Equals("DateTime"))       // round trip format
                    {
                        val = ((DateTime)prop.Value).ToString("o");
                    }
                    else
                    {
                        val = prop.Value?.ToString();
                    }
                    if (!(propValTyp == val.Replace("System.", "")))
                    {
                        //sb.AppendLine($"'name':'{nam}';'type':'{propValTyp}';'value':'{val.Trim()}'");
                        parts.Add(name, index, nam, propValTyp, val.Trim());
                    }

                }
                //Console.WriteLine(prop.ToString());
                //int ix = 0;
            }
            //if (indexNeedsUpdate) index++;
            //return index;
        }


        public InfoParts RunScript(string server, string dataset, string nameid, string scriptText)
        {
            if (string.IsNullOrWhiteSpace(server))
            {
                throw new ArgumentException("Null or Empty Server Name", nameof(server));
            }
            if (string.IsNullOrWhiteSpace(dataset))
            {
                throw new ArgumentException("Null or Empty Dataset Name", nameof(dataset));
            }
            if (string.IsNullOrWhiteSpace(nameid))
            {
                throw new ArgumentException("Null or Empty Name Tag", nameof(nameid));
            }


            Collection<PSObject> results = RunScript(scriptText);


            return HandleResults(server, dataset, nameid, results);
        }

        // may also check this: https://blogs.msdn.microsoft.com/kebab/2014/04/28/executing-powershell-scripts-from-c/

        public Collection<PSObject> RunScript(string scriptText)
        {
            Collection<PSObject> results;

            // create Powershell runspace
            using (Runspace runspace = RunspaceFactory.CreateRunspace())
            {
                runspace.Open();
                // create a pipeline and feed it the script text
                using (Pipeline pipeline = runspace.CreatePipeline())
                {
                    pipeline.Commands.AddScript(scriptText);
                    // extra command to format output as strings
                    // remove this line to get the actual objects, eg: System.Diagnostics.Process instances.
                    //pipeline.Commands.Add("Out-String");
                    results = pipeline.Invoke();
                }
                // close the runspace
                runspace.Close();
            }


            // https://stackoverflow.com/questions/22187464/execute-multiple-line-powershell-script-from-c-sharp
            //string script = @"
            //    $Username = 'User'
            //    $Password = 'Password'
            //    $SecurePass = ConvertTo-SecureString -AsPlainText $Password -Force
            //    $Cred = New-Object System.Management.Automation.PSCredential -ArgumentList     $Username,$SecurePass
            //    $contents = [IO.File]::ReadAllBytes('D:\MyFile.xml')
            //    Invoke-Command -ComputerName ComputerName -Credential $Cred {[IO.File]::WriteAllBytes('D:\\MyFile.xml',$using:contents)}
            //    ";

            return results;
        }

        private InfoParts HandleResults(string server, string dataset, string nameid, Collection<PSObject> results)
        {
            var parts = new InfoParts(server, dataset, DateTime.UtcNow);


            // convert the script result into a single string
            int index = 0;
            //StringBuilder stringBuilder = new StringBuilder();
            foreach (PSObject obj in results)
            {
                //stringBuilder.AppendLine(obj.ToString());
                var name = obj.Properties[nameid].Value.ToString();
                index++;
                //PSPropOfTypeToString<PSAdaptedProperty>(stringBuilder, obj); //PSProperty
                PSPropertyInfoOfTypeToList<PSAliasProperty>(parts, nameid, name, index, obj); //PSPropertyInfo
                PSPropertyInfoOfTypeToList<PSNoteProperty>(parts, nameid, name, index, obj); //PSPropertyInfo
                //PSPropertyInfoOfTypeToString<PSScriptProperty>(stringBuilder, obj); // PSPropertyInfo


                //var psProps = from Properties in obj where TypeName(Property) == "" select obj.Properties; 
                //var psProps = obj.Properties.OfType<PSProperty>();
                foreach (PSProperty prop in obj.Properties.OfType<PSProperty>())
                {
                    if (prop.IsGettable)
                    {
                        var nam = prop.Name;        // __GENUS ; __CLASS; __SUPERCLASS _RELPATH
                        if (nam == name) continue; // this detail can be skipped, it is handled specially
                        if (nam.StartsWith("__")) continue; // bail out on __ items.
                        //var pstyp = prop.MemberType; // pstype.property
                        // Property system.management.automation.psmembertypes.property
                        var propValTyp = prop.TypeNameOfValue.Replace("System.",""); // system.int32; system.string
                        var valtyp = prop.Value?.GetType(); // .name=int32; .fullname = system.int32 string[]
                        string val;
                        if (null == prop.Value) continue;
                        else if (nam.ToLower().Equals("RawData")) continue;
                        else if (propValTyp.StartsWith("Management.")) continue;
                        else if (valtyp.IsArray)
                        {
                            // https://www.codeproject.com/Articles/93260/Retrieving-Information-From-Windows-Management-Ins
                            // fails for value type types
                            if (valtyp.Name.ToLower().StartsWith("int16"))
                            {
                                val = ConvertArrayToString<Int16>(prop);
                            }
                            else if (valtyp.Name.ToLower().StartsWith("int32"))
                            {
                                val = ConvertArrayToString<Int32>(prop);
                            }
                            else if (valtyp.Name.ToLower().StartsWith("int64"))
                            {
                                val = ConvertArrayToString<Int64>(prop);
                            }
                            else if (valtyp.Name.ToLower().StartsWith("uint16"))
                            {
                                val = ConvertArrayToString<UInt16>(prop);
                            }
                            else if (valtyp.Name.ToLower().StartsWith("uint32"))
                            {
                                val = ConvertArrayToString<UInt32>(prop);
                            }
                            else if (valtyp.Name.ToLower().StartsWith("uint64"))
                            {
                                val = ConvertArrayToString<UInt64>(prop);
                            }
                            else if (valtyp.Name.ToLower().StartsWith("byte"))
                            {
                                val = "Btye[]"; //ConvertArrayToString<UInt64>(prop);
                            }
                            else if (valtyp.Name.StartsWith("DateTime"))
                            {
                                object[] ary = (object[])prop.Value;
                                string[] result = ary.Where(x => x != null)
                                                        .Select(x => ((DateTime)x).ToString("o")).ToArray();
                                val = $"{{\"{string.Join("\",\"", result)}\"}}";
                            }
                            else
                            {
                                object[] ary = (object[])prop.Value;
                                string[] result = ary.Where(x => x != null)
                                                        .Select(x => x.ToString()).ToArray();
                                val = $"{{\"{string.Join("\",\"", result)}\"}}";
                            }
                        }
                        else if (propValTyp.Equals("DateTime"))       // round trip format
                        {
                            val = ((DateTime)prop.Value).ToString("o");
                        }
                        else
                        {
                            val = prop.Value.ToString();   
                        }
                        if (!(propValTyp == val.Replace("System.", "")))
                        {
                            //stringBuilder.AppendLine($"'name':'{nam}';'type':'{propValTyp}';'value':'{val.Trim()}'");
                            parts.Add(name, index, nam, propValTyp, val.Trim());
                        }
                    }
                }

            }
            return parts;
        }
    }
}
