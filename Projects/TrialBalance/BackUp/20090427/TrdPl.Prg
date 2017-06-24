Define Class TrdPl As Custom
	ParentObj = ''
	leftcursor = 'L'+Sys(3)		&& Trading Accounts
	Rightcursor = 'R'+Sys(3)	&& Profit And Loss Accounts

	Procedure CurCreation
	Lparameters CurType As String
	Do Case
	Case CurType = 'L'
		Create Cursor (This.leftcursor) (LAC_NAME2 C(54),LExpand l,LExpanded C(1),LORDERLEVEL C(50),LLevel I(4),LMAINFLG C(1),LAC_ID N(20,0),LAC_GROUP_ID N(20,0),LAC_NAME C(50),LGroup C(50),LCLBAL N(19,2),RAC_NAME2 C(54),RExpand l,RExpanded C(1),RORDERLEVEL C(50),RLevel I(4),RMAINFLG C(1),RAC_ID N(20,0),RAC_GROUP_ID N(20,0),RAC_NAME C(50),RGroup C(50),RCLBAL N(19,2))
	Case CurType = 'R'
		Create Cursor (This.Rightcursor) (LAC_NAME2 C(54),LExpand l,LExpanded C(1),LORDERLEVEL C(50),LLevel I(4),LMAINFLG C(1),LAC_ID N(20,0),LAC_GROUP_ID N(20,0),LAC_NAME C(50),LGroup C(50),LCLBAL N(19,2),RAC_NAME2 C(54),RExpand l,RExpanded C(1),RORDERLEVEL C(50),RLevel I(4),RMAINFLG C(1),RAC_ID N(20,0),RAC_GROUP_ID N(20,0),RAC_NAME C(50),RGroup C(50),RCLBAL N(19,2))
	Case CurType = 'B'
		Create Cursor (This.leftcursor) (LAC_NAME2 C(54),LExpand l,LExpanded C(1),LORDERLEVEL C(50),LLevel I(4),LMAINFLG C(1),LAC_ID N(20,0),LAC_GROUP_ID N(20,0),LAC_NAME C(50),LGroup C(50),LCLBAL N(19,2),RAC_NAME2 C(54),RExpand l,RExpanded C(1),RORDERLEVEL C(50),RLevel I(4),RMAINFLG C(1),RAC_ID N(20,0),RAC_GROUP_ID N(20,0),RAC_NAME C(50),RGroup C(50),RCLBAL N(19,2))
		Create Cursor (This.Rightcursor) (LAC_NAME2 C(54),LExpand l,LExpanded C(1),LORDERLEVEL C(50),LLevel I(4),LMAINFLG C(1),LAC_ID N(20,0),LAC_GROUP_ID N(20,0),LAC_NAME C(50),LGroup C(50),LCLBAL N(19,2),RAC_NAME2 C(54),RExpand l,RExpanded C(1),RORDERLEVEL C(50),RLevel I(4),RMAINFLG C(1),RAC_ID N(20,0),RAC_GROUP_ID N(20,0),RAC_NAME C(50),RGroup C(50),RCLBAL N(19,2))
	Endcase
	Endproc

	Procedure PandLcurupdate
	Lparameters mLOR As String ,CurName As String ,CFlg As String ,NLevel As Integer,NFltrLevel As Integer
