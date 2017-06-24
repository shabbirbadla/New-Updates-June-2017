Parameters vDataSessionId
Local EXPARA

EXPARA=' '
x=""

Set DataSession To vDataSessionId

*!*	If Type("_rstatus.isrule")="L"
*!*		If 	_rstatus.isrule=.T.
*!*			Do FORM uefrm_RuleFilter.scx WITH vDataSessionId To x
*!*		ENDIF
*!*	ENDIF
*!*	EXPARA=x

Replace _rstatusclonesex.xTraParam With "'"+EXPARA+"'"

