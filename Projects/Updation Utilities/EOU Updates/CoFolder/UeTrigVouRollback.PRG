_curvouobj = _Screen.ActiveForm
If Main_vw.Entry_ty="PT" And [vuexc] $ vchkprod And !Empty(Main_vw.U_RG23CNO)
	_uet_alias = ALIAS()
	SELECT Main_vw
	Mexamt = Main_vw.U_rg23cpay
	Mcessamt = Main_vw.U_rgcespay
	Mcvdamt = Main_vw.U_cvdpay
	Replace U_rg23cpay With 0,U_rgcespay WITH 0,U_cvdpay WITH 0 In Main_vw
	Replace Examt WITH Main_vw.Examt + Mexamt,;
		U_Cessamt WITH Main_vw.U_cessamt + Mcessamt,;
		U_Cvdamt WITH Main_vw.U_cvdamt + Mcvdamt IN Main_vw
	REPLACE Tot_Examt WITH Main_vw.Tot_Examt + (Mexamt+Mcessamt+Mcvdamt),;
		Tot_Add With Main_vw.Tot_Add - (Mexamt+Mcessamt+Mcvdamt) IN Main_vw

	If _curvouobj.AccountPage = .t.
		_curvouobj.AccountsPosting
	Endif
	IF !EMPTY(_uet_alias)
		SELECT (_uet_alias) 
	ENDIF
ENDIF

