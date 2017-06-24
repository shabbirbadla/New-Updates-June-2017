LPARAMETERS _xmlgenfilenm,_xmleFileName
_screen.ActiveForm.panel.Panels(1).Text = 'Generating XML........'
If !Used('AcesXMLMap_vw')
	Select 0
	Select * From AcesXMLMap WHERE eFileName = _xmleFileName AND Product = mProduct Into Cursor AcesXMLMap_vw Readwrite
	Select AcesXMLMap_vw
	Index On TagId Tag TagId
	Index On XMLOrder Tag XMLOrder
ENDIF

If !Used('XMLDataUpdate_vw')
	Select 0
	Select * From XMLDataUpdate WHERE eFileName = _xmleFileName AND Product = mProduct Into Cursor XMLDataUpdate_vw Readwrite
	Select XMLDataUpdate_vw
	Index On '*'+ALLTRIM(TagGroup)+ALLTRIM(FinalTagNm)+'*' Tag TagName
ENDIF

If !Used('XMLDataValidate_vw')
	Select 0
	Select * From XMLDataValidate WHERE eFileName = _xmleFileName AND Product = mProduct Into Cursor XMLDataValidate_vw Readwrite
	Select XMLDataValidate_vw
	Index On '*'+ALLTRIM(TagGroup)+ALLTRIM(FinalTagNm)+'*' Tag TagName
ENDIF

IF !USED('XMLData_Vw')
	=MESSAGEBOX('XML Data Table Not Found',0,vumess)
	RETURN .f.
ENDIF
SELECT XMLData_Vw
UPDATE XMLData_Vw SET TagOrder = FinalXMLMast.TagOrder From XMLData_Vw,FinalXMLMast ;
	WHERE ALLTRIM(XMLData_Vw.TagGroup) == ALLTRIM(FinalXMLMast.TagGroup) AND ALLTRIM(FinalXMLMast.eFileName) == ALLTRIM(_xmleFileName)
GO Top

IF USED('FinalXMLMast')
	USE IN FinalXMLMast
ENDIF
	
_screen.ActiveForm.panel.Panels(1).Text = 'Generating XML..........'

IF USED('DupErr')
	SELECT DupErr
	USE IN DupErr
Endif	
SELECT 0
SELECT a.TagId,b.ObjDesc,b.ObjDesc As ObjErr FROM AcesXMLMap a,FinalXMLMap b WHERE 1 = 2 INTO CURSOR DupErr Readwrite
SELECT DupErr
Index On TagId Tag TagName

SELECT 0
CREATE CURSOR ErrLog (ErrMsg Memo,WarnMsg Memo)
SELECT ErrLog
APPEND BLANK IN ErrLog

_lsuccess = .t.
oRegExp = CreateObject("VBScript.RegExp")
IF TYPE('oRegExp') <> 'O'
	=MESSAGEBOX('VB Script Not Found',0,vumess)
	RETURN .f.
ENDIF

_screen.ActiveForm.panel.Panels(1).Text = 'Generating XML............'
oXML = CREATEOBJECT("msxml2.DOMDocument")
IF TYPE('oXML') <> 'O'
	oXML = CREATEOBJECT("msxml.DOMDocument")
Endif
IF TYPE('oXML') <> 'O'
	=MESSAGEBOX('MSXML Not Found',0,vumess)
	RETURN .f.
ENDIF

_screen.ActiveForm.panel.Panels(1).Text = 'Validating Data..'
_TryError     = .f.
TRY
	_lsuccessdone = .f.
	_lsuccessdone = eFileDefaValidations()
	IF _lsuccessdone = .f.
		_TryError = .t.
	ENDIF
CATCH TO oException
	IF oException.ErrorNo <> 1
		_TryError = .t.
		=Messagebox(ALLTRIM(oException.Message),0,vumess)
	ENDIF
ENDTRY
IF _TryError = .t.
	_lsuccess = .f.
ENDIF

_screen.ActiveForm.panel.Panels(1).Text = 'Generating XML..............'

