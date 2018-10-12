using System;
using System.Collections.Generic;
using System.Text;
using System.Security.AccessControl;
using System.IO;
using System.Threading;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Globalization;
using System.Collections.Concurrent;
using CIMSave;
using CIMCollect.SqlClasses;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
//using System.Reflection;

namespace DirectorySecurityList
{
    public class RuntimeInfo
    {
        public static string MachineDomain { get; } = Environment.MachineName + Path.DirectorySeparatorChar;
        public static string MachineDomainAlias { get; } = "(localhost)" + Path.DirectorySeparatorChar;
    }

    [DataContract]
    public class ACE : IEquatable<ACE>
    {
        [DataMember]
        public string PrincipalName { get; private set; } = "";
        [DataMember]
        public string PrincipalSID { get; private set; } = "";
        [DataMember]
        public string AccessType { get; private set; } = "";
        [DataMember]
        public FileSystemRights Rights { get; private set; }
        [DataMember]
        public int AceID { get; private set; }

        private int hash;
        private static int _id = 0;

        // cache Name to ID translations
        private static ConcurrentDictionary<string, string> IdSidDictionary = new ConcurrentDictionary<string, string>();
        private string GetSidFromName(System.Security.Principal.IdentityReference principalID)
        {
            var principalName = principalID.ToString();
            if (IdSidDictionary.TryGetValue(principalName, out string idSid))
            {

            }
            else
            {
                // there be bugs in Windows principal Translate to SID here. JDJ 04/02/2018
                //if (principalName.Equals("APPLICATION PACKAGE AUTHORITY\\ALL APPLICATION PACKAGES"))
                //{
                //    principalName = "ALL APPLICATION PACKAGES";
                //    idSid = string.Empty;
                //}
                if (principalName.StartsWith("APPLICATION PACKAGE AUTHORITY"))
                {
                    idSid = string.Empty;
                }
                else
                {
                    try
                    {
                        idSid = principalID.Translate(typeof(System.Security.Principal.SecurityIdentifier)).Value;
                    }
                    catch (Exception ex)
                    {
                        idSid = string.Empty;
                        Console.WriteLine($"@ Unable to Translate({principalName}): {ex.Message}");
                    }
                }
                IdSidDictionary.TryAdd(principalName, idSid);
            }
            return idSid;
        }

        public ACE(FileSystemAccessRule fileSystemAccessRule)
        {
            var idRef = fileSystemAccessRule.IdentityReference;
            var idRefString = idRef.ToString();
            var idSid = GetSidFromName(idRef);
            _ACE(idRefString, idSid,
                 fileSystemAccessRule.AccessControlType.ToString(),
                 fileSystemAccessRule.FileSystemRights);
        }
        public ACE(string principal, string principalSid, string accessType, FileSystemRights fsr)
        {
            _ACE(principal, principalSid, accessType, fsr);
        }
        private void _ACE(string principal, string principalSid, string accessType, FileSystemRights fsr)
        {
            if (principal.StartsWith(RuntimeInfo.MachineDomain))
            {
                this.PrincipalName = principal.Replace(RuntimeInfo.MachineDomain, RuntimeInfo.MachineDomainAlias);
            }
            else
            {
                this.PrincipalName = principal;
            }
            this.PrincipalSID = principalSid;
            this.AccessType = accessType;
            this.Rights = fsr;
            this.AceID = ++_id;
            unchecked
            {
                this.hash = (int)((uint)this.PrincipalName.GetHashCode() ^ (uint)this.AccessType.GetHashCode() ^ (uint)this.Rights);
            }
        }
        public void ResetID(int id)
        {
            _id = id; // + 1;
        }

        public override string ToString()
        {
            return PrincipalName + ":" + AccessType + "(" + Rights.ToString() + ")";
        }

