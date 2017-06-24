*!*	IF INLIST(_screen.ActiveForm.pcvtype,'AR','DC','PT')
*!*		WITH _screen.ActiveForm
*!*		tot_grd_col=_screen.activeform.voupage.page1.grditem.columncount
*!*		FOR i = 1 TO tot_grd_col
*!*
*!*		DO CASE
*!*		CASE .voupage.page1.grditem.columns(i).header1.caption='Ass. Value'
*!*			 .voupage.page1.grditem.columns(i).visible=.F.

*!*			ENDCASE
*!*
*!*		ENDFOR
*!*		ENDWITH
*!*
*!*
*!*	ENDIF

*!*	IF INLIST(_SCREEN.ACTIVEFORM.pcvtype,'D1','D2','D3','D4','D5','C3','C2','C4','C5')		&& Commented by Shrikant S. on 17/03/2012 for Bug-2276
If Inlist(_Screen.ActiveForm.pcvtype,'D2','D3','D4','D5','C3','C2','C4','C5')			&& Added by Shrikant S. on 17/03/2012 for Bug-2276
	With _Screen.ActiveForm
		If Type('_screen.ActiveForm.intbaseday')='U'
			AddProperty(_Screen.ActiveForm,"intbaseday",0)
		Endif
*	.lbldepartment.caption = 'Debit Type'
*	.lbldepartment.left  =  .txtdate.left+.txtdate.width+30
*	.lbldepartment.top =  .txtpartyname.top
*	.txtdepartment.left = .lbldepartment.left+.lbldepartment.width
*	.txtdepartment.top =  .txtpartyname.top
*	.txtdepartment.width = 150
*	.txtdepartment.tabindex = .txtdate.tabindex+1
		tot_grd_col=0
		tot_grd_col=_Screen.ActiveForm.voupage.page1.grditem.ColumnCount
		For i = 1 To tot_grd_col

			Do Case
			Case .voupage.page1.grditem.Columns(i).header1.Caption='Rate'
*		 .voupage.page1.grditem.columns(i).enabled=.F.
				.voupage.page1.grditem.Columns(i).Visible=.F.

			Case .voupage.page1.grditem.Columns(i).header1.Caption='Quantity'
				.voupage.page1.grditem.Columns(i).Visible=Iif(Inlist(_Screen.ActiveForm.pcvtype,'D5','D4','C4','C5'),.T.,.F.)			&& Added by Shrikant S. on 17/03/2012 for Bug-2276
*!*						.voupage.page1.grditem.COLUMNS(i).VISIBLE=IIF(INLIST(_SCREEN.ACTIVEFORM.pcvtype,'D1','D5','D4','C4','C5'),.T.,.F.)		&& Commented by Shrikant S. on 17/03/2012 for Bug-2276
*		 .voupage.page1.grditem.columns(i).enabled=.F.
			Case .voupage.page1.grditem.Columns(i).header1.Caption='Sale Bill No.'
				.voupage.page1.grditem.Columns(i).Width=70

			Case .voupage.page1.grditem.Columns(i).header1.Caption='Item Name'
				.voupage.page1.grditem.Columns(i).Visible=.F.				&& Added by Shrikant S. on 04/01/2013 for Bug-7269
*!*							.voupage.page1.grditem.Columns(i).Visible=Iif(Inlist(_Screen.ActiveForm.pcvtype,'D3','C3'),.T.,.F.)				&& Added by Shrikant S. on 17/03/2012 for Bug-2276	&& Commented by Shrikant S. on 04/01/2013 for Bug-7269
*!*						.voupage.page1.grditem.COLUMNS(i).VISIBLE=IIF(INLIST(_SCREEN.ACTIVEFORM.pcvtype,'D3','D1','C3'),.T.,.F.)	&& Commented by Shrikant S. on 17/03/2012 for Bug-2276
				.voupage.page1.grditem.Columns(i).Width=120

			Endcase

		Endfor
	Endwith
Endif
&& Added by Shrikant S. on 05/11/2012 for Bug-4744		&& Start
If Vartype(oGlblPrdFeat)='O'
	If oGlblPrdFeat.UdChkProd('openord')
		If Inlist(_Screen.ActiveForm.pcvtype,'OO','TR')
			tot_grd_col=_Screen.ActiveForm.voupage.page1.grditem.ColumnCount
			Do Case
			Case _Screen.ActiveForm.pcvtype='OO' And lcode_vw.allowzeroqty=.T.

				With _Screen.ActiveForm
					For i = 1 To tot_grd_col
						Do Case
						Case .voupage.page1.grditem.Columns(i).header1.Caption='Quantity'
							.voupage.page1.grditem.Columns(i).Visible=.F.
							.voupage.page1.grditem.Columns(i).ReadOnly=.T.
						Endcase
					Endfor
				Endwith

			Case _Screen.ActiveForm.pcvtype='TR'
				If Used('Itref_vw')
					Select entry_ty,allowzeroqty From _lcodepickup Where entry_ty=Itref_vw.Rentry_ty Into Cursor _lcodezeroqty
					Select _lcodezeroqty
					If _lcodezeroqty.allowzeroqty==.T.
						With _Screen.ActiveForm
							For i = 1 To tot_grd_col
								Do Case
								Case .voupage.page1.grditem.Columns(i).header1.Caption='Quantity'
									.voupage.page1.grditem.Columns(i).ReadOnly=.T.
								Endcase
							Endfor
						Endwith
					Endif
				Endif
			Endcase
		Endif
	Endif
Endif
&& Added by Shrikant S. on 05/11/2012 for Bug-4744		&& End

&& Added by Shrikant S. on 06/10/2016 for GST		&& Start
If Inlist(_Screen.ActiveForm.pcvtype,'E1','S1','IB','J6','BP','BR','CP','CR','C6','D6','RV')		&& Commented by Shrikant S. on 06/02/2017 for GST
*!*	If Inlist(_Screen.ActiveForm.pcvtype,'E1','S1','IB','J6','BP','BR','CP','CR','GC','GD','C6','D6')			&& Added by Shrikant S. on 06/02/2017 for GST
	If Type('_Screen.ActiveForm.voupage.page1.grditem')<>'U'
		With _Screen.ActiveForm
			tot_grd_col=0
			tot_grd_col=_Screen.ActiveForm.voupage.page1.grditem.ColumnCount
			For i = 1 To tot_grd_col

				Do Case
				Case .voupage.page1.grditem.Columns(i).header1.Caption='Quantity'
					.voupage.page1.grditem.Columns(i).Visible=.F.
*!*						IF INLIST(_Screen.ActiveForm.pcvtype,'GC','GD','C6','D6')
*!*							.voupage.page1.grditem.Columns(i).Visible=.t.
*!*							.voupage.page1.grditem.Columns(i).eNABLED=.F.
*!*						ENDIF
				Case ALLTRIM(UPPER(.voupage.page1.grditem.Columns(i).controlsource))="ITEM_VW.U_ASSEAMT"
*!*						If Inlist(_Screen.ActiveForm.pcvtype,"IB","J6","E1","S1","C6","D6")			&& Commented by Shrikant S. on 06/02/2017 for GST
					If Inlist(_Screen.ActiveForm.pcvtype,"IB","J6")			&& aDDED by Shrikant S. on 09/02/2017 for GST
*!*						If Inlist(_Screen.ActiveForm.pcvtype,"IB","J6","E1","S1")				&& Added by Shrikant S. on 06/02/2017 for GST
						.voupage.page1.grditem.Columns(i).Visible=.F.
					ENDIF
					If Inlist(_Screen.ActiveForm.pcvtype,"C6","D6")	
						.voupage.page1.grditem.Columns(i).header1.Caption="Diff. Taxable Value"
						.voupage.page1.grditem.Columns(i).readonly=.t.
						.voupage.page1.grditem.Columns(i).width=110
					ENDIF
					If Inlist(_Screen.ActiveForm.pcvtype,"S1","E1")	
						.voupage.page1.grditem.Columns(i).readonly=.t.
					ENDIF
				Case .voupage.page1.grditem.Columns(i).header1.Caption='Rate'
					.voupage.page1.grditem.Columns(i).Visible=.F.
*!*						IF INLIST(_Screen.ActiveForm.pcvtype,'GC','GD','C6','D6')
*!*							.voupage.page1.grditem.Columns(i).Visible=.t.
*!*						ENDIF
				Case ALLTRIM(UPPER(.voupage.page1.grditem.Columns(i).controlsource))="ITEM_VW.STAXAMT"
					.voupage.page1.grditem.Columns(i).Readonly=.T.
				Endcase

			Endfor
		Endwith
	Endif
Endif
&& Added by Shrikant S. on 06/10/2016 for GST		&& End


&& Added by Shrikant S. on 23/02/2017 for GST		&& Start
If Inlist(_Screen.ActiveForm.pcvtype,'GD','GC')
	With _Screen.ActiveForm
		tot_grd_col=.voupage.page1.grditem.ColumnCount
		For i = 1 To tot_grd_col
			Do Case
			Case UPPER(.voupage.page1.grditem.Columns(i).controlsource)='ITEM_VW.RATE'
				.voupage.page1.grditem.Columns(i).header1.Caption='Diff. Rate'
			Endcase
		Endfor
	Endwith
Endif
&& Added by Shrikant S. on 23/02/2017 for GST		&& End

*--------------------------------------------------------------------------------------------------------------

*!*	IF INLIST(_screen.ActiveForm.pcvtype,'CI')
*!*		WITH _screen.ActiveForm
*!*
*!*		.lbldepartment.caption = 'Credit Type'
*!*		.lbldepartment.left  =  .txtdate.left+.txtdate.width+30
*!*		.lbldepartment.top =  .txtpartyname.top
*!*		.txtdepartment.left = .lbldepartment.left+.lbldepartment.width
*!*		.txtdepartment.top =  .txtpartyname.top
*!*		.txtdepartment.width = 150
*!*		.txtdepartment.tabindex = .txtdate.tabindex+1
*!*		tot_grd_col=0
*!*		tot_grd_col=_screen.activeform.voupage.page1.grditem.columncount
*!*		FOR i = 1 TO tot_grd_col
*!*
*!*		DO CASE
*!*		CASE .voupage.page1.grditem.columns(i).header1.caption='Rate'
*!*			 .voupage.page1.grditem.columns(i).visible=.F.

*!*		CASE .voupage.page1.grditem.columns(i).header1.caption='Quantity'
*!*			 .voupage.page1.grditem.columns(i).visible=.F.

*!*		CASE .voupage.page1.grditem.columns(i).header1.caption='Purchase Bill No.'
*!*			 .voupage.page1.grditem.columns(i).width=80

*!*		CASE .voupage.page1.grditem.columns(i).header1.caption='Item Name'
*!*			 .voupage.page1.grditem.columns(i).visible=.f.

*!*		ENDCASE
*!*
*!*		ENDFOR
*!*		ENDWITH
*!*	ENDIF


*!*	IF INLIST(_screen.ActiveForm.pcvtype,'DI','CI')
*!*		WITH _screen.ActiveForm
*!*
*!*		.lbldepartment.caption = IIF(.PCVTYPE='DI','Debit Type','Credit Type')
*!*		.lbldepartment.left  =  .txtdate.left+.txtdate.width+30
*!*		.lbldepartment.top =  .txtpartyname.top
*!*		.txtdepartment.left = .lbldepartment.left+.lbldepartment.width
*!*		.txtdepartment.top =  .txtpartyname.top
*!*		.txtdepartment.width = 150
*!*		.txtdepartment.tabindex = .txtdate.tabindex+1
*!*		tot_grd_col=0
*!*		tot_grd_col=_screen.activeform.voupage.page1.grditem.columncount
*!*		FOR i = 1 TO tot_grd_col
*!*
*!*		DO CASE
*!*		CASE .voupage.page1.grditem.columns(i).header1.caption='Rate'
*!*			 .voupage.page1.grditem.columns(i).visible=.F.

