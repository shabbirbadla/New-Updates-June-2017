*:*****************************************************************************
*:        Program: UETRIGETVALID--Udyog ERP
*:        System : Udyog Software
*:        Author :
*: 		  Last modified:
*:		  AIM    : To Call function from Lother.dbf (val_con,whn_con,def_val) /Dcmast.dbf/frx files.
*:*****************************************************************************
PROCEDURE chk_pageno()
PARAMETERS mcommit,nhand	&& Added by Shrikant S. on 29/09/2010 for TKT-4021
_mrgret  = 0
_mrgpage = item_vw.u_pageno		&&FIELDS
&& Added by Shrikant S. on 29/09/2010 for TKT-4021		--Start
IF USED('Gen_SrNo_Vw')	&& 270910
	SELECT gen_srno_vw
	mrec=RECNO()
ENDIF
&& Added by Shrikant S. on 29/09/2010 for TKT-4021		--End
IF !EMPTY(_mrgpage)
*	_MRgRet  = -1  &&FIELDS
	_malias 	= ALIAS()
	SELE item_vw
	_mrecno 	= RECNO()
	etsql_con	= 1
	nhandle     = 0
	_etdatasessionid = _SCREEN.ACTIVEFORM.DATASESSIONID
	SET DATASESSION TO _etdatasessionid
	sqlconobj = NEWOBJECT('SqlConnUdObj','SqlConnection',xapps)

	IF !USED('Gen_SrNo_Vw')
		etsql_str = "Select * From Gen_SrNo where 1=0"
		etsql_con = sqlconobj.dataconn([EXE],company.dbname,etsql_str,[Gen_SrNo_Vw],;
			"nHandle",_etdatasessionid,.F.)
		IF etsql_con < 1 OR !USED("Gen_SrNo_Vw")
			etsql_con = 0
		ELSE
			SELECT gen_srno_vw
			INDEX ON itserial TAG itserial
		ENDIF
	ENDIF
	IF etsql_con > 0
		_mitcode 	= item_vw.it_code
		_mitgrp 	= ''
		_mitchap   	= ''
		_mitserial  = item_vw.itserial
		_mittype = ''
		_mitdate = item_vw.DATE  &&Sandeep 03/02/2011 for TKT-4596
		IF UPPE(coadditional.rg23_srno) = 'G' OR UPPE(coadditional.rg23_srno) = 'C'
			etsql_str = "Select Top 1 [Group],[ChapNo] From It_mast where It_code=?_mitcode"
			etsql_con = sqlconobj.dataconn([EXE],company.dbname,etsql_str,[TmpEt_Vw],;
				"nHandle",_etdatasessionid,.F.)
			IF etsql_con > 0 AND USED("TmpEt_Vw")
				_mitgrp 	= tmpet_vw.GROUP
				_mitchap   	= tmpet_vw.chapno
			ELSE
				etsql_con = 0
			ENDIF
		ENDIF

		IF UPPE(coadditional.rg23_srno) = 'D' &&rup
			etsql_str = "Select Top 1 [type] From It_mast where It_code=?_mitcode"
			etsql_con = sqlconobj.dataconn([EXE],company.dbname,etsql_str,[TmpEt1_Vw],;
				"nHandle",_etdatasessionid,.F.)
			IF etsql_con > 0 AND USED("TmpEt1_Vw")
				_mittype 	= tmpet1_vw.TYPE
			ELSE
				etsql_con = 0
			ENDIF
		ENDIF
	ENDIF
	IF etsql_con > 0
		_mcond = "l_yn='"+ALLTRIM(main_vw.l_yn)+"' and "+"cType = 'RGPART1' And " + IIF(coadditional.cate_srno," Ccate = Main_vw.Cate And ","")
		DO CASE
			CASE UPPE(coadditional.rg23_srno) = 'I'
				_mcond = _mcond+" Cit_code = _mitcode "
			CASE UPPE(coadditional.rg23_srno) = 'G'
				_mcond = _mcond+" Cgroup = _mitgrp "	&&Changes has been done by Vasant on 23/10/2012 as per Bug 6982 (Issue at the time of saving transaction).
			CASE UPPE(coadditional.rg23_srno) = 'C'
				_mcond = _mcond+" Cchapno = _mitchap "	&&Changes has been done by Vasant on 23/10/2012 as per Bug 6982 (Issue at the time of saving transaction).
			CASE UPPE(coadditional.rg23_srno) = 'D'
