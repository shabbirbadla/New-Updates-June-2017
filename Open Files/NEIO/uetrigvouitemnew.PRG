Local tcol
tcol=0

&& Added by Shrikant S. on 01/06/2010 Start
Select DcMast_vw
Replace All Code With "A" For Inlist(fld_nm,'CCDAMT','HCDAMT') And Entry_ty='P1'
&& Added by Shrikant S. on 01/06/2010 End

If Inlist(.pcVtype,'WK','ST','DC') And (([vuexc] $ vchkprod)) &&  Or ([vuinv] $ vchkprod) Comment By Hetal Dt 240310
	tcol1=.Voupage.Page1.Grditem.ColumnCount
	tcol2 = 1
	tcol  = 0
	For tcol2 = 1 To tcol1
		If Upper(.Voupage.Page1.Grditem.Columns(tcol2).ControlSource) = 'ITEM_VW.U_BOMDET'
			tcol = tcol2
			Exit
		Endif
	Endfor
	If tcol = 0
		tcol = tcol1 + 1
		.Voupage.Page1.Grditem.ColumnCount=tcol
	Endif
	.Voupage.Page1.Grditem.Columns(tcol).AddObject('cmdBom','VouClass.cmdBom')
	.Voupage.Page1.Grditem.Columns(tcol).cmdBom.Picture = apath+Iif(.pcVtype='ST','bmp\finish_item.gif','bmp\raw_material.gif')
	.Voupage.Page1.Grditem.Columns(tcol).cmdBom.Caption=	Iif(Inlist(.pcVtype,'ST','DC'),'Receipt','BOM')
	.Voupage.Page1.Grditem.Columns(tcol).cmdBom.PicturePosition= 2
	.Voupage.Page1.Grditem.Columns(tcol).CurrentControl='cmdBom'
	.Voupage.Page1.Grditem.Columns(tcol).header1.Caption=Iif(!Inlist(.pcVtype,'ST','DC'),'BOM','Receipt')
	.Voupage.Page1.Grditem.Columns(tcol).header1.Alignment= 2
	.Voupage.Page1.Grditem.Columns(tcol).cmdBom.Enabled=.T.
	.Voupage.Page1.Grditem.Columns(tcol).cmdBom.Themes=.F.
	.Voupage.Page1.Grditem.Columns(tcol).cmdBom.Visible=.T.
	.Voupage.Page1.Grditem.Columns(tcol).cmdBom.Height=.Voupage.Page1.Grditem.RowHeight
	.Voupage.Page1.Grditem.Columns(tcol).cmdBom.ToolTipText = Iif(Inlist(.pcVtype,'ST','DC'),'Receipt Details','BOM Details')
	.Voupage.Page1.Grditem.Columns(tcol).Sparse=.F.
Endif

** Birendra : Commented above and Added Below on 28/02/2011 for Costing 		---Start
If (.pcVtype="IP")
	With .Voupage.Page1.Grditem
		For tcnt = 1 To .ColumnCount Step 1
			colcontrolsource = "upper(alltrim(.column"+Alltrim(Str(tcnt))+".controlsource))"
			cCond            = &colcontrolsource
			If Alltrim(cCond) = 'ITEM_VW.U_FORPICK'
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
				Endwith
			Endif
		Endfor
	Endwith
Endif
** Birendra : Commented above and Added Below on 28/02/2011 for Costing 		---End

If Inlist(.pcVtype,'IP','OP') And (([vuexc] $ vchkprod) Or ([vuinv] $ vchkprod))
	.cmdBom.Top   = (.Voupage.Top - .cmdBom.Height)
Endif

****** Added By Sachin N. S. on 04/07/2011 for Batchwise/Serialize Inventory ****** Start
_objform = .Voupage.Parent
If Vartype(_objform._BatchSerialStk)='O'
	._BatchSerialStk._ueTrigVouItemNew()
Endif
****** Added By Sachin N. S. on 04/07/2011 for Batchwise/Serialize Inventory ****** End


** Birendra : Commented above and Added Below on 28/02/2011 for Costing 		---Start
Define Class cmdclass As CommandButton
	Caption = "Pickup"
	FontSize = 8
	FontBold = .T.
	ForeColor = Rgb(255,0,0)

	Procedure Click()
	Do Form uefrm_itempickup With Thisform.addmode,Thisform.editmode,Thisform.DataSessionId
	Return
Enddefine
** Added by Shrikant S. on 10/08/2010 for EOU 		---End




