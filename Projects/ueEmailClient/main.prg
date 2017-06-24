*:*****************************************************************************
*:        Program: Main
*:         System: Udyog Software
*:         Author: Satish Pal
*:  Last modified: 15/11/2013
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
	LOCAL _VerValidErr,_VerRetVal,_CurrVerVal
	_VerValidErr = ""
	_VerRetVal  = 'NO'
_CurrVerVal='10.0.0.0' 

oWSHELL = CREATEOBJECT("WScript.Shell")
IF VARTYPE(oWSHELL) <> "O"
	MESSAGEBOX("WScript.Shell Object Creation Error...",16,VuMess)
	RETURN .F.
ENDIF

tcSqlstr = IIF(EMPTY(tcSqlstr),"/* Auto Mail */",tcSqlstr)

tcCompId = Company.CompId
tcCompdb = Company.Dbname

*!*	lcFileStr = ""
*!*	IF !EMPTY(tcSqlstr)
*!*		lcFileStr = ALLTRIM(FULLPATH(CURDIR()))
*!*		lcFileStr = '"'+lcFileStr+"USIL_"+SYS(3)+".Sql"+'"'
*!*		STRTOFILE(tcSqlstr,lcFileStr,0)
*!*	ENDIF

&& Commented by Shrikant S. on 21/09/2015 for Bug-26664		&& Start
***** Added by Sachin N. S. on 25/02/2014 for Bug-20211 -- Start
*!*	If Vartype(oGlblPrdFeat)="O"
*!*		If oGlblPrdFeat.UdChkProd('autoemail')== .f.
*!*			Messagebox("Auto-email module is activated. Please select the Auto-email module while company creation.",16,VuMess)
*!*			Return .F.
*!*		Endif
*!*	Endif
***** Added by Sachin N. S. on 25/02/2014 for Bug-20211 -- End
&& Commented by Shrikant S. on 21/09/2015 for Bug-26664		&& End

SqlConObj = NEWOBJECT('SqlConnUdObj','SqlConnection',xapps)
mvu_user1 = SqlConObj.dec(SqlConObj.ondecrypt(mvu_user))
mvu_pass1 = SqlConObj.dec(SqlConObj.ondecrypt(mvu_pass))
tcCompNm=Company.co_name  
vicopath=STRTRAN(icopath,' ','<*#*>') 
pApplCaption=STRTRAN(vumess,' ','<*#*>')
_ShellExec ="eMailClient.exe "+' "'+TRANSFORM(tcCompId)+'" "NO" "NO" "'+ALLTRIM(tcCompdb)+'" "'+vicopath+'" '+ALLTRIM(pApplCaption)+" "+ALLTRIM(pApplName)+" "+ALLTRIM(STR(pApplId))+" "+ALLTRIM(pApplCode)+" "

oWSHELL.EXEC(_ShellExec)
SqlConObj = NULL
mvu_user1 = NULL
mvu_pass1 = NULL
RELEASE SqlConObj,mvu_user1,mvu_pass1
