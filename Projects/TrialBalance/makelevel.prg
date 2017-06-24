*:*****************************************************************************
*:        Program: UpdateDebitCredit.PRG
*:         System: Udyog Software
*:         Author: RAGHU
*:  Last modified: 19/09/2006
*:			AIM  : Update OrderLevel For Account Master
*:*****************************************************************************
Lparameters mReportType

Local lnI
lnI = 1
Set Deleted On

If mReportType = 'B'
	LnLiabilityId = findgroupFlg('LIABILITIES','ID')
	Select _CTBAcMast
	Locate For Allt(Ac_name)='NET PROFIT & LOSS' And MainFlg = 'L'
	If Found() And debit = 0 And credit = 0
		Replace Ac_group_id With LnLiabilityId
	Endif
Endif

ASCIIVAL = 64
*!*	Update _CTBAcMast Set Level = 1 , OrderLevel = IncAFunction(Ac_name) Where MainFlg = 'G' And Ac_Id = Ac_group_id	&& Commented by Sachin N. S. on 11/07/2012 for Bug-4539
=UpdtOrderLevel('_CTBAcMast')		&& Added by Sachin N. S. on 11/07/2012 for Bug-4539


Select _CTBAcMast
*!*	INDEX ON MainFlg+STR(Updown)+Ac_name TAG MF_Updown
Index On Str(Updown)+Ac_name Tag MF_Updown

If Type('Statdesktop') = 'O'
	Statdesktop.ProgressBar.Value = 40
Endif
&&vasant041109
_morderlevel = ''
If mReportType <> 'P'
	_morderlevel = _morderlevel+[']+Left(findgroupFlg('LIABILITIES','ORDERLEVEL'),1)+[']
	_morderlevel = _morderlevel+[']+Left(findgroupFlg('ASSETS','ORDERLEVEL'),1)+[']
Endif
If mReportType <> 'B'
	_morderlevel = _morderlevel+[']+Left(findgroupFlg('TRADING INCOME','ORDERLEVEL'),1)+[']
	_morderlevel = _morderlevel+[']+Left(findgroupFlg('TRADING EXPENSES','ORDERLEVEL'),1)+[']
	_morderlevel = _morderlevel+[']+Left(findgroupFlg('MANUFACTURING INCOME','ORDERLEVEL'),1)+['] && Added By Hetal Dt.090410
	_morderlevel = _morderlevel+[']+Left(findgroupFlg('MANUFACTURING EXPENSES','ORDERLEVEL'),1)+['] && Added By Hetal Dt.090410
	_morderlevel = _morderlevel+[']+Left(findgroupFlg('EXPENSE','ORDERLEVEL'),1)+[']
	_morderlevel = _morderlevel+[']+Left(findgroupFlg('INCOME','ORDERLEVEL'),1)+[']
Endif
_morderlevel = Strtran(_morderlevel,[''],[','])
&&vasant041109

*!*	UPDATE NLevel Group Value [Start]
For I = 1 To 50 Step 1
	Select a.Ac_Id,a.Ac_group_id,OrderLevel;
		FROM _CTBAcMast a;
		ORDER By a.MainFlg,a.Updown;
		WHERE a.Level = I And MainFlg = 'G'  ;		&&vasant041109
	Into Cursor CurTopLevel
	If _Tally <> 0
		Select CurTopLevel
		Scan
			=FindUnderGroup(CurTopLevel.Ac_Id,I+1,OrderLevel)
			Select CurTopLevel
		Endscan
	Else
		lnI = Iif(I > 1,I-1,I)
		Exit
	Endif
Endfor
*!*	UPDATE NLevel Group Value [End]

If Used("CurTopLevel")
	Use In CurTopLevel
Endif

Select _CTBAcMast
*!*	*!*	IF MESSAGEBOX("Copy To _CTBAcMast",4+32,Vumess) = 6
*!*	*!*		SELECT _CTBAcMast
*!*	*!*		COPY TO C:\_CTBAcMast
*!*	*!*	ENDIF

Delete For Isnull(OrderLevel)

Update _CTBAcMast Set LevelFlg = Left(Alltrim(OrderLevel),1),;
	LevelINT = Val(Right(Strtran(Alltrim(OrderLevel),"/",''),Len(Alltrim(Strtran(Alltrim(OrderLevel),"/",'')))-1))

