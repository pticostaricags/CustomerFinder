CREATE VIEW [dbo].[vwTwitterProfilesInCostaRica]
	AS 
	SELECT *, [dbo].fnGetTwitterAccountUrlByUserName(Username) as TwitterProfileUrl 
	FROM TwitterAccount WITH (NOLOCK) WHERE 
LOCATION LIKE '%Costa Rica%' OR 
LOCATION LIKE '%Alajuela%' OR
LOCATION LIKE '%Heredia%' OR
LOCATION LIKE '%Limón%' OR
LOCATION LIKE '%Cartago%' OR
LOCATION LIKE '%Puntarenas%' OR
LOCATION LIKE '%Guanacaste%' OR
LOCATION LIKE '%San Jose%Costa Rica%'

