if used('_FINDMENU')
	select _FINDMENU
	use
endif
create cursor _FINDMENU (Menu_Name c(100))
SELECT prompname as 'Menu_Name' FROM TEMPTABLE INTO cursor _aFINDMENU   &&vasant
SELECT _FINDMENU
append from dbf('_aFINDMENU')
replace ALL Menu_Name WITH ALLTRIM(STRTRAN(Menu_Name,'\<','')) in _FINDMENU
mPartyName=getpop('_FINDMENU','Select Name','Menu_Name')
ff = _screen.ActiveForm.pageframe1.page1.oletreeNode.Nodes.Count
IF !EMPTY(mpartyname)
	_screen.ActiveForm.pageframe1.page1.oletreeNode.visible = .f.
	FOR n = 1 TO ff
		IF ALLTRIM(_screen.ActiveForm.pageframe1.page1.oletreeNode.Nodes.Item(n).Text) == PROPER(ALLTRIM(mPartyName))
			_screen.ActiveForm.pageframe1.page1.oletreeNode.Nodes.Item(n).Selected = .T.
		ELSE
			_screen.ActiveForm.pageframe1.page1.oletreeNode.Nodes.Item(n).Selected = .f.
			xu = 2
		ENDIF
	ENDFOR
	_screen.ActiveForm.grid1.visible = .f.
	_screen.ActiveForm.label1.visible = .t.
_screen.ActiveForm.pageframe1.page1.oletreeNode.visible = .t.	
ELSE
*	=MESSAGEBOX("Menu Not Found!!",16,vumess)	
ENDIF

