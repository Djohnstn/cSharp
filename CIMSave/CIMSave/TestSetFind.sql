
-- https://www.red-gate.com/simple-talk/sql/t-sql-programming/divided-we-stand-the-sql-of-relational-division/

-- looking for way to find existing list that matches new list

set nocount on
DECLARE @t1 DATETIME;	-- millisecond performance timers
DECLARE @t2 DATETIME;

SET @t1 = GETDATE();

if (1=0)
begin
	IF OBJECT_ID('tempdb..#set') IS NOT NULL
		DROP TABLE #set
--IF OBJECT_ID('tempdb..#set2') IS NOT NULL
--    DROP TABLE #set2

	CREATE TABLE #set
	(
	  grp int INDEX IX1 nonCLUSTERED, 
	  id int INDEX IX2 CLUSTERED,
		INDEX IX3 NONCLUSTERED(Grp, id)
	--INDEX IX4 NONCLUSTERED( id, grp)
	);
end;


DECLARE @SET2 TABLE 
(
  grp int, --INDEX IX1 CLUSTERED, 
  id int --INDEX IX2 NONCLUSTERED,
	--INDEX IX3 NONCLUSTERED(Grp, id)
);

if (1=0)
begin
	IF OBJECT_ID('tempdb..#acl') IS NOT NULL
    DROP TABLE #acl

	create table #acl
	(
	  grp int INDEX IX1 nonCLUSTERED, 
	  xsum int,
	  ids VARCHAR(1600), --INDEX IX2 CLUSTERED
	  --xsum AS CHECKSUM(IDS) 	--INDEX IX3 NONCLUSTERED(xsum, ids)
		--INDEX IX4 NONCLUSTERED( id, grp)
	);
end;

if not exists (Select 1 from #set )
begin
	insert into #set (grp, id)
	values (1, 1),
			(2, 1),
			(2, 2),
			(3, 1),
			(3, 2),
			(3, 3),
			(4, 4),
			(5, 1),
			(5, 2),
			(5, 3),
			(5, 4),
			(6, 7),
			(6, 8),
			(8, 1),	-- oops, duplicate set8 ;) 
			(8, 2),
			(8, 3),
			(8, 8),		
			(9, 1),	-- oops, duplicate set9 ;) 
			(9, 2),
			(9, 3),
			(9, 8)		
;
end;
-- build a large sample set
DECLARE @setS INT = 20960 --19000	-- vs 20000
DECLARE @TESTS INT = @setS * 7
DECLARE @counter INT  = 0
--WHILE @counter < @TESTS 
--  BEGIN 
--	insert into @set2 (grp, id)	values (Rand() * @sets + 10,  RAND() * 10900031 + 20);
--	-- weighted, as common ACEs wll happen
--	--insert into #set2 (grp, id)	values (Rand() * @SETS + 10,  -Log(RAND()) / 1.5 * 170000001 + 20);
--	SET @counter = @counter + 1 ;
--  END
----select * from @set order by grp, id
----go 100
----go

IF OBJECT_ID('[TEST_SET_RAND_2]') IS NULL
begin
	IF OBJECT_ID('[TEST_SET_RAND_2]') IS not NULL
		drop table [TEST_SET_RAND_2];
	;WITH
	L0 AS (SELECT 1 AS c UNION ALL SELECT 1),
	L1 AS (SELECT 1 AS c FROM L0 A CROSS JOIN L0 B),
	L2 AS (SELECT 1 AS c FROM L1 A CROSS JOIN L1 B),
	L3 AS (SELECT 1 AS c FROM L2 A CROSS JOIN L2 B),
	L4 AS (SELECT 1 AS c FROM L3 A CROSS JOIN L3),
	L5 AS (SELECT 1 AS c FROM L4 A CROSS JOIN L4),
	NUMS AS (SELECT 1 AS NUM FROM L5)   
	SELECT TOP 190000
	  --CAST(ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS INT) [ID]
		CAST(Abs(Rand(CHECKSUM(NewID())) * 20960) + 10 AS INT) [ACL]
		, CAST(t.RAND_VALUE AS INT) [ACE]
	INTO [TEST_SET_RAND_2]
	--FROM NUMS CROSS JOIN (SELECT Rand(Abs(CHECKSUM(NEWID()))) * 2073071595  + 20 as RAND_VALUE) t;
	FROM NUMS CROSS JOIN (SELECT Square((Log(Rand(Abs(CHECKSUM(NEWID()))))/2.01 * 6000))  + 20 as RAND_VALUE) t;
	--Select top 100 * from [TEST_SET_RAND_2] order by acl, ace;
	Select top 100 acl, ace, dbo.fn_IntToBase73(ace) from [TEST_SET_RAND_2] order by acl desc, ace asc;
