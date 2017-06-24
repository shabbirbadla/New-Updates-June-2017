If exists(Select * from sysobjects where [name]='USP_REP_CUST_DND_VAT_30' and xtype='P')
Begin
	Drop Procedure USP_REP_CUST_DND_VAT_30
End
go
/*
EXECUTE [USP_REP_CUST_DND_VAT_30] '','','','04/01/2013','03/31/2014','','','','',0,0,'','','','','','','','','2013-2014','3'
*/
CREATE Procedure [dbo].[USP_REP_CUST_DND_VAT_30]
 @TMPAC NVARCHAR(50),@TMPIT NVARCHAR(50),@SPLCOND VARCHAR(8000),@SDATE  SMALLDATETIME,@EDATE SMALLDATETIME
 ,@SAC AS VARCHAR(60),@EAC AS VARCHAR(60)
 ,@SIT AS VARCHAR(60),@EIT AS VARCHAR(60)
 ,@SAMT FLOAT,@EAMT FLOAT
 ,@SDEPT AS VARCHAR(60),@EDEPT AS VARCHAR(60)
 ,@SCATE AS VARCHAR(60),@ECATE AS VARCHAR(60)
 ,@SWARE AS VARCHAR(60),@EWARE AS VARCHAR(60)
 ,@SINV_SR AS VARCHAR(60),@EINV_SR AS VARCHAR(60)
 ,@LYN VARCHAR(20)
 ,@EXPARA AS VARCHAR(60)= null
As
If exists(Select * from sysobjects where [name]='cust_dnd_vat_30_tbl' and xtype='U')
Begin
	Drop Table cust_dnd_vat_30_tbl
End

declare @sta_dt smalldatetime,@end_dt smalldatetime,@ret_period varchar(10),@costate varchar(35)
select @sta_dt=sta_dt,@end_dt=end_dt
,@ret_period = cast(year(sta_dt) as varchar(5))+RIGHT('0'+cast(month(end_dt) as varchar(2)),2),@costate=state
from vudyog..co_mast
where compid=@EXPARA

--drop table #lcode
SELECT ENTRY_TY,BCODE=(CASE WHEN EXT_VOU=1 THEN BCODE_NM ELSE ENTRY_TY END),STAX_ITEM 
INTO #LCODE
FROM LCODE

Create Table cust_dnd_vat_30_tbl
(
	vat_type varchar(50)
	,tran_date smalldatetime
	,tran_no varchar(30)
	,tran_cate varchar(2)
	,tran_type varchar(100)
	,seller_nm varchar(200)
	,seller_addr varchar(500)
	,dest_state varchar(35)
	,seller_tin_no varchar(30)
	,it_desc varchar(100)
	,vat_per decimal(4,2)
	,amt decimal(18,2)
	,tax_amt decimal(18,2)
)

--DamanVAT30

--1) Purchases eligible for credit of input tax
	Insert into 
		cust_dnd_vat_30_tbl(vat_type,tran_date,tran_no,tran_cate,tran_type,seller_nm,seller_tin_no,it_desc,vat_per,amt,tax_amt)
		select 'DamanVAT30',a.date,a.inv_no,'IN','Purchases eligible for credit of input tax',c.mailname,c.s_tax
		,(case when cast(d.it_desc as varchar)<>'' then cast(d.it_desc as varchar) else d.it_name end) as it_desc 
		,e.level1,round(b.u_asseamt+b.EXAMT+b.U_CESSAMT+b.U_HCESAMT+b.tot_add+b.TOT_DEDUC,2),b.taxamt
		from ptmain a
		inner join ptitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		inner join ac_mast c on (a.ac_id=c.ac_id)
		inner join it_mast d on (b.it_code=d.it_code)
		inner join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty)
		inner join #lcode f on (a.entry_ty=f.entry_ty)
		--where f.bcode='PT' and c.s_tax<>'' Bug-22611
		where f.bcode='PT' and c.s_tax<>'' and c.st_type='LOCAL' and d.vatcap<>1 and b.tax_name like '%VAT%'--and c.st_type='LOCAL' -- Bug-22611
		--upper(a.tax_name) like 'VAT%'
		and a.date between @sdate and @edate
		--and (a.tax_name like '%1%' or a.tax_name like '%4%' or a.tax_name like '%12.5%' or a.tax_name like '%20%')
		
