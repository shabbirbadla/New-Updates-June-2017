_malias 	= Alias()
_mrecno 	= Recno()
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

&& Commented by Shrikant S. on 28/09/2016 for GST		&& Start
*!*	*!*		If Inlist(main_vw.entry_ty,"PT","P1") And Type('_Screen.Activeform.HowtoCalculateExamt') = 'C' And (  ([vuexc] $ vchkprod) Or ([vuser] $ vchkprod)  ) And !([vutex] $ vchkprod) && Changed by Sachin N. S. on 13/01/2011 for Visual Udyog 10.0
*!*	*!*			If !Empty(main_vw.u_rg23cno) And Empty(main_vw.u_rg23no) And ((_Screen.ActiveForm.editmode = .T. And _Screen.ActiveForm.saveflag = .F.) Or (_Screen.ActiveForm.addmode = .F. And _Screen.ActiveForm.editmode = .F.))
*!*	*!*				livalias = Alias()

*!*	*!*				msqlstr = "select * from ac_mast where 1=2"
*!*	*!*				mret = 0
*!*	*!*				mret = _Screen.ActiveForm.sqlconobj.dataconn("EXE",company.dbname,msqlstr,"ac_mast","_screen.activeform.nHandle",_Screen.ActiveForm.DataSessionId,.F.)
*!*	*!*				If nretval<0
*!*	*!*					Return .F.
*!*	*!*				Endif
*!*	*!*				mcac_nm  =Padr("BALANCE WITH EXCISE RG23C",Len(ac_mast.ac_name))
*!*	*!*				mcac_nmc =Padr("EXCISE CAPITAL GOODS PAYABLE A/C",Len(ac_mast.ac_name))
*!*	*!*				mccac_nm =Padr("BALANCE WITH CESS SURCHARGE RG23C",Len(ac_mast.ac_name))
*!*	*!*				mccac_nmc=Padr("CESS CAPITAL GOODS PAYABLE A/C",Len(ac_mast.ac_name))
*!*	*!*				mchac_nm =Padr("BALANCE WITH HCESS RG23C",Len(ac_mast.ac_name))
*!*	*!*				mchac_nmc=Padr("H CESS CAPITAL GOODS PAYABLE A/C",Len(ac_mast.ac_name))
*!*	*!*				mcbac_nm =Padr("BALANCE WITH BCD RG23C",Len(ac_mast.ac_name))
*!*	*!*				mcbac_nmc=Padr("BCD CAPITAL GOODS PAYABLE A/C",Len(ac_mast.ac_name))
*!*	*!*				mcvac_nm  =Padr("BALANCE WITH CVD RG23C",Len(ac_mast.ac_name))
*!*	*!*				mcvac_nmc =Padr("CVD CAPITAL GOODS PAYABLE A/C",Len(ac_mast.ac_name))
*!*	*!*				If _Screen.ActiveForm.howtocalculateexamt = [I]
*!*	*!*					Select item_vw
*!*	*!*					livrec  = Iif(!Eof(),Recno(),0)
*!*	*!*					mexamt  = 0
*!*	*!*					mcesamt = 0
*!*	*!*					mhcesamt = 0
*!*	*!*					mbcdamt = 0
*!*	*!*					mcvdamt =0
*!*	*!*					Loca
*!*	*!*					Do While !Eof()
*!*	*!*						mexamt  = mexamt + item_vw.examt
*!*	*!*						mcesamt = mcesamt+ item_vw.u_cessamt
*!*	*!*						mhcesamt = mhcesamt+ item_vw.u_hcesamt
*!*	*!*						mbcdamt = mbcdamt+ item_vw.bcdamt
*!*	*!*						mcvdamt = mcvdamt+ item_vw.u_cvdamt
*!*	*!*						If !Eof()
*!*	*!*							Skip
*!*	*!*						Endif
*!*	*!*					Enddo
*!*	*!*					If livrec > 0
*!*	*!*						Go livrec
*!*	*!*					Endif
*!*	*!*				Else
*!*	*!*					mexamt  = main_vw.examt 	+ main_vw.u_rg23cpay
*!*	*!*					mcesamt = main_vw.u_cessamt + main_vw.u_rgcespay
*!*	*!*					mhcesamt = main_vw.u_hcesamt + main_vw.u_hcespay
*!*	*!*					mbcdamt = main_vw.bcdamt + main_vw.bcdpay
*!*	*!*					mbcdamt = main_vw.u_cvdamt + main_vw.cvdpay
*!*	*!*				Endif
*!*	*!*				If _Screen.ActiveForm.editmode = .T. And _Screen.ActiveForm.saveflag = .F.
*!*	*!*					Select acdet_vw
*!*	*!*					Loca For Inli(ac_name,mcac_nmc,mccac_nmc,mchac_nmc,mcbac_nmc,mcvac_nmc)
*!*	*!*					If Found()
*!*	*!*						Select main_vw
*!*	*!*						Replace	tot_examt With 0 In main_vw
*!*	*!*						If mexamt # 0
*!*	*!*							Replace examt With mexamt,u_rg23cpay With 0 In main_vw
*!*	*!*							Sele acdet_vw
*!*	*!*							Loca For ac_name = mcac_nm
*!*	*!*							If Found()
*!*	*!*								Repla amount With main_vw.examt In acdet_vw
*!*	*!*							Endif
*!*	*!*						Endif
*!*	*!*						Select main_vw
*!*	*!*						If mcesamt  # 0
*!*	*!*							Replace u_cessamt  With mcesamt,u_rgcespay With 0 In main_vw
*!*	*!*							Sele acdet_vw
*!*	*!*							Loca For ac_name = mccac_nm
*!*	*!*							If Found()
*!*	*!*								Repla amount With main_vw.u_cessamt In acdet_vw
*!*	*!*							Endif
*!*	*!*						Endif
*!*	*!*						Select main_vw
*!*	*!*						If mhcesamt  # 0
*!*	*!*							Replace u_hcesamt  With mhcesamt,u_hcespay With 0 In main_vw
*!*	*!*							Sele acdet_vw
*!*	*!*							Loca For ac_name = mchac_nm
*!*	*!*							If Found()
*!*	*!*								Repla amount With main_vw.u_hcesamt In acdet_vw
*!*	*!*							Endif
*!*	*!*						Endif
*!*	*!*						Select main_vw
*!*	*!*						If mbcdamt  # 0
*!*	*!*							Replace bcdamt  With mbcdamt,bcdpay With 0 In main_vw
*!*	*!*							Sele acdet_vw
*!*	*!*							Loca For ac_name = mcbac_nm
*!*	*!*							If Found()
*!*	*!*								Repla amount With main_vw.bcdamt In acdet_vw
*!*	*!*							Endif
*!*	*!*						Endif
*!*	*!*						Select main_vw
*!*	*!*						If mcvdamt  # 0
*!*	*!*							Replace u_cvdamt  With mcvdamt,cvdpay With 0 In main_vw
*!*	*!*							Sele acdet_vw
*!*	*!*							Loca For ac_name = mcvac_nm
*!*	*!*							If Found()
*!*	*!*								Repla amount With main_vw.u_cvdamt In acdet_vw
*!*	*!*							Endif
*!*	*!*						Endif
*!*	*!*						Sele acdet_vw
*!*	*!*						Dele For Inli(ac_name,mcac_nmc,mccac_nmc,mchac_nmc,mcbac_nmc,mcvac_nmc) In acdet_vw
*!*	*!*						Select main_vw
*!*	*!*						Replace tot_examt With main_vw.examt+main_vw.u_cessamt+main_vw.u_hcesamt+main_vw.bcdamt+main_vw.u_cvdamt In main_vw
*!*	*!*					Endif
*!*	*!*				Endif
*!*	*!*				If _Screen.ActiveForm.addmode = .F. And _Screen.ActiveForm.editmode = .F.
*!*	*!*					Replace examt With mexamt-main_vw.u_rg23cpay,u_cessamt With mcesamt-main_vw.u_rgcespay,u_hcesamt With mhcesamt-main_vw.u_hcespay,bcdamt With mbcdamt-main_vw.bcdpay,u_cvdamt With mcvdamt-main_vw.cvdpay In main_vw
*!*	*!*					Replace tot_examt With main_vw.examt+main_vw.u_cessamt+main_vw.u_hcesamt+main_vw.bcdamt+main_vw.u_cvdamt+main_vw.u_rg23cpay + main_vw.u_rgcespay +main_vw.u_hcespay+main_vw.bcdpay+main_vw.cvdpay In main_vw
*!*	*!*				Endif
*!*	*!*			Endif
*!*	*!*		Endif
&& Commented by Shrikant S. on 28/09/2016 for GST		&& End


