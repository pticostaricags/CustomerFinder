CREATE VIEW [dbo].[vwDomainCheckDailyStatus]
	AS 
--DAILY STATUS PER DOMAIN
SELECT CONVERT(DATE,DL.CheckedDateTime) AS [DATE], DI.Domain, DI.Website,
DL.ResponseMessage, ISNULL(DL.ResponseCode,'NULL RESPONSE') AS RESPONSECODE, COUNT(DL.ResponseMessage) AS TIMESMESSAGERECEIVED
FROM DomainInfo DI
INNER JOIN DomainLookup DL ON DL.DomainId = DI.DomainInfoId
--WHERE DL.RESPONSECODE IS NULL OR DL.RESPONSECODE <> 'OK'
GROUP BY CONVERT(DATE,DL.CheckedDateTime), DI.Domain, DI.Website, DL.ResponseMessage, DL.ResponseCode