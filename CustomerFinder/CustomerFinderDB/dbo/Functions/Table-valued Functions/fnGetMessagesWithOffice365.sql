CREATE FUNCTION [dbo].[fnGetMessagesWithOffice365]
(
	@twitterUserNetwork nvarchar(100)
)
RETURNS TABLE
AS
RETURN
(
	SELECT TA.*, 
	[dbo].fnGetTwitterAccountUrlByUserName(Username) as TwitterProfileUrl,
	TUS.STATUSTEXT,
	TUS.CREATEDAT,
	TUS.STATUSURL
	FROM TwitterUserStatus TUS
	INNER JOIN TWITTERACCOUNT TA ON TA.TwitterAccountsId = TUS.TwitterAccountsId
	WHERE CONTAINS(TUS.StatusText,'"#Office365" OR "Office365" OR "Office 365"')
	AND TA.TwitterAccountsId IN 
		(
			SELECT * FROM [dbo].[fnGetTwitterFirstLevelFollowersIds] (@twitterUserNetwork) as FollowersId
			UNION
			SELECT * FROM [dbo].[fnGetTwitterSecondLevelFollowersIds] (@twitterUserNetwork) as FollowersId
		)
)