*!*		CASE .voupage.page1.grditem.columns(i).header1.caption='Quantity'
*!*			 .voupage.page1.grditem.columns(i).visible=.F.

*!*		CASE INLIST(.voupage.page1.grditem.columns(i).header1.caption,'Sale Bill No.','Purchase Bill No.')
*!*			 .voupage.page1.grditem.columns(i).width=70

*!*		CASE .voupage.page1.grditem.columns(i).header1.caption='Item Name'
*!*			 .voupage.page1.grditem.columns(i).visible=.f.

*!*		ENDCASE
*!*
*!*		ENDFOR
*!*		ENDWITH
*!*	ENDIF

*-------------------------------------------------------------------------------------------------------

*!*	IF INLIST(_screen.ActiveForm.pcvtype,'DB')
*!*		WITH _screen.ActiveForm
*!*		tot_grd_col=_screen.activeform.voupage.page1.grditem.columncount
*!*		FOR i = 1 TO tot_grd_col
*!*
*!*		DO CASE
*!*		CASE .voupage.page1.grditem.columns(i).header1.caption='Rate'
*!*			 .voupage.page1.grditem.columns(i).visible=.F.

*!*		CASE .voupage.page1.grditem.columns(i).header1.caption='Quantity'
*!*			 .voupage.page1.grditem.columns(i).visible=.F.

*!*		CASE .voupage.page1.grditem.columns(i).header1.caption='Sale Bill No.'
*!*			 .voupage.page1.grditem.columns(i).width=70

*!*		CASE .voupage.page1.grditem.columns(i).header1.caption='Item Name'
*!*			 .voupage.page1.grditem.columns(i).visible=.f.
*!*		ENDCASE
*!*
*!*		ENDFOR
*!*		ENDWITH
*!*	ENDIF

*!*	** Start : Added by Uday on dated 26/12/2011 for Exim
_mexim = .F.                              && Added by Ajay Jaiswal on 22/02/2012 for EXIM
_mdbk = .F.                               && Added by Ajay Jaiswal on 03/04/2012 for DBK
_mexim = oGlblPrdFeat.UdChkProd('exim')   && Added by Ajay Jaiswal on 22/02/2012 for EXIM
_mdbk = oGlblPrdFeat.UdChkProd('dbk')     && Added by Ajay Jaiswal on 03/04/2012 for DBK
If _mexim  Or _mdbk                                 && Added by Ajay Jaiswal on 21/02/2012 for EXIM & DBK
	If _Screen.ActiveForm.pcvtype = "OP"
		Local mobj
		mobj = _Screen.ActiveForm
		If !Used([Op_PtTax_Vw])

			lcstr = "select a.*,b.it_name as item from Op_PtTaxDet a inner join it_mast b on a.it_code = b.it_code where 1=2"

			sql_con = mobj.sqlconobj.dataconn([EXE],company.dbname,lcstr,[Op_PtTax_vw],;
				"Thisform.nHandle",mobj.DataSessionId,.F.)

			If sql_con < 1
				=mobj.sqlconobj.sqlconnclose("Thisform.nHandle")
				mobj.showmessagebox("Error while open Op_PtTaxDet table",32,vumess)	&&test_z 32
				Return .F.
			Endif

		Endif

		If !Used("dcmastcursor")
			lcstr = [Select Fld_Nm,Pert_Name,Head_Nm,cOrder From Dcmast Where Code = 'E' And Entry_Ty IN('P1','PT') AND Att_File = 0 order by corder]

			sql_con = mobj.sqlconobj.dataconn([EXE],company.dbname,lcstr,[cur_dcmast],;
				"Thisform.nHandle",mobj.DataSessionId,.F.)
			If sql_con < 1
				=mobj.sqlconobj.sqlconnclose("Thisform.nHandle")
				mobj.showmessagebox("Error while open Discount charges Master table",32,vumess)	&&test_z 32
			Endif

			=mobj.sqlconobj.sqlconnclose("Thisform.nHandle")

			Set enginebehavior 70			&& Old version
			Select fld_nm,pert_name,head_nm,'' As rorder From cur_dcmast a;
				GROUP By a.fld_nm,a.pert_name,a.head_nm;
				INTO Cursor _tmpcurs Readwrite
			Set enginebehavior 90			&& Old version

			Update a Set a.rorder = b.corder From _tmpcurs a;
				inner Join cur_dcmast b ;
				ON a.fld_nm = b.fld_nm And a.pert_name = b.pert_name And a.head_nm = b.head_nm

			Set enginebehavior 70			&& Old version
			Select * Into Cursor dcmastcursor From _tmpcurs Order By rorder
			Set enginebehavior 90			&& Old version

			Use In cur_dcmast
			Use In _tmpcurs
		Endif

		With mobj
			With .voupage
				.AddObject("pgAppDuties","PageAppDuty")
				With .pgappduties
					.Caption = "Apportion Tax Details "
					.AddObject("grdAppDuties","Grid")

					With .grdappduties
						.Top = mobj.voupage.page1.grditem.Top
						.Height = mobj.voupage.page1.grditem.Height
						.Width = mobj.voupage.page1.grditem.Width
						.HeaderHeight = mobj.voupage.page1.grditem.HeaderHeight
						.GridLineColor = mobj.voupage.page1.grditem.GridLineColor
						.HighlightBackColor = mobj.voupage.page1.grditem.HighlightBackColor
						.HighlightForeColor = mobj.voupage.page1.grditem.HighlightForeColor
						.AllowHeaderSizing = .F.
						.AllowRowSizing = .F.
						.DeleteMark = .F.
						.RecordMark = .F.
						.ReadOnly = .T.
						.RecordSourceType = 1
						.ColumnCount = 3
						.Visible = .T.
						.RecordSource = "Op_Pttax_vw"

						.column1.header1.Caption = "Item code"
						.column1.header1.FontSize = 8
						.column1.header1.Alignment = 1
						.column1.ControlSource = "op_pttax_vw.It_code"
						.column1.ColumnOrder = 1
						.column1.BackColor = Rgb(255,255,255)
						.column1.FontSize = 8
						.column1.ReadOnly = .T.
						.column1.Sparse = .F.
						.column1.Alignment = 0
						.column1.text1.Alignment = 0


						.column2.header1.Caption = "Item"
						.column2.header1.FontSize = 8
						.column2.header1.Alignment = 1
						.column2.ControlSource = "op_pttax_vw.item"
						.column2.ColumnOrder = 2
						.column2.Width = 210
						.column2.ReadOnly = .T.
						.column2.FontSize = 8
						.column2.Alignment = 0
						.column2.text1.Alignment = 0

						.column3.header1.Caption = "Qty"
						.column3.header1.FontSize = 8
						.column3.header1.Alignment = 1
						.column3.ControlSource = "op_pttax_vw.Qty"
						.column3.ColumnOrder = 3
						.column3.Width = 90
						.column3.ReadOnly = .T.
						.column3.FontSize = 8


						Select dcmastcursor
						Go Top
*!*					stcol = IIF(!coadditional.eou AND thisform.stmast > 0,15,13)
						stcol = 4
						Store "" To mfldname
						Do While !Eof()
							For i = 1 To 2
								If i = 1
									If !Empty(dcmastcursor.pert_name)
										mfldname = "op_pttax_vw."+Substr(Alltrim(dcmastcursor.pert_name),Iif(At("U_",Upper(Alltrim(dcmastcursor.pert_name)))>0,At("U_",Upper(Alltrim(dcmastcursor.pert_name)))+2,1),Len(dcmastcursor.pert_name))
										If Type(mfldname) !='U'
											.ColumnCount = .ColumnCount + 1
											headcapt    = ".column"+Alltrim(Str(stcol))+".header1.caption ="+"'%'"
											headalign   = ".column"+Alltrim(Str(stcol))+".header1.alignment = 2"
											headfontsize = ".column"+Alltrim(Str(stcol))+".header1.fontsize = 8"
											colsource   = ".column"+Alltrim(Str(stcol))+".controlsource = '"+mfldname+"'"
											colorder    = ".column"+Alltrim(Str(stcol))+".columnorder ="+Alltrim(Str(stcol))
											colwidth    = ".column"+Alltrim(Str(stcol))+".width = 100"
											colreadonly = ".column"+Alltrim(Str(stcol))+".readonly = .t."
											colfontsize = ".column"+Alltrim(Str(stcol))+".fontsize = 8"

											&headcapt
											&headalign
											&headfontsize
											&colsource
											&colorder
											&colreadonly
											&colwidth
											&colfontsize
										Endif
									Else
										stcol = stcol -1
									Endif
								Else

									If !Empty(dcmastcursor.fld_nm)
										mfldname = "op_pttax_vw."+Substr(Alltrim(dcmastcursor.fld_nm),Iif(At("U_",Upper(Alltrim(dcmastcursor.fld_nm)))>0,At("U_",Upper(Alltrim(dcmastcursor.fld_nm)))+2,1),Len(dcmastcursor.fld_nm))
										If Type(mfldname) !="U"
											.ColumnCount = .ColumnCount + 1
*stcol = stcol + 1
											headcapt    = ".column"+Alltrim(Str(stcol))+".header1.caption ="+"'"+Proper(Alltrim(dcmastcursor.head_nm))+"'"
											headalign   = ".column"+Alltrim(Str(stcol))+".header1.alignment = 2"
											headfontsize = ".column"+Alltrim(Str(stcol))+".header1.fontsize = 8"
											colsource   = ".column"+Alltrim(Str(stcol))+".controlsource = '" + mfldname +"'"
											colorder    = ".column"+Alltrim(Str(stcol))+".columnorder ="+Alltrim(Str(stcol))
											colwidth    = ".column"+Alltrim(Str(stcol))+".width = 100"
											colreadonly = ".column"+Alltrim(Str(stcol))+".readonly = .t."
											colfontsize = ".column"+Alltrim(Str(stcol))+".fontsize = 8"


											&headcapt
											&headalign
											&headfontsize
											&colsource
											&colorder
											&colreadonly
											&colwidth
											&colfontsize
										Else
											stcol = stcol -1
										Endif
									Else
										stcol = stcol -1
									Endif
								Endif
							Endfor

							stcol = stcol + 1
							Select dcmastcursor
							Skip
						Enddo
						.ScrollBars = 3
						.SetAll("Alignment",2, "header")
						.Refresh()
						If Used("dcmastcursor")
							Use In dcmastcursor
						Endif
					Endwith
				Endwith
			Endwith
		Endwith
	Endif
Endif        && Added by Ajay Jaiswal on 21/02/2012 for EXIM
&& changes by EBS team on 07/03/14 for Bug-21466,21467,21468 start
* Below Changes done as per --> CR_KOEL_0002_Export_Sales_Transaction_&_Export_LC_Master_Cross_Reference
* Changes done by EBS Product Team
_mexim = oGlblPrdFeat.UdChkProd('exim')
_curscrobj = _Screen.ActiveForm
If _mexim
	If Inlist(_curscrobj.pcvtype,"SI")
		If Type("_curscrobj.oLcNoHandler") = 'U'
			_curscrobj.AddProperty("oLcNoHandler")
		Endif

		If Type("_curscrobj.oLcNoMouseEnterHandler") = 'U'
			_curscrobj.AddProperty("oLcNoMouseEnterHandler")
		Endif

		If Type("_curscrobj.oLcNoBtnHandler") = 'U'
			_curscrobj.AddProperty("oLcNoBtnHandler")
		Endif

		If Type("_curscrobj.oLCBalAmt") = 'U'
			_curscrobj.AddProperty("oLCBalAmt")
			_curscrobj.olcbalamt = 0
		Endif

		If Type("_curscrobj.oLCNo") = 'U'
			_curscrobj.AddProperty("oLCNo")
			_curscrobj.olcno = ""
		Endif

		If Type("_curscrobj.oLCUpdtFlag") = 'U'
			_curscrobj.AddProperty("oLCUpdtFlag")
			_curscrobj.oLCUpdtFlag = .F.
		Endif

