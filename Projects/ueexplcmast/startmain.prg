LPARAMETERS tnrange AS INTEGER
*!*	,pdeptran AS STRING  &&Commented by Priyanka on 17012014

IF VARTYPE(VuMess) <> 'C'
	MESSAGEBOX('Internal Application Not Run Directly...',0+48,[])
	QUIT
	RETURN .F.
ENDIF

*!*	DO FORM frmexplcmast WITH pdeptran,tnrange  &&Commented by Priyanka on 17012014
DO FORM frmexplcmast WITH tnrange  &&Added by Priyanka on 17012014
