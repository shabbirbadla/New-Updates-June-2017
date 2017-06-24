
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create VIEW [dbo].[Eou_LItem_vw]
AS
SELECT     Tran_cd, entry_ty, date, It_code, itserial, inv_no, item_no, inv_sr, qty, rate, re_qty, u_asseamt, examt, u_acamt, u_acamt1, u_cessamt, u_examt, 
                      u_hacamt1, u_hcessamt, u_impduty, u_basduty, u_acduty, u_acduty1, u_cessper, u_basduty1, u_hacper, u_hcessper, u_impper,u_hcesamt
FROM         dbo.PTITEM AS a
UNION
SELECT     Tran_cd, entry_ty, date, It_code, itserial, inv_no, item_no, inv_sr, qty, rate, re_qty, u_asseamt, examt, u_acamt, u_acamt1, u_cessamt, U_EXAMT, 
                      u_hacamt1, u_hcessamt, u_impduty, u_basduty, u_acduty, u_acduty1, u_cessper, u_basduty1, u_hacper, u_hcessper, u_impper,u_hcesamt
FROM         dbo.IRITEM AS a
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

