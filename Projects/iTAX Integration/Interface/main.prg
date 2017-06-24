PARAM pType,lrange

IF VARTYPE(VuMess) <> 'C'
	MESSAGEBOX('Internal Application Not Run Directly...',0+48,[i-TAX])
	QUIT
	RETURN .F.
ENDIF

LOCAL moCrvobj
moIntrFcobj = CREATEOBJECT("MainIntFace")
moIntrFcobj.CheckMe(pType)
DO CASE
CASE pType = "X"					&& Interface
	IF ! moIntrFcobj.lAlowMe
		MESSAGEBOX(moIntrFcobj.lcMessage,64,VuMess)
		RETURN .F.
	ENDIF
	DO FORM frmInterface WITH lrange
CASE pType = "C"					&&
	IF ! moIntrFcobj.lAlowMe
		MESSAGEBOX(moIntrFcobj.lcMessage,64,VuMess)
		RETURN .F.
	ENDIF
	IF ! moIntrFcobj.lAlowMe
		MESSAGEBOX(moIntrFcobj.lcMessage,64,VuMess)
		RETURN .F.
	ENDIF
	DO FORM frmcreate
CASE pType = "M"							&& Field Mapping
	IF ! moIntrFcobj.lAlowMe
		MESSAGEBOX(moIntrFcobj.lcMessage,64,VuMess)
		RETURN .F.
	ENDIF
	DO FORM FrmUIMap WITH lrange
CASE INLIST(pType,"S","NCS")				&& Server Setting
	DO FORM frmsrvset WITH pType,lrange
CASE pType = "D2S"							&& Field Mapping
	DO FORM frmvfp2sql WITH lrange
CASE pType = "ERRORVIEW"					&& Field Mapping
	DO FORM frmLogViewer
OTHERWISE
	MESSAGEBOX("Please Pass valid parameter...",64,VuMess)
	RETURN .F.
ENDCASE

DEFINE CLASS MainIntFace AS CUSTOM
	lAlowMe = .F.
	lcMessage = ""

	PROCEDURE CheckMe
	LPARAMETERS pType AS STRING
	IF ! PEMSTATUS(THIS,"sqlconobj",5)
		THIS.ADDOBJECT("sqlconobj","sqlconnudobj")
	ENDIF
	nhandle = 0
	lcsqlstr = "SELECT Co_Name FROM mainset WHERE compid=?company.compid AND Eccno = ?Coadditional.Eccno"
	mretval = THIS.sqlconobj.dataconn("EXE","vudyog",lcsqlstr,"Mainset_Cur","nhandle",_SCREEN.DATASESSIONID,.F.)
	IF mretval <= 0
		RETURN THIS.lAlowMe
	ENDIF
	THIS.lAlowMe = IIF(RECCOUNT("Mainset_Cur") <> 0,.T.,.F.)
	THIS.sqlconobj.sqlconnclose("nhandle")
	IF THIS.lAlowMe
		lcsqlstr = "SELECT [Name] FROM Sysobjects Where [Name] = 'UI2_Settings' And xType = 'U'"
		mretval = THIS.sqlconobj.dataconn("EXE",company.Dbname,lcsqlstr,"Mainset_Cur","nhandle",_SCREEN.DATASESSIONID,.F.)
		IF mretval <= 0
			THIS.lAlowMe = .F.
			RETURN THIS.lAlowMe
		ENDIF
		THIS.lAlowMe = IIF(RECCOUNT("Mainset_Cur") <> 0,.T.,.F.)
		IF THIS.lAlowMe
			IF pType <> "C"
				lcsqlstr = "Select Distinct 'UI2_'+RTrim(LTrim([Dbfnm])) as Dbfname From UI2_Settings "
				lcsqlstr = lcsqlstr+" Where 'UI2_'+RTrim(LTrim([Dbfnm])) Not In (Select [Name] From Sysobjects Where xType = 'U' and Left([Name],4) = 'UI2_')"
				mretval = THIS.sqlconobj.dataconn("EXE",company.Dbname,lcsqlstr,"Mainset_Cur","nhandle",_SCREEN.DATASESSIONID,.F.)
				IF mretval <= 0
					THIS.lAlowMe = .F.
					RETURN THIS.lAlowMe
				ENDIF
				THIS.lAlowMe = IIF(RECCOUNT("Mainset_Cur") = 0,.T.,.F.)
				IF ! THIS.lAlowMe
					THIS.lcMessage = ALLTRIM(Mainset_Cur.Dbfname)+" Database object not found..."
				ENDIF
			ENDIF
		ELSE
			THIS.lcMessage = "UI2_Setting Database object not found..."
		ENDIF
	ELSE
		THIS.lcMessage = "Please Set Server Setting..."
	ENDIF
&& Raghu061109
	IF !THIS.lAlowMe
		IF INLIST(ALLTRIM(UPPER(pType)),"C","M")				&& For field mapping & Restructuring
			lcsqlstr = "SELECT [Name] FROM Sysobjects WHERE xType = 'U' AND [Name] = 'Interface'"
			mretval = THIS.sqlconobj.dataconn("EXE",ALLTRIM(company.Dbname),lcsqlstr,"Mainset_Cur","nhandle",_SCREEN.DATASESSIONID,.F.)
			IF mretval <= 0
				THIS.lAlowMe = .F.
				RETURN THIS.lAlowMe
			ENDIF
			THIS.lAlowMe = IIF(RECCOUNT("Mainset_Cur") = 0,.F.,.T.)
			IF !THIS.lAlowMe
				THIS.lcMessage = "Interface Database object not found..."
			ENDIF
			THIS.sqlconobj.sqlconnclose("nhandle")
		ENDIF
	ENDIF
&& Raghu061109
	IF USED("Mainset_Cur")
		USE IN Mainset_Cur
	ENDIF
	THIS.sqlconobj.sqlconnclose("nhandle")
	RETURN THIS.lAlowMe
	ENDPROC
ENDDEFINE
