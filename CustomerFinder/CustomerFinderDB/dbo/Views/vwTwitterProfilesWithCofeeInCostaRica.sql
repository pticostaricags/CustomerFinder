CREATE VIEW [dbo].[vwTwitterProfilesWithCofeeInCostaRica]
	AS
	SELECT TA.TwitterAccountsId, TA.ProfileDescription, TA.[Location], 
	[dbo].fnGetTwitterAccountUrlByUserName(TA.Username) as TwitterProfileUrl,
TUS.StatusText, TUS.StatusUrl, TUS.CreatedAt as StatusCreatedAt
 FROM vwTwitterProfilesInCostaRica TPCR
INNER JOIN TwitterAccount TA WITH (NOLOCK) ON TA.TwitterAccountsId = TPCR.TwitterAccountsId AND 
CONTAINS(TA.ProfileDescription, 'COFFEE OR CAFE OR @starbucks_cr OR starbucks OR "bebidas de temporada" OR latte')
INNER JOIN TwitterUserStatus TUS WITH (NOLOCK) ON TPCR.TwitterAccountsId = TUS.TwitterAccountsId AND 
CONTAINS(TUS.StatusText,'COFFEE OR CAFE OR @starbucks_cr OR starbucks OR "bebidas de temporada" OR latte')