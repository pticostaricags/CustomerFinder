CREATE FUNCTION [dbo].[fnGetTwitterAccountUrlByUserName]
(	
	-- Add the parameters for the function here
	@username nvarchar(100)
)
RETURNS nvarchar(500) 
AS
BEGIN
	DECLARE @result nvarchar(500)
	-- Add the SELECT statement with parameter references here
	SELECT @result='http://www.twitter.com/' + @username
	RETURN @result
END