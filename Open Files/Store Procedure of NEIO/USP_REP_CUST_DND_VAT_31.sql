If exists(Select * from sysobjects where [name]='USP_REP_CUST_DND_VAT_31' and xtype='P')
Begin
	Drop Procedure USP_REP_CUST_DND_VAT_31
End
go
create Procedure [dbo].[USP_REP_CUST_DND_VAT_31]
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
If exists(Select * from sysobjects where [name]='cust_dnd_vat_31_tbl' and xtype='U')
Begin
	Drop Table cust_dnd_vat_31_tbl
End

declare @sta_dt smalldatetime,@end_dt smalldatetime,@ret_period varchar(10)
select @sta_dt=sta_dt,@end_dt=end_dt
,@ret_period = cast(year(sta_dt) as varchar(5))+RIGHT('0'+cast(month(end_dt) as varchar(2)),2)
from vudyog..co_mast
where compid=@EXPARA
--select @ret_period

--drop table #lcode
SELECT ENTRY_TY,BCODE=(CASE WHEN EXT_VOU=1 THEN BCODE_NM ELSE ENTRY_TY END),STAX_ITEM 
INTO #LCODE
FROM LCODE

Create Table cust_dnd_vat_31_tbl
(
	vat_type varchar(50)
	--,return_period varchar(10)
	,tran_date smalldatetime
	,tran_cate varchar(2)
	,tran_no varchar(30)
	,buyer_nm varchar(200)
	,buyer_tin_no varchar(35)
	,dest_state varchar(35)
	,it_desc varchar(100)
	,sale_type varchar(200)
	,amt decimal(18,2)
	,vat_per decimal(4,2)
	,tax_amt decimal(18,2)
)
--(DamanVAT31 - return_period,tran_date,tran_cate,tran_no,buyer_nm,buyer_tin_no,it_desc,sale_type,amt,vat_per,tax_amt)
--(DamanVAT31A - return_period,tran_date,tran_cate,tran_no,buyer_nm,buyer_tin_no,dest_state,it_desc,sale_type,amt,vat_per,tax_amt)

--Below are the New Template Changes done on 24092013
--(DamanVAT31 - tran_date,tran_cate,tran_no,buyer_nm,buyer_tin_no,it_desc,sale_type,amt,vat_per,tax_amt)
--(DamanVAT31A - tran_date,tran_cate,tran_no,buyer_nm,buyer_tin_no,dest_state,it_desc,sale_type,amt,vat_per,tax_amt)

--DamanVAT31
--1) Sale of exempted Goods listed in Sch-I of Daman and Diu VAT regulation
	Insert into 
		cust_dnd_vat_31_tbl(vat_type,tran_date,tran_cate,tran_no,buyer_nm,buyer_tin_no,it_desc,sale_type,amt,vat_per,tax_amt)
		select 'DamanVAT31',a.date,'IN',a.inv_no,c.mailname,c.s_tax
		,(case when cast(d.it_desc as varchar)<>'' then cast(d.it_desc as varchar) else d.it_name end) as it_desc 
		,'Sale of exempted Goods listed in Sch-I of Daman and Diu VAT regulation',b.gro_amt,e.level1,b.taxamt
		from stmain a
		inner join stitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		inner join ac_mast c on (a.ac_id=c.ac_id)
		inner join it_mast d on (b.it_code=d.it_code)
		--inner join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty) -- Bug-22611
		left outer join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty)-- Bug-22611	
		inner join #lcode f on (a.entry_ty=f.entry_ty)
		where f.bcode='ST' and c.st_type='LOCAL'
		and a.vatmtype='Sale of exempted Goods listed in Sch-I of Daman and Diu VAT regulation'
		and (a.date between @sdate and @edate)
		
