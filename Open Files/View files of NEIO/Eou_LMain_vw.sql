If Exists (Select [Name] From SysObjects Where xType='V' and [Name]='Eou_LMain_vw')
Begin
	Drop View Eou_LMain_vw
End
Go
Create VIEW [dbo].[Eou_LMain_vw]
AS
SELECT     Tran_cd, entry_ty, date, doc_no, inv_no, l_yn, Ac_id, [rule], dept, inv_sr, cate, u_pinvno AS u_beno, u_pinvdt
FROM         dbo.PTMAIN
UNION
SELECT     Tran_cd, entry_ty, date, doc_no, inv_no, l_yn, Ac_id, [rule], dept, inv_sr, cate, U_Pinvno AS u_beno, U_Pinvdt
FROM         dbo.IRMAIN
UNION
SELECT     Tran_cd, entry_ty, date, doc_no, inv_no, l_yn, Ac_id, [rule], dept, inv_sr, cate, U_Pinvno AS u_beno, U_Pinvdt
FROM         dbo.ARMAIN
UNION
SELECT     Tran_cd, entry_ty, date, doc_no, inv_no, l_yn, Ac_id, [rule], dept, inv_sr, cate, U_Pinvno AS u_beno, u_pinvdt
FROM         dbo.OSMAIN
UNION
SELECT     Tran_cd, entry_ty, date, doc_no, inv_no, l_yn, Ac_id, [rule], dept, inv_sr, cate, U_Pinvno AS u_beno, u_pinvdt
FROM         dbo.SRMAIN			--Added srmain table for Bug-6416 by Shrikant S. on 05/10/2012

