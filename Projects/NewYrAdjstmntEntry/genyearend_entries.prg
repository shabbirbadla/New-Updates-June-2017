Lparameters cEntry_ty,nTran_Cd,SqlConObj,_ndatasessionid,oHandle,ccDbName,cdSta_Dt,cdEnd_Dt

Local nHandle, lcSqlStr, nRetVal

nHandle=0
lcSqlStr =	" SELECT * FROM LCODE WHERE ENTRY_TY = '"+cEntry_ty+"' "
*!*	nRetVal= SqlConObj.DataConn([EXE],ccDbName,lcSqlStr,[Tmp_Lcode],"nHandle",_ndatasessionid,.F.) && Commented by Shrikant S. on 20/05/2010 for TKT-1476
nRetVal= SqlConObj.DataConn([EXE],ccDbName,lcSqlStr,[Tmp_Lcode],oHandle,_ndatasessionid,.T.)
If nRetVal<0
	Return .F.
Endif

SELECT Tmp_Lcode
mEnTry_ty=IIF(!EMPTY(Tmp_Lcode.Bcode_nm),Tmp_Lcode.Bcode_nm,cEntry_ty) && Added by Shrikant S. 
lcSqlStr =	" SELECT * FROM "+mEnTry_ty+"MAIN WHERE ENTRY_TY = '"+cEntry_ty+"' AND "+;
	" TRAN_CD = "+Transform(nTran_Cd)
*!*	nRetVal= SqlConObj.DataConn([EXE],ccDbName,lcSqlStr,[Tmp_Main1],"nHandle",_ndatasessionid,.F.) && Commented by Shrikant S. on 20/05/2010 for TKT-1476
nRetVal= SqlConObj.DataConn([EXE],ccDbName,lcSqlStr,[Tmp_Main1],oHandle,_ndatasessionid,.T.)
If nRetVal<0
	Return .F.
Endif

lcSqlStr =	" SELECT * FROM "+mEnTry_ty+"ITEM WHERE ENTRY_TY = '"+cEntry_ty+"' AND "+;
	" TRAN_CD = "+Transform(nTran_Cd)
*!*	nRetVal= SqlConObj.DataConn([EXE],ccDbName,lcSqlStr,[Tmp_Item1],"nHandle",_ndatasessionid,.F.) && Commented by Shrikant S. on 20/05/2010 for TKT-1476
nRetVal= SqlConObj.DataConn([EXE],ccDbName,lcSqlStr,[Tmp_Item1],oHandle,_ndatasessionid,.T.)
If nRetVal<0
	Return .F.
Endif

&&&ADDED BY SATISH PAL DT.06/07/2012-STRAT
lcSqlStr =	" SELECT * FROM gen_srno WHERE ENTRY_TY = '"+cEntry_ty+"' AND CTYPE='RGCPART2' AND "+;
	" TRAN_CD = "+Transform(nTran_Cd)

*!*	nRetVal= SqlConObj.DataConn([EXE],ccDbName,lcSqlStr,[Tmp_Item1],"nHandle",_ndatasessionid,.F.) && Commented by Shrikant S. on 20/05/2010 for TKT-1476
nRetVal= SqlConObj.DataConn([EXE],ccDbName,lcSqlStr,[Tmp_gen_srno1],oHandle,_ndatasessionid,.T.)
If nRetVal<0
	Return .F.
Endif
&&ADDED BY SATISH PAL DT.06/07/2012-END

lcSqlStr =	" SELECT * FROM "+mEnTry_ty+"ACDET WHERE ENTRY_TY = '"+cEntry_ty+"' AND "+;
	" TRAN_CD = "+Transform(nTran_Cd)
*!*	nRetVal= SqlConObj.DataConn([EXE],ccDbName,lcSqlStr,[Tmp_AcDet1],"nHandle",_ndatasessionid,.F.) && Commented by Shrikant S. on 20/05/2010 for TKT-1476
nRetVal= SqlConObj.DataConn([EXE],ccDbName,lcSqlStr,[Tmp_AcDet1],oHandle,_ndatasessionid,.T.)
If nRetVal<0
	Return .F.
