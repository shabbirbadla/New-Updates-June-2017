Para colno
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
*!*----->IP & OP FOR BatchProcess(Rup)
*	if main_vw.entry_ty='IP' and inlist(colno,3) and USED('projectitref_vw') AND ([vuexc] $ vchkprod)
*Birendra Bug-4543 on 31/07/2012 : Commented and modified with Below one:
	If Inlist(main_vw.entry_ty,'IP','WI') And Inlist(colno,3) And Used('projectitref_vw') And ([vuexc] $ vchkprod)
		_Malias 	= Alias()
		_mRecNo	= Recno()
&& Added By Pankaj B. on 14-03-2015 for Bug-25542 and Bug-25243 Start
		Select Sum(qty) As qty From projectitref_vw Where (entry_ty=main_vw.entry_ty And tran_cd=main_vw.tran_cd And itserial=item_vw.itserial) Into Cursor tibl1
		If Used ('tibl1')
			If (item_vw.qty=tibl1.qty)
				Use In tibl1
				Return .T.
			Endif
		Endif
&& Added By Pankaj B. on 14-03-2015 for Bug-25542 and Bug-25243 end
		Select aentry_ty,atran_cd,aitserial,qty From projectitref_vw Where entry_ty=main_vw.entry_ty And tran_cd=main_vw.tran_cd And itserial=item_vw.itserial Into Cursor tibl
		If Used ('tibl')
			If(tibl.qty=0)
				Return .T.
			Endif
			If item_vw.qty<tibl.qty
				Replace qty With item_vw.qty,qty With item_vw.qty In projectitref_vw For entry_ty=main_vw.entry_ty And tran_cd=main_vw.tran_cd And itserial=item_vw.itserial
			Endif
			If item_vw.qty>tibl.qty

				etsql_con	= 1
				nHandle     = 0
				_etDataSessionId = _Screen.ActiveForm.DataSessionId
				Set DataSession To _etDataSessionId
				SqlConObj = Newobject('SqlConnUdObj','SqlConnection',xapps)

				etsql_str = "EXECUTE USP_ENT_BOMDET_CHK_IP '"+item_vw.entry_ty+"',"+Alltrim(Str(item_vw.tran_cd))+",'"+Alltrim(main_vw.Rule);
					+"','"+Alltrim(main_vw.inv_sr)+"','"+Alltrim(main_vw.cate)+"','"+Alltrim(main_vw.dept);
					+"','"+tibl.aentry_ty+"',"+Alltrim(Str(tibl.atran_cd))+",'"+tibl.aitserial+"',"+Alltrim(Str(item_vw.it_code))

				etsql_con = SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[tibl_1],;
					"nHandle",_etDataSessionId,.F.)
				If Used('tibl_1')
					Select Sum(qty) As tqy From projectitref_vw Where aentry_ty=tibl.aentry_ty And atran_cd=tibl.atran_cd And aitserial=tibl.aitserial And ait_code=item_vw.it_code	And itserial<>item_vw.itserial Into Cursor tibl_2
					If Used('tibl_2')
						Select 	tibl_2
						Replace balqty With balqty-Iif(!Isnull(tibl_2.tqy),tibl_2.tqy,0) In tibl_1
*balitem_vw.qty)>balitem_vw.balqty+balitem_vw.tlissqtyp
						If item_vw.qty>tibl_1.balqty+tibl_1.tlissqtyp
							=Messagebox('Qty. Could not be grater then '+Alltrim(Str(tibl_1.balqty+tibl_1.tlissqtyp,14,3)),0+64,VuMess)
							Replace qty With tibl.qty In item_vw
							_Screen.ActiveForm.Voupage.Page1.Grditem.Columns(colno-1).text1.SetFocus()
							Return .F.
						Else
							Replace qty With item_vw.qty,qty With item_vw.qty In projectitref_vw For entry_ty=main_vw.entry_ty And tran_cd=main_vw.tran_cd And itserial=item_vw.itserial
						Endif
						Use In tibl_2
					Endif
					Use In tibl_1
				Endif
			Endif
			Use In tibl
		Endif
		If !Empty(_Malias)
			Select &_Malias
		Endif
		If Betw(_mRecNo,1,Reccount())
			Go _mRecNo
		Endif
	Endif