&&added by priyanka_28052013
		If Type("_curscrobj.ctrlLCObjName") = 'U'
			_curscrobj.AddProperty("ctrlLCObjName")
		Endif

		For i = 1 To _curscrobj.ControlCount Step 1
			ctrlObj = "_curscrobj.txtinfo"+Alltrim(Str(i))
			ctrlSrc = ctrlObj+".controlsource"
			If Type(ctrlObj) <> "U"
				If Alltrim(Upper(&ctrlSrc)) = "MAIN_VW.LC_NO"
					ctrlObjName = &ctrlObj
					_curscrobj.ctrlLCObjName = ctrlObjName
					Exit
				Endif
			Endif
		Endfor
&&added by priyanka_28052013

		If Type("_curscrobj.cntLcBalShow") <> "O"
			_curscrobj.AddObject('cntLcBalShow','container')
			_curscrobj.cntlcbalshow.Visible = .F.
			_curscrobj.cntlcbalshow.Top = ctrlObjName.Top + 30 &&_curscrobj.txtinfo1.TOP + 30           &&changed by priyanka_28052013
			_curscrobj.cntlcbalshow.Left = ctrlObjName.Left &&_curscrobj.txtinfo1.LEFT                  &&changed by priyanka_28052013
			_curscrobj.cntlcbalshow.Width = ctrlObjName.Width - 120 &&_curscrobj.txtinfo1.WIDTH - 120   &&changed by priyanka_28052013
			_curscrobj.cntlcbalshow.Height = ctrlObjName.Height &&_curscrobj.txtinfo1.HEIGHT            &&changed by priyanka_28052013
			_curscrobj.cntlcbalshow.BackColor = Rgb(255,255,230)
			_curscrobj.cntlcbalshow.BorderColor = Rgb(255,202,149)

			_curscrobj.cntlcbalshow.AddObject('lblText','Label')
			_curscrobj.cntlcbalshow.lbltext.Visible = .T.
			_curscrobj.cntlcbalshow.lbltext.FontSize = 7
			_curscrobj.cntlcbalshow.lbltext.BackStyle = 0
			_curscrobj.cntlcbalshow.lbltext.AutoSize = .T.
			_curscrobj.cntlcbalshow.lbltext.FontBold = .T.
			_curscrobj.cntlcbalshow.lbltext.Top = _curscrobj.cntlcbalshow.lbltext.Top + 5
			_curscrobj.cntlcbalshow.lbltext.Left = _curscrobj.cntlcbalshow.lbltext.Left + 5
			_curscrobj.cntlcbalshow.lbltext.Caption = ""
		Endif

		If Type("_curscrobj.cntGrdShowMsg") <> "O"
			_curscrobj.AddObject('cntGrdShowMsg','container')
			_curscrobj.cntGrdShowMsg.Visible = .F.
			_curscrobj.cntGrdShowMsg.Top = _curscrobj.voupage.Top - 2
			_curscrobj.cntGrdShowMsg.Left = _curscrobj.voupage.Width - 580
			_curscrobj.cntGrdShowMsg.Width = 580
			_curscrobj.cntGrdShowMsg.Height = 26
			_curscrobj.cntGrdShowMsg.Anchor = 9
			_curscrobj.cntGrdShowMsg.BackColor = Rgb(255,255,230)
			_curscrobj.cntGrdShowMsg.BorderColor = Rgb(255,202,149)

			_curscrobj.cntGrdShowMsg.AddObject('lblmsg','Label')
			_curscrobj.cntGrdShowMsg.lblmsg.Visible = .T.
			_curscrobj.cntGrdShowMsg.lblmsg.FontSize = 7
			_curscrobj.cntGrdShowMsg.lblmsg.BackStyle = 0
			_curscrobj.cntGrdShowMsg.lblmsg.AutoSize = .T.
			_curscrobj.cntGrdShowMsg.lblmsg.FontBold = .T.
			_curscrobj.cntGrdShowMsg.lblmsg.FontItalic = .T.
			_curscrobj.cntGrdShowMsg.lblmsg.Top = _curscrobj.cntGrdShowMsg.lblmsg.Top + 5
			_curscrobj.cntGrdShowMsg.lblmsg.Left = _curscrobj.cntGrdShowMsg.lblmsg.Left + 5
			_curscrobj.cntGrdShowMsg.lblmsg.Caption = ""
		Endif

		_curscrobj.olcnohandler=Newobject("LcNoValidHandler")  && Item Grid Qty Column
		_curscrobj.olcnomouseenterhandler=Newobject("LcNoMouseEnterHandler")

		_curscrobj.olcnobtnhandler=Newobject("LcNoBtnHandler")

		collcnoobj = "ctrlObjName" &&"_curscrobj.txtinfo1" &&changed by priyanka_28052013
		collcmdobj = "_curscrobj.cmdsdc" && cmdpnsearch

		Bindevent(&collcnoobj,"Valid",_curscrobj.olcnohandler,"LcNovalid",1)
		Bindevent(&collcnoobj,"MouseEnter",_curscrobj.olcnomouseenterhandler,"LcNoMouseEnter",1)
		Bindevent(&collcnoobj,"MouseLeave",_curscrobj.olcnomouseenterhandler,"LcNoMouseLeave",1)
		Bindevent(&collcnoobj,"GotFocus",_curscrobj.olcnomouseenterhandler,"LcNoGotFocus",1)
		Bindevent(&collcnoobj,"LostFocus",_curscrobj.olcnomouseenterhandler,"LcNoLostFocus",1)
		Bindevent(&collcmdobj,"Click",_curscrobj.olcnobtnhandler,"LcNoBtnClick",1)

&&changed by priyanka_28052013
*!*		IF TYPE('_curscrobj.txtinfo1') <> 'U'
		If Type("ctrlObjName") <> "U"
			ctrlObjName.ReadOnly = .F.
*!*			_curscrobj.txtinfo1.READONLY = .F.
		Endif
&&changed by priyanka_28052013
	Endif
Endif
* End --> CR_KOEL_0002_Export_Sales_Transaction_&_Export_LC_Master_Cross_Reference

* EPCG Changes done as per --> PR_EPCG_00001_Import_Purchase_Duty_Saved_EO
* Changes done by EBS Product Team on 05022013

_mepcg = oGlblPrdFeat.UdChkProd('epcg')
_meximaa = oGlblPrdFeat.UdChkProd('exim_aa')
_curscrobj = _Screen.ActiveForm
If _mepcg
	If Inlist(_curscrobj.pcvtype,"P1","EI")
		If Type("_curscrobj.EPCGctrlObjName1") = 'U'
			_curscrobj.AddProperty("EPCGctrlObjName1")
		Endif
		If Type("_curscrobj.EPCGctrlObjName2") = 'U'
			_curscrobj.AddProperty("EPCGctrlObjName2")
		Endif
		If Type("_curscrobj.EPCGctrlObjName3") = 'U'
			_curscrobj.AddProperty("EPCGctrlObjName3")
		Endif

		For i = 1 To _curscrobj.ControlCount Step 1
			ctrlObj = "_curscrobj.txtinfo"+Alltrim(Str(i))
			ctrlSrc = ctrlObj+".controlsource"
			If Type(ctrlObj) <> "U"
				If Alltrim(Upper(&ctrlSrc)) = "LMC_VW.LICEN_NO"
					ctrlObjName1 = &ctrlObj
					_curscrobj.EPCGctrlObjName1= ctrlObjName1
				Endif
				If Alltrim(Upper(&ctrlSrc)) = "LMC_VW.DUTY_SAVED"
					ctrlObjName2 = &ctrlObj
					_curscrobj.EPCGctrlObjName2= ctrlObjName2
				Endif
				If Alltrim(Upper(&ctrlSrc)) = "LMC_VW.TOT_EO"
					ctrlObjName3 = &ctrlObj
					_curscrobj.EPCGctrlObjName3= ctrlObjName3
				Endif
			Endif
		Endfor

&& License No. TextBox Valid Event --start
		If Type("_curscrobj.oEPCGLicenseNoHandler") = 'U'
			_curscrobj.AddProperty("oEPCGLicenseNoHandler")
		Endif
		_curscrobj.oEPCGLicenseNoHandler=Newobject("EPCGLicenseNoValidHandler")
		colLicenseObj = "ctrlObjName1"
		Bindevent(&colLicenseObj,"Valid",_curscrobj.oEPCGLicenseNoHandler,"Licensevalid",1)
&& License No. TextBox Valid Event --end
	Endif

	If Inlist(_curscrobj.pcvtype,"P1")
&& Item Grid AfterRowColChange --start
		If Type("_curscrobj.oEPCGGrdItemHandler") = 'U'
			_curscrobj.AddProperty("oEPCGGrdItemHandler")
		Endif
		_curscrobj.oEPCGGrdItemHandler=Newobject("EPCGGrdItemAftRowColChangeHandler")
		oGrdItemHandler = "_curscrobj.voupage.page1.grditem"
		Bindevent(&oGrdItemHandler,"AfterRowColChange",_curscrobj.oEPCGGrdItemHandler,"GrdItemAftRowColChange",1)

		If Type("_curscrobj.oEPCGTranDtySaved") = 'U'
			_curscrobj.AddProperty("oEPCGTranDtySaved")
			_curscrobj.oEPCGTranDtySaved= 0
		Endif

		If Type("_curscrobj.oEPCGTranTotEO") = 'U'
			_curscrobj.AddProperty("oEPCGTranTotEO")
			_curscrobj.oEPCGTranTotEO=0
		Endif


		If Type("_curscrobj.oEPCGTranItemDtySaved") = 'U'
			_curscrobj.AddProperty("oEPCGTranItemDtySaved")
			_curscrobj.oEPCGTranItemDtySaved= 0
		Endif

		If Type("_curscrobj.oEPCGTranItemTotEO") = 'U'
			_curscrobj.AddProperty("oEPCGTranItemTotEO")
			_curscrobj.oEPCGTranItemTotEO=0
		Endif

		If Type("_curscrobj.oEPCGLicenseNo") = 'U'
			_curscrobj.AddProperty("oEPCGLicenseNo")
			_curscrobj.oEPCGLicenseNo= ""
		Endif

		If Type("_curscrobj.oEPCGLicenseNoUpdtFlag") = 'U'
			_curscrobj.AddProperty("oEPCGLicenseNoUpdtFlag")
			_curscrobj.oEPCGLicenseNoUpdtFlag= .F.
		Endif