        public override bool Equals(object obj)
        {
            // Again just optimization
            if (obj is null) return false;
            if (this is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            // Actually check the type, should not throw exception from Equals override
            if (obj.GetType() != this.GetType()) return false;
            ACE other = (ACE)obj;
            if (other.GetHashCode() != this.GetHashCode()) return false;
            if (this.ToString().Equals(other.ToString())) return true;

            return false;
        }
        public override int GetHashCode()
        {
            return hash;
        }
        public bool Equals(ACE h2)
        {
            if (h2 is null) return false;
            if (this is null) return false;
            if (ReferenceEquals(this, h2)) return true;
            if (h2.GetType() != this.GetType()) return false;
            if (this.GetHashCode() != h2.GetHashCode()) return false;
            if (this.ToString().Equals(h2.ToString())) return true;
            return false;
        }

        internal int CompareTo(ACE h2)
        {
            var c1 = this.PrincipalName.CompareTo(h2.PrincipalName);
            if (c1 != 0) return c1;
            var c2 = this.AccessType.CompareTo(h2.AccessType);
            if (c2 != 0) return c2;
            var c3 = ((uint)this.Rights).CompareTo((uint)h2.Rights);
            return c3;
        }

        //public string ToJSON()
        //{
        //    string json;
        //    var currentCulture = Thread.CurrentThread.CurrentCulture;
        //    try
        //    {
        //        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        //        DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(ACE));
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            js.WriteObject(ms, this);
        //            ms.Position = 0;
        //            using (StreamReader sr = new StreamReader(ms))
        //            {
        //                json = sr.ReadToEnd();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        json = ex.Message;
        //    }
        //    finally
        //    {
        //        Thread.CurrentThread.CurrentCulture = currentCulture;
        //    }
        //    //Console.WriteLine(json);
        //    return json;
        //}

    }
    public class ACEComparer : IComparer<ACE>
    {
        public int Compare(ACE x, ACE y)
        {
            // TODO: Handle x or y being null, or them not having names
            if (x is null && y is null) return 0;
            if (x is null) return 1;
            if (y is null) return -1;
            return x.CompareTo(y);
        }
    }

    [DataContract]
    class ACESet
    {
        ReaderWriterLockSlim locker = new ReaderWriterLockSlim();
        [DataMember]
        public SortedList<ACE, int> aceList = new SortedList<ACE, int>(new ACEComparer());

        [DataMember]
        public ConcurrentDictionary<int, ACE> idList = new ConcurrentDictionary<int, ACE>();
        private int maxID = -1;

        public int GetId(ACE ace)
        {
            locker.EnterUpgradeableReadLock();
            try
            {
                return _UnsafeGetId(ace);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                locker.ExitUpgradeableReadLock();
            }
        }


        private int _UnsafeGetId(ACE ace)
        {
            if (ace is null)
            {
                return 0;
            }

            if (aceList.TryGetValue(ace, out int id))
            {
                ace.ResetID(maxID);
                return id;
            }
            else
            {
                locker.EnterWriteLock();
                try
                {
                    return _UnsafeAddId(ace);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    locker.ExitWriteLock();
                }
            }
        }

        private int _UnsafeAddId(ACE ace)
        {
                var aceid = ace.AceID;
                aceList.Add(ace, aceid);
                if (idList.TryAdd(aceid, ace))
                {

                }
                else
                {
                    throw new ArgumentException($"@ ACESet.GetID Cannot add Duplicate ACE {ace}");
                }
                if (aceid > maxID) maxID = aceid;
                return ace.AceID;
        }

        public ACE GetACE(int aceID)
        {
            locker.EnterReadLock();
            try
            {
                return _unsafeGetACE(aceID);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                locker.ExitReadLock();
            }
        }

        private ACE _unsafeGetACE(int aceID)
        {
            if (idList.TryGetValue(aceID, out ACE aCE))
            {
                return aCE;
            }
            else
            {
                return new ACE(String.Empty, String.Empty, String.Empty, (FileSystemRights)0);
            }
        }

        public string GetAllACEStrings()
        {
            locker.EnterReadLock();
            try
            {
                return _unsafeGetAllACEStrings();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                locker.ExitReadLock();
            }
        }


        private string _unsafeGetAllACEStrings()
        {
            var sb = new StringBuilder();
            foreach (var ace in aceList)
            {
                sb.AppendFormat("{0}; ", ace.Key.ToString());
            }
            return sb.ToString();
        }

