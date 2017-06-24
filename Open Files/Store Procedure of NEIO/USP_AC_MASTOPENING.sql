set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

ALTER PROCEDURE [dbo].[USP_AC_MASTOPENING]
@FDate SMALLDATETIME
AS
SELECT a.Ac_id,
OpBal = isnull(CASE Amt_Ty WHEN 'DR' THEN Sum(a.Amount)END,0)-isnull(CASE Amt_Ty WHEN 'CR' THEN Sum(a.Amount) END,0)
from lac_vw a,lmain_vw b
where (a.entry_ty = b.entry_ty and a.Tran_cd = b.Tran_cd)
and (a.entry_ty = 'OB' Or b.Date < @FDate)
group by a.Ac_id,a.amt_ty

