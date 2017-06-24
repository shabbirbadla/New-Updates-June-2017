*!*	-- =============================================
*!*	-- Create date: 29/09/2010  for TKT-4021
*!*	-- Created By: Shrikant S.
*!*	-- Description:	This trigger will get called after clicking on "DONE" button of additional Info Button. Its calling valid functions for extra data.
*!*	-- Modification Date/By/Reason:
*!*	-- Remark:
*!*	-- =============================================
If Inlist(_Screen.ActiveForm.pcvtype,"RR","RP","GI","GR","HI","HR") And Upper(_Screen.ActiveForm.wtable1)="MAIN_VW"
	If(_Screen.ActiveForm.addmode)
		If Type('_Screen.ActiveForm.txtu_rg23no')='O'
			ms='_Screen.ActiveForm.txtu_rg23no.Valid()'
			&ms
		Endif
		If Type('_Screen.ActiveForm.txtu_rg23cno')='O'
			ms='_Screen.ActiveForm.txtu_rg23cno.Valid()'
			&ms
		Endif
		If Type('_Screen.ActiveForm.txtu_plasr')='O'
			ms='_Screen.ActiveForm.txtu_plasr.Valid()'
			&ms
		Endif
	Endif
Endif

*******	Added By Shrikant S. on 07/02/2011 for TKT-5454 ******* Start
&&Changes has been done by vasant on 17/11/2012 as per Bug 7230 (RG 23 Part 1 no is not generating error).
*!*	If ([vuexc] $ vchkprod) And Inlist(_Screen.ActiveForm.pcvtype,"ST","SR") And Upper(_Screen.ActiveForm.wtable1)="ITEM_VW" 	&& 07/02/2011 Shrikant S. && 07/02/2011 Sandeep Add Entry Type "SR"
*!*		If(_Screen.ActiveForm.addmode )
If ([vuexc] $ vchkprod) And Upper(_Screen.ActiveForm.wtable1)="ITEM_VW" 	&& 07/02/2011 Shrikant S. && 07/02/2011 Sandeep Add Entry Type "SR"
	If(_Screen.ActiveForm.addmode or _Screen.ActiveForm.editmode)
&&Changes has been done by vasant on 17/11/2012 as per Bug 7230 (RG 23 Part 1 no is not generating error).
		If Type('_Screen.ActiveForm.txtu_pageno')='O'
			ms='_Screen.ActiveForm.txtu_pageno.Valid()'
			&ms
		Endif
	Endif
Endif
*******	Added By Shrikant S. on 07/02/2011 for TKT-5454 ******* End

&& Added by Shrikant S. on 28/06/2014 for Bug-23280		&& Start
If Vartype(oglblindfeat)='O'
	If oglblindfeat.udchkind('pharmaind')
		If _Screen.ActiveForm.addmode Or _Screen.ActiveForm.editmode
			If Upper(Alltrim(_Screen.ActiveForm.wtable1))="IT_MAST_VW"
				If Allt(It_mast_vw.batchgen)=="Monthwise" And Empty(It_mast_vw.batmformat)
					Messagebox("Monthwise format is required.",0,vumess)
					Return .F.
				Endif
			Endif
		Endif
	Endif
Endif
&& Added by Shrikant S. on 28/06/2014 for Bug-23280		&& End
&& Added by Sumit on 24/10/2015 for Bug - 27069 & Bug - 27122 Start
IF Inlist(_Screen.ActiveForm.pcvtype,"PT","ST","VR") And Upper(_Screen.ActiveForm.wtable1)="MAIN_VW"
	IF TYPE('_Screen.ActiveForm.txtU_402SRNO') == 'O'
		dup_No("U_402SRNO",_Screen.ActiveForm.txtU_402SRNO.Value,"STKL_VW_MAIN")
	ENDIF
	IF TYPE('_Screen.ActiveForm.txtU_403SRNO') == 'O'
		dup_No("U_403SRNO",_Screen.ActiveForm.txtU_403SRNO.Value,"STKL_VW_MAIN")
	ENDIF
	IF TYPE('_Screen.ActiveForm.txtSERVTXSRNO') == 'O' AND !EMPTY(_Screen.ActiveForm.txtSERVTXSRNO.Value)
		Dup_ServTxSrNo("SERVTXSRNO",_Screen.ActiveForm.txtSERVTXSRNO.Value)
	ENDIF
ENDIF
&& Added by Sumit on 24/10/2015 for Bug - 27069 & Bug - 27122 End