--2) Sale of Mfg/processed or Assembled goods by eligible unit
	Insert into 
		cust_dnd_vat_31_tbl(vat_type,tran_date,tran_cate,tran_no,buyer_nm,buyer_tin_no,it_desc,sale_type,amt,vat_per,tax_amt)
		select 'DamanVAT31',a.date,'IN',a.inv_no,c.mailname,c.s_tax
		,(case when cast(d.it_desc as varchar)<>'' then cast(d.it_desc as varchar) else d.it_name end) as it_desc 
		,'Sale of Mfg/processed or Assembled goods by eligible unit',b.gro_amt,e.level1,b.taxamt
		from stmain a
		inner join stitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		inner join ac_mast c on (a.ac_id=c.ac_id)
		inner join it_mast d on (b.it_code=d.it_code)
		--inner join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty) -- Bug-22611
		left outer join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty)-- Bug-22611	
		inner join #lcode f on (a.entry_ty=f.entry_ty)
		where f.bcode='ST' and c.st_type='LOCAL'
		and a.vatmtype='Sale of Mfg/processed or Assembled goods by eligible unit'
		and (a.date between @sdate and @edate)


--3) Sale of Taxable goods
Insert into 
		cust_dnd_vat_31_tbl(vat_type,tran_date,tran_cate,tran_no,buyer_nm,buyer_tin_no,it_desc,sale_type,amt,vat_per,tax_amt)
		select 'DamanVAT31',a.date,'IN',a.inv_no,c.mailname,c.s_tax
		,(case when cast(d.it_desc as varchar)<>'' then cast(d.it_desc as varchar) else d.it_name end) as it_desc 
		,'Sale of Taxable goods',b.gro_amt,e.level1,b.taxamt
		from stmain a
		inner join stitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		inner join ac_mast c on (a.ac_id=c.ac_id)
		inner join it_mast d on (b.it_code=d.it_code)
		--inner join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty) -- Bug-22611
		left outer join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty)-- Bug-22611	
		inner join #lcode f on (a.entry_ty=f.entry_ty)
		where f.bcode='ST' and b.tax_name not in ('','EXEMPTED') and c.st_type='LOCAL' AND E.RFORM_NM=''
		and (a.date between @sdate and @edate)


--4) Sale of Non Creditable Goods
Insert into 
		cust_dnd_vat_31_tbl(vat_type,tran_date,tran_cate,tran_no,buyer_nm,buyer_tin_no,it_desc,sale_type,amt,vat_per,tax_amt)
		select 'DamanVAT31',a.date,'IN',a.inv_no,c.mailname,c.s_tax
		,(case when cast(d.it_desc as varchar)<>'' then cast(d.it_desc as varchar) else d.it_name end) as it_desc 
		,'Sale of Non Creditable Goods',b.gro_amt,e.level1,b.taxamt
		from stmain a
		inner join stitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		inner join ac_mast c on (a.ac_id=c.ac_id)
		inner join it_mast d on (b.it_code=d.it_code)
		--inner join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty) -- Bug-22611
		left outer join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty)-- Bug-22611	
		inner join #lcode f on (a.entry_ty=f.entry_ty)
		where f.bcode='ST' and c.st_type='LOCAL'
		and a.vatmtype='Sale of Non Creditable Goods'
		and (a.date between @sdate and @edate)

--5) Labour Charges received
Insert into 
		cust_dnd_vat_31_tbl(vat_type,tran_date,tran_cate,tran_no,buyer_nm,buyer_tin_no,it_desc,sale_type,amt,vat_per,tax_amt)
		select 'DamanVAT31',a.date,'WC',a.inv_no,c.mailname,c.s_tax
		,(case when cast(d.it_desc as varchar)<>'' then cast(d.it_desc as varchar) else d.it_name end) as it_desc 
		,'Labour Charges received',b.gro_amt,e.level1,b.taxamt
		from stmain a
		inner join stitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		inner join ac_mast c on (a.ac_id=c.ac_id)
		inner join it_mast d on (b.it_code=d.it_code)
		--inner join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty) -- Bug-22611
		left outer join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty)-- Bug-22611	
		inner join #lcode f on (a.entry_ty=f.entry_ty)
		where f.bcode='ST' and c.st_type='LOCAL'
		and a.vatmtype='Labour Charges received'
		and (a.date between @sdate and @edate)

