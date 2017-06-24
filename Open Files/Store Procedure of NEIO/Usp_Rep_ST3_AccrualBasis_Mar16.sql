If Exists(Select [Name] From SysObjects Where Xtype='P' and [Name]='Usp_Rep_ST3_AccrualBasis_Mar16')
Begin
	Drop Procedure Usp_Rep_ST3_AccrualBasis_Mar16
End
GO
--EXECUTE Usp_Rep_ST3_AccrualBasis_Mar16 '','','','04/01/2015','03/31/2016','','','','',0,0,'','','','','','','','','2015-2016',''
-- =============================================
-- Author:		
-- Create date: 11/10/2013
-- Description:	This Stored procedure is useful to generate Service Tax ST 3 Accrual Basis Report .
-- Remark:
-- Modify by Vasant on 05/03/2014 as per BUG-21952 (Required changes in ST-3 as per the New Format). - BUG21952
-- =============================================

Create procedure [dbo].[Usp_Rep_ST3_AccrualBasis_Mar16]
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
begin --sp
	
declare @sdate1 smalldatetime,@edate1 smalldatetime,@sdate2 smalldatetime,@edate2 smalldatetime,@sdate3 smalldatetime,@edate3 smalldatetime,@sdate4 smalldatetime,@edate4 smalldatetime,@sdate5 smalldatetime,@edate5 smalldatetime,@sdate6 smalldatetime,@edate6 smalldatetime
declare @particulars varchar(250),@particulars1 varchar(250),@u_arrears varchar(250),@sDocNo varchar(50),@sDocDt smalldatetime
declare @c int,@d int,@m int,@y int,@strdt varchar(10)
declare @u_chalno varchar(20),@u_chaldt smalldatetime,@date smalldatetime,@CommanNotivar varchar(300)
declare @isdprouct bit

set @isdprouct=0
if CHARINDEX('VUISD', upper(@EXPARA))>0
begin
	set @isdprouct=1
end

set @sdate1=@sdate1
	
set @c=1
set @m=month(@sdate)
set @y=year(@sdate)
while(@c<=6)
begin
	if(@c=1)
	begin
		--set @sdate1=cast('04/01/2008' as smalldatetime) cast(day(@sdate) as varchar(2))+'/'+cast(month(@sdate) as varchar(2))+'/'+cast(year(@sdate) as varchar(4))
		set @sdate1=cast( cast(month(@sdate) as varchar(2))+'/'+cast(day(@sdate) as varchar(2))+'/'+cast(year(@sdate) as varchar(4))  as smalldatetime)
		if(@m in (1,3,5,7,8,10,12))
		begin
			set @edate1=cast( cast(@m as varchar(2))+'/'+cast(31 as varchar(2))+'/'+cast(@y as varchar(4))  as smalldatetime)
		end
		if(@m in (4,6,9,11))
		begin
			set @edate1=cast( cast(@m as varchar(2))+'/'+cast(30 as varchar(2))+'/'+cast(@y as varchar(4))  as smalldatetime)
		end
		if(@m in (2))
		begin
			set @strdt=cast( @y as varchar(4)  )+'/'+cast( @m as varchar(4)  )+'/29'
			if isdate(@strdt)=1
			begin
				set @edate1=cast(@strdt as smalldatetime)	
			end
			else
			begin
				set @strdt=cast( @y as varchar(4)  )+'/'+cast( @m as varchar(4)  )+'/28'
				set @edate1=cast( @strdt as smalldatetime)	
			end	
		end		
	end


	if(@c=2)
	begin
		set @sdate2=cast( cast(@m as varchar(2))+'/'+cast(1 as varchar(2))+'/'+cast(@y as varchar(4))  as smalldatetime)
		if(@m in (1,3,5,7,8,10,12))
		begin
			set @edate2=cast( cast(@m as varchar(2))+'/'+cast(31 as varchar(2))+'/'+cast(@y as varchar(4))  as smalldatetime)
		end
		if(@m in (4,6,9,11))
		begin
			set @edate2=cast( cast(@m as varchar(2))+'/'+cast(30 as varchar(2))+'/'+cast(@y as varchar(4))  as smalldatetime)
		end
		if(@m in (2))
		begin
			set @strdt=cast( @y as varchar(4)  )+'/'+cast( @m as varchar(4)  )+'/29'
			if isdate(@strdt)=1
			begin
				set @edate2=cast(@strdt as smalldatetime)	
			end
			else
			begin
				set @strdt=cast( @y as varchar(4)  )+'/'+cast( @m as varchar(4)  )+'/28'
				set @edate2=cast( @strdt as smalldatetime)	
			end	
		end		

	end --c=2
	
	if(@c=3)
	begin
		set @sdate3=cast( cast(@m as varchar(2))+'/'+cast(1 as varchar(2))+'/'+cast(@y as varchar(4))  as smalldatetime)

		if(@m in (1,3,5,7,8,10,12))
		begin
			set @edate3=cast( cast(@m as varchar(2))+'/'+cast(31 as varchar(2))+'/'+cast(@y as varchar(4))  as smalldatetime)
		end
		if(@m in (4,6,9,11))
		begin
			set @edate3=cast( cast(@m as varchar(2))+'/'+cast(30 as varchar(2))+'/'+cast(@y as varchar(4))  as smalldatetime)
		end
		if(@m in (2))
		begin
			set @strdt=cast( @y as varchar(4)  )+'/'+cast( @m as varchar(4)  )+'/29'
			if isdate(@strdt)=1
			begin
				set @edate3=cast(@strdt as smalldatetime)	
			end
			else
			begin
				set @strdt=cast( @y as varchar(4)  )+'/'+cast( @m as varchar(4)  )+'/28'
				set @edate3=cast( @strdt as smalldatetime)	
			end	
		end		

	end --c=3

	if(@c=4)
	begin
		set @sdate4=cast( cast(@m as varchar(2))+'/'+cast(1 as varchar(2))+'/'+cast(@y as varchar(4))  as smalldatetime)

		if(@m in (1,3,5,7,8,10,12))
		begin
				set @edate4=cast( cast(@m as varchar(2))+'/'+cast(31 as varchar(2))+'/'+cast(@y as varchar(4))  as smalldatetime)
		end
		if(@m in (4,6,9,11))
		begin
			set @edate4=cast( cast(@m as varchar(2))+'/'+cast(30 as varchar(2))+'/'+cast(@y as varchar(4))  as smalldatetime)
		end
		if(@m in (2))
		begin
			set @strdt=cast( @y as varchar(4)  )+'/'+cast( @m as varchar(4)  )+'/29'
			if isdate(@strdt)=1
			begin
				set @edate4=cast(@strdt as smalldatetime)	
			end
			else
			begin
				set @strdt=cast( @y as varchar(4)  )+'/'+cast( @m as varchar(4)  )+'/28'
				set @edate2=cast( @strdt as smalldatetime)	
			end	
		end		

	end --c=4
		
	if(@c=5)
	begin
		set @sdate5=cast( cast(@m as varchar(2))+'/'+cast(1 as varchar(2))+'/'+cast(@y as varchar(4))  as smalldatetime)

		if(@m in (1,3,5,7,8,10,12))
		begin
			set @edate5=cast( cast(@m as varchar(2))+'/'+cast(31 as varchar(2))+'/'+cast(@y as varchar(4))  as smalldatetime)
		end
		if(@m in (4,6,9,11))
		begin
			set @edate5=cast( cast(@m as varchar(2))+'/'+cast(30 as varchar(2))+'/'+cast(@y as varchar(4))  as smalldatetime)
		end
		if(@m in (2))
		begin
			set @strdt=cast( @y as varchar(4)  )+'/'+cast( @m as varchar(4)  )+'/29'
			if isdate(@strdt)=1
			begin
				set @edate5=cast(@strdt as smalldatetime)	
			end
			else
			begin
				set @strdt=cast( @y as varchar(4)  )+'/'+cast( @m as varchar(4)  )+'/28'
				set @edate5=cast( @strdt as smalldatetime)	
			end	
		end		

	end --c=5

	if(@c=6)
	begin
		set @sdate6=cast( cast(@m as varchar(2))+'/'+cast(1 as varchar(2))+'/'+cast(@y as varchar(4))  as smalldatetime)
		if(@m in (1,3,5,7,8,10,12))
		begin
				set @edate6=cast( cast(@m as varchar(2))+'/'+cast(31 as varchar(2))+'/'+cast(@y as varchar(4))  as smalldatetime)
		end
		if(@m in (4,6,9,11))
		begin
				set @edate6=cast( cast(@m as varchar(2))+'/'+cast(30 as varchar(2))+'/'+cast(@y as varchar(4))  as smalldatetime)
		end
		if(@m in (2))
		begin
			set @strdt=cast( @y as varchar(4)  )+'/'+cast( @m as varchar(4)  )+'/29'
			if isdate(@strdt)=1
			begin
				set @edate6=cast(@strdt as smalldatetime)	
			end
			else
			begin
				set @strdt=cast( @y as varchar(4)  )+'/'+cast( @m as varchar(4)  )+'/28'
				set @edate6=cast( @strdt as smalldatetime)	
			end	
		end		

	end --c=6
	--print @m
	--print @y

	set @m=@m+1
	if (@m>12)
	begin
		set @m=1
		set @y=@y+1
	end
	set @c=@c+1
end	


select distinct [name],[serty],[code],[st3_id],[st3_name] into #SerTax_Mast from SerTax_Mast
/*--->#st3_5b*/
select distinct ac.entry_ty,ac.tran_cd,ac_mast.ac_name,ac.amount,ac.amt_ty,ac.date
,sm.serty 
,m.sertype
/*=case when (isnull(bpm.tdspaytype,1)=2)  then isnull(bpm.sertype,'') else (case when (isnull(cpm.tdspaytype,1)=2)  then isnull(cpm.sertype,'') else (case when (isnull(bpm.tdspaytype,1)=2)  then isnull(bpm.sertype,'') else (case when (isnull(jvm.entry_ty,'')<>'')  then isnull(jvm.sertype,'') else (isnull(m1.sertype,'')) end) end) end) end*/
,beh=(case when l.ext_vou=1 then l.bcode_nm else l.entry_ty end)
,ac_mast.typ
into #st3_5b
from SerTaxAcDet_vw ac 
inner join ac_mast ac_mast on (ac.ac_id=ac_mast.ac_id) 
inner join lcode l on (ac.entry_ty=l.entry_ty)
left join #SerTax_Mast sm on (ac.serty=sm.[name]) /*TKT-4123 Inner join---> Left. Problem found in Service Tax Adjustment entry with Excise-->Servicre Tax*/
left join SerTaxMain_vw m on (m.entry_ty=ac.entry_ty and m.tran_cd =ac.tran_cd)
left join jvmain jvm on (jvm.entry_ty=ac.entry_ty and jvm.tran_cd =ac.tran_cd)
where ac_mast.ac_name like '%service tax available%'
and (ac.u_cldt<= @edate)
--order by ac.tran_cd,ac_mast.ac_name	
union all
select distinct ac.entry_ty,ac.tran_cd
,ac_name=case 
	when ac_mast.ac_name='Service Tax Payable' then 'Service Tax Available' 
	when ac_mast.ac_name='Edu. Cess on Service Tax Payable' then 'Edu. Cess on Service Tax Available'
	when ac_mast.ac_name='S & H Cess on Service Tax Payable' then 'S & H Cess on Service Tax Available' 
	else ac_mast.ac_name end
,ac.amount,ac.amt_ty,ac.date
,sm.serty/*TKT-794 ac.serty*/
,m.sertype
,beh=(case when l.ext_vou=1 then l.bcode_nm else l.entry_ty end)
,typ=case 
	when ac_mast.typ='Service Tax Payable' then 'Service Tax Available' 
	when ac_mast.typ='Service Tax Payable-Ecess' then 'Service Tax Available-Ecess' 
	when ac_mast.typ='Service Tax Payable-Hcess' then 'Service Tax Available-Hcess' 
	else ac_mast.typ end
from SerTaxAcDet_vw ac 
inner join ac_mast ac_mast on (ac.ac_id=ac_mast.ac_id) 
inner join lcode l on (ac.entry_ty=l.entry_ty)
inner join #SerTax_Mast sm on (ac.serty=sm.[name])
left join SerTaxMain_vw m on (m.entry_ty=ac.entry_ty and m.tran_cd =ac.tran_cd)
inner join jvmain jvm on (jvm.entry_ty=ac.entry_ty and jvm.tran_cd =ac.tran_cd)
where ac_mast.ac_name like '%service tax Payable%'
and isnull(jvm.ser_adj,'')='Advance Adjustment' 
and (ac.u_cldt<= @edate)
order by ac.tran_cd,ac_mast.ac_name	
/*<---#st3_5b*/

--select * from #st3_5b where ac_name like 'service tax%'

select m.entry_ty,m.tran_cd,acl.Serty,Amount=sum(acl.Amount),sTaxable=sum(acl.sTaxable),SerBAmt=sum(acl.SerBAmt),SerCAmt=sum(acl.SerCAmt),SerHAmt=sum(acl.SerHAmt)
,SerrBAmt=sum(acl.SerrBAmt),SerrCAmt=sum(acl.SerrCAmt),SerrHAmt=sum(acl.SerrHAmt)--BUG6092
,sabtamt=sum(acl.sabtamt),sexpamt=sum(acl.sexpamt)
,sabtsr,sabtsrsl,ssubcls,sexnoti,sexnotisl,ItSerial
,serbcess=SUM(acl.serbcess),SERRBCESS=sum(acl.serrbcess)
into #acl1
from acdetalloc acl 
inner join SerTaxMain_vw m on (acl.entry_ty=m.entry_ty and acl.tran_cd=m.tran_cd)
where  m.date<@Edate and acl.SerTy<>'OTHERS' and ((acl.SerAvail='SERVICES') or (acl.SerAvail='EXCISE' and acl.Entry_ty = 'BR'))
group by m.entry_ty,m.tran_cd,acl.Serty,sabtsr,sabtsrsl,ssubcls,sexnoti,sexnotisl,ItSerial

--select 'a' as 'a',* from #acl1

select m.entry_ty,m.tran_cd,isd.Serty,Amount=sum(isd.Amount),sTaxable=sum(isd.sTaxable),SerBAmt=sum(isd.SerBAmt),SerCAmt=sum(isd.SerCAmt),SerHAmt=sum(isd.SerHAmt)
,serbcess=SUM(isd.serbcess),SERRBCESS=sum(isd.serrbcess)
into #isd1
from isdallocation isd 
inner join SerTaxMain_vw m on (isd.entry_ty=m.entry_ty and isd.tran_cd=m.tran_cd)
where m.date<@Edate 
group by m.entry_ty,m.tran_cd,isd.Serty

--select 'b',* from #isd1

select a.entry_ty,a.tran_cd,a.serty
,Amount=a.Amount-isnull(i.amount,0),sTaxable=a.sTaxable-isnull(i.sTaxable,0),SerBAmt=a.SerBAmt-isnull(i.SerBAmt,0),SerCAmt=a.SerCAmt-isnull(i.SerCAmt,0),SerHAmt=a.SerHAmt-isnull(i.SerHAmt,0)
,a.SerrBAmt,a.SerrCAmt,a.SerrHAmt	--BUG6092
,a.sabtamt,a.sexpamt
,a.sabtsr,a.sabtsrsl,a.ssubcls,a.sexnoti,a.sexnotisl,ItSerial
,serbcess=a.serbcess-ISNULL(i.serbcess,0),a.SERRBCESS
into #acl
from #acl1 a 
inner join SerTaxMain_vw m on (a.entry_ty=m.entry_ty and a.tran_cd=m.tran_cd)
left join #isd1 i on (a.entry_ty=i.entry_ty   and a.tran_cd=i.tran_cd and a.serty=i.serty)
where a.sTaxable-isnull(i.sTaxable,0)>0.5 
and  not (a.entry_ty in ('E1') and m.SerRule<>'IMPORT')	
--select 'c',* from #acl

select ---m.entry_ty,m.tran_cd,m.date,m.inv_no,acl.serty,rdate=m.date,m.SerRule,acl.Serty,acl.Amount,acl.sTaxable,acl.SerBAmt,acl.SerCAmt,acl.SerHAmt
m.inv_no
/*,acl.serty*/
,t.serty
,m.date
,m.tdspaytype
,gro_amt=acl.amount 
,taxable_amt=acl.staxable 
,acl.sabtamt
/*,m.serbper,m.sercper,m.serhper*/
,serbper=(case when ( m.entry_ty in ('S1','E1','IF','OF') ) then i.SerBPer else m.SerBPer end)
,sercper=(case when ( m.entry_ty in ('S1','E1','IF','OF') ) then i.SerCPer else m.SerCPer end)
,serhper=(case when ( m.entry_ty in ('S1','E1','IF','OF') ) then i.SerHPer else m.SerHPer end)
,acl.serbamt
,acl.sercamt
,acl.serhamt
,acl.serrbamt
,acl.serrcamt
,acl.serrhamt
,acl.entry_ty,acl.tran_cd,acserial=''
,m.serrule
,acl.SabtSr,acl.SabtSrSl,acl.SSubCls,acl.SExNoti,acl.SExNotiSl
/*,sProvider=case when (acl.entry_ty in ('E1','BP','CP') and m.SerRule  in ('IMPORT')) then 'No' else 'Yes' end 
,sReceiver=case when (acl.entry_ty in ('E1','BP','CP') and m.SerRule  in ('IMPORT')) then 'Yes' else 'No' end*/ /*TKT-8320 GTA */
,sProvider=case when ( (acl.entry_ty in ('E1','BP','CP') and m.SerRule  in ('IMPORT')) or (acl.entry_ty in ('IF','OF','B1','C1'))    ) then 'No' else 'Yes' end	
,sReceiver=case when ( (acl.entry_ty in ('E1','BP','CP') and m.SerRule  in ('IMPORT')) or (acl.entry_ty in ('IF','OF','B1','C1'))    ) then 'Yes' else 'No' end	
,sProvRev=(case when acl.entry_ty not in ('EP','B1','C1') and acl.SerRbAmt > 0 then 'YES' else 'NO' end)
,sReceRev=(case when acl.entry_ty in ('EP','B1','C1') and acl.SerRbAmt > 0 then 'YES' else 'NO' end)
,sProvPer= (case when acl.SerRbAmt > 0 then round((acl.SerbAmt*100)/(acl.SerbAmt+acl.SerRbAmt),0) else 0 end)
,sRecePer= (case when acl.SerRbAmt > 0 then round((acl.SerrbAmt*100)/(acl.SerbAmt+acl.SerRbAmt),0) else 0 end)
,sProvAmt= (case when acl.SerRbAmt > 0 then round((acl.staxable * round((acl.SerbAmt*100)/(acl.SerbAmt+acl.SerRbAmt),0))/100,2) else 0 end)
,sReceAmt= (case when acl.SerRbAmt > 0 then round((acl.staxable * round((acl.SerrbAmt*100)/(acl.SerbAmt+acl.SerRbAmt),0))/100,2) else 0 end)
,acl.sExpAmt
,serbcper=(case when ( m.entry_ty in ('S1','E1','IF','OF') ) then i.SERBCPER else m.serbcper end)
,acl.serbcess,acl.SERRBCESS
into #bracdet --used in 3F 
from #acl acl
inner join SerTaxMain_vw m on (acl.entry_ty=m.entry_ty and acl.tran_cd=m.tran_cd)
inner join ac_mast a on (m.ac_id=a.ac_id)
inner join #sertax_mast t on (acl.SerTy=t.[Name])
left join SerTaxItem_vw i on (acl.entry_ty=i.Entry_ty and acl.tran_cd=i.tran_cd and acl.ItSerial=i.ItSerial) /*TKT-8320 GTA*/
Where m.Date<=@eDate
and ( (acl.entry_ty in ('S1','BR','CR')) or 
(acl.entry_ty in ('E1','BP','CP') and m.SerRule  in ('IMPORT')) or	
 ( (acl.entry_ty in ('IF','OF','B1','C1') and acl.SerrBAmt <=0 ) ) /*TKT-8320 GTA*/	--BUG6092
)
Order by m.date,m.Entry_ty,M.Inv_No,acl.Serty


--select * from #bracdet --pankaj

	--select * from #bracdet
--BUG6092
select ColId = IDENTITY(INT,1,1),FullDuty=cast(0 as int),m.entry_ty,m.tran_cd,m.date,acl.itserial,acl.Serty,Amount=sum(acl.Amount),sTaxable=sum(acl.sTaxable)
,SAmount=sum(acl.Amount)
,SerBAmt=sum(case when m.entry_ty in ('IF','OF','B1','C1') then acl.SerBAmt else 0 end)
,SerCAmt=sum(case when m.entry_ty in ('IF','OF','B1','C1') then acl.SerCAmt else 0 end)
,SerHAmt=sum(case when m.entry_ty in ('IF','OF','B1','C1') then acl.SerHAmt else 0 end)
--Added by Shrikant S. on 19/04/2016 for Bug-27848		--Start
,SerrBAmt=sum(case when m.entry_ty in ('IF','OF','B1','C1') then acl.SerrBAmt else 0 end)		
,SerrCAmt=sum(case when m.entry_ty in ('IF','OF','B1','C1') then acl.SerrCAmt else 0 end)
,SerrHAmt=sum(case when m.entry_ty in ('IF','OF','B1','C1') then acl.SerrHAmt else 0 end)
--Added by Shrikant S. on 19/04/2016 for Bug-27848		--End

