CREATE VIEW [dbo].[vwTwitterProfilesForCoaching]
	AS
	SELECT *, [dbo].fnGetTwitterAccountUrlByUserName(Username) as TwitterProfileUrl FROM TwitterAccount WITH (NOLOCK) 
	WHERE ProfileDescription LIKE '%recurso%humano%' OR ProfileDescription like '%gerent%' 
	OR ProfileDescription LIKE '%DUEÑ%' OR ProfileDescription like '%#RRHH%' OR ProfileDescription like '%RRHH%'