*-----------------
	If main_vw.entry_ty='OP' And Inlist(colno,3) And Used('projectitref_vw') And ([vuexc] $ vchkprod)
		_Malias 	= Alias()
		_mRecNo	= Recno()
		Select aentry_ty,atran_cd,aitserial,qty From projectitref_vw Where entry_ty=main_vw.entry_ty And tran_cd=main_vw.tran_cd And itserial=item_vw.itserial Into Cursor tibl
		If Used ('tibl')
			Select tibl
			If Reccount()>0

				If item_vw.qty<tibl.qty
					Replace qty With item_vw.qty,qty With item_vw.qty In projectitref_vw For entry_ty=main_vw.entry_ty And tran_cd=main_vw.tran_cd And itserial=item_vw.itserial
				Endif
				If item_vw.qty>tibl.qty
					etsql_con	= 1
					nHandle     = 0
					_etDataSessionId = _Screen.ActiveForm.DataSessionId
					Set DataSession To _etDataSessionId
					SqlConObj = Newobject('SqlConnUdObj','SqlConnection',xapps)

					etsql_str = "USP_ENT_CHK_OP_ALLOCATION '"+item_vw.entry_ty+"',"+Alltrim(Str(item_vw.tran_cd))+","+Alltrim(Str(item_vw.it_code))+",'"+item_vw.itserial+"','";
						+tibl.aentry_ty+"',"+Alltrim(Str(tibl.atran_cd))+",'"+tibl.aitserial+"'"

					etsql_con = SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[tibl_1],"nHandle",_etDataSessionId,.F.)
					If Used('tibl_1')
						Select Sum(qty) As tqy From projectitref_vw Where aentry_ty=tibl.aentry_ty And atran_cd=tibl.atran_cd And aitserial=tibl.aitserial And ait_code=item_vw.it_code	And itserial<>item_vw.itserial Into Cursor tibl_2
						If Used('tibl_2')
							Select 	tibl_2
							Replace balqty With balqty-Iif(!Isnull(tibl_2.tqy),tibl_2.tqy,0),wipqty With wipqty-Iif(!Isnull(tibl_2.tqy),tibl_2.tqy,0) In tibl_1
							If ((item_vw.qty>tibl_1.wipqty) And tibl_1.wipqty<>0)
								=Messagebox('Qty. could not be grater then '+Alltrim(Str(tibl_1.wipqty,14,3)),0+64,VuMess)
								Replace qty With tibl.qty In item_vw
								_Screen.ActiveForm.Voupage.Page1.Grditem.Columns(colno-1).text1.SetFocus()
								Return .F.
							Else
								Replace qty With item_vw.qty In projectitref_vw For entry_ty=main_vw.entry_ty And tran_cd=main_vw.tran_cd And itserial=item_vw.itserial
							Endif
							Use In tibl_2
						Endif
						Use In tibl_1
					Endif
				Endif
			Endif 	&&IF RECCOUNT>0
			Use In tibl
		Endif 		&&IF used ('tibl')
		If !Empty(_Malias)
			Select &_Malias
		Endif
		If Betw(_mRecNo,1,Reccount())
			Go _mRecNo
		Endif
	Endif
