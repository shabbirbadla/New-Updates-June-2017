&&This Project was created by Vasant on 31/12/2011 as per Bug 1348 - RG Page No. should generate from start (i.e.) for each financial year
Lparameters cRights
If .F.
****Versioning**** && Added By Amrendra for TKT 8121 on 13-06-2011
	Local _VerValidErr,_VerRetVal,_CurrVerVal
	_VerValidErr = ""
	_VerRetVal  = 'NO'
	_CurrVerVal='11.1.0.0' &&[VERSIONNUMBER]
	Try
		_VerRetVal = AppVerChk('NEWYRADJSTMNTENTRY',_CurrVerVal,Justfname(Sys(16)))
	Catch To _VerValidErr
		_VerRetVal  = 'NO'
	Endtry
	If Type("_VerRetVal")="L"
		cMsgStr="Version Error occured!"
		cMsgStr=cMsgStr+Chr(13)+"Kindly update latest version of "+GlobalObj.getPropertyval("ProductTitle")
		Messagebox(cMsgStr,64,VuMess)
		Return .F.
	Endif
	If _VerRetVal  = 'NO'
		Return .F.
	Endif
****Versioning****
Endif

Wait Window "Checking data, Please wait... " Nowait
If !'vutex' $ vchkprod
	=Messagebox("This utility is for Excise Trading Product Only.",0+64,VuMess)
	=ClProc()
	Return .F.
Endif

cCoName = Company.Co_Name
cDbName = Company.dbName
dSta_Dt	= Company.Sta_Dt
dEnd_Dt	= Company.End_Dt

oSession  = Createobject("session")
SqlConObj= Newobject("SqlConNudObj","SqlConnection",xapps)
_etDataSessionId=oSession.DataSessionId
_CurObject = SqlConObj
nHandle=0

&& Added by Shrikant S. on 01/03/2016 for 21618		&& Start
lcSqlstr="select top 1 Sta_Dt,End_dt,dbName from Vudyog..Co_Mast where Co_name = ?cCoName And Sta_dt > ?dEnd_Dt Order by Sta_dt"		
nRetval=  _CurObject.DataConn([EXE],cDbName,lcSqlstr,[_TmpTblNm],"nHandle",_etDataSessionId,.F.)
If nRetval<0
	=ClProc()
	Return .F.
ENDIF
If Used('_TmpTblNm')
	If Alltrim(cDbName)<>Alltrim(_TmpTblNm.dbName)
		=Messagebox("This option is not available in case of split database.",0+64,VuMess)
		=ClProc()
		Return .F.
	Endif
ENDIF
&& Added by Shrikant S. on 01/03/2016 for 21618		&& End



lcSqlstr="select top 1 rgpg_reset,Invwise,Rgbillg from manufact"
nRetval=  _CurObject.DataConn([EXE],cDbName,lcSqlstr,[_manufact],"nHandle",_etDataSessionId,.F.)
If nRetval<0
	=ClProc()
	Return .F.
Endif
If !_manufact.rgpg_reset
	=Messagebox("This option is only for those who has opted for "+;
		CHR(13)+"generate RG Page for Next Year in Company Master",0+64,VuMess)
	=ClProc()
	Return .F.
Endif

&&Changes done by vasant on 05/06/2012 as per BUG-4432 (Once after creation of new year, if pass the purchase and sale in entry in current year, should not allow to delete DC entries in previous year.)
*!*	lcSqlstr="select top 1 entry_ty from TradeMain where [Rule] = 'EXCISE'"
*!*	lcSqlstr=lcSqlstr+ " and Date > ?dEnd_Dt"
*!*	nRetval=  _CurObject.DataConn([EXE],cDbName,lcSqlstr,[_TmpTblNm],"nHandle",_etDataSessionId,.f.)
*!*	If nRetval<0
*!*		=ClProc()
*!*		Return .F.
*!*	ENDIF
*!*	IF RECCOUNT('_TmpTblNm') > 0
*!*		=Messagebox("Transaction done for Next Year, can't generate RG Page for Next Year.",0+64,vuMess)
*!*		=ClProc()
*!*		Return .F.
*!*	Endif
&&Changes done by vasant on 05/06/2012 as per BUG-4432 (Once after creation of new year, if pass the purchase and sale in entry in current year, should not allow to delete DC entries in previous year.)

