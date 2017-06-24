DEFINE CLASS MainProgressBar AS CUSTOM
	MainFrm = ''

	FUNCTION ProgressBarExec
	PARAMETERS MValue
	IF MValue > 100
		THIS.CleaProgressBar()
	ELSE
		IF THIS.MainFrm.showProgress = .F.
			THIS.MainFrm.ctl32_progressbar1.VALUE = THIS.MainFrm.ctl32_progressbar1.VALUE + MValue
			FOR a=1 TO 100000
			ENDFOR
			THIS.MainFrm.REFRESH()
		ENDIF
	ENDIF
	ENDFUNC

	FUNCTION CleaProgressBar
	IF TYPE('This.MainFrm') = 'O'
		THIS.MainFrm.showProgress = .T.
		THIS.MainFrm.RELEASE
	ENDIF
	ENDFUNC

	FUNCTION ShowPBar
	IF TYPE('This.MainFrm') = 'O'
		THIS.MainFrm.VISIBLE = .T.
	ENDIF
	ENDFUNC

ENDDEFINE