&&sandeep -->start 03/02/2011 for TKT-4596
				IF (_mittype # 'Machinery/Stores')
					_mittype='Raw material'
				ENDIF
				_mcond = _mcond+" Cittype = _mittype"   &&Sandeep<---end 03/02/2011 for TKT-4596	&&Changes has been done by Vasant on 23/10/2012 as per Bug 6982 (Issue at the time of saving transaction).

			OTHER
				_mcond = _mcond+" 1 = 1 "
		ENDCASE
		SELECT gen_srno_vw
		SCAN
			IF &_mcond
				&&Changes has been done by vasant on 17/11/2012 as per Bug 7184 (Issue in RG23 Sr.No. When the same Item is taken in Second item level and the RG23 Sr. No is edited).
				*IF gen_srno_vw.itserial != _mitserial AND ALLTRIM(gen_srno_vw.npgno) = ALLTRIM(_mrgpage)
				IF gen_srno_vw.itserial != _mitserial AND ALLTRIM(gen_srno_vw.npgno) == ALLTRIM(_mrgpage)
				&&Changes has been done by vasant on 17/11/2012 as per Bug 7184 (Issue in RG23 Sr.No. When the same Item is taken in Second item level and the RG23 Sr. No is edited).
					_mrgret  = 1
				ENDIF
			ENDIF
		ENDSCAN
		IF _mrgret != 1
			_mcond = "l_yn='"+ALLTRIM(main_vw.l_yn)+"' and cType = 'RGPART1' And " + IIF(coadditional.cate_srno," Ccate = ?Main_vw.Cate And ","")
			*_mcond = _mcond + " LTRIM(RTRIM(NPgNo)) = LTRIM(RTRIM(?_MRgPage)) And "	&&FIELDS	&&Changes has been done by vasant on 16/11/2012 as per Bug 7212 (Software should prompt message if RG23 part(I) No is higher).
			DO CASE
				CASE UPPE(coadditional.rg23_srno) = 'I'
					_mcond = _mcond+" Cit_code = ?_mitcode "
				CASE UPPE(coadditional.rg23_srno) = 'G'
					_mcond = _mcond+" Cgroup = ?_mitgrp "			&&Changes has been done by Vasant on 23/10/2012 as per Bug 6982 (Issue at the time of saving transaction).
				CASE UPPE(coadditional.rg23_srno) = 'C'
					_mcond = _mcond+" Cchapno = ?_mitchap "			&&Changes has been done by Vasant on 23/10/2012 as per Bug 6982 (Issue at the time of saving transaction).
				CASE UPPE(coadditional.rg23_srno) = 'D'
&&sandeep--->start 03/02/2011 for TKT-4596
					IF (_mittype # 'Machinery/Stores')
						_mittype='Raw material'
					ENDIF
					_mcond = _mcond+" Cittype = ?_mittype"  &&sandeep<--end 03/02/2011 for TKT-4596		&&Changes has been done by Vasant on 23/10/2012 as per Bug 6982 (Issue at the time of saving transaction).

				OTHER
					_mcond = _mcond+" 1 = 1 "
			ENDCASE

			&&Changes has been done by vasant on 16/11/2012 as per Bug 7212 (Software should prompt message if RG23 part(I) No is higher).
			_mcond = _mcond + " And "
			_mcond1 = _mcond + " [Date] > ?Main_vw.Date And Cast(NPgNo as Numeric(10)) < Cast(?_MRgPage as Numeric(10)) "
			_mcond = _mcond + " LTRIM(RTRIM(NPgNo)) = LTRIM(RTRIM(?_MRgPage)) "
			&&Changes has been done by vasant on 16/11/2012 as per Bug 7212 (Software should prompt message if RG23 part(I) No is higher).

			etsql_str = "Select Top 1 Entry_ty,Tran_cd,Itserial From Gen_SrNo where "+_mcond
			etsql_con = sqlconobj.dataconn([EXE],company.dbname,etsql_str,[TmpEt_Vw],;
				IIF(mcommit=.F.,"nHandle",nhand),_etdatasessionid,mcommit)	&& Added by Shrikant S. on 29/09/2010 for TKT-4021
*!*						"nHandle",_etdatasessionid,.F.)			&& Commented by Shrikant S. on 29/09/2010 for TKT-4021

			IF etsql_con > 0 AND USED("TmpEt_Vw")
				SELECT tmpet_vw
				&&Changes has been done by vasant on 17/11/2012 as per Bug 7184 (Issue in RG23 Sr.No. When the same Item is taken in Second item level and the RG23 Sr. No is edited).
				*IF RECCOUNT() > 0 AND entry_ty+STR(tran_cd)+itserial # main_vw.entry_ty+STR(main_vw.tran_cd)+_mitserial
				IF RECCOUNT() > 0 AND entry_ty+STR(tran_cd) # main_vw.entry_ty+STR(main_vw.tran_cd)
				&&Changes has been done by vasant on 17/11/2012 as per Bug 7184 (Issue in RG23 Sr.No. When the same Item is taken in Second item level and the RG23 Sr. No is edited).
					_mrgret  = 1
				ELSE
				    &&Changes has been done by vasant on 16/11/2012 as per Bug 7212 (Software should prompt message if RG23 part(I) No is higher).
					etsql_str = "Select Top 1 cType,[Date],NPgNo From Gen_SrNo where "+_mcond1
					etsql_con = sqlconobj.dataconn([EXE],company.dbname,etsql_str,[TmpEt_Vw],;
						IIF(mcommit=.F.,"nHandle",nhand),_etdatasessionid,mcommit)	&& Added by Shrikant S. on 29/09/2010 for TKT-4021
					IF etsql_con > 0 AND USED("TmpEt_Vw")
						SELECT tmpet_vw
						IF RECCOUNT() > 0
							=MESSAGEBOX('RG Part 1 No generated is higher than the RG Part 1 No generated on '+DTOC(TTOD(TmpEt_Vw.Date)),48,vumess)
						Endif	
					Endif		
					&&Changes has been done by vasant on 16/11/2012 as per Bug 7212 (Software should prompt message if RG23 part(I) No is higher).

					SELECT gen_srno_vw
					LOCATE FOR itserial = _mitserial
					IF !FOUND()
						APPEND BLANK IN gen_srno_vw
					ENDIF
					IF INLIST(ctype,"RGPART1") OR EMPTY(ctype)		&& Added by Shrikant S. on 29/09/2010 for TKT-4021
						REPLACE ccate WITH main_vw.cate,npgno WITH _mrgpage,;
							itserial WITH item_vw.itserial,cware WITH item_vw.ware_nm,ctype WITH 'RGPART1',;
							cit_code WITH _mitcode,cgroup WITH _mitgrp,cchapno WITH _mitchap,cittype WITH _mittype,l_yn WITH main_vw.l_yn IN gen_srno_vw
					ENDIF
					_mrgret  = 0
				ENDIF
			ENDIF
		ENDIF
	ENDIF
	IF USED("TmpEt_Vw")
		USE IN tmpet_vw
	ENDIF
	=sqlconobj.sqlconnclose("nHandle")
	SELE item_vw
	IF BETW(_mrecno,1,RECCOUNT())
		GO _mrecno
	ENDIF
	IF !EMPTY(_malias)
		SELECT &_malias
	ENDIF
ENDIF
RETURN IIF(_mrgret = 0,.T.,.F.)
*******************************************************************************************************

PROCEDURE gen_pageno()
_mrgpage = item_vw.u_pageno		&&FIELDS
IF EMPTY(_mrgpage)
	_malias 	= ALIAS()
	SELE item_vw
	_mrecno 	= RECNO()
	etsql_con	= 1
	nhandle     = 0
	_etdatasessionid = _SCREEN.ACTIVEFORM.DATASESSIONID
	SET DATASESSION TO _etdatasessionid
	sqlconobj = NEWOBJECT('SqlConnUdObj','SqlConnection',xapps)

&&-->Rup  12Aug09
	etsql_str = "select tp=[type] from it_mast where it_code="+STR(item_vw.it_code)
	etsql_con = sqlconobj.dataconn([EXE],company.dbname,etsql_str,[itype],"nHandle",_etdatasessionid,.F.)
	IF etsql_con < 1 OR !USED("itype")
		etsql_con = 0
	ELSE
		SELECT itype
		IF INLIST(itype.tp,'Finished','Semi Finished')
			IF TYPE('main_vw.u_gcssr')='L'
				IF main_vw.u_gcssr=.F.
					RETURN ''
				ENDIF
			ELSE
				RETURN ''
			ENDIF
		ENDIF
		IF itype.tp='Trading'
			RETURN ''
		ENDIF
	ENDIF
&&<--Rup  12Aug09

	IF !USED('Gen_SrNo_Vw')
		etsql_str = "Select * From Gen_SrNo where 1=0"
		etsql_con = sqlconobj.dataconn([EXE],company.dbname,etsql_str,[Gen_SrNo_Vw],;
			"nHandle",_etdatasessionid,.F.)
		IF etsql_con < 1 OR !USED("Gen_SrNo_Vw")
			etsql_con = 0
		ELSE
			SELECT gen_srno_vw
			INDEX ON itserial TAG itserial
		ENDIF
	ENDIF
	IF etsql_con > 0
		_mitcode 	= item_vw.it_code
		_mitgrp 	= ''
		_mitchap   	= ''
		_mitdate 	=item_vw.DATE  &&sandeep  03/02/2011 for TKT-4596
		IF UPPE(coadditional.rg23_srno) = 'G' OR UPPE(coadditional.rg23_srno) = 'C'
			etsql_str = "Select Top 1 [Group],[ChapNo] From It_mast where It_code=?_mitcode"
			etsql_con = sqlconobj.dataconn([EXE],company.dbname,etsql_str,[TmpEt_Vw],;
				"nHandle",_etdatasessionid,.F.)
			IF etsql_con > 0 AND USED("TmpEt_Vw")
				_mitgrp 	= tmpet_vw.GROUP
				_mitchap   	= tmpet_vw.chapno
			ELSE
				etsql_con = 0
			ENDIF
		ENDIF
	ENDIF
	IF UPPE(coadditional.rg23_srno) = 'D' &&rup
		etsql_str = "Select Top 1 [type] From It_mast where It_code=?_mitcode"
		etsql_con = sqlconobj.dataconn([EXE],company.dbname,etsql_str,[TmpEt1_Vw],;
			"nHandle",_etdatasessionid,.F.)
		IF etsql_con > 0 AND USED("TmpEt1_Vw")
			_mittype 	= tmpet1_vw.TYPE
		ELSE
			etsql_con = 0
		ENDIF
	ENDIF


	IF etsql_con > 0
		_mcond ="l_yn='"+ALLTRIM(main_vw.l_yn)+"' and cType = 'RGPART1' And " + IIF(coadditional.cate_srno," Ccate = Main_vw.Cate And ","")
		DO CASE
			CASE UPPE(coadditional.rg23_srno) = 'I'
				_mcond = _mcond+" Cit_code = _mitcode "
			CASE UPPE(coadditional.rg23_srno) = 'G'
				_mcond = _mcond+" Cgroup = _mitgrp "		&&Changes has been done by Vasant on 23/10/2012 as per Bug 6982 (Issue at the time of saving transaction).
			CASE UPPE(coadditional.rg23_srno) = 'C'
				_mcond = _mcond+" Cchapno = _mitchap "			&&Changes has been done by Vasant on 23/10/2012 as per Bug 6982 (Issue at the time of saving transaction).
			CASE UPPE(coadditional.rg23_srno) = 'D' &&rup
&&sandeep--->start 03/02/2011 for TKT-4596
				IF (_mittype # 'Machinery/Stores')
					_mittype='Raw material'
				ENDIF
				_mcond = _mcond+" cittype = _mittype "   &&sandeep<--end 03/02/2011 for TKT-4596		&&Changes has been done by Vasant on 23/10/2012 as per Bug 6982 (Issue at the time of saving transaction).

			OTHER
				_mcond = _mcond+" 1 = 1 "
		ENDCASE
		
		&&Changes has been done by vasant on 17/11/2012 as per Bug 7184 (Issue in RG23 Sr.No. When the same Item is taken in Second item level and the RG23 Sr. No is edited).
*!*			SELECT gen_srno_vw
*!*			SCAN
*!*				IF &_mcond
*!*					IF ALLTRIM(_mrgpage) <= ALLT(gen_srno_vw.npgno)
*!*						_mrgpage = ALLTRIM(STR(IIF(ISNULL(gen_srno_vw.npgno),0,VAL(gen_srno_vw.npgno)) + 1))
*!*					ENDIF
*!*				ENDIF
*!*				SELECT gen_srno_vw
*!*			ENDSCAN

		Select Max(Cast(nPgNo as int)) as PageNo From gen_srno_vw WITH (buffering = .t.) ;
			where &_mcond INTO CURSOR TmpEt_Vw
		_mrgpage1=0	
		IF USED('TmpEt_Vw')	
			IF RECCOUNT('TmpEt_Vw') > 0
				_mrgpage1=IIF(ISNULL(TmpEt_Vw.PageNo),0,TmpEt_Vw.PageNo)
				IF _mrgpage1 !=0
					_mrgpage = ALLTRIM(STR(_mrgpage1 + 1))
				ENDIF
			Endif	
		Endif
		&&Changes has been done by vasant on 17/11/2012 as per Bug 7184 (Issue in RG23 Sr.No. When the same Item is taken in Second item level and the RG23 Sr. No is edited).

		&&IF EMPTY(_mrgpage)
			_mcond ="l_yn='"+ALLTRIM(main_vw.l_yn)+"' and cType = 'RGPART1' And " + IIF(coadditional.cate_srno," Ccate = ?Main_vw.Cate And ","")
			DO CASE
				CASE UPPE(coadditional.rg23_srno) = 'I'
					_mcond = _mcond+" Cit_code = ?_mitcode "
				CASE UPPE(coadditional.rg23_srno) = 'G'
					_mcond = _mcond+" Cgroup = ?_mitgrp "			&&Changes has been done by Vasant on 23/10/2012 as per Bug 6982 (Issue at the time of saving transaction).
				CASE UPPE(coadditional.rg23_srno) = 'C'
					_mcond = _mcond+" Cchapno = ?_mitchap "			&&Changes has been done by Vasant on 23/10/2012 as per Bug 6982 (Issue at the time of saving transaction).
				CASE UPPE(coadditional.rg23_srno) = 'D' &&rup
&&sandeep--->start 03/02/2011 for TKT-4596
					IF (_mittype # 'Machinery/Stores')
						_mittype='Raw material'
						_mcond = _mcond+" Cittype = ?_mittype" &&sandeep<--end 03/02/2011 for TKT-4596			&&Changes has been done by Vasant on 23/10/2012 as per Bug 6982 (Issue at the time of saving transaction).
					ENDIF


				OTHER
					_mcond = _mcond+" 1 = 1 "
			ENDCASE
			_mrgpage1=0		&&added by satish pal for bug-6425 dated 17/09/2012
			etsql_str = "Select Max(Cast(nPgNo as int)) as PageNo From Gen_SrNo where "+_mcond
			etsql_con = sqlconobj.dataconn([EXE],company.dbname,etsql_str,[TmpEt_Vw],;
				"nHandle",_etdatasessionid,.F.)
			IF etsql_con > 0 AND USED("TmpEt_Vw")
				&&_mrgpage = ALLTRIM(STR(IIF(ISNULL(tmpet_vw.PAGENO),0,tmpet_vw.PAGENO) + 1))&&commented by satish pal bug-6425 dated 17/09/2012
				_mrgpage1 = ALLTRIM(STR(IIF(ISNULL(tmpet_vw.PAGENO),0,tmpet_vw.PAGENO) + 1)) 	&&added by satish pal dated 17/09/2012 bug-6425 for get max no of rgpage
			ENDIF
			_mrgpage =IIF(_mrgpage1>_mrgpage,_mrgpage1,_mrgpage)	&&added by satish pal dated 17/09/2012 for get max no of rgpage
		&&ENDIF
	ENDIF
	IF etsql_con <= 0
		_mrgpage = '***'
	ENDIF
	IF USED("TmpEt_Vw")
		USE IN tmpet_vw
	ENDIF
	=sqlconobj.sqlconnclose("nHandle")
	SELE item_vw
	IF BETW(_mrecno,1,RECCOUNT())
		GO _mrecno
	ENDIF
	&&Changes has been done by vasant on 17/11/2012 as per Bug 7230 (RG 23 Part 1 no is not generating error).
	_mrgpage = PADR(_mrgpage,LEN(item_vw.u_pageno))
	IF USED('Gen_SrNo_Vw') AND etsql_con > 0 AND !EMPTY(_mrgpage)
		SELECT gen_srno_vw
		LOCATE FOR itserial = item_vw.itserial AND ALLTRIM(ctype) == "RGPART1"
		IF !FOUND()
			APPEND BLANK IN gen_srno_vw
		ENDIF
		REPLACE ccate WITH main_vw.cate,npgno WITH _mrgpage,;
			itserial WITH item_vw.itserial,cware WITH item_vw.ware_nm,ctype WITH 'RGPART1',;
			cit_code WITH _mitcode,cgroup WITH _mitgrp,cchapno WITH _mitchap,cittype WITH _mittype,l_yn WITH main_vw.l_yn IN gen_srno_vw
	Endif
	&&Changes has been done by vasant on 17/11/2012 as per Bug 7230 (RG 23 Part 1 no is not generating error).
	IF !EMPTY(_malias)
		SELECT &_malias
	ENDIF

	*_mrgpage = PADR(_mrgpage,LEN(item_vw.u_pageno))		&&FIELDS	&&Changes has been done by vasant on 17/11/2012 as per Bug 7230 (RG 23 Part 1 no is not generating error).
ENDIF
RETURN _mrgpage
*******************************************************************************************************


**Start ST_CHK_APPACK() --TKT 495
PROC st_chk_appack() &&RUP:-procedure is useful to update default value of  average qty,qty,mrprate,abtper etc. as per it_mast values
malias = ALIAS()
LOCAL vappack,vmrprate,vabtper
sqlconobj=NEWOBJECT('sqlconnudobj',"sqlconnection",xapps)
nhandle=0
sq1="SELECT average,mrprate,abtper FROM IT_MAST WHERE IT_CODE ="+STR(item_vw.it_code)
nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"EXCUR","nHandle",_SCREEN.ACTIVEFORM.DATASESSIONID)
IF nretval<0
	RETURN .F.
ENDIF
SELECT excur
vappack=IIF(!ISNULL(excur.AVERAGE),excur.AVERAGE,0)
vmrprate=IIF(!ISNULL(excur.mrprate),excur.mrprate,0)
vabtper=IIF(!ISNULL(excur.abtper),excur.abtper,0)
IF USED("EXCUR")
	USE IN excur
ENDIF

REPLACE u_appack WITH IIF(item_vw.u_appack=' ',ALLTRIM(STR(vappack)),item_vw.u_appack),u_mrprate WITH IIF(item_vw.u_mrprate=0,vmrprate,item_vw.u_mrprate),abtper WITH IIF(item_vw.abtper=0,vabtper,item_vw.abtper) IN item_vw
_mqty = 0
IF VAL(item_vw.u_pkno)#0 AND VAL(item_vw.u_appack)#0
*!*		REPLACE qty WITH (VAL(item_vw.u_pkno) * VAL(item_vw.u_appack)) IN item_vw &&Comment By Hetal Dt 19/02/2010
	_mqty = (VAL(item_vw.u_pkno) * VAL(item_vw.u_appack))
ENDIF
***Added By Hetal Dt 19/02/2010 Check DC QTY &&St
IF _mqty # 0
	IF  (([vuexc] $ vchkprod) OR ([vuinv] $ vchkprod)) AND TYPE('main_vw.entry_ty')='C' &&Check Existing Records
*!*			IF main_vw.entry_ty='ST'
		IF main_vw.entry_ty='ST' AND USED('_DetailData')	&& Changed By Sachin N. S. on 28/05/2010 for TKT-2055
			STORE 0 TO _balqty
			STORE item_vw.entry_ty TO centryty
			STORE item_vw.tran_cd TO ctrancd
			STORE item_vw.it_code TO citcode
			STORE item_vw.itserial TO citserial
*!* Pending Quantity
			SELECT a.balqtynew balqty FROM _detaildata a ;
				inner JOIN itref_vw b ON(a.entry_ty = b.rentry_ty AND a.itserial = b.ritserial AND a.it_code = b.it_code AND a.l_yn = b.rl_yn AND a.inv_no = b.rinv_no AND a.inv_sr = b.rinv_sr);
				WHERE b.entry_ty = centryty AND b.tran_cd = ctrancd AND b.it_code = citcode AND b.itserial = citserial ;
				INTO CURSOR pendqty
*!*
			SELECT pendqty
			STORE balqty TO _balqty
			IF _mqty >_balqty AND _balqty #0
				REPLACE u_pkno WITH '' IN item_vw
				=MESSAGEBOX('Quantity could not be greater then '+ALLTRIM(STR(_balqty,14,3)),0+64,vumess)
				SELECT (malias)
				RETURN .F.
			ENDIF
		ENDIF
	ENDIF
	REPLACE qty WITH _mqty IN item_vw
ENDIF
***Added By Hetal Dt 19/02/2010 Check DC QTY &&Ed
SELE(malias)
RETURN .T.
***End ST_CHK_APPACK()

**Start OP_CHK_APPACK() TKT 479
PROC op_chk_appack() &&RUP:-procedure is useful to update default value of average qty,qty etc. as per it_mast values && 04Oct09
malias = ALIAS()

LOCAL vappack,vmrprate,vabtper
sqlconobj=NEWOBJECT('sqlconnudobj',"sqlconnection",xapps)
nhandle=0

sq1="SELECT average FROM IT_MAST WHERE IT_CODE ="+STR(item_vw.it_code)
nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"EXCUR","nHandle",_SCREEN.ACTIVEFORM.DATASESSIONID)
IF nretval<0
	RETURN .F.
ENDIF

SELECT excur
vappack=IIF(!ISNULL(excur.AVERAGE),excur.AVERAGE,0)

IF USED("EXCUR")
	USE IN excur
ENDIF

REPLACE u_appack WITH IIF(item_vw.u_appack=' ',ALLTRIM(STR(vappack)),item_vw.u_appack) IN item_vw
_mqty = 0
IF VAL(item_vw.u_pkno)#0 AND VAL(item_vw.u_appack)#0
*!*		REPLACE qty WITH (VAL(item_vw.u_pkno) * VAL(item_vw.u_appack)) IN item_vw &&Changed By Hetal Dt 18/02/2010
	_mqty = (VAL(item_vw.u_pkno) * VAL(item_vw.u_appack))
ENDIF

***Added By Hetal Dt 18/02/2010 Check IP QTY &&St
IF _mqty # 0
	IF  (([vuexc] $ vchkprod) OR ([vuinv] $ vchkprod)) AND TYPE('main_vw.entry_ty')='C' &&Check Existing Records
*!*			If main_vw.entry_ty='OP'		&& Changed By Sachin N. S. on 01/02/2011 for TKT-5729
		IF main_vw.entry_ty='OP' AND USED('Projectitref_vw')
			nretval = 0
			nhandle = 0
			etsql_con  = 0
			nhandle    = 0
			SELECT aentry_ty,atran_cd,aitserial,qty FROM projectitref_vw WHERE entry_ty=main_vw.entry_ty AND tran_cd=main_vw.tran_cd AND itserial=item_vw.itserial INTO CURSOR tibl
			etsql_str  = ""
			etsql_str = "USP_ENT_CHK_OP_ALLOCATION '"+item_vw.entry_ty+"',"+ALLTRIM(STR(item_vw.tran_cd))+","+ALLTRIM(STR(item_vw.it_code))+",'"+item_vw.itserial+"','";
				+tibl.aentry_ty+"',"+ALLTRIM(STR(tibl.atran_cd))+",'"+tibl.aitserial+"'"
			etsql_con = sqlconobj.dataconn([EXE],company.dbname,etsql_str,[tibl_1],"nHandle",_SCREEN.ACTIVEFORM.DATASESSIONID)
			IF etsql_con >0
				IF USED('tibl_1')
					IF ((_mqty >tibl_1.wipqty) AND tibl_1.wipqty<>0)
						REPLACE u_pkno WITH '' IN item_vw
						=MESSAGEBOX('Quantity could not be greater then '+ALLTRIM(STR(tibl_1.wipqty,14,3)),0+64,vumess)
						SELECT (malias)
						RETURN .F.
					ENDIF
					USE IN tibl_1
				ENDIF
			ELSE
				SELECT (malias)
				RETURN .F.
			ENDIF
			USE IN tibl
		ENDIF
	ENDIF
	REPLACE qty WITH _mqty IN item_vw
ENDIF
***Added By Hetal Dt 18/02/2010 Check IP QTY &&Ed
SELE(malias)
RETURN .T.
***END OP_CHK_APPACK()

*/*/*/*/*/*/*

*&&Added by Amrendra on on 30/03/2011 for TKT 6785       -- Start
PROCEDURE getabetment() &&AKS:-procedure is useful to update default value of abtment % and MRP Rate in Sales
LOCAL vmrprate,vabtper,cabtper
malias = ALIAS()  && Added by Amrendra for TKT 7333 on 19/05/2011
sqlconobj=NEWOBJECT('sqlconnudobj',"sqlconnection",xapps)
nhandle=0
sq1="select mrprate,abtper from it_mast WHERE IT_CODE ="+STR(item_vw.it_code)
nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"EXCUR","nHandle",_SCREEN.ACTIVEFORM.DATASESSIONID)
IF nretval<0
	RETURN .F.
ENDIF
SELECT excur
vmrprate=IIF(!ISNULL(excur.mrprate),excur.mrprate,0)
*cabtper=Iif(Iif(Isnull(coadditional.abtper),0,coadditional.abtper)>=100,0,100-coadditional.abtper) && Commented by Amrendra for TKT 7333 on 19/05/2011
cabtper=IIF(IIF(ISNULL(coadditional.abtper),0,coadditional.abtper)>=100,0,IIF(coadditional.abtper=0,0,100-coadditional.abtper)) && Added by Amrendra for TKT 7333 on 19/05/2011
vabtper=IIF(!ISNULL(excur.abtper),excur.abtper,0)
vabtper=IIF(vabtper#0,vabtper,cabtper)

IF item_vw.u_mrprate=0
	REPLACE u_mrprate WITH vmrprate IN item_vw
ENDIF
IF item_vw.abtper=0
	REPLACE abtper WITH vabtper IN item_vw
ENDIF
IF USED("EXCUR")
	USE IN excur
ENDIF
frmxtra.txtabtper.REFRESH
frmxtra.txtu_mrprate.REFRESH
SELECT(malias) && Added by Amrendra for TKT 7333 on 19/05/2011
RETURN .T.
*&&Added by Amrendra on on 30/03/2011 for TKT 6785       -- End

*/*/*/*/*/*/*


PROC st_ass_whn() &&RUP:-procedure is useful to update default value of assessable amount with/without (mrprate and abtper),round-off value in Sales

*!*	**********Commented by Amrendra on on 28/03/2011 for TKT 6785   ------Start
*!*	&& Added By Shrikant S. on 25/02/2011 for TKT-4580	***	Start
*!*	Local vmrprate,vabtper,cabtper
*!*	cabtper=0
*!*	sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)
*!*	nhandle=0

