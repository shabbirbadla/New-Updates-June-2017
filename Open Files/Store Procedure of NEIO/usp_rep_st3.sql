set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


-- =============================================
-- Author:		Ruepesh Prajapati.
-- Create date: 11/07/2008
-- Description:	This Stored procedure is useful to generate Service Tax ST 3 Report .
-- Modification Date/By/Reason: 11/09/2009 Rupesh Prajapati. Modified for Blank Record For 3<->4 part.
-- Modification Date/By/Reason: 01/12/2009 Rupesh Prajapati. Modified for ISD Product Head 6.
-- Modification Date/By/Reason: 03/12/2009 Rupesh Prajapati. Modified for Arrears options in Head 4A I (d).
-- Modification Date/By/Reason: 08/12/2009 Rupesh Prajapati. Modified for Abatement amount claimed 3F1D,3F1F(i).Changes Done With /*Rup 08Dec09*/
-- Modification Date/By/Reason: 12/12/2009 Rupesh Prajapati. Modified for ISD Default Service Category.Changes Done With /*Rup 12/12/2009*/
-- Remark:
-- =============================================
ALTER procedure [dbo].[usp_rep_st3]
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
declare @particulars varchar(250),@particulars1 varchar(250)
declare @c int,@m int,@y int,@strdt varchar(10)
declare @u_chalno varchar(10),@u_chaldt smalldatetime,@date smalldatetime
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

select distinct ac.entry_ty,ac.tran_cd,ac_mast.ac_name,ac.amount,ac.amt_ty,ac.date
,serty=case when (isnull(bpm.tdspaytype,1)=2)  then isnull(bpm.serty,'') else (case when (isnull(cpm.tdspaytype,1)=2)  then isnull(cpm.serty,'') else (case when (isnull(bpm.tdspaytype,1)=2)  then isnull(bpm.serty,'') else (case when (isnull(jvm.entry_ty,'')<>'')  then isnull(jvm.serty,'') else (isnull(m1.serty,'')) end) end) end) end
,sertype=case when (isnull(bpm.tdspaytype,1)=2)  then isnull(bpm.sertype,'') else (case when (isnull(cpm.tdspaytype,1)=2)  then isnull(cpm.sertype,'') else (case when (isnull(bpm.tdspaytype,1)=2)  then isnull(bpm.sertype,'') else (case when (isnull(jvm.entry_ty,'')<>'')  then isnull(jvm.sertype,'') else (isnull(m1.sertype,'')) end) end) end) end
,beh=(case when l.ext_vou=1 then l.bcode_nm else l.entry_ty end)
,ac_mast.typ
into #st3_5b
from lac_vw ac 
inner join ac_mast ac_mast on (ac.ac_id=ac_mast.ac_id) 
inner join lcode l on (ac.entry_ty=l.entry_ty)
left join lmain_vw m on (m.entry_ty=ac.entry_ty and m.tran_cd =ac.tran_cd)
left join bpmain bpm on (bpm.entry_ty=ac.entry_ty and bpm.tran_cd =ac.tran_cd)
left join cpmain cpm on (cpm.entry_ty=ac.entry_ty and cpm.tran_cd =ac.tran_cd)
left join jvmain jvm on (jvm.entry_ty=ac.entry_ty and jvm.tran_cd =ac.tran_cd)
left join mainall_vw mall on (ac.entry_ty=mall.entry_ty and ac.tran_cd=mall.tran_cd)
left join epmain m1 on (m1.entry_ty=mall.entry_all and m1.tran_cd=mall.main_tran)
where ac_mast.ac_name like '%service tax available%'
and (ac.date <= @edate)
order by ac.tran_cd,ac_mast.ac_name	

--select * from #st3_5b

select ac_mast.ac_name,m.inv_no,serty=(case when isnull(m.serty,'')='' then isnull(m1.serty,'') else isnull(m.serty,'') end)
,ac.date,ac.amt_ty
,ac_mast.typ,m.tdspaytype
,gro_amt=case when m.tdspaytype=2 then  m.gro_amt+m.tot_deduc+m.tot_tax else mall.new_all- (isnull(mall.serbamt,0)+isnull(mall.sercamt,0)+isnull(mall.serhamt,0)) end 
--,gro_amt=case when m.tdspaytype=2 then  m.gro_amt+m.tot_deduc+m.tot_tax else mall.new_all- (isnull(mall.serbamt,0)+isnull(mall.sercamt,0)+isnull(mall.serhamt,0)) end 
--,gro_amt=case when m.tdspaytype=2 then  m.gro_amt+m.tot_deduc+m.tot_tax else m.gro_amt end
--,taxable_amt=case when m.tdspaytype=2 then  m.gro_amt+m.tot_deduc+m.tot_tax else m1.gro_amt+m1.tot_deduc+m1.tot_tax end
--,taxable_amt=case when m.tdspaytype=2 then  m.gro_amt+m.tot_deduc+m.tot_tax else m.gro_amt end
,taxable_amt=case when m.tdspaytype=2 then  m.gro_amt+m.tot_deduc+m.tot_tax else mall.new_all- (isnull(mall.serbamt,0)+isnull(mall.sercamt,0)+isnull(mall.serhamt,0)) end 
,sabtamt    =case when m.tdspaytype=2 then  m.sabtamt else isnull(m1.sabtamt,0) end  /*Rup 08Dec09*/
,serbper    =case when m.tdspaytype=2 then  m.serbper else m1.serbper end
,serbamt	=case when m.tdspaytype=2 then  m.serbamt else isnull(mall.serbamt,0) end
,sercper    =case when m.tdspaytype=2 then  m.sercper else m1.sercper end
,sercamt    =case when m.tdspaytype=2 then  m.sercamt else isnull(mall.sercamt,0) end
,serhper    =case when m.tdspaytype=2 then  m.serhper else m1.serhper end
,serhamt    =case when m.tdspaytype=2 then  m.serhamt else isnull(mall.serhamt,0) end
,ac.entry_ty,ac.tran_cd,ac.acserial
,sttran_cd=m1.tran_cd
,serrule=case when m.tdspaytype=2 then  m.serrule else m1.serrule end
into #bracdet --used in 3F
from bracdet ac
inner join brmain m on (m.entry_ty=ac.entry_ty and m.tran_cd=ac.tran_cd)
inner join ac_mast ac_mast on (ac.ac_id=ac_mast.ac_id) 
left join brmall mall on(ac.entry_ty=mall.entry_ty and ac.tran_cd=mall.tran_cd and ac.acserial=mall.acserial)
left join sbmain m1 on (mall.entry_all=m1.entry_ty and mall.main_tran=m1.tran_cd)
where (isnull(m.serty,'')<>'' or  isnull(m1.serty,'') <>'') and ac.amt_ty='cr'
and isnull((case when m.tdspaytype=2 then  m.gro_amt+m.tot_deduc+m.tot_tax else m1.gro_amt+m1.tot_deduc+m1.tot_tax end),0)<>0
and (case when m.tdspaytype=2 then  m.ac_id else m1.ac_id end)=ac_mast.ac_id --Only Party Name record
and (ac.date between @sdate and @edate)
union
select ac_mast.ac_name,m.inv_no,serty=(case when isnull(m.serty,'')='' then isnull(m1.serty,'') else isnull(m.serty,'') end)
,ac.date,ac.amt_ty
,ac_mast.typ,m.tdspaytype
,gro_amt=case when m.tdspaytype=2 then  m.gro_amt+m.tot_deduc+m.tot_tax else mall.new_all- (isnull(mall.serbamt,0)+isnull(mall.sercamt,0)+isnull(mall.serhamt,0)) end 
--,gro_amt=case when m.tdspaytype=2 then  m.gro_amt+m.tot_deduc+m.tot_tax else mall.new_all- (isnull(mall.serbamt,0)+isnull(mall.sercamt,0)+isnull(mall.serhamt,0)) end
--,gro_amt=case when m.tdspaytype=2 then  m.gro_amt+m.tot_deduc+m.tot_tax else m.gro_amt end
--,taxable_amt=case when m.tdspaytype=2 then  m.gro_amt+m.tot_deduc+m.tot_tax else m1.gro_amt+m1.tot_deduc+m1.tot_tax end 
--,taxable_amt=case when m.tdspaytype=2 then  m.gro_amt+m.tot_deduc+m.tot_tax else m.gro_amt end
,taxable_amt=case when m.tdspaytype=2 then  m.gro_amt+m.tot_deduc+m.tot_tax else mall.new_all- (isnull(mall.serbamt,0)+isnull(mall.sercamt,0)+isnull(mall.serhamt,0)) end 
,sabtamt    =case when m.tdspaytype=2 then  m.sabtamt else isnull(m1.sabtamt,0) end /*Rup 08Dec09*/
,serbper    =case when m.tdspaytype=2 then  m.serbper else m1.serbper end
,serbamt	=case when m.tdspaytype=2 then  m.serbamt else isnull(mall.serbamt,0) end
,sercper    =case when m.tdspaytype=2 then  m.sercper else m1.sercper end
,sercamt    =case when m.tdspaytype=2 then  m.sercamt else isnull(mall.sercamt,0) end
,serhper    =case when m.tdspaytype=2 then  m.serhper else m1.serhper end
,serhamt    =case when m.tdspaytype=2 then  m.serhamt else isnull(mall.serhamt,0) end
,ac.entry_ty,ac.tran_cd,ac.acserial
,sttran_cd=m1.tran_cd
,serrule=case when m.tdspaytype=2 then  m.serrule else m1.serrule end
from cracdet ac
inner join crmain m on (m.entry_ty=ac.entry_ty and m.tran_cd=ac.tran_cd)
inner join ac_mast ac_mast on (ac.ac_id=ac_mast.ac_id) 
left join crmall mall on(ac.entry_ty=mall.entry_ty and ac.tran_cd=mall.tran_cd and ac.acserial=mall.acserial)
left join sbmain m1 on (mall.entry_all=m1.entry_ty and mall.main_tran=m1.tran_cd)
where (isnull(m.serty,'')<>'' or  isnull(m1.serty,'') <>'') and ac.amt_ty='cr'
and isnull((case when m.tdspaytype=2 then  m.gro_amt+m.tot_deduc+m.tot_tax else m1.gro_amt+m1.tot_deduc+m1.tot_tax end),0)<>0
and (case when m.tdspaytype=2 then  m.ac_id else m1.ac_id end)=ac_mast.ac_id --Only Party Name record
and (ac.date between @sdate and @edate)

