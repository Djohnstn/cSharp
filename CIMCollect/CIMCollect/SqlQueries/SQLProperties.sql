SELECT CONVERT(sysname, SERVERPROPERTY('servername')) 
		, @@SERVERNAME
		, @@SERVICENAME
		, DB_NAME()
		, SERVERPROPERTY('Edition')
		, SERVERPROPERTY('MachineName') AS ComputerName,
		  SERVERPROPERTY('ServerName') AS InstanceName,  
		  SERVERPROPERTY('Edition') AS Edition,
		  SERVERPROPERTY('ProductVersion') AS ProductVersion,  
		  SERVERPROPERTY('ProductLevel') AS ProductLevel; 

