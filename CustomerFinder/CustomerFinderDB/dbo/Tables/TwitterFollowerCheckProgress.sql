CREATE TABLE [dbo].[TwitterFollowerCheckProgress]
(
	[TwitterFollowerCheckProgressId] BIGINT NOT NULL CONSTRAINT PK_TWITTERFOLLOWERCHECKPROGRESS PRIMARY KEY IDENTITY, 
    [TwitterAccountsId] BIGINT NOT NULL, 
    [NextCursor] BIGINT NOT NULL, 
    CONSTRAINT [FK_TwitterFollowerCheckProgress_TwitterAccount] FOREIGN KEY ([TwitterAccountsId]) REFERENCES [TwitterAccount]([TwitterAccountsId])
)