end


SET @t2 = GETDATE();
--SELECT DATEDIFF(millisecond,@t1,@t2) AS elapsed_ms_construct; -- 900ms

--SET @t1 = GETDATE();
if not exists (Select 1 from #set where grp > 20 )
begin
	print 'Recalc needed';
	--insert into #set (grp, id) Select distinct  acl, ace from [TEST_SET_RAND_2]; -- @set2;
end;
Declare @setnumber int = (Select top 1 grp from #set s where s.grp >= Rand() * (@SETS + 10) order by s.grp)
--Select @setnumber as SetNumber
--Select * from #set order by grp desc
--Declare @rnum int = Rand() * (20960 + 10)
--select @rnum
--Select top 1 grp from #set s where s.grp >= @rnum order by s.grp
--SET @t2 = GETDATE();
--SELECT DATEDIFF(millisecond,@t1,@t2) AS elapsed_ms_select_Dictinct; -- <300ms

SET @t1 = GETDATE();

declare @News TABLE (id int INDEX IX1 CLUSTERED); -- interesting, index speeds this from 100 ms to 40ms
--insert into @News (id) values (1), (2), (3)
insert into @News (id) select distinct id from #set s where s.grp = @setnumber; -- what set to look at this time
--insert into @News (id) values (7), (8);
--Select * from @News;

Declare @NewsWide TABLE (sum1 int index ix1 clustered
						, csv varchar(900) INDEX IX2 nonCLUSTERED);
insert into @NewsWide (csv)
SELECT Cast(STUFF(
		(SELECT ',' + dbo.fn_IntToBase73(N.id) -- Format(N.id, 'X') --Cast(n.id as varchar(30))
		FROM @News n
		order by n.id
	FOR XML PATH('')),1,1,'') as varchar(1600)) as csv;
update @NewsWide set sum1 = CHECKSUM(csv);
--SELECT * from @NewsWide;
SET @t2 = GETDATE();
--SELECT DATEDIFF(millisecond,@t1,@t2) AS elapsed_ms_build_News;	-- 0 ms
DECLARE @newsWideXSum Int = (Select w.sum1 from @NewsWide w);
DECLARE @newsWideCsv varchar(900) = (Select w.csv from @NewsWide w);

if not exists (Select 1 from #acl )
begin

	INSERT INTO #acl (grp, ids)
	SELECT t.grp, 
		STUFF((SELECT ',' + dbo.fn_IntToBase73(S.id) -- Format(S.id, 'X') -- Cast(s.id as varchar(30))
				FROM #set s
				WHERE s.grp = t.grp
				order by s.id
				FOR XML PATH('')),1,1,'') AS CSV
	FROM #set AS t
	GROUP BY t.grp;
	update #acl set xsum = CHECKSUM(ids) from #acl;
	create index IXsum  on #acl(xsum) include (grp);
	create index IXsumIds  on #acl(xsum, ids)  include (grp);
	select top 10 * from #acl a order by a.grp desc;
end
---- SLOW - 19,157ms - 19,487 ms vs 37 - 44 ms for Except
--SET @t1 = GETDATE();
--Select s1.grp
--from @set as s1
--Left join @News as n1 on n1.id = s1.id
--where s1.grp not in (
--		Select distinct s2.grp  -- sets with parts not in new group
--		from @set as s2
--		left join @News as n2 on n2.id = s2.id
--		where n2.id is null
--		)
--group by s1.grp
--having count(s1.id) = (Select count(*) from @News)
--   and Count(n1.id)  = (Select count(*) from @News);
--SET @t2 = GETDATE();
--SELECT DATEDIFF(millisecond,@t1,@t2) AS elapsed_ms_find_a_Set_not_in;

SET @t1 = GETDATE();

Declare @firstResult int;
With Candidates as (
	Select  s0.grp	-- candidate sets that have the required parts
		from #set as s0
		inner join @News as n1 on n1.id = s0.id
		--group by s0.grp
),
SetDiscovery as (
	Select s1.grp	-- candidate sets that have the required parts
		from #set as s1
		inner join @News as n1 on n1.id = s1.id
		group by s1.grp
		having count(s1.id) = (Select count(*) from @News)
		   and Count(n1.id)  = (Select count(*) from @News)
	Except
	Select  s2.grp  -- sets with parts not in new group -- this will be a huge list
		from #set as s2
		--inner join Candidates c0 on c0.grp = s2.grp
		where Not Exists (Select 1 from @News n2 Where n2.id = s2.id)
		----left join @News as n2 on n2.id = s2.id
		----where n2.id is null;
		---- rumor is that the NOT Exists is slightly better than the Left Join is null
		---- they call it a "Left Anti-Semi Join" operator
)
Select @firstResult = s.grp 
	from SetDiscovery s;
SET @t2 = GETDATE();
Declare @GroupsCount int = (Select Count(Distinct grp) from #set)
Declare @GrpIdCount int = (Select Count(*) from #set)
Declare @NewsCount int = (Select Count(*) from @News)
--Select @NewsCount = Count(*) from @News
SELECT DATEDIFF(millisecond, @t1, @t2) AS elapsed_ms_find_a_Set_Except, @firstResult as Firstresult, @setnumber as SetNumber, 
			@NewsCount as NewListItemCount, @GroupsCount as Groups,
			@GrpIdCount as Items ; -- 45ms
--if (1=0) 
--begin
--	Select s.grp
--	from @news as n
--	full outer join @set as s on s.id = n.id
--	group by s.grp
--	having count(s.id) = (Select count(id) from @set)
--		and count(n.id) = (Select count(id) from @set)
--end

--Select * --s.com, count(s.id), count(n.id)
--from @set as s 
--full outer join @news as n on s.id = n.id
--group by s.com
--Where not exists

--SELECT S1.sup_nbr, S2.sup_nbr
--  FROM SupParts AS S1, SupParts AS S2
-- WHERE S1.sup_nbr < S2.sup_nbr -- different suppliers
--   AND S1.part_nbr = S2.part_nbr -- same parts
-- GROUP BY S1.sup_nbr, S2.sup_nbr
--HAVING COUNT(*) = (SELECT COUNT (*)  -- same count of parts
--                     FROM SupParts AS S3
--                    WHERE S3.sup_nbr = S1.sup_nbr)
--   AND COUNT(*) = (SELECT COUNT (*)
--                     FROM SupParts AS S4
--                    WHERE S4.sup_nbr = S2.sup_nbr);


-- exponential random numbers, wighted towards the left side
--Select -log(rand())/1.5

--select top 1000  * from @set




--Select t.* from #acl t where @setnumber = t.grp;
--select top 50 * from #acl


SET @t1 = GETDATE();
With pre as (
	Select a.grp, a.ids, a.xsum
	From #acl a
	Where a.xsum = @newsWideXSum
)
Select @newsWideXSum as NewsXSum, @newsWideCsv as NewsACECSV, a.*, LEN(a.ids) as Len_ids
from #acl a 
Where a.ids = @newsWideCsv --and a.xsum = @newsWideXSum 
SET @t2 = GETDATE();
SELECT DATEDIFF(millisecond, @t1, @t2) AS elapsed_ms_find_via_ParameterQuery;


SET @t1 = GETDATE();
Select n.*, a.*, LEN(a.ids) as Len_ids
from @NewsWide as n 
inner join #acl a on n.sum1 = a.xsum and n.csv = a.ids 
SET @t2 = GETDATE();
SELECT DATEDIFF(millisecond, @t1, @t2) AS elapsed_ms_find_in_reversedSet1A;

SET @t1 = GETDATE();
Select n.*, a.*, LEN(a.ids) as Len_ids
from @NewsWide as n 
inner join #acl a on n.sum1 = a.xsum and n.csv = a.ids 
SET @t2 = GETDATE();
SELECT DATEDIFF(millisecond, @t1, @t2) AS elapsed_ms_find_in_reversedSet1B;


SET @t1 = GETDATE();

;With newacl as (
	SELECT Cast(STUFF(
		(SELECT ',' + dbo.fn_IntToBase73(N.id) -- Format(N.id, 'X') --Cast(n.id as varchar(30))
		FROM @News n
		order by n.id
		FOR XML PATH('')),1,1,'') as varchar(1000)) as csv
),
summedAcl as (	Select csv, checksum(csv) as sum1 from newacl )
Select n.*, a.*, LEN(a.ids) as Len_ids
from summedAcl as n 
inner join #acl a on n.sum1 = a.xsum and n.csv = a.ids 

SET @t2 = GETDATE();
SELECT DATEDIFF(millisecond,@t1,@t2) AS elapsed_ms_find_a_reversedSet_WithCTE;


--Select a.grp, count(*) duplicate_checksums
--from #acl a
--group by a.grp
--having COUNT(1) > 1

-- CSV to columns

--With T as (
--	Select grp, 
--		cast('<r><d>' + replace(ids,',','</d><d>') + '</d></r>' as xml) as x
--	from #acl
--)
--Select top 10 t.grp, m.n.value('.[1]','varchar(80)') as cid
--from t
--cross apply x.nodes('/r/d')m(n)
--order by t.grp desc


--drop table #acl


--SELECT 100, master.dbo.fn_varbintohexstr(100)

--select CONVERT(VARCHAR(8),CONVERT(VARBINARY(4), 215),2)

--Declare @value1 int = 107
--Declare @hex1 varchar(30) = FORMAT(@value1,'X')
--Select @value1, @hex1


--          'xs:base64Binary(xs:hexBinary(sql:column("bin")))'
--Declare @value2 int = 107
--SELECT
--    CAST(N'' AS XML).value(
--          'xs:base64Binary(sql:column("bin"))'
--        , 'VARCHAR(MAX)'
--    )   Base64Encoding
--FROM (
--    SELECT CAST(@value2 AS VARBINARY(MAX)) AS bin
--) AS bin_sql_server_temp;

--select checksum('abbc'), checksum('Abbb')--, BINARY_CHECKSUM('A21'), BINARY_CHECKSUM('A22')

--GO
--CREATE FUNCTION dbo.fn_IntToBase73 (@input BIGINT)
--RETURNS VARCHAR(12)
--AS BEGIN
--	declare @fixthis BIGINT = @input;
--	declare @size int = 73;	-- 26 + 26 + 10 + 13
--	declare @basestring varchar(73) = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/-~@$*()_?[]';
--	declare @outstring varchar(12) = '';
--	declare @int1 INT;
--	if @input = 0 set @outstring = '0';
--	while @fixthis > 0
--	begin
--		Set @int1 = @fixthis % @size;
--		Set @outstring = SUBSTRING(@Basestring, @int1+1, 1) + @outstring;
--		Set @fixthis = @fixthis / @size;
--	end
--	RETURN @outstring;
--END
--GO
--GO
--ALTER FUNCTION dbo.fn_IntToBase36 (@input INT)
--RETURNS VARCHAR(8)
--AS BEGIN
--	declare @fixthis int = @input;
--	declare @basestring varchar(36) = '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ';
--	declare @outstring varchar(8) = '';
--	declare @int1 int;
--	if @input = 0 set @outstring = '0';
--	while @fixthis > 0
--	begin
--		Set @int1 = @fixthis % 36;
--		Set @outstring = SUBSTRING(@Basestring, @int1+1, 1) + @outstring;
--		Set @fixthis = @fixthis / 36;
--		--Select @int1 as int1, @outstring as outstring, @fixthis as Fixthis
--	end
--	RETURN @outstring;
--END
--GO

--Select 0, dbo.fn_IntToBase36(0)
--Select 1, dbo.fn_IntToBase36(1)
--Select 9, dbo.fn_IntToBase36(9)
--Select 10, dbo.fn_IntToBase36(10)
--Select 35, dbo.fn_IntToBase36(35)
--Select 36, dbo.fn_IntToBase36(36)
--Select 12345, dbo.fn_IntToBase36(12345)
--Select 32768, dbo.fn_IntToBase36(32768)
--Select 32768, dbo.fn_IntToBase36(32768)
--Select 60466177, dbo.fn_IntToBase36(60466177)		-- 36^5 + 1
--Select 2176782336, dbo.fn_IntToBase36(2176782336)	-- 36^6 - too big
--select 2147483647, dbo.fn_IntToBase36(2147483647)
--select 2147483648, dbo.fn_IntToBase36(2147483648)


if (1=0)
begin
	-- https://dba.stackexchange.com/questions/152530/fast-way-to-load-a-large-amount-of-test-data
	IF OBJECT_ID('[X_JRO_TEST_RAND_2]') IS NOT NULL
		drop table [X_JRO_TEST_RAND_2];
	;WITH
	L0 AS (SELECT 1 AS c UNION ALL SELECT 1),
	L1 AS (SELECT 1 AS c FROM L0 A CROSS JOIN L0 B),
	L2 AS (SELECT 1 AS c FROM L1 A CROSS JOIN L1 B),
	L3 AS (SELECT 1 AS c FROM L2 A CROSS JOIN L2 B),
	L4 AS (SELECT 1 AS c FROM L3 A CROSS JOIN L3),
	L5 AS (SELECT 1 AS c FROM L4 A CROSS JOIN L4),
	NUMS AS (SELECT 1 AS NUM FROM L5)   
	SELECT TOP 100000
	  --CAST(ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS INT) [ID]
		CAST(Abs(Rand(CHECKSUM(NewID())) * 40960) + 10 AS INT) [ACL]
		, CAST(t.RAND_VALUE AS INT) [ACE]
	INTO [X_JRO_TEST_RAND_2]
	FROM NUMS CROSS JOIN (SELECT Rand(Abs(CHECKSUM(NEWID()))) * 12003  + 20 as RAND_VALUE) t;
	Select top 100 * from [X_JRO_TEST_RAND_2] order by acl, ace;
end;

if (1=0)
begin

	select checksum(rand()) % 409600
	SELECT Rand(Abs(CHECKSUM(NEWID())) % 120031) * 120031 + 20 as RAND_VALUE
end;


if (1=0)
begin
	SELECT 
		s.Name AS SchemaName,
		t.NAME AS TableName,
		p.rows AS RowCounts,
		SUM(a.total_pages) * 8 AS TotalSpaceKB, 
		SUM(a.used_pages) * 8 AS UsedSpaceKB, 
		(SUM(a.total_pages) - SUM(a.used_pages)) * 8 AS UnusedSpaceKB
	FROM 
		sys.tables t
	INNER JOIN      
		sys.indexes i ON t.OBJECT_ID = i.object_id
	INNER JOIN 
		sys.partitions p ON i.object_id = p.OBJECT_ID AND i.index_id = p.index_id
	INNER JOIN 
		sys.allocation_units a ON p.partition_id = a.container_id
	LEFT OUTER JOIN 
		sys.schemas s ON t.schema_id = s.schema_id
	WHERE 
		t.NAME NOT LIKE 'dt%' 
		AND t.is_ms_shipped = 0
		AND i.OBJECT_ID > 255 
	GROUP BY 
		t.Name, s.Name, p.Rows
	ORDER BY 
		t.Name;
	SELECT
		OBJECT_SCHEMA_NAME(i.OBJECT_ID) AS SchemaName,
		OBJECT_NAME(i.OBJECT_ID) AS TableName,
		i.name AS IndexName,
		i.index_id AS IndexID,
		8 * SUM(a.used_pages) AS IndexsizeKB
	FROM
		sys.indexes AS i JOIN 
		sys.partitions AS p ON p.OBJECT_ID = i.OBJECT_ID AND p.index_id = i.index_id JOIN 
		sys.allocation_units AS a ON a.container_id = p.partition_id
	GROUP BY
		i.OBJECT_ID,
		i.index_id,
		i.name
	ORDER BY
		OBJECT_NAME(i.OBJECT_ID),
		i.index_id;
	SELECT * FROM SYS.server_principals;
	SELECT * FROM Master..SysUsers;
	SELECT * FROM CIMInfo..SysUsers;

end;

if (1=0)
begin;

	;with ServerPermsAndRoles as
	(
		select
			spr.name as principal_name,
			spr.type_desc as principal_type,
			spm.permission_name collate SQL_Latin1_General_CP1_CI_AS as security_entity,
			'permission' as security_type,
			spm.state_desc
		from sys.server_principals spr
		inner join sys.server_permissions spm
		on spr.principal_id = spm.grantee_principal_id
		where spr.type in ('s', 'u')

		union all

		select
			sp.name as principal_name,
			sp.type_desc as principal_type,
			spr.name as security_entity,
			'role membership' as security_type,
			null as state_desc
		from sys.server_principals sp
		inner join sys.server_role_members srm
		on sp.principal_id = srm.member_principal_id
		inner join sys.server_principals spr
		on srm.role_principal_id = spr.principal_id
		where sp.type in ('s', 'u')
	)
	select *
		, CAST(rp.[state_desc] AS VARCHAR(MAX)) + ' ' + 
		  CAST(rp.[security_entity] AS VARCHAR(MAX)) + 
		' TO [' + CAST(rp.[principal_name] AS VARCHAR(MAX)) + '];' as GrantQuery
	from ServerPermsAndRoles rp
	order by principal_name

	Select * from CIMInfo.Sys.database_principals;
	Select * from CIMInfo.sys.database_permissions;
	Select * from CIMInfo.Sys.database_principals pr 
		left join CIMInfo.sys.database_permissions dp on dp.grantee_principal_id = pr.principal_id
	-- 		OBJECT_SCHEMA_NAME(i.OBJECT_ID) AS SchemaName,

	select  princ.name
		,       princ.type_desc
		,       perm.permission_name
		,       perm.state_desc
		,       perm.class_desc
		,       object_name(perm.major_id)
		from    sys.database_principals princ
		left join sys.database_permissions perm on perm.grantee_principal_id = princ.principal_id
		order by princ.name 

	SELECT  
		[UserName] = ulogin.[name],
		[UserType] = CASE princ.[type]
						WHEN 'S' THEN 'SQL User'
						WHEN 'U' THEN 'Windows User'
						WHEN 'G' THEN 'Windows Group'
					 END,  
		[DatabaseUserName] = princ.[name],       
		[Role] = null,      
		[PermissionType] = perm.[permission_name],       
		[PermissionState] = perm.[state_desc],       
		[ObjectType] = CASE perm.[class] 
							WHEN 1 THEN obj.type_desc               -- Schema-contained objects
							ELSE perm.[class_desc]                  -- Higher-level objects
					   END,       
		[ObjectName] = CASE perm.[class] 
							WHEN 1 THEN OBJECT_NAME(perm.major_id)  -- General objects
							WHEN 3 THEN schem.[name]                -- Schemas
							WHEN 4 THEN imp.[name]                  -- Impersonations
					   END,
		[ColumnName] = col.[name]
	FROM    
		--database user
		sys.database_principals princ  
	LEFT JOIN
		--Login accounts
		sys.server_principals ulogin on princ.[sid] = ulogin.[sid]
	LEFT JOIN        
		--Permissions
		sys.database_permissions perm ON perm.[grantee_principal_id] = princ.[principal_id]
	LEFT JOIN
		--Table columns
		sys.columns col ON col.[object_id] = perm.major_id 
						AND col.[column_id] = perm.[minor_id]
	LEFT JOIN
		sys.objects obj ON perm.[major_id] = obj.[object_id]
	LEFT JOIN
		sys.schemas schem ON schem.[schema_id] = perm.[major_id]
	LEFT JOIN
		sys.database_principals imp ON imp.[principal_id] = perm.[major_id]
	WHERE 
		princ.[type] IN ('S','U','G') AND
		-- No need for these system accounts
		princ.[name] NOT IN ('sys', 'INFORMATION_SCHEMA')


end;



Select Coalesce(COL_LENGTH('CIM__Paths', 'id'), -1)
select OBJECT_ID('dbox.cim__paths')
