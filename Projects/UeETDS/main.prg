*:*****************************************************************************
*:        Program: Main
*:         System: Udyog Software
*:         Author: Rupesh
*:  Last modified: 
*:			AIM  : Call ueetds.exe
*:*****************************************************************************
PARAMETERS formnm,tnRitghs

IF VARTYPE(Company) <> "O"
	RETURN .F.
ENDIF

IF PARAMETERS() < 1
	=MESSAGEBOX('Passed Valid Parameters',0,"")
	RETURN .T.
ENDIF

****Versioning**** Added By Amrendra On 30/06/2011
	LOCAL _VerValidErr,_VerRetVal,_CurrVerVal
	_VerValidErr = ""
	_VerRetVal  = 'NO'
	TRY
		_VerRetVal = AppVerChk('ETDS',GetFileVersion(),JUSTFNAME(SYS(16)))
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

oWSHELL = CREATEOBJECT("WScript.Shell")
IF VARTYPE(oWSHELL) <> "O"
	MESSAGEBOX("WScript.Shell Object Creation Error...",16,VuMess)
	RETURN .F.
ENDIF



tcCompId = Company.CompId
tcCompdb =Company.Dbname
tcCompNm=Company.co_name
SqlConObj = NEWOBJECT('SqlConnUdObj','SqlConnection',xapps)
mvu_user1 = SqlConObj.dec(SqlConObj.ondecrypt(mvu_user))
mvu_pass1 = SqlConObj.dec(SqlConObj.ondecrypt(mvu_pass))

_ShellExec = "etds.exe "+TRANSFORM(tcCompId)+" "+ALLTRIM(tcCompdb)+" "+ALLTRIM(mvu_server)+" "+ALLTRIM(mvu_user1)+" "+ALLTRIM(mvu_pass1)++" "++alltrim(formnm)
*!*	=MESSAGEBOX(_ShellExec)
oWSHELL.EXEC(_ShellExec)
SqlConObj = NULL
mvu_user1 = NULL
mvu_pass1 = NULL
RELEASE SqlConObj,mvu_user1,mvu_pass1

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