If Type('Statdesktop') = 'O'
	Statdesktop.ProgressBar.Value = 50
Endif

If mReportType = 'B'
	=FindProfitOrLoss()
Endif

=UpdateDebitCredit()

Select _CTBAcMast
Delete For (Empty(Opbal) And Empty(debit) And Empty(credit))

&&vasant041109
If Inlist(mReportType,'P','B')
	Update _CTBAcMast Set Level = Level - 1 Where Level > 0
Endif
_Tally = 0
Select Distinct Level From _CTBAcMast Where Level > 0 And Inlist(Left(OrderLevel,1),&_morderlevel) Into Cursor _TBAcMast
lnI = _Tally
&&vasant041109

Select Iif(a.Level<>1,Space(a.Level*2)+a.Ac_name,a.Ac_name) As Ac_Name2,;
	a.*;
	FROM _CTBAcMast a;
	ORDER By a.LevelFlg,a.OrderLevel,a.Ac_group_id;
	INTO Cursor _TBAcMast Readwrite		&&vasant190909

Select _TBAcMast
Go Top
Return lnI


Function FindUnderGroup
***********************
Parameters gAc_Id,m_Level,mOrderLevel
IVAL = 0
Select _CTBAcMast
Set Filter To Ac_group_id = gAc_Id And Level = 0
Set Order To MF_Updown
Scan
	Replace Level With m_Level
	Replace OrderLevel With IncNFunction(mOrderLevel)
	Select _CTBAcMast
Endscan
Select _CTBAcMast
Set Filter To


Function IncAFunction
********************
Lparameters lcAc_Name
*!*	*!*	ASCIIVAL = ASCIIVAL + 1
Do Case
	Case Upper(lcAc_Name) = 'LIABILITIES'
		ASCIIVAL = 65
	Case Upper(lcAc_Name) = 'ASSETS'
		ASCIIVAL = 66
	Case Upper(lcAc_Name) = 'INCOME'
		ASCIIVAL = 67
	Case Upper(lcAc_Name) = 'EXPENSE'
		ASCIIVAL = 68
	Case Upper(lcAc_Name) = 'TRADING INCOME'
		ASCIIVAL = 69
	Case Upper(lcAc_Name) = 'TRADING EXPENSES'
		ASCIIVAL = 70
	Case Upper(lcAc_Name) = 'MANUFACTURING INCOME'
		ASCIIVAL = 71
	Case Upper(lcAc_Name) = 'MANUFACTURING EXPENSES'
		ASCIIVAL = 72
Endcase
Return Chr(ASCIIVAL)

Function IncNFunction
*********************
Parameters mOrderLevel
IVAL = IVAL + 1
IncFun = Alltrim(mOrderLevel)+'/'+Replicate('0',7-Len(+Alltrim(Str(IVAL))))+Alltrim(Str(IVAL))
Return IncFun


Function FindProfitOrLoss
*************************
Mtincomeflg = findgroupFlg('TRADING INCOME','FLAG')
Mtexpenseflg = findgroupFlg('TRADING EXPENSES','FLAG')
Mexpenseflg = findgroupFlg('EXPENSE','FLAG')
Mincomeflg = findgroupFlg('INCOME','FLAG')
Mmincomeflg = findgroupFlg('MANUFACTURING INCOME','FLAG')		&& Added By Sachin N. S. on 26/04/2010 for TKT-541
Mmexpenseflg = findgroupFlg('MANUFACTURING EXPENSES','FLAG')	&& Added By Sachin N. S. on 26/04/2010 for TKT-541


Store 0 To PandLGp,mTrdIncome,mTrdExpense,mExpense,mIncome,mMfgIncome,mMfgExpense

&& Find GP
Select Sum(a.ClBal) As TrdExpense;
	FROM _CTBAcMast a;
	WHERE a.LevelFlg = Mtexpenseflg;
	INTO Cursor CurTrdIncome
Go Top
mTrdExpense = Iif(Isnull(TrdExpense),0,TrdExpense)				&& Debited

Select Sum(a.ClBal) As TrdIncome;
	FROM _CTBAcMast a;
	WHERE a.LevelFlg = Mtincomeflg;
	INTO Cursor CurTrdIncome
