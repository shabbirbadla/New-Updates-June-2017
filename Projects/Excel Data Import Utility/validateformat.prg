Lparameters ccValue,nType
Local cPattern,oRE

oRE = Createobject("VBScript.RegExp")
Do Case
	Case nType = 1 		
		cPattern = '^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$'
Endcase
oRE.Pattern = cPattern
Return oRE.test(ccValue)
