CREATE FUNCTION [dbo].[fnGetNetworkTwitterUserStatus]
(
	@username NVARCHAR(100)
)
RETURNS TABLE
AS
RETURN
(
	SELECT TUS.*, TA.Username FROM TwitterUserStatus TUS WITH (NOLOCK)
		INNER JOIN TwitterAccount TA WITH (NOLOCK) on TUS.TwitterAccountsId = TA.TwitterAccountsId
		AND TA.TwitterAccountsId IN 
		(
			SELECT * FROM [dbo].[fnGetTwitterFirstLevelFollowersIds] (@username) as FollowersId
			UNION
			SELECT * FROM [dbo].[fnGetTwitterSecondLevelFollowersIds] (@username) as FollowersId
		)
)
