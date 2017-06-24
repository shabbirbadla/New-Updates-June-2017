IF EXISTS(SELECT * FROM SYS.OBJECTS WHERE [NAME]='Usp_Rep_Emp_Processing_Month')
BEGIN
DROP PROCEDURE Usp_Rep_Emp_Processing_Month
END
GO

-- =============================================
-- Author: Romulus
-- Create date: 09/05/2012
-- Description:	This is useful for Processing Month Report
-- Modify date: 
-- Remark:
-- =============================================

CREATE PROCEDURE [dbo].[Usp_Rep_Emp_Processing_Month]
@TMPAC NVARCHAR(50),@TMPIT NVARCHAR(50),@SPLCOND VARCHAR(8000),@SDATE  SMALLDATETIME,@EDATE SMALLDATETIME
,@SAC AS VARCHAR(60),@EAC AS VARCHAR(60)
,@SIT AS VARCHAR(60),@EIT AS VARCHAR(60)
,@SAMT FLOAT,@EAMT FLOAT
,@SDEPT AS VARCHAR(60),@EDEPT AS VARCHAR(60)
,@SCATE AS VARCHAR(60),@ECATE AS VARCHAR(60)
,@SWARE AS VARCHAR(60),@EWARE AS VARCHAR(60)
,@SINV_SR AS VARCHAR(60),@EINV_SR AS VARCHAR(60)
,@LYN VARCHAR(20)
,@EXPARA  AS VARCHAR(1000)
AS

Declare @FCON as NVARCHAR(2000),@SQLCOMMAND as NVARCHAR(4000)
	
BEGIN

set @SQLCOMMAND=''
set @SQLCOMMAND=rtrim(@SQLCOMMAND)+' '+'select LOC_DESC=isnull(LOC_MASTER.LOC_DESC,''''),Emp_Processing_Month.* from Emp_Processing_Month'
set @SQLCOMMAND=rtrim(@SQLCOMMAND)+' '+'Left JOIN LOC_MASTER ON Emp_Processing_Month.LOC_CODE=LOC_MASTER.LOC_CODE'
if(isnull(@EXPARA,'')!='')
begin
set @SQLCOMMAND=rtrim(@SQLCOMMAND)+' '+'WHERE Pay_Year='+char(39)+@EXPARA+char(39)
end
set @SQLCOMMAND=rtrim(@SQLCOMMAND)+' '+'ORDER BY Pay_Year ,Pay_Month'

print @SQLCOMMAND

Execute Sp_ExecuteSql @SqlCommand






END 


