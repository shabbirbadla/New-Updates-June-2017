*:*****************************************************************************
*:        Program: Makeacmast.PRG
*:         System: Udyog Software
*:         Author: RAGHU
*:  Last modified: 19/09/2006
*:			AIM  : Create Account Master
*:*****************************************************************************

Parameters FRDATE,TODate,sqldatasession,mReportType
If Type('sqldatasession') = 'N'
	Set DataSession To sqldatasession
Endif
Set Deleted On
nHandle =0
sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)

If Type('Statdesktop') = 'O'
	Statdesktop.ProgressBar.Value = 10
Endif


=Messagebox("SACHIN - MAKEACMAST - START")


Ldate = Set("Date")
Set Date AMERICAN
*!*	Collecting Debit and Credit Balance [Start]
Strdrcr = " EXEC Usp_Multi_Co_Final_Accounts '"+Dtoc(FRDATE)+"','"+Dtoc(TODate)+"','"+Dtoc(Company.Sta_Dt)+"','"+mReportType+"' "
sql_Con=sqlconobj.dataConn("EXE",Company.DbName,Strdrcr,"_CTBAcMast","nHandle",sqldatasession)
If sql_Con =< 0
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


=Messagebox("SACHIN - PROCEDURE")


SET STEP ON 


******* Changed By Sachin N. S. on 11/07/2009 ******* Start
Set ENGINEBEHAVIOR To 70
Select Space(1) As LevelFlg, ;
	SPACE(100) As OrderLevel, ;
	000000000000000 As Level, ;
	000000000000000 As LevelInt, ;
	a.Updown, a.MainFlg, a.Ac_Id, a.Ac_Group_Id, a.Ac_Name, a.Group, ;
	SUM(a.opBal) As opBal, Sum(a.Debit) As Debit, Sum(Abs(a.Credit)) As Credit, ;
	SUM((a.opBal+a.Debit-Abs(a.Credit))) As ClBal, a.compid ;
	FROM _CTBAcMast a Group By Ac_Name ;
	INTO Cursor _CTBAcMast Readwrite
Set ENGINEBEHAVIOR To 90
******* Changed By Sachin N. S. on 11/07/2009 ******* End


Select _CTBAcMast
Copy To "c:\sachin.Dbf"


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


=Messagebox("SACHIN - MAKEACMAST - END")



Return .T.

Function CloseTmpCursor
***********************
	sql_Con = sqlconobj.SqlConnClose('nHandle')
	If sql_Con < 0
		=Messagebox(Message(),0+16,VuMess)
		Return .T.
	Endif

	Release sqlconobj,nHandle