--2) Purchases of Tax Free Sch - I Goods
	Insert into 
		cust_dnd_vat_30_tbl(vat_type,tran_date,tran_no,tran_cate,tran_type,seller_nm,seller_tin_no,it_desc,vat_per,amt,tax_amt)
		select 'DamanVAT30',a.date,a.inv_no,'CR','Purchases of Tax Free Sch - I Goods',c.mailname,c.s_tax
		,(case when cast(d.it_desc as varchar)<>'' then cast(d.it_desc as varchar) else d.it_name end) as it_desc 
		,e.level1,round(b.u_asseamt+b.EXAMT+b.U_CESSAMT+b.U_HCESAMT+b.tot_add+b.TOT_DEDUC,2),b.taxamt
		from ptmain a
		inner join ptitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		inner join ac_mast c on (a.ac_id=c.ac_id)
		inner join it_mast d on (b.it_code=d.it_code)
		inner join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty)
		inner join #lcode f on (a.entry_ty=f.entry_ty)
		where f.bcode='PT' and c.st_type='LOCAL'--and (a.tax_name like 'NO TAX%' or a.tax_name like 'EXEMP%')
		and (d.u_gvtype='Sch.-I Vat Goods' or a.vatmtype='Tax Free Sch - I goods')
		and a.date between @sdate and @edate

--3) Purchases not eligible for credit of input tax Purchase from Eligible Unit
Insert into 
		cust_dnd_vat_30_tbl(vat_type,tran_date,tran_no,tran_cate,tran_type,seller_nm,seller_tin_no,it_desc,vat_per,amt,tax_amt)
		select 'DamanVAT30',a.date,a.inv_no,'DR','Purchases not eligible for credit of input tax Purchase from Eligible Unit',c.mailname,c.s_tax
		,(case when cast(d.it_desc as varchar)<>'' then cast(d.it_desc as varchar) else d.it_name end) as it_desc 
		,e.level1,round(b.u_asseamt+b.EXAMT+b.U_CESSAMT+b.U_HCESAMT+b.tot_add+b.TOT_DEDUC,2),b.taxamt
		from ptmain a
		inner join ptitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		inner join ac_mast c on (a.ac_id=c.ac_id)
		inner join it_mast d on (b.it_code=d.it_code)
		inner join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty)
		inner join #lcode f on (a.entry_ty=f.entry_ty)
		where f.bcode='PT' and c.st_type='LOCAL'
		and a.vatmtype='Purchases not eligible for credit of input tax Purchase from Eligible Unit'
		and a.date between @sdate and @edate

--4) Purchase of Non Creditable Goods
Insert into 
		cust_dnd_vat_30_tbl(vat_type,tran_date,tran_no,tran_cate,tran_type,seller_nm,seller_tin_no,it_desc,vat_per,amt,tax_amt)
		select 'DamanVAT30',a.date,a.inv_no,'GR','Purchase of Non Creditable Goods',c.mailname,c.s_tax
		,(case when cast(d.it_desc as varchar)<>'' then cast(d.it_desc as varchar) else d.it_name end) as it_desc 
		,e.level1,round(b.u_asseamt+b.EXAMT+b.U_CESSAMT+b.U_HCESAMT+b.tot_add+b.TOT_DEDUC,2),b.taxamt
		from ptmain a
		inner join ptitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		inner join ac_mast c on (a.ac_id=c.ac_id)
		inner join it_mast d on (b.it_code=d.it_code)
		inner join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty)
		inner join #lcode f on (a.entry_ty=f.entry_ty)
		where f.bcode='PT' and c.s_tax='' and c.st_type='LOCAL'
		and a.vatmtype='Purchase of Non Creditable Goods' 
		and a.date between @sdate and @edate

