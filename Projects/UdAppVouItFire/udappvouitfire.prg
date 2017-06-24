
If Inlist(main_vw.entry_ty ,"LR","IL")

	If Empty(_Screen.ActiveForm.txtPartyName.Value)
		Messagebox("Party Name left blank", 64, VuMess)
		_Screen.ActiveForm.txtPartyName.SetFocus
		Return
	Endif
	If Empty(item_vw.Item)
		Messagebox("Item Name left blank", 64, VuMess)
		_Screen.ActiveForm.grditem.column1.text1.SetFocus
		Return
	Endif
	mlbrPickUP=.T.
*mlbrDocNO = Lmain_vw.doc_no

	mlbrtancd=main_vw.tran_cd
	_Screen.ActiveForm.Box(0,0,1000,1000)

	Do Form UEFRM_LJ_ALLOCATION.scx With _Screen.ActiveForm.DataSessionId,_Screen.ActiveForm.addmode,_Screen.ActiveForm.editmode

*_screen.activeform.cls
*-------
Endif

&&added sandeep for bug-4154 on 03/02/2013--->start
If Inlist(main_vw.entry_ty ,"R1")
&&    MESSAGEBOX('III')
	If Empty(_Screen.ActiveForm.txtPartyName.Value)
		Messagebox("Party Name left blank", 64, VuMess)
		_Screen.ActiveForm.txtPartyName.SetFocus
		Return
	Endif
	If Empty(item_vw.Item)
		Messagebox("Item Name left blank", 64, VuMess)
		_Screen.ActiveForm.grditem.column1.text1.SetFocus
		Return
	Endif
	mlbrPickUP=.T.
*mlbrDocNO = Lmain_vw.doc_no
	mlbrtancd=main_vw.tran_cd
	_Screen.ActiveForm.Box(0,0,1000,1000)
	Do Form UEFRM_LJ_ALLOCATION_3.scx With _Screen.ActiveForm.DataSessionId,_Screen.ActiveForm.addmode,_Screen.ActiveForm.editmode
&&	Do Form UEFRM_LJ_ALLOCATION.scx With _Screen.ActiveForm.DataSessionId,_Screen.ActiveForm.addmode,_Screen.ActiveForm.editmode
*_screen.activeform.cls
*-------
Endif
&&added sandeep for bug-4154 on 03/02/2013--->End

***** Added by Sachin N. S. on 02/09/2015 for Bug-26722 -- Start
If Inlist(main_vw.entry_ty ,"RE")

	If Empty(_Screen.ActiveForm.txtPartyName.Value)
		Messagebox("Party Name left blank", 64, VuMess)
		_Screen.ActiveForm.txtPartyName.SetFocus
		Return
	Endif
	If Empty(item_vw.Item)
		Messagebox("Item Name left blank", 64, VuMess)
		_Screen.ActiveForm.grditem.column1.text1.SetFocus
		Return
	Endif
	mlbrPickUP=.T.

	mlbrtancd=main_vw.tran_cd
	_Screen.ActiveForm.Box(0,0,1000,1000)

	Do Form UEFRM_RU_ALLOCATION.scx With _Screen.ActiveForm.DataSessionId,_Screen.ActiveForm.addmode,_Screen.ActiveForm.editmode

Endif
***** Added by Sachin N. S. on 02/09/2015 for Bug-26722 -- End
