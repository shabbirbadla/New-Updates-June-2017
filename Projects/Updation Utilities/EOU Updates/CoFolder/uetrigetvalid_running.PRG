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
		_mcond = "cType = 'RGPART1' And " + iif(CoAdditional.Cate_Srno," Ccate = Main_vw.Cate And ","")	
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
			_mcond = "cType = 'RGPART1' And " + iif(CoAdditional.Cate_Srno," Ccate = ?Main_vw.Cate And ","")	
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
						Cit_code WITH _mitcode,CGroup With _mitgrp,CChapno WITH _mitchap,Cittype WITH _mittype IN Gen_SrNo_Vw
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
		_mcond = "cType = 'RGPART1' And " + iif(CoAdditional.Cate_Srno," Ccate = Main_vw.Cate And ","")	
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
			_mcond = "cType = 'RGPART1' And " + iif(CoAdditional.Cate_Srno," Ccate = ?Main_vw.Cate And ","")	
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



PROC GEN_NO() &&RUP:-procedure is useful to generate part-ii,pla,are1,are2,are3..etc field in Daily Debit form UEFRM_ST_DAILTDEBIT in Sales Entry.
PARAMETERS fldnm,tblnm

LOCAL VRNO
sqlconobj=NEWOBJECT('sqlconnudobj',"sqlconnection",xapps)
nHandle=0
sq1="SELECT MAX(CAST( "+ALLTRIM(fldnm)+"  AS INT)) AS RNO  FROM "+ALLTRIM(tblnm)+" WHERE ISNUMERIC( "+ALLTRIM(fldnm)+" )=1"

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
sq1="SELECT "+FLDNM+" FROM "+TBLNM+" WHERE "+FLDNM+" = '"+ALLTRIM(FLDVAL)+"' AND NOT ("+TBLNM+".TRAN_CD="+STR(MAIN_VW.TRAN_CD)+" AND "+TBLNM+".ENTRY_TY='"+MAIN_VW.ENTRY_TY+"'"+IIF(FLDNM='U_PAGENO'," AND "+TBLNM+".ITSERIAL='"+ITEM_VW.ITSERAIL+"')",")")

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


PROC VAL_SERAPL()&&RUP:-procedure is useful to generate part-i-->pageno field.
	IF UPPER(AC_MAST_VW.SERAPL)='Y' 
		do form uefrm_ac_sertax WITH _SCREEN.ActiveForm.DataSessionId,_screen.activeform.addmode,_screen.activeform.editmode
	ENDIF
RETURN .T.
PROCEDURE bundle_valid1()
**************************start***************************************
	
		   				sq1="select * from bundle where bundle=item_vw.u_bundle"
		   				nRetval = sqlconobj.dataconn([EXE],company.dbname,sq1,"_xbundle","nHandle",_SCREEN.ActiveForm.DATASESSIONID)
			   			select main_vw
*!*				   			scat memv memo
				   		select _xbundle
*!*					   		set order to bi
				   		go top
*!*					   		set exact off
*!*							if reccount('_xbundle') <> 0
			   			scan while !eof()
						   		select item_vw
						   		replace item_vw.item 		with _xbundle.item 		
						   		replace item_vw.U_APPACK	with _xbundle.qty  		
						   		replace item_vw.rate		with 1					
						   		replace item_vw.u_wt 		with _xbundle.weight    
						   		replace item_vw.U_EXPDESC  with _xbundle.weight*u_appack
							    replace ITEM_VW.U_PIECES WITH item_VW.U_APPACK*BDLS  && first assign value & then made 0
								     IF EMPT(item_VW.U_WT)
								     ELSE
									 	REPLA item_VW.U_EXPDESC WITH item_VW.U_WT*item_VW.U_APPACK
								     ENDIF
								     IF EMPT(item_VW.U_PIECES)
									 	REPLA item_VW.U_SWT WITH (item_VW.U_EXPDESC/1000)
								     ELSE
									 	REPLA item_VW.U_SWT WITH (item_VW.U_EXPDESC*BDLS)/1000
								     ENDIF
						   			 REPLA item_VW.U_GWT WITH (item_VW.U_SWT+(item_VW.U_SWT*main_VW.U_GPER)/100)
								     IF !EMPT(item_VW.U_BOXWT)
									      REPLA item_VW.U_GROSSWT WITH round(item_VW.U_GWT+item_VW.U_BOXWT/1000,3)
								     ELSE
									      REPLA item_VW.U_GROSSWT WITH round(item_VW.U_GWT+BDLS/1000,3)
								     ENDIF
								     REPLA item_VW.QTY WITH item_VW.U_GWT
							         REPLA item_VW.U_PIECES WITH 0  && made 0 as it should be replaced only on last line
						   		select _xbundle
					   		endscan
					   		select item_vw
					   		&& replacement only on last line
							REPLA item_VW.U_PIECES WITH item_VW.U_APPACK*BDLS

					  		_screen.activeform.grditem.refresh()
	


****************************end***************************************



RETURN .t.


PROCEDURE bundle_valid()
SET STEP ON
	mAlias = Alias()
	nRetval=1
	nHandle=0
	_etDataSessionId = _Screen.Activeform.DataSessionId
	SET DATASESSION TO _etDataSessionId
	bun=main_vw.u_bunno
	sqlconobj=NEWOBJECT('sqlconnudobj','sqlconnection',xapps)
	nHandle=0
	sq1="SELECT * FROM BUNDLE WHERE BUNDLE=?bun" 
	nRetval = sqlconobj.dataconn([EXE],company.dbname,sq1,"EXCUR","nHandle",_SCREEN.ActiveForm.DATASESSIONID)
	replace item_vw.item WITH EXCUR.item
	replace item_vw.qty WITH EXCUR.qty
	replace item_vw.rate WITH 1
	If nRetval<0
		Return .F.
	ENDIF
*!*		replace item_vw.weight WITH TmpEt_Vw.weight
RETURN .t.