--5) Job Work Charges Paid
	Insert into 
			cust_dnd_vat_30_tbl(vat_type,tran_date,tran_no,tran_cate,tran_type,seller_nm,seller_tin_no,it_desc,vat_per,amt,tax_amt)
			select 'DamanVAT30',a.date,a.inv_no,'CM','Job Work Charges Paid',c.mailname,c.s_tax
			,(case when cast(d.it_desc as varchar)<>'' then cast(d.it_desc as varchar) else d.it_name end) as it_desc 
			,e.level1,round(b.u_asseamt+b.EXAMT+b.U_CESSAMT+b.U_HCESAMT+b.tot_add+b.TOT_DEDUC,2),b.taxamt
			from irmain a
			inner join iritem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
			inner join ac_mast c on (a.ac_id=c.ac_id)
			inner join it_mast d on (b.it_code=d.it_code)
			inner join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty)
			where a.entry_ty='LR' and a.tax_name<>''
			and a.date between @sdate and @edate
			--Union
			--select 'DamanVAT30',a.date,a.inv_no,'CM','Job Work Charges Paid',c.mailname,c.s_tax
			--,'' as it_desc,e.level1,a.gro_amt,a.taxamt
			--from epmain a
			--inner join ac_mast c on (a.ac_id=c.ac_id)
			--inner join stax_mas e on (a.tax_name=e.tax_name and a.entry_ty=e.entry_ty)
			--where a.entry_ty='EP' and a.tax_name<>''
			--and a.date between @sdate and @edate


--6) Purchases from Unregistered Dealer
	Insert into 
		cust_dnd_vat_30_tbl(vat_type,tran_date,tran_no,tran_cate,tran_type,seller_nm,seller_tin_no,it_desc,vat_per,amt,tax_amt)
		select 'DamanVAT30',a.date,a.inv_no,'WC','Purchases from Unregistered Dealer',c.mailname,c.s_tax
		,(case when cast(d.it_desc as varchar)<>'' then cast(d.it_desc as varchar) else d.it_name end) as it_desc 
		,e.level1,round(b.u_asseamt+b.EXAMT+b.U_CESSAMT+b.U_HCESAMT+b.tot_add+b.TOT_DEDUC,2),b.taxamt
		from ptmain a
		inner join ptitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		inner join ac_mast c on (a.ac_id=c.ac_id)
		inner join it_mast d on (b.it_code=d.it_code)
		inner join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty)
		inner join #lcode f on (a.entry_ty=f.entry_ty)
		where f.bcode='PT' and c.s_tax=''  and c.st_type='LOCAL'-- Bug-22611
		and a.date between @sdate and @edate

--7) Purchases of Capital Goods
	Insert into 
		cust_dnd_vat_30_tbl(vat_type,tran_date,tran_no,tran_cate,tran_type,seller_nm,seller_tin_no,it_desc,vat_per,amt,tax_amt)
		select 'DamanVAT30',a.date,a.inv_no,'FC','Purchases of Capital Goods',c.mailname,c.s_tax
		,(case when cast(d.it_desc as varchar)<>'' then cast(d.it_desc as varchar) else d.it_name end) as it_desc 
		,e.level1,round(b.u_asseamt+b.EXAMT+b.U_CESSAMT+b.U_HCESAMT+b.tot_add+b.TOT_DEDUC,2),b.taxamt
		from ptmain a
		inner join ptitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		inner join ac_mast c on (a.ac_id=c.ac_id)
		inner join it_mast d on (b.it_code=d.it_code)
		inner join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty)
		inner join #lcode f on (a.entry_ty=f.entry_ty)
		where f.bcode='PT' and  d.vatcap=1 and c.st_type='LOCAL'
		and a.date between @sdate and @edate

--8) Purchases Taxable at Concessional Rate (Pending)
--Insert into 
--		cust_dnd_vat_30_tbl(vat_type,tran_date,tran_no,tran_cate,tran_type,seller_nm,seller_tin_no,it_desc,vat_per,amt,tax_amt)
--		select 'DamanVAT30',a.date,a.inv_no,'','Purchases Taxable at Concessional Rate',c.mailname,c.s_tax
--		,(case when cast(d.it_desc as varchar)<>'' then cast(d.it_desc as varchar) else d.it_name end) as it_desc 
--		,e.level1,b.gro_amt,b.taxamt
--		from ptmain a
--		inner join ptitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
--		inner join ac_mast c on (a.ac_id=c.ac_id)
--		inner join it_mast d on (b.it_code=d.it_code)
--		inner join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty)
--		where a.entry_ty='PT' and a.tax_name like 'vat%'
--		and a.tax_name not like '%1%' and a.tax_name not like '%4%' and a.tax_name not like '%12.5%' and a.tax_name not like '%20%'
--		and a.date between @sdate and @edate

