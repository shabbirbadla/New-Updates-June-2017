*:*****************************************************************************
*:        Program: Main
*:         System: Udyog Software
*:         Added By: Rupesh
*:  Last modified: 
*:			AIM  : Call HR and Payroll Related Masters
*:*****************************************************************************
PARAMETERS cApp,pRange
IF VARTYPE(Company) <> "O"
	RETURN .F.
ENDIF
cApp=UPPER(cApp)
oWSHELL = CREATEOBJECT("WScript.Shell")
IF VARTYPE(oWSHELL) <> "O"
	MESSAGEBOX("WScript.Shell Object Creation Error...",16,VuMess)
	RETURN .F.
ENDIF

DECLARE INTEGER GetCurrentProcessId  IN kernel32

tcCompId = Company.CompId
tcCompdb =Company.Dbname
tcCompNm=Company.co_name
SqlConObj = NEWOBJECT('SqlConnUdObj','SqlConnection',xapps)
mvu_user1 = SqlConObj.dec(SqlConObj.ondecrypt(mvu_user))
mvu_pass1 = SqlConObj.dec(SqlConObj.ondecrypt(mvu_pass))
vicopath=STRTRAN(icopath,' ','<*#*>')
pApplCaption=STRTRAN(vumess,' ','<*#*>')
appNm=""

DO case
CASE cApp=="DESIGNATION"
	appNm="udEmpDesigMaster.exe"
CASE cApp=="GRADE"
	appNm="udEmpGradeMaster.exe"
CASE cApp=="PAYYEAR"
	appNm="udEmpPayrollYearMaster.exe"
CASE cApp=="PAYHEAD"
	appNm="udEmpPayHeadMaster.exe"
CASE cApp=="SLAB"
	appNm="udEmpPayHeadSlabMaster.exe"
CASE cApp=="HOLIDAY"
	appNm="UdEmpHolidayMaster.exe"
CASE cApp=="SHIFT"
	appNm="udEmpShiftMaster.exe"
CASE cApp=="WEEKLYHOLIDAY"
	appNm="udEmpWeeklyHoliday.exe"
CASE cApp=="DOCUMENT"
	appNm="udEmpDocMaster.exe"
CASE cApp=="ATTENDANCE"
	appNm="udEmpAttendanceSetting.exe"
CASE cApp=="PAYROLL DECLARATION MASTER"
	appNm="udEmpPayrollDeclarationMaster.exe"
OTHERWISE
	appNm=""		
ENDCASE  

IF !EMPTY(appNm)
	_ShellExec =appNm+" "+TRANSFORM(tcCompId)+" "+ALLTRIM(tcCompdb)+" "+ALLTRIM(mvu_server)+" "+ALLTRIM(mvu_user1)+" "+ALLTRIM(mvu_pass1) +" "+ALLTRIM(pRange)+" "+ALLTRIM(musername)+" "+ALLTRIM(vicopath)+" "+ALLTRIM(pApplCaption)+" "+ALLTRIM(pApplName)+" "+ALLTRIM(STR(pApplId))  +" "+ALLTRIM(pApplCode)+" "
*!*		MESSAGEBOX(_ShellExec)
	oWSHELL.EXEC(_ShellExec)
ELSE
	MESSAGEBOX(cApp+" not found",16,VuMess)
ENDIF 
*oWSHELL.Run(_ShellExec,1,.t.)
SqlConObj = NULL
mvu_user1 = NULL
mvu_pass1 = NULL
RELEASE SqlConObj,mvu_user1,mvu_pass1
