*:*****************************************************************************
*:        Program: MainCrystal.PRG
*:         System: Udyog Software
*:         Author: RND
*:  Last modified: 19/06/2007
*:			AIM  : Crystal Report Viewer
*:*****************************************************************************
LPARAMETERS tcCry1,tcBsql,tnSql
*!*	tcCry1		 :	Crystal Report Name
*!*	tcBsql		 :	Sql-String
*!*	tnSql		 :	(1) Preview with Print Button [Default]
*!*					(2) Preview with out Print Button
*!*					(3) Print with Preview
*!*	Usage		 :	Do Uecrviewer With "Acmast.rpt","SELECT AC_NAME,[GROUP] FROM Ac_Mast",1
*:*****************************************************************************
IF VARTYPE(VuMess) <> [C]
	_SCREEN.VISIBLE = .F.
	MESSAGEBOX("Internal Application Are Not Execute Out-Side ...",16)
	QUIT
ENDIF

IF PCOUNT() < 2 OR TYPE("tcCry1") <> "C" OR TYPE("tcBsql") <> "C"
	MESSAGEBOX("Pass valid parameters...",0+64,VuMess)
	RETURN .F.
ELSE
	SET DATASESSION TO _SCREEN.ACTIVEFORM.DATASESSIONID
ENDIF

IF EMPTY(tcBsql)
	MESSAGEBOX('Please Pass Sql-String...',64,VuMess)
	RETURN .F.
ENDIF

IF VARTYPE(oCrystalRuntimeApplication) <> 'O'
	PUBLIC oCrystalRuntimeApplication
	oCrystalRuntimeApplication = CREATEOBJECT("CrystalRuntime.Application")
ENDIF

LOCAL moCrvobj
moCrvobj = CREATEOBJECT("MainCrvclass")
moCrvobj.SplitStringParameters(IIF(VARTYPE(tcBsql)<>'C',"",tcBsql))
tnSql = IIF(VARTYPE(tnSql)<>'N',1,tnSql)

DO FORM frmcrystalreport.scx WITH tcCry1,moCrvobj,tnSql

DEFINE CLASS MainCrvclass AS CUSTOM
	TceSql1 = .F.
	tceSql2 = .F.
	tceSql3 = .F.
	tceSql4 = .F.
	tceSql5 = .F.

	FUNCTION SplitStringParameters
	LPARAMETERS tcSql
	LOCAL lnSql,i,x
	IF ! EMPTY(tcSql)
		xLen = LEN(ALLTRIM(tcSql))
		tcSql = IIF(RIGHT(ALLTRIM(tcSql),1)=':',LEFT(ALLT(tcSql),xLen-1),ALLTRIM(tcSql))
		tcSql = "<<"+STRTRAN(tcSql,":",">><<")+">>"
		lnSql = OCCUR("<<",tcSql)
		FOR i=1 TO lnSql STEP 1
			x = 'This.tceSql'+ALLT(STR((i)))
			&x = STREXTRACT(tcSql,"<<",">>",i)
			IF i = 5
				EXIT
			ENDIF
		ENDFOR
	ENDIF
	
ENDDEFINE
