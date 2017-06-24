IF EXISTS(SELECT * FROM SYS.OBJECTS WHERE [NAME]='USP_REP_EMP_PER_DET')
BEGIN
DROP PROCEDURE USP_REP_EMP_PER_DET
END
GO
-- =============================================
-- Author: Ramya
-- Create date: 05/03/2012
-- Description:	This is useful for Employee Holiday Master Report
-- Modify date: 
-- Remark:
-- =============================================

CREATE PROCEDURE [dbo].[USP_REP_EMP_PER_DET]
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

select LOC_DESC=isnull(L.LOC_DESC,''),e.*,a.ac_name,ac.ac_group_name 
from EmployeeMast e
inner join Ac_mast a on (a.ac_id=E.ac_id)
inner join ac_group_mast ac on (a.ac_group_id=ac.ac_group_id)
Left JOIN LOC_MASTER l ON (e.LOC_CODE=L.LOC_CODE)

END

