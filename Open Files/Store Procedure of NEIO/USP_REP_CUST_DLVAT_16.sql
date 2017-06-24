if exists(select XTYPE,name from sysobjects where xtype='p' and name ='USP_REP_CUST_DLVAT_16')
begin
	drop procedure USP_REP_CUST_DLVAT_16
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create Procedure [dbo].[USP_REP_CUST_DLVAT_16]
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
If exists(Select * from sysobjects where [name]='cust_dlvat_tbl_16' and xtype='U')
Begin
	Drop Table cust_dlvat_tbl_16
End
/*declare @sta_dt smalldatetime,@end_dt smalldatetime,@ret_period varchar(10)
set @sta_dt=@sdate
set @end_dt=@edate
--select @sta_dt=sta_dt,@end_dt=end_dt
--,@ret_period = cast(year(sta_dt) as varchar(5))+RIGHT('0'+cast(month(end_dt) as varchar(2)),2)
select @ret_period = cast(year(@sta_dt) as varchar(5))+right('0'+cast(month(@sta_dt) as varchar(2)),2)
from vudyog..co_mast
where compid=@EXPARA
*/
declare @city varchar(50),@statecode varchar(50)
select @city=a.city,@statecode=b.vat_state_code
from vudyog..co_mast a
inner join vudyog..state b on (a.state=b.state)
where a.compid=@EXPARA

--drop table #lcode
SELECT ENTRY_TY,BCODE=(CASE WHEN EXT_VOU=1 THEN BCODE_NM ELSE ENTRY_TY END),STAX_TRAN,STAX_ITEM
INTO #LCODE
FROM LCODE

Create Table cust_dlvat_tbl_16
(
	vat_type varchar(50)
	,year_qtr int
	,tran_date smalldatetime
	,purbill_date smalldatetime
	,inv_no varchar(30)
	,seller_tin_no varchar(30)
	,seller_name varchar(200)
	,type_of_pur varchar(100)
	,tran_type varchar(100)
	,form_type varchar(10)
	,goods_type varchar(10)
	,rate_of_tax varchar(5) --decimal(18,2)
	,pur_amt decimal(18,2)
	,input_tax_paid decimal(18,2)
	,bill_amt decimal(18,2)
	,gr_no varchar(30)
	,gr_date smalldatetime
	,po_no varchar(30)
	,po_date smalldatetime
	,del_date smalldatetime
	,movnt_place_name varchar(200)
	,movnt_state varchar(100)
	,consign_goods_place_name varchar(200)
	,consign_goods_state varchar(100)
	,diesel_petrol_sales decimal(18,2)
	,chgs_civil decimal(18,2)
	,chgs_land_cost decimal(18,2)
	,sales_agnst_hform decimal(18,2)	
)

--vat_type,tran_date,purbill_date,inv_no,seller_tin_no,seller_name,type_of_pur,tran_type,form_type,goods_type,rate_of_tax,pur_amt,input_tax_paid,bill_amt,gr_no,gr_date,po_no,po_date,del_date,movnt_place_name,movnt_state,consign_goods_place_name,consign_goods_state

--DVAT 16 2A_DVAT 30_Bill

