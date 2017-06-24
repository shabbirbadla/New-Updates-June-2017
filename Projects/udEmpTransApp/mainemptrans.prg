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
CASE cApp=="ATTANDANCEREADER"
	appNm="udAttendanceIntegration.exe"
CASE cApp=="DAYTODAYMUSTER"
	appNm="udAttendanceApproval.exe"

CASE cApp=="LOANADVANCEDETAILS"
	appNm="udEmpLoanDetails.exe"
CASE cApp=="LOANADVANCEREQUEST"
	appNm="udEmpLoanRequest.exe"

CASE cApp=="MONTHLYMUSTER"
	appNm="udEmpMonthlyMuster.exe"
CASE cApp=="MONTHLYPAYROLL"
	appNm="udEmpMonthlyPayroll.exe"

CASE cApp=="PAYROLLDECLARATION"
	appNm="udEmpPayrollDeclaration.exe"
CASE cApp=="PROCESSMONTH"
	appNm="udEmpProcessingMonth.exe"

CASE cApp=="TDSPROJECTION"
	appNm="udEmpTdsProjection.exe"	

****** Added by Sachin N. S. on 26/06/2014 for Bug-21114 -- Start
CASE cApp=="DAILYHOURMUSTER"
	appNm="udDailyHourWiseMuster.exe"	
****** Added by Sachin N. S. on 26/06/2014 for Bug-21114 -- End

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
