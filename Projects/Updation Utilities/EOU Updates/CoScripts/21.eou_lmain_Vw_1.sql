
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create View [dbo].[Eou_LMain_vw] as
Select a.Tran_Cd,a.entry_ty,a.Date,a.Doc_no,a.Inv_No,a.L_Yn,
	a.Ac_Id,a.[Rule],a.Dept,a.[Inv_Sr],a.[Cate],a.u_beno,a.U_pinvdt FROM PTMAIN a
UNION 
Select a.Tran_Cd,a.entry_ty,a.Date,a.Doc_no,a.Inv_No,a.L_Yn,
	a.Ac_Id,a.[Rule],a.Dept,a.[Inv_Sr],a.[Cate],a.u_beno,a.U_pinvdt FROM IRMAIN a
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