*!*	sq1="select mrprate,abtper from it_mast WHERE IT_CODE ="+Str(item_vw.it_code)
*!*	nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"EXCUR","nHandle",_Screen.ActiveForm.DataSessionId)
*!*	If nretval<0
*!*		Return .F.
*!*	Endif
*!*	Select excur
*!*	vmrprate=Iif(!Isnull(excur.mrprate),excur.mrprate,0)
*!*	cabtper=Iif(Iif(Isnull(coadditional.abtper),0,coadditional.abtper)>=100,0,100-coadditional.abtper)
*!*	vabtper=Iif(!Isnull(excur.abtper),excur.abtper,0)
*!*	vabtper=Iif(vabtper#0,vabtper,cabtper)
*!*	Replace u_mrprate With vmrprate In item_vw
*!*	Replace abtper With vabtper In item_vw

*!*	If Used("EXCUR")
*!*		Use In excur
*!*	Endif
*!*	&& Added By Shrikant S. on 25/02/2011 for TKT-4580	***	End
*!*	*********Commented by Amrendra on on 28/03/2011 for TKT 6785   ------End
SELECT item_vw

IF item_vw.u_mrprate#0
	SELE item_vw
	IF item_vw.abtper#0
		REPL u_asseamt WITH ROUND((qty*u_mrprate)-(qty*u_mrprate*abtper)/100,2) IN item_vw
	ELSE
		REPL u_asseamt WITH ROUND((qty*u_mrprate),2) IN item_vw
	ENDIF

*!*	** Commented by Amrendra+Rupesh on 06/05/2011 for TKT 7333 Start
*!*	    Repl rate With Iif(rate=0,Round(u_mrprate-((u_mrprate*abtper)/100),2),rate) In item_vw  &&Commented by Amrendra on on 28/03/2011 for TKT 6785
*!*	***	Repl rate With Round(u_mrprate-((u_mrprate*abtper)/100),2) In item_vw   &&Added by Amrendra on on 28/03/2011 for TKT 6785
*!*	** Commented by Amrendra+Rupesh on 06/05/2011 for TKT 7333 End
ELSE
	REPL u_asseamt WITH ROUND(qty*rate,2) IN item_vw
ENDIF


IF coadditional.rndavalue
	REPLACE u_asseamt WITH ROUND(u_asseamt,0) IN item_vw
ENDIF

frmxtra.txtu_asseamt.REFRESH

***&&Added by Amrendra on on 28/03/2011 for TKT 6785       -- Start
oform=_SCREEN.ACTIVEFORM.fobject
oform.itemgrdbefcalc(1)
***&&Added by Amrendra on on 28/03/2011 for TKT 6785        -- End

RETURN .T.




PROC pt_ass_whn() &&RUP:-procedure is useful to update default value of assessable amount in Purchase entry
SELE item_vw
REPL u_asseamt WITH rate*qty  IN item_vw
RETU



PROC pt_rate_def() &&RUP:-procedure is useful to update default value of rate calculation as per ass.value entred by user  in Purchase entry
SELE item_vw
REPL rate WITH (u_asseamt/qty) IN item_vw
RETU .T.

&&changes done as per TKT-3954
*!*	PROC chk_chapno() &&RUP:-procedure is useful to Check ChapNo in  Item Master. 09/09/09
*!*		PARAMETERS vchapno
*!*		vchapno=ALLTRIM(vchapno)
*!*		visdigit=.T.
*!*		IF LEN(vchapno)<>8
*!*			visdigit=.F.
*!*		ELSE
*!*			FOR i=1 TO LEN(vchapno)
*!*				IF !ISDIGIT(SUBSTR(vchapno,i,1))
*!*					visdigit=.F.
*!*					EXIT
*!*				ENDIF
*!*			NEXT i
*!*		ENDIF
*!*		RETU visdigit
&&changes done as per TKT-3954

&& Commented by Shrikant S. on 29/09/2010 for TKT-4021
*!*	PROC gen_no() &&RUP:-procedure is useful to generate part-ii,pla,are1,are2,are3..etc field in Daily Debit form UEFRM_ST_DAILTDEBIT in Sales Entry.
*!*		PARAMETERS fldnm,tblnm

*!*		LOCAL vrno
*!*		sqlconobj=NEWOBJECT('sqlconnudobj',"sqlconnection",xapps)
*!*		nhandle=0
*!*		*!*	sq1="SELECT MAX(CAST( "+ALLTRIM(fldnm)+"  AS INT)) AS RNO  FROM "+ALLTRIM(tblnm)+" WHERE ISNUMERIC( "+ALLTRIM(fldnm)+" )=1 and l_yn='"+ALLTRIM(main_vw.l_yn)+"' " && Commented by Shrikant S. on 25/05/2010 for TKT-1986
*!*		sq1="SELECT MAX(CAST( "+ALLTRIM(fldnm)+"  AS INT)) AS RNO  FROM "+ALLTRIM(tblnm)+" WHERE ISNUMERIC( "+ALLTRIM(fldnm)+" )=1 and l_yn='"+ALLTRIM(main_vw.l_yn)+"' "+ IIF(coadditional.cate_srno," AND cate = ?Main_vw.Cate ","")

*!*		nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"EXCUR","nHandle",_SCREEN.ACTIVEFORM.DATASESSIONID)
*!*		IF nretval<0
*!*			RETURN .F.
*!*		ENDIF
*!*		SELECT excur
*!*		vrno=ALLTRIM(STR(IIF(ISNULL(excur.rno),1,(excur.rno)+1)))
*!*		IF USED("EXCUR")
*!*			USE IN excur
*!*		ENDIF
*!*		*sele(mAlias)
*!*		RETURN vrno

&& Added by Shrikant S. on 29/09/2010 for TKT-4021		--Start
PROC gen_no() &&RUP:-procedure is useful to generate part-ii,pla and in Daily Debit form UEFRM_ST_DAILTDEBIT in Sales Entry.
PARAMETERS fldnm,tblnm,mcommit,nhand
pgno='Main_Vw.'+fldnm

LOCAL vrno
sqlconobj=NEWOBJECT('sqlconnudobj',"sqlconnection",xapps)
nhandle=0

IF !USED('Gen_SrNo_Vw')
	etsql_str = "Select * From Gen_SrNo where 1=0"
	etsql_con = sqlconobj.dataconn([EXE],company.dbname,etsql_str,[Gen_SrNo_Vw],;
		"nHandle",_SCREEN.ACTIVEFORM.DATASESSIONID,.F.)
	IF etsql_con < 1 OR !USED("Gen_SrNo_Vw")
		etsql_con = 0
	ELSE
		SELECT gen_srno_vw
		INDEX ON itserial TAG itserial
	ENDIF
ENDIF

sq1="SELECT MAX(CAST( Npgno AS INT)) AS RNO  FROM Gen_srno WHERE ISNUMERIC( NpgNo )=1 "+IIF(UPPER(fldnm)=='U_RG23NO'," AND CTYPE='RGAPART2'",IIF(UPPER(fldnm)=='U_RG23CNO'," AND CTYPE='RGCPART2'",IIF(UPPER(fldnm)=='U_PLASR'," AND CTYPE='PLASRNO'","")))+" and l_yn='"+ALLTRIM(main_vw.l_yn)+"' "+ IIF(coadditional.cate_srno," AND cate = ?Main_vw.Cate ","")

*nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"EXCUR","nHandle",_Screen.ActiveForm.DataSessionId)
nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"EXCUR",IIF(mcommit=.F.,"nHandle",nhand),_SCREEN.ACTIVEFORM.DATASESSIONID)
IF nretval<0
	RETURN .F.
ENDIF
SELECT excur
vrno=ALLTRIM(STR(IIF(ISNULL(excur.rno),1,(excur.rno)+1)))

=sqlconobj.sqlconnclose("nHandle")
IF USED("EXCUR")
	USE IN excur
ENDIF

RETURN vrno
&& Added by Shrikant S. on 29/09/2010 for TKT-4021		--End

&& Commented by Shrikant S. on 29/09/2010 for TKT-4021		--Start
*!*	PROCEDURE dup_no &&RUP:-procedure is useful to check duplicate value of part-ii,pla,are1,are2,are3..etc field in Daily Debit form UEFRM_ST_DAILTDEBIT in Sales Entry.
*!*		PARAMETERS fldnm,fldval,tblnm
*!*		_malias 	= ALIAS()
*!*		_mrecno	= RECNO()

*!*		*mAlias = Alias()
*!*		LOCAL vdup
*!*		IF fldval=' '
*!*			RETURN .T.
*!*		ENDIF

*!*		sqlconobj=NEWOBJECT('sqlconnudobj',"sqlconnection",xapps)
*!*		nhandle=0
*!*		*!*	sq1="SELECT "+FLDNM+" FROM "+TBLNM+" WHERE l_yn='"+ALLTRIM(main_vw.l_yn)+"' and "+FLDNM+" = '"+ALLTRIM(FLDVAL)+"' AND NOT ("+TBLNM+".TRAN_CD="+STR(MAIN_VW.TRAN_CD)+" AND "+TBLNM+".ENTRY_TY='"+MAIN_VW.ENTRY_TY+"'"+IIF(FLDNM='U_PAGENO'," AND "+TBLNM+".ITSERIAL='"+ITEM_VW.ITSERAIL+"')",")") && Commented by Shrikant S. on 25/05/2010 for TKT-1986
*!*		sq1="SELECT "+fldnm+" FROM "+tblnm+" WHERE l_yn='"+ALLTRIM(main_vw.l_yn)+"' and "+fldnm+" = '"+ALLTRIM(fldval)+"' AND NOT ("+tblnm+".TRAN_CD="+STR(main_vw.tran_cd)+" AND "+tblnm+".ENTRY_TY='"+main_vw.entry_ty+"'"+IIF(fldnm='U_PAGENO'," AND "+tblnm+".ITSERIAL='"+item_vw.itserail+"')",")")+ IIF(coadditional.cate_srno," AND cate = ?Main_vw.Cate ","")

*!*		nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"EXCUR","nHandle",_SCREEN.ACTIVEFORM.DATASESSIONID)
*!*		IF nretval<0
*!*			RETURN .F.
*!*		ENDIF

*!*		SELECT excur
*!*		vrcount=RECCOUNT()
*!*		IF USED("EXCUR")
*!*			USE IN excur
*!*		ENDIF

*!*		IF !EMPTY(_malias)
*!*			SELECT &_malias
*!*		ENDIF

*!*		IF BETW(_mrecno,1,RECCOUNT())
*!*			GO _mrecno
*!*		ENDIF

*!*		IF vrcount>0 AND !ISNULL(vrcount)
*!*			RETURN .F.
*!*		ELSE
*!*			RETURN .T.
*!*		ENDIF

*!*		*sele(mAlias)

*!*		RETURN

&& Added by Shrikant S. on 29/09/2010 for TKT-4021		--Start
PROCEDURE dup_no &&RUP:-procedure is useful to check duplicate value of part-ii,pla field and in Daily Debit form UEFRM_ST_DAILTDEBIT in Sales Entry.
PARAMETERS fldnm,fldval,tblnm,mcommit,nhand
pgno='Main_Vw.'+fldnm
IF !EMPTY(fldval)
	_malias 	= ALIAS()
	_mrecno	= RECNO()
	notype =IIF(UPPER(fldnm)=='U_RG23NO','RGAPART2',IIF(UPPER(fldnm)=='U_RG23CNO','RGCPART2',IIF(UPPER(fldnm)=='U_PLASR','PLASRNO',"")))
	LOCAL vdup
	IF fldval=' '
		RETURN .T.
	ENDIF
	sqlconobj=NEWOBJECT('sqlconnudobj',"sqlconnection",xapps)

	nhandle=0
	IF USED('Gen_SrNo_Vw')
		SELECT gen_srno_vw
		mrec=RECNO()
	ENDIF
	IF !USED('Gen_SrNo_Vw')
		etsql_str = "Select * From Gen_SrNo where 1=0"
		etsql_con = sqlconobj.dataconn([EXE],company.dbname,etsql_str,[Gen_SrNo_Vw],;
			"nHandle",_SCREEN.ACTIVEFORM.DATASESSIONID,.F.)
		IF etsql_con < 1 OR !USED("Gen_SrNo_Vw")
			etsql_con = 0
		ELSE
			SELECT gen_srno_vw
			INDEX ON itserial TAG itserial
		ENDIF
	ENDIF
	sq1="SELECT NpgNo FROM Gen_Srno WHERE l_yn='"+ALLTRIM(main_vw.l_yn)+"' "+IIF(UPPER(fldnm)=='U_RG23NO'," AND CTYPE='RGAPART2'",IIF(UPPER(fldnm)=='U_RG23CNO'," AND CTYPE='RGCPART2'",IIF(UPPER(fldnm)=='U_PLASR'," AND CTYPE='PLASRNO'","")))+" AND NpgNo='"+ALLTRIM(fldval)+"' AND NOT (TRAN_CD="+STR(main_vw.tran_cd)+" AND ENTRY_TY='"+main_vw.entry_ty+"'"+IIF(fldnm='U_PAGENO'," AND "+tblnm+".ITSERIAL='"+item_vw.itserial+"')",")")+ IIF(coadditional.cate_srno," AND cate = ?Main_vw.Cate ","")
	nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"EXCUR",IIF(mcommit=.F.,"nHandle",nhand),_SCREEN.ACTIVEFORM.DATASESSIONID)	&&280910

	IF nretval<0
		RETURN .F.
	ENDIF
	SELECT excur
	vrcount=RECCOUNT()
	IF nretval >0 AND USED("EXCUR")
		IF	vrcount<=0
			SELECT gen_srno_vw
			GO TOP
			LOCATE FOR entry_ty=main_vw.entry_ty AND tran_cd=main_vw.tran_cd AND (ctype=notype)
			IF !FOUND()
				APPEND BLANK IN gen_srno_vw
			ENDIF

			IF INLIST(ctype,"RGAPART2","RGCPART2","PLASRNO") OR EMPTY(ctype)
				REPLACE DATE WITH main_vw.u_cldt IN gen_srno_vw
				REPLACE ccate WITH main_vw.cate,npgno WITH fldval,entry_ty WITH main_vw.entry_ty, tran_cd WITH main_vw.tran_cd,compid WITH main_vw.compid, ;
					ctype WITH IIF(UPPER(fldnm)=='U_RG23NO','RGAPART2',IIF(UPPER(fldnm)=='U_RG23CNO','RGCPART2',IIF(UPPER(fldnm)=='U_PLASR','PLASRNO',""))),l_yn WITH main_vw.l_yn
			ENDIF
			IF BETW(mrec,1,RECCOUNT())
				GO mrec
			ENDIF
		ENDIF
		USE IN excur
	ENDIF
	=sqlconobj.sqlconnclose("nHandle")
	IF !EMPTY(_malias)
		SELECT &_malias
	ENDIF

	IF BETW(_mrecno,1,RECCOUNT())
		GO _mrecno
	ENDIF

	IF vrcount>0 AND !ISNULL(vrcount)
		RETURN .F.
	ELSE
		RETURN .T.
	ENDIF
ENDIF

RETURN
&& Added by Shrikant S. on 29/09/2010 for TKT-4021		--End

PROC chk_bond_ac() &&RUP:-procedure is useful to Search Bond Account Name in Sales entry in DCMAST.
sqlconobj=NEWOBJECT('sqlconnudobj',"sqlconnection",xapps)
nhandle=0
sq1="select distinct ac_mast.ac_name from obmain m "
sq2="inner join obacdet ac on (m.tran_cd=ac.tran_cd) inner join ac_mast on (ac_mast.ac_id=m.ac_id)"
sq3="where bond_no='"+ALLTRIM(main_vw.bond_no)+"'"

nretval = sqlconobj.dataconn([EXE],company.dbname,sq1+sq2+sq3,"_bondac","nHandle",_SCREEN.ACTIVEFORM.DATASESSIONID)
IF nretval<0
	RETURN .F.
ENDIF
IF USED("_bondac")
	SELECT _bondac
	macname=IIF(!ISNULL(_bondac.ac_name),_bondac.ac_name,"SALES")
ENDIF
macname=IIF(!ISNULL(macname),macname,"SALES")
RETURN macname
ENDPROC


&&code added by Ajay Jasiwal on 16/03/2009 ---> start
PROCEDURE salesman()
sqlconobj=NEWOBJECT('sqlconnudobj',"sqlconnection",xapps)
nhandle=0
sq1="select salesman from ac_mast ac where ac.ac_name=?main_vw.party_nm"

nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"ajcur","nHandle",_SCREEN.ACTIVEFORM.DATASESSIONID)
IF nretval<0
	RETURN .F.
ENDIF
SELECT ajcur
REPLACE salesman WITH ajcur.salesman IN main_vw
ENDPROC
&&code added by Ajay Jasiwal on 16/03/2009 ---> end

PROCEDURE chk_fldlen()
PARAMETERS _chkfldnm
IF LEN(ALLTRIM(_chkfldnm)) <> LEN(_chkfldnm)
	RETURN .F.
ENDIF
RETURN .T.

PROCEDURE chk_time()
PARAMETERS _chkfldnm
IF (VAL(SUBSTR(_chkfldnm,1,2))=24 AND VAL(SUBSTR(_chkfldnm,4,2))<>0)
	RETURN .F.
ENDIF
IF !(BETWEEN(VAL(SUBSTR(_chkfldnm,1,2)),0,24) AND BETWEEN(VAL(SUBSTR(_chkfldnm,4,2)),0,59)) AND !(BETWEEN(VAL(SUBSTR(_chkfldnm,1,2)),0,24) AND VAL(SUBSTR(_chkfldnm,4,2))=0)
	RETURN .F.
ENDIF
RETURN .T.


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
REPLACE eccno WITH UPPER(eccno) IN ac_mast_vw	&& Changed By Sachin N. S. on 26/02/2010 for TKT-484
IF EMPTY(ac_mast_vw.cexregno)
	REPLACE cexregno WITH eccno IN ac_mast_vw
