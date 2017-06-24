
/****** Object:  StoredProcedure [dbo].[USP_REP_PUR]    Script Date: 08/03/2011 11:19:05 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_REP_PUR]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_REP_PUR]
Go

-- =============================================
-- Created by :	Ramya.
-- Create date: 08/03/2011
-- Description:	This Stored procedure is useful to generate Sales Audit report between particular amounts .
-- Modified By: 
-- Modify date: 
-- Remark:
-- =============================================


CREATE PROCEDURE [dbo].[USP_REP_PUR]
@TMPAC NVARCHAR(50),@TMPIT NVARCHAR(50),@SPLCOND VARCHAR(8000),@SDATE  SMALLDATETIME,@EDATE SMALLDATETIME
,@SAC AS VARCHAR(60),@EAC AS VARCHAR(60)
,@SIT AS VARCHAR(60),@EIT AS VARCHAR(60)
,@SAMT NUMERIC(20,2),@EAMT FLOAT
,@SDEPT AS VARCHAR(60),@EDEPT AS VARCHAR(60)
,@SCATE AS VARCHAR(60),@ECATE AS VARCHAR(60)
,@SWARE AS VARCHAR(60),@EWARE AS VARCHAR(60)
	,@SINV_SR AS VARCHAR(60),@EINV_SR AS VARCHAR(60)
,@LYN VARCHAR(20)
,@EXPARA  AS VARCHAR(60)= NULL
AS
Select BHent=(CASE WHEN EXT_VOU=1 THEN BCODE_NM ELSE ENTRY_TY END),Entry_ty,Code_nm Into #L from Lcode Order by BHent

 

Select a.Entry_ty,a.Tran_cd,tot_deduc=sum(a.tot_deduc),tot_tax=sum(a.tot_tax)
,tot_add=sum(a.tot_add),tot_nontax=sum(a.tot_nontax),tot_fdisc=sum(a.tot_fdisc),u_asseamt=sum(a.qty * a.Rate)
Into #ptstax2 from ptitem a
where a.tax_name<>' ' and a.date between @sdate and @edate
group by a.Entry_ty,a.Tran_cd
Union all
Select a.Entry_ty,a.Tran_cd,tot_deduc=sum(a.tot_deduc),tot_tax=sum(a.tot_tax)
,tot_add=sum(a.tot_add),tot_nontax=sum(a.tot_nontax),tot_fdisc=sum(a.tot_fdisc),u_asseamt=sum(a.qty * a.Rate)
from pritem a
where a.tax_name<>' ' and a.date between @sdate and @edate
Group by a.Entry_ty,a.Tran_cd
Union all

Select a.Entry_ty,a.Tran_cd,tot_deduc=sum(a.tot_deduc),tot_tax=sum(a.tot_tax)
,tot_add=sum(a.tot_add),tot_nontax=sum(a.tot_nontax),tot_fdisc=sum(a.tot_fdisc),u_asseamt=sum(a.qty * a.Rate)
from epitem a
where a.tax_name<>' ' and a.date between @sdate and @edate
Group by a.Entry_ty,a.Tran_cd

Union all
Select a.Entry_ty,a.Tran_cd,tot_deduc=sum(a.tot_deduc),tot_tax=sum(a.tot_tax)
,tot_add=sum(a.tot_add),tot_nontax=sum(a.tot_nontax),tot_fdisc=sum(a.tot_fdisc),u_asseamt=sum(a.qty * a.Rate)
from stitem a
where a.tax_name<>' ' and a.date between @sdate and @edate
Group by a.Entry_ty,a.Tran_cd





select a.tran_cd,a.party_nm,a.entry_ty,mon=month(a.date),yearr=year(a.date),monthh=datename(mm,a.date)
,a.date,a.inv_no,a.gro_amt,tot_deduc=a.tot_deduc+abs(b.tot_deduc),tot_tax=a.tot_tax+b.tot_tax
,a.tot_examt,tot_add=a.tot_add+b.tot_add,a.tax_name,a.taxamt
,tot_nontax=a.tot_nontax+b.tot_nontax,tot_fdisc=a.tot_fdisc+abs(b.tot_fdisc),a.net_amt 
,b.u_asseamt
,Net_amt2=b.u_asseamt-(a.tot_deduc+abs(b.tot_deduc))+(a.tot_tax+b.tot_tax)+a.tot_examt
		+a.taxamt+(a.tot_add+b.tot_add)+(a.tot_nontax+b.tot_nontax)-(a.tot_fdisc+abs(b.tot_fdisc))
,srno='a' ,ac.vend_type into #ptstax 
from ptmain a 
Inner Join #ptstax2 b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
Inner Join ac_mast ac on (ac.ac_id=a.ac_id)
where A.tax_name<>' ' and a.date between @sdate and @edate and net_amt>=+CAST(@SAMT AS VARCHAR(25))
union all
select a.tran_cd,a.party_nm,a.entry_ty,mon=month(a.date),yearr=year(a.date),monthh=datename(mm,a.date)
,a.date,a.inv_no,a.gro_amt,tot_deduc=a.tot_deduc+abs(b.tot_deduc),tot_tax=a.tot_tax+b.tot_tax
,a.tot_examt,tot_add=a.tot_add+b.tot_add,a.tax_name,a.taxamt
,tot_nontax=a.tot_nontax+b.tot_nontax,tot_fdisc=a.tot_fdisc+abs(b.tot_fdisc),a.net_amt 
,b.u_asseamt
,Net_amt2=b.u_asseamt-(a.tot_deduc+b.tot_deduc)+(a.tot_tax+b.tot_tax)+a.tot_examt
		+a.taxamt+(a.tot_add+b.tot_add)+(a.tot_nontax+b.tot_nontax)-(a.tot_fdisc+abs(b.tot_fdisc))
,srno='b' ,ac.vend_type
from Prmain a
Inner Join #ptstax2 b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
Inner Join ac_mast ac on (ac.ac_id=a.ac_id)
where a.tax_name<>' ' and a.date between @sdate and @edate and net_amt>=+CAST(@SAMT AS VARCHAR(25))
union all
select a.tran_cd,a.party_nm,a.entry_ty,mon=month(a.date),yearr=year(a.date),monthh=datename(mm,a.date)
,a.date,a.inv_no,a.gro_amt,tot_deduc=a.tot_deduc+abs(b.tot_deduc),tot_tax=a.tot_tax+b.tot_tax
,a.tot_examt,tot_add=a.tot_add+b.tot_add,a.tax_name,a.taxamt
,tot_nontax=a.tot_nontax+b.tot_nontax,tot_fdisc=a.tot_fdisc+abs(b.tot_fdisc),a.net_amt 
,b.u_asseamt
,Net_amt2=b.u_asseamt-(a.tot_deduc+abs(b.tot_deduc))+(a.tot_tax+b.tot_tax)+a.tot_examt
		+a.taxamt+(a.tot_add+b.tot_add)+(a.tot_nontax+b.tot_nontax)-(a.tot_fdisc+abs(b.tot_fdisc))
,srno='c' ,ac.vend_type
from epmain a
left Join #ptstax2 b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
left Join ac_mast ac on (ac.ac_id=a.ac_id)
where a.tax_name<>' ' and a.date between @sdate and @edate and net_amt>=+CAST(@SAMT AS VARCHAR(25))
union all
select a.tran_cd,a.party_nm,a.entry_ty,mon=month(a.date),yearr=year(a.date),monthh=datename(mm,a.date)
,a.date,a.inv_no,a.gro_amt,tot_deduc=a.tot_deduc+abs(b.tot_deduc),tot_tax=a.tot_tax+b.tot_tax
,a.tot_examt,tot_add=a.tot_add+b.tot_add,a.tax_name,a.taxamt
,tot_nontax=a.tot_nontax+b.tot_nontax,tot_fdisc=a.tot_fdisc+abs(b.tot_fdisc),a.net_amt 
,b.u_asseamt
,Net_amt2=b.u_asseamt-(a.tot_deduc+abs(b.tot_deduc))+(a.tot_tax+b.tot_tax)+a.tot_examt
		+a.taxamt+(a.tot_add+b.tot_add)+(a.tot_nontax+b.tot_nontax)-(a.tot_fdisc+abs(b.tot_fdisc))
,srno='d' ,ac.vend_type
from stmain a
Inner Join #ptstax2 b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
Inner Join ac_mast ac on (ac.ac_id=a.ac_id)
where  a.tax_name<>' ' and a.date between @sdate and @edate and net_amt>=+CAST(@SAMT AS VARCHAR(25)) and a.u_imporm = 'Purchase Return'



alter table #ptstax add duty_add bit
Update #ptstax set duty_add=Case when Net_amt=Net_amt2 then 1 else 0 End


--select * from #ptstax order by yearr,mon,tax_name,srno,date
SELECT * FROM #PTSTAX  WHERE net_amt>=+CAST(@SAMT AS VARCHAR(25)) ORDER BY YEARR,MON,SRNO,DATE

Drop table #ptstax
Drop table #ptstax2
--SUM(case when (srno in (''a'') or srno in(''c'')) then net_amt else -net_amt end) >


