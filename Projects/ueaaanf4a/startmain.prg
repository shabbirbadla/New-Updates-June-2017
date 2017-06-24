LPARAMETERS tnrange AS INTEGER

IF VARTYPE(VuMess) <> 'C'
	MESSAGEBOX('Internal Application Not Run Directly...',0+48,[])
	QUIT
	RETURN .F.
ENDIF

DO FORM frmaaanf4a WITH tnrange
