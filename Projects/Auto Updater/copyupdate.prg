LPARAMETERS _ErrHtmlPath
_ErrHtmlPath = STRTRAN(_ErrHtmlPath,'*~*',' ')

Set Deleted On
Set Date British
Set Century On
Set Talk Off
Set Safety Off
Set Status Off
Set Null On
Set NullDisplay To ''
Set StrictDate To 0

PUBLIC vumess,Apath,_UpdateError,_mErrHtmlName

IF FILE(_ErrHtmlPath+'.dbf')
	USE (_ErrHtmlPath) ALIAS _TmpErrLog
	SELECT * FROM _TmpErrLog INTO CURSOR ErrLog ReadWrite
	IF USED('_TmpErrLog')
		USE IN _TmpErrLog
	ENDIF
	ERASE (_ErrHtmlPath+'.dbf') RECYCLE	
	ERASE (_ErrHtmlPath+'.fpt') RECYCLE		
ELSE
	Quit	
ENDIF
	
SELECT ErrLog
GO Top

*!*	_SourceFile  	= _TblNm.DataFldrNm
*!*	_TargetFile  	= _TblNm.MainPath
*!*	_MainCoName		= _TblNm.MainCoNm
*!*	vumess       	= _TblNm.MsgHead
*!*	_UpdateError 	= _TblNm.UpdtErr
*!*	_mErrHtmlName 	= _TblNm.ErrHTMLNm

_SourceFile  	= ErrLog.DataFldrNm
_TargetFile  	= ErrLog.MainPath
_MainCoName		= ErrLog.MainCoNm
vumess       	= ErrLog.MsgHead
_UpdateError 	= ErrLog.UpdtErr
_mErrHtmlName 	= ErrLog.ErrHTMLNm
Apath = _TargetFile

*!*	=ErrLog('','')	
*!*	SELECT ErrLog
*!*	REPLACE ErrMsg WITH _TblNm.ErrMsg,;
*!*		WarnMsg WITH _TblNm.WarnMsg,;
*!*		AuditMsg WITH _TblNm.AuditMsg IN ErrLog

