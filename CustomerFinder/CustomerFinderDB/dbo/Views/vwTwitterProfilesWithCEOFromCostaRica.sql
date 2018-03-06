CREATE VIEW [dbo].[vwTwitterProfilesWithCEOFromCostaRica]
	AS
		SELECT TPCR.* FROM vwTwitterProfilesInCostaRica TPCR
		INNER JOIN vwTwitterProfilesWithCEO TPCEO ON TPCR.TwitterAccountsId = TPCEO.TwitterAccountsId