--6) Any other sale
Insert into 
	cust_dnd_vat_31_tbl(vat_type,tran_date,tran_cate,tran_no,buyer_nm,buyer_tin_no,it_desc,sale_type,amt,vat_per,tax_amt)
		select 'DamanVAT31',a.date,'CM',a.inv_no,c.mailname,c.s_tax
		,(case when cast(d.it_desc as varchar)<>'' then cast(d.it_desc as varchar) else d.it_name end) as it_desc 
		,'Any other sale',b.gro_amt,e.level1,b.taxamt
		from stmain a
		inner join stitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		inner join ac_mast c on (a.ac_id=c.ac_id)
		inner join it_mast d on (b.it_code=d.it_code)
		--inner join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty) -- Bug-22611
		left outer join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty)-- Bug-22611	
		inner join #lcode f on (a.entry_ty=f.entry_ty)
		where f.bcode='ST' and c.st_type='LOCAL' and (a.tax_name in ('','EXEMPTED')) AND a.vatmtype not in ('Labour Charges received','Sale of Non Creditable Goods','Sale of Mfg/processed or Assembled goods by eligible unit','Sale of exempted Goods listed in Sch-I of Daman and Diu VAT regulation')
		and e.rform_nm<>''
		and (a.date between @sdate and @edate)

--DamanVAT31A
--1) Sale to Registered Dealer against Form C
Insert into 
		cust_dnd_vat_31_tbl(vat_type,tran_date,tran_cate,tran_no,buyer_nm,buyer_tin_no,dest_state,it_desc,sale_type,amt,vat_per,tax_amt)
		select 'DamanVAT31A',a.date,'IN',a.inv_no,c.mailname,c.s_tax,case when c.state='' then 'OT' else f.code end
		,(case when cast(d.it_desc as varchar)<>'' then cast(d.it_desc as varchar) else d.it_name end) as it_desc 
		,'Sale to Registered Dealer against Form C',b.gro_amt,e.level1,b.taxamt
		from stmain a
		inner join stitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		inner join ac_mast c on (a.ac_id=c.ac_id)
		inner join it_mast d on (b.it_code=d.it_code)
		--inner join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty) -- Bug-22611
		left outer join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty)-- Bug-22611	
		inner join state f on (c.state=f.state)
		inner join #lcode g on (a.entry_ty=g.entry_ty)
		where g.bcode='ST' and c.s_tax<>'' and e.rform_nm IN ('Form C','C Form') and a.vatmtype=''  --and C.st_type='OUT OF STATE' 
		and (a.date between @sdate and @edate)

--2) Sale of Mfg/Processed or Assembled goods by eligible unit Regd Dealer against Form C [Sale Exempt U/s. 8(5)]
Insert into 
		cust_dnd_vat_31_tbl(vat_type,tran_date,tran_cate,tran_no,buyer_nm,buyer_tin_no,dest_state,it_desc,sale_type,amt,vat_per,tax_amt)
		select 'DamanVAT31A',a.date,'IN',a.inv_no,c.mailname,c.s_tax,case when c.state='' then 'OT' else f.code end
		,(case when cast(d.it_desc as varchar)<>'' then cast(d.it_desc as varchar) else d.it_name end) as it_desc 
		,'Sale of Mfg/Processed or Assembled goods by eligible unit Regd Dealer against Form C [Sale Exempt U/s. 8(5)]',b.gro_amt,e.level1,b.taxamt
		from stmain a
		inner join stitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		inner join ac_mast c on (a.ac_id=c.ac_id)
		inner join it_mast d on (b.it_code=d.it_code)
		--inner join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty) -- Bug-22611
		left outer join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty)-- Bug-22611	
		inner join state f on (c.state=f.state)
		inner join #lcode g on (a.entry_ty=g.entry_ty)
		where g.bcode='ST' and a.vatmtype='Sale of Mfg/Processed or Assembled goods by eligible unit Regd Dealer against Form C [Sale Exempt U/s. 8(5)]'
		and e.rform_nm IN ('Form C','C Form') --and C.st_type='OUT OF STATE' 
		and (a.date between @sdate and @edate)

