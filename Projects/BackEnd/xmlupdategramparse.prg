Lparameters _lcXML,_lcipadd,_prodCode		&& Changed by Sachin N. S. on 06/12/2016 for GST Vudyog Database Working
*!*	LPARAMETERS _lcXML,_lcipadd
Local lcTagName
lnOldMemoWidth = Set("MEMOWIDTH")
Set Memowidth To 255
lnLines = Alines(aXML,_lcXML)
For N = 1 To lnLines
	lcTagName = aXML(N)
	If Upper('net.tcp:') $ Upper(lcTagName)
		_lctagstart = At('"',lcTagName,3)
		_lctagend = At('"',lcTagName,4)
		If _lctagstart > 0 And _lctagend > 0
			_lctagstart = _lctagstart + 1
			_lctagend = _lctagend
			_lctagold   = Substr(lcTagName,_lctagstart,_lctagend-_lctagstart)
			aXML(N) = Strtran(aXML(N),_lctagold,'net.tcp://'+_lcipadd+':8085/LicenseService')
		Endif
	Endif
	If Upper('"UdyogServiceName"') $ Upper(lcTagName)
		_lctagstart = At('"',lcTagName,3)
		_lctagend = At('"',lcTagName,4)
		If _lctagstart > 0 And _lctagend > 0
			_lctagstart = _lctagstart + 1
			_lctagend = _lctagend
			_lctagold   = Substr(lcTagName,_lctagstart,_lctagend-_lctagstart)
			aXML(N) = Strtran(aXML(N),_lctagold,'Udyog Application License Manager '+_prodCode)
		Endif
	Endif
	Replace IpAddress With _regip.IpAddress + aXML(N)+Chr(13) In _regip
Next
If Vartype(lnOldMemoWidth) = "N" And Not Empty(lnOldMemoWidth)
	Set Memowidth To lnOldMemoWidth
Endif
Return
