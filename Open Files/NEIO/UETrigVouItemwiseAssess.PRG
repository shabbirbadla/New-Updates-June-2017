massamt  = 0
_mrprate = 0
_mabtper = 0
If Type('item_vw.u_mrprate') = 'N'
	_mrprate = item_vw.u_mrprate
	_mabtper = item_vw.abtper
Endif
Sele item_vw
If _mrprate#0
	If _mabtper#0
		massamt = Round((QTY*_mrprate)-(QTY*_mrprate*_mabtper)/100,2)
	Else
		massamt = Round((QTY*_mrprate),2)
	Endif
Else
	****** Changed By Sachin N. S. on 28/06/2010 for TKT-2669 ****** Start
	If _Screen.ActiveForm.Multi_Cur = .T.
		If Upper(Alltrim(main1_vw.Fcname)) != Upper(Alltrim(Company.Currency)) And !Empty(main1_vw.Fcname)		&&vasant300609	&&vasant300609
			massamt = Round(QTY*FCRATE,2)
		Else
			massamt = Round(QTY*RATE,2)
		Endif
	Else
		massamt = Round(QTY*RATE,2)
	Endif
	*!*	massamt = Round(QTY*RATE,2)
	****** Changed By Sachin N. S. on 28/06/2010 for TKT-2669 ****** End
Endif
Return massamt
