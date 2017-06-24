&&Amrendra for Bug-6496 on 27/09/2012 ---->
_Malias 	= Alias()
_mRecNo	= Recno()
_curvouobj = _Screen.ActiveForm
Set DataSession To _curvouobj.DataSessionId
QcRetstate=.T.

*Birendra : Bug-7465 on 11/12/2012 :Start:
If Not "vutex" $ vchkprod
	If Inlist(.PcvType,'AR','PT','OP','P1')
		lcStr  = [SELECT top 1 * FROM OTHITREF A where a.rentry_ty = ?Main_vw.Entry_Ty AND a.itref_Tran = ?Main_vw.Tran_cd and a.rl_yn = ?Main_vw.l_yn]
		sql_con = _curvouobj.sqlconobj.Dataconn("EXE",company.dbname,lcStr,"tmp_othitref","This.Parent.nHandle", .DataSessionId)
		If sql_con > 0 And Used('tmp_othitref')
			Select tmp_othitref
			If Reccount() > 0
				Messagebox("Entry Passed Against "+tmp_othitref.entry_ty+".",64,vuMess)
				Use In tmp_othitref
				If !Empty(_Malias)
					Select &_Malias
				Endif
				If Betw(_mRecNo,1,Reccount())
					Go _mRecNo
				Endif
				Return .F.
			Endif
		Endif
	Endif
Endif
*Birendra : Bug-7465 on 11/12/2012 :End:

If Type('_curvouobj.PCVTYPE')='C' And ([vuexc] $ vchkprod)
	If oGlblPrdFeat.UdChkProd('qctrl') And (Inlist(.PcvType,'AR','PT','OP') Or Inlist(.Behave,'AR','PT','OP'))
		QcRetstate=.F.
		sql_con = .sqlconobj.Dataconn([EXE],company.dbname,[Select Top 1 Entry_ty From qc_inspection_master where ;
			Entry_ty = ?Main_vw.Entry_ty And Tran_cd = ?Main_vw.Tran_cd],[tmptbl_vw],"This.Parent.nHandle",.DataSessionId,.F.)
		lResultInsp = .F.
		If sql_con > 0 And Used('tmptbl_vw')
			Select tmptbl_vw
			If Reccount() = 0
				lResultInsp =.T.
			Endif
		Endif

		sql_con = .sqlconobj.Dataconn([EXE],company.dbname,[Select Top 1 Entry_ty From qchistory where ;
			Entry_ty = ?Main_vw.Entry_ty And Tran_cd = ?Main_vw.Tran_cd],[tmptbl_vw],"This.Parent.nHandle",.DataSessionId,.F.)
		lResultCtrl = .F.
		If sql_con > 0 And Used('tmptbl_vw')
			Select tmptbl_vw
			If Reccount() = 0
				lResultCtrl =.T.
			Endif
		Endif

		Do Case
		Case lResultInsp =.F. And lResultCtrl =.F.
			Msg = "QC Entry Passed against this Voucher,"+Chr(13)+;
				"Are you sure you wish to delete this Voucher?"
			If .ShowMessageBox(Msg,4+32+256,vuMess,1)=7
				Return .F.
			Endif
		Case lResultInsp =.F.
			Msg = "QC Entry Passed against this Voucher,"+Chr(13)+;
				"Are you sure you wish to delete this Voucher?"
			If .ShowMessageBox(Msg,4+32+256,vuMess,1)=7
				Return .F.
			Endif
		Case lResultCtrl =.F.
			Msg = "QC Entry Passed against this Voucher,"+Chr(13)+;
				"Are you sure you wish to delete this Voucher?"
			If .ShowMessageBox(Msg,4+32+256,vuMess,1)=7
				Return .F.
			Endif
*** Added for Bug-6965 on 23/10/2012 ---->
		Case lResultInsp =.T. And lResultCtrl =.T.
			If !Empty(_Malias)
				Select &_Malias
			Endif
			If Betw(_mRecNo,1,Reccount())
				Go _mRecNo
			Endif
			Return .T.
*** Added for Bug-6965 on 23/10/2012 <----
		Endcase

		If lResultCtrl =.F. Or lResultInsp =.F.
&& Added by Shrikant S. on 29/09/2014 	for Bug-23879		&& Start
			lnInsp_id=0
			sql_con = .sqlconobj.Dataconn([EXE],company.dbname,[Select Top 1 Insp_id From qc_inspection_master where ;
			Entry_ty = ?Main_vw.Entry_ty And Tran_cd = ?Main_vw.Tran_cd],[tmptbl_vw],"This.Parent.nHandle",.DataSessionId,.F.)
			If Reccount('tmptbl_vw')>0
				Select tmptbl_vw
				Locate
				lnInsp_id=tmptbl_vw.insp_id
			Endif
			If lnInsp_id>0
				mSqlStr = _curvouobj.sqlconobj.GenDelete("qc_inspection_item","Insp_Id=?lnInsp_id")
				sql_con = _curvouobj.sqlconobj.Dataconn([EXE],company.dbname,mSqlStr,[],;
					"This.Parent.nHandle",_curvouobj.DataSessionId,.T.)

				mSqlStr = _curvouobj.sqlconobj.GenDelete("QC_INSPECTION_PARAMETER","Insp_Id=?lnInsp_id")
				sql_con = _curvouobj.sqlconobj.Dataconn([EXE],company.dbname,mSqlStr,[],;
					"This.Parent.nHandle",_curvouobj.DataSessionId,.T.)
			Endif
