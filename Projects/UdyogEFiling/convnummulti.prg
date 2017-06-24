LPARAMETERS _FldToConv,_FldWid,_DefaDec

_FldAddCnt = OCCURS(':',_FldToConv)
_FirstTot  = 0
_SecondTot = 0
_MaxDec	   = 0
FOR i1 = 1 to (_FldAddCnt + 1)
	_VarLst = subs(_FldToConv,1,IIF(AT(':',_FldToConv) > 0,AT(':',_FldToConv)-1,LEN(_FldToConv)))

	_FirstVal  = VAL(IIF(AT('.',_VarLst) > 0,SUBSTR(_VarLst,1,AT('.',_VarLst)-1),_VarLst))
	_SecondVal = ALLTRIM(IIF(AT('.',_VarLst) > 0,SUBSTR(_VarLst,AT('.',_VarLst)+1),''))
	_MaxDec		= IIF(_MaxDec < LEN(_SecondVal),LEN(_SecondVal),_MaxDec)
	_SecondVal = VAL(_SecondVal)

	_FirstTot = _FirstTot + _FirstVal
	_SecondTot = _SecondTot + _SecondVal
	_FldToConv = SUBSTR(_FldToConv,LEN(_VarLst)+2)
ENDFOR
_TotVal = ROUND(_FirstTot + (_SecondTot/VAL(PADR('1',_MaxDec+1,'0'))),_FldWid)

_FldToLen  = LEN(ALLTRIM(STR(_TotVal))) + 10
IF _DefaDec > _FldWid
	_FldWid = _DefaDec
Endif
_FldToConv = STR(_TotVal,_FldToLen,_FldWid)
RETURN _FldToConv