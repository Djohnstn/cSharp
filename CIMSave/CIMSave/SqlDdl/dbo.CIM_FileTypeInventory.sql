CREATE TABLE [dbo].[CIM_FileTypeInventory] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [ServerId]      INT            NOT NULL,
    [InstanceId]    INT            NOT NULL,
    [PathId]        INT            NOT NULL,
    [Name]          NVARCHAR (255) NOT NULL,
    [FileCount]     INT            NOT NULL,
    [BytesUsedAll]  BIGINT         NOT NULL,
    [BytesSmallest] BIGINT         NULL,
    [BytesLargest]  BIGINT         NULL,
    [UtcOldest]     DATETIME2 (7)  NULL,
    [UtcNewest]     DATETIME2 (7)  NULL,
    [FileName1]     NVARCHAR (255) NULL,
    [FileName2]     NVARCHAR (255) NULL,
    CONSTRAINT [PK_dbo_CIM_FileTypeinventory_ID] PRIMARY KEY CLUSTERED ([Id] ASC)
);

