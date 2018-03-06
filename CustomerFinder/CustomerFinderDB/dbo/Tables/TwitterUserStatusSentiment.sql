CREATE TABLE [dbo].[TwitterUserStatusSentiment]
(
	[TwitterUserStatusSentimentId] BIGINT NOT NULL CONSTRAINT PKTWITTERUSERSTATUSSENTIMENT PRIMARY KEY IDENTITY, 
    [TwitterUserStatusId] BIGINT NOT NULL, 
    [score] FLOAT NOT NULL, 
    CONSTRAINT [FK_TwitterUserStatusSentiment_TwitterUserStatus] FOREIGN KEY ([TwitterUserStatusId]) REFERENCES [TwitterUserStatus]([TwitterUserStatusId])
)
