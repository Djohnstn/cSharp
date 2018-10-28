CREATE TABLE [dbo].[CIM_ACL_Entry] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Hash] VARBINARY (65) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [ixCIMACLEntry]
    ON [dbo].[CIM_ACL_Entry]([Hash] ASC);

