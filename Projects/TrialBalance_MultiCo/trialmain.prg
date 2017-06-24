*:*****************************************************************************
*:        Program: TrialMain
*:         System: Udyog Software
*:         Author: RAGHU
*:  Last modified: 27/04/2006
*:			AIM  : Trial Balance Or Group Summary Report In ZOOM IN
*:			Use	 : =TrialMain(FromDate,ToDate,'T','')	&& Trial Balance
*:				 : =TrialMain(FromDate,ToDate,'B','')	&& Balance Sheet
*:				 : =TrialMain(FromDate,ToDate,'P','')	&& Profit And Loss Account
*:				 : =TrialMain(FromDate,ToDate,'G',13)	&& Group Summary
*:*****************************************************************************
Parameters mFromDt,mTodate,mReportType,GCode,sqldatasession
*!*	mFromDt		 : From Date
*!*	mTodate		 : To Date
*!*	mReportType	 : 'T' For Trial Balanace 'G' - Group Summary
*!*	GCode		 : Group Code
*!*	sqldatasession : Datasession

If Parameters() <> 5
	=Messagebox('Pass Valid Parameters',64,vuMess)
	Return .T.
Endif



SET STEP ON 



=MESSAGEBOX("SACHIN - TRIALMAIN - START")


If !("GRIDFIND.VCX" $ Upper(Set("Classlib")))
	Set Classlib To gridfind.vcx Additive
Endif
If !("CONFA.VCX" $ Upper(Set("Classlib")))
	Set Classlib To confa.vcx Additive
Endif


Local lnI
Set Talk Off
Set StrictDate To 0
Set Deleted On
mFromDt = Ttod(mFromDt)
mTodate = Ttod(mTodate)

llRet = MakeAcmast(mFromDt,mTodate,sqldatasession,mReportType)		&& Make Accounts Master
If llRet = .F.
	Return .F.
Endif
lnI=MakeLevel(mReportType)												&& Make Level
If Type('Statdesktop') = 'O'
	Statdesktop.ProgressBar.Value = 80
ENDIF

cAlias=ALIAS()

=MESSAGEBOX(" Sachin - 545234465 ")




SELECT _TBAcMast
COPY TO "c:\Sachin1.dbf"
GO top



=MESSAGEBOX(" Sachin - 65432135465 ")


SELECT (cAlias)


=MESSAGEBOX(" Sachin - 111111 ")


Do Form finalacc With mReportType,GCode,mFromDt,mTodate,sqldatasession,lnI


=MESSAGEBOX(" Sachin - 222222 ")


If Type('Statdesktop') = 'O'
	Statdesktop.ProgressBar.Value = 100
	Statdesktop.ProgressBar.Visible = .F.
Endif

=MESSAGEBOX("SACHIN - TRIALMAIN - END")