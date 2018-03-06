CREATE TABLE [dbo].[ResourceGroup]
(
	[ResourceGroupId] INT NOT NULL CONSTRAINT PK_RESOURCEGROUP PRIMARY KEY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [OwnerUserId] BIGINT NOT NULL, 
    CONSTRAINT [FK_ResourceGroup_User] FOREIGN KEY ([OwnerUserId]) REFERENCES [User]([UserId])
)

GO

CREATE UNIQUE INDEX [UQ_ResourceGroup_Name] ON [dbo].[ResourceGroup] ([Name])
