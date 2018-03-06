CREATE VIEW [dbo].[vwTwitterProfilesWithFounderOrCoFounder]
AS
SELECT *, [dbo].fnGetTwitterAccountUrlByUserName(Username) as TwitterProfileUrl 
FROM TwitterAccount WITH (NOLOCK)
	WHERE CONTAINS (ProfileDescription, ' "founder" OR "co%founder" OR "creador"')