&& Item Grid AfterRowColChange --end
		If Type("_curscrobj.cntEPCGLcBalShow") <> "O"
			_curscrobj.AddObject('cntEPCGLcBalShow','container')
			_curscrobj.cntEPCGLcBalShow.Visible = .F.
			_curscrobj.cntEPCGLcBalShow.Top = ctrlObjName1.Top + 30
			_curscrobj.cntEPCGLcBalShow.Left = ctrlObjName1.Left
			_curscrobj.cntEPCGLcBalShow.Width = ctrlObjName1.Width - 150
			_curscrobj.cntEPCGLcBalShow.Height = ctrlObjName1.Height + 10
			_curscrobj.cntEPCGLcBalShow.BackColor = Rgb(255,255,230)
			_curscrobj.cntEPCGLcBalShow.BorderColor = Rgb(255,202,149)

			_curscrobj.cntEPCGLcBalShow.AddObject('lblText','Label')
			_curscrobj.cntEPCGLcBalShow.lbltext.Visible = .T.
			_curscrobj.cntEPCGLcBalShow.lbltext.FontSize = 7
			_curscrobj.cntEPCGLcBalShow.lbltext.BackStyle = 0
			_curscrobj.cntEPCGLcBalShow.lbltext.AutoSize = .T.
			_curscrobj.cntEPCGLcBalShow.lbltext.FontBold = .T.
			_curscrobj.cntEPCGLcBalShow.lbltext.Top = _curscrobj.cntEPCGLcBalShow.lbltext.Top + 5
			_curscrobj.cntEPCGLcBalShow.lbltext.Left = _curscrobj.cntEPCGLcBalShow.lbltext.Left + 5
			_curscrobj.cntEPCGLcBalShow.lbltext.Caption = ""

			_curscrobj.cntEPCGLcBalShow.AddObject('lblText2','Label')
			_curscrobj.cntEPCGLcBalShow.lbltext2.Visible = .T.
			_curscrobj.cntEPCGLcBalShow.lbltext2.FontSize = 7
			_curscrobj.cntEPCGLcBalShow.lbltext2.BackStyle = 0
			_curscrobj.cntEPCGLcBalShow.lbltext2.AutoSize = .T.
			_curscrobj.cntEPCGLcBalShow.lbltext2.FontBold = .T.
			_curscrobj.cntEPCGLcBalShow.lbltext2.Top = _curscrobj.cntEPCGLcBalShow.lbltext.Top + 15
			_curscrobj.cntEPCGLcBalShow.lbltext2.Left = _curscrobj.cntEPCGLcBalShow.lbltext.Left
			_curscrobj.cntEPCGLcBalShow.lbltext2.Caption = ""
		Endif

		If Type("_curscrobj.oEPCGLcNoMouseEnterHandler") = 'U'
			_curscrobj.AddProperty("oEPCGLcNoMouseEnterHandler")
		Endif

		If Type("_curscrobj.oEPCGLCBalAmt") = 'U'
			_curscrobj.AddProperty("oEPCGLCBalAmt")
			_curscrobj.oEPCGLCBalAmt= 0
		Endif

		If Type("_curscrobj.oEPCGEOBalAmt") = 'U'
			_curscrobj.AddProperty("oEPCGEOBalAmt")
			_curscrobj.oEPCGEOBalAmt= 0
		Endif

		_curscrobj.oEPCGLcNoMouseEnterHandler=Newobject("EPCGLcNoMouseEnterHandler")

		Bindevent(&colLicenseObj,"MouseEnter",_curscrobj.oEPCGLcNoMouseEnterHandler,"LcNoMouseEnter",1)
		Bindevent(&colLicenseObj,"MouseLeave",_curscrobj.oEPCGLcNoMouseEnterHandler,"LcNoMouseLeave",1)
		Bindevent(&colLicenseObj,"GotFocus",_curscrobj.oEPCGLcNoMouseEnterHandler,"LcNoGotFocus",1)
		Bindevent(&colLicenseObj,"LostFocus",_curscrobj.oEPCGLcNoMouseEnterHandler,"LcNoLostFocus",1)
	Endif
Endif
* End ---> PR_EPCG_00001_Import_Purchase_Duty_Saved_EO

* AA Changes done as per --> PR_AA_00001_Import_Purchase_AA
* Changes done by EBS Product Team on 29122012
If _meximaa
	If Inlist(_curscrobj.pcvtype,"P1","EI")
		If Type("_curscrobj.AActrlObjName1") = 'U'
			_curscrobj.AddProperty("AActrlObjName1")
		Endif
		If Type("_curscrobj.AActrlObjName2") = 'U'
			_curscrobj.AddProperty("AActrlObjName2")
		Endif
		If Type("_curscrobj.AActrlObjName3") = 'U'
			_curscrobj.AddProperty("AActrlObjName3")
		Endif

		For i = 1 To _curscrobj.ControlCount Step 1
			ctrlObj = "_curscrobj.txtinfo"+Alltrim(Str(i))
			ctrlSrc = ctrlObj+".controlsource"
			If Type(ctrlObj) <> "U"
				If Alltrim(Upper(&ctrlSrc)) = "LMC_VW.AALIC_NO"
					ctrlObjName1 = &ctrlObj
					_curscrobj.AActrlObjName1= ctrlObjName1
				Endif
				If Alltrim(Upper(&ctrlSrc)) = "LMC_VW.AA_INVAMT"
					ctrlObjName2 = &ctrlObj
					_curscrobj.AActrlObjName2= ctrlObjName2
				Endif
				If Alltrim(Upper(&ctrlSrc)) = "LMC_VW.AA_DUTY"
					ctrlObjName3 = &ctrlObj
					_curscrobj.AActrlObjName3= ctrlObjName3
				Endif
			Endif
		Endfor

&& License No. TextBox Valid Event --start
		If Type("_curscrobj.oAALicenseNoHandler") = "U"
			_curscrobj.AddProperty("oAALicenseNoHandler")
		Endif
		_curscrobj.oAALicenseNoHandler = Newobject("AALicenseNoValidHandler")
		colLicObj = "ctrlObjName1"
		Bindevent(&colLicObj,"Valid",_curscrobj.oAALicenseNoHandler,"Licensevalid",1)
&& License No. TextBox Valid Event --end

&& Item Grid AfterRowColChange --start
		If Type("_curscrobj.oAAGrdItemHandler") = "U"
			_curscrobj.AddProperty("oAAGrdItemHandler")
		Endif

		_curscrobj.oAAGrdItemHandler = Newobject("AAGrdItemAftRowColChangeHandler")
		oGrdItemHandler = "_curscrobj.voupage.page1.grditem"
		Bindevent(&oGrdItemHandler,"AfterRowColChange",_curscrobj.oAAGrdItemHandler,"GrdItemAftRowColChange",1)

		If Type("_curscrobj.oAATranDtySaved") = "U"
			_curscrobj.AddProperty("oAATranDtySaved")
			_curscrobj.oAATranDtySaved = 0
		Endif

		If Type("_curscrobj.oAATranItemDtySaved") = "U"
			_curscrobj.AddProperty("oAATranItemDtySaved")
			_curscrobj.oAATranItemDtySaved = 0
		Endif

		If Type("_curscrobj.oAALicenseNo") = "U"
			_curscrobj.AddProperty("oAALicenseNo")
			_curscrobj.oAALicenseNo = ""
		Endif

		If Type("_curscrobj.oAALCUpdtFlag") = 'U'
			_curscrobj.AddProperty("oAALCUpdtFlag")
			_curscrobj.oAALCUpdtFlag = .F.
		Endif

		If Type("_curscrobj.oAATranTotInvAmt") = "U"
			_curscrobj.AddProperty("oAATranTotInvAmt")
			_curscrobj.oAATranTotInvAmt = 0
		Endif

		If Type("_curscrobj.oAATranItemInvAmt") = "U"
			_curscrobj.AddProperty("oAATranItemInvAmt")
			_curscrobj.oAATranItemInvAmt = 0
		Endif

		If Type("_curscrobj.oAAEOInvAmt") = "U"
			_curscrobj.AddProperty("oAAEOInvAmt")
			_curscrobj.oAAEOInvAmt = 0
		Endif
&& Item Grid AfterRowColChange --end

		If Type("_curscrobj.cntAALcBalShow") <> "O"
			_curscrobj.AddObject('cntAALcBalShow','container')
			_curscrobj.cntAAlcbalshow.Visible = .F.
			_curscrobj.cntAAlcbalshow.Top = _curscrobj.AActrlObjName1.Top + 30
			_curscrobj.cntAAlcbalshow.Left = _curscrobj.AActrlObjName1.Left
			_curscrobj.cntAAlcbalshow.Width = _curscrobj.AActrlObjName1.Width - 150
			If _curscrobj.pcvtype = "P1"
				_curscrobj.cntAAlcbalshow.Height = _curscrobj.AActrlObjName1.Height + 30
			Else
				If _curscrobj.pcvtype = "EI"
					_curscrobj.cntAAlcbalshow.Height = _curscrobj.AActrlObjName1.Height
				Endif
			Endif
			_curscrobj.cntAAlcbalshow.BackColor = Rgb(255,255,230)
			_curscrobj.cntAAlcbalshow.BorderColor = Rgb(255,202,149)

			_curscrobj.cntAAlcbalshow.AddObject('lblText','Label')
			_curscrobj.cntAAlcbalshow.lbltext.Visible = .T.
			_curscrobj.cntAAlcbalshow.lbltext.FontSize = 7
			_curscrobj.cntAAlcbalshow.lbltext.BackStyle = 0
			_curscrobj.cntAAlcbalshow.lbltext.AutoSize = .T.
			_curscrobj.cntAAlcbalshow.lbltext.FontBold = .T.
			_curscrobj.cntAAlcbalshow.lbltext.Top = _curscrobj.cntAAlcbalshow.lbltext.Top + 5
			_curscrobj.cntAAlcbalshow.lbltext.Left = _curscrobj.cntAAlcbalshow.lbltext.Left + 5
			_curscrobj.cntAAlcbalshow.lbltext.Caption = ""

			_curscrobj.cntAAlcbalshow.AddObject('lblText2','Label')
			_curscrobj.cntAAlcbalshow.lbltext2.Visible = .T.
			_curscrobj.cntAAlcbalshow.lbltext2.FontSize = 7
			_curscrobj.cntAAlcbalshow.lbltext2.BackStyle = 0
			_curscrobj.cntAAlcbalshow.lbltext2.AutoSize = .T.
			_curscrobj.cntAAlcbalshow.lbltext2.FontBold = .T.
			_curscrobj.cntAAlcbalshow.lbltext2.Top = _curscrobj.cntAAlcbalshow.lbltext.Top + 15
			_curscrobj.cntAAlcbalshow.lbltext2.Left = _curscrobj.cntAAlcbalshow.lbltext.Left
			_curscrobj.cntAAlcbalshow.lbltext2.Caption = ""
* Changes done by EBS Product Team on 27022013
			_curscrobj.cntAAlcbalshow.AddObject('lblText3','Label')
			_curscrobj.cntAAlcbalshow.lbltext3.Visible = .T.
			_curscrobj.cntAAlcbalshow.lbltext3.FontSize = 7
			_curscrobj.cntAAlcbalshow.lbltext3.BackStyle = 0
			_curscrobj.cntAAlcbalshow.lbltext3.AutoSize = .T.
			_curscrobj.cntAAlcbalshow.lbltext3.FontBold = .T.
			_curscrobj.cntAAlcbalshow.lbltext3.Top = _curscrobj.cntAAlcbalshow.lbltext2.Top + 15
			_curscrobj.cntAAlcbalshow.lbltext3.Left = _curscrobj.cntAAlcbalshow.lbltext2.Left
			_curscrobj.cntAAlcbalshow.lbltext3.Caption = ""
* Changes done by EBS Product Team on 27022013
		Endif

		If Type("_curscrobj.oAALcNoMouseEnterHandler") = 'U'
			_curscrobj.AddProperty("oAALcNoMouseEnterHandler")
		Endif

		If Type("_curscrobj.oAALCBalAmt") = 'U'
			_curscrobj.AddProperty("oAALCBalAmt")
			_curscrobj.oAAlcbalamt = 0
		Endif

		If Type("_curscrobj.oAAActInvAmt") = 'U'
			_curscrobj.AddProperty("oAAActInvAmt")
			_curscrobj.oAAActInvAmt = 0
		Endif

		If Type("_curscrobj.oAAEOBalAmt") = 'U'
			_curscrobj.AddProperty("oAAEOBalAmt")
			_curscrobj.oAAEOBalAmt = 0
		Endif

		If Type("_curscrobj.oAATotInvAmt") = 'U'
			_curscrobj.AddProperty("oAATotInvAmt")
			_curscrobj.oAATotInvAmt = 0
		Endif

		_curscrobj.oAAlcnomouseenterhandler=Newobject("AALcNoMouseEnterHandler")

		Bindevent(&colLicObj,"MouseEnter",_curscrobj.oAAlcnomouseenterhandler,"LcNoMouseEnter",1)
		Bindevent(&colLicObj,"MouseLeave",_curscrobj.oAAlcnomouseenterhandler,"LcNoMouseLeave",1)
		Bindevent(&colLicObj,"GotFocus",_curscrobj.oAAlcnomouseenterhandler,"LcNoGotFocus",1)
		Bindevent(&colLicObj,"LostFocus",_curscrobj.oAAlcnomouseenterhandler,"LcNoLostFocus",1)
	Endif