--9) Any Other Purchase
Insert into 
		cust_dnd_vat_30_tbl(vat_type,tran_date,tran_no,tran_cate,tran_type,seller_nm,seller_tin_no,it_desc,vat_per,amt,tax_amt)
		select 'DamanVAT30',a.date,a.inv_no,'','Any Other Purchase',c.mailname,c.s_tax
		,(case when cast(d.it_desc as varchar)<>'' then cast(d.it_desc as varchar) else d.it_name end) as it_desc 
		,e.level1,round(b.u_asseamt+b.EXAMT+b.U_CESSAMT+b.U_HCESAMT+b.tot_add+b.TOT_DEDUC,2),b.taxamt
		from ptmain a
		inner join ptitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		inner join ac_mast c on (a.ac_id=c.ac_id)
		inner join it_mast d on (b.it_code=d.it_code)
		--inner join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty)-- Bug-22611
		left outer join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty)-- Bug-22611
		inner join #lcode f on (a.entry_ty=f.entry_ty)
		where f.bcode='PT' and c.st_type='LOCAL' and (b.tax_name not like '%VAT%') and (a.vatmtype not in ('Purchase of Non Creditable Goods',
		'Purchases not eligible for credit of input tax Purchase from Eligible Unit'))
		and a.date between @sdate and @edate

--DamanVAT30A
--1) Import From Outside india
Insert into 
		cust_dnd_vat_30_tbl(vat_type,tran_date,tran_no,tran_cate,tran_type,seller_nm,seller_addr,dest_state,seller_tin_no,it_desc,vat_per,amt)
		select 'DamanVAT30A',a.date,a.inv_no,'IN','Import From Outside india',c.mailname
		,(rtrim(c.add1)+' '+rtrim(c.add2)+' '+rtrim(c.add3)),case when @costate='' then 'OT' else f.code end,c.s_tax
		--,(rtrim(c.add1)+' '+rtrim(c.add2)+' '+rtrim(c.add3)),@costate,c.s_tax
		,(case when cast(d.it_desc as varchar)<>'' then cast(d.it_desc as varchar) else d.it_name end) as it_desc 
		,e.level1,round(b.u_asseamt+b.EXAMT+b.U_CESSAMT+b.U_HCESAMT+b.tot_add+b.TOT_DEDUC,2)
		from ptmain a
		inner join ptitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		inner join ac_mast c on (a.ac_id=c.ac_id)
		inner join it_mast d on (b.it_code=d.it_code)
		--inner join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty)-- Bug-22611
		left outer join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty)-- Bug-22611
		inner join state f on (f.state=@costate)
		inner join #lcode g on (a.entry_ty=g.entry_ty)
		where g.bcode='PT' and c.st_type='OUT OF COUNTRY'
		and (a.date between @sdate and @edate)

--2) Stock or Consignment transfer
Insert into 
		cust_dnd_vat_30_tbl(vat_type,tran_date,tran_no,tran_cate,tran_type,seller_nm,seller_addr,dest_state,seller_tin_no,it_desc,vat_per,amt)
		select 'DamanVAT30A',a.date,a.inv_no,'IN','Stock or Consignment transfer',c.mailname
		,(rtrim(c.add1)+' '+rtrim(c.add2)+' '+rtrim(c.add3)),case when @costate='' then 'OT' else f.code end ,c.s_tax
		,(case when cast(d.it_desc as varchar)<>'' then cast(d.it_desc as varchar) else d.it_name end) as it_desc 
		,e.level1,round(b.u_asseamt+b.EXAMT+b.U_CESSAMT+b.U_HCESAMT+b.tot_add+b.TOT_DEDUC,2)
		from ptmain a
		inner join ptitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		inner join ac_mast c on (a.ac_id=c.ac_id)
		inner join it_mast d on (b.it_code=d.it_code)
		--inner join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty) --Bug-22611
		left outer join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty)-- Bug-22611
		inner join state f on (f.state=@costate)
		inner join #lcode g on (a.entry_ty=g.entry_ty)
		where g.bcode='PT' and a.u_imporm IN ('Branch Transfer','Consignment Transfer')
		and (a.date between @sdate and @edate)

