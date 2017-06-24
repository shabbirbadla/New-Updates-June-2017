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
If ([vuexc] $ vchkprod) And Inlist(_Screen.ActiveForm.pcvtype,"SR","ST") And Upper(_Screen.ActiveForm.wtable1)="ITEM_VW" 	&& 07/02/2011 Shrikant S.
	If(_Screen.ActiveForm.addmode )
		If Type('_Screen.ActiveForm.txtu_pageno')='O'
			ms='_Screen.ActiveForm.txtu_pageno.Valid()'
			&ms
		Endif
	Endif
Endif
*******	Added By Shrikant S. on 07/02/2011 for TKT-5454 ******* End