Endif
* End ---> PR_AA_00001_Import_Purchase_AA

* AA Changes done as per --> PR_AA_00001_Import_Purchase_AA
* Changes done by EBS Product Team on 29122012
&& Below procedure show Balance Amount in tooltip style when user's cursor focus on License No. Textbox
Procedure AAlcbalamtshowcnt
_curscrobj = _Screen.ActiveForm
If Type('_curscrobj.addmode') = 'U'
	Return
Endif

If _curscrobj.addmode = .T. Or _curscrobj.editmode = .T.
	If Type("_curscrobj.cntAAlcbalshow") = "O"
		If Type("_curscrobj.oAALCBalAmt") <> 'U' Or Type("_curscrobj.oAAActInvAmt") <> 'U';
				Or Type("_curscrobj.oAAEOBalAmt") <> 'U' Or Type("_curscrobj.oAATotInvAmt") <> 'U'
*!*				IF _curscrobj.olcbalamt > 0 OR _curscrobj.oActInvAmt > 0 OR _curscrobj.oEOBalAmt > 0 OR _curscrobj.oTotInvAmt > 0
			_curscrobj.cntAAlcbalshow.Visible = .T.
			If main_vw.entry_ty = "P1"
				_curscrobj.cntAAlcbalshow.lbltext.Caption = "Total Duty Paid : " + Alltrim(Str(_curscrobj.oAAlcbalamt,16,2))
* Changes done by EBS Product Team on 27022013
				_curscrobj.cntAAlcbalshow.lbltext2.Caption = "Actual Invoice Value : " + Alltrim(Str(_curscrobj.oAAActInvAmt,16,2))
				_curscrobj.cntAAlcbalshow.lbltext3.Caption = "EO Invoice Value : " + Alltrim(Str(_curscrobj.oAAEOBalAmt,16,2))
* Changes done by EBS Product Team on 27022013

			Else
				If main_vw.entry_ty = "EI"
					_curscrobj.cntAAlcbalshow.lbltext.Caption = "Total Invoice Value : " + Alltrim(Str(_curscrobj.oAATotInvAmt,16,2))
				Endif
			Endif
			_curscrobj.cntAAlcbalshow.Refresh()
*!*				ENDIF
		Endif
	Endif
Endif
Endproc

&& Below procedure Hide Balance Amount when user has lost focus from the License No. Textbox
Procedure AAlcbalamthidecnt
_curscrobj = _Screen.ActiveForm

If Type('_curscrobj.addmode') = 'U'
	Return
Endif

If _curscrobj.addmode = .T. Or _curscrobj.editmode = .T.
	If Type("_curscrobj.cntAAlcbalshow") = "O"
		_curscrobj.cntAAlcbalshow.Visible = .F.
	Endif
Endif
Endproc

Define Class AAlcnomouseenterhandler As Custom
	Procedure lcnomouseenter
	Lparameters nbutton, nshift, nxcoord, nycoord
	AAlcbalamtshowcnt()
	Endproc

	Procedure lcnomouseleave
	Lparameters nbutton, nshift, nxcoord, nycoord
	AAlcbalamthidecnt()
	Endproc

	Procedure lcnogotfocus
	AAlcbalamtshowcnt()
	Endproc

	Procedure lcnolostfocus
	AAlcbalamthidecnt()
	Endproc
Enddefine

Define Class AAGrdItemAftRowColChangeHandler As Custom
	Procedure GrdItemAftRowColChange
	Parameters lcol
	_curscrobj = _Screen.ActiveForm
	curDtPoint = Select()  &&Get Old Data Area
	AACalProc(_curscrobj)
	Select(curDtPoint)  &&Set Old Data Area
	Endproc
Enddefine

Define Class AALicenseNoValidHandler As Custom
	Procedure Licensevalid
	_curscrobj = _Screen.ActiveForm
	curDtPoint = Select()  &&Get Old Data Area
	If Type("_curscrobj.AActrlObjName1") = "O"
		If Empty(_curscrobj.AActrlObjName1.Value)
			Return
		Endif
	Else
		Return
	Endif

	mSqlStr = "Select * from AA_MAST where licen_no = '" + Alltrim(_curscrobj.AActrlObjName1.Value) + "'"
	nretval = _curscrobj.sqlconobj.dataconn("EXE",company.dbname,mSqlStr,"curaamast","_curscrobj.nhandle",_curscrobj.DataSessionId,.T.)
	If nretval < 0
		Messagebox("Error found while validating License No. details..",0+16,vumess)
		_curscrobj.sqlconobj.sqlconnclose("_curscrobj.nhandle")
		Return 0
	Endif
	_curscrobj.sqlconobj.sqlconnclose("_curscrobj.nhandle")

	If Reccount("curaamast") = 0
		Messagebox("License No. not found in AA Master, please enter correct License No...!!",0+16,vumess)
		_curscrobj.AActrlObjName1.Value = ""
		Return 0
	Endif

	If main_vw.Date < curaamast.issue_dt
		Messagebox("Selected License No. issue date cannot be greater than transaction date...!!",0+16,vumess)
		_curscrobj.AActrlObjName1.Value = ""
		Return 0
	Endif

	If main_vw.Date > curaamast.expiry_dt
		Messagebox("Selected License No. expiry date cannot be less than transaction date...!!",0+16,vumess)
		_curscrobj.AActrlObjName1.Value = ""
		Return 0
	Endif

	If !Empty(_curscrobj.oAALicenseNo)
		If Alltrim(_curscrobj.oAALicenseNo) <> Alltrim(lmc_vw.aalic_no) And _curscrobj.oAALCUpdtFlag = .F.
			If Messagebox("Remove Previous License No. Details ?",4+32+256,vumess) = 7
				Replace lmc_vw.aalic_no With Alltrim(_curscrobj.oAALicenseNo) In lmc_vw
				_curscrobj.AActrlObjName1.Refresh()
				Return
			Else
				_curscrobj.oAALCUpdtFlag = .T.
			Endif
		Endif
	Endif

	If Inlist(_curscrobj.pcvtype,"P1")
		If _curscrobj.oAATranDtySaved = 0 Or _curscrobj.oAATranTotInvAmt = 0
			_curscrobj.AActrlObjName3.Value = 0.00
		Endif
		_curscrobj.oAAlcbalamt = curaamast.act_duty
		_curscrobj.oAAActInvAmt = curaamast.act_invamt
		_curscrobj.oAAEOBalAmt = curaamast.eo_invamt
		_curscrobj.AActrlObjName3.Refresh()
	Endif

	If Inlist(_curscrobj.pcvtype,"EI")
		If _curscrobj.oAATranTotInvAmt = 0
			_curscrobj.AActrlObjName2.Value = 0.00
		Endif
		_curscrobj.oAATotInvAmt = curaamast.exp_invamt
		_curscrobj.AActrlObjName2.Refresh()
	Endif

	_Tally=0
	Select Item From item_vw With (Buffering = .T.) Into Cursor curchkrec
	If _Tally # 0
		AACalProc(_curscrobj) && In between user has changed License No.
	Endif
	Select(curDtPoint)  && Set Old Data Area
	Endproc
Enddefine

Procedure AACalProc
Lparameters _curscrobj
&& If License no. has empty further processing will not consider.
If Type("_curscrobj.AActrlObjName1") = "O"
	_curscrobj.AActrlObjName1.Refresh()
	If Empty(_curscrobj.AActrlObjName1.Value)
		Return
	Endif
Else
	Return
Endif

If _curscrobj.addmode = .F. And _curscrobj.editmode = .F.
	Return
Endif

If !Used("curaamast")
	mSqlStr = "Select * from AA_MAST where licen_no = '" + Alltrim(_curscrobj.AActrlObjName1.Value) + "'"
	nretval = _curscrobj.sqlconobj.dataconn("EXE",company.dbname,mSqlStr,"curaamast","_curscrobj.nhandle",_curscrobj.DataSessionId,.T.)
	If nretval < 0
		Messagebox("Error found while collecting License No. details..",0+16,vumess)
		_curscrobj.sqlconobj.sqlconnclose("_curscrobj.nhandle")
		Return 0
	Endif
	_curscrobj.sqlconobj.sqlconnclose("_curscrobj.nhandle")
Endif

* AA Changes done as per --> PR_AA_00001_Import_Purchase_AA
* Changes done by EBS Product Team on 29122012

If Inlist(_curscrobj.pcvtype,"P1")

	Store 0 To lTotExAmt,mRecno
	ofilter = Filter("dcmast_vw")
	Set Filter To In dcmast_vw
	mRecno = Recno("item_vw")
	Go Top In item_vw
	Do While !Eof("item_vw")
		Go Top In dcmast_vw
		Do While !Eof("dcmast_vw")
			If dcmast_vw.Code = 'E'
				Do Case
				Case !Empty(dcmast_vw.a_s)
					ma_s = dcmast_vw.a_s
				Otherwise
					ma_s = '+'
				Endcase

				lfld_nm="Item_vw."+Allt(dcmast_vw.fld_nm)
				lTotExAmt = lTotExAmt + Evaluate(ma_s+lfld_nm)
			Endif
			Skip In dcmast_vw
		Enddo
		Skip In item_vw
	Enddo
	Set Filter To &ofilter In dcmast_vw
	Go mRecno In item_vw

