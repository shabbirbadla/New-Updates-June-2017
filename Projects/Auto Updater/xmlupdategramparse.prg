LPARAMETERS _lcXML,_lcipadd
LOCAL lcTagName
lnOldMemoWidth = SET("MEMOWIDTH")
SET MEMOWIDTH TO 255
lnLines = ALINES(aXML,_lcXML)
FOR n = 1 TO lnLines
	lcTagName = aXML(n)
	IF UPPER('net.tcp:') $ UPPER(lcTagName)
		_lctagstart = AT('"',lcTagName,3)
		_lctagend = AT('"',lcTagName,4)
		IF _lctagstart > 0 AND _lctagend > 0
			_lctagstart = _lctagstart + 1
			_lctagend = _lctagend 
			_lctagold   = SUBSTR(lcTagName,_lctagstart,_lctagend-_lctagstart)
			aXML(n) = STRTRAN(aXML(n),_lctagold,'net.tcp://'+_lcipadd+':8085/LicenseService')
		Endif	
	ENDIF
	REPLACE IpAddress WITH _regip.IpAddress + aXML(n)+CHR(13) IN _regip
NEXT
IF VARTYPE(lnOldMemoWidth) = "N" AND NOT EMPTY(lnOldMemoWidth)
   SET MEMOWIDTH TO lnOldMemoWidth
ENDIF
RETURN
