LPARAMETERS tctype as String,tnrange as String

If Vartype(VuMess) <> 'C'
	Messagebox('Internal Application Not Run Directly...',0+48,[])
	Quit
	Return .F.
Endif

DO FORM frmdbkmast WITH tnrange
