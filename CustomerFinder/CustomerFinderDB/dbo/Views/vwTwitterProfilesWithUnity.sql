CREATE VIEW [dbo].[vwTwitterProfilesWithUnity]
	AS
	SELECT *, [dbo].fnGetTwitterAccountUrlByUserName(Username) as TwitterProfileUrl 
	FROM TwitterAccount WITH (NOLOCK) 
	WHERE CONTAINS(ProfileDescription,'("UNITY" AND "GAME") OR "UNITY%3D"')