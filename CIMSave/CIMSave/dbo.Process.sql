CREATE TABLE [dbo].[Process]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[ServerId] int not null ,
	[Name] nvarchar(32) not null,
	[ExePath] nvarchar(90) null
)

GO

CREATE INDEX [IX_Process_ServerID_Name] ON [dbo].[Process] ([ServerId], [Name])
