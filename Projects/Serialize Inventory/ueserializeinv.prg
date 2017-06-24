*!*	Lparameters _cEntTyp,_cItem,_nDataSession,_cDispType
Lparameters _cEntTyp,_cItem,_nDataSession,_cDispType, _lStkReserve		&& Changed by Sachin N. S. on 26/02/2014 for Bug-21381

If 'serialinv' $ vchkprod
	If !('BatchSerialStk' $ Set("Classlib"))
		Set Classlib To BatchSerialStk Additive
	Endif
	If !('Vouclass' $ Set("Classlib"))
		Set Classlib To Vouclass Additive
	Endif

*!*		Do Form ueFrmBatchSerialNo With _cEntTyp,_cItem,_nDataSession,_cDispType
	Do Form ueFrmBatchSerialNo With _cEntTyp,_cItem,_nDataSession,_cDispType, _lStkReserve		&& Changed by Sachin N. S. on 26/02/2014 for Bug-21381
Endif