*-----------------
*if main_vw.entry_ty ='ST' and inlist(colno,3) and USED('projectitref_vw') AND (([vuexc] $ vchkprod) Or ([vuinv] $ vchkprod))
	If Inlist(main_vw.entry_ty,'DC','ST') And Inlist(colno,2) And Used('projectitref_vw') And ([vuexc] $ vchkprod)
		_Malias = Alias()
		_mRecNo	= Recno()
		Select projectitref_vw
		Locate
		paqty=projectitref_vw.aqty
		Select Sum(qty) As qty From projectitref_vw Where (entry_ty=main_vw.entry_ty And tran_cd=main_vw.tran_cd And itserial=item_vw.itserial) Into Cursor tibl &&aentry_ty,atran_cd,aitserial,
		If Used ('tibl')
			If (item_vw.qty=tibl.qty)
				Use In tibl
				Return .T.
			Endif
			Select aentry_ty,atran_cd,aitserial,qty From projectitref_vw Where entry_ty=main_vw.entry_ty And tran_cd=main_vw.tran_cd And itserial=item_vw.itserial Into Cursor tibl1
			Select tibl1
			If (Reccount()<=0)
				Use In tibl
				Use In tibl1
				Return .T.
			Endif
			If (Reccount()>1) And (item_vw.qty<>tibl.qty)
				=Messagebox('Please delete this item then make new entry with New Allocation...!'),0+64,VuMess)
				Replace qty With tibl.qty In item_vw
				_Screen.ActiveForm.Voupage.Page1.Grditem.Columns(colno-1).text1.SetFocus()
				Use In tibl
				Use In tibl1
				Return .F.
			Endif

			If item_vw.qty<tibl.qty
				Replace qty With item_vw.qty In projectitref_vw For entry_ty=main_vw.entry_ty And tran_cd=main_vw.tran_cd And itserial=item_vw.itserial
			Endif

			If item_vw.qty>tibl.qty
				etsql_con	= 1
				nHandle     = 0

				_etDataSessionId = _Screen.ActiveForm.DataSessionId
				Set DataSession To _etDataSessionId
				SqlConObj = Newobject('SqlConnUdObj','SqlConnection',xapps)
				etsql_str = "USP_ENT_CHK_ST_ALLOCATION '"+item_vw.entry_ty+"',"+Alltrim(Str(item_vw.tran_cd))+",'"+item_vw.itserial+"',"+Alltrim(Str(item_vw.it_code))+",'";
					+tibl1.aentry_ty+"',"+Alltrim(Str(tibl1.atran_cd))+",'"+tibl1.aitserial+"'"

				etsql_con = SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[tibl_1],"nHandle",_etDataSessionId,.F.)
				If Used('tibl_1')
*!*					SELECT 	projectitref_vw
					If (Reccount()=0) And (item_vw.qty>paqty)
						=Messagebox('Qty. could not be grater then '+Alltrim(Str(paqty,14,3)),0+64,VuMess)
						Replace qty With tibl.qty In item_vw
						Return .F.
					Endif
					Select Sum(qty) As tqy From projectitref_vw Where aentry_ty=tibl1.aentry_ty And atran_cd=tibl1.atran_cd And aitserial=tibl1.aitserial And ait_code=item_vw.it_code	And itserial<>item_vw.itserial Into Cursor tibl_2

					If Used('tibl_2')

						Replace balqty With balqty-Iif(!Isnull(tibl_2.tqy),tibl_2.tqy,0) In tibl_1
						If item_vw.qty>tibl_1.balqty And tibl_1.balqty!=Null And tibl_1.balqty!=0
							=Messagebox('Qty. could not be grater then '+Alltrim(Str(tibl_1.balqty,14,3)),0+64,VuMess)
							Replace qty With tibl.qty In item_vw
							_Screen.ActiveForm.Voupage.Page1.Grditem.Columns(colno-1).text1.SetFocus()
							Return .F.
						Else

							Replace qty With item_vw.qty In projectitref_vw For entry_ty=main_vw.entry_ty And tran_cd=main_vw.tran_cd And itserial=item_vw.itserial
						Endif
						Use In tibl_2
					Endif

					Use In tibl_1
				Endif
			Endif
			Use In tibl
		Endif
		If !Empty(_Malias)
			Select &_Malias
		Endif
		If Betw(_mRecNo,1,Reccount())
			Go _mRecNo
		Endif
	Endif
*!*<-----IP & OP FOR BatchProcess(Rup)
Endif
&& Changes by EBS team on 07/03/14 for Bug-21466,21467,21468 start
* Changes done as per --> CR_KOEL_0002_Export_Sales_Transaction_&_Export_LC_Master_Cross_Reference
* Changes done by EBS Product Team
_mexim = oglblprdfeat.udchkprod('exim')
_curscrobj = _Screen.ActiveForm
If _mexim
	If Inlist(main_vw.entry_ty,'SI')
		If colno > 2