Insert into 
		cust_dlvat_tbl_16(vat_type,tran_date,purbill_date,inv_no,seller_tin_no,seller_name,type_of_pur,tran_type,form_type
		,goods_type,rate_of_tax,pur_amt,input_tax_paid,bill_amt,gr_no,gr_date,po_no,po_date,del_date,movnt_place_name
		,movnt_state,consign_goods_place_name,consign_goods_state)
		
		select 'DVAT 16 2A_DVAT 30_BILL',a.[date],a.u_pinvdt,a.inv_no,c.s_tax,c.mailname
		--Type of Purchase
		,CASE WHEN A.U_IMPORM='' THEN 
		CASE WHEN C.ST_TYPE='LOCAL' AND C.S_TAX<>'' AND B.TAX_NAME LIKE '%VAT%' THEN 'GD'
		WHEN C.ST_TYPE='LOCAL' AND C.S_TAX='' THEN 'PUR' END 
		ELSE
		CASE 
		WHEN A.U_IMPORM IN ('Purchase from Exempted Units','Tax free Goods') AND C.ST_TYPE='LOCAL'   THEN 'PTF'
		WHEN A.U_IMPORM='Non Creditable Goods' THEN 'PCG'
		WHEN A.U_IMPORM='Composition Dealer' THEN 'PCD'
		WHEN A.U_IMPORM='Against Retail Invoices' THEN 'PRI'
		WHEN A.U_IMPORM='Work Contract taxable' THEN 'WC'
		WHEN A.U_IMPORM='Purchase of Diesel & Petrol from Oil Marketing Companies' AND C.[STATE]='DELHI' THEN 'PPD' ELSE '' END
		END
		--Transaction Type
		,CASE WHEN A.U_IMPORM='' THEN 
		CASE WHEN C.ST_TYPE='LOCAL' AND C.S_TAX<>'' AND B.TAX_NAME LIKE '%VAT%' THEN 'Eligible Local purchases'
		WHEN C.ST_TYPE='LOCAL' AND C.S_TAX='' THEN 'Purchase from Unregistered Dealer'  
		WHEN C.ST_TYPE='OUT OF STATE' THEN 'Inter State Purchase'
		WHEN C.ST_TYPE='OUT OF COUNTRY' THEN 'Import from Outside India'
		ELSE '' END 
		ELSE
		A.U_IMPORM
		END
		,(case when C.ST_TYPE='OUT OF STATE' then 
		(case when upper(s.form_nm) in ('FORM C','C FORM') then 'C' else
		(case when upper(s.form_nm) IN ('FORM F','F FORM') then 'F' else
		(case when upper(s.form_nm) IN ('FORM C','C FORM') and (upper(s.form_nm) IN ('E1 FORM','FORM E1','E2 FORM','FORM E2')) then 'C + E1/E2' else
		(case when upper(s.form_nm) IN ('FORM H','H FORM') then 'H' else
		(case when upper(s.form_nm) IN ('FORM E1','E1 FORM') then 'E1' else
		(case when upper(s.form_nm) IN ('FORM E2','E2 FORM') then 'E2' else
		(case when upper(s.form_nm) IN ('FORM E1','E1 FORM') and upper(s.form_nm) IN ('FORM E2','E2 FORM') then 'E1 E2' else
		(case when b.tax_name = '' or upper(b.tax_name) = '' then 'None' else 
		'' end) end) end) end) end) end) end) end) else '' end)
		,case when upper(it.[type]) = 'MACHINERY/STORES' then 'CG' else 'OT' end
		,s.level1
		,case when e.stax_item=1 then B.u_asseamt+B.EXAMT+B.U_CESSAMT+B.U_HCESAMT+B.tot_add+B.TOT_DEDUC else 
			(A.gro_amt+A.tot_add+A.tot_tax)-A.tot_deduc end
		,case when e.stax_item=1 then b.taxamt else 
		 a.taxamt END
		--PONO,PODT AND DELIVARY DATE IS ADDED FROM BUG-21051
		,a.net_amt,a.u_chalno,a.u_chaldt
		,u_pono=ISNULL(A.U_PONO,''),U_PODT=CASE WHEN YEAR(A.U_PODT)<=1900 THEN '' ELSE CONVERT(VARCHAR(10),A.U_PODT,103) END,u_delidate=CASE WHEN YEAR(A.U_DELIDATE)<=1900 THEN '' ELSE CONVERT(VARCHAR(10),A.U_DELIDATE,103) END
		,c.city,st.vat_state_code,@city,@statecode
		from ptmain a
		inner join ptitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		inner join ac_mast c on (a.ac_id=c.ac_id)
		inner join #lcode e on (a.entry_ty=e.entry_ty)
		left join stax_mas s on (b.entry_ty=s.entry_ty and (case when e.stax_item=1 then (b.tax_name) else (case when e.stax_item=0 then (a.tax_name) else '' end) end)=s.tax_name)
		inner join it_mast it on (b.it_code=it.it_code)
		left join state st on (c.state_id=st.state_id)
		where e.bcode = 'PT'
		and a.date between @sdate and @edate
		
		union all
		
		select 'DVAT 16 2A_DVAT 30_BILL',a.[date],a.u_pinvdt,a.inv_no,C.s_tax,C.mailname
		--Type of Purchase
		,CASE WHEN A.U_IMPORM='' THEN 
		CASE WHEN C.ST_TYPE='LOCAL' AND C.S_TAX<>'' AND B.TAX_NAME LIKE '%VAT%' THEN 'GD'
		WHEN C.ST_TYPE='LOCAL' AND C.S_TAX='' THEN 'PUR' END 
		ELSE
		CASE 
		WHEN A.U_IMPORM IN ('Purchase from Exempted Units','Tax free Goods') AND C.ST_TYPE='LOCAL'   THEN 'PTF'
		WHEN A.U_IMPORM='Non Creditable Goods' THEN 'PCG'
		WHEN A.U_IMPORM='Composition Dealer' THEN 'PCD'
		WHEN A.U_IMPORM='Against Retail Invoices' THEN 'PRI'
		WHEN A.U_IMPORM='Work Contract taxable' THEN 'WC'
		WHEN A.U_IMPORM='Purchase of Diesel & Petrol from Oil Marketing Companies' AND C.[STATE]='DELHI' THEN 'PPD' ELSE '' END
		END
		--Transaction Type
		,CASE WHEN A.U_IMPORM='' THEN 
		CASE WHEN C.ST_TYPE='LOCAL' AND C.S_TAX<>'' AND B.TAX_NAME LIKE '%VAT%' THEN 'Eligible Local purchases'
		WHEN C.ST_TYPE='LOCAL' AND C.S_TAX='' THEN 'Purchase from Unregistered Dealer'  
		WHEN C.ST_TYPE='OUT OF STATE' THEN 'Inter State Purchase'
		WHEN C.ST_TYPE='OUT OF COUNTRY' THEN 'Import from Outside India'
		ELSE '' END 
		ELSE
		A.U_IMPORM
		END
		,(case when C.ST_TYPE='OUT OF STATE' then 
		(case when upper(s.form_nm) in ('FORM C','C FORM') then 'C' else
		(case when upper(s.form_nm) IN ('FORM F','F FORM') then 'F' else
		(case when upper(s.form_nm) IN ('FORM C','C FORM') and (upper(s.form_nm) IN ('E1 FORM','FORM E1','E2 FORM','FORM E2')) then 'C + E1/E2' else
		(case when upper(s.form_nm) IN ('FORM H','H FORM') then 'H' else
		(case when upper(s.form_nm) IN ('FORM E1','E1 FORM') then 'E1' else
		(case when upper(s.form_nm) IN ('FORM E2','E2 FORM') then 'E2' else
		(case when upper(s.form_nm) IN ('FORM E1','E1 FORM') and upper(s.form_nm) IN ('FORM E2','E2 FORM') then 'E1 E2' else
		(case when b.tax_name = '' or upper(b.tax_name) = '' then 'None' else 
		'' end) end) end) end) end) end) end) end) else '' end)
		,case when upper(it.[type]) = 'MACHINERY/STORES' then 'CG' else 'OT' end
		,s.level1
		,case when e.stax_item=1 then (B.QTY*B.RATE)+B.EXAMT+B.U_CESSAMT+B.U_HCESAMT+B.tot_add+B.TOT_DEDUC else 
			(A.gro_amt+A.tot_add+A.tot_tax)-A.tot_deduc end
		,case when e.stax_item=1 then b.taxamt else a.taxamt end
		,a.net_amt,u_chalno='',u_chaldt='',u_pono='',u_podt='',u_delidate='',C.city,st.vat_state_code,@city,@statecode
		from epmain a
		inner join epitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		inner join ac_mast C on (a.ac_id=C.ac_id)
		inner join #lcode e on (a.entry_ty=e.entry_ty)
		left join stax_mas s on (b.entry_ty=s.entry_ty and (case when e.stax_item=1 then (b.tax_name) else 
			(case when e.stax_tran=1 then (a.tax_name) else '' end) end)=s.tax_name)
		left join it_mast it on (b.it_code=it.it_code)
		left join state st on (C.STATE=st.state)
		where e.bcode = 'EP'
		and a.date between @sdate and @edate

