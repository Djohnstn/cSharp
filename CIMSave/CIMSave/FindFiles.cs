using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using CIMCollect;
using System.Data;
//using System.Data.DataSetExtensions;

namespace CIMSave
{
    // https://stackoverflow.com/questions/7574606/left-function-in-c-sharp
    public static class StringExtensions
    {
        public static string Left(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            maxLength = Math.Abs(maxLength);

            return (value.Length <= maxLength
                   ? value
                   : value.Substring(0, maxLength)
                   );
        }
    }

    public static class ByteExtensions
    {
        public static string Left(this byte[] value, int maxLength)
        {
            if (value.Length == 0) return String.Empty;
            maxLength = Math.Abs(maxLength);
            var result = BitConverter.ToString(value).Replace("-", "");
            return (result.Length <= maxLength
                   ? result
                   : result.Substring(0, maxLength)
                   );
        }
    }


    class FindFiles
    {
        public string SqlConnectionString { get; set; }
        public string TablePrefix { get; set; }

        public int Breakspot { get; set; } = 0;

        public void AllJson(string filePath, string fileMask, string databaseSchema, string tablePrefix)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("message", nameof(filePath));
            }

            if (string.IsNullOrWhiteSpace(fileMask))
            {
                throw new ArgumentException("message", nameof(fileMask));
            }

            if (string.IsNullOrWhiteSpace(databaseSchema))
            {
                throw new ArgumentException("message", nameof(databaseSchema));
            }

            if (string.IsNullOrWhiteSpace(tablePrefix))
            {
                throw new ArgumentException("message", nameof(tablePrefix));
            }

