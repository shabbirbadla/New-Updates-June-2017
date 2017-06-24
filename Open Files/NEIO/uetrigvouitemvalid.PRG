&&-->Ipop(Rup)
*-->Checking Allocated entry
_Malias 	= Alias()
_mRecNo	= Recno()
_curvouobj = _Screen.ActiveForm
&&vasant061009
If Type('_curvouobj.mainalias') = 'C'
	If Upper(_curvouobj.mainalias) <> 'MAIN_VW'
		Return
	Endif
Endif
&&vasant061009
*Set DataSession To _curvouobj.DataSessionId	&&vasant071009
If Type('_curvouobj.PcvType') = 'C'
	If _Screen.ActiveForm.voupage.page1.GrdItem.Column1.text1.Value<>_Screen.ActiveForm.voupage.page1.GrdItem.Column1.text1.Tag
*!*			If  (([vuexc] $ vchkprod) Or ([vuinv] $ vchkprod)) And Type('_curvouobj.PCVTYPE')='C' &&Check Existing Records
		If  (([vuexc] $ vchkprod) Or ([vuinv] $ vchkprod)) AND !([vutex] $ vchkprod) And Type('_curvouobj.PCVTYPE')='C' &&Check Existing Records		&& Changed By Sachin N. S. on 13/01/2011 for Visual Udyog 10.0
			etsql_str  = "select top 1 entry_ty from projectitref Where aTran_cd = ?Main_vw.Tran_cd And aEntry_ty = ?Main_vw.Entry_ty and aitserial=?item_vw.itserial"
			etsql_str1 = " union select top 1 aentry_ty from projectitref Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty and itserial=?item_vw.itserial"
			etsql_con1 = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,etsql_str+etsql_str1,[_chkbom],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
			If Used('_chkbom')
				If Reccount()>0
					Select _chkbom
					=Messagebox("Entry Passed Against /"+_chkbom.entry_ty+". Item Could not be Changed",16,vumess)
					_Screen.ActiveForm.voupage.page1.GrdItem.Column1.text1.Value=_Screen.ActiveForm.voupage.page1.GrdItem.Column1.text1.Tag
					Use In _chkbom
					Return .F.
				Endif
				Use In _chkbom
			Endif
		Endif
	Endif
Endif

******** Added By Sachin N. S. on 15/07/2011 for Batchwise/Serialize Inventory ******** Start
If _Screen.ActiveForm.voupage.page1.GrdItem.Column1.text1.Value<>_Screen.ActiveForm.voupage.page1.GrdItem.Column1.text1.Tag
	If _curvouobj.itempage And Vartype(_curvouobj._BatchSerialStk)='O'
		etsql_con=_curvouobj._BatchSerialStk._ueTrigVouItemValid()
		Return Iif(etsql_con>0,.T.,.F.)
	Endif
Endif
******** Added By Sachin N. S. on 15/07/2011 for Batchwise/Serialize Inventory ******** End

If !Empty(_Malias)
	Select &_Malias
Endif
If Betw(_mRecNo,1,Reccount())
	Go _mRecNo
Endif
*<--Checking Allocated entry
&&<--Ipop(Rup)