ENDIF
*!*	  oobject.Parent.Refresh --> Ajay Jaiswal on 9/03/2010
ENDPROC


PROCEDURE pageno_whn() &&-->Rup  26Sep09
_malias 	= ALIAS()
SELE item_vw
_mrecno 	= RECNO()

retval=.T.
etsql_str = "select tp=[type] from it_mast where it_code="+STR(item_vw.it_code)
_etdatasessionid = _SCREEN.ACTIVEFORM.DATASESSIONID
SET DATASESSION TO _etdatasessionid
sqlconobj = NEWOBJECT('SqlConnUdObj','SqlConnection',xapps)
etsql_con = sqlconobj.dataconn([EXE],company.dbname,etsql_str,[itype],"nHandle",_etdatasessionid,.F.)
IF etsql_con < 1 OR !USED("itype")
	etsql_con = 0
ELSE
	SELECT itype
	IF INLIST(itype.tp,'Finished','Semi Finished')
		IF TYPE('main_vw.u_gcssr')='L'
			IF main_vw.u_gcssr=.F.
				retval=.F.
			ENDIF
		ELSE
			retval=.F.
		ENDIF
	ENDIF
	IF itype.tp='Trading'
		retval=.F.
	ENDIF
ENDIF
RETURN retval
&&<--Rup  26Sep09

&&<--Shrikant 26Sep09
PROC gen_nextno() &&Shrikant: procedure is useful to generate CT-1,CT-3,are1 No.,are2 No.,are3 No. in form UEFRM_ST_exdata1 in Sales Entry.
PARAMETERS fldnm,tblnm,filconfld,filconval
LOCAL vrno
sqlconobj=NEWOBJECT('sqlconnudobj',"sqlconnection",xapps)
nhandle=0
*!*		sq1="SELECT MAX(CAST( "+ALLTRIM(fldnm)+"  AS INT)) AS RNO  FROM "+ALLTRIM(tblnm)+" WHERE ISNUMERIC( "+ALLTRIM(fldnm)+" )=1 and l_yn='"+ALLTRIM(main_vw.l_yn)+"' " +" and "+ALLTRIM(filconfld)+ " = '"+ALLTRIM(filconval)+"'"		&& Commented by Shrikant S. on 29/09/2010 for TKT-4021
sq1="SELECT MAX(CAST( "+ALLTRIM(fldnm)+"  AS INT)) AS RNO  FROM "+ALLTRIM(tblnm)+" WHERE ISNUMERIC( "+ALLTRIM(fldnm)+" )=1 and l_yn='"+ALLTRIM(main_vw.l_yn)+"' "+IIF(!EMPTY(ALLTRIM(filconfld))," and "+ALLTRIM(filconfld)+ " = '"+ALLTRIM(filconval)+"'","")	&& Added by Shrikant S. on 29/09/2010 for TKT-4021
nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"EXCUR","nHandle",_SCREEN.ACTIVEFORM.DATASESSIONID)

IF nretval<0
	RETURN .F.
ENDIF
SELECT excur
vrno=ALLTRIM(STR(IIF(ISNULL(excur.rno),1,(excur.rno)+1)))
IF USED("EXCUR")
	USE IN excur
ENDIF
RETURN vrno
&&<--Shrikant 26Sep09


PROCEDURE mfgexpdtchk		&& Procedure to Check the Manufacturing Date with Expiry Date
LPARAMETERS oobject

SELECT item_vw
*!*	If !Empty(item_vw.MFGDT) And !Empty(item_vw.EXPDT)
*!*		If item_vw.MFGDT > item_vw.EXPDT
*!*	*!*			Return .F.
*!*		Endif
*!*	Endif

&&Added by Shrikant S. on 09 Mar, 2010 ---- Start
IF UPPER(ALLTRIM(STREXTRACT(oobject.ucontrolsource,'.'))) = 'EXPDT'
	IF !EMPTY(item_vw.mfgdt) AND !EMPTY(oobject.VALUE)
		IF item_vw.mfgdt > oobject.VALUE
			RETURN .F.
		ENDIF
	ENDIF
ELSE
	IF !EMPTY(oobject.VALUE) AND !EMPTY(item_vw.expdt)
		IF oobject.VALUE > item_vw.expdt
			RETURN .F.
		ENDIF
	ENDIF
ENDIF
&&Added by Shrikant S. on 09 Mar, 2010 ---- End

IF !EMPTY(item_vw.mfgdt) AND EMPTY(item_vw.expdt) AND UPPER(ALLTRIM(STREXTRACT(oobject.CONTROLSOURCE,'.'))) = 'EXPDT'
	RETURN .F.
ENDIF

*!*	oObject.Parent.Refresh  --> Ajay Jaiswal on 9/03/2010
ENDPROC

PROCEDURE check_bondperiod()
LPARAMETERS oobject

SELECT main_vw
IF UPPER(ALLTRIM(STREXTRACT(oobject.ucontrolsource,'.'))) = 'EXBVLDT'
	IF !EMPTY(main_vw.u_pinvdt) AND !EMPTY(oobject.VALUE)
		IF main_vw.u_pinvdt > oobject.VALUE
			RETURN .F.
		ENDIF
	ENDIF
ELSE
	IF !EMPTY(oobject.VALUE) AND !EMPTY(main_vw.exbvldt)
		IF oobject.VALUE > main_vw.exbvldt
			RETURN .F.
		ENDIF
	ENDIF
ENDIF

IF !EMPTY(main_vw.u_pinvdt) AND EMPTY(main_vw.exbvldt) AND UPPER(ALLTRIM(STREXTRACT(oobject.CONTROLSOURCE,'.'))) = 'EXBVLDT'
	RETURN .F.
ENDIF
*!*	oobject.Parent.Refresh
ENDPROC



PROCEDURE chk_vendtype
_curform = _SCREEN.ACTIVEFORM
_etdatasessionid = _curform.DATASESSIONID
SET DATASESSION TO _etdatasessionid
nhandle=0
macgroup = ac_mast_vw.GROUP
sq1=" Execute Usp_Ent_Get_Parent_Acgroup ?mAcGroup "
nretval = _curform.sqlconobj.dataconn([EXE],company.dbname,sq1,"panchkcur","nHandle",_etdatasessionid )
IF nretval<0
	RETURN .F.
ENDIF

*!*	Select ac_group_name From panchkcur Where ac_group_name = 'SUNDRY CREDITORS' Into Cursor cur1		&& Commented By Shrikant S. on 07/12/2012 for Bug-7404
Select ac_group_name From panchkcur Where ac_group_name = macgroup  Into Cursor cur1			&& Added By Shrikant S. on 07/12/2012 for Bug-7404
IF _TALLY > 0
	&& Added By Shrikant S. on 07/12/2012 for Bug-7404		&& Start
	If Vartype(oGlblPrdFeat)='O'
		If oGlblPrdFeat.UdChkProd('vutex')
			sq1=" Select Top 1 Cons_id as acid From ArMain Where Cons_id =?Ac_Mast_vw.Ac_id and isnull(scons_id,0)=0 "+;
				" Union all "+;
				" Select Top 1 manuac_id as acid From ArItem Where Manuac_Id =?Ac_Mast_vw.Ac_id  and isnull(manusac_id,0)=0 "

			nretval = _curform.sqlconobj.dataconn([EXE],company.dbname,sq1,"tmpAcType","nHandle",_etdatasessionid )
			If nretval<0
				Return .F.
			Endif
			If Reccount('tmpAcType')>0
				Return .F.
			Endif
			Use In tmpAcType
		Endif
	ENDIF
	&& Added By Shrikant S. on 07/12/2012 for Bug-7404		&& End
	RETURN .T.
ELSE
	RETURN .F.
ENDIF

ENDPROC

&& Added By Shrikant S. on 07/12/2012 for Bug-7404		&& Start
Procedure chk_Shiptovendtype
_curform = _Screen.ActiveForm
_etdatasessionid = _curform.DataSessionId
Set DataSession To _etdatasessionid
nhandle=0
macgroup = ac_mast_vw.Group
sq1=" Execute Usp_Ent_Get_Parent_Acgroup ?mAcGroup "
nretval = _curform.sqlconobj.dataconn([EXE],company.dbname,sq1,"panchkcur","nHandle",_etdatasessionid )
If nretval<0
	Return .F.
Endif
Select ac_group_name From panchkcur Where ac_group_name = macgroup  Into Cursor cur1
If _Tally > 0
	If Vartype(oGlblPrdFeat)='O'
		If oGlblPrdFeat.UdChkProd('vutex')
			sq1=" Select Top 1 scons_id From ArMain Where scons_Id=?_shipto.Shipto_Id and cons_id=?Ac_mast_vw.Ac_id"+;
				" and isnull(scons_id,0)<>0 "+;
				" Union all "+;
				" Select Top 1 manusac_id From ArItem Where Manusac_Id =?_shipto.Shipto_Id and Manuac_Id =?Ac_Mast_vw.Ac_id "+;
				" and isnull(manusac_id,0)<>0 "

			nretval = _curform.sqlconobj.dataconn([EXE],company.dbname,sq1,"tmpAcType","nHandle",_etdatasessionid )
			If nretval<0
				Return .F.
			Endif
			If Reccount('tmpAcType')>0
				Return .F.
			Endif
			Use In tmpAcType
		Endif
	ENDIF
	Return .T.
Else
	Return .F.
Endif

Endproc
&& Added By Shrikant S. on 07/12/2012 for Bug-7404		&& End

&&Rup 14/11/2009-->
PROCEDURE chk_empty_pan() &&Rup 14/11/2009
_etdatasessionid = _SCREEN.ACTIVEFORM.DATASESSIONID
SET DATASESSION TO _etdatasessionid
vpanempty=.F.
*sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)
nhandle=0
sq1="select i_tax from ac_mast where ac_id=?main_vw.ac_id"
sqlconobj = NEWOBJECT('SqlConnUdObj','SqlConnection',xapps)
nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"panchkcur","nHandle",_etdatasessionid )
IF nretval<0
	RETURN .F.
ENDIF
IF USED('panchkcur')
	SELECT panchkcur
	IF !EMPTY(panchkcur.i_tax)
		IF INLIST(ALLTRIM(panchkcur.i_tax),'PANAPPLIED','PANNOTAVBL','PANINVALID')
			vpanempty=.T.
		ENDIF
	ELSE
		vpanempty=.T.
	ENDIF
	USE IN panchkcur
ENDIF
RETURN vpanempty

&&<--Rup 14/11/2009

**&& Added For Expenses Purchase On 21/12/2009 by Hetal L Patel Start
PROCEDURE calcvat()
REPLACE u_vatonamt WITH ROUND(((item_vw.qty*item_vw.rate)*item_vw.u_vatonp)/100,2) IN item_vw
**&& Added For Expenses Purchase On 21/12/2009 by Hetal L Patel End


**start OPST_CHK_AVGPACK() TKT 479 and TKT 495
**&& Added On 22/02/2010 by Hetal L Patel St
PROC opst_chk_avgpack() &&Check reverse Calculation
malias = ALIAS()
sqlconobj=NEWOBJECT('sqlconnudobj',"sqlconnection",xapps)
nhandle=0

IF VAL(item_vw.u_pkno)#0
	_mqty = 0
	IF VAL(item_vw.u_pkno)#0 AND VAL(item_vw.u_appack)#0
		_mqty = (VAL(item_vw.u_pkno) * VAL(item_vw.u_appack))
	ENDIF
	IF _mqty # 0
		IF  (([vuexc] $ vchkprod) OR ([vuinv] $ vchkprod)) AND TYPE('main_vw.entry_ty')='C' &&Check Existing Records
			DO CASE
*!*					Case main_vw.entry_ty='OP'	&& Changed By Sachin N. S. on 01/02/2011 for TKT-5729
				CASE main_vw.entry_ty='OP' AND USED('Projectitref_vw')
					nretval = 0
					nhandle = 0
					etsql_con  = 0
					nhandle    = 0
					SELECT aentry_ty,atran_cd,aitserial,qty FROM projectitref_vw WHERE entry_ty=main_vw.entry_ty AND tran_cd=main_vw.tran_cd AND itserial=item_vw.itserial INTO CURSOR tibl
					etsql_str  = ""
					etsql_str = "USP_ENT_CHK_OP_ALLOCATION '"+item_vw.entry_ty+"',"+ALLTRIM(STR(item_vw.tran_cd))+","+ALLTRIM(STR(item_vw.it_code))+",'"+item_vw.itserial+"','";
						+tibl.aentry_ty+"',"+ALLTRIM(STR(tibl.atran_cd))+",'"+tibl.aitserial+"'"
					etsql_con = sqlconobj.dataconn([EXE],company.dbname,etsql_str,[tibl_1],"nHandle",_SCREEN.ACTIVEFORM.DATASESSIONID)
					IF etsql_con >0
						IF USED('tibl_1')
							IF ((_mqty >tibl_1.wipqty) AND tibl_1.wipqty<>0)
								REPLACE u_appack WITH '' IN item_vw
								=MESSAGEBOX('Quantity could not be greater then '+ALLTRIM(STR(tibl_1.wipqty,14,3)),0+64,vumess)
								SELECT (malias)
								RETURN .F.
							ENDIF
							USE IN tibl_1
						ENDIF
					ELSE
						SELECT (malias)
						RETURN .F.
					ENDIF
					USE IN tibl
				CASE main_vw.entry_ty='ST'
					IF USED('_DetailData')	&& Added By Sachin N. S. on 28/05/2010 for TKT-2055
						STORE 0 TO _balqty
						STORE item_vw.entry_ty TO centryty
						STORE item_vw.tran_cd TO ctrancd
						STORE item_vw.it_code TO citcode
						STORE item_vw.itserial TO citserial
*!* Pending Quantity
						SELECT a.balqtynew balqty FROM _detaildata a ;
							inner JOIN itref_vw b ON(a.entry_ty = b.rentry_ty AND a.itserial = b.ritserial AND a.it_code = b.it_code AND a.l_yn = b.rl_yn AND a.inv_no = b.rinv_no AND a.inv_sr = b.rinv_sr);
							WHERE b.entry_ty = centryty AND b.tran_cd = ctrancd AND b.it_code = citcode AND b.itserial = citserial ;
							INTO CURSOR pendqty
*!*
						SELECT pendqty
						STORE balqty TO _balqty
						IF _mqty >_balqty AND _balqty #0
							REPLACE u_appack WITH '' IN item_vw
							=MESSAGEBOX('Quantity could not be greater then '+ALLTRIM(STR(_balqty,14,3)),0+64,vumess)
							SELECT (malias)
							RETURN .F.
						ENDIF
					ENDIF
			ENDCASE
		ENDIF
		REPLACE qty WITH _mqty IN item_vw
	ENDIF
ENDIF
SELE(malias)
RETURN .T.
**&& Added On 22/02/2010 by Hetal L Patel Ed
***End OPST_CHK_AVGPACK()


&&--->TKT-941 &&Vasant250410
PROCEDURE calculateservicetax
m_serviceamount = 0
m_etvalidalias = ALIAS()
IF USED('AcdetAlloc_vw')
	SELECT acdetalloc_vw
	SUM(staxable) TO m_serviceamount FOR serty <> 'OTHERS' AND !EMPTY(serty)
ENDIF
IF !EMPTY(m_etvalidalias)
	SELECT (m_etvalidalias)
ENDIF
RETURN m_serviceamount

PROCEDURE findserviceavailtype
_actfrm = _SCREEN.ACTIVEFORM
*!*	m_ServiceAvailType = 'EXCISE'
m_serviceavailtype = 'SERVICES'		&& Changed By Sachin N. S. on 28/07/2010 for TKT-3279

m_etvalidalias = ALIAS()
IF USED('AcdetAlloc_vw') AND USED('CalcTax_vw')
	m_servicename = ''
	m_servicename = SUBSTR(calctax_vw.addpostcnd,AT(_actfrm.splitcharacter,calctax_vw.addpostcnd)+LEN(_actfrm.splitcharacter)+2)
	m_servicename = PADL(m_servicename,LEN(acdetalloc_vw.serty),' ')
	SELECT acdetalloc_vw
	IF SEEK(m_servicename,'AcdetAlloc_vw','Serty')
		m_serviceavailtype = seravail
	ENDIF
ELSE
	IF TYPE('_mseravail') = 'C'
		m_serviceavailtype = _mseravail
	ENDIF
ENDIF
IF !EMPTY(m_etvalidalias)
	SELECT (m_etvalidalias)
ENDIF
RETURN m_serviceavailtype


