PARAMETERS pRange
IF PARAMETERS()<1
	=MESSAGEBOX('Please Pass Valid Parameters',0,"")
	RETURN .T.
ENDIF

Create Cursor FileList (cAppName c(50),CommStr c(250))
Create Cursor currVers (fVersion c(250))
Create Cursor cVers (cAppName c(50),cVersion c(15),rVersion c(15))

cDirName=JUSTPATH(SYS(16,0))
getFilelist(cDirName,ADDBS(cDirName)+'*.app','FileList')

SCAN
	IF RIGHT(UPPER(ALLTRIM(cAppName)),3)='APP'
		REPLACE CommStr WITH "Do GetFileVersion in "+ADDBS(cDirName)+ALLTRIM(cAppName)+" with 'currVers'"
	ELSE
		DELETE
	ENDIF
ENDSCAN

err=''
xx=''
SCAN
	xx=ALLTRIM(CommStr)
	TRY
		&xx
	CATCH TO err
		SELECT currVers
		APPEND BLANK
		replace fVersion WITH ALLTRIM(FileList.cAppName)+'     '+'No Version'
	ENDTRY
	SELECT FileList
ENDSCAN

IF USED('FileList')
	USE IN  FileList
ENDIF
SELECT currVers
SCAN
	SELECT cVers
	APPEND BLANK
	REPLACE cVers.cAppName WITH ALLTRIM(SUBSTR(currVers.fVersion,1,AT(' ',currVers.fVersion)))
	REPLACE cVers.cVersion WITH ALLTRIM(SUBSTR(currVers.fVersion,AT(' ',currVers.fVersion)+1))
	_verReqError=""
	TRY
		_VerReqNo=AppVerGet(ALLTRIM(cVers.cAppName))
	CATCH TO _verReqError
		_VerReqNo='Not Found'
	ENDTRY
	REPLACE cVers.rVersion WITH _VerReqNo

	SELECT currVers
ENDSCAN

IF USED('currVers')
	USE IN  currVers
ENDIF

SELECT cVers
***//COPY TO d:\filever
GO TOP
Do Form frmAbout WITH 'cVers'
RETURN

****Versioning**** Added By Amrendra On 01/06/2011
LOCAL _VerValidErr,_VerRetVal,_CurrVerVal
_VerValidErr = ""
_VerRetVal  = 'NO'
TRY
	_VerRetVal = AppVerChk('ABOUT',GetFileVersion(),JUSTFNAME(SYS(16)))
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








Function  getFilelist
Parameters cLok,cFilterCond,cStor
*Set Default To '"'+Alltrim(cLok)+'"'
lnFileCount=Adir(arFilelist,cFilterCond)
Select &cStor
For i=1 To lnFileCount
*	Wait Window Addbs(cLok)+arFilelist(i,1) Nowait
	Select &cStor
	Append Blank
	Replace cAppName With arFilelist(i,1)
Next

*>>>***Versioning**** Added By Amrendra On 08/07/2011
FUNCTION GetFileVersion
PARAMETERS lcTable
_CurrVerVal='10.3.0.0' &&[VERSIONNUMBER]
IF !EMPTY(lcTable)
	SELECT(lcTable)
	APPEND BLANK
	replace fVersion WITH JUSTFNAME(SYS(16))+'   '+_CurrVerVal
ENDIF
RETURN _CurrVerVal
*<<<***Versioning**** Added By Amrendra On 08/07/2011
