&&-->Ipop(Rup)
*-->Checking Allocated entry
_malias 	= Alias()
_mrecno	= Recno()
_curvouobj = _Screen.ActiveForm
&&&Added by satish pal dt.25/07/2012 for bug-5450--start
If Type("_SCREEN.ACTIVEFORM.mainAlias") <> "C"
&&	RETURN 0 &&commnetd by Shrikant S. for auto updater 10.4.16
	Return	&&ADDED BY Shrikant S. for auto updater 10.4.16
Endif
&&&Added by satish pal dt.25/07/2012 for bug-5450--end
&&vasant061009
If Type('_curvouobj.mainalias') = 'C'
	If Upper(_curvouobj.mainalias) <> 'MAIN_VW'
		Return
	Endif
Endif
&&vasant061009

*Set DataSession To _curvouobj.DataSessionId	&&vasant071009
If Type('_curvouobj.PcvType') = 'C'
	If _Screen.ActiveForm.voupage.page1.grditem.column2.text1.Value<>Val(_Screen.ActiveForm.voupage.page1.grditem.column2.text1.Tag)
		If  (([vuexc] $ vchkprod) Or ([vuinv] $ vchkprod)) And Type('_curvouobj.PCVTYPE')='C' &&Check Existing Records

			If _curvouobj.pcvtype='WK' And (_Screen.ActiveForm.voupage.page1.grditem.column2.text1.Value < Val(_Screen.ActiveForm.voupage.page1.grditem.column2.text1.Tag))
				etsql_str  = "select top 1 entry_ty from projectitref Where aTran_cd = ?Main_vw.Tran_cd And aEntry_ty = ?Main_vw.Entry_ty and aitserial=?item_vw.itserial"
				etsql_str1 = " union select top 1 aentry_ty from projectitref Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty and itserial=?item_vw.itserial"
				etsql_con1 = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str+etsql_str1,[_chkbom],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
				If Used('_chkbom')
					If Reccount()>0
						Select _chkbom
						=Messagebox("Entry Passed Against /"+_chkbom.entry_ty+". Quantity Could not be Changed",16,vumess)
						_Screen.ActiveForm.voupage.page1.grditem.column2.text1.Value=_Screen.ActiveForm.voupage.page1.grditem.column2.text1.Tag
						Use In _chkbom
						Return .F.
					Endif
					Use In _chkbom
				Endif
****************************************added by satish pal for bug-3335 dt.17/11/2012--start----
				etsql_str= "Select top 1 trm_Qty From item Where Entry_ty=?Main_vw.Entry_ty and Tran_cd=?Main_vw.Tran_cd and itserial=?item_vw.itserial"
				etsql_con1 = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[_chkbomitem],"_curvouobj.nHandle",_curvouobj.DataSessionId,.F.)
				If Used('_chkbomitem')
					If (_curvouobj.voupage.page1.grditem.column2.text1.Value) < _chkbomitem.trm_qty
						Select _chkbomitem
						=Messagebox("Entry Passed Against "+_curvouobj.Caption+". Quantity Could not be Changed",16,vumess)
						_curvouobj.voupage.page1.grditem.column2.text1.Value=_curvouobj.voupage.page1.grditem.column2.text1.Tag
						Use In _chkbomitem
						Return .F.
					Endif
					Use In _chkbomitem
				Endif
****************************************added by satish pal for bug-3335 dt.17/11/2012--end----

			Endif

******* Added By Amrendra For Bug-2179 On 23-5-2012
			If _curvouobj.pcvtype='IP' And Used('projectitref_vw')
				Select aentry_ty,atran_cd,aitserial,qty From projectitref_vw Where entry_ty=main_vw.entry_ty And tran_cd=main_vw.tran_cd And itserial=item_vw.itserial Into Cursor tibl
				If Used ('tibl')
					If(tibl.qty=0)
* RETURN .T.		&& Commented By Shrikant S. on 05/09/2012 for Bug-5930
					Endif
				Endif
