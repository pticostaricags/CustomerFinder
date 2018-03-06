CREATE VIEW [dbo].[vwTwitterProfilesForMicrosoftInEducation]
	AS
	SELECT *, [dbo].fnGetTwitterAccountUrlByUserName(Username) as TwitterProfileUrl 
	FROM TwitterAccount WITH (NOLOCK) WHERE 
	CONTAINS(ProfileDescription, '"MIEEXPERT" OR "MICROSOFT%#EDUCATION" OR "MICROSOFT%TEACHER" OR "MICROSOFT%EDUCATOR"')
