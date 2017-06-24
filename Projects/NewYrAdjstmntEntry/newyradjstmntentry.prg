Lparameters nType,cDbName,dSta_Dt,dEnd_Dt,cRights,_etDataSessionId
****Versioning**** && Added By Amrendra for TKT 8121 on 13-06-2011
	LOCAL _VerValidErr,_VerRetVal,_CurrVerVal
	_VerValidErr = ""
	_VerRetVal  = 'NO'
_CurrVerVal='11.1.0.0' &&[VERSIONNUMBER]
	TRY
		_VerRetVal = AppVerChk('NEWYRADJSTMNTENTRY',_CurrVerVal,JUSTFNAME(SYS(16)))
	CATCH TO _VerValidErr
		_VerRetVal  = 'NO'
	Endtry	
	IF TYPE("_VerRetVal")="L"
		cMsgStr="Version Error occured!"
		cMsgStr=cMsgStr+CHR(13)+"Kindly update latest version of "+GlobalObj.getPropertyval("ProductTitle")
		Messagebox(cMsgStr,64,VuMess)
		Return .F.
	ENDIF
	IF _VerRetVal  = 'NO'
		Return .F.
	Endif
****Versioning****
*Local nSession,cAlias			&& Commented by Shrikant S. on 20/05/2010 for TKT-1476


Wait Window "Generating Capital Goods Entry, Please wait... " Nowait
If nType = 2
*!*		nSession = Set("Datasession") && Commented by Shrikant S. on 20/05/2010 for TKT-1476
*!*		cAlias=Alias()					&& Commented by Shrikant S. on 20/05/2010 for TKT-1476
	cDbName = Company.dbName
	dSta_Dt	= Company.Sta_Dt
	dEnd_Dt	= Company.End_Dt
Endif
If nType =1
	Set DataSession To _etDataSessionId
	_CurObject=_Screen.ActiveForm.SqlConObj
	nHandle=_screen.ActiveForm.nHandle
Else
	oSession  = Createobject("session")
	SqlConObj= Newobject("SqlConNudObj","SqlConnection",xapps)
	_etDataSessionId=oSession.DataSessionId
	_CurObject = SqlConObj
	nHandle=0
Endif

lcSqlstr =	" SELECT DISTINCT ENTRY_TY,TRAN_CD FROM PTACDET WHERE AC_NAME like "+;
	" '%CAPITAL GOODS PAYABLE A/C%' AND ENTRY_TY IN ('PT','P1') "+;
	" AND L_YN <> '"+Alltr(Str(Year(dSta_Dt)))+"-"+Alltr(Str(Year(dEnd_Dt)))+"' "+;
	" AND ENTRY_TY+CAST(TRAN_CD AS VARCHAR(10)) NOT IN (SELECT ENTRY_TY+CAST(TRAN_CD AS VARCHAR(10)) FROM NEWYEARTRAN ) "

*!*	nRetval= SqlConObj1.DataConn([EXE],cDbName,lcSqlstr,[_TmpAcDet],"nHandle",oSession.DataSessionId,.F.) && Commented by Shrikant S. on 20/05/2010 for TKT-1476
nRetval=  _CurObject.DataConn([EXE],cDbName,lcSqlstr,[_TmpAcDet],"nHandle",_etDataSessionId,.T.)
If nRetval<0
	Return .F.
Endif

*!*		nRetval= _CurObject.sqlconnclose("nHandle")

If Reccount('_TmpAcDet')<=0
	=Messagebox("No records found to make Credit entry for capital goods.",0+64,vuMess)
&& Commented by Shrikant S. on 20/05/2010 for TKT-1476	 *** Start
*!*		Store Null To oSession,SqlConObj1
*!*		Release oSession,SqlConObj1

*!*		If !Empty(cAlias)
*!*			Select (cAlias)
*!*		Endif
*!*		If nSession>0
*!*			Set DataSession To nSession
*!*		Endif
	Return .F.
&& Commented by Shrikant S. on 20/05/2010 for TKT-1476   *** End
Endif


nRet=.T.
Select _TmpAcDet
Scan
	Select _TmpAcDet
*!*		nRet=GenYearEnd_Entries(_TmpAcDet.Entry_ty,_TmpAcDet.Tran_Cd,SqlConObj1,oSession.DataSessionId,ooHandle,cDbName,dSta_Dt,dEnd_Dt) && Commented by Shrikant S. on 20/05/2010 for TKT-1476
	nRet=GenYearEnd_Entries(_TmpAcDet.Entry_ty,_TmpAcDet.Tran_Cd,_CurObject,_etDataSessionId,"nHandle",cDbName,dSta_Dt,dEnd_Dt)
	If !nRet
		Exit
	Endif
	Select _TmpAcDet
Endscan
If nType =2
	If nRet
		sql_con = _CurObject._SqlCommit("nHandle")
		If sql_con > 0
			sql_con = _CurObject._SqlRollback("nHandle")
		Endif
		=Messagebox("Capital Goods Entry created successfully.",0+64,vuMess)
	Else
		If nHandle > 0
			sql_con = _CurObject._SqlRollback("nHandle")
		Endif
	Endif
	nRetval=_CurObject.sqlconnclose("nHandle")
Endif
&& Commented by Shrikant S. on 20/05/2010 for TKT-1476		** Start
*!*	Store Null To oSession,SqlConObj1 && Commented by Shrikant S.
*!*	Release oSession,SqlConObj1

*!*	If !Empty(cAlias)
*!*		Select (cAlias)
*!*	Endif
*!*	If nSession>0
*!*		Set DataSession To nSession
*!*	Endif
&& Commented by Shrikant S. on 20/05/2010 for TKT-1476		** End
