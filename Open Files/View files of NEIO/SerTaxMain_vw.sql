-- =============================================
-- Author:		Ruepesh Prajapati
-- Create date: 
-- Description:	This View is used in Service Tax Reports.
-- Modification Date/By/Reason: 28/07/2010 Rupesh Prajapati. TKT-794 GTA Add inv_sr column.
-- Modification Date/By/Reason: 23/09/2010 Rupesh Prajapati. TKT-4200 add ,sDocNo,sDocDt.
-- Modification Date/By/Reason: 23/09/2010 Rupesh Prajapati. TKT-7412 add cons_id.
-- Modification Date/By/Reason: 14/11/2011 Shrikant S. for Bug-259 added table P4main.
-- Modification Date/By/Reason: 11/09/2012 Shrikant S. for Bug-5779 for ISD
-- Modification Date/By/Reason: 22/09/2012 Sachin N. S. for Bug-5164 for adding Service Tax Serial No (ServTxSRNo) and Added for STMain
-- Modification Date/By/Reason: 09/02/2015. Pankaj B. Bug 24957 (SER_ADJ,SPACE(1) as SERTYI, serbcess, serbcper filed added)
-- Modification Date/By/Reason: 16/11/2015 Shrikant S. for Bug-27242 (Swachh Bharat Cess)
-- Modification Date/By/Reason: 24/05/2016 Shrikant S. for Bug-28132 (Krishi Kalyan Cess)
-- Remark: 
-- =============================================
ALTER view [dbo].[SerTaxMain_vw]
as
select entry_ty,tran_cd,ac_id,u_pinvno,u_pinvdt,date,u_cldt,inv_no,serbamt,sercamt,serhamt,serty,serbper,sercper,serhper,tdspaytype=0,gro_amt,tot_deduc,tot_tax,bank_nm=space(1),cheq_no=space(1),net_amt,l_yn,serrule,u_arrears=space(1),u_chalno='',u_chaldt='',sertype,sabtper,sabtamt,Narr,inv_sr,sDocNo='',sDocDt='',cons_id=0,'' as ServTxSrNo,SPACE(1) as SER_ADJ,SPACE(1) as SERTYI, serbcess, serbcper,skkcper,skkcamt from epmain		-->Bug23384
union all
select entry_ty,tran_cd,ac_id,u_pinvno,u_pinvdt,date,u_cldt,inv_no,serbamt,sercamt,serhamt,serty,serbper,sercper,serhper,tdspaytype,gro_amt,tot_deduc,tot_tax,bank_nm,cheq_no,net_amt,l_yn,serrule,u_arrears,u_chalno,u_chaldt,sertype,sabtper,sabtamt,Narr,inv_sr,sDocNo,sDocDt,cons_id=0, space(1) as ServTxSrNo,SPACE(1) as SER_ADJ,SPACE(1) as SERTYI, serbcess, serbcper,skkcper,skkcamt from bpmain
union all
select entry_ty,tran_cd,ac_id,u_pinvno,u_pinvdt,date,u_cldt,inv_no,serbamt,sercamt,serhamt,serty,serbper,sercper,serhper,tdspaytype,gro_amt,tot_deduc,tot_tax,bank_nm,cheq_no,net_amt,l_yn,serrule,u_arrears=space(1),u_chalno='',u_chaldt='',sertype,sabtper,sabtamt,Narr,inv_sr,sDocNo='',sDocDt='',cons_id=0, space(1) as ServTxSrNo,SPACE(1) as SER_ADJ,SPACE(1) as SERTYI, serbcess, serbcper,skkcper,skkcamt from cpmain
union all
select entry_ty,tran_cd,ac_id,u_pinvno,u_pinvdt,date,u_cldt=space(1),inv_no,serbamt,sercamt,serhamt,serty,serbper,sercper,serhper,tdspaytype=0,gro_amt,tot_deduc,tot_tax,bank_nm=space(1),cheq_no=space(1),net_amt,l_yn,serrule,u_arrears=space(1),u_chalno='',u_chaldt='',sertype='',sabtper,sabtamt,Narr,inv_sr,sDocNo='',sDocDt='',cons_id=0, space(1) as ServTxSrNo,SPACE(1) as SER_ADJ,SPACE(1) as SERTYI, serbcess, serbcper,skkcper,skkcamt from sbmain
union all
select entry_ty,tran_cd,ac_id,u_pinvno,u_pinvdt,date,u_cldt=space(1),inv_no,serbamt,sercamt,serhamt,serty,serbper,sercper,serhper,tdspaytype=0,gro_amt,tot_deduc,tot_tax,bank_nm=space(1),cheq_no=space(1),net_amt,l_yn,serrule,u_arrears=space(1),u_chalno='',u_chaldt='',sertype='',sabtper,sabtamt,Narr,inv_sr,sDocNo='',sDocDt='',cons_id=0, space(1) as ServTxSrNo,SPACE(1) as SER_ADJ,SPACE(1) as SERTYI,serbcess=0,serbcper=0,skkcper,skkcamt from sdmain
union all
select entry_ty,tran_cd,ac_id,u_pinvno,u_pinvdt,date,u_cldt,inv_no,serbamt,sercamt,serhamt,serty,serbper,sercper,serhper,tdspaytype,gro_amt,tot_deduc,tot_tax,bank_nm,cheq_no,net_amt,l_yn,serrule,u_arrears=space(1),u_chalno='',u_chaldt='',sertype='',sabtper,sabtamt,Narr,inv_sr,sDocNo='',sDocDt='',cons_id=0, space(1) as ServTxSrNo,SPACE(1) as SER_ADJ,SPACE(1) as SERTYI, serbcess, serbcper,skkcper,skkcamt from brmain
union all
select entry_ty,tran_cd,ac_id,u_pinvno,u_pinvdt,date,u_cldt='',inv_no,serbamt,sercamt,serhamt,serty,serbper,sercper,serhper,tdspaytype,gro_amt,tot_deduc,tot_tax,bank_nm,cheq_no,net_amt,l_yn,serrule,u_arrears=space(1),u_chalno='',u_chaldt='',sertype='',sabtper,sabtamt,Narr,inv_sr,sDocNo='',sDocDt='',cons_id=0, space(1) as ServTxSrNo,SPACE(1) as SER_ADJ,SPACE(1) as SERTYI, serbcess, serbcper,skkcper,skkcamt from crmain
union all
--select entry_ty,tran_cd,ac_id,u_pinvno,u_pinvdt,date,u_cldt='',inv_no,serbamt=0,sercamt=0,serhamt=0,serty=space(1),serbper=0,sercper=0,serhper=0,tdspaytype=0,gro_amt,tot_deduc,tot_tax,bank_nm=space(1),cheq_no,net_amt,l_yn,serrule=space(1),u_arrears,u_chalno='',u_chaldt='',sertype,sabtper=0,sabtamt=0,Narr,inv_sr,sDocNo='',sDocDt='',cons_id from Jvmain		--Commented By Shrikant S. on 11/09/2012 for Bug-5779
select entry_ty,tran_cd,ac_id,u_pinvno,u_pinvdt,date,u_cldt='',inv_no,serbamt,sercamt,serhamt,serty,serbper=0,sercper=0,serhper=0,tdspaytype=0,gro_amt,tot_deduc,tot_tax,bank_nm=space(1),cheq_no,net_amt,l_yn,serrule=space(1),u_arrears,u_chalno='',u_chaldt='',sertype,sabtper=0,sabtamt=0,Narr,inv_sr,sDocNo='',sDocDt='',cons_id, ServTxSrNo,SER_ADJ,SERTYI, serbcess, serbcper,skkcper,skkcamt from Jvmain	--Added By Shrikant S. on 11/09/2012 for Bug-5779
union all
select entry_ty,tran_cd,ac_id,u_pinvno,u_pinvdt,date,u_cldt='',inv_no,serbamt=0,sercamt=0,serhamt=0,serty=space(1),serbper=0,sercper=0,serhper=0,tdspaytype=0,gro_amt,tot_deduc,tot_tax,bank_nm=space(1),cheq_no,net_amt,l_yn,serrule=space(1),u_arrears='',u_chalno='',u_chaldt='',sertype='',sabtper=0,sabtamt=0,Narr,inv_sr,sDocNo='',sDocDt='',cons_id=0, ServTxSrNo,SPACE(1) as SER_ADJ,SPACE(1) as SERTYI, serbcess=0, serbcper=0,skkcper=0,skkcamt=0 from IRmain
union all
select entry_ty,tran_cd,ac_id,u_pinvno,u_pinvdt,date,u_cldt='',inv_no,serbamt=0,sercamt=0,serhamt=0,serty=space(1),serbper=0,sercper=0,serhper=0,tdspaytype=0,gro_amt,tot_deduc,tot_tax,bank_nm=space(1),cheq_no,net_amt,l_yn,serrule=space(1),u_arrears='',u_chalno='',u_chaldt='',sertype='',sabtper=0,sabtamt=0,Narr,inv_sr,sDocNo='',sDocDt='',cons_id=0, space(1) as ServTxSrNo,SPACE(1) as SER_ADJ,SPACE(1) as SERTYI, serbcess=0, serbcper=0,skkcper=0,skkcamt=0 from Obmain
union all
select entry_ty,tran_cd,ac_id,u_pinvno,u_pinvdt,date,u_cldt=Date,inv_no,serbamt=0,sercamt=0,serhamt=0,serty=space(1),serbper=0,sercper=0,serhper=0,tdspaytype=0,gro_amt,tot_deduc,tot_tax,bank_nm=space(1),cheq_no,net_amt,l_yn,serrule=space(1),u_arrears='',u_chalno='',u_chaldt='',sertype='',sabtper=0,sabtamt=0,Narr,inv_sr,sDocNo='',sDocDt='',cons_id=0, ServTxSrNo,SPACE(1) as SER_ADJ,SPACE(1) as SERTYI, serbcess, serbcper=0,skkcper=0,skkcamt=0 from Stmain		 --Changed By Shrikant S. on 21/03/2016 for Bug-27769
union all
select entry_ty,tran_cd,ac_id,u_pinvno,u_pinvdt,date,u_cldt=space(1),inv_no,serbamt,sercamt,serhamt,serty,serbper,sercper,serhper,tdspaytype=0,gro_amt,tot_deduc,tot_tax,bank_nm=space(1),cheq_no=space(1),net_amt,l_yn,serrule,u_arrears=space(1),u_chalno='',u_chaldt='',sertype='',sabtper,sabtamt,Narr,inv_sr,sDocNo='',sDocDt='',cons_id=0, space(1) as ServTxSrNo,SPACE(1) as SER_ADJ,SPACE(1) as SERTYI, serbcess, serbcper,skkcper,skkcamt from P4main




