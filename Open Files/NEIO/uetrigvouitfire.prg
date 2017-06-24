IF INLIST(main_vw.entry_ty ,"LR","IL")

	IF EMPTY(_SCREEN.ACTIVEFORM.txtPartyName.VALUE)
		MESSAGEBOX("Party Name left blank", 64, VuMess)
		_SCREEN.ACTIVEFORM.txtPartyName.SETFOCUS
		RETURN
	ENDIF
	IF EMPTY(item_vw.ITEM)
		MESSAGEBOX("Item Name left blank", 64, VuMess)
		_SCREEN.ACTIVEFORM.grditem.column1.text1.SETFOCUS
		RETURN
	ENDIF
	mlbrPickUP=.T.
*mlbrDocNO = Lmain_vw.doc_no
	mlbrtancd=main_vw.tran_cd
	_SCREEN.ACTIVEFORM.BOX(0,0,1000,1000)

	DO FORM UEFRM_LJ_ALLOCATION.scx WITH _SCREEN.ACTIVEFORM.DATASESSIONID,_SCREEN.ACTIVEFORM.addmode,_SCREEN.ACTIVEFORM.editmode

*_screen.activeform.cls
*-------
ENDIF
