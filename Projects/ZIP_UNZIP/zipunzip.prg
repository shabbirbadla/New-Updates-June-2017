PARAMETER WHAT, ZipFileNames, ZipName, vDir, ZipExtractTo, ZipTitle, xtyesNo, PwdProtect
****Versioning**** && Added By Amrendra for TKT 8121 on 13-06-2011
	LOCAL _VerValidErr,_VerRetVal,_CurrVerVal
	_VerValidErr = ""
	_VerRetVal  = 'NO'
	TRY
		_VerRetVal = AppVerChk('ZIPUNZIP',GetFileVersion(),JUSTFNAME(SYS(16)))
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

vformexist=.F.

IF ToFireVoucher()
	vformexist=.T.
	IF VARTYPE(mvu_Auto_object)<>'O'
		IF VARTYPE(_SCREEN.ACTIVEFORM) = 'O'
			_SCREEN.ACTIVEFORM.LOCKSCREEN=.T.
		ENDIF
	ENDIF
ENDIF

DO FORM frmzipunzip WITH WHAT, ZipFileNames, ZipName, vDir, ZipExtractTo, ZipTitle, xtyesNo, PwdProtect

*!*	WAIT WINDOW zIPnAME

IF vformexist
*!*		WAIT WINDOW ZipName+ " END" NOWAIT
	IF VARTYPE(mvu_Auto_object)<>'O'
		IF VARTYPE(_SCREEN.ACTIVEFORM) = 'O'
			_SCREEN.ACTIVEFORM.LOCKSCREEN=.F.
		ENDIF
	ENDIF
ENDIF

RELEASE vformexist
RETURN

PROCEDURE ToFireVoucher
vctr=0
FOR i = 1 TO _SCREEN.FORMCOUNT
	IF UPPE(ALLT(_SCREEN.FORMS(i).CLASS))==[FORM]
		vctr=1
		EXIT FOR
	ENDIF
ENDFOR
RETURN IIF(vctr=1,.T.,.F.)

*>>>***Versioning**** Added By Amrendra On 08/07/2011
FUNCTION GetFileVersion
PARAMETERS lcTable
	_CurrVerVal='10.0.0.0' &&[VERSIONNUMBER]
	IF !EMPTY(lcTable)
		SELECT(lcTable)
		APPEND BLANK 
		replace fVersion WITH JUSTFNAME(SYS(16))+'   '+_CurrVerVal
	ENDIF 
RETURN _CurrVerVal
*<<<***Versioning**** Added By Amrendra On 08/07/2011