*!*					If Item_vw.Qty != tibl.Qty			&& Commented By Shrikant S. on 05/09/2012 for Bug-5930
				If item_vw.qty != tibl.qty And tibl.qty>0					&& Added By Shrikant S. on 05/09/2012 for Bug-5930
					=Messagebox('Quantity cannot be changed as selected Qty is : '+Alltrim(Str(tibl.qty,16,company.Deci)),0+64,vumess)
					Replace qty With tibl.qty In item_vw
					Return .F.
				Endif
			Endif
******* Added By Amrendra For Bug-2179 On 23-5-2012

********** 	Added By Shrikant S. on 24/08/2012 for Bug-5930		&& Start
			If _curvouobj.pcvtype='IP' And Used('Othitref_Vw')
				With _curvouobj.voupage.page1.grditem
					For tcnt = 1 To .ColumnCount Step 1
						colcontrolsource = "upper(alltrim(.column"+Alltrim(Str(tcnt))+".controlsource))"
						ccond            = &colcontrolsource
						If Alltrim(ccond) = 'ITEM_VW.U_FORPICK'
							_curvouobj.voupage.page1.grditem.Columns(tcnt).cmdpick.Click()
						Endif
					Endfor
				Endwith
			Endif
********** 	Added By Shrikant S. on 24/08/2012 for Bug-5930		&& End

********** 	Added By Amrendra on 28/11/2012 for Bug-4973		&& Start
*Birendra : Bug-15000 on 08/06/2013 :Start:
*			If INLIST(_curvouobj.pcvtype,'DC','ST') And (Used('Othitref_Vw') or Used('projectitref_Vw')) && Added By Amrendra on 16/01/2013 for Bug-7863
			If INLIST(_curvouobj.pcvtype,'DC','ST') And (Used('projectitref_Vw')) 
				SELECT projectitref_Vw
				LOCATE FOR entry_ty==main_vw.entry_ty And tran_cd==main_vw.tran_cd And ALLTRIM(itserial)==ALLTRIM(item_vw.itserial) AND it_code==item_vw.it_code
				IF FOUND()
*Birendra : Bug-15000 on 08/06/2013 :End:
					With _curvouobj.voupage.page1.grditem
						For tcnt = 1 To .ColumnCount Step 1
							colcontrolsource = "upper(alltrim(.column"+Alltrim(Str(tcnt))+".Header1.Caption))"
							ccond            = &colcontrolsource
							If Upper(Alltrim(ccond)) = 'RECEIPT'
								_curvouobj.voupage.page1.grditem.Columns(tcnt).cmdbom.Click()
							Endif
						Endfor
					ENDWITH
				ENDIF &&Birendra : Bug-15000 on 08/06/2013 :endif:
			Endif
********** 	Added By Amrendra on 28/11/2012 for Bug-4973		&& End

*!*	********** 	Added By Amrendra on 28/11/2012 for Bug-4973		&& Start
*!*	*			If Inlist(_curvouobj.pcvtype,'DC','ST') And Used('Othitref_Vw') && Commented By Amrendra on 16/01/2013 for Bug-7863
*!*				If INLIST(_curvouobj.pcvtype,'DC','ST') And (Used('Othitref_Vw') or Used('projectitref_Vw')) && Added By Amrendra on 16/01/2013 for Bug-7863
*!*					With _curvouobj.voupage.page1.grditem
*!*						For tcnt = 1 To .ColumnCount Step 1
*!*							colcontrolsource = "upper(alltrim(.column"+Alltrim(Str(tcnt))+".Header1.Caption))"
*!*							ccond            = &colcontrolsource
*!*							If Upper(Alltrim(ccond)) = 'RECEIPT'
*!*								_curvouobj.voupage.page1.grditem.Columns(tcnt).cmdbom.Click()
*!*							Endif
*!*						Endfor
*!*					Endwith
*!*				Endif
*!*	********** 	Added By Amrendra on 28/11/2012 for Bug-4973		&& End