mYesNo = 0
mYesNo	= Messagebox("Proceed with generation of RG Page for Next Year of Current Year Transactions.",4+32,VuMess)
If mYesNo != 6
	=ClProc()
	Return .F.
Endif


lcSqlstr="select top 1 Sta_Dt,End_dt from Vudyog..Co_Mast where Co_name = ?cCoName And Dbname = ?cDbName And Sta_dt > ?dEnd_Dt Order by Sta_dt"			
nRetval=  _CurObject.DataConn([EXE],cDbName,lcSqlstr,[_TmpTblNm],"nHandle",_etDataSessionId,.F.)
If nRetval<0
	=ClProc()
	Return .F.
ENDIF


If Reccount('_TmpTblNm') = 0
	=Messagebox("No New Year Records Found of this Company.",0+64,VuMess)
	=ClProc()
	Return .F.
Else
	dNSta_Dt	= _TmpTblNm.Sta_Dt
	dNEnd_Dt	= _TmpTblNm.End_Dt
Endif

*!*	lcSqlstr="select a.tran_cd from TradeItem a,TradeMain b where a.Entry_ty = b.Entry_ty and a.Tran_cd = b.Tran_cd "
*!*	lcSqlstr=lcSqlstr+" and b.[Rule] = 'EXCISE' and b.date >= ?dNSta_Dt"
*!*	lcSqlstr=lcSqlstr+" and (c.Entry_ty in ('DC','SS') or c.bCode_nm in ('DC','SS') or (c.Entry_ty = 'GT' and b.U_sinfo = 1) or (c.bCode_nm = 'GT' and b.U_sinfo = 1))"
*!*	nRetval=  _CurObject.DataConn([EXE],cDbName,lcSqlstr,[_TmpTblNm],"nHandle",_etDataSessionId,.f.)
*!*	If nRetval<0
*!*		=ClProc()
*!*		Return .F.
*!*	ENDIF
*!*	IF RECCOUNT('_TmpTblNm') > 0
*!*		=Messagebox("Transaction done for Next Year, can't generate RG Page for Next Year.",0+64,vuMess)
*!*		=ClProc()
*!*		Return .F.
*!*	Endif

lcSqlstr="select top 1 entry_ty from Gen_SrNo where Sta_dt = ?dNSta_Dt"
nRetval=  _CurObject.DataConn([EXE],cDbName,lcSqlstr,[_TmpTblNm],"nHandle",_etDataSessionId,.F.)
If nRetval<0
	=ClProc()
	Return .F.
Endif
If Reccount('_TmpTblNm') > 0
	mYesNo = 0
	mYesNo	= Messagebox("Already RG Page for Next Year of Current Year Transactions generated,"+;
		CHR(13)+"Re-generate again",4+32,VuMess)
	If mYesNo != 6
		=ClProc()
		Return .F.
	Endif
Endif

Wait Window "Generating RG Page for Next Year for Current Year Transactions, Please wait... " Nowait