Endif

lcSqlStr =	" SELECT * FROM DCMAST WHERE ENTRY_TY = 'HR' AND "+;
	" CODE = 'E' "
*!*	nRetVal= SqlConObj.DataConn([EXE],ccDbName,lcSqlStr,[Tmp_DcMast],"nHandle",_ndatasessionid,.F.) && Commented by Shrikant S. on 20/05/2010 for TKT-1476
nRetVal= SqlConObj.DataConn([EXE],ccDbName,lcSqlStr,[Tmp_DcMast],oHandle,_ndatasessionid,.T.) 
If nRetVal<0
	Return .F.
Endif

lcSqlStr =	" SELECT GETDATE() as SysDate "
*!*	nRetVal= SqlConObj.DataConn([EXE],ccDbName,lcSqlStr,[_TmpSysDate],"nHandle",_ndatasessionid,.F.) && Commented by Shrikant S. on 20/05/2010 for TKT-1476
nRetVal= SqlConObj.DataConn([EXE],ccDbName,lcSqlStr,[_TmpSysDate],oHandle,_ndatasessionid,.T.)
If nRetVal<0
	Return .F.
Endif
cSysDate = Ttoc(_TmpSysDate.SysDate)


*!*	nRetVal= SqlConObj.sqlconnclose("nHandle")		&& Commented by Shrikant S. on 20/05/2010 for TKT-1476

****************
lcSqlStr =	" SELECT * FROM IRMAIN WHERE 1=2 "
nRetVal= SqlConObj.DataConn([EXE],ccDbName,lcSqlStr,[Tmp_Main],oHandle,_ndatasessionid,.T.)
*!*	MESSAGEBOX(nRetVal)
If nRetVal<0
	Return .F.
Endif

lcSqlStr =	" SELECT * FROM IRITEM WHERE 1=2 "
nRetVal= SqlConObj.DataConn([EXE],ccDbName,lcSqlStr,[Tmp_Item],oHandle,_ndatasessionid,.T.)
If nRetVal<0
	Return .F.
Endif

lcSqlStr =	" SELECT * FROM IRACDET WHERE 1=2 "
nRetVal= SqlConObj.DataConn([EXE],ccDbName,lcSqlStr,[Tmp_AcDet],oHandle,_ndatasessionid,.T.)
If nRetVal<0
	Return .F.
Endif

*****ADDED BY SATISH PAL FOR BUG-4996 DT.06/07/2012
lcSqlStr =	" SELECT * FROM gen_srno WHERE 1=2 "
nRetVal= SqlConObj.DataConn([EXE],ccDbName,lcSqlStr,[Tmp_gen_srno],oHandle,_ndatasessionid,.T.)
If nRetVal<0
	Return .F.
Endif
*****END BY SATISH PAL

Select Tmp_Main
Append From Dbf('Tmp_Main1')

Select Tmp_Item
Append From Dbf('Tmp_Item1')

Select Tmp_AcDet
Append From Dbf('Tmp_AcDet1')

&&&added by satish pal FOR BUG-4996 dt.06/072012-start

Select Tmp_gen_srno
Append From Dbf('Tmp_gen_srno1')


&&&added by satish pal FOR BUG-4996 dt.06/072012--end

***************

cEntry_ty1 = 'HR'
sql_main = "IRMain"
cdocno=GenerateDocNo(cEntry_ty1,cdSta_Dt,'IR',SqlConObj,_ndatasessionid,oHandle,ccDbName,cdSta_Dt,cdEnd_Dt)
cInvNo=GenerateInvNo(cEntry_ty1, Tmp_Main.inv_sr, '', cdSta_Dt, '', '',Tmp_Lcode.invno_size,SqlConObj,_ndatasessionid,'IR',oHandle,ccDbName,cdSta_Dt,cdEnd_Dt)
cPgNo =GenPageNo("U_RG23CNO","STKL_VW_MAIN",SqlConObj,oHandle,_ndatasessionid,ccDbName,cdSta_Dt,cdEnd_Dt)

