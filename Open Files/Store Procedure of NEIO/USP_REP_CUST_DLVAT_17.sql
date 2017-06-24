If exists(Select * from sysobjects where [name]='USP_REP_CUST_DLVAT_17' and xtype='P')
Begin
	Drop Procedure USP_REP_CUST_DLVAT_17
End
go

create Procedure [dbo].[USP_REP_CUST_DLVAT_17]
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
If exists(Select * from sysobjects where [name]='cust_dlvat_tbl_17' and xtype='U')
Begin
	Drop Table cust_dlvat_tbl_17
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
--declare @city varchar(50),@statecode varchar(50)
--select @city=a.city,@statecode=b.vat_state_code
--from vudyog..co_mast a
--inner join vudyog..state b on (a.state=b.state)
--where a.compid=@EXPARA

--drop table #lcode
SELECT ENTRY_TY,BCODE=(CASE WHEN EXT_VOU=1 THEN BCODE_NM ELSE ENTRY_TY END),STAX_TRAN,STAX_ITEM
INTO #LCODE
FROM LCODE

Create Table cust_dlvat_tbl_17
(
	vat_type varchar(50)
	,tran_date smalldatetime
	,inv_no varchar(30)
	,seller_tin_no varchar(30)
	,seller_name varchar(200)
	,cate_of_cont varchar(10)
	,rate_of_comp decimal(18,2)
	,tran_type varchar(100)
	,form_type varchar(10)
	,pur_amt decimal(18,2)
	,rate_of_tax varchar(5)--decimal(18,2)
	,tax_amt decimal(18,2)
	,comp_tax decimal(18,2)
	,form_43_id varchar(30)
	,form_43_date smalldatetime
	,sales_price decimal(18,2)
	,output_tax decimal(18,2)
	)

--vat_type,tran_date,seller_tin_no,seller_name,tran_type,form_type,pur_amt,rate_of_tax,tax_amt

--DVAT 17 2A

Insert into 
		cust_dlvat_tbl_17(vat_type,tran_date,seller_tin_no,seller_name,tran_type,form_type,pur_amt,rate_of_tax,tax_amt)
		
		select 'DVAT 17 2A',a.[date],c.s_tax,c.mailname
		,CASE WHEN A.U_IMPORM='' THEN 
		CASE WHEN C.ST_TYPE='LOCAL' AND C.S_TAX<>'' AND B.TAX_NAME LIKE '%VAT%' THEN 'Eligible Local purchases'
		WHEN C.ST_TYPE='LOCAL' AND C.S_TAX='' THEN 'Purchase from Unregistered Dealer'  
		WHEN C.ST_TYPE='OUT OF STATE' THEN 'Inter State Purchase'
		WHEN C.ST_TYPE='OUT OF COUNTRY' THEN 'Import from Outside India'
		ELSE '' END 
		ELSE
		A.U_IMPORM
		END
		,(case when upper(s.form_nm) in ('FORM C','C FORM') then 'C' else
		(case when upper(s.form_nm) IN ('FORM F','F FORM') then 'F' else
		(case when b.tax_name = '' or upper(b.tax_name) = 'NO-TAX' then 'None' else 
		'' end) end) end)
		,case when e.stax_item=1 then B.u_asseamt+B.EXAMT+B.U_CESSAMT+B.U_HCESAMT+B.tot_add+B.TOT_DEDUC else 
			(A.gro_amt+A.tot_add+A.tot_tax)-A.tot_deduc end
		--,s.level1
		,s.level1
		,case when e.stax_item=1 then b.taxamt else a.taxamt end
		from ptmain a
		inner join ptitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		inner join ac_mast c on (a.ac_id=c.ac_id)
		--inner join lcode e on (a.entry_ty=e.entry_ty)
		inner join #lcode e on (a.entry_ty=e.entry_ty)
		--left join stax_mas s on (b.entry_ty=s.entry_ty and b.tax_name=s.tax_name)
		left join stax_mas s on (b.entry_ty=s.entry_ty and (case when e.stax_item=1 then (b.tax_name) else 
			(case when e.stax_tran=1 then (a.tax_name) else '' end) end)=s.tax_name)
		inner join it_mast it on (b.it_code=it.it_code)
		left join state st on (c.state_id=st.state_id)
		where e.bcode = 'PT' --or a.entry_ty = 'PT')
		--and b.tax_name<>''
		and a.date between @sdate and @edate
		
		union all
		
		select 'DVAT 17 2A',a.[date],C.s_tax,C.mailname
		,CASE WHEN A.U_IMPORM='' THEN 
		CASE WHEN C.ST_TYPE='LOCAL' AND C.S_TAX='' AND B.TAX_NAME LIKE '%VAT%' THEN 'Eligible Local purchases'
		WHEN C.ST_TYPE='LOCAL' AND C.S_TAX<>'' THEN 'Purchase from Unregistered Dealer'  
		WHEN C.ST_TYPE='OUT OF STATE' THEN 'Inter State Purchase'
		WHEN C.ST_TYPE='OUT OF COUNTRY' THEN 'Import from Outside India'
		ELSE '' END 
		ELSE
		A.U_IMPORM
		END
		,(case when upper(s.form_nm) in ('FORM C','C FORM') then 'C' else
		(case when upper(s.form_nm) IN ('FORM F','F FORM') then 'F' else
		(case when b.tax_name = '' or upper(b.tax_name) = 'NO-TAX' then 'None' else 
		'' end) end) end)
		,case when e.stax_item=1 then (B.QTY*B.RATE)+B.EXAMT+B.U_CESSAMT+B.U_HCESAMT+B.tot_add+B.TOT_DEDUC else 
			(A.gro_amt+A.tot_add+A.tot_tax)-A.tot_deduc end
		--,s.level1
		,s.level1
		,case when e.stax_item=1 then b.taxamt else a.taxamt end
		from epmain a
		inner join epitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		--left join epacdet c on (a.entry_ty=c.entry_ty and a.tran_cd=c.tran_cd)
		inner join ac_mast C on (a.ac_id=C.ac_id)
		--inner join lcode e on (a.entry_ty=e.entry_ty)
		inner join #lcode e on (a.entry_ty=e.entry_ty)
		--left join stax_mas s on (b.entry_ty=s.entry_ty and b.tax_name=s.tax_name)
		left join stax_mas s on (b.entry_ty=s.entry_ty and (case when e.stax_item=1 then (b.tax_name) else 
			(case when e.stax_tran=1 then (a.tax_name) else '' end) end)=s.tax_name)
		left join it_mast it on (b.it_code=it.it_code)
		--left join state st on (d.state_id=st.state_id)
		where e.bcode = 'EP' --or a.entry_ty = 'EP')
		and b.tax_name<>''
		and a.date between @sdate and @edate
		
