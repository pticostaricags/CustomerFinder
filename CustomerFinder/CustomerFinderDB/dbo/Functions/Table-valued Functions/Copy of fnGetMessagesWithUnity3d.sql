CREATE FUNCTION [dbo].[fnGetMessagesWithUnity3d]
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
	INNER JOIN TWITTERACCOUNT TA ON TA.TwitterAccountsId = TUS.TWITTERACCOUNTSID
	WHERE CONTAINS(TUS.StatusText,'("UNITY" AND "GAME") OR "UNITY%3D"')
	AND TA.TwitterAccountsId IN 
		(
			SELECT * FROM [dbo].[fnGetTwitterFirstLevelFollowersIds] (@twitterUserNetwork) as FollowersId
			UNION
			SELECT * FROM [dbo].[fnGetTwitterSecondLevelFollowersIds] (@twitterUserNetwork) as FollowersId
		)
)