*:*****************************************************************************
*:        Program: UETRIGETVALID--Udyog ERP
*:        System : Udyog Software
*:        Author : 
*: 		  Last modified:
*:		  AIM    : To Call function from Lother.dbf (val_con,whn_con,def_val) /Dcmast.dbf/frx files.
*:*****************************************************************************
PROCEDURE CHK_PAGENO()
_MRgRet  = 0
_MRgPage = Item_vw.U_pageno		&&FIELDS
If !Empty(_MRgPage)
*	_MRgRet  = -1  &&FIELDS
	_Malias 	= Alias()
	Sele Item_vw
	_mRecNo 	= Recno()
	etsql_con	= 1
	nHandle     = 0
	_etDataSessionId = _Screen.Activeform.DataSessionId
	SET DATASESSION TO _etDataSessionId
	SqlConObj = NEWOBJECT('SqlConnUdObj','SqlConnection',xapps)
	
	If !Used('Gen_SrNo_Vw')
		etsql_str = "Select * From Gen_SrNo where 1=0"
		etsql_con = SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[Gen_SrNo_Vw],;
			"nHandle",_etDataSessionId,.f.)
		If etsql_con < 1 OR !Used("Gen_SrNo_Vw")
			etsql_con = 0
		ELSE
			SELECT Gen_SrNo_Vw
			INDEX On ItSerial TAG ItSerial
		Endif	
	ENDIF
	If etsql_con > 0
		_mitcode 	= item_vw.it_code
		_mitgrp 	= ''
		_mitchap   	= ''
		_mitserial  = item_vw.itserial
		_mittype = ''
		If uppe(CoAdditional.Rg23_Srno) = 'G' or uppe(CoAdditional.Rg23_Srno) = 'C'
			etsql_str = "Select Top 1 [Group],[ChapNo] From It_mast where It_code=?_mitcode"
			etsql_con = SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[TmpEt_Vw],;
				"nHandle",_etDataSessionId,.f.)
			If etsql_con > 0 And Used("TmpEt_Vw")
				_mitgrp 	= TmpEt_Vw.Group
				_mitchap   	= TmpEt_Vw.ChapNo
			Else
				etsql_con = 0
			Endif
		ENDIF
		
		If uppe(CoAdditional.Rg23_Srno) = 'D' &&rup 
			etsql_str = "Select Top 1 [type] From It_mast where It_code=?_mitcode"
			etsql_con = SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[TmpEt1_Vw],;
				"nHandle",_etDataSessionId,.f.)
			If etsql_con > 0 And Used("TmpEt1_Vw")
				_mittype 	= TmpEt1_Vw.type
			Else
				etsql_con = 0
			Endif
		ENDIF
	Endif
	If etsql_con > 0	
		_mcond = "l_yn='"+ALLTRIM(main_vw.l_yn)+"' and "+"cType = 'RGPART1' And " + iif(CoAdditional.Cate_Srno," Ccate = Main_vw.Cate And ","")	
		Do Case
		Case uppe(CoAdditional.Rg23_Srno) = 'I'
			_mcond = _mcond+" Cit_code = _mitcode "
		Case uppe(CoAdditional.Rg23_Srno) = 'G'
			_mcond = _mcond+" [Cgroup] = _mitgrp "
		Case uppe(CoAdditional.Rg23_Srno) = 'C'
			_mcond = _mcond+" [Cchapno] = _mitchap "
		Case uppe(CoAdditional.Rg23_Srno) = 'D'
			_mcond = _mcond+" [Cittype] = _mittype "
		Other
			_mcond = _mcond+" 1 = 1 "
		Endcase
		SELECT Gen_SrNo_Vw
		SCAN
			IF &_mcond
				IF Gen_SrNo_Vw.ItSerial != _mitserial AND ALLTRIM(Gen_SrNo_Vw.NPgNo) = ALLTRIM(_MRgPage)
					_MRgRet  = 1
				ENDIF
			Endif
		ENDSCAN
		IF _MRgRet != 1
			_mcond = "l_yn='"+ALLTRIM(main_vw.l_yn)+"' and cType = 'RGPART1' And " + iif(CoAdditional.Cate_Srno," Ccate = ?Main_vw.Cate And ","")	
			_mcond = _mcond + " LTRIM(RTRIM(NPgNo)) = LTRIM(RTRIM(?_MRgPage)) And "	&&FIELDS
			Do Case
			Case uppe(CoAdditional.Rg23_Srno) = 'I'
 				_mcond = _mcond+" Cit_code = ?_mitcode "
			Case uppe(CoAdditional.Rg23_Srno) = 'G'
				_mcond = _mcond+" [Cgroup] = ?_mitgrp "
			Case uppe(CoAdditional.Rg23_Srno) = 'C'
				_mcond = _mcond+" [Cchapno] = ?_mitchap "
			Case uppe(CoAdditional.Rg23_Srno) = 'D'
				_mcond = _mcond+" [Cittype] = ?_mittype "
			Other
				_mcond = _mcond+" 1 = 1 "
			ENDCASE
			etsql_str = "Select Top 1 Entry_ty,Tran_cd,Itserial From Gen_SrNo where "+_mcond
			etsql_con = SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[TmpEt_Vw],;
				"nHandle",_etDataSessionId,.f.)
			If etsql_con > 0 And Used("TmpEt_Vw")
				SELECT TmpEt_Vw
				IF RECCOUNT() > 0 AND Entry_ty+STR(Tran_cd)+Itserial # Main_vw.Entry_ty+STR(Main_vw.Tran_cd)+_mitserial
					_MRgRet  = 1
				ELSE
					SELECT Gen_SrNo_Vw
					LOCATE FOR ItSerial = _mitserial
					IF !FOUND()
						APPEND BLANK IN Gen_SrNo_Vw
					Endif	
					REPLACE Ccate With Main_vw.Cate,NPgNo With _MRgPage,;
						Itserial WITH Item_vw.Itserial,Cware WITH Item_vw.Ware_nm,CType WITH 'RGPART1',;
						Cit_code WITH _mitcode,CGroup With _mitgrp,CChapno WITH _mitchap,Cittype WITH _mittype,l_yn WITH main_vw.l_yn IN Gen_SrNo_Vw
					_MRgRet  = 0					
				Endif
			Endif	
		Endif
	Endif
	If Used("TmpEt_Vw")
		Use In TmpEt_Vw
	Endif	
	=SqlConObj.SqlConnClose("nHandle")
	Sele Item_vw
	If Betw(_mRecNo,1,Reccount())
		Go _mRecNo
	ENDIF
	If !Empty(_Malias)
		Select &_Malias
	ENDIF
