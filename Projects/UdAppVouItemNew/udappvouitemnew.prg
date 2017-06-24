Local tcol
tcol=0

&& Added by Shrikant S. on 01/06/2010 Start
Select dcmast_vw
*!*	Replace All Code With "A" For Inlist(fld_nm,'CCDAMT','HCDAMT','BCDAMT') And entry_ty='P1'		&& Changed by Shrikant S.on 30/05/2017 for GST

Replace All Code With "A" For Inlist(fld_nm,'U_CUSTAMT','CCDAMT','HCDAMT') And Inlist(entry_ty,'EI','ST') && Added by Ajay Jaiswal on 23/02/2012 for EXIM
&& Added by Shrikant S. on 01/06/2010 End
*!*	Replace All Code With "A" For Inlist(fld_nm,'BCDAMT','INSAMT','FREAMT') And entry_ty='P1'		&& Added by Shrikant S. on 04/10/2016 For GST		&& Commented by Shrikant S.on 30/05/2017 for GST

* Birendra : Start - Changes for TKT-9537 on 20 sept 2011
*!*	IF INLIST(.pcvtype,'P1') AND (([vuexc] $ vchkprod))			&& Commented by Shrikant S. on 03/01/2017 for GST
&& Commented by Shrikant S. on 03/01/2017 for GST		&&Start
*!*	If Inlist(.pcvtype,'P1') And (([vuexc] $ vchkprod) Or ([vugst] $ vchkprod))		&& Commented by Shrikant S. on 03/01/2017 for GST
*!*		tcol1=.voupage.page1.grditem.ColumnCount
*!*		tcol2 = 1
*!*		tcol  = 0
*!*		For tcol2 = 1 To tcol1
*!*			If Inlist(Upper(.voupage.page1.grditem.Columns(tcol2).ControlSource),'ITEM_VW.CCDAMT','ITEM_VW.HCDAMT','ITEM_VW.CCDPER','ITEM_VW.HCDPER','ITEM_VW.BCDAMT','ITEM_VW.BCDPER')
*!*				.voupage.page1.grditem.Columns(tcol2).Tag=Strtran(.voupage.page1.grditem.Columns(tcol2).Tag,"#E#","#A#",1,1)
*!*			Endif
*!*		Endfor
*!*	Endif
&& Commented by Shrikant S. on 03/01/2017 for GST		&&end

* Birendra : End

&& Added by Ajay Jaiswal on 23/02/2012 for EXIM --> Start
If Inlist(.pcvtype,'EI','ST') And (([vuexc] $ vchkprod))
	tcol1=.voupage.page1.grditem.ColumnCount
	tcol2 = 1
	tcol  = 0
	For tcol2 = 1 To tcol1
		If Inlist(Upper(.voupage.page1.grditem.Columns(tcol2).ControlSource),'ITEM_VW.U_CUSTAMT','ITEM_VW.CCDAMT','ITEM_VW.HCDAMT','ITEM_VW.U_CUSTPER','ITEM_VW.CCDPER','ITEM_VW.HCDPER')
			.voupage.page1.grditem.Columns(tcol2).Tag=Strtran(.voupage.page1.grditem.Columns(tcol2).Tag,"#E#","#A#",1,1)
		Endif
	Endfor
Endif
&& Added by Ajay Jaiswal on 23/02/2012 for EXIM --> End


