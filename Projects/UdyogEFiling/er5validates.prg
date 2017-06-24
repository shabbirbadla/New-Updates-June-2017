
PROCEDURE BeforeRawXMLUpdates
_errmessage = ''
RETURN _errmessage


PROCEDURE XMLGroupFinder
LPARAMETERS _trigtbl
IF TYPE('_trigtbl') <> 'C'
	_trigtbl = ''
Endif
_trigretvalue = ''
_oldtrigdatasession = SET("Datasession")
_trigtotsession = ASESSIONS(_trigsessionarr)
IF EMPTY(_trigretvalue) AND _trigretvalue <> '0'
	FOR _trigstartsession = 1 TO _trigtotsession
		SET DATASESSION TO _trigsessionarr(_trigstartsession)
		IF USED(JUSTSTEM(_trigtbl)) AND !EMPTY(_trigtbl)
			IF TYPE(_trigtbl) = 'N'
				_trigretvalue = str(&_trigtbl,1)
			Else
				_trigretvalue = TRANSFORM(&_trigtbl)
			Endif	
		Endif	
	ENDFOR
Endif	
SET DATASESSION TO (_oldtrigdatasession)
RETURN ALLTRIM(_trigretvalue)


PROCEDURE eFileDefaValidations
_oldAlias = ALIAS()
SELECT XmlData_vw
_thismthodsuccessdone = .t.
LOCATE FOR ALLTRIM(Taggroup) == '3'
IF !FOUND()
	REPLACE ErrMsg WITH ErrLog.ErrMsg +;
		CHR(13)+'Details of QUANTITY OF PRINCIPAL INPUTS USED FOR MANUFACTURING OF FINISHED GOODS not Found.' IN ErrLog
	_thismthodsuccessdone = .f.	
Endif

SELECT ALLTRIM(Left(RmName3,255)) As RmName,ALLTRIM(Left(RmChap3,255)) As RmChap,ALLTRIM(Left(RmRateUnit3,255)) As RmUom,;
	ALLTRIM(Left(FgName3,255)) As FgName,ALLTRIM(Left(FgChap3,255)) As FgChap,ALLTRIM(Left(FgRateUnit3,255))  As FgUom; 
	FROM XmlData_vw WHERE INLIST(TagGroup,'3') INTO CURSOR _lst_exc1
IF USED('_lst_exc1')	
	SELECT dist RmChap,RmName FROM _lst_exc1 WHERE !EMPTY(RmChap) AND RmChap NOT in (SELECT cetsh FROM cetsh) INTO CURSOR _lst_exc2
	IF USED('_lst_exc2')	
		SELECT _lst_exc2
		IF RECCOUNT() > 0
			SCAN
				REPLACE ErrMsg WITH ErrLog.ErrMsg +;
					CHR(13)+'Unable to find Chap. No. '+IIF(EMPTY(STRTRAN(_lst_exc2.RmChap,'0','')),'<BLANK>',ALLTRIM(_lst_exc2.RmChap)) + ;
					' in ACES List for Item : '+ALLTRIM(_lst_exc2.RmName) IN ErrLog
			Endscan		
			_thismthodsuccessdone = .f.	
		Endif	
		USE IN _lst_exc2
	ELSE
		_thismthodsuccessdone = .f.	
	ENDIF

	SELECT dist a.RmChap,a.RmUom as UOM_Cur,b.RateUnit as UOM_Org FROM _lst_exc1 a,cetsh b WHERE ;
		ALLTRIM(a.RmChap) == ALLTRIM(b.cetsh) ;
		AND a.RmUom NOT in (SELECT Dist RateUnit FROM cetsh) INTO CURSOR _lst_exc2
	IF USED('_lst_exc2')	
		SELECT _lst_exc2
		IF RECCOUNT() > 0
			SCAN
				IF !(UPPER(ALLTRIM(_lst_exc2.UOM_Cur)) == UPPER(ALLTRIM(_lst_exc2.UOM_Org)))
					REPLACE ErrMsg WITH ErrLog.ErrMsg +;
						CHR(13) + 'Current Value : '+IIF(EMPTY(_lst_exc2.UOM_Cur),'<BLANK>',ALLTRIM(_lst_exc2.UOM_Cur)) IN ErrLog
					REPLACE ErrMsg WITH ErrLog.ErrMsg +;
						' --> '+'UOM for the Chap. No. '+ALLTRIM(_lst_exc2.RmChap)+' should be '+ALLTRIM(_lst_exc2.UOM_Org) IN ErrLog
					_thismthodsuccessdone = .f.	
				Endif		
			Endscan		
		Endif	
		USE IN _lst_exc2
	ELSE
		_thismthodsuccessdone = .f.	
	ENDIF

	SELECT dist FgChap,FgName FROM _lst_exc1 WHERE !EMPTY(FgChap) AND FgChap NOT in (SELECT cetsh FROM cetsh) INTO CURSOR _lst_exc2
	IF USED('_lst_exc2')	
		SELECT _lst_exc2
		IF RECCOUNT() > 0
			SCAN
				REPLACE ErrMsg WITH ErrLog.ErrMsg +;
					CHR(13)+'Unable to find Chap. No. '+IIF(EMPTY(STRTRAN(_lst_exc2.FgChap,'0','')),'<BLANK>',ALLTRIM(_lst_exc2.FgChap)) + ;
					' in ACES List for Item : '+ALLTRIM(_lst_exc2.FgName) IN ErrLog
			Endscan		
			_thismthodsuccessdone = .f.	
		Endif	
		USE IN _lst_exc2
	ELSE
		_thismthodsuccessdone = .f.	
	ENDIF

	SELECT dist a.FgChap,a.FgUom as UOM_Cur,b.RateUnit as UOM_Org FROM _lst_exc1 a,cetsh b WHERE ;
		ALLTRIM(a.FgChap) == ALLTRIM(b.cetsh) ;
		AND a.FgUom NOT in (SELECT Dist RateUnit FROM cetsh) INTO CURSOR _lst_exc2
	IF USED('_lst_exc2')	
		SELECT _lst_exc2
		IF RECCOUNT() > 0
			SCAN
				IF !(UPPER(ALLTRIM(_lst_exc2.UOM_Cur)) == UPPER(ALLTRIM(_lst_exc2.UOM_Org)))
					REPLACE ErrMsg WITH ErrLog.ErrMsg +;
						CHR(13) + 'Current Value : '+IIF(EMPTY(_lst_exc2.UOM_Cur),'<BLANK>',ALLTRIM(_lst_exc2.UOM_Cur)) IN ErrLog
					REPLACE ErrMsg WITH ErrLog.ErrMsg +;
						' --> '+'UOM for the Chap. No. '+ALLTRIM(_lst_exc2.FgChap)+' should be '+ALLTRIM(_lst_exc2.UOM_Org) IN ErrLog
					_thismthodsuccessdone = .f.	
				Endif		
			Endscan		
		Endif	
		USE IN _lst_exc2
	ELSE
		_thismthodsuccessdone = .f.	
	ENDIF

	USE IN _lst_exc1
ELSE
	_thismthodsuccessdone = .f.	
ENDIF

IF !EMPTY(_oldAlias)
	SELECT (_oldAlias)
Endif	
RETURN _thismthodsuccessdone