*!*	lcSqlstr="select b.entry_ty,b.date,b.tran_cd,b.inv_no,a.itserial,a.ware_nm,a.RgPage from TradeItem a,TradeMain b,Lcode c where a.Entry_ty = b.Entry_ty and a.Tran_cd = b.Tran_cd and b.entry_ty = c.entry_ty"
*!*	lcSqlstr=lcSqlstr+" and a.BalQty > 0 and b.[Rule] = 'EXCISE' and b.Date <= ?dEnd_Dt"
*!*	lcSqlstr=lcSqlstr+" and (c.Entry_ty in ('AR','IR') or c.bCode_nm in ('AR','IR') or (c.Entry_ty = 'GT' and b.U_sinfo = 0) or (c.bCode_nm = 'GT' and b.U_sinfo = 0))"
&&Changes done by vasant on 05/06/2012 as per BUG-4432 (Once after creation of new year, if pass the purchase and sale in entry in current year, should not allow to delete DC entries in previous year.)
*!*	lcSqlstr="select b.entry_ty,b.date,b.tran_cd,b.inv_no,a.itserial,a.ware_nm,a.RgPage from TradeItem a,TradeMain b,Lcode c where a.Entry_ty = b.Entry_ty and a.Tran_cd = b.Tran_cd and b.entry_ty = c.entry_ty"
*!*	lcSqlstr=lcSqlstr+" and a.BalQty > 0 and b.[Rule] = 'EXCISE'"
*!*	lcSqlstr="select b.entry_ty,b.date,b.tran_cd,b.inv_no,a.itserial,a.ware_nm,a.RgPage from TradeItem a,TradeMain b,Lcode c where a.Entry_ty = b.Entry_ty and a.Tran_cd = b.Tran_cd and b.entry_ty = c.entry_ty"	&& Commented By Shrikant S. on 07/01/2014 for Bug-21127
lcSqlstr="select b.entry_ty,b.date,b.tran_cd,b.inv_no,a.itserial,a.ware_nm,a.RgPage,a.balqty from TradeItem a,TradeMain b,Lcode c where a.Entry_ty = b.Entry_ty and a.Tran_cd = b.Tran_cd and b.entry_ty = c.entry_ty"			&& Added By Shrikant S. on 07/01/2014 for Bug-21127
lcSqlstr=lcSqlstr+" and a.BalQty > 0 and b.[Rule] = 'EXCISE' and b.date < ?dNSta_Dt"
&& Added By Shrikant S. on 07/01/2014 for Bug-21127		&& Start
lcSqlstr=lcSqlstr+" union all "
lcSqlstr=lcSqlstr+" select b.entry_ty,b.date,b.tran_cd,b.inv_no,a.itserial,a.ware_nm,a.RgPage,a.balqty from TradeItem a,TradeMain b,Lcode c where a.Entry_ty = b.Entry_ty and a.Tran_cd = b.Tran_cd and b.entry_ty = c.entry_ty"
lcSqlstr=lcSqlstr+" and a.BalQty <= 0 and b.[Rule] = 'EXCISE' and b.date < ?dNSta_Dt "
lcSqlstr=lcSqlstr+" and a.entry_ty+STR(a.tran_cd)+a.itserial  in (select entry_ty+STR(tran_cd)+itserial from GEN_SRNO where sta_dt =?dNSta_Dt)"
&& Added By Shrikant S. on 07/01/2014 for Bug-21127		&& End

&&Changes done by vasant on 05/06/2012 as per BUG-4432 (Once after creation of new year, if pass the purchase and sale in entry in current year, should not allow to delete DC entries in previous year.)
lcSqlstr=lcSqlstr+" and (c.Entry_ty in ('AR','IR') or c.bCode_nm in ('AR','IR') or (c.Entry_ty = 'GT' and b.U_sinfo = 0) or (c.bCode_nm = 'GT' and b.U_sinfo = 0))"
nRetval=  _CurObject.DataConn([EXE],cDbName,lcSqlstr,[_TmpTblNm],"nHandle",_etDataSessionId,.F.)
If nRetval<0
	=ClProc()
	Return .F.
Endif

&&Changes done by vasant on 05/06/2012 as per BUG-4432 (Once after creation of new year, if pass the purchase and sale in entry in current year, should not allow to delete DC entries in previous year.)
Select _TmpTblNm
Scan

	lcSqlstr="Select Top 1 a.Entry_ty from Litemall a,TradeMain b where a.entry_ty = b.entry_ty and a.tran_cd = b.tran_cd "
	lcSqlstr= lcSqlstr +  " and a.Pentry_ty = ?_TmpTblNm.Entry_ty and a.Ptran_cd = ?_TmpTblNm.Tran_cd and a.Pitserial = ?_TmpTblNm.ItSerial "
	lcSqlstr= lcSqlstr +  " and b.date >= ?dNSta_Dt"
	nRetval=  _CurObject.DataConn([EXE],cDbName,lcSqlstr,[_TmpTblNm1],"nHandle",_etDataSessionId,.T.)
	If nRetval<0
		=ClProc()
		Return .F.
	Endif
	Select _TmpTblNm1
	If Reccount() =  0
		lcSqlstr="Delete from Gen_SrNo where Sta_dt = ?dNSta_Dt"
		lcSqlstr= lcSqlstr +  " and Entry_ty = ?_TmpTblNm.Entry_ty and Tran_cd = ?_TmpTblNm.Tran_cd and Itserial = ?_TmpTblNm.ItSerial "
		nRetval=  _CurObject.DataConn([EXE],cDbName,lcSqlstr,[],"nHandle",_etDataSessionId,.T.)
		If nRetval<0
			=ClProc()
			Return .F.
		Endif
	Endif

	Select _TmpTblNm
