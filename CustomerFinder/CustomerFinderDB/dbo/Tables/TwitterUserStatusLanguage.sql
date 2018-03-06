CREATE TABLE [dbo].[TwitterUserStatusLanguage]
(
	[TwitterUserStatusLanguageId] BIGINT NOT NULL CONSTRAINT PK_TWITTERUSERSTATUSLANGUAGE PRIMARY KEY IDENTITY, 
    [TwitterUserStatusId] BIGINT NOT NULL, 
    [Name] NVARCHAR(250) NOT NULL, 
    [iso6391Name] NVARCHAR(250) NOT NULL, 
    [score] FLOAT NOT NULL, 
    CONSTRAINT [FK_TwitterUserStatusLanguage_TwitterUserStatus] FOREIGN KEY ([TwitterUserStatusId]) REFERENCES [TwitterUserStatus]([TwitterUserStatusId])
)
