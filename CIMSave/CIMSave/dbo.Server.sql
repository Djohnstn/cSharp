CREATE TABLE [dbo].[Server]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [NodeName] NCHAR(25) NOT NULL, 
    [Location] NVARCHAR(50) NULL, 
    [Contact] NVARCHAR(50) NULL, 
    [IPAddress] NCHAR(15) NULL, 
    [IPv6Address] NVARCHAR(55) NULL, 
    [OSName] NVARCHAR(30) NULL, 
    [NetworkName] NVARCHAR(20) NULL, 
    [Obsolete] BIT NULL DEFAULT 0, 
    [SQLServer] BIT NULL DEFAULT 0, 
    [WebServer] BIT NULL DEFAULT 0, 
    [IIOTServer] BIT NULL DEFAULT 0
)

GO


CREATE INDEX [IX_Server_Nodename] ON [dbo].[Server] ([NodeName])
