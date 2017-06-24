IF EXISTS(SELECT * FROM SYS.OBJECTS WHERE [NAME]='Usp_Rep_Emp_Pay_Head_Master')
BEGIN
DROP PROCEDURE [Usp_Rep_Emp_Pay_Head_Master]
END
GO

-- =============================================
-- Author: Ramya
-- Create date: 26/12/2011
-- Description:	This is useful for Employee Pay Head Master Report
-- Modify date: 
-- Remark:
-- =============================================

Create PROCEDURE [dbo].[Usp_Rep_Emp_Pay_Head_Master]
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

Declare @FCON as NVARCHAR(2000)
	
	EXECUTE USP_REP_FILTCON 
		@VTMPAC=null,@VTMPIT=@TMPIT,@VSPLCOND=@SPLCOND,
		@VSDATE=@SDATE,@VEDATE=@EDATE,
		@VSAC =null,@VEAC =null,
		@VSIT=@SIT,@VEIT=@EIT,
		@VSAMT=null,@VEAMT=null,
		@VSDEPT=@SDEPT,@VEDEPT=@EDEPT,
		@VSCATE =@SCATE,@VECATE =@ECATE,
		@VSWARE =@SWARE,@VEWARE  =@EWARE,
		@VSINV_SR =@SINV_SR,@VEINV_SR =@EINV_SR,
		@VMAINFILE='',@VITFILE='',@VACFILE=null,
		@VDTFLD = 'DATE',@VLYN=null,@VEXPARA=@EXPARA,
		@VFCON =@FCON OUTPUT
BEGIN

select a.*,b.HeadType from Emp_Pay_Head_Master a
inner join Emp_Pay_Head b on(a.HeadTypeCode=b.HeadTypeCode)
order by b.SortOrd,a.SortOrd

END 