ENDIF
Return IIF(_MRgRet = 0,.t.,.f.)
*******************************************************************************************************

PROCEDURE GEN_PAGENO()
_MRgPage = Item_vw.U_pageno		&&FIELDS
If Empty(_MRgPage)
	_Malias 	= Alias()
	Sele Item_vw
	_mRecNo 	= Recno()
	etsql_con	= 1
	nHandle     = 0
	_etDataSessionId = _Screen.Activeform.DataSessionId
	SET DATASESSION TO _etDataSessionId
	SqlConObj = NEWOBJECT('SqlConnUdObj','SqlConnection',xapps)

&&-->Rup  12Aug09
	etsql_str = "select tp=[type] from it_mast where it_code="+Str(Item_vw.it_code)
	etsql_con = SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[itype],"nHandle",_etDataSessionId,.F.)
	If etsql_con < 1 Or !Used("itype")
		etsql_con = 0
	ELSE
		Select itype
		If Inlist(itype.tp,'Finished','Semi Finished')
			If Type('main_vw.u_gcssr')='L'
				If main_vw.u_gcssr=.F.
					Return ''
				Endif
			Else
				Return ''
			Endif
		Endif
		If itype.tp='Trading'
			Return ''
		Endif
	Endif
&&<--Rup  12Aug09

	If !Used('Gen_SrNo_Vw')
		etsql_str = "Select * From Gen_SrNo where 1=0"
		etsql_con = SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[Gen_SrNo_Vw],;
			"nHandle",_etDataSessionId,.f.)
		If etsql_con < 1 OR !Used("Gen_SrNo_Vw")
			etsql_con = 0
		ELSE
			SELECT Gen_SrNo_Vw
			INDEX On ItSerial TAG ItSerial
		Endif	
	Endif
	If etsql_con > 0
		_mitcode 	= item_vw.it_code
		_mitgrp 	= ''
		_mitchap   	= ''
		If uppe(CoAdditional.Rg23_Srno) = 'G' or uppe(CoAdditional.Rg23_Srno) = 'C'
			etsql_str = "Select Top 1 [Group],[ChapNo] From It_mast where It_code=?_mitcode"
			etsql_con = SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[TmpEt_Vw],;
				"nHandle",_etDataSessionId,.f.)
			If etsql_con > 0 And Used("TmpEt_Vw")
				_mitgrp 	= TmpEt_Vw.Group
				_mitchap   	= TmpEt_Vw.ChapNo
			Else
				etsql_con = 0
			Endif
		Endif
	ENDIF
	If uppe(CoAdditional.Rg23_Srno) = 'D' &&rup 		
			etsql_str = "Select Top 1 [type] From It_mast where It_code=?_mitcode"
			etsql_con = SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[TmpEt1_Vw],;
				"nHandle",_etDataSessionId,.f.)
			If etsql_con > 0 And Used("TmpEt1_Vw")
				_mittype 	= TmpEt1_Vw.type
			Else
				etsql_con = 0
			Endif
	Endif


	If etsql_con > 0	
		_mcond ="l_yn='"+ALLTRIM(main_vw.l_yn)+"' and cType = 'RGPART1' And " + iif(CoAdditional.Cate_Srno," Ccate = Main_vw.Cate And ","")	
		Do Case
		Case uppe(CoAdditional.Rg23_Srno) = 'I'
			_mcond = _mcond+" Cit_code = _mitcode "
		Case uppe(CoAdditional.Rg23_Srno) = 'G'
			_mcond = _mcond+" [Cgroup] = _mitgrp "
		Case uppe(CoAdditional.Rg23_Srno) = 'C'
			_mcond = _mcond+" [Cchapno] = _mitchap "
		Case uppe(CoAdditional.Rg23_Srno) = 'D' &&rup
			_mcond = _mcond+" [cittype] = _mittype "	
		Other
			_mcond = _mcond+" 1 = 1 "
		Endcase
		SELECT Gen_SrNo_Vw
		SCAN
			IF &_mcond
				IF ALLTRIM(_MRgPage) <= Allt(Gen_SrNo_Vw.NPgNo)
					_MRgPage = ALLTRIM(STR(IIF(ISNULL(Gen_SrNo_Vw.NPgNo),0,VAL(Gen_SrNo_Vw.NPgNo)) + 1))
				ENDIF
			ENDIF
			SELECT Gen_SrNo_Vw
		ENDSCAN
		IF EMPTY(_MRgPage)
			_mcond ="l_yn='"+ALLTRIM(main_vw.l_yn)+"' and cType = 'RGPART1' And " + iif(CoAdditional.Cate_Srno," Ccate = ?Main_vw.Cate And ","")	
			Do Case
			Case uppe(CoAdditional.Rg23_Srno) = 'I'
				_mcond = _mcond+" Cit_code = ?_mitcode "
			Case uppe(CoAdditional.Rg23_Srno) = 'G'
				_mcond = _mcond+" [Cgroup] = ?_mitgrp "
			Case uppe(CoAdditional.Rg23_Srno) = 'C'
				_mcond = _mcond+" [Cchapno] = ?_mitchap "
			Case uppe(CoAdditional.Rg23_Srno) = 'D' &&rup
				_mcond = _mcond+" [Cittype] = ?_mittype "
			Other
				_mcond = _mcond+" 1 = 1 "
			Endcase
			etsql_str = "Select Max(Cast(nPgNo as int)) as PageNo From Gen_SrNo where "+_mcond
			etsql_con = SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[TmpEt_Vw],;
				"nHandle",_etDataSessionId,.f.)
			If etsql_con > 0 And Used("TmpEt_Vw")
				_MRgPage = ALLTRIM(STR(IIF(ISNULL(TmpEt_Vw.PageNo),0,TmpEt_Vw.PageNo) + 1))
			Endif	
		Endif	
	Endif
	If etsql_con <= 0
		_MRgPage = '***'
	Endif
	If Used("TmpEt_Vw")
		Use In TmpEt_Vw
	Endif	
	=SqlConObj.SqlConnClose("nHandle")
	Sele Item_vw
	If Betw(_mRecNo,1,Reccount())
		Go _mRecNo
	Endif
	If !Empty(_Malias)
		Select &_Malias
	ENDIF
	
	_MRgPage = Padr(_MRgPage,LEN(Item_vw.U_PageNo))		&&FIELDS