Endscan

lcSqlstr="Select * from Gen_SrNo where Sta_dt = ?dNSta_Dt"
nRetval=  _CurObject.DataConn([EXE],cDbName,lcSqlstr,[_TmpTblNm1],"nHandle",_etDataSessionId,.T.)
If nRetval<0
	=ClProc()
	Return .F.
Endif
Select _TmpTblNm1
Index On 'A*'+Alltrim(NPgNo)+'*A' Tag NPgNo
Index On Alltrim(CWare) + 'A*'+Entry_ty+Str(Tran_cd) Tag WareET
Index On Alltrim(CWare) + 'A*'+Alltrim(NPgNo)+'*A' Tag WarePgNo

*!*	IF RECCOUNT('_TmpTblNm') = 0
*!*		lcSqlstr="Delete from Gen_SrNo where Sta_dt = ?dNSta_Dt"
*!*		nRetval=  _CurObject.DataConn([EXE],cDbName,lcSqlstr,[],"nHandle",_etDataSessionId,.f.)
*!*		=Messagebox("No Pending Records for Re-generation of RG Page No.",0+64,vuMess)
*!*		=ClProc()
*!*		Return .F.
*!*	Endif
&&Changes done by vasant on 05/06/2012 as per BUG-4432 (Once after creation of new year, if pass the purchase and sale in entry in current year, should not allow to delete DC entries in previous year.)

Select _TmpTblNm
Delete All For balqty<=0		&& Added by Shrikant S. on 07/01/2014 for Bug-21127
Set Deleted On		&& Added by Shrikant S. on 07/01/2014 for Bug-21127

Replace All RgPage With '' In _TmpTblNm
&&Changes done by vasant on 05/06/2012 as per BUG-4432 (Once after creation of new year, if pass the purchase and sale in entry in current year, should not allow to delete DC entries in previous year.)
_SkipString = 'A*~*A'
Update _TmpTblNm Set RgPage = _SkipString From _TmpTblNm,_TmpTblNm1 Where ;
	_TmpTblNm.Entry_ty = _TmpTblNm1.Entry_ty And _TmpTblNm.Tran_cd = _TmpTblNm1.Tran_cd And _TmpTblNm.ItSerial = _TmpTblNm1.ItSerial
&&Changes done by vasant on 05/06/2012 as per BUG-4432 (Once after creation of new year, if pass the purchase and sale in entry in current year, should not allow to delete DC entries in previous year.)

Do Case
Case !_manufact.Invwise And !_manufact.Rgbillg
	Select _TmpTblNm
	Index On Ware_nm+Dtos(Date)+Entry_ty+Inv_no+Str(Tran_cd)+ItSerial Tag DEII
	Go Top
	Do While !Eof()
		MrgPage = ''
		vrgpage = 0
		vgodown = _TmpTblNm.Ware_nm
		Do While Ware_nm = vgodown And !Eof()

&&Changes done by vasant on 05/06/2012 as per BUG-4432 (Once after creation of new year, if pass the purchase and sale in entry in current year, should not allow to delete DC entries in previous year.)
*!*				vrgpage = vrgpage + 1
*!*				MrgPage = ALLTRIM(STR(vrgpage))
*!*				REPLACE RgPage WITH MrgPage IN _TmpTblNm
			If Alltrim(_TmpTblNm.RgPage) == _SkipString
				Skip
				Loop
			Endif
			Do While .T.
				vrgpage = vrgpage + 1
				MrgPage = Alltrim(Str(vrgpage))
				If !Seek(Alltrim(vgodown) + 'A*'+MrgPage+'*A','_TmpTblNm1','WarePgNo')
					Exit
				Endif
			Enddo
			Replace RgPage With MrgPage In _TmpTblNm
