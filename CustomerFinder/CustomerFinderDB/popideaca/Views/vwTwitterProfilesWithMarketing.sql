CREATE VIEW [popidea].[vwTwitterProfilesWithMarketing]
	AS
	SELECT TA.*, [dbo].fnGetTwitterAccountUrlByUserName(Username) as TwitterProfileUrl FROM TwitterAccount TA WITH (NOLOCK)
	WHERE TA.TwitterAccountsId IN 
	(
		SELECT * FROM [dbo].[fnGetTwitterFirstLevelFollowersIds] (@username) as FollowersId
		UNION
		SELECT * FROM [dbo].[fnGetTwitterSecondLevelFollowersIds] (@username) as FollowersId
	)
	AND CONTAINS(ProfileDescription,'marketing or mercadeo')