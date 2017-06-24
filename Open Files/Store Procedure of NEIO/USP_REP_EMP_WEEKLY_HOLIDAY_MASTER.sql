IF EXISTS(SELECT * FROM SYS.OBJECTS WHERE [NAME]='USP_REP_EMP_WEEKLY_HOLIDAY_MASTER')
BEGIN
DROP PROCEDURE USP_REP_EMP_WEEKLY_HOLIDAY_MASTER
END
GO
-- =============================================
-- Author: Ramya
-- Create date: 05/03/2012
-- Description:	This is useful for Employee Holiday Master Report
-- Modify date: 
-- Remark:
-- =============================================

CREATE PROCEDURE [dbo].[USP_REP_EMP_WEEKLY_HOLIDAY_MASTER]
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

SELECT LOC_DESC=isnull(LOC_MASTER.LOC_DESC,''),EmployeeMast.EmployeeCode,EmployeeMast.EmployeeName,EmployeeMast.Department,EmployeeMast.Grade,EmployeeMast.Designation,EmployeeMast.Category,
--EmployeeMast.FirstWeekVal,EmployeeMast.SecondWeekVal,EmployeeMast.ThirdWeekVal,EmployeeMast.FourthWeekVal,EmployeeMast.FifthWeekVal,
EmployeeMast.FirstWeekWO,EmployeeMast.SecondWeekWO,EmployeeMast.ThirdWeekWO,EmployeeMast.FourthWeekWO,EmployeeMast.FifthWeekWO,EmployeeMast.SixthWeekWO
FROM EmployeeMast 
Left JOIN LOC_MASTER ON EmployeeMast.LOC_CODE=LOC_MASTER.LOC_CODE
--WHERE Lv_Year=@EXPARA ORDER BY Lv_Year DESC


END

