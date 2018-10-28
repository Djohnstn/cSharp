CREATE TABLE [dbo].[CIM_ACE_Principal] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [PrincipalName] NVARCHAR (100) NOT NULL,
    [SID]           NVARCHAR (120) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [ixPrincipalNameSID]
    ON [dbo].[CIM_ACE_Principal]([PrincipalName] ASC, [SID] ASC);