SELECT XMLData_Vw
GO Top
DO WHILE !EOF()

	objRecord = ''
	Select AcesXMLMap_vw
	SET ORDER TO Tag XMLOrder
	SCAN
		IF EMPTY(AcesXMLMap_vw.FinalTagNm)
			Loop
		Endif
		IF !EMPTY(AcesXMLMap_vw.RunCond)
			=EVALUATE(AcesXMLMap_vw.RunCond)	
		Endif
		_mSkipCond = .t.
		IF !EMPTY(AcesXMLMap_vw.SkipCond)
			_mSkipCond = EVALUATE(AcesXMLMap_vw.SkipCond)	
		Endif
		IF _mSkipCond = .f.
			LOOP
		Endif	

		_objPrntNode = 'objRoot'
		IF TYPE(_objPrntNode) = 'O'
			_objPrntNodeName = _objPrntNode+'.nodeName'
			IF !INLIST(UPPER(&_objPrntNodeName),'#DOCUMENT')
				DO WHILE .t.
					_objPrntNodeName = _objPrntNode+'.nodeName'
					IF &_objPrntNodeName = ALLTRIM(AcesXMLMap_vw.ParentTgId)
						objRecord = &_objPrntNode
						Exit
					Else
						_objPrntNode = _objPrntNode + '.ParentNode'
						IF TYPE(_objPrntNode) = 'O'
						ELSE
							EXIT
						ENDIF
					EndIf
				ENDDO
			ENDIF 	
		ENDIF
		objRoot = oxml.createElement(ALLTRIM(AcesXMLMap_vw.FinalTagNm))
		
		_mObjDescVal = ''
		IF !EMPTY(AcesXMLMap_vw.Atrib1_Val)
			_mObjDescVal = EVALUATE(AcesXMLMap_vw.Atrib1_Val)	
		Endif
		IF !EMPTY(_mObjDescVal) AND !EMPTY(AcesXMLMap_vw.Atrib1_Tag)
			objRoot.setattribute(ALLTRIM(AcesXMLMap_vw.Atrib1_Tag),_mObjDescVal)
		Endif

		_mObjDescVal = ''
		IF !EMPTY(AcesXMLMap_vw.Atrib2_Val)
			_mObjDescVal = EVALUATE(AcesXMLMap_vw.Atrib2_Val)	
		Endif
		IF !EMPTY(_mObjDescVal) AND !EMPTY(AcesXMLMap_vw.Atrib2_Tag)
			objRoot.setattribute(ALLTRIM(AcesXMLMap_vw.Atrib2_Tag),_mObjDescVal)
		Endif
	
		SELECT XMLData_Vw	
		IF !EMPTY(MLINE(AcesXMLMap_vw.RawTagName,1))
			_avalue  = ALLTRIM(MLINE(AcesXMLMap_vw.RawTagName,1))
			_avalue1 = ''
			IF !EMPTY(_avalue)	
				_avalue1 = EVALUATE(_avalue)
			endif		
			IF EMPTY(_avalue1) AND !EMPTY(AcesXMLMap_vw.defaval)
				_avalue1 = EVALUATE(AcesXMLMap_vw.defaval)
			ENDIF
			_avalue1 = DefaUpdates(_avalue1)

			DIMENSION _DefaValidRetValue(1,2)
			_DefaValidRetValue(1,1) = ''
			_DefaValidRetValue(1,2) = .f.
			=DefaValidations(_avalue1)
			IF _DefaValidRetValue(1,2) = .f.
				_lsuccess = .f.
			ENDIF
			_avalue1   = _DefaValidRetValue(1,1) 	
			_avaluelen =  LEN(TRANSFORM(_avalue1))
			IF ISNULL(_avalue1)	
				_avalue1 = ''
			ENDIF
			IF XMLData_Vw.BlankTag = .t.		&&&
			Else
				objRoot.Text = iif(_avaluelen > 0,CAST(_avalue1 as varchar(_avaluelen)),_avalue1)
			Endif	
		Endif	

		IF TYPE('objRecord') = 'O'
			objRecord.appendChild(objRoot)
		ELSE
			oXML.appendChild(objRoot)
		Endif	

		Select AcesXMLMap_vw
	ENDSCAN

	SELECT XMLData_Vw
	IF !EOF()
		SKIP
	ENDIF
