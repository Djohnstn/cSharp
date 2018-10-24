CREATE TABLE [dbo].[CIM_FileNameInventory] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [ServerId]   INT            NOT NULL,
    [InstanceId] INT            NOT NULL,
    [PathId]     INT            NOT NULL,
    [Name]       NVARCHAR (255) NOT NULL,
    [version]    NVARCHAR (80)  NULL,
    [length]     INT            NULL,
    [modified]   DATETIME2 (7)  NULL,
    [hash]       VARCHAR (44)   NULL,
    CONSTRAINT [PK_dbo_CIM_FileNameinventory_ID] PRIMARY KEY CLUSTERED ([Id] ASC)
);