Go Top
&&Changes done by vasant on 020112 as per Bug 1280 - (Trial balance not matching with Profit & Loss and Balance sheet) 
*mTrdIncome = Iif(TrdIncome<0,TrdIncome*-1,Iif(Isnull(TrdIncome),0,TrdIncome))	&& Credited
mTrdIncome = Iif(Isnull(TrdIncome),0,TrdIncome)

*!*	Do Case
*!*		Case mTrdIncome > mTrdExpense			&& INCOME IS GREATER THAN EXPENSE
*!*			m.diff = mTrdIncome - mTrdExpense
*!*			mIncome = Abs(m.diff)
*!*		Case mTrdExpense > mTrdIncome			&& EXPENSE IS GREATER THAN INCOME
*!*			m.diff = mTrdExpense-mTrdIncome
*!*			mExpense = Abs(m.diff)
*!*		Otherwise
*!*			mIncome = 0
*!*			mExpense = 0
*!*	Endcase
&&Changes done by vasant on 020112 as per Bug 1280 - (Trial balance not matching with Profit & Loss and Balance sheet) 

***Hetal Patel Dt.090410 ST
Mmincomeflg = findgroupFlg('MANUFACTURING INCOME','FLAG')
Mmexpenseflg = findgroupFlg('MANUFACTURING EXPENSES','FLAG')

&& Find GP
Select Sum(a.ClBal) As TrdExpense;
	FROM _CTBAcMast a;
	WHERE a.LevelFlg = Mmexpenseflg;
	INTO Cursor CurTrdIncome
Go Top
&&Changes done by vasant on 020112 as per Bug 1280 - (Trial balance not matching with Profit & Loss and Balance sheet) 
*mMfgExpense = mMfgExpense + Iif(Isnull(TrdExpense),0,TrdExpense)				&& Debited
mMfgExpense = mTrdExpense + Iif(Isnull(TrdExpense),0,TrdExpense)				&& Debited	
&&Changes done by vasant on 020112 as per Bug 1280 - (Trial balance not matching with Profit & Loss and Balance sheet) 

Select Sum(a.ClBal) As TrdIncome;
	FROM _CTBAcMast a;
	WHERE a.LevelFlg = Mmincomeflg;
	INTO Cursor CurTrdIncome
Go Top
&&Changes done by vasant on 020112 as per Bug 1280 - (Trial balance not matching with Profit & Loss and Balance sheet) 
*mMfgIncome = mMfgIncome + Iif(TrdIncome<0,TrdIncome*-1,Iif(Isnull(TrdIncome),0,TrdIncome))	&& Credited
mMfgIncome = mTrdIncome + Iif(Isnull(TrdIncome),0,TrdIncome)	&& Credited	
mMfgIncome = -mMfgIncome

*!*	******* Added By Sachin N. S. on 26/04/2010 for TKT-541 ******* Start
*!*	Do Case
*!*		Case mIncome > mMfgExpense			&& INCOME IS GREATER THAN EXPENSE
*!*			m.diff = mIncome - mMfgExpense
*!*			mIncome = Abs(m.diff)
*!*		Case mExpense > mMfgIncome			&& EXPENSE IS GREATER THAN INCOME
*!*			m.diff = mExpense-mMfgIncome
*!*			mExpense = Abs(m.diff)
*!*		Otherwise
*!*			mIncome  = 0
*!*			mExpense = 0
*!*	Endcase
*!*	******* Added By Sachin N. S. on 26/04/2010 for TKT-541 ******* End
*!*	***Hetal Patel Dt.090410 ED
Do Case
	Case mMfgIncome > mMfgExpense			&& INCOME IS GREATER THAN EXPENSE
		m.diff = mMfgIncome - mMfgExpense
		mIncome = Abs(m.diff)
	&&Case mExpense > mMfgIncome			&& EXPENSE IS GREATER THAN INCOME &&&commented by satish pal for bug-14683 dated 15/06/2013
	Case mMfgExpense > mMfgIncome			&& EXPENSE IS GREATER THAN INCOME  &&Added by satish pal for bug-14683 dated 15/06/2013
		m.diff = mMfgExpense-mMfgIncome
		mExpense = Abs(m.diff)
	Otherwise
		mIncome  = 0
		mExpense = 0
Endcase
&&Changes done by vasant on 020112 as per Bug 1280 - (Trial balance not matching with Profit & Loss and Balance sheet) 