* Changes done by EBS Product Team on 27022013
	Store 0 To lTotNetAmt,mRecno
	mRecno = Recno("item_vw")
	Go Top In item_vw
	Do While !Eof("item_vw")
		lTotNetAmt = lTotNetAmt + item_vw.gro_amt
		Skip In item_vw
	Enddo
	Go mRecno In item_vw

	If Used("curaamast")
		Store 0 To lDutySaved,lActInvamt,EOInvAmt
		Do Case
		Case (_curscrobj.oAALicenseNo <> lmc_vw.aalic_no)

			Do Case
			Case (_curscrobj.oAATranItemDtySaved <> lTotExAmt)
				If _curscrobj.oAATranDtySaved > 0 And _curscrobj.oAATranItemDtySaved > 0
					lDutySaved = _curscrobj.oAATranItemDtySaved - _curscrobj.oAATranItemDtySaved
					lDutySaved = lDutySaved + lTotExAmt
				Else
					lDutySaved = lTotExAmt
				Endif

				If Type("main_vw.aa_duty") <> "U"
					Replace aa_duty With lDutySaved In main_vw
				Else
					If Type("lmc_vw.aa_duty") <> "U"
						Replace aa_duty With lDutySaved In lmc_vw
					Endif
				Endif

			Case (_curscrobj.oAATranItemDtySaved = lTotExAmt)
				If Type("main_vw.aa_duty") <> "U"
					Replace aa_duty With lTotExAmt In main_vw
				Else
					If Type("lmc_vw.aa_duty") <> "U"
						Replace aa_duty With lTotExAmt In lmc_vw
					Endif
				Endif
			Endcase

			Do Case
			Case (_curscrobj.oAATranItemInvAmt <> lTotNetAmt)
				If _curscrobj.oAATranTotInvAmt > 0 And _curscrobj.oAATranItemInvAmt > 0
					lActInvamt = _curscrobj.oAATranItemInvAmt - _curscrobj.oAATranItemInvAmt
					lActInvamt = lActInvamt + lTotNetAmt
				Else
					lActInvamt = lTotNetAmt
				Endif

				If Type("main_vw.aa_invamt") <> "U"
					Replace aa_invamt With lActInvamt In main_vw
				Else
					If Type("lmc_vw.aa_invamt") <> "U"
						Replace aa_invamt With lActInvamt In lmc_vw
					Endif
				Endif

			Case (_curscrobj.oAATranItemInvAmt = lTotNetAmt)
				If Type("main_vw.aa_invamt") <> "U"
					Replace aa_invamt With lTotNetAmt In main_vw
				Else
					If Type("lmc_vw.aa_invamt") <> "U"
						Replace aa_invamt With lTotNetAmt In lmc_vw
					Endif
				Endif
			Endcase

		Case (_curscrobj.oAALicenseNo = lmc_vw.aalic_no)
			Do Case
			Case (_curscrobj.oAATranItemDtySaved <> lTotExAmt)
				If _curscrobj.oAATranDtySaved > 0 And _curscrobj.oAATranItemDtySaved > 0
					lDutySaved = _curscrobj.oAATranDtySaved - _curscrobj.oAATranItemDtySaved
					lDutySaved = lDutySaved + lTotExAmt
				Else
					lDutySaved = lTotExAmt
				Endif

				If Type("main_vw.aa_duty") <> "U"
					Replace aa_duty With lDutySaved In main_vw
				Else
					If Type("lmc_vw.aa_duty") <> "U"
						Replace aa_duty With lDutySaved In lmc_vw
					Endif
				Endif
			Case (_curscrobj.oAATranItemDtySaved = lTotExAmt)
				If _curscrobj.oAATranItemDtySaved <> 0 And _curscrobj.oAATranDtySaved = 0
					If Type("main_vw.aa_duty") <> "U"
						Replace aa_duty With lTotExAmt In main_vw
					Else
						If Type("lmc_vw.aa_duty") <> "U"
							Replace aa_duty With lTotExAmt In lmc_vw
						Endif
					Endif
				Endif
			Endcase

			Do Case
			Case (_curscrobj.oAATranItemInvAmt <> lTotNetAmt)
				If _curscrobj.oAATranTotInvAmt > 0 And _curscrobj.oAATranItemInvAmt > 0
					lActInvamt = _curscrobj.oAATranTotInvAmt - _curscrobj.oAATranItemInvAmt
					lActInvamt = lActInvamt + lTotNetAmt
				Else
					lActInvamt = lTotNetAmt
				Endif

				If Type("main_vw.aa_invamt") <> "U"
					Replace aa_invamt With lActInvamt In main_vw
				Else
					If Type("lmc_vw.aa_invamt") <> "U"
						Replace aa_invamt With lActInvamt In lmc_vw
					Endif
				Endif

			Case (_curscrobj.oAATranItemInvAmt = lTotNetAmt)
				If _curscrobj.oAATranTotInvAmt = 0
					If Type("main_vw.aa_invamt") <> "U"
						Replace aa_invamt With lTotNetAmt In main_vw
					Else
						If Type("lmc_vw.aa_invamt") <> "U"
							Replace aa_invamt With lTotNetAmt In lmc_vw
						Endif
					Endif
				Endif
			Endcase
		Endcase
		EOInvAmt = lTotNetAmt + (lTotNetAmt * curaamast.va_per / 100)
		If Type("main_vw.aa_eoinvamt") <> "U"
			Replace aa_eoinvamt With EOInvAmt In main_vw
		Else
			If Type("lmc_vw.aa_eoinvamt") <> "U"
				Replace aa_eoinvamt With EOInvAmt In lmc_vw
			Endif
		Endif
	Endif
Endif
* Changes done by EBS Product Team on 27022013
* End ---> PR_AA_00001_Import_Purchase_AA

* AA Changes done as per --> PR_AA_00002_Export_Sales_EO_AA
* Changes done by EBS Product Team on 04022013

If Inlist(_curscrobj.pcvtype,"EI")
	Store 0 To lTotNetAmt,mRecno
	mRecno = Recno("item_vw")
	Go Top In item_vw
	Do While !Eof("item_vw")
		lTotNetAmt = lTotNetAmt + item_vw.gro_amt
		Skip In item_vw
	Enddo
	Go mRecno In item_vw
* Changes done by EBS Product Team on 27022013
	If Used("curaamast")
		Do Case
		Case (_curscrobj.oAALicenseNo <> lmc_vw.aalic_no)
			Do Case
			Case (_curscrobj.oAATranItemInvAmt <> lTotNetAmt)
				Store 0 To lDutySaved
				If _curscrobj.oAATranTotInvAmt > 0 And _curscrobj.oAATranItemInvAmt > 0
					lDutySaved = _curscrobj.oAATranItemInvAmt - _curscrobj.oAATranItemInvAmt
					lDutySaved = lDutySaved + lTotNetAmt
				Else
					lDutySaved = lTotNetAmt
				Endif

				If Type("main_vw.aa_invamt") <> "U"
					Replace aa_invamt With lDutySaved In main_vw
				Else
					If Type("lmc_vw.aa_invamt") <> "U"
						Replace aa_invamt With lDutySaved In lmc_vw
					Endif
				Endif
			Case (_curscrobj.oAATranItemInvAmt = lTotNetAmt)
				If Type("main_vw.aa_invamt") <> "U"
					Replace aa_invamt With lTotNetAmt In main_vw
				Else
					If Type("lmc_vw.aa_invamt") <> "U"
						Replace aa_invamt With lTotNetAmt In lmc_vw
					Endif
				Endif
			Endcase
		Case (_curscrobj.oAALicenseNo = lmc_vw.aalic_no)
			Do Case
			Case (_curscrobj.oAATranItemInvAmt <> lTotNetAmt)
				Store 0 To lDutySaved
				If _curscrobj.oAATranTotInvAmt > 0 And _curscrobj.oAATranItemInvAmt > 0
					lDutySaved = _curscrobj.oAATranTotInvAmt - _curscrobj.oAATranItemInvAmt
					lDutySaved = lDutySaved + lTotNetAmt
				Else
					lDutySaved = lTotNetAmt
				Endif

				If Type("main_vw.aa_invamt") <> "U"
					Replace aa_invamt With lDutySaved In main_vw
				Else
					If Type("lmc_vw.aa_invamt") <> "U"
						Replace aa_invamt With lDutySaved In lmc_vw
					Endif
				Endif
			Case (_curscrobj.oAATranItemInvAmt = lTotNetAmt)
				If _curscrobj.oAATranTotInvAmt = 0
					If Type("main_vw.aa_invamt") <> "U"
						Replace aa_invamt With lTotNetAmt In main_vw
					Else
						If Type("lmc_vw.aa_invamt") <> "U"
							Replace aa_invamt With lTotNetAmt In lmc_vw
						Endif
					Endif
				Endif
			Endcase
		Endcase
	Endif
Endif
* Changes done by EBS Product Team on 27022013
* End ---> PR_AA_00002_Export_Sales_EO_AA

If Type('_curscrobj.AActrlObjName2')='O'
	_curscrobj.AActrlObjName2.Refresh()
Endif
If Type('_curscrobj.AActrlObjName3')='O'
	_curscrobj.AActrlObjName3.Refresh()
Endif
Endproc
* End ---> PR_AA_00001_Import_Purchase_AA

* EPCG Changes done as per --> PR_EPCG_00001_Import_Purchase_Duty_Saved_EO
* Changes done by EBS Product Team on 05022013
&& Below procedure show Balance Amount in tooltip style when user's cursor focus on License No. Textbox
Procedure EPCGlcbalamtshowcnt
_curscrobj = _Screen.ActiveForm

If Type('_curscrobj.addmode') = 'U'
	Return
Endif

If _curscrobj.addmode = .T. Or _curscrobj.editmode = .T.
	If Type("_curscrobj.cntEPCGLcBalShow") = "O"
		If Type("_curscrobj.oEPCGLCBalAmt") <> 'U' Or Type("_curscrobj.oEPCGEOBalAmt") <> 'U'
*!*				If _curscrobj.oEPCGLCBalAmt > 0 Or _curscrobj.oEPCGEOBalAmt > 0
			_curscrobj.cntEPCGLcBalShow.Visible = .T.
			_curscrobj.cntEPCGLcBalShow.lbltext.Caption = "Total Duty Saved : " + Alltrim(Str(_curscrobj.oEPCGLCBalAmt,16,2))
			_curscrobj.cntEPCGLcBalShow.lbltext2.Caption = "Total EO : " + Alltrim(Str(_curscrobj.oEPCGEOBalAmt,16,2))
			_curscrobj.cntEPCGLcBalShow.Refresh()
			_curscrobj.cntEPCGLcBalShow.lbltext.Refresh()
			_curscrobj.cntEPCGLcBalShow.lbltext2.Refresh()
*!*				Endif
		Endif
	Endif
Endif
Endproc

&& Below procedure Hide Balance Amount when user has lost focus from the License No. Textbox
Procedure EPCGlcbalamthidecnt
_curscrobj = _Screen.ActiveForm

If Type('_curscrobj.addmode') = 'U'
	Return
Endif

If _curscrobj.addmode = .T. Or _curscrobj.editmode = .T.
	If Type("_curscrobj.cntEPCGLcBalShow") = "O"
		_curscrobj.cntEPCGLcBalShow.Visible = .F.
	Endif
Endif
Endproc

Define Class EPCGLcNoMouseEnterHandler As Custom
	Procedure lcnomouseenter
	Lparameters nbutton, nshift, nxcoord, nycoord
	EPCGlcbalamtshowcnt()
	Endproc

	Procedure lcnomouseleave
	Lparameters nbutton, nshift, nxcoord, nycoord
	EPCGlcbalamthidecnt()
	Endproc

	Procedure lcnogotfocus
	EPCGlcbalamtshowcnt()
	Endproc

	Procedure lcnolostfocus
	EPCGlcbalamthidecnt()
	Endproc
Enddefine

Define Class EPCGGrdItemAftRowColChangeHandler As Custom
	Procedure GrdItemAftRowColChange
	Parameters lcol
	_curscrobj = _Screen.ActiveForm
	curDtPoint = Select()  && Get Old Data Area
	EPCGCalProc(_curscrobj)
	Select(curDtPoint)  && Set Old Data Area
	Endproc
Enddefine


Define Class EPCGLicenseNoValidHandler As  Custom

	Procedure Licensevalid
	_curscrobj = _Screen.ActiveForm
	curDtPoint = Select()  && Get Old Data Area
	If Type('_curscrobj.EPCGctrlObjName1') = 'O'
		If Empty(_curscrobj.EPCGctrlObjName1.Value)
			Return
		Endif
	Else
		Return
	Endif

	strSql = "Select * from epcg_mast where licen_no ='" + Alltrim(_curscrobj.EPCGctrlObjName1.Value) + "'"
	nretval = _curscrobj.sqlconobj.dataconn("EXE",company.dbname,strSql,"curepcgmast","_curscrobj.nhandle",_curscrobj.DataSessionId,.T.)
	If nretval<0
		Messagebox("Error found while validate License no. details..",16,vumess)
		_curscrobj.sqlconobj.sqlconnclose("_curscrobj.nhandle")
		Return 0
	Endif
	_curscrobj.sqlconobj.sqlconnclose("_curscrobj.nhandle")

	If Reccount("curepcgmast") = 0
		Messagebox("License no. not found in Epcg master, please enter correct License no...!!",16,vumess)
		_curscrobj.EPCGctrlObjName1.Value = ""
		Return 0
	Endif

	If main_vw.Date < curepcgmast.issue_dt
		Messagebox("Selected license no. issue date should not be less than transaction date...",16,vumess)
		_curscrobj.ctrlObjName1.Value = ""
		Return 0
	Endif

	If main_vw.Date > curepcgmast.expiry_dt
		Messagebox("Selected License no. expiry date cannot be Less than Transaction date..!!",16,vumess)
		_curscrobj.EPCGctrlObjName1.Value = ""
		Return 0
	Endif

	If Inlist(_curscrobj.pcvtype,"P1")

		If !Empty(_curscrobj.oEPCGLicenseNo)
			If Alltrim(_curscrobj.oEPCGLicenseNo) <> Alltrim(lmc_vw.licen_no) And _curscrobj.oEPCGLicenseNoUpdtFlag = .F.
				If Messagebox("Remove Previous License No. Details ?",4+32+256,vumess) = 7
					Replace lmc_vw.licen_no With Alltrim(_curscrobj.oEPCGLicenseNo) In lmc_vw
					_curscrobj.EPCGctrlObjName1.Refresh()
					Return
				Else
					_curscrobj.oEPCGLicenseNoUpdtFlag = .T.
				Endif
			Endif
		Endif

		If _curscrobj.oEPCGTranTotEO = 0 And  _curscrobj.oEPCGTranDtySaved = 0
			_curscrobj.EPCGctrlObjName2.Value = 0.00
			_curscrobj.EPCGctrlObjName3.Value = 0.00
		Endif

		_curscrobj.oEPCGLCBalAmt = curepcgmast.duty_saved
		_curscrobj.oEPCGEOBalAmt = curepcgmast.tot_eo

		_curscrobj.EPCGctrlObjName2.Refresh()
		_curscrobj.EPCGctrlObjName3.Refresh()


		_Tally=0
		Select Item From item_vw With (Buffering = .T.) Into Cursor curchkrec
		If _Tally # 0
			EPCGCalProc(_curscrobj) && In between user has changed License No.
		Endif
		Select(curDtPoint)  && Set Old Data Area
	Endif
	Endproc
