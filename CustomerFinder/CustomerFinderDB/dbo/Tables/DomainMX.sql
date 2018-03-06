CREATE TABLE [dbo].[DomainMX]
(
	[DomainMXId] BIGINT NOT NULL CONSTRAINT PK_DomainMXId PRIMARY KEY IDENTITY, 
	[DomainInfoId] int NOT NULL, 
    [MXRecord] NVARCHAR(100) NOT NULL, 
    CONSTRAINT [FK_DomainMXs_DomainInfo] FOREIGN KEY ([DomainInfoId]) REFERENCES [DomainInfo]([DomainInfoId])
)
