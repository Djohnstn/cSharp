﻿using DirectorySecurityList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIMSave
{
    class CimDirectoryToDB
    {

        internal class XCimFile
        {
            internal int instanceId;
            internal int folderId;
            internal CIMFileInfo fileInfo;
        }

        internal class XCimType
        {
            internal int instanceId;
            internal int folderId;
            internal CIMFileTypeinfo typeInfo;
        }


        private CIMDirectoryCollection Files;
        private ACLSet aclset;
        private SQLHandler sqlHandler;
        private SQLHandlerBase sQLHandlerBase;
        readonly string ConnectionString;

        public CimDirectoryToDB(CIMDirectoryCollection newFiles, ACLSet newAclset)
        {
            Files = newFiles;
            aclset = newAclset;
            sqlHandler = new SQLHandler();
            sQLHandlerBase = new SQLHandlerBase();
            ConnectionString = sQLHandlerBase.ConnectionString;
        }

        internal void ToDB_first()
        {
            //using (var con as )
            var foundDirectories = new List<string>();
            var serverid = (new CIMSave.Servers()).ID(Files.Server);
            foreach (var directory in Files.Directories)
            {
                var key = directory.Key;
                foundDirectories.Add(key);  // keep list of directories so we can delete old records that don't exist now
                var value = directory.Value;
                var dirDbid = GetDirectoryDbID(serverid, key, value, aclset);
                FileListToDb(serverid, dirDbid, value);
                TypeListToDb(serverid, dirDbid, value);
            }

            // delete from directories were olddir not in foundDirectories
            throw new NotImplementedException();
        }

        //public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        //{
        //    foreach (var item in source)
        //        action(item);
        //}

        public void ToDB()
        {
            var dirInt = DirectoriesToDB();
            var serverId = sqlHandler.ServerID(Files.Server);

            FilesToDb(dirInt, serverId);
            FileTypesToDb(dirInt, serverId);
        }

        private int FilesToDb(Dictionary<string, int> dirInt, int serverId)
        {

            var thisProc = "FilesToDb";
            var spot = "FilesToDb";
            int rows = 0;
            int rc = 0;

            SqlTransaction trans = null;
            using (var conn = new SqlConnection(ConnectionString))
                try
                {
                    spot = "conn.Open";
                    conn.Open();
                    trans = conn.BeginTransaction();

                    spot = "Create Table #cimFiles";
                    const string sql1 = @"CREATE TABLE #cimFiles ( 
                                serverid INT, 
                                instanceid INT, 
                                pathid INT, 
                                name NVARCHAR(255) NOT NULL, 
                                version NVARCHAR(80),
                                length INT, 
                                modified DateTime2(7),
                                hash VARCHAR(44));";
                    ExecSqlInTransaction(conn, trans, sql1);

                    spot = "INSERT prep";
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        {
                            const string sqlInsTemp =
                                "INSERT INTO #cimFiles VALUES (@serverid, @instanceid, @pathid, @name, @version, @length, @modified, @hash);";
                            cmd.CommandText = sqlInsTemp;
                            cmd.Parameters.Add("@serverid", SqlDbType.Int);
                            cmd.Parameters.Add("@instanceid", SqlDbType.Int);
                            cmd.Parameters.Add("@pathid", SqlDbType.Int);
                            cmd.Parameters.Add("@name", SqlDbType.NVarChar, 255);
                            cmd.Parameters.Add("@version", SqlDbType.NVarChar, 80);
                            cmd.Parameters.Add("@length", SqlDbType.Int);
                            cmd.Parameters.Add("@modified", SqlDbType.DateTime2, 7);
                            cmd.Parameters.Add("@hash", SqlDbType.VarChar, 44);
                            cmd.Transaction = trans;

                            cmd.Prepare();
                        }
                        spot = "foreach in EachFile";

                        foreach (var file in EachFile(dirInt))
                        {
                            spot = "foreach try " + file.fileInfo.Name;
                            //var dirDbid = GetDirectoryDbID(serverid, key, value, aclset);
                            try
                            {
                                cmd.Parameters[0].Value = serverId;
                                cmd.Parameters[1].Value = file.instanceId;
                                cmd.Parameters[2].Value = file.folderId;
                                cmd.Parameters[3].Value = file.fileInfo.Name;
                                cmd.Parameters[4].Value = file.fileInfo.Version;
                                cmd.Parameters[5].Value = file.fileInfo.Length;
                                cmd.Parameters[6].Value = file.fileInfo.UtcModified;
                                cmd.Parameters[7].Value = file.fileInfo.Hash;
                                spot = "ExecuteNonQuery Insert";
                                cmd.ExecuteNonQuery(); //Error thrown here
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"{thisProc}.{spot}: " + e.Message);
                                throw;
                            }
                        }

                    }

                    spot = "sqlDelGone";
                    const string sqlDelGone = @"DELETE f FROM DBO.CIM_FileNameInventory AS f  
                                    INNER JOIN #cimFiles AS n ON n.ServerID = f.ServerID 
                                                    and n.instanceId = f.instanceId
                                    WHERE Not Exists (Select 1 From #cimFiles m
                                                    Where m.ServerID = f.ServerID 
                                                    and m.instanceId = f.instanceId
                                                    and m.pathId = f.pathId
                                                    and m.name = f.name) ";

                    rc = ExecSqlInTransaction(conn, trans, sqlDelGone);
                    if (rc > 0) rows += rc;
                    spot = "sqlInsNew";
                    const string sqlInsNew = @"INSERT INTO DBO.CIM_FileNameInventory 
                                            (serverid, instanceid, pathid, name, version, length, modified, hash)
                                Select n.serverid, n.instanceid, n.pathid, n.name, n.version, n.length, n.modified, n.hash
                                FROM #cimFiles AS n  
                                LEFT JOIN DBO.CIM_FileNameInventory AS f ON n.ServerID = f.ServerID 
                                                    and n.instanceId = f.instanceId
                                                    and n.pathId = f.pathId
                                                    and n.name = f.name
                                WHERE f.name IS NULL ";

                    rc = ExecSqlInTransaction(conn, trans, sqlInsNew);
                    if (rc > 0) rows += rc;

                    spot = "sqlUpdate";
                    //throw new NotImplementedException();
                    const string sqlUpdate =
                        @"UPDATE f SET version = n.version, length = n.length, modified = n.modified, hash = n.hash
                          FROM DBO.CIM_FileNameinventory AS f   
                          INNER JOIN #cimFiles AS n ON n.ServerID = f.ServerID 
                                                    and n.instanceId = f.instanceId
                                                    and n.pathId = f.pathId
                                                    and n.name = f.name
                          WHERE ISNULL(f.version, '') <> ISNULL(n.version, '') 
                             OR ISNULL(f.length, -1) <> ISNULL(n.length, -1)
                             OR ISNULL(f.modified, '1900-01-01') <> ISNULL(n.modified, '1900-01-01')
                             OR ISNULL(f.hash, '') <> ISNULL(n.hash, '') ";

                    //WHERE f.version <> n.version   OR (f.version IS NULL <> n.version IS NULL) 
                    //   OR f.length <> n.length     OR (f.length IS NULL <> n.length IS NULL) 
                    //   OR f.modified <> n.modified OR (f.modified IS NULL <> n.modified IS NULL) 
                    //   OR f.hash <> n.hash         OR (f.hash IS NULL <> n.hash IS NULL) ";

                    rc = ExecSqlInTransaction(conn, trans, sqlUpdate);
                    if (rc > 0) rows += rc;


                    //spot = "sqlSelect";
                    //const string sqlSelect = @"SELECT n.jsonkey, f.id
                    //            FROM  #cimFile AS n
                    //            INNER JOIN DBO.CIM_Fileinventory AS f ON n.ServerID = f.ServerID 
                    //                                and n.instanceId = f.instanceId
                    //                                and n.pathId = f.pathId
                    //                                and n.name = f.name";
                    //var dt = ExecSqlDataTable(conn, trans, sqlSelect);
                    //var d = dt.AsEnumerable().ToDictionary<DataRow, string, int>(row => row.Field<string>(0), row => row.Field<int>(1));

                    //dx = dt.AsEnumerable().ToDictionary(row => row.Field<string>(0), row => row.Field<int>(1));


                    spot = "Commit";
                    trans.Commit();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{thisProc}.{spot}: " + e.Message);
                    trans?.Rollback();
                }
                finally
                {
                    Console.WriteLine($"{thisProc}.Rows Updated = {rows}");
                }
            spot = "out!";
            //Other statements after SQL completed
            return rows;
        }

        private IEnumerable<XCimFile> EachFile(Dictionary<string, int> dirInt)
        {
            int diskId = -1;
            foreach (var directory in Files.Directories)
            {
                var key = directory.Key;
                if (diskId == -1)
                {
                    var diskname = GetDirectoryDisk(key);
                    diskId = CIMSave.Instances.ID(diskname);
                }
                var dirDbid = dirInt[key];
                var value = directory.Value;
                foreach(var file in value.FileList)
                {
                    yield return new XCimFile()
                    {
                        instanceId = diskId,
                        folderId = dirDbid,
                        fileInfo = file.Value
                    };
                }
            }
        }

        private int FileTypesToDb(Dictionary<string, int> dirInt, int serverId)
        {

            var thisProc = "FileTypesToDb";
            var spot = "FileTypesToDb";
            int rows = 0;
            int rc = 0;

            SqlTransaction trans = null;
            using (var conn = new SqlConnection(ConnectionString))
                try
                {
                    spot = "conn.Open";
                    conn.Open();
                    trans = conn.BeginTransaction();

                    spot = "Create Table #cimFileTypes";
                    const string sql1 = @"CREATE TABLE #cimFileTypes ( 
                                serverid INT, 
                                instanceid INT, 
                                pathid INT, 
                                Name NVARCHAR(255) NOT NULL, 
                                FileCount INT, 
                                BytesUsedAll BIGINT, 
                                BytesSmallest BIGINT, 
                                BytesLargest BIGINT, 
                                UtcOldest DateTime2(7),
                                UtcNewest DateTime2(7),
                                FileName1 NVARCHAR(255),
                                FileName2 NVARCHAR(255)); ";
                    ExecSqlInTransaction(conn, trans, sql1);

                    spot = "INSERT prep";
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        {
                            const string sqlInsTemp =
                                @"INSERT INTO #cimFileTypes VALUES (@serverid, @instanceid, @pathid, @Name,
                                    @FileCount, @BytesUsedAll, @BytesSmallest, @BytesLargest, @UtcOldest, @UtcNewest, @FileName1, @FileName2); ";

                            cmd.CommandText = sqlInsTemp;
                            cmd.Parameters.Add("@serverid", SqlDbType.Int);
                            cmd.Parameters.Add("@instanceid", SqlDbType.Int);
                            cmd.Parameters.Add("@pathid", SqlDbType.Int);
                            cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 255);
                            cmd.Parameters.Add("@FileCount", SqlDbType.Int);
                            cmd.Parameters.Add("@BytesUsedAll", SqlDbType.BigInt);
                            cmd.Parameters.Add("@BytesSmallest", SqlDbType.BigInt);
                            cmd.Parameters.Add("@BytesLargest", SqlDbType.BigInt);
                            cmd.Parameters.Add("@UtcOldest", SqlDbType.DateTime2, 7);
                            cmd.Parameters.Add("@UtcNewest", SqlDbType.DateTime2, 7);
                            cmd.Parameters.Add("@FileName1", SqlDbType.VarChar, 255);
                            cmd.Parameters.Add("@FileName2", SqlDbType.VarChar, 255);
                            cmd.Transaction = trans;

                            cmd.Prepare();
                        }
                        spot = "foreach in EachFile";

                        foreach (var ftype in EachFileType(dirInt))
                        {
                            spot = "foreach try " + ftype.typeInfo.FileType;
                            //var dirDbid = GetDirectoryDbID(serverid, key, value, aclset);
                            try
                            {
                                cmd.Parameters[0].Value = serverId;
                                cmd.Parameters[1].Value = ftype.instanceId;
                                cmd.Parameters[2].Value = ftype.folderId;
                                cmd.Parameters[3].Value = ftype.typeInfo.FileType;
                                cmd.Parameters[4].Value = ftype.typeInfo.FileCount;
                                cmd.Parameters[5].Value = ftype.typeInfo.BytesUsedAll;
                                cmd.Parameters[6].Value = ftype.typeInfo.BytesSmallest;
                                cmd.Parameters[7].Value = ftype.typeInfo.BytesLargest;
                                cmd.Parameters[8].Value = ftype.typeInfo.UtcOldest;
                                cmd.Parameters[9].Value = ftype.typeInfo.UtcNewest;
                                cmd.Parameters[10].Value = ftype.typeInfo.FileName1;
                                cmd.Parameters[11].Value = ftype.typeInfo.FileName2;
                                spot = "ExecuteNonQuery Insert";
                                cmd.ExecuteNonQuery(); //Error thrown here
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"{thisProc}.{spot}: " + e.Message);
                                throw;
                            }
                        }

                    }

                    spot = "sqlDelGone";
                    const string sqlDelGone = @"DELETE f FROM DBO.CIM_FileTypeInventory AS f  
                                    INNER JOIN #cimFileTypes AS n ON n.ServerID = f.ServerID 
                                                    and n.instanceId = f.instanceId
                                    WHERE Not Exists (Select 1 From #cimFileTypes m
                                                    Where m.ServerID = f.ServerID 
                                                    and m.instanceId = f.instanceId
                                                    and m.pathId = f.pathId
                                                    and m.name = f.name) ";

                    rc = ExecSqlInTransaction(conn, trans, sqlDelGone);
                    if (rc > 0) rows += rc;
                    spot = "sqlInsNew";
                    const string sqlInsNew =
                        @"INSERT INTO DBO.CIM_FileTypeInventory 
                        (serverid, instanceid, pathid, name, FileCount, BytesUsedAll, BytesSmallest, BytesLargest, UtcOldest, UtcNewest, FileName1, FileName2)
                Select n.serverid, n.instanceid, n.pathid, n.name, n.FileCount, n.BytesUsedAll, n.BytesSmallest, n.BytesLargest, n.UtcOldest, n.UtcNewest, n.FileName1, n.FileName2
                                FROM #cimFileTypes AS n  
                                LEFT JOIN DBO.CIM_FileTypeInventory AS f ON n.ServerID = f.ServerID 
                                                    and n.instanceId = f.instanceId
                                                    and n.pathId = f.pathId
                                                    and n.name = f.name
                                WHERE f.name IS NULL ";

                    rc = ExecSqlInTransaction(conn, trans, sqlInsNew);
                    if (rc > 0) rows += rc;

                    spot = "sqlUpdate";
                    //throw new NotImplementedException();
                    const string sqlUpdate =
                        @"UPDATE f SET FileCount = n.FileCount, BytesUsedAll = n.BytesUsedAll, BytesSmallest = n.BytesSmallest, 
                            BytesLargest = n.BytesLargest, UtcOldest = n.UtcOldest, UtcNewest = n.UtcNewest,
                            FileName1 = n.FileName1, FileName2 = n.FileName2
                          FROM DBO.CIM_FileTypeInventory AS f   
                          INNER JOIN #cimFileTypes AS n ON n.ServerID = f.ServerID 
                                                    and n.instanceId = f.instanceId
                                                    and n.pathId = f.pathId
                                                    and n.name = f.name
                          WHERE ISNULL(f.FileCount,-1) <> ISNULL(n.FileCount, -1) 
                             OR ISNULL(f.BytesUsedAll, -1) <> ISNULL(n.BytesUsedAll, -1) 
                             OR ISNULL(f.BytesSmallest, -1) <> ISNULL(n.BytesSmallest, -1) 
                             OR ISNULL(f.BytesLargest, -1) <> ISNULL(n.BytesLargest, -1)
                             OR ISNULL(f.UtcOldest, '1900-01-01') <> ISNULL(n.UtcOldest, '1900-01-01') 
                             OR ISNULL(f.UtcNewest, '1900-01-01') <> ISNULL(n.UtcNewest, '1900-01-01') 
                             OR ISNULL(f.FileName1, '') <> ISNULL(n.FileName1, '') 
                             OR ISNULL(f.FileName2, '') <> ISNULL(n.FileName2, '') ";

                    rc = ExecSqlInTransaction(conn, trans, sqlUpdate);
                    if (rc > 0) rows += rc;


                    //spot = "sqlSelect";
                    //const string sqlSelect = @"SELECT n.jsonkey, f.id
                    //            FROM  #cimFile AS n
                    //            INNER JOIN DBO.CIM_Fileinventory AS f ON n.ServerID = f.ServerID 
                    //                                and n.instanceId = f.instanceId
                    //                                and n.pathId = f.pathId
                    //                                and n.name = f.name";
                    //var dt = ExecSqlDataTable(conn, trans, sqlSelect);
                    //var d = dt.AsEnumerable().ToDictionary<DataRow, string, int>(row => row.Field<string>(0), row => row.Field<int>(1));

                    //dx = dt.AsEnumerable().ToDictionary(row => row.Field<string>(0), row => row.Field<int>(1));


                    spot = "Commit";
                    trans.Commit();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{thisProc}.{spot}: " + e.Message);
                    trans?.Rollback();
                }
                finally
                {
                    Console.WriteLine($"{thisProc}.Rows Updated = {rows}");
                }
            spot = "out!";
            //Other statements after SQL completed
            return rows;
        }


        private IEnumerable<XCimType> EachFileType(Dictionary<string, int> dirInt)
        {
            int diskId = -1;
            foreach (var directory in Files.Directories)
            {
                var key = directory.Key;
                if (diskId == -1)
                {
                    var diskname = GetDirectoryDisk(key);
                    diskId = CIMSave.Instances.ID(diskname);
                }
                var dirDbid = dirInt[key];
                var value = directory.Value;
                foreach (var type in value.Typelist)
                {
                    yield return new XCimType()
                    {
                        instanceId = diskId,
                        folderId = dirDbid,
                        typeInfo = type.Value
                    };
                }
            }
        }


        private Dictionary<string, int> DirectoriesToDB()
        {
            var thisProc = "DirectoriesToDB";
            var spot = "DirectoriesToDB";
            int rows = 0;
            int rc = 0;
            var dx = new Dictionary<string, int>(2000);

            SqlTransaction trans = null;
            using (var conn = new SqlConnection(ConnectionString))
            try
                {
                    spot = "conn.Open";
                    conn.Open();
                    trans = conn.BeginTransaction();

                    spot = "Create Table #cimFile";
                    const string sql1 = @"CREATE TABLE #cimFile ( 
                                serverid INT, 
                                instanceid INT, 
                                pathid INT, 
                                name NVARCHAR(255) NOT NULL, 
                                aclid INT, 
                                aclsameasparent BIT,
                                jsonkey NVARCHAR(255));";
                    ExecSqlInTransaction(conn, trans, sql1);

                    spot = "INSERT prep";
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        const string sqlInsTemp = "INSERT INTO #cimFile VALUES (@serverid, @instanceid, @pathid, @name, @aclid, @aclsameasparent, @jsonkey);";
                        cmd.CommandText = sqlInsTemp;
                        cmd.Parameters.Add("@serverid", SqlDbType.Int);
                        cmd.Parameters.Add("@instanceid", SqlDbType.Int);
                        cmd.Parameters.Add("@pathid", SqlDbType.Int);
                        cmd.Parameters.Add("@name", SqlDbType.NVarChar, 255);
                        cmd.Parameters.Add("@aclid", SqlDbType.Int);
                        cmd.Parameters.Add("@aclsameasparent", SqlDbType.Bit);
                        cmd.Parameters.Add("@jsonkey", SqlDbType.NVarChar, 255);
                        cmd.Transaction = trans;

                        cmd.Prepare();
                        spot = "ServerID";
                        var serverid = (new CIMSave.Servers()).ID(Files.Server);

                        foreach (var directory in Files.Directories)
                        {
                            spot = "foreach";
                            var key = directory.Key;
                            var diskname = GetDirectoryDisk(key);
                            var diskId = CIMSave.Instances.ID(diskname);
                            var pathname = GetDirectoryPath(key);
                            var pathId = CIMSave.Paths.ID(pathname);
                            var thedir = GetDirectoryName(key);
                            //foundDirectories.Add(key);  // keep list of directories so we can delete old records that don't exist now
                            var value = directory.Value;
                            var aclDbId = aclset.GetAclDbId(value.ACLid);
                            var aclSameAsParent = value.AclSameAsParent;
                            //var dirDbid = GetDirectoryDbID(serverid, key, value, aclset);
                            try
                            {
                                cmd.Parameters[0].Value = serverid;
                                cmd.Parameters[1].Value = diskId;
                                cmd.Parameters[2].Value = pathId;
                                cmd.Parameters[3].Value = thedir;
                                cmd.Parameters[4].Value = aclDbId;
                                cmd.Parameters[5].Value = aclSameAsParent;
                                cmd.Parameters[6].Value = key;
                                spot = "ExecuteNonQuery Insert";
                                cmd.ExecuteNonQuery(); //Error thrown here
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"{thisProc}.{spot}: " + e.Message);
                                throw;
                            }
                        }

                    }

                    spot = "sqlDelGone";
                    const string sqlDelGone = @"DELETE f FROM DBO.CIM_Fileinventory AS f  
                                INNER JOIN #cimFile AS n ON n.ServerID = f.ServerID 
                                                    and n.instanceId = f.instanceId
                                  WHERE Not Exists (Select * From #cimFile m
                                                    Where m.ServerID = f.ServerID 
                                                    and m.instanceId = f.instanceId
                                                    and m.pathId = f.pathId
                                                    and m.name = f.name) ";

                    rc = ExecSqlInTransaction(conn, trans, sqlDelGone);
                    if (rc > 0) rows += rc;
                    spot = "sqlInsNew";
                    const string sqlInsNew = 
                        @"INSERT INTO DBO.CIM_Fileinventory 
                                            (serverid, instanceid, pathid, name, aclid, aclsameasparent)
                          Select n.serverid, n.instanceid, n.pathid, n.name, n.aclid, n.aclsameasparent
                          FROM #cimFile AS n  
                          LEFT JOIN DBO.CIM_Fileinventory AS f ON n.ServerID = f.ServerID 
                                                and n.instanceId = f.instanceId
                                                and n.pathId = f.pathId
                                                and n.name = f.name
                          WHERE f.name IS NULL ";

                    rc = ExecSqlInTransaction(conn, trans, sqlInsNew);
                    if (rc > 0) rows += rc;

                    spot = "sqlUpdate";
                    const string sqlUpdate = 
                        @"UPDATE f SET aclid = n.aclid, aclsameasparent = n.aclsameasparent
                          FROM DBO.CIM_Fileinventory AS f   
                          INNER JOIN #cimFile AS n ON n.ServerID = f.ServerID 
                                                    and n.instanceId = f.instanceId
                                                    and n.pathId = f.pathId
                                                    and n.name = f.name
                          WHERE f.aclid <> n.aclid OR f.aclsameasparent <> n.aclsameasparent ";

                    rc = ExecSqlInTransaction(conn, trans, sqlUpdate);
                    if (rc > 0) rows += rc;


                    spot = "sqlSelect";
                    const string sqlSelect = @"SELECT n.jsonkey, f.id
                                FROM  #cimFile AS n
                                INNER JOIN DBO.CIM_Fileinventory AS f ON n.ServerID = f.ServerID 
                                                    and n.instanceId = f.instanceId
                                                    and n.pathId = f.pathId
                                                    and n.name = f.name";
                    var dt = ExecSqlDataTable(conn, trans, sqlSelect);
                    //var d = dt.AsEnumerable().ToDictionary<DataRow, string, int>(row => row.Field<string>(0), row => row.Field<int>(1));

                    dx = dt.AsEnumerable().ToDictionary(row => row.Field<string>(0), row => row.Field<int>(1));


                    spot = "Commit";
                    trans.Commit();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{thisProc}.{spot}: " + e.Message);
                    trans?.Rollback();
                }
                finally
                {
                    Console.WriteLine($"{thisProc}.Rows Updated = {rows}");
                }
            spot = "out!";
            //Other statements after SQL completed
            return dx;
        }

        private static int ExecSqlInTransaction(SqlConnection conn, SqlTransaction trans, string sql)
        {
            int rows = -1;
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.Transaction = trans;
                cmd.CommandText = sql;
                rows = cmd.ExecuteNonQuery();
            }
            return rows;
        }

        private static DataTable ExecSqlDataTable(SqlConnection conn, SqlTransaction trans, string sql)
        {
            var dt = new DataTable();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.Transaction = trans;
                cmd.CommandText = sql;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    dt.Load(reader);
                }
            }
            return dt;
        }

        private static void TypeListToDb(int serverid, int dirDbId, CIMDirectoryInfo value)
        {
            foreach (var filetype in value.Typelist)
            {

            }
            throw new NotImplementedException();
        }

        private static void FileListToDb(int serverid, int dirDbId, CIMDirectoryInfo value)
        {
            var foundFiles = new List<string>();
            foreach (var file in value.FileList)
            {
                var fname = file.Key;
                var fvalue = file.Value;
                var fname2 = fvalue.Name;
                foundFiles.Add(fname2);
                var flength = fvalue.Length;
                var fmodified = fvalue.UtcModified;
                var fversion = fvalue.Version;
                var fhash = fvalue.Hash;
            }
            // delete from table where serverid = serverid and dirinfo = x and name not in foundfiles;
            throw new NotImplementedException();
        }

        private int GetDirectoryDbID(int serverid, string key, CIMDirectoryInfo value, ACLSet aclset)
        {
            var diskname = GetDirectoryDisk(key);
            var diskId = CIMSave.Instances.ID(diskname);
            var pathname = GetDirectoryPath(key);
            var pathId = CIMSave.Paths.ID(pathname);
            var thedir = GetDirectoryName(key);
            var aclDbId = aclset.GetAclDbId(value.ACLid);
            var aclSameAsParent = value.AclSameAsParent;
            var err = value.ErrorAccessing;
            throw new NotImplementedException();
        }
        string GetDirectoryDisk(string f)
        {
            try
            {
                return f.Substring(0, f.IndexOf(':') + 1);
            }
            catch
            {
                return string.Empty;
            }
        }
        string GetDirectoryPath(string f)
        {
            try
            {
                return f.Substring(0, f.LastIndexOf('\\'));
            }
            catch
            {
                return string.Empty;
            }
        }
        string GetDirectoryName(string f)
        {
            try
            {
                return f.Substring(f.LastIndexOf('\\') + 1);
            }
            catch
            {
                return string.Empty;
            }
        }


    }
}