            int files = 0;
            var server = Environment.MachineName;
            TablePrefix = tablePrefix;
            var sw = new Stopwatch();
            Console.WriteLine($"{LogTime()} Processing {filePath}\\{fileMask} files.");
            sw.Start();
            foreach (var filename in System.IO.Directory.EnumerateFiles(filePath, fileMask))
            {
                files++;
                HandleFile(filename, databaseSchema);
            }
            sw.Stop();
            Console.WriteLine($"{LogTime()} Processed {files} dataset files in {sw.ElapsedMilliseconds}ms.");
            Pause();
        }

        private void Pause()
        {
            Console.Write("Press enter to exit");
            Console.ReadLine();
        }

        private string LogTime() => DateTime.Now.ToString("u");

        private void HandleFile(string filename, string schema)
        {

            Console.WriteLine($"{LogTime()} Processing file {filename}.");

            InfoParts p = InfoParts.FromJsonFile(filename);
            //p.SqlConnectionString = this.SqlConnectionString;
            PartsToDataSet(p, schema);
            //Pause();
        }

        class TableColumn
        {
            public string ColName { get; set; }
            public string ColType { get; set; }
            public int ColLength { get; set; }
        }


        private void PartsToDataSet(InfoParts fullPartsList, string schema = "dbo")
        {

            var tableColumns = PartsSizes(fullPartsList);
            var sQLHandler = new SQLHandler(SqlConnectionString);
            var server = fullPartsList.Server;
            var serverId = sQLHandler.ServerID(server);
            //var schema = "dbo";
            var dtName = TablePrefix + fullPartsList.Set;
            var fileDt = AddColumnsToDataTable(dtName, tableColumns);
            var tablebOK = PrepareOrUpdateTable(sQLHandler, schema, dtName, tableColumns);
            Breakspot = 0;

            DataTable sqlDT = PrepareSQLDT(fileDt);

            var sqlDTLoaded = FillSqlDt(sQLHandler, sqlDT, schema, dtName, server);

            var dtLoaded = FillDataTable(fileDt, fullPartsList, serverId);

            var sqUpdateNeeded = CompareSQLDTtoFileDT(sqlDT, fileDt);

            var result2 = sQLHandler.UpdateDA(sqlDT, schema, dtName, server);

        }

        private bool CompareSQLDTtoFileDT(DataTable sQLDt, DataTable fileDt)
        {

            // checksum columns for each row?
            HashTable sqlHash = HashDT(sQLDt);
            HashTable fileHash = HashDT(fileDt);
            sQLDt.PrimaryKey = new DataColumn[] { sQLDt.Columns["id"]};
            fileDt.PrimaryKey = new DataColumn[] { fileDt.Columns["id"]};

            this.Breakspot = 0;

            // dump the lists
            //SQLHandler.DTtoConsole(sQLDt, sqlHash, "sQLDt");
            //SQLHandler.DTtoConsole(fileDt, fileHash, "fileDt");

            // delete all sqlDT not in fileDT
            DeleteDTNotInDT(sQLDt, sqlHash, fileDt, fileHash);

            // delete all fileDT that are already in sqlDT
            DeleteDTinDT(fileDt, fileHash, sQLDt, sqlHash);

            // add all fileDT that are left
            sQLDt.Merge(fileDt, true);

            return true;
        }

        private bool DeleteDTinDT(DataTable dtWithExtra, HashTable hashExtra,
            DataTable dtWithBase, HashTable hashBase)
        {
            bool status = true;
            var basename = dtWithBase.TableName;
            var tempDt = dtWithBase.Select(null, null, DataViewRowState.CurrentRows);
            foreach (var xSum in hashBase.Hashs())
            {
                if (hashExtra.TryGetID(xSum, out int idExtra))
                {
                    (from myRow in dtWithExtra.AsEnumerable()
                               where myRow.Field<int>("id") == idExtra // &&
                               select myRow)
                                .FirstOrDefault<DataRow>()?.Delete();
                }
            }
            return status;
        }

        private bool DeleteDTNotInDT(DataTable dtWithExtra, HashTable hashExtra,
                                     DataTable dtWithBase, HashTable hashBase)
        {
            var basename = dtWithBase.TableName;
            bool status = true;
            var toKeep = new SortedSet<int>();

            foreach (var xSum in hashBase.Hashs())
            {
                if (hashExtra.TryGetID(xSum, out int idExtra))
                {
                    toKeep.Add(idExtra);
                }
            }

            foreach (DataRow xdr in dtWithExtra.Rows)
            {
                var id = xdr.Field<int>("id");
                if (!toKeep.Contains(id))
                {   // get rid of unknow record ids
                    var xname = xdr.Field<string>("Name");
                    var xidStr = String.Format("{0,4:####}", id);
                    var xsumStr = hashExtra.HashValue(id).ToString().Left(8); // hashValue(hashExtra, id); //sdr.Field<string>(checkHashColumn);
                    //var xsum = xdr.Field<string>(checkHashColumn);
                    Console.WriteLine($"Deleting row {xidStr}: {xname} hash {xsumStr} because it is not in file {basename}.json.");
                    xdr.Delete();
                }
            }
            return status;
        }


        HashTable HashDT(DataTable dt)
        {   
            var hashList = new HashTable() ; //Dictionary<int, byte[]>(); // HashList<int, string>
            var column = dt.Columns;
            foreach (DataRow row in dt.Rows)
            {
                hashList.Add(row);
            }
            return hashList;
        }

        private bool FillSqlDt(SQLHandler sQLHandler, DataTable sqlDT, 
            string schema, string tableName, string server)
        {
            var ok = sQLHandler.FillDT(sqlDT, schema, tableName, server);
            return ok;
        }

        private bool FillDataTable(DataTable fileDt, InfoParts fullPartsList, int serverId)
        {
            int lastPart = -1;
            DataRow myRow = null;
            foreach (InfoPart part in fullPartsList.PartsList)
            {
                var partID = part.Identity;     // Name column
                var partIndex = part.Index;     // part - different numbers in case of collission of name
                var partName = part.Name;       // column name
                var partType = part.Type;       // column type
                var partValue = part.Value;     // column value
                if (partIndex == lastPart)
                {
                    // not new row
                }
                else
                {
                    // save previous row
                    if (myRow != null)
                    {
                        myRow.EndEdit();
                        fileDt.Rows.Add(myRow);
                    }
                    // new row
                    myRow = fileDt.NewRow();
                    myRow.BeginEdit();
                    myRow["id"] = partIndex;    // hope this is it...
                    myRow["ServerId"] = serverId;
                    myRow["Name"] = partID;
                    lastPart = partIndex;
                };
                ??? can we fix it path and instance;
                var rowCol = myRow[partName];
                var type = rowCol?.GetType();
                object foo;
                if (partType.StartsWith("UInt"))
                {   // may have to fix this to Int
                    foo = GetStringIntforUInt(partType, partValue);
                }
                else
                {
                    foo = partValue.Left(SQLHandler.MaxStringLength);
                }
                myRow[partName] = foo;

            }
            // save previous row
            if (myRow != null)
            {
                myRow.EndEdit();
                fileDt.Rows.Add(myRow);
            }
            return true;
        }

        private string GetStringIntforUInt(string partType, string partValue)
        {
            string newValue = String.Empty;
            if (String.IsNullOrWhiteSpace(partValue))
            {
                // leave it alone
            }
            else if (partValue.Length < 5)
            {
                // shortcut if value is small 0 - 99
            }
            else if (partType.EndsWith("[]"))
            {
                // string, skip it
            }
            else if (partType.EndsWith("16"))
            {
                if (partValue.Length > 4)   // 1-60,000
                {
                    bool success = UInt16.TryParse(partValue, out UInt16 aNumber);
                    if (success)
                    {
                        byte[] bytes = BitConverter.GetBytes(aNumber);
                        Int16 x = BitConverter.ToInt16(bytes, 0);
                        newValue = x.ToString();
                    }
                }
            }
            else if (partType.EndsWith("32"))
            {
                if (partValue.Length > 9) // 1-4,000,000,000
                {
                    bool success = UInt32.TryParse(partValue, out UInt32 aNumber);
                    if (success)
                    {
                        byte[] bytes = BitConverter.GetBytes(aNumber);
                        Int32 x = BitConverter.ToInt32(bytes, 0);
                        newValue = x.ToString();
                    }
                }
            }
            else if (partType.EndsWith("64"))
            {
                if (partValue.Length > 18) // 1-9,223,372,036,854,775,808
                {
                    bool success = UInt64.TryParse(partValue, out UInt64 aNumber);
                    if (success)
                    {
                        byte[] bytes = BitConverter.GetBytes(aNumber);
                        Int64 x = BitConverter.ToInt64(bytes, 0);
                        newValue = x.ToString();
                    }
                }
            }

            return (newValue.Length > 0)? newValue : partValue;
        }

        private T Caster<T>(object foo)
        {
            T result = (T)foo;
            return result;
        }

        private DataTable PrepareSQLDT(DataTable fileDt)
        {
            var sQLDt = fileDt.Clone();
            var idFound = sQLDt.Columns.Contains("id");
            if (!idFound)   // add missing row/record ID column? 
            {
                var idCol = sQLDt.Columns.Add("id", typeof(int));
                idCol.SetOrdinal(0);    // move column to front of datatable
            }
            return sQLDt;
        }

        private bool PrepareOrUpdateTable(SQLHandler sQLHandler, 
                                    string schema, 
                                    string tableName,
                                    SortedDictionary<string, TableColumn> tableColumns)
        {
            bool success = false;
            var tableExists = sQLHandler.TableExists(schema, tableName); // dtName);
            if (tableExists)
            {
                int failures = 0;
                // check all columns
                foreach (var col in tableColumns)
                {
                    var colName = col.Key;
                    var colType = col.Value.ColType;
                    var ct = sQLHandler.ColTypeStringToEnum(colType);
                    var colLength = col.Value.ColLength;   // max length in table
                    var dbColLength = sQLHandler.ColumnLength(schema, tableName, colName);
                    var colExists = (dbColLength != 0);
                    if (!colExists)
                    {
                        if (colName.Equals("Name")) continue;   // skip this Name column?
                        var result = sQLHandler.AddColumn(schema, tableName, colName, ct, colLength);
                        if (!result)
                        {
                            failures++;
                            var msg = $"Add Column Error table [{schema}].[{tableName}] Column {colName}.";
                            Console.WriteLine(msg);
                        }
                    }
                    if ((dbColLength < colLength) && (dbColLength < SQLHandler.MaxStringLength))
                    {
                        var result = sQLHandler.AlterColumn(schema, tableName, colName, ct, colLength);
                        if (result)
                        {
                            failures++;
                            var msg = $"Alter Column Error? table [{schema}].[{tableName}] Column {colName}.";
                            Console.WriteLine(msg);
                        }
                    }
                }
                if (failures == 0) success = true;
            }
            else
            {
                // how long is the "Name" key field?
                int nameLength = 32;    // guess
                {
                    try
                    {
                        nameLength = tableColumns["Name"].ColLength;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error creating table [{schema}].[{tableName}] - Can't find 'Name' column. {ex.Message}");
                    }
                }
                // create new table
                var newtable = sQLHandler.PrepareTable(schema, tableName, nameLength);
                if (newtable)
                {
                    bool columnsOK = true;
                    foreach (var col in tableColumns)
                    {
                        var colName = col.Key;
                        if (colName.Equals("Name")) continue;   // skip this name, OK dont skip this,but must do sooner
                        var colType = col.Value.ColType;
                        var ct = sQLHandler.ColTypeStringToEnum(colType);
                        var colLength = col.Value.ColLength;   // max length in table
                        if (colName.Equals("Path")) { colName = "PathId"; ct = SQLHandler.ColumnType.ctInt; colLength = 4; }
                        if (colName.Equals("Instance")) { colName = "InstanceId"; ct = SQLHandler.ColumnType.ctInt; colLength = 4; }
                        var newparm1 = sQLHandler.PrepareColumn(colName, ct, colLength);
                        if (!newparm1)
                        {
                            columnsOK = false;
                            var msg = $"Column Error table [{schema}].[{tableName}] Column {colName}.";
                            Console.WriteLine(msg);
                        }
                    }
                    if (columnsOK)
                    {
                        var newTableCreated = sQLHandler.CreateTable();
                        success = newTableCreated;
                        Console.WriteLine($"Could {(newTableCreated? "" : "not ")}create table [{schema}].[{tableName}]");
                    }
                    else
                    {
                        sQLHandler.AbandonNewTable();
                        Console.WriteLine($"Could not create table [{schema}].[{tableName}]");
                    }
                }
            }
            return success;
        }

        private DataTable AddColumnsToDataTable(string tableName, SortedDictionary<string, TableColumn> columnList)
        {
            var dt = new DataTable(tableName);
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("ServerId", typeof(int));
            //dt.Columns.Add("InstanceId", typeof(int));
            //dt.Columns.Add("PathId", typeof(int));
            dt.Columns.Add("Name", typeof(string));

            foreach (var part in columnList.Values)
            {
                var pName = part.ColName;
                if (pName == "Name") continue;
                //if (pName == "Instance") continue;
                //if (pName == "Path") continue;
                if (pName == "Server") continue;
                if (pName == "ServerId") continue;
                var pType = part.ColType;
                var len = part.ColLength;
                // JDJ 8-29-2018 - add two special lookups - Path and Instance
                if (pName.Equals("Path")) { pName = "PathId"; pType = "Int32"; len = 4; }
                if (pName.Equals("Instance")) { pName = "InstanceId"; pType = "Int32"; len = 4; }
                bool setMaxLength = false;
                DataColumn dc = new DataColumn()
                {
                    ColumnName = pName,
                    AllowDBNull = true
                };
                if (pType.StartsWith("String"))
                {
                    dc.DataType = typeof(String);
                    setMaxLength = true;
                }
                else if (pType.Equals("Byte")) { dc.DataType = typeof(Byte); }
                else if (pType.Equals("Boolean")) { dc.DataType = typeof(Boolean); }
                else if (pType.Equals("UInt16")) { dc.DataType = typeof(Int16); }   // fix UInt to Int
                else if (pType.Equals("UInt32")) { dc.DataType = typeof(Int32); }   // fix UInt to Int
                else if (pType.Equals("UInt64")) { dc.DataType = typeof(Int64); }   // fix UInt to Int
                else if (pType.Equals("Int16")) { dc.DataType = typeof(Int16); }
                else if (pType.Equals("Int32")) { dc.DataType = typeof(Int32); }
                else if (pType.Equals("Int64")) { dc.DataType = typeof(Int64); }
                else if (pType.Equals("DateTime")) { dc.DataType = typeof(DateTime); }
                else
                {
                    dc.DataType = typeof(String);
                    setMaxLength = true;
                    throw new EvaluateException($"Unhandled Type: {pType}");
                }
                if (setMaxLength) dc.MaxLength = len;
                dt.Columns.Add(dc);
            }
            return dt;
        }


        private SortedDictionary<string, TableColumn> PartsSizes(InfoParts fullPartsList)
        {
            var tableColumns = new SortedDictionary<string, TableColumn>();
            int identityLength = 1;
            foreach (var part in fullPartsList.PartsList)
            {
                int iLen = part.Identity.Length;
                if (identityLength < iLen) identityLength = iLen;
                var pName = part.Name;
                var pType = part.Type;
                var len = 0;
                if (pType.EndsWith("[]"))
                {
                    pType = "String";
                    len = part.Value.Length;
                }
                else if (pType.StartsWith("$"))
                {
                    bool ok = int.TryParse(pType.Remove(0, 1), out len);
                    if (ok)
                    {
                        pType = "String";
                    }
                    else
                    {
                        len = 0;
                    }
                }
                else if (pType.Equals("Byte"))
                {
                    len = 1;
                }
                else if (pType.Equals("Boolean"))
                {
                    len = 1;
                }
                else if (pType.Equals("UInt16"))
                {
                    len = 2;
                }
                else if (pType.Equals("UInt32"))
                {
                    len = 4;
                }
                else if (pType.Equals("UInt64"))
                {
                    len = 8;
                }
                else if (pType.Equals("Int16"))
                {
                    len = 2;
                }
                else if (pType.Equals("Int32"))
                {
                    len = 4;
                }
                else if (pType.Equals("Int64"))
                {
                    len = 8;
                }
                else if (pType.Equals("DateTime"))
                {
                    len = 8;
                }
                else if (pType.Equals("Microsoft.PowerShell.Cmdletization.GeneratedTypes.ScheduledTask.StateEnum"))
                {
                    len = part.Value.Length;
                    pType = "String";
                }
                else
                {
                    len = part.Value.Length;
                    Console.WriteLine($"Unexpected Type on: {part.Identity}, Part: {pName}, Type: {pType} set to String. Length {len}");
                    pType = "String";
                    //throw new EvaluateException($"Unhandled Type: {pType}");
                }
                var found = tableColumns.TryGetValue(pName, out TableColumn tc);
                if (found)
                {
                    if (tc.ColLength < len)
                    {
                        tc.ColLength = len;
                    }
                }
                else
                {
                    var newtc = new TableColumn()
                    {
                        ColName = pName,
                        ColType = pType,
                        ColLength = len
                    };
                    tableColumns.Add(pName, newtc);
                }
            }
            // fix up Name field length from Identity part of infoPart
            {
                var found = tableColumns.TryGetValue("Name", out TableColumn tc);
                if (found)
                {
                    if (tc.ColLength < identityLength)
                    {
                        tc.ColLength = identityLength;
                    }
                }
                else
                {
                    var newtc = new TableColumn()
                    {
                        ColName = "Name",
                        ColType = "String",
                        ColLength = identityLength
                    };
                    tableColumns.Add("Name", newtc);
                }
            }
            return tableColumns;

        }

        //// https://stackoverflow.com/questions/12416249/hashing-a-string-with-sha256
        //static string HashStringOfString(string randomString)
        //{
        //    var crypt = new System.Security.Cryptography.SHA256Managed();
        //    var hash = new System.Text.StringBuilder();
        //    byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
        //    // johnston change
        //    return Convert.ToBase64String(crypto);
        //    //foreach (byte theByte in crypto) {hash.Append(theByte.ToString("x2"));}
        //    //return hash.ToString();
        //}

        //// https://stackoverflow.com/questions/1389570/c-sharp-byte-array-comparison
        //// jon skeet suggestion, further work by Guffa, and 8byte suggestion by Joe

        //public unsafe bool ByteArraysEqual(byte[] b1, byte[] b2)
        //{
        //    if (b1 == b2) return true;
        //    if (b1 == null || b2 == null) return false;
        //    if (b1.Length != b2.Length) return false;
        //    int len = b1.Length;
        //    fixed (byte* p1 = b1, p2 = b2)
        //    {
        //        long* i1 = (long*)p1;
        //        long* i2 = (long*)p2;
        //        while (len >= sizeof(long))
        //        {
        //            if (*i1 != *i2) return false;
        //            i1++;
        //            i2++;
        //            len -= sizeof(long);
        //        }
        //        byte* c1 = (byte*)i1;
        //        byte* c2 = (byte*)i2;
        //        while (len > 0)
        //        {
        //            if (*c1 != *c2) return false;
        //            c1++;
        //            c2++;
        //            len--;
        //        }
        //    }
        //    return true;
        //}


        // https://stackoverflow.com/questions/43289/comparing-two-byte-arrays-in-net
        // dotNet 4.7
        // byte[] is implicitly convertible to ReadOnlySpan<byte>
        //static bool ByteArrayCompare(ReadOnlySpan<byte> a1, ReadOnlySpan<byte> a2)
        //{
        //    return a1.SequenceEqual(a2);
        //}

        //static void TestSpan()
        //{
        //    var arr = new byte[10];
        //    Span<byte> bytes = arr; // Implicit cast from T[] to Span<T> 
        //}

        //// 64 bit hash
        //// https://stackoverflow.com/questions/8820399/c-sharp-4-0-how-to-get-64-bit-hash-code-of-given-string
        //static Int64 GetInt64HashCode(string strText)
        //{
        //    Int64 hashCode = 0;
        //    if (!string.IsNullOrEmpty(strText))
        //    {
        //        //Unicode Encode Covering all characterset
        //        byte[] byteContents = Encoding.UTF8.GetBytes(strText);
        //        //System.Security.Cryptography.SHA256 hash =
        //        //            new System.Security.Cryptography.SHA256CryptoServiceProvider();
        //        System.Security.Cryptography.SHA256 hash2 = new System.Security.Cryptography.SHA256Managed();
        //        byte[] hashText = hash2.ComputeHash(byteContents);
        //        //32Byte hashText separate
        //        //hashCodeStart = 0~7  8Byte
        //        //hashCodeMedium = 8~23  8Byte
        //        //hashCodeEnd = 24~31  8Byte
        //        //and Fold
        //        Int64 hashCodeStart = BitConverter.ToInt64(hashText, 0);
        //        //Int64 hashCodeFirstMid = BitConverter.ToInt64(hashText, 8);
        //        //Int64 hashCodeLastMid = BitConverter.ToInt64(hashText, 16);
        //        //Int64 hashCodeEnd = BitConverter.ToInt64(hashText, 24);
        //        hashCode = hashCodeStart; // ^ hashCodeFirstMid ^ hashCodeLastMid ^ hashCodeEnd;
        //    }
        //    return (hashCode);
        //}

    }
}