&& Find NP
Select Sum(a.ClBal) As Expense;
	FROM _CTBAcMast a;
	WHERE a.LevelFlg = Mexpenseflg;
	INTO Cursor CurIncome
Go Top
mExpense = mExpense+Iif(Isnull(Expense),0,Expense)				&& Debited

Select Sum(a.ClBal) As Income;
	FROM _CTBAcMast a;
	WHERE a.LevelFlg = Mincomeflg;
	INTO Cursor CurIncome
Go Top
&&Changes done by vasant on 020112 as per Bug 1280 - (Trial balance not matching with Profit & Loss and Balance sheet) 
*mIncome = mIncome+Iif(Income<0,Income*-1,Iif(Isnull(Income),0,Income))	&& Credited
mIncome = mIncome-Iif(Isnull(Income),0,Income)	&& Credited
&&Changes done by vasant on 020112 as per Bug 1280 - (Trial balance not matching with Profit & Loss and Balance sheet) 

Do Case
	Case mIncome > mExpense			&& INCOME IS GREATER THAN EXPENSE
		m.diff = mIncome - mExpense
		PandLGp = -m.diff
	Case mExpense > mIncome			&& EXPENSE IS GREATER THAN INCOME
		m.diff = mExpense-mIncome
		PandLGp = m.diff
	Otherwise
		PandLGp = 0
Endcase

If Used('CurIncome')
	Use In CurIncome
Endif

If Used('CurTrdIncome')
	Use In CurTrdIncome
Endif

Select _CTBAcMast
Locate For Allt(Ac_name)='NET PROFIT & LOSS' And MainFlg = 'L'
If Found()
	Replace _CTBAcMast.debit With Iif(PandLGp>0,PandLGp,0)
	Replace _CTBAcMast.credit With Iif(PandLGp<0,Abs(PandLGp),0)
	Replace _CTBAcMast.ClBal With _CTBAcMast.debit-_CTBAcMast.credit
Endif

Function findgroupFlg
********************
Parameters mAc_Name,tcType
&&vasant041109
*!*	SELECT a.LevelFlg,a.Ac_Id;
*!*		FROM _CTBAcMast a;
*!*		WHERE a.Ac_name = mAc_Name;
*!*		INTO CURSOR FindLevelFlg
*!*	GO TOP
*!*	lcLevelFlg = IIF(tcType='ID',Ac_Id,LevelFlg)
_MlcLevelFld = tcType
Do Case
	Case tcType='ID'
		_MlcLevelFld = 'Ac_id'
	Case tcType='FLAG'
		_MlcLevelFld = 'LevelFlg'
Endcase
Select &_MlcLevelFld ;
	FROM _CTBAcMast ;
	WHERE Ac_name = mAc_Name;
	INTO Cursor FindLevelFlg
Go Top
lcLevelFlg = &_MlcLevelFld
&&vasant041109
If Used('FindLevelFlg')
	Use In FindLevelFlg
Endif
Return lcLevelFlg


***** Added by Sachin N. S. on 11/07/2012 for Bug-4539 ***** Start
Function UpdtOrderLevel
Lparameters _lTblAcmast

Select Ac_name,Ac_Id, Updown, Iif(Upper(Ac_name)='LIABILITIES','A',Iif(Upper(Ac_name)='ASSETS','B',Iif(Upper(Ac_name)='INCOME','C', ;
	IIF(Upper(Ac_name)='EXPENSE','D',Iif(Upper(Ac_name)='TRADING INCOME','E',Iif(Upper(Ac_name)='TRADING EXPENSES','F', ;
	IIF(Upper(Ac_name)='MANUFACTURING INCOME','G',Iif(Upper(Ac_name)='MANUFACTURING EXPENSES','H','I')))))))) As LevelOrd ;
From (_lTblAcmast) Where MainFlg = 'G' And Ac_Id = Ac_group_id And Empty(OrderLevel) Order By Updown, LevelOrd Into Cursor _curr1

ASCIIVAL = 65
Select _curr1
Scan
	Select _curr1
	Update (_lTblAcmast) Set Level = 1 , OrderLevel = Chr(ASCIIVAL) Where MainFlg = 'G' And Ac_Id = Ac_group_id And Ac_Id=_curr1.Ac_Id

	ASCIIVAL = ASCIIVAL+1
	Select _curr1
Endscan

Endfunc
***** Added by Sachin N. S. on 11/07/2012 for Bug-4539 ***** End
