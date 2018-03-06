CREATE TABLE [dbo].[User]
(
	[UserId] BIGINT NOT NULL CONSTRAINT PK_USER PRIMARY KEY IDENTITY, 
    [UserPrincipalName] NVARCHAR(50) NOT NULL, 
    [Company] NVARCHAR(50) NOT NULL
)

GO

CREATE UNIQUE INDEX [UQ_User_UserPrincipalName] ON [dbo].[User] ([UserPrincipalName])