Endif
Return _MRgPage
*******************************************************************************************************

PROC ST_CHK_APPACK() &&RUP:-procedure is useful to update default value of average qty,qty,mrprate,abtper etc. as per it_mast values
mAlias = Alias()
LOCAL vappack,vmrprate,vabtper
sqlconobj=NEWOBJECT('sqlconnudobj',"sqlconnection",xapps)
nHandle=0
sq1="SELECT average,mrprate,abtper FROM IT_MAST WHERE IT_CODE ="+STR(ITEM_VW.IT_CODE)
nRetval = sqlconobj.dataconn([EXE],company.dbname,sq1,"EXCUR","nHandle",_SCREEN.ActiveForm.DATASESSIONID)
If nRetval<0
	Return .F.
ENDIF
SELECT EXCUR
vappack=IIF(!ISNULL(EXCUR.average),EXCUR.average,0)
vmrprate=IIF(!ISNULL(EXCUR.mrprate),EXCUR.mrprate,0)
vabtper=IIF(!ISNULL(EXCUR.abtper),EXCUR.abtper,0)
IF USED("EXCUR")
	USE IN EXCUR
ENDIF

REPLACE u_appack WITH IIF(item_vw.u_appack=' ',ALLTRIM(STR(vappack)),item_vw.u_appack),u_mrprate WITH IIF(item_vw.u_mrprate=0,vmrprate,item_vw.u_mrprate),abtper WITH IIF(item_vw.abtper=0,vabtper,item_vw.abtper) IN ITEM_VW
IF VAL(item_vw.u_pkno)#0 AND VAL(item_vw.u_appack)#0
	REPLACE qty WITH (VAL(item_vw.u_pkno) * VAL(item_vw.u_appack)) IN item_vw
ENDIF
sele(mAlias)
RETURN .T.

PROC OP_CHK_APPACK() &&RUP:-procedure is useful to update default value of average qty,qty etc. as per it_mast values && 04Oct09
mAlias = Alias()
LOCAL vappack,vmrprate,vabtper
sqlconobj=NEWOBJECT('sqlconnudobj',"sqlconnection",xapps)
nHandle=0
sq1="SELECT average FROM IT_MAST WHERE IT_CODE ="+STR(ITEM_VW.IT_CODE)
nRetval = sqlconobj.dataconn([EXE],company.dbname,sq1,"EXCUR","nHandle",_SCREEN.ActiveForm.DATASESSIONID)
If nRetval<0
	Return .F.
ENDIF
SELECT EXCUR
vappack=IIF(!ISNULL(EXCUR.average),EXCUR.average,0)

IF USED("EXCUR")
	USE IN EXCUR
ENDIF

REPLACE u_appack WITH IIF(item_vw.u_appack=' ',ALLTRIM(STR(vappack)),item_vw.u_appack) IN ITEM_VW
IF VAL(item_vw.u_pkno)#0 AND VAL(item_vw.u_appack)#0
	REPLACE qty WITH (VAL(item_vw.u_pkno) * VAL(item_vw.u_appack)) IN item_vw
ENDIF
sele(mAlias)
RETURN .T.

PROC st_ass_whn() &&RUP:-procedure is useful to update default value of assessable amount with/without (mrprate and abtper),round-off value in Sales
	IF item_vw.u_mrprate#0
		SELE item_vw
		IF item_vw.abtper#0
			repl U_ASSEAMT WITH ROUND((QTY*U_MRPRATE)-(QTY*U_MRPRATE*ABTPER)/100,2) IN item_vw
		ELSE
			repl U_ASSEAMT WITH ROUND((QTY*U_MRPRATE),2) IN item_vw
		ENDIF
	    repl rate with iif(rate=0,round(u_mrprate-((u_mrprate*abtper)/100),2),rate) IN item_vw
	ELSE
		REPL U_ASSEAMT WITH ROUND(QTY*RATE,2) IN item_vw
	ENDIF
	
	if coadditional.rndavalue
	    replace u_asseamt with round(u_asseamt,0) in item_vw
	endif
	
	FRMXTRA.TXTU_ASSEAMT.REFRESH

	RETURN .T.




PROC PT_ASS_WHN() &&RUP:-procedure is useful to update default value of assessable amount in Purchase entry
	SELE item_vw
	REPL U_asseamt WITH RATE*QTY  IN item_vw
	RETU



PROC PT_RATE_DEF() &&RUP:-procedure is useful to update default value of rate calculation as per ass.value entred by user  in Purchase entry
	SELE item_vw
	REPL RATE WITH (U_ASSEAMT/QTY) IN item_vw
	RETU .T.

PROC CHK_CHAPNO() &&RUP:-procedure is useful to Check ChapNo in  Item Master. 09/09/09  
	PARAMETERS vchapno
	vchapno=ALLTRIM(vchapno)
	visdigit=.t.
	IF LEN(vchapno)<>8
		visdigit=.F.
	ELSE
		FOR i=1 TO LEN(vchapno)
			IF !ISDIGIT(substr(vchapno,i,1))
				visdigit=.f.
				exit		
			ENDIF
		NEXT i
	ENDIF  
	RETU visdigit

PROC GEN_NO() &&RUP:-procedure is useful to generate part-ii,pla,are1,are2,are3..etc field in Daily Debit form UEFRM_ST_DAILTDEBIT in Sales Entry.
PARAMETERS fldnm,tblnm

LOCAL VRNO
sqlconobj=NEWOBJECT('sqlconnudobj',"sqlconnection",xapps)
nHandle=0
sq1="SELECT MAX(CAST( "+ALLTRIM(fldnm)+"  AS INT)) AS RNO  FROM "+ALLTRIM(tblnm)+" WHERE ISNUMERIC( "+ALLTRIM(fldnm)+" )=1 and l_yn='"+ALLTRIM(main_vw.l_yn)+"' "

