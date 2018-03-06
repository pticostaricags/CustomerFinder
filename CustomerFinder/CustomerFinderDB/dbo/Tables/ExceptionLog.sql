CREATE TABLE [dbo].[ExceptionLog]
(
	[ExceptionLogId] BIGINT NOT NULL CONSTRAINT PK_ExceptionLogId PRIMARY KEY IDENTITY, 
    [Message] TEXT NOT NULL, 
    [Exception] TEXT NOT NULL, 
    [LoggedDateTime] DATETIMEOFFSET NOT NULL, 
    [StackTrace] TEXT NOT NULL, 
    [Url] TEXT NULL,
    [ApplicationName] NVARCHAR(50) NOT NULL, 
    [ServerIP] NVARCHAR(50) NOT NULL, 
    [RequestData] TEXT NULL
)
