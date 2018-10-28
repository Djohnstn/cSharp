CREATE TABLE [dbo].[CIM_ACE_Rights] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Rights]      INT           NOT NULL,
    [Description] NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [ixACERights]
    ON [dbo].[CIM_ACE_Rights]([Rights] ASC);