lcSqlStr = "Select ac_id from ac_mast where ac_name = 'EXCISE CAPITAL GOODS PAYABLE A/C'"
*!*	nRetVal= SqlConObj.DataConn([EXE],ccdbname,lcSqlStr,[_Tmp_acid],"nHandle",_ndatasessionid,.F.) && Commented by Shrikant S. on 20/05/2010 for TKT-1476
nRetVal= SqlConObj.DataConn([EXE],ccdbname,lcSqlStr,[_Tmp_acid],oHandle,_ndatasessionid,.T.)
If nRetVal<0
	Return .F.
Endif
*!*	nRetVal= SqlConObj.sqlconnclose("nHandle")
nAc_Id = _Tmp_acid.ac_id

Select Tmp_Main
Replace entry_ty With cEntry_ty1, Tran_cd With 0, Date With cdSta_Dt, Doc_no With cdocno, Inv_no With cInvNo, ;
	Lock With .F., l_yn With Alltr(Str(Year(cdSta_Dt)))+"-"+Alltr(Str(Year(cdEnd_Dt))), ;
	party_nm With 'EXCISE CAPITAL GOODS PAYABLE A/C', ac_id With nAc_Id, U_RG23CNO With cPgNo, u_cldt With cdSta_Dt, ;
	SysDate With cSysDate, user_name With musername, apgentime With cSysDate, apledtime With cSysDate, ;
	rule With '', u_cessper With 0, u_hcessper With 0, u_cvdper With 0, bcdper With 0, gro_amt With 0, tot_add With 0, ;
	net_amt With 0, examt With 0, u_cessamt With 0, u_hcesamt With 0, u_cvdamt With 0, bcdamt With 0, u_plasr WITH ' ' ; && added following by amrendra for TKT7434 on 29/04/2011 ", u_plasr WITH ' '"
	IN Tmp_Main

lcSqlStr = SqlConObj.GenInsert(sql_main,"'Tran_cd'","","Tmp_Main",1)
nRetVal= SqlConObj.DataConn([EXE],ccdbname,lcSqlStr,[],oHandle,_ndatasessionid,.T.)
If nRetVal<0
	Return .F.
Endif
mtran_cd=0
If nRetVal > 0 And mtran_cd <= 0
	sql_main = 'IRMain'
	nRetVal = SqlConObj.DataConn([EXE],ccdbname,[Select top 1 Tran_cd From ]+sql_main+[ where ;
			Entry_ty = ?Tmp_Main.Entry_ty And Date = ?Tmp_Main.Date And Doc_no = ?Tmp_Main.Doc_no],[tmptbl_vw],;
		oHandle,_ndatasessionid,.T.)
	If nRetVal > 0 And Used('tmptbl_vw')
		Select tmptbl_vw
		If Reccount() > 0
			mtran_cd = tmptbl_vw.Tran_cd
		Endif
	Endif
Endif
If mtran_cd<=0
	Return .F.
Endif
Replace Tran_cd With mtran_cd In Tmp_Main

*********** Updating Account Details
nAmount = 0
nExAmt	= 0
nCessAmt= 0
nHCesAmt= 0
nCVDAmt = 0
nBCDAmt = 0
nMnExAmt  = 0
nMnCessAmt= 0
nMnHCesAmt= 0
nMnCVDAmt = 0
nMnBCDAmt = 0

Select Tmp_AcDet
Go Top
Locate For ac_name = 'EXCISE CAPITAL GOODS PAYABLE A/C'
If Found()
	Replace amt_ty With 'CR' In Tmp_AcDet
	nExAmt	= Tmp_AcDet.Amount
	nMnExAmt= Tmp_Main1.examt + Tmp_Main1.U_RG23CPAY
	nAmount = nAmount + Tmp_AcDet.Amount
	Replace Amount With nExAmt For ac_name='BALANCE WITH EXCISE RG23C' In Tmp_AcDet
