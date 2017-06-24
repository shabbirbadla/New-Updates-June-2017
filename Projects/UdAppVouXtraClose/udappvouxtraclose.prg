*!*	 =============================================
*!*	 Author:		Birendra Prasad
*!*	 Create date: 11/10/2010
*!*	 Description:	This Triger called after uextra close.
*!*	 Modified By: Birendra Prasad
*!*	 Modify date/Reason: 11/10/2010 To call _Screen.ActiveForm.itemgrdbefcalc(1) for interation of all grid item
*!*	 Modify date/Reason: 12/02/2011 TO CALL _Screen.ActiveForm.itemgrdbefcalc(1)
*!*	 Remark:
*!*	 =============================================
&& Added By Shrikant S. on 12/02/2011 for TKT-4580	---Start
_actfrm = _Screen.ActiveForm

&& Added by Shrikant on 29/10/2012 for Bug-6867		&& Start
If Type('_screen.ActiveForm.oItemControl')='O'
	If Pemstatus(_Screen.ActiveForm.oItemControl,'itemgrdbefcalc',5)
		_actfrm =_Screen.ActiveForm.oItemControl
		Return
	Endif
Endif
&& Added by Shrikant on 29/10/2012 for Bug-6867		&& End

_itemgrdclassin = .F.
If Type('_itemgrdclass') = 'L'
	_itemgrdclassin = _itemgrdclass
Endif
If _actfrm.ItemPage = .T.
	If _actfrm.Editmode = .T. Or _actfrm.AddMode = .T. && Added by Shrikant S. on 21/05/2010 for TKT-1476
		Select item_vw		&& Start : Added by Birendra Prasad on 11/10/2010 for TKT-3783 (record pointer and scan ... endscan)
		vrec = Iif(!Eof(),Recno(),0)
		If _itemgrdclassin=.T.
*Birendra : Bug-19957 on 28/10/2013 : Commented Scan...endscan statement:
*			Scan 				&& End : Added by Birendra Prasad on 11/10/2010 for TKT-3783 (record pointer and scan ... endscan)
			_actfrm.itemgrdbefcalc(1)
			Select item_vw		&&Start : Added by Birendra Prasad on 11/10/2010 for TKT-3783 (record pointer and scan ... endscan)
*			Endscan
		Else
*			_actfrm.itemgrdbefcalc(1)
			Scan 				&& Start : Added by Birendra Prasad on 12/10/2011 for Bug-60 (record pointer and scan ... endscan)
				_actfrm.itemgrdbefcalc(1)
				Select item_vw		&&End : Added by Birendra Prasad on 12/10/2011 for Bug-60 (record pointer and scan ... endscan)
			Endscan

		Endif
		Select item_vw
		If vrec > 0
			Go vrec
		Endif 				&&End: Added by Birendra Prasad on 11/10/2010 for TKT-3783 (record pointer and scan ... endscan)
	Endif 									&& Added by Shrikant S. on 21/05/2010 for TKT-1476
Endif
&& Added By Shrikant S. on 12/02/2011 for TKT-4580	---End

&& Commented By Shrikant S. on 12/02/2011 for TKT-4580	---Start
*!*	If _Screen.ActiveForm.ItemPage = .T.
*!*		If _Screen.ActiveForm.Editmode = .T. Or _Screen.ActiveForm.AddMode = .T. && Added by Shrikant S. on 21/05/2010 for TKT-1476
*!*			_Screen.ActiveForm.itemgrdbefcalc(1)
*!*		Endif 									&& Added by Shrikant S. on 21/05/2010 for TKT-1476
*!*	Endif
&& Commented By Shrikant S. on 12/02/2011 for TKT-4580	---End

&& Added by Shrikant S. on 05/10/2016 for GST		&& Start

If Type('_actfrm.pcvtype')='C'
	If Inli(_actfrm.pcvtype,'J5')
		If _actfrm.AddMode Or _actfrm.Editmode
			Select AcDet_Vw
			Go Top
			Select main_vw
			Go Top
			Replace main_vw.Party_nm With AcDet_Vw.Ac_Name,main_vw.Ac_Id With AcDet_Vw.Ac_Id In main_vw
		Endif
	Endif

	If Inli(_actfrm.pcvtype,'BR','BP','CP','CR')
		If (_actfrm.AddMode Or _actfrm.Editmode) And main_vw.tdspaytype=2 And main_vw.AGAINSTGS="SERVICES"

			If !Used("AcdetAlloc_vw")
				Return
			Else
				Select AcdetAlloc_vw
				Locate For Entry_ty=main_vw.Entry_ty And Tran_cd=main_vw.Tran_cd And Itserial=item_vw.Itserial
				If Found()
					Replace Amount With Item_vw.staxable ;
						,sabtper With item_vw.sabtper,sabtamt With item_vw.sabtamt,staxable With item_vw.staxable,Amountin With item_vw.AMTINCGST ;
						,cgst_amt With item_vw.cgst_amt,sgst_amt With item_vw.sgst_amt,igst_amt With item_vw.igst_amt;
						,cgsrt_amt With item_vw.cgsrt_amt,sgsrt_amt With item_vw.sgsrt_amt,igsrt_amt With item_vw.igsrt_amt;
						In AcdetAlloc_vw
				ENDIF
			ENDIF
			replace u_asseamt WITH Item_vw.staxable IN Item_vw
		Endif
	Endif
Endif
&& Added by Shrikant S. on 05/10/2016 for GST		&& End