&&Changes done by vasant on 05/06/2012 as per BUG-4432 (Once after creation of new year, if pass the purchase and sale in entry in current year, should not allow to delete DC entries in previous year.)

			Select _TmpTblNm
			If !Eof()
				Skip
			Endif
		Enddo
	Enddo

Case _manufact.Invwise
	Select _TmpTblNm
	Index On Ware_nm+Dtos(Date)+Entry_ty+Inv_no+Str(Tran_cd)+ItSerial Tag DEII
	Go Top
	Do While !Eof()
		MrgPage = ''
		vrgpage = 0
		vgodown = _TmpTblNm.Ware_nm
		Do While Ware_nm = vgodown And !Eof()
&&Changes done by vasant on 05/06/2012 as per BUG-4432 (Once after creation of new year, if pass the purchase and sale in entry in current year, should not allow to delete DC entries in previous year.)
*!*				vrgpage = vrgpage + 1
*!*				lrgpage = 0
*!*				vtran_det = _TmpTblNm.entry_ty+STR(_TmpTblNm.tran_cd)
			If Alltrim(_TmpTblNm.RgPage) == _SkipString
				Skip
				Loop
			Endif
			vtran_det = _TmpTblNm.Entry_ty+Str(_TmpTblNm.Tran_cd)
			If !Seek(Alltrim(vgodown)+ 'A*'+vtran_det,'_TmpTblNm1','WareET')
				vrgpage = vrgpage + 1
			Else
				vrgpage = Val(Substr(_TmpTblNm.NPgNo,1,At('/',_TmpTblNm.NPgNo) - 1))
			Endif
			lrgpage = 0
&&Changes done by vasant on 05/06/2012 as per BUG-4432 (Once after creation of new year, if pass the purchase and sale in entry in current year, should not allow to delete DC entries in previous year.)
			Do While Ware_nm = vgodown And Entry_ty+Str(Tran_cd) = vtran_det And !Eof()
&&Changes done by vasant on 05/06/2012 as per BUG-4432 (Once after creation of new year, if pass the purchase and sale in entry in current year, should not allow to delete DC entries in previous year.)
*!*					lrgpage = lrgpage + 1
*!*					MrgPage = Padr(Allt(Str(vrgpage))+'/'+Allt(Str(lrgpage)),10,' ')
*!*					REPLACE RgPage WITH MrgPage IN _TmpTblNm
				If Alltrim(_TmpTblNm.RgPage) == _SkipString
					Skip
					Loop
				Endif
				Do While .T.
					lrgpage = lrgpage + 1
					MrgPage = Padr(Allt(Str(vrgpage))+'/'+Allt(Str(lrgpage)),10,' ')
					If !Seek(Alltrim(vgodown) + 'A*'+MrgPage+'*A','_TmpTblNm1','WarePgNo')
						Exit
					Endif
				Enddo
				Replace RgPage With MrgPage In _TmpTblNm
&&Changes done by vasant on 05/06/2012 as per BUG-4432 (Once after creation of new year, if pass the purchase and sale in entry in current year, should not allow to delete DC entries in previous year.)

				Select _TmpTblNm
				If !Eof()
					Skip
				Endif
			Enddo
		Enddo
	Enddo

Case _manufact.Rgbillg
	MrgPage = ''
	vrgpage = 0

	Select _TmpTblNm
	Index On Dtos(Date)+Entry_ty+Inv_no+Str(Tran_cd)+ItSerial Tag DEII
	Go Top
	Do While !Eof()

&&Changes done by vasant on 05/06/2012 as per BUG-4432 (Once after creation of new year, if pass the purchase and sale in entry in current year, should not allow to delete DC entries in previous year.)
*!*			vrgpage = vrgpage + 1
*!*			MrgPage = ALLTRIM(STR(vrgpage))
*!*			REPLACE RgPage WITH MrgPage IN _TmpTblNm
		If Alltrim(_TmpTblNm.RgPage) == _SkipString
			Skip
			Loop
		Endif
		Do While .T.
			vrgpage = vrgpage + 1
			MrgPage = Alltrim(Str(vrgpage))
			If !Seek('A*'+MrgPage+'*A','_TmpTblNm1','NPgNo')
				Exit
			Endif
		Enddo
		Replace RgPage With MrgPage In _TmpTblNm
