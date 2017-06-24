****** Added By Sachin N. S. on 30/09/2011 for TKT-9711 ****** Start
_oForm = _Screen.ActiveForm
If ([vuexc] $ vchkprod)
	If Uppe(Allt(wTable))=Upper([Main_Vw])
		Select lother
		_nrecno=Iif(!Eof(),Recno(),0)
		Locate For Upper(Alltrim(fld_nm))='EXMCLEARTY'
		If Found()
			sq1= "SELECT ExMClearTy FROM [Rules] where [rule] = ?Main_vw.Rule "
			nRetval = _oForm.sqlconobj.dataconn([EXE],company.dbname,sq1,"_trans","thisform.nHandle",_oForm.DataSessionId)
			If nRetval<0
				Return .F.
			Endif
			nRetval = _oForm.sqlconobj.sqlconnclose("thisform.nHandle")
			If nRetval<0
				Return .F.
			Endif

			Replace filtcond With _Trans.ExMClearTy In lother
			_tblnm = Iif('LMC' $ Upper(Alltrim(lother.tbl_nm)),'LMC_VW','MAIN_VW')
			If Type(_tblnm+'.ExMClearTy')='C'
				If Empty(&_tblnm..ExMClearTy)
					Replace ExMClearTy With Left(_Trans.ExMClearTy,At(',',_Trans.ExMClearTy)-1) In (_tblnm)
				Endif
			Endif
		Endif
		Select lother
		If _nrecno!=0
			Go _nrecno
		Endif
	Endif
Endif
****** Added By Sachin N. S. on 30/09/2011 for TKT-9711 ****** End
