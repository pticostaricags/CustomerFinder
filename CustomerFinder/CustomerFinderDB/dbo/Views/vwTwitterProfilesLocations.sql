CREATE VIEW [dbo].[vwTwitterProfilesLocations]
	AS 
	SELECT TwitterAccountsId, 'http://www.twitter.com/' + Username as ProfileUrl,[ProfileDescription], [Location] FROM [TwitterAccount] WITH (NOLOCK)
