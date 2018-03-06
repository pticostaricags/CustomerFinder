CREATE FUNCTION [dbo].[fnGetTwitterFirstLevelFollowersIds]
(
	@username nvarchar(100)
)
RETURNS @returntable TABLE
(
	TwitterAccountsId nvarchar(100)
)
AS
BEGIN
	INSERT @returntable
	SELECT TA.TwitterAccountsId FROM FOLLOWERS F
	INNER JOIN TWITTERACCOUNT TA ON TA.Username = F.UserFollowing
	WHERE F.UserFollowed=@username
	RETURN
END
