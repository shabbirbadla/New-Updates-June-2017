IF EXISTS(SELECT * FROM SYS.OBJECTS WHERE [NAME]='USP_REP_EMP_SHIFT_MASTER')
BEGIN
DROP PROCEDURE [USP_REP_EMP_SHIFT_MASTER]
END
GO
-- =============================================
-- Author: Ramya
-- Create date: 05/03/2012
-- Description:	This is useful for Employee Shift Master Report
-- Modify date: 
-- Remark:
-- =============================================

CREATE PROCEDURE [dbo].[USP_REP_EMP_SHIFT_MASTER]
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
BEGIN

select Incharge=EmployeeMast.EmployeeName,LOC_DESC=isnull(LOC_MASTER.LOC_DESC,''),Emp_Shift_Master.* from Emp_Shift_Master
Left JOIN LOC_MASTER ON Emp_Shift_Master.LOC_CODE=LOC_MASTER.LOC_CODE
Left JOIN EmployeeMast ON Emp_Shift_Master.Incharge_Code=EmployeeMast.EmployeeCode




END



