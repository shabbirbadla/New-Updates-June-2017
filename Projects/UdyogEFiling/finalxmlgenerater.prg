LPARAMETERS _xmlgenfilenm,_xmleFileName
_screen.ActiveForm.panel.Panels(1).Text = 'Generating XML........'
If !Used('FinalXMLMap_vw')
	Select 0
	Select * From FinalXMLMap WHERE eFileName = _xmleFileName AND Product = mProduct Into Cursor FinalXMLMap_vw Readwrite
	Select FinalXMLMap_vw
	Index On ALLTRIM(STR(Level))+'~'+TagGroup Tag LvlTagGrp
ENDIF

If !Used('XMLDataUpdate_vw')
	Select 0
	Select * From XMLDataUpdate WHERE eFileName = _xmleFileName AND Product = mProduct Into Cursor XMLDataUpdate_vw Readwrite
	Select XMLDataUpdate_vw
	Index On TagGroup+FinalTagNm Tag TagName
ENDIF

If !Used('XMLDataValidate_vw')
	Select 0
	Select * From XMLDataValidate WHERE eFileName = _xmleFileName AND Product = mProduct Into Cursor XMLDataValidate_vw Readwrite
	Select XMLDataValidate_vw
	Index On TagGroup+FinalTagNm Tag TagName
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
SELECT TagGroup,FinalTagNm,ObjDesc,ObjDesc As ObjErr FROM FinalXMLMap_vw WHERE 1 = 2 INTO CURSOR DupErr Readwrite
SELECT DupErr
Index On TagGroup+FinalTagNm Tag TagName

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
oXMLnew = oxml.createElement('DataSet')		
oXMLnew.SetAttribute('Version',mversion)		&&vasant010710
oXML.appendChild(oXMLnew)

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

*!*	SELECT XMLData_Vw
*!*	INDEX on TagGroup TAG TagGroup
*!*	SCAN
*!*		_lsuccessdone = eFileValidations(_xmleFileName,TagGroup)
*!*		IF _lsuccessdone = .f.
*!*			_lsuccess = .f.
*!*		ENDIF
*!*		SELECT XMLData_Vw
*!*	endscan	