--select 'a',* from #bracdet
update #bracdet set tdspaytype =1 where isnull(tdspaytype,0)=0


select m.serty,ac.entry_ty,ac.tran_cd,ac.date,ac_mast.ac_name,amount,typ,beh=(case when l.ext_vou=1 then l.bcode_nm else l.entry_ty end),m.u_arrears
,m.U_CHALNO,m.U_CHALDT
into #bpacdet
from bpacdet ac
inner join bpmain m on (m.entry_ty=ac.entry_ty and m.tran_cd=ac.tran_cd)
inner join ac_mast on (ac_mast.ac_id=ac.ac_id)
inner join lcode l on (m.entry_ty=l.entry_ty)
where ac.amt_ty='DR' and ac_mast.typ in ('Service Tax Payable','Service Tax Payable-Ecess','Service Tax Payable-Hcess')
and (ac.date between @sdate and @edate) and isnull(m.serty,'')<>''
union 
select m.serty,ac.entry_ty,ac.tran_cd,ac.date,ac_mast.ac_name,amount,typ,beh=(case when l.ext_vou=1 then l.bcode_nm else l.entry_ty end),m.u_arrears
,U_CHALNO='',U_CHALDT=''
from jvacdet ac
inner join jvmain m on (m.entry_ty=ac.entry_ty and m.tran_cd=ac.tran_cd)
inner join ac_mast on (ac_mast.ac_id=ac.ac_id)
inner join lcode l on (m.entry_ty=l.entry_ty)
where ac.amt_ty='DR' and ac_mast.typ in ('Service Tax Payable','Service Tax Payable-Ecess','Service Tax Payable-Hcess')
and (ac.date between @sdate and @edate) and isnull(m.serty,'')<>''
union
select m.serty,ac.entry_ty,ac.tran_cd,ac.date,ac_mast.ac_name,amount,typ,beh=(case when l.ext_vou=1 then l.bcode_nm else l.entry_ty end),m.u_arrears
,m.U_CHALNO,m.U_CHALDT
from cpacdet ac
inner join cpmain m on (m.entry_ty=ac.entry_ty and m.tran_cd=ac.tran_cd)
inner join ac_mast on (ac_mast.ac_id=ac.ac_id)
inner join lcode l on (m.entry_ty=l.entry_ty)
where ac.amt_ty='dr' and ac_mast.typ in ('Service Tax Payable','Service Tax Payable-Ecess','Service Tax Payable-Hcess')
and (ac.date between @sdate and @edate) and isnull(m.serty,'')<>''



update #bracdet set tdspaytype =1 where isnull(tdspaytype,0)=0

update #bracdet set taxable_amt=isnull(taxable_amt,0),serbper=isnull(serbper,0),serbamt=isnull(serbamt,0),sercper=isnull(sercper,0),sercamt=isnull(sercamt,0),serhper=isnull(serhper,0),serhamt=isnull(serhamt,0)


declare @amt1 decimal(17,2),@amt2 decimal(17,2),@amt3 decimal(17,2),@amt4 decimal(17,2),@amt5 decimal(17,2),@amt6 decimal(17,2),@serty varchar(100)
declare @SERBPER decimal(6,2),@SERBAMT decimal(17,2),@SERCPER decimal(6,2),@SERCAMT decimal(17,2),@SERHPER decimal(6,2),@SERHAMT decimal(17,2)

select particulars=space(250),serty,srno1='AA',srno2='AA',srno3='AA',srno4='AA',srno5='AA',srno6='AA'
,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
,sdate1=@sdate,sdate2=@sdate,sdate3=@sdate,sdate4=@sdate,sdate5=@sdate,sdate6=@sdate
,amt1=net_amt,amt2=net_amt,amt3=net_amt,amt4=net_amt,amt5=net_amt,amt6=net_amt
,chalno1=space(10),chaldt1 =cast('' as smalldatetime),chalno2=space(10),chaldt2 =cast('' as smalldatetime),chalno3=space(10),chaldt3 =cast('' as smalldatetime),chalno4=space(10),chaldt4 =cast('' as smalldatetime),chalno5=space(10),chaldt5 =cast('' as smalldatetime),chalno6=space(10),chaldt6 =cast('' as smalldatetime)
into #st3 
from sbmain m
inner join stax_mas st on (m.tax_name=st.tax_name)
where 1=2

/*Repetable Part Start*/

