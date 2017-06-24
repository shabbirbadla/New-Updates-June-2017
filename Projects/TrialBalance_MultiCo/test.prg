Define Class LeftRightManager As Custom
	DuplicateCursor = ''
	NumRecno = 0
	TmpCurSor = ''
	LeftOrRight = ''
	StrCursor = ''
	ActiveFilter = 0

	Function MakeAdjust
	Para mOrderLevel,mAc_Id,mAc_Group_Id,LeftOrRight,ShowHide,StrCursor,mActiveFilter
	Try
		This.LeftOrRight = LeftOrRight
		This.StrCursor = StrCursor
		This.ActiveFilter = mActiveFilter
		This.SetDuplicateCursor()
		Do Case
		Case ShowHide = 1			&& Show
			Select (This.StrCursor)
			This.NumRecno =Recno()
			mFieldName = This.LeftOrRight+'Expand'
			Replace (mFieldName) With .T.
			mFieldName = This.LeftOrRight+'Expanded'
			Replace (mFieldName) With 'Y'
			This.ShowDetail(mOrderLevel,mAc_Id,mAc_Group_Id)
			Select (This.StrCursor)
			Go (This.NumRecno)
		Case ShowHide = 2			&& Hide
			Select (This.StrCursor)
			This.NumRecno =Recno()
			mFieldName = This.LeftOrRight+'Expand'
			Replace (mFieldName) With .F.
			mFieldName = This.LeftOrRight+'Expanded'
			Replace (mFieldName) With 'N'
			This.HideDetail(mOrderLevel,mAc_Id,mAc_Group_Id)
			Select (This.StrCursor)
			Go (This.NumRecno)
		Endcase
		This.CloseTmpCursor()
	Catch To oErr1
		This.errorhandler(oErr1)
	Endtry
	Endfun

	Function ShowDetail
	Para mOrderLevel,mAc_Id,mAc_Group_Id
	Try
		This.TmpCurSor = Sys(2015)
		Select _TBAcMast.*,;
			.T. As Expand,;
			'Y' As Expanded;
			FROM _TBAcMast;
			WHERE Left(OrderLevel,mLenOrdLevel)=mOrderLevel;
			AND (Allt(Str(Ac_Id))+":"+Allt(Str(Ac_Group_Id))<>Allt(Str(mAc_Id))+":"+Allt(Str(mAc_Group_Id)));
			INTO Cursor (This.TmpCurSor)
		If 	_Tally <> 0
			This.InsertValue(This.LeftOrRight)
		Endif
	Catch To oErr1
		This.errorhandler(oErr1)
	Endtry
	Endfun

	Function HideDetail
	Para mOrderLevel,mAc_Id,mAc_Group_Id
	Try
		This.TmpCurSor = Sys(2015)
		Select _TBAcMast.*,;
			.T. As Expand,;
			'Y' As Expanded;
			FROM _TBAcMast;
			WHERE Left(OrderLevel,mLenOrdLevel)=mOrderLevel;
			AND (Allt(Str(Ac_Id))+":"+Allt(Str(Ac_Group_Id))<>Allt(Str(mAc_Id))+":"+Allt(Str(mAc_Group_Id)));
			INTO Cursor (This.TmpCurSor)
		If 	_Tally <> 0
			Select (This.StrCursor)
			LOrR = Iif(This.LeftOrRight='L',This.LeftOrRight,'R')
			Delete For Left(Eval(LOrR+'OrderLevel'),mLenOrdLevel)=mOrderLevel;
				AND (Allt(Str(Eval(LOrR+'Ac_Id')))+":"+Allt(Str(Eval(LOrR+'Ac_Group_Id')))<>Allt(Str(mAc_Id))+":"+Allt(Str(mAc_Group_Id)))
			Laexact = Set("Exact")
			Set Exact On
			This.SETCURSOR(LOrR)
			Set Exact &Laexact
		Endif
	Catch To oErr1
		This.errorhandler(oErr1)
	Endtry
	Endfun

	Function InsertValue
	Para LOrR
	Try
		Laexact = Set("Exact")
		Set Exact On
		Select (This.StrCursor)
		Insert Blank
		Select (This.TmpCurSor)
		mFCount = Fcount()
		Go Top
		Do While ! Eof()
			For I = 1 To mFCount Step 1
				Select (This.TmpCurSor)
				mFieldName = Allt(Iif(LOrR='L',LOrR,'R')+Allt(Field(I)))
				mFieldVal = Evaluate(Field(I))
				Select (This.StrCursor)
				If Inlist(mFieldName,LOrR+'AC_NAME2',LOrR+'ORDERLEVEL',LOrR+'LEVEL',LOrR+'MAINFLG',LOrR+'AC_ID',LOrR+'AC_GROUP_ID',LOrR+'AC_NAME',LOrR+'GROUP',LOrR+'CLBAL')<>.F.
					If mFieldName = 'RCLBAL'
						Replace (mFieldName) With Iif((mFieldVal)<0,Abs(mFieldVal),Iif((mFieldVal)>0,-1*(mFieldVal),0))
					Else
						Replace (mFieldName) With (mFieldVal)
					Endif
				Endif
			Endfor
			Select (This.StrCursor)
			mFieldName = LOrR+'Expand'
			Replace (mFieldName) With .T.
			mFieldName = LOrR+'Expanded'
			Replace (mFieldName) With 'Y'
			Select (This.TmpCurSor)
			Skip
			If ! Eof()
				Select (This.StrCursor)
				Insert Blank
			Endif
		Enddo

		This.SETCURSOR(LOrR)

		Set Exact &Laexact
	Catch To oErr1
		This.errorhandler(oErr1)
	Endtry

	Endfun

	Function RemoveValue
	Para LeftOrRight
	Endfunc

	Function SetDuplicateCursor
	Try
		This.DuplicateCursor = Sys(2015)
		This.NumRecno = Recno()
		Select * From (This.StrCursor) Into Cursor (This.DuplicateCursor) Readwrite
	Catch To oErr1
		This.errorhandler(oErr1)
	Endtry
	Endfunc

	Function SETCURSOR
	Parameters LOrR
	Select (This.StrCursor)
	Go Top
	mFCount = Fcount()
	TmpLOrR = Iif(LOrR='L','R','L')
	For I = 1 To mFCount Step 1
		mFieldName = Allt(Field(I))
		mFieldVal = This.FunEmptyVal(Eval(mFieldName))
		Select (This.StrCursor)
		If Inlist(mFieldName,TmpLOrR+'AC_NAME2',TmpLOrR+'ORDERLEVEL',TmpLOrR+'LEVEL',TmpLOrR+'MAINFLG',TmpLOrR+'AC_ID',TmpLOrR+'AC_GROUP_ID',TmpLOrR+'AC_NAME',TmpLOrR+'GROUP',TmpLOrR+'CLBAL')<>.F.
			Replace All (mFieldName) With (mFieldVal)
		Endif
		Select (This.StrCursor)
		Go Top
	Endfor

	Select (This.DuplicateCursor)
	Delete For Empty(Eval(TmpLOrR+'Ac_Name'))
	Go Top
	mFCount = Fcount()
	Do While ! Eof()
		Select (This.StrCursor)
		Go Top
		Locate For Empty(Eval(TmpLOrR+'AC_NAME'))
		If !Found()
			Append Blank
		Endif
		For I = 1 To mFCount Step 1
			Select (This.DuplicateCursor)
			mFieldName = Allt(Field(I))
			mFieldVal = Eval(mFieldName)
			If Inlist(mFieldName,TmpLOrR+'AC_NAME2',TmpLOrR+'ORDERLEVEL',TmpLOrR+'LEVEL',TmpLOrR+'MAINFLG',TmpLOrR+'AC_ID',TmpLOrR+'AC_GROUP_ID',TmpLOrR+'AC_NAME',TmpLOrR+'GROUP',TmpLOrR+'CLBAL')<>.F.
				Select (This.StrCursor)
				Replace (mFieldName) With (mFieldVal)
			Endif
		Endfor
		Select (This.DuplicateCursor)
		Skip
	Enddo
	Endfunc


	Function CloseTmpCursor
	Try
		If Used(This.DuplicateCursor)
			Use In (This.DuplicateCursor)
		Endif
		If Used(This.TmpCurSor)
			Use In (This.TmpCurSor)
		Endif
		This.DuplicateCursor = ''
		This.NumRecno = 0
	Catch To oErr1
		This.errorhandler(oErr1)
	Endtry
	Endfunc

	Function FunEmptyVal
	Parameters mKeyfld
	Try
		Private Y
		Y = ''
		Do Case
		Case Type('mKeyfld')='C'
			Y = ''
		Case Type('mKeyfld')='D'
			Y = {}
		Case Type('mKeyfld')='N'
			Y = 0
		Case Type('mKeyfld') = 'L'
			Y = .F.
		Endcase
	Catch To oErr1
		This.errorhandler(oErr1)
	Endtry
	Return Y
	Endfunc


	Function errorhandler
	Lpara ErrorObj As Object
	Local StingMess As String
	tingMess = ''
	StingMess = StingMess +Chr(13)+[  Error: ] + Str(ErrorObj.ErrorNo)
	StingMess = StingMess +Chr(13)+[  LineNo: ] + Str(ErrorObj.Lineno)
	StingMess = StingMess +Chr(13)+[  Message: ] + ErrorObj.Message
	StingMess = StingMess +Chr(13)+[  Procedure: ] + ErrorObj.Procedure
	StingMess = StingMess +Chr(13)+[  Details: ] + ErrorObj.Details
	StingMess = StingMess +Chr(13)+[  StackLevel: ] + Str(ErrorObj.StackLevel)
	StingMess = StingMess +Chr(13)+[  LineContents: ] + ErrorObj.LineContents
	StingMess = StingMess +Chr(13)+[  UserValue: ] + ErrorObj.UserValue
	Messagebox(StingMess,0+64,VuMess)
	Endfunc

Enddefine
