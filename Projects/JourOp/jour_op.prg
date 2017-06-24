Para lcCurrentVal1,llEnabled1,llCancled,mWhat,dEtype,mDataSession,lladded,lcDbname
****Versioning**** Added By Amrendra On 01/06/2011
	LOCAL _VerValidErr,_VerRetVal,_CurrVerVal
	_VerValidErr = ""
	_VerRetVal  = 'NO'
	TRY
		_VerRetVal = AppVerChk('JOUR_OP',GetFileVersion(),JUSTFNAME(SYS(16)))
	CATCH TO _VerValidErr
		_VerRetVal  = 'NO'
	Endtry	
	IF TYPE("_VerRetVal")="L"
		cMsgStr="Version Error occured!"
		cMsgStr=cMsgStr+CHR(13)+"Kindly update latest version of "+GlobalObj.getPropertyval("ProductTitle")
		Messagebox(cMsgStr,64,VuMess)
		Return lladded
	ENDIF
	IF _VerRetVal  = 'NO'
		Return lladded
	Endif
****Versioning****
*!*	do form jour_op with lcCurrentVal1,llEnabled1,llCancled,mWhat,dEtype,mDataSession to lladded
Do Form frmtranselect With lcCurrentVal1,llEnabled1,llCancled,mWhat,dEtype,mDataSession,lcDbname To lladded
Retur lladded

*>>>***Versioning**** Added By Amrendra On 05/07/2011
FUNCTION GetFileVersion
PARAMETERS lcTable
	_CurrVerVal='10.0.0.0' &&[VERSIONNUMBER]
	IF !EMPTY(lcTable)
		SELECT(lcTable)
		APPEND BLANK 
		replace fVersion WITH JUSTFNAME(SYS(16))+'   '+_CurrVerVal
	ENDIF 
RETURN _CurrVerVal
*<<<***Versioning**** Added By Amrendra On 05/07/2011