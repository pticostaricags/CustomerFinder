CREATE TABLE [dbo].[TwitterUserStatusKeyPhrase]
(
	[TwitterUserStatusKeyPhraseId] BIGINT NOT NULL CONSTRAINT PK_TWITTERUSERSTATUSKEYPHRASE PRIMARY KEY IDENTITY, 
    [TwitterUserStatusId] BIGINT NOT NULL, 
    [KeyPhrase] NVARCHAR(100) NOT NULL, 
    CONSTRAINT [FK_TwitterUserStatusKeyPhrase_TwitterUserStatus] FOREIGN KEY ([TwitterUserStatusId]) REFERENCES [TwitterUserStatus]([TwitterUserStatusId])
)