--Commented by Shrikant S. on 19/04/2016 for Bug-27848		--Start
--,SerrBAmt=sum(acl.SerrBAmt)
--,SerrCAmt=sum(acl.SerrCAmt)
--,SerrHAmt=sum(acl.SerrHAmt)
--Commented by Shrikant S. on 19/04/2016 for Bug-27848		--End

,sabtamt=sum(acl.sabtamt),sexpamt=sum(acl.sexpamt)
,sabtsr,sabtsrsl,ssubcls,sexnoti,sexnotisl
,serbcess=sum(case when m.entry_ty in ('IF','OF','B1','C1') then acl.serbcess else 0 end)
--,SERRBCESS=SUM(SERRBCESS)				--Commented by Shrikant S. on 19/04/2016 for Bug-27848		
,SERRBCESS=sum(case when m.entry_ty in ('IF','OF','B1','C1') then acl.SERRBCESS else 0 end)		--Added by Shrikant S. on 19/04/2016 for Bug-27848		
into #acl1a
from acdetalloc acl 
inner join SerTaxMain_vw m on (acl.entry_ty=m.entry_ty and acl.tran_cd=m.tran_cd)
where  m.date<@Edate and acl.SerTy<>'OTHERS' and acl.SerAvail='SERVICES'
and m.entry_ty in ('E1','IF','OF','BP','CP','B1','C1') and acl.SerrBAmt > 0
group by m.entry_ty,m.tran_cd,m.date,acl.itserial,acl.Serty,sabtsr,sabtsrsl,ssubcls,sexnoti,sexnotisl


select b.entry_ty,b.tran_cd,isd.Serty
,SAmount=sum(isnull(isd.Amount,0))		
,SerBAmt=sum(isnull(isd.SerBAmt,0))
,SerCAmt=sum(isnull(isd.SerCAmt,0))
,SerHAmt=sum(isnull(isd.SerHAmt,0))
,SerrBAmt=sum(isnull(isd.SerrBAmt,0))
,SerrCAmt=sum(isnull(isd.SerrCAmt,0))
,SerrHAmt=sum(isnull(isd.SerrHAmt,0))
,serbcess=sum(isnull(isd.SERBCESS,0))
,SERRBCESS=SUM(isnull(isd.serrbcess,0))
into #isd1a
from isdallocation isd 
inner join SerTaxMain_vw b on (isd.entry_ty=b.entry_ty and isd.tran_cd=b.tran_cd)
where b.date<@Edate 
group by b.entry_ty,b.tran_cd,isd.Serty


update #acl1a set 
	SAmount=  a.SAmount - isnull(i.SAmount,0)		
	,SerBAmt = a.SerBAmt - isnull(i.SerBAmt,0)
	,SerCAmt = a.SerCAmt - isnull(i.SerCAmt,0)
	,SerHAmt = a.SerHAmt - isnull(i.SerHAmt,0)
	,SerrBAmt = a.SerrBAmt - isnull(i.SerrBAmt,0)
	,SerrCAmt = a.SerrCAmt - isnull(i.SerrCAmt,0)
	,SerrHAmt = a.SerrHAmt - isnull(i.SerrHAmt,0)
	,serbcess=a.serbcess-isnull(i.serbcess,0)
	,SERRBCESS=a.SERRBCESS-isnull(i.SERRBCESS,0)
	from #acl1a a
	left join #isd1a i on (a.entry_ty=i.entry_ty  and a.tran_cd=i.tran_cd and a.serty=i.serty)
delete from #acl1a where SAmount <= 0




select m.entry_ty,m.tran_cd,isd.Serty
,SerrBAmt=sum(isnull(isd.SerrBAmt,0))
,SerrCAmt=sum(isnull(isd.SerrCAmt,0))
,SerrHAmt=sum(isnull(isd.SerrHAmt,0))
,SERRBCESS=sum(isnull(isd.SERRBCESS,0))
into #isd2a
from isdallocation isd 
inner join SerTaxMain_vw m on (isd.aentry_ty=m.entry_ty and isd.atran_cd=m.tran_cd)
inner join SerTaxMain_vw b on (isd.entry_ty=b.entry_ty and isd.tran_cd=b.tran_cd)
where m.date<@Edate and b.date<@Edate 
group by m.entry_ty,m.tran_cd,isd.Serty

update #acl1a set 
	FullDuty = (case when (@Edate-a.date) > 180 or a.entry_ty in ('BP','CP','B1','C1') then 1 else 0 end),
	SerRBAmt = (case when (@Edate-a.date) > 180 or a.entry_ty in ('BP','CP','B1','C1') then a.SerrBAmt else isnull(i.SerrBAmt,0) end),
	SerRCAmt = (case when (@Edate-a.date) > 180 or a.entry_ty in ('BP','CP','B1','C1') then a.SerrCAmt else isnull(i.SerrCAmt,0) end),
	SerrHAmt = (case when (@Edate-a.date) > 180 or a.entry_ty in ('BP','CP','B1','C1') then a.SerrHAmt else isnull(i.SerrHAmt,0) end), 
	Serrbcess = (case when (@Edate-a.date) > 180 or a.entry_ty in ('BP','CP','B1','C1') then a.Serrbcess else isnull(i.Serrbcess,0) end) 
	from #acl1a a
	left join #isd2a i on (a.entry_ty=i.entry_ty  and a.tran_cd=i.tran_cd and a.serty=i.serty)

delete from #acl1a where SerrBAmt <= 0 and SerBAmt <= 0

Select Min(ColId) as ColId,Count(ColId) as ColCnt,Entry_ty,Tran_cd,Serty into #acl2a from #acl1a group by Entry_ty,Tran_cd,Serty