PROCEDURE findserviceruletype
_actfrm = _SCREEN.ACTIVEFORM
m_serviceavailtype = ''
m_etvalidalias = ALIAS()
IF TYPE('Main_vw.SerRule') = 'C'
	m_serviceavailtype = main_vw.serrule
ENDIF
IF TYPE('_mseravail') = 'C'
	m_serviceavailtype =  _mseravail
ENDIF
IF !EMPTY(m_etvalidalias)
	SELECT (m_etvalidalias)
ENDIF
RETURN m_serviceavailtype
&&<---TKT-941 &&Vasant250410ww

&&changes done as per TKT-3954
PROCEDURE get_exrateunit
_curform = _SCREEN.ACTIVEFORM
_etdatasessionid = _curform.DATASESSIONID
SET DATASESSION TO _etdatasessionid
_totrec = 0
IF TYPE('_curform.sqlconobj') = 'O'
	sq1="Select Top 1 RateUnit from Cetsh where Cetsh = ?it_mast_vw.Chapno "
	nretval = _curform.sqlconobj.dataconn([EXE],company.dbname,sq1,"_tmprateunit","_curform.nHandle",_etdatasessionid)
	IF nretval > 0 AND USED('_tmprateunit')
		REPLACE exrateunit WITH _tmprateunit.rateunit IN it_mast_vw
		_totrec = 1
	ENDIF
	IF USED('_tmprateunit')
		USE IN _tmprateunit
	ENDIF
ELSE
	_totrec = 1
ENDIF
IF _totrec > 0
	RETURN .T.
ELSE
	RETURN .F.
ENDIF

PROCEDURE chk_exrateunit
_curform = _SCREEN.ACTIVEFORM
_etdatasessionid = _curform.DATASESSIONID
SET DATASESSION TO _etdatasessionid
_totrec = 0
IF TYPE('_curform.sqlconobj') = 'O'
	sq1="Select Top 1 RateUnit from Cetsh where Rateunit = ?it_mast_vw.exrateunit"
	nretval = _curform.sqlconobj.dataconn([EXE],company.dbname,sq1,"_tmprateunit","_curform.nHandle",_etdatasessionid)
	IF nretval > 0 AND USED('_tmprateunit')
		_totrec = RECCOUNT('_tmprateunit')
	ENDIF
	IF USED('_tmprateunit')
		USE IN _tmprateunit
	ENDIF
ELSE
	_totrec = 1
ENDIF
IF _totrec > 0
	RETURN .T.
ELSE
	RETURN .F.
ENDIF

PROCEDURE chk_chapno
PARAMETERS oobject							&& Added by Shrikant S. on 04/10/2010 for TKT-4242
_curform = _SCREEN.ACTIVEFORM
_etdatasessionid = _curform.DATASESSIONID
SET DATASESSION TO _etdatasessionid
_totrec = 0
IF TYPE('_curform.sqlconobj') = 'O'
&&Done by Vasant on 21/01/11 Refer Note A
	_mvalue   = oobject.VALUE
	_mcsource = oobject.CONTROLSOURCE
&&Done by Vasant on 21/01/11 Refer Note A
	sq1="Select Top 1 RateUnit from Cetsh where Cetsh = ?_mValue"	&&Done by Vasant on 21/01/11 Refer Note A
	nretval = _curform.sqlconobj.dataconn([EXE],company.dbname,sq1,"_tmpchapno","_curform.nHandle",_etdatasessionid)
	IF nretval > 0 AND USED('_tmpchapno')
		_totrec = RECCOUNT('_tmpchapno')
		IF _totrec > 0
&&Done by Vasant on 21/01/11 Refer Note A
			_mexrateunitfld =  ALLTRIM(JUSTSTEM(_mcsource))+'.exrateunit'
			IF TYPE(_mexrateunitfld) = 'C'
				REPLACE exrateunit WITH _tmpchapno.rateunit IN (ALLTRIM(JUSTSTEM(_mcsource)))
				oobject.PARENT.REFRESH
			ENDIF
&&Done by Vasant on 21/01/11
		ENDIF
	ENDIF
	IF USED('_tmpchapno')
		USE IN _tmpchapno
	ENDIF
ELSE
	_totrec = 1
ENDIF
IF _totrec > 0
	RETURN .T.
ELSE
	RETURN .F.
ENDIF
*-*
*Note A
&&Earlier this validation was only for Item Master Chap. No., Now also done for Item Group Master
*-*
&&changes done as per TKT-3954

******* Added By Sachin N. S. for New Installer from NXIO Zip
PROC val_serapl()&&RUP:-procedure is useful to generate part-i-->pageno field.
IF UPPER(ac_mast_vw.serapl)='Y'
	DO FORM uefrm_ac_sertax WITH _SCREEN.ACTIVEFORM.DATASESSIONID,_SCREEN.ACTIVEFORM.addmode,_SCREEN.ACTIVEFORM.editmode
ENDIF
RETURN .T.


******* Added By Sachin N. S. for New Installer from NXIO Zip
PROCEDURE pvendor_type
LPARAMETERS ngrpid,nrettyp

LOCAL cretgrp,csql,cansi
cansi=SET('ansi')
cretgrp = IIF(nrettyp=1,.F.,"")
sqlconobj=NEWOBJECT('sqlconnudobj',"sqlconnection",xapps)
DO WHILE .T.
	IF !EMPTY(ngrpid)
		nhandle=0
		csql =  "SELECT ac_group_name,gAc_id,ac_group_id FROM ac_group_mast WHERE Ac_Group_Id = "+STR(ngrpid)
		nretval = sqlconobj.dataconn([EXE],company.dbname,csql,"EXCUR","nHandle",_SCREEN.ACTIVEFORM.DATASESSIONID)
		IF nretval<=0
			EXIT
		ENDIF
		SELECT excur
		IF INLIST(UPPER(ALLTRIM(excur.ac_group_name)),"SUNDRY CREDITORS")
			cretgrp = IIF(nrettyp=1,.T.,"Importer")
			EXIT
		ENDIF
		ngrpid = excur.gac_id
		IF ngrpid = 1 OR ngrpid = excur.ac_group_id
			EXIT
		ENDIF
	ELSE
		EXIT
	ENDIF
ENDDO
IF !EMPTY(cansi)
	SET ANSI &cansi
ENDIF

RETURN cretgrp
ENDPROC

******* Added By Sachin N. S. for New Installer from NXIO Zip
**Added by Shrikant S on 11/02/2010 12.58am
PROCEDURE repl_eccno
IF !EMPTY(ac_mast_vw.eccno)
	REPLACE exregno WITH eccno IN ac_mast_vw
ENDIF
ENDPROC

&& Added by Shrikant S. on 13/04/2011 for TKT-7206	--Start
PROCEDURE calc_brokerage
PARAMETERS 	oobject
IF (item_vw.brokperc >0)
	REPLACE brokamt WITH (item_vw.qty * item_vw.rate) * item_vw.brokperc/100 IN item_vw
	oobject.PARENT.REFRESH
ENDIF
ENDPROC

PROCEDURE calc_commamt
PARAMETERS 	oobject
IF (item_vw.commpermt >0)
	REPLACE commamt WITH (item_vw.qty * item_vw.commpermt) IN item_vw
	oobject.PARENT.REFRESH
ENDIF
ENDPROC
&& Added by Shrikant S. on 13/04/2011 for TKT-7206	--End


PROCEDURE calc_servicetax_s1() &&Rup TKT-7412,TKT-8319 26/05/2011
_sertaxableamt = 0
SELECT acdetalloc_vw
IF SEEK(item_vw.itserial,'acdetalloc_vw','itserial')  AND UPPER(main_vw.serrule)<>'EXEMPT'
	_sertaxableamt = acdetalloc_vw.staxable
ENDIF
*!*	SCAN FOR itserial==item_vw.itserial AND UPPER(main_vw.SerRule)<>'EXEMPT'
*!*		_SerTaxableAmt = acdetalloc_vw.staxable
*!*	Endscan
RETURN _sertaxableamt


PROCEDURE sertaxaceffect_s1() &&Rup TKT-7412,TKT-8319 26/05/2011
PARAMETERS _fldnm
_vacnm = ''
_isadv = .F.

IF TYPE('_mseravail') = 'C'
	IF INLIST(_mseravail,"EXCISE","SERVICE")
*	IF _mseravail= 'ADJUSTMENT'
		_isadv = .T.
	ENDIF
ENDIF


SELECT acdetalloc_vw
IF SEEK(item_vw.itserial,'acdetalloc_vw','itserial')
	DO CASE
		CASE UPPER(_fldnm)="SERBAMT" AND _isadv = .T.
			_vacnm="Service Tax Adjustment A/C"
		CASE UPPER(_fldnm)="SERBAMT" AND _isadv = .F.
			_vacnm="Service Tax Payable"
		CASE UPPER(_fldnm)="SERCAMT" AND _isadv = .T.
			_vacnm="Edu. Cess on Service Tax Adjustment A/C"
		CASE UPPER(_fldnm)="SERCAMT" AND _isadv = .F.
			_vacnm="Edu. Cess on Service Tax Payable"
		CASE UPPER(_fldnm)="SERHAMT" AND _isadv = .T.
			_vacnm="S & H Cess on Service Tax Adjustment A/C"
		CASE UPPER(_fldnm)="SERHAMT" AND _isadv = .F.
			_vacnm="S & H Cess on Service Tax Payable"
	ENDCASE
ENDIF
RETURN _vacnm


PROCEDURE sertaxaceffect_s1_cac() &&Rup TKT-7412,TKT-8319 26/05/2011
PARAMETERS _fldnm
_vacnm = ''
_isadv = .F.
IF TYPE('_mseravail') = 'C'
*!*		IF _mseravail= 'ADJUSTMENT'
	IF INLIST(_mseravail,"EXCISE","SERVICE")
		_isadv = .T.
	ENDI
ENDIF

SELECT acdetalloc_vw
IF SEEK(item_vw.itserial,'acdetalloc_vw','itserial')
	DO CASE
		CASE UPPER(_fldnm)="SERBAMT" AND _isadv = .T.
			_vacnm="Service Tax Payable"
		CASE UPPER(_fldnm)="SERCAMT" AND _isadv = .T.
			_vacnm="Edu. Cess on Service Tax Payable"
		CASE UPPER(_fldnm)="SERHAMT" AND _isadv = .T.
			_vacnm="S & H Cess on Service Tax Payable"
	ENDCASE
ENDIF
RETURN _vacnm


PROCEDURE sertaxaceffect_e1() &&Rup TKT-7412,TKT-8319 26/05/2011
PARAMETERS _fldnm
_fldnm = UPPER(_fldnm)
_vacnm = ''
_isadv = .F.
IF TYPE('_mseravail') = 'C'
	IF INLIST(_mseravail,"EXCISE","SERVICE")
		_isadv = .T.
	ENDI
ENDIF
SELECT acdetalloc_vw

IF SEEK(item_vw.itserial,'acdetalloc_vw','itserial')
	DO CASE
		CASE _isadv = .T. AND main_vw.serrule='IMPORT'
			IF _fldnm="SERBAMT"
				_vacnm="Input Service Tax Adjustment A/C"
			ENDIF
			IF  _fldnm="SERCAMT"
				_vacnm="Input Edu. Cess on Service Tax Adjustment A/C"
			ENDIF
			IF 	_fldnm="SERHAMT"
				_vacnm="Input S & H Cess on Service Tax Adjustment A/C"
			ENDIF
		CASE _isadv = .F. AND main_vw.serrule='IMPORT'
			IF _fldnm="SERBAMT"
				_vacnm="Input Service Tax Adjustment A/C"
			ENDIF
			IF  _fldnm="SERCAMT"
				_vacnm="Input Edu. Cess on Service Tax Adjustment A/C"
			ENDIF
			IF 	_fldnm="SERHAMT"
				_vacnm="Input S & H Cess on Service Tax Adjustment A/C"
			ENDIF
		CASE _isadv = .T. AND main_vw.serrule!='IMPORT'

			IF _fldnm="SERBAMT"
				_vacnm="Input Service Tax Adjustment A/C"
			ENDIF
			IF  _fldnm="SERCAMT"
				_vacnm="Input Edu. Cess on Service Tax Adjustment A/C"
			ENDIF
			IF 	_fldnm="SERHAMT"
				_vacnm="Input S & H Cess on Service Tax Adjustment A/C"
			ENDIF

		CASE _isadv = .F. AND main_vw.serrule!='IMPORT'
			IF acdetalloc_vw.seravail="SERVICE"
				IF _fldnm="SERBAMT"
					_vacnm="Service Tax Available"
				ENDIF
				IF _fldnm="SERCAMT"
					_vacnm="Edu. Cess on Service Tax Available"
				ENDIF
				IF _fldnm="SERHAMT"
					_vacnm="S & H Cess on Service Tax Available"
				ENDIF
			ENDIF
			IF acdetalloc_vw.seravail="EXCISE"
				IF _fldnm="SERBAMT"
					_vacnm="BALANCE WITH SERVICE TAX A/C"
				ENDIF
				IF _fldnm="SERCAMT"
					_vacnm="BALANCE WITH SERVICE TAX CESS A/C"
				ENDIF
				IF _fldnm="SERHAMT"
					_vacnm="BALANCE WITH SERVICE TAX HCESS A/C"
				ENDIF
			ENDIF
	ENDCASE
ENDIF
*=MESSAGEBOX("SerTaxAcEffect_E1 "+_vacnm)
RETURN _vacnm


PROCEDURE sertaxaceffect_e1_cac() &&Rup TKT-7412,TKT-8319 26/05/2011
PARAMETERS _fldnm
SET STEP ON
_fldnm = UPPER(_fldnm)
_vacnm = ''
_isadv = .F.
IF TYPE('_mseravail') = 'C'
	IF INLIST(_mseravail,"EXCISE","SERVICE")
		_isadv = .T.
	ENDI
ENDIF
*!*	IF TYPE('_mseravail') = 'C'
*!*		IF _mseravail= 'ADJUSTMENT'
*!*			_isadv = .t.
*!*		endi
*!*	ENDIF
SELECT acdetalloc_vw

IF SEEK(item_vw.itserial,'acdetalloc_vw','itserial')
	DO CASE
		CASE _isadv = .T.
			IF UPPER(_fldnm)="SERBAMT"
				_vacnm="Input Service Tax Adjustment A/C"
			ENDIF
			IF UPPER(_fldnm)="SERCAMT"
				_vacnm="Input Edu. Cess on Service Tax Adjustment A/C"
			ENDIF
			IF UPPER(_fldnm)="SERHAMT"
				_vacnm="Input S & H Cess on Service Tax Adjustment A/C"
			ENDIF

		CASE _isadv = .F.
			IF main_vw.serrule<>'IMPORT'
				IF UPPER(_fldnm)="SERBAMT"
					_vacnm=""
				ENDIF
				IF UPPER(_fldnm)="SERCAMT"
					_vacnm=""
				ENDIF
				IF UPPER(_fldnm)="SERHAMT"
					_vacnm=""
				ENDIF
			ENDIF
			IF main_vw.serrule='IMPORT'
				IF UPPER(_fldnm)="SERBAMT"
					_vacnm="Service Tax Payable"
				ENDIF
				IF UPPER(_fldnm)="SERCAMT"
					_vacnm="Edu. Cess on Service Tax Payable"
				ENDIF
				IF UPPER(_fldnm)="SERHAMT"
					_vacnm="S & H Cess on Service Tax Payable"
				ENDIF
			ENDIF
	ENDCASE
	IF _isadv = .T.
		IF _mseravail<>"EXCISE"
			IF _fldnm="SERBAMT"
				_vacnm="Service Tax Available"
			ENDIF
			IF _fldnm="SERCAMT"
				_vacnm="Edu. Cess on Service Tax Available"
			ENDIF
			IF _fldnm="SERHAMT"
				_vacnm="S & H Cess on Service Tax Available"
			ENDIF
		ELSE
			IF UPPER(_fldnm)='SERBAMT'
				_vacnm = "BALANCE WITH SERVICE TAX A/C"
			ENDIF
			IF UPPER(_fldnm)='SERCAMT'
				_vacnm = "BALANCE WITH SERVICE TAX CESS A/C"
			ENDIF
			IF UPPER(_fldnm)='SERHAMT'
				_vacnm = "BALANCE WITH SERVICE TAX HCESS A/C"
			ENDIF
		ENDIF
	ENDIF
ENDIF
RETURN _vacnm