--3) Branch/Consignment Transfer against Form F
Insert into 
		cust_dnd_vat_31_tbl(vat_type,tran_date,tran_cate,tran_no,buyer_nm,buyer_tin_no,dest_state,it_desc,sale_type,amt,vat_per,tax_amt)
		select 'DamanVAT31A',a.date,'IN',a.inv_no,c.mailname,c.s_tax,case when c.state='' then 'OT' else f.code end
		,(case when cast(d.it_desc as varchar)<>'' then cast(d.it_desc as varchar) else d.it_name end) as it_desc 
		,'Branch/Consignment Transfer against Form F',b.gro_amt,e.level1,b.taxamt
		from stmain a
		inner join stitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		inner join ac_mast c on (a.ac_id=c.ac_id)
		inner join it_mast d on (b.it_code=d.it_code)
		--inner join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty) -- Bug-22611
		left outer join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty)-- Bug-22611	
		inner join state f on (c.state=f.state)
		inner join #lcode g on (a.entry_ty=g.entry_ty)
		where g.bcode='ST' and (a.u_imporm='Branch Transfer' or a.u_imporm='Consignment Transfer')
		and e.rform_nm IN ('Form F','F Form') and a.vatmtype='' --and C.st_type='OUT OF STATE' 
		and (a.date between @sdate and @edate)

--4) Export out of India
Insert into 
		cust_dnd_vat_31_tbl(vat_type,tran_date,tran_cate,tran_no,buyer_nm,buyer_tin_no,dest_state,it_desc,sale_type,amt,vat_per,tax_amt)
		select 'DamanVAT31A',a.date,'IN',a.inv_no,c.mailname,c.s_tax,'OT'
		,(case when cast(d.it_desc as varchar)<>'' then cast(d.it_desc as varchar) else d.it_name end) as it_desc 
		,'Export out of India',b.gro_amt,e.level1,b.taxamt
		from stmain a
		inner join stitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		inner join ac_mast c on (a.ac_id=c.ac_id)
		inner join it_mast d on (b.it_code=d.it_code)
		--inner join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty) Bug-22611
		left outer join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty) -- Bug-22611
		inner join state f on (c.state=f.state)
		inner join #lcode g on (a.entry_ty=g.entry_ty)
		where g.bcode='ST' and c.st_type='OUT OF COUNTRY' 
		and (a.date between @sdate and @edate)

--5) Penultimate Sale to Export against Form H
Insert into 
		cust_dnd_vat_31_tbl(vat_type,tran_date,tran_cate,tran_no,buyer_nm,buyer_tin_no,dest_state,it_desc,sale_type,amt,vat_per,tax_amt)
		select 'DamanVAT31A',a.date,'IN',a.inv_no,c.mailname,c.s_tax,case when c.state='' then 'OT' else f.code end
		,(case when cast(d.it_desc as varchar)<>'' then cast(d.it_desc as varchar) else d.it_name end) as it_desc 
		,'Penultimate Sale to Export against Form H',b.gro_amt,e.level1,b.taxamt
		from stmain a
		inner join stitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		inner join ac_mast c on (a.ac_id=c.ac_id)
		inner join it_mast d on (b.it_code=d.it_code)
		--inner join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty) -- Bug-22611
		left outer join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty)-- Bug-22611	
		inner join state f on (c.state=f.state)
		inner join #lcode g on (a.entry_ty=g.entry_ty)
		where g.bcode='ST' and e.rform_nm IN ('Form H','H Form') and a.vatmtype='Penultimate Sale to Export against Form H'
		and (a.date between @sdate and @edate) and C.st_type='OUT OF STATE' 

--6) Sale to Dealers in SEZ against Form I
Insert into 
		cust_dnd_vat_31_tbl(vat_type,tran_date,tran_cate,tran_no,buyer_nm,buyer_tin_no,dest_state,it_desc,sale_type,amt,vat_per,tax_amt)
		select 'DamanVAT31A',a.date,'IN',a.inv_no,c.mailname,c.s_tax,case when c.state='' then 'OT' else f.code end
		,(case when cast(d.it_desc as varchar)<>'' then cast(d.it_desc as varchar) else d.it_name end) as it_desc 
		,'Sale to Dealers in SEZ against Form I',b.gro_amt,e.level1,b.taxamt
		from stmain a
		inner join stitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		inner join ac_mast c on (a.ac_id=c.ac_id)
		inner join it_mast d on (b.it_code=d.it_code)
		--inner join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty) -- Bug-22611
		left outer join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty)-- Bug-22611	
		inner join state f on (c.state=f.state)
		inner join #lcode g on (a.entry_ty=g.entry_ty)
		where g.bcode='ST' and e.rform_nm IN ('Form I','I Form') and a.vatmtype='Sale to Dealers in SEZ against Form I'
		and (a.date between @sdate and @edate) --and C.st_type='OUT OF STATE' 

