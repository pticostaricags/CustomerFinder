CREATE VIEW [dbo].[vwTwitterProfilesSentiment]
	AS 
	Select TA.TwitterAccountsId, TA.Username,TA.ProfileImageUrl, 
	count(TUS.TwitterUserStatusId) as TotalMessages, AVG(TUSS.score) as AverageSentiment 
	From TwitterUserStatusSentiment TUSS with (nolock)
INNER JOIN TwitterUserStatus TUS with (nolock) ON TUSS.TwitterUserStatusId = TUS.TwitterUserStatusId
INNER JOIN TwitterAccount TA with (nolock) on TA.TwitterAccountsId = TUS.TwitterAccountsId
where ta.profileimageurl is not null
group by TA.TwitterAccountsId, TA.Username, TA.ProfileImageUrl
