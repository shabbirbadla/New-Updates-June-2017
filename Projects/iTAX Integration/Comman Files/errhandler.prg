*!*	*!*	on error do errHandler with	error(), message(), message(1), program(), lineno()
LPARAMETER merror, cmess, mess1, mprog, mlineno
LOCAL lcMessage,lcErrormess,lcErrorNo
lcMessage = 'Error Number: ' + ALLT(TRANS(merror))+" "+'Message: '+ALLTRIM(cmess)
lcProgram = 'Program: ' + mprog
lcLineno  = 'Error Line No: '+TRANSFORM(mlineno)
_SCREEN.ACTIVEFORM.oUi2_libs.Ui2_error_log("P",lcProgram,lcLineno,lcMessage)
ENDPROC
