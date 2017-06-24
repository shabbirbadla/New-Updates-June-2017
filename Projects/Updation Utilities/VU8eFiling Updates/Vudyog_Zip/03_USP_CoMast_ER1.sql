set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[USP_CoMast_ER1]  
	-- Add the parameters for the stored procedure hereUSP_CoMast_ER1
	@compname AS VARCHAR(60)
AS
BEGIN
	-- Insert statements for procedure here
	SELECT co_name as AssesName, '' as City FROM Co_mast where co_name=@compname;
END


-- exec [dbo].[USP_CoMast_ER1][Sandeep Steel]