        public string ToJSON()
        {
            locker.EnterReadLock();
            try
            {
                return _unsafeToJSON();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                locker.ExitReadLock();
            }
        }


        private string _unsafeToJSON()
        {
            string json;
            var currentCulture = Thread.CurrentThread.CurrentCulture;
            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(ACESet));
                using (MemoryStream ms = new MemoryStream())
                {
                    js.WriteObject(ms, this);
                    ms.Position = 0;
                    using (StreamReader sr = new StreamReader(ms))
                    {
                        json = sr.ReadToEnd();
                        //sr.Close();
                    }
                    //ms.Close();
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

        public static int FromJSON()
        {
            int count = 0;


            return count;
        }

    }

    [DataContract]
    public class ACL : IEquatable<ACL>
    {
        //[DataMember]
        //public List<ACE> acl = null;
        [DataMember]
        public SortedList<int, ACE> list = new SortedList<int, ACE>();
        private int hash = 0;
        //private ReaderWriterLockSlim locker = new ReaderWriterLockSlim();

        public void Add(ACE ace, int id)
        {
            if (hash != 0) throw new Exception("ACL.Add(,) not allowed after Seal().");
            if (list.TryGetValue(id, out ACE value))
            {
                if (!value.Equals(ace)) throw new Exception("ACL.Add(,) AceID and ACE inconsistent.");
            }
            else
            {
                list.Add(id, ace);
            }
        }
        public void Seal()
        {
            //acl = new List<ACE>();
            //foreach (ACE j in list.Values) { acl.Add(j); }
            hash = this.ToString().GetHashCode();
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var k in list.Keys)
            {
                sb.AppendFormat("{0}.", k.ToString());
            }
            sb.Length--;    // remove last "."
            return sb.ToString();
        }
        public string ACLString()
        {
            var sb = new StringBuilder();
            foreach (var ace in list.Values)
            {
                sb.AppendLine(ace.ToString());
            }
            return sb.ToString();
        }
        public override bool Equals(object obj)
        {
            // Again just optimization
            if (obj is null) return false;
            if (this is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            // Actually check the type, should not throw exception from Equals override
            if (obj.GetType() != this.GetType()) return false;
            ACL other = (ACL)obj;
            if (other.GetHashCode() != this.GetHashCode()) return false;
            if (this.ToString().Equals(other.ToString())) return true;

            return false;
        }
        public override int GetHashCode()
        {
            return hash;
        }

        public bool Equals(ACL h2)
        {
            if (h2 is null) return false;
            if (this is null) return false;
            if (ReferenceEquals(this, h2)) return true;
            if (h2.GetType() != this.GetType()) return false;
            if (h2.GetHashCode() != this.GetHashCode()) return false;
            if (this.ToString().Equals(h2.ToString())) return true;
            return false;
        }

        internal int CompareTo(ACL h2)
        {
            var c1 = this.ToString().CompareTo(h2.ToString());
            return c1;
        }

        //public string ToJSON()
        //{
        //    string json;
        //    var currentCulture = Thread.CurrentThread.CurrentCulture;
        //    try
        //    {
        //        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        //        acl = new List<ACE>();
        //        foreach (ACE j in list.Values) { acl.Add(j); }

        //        DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(ACL));
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            js.WriteObject(ms, this);
        //            ms.Position = 0;
        //            using (StreamReader sr = new StreamReader(ms))
        //            {
        //                json = sr.ReadToEnd();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        json = ex.Message;
        //    }
        //    finally
        //    {
        //        acl = null;
        //        Thread.CurrentThread.CurrentCulture = currentCulture;
        //    }
        //    //Console.WriteLine(json);
        //    return json;
        //}

        //public string ToJSON()
        //{
        //    locker.EnterReadLock();
        //    try
        //    {
        //        return _UnsafeToJSON();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        locker.ExitReadLock();
        //    }
        //}

        //private string _UnsafeToJSON()
        //{
        //    System.Diagnostics.Debug.Assert(locker.IsReadLockHeld || locker.IsUpgradeableReadLockHeld);
        //    string json;
        //    var currentCulture = Thread.CurrentThread.CurrentCulture;
        //    try
        //    {
        //        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        //        DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(ACLSet));
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            js.WriteObject(ms, this);
        //            ms.Position = 0;
        //            using (StreamReader sr = new StreamReader(ms))
        //            {
        //                json = sr.ReadToEnd();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        json = ex.Message;
        //    }
        //    finally
        //    {
        //        Thread.CurrentThread.CurrentCulture = currentCulture;
        //    }
        //    return json;
        //}

        //public static ACLSet FromJSON(string jsonfilename)
        //{
        //    return GZfileIO.ReadGZtoJson<ACLSet>(jsonfilename);
        //}

        }
        public class ACLComparer : IComparer<ACL>
    {
        public int Compare(ACL x, ACL y)
        {
            // TODO: Handle x or y being null, or them not having names
            if (x is null && y is null) return 0;
            if (x is null) return 1;
            if (y is null) return -1;
            return x.CompareTo(y);
        }
    }

