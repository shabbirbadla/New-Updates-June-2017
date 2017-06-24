Lparameters _nDataSession

If Vartype(oGlblPrdFeat)='O'
	If oGlblPrdFeat.UdChkProd('pos')
		If Type('_screen.ActiveForm.pcvType')='C'
			If _Screen.ActiveForm.pcvType = 'PS'
				If !('udClsPointOfSale' $ Set("Classlib"))
					Set Classlib To udClsPointOfSale Additive
				Endif
				If !('Vouclass' $ Set("Classlib"))
					Set Classlib To Vouclass Additive
				Endif
			Endif
		Endif
	Endif
Endif