--3) Against form C
Insert into 
		cust_dnd_vat_30_tbl(vat_type,tran_date,tran_no,tran_cate,tran_type,seller_nm,seller_addr,dest_state,seller_tin_no,it_desc,vat_per,amt)
		select 'DamanVAT30A',a.date,a.inv_no,'IN','Against form C',c.mailname
		,(rtrim(c.add1)+' '+rtrim(c.add2)+' '+rtrim(c.add3)),case when @costate='' then 'OT' else f.code end ,c.s_tax
		,(case when cast(d.it_desc as varchar)<>'' then cast(d.it_desc as varchar) else d.it_name end) as it_desc 
		,e.level1,round(b.u_asseamt+b.EXAMT+b.U_CESSAMT+b.U_HCESAMT+b.tot_add+b.TOT_DEDUC,2)
		from ptmain a
		inner join ptitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		inner join ac_mast c on (a.ac_id=c.ac_id)
		inner join it_mast d on (b.it_code=d.it_code)
		--inner join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty) --Bug-22611
		left outer join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty)-- Bug-22611
		inner join state f on (f.state=@costate)
		inner join #lcode g on (a.entry_ty=g.entry_ty)
		where g.bcode='PT' and C.st_type='OUT OF STATE' 
		--and upper(e.tax_name) not like 'NO TAX%' and upper(e.tax_name) not like 'EXEM%'
		and e.form_nm IN ('Form C','C FORM') AND a.u_imporm not in ('Branch Transfer','Consignment Transfer')
		and (a.date between @sdate and @edate)

--4) Against form C without tax (exempted goods)
Insert into 
		cust_dnd_vat_30_tbl(vat_type,tran_date,tran_no,tran_cate,tran_type,seller_nm,seller_addr,dest_state,seller_tin_no,it_desc,vat_per,amt)
		select 'DamanVAT30A',a.date,a.inv_no,'IN','Against form C without tax (exempted goods)',c.mailname
		,(rtrim(c.add1)+' '+rtrim(c.add2)+' '+rtrim(c.add3)),case when @costate='' then 'OT' else f.code end ,c.s_tax
		,(case when cast(d.it_desc as varchar)<>'' then cast(d.it_desc as varchar) else d.it_name end) as it_desc 
		,e.level1,round(b.u_asseamt+b.EXAMT+b.U_CESSAMT+b.U_HCESAMT+b.tot_add+b.TOT_DEDUC,2)
		from ptmain a
		inner join ptitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		inner join ac_mast c on (a.ac_id=c.ac_id)
		inner join it_mast d on (b.it_code=d.it_code)
		--inner join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty) -- Bug-22611
		left outer join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty)-- Bug-22611		
		inner join state f on (f.state=@costate)
		inner join #lcode g on (a.entry_ty=g.entry_ty)
		where g.bcode='PT' and B.tax_name like 'EXEM%'
		and e.form_nm IN ('Form C','C FORM') AND a.u_imporm not in ('Branch Transfer','Consignment Transfer') and C.st_type='OUT OF STATE' 
		and (a.date between @sdate and @edate)

--5) Taxable Purchases
Insert into 
		cust_dnd_vat_30_tbl(vat_type,tran_date,tran_no,tran_cate,tran_type,seller_nm,seller_addr,dest_state,seller_tin_no,it_desc,vat_per,amt)
		select 'DamanVAT30A',a.date,a.inv_no,'IN','Taxable Purchases',c.mailname
		,(rtrim(c.add1)+' '+rtrim(c.add2)+' '+rtrim(c.add3)),case when @costate='' then 'OT' else f.code end ,c.s_tax
		,(case when cast(d.it_desc as varchar)<>'' then cast(d.it_desc as varchar) else d.it_name end) as it_desc 
		,e.level1,round(b.u_asseamt+b.EXAMT+b.U_CESSAMT+b.U_HCESAMT+b.tot_add+b.TOT_DEDUC,2)
		from ptmain a
		inner join ptitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		inner join ac_mast c on (a.ac_id=c.ac_id)
		inner join it_mast d on (b.it_code=d.it_code)
		--inner join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty) -- Bug-22611
		left outer join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty)-- Bug-22611		
		inner join state f on (f.state=@costate)
		inner join #lcode g on (a.entry_ty=g.entry_ty)
		--where g.bcode='PT' and a.tax_name<>'' -- Bug-22611
		where g.bcode='PT' and c.s_tax<>'' and a.tax_name like '%C.S.T.%' and C.st_type='OUT OF STATE' AND a.u_imporm not in ('Branch Transfer','Consignment Transfer') and e.form_nm=''-- Bug-22611
		and (a.date between @sdate and @edate)

