&&vasant041209
PROCEDURE GenInptMask
LPARAMETERS _vougencont
_vougeninptmask = ''
_vougenfldnm = JUSTEXT(_vougencont)
_vougentblnm = JUSTSTEM(_vougencont)
IF !EMPTY(_vougentblnm) AND !EMPTY(_vougenfldnm)
	_vougenalias = ALIAS()
	SELECT &_vougenfldnm as fld_nm FROM &_vougentblnm INTO CURSOR _vougentmptbl
	IF USED('_vougentmptbl')
		=AFIELDS(_vougenarr)
		DO Case
		Case TYPE('fld_nm') = 'N'
			_vougeninptmask = REPLICATE('9',_vougenarr(1,3)-(_vougenarr(1,4)*2))+IIF(_vougenarr(1,4) > 0,'.'+REPLICATE('9',_vougenarr(1,4)),'')
		ENDCASE
		USE IN ('_vougentmptbl')
	Endif
	IF !EMPTY(_vougenalias)
		SELECT (_vougenalias)
	Endif	
ENDIF
RETURN _vougeninptmask


PROCEDURE GenDynInptMask
LPARAMETERS _vougencont
_vougeninptmask = ''
_vougenfldnm = JUSTEXT(_vougencont)
_vougentblnm = JUSTSTEM(_vougencont)
IF !EMPTY(_vougentblnm) AND !EMPTY(_vougenfldnm)
	_vougenalias = ALIAS()
	SELECT &_vougenfldnm as fld_nm FROM &_vougentblnm INTO CURSOR _vougentmptbl
	IF USED('_vougentmptbl')
		=AFIELDS(_vougenarr)
		DO Case
		Case TYPE('fld_nm') = 'N'
			_vougeninptmask = REPLICATE('9',_vougenarr(1,3)-(_vougenarr(1,4)*2))+IIF(_vougenarr(1,4) > 0,'.'+REPLICATE('9',_vougenarr(1,4)),'')
		ENDCASE
		USE IN ('_vougentmptbl')
	Endif
	IF !EMPTY(_vougenalias)
		SELECT (_vougenalias)
	Endif	
ENDIF
RETURN _vougeninptmask
&&vasant041209