nRetval = sqlconobj.dataconn([EXE],company.dbname,sq1,"EXCUR","nHandle",_SCREEN.ActiveForm.DATASESSIONID)
If nRetval<0
	Return .F.
ENDIF
SELECT EXCUR
VRNO=ALLTRIM(STR(IIF(ISNULL(EXCUR.RNO),1,(EXCUR.RNO)+1)))
IF USED("EXCUR")
	USE IN EXCUR
ENDIF
*sele(mAlias)
RETURN VRNO


PROCEDURE dup_No &&RUP:-procedure is useful to check duplicate value of part-ii,pla,are1,are2,are3..etc field in Daily Debit form UEFRM_ST_DAILTDEBIT in Sales Entry.
PARAMETERS FLDNM,FLDVAL,TBLNM
_Malias 	= Alias()
_mRecNo	= Recno()
	
*mAlias = Alias()
LOCAL VDUP
IF fldval=' '
	RETURN .t.
ENDIF

sqlconobj=NEWOBJECT('sqlconnudobj',"sqlconnection",xapps)
nHandle=0
sq1="SELECT "+FLDNM+" FROM "+TBLNM+" WHERE l_yn='"+ALLTRIM(main_vw.l_yn)+"' and "+FLDNM+" = '"+ALLTRIM(FLDVAL)+"' AND NOT ("+TBLNM+".TRAN_CD="+STR(MAIN_VW.TRAN_CD)+" AND "+TBLNM+".ENTRY_TY='"+MAIN_VW.ENTRY_TY+"'"+IIF(FLDNM='U_PAGENO'," AND "+TBLNM+".ITSERIAL='"+ITEM_VW.ITSERAIL+"')",")")

nRetval = sqlconobj.dataconn([EXE],company.dbname,sq1,"EXCUR","nHandle",_SCREEN.ActiveForm.DATASESSIONID)
If nRetval<0
	Return .F.
ENDIF

SELECT EXCUR
VRCOUNT=RECCOUNT()
IF USED("EXCUR")
	USE IN EXCUR
ENDIF

If !Empty(_Malias)
	Select &_Malias
ENDIF

If Betw(_mRecNo,1,Reccount())
	Go _mRecNo
ENDIF

IF VRCOUNT>0 AND !ISNULL(VRCOUNT)
	RETURN .f.
ELSE
	RETURN .t.
ENDIF

*sele(mAlias)

RETURN 

Proc chk_bond_ac() &&RUP:-procedure is useful to Search Bond Account Name in Sales entry in DCMAST.
sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)
nHandle=0
sq1="select distinct ac_mast.ac_name from obmain m "
sq2="inner join obacdet ac on (m.tran_cd=ac.tran_cd) inner join ac_mast on (ac_mast.ac_id=m.ac_id)"
sq3="where bond_no='"+Alltrim(main_vw.bond_no)+"'"

nRetval = sqlconobj.dataconn([EXE],company.dbname,sq1+sq2+sq3,"_bondac","nHandle",_Screen.ActiveForm.DataSessionId)
If nRetval<0
	Return .F.
Endif
If Used("_bondac")
	Select _bondac
	macname=Iif(!Isnull(_bondac.ac_name),_bondac.ac_name,"SALES")
Endif
macname=Iif(!Isnull(macname),macname,"SALES")
Return macname
Endproc


&&code added by Ajay Jasiwal on 16/03/2009 ---> start
PROCEDURE SALESMAN()
sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)
nHandle=0
sq1="select salesman from ac_mast ac where ac.ac_name=?main_vw.party_nm"

nRetval = sqlconobj.dataconn([EXE],company.dbname,sq1,"ajcur","nHandle",_Screen.ActiveForm.DataSessionId)
If nRetval<0
	Return .F.
ENDIF
SELECT ajcur
replace salesman WITH ajcur.salesman IN main_vw
endproc
&&code added by Ajay Jasiwal on 16/03/2009 ---> end

PROCEDURE chk_fldlen()
PARAMETERS _chkfldnm
IF LEN(ALLTRIM(_chkfldnm)) <> LEN(_chkfldnm)
	RETURN .f.
ENDIF
RETURN .t.

PROCEDURE Chk_Time()
PARAMETERS _ChkFldNm
IF (VAL(SUBSTR(_ChkFldNm,1,2))=24 AND VAL(SUBSTR(_ChkFldNm,4,2))<>0) 
	RETURN .f.
ENDIF 
IF !(BETWEEN(VAL(SUBSTR(_ChkFldNm,1,2)),0,24) AND BETWEEN(VAL(SUBSTR(_ChkFldNm,4,2)),0,59)) and !(BETWEEN(VAL(SUBSTR(_ChkFldNm,1,2)),0,24) AND VAL(SUBSTR(_ChkFldNm,4,2))=0)
	RETURN .f.
ENDIF
RETURN .t.	

PROCEDURE repleccno			&& used in accountmmaster --> 110909----sachin.s
	LPARAMETERS oobject
	cvalue = ALLTRIM(ac_mast_vw.eccno)
	llret=.T.
	DO WHILE .T.
		nkeycode = ASC(LEFT(cvalue,1))
		IF !EMPTY(cvalue)
			IF !BETWEEN(nkeycode,ASC('A'),ASC('Z')) AND !BETWEEN(nkeycode,ASC('a'),ASC('z')) AND                                      !BETWEEN(nkeycode,ASC('0'),ASC('9')) AND !INLIST(nkeycode,6,9,13,15,32,127,5,4,19,24,7,52,54)
				llret = .F.
				EXIT
			ENDIF
		ELSE
			EXIT
		ENDIF
		cvalue = SUBSTR(cvalue,2)
	ENDDO
	IF !llret
		=MESSAGEBOX("Please enter valid character values.",64,vumess)
		RETURN .F.
	ENDIF
	SELECT ac_mast_vw
	IF EMPTY(ac_mast_vw.cexregno)
		REPLACE cexregno WITH eccno IN ac_mast_vw
	ENDIF
	oobject.PARENT.REFRESH
ENDPROC

PROCEDURE PAGENO_WHN() &&-->Rup  26Sep09
_Malias 	= Alias()
Sele Item_vw
_mRecNo 	= Recno()

