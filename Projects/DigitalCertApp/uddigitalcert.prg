*:*****************************************************************************
*:        Program: Main
*:         System: Udyog Software
*:         Author: Birendra Prasad
*:  Last modified: 
*:			AIM  : UeBasedOnUsages exe
*:*****************************************************************************
PARAMETERS pPdfPath,pDept,pCat,pInv_sr,pEntry_ty,pDate
IF VARTYPE(Company) <> "O"
	RETURN .F.
ENDIF
*!*	IF PARAMETERS() < 1
*!*		=MESSAGEBOX('Pass Valid Parameters',0,"")
*!*		RETURN .T.
*!*	ENDIF

oWSHELL = CREATEOBJECT("WScript.Shell")
IF VARTYPE(oWSHELL) <> "O"
	MESSAGEBOX("WScript.Shell Object Creation Error...",16,VuMess)
	RETURN .F.
ENDIF

DECLARE INTEGER GetCurrentProcessId  IN kernel32
pid=GetCurrentProcessId()
tcCompId = Company.CompId
tcCompdb =Company.Dbname
tcCompNm=Company.co_name
SqlConObj = NEWOBJECT('SqlConnUdObj','SqlConnection',xapps)
mvu_user1 = SqlConObj.dec(SqlConObj.ondecrypt(mvu_user))
mvu_pass1 = SqlConObj.dec(SqlConObj.ondecrypt(mvu_pass))
vicopath=STRTRAN(icopath,' ','<*#*>')
pApplCaption=STRTRAN(vumess,' ','<*#*>')
pDept=STRTRAN(pDept,' ','<*#*>')
pInv_sr=STRTRAN(pInv_sr,' ','<*#*>')
pCat=STRTRAN(pCat,' ','<*#*>')
pDate=TTOD(pDate)

pApplName=justfname(SYS(16))
_ShellExec ="udPdfSignature.exe " +TRANSFORM(pPdfPath)+" "+TRANSFORM(tcCompId)+" "+ALLTRIM(tcCompdb)+" "+ALLTRIM(mvu_server)+" "+ALLTRIM(mvu_user1)+" "+ALLTRIM(mvu_pass1)+" "+ALLTRIM(pDept)+" "+ALLTRIM(pCat)+" "+ALLTRIM(pInv_sr)+" "+ALLTRIM(pEntry_ty)+" "+TRANSFORM(pDate)
_ShellExec = _ShellExec+" "+ALLTRIM(pApplCaption)+" "+ALLTRIM(pApplName)+" "+ALLTRIM(STR(pApplId))  +" "+ALLTRIM(pApplCode)	&& Added by Sachin N. S. on 18/09/2015 for Bug-26664

oWSHELL.EXEC(_ShellExec)
SqlConObj = NULL
mvu_user1 = NULL
mvu_pass1 = NULL
RELEASE SqlConObj,mvu_user1,mvu_pass1