declare cur_serty cursor for
select distinct serty from #bracdet
union /*Don't use union all*/
select distinct serty from #bpacdet
open cur_serty
fetch next from cur_serty into @serty
while (@@fetch_status=0)
begin

	-->1F1 A to I
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'3','F','1','a','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(gro_amt,0) else 0 end)/*taxable_amt*/
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(gro_amt,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(gro_amt,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(gro_amt,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(gro_amt,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(gro_amt,0) else 0 end)
	from #bracdet
	where tdspaytype=1
	and serty=@serty
	
	
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'3','F','1','A','1',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
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
	where tdspaytype=2
	and serty=@serty
	
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'3','F','1','A','2',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'3','F','1','B','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'3','F','1','C','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	
		
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
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
		
	

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'3','F','1','C','1',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(taxable_amt,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(taxable_amt,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(taxable_amt,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(taxable_amt,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(taxable_amt,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(taxable_amt,0) else 0 end)
	from #bracdet
	where serrule='EXEMPTED'
	and serty=@serty
	--SELECT 'A',* FROM #bracdet 	where serrule='EXEMPTED'

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'3','F','1','C','2',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'3','F','1','C','3',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	
--	select 
--	amt1=sum(case when (date between @sdate1 and @edate1) then isnull(sabtamt,0) else 0 end)
--	,amt2=sum(case when (date between @sdate2 and @edate2) then isnull(sabtamt,0) else 0 end)
--	,amt3=sum(case when (date between @sdate3 and @edate3) then isnull(sabtamt,0) else 0 end)
--	,amt4=sum(case when (date between @sdate4 and @edate4) then isnull(sabtamt,0) else 0 end)
--	,amt5=sum(case when (date between @sdate5 and @edate5) then isnull(sabtamt,0) else 0 end)
--	,amt6=sum(case when (date between @sdate6 and @edate6) then isnull(sabtamt,0) else 0 end)
--	 from #bracdet
--	where serty=@serty 

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(sabtamt,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(sabtamt,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(sabtamt,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(sabtamt,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(sabtamt,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(sabtamt,0) else 0 end)
	from #bracdet
	where serty=@serty
	
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'3','F','1','D','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when srno4 in('A','B') then amt1 else -amt1 end )
	,@amt2=sum(case when srno4 in('A','B') then amt2 else -amt2 end ) 
	,@amt3=sum(case when srno4 in('A','B') then amt3 else -amt3 end )
	,@amt4=sum(case when srno4 in('A','B') then amt4 else -amt4 end )
	,@amt5=sum(case when srno4 in('A','B') then amt5 else -amt5 end )
	,@amt6=sum(case when  serty=@serty and srno1='3' and srno2='F' and srno3='1' Then ( case when srno4 in('A','B') then amt6 else -amt6 end ) else 0 end)
	from #st3 where serty=@serty and srno1='3' and srno2='F' and srno3='1' AND SRNO4 IN ('A','B','C','D')

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'3','F','1','E','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'3','F','1','F','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	
	--select distinct sttran_cd,date,taxable_amt,serbper,serbamt,sercper,sercamt,serhper,serhamt 
	--into #tmpbracdet
	--from #bracdet
	--where isnull(serbper,0)<>0 and serty=@serty	 
	--declare  @sttran_cd int,@date smalldatetime,@taxable_amt smalldatetime
	
	declare cur_st3_1 cursor for
	select distinct serbper
	from #bracdet 
	where isnull(serbper,0)<>0 --and serty=@serty	 -->3F1F
	order by serbper

	open cur_st3_1
	fetch next from cur_st3_1 into @serbper--@sttran_cd,@date,@taxable_amt,@serbper,@serbamt,@sercper,@sercamt,@serhper,@serhamt
	set @c=0
	set @particulars1=' '
	while(@@fetch_status=0)
	begin
		set @c=@c+1
		
		select @particulars=(case when @c=1 then '(i)' else (case when @c=2 then '(ii)' else (case when @c=3 then '(iii)' else (case when @c=4 then '(iv)' else (case when @c=5 then '(v)' else (case when @c=6 then '(vi)' else (case when @c=7 then '(vii)' else (case when @c=8 then '(viii)' else (case when @c=9 then '(ix)' else (case when @c=10 then '(x)' else '' end) end) end) end) end) end) end) end) end) end)
		set @particulars1=rtrim(@particulars1)+'('+ltrim(cast(@serbper as varchar))+'%'+' of F'+@particulars
		set @particulars=rtrim(@particulars)+' Value on which Service Tax Payable @'+ltrim(cast(@serbper as varchar))+'%'
			
		--select *,taxable_amt from #bracdet where serbper=@serbper and serty=@serty		
		--select 'a',* from #bracdet
		insert into #st3
		(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
		,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
		,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
		,amt1,amt2,amt3,amt4,amt5,amt6
		)
		select @particulars,@serty,'3','F','1','F',' ',' '
		,serbper,0,0,0,0,0
		,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
		,amt1=sum(case when (date between @sdate1 and @edate1) then isnull(taxable_amt-sabtamt,0) else 0 end) /*Rup 08Dec09*/
		,amt2=sum(case when (date between @sdate2 and @edate2) then isnull(taxable_amt-sabtamt,0) else 0 end)
		,amt3=sum(case when (date between @sdate3 and @edate3) then isnull(taxable_amt-sabtamt,0) else 0 end)
		,amt4=sum(case when (date between @sdate4 and @edate4) then isnull(taxable_amt-sabtamt,0) else 0 end)
		,amt5=sum(case when (date between @sdate5 and @edate5) then isnull(taxable_amt-sabtamt,0) else 0 end)
		,amt6=sum(case when (date between @sdate6 and @edate6) then isnull(taxable_amt-sabtamt,0) else 0 end)
		from #bracdet
		where serbper=@serbper
		and serty=@serty
		group by serbper
		--select * from #bracdet
		



		fetch next from cur_st3_1 into @serbper--@sttran_cd,@date,@taxable_amt,@serbper,@serbamt,@sercper,@sercamt,@serhper,@serhamt
	end
	close cur_st3_1
	deallocate cur_st3_1
	
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	set @c=@c+1
	select @particulars=(case when @c=1 then '(i)' else (case when @c=2 then '(ii)' else (case when @c=3 then '(iii)' else (case when @c=4 then '(iv)' else (case when @c=5 then '(v)' else (case when @c=6 then '(vi)' else (case when @c=7 then '(vii)' else (case when @c=8 then '(vii)' else (case when @c=9 then '(ix)' else (case when @c=10 then '(x)' else '' end) end) end) end) end) end) end) end) end) end)
	set @particulars=rtrim(@particulars)+' other rate, if any,(please specify)'

	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	(@particulars,@serty,'3','F','1','F','',''
	,99,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)

	select @particulars=(case when @c=1 then '(i)' else (case when @c=2 then '(ii)' else (case when @c=3 then '(iii)' else (case when @c=4 then '(iv)' else (case when @c=5 then '(v)' else (case when @c=6 then '(vi)' else (case when @c=7 then '(vii)' else (case when @c=8 then '(vii)' else (case when @c=9 then '(ix)' else (case when @c=10 then '(x)' else '' end) end) end) end) end) end) end) end) end) end)
	set @particulars1='Service Tax Payable='+rtrim(@particulars1)+'+ x % of '+@particulars+')'
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	select @particulars1,@serty,'3','F','1','G','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,amt1=sum(case when (date between @sdate1 and @edate1) then isnull(serbamt,0) else 0 end)
	,amt2=sum(case when (date between @sdate2 and @edate2) then isnull(serbamt,0) else 0 end)
	,amt3=sum(case when (date between @sdate3 and @edate3) then isnull(serbamt,0) else 0 end)
	,amt4=sum(case when (date between @sdate4 and @edate4) then isnull(serbamt,0) else 0 end)
	,amt5=sum(case when (date between @sdate5 and @edate5) then isnull(serbamt,0) else 0 end)
	,amt6=sum(case when (date between @sdate6 and @edate6) then isnull(serbamt,0) else 0 end)
	from #bracdet
	where serty=@serty
	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	select '',@serty,'3','F','1','H','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,amt1=sum(case when (date between @sdate1 and @edate1) then isnull(sercamt,0) else 0 end)
	,amt2=sum(case when (date between @sdate2 and @edate2) then isnull(sercamt,0) else 0 end)
	,amt3=sum(case when (date between @sdate3 and @edate3) then isnull(sercamt,0) else 0 end)
	,amt4=sum(case when (date between @sdate4 and @edate4) then isnull(sercamt,0) else 0 end)
	,amt5=sum(case when (date between @sdate5 and @edate5) then isnull(sercamt,0) else 0 end)
	,amt6=sum(case when (date between @sdate6 and @edate6) then isnull(sercamt,0) else 0 end)
	from #bracdet
	where serty=@serty

	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	select '',@serty,'3','F','1','I','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,amt1=sum(case when (date between @sdate1 and @edate1) then isnull(serhamt,0) else 0 end)
	,amt2=sum(case when (date between @sdate2 and @edate2) then isnull(serhamt,0) else 0 end)
	,amt3=sum(case when (date between @sdate3 and @edate3) then isnull(serhamt,0) else 0 end)
	,amt4=sum(case when (date between @sdate4 and @edate4) then isnull(serhamt,0) else 0 end)
	,amt5=sum(case when (date between @sdate5 and @edate5) then isnull(serhamt,0) else 0 end)
	,amt6=sum(case when (date between @sdate6 and @edate6) then isnull(serhamt,0) else 0 end)
	from #bracdet
	where serty=@serty
	
	--<--1F1 A to I	
	-->1F2 J to P
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	select '',@serty,'3','F','2','J','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,amt1=sum(case when (date between @sdate1 and @edate1) then isnull(gro_amt+tot_deduc+tot_tax,0) else 0 end)
	,amt2=sum(case when (date between @sdate2 and @edate2) then isnull(gro_amt+tot_deduc+tot_tax,0) else 0 end)
	,amt3=sum(case when (date between @sdate3 and @edate3) then isnull(gro_amt+tot_deduc+tot_tax,0) else 0 end)
	,amt4=sum(case when (date between @sdate4 and @edate4) then isnull(gro_amt+tot_deduc+tot_tax,0) else 0 end)
	,amt5=sum(case when (date between @sdate5 and @edate5) then isnull(gro_amt+tot_deduc+tot_tax,0) else 0 end)
	,amt6=sum(case when (date between @sdate6 and @edate6) then isnull(gro_amt+tot_deduc+tot_tax,0) else 0 end)
	from sbmain 
	where entry_ty in ('SB') 
	and (date between @sdate and @edate)
	and serty=@serty

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'3','F','2','k','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (date between @sdate1 and @edate1) then isnull(gro_amt+tot_deduc+tot_tax,0) else 0 end)
	,@amt2=sum(case when (date between @sdate2 and @edate2) then isnull(gro_amt+tot_deduc+tot_tax,0) else 0 end)
	,@amt3=sum(case when (date between @sdate3 and @edate3) then isnull(gro_amt+tot_deduc+tot_tax,0) else 0 end)
	,@amt4=sum(case when (date between @sdate4 and @edate4) then isnull(gro_amt+tot_deduc+tot_tax,0) else 0 end)
	,@amt5=sum(case when (date between @sdate5 and @edate5) then isnull(gro_amt+tot_deduc+tot_tax,0) else 0 end)
	,@amt6=sum(case when (date between @sdate6 and @edate6) then isnull(gro_amt+tot_deduc+tot_tax,0) else 0 end)
	from sbmain 
	where entry_ty in ('SB') and (date between @sdate and @edate) and serrule='EXPORT' and serty=@serty
	

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'3','F','2','L','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)	

	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	select '',@serty,'3','F','2','M','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,amt1=sum(case when (date between @sdate1 and @edate1) then isnull(gro_amt+tot_deduc+tot_tax,0) else 0 end)
	,amt2=sum(case when (date between @sdate2 and @edate2) then isnull(gro_amt+tot_deduc+tot_tax,0) else 0 end)
	,amt3=sum(case when (date between @sdate3 and @edate3) then isnull(gro_amt+tot_deduc+tot_tax,0) else 0 end)
	,amt4=sum(case when (date between @sdate4 and @edate4) then isnull(gro_amt+tot_deduc+tot_tax,0) else 0 end)
	,amt5=sum(case when (date between @sdate5 and @edate5) then isnull(gro_amt+tot_deduc+tot_tax,0) else 0 end)
	,amt6=sum(case when (date between @sdate6 and @edate6) then isnull(gro_amt+tot_deduc+tot_tax,0) else 0 end)
	from sbmain 
	where entry_ty in ('SB') and (date between @sdate and @edate) and serrule='EXEMPTED' and serty=@serty
		
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'3','F','2','N','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)

	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	select '',@serty,'3','F','2','O','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,amt1=sum(case when (date between @sdate1 and @edate1) then isnull(sabtamt,0) else 0 end)
	,amt2=sum(case when (date between @sdate2 and @edate2) then isnull(sabtamt,0) else 0 end)
	,amt3=sum(case when (date between @sdate3 and @edate3) then isnull(sabtamt,0) else 0 end)
	,amt4=sum(case when (date between @sdate4 and @edate4) then isnull(sabtamt,0) else 0 end)
	,amt5=sum(case when (date between @sdate5 and @edate5) then isnull(sabtamt,0) else 0 end)
	,amt6=sum(case when (date between @sdate6 and @edate6) then isnull(sabtamt,0) else 0 end)
	from sbmain 
	where entry_ty in ('SB') and (date between @sdate and @edate)  and serty=@serty
	
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when srno4 in('J','K') then amt1 else -amt1 end )
	,@amt2=sum(case when srno4 in('J','K') then amt2 else -amt2 end ) 
	,@amt3=sum(case when srno4 in('J','K') then amt3 else -amt3 end )
	,@amt4=sum(case when srno4 in('J','K') then amt4 else -amt4 end )
	,@amt5=sum(case when srno4 in('J','K') then amt5 else -amt5 end )
	,@amt6=sum(case when srno4 in('J','K') then amt6 else -amt6 end )
	/*,@amt6=sum(case when  serty=@serty and srno1='3' and srno2='F' and srno3='1' Then ( case when srno4 in('A','B') then amt6 else -amt6 end ) else 0 end)*/
	from #st3 where serty=@serty and srno1='3' and srno2='F' and srno3='2'  AND SRNO4 IN ('J','K','L','M','N','O')

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'3','F','2','P','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	
	-->4A1
	
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'4','A','1','a','0',' '
	,0,0,0,0,0,0
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
	where serty=@serty and typ='Service Tax Payable' and beh in ('BP','CP') and isnull(u_arrears,' ')=' '
		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'4','A','1','A','1',' '
	,0,0,0,0,0,0
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
	where serty=@serty and typ='Service Tax Payable' and beh in ('JV') and isnull(u_arrears,' ')=' '

		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'4','A','1','a','2',' '
	,0,0,0,0,0,0
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
	where serty=@serty and typ='Service Tax Payable' and isnull(u_arrears,' ')='Rule 6 (3) of ST Rules'
		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'4','A','1','a','3',' '
	,0,0,0,0,0,0
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
	where serty=@serty and typ='Service Tax Payable' and isnull(u_arrears,' ')='Rule 6 (4A) of ST Rules'
		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'4','A','1','a','4',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)		
	
	--
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'4','A','1','b','0',' '
	,0,0,0,0,0,0
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
	where serty=@serty and typ='Service Tax Payable-Ecess' and beh in ('BP','CP') and isnull(u_arrears,' ')=' '
		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'4','A','1','b','1',' '
	,0,0,0,0,0,0
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
	where serty=@serty and typ='Service Tax Payable-Ecess' and beh in ('JV') and isnull(u_arrears,' ')=' '

		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'4','A','1','b','2',' '
	,0,0,0,0,0,0
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
	where serty=@serty and typ='Service Tax Payable-Ecess' and isnull(u_arrears,' ')='Rule 6 (3) of ST Rules'
		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'4','A','1','b','3',' '
	,0,0,0,0,0,0
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
	where serty=@serty and typ='Service Tax Payable-Ecess' and isnull(u_arrears,' ')='Rule 6 (4A) of ST Rules'
		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'4','A','1','b','4',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)		
	--
	--
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'4','A','1','c','0',' '
	,0,0,0,0,0,0
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
	where serty=@serty and typ='Service Tax Payable-Hcess' and beh in ('BP','CP') and isnull(u_arrears,' ')=' '
		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'4','A','1','c','1',' '
	,0,0,0,0,0,0
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
	where serty=@serty and typ='Service Tax Payable-Hcess' and beh in ('JV') and isnull(u_arrears,' ')=' '

		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'4','A','1','c','2',' '
	,0,0,0,0,0,0
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
	where serty=@serty and typ='Service Tax Payable-Hcess' and isnull(u_arrears,' ')='Rule 6 (3) of ST Rules'
		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'4','A','1','c','3',' '
	,0,0,0,0,0,0
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
	where serty=@serty and typ='Service Tax Payable-Hcess' and isnull(u_arrears,' ')='Rule 6 (4A) of ST Rules'
		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'4','A','1','c','4',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)		
	--
	--4A1D
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'4','A','1','d','0',''
	,0,0,0,0,0,0
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
	where serty=@serty and isnull(u_arrears,' ')='Arrears of revenue paid in cash'

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'4','A','1','d','1',' '
	,0,0,0,0,0,0
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
	where serty=@serty and isnull(u_arrears,' ')='Arrears of revenue paid by credit'
		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'4','A','1','d','2',' '
	,0,0,0,0,0,0
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
	where serty=@serty and isnull(u_arrears,' ')='Arrears of education cess paid in cash (Differantial of Cess)'
		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'4','A','1','d','3',' '
	,0,0,0,0,0,0
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
	where serty=@serty and isnull(u_arrears,' ')='Arrears of education cess paid by credit'
		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'4','A','1','d','4',' '
	,0,0,0,0,0,0
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
	where serty=@serty and isnull(u_arrears,' ')='Arrears of Sec & higher edu cess paid by cash'
		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'4','A','1','d','5',' '
	,0,0,0,0,0,0
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
	where serty=@serty and isnull(u_arrears,' ')='Arrears of Sec & higher edu cess paid by credit'
		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'4','A','1','d','6',' '
	,0,0,0,0,0,0
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
	where serty=@serty and isnull(u_arrears,' ')='Interest paid'
		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'4','A','1','d','7',' '
	,0,0,0,0,0,0
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
	where serty=@serty and isnull(u_arrears,' ')='Penalty paid'
		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'4','A','1','d','8',' '
	,0,0,0,0,0,0
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
	where serty=@serty and isnull(u_arrears,' ')='Section 73A amount paid'
		
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'4','A','1','d','9',' '
	,0,0,0,0,0,0
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
	where serty=@serty 
	and isnull(u_arrears,'') not in ('','Arrears of revenue paid in cash','Arrears of revenue paid by credit','Arrears of education cess paid in cash (Differantial of Cess)','Arrears of education cess paid by credit','Arrears of Sec & higher edu cess paid by cash','Arrears of Sec & higher edu cess paid by credit','Interest paid','Penalty paid','Section 73A amount paid') 
	
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'4','A','1','d','10',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)			
	
	--4A1D
	--4A2 ChalNo
	set @c=0
	
	declare cur_st3_2 cursor for
	select distinct date,u_chalno,u_chaldt
	from #bpacdet 
	where ((isnull(u_chalno,'')<>'') or (isnull(u_chaldt,'')<>'') ) and serty=@serty
	order by date

	open cur_st3_2
	fetch next from cur_st3_2 into @date,@u_chalno,@u_chaldt
	set @c=0
	set @particulars1=' '
	while(@@fetch_status=0)
	begin
		set @c=@c+1
		select @particulars=(case when @c=1 then '(i)' else (case when @c=2 then '(ii)' else (case when @c=3 then '(iii)' else (case when @c=4 then '(iv)' else (case when @c=5 then '(v)' else (case when @c=6 then '(vi)' else (case when @c=7 then '(vii)' else (case when @c=8 then '(viii)' else (case when @c=9 then '(ix)' else (case when @c=10 then '(x)' else '' end) end) end) end) end) end) end) end) end) end)
		insert into #st3
		(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
		,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
		,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
		,amt1,amt2,amt3,amt4,amt5,amt6
		,chalno1,chaldt1,chalno2,chaldt2,chalno3,chaldt3,chalno4,chaldt4,chalno5,chaldt5,chalno6,chaldt6
		)
		select @particulars,@serty,'4','A','2','A','1',cast(@c as varchar(2))
		,0,0,0,0,0,0
		,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
		,0,0,0,0,0,0
		,chalno1=(case when (@date between @sdate1 and @edate1) then isnull(@u_chalno,'') else '' end)
		,chaldt1=' '--(case when (@date between @sdate1 and @edate1) then isnull(@u_chaldt,'') else '' end)
		,chalno2=(case when (@date between @sdate2 and @edate2) then isnull(@u_chalno,'') else '' end)
		,chaldt2=' '--(case when (@date between @sdate2 and @edate2) then isnull(@u_chaldt,'') else '' end)
		,chalno3=(case when (@date between @sdate3 and @edate3) then isnull(@u_chalno,'') else '' end)
		,chaldt3=' '--(case when (@date between @sdate3 and @edate3) then isnull(@u_chaldt,'') else '' end)
		,chalno4=(case when (@date between @sdate4 and @edate4) then isnull(@u_chalno,'') else '' end)
		,chaldt4=' '--(case when (@date between @sdate4 and @edate4) then isnull(@u_chaldt,'') else '' end)
		,chalno5=(case when (@date between @sdate5 and @edate5) then isnull(@u_chalno,'') else '' end)
		,chaldt5=' '--(case when (@date between @sdate5 and @edate5) then isnull(@u_chaldt,'') else '' end)
		,chalno6=(case when (@date between @sdate6 and @edate6) then isnull(@u_chalno,'') else '' end)
		,chaldt6=' '--(case when (@date between @sdate6 and @edate6) then isnull(@u_chaldt,'') else '' end)

		insert into #st3
		(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
		,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
		,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
		,amt1,amt2,amt3,amt4,amt5,amt6
		,chalno1,chaldt1,chalno2,chaldt2,chalno3,chaldt3,chalno4,chaldt4,chalno5,chaldt5,chalno6,chaldt6
		)
		select @particulars,@serty,'4','A','2','A','2',cast(@c as varchar(2))
		,0,0,0,0,0,0
		,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
		,0,0,0,0,0,0
		,chalno1=' '--(case when (@date between @sdate1 and @edate1) then isnull(@u_chalno,'') else '' end)
		,chaldt1=(case when (@date between @sdate1 and @edate1) then isnull(@u_chaldt,'') else '' end)
		,chalno2=' '--(case when (@date between @sdate2 and @edate2) then isnull(@u_chalno,'') else '' end)
		,chaldt2=(case when (@date between @sdate2 and @edate2) then isnull(@u_chaldt,'') else '' end)
		,chalno3=' '--(case when (@date between @sdate3 and @edate3) then isnull(@u_chalno,'') else '' end)
		,chaldt3=(case when (@date between @sdate3 and @edate3) then isnull(@u_chaldt,'') else '' end)
		,chalno4=' '--(case when (@date between @sdate4 and @edate4) then isnull(@u_chalno,'') else '' end)
		,chaldt4=(case when (@date between @sdate4 and @edate4) then isnull(@u_chaldt,'') else '' end)
		,chalno5=' '--(case when (@date between @sdate5 and @edate5) then isnull(@u_chalno,'') else '' end)
		,chaldt5=(case when (@date between @sdate5 and @edate5) then isnull(@u_chaldt,'') else '' end)
		,chalno6=' '--(case when (@date between @sdate6 and @edate6) then isnull(@u_chalno,'') else '' end)
		,chaldt6=(case when (@date between @sdate6 and @edate6) then isnull(@u_chaldt,'') else '' end)

		set @particulars=cast(@c as varchar)
		select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
		insert into #st3
		(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
		,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
		,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
		,amt1,amt2,amt3,amt4,amt5,amt6
		,chalno1,chaldt1
		)
		values
		('',@serty,'4','B','0',@particulars,'',''
		,0,0,0,0,0,0
		,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
		,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
		,@u_chalno,@u_chaldt
		)
		
		fetch next from cur_st3_2 into @date,@u_chalno,@u_chaldt
	end
	close cur_st3_2
	deallocate cur_st3_2

	
	if not exists(select * from #st3 where srno1='4' and srno2='A' and srno3='2' and srno4='A' and srno5='1' and serty=@serty)
	begin
		set @c=1
		select @particulars=(case when @c=1 then '(i)' else (case when @c=2 then '(ii)' else (case when @c=3 then '(iii)' else (case when @c=4 then '(iv)' else (case when @c=5 then '(v)' else (case when @c=6 then '(vi)' else (case when @c=7 then '(vii)' else (case when @c=8 then '(viii)' else (case when @c=9 then '(ix)' else (case when @c=10 then '(x)' else '' end) end) end) end) end) end) end) end) end) end)
		insert into #st3
		(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
		,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
		,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
		,amt1,amt2,amt3,amt4,amt5,amt6
		,chalno1,chaldt1,chalno2,chaldt2,chalno3,chaldt3,chalno4,chaldt4,chalno5,chaldt5,chalno6,chaldt6
		)
		select @particulars,@serty,'4','A','2','A','1',cast(@c as varchar(2))
		,0,0,0,0,0,0
		,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
		,0,0,0,0,0,0
		,chalno1=''
		,chaldt1=''
		,chalno2=''
		,chaldt2=''
		,chalno3=''
		,chaldt3=''
		,chalno4=''
		,chaldt4=''
		,chalno5=''
		,chaldt5=''
		,chalno6=''
		,chaldt6=''
	end
	if not exists(select * from #st3 where srno1='4' and srno2='A' and srno3='2' and srno4='A' and srno5='2' and serty=@serty)
	begin
		set @c=1
		select @particulars=(case when @c=1 then '(i)' else (case when @c=2 then '(ii)' else (case when @c=3 then '(iii)' else (case when @c=4 then '(iv)' else (case when @c=5 then '(v)' else (case when @c=6 then '(vi)' else (case when @c=7 then '(vii)' else (case when @c=8 then '(viii)' else (case when @c=9 then '(ix)' else (case when @c=10 then '(x)' else '' end) end) end) end) end) end) end) end) end) end)
		insert into #st3
		(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
		,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
		,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
		,amt1,amt2,amt3,amt4,amt5,amt6
		,chalno1,chaldt1,chalno2,chaldt2,chalno3,chaldt3,chalno4,chaldt4,chalno5,chaldt5,chalno6,chaldt6
		)
		select @particulars,@serty,'4','A','2','A','2',cast(@c as varchar(2))
		,0,0,0,0,0,0
		,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
		,0,0,0,0,0,0
		,chalno1=''
		,chaldt1=''
		,chalno2=''
		,chaldt2=''
		,chalno3=''
		,chaldt3=''
		,chalno4=''
		,chaldt4=''
		,chalno5=''
		,chaldt5=''
		,chalno6=''
		,chaldt6=''
		
		
	end
	--<--4A2 ChalNo
	if not exists(select * from #st3 where srno1='4' and srno2='B' and serty=@serty)
	begin
		set @c=1
		set @particulars=cast(@c as varchar)
		select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
		insert into #st3
		(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
		,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
		,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
		,amt1,amt2,amt3,amt4,amt5,amt6
		,chalno1,chaldt1
		)
		values
		('',@serty,'4','B','0','','',''
		,0,0,0,0,0,0
		,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
		,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
		,'',''
		)
		
	end
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'4','C','0','','',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	
	fetch next from cur_serty into @serty
end--while (@@fetch_status=0)
close cur_serty
deallocate cur_serty /*Repetable Part Over*/


--5a-->
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'5','A','0','','',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	--<--5a
	--5b-->
	--5b1
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (beh='OB' or date<@sdate1) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
	,@amt2=sum(case when (beh='OB' or date<@sdate2) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
	,@amt3=sum(case when (beh='OB' or date<@sdate3) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
	,@amt4=sum(case when (beh='OB' or date<@sdate4) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
	,@amt5=sum(case when (beh='OB' or date<@sdate5) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
	,@amt6=sum(case when (beh='OB' or date<@sdate6) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
	from #st3_5b
	where  (date<=@edate) and (typ='Service Tax Available') and @isdprouct=0--(serty=@serty) and
 
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'5','B','1','A','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)			
	
	--5b1b
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'5','B','1','B','0',' '
	,0,0,0,0,0,0
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
	where  (typ='Service Tax Available')  and (sertype in ('Credit taken On input')) and @isdprouct=0--(serty=@serty) and

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'5','B','1','B','1',' '
	,0,0,0,0,0,0
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
	where  (typ='Service Tax Available') and (sertype='Credit taken On capital goods') and @isdprouct=0--(serty=@serty) and

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'5','B','1','B','2',' '
	,0,0,0,0,0,0
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
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'5','B','1','B','3',' '
	,0,0,0,0,0,0
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
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'5','B','1','B','4',' '
	,0,0,0,0,0,0
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
	where  (typ='Service Tax Available') and (sertype='Credit taken From inter unit transfer by a LTU*') and @isdprouct=0 --(serty=@serty) and

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'5','B','1','B','5',' '
	,0,0,0,0,0,0
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
	from #st3 where  srno1='5' and srno2='B' and srno3='1' and srno4='B' and srno5<>'6'
	--,@amt4=sum(case when srno4 in('J','K') then amt4 else -amt4 end )

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'5','B','1','B','6',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	--5B1B
	--5B1C
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'5','B','1','C','0',' '
	,0,0,0,0,0,0
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
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'5','B','1','C','1',' '
	,0,0,0,0,0,0
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
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'5','B','1','C','2',' '
	,0,0,0,0,0,0
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
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'5','B','1','C','3',' '
	,0,0,0,0,0,0
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
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'5','B','1','C','4',' '
	,0,0,0,0,0,0
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
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'5','B','1','C','5',' '
	,0,0,0,0,0,0
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
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'5','B','1','C','6',' '
	,0,0,0,0,0,0
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
	from #st3 where srno1='5' and srno2='B' and srno3='1' and srno4='C' and srno5<>'7'
	--,@amt4=sum(case when srno4 in('J','K') then amt4 else -amt4 end )

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'5','B','1','C','7',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	--5B1C

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when srno4 in('A','B') then isnull(amt1,0) else -isnull(amt1,0) end)
	,@amt2=sum(case when srno4 in('A','B') then isnull(amt2,0) else -isnull(amt2,0) end)
	,@amt3=sum(case when srno4 in('A','B') then isnull(amt3,0) else -isnull(amt3,0) end)
	,@amt4=sum(case when srno4 in('A','B') then isnull(amt4,0) else -isnull(amt4,0) end)
	,@amt5=sum(case when srno4 in('A','B') then isnull(amt5,0) else -isnull(amt5,0) end)
	,@amt6=sum(case when srno4 in('A','B') then isnull(amt6,0) else -isnull(amt6,0) end)
	from #st3 where srno1='5' and srno2='B' and srno3='1' and (srno4 in('A') or (srno4 in('B') and srno5='6') or (srno4 in('C') and srno5='7') ) --and srno4 in('A','B','C') and srno5<>'7'
	

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'5','B','1','D','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	--5b1
	--5b2
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when (beh='OB' or date<@sdate1) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
	,@amt2=sum(case when (beh='OB' or date<@sdate2) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
	,@amt3=sum(case when (beh='OB' or date<@sdate3) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
	,@amt4=sum(case when (beh='OB' or date<@sdate4) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
	,@amt5=sum(case when (beh='OB' or date<@sdate5) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
	,@amt6=sum(case when (beh='OB' or date<@sdate6) then (case when amt_ty='DR' then amount else -amount end) else 0 end)
	from #st3_5b
	where (date<=@edate1) and @isdprouct=0 and ( typ in('Service Tax Available-Ecess','Service Tax Available-Hcess') )
 
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'5','B','2','A','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)			
	
	--5b2b
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'5','B','2','B','0',' '
	,0,0,0,0,0,0
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
	where  (typ in('Service Tax Available-Ecess','Service Tax Available-Hcess'))  and @isdprouct=0 and (sertype='Credit taken On input')

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'5','B','2','B','1',' '
	,0,0,0,0,0,0
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
	where  (typ in('Service Tax Available-Ecess','Service Tax Available-Hcess')) and @isdprouct=0 and (sertype='Credit taken On capital goods') 

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'5','B','2','B','2',' '
	,0,0,0,0,0,0
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
	where  (typ in('Service Tax Available-Ecess','Service Tax Available-Hcess')) and @isdprouct=0
	and sertype=''

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'5','B','2','B','3',' '
	,0,0,0,0,0,0
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
	where  (typ in('Service Tax Available-Ecess','Service Tax Available-Hcess')) and @isdprouct=0
	and (sertype ='Credit taken As received from input service distributor')

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'5','B','2','B','4',' '
	,0,0,0,0,0,0
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
	where  (typ in('Service Tax Available-Ecess','Service Tax Available-Hcess')) and @isdprouct=0 and (sertype='Credit taken From inter unit transfer by a LTU*') 

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'5','B','2','B','5',' '
	,0,0,0,0,0,0
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
	from #st3 where  srno1='5' and srno2='B' and srno3='2' and srno4='B' and srno5<>'6'
	--,@amt4=sum(case when srno4 in('J','K') then amt4 else -amt4 end )

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'5','B','2','B','6',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	--5B2B
	--5B2C
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'5','B','2','C','0',' '
	,0,0,0,0,0,0
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
	where (typ in('Service Tax Available-Ecess','Service Tax Available-Hcess')) and @isdprouct=0
	and (sertype='') 
	--and (sertype not in ('Credit utilized For payment of education cess and secondary and higher education cess on goods','Credit utilized Towards payment of education cess and secondary and higher education cess on clearance of input goods and capital goods removed as such','Credit utilized Towards inter unit transfer of LTU*')) 

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'5','B','2','C','1',' '
	,0,0,0,0,0,0
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
	where (typ in('Service Tax Available-Ecess','Service Tax Available-Hcess'))  and @isdprouct=0
	and (sertype='Credit utilized For payment of education cess and secondary and higher education cess on goods') 

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0--Not Known
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'5','B','2','C','2',' '
	,0,0,0,0,0,0
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
	where (typ in('Service Tax Available-Ecess','Service Tax Available-Hcess')) and @isdprouct=0
	and (sertype ='Credit utilized Towards payment of education cess and secondary and higher education cess on clearance of input goods and capital goods removed as such') 

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'5','B','2','C','3',' '
	,0,0,0,0,0,0
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
	where  (typ in('Service Tax Available-Ecess','Service Tax Available-Hcess'))  and @isdprouct=0
	and (sertype='Credit utilized Towards inter unit transfer of LTU*') 
	

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'5','B','2','C','4',' '
	,0,0,0,0,0,0
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
	from #st3 where  srno1='5' and srno2='B' and srno3='2' and srno4='C' and srno5<>'5'

	--,@amt4=sum(case when srno4 in('J','K') then amt4 else -amt4 end )

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'5','B','2','C','5',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	--5B2C

	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when srno4 in('A','B') then isnull(amt1,0) else -isnull(amt1,0) end)
	,@amt2=sum(case when srno4 in('A','B') then isnull(amt2,0) else -isnull(amt2,0) end)
	,@amt3=sum(case when srno4 in('A','B') then isnull(amt3,0) else -isnull(amt3,0) end)
	,@amt4=sum(case when srno4 in('A','B') then isnull(amt4,0) else -isnull(amt4,0) end)
	,@amt5=sum(case when srno4 in('A','B') then isnull(amt5,0) else -isnull(amt5,0) end)
	,@amt6=sum(case when srno4 in('A','B') then isnull(amt6,0) else -isnull(amt6,0) end)
	from #st3 where srno1='5' and srno2='B' and srno3='2' and (srno4 in('A') or (srno4 in('B') and srno5='6') or (srno4 in('C') and srno5='5') )
	--srno1='5' and srno2='B' and srno3='2' and srno4 in('A','B','C') and srno5<>'6'
	--,@amt4=sum(case when srno4 in('J','K') then amt4 else -amt4 end )
	

	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'5','B','2','D','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	--5b2
	--<--5b
	-->--6
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
		where (date<=@edate) and (typ='Service Tax Available') --(serty=@serty) and
	end
	else
	begin
		select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	end
  			
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'6','0','1','A','0',' '
	,0,0,0,0,0,0
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
		where  (typ='Service Tax Available')

		select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)
	end
	else
	begin
		select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	end	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'6','0','1','B','0',' '
	,0,0,0,0,0,0
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
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'6','0','1','C','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'6','0','1','D','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when srno4 in('A','B') then isnull(amt1,0) else -isnull(amt1,0) end)
	,@amt2=sum(case when srno4 in('A','B') then isnull(amt2,0) else -isnull(amt2,0) end)
	,@amt3=sum(case when srno4 in('A','B') then isnull(amt3,0) else -isnull(amt3,0) end)
	,@amt4=sum(case when srno4 in('A','B') then isnull(amt4,0) else -isnull(amt4,0) end)
	,@amt5=sum(case when srno4 in('A','B') then isnull(amt5,0) else -isnull(amt5,0) end)
	,@amt6=sum(case when srno4 in('A','B') then isnull(amt6,0) else -isnull(amt6,0) end)
	from #st3 where srno1='6' and srno2='0' and srno3='1' and (srno4 in('A','B','C','D') )
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'6','0','1','E','0',' '
	,0,0,0,0,0,0
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
		where (date<=@edate) and (typ in('Service Tax Available-Ecess','Service Tax Available-Hcess'))
	end
	else
	begin
		select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	end
  			
	select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'6','0','2','A','0',' '
	,0,0,0,0,0,0
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
		where  (typ in('Service Tax Available-Ecess','Service Tax Available-Hcess'))

		select @amt1=isnull(@amt1,0),@amt2=isnull(@amt2,0),@amt3=isnull(@amt3,0),@amt4=isnull(@amt4,0),@amt5=isnull(@amt5,0),@amt6=isnull(@amt6,0)
	end
	else
	begin
		select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	end	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'6','0','2','B','0',' '
	,0,0,0,0,0,0
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
		where  (typ in('Service Tax Available-Ecess','Service Tax Available-Hcess'))
	end
	else
	begin
		select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	end	
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'6','0','2','C','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'6','0','1','D','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)
	
	select @amt1=0,@amt2=0,@amt3=0,@amt4=0,@amt5=0,@amt6=0
	select 
	 @amt1=sum(case when srno4 in('A','B') then isnull(amt1,0) else -isnull(amt1,0) end)
	,@amt2=sum(case when srno4 in('A','B') then isnull(amt2,0) else -isnull(amt2,0) end)
	,@amt3=sum(case when srno4 in('A','B') then isnull(amt3,0) else -isnull(amt3,0) end)
	,@amt4=sum(case when srno4 in('A','B') then isnull(amt4,0) else -isnull(amt4,0) end)
	,@amt5=sum(case when srno4 in('A','B') then isnull(amt5,0) else -isnull(amt5,0) end)
	,@amt6=sum(case when srno4 in('A','B') then isnull(amt6,0) else -isnull(amt6,0) end)
	from #st3 where srno1='6' and srno2='0' and srno3='2' and (srno4 in('A','B','C','D') )
	insert into #st3
	(particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('',@serty,'6','0','2','E','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,@amt1,@amt2,@amt3,@amt4,@amt5,@amt6
	)

	--<--6

--<---

-->Checking Blank Record
if not exists(select serty from #st3 where srno1='3' and srno2='F' and srno3='1' and srno4='A' and srno5=0)
begin
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','3','F','1','A','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
	
end

if not exists(select serty from #st3 where srno1='3' and srno2='F' and srno3='1' and srno4='A' and srno5=1)
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','3','F','1','A','1',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end
if not exists(select serty from #st3 where srno1='3' and srno2='F' and srno3='1' and srno4='A' and srno5=2)
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','3','F','1','A','2',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end

if not exists(select serty from #st3 where srno1='3' and srno2='F' and srno3='1' and srno4='B' and srno5=0)
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','3','F','1','B','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end

if not exists(select serty from #st3 where srno1='3' and srno2='F' and srno3='1' and srno4='C' and srno5=0)
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','3','F','1','C','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end
if not exists(select serty from #st3 where srno1='3' and srno2='F' and srno3='1' and srno4='C' and srno5=1)
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','3','F','1','C','1',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end
if not exists(select serty from #st3 where srno1='3' and srno2='F' and srno3='1' and srno4='C' and srno5=2)
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','3','F','1','C','2',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end
if not exists(select serty from #st3 where srno1='3' and srno2='F' and srno3='1' and srno4='C' and srno5=3)
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','3','F','1','C','3',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end

if not exists(select serty from #st3 where srno1='3' and srno2='F' and srno3='1' and srno4='D' and srno5=0)
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','3','F','1','D','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end
if not exists(select serty from #st3 where srno1='3' and srno2='F' and srno3='1' and srno4='E' and srno5=0)
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','3','F','1','E','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end
if not exists(select serty from #st3 where srno1='3' and srno2='F' and srno3='1' and srno4='F' and srno5=0)
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','3','F','1','F','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end
--if not exists(select serty from #st3 where srno1='3' and srno2='F' and srno3='1' and srno4='F' and srno5=1)
--begin 
--	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
--	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
--	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
--	,amt1,amt2,amt3,amt4,amt5,amt6
--	)
--	values
--	('','','3','F','1','F','1',' '
--	,0,0,0,0,0,0
--	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
--	,0,0,0,0,0,0
--	)
--end
--if not exists(select serty from #st3 where srno1='3' and srno2='F' and srno3='1' and srno4='F' and srno5=2)
--begin 
--	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
--	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
--	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
--	,amt1,amt2,amt3,amt4,amt5,amt6
--	)
--	values
--	('','','3','F','1','F','2',' '
--	,0,0,0,0,0,0
--	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
--	,0,0,0,0,0,0
--	)
--end
/*if not exists(select serty from #st3 where srno1='3' and srno2='F' and srno3='1' and srno4='F' and srno5='')
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','3','F','1','F','',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end*/
if not exists(select serty from #st3 where srno1='3' and srno2='F' and srno3='1' and srno4='G' and srno5='0')
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('Service Tax Payable','','3','F','1','G','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end
if not exists(select serty from #st3 where srno1='3' and srno2='F' and srno3='1' and srno4='H' and srno5='0')
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','3','F','1','H','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end
if not exists(select serty from #st3 where srno1='3' and srno2='F' and srno3='1' and srno4='I' and srno5='0')
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','3','F','1','I','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end
if not exists(select serty from #st3 where srno1='3' and srno2='F' and srno3='2' and srno4='J' and srno5='0')
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','3','F','2','J','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end
if not exists(select serty from #st3 where srno1='3' and srno2='F' and srno3='2' and srno4='K' and srno5='0')
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','3','F','2','K','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end
if not exists(select serty from #st3 where srno1='3' and srno2='F' and srno3='2' and srno4='L' and srno5='0')
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','3','F','2','L','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end
if not exists(select serty from #st3 where srno1='3' and srno2='F' and srno3='2' and srno4='M' and srno5='0')
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','3','F','2','M','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end
if not exists(select serty from #st3 where srno1='3' and srno2='F' and srno3='2' and srno4='N' and srno5='0')
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','3','F','2','N','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end

if not exists(select serty from #st3 where srno1='3' and srno2='F' and srno3='2' and srno4='N' and srno5='0')
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','3','F','2','N','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end

if not exists(select serty from #st3 where srno1='3' and srno2='F' and srno3='2' and srno4='O' and srno5='0')
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','3','F','2','O','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end
if not exists(select serty from #st3 where srno1='3' and srno2='F' and srno3='2' and srno4='P' and srno5='0')
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','3','F','2','P','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end

--4F
if not exists(select serty from #st3 where srno1='4' and srno2='A' and srno3='1' and srno4='A' and srno5='1')
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','A','1','A','1',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end
if not exists(select serty from #st3 where srno1='4' and srno2='A' and srno3='1' and srno4='A' and srno5='2')
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','A','1','A','2',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end
if not exists(select serty from #st3 where srno1='4' and srno2='A' and srno3='1' and srno4='A' and srno5='3')
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','A','1','A','3',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end
if not exists(select serty from #st3 where srno1='4' and srno2='A' and srno3='1' and srno4='A' and srno5='4')
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','A','1','A','4',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end

if not exists(select serty from #st3 where srno1='4' and srno2='A' and srno3='1' and srno4='B' and srno5='0')
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','A','1','B','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end
if not exists(select serty from #st3 where srno1='4' and srno2='A' and srno3='1' and srno4='B' and srno5='1')
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','A','1','B','1',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end
if not exists(select serty from #st3 where srno1='4' and srno2='A' and srno3='1' and srno4='B' and srno5='2')
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','A','1','B','2',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end
if not exists(select serty from #st3 where srno1='4' and srno2='A' and srno3='1' and srno4='B' and srno5='3')
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','A','1','B','3',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end
if not exists(select serty from #st3 where srno1='4' and srno2='A' and srno3='1' and srno4='B' and srno5='4')
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','A','1','B','4',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end

if not exists(select serty from #st3 where srno1='4' and srno2='A' and srno3='1' and srno4='C' and srno5='0')
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','A','1','C','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end
if not exists(select serty from #st3 where srno1='4' and srno2='A' and srno3='1' and srno4='C' and srno5='1')
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','A','1','C','1',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end
if not exists(select serty from #st3 where srno1='4' and srno2='A' and srno3='1' and srno4='C' and srno5='2')
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','A','1','C','2',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end
if not exists(select serty from #st3 where srno1='4' and srno2='A' and srno3='1' and srno4='C' and srno5='3')
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','A','1','C','3',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end
if not exists(select serty from #st3 where srno1='4' and srno2='A' and srno3='1' and srno4='C' and srno5='4')
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','A','1','C','4',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end

if not exists(select serty from #st3 where srno1='4' and srno2='A' and srno3='1' and srno4='D' and srno5='0')
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','A','1','D','0',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end
if not exists(select serty from #st3 where srno1='4' and srno2='A' and srno3='1' and srno4='D' and srno5='1')
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','A','1','D','1',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end
if not exists(select serty from #st3 where srno1='4' and srno2='A' and srno3='1' and srno4='D' and srno5='2')
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','A','1','D','2',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end
if not exists(select serty from #st3 where srno1='4' and srno2='A' and srno3='1' and srno4='D' and srno5='3')
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','A','1','D','3',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end
if not exists(select serty from #st3 where srno1='4' and srno2='A' and srno3='1' and srno4='D' and srno5='4')
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','A','1','D','4',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end
if not exists(select serty from #st3 where srno1='4' and srno2='A' and srno3='1' and srno4='D' and srno5='5')
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','A','1','D','5',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end
if not exists(select serty from #st3 where srno1='4' and srno2='A' and srno3='1' and srno4='D' and srno5='6')
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','A','1','D','6',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end
if not exists(select serty from #st3 where srno1='4' and srno2='A' and srno3='1' and srno4='D' and srno5='7')
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','A','1','D','7',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end
if not exists(select serty from #st3 where srno1='4' and srno2='A' and srno3='1' and srno4='D' and srno5='8')
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','A','1','D','8',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end
if not exists(select serty from #st3 where srno1='4' and srno2='A' and srno3='1' and srno4='D' and srno5='9')
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','A','1','D','9',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end
if not exists(select serty from #st3 where srno1='4' and srno2='A' and srno3='1' and srno4='D' and srno5='10')
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','A','1','D','10',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end
if not exists(select serty from #st3 where srno1='4' and srno2='A' and srno3='2' and srno4='A' and srno5='1')
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','A','2','A','1',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end
	
if not exists(select serty from #st3 where srno1='4' and srno2='A' and srno3='2' and srno4='A' and srno5='2')
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','A','2','A','2',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end
if not exists(select serty from #st3 where srno1='4' and srno2='B' and srno3='0')
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','B','0','','',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end

if not exists(select serty from #st3 where srno1='4' and srno2='C' and srno3='0')
begin 
	insert into #st3 (particulars,serty,srno1,srno2,srno3,srno4,srno5,srno6
	,SERBPER,SERBAMT,SERCPER,SERCAMT,SERHPER,SERHAMT
	,sdate1,sdate2,sdate3,sdate4,sdate5,sdate6
	,amt1,amt2,amt3,amt4,amt5,amt6
	)
	values
	('','','4','C','0','','',' '
	,0,0,0,0,0,0
	,@sdate1,@sdate2,@sdate3,@sdate4,@sdate5,@sdate6
	,0,0,0,0,0,0
	)
end

--<--Checking Blank Record

update #st3 set serty=isnull(serty,''), amt1=isnull(amt1,0),amt2=isnull(amt2,0),amt3=isnull(amt3,0),amt4=isnull(amt4,0),amt5=isnull(amt5,0),amt6=isnull(amt6,0)	

if @isdprouct=1 /*Rup 12/12/2009*/
begin
	update #st3 set serty='INPUT SERVICE DISTRIBUTION'
end

select L_YN=substring(@LYN,1,4)+'-'+substring(@LYN,8,2),* 
from #st3 
order by serty,srno1,srno2,srno3,srno4,cast(srno5 as int),srno6


/*ORDER BY SRNO1,SRNO2,SRNO3,SRNO4,SRNO5
select * from #bracdet
select * from #bracdet where typ like '%out%' order by tran_cd*/
end--sp


/*print '@sdate1= '+cast(@sdate1 as varchar)
print @edate1
print '@sdate2= '+cast(@sdate2 as varchar)
print @edate2
print '@sdate3= '+cast(@sdate3 as varchar)
print @edate3
print '@sdate4= '+cast(@sdate4 as varchar)
print @edate4
print '@sdate5= '+cast(@sdate5 as varchar)
print @edate5
print '@sdate6= '+cast(@sdate6 as varchar)
print @edate6*/




