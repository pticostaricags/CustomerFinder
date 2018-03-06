CREATE FUNCTION [dbo].[fnGetMessagesWithMicrosoftInEducation]
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
	WHERE CONTAINS(TUS.StatusText,'"MIEEXPERT" OR "MICROSOFT%#EDUCATION" OR "MICROSOFT%TEACHER" OR "MICROSOFT%EDUCATOR" OR "#MSFTEDU" OR "#MIEE"')
	AND TA.TwitterAccountsId IN 
		(
			SELECT * FROM [dbo].[fnGetTwitterFirstLevelFollowersIds] (@twitterUserNetwork) as FollowersId
			UNION
			SELECT * FROM [dbo].[fnGetTwitterSecondLevelFollowersIds] (@twitterUserNetwork) as FollowersId
		)
)