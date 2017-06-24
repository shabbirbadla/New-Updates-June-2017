*:*****************************************************************************
*:        Program: UpdateDebitCredit.PRG
*:         System: Udyog Software
*:         Author: RAGHU
*:  Last modified: 19/09/2006
*:			AIM  : Create Account Master
*:*****************************************************************************

PARAMETERS FRDATE,TODate,sqldatasession,mReportType
IF TYPE('sqldatasession') ='N'
	SET DATASESSION TO sqldatasession
ENDIF
SET DELETED ON
nHandle = 0
sqlconobj=NEWOBJECT('sqlconnudobj',"sqlconnection",xapps)

IF TYPE('Statdesktop') = 'O'
	Statdesktop.ProgressBar.VALUE = 10
ENDIF

StrAc_Mast = "Exec Usp_Ac_Mast"
sql_con=sqlconobj.dataconn("EXE",Company.DbName,StrAc_Mast,"_CTBAcMast","nHandle",sqldatasession)
IF sql_con =< 0
	=MESSAGEBOX(MESSAGE(),0+16,VuMess)
	RETURN .T.
ENDIF

IF TYPE('Statdesktop') = 'O'
	Statdesktop.ProgressBar.VALUE = 20
ENDIF


Ldate = SET("Date")
SET DATE AMERICAN
*!*	Collecting Debit and Credit Balance [Start]
Strdrcr = "EXEC USP_AC_MASTDRCR '"+DTOC(FRDATE)+"','"+DTOC(TODate)+"'"
sql_con=sqlconobj.dataconn("EXE",Company.DbName,Strdrcr,"_TBDrcr","nHandle",sqldatasession)
IF sql_con =< 0
	=MESSAGEBOX(MESSAGE(),0+16,VuMess)
	RETURN .T.
ENDIF
*!*	Collecting Debit and Credit Balance [End]

IF TYPE('Statdesktop') = 'O'
	Statdesktop.ProgressBar.VALUE = 30
ENDIF

*!*	Collecting Opening Balance [Start]
StrOpening = "EXEC USP_AC_MASTOPENING '"+DTOC(FRDATE)+"'"
sql_con=sqlconobj.dataconn("EXE",Company.DbName,StrOpening,"_TBOpBal","nHandle",sqldatasession)
IF sql_con =< 0
	=MESSAGEBOX(MESSAGE(),0+16,VuMess)
	RETURN .T.
ENDIF
*!*	Collecting Opening Balance [End]
SET DATE &Ldate

*!*	Grouping Opening,Debit&Credit Tables [Start]
lENGINEBEHAVIOR = SET("EngineBehavior")
SET ENGINEBEHAVIOR 70				&& Set Compiler To VFP 7.0
SELECT a.Ac_Id,a.ac_Group_Id,SUM(b.Opbal) AS Opbal,a.debit,a.credit,a.ClBal;
	FROM _CTBAcMast a,_TBOpBal b;
	ORDER BY a.Ac_Id;
	GROUP BY a.Ac_Id;
	WHERE a.Ac_Id = b.Ac_Id AND MainFlg = 'L';
	UNION ALL;
	SELECT a.Ac_Id,a.ac_Group_Id,a.Opbal,SUM(b.debit) AS debit,SUM(b.credit) AS credit,a.ClBal;
	FROM _CTBAcMast a,_TBDrcr b;
	GROUP BY a.Ac_Id;
	WHERE a.Ac_Id = b.Ac_Id AND MainFlg = 'L';
	INTO CURSOR CurTbAc_Mast

SELECT 'L' AS MainFlg,a.Ac_Id,a.ac_Group_Id,SUM(a.Opbal) AS Opbal,SUM(a.debit) AS debit,SUM(a.credit) AS credit;
	FROM CurTbAc_Mast a;
	ORDER BY a.Ac_Id;
	GROUP BY a.Ac_Id;
	INTO CURSOR CurTbAcMast