&&Changes done by vasant on 05/06/2012 as per BUG-4432 (Once after creation of new year, if pass the purchase and sale in entry in current year, should not allow to delete DC entries in previous year.)

		Select _TmpTblNm
		If !Eof()
			Skip
		Endif
	Enddo

Endcase

&&Changes done by vasant on 05/06/2012 as per BUG-4432 (Once after creation of new year, if pass the purchase and sale in entry in current year, should not allow to delete DC entries in previous year.)
*!*	lcSqlstr="Delete from Gen_SrNo where Sta_dt = ?dNSta_Dt"
*!*	nRetval=  _CurObject.DataConn([EXE],cDbName,lcSqlstr,[],"nHandle",_etDataSessionId,.t.)
*!*	If nRetval<0
*!*		=ClProc()
*!*		Return .F.
*!*	ENDIF
&&Changes done by vasant on 05/06/2012 as per BUG-4432 (Once after creation of new year, if pass the purchase and sale in entry in current year, should not allow to delete DC entries in previous year.)

Select _TmpTblNm
Go Top
Do While !Eof()
&&Changes done by vasant on 05/06/2012 as per BUG-4432 (Once after creation of new year, if pass the purchase and sale in entry in current year, should not allow to delete DC entries in previous year.)
	If Alltrim(_TmpTblNm.RgPage) == _SkipString
		Skip
		Loop
	Endif
&&Changes done by vasant on 05/06/2012 as per BUG-4432 (Once after creation of new year, if pass the purchase and sale in entry in current year, should not allow to delete DC entries in previous year.)

	lcSqlstr="Select Top 1 * from Gen_SrNo where Entry_ty = ?_TmpTblNm.Entry_ty and Tran_cd = ?_TmpTblNm.Tran_cd and ItSerial = ?_TmpTblNm.ItSerial"
	nRetval=  _CurObject.DataConn([EXE],cDbName,lcSqlstr,[tmpGen_SrNo],"nHandle",_etDataSessionId,.T.)
	If nRetval<0
		=ClProc()
		Return .F.
	Endif

	Select tmpGen_SrNo
	If Reccount() > 0
		Replace NPgNo With _TmpTblNm.RgPage,;
			Sta_Dt With dNSta_Dt, End_Dt With dNEnd_Dt In tmpGen_SrNo

		lcSqlstr= _CurObject.GenInsert("Gen_SrNo","","","tmpGen_SrNo",mvu_backend)
		nRetval = _CurObject.DataConn([EXE],cDbName,lcSqlstr,[],"nHandle",_etDataSessionId,.T.)
		If nRetval<0
			=ClProc()
			Return .F.
		Endif
	Endif

	Select _TmpTblNm
	If !Eof()
		Skip
	Endif
Enddo

If nRetval > 0
	sql_con = _CurObject._SqlCommit("nHandle")
	If sql_con > 0
		sql_con = _CurObject._SqlRollback("nHandle")
	Endif
	=Messagebox("Generation of RG Page for Next Year of Current Year Transactions was Successfull.",0+64,VuMess)
Else
	If nHandle > 0
		sql_con = _CurObject._SqlRollback("nHandle")
	Endif
Endif
nRetval=_CurObject.sqlconnclose("nHandle")

=ClProc()


Procedure ClProc
nRetval=_CurObject.sqlconnclose("nHandle")
If Used('_manufact')
	Use In _manufact
Endif
If Used('_TmpTblNm')
	Use In _TmpTblNm
Endif
If Used('tmpGen_SrNo')
	Use In tmpGen_SrNo
Endif
&&Changes done by vasant on 05/06/2012 as per BUG-4432 (Once after creation of new year, if pass the purchase and sale in entry in current year, should not allow to delete DC entries in previous year.)
If Used('_TmpTblNm1')
	Use In _TmpTblNm1
Endif
&&Changes done by vasant on 05/06/2012 as per BUG-4432 (Once after creation of new year, if pass the purchase and sale in entry in current year, should not allow to delete DC entries in previous year.)

