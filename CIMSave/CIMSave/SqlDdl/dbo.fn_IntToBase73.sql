CREATE FUNCTION dbo.fn_IntToBase73 (@input BIGINT)
RETURNS VARCHAR(12)
AS BEGIN
	declare @fixthis BIGINT = @input;
	declare @size int = 73;	-- 26 + 26 + 10 + 13
	declare @basestring varchar(73) = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/-~@$*()_?[]';
	declare @outstring varchar(12) = '';
	declare @int1 INT;
	if @input = 0 set @outstring = '0';
	while @fixthis > 0
	begin
		Set @int1 = @fixthis % @size;
		Set @outstring = SUBSTRING(@Basestring, @int1+1, 1) + @outstring;
		Set @fixthis = @fixthis / @size;
	end
	RETURN @outstring;
END