SELECT 'G' AS MainFlg,a.ac_Group_Id,0 AS Opbal,0 AS debit,0 AS credit;
	FROM CurTbAc_Mast a;
	ORDER BY a.ac_Group_Id;
	GROUP BY a.ac_Group_Id;
	INTO CURSOR CurTbAcGMast

SET ENGINEBEHAVIOR &lENGINEBEHAVIOR	&& Set Compiler To Current Version
*!*	Joining Opening,Debit&Credit Tables [End]

*!*	UPDATE Group And Ledger Opening,Debit,Credit,Closing Balance [Start]
UPDATE _CTBAcMast SET _CTBAcMast.Opbal = a.Opbal,;
	_CTBAcMast.debit = a.debit,;
	_CTBAcMast.credit = a.credit,;
	_CTBAcMast.ClBal = (a.Opbal+a.debit-a.credit);
	FROM CurTbAcMast a;
	WHERE a.Ac_Id = _CTBAcMast.Ac_Id AND _CTBAcMast.MainFlg = 'L'

*!*	Grouping Opening,Debit&Credit Tables [Start]
lENGINEBEHAVIOR = SET("EngineBehavior")
SET ENGINEBEHAVIOR 70				&& Set Compiler To VFP 7.0

SELECT a.ac_Group_Id AS Ac_Id,b.*;
	FROM _CTBAcMast a,CurTbAcGMast b;
	WHERE a.MainFlg = b.MainFlg;
	AND a.Ac_Id = b.ac_Group_Id;
	INTO CURSOR CurTbAcGMast
SET ENGINEBEHAVIOR &lENGINEBEHAVIOR	&& Set Compiler To Current Version
*!*	Grouping Opening,Debit&Credit Tables [End]

*!* Inserting Additional Fields [START]
SELECT SPACE(1) AS LevelFlg,;
	000000000000000 AS LevelInt,;
	a.*;
	FROM _CTBAcMast a;
	INTO CURSOR _CTBAcMast READWRITE
*!* Inserting Additional Fields [END]

*!*	Close Temp Cursors [Start]
=CloseTmpCursor()
*!*	Close Temp Cursors [End]

IF INLIST(mReportType,'B','P')
	mShowStkfrm = 0

	SELECT _CTBAcMast
	LOCATE FOR ALLT(Ac_Name)=IIF(mReportType='P','CLOSING STOCK (P & L)','CLOSING STOCK') AND MainFlg = 'L' AND ClBal <> 0
	IF FOUND()
		mShowStkfrm = 1
	ENDIF

	SELECT _CTBAcMast
	LOCATE FOR ALLT(Ac_Name)=IIF(mReportType='B','PROVISIONAL EXPENSES','PROVISIONAL EXPENSES (P & L)') AND MainFlg = 'L' AND ClBal <> 0
	IF FOUND()
		mShowStkfrm = mShowStkfrm+1
	ENDIF

	IF mShowStkfrm < 2
		DO FORM frmstkval WITH sqldatasession,mReportType
	ENDIF

	IF mReportType = 'B'
		=FindProfitOrLoss()
	ENDIF

ENDIF

FUNCTION CloseTmpCursor
***********************
IF USED("_TBDrcr")
	USE IN _TBDrcr
ENDIF

IF USED("_TBOpBal")
	USE IN _TBOpBal
ENDIF

IF USED("CurTbAc_Mast")
	USE IN CurTbAc_Mast
ENDIF

IF USED("CurTbAcMast")
	USE IN CurTbAcMast
ENDIF

IF USED("CurTbAcGMast")
	USE IN CurTbAcGMast
ENDIF

sql_con = sqlconobj.SqlConnClose('nHandle')
IF sql_con =< 0
	=MESSAGEBOX(MESSAGE(),0+16,VuMess)
	RETURN .T.
ENDIF

RELEASE sqlconobj,nHandle