******* Added By Sachin N. S. on 02/03/2012 for Bug-2239 ******* Start
			If _curvouobj.pcvtype='OP'
				Select item_vw
				mentry_ty=item_vw.entry_ty
				mtran_cd=item_vw.tran_cd
				mitserial=item_vw.itserial
				mbomid=item_vw.bomid
				mbomlevel=item_vw.bomlevel
				If !Empty(mbomid)		&& Added By Sachin N. S. on 30/04/2012 for Bug-3856
					Select Top 1 item_no From item_vw Where entry_ty=mentry_ty And tran_cd=mtran_cd And itserial!=mitserial And bomid=mbomid And ;
						bomlevel=mbomlevel And ritserial=mitserial Order By item_no Into Cursor _cur1	&& Changed By Sachin N. S. on 30/04/2012 for Bug-3856
*!*							BomLevel=mBomLevel Order By Item_no Into Cursor _cur1
					If _Tally>0
						=Messagebox("Line no. "+Alltrim(Transform(_cur1.item_no))+" is referring this Item. Cannot change the quantity.",0+16,vumess)
						Use In _cur1
						Return .F.
					Endif
				Endif
			Endif
******* Added By Sachin N. S. on 02/03/2012 for Bug-2239 ******* End

		Endif
	Endif
Endif
&& Added by Shrikant. S. on 13/02/2012 for Bug-2057 		&& Start
If Type('_curvouobj.PcvType') = 'C' 		&& Aded by Shrikant S. on 02/08/2012 for auto updater 10.4.10
	If _Screen.ActiveForm.voupage.page1.grditem.column2.text1.Value<>Val(_Screen.ActiveForm.voupage.page1.grditem.column2.text1.Tag)
		If (lcode_vw.inv_stk='+'  And company.neg_itbal=.F. And Empty(item_vw.dc_no) And _curvouobj.editmode ) And !("vutex" $ vchkprod)
			Select item_vw
			lirecno=Iif(!Eof(),Recno(),0)
			_balitemname = ALLTRIM(item_vw.item)	&&Changes has been done by vasant on 10/11/2012 as per Bug 6773 (Facing issue in edit mode of opening stock Transaction). 
			mitemname=""
			balqty =0
&&Changes done by vasant on 22/11/12 as per Bug-7185 (Issue while Deleting a Dependency Transaction Due to Rule Validation.).
*!*				msqlstr="Select sum(Case when B.Inv_stk='+' then A.Qty Else (Case when B.Inv_stk='-' then -Qty Else 0 End)end) as Qty From It_balw A "+;
*!*					" Inner join Lcode B ON (A.Entry_ty = B.Entry_Ty ) Inner Join It_Mast It ON(It.It_code=A.It_code) "+;
*!*					" where B.inv_stk != ' ' And It.It_Name=?item_vw.item And A.[rule]=?main_vw.rule"+;
*!*					" and A.ware_nm=?item_vw.ware_nm  "
			msqlstr = "Select sum(Case when B.Inv_stk='+' then A.Qty Else (Case when B.Inv_stk='-' then -Qty Else 0 End)end) as Qty From It_balw A "
			msqlstr	= msqlstr + " Inner join Lcode B ON (A.Entry_ty = B.Entry_Ty ) Inner Join It_Mast It ON(It.It_code=A.It_code) "
			msqlstr	= msqlstr + " where B.inv_stk != ' ' And It.It_Name=?item_vw.item "
			msqlstr	= msqlstr + " and A.ware_nm=?item_vw.ware_nm  "
			_mRule = ''
			If 'vutex' $ vchkprod
				If Inlist(Upper(main_vw.Rule),'EXCISE','NON-EXCISE')
					msqlstr  = msqlstr + " And a.[Rule] = ?Main_vw.Rule "
					_mRule = main_vw.Rule
				Else
					msqlstr  = msqlstr + " And a.[Rule] NOT IN ('EXCISE','NON-EXCISE') "
					_mRule = 'Others'
				Endif
			Else
				If Inlist(Upper(main_vw.Rule),'NON-MODVATABLE','ANNEXURE V')
					msqlstr  = msqlstr + " And a.[Rule] = ?Main_vw.Rule "
					_mRule = main_vw.Rule
				Else
					msqlstr  = msqlstr + " And a.[Rule] NOT IN ('NON-MODVATABLE','ANNEXURE V') "
					_mRule = 'EXCISE'
				Endif
			Endif
