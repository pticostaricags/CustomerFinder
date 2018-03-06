CREATE FUNCTION [dbo].[fnGetTwitterProfilesWithUnity]
(
	@twitterUserNetwork nvarchar(100)
)
RETURNS TABLE
AS
RETURN
(
	SELECT TA.*, 
	[dbo].fnGetTwitterAccountUrlByUserName(Username) as TwitterProfileUrl 
	FROM TWITTERACCOUNT TA WITH (NOLOCK) 
	WHERE CONTAINS(ProfileDescription,'("UNITY" AND "GAME") OR "%UNITY%3D"')
	AND TA.TwitterAccountsId IN 
		(
			SELECT * FROM [dbo].[fnGetTwitterFirstLevelFollowersIds] (@twitterUserNetwork) as FollowersId
			UNION
			SELECT * FROM [dbo].[fnGetTwitterSecondLevelFollowersIds] (@twitterUserNetwork) as FollowersId
		)
)
