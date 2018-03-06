CREATE VIEW [dbo].[vwTwitterProfilesWithEducation]
	AS
SELECT *, [dbo].fnGetTwitterAccountUrlByUserName(Username) as TwitterProfileUrl FROM TwitterAccount WITH (NOLOCK)
WHERE CONTAINS(ProfileDescription,'EDUCATION')