*!*	mLOR 		: L/R i.e Left/Right
*!*	CurName		: CursorName Update Which
*!*	CFlg		: Flag for TradingI/E,PAndL I/E
*!*	NLevel		: Accounts Level
	Local oErr1 As Exception
	Try
		Select(CurName)
		Go Top
		Do Case
		Case mLOR = 'L'
			Locate For Empty(LAC_NAME2)
			If ! Found()
				Append Blank
			Endif
		Case mLOR = 'R'
			Locate For Empty(RAC_NAME2)
			If ! Found()
				Append Blank
			Endif
		Endcase

		Laexact = Set("Exact")
		Set Exact On
		Select _TBAcMast
		If Type('NFltrLevel') = 'N'
			Set Filter To LevelFlg = CFlg And Between(Level,This.ParentObj.CurrentLevel,NFltrLevel)
		Else
			Set Filter To LevelFlg = CFlg And Level = NLevel
		Endif
		If This.ParentObj.Shownull = .F.
			mOldFilter = Set("Filter")
			Set Filter To (!Empty(opbal) Or !Empty(Debit) Or !Empty(Credit)) And &mOldFilter
		Endif
		mLastCursor = Alias()
		mTotalFlds = Fcount()
		Go Top
		Do While ! Eof()
			Select (mLastCursor)
			mFieldName = 'CLBAL'
			mFieldVal = Evaluate(mFieldName)
			If mFieldVal <> 0
				For I = 1 To mTotalFlds Step 1
					Select (mLastCursor)
					mFieldName = Iif(mLOR='L','L','R')+Allt(Field(I))
					mFieldVal = Evaluate(Field(I))
					Select (CurName)
					If Inlist(mFieldName,mLOR+'AC_NAME2',mLOR+'ORDERLEVEL',mLOR+'LEVEL',mLOR+'MAINFLG',mLOR+'AC_ID',mLOR+'AC_GROUP_ID',mLOR+'AC_NAME',mLOR+'GROUP',mLOR+'CLBAL') = .T.
						If mFieldName = 'RCLBAL'
							Replace (mFieldName) With Iif((mFieldVal)<0,Abs(mFieldVal),Iif((mFieldVal)>0,-1*(mFieldVal),0))
						Else
							Replace (mFieldName) With (mFieldVal)
						Endif
					Endif
				Endfor
				Select (CurName)
				mFieldName = mLOR+'Expand'
				mFieldLevel = mLOR+'Level'
				MFldVal = This.ParentObj.tbrfinalac.TxtLevel.Value
				Replace (mFieldName) With Iif(Evaluate(mFieldLevel)=MFldVal,.T.,.F.)
				mFieldName = mLOR+'Expanded'
				Replace (mFieldName) With Iif(Evaluate(mFieldLevel)=MFldVal,'N','Y')
				Skip
				If Eof()
					Append Blank
				Endif
			Endif
			Select (mLastCursor)
			Skip
		Enddo
		Set Exact &Laexact
		Select _TBAcMast
		Set Filter To
		Select(CurName)
	Catch To oErr1
		Thisform.errorhandler(oErr1)
	Endtry
	Endproc

	Procedure UpdateGP
	Select (This.leftcursor)
	Sum LCLBAL To m.dr For LLevel = 2 And LAC_NAME <> "GROSS PROFIT c/d"
	Sum RCLBAL To m.cr For RLevel = 2 And RAC_NAME <> "GROSS LOSS c/d"
	Do Case
	Case m.cr > m.dr
		m.diff = m.cr - m.dr
		Locate For Empty(LAC_NAME2)
		If ! Found()
			Append Blank
		Endif
		Replace LAC_NAME2 With "GROSS PROFIT c/d",;
			LExpand With .F.,;
			LExpanded With 'Y',;
			LORDERLEVEL With '',;
			LLevel With 2,;
			LMAINFLG With 'L',;
			LAC_ID With 1,;
			LAC_GROUP_ID With 2,;
			LAC_NAME With "GROSS PROFIT c/d",;
			LGroup With "GROSS PROFIT c/d",;
			LCLBAL With Abs(m.diff)
	Case m.dr > m.cr
		m.diff = m.dr-m.cr
		Locate For Empty(RAC_NAME2)
		If ! Found()
			Append Blank
		Endif
		Replace RAC_NAME2 With "GROSS LOSS c/d",;
			RExpand With .F.,;
			RExpanded With 'Y',;
			RORDERLEVEL With '',;
			RLevel With 2,;
			RMAINFLG With 'L',;
			RAC_ID With 1,;
			RAC_GROUP_ID With 2,;
			RAC_NAME With "GROSS LOSS c/d",;
			RGroup With "GROSS LOSS c/d",;
			RCLBAL With Abs(m.diff)
	Endcase

	Select (This.Rightcursor)		&& == Carry Forward Gpl
	Do Case
	Case m.cr > m.dr				&& == Income is more than expense
		m.diff = m.cr - m.dr
		Append Blank
		Replace RAC_NAME2 With "GROSS PROFIT b/d",;
			RExpand With .F.,;
			RExpanded With 'Y',;
			RORDERLEVEL With '',;
			RLevel With 2,;
			RMAINFLG With 'L',;
			RAC_ID With 1,;
			RAC_GROUP_ID With 2,;
			RAC_NAME With "GROSS PROFIT b/d",;
			RGroup With "GROSS PROFIT b/d",;
			RCLBAL With Abs(m.diff)
	Case m.dr > m.cr				&& == Expense is greater than income
		m.diff = m.dr-m.cr
		Append Blank
		Replace LAC_NAME2 With "GROSS LOSS  b/d",;
			LExpand With .F.,;
			LExpanded With 'Y',;
			LORDERLEVEL With '',;
			LLevel With 2,;
			LMAINFLG With 'L',;
			LAC_ID With 1,;
			LAC_GROUP_ID With 2,;
			LAC_NAME With "GROSS LOSS  b/d",;
			LGroup With "GROSS LOSS  b/d",;
			LCLBAL With Abs(m.diff)
	Otherwise
		Append Blank
	Endcase
	Endproc

	Procedure UpdateNP
	Select (This.Rightcursor)
	Sum LCLBAL To m.dr For LLevel = 2 And LAC_NAME <> "NET PROFIT b/d"
	Sum RCLBAL To m.cr For RLevel = 2 And RAC_NAME <> "NET LOSS b/d"
	Go Top
	Do Case
	Case m.cr > m.dr			&& ====== INCOME IS GREATER THAN EXPENSE
		m.diff =  m.cr - m.dr
		Locate For Empty(LAC_NAME2)
		If ! Found()
			Append Blank
		Endif
		Replace LAC_NAME2 With "NET PROFIT b/d",;
			LExpand With .F.,;
			LExpanded With 'Y',;
			LORDERLEVEL With '',;
			LLevel With 2,;
			LMAINFLG With 'L',;
			LAC_ID With 1,;
			LAC_GROUP_ID With 2,;
			LAC_NAME With "NET PROFIT b/d",;
			LGroup With "NET PROFIT b/d",;
			LCLBAL With Abs(m.diff)
	Case m.dr > m.cr			&& ===== EXPENSE IS GREATER THAN INCOME
		m.diff = m.dr-m.cr
		Locate For Empty(RAC_NAME2)
		If ! Found()
			Append Blank
		Endif
		Replace RAC_NAME2 With "NET LOSS b/d",;
			RExpand With .F.,;
			RExpanded With 'Y',;
			RORDERLEVEL With '',;
			RLevel With 2,;
			RMAINFLG With 'L',;
			RAC_ID With 1,;
			RAC_GROUP_ID With 2,;
			RAC_NAME With "NET LOSS b/d",;
			RGroup With "NET LOSS b/d",;
			RCLBAL With -Abs(m.diff)
	Endcase
	Endproc

	Procedure FindLevelFlg
	Parameters mAc_Name
	Local oErr1 As Exception
	Try
		Select A.LevelFlg;
			FROM _TBAcMast A;
			WHERE A.Ac_Name = mAc_Name;
			INTO Cursor FindLevelFlg
		Go Top
	Catch To oErr1
		Thisform.errorhandler(oErr1)
	Endtry
	Return LevelFlg
	Endproc

	Procedure errorhandler
	Lparameters ErrorObj As Object
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
	Endproc

Enddefine
