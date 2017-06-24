Parameters What,pRange

If Vartype(VuMess) <> [C]
	_Screen.Visible = .F.
	Messagebox("Internal Application Are Not Execute Out-Side ...",16)
	Return .F.
Endif

rval = .F.

If ! "\datepicker." $ Lower(Set("Classlib"))
*!*		Set Classlib To "datepicker.vcx" Additive && Commented By Shrikant S. on 15/03/2013 for Bug-8205	
	Set Classlib To apath+"class\datepicker.vcx" Additive		&& Added By Shrikant S. on 15/03/2013 for Bug-8205	
Endif

Do Form co_mast With What,pRange Name rval