PROCEDURE sertaxaceffect_bp() &&Rup TKT-7412,TKT-8319 26/05/2011
PARAMETERS _fldnm
_vacnm = ''
DO CASE
	CASE findallocservicetype() <> "IMPORT" AND main_vw.tdspaytype=1
		_vacnm = ''
	CASE findallocservicetype()='IMPORT' AND main_vw.tdspaytype=1
		IF findserviceavailtype()="EXCISE"
			IF UPPER(_fldnm)='SERBAMT'
				_vacnm = "BALANCE WITH SERVICE TAX A/C"
			ENDIF
			IF UPPER(_fldnm)='SERCAMT'
				_vacnm = "BALANCE WITH SERVICE TAX CESS A/C"
			ENDIF
			IF UPPER(_fldnm)='SERHAMT'
				_vacnm = "BALANCE WITH SERVICE TAX HCESS A/C"
			ENDIF
		ELSE
			IF UPPER(_fldnm)='SERBAMT'
				_vacnm = "SERVICE TAX AVAILABLE"
			ENDIF
			IF UPPER(_fldnm)='SERCAMT'
				_vacnm = "Edu. Cess on Service Tax Available"
			ENDIF
			IF UPPER(_fldnm)='SERHAMT'
				_vacnm = "S & H Cess on Service Tax Available"
			ENDIF

		ENDIF
	CASE findserviceavailtype()="EXCISE" AND main_vw.tdspaytype=2
		IF UPPER(_fldnm)='SERBAMT'
			_vacnm = "BALANCE WITH SERVICE TAX A/C"
		ENDIF
		IF UPPER(_fldnm)='SERCAMT'
			_vacnm = "BALANCE WITH SERVICE TAX CESS A/C"
		ENDIF
		IF UPPER(_fldnm)='SERHAMT'
			_vacnm = "BALANCE WITH SERVICE TAX HCESS A/C"
		ENDIF
	CASE findserviceavailtype()<>"EXCISE" AND main_vw.tdspaytype=2
		IF UPPER(_fldnm)='SERBAMT'
			_vacnm = "SERVICE TAX AVAILABLE"
		ENDIF
		IF UPPER(_fldnm)='SERCAMT'
			_vacnm = "Edu. Cess on Service Tax Available"
		ENDIF
		IF UPPER(_fldnm)='SERHAMT'
			_vacnm = "S & H Cess on Service Tax Available"
		ENDIF
ENDCASE
RETURN _vacnm



PROCEDURE sertaxaceffect_bp_cac() &&Rup TKT-7412,TKT-8319 26/05/2011
PARAMETERS _fldnm
_vacnm = ''
DO CASE
	CASE  main_vw.tdspaytype=1
		IF findallocservicetype() = "IMPORT"
			IF !EVAL(coadditional.seraccdt)
				IF UPPER(_fldnm)='SERBAMT'
					_vacnm = "Input Service Tax"
				ENDIF
				IF UPPER(_fldnm)='SERCAMT'
					_vacnm = "Edu. Cess on Input Service Tax"
				ENDIF
				IF UPPER(_fldnm)='SERHAMT'
					_vacnm = "S & H Cess on Input Service Tax"
				ENDIF
			ELSE
				IF UPPER(_fldnm)='SERBAMT'
					_vacnm = "Input Service Tax Adjustment A/C"
				ENDIF
				IF UPPER(_fldnm)='SERCAMT'
					_vacnm = "Input Edu. Cess on Service Tax Adjustment A/C"
				ENDIF
				IF UPPER(_fldnm)='SERHAMT'
					_vacnm = "Input S & H Cess on Service Tax Adjustment A/C"
				ENDIF
			ENDIF
		ELSE
			_vacnm = ''
		ENDIF
*eval(CoAdditional.SerAccDt) AND
*_vacnm = ''

	CASE !EVAL(coadditional.seraccdt)
		IF UPPER(_fldnm)='SERBAMT'
			_vacnm = "Input Service Tax"
		ENDIF
		IF UPPER(_fldnm)='SERCAMT'
			_vacnm = "Edu. Cess on Input Service Tax"
		ENDIF
		IF UPPER(_fldnm)='SERHAMT'
			_vacnm = "S & H Cess on Input Service Tax"
		ENDIF
	CASE EVAL(coadditional.seraccdt)
		IF UPPER(_fldnm)='SERBAMT'
			_vacnm = "Input Service Tax Adjustment A/C"
		ENDIF
		IF UPPER(_fldnm)='SERCAMT'
			_vacnm = "Input Edu. Cess on Service Tax Adjustment A/C"
		ENDIF
		IF UPPER(_fldnm)='SERHAMT'
			_vacnm = "Input S & H Cess on Service Tax Adjustment A/C"
		ENDIF

*!*	CASE eval(CoAdditional.SerAccDt) AND UPPER(_fldnm)='SERBAMT' AND main_vw.tdspaytype=2
*!*		_vacnm = "Input Service Tax Adjustment A/C"
*!*	CASE eval(CoAdditional.SerAccDt) AND UPPER(_fldnm)='SERCAMT' AND main_vw.tdspaytype=2
*!*		_vacnm = "Input Edu. Cess on Service Tax Adjustment A/C"
*!*	CASE eval(CoAdditional.SerAccDt) AND UPPER(_fldnm)='SERHAMT'
*!*		_vacnm = "S & H Cess on Input Service Tax"

ENDCASE
RETURN _vacnm


PROCEDURE sertaxaceffect_br() &&Rup TKT-7412,TKT-8319 26/05/2011
PARAMETERS _fldnm
_vacnm = ''
DO CASE
	CASE EVAL(coadditional.seraccdt) AND  main_vw.tdspaytype=1
		_vacnm = ""
	CASE !EVAL(coadditional.seraccdt) AND UPPER(_fldnm)='SERBAMT'
		_vacnm = "Output Service Tax"
	CASE  !EVAL(coadditional.seraccdt) AND UPPER(_fldnm)='SERCAMT'
		_vacnm = "Edu. Cess on Output Service Tax"
	CASE  !EVAL(coadditional.seraccdt) AND UPPER(_fldnm)='SERHAMT'
		_vacnm = "S & H Cess on OutPut Service Tax"

	CASE EVAL(coadditional.seraccdt) AND UPPER(_fldnm)='SERBAMT'
		_vacnm = "Service Tax Adjustment A/C"
	CASE EVAL(coadditional.seraccdt) AND UPPER(_fldnm)='SERCAMT'
		_vacnm = "Edu. Cess on Service Tax Adjustment A/C"
	CASE EVAL(coadditional.seraccdt) AND UPPER(_fldnm)='SERHAMT'
		_vacnm = "S & H Cess on Service Tax Adjustment A/C"
ENDCASE
RETURN _vacnm



PROCEDURE sertaxaceffect_br_cac() &&Rup TKT-7412,TKT-8319 26/05/2011
PARAMETERS _fldnm
_vacnm = ''
DO CASE
	CASE  main_vw.tdspaytype=2 AND UPPER(_fldnm)='SERBAMT'
		_vacnm = "Service Tax Payable"
	CASE   main_vw.tdspaytype=2 AND UPPER(_fldnm)='SERCAMT'
		_vacnm = "Edu. Cess on Service Tax Payable"
	CASE   main_vw.tdspaytype=2 AND UPPER(_fldnm)='SERHAMT'
		_vacnm = "S & H Cess on Service Tax Payable"
ENDCASE
RETURN _vacnm

PROCEDURE findallocservicetype() &&Rup TKT-7412,TKT-8319 26/05/2011
_curform = _SCREEN.ACTIVEFORM
_curallocservicetype = ''
IF USED('Mall_vw')

	SELECT mall_vw
	SCAN
		_curretval = _curform.sqlconobj.dataconn([EXE],company.dbname,"select top 1 SerRule from SerTaxMain_vw where Entry_ty = ?Mall_vw.Entry_all And Tran_cd = ?Mall_vw.Main_Tran","_TmpAlloc","This.Parent.nHandle",_curform.DATASESSIONID,.F.)
		IF _curretval > 0 AND USED('_TmpAlloc')
			_curallocservicetype = _tmpalloc.serrule
			EXIT
		ENDIF

		SELECT mall_vw
	ENDSCAN
ENDIF
RETURN _curallocservicetype

&&--->TKT-8320 GTA
PROCEDURE sertaxaceffect_if_cac() &&Used in GTA IF,OF Cr. A/c effect
PARAMETERS _fldnm
_fldnm = UPPER(_fldnm)
_vacnm = ''
_isadv = .F.
IF TYPE('_mseravail') = 'C'
	IF INLIST(_mseravail,"EXCISE","SERVICE")
		_isadv = .T.
	ENDI
ENDIF
SELECT acdetalloc_vw

IF SEEK(item_vw.itserial,'acdetalloc_vw','itserial')
	DO CASE
		CASE _isadv = .T. AND main_vw.serrule!='IMPORT'

			IF _fldnm="SERBAMT"
				_vacnm="Service Tax Adjustment A/C"
			ENDIF
			IF  _fldnm="SERCAMT"
				_vacnm="Edu. Cess on Service Tax Adjustment A/C"
			ENDIF
			IF 	_fldnm="SERHAMT"
				_vacnm="S & H Cess on Service Tax Adjustment A/C"
			ENDIF

		CASE _isadv = .F. AND main_vw.serrule!='IMPORT'
			IF _fldnm="SERBAMT"
				_vacnm="GTA Service Tax Payable"
			ENDIF
			IF _fldnm="SERCAMT"
				_vacnm="GTA Edu. Cess on Service Tax Payable"
			ENDIF
			IF _fldnm="SERHAMT"
				_vacnm="GTA S & H Cess on Service Tax Payable"
			ENDIF
	ENDCASE
ENDIF
RETURN _vacnm


PROCEDURE sertaxaceffect_bp_gta() &&Used in GTA BP,CP Dr. A/c effect
PARAMETERS _fldnm
_vacnm = ''
DO CASE
	CASE EVAL(coadditional.seraccdt) AND  main_vw.tdspaytype=1
		_vacnm = ""
	CASE !EVAL(coadditional.seraccdt) AND UPPER(_fldnm)='SERBAMT'
		_vacnm = "Output Service Tax"
	CASE  !EVAL(coadditional.seraccdt) AND UPPER(_fldnm)='SERCAMT'
		_vacnm = "Edu. Cess on Output Service Tax"
	CASE  !EVAL(coadditional.seraccdt) AND UPPER(_fldnm)='SERHAMT'
		_vacnm = "S & H Cess on OutPut Service Tax"

	CASE EVAL(coadditional.seraccdt) AND UPPER(_fldnm)='SERBAMT'
		_vacnm = "Service Tax Adjustment A/C"
	CASE EVAL(coadditional.seraccdt) AND UPPER(_fldnm)='SERCAMT'
		_vacnm = "Edu. Cess on Service Tax Adjustment A/C"
	CASE EVAL(coadditional.seraccdt) AND UPPER(_fldnm)='SERHAMT'
		_vacnm = "S & H Cess on Service Tax Adjustment A/C"
ENDCASE
RETURN _vacnm

PROCEDURE sertaxaceffect_bp_gta_cac() &&Used in GTA BP,CP Cr. A/c effect
PARAMETERS _fldnm
_vacnm = ''
DO CASE
	CASE  main_vw.tdspaytype=2 AND UPPER(_fldnm)='SERBAMT'
		_vacnm = "GTA Service Tax Payable"
	CASE   main_vw.tdspaytype=2 AND UPPER(_fldnm)='SERCAMT'
		_vacnm = "GTA Edu. Cess on Service Tax Payable"
	CASE   main_vw.tdspaytype=2 AND UPPER(_fldnm)='SERHAMT'
		_vacnm = "GTA S & H Cess on Service Tax Payable"
ENDCASE
RETURN _vacnm
&&<---TKT-8320 GTA
*****************************************************************************************
***************************ADDED BY SATISH PAL DT.04/01/2012*****************************

PROC gen_no_b() &&Birendra:-procedure is useful to generate Bond Serial No. on the base of rule, bond no.
PARAMETERS fldnm,tblnm,tabletype

LOCAL vrno
sqlconobj=NEWOBJECT('sqlconnudobj',"sqlconnection",xapps)
nhandle=0

IF !USED('Gen_SrNo_Vw')
	etsql_str = "Select * From Gen_SrNo where 1=0"
	etsql_con = sqlconobj.dataconn([EXE],company.dbname,etsql_str,[Gen_SrNo_Vw],;
		"nHandle",_SCREEN.ACTIVEFORM.DATASESSIONID,.F.)
	IF etsql_con < 1 OR !USED("Gen_SrNo_Vw")
		etsql_con = 0
	ELSE
		SELECT gen_srno_vw
		INDEX ON itserial TAG itserial
	ENDIF
ENDIF

*	sq1="SELECT MAX(CAST( "+ALLTRIM(fldnm)+"  AS INT)) AS RNO  FROM "+ALLTRIM(tblnm)+" WHERE ISNUMERIC( "+ALLTRIM(fldnm)+" )=1 and l_yn='"+ALLTRIM(main_vw.l_yn)+"' "+ IIF(coadditional.cate_srno," AND cate = ?Main_vw.Cate ","")
IF INLIST(ALLTRIM(main_vw.RULE),'CT-1','CT-3','EXPORT')
	sq1="SELECT MAX(CAST( npgno AS INT)) AS RNO  FROM Gen_srno WHERE ISNUMERIC(npgno)=1 and l_yn='"+ALLTRIM(main_vw.l_yn)+"' and crule IN ('CT-1','CT-3','EXPORT') "+" and cBond_no = '"+ ALLTRIM(main_vw.bond_no) +" ' " +IIF(coadditional.cate_srno," AND cate = ?Main_vw.Cate ","")
ELSE
*		sq1="SELECT MAX(CAST( npgno AS INT)) AS RNO  FROM Gen_srno WHERE ISNUMERIC(npgno)=1 and l_yn='"+ALLTRIM(main_vw.l_yn)+"' and crule = '"+ ALLTRIM(main_vw.RULE) +" ' and cBond_no = '"+ ALLTRIM(main_vw.bond_no) +" ' " +IIF(coadditional.cate_srno," AND cate = ?Main_vw.Cate ","")
*added by Birendra: for EOU itemwise bond sr no on 26 nov 2010
	sq1="SELECT MAX(CAST( npgno AS INT)) AS RNO  FROM gen_srno WHERE ISNUMERIC(npgno)=1 and l_yn='"+ALLTRIM(main_vw.l_yn)+"' and crule = '"
	zx = "tt"
	IF VARTYPE(tabletype)='C' AND coadditional.eou
		zx = tabletype + ".u_rule"
	ENDIF
	sq1= sq1 + IIF(VARTYPE(tabletype)='C',ALLTRIM(IIF(INLIST(UPPER(tabletype),"ITEM_VW","ISSDET"),&zx,main_vw.RULE)),main_vw.RULE)
	sq1= sq1 +" ' and cBond_no = '"
	IF VARTYPE(tabletype)='C'
		zx = tabletype + ".bond_no"
	ENDIF
	sq1= sq1 + IIF(VARTYPE(tabletype)='C',ALLTRIM(IIF(INLIST(UPPER(tabletype),"ITEM_VW","ISSDET"),ALLTRIM(&zx),ALLTRIM(main_vw.bond_no))),ALLTRIM(main_vw.bond_no))
	sq1= sq1 +" ' " +IIF(coadditional.cate_srno," AND cate = ?Main_vw.Cate ","")
*end by birendra:

*IIF(VARTYPE(tabletype)='C',Alltrim(IIF(UPPER(tabletype) = "ITEM_VW",item_vw.u_rule,main_vw.rule)),main_vw.rule)
ENDIF

nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"EXCUR","nHandle",_SCREEN.ACTIVEFORM.DATASESSIONID)
IF nretval<0
	RETURN .F.
ENDIF
SELECT excur
vrno=ALLTRIM(STR(IIF(ISNULL(excur.rno),1,(excur.rno)+1)))

*added by Birendra: for EOU itemwise bond sr no on 26 nov 2010
IF VARTYPE(tabletype)='C' AND coadditional.eou
	IF INLIST(UPPER(tabletype),"ITEM_VW","ISSDET")
		SELECT gen_srno_vw