If Inlist(.pcvtype,'WK','ST','DC') And (([vuexc] $ vchkprod)) &&  Or ([vuinv] $ vchkprod) Comment By Hetal Dt 240310
	tcol1=.voupage.page1.grditem.ColumnCount
	tcol2 = 1
	tcol  = 0
	For tcol2 = 1 To tcol1
		If Upper(.voupage.page1.grditem.Columns(tcol2).ControlSource) = 'ITEM_VW.U_BOMDET'
			tcol = tcol2
			Exit
		Endif
	Endfor
	If tcol = 0
		tcol = tcol1 + 1
		.voupage.page1.grditem.ColumnCount=tcol
	Endif
	.voupage.page1.grditem.Columns(tcol).AddObject('cmdBom','VouClass.cmdBom')
	.voupage.page1.grditem.Columns(tcol).cmdbom.Picture = apath+Iif(.pcvtype='ST','bmp\finish_item.gif','bmp\raw_material.gif')
	.voupage.page1.grditem.Columns(tcol).cmdbom.Caption=	Iif(Inlist(.pcvtype,'ST','DC'),'Receipt','BOM')
	.voupage.page1.grditem.Columns(tcol).cmdbom.PicturePosition= 2
	.voupage.page1.grditem.Columns(tcol).CurrentControl='cmdBom'
	.voupage.page1.grditem.Columns(tcol).header1.Caption=Iif(!Inlist(.pcvtype,'ST','DC'),'BOM','Receipt')
	.voupage.page1.grditem.Columns(tcol).header1.Alignment= 2
	.voupage.page1.grditem.Columns(tcol).cmdbom.Enabled=.T.
	.voupage.page1.grditem.Columns(tcol).cmdbom.Themes=.F.
	.voupage.page1.grditem.Columns(tcol).cmdbom.Visible=.T.
	.voupage.page1.grditem.Columns(tcol).cmdbom.Height=.voupage.page1.grditem.RowHeight
	.voupage.page1.grditem.Columns(tcol).cmdbom.ToolTipText = Iif(Inlist(.pcvtype,'ST','DC'),'Receipt Details','BOM Details')
	.voupage.page1.grditem.Columns(tcol).Sparse=.F.
Endif

** Birendra : Commented above and Added Below on 28/02/2011 for Costing ---> Start
&& Comment by Ajay Jaiswal on 23/02/2012 : This same code is also used in EXIM module
*!*	IF (.pcvtype="IP")		&& Commented By Shrikant S. on 08/01/2014		for Bug-20752
*!*	If Inlist(.pcvtype,"IP","OP")	&& Added By Shrikant S. on 08/01/2014		for Bug-20752 && Commented By Kishor A. on 30/09/2015 for Bug-27021
If Inlist(.pcvtype,"IP","OP","AR")			&& Added By Kishor A. on 30/09/2015 for Bug-27021
	With .voupage.page1.grditem
		For tcnt = 1 To .ColumnCount Step 1
			colcontrolsource = "upper(alltrim(.column"+Alltrim(Str(tcnt))+".controlsource))"
			ccond            = &colcontrolsource
			If Alltrim(ccond) = 'ITEM_VW.U_FORPICK'
				colwidth = ".column"+Alltrim(Str(tcnt))+".width = 50"
				withcol  = ".column"+Alltrim(Str(tcnt))
				&colwidth
				With &withcol
					If Type("cmdpick") = 'U'
						.AddObject("cmdpick","cmdclass")
					Endif
					.CurrentControl = "cmdpick"
					.cmdpick.Visible = .T.
					.Sparse = .F.
&& Added By Shrikant S. on 08/01/2014		for Bug-20752		&& Start
					If (Main_vw.entry_ty=="OP")
						.cmdpick.Caption="IP Pickup"
					Endif
&& Added By Shrikant S. on 08/01/2014		for Bug-20752		&& End
				Endwith
			Endif
		Endfor
	Endwith
Endif
** Birendra : Commented above and Added Below on 28/02/2011 for Costing ---> End

** Birendra : Added Below on 30/09/2011 for TKT-8452 		---Start
If oglblprdfeat.udchkprod('AutoTran')
	If Inlist(.pcvtype,"PT","ST")
		If !lcode_vw.QC_Module
			With .voupage.page1.grditem
				For tcnt = 1 To .ColumnCount Step 1
					colcontrolsource = "upper(alltrim(.column"+Alltrim(Str(tcnt))+".controlsource))"
					ccond            = &colcontrolsource
					If Alltrim(ccond) = 'ITEM_VW.U_FORPICK'
						colwidth = ".column"+Alltrim(Str(tcnt))+".width = 80"
						withcol  = ".column"+Alltrim(Str(tcnt))
						&colwidth
						With &withcol
							If Type("cmdpick") = 'U'
								.AddObject("cmdpick","cmdclass")
							Endif
							.CurrentControl = "cmdpick"
							.cmdpick.Caption="WorkOrder"
							.cmdpick.Visible = .T.
							.Sparse = .F.
						Endwith
					Endif
				Endfor
			Endwith
		Endif
	Endif
