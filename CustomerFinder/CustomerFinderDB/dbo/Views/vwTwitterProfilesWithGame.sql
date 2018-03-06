CREATE VIEW [dbo].[vwTwitterProfilesWithGame]
	AS
	SELECT *, [dbo].fnGetTwitterAccountUrlByUserName(Username) as TwitterProfileUrl FROM TwitterAccount WITH (NOLOCK)
where CONTAINS(ProfileDescription,'GAME')
