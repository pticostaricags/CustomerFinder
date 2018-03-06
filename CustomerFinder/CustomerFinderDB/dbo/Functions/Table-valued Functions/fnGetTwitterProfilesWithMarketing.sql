CREATE FUNCTION [dbo].[fnGetTwitterProfilesWithMarketing]
(
	@username NVARCHAR(100)
)
RETURNS TABLE
AS
RETURN
(
	SELECT TA.*, [dbo].fnGetTwitterAccountUrlByUserName(Username) as TwitterProfileUrl FROM TwitterAccount TA WITH (NOLOCK)
	WHERE TA.TwitterAccountsId IN 
	(
		SELECT * FROM [dbo].[fnGetTwitterFirstLevelFollowersIds] (@username) as FollowersId
		UNION
		SELECT * FROM [dbo].[fnGetTwitterSecondLevelFollowersIds] (@username) as FollowersId
	)
	AND CONTAINS(ProfileDescription,'marketing or mercadeo')
)