****** Added By Sachin N. S. on 13/02/2012 for TKT-9411 and Bug-660 Batchwise/Serialize Inventory ****** Start
	If Type('_curvouobj._BatchSerialStk')='O'
		_curvouobj._batchserialstk._uetrigvourefresh()
	Endif
****** Added By Sachin N. S. on 13/02/2012 for TKT-9411 and Bug-660 Batchwise/Serialize Inventory ****** End
Endif

*Birendra : Auto Debit and Credit Note on 18 july 2011 :Start:
*----------------------------------------For Auto Debit note --------------------------------------------
*!*	IF INLIST(main_vw.entry_ty,'D1','D2','D3','D4','D5','C2','C3','C4','C5')		&& Commented by Shrikant S. on 17/03/2012 for Bug-2276
If Inlist(main_vw.entry_ty,'D2','D3','D4','D5','C2','C3','C4','C5')			&& Added by Shrikant S. on 17/03/2012 for Bug-2276
	With _Screen.ActiveForm
		tot_grd_col=.voupage.page1.grditem.ColumnCount
		For i = 1 To tot_grd_col

			Do Case
			Case .voupage.page1.grditem.Columns(i).header1.Caption='Ass. Value'
				.voupage.page1.grditem.Columns(i).Visible=.F.

			Case .voupage.page1.grditem.Columns(i).header1.Caption='Rate'
				.voupage.page1.grditem.Columns(i).Visible=.F.

			Case .voupage.page1.grditem.Columns(i).header1.Caption='Quantity'
*			 		.voupage.page1.grditem.columns(i).visible=IIF(INLIST(main_vw.dept,'LATE PAYMENT','FORWARDING'),.F.,.T.)
				.voupage.page1.grditem.Columns(i).Visible=Iif(Inlist(main_vw.entry_ty,'D2','D3','C2','C3'),.F.,.T.)

			Case .voupage.page1.grditem.Columns(i).header1.Caption='Charges'