--vat_type,tran_date,inv_no,seller_tin_no,seller_name,cate_of_cont,rate_of_comp,pur_amt,comp_tax,form_43_id,form_43_date,sales_price,rate_of_tax,output_tax

--DVAT 17 2B

Insert into 
		cust_dlvat_tbl_17(vat_type,tran_date,inv_no,seller_tin_no,seller_name,cate_of_cont,rate_of_comp,pur_amt,comp_tax
		,form_43_id,form_43_date,sales_price,rate_of_tax,output_tax)
		
		select 'DVAT 17 2B',a.[date],a.inv_no,c.s_tax,c.mailname,'',0,a.net_amt
		,0,'',''
		--,(b.gro_amt - b.taxamt)
		,case when e.stax_item=1 then (b.gro_amt - b.taxamt) else 
			(case when e.stax_tran=1 then (a.gro_amt - a.taxamt) else 0 end) end
		--,s.level1
		,case when s.level1=0 then '0.00' else rtrim(replace(substring((case when e.stax_item=1 then b.tax_name else 
			(case when e.stax_tran=1 then a.tax_name else '' end) end)
			,charindex(' ',(case when e.stax_item=1 then b.tax_name else 
			(case when e.stax_tran=1 then a.tax_name else '' end) end)),len(case when e.stax_item=1 then b.tax_name else 
			(case when e.stax_tran=1 then a.tax_name else '' end) end)),'%','')) end
		--,b.taxamt
		,case when e.stax_item=1 then b.taxamt else 
			(case when e.stax_tran=1 then a.taxamt else 0 end) end
		from stmain a
		inner join stitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		inner join ac_mast c on (a.ac_id=c.ac_id)
		--inner join lcode e on (a.entry_ty=e.entry_ty)
		inner join #lcode e on (a.entry_ty=e.entry_ty)
		--left join stax_mas s on (b.entry_ty=s.entry_ty and b.tax_name=s.tax_name)
		left join stax_mas s on (b.entry_ty=s.entry_ty and (case when e.stax_item=1 then (b.tax_name) else 
			(case when e.stax_tran=1 then (a.tax_name) else '' end) end)=s.tax_name)
		inner join it_mast it on (b.it_code=it.it_code)
		where e.bcode = 'ST'-- or a.entry_ty = 'ST')
		and b.tax_name<>''
		and a.date between @sdate and @edate

		Union all

		select 'DVAT 17 2B',a.[date],a.inv_no,e.s_tax,e.mailname,a.u_catecont
		,(case when b.tax_name like 'VAT%' then s.level1 else 0 end)
		,a.net_amt,0--b.comptaxamt
		,a.u_dvat43id,a.u_dvat43dt
		--,(b.gro_amt - b.taxamt)
		,case when f.stax_item=1 then (b.gro_amt - b.taxamt) else (a.gro_amt - a.taxamt) end
		--,s.level1
		,s.level1
		,case when f.stax_item=1 then b.taxamt else a.taxamt end
		from sbmain a
		left outer join sbitem b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
		--inner join acdetalloc c on (c.entry_ty=b.entry_ty and c.tran_cd=b.tran_cd and c.itserial=b.itserial)
		--inner join sbacdet d on (a.entry_ty=d.entry_ty and a.tran_cd=d.tran_cd)
		inner join ac_mast e on (a.ac_id=e.ac_id)
		--inner join lcode f on (a.entry_ty=f.entry_ty)
		inner join #lcode f on (a.entry_ty=f.entry_ty)
		--left join stax_mas s on (b.entry_ty=s.entry_ty and b.tax_name=s.tax_name)
		left join stax_mas s on (b.entry_ty=s.entry_ty and (case when f.stax_item=1 then (b.tax_name) else 
			(case when f.stax_tran=1 then (a.tax_name) else '' end) end)=s.tax_name)
		left join it_mast it on (b.it_code=it.it_code)
		where f.bcode = 'SB'-- or a.entry_ty = 'SB'
		and a.date between @sdate and @edate

select * from cust_dlvat_tbl_17
order by vat_type,tran_date
--go
--Execute USP_REP_CUST_DLVAT_17 '','','','04/01/2013','03/31/2014','','','','',0,0,'','','','','','','','','2013-2014',''
--go
