CREATE VIEW [dbo].[vwTwitterProfilesWithCEO]
	AS
		SELECT *, [dbo].fnGetTwitterAccountUrlByUserName(Username) as TwitterProfileUrl FROM TwitterAccount WITH (NOLOCK)
WHERE 
--ProfileDescription COLLATE Latin1_General_CS_AS LIKE '%CEO%' or ProfileDescription COLLATE Latin1_General_CS_AS like '%GERENT%'
CONTAINS(ProfileDescription, ' "CEO" OR "GERENT"')