IF _UpdateError = 1
	_continue = .t.
	IF DIRECTORY(_SourceFile,1) = .f.
		*=Messagebox("Unable to Find Folder "+_SourceFile+".",0+16,vumess)
		*RETURN .f.
		=ErrLog("Unable to Find Folder "+_SourceFile+".",'W')
		_continue = .f.
	ENDIF
	IF DIRECTORY(_TargetFile,1) = .f.
		*=Messagebox("Unable to Find Folder "+_TargetFile+".",0+16,vumess)
		*RETURN .f.
		=ErrLog("Unable to Find Folder "+_TargetFile+".",'W')
		_continue = .f.		
	ENDIF
	
	IF _continue = .t.
		_TmpTblNm1 = '_TmpBefUpdt'
		_TmpTblNm2 = '_TmpAftUpdt'

		_getupdttbl = _TmpTblNm1
		IF !USED(_getupdttbl)
			CREATE CURSOR (_getupdttbl) (Co_name Varchar(250),Co_path Varchar(250),FName Varchar(250),FDate Varchar(50),FSize N(10))
		ENDIF
		_getupdtdirnm = _TargetFile
		_afilecnt = ADIR(_afilenm,_getupdtdirnm+'*.*')
		FOR i1 = 1 TO _afilecnt
			SELECT (_getupdttbl)
			APPEND BLANK IN (_getupdttbl)
			REPLACE Co_Name WITH _MainCoName,Co_Path WITH _getupdtdirnm,;
				FName WITH _afilenm(i1,1),FSize WITH _afilenm(i1,2),;
				FDate WITH dTOc(_afilenm(i1,3))+' '+SUBSTR(TTOC(CTOT(_afilenm(i1,4))),11) IN (_getupdttbl)					
		ENDFOR
		RELEASE _afilenm

		RELEASE _FileSrcInfo,_FileTgtInfo
		_FileSrcCnt = 0
		_FileTgtCnt = 0
		_FileSrcCnt = ADIR(_FileSrcInfo,_SourceFile+'*.*')
		v1 = 0
		FOR i1 = 1 TO _FileSrcCnt
			_ErrMsg = ''
			_mFileNm = _FileSrcInfo(i1,1)			
			Try
				COPY FILE (_SourceFile+_mFileNm) TO (_TargetFile +_mFileNm)
			CATCH TO m_errMsg
				_ErrMsg = ALLTRIM(m_errMsg.Message)
			endtry	
			IF !EMPTY(_ErrMsg)
				*=Messagebox('File Name '+_mFileNm+CHR(13)+CHR(13)+_ErrMsg+CHR(13)+CHR(13)+;
					'Please Copy File from '+_SourceFile+' to '+_TargetFile,0+16,vumess)
				=ErrLog('File Name '+_mFileNm+CHR(13)+CHR(13)+_ErrMsg+CHR(13)+CHR(13)+;
					'Please Copy File from '+_SourceFile+' to '+_TargetFile,'W')
				Loop
			ENDIF
			_FileTgtCnt = ADIR(_FileTgtInfo,_TargetFile +_mFileNm)
			IF _FileTgtCnt = 0
				=ErrLog(_mFileNm+' not copied to '+_mtrgtpath+' Folder.'+CHR(13)+CHR(13)+;
					'Please Copy File from '+_SourceFile+' to '+_TargetFile,'W')
				Loop
			ENDIF
			IF _FileTgtInfo(1,2) <> _FileSrcInfo(i1,2) OR _FileTgtInfo(1,3) <> _FileSrcInfo(i1,3) OR _FileTgtInfo(1,4) <> _FileSrcInfo(i1,4)
				=ErrLog(_mFileNm+' not copied properly to '+_TargetFile+' Folder.'+CHR(13)+CHR(13)+;
					'Please Copy File from '+_SourceFile+' to '+_TargetFile,'W')
				Loop
			ENDIF
			Try
				ERASE (_SourceFile+_mFileNm) RECYCLE
			CATCH TO m_errMsg
				_ErrMsg = ALLTRIM(m_errMsg.Message)
			endtry	
			
			IF v1 = 0
				=ErrLog("Files Updated",'A','Y','')
				=ErrLog(_MainCoName,'A','Y','B')
			Endif
			=ErrLog(_mFileNm,'A','N','')

			v1 = v1 + 1	
		Endfor	
		Try
			RmDir (_SourceFile)	
		CATCH TO m_errMsg
			_ErrMsg = ALLTRIM(m_errMsg.Message)
		endtry	

		_getupdttbl = _TmpTblNm2
		IF !USED(_getupdttbl)
			CREATE CURSOR (_getupdttbl) (Co_name Varchar(250),Co_path Varchar(250),FName Varchar(250),FDate Varchar(50),FSize N(10))
		ENDIF
		_getupdtdirnm = _TargetFile
		_afilecnt = ADIR(_afilenm,_getupdtdirnm+'*.*')
		FOR i1 = 1 TO _afilecnt
			SELECT (_getupdttbl)
			APPEND BLANK IN (_getupdttbl)
			REPLACE Co_Name WITH _MainCoName,Co_Path WITH _getupdtdirnm,;
				FName WITH _afilenm(i1,1),FSize WITH _afilenm(i1,2),;
				FDate WITH dTOc(_afilenm(i1,3))+' '+SUBSTR(TTOC(CTOT(_afilenm(i1,4))),11) IN (_getupdttbl)					
		ENDFOR
		RELEASE _afilenm
	
		=GenUpdtLog(_MainCoName)
		
	Endif	
Endif

