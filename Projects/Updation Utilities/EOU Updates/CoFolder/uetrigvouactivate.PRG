If (_Screen.ActiveForm.PCVTYPE="PT")
	IF "VOUCLASS" $ UPPER(SET('classlib'))
	ELSE
		SET CLASSLIB TO vouclass ADDITIVE
	ENDIF
	WITH _screen.ActiveForm
		.cmdainfo.TOP=.cmdainfo.TOP+40
		.cmdnarration.top=.cmdainfo.top+40
		.cmdexdetail.HEIGHT=.cmdainfo.HEIGHT
		.cmdexdetail.WIDTH=.cmdnarration.WIDTH	&&+30
		.cmdexdetail.TOP = .cmdainfo.TOP
		.cmdexdetail.LEFT = .cmdainfo.LEFT+100 &&-.cmdexdetail.width-5
		.cmdnarration.LEFT = .cmdainfo.LEFT
	endwith
	
endif

