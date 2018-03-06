CREATE VIEW [dbo].[vwTwitterProfilesForSeriousGames]
	AS 
	SELECT *, [dbo].fnGetTwitterAccountUrlByUserName(Username) as TwitterProfileUrl 
	FROM TwitterAccount WITH (NOLOCK) 
	WHERE CONTAINS(ProfileDescription,'"game%change" OR "SERIOUS%GAME" OR "GAME%IMPACT"') 