FUNCTION FindProfitOrLoss
*************************
Mtincomeflg = findgroupid('TRADING INCOME')
Mtexpenseflg = findgroupid('TRADING EXPENSE')
Mexpenseflg = findgroupid('EXPENSE')
Mincomeflg = findgroupid('INCOME')
MLiabilflg = findgroupid('LIABILITIES')

STORE 0 TO PandLGp,mTrdIncome,mTrdExpense,mExpense,mIncome

&& Find GP
SELECT SUM(a.ClBal) AS TrdExpense;
	FROM _CTBAcMast a;
	WHERE a.ac_Group_Id = Mtexpenseflg;
	AND a.MainFlg == 'L';
	INTO CURSOR CurTrdIncome
GO TOP
mTrdExpense = IIF(ISNULL(TrdExpense),0,TrdExpense)				&& Debited

SELECT SUM(a.ClBal) AS TrdIncome;
	FROM _CTBAcMast a;
	WHERE a.ac_Group_Id = Mtincomeflg;
	AND a.MainFlg == 'L';
	INTO CURSOR CurTrdIncome
GO TOP
mTrdIncome = IIF(TrdIncome<0,TrdIncome*-1,IIF(ISNULL(TrdIncome),0,TrdIncome))	&& Credited

DO CASE
CASE mTrdIncome > mTrdExpense			&& INCOME IS GREATER THAN EXPENSE
	m.diff = mTrdIncome - mTrdExpense
	mIncome = ABS(m.diff)
CASE mTrdExpense > mTrdIncome			&& EXPENSE IS GREATER THAN INCOME
	m.diff = mTrdExpense-mTrdIncome
	mExpense = ABS(m.diff)
OTHERWISE
	mIncome = 0
	mExpense = 0
ENDCASE

&& Find NP
SELECT SUM(a.ClBal) AS Expense;
	FROM _CTBAcMast a;
	WHERE a.ac_Group_Id = Mexpenseflg;
	AND a.MainFlg == 'L';
	INTO CURSOR CurIncome
GO TOP
mExpense = mExpense+IIF(ISNULL(Expense),0,Expense)				&& Debited

SELECT SUM(a.ClBal) AS Income;
	FROM _CTBAcMast a;
	WHERE a.ac_Group_Id = Mincomeflg;
	AND a.MainFlg == 'L';
	INTO CURSOR CurIncome
GO TOP
mIncome = mIncome+IIF(Income<0,Income*-1,IIF(ISNULL(Income),0,Income))	&& Credited

DO CASE
CASE mIncome > mExpense			&& INCOME IS GREATER THAN EXPENSE
	m.diff = mIncome - mExpense
	PandLGp = -m.diff
CASE mExpense > mIncome			&& EXPENSE IS GREATER THAN INCOME
	m.diff = mExpense-mIncome
	PandLGp = m.diff
OTHERWISE
	PandLGp = 0
ENDCASE

IF USED('CurIncome')
	USE IN CurIncome
ENDIF
IF USED('CurTrdIncome')
	USE IN CurTrdIncome
ENDIF

SELECT _CTBAcMast
LOCATE FOR ALLT(Ac_Name)='NET PROFIT & LOSS' AND MainFlg = 'L'
IF FOUND()
	REPLACE _CTBAcMast.debit WITH IIF(PandLGp>0,PandLGp,0)
	REPLACE _CTBAcMast.credit WITH IIF(PandLGp<0,ABS(PandLGp),0)
	REPLACE _CTBAcMast.ClBal WITH _CTBAcMast.debit-_CTBAcMast.credit
	REPLACE _CTBAcMast.ac_Group_Id WITH MLiabilflg
ENDIF

FUNCTION findgroupid
********************
PARAMETERS mAc_Name
SELECT a.Ac_Id;
	FROM _CTBAcMast a;
	WHERE a.Ac_Name = mAc_Name;
	INTO CURSOR FindLevelFlg
GO TOP
LevelFlg = Ac_Id
IF USED('FindLevelFlg')
	USE IN FindLevelFlg
ENDIF
RETURN LevelFlg
