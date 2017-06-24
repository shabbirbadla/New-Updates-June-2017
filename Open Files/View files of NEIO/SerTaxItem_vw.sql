-- =============================================
-- Author:		Ruepesh Prajapati
-- Create date: 
-- Description:	This View is used in Service Tax Reports.
-- Modification Date/By/Reason: 14/11/2011 Shrikant S. for Bug-259 added p4item table and it_code,qty,rate fields
-- Remark: 
-- =============================================
ALTER view [dbo].[SerTaxItem_vw]
as
select entry_ty,tran_cd,ac_id,date,inv_no,serbamt,sercamt,serhamt,serbper,sercper,serhper,gro_amt,itserial,it_code,qty,rate,serbcper,SERBCESS from epitem		--Changed By Shrikant S. on 21/03/2016 for Bug-27769	(Added serbcper,SERBCESS fields)
union all
select entry_ty,tran_cd,ac_id,date,inv_no,serbamt,sercamt,serhamt,serbper,sercper,serhper,gro_amt,itserial,it_code,qty,rate,SERBCPER,SERBCESS from sbitem		--Changed By Shrikant S. on 21/03/2016 for Bug-27769	(Added serbcper,SERBCESS fields)
union all
select entry_ty,tran_cd,ac_id,date,inv_no,serbamt,sercamt,serhamt,serbper,sercper,serhper,gro_amt,itserial,it_code,Qty,rate,SERBCPER,SERBCESS from p4item		--Changed By Shrikant S. on 21/03/2016 for Bug-27769	(Added serbcper,SERBCESS fields)	


GO