*			 		.voupage.page1.grditem.columns(i).visible=IIF(INLIST(main_vw.dept,'LATE PAYMENT'),.f.,.t.)
				.voupage.page1.grditem.Columns(i).Visible=Iif(Inlist(main_vw.entry_ty,'D3','C3'),.F.,.T.)

			Case .voupage.page1.grditem.Columns(i).header1.Caption='Sale Bill No.'
				.voupage.page1.grditem.Columns(i).Width=70

			Case .voupage.page1.grditem.Columns(i).header1.Caption='Item Name'
				.voupage.page1.grditem.Columns(i).Width=120
				.voupage.page1.grditem.Columns(i).Visible=Iif(Inlist(main_vw.entry_ty,'D2','D3','D4','D5','C2','C3','C4','C5'),.F.,.T.)	&& Added By Shrikant S. on 04/01/2013 for Bug-7269
*!*						.voupage.page1.grditem.COLUMNS(i).VISIBLE=IIF(INLIST(main_vw.entry_ty,'D2','D3','D4','C2','C3','C4'),.F.,.T.)		&& Commented By Shrikant S. on 04/01/2013 for Bug-7269


*			   CASE .voupage.page1.grditem.columns(i).header1.caption='Payment Recd. Date'
			Case .voupage.page1.grditem.Columns(i).header1.Caption=Iif(Inlist(main_vw.entry_ty,'C3'),'Payment Made Date','Payment Recd. Date')
				.voupage.page1.grditem.Columns(i).header1.Caption=Iif(Inlist(main_vw.entry_ty,'C3'),'Payment Made Date','Payment Recd. Date')
				.voupage.page1.grditem.Columns(i).Visible=Iif(Inlist(main_vw.entry_ty,'D3','C3'),.T.,.F.)

*			   CASE .voupage.page1.grditem.columns(i).header1.caption='Recd. Amount'
			Case .voupage.page1.grditem.Columns(i).header1.Caption=Iif(Inlist(main_vw.entry_ty,'C3'),'Made Amount','Recd. Amount')
				.voupage.page1.grditem.Columns(i).header1.Caption=Iif(Inlist(main_vw.entry_ty,'C3'),'Made Amount','Recd. Amount')
				.voupage.page1.grditem.Columns(i).Visible=Iif(Inlist(main_vw.entry_ty,'D3','C3'),.T.,.F.)

			Case .voupage.page1.grditem.Columns(i).header1.Caption='Late Days'
*			 		.voupage.page1.grditem.columns(i).visible=IIF(INLIST(main_vw.dept,'LATE PAYMENT'),.T.,.F.)
				.voupage.page1.grditem.Columns(i).Visible=Iif(Inlist(main_vw.entry_ty,'D3','C3'),.T.,.F.)

			Case .voupage.page1.grditem.Columns(i).header1.Caption='Interest %'
*			 		.voupage.page1.grditem.columns(i).visible=IIF(INLIST(main_vw.dept,'LATE PAYMENT'),.T.,.F.)
				.voupage.page1.grditem.Columns(i).Visible=Iif(Inlist(main_vw.entry_ty,'D3','C3'),.T.,.F.)

			Case .voupage.page1.grditem.Columns(i).header1.Caption='Interest Amount'
*			 		.voupage.page1.grditem.columns(i).visible=IIF(INLIST(main_vw.dept,'LATE PAYMENT'),.T.,.F.)
				.voupage.page1.grditem.Columns(i).Visible=Iif(Inlist(main_vw.entry_ty,'D3','C3'),.T.,.F.)

				.voupage.page1.grditem.Refresh

			Endcase

		Endfor
	Endwith
Endif
*Birendra : Auto Debit and Credit Note on 18 july 2011 :End:

&& Added by Shrikant S. 18/06/2012 for Bug-4744		&& Start
If Vartype(oGlblPrdFeat)='O'
	If oGlblPrdFeat.UdChkProd('openord')
		If Type('Lcode_vw.allowzeroqty')!='U'
			If Inlist(main_vw.entry_ty,'OO','TR')
				Do Case
				Case main_vw.entry_ty='OO' And Lcode_vw.allowzeroqty=.T.
					With _Screen.ActiveForm
						tot_grd_col=.voupage.page1.grditem.ColumnCount
						For i = 1 To tot_grd_col
							Do Case
							Case .voupage.page1.grditem.Columns(i).header1.Caption='Quantity'
								.voupage.page1.grditem.Columns(i).Visible=.F.
								.voupage.page1.grditem.Columns(i).ReadOnly=.T.
							Endcase
						Endfor
					Endwith

				Case  main_vw.entry_ty='TR'
					If Used('ITREF_VW')
*Select entry_ty,allowzeroqty From _lcodepickup Where entry_ty=Itref_vw.Rentry_ty Into Cursor _lcodezeroqty
						msqlstr = "Select entry_ty,allowzeroqty From Lcode Where entry_ty=?Itref_vw.Rentry_ty"
						mret = _Screen.ActiveForm.sqlconobj.dataconn("EXE",company.dbname,msqlstr,"_lcodezeroqty","_screen.activeform.nHandle",_Screen.ActiveForm.DataSessionId,.F.)
						If mret<0
							Return .F.
						Endif

						Select _lcodezeroqty
						If _lcodezeroqty.allowzeroqty==.T.
							With _Screen.ActiveForm
								tot_grd_col=.voupage.page1.grditem.ColumnCount
								For i = 1 To tot_grd_col
									Do Case
									Case .voupage.page1.grditem.Columns(i).header1.Caption='Quantity'
										.voupage.page1.grditem.Columns(i).ReadOnly=.T.
									Endcase
								Endfor
							Endwith
							Use In _lcodezeroqty
						Endif
					Endif
				Endcase
			Endif
		Endif
	Endif
