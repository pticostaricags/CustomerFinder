CREATE VIEW [dbo].[vwTwitterProfilesInCentralAmerica]
	AS 
SELECT *, [dbo].fnGetTwitterAccountUrlByUserName(Username) as TwitterProfileUrl 
FROM TwitterAccount WITH (NOLOCK) WHERE 
LOCATION LIKE '%LATAM%' OR 
LOCATION LIKE '%América Latina%' OR 
LOCATION LIKE 'Latin%America%' OR
LOCATION LIKE '%Nicaragua%' OR 
LOCATION LIKE '%Honduras%' OR 
LOCATION LIKE '%Costa Rica%' OR 
LOCATION LIKE '%Guatemala%' OR 
LOCATION LIKE '%Salvador%' OR 
LOCATION LIKE '%Panamá%' OR 
LOCATION LIKE '%Panama%' OR 
LOCATION LIKE '%Belice%' OR 
LOCATION LIKE '%Belize%' OR
LOCATION LIKE '%Ibero%America%' OR
LOCATION LIKE '%Ibero%América%' OR
LOCATION LIKE '%Iberian%America%'