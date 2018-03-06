/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

ALTER FULLTEXT INDEX ON [dbo].[TwitterAccount] ENABLE; 
ALTER FULLTEXT INDEX ON [dbo].[TwitterUserStatus] ENABLE; 

ALTER FULLTEXT INDEX ON [dbo].[TwitterAccount] START FULL POPULATION;
ALTER FULLTEXT INDEX ON [dbo].[TwitterUserStatus] START FULL POPULATION;