Endif
&& Added by Shrikant S. 18/06/2012 for Bug-4744		&& End

&& Added by Shrikant S. on 06/10/2016 for GST		&& Start
If Type('_curvouobj.pcvtype')<>'U'
	If Inlist(_curvouobj.pcvtype,'E1','S1','IB','J6','BP','BR','CP','CR','C6','D6','RV')			&& Commented by Shrikant S. on 06/02/2017 for GST
*!*		If Inlist(_curvouobj.pcvtype,'E1','S1','IB','J6','BP','BR','CP','CR','GC','GD','C6','D6')				&& Added by Shrikant S. on 06/02/2017 for GST
*	If Type('_Screen.ActiveForm.voupage.page1.grditem')<>'U'
		With _curvouobj
			tot_grd_col=.voupage.page1.grditem.ColumnCount

			For i = 1 To tot_grd_col

				Do Case
				Case .voupage.page1.grditem.Columns(i).header1.Caption='Quantity'
					.voupage.page1.grditem.Columns(i).Visible=.F.
*!*						IF Inlist(_curvouobj.pcvtype,'GC','GD','C6','D6')
*!*							.voupage.page1.grditem.Columns(i).Visible=.t.
*!*							.voupage.page1.grditem.Columns(i).eNABLED=.F.
*!*						ENDIF
				Case .voupage.page1.grditem.Columns(i).header1.Caption='Rate'
					.voupage.page1.grditem.Columns(i).Visible=.F.
*!*						IF Inlist(_curvouobj.pcvtype,'GC','GD','C6','D6')
*!*							.voupage.page1.grditem.Columns(i).Visible=.t.
*!*						ENDIF

				Case  Upper(.voupage.page1.grditem.Columns(i).ControlSource)='ITEM_VW.U_ASSEAMT'
*!*						If Inlist(_Screen.ActiveForm.pcvtype,"IB","J6","E1","S1","C6","D6")			&& Commented by Shrikant S. on 06/02/2017 for GST
					If Inlist(_Screen.ActiveForm.pcvtype,"IB","J6")			&& ADDED by Shrikant S. on 09/02/2017 for GST
						.voupage.page1.grditem.Columns(i).Visible=.F.
					Endif
					If Inlist(_Screen.ActiveForm.pcvtype,"C6","D6")
						.voupage.page1.grditem.Columns(i).header1.Caption="Diff. Taxable Value"
						.voupage.page1.grditem.Columns(i).ReadOnly=.T.
						.voupage.page1.grditem.Columns(i).Width=110
					ENDIF
					If Inlist(_Screen.ActiveForm.pcvtype,"E1","S1")
						.voupage.page1.grditem.Columns(i).ReadOnly=.T.
					endif
				Endcase
			Endfor
		Endwith
*	Endif
	Endif
Endif
&& Added by Shrikant S. on 06/10/2016 for GST		&& End

&& Added by Shrikant S. on 23/02/2017 for GST		&& Start
If Type('_curvouobj.pcvtype')<>'U'
	If Inlist(_curvouobj.pcvtype,'GD','GC')
		With _Screen.ActiveForm
			tot_grd_col=.voupage.page1.grditem.ColumnCount
			For i = 1 To tot_grd_col
				Do Case
				Case Upper(.voupage.page1.grditem.Columns(i).ControlSource)='ITEM_VW.RATE'
					.voupage.page1.grditem.Columns(i).header1.Caption='Diff. Rate'
				Endcase
			Endfor
		Endwith
	Endif
Endif
&& Added by Shrikant S. on 23/02/2017 for GST		&& End


&& Added by Shrikant S. on 28/06/014 for Bug-23280		&& Start
If Vartype(oglblindfeat)='O'
	If oglblindfeat.udchkind('pharmaind')
		If Inlist(main_vw.entry_ty,'IP')
			If Type('_curvouobj.cmdbom')='O'
				_curvouobj.cmdbom.Enabled=.F.
			Endif
			Return .T.
		Endif
	Endif
Endif

&& Added by Shrikant S. on 28/06/014 for Bug-23280		&& end
If Vartype(oglblindfeat)='O'
	If oglblindfeat.udchkind('vugst')
		If Inlist(_curvouobj.pcvtype,'J5')
			_curvouobj.txtpartyname.Visible=.F.
			_curvouobj.cmdacsearch.Visible=.F.
			_curvouobj.lblpartyname.Visible=.F.
			_curvouobj.cmdConsName.Visible=.F.
			Return .T.
		Endif
	Endif
Endif

If !Empty(_malias)
	Select &_malias
Endif
If Betw(_mrecno,1,Reccount())
	Go _mrecno
Endif
***<---Code is used to give 50% accounting effect for Capital Goods in Purchase Entry. &&Rup.

