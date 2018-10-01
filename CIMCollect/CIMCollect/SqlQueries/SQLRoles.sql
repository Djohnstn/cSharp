SELECT roles.name AS role_name
    , roles.principal_id
    , roles.type AS role_type
    , roles.type_desc AS role_type_desc
    , roles.is_fixed_role AS role_is_fixed_role
    , memberdatabaseprincipal.name AS member_name
    , memberdatabaseprincipal.principal_id AS member_principal_id
    , memberdatabaseprincipal.type AS member_type
    , memberdatabaseprincipal.type_desc AS member_type_desc
    , memberdatabaseprincipal.is_fixed_role AS member_is_fixed_role
    , memberserverprincipal.name AS member_principal_name
    , memberserverprincipal.type_desc member_principal_type_desc
    , N'ALTER ROLE ' + QUOTENAME(roles.name) + N' ADD MEMBER ' + QUOTENAME(memberdatabaseprincipal.name) AS AddRoleMembersStatement
FROM sys.database_principals AS roles
INNER JOIN sys.database_role_members
    ON sys.database_role_members.role_principal_id = roles.principal_id
INNER JOIN sys.database_principals AS memberdatabaseprincipal
    ON memberdatabaseprincipal.principal_id = sys.database_role_members.member_principal_id
LEFT OUTER JOIN sys.server_principals AS memberserverprincipal
    ON memberserverprincipal.sid = memberdatabaseprincipal.sid
ORDER BY role_name
    , member_name