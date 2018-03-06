CREATE VIEW [dbo].[vwTwitterProfilesWithCEOandGames]
	AS
	SELECT *, [dbo].fnGetTwitterAccountUrlByUserName(Username) as TwitterProfileUrl 
	FROM TwitterAccount TA WHERE CONTAINS(TA.ProfileDescription, '"CEO" AND "GAMES"')
