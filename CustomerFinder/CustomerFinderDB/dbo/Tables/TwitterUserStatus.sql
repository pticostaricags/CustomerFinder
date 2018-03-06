CREATE TABLE [dbo].[TwitterUserStatus] (
    [TwitterUserStatusId] BIGINT             IDENTITY (1, 1) NOT NULL,
    [TwitterAccountsId]   BIGINT             NOT NULL,
    [StatusText]          TEXT               NOT NULL,
    [CreatedAt]           DATETIMEOFFSET (7) NOT NULL,
    [StatusUrl]           TEXT               NULL,
    [FavoriteCount]       INT                NULL,
    [RetweetCount]        INT                NULL,
    [StatusCount]         INT                NULL,
    [TweetId]             NVARCHAR (50)      NULL,
    CONSTRAINT [PK_TWITTERUSERSTATUS] PRIMARY KEY CLUSTERED ([TwitterUserStatusId] ASC),
    CONSTRAINT [FK_TwitterUserStatus_TwitterAccount] FOREIGN KEY ([TwitterAccountsId]) REFERENCES [dbo].[TwitterAccount] ([TwitterAccountsId])
);



GO

CREATE FULLTEXT INDEX ON [dbo].[TwitterUserStatus]
    ([StatusText] LANGUAGE 1033)
    KEY INDEX [PK_TWITTERUSERSTATUS]
    ON [ftCatalog];



GO

CREATE INDEX [IX_TwitterUserStatus_TweetId] ON [dbo].[TwitterUserStatus] ([TweetId])