update #acl1a set 
	SerRBAmt = 1, SerRCAmt = 0, SerrHAmt = 0,SERRBCESS=0 from #acl1a 
	where ColId not in (Select ColId from #acl2a) and FullDuty = 0

update #acl1a set 
	SerRBAmt = a.SerRBAmt-(b.ColCnt-1) from #acl1a a,#acl2a b
	where a.ColId = b.ColId and a.FullDuty = 0
	

	
select
m.inv_no
,acl.serty
,m.date
,m.tdspaytype
,gro_amt=acl.amount 
,taxable_amt=acl.staxable 
,acl.sabtamt
,serbper=(case when ( m.entry_ty in ('S1','E1','IF','OF') ) then i.SerBPer else m.SerBPer end)
,sercper=(case when ( m.entry_ty in ('S1','E1','IF','OF') ) then i.SerCPer else m.SerCPer end)
,serhper=(case when ( m.entry_ty in ('S1','E1','IF','OF') ) then i.SerHPer else m.SerHPer end)
,serbamt=acl.serrbamt+acl.serbamt
,sercamt=acl.serrcamt+acl.sercamt
,serhamt=acl.serrhamt+acl.serhamt
,acl.serrbamt
,acl.serrcamt
,acl.serrhamt
,acl.entry_ty,acl.tran_cd,acserial=''
,m.serrule
,acl.SabtSr,acl.SabtSrSl,acl.SSubCls,acl.SExNoti,acl.SExNotiSl
,sProvider='No'
,sReceiver='Yes'
,sProvRev=(case when m.entry_ty not in ('E1','IF','OF') and acl.SerRbAmt > 0 then 'YES' else 'NO' end)
,sReceRev=(case when m.entry_ty in ('E1','IF','OF') and acl.SerRbAmt > 0 then 'YES' else 'NO' end)
,sProvPer= (case when acl.SerRbAmt > 0 then round((acl.SerbAmt*100)/(acl.SerbAmt+acl.SerRbAmt),0) else 0 end)
,sRecePer= (case when acl.SerRbAmt > 0 then round((acl.SerrbAmt*100)/(acl.SerbAmt+acl.SerRbAmt),0) else 0 end)
,sProvAmt= (case when acl.SerRbAmt > 0 then round((acl.staxable * round((acl.SerbAmt*100)/(acl.SerbAmt+acl.SerRbAmt),0))/100,2) else 0 end)
,sReceAmt= (case when acl.SerRbAmt > 0 then round((acl.staxable * round((acl.SerrbAmt*100)/(acl.SerbAmt+acl.SerRbAmt),0))/100,2) else 0 end)
,acl.sExpAmt
,serbcper=(case when ( m.entry_ty in ('S1','E1','IF','OF') ) then i.serbcper else m.serbcper end)
,serbcess=acl.serbcess+acl.SERRBCESS
,acl.serrbcess
into #bracdet1a from #acl1a acl
inner join SerTaxMain_vw m on (acl.entry_ty=m.entry_ty and acl.tran_cd=m.tran_cd)
inner join ac_mast a on (m.ac_id=a.ac_id)
inner join #sertax_mast t on (acl.SerTy=t.[Name])
left join SerTaxItem_vw i on (acl.entry_ty=i.Entry_ty and acl.tran_cd=i.tran_cd and acl.ItSerial=i.ItSerial)
Where m.Date<=@eDate
Order by m.date,m.Entry_ty,M.Inv_No,acl.Serty




update #bracdet1a set serty = b.serty from #bracdet1a a,sertax_mast b where a.serty = b.name



Insert into #bracdet Select * from #bracdet1a


-----	select * from #bracdet		tdspaytype
--eFiling
--Update #bracdet Set sProvRev = 'Yes' Where sProvider='Yes' And		--eFilinga
--eFiling
--select * from #bracdet 
drop table #acl1a
drop table #acl2a
drop table #isd1a
drop table #isd2a
drop table #bracdet1a
--BUG6092
	
--select 'a1',* from #bracdet

--select 'a1' as a1 ,*
--			from #bracdet
--			where tdspaytype=1
--			and serty=@serty 
			--and sProvider=case when @sProvider ='Yes' then 'Yes' else 'No' end
			--and sReceiver=case when @sProvider ='Yes' then 'No' else 'Yes' end

update #bracdet set tdspaytype =1 where isnull(tdspaytype,0)=0
-- Pankaj

/*--->#bpacdet --used in 4A*/
select
ac.entry_ty,ac.tran_cd
,date=(case when isnull(ac.u_cldt,'')='' then ac.date else ac.u_cldt end)
,ac_mast.ac_name,amount,typ,beh=(case when l.ext_vou=1 then l.bcode_nm else l.entry_ty end)
,m.u_arrears,m.sertype,m.serty
,m.u_chalno,m.u_chaldt,er_adj=isnull(jvm.ser_adj,'')
,m.sDocNo,m.sDocDt
,ac.u_cldt
,m.TDSPayType,AC.AMT_TY
into #bpacdet
from SerTaxAcdet_vw ac
inner join SerTaxMain_vw m on (m.entry_ty=ac.entry_ty and m.tran_cd=ac.tran_cd)
left join jvmain jvm on (jvm.entry_ty=ac.entry_ty and jvm.tran_cd=ac.tran_cd)
inner join ac_mast on (ac_mast.ac_id=ac.ac_id)
inner join lcode l on (m.entry_ty=l.entry_ty)
where ac.amt_ty='DR' 
AND ac_mast.typ in ('Service Tax Payable','Service Tax Payable-Ecess','Service Tax Payable-Hcess','GTA Service Tax Payable','GTA Service Tax Payable-Ecess','GTA Service Tax Payable-Hcess','Service Tax Payable-SBcess','Swachh Bharat Cess') 
and ( ac.u_cldt between @sdate and @edate) 


--select * from #bpacdet



/*<---#bpacdet --used in 4A*/
update #bracdet set tdspaytype =1 where isnull(tdspaytype,0)=0

update #bracdet set taxable_amt=isnull(taxable_amt,0),serbper=isnull(serbper,0),serbamt=isnull(serbamt,0),sercper=isnull(sercper,0),sercamt=isnull(sercamt,0),serhper=isnull(serhper,0),serhamt=isnull(serhamt,0),serbcper=ISNULL(serbcper,0),serbcess=ISNULL(serbcess,0)


--select * from #bracdet

--Bug-24957 Strat
--update A set A.serbamt=A.serbamt+(select Isnull(sum(case when ac.typ in ('Service Tax Payable','GTA Service Tax Payable') then (case when b.amt_ty='CR' THEN isnull(B.amount,0) WHEN B.AMT_TY='DR' THEN -isnull(B.amount,0) else 0 end) ELSE 0 END),0)
--from  JVACDET B 
--inner join jvmain d on (d.tran_cd=b.tran_cd and b.entry_ty=d.entry_ty)
--inner join ac_mast ac on (ac.ac_id=b.ac_id)
--WHERE ac.typ in ('Service Tax Payable','GTA Service Tax Payable')
--AND (B.DATE BETWEEN @SDATE AND @EDATE)) from #bracdet A



--update A set A.sercamt=A.sercamt+(select Isnull(sum(case when ac.typ in ('Service Tax Payable-Ecess','GTA Service Tax Payable-Ecess') then (case when b.amt_ty='CR' THEN isnull(B.amount,0) WHEN B.AMT_TY='DR' THEN -isnull(B.amount,0) else 0 end) ELSE 0 END),0)
--from  JVACDET B 
--inner join jvmain d on (d.tran_cd=b.tran_cd and b.entry_ty=d.entry_ty)
--inner join ac_mast ac on (ac.ac_id=b.ac_id)
--WHERE ac.typ in ('Service Tax Payable-Ecess','GTA Service Tax Payable-Ecess')
--AND (B.DATE BETWEEN @SDATE AND @EDATE)) from #bracdet A


--update A set A.serhamt=A.serhamt+(select Isnull(sum(case when ac.typ in ('Service Tax Payable-Hcess','GTA Service Tax Payable-Hcess') then (case when b.amt_ty='CR' THEN isnull(B.amount,0) WHEN B.AMT_TY='DR' THEN -isnull(B.amount,0) else 0 end) ELSE 0 END),0)
--from  JVACDET B 
--inner join jvmain d on (d.tran_cd=b.tran_cd and b.entry_ty=d.entry_ty)
--inner join ac_mast ac on (ac.ac_id=b.ac_id)
--WHERE ac.typ in ('Service Tax Payable-Hcess','GTA Service Tax Payable-Hcess')
--AND (B.DATE BETWEEN @SDATE AND @EDATE)) from #bracdet A
----,A.sercamt=sum(case when B.ac_name='Edu. Cess on Service Tax Payable' then B.amount else 0 end),A.serhamt=sum(case when B.ac_name='S & H Cess on Service Tax Payable' then B.amount else 0 end) FROM  
----Bug-24957 End



--update A set A.serbcess=A.serbcess+(select Isnull(sum(case when ac.typ in ('Service Tax Payable-SBcess') then (case when b.amt_ty='CR' THEN isnull(B.amount,0) WHEN B.AMT_TY='DR' THEN -isnull(B.amount,0) else 0 end) ELSE 0 END),0)
--from  JVACDET B 
--inner join jvmain d on (d.tran_cd=b.tran_cd and b.entry_ty=d.entry_ty)
--inner join ac_mast ac on (ac.ac_id=b.ac_id)
--WHERE ac.typ in ('Service Tax Payable-SBcess')
--AND (B.DATE BETWEEN @SDATE AND @EDATE)) from #bracdet A




update #bpacdet set u_arrears = 'D3-Advance Service Tax utilized [Rule 6(1A)]' where typ in ('Service Tax Payable','GTA Service Tax Payable') and (u_arrears = 'Advance under Rule 6 (1A) of ST Rules' or sertype = 'Advance under Rule 6 (1A) of ST Rules')
update #bpacdet set u_arrears = 'E3-Advance Educess  utilized [Rule 6(1A)]' where typ in ('Service Tax Payable-Ecess','GTA Service Tax Payable-Ecess') and (u_arrears = 'Advance under Rule 6 (1A) of ST Rules' or sertype = 'Advance under Rule 6 (1A) of ST Rules')
update #bpacdet set u_arrears = 'F3-Advance Sec. and High. Educess  utilized [Rule 6(1A)]' where typ in ('Service Tax Payable-Hcess','GTA Service Tax Payable-Hcess') and (u_arrears = 'Advance under Rule 6 (1A) of ST Rules' or sertype = 'Advance under Rule 6 (1A) of ST Rules')

update #bpacdet set u_arrears = 'D7-Service Tax paid [Book Adjustment]' where typ in ('Service Tax Payable','GTA Service Tax Payable') and (u_arrears in ('Advance Adjustment','Book Adjustment') or er_adj in ('Advance Adjustment','Book Adjustment'))
update #bpacdet set u_arrears = 'E7-Educess  paid [Book Adjustment]' where typ in ('Service Tax Payable-Ecess','GTA Service Tax Payable-Ecess') and (u_arrears in ('Advance Adjustment','Book Adjustment') or er_adj in ('Advance Adjustment','Book Adjustment'))
update #bpacdet set u_arrears = 'F7-Sec. and High. Educess  paid [Book Adjustment]' where typ in ('Service Tax Payable-Hcess','GTA Service Tax Payable-Hcess') and (u_arrears in ('Advance Adjustment','Book Adjustment') or er_adj in ('Advance Adjustment','Book Adjustment'))

update #bpacdet set u_arrears = 'D4-Service Tax paid [Rule 6(3)]' where typ in ('Service Tax Payable','GTA Service Tax Payable') and u_arrears = 'under Rule 6 (3) of ST Rules'
update #bpacdet set u_arrears = 'E4-Educess  paid [Rule 6(3)]' where typ in ('Service Tax Payable-Ecess','GTA Service Tax Payable-Ecess') and u_arrears = 'under Rule 6 (3) of ST Rules'
update #bpacdet set u_arrears = 'F4-Sec. and High. Educess  paid [Rule 6(3)]' where typ in ('Service Tax Payable-Hcess','GTA Service Tax Payable-Hcess') and u_arrears = 'under Rule 6 (3) of ST Rules'

update #bpacdet set u_arrears = 'D5-Service Tax paid [Rule 6(4A)]' where typ in ('Service Tax Payable','GTA Service Tax Payable') and u_arrears = 'under Rule 6 (4A) of ST Rules'
update #bpacdet set u_arrears = 'E5-Educess  paid [Rule 6(4A)]' where typ in ('Service Tax Payable-Ecess','GTA Service Tax Payable-Ecess') and u_arrears = 'under Rule 6 (4A) of ST Rules'
update #bpacdet set u_arrears = 'F5-Sec. and High. Educess  paid [Rule 6(4A)]' where typ in ('Service Tax Payable-Hcess','GTA Service Tax Payable-Hcess') and u_arrears = 'under Rule 6 (4A) of ST Rules'

update #bpacdet set u_arrears = 'D6-Service Tax paid [Rule 6(4C)]' where typ in ('Service Tax Payable','GTA Service Tax Payable') and u_arrears = 'under Rule 6 (4C) of ST Rules'
update #bpacdet set u_arrears = 'E6-Educess  paid [Rule 6(4C)]' where typ in ('Service Tax Payable-Ecess','GTA Service Tax Payable-Ecess') and u_arrears = 'under Rule 6 (4C) of ST Rules'
update #bpacdet set u_arrears = 'F6-Sec. and High. Educess  paid [Rule 6(4C)]' where typ in ('Service Tax Payable-Hcess','GTA Service Tax Payable-Hcess') and u_arrears = 'under Rule 6 (4C) of ST Rules'

update #bpacdet set u_arrears = 'G1-Other amounts paid - Arrears Cash' where typ in ('Service Tax Payable','GTA Service Tax Payable') and u_arrears in ('Arrears of revenue paid in cash','Arrears of education cess paid in cash (Differential of Cess)','Arrears of education cess paid in cash (Differantial of Cess)','G5-Other amounts paid - Arrears (Sec and Higher Educess) Cash')
update #bpacdet set u_arrears = 'G2-Other amounts paid - Arrears Credit' where typ in ('Service Tax Payable','GTA Service Tax Payable') and u_arrears in ('Arrears of revenue paid by credit',',Arrears of education cess paid by credit',',Arrears of Sec & higher edu cess paid by credit')
update #bpacdet set u_arrears = 'G3-Other amounts paid - Arrears (Educess) Cash' where typ in ('Service Tax Payable-Ecess','GTA Service Tax Payable-Ecess') and u_arrears in ('Arrears of revenue paid in cash','Arrears of education cess paid in cash (Differential of Cess)','Arrears of education cess paid in cash (Differantial of Cess)','G5-Other amounts paid - Arrears (Sec and Higher Educess) Cash')
update #bpacdet set u_arrears = 'G4-Other amounts paid - Arrears (Educess) Credit' where typ in ('Service Tax Payable-Ecess','GTA Service Tax Payable-Ecess') and u_arrears in ('Arrears of revenue paid by credit',',Arrears of education cess paid by credit',',Arrears of Sec & higher edu cess paid by credit')
update #bpacdet set u_arrears = 'G5-Other amounts paid - Arrears (Sec and Higher Educess) Cash' where typ in ('Service Tax Payable-Hcess','GTA Service Tax Payable-Hcess') and u_arrears in ('Arrears of revenue paid in cash','Arrears of education cess paid in cash (Differential of Cess)','Arrears of education cess paid in cash (Differantial of Cess)','G5-Other amounts paid - Arrears (Sec and Higher Educess) Cash')
update #bpacdet set u_arrears = 'G6-Other amounts paid - Arrears (Sec and Higher Educess) Credit' where typ in ('Service Tax Payable-Hcess','GTA Service Tax Payable-Hcess') and u_arrears in ('Arrears of revenue paid by credit',',Arrears of education cess paid by credit',',Arrears of Sec & higher edu cess paid by credit')

update #bpacdet set u_arrears = 'G7-Other amounts paid - [Rule (73A)]' where typ in ('Service Tax Payable','GTA Service Tax Payable') and u_arrears = 'Section 73A amount paid'
update #bpacdet set u_arrears = 'G7-Other amounts paid - [Rule (73A)]' where typ in ('Service Tax Payable-Ecess','GTA Service Tax Payable-Ecess') and u_arrears = 'Section 73A amount paid'
update #bpacdet set u_arrears = 'G7-Other amounts paid - [Rule (73A)]' where typ in ('Service Tax Payable-Hcess','GTA Service Tax Payable-Hcess') and u_arrears = 'Section 73A amount paid'

update #bpacdet set u_arrears = 'G8-Other amounts paid - Interest' where typ in ('Service Tax Payable','GTA Service Tax Payable') and u_arrears = 'Interest paid'
update #bpacdet set u_arrears = 'G8-Other amounts paid - Interest' where typ in ('Service Tax Payable-Ecess','GTA Service Tax Payable-Ecess') and u_arrears = 'Interest paid'
update #bpacdet set u_arrears = 'G8-Other amounts paid - Interest' where typ in ('Service Tax Payable-Hcess','GTA Service Tax Payable-Hcess') and u_arrears = 'Interest paid'

update #bpacdet set u_arrears = 'G9-Other amounts paid - Penalty' where typ in ('Service Tax Payable','GTA Service Tax Payable') and u_arrears = 'Penalty paid'
update #bpacdet set u_arrears = 'G9-Other amounts paid - Penalty' where typ in ('Service Tax Payable-Ecess','GTA Service Tax Payable-Ecess') and u_arrears = 'Penalty paid'
update #bpacdet set u_arrears = 'G9-Other amounts paid - Penalty' where typ in ('Service Tax Payable-Hcess','GTA Service Tax Payable-Hcess') and u_arrears = 'Penalty paid'

update #bpacdet set u_arrears = 'G10-Other amounts paid - (Late Fee)' where typ in ('Service Tax Payable','GTA Service Tax Payable') and u_arrears = 'Late Fee paid'
update #bpacdet set u_arrears = 'G10-Other amounts paid - (Late Fee)' where typ in ('Service Tax Payable-Ecess','GTA Service Tax Payable-Ecess') and u_arrears = 'Late Fee paid'
update #bpacdet set u_arrears = 'G10-Other amounts paid - (Late Fee)' where typ in ('Service Tax Payable-Hcess','GTA Service Tax Payable-Hcess') and u_arrears = 'Late Fee paid'

update #bpacdet set u_arrears = 'G11-Any other amounts paid' where typ in ('Service Tax Payable','GTA Service Tax Payable') and u_arrears = 'Any other amount'
update #bpacdet set u_arrears = 'G11-Any other amounts paid' where typ in ('Service Tax Payable-Ecess','GTA Service Tax Payable-Ecess') and u_arrears = 'Any other amount'
update #bpacdet set u_arrears = 'G11-Any other amounts paid' where typ in ('Service Tax Payable-Hcess','GTA Service Tax Payable-Hcess') and u_arrears = 'Any other amount'
--



	
declare @amt0 decimal(17,2),@amt1 decimal(17,2),@amt2 decimal(17,2),@amt3 decimal(17,2),@amt4 decimal(17,2),@amt5 decimal(17,2),@amt6 decimal(17,2),@serty varchar(500)
Declare @sProvider varchar(3),@sReceiver varchar(3),@ExpAmt Numeric(17,2),@SabtNoti varchar(13),@SabtSr varchar(13),@SabtSrSl varchar(15),@SSubCls varchar(13),@SExNoti varchar(13),@SExNotiSl varchar(5)
Declare @sProvRev varchar(3),@sReceRev varchar(3),@sProvPer decimal(6,2),@sRecePer decimal(6,2)
declare @SERBPER decimal(6,2),@SERBAMT decimal(17,2),@SERCPER decimal(6,2),@SERCAMT decimal(17,2),@SERHPER decimal(6,2),@SERHAMT decimal(17,2),@SERBCPER decimal(6,2),@SERBCESS Decimal(17,2)

select particulars=space(250),serty=SPACE(500),srno1='AA',srno2='AA',srno3='AA',srno4='AA',srno5='AA',srno6='AA'
,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
,sdate1=@sdate,sdate2=@sdate,sdate3=@sdate,sdate4=@sdate,sdate5=@sdate,sdate6=@sdate
,amt1=net_amt,amt2=net_amt,amt3=net_amt,amt4=net_amt,amt5=net_amt,amt6=net_amt
,chalno1=space(20),chaldt1 =cast('' as smalldatetime),chalno2=space(20),chaldt2 =cast('' as smalldatetime),chalno3=space(20),chaldt3 =cast('' as smalldatetime),chalno4=space(20),chaldt4 =cast('' as smalldatetime),chalno5=space(20),chaldt5 =cast('' as smalldatetime),chalno6=space(20),chaldt6 =cast('' as smalldatetime)
,SProvider=space(3),SReceiver=space(3),SabtNoti =Space(13),SabtSr=space(13),SabtSrSl=space(15),SSubCls = space(13),SExNoti =space(13),SExNotiSl =space(5)
,sDocNo=space(50),sDocDt=m.date
,sProvRev=space(3),sReceRev=space(3)	--eFiling
,sProvPer=SERBPER,sRecePer=SERBPER	--eFiling
into #st3 
from sbmain m
inner join stax_mas st on (m.tax_name=st.tax_name)
where 1=2



/*3F--->*/
select distinct sm.serty,sProvider,sReceiver,SabtSr,SabtSrSl,SSubCls,SExNoti,SExNotiSl
,sProvRev,sReceRev	--eFiling
,sProvPer,sRecePer	--eFiling
,sProvAmt,sReceAmt
into #br3a
from #bracdet
inner join #SerTax_Mast sm on (#bracdet.serty=sm.[name])
union /*Don't use union all*/
select distinct sm.serty
,sProvider=(case when m.entry_ty in ('EP','B1','C1') then 'NO' else 'YES' end)
,sReceiver=(case when m.entry_ty in ('EP','B1','C1') then 'YES' else 'NO' end),acl.SabtSr,acl.SabtSrSl,acl.SSubCls,acl.SExNoti,acl.SExNotiSl/*acl.serty TKT-794 GTA*/
,sProvRev=(case when m.entry_ty not in ('EP','B1','C1') and acl.SerRbAmt > 0 then 'YES' else 'NO' end)
,sReceRev=(case when m.entry_ty in ('EP','B1','C1') and acl.SerRbAmt > 0 then 'YES' else 'NO' end)
,sProvPer= (case when acl.SerRbAmt > 0 then round((acl.SerbAmt*100)/(acl.SerbAmt+acl.SerRbAmt),0) else 0 end)
,sRecePer= (case when acl.SerRbAmt > 0 then round((acl.SerrbAmt*100)/(acl.SerbAmt+acl.SerRbAmt),0) else 0 end)
,sProvAmt= (case when acl.SerRbAmt > 0 then round((acl.staxable * round((acl.SerbAmt*100)/(acl.SerbAmt+acl.SerRbAmt),0))/100,2) else 0 end)
,sReceAmt= (case when acl.SerRbAmt > 0 then round((acl.staxable * round((acl.SerrbAmt*100)/(acl.SerbAmt+acl.SerRbAmt),0))/100,2) else 0 end)
from acdetalloc acl 
inner join sbmain m on (m.entry_ty=acl.entry_ty and m.tran_cd=acl.tran_cd)
inner join #SerTax_Mast sm on (acl.serty=sm.[name])
where acl.serty<>'' and  (date between @sdate and @edate) /*Used for Export Service*/

update #bracdet set sProvAmt = IsNull(sProvAmt,0), sReceAmt = IsNull(sReceAmt,0)
--print 'zz'
set @serty=''

select top 1 @serty=serty from  #br3a
if isnull(@serty,'')=''
begin
	insert into #br3a (serty,sProvider,sReceiver,SabtSr,SabtSrSl,SSubCls,SExNoti,SExNotiSl,sProvRev,sReceRev,sProvPer,sRecePer,sProvAmt,sReceAmt) 
	values ('z','Yes','','','','','','','','',0,0,0,0)
end
--select distinct 'a',* from #br3a order by serty
--select distinct 'b',serty from #br3a order by serty


declare cur_serty cursor for
select distinct serty from #br3a order by serty
open cur_serty
fetch next from cur_serty into @serty

while (@@fetch_status=0)
begin
	print 'aaa '+@serty
	Set @sSubCls = ''
	Select Top 1 @sSubCls = sSubCls From #bracdet where serty=@serty And IsNull(sSubCls ,'') <> ''

	/*-->3A to E*/
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	,SabtNoti,SabtSr,SSubCls,SExNoti,sProvider,sReceiver
	,SabtSrSl,SExNotiSl
	)
	values
	('',@serty,'3','A','10','0','','z'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	,'','',@sSubCls,'','',''		--BUG21952
	,'',''
	)
	

	set @sProvider='No'
	if exists(select serty from #br3a where sProvider='Yes' and serty=@serty )
	begin
		set @sProvider='Yes'
	end
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6,SabtNoti,SabtSr,SSubCls,SExNoti,sProvider,sReceiver
	,SabtSrSl,SExNotiSl
	)
	values
	('',@serty,'3','A','10','1','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	,'','',@sSubCls,'',@sProvider,''
	,'',''
	)


	set @sReceiver='No'
	if exists(select serty from #br3a where sReceiver='Yes' and serty=@serty)
	begin
		set @sReceiver='Yes'
	end

	set @sProvRev='No'
	if exists(select serty from #br3a where sProvider='Yes' and sProvRev='Yes' and serty=@serty )
	begin
		set @sProvRev='Yes'
	end

	set @sReceRev='No'
	if exists(select serty from #br3a where sReceiver='Yes' and sReceRev='Yes' and serty=@serty)
	begin
		set @sReceRev='Yes'
	end

	set @sProvPer=0
	select @sProvPer=sProvPer from #br3a where sProvider='Yes' and sProvRev='Yes' and serty=@serty 

	set @sRecePer=0
	select @sRecePer=sRecePer from #br3a where sReceiver='Yes' and sReceRev='Yes' and serty=@serty 

	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6,SabtNoti,SabtSr,SSubCls,SExNoti,sProvider,sReceiver
	,sProvRev,sReceRev,sProvPer,sRecePer
	,SabtSrSl,SExNotiSl
	)
	values
	('',@serty,'3','A','10','2','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	,'','',@sSubCls,'','',@sReceiver
	,@sProvRev,@sReceRev,@sProvPer,@sRecePer
	,'',''
	)

	set @CommanNotivar='No'
	if exists(select serty from #br3a where isnull(SExNoti,'')<>'' and serty=@serty)
	begin
		set @CommanNotivar='Yes'
	end

	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6,SabtNoti,SabtSr,SSubCls,SExNoti,sProvider,sReceiver
	,SabtSrSl,SExNotiSl
	)
	values
	('',@serty,'3','A','11','1','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	,'','','',@CommanNotivar,'',''
	,'',''
	)

	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6,SabtNoti,SabtSr,SSubCls,SExNoti,sProvider,sReceiver
	,SabtSrSl,SExNotiSl
	)
	select '',@serty,'3','A','11','2','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	,'','','',SExNoti,'',''	
	,'',SExNotiSl
	from #br3a
	where isnull(SExNoti,'')<>'' and serty=@serty

	set @CommanNotivar='No'
	if exists(select serty from #br3a where isnull(SabtSr,'')<>'' and serty=@serty)
	begin
		set @CommanNotivar='Yes'
	end
	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6,SabtNoti,SabtSr,SSubCls,SExNoti,sProvider,sReceiver
	,SabtSrSl,SExNotiSl
	)
	Values
	('',@serty,'3','A','12','1','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	,'',@CommanNotivar,'','','',''
	,'',''
	)
		
	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6,SabtNoti,SabtSr,SSubCls,SExNoti,sProvider,sReceiver
	,SabtSrSl,SExNotiSl
	)
	select '',@serty,'3','A','12','2','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	,'',SabtSr,'','','',''
	,SabtSrSl,''
	from #br3a
	where isnull(SabtSr,'')<>'' and serty=@serty
	
	--Provisional Assessment Challan Details
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0	
	set @CommanNotivar='No'

	--Select * From #bracdet

	declare cur_st3_2 cursor for
	select sum(amount) as amount,date,u_chalno,u_chaldt
	from #bpacdet 
	where ((isnull(u_chalno,'')<>'') or (isnull(u_chaldt,'')<>'') )
	and isnull(u_arrears,'') in ('D5-Service Tax paid [Rule 6(4A)]','E5-Educess  paid [Rule 6(4A)]','F5-Sec. and High. Educess  paid [Rule 6(4A)]')
	and serty = @serty
	group by date,u_chalno,u_chaldt
	
	open cur_st3_2
	fetch next from cur_st3_2 into @amt1,@date,@u_chalno,@u_chaldt
	
	set @c=0
	set @particulars1=' '
	while(@@fetch_status=0)
	begin
		set @CommanNotivar='Yes'
		set @c=@c+1
		select @particulars=(case when @c=1 then '(i)' else (case when @c=2 then '(ii)' else (case when @c=3 then '(iii)' else (case when @c=4 then '(iv)' else (case when @c=5 then '(v)' else (case when @c=6 then '(vi)' else (case when @c=7 then '(vii)' else (case when @c=8 then '(viii)' else (case when @c=9 then '(ix)' else (case when @c=10 then '(x)' else '' end) end) end) end) end) end) end) end) end) end)
		insert into #st3
		(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
		,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
		,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
		,amt1,amt2,amt3,amt4,amt5,amt6
		,chalno1,chaldt1,chalno2,chaldt2,chalno3,chaldt3,chalno4,chaldt4,chalno5,chaldt5,chalno6,chaldt6
		)
		values
		('',@serty,'3','A','13','2','1',''
		,0,0,0,0,0,0,0,0
		,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
		,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
		,(case when (@date between @sdate1 and @edate6) then isnull(@u_chalno,'') else '' end)
		,(case when (@date between @sdate1 and @edate6) then isnull(@u_chaldt,'') else '' end)
		,(case when (@date between @sdate2 and @edate2) then isnull(@u_chalno,'') else '' end)
		,(case when (@date between @sdate2 and @edate2) then isnull(@u_chaldt,'') else '' end)
		,(case when (@date between @sdate3 and @edate3) then isnull(@u_chalno,'') else '' end)
		,(case when (@date between @sdate3 and @edate3) then isnull(@u_chaldt,'') else '' end)
		,(case when (@date between @sdate4 and @edate4) then isnull(@u_chalno,'') else '' end)
		,(case when (@date between @sdate4 and @edate4) then isnull(@u_chaldt,'') else '' end)
		,(case when (@date between @sdate5 and @edate5) then isnull(@u_chalno,'') else '' end)
		,(case when (@date between @sdate5 and @edate5) then isnull(@u_chaldt,'') else '' end)
		,(case when (@date between @sdate6 and @edate6) then isnull(@u_chalno,'') else '' end)
		,(case when (@date between @sdate6 and @edate6) then isnull(@u_chaldt,'') else '' end)
		)
	
		fetch next from cur_st3_2 into @amt1,@date,@u_chalno,@u_chaldt
	end
	close cur_st3_2
	deallocate cur_st3_2

	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6,SabtNoti,SabtSr,SSubCls,SExNoti,sProvider,sReceiver
	,SabtSrSl,SExNotiSl
	)
	values
	('',@serty,'3','A','13','1','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	,'','','',@CommanNotivar,'',''
	,'',''
	)
	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6,SabtNoti,SabtSr,SSubCls,SExNoti,sProvider,sReceiver
	,SabtSrSl,SExNotiSl
	)
	values
	('',@serty,'3','A','13','2','0',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	,'','','','','',''
	,'',''
	)	

	/*<--3A to E*/
	/*--->3F*/
		declare @i int 
		set @i=0
		while @i<=1
		begin
			set @i=@i+1
			select  @sProvider='No',@sReceiver='No'

			if @i=1 begin set @sProvider='Yes' end   else  begin set @sReceiver='Yes'end
			
			/*-->3F 1*/
			select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
			select 
			 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(gro_amt,0)-(sProvAmt+sReceAmt) else 0 end)/*taxable_amt*/
			,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(gro_amt,0)-(sProvAmt+sReceAmt) else 0 end)
			,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(gro_amt,0)-(sProvAmt+sReceAmt) else 0 end)
			,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(gro_amt,0)-(sProvAmt+sReceAmt) else 0 end)
			,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(gro_amt,0)-(sProvAmt+sReceAmt) else 0 end)
			,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(gro_amt,0)-(sProvAmt+sReceAmt) else 0 end)
			from #bracdet
			where tdspaytype=1 and SerRule<>'IMPORT' and SerRBAmt = 0
			and serty=@serty 
			and sProvider=case when @sProvider ='Yes' then 'Yes' else 'No' end
			and sReceiver=case when @sProvider ='Yes' then 'No' else 'Yes' end
			
			select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
			insert into #st3
			(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
			,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
			,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
			,amt1,amt2,amt3,amt4,amt5,amt6,sProvider,sReceiver
			)
			values
			('',@serty,'3','B','1',' 1','0',''
			,0,0,0,0,0,0,0,0
			,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
			,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6,@sProvider,@sReceiver
			)

			select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
			select 
			 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(gro_amt,0)-(sProvAmt+sReceAmt) else 0 end)/*taxable_amt*/
			,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(gro_amt,0)-(sProvAmt+sReceAmt) else 0 end)
			,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(gro_amt,0)-(sProvAmt+sReceAmt) else 0 end)
			,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(gro_amt,0)-(sProvAmt+sReceAmt) else 0 end)
			,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(gro_amt,0)-(sProvAmt+sReceAmt) else 0 end)
			,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(gro_amt,0)-(sProvAmt+sReceAmt) else 0 end)
			from #bracdet
			where tdspaytype=2 and SerRule<>'IMPORT' and SerRBAmt = 0
			and serty=@serty 
			and sProvider=case when @sProvider ='Yes' then 'Yes' else 'No' end
			and sReceiver=case when @sProvider ='Yes' then 'No' else 'Yes' end
			
			select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
			insert into #st3
			(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
			,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
			,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
			,amt1,amt2,amt3,amt4,amt5,amt6,sProvider,sReceiver
			)
			values
			('',@serty,'3','B','1',' 2','0',' '
			,0,0,0,0,0,0,0,0
			,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
			,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6,@sProvider,@sReceiver
			)

			select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
			insert into #st3
			(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
			,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
			,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
			,amt1,amt2,amt3,amt4,amt5,amt6,sProvider,sReceiver
			)
			values
			('',@serty,'3','B','1',' 3','0',' '
			,0,0,0,0,0,0,0,0
			,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
			,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6,@sProvider,@sReceiver
			)

			select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
			insert into #st3
			(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
			,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
			,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
			,amt1,amt2,amt3,amt4,amt5,amt6,sProvider,sReceiver
			)
			values
			('',@serty,'3','B','1',' 4','0',' '
			,0,0,0,0,0,0,0,0
			,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
			,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6,@sProvider,@sReceiver
			)
			
			select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
			select 
			 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(gro_amt,0) else 0 end)/*taxable_amt*/
			,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(gro_amt,0) else 0 end)/*taxable_amt*/
			,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(gro_amt,0) else 0 end)/*taxable_amt*/
			,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(gro_amt,0) else 0 end)/*taxable_amt*/
			,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(gro_amt,0) else 0 end)/*taxable_amt*/
			,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(gro_amt,0) else 0 end)/*taxable_amt*/
			from #bracdet
			where SerRule='IMPORT'
			and serty=@serty
			and sProvider=case when @sProvider ='Yes' then 'Yes' else 'No' end
			and sReceiver=case when @sProvider ='Yes' then 'No' else 'Yes' end

			select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)
			insert into #st3
			(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
			,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
			,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
			,amt1,amt2,amt3,amt4,amt5,amt6,sProvider,sReceiver
			)
			values
			('',@serty,'3','B','1',' 5','0',' '
			,0,0,0,0,0,0,0,0
			,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
			,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6,@sProvider,@sReceiver
			)
			
			select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
			if @sProvider ='Yes'
			begin
				select 
				 @amt1=sum(case when (date between @sdate1 and @edate1) then (sProvAmt+sReceAmt) else 0 end)/*taxable_amt*/
				,@amt2=sum(case when (date between @sdate2 and @edate2) then (sProvAmt+sReceAmt) else 0 end)
				,@amt3=sum(case when (date between @sdate3 and @edate3) then (sProvAmt+sReceAmt) else 0 end)
				,@amt4=sum(case when (date between @sdate4 and @edate4) then (sProvAmt+sReceAmt) else 0 end)
				,@amt5=sum(case when (date between @sdate5 and @edate5) then (sProvAmt+sReceAmt) else 0 end)
				,@amt6=sum(case when (date between @sdate6 and @edate6) then (sProvAmt+sReceAmt) else 0 end)
				from #bracdet
				where SerRBAmt <> 0
				and serty=@serty 
				and sProvider=case when @sProvider ='Yes' then 'Yes' else 'No' end
				and sReceiver=case when @sProvider ='Yes' then 'No' else 'Yes' end
			end
			select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)							
			insert into #st3
			(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
			,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
			,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
			,amt1,amt2,amt3,amt4,amt5,amt6,sProvider,sReceiver
			)
			values
			('',@serty,'3','B','1',' 6','0',' '
			,0,0,0,0,0,0,0,0
			,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
			,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6,@sProvider,@sReceiver
			)

			select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
			if @sProvider ='Yes'
			begin
				select 
				 @amt1=sum(amt1)
				,@amt2=sum(amt2)
				,@amt3=sum(amt3)
				,@amt4=sum(amt4)
				,@amt5=sum(amt5)
				,@amt6=sum(amt6)
				from #st3
				where ltrim(rtrim(srno1)) = '3' and srno2 = 'B' and ltrim(rtrim(srno3)) = '1'
				and serty=@serty 
				and sProvider=case when @sProvider ='Yes' then 'Yes' else 'No' end
				and sReceiver=case when @sProvider ='Yes' then 'No' else 'Yes' end
			end	
			else
			begin
				select 
				 @amt1=sum(case when (date between @sdate1 and @edate1) then (sProvAmt+sReceAmt) else 0 end)/*taxable_amt*/
				,@amt2=sum(case when (date between @sdate2 and @edate2) then (sProvAmt+sReceAmt) else 0 end)
				,@amt3=sum(case when (date between @sdate3 and @edate3) then (sProvAmt+sReceAmt) else 0 end)
				,@amt4=sum(case when (date between @sdate4 and @edate4) then (sProvAmt+sReceAmt) else 0 end)
				,@amt5=sum(case when (date between @sdate5 and @edate5) then (sProvAmt+sReceAmt) else 0 end)
				,@amt6=sum(case when (date between @sdate6 and @edate6) then (sProvAmt+sReceAmt) else 0 end)
				from #bracdet
				where SerRBAmt <> 0
				and serty=@serty 
				and sProvider=case when @sProvider ='Yes' then 'Yes' else 'No' end
				and sReceiver=case when @sProvider ='Yes' then 'No' else 'Yes' end
			end	
			
			select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
			insert into #st3
			(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
			,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
			,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
			,amt1,amt2,amt3,amt4,amt5,amt6,sProvider,sReceiver
			)
			values
			('',@serty,'3','B','1',' 7','0',' '
			,0,0,0,0,0,0,0,0
			,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
			,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6,@sProvider,@sReceiver
			)
			
			select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
			if @sProvider ='Yes'
			begin
				select 
				 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(taxable_amt,0) else 0 end)
				,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(taxable_amt,0) else 0 end)
				,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(taxable_amt,0) else 0 end)
				,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(taxable_amt,0) else 0 end)
				,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(taxable_amt,0) else 0 end)
				,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(taxable_amt,0) else 0 end)
				from #bracdet
				where serrule='EXPORT'
				and serty=@serty
				and sProvider=case when @sProvider ='Yes' then 'Yes' else 'No' end
				and sReceiver=case when @sProvider ='Yes' then 'No' else 'Yes' end
			end	
			else
			begin
				select 
				 @amt1=sum(amt1)
				,@amt2=sum(amt2)
				,@amt3=sum(amt3)
				,@amt4=sum(amt4)
				,@amt5=sum(amt5)
				,@amt6=sum(amt6)
				from #st3
				where ltrim(rtrim(srno1)) = '3' and srno2 = 'B' and ltrim(rtrim(srno3)) = '1'
				and serty=@serty 
				and sProvider=case when @sProvider ='Yes' then 'Yes' else 'No' end
				and sReceiver=case when @sProvider ='Yes' then 'No' else 'Yes' end
			end	
			
			declare @SrNo4 varchar(2),@srno5 varchar(2)

			select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
			insert into #st3
			(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
			,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
			,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
			,amt1,amt2,amt3,amt4,amt5,amt6,sProvider,sReceiver
			)
			values
			('',@serty,'3','B','1',' 8','0',' '
			,0,0,0,0,0,0,0,0
			,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
			,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6,@sProvider,@sReceiver
			)
			
	
			select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
			--if @sProvider ='Yes'
			--begin
				select 
				 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(taxable_amt,0) else 0 end)
				,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(taxable_amt,0) else 0 end)
				,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(taxable_amt,0) else 0 end)
				,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(taxable_amt,0) else 0 end)
				,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(taxable_amt,0) else 0 end)
				,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(taxable_amt,0) else 0 end)
				from #bracdet
				where serrule='EXEMPT'
				and serty=@serty
				and sProvider=case when @sProvider ='Yes' then 'Yes' else 'No' end
				and sReceiver=case when @sProvider ='Yes' then 'No' else 'Yes' end
			--end	

			select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
			insert into #st3
			(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
			,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
			,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
			,amt1,amt2,amt3,amt4,amt5,amt6,sProvider,sReceiver
			)
			values
			('',@serty,'3','B','1',' 9','0',' '
			,0,0,0,0,0,0,0,0
			,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
			,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6,@sProvider,@sReceiver
			)
			
			--select @srno5 = case when @sProvider='Yes' then '3' else '2' end
			--set @srno5=isnull(@srno5,'')
			select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
			--if @sProvider ='Yes'
			--begin
				select 
				 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(sExpAmt,0) else 0 end)
				,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(sExpAmt,0) else 0 end)
				,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(sExpAmt,0) else 0 end)
				,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(sExpAmt,0) else 0 end)
				,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(sExpAmt,0) else 0 end)
				,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(sExpAmt,0) else 0 end)
				from #bracdet
				where serrule='PURE AGENT'
				and serty=@serty
				and sProvider=case when @sProvider ='Yes' then 'Yes' else 'No' end
				and sReceiver=case when @sProvider ='Yes' then 'No' else 'Yes' end			
			--end	

			select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
			insert into #st3
			(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
			,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
			,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
			,amt1,amt2,amt3,amt4,amt5,amt6,sProvider,sReceiver
			)
			values
			('',@serty,'3','B','1','10','0',' '
			,0,0,0,0,0,0,0,0
			,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
			,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6,@sProvider,@sReceiver
			)

		
			select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
			select 
			 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(Gro_Amt-taxable_amt-sExpAmt,0) else 0 end)
			,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(Gro_Amt-taxable_amt-sExpAmt,0) else 0 end)
			,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(Gro_Amt-taxable_amt-sExpAmt,0) else 0 end)
			,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(Gro_Amt-taxable_amt-sExpAmt,0) else 0 end)
			,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(Gro_Amt-taxable_amt-sExpAmt,0) else 0 end)
			,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(Gro_Amt-taxable_amt-sExpAmt,0) else 0 end)
			from #bracdet
			where serty=@serty
			and sProvider=case when @sProvider ='Yes' then 'Yes' else 'No' end
			and sReceiver=case when @sProvider ='Yes' then 'No' else 'Yes' end
					
			select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
			
			insert into #st3
			(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
			,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
			,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
			,amt1,amt2,amt3,amt4,amt5,amt6,sProvider,sReceiver
			)
			values
			('',@serty,'3','B','1','11','0',' '
			,0,0,0,0,0,0,0,0
			,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
			,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6,@sProvider,@sReceiver
			)

			select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
			insert into #st3
			(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
			,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
			,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
			,amt1,amt2,amt3,amt4,amt5,amt6,sProvider,sReceiver
			)
			values
			('',@serty,'3','B','1','12','0',' '
			,0,0,0,0,0,0,0,0
			,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
			,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6,@sProvider,@sReceiver
			)

			select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
			if @sProvider ='Yes'
			begin
				select 
				 @amt1=sum(amt1)
				,@amt2=sum(amt2)
				,@amt3=sum(amt3)
				,@amt4=sum(amt4)
				,@amt5=sum(amt5)
				,@amt6=sum(amt6)
				from #st3
				where ltrim(rtrim(srno1)) = '3' and srno2 = 'B' and ltrim(rtrim(srno3)) = '1' and ltrim(rtrim(srno4)) in ('8','9','10','11','12')
				and serty=@serty 
				and sProvider=case when @sProvider ='Yes' then 'Yes' else 'No' end
				and sReceiver=case when @sProvider ='Yes' then 'No' else 'Yes' end
			end	
			else
			begin
				select 
				 @amt1=sum(amt1)
				,@amt2=sum(amt2)
				,@amt3=sum(amt3)
				,@amt4=sum(amt4)
				,@amt5=sum(amt5)
				,@amt6=sum(amt6)
				from #st3
				where ltrim(rtrim(srno1)) = '3' and srno2 = 'B' and ltrim(rtrim(srno3)) = '1' and ltrim(rtrim(srno4)) in ('9','10','11','12')
				and serty=@serty 
				and sProvider=case when @sProvider ='Yes' then 'Yes' else 'No' end
				and sReceiver=case when @sProvider ='Yes' then 'No' else 'Yes' end
			end	
			select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
			
			insert into #st3
			(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
			,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
			,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
			,amt1,amt2,amt3,amt4,amt5,amt6,sProvider,sReceiver
			)
			values
			('',@serty,'3','B','1','13','0',' '
			,0,0,0,0,0,0,0,0
			,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
			,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6,@sProvider,@sReceiver
			)

			select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
			if @sProvider ='Yes'
			begin
				select 
				 @amt1=sum(case when ltrim(rtrim(srno4)) = '7' then amt1 else -amt1 end)
				,@amt2=sum(case when ltrim(rtrim(srno4)) = '7' then amt2 else -amt2 end)
				,@amt3=sum(case when ltrim(rtrim(srno4)) = '7' then amt3 else -amt3 end)
				,@amt4=sum(case when ltrim(rtrim(srno4)) = '7' then amt4 else -amt4 end)
				,@amt5=sum(case when ltrim(rtrim(srno4)) = '7' then amt5 else -amt5 end)
				,@amt6=sum(case when ltrim(rtrim(srno4)) = '7' then amt6 else -amt6 end)
				from #st3
				where ltrim(rtrim(srno1)) = '3' and srno2 = 'B' and ltrim(rtrim(srno3)) = '1' and ltrim(rtrim(srno4)) in ('7','13')
				and serty=@serty 
				and sProvider=case when @sProvider ='Yes' then 'Yes' else 'No' end
				and sReceiver=case when @sProvider ='Yes' then 'No' else 'Yes' end
			end
			else	
			begin
				select 
				 @amt1=sum(case when ltrim(rtrim(srno4)) = '8' then amt1 else -amt1 end)
				,@amt2=sum(case when ltrim(rtrim(srno4)) = '8' then amt2 else -amt2 end)
				,@amt3=sum(case when ltrim(rtrim(srno4)) = '8' then amt3 else -amt3 end)
				,@amt4=sum(case when ltrim(rtrim(srno4)) = '8' then amt4 else -amt4 end)
				,@amt5=sum(case when ltrim(rtrim(srno4)) = '8' then amt5 else -amt5 end)
				,@amt6=sum(case when ltrim(rtrim(srno4)) = '8' then amt6 else -amt6 end)
				from #st3
				where ltrim(rtrim(srno1)) = '3' and srno2 = 'B' and ltrim(rtrim(srno3)) = '1' and ltrim(rtrim(srno4)) in ('8','13')
				and serty=@serty 
				and sProvider=case when @sProvider ='Yes' then 'Yes' else 'No' end
				and sReceiver=case when @sProvider ='Yes' then 'No' else 'Yes' end
			end
				
			select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)
			insert into #st3
			(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
			,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
			,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
			,amt1,amt2,amt3,amt4,amt5,amt6,sProvider,sReceiver
			)
			values
			('',@serty,'3','B','1','14','0',''
			,0,0,0,0,0,0,0,0
			,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
			,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6,@sProvider,@sReceiver
			)
			
			select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
			insert into #st3
			(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
			,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
			,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
			,amt1,amt2,amt3,amt4,amt5,amt6,sProvider,sReceiver
			)
			values
			('',@serty,'3','B','1','15','0',' '
			,0,0,0,0,0,0,0,0
			,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
			,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6,@sProvider,@sReceiver
			)

			declare cur_st3_3 cursor for
			select distinct Serbper,Sercper,Serhper,Serbcper
			from #bracdet
			where serty=@serty and Serbper in (10,12)
			order by Serbper,Sercper,Serhper
			open cur_st3_3
			fetch next from cur_st3_3 into @Serbper,@Sercper,@Serhper,@Serbcper
			set @d=0
			while(@@fetch_status=0)
			begin
				select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
				set @d = @d + 1
				select 
				 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(taxable_amt,0) else 0 end)
				,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(taxable_amt,0) else 0 end)
				,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(taxable_amt,0) else 0 end)
				,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(taxable_amt,0) else 0 end)
				,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(taxable_amt,0) else 0 end)
				,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(taxable_amt,0) else 0 end)
				from #bracdet
				where serty=@serty
				and sProvider=case when @sProvider ='Yes' then 'Yes' else 'No' end
				and sReceiver=case when @sProvider ='Yes' then 'No' else 'Yes' end
				and serbper=@Serbper and Sercper=@Sercper and Serhper=@Serhper and serbcper=@serbcper
				 
				select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
				insert into #st3
				(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
				,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
				,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
				,amt1,amt2,amt3,amt4,amt5,amt6,sProvider,sReceiver
				)
				values
				('',@serty,'3','B','1','15',cast(@d as varchar(1)),''
				,@SERBPER,0,@SERCPER,0,@SERHPER,0,@SERBCPER,0
				,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
				,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6,@sProvider,@sReceiver
				)

				fetch next from cur_st3_3 into @Serbper,@Sercper,@Serhper,@Serbcper
			end
			close cur_st3_3
			deallocate cur_st3_3


			if not exists(select serty from #st3 where srno1='3' and srno2='B' and srno3='1' and srno4='15' and srno5='1')
			begin
				insert into #st3
				(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
				,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
				,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
				,amt1,amt2,amt3,amt4,amt5,amt6,sProvider,sReceiver
				)
				values
				('',@serty,'3','B','1','15','1',''
				,0,0,0,0,0,0,0,0
				,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
				,0,0,0,0,0,0,@sProvider,@sReceiver
				)
			end
	
			select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
			insert into #st3
			(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
			,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
			,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
			,amt1,amt2,amt3,amt4,amt5,amt6,sProvider,sReceiver
			)
			values
			('',@serty,'3','B','1','16','0',' '
			,0,0,0,0,0,0,0,0
			,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
			,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6,@sProvider,@sReceiver
			)


			declare cur_st3_3 cursor for
			select distinct Serbper,Sercper,Serhper,serbcper
			from #bracdet
			where serty=@serty and Serbper not in (10,12)
			order by Serbper,Sercper,Serhper
			open cur_st3_3
			fetch next from cur_st3_3 into @Serbper,@Sercper,@Serhper,@serbcper
			set @d=0
			while(@@fetch_status=0)
			begin
				select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
				set @d = @d + 1
				select 
				 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(taxable_amt,0) else 0 end)
				,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(taxable_amt,0) else 0 end)
				,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(taxable_amt,0) else 0 end)
				,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(taxable_amt,0) else 0 end)
				,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(taxable_amt,0) else 0 end)
				,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(taxable_amt,0) else 0 end)
				from #bracdet
				where serty=@serty
				and sProvider=case when @sProvider ='Yes' then 'Yes' else 'No' end
				and sReceiver=case when @sProvider ='Yes' then 'No' else 'Yes' end
				and serbper=@Serbper and Sercper=@Sercper and Serhper=@Serhper and serbcper=@serbcper
				 
				select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
				insert into #st3
				(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
				,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
				,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
				,amt1,amt2,amt3,amt4,amt5,amt6,sProvider,sReceiver
				)
				values
				('',@serty,'3','B','1','16',cast(@d as varchar(1)),''
				,@SERBPER,0,@SERCPER,0,@SERHPER,0,@SERBCPER,0
				,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
				,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6,@sProvider,@sReceiver
				)

				fetch next from cur_st3_3 into @Serbper,@Sercper,@Serhper,@serbcper
			end
			close cur_st3_3
			deallocate cur_st3_3

			if not exists(select serty from #st3 where srno1='3' and srno2='B' and srno3='1' and srno4='16' and srno5='1')
			begin
				insert into #st3
				(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
				,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
				,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
				,amt1,amt2,amt3,amt4,amt5,amt6,sProvider,sReceiver
				)
				values
				('',@serty,'3','B','1','16','1',''
				,0,0,0,0,0,0,0,0
				,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
				,0,0,0,0,0,0,@sProvider,@sReceiver
				)
			end

			--select * from #bracdet
			
			select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
			select 
			@amt1=sum(case when (date between @sdate1 and @edate1) then isnull(serbamt,0) else 0 end)
			,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(serbamt,0) else 0 end)
			,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(serbamt,0) else 0 end)
			,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(serbamt,0) else 0 end)
			,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(serbamt,0) else 0 end)
			,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(serbamt,0) else 0 end)
			from #bracdet
			where serty=@serty
			and sProvider=case when @sProvider ='Yes' then 'Yes' else 'No' end
			and sReceiver=case when @sProvider ='Yes' then 'No' else 'Yes' end
			
			select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)
			insert into #st3
			(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
			,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
			,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
			,amt1,amt2,amt3,amt4,amt5,amt6,sProvider,sReceiver
			)
			values
			( '',@serty,'3','B','1','17','0',''
			,0,0,0,0,0,0,0,0
			,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
			,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6,@sProvider,@sReceiver
			)	

			select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
			insert into #st3
			(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
			,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
			,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
			,amt1,amt2,amt3,amt4,amt5,amt6,sProvider,sReceiver
			)
			values
			('',@serty,'3','B','1','18','0',' '
			,0,0,0,0,0,0,0,0
			,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
			,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6,@sProvider,@sReceiver
			)

			select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
			select 
			 @amt1=sum(case when ltrim(rtrim(srno4)) = '17' then amt1 else -amt1 end)
			,@amt2=sum(case when ltrim(rtrim(srno4)) = '17' then amt2 else -amt2 end)
			,@amt3=sum(case when ltrim(rtrim(srno4)) = '17' then amt3 else -amt3 end)
			,@amt4=sum(case when ltrim(rtrim(srno4)) = '17' then amt4 else -amt4 end)
			,@amt5=sum(case when ltrim(rtrim(srno4)) = '17' then amt5 else -amt5 end)
			,@amt6=sum(case when ltrim(rtrim(srno4)) = '17' then amt6 else -amt6 end)
			from #st3
			where ltrim(rtrim(srno1)) = '3' and srno2 = 'B' and ltrim(rtrim(srno3)) = '1' and ltrim(rtrim(srno4)) in ('17','18')
			and serty=@serty 
			and sProvider=case when @sProvider ='Yes' then 'Yes' else 'No' end
			and sReceiver=case when @sProvider ='Yes' then 'No' else 'Yes' end

			select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
			insert into #st3
			(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
			,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
			,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
			,amt1,amt2,amt3,amt4,amt5,amt6,sProvider,sReceiver
			)
			values
			('',@serty,'3','B','1','19','0',' '
			,0,0,0,0,0,0
			,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
			,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6,@sProvider,@sReceiver
			)


			select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
			select 
			@amt1=sum(case when (date between @sdate1 and @edate1) then isnull(sercamt,0) else 0 end)
			,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(sercamt,0) else 0 end)
			,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(sercamt,0) else 0 end)
			,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(sercamt,0) else 0 end)
			,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(sercamt,0) else 0 end)
			,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(sercamt,0) else 0 end)
			from #bracdet
			where serty=@serty
			and sProvider=case when @sProvider ='Yes' then 'Yes' else 'No' end
			and sReceiver=case when @sProvider ='Yes' then 'No' else 'Yes' end
			select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)
			insert into #st3
			(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
			,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
			,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
			,amt1,amt2,amt3,amt4,amt5,amt6,sProvider,sReceiver
			)
			values
			( '',@serty,'3','B','1','20','0',''
			,0,0,0,0,0,0,0,0
			,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
			,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6,@sProvider,@sReceiver
			)	
			
			select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
			select 
			 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(serhamt,0) else 0 end)
			,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(serhamt,0) else 0 end)
			,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(serhamt,0) else 0 end)
			,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(serhamt,0) else 0 end)
			,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(serhamt,0) else 0 end)
			,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(serhamt,0) else 0 end)
			from #bracdet
			where serty=@serty
			and sProvider=case when @sProvider ='Yes' then 'Yes' else 'No' end
			and sReceiver=case when @sProvider ='Yes' then 'No' else 'Yes' end
			select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)
			insert into #st3
			(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
			,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
			,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
			,amt1,amt2,amt3,amt4,amt5,amt6,sProvider,sReceiver
			)
			values
			( '',@serty,'3','B','1','21','0',''
			,0,0,0,0,0,0,0,0
			,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
			,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6,@sProvider,@sReceiver
			)	
			
			
			select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
			select 
			 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(serbcess,0) else 0 end)
			,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(serbcess,0) else 0 end)
			,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(serbcess,0) else 0 end)
			,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(serbcess,0) else 0 end)
			,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(serbcess,0) else 0 end)
			,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(serbcess,0) else 0 end)
			from #bracdet
			where serty=@serty and Serbper in (10,12)
			and sProvider=case when @sProvider ='Yes' then 'Yes' else 'No' end
			and sReceiver=case when @sProvider ='Yes' then 'No' else 'Yes' end
			select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)
			insert into #st3
			(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
			,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
			,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
			,amt1,amt2,amt3,amt4,amt5,amt6,sProvider,sReceiver
			)
			values
			( '',@serty,'3','B','1','22','0',''
			,0,0,0,0,0,0,0,0
			,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
			,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6,@sProvider,@sReceiver
			)	
			
			select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
			select 
			 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(serbcess,0) else 0 end)
			,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(serbcess,0) else 0 end)
			,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(serbcess,0) else 0 end)
			,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(serbcess,0) else 0 end)
			,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(serbcess,0) else 0 end)
			,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(serbcess,0) else 0 end)
			from #bracdet
			where serty=@serty and Serbper not in (10,12)
			and sProvider=case when @sProvider ='Yes' then 'Yes' else 'No' end
			and sReceiver=case when @sProvider ='Yes' then 'No' else 'Yes' end
			
			
			select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)
			insert into #st3
			(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
			,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
			,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
			,amt1,amt2,amt3,amt4,amt5,amt6,sProvider,sReceiver
			)
			values
			( '',@serty,'3','B','1','23','0',''
			,0,0,0,0,0,0,0,0
			,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
			,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6,@sProvider,@sReceiver
			)	
			
			select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
			select 
			 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(serbcess,0) else 0 end)
			,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(serbcess,0) else 0 end)
			,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(serbcess,0) else 0 end)
			,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(serbcess,0) else 0 end)
			,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(serbcess,0) else 0 end)
			,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(serbcess,0) else 0 end)
			from #bracdet
			where serty=@serty
			and sProvider=case when @sProvider ='Yes' then 'Yes' else 'No' end
			and sReceiver=case when @sProvider ='Yes' then 'No' else 'Yes' end
			
			select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)
			insert into #st3
			(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
			,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
			,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
			,amt1,amt2,amt3,amt4,amt5,amt6,sProvider,sReceiver
			)
			values
			( '',@serty,'3','B','1','24','0',''
			,0,0,0,0,0,0,0,0
			,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
			,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6,@sProvider,@sReceiver
			)	
			
		end /*while i=<2*/
	
	
	
	/*<--Blank Records*/	
	fetch next from cur_serty into @serty
