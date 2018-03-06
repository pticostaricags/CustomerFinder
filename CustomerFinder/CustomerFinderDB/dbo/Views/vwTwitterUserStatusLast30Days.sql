CREATE VIEW [dbo].[vwTwitterUserStatusLast30Days]
	AS SELECT TUS.*, TA.Username FROM TwitterUserStatus TUS
		INNER JOIN TwitterAccount TA on TUS.TwitterAccountsId = TA.TwitterAccountsId
		WHERE TUS.CreatedAt >= DATEADD(DAY, -30, GETUTCDATE())
