*:*****************************************************************************
*:        Program: Main
*:         System: Udyog Software
*:         Author: RAGHU
*:  Last modified: 04/11/2008
*:			AIM  : Call Sql-editor exe
*:*****************************************************************************
Parameters tcSqlstr,tnRitghs
*!*	tcCompid	 : Company Id 	{?Company.CompId}
*!*	tcCompdb	 : Database 	{?Company.dbname}
*!*	tnRights	 : User Rights

If Vartype(Company) <> "O"
	Return .F.
Endif

If Parameters() < 2
	=Messagebox('Passed Valid Parameters',0,"")
	Return .T.
Endif
Local _VerValidErr,_VerRetVal,_CurrVerVal
_VerValidErr = ""
_VerRetVal  = 'NO'
_CurrVerVal='10.0.0.0'

oWSHELL = Createobject("WScript.Shell")
If Vartype(oWSHELL) <> "O"
	Messagebox("WScript.Shell Object Creation Error...",16,VuMess)
	Return .F.
Endif

tcSqlstr = Iif(Empty(tcSqlstr),"/* Auto Mail */",tcSqlstr)

tcCompId = Company.CompId
tcCompdb = Company.Dbname

*!*	lcFileStr = ""
*!*	If !Empty(tcSqlstr)
*!*		lcFileStr = Alltrim(Fullpath(Curdir()))
*!*		lcFileStr = '"'+lcFileStr+"USIL_"+Sys(3)+".Sql"+'"'
*!*	*!*		STRTOFILE(tcSqlstr,lcFileStr,0)
*!*	Endif

***** Added by Sachin N. S. on 25/02/2014 for Bug-20211 -- Start
If Vartype(oGlblPrdFeat)="O"
	If oGlblPrdFeat.UdChkProd('autoemail')== .f.
		Messagebox("Auto-email module is activated. Please select the Auto-email module while company creation.",16,VuMess)
		Return .F.
	Endif
Endif
***** Added by Sachin N. S. on 25/02/2014 for Bug-20211 -- End

SqlConObj = Newobject('SqlConnUdObj','SqlConnection',xapps)
mvu_user1 = SqlConObj.dec(SqlConObj.ondecrypt(mvu_user))
mvu_pass1 = SqlConObj.dec(SqlConObj.ondecrypt(mvu_pass))
tcCompNm=Company.co_name
vicopath=Strtran(icopath,' ','<*#*>')
pApplCaption=Strtran(VuMess,' ','<*#*>')
_ShellExec ="eMailClientWizard.exe "+' "'+Transform(tcCompId)+'" "NO" "NO" "'+Alltrim(tcCompdb)+'" "'+Alltrim(vicopath)+'" '+Alltrim(pApplCaption)+" "+Alltrim(pApplName)+" "+Alltrim(Str(pApplId))  +" "+Alltrim(pApplCode)+" "

oWSHELL.Exec(_ShellExec)
SqlConObj = Null
mvu_user1 = Null
mvu_pass1 = Null
Release SqlConObj,mvu_user1,mvu_pass1
