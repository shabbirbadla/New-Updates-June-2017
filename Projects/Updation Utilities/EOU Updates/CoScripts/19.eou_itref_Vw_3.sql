
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


create View  [dbo].[Eou_Itref_vw]
 as
SELECT     Tran_cd, entry_ty, Itserial, Itref_tran, rentry_ty, Ritserial, rqty
FROM         dbo.SOITREF AS a
UNION
SELECT     Tran_cd, entry_ty, Itserial, Itref_tran, rentry_ty, Ritserial, rqty
FROM         dbo.OBITREF AS a
UNION
SELECT     Tran_cd, entry_ty, Itserial, Itref_tran, rentry_ty, Ritserial, rqty
FROM         dbo.BPITREF AS a
UNION
SELECT     Tran_cd, entry_ty, Itserial, Itref_tran, rentry_ty, Ritserial, rqty
FROM         dbo.IIITREF AS a
UNION
SELECT     Tran_cd, entry_ty, Itserial, Itref_tran, rentry_ty, Ritserial, rqty
FROM         dbo.STITREF AS a
UNION
SELECT     Tran_cd, entry_ty, Itserial, Itref_tran, rentry_ty, Ritserial, rqty
FROM         dbo.IRITREF AS a
UNION
SELECT     Tran_cd, entry_ty, Itserial, Itref_tran, rentry_ty, Ritserial, rqty
FROM         dbo.OPITREF AS a
UNION
SELECT     Tran_cd, entry_ty, Itserial, Itref_tran, rentry_ty, Ritserial, rqty
FROM         dbo.PTITREF AS a
UNION
SELECT     Tran_cd, entry_ty, Itserial, Itref_tran, rentry_ty, Ritserial, rqty
FROM         dbo.SRITREF AS a
UNION
SELECT     Tran_cd, entry_ty, Itserial, Itref_tran, rentry_ty, Ritserial, rqty
FROM         dbo.IPITREF AS a
UNION
SELECT     Tran_cd, entry_ty, Itserial, Itref_tran, rentry_ty, Ritserial, rqty
FROM         dbo.BRITREF AS a
UNION
SELECT     Tran_cd, entry_ty, Itserial, Itref_tran, rentry_ty, Ritserial, rqty
FROM         dbo.ARITREF AS a
UNION
SELECT     Tran_cd, entry_ty, Itserial, Itref_tran, rentry_ty, Ritserial, rqty
FROM         dbo.PRITREF AS a
UNION
SELECT     Tran_cd, entry_ty, Itserial, Itref_tran, rentry_ty, Ritserial, rqty
FROM         dbo.OSITREF AS a
UNION
SELECT     Tran_cd, entry_ty, Itserial, Itref_tran, rentry_ty, Ritserial, rqty
FROM         dbo.CNITREF AS a
UNION
SELECT     Tran_cd, entry_ty, Itserial, Itref_tran, rentry_ty, Ritserial, rqty
FROM         dbo.CPITREF AS a
UNION
SELECT     Tran_cd, entry_ty, Itserial, Itref_tran, rentry_ty, Ritserial, rqty
FROM         dbo.CRITREF AS a
UNION
SELECT     Tran_cd, entry_ty, Itserial, Itref_tran, rentry_ty, Ritserial, rqty
FROM         dbo.DCITREF AS a
UNION
SELECT     Tran_cd, entry_ty, Itserial, Itref_tran, rentry_ty, Ritserial, rqty
FROM         dbo.DNITREF AS a
UNION
SELECT     Tran_cd, entry_ty, Itserial, Itref_tran, rentry_ty, Ritserial, rqty
FROM         dbo.EPITREF AS a
UNION
SELECT     Tran_cd, entry_ty, Itserial, Itref_tran, rentry_ty, Ritserial, rqty
FROM         dbo.EQITREF AS a
UNION
SELECT     Tran_cd, entry_ty, Itserial, Itref_tran, rentry_ty, Ritserial, rqty
FROM         dbo.ESITREF AS a
UNION
SELECT     Tran_cd, entry_ty, Itserial, Itref_tran, rentry_ty, Ritserial, rqty
FROM         dbo.JVITREF AS a
UNION
SELECT     Tran_cd, entry_ty, Itserial, Itref_tran, rentry_ty, Ritserial, rqty
FROM         dbo.PCITREF AS a
UNION
SELECT     Tran_cd, entry_ty, Itserial, Itref_tran, rentry_ty, Ritserial, rqty
FROM         dbo.POITREF AS a
UNION
SELECT     Tran_cd, entry_ty, Itserial, Itref_tran, rentry_ty, Ritserial, rqty
FROM         dbo.SQITREF AS a
UNION
SELECT     Tran_cd, entry_ty, Itserial, Itref_tran, rentry_ty, Ritserial, rqty
FROM         dbo.SSITREF AS a
UNION
SELECT     Tran_cd, entry_ty, Itserial, Itref_tran, rentry_ty, Ritserial, rqty
FROM         dbo.TRITREF AS a
UNION
SELECT     Tran_cd, entry_ty, Itserial, Itref_tran, rentry_ty, Ritserial, rqty
FROM         dbo.ITREF AS a
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

