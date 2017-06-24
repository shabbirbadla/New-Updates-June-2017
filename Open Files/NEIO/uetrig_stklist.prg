Parameters vDataSessionId
Local EXPARA
EXPARA=' '
yesno = 0
yesno = Messagebox ('Do you want to show Items having NIL balance?',4+32,vumess)

If yesno = 6
	EXPARA=EXPARA+'$'+'ANS=YES'
Else
	EXPARA=EXPARA+'$'+'ANS=NO'
Endif
x=""

Set DataSession To vDataSessionId

*!*	If Type("_rstatus.isrule")="L"
*!*		If 	_rstatus.isrule=.T.
*!*			Do FORM uefrm_RuleFilter.scx WITH vDataSessionId To x
*!*		ENDIF
*!*	ENDIF
EXPARA=EXPARA+x

Replace _rstatusclonesex.xTraParam With "'"+EXPARA+"'"

