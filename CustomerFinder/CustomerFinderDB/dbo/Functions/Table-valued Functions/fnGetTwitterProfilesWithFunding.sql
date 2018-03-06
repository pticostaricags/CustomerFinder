CREATE FUNCTION [dbo].[fnGetTwitterProfilesWithFunding]
(
	@twitterUserNetwork nvarchar(100)
)
RETURNS TABLE
AS
RETURN
(
	SELECT TA.*, 
	[dbo].fnGetTwitterAccountUrlByUserName(Username) as TwitterProfileUrl
	FROM TwitterAccount TA WITH (NOLOCK)
	WHERE CONTAINS(TA.ProfileDescription, '"FUNDING" OR "#FUNDING" OR "CAPITAL%FUNDING"')
	AND TA.TwitterAccountsId IN 
		(
			SELECT * FROM [dbo].[fnGetTwitterFirstLevelFollowersIds] (@twitterUserNetwork) as FollowersId
			UNION
			SELECT * FROM [dbo].[fnGetTwitterSecondLevelFollowersIds] (@twitterUserNetwork) as FollowersId
		)
)
