If Exists(Select [name] From SysObjects Where xType='P' and [Name]='USP_REP_INPUT_SERVICETAX_MultiService')
Begin
	Drop Procedure USP_REP_INPUT_SERVICETAX_MultiService
End
Go


-- =============================================
-- Author:		Ruepesh Prajapati.
-- Create date: 22/05/210
-- Description:	This Stored procedure is useful to generate Input Service Tax as per Bill Report.
-- Modification Date/By/Reason:
-- Modification Date/By/Reason: Birendra on 7 July 2010 for Tkt-2833
-- Remark:
-- =============================================
CREATE procedure [dbo].[USP_REP_INPUT_SERVICETAX_MultiService]
@TMPAC NVARCHAR(50),@TMPIT NVARCHAR(50),@SPLCOND VARCHAR(8000),@SDATE  SMALLDATETIME,@EDATE SMALLDATETIME
,@SAC AS VARCHAR(60),@EAC AS VARCHAR(60)
,@SIT AS VARCHAR(60),@EIT AS VARCHAR(60)
,@SAMT FLOAT,@EAMT FLOAT
,@SDEPT AS VARCHAR(60),@EDEPT AS VARCHAR(60)
,@SCATE AS VARCHAR(60),@ECATE AS VARCHAR(60)
,@SWARE AS VARCHAR(60),@EWARE AS VARCHAR(60)
,@SINV_SR AS VARCHAR(60),@EINV_SR AS VARCHAR(60)
,@LYN VARCHAR(20)
,@EXPARA  AS VARCHAR(60)= null
AS
Begin
	Declare @FCON as NVARCHAR(2000),@SQLCOMMAND as NVARCHAR(4000)
	
	EXECUTE USP_REP_FILTCON 
	@VTMPAC =@TMPAC,@VTMPIT =@TMPIT,@VSPLCOND =@SPLCOND
	,@VSDATE=@SDATE,@VEDATE=@EDATE
	,@VSAC =@SAC,@VEAC =@EAC
	,@VSIT=@SIT,@VEIT=@EIT
	,@VSAMT=@SAMT,@VEAMT=@EAMT
	,@VSDEPT=@SDEPT,@VEDEPT=@EDEPT
	,@VSCATE =@SCATE,@VECATE =@ECATE
	,@VSWARE =@SWARE,@VEWARE  =@EWARE
	,@VSINV_SR =@SINV_SR,@VEINV_SR =@SINV_SR
	,@VMAINFILE='m',@VITFILE=Null,@VACFILE='AC'
	,@VDTFLD ='DATE'
	,@VLYN=Null
	,@VEXPARA=@EXPARA
	,@VFCON =@FCON OUTPUT

	set @sqlcommand='select part=1,m.entry_ty,m.tran_cd,m.date,m.inv_no,m.u_pinvno,m.u_pinvdt,al.serty,m.serrule,m.inv_no,m.date,al.seravail'
	set @sqlcommand=rtrim(@sqlcommand)+' '+',ac_mast.ac_name,ac_mast.add1,ac_mast.add2,ac_mast.add3,ac_mast.city,ac_mast.SREGN,m.net_amt,taxable_amt=al.staxable'
	set @sqlcommand=rtrim(@sqlcommand)+' '+',bSrTax=al.serbamt'
	set @sqlcommand=rtrim(@sqlcommand)+' '+',bESrTax=al.sercamt'
	set @sqlcommand=rtrim(@sqlcommand)+' '+',bHSrTax=al.serhamt,ac_mast.MailName' --'ac_mast.mailname' Added by Birendra on 7-July 2010 for TKT-2833
	set @sqlcommand=rtrim(@sqlcommand)+' '+'from epmain m'
	set @sqlcommand=rtrim(@sqlcommand)+' '+'inner join acdetalloc al on (m.entry_ty=al.entry_ty and m.tran_cd=al.tran_cd)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+'inner join ac_mast on (m.ac_id=ac_mast.ac_id)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+'inner join lcode l on (m.entry_ty=l.entry_ty)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+rtrim(@fcon)
	set @sqlcommand=rtrim(@sqlcommand)+' '+' and (l.entry_ty in (''EP'') or l.bcode_nm in (''EP''))'
	set @sqlcommand=rtrim(@sqlcommand)+' '+' and (l.entry_ty NOT in (''IF'',''OF''))'	
	--set @sqlcommand=rtrim(@sqlcommand)+' '+'order by m.date,m.tran_cd,al.serty'--Bug-24957
	--Bug-24957 Strat
	set @sqlcommand=rtrim(@sqlcommand)+' '+' union all'
	set @sqlcommand=rtrim(@sqlcommand)+' '+' select part=2,jvi.entry_ty,jvi.tran_cd,jvi.date,jvi.inv_no,'''','''',jvm.sertyi,jvi.amt_ty,'''','''','''',jvm.party_nm,ac.add1,ac.add2,ac.add3,AC.CITY,ac.sregn,jvm.net_amt,jvm.net_amt,sum(case when a.typ IN (''Service Tax Available'',''Input Service Tax'') then jvi.amount else 0 end),sum(case when a.typ IN (''Service Tax Available-Ecess'',''Input Service Tax-Ecess'') then jvi.amount else 0 end),sum(case when a.typ IN (''Service Tax Available-Hcess'',''Input Service Tax-Hcess'') then jvi.amount else 0 end),'''' from jvacdet jvi'
	set @sqlcommand=rtrim(@sqlcommand)+' '+' inner join jvmain jvm on (jvi.tran_cd=jvm.tran_cd) '
	set @sqlcommand=rtrim(@sqlcommand)+' '+' inner join ac_mast a on (jvi.ac_id=a.ac_id)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+' inner join ac_mast ac on (jvm.ac_id=ac.ac_id)'
	set @sqlcommand=rtrim(@sqlcommand)+' '+' where a.typ in (''Service Tax Available'',''Service Tax Available-Ecess'',''Service Tax Available-Hcess'',''Input Service Tax-Ecess'',''Input Service Tax'',''Input Service Tax-Hcess'')  AND (JVI.DATE BETWEEN '''+CAST(@SDATE AS VARCHAR)+''' AND '''+CAST(@EDATE AS VARCHAR)+''')'
	set @sqlcommand=rtrim(@sqlcommand)+' '+' group by jvi.entry_ty,jvi.tran_cd,jvi.date,jvi.inv_no,jvi.serty,jvm.sertyi,jvm.party_nm,jvm.net_amt,jvi.amt_ty,ac.add1,ac.add2,ac.add3,AC.CITY,ac.sregn'
	--Bug-24957 End

	print @sqlcommand
	execute sp_executesql @sqlcommand 
END