_screen.ActiveForm.panel.Panels(1).Text = 'Generating XML..............'
_mTagGroup = ''
SELECT XMLData_Vw
SCAN
	_isLoop = .f.

	objRecord = ''
	IF !(TagGroup == _mTagGroup)
		_mTagGroup = XMLData_Vw.TagGroup

		_mLevel = '1'
		Select FinalXMLMap_vw
		IF SEEK(_mLevel+'~'+_mTagGroup,'FinalXMLMap_vw','LvlTagGrp')
			SCAN WHILE Level = VAL(_mLevel) AND TagGroup = _mTagGroup
				IF EMPTY(FinalXMLMap_vw.FinalTagNm)
					Loop
				Endif
				SELECT XMLData_Vw
				_xmlRecSkipCond = FinalXMLMap_vw.RecSkipCon
				IF !EMPTY(_xmlRecSkipCond)
					IF EVALUATE(_xmlRecSkipCond) = .f.
						_isloop = .t.	
					Endif	
				Endif
				IF _isLoop = .t.
					Select FinalXMLMap_vw
					loop
				ENDIF
				IF _mDirecteFiling = .f.
					objRoot = oxml.createElement(ALLTRIM(FinalXMLMap_vw.FinalTagNm))
					oXMLnew.appendChild(objRoot)
				ELSE
					APPEND BLANK 
				endif	
				Select FinalXMLMap_vw
			ENDSCAN
		Endif	
	endif	
	IF type('objRoot') <> 'O'
		SELECT XMLData_Vw
		loop
	ENDIF
	
	_avalue  = ''
	_avalue1 = ''
	_mLevel = '2'
	Select FinalXMLMap_vw
	IF SEEK(_mLevel+'~'+_mTagGroup,'FinalXMLMap_vw','LvlTagGrp')
		SCAN WHILE Level = VAL(_mLevel) AND TagGroup = _mTagGroup
			IF EMPTY(FinalXMLMap_vw.FinalTagNm)
				Loop
			Endif
			SELECT XMLData_Vw
			_xmlRecSkipCond = FinalXMLMap_vw.RecSkipCon
			IF !EMPTY(_xmlRecSkipCond)
				IF EVALUATE(_xmlRecSkipCond) = .f.
					_isloop = .t.	
				Endif	
			Endif
			IF _isLoop = .t.
				Select FinalXMLMap_vw
				loop
			ENDIF
			_avalue = ALLTRIM(FinalXMLMap_vw.objdesc)
			_avalue1 = ''
			IF !EMPTY(_avalue)	
				_avalue1 = EVALUATE(_avalue)
			endif		
			IF EMPTY(_avalue) OR !EMPTY(_avalue1)
				IF _mDirecteFiling = .f.
					objRecord = oxml.createElement(ALLTRIM(FinalXMLMap_vw.FinalTagNm))
					objRoot.appendChild(objRecord)
					IF !EMPTY(_avalue1)
						objRecord.setattribute('id',_avalue1)
					Endif	
				ELSE
					APPEND BLANK 	
				Endif	
			Endif	
			Select FinalXMLMap_vw
		ENDSCAN
	Endif	
	IF TYPE('objRecord') <> 'O'
		objRecord = objRoot
	Endif	

	IF _isLoop = .t.
		SELECT XMLData_Vw
		loop
	ENDIF

	IF EMPTY(_avalue) OR !EMPTY(_avalue1)
		_mLevel = '0'
		Select FinalXMLMap_vw
		IF SEEK(_mLevel+'~'+_mTagGroup,'FinalXMLMap_vw','LvlTagGrp')
			SCAN WHILE Level = VAL(_mLevel) AND TagGroup = _mTagGroup
				IF EMPTY(FinalXMLMap_vw.FinalTagNm)
					Loop
				Endif
				SELECT XMLData_Vw
				IF _mDirecteFiling = .f.
					objName = oxml.createElement(ALLTRIM(FinalXMLMap_vw.FinalTagNm))
				Endif	
				_avalue  = ALLTRIM(MLINE(FinalXMLMap_vw.RawTagName,1))
				_avalue1 = ''
				IF !EMPTY(_avalue)	
					_avalue1 = EVALUATE(_avalue)
				endif		
				IF EMPTY(_avalue1) AND !EMPTY(FinalXMLMap_vw.defaval)
					_avalue1 = EVALUATE(FinalXMLMap_vw.defaval)
				ENDIF
			
				_avalue1 = DefaUpdates(_avalue1)

				&&vasant300610
				DIMENSION _DefaValidRetValue(1,2)
				_DefaValidRetValue(1,1) = ''
				_DefaValidRetValue(1,2) = .f.
				=DefaValidations(_avalue1)
				IF _DefaValidRetValue(1,2) = .f.
				&&vasant300610
					_lsuccess = .f.
				ENDIF
				_avalue1   = _DefaValidRetValue(1,1) 	&&vasant010710
				_avaluelen =  LEN(TRANSFORM(_avalue1))
				IF ISNULL(_avalue1)	
					_avalue1 = ''
				ENDIF
				IF _mDirecteFiling = .f.	
					IF XMLData_Vw.BlankTag = .t.
					Else
						objName.Text = iif(_avaluelen > 0,CAST(_avalue1 as varchar(_avaluelen)),_avalue1)
					Endif	
					objRecord.appendChild(objName)
				ELSE
					APPEND BLANK 	
				Endif	
				Select FinalXMLMap_vw
			ENDSCAN
		Endif	
	Endif	

	SELECT XMLData_Vw
ENDSCAN	

IF _mDirecteFiling = .f.
	objIntro = oxml.createProcessingInstruction("xml","version='1.0'")	&&vasant010710
	oxml.insertBefore(objIntro,oxml.childNodes(0))
ENDIF

