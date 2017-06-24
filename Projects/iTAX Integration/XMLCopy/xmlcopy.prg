PARAMETERS pCopyAll AS STRING,mCompId AS INTEGER,cThisXmlOnly AS STRING
cThisXmlOnly = IIF(VARTYPE(cThisXmlOnly)<>"C","",cThisXmlOnly)
pCopyAll = IIF(pCopyAll="N","S",pCopyAll)
mCompId = IIF(VARTYPE(mCompId)<>"N",0,mCompId)

&& Validation [Start]
DO CASE
CASE VARTYPE(VuMess) <> [C]
	_SCREEN.VISIBLE = .F.
	MESSAGEBOX("Internal Application Are Not Execute Out-Side ...",16,[])
	QUIT
	RETURN .F.
CASE PCOUNT() = 0
	MESSAGEBOX('Please Pass Valid Parameters',16,VuMess)
	RETURN .F.
ENDCASE
&& Validation [End]


DO CASE
CASE pCopyAll = "S"								&& Single Company
	DO CASE
	CASE VARTYPE(Company) <> 'O'
		MESSAGEBOX('Company Object Not Found',16,VuMess)
		RETURN .F.
	CASE EMPTY(mCompId) OR ISNULL(mCompId)
		MESSAGEBOX('Please Pass Company Code',16,VuMess)
		RETURN .F.
	ENDCASE
	=CallForm(Company.CompId,pCopyAll,.F.,cThisXmlOnly)
CASE pCopyAll = "BS"							&& Both Download & Upload for Single Company.
	DO CASE
	CASE VARTYPE(Company) <> 'O'
		MESSAGEBOX('Company Object Not Found',16,VuMess)
		RETURN .F.
	CASE EMPTY(mCompId) OR ISNULL(mCompId)
		MESSAGEBOX('Please Pass Company Code',16,VuMess)
		RETURN .F.
	ENDCASE
	=CallForm(Company.CompId,[B],.T.,cThisXmlOnly)
CASE INLIST(pCopyAll,"D","U","B")				&& Multi Company
	llReturn = Make_Std_objects('C',mCompId)
	IF !llReturn
		MESSAGEBOX("Standard Object Cursor Creation Error...",16,VuMess)
		RETURN .F.
	ENDIF
	SELECT Cur_Co_Mast_Vw
	SCAN
		llReturn = Make_Std_objects('O')
		IF !llReturn
			LOOP
		ENDIF
		=CallForm(Company.CompId,pCopyAll,.T.,cThisXmlOnly)
	ENDSCAN
OTHERWISE
	MESSAGEBOX("Please Pass Valid Parameters",64,VuMess)
	RETURN .F.
ENDCASE






FUNCTION CallForm
LPARAMETERS tnCompId AS INTEGER,tcUD AS STRING,tlIsshedule AS Logical,cThisXmlOnly AS STRING
DO CASE
CASE INLIST(tcUD,"S","D")			&& Download
	LOCAL objXml
	objXml = CREATEOBJECT("MSXML2.DOMDocument")
	IF VARTYPE(objXml) <> "O"
		MESSAGEBOX("XML Object creation error",16,VuMess)
		RETURN .F.
	ENDIF
	objXml = NULL
	DO FORM frmDownload WITH tnCompId,tlIsshedule,cThisXmlOnly
CASE INLIST(tcUD,"U")				&& Upload
	DO xmlupload.EXE WITH "S",tnCompId,.T.,cThisXmlOnly
CASE INLIST(tcUD,"B")				&& Download And Upload
	IF CallForm(tnCompId,"D",tlIsshedule,cThisXmlOnly)
		WAIT WINDOW "" TIMEOUT .40
		=CallForm(tnCompId,"U",.T.,cThisXmlOnly)
	ENDIF
ENDCASE
RETURN .T.
ENDFUNC




FUNCTION Make_Std_objects
LPARAMETERS lcType AS STRING,mCompId AS INTEGER
nHandle=0
sqlconobj=NEWOBJECT('sqlconnudobj',"sqlconnection",xapps)
IF VARTYPE(sqlconobj) <> 'O'
	RETURN .F.
ENDIF

DO CASE
CASE lcType = "C"					&& Company & Coaddtional Cursor Creation
	lcSqlstr = "SELECT b.* FROM Vudyog..Co_Mast b,Vudyog..mainset a WHERE a.CompId = b.CompId"
	IF mCompId <> 0					&& Raghu171109
		lcSqlstr = lcSqlstr+" And b.CompId = "+TRANSFORM(mCompId)
	ENDIF							&& Raghu171109
	nRetval = sqlconobj.dataconn([EXE],[Vudyog],lcSqlstr,"Cur_Co_Mast_Vw","nHandle",_SCREEN.DATASESSIONID)
	IF nRetval<0
		RETURN .F.
	ENDIF
	SELECT Cur_Co_Mast_Vw
	lcSqlstr = "SELECT b.CompId,b.DbName,'SELECT * FROM '+RTrim(b.Dbname)+'..Manufact' as Sqlstr FROM Vudyog..Co_Mast b,Vudyog..mainset a WHERE a.CompId = b.CompId"
	IF mCompId <> 0					&& Raghu171109
		lcSqlstr = lcSqlstr+" And b.CompId = "+TRANSFORM(mCompId)
	ENDIF							&& Raghu171109
	nRetval = sqlconobj.dataconn([EXE],[Vudyog],lcSqlstr,"Manufact_Str_Vw","nHandle",_SCREEN.DATASESSIONID)
	IF nRetval<0
		RETURN .F.
	ENDIF
	=sqlconobj.SqlConnClose("nHandle")
CASE lcType = "O"					&& Company & Coaddtional Cursor Creation
	IF ! USED("Cur_Co_Mast_Vw")
		RETURN .F.
	ENDIF
	IF ! USED("Manufact_Str_Vw")
		RETURN .F.
	ENDIF
	SELECT Cur_Co_Mast_Vw
	SCATTER MEMO NAME Company

	SELECT Manufact_Str_Vw
	LOCATE FOR CompId = Company.CompId
	IF ! FOUND()
		RETURN .F.
	ENDIF

	SELECT Manufact_Str_Vw
	lcSqlstr = ALLTRIM(Manufact_Str_Vw.SQLSTR)
	nRetval = sqlconobj.dataconn([EXE],[Vudyog],lcSqlstr,"Manufact_Vw","nHandle",_SCREEN.DATASESSIONID)
	IF nRetval<0
		RETURN .F.
	ENDIF
	=sqlconobj.SqlConnClose("nHandle")
	SELECT Manufact_Vw
	SCATTER MEMO NAME Coadditional
ENDCASE
RETURN .T.
ENDFUNC