--7) Sale of Tax Free goods listed in Sch-I of Daman and Diu VAT Regulation
Insert into 
		cust_dnd_vat_31_tbl(vat_type,tran_date,tran_cate,tran_no,buyer_nm,buyer_tin_no,dest_state,it_desc,sale_type,amt,vat_per,tax_amt)
		select 'DamanVAT31A',a.date,'IN',a.inv_no,c.mailname,c.s_tax,case when c.state='' then 'OT' else f.code end
		,(case when cast(d.it_desc as varchar)<>'' then cast(d.it_desc as varchar) else d.it_name end) as it_desc 
		,'Sale of Tax Free goods listed in Sch-I of Daman and Diu VAT Regulation',b.gro_amt,e.level1,b.taxamt
		from stmain a
		inner join stitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		inner join ac_mast c on (a.ac_id=c.ac_id)
		inner join it_mast d on (b.it_code=d.it_code)
		--inner join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty) -- Bug-22611
		left outer join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty)-- Bug-22611	
		inner join state f on (c.state=f.state)
		inner join #lcode g on (a.entry_ty=g.entry_ty)
		where g.bcode='ST' and a.vatmtype='Sale of Tax Free goods listed in Sch-I of Daman and Diu VAT Regulation'
		and (a.date between @sdate and @edate) --and C.st_type='OUT OF STATE' 

--8) Sale by Transfer of Document Exempt U/s.6(2) of the Act
Insert into 
		cust_dnd_vat_31_tbl(vat_type,tran_date,tran_cate,tran_no,buyer_nm,buyer_tin_no,dest_state,it_desc,sale_type,amt,vat_per,tax_amt)
		select 'DamanVAT31A',a.date,'IN',a.inv_no,c.mailname,c.s_tax,case when c.state='' then 'OT' else f.code end
		,(case when cast(d.it_desc as varchar)<>'' then cast(d.it_desc as varchar) else d.it_name end) as it_desc 
		,'Sale of Transfer of Document Exempt U/s.6(2) of the Act',b.gro_amt,e.level1,b.taxamt
		from stmain a
		inner join stitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		inner join ac_mast c on (a.ac_id=c.ac_id)
		inner join it_mast d on (b.it_code=d.it_code)
		--inner join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty) -- Bug-22611
		left outer join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty)-- Bug-22611	
		inner join state f on (c.state=f.state)
		inner join #lcode g on (a.entry_ty=g.entry_ty)
		where g.bcode='ST' and a.vatmtype='Sale by Transfer of Document Exempt U/s.6(2) of the Act'
		and (a.date between @sdate and @edate) --and C.st_type='OUT OF STATE' 

--9) Sale of goods in course of import into India (As defined in Section 5(2) of the Act) (High Seas/Sale/Purchase)
Insert into 
		cust_dnd_vat_31_tbl(vat_type,tran_date,tran_cate,tran_no,buyer_nm,buyer_tin_no,dest_state,it_desc,sale_type,amt,vat_per,tax_amt)
		select 'DamanVAT31A',a.date,'IN',a.inv_no,c.mailname,c.s_tax,case when c.state='' then 'OT' else f.code end
		,(case when cast(d.it_desc as varchar)<>'' then cast(d.it_desc as varchar) else d.it_name end) as it_desc 
		,'Sale of goods in course of import into India (As defined in Section 5(2) of the Act) (High Seas/Sale/Purchase)'
		,b.gro_amt,e.level1,b.taxamt
		from stmain a
		inner join stitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		inner join ac_mast c on (a.ac_id=c.ac_id)
		inner join it_mast d on (b.it_code=d.it_code)
		--inner join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty) -- Bug-22611
		left outer join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty)-- Bug-22611	
		inner join state f on (c.state=f.state)
		--inner join lcode g on (a.entry_ty=g.entry_ty)
		inner join #lcode g on (a.entry_ty=g.entry_ty)
		where g.bcode='ST' 
		and a.vatmtype='Sale of goods in course of import into India (As defined in Section 5(2) of the Act) (High Seas/Sale/Purchase)'
		and (a.date between @sdate and @edate) --and C.st_type='OUT OF STATE' 



