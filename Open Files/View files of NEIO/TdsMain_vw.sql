If Exists(Select [Name] from Sysobjects where xType='v' and Id=Object_Id(N'TdsMain_vw'))
Begin
	Drop VIEW TdsMain_vw
End
Go

/*-- =============================================
-- Author:		
-- Create date: 
-- Description:	This View is useful for TDS and TCS Entries\Reports.
-- Modification Date\By\Reason: 21/01/2011. Rupesh. Add TCS TKT-5692.
-- Modification Date\By\Reason: 17/02/2011. Prasanth. Add UTN TKT-5692.
-- Modification Date\By\Reason: 26/06/2012. Sandeep 'SERBAMT ,SERCAMT, SERHAMT' Added columns for  bug-4838.
-- Modification Date\By\Reason: 17/06/2013 Shrikant S. for Bug-14220
-- Modification Date\By\Reason: 18/07/2013 Shrikant S. for Bug-17628 for fields  ,TDSRATEACT,NAT_REM,ACK15CA,u_nature
-- Remark:
-- =============================================*/
create view [dbo].[TdsMain_vw] as
select entry_ty,tran_cd,date,date as u_pinvdt,inv_no,inv_no as u_pinvno,ac_id,tdsonamt,tdsamt,scamt,ecamt,hcamt,svc_cate,cheq_no,u_chalno,u_bsrcode as bsrcode,u_chaldt,net_amt,u_cldt,u_arrears,tds_tp,sc_tp,ec_tp,hc_tp,tdspaytype,l_yn,tot_examt,utn
,SERBAMT ,SERCAMT, SERHAMT  ,space(1) as TDSRATEACT,space(1) as NAT_REM,space(1) as ACK15CA,u_nature
from bpmain     
union all
select entry_ty,tran_cd,date,date as u_pinvdt,inv_no,inv_no as u_pinvno,ac_id,tdsonamt,tdsamt,scamt,ecamt,hcamt,svc_cate,cheq_no,u_chalno,u_bsrcode as bsrcode,u_chaldt,net_amt,u_cldt,u_arrears,tds_tp,sc_tp,ec_tp,hc_tp,tdspaytype,l_yn,tot_examt,utn='' 
,SERBAMT ,SERCAMT, SERHAMT  ,space(1) as TDSRATEACT,space(1) as NAT_REM,space(1) as ACK15CA,space(1) as u_nature
from cpmain 
union all
select entry_ty,tran_cd,date,u_pinvdt,inv_no,u_pinvno,ac_id,tdsonamt,tdsamt,scamt,ecamt,hcamt,svc_cate,space(1) as cheq_no,space(1) as u_chalno,'' as bsrcode,getdate() as u_chaldt,net_amt,date as u_cldt,'' as u_arrears ,tds_tp,sc_tp,ec_tp,hc_tp,0 as tdspaytype,l_yn,tot_examt,utn=''
,SERBAMT ,SERCAMT, SERHAMT ,TDSRATEACT,NAT_REM,ACK15CA,space(1) as u_nature
from epmain
union all
select entry_ty,tran_cd,date,date as u_pinvdt,inv_no,inv_no as u_pinvno,ac_id,0 as tdsonamt,tcsamt as tdsamt,stcsamt as scamt,etcsamt as ecamt,htcsamt as hcamt,svc_cate,cheq_no,u_chalno,'' as bsrcode,u_chaldt,net_amt,u_cldt,'' as u_arrears,tds_tp,sc_tp,ec_tp,hc_tp,tdspaytype,l_yn,tot_examt,utn='' 
,SERBAMT ,SERCAMT, SERHAMT  ,space(1) as TDSRATEACT,space(1) as NAT_REM,space(1) as ACK15CA,space(1) as u_nature
from brmain     
union all
select entry_ty,tran_cd,date,date as u_pinvdt,inv_no,inv_no as u_pinvno,ac_id,0 as tdsonamt,tcsamt as tdsamt,stcsamt as scamt,etcsamt as ecamt,htcsamt as hcamt,svc_cate,cheq_no,'' as u_chalno,'' as  bsrcode,'' as u_chaldt,net_amt,date as u_cldt,'' as u_arrears,tds_tp,sc_tp,ec_tp,hc_tp,tdspaytype,l_yn,tot_examt,utn='' 
,SERBAMT ,SERCAMT, SERHAMT  ,space(1) as TDSRATEACT,space(1) as NAT_REM,space(1) as ACK15CA,space(1) as u_nature
from crmain 
union all
select entry_ty,tran_cd,date,'' as u_pinvdt,inv_no,'' as u_pinvno,ac_id,tdsonamt=gro_amt+tot_add+tot_tax,tcsamt as tdsamt,stcsamt as scamt,etcsamt as ecamt,htcsamt as hcamt,svc_cate,space(1) as cheq_no,space(1) as u_chalno,'' as bsrcode,getdate() as u_chaldt,net_amt,date as u_cldt,'' as u_arrears ,tds_tp,sc_tp,ec_tp,hc_tp,0 as tdspaytype,l_yn,tot_examt,utn=''
,SERBAMT ,SERCAMT, SERHAMT  ,space(1) as TDSRATEACT,space(1) as NAT_REM,space(1) as ACK15CA,space(1) as u_nature
from stmain
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

