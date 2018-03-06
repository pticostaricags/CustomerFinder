CREATE VIEW [dbo].[vwTwitterProfilesByKeyword]
	AS
SELECT TwitterAccountsId, Username, 'Education' as Keyword, ProfileDescription AS ProfileDescription, [dbo].fnGetTwitterAccountUrlByUserName(Username) as TwitterProfileUrl FROM TwitterAccount WITH (NOLOCK) WHERE CONTAINS(ProfileDescription,'EDUCATION')
UNION SELECT TwitterAccountsId, Username, 'Games' as Keyword, ProfileDescription AS ProfileDescription, [dbo].fnGetTwitterAccountUrlByUserName(Username) as TwitterProfileUrl FROM TwitterAccount WITH (NOLOCK) WHERE CONTAINS(ProfileDescription,'GAMES')
UNION SELECT TwitterAccountsId, Username, 'Marketing' as Keyword, ProfileDescription AS ProfileDescription, [dbo].fnGetTwitterAccountUrlByUserName(Username) as TwitterProfileUrl FROM TwitterAccount WITH (NOLOCK) WHERE CONTAINS(ProfileDescription,'MARKETING')
UNION  SELECT TwitterAccountsId, Username, 'Office' as Keyword, ProfileDescription AS ProfileDescription, [dbo].fnGetTwitterAccountUrlByUserName(Username) as TwitterProfileUrl FROM TwitterAccount WITH (NOLOCK) WHERE CONTAINS(ProfileDescription,'OFFICE')
GO