Endif
** Birendra : Added Below on 19/01/2013 for TKT-8436 		---Start
If !oglblprdfeat.udchkprod('AutoTran')
	If Inlist(.pcvtype,"PT","ST")
		With .voupage.page1.grditem
			For tcnt = 1 To .ColumnCount Step 1
				colcontrolsource = "upper(alltrim(.column"+Alltrim(Str(tcnt))+".controlsource))"
				ccond            = &colcontrolsource
				If Alltrim(ccond) = 'ITEM_VW.U_FORPICK'
					withcol  = ".column"+Alltrim(Str(tcnt))+".Visible = .f."
					&withcol
					Exit
				Endif
			Endfor
		Endwith
	Endif
Endif

** Birendra : Added Below on 19/01/2013 for TKT-8436 		---End
** Birendra : Added Below on 30/09/2011 for TKT-8452 		---End


If Inlist(.pcvtype,'IP','OP') And (([vuexc] $ vchkprod) Or ([vuinv] $ vchkprod))
	.cmdbom.Top   = (.voupage.Top - .cmdbom.Height)
Endif

****** Added By Sachin N. S. on 13/02/2012 for TKT-9411 and Bug-660 Batchwise/Serialize Inventory ****** Start
_objform = .voupage.Parent
If Vartype(_objform._batchserialstk)='O'
	._batchserialstk._uetrigvouitemnew()
Endif
****** Added By Sachin N. S. on 13/02/2012 for TKT-9411 and Bug-660 Batchwise/Serialize Inventory ****** End

&& Added By Shrikant S. on 	21/04/2015 for Bug-25878		&& Start
&& Added by Kishor A For bug-27300 on 01/12/2015 Start
Select lcode_vw
If Type("lcode_vw.intrtrn")<>'U'
	If lcode_vw.INTRTRN =.T.
&& Added by Kishor A For bug-27300 on 01/12/2015 Endif
		If Inlist(Main_vw.entry_ty,"II") And Not('vutex' $ vchkprod)
			With .voupage.page1.grditem
				For tcnt = 1 To .ColumnCount Step 1
					colcontrolsource = "upper(alltrim(.column"+Alltrim(Str(tcnt))+".controlsource))"
					ccond            = &colcontrolsource
					If Alltrim(ccond) = 'ITEM_VW.WARE_NM'
						withcol  = ".column"+Alltrim(Str(tcnt))+".enabled=.F."
						&withcol
					Endif
				Endfor
			Endwith
		Endif
	Endif && Added by Kishor A For bug-27300 on 01/12/2015
Endif && Added by Kishor A For bug-27300 on 01/12/2015
&& Added By Shrikant S. on 	21/04/2015 for Bug-25878		&& End



