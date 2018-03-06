CREATE FUNCTION [dbo].[fnGetTwitterFirstLevelFollowers]
(
	@username NVARCHAR(100)
)
RETURNS TABLE
AS
RETURN
(
	SELECT TA.*, [dbo].fnGetTwitterAccountUrlByUserName(Username) as TwitterProfileUrl FROM TwitterAccount TA
	INNER JOIN dbo.fnGetTwitterFirstLevelFollowersIds(@username) TFLF ON TFLF.TwitterAccountsId = TA.TwitterAccountsId
)