*!*				zx = "Locate For cit_code = " + tabletype + ".it_code And ctype='BONDSRNO' "
*!*				zx = zx+"and tran_cd =" +tabletype+".tran_cd and itserial = "+tabletype+".itserial and itserial1 = item_vw.itserial"
		zx = "Locate For cit_code="+tabletype +".it_code And itserial=" +"item_vw.itserial And ctype='BONDSRNO' and tran_cd = main_vw.tran_cd"
		IF UPPER(tabletype)<>"ITEM_VW"
			zx = zx+" and aitserial=" +tabletype+".itserial And atran_cd = "+tabletype+".tran_cd and aentry_ty = "+tabletype+".entry_ty"
		ENDIF
		&zx
		IF FOUND()
			IF tabletype = 'issdet'
				SELECT rmdet_vw
				zx = "Locate For li_it_code = " + tabletype + ".it_code "
				zx = zx+"and li_tran_cd =" +tabletype+".tran_cd and li_itser = "+tabletype+".itserial and itserial = item_vw.itserial"
				&zx
				zx = "Empty( "+tabletype+".bondsrno)"
				IF &zx
					SELECT gen_srno_vw
					zx="SELECT MAX(npgno) FROM gen_srno_vw WHERE crule = "+tabletype+".u_rule INTO array vrno1"
					&zx
					IF INT(VAL(vrno1))> 0
						vrno = INT(VAL(vrno1)) + 1
					ENDIF
				ELSE
					SELECT gen_srno_vw
					IF !EMPTY(rmdet_vw.bondsrno)
						LOCATE FOR ALLTRIM(npgno)=ALLTRIM(rmdet_vw.bondsrno)
					ENDIF
					vrno = npgno
				ENDIF
			ELSE
				SELECT gen_srno_vw
				zx = "Locate For cit_code="+tabletype +".it_code And itserial=" +"item_vw.itserial And ctype='BONDSRNO' and tran_cd = main_vw.tran_cd "
*						zx = zx+" and aitserial=" +tabletype+".itserial And atran_cd = "+tabletype+".tran_cd and aentry_ty = "+tabletype+".entry_ty"
				&zx
				IF FOUND()
					zx1 = RECNO()
					IF ALLTRIM(crule)<>ALLTRIM(item_vw.u_rule)
						zx="SELECT MAX(npgno) FROM gen_srno_vw WHERE crule = "+tabletype+".u_rule and RECNO()<>zx1 INTO array vrno1"
						&zx
						IF INT(VAL(vrno1))> 0
							vrno = INT(VAL(vrno1)) + 1
						ENDIF
					ELSE
						vrno = npgno
					ENDIF
				ENDIF
			ENDIF
		ELSE
			zx="SELECT MAX(npgno) FROM gen_srno_vw WHERE crule = "+tabletype+".u_rule INTO array vrno1"
			&zx
			IF INT(VAL(vrno1))> 0
				vrno = INT(VAL(vrno1)) + 1
			ENDIF
		ENDI
	ENDIF
ENDIF
*end by birendra:

IF USED("EXCUR")
	USE IN excur
ENDIF
*sele(mAlias)
RETURN vrno
PROCEDURE fcnmlostfocusproc
sql_updt = 0
IF UPPER(ALLTRIM(item_vw.u_fcname))	== UPPER(ALLTRIM(company.CURRENCY))
	sql_updt = 0
ELSE
&& Added by Ajay Jaiswal For EOU on 03/09/2010 - Start
	nretval = _SCREEN.ACTIVEFORM.sqlconobj.dataconn([EXE],company.dbname,[SELECT currencyid from curr_mast ;
				where curr_mast.currencycd = ?item_vw.u_fcname],[EXCUR],"This.Parent.nHandle",_SCREEN.ACTIVEFORM.DATASESSIONID ,.F.)
	IF nretval<0
		RETURN .F.
	ENDIF

	tcurrencyid = excur.currencyid
&& Added by Ajay Jaiswal For EOU on 03/09/2010 - End
	msqlstr = []
	msqlstr = IIF(lcode_vw.fcrateon='IMPORT',[impcurrrate],[expcurrrate])
	sql_con = _SCREEN.ACTIVEFORM.sqlconobj.dataconn([EXE],company.dbname,[Select Top 1 ]+msqlstr+[ as FcRate From Curr_rate ;
							Where Currencyid = ?tcurrencyid And Remark = 'Daily' And Currdate <= ?Main_vw.Date ;
							Order By Currdate Desc],[_xtrtblv],;
		"This.Parent.nHandle",_SCREEN.ACTIVEFORM.DATASESSIONID ,.F.)
	IF sql_con > 0 AND USED('_xtrtblv')
		SELECT _xtrtblv
		IF RECCOUNT() > 0
			sql_updt = _xtrtblv.fcrate
		ENDIF
	ENDIF
ENDIF		&&atlas140408

IF sql_updt >= 0
	REPLACE u_fcexrate WITH sql_updt IN item_vw
ENDIF
ENDPROC
PROCEDURE chk_bondsrno
sq1 = " select b.bondsrno from ptitref a inner join ptmain b on (a.entry_ty =b.entry_ty and a.tran_cd=b.tran_cd) "
sq2 = " where (a.rentry_ty= '"+	main_vw.entry_ty+"' and a.itref_tran = "+STR(main_vw.tran_cd)+") "
sq3 = " union select b.bondsrno from aritref a inner join armain b on (a.entry_ty =b.entry_ty and a.tran_cd=b.tran_cd) "
sq4 = " where (a.rentry_ty= '"+	main_vw.entry_ty+"' and a.itref_tran = "+STR(main_vw.tran_cd)+") "

nretval = _SCREEN.ACTIVEFORM.sqlconobj.dataconn([EXE],company.dbname,sq1+sq2+sq3+sq4,"ajcur","This.Parent.nHandle",_SCREEN.ACTIVEFORM.DATASESSIONID,.F.)
IF nretval <0
	RETURN .F.
ENDIF

srnofound=.T.

SELECT ajcur
SCAN
	IF main_vw.bondsrno<>ajcur.bondsrno
		srnofound = .F.
	ENDIF
ENDSCAN
RETURN srnofound
ENDPROC
PROCEDURE dupbond_no &&RUP:-procedure is useful to check duplicate value of part-ii,pla field and in Daily Debit form UEFRM_ST_DAILTDEBIT in Sales Entry.
PARAMETERS fldnm,fldval,tblnm,mcommit,nhand,tabletype
pgno='Main_Vw.'+fldnm
* SET STEP ON
IF !EMPTY(fldval)
	_malias 	= ALIAS()
	_mrecno	= RECNO()
**notype =Iif(Upper(fldnm)=='Bondsrno','RGAPART2',Iif(Upper(fldnm)=='U_RG23CNO','RGCPART2',Iif(Upper(fldnm)=='U_PLASR','PLASRNO',"")))
	LOCAL vdup
	IF fldval=' '
		RETURN .T.
	ENDIF
	sqlconobj=NEWOBJECT('sqlconnudobj',"sqlconnection",xapps)

	nhandle=0
	IF USED('Gen_SrNo_Vw')
		SELECT gen_srno_vw
		mrec=RECNO()
	ENDIF
	IF !USED('Gen_SrNo_Vw')
		etsql_str = "Select * From Gen_SrNo where 1=0"
		etsql_con = sqlconobj.dataconn([EXE],company.dbname,etsql_str,[Gen_SrNo_Vw],;
			"nHandle",_SCREEN.ACTIVEFORM.DATASESSIONID,.F.)
		IF etsql_con < 1 OR !USED("Gen_SrNo_Vw")
			etsql_con = 0
		ELSE
			SELECT gen_srno_vw
			INDEX ON itserial TAG itserial
		ENDIF
	ENDIF

*sq1="SELECT NpgNo FROM Gen_Srno WHERE l_yn='"+Alltrim(main_vw.l_yn)+"' "+Iif(Upper(fldnm)=='U_RG23NO'," AND CTYPE='RGAPART2'",Iif(Upper(fldnm)=='U_RG23CNO'," AND CTYPE='RGCPART2'",Iif(Upper(fldnm)=='U_PLASR'," AND CTYPE='PLASRNO'","")))+" AND NpgNo='"+Alltrim(fldval)+"' AND NOT (TRAN_CD="+Str(main_vw.tran_cd)+" AND ENTRY_TY='"+main_vw.entry_ty+"'"+Iif(fldnm='U_PAGENO'," AND "+tblnm+".ITSERIAL='"+item_vw.itserial+"')",")")+ Iif(coadditional.cate_srno," AND cate = ?Main_vw.Cate ","")
**	IF INLIST(ALLTRIM(main_vw.RULE),'CT-1','CT-3','EXPORT')
*		sq1="SELECT npgno  FROM Gen_srno WHERE l_yn='"+ALLTRIM(main_vw.l_yn)+"' and ctype = 'BONDSRNO' and crule = '"+ ALLTRIM(main_vw.RULE) +" 'and cBond_no = '"+ ALLTRIM(main_vw.bond_no) +" ' " + " AND NpgNo='"+Alltrim(fldval)+"' AND NOT (TRAN_CD="+Str(main_vw.tran_cd)+" AND ENTRY_TY='"+main_vw.entry_ty+"')"+IIF(coadditional.cate_srno," AND cate = ?Main_vw.Cate ","")
*birendra : above line commented and bellow added line of code for EOU
	IF VARTYPE(tabletype)='C' AND coadditional.eou
		IF INLIST(UPPER(tabletype),"ITEM_VW","ISSDET")
			zx="tmprule="+tabletype+".u_rule"
			&zx
			zx="tmpbondno="+tabletype+".bond_no"
			&zx
			zx="tmpentryty="+tabletype+".entry_ty"
			&zx
			zx="tmptrancd="+tabletype+".tran_cd"
			&zx
			zx="tmpitserial="+tabletype+".itserial"
			&zx
			sq1="SELECT npgno  FROM Gen_srno WHERE l_yn='"+ALLTRIM(main_vw.l_yn)+"' and ctype = 'BONDSRNO' and crule = '"+ tmprule +"' and cBond_no = '"+ ALLTRIM(tmpbondno) +"'" + " AND NpgNo='"+ALLTRIM(fldval)+"' AND NOT (TRAN_CD="+STR(main_vw.tran_cd)+" AND ENTRY_TY='"+main_vw.entry_ty+"' and ITSERIAL='"+item_vw.itserial
			IF UPPER(tabletype)<>"ITEM_VW"
				sq1=sq1+"' AND aITSERIAL='"+ALLTRIM(tmpitserial)+"' and aTRAN_CD="+ALLTRIM(STR(tmptrancd))+" AND aENTRY_TY='"+ALLTRIM(tmpentryty)
			ENDIF
			sq1=sq1+"')"
			sq1=sq1+IIF(coadditional.cate_srno," AND cate = ?Main_vw.Cate ","")
		ELSE
			sq1="SELECT npgno  FROM Gen_srno WHERE l_yn='"+ALLTRIM(main_vw.l_yn)+"' and ctype = 'BONDSRNO' and crule = '"+ ALLTRIM(main_vw.RULE) +" 'and cBond_no = '"+ ALLTRIM(main_vw.bond_no) +" ' " + " AND NpgNo='"+ALLTRIM(fldval)+"' AND NOT (TRAN_CD="+STR(main_vw.tran_cd)+" AND ENTRY_TY='"+main_vw.entry_ty+"')"+IIF(coadditional.cate_srno," AND cate = ?Main_vw.Cate ","")
		ENDIF
	ELSE
		sq1="SELECT npgno  FROM Gen_srno WHERE l_yn='"+ALLTRIM(main_vw.l_yn)+"' and ctype = 'BONDSRNO' and crule = '"+ ALLTRIM(main_vw.RULE) +" ' and cBond_no = '"+ ALLTRIM(main_vw.bond_no) +"' " + " AND NpgNo='"+ALLTRIM(fldval)+"' AND NOT (TRAN_CD="+STR(main_vw.tran_cd)+" AND ENTRY_TY='"+main_vw.entry_ty+"')"+IIF(coadditional.cate_srno," AND cate = ?Main_vw.Cate ","")
	ENDIF
*!*		ELSE
*!*			sq1="SELECT MAX(CAST( npgno AS INT)) AS RNO  FROM Gen_srno WHERE ISNUMERIC(npgno)=1 and l_yn='"+ALLTRIM(main_vw.l_yn)+"' and ctype = 'BONDSRNO' and crule = '"+ ALLTRIM(main_vw.[RULE]) +" ' and cBond_no = '"+ ALLTRIM(main_vw.bond_no) +" ' " +IIF(coadditional.cate_srno," AND cate = ?Main_vw.Cate ","")
*!*		ENDIF
	nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"EXCUR",IIF(mcommit=.F.,"nHandle",nhand),_SCREEN.ACTIVEFORM.DATASESSIONID)	&&280910
	IF nretval<0
		RETURN .F.
	ENDIF
	SELECT excur
	vrcount=RECCOUNT()
*	MESSAGEBOX(vrcount)
	IF nretval >0 AND USED("EXCUR")
		IF	vrcount<=0
			SELECT gen_srno_vw
			GO TOP
*Change by Birendra : EOU item wise Bond sr no gen
			IF VARTYPE(tabletype)='C' AND coadditional.eou
				IF INLIST(UPPER(tabletype),"ITEM_VW","ISSDET")
					zx = "Locate For cit_code="+tabletype +".it_code And itserial=" +"item_vw.itserial And ctype='BONDSRNO' and tran_cd = main_vw.tran_cd"
					IF UPPER(tabletype)<>"ITEM_VW"
						zx = zx+" and aitserial=" +tabletype+".itserial And atran_cd = "+tabletype+".tran_cd and aentry_ty = "+tabletype+".entry_ty"
					ENDIF
*					zx = zx + IIF(INLIST(UPPER(tabletype),"ITEM_VW")," AND CRULE = "+tabletype+".U_RULE","")
					&zx
					IF !FOUND()
						APPEND BLANK IN gen_srno_vw
					ENDIF

					IF INLIST(ctype,"BONDSRNO") OR EMPTY(ctype)
						zx="Replace ccate With "+tabletype+".cate,npgno With fldval,entry_ty With main_vw.entry_ty, tran_cd With main_vw.tran_cd,compid With main_vw.compid,"
						zx =zx +"ctype With 'BONDSRNO',l_yn With main_vw.l_yn,CRULE WITH "+tabletype+".U_RULE,CBOND_NO WITH "+tabletype+".BOND_NO,"
						zx =zx+"cit_Code WITH "+tabletype+".it_code,itserial WITH "+"item_vw.itserial,"
						zx =zx+"aentry_ty WITH "+tabletype+".entry_ty,atran_cd with "+tabletype+".tran_cd,aitserial WITH "+tabletype+".itserial"
						&zx
					ENDIF
				ELSE
					LOCATE FOR entry_ty=main_vw.entry_ty AND tran_cd=main_vw.tran_cd AND (ctype='BONDSRNO')
					IF !FOUND()
						APPEND BLANK IN gen_srno_vw
					ENDIF

					IF INLIST(ctype,"BONDSRNO") OR EMPTY(ctype)
						REPLACE ccate WITH main_vw.cate,npgno WITH fldval,entry_ty WITH main_vw.entry_ty, tran_cd WITH main_vw.tran_cd,compid WITH main_vw.compid,;
							ctype WITH 'BONDSRNO',l_yn WITH main_vw.l_yn,crule WITH main_vw.RULE,cbond_no WITH main_vw.bond_no
					ENDIF
				ENDIF
			ELSE
				LOCATE FOR entry_ty=main_vw.entry_ty AND tran_cd=main_vw.tran_cd AND (ctype='BONDSRNO')
				IF !FOUND()
					APPEND BLANK IN gen_srno_vw
				ENDIF

				IF INLIST(ctype,"BONDSRNO") OR EMPTY(ctype)
					REPLACE ccate WITH main_vw.cate,npgno WITH fldval,entry_ty WITH main_vw.entry_ty, tran_cd WITH main_vw.tran_cd,compid WITH main_vw.compid,;
						ctype WITH 'BONDSRNO',l_yn WITH main_vw.l_yn,crule WITH main_vw.RULE,cbond_no WITH main_vw.bond_no
				ENDIF
			ENDIF
*End by Birendra

*!*				Locate For entry_ty=main_vw.entry_ty And tran_cd=main_vw.tran_cd And (ctype='BONDSRNO')
*!*				If !Found()
*!*					Append Blank In Gen_SrNo_Vw
*!*				Endif

*!*				If Inlist(ctype,"BONDSRNO") Or Empty(ctype)
*!*	*				Replace Date With main_vw.u_cldt In Gen_SrNo_Vw
*!*					Replace ccate With main_vw.cate,npgno With fldval,entry_ty With main_vw.entry_ty, tran_cd With main_vw.tran_cd,compid With main_vw.compid,;
*!*						ctype With 'BONDSRNO',l_yn With main_vw.l_yn,CRULE WITH MAIN_VW.RULE,CBOND_NO WITH MAIN_VW.BOND_NO
*!*				ENDIF
			IF BETW(mrec,1,RECCOUNT())
				GO mrec
			ENDIF
		ENDIF
		USE IN excur
	ENDIF
	=sqlconobj.sqlconnclose("nHandle")
