Para Flag,pType,pRange
****Versioning**** Added By Amrendra On 01/06/2011
	LOCAL _VerValidErr,_VerRetVal,_CurrVerVal
	_VerValidErr = ""
	_VerRetVal  = 'NO'
_CurrVerVal='10.0.0.0' &&[VERSIONNUMBER]
	TRY
		_VerRetVal = AppVerChk('ITEMMAST',_CurrVerVal,JUSTFNAME(SYS(16)))
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
nPcnt = Pcount()
Rele onItem
Publ onItem
onItem=""
***** Changed By Sachin N. S. on 28/01/2011 for Visual Udyog 10.0 ***** Start
*!*	Do Form itemmas With Flag,pRange
If nPcnt = 2
	Do Form itemmas With Flag,"",pType
Else
	Do Form itemmas With Flag,pType,pRange
Endif
***** Changed By Sachin N. S. on 28/01/2011 for Visual Udyog 10.0 ***** End
Return onItem

