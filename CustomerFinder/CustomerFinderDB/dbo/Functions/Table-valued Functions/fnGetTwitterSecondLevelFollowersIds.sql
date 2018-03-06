CREATE FUNCTION [dbo].[fnGetTwitterSecondLevelFollowersIds]
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
	SELECT DISTINCT TA.TwitterAccountsId F2 FROM FOLLOWERS F2 
	INNER JOIN TwitterAccount TA ON TA.Username = F2.UserFollowing
		WHERE F2.UserFollowed IN 
		(SELECT F1.UserFollowing FROM FOLLOWERS F1 WHERE F1.UserFollowed = @username)
	RETURN
END
