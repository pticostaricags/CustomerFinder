CREATE FUNCTION [dbo].[fnGetTwitterProfilesWithCEOAndGames]
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
	INNER JOIN Followers F WITH (NOLOCK) ON F.UserFollowing = TA.Username 
	WHERE CONTAINS(TA.ProfileDescription, '"CEO" AND "GAMES"')
	AND F.UserFollowed = @twitterUserNetwork
)
