CREATE TABLE [dbo].[DomainInfo] (
    [DomainInfoId] INT NOT NULL IDENTITY,
	[Domain] NVARCHAR(100) NOT NULL,
    [ContactName] NVARCHAR(100) NULL, 
    [ContactEmailAddress] NVARCHAR(100) NOT NULL, 
	[CompanyCountry] NVARCHAR(100)
    CONSTRAINT [PK_DomainInfo] PRIMARY KEY CLUSTERED ([DomainInfoId] ASC), 
    [Website] NVARCHAR(100) NOT NULL, 
    CONSTRAINT [UK_DomainInfo_Domain] UNIQUE ([Domain])
);

