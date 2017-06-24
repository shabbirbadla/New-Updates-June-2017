PARAMETER menuProd,pRange
****Versioning**** Added By Amrendra On 01/06/2011
	LOCAL _VerValidErr,_VerRetVal,_CurrVerVal
	_VerValidErr = ""
	_VerRetVal  = 'NO'
_CurrVerVal='10.0.0.0' &&[VERSIONNUMBER]
	TRY
		_VerRetVal = AppVerChk('EXDTMAST',_CurrVerVal,JUSTFNAME(SYS(16)))
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
*!*	ans = 7
*!*	ans = MESSAGEBOX("Before You Proceed to EXTRA DATA MASTER, All the Users Should EXIT from The Current Company. Proceed ?",36,"Visual Udyog")
*!*	IF ans = 7
*!*		RETURN
*!*	ENDIF
*!*	IF USED("lmain")
*!*		SELE lmain
*!*		USE
*!*	ENDIF
*!*	IF USED("lmain_vw")
*!*		SELE lmain_vw
*!*		USE
*!*	ENDIF
*!*	IF USED("litem")
*!*		SELE litem
*!*		USE
*!*	ENDIF
*!*	IF USED("litem_vw")
*!*		SELE litem_vw
*!*		USE
*!*	ENDIF
*!*	IF USED("lac_det")
*!*		SELE lac_det
*!*		USE
*!*	ENDIF
*!*	IF USED("lac_vw")
*!*		SELE lac_vw
*!*		USE
*!*	ENDIF
*!*	errfile=on("error")
*!*	on error exclfalse=.T.
*!*	PUBL exclfalse

*$*IF !USED('lmain')
*$*	USE lmain ALIAS lmain Share in 0
*$*ENDIF
*$*if !used('litem')
*$*	use litem alias litem Share in 0
*$*endif


*!*	RELE exclfalse
*!*	on error &errfile

*!*	DO FORM EXDTMAST WITH UPPE(ALLT(menuProd))
DO FORM EXDTMAST WITH "",pRange