    [DataContract]
    class ACLSet
    {
        ReaderWriterLockSlim locker = new ReaderWriterLockSlim();
        private SortedDictionary<ACL, int> aclList = new SortedDictionary<ACL, int>(new ACLComparer());
        //public SortedDictionary<ACL, int> List = SortedDictionary. //..(_unsyncdList);
        
        [DataMember]
        private ConcurrentDictionary<int, ACL> idList = new ConcurrentDictionary<int, ACL>();
        private int id = 0;

        public int GetId(ACL acl)
        {
            locker.EnterUpgradeableReadLock();
            try
            {
                return _UnsafeGetId(acl);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                locker.ExitUpgradeableReadLock();
            }
        }

        private int _UnsafeGetId(ACL acl)
        {
            if (aclList.TryGetValue(acl, out int value))
            {
                return value;
            }
            else
            {
                locker.EnterWriteLock();
                try
                {
                    _UnsafeAddId(acl);
                    return id;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    locker.ExitWriteLock();
                }
            }
        }
        private int _UnsafeAddId(ACL acl)
        {
            System.Diagnostics.Debug.Assert(locker.IsReadLockHeld || locker.IsUpgradeableReadLockHeld);
            id++;
            aclList.Add(acl, id);
            if (idList.TryAdd(id, acl))
            {

            }
            else
            {
                throw new ArgumentException($"@ ACLSet.GetID Cannot add Duplicate ACE {acl}");
            }
            return id;
        }

        public ACL GetACL(int aclID)
        {
            locker.EnterReadLock();
            try
            {
                return _UnsafeGetACL(aclID);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                locker.ExitReadLock();
            }
        }

        private ACL _UnsafeGetACL(int aclID)
        {
            if (idList.TryGetValue(aclID, out ACL aCL))
            {
                return aCL;
            }
            else
            {   // returns empty ACL!?
                var acl = new ACL();
                acl.Seal();
                return acl;
            }
        }

        public string ToJSON()
        {
            locker.EnterReadLock();
            try
            {
                return _UnsafeToJSON();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                locker.ExitReadLock();
            }
        }

        private string _UnsafeToJSON()
        {
            System.Diagnostics.Debug.Assert(locker.IsReadLockHeld || locker.IsUpgradeableReadLockHeld);
            string json;
            var currentCulture = Thread.CurrentThread.CurrentCulture;
            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(ACLSet));
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
                json = ex.Message;
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = currentCulture;
            }
            return json;
        }

        public static ACLSet FromJSON(string jsonfilename)
        {
            return GZfileIO.ReadGZtoPOCO<ACLSet>(jsonfilename);
            //return GZfileIO.ReadGZtoJson<ACLSet>(jsonfilename);
            //var json = GZfileIO.ReadGZtoString(jsonfilename);
            
            ////var deserializer = new JavaScriptSerializer

            //var deserializer = new DataContractJsonSerializer(typeof(ACLSet));
            //return (ACLSet)deserializer.ReadObject(json);

            //using (var sr = new FileStream(jsonfilename, FileMode.Open, FileAccess.Read))
            //{
            //    var deserializer = new DataContractJsonSerializer(typeof(ACLSet));
            //    return (ACLSet)deserializer.ReadObject(sr);
            //}
        }

#pragma warning disable IDE0044 // Add readonly modifier
        private Dictionary<int, int> JsonACLIDtoDBID = new Dictionary<int, int>(30);
#pragma warning restore IDE0044 // Add readonly modifier
                               //         public int ToDB(string connectionString)

