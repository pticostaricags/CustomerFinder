CREATE VIEW [dbo].[vwConstantUnfollowers]
	AS
SELECT UserUnFollowed, UserUnFollowing, COUNT(*) as TimesUnFollowing FROM UnFollowers WITH (NOLOCK)
GROUP BY UserUnFollowed, UserUnFollowing