Enddefine

Procedure EPCGCalProc
Lparameters _curscrobj
Store 0 To lTotExAmt
&& If License no. has empty further processing will not consider.
If Type('_curscrobj.EPCGctrlObjName1') = 'O'
	_curscrobj.EPCGctrlObjName1.Refresh()
	If Empty(_curscrobj.EPCGctrlObjName1.Value)
		Return
	Endif
Else
	Return
Endif

If _curscrobj.addmode = .F. And _curscrobj.editmode = .F.
	Return
Endif

If !Used('curepcgmast')
	strSql = "Select * from epcg_mast where licen_no ='" + Alltrim(_curscrobj.EPCGctrlObjName1.Value) + "'"
	nretval = _curscrobj.sqlconobj.dataconn("EXE",company.dbname,strSql,"curepcgmast","_curscrobj.nhandle",_curscrobj.DataSessionId,.T.)
	If nretval<0
		Messagebox("Error found while collecting License no. details..",16,vumess)
		_curscrobj.sqlconobj.sqlconnclose("_curscrobj.nhandle")
		Return 0
	Endif
	_curscrobj.sqlconobj.sqlconnclose("_curscrobj.nhandle")
Endif
*!*	SELECT dcmast_vw
*!*	COPY TO "D:\tempdcmast_vw"
*!*	SELECT dcmast_vw
*!*	SELECT dcmast_vw
*!*	SET FILTER TO
*!*	COPY TO "D:\tempdcmast_vw_1"
ofilter = Filter("dcmast_vw")
Set Filter To In dcmast_vw
Go Top In dcmast_vw
Do While !Eof('dcmast_vw')
	If dcmast_vw.Code = 'E'
		If Upper(Allt(dcmast_vw.fld_nm)) = "BCDAMT"
			If item_vw.u_paid_cvd = .F.
				Skip In dcmast_vw
				Loop
			Endif
		Endif

		Do Case
		Case !Empty(dcmast_vw.a_s)
			ma_s = dcmast_vw.a_s
		Otherwise
			ma_s = '+'
		Endcase

		lfld_nm="Item_vw."+Allt(dcmast_vw.fld_nm)
		lTotExAmt = lTotExAmt + Evaluate(ma_s+lfld_nm)
	Endif
	Skip In dcmast_vw
Enddo
Set Filter To &ofilter In dcmast_vw
lTotExAmt = lTotExAmt -(lTotExAmt * Iif(Isnull(curepcgmast.duty_rate),0.00,curepcgmast.duty_rate) / 100)

Replace duty_saved With lTotExAmt In item_vw

If Used('curepcgmast')
	Replace basic_eo With (lTotExAmt * curepcgmast.n_times) In item_vw

	_Tally=0
	Select Sum(basic_eo) As sumbasiceo,Sum(duty_saved) As sumdutysaved From item_vw With (Buffering = .T.) ;
		INTO Cursor cursumitem_vw

	If _Tally # 0
&& Duty Saved
		If _curscrobj.oEPCGLicenseNo <> lmc_vw.licen_no
			If _curscrobj.oEPCGTranItemDtySaved <> cursumitem_vw.sumdutysaved
				Store 0 To lDutySaved
				If _curscrobj.oEPCGTranDtySaved > 0 And _curscrobj.oEPCGTranItemDtySaved > 0
					lDutySaved = _curscrobj.oEPCGTranDtySaved - _curscrobj.oEPCGTranItemDtySaved
					lDutySaved = lDutySaved + cursumitem_vw.sumdutysaved
				Else
					lDutySaved = cursumitem_vw.sumdutysaved
				Endif

				If Type("main_vw.duty_saved") <> "U"
					Replace duty_saved With lDutySaved In main_vw
				Else
					If Type("lmc_vw.duty_saved") <> "U"
						Replace duty_saved With lDutySaved In lmc_vw
					Endif
				Endif
			Endif
&& Basic_Eo
			If _curscrobj.oEPCGTranItemTotEO <>  cursumitem_vw.sumbasiceo
				Store 0 To lDutySaved
				If (_curscrobj.oEPCGTranTotEO > 0 And _curscrobj.oEPCGTranItemTotEO > 0)
					lDutySaved = _curscrobj.oEPCGTranTotEO - _curscrobj.oEPCGTranItemTotEO
					lDutySaved = lDutySaved + cursumitem_vw.sumbasiceo
				Else
					lDutySaved = cursumitem_vw.sumbasiceo
				Endif

				If Type("main_vw.tot_eo") <> 'U'
					If cursumitem_vw.sumbasiceo > 0
						Replace tot_eo With lDutySaved In main_vw
					Endif
				Else
					If Type("lmc_vw.tot_eo") <> 'U'
						If cursumitem_vw.sumbasiceo > 0
							Replace tot_eo With lDutySaved In lmc_vw
						Endif
					Endif
				Endif
			Endif
		Else
&& Duty Saved
			If _curscrobj.oEPCGTranItemDtySaved <> cursumitem_vw.sumdutysaved
				Store 0 To lDutySaved
				If (_curscrobj.oEPCGTranDtySaved > 0 And _curscrobj.oEPCGTranItemDtySaved > 0)
					lDutySaved = _curscrobj.oEPCGTranDtySaved - _curscrobj.oEPCGTranItemDtySaved
					lDutySaved = lDutySaved + cursumitem_vw.sumdutysaved
				Else
					lDutySaved = cursumitem_vw.sumdutysaved
				Endif

				If Type("main_vw.duty_saved") <> 'U'
					Replace duty_saved With lDutySaved In main_vw
				Else
					If Type("lmc_vw.duty_saved") <> 'U'
						Replace duty_saved With lDutySaved In lmc_vw
					Endif
				Endif
			Endif

&& Basic_Eo
			If _curscrobj.oEPCGTranItemTotEO <>  cursumitem_vw.sumbasiceo
				Store 0 To lDutySaved

				If (_curscrobj.oEPCGTranTotEO > 0 And _curscrobj.oEPCGTranItemTotEO > 0)
					lDutySaved = _curscrobj.oEPCGTranTotEO - _curscrobj.oEPCGTranItemTotEO
					lDutySaved = lDutySaved + cursumitem_vw.sumbasiceo
				Else
					lDutySaved = cursumitem_vw.sumbasiceo
				Endif

				If Type("main_vw.tot_eo") <> 'U'
					If cursumitem_vw.sumbasiceo > 0
						Replace tot_eo With lDutySaved In main_vw
					Endif
				Else
					If Type("lmc_vw.tot_eo") <> 'U'
						If cursumitem_vw.sumbasiceo > 0
							Replace tot_eo With lDutySaved In lmc_vw
						Endif
					Endif
				Endif
			Endif
		Endif
		If Type('_curscrobj.EPCGctrlObjName2')='O'
			_curscrobj.EPCGctrlObjName2.Refresh()
		Endif

		If Type('_curscrobj.EPCGctrlObjName3')='O'
			_curscrobj.EPCGctrlObjName3.Refresh()
		Endif
	Endif
	If Used("cursumitem_vw")
		Use In cursumitem_vw
	Endif
Endif
Endproc
* End ---> PR_EPCG_00001_Import_Purchase_Duty_Saved_EO

* Below Changes done as per --> CR_KOEL_0002_Export_Sales_Transaction_&_Export_LC_Master_Cross_Reference
* Changes done by EBS Product Team
&& Below procedure show LC Balance Amount in tooltip style when user's cursor focus on LC No. Textbox
Procedure lcbalamtshowcnt
_curscrobj = _Screen.ActiveForm

If Type('_curscrobj.addmode') = 'U'
	Return
Endif

If _curscrobj.addmode = .T. Or _curscrobj.editmode = .T.
	If Type("_curscrobj.cntLcBalShow") = "O"
		If Type("_curscrobj.oLCBalAmt") <> 'U'
			If _curscrobj.olcbalamt > 0
				_curscrobj.cntlcbalshow.Visible = .T.
				_curscrobj.cntlcbalshow.lbltext.Caption = "LC Balance Amt : " + Alltrim(Str(_curscrobj.olcbalamt,16,2))
				_curscrobj.cntlcbalshow.Refresh()
			Endif
		Endif
	Endif
Endif
Endproc

&& Below procedure Hide LC Balance Amount when user has lost focus from the LC No. Textbox
Procedure lcbalamthidecnt
_curscrobj = _Screen.ActiveForm

If Type('_curscrobj.addmode') = 'U'
	Return
Endif

If _curscrobj.addmode = .T. Or _curscrobj.editmode = .T.
	If Type("_curscrobj.cntLcBalShow") = "O"
		_curscrobj.cntlcbalshow.Visible = .F.
	Endif
Endif
Endproc

&& Below Procedure call from LC No. Textbox valid event.
Procedure dovalidlcno
_curscrobj = _Screen.ActiveForm
Set Step On
If Alltrim(Upper(_curscrobj.Name)) != "VOUCHER"
	Return 0
Endif
nhandle=0
loldarea = Select()

If _curscrobj.addmode = .F. And _curscrobj.editmode = .F.
	Return
Endif

&& Validating LC no. when user enter LC no.
If !Empty(main_vw.lc_no)
	sq1 = "Select * from export_lc_mast where lc_no=?main_vw.lc_no and ?main_vw.date between lc_date and lc_expiryextendate"
	nretval = _curscrobj.sqlconobj.dataconn([EXE],company.dbname,sq1,"cur_incorlcno","_curscrobj.nHandle",_curscrobj.DataSessionId)
	If nretval < 0
		nretval = _curscrobj.sqlconobj.sqlconnclose("_curscrobj.nHandle")
		Return 0
	Endif
	nretval = _curscrobj.sqlconobj.sqlconnclose("_curscrobj.nHandle")
	Select cur_incorlcno

	If Reccount("cur_incorlcno") <= 0
		Messagebox("Selected LC No. not found in Export LC Master OR Transaction Date not between LC Date and Expiry Date."+Chr(13)+;
			"Please check in Export LC Master.",0+64,vumess)
		Replace main_vw.lc_no With '' In main_vw
		_curscrobj.olcbalamt = 0
*!*			_curscrobj.txtinfo1.REFRESH()
		_curscrobj.ctrlLCObjName.Refresh()  &&changed by priyanka_28052013
		Return 0
	Endif
Endif

