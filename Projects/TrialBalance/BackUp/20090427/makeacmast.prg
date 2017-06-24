*:*****************************************************************************
*:        Program: Makeacmast.PRG
*:         System: Udyog Software
*:         Author: RAGHU
*:  Last modified: 19/09/2006
*:			AIM  : Create Account Master
*:*****************************************************************************

Parameters FRDATE,TODate,sqldatasession,mReportType
If Type('sqldatasession') ='N'
	Set DataSession To sqldatasession
Endif
Set Deleted On
nHandle =0
sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)

If Type('Statdesktop') = 'O'
	Statdesktop.ProgressBar.Value = 10
Endif

Ldate = Set("Date")
Set Date AMERICAN
*!*	Collecting Debit and Credit Balance [Start]
Strdrcr = "EXEC Usp_Final_Accounts '"+Dtoc(FRDATE)+"','"+Dtoc(TODate)+"','"+Dtoc(company.Sta_Dt)+"','"+mReportType+"' "
sql_con=sqlconobj.dataconn("EXE",company.DbName,Strdrcr,"_CTBAcMast","nHandle",sqldatasession)
If sql_con =< 0
	Set Date &Ldate
	=Messagebox('Main cursor creation '+Chr(13)+Message(),0+16,VuMess)
	Return .F.
Endif

*!*	Collecting Debit and Credit Balance [End]
Set Date &Ldate

If Type('Statdesktop') = 'O'
	Statdesktop.ProgressBar.Value = 30
Endif

If mReportType = 'P'
*!*		UPDATE _CTBAcMast SET ClBal = Debit-ABS(Credit)
	Update _CTBAcMast Set ClBal = opBal+Debit-Abs(Credit)	&& Changed By Sachin N. S. on 18/03/2009
Endif


*!* Inserting Additional Fields [START]
Select Space(1) As LevelFlg,;
	SPACE(100) As OrderLevel,;
	000000000000000 As Level,;
	000000000000000 As LevelInt,;
	a.*;
	FROM _CTBAcMast a;
	INTO Cursor _CTBAcMast Readwrite
*!* Inserting Additional Fields [END]

*!*	Close Temp Cursors [Start]
=CloseTmpCursor()
*!*	Close Temp Cursors [End]

If Inlist(mReportType,'B','P')
	mShowStkfrm = 0

	Select _CTBAcMast
	Locate For Allt(Ac_Name)=Iif(mReportType='P','CLOSING STOCK (P & L)','CLOSING STOCK') And MainFlg = 'L' And ClBal <> 0
	If Found()
		mShowStkfrm = 1
	Endif

	Select _CTBAcMast
	Locate For Allt(Ac_Name)=Iif(mReportType='B','PROVISIONAL EXPENSES','PROVISIONAL EXPENSES (P & L)') And MainFlg = 'L' And ClBal <> 0
	If Found()
		mShowStkfrm = mShowStkfrm+1
	Endif

	Select _CTBAcMast
	Locate For Allt(Ac_Name)=Iif(mReportType='P','OPENING STOCK','OPENING STOCK') And MainFlg = 'L' And opBal <> 0
	If Found()
		mShowStkfrm = mShowStkfrm+1
	Endif

	If mShowStkfrm < 2
		Do Form frmstkval With sqldatasession,mReportType
	Endif

Endif
Return .T.

Function CloseTmpCursor
***********************
sql_con = sqlconobj.SqlConnClose('nHandle')
If sql_con < 0
	=Messagebox(Message(),0+16,VuMess)
	Return .T.
Endif

Release sqlconobj,nHandle