*!*	** Start : Added by Uday on 26/12/2011 for Exim
_mexim = .F.                              && Added by Ajay Jaiswal on 22/02/2012 for EXIM
_mdbk = .F.                               && Added by Ajay Jaiswal on 03/04/2012 for DBK
_mexim = oGlblPrdFeat.UdChkProd('exim')   && Added by Ajay Jaiswal on 22/02/2012 for EXIM
_mdbk = oGlblPrdFeat.UdChkProd('dbk')     && Added by Ajay Jaiswal on 03/04/2012 for DBK
If _mexim  Or _mdbk                       && Added by Ajay Jaiswal on 21/02/2012 for EXIM & DBK
	If Type('_curvouobj.PcvType') = 'C'
		If main_vw.entry_ty = "OP"
			Local mobj
			mobj = _Screen.ActiveForm
			If mobj.addmode = .F. And mobj.editmode = .F.
				If Used([Op_PtTax_Vw])
					Do calldata With mobj,"E"
				Endif
			Else
				If mobj.addmode = .T. And mobj.editmode = .F.
					If Used([Op_PtTax_Vw])
						Do calldata With mobj,"A"
					Endif
				Endif
			Endif
			With mobj.voupage.pgappduties.grdappduties
				.Refresh()
			Endwith
		Endif
	Endif
Endif

*changes by EBS team on 07/03/14 for Bug-21466,21467,21468 start
* Below Changes done as per --> CR_KOEL_0002_Export_Sales_Transaction_&_Export_LC_Master_Cross_Reference
* Changes done by EBS Product Team
_mexim = oGlblPrdFeat.UdChkProd('exim')
_curscrobj = _Screen.ActiveForm
If _mexim
	If Inlist(main_vw.entry_ty,"SI")
		If _curscrobj.addmode = .F. And _curscrobj.editmode = .F.
			If Type("_curscrobj.oPrevInvAmt") = 'U'
				_curscrobj.AddProperty("oPrevInvAmt")
			Endif

			If Type("_curscrobj.oPrevInvAmt") <> 'U'
				_curscrobj.oPrevInvAmt = Iif(Isnull(main_vw.fcnet_amt),0,main_vw.fcnet_amt)
			Endif

&& Clears LC Balance Amount Form Property while in View Mode
			If Type("_curscrobj.oLCBalAmt") <> 'U'
				_curscrobj.oLCBalAmt = 0
			Endif

&& Stores LC No. Form Property while in View Mode
			If Type("_curscrobj.oLCNo") <> 'U'
				_curscrobj.oLCNo = Iif(Isnull(main_vw.lc_no),'',main_vw.lc_no)
			Endif

			If Type("_curscrobj.oLCUpdtFlag") <> 'U'
				_curscrobj.oLCUpdtFlag = .F.
			Endif

			If Type("_curscrobj.cntLcBalShow") <> 'U'
				_curscrobj.cntLcBalShow.Visible = .F.
			Endif

			If Type("_curscrobj.cntGrdShowMsg") <> 'U'
				_curscrobj.cntGrdShowMsg.Visible = .F.
			Endif

			If Used("curUpMyValues")
				Use In curUpMyValues
			Endif
		Else
			If _curscrobj.addmode = .T. Or _curscrobj.editmode = .T.

&& Clears Property value in Add Mode
				If _curscrobj.addmode = .T.
					If Type("_curscrobj.oLCNo") <> 'U'
						_curscrobj.oLCNo = ""
					Endif
				Endif

				If !Empty(main_vw.lc_no)
					If Type("_curscrobj.oLCBalAmt") <> 'U'
						If _curscrobj.oLCBalAmt = 0
							loldArea = Select()
							lcsqlstr = "Select lc_balamt from export_lc_mast where lc_no=?main_vw.lc_no"
							nretval = _curscrobj.sqlconobj.dataconn([EXE],company.dbname,lcsqlstr,"cur_lcbal","_curscrobj.nHandle",_curscrobj.DataSessionId,.T.)
							If nretval < 0
								nretval = _curscrobj.sqlconobj.sqlconnclose("_curscrobj.nHandle")
								Return .F.
							Endif
							nretval = _curscrobj.sqlconobj.sqlconnclose("_curscrobj.nHandle")
							_curscrobj.oLCBalAmt = cur_lcbal.lc_balamt
							If Used('cur_lcbal')
								Use In cur_lcbal
							Endif
							Select(loldArea)
						Endif
					Endif
				Endif
			Endif
		Endif
	Endif

* End --> CR_KOEL_0002_Export_Sales_Transaction_&_Export_LC_Master_Cross_Reference

* Changes done as per --> CR_KOEL_0005A_Form_To_Record_Pre_Shipment_Info
* Date : 08/11/2012
* Changes done by EBS Product Team

	If Inlist(main_vw.entry_ty,"SI")
		_curscrobj = _Screen.ActiveForm
		If _curscrobj.addmode = .F. And _curscrobj.editmode = .F.
			If Used("Tbl_PreShipment_Vw")
				Use In Tbl_PreShipment_Vw
			Endif

			If Used("curPreShipment")
				Use In curPreShipment
			Endif
		Endif
	Endif
Endif
* End --> CR_KOEL_0005A_Form_To_Record_Pre_Shipment_Info

* EPCG Changes done as per --> PR_EPCG_00001_Import_Purchase_Duty_Saved_EO
* Changes done by EBS Product Team on 05022013
_mepcg = oGlblPrdFeat.UdChkProd('epcg')
_meximaa = oGlblPrdFeat.UdChkProd('exim_aa')
_curscrobj = _Screen.ActiveForm
If _mepcg
* Changed by GAURAV (as guided by Shrikant) instead of _curscrobj PCVTYPE now getting from main_vw, Bug-25365 (Date : 23/03/2015) - start *
*If Inlist(_curscrobj.pcvtype,'P1')
	If Inlist(main_vw.entry_ty,'P1')