&& Added By Shrikant S. on 04/01/2013 for Bug-7269			&& Start
If Inlist(Main_vw.entry_ty,'D2','D3','D4','D5','C3','C2','C4','C5') And (_Screen.ActiveForm.addmode Or _Screen.ActiveForm.editmode)
	With _Screen.ActiveForm
		tot_grd_col=.voupage.page1.grditem.ColumnCount
		For i = 1 To tot_grd_col
			Do Case
			Case .voupage.page1.grditem.Columns(i).header1.Caption='Ass. Value'
				.voupage.page1.grditem.Columns(i).Visible=.F.

			Case .voupage.page1.grditem.Columns(i).header1.Caption='Rate'
				.voupage.page1.grditem.Columns(i).Visible=.F.

			Case .voupage.page1.grditem.Columns(i).header1.Caption='Quantity'
				.voupage.page1.grditem.Columns(i).Visible=Iif(Inlist(Main_vw.entry_ty,'D5','D4','C4','C5'),.T.,.F.)

			Case .voupage.page1.grditem.Columns(i).header1.Caption='Sale Bill No.'
				.voupage.page1.grditem.Columns(i).Width=70

			Case .voupage.page1.grditem.Columns(i).header1.Caption='Item Name'
				.voupage.page1.grditem.Columns(i).Width=120
				.voupage.page1.grditem.Columns(i).Visible=.F.

			Case .voupage.page1.grditem.Columns(i).header1.Caption=Iif(Inlist(Main_vw.entry_ty,'C3'),'Payment Made Date','Payment Recd. Date')
				.voupage.page1.grditem.Columns(i).header1.Caption=Iif(Inlist(Main_vw.entry_ty,'C3'),'Payment Made Date','Payment Recd. Date')
				.voupage.page1.grditem.Columns(i).Visible=Iif(Inlist(Main_vw.entry_ty,'D3','C3'),.T.,.F.)

			Case .voupage.page1.grditem.Columns(i).header1.Caption=Iif(Inlist(Main_vw.entry_ty,'C3'),'Made Amount','Recd. Amount')
				.voupage.page1.grditem.Columns(i).header1.Caption=Iif(Inlist(Main_vw.entry_ty,'C3'),'Made Amount','Recd. Amount')
				.voupage.page1.grditem.Columns(i).Visible=Iif(Inlist(Main_vw.entry_ty,'D3','C3'),.T.,.F.)

			Case .voupage.page1.grditem.Columns(i).header1.Caption='Late Days'
				.voupage.page1.grditem.Columns(i).Visible=Iif(Inlist(Main_vw.entry_ty,'D3','C3'),.T.,.F.)

			Case .voupage.page1.grditem.Columns(i).header1.Caption='Interest %'
				.voupage.page1.grditem.Columns(i).Visible=Iif(Inlist(Main_vw.entry_ty,'D3','C3'),.T.,.F.)

			Case .voupage.page1.grditem.Columns(i).header1.Caption='Interest Amount'
				.voupage.page1.grditem.Columns(i).Visible=Iif(Inlist(Main_vw.entry_ty,'D3','C3'),.T.,.F.)
				.voupage.page1.grditem.Refresh
			Endcase
		Endfor
	Endwith
Endif
&& Added By Shrikant S. on 04/01/2013 for Bug-7269			&& End


&& Added By Shrikant S. on 26/06/2012 for Bug-4744		&& Start
If Vartype(oglblprdfeat)='O'
	If oglblprdfeat.udchkprod('openord')
		If Type('lcode_vw.allowzeroqty')!='U'
			If (.pcvtype='OO') And lcode_vw.allowzeroqty=.T.
				With .voupage.page1.grditem
					For tcnt = 1 To .ColumnCount Step 1
						colcontrolsource = "upper(alltrim(.column"+Alltrim(Str(tcnt))+".controlsource))"
						ccond            = &colcontrolsource
						If Alltrim(ccond) = 'ITEM_VW.QTY'
							colVisible= ".column"+Alltrim(Str(tcnt))+".VISIBLE = .F."
							&colVisible
						Endif
					Endfor
				Endwith
			Endif
		Endif
	Endif
Endif
&& Added By Shrikant S. on 26/06/2012 for Bug-4744		&& End

