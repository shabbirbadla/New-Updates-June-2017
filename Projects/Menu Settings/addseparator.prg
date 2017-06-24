o = _screen.ActiveForm.pageframe1.page1.oletreeNode
IF !ISNULL(o.SelectedItem) THEN
	IF ISNULL(_screen.ActiveForm.PAgeframe1.page1.OLETReeNode.SelectedItem.Parent) 
		_screen.ActiveForm.cnextkey = "a"+ALLTRIM(STR(1000 * RAND()))
		o.Nodes.Add(_screen.ActiveForm.PAgeframe1.page1.OLETReeNode.SElectedItem.Key, 4, _screen.ActiveForm.NewKey(), REPLICATE("_",20) ,0)
		_screen.ActiveForm.pageframe1.PAGE1.oletreeNode.labelEdit = 0  && tvwAutomatic
		_screen.ActiveForm.cformaction = "NC"
		*****************************************************************
		_screen.ActiveForm.lvar = 1
		skey = _screen.ActiveForm.PAgeframe1.page1.OLETReeNode.SelectedItem.Key
		chldcnt = _screen.ActiveForm.pageframe1.page1.oletreeNode.selectedItem.Children
		barn = _screen.ActiveForm.pageframe1.page1.oletreeNode.selectedItem.Children
		IF chldcnt > 0
			chldcnt = chldcnt + 1
		ELSE
			chldcnt = 1
		ENDIF
		SELECT PROGNaME as progname, barname as barname , padname as padname, range as range FROM temptable INTO cursor _XTEMP WHERE LEVELC == sKEY
		sELECT _XTEMP
		GO TOP
		gbrname = REPLICATE("_",20)
		trnbrnm = STRTRAN(gbrname,' ','')
		gpdname = ALLTRIM(barname)
		r1g = range
		lrng = (r1g - MOD(r1g,1000))
		lsrng = (lrng + 1000)
		SELECT MAX(range) as range from temptable WHERE (range >= lrng) AND (range < lsrng) INTO CURSOR x12
		sELECT x12
		IF RECCOUNT() > 0
			GO top
			rng = range + 1
		ENDIF
		curkey = skey + CHR(65)
		insert into TEMPTABLE values ('Y',skey,curkey, rng ,gpdname,0,trnbrnm,barn,gbrname,0,.f.,' ',' ',' ',' ',' ',' ',.t.,.t.,.t.,.t.,.t.,.t.,.t.,.t.,' ', ALLTRIM(gbrname),.t.,.t.,.f.,.f.)
		SELECT TEMPTABLE
		REPLACE numitem WITH chldcnt FOR range = r1g
		WITH _screen.ActiveForm
			.LockScreen = .T.
			.SETAPPLY = 1
			.pageframe1.page1.oletreeNode.visible = .f.
			.pageframe1.page1.oletreeNode.Nodes.Clear()
			.pg1()
			.gridformat()
			.pageframe1.page1.oletreenode.refresh()
			.LockScreen = .f.
		ENDWITH
		SELECT prompname as prompname, levelc as levelc FROM TEMPTABLE INTO CURSOR Cx WHERE RANGE = RNG
		SELECT Cx
		IF RECCOUNT() > 0
			BRN = STRTRAN(ALLTRIM(prompname),'\<','')
			ff = _screen.ActiveForm.pageframe1.page1.oletreeNode.Nodes.Count
			keyfind = levelc
			IF !EMPTY(BRN)
				FOR n = 1 TO ff
					IF ALLTRIM(_screen.ActiveForm.pageframe1.page1.oletreeNode.Nodes.Item(n).Text) == PROPER(ALLTRIM(BRN)) AND levelc = keyfind
						_screen.ActiveForm.pageframe1.page1.oletreeNode.Nodes.Item(n).Selected = .T.
					ENDIF
				ENDFOR
			ENDIF
			_screen.ActiveForm.grid1.visible = .f.
			_screen.ActiveForm.label1.visible = .t.
			_screen.ActiveForm.pageframe1.page1.oletreeNode.visible = .t.
		ENDIF
		_screen.ActiveForm.command4.Caption = "\<Cancel"
	ELSE
		=MESSAGEBOX("Cannot Create Separator For Child Menu.",64,vumess)
	ENDIF
ENDIF	