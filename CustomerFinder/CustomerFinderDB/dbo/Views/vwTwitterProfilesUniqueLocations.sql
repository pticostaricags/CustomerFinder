CREATE VIEW [dbo].[vwTwitterProfilesUniqueLocations]
	AS 
	SELECT DISTINCT [Location] FROM [TwitterAccount] WITH (NOLOCK)
