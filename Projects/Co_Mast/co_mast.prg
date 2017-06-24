Parameters What,pRange
** Added By amrendra on 09/06/2011 for TKT-8121 Start
****Versioning**** 
	LOCAL _VerValidErr,_VerRetVal,_CurrVerVal
	_VerValidErr = ""
	_VerRetVal  = 'NO'
_CurrVerVal='10.0.0.0' &&[VERSIONNUMBER]
	TRY
		_VerRetVal = AppVerChk('CO_MAST',_CurrVerVal,JUSTFNAME(SYS(16)))
	CATCH TO _VerValidErr
		_VerRetVal  = 'NO'
	Endtry	
	IF TYPE("_VerRetVal")="L"
		cMsgStr="Version Error occured!"
		cMsgStr=cMsgStr+CHR(13)+"Kindly update latest version of "+GlobalObj.getPropertyval("ProductTitle")
		Messagebox(cMsgStr,64,VuMess)
		Return .F.
	ENDIF
	IF _VerRetVal  = 'NO'
		Return .F.
	Endif
****Versioning****
** Added By amrendra on 09/06/2011 for TKT-8121 End
If Vartype(VuMess) <> [C]
	_Screen.Visible = .F.
	Messagebox("Internal Application Are Not Execute Out-Side ...",16)
	Return .F.
Endif

rval = .F.

If ! "\datepicker." $ Lower(Set("Classlib"))
	Set Classlib To apath+"class\datepicker.vcx" Additive
*!*		Set Classlib To "datepicker.vcx" Additive
Endif

Do Form co_mast With What,pRange Name rval