&& Added by Shrikant S. on 28/06/2014 for Bug-23280		&& Start
If Vartype(oglblindfeat)='O'
	If oglblindfeat.udchkind('pharmaind')
		If Inlist(.pcvtype,"AR")
			tot_grd_col=.voupage.page1.grditem.ColumnCount
			For i = 1 To tot_grd_col
				Do Case
				Case .voupage.page1.grditem.Columns(i).header1.Caption ='Quantity'
					lnqtycol=i
				Case .voupage.page1.grditem.Columns(i).header1.Caption ='Rate'
					lnratecol=i
				Case .voupage.page1.grditem.Columns(i).header1.Caption ='Conv. Qty'
					lnqty1col=i
				Case .voupage.page1.grditem.Columns(i).header1.Caption ='Conv. Unit'
					lnunitcol=i
				Case .voupage.page1.grditem.Columns(i).header1.Caption ='Conv. Rate'
					lncrate=i
				Case .voupage.page1.grditem.Columns(i).header1.Caption ='Amount'
					lnamount=i
				Endcase

			Endfor
			lntmpOrder=.voupage.page1.grditem.Columns(lnqtycol).ColumnOrder
			.voupage.page1.grditem.Columns(lnqtycol).ColumnOrder=.voupage.page1.grditem.Columns(lnqty1col).ColumnOrder
			.voupage.page1.grditem.Columns(lnqty1col).ColumnOrder=lntmpOrder

			lntmpOrder=.voupage.page1.grditem.Columns(lnratecol).ColumnOrder
			.voupage.page1.grditem.Columns(lnratecol).ColumnOrder=.voupage.page1.grditem.Columns(lncrate).ColumnOrder
			.voupage.page1.grditem.Columns(lncrate).ColumnOrder=lntmpOrder

		Endif

		If Inlist(.pcvtype,"IP")
			tot_grd_col=.voupage.page1.grditem.ColumnCount
			For i = 1 To tot_grd_col
				Do Case
				Case .voupage.page1.grditem.Columns(i).header1.Caption ='Pickup'
					.voupage.page1.grditem.Columns(i).cmdpick.Caption ='Select'
					Exit
				Endcase
			Endfor
		Endif
		If Inlist(.pcvtype,'ST') And (([vuexc] $ vchkprod))
			tcol1=.voupage.page1.grditem.ColumnCount
			tcol2 = 1
			tcol  = 0
			For tcol2 = 1 To tcol1
				If Alltrim(Upper(.voupage.page1.grditem.Columns(tcol2).header1.Caption)) == 'RECEIPT'
					tcol = tcol2
					Exit
				Endif
			Endfor
			If tcol >0
				.voupage.page1.grditem.Columns(tcol).ColumnOrder=9
			Endif
		Endif

		If Inlist(.pcvtype,'SS')
			With .voupage.page1.grditem
				For tcnt = 1 To .ColumnCount Step 1
					colcontrolsource = "upper(alltrim(.column"+Alltrim(Str(tcnt))+".controlsource))"
					ccond            = &colcontrolsource
					If Alltrim(ccond) = 'ITEM_VW.U_FORPICK'
						colwidth = ".column"+Alltrim(Str(tcnt))+".width = 80"
						withcol  = ".column"+Alltrim(Str(tcnt))
						&colwidth
						With &withcol
							If Type("cmdbtn") = 'U'
								.AddObject("cmdbtn","cmdbtncls")
							Endif
							.CurrentControl = "cmdbtn"
							.cmdbtn.Caption="Click Here"
							.cmdbtn.Visible = .T.
							.Sparse = .F.
						Endwith
					Endif
				Endfor
			Endwith

		Endif
	Endif
Endif
&& Added by Shrikant S. on 28/06/2014 for Bug-23280		&& End

&& Added by Shrikant S. on 06/10/2016 for GST			&& Start
If Type('_Screen.ActiveForm.pcvtype')<>'U'
	If Inlist(_Screen.ActiveForm.pcvtype,'E1','S1','IB','J6','BP','BR','CP','CR','C6','D6','RV') And (_Screen.ActiveForm.addmode Or _Screen.ActiveForm.editmode)		&& Commented by Shrikant S. on 06/02/2017 for GST
*!*	If Inlist(Main_vw.Entry_ty,'E1','S1','IB','J6','BP','BR','CP','CR','GC','GD','C6','D6') And (_Screen.ActiveForm.addmode Or _Screen.ActiveForm.editmode)				&& Added by Shrikant S. on 06/02/2017 for GST
		With _Screen.ActiveForm
			tot_grd_col=.voupage.page1.grditem.ColumnCount
			For i = 1 To tot_grd_col
				Do Case
				Case .voupage.page1.grditem.Columns(i).header1.Caption='Quantity'
					.voupage.page1.grditem.Columns(i).Visible=.F.
*!*					IF Inlist(Main_vw.Entry_ty,'GC','GD','C6','D6')
*!*						.voupage.page1.grditem.Columns(i).Visible=.T.
*!*						.voupage.page1.grditem.Columns(i).Enabled=.F.
*!*					endif
				Case .voupage.page1.grditem.Columns(i).header1.Caption='Rate'
					.voupage.page1.grditem.Columns(i).Visible=.F.
