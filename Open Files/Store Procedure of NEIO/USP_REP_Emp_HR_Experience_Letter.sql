IF EXISTS(SELECT * FROM SYS.OBJECTS WHERE [NAME]='USP_REP_Emp_HR_Experience_Letter')
BEGIN
DROP PROCEDURE [USP_REP_Emp_HR_Experience_Letter]
END
GO
-- =============================================
-- Author:		
-- Create date:
-- Description:	This Stored Procedure is useful for Leave Maintance Details in reports
-- =============================================
Create procedure [dbo].[USP_REP_Emp_HR_Experience_Letter]
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
BEGIN
DECLARE @FCON AS NVARCHAR(2000)
EXECUTE   USP_REP_FILTCON 
@VTMPAC =@TMPAC,@VTMPIT =@TMPIT,@VSPLCOND =@SPLCOND
,@VSDATE=null
,@VEDATE=null
,@VSAC =@SAC,@VEAC =@EAC
,@VSIT=@SIT,@VEIT=@EIT
,@VSAMT=@SAMT,@VEAMT=@EAMT
,@VSDEPT=@SDEPT,@VEDEPT=@EDEPT
,@VSCATE =@SCATE,@VECATE =@ECATE
,@VSWARE =@SWARE,@VEWARE  =@EWARE
,@VSINV_SR =@SINV_SR,@VEINV_SR =@SINV_SR
,@VMAINFILE='e',@VITFILE='',@VACFILE=' '
,@VDTFLD =''
,@VLYN =NULL
,@VEXPARA=@EXPARA
,@VFCON =@FCON OUTPUT

if(@FCON='')
begin
	set @FCON=' where 1=1'
end 	
set @FCON=replace(@FCON,'Dept','Department')
set @FCON=replace(@FCON,'Cate','Category')
print @FCON
Declare @SQLCOMMAND as NVARCHAR(4000)

--@Loc_Desc varchar(30),
--@Year varchar(30),
--@Month varchar(30),
--@EmployeeName varchar(50)
--
--as
--begin
Declare @Pay_Year varchar(30),@Pay_Month varchar(30),@EmpNm varchar(100)
Declare @POS INT
if(charindex('[Pay_Year=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX(']',@EXPARA)
	SET @Pay_Year=SUBSTRING(@EXPARA,11,@POS-11)
end 	

if(charindex('[Pay_Month=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[Pay_Month=',@EXPARA)
	SET @Pay_Month=SUBSTRING(@EXPARA,@POS+11,len(@EXPARA)-@pos)
	SET @Pay_Month=replace(@Pay_Month,'[Pay_Month=','')
	SET @POS=CHARINDEX(']',@Pay_Month)
	SET @Pay_Month=SUBSTRING(@Pay_Month,1,@pos)
	SET @Pay_Month=replace(@Pay_Month,']','')
end


if(charindex('[EmpNm=',@EXPARA)>0)
begin
	SET @POS=CHARINDEX('[EmpNm=',@EXPARA)
	SET @EmpNm=SUBSTRING(@EXPARA,@POS+7,len(@EXPARA)-@pos)
	SET @EmpNm=replace(@EmpNm,']','')
end

declare @empcode varchar(30)
select @empcode=employeecode from employeemast where employeename=@empnm

select employeecode,employeename,doj,dol,designation from employeemast where 
employeename=@empnm and employeecode=@empcode

END

