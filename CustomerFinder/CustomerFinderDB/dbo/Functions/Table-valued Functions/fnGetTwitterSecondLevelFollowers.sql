CREATE FUNCTION [dbo].[fnGetTwitterSecondLevelFollowers]
(
	@username NVARCHAR(100)
)
RETURNS TABLE
AS
RETURN
(
	SELECT TA.*, [dbo].fnGetTwitterAccountUrlByUserName(Username) as TwitterProfileUrl FROM TwitterAccount TA
	INNER JOIN dbo.fnGetTwitterSecondLevelFollowersIds(@username) TFLF ON TFLF.TwitterAccountsId = TA.TwitterAccountsId
)