Endif
Select Tmp_AcDet
Go Top
Locate For ac_name = 'CESS CAPITAL GOODS PAYABLE A/C'
If Found()
	Replace amt_ty With 'CR' In Tmp_AcDet
	nCessAmt = Tmp_AcDet.Amount
	nMnCessAmt = Tmp_Main1.u_cessamt + Tmp_Main1.U_RGCESPAY
	nAmount = nAmount + Tmp_AcDet.Amount
	Replace Amount With nCessAmt For ac_name='BALANCE WITH CESS SURCHARGE RG23C' In Tmp_AcDet
Endif

Select Tmp_AcDet
Go Top
Locate For ac_name = 'CVD CAPITAL GOODS PAYABLE A/C'
If Found()
	Replace amt_ty With 'CR' In Tmp_AcDet
	nCVDAmt = Tmp_AcDet.Amount
	nMnCVDAmt = Tmp_Main1.u_cvdamt + Tmp_Main1.U_CVDPAY
	nAmount = nAmount + Tmp_AcDet.Amount
	Replace Amount With nCVDAmt For ac_name='BALANCE WITH CVD RG23C' In Tmp_AcDet
Endif

Select Tmp_AcDet
Go Top
Locate For ac_name = 'H CESS CAPITAL GOODS PAYABLE A/C'
If Found()
	Replace amt_ty With 'CR' In Tmp_AcDet
	nHCesAmt = Tmp_AcDet.Amount
	nMnHCesAmt = Tmp_Main1.u_hcesamt + Tmp_Main1.U_HCESPAY
	nAmount = nAmount + Tmp_AcDet.Amount
	Replace Amount With nHCesAmt For ac_name='BALANCE WITH HCESS RG23C' In Tmp_AcDet
Endif

Select Tmp_AcDet
Go Top
Locate For ac_name = 'BCD CAPITAL GOODS PAYABLE A/C'
If Found()
	Replace amt_ty With 'CR' In Tmp_AcDet
	nBCDAmt = Tmp_AcDet.Amount
	nMnBCDAmt = Tmp_Main1.bcdamt + Tmp_Main1.BCDPAY
	nAmount = nAmount + Tmp_AcDet.Amount
	Replace Amount With nBCDAmt For ac_name='BALANCE WITH BCD RG23C' In Tmp_AcDet
Endif
*********** Updating Account Details

nExAmt1	 = nExAmt
nCessAmt1= nCessAmt
nHCesAmt1= nHCesAmt
nCVDAmt1 = nCVDAmt
nBCDAmt1 = nBCDAmt


Select Tmp_Item
nRecCnt=Reccount()
Replace All Tran_cd With mtran_cd, entry_ty With Tmp_Main.entry_ty, ;
	Date With Tmp_Main.Date, Doc_no With Tmp_Main.Doc_no, Inv_no With Tmp_Main.Inv_no, ;
	re_qty With 0, l_yn With Tmp_Main.l_yn, pmkey With '', u_pageno With '', ;
	examt With Iif(nMnExAmt>0,Round((nExAmt/nMnExAmt)*examt,2),0), u_cessamt With Iif(nMnCessAmt>0,Round((nCessAmt/nMnCessAmt)*u_cessamt,2),0), ;
	u_hcesamt With Iif(nMnHCesAmt>0,Round((nHCesAmt/nMnHCesAmt)*u_hcesamt,2),0), u_cvdamt With Iif(nMnCVDAmt>0,Round((nCVDAmt/nMnCVDAmt)*u_cvdamt,2),0), ;
	bcdamt With Iif(nMnBCDAmt>0,Round((nBCDAmt/nMnBCDAmt)*bcdamt,2),0) In Tmp_Item