--DVAT 16 2B_DVAT 31

Insert into 
		cust_dlvat_tbl_16(vat_type,tran_date,inv_no,seller_tin_no,seller_name,tran_type
		,goods_type,form_type,type_of_pur,pur_amt,input_tax_paid,rate_of_tax,diesel_petrol_sales
		,chgs_civil,chgs_land_cost,sales_agnst_hform)
		
		select 'DVAT 16 2B_DVAT 31',a.[date],a.inv_no,c.s_tax,c.mailname
		,CASE WHEN A.U_IMPORM='' THEN case
		WHEN C.ST_TYPE='LOCAL'  THEN 'Local sales'
		WHEN C.ST_TYPE='OUT OF STATE'  THEN 'Inter State sales'
		WHEN C.ST_TYPE='OUT OF COUNTRY'  THEN 'Export Out of India' end
		ELSE
		CASE 
		WHEN A.U_IMPORM IN ('Inter-State Branch','Inter-State Consignment Transfer','Export Out of India','High Sea Sales','Inter State sales','Local sales') THEN a.u_imporm
		 ELSE '' END
		END
		,(case when upper(it.[type]) = 'MACHINERY/STORES' then 'CG' else 'OT' end)
		,(case when upper(s.Rform_nm) in ('FORM C','C FORM') then 'C' else
		(case when upper(s.Rform_nm) IN ('FORM F','F FORM') then 'F' else
		(case when upper(s.Rform_nm) IN ('FORM C','C FORM') and (upper(s.rform_nm) IN ('E1 FORM','FORM E1','E2 FORM','FORM E2')) then 'C + E1/E2' else
		(case when upper(s.Rform_nm) IN ('FORM H','H FORM') then 'H' else
		(case when upper(s.Rform_nm) IN ('FORM E1','E1 FORM') then 'E1' else
		(case when upper(s.Rform_nm) IN ('FORM E2','E2 FORM') then 'E2' else
		(case when upper(s.Rform_nm) IN ('FORM E1','E1 FORM') and upper(s.rform_nm) IN ('FORM E2','E2 FORM') then 'E1 E2' else
		(case when b.tax_name = '' or upper(b.tax_name) = '' then 'None' else 
		'' end) end) end) end) end) end) end) end) 
		,CASE WHEN C.ST_TYPE='LOCAL' AND B.TAX_NAME LIKE '%VAT%' THEN 'GD' ELSE '' END	
		,case when e.stax_item=1 then B.u_asseamt+B.EXAMT+B.U_CESSAMT+B.U_HCESAMT+B.tot_add+B.TOT_DEDUC else 
		(A.gro_amt+A.tot_add+A.tot_tax)-A.tot_deduc end
		,case when e.stax_item=1 then b.taxamt else  a.taxamt END
		,s.level1
		,case when (it.it_name like '%petrol%' or it.it_name like '%diesel%') then 
		(case when e.stax_item=1 then b.gro_amt else 
		(case when e.stax_tran=1 then a.gro_amt else 0 end) end) else 0 end
		,0
		,case when (it.it_name like '%land%') then (case when E.stax_item=1 then b.gro_amt else 0 end) end
		,case when upper(s.rform_nm) IN ('FORM H','H FORM') and s.st_type='LOCAL'
			then (case when e.stax_item=1 then b.gro_amt else a.gro_amt end) end
		from stmain a
		inner join stitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		inner join ac_mast c on (a.ac_id=c.ac_id)
		inner join #lcode e on (a.entry_ty=e.entry_ty)
		left join stax_mas s on (b.entry_ty=s.entry_ty and (case when e.stax_item=1 then (b.tax_name) else 
			(case when e.stax_tran=1 then (a.tax_name) else '' end) end)=s.tax_name)
		inner join it_mast it on (b.it_code=it.it_code)
		where e.bcode = 'ST'
		and b.tax_name<>''
		and a.date between @sdate and @edate

		Union all

		select 'DVAT 16 2B_DVAT 31',a.[date],a.inv_no,C.s_tax,C.mailname
		,CASE WHEN A.U_IMPORM='' THEN case
		WHEN C.ST_TYPE='LOCAL'  THEN 'Local sales'
		WHEN C.ST_TYPE='OUT OF STATE'  THEN 'Inter State sales'
		WHEN C.ST_TYPE='OUT OF COUNTRY'  THEN 'Export Out of India' end
		ELSE
		CASE 
		WHEN A.U_IMPORM IN ('Inter-State Branch','Inter-State Consignment Transfer','Export Out of India','High Sea Sales','Inter State sales','Local sales') THEN a.u_imporm
		 ELSE '' END
		END
		,(case when upper(it.[type]) = 'MACHINERY/STORES' then 'CG' else 'OT' end)
		,(case when upper(s.Rform_nm) in ('FORM C','C FORM') then 'C' else
		(case when upper(s.Rform_nm) IN ('FORM F','F FORM') then 'F' else
		(case when upper(s.Rform_nm) IN ('FORM C','C FORM') and (upper(s.rform_nm) IN ('E1 FORM','FORM E1','E2 FORM','FORM E2')) then 'C + E1/E2' else
		(case when upper(s.Rform_nm) IN ('FORM H','H FORM') then 'H' else
		(case when upper(s.Rform_nm) IN ('FORM E1','E1 FORM') then 'E1' else
		(case when upper(s.Rform_nm) IN ('FORM E2','E2 FORM') then 'E2' else
		(case when upper(s.Rform_nm) IN ('FORM E1','E1 FORM') and upper(s.rform_nm) IN ('FORM E2','E2 FORM') then 'E1 E2' else
		(case when b.tax_name = '' or upper(b.tax_name) = '' then 'None' else 
		'' end) end) end) end) end) end) end) end) 
		,CASE WHEN C.ST_TYPE='LOCAL' AND B.TAX_NAME LIKE '%VAT%' THEN 'GD' ELSE '' END	
		,case when e.stax_item=1 then B.u_asseamt+B.EXAMT+B.U_CESSAMT+B.U_HCESAMT+B.tot_add+B.TOT_DEDUC else 
		(A.gro_amt+A.tot_add+A.tot_tax)-A.tot_deduc end
		,case when e.stax_item=1 then b.taxamt else  a.taxamt END
		,s.level1
		,case when (it.it_name like '%petrol%' or it.it_name like '%diesel%') then 
		(case when e.stax_item=1 then b.gro_amt else a.gro_amt end) end
		,0
		,case when (it.it_name like '%land%') then (case when E.stax_item=1 then b.gro_amt else 0 end) end
		,case when upper(s.rform_nm) IN ('FORM H','H FORM') and s.st_type='LOCAL'
			then (case when e.stax_item=1 then b.gro_amt else a.gro_amt end) end
		from sBmain a
		inner join sbitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		inner join ac_mast C on (a.ac_id=C.ac_id)
		inner join #lcode E on (a.entry_ty=E.entry_ty)
		left join stax_mas s on (b.entry_ty=s.entry_ty and (case when E.stax_item=1 then (b.tax_name) else 
			(case when E.stax_tran=1 then (a.tax_name) else '' end) end)=s.tax_name)
		left join it_mast it on (b.it_code=it.it_code)
		where E.bcode = 'SB' 
		and a.date between @sdate and @edate

