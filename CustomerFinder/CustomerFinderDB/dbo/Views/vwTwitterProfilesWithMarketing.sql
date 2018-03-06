CREATE VIEW [dbo].[vwTwitterProfilesWithMarketing]
	AS
	SELECT *, [dbo].fnGetTwitterAccountUrlByUserName(Username) as TwitterProfileUrl FROM TwitterAccount WITH (NOLOCK)
WHERE CONTAINS(ProfileDescription,'marketing')