Select Tmp_Item
Scan
	Select Tmp_Item
	nAmount = nAmount + Tmp_Item.u_AsseAmt

	If nRecCnt = Recno()
		Replace examt With nExAmt1, u_cessamt With nCessAmt1, u_hcesamt With nHCesAmt1, u_cvdamt With nCVDAmt1, ;
			bcdamt With nBCDAmt1 In Tmp_Item
	Endif
	nExAmt1	 = nExAmt1 - Tmp_Item.examt
	nCessAmt1= nCessAmt1 - Tmp_Item.u_cessamt
	nHCesAmt1= nHCesAmt1 - Tmp_Item.u_hcesamt
	nCVDAmt1 = nCVDAmt1 - Tmp_Item.u_cvdamt
	nBCDAmt1 = nBCDAmt1 - Tmp_Item.bcdamt

	Replace gro_amt With u_AsseAmt+examt+u_cessamt+u_hcesamt+u_cvdamt+bcdamt In Tmp_Item
	sql_item = 'IRItem'
	lcSqlStr = SqlConObj.GenInsert(sql_item,"","","Tmp_Item",1)
	nRetVal= SqlConObj.DataConn([EXE],ccdbname,lcSqlStr,[],oHandle,_ndatasessionid,.T.)
	If nRetVal<0
		Return .F.
	Endif
	Select Tmp_Item
Endscan

&&&added by satish pal FOR BUG-4996 dt.06/07/2012
Select Tmp_gen_srno
nRecCnt=Reccount()
Replace entry_ty With Tmp_Main.entry_ty,Tran_cd With mtran_cd , ;
	Date With Tmp_Main.Date,npgno With cPgNo, l_yn With Tmp_Main.l_yn  In Tmp_gen_srno
Select Tmp_gen_srno

sql_gen_srno = 'gen_srno'
lcSqlStr = SqlConObj.GenInsert(sql_gen_srno ,"","","Tmp_gen_srno",1)
nRetVal= SqlConObj.DataConn([EXE],ccDbName,lcSqlStr,[],oHandle,_ndatasessionid,.T.)
If nRetVal<0
	Return .F.
ENDIF

&&&added by satish pal FOR BUG-4996 -end


Select Tmp_AcDet
Replace All Tran_cd With mtran_cd, entry_ty With Tmp_Main.entry_ty, ;
	Date With Tmp_Main.Date, Doc_no With Tmp_Main.Doc_no, Inv_no With Tmp_Main.Inv_no, ;
	l_yn With Tmp_Main.l_yn, Ref_no With '', Re_all With 0, Tds With 0, Disc With 0, ;
	u_cldt With cdSta_Dt In Tmp_AcDet
If Type('Tmp_AcDet.Cl_date') = 'T'
	Replace All Cl_date With cdSta_Dt In Tmp_AcDet
Endif



****** Replace oac_name ***** Start
Local Dbi,Crj,lDbi,lCrj,CrLf
CrLf=Chr(13)+Chr(10)
Store 1 To Dbi
Dime DbArray[Dbi]
DbArray[Dbi]=[]

Select Tmp_AcDet
Scan
	If !Inlist(ac_name,'EXCISE CAPITAL GOODS PAYABLE A/C','BALANCE WITH EXCISE RG23C','CESS CAPITAL GOODS PAYABLE A/C','BALANCE WITH CESS SURCHARGE RG23C','CVD CAPITAL GOODS PAYABLE A/C','BALANCE WITH CVD RG23C','H CESS CAPITAL GOODS PAYABLE A/C','BALANCE WITH HCESS RG23C','BCD CAPITAL GOODS PAYABLE A/C','BALANCE WITH BCD RG23C')
		Loop
	Endif
	Dime DbArray[Dbi]
	DbArray[Dbi]=ac_name+Allt(Str(Amount,17,2))+amt_ty
	Dbi=Dbi+1
Endscan

