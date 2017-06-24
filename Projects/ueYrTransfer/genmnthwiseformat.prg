Lparameters dDate,cMnthFormat,oSqlConObj,nDataSessionId,vnHandle,vvcDbName,vvdSta_Dt,vvdEnd_Dt
Local cDate,cRetVal
cDate = Dtos(dDate)
*!*	nHandle = 0  		&& Commented by Shrikant S. on 20/05/2010 for TKT-1476
cMthFormat = Alltrim(cMnthFormat)
mRet=oSqlConObj.dataconn("EXE",vvcdbname,"select * from MonthFormat where MnthFrmt = ?cMthFormat ","_MnthFrmt",vnHandle,nDataSessionId)
If mRet > 0
*!*		mRet=oSqlConObj.SqlConnClose("nHandle") && Commented by Shrikant S. on 20/05/2010 for TKT-1476
	If mRet <= 0
		Return .F.
	Endif
	Sele _MnthFrmt
Endif
Select _MnthFrmt
If Reccount('_MnthFrmt') > 0
	cDtEval = Iif(!Empty(_MnthFrmt.FrmtEval),_MnthFrmt.FrmtEval,'Subs(cDate,5,2)+Subs(cDate,3,2)')
Else
	cDtEval = 'Subs(cDate,5,2)+Subs(cDate,3,2)'
Endif

cRetVal = Evaluate(cDtEval)

Return cRetVal
