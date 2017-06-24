&&--->TKT-8320 GTA
If Inlist(.pcVtype,'IF','OF') &&And (([vuexc] $ vchkprod) Or ([vuinv] $ vchkprod))
	Replace qty With 1 In item_vw
Endif
&&<---TKT-8320 GTA

&& Added by Shrikant S. on 21/06/2012 for Bug-4744		&& Start
If Vartype(oGlblPrdFeat)='O'
	If oGlblPrdFeat.UdChkProd('openord')
		If Type('lcode_vw.allowzeroqty')!='U'
			If Inlist(.pcVtype,'OO') And Lcode_vw.allowzeroqty=.T.
				Replace qty With 1 In item_vw
			Endif
		Endif
	Endif
Endif
&& Added by Shrikant S. on 21/06/2012 for Bug-4744		&& End

*Birendra : TKT-9363, on 19/09/2011 for Auto Debit/Credit Note :Start:
If Inlist(.pcVtype,'D2','D3','D4','D5','C2','C3','C4','C5') &&,'CI')
	_curfrmobj = _Screen.ActiveForm

	nRet = _curfrmobj.sqlconobj.DataConn([EXE],Company.DbName,"select it_code from it_mast where it_name='Interest'",[tmp_itcode],;
		"_curfrmobj.nHandle",_curfrmobj.DataSessionId,.F.)
	If nRet <= 0
		Return .F.
	Endif

	Select item_vw
	Replace item_vw.Item With 'Interest' In item_vw
	Replace item_vw.qty With 1 In item_vw
	Replace item_vw.it_code With tmp_itcode.it_code In item_vw
	If Used('tmp_itcode')
		Use In tmp_itcode
	Endif
	Select item_vw
Endif
*Birendra : TKT-9363, on 19/09/2011 for Auto Debit/Credit Note :End:

&& Added by Shrikant S. on 28/06/2014 	for Bug-23280		&& Start
If Vartype(oglblindfeat)='O'
	If oglblindfeat.udchkind('pharmaind')
		If Type('.pcvtype')='C'
			If Inlist(.pcVtype,"WK")
				_malias=Alias()
				_mrecno=Iif(!Eof(),Recno(),0)

				Select item_vw
				itemRec=Iif(!Eof(),Recno(),0)
				Count For !Deleted() To itemcnt

				If itemcnt>1
					Select item_vw
					If itemRec>0
						Go itemRec
						Messagebox("Only one Finished/Semi Finished item is allowed.",0,vumess)
						Delete In item_vw
					Endif
					If !Empty(_malias)
						Select &_malias
						Go _mrecno
					Endif
					Return .F.
				Endif

			Endif
		Endif
	Endif
Endif
&& Added by Shrikant S. on 28/06/2014 	for Bug-23280		&& End


&& Added by Shrikant S. on 21/04/2015 for bug-25878		&& Start
If Not('vutex' $ vchkprod)
*!*		If Type('lmc_vw.fware')<>'U' Or Type('main_vw.fware')<>'U'		&& Commented  by Shrikant S. on 24/04/2017 for GST
	If Type('lmc_vw.fware')<>'U' And Type('main_vw.fware')<>'U'			&& Added  by Shrikant S. on 24/04/2017 for GST
		Replace item_vw.ware_nm With lmc_vw.fware In item_vw
	Endif
Endif

If Inlist(main_vw.Entry_ty,'II') And Not('vutex' $ vchkprod)
*!*		Replace item_vw.ware_nm With lmc_vw.fware In item_vw
	Select item_vw
	mrecno=Iif(!Eof(),Recno(),0)

	Select Distinct item_vw.tWare From item_vw With (Buffering=.T.) Where !Empty(item_vw.tWare) Into Cursor curware

	Select curware
	If Reccount('curware')>0
		Locate
		If mrecno>0
			Select item_vw
		Endif
		Replace item_vw.tWare With curware.tWare In item_vw

		With .Voupage.Page1.Grditem
			For tcnt = 1 To .ColumnCount Step 1
				colcontrolsource = "upper(alltrim(.column"+Alltrim(Str(tcnt))+".controlsource))"
				cCond            = &colcontrolsource
				If Alltrim(cCond) = 'ITEM_VW.TWARE'
					withcol  = ".column"+Alltrim(Str(tcnt))+".enabled=.F."
					&withcol
				Endif
			Endfor
		Endwith
	Else
		With .Voupage.Page1.Grditem
			For tcnt = 1 To .ColumnCount Step 1
				colcontrolsource = "upper(alltrim(.column"+Alltrim(Str(tcnt))+".controlsource))"
				cCond            = &colcontrolsource
				If Alltrim(cCond) = 'ITEM_VW.TWARE'
					withcol  = ".column"+Alltrim(Str(tcnt))+".enabled=.T."
					&withcol
				Endif
			Endfor
		Endwith
	Endif
	If Used('curware')
		Use In curware
	Endif
Endif
&& Added by Shrikant S. on 21/04/2015 for bug-25878		&& End


&& Added by Shrikant S. on 04/10/2016 for GST		&& Start
If Type('ITEM_VW.IGST_AMT')<>'U'
	Do Case
	Case Inlist(Alltrim(.taxapplarea),"INTRASTATE") Or Empty(.taxapplarea)
		If Type('item_vw.IGST_AMT')<>'U'
			Replace IGST_AMT With 0,IGST_PER With 0 In item_vw
		Endif
	Case Inlist(Alltrim(.taxapplarea),"INTERSTATE","OUT OF COUNTRY")
		If Type('item_vw.CGST_AMT')<>'U'
			Replace CGST_AMT With 0,CGST_PER With 0
		Endif
		If Type('item_vw.SGST_AMT')<>'U'
			Replace SGST_AMT With 0,SGST_PER With 0 In item_vw
		Endif
	Endcase
Endif
*!*	IF INLIST(_curfrmobj.pcvtype,'E1','S1','IB','J6','BP','BR')
If Inlist(.pcVtype,'E1','S1','IB','J6','J7','BP','BR','CP','CR','C6','D6')
	Select item_vw
	Replace item_vw.qty With 1,item_vw.rate With 0 In item_vw
*	,Sr_Sr With Subs(Item_vw.Sr_Sr,1,2)+'TTT' In item_vw
Endif
&& Added by Shrikant S. on 04/10/2016 for GST		&& End