&& Added by Shrikant S. on 29/09/2014 	for Bug-23879		&& End
			mSqlStr = _curvouobj.sqlconobj.GenDelete("qc_inspection_master","Entry_ty = ?Main_vw.Entry_ty And Tran_cd = ?Main_vw.Tran_cd")
			sql_con = _curvouobj.sqlconobj.Dataconn([EXE],company.dbname,mSqlStr,[],;
				"This.Parent.nHandle",_curvouobj.DataSessionId,.T.)
			mSqlStr = _curvouobj.sqlconobj.GenDelete("qchistory","Entry_ty = ?Main_vw.Entry_ty And Tran_cd = ?Main_vw.Tran_cd")
			sql_con = _curvouobj.sqlconobj.Dataconn([EXE],company.dbname,mSqlStr,[],;
				"This.Parent.nHandle",_curvouobj.DataSessionId,.T.)
			QcRetstate=.T.
		Endif
	Endif
Endif


&& Added By Shrikant S. on 29/12/2012 for Bug-2267 		&&Start 	&&vasant030412
If Type('_curvouobj.PcvType') = 'C'
	_mstkresrvtn = .F.
	_mstkresrvtn = oGlblPrdFeat.UdChkProd('stkresrvtn')
	If _mstkresrvtn = .T.
		If (lcode_vw.entry_ty ='SO' Or lcode_vw.bcode_nm ='SO')
			_mQty = 0
*!*				mSqlStr="Select Top 1 AllocQty From StkResrvSum Where Entry_ty = ?Main_vw.Entry_ty And Tran_cd = ?Main_vw.Tran_cd"	 && Commented By Shrikant S. on 08/12/2012 for Bug-2267
			mSqlStr="Select Top 1 a.AllocQty From StkResrvSum a  Inner Join StkResrvDet b on (a.Entry_ty=b.rEntry_ty and a.Tran_cd=b.Rtran_cd) "+;
				" Where a.Entry_ty = ?Main_vw.Entry_ty And a.Tran_cd = ?Main_vw.Tran_cd and b.allocqty>0 " 		&& Added By Shrikant S. on 08/12/2012 for Bug-2267
			sql_con = _curvouobj.sqlconobj.Dataconn([EXE],company.dbname,mSqlStr,[chktbl_vw],"This.Parent.nHandle",_curvouobj.DataSessionId,.F.)
			If sql_con > 0 And Used('chktbl_vw')
				Select chktbl_vw
				_mQty = Iif(Isnull(chktbl_vw.AllocQty)=.F.,chktbl_vw.AllocQty,0)
				Use In chktbl_vw
			Endif
			Select item_vw
			If _mQty > 0
				.ShowMessageBox("Stock Reservation done"+Chr(13)+"Entry Cannot be Deleted",0+32,vuMess)
				If !Empty(_Malias)
					Select &_Malias
				Endif
				If Betw(_mRecNo,1,Reccount())
					Go _mRecNo
				Endif
				Return .F.
			Endif
		Endif
		If (Inlist(lcode_vw.entry_ty,'WK','PO') Or Inlist(lcode_vw.bcode_nm,'WK','PO') Or lcode_vw.inv_stk = '+')
			_mQty = 0
			mSqlStr="Select SUM(AllocQty) as AllocQty From StkResrvDet Where Entry_ty = ?Main_vw.Entry_ty And Tran_cd = ?Main_vw.Tran_cd"
			sql_con = _curvouobj.sqlconobj.Dataconn([EXE],company.dbname,mSqlStr,[chktbl_vw],"This.Parent.nHandle",_curvouobj.DataSessionId,.F.)
			If sql_con > 0 And Used('chktbl_vw')
				Select chktbl_vw
				_mQty = Iif(Isnull(chktbl_vw.AllocQty)=.F.,chktbl_vw.AllocQty,0)
				Use In chktbl_vw
			Endif
			Select item_vw
			If _mQty > 0
				.ShowMessageBox("Stock Reservation done"+Chr(13)+"Entry Cannot be Deleted",0+32,vuMess)
				If !Empty(_Malias)
					Select &_Malias
				Endif
				If Betw(_mRecNo,1,Reccount())
					Go _mRecNo
				Endif
				Return .F.
			Endif
		Endif
	Endif
Endif
&& Added By Shrikant S. on 29/12/2012 for Bug-2267 		&&End 	&&vasant030412

If !Empty(_Malias)
	Select &_Malias
Endif
If Betw(_mRecNo,1,Reccount())
	Go _mRecNo
Endif
Return	QcRetstate

&&Amrendra for Bug-6496 on 27/09/2012 <----