IF _lsuccess = .f. 
	_oldwindowstate = _screen.ActiveForm.windowstate
	_screen.ActiveForm.windowstate = 2
	DO FORM eFileErrWindow TO _lsuccess
	_screen.ActiveForm.windowstate = _oldwindowstate
Endif	

&&vasant010710
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
&&vasant010710
IF _lsuccess = .t.
	&&vasant010710
	IF _errstatus >= 0
		=FPUTS(_errstatus,'')
		=FPUTS(_errstatus,'Successfully Generated')
		=FPUTS(_errstatus,'')
		=FPUTS(_errstatus,'------------------------------------ < End > ------------------------------------')
		=FPUTS(_errstatus,'')
	ENDIF
	=FCLOSE(_errstatus)
	&&vasant010710
	IF _mDirecteFiling = .f.
		oXML.save(ALLTRIM(_xmleFileName)+'.XML')
	Endif	
&&vasant010710
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
&&vasant010710	
Endif	
RETURN _lsuccess


PROCEDURE DefaUpdates
LPARAMETERS _grouptovalidate

_thismthodsuccessdone = .t. 
SELECT XMLDataUpdate_vw
IF SEEK(FinalXMLMap_vw.TagGroup+FinalXMLMap_vw.FinalTagNm,'XMLDataUpdate_vw','TagName')
	SCAN WHILE ALLTRIM(TagGroup) == ALLTRIM(FinalXMLMap_vw.TagGroup) AND ALLTRIM(FinalTagNm) == ALLTRIM(FinalXMLMap_vw.FinalTagNm)

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
IF SEEK(FinalXMLMap_vw.TagGroup+FinalXMLMap_vw.FinalTagNm,'XMLDataValidate_vw','TagName')
	SCAN WHILE ALLTRIM(TagGroup) == ALLTRIM(FinalXMLMap_vw.TagGroup) AND ALLTRIM(FinalTagNm) = ALLTRIM(FinalXMLMap_vw.FinalTagNm)
		IF TYPE('_grouptovalidate') = 'C'
			_grouptovalidate = ALLTRIM(_grouptovalidate)	
		Endif	
		_avalue = ALLTRIM(XMLDataValidate_vw.format)
		IF !EMPTY(_avalue)
			_grouptovalidate = ALLTRIM(TRANSFORM(_grouptovalidate,_avalue))
		Endif

		&&vasant010710
		*_avalue = EVALUATE(ALLTRIM(XMLDataValidate_vw.regularexp))
		_avalue = ALLTRIM(XMLDataValidate_vw.regularexp)
		IF !EMPTY(_avalue)
			_avalue = EVALUATE(_avalue)
		Endif	
		&&vasant010710
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
					LOCATE FOR ALLTRIM(TagGroup) == ALLTRIM(XMLDataValidate_vw.TagGroup) AND ALLTRIM(FinalTagNm) == ALLTRIM(XMLDataValidate_vw.FinalTagNm) AND ALLTRIM(TRANSFORM(ObjDesc)) == ALLTRIM(TRANSFORM(_grouptovalidate)) AND ALLTRIM(TRANSFORM(ObjErr)) == ALLTRIM(TRANSFORM(_errmessage))
					IF FOUND()
						_ShowErr = .f.
					ELSE
						APPEND BLANK IN DupErr
						REPLACE TagGroup WITH ALLTRIM(XMLDataValidate_vw.TagGroup),;
							FinalTagNm WITH ALLTRIM(XMLDataValidate_vw.FinalTagNm),;
							ObjDesc WITH ALLTRIM(TRANSFORM(_grouptovalidate)),;
							ObjErr WITH ALLTRIM(TRANSFORM(_errmessage)) IN DupErr
					Endif	
				Endif	
				IF _ShowErr = .t.
					&&vasant010710
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
				&&vasant010710
			Endif	
		Endif	

		SELECT XMLDataValidate_vw
	ENDSCAN
Endif	
DIMENSION _DefaValidRetValue(1,2)
_DefaValidRetValue(1,1) = _grouptovalidate
_DefaValidRetValue(1,2) = _thismthodsuccessdone



