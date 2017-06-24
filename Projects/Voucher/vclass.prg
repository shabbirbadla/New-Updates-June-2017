DEFINE CLASS MyPage As Page

	Func Activate
		This.FontBold=.t.
		Thisform.PgRefresh(This.Name)
		Thisform.ActivateClicked =.t.
	Endfunc

	Func DeActivate
		This.FontBold=.f.
		Thisform.ActivateClicked =.f.
	Endfunc

	Func Refresh
		IF Thisform.ActivateClicked =.f.
			Thisform.LockScreen = .t.
			Thisform.PgRefresh(This.Name)
			Thisform.LockScreen = .f.
		Endif	
	Endfunc

Enddefine