CREATE TABLE [dbo].[TwitterAccount] (
    [TwitterAccountsId]       BIGINT             IDENTITY (1, 1) NOT NULL,
    [Username]                NVARCHAR (100)     NULL,
    [ProfileDescription]      NVARCHAR (1000)    NOT NULL,
    [LastCheckedDate]         DATETIMEOFFSET (7) NULL,
    [Location]                NVARCHAR (1000)    NULL,
    [ProfileImageUrl]         NVARCHAR (1000)    NULL,
    [WebsiteUrl]              NVARCHAR (50)      NULL,
    [LastTimeTweetsProcessed] DATETIMEOFFSET (7) NULL,
    CONSTRAINT [PK_TWITTERACCOUNT] PRIMARY KEY CLUSTERED ([TwitterAccountsId] ASC)
);



GO

CREATE UNIQUE INDEX [IX_TwitterAccounts_Username] ON [dbo].[TwitterAccount] ([Username])

GO

CREATE FULLTEXT INDEX ON [dbo].[TwitterAccount]
    ([ProfileDescription] LANGUAGE 1033)
    KEY INDEX [PK_TWITTERACCOUNT]
    ON [ftCatalog];


