CREATE FUNCTION [dbo].[fnGetTwitterProfilesForMicrosoftInEducation]
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
	WHERE CONTAINS(TA.ProfileDescription, '"MIEEXPERT" OR "MICROSOFT%#EDUCATION" OR "MICROSOFT%TEACHER" OR "MICROSOFT%EDUCATOR" OR "#MSFTEDU" OR "#MIEE"')
	AND F.UserFollowed = @twitterUserNetwork
)
