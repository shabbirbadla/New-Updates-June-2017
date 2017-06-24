LPARAMETERS _mErrLogDesc,_errType,_errBold,_errColor
IF TYPE('_errBold') != 'C'
	_errBold = 'N'
ENDIF
IF TYPE('_errColor') != 'C'
	_errColor = ''
ENDIF
olderralias = ALIAS()

Local lcHTML
lcHTML = ''
*lcHTML = lcHTML + '<HTML>'
*lcHTML = lcHTML + '<BODY>'
lcHTML = lcHTML + '<p>' 
IF _errBold = 'Y'
	lcHTML = lcHTML + '<b>'
Endif	
IF _errColor = 'B'
	lcHTML = lcHTML + '<font color="blue">'
Endif	
lcHTML = lcHTML + _mErrLogDesc
IF !EMPTY(_errColor)
	lcHTML = lcHTML + '</font>'
Endif	
IF _errBold = 'Y'
	lcHTML = lcHTML + '</b>'
Endif
lcHTML = lcHTML + '</p>'
*lcHTML = lcHTML + '</BODY>'
*lcHTML = lcHTML + '</HTML>'
=STRTOFILE(lcHTML,_mErrHtmlName,1)


*!*	IF !FILE(_mErrLogName+'.dbf')
*!*		CREATE Cursor (_mErrLogName) (ErrMsg Memo,WarnMsg Memo)
*!*		IF USED(JUSTFNAME(_mErrLogName))
*!*			USE IN (JUSTFNAME(_mErrLogName))
*!*		ENDIF
*!*	ENDIF
*!*	IF !USED('ErrLog')
*!*		SELECT 0
*!*		USE (_mErrLogName) AGAIN SHARED ALIAS ErrLog
*!*	ENDIF
IF !USED('ErrLog')
	SELECT 0
	CREATE Cursor ErrLog (ErrMsg Memo,WarnMsg Memo,AuditMsg Memo,;
		MainCoNm Varchar(250),DataFldrNm Memo,MainPath Memo,MsgHead C(50),UpdtErr N(1),ErrHTMLNm Memo)
	APPEND BLANK IN ErrLog
ENDIF

SELECT ErrLog
*!*	IF RECCOUNT() <= 0
*!*		APPEND BLANK IN ErrLog	
*!*	Endif	
mspl_chr = '*~*' + IIF(_errBold = 'Y' And EMPTY(_errColor),'*~ZIP~*','') + IIF(_errBold = 'Y' And _errColor = 'B','*~COMP~*','')
IF _errType = 'E' AND !EMPTY(_mErrLogDesc)
	REPLACE ErrMsg WITH ErrMsg + CHR(13) + mspl_chr + _mErrLogDesc IN ErrLog	
Endif	
IF _errType = 'W' AND !EMPTY(_mErrLogDesc)
	REPLACE WarnMsg WITH WarnMsg + CHR(13) + mspl_chr + _mErrLogDesc IN ErrLog	
Endif	
IF _errType = 'A' AND !EMPTY(_mErrLogDesc)
	REPLACE AuditMsg WITH AuditMsg + CHR(13) + mspl_chr + _mErrLogDesc IN ErrLog	
Endif	

SELECT 0
IF !EMPTY(olderralias)
	SELECT (olderralias)
Endif	

