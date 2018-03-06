CREATE VIEW [dbo].[vwTwitterProfilesWithEntrepreneur]
AS
	SELECT *, [dbo].fnGetTwitterAccountUrlByUserName(Username) as TwitterProfileUrl FROM TwitterAccount WITH (NOLOCK)
WHERE CONTAINS(ProfileDescription,'"entrepreneur" OR "emprendedor"')