IF _UpdateError = 2
	_continue = .t.
	IF DIRECTORY(_SourceFile,1) = .f.
		_continue = .f.
	ENDIF
	IF _continue = .t.
		RELEASE _FileSrcInfo
		_FileSrcCnt = 0
		_FileSrcCnt = ADIR(_FileSrcInfo,_SourceFile+'*.*')
		FOR i1 = 1 TO _FileSrcCnt
			_ErrMsg = ''
			_mFileNm = _FileSrcInfo(i1,1)			
			Try
				ERASE (_SourceFile+_mFileNm) RECYCLE
			CATCH TO m_errMsg
				_ErrMsg = ALLTRIM(m_errMsg.Message)
			endtry	
		Endfor	
		Try
			RmDir (_SourceFile)	
		CATCH TO m_errMsg
			_ErrMsg = ALLTRIM(m_errMsg.Message)
		endtry	
	Endif	
Endif

=ErrLog("Update Ended at "+TRANSFORM(DATETIME()),'A')

SELECT 0
CREATE Cursor ErrLogCur (ErrGrp Char(1),ErrSec Char(10),ErrMsg Memo)

_ErrVal = ErrLog.ErrMsg
i1 = 1
i3 = '*~*'
i2 = OCCURS(i3,_ErrVal)
FOR i1 = 1 TO i2
	_ErrVal1 = subs(_ErrVal,AT(i3,_ErrVal)+3,IIF(i1 >= i2,LEN(_ErrVal),AT(i3,_ErrVal,2)-5))
	_ErrVal2 = IIF('*~ZIP~*' $ _ErrVal1,'Z',IIF('*~COMP~*' $ _ErrVal1,'C',''))
	_ErrVal1 = STRTRAN(_ErrVal1,'*~ZIP~*','')
	_ErrVal1 = STRTRAN(_ErrVal1,'*~COMP~*','')
	_ErrVal = subs(_ErrVal,AT(i3,_ErrVal,2))
	
	SELECT ErrLogCur
	APPEND BLANK IN ErrLogCur
	REPLACE ErrGrp WITH 'E',ErrSec WITH _ErrVal2, ErrMsg WITH _ErrVal1 IN ErrLogCur
Endfor

_ErrVal = ErrLog.WarnMsg
i1 = 1
i3 = '*~*'
i2 = OCCURS(i3,_ErrVal)
FOR i1 = 1 TO i2
	_ErrVal1 = subs(_ErrVal,AT(i3,_ErrVal)+3,IIF(i1 >= i2,LEN(_ErrVal),AT(i3,_ErrVal,2)-5))
	_ErrVal2 = IIF('*~ZIP~*' $ _ErrVal1,'Z',IIF('*~COMP~*' $ _ErrVal1,'C',''))
	_ErrVal1 = STRTRAN(_ErrVal1,'*~ZIP~*','')
	_ErrVal1 = STRTRAN(_ErrVal1,'*~COMP~*','')
	_ErrVal = subs(_ErrVal,AT(i3,_ErrVal,2))

	SELECT ErrLogCur
	APPEND BLANK IN ErrLogCur
	REPLACE ErrGrp WITH 'W',ErrSec WITH _ErrVal2, ErrMsg WITH _ErrVal1 IN ErrLogCur
Endfor

_ErrVal = ErrLog.AuditMsg
i1 = 1
i3 = '*~*'
i2 = OCCURS(i3,_ErrVal)
FOR i1 = 1 TO i2
	_ErrVal1 = subs(_ErrVal,AT(i3,_ErrVal)+3,IIF(i1 >= i2,LEN(_ErrVal),AT(i3,_ErrVal,2)-5))
	_ErrVal2 = IIF('*~ZIP~*' $ _ErrVal1,'Z',IIF('*~COMP~*' $ _ErrVal1,'C',''))
	_ErrVal1 = STRTRAN(_ErrVal1,'*~ZIP~*','')
	_ErrVal1 = STRTRAN(_ErrVal1,'*~COMP~*','')
	_ErrVal = subs(_ErrVal,AT(i3,_ErrVal,2))

	SELECT ErrLogCur
	APPEND BLANK IN ErrLogCur
	REPLACE ErrGrp WITH 'A',ErrSec WITH _ErrVal2, ErrMsg WITH _ErrVal1 IN ErrLogCur
