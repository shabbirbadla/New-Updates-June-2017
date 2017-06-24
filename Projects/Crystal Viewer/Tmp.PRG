LPARAMETERS tcCry1,tcSql1,tcBsql,tnSql

IF PCOUNT() < 2 OR TYPE("tcCry1") <> "C" OR TYPE("tcSql1") <> "C"
	MESSAGEBOX("Pass valid parameters...",0+64,Vumess)
	RETURN .F.
ENDIF

LOCAL moCrvobj
moCrvobj = CREATEOBJECT("MainCrvclass")
moCrvobj.SplitStringParameters(tcBsql)

DO FORM frmcrystalreport.scx WITH tcCry1,tcSql1,moCrvobj,tnSql


DEFINE CLASS MainCrvclass AS CUSTOM
	tceSql1 = .F.
	tceSql2 = .F.
	tceSql3 = .F.
	tceSql4 = .F.

	FUNCTION SplitStringParameters
	LPARAMETERS tcSql
	LOCAL lnSql,i
	IF ! EMPTY(tcSql)
		xLen = LEN(ALLTRIM(tcSql))
		tcSql = IIF(RIGHT(ALLTRIM(tcSql),1)=':',LEFT(ALLT(tcSql),xLen-1),ALLTRIM(tcSql))
		tcSql = "<<"+STRTRAN(tcSql,":",">><<")+">>"
		lnSql = OCCUR("<<",tcSql)
		FOR i=1 TO lnSql STEP 1
			x = 'This.tceSql'+ALLT(STR((i)))
			&x = STREXTRACT(tcSql,"<<",">>",i)
			IF i = 4
				EXIT
			ENDIF
		ENDFOR
	ENDIF
*!*		RETURN tcSql
ENDDEFINE

