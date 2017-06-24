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
_itemgrdclassin = .F.
If Type('_itemgrdclass') = 'L'
	_itemgrdclassin = _itemgrdclass
Endif
If _actfrm.ItemPage = .T.
	If _actfrm.Editmode = .T. Or _actfrm.AddMode = .T. && Added by Shrikant S. on 21/05/2010 for TKT-1476
		Select item_vw		&& Start : Added by Birendra Prasad on 11/10/2010 for TKT-3783 (record pointer and scan ... endscan)
		vrec = Iif(!Eof(),Recno(),0)
		If _itemgrdclassin=.T.
			Scan 				&& End : Added by Birendra Prasad on 11/10/2010 for TKT-3783 (record pointer and scan ... endscan)
				_actfrm.itemgrdbefcalc(1)
				Select item_vw		&&Start : Added by Birendra Prasad on 11/10/2010 for TKT-3783 (record pointer and scan ... endscan)
			Endscan
		Else
			_actfrm.itemgrdbefcalc(1)
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
*!*			Select item_vw		&& Start : Added by Birendra Prasad on 11/10/2010 for TKT-3783 (record pointer and scan ... endscan)
*!*			vrec = Iif(!Eof(),Recno(),0)
*!*			SCAN 				&& End : Added by Birendra Prasad on 11/10/2010 for TKT-3783 (record pointer and scan ... endscan)
*!*				_Screen.ActiveForm.itemgrdbefcalc(1)
*!*			Select item_vw		&&Start : Added by Birendra Prasad on 11/10/2010 for TKT-3783 (record pointer and scan ... endscan)
*!*			ENDSCAN 
*!*			Select item_vw 
*!*			If vrec > 0 
*!*				Go vrec
*!*			ENDIF 				&&End: Added by Birendra Prasad on 11/10/2010 for TKT-3783 (record pointer and scan ... endscan)
*!*		Endif 									&& Added by Shrikant S. on 21/05/2010 for TKT-1476
*!*	Endif
&& Commented By Shrikant S. on 12/02/2011 for TKT-4580	---End