retval=.t.
etsql_str = "select tp=[type] from it_mast where it_code="+Str(Item_vw.it_code)
_etDataSessionId = _Screen.Activeform.DataSessionId
SET DATASESSION TO _etDataSessionId
SqlConObj = NEWOBJECT('SqlConnUdObj','SqlConnection',xapps)
etsql_con = SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[itype],"nHandle",_etDataSessionId,.F.)
If etsql_con < 1 Or !Used("itype")
	etsql_con = 0
ELSE
	Select itype
	If Inlist(itype.tp,'Finished','Semi Finished')
		If Type('main_vw.u_gcssr')='L'
			If main_vw.u_gcssr=.F.
				retval=.f.
			Endif
		Else
			retval=.f.
		Endif
	Endif
	If itype.tp='Trading'
		retval=.f.
	ENDIF
ENDIF
RETURN retval
&&<--Rup  26Sep09

&&<--Shrikant 26Sep09 
PROC GEN_NextNo() &&Shrikant: procedure is useful to generate CT-1,CT-3,are1 No.,are2 No.,are3 No. in form UEFRM_ST_exdata1 in Sales Entry.
PARAMETERS fldnm,tblnm,filconfld,filconval
LOCAL VRNO
sqlconobj=NEWOBJECT('sqlconnudobj',"sqlconnection",xapps)
nHandle=0
sq1="SELECT MAX(CAST( "+ALLTRIM(fldnm)+"  AS INT)) AS RNO  FROM "+ALLTRIM(tblnm)+" WHERE ISNUMERIC( "+ALLTRIM(fldnm)+" )=1 and l_yn='"+ALLTRIM(main_vw.l_yn)+"' " +" and "+ALLTRIM(filconfld)+ " = '"+ALLTRIM(filconval)+"'"
nRetval = sqlconobj.dataconn([EXE],company.dbname,sq1,"EXCUR","nHandle",_SCREEN.ActiveForm.DATASESSIONID)
If nRetval<0
	Return .F.
ENDIF
SELECT EXCUR
VRNO=ALLTRIM(STR(IIF(ISNULL(EXCUR.RNO),1,(EXCUR.RNO)+1)))
IF USED("EXCUR")
	USE IN EXCUR
ENDIF
RETURN VRNO
&&<--Shrikant 26Sep09 


Procedure MfgExpDtChk		&& Procedure to Check the Manufacturing Date with Expiry Date
	Lparameters oObject
Select item_vw
If !Empty(item_vw.MFGDT) And !Empty(item_vw.EXPDT)
	If item_vw.MFGDT > item_vw.EXPDT
		Return .F.
	Endif
Endif

If !Empty(item_vw.MFGDT) And Empty(item_vw.EXPDT) And Upper(Alltrim(Strextract(oObject.ControlSource,'.'))) = 'EXPDT'

	Return .F.
Endif
	oObject.Parent.Refresh
ENDPROC


**Add the below code to the uetrigetvalid.prg and then compile 
PROCEDURE Check_BondPeriod()
Lparameters oObject

Select Main_vw
If !Empty(Main_vw.u_pinvdt) And !Empty(Main_vw.EXBVLDT)
	If Main_vw.u_pinvdt > Main_vw.EXBVLDT
		Return .F.
	Endif
Endif

If !Empty(Main_vw.u_pinvdt) And Empty(Main_vw.EXBVLDT) And Upper(Alltrim(Strextract(oObject.ControlSource,'.'))) = 'EXBVLDT'
	Return .F.
Endif
oObject.Parent.Refresh
ENDPROC


&&Rup 14/11/2009-->
PROCEDURE chk_empty_pan() &&Rup 14/11/2009
_etDataSessionId = _Screen.Activeform.DataSessionId
SET DATASESSION TO _etDataSessionId 
vpanempty=.f.
*sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)
nHandle=0
sq1="select i_tax from ac_mast where ac_id=?main_vw.ac_id"
SqlConObj = NEWOBJECT('SqlConnUdObj','SqlConnection',xapps)
nRetval = sqlconobj.dataconn([EXE],company.dbname,sq1,"panchkcur","nHandle",_etDataSessionId )
If nRetval<0
	Return .F.
ENDIF
IF USED('panchkcur')
	SELECT panchkcur
	IF !EMPTY(panchkcur.i_tax)
		IF inlist(ALLTRIM(panchkcur.i_tax),'PANAPPLIED','PANNOTAVBL','PANINVALID')
			vpanempty=.t.
		ENDIF 
	ELSE
		vpanempty=.t.
	ENDIF 
	USE IN panchkcur
ENDIF 
RETURN vpanempty

&&<--Rup 14/11/2009


&&<--San  21/01/2010


PROCEDURE AVGQTY
REPLACE U_AVGWT WITH ((Main_vw.U_WT1+Main_vw.U_WT2+Main_vw.U_WT3+Main_vw.U_WT4+Main_vw.U_WT5)/(Main_vw.U_MT1+Main_vw.U_MT2+Main_vw.U_MT3+Main_vw.U_MT4+Main_vw.U_MT5)) IN Main_vw
RETURN .T.

PROCEDURE DUPBALE
ubale=u_balenos
itemno=Item_vw.ItSerial
*   WAIT WINDOW itemno
SELECT Item_vw
GO TOP
DO WHILE ItSerial <> itemno
	IF u_balenos=ubale
		MESSAGEBOX("Duplicate Bale no")
		RETURN .F.
	ENDIF
	SKIP
ENDDO
RETURN .T.



PROCEDURE lrcheck
*!*	IF BETWEEN(item_vw.u_lrdt,CTOD("01/04/2007"),CTOD("31/03/2008"))
*!*		WAIT WINDOW "OK"
*!*	    RETURN .T.
*!*	ENDIF
WAIT WINDOW "not ok"
RETURN .F.

PROCEDURE mtrrepl
REPLACE u_proacptq WITH u_accptmtr IN Main_vw
RETURN .T.

PROCEDURE  DBK()
mAlias = ALIAS()
itname=Item_vw.ITEM
date1= Item_vw.DATE
SqlConObj=NEWOBJECT('sqlconnudobj',"sqlconnection",xapps)
nHandle=0
sq1="SELECT it_name,dbkper,dbkwt FROM drawback WHERE it_name=?itname and (?date1 between fdate and tdate)"
nRet = SqlConObj.DataConn([EXE],Company.DbName,sq1,[TmpEt_Vw],;
	"nHandle",_SCREEN.ACTIVEFORM.DATASESSIONID,.F.)
