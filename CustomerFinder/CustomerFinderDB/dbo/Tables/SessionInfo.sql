CREATE TABLE [dbo].[SessionInfo]
(
	[SessionInfoId] BIGINT NOT NULL CONSTRAINT [PK_SESSIONINFO] PRIMARY KEY IDENTITY, 
    [SessionId] UNIQUEIDENTIFIER NOT NULL, 
    [StartDateTime] DATETIMEOFFSET NOT NULL, 
    [EndDateTime] DATETIMEOFFSET NULL, 
    [IsWaitingTwitterLimit] BIT NULL, 
    [LastTimeWaiting] DATETIMEOFFSET NULL, 
    [UserBeingProcessed] NVARCHAR(50) NULL, 
    [MillisecondsToWait] INT NULL, 
    [CustomerTwitterUsername] NVARCHAR(50) NULL
)

GO

CREATE UNIQUE INDEX [IX_SessionInfo_SessionId] ON [dbo].[SessionInfo] ([SessionId])
