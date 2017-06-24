*!*	If (_Screen.ActiveForm.PCVTYPE="CT")
*!*		replace  main_vw.u_sr WITH item_vw.u_SRNO 
*!*	endif	
If (_Screen.ActiveForm.PCVTYPE="PH")
	replace item_vw.u_exchange WITH main_vw.u_exchange
ENDIF