--10) Taxable sale
Insert into 
		cust_dnd_vat_31_tbl(vat_type,tran_date,tran_cate,tran_no,buyer_nm,buyer_tin_no,dest_state,it_desc,sale_type,amt,vat_per,tax_amt)
		select 'DamanVAT31A',a.date,'IN',a.inv_no,c.mailname,c.s_tax,case when c.state='' then 'OT' else f.code end
		,(case when cast(d.it_desc as varchar)<>'' then cast(d.it_desc as varchar) else d.it_name end) as it_desc 
		,'Taxable sale'	,b.gro_amt,e.level1,b.taxamt
		from stmain a
		inner join stitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		inner join ac_mast c on (a.ac_id=c.ac_id)
		inner join it_mast d on (b.it_code=d.it_code)
		--inner join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty) -- Bug-22611
		left outer join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty)-- Bug-22611	
		inner join state f on (c.state=f.state)
		inner join #lcode g on (a.entry_ty=g.entry_ty)
		--where g.bcode='ST' and a.tax_name<>'' Bug-22611
		where g.bcode='ST'  and (b.tax_name not in ('','exempted')) and a.vatmtype='' and e.rform_nm=''-- Bug-22611
		and (a.date between @sdate and @edate) and C.st_type='OUT OF STATE' 

--11) Labour Charges received
Insert into 
		cust_dnd_vat_31_tbl(vat_type,tran_date,tran_cate,tran_no,buyer_nm,buyer_tin_no,dest_state,it_desc,sale_type,amt,vat_per,tax_amt)
		select 'DamanVAT31A',a.date,'IN',a.inv_no,c.mailname,c.s_tax,case when c.state='' then 'OT' else f.code end
		,(case when cast(d.it_desc as varchar)<>'' then cast(d.it_desc as varchar) else d.it_name end) as it_desc 
		,'Labour Charges received',b.gro_amt,e.level1,b.taxamt
		from stmain a
		inner join stitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		inner join ac_mast c on (a.ac_id=c.ac_id)
		inner join it_mast d on (b.it_code=d.it_code)
		--inner join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty) -- Bug-22611
		left outer join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty)-- Bug-22611	
		inner join state f on (c.state=f.state)
		inner join #lcode g on (a.entry_ty=g.entry_ty)
		where g.bcode='ST' and a.vatmtype='Labour Charges received' --and C.st_type='OUT OF STATE' 
		and a.date between @sdate and @edate

--12) Any other sale
Insert into 
		cust_dnd_vat_31_tbl(vat_type,tran_date,tran_cate,tran_no,buyer_nm,buyer_tin_no,dest_state,it_desc,sale_type,amt,vat_per,tax_amt)
		select 'DamanVAT31A',a.date,'IN',a.inv_no,c.mailname,c.s_tax,case when c.state='' then 'OT' else f.code end
		,(case when cast(d.it_desc as varchar)<>'' then cast(d.it_desc as varchar) else d.it_name end) as it_desc 
		,'Any other sale',b.gro_amt,e.level1,b.taxamt
		from stmain a
		inner join stitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		inner join ac_mast c on (a.ac_id=c.ac_id)
		inner join it_mast d on (b.it_code=d.it_code)
		--inner join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty) -- Bug-22611
		left outer join stax_mas e on (b.tax_name=e.tax_name and b.entry_ty=e.entry_ty)-- Bug-22611	
		inner join state f on (c.state=f.state)
		inner join #lcode g on (a.entry_ty=g.entry_ty)
		where (g.bcode='ST'  and a.vatmtype=''  and C.st_type='OUT OF STATE'  
		 AND C.st_type<>'OUT OF COUNTRY' AND e.rform_nm NOT IN ('C FORM','FORM C','')  and (a.date between @sdate and @edate))
		 OR (b.tax_name in ('','exempted') and C.st_type='OUT OF STATE')
		
select * from cust_dnd_vat_31_tbl
--go
--Execute USP_REP_CUST_DND_VAT_31 '','','','01/01/2013','12/31/2013','','','','',0,0,'','','','','','','','','2013-2014','48'
--GO