*!*					IF Inlist(Main_vw.Entry_ty,'GC','GD','C6','D6')
*!*						.voupage.page1.grditem.Columns(i).Visible=.T.
*!*					endif

				Case Upper(.voupage.page1.grditem.Columns(i).ControlSource)='ITEM_VW.U_ASSEAMT'
*!*					If Inlist(_Screen.ActiveForm.pcvtype,"IB","J6","E1","S1","C6","D6")		&& Commented by Shrikant S. on 06/02/2017 for GST
					If Inlist(_Screen.ActiveForm.pcvtype,"IB","J6")		&& Added by Shrikant S. on 06/02/2017 for GST
*!*					If Inlist(_Screen.ActiveForm.pcvtype,"IB","J6","E1","S1")					&& Added by Shrikant S. on 06/02/2017 for GST
						.voupage.page1.grditem.Columns(i).Visible=.F.
					Endif
					If Inlist(_Screen.ActiveForm.pcvtype,"C6","D6")
						.voupage.page1.grditem.Columns(i).header1.Caption="Diff. Taxable Value"
						.voupage.page1.grditem.Columns(i).ReadOnly=.T.
						.voupage.page1.grditem.Columns(i).Width=110
					ENDIF
					If Inlist(_Screen.ActiveForm.pcvtype,"S1","E1")
						.voupage.page1.grditem.Columns(i).ReadOnly=.T.
					Endif

				Case Upper(.voupage.page1.grditem.Columns(i).ControlSource)="ITEM_VW.STAXAMT"
					.voupage.page1.grditem.Columns(i).ReadOnly=.T.
				Endcase
			Endfor
		Endwith
	Endif
Endif
&& Added by Shrikant S. on 06/10/2016 for GST			&& End

&& Added by Shrikant S. on 23/02/2017 for GST		&& Start
If Type('_Screen.ActiveForm.pcvtype')<>'U'
	If Inlist(_Screen.ActiveForm.pcvtype,'GD','GC')
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


** Birendra : Commented above and Added Below on 28/02/2011 for Costing 		---Start
&& Comment by Ajay Jaiswal on 23/02/2012 : This same code is also used in EXIM module
*!*	DEFINE CLASS cmdclass AS COMMANDBUTTON
*!*		CAPTION = "Pickup"
*!*		FONTSIZE = 8
*!*		FONTBOLD = .T.
*!*		FORECOLOR = RGB(255,0,0)

*!*		PROCEDURE CLICK()
*!*		DO FORM uefrm_itempickup WITH THISFORM.addmode,THISFORM.editmode,THISFORM.DATASESSIONID
*!*		RETURN
*!*	ENDDEFINE

*changes by EBS team on 07/03/14 for Bug-21466,21467,21468 start
* Changes done as per --> CR_KOEL_0005A_Form_To_Record_Pre_Shipment_Info
* Date : 08/11/2012
* Changes done by EBS Product Team
_mexim = oglblprdfeat.udchkprod('exim')
If _mexim
	If Inlist(.pcvtype,'SI')
		With .voupage.page1.grditem
			For tcnt = 1 To .ColumnCount Step 1
				colcontrolsource = "upper(alltrim(.column"+Alltrim(Str(tcnt))+".controlsource))"
				ccond            = &colcontrolsource
				If Alltrim(ccond) = 'ITEM_VW.U_FORINFO'
					colwidth = ".column"+Alltrim(Str(tcnt))+".width = 120"
					withcol  = ".column"+Alltrim(Str(tcnt))
					&colwidth
					With &withcol
						If Type("cmdPreShipInfo") = 'U'
							.AddObject("cmdPreShipInfo","cmdButton")
						Endif
						.CurrentControl = "cmdPreShipInfo"
						.cmdPreShipInfo.Visible = .T.
						.Sparse = .F.
						.header1.Caption = ""
						.header1.Alignment = 2
						.cmdPreShipInfo.Picture = apath+'bmp\additional_info.gif'
						.cmdPreShipInfo.DisabledPicture = apath+'bmp\additional_info_off.gif'
						.cmdPreShipInfo.Caption=	'  Pre Shipment Info.'
						.cmdPreShipInfo.ToolTipText = '  Pre Shipment Info.'
						.cmdPreShipInfo.FontSize= 8
						.cmdPreShipInfo.PicturePosition= 2
					Endwith
				Endif
			Endfor
		Endwith
	Endif
