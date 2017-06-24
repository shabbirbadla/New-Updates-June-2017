If Exists(Select [Name] from Sysobjects where xType='V' and Id=Object_Id(N'bankreco'))
Begin
	Drop view bankreco
End
Go




SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		
-- Create date: 
-- Description:	
-- Modified By/date/remark 13/06/12 Added by Sandeep S. Bank_nm Column for Bug-4311 dated 22-Jun-12--->Start
-- =============================================

create VIEW [dbo].[bankreco]
AS
SELECT     a.ac_name, a.cl_date, a.entry_ty, a.date, b.inv_no, b.cheq_no, b.party_nm, (CASE WHEN a.amt_ty = 'DR' THEN a.amount ELSE 0 END) AS debit, 
                      (CASE WHEN a.amt_ty = 'CR' THEN a.amount ELSE 0 END) AS credit, a.clause, a.doc_no, a.Tran_cd, 'BP' AS db, a.amt_ty,b.bank_nm
FROM         dbo.BPACDET AS a INNER JOIN
                      dbo.BPMAIN AS b ON a.Tran_cd = b.Tran_cd
UNION
SELECT     a.ac_name, a.cl_date, a.entry_ty, a.date, b.inv_no, b.cheq_no, b.party_nm, (CASE WHEN a.amt_ty = 'DR' THEN a.amount ELSE 0 END) AS debit, 
                      (CASE WHEN a.amt_ty = 'CR' THEN a.amount ELSE 0 END) AS credit, a.clause, a.doc_no, a.Tran_cd, 'BR' AS db, a.amt_ty,b.bank_nm
FROM         dbo.BRACDET AS a INNER JOIN
                      dbo.BRMAIN AS b ON a.Tran_cd = b.Tran_cd
UNION
SELECT     a.ac_name, a.cl_date, a.entry_ty, a.date, b.inv_no, b.cheq_no, b.party_nm, (CASE WHEN a.amt_ty = 'DR' THEN a.amount ELSE 0 END) AS debit, 
                      (CASE WHEN a.amt_ty = 'CR' THEN a.amount ELSE 0 END) AS credit, a.clause, a.doc_no, a.Tran_cd, 'OB' AS db, a.amt_ty,'' as bank_nm
FROM         dbo.OBACDET AS a INNER JOIN
                      dbo.OBMAIN AS b ON a.Tran_cd = b.Tran_cd
UNION
SELECT     a.ac_name, a.cl_date, a.entry_ty, a.date, b.inv_no, b.cheq_no, b.party_nm, (CASE WHEN a.amt_ty = 'DR' THEN a.amount ELSE 0 END) AS debit, 
                      (CASE WHEN a.amt_ty = 'CR' THEN a.amount ELSE 0 END) AS credit, a.clause, a.doc_no, a.Tran_cd, 'JV' AS db, a.amt_ty,'' as bank_nm
FROM         dbo.JVACDET AS a INNER JOIN
                      dbo.JVMAIN AS b ON a.Tran_cd = b.Tran_cd
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

