--drop function dbo.CalculateSimilarity
--Select OBJECT_ID('dbo.CalculateSimilarity');
go
IF OBJECT_ID('dbo.CalculateSimilarityFromAbstract') IS NULL
	EXEC('CREATE FUNCTION dbo.CalculateSimilarityFromAbstract() RETURNS FLOAT BEGIN RETURN 1.0 END')
go
--IF OBJECT_ID('mySchema.myProc') IS NULL
--EXEC('CREATE FUNCTION dbo.CalculateSimilarity() AS RETURN 1;')
--GO

--Drop Function dbo.CalculateSimilarity ;
--go
Set nocount on
DECLARE @t1 DATETIME;	-- millisecond performance timers
DECLARE @t2 DATETIME;
Set @t1 = GETDATE();

GO
Alter Function dbo.CalculateSimilarityFromAbstract (
	@CompareThis nvarchar(4000),  
	@CompareThat nvarchar(4000))
	Returns Float (24)
As
Begin
	Declare @PercentMatch Float(24);

	Declare @Abstracts table(id int, csv varchar(2500));
	-- case sensitive collation sequence used on purpose
	Declare @table1 table(id int, 
							partName varchar(80) COLLATE Latin1_General_CS_AS, -- INDEX IX1 CLUSTERED, 
							partCount int, 
							INDEX IX3 CLUSTERED(partName, id));
	-- create table of inputs
	Insert into @Abstracts (id, csv)
	Select 1, @CompareThis
	Union
	Select 2, @CompareThat;
	-- turn CSV into multiple lines via csv to xml to cross apply, as of SQL 2017, can use STRING_SPLIT function instead (finally!)
	;With T1 as (
		Select ab.id, cast('<r><d>' + replace(ab.csv, ',', '</d><d>') + '</d></r>' as xml) as x
		From @Abstracts ab 
	),
	tokens as (
		Select t1.id, m.n.value('.[1]','varchar(80)') as token
		from t1
		cross apply x.nodes('/r/d')m(n)
	)
	Insert into @table1 (id, partName, partCount) 
	Select id, ParseName(t1.token,2) as partName, 
			iif(IsNumeric(ParseName(t1.token,1))=1, Cast(ParseName(t1.token,1) as Int), 0) as partCount From tokens as t1;

	Declare @TotalParts Float;
	Declare @TotalDiffs Float;

	-- summarize each artifact into total and differences
	-- TRY 2
	With Summary as (
		Select t1.partname, 
				Max(t1.partCount) as tCountMax, 
				Min(t1.partcount) as tCountMin,
				Count(*) as tCountFound
		from @table1 t1
		Group by t1.partName
	),
	Summary2 as (
		Select s1.partName, s1.tCountMax, Iif(s1.tCountFound = 1, s1.tCountMax, tCountMax - tCountMin) as tDelta
		From Summary as s1
	)
	--Select * from Summary2;
	Select @TotalParts = Cast(Sum(tCountMax) as Float), @TotalDiffs = Cast(Sum(tDelta) as Float)
	from Summary2;
	-- END TRY 2


	-- zero parts guard, return null!
	Set @PercentMatch = iif(@Totalparts<1, Null, (@TotalParts - @TotalDiffs) / @TotalParts * 100.0);
	--Select @PercentMatch as PercentMatch, @TotalDiffs as PartsDifferences, @TotalParts as TotalParts;
	Return @PercentMatch;

End
GO
--Set @t2 = GETDATE();
--Select DATEDIFF(millisecond,@t1,@t2) AS elapsed_ms_Test1

GO
DECLARE @t1 DATETIME;	-- millisecond performance timers
DECLARE @t2 DATETIME;
Set @t1 = GETDATE();
Select dbo.CalculateSimilarityFromAbstract( '!abcxwwwz.1,def.3,ghijk.40,lmnop.1,QAS.1,aaaz.1,bbbbbb.2', 
											'!abcxwwwz.1,def.3,ghijk.40,lmnop.1,QAS.2,aaaz.1' ) as Similarity
Set @t2 = GETDATE();
Select DATEDIFF(millisecond,@t1,@t2) AS elapsed_ms_Similarity

Set @t1 = GETDATE();
Select dbo.CalculateSimilarityFromAbstract('abc.1,cde.1', 'zz.0,efg.1,hij.1,abc.1') as Similarity
Set @t2 = GETDATE();
Select DATEDIFF(millisecond,@t1,@t2) AS elapsed_ms_Similarity

Select dbo.CalculateSimilarityFromAbstract('abc.1,cde.1', 'abc.1,cde.1') as Similarity_Test100pct
Select dbo.CalculateSimilarityFromAbstract('!abcz.1,abc.1,cde.1', '!abcz.1,abc.1,cde.1') as Similarity_Test100pct2
Select dbo.CalculateSimilarityFromAbstract('abc.1,cde.c', 'abc.1,cde.1') as Similarity_Test3Baddata1
Select dbo.CalculateSimilarityFromAbstract('abc.1,cde.1', 'abc.1;efg.1;hij.1;abc.1') as Similarity_Test4BadData2
Select dbo.CalculateSimilarityFromAbstract('', null) as Similarity_Test5BadDataEmptynull
Select dbo.CalculateSimilarityFromAbstract(Null, null) as Similarity_Test6BadDataEmptynull
Select dbo.CalculateSimilarityFromAbstract(Null, 'abc.1') as Similarity_Test7BadDataNullValid
Select dbo.CalculateSimilarityFromAbstract('.2', 'abc') as Similarity_Test8BadDataValidBad
Select dbo.CalculateSimilarityFromAbstract(',', ',') as Similarity_Test9BadDataEmptyEmpty