Enddo		


IF _mDirecteFiling = .f.
	objIntro = oxml.createProcessingInstruction("xml","version='1.0'")	
	oxml.insertBefore(objIntro,oxml.childNodes(0))
ENDIF

IF _lsuccess = .f. 
	_oldwindowstate = _screen.ActiveForm.windowstate
	_screen.ActiveForm.windowstate = 2
	DO FORM eFileErrWindow TO _lsuccess
	_screen.ActiveForm.windowstate = _oldwindowstate
Endif	

_efilePeriod = ''
IF _screen.ActiveForm.cmbPeriodFrm.Visible = .t.
	_efilePeriod = _efilePeriod + ALLTRIM(_screen.ActiveForm.cmbPeriodFrm.DisplayValue)
ENDIF
_efilePeriod = STRTRAN(_efilePeriod,',','-')
_errFileName = ALLTRIM(_xmleFileName)+'_'+_efilePeriod+'.Log'
IF FILE(_errFileName)
	_errstatus = FOPEN(_errFileName,12)
ELSE
	_errstatus = FCREATE(_errFileName)
ENDIF
=FSeek(_errstatus,0,2)
=FPuts(_errstatus,'Report Generated on '+TTOC(DATETIME()))
IF _lsuccess = .t.
	IF _errstatus >= 0
		=FPUTS(_errstatus,'')
		=FPUTS(_errstatus,'Successfully Generated')
		=FPUTS(_errstatus,'')
		=FPUTS(_errstatus,'------------------------------------ < End > ------------------------------------')
		=FPUTS(_errstatus,'')
	ENDIF
	=FCLOSE(_errstatus)
	IF _mDirecteFiling = .f.
		oXML.save(ALLTRIM(_xmleFileName)+'.XML')
		oXML.save(_finalxmlname)
	Endif	
ELSE
	IF _errstatus >= 0
		=FPUTS(_errstatus,'')
		IF !EMPTY(ErrLog.WarnMsg)
			=FPUTS(_errstatus,'Information :-')
			=FPUTS(_errstatus,ErrLog.WarnMsg)
			=FPUTS(_errstatus,'')
		ENDIF
		IF !EMPTY(ErrLog.ErrMsg)
			=FPUTS(_errstatus,'Error :-')
			=FPUTS(_errstatus,ErrLog.ErrMsg)
			=FPUTS(_errstatus,'')
		ENDIF
		=FPUTS(_errstatus,'------------------------------------ < End > ------------------------------------')
		=FPUTS(_errstatus,'')
	ENDIF
	=FCLOSE(_errstatus)
Endif	
RETURN _lsuccess


PROCEDURE DefaUpdates
LPARAMETERS _grouptovalidate

_thismthodsuccessdone = .t. 
SELECT XMLDataUpdate_vw
IF SEEK('*'+ALLTRIM(AcesXMLMap_vw.TagId)+'*','XMLDataUpdate_vw','TagName')
	SCAN WHILE ALLTRIM(TagGroup) + ALLTRIM(FinalTagNm) == ALLTRIM(AcesXMLMap_vw.TagId)

		_grouptovalidate = STRTRAN(_grouptovalidate,ALLTRIM(XMLDataUpdate_vw.oldstr),ALLTRIM(XMLDataUpdate_vw.newstr))

		SELECT XMLDataUpdate_vw
	ENDSCAN
Endif	
RETURN _grouptovalidate


PROCEDURE DefaValidations
LPARAMETERS _grouptovalidate

