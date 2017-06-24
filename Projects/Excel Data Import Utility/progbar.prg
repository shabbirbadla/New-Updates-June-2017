Define Class MainProgressBar As Custom
	MainFrm = ''

	Function ProgressBarExec
	Parameters MValue
	If MValue > 100
		This.CleaProgressBar()
	Else
		If This.MainFrm.showProgress = .F.
			This.MainFrm.ctl32_progressbar1.Value = This.MainFrm.ctl32_progressbar1.Value + MValue
			For a=1 To 100000
			Endfor
			This.MainFrm.Refresh()
		Endif
	Endif
	Endfunc

	Function SetStatus
	Parameters lcMsg
	If !Empty(lcMsg)
		This.MainFrm.olesbar.simpleText=lcMsg
		This.MainFrm.olesbar.Refresh()
	Endif

	Endfunc
	Function CleaProgressBar
	If Type('This.MainFrm') = 'O'
		This.MainFrm.showProgress = .T.
		This.MainFrm.Release
	Endif
	Endfunc

	Function ShowPBar
	If Type('This.MainFrm') = 'O'
		This.MainFrm.Visible = .T.
	Endif
	Endfunc

Enddefine
