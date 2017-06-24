set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go




-- =============================================
-- Author:		Yaaminikanth
-- Edited By : Romulus and Deepa for changes on dated 18-feb-2010
-- Create date: 15th December 2009
-- Description:	Procedure for capturing Company details
-- =============================================
create PROCEDURE [dbo].[Get_Company_Details]
@compid varchar(50),
@temp int
AS
BEGIN
	
if(@temp=1)
 

SELECT dir_nm,co_name,compid,CONVERT(nvarchar, datepart(year, sta_dt))+'-'+CONVERT(nvarchar, datepart(year, end_dt)) as FinYear,dbname from co_mast order by dir_nm;
else
SELECT CONVERT(nvarchar, datepart(year, sta_dt))+'-'+CONVERT(nvarchar, datepart(year, end_dt)) as FinYear,dbname from co_mast where compid= @compid;


END