*!*			MESSAGEBOX("Prev LCNo: " + ALLTRIM(_curscrobj.oLCNo) + CHR(13) + "Curr LCNO: " + ALLTRIM(main_vw.lc_no) ;
*!*				+ CHR(13) + "Prev Amt: " + ALLTRIM(STR(_curscrobj.oPrevInvAmt,12,2)) + CHR(13) + "Curr Amt: " + ALLTRIM(STR(main_vw.fcnet_amt,12,2)))
			Store .T. To lcheck
			If _curscrobj.addmode = .T. Or _curscrobj.editmode = .T.
				If !Empty(main_vw.lc_no)
					Store 0 To totBal
					If Type("_curscrobj.oPrevInvAmt") <> 'U' And _curscrobj.oPrevInvAmt > 0 &&changed by priyanka_28052013
						Do Case
						Case !Empty(_curscrobj.oLCNo) And Alltrim(_curscrobj.oLCNo) <> Alltrim(main_vw.lc_no)
							totBal = _curscrobj.olcbalamt
						Case Empty(_curscrobj.oLCNo) And Alltrim(_curscrobj.oLCNo) <> Alltrim(main_vw.lc_no)
							totBal = _curscrobj.olcbalamt
						Otherwise
							totBal = _curscrobj.olcbalamt + _curscrobj.oPrevInvAmt
						Endcase
						If main_vw.fcnet_amt <> _curscrobj.oPrevInvAmt
							If (totBal < main_vw.fcnet_amt)
								lcheck = .F.
							Endif
						Else
							If (totBal < main_vw.fcnet_amt)
								lcheck = .F.
							Endif
						Endif
					Else
						If (_curscrobj.olcbalamt < main_vw.fcnet_amt)
							lcheck = .F.
						Endif
					Endif
				Endif
*!*				MESSAGEBOX("totbal: " + STR(totBal,12,2))
*!*				MESSAGEBOX(lcheck).

				If lcheck = .F.
					If Type("_curscrobj.cntGrdShowMsg") = "O"
						If Type("_curscrobj.oLCBalAmt") <> 'U'
							_curscrobj.cntGrdShowMsg.Visible = .T.
							_curscrobj.cntGrdShowMsg.lblmsg.Caption = "Warning: Invoice Amount " + Alltrim(Str(main_vw.fcnet_amt,14,2)) +;
								" cannot be greater than LC Balance Amount " + Alltrim(Str(_curscrobj.olcbalamt,16,2)) +;
								". Please select appropriate LC No."
							_curscrobj.cntGrdShowMsg.Refresh()
						Endif
					Endif
				Else
					If Type("_curscrobj.cntGrdShowMsg") = "O"		&& Added by Shrikant S. on 17/08/2015 for Bug-26703
						_curscrobj.cntGrdShowMsg.lblmsg.Caption = ''
						_curscrobj.cntGrdShowMsg.Visible = .F.
						_curscrobj.cntGrdShowMsg.Refresh()
					Endif				&& Added by Shrikant S. on 17/08/2015 for Bug-26703
				Endif
			Endif
		Endif
	Endif
Endif
* End --> CR_KOEL_0002_Export_Sales_Transaction_&_Export_LC_Master_Cross_Reference
&&changes by EBS team on 07/03/14 for Bug-21466,21467,21468 End

&& Added by Shrikant S. on 27/06/2014 for Bug-23280		&& Start
If Vartype(oglblindfeat)='O'
	If oglblindfeat.udchkind('pharmaind')
		If Type('_curvouobj.PcvType') = 'C'
			If Inlist(main_vw.entry_ty,'AR')
				If Upper(_curvouobj.Voupage.Page1.Grditem.Columns(colno).ControlSource)=="ITEM_VW.QTY"
					ConvertQty()
				Endif
			Endif
		Endif
	Endif
Endif
&& Added by Shrikant S. on 27/06/2014 for Bug-23280		&& End

