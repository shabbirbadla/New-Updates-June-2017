LOCAL tcol
tcol=0
IF INLIST(.pcVtype,'WK','ST','DC') AND (([vuexc] $ vchkprod) Or ([vuinv] $ vchkprod))
	tcol1=.Voupage.Page1.Grditem.ColumnCount
	tcol2 = 1
	tcol  = 0
	FOR tcol2 = 1 TO tcol1
		IF UPPER(.Voupage.Page1.Grditem.Columns(tcol2).controlsource) = 'ITEM_VW.U_BOMDET'
			tcol = tcol2
			exit
		endif
	ENDFOR
	IF tcol = 0
		tcol = tcol1 + 1
		.Voupage.Page1.Grditem.ColumnCount=tcol
	Endif
	.Voupage.Page1.Grditem.Columns(tcol).AddObject('cmdBom','VouClass.cmdBom')	
	.Voupage.Page1.Grditem.Columns(tcol).cmdBom.Picture = apath+IIF(.pcVtype='ST','bmp\finish_item.gif','bmp\raw_material.gif')
	.Voupage.Page1.Grditem.Columns(tcol).cmdBom.caption=	IIF(INLIST(.pcVtype,'ST','DC'),'Receipt','BOM')
	.Voupage.Page1.Grditem.Columns(tcol).cmdBom.PicturePosition= 2 
	.Voupage.Page1.Grditem.columns(tcol).currentcontrol='cmdBom'
	.Voupage.Page1.Grditem.columns(tcol).header1.Caption=IIF(!INLIST(.pcVtype,'ST','DC'),'BOM','Receipt')
	.Voupage.Page1.Grditem.columns(tcol).header1.Alignment= 2
	.Voupage.Page1.Grditem.columns(tcol).cmdBom.Enabled=.T.
	.Voupage.Page1.Grditem.columns(tcol).cmdBom.themes=.F.
	.Voupage.Page1.Grditem.columns(tcol).cmdBom.Visible=.T.
	.Voupage.Page1.Grditem.columns(tcol).cmdBom.Height=.Voupage.Page1.Grditem.rowheight
	.Voupage.Page1.Grditem.columns(tcol).cmdBom.ToolTipText = IIF(INLIST(.pcVtype,'ST','DC'),'Receipt Details','BOM Details')
	.Voupage.Page1.Grditem.columns(tcol).sparse=.F.
ENDIF

If (.PCVTYPE="IP") OR (.PCVTYPE="II")
		WITH .voupage.page1.grditem
			for tcnt = 1 to .columncount step 1
				colcontrolsource = "upper(alltrim(.column"+alltrim(str(tcnt))+".controlsource))"
				cCond            = &colcontrolsource
				if alltrim(ccond) = 'ITEM_VW.U_FORPICK'
					headcapt = ".column"+alltrim(str(tcnt))+".header1.caption = ''"
					colwidth = ".column"+alltrim(str(tcnt))+".width = 50"
					withcol  = ".column"+alltrim(str(tcnt))
					&headcapt
					&colwidth
					with &withcol
						IF TYPE("cmdpick") = 'U'
							.addobject("cmdpick","cmdclass")
						Endif	
						.currentcontrol = "cmdpick"
						.cmdpick.visible = .t.
						.sparse = .f.
					ENDWITH
				endif
			endfor
		ENDWITH
ENDIF

If Inlist(.pcVtype,'IP','OP') And (([vuexc] $ vchkprod) Or ([vuinv] $ vchkprod))
	.cmdBom.top   = (.voupage.top - .cmdBom.height)
Endif


			
	
			

define class cmdclass as commandbutton
*!*	PARAMETERS .addmode,.EditMode,.DataSessionId
	caption = "Pickup"
	fontsize = 8
	fontbold = .t.
	forecolor = rgb(255,0,0)

	procedure click()
		Do Form uefrm_itempickup WITH Thisform.addmode,Thisform.editmode,Thisform.DATASESSIONID
	return
enddefine




	

