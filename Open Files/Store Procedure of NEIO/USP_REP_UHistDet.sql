set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


-- =============================================
-- Author:		AJAY JAISWAL
-- Create date: 05/05/2010
-- Description:	This is useful for generating Detailed User History Date-wise and Transaction No. wise Report
-- Modify date: 
-- Modified By:  
-- Remark:
-- =============================================

ALTER PROCEDURE [dbo].[USP_REP_UHistDet]
@TMPAC NVARCHAR(50),@TMPIT NVARCHAR(50),@SPLCOND VARCHAR(8000),@SDATE  SMALLDATETIME,@EDATE SMALLDATETIME
,@SAC AS VARCHAR(60),@EAC AS VARCHAR(60)
,@SIT AS VARCHAR(60),@EIT AS VARCHAR(60)
,@SAMT FLOAT,@EAMT FLOAT
,@SDEPT AS VARCHAR(60),@EDEPT AS VARCHAR(60)
,@SCATE AS VARCHAR(60),@ECATE AS VARCHAR(60)
,@SWARE AS VARCHAR(60),@EWARE AS VARCHAR(60)
,@SINV_SR AS VARCHAR(60),@EINV_SR AS VARCHAR(60)
,@LYN VARCHAR(20)
,@EXPARA  AS VARCHAR(60)= NULL
AS
Declare @sqlcommand nvarchar(4000)
set @sqlcommand = '
Select UserHist_vw.[user_name],UserHist_vw.date,UserHist_vw.entry_ty,
UserHist_vw.inv_no,lcode.code_nm 
from UserHist_vw 
inner join lcode on UserHist_vw.entry_ty = lcode.entry_ty 
Where (UserHist_vw.date between '''+convert(varchar(50),@sdate)+''' and '''+convert(varchar(50),@edate)+''')'
+@EXPARA+' 
order by date, code_nm, inv_no'

Execute sp_executesql @sqlcommand