Select Tmp_AcDet
Scan
	Select Tmp_AcDet
	If !Inlist(ac_name,'EXCISE CAPITAL GOODS PAYABLE A/C','BALANCE WITH EXCISE RG23C','CESS CAPITAL GOODS PAYABLE A/C','BALANCE WITH CESS SURCHARGE RG23C','CVD CAPITAL GOODS PAYABLE A/C','BALANCE WITH CVD RG23C','H CESS CAPITAL GOODS PAYABLE A/C','BALANCE WITH HCESS RG23C','BCD CAPITAL GOODS PAYABLE A/C','BALANCE WITH BCD RG23C')
		Loop
	Endif
	Replace oac_name With [] In Tmp_AcDet
	_FirstName = 1
	For lDbi= 1 To Dbi-1
		If Substr(DbArray[lDbi],1,Len(Tmp_AcDet.ac_name))!= Tmp_AcDet.ac_name					&&Amt_Ty = 'DR'
			Replace oac_name With Iif(_FirstName = 1,'',oac_name+CrLf)+DbArray[lDbi] In Tmp_AcDet
			_FirstName = _FirstName + 1
		Endif
	Endfor
	Select Tmp_AcDet
Endscan
****** Replace oac_name ***** End


Select Tmp_AcDet
Scan
	Select Tmp_AcDet
	If !Inlist(Upper(Tmp_AcDet.ac_name),'EXCISE CAPITAL GOODS PAYABLE A/C','BALANCE WITH EXCISE RG23C','CESS CAPITAL GOODS PAYABLE A/C','BALANCE WITH CESS SURCHARGE RG23C','CVD CAPITAL GOODS PAYABLE A/C','BALANCE WITH CVD RG23C','H CESS CAPITAL GOODS PAYABLE A/C','BALANCE WITH HCESS RG23C','BCD CAPITAL GOODS PAYABLE A/C','BALANCE WITH BCD RG23C')
		Loop
	Endif
	sql_item = 'IRAcDet'
	lcSqlStr = SqlConObj.GenInsert(sql_item,"","","Tmp_Acdet",1)
	nRetVal= SqlConObj.DataConn([EXE],ccdbname,lcSqlStr,[],oHandle,_ndatasessionid,.T.)
	If nRetVal<0
		Return .F.
	Endif

	Select Tmp_AcDet
Endscan

lcSqlStr = 	" UPDATE IRMAIN SET GRO_AMT = ?nAmount, NET_AMT = ?nAmount, EXAMT = ?nExAmt, "+;
	" TOT_EXAMT = ?nExAmt+?nCessAmt+?nHcesAmt+?nBCDAmt+?nCVDAmt, "+;
	" BCDAMT = ?nBCDAmt, U_CESSAMT = ?nCessAmt, U_HCESAMT = ?nHCesAmt, U_CVDAMT = ?nCVDAmt "+;
	" WHERE ENTRY_TY = 'HR' AND TRAN_CD = "+Transform(mtran_cd)
nRetVal= SqlConObj.DataConn([EXE],ccdbname,lcSqlStr,[],oHandle,_ndatasessionid,.T.)
If nRetVal<0
	Return .F.
Endif



*!*	lcSqlStr = " INSERT INTO NEWYEARTRAN VALUES('"+cEntry_ty+"',"+Transform(nTran_Cd)+", ?mUserName, ?cSysDate) "	&& Commented by Shrikant S. on 20/05/2010 for TKT-1476
*!* lcSqlStr = " INSERT INTO NEWYEARTRAN VALUES('"+cEntry_ty+"',"+Transform(nTran_Cd)+", ?mUserName, LEFT(?cSysDate,20)) " && Commented By Amrendra on 29/04/2011 for TKT-7434
lcSqlStr = " INSERT INTO NEWYEARTRAN VALUES('"+cEntry_ty+"',"+Transform(nTran_Cd)+", ?mUserName, LEFT(?cSysDate,20),'"+cEntry_ty1+"',"+Transform(mtran_cd)+") "
nRetVal= SqlConObj.DataConn([EXE],ccdbname,lcSqlStr,[],oHandle,_ndatasessionid,.T.)
If nRetVal<0
	Return .F.
Endif