* Changed by GAURAV (as guided by Shrikant) instead of _curscrobj PCVTYPE now getting from main_vw, Bug-25365 (Date : 23/03/2015) - end *
		If _curscrobj.addmode = .T. Or _curscrobj.editmode = .T.

			If Type('_curscrobj.EPCGctrlObjName2') = 'O'
				_curscrobj.EPCGctrlObjName2.Enabled = .F.
			Endif

			If Type('_curscrobj.EPCGctrlObjName3') = 'O'
				_curscrobj.EPCGctrlObjName3.Enabled = .F.
			Endif

			If _curscrobj.addmode = .T.
				_curscrobj.oEPCGTranTotEO = 0
				_curscrobj.oEPCGTranDtySaved = 0
				_curscrobj.oEPCGTranItemDtySaved = 0
				_curscrobj.oEPCGTranItemTotEO = 0
				_curscrobj.oEPCGLicenseNo = ""
			Endif

			If !Empty(lmc_vw.licen_no) Or !Used("curepcgmast") Or _curscrobj.oEPCGLicenseNo <> lmc_vw.licen_no
				loldArea = Select()
				lcsqlstr = "Select * from epcg_mast where licen_no =?lmc_vw.licen_no"
				nretval = _curscrobj.sqlconobj.dataconn([EXE],company.dbname,lcsqlstr,"curepcgmast","_curscrobj.nHandle",_curscrobj.DataSessionId,.T.)
				If nretval < 0
					nretval = _curscrobj.sqlconobj.sqlconnclose("_curscrobj.nHandle")
					Return .F.
				Endif
				nretval = _curscrobj.sqlconobj.sqlconnclose("_curscrobj.nHandle")

				_curscrobj.oEPCGLCBalAmt = curepcgmast.duty_saved
				_curscrobj.oEPCGEOBalAmt = curepcgmast.tot_eo
				Select(loldArea)
			Endif
		Else
			If _curscrobj.addmode = .F. And _curscrobj.editmode = .F.
* Changes done by EBS Product Team on 14032013
				If !Used("curepcgmast")
					loldArea = Select()
					lcsqlstr = "Select * from epcg_mast where licen_no =?lmc_vw.licen_no"
					nretval = _curscrobj.sqlconobj.dataconn([EXE],company.dbname,lcsqlstr,"curepcgmast","_curscrobj.nHandle",_curscrobj.DataSessionId,.T.)
					If nretval < 0
						nretval = _curscrobj.sqlconobj.sqlconnclose("_curscrobj.nHandle")
						Return .F.
					Endif
					nretval = _curscrobj.sqlconobj.sqlconnclose("_curscrobj.nHandle")
					Select(loldArea)
				Endif
* Changes done by EBS Product Team on 14032013
&& Clears LC Balance Amount Form Property while in View Mode
				If Type("_curscrobj.oEPCGLCBalAmt") <> 'U'
					_curscrobj.oEPCGLCBalAmt = 0
				Endif

				If Type("_curscrobj.oEPCGEOBalAmt") <> 'U'
					_curscrobj.oEPCGEOBalAmt = 0
				Endif

				If Type("_curscrobj.cntEPCGLcBalShow") <> 'U'
					_curscrobj.cntEPCGLcBalShow.Visible = .F.
				Endif

&& Stores LC No. Form Property while in View Mode
				If Type("_curscrobj.oEPCGLicenseNo") <> 'U'
					_curscrobj.oEPCGLicenseNo = Iif(Isnull(lmc_vw.licen_no),'',lmc_vw.licen_no)
				Endif

				If Type("_curscrobj.oEPCGLicenseNoUpdtFlag") <> 'U'
					_curscrobj.oEPCGLicenseNoUpdtFlag = .F.
				Endif

&& Clears LC Balance Amount Form Property while in View Mode

				If Type("_curscrobj.oEPCGTranItemDtySaved") <> 'U'
					_curscrobj.oEPCGTranItemDtySaved= 0
				Endif

				If Type("_curscrobj.oEPCGTranItemTotEO") <> 'U'
					_curscrobj.oEPCGTranItemTotEO =0
				Endif

				If Type("_curscrobj.oEPCGTranItemDtySaved") <> 'U' And Type("_curscrobj.oEPCGTranItemTotEO") <> 'U'
					_Tally=0
					Select Sum(basic_eo) As sumbasiceo,Sum(duty_saved) As sumdutysaved From item_vw With (Buffering = .T.) ;
						INTO Cursor cursumitem_vw

					If _Tally > 0
						_curscrobj.oEPCGTranItemTotEO =  cursumitem_vw.sumbasiceo
						_curscrobj.oEPCGTranItemDtySaved =  cursumitem_vw.sumdutysaved
					Else
						_curscrobj.oEPCGTranItemTotEO = 0
						_curscrobj.oEPCGTranItemDtySaved = 0
					Endif

					If Used("cursumitem_vw")
						Use In cursumitem_vw
					Endif
				Endif

				If Type("_curscrobj.oEPCGTranDtySaved") <> 'U' And Type("_curscrobj.oEPCGTranTotEO") <> 'U'
					_curscrobj.oEPCGTranTotEO =  lmc_vw.tot_eo
					_curscrobj.oEPCGTranDtySaved =  lmc_vw.duty_saved
				Endif

				If Used('curepcgmast')
					Use In curepcgmast
				Endif
			Endif
		Endif
*!*			Messagebox(curepcgmast.licen_no,Program())
	Endif
Endif
* End ---> PR_EPCG_00001_Import_Purchase_Duty_Saved_EO