&&Changes done by vasant on 22/11/12 as per Bug-7185 (Issue while Deleting a Dependency Transaction Due to Rule Validation.).
			sql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,[chktbl_vw],"This.Parent.nHandle",_curvouobj.DataSessionId,.F.)
			If Used('chktbl_vw')
				Select chktbl_vw
				balqty = Iif(Isnull(qty) = .F.,qty,0)
				Use In chktbl_vw
			Endif
&& Added and commnted by satish pal for bug-6064 dated 21/09/2012--Start
			msqlstr="Select SUM(Qty) as qty From "+Alltrim(_curvouobj.entry_tbl)+"item Where Entry_ty='"+Alltrim(item_vw.entry_ty)+"' and Tran_cd="+Str(item_vw.tran_cd)+" and item=?ALLTRIM(item_vw.ITEM)"
*			msqlstr="Select SUM(Qty) as qty From "+ALLTRIM(_curvouobj.entry_tbl)+"item Where Entry_ty='"+ALLTRIM(item_vw.entry_ty)+"' and Tran_cd="+STR(item_vw.tran_cd)+" and item='"+ALLTRIM(item_vw.ITEM)+"'"
&& Added and commnted by satish pal for bug-6064 dated 21/09/2012--end
			sql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,[chkqty_tbl],"This.Parent.nHandle",_curvouobj.DataSessionId,.F.)
			If Used('chkqty_tbl')
				Select chkqty_tbl
				prevqty = Iif(Isnull(qty) = .F.,qty,0)
				Use In chkqty_tbl
			Endif
			qtychk=0
			Select item_vw
			*Scan For Item=item_vw.Item						&&Changes has been done by vasant on 10/11/2012 as per Bug 6773 (Facing issue in edit mode of opening stock Transaction). 
			SCAN FOR ALLTRIM(ITEM) == ALLTRIM(_balitemname)	&&Changes has been done by vasant on 10/11/2012 as per Bug 6773 (Facing issue in edit mode of opening stock Transaction). 
				qtychk=qtychk+item_vw.qty
			Endscan
			If lirecno> 0
				Go lirecno
			Endif
			If qtychk < prevqty -balqty
&&Changes done by vasant on 22/11/12 as per Bug-7185 (Issue while Deleting a Dependency Transaction Due to Rule Validation.).
*mitemname=ALLTRIM(item_vw.ITEM)+", "
				If !('~#~'+Alltrim(item_vw.Item)+'~#~' $ mitemname)
					mitemname=mitemname+Iif(!Empty(mitemname),', ','')+'~#~'+Alltrim(item_vw.Item)+'~#~'
				Endif
&&Changes done by vasant on 22/11/12 as per Bug-7185 (Issue while Deleting a Dependency Transaction Due to Rule Validation.).
			Endif
*mitemname=SUBSTR(mitemname,1,LEN(mitemname)-2)	&&Changes done by vasant on 22/11/12 as per Bug-7185 (Issue while Deleting a Dependency Transaction Due to Rule Validation.).

			Select item_vw
			If !Empty(mitemname)
&&Changes done by vasant on 22/11/12 as per Bug-7185 (Issue while Deleting a Dependency Transaction Due to Rule Validation.).
*.showmessagebox("Less "+ALLTRIM(main_vw.RULE)+" Stock available for item(s) "+ALLTRIM(mitemname)+CHR(13)+CHR(13)+"Entry Cannot be Changed",0+32,vumess)
				mitemname = Strtran(mitemname,'~#~','')
				.showmessagebox("Less "+Iif(Inlist(Upper(_mRule),'EXCISE','MODVATABLE'),'Modvatable','Non-Modvatable')+" Stock available for item(s) "+Alltrim(mitemname)+Chr(13)+Chr(13)+"Entry Cannot be Changed",0+32,vumess)
