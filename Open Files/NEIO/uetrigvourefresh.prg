_Malias 	= Alias()
_mRecNo 	= Recno()
_curvouobj = _Screen.ActiveForm
&&vasant061009
If Type('_curvouobj.mainalias') = 'C'
	If Upper(_curvouobj.mainalias) <> 'MAIN_VW'
		Return
	Endif
Endif

&&vasant061009
*SET datasession to _curvouobj.datasessionid	&&vasant071009
If Type('_curvouobj.PcvType') = 'C'
***-->Code is used to give 50% accounting effect for Capital Goods in Purchase Entry. &&Rup.
*!*		If main_vw.entry_ty="PT" And Type('_Screen.Activeform.HowtoCalculateExamt') = 'C' And [vuexc] $ vchkprod
*!*		If main_vw.entry_ty="PT" And Type('_Screen.Activeform.HowtoCalculateExamt') = 'C' And (  ([vuexc] $ vchkprod) OR ([vuser] $ vchkprod)  ) &&Rup 03Sep09 && Commented by Shrikant S. on 27/05/2010 for TKT-2043
*!*		If Inlist(main_vw.entry_ty,"PT","P1") And Type('_Screen.Activeform.HowtoCalculateExamt') = 'C' And (  ([vuexc] $ vchkprod) Or ([vuser] $ vchkprod)  ) && Changed by Shrikant S. on 27/05/2010 for TKT-2043
	If Inlist(main_vw.entry_ty,"PT","P1") And Type('_Screen.Activeform.HowtoCalculateExamt') = 'C' And (  ([vuexc] $ vchkprod) Or ([vuser] $ vchkprod)  ) And !([vutex] $ vchkprod) && Changed by Sachin N. S. on 13/01/2011 for Visual Udyog 10.0
		If !Empty(main_vw.U_RG23CNO) And Empty(main_vw.U_RG23NO) And ((_Screen.ActiveForm.Editmode = .T. And _Screen.ActiveForm.Saveflag = .F.) Or (_Screen.ActiveForm.Addmode = .F. And _Screen.ActiveForm.Editmode = .F.))
			Livalias = Alias()

			msqlstr = "select * from ac_mast where 1=2"
			mret = 0
			mret = _Screen.ActiveForm.sqlconobj.dataconn("EXE",company.dbname,msqlstr,"ac_mast","_screen.activeform.nHandle",_Screen.ActiveForm.DataSessionId,.F.)
			If nRetval<0
				Return .F.
			Endif
			mCAc_nm  =Padr("BALANCE WITH EXCISE RG23C",Len(Ac_mast.Ac_name))
			mCAc_nmC =Padr("EXCISE CAPITAL GOODS PAYABLE A/C",Len(Ac_mast.Ac_name))
			mCCAc_nm =Padr("BALANCE WITH CESS SURCHARGE RG23C",Len(Ac_mast.Ac_name))
			mCCAc_nmC=Padr("CESS CAPITAL GOODS PAYABLE A/C",Len(Ac_mast.Ac_name))
			mCHAc_nm =Padr("BALANCE WITH HCESS RG23C",Len(Ac_mast.Ac_name))
			mCHAc_nmC=Padr("H CESS CAPITAL GOODS PAYABLE A/C",Len(Ac_mast.Ac_name))
			mCBAc_nm =Padr("BALANCE WITH BCD RG23C",Len(Ac_mast.Ac_name))
			mCBAc_nmC=Padr("BCD CAPITAL GOODS PAYABLE A/C",Len(Ac_mast.Ac_name))
			mCVAc_nm  =Padr("BALANCE WITH CVD RG23C",Len(Ac_mast.Ac_name))
			mCVAc_nmC =Padr("CVD CAPITAL GOODS PAYABLE A/C",Len(Ac_mast.Ac_name))
			If _Screen.ActiveForm.HowtoCalculateExamt = [I]
				Select item_vw
				LivRec  = Iif(!Eof(),Recno(),0)
				Mexamt  = 0
				MCesamt = 0
				MHcesamt = 0
				Mbcdamt = 0
				Mcvdamt =0
				Loca
				Do While !Eof()
					Mexamt  = Mexamt + item_vw.Examt
					MCesamt = MCesamt+ item_vw.U_cessamt
					MHcesamt = MHcesamt+ item_vw.U_hcesamt
					Mbcdamt = Mbcdamt+ item_vw.bcdamt
					Mcvdamt = Mcvdamt+ item_vw.u_cvdamt
					If !Eof()
						Skip
					Endif
				Enddo
				If LivRec > 0
					Go LivRec
				Endif
			Else
				Mexamt  = main_vw.Examt 	+ main_vw.U_RG23CPAY
				MCesamt = main_vw.U_cessamt + main_vw.U_RGCESPAY
				MHcesamt = main_vw.U_hcesamt + main_vw.U_HCESPAY
				Mbcdamt = main_vw.bcdamt + main_vw.BCDPAY
				Mbcdamt = main_vw.u_cvdamt + main_vw.CVDPAY
			Endif
			If _Screen.ActiveForm.Editmode = .T. And _Screen.ActiveForm.Saveflag = .F.
				Select acdet_vw
				Loca For Inli(Ac_name,mCAc_nmC,mCCAc_nmC,mCHAc_nmC,mCBAc_nmC,mCVAc_nmC)
				If Found()
					Select main_vw
					Replace	Tot_examt With 0 In main_vw
					If Mexamt # 0
						Replace Examt With Mexamt,U_RG23CPAY With 0 In main_vw
						Sele acdet_vw
						Loca For Ac_name = mCAc_nm
						If Found()
							Repla Amount With main_vw.Examt In acdet_vw
						Endif
					Endif
					Select main_vw
					If MCesamt  # 0
						Replace U_cessamt  With MCesamt,U_RGCESPAY With 0 In main_vw
						Sele acdet_vw
						Loca For Ac_name = mCCAc_nm
						If Found()
							Repla Amount With main_vw.U_cessamt In acdet_vw
						Endif
					Endif
					Select main_vw
					If MHcesamt  # 0
						Replace U_hcesamt  With MHcesamt,U_HCESPAY With 0 In main_vw
						Sele acdet_vw
						Loca For Ac_name = mCHAc_nm
						If Found()
							Repla Amount With main_vw.U_hcesamt In acdet_vw
						Endif
					Endif
					Select main_vw
					If Mbcdamt  # 0
						Replace bcdamt  With Mbcdamt,BCDPAY With 0 In main_vw
						Sele acdet_vw
						Loca For Ac_name = mCBAc_nm
						If Found()
							Repla Amount With main_vw.bcdamt In acdet_vw
						Endif
					Endif
					Select main_vw
					If Mcvdamt  # 0
						Replace u_cvdamt  With Mcvdamt,CVDPAY With 0 In main_vw
						Sele acdet_vw
						Loca For Ac_name = mCVAc_nm
						If Found()
							Repla Amount With main_vw.u_cvdamt In acdet_vw
						Endif
					Endif
					Sele acdet_vw
					Dele For Inli(Ac_name,mCAc_nmC,mCCAc_nmC,mCHAc_nmC,mCBAc_nmC,mCVAc_nmC) In acdet_vw
					Select main_vw
					Replace Tot_examt With main_vw.Examt+main_vw.U_cessamt+main_vw.U_hcesamt+main_vw.bcdamt+main_vw.u_cvdamt In main_vw
				Endif
			Endif
			If _Screen.ActiveForm.Addmode = .F. And _Screen.ActiveForm.Editmode = .F.
				Replace Examt With Mexamt-main_vw.U_RG23CPAY,U_cessamt With MCesamt-main_vw.U_RGCESPAY,U_hcesamt With MHcesamt-main_vw.U_HCESPAY,bcdamt With Mbcdamt-main_vw.BCDPAY,u_cvdamt With Mcvdamt-main_vw.CVDPAY In main_vw
				Replace Tot_examt With main_vw.Examt+main_vw.U_cessamt+main_vw.U_hcesamt+main_vw.bcdamt+main_vw.u_cvdamt+main_vw.U_RG23CPAY + main_vw.U_RGCESPAY +main_vw.U_HCESPAY+main_vw.BCDPAY+main_vw.CVDPAY In main_vw
			Endif
		Endif
	Endif

****** Added By Sachin N. S. on 04/07/2011 for Batchwise/Serialize Inventory ****** Start
	If Type('_curvouobj._BatchSerialStk')='O'
		_curvouobj._BatchSerialStk._ueTrigVouRefresh()
	Endif
****** Added By Sachin N. S. on 04/07/2011 for Batchwise/Serialize Inventory ****** End

Endif
If !Empty(_Malias)
	Select &_Malias
Endif
If Betw(_mRecNo,1,Reccount())
	Go _mRecNo
Endif
***<---Code is used to give 50% accounting effect for Capital Goods in Purchase Entry. &&Rup.
