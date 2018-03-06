CREATE FUNCTION [dbo].[fnGetTwitterUserStatusLast30Days]
(
	@username NVARCHAR(100)
)
RETURNS TABLE
AS
RETURN
(
	SELECT TUS.*, TA.Username FROM TwitterUserStatus TUS
		INNER JOIN TwitterAccount TA on TUS.TwitterAccountsId = TA.TwitterAccountsId
		WHERE TUS.CreatedAt >= DATEADD(DAY, -30, GETUTCDATE())
		AND TA.TwitterAccountsId IN 
		(
			SELECT * FROM [dbo].[fnGetTwitterFirstLevelFollowersIds] (@username) as FollowersId
			UNION
			SELECT * FROM [dbo].[fnGetTwitterSecondLevelFollowersIds] (@username) as FollowersId
		)
)