        public int ToDB()
        {
            TvpSetup();
            int records = 0;
            if (JsonACLIDtoDBID ==null)
            {
                JsonACLIDtoDBID = new Dictionary<int, int>(300);
            }
            foreach(var idAcl in idList)
            {
                var idNumber = idAcl.Key;
                int aCLdbId = this.ACLdbId(idAcl.Value);
                JsonACLIDtoDBID.Add(idNumber, aCLdbId); // save result for binding from FileList to proper database id
                records++;
                //throw new NotImplementedException(ix.ToString());
            }

            return records;
        }

        private static bool tvpSetupDone = false;
        const string tvpTypeInt = "dbo.TVP_INT";
        private void TvpSetup()
        {
            if (tvpSetupDone) return;
            tvpSetupDone = true;
            const string makeTVP = "CREATE TYPE dbo.TVP_INT AS TABLE(id INT); ";
            var db = new DBTableMaker();
            //if (!db.IfExists(tvpTypeInt))
            if (!db.IfTypeExists(tvpTypeInt))
            {
                var handlerBase = new SQLHandlerBase();
                handlerBase.DoQueryNonScaler(makeTVP);
            }

        }

        private int ACLdbId(ACL aCL)
        {
            var aceList = new List<int>();
            var aceListHasher = new BinaryIntegerList();
            foreach (var acl in aCL.list)
            {
                //var id = x.Key;
                // may not need to do  anything with the "id"? I'm resolving ACE(Principal and access) to database ACL
                int aceid = this.ACEdbId(acl.Value);
                aceList.Add(aceid);
                aceListHasher.Add(aceid);
            }
            var aclHash = aceListHasher.ToRLE();
            Debug.WriteLine(aceListHasher.ToString());
            //var aceXSum = IntListCheckSum(aceList);

            // aceXSum is "mostly" unique, should give list of 1 ACLs per ACEXSum match
            // do ACLID query,
            var aclid = AclID(aceList, aclHash);
            // if no match
                // create an ACL(1)-ACE(*) list
                // create an ACL(1)(aceid) list
            // do ACLID query again
            // if not found, try one more time, then throw up.
            //Console.WriteLine("ACL " + Convert.ToBase64String(aceXSum));
            return aclid;
        }

        const string monsterACLQuery = @"dbo.SelectOrInsert_ACL";

        const string monsterACLInsert1 = @" ";  // insert into acl table (acl, acel-list-hash)
        const string monsterACLInsert2 = @" ";  // insert into acl-ace table