Endif
* End --> CR_KOEL_0005A_Form_To_Record_Pre_Shipment_Info


***** Added by Sachin N. S. on 01/04/2016 for Bug-27503 -- Start
_curvouobj = _Screen.ActiveForm
If Type('_curvouobj._udClsPointOfSale')='O'
	_curvouobj._udClsPointOfSale._uetrigvouitemnew()
Endif
***** Added by Sachin N. S. on 01/04/2016 for Bug-27503 -- End


* Changes done as per --> CR_KOEL_0005A_Form_To_Record_Pre_Shipment_Info
* Date : 08/11/2012
* Changes done by EBS Product Team
Define Class cmdButton As CommandButton
	Procedure Click
	curObj = _Screen.ActiveForm
	If !Used("Tbl_PreShipment_Vw")
		lcStr = "Select CAST(0 as int) as serial,PreShipInfo.* From [PreShipInfo] ;
					 Where Entry_TY = ?Item_vw.Entry_Ty And Tran_cd = ?Item_vw.Tran_Cd Order by Itserial"

		nretval = curObj.sqlconobj.dataconn("EXE",company.dbname,lcStr,"Tbl_PreShipment_Vw","curObj.nhandle",curObj.DataSessionId)
		If nretval < 0
			Return .F.
		Endif
		nretval=curObj.sqlconobj.sqlconnclose("curObj.nHandle")
	Endif

	Select Tbl_PreShipment_Vw
	Go Top
	Do Form frm_PreShipmentInfo With curObj.addmode,curObj.editmode,curObj.DataSessionId
	Select Item_vw
	Endproc
Enddefine

* End --> CR_KOEL_0005A_Form_To_Record_Pre_Shipment_Info
*changes by EBS team on 07/03/14 for Bug-21466,21467,21468 end

Define Class cmdclass As CommandButton
	Caption = "Pickup"
	FontSize = 8
	FontBold = .T.
	ForeColor = Rgb(0,0,0)

	Procedure Click()
&& Added By Pankaj B. on 14-03-2015 for Bug-25542 Start
	curObj = _Screen.ActiveForm

	lcStr = "Select type From it_mast where it_code= ?Item_vw.it_code"
	nretval = curObj.sqlconobj.dataconn("EXE",company.dbname,lcStr,"ipmast_vw","curObj.nhandle",curObj.DataSessionId)
	If nretval < 0
		Return .F.
	Endif
	nretval=curObj.sqlconobj.sqlconnclose("curObj.nHandle")


*!*		IF this.Caption="Pickup" And !Inlist(UPPER(ipmast_vw.Type),"FINISHED","SEMI FINISHED") && Commented Kishor A. for bug-27021 on 30/09/2015
	If This.Caption="Pickup" And !Inlist(Upper(ipmast_vw.Type),"FINISHED","SEMI FINISHED") And !Main_vw.entry_ty="AR" && Added by Kishor A. for bug-27021
		Do Form uefrm_itempickup With Thisform.addmode,Thisform.editmode,Thisform.DataSessionId
	Endif

&& Added by Kishor A. for bug-27021 on 30/09/2015 Start..
	If Inlist(Main_vw.entry_ty,'AR')
		Do Form uefrm_itpopickup With Thisform.addmode,Thisform.editmode,Thisform.DataSessionId
		Thisform.ItemGrdBefCalc(4)
	Endif
&& Added by Kishor A. for bug-27021 on 30/09/2015 End..

*!*		IF this.Caption="Pickup" And Inlist(UPPER(ipmast_vw.Type),"FINISHED","SEMI FINISHED") && Commented Kishor A. for bug-27021 on 30/09/2015
	If This.Caption="Pickup" And Inlist(Upper(ipmast_vw.Type),"FINISHED","SEMI FINISHED") And !Main_vw.entry_ty="AR" && Added by Kishor A. for bug-27021
