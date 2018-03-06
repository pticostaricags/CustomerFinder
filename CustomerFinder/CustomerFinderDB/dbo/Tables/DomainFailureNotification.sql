CREATE TABLE [dbo].[DomainFailureNotification]
(
	[DomainFailureNotificationId] BIGINT NOT NULL CONSTRAINT PK_DomainFailureNotificationId PRIMARY KEY IDENTITY, 
    [DomainInfoId] INT NOT NULL, 
    [SentDate] DATETIMEOFFSET NOT NULL, 
    [SentMessage] TEXT NOT NULL, 
    CONSTRAINT [FK_DomainFailureNotification_DomainInfo] FOREIGN KEY ([DomainInfoId]) REFERENCES [DomainInfo]([DomainInfoId])
)
