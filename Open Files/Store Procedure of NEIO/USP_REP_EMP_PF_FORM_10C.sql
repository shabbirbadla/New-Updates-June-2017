IF EXISTS( Select [Name] from sysobjects where [Name]='USP_REP_EMP_PF_FORM_10C')
BEGIN
DROP PROCEDURE USP_REP_EMP_PF_FORM_10C
END
GO
-- =============================================
-- Author:		Ramya.
-- Create date: 02/02/2013
-- Description:	This Stored Procedure is useful for PF form 10C.
-- =============================================
CREATE PROCEDURE    [dbo].[USP_REP_EMP_PF_FORM_10C]
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
Begin
	DECLARE @FCON AS NVARCHAR(2000),@SQLCOMMAND NVARCHAR(4000)
	EXECUTE   USP_REP_FILTCON 
	@VTMPAC =@TMPAC,@VTMPIT =@TMPIT,@VSPLCOND =@SPLCOND
	,@VSDATE=@SDATE
	,@VEDATE=@EDATE
	,@VSAC =@SAC,@VEAC =@EAC
	,@VSIT=@SIT,@VEIT=@EIT
	,@VSAMT=@SAMT,@VEAMT=@EAMT
	,@VSDEPT=@SDEPT,@VEDEPT=@EDEPT
	,@VSCATE =@SCATE,@VECATE =@ECATE
	,@VSWARE =@SWARE,@VEWARE  =@EWARE
	,@VSINV_SR =@SINV_SR,@VEINV_SR =@SINV_SR
	,@VMAINFILE='p',@VITFILE='',@VACFILE=''
	,@VDTFLD ='MnthLastDt'
	,@VLYN =NULL
	,@VEXPARA=@EXPARA
	,@VFCON =@FCON OUTPUT

Declare @tDate SmallDateTime

Set @tDate=@sDate


Select EmployeeCode,FullNm into #fm From Emp_Family_Details where  Relation='Father'
Select EmployeeCode,FullNm into #fm1 From Emp_Family_Details where  Relation='Husband'


--insert into #fm (EmployeeCode,FullNm) Select EmployeeCode,FullNm From Emp_Family_Details 
--where  Relation='Father'
--and EmployeeCode not in (Select EmployeeCode From #Fm)


select  em.EmployeeCode,pMailName=(Case when isnull(em.pMailName,'')='' Then em.EmployeeName Else em.pMailName End),em.DOB,em.CAdd1,em.CAdd2,em.CAdd3,em.CCity,em.CState,em.CCountry,em.CPin
,em.DOJ,em.PAdd1,em.Padd2,em.PAdd3,em.PCity,em.PState,em.PCountry,em.PPin,em.PFNO,lc.pf_code,em.DOLReason,em.DOL
,FatherNm=isnull(#fm.FullNm,''),Husband=isnull(#fm1.FullNm,'')
,em.BankAccNo
,em.BankName
,em.Branch
,fm.Fullnm,fm.Relation,fDOB=fm.DOB,fm.Depen
from employeemast em 
Left join Emp_Family_Details fm on(em.EmployeeCode=fm.EmployeeCode)
Left join #fm on(em.EmployeeCode=#fm.EmployeeCode)
Left join #fm1 on(em.EmployeeCode=#fm1.EmployeeCode)
Left join Loc_Master lc on(em.loc_code=lc.loc_code)
order by lc.pf_code,em.employeecode

end