*!*			Do Form uefrm_OpStItemAllocation With Thisform.DataSessionId,Thisform.addmode,Thisform.editmode,Thisform			&& Commented by Shrikant S. on 02/02/2017 for GST
		Do Form uefrm_itempickup With Thisform.addmode,Thisform.editmode,Thisform.DataSessionId						&& Added by Shrikant S. on 02/02/2017 for GST
	Endif
&& Added By Pankaj B. on 14-03-2015 for Bug-25542 End

&& Comented By Pankaj B. on 14-03-2015 for Bug-25542 Start
*!*		IF this.Caption="Pickup"
*!*		Do Form uefrm_itempickup With Thisform.addmode,Thisform.editmode,Thisform.DataSessionId
*!*		ENDIF
&& Comented By Pankaj B. on 14-03-2015 for Bug-25542 End

** Birendra : Added Below on 14/14/2011 for TKT-8452 		---Start
	If This.Caption="WorkOrder" And oglblprdfeat.udchkprod('AutoTran')
		formObj=_Screen.ActiveForm
		_malias 	= Alias()
		If Inlist(Main_vw.entry_ty,'PT') And (([vuexc] $ vchkprod) Or ([vuinv] $ vchkprod))
			Do Form uefrm_bomdetailsIP_AUTO With formObj.DataSessionId,formObj.addmode,formObj.editmode,formObj
		Endif
		If Inlist(Main_vw.entry_ty,'ST') And (([vuexc] $ vchkprod) Or ([vuinv] $ vchkprod))
			Do Form uefrm_bomdetailsop_AUTO With formObj.DataSessionId,formObj.addmode,formObj.editmode,formObj
		Endif
		If !Empty(_malias)
			Select &_malias
		Endif
	Endif
** Birendra : Added Below on 14/14/2011 for TKT-8452 		---End
&& Added by Shrikant S. on 28/06/2014 for Bug-23280		&& Start && Comented By Pankaj B. on 14-03-2015 for Bug-25542 Start
*!*		IF this.Caption="Select"
*!*			Do Form uefrm_itempickup_pharmaind With Thisform.addmode,Thisform.editmode,Thisform.DataSessionId
*!*		ENDIF
&& Added by Shrikant S. on 28/06/2014 for Bug-23280		&& End && Comented By Pankaj B. on 14-03-2015 for Bug-25542 End

&& Added By Pankaj B. on 14-03-2015 for Bug-25542 Start
	If This.Caption="Select" And !Inlist(Upper(ipmast_vw.Type),"FINISHED","SEMI FINISHED")
		Do Form uefrm_itempickup_pharmaind With Thisform.addmode,Thisform.editmode,Thisform.DataSessionId
	Endif
	If This.Caption="Select" And Inlist(Upper(ipmast_vw.Type),"FINISHED","SEMI FINISHED")
		Do Form uefrm_OpStItemAllocation_pharmaind With Thisform.DataSessionId,Thisform.addmode,Thisform.editmode,Thisform
	Endif
&& Added By Pankaj B. on 14-03-2015 for Bug-25542 End

&& Added By Shrikant S. on 08/01/2014		for Bug-20752		&& Start
	If This.Caption=="IP Pickup"
		Do Form UEFRM_IP_ALLOCATION With _Screen.ActiveForm.DataSessionId,_Screen.ActiveForm.addmode,_Screen.ActiveForm.editmode
	Endif
&& Added By Shrikant S. on 08/01/2014		for Bug-20752		&& End

	Return
Enddefine

** Added by Shrikant S. on 10/08/2010 for EOU 		---End


&& Added by Shrikant S. on 28/06/2014 for Bug-23280		&& Start
Define Class cmdbtncls As CommandButton
	Procedure Click
	_malias 	= Alias()
	_mRecNo	= Recno()
	Do Form uefrm_batchselect With _Screen.ActiveForm.addmode,_Screen.ActiveForm.editmode,_Screen.ActiveForm.DataSessionId,Thisform
	If !Empty(_malias)
		Select &_malias
	Endif
	If Betw(_mRecNo,1,Reccount())
		Go _mRecNo
	Endif
	Endproc
Enddefine
&& Added by Shrikant S. on 28/06/2014 for Bug-23280		&& End
