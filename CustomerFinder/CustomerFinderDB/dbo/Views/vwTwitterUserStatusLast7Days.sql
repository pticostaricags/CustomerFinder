CREATE VIEW [dbo].[vwTwitterUserStatusLast7Days]
	AS SELECT TUS.*, TUSS.score * 100 as Sentiment FROM TwitterUserStatus TUS
		INNER JOIN TwitterUserStatusSentiment TUSS ON TUSS.TwitterUserStatusId = TUS.TwitterUserStatusId
		WHERE TUS.CreatedAt >= DATEADD(DAY, -7, GETUTCDATE())