IF nRetval<0
	RETURN .F.
ENDIF
REPLACE Item_vw.u_dbkper WITH TmpEt_Vw.dbkper
REPLACE Item_vw.u_dbkwt WITH TmpEt_Vw.dbkwt
RETURN .T.




PROCEDURE STOP_LOT_CHK()
mAlias = ALIAS()
lotno=Main_vw.u_lotnop
party=Main_vw.party_nm
SET STEP ON
SqlConObj=NEWOBJECT('sqlconnudobj',"sqlconnection",xapps)
nHandle=0
sq1="SELECT u_lrno,u_lrdt,u_lotdt,u_stoplot FROM IIITEM WHERE u_lotno=?lotno and party_nm=?party"
nRet = SqlConObj.DataConn([EXE],Company.DbName,sq1,[TmpEt_Vw],;
	"nHandle",_SCREEN.ACTIVEFORM.DATASESSIONID,.F.)
nRetval = SqlConObj.DataConn([EXE],Company.DbName,sq1,"EXCUR","nHandle",_SCREEN.ACTIVEFORM.DATASESSIONID)
IF nRetval<0
	RETURN .F.
ENDIF
IF TmpEt_Vw.u_stoplot=.T.
	MESSAGEBOX("This Lot is STOP LOT")
	RETURN .F.
	QUIT
ELSE
	RETURN .T.
ENDIF
PROCEDURE ES_REPLA()
mAlias = ALIAS()
lotno=Main_vw.u_lotnop
party=Main_vw.party_nm
SqlConObj=NEWOBJECT('sqlconnudobj',"sqlconnection",xapps)
nHandle=0
sq1="SELECT * FROM IIITEM WHERE u_lotno=?lotno and party_nm=?party"
nRet = SqlConObj.DataConn([EXE],Company.DbName,sq1,[TmpEt_Vw],;
	"nHandle",_SCREEN.ACTIVEFORM.DATASESSIONID,.F.)
nRetval = SqlConObj.DataConn([EXE],Company.DbName,sq1,"EXCUR","nHandle",_SCREEN.ACTIVEFORM.DATASESSIONID)
IF nRetval<0
	RETURN .F.
ENDIF

tcd = TmpEt_Vw.Tran_cd
REPLACE u_lrdt WITH TmpEt_Vw.u_lrdt,u_lotdt WITH TmpEt_Vw.u_lotdt,u_lotno WITH TmpEt_Vw.u_lotno,u_lrno WITH TmpEt_Vw.u_lrno,u_qltynm WITH TmpEt_Vw.u_qltynm IN Item_vw

SqlConObj=NEWOBJECT('sqlconnudobj',"sqlconnection",xapps)
nHandle=0
sq1="SELECT * FROM IIMAIN WHERE TRAN_CD=?TCD"
nRet = SqlConObj.DataConn([EXE],Company.DbName,sq1,[TmpEt_Vw],;
	"nHandle",_SCREEN.ACTIVEFORM.DATASESSIONID,.F.)
nRetval = SqlConObj.DataConn([EXE],Company.DbName,sq1,"EXCUR","nHandle",_SCREEN.ACTIVEFORM.DATASESSIONID)
IF nRetval<0
	RETURN .F.
ENDIF
REPLACE u_PAINVNO WITH TmpEt_Vw.u_PAINVNO,u_TRANSPRT WITH TmpEt_Vw.u_TRANSPRT,u_SUPLNM WITH TmpEt_Vw.u_SUPLNM,u_PAINVDT WITH TmpEt_Vw.u_PAINVDT,u_CONNO WITH TmpEt_Vw.u_CONNO,u_BROKER WITH TmpEt_Vw.u_BROKER IN Main_vw
RETURN .T.


PROCEDURE ES_DET_REPLA
mAlias = ALIAS()
lotno=Main_vw.u_lotnop
party=Main_vw.party_nm
SqlConObj=NEWOBJECT('sqlconnudobj',"sqlconnection",xapps)
nHandle=0
sq1="SELECT * FROM IIITEM WHERE u_lotno=?lotno and party_nm=?party"
nRet = SqlConObj.DataConn([EXE],Company.DbName,sq1,[TmpEt_Vw],;
	"nHandle",_SCREEN.ACTIVEFORM.DATASESSIONID,.F.)
nRetval = SqlConObj.DataConn([EXE],Company.DbName,sq1,"EXCUR","nHandle",_SCREEN.ACTIVEFORM.DATASESSIONID)
IF nRetval<0
	RETURN .F.
ENDIF

tcd = TmpEt_Vw.Tran_cd
REPLACE u_lrdt WITH TmpEt_Vw.u_lrdt,u_lotdt WITH TmpEt_Vw.u_lotdt,u_lotno WITH TmpEt_Vw.u_lotno,u_lrno WITH TmpEt_Vw.u_lrno,u_qltynm WITH TmpEt_Vw.u_qltynm IN Item_vw
RETURN .T.

PROCEDURE bundle_valid
LPARAMETERS tcBun AS STRING,tcnobun AS STRING
SET STEP ON
IF EMPTY(tcBun)
	RETURN .T.
ENDIF

IF LASTKEY() = -1			&& f2
	RETURN .T.
ENDIF


*!*	DO CASE
*!*	CASE tcFldname = 'BUNSEC'
*!*	CASE tcFldname = 'BUNSEC'
*!*	CASE tcFldname = 'BUNSEC'
*!*	ENDCASE

