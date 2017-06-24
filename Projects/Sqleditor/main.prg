*:*****************************************************************************
*:        Program: Main
*:         System: Udyog Software
*:         Author: RAGHU
*:  Last modified: 04/11/2008
*:			AIM  : Call Sql-editor exe
*:*****************************************************************************
PARAMETERS tcSqlstr,tnRitghs
*!*	tcCompid	 : Company Id 	{?Company.CompId}
*!*	tcCompdb	 : Database 	{?Company.dbname}
*!*	tnRights	 : User Rights

IF VARTYPE(Company) <> "O"
	RETURN .F.
ENDIF

IF PARAMETERS() < 2
	=MESSAGEBOX('Passed Valid Parameters',0,"")
	RETURN .T.
ENDIF
****Versioning**** && Added By Amrendra for TKT 8121 on 13-06-2011
	LOCAL _VerValidErr,_VerRetVal,_CurrVerVal
	_VerValidErr = ""
	_VerRetVal  = 'NO'
_CurrVerVal='10.0.0.0' &&[VERSIONNUMBER]
	TRY
		_VerRetVal = AppVerChk('SQLEDITOR',_CurrVerVal,JUSTFNAME(SYS(16)))
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

tcSqlstr = IIF(EMPTY(tcSqlstr),"/* SQL Editor */",tcSqlstr)

tcCompId = Company.CompId
tcCompdb = Company.Dbname

lcFileStr = ""
IF !EMPTY(tcSqlstr)
	lcFileStr = ALLTRIM(FULLPATH(CURDIR()))
	lcFileStr = '"'+lcFileStr+"USIL_"+SYS(3)+".Sql"+'"'
	STRTOFILE(tcSqlstr,lcFileStr,0)
ENDIF

SqlConObj = NEWOBJECT('SqlConnUdObj','SqlConnection',xapps)
mvu_user1 = SqlConObj.dec(SqlConObj.ondecrypt(mvu_user))
mvu_pass1 = SqlConObj.dec(SqlConObj.ondecrypt(mvu_pass))

&& Added by Archana K. on 17/07/13 for Bug-17525 Start
tcCompNm=Company.co_name  
vicopath=STRTRAN(icopath,' ','<*#*>') 
pApplCaption=STRTRAN(vumess,' ','<*#*>')
&& Added by Archana K. on 17/07/13 for Bug-17525 End
*!*	_ShellExec = "SQLEditor.exe "+TRANSFORM(tcCompId)+" "+ALLTRIM(tcCompdb)+" "+ALLTRIM(lcFileStr)+" "+ALLTRIM(mvu_server)+" "+ALLTRIM(mvu_user1)+" "+ALLTRIM(mvu_pass1) && Commented by Archana K. on 17/07/13 for Bug-17525 
_ShellExec ="SQLEditor.exe "+" "+TRANSFORM(tcCompId)+" "+ALLTRIM(tcCompdb)+" "+ALLTRIM(lcFileStr)+" "+ALLTRIM(mvu_server)+" "+ALLTRIM(mvu_user1)+" "+ALLTRIM(mvu_pass1) +" "+ALLTRIM(tnRitghs)+" "+ALLTRIM(musername)+" "+ALLTRIM(vicopath)+" "+ALLTRIM(pApplCaption)+" "+ALLTRIM(pApplName)+" "+ALLTRIM(STR(pApplId))  +" "+ALLTRIM(pApplCode)+" " && Changed by Archana K. on 17/07/13 for Bug-17525
oWSHELL.EXEC(_ShellExec)
SqlConObj = NULL
mvu_user1 = NULL
mvu_pass1 = NULL
RELEASE SqlConObj,mvu_user1,mvu_pass1