Endfor

Local lcHTML
lcHTML = ''
_OldErrGrp = '*'
SELECT ErrLogCur
SCAN
	IF _OldErrGrp != ErrLogCur.ErrGrp
		lcHTML = lcHTML + '<p>' 
		DO case
		CASE ErrLogCur.ErrGrp = 'E'
			lcHTML = lcHTML + '<font color="red">'
			lcHTML = lcHTML + 'Errors'
		CASE ErrLogCur.ErrGrp = 'W'
			lcHTML = lcHTML + '<font color="green">'
			lcHTML = lcHTML + 'Information'
		CASE ErrLogCur.ErrGrp = 'A'
			lcHTML = lcHTML + '<font color="green">'
			lcHTML = lcHTML + 'Update Details'
		Endcase			
		lcHTML = lcHTML + '</font>'
		lcHTML = lcHTML + '</p>'
	Endif

	lcHTML = lcHTML + '<p>' 
	IF INLIST(ErrLogCur.ErrSec,'Z','C') AND !EMPTY(ErrLogCur.ErrSec)
		lcHTML = lcHTML + '<b>'
	Endif	
	IF INLIST(ErrLogCur.ErrSec,'C') AND !EMPTY(ErrLogCur.ErrSec)
		lcHTML = lcHTML + '<font color="blue">'
	Endif	
	lcHTML = lcHTML + ErrLogCur.ErrMsg
	IF INLIST(ErrLogCur.ErrSec,'C') AND !EMPTY(ErrLogCur.ErrSec)
		lcHTML = lcHTML + '</font>'
	Endif	
	IF INLIST(ErrLogCur.ErrSec,'Z','C') AND !EMPTY(ErrLogCur.ErrSec)
		lcHTML = lcHTML + '</b>'
	Endif
	lcHTML = lcHTML + '</p>'

	_OldErrGrp = ErrLogCur.ErrGrp
	SELECT ErrLogCur
Endscan
=STRTOFILE(lcHTML,_mErrHtmlName)

SELECT ErrLog
REPLACE ALL ErrMsg WITH STRTRAN(STRTRAN(STRTRAN(ErrMsg,'*~*',''),'*~ZIP~*',''),'*~COMP~*','') IN ErrLog
REPLACE ALL WarnMsg WITH STRTRAN(STRTRAN(STRTRAN(WarnMsg,'*~*',''),'*~ZIP~*',''),'*~COMP~*','') IN ErrLog
REPLACE ALL AuditMsg WITH STRTRAN(STRTRAN(STRTRAN(AuditMsg,'*~*',''),'*~ZIP~*',''),'*~COMP~*','') IN ErrLog
GO Top

IF _UpdateError = 2	OR LEN(ALLTRIM(ErrLog.ErrMsg)) >  0 OR LEN(ALLTRIM(ErrLog.WarnMsg)) >  0
	DO FORM efileerrwindow
	READ Events
ELSE
	_SCREEN.VISIBLE = .F.
	DO Case
	Case _UpdateError = 1
		=Messagebox('Update Done Successfully',64,vumess)
	OTHERWISE
		=Messagebox('Nothing to Update',64,vumess)
	ENDCASE
*!*		IF FILE(_mErrLogName+'.dbf')
*!*			IF USED('ErrLog')
*!*				USE IN ErrLog
*!*			ENDIF
*!*			ERASE (_mErrLogName+'.dbf') RECYCLE	
*!*			ERASE (_mErrLogName+'.fpt') RECYCLE		
*!*		ENDIF
Endif	
Quit
