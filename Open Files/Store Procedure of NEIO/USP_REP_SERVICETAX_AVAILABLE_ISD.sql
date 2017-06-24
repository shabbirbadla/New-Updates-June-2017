If Exists (Select [Name] From SysObjects Where xType='P' and [Name]='USP_REP_SERVICETAX_AVAILABLE_ISD')
Begin
	Drop Procedure USP_REP_SERVICETAX_AVAILABLE_ISD
End
Go
-- =============================================
-- Author:		Ruepesh Prajapati.
-- Create date: 25/11/2009
-- Description:	This Stored procedure is useful to generate Service Tax Input Credit Available Report .
-- Modify date: By: date: Shrikant S. on 08/08/2012 for ISD issue
-- Remark:
-- =============================================
Create procedure [dbo].[USP_REP_SERVICETAX_AVAILABLE_ISD]
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
	,@VMAINFILE='mall',@VITFILE=Null,@VACFILE='AC'
	,@VDTFLD ='DATE'
	,@VLYN=Null
	,@VEXPARA=@EXPARA
	,@VFCON =@FCON OUTPUT

	/*select m.entry_ty,m.tran_cd,m.u_pinvdt,m.date,m.inv_no,m.u_pinvno,m.serty,pdate=m.date ,a.ac_name,a.add1,a.add2,a.add3,a.SREGN,m.net_amt
	,taxable_amt=m.gro_amt+m.tot_deduc+m.tot_tax 
	,bSrTax=m.net_amt
	,bESrTax=m.net_amt
	,bHSrTax=m.net_amt
	,aSrTax=m.net_amt
	,aESrTax=m.net_amt
	,aHSrTax=m.net_amt
	,m.bank_nm,m.cheq_no
	into #serava
	from bpacdet ac 
	inner join bpmain m on (ac.entry_ty=m.entry_ty and ac.tran_cd=m.tran_cd) 
	inner join ac_mast a on (m.ac_id=a.ac_id) inner join ac_mast aa on (ac.ac_id=aa.ac_id) 
	inner join mainall_vw mall on (m.entry_ty=mall.entry_all and m.tran_cd=mall.main_tran and m.ac_id=mall.ac_id)   
	WHERE 1=2*/
	

	select aentry_ty,atran_cd,serbamt=sum(serbamt),sercamt=sum(sercamt),serhamt=sum(serhamt) 
	into #ISDAllocation 
	from ISDAllocation 
	group by aentry_ty,atran_cd

	
--	Added by Shrikant S. on 22/08/2012 for Bug-5779		--Start
	select m.date,m.entry_ty,m.tran_cd,acl.Serty,Amount=sum(isnull(acl.Amount,0)),sTaxable=sum(isnull(acl.sTaxable,0)),SerBAmt=sum(isnull(acl.SerBAmt,0)),SerCAmt=sum(isnull(acl.SerCAmt,0)),SerHAmt=sum(isnull(acl.SerHAmt,0))
	into #acl1
	from acdetalloc acl 
	inner join SerTaxMain_vw m on (acl.entry_ty=m.entry_ty and acl.tran_cd=m.tran_cd)
	where  m.date<=@EDATE and acl.SerTy<>'OTHERS' and acl.SerAvail='SERVICES' and acl.entry_ty not in ('S1') 
	and acl.entry_ty in ('E1','BP','CP','JV','J3')
	group by m.date,m.entry_ty,m.tran_cd,acl.Serty

	insert into #acl1 (entry_ty,tran_cd,serty,Amount,sTaxable,SerBAmt,SerCAmt,SerHAmt,date)
	select ac.entry_ty,ac.tran_cd,jm.Serty
	,Amount=(case when jm.entry_ty in ('J3') then jm.sTaxable else 0 end),sTaxable=(case when jm.entry_ty in ('J3') then jm.sTaxable else 0 end)
	,SerBAmt=sum(case when a.typ='Service Tax Available' then ac.amount else 0 end)
	,SerCAmt=sum(case when a.typ='Service Tax Available-Ecess' then ac.amount else 0 end)
	,SerHAmt=sum(case when a.typ='Service Tax Available-Hcess' then ac.amount else 0 end)
	,(case when (m.entry_ty in ('J3')) then ac.u_cldt else m.date end)
	from SerTaxAcDet_vw ac 
	inner join SerTaxMain_vw m on (m.entry_ty=ac.entry_ty and m.tran_cd=ac.tran_cd)
	inner join ac_mast a on (ac.ac_id=a.ac_id) 
	left join JvMain jm on (jm.entry_ty=ac.entry_ty and jm.tran_cd=ac.tran_cd)
	where ( (case when (m.entry_ty in ('J3')) then ac.u_cldt else m.date end)  <=@EDATE )
	AND ac.amt_ty='Dr' and a.typ in ('Service Tax Available' ,'Service Tax Available-Ecess','Service Tax Available-Hcess')
	and ac.entry_ty not in ('E1','BP','CP')
	group by ac.entry_ty,ac.tran_cd,jm.Serty,(case when (m.entry_ty in ('J3')) then ac.u_cldt else m.date end),jm.entry_ty,jm.sTaxable