&&Changes done by vasant on 22/11/12 as per Bug-7185 (Issue while Deleting a Dependency Transaction Due to Rule Validation.).
				Return .F.
			Endif
		Endif
*!*		&& Added By Shrikant S. on 20/02/2012 for Bug-2057		&& Start
		If lcode_vw.inv_stk='+'  And Empty(item_vw.or_sr) And _curvouobj.editmode
			msqlstr="Select Top 1 entry_ty,Tran_cd,rqty=SUM(ISNULL(rQty,0)) from Othitref Where Rentry_ty=?item_vw.entry_ty and itref_Tran=?item_vw.tran_cd  and Ritserial=?item_vw.itserial Group by entry_ty,Tran_cd"
			sql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,[chkReftbl],"This.Parent.nHandle",_curvouobj.DataSessionId,.F.)
			If Used('chkReftbl')
				Select chkreftbl
				lcmsg="Quantity cannot be less than "+Alltrim(Str(chkreftbl.rqty))+" as it is used in '"+Allt(chkreftbl.entry_ty)+"' transaction."
				If chkreftbl.rqty>0 And chkreftbl.rqty > item_vw.qty
					.showmessagebox(lcmsg,0+32,vumess)
					Use In chkreftbl
					Return .F.
				Endif
			Endif
		Endif
*!*		&& Added By Shrikant S. on 20/02/2012 for Bug-2057		&& End
	Endif
&&Added By Shrikant S. on 29/12/2012 for Bug-2267		&& Start	&&vasant030412
	If _curvouobj.voupage.page1.grditem.column2.text1.Value<>Val(_curvouobj.voupage.page1.grditem.column2.text1.Tag)
		_mstkresrvtn = .F.
		_mstkresrvtn = oGlblPrdFeat.UdChkProd('stkresrvtn')
		If _mstkresrvtn = .T.
			If (lcode_vw.entry_ty ='SO' Or lcode_vw.bcode_nm ='SO')
				_mQty = 0
				msqlstr="Select Top 1 AllocQty From StkResrvSum Where Entry_ty = ?Main_vw.Entry_ty And Tran_cd = ?Main_vw.Tran_cd ;
					And Itserial = ?Item_vw.Itserial"
				sql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,[chktbl_vw],"This.Parent.nHandle",_curvouobj.DataSessionId,.F.)
				If sql_con > 0 And Used('chktbl_vw')
					Select chktbl_vw
					_mQty = Iif(Isnull(chktbl_vw.AllocQty)=.F.,chktbl_vw.AllocQty,0)
					Use In chktbl_vw
				Endif
				Select item_vw
				If item_vw.qty < _mQty
					_mRateUnit = ''
					msqlstr="Select Top 1 RateUnit From It_mast Where It_code = ?Item_vw.It_code "
					sql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,[chktbl_vw],"This.Parent.nHandle",_curvouobj.DataSessionId,.F.)
					If sql_con > 0 And Used('chktbl_vw')
						Select chktbl_vw
						_mRateUnit = chktbl_vw.RateUnit
						Use In chktbl_vw
					Endif
					Select item_vw
					.showmessagebox("Stock Reservation done Quantity is "+Alltrim(Transform(_mQty))+' '+_mRateUnit,0+32,vumess)
					If !Empty(_malias)
						Select &_malias
					Endif
					If Betw(_mrecno,1,Reccount())
						Go _mrecno
					Endif
					Return .F.
				Endif
			Endif
