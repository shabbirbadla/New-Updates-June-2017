Lparameters tcAction As String,tcType As String,tcMastTrans As String
*!*	tcAction 			: What action should execute
*!*	tcType 				: Transaction / Master
*!*	tcMastTrans 		: Transaction Type(Entry_ty) / Master Code
If ("DBFLOW_BLL" $ Set("Classlib")) = .F.
	Set Classlib To dbflow_bll.vcx Additive
Endif

lcOldAlias = Alias()
If Upper(Alltrim(tcAction)) == "INIT"						&&& One Time Activity
	tcType = Iif(Type("tcType")<>"C","",tcType)
	If !Inlist(tcType,"M","T")
		Messagebox("Type should be"+Chr(13)+"[M] - Masters"+Chr(13)+"[T] - Transactions",64,VuMess)
		Return .F.
	Endif
	If Type("_Screen.ActiveForm") = "O"
		If Type("_Screen.ActiveForm.oDbdataflow") <> "O"
			_Screen.ActiveForm.AddObject("oDbdataflow","dbdataflow")
			_Screen.ActiveForm.oDbdataflow.cMastTrans = tcMastTrans
			_Screen.ActiveForm.oDbdataflow.cType = tcType
		Endif
	Endif
	Return
Endif														&&& One Time Activity

If Upper(Alltrim(tcAction)) == "PROCESS"
	If Type("_Screen.ActiveForm.oDbdataflow") <> "O"
		Return
	Endif
	Do Case
	Case _Screen.ActiveForm.oDbdataflow.cType = "M"			&& Master Updates
		If !_Screen.ActiveForm.oDbdataflow.Masters_updates()
			=resetlastalias(lcOldAlias)
			Return .F.
		Endif
	Case _Screen.ActiveForm.oDbdataflow.cType = "T"			&& Transaction Updates
		_Screen.ActiveForm.oDbdataflow.Inv_sr = Alltrim(Main_vw.Inv_sr)
		_Screen.ActiveForm.oDbdataflow.Entry_tbl = Alltrim(_Screen.ActiveForm.Entry_tbl)
		If Type("_Screen.ActiveForm.LinkHandle") = "U"
			_Screen.ActiveForm.AddProperty("LinkHandle",0)
		ENDIF
		If !_Screen.ActiveForm.oDbdataflow.Transaction_updates()
			=resetlastalias(lcOldAlias)
			Return .F.
		Endif
	Endcase
Endif

Function resetlastalias
	Lparameters lcOldAlias As String
	If !Empty(lcOldAlias)
		If Used(lcOldAlias)
			Select (lcOldAlias)
		Endif
	Endif
Endfunc