* AA Changes done as per --> PR_AA_00001_Import_Purchase_AA
* Changes done by EBS Product Team on 29122012
If _meximaa
* Changed by GAURAV (as guided by Shrikant) instead of _curscrobj PCVTYPE now getting from main_vw, Bug-25365 (Date : 23/03/2015) - start *
*If Inlist(_curscrobj.pcvtype,"P1","EI")
	If Inlist(main_vw.entry_ty,"P1","EI")
* Changed by GAURAV (as guided by Shrikant) instead of _curscrobj PCVTYPE now getting from main_vw, Bug-25365 (Date : 23/03/2015) - end *
		If _curscrobj.addmode = .T. Or _curscrobj.editmode = .T.

			If Type('_curscrobj.AActrlObjName2') = 'O'
				_curscrobj.AActrlObjName2.Enabled = .F.
			Endif
			If Type('_curscrobj.AActrlObjName3') = 'O'
				_curscrobj.AActrlObjName3.Enabled = .F.
			Endif

			If _curscrobj.addmode = .T.
				_curscrobj.oAATranDtySaved = 0
				_curscrobj.oAATranItemDtySaved = 0
				_curscrobj.oAATranTotInvAmt = 0
				_curscrobj.oAATranItemInvAmt = 0
				_curscrobj.oAAEOInvAmt = 0
				_curscrobj.oAALicenseNo = ""
			Endif

			If !Empty(lmc_vw.aalic_no) Or !Used("curaamast") Or _curscrobj.oAALicenseNo <> lmc_vw.aalic_no
				loldArea = Select()
				lcsqlstr = "Select * from aa_mast where licen_no =?lmc_vw.aalic_no"
				nretval = _curscrobj.sqlconobj.dataconn([EXE],company.dbname,lcsqlstr,"curaamast","_curscrobj.nHandle",_curscrobj.DataSessionId,.T.)
				If nretval < 0
					nretval = _curscrobj.sqlconobj.sqlconnclose("_curscrobj.nHandle")
					Return .F.
				Endif
				nretval = _curscrobj.sqlconobj.sqlconnclose("_curscrobj.nHandle")

				If main_vw.entry_ty = "P1"
					_curscrobj.oAALCBalAmt = curaamast.act_duty
* Changes done by EBS Product Team on 27022013
					_curscrobj.oAAActInvAmt = curaamast.act_invamt
					_curscrobj.oAAEOBalAmt = curaamast.eo_invamt
* Changes done by EBS Product Team on 27022013
				Else
					If main_vw.entry_ty = "EI"
						_curscrobj.oAATotInvAmt = curaamast.exp_invamt
					Endif
				Endif
				Select(loldArea)

			Endif
		Else
			If _curscrobj.addmode = .F. And _curscrobj.editmode = .F.
* Changes done by EBS Product Team on 27022013
				If !Used("curaamast")
					loldArea = Select()
					lcsqlstr = "Select * from aa_mast where licen_no =?lmc_vw.aalic_no"
					nretval = _curscrobj.sqlconobj.dataconn([EXE],company.dbname,lcsqlstr,"curaamast","_curscrobj.nHandle",_curscrobj.DataSessionId,.T.)
					If nretval < 0
						nretval = _curscrobj.sqlconobj.sqlconnclose("_curscrobj.nHandle")
						Return .F.
					Endif
					nretval = _curscrobj.sqlconobj.sqlconnclose("_curscrobj.nHandle")
					Select(loldArea)
				Endif
* Changes done by EBS Product Team on 27022013

&& Clears LC Balance Amount Form Property while in View Mode
				If Type("_curscrobj.oAALCBalAmt") <> 'U'
					_curscrobj.oAALCBalAmt = 0
				Endif

				If Type("_curscrobj.oAAEOBalAmt") <> 'U'
					_curscrobj.oAAEOBalAmt = 0
				Endif

				If Type("_curscrobj.cntAALcBalShow") <> 'U'
					_curscrobj.cntAALcBalShow.Visible = .F.
				Endif

&& Stores LC No. Form Property while in View Mode
				If Type("_curscrobj.oAALicenseNo") <> 'U'
					_curscrobj.oAALicenseNo = Iif(Isnull(lmc_vw.aalic_no),'',lmc_vw.aalic_no)
				Endif

				If Type("_curscrobj.oAALCUpdtFlag") <> 'U'
					_curscrobj.oAALCUpdtFlag = .F.
				Endif

* AA Changes done as per --> PR_AA_00001_Import_Purchase_AA
* Changes done by EBS Product Team on 29122012

				If Inlist(_curscrobj.pcvtype,"P1")

					If Type("_curscrobj.oAATranItemDtySaved") <> "U"
						_curscrobj.oAATranItemDtySaved = main_vw.tot_examt
					Else
						_curscrobj.oAATranItemDtySaved = 0
					Endif

					If Type("_curscrobj.oAATranDtySaved") <> "U"
						If Type("main_vw.aa_duty") <> "U"
							_curscrobj.oAATranDtySaved = Iif(Isnull(main_vw.aa_duty),0.00,main_vw.aa_duty)
						Else
							If Type("lmc_vw.aa_duty") <> "U"
								_curscrobj.oAATranDtySaved = Iif(Isnull(lmc_vw.aa_duty),0.00,lmc_vw.aa_duty)
							Endif
						Endif
					Endif

* Changes done by EBS Product Team on 27022013
					If Type("_curscrobj.oAATranItemInvAmt") <> "U"
						_curscrobj.oAATranItemInvAmt = main_vw.net_amt
					Else
						_curscrobj.oAATranItemInvAmt = 0
					Endif
