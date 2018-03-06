CREATE TABLE [dbo].[TwitterMultiClientQueue]
(
	[TwitterMultiClientQueueId] BIGINT NOT NULL CONSTRAINT OK_TWITTERMULTICLIENTQUEUE PRIMARY KEY IDENTITY, 
    [TwitterUsername] NVARCHAR(100) NOT NULL, 
    [Enabled] BIT NOT NULL
)

GO

CREATE UNIQUE INDEX [IX_TwitterMultiClientQueue_TwitterUsername] ON [dbo].[TwitterMultiClientQueue] ([TwitterUsername])
