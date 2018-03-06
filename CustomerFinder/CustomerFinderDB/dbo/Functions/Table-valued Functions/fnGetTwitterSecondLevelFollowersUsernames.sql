CREATE FUNCTION [dbo].[fnGetTwitterSecondLevelFollowersUsernames]
(
	@username nvarchar(100)
)
RETURNS @returntable TABLE
(
	username nvarchar(100)
)
AS
BEGIN
	INSERT @returntable
	SELECT DISTINCT F2.UserFollowing F2 FROM Followers F2 
		WHERE F2.UserFollowed IN 
		(SELECT F1.UserFollowing FROM FOLLOWERS F1 WHERE F1.UserFollowed = @username)
	RETURN
END
