CREATE VIEW [dbo].[vwTwitterProfilesWithOffice]
	AS
	SELECT *, [dbo].fnGetTwitterAccountUrlByUserName(Username) as TwitterProfileUrl FROM TwitterAccount WITH (NOLOCK)
where CONTAINS(ProfileDescription,'office')