If !Empty(_curscrobj.olcno)
	If Alltrim(_curscrobj.olcno) <> Alltrim(main_vw.lc_no) And _curscrobj.oLCUpdtFlag = .F.
		If Messagebox("Remove Previous LC Details ?",4+32+256,vumess) = 7
			Replace main_vw.lc_no With Alltrim(_curscrobj.olcno) In main_vw
*!*				_curscrobj.txtinfo1.REFRESH()
			_curscrobj.ctrlLCObjName.Refresh()  &&changed by priyanka_28052013
			Return
		Else
			_curscrobj.oLCUpdtFlag = .T.
		Endif
	Endif
Endif
Set Step On
&& Get Bank details of selected LC No.
If !Empty(main_vw.lc_no)
	sq1 = "Select a.*,b.ac_name,b.add1,b.add2,b.add3 from export_lc_mast a left join ac_mast b on (a.lc_iss_bankname=b.ac_name) where a.lc_no=?main_vw.lc_no"
	nretval = _curscrobj.sqlconobj.dataconn([EXE],company.dbname,sq1,"cur_lcno","_curscrobj.nHandle",_curscrobj.DataSessionId)
	If nretval < 0
		nretval = _curscrobj.sqlconobj.sqlconnclose("_curscrobj.nHandle")
		Return 0
	Endif
	nretval = _curscrobj.sqlconobj.sqlconnclose("_curscrobj.nHandle")
	Select cur_lcno

	If main_vw.fcnet_amt > 0
		If Type("_curscrobj.oPrevInvAmt") <> 'U' And _curscrobj.oPrevInvAmt > 0
			If (cur_lcno.lc_balamt + _curscrobj.oPrevInvAmt  <= main_vw.fcnet_amt)
				Messagebox("LC Balance Amount found less than Invoice Amount for the selected LC No. Please Select the appropriate LC No.",0+64,vumess)
				Replace main_vw.lc_no With '' In main_vw
				_curscrobj.olcbalamt = 0
*!*				_curscrobj.txtinfo1.REFRESH()
				_curscrobj.ctrlLCObjName.Refresh()  &&changed by priyanka_28052013
				Return 0
			Endif
		Endif
	Endif
Endif

&& Showing the LC Balance Amount in tooltip style.
If Type("_curscrobj.oLCBalAmt") <> 'U'
	If Used("cur_lcno")
		_curscrobj.olcbalamt = cur_lcno.lc_balamt
		If _curscrobj.olcbalamt > 0
			_curscrobj.cntlcbalshow.lbltext.Caption = "LC Balance Amt : " + Alltrim(Str(_curscrobj.olcbalamt,16,2))
			_curscrobj.cntlcbalshow.Refresh()
		Endif
	Endif
Endif

&& Clear existing fields details in Add Mode if User removes LC no.
If Empty(main_vw.lc_no)
	If Used("curUpMyValues")
		If Reccount("curUpMyValues") >0
			Do doclearfields
		Endif
	Endif
	Return
Endif

&& Clear existing fields details in Add Mode if User changes LC no.
If Used("curUpMyValues")
	If Reccount("curUpMyValues") >0
		Do doclearfields
	Endif
Endif

&& Create Temp Cursor to keep track on fields to clear in add mode
If !Used("curUpMyValues")
	Create Cursor curUpMyValues(m_tblname Character(20),m_fields Character(254),m_type Character(1),isflag l)
Endif

If Used("cur_lcno")
	If Type("main_vw.FrmDocCr") <> 'U'
		Replace main_vw.frmdoccr With cur_lcno.lc_frmdoccr In main_vw
	Else
		If Type("lmc_vw.FrmDocCr") <> 'U'
			Replace lmc_vw.frmdoccr With cur_lcno.lc_frmdoccr In lmc_vw
		Endif
	Endif

	If Type("main_vw.DocCrNo") <> 'U'
		Replace main_vw.doccrno With cur_lcno.lc_doccrno In main_vw
	Else
		If Type("lmc_vw.DocCrNo") <> 'U'
			Replace lmc_vw.doccrno With cur_lcno.lc_doccrno In lmc_vw
		Endif
	Endif

	If Type("main_vw.PerInv_No") <> 'U'
		Replace main_vw.perinv_no With cur_lcno.lc_perinv_no In main_vw
	Else
		If Type("lmc_vw.PerInv_No") <> 'U'
			Replace lmc_vw.perinv_no With cur_lcno.lc_perinv_no In lmc_vw
		Endif
	Endif

	If Type("main_vw.PerInv_Dt") <> 'U'
		Replace main_vw.perinv_dt With Ttod(cur_lcno.lc_perinv_date) In main_vw
	Else
		If Type("lmc_vw.PerInv_Dt") <> 'U'
			Replace lmc_vw.perinv_dt With Ttod(cur_lcno.lc_perinv_date) In lmc_vw
		Endif
	Endif

	If Type("main_vw.DraftsAt") <> 'U'
		Replace main_vw.draftsat With cur_lcno.lc_draftsat In main_vw
	Else
		If Type("lmc_vw.DraftsAt") <> 'U'
			Replace lmc_vw.draftsat With cur_lcno.lc_draftsat In lmc_vw
		Endif
	Endif

	If Type("main_vw.u_bank") <> 'U'
		Replace main_vw.u_bank With cur_lcno.lc_iss_bankname In main_vw
	Else
		If Type("lmc_vw.u_bank") <> 'U'
			Replace lmc_vw.u_bank With cur_lcno.lc_iss_bankname In lmc_vw
		Endif
	Endif

	If Type("main_vw.u_throu1") <> 'U'
		Replace main_vw.u_throu1 With cur_lcno.add1 In main_vw
	Else
		If Type("lmc_vw.u_throu1") <> 'U'
			Replace lmc_vw.u_throu1 With cur_lcno.add1 In lmc_vw
		Endif
	Endif

	If Type("main_vw.u_throu2") <> 'U'
		Replace main_vw.u_throu2 With cur_lcno.add2 In main_vw
	Else
		If Type("lmc_vw.u_throu2") <> 'U'
			Replace lmc_vw.u_throu2 With cur_lcno.add2 In lmc_vw
		Endif
	Endif

	If Type("main_vw.u_throu3") <> 'U'
		Replace main_vw.u_throu3 With cur_lcno.add3 In main_vw
	Else
		If Type("lmc_vw.u_throu3") <> 'U'
			Replace lmc_vw.u_throu3 With cur_lcno.add3 In lmc_vw
		Endif
	Endif

&& Below fields get updated from XML Data, if below fields are empty
&&changed by priyanka_28052013
	If Type("main_vw.U_Loading") <> 'U' And Empty(main_vw.u_loading)
		Replace main_vw.u_loading With cur_lcno.u_loading In main_vw
		currvalues("main_vw","U_Loading","C",.T.)
	Else
		If Type("lmc_vw.U_Loading") <> 'U' And Empty(lmc_vw.u_loading)
			Replace lmc_vw.u_loading With cur_lcno.u_loading In lmc_vw
			currvalues("lmc_vw","U_Loading","C",.T.)
		Endif

	Endif

	If Type("main_vw.U_Port") <> 'U' And Empty(main_vw.u_port)
		Replace main_vw.u_port With cur_lcno.u_port In main_vw
		currvalues("main_vw","U_Port","C",.T.)
	Else
		If Type("lmc_vw.U_Port") <> 'U' And Empty(lmc_vw.u_port)
			Replace lmc_vw.u_port With cur_lcno.u_port In lmc_vw
			currvalues("lmc_vw","U_Port","C",.T.)
		Endif
	Endif

	If Type("main_vw.U_Terms") <> 'U' And Empty(main_vw.U_Terms)
		Replace main_vw.U_Terms With cur_lcno.u_payment In main_vw
		currvalues("main_vw","U_Terms","C",.T.)
	Else
		If Type("lmc_vw.U_Terms") <> 'U' And Empty(lmc_vw.U_Terms)
			Replace lmc_vw.U_Terms With cur_lcno.u_payment In lmc_vw
			currvalues("lmc_vw","U_Terms","C",.T.)
		Endif
	Endif

	If Type("main_vw.U_Receipt") <> 'U' And Empty(main_vw.u_receipt)
		Replace main_vw.u_receipt With cur_lcno.u_receipt In main_vw
		currvalues("main_vw","u_receipt","C",.T.)
	Else
		If Type("lmc_vw.U_Receipt") <> 'U' And Empty(lmc_vw.u_receipt)
			Replace lmc_vw.u_receipt With cur_lcno.u_receipt In lmc_vw
			currvalues("lmc_vw","u_receipt","C",.T.)
		Endif
	Endif

	If Type("main_vw.U_Fdesti") <> 'U' And Empty(main_vw.u_fdesti)
		Replace main_vw.u_fdesti With cur_lcno.u_fdesti In main_vw
		currvalues("main_vw","U_Fdesti","C",.T.)
	Else
		If Type("lmc_vw.U_Fdesti") <> 'U' And Empty(lmc_vw.u_fdesti)
			Replace lmc_vw.u_fdesti With cur_lcno.u_fdesti In lmc_vw
			currvalues("lmc_vw","U_Fdesti","C",.T.)
		Endif
	Endif
&&changed by priyanka_28052013
Endif

If Used("cur_lcno")
	Use In cur_lcno
Endif

If Used("cur_incorlcno")
	Use In cur_incorlcno
Endif

Select (loldarea)
Endproc

Procedure doclearfields
Store "" To lstr
Select curUpMyValues
Go Top
Do While !Eof("curUpMyValues")
	If curUpMyValues.m_type = "C"
		lstr = "replace " + Alltrim(curUpMyValues.m_fields) + " with '' in " + Alltrim(curUpMyValues.m_tblname)
	Else
		If Inlist(curUpMyValues.m_type,"D","T")
			lstr = "replace " + Alltrim(curUpMyValues.m_fields) + " with {} in " + Alltrim(curUpMyValues.m_tblname)
		Endif
	Endif
	&lstr
	Skip In curUpMyValues
Enddo

If Used("curUpMyValues")
	Use In curUpMyValues
Endif
Endproc

Procedure currvalues
Lparameters mtblname,mfieldname,mtype,mflag
Insert Into curUpMyValues(m_tblname,m_fields,m_type,isflag) Values(mtblname,mfieldname,mtype,mflag)
Endproc

Define Class lcnobtnhandler As Custom
	Procedure lcnobtnclick
	_curscrobj =_Screen.ActiveForm

	If Alltrim(Upper(_curscrobj.mousecurfld)) = Alltrim(_curscrobj.ctrlLCObjName.FullName) &&"THISFORM.TXTINFO1"   &&changed by priyanka_28052013
		Do dovalidlcno
	Endif
	Endproc
Enddefine

Define Class lcnomouseenterhandler As Custom
	Procedure lcnomouseenter
	Lparameters nbutton, nshift, nxcoord, nycoord
	lcbalamtshowcnt()
	Endproc

	Procedure lcnomouseleave
	Lparameters nbutton, nshift, nxcoord, nycoord
	lcbalamthidecnt()
	Endproc

	Procedure lcnogotfocus
	lcbalamtshowcnt()
	Endproc

	Procedure lcnolostfocus
	lcbalamthidecnt()
	Endproc
Enddefine

Define Class lcnovalidhandler As Custom
	Procedure lcnovalid
	Do dovalidlcno
	Endproc
Enddefine

* End --> CR_KOEL_0002_Export_Sales_Transaction_&_Export_LC_Master_Cross_Reference
&& changes by EBS team on 07/03/14 for Bug-21466,21467,21468 end

Define Class pageappduty As Page
	Procedure Deactivate()
	Thisform.show_ac_it_info(2,.F.)
	Thisform.stkrpt.Visible = .F.
	This.FontBold=.F.
	Thisform.activateclicked =.F.
	Return

	Procedure Activate()
	This.FontBold=.T.
	Thisform.activateclicked =.F.
	Return
Enddefine
*!*	** End : Added by Uday on dated 26/12/2011 for Exim
