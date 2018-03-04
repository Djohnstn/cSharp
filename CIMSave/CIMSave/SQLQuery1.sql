select * 
from sys.tables t
inner join sys.schemas s on s.schema_id = t.schema_id 
where t.type = 'U' and t.name = 'Process' and s.name = 'dbo'

--Select schema_id from sys.schemas where name = 'dbo'

select c.name, c.max_length, ty.name as typeName --, ty.length --, c.*, ty.*, c.column_id, 
from sys.columns c 
inner join sys.tables t on t.object_id = c.object_id
inner join sys.schemas s on s.schema_id = t.schema_id 
inner join sys.systypes ty on ty.xusertype = c.user_type_id
where  t.name = 'Process' and s.name = 'dbo' and c.name = 'Name'
--t.type = 'U' and

--where c.
Select *
from sys.systypes

select b.* from dbo.bios b