*!*	MESSAGEBOX(tcBun)
*!*	MESSAGEBOX(tcnobun)
bun = tcBun
mAlias = ALIAS()
nRetval=1
nHandle=0
itsr=VAL(Item_vw.item_no)
itsr=itsr+1
SqlConObj=NEWOBJECT('sqlconnudobj','sqlconnection',xapps)
nHandle=0
sq1="SELECT * FROM BUNDLE WHERE BUNDLE=?bun"
nRetval = SqlConObj.DataConn([EXE],Company.DbName,sq1,"_xbundle","nHandle",_SCREEN.ACTIVEFORM.DATASESSIONID)
SELE _xbundle
GO TOP
IF RECCOUNT('_xbundle') <> 0
	SCAN
		SELECT _xbundle
		SCATTER NAME _oxbundle
		SELE Item_vw
		APPEND BLANK
		GATH NAME _oxbundle
		REPL ITEM 		WITH _xbundle.ITEM 		,;
			u_appack	WITH _xbundle.qty  		,;
			rate		WITH 1					,;
			item_no   WITH ALLTRIM(STR(itsr))	,;
			u_wt 		WITH _xbundle.weight    	,;
			U_EXPDESC  WITH _xbundle.weight*u_appack
		REPLA Item_vw.U_PIECES WITH Item_vw.u_appack&& first assign value & then made 0
		IF EMPT(Item_vw.u_wt)
		ELSE
			REPLA Item_vw.U_EXPDESC WITH Item_vw.u_wt*Item_vw.u_appack
		ENDIF
		IF EMPT(Item_vw.U_PIECES)
			REPLA Item_vw.U_SWT WITH (((Item_vw.U_EXPDESC)/1000)*VAL(STR(tcnobun)))
		ELSE
			REPLA Item_vw.U_SWT WITH (((Item_vw.U_EXPDESC)/1000)*VAL(STR(tcnobun)))
		ENDIF
		REPLA Item_vw.U_GWT WITH (Item_vw.U_SWT+(Item_vw.U_SWT*Main_vw.U_GPER)/100)
		IF !EMPT(Item_vw.U_BOXWT)
			REPLA Item_vw.U_GROSSWT WITH ROUND(Item_vw.U_GWT+Item_vw.U_BOXWT/1000,3)
		ELSE
			REPLA Item_vw.U_GROSSWT WITH ROUND(Item_vw.U_GWT,3)
		ENDIF
		REPLACE Item_vw.qty WITH Item_vw.U_GWT
		REPLACE Item_vw.U_PIECES WITH 0  && made 0 as it should be replaced only on last line
		REPLACE Item_vw.doc_no WITH Main_vw.doc_no
		REPLACE Item_vw.Entry_ty WITH Main_vw.Entry_ty
		REPLACE Item_vw.DATE WITH Main_vw.DATE
		itsr=itsr+1
		SELE _xbundle
	ENDSCAN
	REPLACE Item_vw.u_bundle WITH VAL(STR(tcnobun)) IN Item_vw
	REPLACE Item_vw.U_GROSSWT WITH (Item_vw.U_GROSSWT+(VAL(STR(tcnobun))/1000)) IN Item_vw
ENDIF
IF nRetval<0
	RETURN .F.
ENDIF
RETURN .T.
ENDPROC



*!*	PROCEDURE lrdt()
*!*		mAlias = Alias()
*!*		lotno=item_vw.u_lotno
*!*		party=item_vw.party_nm
*!*		SET STEP ON
*!*		sqlconobj=NEWOBJECT('sqlconnudobj',"sqlconnection",xapps)
*!*		nHandle=0
*!*		sq1="SELEwCT u_lrno,u_lrdt,u_lotdt FROM IIITEM WHERE u_lotno=?lotno and party_nm=?party"
*!*		nRet = sqlconobj.DataConn([EXE],Company.DbName,sq1,[TmpEt_Vw],;
*!*					"nHandle",_Screen.Activeform.DataSessionId,.f.)
*!*	*!*		nRetval = sqlconobj.dataconn([EXE],company.dbname,sq1,"EXCUR","nHandle",_SCREEN.ActiveForm.DATASESSIONID)
*!*		If nRetval<0
*!*			Return .F.
*!*		ENDIF
*!*		replace item_vw.u_lrdt WITH TmpEt_Vw.u_lrdt
*!*		replace item_vw.u_lotdt WITH TpmpEt_vw.u_lotdt
*!*	RETURN .t.
PROCEDURE sr()
sr=Item_vw.U_SRNO
REPLACE ALL Item_vw.U_SRNO WITH sr
RETURN .T.

PROCEDURE Cate()
mAlias = ALIAS()
category=Main_vw.Cate
SqlConObj=NEWOBJECT('sqlconnudobj',"sqlconnection",xapps)
nHandle=0
sq1="SELECT * FROM category WHERE cate=?category"
nRet = SqlConObj.DataConn([EXE],Company.DbName,sq1,[TmpEt_Vw],;
	"nHandle",_SCREEN.ACTIVEFORM.DATASESSIONID,.F.)
nRetval = SqlConObj.DataConn([EXE],Company.DbName,sq1,"EXCUR","nHandle",_SCREEN.ACTIVEFORM.DATASESSIONID)
IF nRetval<0
	RETURN .F.
ENDIF

IF TmpEt_Vw.U_GPER > 0
	REPLACE U_GPER WITH TmpEt_Vw.U_GPER IN Main_vw
ELSE
	REPLACE U_GPER WITH 0.000 IN Main_vw
ENDIF
REPLACE u_consigne WITH TmpEt_Vw.u_consigne IN Main_vw
REPLACE	u_port1 WITH TmpEt_Vw.u_port IN Main_vw
REPLACE	u_notify1 WITH TmpEt_Vw.u_notify	IN Main_vw
REPLACE	u_contract	WITH TmpEt_Vw.u_contract IN Main_vw
RETURN .T.

PROCEDURE gen_b17no
IF _SCREEN.ACTIVEFORM.addmode=.T. AND (Main_vw.RULE='IMPORTED            ' OR Main_vw.RULE='INDIGENIOUS         ')
	mAlias = ALIAS()
	SqlConObj=NEWOBJECT('sqlconnudobj',"sqlconnection",xapps)
	nHandle=0
	sq1="SELECT b17no from b17no"
	nRetval = SqlConObj.DataConn([EXE],Company.DbName,sq1,"EXCUR","nHandle",_SCREEN.ACTIVEFORM.DATASESSIONID)
	IF nRetval<0
		RETURN .F.
	ENDIF
	tmp=EXCUR.b17no+1
	IF EMPTY(Main_vw.u_b1no)
		REPLACE Main_vw.u_b1no WITH STR(tmp)
		sq2="update b17no set b17no=?tmp"
		nRetval = SqlConObj.DataConn([EXE],Company.DbName,sq2,"EXCUR","nHandle",_SCREEN.ACTIVEFORM.DATASESSIONID)
	ENDIF
	RETURN .T.