--6) Against form H
Insert into 
		cust_dnd_vat_30_tbl(vat_type,tran_date,tran_no,tran_cate,tran_type,seller_nm,seller_addr,dest_state,seller_tin_no,it_desc,vat_per,amt)
		select 'DamanVAT30A',a.date,a.inv_no,'IN','Against form H',c.mailname
		,(rtrim(c.add1)+' '+rtrim(c.add2)+' '+rtrim(c.add3)),case when @costate='' then 'OT' else f.code end ,c.s_tax
		,(case when cast(d.it_desc as varchar)<>'' then cast(d.it_desc as varchar) else d.it_name end) as it_desc 
		,e.level1,round(b.u_asseamt+b.EXAMT+b.U_CESSAMT+b.U_HCESAMT+b.tot_add+b.TOT_DEDUC,2)
		from ptmain a
		inner join ptitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		inner join ac_mast c on (a.ac_id=c.ac_id)
		inner join it_mast d on (b.it_code=d.it_code)
		--inner join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty) -- Bug-22611
		left outer join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty)-- Bug-22611		
		inner join state f on (f.state=@costate)
		inner join #lcode g on (a.entry_ty=g.entry_ty)
		where g.bcode='PT' and E.FORM_NM IN ('Form H','H Form') AND a.u_imporm not in ('Branch Transfer','Consignment Transfer') and C.st_type='OUT OF STATE'
		and (a.date between @sdate and @edate)

--7) Labour charges Paid
Insert into 
		cust_dnd_vat_30_tbl(vat_type,tran_date,tran_no,tran_cate,tran_type,seller_nm,seller_addr,dest_state,seller_tin_no,it_desc,vat_per,amt)
		select 'DamanVAT30A',a.date,a.inv_no,'IN','Labour charges Paid',c.mailname
		,(rtrim(c.add1)+' '+rtrim(c.add2)+' '+rtrim(c.add3)),case when @costate='' then 'OT' else f.code end ,c.s_tax
		,(case when cast(d.it_desc as varchar)<>'' then cast(d.it_desc as varchar) else d.it_name end) as it_desc 
		,e.level1,round(b.u_asseamt+b.EXAMT+b.U_CESSAMT+b.U_HCESAMT+b.tot_add+b.TOT_DEDUC,2)
		from ptmain a
		inner join ptitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		inner join ac_mast c on (a.ac_id=c.ac_id)
		inner join it_mast d on (b.it_code=d.it_code)
		--inner join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty) -- Bug-22611
		left outer join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty)-- Bug-22611		
		inner join state f on (f.state=@costate)
		inner join #lcode g on (a.entry_ty=g.entry_ty)
		where g.bcode='PT' and a.vatmtype='Labour charges Paid' 
		and a.date between @sdate and @edate

--8) Purchases from Unregistered Dealer
Insert into 
		cust_dnd_vat_30_tbl(vat_type,tran_date,tran_no,tran_cate,tran_type,seller_nm,seller_addr,dest_state,seller_tin_no,it_desc,vat_per,amt)
		--select 'DamanVAT30A',a.date,a.inv_no,'IN','Against form H',c.mailname --Commented by Priyanka on 06022014
		select 'DamanVAT30A',a.date,a.inv_no,'IN','Purchases from Unregistered Dealer',c.mailname --Added by Priyanka on 06022014
		,(rtrim(c.add1)+' '+rtrim(c.add2)+' '+rtrim(c.add3)),case when @costate='' then 'OT' else f.code end ,c.s_tax
		,(case when cast(d.it_desc as varchar)<>'' then cast(d.it_desc as varchar) else d.it_name end) as it_desc 
		,e.level1,round(b.u_asseamt+b.EXAMT+b.U_CESSAMT+b.U_HCESAMT+b.tot_add+b.TOT_DEDUC,2)
		from ptmain a
		inner join ptitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		inner join ac_mast c on (a.ac_id=c.ac_id)
		inner join it_mast d on (b.it_code=d.it_code)
		--inner join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty) -- Bug-22611
		left outer join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty)-- Bug-22611		
		inner join state f on (f.state=@costate)
		inner join #lcode g on (a.entry_ty=g.entry_ty)
		where g.bcode='PT' and c.s_tax='' AND a.u_imporm not in ('Branch Transfer','Consignment Transfer') and  C.st_type='OUT OF STATE'
		and E.FORM_NM NOT IN ('Form H','H Form','Form C','C FORM') and  (a.date between @sdate and @edate)

select * from cust_dnd_vat_30_tbl