* Changes done by EBS Product Team on 27022013

					If Type("_curscrobj.oAATranTotInvAmt") <> "U"
						If Type("main_vw.aa_invamt") <> "U"
							_curscrobj.oAATranTotInvAmt = Iif(Isnull(main_vw.aa_invamt),0.00,main_vw.aa_invamt)
						Else
							If Type("lmc_vw.aa_invamt") <> "U"
								_curscrobj.oAATranTotInvAmt = Iif(Isnull(lmc_vw.aa_invamt),0.00,lmc_vw.aa_invamt)
							Endif
						Endif
					Endif

					If Type("_curscrobj.oAAEOInvAmt") <> "U"
						_curscrobj.oAAEOInvAmt = main_vw.aa_eoinvamt
					Endif
				Endif
* End ---> PR_AA_00001_Import_Purchase_AA

* AA Changes done as per --> PR_AA_00002_Export_Sales_EO_AA
* Changes done by EBS Product Team on 04022013

				If Inlist(_curscrobj.pcvtype,"EI")
					If Type("_curscrobj.oAATranItemInvAmt") <> "U"
						_curscrobj.oAATranItemInvAmt = main_vw.net_amt
					Endif

					If Type("_curscrobj.oAATranTotInvAmt") <> "U"
						If Type("main_vw.aa_invamt") <> "U"
							_curscrobj.oAATranTotInvAmt = Iif(Isnull(main_vw.aa_invamt),0.00,main_vw.aa_invamt)
						Else
							If Type("lmc_vw.aa_invamt") <> "U"
								_curscrobj.oAATranTotInvAmt = Iif(Isnull(lmc_vw.aa_invamt),0.00,lmc_vw.aa_invamt)
							Endif
						Endif
					Endif
				Endif
* End ---> PR_AA_00002_Export_Sales_EO_AA

			Endif
		Endif
	Endif
Endif
* End ---> PR_AA_00001_Import_Purchase_AA
*changes by EBS team on 07/03/14 for Bug-21466,21467,21468 end

**** Added by Sachin N. S. on 01/04/2016 for Bug-27503 -- Start
If Type('_curvouobj._udClsPointOfSale')='O'
	_curvouobj._udClsPointOfSale._uetrigvourefresh()
Endif
**** Added by Sachin N. S. on 01/04/2016 for Bug-27503 -- End



***** Added by Sachin N. S. on 14/10/2016 for GST -- Start
If Inlist(main_vw.entry_ty,'E1','S1','IB','J6','BP','BR')
	If Type('_curvouobj.voupage.page1.grditem')<>'U'
		tot_grd_col=_curvouobj.voupage.page1.grditem.ColumnCount
		For i = 1 To tot_grd_col

			Do Case
			Case _curvouobj.voupage.page1.grditem.Columns(i).header1.Caption='Quantity'
				_curvouobj.voupage.page1.grditem.Columns(i).Visible=.F.
			Case _curvouobj.voupage.page1.grditem.Columns(i).header1.Caption='Rate'
				_curvouobj.voupage.page1.grditem.Columns(i).Visible=.F.
			Case _curvouobj.voupage.page1.grditem.Columns(i).header1.Caption='Taxable Value'
*!*					If Inlist(_curvouobj.pcvtype,"IB","J6","E1","S1")			&& Commented by Shrikant S. on 09/02/2017 for GST
				If Inlist(_curvouobj.pcvtype,"IB","J6")				&& Added by Shrikant S. on 09/02/2017 for GST
					_curvouobj.voupage.page1.grditem.Columns(i).Visible=.F.
				Endif
			Case Upper(_curvouobj.voupage.page1.grditem.Columns(i).ControlSource)="ITEM_VW.STAXAMT"
				_curvouobj.voupage.page1.grditem.Columns(i).ReadOnly=.T.
			Endcase
		Endfor
	Endif
Endif

If Inlist(main_vw.entry_ty,'BP','BR','CP','CR') And Type('_curvouobj.pcvtype')<>'U'
	_curvouobj.cmdservicetax.Enabled=.F.
Endif
***** Added by Sachin N. S. on 14/10/2016 for GST -- End


Procedure calldata()
Parameters mobj,mmode
If mmode = "E"
	lcstr = "select a.*,b.it_name as item from Op_PtTaxDet a inner join it_mast b on a.it_code = b.it_code ;
				 where a.entry_ty= ?main_vw.entry_ty and a.tran_cd = ?main_vw.tran_cd"

	sql_con = mobj.sqlconobj.dataconn([EXE],company.dbname,lcstr,[curOpPtTaxvw],;
		"Thisform.nHandle",mobj.DataSessionId,.F.)

	If sql_con < 1
		=mobj.sqlconobj.sqlconnclose("Thisform.nHandle")
		mobj.showmessagebox("Error while open Op_PtTaxDet table",32,vumess)	&&test_z 32
		Return .F.
	Endif

	Delete From op_pttax_vw
	Insert Into op_pttax_vw Select * From curoppttaxvw
	Go Top In op_pttax_vw
Else
	If mmode = "A"
		malias = Alias()
		Select op_pttax_vw
		Delete All
		Select (malias)
		With mobj.voupage.pgappduties.grdappduties
			.Refresh()
		Endwith
	Endif
Endif
Endproc
*!*	** End : Added by Uday on 26/12/2011 for Exim
