CREATE FUNCTION [dbo].[fnGetTwitterProfilesWithHumanResources]
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
	AND CONTAINS(ProfileDescription,'"human resource" or "recursos humanos" or "R.H." or "RH" or "HR" or "recurso humano"')
)