end--while (@@fetch_status=0)
close cur_serty
deallocate cur_serty /*Repetable Part Over*/
/*<---3F*/


/*--->4*/
	/*-->4 1&2*/
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6,sProvider,sReceiver
	)
	values
	('','','4','C','0','','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6,@sProvider,@sReceiver
	)

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select
	@amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where beh in ('BP','CP')  
	and tdspaytype=2		--Added by Shrikant S. on 28/03/2016 for Bug-27848	
	and isnull(u_arrears,'')='D3-Advance Service Tax utilized [Rule 6(1A)]'

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6,sProvider,sReceiver
	)
	values
	('','','4','C','1','','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6,@sProvider,@sReceiver
	)

	--SELECT * FROM #bpacdet
	
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select
	@amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where beh in ('BP','CP') 
	and tdspaytype=2		--Added by Shrikant S. on 28/03/2016 for Bug-27848	
	and isnull(u_arrears,'')='Advance Payment of Swachh Bharat Cess'	

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6,sProvider,sReceiver
	)
	values
	('','','4','C','1','1','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6,@sProvider,@sReceiver
	)
	
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select
	@amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where beh in ('BP','CP') 
	and tdspaytype=2		--Added by Shrikant S. on 28/03/2016 for Bug-27848	
	and isnull(u_arrears,'')='E3-Advance Educess  utilized [Rule 6(1A)]'
	
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6,sProvider,sReceiver
	)
	values
	('','','4','C','2','','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6,@sProvider,@sReceiver
	)

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select
	@amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where beh in ('BP','CP') 
	and tdspaytype=2		--Added by Shrikant S. on 28/03/2016 for Bug-27848	
	and isnull(u_arrears,'')='F3-Advance Sec. and High. Educess  utilized [Rule 6(1A)]'
	
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6,sProvider,sReceiver
	)
	values
	('','','4','C','3','','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6,@sProvider,@sReceiver
	)

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0	
	declare cur_st3_2 cursor for
	select sum(amount) as amount,date,u_chalno,u_chaldt
	from #bpacdet 
	where ((isnull(u_chalno,'')<>'') or (isnull(u_chaldt,'')<>'') )
	and isnull(u_arrears,'') in ('D3-Advance Service Tax utilized [Rule 6(1A)]','E3-Advance Educess  utilized [Rule 6(1A)]','F3-Advance Sec. and High. Educess  utilized [Rule 6(1A)]')
	and tdspaytype=2	--Added by Shrikant S. on 28/03/2016 for BUg-27848
	group by date,u_chalno,u_chaldt
	
	open cur_st3_2
	fetch next from cur_st3_2 into @amt1,@date,@u_chalno,@u_chaldt
	
	set @c=0
	set @particulars1=' '
	while(@@fetch_status=0)
	begin
		set @c=@c+1
		select @particulars=(case when @c=1 then '(i)' else (case when @c=2 then '(ii)' else (case when @c=3 then '(iii)' else (case when @c=4 then '(iv)' else (case when @c=5 then '(v)' else (case when @c=6 then '(vi)' else (case when @c=7 then '(vii)' else (case when @c=8 then '(viii)' else (case when @c=9 then '(ix)' else (case when @c=10 then '(x)' else '' end) end) end) end) end) end) end) end) end) end)
		insert into #st3
		(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
		,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
		,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
		,amt1,amt2,amt3,amt4,amt5,amt6
		,chalno1,chaldt1,chalno2,chaldt2,chalno3,chaldt3,chalno4,chaldt4,chalno5,chaldt5,chalno6,chaldt6
		)
		values
		('','','4','C','4','','',''
		,0,0,0,0,0,0,0,0
		,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
		,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
		,(case when (@date between @sdate1 and @edate6) then isnull(@u_chalno,'') else '' end)
		,(case when (@date between @sdate1 and @edate6) then isnull(@u_chaldt,'') else '' end)
		,(case when (@date between @sdate2 and @edate2) then isnull(@u_chalno,'') else '' end)
		,(case when (@date between @sdate2 and @edate2) then isnull(@u_chaldt,'') else '' end)
		,(case when (@date between @sdate3 and @edate3) then isnull(@u_chalno,'') else '' end)
		,(case when (@date between @sdate3 and @edate3) then isnull(@u_chaldt,'') else '' end)
		,(case when (@date between @sdate4 and @edate4) then isnull(@u_chalno,'') else '' end)
		,(case when (@date between @sdate4 and @edate4) then isnull(@u_chaldt,'') else '' end)
		,(case when (@date between @sdate5 and @edate5) then isnull(@u_chalno,'') else '' end)
		,(case when (@date between @sdate5 and @edate5) then isnull(@u_chaldt,'') else '' end)
		,(case when (@date between @sdate6 and @edate6) then isnull(@u_chalno,'') else '' end)
		,(case when (@date between @sdate6 and @edate6) then isnull(@u_chaldt,'') else '' end)
		)
		
		
		
		fetch next from cur_st3_2 into @amt1,@date,@u_chalno,@u_chaldt
	end
	close cur_st3_2
	deallocate cur_st3_2
	
	if not exists(select serty from #st3 where srno1='4' and srno2='C' and srno3='2')
	begin
		insert into #st3
		(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
		,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
		,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
		,amt1,amt2,amt3,amt4,amt5,amt6
		)
		values
		('','','4','C','2','','',''
		,0,0,0,0,0,0,0,0
		,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
		,0,0,0,0,0,0
		)
	end
	if not exists(select serty from #st3 where srno1='4' and srno2='C' and srno3='3')
	begin
		insert into #st3
		(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
		,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
		,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
		,amt1,amt2,amt3,amt4,amt5,amt6
		)
		values
		('','','4','C','3','','',''
		,0,0,0,0,0,0,0,0
		,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
		,0,0,0,0,0,0
		)
	end

	if not exists(select serty from #st3 where srno1='4' and srno2='C' and srno3='4')
	begin
		insert into #st3
		(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
		,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
		,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
		,amt1,amt2,amt3,amt4,amt5,amt6
		)
		values
		('','','4','C','4','','',''
		,0,0,0,0,0,0,0,0
		,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
		,0,0,0,0,0,0
		)
	end

	/*<--4 1&2*/
	
	/*-->4 A1*/
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','D','0','','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	/*->Service Tax*/
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where typ in ('Service Tax Payable','GTA Service Tax Payable') and beh in ('BP','CP') and isnull(u_arrears,' ')=' '
	and tdspaytype<>2	--Added by Shrikant S. on 28/03/2016 for BUg-27848
	
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','D','1','','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where typ in ('Service Tax Payable','GTA Service Tax Payable') and beh in ('JV')  and isnull(u_arrears,' ')=' ' 
	and tdspaytype<>2	--Added by Shrikant S. on 28/03/2016 for BUg-27848
	
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','D','2','','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)	

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where isnull(u_arrears,' ') = 'D3-Advance Service Tax utilized [Rule 6(1A)]' 
	and tdspaytype<>2	--Added by Shrikant S. on 28/03/2016 for BUg-27848
		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','D','3','','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)		

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where isnull(u_arrears,' ')='D4-Service Tax paid [Rule 6(3)]' 
	and tdspaytype<>2	--Added by Shrikant S. on 28/03/2016 for BUg-27848
		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','D','4','','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)		

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where isnull(u_arrears,' ')='D5-Service Tax paid [Rule 6(4A)]' 
	and tdspaytype<>2	--Added by Shrikant S. on 28/03/2016 for BUg-27848
		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','D','5','','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)		

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where isnull(u_arrears,' ')='D6-Service Tax paid [Rule 6(4C)]' 
	and tdspaytype<>2	--Added by Shrikant S. on 28/03/2016 for BUg-27848

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','D','6','','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)		

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where isnull(u_arrears,' ')='D7-Service Tax paid [Book Adjustment]' 
	and tdspaytype<>2	--Added by Shrikant S. on 28/03/2016 for BUg-27848

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','D','7','','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)		

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(amt1)
	,@amt2=sum(amt2)
	,@amt3=sum(amt3)
	,@amt4=sum(amt4)
	,@amt5=sum(amt5)
	,@amt6=sum(amt6)
	from #st3
	where ltrim(rtrim(srno1)) = '4' and srno2 = 'D'

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','D','8','','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)		

	/*<-Service Tax*/
	
	/*--> Swachh Bharat Cess */
		select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','DA','0','','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	
	
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where typ in ('Service Tax Payable-SBcess') and beh in ('BP','CP') and isnull(u_arrears,' ')=' '
	
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','DA','1','','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where typ in ('Service Tax Payable-SBcess') and beh in ('BP','CP') and isnull(u_arrears,' ') = 'Advance under Rule 6 (1A) of ST Rules'
		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','DA','2','','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)	
	
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where typ in ('Service Tax Payable-SBcess') and beh in ('BP','CP') and isnull(u_arrears,' ')='under Rule 6 (3) of ST Rules'
		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','DA','3','','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	--SELECT * FROM #bpacdet
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where typ in ('Service Tax Payable-SBcess') and beh in ('BP','CP') and isnull(u_arrears,' ')='under Rule 6 (4A) of ST Rules'
		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','DA','4','','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where typ in ('Service Tax Payable-SBcess') and beh in ('BP','CP') and isnull(u_arrears,' ')='Advance Adjustment'
		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','DA','5','','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(amt1)
	,@amt2=sum(amt2)
	,@amt3=sum(amt3)
	,@amt4=sum(amt4)
	,@amt5=sum(amt5)
	,@amt6=sum(amt6)
	from #st3
	where ltrim(rtrim(srno1)) = '4' and srno2 = 'DA'

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','DA','6','','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)		
	/* Swachh Bharat Cess <--*/
	
	/*->Edu. Cess*/
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','E','0','','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where typ in ('Service Tax Payable-Ecess','GTA Service Tax Payable-Ecess') and beh in ('BP','CP') and isnull(u_arrears,' ')=' '
		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','E','1','','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where typ in ('Service Tax Payable-Ecess','GTA Service Tax Payable-Ecess') and beh in ('JV') and isnull(u_arrears,' ')=' ' 
	
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','E','2','','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)	

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where isnull(u_arrears,' ')=	'E3-Advance Educess  utilized [Rule 6(1A)]'
		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','E','3','','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)		

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where isnull(u_arrears,' ')='E4-Educess  paid [Rule 6(3)]'
		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','E','4','','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)		

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where isnull(u_arrears,' ')='E5-Educess  paid [Rule 6(4A)]'
		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','E','5','','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)		

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where isnull(u_arrears,' ')='E6-Educess  paid [Rule 6(4C)]'

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','E','6','','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)		

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where isnull(u_arrears,' ')='E7-Educess  paid [Book Adjustment]'

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','E','7','','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)		

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(amt1)
	,@amt2=sum(amt2)
	,@amt3=sum(amt3)
	,@amt4=sum(amt4)
	,@amt5=sum(amt5)
	,@amt6=sum(amt6)
	from #st3
	where ltrim(rtrim(srno1)) = '4' and srno2 = 'E'

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','E','8','','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)		

	/*<-Edu. Cess*/
	/*->S & H Cess*/
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','F','0','','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where typ in ('Service Tax Payable-Hcess','GTA Service Tax Payable-Hcess') and beh in ('BP','CP') and isnull(u_arrears,' ')=' '
		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','F','1','','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where typ in ('Service Tax Payable-Hcess','GTA Service Tax Payable-Hcess') and beh in ('JV') and isnull(u_arrears,' ')=' ' 
	
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','F','2','','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)	

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where isnull(u_arrears,' ')='F3-Advance Sec. and High. Educess  utilized [Rule 6(1A)]'
		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','F','3','','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)		

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where isnull(u_arrears,' ')='F4-Sec. and High. Educess  paid [Rule 6(3)]'
		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','F','4','','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)		

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where isnull(u_arrears,' ')='F5-Sec. and High. Educess  paid [Rule 6(4A)]'
		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','F','5','','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)		

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where isnull(u_arrears,' ')='F6-Sec. and High. Educess  paid [Rule 6(4C)]'

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','F','6','','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)		

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where isnull(u_arrears,' ')='F7-Sec. and High. Educess  paid [Book Adjustment]'

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','F','7','','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)		

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(amt1)
	,@amt2=sum(amt2)
	,@amt3=sum(amt3)
	,@amt4=sum(amt4)
	,@amt5=sum(amt5)
	,@amt6=sum(amt6)
	from #st3
	where ltrim(rtrim(srno1)) = '4' and srno2 = 'F'

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','F','8','','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)		

	/*<-S & H Cess*/
	/*4 A D->*/
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','G',' 0','','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)		

	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where  isnull(u_arrears,' ')='G1-Other amounts paid - Arrears Cash'

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','G',' 1','','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)		

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where isnull(u_arrears,' ')='G2-Other amounts paid - Arrears Credit'
		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','G',' 2','','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)		

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where isnull(u_arrears,' ')='G3-Other amounts paid - Arrears (Educess) Cash'
		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','G',' 3','','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)		
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where  isnull(u_arrears,' ')='G4-Other amounts paid - Arrears (Educess) Credit'
		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','G',' 4','','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)		

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where  isnull(u_arrears,' ')='G5-Other amounts paid - Arrears (Sec and Higher Educess) Cash'
		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','G',' 5','','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)		

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where isnull(u_arrears,' ')='G6-Other amounts paid - Arrears (Sec and Higher Educess) Credit'
		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','G',' 6','','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)		

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where isnull(u_arrears,' ') = 'G7-Other amounts paid - [Rule (73A)]'
		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','G',' 7','','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)		

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where  isnull(u_arrears,' ')='G8-Other amounts paid - Interest'
		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','G',' 8','','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)		
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where  isnull(u_arrears,' ')='G9-Other amounts paid - Penalty'
		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','G',' 9','','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)		

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where isnull(u_arrears,' ')='G10-Other amounts paid - (Late Fee)'
		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','G','10','','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)		

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where isnull(u_arrears,'') = 'G11-Any other amounts paid'

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','G','11','','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)			

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(amt1)
	,@amt2=sum(amt2)
	,@amt3=sum(amt3)
	,@amt4=sum(amt4)
	,@amt5=sum(amt5)
	,@amt6=sum(amt6)
	from #st3
	where ltrim(rtrim(srno1)) = '4' and srno2 = 'G'

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','G','12','','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)		
	
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where isnull(u_arrears,'') = 'Arrears of SBC paid in cash '

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','G','13','','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)			
	
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where isnull(u_arrears,'') = 'Interest on SBC paid in cash'

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','G','14','','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)			
	
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(amount,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(amount,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(amount,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(amount,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(amount,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(amount,0) else 0 end)
	from #bpacdet
	where isnull(u_arrears,'') = 'Penalty on SBC paid in cash'

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','G','15','','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(amt1)
	,@amt2=sum(amt2)
	,@amt3=sum(amt3)
	,@amt4=sum(amt4)
	,@amt5=sum(amt5)
	,@amt6=sum(amt6)
	from #st3
	where ltrim(rtrim(srno1)) = '4' and srno2 = 'G' AND srno3 in ('13','14','15')

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','G','16','','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	
				
	/*<-4 A D*/
	/*<--4 A1*/
	/*-->4 A2 & B */
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','H','','','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)	
	
	declare cur_st3_2 cursor for
	select date,u_chalno,u_chaldt,u_arrears,sDocNo,sDocDt,sum(amount)
	from #bpacdet 
	where ((isnull(u_chalno,'')<>'') or (isnull(u_chaldt,'')<>'') )
	group by date,u_chalno,u_chaldt,u_arrears,sDocNo,sDocDt
	order by u_chaldt,date

	open cur_st3_2
	fetch next from cur_st3_2 into @date,@u_chalno,@u_chaldt,@u_arrears,@sDocNo,@sDocDt,@amt0
	set @c=0
	set @particulars1=' '
	while(@@fetch_status=0)
	begin
		set @c=@c+1
		select @particulars=(case when @c=1 then '(i)' else (case when @c=2 then '(ii)' else (case when @c=3 then '(iii)' else (case when @c=4 then '(iv)' else (case when @c=5 then '(v)' else (case when @c=6 then '(vi)' else (case when @c=7 then '(vii)' else (case when @c=8 then '(viii)' else (case when @c=9 then '(ix)' else (case when @c=10 then '(x)' else '' end) end) end) end) end) end) end) end) end) end)
		insert into #st3
		(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
		,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
		,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
		,amt1,amt2,amt3,amt4,amt5,amt6
		,chalno1,chaldt1,chalno2,chaldt2,chalno3,chaldt3,chalno4,chaldt4,chalno5,chaldt5,chalno6,chaldt6
		)
		values
		(@particulars,'','4','H','1','1','',''
		,0,0,0,0,0,0,0,0
		,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
		,(case when (@date between @sdate1 and @edate1) then isnull(@amt0,0) else 0 end)
		,(case when (@date between @sdate2 and @edate2) then isnull(@amt0,0) else 0 end)
		,(case when (@date between @sdate3 and @edate3) then isnull(@amt0,0) else 0 end)
		,(case when (@date between @sdate4 and @edate4) then isnull(@amt0,0) else 0 end)
		,(case when (@date between @sdate5 and @edate5) then isnull(@amt0,0) else 0 end)
		,(case when (@date between @sdate6 and @edate6) then isnull(@amt0,0) else 0 end)
		,(case when (@date between @sdate1 and @edate1) then isnull(@u_chalno,'') else '' end)
		,(case when (@date between @sdate1 and @edate1) then isnull(@u_chaldt,'') else '' end)
		,(case when (@date between @sdate2 and @edate2) then isnull(@u_chalno,'') else '' end)
		,(case when (@date between @sdate2 and @edate2) then isnull(@u_chaldt,'') else '' end)
		,(case when (@date between @sdate3 and @edate3) then isnull(@u_chalno,'') else '' end)
		,(case when (@date between @sdate3 and @edate3) then isnull(@u_chaldt,'') else '' end)
		,(case when (@date between @sdate4 and @edate4) then isnull(@u_chalno,'') else '' end)
		,(case when (@date between @sdate4 and @edate4) then isnull(@u_chaldt,'') else '' end)
		,(case when (@date between @sdate5 and @edate5) then isnull(@u_chalno,'') else '' end)
		,(case when (@date between @sdate5 and @edate5) then isnull(@u_chaldt,'') else '' end)
		,(case when (@date between @sdate6 and @edate6) then isnull(@u_chalno,'') else '' end)
		,(case when (@date between @sdate6 and @edate6) then isnull(@u_chaldt,'') else '' end)
		)

		fetch next from cur_st3_2 into @date,@u_chalno,@u_chaldt,@u_arrears,@sDocNo,@sDocDt,@amt0
	end
	close cur_st3_2
	deallocate cur_st3_2

	if not exists(select serty from #st3 where srno1='4' and srno2='H' and srno3='1' and srno4='1')
	begin
		insert into #st3
		(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
		,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
		,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
		,amt1,amt2,amt3,amt4,amt5,amt6
		)
		values
		('','','4','H','1','1','',''
		,0,0,0,0,0,0,0,0
		,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
		,0,0,0,0,0,0
		)
	end

	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	,chalno1,chaldt1,chalno2,chaldt2,chalno3,chaldt3,chalno4,chaldt4,chalno5,chaldt5,chalno6,chaldt6
	)
	values
	(@particulars,'','4','H','2','1','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	,'','','','','',''
	,'','','','','',''
	)

	declare cur_st3_2 cursor for
	select date,u_chalno,u_chaldt,u_arrears,sDocNo,sDocDt,sum(amount)
	from #bpacdet 
	where isnull(u_arrears,'')<>''
	group by date,u_chalno,u_chaldt,u_arrears,sDocNo,sDocDt
	order by u_chaldt,date

	open cur_st3_2
	fetch next from cur_st3_2 into @date,@u_chalno,@u_chaldt,@u_arrears,@sDocNo,@sDocDt,@amt0
	set @c=0
	set @particulars1=' '
	while(@@fetch_status=0)
	begin
		set @c=@c+1
		set @particulars= isnull(@u_arrears,'')
				
		insert into #st3
		(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
		,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
		,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
		,amt1,amt2,amt3,amt4,amt5,amt6
		,chalno1,chaldt1,chalno2,chaldt2,chalno3,chaldt3,chalno4,chaldt4,chalno5,chaldt5,chalno6,chaldt6
		,sdocno,sdocdt
		)
		values
		(@particulars,'','4','H','2','2','',''
		,0,0,0,0,0,0,0,0
		,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
		,(case when (@date between @sdate1 and @edate1) then isnull(@amt0,0) else 0 end)
		,(case when (@date between @sdate2 and @edate2) then isnull(@amt0,0) else 0 end)
		,(case when (@date between @sdate3 and @edate3) then isnull(@amt0,0) else 0 end)
		,(case when (@date between @sdate4 and @edate4) then isnull(@amt0,0) else 0 end)
		,(case when (@date between @sdate5 and @edate5) then isnull(@amt0,0) else 0 end)
		,(case when (@date between @sdate6 and @edate6) then isnull(@amt0,0) else 0 end)
		,(case when (@date between @sdate1 and @edate1) then isnull(@u_chalno,'') else '' end)
		,(case when (@date between @sdate1 and @edate1) then isnull(@u_chaldt,'') else '' end)
		,(case when (@date between @sdate2 and @edate2) then isnull(@u_chalno,'') else '' end)
		,(case when (@date between @sdate2 and @edate2) then isnull(@u_chaldt,'') else '' end)
		,(case when (@date between @sdate3 and @edate3) then isnull(@u_chalno,'') else '' end)
		,(case when (@date between @sdate3 and @edate3) then isnull(@u_chaldt,'') else '' end)
		,(case when (@date between @sdate4 and @edate4) then isnull(@u_chalno,'') else '' end)
		,(case when (@date between @sdate4 and @edate4) then isnull(@u_chaldt,'') else '' end)
		,(case when (@date between @sdate5 and @edate5) then isnull(@u_chalno,'') else '' end)
		,(case when (@date between @sdate5 and @edate5) then isnull(@u_chaldt,'') else '' end)
		,(case when (@date between @sdate6 and @edate6) then isnull(@u_chalno,'') else '' end)
		,(case when (@date between @sdate6 and @edate6) then isnull(@u_chaldt,'') else '' end)
		,(case when isnull(@sDocNo,'') = '' then @u_chalno else @sDocNo end)
		,(case when isnull(@sDocNo,'') = '' then @u_chaldt else @sDocDt end)
		)

		fetch next from cur_st3_2 into @date,@u_chalno,@u_chaldt,@u_arrears,@sDocNo,@sDocDt,@amt0
	end
	close cur_st3_2
	deallocate cur_st3_2

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','1','','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('No','','5','I','1','1','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('No','','5','I','1','2','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('No','','5','I','1','3','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','1','4','0',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0

	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('No','','5','I','1','4','1',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('No','','5','I','1','4','2',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('No','','5','I','1','4','3',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)

/*<---5A*/

/*--->5AA*/
select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','2','0','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)


	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','2','1','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','2','2','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','2','3','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','2','4','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','2','5','',' '
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','0','0','0'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)	

	--5b1b
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','1','0','0'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)	

	--select * from #st3_5b
	
	/*-->5B 1*/
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (beh='OB' or date<@sdate1) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
	,@amt2=sum(case when (beh='OB' or date<@sdate2) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
	,@amt3=sum(case when (beh='OB' or date<@sdate3) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
	,@amt4=sum(case when (beh='OB' or date<@sdate4) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
	,@amt5=sum(case when (beh='OB' or date<@sdate5) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
	,@amt6=sum(case when (beh='OB' or date<@sdate6) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
	from #st3_5b
	where  (typ='Service Tax Available') and @isdprouct=0 

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','1','1','0'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)			

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','1','2','0'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)	

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when entry_ty<>'OB' and  (date between @sdate1 and @edate1) and (amt_ty='DR') then  amount else  0 end)
	,@amt2=sum(case when entry_ty<>'OB' and  (date between @sdate2 and @edate2) and (amt_ty='DR') then  amount else  0 end) 
	,@amt3=sum(case when entry_ty<>'OB' and  (date between @sdate3 and @edate3) and (amt_ty='DR') then  amount else  0 end)
	,@amt4=sum(case when entry_ty<>'OB' and  (date between @sdate4 and @edate4) and (amt_ty='DR') then  amount else  0 end)
	,@amt5=sum(case when entry_ty<>'OB' and  (date between @sdate5 and @edate5) and (amt_ty='DR') then  amount else  0 end)
	,@amt6=sum(case when entry_ty<>'OB' and  (date between @sdate6 and @edate6) and (amt_ty='DR') then  amount else  0 end)
	from #st3_5b
	where  (typ='Service Tax Available')  
	and (sertype in ('Credit taken On input')) 
	and @isdprouct=0

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','1','2','1'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)			

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when entry_ty<>'OB' and  (date between @sdate1 and @edate1) and (amt_ty='DR') then  amount else  0 end)
	,@amt2=sum(case when entry_ty<>'OB' and  (date between @sdate2 and @edate2) and (amt_ty='DR') then  amount else  0 end) 
	,@amt3=sum(case when entry_ty<>'OB' and  (date between @sdate3 and @edate3) and (amt_ty='DR') then  amount else  0 end)
	,@amt4=sum(case when entry_ty<>'OB' and  (date between @sdate4 and @edate4) and (amt_ty='DR') then  amount else  0 end)
	,@amt5=sum(case when entry_ty<>'OB' and  (date between @sdate5 and @edate5) and (amt_ty='DR') then  amount else  0 end)
	,@amt6=sum(case when entry_ty<>'OB' and  (date between @sdate6 and @edate6) and (amt_ty='DR') then  amount else  0 end)
	from #st3_5b
	where  (typ='Service Tax Available') 
	and (sertype='Credit taken On capital goods') and @isdprouct=0--(serty=@serty) and

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','1','2','2'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)			

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when entry_ty<>'OB' and  (date between @sdate1 and @edate1) and (amt_ty='DR') then  amount else  0 end)
	,@amt2=sum(case when entry_ty<>'OB' and  (date between @sdate2 and @edate2) and (amt_ty='DR') then  amount else  0 end) 
	,@amt3=sum(case when entry_ty<>'OB' and  (date between @sdate3 and @edate3) and (amt_ty='DR') then  amount else  0 end)
	,@amt4=sum(case when entry_ty<>'OB' and  (date between @sdate4 and @edate4) and (amt_ty='DR') then  amount else  0 end)
	,@amt5=sum(case when entry_ty<>'OB' and  (date between @sdate5 and @edate5) and (amt_ty='DR') then  amount else  0 end)
	,@amt6=sum(case when entry_ty<>'OB' and  (date between @sdate6 and @edate6) and (amt_ty='DR') then  amount else  0 end)
	from #st3_5b
	where  (typ='Service Tax Available') and @isdprouct=0--(serty=@serty)
	and sertype=''

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','1','2','3'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when entry_ty<>'OB' and  (date between @sdate1 and @edate1) and (amt_ty='DR') then  amount else  0 end)
	,@amt2=sum(case when entry_ty<>'OB' and  (date between @sdate2 and @edate2) and (amt_ty='DR') then  amount else  0 end) 
	,@amt3=sum(case when entry_ty<>'OB' and  (date between @sdate3 and @edate3) and (amt_ty='DR') then  amount else  0 end)
	,@amt4=sum(case when entry_ty<>'OB' and  (date between @sdate4 and @edate4) and (amt_ty='DR') then  amount else  0 end)
	,@amt5=sum(case when entry_ty<>'OB' and  (date between @sdate5 and @edate5) and (amt_ty='DR') then  amount else  0 end)
	,@amt6=sum(case when entry_ty<>'OB' and  (date between @sdate6 and @edate6) and (amt_ty='DR') then  amount else  0 end)
	from #st3_5b
	where  (typ='Service Tax Available') and @isdprouct=0--(serty=@serty) and
	and (sertype ='Credit taken As received from input service distributor')

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','1','2','4'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when entry_ty<>'OB' and  (date between @sdate1 and @edate1) and (amt_ty='DR') then  amount else  0 end)
	,@amt2=sum(case when entry_ty<>'OB' and  (date between @sdate2 and @edate2) and (amt_ty='DR') then  amount else  0 end) 
	,@amt3=sum(case when entry_ty<>'OB' and  (date between @sdate3 and @edate3) and (amt_ty='DR') then  amount else  0 end)
	,@amt4=sum(case when entry_ty<>'OB' and  (date between @sdate4 and @edate4) and (amt_ty='DR') then  amount else  0 end)
	,@amt5=sum(case when entry_ty<>'OB' and  (date between @sdate5 and @edate5) and (amt_ty='DR') then  amount else  0 end)
	,@amt6=sum(case when entry_ty<>'OB' and  (date between @sdate6 and @edate6) and (amt_ty='DR') then  amount else  0 end)
	from #st3_5b
	where  (typ='Service Tax Available') 
	and (sertype Like 'Credit taken From inter unit transfer by a LTU%') 
	and @isdprouct=0 --(serty=@serty) and

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','1','2','5'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
		select 
	 @amt1=sum(case when entry_ty<>'OB' and  (date between @sdate1 and @edate1) and (amt_ty='DR') then  amount else  0 end)
	,@amt2=sum(case when entry_ty<>'OB' and  (date between @sdate2 and @edate2) and (amt_ty='DR') then  amount else  0 end) 
	,@amt3=sum(case when entry_ty<>'OB' and  (date between @sdate3 and @edate3) and (amt_ty='DR') then  amount else  0 end)
	,@amt4=sum(case when entry_ty<>'OB' and  (date between @sdate4 and @edate4) and (amt_ty='DR') then  amount else  0 end)
	,@amt5=sum(case when entry_ty<>'OB' and  (date between @sdate5 and @edate5) and (amt_ty='DR') then  amount else  0 end)
	,@amt6=sum(case when entry_ty<>'OB' and  (date between @sdate6 and @edate6) and (amt_ty='DR') then  amount else  0 end)
	from #st3_5b
	where  (typ='Service Tax Available') 
	and (sertype ='Any other Credit taken') 
	and @isdprouct=0
	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','1','2','6'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)


	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(isnull(amt1,0))
	,@amt2=sum(isnull(amt2,0))
	,@amt3=sum(isnull(amt3,0))
	,@amt4=sum(isnull(amt4,0))
	,@amt5=sum(isnull(amt5,0))
	,@amt6=sum(isnull(amt6,0))
	from #st3 where ltrim(rtrim(srno1))='5' and ltrim(rtrim(srno2))='I' and ltrim(rtrim(srno3))='3' and ltrim(rtrim(srno4))='1' and (ltrim(rtrim(srno5))='1' or ltrim(rtrim(srno5))='2')

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','1','2','7'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	--5B1B
		--5B1C
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','1','3','0'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)	

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when entry_ty<>'OB' and  (date between @sdate1 and @edate1) and (amt_ty='CR') then  amount else  0 end)
	,@amt2=sum(case when entry_ty<>'OB' and  (date between @sdate2 and @edate2) and (amt_ty='CR') then  amount else  0 end) 
	,@amt3=sum(case when entry_ty<>'OB' and  (date between @sdate3 and @edate3) and (amt_ty='CR') then  amount else  0 end)
	,@amt4=sum(case when entry_ty<>'OB' and  (date between @sdate4 and @edate4) and (amt_ty='CR') then  amount else  0 end)
	,@amt5=sum(case when entry_ty<>'OB' and  (date between @sdate5 and @edate5) and (amt_ty='CR') then  amount else  0 end)
	,@amt6=sum(case when entry_ty<>'OB' and  (date between @sdate6 and @edate6) and (amt_ty='CR') then  amount else  0 end)
	from #st3_5b
	where  (typ='Service Tax Available')  and sertype not like 'Credit utilized%' and @isdprouct=0--(sertype'') --(serty=@serty) and

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','1','3','1'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)			

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when entry_ty<>'OB' and  (date between @sdate1 and @edate1) and (amt_ty='CR') then  amount else  0 end)
	,@amt2=sum(case when entry_ty<>'OB' and  (date between @sdate2 and @edate2) and (amt_ty='CR') then  amount else  0 end) 
	,@amt3=sum(case when entry_ty<>'OB' and  (date between @sdate3 and @edate3) and (amt_ty='CR') then  amount else  0 end)
	,@amt4=sum(case when entry_ty<>'OB' and  (date between @sdate4 and @edate4) and (amt_ty='CR') then  amount else  0 end)
	,@amt5=sum(case when entry_ty<>'OB' and  (date between @sdate5 and @edate5) and (amt_ty='CR') then  amount else  0 end)
	,@amt6=sum(case when entry_ty<>'OB' and  (date between @sdate6 and @edate6) and (amt_ty='CR') then  amount else  0 end)
	from #st3_5b
	where  (typ='Service Tax Available') and @isdprouct=0 and (sertype='Credit utilized For payment of education cess on taxable service') --(serty=@serty) and

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','1','3','2'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)			

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when entry_ty<>'OB' and  (date between @sdate1 and @edate1) and (amt_ty='CR') then  amount else  0 end)
	,@amt2=sum(case when entry_ty<>'OB' and  (date between @sdate2 and @edate2) and (amt_ty='CR') then  amount else  0 end) 
	,@amt3=sum(case when entry_ty<>'OB' and  (date between @sdate3 and @edate3) and (amt_ty='CR') then  amount else  0 end)
	,@amt4=sum(case when entry_ty<>'OB' and  (date between @sdate4 and @edate4) and (amt_ty='CR') then  amount else  0 end)
	,@amt5=sum(case when entry_ty<>'OB' and  (date between @sdate5 and @edate5) and (amt_ty='CR') then  amount else  0 end)
	,@amt6=sum(case when entry_ty<>'OB' and  (date between @sdate6 and @edate6) and (amt_ty='CR') then  amount else  0 end)
	from #st3_5b
	where  (typ='Service Tax Available') and @isdprouct=0 and (sertype='Credit utilized For payment of secondary and higher education cess on taxable service') --(serty=@serty) and

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','1','3','3'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)			

	--select * from #st3_5b

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when entry_ty<>'OB' and  (date between @sdate1 and @edate1) and (amt_ty='CR') then  amount else  0 end)
	,@amt2=sum(case when entry_ty<>'OB' and  (date between @sdate2 and @edate2) and (amt_ty='CR') then  amount else  0 end) 
	,@amt3=sum(case when entry_ty<>'OB' and  (date between @sdate3 and @edate3) and (amt_ty='CR') then  amount else  0 end)
	,@amt4=sum(case when entry_ty<>'OB' and  (date between @sdate4 and @edate4) and (amt_ty='CR') then  amount else  0 end)
	,@amt5=sum(case when entry_ty<>'OB' and  (date between @sdate5 and @edate5) and (amt_ty='CR') then  amount else  0 end)
	,@amt6=sum(case when entry_ty<>'OB' and  (date between @sdate6 and @edate6) and (amt_ty='CR') then  amount else  0 end)
	from #st3_5b
	where  (typ='Service Tax Available') and @isdprouct=0 --(serty=@serty) and
	and (sertype='Credit utilized For payment of excise or any other duty')

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','1','3','4'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)

 	select 
	 @amt1=sum(case when entry_ty<>'OB' and  (date between @sdate1 and @edate1) and (amt_ty='CR') then  amount else  0 end)
	,@amt2=sum(case when entry_ty<>'OB' and  (date between @sdate2 and @edate2) and (amt_ty='CR') then  amount else  0 end) 
	,@amt3=sum(case when entry_ty<>'OB' and  (date between @sdate3 and @edate3) and (amt_ty='CR') then  amount else  0 end)
	,@amt4=sum(case when entry_ty<>'OB' and  (date between @sdate4 and @edate4) and (amt_ty='CR') then  amount else  0 end)
	,@amt5=sum(case when entry_ty<>'OB' and  (date between @sdate5 and @edate5) and (amt_ty='CR') then  amount else  0 end)
	,@amt6=sum(case when entry_ty<>'OB' and  (date between @sdate6 and @edate6) and (amt_ty='CR') then  amount else  0 end)
	from #st3_5b
	where  (typ='Service Tax Available') and @isdprouct=0 and (sertype='Credit utilized Towards clearance of input goods and capital goods removed as such') --(serty=@serty) and

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','1','3','5'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when entry_ty<>'OB' and  (date between @sdate1 and @edate1) and (amt_ty='CR') then  amount else  0 end)
	,@amt2=sum(case when entry_ty<>'OB' and  (date between @sdate2 and @edate2) and (amt_ty='CR') then  amount else  0 end) 
	,@amt3=sum(case when entry_ty<>'OB' and  (date between @sdate3 and @edate3) and (amt_ty='CR') then  amount else  0 end)
	,@amt4=sum(case when entry_ty<>'OB' and  (date between @sdate4 and @edate4) and (amt_ty='CR') then  amount else  0 end)
	,@amt5=sum(case when entry_ty<>'OB' and  (date between @sdate5 and @edate5) and (amt_ty='CR') then  amount else  0 end)
	,@amt6=sum(case when entry_ty<>'OB' and  (date between @sdate6 and @edate6) and (amt_ty='CR') then  amount else  0 end)
	from #st3_5b
	where  (typ='Service Tax Available') and @isdprouct=0 and (sertype='Credit utilized Towards inter unit transfer of LTU*') --(serty=@serty) and

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','1','3','6'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when entry_ty<>'OB' and  (date between @sdate1 and @edate1) and (amt_ty='CR') then  amount else  0 end)
	,@amt2=sum(case when entry_ty<>'OB' and  (date between @sdate2 and @edate2) and (amt_ty='CR') then  amount else  0 end) 
	,@amt3=sum(case when entry_ty<>'OB' and  (date between @sdate3 and @edate3) and (amt_ty='CR') then  amount else  0 end)
	,@amt4=sum(case when entry_ty<>'OB' and  (date between @sdate4 and @edate4) and (amt_ty='CR') then  amount else  0 end)
	,@amt5=sum(case when entry_ty<>'OB' and  (date between @sdate5 and @edate5) and (amt_ty='CR') then  amount else  0 end)
	,@amt6=sum(case when entry_ty<>'OB' and  (date between @sdate6 and @edate6) and (amt_ty='CR') then  amount else  0 end)
	from #st3_5b
	where  (typ='Service Tax Available') and @isdprouct=0 and (sertype='Credit utilized for payment under rule 6 (3) of the Cenvat Credit Rules(2004)') --(serty=@serty) and

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','1','3','7'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','1','3','8'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	
	select 
	 @amt1=sum(isnull(amt1,0))
	,@amt2=sum(isnull(amt2,0))
	,@amt3=sum(isnull(amt3,0))
	,@amt4=sum(isnull(amt4,0))
	,@amt5=sum(isnull(amt5,0))
	,@amt6=sum(isnull(amt6,0))
	from #st3 where ltrim(rtrim(srno1))='5' and ltrim(rtrim(srno2))='I' and ltrim(rtrim(srno3))='3' and ltrim(rtrim(srno4))='1' and ltrim(rtrim(srno5))='3'


	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','1','3','9'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	--5B1C
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	
	select 
	 @amt1=sum(case when (ltrim(rtrim(srno5))='2' and ltrim(rtrim(srno6))='7') then isnull(amt1,0) else -isnull(amt1,0) end)
	,@amt2=sum(case when (ltrim(rtrim(srno5))='2' and ltrim(rtrim(srno6))='7') then isnull(amt2,0) else -isnull(amt2,0) end)
	,@amt3=sum(case when (ltrim(rtrim(srno5))='2' and ltrim(rtrim(srno6))='7') then isnull(amt3,0) else -isnull(amt3,0) end)
	,@amt4=sum(case when (ltrim(rtrim(srno5))='2' and ltrim(rtrim(srno6))='7') then isnull(amt4,0) else -isnull(amt4,0) end)
	,@amt5=sum(case when (ltrim(rtrim(srno5))='2' and ltrim(rtrim(srno6))='7') then isnull(amt5,0) else -isnull(amt5,0) end)
	,@amt6=sum(case when (ltrim(rtrim(srno5))='2' and ltrim(rtrim(srno6))='7') then isnull(amt6,0) else -isnull(amt6,0) end)
	from #st3 where ltrim(rtrim(srno1))='5' and ltrim(rtrim(srno2))='I' and ltrim(rtrim(srno3))='3' and ltrim(rtrim(srno4))='1' and ((ltrim(rtrim(srno5))='2' and ltrim(rtrim(srno6))='7') or (ltrim(rtrim(srno5))='3' and ltrim(rtrim(srno6))='9'))

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','1','4','0'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	/*<--5B 1*/
	/*-->5B 2*/

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','2','0','0'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)			

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (beh='OB' or date<@sdate1) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
	,@amt2=sum(case when (beh='OB' or date<@sdate2) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
	,@amt3=sum(case when (beh='OB' or date<@sdate3) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
	,@amt4=sum(case when (beh='OB' or date<@sdate4) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
	,@amt5=sum(case when (beh='OB' or date<@sdate5) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
	,@amt6=sum(case when (beh='OB' or date<@sdate6) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
	from #st3_5b
	where 
	@isdprouct=0 and ( typ in('Service Tax Available-Ecess') ) and sertype<>''
 

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','2','1','0'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)			
	
	--5b2b
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','2','2','0'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)	

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when entry_ty<>'OB' and  (date between @sdate1 and @edate1) and (amt_ty='DR') then  amount else  0 end)
	,@amt2=sum(case when entry_ty<>'OB' and  (date between @sdate2 and @edate2) and (amt_ty='DR') then  amount else  0 end) 
	,@amt3=sum(case when entry_ty<>'OB' and  (date between @sdate3 and @edate3) and (amt_ty='DR') then  amount else  0 end)
	,@amt4=sum(case when entry_ty<>'OB' and  (date between @sdate4 and @edate4) and (amt_ty='DR') then  amount else  0 end)
	,@amt5=sum(case when entry_ty<>'OB' and  (date between @sdate5 and @edate5) and (amt_ty='DR') then  amount else  0 end)
	,@amt6=sum(case when entry_ty<>'OB' and  (date between @sdate6 and @edate6) and (amt_ty='DR') then  amount else  0 end)
	from #st3_5b
	where  (typ in('Service Tax Available-Ecess'))  and @isdprouct=0 and (sertype='Credit taken On input')

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','2','2','1'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)			

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when entry_ty<>'OB' and  (date between @sdate1 and @edate1) and (amt_ty='DR') then  amount else  0 end)
	,@amt2=sum(case when entry_ty<>'OB' and  (date between @sdate2 and @edate2) and (amt_ty='DR') then  amount else  0 end) 
	,@amt3=sum(case when entry_ty<>'OB' and  (date between @sdate3 and @edate3) and (amt_ty='DR') then  amount else  0 end)
	,@amt4=sum(case when entry_ty<>'OB' and  (date between @sdate4 and @edate4) and (amt_ty='DR') then  amount else  0 end)
	,@amt5=sum(case when entry_ty<>'OB' and  (date between @sdate5 and @edate5) and (amt_ty='DR') then  amount else  0 end)
	,@amt6=sum(case when entry_ty<>'OB' and  (date between @sdate6 and @edate6) and (amt_ty='DR') then  amount else  0 end)
	from #st3_5b
	where  (typ in('Service Tax Available-Ecess')) and @isdprouct=0 and (sertype='Credit taken On capital goods') 

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','2','2','2'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)			

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when entry_ty<>'OB' and  (date between @sdate1 and @edate1) and (amt_ty='DR') then  amount else  0 end)
	,@amt2=sum(case when entry_ty<>'OB' and  (date between @sdate2 and @edate2) and (amt_ty='DR') then  amount else  0 end) 
	,@amt3=sum(case when entry_ty<>'OB' and  (date between @sdate3 and @edate3) and (amt_ty='DR') then  amount else  0 end)
	,@amt4=sum(case when entry_ty<>'OB' and  (date between @sdate4 and @edate4) and (amt_ty='DR') then  amount else  0 end)
	,@amt5=sum(case when entry_ty<>'OB' and  (date between @sdate5 and @edate5) and (amt_ty='DR') then  amount else  0 end)
	,@amt6=sum(case when entry_ty<>'OB' and  (date between @sdate6 and @edate6) and (amt_ty='DR') then  amount else  0 end)
	from #st3_5b
	where  (typ in('Service Tax Available-Ecess')) and @isdprouct=0 and sertype=''
	and sertype=''

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','2','2','3'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when entry_ty<>'OB' and  (date between @sdate1 and @edate1) and (amt_ty='DR') then  amount else  0 end)
	,@amt2=sum(case when entry_ty<>'OB' and  (date between @sdate2 and @edate2) and (amt_ty='DR') then  amount else  0 end) 
	,@amt3=sum(case when entry_ty<>'OB' and  (date between @sdate3 and @edate3) and (amt_ty='DR') then  amount else  0 end)
	,@amt4=sum(case when entry_ty<>'OB' and  (date between @sdate4 and @edate4) and (amt_ty='DR') then  amount else  0 end)
	,@amt5=sum(case when entry_ty<>'OB' and  (date between @sdate5 and @edate5) and (amt_ty='DR') then  amount else  0 end)
	,@amt6=sum(case when entry_ty<>'OB' and  (date between @sdate6 and @edate6) and (amt_ty='DR') then  amount else  0 end)
	from #st3_5b
	where  (typ in('Service Tax Available-Ecess')) and @isdprouct=0
	and (sertype ='Credit taken As received from input service distributor')

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','2','2','4'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when entry_ty<>'OB' and  (date between @sdate1 and @edate1) and (amt_ty='DR') then  amount else  0 end)
	,@amt2=sum(case when entry_ty<>'OB' and  (date between @sdate2 and @edate2) and (amt_ty='DR') then  amount else  0 end) 
	,@amt3=sum(case when entry_ty<>'OB' and  (date between @sdate3 and @edate3) and (amt_ty='DR') then  amount else  0 end)
	,@amt4=sum(case when entry_ty<>'OB' and  (date between @sdate4 and @edate4) and (amt_ty='DR') then  amount else  0 end)
	,@amt5=sum(case when entry_ty<>'OB' and  (date between @sdate5 and @edate5) and (amt_ty='DR') then  amount else  0 end)
	,@amt6=sum(case when entry_ty<>'OB' and  (date between @sdate6 and @edate6) and (amt_ty='DR') then  amount else  0 end)
	from #st3_5b
	where  (typ in('Service Tax Available-Ecess')) and @isdprouct=0 and (sertype='Credit taken From inter unit transfer by a LTU*') 

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','2','2','5'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	
	select 
	 @amt1=sum(case when entry_ty<>'OB' and  (date between @sdate1 and @edate1) and (amt_ty='DR') then  amount else  0 end)
	,@amt2=sum(case when entry_ty<>'OB' and  (date between @sdate2 and @edate2) and (amt_ty='DR') then  amount else  0 end) 
	,@amt3=sum(case when entry_ty<>'OB' and  (date between @sdate3 and @edate3) and (amt_ty='DR') then  amount else  0 end)
	,@amt4=sum(case when entry_ty<>'OB' and  (date between @sdate4 and @edate4) and (amt_ty='DR') then  amount else  0 end)
	,@amt5=sum(case when entry_ty<>'OB' and  (date between @sdate5 and @edate5) and (amt_ty='DR') then  amount else  0 end)
	,@amt6=sum(case when entry_ty<>'OB' and  (date between @sdate6 and @edate6) and (amt_ty='DR') then  amount else  0 end)
	from #st3_5b
	where  (typ in('Service Tax Available-Ecess')) and @isdprouct=0 and (sertype ='Any other Credit taken') 

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','2','2','6'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(isnull(amt1,0))
	,@amt2=sum(isnull(amt2,0))
	,@amt3=sum(isnull(amt3,0))
	,@amt4=sum(isnull(amt4,0))
	,@amt5=sum(isnull(amt5,0))
	,@amt6=sum(isnull(amt6,0))
	from #st3 where ltrim(rtrim(srno1))='5' and ltrim(rtrim(srno2))='I' and ltrim(rtrim(srno3))='3' and ltrim(rtrim(srno4))='2' and (ltrim(rtrim(srno5))='1' or ltrim(rtrim(srno5))='2')
	
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','2','2','7'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	--5B2B
	--5B2C
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','2','3','0'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)	

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when entry_ty<>'OB' and  (date between @sdate1 and @edate1) and (amt_ty='CR') then  amount else  0 end)
	,@amt2=sum(case when entry_ty<>'OB' and  (date between @sdate2 and @edate2) and (amt_ty='CR') then  amount else  0 end) 
	,@amt3=sum(case when entry_ty<>'OB' and  (date between @sdate3 and @edate3) and (amt_ty='CR') then  amount else  0 end)
	,@amt4=sum(case when entry_ty<>'OB' and  (date between @sdate4 and @edate4) and (amt_ty='CR') then  amount else  0 end)
	,@amt5=sum(case when entry_ty<>'OB' and  (date between @sdate5 and @edate5) and (amt_ty='CR') then  amount else  0 end)
	,@amt6=sum(case when entry_ty<>'OB' and  (date between @sdate6 and @edate6) and (amt_ty='CR') then  amount else  0 end)
	from #st3_5b
	where (typ in('Service Tax Available-Ecess'))  and @isdprouct=0
	and (sertype='Credit utilized For payment of education cess and secondary and higher education cess on goods') 

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0--Not Known
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','2','3','1'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)			


	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when entry_ty<>'OB' and  (date between @sdate1 and @edate1) and (amt_ty='CR') then  amount else  0 end)
	,@amt2=sum(case when entry_ty<>'OB' and  (date between @sdate2 and @edate2) and (amt_ty='CR') then  amount else  0 end) 
	,@amt3=sum(case when entry_ty<>'OB' and  (date between @sdate3 and @edate3) and (amt_ty='CR') then  amount else  0 end)
	,@amt4=sum(case when entry_ty<>'OB' and  (date between @sdate4 and @edate4) and (amt_ty='CR') then  amount else  0 end)
	,@amt5=sum(case when entry_ty<>'OB' and  (date between @sdate5 and @edate5) and (amt_ty='CR') then  amount else  0 end)
	,@amt6=sum(case when entry_ty<>'OB' and  (date between @sdate6 and @edate6) and (amt_ty='CR') then  amount else  0 end)
	from #st3_5b
	where (typ in('Service Tax Available-Ecess')) and @isdprouct=0
	and (sertype ='Credit utilized For Towards payment of education cess and secondary and higher education cess on clearance of input goods and capital goods removed as such') 

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','2','3','2'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when entry_ty<>'OB' and  (date between @sdate1 and @edate1) and (amt_ty='CR') then  amount else  0 end)
	,@amt2=sum(case when entry_ty<>'OB' and  (date between @sdate2 and @edate2) and (amt_ty='CR') then  amount else  0 end) 
	,@amt3=sum(case when entry_ty<>'OB' and  (date between @sdate3 and @edate3) and (amt_ty='CR') then  amount else  0 end)
	,@amt4=sum(case when entry_ty<>'OB' and  (date between @sdate4 and @edate4) and (amt_ty='CR') then  amount else  0 end)
	,@amt5=sum(case when entry_ty<>'OB' and  (date between @sdate5 and @edate5) and (amt_ty='CR') then  amount else  0 end)
	,@amt6=sum(case when entry_ty<>'OB' and  (date between @sdate6 and @edate6) and (amt_ty='CR') then  amount else  0 end)
	from #st3_5b
	where  (typ in('Service Tax Available-Ecess'))  and @isdprouct=0
	and (sertype='Credit utilized Towards inter unit transfer of LTU*') 
	

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','2','3','3'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when entry_ty<>'OB' and  (date between @sdate1 and @edate1) and (amt_ty='CR') then  amount else  0 end)
	,@amt2=sum(case when entry_ty<>'OB' and  (date between @sdate2 and @edate2) and (amt_ty='CR') then  amount else  0 end) 
	,@amt3=sum(case when entry_ty<>'OB' and  (date between @sdate3 and @edate3) and (amt_ty='CR') then  amount else  0 end)
	,@amt4=sum(case when entry_ty<>'OB' and  (date between @sdate4 and @edate4) and (amt_ty='CR') then  amount else  0 end)
	,@amt5=sum(case when entry_ty<>'OB' and  (date between @sdate5 and @edate5) and (amt_ty='CR') then  amount else  0 end)
	,@amt6=sum(case when entry_ty<>'OB' and  (date between @sdate6 and @edate6) and (amt_ty='CR') then  amount else  0 end)
	from #st3_5b
	where  (typ in('Service Tax Available-Ecess'))  and @isdprouct=0
	and (sertype ='Any other Credit taken') 
	

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','2','3','4'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	--5B2C

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(isnull(amt1,0))
	,@amt2=sum(isnull(amt2,0))
	,@amt3=sum(isnull(amt3,0))
	,@amt4=sum(isnull(amt4,0))
	,@amt5=sum(isnull(amt5,0))
	,@amt6=sum(isnull(amt6,0))
	from #st3 where ltrim(rtrim(srno1))='5' and ltrim(rtrim(srno2))='I' and ltrim(rtrim(srno3))='3' and ltrim(rtrim(srno4))='2' and ltrim(rtrim(srno5))='3'

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','2','3','5'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)


	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (ltrim(rtrim(srno5))='2' and ltrim(rtrim(srno6))='7') then isnull(amt1,0) else -isnull(amt1,0) end)
	,@amt2=sum(case when (ltrim(rtrim(srno5))='2' and ltrim(rtrim(srno6))='7') then isnull(amt2,0) else -isnull(amt2,0) end)
	,@amt3=sum(case when (ltrim(rtrim(srno5))='2' and ltrim(rtrim(srno6))='7') then isnull(amt3,0) else -isnull(amt3,0) end)
	,@amt4=sum(case when (ltrim(rtrim(srno5))='2' and ltrim(rtrim(srno6))='7') then isnull(amt4,0) else -isnull(amt4,0) end)
	,@amt5=sum(case when (ltrim(rtrim(srno5))='2' and ltrim(rtrim(srno6))='7') then isnull(amt5,0) else -isnull(amt5,0) end)
	,@amt6=sum(case when (ltrim(rtrim(srno5))='2' and ltrim(rtrim(srno6))='7') then isnull(amt6,0) else -isnull(amt6,0) end)
	from #st3 where ltrim(rtrim(srno1))='5' and ltrim(rtrim(srno2))='I' and ltrim(rtrim(srno3))='3' and ltrim(rtrim(srno4))='2' and ((ltrim(rtrim(srno5))='2' and ltrim(rtrim(srno6))='7') or (ltrim(rtrim(srno5))='3' and ltrim(rtrim(srno6))='5'))

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','2','4','0'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)

	/*<--5B 2*/
----------------------
	/*-->5B 3*/

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','3','0','0'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)			

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (beh='OB' or date<@sdate1) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
	,@amt2=sum(case when (beh='OB' or date<@sdate2) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
	,@amt3=sum(case when (beh='OB' or date<@sdate3) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
	,@amt4=sum(case when (beh='OB' or date<@sdate4) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
	,@amt5=sum(case when (beh='OB' or date<@sdate5) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
	,@amt6=sum(case when (beh='OB' or date<@sdate6) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
	from #st3_5b
	where 
	@isdprouct=0 and ( typ in('Service Tax Available-Hcess') ) and sertype<>''
 

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','3','1','0'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)			
	
	--5b2b
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','3','2','0'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)	

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when entry_ty<>'OB' and  (date between @sdate1 and @edate1) and (amt_ty='DR') then  amount else  0 end)
	,@amt2=sum(case when entry_ty<>'OB' and  (date between @sdate2 and @edate2) and (amt_ty='DR') then  amount else  0 end) 
	,@amt3=sum(case when entry_ty<>'OB' and  (date between @sdate3 and @edate3) and (amt_ty='DR') then  amount else  0 end)
	,@amt4=sum(case when entry_ty<>'OB' and  (date between @sdate4 and @edate4) and (amt_ty='DR') then  amount else  0 end)
	,@amt5=sum(case when entry_ty<>'OB' and  (date between @sdate5 and @edate5) and (amt_ty='DR') then  amount else  0 end)
	,@amt6=sum(case when entry_ty<>'OB' and  (date between @sdate6 and @edate6) and (amt_ty='DR') then  amount else  0 end)
	from #st3_5b
	where  (typ in('Service Tax Available-Hcess'))  and @isdprouct=0 and (sertype='Credit taken On input')

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','3','2','1'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)			

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when entry_ty<>'OB' and  (date between @sdate1 and @edate1) and (amt_ty='DR') then  amount else  0 end)
	,@amt2=sum(case when entry_ty<>'OB' and  (date between @sdate2 and @edate2) and (amt_ty='DR') then  amount else  0 end) 
	,@amt3=sum(case when entry_ty<>'OB' and  (date between @sdate3 and @edate3) and (amt_ty='DR') then  amount else  0 end)
	,@amt4=sum(case when entry_ty<>'OB' and  (date between @sdate4 and @edate4) and (amt_ty='DR') then  amount else  0 end)
	,@amt5=sum(case when entry_ty<>'OB' and  (date between @sdate5 and @edate5) and (amt_ty='DR') then  amount else  0 end)
	,@amt6=sum(case when entry_ty<>'OB' and  (date between @sdate6 and @edate6) and (amt_ty='DR') then  amount else  0 end)
	from #st3_5b
	where  (typ in('Service Tax Available-Hcess')) and @isdprouct=0 and (sertype='Credit taken On capital goods') 

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','3','2','2'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)			

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when entry_ty<>'OB' and  (date between @sdate1 and @edate1) and (amt_ty='DR') then  amount else  0 end)
	,@amt2=sum(case when entry_ty<>'OB' and  (date between @sdate2 and @edate2) and (amt_ty='DR') then  amount else  0 end) 
	,@amt3=sum(case when entry_ty<>'OB' and  (date between @sdate3 and @edate3) and (amt_ty='DR') then  amount else  0 end)
	,@amt4=sum(case when entry_ty<>'OB' and  (date between @sdate4 and @edate4) and (amt_ty='DR') then  amount else  0 end)
	,@amt5=sum(case when entry_ty<>'OB' and  (date between @sdate5 and @edate5) and (amt_ty='DR') then  amount else  0 end)
	,@amt6=sum(case when entry_ty<>'OB' and  (date between @sdate6 and @edate6) and (amt_ty='DR') then  amount else  0 end)
	from #st3_5b
	where  (typ in('Service Tax Available-Hcess')) and @isdprouct=0 and sertype=''
	and sertype=''

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','3','2','3'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when entry_ty<>'OB' and  (date between @sdate1 and @edate1) and (amt_ty='DR') then  amount else  0 end)
	,@amt2=sum(case when entry_ty<>'OB' and  (date between @sdate2 and @edate2) and (amt_ty='DR') then  amount else  0 end) 
	,@amt3=sum(case when entry_ty<>'OB' and  (date between @sdate3 and @edate3) and (amt_ty='DR') then  amount else  0 end)
	,@amt4=sum(case when entry_ty<>'OB' and  (date between @sdate4 and @edate4) and (amt_ty='DR') then  amount else  0 end)
	,@amt5=sum(case when entry_ty<>'OB' and  (date between @sdate5 and @edate5) and (amt_ty='DR') then  amount else  0 end)
	,@amt6=sum(case when entry_ty<>'OB' and  (date between @sdate6 and @edate6) and (amt_ty='DR') then  amount else  0 end)
	from #st3_5b
	where  (typ in('Service Tax Available-Hcess')) and @isdprouct=0
	and (sertype ='Credit taken As received from input service distributor')

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','3','2','4'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when entry_ty<>'OB' and  (date between @sdate1 and @edate1) and (amt_ty='DR') then  amount else  0 end)
	,@amt2=sum(case when entry_ty<>'OB' and  (date between @sdate2 and @edate2) and (amt_ty='DR') then  amount else  0 end) 
	,@amt3=sum(case when entry_ty<>'OB' and  (date between @sdate3 and @edate3) and (amt_ty='DR') then  amount else  0 end)
	,@amt4=sum(case when entry_ty<>'OB' and  (date between @sdate4 and @edate4) and (amt_ty='DR') then  amount else  0 end)
	,@amt5=sum(case when entry_ty<>'OB' and  (date between @sdate5 and @edate5) and (amt_ty='DR') then  amount else  0 end)
	,@amt6=sum(case when entry_ty<>'OB' and  (date between @sdate6 and @edate6) and (amt_ty='DR') then  amount else  0 end)
	from #st3_5b
	where  (typ in('Service Tax Available-Hcess')) and @isdprouct=0 and (sertype='Credit taken From inter unit transfer by a LTU*') 

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','3','2','5'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when entry_ty<>'OB' and  (date between @sdate1 and @edate1) and (amt_ty='DR') then  amount else  0 end)
	,@amt2=sum(case when entry_ty<>'OB' and  (date between @sdate2 and @edate2) and (amt_ty='DR') then  amount else  0 end) 
	,@amt3=sum(case when entry_ty<>'OB' and  (date between @sdate3 and @edate3) and (amt_ty='DR') then  amount else  0 end)
	,@amt4=sum(case when entry_ty<>'OB' and  (date between @sdate4 and @edate4) and (amt_ty='DR') then  amount else  0 end)
	,@amt5=sum(case when entry_ty<>'OB' and  (date between @sdate5 and @edate5) and (amt_ty='DR') then  amount else  0 end)
	,@amt6=sum(case when entry_ty<>'OB' and  (date between @sdate6 and @edate6) and (amt_ty='DR') then  amount else  0 end)
	from #st3_5b
	where  (typ in('Service Tax Available-Hcess')) and @isdprouct=0 and (sertype ='Any other Credit taken') 
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','3','2','6'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(isnull(amt1,0))
	,@amt2=sum(isnull(amt2,0))
	,@amt3=sum(isnull(amt3,0))
	,@amt4=sum(isnull(amt4,0))
	,@amt5=sum(isnull(amt5,0))
	,@amt6=sum(isnull(amt6,0))
	from #st3 where  ltrim(rtrim(srno1))='5' and ltrim(rtrim(srno2))='I' and ltrim(rtrim(srno3))='3' and ltrim(rtrim(srno4))='3' and (ltrim(rtrim(srno5))='1' or ltrim(rtrim(srno5))='2')

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','3','2','7'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	--5B2B
	--5B2C
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','3','3','0'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)	

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when entry_ty<>'OB' and  (date between @sdate1 and @edate1) and (amt_ty='CR') then  amount else  0 end)
	,@amt2=sum(case when entry_ty<>'OB' and  (date between @sdate2 and @edate2) and (amt_ty='CR') then  amount else  0 end) 
	,@amt3=sum(case when entry_ty<>'OB' and  (date between @sdate3 and @edate3) and (amt_ty='CR') then  amount else  0 end)
	,@amt4=sum(case when entry_ty<>'OB' and  (date between @sdate4 and @edate4) and (amt_ty='CR') then  amount else  0 end)
	,@amt5=sum(case when entry_ty<>'OB' and  (date between @sdate5 and @edate5) and (amt_ty='CR') then  amount else  0 end)
	,@amt6=sum(case when entry_ty<>'OB' and  (date between @sdate6 and @edate6) and (amt_ty='CR') then  amount else  0 end)
	from #st3_5b
	where (typ in('Service Tax Available-Hcess'))  and @isdprouct=0
	and (sertype='Credit utilized For payment of education cess and secondary and higher education cess on goods') 

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0--Not Known
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','3','3','1'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)			


	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when entry_ty<>'OB' and  (date between @sdate1 and @edate1) and (amt_ty='CR') then  amount else  0 end)
	,@amt2=sum(case when entry_ty<>'OB' and  (date between @sdate2 and @edate2) and (amt_ty='CR') then  amount else  0 end) 
	,@amt3=sum(case when entry_ty<>'OB' and  (date between @sdate3 and @edate3) and (amt_ty='CR') then  amount else  0 end)
	,@amt4=sum(case when entry_ty<>'OB' and  (date between @sdate4 and @edate4) and (amt_ty='CR') then  amount else  0 end)
	,@amt5=sum(case when entry_ty<>'OB' and  (date between @sdate5 and @edate5) and (amt_ty='CR') then  amount else  0 end)
	,@amt6=sum(case when entry_ty<>'OB' and  (date between @sdate6 and @edate6) and (amt_ty='CR') then  amount else  0 end)
	from #st3_5b
	where (typ in('Service Tax Available-Hcess')) and @isdprouct=0
	and (sertype ='Credit utilized For Towards payment of education cess and secondary and higher education cess on clearance of input goods and capital goods removed as such') 

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','3','3','2'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when entry_ty<>'OB' and  (date between @sdate1 and @edate1) and (amt_ty='CR') then  amount else  0 end)
	,@amt2=sum(case when entry_ty<>'OB' and  (date between @sdate2 and @edate2) and (amt_ty='CR') then  amount else  0 end) 
	,@amt3=sum(case when entry_ty<>'OB' and  (date between @sdate3 and @edate3) and (amt_ty='CR') then  amount else  0 end)
	,@amt4=sum(case when entry_ty<>'OB' and  (date between @sdate4 and @edate4) and (amt_ty='CR') then  amount else  0 end)
	,@amt5=sum(case when entry_ty<>'OB' and  (date between @sdate5 and @edate5) and (amt_ty='CR') then  amount else  0 end)
	,@amt6=sum(case when entry_ty<>'OB' and  (date between @sdate6 and @edate6) and (amt_ty='CR') then  amount else  0 end)
	from #st3_5b
	where  (typ in('Service Tax Available-Hcess'))  and @isdprouct=0
	and (sertype='Credit utilized Towards inter unit transfer of LTU*') 
	

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','3','3','3'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','3','3','4'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	--5B2C

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(isnull(amt1,0))
	,@amt2=sum(isnull(amt2,0))
	,@amt3=sum(isnull(amt3,0))
	,@amt4=sum(isnull(amt4,0))
	,@amt5=sum(isnull(amt5,0))
	,@amt6=sum(isnull(amt6,0))
	from #st3 where ltrim(rtrim(srno1))='5' and ltrim(rtrim(srno2))='I' and ltrim(rtrim(srno3))='3' and ltrim(rtrim(srno4))='3' and ltrim(rtrim(srno5))='3'

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','3','3','5'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (ltrim(rtrim(srno5))='2' and ltrim(rtrim(srno6))='7') then isnull(amt1,0) else -isnull(amt1,0) end)
	,@amt2=sum(case when (ltrim(rtrim(srno5))='2' and ltrim(rtrim(srno6))='7') then isnull(amt2,0) else -isnull(amt2,0) end)
	,@amt3=sum(case when (ltrim(rtrim(srno5))='2' and ltrim(rtrim(srno6))='7') then isnull(amt3,0) else -isnull(amt3,0) end)
	,@amt4=sum(case when (ltrim(rtrim(srno5))='2' and ltrim(rtrim(srno6))='7') then isnull(amt4,0) else -isnull(amt4,0) end)
	,@amt5=sum(case when (ltrim(rtrim(srno5))='2' and ltrim(rtrim(srno6))='7') then isnull(amt5,0) else -isnull(amt5,0) end)
	,@amt6=sum(case when (ltrim(rtrim(srno5))='2' and ltrim(rtrim(srno6))='7') then isnull(amt6,0) else -isnull(amt6,0) end)
	from #st3 where ltrim(rtrim(srno1))='5' and ltrim(rtrim(srno2))='I' and ltrim(rtrim(srno3))='3' and ltrim(rtrim(srno4))='3' and ((ltrim(rtrim(srno5))='2' and ltrim(rtrim(srno6))='7') or (ltrim(rtrim(srno5))='3' and ltrim(rtrim(srno6))='5'))

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','5','I','3','3','4','0'
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)

	/*<--5B 3*/
	
/*<---5B*/
/*--->6*/
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','6','J','0','0','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)			

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','6','J','1','0','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)			

	if @isdprouct=1
	begin
		select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
		select 
		@amt1=sum(case when (beh='OB' or date<@sdate1) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
		,@amt2=sum(case when (beh='OB' or date<@sdate2) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
		,@amt3=sum(case when (beh='OB' or date<@sdate3) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
		,@amt4=sum(case when (beh='OB' or date<@sdate4) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
		,@amt5=sum(case when (beh='OB' or date<@sdate5) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
		,@amt6=sum(case when (beh='OB' or date<@sdate6) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
		from #st3_5b
		where typ in ('Service Tax Available')
	end
	else
	begin
		select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	end
  			
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','6','J','1','1','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)			
	--601b
	if @isdprouct=1
	begin
		select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
		select 
		@amt1=sum(case when entry_ty<>'OB' and  (date between @sdate1 and @edate1) and (amt_ty='DR') then  amount else  0 end)
		,@amt2=sum(case when entry_ty<>'OB' and  (date between @sdate2 and @edate2) and (amt_ty='DR') then  amount else  0 end) 
		,@amt3=sum(case when entry_ty<>'OB' and  (date between @sdate3 and @edate3) and (amt_ty='DR') then  amount else  0 end)
		,@amt4=sum(case when entry_ty<>'OB' and  (date between @sdate4 and @edate4) and (amt_ty='DR') then  amount else  0 end)
		,@amt5=sum(case when entry_ty<>'OB' and  (date between @sdate5 and @edate5) and (amt_ty='DR') then  amount else  0 end)
		,@amt6=sum(case when entry_ty<>'OB' and  (date between @sdate6 and @edate6) and (amt_ty='DR') then  amount else  0 end)
		from #st3_5b
		where  typ in ('Service Tax Available')
		select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)
	end
	else
	begin
		select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	end	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','6','J','1','2','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)	

	if @isdprouct=1
	begin
		select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
		select 
		@amt1=sum(case when entry_ty<>'OB' and  (date between @sdate1 and @edate1) and (amt_ty='CR') then  amount else  0 end)
		,@amt2=sum(case when entry_ty<>'OB' and  (date between @sdate2 and @edate2) and (amt_ty='CR') then  amount else  0 end) 
		,@amt3=sum(case when entry_ty<>'OB' and  (date between @sdate3 and @edate3) and (amt_ty='CR') then  amount else  0 end)
		,@amt4=sum(case when entry_ty<>'OB' and  (date between @sdate4 and @edate4) and (amt_ty='CR') then  amount else  0 end)
		,@amt5=sum(case when entry_ty<>'OB' and  (date between @sdate5 and @edate5) and (amt_ty='CR') then  amount else  0 end)
		,@amt6=sum(case when entry_ty<>'OB' and  (date between @sdate6 and @edate6) and (amt_ty='CR') then  amount else  0 end)
		from #st3_5b
		where  (typ='Service Tax Available')
	end
	else
	begin
		select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	end	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','6','J','1','3','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','6','J','1','4','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (ltrim(rtrim(srno4))='1' or ltrim(rtrim(srno4))='2') then isnull(amt1,0) else -isnull(amt1,0) end)
	,@amt2=sum(case when (ltrim(rtrim(srno4))='1' or ltrim(rtrim(srno4))='2') then isnull(amt2,0) else -isnull(amt2,0) end)
	,@amt3=sum(case when (ltrim(rtrim(srno4))='1' or ltrim(rtrim(srno4))='2') then isnull(amt3,0) else -isnull(amt3,0) end)
	,@amt4=sum(case when (ltrim(rtrim(srno4))='1' or ltrim(rtrim(srno4))='2') then isnull(amt4,0) else -isnull(amt4,0) end)
	,@amt5=sum(case when (ltrim(rtrim(srno4))='1' or ltrim(rtrim(srno4))='2') then isnull(amt5,0) else -isnull(amt5,0) end)
	,@amt6=sum(case when (ltrim(rtrim(srno4))='1' or ltrim(rtrim(srno4))='2') then isnull(amt6,0) else -isnull(amt6,0) end)
	from #st3 where srno1='6' and srno2='J' and srno3='1'

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)

	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','6','J','1','5','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','6','J','2','0','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)			

	if @isdprouct=1
	begin
		select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
		select 
		@amt1=sum(case when (beh='OB' or date<@sdate1) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
		,@amt2=sum(case when (beh='OB' or date<@sdate2) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
		,@amt3=sum(case when (beh='OB' or date<@sdate3) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
		,@amt4=sum(case when (beh='OB' or date<@sdate4) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
		,@amt5=sum(case when (beh='OB' or date<@sdate5) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
		,@amt6=sum(case when (beh='OB' or date<@sdate6) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
		from #st3_5b
		where typ in('Service Tax Available-Ecess')
	end
	else
	begin
		select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	end
  			
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','6','J','2','1','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)			
	--601b
	if @isdprouct=1
	begin
		select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
		select 
		@amt1=sum(case when entry_ty<>'OB' and  (date between @sdate1 and @edate1) and (amt_ty='DR') then  amount else  0 end)
		,@amt2=sum(case when entry_ty<>'OB' and  (date between @sdate2 and @edate2) and (amt_ty='DR') then  amount else  0 end) 
		,@amt3=sum(case when entry_ty<>'OB' and  (date between @sdate3 and @edate3) and (amt_ty='DR') then  amount else  0 end)
		,@amt4=sum(case when entry_ty<>'OB' and  (date between @sdate4 and @edate4) and (amt_ty='DR') then  amount else  0 end)
		,@amt5=sum(case when entry_ty<>'OB' and  (date between @sdate5 and @edate5) and (amt_ty='DR') then  amount else  0 end)
		,@amt6=sum(case when entry_ty<>'OB' and  (date between @sdate6 and @edate6) and (amt_ty='DR') then  amount else  0 end)
		from #st3_5b
		where  typ in('Service Tax Available-Ecess')
		select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)
	end
	else
	begin
		select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	end	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','6','J','2','2','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)	

	if @isdprouct=1
	begin
		select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
		select 
		@amt1=sum(case when entry_ty<>'OB' and  (date between @sdate1 and @edate1) and (amt_ty='CR') then  amount else  0 end)
		,@amt2=sum(case when entry_ty<>'OB' and  (date between @sdate2 and @edate2) and (amt_ty='CR') then  amount else  0 end) 
		,@amt3=sum(case when entry_ty<>'OB' and  (date between @sdate3 and @edate3) and (amt_ty='CR') then  amount else  0 end)
		,@amt4=sum(case when entry_ty<>'OB' and  (date between @sdate4 and @edate4) and (amt_ty='CR') then  amount else  0 end)
		,@amt5=sum(case when entry_ty<>'OB' and  (date between @sdate5 and @edate5) and (amt_ty='CR') then  amount else  0 end)
		,@amt6=sum(case when entry_ty<>'OB' and  (date between @sdate6 and @edate6) and (amt_ty='CR') then  amount else  0 end)
		from #st3_5b
		where typ in('Service Tax Available-Ecess')
	end
	else
	begin
		select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	end	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','6','J','2','3','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','6','J','2','4','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (ltrim(rtrim(srno4))='1' or ltrim(rtrim(srno4))='2') then isnull(amt1,0) else -isnull(amt1,0) end)
	,@amt2=sum(case when (ltrim(rtrim(srno4))='1' or ltrim(rtrim(srno4))='2') then isnull(amt2,0) else -isnull(amt2,0) end)
	,@amt3=sum(case when (ltrim(rtrim(srno4))='1' or ltrim(rtrim(srno4))='2') then isnull(amt3,0) else -isnull(amt3,0) end)
	,@amt4=sum(case when (ltrim(rtrim(srno4))='1' or ltrim(rtrim(srno4))='2') then isnull(amt4,0) else -isnull(amt4,0) end)
	,@amt5=sum(case when (ltrim(rtrim(srno4))='1' or ltrim(rtrim(srno4))='2') then isnull(amt5,0) else -isnull(amt5,0) end)
	,@amt6=sum(case when (ltrim(rtrim(srno4))='1' or ltrim(rtrim(srno4))='2') then isnull(amt6,0) else -isnull(amt6,0) end)
	from #st3 where srno1='6' and srno2='J' and srno3='2'

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','6','J','2','5','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)


	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','6','J','3','0','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)			

	if @isdprouct=1
	begin
		select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
		select 
		@amt1=sum(case when (beh='OB' or date<@sdate1) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
		,@amt2=sum(case when (beh='OB' or date<@sdate2) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
		,@amt3=sum(case when (beh='OB' or date<@sdate3) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
		,@amt4=sum(case when (beh='OB' or date<@sdate4) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
		,@amt5=sum(case when (beh='OB' or date<@sdate5) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
		,@amt6=sum(case when (beh='OB' or date<@sdate6) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
		from #st3_5b
		where typ in('Service Tax Available-Hcess')
	end
	else
	begin
		select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	end
  			
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','6','J','3','1','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)			
	--601b
	if @isdprouct=1
	begin
		select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
		select 
		@amt1=sum(case when entry_ty<>'OB' and  (date between @sdate1 and @edate1) and (amt_ty='DR') then  amount else  0 end)
		,@amt2=sum(case when entry_ty<>'OB' and  (date between @sdate2 and @edate2) and (amt_ty='DR') then  amount else  0 end) 
		,@amt3=sum(case when entry_ty<>'OB' and  (date between @sdate3 and @edate3) and (amt_ty='DR') then  amount else  0 end)
		,@amt4=sum(case when entry_ty<>'OB' and  (date between @sdate4 and @edate4) and (amt_ty='DR') then  amount else  0 end)
		,@amt5=sum(case when entry_ty<>'OB' and  (date between @sdate5 and @edate5) and (amt_ty='DR') then  amount else  0 end)
		,@amt6=sum(case when entry_ty<>'OB' and  (date between @sdate6 and @edate6) and (amt_ty='DR') then  amount else  0 end)
		from #st3_5b
		where typ in('Service Tax Available-Hcess')

		select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)
	end
	else
	begin
		select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	end	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','6','J','3','2','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)	

	if @isdprouct=1
	begin
		select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
		select 
		@amt1=sum(case when entry_ty<>'OB' and  (date between @sdate1 and @edate1) and (amt_ty='CR') then  amount else  0 end)
		,@amt2=sum(case when entry_ty<>'OB' and  (date between @sdate2 and @edate2) and (amt_ty='CR') then  amount else  0 end) 
		,@amt3=sum(case when entry_ty<>'OB' and  (date between @sdate3 and @edate3) and (amt_ty='CR') then  amount else  0 end)
		,@amt4=sum(case when entry_ty<>'OB' and  (date between @sdate4 and @edate4) and (amt_ty='CR') then  amount else  0 end)
		,@amt5=sum(case when entry_ty<>'OB' and  (date between @sdate5 and @edate5) and (amt_ty='CR') then  amount else  0 end)
		,@amt6=sum(case when entry_ty<>'OB' and  (date between @sdate6 and @edate6) and (amt_ty='CR') then  amount else  0 end)
		from #st3_5b
		where typ in('Service Tax Available-Hcess')
	end
	else
	begin
		select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	end	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','6','J','3','3','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','6','J','3','4','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (ltrim(rtrim(srno4))='1' or ltrim(rtrim(srno4))='2') then isnull(amt1,0) else -isnull(amt1,0) end)
	,@amt2=sum(case when (ltrim(rtrim(srno4))='1' or ltrim(rtrim(srno4))='2') then isnull(amt2,0) else -isnull(amt2,0) end)
	,@amt3=sum(case when (ltrim(rtrim(srno4))='1' or ltrim(rtrim(srno4))='2') then isnull(amt3,0) else -isnull(amt3,0) end)
	,@amt4=sum(case when (ltrim(rtrim(srno4))='1' or ltrim(rtrim(srno4))='2') then isnull(amt4,0) else -isnull(amt4,0) end)
	,@amt5=sum(case when (ltrim(rtrim(srno4))='1' or ltrim(rtrim(srno4))='2') then isnull(amt5,0) else -isnull(amt5,0) end)
	,@amt6=sum(case when (ltrim(rtrim(srno4))='1' or ltrim(rtrim(srno4))='2') then isnull(amt6,0) else -isnull(amt6,0) end)
	from #st3 where srno1='6' and srno2='J' and srno3='3'

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT,serbcper,serbcess
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','6','J','3','5','',''
	,0,0,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)

/*<---6*/


select distinct Serty,Code,St3_Name,St3_Id into #SerTax_Mast1 from #SerTax_Mast /*TKT-8320 GTA*/

select SrNo=srno1+srno2+srno3+srno4+srno5+srno6,tAmt1=amt1+amt2+amt3,tAmt2=amt4+amt5+amt6
,tChalNo1=ChalNo1+ChalNo2+ChalNo3,tChalNo2=ChalNo4+ChalNo5+ChalNo6
,srno1,srno2,srno3,srno4,srno5,srno6,amt1,amt2,amt3,amt4,amt5,amt6
,ChalNo1,ChalNo2,ChalNo3,ChalNo4,ChalNo5,ChalNo6
,ChalDt1,ChalDt2,ChalDt3,ChalNo4,ChalNo5,ChalNo6
, L_YN=substring(@LYN,1,4)+'-'+substring(@LYN,8,2),a.* ,b.code,b.st3_id,St3Name = isnull(b.St3_Name,a.Serty)
from #st3 a
left join #SerTax_Mast1 b on (a.serty=b.serty)  /*TKT-8320 GTA*/
order by 
(case when a.srno1='3' then isnull(b.St3_Name,a.Serty) else 'z' end)
,(case when a.srno1='3' then a.Serty else 'z' end)
,(case when a.srno1='3' and srno2='A' then 'A'+a.srno3+a.srno4+a.srno5 else 'Z' end)
,case when isnull(a.sProvider,'No') = 'Yes' then 'A' else 'Z' end
,case when isnull(a.sReceiver,'No') = 'Yes' then 'A' else 'Z' end
,a.srno1,a.srno2,a.srno3,a.srno4,a.srno5,a.srno6
,a.ChalDt6,a.ChalDt5,a.ChalDt4,a.ChalDt3,a.ChalDt2,a.ChalDt1



end--sp










