GO

Alter FUNCTION dbo.fn_RleToString (@input VARBINARY(129))
RETURNS VARCHAR(250)
AS BEGIN
	--Declare @input varbinary(129) = 0x0267656A666F66CC;
	declare @returned VARBINARY(129) = @input;
	Declare @inputLen Int = Len(@input);
	declare @oldvalue INT = 0;
	declare @thisvalue INT = 0;
	declare @sbr varchar(250) = '';
	Declare @ixr int = 2;
	Declare @Cur INT;
	Declare @BinType INT = SUBSTRING(@Returned, 1, 1);
	--Declare @Prefix varchar(8) = '(' + Convert(varchar(4), @BinType, 2) + '):';
	Declare @Prefix varchar(8) = '(' + FORMAT(@BinType,'X') + '):';
	
	If @BinType = 1 Set @Prefix = 'Int:';
	If @BinType = 2 Set @Prefix = 'Rle:';
	If @BinType = 3 Set @Prefix = 'SHAi:';
	If @BinType = 4 Set @Prefix = 'SHAr:';
	while @BinType = 1 AND @ixr <= @inputLen
	begin	-- integer array
		Set @Cur = SUBSTRING(@Returned, @ixr, 1) 
				 + SUBSTRING(@Returned, @ixr+1, 1) * 256
				 + SUBSTRING(@Returned, @ixr+2, 1) * 65536
				 + SUBSTRING(@Returned, @ixr+3, 1) * 16777216;
		Set @sbr += Cast(@cur as varchar(12));
		Set @ixr += 4;
		If @ixr < @inputLen Set @sbr += ',';
	END
	while @BinType = 2 AND @ixr <= @inputLen
	begin
		--Print @ixr;
		Set @Cur = SUBSTRING(@Returned, @ixr, 1);
		--Print @Cur;
		IF @Cur > 199
		BEGIN
			Set @thisvalue = @oldvalue + @cur - 200;
			IF @oldvalue = 0 Set @sbr += '1';
			Set @sbr += '..';
			Set @oldvalue = @thisvalue;
			Set @sbr += Cast(@oldvalue as varchar(12));
			Set @thisvalue = 0;
		END
		ELSE
		BEGIN
			IF @Cur > 100
			BEGIN
                    Set @thisvalue = @thisvalue * 100 + @cur - 100;
                    Set @oldvalue += @thisvalue;
                    IF Len(@sbr) > 0 Set @sbr += ',';
                    Set @sbr += Cast(@oldvalue as varchar(12));
                    Set @thisvalue = 0;
			END
			ELSE
			BEGIN
                    Set @thisvalue = @thisvalue * 100 + @cur;
			END
		END
		Set @ixr = @ixr + 1;
		--Print @sbr;
	END
	IF Len(@sbr) = 0 
	BEGIN
		-- other types, just get the bytes as hex
		--Set @sbr = SUBSTRING(CONVERT(VARCHAR(255), @returned, 2), 3, 990);
		Set @sbr = CONVERT(VARCHAR(255), @returned, 2);
		Set @sbr = Substring(CONVERT(VARCHAR(255), @returned, 2), 3, 999);
	END
	Declare @returnVal varchar(255) = @Prefix + @sbr;
	RETURN @returnVal;
END

GO
GO

--Declare @value1 varbinary(129) = 0x01020304; 
--Select SUBSTRING(@Value1, 1, 1)
go

select top 250 L.id
	, L.Hash
	, Len(L.Hash) - 1 as AclLength
	--, Substring(CONVERT(VARCHAR(255), L.Hash, 2), 3, 999)
	, dbo.fn_RleToString(L.Hash) as RleString
from dbo.CIM_ACL_Entry L

--SELECT FORMAT(512+255,'X')
--insert into dbo.CIM_ACL_Entry (hash) values(0x037FB9E1D1172D3B25CD376C7AB2964BA1B78362C90507C303C049E4B3950C1BA0)
--insert into dbo.CIM_ACL_Entry (hash) values(0x04120939B1C747802C4281218B06BEDA28A0C09BB15F37915B068C6A57564D8D824B3DA0E3C308BBC32C918220B956F5EB774E7CEB1BC20A13)
--Select hashbytes('SHA2_512',Convert(varbinary(512), NewID()))
--insert into dbo.CIM_ACL_Entry (hash) values(hashbytes('SHA2_512',Convert(varbinary(512), NewID())))