_thismthodsuccessdone = .t. 
_thismthodsuccesstest = .t.
SELECT XMLDataValidate_vw
IF SEEK('*'+ALLTRIM(AcesXMLMap_vw.TagId)+'*','XMLDataValidate_vw','TagName')
	SCAN WHILE ALLTRIM(TagGroup) + ALLTRIM(FinalTagNm) == ALLTRIM(AcesXMLMap_vw.TagId)
		IF TYPE('_grouptovalidate') = 'C'
			_grouptovalidate = ALLTRIM(_grouptovalidate)	
		Endif	
		_avalue = ALLTRIM(XMLDataValidate_vw.format)
		IF !EMPTY(_avalue)
			_grouptovalidate = ALLTRIM(TRANSFORM(_grouptovalidate,_avalue))
		Endif

		_avalue = ALLTRIM(XMLDataValidate_vw.regularexp)
		IF !EMPTY(_avalue)
			_avalue = EVALUATE(_avalue)
		Endif	
		IF !EMPTY(_avalue)
			oRegExp.Pattern = _avalue
			IF ISNULL(_grouptovalidate)	
				_thismthodsuccesstest = .f.
			Else	
				_thismthodsuccesstest = oRegExp.test(_grouptovalidate)
			Endif	
			IF _thismthodsuccesstest = .f.
				_errmessage = ''
				_avalue = ALLTRIM(XMLDataValidate_vw.regexperr)
				IF !EMPTY(_avalue)
					_errmessage = EVALUATE(_avalue)
				Endif	
				_ShowErr = .t.
				IF XMLDataValidate_vw.ValidOnce
					SELECT DupErr
					GO Top
					LOCATE FOR ALLTRIM(TagId) == ALLTRIM(XMLDataValidate_vw.TagGroup)+ALLTRIM(XMLDataValidate_vw.FinalTagNm) AND ALLTRIM(TRANSFORM(ObjDesc)) == ALLTRIM(TRANSFORM(_grouptovalidate)) AND ALLTRIM(TRANSFORM(ObjErr)) == ALLTRIM(TRANSFORM(_errmessage))
					IF FOUND()
						_ShowErr = .f.
					ELSE
						APPEND BLANK IN DupErr
						REPLACE TagId WITH ALLTRIM(XMLDataValidate_vw.TagGroup)+ALLTRIM(XMLDataValidate_vw.FinalTagNm),;
							ObjDesc WITH ALLTRIM(TRANSFORM(_grouptovalidate)),;
							ObjErr WITH ALLTRIM(TRANSFORM(_errmessage)) IN DupErr
					Endif	
				Endif	
				IF _ShowErr = .t.
					IF XMLDataValidate_vw.IsWarning
						REPLACE WarnMsg WITH ErrLog.WarnMsg +;
							CHR(13) + 'Current Value : '+IIF(EMPTY(TRANSFORM(_grouptovalidate)),'<BLANK>',ALLTRIM(TRANSFORM(_grouptovalidate))) IN ErrLog
						_avalue = ALLTRIM(XMLDataValidate_vw.regexperr)
						IF !EMPTY(_avalue)
							REPLACE WarnMsg WITH ErrLog.WarnMsg + ' --> '+_errmessage IN ErrLog
						Endif	
					ELSE
						REPLACE ErrMsg WITH ErrLog.ErrMsg +;
							CHR(13) + 'Current Value : '+IIF(EMPTY(TRANSFORM(_grouptovalidate)),'<BLANK>',ALLTRIM(TRANSFORM(_grouptovalidate))) IN ErrLog
						_avalue = ALLTRIM(XMLDataValidate_vw.regexperr)
						IF !EMPTY(_avalue)
							REPLACE ErrMsg WITH ErrLog.ErrMsg + ' --> '+_errmessage IN ErrLog
						Endif	
					Endif	
				Endif	
				_thismthodsuccessdone = .f.
			Endif	
		Endif	

		SELECT XMLDataValidate_vw
	ENDSCAN
Endif	
DIMENSION _DefaValidRetValue(1,2)
_DefaValidRetValue(1,1) = _grouptovalidate
_DefaValidRetValue(1,2) = _thismthodsuccessdone



