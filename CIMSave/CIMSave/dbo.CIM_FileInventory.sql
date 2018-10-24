CREATE TABLE [dbo].[CIM_FileInventory] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [ServerId]        INT            NOT NULL,
    [InstanceId]      INT            NOT NULL,
    [PathId]          INT            NOT NULL,
    [Name]            NVARCHAR (255) NOT NULL,
    [ACLid]           INT            NULL,
    [ACLSameAsParent] BIT            NULL,
    [Size]            BIGINT         NULL,
    [Files]           INT            NULL,
    [ErrorProcessing] NVARCHAR (255) NULL,
    CONSTRAINT [PK_dbo_CIM_Fileinventory_ID] PRIMARY KEY CLUSTERED ([Id] ASC)
);