*	MESSAGEBOX('b')
	IF !EMPTY(_malias)
		SELECT &_malias
	ENDIF
* 	MESSAGEBOX('c')
	IF BETW(_mrecno,1,RECCOUNT())
		GO _mrecno
	ENDIF
* 	MESSAGEBOX('d')
	IF vrcount>0 AND !ISNULL(vrcount)
		RETURN .F.
	ELSE
		RETURN .T.
	ENDIF
ENDIF
RETURN

***************************END BY SATISH PAL DT.04/01/2012*******************************
*****************************************************************************************
*Birendra : Auto Debit and Credit Note on 18 july 2011 :Start:
PROCEDURE calc_brok_comm
IF TYPE('item_vw.billqty')<>'U'
	IF item_vw.billqty > 0 AND main_vw.entry_ty='DB'
		SELECT item_vw
		REPLACE item_vw.brok_amt WITH ROUND(main_vw.brokperm*item_vw.billqty,0) IN item_vw
		REPLACE item_vw.comm_amt WITH ROUND(main_vw.compermt*item_vw.billqty,0) IN item_vw
		REPLACE item_vw.gro_amt WITH (item_vw.brok_amt+item_vw.comm_amt) IN item_vw
		REPLACE item_vw.rate WITH (item_vw.gro_amt/item_vw.qty) IN item_vw
	ENDIF
ENDIF

*	IF INLIST(main_vw.entry_TY,'D1','D2','D3','D4','D5','CI') AND qty > 0
*!*	IF INLIST(main_vw.entry_ty,'D1','D2','D3','D4','D5','C2','C3','C4','C5') AND qty > 0		&& Commented By Shrikant S. on 17/03/2012 for Bug-2276
IF INLIST(main_vw.entry_ty,'D2','D3','D4','D5','C2','C3','C4','C5') AND qty > 0			&& Added By Shrikant S. on 17/03/2012 for Bug-2276
	SELECT item_vw
	REPLACE item_vw.comm_amt WITH IIF(main_vw.compermt>0,main_vw.compermt*item_vw.qty,item_vw.comm_amt) IN item_vw
	REPLACE item_vw.rate WITH  IIF(main_vw.compermt>0,main_vw.compermt,item_vw.comm_amt) IN item_vw
ENDIF

*----------------For Auto Debit Note----------------------------------------
*	IF INLIST(main_vw.entry_TY,'DI','CI') AND main_vw.dept ='LATE PAYMENT'
*	IF INLIST(main_vw.entry_TY,'D3','CI')
IF INLIST(main_vw.entry_ty,'D3','C3')
	ztmpintbaseday=365
	IF _SCREEN.ACTIVEFORM.intbaseday>0
		ztmpintbaseday=_SCREEN.ACTIVEFORM.intbaseday
	ENDIF
	SELECT item_vw
	REPLACE item_vw.qty WITH 1 IN item_vw
	REPLACE item_vw.intamt WITH ROUND(((item_vw.recdamt*(item_vw.intper/100))/ztmpintbaseday)*item_vw.ltdays,0) IN item_vw
	REPLACE item_vw.rate WITH intamt IN item_vw
	REPLACE item_vw.gro_amt WITH item_vw.rate*item_vw.qty IN item_vw

ENDIF
*----------------------------------------------------------------------------

RETURN
*Birendra : Auto Debit and Credit Note on 18 july 2011 :End:

&& Start : Added by Uday on 26/12/2011 for EXIM
PROCEDURE saveoppttaxdet()
IF INLIST(main_vw.entry_ty,'OP')
	LOCAL mobj
	mobj = _SCREEN.ACTIVEFORM

	IF USED("Projectitref_vw")
		DELETE FROM op_pttax_vw WHERE entry_ty = main_vw.entry_ty AND tran_cd = main_vw.tran_cd

		DO WHILE !EOF("Projectitref_vw")
			lcstr = "execute USP_ENT_EXIM_OP_PT_DUTY 'IP','" + ALLTRIM(projectitref_vw.aentry_ty) +;
				"'," + ALLTRIM(STR(projectitref_vw.atran_cd)) + ",'" + ALLTRIM(projectitref_vw.aitserial) + "'"

			sql_con = mobj.sqlconobj.dataconn([EXE],company.dbname,lcstr,[cursIpItem],;
				"Thisform.nHandle",mobj.DATASESSIONID,.F.)

			IF sql_con < 1
				=mobj.sqlconobj.sqlconnclose("Thisform.nHandle")
				mobj.showmessagebox("Error while retrive records from IP ",32,vumess)	&&test_z 32
				RETURN 0
			ENDIF

			=mobj.sqlconobj.sqlconnclose("Thisform.nHandle")

			IF USED("cursIpItem")
				SELECT cursipitem
				GO TOP
				DO WHILE !EOF("cursIPItem")

					IF USED("CursDcMast")
						USE IN cursdcmast
					ENDIF

					DO CASE
						CASE ALLTRIM(UPPER(cursipitem.RULE)) = 'IMPORTED'
							lcstr = [Select Fld_Nm,Pert_Name,Head_Nm From Dcmast Where Code = 'E' And Entry_Ty = 'P1' AND Att_File = 0 order by corder]
						CASE ALLTRIM(UPPER(cursipitem.RULE)) = 'ANNEXURE IV' OR ALLTRIM(UPPER(cursipitem.RULE)) = 'IMPORTED'
							lcstr = [Select Fld_Nm,Pert_Name,Head_Nm From Dcmast Where Code = 'E' And Entry_Ty = 'P1' AND Att_File = 0 order by corder]
						OTHERWISE
							lcstr = [Select Fld_Nm,Pert_Name,Head_Nm From Dcmast Where Code = 'E' And Entry_Ty = 'PT' AND Att_File = 0 order by corder]
					ENDCASE
					sql_con = mobj.sqlconobj.dataconn([EXE],company.dbname,lcstr,[cursdcmast],;
						"Thisform.nHandle",mobj.DATASESSIONID,.F.)
					IF sql_con < 1
						mobj.showmessagebox("Error while open Discount charges Master table",32,vumess)	&&test_z 32
					ENDIF

					APPEND BLANK IN op_pttax_vw

					REPLACE entry_ty WITH item_vw.entry_ty,;
						tran_cd  WITH  item_vw.tran_cd,;
						itserial WITH  item_vw.itserial,;
						pentry_ty WITH cursipitem.pentry_ty,;
						ptran_cd WITH cursipitem.ptran_cd,;
						pitserial WITH cursipitem.pitserial,;
						it_code WITH cursipitem.it_code,;
						ITEM    WITH cursipitem.ITEM,;
						qty     WITH cursipitem.qty,;
						genby   WITH musername IN op_pttax_vw

					SELECT cursdcmast
					GO TOP
					ztmpexamt = 0
					ztmpexamt1 = 0

					DO WHILE !EOF()
						tfld_nm = SPACE(05)
						flditname = "cursIPItem."+ALLTRIM(cursdcmast.fld_nm)
						fldname   = "Op_PtTax_vw."+SUBSTR(ALLTRIM(cursdcmast.fld_nm),IIF(AT("U_",UPPER(ALLTRIM(cursdcmast.fld_nm)))>0,AT("U_",UPPER(ALLTRIM(cursdcmast.fld_nm)))+2,1),LEN(cursdcmast.fld_nm))
						flditname1 = "cursIPItem."+ALLTRIM(cursdcmast.pert_name)
						fldname1   = "Op_PtTax_vw."+SUBSTR(ALLTRIM(cursdcmast.pert_name),IIF(AT("U_",UPPER(ALLTRIM(cursdcmast.pert_name)))>0,AT("U_",UPPER(ALLTRIM(cursdcmast.pert_name)))+2,1),LEN(cursdcmast.pert_name))

						STORE "" TO upfields
						IF TYPE(fldname) # 'U'
							upfields = "Replace "+ALLTRIM(fldname)+" WITH "+ALLTRIM(flditname)
						ENDIF

						IF TYPE(fldname1) # 'U'
							upfields = upfields +", "+ALLTRIM(fldname1)+" with "+ALLTRIM(flditname1)+" in op_pttax_vw"
						ELSE
							upfields = upfields + IIF(!EMPTY(upfields)," in op_pttax_vw","")
						ENDIF

						IF !EMPTY(upfields)
							&upfields
						ENDIF

						SKIP IN cursdcmast
					ENDDO
					SKIP IN cursipitem
				ENDDO
			ENDIF
			SKIP IN projectitref_vw
		ENDDO
		IF USED("cursIPItem")
			USE IN cursipitem
		ENDIF
		WITH mobj.voupage.pgappduties.grdappduties
			.REFRESH()
		ENDWITH
	ENDIF
ENDIF
ENDPROC
&& End : Added by Uday on 26/12/2011 for EXIM


***** Added By Sachin N. S. on 17/09/2012 for Bug-5164 ***** Start
***** Procedure to generate Service Tax Serial No.
Proc Gen_ServTxSrno
Parameters SRNoType,_mcommit,_nhandle

Local srno
sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)
nhandle=0
sq1="SELECT ISNULL(MAX(CONVERT(INT,NPGNO)),0) AS NPGNO FROM GEN_SRNO WHERE CTYPE = '"+Alltrim(SRNoType)+"' and l_yn='"+Alltrim(main_vw.l_yn)+"' group by CTYPE,l_yn "
nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"EXCUR",IIF(TYPE('_nHandle')='L',"nHandle",_nHandle),_Screen.ActiveForm.DataSessionId)
If nretval<0
	Return .F.
ENDIF
SELECT excur
_mrgpage=ALLTRIM(STR(IIF(ISNULL(excur.NPGNO),1,(excur.NPGNO)+1)))

If !Used('Gen_SrNo_Vw')
	etsql_str = "Select * From Gen_SrNo where 1=0"
	etsql_con = sqlconobj.dataconn([EXE],company.dbname,etsql_str,[Gen_SrNo_Vw],"nHandle",_Screen.ActiveForm.DataSessionId,.F.)
	If etsql_con < 1 Or !Used("Gen_SrNo_Vw")
		etsql_con = 0
	Else
		Select gen_srno_vw
		Index On itserial Tag itserial
	Endif
Endif

=sqlconobj.sqlconnclose("nHandle")

Select gen_srno_vw
Scan
	If Alltrim(_mrgpage) <= Allt(gen_srno_vw.npgno) AND cType = SRNoType AND Tran_cd!=Main_vw.Tran_cd
		_mrgpage = Alltrim(Str(Iif(Isnull(gen_srno_vw.npgno),0,Val(gen_srno_vw.npgno)) + 1))
	Endif
	Select gen_srno_vw
Endscan

If Used("EXCUR")
	Use In excur
Endif
Return _mrgpage

***** Procedure to Check Duplicate Service Tax Serial No.
Procedure Dup_ServTxSrNo
Parameters SRNoType,fldval,_itSerial,_SerTy,_mCommit,_nHandle
_malias = Alias()
_mrecno	= Recno()

Set Notify Off 	&& Added by Shrikant S. on 16/11/2012 for Bug-7227
Local vdup
If fldval=' '
	Return .T.
Endif
_itSerial = Iif(Type('_itSerial')='L','',_itSerial)
_SerTy = Iif(Type('_SerTy')='L','',_SerTy)

sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)
nhandle=0
*!*	sq1="SELECT ISNULL(CONVERT(INT,NPGNO),0) AS NPGNO FROM GEN_SRNO WHERE CTYPE = '"+Alltrim(SRNoType)+"' and l_yn='"+Alltrim(main_vw.l_yn)+"' and ISNULL(CONVERT(INT,NPGNO),0) = "+Transform(fldval)	&& Commented By Shrikant S. on 16/11/2012 for Bug-7227
sq1="SELECT ISNULL(CONVERT(INT,NPGNO),0) AS NPGNO FROM GEN_SRNO WHERE CTYPE = '"+Alltrim(SRNoType)+"' and l_yn='"+Alltrim(main_vw.l_yn)+;
	"' and ISNULL(CONVERT(INT,NPGNO),0) = "+Transform(fldval)+" AND NOT (TRAN_CD="+Str(main_vw.tran_cd)+" AND ENTRY_TY='"+main_vw.entry_ty+"')"		&& Added by Shrikant S. on 16/11/2012 for Bug-7227
nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"EXCUR",IIF(TYPE('_nHandle')='L',"nHandle",_nHandle),_Screen.ActiveForm.DataSessionId)
If nretval<0
	Return .F.
Endif
=sqlconobj.sqlconnclose("nHandle")

Select excur
vrcount=Reccount()
If Used("EXCUR")
	Use In excur
Endif

If !Empty(_malias)
	Select &_malias
Endif

If Betw(_mrecno,1,Reccount())
	Go _mrecno
Endif

If vrcount>0 And !Isnull(vrcount)
	Return .F.
Else
	If nretval > 0 
		Select gen_srno_vw
*!*			Locate For itserial = _itserial And serty = Alltrim(_SerTy) AND ctype=SRNoType		&& Commented By Shrikant S. on 09/11/2012 for Bug-7227
		Locate For itserial = _itSerial And ctype=SRNoType			&& Added By Shrikant S. on 09/11/2012 for Bug-7227
		If !Found()
			Append Blank In gen_srno_vw
		Endif
		If Inlist(ctype,"SERVTXSRNO") Or Empty(ctype)
			Replace npgno With fldval,;
				itserial With _itSerial,ctype With 'SERVTXSRNO',;
				l_yn With main_vw.l_yn In gen_srno_vw
				*,serty With _SerTy		Removed Serty Replace for Bug-7227 by Shrikant S.
		Endif
		_mrgret  = 0
	Endif

	Return .T.
Endif
Return

***** Added By Sachin N. S. on 17/09/2012 for Bug-5164 ***** End


***** Added by Sachin N. S. on 21/11/2012 for Bug-7313 ***** Start
Procedure chkDuplSerialNo
Lparameters _txtObj,_valCur,_CurRecond
_valCur1  = Alltrim(_valCur)
_curform  = _Screen.ActiveForm
_FldSrc   = Justext(_txtObj.Parent.ControlSource)

If _ItSrTrn.Mode='D' Or Eof('_ItSrTrn')
	Return
Endif

If Empty(_valCur)
	_txtObj.cErrMsg="'"+Alltrim(_txtObj.Parent.Header1.Caption)+" cannot be empty.'"
	_txtObj.Parent.SetFocus()
	Return .F.
Endif

_curretval = _curform.sqlconobj.dataconn([EXE],company.dbname,"select top 1 "+Alltrim(_FldSrc)+" from It_srStk where "+Alltrim(_FldSrc)+"= ?_valCur1 and "+_CurRecond,"_TmpAlloc","This.Parent.nHandle",_curform.DataSessionId,.F.)
_curform.sqlconobj.sqlconnclose("This.Parent.nHandle")
If _curretval > 0 And Used('_TmpAlloc')
	Select _tmpalloc
	If Reccount()>0
		_txtObj.cErrMsg="'Duplicate "+Alltrim(_txtObj.Parent.Header1.Caption)+". Cannot continue...!!'"
		_txtObj.Parent.SetFocus()
		Return .F.
	Else
		Select _ItSrTrn
		m.InEntry_ty = _ItSrTrn.entry_ty
		m.InTran_cd  = _ItSrTrn.tran_cd
		m.InItSerial = _ItSrTrn.itserial
		m.iTran_cd	 = _ItSrTrn.iTran_cd
		m.it_code	 = _ItSrTrn.it_code
		_CurRecond = " not (InEntry_ty = '"+Transform(m.InEntry_ty)+"' and InTran_cd = "+Transform(m.InTran_cd)+" and InItSerial='"+Transform(m.InItSerial)+"' and iTran_cd="+Transform(m.iTran_cd)+") and It_Code="+Transform(m.it_code)+" and Mode!='D' "
		csql="select top 1 "+Alltrim(_FldSrc)+" from _ItSrTrn where ALLTRIM("+Alltrim(_FldSrc)+")== _valCur1 and "+_CurRecond+" order by "+Alltrim(_FldSrc)+" into cursor _tmpalloc"
		&csql
		Select _tmpalloc
		If Reccount()>0
			_txtObj.cErrMsg="'Duplicate "+Alltrim(_txtObj.Parent.Header1.Caption)+". Cannot continue...!!'"
			_txtObj.Parent.SetFocus()
			Return .F.
		Endif
	Endif
Endif
Return .T.
Endproc
***** Added by Sachin N. S. on 21/11/2012 for Bug-7313 ***** End
