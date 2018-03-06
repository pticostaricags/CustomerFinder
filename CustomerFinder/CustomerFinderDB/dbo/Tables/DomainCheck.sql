CREATE TABLE [dbo].[DomainLookup]
(
	[DomainCheckId] INT NOT NULL CONSTRAINT [PK_DomainCheckId] PRIMARY KEY IDENTITY, 
	[DomainId] INT NOT NULL,
    [CheckedDateTime] DATETIMEOFFSET NOT NULL, 
    [ResponseMessage] NVARCHAR(100) NOT NULL, 
    [ResponseCode] NVARCHAR(100) NULL, 
    CONSTRAINT [FK_DomainCheck_DomainInfo] FOREIGN KEY ([DomainId]) REFERENCES [DomainInfo]([DomainInfoId])
)