        private int AclID(List<int> aceList, byte[] aclHash)
        {
            var sqlbase = new SQLHandlerBase();
            var connectStr = sqlbase.ConnectionString;
            int ID = -1;
            bool aclExists = false;
            using (var connection = new SqlConnection(connectStr))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(monsterACLQuery, connection))
                {
                    //IEnumerable<int> aceEnumerable = aceList.ToArray().AsEnumerable();
                    //IEnumerable<int> aceEnumerable = GetEnumerable<int>(aceList).c.ToList<int>();
                    var aceTable = aceList.ToDataTable("tvp");
                    var pList = new SqlParameter("@tvp", SqlDbType.Structured)
                    {
                        TypeName = tvpTypeInt,
                        Value = aceTable
                    };
                    var pHash = new SqlParameter("@hash", SqlDbType.VarBinary, aclHash.Length)
                    {
                        Value = aclHash
                    };
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(pList);
                    cmd.Parameters.Add(pHash);

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            ID = dr.GetInt32(0);
                            aclExists = true;
                        }
                    }
                }
                if (!aclExists)
                {
                    using (SqlCommand cmd = new SqlCommand(monsterACLInsert1, connection))
                    {
                        var pList = new SqlParameter("@tvp", SqlDbType.Structured)
                        {
                            TypeName = tvpTypeInt,
                            Value = aceList
                        };
                        var pHash = new SqlParameter("@hash", SqlDbType.VarBinary, aclHash.Length)
                        {
                            Value = aclHash
                        };

                        cmd.Parameters.Add(pList);
                        cmd.Parameters.Add(pHash);
                        using (var dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                ID = dr.GetInt32(0);
                                aclExists = true;
                            }
                        }

                    }

                }
                return ID;
            }
        }

        //public IEnumerable<T> GetEnumerable<T>(List<T> x)
        //{
        //    IEnumerable<T> result = x;
        //    foreach(var item in x)
        //    {
        //        result.
        //    }
        //    return result;
        //}

        //private byte[] IntListCheckSum(List<int> intList)
        //{
        //    int[] intArray = intList.ToArray();
        //    var checksum = new byte[64];
        //    byte[] result = new byte[intArray.Length * sizeof(int)];
        //    Buffer.BlockCopy(intArray, 0, result, 0, result.Length);
        //    using (var sha = new SHA512Managed())
        //    {
        //        checksum = sha.ComputeHash(result);
        //    }
        //    return checksum;
        //}

        private int ACEdbId(ACE value)
        {
            var pName = value.PrincipalName;
            var pSID = value.PrincipalSID;
            var rights = value.Rights;

            var pNameID = PrincipalID(pName, pSID);
            var rightsID = RightsID(rights);

            var aceID = AceDBID(pNameID, rightsID);
            return aceID;
        }

        static bool AceDbTableValidated = false;

        private static DBMapper2<int, int, int> mapAceDb =
            new DBMapper2<int, int, int>(tablename: "ACE_Principal_Rights", defaultvalue: -1,
                                            nameparameter0: "PrincipalId", nameparameter: "RightsId");
        private int AceDBID(int pNameID, int rightsID)
        {
            if (!AceDbTableValidated)
            {
                DBTableMaker checker = new DBTableMaker("ACE_Principal_Rights");
                checker.ForColumn("PrincipalId", "INT", nullable: false, indexed: true);
                checker.ForColumn("RightsId", "INT", nullable: false, indexed: true);
                RightsTableValidated = checker.ValidateTable();
            }
            int id = mapAceDb.ID(pNameID, rightsID);
            return id;
        }

        static bool RightsTableValidated = false;

        private static DBMapper2<int, string, int> mapRights =
            new DBMapper2<int, string, int>(tablename: "ACE_Rights", defaultvalue: -1, 
                                            nameparameter0:  "Rights", nameparameter: "Description");
        private int RightsID(FileSystemRights rights)
        {
            if (!RightsTableValidated)
            {
                DBTableMaker checker = new DBTableMaker("ACE_Rights");
                checker.ForColumn("Rights", "INT", true, true);
                checker.ForColumn("Description", "NVARCHAR(120)", true, false);
                RightsTableValidated = checker.ValidateTable();
            }
            var description = rights.ToString();
            int rightsInt = (int)rights;
            int id = mapRights.ID(rightsInt, description);
            return id;
            //            throw new NotImplementedException();
        }

#pragma warning disable IDE0044 // Add readonly modifier
        static bool PrincipalTableValidated = false;
#pragma warning restore IDE0044 // Add readonly modifier

        private static DBMapper2<string, string, int> mapPrincipal = 
            new DBMapper2<string, string, int>("ACE_Principal", -1, "PrincipalName", "SID");
        private int PrincipalID(string pName, string pSID)
        {
            if (!PrincipalTableValidated)
            {
                var checker = new DBTableMaker("ACE_Principal");
                checker.ForColumn("PrincipalName", "NVARCHAR(100)", true, true);
                checker.ForColumn("SID", "NVARCHAR(120)", true, true);
                RightsTableValidated = checker.ValidateTable();
            }
            int id = mapPrincipal.ID(pName,pSID);
            return id;
        }

    }

    public class Instances
    {
        private static DBMapper<string, int> map = new DBMapper<string, int>("_Instances", -1, "Name");
        public static int ID(string name) => map.ID(name);
        //public int ID2(string name) => (new DBMapper<string, int>("_Instances", -1, "Identity")).ID(name);
    }

}
