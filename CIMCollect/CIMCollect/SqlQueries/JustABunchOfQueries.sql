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