--	Added by Shrikant S. on 22/08/2012 for Bug-5779		--End

	select entry_ty,code_nm,bcode_nm=case when ext_vou=0 then entry_ty else bcode_nm end into #lcode from lcode

	select  
	entry_ty=(case when m.tdspaytype=2 then i.entry_ty else i.aentry_ty end)
	,tran_cd=(case when m.tdspaytype=2 then i.tran_cd else i.atran_Cd end)
	,serbamt=sum(i.serbamt),sercamt=sum(i.sercamt),serhamt=sum(i.serhamt)
	,pdate=m.date
	into #isdallocationR 
	from isdallocation i 
	inner join SerTaxMain_vw m on (i.entry_ty=m.entry_ty and i.tran_cd=m.tran_cd)
	inner join #lcode l on (m.entry_ty=l.entry_ty)
	where m.date between  @sdate and @edate
	group by i.entry_ty,i.tran_cd,i.aentry_ty,i.atran_Cd,m.tdspaytype,m.bank_nm,m.cheq_no,m.date
	
--select * from #isdallocationR

--	Added by Shrikant S. on 22/08/2012 for Bug-5779		--Start
select 
	m.entry_ty,m.tran_cd,m.u_pinvdt,m.date,m.inv_no,m.u_pinvno,acdet.serty,pdate=case when m.entry_ty IN ('BP','CP') then m.date else 0 end
	,ac_name=case when m.Entry_ty='J3' then ac2.Ac_name else ac.ac_name end
	,add1=case when m.Entry_ty='J3' then ac2.add1 else ac.add1 end
	,add2=case when m.Entry_ty='J3' then ac2.add2 else ac.add2 end
	,add3=case when m.Entry_ty='J3' then ac2.add3 else ac.add3 end
	,SREGN=case when m.Entry_ty='J3' then ac2.SREGN else ac.SREGN end
	,m.net_amt
	,taxable_amt=acdet.staxable	
	,bSrTax=m.serbamt
	,bESrTax=m.sercamt
	,bHSrTax=m.serhamt
	,aSrTax=isnull(a.serbamt,0)
	,aESrTax=isnull(a.sercamt,0)
	,aHSrTax=isnull(a.serhamt,0)
	,m.bank_nm,m.cheq_no
	,Ava_SrTax=m.serbamt-isnull(i.serbamt,0)
	,Ava_ESrTax=m.sercamt-isnull(i.sercamt,0)
	,Ava_HSrTax=m.serhamt-isnull(i.serhamt,0)
	into #serava
	from SerTaxMain_vw m 
	inner join #acl1 acdet on (m.entry_ty=acdet.entry_ty and m.tran_cd=acdet.tran_cd)
	inner join ac_mast ac on (m.ac_id=ac.ac_id)
	left join #isdallocationR  a on (a.entry_ty=m.entry_ty and a.tran_cd=m.tran_cd)
	left join #lcode l on (a.entry_ty=l.entry_ty)	
	left join #ISDAllocation i on (a.entry_ty=i.aentry_ty and a.tran_cd=i.atran_cd)
	Left Join ac_mast ac2 on (ac2.ac_id=m.cons_id)
	--where (l.bcode_nm in('EP','BP','CP') or l.entry_ty in ('EP','BP','CP') )  and m.date between  @sdate and @edate 
	where m.date between  @sdate and @edate 
	order by m.date
--	Added by Shrikant S. on 22/08/2012 for Bug-5779		--End

-- Commented by Shrikant S. on 22/08/2012 for Bug-5779		--Start
--	select 
--	a.entry_ty,a.tran_cd,m.u_pinvdt,m.date,m.inv_no,m.u_pinvno,m.serty,pdate=m.date,ac.ac_name,ac.add1,ac.add2,ac.add3,ac.SREGN,m.net_amt
--	,taxable_amt=m.gro_amt+m.tot_deduc+m.tot_tax
--	,bSrTax=m.serbamt
--	,bESrTax=m.sercamt
--	,bHSrTax=m.serhamt
--	,aSrTax=a.serbamt
--	,aESrTax=a.sercamt
--	,aHSrTax=a.serhamt
--	,a.bank_nm,a.cheq_no
--	into #serava
--	from #isdallocationR  a
--	inner join #lcode l on (a.entry_ty=l.entry_ty)
--	inner join SerTaxMain_vw m on (a.entry_ty=m.entry_ty and a.tran_cd=m.tran_cd)
--	inner join ac_mast ac on (m.ac_id=ac.ac_id)
--	left join #ISDAllocation i on (a.entry_ty=i.aentry_ty and a.tran_cd=i.atran_cd)
--	where l.bcode_nm in('EP','BP','CP') 
--	order by m.date
-- Commented by Shrikant S. on 22/08/2012 for Bug-5779		--End

	select * from #serava order by date 
    DROP TABLE #serava
	
END