ENDIF
ENDPROC





&&<--San  21/01/2010
Procedure Chk_VendType
_curform = _Screen.ActiveForm
_etDataSessionId = _curform.DataSessionId
Set DataSession To _etDataSessionId
nHandle=0
mAcGroup = Ac_Mast_vw.Group
sq1=" Execute Usp_Ent_Get_Parent_Acgroup ?mAcGroup "
nRetval = _curform.SqlConObj.DataConn([EXE],Company.DbName,sq1,"panchkcur","nHandle",_etDataSessionId )
If nRetval<0
	Return .F.
Endif

Select Ac_Group_Name From panchkcur Where Ac_Group_Name = 'SUNDRY CREDITORS' Into Cursor cur1
If _Tally > 0
	Return .T.
Else
	Return .F.
Endif

Endproc


&&Rup 14/11/2009-->
PROCEDURE chk_empty_pan() &&Rup 14/11/2009
_etDataSessionId = _Screen.Activeform.DataSessionId
SET DATASESSION TO _etDataSessionId 
vpanempty=.f.
*sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)
nHandle=0
sq1="select i_tax from ac_mast where ac_id=?main_vw.ac_id"
SqlConObj = NEWOBJECT('SqlConnUdObj','SqlConnection',xapps)
nRetval = sqlconobj.dataconn([EXE],company.dbname,sq1,"panchkcur","nHandle",_etDataSessionId )
If nRetval<0
	Return .F.
ENDIF
IF USED('panchkcur')
	SELECT panchkcur
	IF !EMPTY(panchkcur.i_tax)
		IF inlist(ALLTRIM(panchkcur.i_tax),'PANAPPLIED','PANNOTAVBL','PANINVALID')
			vpanempty=.t.
		ENDIF 
	ELSE
		vpanempty=.t.
	ENDIF 
	USE IN panchkcur
ENDIF 
RETURN vpanempty

&&<--Rup 14/11/2009

**&& Added For Expenses Purchase On 21/12/2009 by Hetal L Patel Start
PROCEDURE calcvat() 
REPLACE u_vatonamt WITH ROUND(((item_vw.qty*item_vw.rate)*item_vw.u_vatonp)/100,2) IN item_vw
**&& Added For Expenses Purchase On 21/12/2009 by Hetal L Patel End


**start OPST_CHK_AVGPACK() TKT 479
**&& Added On 22/02/2010 by Hetal L Patel St
PROC OPST_CHK_AVGPACK() &&Check reverse Calculation
mAlias = Alias()
sqlconobj=NEWOBJECT('sqlconnudobj',"sqlconnection",xapps)
nHandle=0

IF VAL(item_vw.U_PKNO)#0
	_mqty = 0
	IF VAL(item_vw.u_pkno)#0 AND VAL(item_vw.u_appack)#0
		_mqty = (VAL(item_vw.u_pkno) * VAL(item_vw.u_appack))
	ENDIF
	IF _mqty # 0 
		IF  (([vuexc] $ vchkprod) Or ([vuinv] $ vchkprod)) AND Type('main_vw.entry_ty')='C' &&Check Existing Records
			DO CASE 
				CASE main_vw.entry_ty='OP'
					nretval = 0
					nHandle = 0
					etsql_con  = 0
					nHandle    = 0
					SELECT aentry_ty,atran_cd,aitserial,qty FROM projectitref_vw WHERE entry_ty=main_vw.entry_ty AND tran_cd=main_vw.tran_cd AND itserial=item_vw.itserial INTO CURSOR tibl
					etsql_str  = ""
					etsql_str = "USP_ENT_CHK_OP_ALLOCATION '"+item_vw.entry_ty+"',"+alltrim(STR(item_vw.tran_cd))+","+alltrim(STR(item_vw.IT_CODE))+",'"+item_vw.itserial+"','";
								+tibl.Aentry_ty+"',"+ALLTRIM(STR(tibl.atran_cd))+",'"+tibl.aitserial+"'"
					etsql_con = SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[tibl_1],"nHandle",_SCREEN.ActiveForm.DATASESSIONID)
					IF etsql_con >0
						If Used('tibl_1')
							IF ((_mqty >tibl_1.wipqty) AND tibl_1.wipqty<>0)
								REPLACE u_Appack WITH '' IN item_vw
								=messagebox('Quantity could not be greater then '+ALLTRIM(str(tibl_1.wipqty,14,3)),0+64,VuMess)
								SELECT (malias)
								RETURN .F. 	
							endif
							USE IN tibl_1
						ENDIF
					ELSE
						SELECT (malias)
						RETURN .f.
					ENDIF
					USE IN tibl
				CASE main_vw.entry_ty='ST'
					STORE 0 TO _BalQty
					STORE item_vw.Entry_ty TO cEntryTy 
					Store item_vw.Tran_cd TO cTrancd 
					STORE item_vw.It_code TO cItCode 
					STORE item_vw.Itserial TO cItSerial
					*!* Pending Quantity
					SELECT A.Balqtynew Balqty FROM _detaildata A ;
					Inner join itref_vw B ON(A.Entry_ty = B.REntry_ty And A.Itserial = B.RItserial And A.It_code = B.It_code AND A.l_yn = B.rl_yn AND A.Inv_no = B.RInv_no AND A.Inv_Sr = B.rInv_sr);
					WHERE B.Entry_ty = cEntryTy AND B.Tran_cd = cTrancd AND B.It_code = cItCode AND B.Itserial = cItSerial ;
					INTO CURSOR PENDQTY 
					*!*
					SELECT PENDQTY
					STORE BalQty TO _Balqty
					IF _mqty >_BalQty AND _BalQTy #0
						REPLACE u_appack WITH '' IN item_vw
						=messagebox('Quantity could not be greater then '+ALLTRIM(str(_BalQTy,14,3)),0+64,VuMess)
						SELECT (malias)
						RETURN .F. 	
					ENDIF 
			ENDCASE  
		ENDIF
		REPLACE qty WITH _mqty IN item_vw
	ENDIF 
ENDIF 
sele(mAlias)
RETURN .T.
**&& Added On 22/02/2010 by Hetal L Patel Ed
***End OPST_CHK_AVGPACK()