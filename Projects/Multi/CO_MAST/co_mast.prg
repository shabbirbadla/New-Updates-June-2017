Parameters What,pRange

If Vartype(VuMess) <> [C]
	_Screen.Visible = .F.
	Messagebox("Internal Application Are Not Execute Out-Side ...",16)
	Return .F.
Endif

rval = .F.

If ! "\datepicker." $ Lower(Set("Classlib"))
*!*		Set Classlib To apath+"class\datepicker.vcx" Additive
	Set Classlib To "datepicker.vcx" Additive
Endif

Do Form co_mast With What,pRange Name rval
