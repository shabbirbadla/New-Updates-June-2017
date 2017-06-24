PARAMETERS IsFirstFlag, IsPrevFlag, IsNextFlag, IsLastFlag,;
	IsFindFlag, IsLocFlag,;
	IsNewFlag, IsEditFlag,;
	IsDelFlag,;
	IsCopyFlag,;
	IsViewFlag,;
	IsPrintFlag,;
	IsCloseFlag,;
	IsHelpFlag


WITH  tbrDeskTop
	.btnTop.ENABLED  = IsFirstFlag
	.btnPrv.ENABLED  = IsPrevFlag
	.btnNxt.ENABLED  = IsNextFlag
	.btnBtm.ENABLED  = IsLastFlag
	.btnFind.ENABLED = IsFindFlag
	.btnLoc.ENABLED  = IsLocFlag
	.btnNew.ENABLED  = IsNewFlag
	.btnModi.ENABLED = IsEditFlag
	.btnDele.ENABLED = IsDelFlag
	.btnCopy.ENABLED = IsCopyFlag
	.btnView.ENABLED = IsViewFlag
	.btnPrin.ENABLED = IsPrintFlag
	.btnHelp.ENABLED = IsHelpFlag
	.btnExit.ENABLED = IsCloseFlag
ENDWITH
RETURN
