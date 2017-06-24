IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SerTaxAcDet_vw]') AND type in (N'V', N'VC'))
begin
	DROP view [dbo].[SerTaxAcDet_vw]
end

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ruepesh Prajapati
-- Create date: 
-- Description:	This View is used in Service Tax Reports.
-- Modification Date/By/Reason: 28/07/2010 Rupesh Prajapati. TKT-794 GTA Add inv_sr column.
-- Modification Date/By/Reason: 13/10/2011 Rupesh Prajapati. TKT-9722 add Sales Transaction
-- Modification Date/By/Reason: 14/06/2012 Sandeep Shah. bug-4574 Remove the space of u_cldt column from obacdet table
-- Modification Date/By/Reason: 22/09/2012 Sachin N. S. Bug-5164 -- Added new Service Tax Serial No. (ServTxSrNo)
-- Remark: 
-- =============================================
create view [dbo].[SerTaxAcDet_vw]
as
select entry_ty,tran_cd,ac_id,date,u_cldt=space(1),serty,amount,amt_ty ,acserial, Space(1) as ServTxSrNo from epacdet
union all
select entry_ty,tran_cd,ac_id,date,u_cldt,serty,amount,amt_ty,acserial, Space(1) as ServTxSrNo from bpacdet
union all
select entry_ty,tran_cd,ac_id,date,u_cldt=space(1),serty,amount,amt_ty,acserial, Space(1) as ServTxSrNo from cpacdet
union all
select entry_ty,tran_cd,ac_id,date,u_cldt=space(1),serty,amount,amt_ty,acserial, Space(1) as ServTxSrNo from sbacdet
union all
select entry_ty,tran_cd,ac_id,date,u_cldt=space(1),serty,amount,amt_ty,acserial, Space(1) as ServTxSrNo from sdacdet
union all
select entry_ty,tran_cd,ac_id,date,u_cldt=space(1),serty,amount,amt_ty,acserial, Space(1) as ServTxSrNo from bracdet
union all
select entry_ty,tran_cd,ac_id,date,u_cldt=space(1),serty,amount,amt_ty,acserial, Space(1) as ServTxSrNo from cracdet
union all
select entry_ty,tran_cd,ac_id,date,u_cldt,serty,amount,amt_ty,acserial, ServTxSrNo from JVacdet
union all
select entry_ty,tran_cd,ac_id,date,u_cldt,serty='',amount,amt_ty,acserial, ServTxSrNo from IRacdet
union all
select entry_ty,tran_cd,ac_id,date,u_cldt,serty='',amount,amt_ty,acserial, Space(1) as ServTxSrNo from OBacdet
union all
select entry_ty,tran_cd,ac_id,date,u_cldt=date,serty='',amount,amt_ty,acserial, ServTxSrNo from Stacdet
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

