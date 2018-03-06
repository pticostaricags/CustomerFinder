CREATE VIEW [dbo].[vwTwitterProfilesWithStartup]
	AS
SELECT *, [dbo].fnGetTwitterAccountUrlByUserName(Username) as TwitterProfileUrl FROM TwitterAccount WITH (NOLOCK) where CONTAINS(ProfileDescription,'STARTUP')
