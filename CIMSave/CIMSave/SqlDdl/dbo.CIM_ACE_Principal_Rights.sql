CREATE TABLE [dbo].[CIM_ACE_Principal_Rights] (
    [Id]          INT IDENTITY (1, 1) NOT NULL,
    [PrincipalId] INT NULL,
    [RightsId]    INT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

