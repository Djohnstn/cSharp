CREATE Procedure dbo.SelectOrInsert_ACL 
		@tvp dbo.TVP_INT READONLY,
		@hash varbinary(250),
		@test int = 0,
		@tag nvarchar(30) = ''
as
begin
	set nocount on;
	
	-- [dbo].[CIM_ACL_Entry] id, hash			ACL to ACE hash finder
	-- [dbo].[CIM_ACL_ACE] id, ACLID, ACEID		ACE to ACL details finder
	--Select * from @tvp;
	Declare @tvpCount int = (Select Count(*) from @tvp);
	--Select @tvpCount as TVP_COUNT
	Declare @tmp table(id int);

	---
	-- from original test sql code
	Declare @firstResult int;
	With Candidates as (
		Select s0.id from dbo.CIM_ACL_Entry as s0  Where s0.[hash] = @hash
	),
	SetDiscovery as (
		Select s1.ACLID	-- candidate sets that have the required parts
			from dbo.CIM_ACL_ACE as s1
			inner join Candidates c0 on c0.id = s1.ACLID
			inner join @tvp as n1 on n1.id = s1.ACEID
			group by s1.ACLID
			having count(s1.ACEID) = @tvpCount -- (Select count(*) from @tvp)
			   and Count(n1.id)  = @tvpCount --(Select count(*) from @tvp)
		Except
		Select  s2.ACLID  -- sets with parts not in new group -- this will be a huge list
			from dbo.CIM_ACL_ACE as s2
			inner join Candidates c0 on c0.Id = s2.ACLID
			where Not Exists (Select 1 from @tvp n2 Where n2.id = s2.ACEID)
	)
	Select @firstResult = s.ACLID from SetDiscovery s;
	--Select @firstResult as 'First Result'
	----
	--Declare @firstAclResult int = -1;

	-- duplicate hashs are possible, it's ok, but we start with them
--;	With Candidates as (
--		Select s0.id from dbo.CIM_ACL_Entry as s0  Where s0.[hash] = @hash
--	),
--	SetDiscovery as (
--		Select s1.ACLID 
--		from dbo.CIM_ACL_ACE as s1 
--		inner join Candidates c0 on c0.id = s1.ACLID
--		inner join @tvp as n1 on n1.id = s1.ACEID
--		Group by s1.ACLID
--		Having Count(s1.ACEID) = @tvpCount
--		   and Count(n1.id) = @tvpCount
--		Except -- sets with parts not in new group -- this may or will be a huge list
--		Select s2.ACLID from dbo.CIM_ACL_ACE as s2 
--			inner join Candidates c0 on c0.id = s2.ACLID
--			where Not Exists (Select 1 from @tvp n2 Where n2.id = s2.ACEID)
--	)
--	Insert Into @tmp (id)
--	Select s.ACLID from SetDiscovery s;

	--Select * from @tmp;
	--Declare @found int;
	--Select @found = Count(*) from @tmp;

	if (@firstResult = 0 or @firstResult is null) and @test = 0
	begin
		--Select ' need a new acl', @firstResult
		Begin Transaction [NewACL]
		DECLARE @OutputTbl TABLE (AclID INT)
		Insert Into dbo.CIM_ACL_Entry ([Hash]) 
			OUTPUT INSERTED.ID INTO @OutputTbl(AclID)
			Values (@hash);
		Declare @aclid int;
		Select @aclid = AclID from @OutputTbl;
		--Declare @AclAce Table(acl int, ace int);
		Insert into dbo.CIM_ACL_ACE(aclid, aceid)
		Select @aclid, id
			From @tvp
		--Select * from dbo.CIM_ACL_ACE

		Select @aclid as [NewACLID], 'New' as [Status], @tag as [Tag]
		Commit Transaction [NewACL]
	end
	else 
	begin
		Select @firstResult as [FoundAclID], 'Old' as [Status], @tag as [Tag]
	end

end