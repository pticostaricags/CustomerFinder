﻿CREATE TABLE [dbo].[ApiLimit]
(
	[ApiLimitId] BIGINT NOT NULL CONSTRAINT PK_APILIMIT PRIMARY KEY IDENTITY, 
    [ApiName] NVARCHAR(250) NOT NULL, 
    [LimitFoundAt] DATETIMEOFFSET NULL
)