*If (INLIST(lcode_vw.entry_ty,'OP','WK','PO','AR','PT') OR INLIST(lcode_vw.bcode_nm,'OP','WK','PO','AR','PT'))
			If (Inlist(lcode_vw.entry_ty,'WK','PO') Or Inlist(lcode_vw.bcode_nm,'WK','PO') Or lcode_vw.inv_stk = '+')
				_mQty = 0
				msqlstr="Select SUM(AllocQty) as AllocQty From StkResrvDet Where Entry_ty = ?Main_vw.Entry_ty And Tran_cd = ?Main_vw.Tran_cd ;
					And Itserial = ?Item_vw.Itserial"
				sql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,[chktbl_vw],"This.Parent.nHandle",_curvouobj.DataSessionId,.F.)
				If sql_con > 0 And Used('chktbl_vw')
					Select chktbl_vw
					_mQty = Iif(Isnull(chktbl_vw.AllocQty)=.F.,chktbl_vw.AllocQty,0)
					Use In chktbl_vw
				Endif
				Select item_vw
				If item_vw.qty < _mQty
					_mRateUnit = ''
					msqlstr="Select Top 1 RateUnit From It_mast Where It_code = ?Item_vw.It_code "
					sql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,[chktbl_vw],"This.Parent.nHandle",_curvouobj.DataSessionId,.F.)
					If sql_con > 0 And Used('chktbl_vw')
						Select chktbl_vw
						_mRateUnit = chktbl_vw.RateUnit
						Use In chktbl_vw
					Endif
					Select item_vw
					.showmessagebox("Stock Reservation done Quantity is "+Alltrim(Transform(_mQty))+' '+_mRateUnit,0+32,vumess)
					If !Empty(_malias)
						Select &_malias
					Endif
					If Betw(_mrecno,1,Reccount())
						Go _mrecno
					Endif
					Return .F.
				Endif
			Endif
		Endif
	Endif
&&Added By Shrikant S. on 29/12/2012 for Bug-2267		&& End	&&vasant030412
Endif
&& Added by Shrikant. S. on 13/02/2012 for Bug-2057 		&& End

******** Added By Sachin N. S. on 15/07/2011 for Batchwise/Serialize Inventory ******** Start
If Type('_curvouobj.PcvType') = 'C' 		&& Aded by Shrikant S. on 02/08/2012 for auto updater 10.4.10
	If _Screen.ActiveForm.voupage.page1.grditem.column2.text1.Value<>Val(_Screen.ActiveForm.voupage.page1.grditem.column2.text1.Tag)
		If _curvouobj.itempage And Vartype(_curvouobj._batchserialstk)='O'
			etsql_con=_curvouobj._batchserialstk._uetrigvouqtyvalid()
			Return Iif(etsql_con>0,.T.,.F.)
		Endif
	Endif
Endif
******** Added By Sachin N. S. on 15/07/2011 for Batchwise/Serialize Inventory ******** End

&& Added by Shrikant S. on 28/06/2014 for Bug-23280		&& Start
If Vartype(oglblindfeat)='O'
	If oglblindfeat.udchkind('pharmaind')
		If Type('_curvouobj.PcvType') = 'C'
			If _Screen.ActiveForm.voupage.page1.grditem.column2.text1.Value<>Val(_Screen.ActiveForm.voupage.page1.grditem.column2.text1.Tag)
				If _curvouobj.pcvtype='WK' And Used('wkrmdet_vw')
					Select entry_ty,tran_cd,qty From wkrmdet_vw Where entry_ty=main_vw.entry_ty And tran_cd=main_vw.tran_cd Group By entry_ty,tran_cd,qty Into Cursor tibl
					If item_vw.qty != tibl.qty And tibl.qty>0
						=Messagebox('Quantity cannot be changed as selected Qty is : '+Alltrim(Str(tibl.qty,16,company.Deci)),0+64,vumess)
						Replace qty With tibl.qty In item_vw
						Return .F.
					Endif
				Endif
			Endif
		Endif
	Endif
Endif
&& Added by Shrikant S. on 28/06/2014 for Bug-23280		&& End


If !Empty(_malias)
	Select &_malias
Endif
If Betw(_mrecno,1,Reccount())
	Go _mrecno
Endif
*<--Checking Allocated entry
&&<--Ipop(Rup)