--vat_type,year_qtr,seller_tin_no,seller_name,tran_type,pur_amt,input_tax_paid,form_type

--DVAT 16 2C2D

Insert into 
		cust_dlvat_tbl_16(vat_type,year_qtr,seller_tin_no,seller_name,tran_type
		,pur_amt,input_tax_paid,form_type)
		
		select 'DVAT 16 2C2D',isnull(cast(cast(year(a.date) as varchar(5))
		+ cast((case when month(a.date)=4 or month(a.date)=5 or month(a.date)=6 then 41 else 
		(case when month(a.date)=7 or month(a.date)=8 or month(a.date)=9 then 42 else 
		(case when month(a.date)=10 or month(a.date)=11 or month(a.date)=12 then 43 else 
		(case when month(a.date)=1 or month(a.date)=2 or month(a.date)=3 then 44 else 
		0 end) end) end) end) as varchar(5)) as int),'')
		,d.s_tax,d.mailname,a.entry_ty,a.net_amt
		--,b.taxamt
		,case when e.stax_item=1 then b.taxamt else 
			(case when e.stax_tran=1 then a.taxamt else 0 end) end
		,(case when d.[group] like '%credito%' then '2C' else 
			(case when d.[group] like '%debto%' then '2D' else '' end) end)
		from cnmain a
		left join cnitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		--inner join cnacdet c on (a.entry_ty=c.entry_ty and a.tran_cd=c.tran_cd)
		inner join ac_mast d on (a.ac_id=d.ac_id)
		inner join #lcode e on (a.entry_ty=e.entry_ty)
		where e.bcode='CN'
		and a.date between @sdate and @edate
		
		union all
		
		select 'DVAT 16 2C2D',isnull(cast(cast(year(a.date) as varchar(5))
		+ cast((case when month(a.date)=4 or month(a.date)=5 or month(a.date)=6 then 41 else 
		(case when month(a.date)=7 or month(a.date)=8 or month(a.date)=9 then 42 else 
		(case when month(a.date)=10 or month(a.date)=11 or month(a.date)=12 then 43 else 
		(case when month(a.date)=1 or month(a.date)=2 or month(a.date)=3 then 44 else 
		0 end) end) end) end) as varchar(5)) as int),'')
		,d.s_tax,d.mailname,a.entry_ty,a.net_amt
		--,b.taxamt
		,case when e.stax_item=1 then b.taxamt else 
			(case when e.stax_tran=1 then a.taxamt else 0 end) end
		,(case when d.[group] like '%credito%' then '2C' else 
			(case when d.[group] like '%debto%' then '2D' else '' end) end)
		from dnmain a
		left join dnitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		--inner join dnacdet c on (a.entry_ty=c.entry_ty and a.tran_cd=c.tran_cd)
		inner join ac_mast d on (a.ac_id=d.ac_id)
		inner join #lcode e on (a.entry_ty=e.entry_ty)
		where e.bcode='DN'
		and a.date between @sdate and @edate
		
select * from cust_dlvat_tbl_16
order by vat_type,tran_date
