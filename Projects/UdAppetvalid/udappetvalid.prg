*:*****************************************************************************
*:        Program: UETRIGETVALID--Udyog ERP
*:        System : Udyog Software
*:        Author :
*: 		  Last modified:
*:		  AIM    : To Call function from Lother.dbf (val_con,whn_con,def_val) /Dcmast.dbf/frx files.
*:*****************************************************************************
Procedure chk_pageno()
Parameters mcommit,nhand	&& Added by Shrikant S. on 29/09/2010 for TKT-4021
_mrgret  = 0
_mrgpage = item_vw.u_pageno		&&FIELDS
&& Added by Shrikant S. on 29/09/2010 for TKT-4021		--Start
If Used('Gen_SrNo_Vw')	&& 270910
	Select gen_srno_vw
	mrec=Recno()
Endif
&& Added by Shrikant S. on 29/09/2010 for TKT-4021		--End
If !Empty(_mrgpage)
*	_MRgRet  = -1  &&FIELDS
	_malias 	= Alias()
	Sele item_vw
	_mrecno 	= Recno()
	etsql_con	= 1
	nhandle     = 0
	_etdatasessionid = _Screen.ActiveForm.DataSessionId
	Set DataSession To _etdatasessionid
	sqlconobj = Newobject('SqlConnUdObj','SqlConnection',xapps)

	If !Used('Gen_SrNo_Vw')
		etsql_str = "Select * From Gen_SrNo where 1=0"
		etsql_con = sqlconobj.dataconn([EXE],company.dbname,etsql_str,[Gen_SrNo_Vw],;
			"nHandle",_etdatasessionid,.F.)
		If etsql_con < 1 Or !Used("Gen_SrNo_Vw")
			etsql_con = 0
		Else
			Select gen_srno_vw
			Index On itserial Tag itserial
		Endif
	Endif
	If etsql_con > 0
		_mitcode 	= item_vw.it_code
		_mitgrp 	= ''
		_mitchap   	= ''
		_mitserial  = item_vw.itserial
		_mittype = ''
		_mitdate = item_vw.Date  &&Sandeep 03/02/2011 for TKT-4596
		If Uppe(coadditional.rg23_srno) = 'G' Or Uppe(coadditional.rg23_srno) = 'C'
			etsql_str = "Select Top 1 [Group],[ChapNo] From It_mast where It_code=?_mitcode"
			etsql_con = sqlconobj.dataconn([EXE],company.dbname,etsql_str,[TmpEt_Vw],;
				"nHandle",_etdatasessionid,.F.)
			If etsql_con > 0 And Used("TmpEt_Vw")
				_mitgrp 	= tmpet_vw.Group
				_mitchap   	= tmpet_vw.chapno
			Else
				etsql_con = 0
			Endif
		Endif

		If Uppe(coadditional.rg23_srno) = 'D' &&rup
			etsql_str = "Select Top 1 [type] From It_mast where It_code=?_mitcode"
			etsql_con = sqlconobj.dataconn([EXE],company.dbname,etsql_str,[TmpEt1_Vw],;
				"nHandle",_etdatasessionid,.F.)
			If etsql_con > 0 And Used("TmpEt1_Vw")
				_mittype 	= tmpet1_vw.Type
			Else
				etsql_con = 0
			Endif
		Endif
	Endif

	If etsql_con > 0
		_mcond = "l_yn='"+Alltrim(main_vw.l_yn)+"' and "+"cType = 'RGPART1' And " + Iif(coadditional.cate_srno," Ccate = Main_vw.Cate And ","")
		Do Case
		Case Uppe(coadditional.rg23_srno) = 'I'
			_mcond = _mcond+" Cit_code = _mitcode "
		Case Uppe(coadditional.rg23_srno) = 'G'
			_mcond = _mcond+" Cgroup = _mitgrp "	&&Changes has been done by Vasant on 23/10/2012 as per Bug 6982 (Issue at the time of saving transaction).
		Case Uppe(coadditional.rg23_srno) = 'C'
			_mcond = _mcond+" Cchapno = _mitchap "	&&Changes has been done by Vasant on 23/10/2012 as per Bug 6982 (Issue at the time of saving transaction).
		Case Uppe(coadditional.rg23_srno) = 'D'
&&sandeep -->start 03/02/2011 for TKT-4596
			If (_mittype # 'Machinery/Stores')
				_mittype='Raw material'
			Endif
			_mcond = _mcond+" Cittype = _mittype"   &&Sandeep<---end 03/02/2011 for TKT-4596	&&Changes has been done by Vasant on 23/10/2012 as per Bug 6982 (Issue at the time of saving transaction).

		Other
			_mcond = _mcond+" 1 = 1 "
		Endcase
		Select gen_srno_vw
		Scan
			If &_mcond
&&Changes has been done by vasant on 17/11/2012 as per Bug 7184 (Issue in RG23 Sr.No. When the same Item is taken in Second item level and the RG23 Sr. No is edited).
*IF gen_srno_vw.itserial != _mitserial AND ALLTRIM(gen_srno_vw.npgno) = ALLTRIM(_mrgpage)
				If gen_srno_vw.itserial != _mitserial And Alltrim(gen_srno_vw.npgno) == Alltrim(_mrgpage)
&&Changes has been done by vasant on 17/11/2012 as per Bug 7184 (Issue in RG23 Sr.No. When the same Item is taken in Second item level and the RG23 Sr. No is edited).
					_mrgret  = 1
				Endif
			Endif
		Endscan

		If _mrgret != 1
			_mcond = "l_yn='"+Alltrim(main_vw.l_yn)+"' and cType = 'RGPART1' And " + Iif(coadditional.cate_srno," Ccate = ?Main_vw.Cate And ","")
*_mcond = _mcond + " LTRIM(RTRIM(NPgNo)) = LTRIM(RTRIM(?_MRgPage)) And "	&&FIELDS	&&Changes has been done by vasant on 16/11/2012 as per Bug 7212 (Software should prompt message if RG23 part(I) No is higher).
			Do Case
			Case Uppe(coadditional.rg23_srno) = 'I'
				_mcond = _mcond+" Cit_code = ?_mitcode "
			Case Uppe(coadditional.rg23_srno) = 'G'
				_mcond = _mcond+" Cgroup = ?_mitgrp "			&&Changes has been done by Vasant on 23/10/2012 as per Bug 6982 (Issue at the time of saving transaction).
			Case Uppe(coadditional.rg23_srno) = 'C'
				_mcond = _mcond+" Cchapno = ?_mitchap "			&&Changes has been done by Vasant on 23/10/2012 as per Bug 6982 (Issue at the time of saving transaction).
			Case Uppe(coadditional.rg23_srno) = 'D'
&&sandeep--->start 03/02/2011 for TKT-4596
				If (_mittype # 'Machinery/Stores')
					_mittype='Raw material'
				Endif
				_mcond = _mcond+" Cittype = ?_mittype"  &&sandeep<--end 03/02/2011 for TKT-4596		&&Changes has been done by Vasant on 23/10/2012 as per Bug 6982 (Issue at the time of saving transaction).

			Other
				_mcond = _mcond+" 1 = 1 "
			Endcase

&&Changes has been done by vasant on 16/11/2012 as per Bug 7212 (Software should prompt message if RG23 part(I) No is higher).
			_mcond = _mcond + " And "
			_mcond1 = _mcond + " [Date] > ?Main_vw.Date And Cast(NPgNo as Numeric(10)) < Cast(?_MRgPage as Numeric(10)) "
			_mcond = _mcond + " LTRIM(RTRIM(NPgNo)) = LTRIM(RTRIM(?_MRgPage)) "
&&Changes has been done by vasant on 16/11/2012 as per Bug 7212 (Software should prompt message if RG23 part(I) No is higher).

			etsql_str = "Select Top 1 Entry_ty,Tran_cd,Itserial From Gen_SrNo where "+_mcond
			etsql_con = sqlconobj.dataconn([EXE],company.dbname,etsql_str,[TmpEt_Vw],;
				IIF(mcommit=.F.,"nHandle",nhand),_etdatasessionid,mcommit)	&& Added by Shrikant S. on 29/09/2010 for TKT-4021
*!*						"nHandle",_etdatasessionid,.F.)			&& Commented by Shrikant S. on 29/09/2010 for TKT-4021

			If etsql_con > 0 And Used("TmpEt_Vw")
				Select tmpet_vw
&&Changes has been done by vasant on 17/11/2012 as per Bug 7184 (Issue in RG23 Sr.No. When the same Item is taken in Second item level and the RG23 Sr. No is edited).
*IF RECCOUNT() > 0 AND entry_ty+STR(tran_cd)+itserial # main_vw.entry_ty+STR(main_vw.tran_cd)+_mitserial
				If Reccount() > 0 And entry_ty+Str(tran_cd) # main_vw.entry_ty+Str(main_vw.tran_cd)
&&Changes has been done by vasant on 17/11/2012 as per Bug 7184 (Issue in RG23 Sr.No. When the same Item is taken in Second item level and the RG23 Sr. No is edited).
					_mrgret  = 1
				Else
&&Changes has been done by vasant on 16/11/2012 as per Bug 7212 (Software should prompt message if RG23 part(I) No is higher).
					etsql_str = "Select Top 1 cType,[Date],NPgNo From Gen_SrNo where "+_mcond1
					etsql_con = sqlconobj.dataconn([EXE],company.dbname,etsql_str,[TmpEt_Vw],;
						IIF(mcommit=.F.,"nHandle",nhand),_etdatasessionid,mcommit)	&& Added by Shrikant S. on 29/09/2010 for TKT-4021
					If etsql_con > 0 And Used("TmpEt_Vw")
						Select tmpet_vw
						If Reccount() > 0
							If !mcommit	&&Changes done by vasant on 23/08/2013 as per Bug-18583 (Issue in Input to Production).
								=Messagebox('RG Part 1 No generated is higher than the RG Part 1 No generated on '+Dtoc(Ttod(tmpet_vw.Date)),48,vumess)
							Endif		&&Changes done by vasant on 23/08/2013 as per Bug-18583 (Issue in Input to Production).
						Endif
					Endif
&&Changes has been done by vasant on 16/11/2012 as per Bug 7212 (Software should prompt message if RG23 part(I) No is higher).

					Select gen_srno_vw
					Locate For itserial = _mitserial
					If !Found()
						Append Blank In gen_srno_vw
					Endif
					If Inlist(ctype,"RGPART1") Or Empty(ctype)		&& Added by Shrikant S. on 29/09/2010 for TKT-4021
						Replace ccate With main_vw.cate,npgno With _mrgpage,;
							itserial With item_vw.itserial,cware With item_vw.ware_nm,ctype With 'RGPART1',;
							cit_code With _mitcode,cgroup With _mitgrp,cchapno With _mitchap,cittype With _mittype,l_yn With main_vw.l_yn In gen_srno_vw
					Endif
					_mrgret  = 0
				Endif
			Endif
		Endif
	Endif
	If Used("TmpEt_Vw")
		Use In tmpet_vw
	Endif
	=sqlconobj.sqlconnclose("nHandle")
	Sele item_vw
	If Betw(_mrecno,1,Reccount())
		Go _mrecno
	Endif
	If !Empty(_malias)
		Select &_malias
	Endif
Endif
Return Iif(_mrgret = 0,.T.,.F.)
*******************************************************************************************************

Procedure gen_pageno()
_mrgpage = item_vw.u_pageno		&&FIELDS
If Empty(_mrgpage)
	_malias 	= Alias()
	Sele item_vw
	_mrecno 	= Recno()
	etsql_con	= 1
	nhandle     = 0
	_etdatasessionid = _Screen.ActiveForm.DataSessionId
	Set DataSession To _etdatasessionid
	sqlconobj = Newobject('SqlConnUdObj','SqlConnection',xapps)

&&-->Rup  12Aug09
	etsql_str = "select tp=[type] from it_mast where it_code="+Str(item_vw.it_code)
	etsql_con = sqlconobj.dataconn([EXE],company.dbname,etsql_str,[itype],"nHandle",_etdatasessionid,.F.)
	If etsql_con < 1 Or !Used("itype")
		etsql_con = 0
	Else
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
		etsql_con = sqlconobj.dataconn([EXE],company.dbname,etsql_str,[Gen_SrNo_Vw],;
			"nHandle",_etdatasessionid,.F.)
		If etsql_con < 1 Or !Used("Gen_SrNo_Vw")
			etsql_con = 0
		Else
			Select gen_srno_vw
			Index On itserial Tag itserial
		Endif
	Endif
	If etsql_con > 0
		_mitcode 	= item_vw.it_code
		_mitgrp 	= ''
		_mitchap   	= ''
		_mitdate 	=item_vw.Date  &&sandeep  03/02/2011 for TKT-4596
		_mittype	= '' &&Birendra : Bug-21318 on 10/01/2014

		If Uppe(coadditional.rg23_srno) = 'G' Or Uppe(coadditional.rg23_srno) = 'C'
			etsql_str = "Select Top 1 [Group],[ChapNo] From It_mast where It_code=?_mitcode"
			etsql_con = sqlconobj.dataconn([EXE],company.dbname,etsql_str,[TmpEt_Vw],;
				"nHandle",_etdatasessionid,.F.)
			If etsql_con > 0 And Used("TmpEt_Vw")
				_mitgrp 	= tmpet_vw.Group
				_mitchap   	= tmpet_vw.chapno
			Else
				etsql_con = 0
			Endif
		Endif
	Endif
	If Uppe(coadditional.rg23_srno) = 'D' &&rup
		etsql_str = "Select Top 1 [type] From It_mast where It_code=?_mitcode"
		etsql_con = sqlconobj.dataconn([EXE],company.dbname,etsql_str,[TmpEt1_Vw],;
			"nHandle",_etdatasessionid,.F.)
		If etsql_con > 0 And Used("TmpEt1_Vw")
			_mittype 	= tmpet1_vw.Type
		Else
			etsql_con = 0
		Endif
	Endif


	If etsql_con > 0
		_mcond ="l_yn='"+Alltrim(main_vw.l_yn)+"' and cType = 'RGPART1' And " + Iif(coadditional.cate_srno," Ccate = Main_vw.Cate And ","")
		Do Case
		Case Uppe(coadditional.rg23_srno) = 'I'
			_mcond = _mcond+" Cit_code = _mitcode "
		Case Uppe(coadditional.rg23_srno) = 'G'
			_mcond = _mcond+" Cgroup = _mitgrp "		&&Changes has been done by Vasant on 23/10/2012 as per Bug 6982 (Issue at the time of saving transaction).
		Case Uppe(coadditional.rg23_srno) = 'C'
			_mcond = _mcond+" Cchapno = _mitchap "			&&Changes has been done by Vasant on 23/10/2012 as per Bug 6982 (Issue at the time of saving transaction).
		Case Uppe(coadditional.rg23_srno) = 'D' &&rup
&&sandeep--->start 03/02/2011 for TKT-4596
			If (_mittype # 'Machinery/Stores')
				_mittype='Raw material'
			Endif
			_mcond = _mcond+" cittype = _mittype "   &&sandeep<--end 03/02/2011 for TKT-4596		&&Changes has been done by Vasant on 23/10/2012 as per Bug 6982 (Issue at the time of saving transaction).

		Other
			_mcond = _mcond+" 1 = 1 "
		Endcase

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

		Select Max(Cast(npgno As Int)) As PageNo From gen_srno_vw With (Buffering = .T.) ;
			where &_mcond Into Cursor tmpet_vw
		_mrgpage1=0
		If Used('TmpEt_Vw')
			If Reccount('TmpEt_Vw') > 0
				_mrgpage1=Iif(Isnull(tmpet_vw.PageNo),0,tmpet_vw.PageNo)
				If _mrgpage1 !=0
					_mrgpage = Alltrim(Str(_mrgpage1 + 1))
				Endif
			Endif
		Endif
&&Changes has been done by vasant on 17/11/2012 as per Bug 7184 (Issue in RG23 Sr.No. When the same Item is taken in Second item level and the RG23 Sr. No is edited).

&&IF EMPTY(_mrgpage)
		_mcond ="l_yn='"+Alltrim(main_vw.l_yn)+"' and cType = 'RGPART1' And " + Iif(coadditional.cate_srno," Ccate = ?Main_vw.Cate And ","")
		Do Case
		Case Uppe(coadditional.rg23_srno) = 'I'
			_mcond = _mcond+" Cit_code = ?_mitcode "
		Case Uppe(coadditional.rg23_srno) = 'G'
			_mcond = _mcond+" Cgroup = ?_mitgrp "			&&Changes has been done by Vasant on 23/10/2012 as per Bug 6982 (Issue at the time of saving transaction).
		Case Uppe(coadditional.rg23_srno) = 'C'
			_mcond = _mcond+" Cchapno = ?_mitchap "			&&Changes has been done by Vasant on 23/10/2012 as per Bug 6982 (Issue at the time of saving transaction).
		Case Uppe(coadditional.rg23_srno) = 'D' &&rup
&&sandeep--->start 03/02/2011 for TKT-4596
			If (_mittype # 'Machinery/Stores')
				_mittype='Raw material'
*_mcond = _mcond+" Cittype = ?_mittype" &&sandeep<--end 03/02/2011 for TKT-4596			&&Changes has been done by Vasant on 23/10/2012 as per Bug 6982 (Issue at the time of saving transaction).	&& Commented by Shrikant S. on 10/06/2013 for Bug-15585
			Endif
			_mcond = _mcond+" Cittype = ?_mittype" 	&& Added by Shrikant S. on 10/06/2013 for Bug-15585

		Other
			_mcond = _mcond+" 1 = 1 "
		Endcase
		_mrgpage1=0		&&added by satish pal for bug-6425 dated 17/09/2012
		etsql_str = "Select Max(Cast(nPgNo as int)) as PageNo From Gen_SrNo where "+_mcond
		etsql_con = sqlconobj.dataconn([EXE],company.dbname,etsql_str,[TmpEt_Vw],;
			"nHandle",_etdatasessionid,.F.)
		If etsql_con > 0 And Used("TmpEt_Vw")
&&_mrgpage = ALLTRIM(STR(IIF(ISNULL(tmpet_vw.PAGENO),0,tmpet_vw.PAGENO) + 1))&&commented by satish pal bug-6425 dated 17/09/2012
			_mrgpage1 = Alltrim(Str(Iif(Isnull(tmpet_vw.PageNo),0,tmpet_vw.PageNo) + 1)) 	&&added by satish pal dated 17/09/2012 bug-6425 for get max no of rgpage
		Endif
		_mrgpage =Iif(_mrgpage1>_mrgpage,_mrgpage1,_mrgpage)	&&added by satish pal dated 17/09/2012 for get max no of rgpage
&&ENDIF
	Endif
	If etsql_con <= 0
		_mrgpage = '***'
	Endif
	If Used("TmpEt_Vw")
		Use In tmpet_vw
	Endif
	=sqlconobj.sqlconnclose("nHandle")
	Sele item_vw
	If Betw(_mrecno,1,Reccount())
		Go _mrecno
	Endif
&&Changes has been done by vasant on 17/11/2012 as per Bug 7230 (RG 23 Part 1 no is not generating error).
	_mrgpage = Padr(_mrgpage,Len(item_vw.u_pageno))
	If Used('Gen_SrNo_Vw') And etsql_con > 0 And !Empty(_mrgpage)
		Select gen_srno_vw
		Locate For itserial = item_vw.itserial And Alltrim(ctype) == "RGPART1"
		If !Found()
			Append Blank In gen_srno_vw
		Endif
		Replace ccate With main_vw.cate,npgno With _mrgpage,;
			itserial With item_vw.itserial,cware With item_vw.ware_nm,ctype With 'RGPART1',;
			cit_code With _mitcode,cgroup With _mitgrp,cchapno With _mitchap,cittype With _mittype,l_yn With main_vw.l_yn In gen_srno_vw
	Endif
&&Changes has been done by vasant on 17/11/2012 as per Bug 7230 (RG 23 Part 1 no is not generating error).
	If !Empty(_malias)
		Select &_malias
	Endif

*_mrgpage = PADR(_mrgpage,LEN(item_vw.u_pageno))		&&FIELDS	&&Changes has been done by vasant on 17/11/2012 as per Bug 7230 (RG 23 Part 1 no is not generating error).
Endif
Return _mrgpage
*******************************************************************************************************


**Start ST_CHK_APPACK() --TKT 495
Proc st_chk_appack() &&RUP:-procedure is useful to update default value of  average qty,qty,mrprate,abtper etc. as per it_mast values
malias = Alias()
Local vappack,vmrprate,vabtper
sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)
nhandle=0
sq1="SELECT average,mrprate,abtper FROM IT_MAST WHERE IT_CODE ="+Str(item_vw.it_code)
nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"EXCUR","nHandle",_Screen.ActiveForm.DataSessionId)
If nretval<0
	Return .F.
Endif
Select excur
vappack=Iif(!Isnull(excur.Average),excur.Average,0)
vmrprate=Iif(!Isnull(excur.mrprate),excur.mrprate,0)
vabtper=Iif(!Isnull(excur.abtper),excur.abtper,0)
If Used("EXCUR")
	Use In excur
Endif

Replace u_appack With Iif(item_vw.u_appack=' ',Alltrim(Str(vappack)),item_vw.u_appack),u_mrprate With Iif(item_vw.u_mrprate=0,vmrprate,item_vw.u_mrprate),abtper With Iif(item_vw.abtper=0,vabtper,item_vw.abtper) In item_vw
_mqty = 0
If Val(item_vw.u_pkno)#0 And Val(item_vw.u_appack)#0
*!*		REPLACE qty WITH (VAL(item_vw.u_pkno) * VAL(item_vw.u_appack)) IN item_vw &&Comment By Hetal Dt 19/02/2010
	_mqty = (Val(item_vw.u_pkno) * Val(item_vw.u_appack))
Endif
***Added By Hetal Dt 19/02/2010 Check DC QTY &&St
If _mqty # 0
	If  (([vuexc] $ vchkprod) Or ([vuinv] $ vchkprod)) And Type('main_vw.entry_ty')='C' &&Check Existing Records
*!*			IF main_vw.entry_ty='ST'
		If main_vw.entry_ty='ST' And Used('_DetailData')	&& Changed By Sachin N. S. on 28/05/2010 for TKT-2055
			Store 0 To _balqty
			Store item_vw.entry_ty To centryty
			Store item_vw.tran_cd To ctrancd
			Store item_vw.it_code To citcode
			Store item_vw.itserial To citserial
*!* Pending Quantity
			Select a.balqtynew balqty From _detaildata a ;
				inner Join itref_vw b On(a.entry_ty = b.rentry_ty And a.itserial = b.ritserial And a.it_code = b.it_code And a.l_yn = b.rl_yn And a.inv_no = b.rinv_no And a.inv_sr = b.rinv_sr);
				WHERE b.entry_ty = centryty And b.tran_cd = ctrancd And b.it_code = citcode And b.itserial = citserial ;
				INTO Cursor pendqty
*!*
			Select pendqty
			Store balqty To _balqty
			If _mqty >_balqty And _balqty #0
				Replace u_pkno With '' In item_vw
				=Messagebox('Quantity could not be greater then '+Alltrim(Str(_balqty,14,3)),0+64,vumess)
				Select (malias)
				Return .F.
			Endif
		Endif
	Endif
	Replace qty With _mqty In item_vw
Endif
***Added By Hetal Dt 19/02/2010 Check DC QTY &&Ed
***Added By Hetal Dt 18/02/2010 Check IP QTY &&Ed
***Added By SATISH PAL Dt 05/08/2013 FOR BUG-18284-STATRT
If !Empty(malias)
	Sele(malias)
Endif
**	SELE(malias)
***Added By SATISH PAL Dt 05/08/2013 FOR BUG-18284-END

&& Added by Suraj for Bug-26975 Start
frmxtra.txtu_appack.SelectOnEntry = .T.
frmxtra.txtu_appack.Refresh()
&& added by Suraj for Bug-26975 End

Return .T.
***End ST_CHK_APPACK()

**Start OP_CHK_APPACK() TKT 479
Proc op_chk_appack() &&RUP:-procedure is useful to update default value of average qty,qty etc. as per it_mast values && 04Oct09
malias = Alias()

Local vappack,vmrprate,vabtper
sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)
nhandle=0

sq1="SELECT average FROM IT_MAST WHERE IT_CODE ="+Str(item_vw.it_code)
nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"EXCUR","nHandle",_Screen.ActiveForm.DataSessionId)
If nretval<0
	Return .F.
Endif

Select excur
vappack=Iif(!Isnull(excur.Average),excur.Average,0)

If Used("EXCUR")
	Use In excur
Endif

Replace u_appack With Iif(item_vw.u_appack=' ',Alltrim(Str(vappack)),item_vw.u_appack) In item_vw
_mqty = 0
If Val(item_vw.u_pkno)#0 And Val(item_vw.u_appack)#0
*!*		REPLACE qty WITH (VAL(item_vw.u_pkno) * VAL(item_vw.u_appack)) IN item_vw &&Changed By Hetal Dt 18/02/2010
	_mqty = (Val(item_vw.u_pkno) * Val(item_vw.u_appack))
Endif

***Added By Hetal Dt 18/02/2010 Check IP QTY &&St
If _mqty # 0
	If  (([vuexc] $ vchkprod) Or ([vuinv] $ vchkprod)) And Type('main_vw.entry_ty')='C' &&Check Existing Records
*!*			If main_vw.entry_ty='OP'		&& Changed By Sachin N. S. on 01/02/2011 for TKT-5729
		If main_vw.entry_ty='OP' And Used('Projectitref_vw')
			nretval = 0
			nhandle = 0
			etsql_con  = 0
			nhandle    = 0
			Select aentry_ty,atran_cd,aitserial,qty From projectitref_vw Where entry_ty=main_vw.entry_ty And tran_cd=main_vw.tran_cd And itserial=item_vw.itserial Into Cursor tibl
			etsql_str  = ""
			etsql_str = "USP_ENT_CHK_OP_ALLOCATION '"+item_vw.entry_ty+"',"+Alltrim(Str(item_vw.tran_cd))+","+Alltrim(Str(item_vw.it_code))+",'"+item_vw.itserial+"','";
				+tibl.aentry_ty+"',"+Alltrim(Str(tibl.atran_cd))+",'"+tibl.aitserial+"'"
			etsql_con = sqlconobj.dataconn([EXE],company.dbname,etsql_str,[tibl_1],"nHandle",_Screen.ActiveForm.DataSessionId)
			If etsql_con >0
				If Used('tibl_1')
					If ((_mqty >tibl_1.wipqty) And tibl_1.wipqty<>0)
						Replace u_pkno With '' In item_vw
						=Messagebox('Quantity could not be greater then '+Alltrim(Str(tibl_1.wipqty,14,3)),0+64,vumess)
						Select (malias)
						Return .F.
					Endif
					Use In tibl_1
				Endif
			Else
				Select (malias)
				Return .F.
			Endif
			Use In tibl
		Endif
	Endif
	Replace qty With _mqty In item_vw
Endif
***Added By Hetal Dt 18/02/2010 Check IP QTY &&Ed
***Added By Hetal Dt 18/02/2010 Check IP QTY &&Ed
***Added By SATISH PAL Dt 05/08/2013 FOR BUG-18284-STATRT
If !Empty(malias)
	Sele(malias)
Endif
**	SELE(malias)
***Added By SATISH PAL Dt 05/08/2013 FOR BUG-18284-END
Return .T.
***END OP_CHK_APPACK()

*/*/*/*/*/*/*

*&&Added by Amrendra on on 30/03/2011 for TKT 6785       -- Start
Procedure getabetment() &&AKS:-procedure is useful to update default value of abtment % and MRP Rate in Sales
Local vmrprate,vabtper,cabtper
malias = Alias()  && Added by Amrendra for TKT 7333 on 19/05/2011
sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)
nhandle=0
sq1="select mrprate,abtper from it_mast WHERE IT_CODE ="+Str(item_vw.it_code)
nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"EXCUR","nHandle",_Screen.ActiveForm.DataSessionId)
If nretval<0
	Return .F.
Endif
Select excur
vmrprate=Iif(!Isnull(excur.mrprate),excur.mrprate,0)
*cabtper=Iif(Iif(Isnull(coadditional.abtper),0,coadditional.abtper)>=100,0,100-coadditional.abtper) && Commented by Amrendra for TKT 7333 on 19/05/2011
cabtper=Iif(Iif(Isnull(coadditional.abtper),0,coadditional.abtper)>=100,0,Iif(coadditional.abtper=0,0,100-coadditional.abtper)) && Added by Amrendra for TKT 7333 on 19/05/2011
vabtper=Iif(!Isnull(excur.abtper),excur.abtper,0)
vabtper=Iif(vabtper#0,vabtper,cabtper)

If item_vw.u_mrprate=0
	Replace u_mrprate With vmrprate In item_vw
Endif
If item_vw.abtper=0
	Replace abtper With vabtper In item_vw
Endif
If Used("EXCUR")
	Use In excur
Endif
frmxtra.txtabtper.Refresh
frmxtra.txtu_mrprate.Refresh
Select(malias) && Added by Amrendra for TKT 7333 on 19/05/2011
Return .T.
*&&Added by Amrendra on on 30/03/2011 for TKT 6785       -- End

*/*/*/*/*/*/*

*!*	&& Commented the whole method By Shrikant S. on 28/05/2013 for Bug-12163	&& Start
*!*	PROC st_ass_whn() &&RUP:-procedure is useful to update default value of assessable amount with/without (mrprate and abtper),round-off value in Sales

*!*	*!*	**********Commented by Amrendra on on 28/03/2011 for TKT 6785   ------Start
*!*	*!*	&& Added By Shrikant S. on 25/02/2011 for TKT-4580	***	Start
*!*	*!*	Local vmrprate,vabtper,cabtper
*!*	*!*	cabtper=0
*!*	*!*	sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)
*!*	*!*	nhandle=0

*!*	*!*	sq1="select mrprate,abtper from it_mast WHERE IT_CODE ="+Str(item_vw.it_code)
*!*	*!*	nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"EXCUR","nHandle",_Screen.ActiveForm.DataSessionId)
*!*	*!*	If nretval<0
*!*	*!*		Return .F.
*!*	*!*	Endif
*!*	*!*	Select excur
*!*	*!*	vmrprate=Iif(!Isnull(excur.mrprate),excur.mrprate,0)
*!*	*!*	cabtper=Iif(Iif(Isnull(coadditional.abtper),0,coadditional.abtper)>=100,0,100-coadditional.abtper)
*!*	*!*	vabtper=Iif(!Isnull(excur.abtper),excur.abtper,0)
*!*	*!*	vabtper=Iif(vabtper#0,vabtper,cabtper)
*!*	*!*	Replace u_mrprate With vmrprate In item_vw
*!*	*!*	Replace abtper With vabtper In item_vw

*!*	*!*	If Used("EXCUR")
*!*	*!*		Use In excur
*!*	*!*	Endif
*!*	*!*	&& Added By Shrikant S. on 25/02/2011 for TKT-4580	***	End
*!*	*!*	*********Commented by Amrendra on on 28/03/2011 for TKT 6785   ------End
*!*	SELECT item_vw

*!*	IF item_vw.u_mrprate#0
*!*		SELE item_vw
*!*		IF item_vw.abtper#0
*!*			REPL u_asseamt WITH ROUND((qty*u_mrprate)-(qty*u_mrprate*abtper)/100,2) IN item_vw
*!*		ELSE
*!*			REPL u_asseamt WITH ROUND((qty*u_mrprate),2) IN item_vw
*!*		ENDIF

*!*	*!*	** Commented by Amrendra+Rupesh on 06/05/2011 for TKT 7333 Start
*!*	*!*	    Repl rate With Iif(rate=0,Round(u_mrprate-((u_mrprate*abtper)/100),2),rate) In item_vw  &&Commented by Amrendra on on 28/03/2011 for TKT 6785
*!*	*!*	***	Repl rate With Round(u_mrprate-((u_mrprate*abtper)/100),2) In item_vw   &&Added by Amrendra on on 28/03/2011 for TKT 6785
*!*	*!*	** Commented by Amrendra+Rupesh on 06/05/2011 for TKT 7333 End
*!*	ELSE
*!*		REPL u_asseamt WITH ROUND(qty*rate,2) IN item_vw
*!*	ENDIF


*!*	IF coadditional.rndavalue
*!*		REPLACE u_asseamt WITH ROUND(u_asseamt,0) IN item_vw
*!*	ENDIF

*!*	frmxtra.txtu_asseamt.REFRESH

*!*	***&&Added by Amrendra on on 28/03/2011 for TKT 6785       -- Start
*!*	oform=_SCREEN.ACTIVEFORM.fobject
*!*	oform.itemgrdbefcalc(1)
*!*	***&&Added by Amrendra on on 28/03/2011 for TKT 6785        -- End

*!*	RETURN .T.
*!*	&& Commented By Shrikant S. on 28/05/2013 for Bug-12163		&& End

&& Added By Shrikant S. on 28/05/2013 for Bug-12163		&& Start
Proc st_ass_whn()
oform=_Screen.ActiveForm.fobject
If oform.UdNewTrigEnbl
	If File('UDAppvouItemwiseAssess.app')
		Local vtemp
		vtemp=UDAppvouItemwiseAssess(oform)
		If !Empty(vtemp)
			mAValue = vtemp
		Endif
	Endif
	If File('UDTrigvouItemwiseAssess.fxp')
		Local vtemp
		vtemp=UDTrigvouItemwiseAssess(oform)
		If !Empty(vtemp)
			mAValue = vtemp
		Endif
	Endif
Else
	If File('UETrigvouItemwiseAssess.fxp')
		Local vtemp
		vtemp=UETrigvouItemwiseAssess(oform)
		If !Empty(vtemp)
			mAValue = vtemp
		Endif
	Endif
Endif

If Type('mAValue')='N'
	Repl u_asseamt With mAValue In item_vw
Endif
frmxtra.txtu_asseamt.Refresh
Return .T.
&& Added By Shrikant S. on 28/05/2013 for Bug-12163		&& End


Proc pt_ass_whn() &&RUP:-procedure is useful to update default value of assessable amount in Purchase entry
Sele item_vw
Repl u_asseamt With rate*qty  In item_vw
Retu



Proc pt_rate_def() &&RUP:-procedure is useful to update default value of rate calculation as per ass.value entred by user  in Purchase entry
Sele item_vw
Repl rate With (u_asseamt/qty) In item_vw
Retu .T.

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
Proc gen_no() &&RUP:-procedure is useful to generate part-ii,pla and in Daily Debit form UEFRM_ST_DAILTDEBIT in Sales Entry.
Parameters fldnm,tblnm,mcommit,nhand
pgno='Main_Vw.'+fldnm

Local vrno
sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)
nhandle=0

If !Used('Gen_SrNo_Vw')
	etsql_str = "Select * From Gen_SrNo where 1=0"
	etsql_con = sqlconobj.dataconn([EXE],company.dbname,etsql_str,[Gen_SrNo_Vw],;
		"nHandle",_Screen.ActiveForm.DataSessionId,.F.)
	If etsql_con < 1 Or !Used("Gen_SrNo_Vw")
		etsql_con = 0
	Else
		Select gen_srno_vw
		Index On itserial Tag itserial
	Endif
Endif

*sq1="SELECT MAX(CAST( Npgno AS INT)) AS RNO  FROM Gen_srno WHERE ISNUMERIC( NpgNo )=1 "+IIF(UPPER(fldnm)=='U_RG23NO'," AND CTYPE='RGAPART2'",IIF(UPPER(fldnm)=='U_RG23CNO'," AND CTYPE='RGCPART2'",IIF(UPPER(fldnm)=='U_PLASR'," AND CTYPE='PLASRNO'","")))+" and l_yn='"+ALLTRIM(main_vw.l_yn)+"' "+ IIF(coadditional.cate_srno," AND Ccate = ?Main_vw.Cate ","")  &&changes by sandeep Cate column rectified by ccate for bug-18474 ON 20-Aug-13
*Birendra: Bug-19986 on 10/10/2013 :Modified with below one:FORM402SRNO
*!*	sq1="SELECT MAX(CAST( Npgno AS INT)) AS RNO  FROM Gen_srno WHERE ISNUMERIC( NpgNo )=1 "+IIF(UPPER(fldnm)=='U_RG23NO'," AND CTYPE='RGAPART2'",IIF(UPPER(fldnm)=='U_RG23CNO'," AND CTYPE='RGCPART2'",IIF(UPPER(fldnm)=='U_PLASR'," AND CTYPE='PLASRNO'",IIF(UPPER(fldnm)=='U_402SRNO'," AND CTYPE='FORM402SRNO'",""))))+" and l_yn='"+ALLTRIM(main_vw.l_yn)+"' "+ IIF(coadditional.cate_srno," AND cate = ?Main_vw.Cate ","")&& commented by Archana K. on 16/12/13 for Bug-21083
&&	sq1="SELECT MAX(CAST( Npgno AS INT)) AS RNO  FROM Gen_srno WHERE ISNUMERIC( NpgNo )=1 "+IIF(UPPER(fldnm)=='U_RG23NO'," AND CTYPE='RGAPART2'",IIF(UPPER(fldnm)=='U_RG23CNO'," AND CTYPE='RGCPART2'",IIF(UPPER(fldnm)=='U_PLASR'," AND CTYPE='PLASRNO'",IIF(UPPER(fldnm)=='U_402SRNO'," AND CTYPE='FORM402SRNO'",""))))+" and l_yn='"+ALLTRIM(main_vw.l_yn)+"' "+ IIF(coadditional.cate_srno," AND Ccate = ?Main_vw.Cate ","")&&changed by Archana K. on 16/12/13 for Bug-21083 &&Commented by sandeep for bug-21327 on 08-02-2014
sq1="SELECT MAX(CAST( Npgno AS INT)) AS RNO  FROM Gen_srno WHERE ISNUMERIC( NpgNo )=1 "+Iif(Upper(fldnm)=='U_RG23NO'," AND CTYPE='RGAPART2'",Iif(Upper(fldnm)=='U_RG23CNO'," AND CTYPE='RGCPART2'",Iif(Upper(fldnm)=='U_PLASR'," AND CTYPE='PLASRNO'",Iif(Upper(fldnm)=='U_402SRNO'," AND CTYPE='FORM402SRNO'",Iif(Upper(fldnm)=='U_403SRNO'," AND CTYPE='FORM403SRNO'","")))))+" and l_yn='"+Alltrim(main_vw.l_yn)+"' "+ Iif(coadditional.cate_srno," AND Ccate = ?Main_vw.Cate ","")&&changed by Archana K. on 16/12/13 for Bug-21083  &&Change by sandeep for  bug-21327 on 08-02-2014

*nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"EXCUR","nHandle",_Screen.ActiveForm.DataSessionId)
nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"EXCUR",Iif(mcommit=.F.,"nHandle",nhand),_Screen.ActiveForm.DataSessionId)
If nretval<0
	Return .F.
Endif
Select excur
vrno=Alltrim(Str(Iif(Isnull(excur.rno),1,(excur.rno)+1)))

=sqlconobj.sqlconnclose("nHandle")
If Used("EXCUR")
	Use In excur
Endif

Return vrno
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
Procedure dup_no &&RUP:-procedure is useful to check duplicate value of part-ii,pla field and in Daily Debit form UEFRM_ST_DAILTDEBIT in Sales Entry.
Parameters fldnm,fldval,tblnm,mcommit,nhand
pgno='Main_Vw.'+fldnm
If !Empty(fldval)
	_malias 	= Alias()
	_mrecno	= Recno()
*	notype =IIF(UPPER(fldnm)=='U_RG23NO','RGAPART2',IIF(UPPER(fldnm)=='U_RG23CNO','RGCPART2',IIF(UPPER(fldnm)=='U_PLASR','PLASRNO',"")))
*Birendra: Bug-19986 on 10/10/2013 :Modified with below one:FORM402SRNO
&&	notype =IIF(UPPER(fldnm)=='U_RG23NO','RGAPART2',IIF(UPPER(fldnm)=='U_RG23CNO','RGCPART2',IIF(UPPER(fldnm)=='U_PLASR','PLASRNO',IIF(UPPER(fldnm)=='U_402SRNO','FORM402SRNO',"")))) &&commented by sandeep for bug-21327 on 08-02-2014
	notype =Iif(Upper(fldnm)=='U_RG23NO','RGAPART2',Iif(Upper(fldnm)=='U_RG23CNO','RGCPART2',Iif(Upper(fldnm)=='U_PLASR','PLASRNO',Iif(Upper(fldnm)=='U_402SRNO','FORM402SRNO',Iif(Upper(fldnm)=='U_403SRNO','FORM403SRNO',""))))) &&change by sandeep for bug-21327 on 08-02-2014
	Local vdup
	If fldval=' '
		Return .T.
	Endif
	sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)

	nhandle=0
	If Used('Gen_SrNo_Vw')
		Select gen_srno_vw
		mrec=Recno()
	Endif
	If !Used('Gen_SrNo_Vw')
		etsql_str = "Select * From Gen_SrNo where 1=0"
		etsql_con = sqlconobj.dataconn([EXE],company.dbname,etsql_str,[Gen_SrNo_Vw],;
			"nHandle",_Screen.ActiveForm.DataSessionId,.F.)
		If etsql_con < 1 Or !Used("Gen_SrNo_Vw")
			etsql_con = 0
		Else
			Select gen_srno_vw
			Index On itserial Tag itserial
		Endif
	Endif

*	sq1="SELECT NpgNo FROM Gen_Srno WHERE l_yn='"+ALLTRIM(main_vw.l_yn)+"' "+IIF(UPPER(fldnm)=='U_RG23NO'," AND CTYPE='RGAPART2'",IIF(UPPER(fldnm)=='U_RG23CNO'," AND CTYPE='RGCPART2'",IIF(UPPER(fldnm)=='U_PLASR'," AND CTYPE='PLASRNO'","")))+" AND NpgNo='"+ALLTRIM(fldval)+"' AND NOT (TRAN_CD="+STR(main_vw.tran_cd)+" AND ENTRY_TY='"+main_vw.entry_ty+"'"+IIF(fldnm='U_PAGENO'," AND "+tblnm+".ITSERIAL='"+item_vw.itserial+"')",")")+ IIF(coadditional.cate_srno," AND Ccate = ?Main_vw.Cate ","")  &&changes by sandeep Cate column rectified by ccate for bug-18474 ON 20-Aug-13
*Birendra: Bug-19986 on 10/10/2013 :Modified with below one:FORM402SRNO
*!*		sq1="SELECT NpgNo FROM Gen_Srno WHERE l_yn='"+ALLTRIM(main_vw.l_yn)+"' "+IIF(UPPER(fldnm)=='U_RG23NO'," AND CTYPE='RGAPART2'",IIF(UPPER(fldnm)=='U_RG23CNO'," AND CTYPE='RGCPART2'",IIF(UPPER(fldnm)=='U_PLASR'," AND CTYPE='PLASRNO'",IIF(UPPER(fldnm)=='U_402SRNO'," AND CTYPE='FORM402SRNO'",""))))+" AND NpgNo='"+ALLTRIM(fldval)+"' AND NOT (TRAN_CD="+STR(main_vw.tran_cd)+" AND ENTRY_TY='"+main_vw.entry_ty+"'"+IIF(fldnm='U_PAGENO'," AND "+tblnm+".ITSERIAL='"+item_vw.itserial+"')",")")+ IIF(coadditional.cate_srno," AND cate = ?Main_vw.Cate ","")&& commented by Archana K. on 16/12/13 for Bug-21083
&&	sq1="SELECT NpgNo FROM Gen_Srno WHERE l_yn='"+ALLTRIM(main_vw.l_yn)+"' "+IIF(UPPER(fldnm)=='U_RG23NO'," AND CTYPE='RGAPART2'",IIF(UPPER(fldnm)=='U_RG23CNO'," AND CTYPE='RGCPART2'",IIF(UPPER(fldnm)=='U_PLASR'," AND CTYPE='PLASRNO'",IIF(UPPER(fldnm)=='U_402SRNO'," AND CTYPE='FORM402SRNO'",""))))+" AND NpgNo='"+ALLTRIM(fldval)+"' AND NOT (TRAN_CD="+STR(main_vw.tran_cd)+" AND ENTRY_TY='"+main_vw.entry_ty+"'"+IIF(fldnm='U_PAGENO'," AND "+tblnm+".ITSERIAL='"+item_vw.itserial+"')",")")+ IIF(coadditional.cate_srno," AND Ccate = ?Main_vw.Cate ","")&& changed by Archana K. on 16/12/13 for Bug-21083	nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"EXCUR",IIF(mcommit=.F.,"nHandle",nhand),_SCREEN.ACTIVEFORM.DATASESSIONID)	&&280910&&commented by  sandeep for GVAT
	sq1="SELECT NpgNo FROM Gen_Srno WHERE l_yn='"+Alltrim(main_vw.l_yn)+"' "+Iif(Upper(fldnm)=='U_RG23NO'," AND CTYPE='RGAPART2'",Iif(Upper(fldnm)=='U_RG23CNO'," AND CTYPE='RGCPART2'",Iif(Upper(fldnm)=='U_PLASR'," AND CTYPE='PLASRNO'",Iif(Upper(fldnm)=='U_402SRNO'," AND CTYPE='FORM402SRNO'",Iif(Upper(fldnm)=='U_403SRNO'," AND CTYPE='FORM403SRNO'","")))))+" AND NpgNo='"+Alltrim(fldval)+"' AND NOT (TRAN_CD="+Str(main_vw.tran_cd)+" AND ENTRY_TY='"+main_vw.entry_ty+"'"+Iif(fldnm='U_PAGENO'," AND "+tblnm+".ITSERIAL='"+item_vw.itserial+"')",")")+ Iif(coadditional.cate_srno," AND Ccate = ?Main_vw.Cate ","")&& changed by  sandeep for GVAT
	nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"EXCUR",Iif(mcommit=.F.,"nHandle",nhand),_Screen.ActiveForm.DataSessionId)	&&280910

	If nretval<0
		Return .F.
	Endif
	Select excur
	vrcount=Reccount()
	If nretval >0 And Used("EXCUR")
		If	vrcount<=0
			Select gen_srno_vw
			Go Top
			Locate For entry_ty=main_vw.entry_ty And tran_cd=main_vw.tran_cd And (ctype=notype)
			If !Found()
				Append Blank In gen_srno_vw
			Endif

*			IF INLIST(ctype,"RGAPART2","RGCPART2","PLASRNO") OR EMPTY(ctype)
*Birendra: Bug-19986 on 10/10/2013 :Modified with below one:FORM402SRNO
&&			IF INLIST(ctype,"RGAPART2","RGCPART2","PLASRNO","FORM402SRNO") OR EMPTY(ctype)&& commented by  sandeep for bug-21327 on 8-02-2014
			If Inlist(ctype,"RGAPART2","RGCPART2","PLASRNO","FORM402SRNO","FORM403SRNO") Or Empty(ctype)&& changed by  sandeep for bug-21327 on 8-02-2014

				Replace Date With main_vw.u_cldt In gen_srno_vw
*!*					REPLACE ccate WITH main_vw.cate,npgno WITH fldval,entry_ty WITH main_vw.entry_ty, tran_cd WITH main_vw.tran_cd,compid WITH main_vw.compid, ;
*!*						ctype WITH IIF(UPPER(fldnm)=='U_RG23NO','RGAPART2',IIF(UPPER(fldnm)=='U_RG23CNO','RGCPART2',IIF(UPPER(fldnm)=='U_PLASR','PLASRNO',""))),l_yn WITH main_vw.l_yn
*Birendra: Bug-19986 on 10/10/2013 :Modified with below one:FORM402SRNO
				Replace ccate With main_vw.cate,npgno With fldval,entry_ty With main_vw.entry_ty, tran_cd With main_vw.tran_cd,compid With main_vw.compid, ;
					ctype With Iif(Upper(fldnm)=='U_RG23NO','RGAPART2',Iif(Upper(fldnm)=='U_RG23CNO','RGCPART2',Iif(Upper(fldnm)=='U_PLASR','PLASRNO',Iif(Upper(fldnm)=='U_402SRNO','FORM402SRNO',Iif(Upper(fldnm)=='U_403SRNO','FORM403SRNO',""))))),l_yn With main_vw.l_yn && changed by  sandeep for bug-21327 on 8-02-2014
&&					ctype WITH IIF(UPPER(fldnm)=='U_RG23NO','RGAPART2',IIF(UPPER(fldnm)=='U_RG23CNO','RGCPART2',IIF(UPPER(fldnm)=='U_PLASR','PLASRNO',IIF(UPPER(fldnm)=='U_402SRNO','FORM402SRNO',"")))),l_yn WITH main_vw.l_yn && commented by  sandeep for bug-21327 on 8-02-2014
			Endif
			If Betw(mrec,1,Reccount())
				Go mrec
			Endif
		Endif
		Use In excur
	Endif
	=sqlconobj.sqlconnclose("nHandle")
	If !Empty(_malias)
		Select &_malias
	Endif

	If Betw(_mrecno,1,Reccount())
		Go _mrecno
	Endif

	If vrcount>0 And !Isnull(vrcount)
		Return .F.
	Else
		Return .T.
	Endif
Endif

Return
&& Added by Shrikant S. on 29/09/2010 for TKT-4021		--End

Proc chk_bond_ac() &&RUP:-procedure is useful to Search Bond Account Name in Sales entry in DCMAST.
sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)
nhandle=0
sq1="select distinct ac_mast.ac_name from obmain m "
sq2="inner join obacdet ac on (m.tran_cd=ac.tran_cd) inner join ac_mast on (ac_mast.ac_id=m.ac_id)"
sq3="where bond_no='"+Alltrim(main_vw.bond_no)+"'"

nretval = sqlconobj.dataconn([EXE],company.dbname,sq1+sq2+sq3,"_bondac","nHandle",_Screen.ActiveForm.DataSessionId)
If nretval<0
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
Procedure salesman()
sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)
nhandle=0
sq1="select salesman from ac_mast ac where ac.ac_name=?main_vw.party_nm"

nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"ajcur","nHandle",_Screen.ActiveForm.DataSessionId)
If nretval<0
	Return .F.
Endif
Select ajcur
Replace salesman With ajcur.salesman In main_vw
Endproc
&&code added by Ajay Jasiwal on 16/03/2009 ---> end

Procedure chk_fldlen()
Parameters _chkfldnm
If Len(Alltrim(_chkfldnm)) <> Len(_chkfldnm)
	Return .F.
Endif
Return .T.

Procedure chk_time()
Parameters _chkfldnm
If (Val(Substr(_chkfldnm,1,2))=24 And Val(Substr(_chkfldnm,4,2))<>0)
	Return .F.
Endif
If !(Between(Val(Substr(_chkfldnm,1,2)),0,24) And Between(Val(Substr(_chkfldnm,4,2)),0,59)) And !(Between(Val(Substr(_chkfldnm,1,2)),0,24) And Val(Substr(_chkfldnm,4,2))=0)
	Return .F.
Endif
Return .T.


Procedure repleccno			&& used in accountmmaster --> 110909----sachin.s
Lparameters oobject
cvalue = Alltrim(ac_mast_vw.eccno)
llret=.T.
Do While .T.
	nkeycode = Asc(Left(cvalue,1))
	If !Empty(cvalue)
		If !Between(nkeycode,Asc('A'),Asc('Z')) And !Between(nkeycode,Asc('a'),Asc('z')) And                                      !Between(nkeycode,Asc('0'),Asc('9')) And !Inlist(nkeycode,6,9,13,15,32,127,5,4,19,24,7,52,54)
			llret = .F.
			Exit
		Endif
	Else
		Exit
	Endif
	cvalue = Substr(cvalue,2)
Enddo
If !llret
	=Messagebox("Please enter valid character values.",64,vumess)
	Return .F.
Endif
Select ac_mast_vw
Replace eccno With Upper(eccno) In ac_mast_vw	&& Changed By Sachin N. S. on 26/02/2010 for TKT-484
If Empty(ac_mast_vw.cexregno)
	Replace cexregno With eccno In ac_mast_vw
Endif
*!*	  oobject.Parent.Refresh --> Ajay Jaiswal on 9/03/2010
Endproc


Procedure pageno_whn() &&-->Rup  26Sep09
_malias 	= Alias()
Sele item_vw
_mrecno 	= Recno()

retval=.T.
etsql_str = "select tp=[type] from it_mast where it_code="+Str(item_vw.it_code)
_etdatasessionid = _Screen.ActiveForm.DataSessionId
Set DataSession To _etdatasessionid
sqlconobj = Newobject('SqlConnUdObj','SqlConnection',xapps)
etsql_con = sqlconobj.dataconn([EXE],company.dbname,etsql_str,[itype],"nHandle",_etdatasessionid,.F.)
If etsql_con < 1 Or !Used("itype")
	etsql_con = 0
Else
	Select itype
	If Inlist(itype.tp,'Finished','Semi Finished')
		If Type('main_vw.u_gcssr')='L'
			If main_vw.u_gcssr=.F.
				retval=.F.
			Endif
		Else
			retval=.F.
		Endif
	Endif
	If itype.tp='Trading'
		retval=.F.
	Endif
Endif
Return retval
&&<--Rup  26Sep09

&&<--Shrikant 26Sep09
Proc gen_nextno() &&Shrikant: procedure is useful to generate CT-1,CT-3,are1 No.,are2 No.,are3 No. in form UEFRM_ST_exdata1 in Sales Entry.
Parameters fldnm,tblnm,filconfld,filconval
Local vrno
sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)
nhandle=0
*!*		sq1="SELECT MAX(CAST( "+ALLTRIM(fldnm)+"  AS INT)) AS RNO  FROM "+ALLTRIM(tblnm)+" WHERE ISNUMERIC( "+ALLTRIM(fldnm)+" )=1 and l_yn='"+ALLTRIM(main_vw.l_yn)+"' " +" and "+ALLTRIM(filconfld)+ " = '"+ALLTRIM(filconval)+"'"		&& Commented by Shrikant S. on 29/09/2010 for TKT-4021
sq1="SELECT MAX(CAST( "+Alltrim(fldnm)+"  AS INT)) AS RNO  FROM "+Alltrim(tblnm)+" WHERE ISNUMERIC( "+Alltrim(fldnm)+" )=1 and l_yn='"+Alltrim(main_vw.l_yn)+"' "+Iif(!Empty(Alltrim(filconfld))," and "+Alltrim(filconfld)+ " = '"+Alltrim(filconval)+"'","")	&& Added by Shrikant S. on 29/09/2010 for TKT-4021
nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"EXCUR","nHandle",_Screen.ActiveForm.DataSessionId)

If nretval<0
	Return .F.
Endif
Select excur
vrno=Alltrim(Str(Iif(Isnull(excur.rno),1,(excur.rno)+1)))
If Used("EXCUR")
	Use In excur
Endif
Return vrno
&&<--Shrikant 26Sep09


Procedure mfgexpdtchk		&& Procedure to Check the Manufacturing Date with Expiry Date
Lparameters oobject

Select item_vw
*!*	If !Empty(item_vw.MFGDT) And !Empty(item_vw.EXPDT)
*!*		If item_vw.MFGDT > item_vw.EXPDT
*!*	*!*			Return .F.
*!*		Endif
*!*	Endif

&&Added by Shrikant S. on 09 Mar, 2010 ---- Start
If Upper(Alltrim(Strextract(oobject.ucontrolsource,'.'))) = 'EXPDT'
	If !Empty(item_vw.mfgdt) And !Empty(oobject.Value)
		If item_vw.mfgdt > oobject.Value
			Return .F.
		Endif
	Endif
Else
	If !Empty(oobject.Value) And !Empty(item_vw.expdt)
		If oobject.Value > item_vw.expdt
			Return .F.
		Endif
	Endif
Endif
&&Added by Shrikant S. on 09 Mar, 2010 ---- End

If !Empty(item_vw.mfgdt) And Empty(item_vw.expdt) And Upper(Alltrim(Strextract(oobject.ControlSource,'.'))) = 'EXPDT'
	Return .F.
Endif

*!*	oObject.Parent.Refresh  --> Ajay Jaiswal on 9/03/2010
Endproc

Procedure check_bondperiod()
Lparameters oobject

Select main_vw
If Upper(Alltrim(Strextract(oobject.ucontrolsource,'.'))) = 'EXBVLDT'
*!*		If !Empty(main_vw.u_pinvdt) And !Empty(oobject.Value)		&& Commented by Shrikant S. on 25/01/2017 for GST
*!*			If main_vw.u_pinvdt > oobject.Value						&& Commented by Shrikant S. on 25/01/2017 for GST
	If !Empty(main_vw.pinvdt) And !Empty(oobject.Value)			&& Added by Shrikant S. on 25/01/2017 for GST
		If main_vw.pinvdt > oobject.Value							&& Added by Shrikant S. on 25/01/2017 for GST
			Return .F.
		Endif
	Endif
Else
	If !Empty(oobject.Value) And !Empty(main_vw.exbvldt)
		If oobject.Value > main_vw.exbvldt
			Return .F.
		Endif
	Endif
Endif

*!*	If !Empty(main_vw.u_pinvdt) And Empty(main_vw.exbvldt) And Upper(Alltrim(Strextract(oobject.ControlSource,'.'))) = 'EXBVLDT'		&& Commented by Shrikant S. on 25/01/2017 for GST
If !Empty(main_vw.pinvdt) And Empty(main_vw.exbvldt) And Upper(Alltrim(Strextract(oobject.ControlSource,'.'))) = 'EXBVLDT'			&& Added by Shrikant S. on 25/01/2017 for GST
	Return .F.
Endif
*!*	oobject.Parent.Refresh
Endproc


&&PROCEDURE chk_vendtype &&COMMENTED BY SATISH PAL BUG-8064 DATED 31/12/2012
Procedure chk_vendtype() &&ADDED BY SATISH PAL BUG-8064 DATED 31/12/2012
Lparameters oobject1 &&ADDED BY SATISH PAL BUG-8064 DATED 31/12/2012
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

*!*	Select ac_group_name From panchkcur Where ac_group_name = 'SUNDRY CREDITORS' Into Cursor cur1		&& Commented By Shrikant S. on 07/12/2012 for Bug-7404
&&added and commented BY SATISH PAL BUG-8064 DATED 31/12/2012-Start
&&Select ac_group_name From panchkcur Where ac_group_name = macgroup  Into Cursor cur1			&& Added By Shrikant S. on 07/12/2012 for Bug-7404
If !Empty(oobject1)
	macgroup = "'"+Strtran(oobject1, ',', "','")+"'"
	Select ac_group_name From panchkcur Where Inlist(ac_group_name, &macgroup)  Into Cursor cur1
Else
	Select ac_group_name From panchkcur Into Cursor cur1
Endif
&&added and commented by SATISH PAL BUG-8064 DATED 31/12/2012-End
If _Tally > 0
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
	Endif
&& Added By Shrikant S. on 07/12/2012 for Bug-7404		&& End
	Return .T.
Else
	Return .F.
Endif

Endproc

&& Added By Shrikant S. on 07/12/2012 for Bug-7404		&& Start
&&Procedure chk_Shiptovendtype &&commented BY SATISH PAL BUG-8064 DATED 31/12/2012
Procedure chk_Shiptovendtype () &&ADDED BY SATISH PAL BUG-8064 DATED 31/12/2012
Lparameters oobject2 &&ADDED BY SATISH PAL BUG-8064 DATED 31/12/2012
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
&&added and commented BY SATISH PAL BUG-8064 DATED 31/12/2012-Start
&&SELECT ac_group_name FROM panchkcur WHERE ac_group_name = macgroup  INTO CURSOR cur1
If !Empty(oobject2)
	macgroup = "'"+Strtran(oobject2, ',', "','")+"'"
	Select ac_group_name From panchkcur Where Inlist(ac_group_name, &macgroup)  Into Cursor cur1
Else
	Select ac_group_name From panchkcur Into Cursor cur1
Endif
&&added and commented by SATISH PAL BUG-8064 DATED 31/12/2012-End
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
	Endif
	Return .T.
Else
	Return .F.
Endif

Endproc
&& Added By Shrikant S. on 07/12/2012 for Bug-7404		&& End

&&Rup 14/11/2009-->
Procedure chk_empty_pan() &&Rup 14/11/2009
_etdatasessionid = _Screen.ActiveForm.DataSessionId
Set DataSession To _etdatasessionid
vpanempty=.F.
*sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)
nhandle=0
sq1="select i_tax from ac_mast where ac_id=?main_vw.ac_id"
sqlconobj = Newobject('SqlConnUdObj','SqlConnection',xapps)
nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"panchkcur","nHandle",_etdatasessionid )
If nretval<0
	Return .F.
Endif
If Used('panchkcur')
	Select panchkcur
	If !Empty(panchkcur.i_tax)
		If Inlist(Alltrim(panchkcur.i_tax),'PANAPPLIED','PANNOTAVBL','PANINVALID')
			vpanempty=.T.
		Endif
	Else
		vpanempty=.T.
	Endif
	Use In panchkcur
Endif
Return vpanempty

&&<--Rup 14/11/2009

**&& Added For Expenses Purchase On 21/12/2009 by Hetal L Patel Start
Procedure calcvat()
Replace u_vatonamt With Round(((item_vw.qty*item_vw.rate)*item_vw.u_vatonp)/100,2) In item_vw
**&& Added For Expenses Purchase On 21/12/2009 by Hetal L Patel End


**start OPST_CHK_AVGPACK() TKT 479 and TKT 495
**&& Added On 22/02/2010 by Hetal L Patel St
Proc opst_chk_avgpack() &&Check reverse Calculation
malias = Alias()
sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)
nhandle=0

If Val(item_vw.u_pkno)#0
	_mqty = 0
	If Val(item_vw.u_pkno)#0 And Val(item_vw.u_appack)#0
		_mqty = (Val(item_vw.u_pkno) * Val(item_vw.u_appack))
	Endif
	If _mqty # 0
		If  (([vuexc] $ vchkprod) Or ([vuinv] $ vchkprod)) And Type('main_vw.entry_ty')='C' &&Check Existing Records
			Do Case
*!*					Case main_vw.entry_ty='OP'	&& Changed By Sachin N. S. on 01/02/2011 for TKT-5729
			Case main_vw.entry_ty='OP' And Used('Projectitref_vw')
				nretval = 0
				nhandle = 0
				etsql_con  = 0
				nhandle    = 0
				Select aentry_ty,atran_cd,aitserial,qty From projectitref_vw Where entry_ty=main_vw.entry_ty And tran_cd=main_vw.tran_cd And itserial=item_vw.itserial Into Cursor tibl
				etsql_str  = ""
				etsql_str = "USP_ENT_CHK_OP_ALLOCATION '"+item_vw.entry_ty+"',"+Alltrim(Str(item_vw.tran_cd))+","+Alltrim(Str(item_vw.it_code))+",'"+item_vw.itserial+"','";
					+tibl.aentry_ty+"',"+Alltrim(Str(tibl.atran_cd))+",'"+tibl.aitserial+"'"
				etsql_con = sqlconobj.dataconn([EXE],company.dbname,etsql_str,[tibl_1],"nHandle",_Screen.ActiveForm.DataSessionId)
				If etsql_con >0
					If Used('tibl_1')
						If ((_mqty >tibl_1.wipqty) And tibl_1.wipqty<>0)
							Replace u_appack With '' In item_vw
							=Messagebox('Quantity could not be greater then '+Alltrim(Str(tibl_1.wipqty,14,3)),0+64,vumess)
							Select (malias)
							Return .F.
						Endif
						Use In tibl_1
					Endif
				Else
					Select (malias)
					Return .F.
				Endif
				Use In tibl
			Case main_vw.entry_ty='ST'
				If Used('_DetailData')	&& Added By Sachin N. S. on 28/05/2010 for TKT-2055
					Store 0 To _balqty
					Store item_vw.entry_ty To centryty
					Store item_vw.tran_cd To ctrancd
					Store item_vw.it_code To citcode
					Store item_vw.itserial To citserial
*!* Pending Quantity
					Select a.balqtynew balqty From _detaildata a ;
						inner Join itref_vw b On(a.entry_ty = b.rentry_ty And a.itserial = b.ritserial And a.it_code = b.it_code And a.l_yn = b.rl_yn And a.inv_no = b.rinv_no And a.inv_sr = b.rinv_sr);
						WHERE b.entry_ty = centryty And b.tran_cd = ctrancd And b.it_code = citcode And b.itserial = citserial ;
						INTO Cursor pendqty
*!*
					Select pendqty
					Store balqty To _balqty
					If _mqty >_balqty And _balqty #0
						Replace u_appack With '' In item_vw
						=Messagebox('Quantity could not be greater then '+Alltrim(Str(_balqty,14,3)),0+64,vumess)
						Select (malias)
						Return .F.
					Endif
				Endif
			Endcase
		Endif
		Replace qty With _mqty In item_vw
	Endif
Endif
***Added By Hetal Dt 18/02/2010 Check IP QTY &&Ed
***Added By SATISH PAL Dt 05/08/2013 FOR BUG-18284-STATRT
If !Empty(malias)
	Sele(malias)
Endif
**	SELE(malias)
***Added By SATISH PAL Dt 05/08/2013 FOR BUG-18284-END
Return .T.
**&& Added On 22/02/2010 by Hetal L Patel Ed
***End OPST_CHK_AVGPACK()

&&Changes has been done by vasant on 25/02/2012 as per Bug-6092 (Required "Service Tax REVERSE CHARGE MECHANISM" in our Default Products.)
*!*	&&--->TKT-941 &&Vasant250410
*!*	PROCEDURE calculateservicetax
*!*	m_serviceamount = 0
*!*	m_etvalidalias = ALIAS()
*!*	IF USED('AcdetAlloc_vw')
*!*		SELECT acdetalloc_vw
*!*		SUM(staxable) TO m_serviceamount FOR serty <> 'OTHERS' AND !EMPTY(serty)
*!*	ENDIF
*!*	IF !EMPTY(m_etvalidalias)
*!*		SELECT (m_etvalidalias)
*!*	ENDIF
*!*	RETURN m_serviceamount

*!*	PROCEDURE findserviceavailtype
*!*	_actfrm = _SCREEN.ACTIVEFORM
*!*	*!*	m_ServiceAvailType = 'EXCISE'
*!*	m_serviceavailtype = 'SERVICES'		&& Changed By Sachin N. S. on 28/07/2010 for TKT-3279

*!*	m_etvalidalias = ALIAS()
*!*	IF USED('AcdetAlloc_vw') AND USED('CalcTax_vw')
*!*		m_servicename = ''
*!*		m_servicename = SUBSTR(calctax_vw.addpostcnd,AT(_actfrm.splitcharacter,calctax_vw.addpostcnd)+LEN(_actfrm.splitcharacter)+2)
*!*		m_servicename = PADL(m_servicename,LEN(acdetalloc_vw.serty),' ')
*!*		SELECT acdetalloc_vw
*!*		IF SEEK(m_servicename,'AcdetAlloc_vw','Serty')
*!*			m_serviceavailtype = seravail
*!*		ENDIF
*!*	ELSE
*!*		IF TYPE('_mseravail') = 'C'
*!*			m_serviceavailtype = _mseravail
*!*		ENDIF
*!*	ENDIF
*!*	IF !EMPTY(m_etvalidalias)
*!*		SELECT (m_etvalidalias)
*!*	ENDIF
*!*	RETURN m_serviceavailtype
&&Changes has been done by vasant on 25/02/2012 as per Bug-6092 (Required "Service Tax REVERSE CHARGE MECHANISM" in our Default Products.)

Procedure findserviceruletype
_actfrm = _Screen.ActiveForm
m_serviceavailtype = ''
m_etvalidalias = Alias()
If Type('Main_vw.SerRule') = 'C'
	m_serviceavailtype = main_vw.serrule
Endif
If Type('_mseravail') = 'C'
	m_serviceavailtype =  _mseravail
Endif
If !Empty(m_etvalidalias)
	Select (m_etvalidalias)
Endif
Return m_serviceavailtype
&&<---TKT-941 &&Vasant250410ww

&&changes done as per TKT-3954
Procedure get_exrateunit
_curform = _Screen.ActiveForm
_etdatasessionid = _curform.DataSessionId
Set DataSession To _etdatasessionid
_totrec = 0
If Type('_curform.sqlconobj') = 'O'
	sq1="Select Top 1 RateUnit from Cetsh where Cetsh = ?it_mast_vw.Chapno "
	nretval = _curform.sqlconobj.dataconn([EXE],company.dbname,sq1,"_tmprateunit","_curform.nHandle",_etdatasessionid)
	If nretval > 0 And Used('_tmprateunit')
		Replace exrateunit With _tmprateunit.rateunit In it_mast_vw
		_totrec = 1
	Endif
	If Used('_tmprateunit')
		Use In _tmprateunit
	Endif
Else
	_totrec = 1
Endif
If _totrec > 0
	Return .T.
Else
	Return .F.
Endif

Procedure chk_exrateunit
_curform = _Screen.ActiveForm
_etdatasessionid = _curform.DataSessionId
Set DataSession To _etdatasessionid
_totrec = 0
If Type('_curform.sqlconobj') = 'O'
	sq1="Select Top 1 RateUnit from Cetsh where Rateunit = ?it_mast_vw.exrateunit"
	nretval = _curform.sqlconobj.dataconn([EXE],company.dbname,sq1,"_tmprateunit","_curform.nHandle",_etdatasessionid)
	If nretval > 0 And Used('_tmprateunit')
		_totrec = Reccount('_tmprateunit')
	Endif
	If Used('_tmprateunit')
		Use In _tmprateunit
	Endif
Else
	_totrec = 1
Endif
If _totrec > 0
	Return .T.
Else
	Return .F.
Endif

Procedure chk_chapno
Parameters oobject							&& Added by Shrikant S. on 04/10/2010 for TKT-4242
_curform = _Screen.ActiveForm
_etdatasessionid = _curform.DataSessionId
Set DataSession To _etdatasessionid
_totrec = 0
If Type('_curform.sqlconobj') = 'O'
&&Done by Vasant on 21/01/11 Refer Note A
	_mvalue   = oobject.Value
	_mcsource = oobject.ControlSource
&&Done by Vasant on 21/01/11 Refer Note A
	sq1="Select Top 1 RateUnit from Cetsh where Cetsh = ?_mValue"	&&Done by Vasant on 21/01/11 Refer Note A
	nretval = _curform.sqlconobj.dataconn([EXE],company.dbname,sq1,"_tmpchapno","_curform.nHandle",_etdatasessionid)
	If nretval > 0 And Used('_tmpchapno')
		_totrec = Reccount('_tmpchapno')
		If _totrec > 0
&&Done by Vasant on 21/01/11 Refer Note A
			_mexrateunitfld =  Alltrim(Juststem(_mcsource))+'.exrateunit'
			If Type(_mexrateunitfld) = 'C'
				Replace exrateunit With _tmpchapno.rateunit In (Alltrim(Juststem(_mcsource)))
				oobject.Parent.Refresh
			Endif
&&Done by Vasant on 21/01/11
		Endif
	Endif
	If Used('_tmpchapno')
		Use In _tmpchapno
	Endif
Else
	_totrec = 1
Endif
If _totrec > 0
	Return .T.
Else
	Return .F.
Endif
*-*
*Note A
&&Earlier this validation was only for Item Master Chap. No., Now also done for Item Group Master
*-*
&&changes done as per TKT-3954

******* Added By Sachin N. S. for New Installer from NXIO Zip
Proc val_serapl()&&RUP:-procedure is useful to generate part-i-->pageno field.
If Upper(ac_mast_vw.serapl)='Y'
	Do Form uefrm_ac_sertax With _Screen.ActiveForm.DataSessionId,_Screen.ActiveForm.addmode,_Screen.ActiveForm.editmode
Endif
Return .T.


******* Added By Sachin N. S. for New Installer from NXIO Zip
Procedure pvendor_type
Lparameters ngrpid,nrettyp

Local cretgrp,csql,cansi
cansi=Set('ansi')
cretgrp = Iif(nrettyp=1,.F.,"")
sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)
Do While .T.
	If !Empty(ngrpid)
		nhandle=0
		csql =  "SELECT ac_group_name,gAc_id,ac_group_id FROM ac_group_mast WHERE Ac_Group_Id = "+Str(ngrpid)
		nretval = sqlconobj.dataconn([EXE],company.dbname,csql,"EXCUR","nHandle",_Screen.ActiveForm.DataSessionId)
		If nretval<=0
			Exit
		Endif
		Select excur
		If Inlist(Upper(Alltrim(excur.ac_group_name)),"SUNDRY CREDITORS")
			cretgrp = Iif(nrettyp=1,.T.,"Importer")
			Exit
		Endif
		ngrpid = excur.gac_id
		If ngrpid = 1 Or ngrpid = excur.ac_group_id
			Exit
		Endif
	Else
		Exit
	Endif
Enddo
If !Empty(cansi)
	Set Ansi &cansi
Endif

Return cretgrp
Endproc

******* Added By Sachin N. S. for New Installer from NXIO Zip
**Added by Shrikant S on 11/02/2010 12.58am
Procedure repl_eccno
If !Empty(ac_mast_vw.eccno)
	Replace exregno With eccno In ac_mast_vw
Endif
Endproc

&& Added by Shrikant S. on 13/04/2011 for TKT-7206	--Start
Procedure calc_brokerage
Parameters 	oobject
If (item_vw.brokperc >0)
	Replace brokamt With (item_vw.qty * item_vw.rate) * item_vw.brokperc/100 In item_vw
	oobject.Parent.Refresh
Endif
Endproc

Procedure calc_commamt
Parameters 	oobject
If (item_vw.commpermt >0)
	Replace commamt With (item_vw.qty * item_vw.commpermt) In item_vw
	oobject.Parent.Refresh
Endif
Endproc
&& Added by Shrikant S. on 13/04/2011 for TKT-7206	--End

&&Changes has been done by vasant on 25/02/2012 as per Bug-6092 (Required "Service Tax REVERSE CHARGE MECHANISM" in our Default Products.)
*!*	PROCEDURE calc_servicetax_s1() &&Rup TKT-7412,TKT-8319 26/05/2011
*!*	_sertaxableamt = 0
*!*	SELECT acdetalloc_vw
*!*	IF SEEK(item_vw.itserial,'acdetalloc_vw','itserial')  AND UPPER(main_vw.serrule)<>'EXEMPT'
*!*		_sertaxableamt = acdetalloc_vw.staxable
*!*	ENDIF
*!*	*!*	SCAN FOR itserial==item_vw.itserial AND UPPER(main_vw.SerRule)<>'EXEMPT'
*!*	*!*		_SerTaxableAmt = acdetalloc_vw.staxable
*!*	*!*	Endscan
*!*	RETURN _sertaxableamt


*!*	PROCEDURE sertaxaceffect_s1() &&Rup TKT-7412,TKT-8319 26/05/2011
*!*	PARAMETERS _fldnm
*!*	_vacnm = ''
*!*	_isadv = .F.

*!*	IF TYPE('_mseravail') = 'C'
*!*		IF INLIST(_mseravail,"EXCISE","SERVICE")
*!*	*	IF _mseravail= 'ADJUSTMENT'
*!*			_isadv = .T.
*!*		ENDIF
*!*	ENDIF


*!*	SELECT acdetalloc_vw
*!*	IF SEEK(item_vw.itserial,'acdetalloc_vw','itserial')
*!*		DO CASE
*!*			CASE UPPER(_fldnm)="SERBAMT" AND _isadv = .T.
*!*				_vacnm="Service Tax Adjustment A/C"
*!*			CASE UPPER(_fldnm)="SERBAMT" AND _isadv = .F.
*!*				_vacnm="Service Tax Payable"
*!*			CASE UPPER(_fldnm)="SERCAMT" AND _isadv = .T.
*!*				_vacnm="Edu. Cess on Service Tax Adjustment A/C"
*!*			CASE UPPER(_fldnm)="SERCAMT" AND _isadv = .F.
*!*				_vacnm="Edu. Cess on Service Tax Payable"
*!*			CASE UPPER(_fldnm)="SERHAMT" AND _isadv = .T.
*!*				_vacnm="S & H Cess on Service Tax Adjustment A/C"
*!*			CASE UPPER(_fldnm)="SERHAMT" AND _isadv = .F.
*!*				_vacnm="S & H Cess on Service Tax Payable"
*!*		ENDCASE
*!*	ENDIF
*!*	RETURN _vacnm


*!*	PROCEDURE sertaxaceffect_s1_cac() &&Rup TKT-7412,TKT-8319 26/05/2011
*!*	PARAMETERS _fldnm
*!*	_vacnm = ''
*!*	_isadv = .F.
*!*	IF TYPE('_mseravail') = 'C'
*!*	*!*		IF _mseravail= 'ADJUSTMENT'
*!*		IF INLIST(_mseravail,"EXCISE","SERVICE")
*!*			_isadv = .T.
*!*		ENDI
*!*	ENDIF

*!*	SELECT acdetalloc_vw
*!*	IF SEEK(item_vw.itserial,'acdetalloc_vw','itserial')
*!*		DO CASE
*!*			CASE UPPER(_fldnm)="SERBAMT" AND _isadv = .T.
*!*				_vacnm="Service Tax Payable"
*!*			CASE UPPER(_fldnm)="SERCAMT" AND _isadv = .T.
*!*				_vacnm="Edu. Cess on Service Tax Payable"
*!*			CASE UPPER(_fldnm)="SERHAMT" AND _isadv = .T.
*!*				_vacnm="S & H Cess on Service Tax Payable"
*!*		ENDCASE
*!*	ENDIF
*!*	RETURN _vacnm


*!*	PROCEDURE sertaxaceffect_e1() &&Rup TKT-7412,TKT-8319 26/05/2011
*!*	PARAMETERS _fldnm
*!*	_fldnm = UPPER(_fldnm)
*!*	_vacnm = ''
*!*	_isadv = .F.
*!*	IF TYPE('_mseravail') = 'C'
*!*		IF INLIST(_mseravail,"EXCISE","SERVICE")
*!*			_isadv = .T.
*!*		ENDI
*!*	ENDIF
*!*	SELECT acdetalloc_vw

*!*	IF SEEK(item_vw.itserial,'acdetalloc_vw','itserial')
*!*		DO CASE
*!*			CASE _isadv = .T. AND main_vw.serrule='IMPORT'
*!*				IF _fldnm="SERBAMT"
*!*					_vacnm="Input Service Tax Adjustment A/C"
*!*				ENDIF
*!*				IF  _fldnm="SERCAMT"
*!*					_vacnm="Input Edu. Cess on Service Tax Adjustment A/C"
*!*				ENDIF
*!*				IF 	_fldnm="SERHAMT"
*!*					_vacnm="Input S & H Cess on Service Tax Adjustment A/C"
*!*				ENDIF
*!*			CASE _isadv = .F. AND main_vw.serrule='IMPORT'
*!*				IF _fldnm="SERBAMT"
*!*					_vacnm="Input Service Tax Adjustment A/C"
*!*				ENDIF
*!*				IF  _fldnm="SERCAMT"
*!*					_vacnm="Input Edu. Cess on Service Tax Adjustment A/C"
*!*				ENDIF
*!*				IF 	_fldnm="SERHAMT"
*!*					_vacnm="Input S & H Cess on Service Tax Adjustment A/C"
*!*				ENDIF
*!*			CASE _isadv = .T. AND main_vw.serrule!='IMPORT'

*!*				IF _fldnm="SERBAMT"
*!*					_vacnm="Input Service Tax Adjustment A/C"
*!*				ENDIF
*!*				IF  _fldnm="SERCAMT"
*!*					_vacnm="Input Edu. Cess on Service Tax Adjustment A/C"
*!*				ENDIF
*!*				IF 	_fldnm="SERHAMT"
*!*					_vacnm="Input S & H Cess on Service Tax Adjustment A/C"
*!*				ENDIF

*!*			CASE _isadv = .F. AND main_vw.serrule!='IMPORT'
*!*				IF acdetalloc_vw.seravail="SERVICE"
*!*					IF _fldnm="SERBAMT"
*!*						_vacnm="Service Tax Available"
*!*					ENDIF
*!*					IF _fldnm="SERCAMT"
*!*						_vacnm="Edu. Cess on Service Tax Available"
*!*					ENDIF
*!*					IF _fldnm="SERHAMT"
*!*						_vacnm="S & H Cess on Service Tax Available"
*!*					ENDIF
*!*				ENDIF
*!*				IF acdetalloc_vw.seravail="EXCISE"
*!*					IF _fldnm="SERBAMT"
*!*						_vacnm="BALANCE WITH SERVICE TAX A/C"
*!*					ENDIF
*!*					IF _fldnm="SERCAMT"
*!*						_vacnm="BALANCE WITH SERVICE TAX CESS A/C"
*!*					ENDIF
*!*					IF _fldnm="SERHAMT"
*!*						_vacnm="BALANCE WITH SERVICE TAX HCESS A/C"
*!*					ENDIF
*!*				ENDIF
*!*		ENDCASE
*!*	ENDIF
*!*	*=MESSAGEBOX("SerTaxAcEffect_E1 "+_vacnm)
*!*	RETURN _vacnm


*!*	PROCEDURE sertaxaceffect_e1_cac() &&Rup TKT-7412,TKT-8319 26/05/2011
*!*	PARAMETERS _fldnm
*!*	SET STEP ON
*!*	_fldnm = UPPER(_fldnm)
*!*	_vacnm = ''
*!*	_isadv = .F.
*!*	IF TYPE('_mseravail') = 'C'
*!*		IF INLIST(_mseravail,"EXCISE","SERVICE")
*!*			_isadv = .T.
*!*		ENDI
*!*	ENDIF
*!*	*!*	IF TYPE('_mseravail') = 'C'
*!*	*!*		IF _mseravail= 'ADJUSTMENT'
*!*	*!*			_isadv = .t.
*!*	*!*		endi
*!*	*!*	ENDIF
*!*	SELECT acdetalloc_vw

*!*	IF SEEK(item_vw.itserial,'acdetalloc_vw','itserial')
*!*		DO CASE
*!*			CASE _isadv = .T.
*!*				IF UPPER(_fldnm)="SERBAMT"
*!*					_vacnm="Input Service Tax Adjustment A/C"
*!*				ENDIF
*!*				IF UPPER(_fldnm)="SERCAMT"
*!*					_vacnm="Input Edu. Cess on Service Tax Adjustment A/C"
*!*				ENDIF
*!*				IF UPPER(_fldnm)="SERHAMT"
*!*					_vacnm="Input S & H Cess on Service Tax Adjustment A/C"
*!*				ENDIF

*!*			CASE _isadv = .F.
*!*				IF main_vw.serrule<>'IMPORT'
*!*					IF UPPER(_fldnm)="SERBAMT"
*!*						_vacnm=""
*!*					ENDIF
*!*					IF UPPER(_fldnm)="SERCAMT"
*!*						_vacnm=""
*!*					ENDIF
*!*					IF UPPER(_fldnm)="SERHAMT"
*!*						_vacnm=""
*!*					ENDIF
*!*				ENDIF
*!*				IF main_vw.serrule='IMPORT'
*!*					IF UPPER(_fldnm)="SERBAMT"
*!*						_vacnm="Service Tax Payable"
*!*					ENDIF
*!*					IF UPPER(_fldnm)="SERCAMT"
*!*						_vacnm="Edu. Cess on Service Tax Payable"
*!*					ENDIF
*!*					IF UPPER(_fldnm)="SERHAMT"
*!*						_vacnm="S & H Cess on Service Tax Payable"
*!*					ENDIF
*!*				ENDIF
*!*		ENDCASE
*!*		IF _isadv = .T.
*!*			IF _mseravail<>"EXCISE"
*!*				IF _fldnm="SERBAMT"
*!*					_vacnm="Service Tax Available"
*!*				ENDIF
*!*				IF _fldnm="SERCAMT"
*!*					_vacnm="Edu. Cess on Service Tax Available"
*!*				ENDIF
*!*				IF _fldnm="SERHAMT"
*!*					_vacnm="S & H Cess on Service Tax Available"
*!*				ENDIF
*!*			ELSE
*!*				IF UPPER(_fldnm)='SERBAMT'
*!*					_vacnm = "BALANCE WITH SERVICE TAX A/C"
*!*				ENDIF
*!*				IF UPPER(_fldnm)='SERCAMT'
*!*					_vacnm = "BALANCE WITH SERVICE TAX CESS A/C"
*!*				ENDIF
*!*				IF UPPER(_fldnm)='SERHAMT'
*!*					_vacnm = "BALANCE WITH SERVICE TAX HCESS A/C"
*!*				ENDIF
*!*			ENDIF
*!*		ENDIF
*!*	ENDIF
*!*	RETURN _vacnm



*!*	PROCEDURE sertaxaceffect_bp() &&Rup TKT-7412,TKT-8319 26/05/2011
*!*	PARAMETERS _fldnm
*!*	_vacnm = ''
*!*	DO CASE
*!*		CASE findallocservicetype() <> "IMPORT" AND main_vw.tdspaytype=1
*!*			_vacnm = ''
*!*		CASE findallocservicetype()='IMPORT' AND main_vw.tdspaytype=1
*!*			IF findserviceavailtype()="EXCISE"
*!*				IF UPPER(_fldnm)='SERBAMT'
*!*					_vacnm = "BALANCE WITH SERVICE TAX A/C"
*!*				ENDIF
*!*				IF UPPER(_fldnm)='SERCAMT'
*!*					_vacnm = "BALANCE WITH SERVICE TAX CESS A/C"
*!*				ENDIF
*!*				IF UPPER(_fldnm)='SERHAMT'
*!*					_vacnm = "BALANCE WITH SERVICE TAX HCESS A/C"
*!*				ENDIF
*!*			ELSE
*!*				IF UPPER(_fldnm)='SERBAMT'
*!*					_vacnm = "SERVICE TAX AVAILABLE"
*!*				ENDIF
*!*				IF UPPER(_fldnm)='SERCAMT'
*!*					_vacnm = "Edu. Cess on Service Tax Available"
*!*				ENDIF
*!*				IF UPPER(_fldnm)='SERHAMT'
*!*					_vacnm = "S & H Cess on Service Tax Available"
*!*				ENDIF

*!*			ENDIF
*!*		CASE findserviceavailtype()="EXCISE" AND main_vw.tdspaytype=2
*!*			IF UPPER(_fldnm)='SERBAMT'
*!*				_vacnm = "BALANCE WITH SERVICE TAX A/C"
*!*			ENDIF
*!*			IF UPPER(_fldnm)='SERCAMT'
*!*				_vacnm = "BALANCE WITH SERVICE TAX CESS A/C"
*!*			ENDIF
*!*			IF UPPER(_fldnm)='SERHAMT'
*!*				_vacnm = "BALANCE WITH SERVICE TAX HCESS A/C"
*!*			ENDIF
*!*		CASE findserviceavailtype()<>"EXCISE" AND main_vw.tdspaytype=2
*!*			IF UPPER(_fldnm)='SERBAMT'
*!*				_vacnm = "SERVICE TAX AVAILABLE"
*!*			ENDIF
*!*			IF UPPER(_fldnm)='SERCAMT'
*!*				_vacnm = "Edu. Cess on Service Tax Available"
*!*			ENDIF
*!*			IF UPPER(_fldnm)='SERHAMT'
*!*				_vacnm = "S & H Cess on Service Tax Available"
*!*			ENDIF
*!*	ENDCASE
*!*	RETURN _vacnm



*!*	PROCEDURE sertaxaceffect_bp_cac() &&Rup TKT-7412,TKT-8319 26/05/2011
*!*	PARAMETERS _fldnm
*!*	_vacnm = ''
*!*	DO CASE
*!*		CASE  main_vw.tdspaytype=1
*!*			IF findallocservicetype() = "IMPORT"
*!*				IF !EVAL(coadditional.seraccdt)
*!*					IF UPPER(_fldnm)='SERBAMT'
*!*						_vacnm = "Input Service Tax"
*!*					ENDIF
*!*					IF UPPER(_fldnm)='SERCAMT'
*!*						_vacnm = "Edu. Cess on Input Service Tax"
*!*					ENDIF
*!*					IF UPPER(_fldnm)='SERHAMT'
*!*						_vacnm = "S & H Cess on Input Service Tax"
*!*					ENDIF
*!*				ELSE
*!*					IF UPPER(_fldnm)='SERBAMT'
*!*						_vacnm = "Input Service Tax Adjustment A/C"
*!*					ENDIF
*!*					IF UPPER(_fldnm)='SERCAMT'
*!*						_vacnm = "Input Edu. Cess on Service Tax Adjustment A/C"
*!*					ENDIF
*!*					IF UPPER(_fldnm)='SERHAMT'
*!*						_vacnm = "Input S & H Cess on Service Tax Adjustment A/C"
*!*					ENDIF
*!*				ENDIF
*!*			ELSE
*!*				_vacnm = ''
*!*			ENDIF
*!*	*eval(CoAdditional.SerAccDt) AND
*!*	*_vacnm = ''

*!*		CASE !EVAL(coadditional.seraccdt)
*!*			IF UPPER(_fldnm)='SERBAMT'
*!*				_vacnm = "Input Service Tax"
*!*			ENDIF
*!*			IF UPPER(_fldnm)='SERCAMT'
*!*				_vacnm = "Edu. Cess on Input Service Tax"
*!*			ENDIF
*!*			IF UPPER(_fldnm)='SERHAMT'
*!*				_vacnm = "S & H Cess on Input Service Tax"
*!*			ENDIF
*!*		CASE EVAL(coadditional.seraccdt)
*!*			IF UPPER(_fldnm)='SERBAMT'
*!*				_vacnm = "Input Service Tax Adjustment A/C"
*!*			ENDIF
*!*			IF UPPER(_fldnm)='SERCAMT'
*!*				_vacnm = "Input Edu. Cess on Service Tax Adjustment A/C"
*!*			ENDIF
*!*			IF UPPER(_fldnm)='SERHAMT'
*!*				_vacnm = "Input S & H Cess on Service Tax Adjustment A/C"
*!*			ENDIF

*!*	*!*	CASE eval(CoAdditional.SerAccDt) AND UPPER(_fldnm)='SERBAMT' AND main_vw.tdspaytype=2
*!*	*!*		_vacnm = "Input Service Tax Adjustment A/C"
*!*	*!*	CASE eval(CoAdditional.SerAccDt) AND UPPER(_fldnm)='SERCAMT' AND main_vw.tdspaytype=2
*!*	*!*		_vacnm = "Input Edu. Cess on Service Tax Adjustment A/C"
*!*	*!*	CASE eval(CoAdditional.SerAccDt) AND UPPER(_fldnm)='SERHAMT'
*!*	*!*		_vacnm = "S & H Cess on Input Service Tax"

*!*	ENDCASE
*!*	RETURN _vacnm


*!*	PROCEDURE sertaxaceffect_br() &&Rup TKT-7412,TKT-8319 26/05/2011
*!*	PARAMETERS _fldnm
*!*	_vacnm = ''
*!*	DO CASE
*!*		CASE EVAL(coadditional.seraccdt) AND  main_vw.tdspaytype=1
*!*			_vacnm = ""
*!*		CASE !EVAL(coadditional.seraccdt) AND UPPER(_fldnm)='SERBAMT'
*!*			_vacnm = "Output Service Tax"
*!*		CASE  !EVAL(coadditional.seraccdt) AND UPPER(_fldnm)='SERCAMT'
*!*			_vacnm = "Edu. Cess on Output Service Tax"
*!*		CASE  !EVAL(coadditional.seraccdt) AND UPPER(_fldnm)='SERHAMT'
*!*			_vacnm = "S & H Cess on OutPut Service Tax"

*!*		CASE EVAL(coadditional.seraccdt) AND UPPER(_fldnm)='SERBAMT'
*!*			_vacnm = "Service Tax Adjustment A/C"
*!*		CASE EVAL(coadditional.seraccdt) AND UPPER(_fldnm)='SERCAMT'
*!*			_vacnm = "Edu. Cess on Service Tax Adjustment A/C"
*!*		CASE EVAL(coadditional.seraccdt) AND UPPER(_fldnm)='SERHAMT'
*!*			_vacnm = "S & H Cess on Service Tax Adjustment A/C"
*!*	ENDCASE
*!*	RETURN _vacnm



*!*	PROCEDURE sertaxaceffect_br_cac() &&Rup TKT-7412,TKT-8319 26/05/2011
*!*	PARAMETERS _fldnm
*!*	_vacnm = ''
*!*	DO CASE
*!*		CASE  main_vw.tdspaytype=2 AND UPPER(_fldnm)='SERBAMT'
*!*			_vacnm = "Service Tax Payable"
*!*		CASE   main_vw.tdspaytype=2 AND UPPER(_fldnm)='SERCAMT'
*!*			_vacnm = "Edu. Cess on Service Tax Payable"
*!*		CASE   main_vw.tdspaytype=2 AND UPPER(_fldnm)='SERHAMT'
*!*			_vacnm = "S & H Cess on Service Tax Payable"
*!*	ENDCASE
*!*	RETURN _vacnm

*!*	PROCEDURE findallocservicetype() &&Rup TKT-7412,TKT-8319 26/05/2011
*!*	_curform = _SCREEN.ACTIVEFORM
*!*	_curallocservicetype = ''
*!*	IF USED('Mall_vw')

*!*		SELECT mall_vw
*!*		SCAN
*!*			_curretval = _curform.sqlconobj.dataconn([EXE],company.dbname,"select top 1 SerRule from SerTaxMain_vw where Entry_ty = ?Mall_vw.Entry_all And Tran_cd = ?Mall_vw.Main_Tran","_TmpAlloc","This.Parent.nHandle",_curform.DATASESSIONID,.F.)
*!*			IF _curretval > 0 AND USED('_TmpAlloc')
*!*				_curallocservicetype = _tmpalloc.serrule
*!*				EXIT
*!*			ENDIF

*!*			SELECT mall_vw
*!*		ENDSCAN
*!*	ENDIF
*!*	RETURN _curallocservicetype
&&Changes has been done by vasant on 25/02/2012 as per Bug-6092 (Required "Service Tax REVERSE CHARGE MECHANISM" in our Default Products.)

&&--->TKT-8320 GTA
Procedure sertaxaceffect_if_cac() &&Used in GTA IF,OF Cr. A/c effect
Parameters _fldnm
_fldnm = Upper(_fldnm)
_vacnm = ''
_isadv = .F.
If Type('_mseravail') = 'C'
	If Inlist(_mseravail,"EXCISE","SERVICE")
		_isadv = .T.
	Endi
Endif
Select acdetalloc_vw

If Seek(item_vw.itserial,'acdetalloc_vw','itserial')
	Do Case
	Case _isadv = .T. And main_vw.serrule!='IMPORT'

		If _fldnm="SERBAMT"
			_vacnm="Service Tax Adjustment A/C"
		Endif
		If  _fldnm="SERCAMT"
			_vacnm="Edu. Cess on Service Tax Adjustment A/C"
		Endif
		If 	_fldnm="SERHAMT"
			_vacnm="S & H Cess on Service Tax Adjustment A/C"
		Endif

&& Added by Shrikant S. on 18/05/2016 for Bug-28132 Krishi Kalyan Cess	&& Start
		If _fldnm="SKKCAMT"
			_vacnm="Krishi Kalyan Cess Adjustment A/C"
		Endif
&& Added by Shrikant S. on 18/05/2016 for Bug-28132 Krishi Kalyan Cess	&& End

	Case _isadv = .F. And main_vw.serrule!='IMPORT'
		If _fldnm="SERBAMT"
			_vacnm="GTA Service Tax Payable"
		Endif
		If _fldnm="SERCAMT"
			_vacnm="GTA Edu. Cess on Service Tax Payable"
		Endif
		If _fldnm="SERHAMT"
			_vacnm="GTA S & H Cess on Service Tax Payable"
		Endif
&& Added by Shrikant S. on 13/11/2015 for Bug-Swachh Bharat Cess	&& Start
		If _fldnm="SERBCESS"
			_vacnm="GTA Swachh Bharat Cess Payable"
		Endif
&& Added by Shrikant S. on 13/11/2015 for Bug-Swachh Bharat Cess	&& End

&& Added by Shrikant S. on 18/05/2016 for Bug-28132 Krishi Kalyan Cess	&& Start
		If _fldnm="SKKCAMT"
			_vacnm="GTA Krishi Kalyan Cess Payable"
		Endif
&& Added by Shrikant S. on 18/05/2016 for Bug-28132 Krishi Kalyan Cess	&& End

	Endcase
Endif
Return _vacnm


Procedure sertaxaceffect_bp_gta() &&Used in GTA BP,CP Dr. A/c effect
Parameters _fldnm
_vacnm = ''
Do Case
Case Eval(coadditional.seraccdt) And  main_vw.tdspaytype=1
	_vacnm = ""
Case !Eval(coadditional.seraccdt) And Upper(_fldnm)='SERBAMT'
	_vacnm = "Output Service Tax"
Case  !Eval(coadditional.seraccdt) And Upper(_fldnm)='SERCAMT'
	_vacnm = "Edu. Cess on Output Service Tax"
Case  !Eval(coadditional.seraccdt) And Upper(_fldnm)='SERHAMT'
	_vacnm = "S & H Cess on OutPut Service Tax"
&& Added by Shrikant S. on 13/11/2015 for Bug-Swachh Bharat Cess	&& Start
Case  !Eval(coadditional.seraccdt) And Upper(_fldnm)='SERBCESS'
	_vacnm = "Swachh Bharat Cess Payable"
&& Added by Shrikant S. on 13/11/2015 for Bug-Swachh Bharat Cess	&& End

&& Added by Shrikant S. on 18/05/2016 for Bug-28132 Krishi Kalyan Cess	&& Start
Case  !Eval(coadditional.seraccdt) And Upper(_fldnm)='SKKCAMT'
	_vacnm = "Krishi Kalyan Cess Payable"
&& Added by Shrikant S. on 18/05/2016 for Bug-28132 Krishi Kalyan Cess	&& End

Case Eval(coadditional.seraccdt) And Upper(_fldnm)='SERBAMT'
	_vacnm = "Service Tax Adjustment A/C"
Case Eval(coadditional.seraccdt) And Upper(_fldnm)='SERCAMT'
	_vacnm = "Edu. Cess on Service Tax Adjustment A/C"
Case Eval(coadditional.seraccdt) And Upper(_fldnm)='SERHAMT'
	_vacnm = "S & H Cess on Service Tax Adjustment A/C"
Endcase
Return _vacnm

Procedure sertaxaceffect_bp_gta_cac() &&Used in GTA BP,CP Cr. A/c effect
Parameters _fldnm
_vacnm = ''
Do Case
Case  main_vw.tdspaytype=2 And Upper(_fldnm)='SERBAMT'
	_vacnm = "GTA Service Tax Payable"
Case   main_vw.tdspaytype=2 And Upper(_fldnm)='SERCAMT'
	_vacnm = "GTA Edu. Cess on Service Tax Payable"
Case   main_vw.tdspaytype=2 And Upper(_fldnm)='SERHAMT'
	_vacnm = "GTA S & H Cess on Service Tax Payable"
&& Added by Shrikant S. on 13/11/2015 for Bug-Swachh Bharat Cess	&& Start
Case   main_vw.tdspaytype=2 And Upper(_fldnm)='SERBCESS'
	_vacnm = "GTA Swachh Bharat Cess Payable"
&& Added by Shrikant S. on 13/11/2015 for Bug-Swachh Bharat Cess	&& end

&& Added by Shrikant S. on 18/05/2016 for Bug-28132 Krishi Kalyan Cess	&& Start
Case  main_vw.tdspaytype=2 And Upper(_fldnm)='SKKCAMT'
	_vacnm = "GTA Krishi Kalyan Cess Payable"
&& Added by Shrikant S. on 18/05/2016 for Bug-28132 Krishi Kalyan Cess	&& End

Endcase
Return _vacnm
&&<---TKT-8320 GTA
*****************************************************************************************
***************************ADDED BY SATISH PAL DT.04/01/2012*****************************

Proc gen_no_b() &&Birendra:-procedure is useful to generate Bond Serial No. on the base of rule, bond no.
Parameters fldnm,tblnm,tabletype

Local vrno
sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)
nhandle=0

If !Used('Gen_SrNo_Vw')
	etsql_str = "Select * From Gen_SrNo where 1=0"
	etsql_con = sqlconobj.dataconn([EXE],company.dbname,etsql_str,[Gen_SrNo_Vw],;
		"nHandle",_Screen.ActiveForm.DataSessionId,.F.)
	If etsql_con < 1 Or !Used("Gen_SrNo_Vw")
		etsql_con = 0
	Else
		Select gen_srno_vw
		Index On itserial Tag itserial
	Endif
Endif

*	sq1="SELECT MAX(CAST( "+ALLTRIM(fldnm)+"  AS INT)) AS RNO  FROM "+ALLTRIM(tblnm)+" WHERE ISNUMERIC( "+ALLTRIM(fldnm)+" )=1 and l_yn='"+ALLTRIM(main_vw.l_yn)+"' "+ IIF(coadditional.cate_srno," AND cate = ?Main_vw.Cate ","")
If Inlist(Alltrim(main_vw.Rule),'CT-1','CT-3','EXPORT')

	sq1="SELECT MAX(CAST( npgno AS INT)) AS RNO  FROM Gen_srno WHERE ISNUMERIC(npgno)=1 and l_yn='"+Alltrim(main_vw.l_yn)+"' and crule IN ('CT-1','CT-3','EXPORT') "+" and cBond_no = '"+ Alltrim(main_vw.bond_no) +" ' " +Iif(coadditional.cate_srno," AND Ccate = ?Main_vw.Cate ","") &&changes by sandeep Cate column rectified by ccate for bug-18474 ON 20-Aug-13
Else
*		sq1="SELECT MAX(CAST( npgno AS INT)) AS RNO  FROM Gen_srno WHERE ISNUMERIC(npgno)=1 and l_yn='"+ALLTRIM(main_vw.l_yn)+"' and crule = '"+ ALLTRIM(main_vw.RULE) +" ' and cBond_no = '"+ ALLTRIM(main_vw.bond_no) +" ' " +IIF(coadditional.cate_srno," AND cate = ?Main_vw.Cate ","")
*added by Birendra: for EOU itemwise bond sr no on 26 nov 2010
	sq1="SELECT MAX(CAST( npgno AS INT)) AS RNO  FROM gen_srno WHERE ISNUMERIC(npgno)=1 and l_yn='"+Alltrim(main_vw.l_yn)+"' and crule = '"
	zx = "tt"
	If Vartype(tabletype)='C' And coadditional.eou
		zx = tabletype + ".u_rule"
	Endif
	sq1= sq1 + Iif(Vartype(tabletype)='C',Alltrim(Iif(Inlist(Upper(tabletype),"ITEM_VW","ISSDET"),&zx,main_vw.Rule)),main_vw.Rule)
	sq1= sq1 +" ' and cBond_no = '"
	If Vartype(tabletype)='C'
		zx = tabletype + ".bond_no"
	Endif
	sq1= sq1 + Iif(Vartype(tabletype)='C',Alltrim(Iif(Inlist(Upper(tabletype),"ITEM_VW","ISSDET"),Alltrim(&zx),Alltrim(main_vw.bond_no))),Alltrim(main_vw.bond_no))

	sq1= sq1 +" ' " +Iif(coadditional.cate_srno," AND cate = ?Main_vw.Cate ","")
*end by birendra:

*IIF(VARTYPE(tabletype)='C',Alltrim(IIF(UPPER(tabletype) = "ITEM_VW",item_vw.u_rule,main_vw.rule)),main_vw.rule)
Endif

nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"EXCUR","nHandle",_Screen.ActiveForm.DataSessionId)
If nretval<0
	Return .F.
Endif
Select excur
vrno=Alltrim(Str(Iif(Isnull(excur.rno),1,(excur.rno)+1)))

*added by Birendra: for EOU itemwise bond sr no on 26 nov 2010
If Vartype(tabletype)='C' And coadditional.eou
	If Inlist(Upper(tabletype),"ITEM_VW","ISSDET")
		Select gen_srno_vw
*!*				zx = "Locate For cit_code = " + tabletype + ".it_code And ctype='BONDSRNO' "
*!*				zx = zx+"and tran_cd =" +tabletype+".tran_cd and itserial = "+tabletype+".itserial and itserial1 = item_vw.itserial"
		zx = "Locate For cit_code="+tabletype +".it_code And itserial=" +"item_vw.itserial And ctype='BONDSRNO' and tran_cd = main_vw.tran_cd"
		If Upper(tabletype)<>"ITEM_VW"
			zx = zx+" and aitserial=" +tabletype+".itserial And atran_cd = "+tabletype+".tran_cd and aentry_ty = "+tabletype+".entry_ty"
		Endif
		&zx
		If Found()
			If tabletype = 'issdet'
				Select rmdet_vw
				zx = "Locate For li_it_code = " + tabletype + ".it_code "
				zx = zx+"and li_tran_cd =" +tabletype+".tran_cd and li_itser = "+tabletype+".itserial and itserial = item_vw.itserial"
				&zx
				zx = "Empty( "+tabletype+".bondsrno)"
				If &zx
					Select gen_srno_vw
					zx="SELECT MAX(npgno) FROM gen_srno_vw WHERE crule = "+tabletype+".u_rule INTO array vrno1"
					&zx
					If Int(Val(vrno1))> 0
						vrno = Int(Val(vrno1)) + 1
					Endif
				Else
					Select gen_srno_vw
					If !Empty(rmdet_vw.bondsrno)
						Locate For Alltrim(npgno)=Alltrim(rmdet_vw.bondsrno)
					Endif
					vrno = npgno
				Endif
			Else
				Select gen_srno_vw
				zx = "Locate For cit_code="+tabletype +".it_code And itserial=" +"item_vw.itserial And ctype='BONDSRNO' and tran_cd = main_vw.tran_cd "
*						zx = zx+" and aitserial=" +tabletype+".itserial And atran_cd = "+tabletype+".tran_cd and aentry_ty = "+tabletype+".entry_ty"
				&zx
				If Found()
					zx1 = Recno()
					If Alltrim(crule)<>Alltrim(item_vw.u_rule)
						zx="SELECT MAX(npgno) FROM gen_srno_vw WHERE crule = "+tabletype+".u_rule and RECNO()<>zx1 INTO array vrno1"
						&zx
						If Int(Val(vrno1))> 0
							vrno = Int(Val(vrno1)) + 1
						Endif
					Else
						vrno = npgno
					Endif
				Endif
			Endif
		Else
			zx="SELECT MAX(npgno) FROM gen_srno_vw WHERE crule = "+tabletype+".u_rule INTO array vrno1"
			&zx
			If Int(Val(vrno1))> 0
				vrno = Int(Val(vrno1)) + 1
			Endif
		Endi
	Endif
Endif
*end by birendra:

If Used("EXCUR")
	Use In excur
Endif
*sele(mAlias)
Return vrno
Procedure fcnmlostfocusproc
sql_updt = 0
If Upper(Alltrim(item_vw.u_fcname))	== Upper(Alltrim(company.Currency))
	sql_updt = 0
Else
&& Added by Ajay Jaiswal For EOU on 03/09/2010 - Start
	nretval = _Screen.ActiveForm.sqlconobj.dataconn([EXE],company.dbname,[SELECT currencyid from curr_mast ;
				where curr_mast.currencycd = ?item_vw.u_fcname],[EXCUR],"This.Parent.nHandle",_Screen.ActiveForm.DataSessionId ,.F.)
	If nretval<0
		Return .F.
	Endif

	tcurrencyid = excur.currencyid
&& Added by Ajay Jaiswal For EOU on 03/09/2010 - End
	msqlstr = []
	msqlstr = Iif(lcode_vw.fcrateon='IMPORT',[impcurrrate],[expcurrrate])
	sql_con = _Screen.ActiveForm.sqlconobj.dataconn([EXE],company.dbname,[Select Top 1 ]+msqlstr+[ as FcRate From Curr_rate ;
							Where Currencyid = ?tcurrencyid And Remark = 'Daily' And Currdate <= ?Main_vw.Date ;
							Order By Currdate Desc],[_xtrtblv],;
		"This.Parent.nHandle",_Screen.ActiveForm.DataSessionId ,.F.)
	If sql_con > 0 And Used('_xtrtblv')
		Select _xtrtblv
		If Reccount() > 0
			sql_updt = _xtrtblv.fcrate
		Endif
	Endif
Endif		&&atlas140408

If sql_updt >= 0
	Replace u_fcexrate With sql_updt In item_vw
Endif
Endproc
Procedure chk_bondsrno
sq1 = " select b.bondsrno from ptitref a inner join ptmain b on (a.entry_ty =b.entry_ty and a.tran_cd=b.tran_cd) "
sq2 = " where (a.rentry_ty= '"+	main_vw.entry_ty+"' and a.itref_tran = "+Str(main_vw.tran_cd)+") "
sq3 = " union select b.bondsrno from aritref a inner join armain b on (a.entry_ty =b.entry_ty and a.tran_cd=b.tran_cd) "
sq4 = " where (a.rentry_ty= '"+	main_vw.entry_ty+"' and a.itref_tran = "+Str(main_vw.tran_cd)+") "

nretval = _Screen.ActiveForm.sqlconobj.dataconn([EXE],company.dbname,sq1+sq2+sq3+sq4,"ajcur","This.Parent.nHandle",_Screen.ActiveForm.DataSessionId,.F.)
If nretval <0
	Return .F.
Endif

srnofound=.T.

Select ajcur
Scan
	If main_vw.bondsrno<>ajcur.bondsrno
		srnofound = .F.
	Endif
Endscan
Return srnofound
Endproc
Procedure dupbond_no &&RUP:-procedure is useful to check duplicate value of part-ii,pla field and in Daily Debit form UEFRM_ST_DAILTDEBIT in Sales Entry.
Parameters fldnm,fldval,tblnm,mcommit,nhand,tabletype
* SET STEP ON
If !Empty(fldval)
	_malias 	= Alias()
	_mrecno	= Recno()
**notype =Iif(Upper(fldnm)=='Bondsrno','RGAPART2',Iif(Upper(fldnm)=='U_RG23CNO','RGCPART2',Iif(Upper(fldnm)=='U_PLASR','PLASRNO',"")))
	Local vdup
	If fldval=' '
		Return .T.
	Endif
	sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)

	nhandle=0
	If Used('Gen_SrNo_Vw')
		Select gen_srno_vw
		mrec=Recno()
	Endif
	If !Used('Gen_SrNo_Vw')
		etsql_str = "Select * From Gen_SrNo where 1=0"
		etsql_con = sqlconobj.dataconn([EXE],company.dbname,etsql_str,[Gen_SrNo_Vw],;
			"nHandle",_Screen.ActiveForm.DataSessionId,.F.)
		If etsql_con < 1 Or !Used("Gen_SrNo_Vw")
			etsql_con = 0
		Else
			Select gen_srno_vw
			Index On itserial Tag itserial
		Endif
	Endif

*sq1="SELECT NpgNo FROM Gen_Srno WHERE l_yn='"+Alltrim(main_vw.l_yn)+"' "+Iif(Upper(fldnm)=='U_RG23NO'," AND CTYPE='RGAPART2'",Iif(Upper(fldnm)=='U_RG23CNO'," AND CTYPE='RGCPART2'",Iif(Upper(fldnm)=='U_PLASR'," AND CTYPE='PLASRNO'","")))+" AND NpgNo='"+Alltrim(fldval)+"' AND NOT (TRAN_CD="+Str(main_vw.tran_cd)+" AND ENTRY_TY='"+main_vw.entry_ty+"'"+Iif(fldnm='U_PAGENO'," AND "+tblnm+".ITSERIAL='"+item_vw.itserial+"')",")")+ Iif(coadditional.cate_srno," AND cate = ?Main_vw.Cate ","")
**	IF INLIST(ALLTRIM(main_vw.RULE),'CT-1','CT-3','EXPORT')
*		sq1="SELECT npgno  FROM Gen_srno WHERE l_yn='"+ALLTRIM(main_vw.l_yn)+"' and ctype = 'BONDSRNO' and crule = '"+ ALLTRIM(main_vw.RULE) +" 'and cBond_no = '"+ ALLTRIM(main_vw.bond_no) +" ' " + " AND NpgNo='"+Alltrim(fldval)+"' AND NOT (TRAN_CD="+Str(main_vw.tran_cd)+" AND ENTRY_TY='"+main_vw.entry_ty+"')"+IIF(coadditional.cate_srno," AND cate = ?Main_vw.Cate ","")
*birendra : above line commented and bellow added line of code for EOU
	If Vartype(tabletype)='C' And coadditional.eou
		If Inlist(Upper(tabletype),"ITEM_VW","ISSDET")
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
			sq1="SELECT npgno  FROM Gen_srno WHERE l_yn='"+Alltrim(main_vw.l_yn)+"' and ctype = 'BONDSRNO' and crule = '"+ tmprule +"' and cBond_no = '"+ Alltrim(tmpbondno) +"'" + " AND NpgNo='"+Alltrim(fldval)+"' AND NOT (TRAN_CD="+Str(main_vw.tran_cd)+" AND ENTRY_TY='"+main_vw.entry_ty+"' and ITSERIAL='"+item_vw.itserial
			If Upper(tabletype)<>"ITEM_VW"
				sq1=sq1+"' AND aITSERIAL='"+Alltrim(tmpitserial)+"' and aTRAN_CD="+Alltrim(Str(tmptrancd))+" AND aENTRY_TY='"+Alltrim(tmpentryty)
			Endif
			sq1=sq1+"')"

			sq1=sq1+Iif(coadditional.cate_srno," AND Ccate = ?Main_vw.Cate ","")&&changes by sandeep Cate column rectified by ccate for bug-18474 ON 20-Aug-13
		Else

			sq1="SELECT npgno  FROM Gen_srno WHERE l_yn='"+Alltrim(main_vw.l_yn)+"' and ctype = 'BONDSRNO' and crule = '"+ Alltrim(main_vw.Rule) +" 'and cBond_no = '"+ Alltrim(main_vw.bond_no) +" ' " + " AND NpgNo='"+Alltrim(fldval)+"' AND NOT (TRAN_CD="+Str(main_vw.tran_cd)+" AND ENTRY_TY='"+main_vw.entry_ty+"')"+Iif(coadditional.cate_srno," AND Ccate = ?Main_vw.Cate ","")&&changes by sandeep Cate column rectified by ccate for bug-18474 ON 20-Aug-13
		Endif
	Else
		sq1="SELECT npgno  FROM Gen_srno WHERE l_yn='"+Alltrim(main_vw.l_yn)+"' and ctype = 'BONDSRNO' and crule = '"+ Alltrim(main_vw.Rule) +" ' and cBond_no = '"+ Alltrim(main_vw.bond_no) +"' " + " AND NpgNo='"+Alltrim(fldval)+"' AND NOT (TRAN_CD="+Str(main_vw.tran_cd)+" AND ENTRY_TY='"+main_vw.entry_ty+"')"+Iif(coadditional.cate_srno," AND Ccate = ?Main_vw.Cate ","")&&changes by sandeep Cate column rectified by ccate for bug-18474 ON 20-Aug-13
	Endif
*!*		ELSE
*!*			sq1="SELECT MAX(CAST( npgno AS INT)) AS RNO  FROM Gen_srno WHERE ISNUMERIC(npgno)=1 and l_yn='"+ALLTRIM(main_vw.l_yn)+"' and ctype = 'BONDSRNO' and crule = '"+ ALLTRIM(main_vw.[RULE]) +" ' and cBond_no = '"+ ALLTRIM(main_vw.bond_no) +" ' " +IIF(coadditional.cate_srno," AND cate = ?Main_vw.Cate ","")
*!*		ENDIF
	nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"EXCUR",Iif(mcommit=.F.,"nHandle",nhand),_Screen.ActiveForm.DataSessionId)	&&280910
	If nretval<0
		Return .F.
	Endif
	Select excur
	vrcount=Reccount()
*	MESSAGEBOX(vrcount)
	If nretval >0 And Used("EXCUR")
		If	vrcount<=0
			Select gen_srno_vw
			Go Top
*Change by Birendra : EOU item wise Bond sr no gen
			If Vartype(tabletype)='C' And coadditional.eou
				If Inlist(Upper(tabletype),"ITEM_VW","ISSDET")
					zx = "Locate For cit_code="+tabletype +".it_code And itserial=" +"item_vw.itserial And ctype='BONDSRNO' and tran_cd = main_vw.tran_cd"
					If Upper(tabletype)<>"ITEM_VW"
						zx = zx+" and aitserial=" +tabletype+".itserial And atran_cd = "+tabletype+".tran_cd and aentry_ty = "+tabletype+".entry_ty"
					Endif
*					zx = zx + IIF(INLIST(UPPER(tabletype),"ITEM_VW")," AND CRULE = "+tabletype+".U_RULE","")
					&zx
					If !Found()
						Append Blank In gen_srno_vw
					Endif

					If Inlist(ctype,"BONDSRNO") Or Empty(ctype)
						zx="Replace ccate With "+tabletype+".cate,npgno With fldval,entry_ty With main_vw.entry_ty, tran_cd With main_vw.tran_cd,compid With main_vw.compid,"
						zx =zx +"ctype With 'BONDSRNO',l_yn With main_vw.l_yn,CRULE WITH "+tabletype+".U_RULE,CBOND_NO WITH "+tabletype+".BOND_NO,"
						zx =zx+"cit_Code WITH "+tabletype+".it_code,itserial WITH "+"item_vw.itserial,"
						zx =zx+"aentry_ty WITH "+tabletype+".entry_ty,atran_cd with "+tabletype+".tran_cd,aitserial WITH "+tabletype+".itserial"
						&zx
					Endif
				Else
					Locate For entry_ty=main_vw.entry_ty And tran_cd=main_vw.tran_cd And (ctype='BONDSRNO')
					If !Found()
						Append Blank In gen_srno_vw
					Endif

					If Inlist(ctype,"BONDSRNO") Or Empty(ctype)
						Replace ccate With main_vw.cate,npgno With fldval,entry_ty With main_vw.entry_ty, tran_cd With main_vw.tran_cd,compid With main_vw.compid,;
							ctype With 'BONDSRNO',l_yn With main_vw.l_yn,crule With main_vw.Rule,cbond_no With main_vw.bond_no
					Endif
				Endif
			Else
				Locate For entry_ty=main_vw.entry_ty And tran_cd=main_vw.tran_cd And (ctype='BONDSRNO')
				If !Found()
					Append Blank In gen_srno_vw
				Endif

				If Inlist(ctype,"BONDSRNO") Or Empty(ctype)
					Replace ccate With main_vw.cate,npgno With fldval,entry_ty With main_vw.entry_ty, tran_cd With main_vw.tran_cd,compid With main_vw.compid,;
						ctype With 'BONDSRNO',l_yn With main_vw.l_yn,crule With main_vw.Rule,cbond_no With main_vw.bond_no
				Endif
			Endif
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
			If Betw(mrec,1,Reccount())
				Go mrec
			Endif
		Endif
		Use In excur
	Endif
	=sqlconobj.sqlconnclose("nHandle")
*	MESSAGEBOX('b')
	If !Empty(_malias)
		Select &_malias
	Endif
* 	MESSAGEBOX('c')
	If Betw(_mrecno,1,Reccount())
		Go _mrecno
	Endif
* 	MESSAGEBOX('d')
	If vrcount>0 And !Isnull(vrcount)
		Return .F.
	Else
		Return .T.
	Endif
Endif
Return

***************************END BY SATISH PAL DT.04/01/2012*******************************
*****************************************************************************************
*Birendra : Auto Debit and Credit Note on 18 july 2011 :Start:
Procedure calc_brok_comm
If Type('item_vw.billqty')<>'U'
	If item_vw.billqty > 0 And main_vw.entry_ty='DB'
		Select item_vw
		Replace item_vw.brok_amt With Round(main_vw.brokperm*item_vw.billqty,0) In item_vw
		Replace item_vw.comm_amt With Round(main_vw.compermt*item_vw.billqty,0) In item_vw
		Replace item_vw.gro_amt With (item_vw.brok_amt+item_vw.comm_amt) In item_vw
		Replace item_vw.rate With (item_vw.gro_amt/item_vw.qty) In item_vw
	Endif
Endif

*	IF INLIST(main_vw.entry_TY,'D1','D2','D3','D4','D5','CI') AND qty > 0
*!*	IF INLIST(main_vw.entry_ty,'D1','D2','D3','D4','D5','C2','C3','C4','C5') AND qty > 0		&& Commented By Shrikant S. on 17/03/2012 for Bug-2276
If Inlist(main_vw.entry_ty,'D2','D3','D4','D5','C2','C3','C4','C5') And qty > 0			&& Added By Shrikant S. on 17/03/2012 for Bug-2276
	Select item_vw
	Replace item_vw.comm_amt With Iif(main_vw.compermt>0,main_vw.compermt*item_vw.qty,item_vw.comm_amt) In item_vw
	Replace item_vw.rate With  Iif(main_vw.compermt>0,main_vw.compermt,item_vw.comm_amt) In item_vw
Endif

*----------------For Auto Debit Note----------------------------------------
*	IF INLIST(main_vw.entry_TY,'DI','CI') AND main_vw.dept ='LATE PAYMENT'
*	IF INLIST(main_vw.entry_TY,'D3','CI')
If Inlist(main_vw.entry_ty,'D3','C3')
	ztmpintbaseday=365
	If _Screen.ActiveForm.intbaseday>0
		ztmpintbaseday=_Screen.ActiveForm.intbaseday
	Endif
	Select item_vw
	Replace item_vw.qty With 1 In item_vw
	Replace item_vw.intamt With Round(((item_vw.recdamt*(item_vw.intper/100))/ztmpintbaseday)*item_vw.ltdays,0) In item_vw
	Replace item_vw.rate With intamt In item_vw
	Replace item_vw.gro_amt With item_vw.rate*item_vw.qty In item_vw

Endif
*----------------------------------------------------------------------------

Return
*Birendra : Auto Debit and Credit Note on 18 july 2011 :End:

&& Start : Added by Uday on 26/12/2011 for EXIM
Procedure saveoppttaxdet()
If Inlist(main_vw.entry_ty,'OP')
	Local mobj
	mobj = _Screen.ActiveForm

	If Used("Projectitref_vw")
		Delete From op_pttax_vw Where entry_ty = main_vw.entry_ty And tran_cd = main_vw.tran_cd

		Do While !Eof("Projectitref_vw")
			lcstr = "execute USP_ENT_EXIM_OP_PT_DUTY 'IP','" + Alltrim(projectitref_vw.aentry_ty) +;
				"'," + Alltrim(Str(projectitref_vw.atran_cd)) + ",'" + Alltrim(projectitref_vw.aitserial) + "'"

			sql_con = mobj.sqlconobj.dataconn([EXE],company.dbname,lcstr,[cursIpItem],;
				"Thisform.nHandle",mobj.DataSessionId,.F.)

			If sql_con < 1
				=mobj.sqlconobj.sqlconnclose("Thisform.nHandle")
				mobj.showmessagebox("Error while retrive records from IP ",32,vumess)	&&test_z 32
				Return 0
			Endif

			=mobj.sqlconobj.sqlconnclose("Thisform.nHandle")

			If Used("cursIpItem")
				Select cursipitem
				Go Top
				Do While !Eof("cursIPItem")

					If Used("CursDcMast")
						Use In cursdcmast
					Endif

					Do Case
					Case Alltrim(Upper(cursipitem.Rule)) = 'IMPORTED'
						lcstr = [Select Fld_Nm,Pert_Name,Head_Nm From Dcmast Where Code = 'E' And Entry_Ty = 'P1' AND Att_File = 0 order by corder]
					Case Alltrim(Upper(cursipitem.Rule)) = 'ANNEXURE IV' Or Alltrim(Upper(cursipitem.Rule)) = 'IMPORTED'
						lcstr = [Select Fld_Nm,Pert_Name,Head_Nm From Dcmast Where Code = 'E' And Entry_Ty = 'P1' AND Att_File = 0 order by corder]
					Otherwise
						lcstr = [Select Fld_Nm,Pert_Name,Head_Nm From Dcmast Where Code = 'E' And Entry_Ty = 'PT' AND Att_File = 0 order by corder]
					Endcase
					sql_con = mobj.sqlconobj.dataconn([EXE],company.dbname,lcstr,[cursdcmast],;
						"Thisform.nHandle",mobj.DataSessionId,.F.)
					If sql_con < 1
						mobj.showmessagebox("Error while open Discount charges Master table",32,vumess)	&&test_z 32
					Endif

					Append Blank In op_pttax_vw

					Replace entry_ty With item_vw.entry_ty,;
						tran_cd  With  item_vw.tran_cd,;
						itserial With  item_vw.itserial,;
						pentry_ty With cursipitem.pentry_ty,;
						ptran_cd With cursipitem.ptran_cd,;
						pitserial With cursipitem.pitserial,;
						it_code With cursipitem.it_code,;
						ITEM    With cursipitem.Item,;
						qty     With cursipitem.qty,;
						genby   With musername In op_pttax_vw

					Select cursdcmast
					Go Top
					ztmpexamt = 0
					ztmpexamt1 = 0

					Do While !Eof()
						tfld_nm = Space(05)
						flditname = "cursIPItem."+Alltrim(cursdcmast.fld_nm)
						fldname   = "Op_PtTax_vw."+Substr(Alltrim(cursdcmast.fld_nm),Iif(At("U_",Upper(Alltrim(cursdcmast.fld_nm)))>0,At("U_",Upper(Alltrim(cursdcmast.fld_nm)))+2,1),Len(cursdcmast.fld_nm))
						flditname1 = "cursIPItem."+Alltrim(cursdcmast.pert_name)
						fldname1   = "Op_PtTax_vw."+Substr(Alltrim(cursdcmast.pert_name),Iif(At("U_",Upper(Alltrim(cursdcmast.pert_name)))>0,At("U_",Upper(Alltrim(cursdcmast.pert_name)))+2,1),Len(cursdcmast.pert_name))

						Store "" To upfields
						If Type(fldname) # 'U'
							upfields = "Replace "+Alltrim(fldname)+" WITH "+Alltrim(flditname)
						Endif

						If Type(fldname1) # 'U'
							upfields = upfields +", "+Alltrim(fldname1)+" with "+Alltrim(flditname1)+" in op_pttax_vw"
						Else
							upfields = upfields + Iif(!Empty(upfields)," in op_pttax_vw","")
						Endif

						If !Empty(upfields)
							&upfields
						Endif

						Skip In cursdcmast
					Enddo
					Skip In cursipitem
				Enddo
			Endif
			Skip In projectitref_vw
		Enddo
		If Used("cursIPItem")
			Use In cursipitem
		Endif
		With mobj.voupage.pgappduties.grdappduties
			.Refresh()
		Endwith
	Endif
Endif
Endproc
&& End : Added by Uday on 26/12/2011 for EXIM


***** Added By Sachin N. S. on 17/09/2012 for Bug-5164 ***** Start
***** Procedure to generate Service Tax Serial No.
Proc Gen_ServTxSrno
*!*	Parameters SRNoType,_mcommit,_nhandle				&&Commented by vasant on 18/07/2014 as per Bug 23384 - (Issue In Service Tax Credit Register).
Parameters SRNoType,_mcommit,_nhandle,_ltblnm,_lfldnm			&&Added by vasant on 18/07/2014 as per Bug 23384 - (Issue In Service Tax Credit Register).
Local srno
sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)
nhandle=0
sq1="SELECT ISNULL(MAX(CONVERT(INT,NPGNO)),0) AS NPGNO FROM GEN_SRNO WHERE CTYPE = '"+Alltrim(SRNoType)+"' and l_yn='"+Alltrim(main_vw.l_yn)+"' group by CTYPE,l_yn "
nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"EXCUR",Iif(Type('_nHandle')='L',"nHandle",_nhandle),_Screen.ActiveForm.DataSessionId)
If nretval<0
	Return .F.
Endif
Select excur
_mrgpage=Alltrim(Str(Iif(Isnull(excur.npgno),1,(excur.npgno)+1)))

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
	If Alltrim(_mrgpage) <= Allt(gen_srno_vw.npgno) And ctype = SRNoType And tran_cd!=main_vw.tran_cd
		_mrgpage = Alltrim(Str(Iif(Isnull(gen_srno_vw.npgno),0,Val(gen_srno_vw.npgno)) + 1))
	Endif
	Select gen_srno_vw
Endscan

&&Added by vasant on 18/07/2014 as per Bug 23384 - (Issue In Service Tax Credit Register).
If Type('_ltblnm') = 'C' And Type('_lfldnm') = 'C'
	Replace (_lfldnm) With _mrgpage In (_ltblnm)
	_lreffldnm = 'frmxtra.txt'+Alltrim(_lfldnm)+'.refresh'
	&_lreffldnm
Endif
&&Added by vasant on 18/07/2014 as per Bug 23384 - (Issue In Service Tax Credit Register).


If Used("EXCUR")
	Use In excur
Endif
Return _mrgpage

***** Procedure to Check Duplicate Service Tax Serial No.
Procedure Dup_ServTxSrNo
Parameters SRNoType,fldval,_itSerial,_SerTy,_mcommit,_nhandle
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
nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"EXCUR",Iif(Type('_nHandle')='L',"nHandle",_nhandle),_Screen.ActiveForm.DataSessionId)
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
&& Added by Shrikant S. on 16/06/2015 for Bug-26133		&& Start
	If (_txtObj.Parent.Parent.Parent.oParForm.IsBarCode =.T.)
		If (Alltrim(_txtObj.Tag) <> Alltrim(_valCur))
			lcfld_nm=""
			lcfld_val=""
			Select BarCodeSrNo_vw
			Locate
			Locate For Alltrim(Inventsrno)==Alltrim(_txtObj.Tag)
			If Found()
				lcfld_nm=Alltrim(BarCodeSrNo_vw.fld_nm)
				_mBCRetValue = _curform.oParForm.BarCodeObj.RemLine('S')
				If _txtObj.Parent.Parent.Parent.oParForm.addmode
					Delete In BarCodeSrNo_vw
				Endif
			Endif

			Select BarCodeMast_vw
			lnrecno=Iif(!Eof(),Recno(),0)
			If (lnrecno > 0)
				Select BarCodeMast_vw
				Go lnrecno
			Else
				Locate
			Endif
			lcfld_nm="Alltrim("+Alltrim(BarCodeMast_vw.Fld_Nm2)+")"
			If !Empty(lcfld_nm)
				Select BarCodeTran_vw
				Locate
				Locate For &lcfld_nm==Alltrim(_txtObj.Tag)
				If Found()
					If _txtObj.Parent.Parent.Parent.oParForm.addmode
						Delete In BarCodeTran_vw
					Endif
				Endif
			Endif
		Endif
	Endif
&& Added by Shrikant S. on 16/06/2015 for Bug-26133		&& End
Endif
Return .T.
Endproc
***** Added by Sachin N. S. on 21/11/2012 for Bug-7313 ***** End

&& Added by Shrikant S. on 16/06/2015 for Bug-26133		&& Start
Procedure GetValueBeforeChange
Lparameters _txtObj
_txtObj.Tag=_txtObj.Value
Return .T.
Endproc
&& Added by Shrikant S. on 16/06/2015 for Bug-26133		&& End

&& Added By Shrikant S. on 05/11/2012 for Bug-6867		&& Start
Procedure SetOldValue
Parameters objParam
If !Empty(objParam.Value) && added by sandeep for bug-16364 on 24-jun-13
	objParam.Tag=Dtoc(objParam.Value)
Endif
Return .T.
Endproc
&& Added By Shrikant S. on 05/11/2012 for Bug-6867		&& End

&&Changes has been done by vasant on 25/02/2012 as per Bug-6092 (Required "Service Tax REVERSE CHARGE MECHANISM" in our Default Products.)
Procedure SerTaxRecApply
Lparameters _SerTaxTranType
etsql_con	= 1
etsql_str	= ''
_oldconval  = 0
nhandle     = 0
_etdatasessionid = _Screen.ActiveForm.DataSessionId
_etactform       = _Screen.ActiveForm
Set DataSession To _etdatasessionid
If Type('_etactform.sqlconobj') = 'O'
	sqlconobj1 = _etactform.sqlconobj
Else
	sqlconobj1 = Newobject('SqlConnUdObj','SqlConnection',xapps)
Endif
If Type('_etactform.nhandle') = 'N'
	_oldconval = _etactform.nhandle
Endif
If _oldconval > 0
	nhandle = _oldconval
Endif
_mSerTaxRecApply = .F.

lcserty=Iif(Type('Item_vw.serty')<>'U',item_vw.serty,acdetalloc_vw.serty)		&& Added by Shrikant S. on 06/10/2016 for GST
*!*	If !Empty(coadditional.TypeOrg) And !Empty(acdetalloc_vw.Serty)		&& Commented by Shrikant S. on 20/10/2016 for GST
If !Empty(coadditional.ConstBusi) And !Empty(lcserty)		&& Added by Shrikant S. on 20/10/2016 for GST

*!*		_mCoSerDed_Type  = Upper(Alltrim(coadditional.TypeOrg))
	_mCoSerDed_Type  = Upper(Alltrim(coadditional.ConstBusi))
	_mAcSerDed_Type  = ''
	_mRevSerProvider = 'N.A.'
	_mRevSerReceiver = 'N.A.'

*!*		etsql_str = "Select Top 1 RevSerProvider,RevSerReceiver From Sertax_mast Where Name = ?AcdetAlloc_vw.Serty"
	etsql_str = "Select Top 1 RevSerProvider,RevSerReceiver From Sertax_mast Where Name = ?lcserty"

	etsql_str = etsql_str + " and (?Main_vw.Date Between SDate and EDate)"
	etsql_con = sqlconobj1.dataconn([EXE],company.dbname,etsql_str,"_tmpdata","nHandle",_etdatasessionid)
	If etsql_con > 0 And Used('_tmpdata')
		If Reccount('_tmpdata')	> 0
			_mRevSerProvider = Upper(_tmpdata.RevSerProvider)
			_mRevSerReceiver = Upper(_tmpdata.RevSerReceiver)
		Endif
	Endif

	etsql_str = "Select Top 1 SerDed_Type From Ac_mast Where Ac_id = ?Main_vw.Ac_id"
	etsql_con = sqlconobj1.dataconn([EXE],company.dbname,etsql_str,"_tmpdata","nHandle",_etdatasessionid)
	If etsql_con > 0 And Used('_tmpdata')
		If Reccount('_tmpdata')	> 0
			_mAcSerDed_Type = Upper(Alltrim(_tmpdata.SerDed_Type))
		Endif
	Endif

	If !Empty(_mAcSerDed_Type)
		If _SerTaxTranType = 'REC'
			If (_mCoSerDed_Type $ _mRevSerReceiver Or Empty(_mRevSerReceiver)) And (_mAcSerDed_Type $ _mRevSerProvider Or Empty(_mRevSerProvider))
				_mSerTaxRecApply = .T.
			Endif
		Endif
		If _SerTaxTranType = 'PRO'
			If (_mCoSerDed_Type $ _mRevSerProvider Or Empty(_mRevSerProvider)) And (_mAcSerDed_Type $ _mRevSerReceiver Or Empty(_mRevSerReceiver))
				_mSerTaxRecApply = .T.
			Endif
		Endif
	Endif
Endif
If _oldconval <= 0
	=sqlconobj1.sqlconnclose("nHandle")
Endif
Release sqlconobj1
Return _mSerTaxRecApply


Procedure calc_servicetax_s1()
_sertaxableamt = 0
Select acdetalloc_vw
If Seek(item_vw.itserial,'acdetalloc_vw','itserial')  &&And Upper(main_vw.serrule)<>'EXEMPT'
	_sertaxableamt = acdetalloc_vw.staxable
Endif
Return _sertaxableamt

Procedure calculateservicetax()
_etactform       = _Screen.ActiveForm	&&Changes has been done by vasant on 24/04/2013 as per Bug 11644 (The Updates for 'Service Tax Reverse Mechanism'(BUG-6092) are not working when 'Multi Currency' option is selected for Service Tax Transactions).
_sertaxableamt = 0
m_etvalidalias = Alias()
If Used('AcdetAlloc_vw')
	Select acdetalloc_vw
	m_etvalidrecno = Iif(!Eof(),Recno(),0)
	Sum(staxable) To _sertaxableamt For serty <> 'OTHERS' And !Empty(serty)
	Select acdetalloc_vw
	If m_etvalidrecno > 0
		Go m_etvalidrecno
	Endif
Endif
If !Empty(m_etvalidalias)
	Select (m_etvalidalias)
Endif
&&Changes has been done by vasant on 24/04/2013 as per Bug 11644 (The Updates for 'Service Tax Reverse Mechanism'(BUG-6092) are not working when 'Multi Currency' option is selected for Service Tax Transactions).
If lcode_vw.Multi_Cur   = .T.
	If Upper(Alltrim(main1_vw.Fcname)) != Upper(Alltrim(company.Currency)) And !Empty(main1_vw.Fcname)
		_sertaxableamt = _sertaxableamt/main_vw.fcexrate
	Endif
Endif
&&Changes has been done by vasant on 24/04/2013 as per Bug 11644 (The Updates for 'Service Tax Reverse Mechanism'(BUG-6092) are not working when 'Multi Currency' option is selected for Service Tax Transactions).
Return _sertaxableamt


Procedure EtValidFindAcctName()
Parameters _typnm
etsql_con	= 1
etsql_str	= ''
_oldconval  = 0
nhandle     = 0
_etdatasessionid = _Screen.ActiveForm.DataSessionId
_etactform       = _Screen.ActiveForm
Set DataSession To _etdatasessionid
If Type('_etactform.sqlconobj') = 'O'
	sqlconobj1 = _etactform.sqlconobj
Else
	sqlconobj1 = Newobject('SqlConnUdObj','SqlConnection',xapps)
Endif
If Type('_etactform.nhandle') = 'N'
	_oldconval = _etactform.nhandle
Endif
If _oldconval > 0
	nhandle = _oldconval
Endif
_typacnm = ''
If !Empty(_typnm)
	etsql_str = "Select Top 1 Ac_Name From Ac_mast Where Typ = ?_typnm"
	etsql_str = etsql_str + " And (ldeactive = 0 Or (ldeactive = 1 And deactfrom > ?Main_vw.Date))"
	etsql_con = sqlconobj1.dataconn([EXE],company.dbname,etsql_str,"_tmpdata","nHandle",_etdatasessionid)
	If etsql_con > 0 And Used('_tmpdata')
		If Reccount('_tmpdata')	> 0
			_typacnm = _tmpdata.ac_name
		Endif
	Endif
Endif
If _oldconval <= 0
	=sqlconobj1.sqlconnclose("nHandle")
Endif
Release sqlconobj1
Return _typacnm

Procedure findallocservicetype()
_curallocservicetype = ''
If Used('Mall_vw')
	etsql_con	= 1
	etsql_str	= ''
	_oldconval  = 0
	nhandle     = 0
	_etdatasessionid = _Screen.ActiveForm.DataSessionId
	_etactform       = _Screen.ActiveForm
	Set DataSession To _etdatasessionid
	If Type('_etactform.sqlconobj') = 'O'
		sqlconobj1 = _etactform.sqlconobj
	Else
		sqlconobj1 = Newobject('SqlConnUdObj','SqlConnection',xapps)
	Endif
	If Type('_etactform.nhandle') = 'N'
		_oldconval = _etactform.nhandle
	Endif
	If _oldconval > 0
		nhandle = _oldconval
	Endif

	Select mall_vw
	Scan
		etsql_str = "select top 1 SerRule from SerTaxMain_vw where Entry_ty = ?Mall_vw.Entry_all And Tran_cd = ?Mall_vw.Main_Tran"
		etsql_con = sqlconobj1.dataconn([EXE],company.dbname,etsql_str,"_tmpdata","nHandle",_etdatasessionid)
		If etsql_con > 0 And Used('_tmpdata')
			If Reccount('_tmpdata')	> 0
				_curallocservicetype = _tmpdata.serrule
				Exit
			Endif
		Endif
		Select mall_vw
	Endscan

	If _oldconval <= 0
		=sqlconobj1.sqlconnclose("nHandle")
	Endif
	Release sqlconobj1
Endif
Return _curallocservicetype


Procedure findserviceavailtype()
m_serviceavailtype = 'SERVICES'
m_etvalidalias = Alias()
If Used('AcdetAlloc_vw') And Used('CalcTax_vw')
	_etdatasessionid = _Screen.ActiveForm.DataSessionId
	_etactform       = _Screen.ActiveForm
	Set DataSession To _etdatasessionid
	m_servicename = ''
	m_servicename = Substr(calctax_vw.addpostcnd,At(_etactform.splitcharacter,calctax_vw.addpostcnd)+Len(_etactform.splitcharacter)+2)
	m_servicename = Padl(m_servicename,Len(acdetalloc_vw.serty),' ')
	Select acdetalloc_vw
	If Seek(m_servicename,'AcdetAlloc_vw','Serty')
		m_serviceavailtype = seravail
	Endif
Else
	If Type('_mseravail') = 'C'
		m_serviceavailtype = _mseravail
	Endif
Endif
If !Empty(m_etvalidalias)
	Select (m_etvalidalias)
Endif
Return m_serviceavailtype

Procedure sertaxaceffect_s1()
Parameters _fldnm
_fldnm		= Upper(_fldnm)
_vacnm      = ''
_vacacname  = ''
_isadv      = .F.
If Type('_mseravail') = 'C'
	If Inlist(_mseravail,"EXCISE","SERVICE")
		_isadv = .T.
	Endif
Endif
Select acdetalloc_vw
If Seek(item_vw.itserial,'acdetalloc_vw','itserial')
	Do Case
	Case _fldnm	= "SERBAMT" And _isadv = .T.
		_vacnm="Service Tax Adjustment"
	Case _fldnm = "SERBAMT" And _isadv = .F.
		_vacnm="Service Tax Payable"
	Case _fldnm = "SERCAMT" And _isadv = .T.
		_vacnm="Service Tax Cess Adjustment"
	Case _fldnm = "SERCAMT" And _isadv = .F.
		_vacnm="Service Tax Payable-Ecess"
	Case _fldnm = "SERHAMT" And _isadv = .T.
		_vacnm="Service Tax HCess Adjustment"
	Case _fldnm = "SERHAMT" And _isadv = .F.
		_vacnm="Service Tax Payable-Hcess"
&& Added by Shrikant S. on 13/11/2015 for Swachh Bharat Cess 	&& Start
	Case _fldnm = "SERBCESS" And _isadv = .F.
		_vacnm="Service Tax Payable-SBcess"
&& Added by Shrikant S. on 13/11/2015 for Swachh Bharat Cess 	&& End

&& Added by Shrikant S. on 18/05/2016 for Krishi Kalyan Cess 	&& Start
*!*		Case _fldnm = "SKKCAMT" And _isadv = .T.
*!*			_vacnm="Krishi Kalyan Cess Adjustment"

	Case _fldnm = "SKKCAMT" And _isadv = .F.
		_vacnm="Krishi Kalyan Cess Payable"
&& Added by Shrikant S. on 18/05/2016 for Krishi Kalyan Cess 	&& End

	Endcase
Endif
If !Empty(_vacnm)
	_vacacname = EtValidFindAcctName(_vacnm)
Endif
Return _vacacname

Procedure sertaxaceffect_s1_cac()
Parameters _fldnm
_fldnm		= Upper(_fldnm)
_vacnm      = ''
_vacacname  = ''
_isadv      = .F.
If Type('_mseravail') = 'C'
	If Inlist(_mseravail,"EXCISE","SERVICE")
		_isadv = .T.
	Endif
Endif
Select acdetalloc_vw
If Seek(item_vw.itserial,'acdetalloc_vw','itserial')
	Do Case
	Case _fldnm	= "SERBAMT" And _isadv = .T.
		_vacnm="Service Tax Payable"
	Case _fldnm = "SERCAMT" And _isadv = .T.
		_vacnm="Service Tax Payable-Ecess"
	Case _fldnm = "SERHAMT" And _isadv = .T.
		_vacnm="Service Tax Payable-Hcess"
&& Added by Shrikant S. on 13/11/2015 for Swachh Bharat Cess 	&& Start
	Case _fldnm = "SERBCESS" And _isadv = .T.
		_vacnm="Service Tax Payable-SBcess"
&& Added by Shrikant S. on 13/11/2015 for Swachh Bharat Cess 	&& End

&& Added by Shrikant S. on 18/05/2016 for Krishi Kalyan Cess 	&& Start
	Case _fldnm = "SKKCAMT" And _isadv = .T.
		_vacnm="Krishi Kalyan Cess Payable"
&& Added by Shrikant S. on 18/05/2016 for Krishi Kalyan Cess 	&& End

	Endcase
Endif
If !Empty(_vacnm)
	_vacacname = EtValidFindAcctName(_vacnm)
Endif
Return _vacacname


Procedure sertaxaceffect_br()
Parameters _fldnm
_fldnm		= Upper(_fldnm)
_vacnm      = ''
_vacacname  = ''
_isadv      = .F.
If !Empty(coadditional.seraccdt)
	_isadv = Eval(coadditional.seraccdt)
Endif
Do Case
Case main_vw.tdspaytype = 1 And _isadv = .T.
	_vacnm=""
Case _fldnm	= "SERBAMT" And _isadv = .T.
	_vacnm="Service Tax Adjustment"
Case _fldnm = "SERBAMT" And _isadv = .F.
	_vacnm="Output Service Tax"
Case _fldnm = "SERCAMT" And _isadv = .T.
	_vacnm="Service Tax Cess Adjustment"
Case _fldnm = "SERCAMT" And _isadv = .F.
	_vacnm="Output Service Tax-Ecess"
Case _fldnm = "SERHAMT" And _isadv = .T.
	_vacnm="Service Tax HCess Adjustment"
Case _fldnm = "SERHAMT" And _isadv = .F.
	_vacnm="Output Service Tax-Hcess"
*!*	&& Added by Shrikant S. on 18/05/2016 for Krishi Kalyan Cess 	&& Start
*!*	Case _fldnm = "SERHAMT" And _isadv = .T.
*!*		_vacnm="Krishi Kalyan Cess Adjustment"
*!*	Case _fldnm = "SERHAMT" And _isadv = .F.
*!*		_vacnm="Output Krishi Kalyan Cess"
*!*	&& Added by Shrikant S. on 18/05/2016 for Krishi Kalyan Cess 	&& End
Endcase
If !Empty(_vacnm)
	_vacacname = EtValidFindAcctName(_vacnm)
Endif
Return _vacacname


Procedure sertaxaceffect_br_cac()
Parameters _fldnm
_fldnm		= Upper(_fldnm)
_vacnm      = ''
_vacacname  = ''
Do Case
Case _fldnm	= "SERBAMT" And main_vw.tdspaytype = 2
	_vacnm="Service Tax Payable"
Case _fldnm = "SERCAMT" And main_vw.tdspaytype = 2
	_vacnm="Service Tax Payable-Ecess"
Case _fldnm = "SERHAMT" And main_vw.tdspaytype = 2
	_vacnm="Service Tax Payable-Hcess"
&& Added by Shrikant S. on 13/11/2015 for Swachh Bharat Cess 	&& Start
Case _fldnm = "SERBCESS" And main_vw.tdspaytype = 2
	_vacnm="Service Tax Payable-SBcess"
&& Added by Shrikant S. on 13/11/2015 for Swachh Bharat Cess 	&& eND

&& Added by Shrikant S. on 18/05/2016 for Krishi Kalyan Cess 	&& Start
Case _fldnm = "SKKCAMT" And main_vw.tdspaytype = 2
	_vacnm="Krishi Kalyan Cess Payable"
&& Added by Shrikant S. on 18/05/2016 for Krishi Kalyan Cess 	&& End

Endcase
If !Empty(_vacnm)
	_vacacname = EtValidFindAcctName(_vacnm)
Endif
Return _vacacname

Procedure sertaxaceffect_e1()
Parameters _fldnm
_fldnm = Upper(_fldnm)
_vacnm = ''
_isadv = .F.
_vacacname  = ''
_spacct	    = ''
If Type('_mseravail') = 'C'
	If Inlist(_mseravail,"EXCISE","SERVICE")
		_isadv = .T.
	Endi
Endif
Select acdetalloc_vw
If Seek(item_vw.itserial,'acdetalloc_vw','itserial')
	_vservCreditAvail = findallocserviceCreditAvail()	&&Changes has been done by Vasant on 17/02/2014 as per Bug-21539 (No Service Tax Credit for Car and Cleaning Services).
	Do Case
	Case (main_vw.serrule='IMPORT') Or (_isadv = .T. And main_vw.serrule != 'IMPORT')
		Do Case
		Case _fldnm="SERBAMT"
			_vacnm="Input Service Tax Adjustment"
		Case _fldnm="SERCAMT"
			_vacnm="Input Service Tax Cess Adjustment"
		Case _fldnm="SERHAMT"
			_vacnm="Input Service Tax HCess Adjustment"
&& Added by Shrikant S. on 13/11/2015 for Swachh Bharat Cess 	&& Start
		Case _fldnm="SERBCESS"
			_vacnm="Swachh Bharat Cess Adjustment"
&& Added by Shrikant S. on 13/11/2015 for Swachh Bharat Cess 	&& End

&& Added by Shrikant S. on 18/05/2016 for Krish Kalyan Cess 	&& Start
		Case _fldnm="SKKCAMT"
			_vacnm="Krishi Kalyan Cess Adjustment"
&& Added by Shrikant S. on 18/05/2016 for Krish Kalyan Cess 	&& End

		Endcase
	Case _isadv = .F. And main_vw.serrule!='IMPORT'
		If acdetalloc_vw.seravail="SERVICE"
			Do Case
			Case _fldnm="SERBAMT"
				_vacnm="Service Tax Available"
			Case _fldnm="SERCAMT"
				_vacnm="Service Tax Available-Ecess"
			Case _fldnm="SERHAMT"
				_vacnm="Service Tax Available-Hcess"
&& Added by Shrikant S. on 13/11/2015 for Swachh Bharat Cess 	&& Start
			Case _fldnm="SERBCESS"
				_vacnm="Swachh Bharat Cess"
&& Added by Shrikant S. on 13/11/2015 for Swachh Bharat Cess 	&& End

&& Added by Shrikant S. on 18/05/2016 for Krish Kalyan Cess 	&& Start
			Case _fldnm="SKKCAMT"
				_vacnm="Krishi Kalyan Cess Available"
&& Added by Shrikant S. on 18/05/2016 for Krish Kalyan Cess 	&& End
			Endcase
		Endif
		If acdetalloc_vw.seravail="EXCISE"
			Do Case
			Case _fldnm="SERBAMT"
				_spacct="BALANCE WITH SERVICE TAX A/C"
			Case _fldnm="SERCAMT"
				_spacct="BALANCE WITH SERVICE TAX CESS A/C"
			Case _fldnm="SERHAMT"
				_spacct="BALANCE WITH SERVICE TAX HCESS A/C"
&& Added by Shrikant S. on 18/05/2016 for Krish Kalyan Cess 	&& Start
			Case _fldnm="SKKCAMT"
				_spacct="Krishi Kalyan Cess Available"
&& Added by Shrikant S. on 18/05/2016 for Krish Kalyan Cess 	&& eND
			Endcase
		Endif
&&Changes has been done by Vasant on 17/02/2014 as per Bug-21539 (No Service Tax Credit for Car and Cleaning Services).
		If _vservCreditAvail = .F.
			Do Case
			Case _fldnm="SERBAMT"
				_vacnm="Input Service Tax Adjustment"
			Case _fldnm="SERCAMT"
				_vacnm="Input Service Tax Cess Adjustment"
			Case _fldnm="SERHAMT"
				_vacnm="Input Service Tax HCess Adjustment"
&& Added by Shrikant S. on 13/11/2015 for Swachh Bharat Cess 	&& Start
			Case _fldnm="SERBCESS"
				_vacnm="Swachh Bharat Cess Adjustment"
&& Added by Shrikant S. on 13/11/2015 for Swachh Bharat Cess 	&& End

&& Added by Shrikant S. on 18/05/2016 for Krishi Kalyan Cess 	&& Start
			Case _fldnm="SKKCAMT"
				_vacnm="Krishi Kalyan Cess Adjustment"
&& Added by Shrikant S. on 18/05/2016 for Krishi Kalyan Cess 	&& End

			Endcase
		Endif
&&Changes has been done by Vasant on 17/02/2014 as per Bug-21539 (No Service Tax Credit for Car and Cleaning Services).
	Endcase
Endif
If !Empty(_spacct)
	_vacacname = _spacct
Else
	If !Empty(_vacnm)
		_vacacname = EtValidFindAcctName(_vacnm)
	Endif
Endif
Return _vacacname


Procedure sertaxaceffect_e1_cac()
Parameters _fldnm
_fldnm = Upper(_fldnm)
_vacnm = ''
_isadv = .F.
_vacacname  = ''
_spacct	    = ''
If Type('_mseravail') = 'C'
	If Inlist(_mseravail,"EXCISE","SERVICE")
		_isadv = .T.
	Endi
Endif
Select acdetalloc_vw
If Seek(item_vw.itserial,'acdetalloc_vw','itserial')
	Do Case
	Case _isadv = .T.
		If _mseravail="SERVICE"
			Do Case
			Case _fldnm="SERBAMT"
				_vacnm="Service Tax Available"
			Case _fldnm="SERCAMT"
				_vacnm="Service Tax Available-Ecess"
			Case _fldnm="SERHAMT"
				_vacnm="Service Tax Available-Hcess"
&& Added by Shrikant S. on 13/11/2015 for Swachh Bharat Cess		&& Start
			Case _fldnm="SERBCESS"
				_vacnm="Swachh Bharat Cess"
&& Added by Shrikant S. on 13/11/2015 for Swachh Bharat Cess		&& End

&& Added by Shrikant S. on 18/05/2016 for Krishi Kalyan Cess		&& Start
			Case _fldnm="SKKCAMT"
				_vacnm="Krishi Kalyan Cess Available"
&& Added by Shrikant S. on 18/05/2016 for Krishi Kalyan Cess		&& End


			Endcase
		Endif
		If _mseravail="EXCISE"
			Do Case
			Case _fldnm="SERBAMT"
				_spacct="BALANCE WITH SERVICE TAX A/C"
			Case _fldnm="SERCAMT"
				_spacct="BALANCE WITH SERVICE TAX CESS A/C"
			Case _fldnm="SERHAMT"
				_spacct="BALANCE WITH SERVICE TAX HCESS A/C"
&& Added by Shrikant S. on 18/05/2016 for Krishi Kalyan Cess		&& Start
			Case _fldnm="SKKCAMT"
				_spacct="Krishi Kalyan Cess Available"
&& Added by Shrikant S. on 18/05/2016 for Krishi Kalyan Cess		&& End
			Endcase
		Endif

	Case _isadv = .F.
		If main_vw.serrule<>'IMPORT'
			_vacnm=""
		Endif
		If main_vw.serrule='IMPORT'
			Do Case
			Case _fldnm	= "SERBAMT"
				_vacnm="Service Tax Payable"
			Case _fldnm = "SERCAMT"
				_vacnm="Service Tax Payable-Ecess"
			Case _fldnm = "SERHAMT"
				_vacnm="Service Tax Payable-Hcess"
&& Added by Shrikant S. on 13/11/2015 for Swachh Bharat Cess		&& Start
			Case _fldnm = "SERBCESS"
				_vacnm="Swachh Bharat Cess"
&& Added by Shrikant S. on 13/11/2015 for Swachh Bharat Cess		&& End

&& Added by Shrikant S. on 18/05/2016 for Krishi Kalyan Cess		&& Start
			Case _fldnm = "SKKCAMT"
				_vacnm="Krishi Kalyan Cess Payable"
&& Added by Shrikant S. on 18/05/2016 for Krishi Kalyan Cess		&& End
			Endcase
		Endif
	Endcase
Endif
If !Empty(_spacct)
	_vacacname = _spacct
Else
	If !Empty(_vacnm)
		_vacacname = EtValidFindAcctName(_vacnm)
	Endif
Endif
Return _vacacname

Procedure sertaxaceffect_bp()
Parameters _fldnm
_fldnm = Upper(_fldnm)
_vacnm = ''
_isadv = .F.
_vacacname  = ''
_spacct	    = ''
_vseravail = findserviceavailtype()
_vservtype = findallocservicetype()
Do Case
Case _vservtype <> "IMPORT" And main_vw.tdspaytype=1
	_vacnm = ''
*!*		Do Case
*!*		case _fldnm="SERBAMT"
*!*			_vacnm="Service Tax Available"
*!*		case _fldnm="SERCAMT"
*!*			_vacnm="Service Tax Available-Ecess"
*!*		case _fldnm="SERHAMT"
*!*			_vacnm="Service Tax Available-Hcess"
*!*		Endcase
Case (_vservtype = "IMPORT" And main_vw.tdspaytype=1) Or (main_vw.tdspaytype=2)
	If _vseravail="SERVICE"
		Do Case
		Case _fldnm="SERBAMT"
			_vacnm="Service Tax Available"
		Case _fldnm="SERCAMT"
			_vacnm="Service Tax Available-Ecess"
		Case _fldnm="SERHAMT"
			_vacnm="Service Tax Available-Hcess"
&& Added by Shrikant S. on 13/11/2015 for Swachh Bharat Cess 	&& Start
		Case _fldnm="SERBCESS"
			_vacnm="Swachh Bharat Cess"
&& Added by Shrikant S. on 13/11/2015 for Swachh Bharat Cess 	&& End

&& Added by Shrikant S. on 18/05/2016 for Krishi Kalyan Cess 	&& Start
		Case _fldnm="SKKCAMT"
			_vacnm="Krishi Kalyan Cess Available"
&& Added by Shrikant S. on 18/05/2016 for Krishi Kalyan Cess 	&& Start

		Endcase
	Endif
	If _vseravail="EXCISE"
		Do Case
		Case _fldnm="SERBAMT"
			_spacct="BALANCE WITH SERVICE TAX A/C"
		Case _fldnm="SERCAMT"
			_spacct="BALANCE WITH SERVICE TAX CESS A/C"
		Case _fldnm="SERHAMT"
			_spacct="BALANCE WITH SERVICE TAX HCESS A/C"
&& Added by Shrikant S. on 18/05/2016 for Krishi Kalyan Cess 	&& Start
		Case _fldnm="SKKCAMT"
			_spacct="Krishi Kalyan Cess Available"
&& Added by Shrikant S. on 18/05/2016 for Krishi Kalyan Cess 	&& End
		Endcase
	Endif
Endcase
If !Empty(_spacct)
	_vacacname = _spacct
Else
	If !Empty(_vacnm)
		_vacacname = EtValidFindAcctName(_vacnm)
	Endif
Endif

Return _vacacname

Procedure sertaxaceffect_bp_cac()
Parameters _fldnm
_fldnm = Upper(_fldnm)
_vacnm = ''
_isadv = .F.
_vacacname  = ''
_spacct	    = ''
_vseravail = findserviceavailtype()
_vservtype = findallocservicetype()
_vservCreditAvail = findallocserviceCreditAvail()	&&Changes has been done by Vasant on 17/02/2014 as per Bug-21539 (No Service Tax Credit for Car and Cleaning Services).
_isadv      = .F.
If !Empty(coadditional.seraccdt)
	_isadv = Eval(coadditional.seraccdt)
Endif
Do Case
Case _vservtype <> "IMPORT" And main_vw.tdspaytype=1
	_vacnm = ''
*!*		Do Case
*!*		case _fldnm="SERBAMT"
*!*			_vacnm="Service Tax Available"
*!*		case _fldnm="SERCAMT"
*!*			_vacnm="Service Tax Available-Ecess"
*!*		case _fldnm="SERHAMT"
*!*			_vacnm="Service Tax Available-Hcess"
*!*		Endcase
Case (_vservtype = "IMPORT" And main_vw.tdspaytype=1) Or (main_vw.tdspaytype=2)
	If _isadv = .F.
		Do Case
		Case _fldnm="SERBAMT"
			_vacnm="Input Service Tax"
		Case _fldnm="SERCAMT"
			_vacnm="Input Service Tax-Ecess"
		Case _fldnm="SERHAMT"
			_vacnm="Input Service Tax-Hcess"
*!*	&& Added by Shrikant S. on 18/05/2016 for Krishi Kalyan Cess 	&& Start
*!*			Case _fldnm="SKKCAMT"
*!*				_vacnm="Input Krishi Kalyan Cess"
*!*	&& Added by Shrikant S. on 18/05/2016 for Krishi Kalyan Cess 	&& End
		Endcase
	Endif
	If _isadv = .T.
		Do Case
		Case _fldnm="SERBAMT"
			_vacnm="Input Service Tax Adjustment"
		Case _fldnm="SERCAMT"
			_vacnm="Input Service Tax Cess Adjustment"
		Case _fldnm="SERHAMT"
			_vacnm="Input Service Tax HCess Adjustment"
&& Added by Shrikant S. on 13/11/2015 for Swachh Bharat Cess	&& Start
		Case _fldnm="SERBCESS"
			_vacnm="Swachh Bharat Cess Adjustment"
&& Added by Shrikant S. on 13/11/2015 for Swachh Bharat Cess	&& eND

&& Added by Shrikant S. on 18/05/2016 for Krishi Kalyan Cess	&& Start
		Case _fldnm="SKKCAMT"
			_vacnm="Krishi Kalyan Cess Adjustment"
&& Added by Shrikant S. on 18/05/2016 for Krishi Kalyan Cess	&& End
		Endcase
	Endif
&&Changes has been done by Vasant on 17/02/2014 as per Bug-21539 (No Service Tax Credit for Car and Cleaning Services).
	If _vservCreditAvail = .F. And main_vw.tdspaytype=2
		If _vseravail="SERVICE"
			Do Case
			Case _fldnm="SERBAMT"
				_vacnm="Service Tax Available"
			Case _fldnm="SERCAMT"
				_vacnm="Service Tax Available-Ecess"
			Case _fldnm="SERHAMT"
				_vacnm="Service Tax Available-Hcess"
&& Added by Shrikant S. on 13/11/2015 for Swachh Bharat Cess	&& Start
			Case _fldnm="SERBCESS"
				_vacnm="Swachh Bharat Cess"
&& Added by Shrikant S. on 13/11/2015 for Swachh Bharat Cess	&& End

&& Added by Shrikant S. on 18/05/2016 for Krishi Kalyan Cess	&& Start
			Case _fldnm="SKKCAMT"
				_vacnm="Krishi Kalyan Cess Available"
&& Added by Shrikant S. on 18/05/2016 for Krishi Kalyan Cess	&& End
			Endcase
		Endif
		If _vseravail="EXCISE"
			Do Case
			Case _fldnm="SERBAMT"
				_spacct="BALANCE WITH SERVICE TAX A/C"
			Case _fldnm="SERCAMT"
				_spacct="BALANCE WITH SERVICE TAX CESS A/C"
			Case _fldnm="SERHAMT"
				_spacct="BALANCE WITH SERVICE TAX HCESS A/C"
&& Added by Shrikant S. on 18/05/2016 for Krishi Kalyan Cess	&& Start
			Case _fldnm="SKKCAMT"
				_spacct="Krishi Kalyan Cess Available"
&& Added by Shrikant S. on 18/05/2016 for Krishi Kalyan Cess	&& END
			Endcase
		Endif
	Endif
&&Changes has been done by Vasant on 17/02/2014 as per Bug-21539 (No Service Tax Credit for Car and Cleaning Services).
Endcase
If !Empty(_spacct)
	_vacacname = _spacct
Else
	If !Empty(_vacnm)
		_vacacname = EtValidFindAcctName(_vacnm)
	Endif
Endif
Return _vacacname

*!*	Procedure sertaxaceffect_bp_adv()
*!*	Parameters _fldnm
*!*	_fldnm = Upper(_fldnm)
*!*	_vacnm = ''
*!*	_vacacname  = ''
*!*	_vseravail = findserviceavailtype()
*!*	_spacct	    = ''
*!*	If _vseravail="SERVICE"
*!*		Do Case
*!*		case _fldnm="SERRBAMT"
*!*			_vacnm="Service Tax Available"
*!*		case _fldnm="SERRCAMT"
*!*			_vacnm="Service Tax Available-Ecess"
*!*		case _fldnm="SERRHAMT"
*!*			_vacnm="Service Tax Available-Hcess"
*!*		Endcase
*!*	Endif
*!*	If _vseravail="EXCISE"
*!*		Do Case
*!*		case _fldnm="SERRBAMT"
*!*			_spacct="BALANCE WITH SERVICE TAX A/C"
*!*		case _fldnm="SERRCAMT"
*!*			_spacct="BALANCE WITH SERVICE TAX CESS A/C"
*!*		case _fldnm="SERRHAMT"
*!*			_spacct="BALANCE WITH SERVICE TAX HCESS A/C"
*!*		Endcase
*!*	Endif
*!*	IF !EMPTY(_spacct)
*!*		_vacacname = _spacct
*!*	else
*!*		IF !EMPTY(_vacnm)
*!*			_vacacname = EtValidFindAcctName(_vacnm)
*!*		Endif
*!*	ENDIF
*!*	Return _vacacname

Procedure sertaxaceffect_bp_adv()
Parameters _fldnm
_fldnm = Upper(_fldnm)
_vacnm = ''
_vacacname  = ''
Do Case
Case _fldnm="SERRBAMT"
	_vacnm="Input Service Tax Adjustment"
Case _fldnm="SERRCAMT"
	_vacnm="Input Service Tax Cess Adjustment"
Case _fldnm="SERRHAMT"
	_vacnm="Input Service Tax HCess Adjustment"
*!*	&& Added by Shrikant S. on 30/05/2016 for Bug-28132		&& Start
*!*	Case _fldnm="SKKRCAMT"
*!*		_vacnm="Input Krishi Kalyan Cess Adjustment"
*!*	&& Added by Shrikant S. on 30/05/2016 for Bug-28132		&& End
Endcase
If !Empty(_vacnm)
	_vacacname = EtValidFindAcctName(_vacnm)
Endif
Return _vacacname

Procedure sertaxaceffect_bp_adv_cac()
Parameters _fldnm
_fldnm = Upper(_fldnm)
_vacnm = ''
_vacacname  = ''
Do Case
Case _fldnm	= "SERRBAMT"
	_vacnm="Service Tax Payable"
Case _fldnm = "SERRCAMT"
	_vacnm="Service Tax Payable-Ecess"
Case _fldnm = "SERRHAMT"
	_vacnm="Service Tax Payable-Hcess"
&& Added by Shrikant S. on 13/11/2015 for Swachh Bharat Cess		&& Start
Case _fldnm = "SERRBCESS"
	_vacnm="Service Tax Payable-SBcess"
&& Added by Shrikant S. on 13/11/2015 for Swachh Bharat Cess		&& End

&& Added by Shrikant S. on 18/05/2016 for Krishi Kalyan Cess		&& Start
Case _fldnm = "SKKRCAMT"
	_vacnm="Krishi Kalyan Cess Payable"
&& Added by Shrikant S. on 18/05/2016 for Krishi Kalyan Cess		&& End

Endcase
If !Empty(_vacnm)
	_vacacname = EtValidFindAcctName(_vacnm)
Endif
Return _vacacname

Procedure sertaxaceffect_bp_gta_adv()
Parameters _fldnm
_fldnm = Upper(_fldnm)
_vacnm = ''
_vacacname  = ''
_spacct	    = ''
Do Case
Case _fldnm="SERRBAMT"
	_vacnm="Service Tax Adjustment"
Case _fldnm="SERRCAMT"
	_vacnm="Service Tax Cess Adjustment"
Case _fldnm="SERRHAMT"
	_vacnm="Service Tax HCess Adjustment"
&& Added by Shrikant S. on 18/05/2016 for Krishi Kalyan Cess		&& Start
Case _fldnm="SKKRCAMT"
	_vacnm="Krishi Kalyan Cess Adjustment"
&& Added by Shrikant S. on 18/05/2016 for Krishi Kalyan Cess		&& End
Endcase
If !Empty(_spacct)
	_vacacname = _spacct
Else
	If !Empty(_vacnm)
		_vacacname = EtValidFindAcctName(_vacnm)
	Endif
Endif
Return _vacacname

Procedure sertaxaceffect_bp_gta_adv_cac()
Parameters _fldnm
_fldnm = Upper(_fldnm)
_vacnm = ''
_vacacname  = ''
Do Case
Case _fldnm	= "SERRBAMT"
	_vacnm="GTA Service Tax Payable"
Case _fldnm = "SERRCAMT"
	_vacnm="GTA Service Tax Payable-Ecess"
Case _fldnm = "SERRHAMT"
	_vacnm="GTA Service Tax Payable-Hcess"
&& Added by Shrikant S. on 13/11/2015 for Swachh Bharat Cess		&& Start
Case _fldnm = "SERRBCESS"
	_vacnm="GTA Service Tax Payable-SBcess"
&& Added by Shrikant S. on 13/11/2015 for Swachh Bharat Cess		&& End

&& Added by Shrikant S. on 18/05/2016 for Krishi Kalyan Cess		&& Start
Case _fldnm = "SKKRCAMT"
	_vacnm="GTA Krishi Kalyan Cess Payable"
&& Added by Shrikant S. on 18/05/2016 for Krishi Kalyan Cess		&& End


Endcase
If !Empty(_vacnm)
	_vacacname = EtValidFindAcctName(_vacnm)
Endif
Return _vacacname
&&Changes has been done by vasant on 25/02/2012 as per Bug-6092 (Required "Service Tax REVERSE CHARGE MECHANISM" in our Default Products.)

*Birendra : Bug-7528 on 15/03/2013 :Start:
Procedure depre_valid ()
Lparameters Depr_type
Do Case
Case Alltrim(Depr_type)=='STRAIGHT LINE'
	Replace it_mast_vw.deprper With 0,it_mast_vw.estimate With 0,it_mast_vw.salvage With 0,it_mast_vw.usgunit With ''  In it_mast_vw
Case Alltrim(Depr_type)=='WDV'
	Replace it_mast_vw.noofyr With 0,it_mast_vw.estimate With 0,it_mast_vw.salvage With 0,it_mast_vw.usgunit With ''  In it_mast_vw
Case Alltrim(Depr_type)=='BASED ON USAGE'
	Replace it_mast_vw.deprper With 0,it_mast_vw.noofyr With 0   In it_mast_vw
Otherwise
Case Alltrim(Depr_type)==''
	Replace it_mast_vw.deprper With 0,it_mast_vw.estimate With 0,it_mast_vw.salvage With 0,it_mast_vw.usgunit With '',it_mast_vw.noofyr With 0  In it_mast_vw
Endcase
_Screen.ActiveForm.Refresh()
*Birendra : Bug-7528 on 15/03/2013 :End:

&&Changes has been done by Vasant on 04/12/2013 as per Bug-20986 (Issue in BSR Code in ER-1 Report and XML generated from DE Tool).
Procedure GetBSRCode
Lparameters _mParaBankName
_mBankName  = _mParaBankName
_mBSRCode   = ''
etsql_con	= 1
etsql_str	= ''
_oldconval  = 0
nhandle     = 0
_etdatasessionid = _Screen.ActiveForm.DataSessionId
_etactform       = _Screen.ActiveForm
Set DataSession To _etdatasessionid
If Type('_etactform.sqlconobj') = 'O'
	sqlconobj1 = _etactform.sqlconobj
Else
	sqlconobj1 = Newobject('SqlConnUdObj','SqlConnection',xapps)
Endif
If Type('_etactform.nhandle') = 'N'
	_oldconval = _etactform.nhandle
Endif
If _oldconval > 0
	nhandle = _oldconval
Endif
etsql_str = "select top 1 BSRCode from Ac_Mast where AC_name = ?_mBankName"
etsql_con = sqlconobj1.dataconn([EXE],company.dbname,etsql_str,"_tmpdata","nHandle",_etdatasessionid)
If etsql_con > 0 And Used('_tmpdata')
	If Reccount('_tmpdata')	> 0
		_mBSRCode = _tmpdata.BSRCode
	Endif
Endif
_mBSRCode = Iif(Empty(_mBSRCode),coadditional.BCD,_mBSRCode)
If _oldconval <= 0
	=sqlconobj1.sqlconnclose("nHandle")
Endif
Release sqlconobj1
Return _mBSRCode
&&Changes has been done by Vasant on 04/12/2013 as per Bug-20986 (Issue in BSR Code in ER-1 Report and XML generated from DE Tool).

&&Changes has been done by Vasant on 17/02/2014 as per Bug-21539 (No Service Tax Credit for Car and Cleaning Services).
Procedure findallocserviceCreditAvail()
_curallocserviceCreditAvail = .T.
If Used('acdetalloc_vw')
	etsql_con	= 1
	etsql_str	= ''
	_oldconval  = 0
	nhandle     = 0
	_etdatasessionid = _Screen.ActiveForm.DataSessionId
	_etactform       = _Screen.ActiveForm
	Set DataSession To _etdatasessionid
	If Type('_etactform.sqlconobj') = 'O'
		sqlconobj1 = _etactform.sqlconobj
	Else
		sqlconobj1 = Newobject('SqlConnUdObj','SqlConnection',xapps)
	Endif
	If Type('_etactform.nhandle') = 'N'
		_oldconval = _etactform.nhandle
	Endif
	If _oldconval > 0
		nhandle = _oldconval
	Endif

	etsql_str = "select top 1 CreditAvail from sertax_mast where Name = ?acdetalloc_vw.Serty And ?Main_vw.Date Between Sdate And Edate"
	etsql_con = sqlconobj1.dataconn([EXE],company.dbname,etsql_str,"_tmpdata","nHandle",_etdatasessionid)
	If etsql_con > 0 And Used('_tmpdata')
		If Reccount('_tmpdata')	> 0
			_curallocserviceCreditAvail = _tmpdata.CreditAvail
		Endif
	Endif

	If _oldconval <= 0
		=sqlconobj1.sqlconnclose("nHandle")
	Endif
	Release sqlconobj1
Endif
Return _curallocserviceCreditAvail
&&Changes has been done by Vasant on 17/02/2014 as per Bug-21539 (No Service Tax Credit for Car and Cleaning Services).

&& Added by Pankaj B. on 28-06-2014 For checking if Rg Page No. is generated or not start
Procedure chk_ispagegen()
_pickupdonetrantype='*'
If Used("itref_vw")
	Select itref_vw
	If Seek(item_vw.entry_ty+Str(item_vw.tran_cd)+item_vw.itserial,"itref_vw",'etits')
		_pickupdonetrantype=itref_vw.rentry_ty
	Endif
Endif
Return _pickupdonetrantype
&& Added by Pankaj B. on 28-06-2014 For checking if Rg Page No. is generated or not End

&&added by Pankaj B. on 30-09-2014 For Bug-22613 Start
Procedure Chk_Sugam_No()
If Inlist(main_vw.entry_ty,"ST","PT")
	_malias=Alias()
	nhandle     = 0
	_etdatasessionid = _Screen.ActiveForm.DataSessionId
	Set DataSession To _etdatasessionid
	sqlconobj = Newobject('SqlConnUdObj','SqlConnection',xapps)
	etsql_con	= 1

	If main_vw.entry_ty = "ST"
		etsql_str = "Select Top 1 u_esugamno from Stmain where u_esugamno = '" + Iif(Type("main_vw.u_esugamno")<>'U',main_vw.u_esugamno,lmc_vw.u_esugamno) + "' and u_esugamno<>''"
	Else
		If main_vw.entry_ty = "PT"
			etsql_str = "Select Top 1 u_esugamno from Ptmain where u_esugamno = '" + Iif(Type("main_vw.u_esugamno")<>'U',main_vw.u_esugamno,lmc_vw.u_esugamno) + "' and u_esugamno<>''"
		Endif
	Endif

	etsql_con = sqlconobj.dataconn([EXE],company.dbname,etsql_str,[curmain_vw],"nHandle",_etdatasessionid,.F.)

	Select curmain_vw
	If Reccount("curmain_vw") > 0
		Messagebox("Duplicate Sugam No. found!!"+Chr(13)+"Please enter different Sugam No.",16,vumess)
		Return .F.
	Else
		Wait Window "Sugam No. validated successfully!!" Nowait
	Endif

	If !Used("curmain_vw")
		Messagebox("Failed to Connect database, Validation failed",16,vumess)
		etsql_con = 0
	Else
		If !Empty(_malias)
			Select &_malias
		Endif
		Use In curmain_vw
	Endif
	=sqlconobj.sqlconnclose("nHandle")
	Return .T.
Endif
Endproc
&&added by Pankaj B. on 30-09-2014 For Bug-22613 End

&& Added by Shrikant S. on 12/01/2014 for Bug-25081		&& Start
Procedure isdcpicked
retval=.T.
If [vutex] $ vchkprod And main_vw.entry_ty='ST'
	If Used('itref_vw')
		Select itref_vw
		mrecno=Iif(!Eof(),Recno(),0)
		Select itref_vw
		Scan
			If itref_vw.rqty>0
				retval=.F.
				Exit
			Endif
		Endscan
		If mrecno>0
			Select itref_vw
			Go mrecno
		Endif
	Endif
Endif
Return retval
&& Added by Shrikant S. on 12/01/2014 for Bug-25081		&& End

&& Added by Shrikant S. on 27/06/2014 for Bug-23280		&& Start

Procedure ConvertQty
If item_vw.Itemunit <> item_vw.convunit
	If item_vw.qty1 <>0
		sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)
		nhandle=0

		msqlstr="select convRatio from it_mast WHERE convRatio<>0  and It_code=?item_vw.it_code"
		nretval = sqlconobj.dataconn([EXE],company.dbname,msqlstr,"_item","nHandle",_Screen.ActiveForm.DataSessionId)
		If nretval<0
			Return .F.
		Endif
		If _item.convRatio>0
			If Inlist(Upper(Alltrim(item_vw.convunit)),'KG','KGS')
				Replace qty With qty1/_item.convRatio In item_vw
			Else
				Replace qty With qty1*_item.convRatio In item_vw
			Endif

		Endif
		=sqlconobj.sqlconnclose("nHandle")
		If Used("_item")
			Use In _item
		Endif
	Endif
Else
	Replace qty With Iif(qty=0,qty1,qty),rate With Iif(rate=0,convRate,rate) In item_vw
Endif
Return 0

Procedure ConvertRate
If item_vw.Itemunit <> item_vw.convunit
	Replace u_asseamt With qty1 * convRate In item_vw
	Replace rate With u_asseamt/qty In item_vw
	oform=_Screen.ActiveForm
	oform.itemgrdbefcalc(1)
Endif
Return .T.

Procedure UpdateInventory
Replace invmethod With Iif(Empty(lmc_vw.invmethod),"FIFO",lmc_vw.invmethod) In lmc_vw
Return .T.

Procedure CalcLBT
If item_vw.LBTPER >0
	lnlbtamt= (item_vw.gro_amt *  item_vw.LBTPER)/100

	If item_vw.lbtamt> 0 And lnlbtamt<>item_vw.lbtamt
		Replace LBTPER With 0 In item_vw
		Return .T.
	Endif
	Replace lbtamt With lnlbtamt In item_vw
Endif
Return .T.

Procedure GenBatchNo
Parameters otxt

&& Added By Kishor A. for Bug-28461 on 07-09-2016 Start
If otxt.Name = "FRMXTRA"
	_curobject=otxt.fobject
Else
	_curobject = otxt
Endif
&& Added By Kishor A. for Bug-28461 on 07-09-2016 End

lcbatchno=item_vw.batchno
If coadditional.autobatch

	If Empty(item_vw.batchno)

		_malias 	= Alias()
		Sele item_vw
		_mrecno 	= Iif(!Eof(),Recno(),0)
		etsql_con	= 1
		nhandle     = 0
		_etdatasessionid = _Screen.ActiveForm.DataSessionId
		Set DataSession To _etdatasessionid

		sqlconobj = Newobject('SqlConnUdObj','SqlConnection',xapps)

		_mrunno=0
		msqlstr="Select BatchPFix,BatchSFix,BatchCode,BatchGen,BatMFormat,BatRunning as IsRunning from it_mast WHERE It_code=?item_vw.it_code"
		nretval = sqlconobj.dataconn([EXE],company.dbname,msqlstr,"_item","nHandle",_Screen.ActiveForm.DataSessionId)
		If nretval<0
			Return .F.
		Endif
		lcformat=""
		Do Case
		Case Empty(_item.BatchGen)
			lcformat=""
		Case Alltrim(_item.BatchGen)='Datewise'
			lcformat   = Dtos(main_vw.Date)
			lcformat   = Subs(lcformat,7,2)+Subs(lcformat,5,2)+Subs(lcformat,3,2)
		Case Alltrim(_item.BatchGen)='Monthwise'
*!*				lcformat   = Alltrim(_curobject.genMnthWiseFormat(main_vw.Date,Alltrim(_item.BatMFormat)))	&&Commented By Kishor A. for Bug-28461 on 07-09-2016
			lcformat   = Alltrim(BatchGenMnthWiseFormat(main_vw.Date,Alltrim(_item.BatMFormat),_curobject)) &&Added By Kishor A. for Bug-28461 on 07-09-2016
		Endcase

		If !Used('BatchTbl_Vw')
			etsql_str = "Select * From BatchGenTbl where 1=0"
			etsql_con = sqlconobj.dataconn([EXE],company.dbname,etsql_str,[BatchTbl_Vw],"nHandle",_etdatasessionid,.F.)
			If etsql_con < 1 Or !Used("BatchTbl_Vw")
				etsql_con = 0
			Else
				Select BatchTbl_Vw
				Index On itserial Tag itserial
			Endif
		Endif
		If etsql_con > 0 And _item.IsRunning=.T.
			_mcond ="l_yn='"+Alltrim(main_vw.l_yn)+"' "

&& Added By Kishor A. For bug-28461 on 06-09-2016 Start
			If Type('coadditional.BatchRun') # 'U'
				If coadditional.BatchItem
					_mcond ="l_yn='"+Alltrim(main_vw.l_yn)+"' "
				Endif
			Endif
			If Type('coadditional.BatchItem') # 'U' And Type('_item.Batrunning')
				If coadditional.BatchItem And _item.Batrunning
					_mcond ="l_yn='"+Alltrim(main_vw.l_yn)+"' and it_code="+Alltrim(Str(item_vw.it_code))
				Endif
			Endif
&& Added By Kishor A. For bug-28461 on 06-09-2016 End

			Select Max(Cast(runno As Int)) As runno From BatchTbl_Vw With (Buffering = .T.) ;
				where &_mcond Into Cursor tmprunno_vw
			_mrunno1=0
			If Used('tmprunno_vw')
				If Reccount('tmprunno_vw') > 0
					_mrunno1=Iif(Isnull(tmprunno_vw.runno ),0,tmprunno_vw.runno )
					If _mrunno1 !=0
						_mrunno= _mrunno1 + 1
					Endif
				Endif
			Endif

		Endif
		If _item.IsRunning=.T.
			_mrunno1 =0
			etsql_str = "Select Max(Cast(Runno as int)) as Runno  From BatchGenTbl where "+_mcond
			etsql_con = sqlconobj.dataconn([EXE],company.dbname,etsql_str,[tmprunno_vw],"nHandle",_etdatasessionid,.F.)
			If etsql_con > 0 And Used("tmprunno_vw")
				_mrunno1 = Iif(Isnull(tmprunno_vw.runno),0,tmprunno_vw.runno) + 1
			Endif
			_mrunno=Iif(_mrunno1 >_mrunno,_mrunno1,_mrunno)
		Endif

		If etsql_con <= 0
			_mrunno= '***'
		Endif
		If Used("tmprunno_vw")
			Use In tmprunno_vw
		Endif
		=sqlconobj.sqlconnclose("nHandle")
		Sele item_vw
		If Betw(_mrecno,1,Reccount())
			Go _mrecno
		Endif

*		_mrgpage = Padr(_mrunno,Len(item_vw.batchno))
		If Used('BatchTbl_Vw') And etsql_con > 0
			Select BatchTbl_Vw
			Locate For itserial = item_vw.itserial
			If !Found()
				Append Blank In BatchTbl_Vw
			Endif


			lcbatchno=Iif(!Empty(Alltrim(_item.BatchPFix)),Iif(Inlist(Left(Alltrim(_item.BatchPFix),1),"'",'"'),Evaluate(Alltrim(_item.BatchPFix)),Evaluate("'"+Alltrim(_item.BatchPFix)+"'")),'')
			lcbatchno=lcbatchno+Iif(Alltrim(lcbatchno)=="",Iif(!Empty(_item.BatchCode),Evaluate(Alltrim(_item.BatchCode)),''),Iif(!Empty(_item.BatchCode),'-'+Evaluate(Alltrim(_item.BatchCode)),''))
			lcbatchno=lcbatchno+Iif(Alltrim(lcbatchno)=="",Iif(!Empty(lcformat),Alltrim(lcformat),''),Iif(!Empty(lcformat),'-'+Alltrim(lcformat),''))
			lcbatchno=lcbatchno+Iif(Alltrim(lcbatchno)=="",Iif(_mrunno<>0,Padl(Alltrim(Str(_mrunno)),6,'0'),''),Iif(_mrunno<>0,'-'+Padl(Alltrim(Str(_mrunno)),6,'0'),''))
			lcfix=Iif(!Empty(Alltrim(_item.BatchSfix)),Iif(Inlist(Left(Alltrim(_item.BatchSfix),1),"'",'"'),Evaluate(Alltrim(_item.BatchSfix)),Evaluate("'"+Alltrim(_item.BatchSfix)+"'")),'')
			lcbatchno=lcbatchno+Iif(Alltrim(lcbatchno)=="",Iif(!Empty(lcfix),lcfix,''),Iif(!Empty(lcfix),'-'+lcfix,''))

&& Added By Kishor A. For bug-28461 on 06-09-2016 Start
			If Type('coadditional.BatchRun') # 'U'
				If coadditional.BtchProcRunning
					Replace runno With Padl(Alltrim(Str(_mrunno)),6,'0'),batchno With lcbatchno,itserial With item_vw.itserial,l_yn With main_vw.l_yn In BatchTbl_Vw
				Endif
			Endif
			If Type('coadditional.BatchItem') # 'U'
				If coadditional.BtchProcItemWise
					Replace runno With Padl(Alltrim(Str(_mrunno)),6,'0'),batchno With lcbatchno,itserial With item_vw.itserial,l_yn With main_vw.l_yn,it_code With item_vw.it_code In BatchTbl_Vw
				Endif
			Endif
&& Added By Kishor A. For bug-28461 on 06-09-2016 End

*!*				Replace runno With Padl(Alltrim(Str(_mrunno)),6,'0'),batchno With lcbatchno,itserial With item_vw.itserial,l_yn With main_vw.l_yn In BatchTbl_Vw		&& Commented By Kishor A. For bug-28461 on 06-09-2016

		Endif

		If !Empty(_malias)
			Select &_malias
		Endif
	Else
		lcbatchno=item_vw.batchno
		If Type('otxt')='O'
			otxt.Tag=item_vw.batchno
		Endif
	Endif

Endif
Return lcbatchno

Procedure CheckBatchNo
Parameters mcommit,nhand,otxt
_mrgret  = 0

&& Added By Kishor A. for Bug-28461 on 07-09-2016 Start
If otxt.BaseClass # "Form"
	_curvouobj = otxt.Parent.fobject
Else
	_curvouobj = otxt
Endif
&& Added By Kishor A. for Bug-28461 on 07-09-2016 End

_curvouobj = _Screen.ActiveForm
If Type('_curvouobj.mainalias') = 'C'
	If Upper(_curvouobj.mainalias) <> 'MAIN_VW'
		Return
	Endif
Endif

If Used('BatchTbl_Vw')
	Select BatchTbl_Vw
	mrec=Recno()
Endif

If !Empty(item_vw.batchno)
	_malias 	= Alias()
	Sele item_vw
	_mrecno 	= Recno()
	etsql_con	= 1
	nhandle     = 0
	_etdatasessionid = _Screen.ActiveForm.DataSessionId
	Set DataSession To _etdatasessionid
	sqlconobj = Newobject('SqlConnUdObj','SqlConnection',xapps)

&& Added by Shrikant S. on 19/05/2015 for Bug-26131			&& Start
	llbatchno=.F.
	msqlstr="select batchvalid from it_mast where it_code="+Alltrim(Str(item_vw.it_code))
	nretval = sqlconobj.dataconn("EXE",company.dbname,msqlstr,"TmpITMast","nhandle",_Screen.ActiveForm.DataSessionId)
	If Used('TmpITMast')
		If TmpITMast.batchvalid=.T.
			llbatchno=.T.
		Else
			llbatchno=.F.
		Endif
		Use In TmpITMast
	Endif
&& Added by Shrikant S. on 19/05/2015 for Bug-26131			&& End


	If Inlist(main_vw.entry_ty,"OP")
		If Used('PROJECTITREF_VW')
			Select projectitref_vw
			Locate For entry_ty=main_vw.entry_ty And tran_cd=main_vw.tran_cd And itserial=item_vw.itserial
			If Found()
				If item_vw.batchno<>projectitref_vw.batchno
					Messagebox("Batch no. can't be changed as it is picked against work order.",0,vumess)
					Replace batchno With projectitref_vw.batchno In item_vw
				Endif

				Return .T.
			Endif
		Endif
	Endif
	msqlstr="Select BatchPFix,BatchSFix,BatchCode,BatchGen,BatMFormat,BatRunning as IsRunning from it_mast WHERE It_code=?item_vw.it_code"
	nretval = sqlconobj.dataconn([EXE],company.dbname,msqlstr,"_item","nHandle",_Screen.ActiveForm.DataSessionId)
	If nretval<0
		Return .F.
	Endif
	lcformat=""
	Do Case
	Case Empty(_item.BatchGen)
		lcformat=""
	Case Alltrim(_item.BatchGen)='Datewise'
		lcformat   = Dtos(main_vw.Date)
		lcformat   = Subs(lcformat,7,2)+Subs(lcformat,5,2)+Subs(lcformat,3,2)
	Case Alltrim(_item.BatchGen)='Monthwise'
		lcformat   = Alltrim(BatchGenMnthWiseFormat(main_vw.Date,Alltrim(_item.BatMFormat),_curvouobj)) &&Added By Kishor A. for Bug-28461 on 07-09-2016
*!*			lcformat   = Alltrim(_curobject.genMnthWiseFormat(main_vw.Date,Alltrim(_item.BatMFormat)))		&& Commented By Kishor A. for Bug-28461 on 07-09-2016

	Endcase

	If _item.IsRunning=.T.
		If Alltrim(_item.BatchSfix)<>""
			_mrunno=Val(Left(Right(Alltrim(item_vw.batchno),6+Len(Alltrim(BatchSfix))),6))
		Else
			_mrunno=Val(Right(Alltrim(item_vw.batchno),6))
		Endif
	Else
		_mrunno=0
	Endif

	If !Used('BatchTbl_Vw')
		etsql_str = "Select * From BatchGenTbl where 1=0"
		etsql_con = sqlconobj.dataconn([EXE],company.dbname,etsql_str,[BatchTbl_Vw],"nHandle",_etdatasessionid,.F.)
		If etsql_con < 1 Or !Used("BatchTbl_Vw")
			etsql_con = 0
		Else
			Select BatchTbl_Vw
			Index On itserial Tag itserial
		Endif
	Endif

	If etsql_con > 0
		_mcond = "l_yn='"+Alltrim(main_vw.l_yn)+"'"
		_mcond =_mcond +" and Batchno=Item_vw.batchno"
		Select BatchTbl_Vw
		Scan
			If &_mcond
				If BatchTbl_Vw.itserial != item_vw.itserial And Val(BatchTbl_Vw.runno) == _mrunno
					_mrgret  = 1
				Endif
			Endif
		Endscan
		If _mrgret != 1
			_mcond = "l_yn='"+Alltrim(main_vw.l_yn)+"'"
			_mcond = _mcond + " And  BatchNo=?Item_vw.BatchNo"

			etsql_str = "Select Top 1 Entry_ty,Tran_cd,Itserial From BatchGenTbl where "+_mcond
			etsql_con = sqlconobj.dataconn([EXE],company.dbname,etsql_str,[tmprunno_vw],Iif(mcommit=.F.,"nHandle",nhand),_etdatasessionid,mcommit)

			If etsql_con > 0 And Used("tmprunno_vw")
				Select tmprunno_vw
				If Reccount() > 0 And entry_ty+Str(tran_cd) # main_vw.entry_ty+Str(main_vw.tran_cd)
					_mrgret  = 1
				Else
					Select BatchTbl_Vw
					Locate For itserial = item_vw.itserial
					If !Found()
						Append Blank In BatchTbl_Vw
					Endif
					If Empty(item_vw.batchno)
						lcbatchno=Iif(!Empty(Alltrim(_item.BatchPFix)),Iif(Inlist(Left(Alltrim(_item.BatchPFix),1),"'",'"'),Evaluate(Alltrim(_item.BatchPFix)),Evaluate("'"+Alltrim(_item.BatchPFix)+"'")),'')
						lcbatchno=lcbatchno+Iif(Alltrim(lcbatchno)=="",Iif(!Empty(_item.BatchCode),Evaluate(Alltrim(_item.BatchCode)),''),Iif(!Empty(_item.BatchCode),'-'+Evaluate(Alltrim(_item.BatchCode)),''))
						lcbatchno=lcbatchno+Iif(Alltrim(lcbatchno)=="",Iif(!Empty(lcformat),Alltrim(lcformat),''),Iif(!Empty(lcformat),'-'+Alltrim(lcformat),''))
						lcbatchno=lcbatchno+Iif(Alltrim(lcbatchno)=="",Iif(_mrunno<>0,Padl(Alltrim(Str(_mrunno)),6,'0'),''),Iif(_mrunno<>0,'-'+Padl(Alltrim(Str(_mrunno)),6,'0'),''))

						lcfix=Iif(!Empty(Alltrim(_item.BatchSfix)),Iif(Inlist(Left(Alltrim(_item.BatchSfix),1),"'",'"'),Evaluate(Alltrim(_item.BatchSfix)),Evaluate("'"+Alltrim(_item.BatchSfix)+"'")),'')
						lcbatchno=lcbatchno+Iif(Alltrim(lcbatchno)=="",Iif(!Empty(lcfix),lcfix,''),Iif(!Empty(lcfix),'-'+lcfix,''))
					Else
						lcbatchno=item_vw.batchno
					Endif
					Replace runno With Padl(Alltrim(Str(_mrunno)),6,'0'),batchno With lcbatchno,itserial With item_vw.itserial,l_yn With main_vw.l_yn In BatchTbl_Vw

					_mrgret  = 0
				Endif
			Endif
		Endif
	Endif
	If Used("tmprunno_vw")
		Use In tmprunno_vw
	Endif
	=sqlconobj.sqlconnclose("nHandle")
	Sele item_vw
	If Betw(_mrecno,1,Reccount())
		Go _mrecno
	Endif
	If !Empty(_malias)
		Select &_malias
	Endif
Else
	Select item_vw
	_mrecno 	= Iif(!Eof(),Recno(),0)
	mitserial=item_vw.itserial
	If Used('BatchTbl_Vw')
		Select BatchTbl_Vw
		Locate For itserial=mitserial
		If Found()
			Delete In BatchTbl_Vw
		Endif
	Endif
	Select item_vw
	If _mrecno >0
		Go _mrecno
	Endif

Endif
retval=Iif(_mrgret = 0,.T.,.F.)
If !retval And Type('otxt')='O'
&&	 otxt.Value=otxt.Tag      commented by nilesh for bug 25365 on 10/03/2015
	otxt.SetFocus
Endif
Return retval

&&Added By Kishor A. for Bug-28461 on 07-09-2016 Start
Procedure BatchGenMnthWiseFormat
Lparameters dDate,cMnthFormat,lcCurObject
Local cDate,cRetVal
cDate = Dtos(dDate)
nnhandle=0
cMthFormat = Alltrim(cMnthFormat)
mRet=lcCurObject.sqlconobj.dataconn("EXE",company.dbname,"select * from MonthFormat where MnthFrmt = ?cMthFormat ","_MnthFrmt","nnhandle",lcCurObject.DataSessionId)
If mRet > 0
	mRet=lcCurObject.sqlconobj.sqlconnclose("nnhandle")
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
Endproc

&&Added By Kishor A. for Bug-28461 on 07-09-201 End

&&Added By Kishor A. for Bug-28461 on 07-09-2016 Start
Procedure ItMastBatchNoAutoSel
Parameters otxt

If otxt.BaseClass # "Form"
	_curvouobj = otxt.Parent.fobject
Else
	_curvouobj = otxt
Endif

nnhandle=0
mRet=_curvouobj.sqlconobj.dataconn("EXE",company.dbname,"select BATRUNNING from IT_MAST where it_code = ?item_vw.it_code","_Item","nnhandle",_curvouobj.DataSessionId)
=_curvouobj.sqlconobj.sqlconnclose("nHandle")

If Type('_item.Batrunning') # 'U'
	If _item.Batrunning
		Return .T.
	Endif
Endif

Return .F.
Endproc

&&Added By Kishor A. for Bug-28461 on 07-09-201 End

Procedure GetBomDetails
_curvouobj = _Screen.ActiveForm
If Type('_curvouobj.pcvtype')='C'
	If Inlist(_curvouobj.pcvtype,'WK')
		_malias 	= Alias()
		_mrecno	= Iif(!Eof(),Recno(),0)
		nhandle     = 0

		If !Empty(lmc_vw.FGSFGCODE)
			etsql_str = "Select Count(A.[BomId]) AS Bomcnt "
			etsql_str = etsql_str + " From BOMHEAD A, BOMDET B, IT_MAST C "
			etsql_str = etsql_str + " Where A.BOMID = B.BOMID AND A.ITEM = ?lmc_vw.FGSFGCODE "
			etsql_str = etsql_str + " And B.RMITEMID = C.IT_CODE AND C.[TYPE] in ('Raw Material','Semi Finished') "
			etsql_str = etsql_str  + " And a.bomclose = 0 "
			etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[tmpbom],"_curvouobj.nHandle",_curvouobj.DataSessionId,.F.)
			If etsql_con <=0
				Return .F.
			Endif

			lnBomcnt=tmpbom.bomcnt
			Do Case
			Case lnBomcnt=0
				Messagebox("Bill of Material not Found. Cannot Go Further.",0,vumess)
				Replace FGSFGCODE With "",fgsfgname With "",bomno With "",batchno With "" In lmc_vw
				Return .F.
			Case lnBomcnt>0
				etsql_str ="Select Distinct a.BomId From BomHead a Inner Join Bomdet b ON(a.BomId=b.Bomid) "
				etsql_str =etsql_str +" Inner Join It_mast c ON(b.rmitemId=c.it_code)"
				etsql_str =etsql_str +" Where a.Item=?lmc_vw.fgsfgcode and c.[type] IN ('Raw Material','Semi Finished') "
				etsql_str =etsql_str +" And a.bomclose = 0 "
				etsql_str =etsql_str +" Order by a.BomId"
				etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[tmpbom],"_curvouobj.nHandle",_curvouobj.DataSessionId,.F.)
				If etsql_con <=0
					Return .F.
				Endif
				If Reccount('tmpbom')=1
					Replace bomno With tmpbom.bomid In lmc_vw
					frmxtra.txtbomno.Refresh
				Else
					lcBomId = ""

					lcBomId=Uegetpop('tmpbom','Select BOM','BomID','BomID',"",'','','',.F.,[],[],[BomID:BOM ID])
					lcBomId=Iif(Type('lcBomId')='L',"",lcBomId)
					Replace bomno With lcBomId In lmc_vw
				Endif
			Endcase

			etsql_str = "Select it_alias from It_Mast Where It_name = ?lmc_vw.fgsfgcode"
			etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[tmpcode],"_curvouobj.nHandle",_curvouobj.DataSessionId,.F.)
			If etsql_con <=0
				Return .F.
			Endif
			Select tmpcode
			Replace fgsfgname With tmpcode.it_alias In lmc_vw
			frmxtra.txtfgsfgname.Refresh

			If Used('tmpbom')
				Use In tmpbom
			Endif
			If Used('tmpcode')
				Use In tmpcode
			Endif
			If !Empty(_malias)
				Select &_malias
			Endif
			If _mrecno >0
				Go _mrecno
			Endif
		Endif

	Endif
Endif

Return .T.


Procedure Get_Bomdetails
_malias 	= Alias()
_mrecno=Iif(!Eof(),Recno(),0)

_fgsfgitem=Alltrim(lmc_vw.FGSFGCODE)
_fgbomqty=lmc_vw.batchsize

srno = 0
tsrno = 0
_rmlist=""
nhandle = 0
Select item_vw
Delete All

_etdatasessionid = _Screen.ActiveForm.DataSessionId
Set DataSession To _etdatasessionid

sqlconobj = Newobject('SqlConnUdObj','SqlConnection',xapps)

etsql_str = " Select A.FGQty , B.RMItemID, B.RMItem, B.RMQty, C.[TYPE] AS ITTYPE "
etsql_str = etsql_str + " From BomHead A, BomDet B, IT_MAST C "
etsql_str = etsql_str + " Where A.BomID = B.BomID AND A.ITEM = ?lmc_vw. AND A.BomID = ?lmc_vw.bomno "
etsql_str = etsql_str + " AND B.RMItemID = C.IT_CODE "
etsql_str = etsql_str + " ORDER BY C.[TYPE] desc"

etsql_con = sqlconobj.dataconn([EXE],company.dbname,etsql_str,[_BOMDET],"nHandle",_etdatasessionid,.F.)

Select rmitem,rmqty From _BOMDET Where 1 = 2 Into Cursor STK_NOT_EXIST Readwrite

Select _BOMDET
Scan
	_rmitem=_BOMDET.rmitem
	_rmqty=_BOMDET.rmqty
	_fgqty=_BOMDET.fgqty
	_reqrmqty=(_fgbomqty * _rmqty)/_fgqty

	etsql_str = " Select it_code,item,batchno,SUM(Case When Pmkey='+' Then QCaceptQty Else -QCaceptQty End) AS RELQTY,EXPDT"
	etsql_str = etsql_str + " From ITIS_STK_STATUS_VW"
	etsql_str = etsql_str + " Where DATE <= ?main_vw.Date AND it_code = ?_BOMDET.RMItemID"
	If lmc_vw.OrderType  = "Repacking" And Alltrim(_BOMDET.ItType) ="Raw Material"
		etsql_str = etsql_str + " And Batchno = ?_BOMDET.batchno "
	Endif
	etsql_str = etsql_str + " Group by item,batchno,it_code,EXPDT "
	etsql_str = etsql_str + " Having SUM(Case When Pmkey='+' Then U_relqty Else -U_relqty  End) > 0  "
	etsql_str = etsql_str + " Order By ExpDt"

	etsql_con = sqlconobj.dataconn([EXE],company.dbname,etsql_str ,[_BOMDET1],"nHandle",_etdatasessionid,.F.)
	If etsql_con<=0
		Return .F.
	Endif

	entqty = 0
	Select _BOMDET1
	Scan
		If _reqrmqty >0
			If _reqrmqty <= _BOMDET1.RELQTY   		&&	BATCH QTY ONLY TAKEN FROM ONE BATCH ONLY WHEN ENTRY TYPE IS REPACKING
				entqty = _reqrmqty
				trmbatchqty=0
			Else
				entqty = _BOMDET1.RELQTY
				_reqrmqty =_reqrmqty - _BOMDET1.RELQTY
			Endif

			Select item_vw
			Append Blank

			tsrno = tsrno+1
			Replace item_no With Str(tsrno,5),itserial With Padl(Allt(Str(tsrno)),5,"0"),Item With _BOMDET1.Item;
				,qty With entqty,U_relqty With qty,batchno With _BOMDET1.batchno,expdt With _BOMDET1.expdt;
				,entry_ty With main_vw.entry_ty,Date With main_vw.Date,Doc_no With main_vw.Doc_no,it_code With _BOMDET1.it_code;
				In item_vw

		Endif
	Endscan
	If _reqrmqty >0
		Select STK_NOT_EXIST
		Append Blank
		Replace rmitem With _BOMDET.rmitem
		Replace rmqty With _reqrmqty
		_rmlist=_rmlist+_BOMDET.rmitem+","
	Endif

Endscan

If Len(Alltrim(_rmlist))>0
	Replace lmc_vw.bomno With "" In lmc_vw
	Select item_vw
	Delete All

	_rmlist=Left(Alltrim(_rmlist),Len(Alltrim(_rmlist))-1)
	Messagebox("Stock Not Available for the items "+_rmlist+" as per Bill Of Material.",0,vumess)
	Return .F.
Endif

*!*	Select item_vw
*!*	Locate
If !Empty(_malias)
	Select &_malias
	If _mrecno >0
		Go _mrecno
	Endif
Endif
Return .T.

Procedure GetWorkOrderDetail
_curvouobj = _Screen.ActiveForm

If Type('_curvouobj.PCVTYPE')='C'
	If Inlist(_curvouobj.pcvtype,'IP')
		_malias=Alias()
		_mrecno=Iif(!Eof(),Recno(),0)


		If Empty(lmc_vw.ALLOCID)
			Return .T.
		Endif
		etsql_str1 = " SELECT i.item,i.inv_no,i.batchno,i.tran_cd,i.bomid,i.batchsize,it.it_alias FROM item i Inner Join it_mast it ON(i.it_code=it.it_code) WHERE i.ENTRY_TY = 'WK' AND i.TRAN_CD = '"+Alltrim(lmc_vw.ALLOCID) + "'"
		etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str1,[_wkdetail],"_curvouobj.nHandle",_curvouobj.DataSessionId,.F.)
		If etsql_con <= 0
			Return .F.
		Endif

		Select _wkdetail
		Replace FGSFGCODE With _wkdetail.Item,fgsfgname With _wkdetail.it_alias,bomid With _wkdetail.bomid;
			,allocno With _wkdetail.inv_no,batchsize With _wkdetail.batchsize,fgbatchno With _wkdetail.batchno,batchsize With _wkdetail.batchsize In lmc_vw



		If !Used("PROJECTITREF_vw")
			msqlstr="SELECT [entry_ty],[tran_cd],[it_code],[item],[qty],[ac_id],[itserial],[bomid],[bomlevel];
		,[aentry_ty],[atran_cd],[ait_code],[aitserial],[aqty],[BATCHNO],[MFGDT],[EXPDT],[PMKEY] FROM PROJECTITREF where entry_ty='"+Alltrim(main_vw.entry_ty)+"' and tran_cd="+Str(main_vw.tran_cd)
			nretval = _curvouobj.sqlconobj.dataconn("EXE",company.dbname,msqlstr,"PROJECTITREF_vw1","_curvouobj.nhandle",_curvouobj.DataSessionId)

			A1=Afields(ARPROJECTITREF_vw1,'PROJECTITREF_vw1')
			For nCount = 1 To A1
				If ARPROJECTITREF_vw1(nCount,2)='T'
					ARPROJECTITREF_vw1(nCount,2)='D'
				Endif
			Endfor
			Create Cursor projectitref_vw From Array ARPROJECTITREF_vw1
			Insert Into projectitref_vw Select * From PROJECTITREF_vw1
			If Used('PROJECTITREF_vw1')
				Use In PROJECTITREF_vw1
			Endif
		Endif

		If !Used('Othitref_vw')
			lcstr = [SELECT * FROM OTHITREF A where a.entry_ty = ?Main_vw.Entry_Ty AND a.Tran_cd = ?Main_vw.Tran_cd Order by a.Rdate,a.rInv_no,a.ritserial]
			vald=_curvouobj.sqlconobj.dataconn("EXE",company.dbname,lcstr,"Othitref_vw","_curvouobj.nHandle",_curvouobj.DataSessionId)
			_curvouobj.sqlconobj.sqlconnclose("Thisform.nHandle")

			Select Othitref_vw
			Index On  entry_ty  + Str(tran_cd ) + itserial Tag ETITS
			Index On  rentry_ty + Str(itref_Tran) + ritserial Tag RETITS
			Index On entry_ty+Str(tran_cd)+itserial+rentry_ty+Str(itref_Tran)+ritserial Tag EtiReti
			Index On  entry_ty + Dtos(Date) + Doc_no + itserial Tag EDDITS
		Endif
		If !Used('WKRMDET_VW')
			If !Empty(lmc_vw.ALLOCID)
				msqlstr="SELECT * FROM WKRMDET where entry_ty='WK' and tran_cd="+lmc_vw.ALLOCID
				nretval = _curvouobj.sqlconobj.dataconn("EXE",company.dbname,msqlstr,"WKRMDET_VW","_curvouobj.nhandle",_curvouobj.DataSessionId)
			Endif
		Endif

		Delete From projectitref_vw Where entry_ty=main_vw.entry_ty And tran_cd=main_vw.tran_cd

		Delete From item_vw
		If Used('Gen_SrNo_Vw')
			Delete From gen_srno_vw
		Endif

		vitserial=0
		Select WKRMDET_VW
		Scan

			If WKRMDET_VW.rmqty>0
				msqlstr="SELECT it_name FROM It_mast where it_code=?WKRMDET_VW.rmit_code"
				nretval = _curvouobj.sqlconobj.dataconn("EXE",company.dbname,msqlstr,"tmptbl_vw","_curvouobj.nhandle",_curvouobj.DataSessionId)
				If nretval <=0
					Messagebox("Issue while getting item ",0,vumess)
					Return .F.
				Endif
				mitem=tmptbl_vw.it_name
				msqlstr="SELECT mfgdt,expdt,supbatchno,supmfgdt,supexpdt FROM stk_stat_vw where entry_ty=?WKRMDET_VW.rentry_ty and tran_cd=?WKRMDET_VW.rtran_cd and itserial=?WKRMDET_VW.ritserial"
				nretval = _curvouobj.sqlconobj.dataconn("EXE",company.dbname,msqlstr,"tmptbl_vw","_curvouobj.nhandle",_curvouobj.DataSessionId)
				If nretval <=0
					Messagebox("Issue while getting item ",0,vumess)
					Return .F.
				Endif

				vitserial=vitserial+1
				Select item_vw
				Append Blank
				Replace entry_ty With main_vw.entry_ty ;
					,Date With main_vw.Date;
					,Doc_no With main_vw.Doc_no ;
					,itserial With Padl(vitserial,5,'0');
					,item_no With Str(vitserial,5);
					,Item With mitem;
					,qty With WKRMDET_VW.rmqty;
					,qcaceptqty With Iif(Alltrim(lmc_vw.issuetype)=='RELEASED',WKRMDET_VW.rmqty,0);
					,qcrejqty With Iif(Alltrim(lmc_vw.issuetype)=='REJECTED',WKRMDET_VW.rmqty,0);
					,rate With 0;
					,gro_amt With 0;
					,cate With main_vw.cate;
					,party_nm With main_vw.party_nm;
					,inv_sr  With main_vw.inv_sr;
					,inv_no With main_vw.inv_no;
					,l_yn With main_vw.l_yn;
					,tran_cd With main_vw.tran_cd;
					,it_code  With WKRMDET_VW.rmit_code;
					,ac_id With main_vw.ac_id ;
					,bomid With WKRMDET_VW.bomid;
					,bomlevel With WKRMDET_VW.bomlevel;
					,batchno With WKRMDET_VW.rmbatchno;
					,mfgdt With tmptbl_vw.mfgdt;
					,expdt With tmptbl_vw.expdt;
					,SUPBATCHNO With tmptbl_vw.SUPBATCHNO;
					,SUPMFGDT With tmptbl_vw.SUPMFGDT ;
					,SUPEXPDT With tmptbl_vw.SUPEXPDT;
					In item_vw

				msqlstr="SELECT Top 1 Entry_ty,tran_cd,itserial,it_code,qty FROM item where tran_cd="+Alltrim(lmc_vw.ALLOCID)
				nretval = _curvouobj.sqlconobj.dataconn("EXE",company.dbname,msqlstr,"tmptbl_vw1","_curvouobj.nhandle",_curvouobj.DataSessionId)
				If nretval <=0
					Messagebox("Issue while getting work order item ",0,vumess)
					Return .F.
				Endif
				_aEntry_ty=tmptbl_vw1.entry_ty
				_atran_cd=tmptbl_vw1.tran_cd
				_aitserial=tmptbl_vw1.itserial
				_aqty =tmptbl_vw1.qty
				_ait_code=tmptbl_vw1.it_code

				Select projectitref_vw
				Append Blank
				Replace entry_ty With main_vw.entry_ty,tran_cd With main_vw.tran_cd,it_code With WKRMDET_VW.rmit_code,Item With mitem, qty With item_vw.qty,ac_id With main_vw.ac_id ;
					,itserial With item_vw.itserial,bomid With item_vw.bomid,bomlevel With item_vw.bomlevel ,aentry_ty With _aEntry_ty;
					,atran_cd With _atran_cd,ait_code With _ait_code,aitserial With _aitserial;
					,aqty With _aqty,pmkey With '-',batchno With WKRMDET_VW.rmbatchno;
					,mfgdt With tmptbl_vw.mfgdt,expdt With tmptbl_vw.expdt In projectitref_vw


				Select WKRMDET_VW
				Locate For rmit_code=item_vw.it_code And Alltrim(rmbatchno)=Alltrim(item_vw.batchno)
				If Found()
					_rentry_ty=WKRMDET_VW.rentry_ty
					_rtran_cd=WKRMDET_VW.rtran_cd
					_ritserial=WKRMDET_VW.ritserial
					_rqty=WKRMDET_VW.rmqty
				Endif

				Select Othitref_vw
				If ! Seek(main_vw.entry_ty+Str(main_vw.tran_cd)+item_vw.itserial,"Othitref_vw","ETITS")
					Select Othitref_vw
					Append Blank In Othitref_vw
					Replace Othitref_vw.entry_ty With main_vw.entry_ty,;
						Othitref_vw.tran_cd With main_vw.tran_cd,;
						Othitref_vw.Date With main_vw.Date,;
						Othitref_vw.Doc_no With main_vw.Doc_no,;
						Othitref_vw.itserial With item_vw.itserial,;
						Othitref_vw.item_no With item_vw.item_no,;
						Othitref_vw.Item With item_vw.Item,;
						Othitref_vw.Item With item_vw.ware_nm,;
						Othitref_vw.rentry_ty With _rentry_ty,;
						Othitref_vw.itref_Tran With _rtran_cd,;
						Othitref_vw.ritserial With _ritserial,;
						Othitref_vw.rqty With _rqty;
						Othitref_vw.it_code With item_vw.it_code;
						In Othitref_vw
				Endif

			Endif
			Select WKRMDET_VW
		Endscan

		Select item_vw
		Go Top


	Endif
Endif
If Used("WKRMDET_VW") Then
	Use In WKRMDET_VW
Endif

_curvouobj.Refresh()
Return .T.

Procedure wkallocation_disable
_curvouobj = _Screen.ActiveForm
If Type('_curvouobj.PCVTYPE')='C' 			&& AND ([vuexc] $ vchkprod)
	If Inlist(_curvouobj.pcvtype,'IP')
		Select item_vw
		_mrecno=Iif(!Eof(),Recno(),0)

		itemcnt=0
		Count For !Deleted() To itemcnt
		If _mrecno>0
			Select item_vw
			Go _mrecno
		Endif
		If itemcnt>0
			Return .F.
		Endif

		If lmc_vw.issuetype='REJECTED'
			Return .F.
		Endif
		msqlstr="SELECT Top 1 TRAN_CD FROM MAIN WHERE ENTRY_TY = 'WK' ";
			+" AND TRAN_CD NOT IN (SELECT ALLOCID FROM IPMAIN WHERE MAIN.TRAN_CD = IPMAIN.ALLOCID ) AND DATE<=?main_vw.date  ORDER BY DATE,INV_SR,INV_NO"
		nretval = _curvouobj.sqlconobj.dataconn("EXE",company.dbname,msqlstr,"tmptbl_vw","_curvouobj.nhandle",_curvouobj.DataSessionId)
		If nretval <=0
			Messagebox("Issue while getting work order detail.",0,vumess)
			Return .F.
		Endif
		If Reccount('tmptbl_vw')<=0
			Return .F.
		Endif
	Endif
Endif

Return .T.


Procedure wkalloc_disable
_curvouobj = _Screen.ActiveForm
If Type('_curvouobj.PCVTYPE')='C' 			&& AND ([vuexc] $ vchkprod)
	If Inlist(_curvouobj.pcvtype,"IP","ST")
		Select item_vw
		_mrecno=Iif(!Eof(),Recno(),0)

		itemcnt=0
		Count For !Deleted() To itemcnt
		If _mrecno>0
			Select item_vw
			Go _mrecno
		Endif
		If itemcnt>0
			Return .F.
		Endif
		If !Empty(lmc_vw.issuetype)
			Return .F.
		Endif
	Endif
Endif

Return .T.


Procedure deactwkbtn
If Inlist(main_vw.entry_ty,'IP')
	_Screen.ActiveForm.cmdbom.Enabled=.F.
Endif
Return .T.


Procedure Disable_MFormat
If Type('frmxtra.cboBatMFormat')='O'
	If Alltrim(it_mast_vw.BatchGen)=="Monthwise"
		frmxtra.cboBatMFormat.Enabled=.T.
		frmxtra.cmdBatMFormat.Enabled=.T.
		frmxtra.cboBatMFormat.Refresh()
	Else
		frmxtra.cboBatMFormat.Enabled=.F.
		frmxtra.cmdBatMFormat.Enabled=.F.
		frmxtra.cboBatMFormat.Value=''
		frmxtra.cboBatMFormat.Refresh()
	Endif
Endif

Return .T.

Procedure mfgdtexpdt_valid		&& Procedure to Check the Manufacturing Date with Expiry Date
Lparameters oobject

Select item_vw
If Upper(Alltrim(Strextract(oobject.ucontrolsource,'.'))) = 'EXPDT'
	If !Empty(item_vw.mfgdt) And !Empty(oobject.Value) And !(Year(item_vw.mfgdt)<=1900) And !(Year(item_vw.expdt)<=1900)
		If item_vw.mfgdt > oobject.Value
			=Messagebox("Manufacturing date cannot be greater than Expiry date.",0+64,vumess)
			Return .F.
		Endif
	Endif
Else
	If !Empty(oobject.Value) And !Empty(item_vw.expdt) And !(Year(item_vw.mfgdt)<=1900) And !(Year(item_vw.expdt)<=1900)
		If oobject.Value > item_vw.expdt
			=Messagebox("Manufacturing date cannot be greater than Expiry date.",0+64,vumess)
			Return .F.
		Endif
	Endif
Endif


If !Empty(item_vw.mfgdt) And !(Year(item_vw.mfgdt)<=1900) And Empty(item_vw.expdt) And Upper(Alltrim(Strextract(oobject.ControlSource,'.'))) = 'EXPDT'
	=Messagebox("Manufacturing date cannot be greater than Expiry date.",0+64,vumess)
	Return .F.
Endif
Do Case
Case Inlist(main_vw.entry_ty,"AR","OS")
	If !Empty(item_vw.mfgdt) And !(Year(item_vw.mfgdt)<=1900)
		If item_vw.mfgdt>main_vw.Date
			=Messagebox("Manufacturing date should be less than Transaction date .",0+64,vumess)
			Return .F.
		Endif
	Endif
Case Inlist(main_vw.entry_ty,"WK","OP")
	If !Empty(item_vw.mfgdt) And !(Year(item_vw.mfgdt)<=1900)
		If item_vw.mfgdt<main_vw.Date
			=Messagebox("Manufacturing date can't be less Transaction date.",0+64,vumess)
			Return .F.
		Endif
	Endif
	If !Empty(item_vw.mfgdt) And !(Year(item_vw.mfgdt)<=1900) And Empty(item_vw.expdt) And main_vw.entry_ty="OP"
		_curvouobj = _Screen.ActiveForm
		etsql_str = " Select validdays FROM it_mast WHERE it_code = ?Item_vw.it_code"
		etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[_vdays],"_curvouobj.nHandle",_curvouobj.DataSessionId,.F.)
		If etsql_con <= 0
			Messagebox("Error occured while getting valid days",0,vumess)
			Return .F.
		Endif
		If _vdays.validdays >0
			Replace expdt With Ttod(item_vw.mfgdt) + _vdays.validdays In item_vw
		Endif
	Endif
Endcase
If !Empty(item_vw.expdt) And !(Year(item_vw.expdt)<=1900)
	If item_vw.expdt<=main_vw.Date
		=Messagebox("Expiry date should be greater than Transaction date.",0+64,vumess)
		Return .F.
	Endif
Endif
Endproc

Procedure check_batchnoIP()
_mrgret  = 0

_curvouobj = _Screen.ActiveForm
If Type('_curvouobj.mainalias') = 'C'
	If Upper(_curvouobj.mainalias) <> 'MAIN_VW'
		Return
	Endif
Endif

If Used('BatchTbl_Vw')
	Select BatchTbl_Vw
	mrec=Recno()
Endif

If !Empty(item_vw.batchno)
	_malias 	= Alias()
	Sele item_vw
	_mrecno 	= Recno()

	If Inlist(main_vw.entry_ty,"IP")
		If Used('PROJECTITREF_VW')
			Select projectitref_vw
			Locate For entry_ty=main_vw.entry_ty And tran_cd=main_vw.tran_cd And itserial=item_vw.itserial
			If Found()
				If item_vw.batchno<>projectitref_vw.batchno
					Messagebox("Batch no. can't be changed as it is picked against work order.",0,vumess)
					Replace batchno With projectitref_vw.batchno In item_vw
				Endif
				Return .T.
			Endif
		Endif
	Endif
	Sele item_vw
	If Betw(_mrecno,1,Reccount())
		Go _mrecno
	Endif
	If !Empty(_malias)
		Select &_malias
	Endif
Endif
Return .T.





&& Added by Shrikant S. on 27/06/2014 for Bug-23280		&& End

&&Added by vasant on 18/07/2014 as per Bug 23384 - (Issue In Service Tax Credit Register).
Procedure MakeBlnk
Lparameters _ltblnm,_lfldnm
If Type('_ltblnm') = 'C' And Type('_lfldnm') = 'C'
	Replace (_lfldnm) With '' In (_ltblnm)
	_lreffldnm = 'frmxtra.txt'+Alltrim(_lfldnm)+'.refresh'
	&_lreffldnm
Endif
Return .T.
&&Added by vasant on 18/07/2014 as per Bug 23384 - (Issue In Service Tax Credit Register).


&& Added by Shrikant S. on 10/01/2014 for Bug-25063		&& Start
Procedure chk_Notresident
Lparameters _mPartyNm
_lcParty=_mPartyNm
_isNonResident=.F.
_msttype=''
etsql_con	= 1
etsql_str	= ''
_oldconval  = 0
nhandle     = 0
_etdatasessionid = _Screen.ActiveForm.DataSessionId
_etactform       = _Screen.ActiveForm
Set DataSession To _etdatasessionid
If Type('_etactform.sqlconobj') = 'O'
	sqlconobj1 = _etactform.sqlconobj
Else
	sqlconobj1 = Newobject('SqlConnUdObj','SqlConnection',xapps)
Endif
If Type('_etactform.nhandle') = 'N'
	_oldconval = _etactform.nhandle
Endif
If _oldconval > 0
	nhandle = _oldconval
Endif
etsql_str = "select top 1 st_type from Ac_Mast where AC_name = ?_lcParty"
etsql_con = sqlconobj1.dataconn([EXE],company.dbname,etsql_str,"_tmpdata","nHandle",_etdatasessionid)
If etsql_con > 0 And Used('_tmpdata')
	If Reccount('_tmpdata')	> 0
		_msttype = _tmpdata.st_type
	Endif
Endif
If !Empty(_msttype)
	If Alltrim(_tmpdata.st_type)=="OUT OF COUNTRY"
		_isNonResident=.T.
	Endif
Endif

If _oldconval <= 0
	=sqlconobj1.sqlconnclose("nHandle")
Endif
Release sqlconobj1
Return _isNonResident
&& Added by Shrikant S. on 10/01/2014 for Bug-25063		&& End
&&	Added by Shrikant S. on 19/05/2015 for Bug-26131		&& Start
Procedure CheckBatch
If oGlblPrdFeat.UdChkProd('exmfgbp')
	_curvouobj = _Screen.ActiveForm
	nhandle=0
	lcsqlstr=" Select Top 1 it_code from Litem_vw Where it_code=?it_mast_vw.it_code"
	nretval = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,lcsqlstr,"tmpit","nHandle",_curvouobj.DataSessionId )
	If nretval<0
		Return .F.
	Endif
	If Reccount('tmpit')>0
		Return .F.
	Endif
Endif
Return .T.

&& Added By Kishor A. for Bug-27300 on 01-12-2015 Start
Select Lother_vw
Go Top
If Type("lcode_vw.intrtrn")<>'U' And (.pcvtype="II")
	If lcode_vw.intrtrn =.T.
		Replace All Lother_vw.inter_use With ".F." For Inlist(Alltrim(Lother_vw.fld_nm),"TWARE","FWARE")
		Replace All Lother_vw.mandatory With ".T." For Inlist(Alltrim(Lother_vw.fld_nm),"TWARE","FWARE")
	Else
		Replace All Lother_vw.inter_use With ".T." For Inlist(Alltrim(Lother_vw.fld_nm),"TWARE","FWARE")
		Replace All Lother_vw.mandatory With ".F." For Inlist(Alltrim(Lother_vw.fld_nm),"TWARE","FWARE")
	Endif
Endif
&& Added By Kishor A. for Bug-27300 on 01-12-2015 End.

Procedure checkbatchExp
_curvouobj = _Screen.ActiveForm
nhandle=0
lcsqlstr=" Select Top 1 batchvalid from It_mast Where it_code=?item_vw.it_code"
nretval = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,lcsqlstr,"tmpit","nHandle",_curvouobj.DataSessionId )
If nretval<0
	Return .F.
Endif
If tmpit.batchvalid <>.T.
	Return .F.
Endif
Return .T.
&&	Added by Shrikant S. on 19/05/2015 for Bug-26131		&& End

&& Added by Shrikant S. on 21/04/2015 for Bug-25878		&& Start
Procedure Update_Warehouse
Lparameters oobject

malias=Alias()

Select item_vw
mrecno=Iif(!Eof(),Recno(),0)
Scan
	Replace ware_nm With lmc_vw.fware In item_vw
Endscan
If mrecno >0
	Select item_vw
	Go mrecno
Endif
If !Empty(malias)
	Select &malias
Endif
Return .T.
&& Added by Shrikant S. on 21/04/2015 for Bug-25878		&& End

&& Added by Shrikant S. on 21/04/2015 for Bug-26703		&& Start
Procedure checkInventoryallocation()
curObj=_Screen.ActiveForm
nhandle=0

sql_str  = 	" select Top 1 Entry_ty,Tran_cd,Itserial from it_srtrn "+;
	" where REntry_ty=?_ItSrTrn.Entry_ty and RTran_cd=?_ItSrTrn.Tran_cd	and "+;
	" RItSerial=?_ItSrTrn.ItSerial and iTran_cd=?_ItSrTrn.iTran_cd and pmkey<>'' "
sql_con = curObj.sqlconobj.dataconn([EXE],company.dbname,sql_str,[_RecAlloc],"nHandle",curObj.DataSessionId,.F.)

If Reccount('_RecAlloc')>0
	Return .F.
Endif
If Used('_RecAlloc')
	Use In _RecAlloc
Endif
Endproc
&& Added by Shrikant S. on 21/04/2015 for Bug-26703		&& End


&& Added by Shrikant S. on 19/11/2015 for Bug-27242		&& Start
Procedure checkSBCValid

If	main_vw.Date>Ctod("14/11/2015")
	Return .T.
Endif

Return .F.
&& Added by Shrikant S. on 19/11/2015 for Bug-27242		&& End


&& Added by Shrikant S. on 25/06/2016 for Bug-28339		&& Start
Procedure Check_KKCessDateValidation
Parameters _mDate

_ldate=main_vw.Date
If Type('_mDate') = 'T'
	If !Empty(_mDate) And Year(_mDate) > 1900
		_ldate= _mDate
	Endif
Endif
If	_ldate>=Ctod("01/06/2016")
	Return .T.
Endif
Return .F.
&& Added by Shrikant S. on 25/06/2016 for Bug-28339		&& End

&& Added by Shrikant S. on 04/10/2016 for GST	&& Start
Procedure GetAbatement
Parameters lcService

curObj=_Screen.ActiveForm
nhandle=0


If !Empty(lcService)
	mSqlCondn = [ Where ?Main_vw.Date Between Sdate and Edate and Name = ?lcService ]
	msqlstr = [ Select Top 1 Name,Abt_per,Ser_per,Cess_per,HCess_per From SerTax_Mast ]+mSqlCondn+[ Order by Name ]
	sql_con = curObj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,[tmptbl_vw],"nHandle",curObj.DataSessionId,.F.)
	If sql_con > 0 And Used('tmptbl_vw')
		Select tmptbl_vw
		If Reccount() <= 0
			=Messagebox("Service Tax Category Not Found in Master",0+32,vumess)
			Return .F.
		Endif
	Endif

	If item_vw.sabtamt <=0 And tmptbl_vw.Abt_per >0
		Replace sabtper With tmptbl_vw.Abt_per In item_vw
	Endif



Endif
Return

Procedure calc_abatement()

Replace rate  With SERVAMOUNT/qty,u_asseamt  With SERVAMOUNT/qty,staxable  With SERVAMOUNT - item_vw.SEXPAMT In item_vw

If item_vw.sabtper > 0 And !Empty(item_vw.serty)
	Replace sabtamt With (SERVAMOUNT * item_vw.sabtper /100);
		,staxable  With SERVAMOUNT -(SERVAMOUNT * item_vw.sabtper /100)- item_vw.SEXPAMT   In item_vw

Endif
Return

Procedure Cal_gstincl
Parameters lnval


If lnval<=0
	Retur
Endi
_PerPayReceiver = 0
If !Empty(item_vw.serty)
	curObj=_Screen.ActiveForm
	nhandle=0
	mSqlCondn = [ Where ?Main_vw.Date Between Sdate and Edate and Name = ?item_vw.serty]
	msqlstr = [ Select Top 1 PerPayReceiver From SerTax_Mast ]+mSqlCondn+[ Order by Name ]
	sql_con = curObj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,[tmptbl_vw],"nHandle",curObj.DataSessionId,.F.)
	If sql_con > 0 And Used('tmptbl_vw')
		Select tmptbl_vw
		If Reccount() <= 0
			=Messagebox("Service Tax Category Not Found in Master",0+32,vumess)
			Return .F.
		Endif
	Endif
	If !Empty(tmptbl_vw.PerPayReceiver)
		_PerPayReceiver = Evaluate(tmptbl_vw.PerPayReceiver)
	Endif

Endif



baseamt=100
If item_vw.sabtper<>0
	baseamt=(baseamt-item_vw.sabtper)
Endif
If item_vw.SEXPAMT<>0
	baseamt=(baseamt-(item_vw.SEXPAMT*100/lnval))
Endif

bsamt=baseamt*item_vw.SGST_PER /100
bcamt=baseamt*item_vw.CGST_PER  /100
biamt=baseamt*item_vw.IGST_PER  /100
tbamt=bsamt+bcamt+biamt

_PerPayReceiver = 100 - _PerPayReceiver
tbamt = ((tbamt * _PerPayReceiver)/100)
tbamt = (lnval * 100)/(100 + tbamt)		

*!*	ccgst=tbamt* item_vw.CGST_PER /100
*!*	csgst=tbamt* item_vw.SGST_PER /100
*!*	cigst=tbamt* item_vw.IGST_PER /100

*!*	rccgst=ROUND(ccgst,0)
*!*	rcsgst=ROUND(ccgst,0)
*!*	rcigst=ROUND(cigst,0)


*!*	tbamt=tbamt+ (ccgst-rccgst)
*!*	tbamt=tbamt+ (csgst-rcsgst)
*!*	tbamt=tbamt+ (cigst-rcigst)

Replace amtexcgst With tbamt,staxable With tbamt,u_asseamt With tbamt In item_vw


If item_vw.sabtper > 0 And !Empty(item_vw.serty)
	Replace sabtamt With (amtexcgst * item_vw.sabtper /100);
		,staxable  With amtexcgst -(amtexcgst * item_vw.sabtper /100)- item_vw.SEXPAMT   In item_vw

Endif
Replace rate  With staxable/qty,u_asseamt  With staxable/qty In item_vw		&& 06/12/2016 for GST
If !Empty(item_vw.serty)
	If Used("Acdetalloc_vw")
		Select acdetalloc_vw
		Set Order To
		Locate For entry_ty=main_vw.entry_ty And tran_cd=main_vw.tran_cd And itserial=item_vw.itserial
		If !Found()
			Append Blank In acdetalloc_vw
			Replace entry_ty With main_vw.entry_ty, tran_cd With main_vw.tran_cd,itserial With item_vw.itserial,serty With item_vw.serty,Amount With tbamt ;
				,sabtper With item_vw.sabtper,sabtamt With item_vw.sabtamt,staxable With item_vw.staxable;
				,SEXPAMT With item_vw.SEXPAMT,sabtsr With item_vw.sabtsr,ssubcls With item_vw.ssubcls,sexnoti With item_vw.sexnoti;
				,Amountin With item_vw.AMTINCGST,SEXNOTISL With item_vw.SEXNOTISL,SABTSRSL With item_vw.SABTSRSL;
				,cgst_amt With item_vw.cgst_amt,sgst_amt With item_vw.sgst_amt,igst_amt With item_vw.igst_amt;
				,cgsrt_amt With item_vw.cgsrt_amt,sgsrt_amt With item_vw.sgsrt_amt,igsrt_amt With item_vw.igsrt_amt;
				In acdetalloc_vw
		Else
			Replace Amount With tbamt ;
				,sabtper With item_vw.sabtper,sabtamt With item_vw.sabtamt,staxable With item_vw.staxable,Amountin With item_vw.AMTINCGST ;
				,cgst_amt With item_vw.cgst_amt,sgst_amt With item_vw.sgst_amt,igst_amt With item_vw.igst_amt;
				,cgsrt_amt With item_vw.cgsrt_amt,sgsrt_amt With item_vw.sgsrt_amt,igsrt_amt With item_vw.igsrt_amt;
				In acdetalloc_vw

		Endif
	Endif
Endif
*Replace rate  With staxable/qty,u_asseamt  With staxable/qty,staxable  With staxable - item_vw.SEXPAMT In item_vw		&& 06/12/2016 for GST



Return


Procedure Cal_gstexcl()


Replace rate  With amtexcgst /qty,u_asseamt  With amtexcgst /qty,staxable  With amtexcgst - item_vw.SEXPAMT In item_vw


If item_vw.sabtper > 0 And !Empty(item_vw.serty)
	Replace sabtamt With (amtexcgst * item_vw.sabtper /100);
		,staxable  With amtexcgst -(amtexcgst * item_vw.sabtper /100)- item_vw.SEXPAMT   In item_vw

Endif
If !Empty(item_vw.serty)
	If Used("Acdetalloc_vw")
		Select acdetalloc_vw
		Set Order To
		Locate For entry_ty=main_vw.entry_ty And tran_cd=main_vw.tran_cd And itserial=item_vw.itserial
		If !Found()
			Append Blank In acdetalloc_vw
			Replace entry_ty With main_vw.entry_ty, tran_cd With main_vw.tran_cd,itserial With item_vw.itserial,serty With item_vw.serty,Amount With item_vw.staxable ;
				,sabtper With item_vw.sabtper,sabtamt With item_vw.sabtamt,staxable With item_vw.staxable;
				,SEXPAMT With item_vw.SEXPAMT,sabtsr With item_vw.sabtsr,ssubcls With item_vw.ssubcls,sexnoti With item_vw.sexnoti;
				,Amountin With item_vw.AMTINCGST,SEXNOTISL With item_vw.SEXNOTISL,SABTSRSL With item_vw.SABTSRSL;
				,cgst_amt With item_vw.cgst_amt,sgst_amt With item_vw.sgst_amt,igst_amt With item_vw.igst_amt;
				,cgsrt_amt With item_vw.cgsrt_amt,sgsrt_amt With item_vw.sgsrt_amt,igsrt_amt With item_vw.igsrt_amt;
				In acdetalloc_vw
		Else
			Replace Amount With item_vw.staxable ;
				,sabtper With item_vw.sabtper,sabtamt With item_vw.sabtamt,staxable With item_vw.staxable,Amountin With item_vw.AMTINCGST ;
				,cgst_amt With item_vw.cgst_amt,sgst_amt With item_vw.sgst_amt,igst_amt With item_vw.igst_amt;
				,cgsrt_amt With item_vw.cgsrt_amt,sgsrt_amt With item_vw.sgsrt_amt,igsrt_amt With item_vw.igsrt_amt;
				In acdetalloc_vw

		Endif
	Endif
Endif

Return

Procedure act_deactcontrol()
_oForm=_Screen.ActiveForm
frmxtra.txtserty.ReadOnly=.T.
If item_vw.sabtper >0 And (_oForm.addmode Or _oForm.editmode)
	frmxtra.cbosabtsrsl.Enabled=.T.
	frmxtra.cbosabtsr.Enabled =.T.
	frmxtra.cmdsabtsrsl.Enabled=.T.
	frmxtra.cmdsabtsr.Enabled =.T.
Else
	frmxtra.cbosabtsrsl.Enabled=.F.
	frmxtra.cbosabtsr.Enabled =.F.
	frmxtra.cmdsabtsrsl.Enabled=.F.
	frmxtra.cmdsabtsr.Enabled =.F.
Endif


&& Commented by Shrikant S. on 09/02/2017 for GST		&& Start
*!*	If Upper(main_vw.serrule)="PURE AGENT"
*!*		frmxtra.txtsexpamt.Enabled= .T.
*!*	Endif

*!*	If Upper(main_vw.serrule)="EXEMPT"  And (_oForm.addmode Or _oForm.editmode)
*!*		frmxtra.cbosexnoti.Enabled= .T.
*!*		frmxtra.cbosexnotisl.Enabled=.T.
*!*		frmxtra.cmdsexnoti.Enabled= .T.
*!*		frmxtra.cmdsexnotisl.Enabled=.T.
*!*	Else
*!*		frmxtra.cbosexnoti.Enabled= .F.
*!*		frmxtra.cbosexnotisl.Enabled=.F.
*!*		frmxtra.cmdsexnoti.Enabled= .F.
*!*		frmxtra.cmdsexnotisl.Enabled=.F.
*!*	Endif
&& Commented by Shrikant S. on 09/02/2017 for GST		&& End

Return


Procedure Update_BillDate
Parameters lcbillno

curObj=_Screen.ActiveForm
nhandle=0

If !Empty(lcbillno)
*!*		msqlstr = " Select Top 1 u_pinvdt From PTMAIN Where u_pinvno='"+Rtrim(lcbillno)+"'"			&& Commented by Shrikant S. on 25/01/2017 for GST
	msqlstr = " Select Top 1 pinvdt From PTMAIN Where pinvno='"+Rtrim(lcbillno)+"'"				&& Added by Shrikant S. on 25/01/2017 for GST
	sql_con = curObj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,[tmptbl_vw],"nHandle",curObj.DataSessionId,.F.)
	If sql_con > 0 And Used('tmptbl_vw')
		Select tmptbl_vw
		If Reccount() <= 0
			=Messagebox("Bill date not found",0+32,vumess)
			Return .F.
		Endif
*!*			Replace sbdate With tmptbl_vw.u_pinvdt In item_vw					&& Commented by Shrikant S. on 25/01/2017 for GST
		Replace sbdate With tmptbl_vw.pinvdt In item_vw						&& Added by Shrikant S. on 25/01/2017 for GST
	Endif
Endif
Return

Procedure Update_category
Parameters lcadvtype

curObj=_Screen.ActiveForm
If curObj.addmode Or curObj.editmode
	If Upper(Alltrim(lcadvtype.Value))=="GOODS"
		IF curObj.pcvtype="BR"
			curObj.txtsvc_cate.Enabled=.T.
			curObj.cmdsvc_cate.Enabled=.T.
		ELSE
			curObj.txtsvc_cate.Enabled=.F.
			curObj.cmdsvc_cate.Enabled=.F.
		ENDIF
		Replace serrule With "" In main_vw
		curObj.Refresh()
	Else
		curObj.txtsvc_cate.Enabled=.T.
		curObj.cmdsvc_cate.Enabled=.T.
	Endif
*!*		lcadvtype.Enabled=.F.
*!*		lcadvtype.Refresh()
	curObj.txtsvc_cate.Refresh()
	curObj.cmdsvc_cate.Refresh()
Endif
Return

Procedure AdvtypeSetting

curObj=_Screen.ActiveForm
llvalid=Iif(Type('main_vw.tdspaytype')<>'U',Iif(main_vw.tdspaytype=2,.T.,.F.),.T.)
*!*	If(main_vw.tdspaytype=2 And curObj.addmode)
If(curObj.addmode) And llvalid

	itemcount=0
	Select item_vw
	lnrecno=Iif(!Eof(),Recno(),0)
	Count For !Deleted() To itemcount

	If lnrecno >0
		Select item_vw
		Go lnrecno
	Endif
	If itemcount> 0
		Return .F.
	Endif
Else
	Return .F.
Endif

Return

Procedure ServiceRuleSetting
curObj=_Screen.ActiveForm
If(main_vw.tdspaytype=2 And curObj.addmode And Alltrim(main_vw.againstgs)="SERVICES")
	itemcount=0
	Select item_vw
	lnrecno=Iif(!Eof(),Recno(),0)

	Count For !Deleted() To itemcount


	If lnrecno >0
		Select item_vw
		Go lnrecno
	Endif
	If itemcount> 0
		Return .F.
	Endif
Else
	Return .F.
Endif
*!*	If curObj.addmode Or curObj.editmode
*!*		loserrule.Enabled=.F.
*!*		loserrule.Refresh()
*!*	Endi
Return

Procedure ControlSetting_GB
Parameters lcpaytype
Set Step On
_oForm=_Screen.ActiveForm

If Type("lcpaytype")='O'

	If (_oForm.addmode Or _oForm.editmode) And _oForm.pcvtype='GB'
		lcsource=Upper(Alltrim(lcpaytype.ControlSource))
		frmxtra.txtCREDITCNO.Enabled=.F.
		frmxtra.txtCREDITCNM.Enabled=.F.
		frmxtra.txtNETBANKAC.Enabled=.F.
		frmxtra.cboPAYTYPE.Enabled=.F.
		frmxtra.txtBANKACNO.Enabled=.F.
		frmxtra.txtBENEACNO.Enabled=.F.
		frmxtra.txtBENEACNM.Enabled=.F.
		frmxtra.txtBANKCODE.Enabled=.F.


		If Inlist(lcsource,"LMC_VW.EPAY","LMC_VW.OVERCOUNT","LMC_VW.NEFTRTGS")
			Do Case
			Case lcsource="LMC_VW.EPAY"
				If lmc_vw.EPAY=.T. And (lmc_vw.OVERCOUNT=.T. Or lmc_vw.NEFTRTGS=.T.)
					Replace EPAY With .F. In lmc_vw
				Endif
			Case lcsource="LMC_VW.OVERCOUNT"
				If lmc_vw.OVERCOUNT=.T. And (lmc_vw.EPAY=.T. Or lmc_vw.NEFTRTGS=.T.)
					Replace OVERCOUNT With .F. In lmc_vw
				Endif
			Case lcsource="LMC_VW.NEFTRTGS"
				If lmc_vw.NEFTRTGS=.T. And (lmc_vw.OVERCOUNT=.T. Or lmc_vw.EPAY=.T.)
					Replace NEFTRTGS With .F. In lmc_vw
				Endif
			Endcase
			If lmc_vw.EPAY=.T.
				frmxtra.txtCREDITCNO.Enabled=.T.
				frmxtra.txtCREDITCNM.Enabled=.T.
				frmxtra.txtNETBANKAC.Enabled=.T.
				Replace OVERCOUNT With .F., NEFTRTGS  With .F. In lmc_vw
			Else
				frmxtra.txtCREDITCNO.Enabled=.F.
				frmxtra.txtCREDITCNM.Enabled=.F.
				frmxtra.txtNETBANKAC.Enabled=.F.

				frmxtra.txtCREDITCNO.Value=""
				frmxtra.txtCREDITCNM.Value=""
				frmxtra.txtNETBANKAC.Value=""
			Endif

			If lmc_vw.OVERCOUNT=.T.
				frmxtra.cboPAYTYPE.Enabled=.T.
				frmxtra.txtBANKACNO.Enabled=.T.
				Replace EPAY With .F., NEFTRTGS  With .F. In lmc_vw
			Else
				frmxtra.cboPAYTYPE.Enabled=.F.
				frmxtra.txtBANKACNO.Enabled=.F.

				frmxtra.cboPAYTYPE.Value=""
				frmxtra.txtBANKACNO.Value=""
			Endif
			If lmc_vw.NEFTRTGS =.T.
				frmxtra.txtBENEACNO.Enabled=.T.
				frmxtra.txtBENEACNM.Enabled=.T.
				frmxtra.txtBANKCODE.Enabled=.T.
				Replace EPAY With .F., OVERCOUNT With .F. In lmc_vw
			Else
				frmxtra.txtBENEACNO.Enabled=.F.
				frmxtra.txtBENEACNM.Enabled=.F.
				frmxtra.txtBANKCODE.Enabled=.F.

				frmxtra.txtBENEACNO.Value=""
				frmxtra.txtBENEACNM.Value=""
				frmxtra.txtBANKCODE.Value=""
			Endif
			frmxtra.Refresh()
		Endif
	Endif
Endif
Return

Procedure debitaceffect_bp()
Parameters _fldnm


_fldnm = Upper(_fldnm)
_vacnm = ''
_isadv = .F.
_vacacname  = ''
_spacct	    = ''
_vseravail = findserviceavailtype()
*!*	_vservtype = findallocservicetype()			&& Commented by Shrikant S. on 09/02/2017 for GST
_llImport=IsUnderImport()						&& Added by Shrikant S. on 09/02/2017 for GST

Do Case
*!*	Case _vservtype <> "IMPORT" And main_vw.tdspaytype=1				&& Commented by Shrikant S. on 09/02/2017 for GST
Case !_llImport And main_vw.tdspaytype=1					&& Added by Shrikant S. on 09/02/2017 for GST
	_vacnm = ''
*!*	Case (_vservtype = "IMPORT" And main_vw.tdspaytype=1) Or (main_vw.tdspaytype=2)				&& Commented by Shrikant S. on 09/02/2017 for GST
Case (_llImport And main_vw.tdspaytype=1) Or (main_vw.tdspaytype=2)			&& Added by Shrikant S. on 09/02/2017 for GST
	Do Case
	Case _fldnm="CGST_AMT"
		_vacnm="Input CGST"
	Case _fldnm="SGST_AMT"
		_vacnm="Input SGST"
	Case _fldnm="IGST_AMT"
		_vacnm="Input IGST"
	Endcase

&& Commented by Shrikant S. on 13/04/2017 for GST 		&& Start
*!*		If IsUnderImport()
*!*			Do Case
*!*			Case _fldnm="CGST_AMT"
*!*				_vacnm="Input CGST"
*!*			Case _fldnm="SGST_AMT"
*!*				_vacnm="Input SGST"
*!*			Case _fldnm="IGST_AMT"
*!*				_vacnm="Input IGST"
*!*			Endcase
*!*		Else
*!*			Do Case
*!*			Case _fldnm="CGST_AMT"
*!*				_vacnm="CGST Provisional"
*!*			Case _fldnm="SGST_AMT"
*!*				_vacnm="SGST Provisional"
*!*			Case _fldnm="IGST_AMT"
*!*				_vacnm="IGST Provisional"
*!*			Endcase

*!*		Endif
&& Commented by Shrikant S. on 13/04/2017 for GST 		&& End


*!*		If _vseravail="SERVICE"
*!*			Do Case
*!*			Case _fldnm="CGST_AMT"
*!*				_vacnm="CGST Provisional"
*!*			Case _fldnm="SGST_AMT"
*!*				_vacnm="SGST Provisional"
*!*			Case _fldnm="IGST_AMT"
*!*				_vacnm="IGST Provisional"
*!*			Endcase
*!*		Endif
*!*		If _vseravail="EXCISE"
*!*			Do Case
*!*			Case _fldnm="CGST_AMT"
*!*				_spacct="CGST Balance"
*!*			Case _fldnm="SGST_AMT"
*!*				_spacct="SGST Balance"
*!*			Case _fldnm="IGST_AMT"
*!*				_spacct="IGST Balance"
*!*			Endcase
*!*		Endif
Endcase
If !Empty(_spacct)
	_vacacname =EtValidFindAcctName(_spacct)
Else
	If !Empty(_vacnm)
		_vacacname = EtValidFindAcctName(_vacnm)
	Endif
Endif

Return _vacacname


Procedure creditaceffect_bp()
Parameters _fldnm


_fldnm = Upper(_fldnm)
_vacnm = ''
_isadv = .F.
_vacacname  = ''
_spacct	    = ''
_vseravail = findserviceavailtype()
*!*	_vservtype = findallocservicetype()				&& Commented by Shrikant S. on 09/02/2017 for GST
_vservCreditAvail = findallocserviceCreditAvail()	&&Changes has been done by Vasant on 17/02/2014 as per Bug-21539 (No Service Tax Credit for Car and Cleaning Services).
_isadv      = .F.
_llImport=IsUnderImport()							&& Added by Shrikant S. on 09/02/2017 for GST
If !Empty(coadditional.seraccdt)
	_isadv = Eval(coadditional.seraccdt)
Endif
Do Case
*!*	Case _vservtype <> "IMPORT" And main_vw.tdspaytype=1			&& Commented by Shrikant S. on 09/02/2017 for GST
Case !_llImport And main_vw.tdspaytype=1					&& Added by Shrikant S. on 09/02/2017 for GST
	_vacnm = ''
*!*	Case (_vservtype = "IMPORT" And main_vw.tdspaytype=1) Or (main_vw.tdspaytype=2)			&& Commented by Shrikant S. on 09/02/2017 for GST
Case (_llImport And main_vw.tdspaytype=1) Or (main_vw.tdspaytype=2)						&& Added by Shrikant S. on 09/02/2017 for GST
*!*		If _isadv = .F.
*!*			Do Case
*!*			Case _fldnm="SERBAMT"
*!*				_vacnm="Input Service Tax"
*!*			Case _fldnm="SERCAMT"
*!*				_vacnm="Input Service Tax-Ecess"
*!*			Case _fldnm="SERHAMT"
*!*				_vacnm="Input Service Tax-Hcess"
*!*			Endcase
*!*		Endif
	If _isadv = .T.


&& Commented by Shrikant S. on 10/02/2017 for GST		&& Start
		If IsUnderImport()
			Do Case
			Case _fldnm="CGST_AMT"
				_vacnm="CGST Payable"
			Case _fldnm="SGST_AMT"
				_vacnm="SGST Payable"
			Case _fldnm="IGST_AMT"
				_vacnm="IGST Payable"
			Endcase
		Else
			Do Case
			Case _fldnm="CGST_AMT"
				_vacnm="Input CGST"
			Case _fldnm="SGST_AMT"
				_vacnm="Input SGST"
			Case _fldnm="IGST_AMT"
				_vacnm="Input IGST"
			Endcase

		Endif
&& Commented by Shrikant S. on 10/02/2017 for GST		&& End



	Endif
&&Changes has been done by Vasant on 17/02/2014 as per Bug-21539 (No Service Tax Credit for Car and Cleaning Services).
	If _vservCreditAvail = .F. And main_vw.tdspaytype=2
		If _vseravail="SERVICE"
			Do Case
			Case _fldnm="CGST_AMT"
				_vacnm="CGST Provisional"
			Case _fldnm="SGST_AMT"
				_vacnm="SGST Provisional"
			Case _fldnm="IGST_AMT"
				_vacnm="IGST Provisional"
			Endcase
		Endif
		If _vseravail="EXCISE"
			Do Case
			Case _fldnm="CGST_AMT"
				_spacct="CGST Balance"
			Case _fldnm="SGST_AMT"
				_spacct="SGST Balance"
			Case _fldnm="IGST_AMT"
				_spacct="IGST Balance"
			Endcase
		Endif
	Endif
&&Changes has been done by Vasant on 17/02/2014 as per Bug-21539 (No Service Tax Credit for Car and Cleaning Services).
Endcase
If !Empty(_spacct)
	_vacacname =EtValidFindAcctName(_spacct)
Else
	If !Empty(_vacnm)
		_vacacname = EtValidFindAcctName(_vacnm)
	Endif
Endif
Return _vacacname

Procedure debitaceffect_bp_reversal()
Parameters _fldnm
_fldnm = Upper(_fldnm)
_vacnm = ''
_vacacname  = ''
&& Commented by Shrikant S. on 10/02/2017 for GST		&& Start
Do Case
Case _fldnm="CGSRT_AMT"
	_vacnm="Input CGST"
Case _fldnm="SGSRT_AMT"
	_vacnm="Input SGST"
Case _fldnm="IGSRT_AMT"
	_vacnm="Input IGST"
Endcase
&& Commented by Shrikant S. on 10/02/2017 for GST		&& End


*!*	Do Case
*!*	Case _fldnm="CGSRT_AMT"
*!*		_vacnm="CGST Provisional"
*!*	Case _fldnm="SGSRT_AMT"
*!*		_vacnm="SGST Provisional"
*!*	Case _fldnm="IGSRT_AMT"
*!*		_vacnm="iGST Provisional"
*!*	Endcase


If !Empty(_vacnm)
	_vacacname = EtValidFindAcctName(_vacnm)
Endif
Return _vacacname


Procedure creditaceffect_bp_reversal()
Parameters _fldnm
_fldnm = Upper(_fldnm)
_vacnm = ''
_vacacname  = ''
Do Case
Case _fldnm	= "CGSRT_AMT"
	_vacnm="CGST Payable"
Case _fldnm	= "SGSRT_AMT"
	_vacnm="SGST Payable"
Case _fldnm	= "IGSRT_AMT"
	_vacnm="IGST Payable"
Endcase
If !Empty(_vacnm)
	_vacacname = EtValidFindAcctName(_vacnm)
Endif
Return _vacacname





Procedure debitaceffect_e1()
Parameters _fldnm

_fldnm = Upper(_fldnm)
_vacnm = ''
_isadv = .F.
_vacacname  = ''
_spacct	    = ''

llImport=IsUnderImport()
If Type('_mseravail') = 'C'
	If Inlist(_mseravail,"EXCISE","SERVICES")
		_isadv = .T.
	Endi
Endif
Select acdetalloc_vw
If Seek(item_vw.itserial,'acdetalloc_vw','itserial')
	_vservCreditAvail = findallocserviceCreditAvail()	&&(No Service Tax Credit for Car and Cleaning Services).
	Do Case
*!*		Case (main_vw.serrule='IMPORT') Or (_isadv = .T. And main_vw.serrule != 'IMPORT')			&& Commented by Shrikant S. on 09/02/2017 for GST
	Case llImport Or (_isadv = .T. And !llImport)				&& Added by Shrikant S. on 09/02/2017 for GST

&& added by Shrikant S. on 10/04/2017 for GST		&& Start
		If llImport
			Do Case
			Case _fldnm="CGST_AMT"
				_vacnm="Input CGST"
			Case _fldnm="SGST_AMT"
				_vacnm="Input SGST"
			Case _fldnm="IGST_AMT"
				_vacnm="Input IGST"
			Endcase
		Else
			Do Case
			Case _fldnm="CGST_AMT"
				_vacnm="CGST Provisional"
			Case _fldnm="SGST_AMT"
				_vacnm="SGST Provisional"
			Case _fldnm="IGST_AMT"
				_vacnm="IGST Provisional"
			Endcase

		Endif
&& added by Shrikant S. on 10/04/2017 for GST		&& End


&& 130217
*!*			Do Case
*!*			Case _fldnm="CGST_AMT"
*!*				_vacnm="Input CGST"
*!*			Case _fldnm="SGST_AMT"
*!*				_vacnm="Input SGST"
*!*			Case _fldnm="IGST_AMT"
*!*				_vacnm="Input IGST"
*!*			Endcase

*!*	&& Commented by Shrikant S. on 10/02/2017 for GST		&& Start
*!*			If llImport
*!*				Do Case
*!*				Case _fldnm="CGST_AMT"
*!*					_vacnm="CGST Provisional"
*!*				Case _fldnm="SGST_AMT"
*!*					_vacnm="SGST Provisional"
*!*				Case _fldnm="IGST_AMT"
*!*					_vacnm="IGST Provisional"
*!*				Endcase
*!*			Else
*!*				Do Case
*!*				Case _fldnm="CGST_AMT"
*!*					_vacnm="Input CGST"
*!*				Case _fldnm="SGST_AMT"
*!*					_vacnm="Input SGST"
*!*				Case _fldnm="IGST_AMT"
*!*					_vacnm="Input IGST"
*!*				Endcase
*!*			Endif
*!*	&& Commented by Shrikant S. on 10/02/2017 for GST		&& End

&& Added by Shrikant S. on 10/02/2017 for GST		&& Start
*!*			Do Case
*!*			Case _fldnm="CGST_AMT"
*!*				_vacnm="CGST Provisional"
*!*			Case _fldnm="SGST_AMT"
*!*				_vacnm="SGST Provisional"
*!*			Case _fldnm="IGST_AMT"
*!*				_vacnm="IGST Provisional"
*!*			Endcase
&& Added by Shrikant S. on 10/02/2017 for GST		&& End

*!*		Case _isadv = .F. And main_vw.serrule!='IMPORT'			&& Commented by Shrikant S. on 09/02/2017 for GST
	Case _isadv = .F. And !llImport				&& Added by Shrikant S. on 09/02/2017 for GST
		If acdetalloc_vw.seravail="SERVICES"
			Do Case
			Case _fldnm="CGST_AMT"
				_vacnm="CGST Provisional"
			Case _fldnm="SGST_AMT"
				_vacnm="SGST Provisional"
			Case _fldnm="IGST_AMT"
				_vacnm="IGST Provisional"
			Endcase
		Endif
*!*			If acdetalloc_vw.seravail="EXCISE"
*!*				Do Case
*!*				Case _fldnm="CGST_AMT"
*!*					_spacct="CGST Balance"
*!*				Case _fldnm="SGST_AMT"
*!*					_spacct="SGST Balance"
*!*				Case _fldnm="IGST_AMT"
*!*					_spacct="IGST Balance"
*!*				Endcase
*!*			Endif
&& (No Service Tax Credit for Car and Cleaning Services).
		If _vservCreditAvail = .F.

&& Commented by Shrikant S. on 10/02/2017 for GST		&& Start

&& 130217
*!*				Do Case
*!*				Case _fldnm="CGST_AMT"
*!*					_vacnm="Input CGST"
*!*				Case _fldnm="SGST_AMT"
*!*					_vacnm="Input SGST"
*!*				Case _fldnm="IGST_AMT"
*!*					_vacnm="Input IGST"
*!*				Endcase

			If llImport
				Do Case
				Case _fldnm="CGST_AMT"
					_vacnm="CGST Provisional"
				Case _fldnm="SGST_AMT"
					_vacnm="SGST Provisional"
				Case _fldnm="IGST_AMT"
					_vacnm="IGST Provisional"
				Endcase
			Else
				Do Case
				Case _fldnm="CGST_AMT"
					_vacnm="Input CGST"
				Case _fldnm="SGST_AMT"
					_vacnm="Input SGST"
				Case _fldnm="IGST_AMT"
					_vacnm="Input IGST"
				Endcase

			Endif
&& Commented by Shrikant S. on 10/02/2017 for GST		&& End
		Endif
&& (No Service Tax Credit for Car and Cleaning Services).
	Endcase
Endif
If !Empty(_spacct)
	_vacacname = EtValidFindAcctName(_spacct)
Else
	If !Empty(_vacnm)
		_vacacname =EtValidFindAcctName(_vacnm)
	Endif
Endif
Return _vacacname


Procedure creditaceffect_e1()
Parameters _fldnm

_fldnm = Upper(_fldnm)
_vacnm = ''
_isadv = .F.
_vacacname  = ''
_spacct	    = ''

llImport=IsUnderImport()
If Type('_mseravail') = 'C'
	If Inlist(_mseravail,"EXCISE","SERVICE")
		_isadv = .T.
	Endi
Endif
Select acdetalloc_vw
If Seek(item_vw.itserial,'acdetalloc_vw','itserial')
	Do Case
	Case _isadv = .T.
		If _mseravail="SERVICE"
			Do Case
			Case _fldnm="CGST_AMT"
				_vacnm="CGST Provisional"
			Case _fldnm="SGST_AMT"
				_vacnm="SGST Provisional"
			Case _fldnm="IGST_AMT"
				_vacnm="IGST Provisional"
			Endcase
		Endif
		If _mseravail="EXCISE"
			Do Case
			Case _fldnm="CGST_AMT"
				_spacct="CGST Balance"
			Case _fldnm="SGST_AMT"
				_spacct="SGST Balance"
			Case _fldnm="IGST_AMT"
				_spacct="IGST Balance"
			Endcase
		Endif

	Case _isadv = .F.
&& Commented by Shrikant S. on 09/02/2017 for GST		&& Start
*!*			If main_vw.serrule<>'IMPORT'
*!*				_vacnm=""
*!*			Endif
&& Commented by Shrikant S. on 09/02/2017 for GST		&& End

*!*			If main_vw.serrule='IMPORT'			&& Commented by Shrikant S. on 09/02/2017 for GST
		If llImport					&& Added by Shrikant S. on 09/02/2017 for GST
			Do Case
			Case _fldnm	= "CGST_AMT"
				_vacnm="CGST Payable"
			Case _fldnm	= "SGST_AMT"
				_vacnm="SGST Payable"
			Case _fldnm	= "IGST_AMT"
				_vacnm="IGST Payable"
			Endcase
		Endif
	Endcase
Endif
If !Empty(_spacct)
	_vacacname = EtValidFindAcctName(_spacct)
Else
	If !Empty(_vacnm)
		_vacacname = EtValidFindAcctName(_vacnm)
	Endif
Endif
Return _vacacname


Procedure debitaceffect_br
Parameters _fldnm

_fldnm		= Upper(_fldnm)
_vacnm      = ''
_vacacname  = ''
_isadv      = .F.
If !Empty(coadditional.seraccdt)
	_isadv = Eval(coadditional.seraccdt)
Endif

Do Case
Case main_vw.tdspaytype = 1 And _isadv = .T.
	_vacnm=""
Case _fldnm	= "CGST_AMT" And _isadv = .T.
	_vacnm="Output CGST"
Case _fldnm	= "SGST_AMT" And _isadv = .T.
	_vacnm="Output SGST"
Case _fldnm	= "IGST_AMT" And _isadv = .T.
	_vacnm="Output IGST"
Endcase

If !Empty(_vacnm)
	_vacacname = EtValidFindAcctName(_vacnm)
Endif
Return _vacacname


Procedure creditaceffect_br
Parameters _fldnm
_fldnm		= Upper(_fldnm)
_vacnm      = ''
_vacacname  = ''

Do Case
Case _fldnm	= "CGST_AMT" And main_vw.tdspaytype = 2
	_vacnm="CGST Payable"
Case _fldnm	= "SGST_AMT" And main_vw.tdspaytype = 2
	_vacnm="SGST Payable"
Case _fldnm	= "IGST_AMT" And main_vw.tdspaytype = 2
	_vacnm="IGST Payable"
Endcase
If !Empty(_vacnm)
	_vacacname = EtValidFindAcctName(_vacnm)
Endif
Return _vacacname

Procedure debitaceffect_S1
Parameters _fldnm
_fldnm		= Upper(_fldnm)
_vacnm      = ''
_vacacname  = ''
_isadv      = .F.
If Type('_mseravail') = 'C'
	If Inlist(_mseravail,"EXCISE","SERVICE")
		_isadv = .T.
	ENDIF
&& Added by Shrikant S. on 29/04/2017 for GST		&& Start	
ELSE
	IF IsUnderExport()
		_isadv = .T.
	ENDIF
&& Added by Shrikant S. on 29/04/2017 for GST		&& End	
ENDIF
Select acdetalloc_vw
If Seek(item_vw.itserial,'acdetalloc_vw','itserial')

	Do Case
	Case _fldnm	= "CGST_AMT" And _isadv = .T.
		_vacnm="Output CGST"
	Case _fldnm	= "SGST_AMT" And _isadv = .T.
		_vacnm="Output SGST"
	Case _fldnm	= "IGST_AMT" And _isadv = .T.
		_vacnm="Output IGST"

	Case _fldnm = "CGST_AMT" And _isadv = .F.
		_vacnm="CGST Payable"
	Case _fldnm = "SGST_AMT" And _isadv = .F.
		_vacnm="SGST Payable"
	Case _fldnm = "IGST_AMT" And _isadv = .F.
		_vacnm="IGST Payable"

	Endcase
Endif
If !Empty(_vacnm)
	_vacacname = EtValidFindAcctName(_vacnm)
Endif
Return _vacacname

Procedure creditaceffect_S1
Parameters _fldnm
_fldnm		= Upper(_fldnm)
_vacnm      = ''
_vacacname  = ''
_isadv      = .F.
If Type('_mseravail') = 'C'
	If Inlist(_mseravail,"EXCISE","SERVICE")
		_isadv = .T.
	ENDIF
&& Added by Shrikant S. on 29/04/2017 for GST		&& Start	
ELSE
	IF IsUnderExport()
		_isadv = .T.
	ENDIF
&& Added by Shrikant S. on 29/04/2017 for GST		&& End	
Endif
Select acdetalloc_vw
If Seek(item_vw.itserial,'acdetalloc_vw','itserial')
	Do Case
	Case _fldnm	= "CGST_AMT" And _isadv = .T.
		_vacnm="CGST Payable"
	Case _fldnm	= "SGST_AMT" And _isadv = .T.
		_vacnm="SGST Payable"
	Case _fldnm	= "IGST_AMT" And _isadv = .T.
		_vacnm="IGST Payable"
	Endcase
Endif
If !Empty(_vacnm)
	_vacacname = EtValidFindAcctName(_vacnm)
Endif
Return _vacacname


Procedure act_deactcontrol_GC
Parameters oChoice
voupicked=.F.
If oChoice.Value=.T.

	If Used('OthItref_vw')
		Select Othitref_vw
		Scan For entry_ty=main_vw.entry_ty And tran_cd=main_vw.tran_cd
			voupicked=.T.
		Endscan
		If voupicked
			Ans=Messagebox("You have already picked the transaction."+Chr(13);
				+"In case of supplimentory, bill quan")

		Endif
	Endif
	oChoice.Enabled=.F.

Endif
Return

Procedure Get_gst_type

curObj=_Screen.ActiveForm
nhandle=0

lcsqlstr=""
lcsttype=""
If Type('main_vw.cons_id')<>'U'
	If Type('main_vw.scons_id')<>'U'
		If main_vw.scons_id >0
			lcsqlstr="select st_type,gstin,state from shipto where Ac_id=?main_vw.cons_id and Shipto_id=?main_vw.scons_id"
		Else
			lcsqlstr="select st_type,gstin,state from ac_mast where Ac_id=?main_vw.cons_id"
		Endif
	Else
		lcsqlstr="select st_type,gstin,state from ac_mast where Ac_id=?main_vw.cons_id"
	Endif
Else
	If Type('main_vw.sac_id')<>'U'
		If main_vw.sac_id >0
			lcsqlstr="select st_type,gstin,state from shipto where Ac_id=?main_vw.Ac_id and Shipto_id=?main_vw.sac_id"
		Else
			lcsqlstr="select st_type,gstin,state from ac_mast where Ac_id=?main_vw.Ac_id"
		Endif
	Else
		lcsqlstr="select st_type,gstin,state from ac_mast where Ac_id=?main_vw.Ac_id"
	Endif
Endif
nRet = curObj.sqlconobj.dataconn([EXE],company.dbname,lcsqlstr,[tmp_type],"nhandle",curObj.DataSessionId,.F.)

If nRet <= 0
	Return .F.
Endif
lcsttype= Alltrim(tmp_type.st_type)

Return lcsttype



Procedure IsUnderExport

curObj=_Screen.ActiveForm
nhandle=0
retval=.F.
lcsqlstr=""

If Type('main_vw.cons_id')<>'U'
	If Type('main_vw.scons_id')<>'U'
		If main_vw.scons_id >0
			lcsqlstr="select supp_type,st_type,gstin,state from shipto where Ac_id=?main_vw.cons_id and Shipto_id=?main_vw.scons_id"
		Else
			lcsqlstr="select supp_type,st_type,gstin,state from ac_mast where Ac_id=?main_vw.cons_id"
		Endif
	Else
		lcsqlstr="select supp_type,st_type,gstin,state from ac_mast where Ac_id=?main_vw.cons_id"
	Endif
Else
	If Type('main_vw.sac_id')<>'U'
		If main_vw.sac_id >0
			lcsqlstr="select supp_type,st_type,gstin,state from shipto where Ac_id=?main_vw.Ac_id and Shipto_id=?main_vw.sac_id"
		Else
			lcsqlstr="select supp_type,st_type,gstin,state from ac_mast where Ac_id=?main_vw.Ac_id"
		Endif
	Else
		lcsqlstr="select supp_type,st_type,gstin,state from ac_mast where Ac_id=?main_vw.Ac_id"
	Endif
Endif
nRet = curObj.sqlconobj.dataconn([EXE],company.dbname,lcsqlstr,[tmp_type],"nhandle",curObj.DataSessionId,.F.)

If nRet <= 0
	Return .F.
Endif
retval=Iif(Alltrim(tmp_type.supp_type)=="SEZ" Or Alltrim(tmp_type.st_type)=="OUT OF COUNTRY",.T.,.F.)

Return retval


Procedure act_deactcontrol_ST()
curObj=_Screen.ActiveForm

*!*	If (Get_gst_type()="OUT OF COUNTRY"  )
If IsUnderExport()
	itemcount=0
	Select item_vw
	lnrecno=Iif(!Eof(),Recno(),0)

	Count For !Deleted() To itemcount


	If lnrecno >0
		Select item_vw
		Go lnrecno
	Endif
	If itemcount> 0 And !Empty(lmc_vw.ExpoType)
		Return .F.
	Endif

	Return .T.
Else
	Replace ExpoType With "" In lmc_vw
	curObj.Refresh()
	Return .F.
Endif
Return

Procedure SetGSTColumns
If Pemstatus(_Screen.ActiveForm,"enable_disable_gst_columns",5)
	_Screen.ActiveForm.enable_disable_gst_columns(3)

	Select item_vw
	lnrecno=Iif(!Eof(),Recno(),0)

	If Type('Lmc_vw.ExpoType')<>'U'
		If Upper(Alltrim(lmc_vw.ExpoType))="WITHOUT IGST"
			Replace All IGST_PER With 0,igst_amt With 0 In item_vw

			Select item_vw
			Scan
				_Screen.ActiveForm.itemgrdbefcalc(1)
			Endscan
		Endif
	Endif

	If lnrecno >0
		Select item_vw
		Go lnrecno
	Endif
Endif
Return


Procedure DebitPosting_GD
Parameters _fldnm

Do Case
Case Type('_Screen.ActiveForm.pcvtype')<>'U'
	curObj=_Screen.ActiveForm
Case Type('_Screen.ActiveForm.objform')<>'U'
	curObj=_Screen.ActiveForm.objform
Case Type('_Screen.ActiveForm.ofrmfrom')<>'U'
	curObj=_Screen.ActiveForm.ofrmfrom
Endcase


*!*	nhandle=0
*!*	lcsqlstr=""
*!*	lcsqlstr="WITH Ac_Group(Gac_id, Ac_group_id,[AcgpName], Level)"
*!*	lcsqlstr=lcsqlstr+"	 AS "
*!*	lcsqlstr=lcsqlstr+"	 ( "
*!*	lcsqlstr=lcsqlstr+"	 SELECT Gac_id, Ac_group_id,[AcgpName]=[GROUP],0 AS Level FROM dbo.ac_Group_Mast AS e WHERE Ac_group_Name =(SELECT TOP 1 [Group] FROM Ac_mast "
*!*	lcsqlstr=lcsqlstr+"	 Where Ac_Name=?Main_vw.Party_nm)"
*!*	lcsqlstr=lcsqlstr+"	 UNION ALL "
*!*	lcsqlstr=lcsqlstr+"	 SELECT e.Gac_id, e.Ac_group_id,[AcgpName]=E.[GROUP],Level + 1 FROM ac_Group_Mast AS e INNER JOIN Ac_Group AS d ON (d.Gac_id = e.Ac_group_id)"
*!*	lcsqlstr=lcsqlstr+"	 Where e.[Group]<>''"
*!*	lcsqlstr=lcsqlstr+"	 )"
*!*	lcsqlstr=lcsqlstr+"	 SELECT Top 1 AcgpName FROM Ac_Group order by Level desc"

*!*	nRet = curObj.sqlconobj.dataconn([EXE],company.dbname,lcsqlstr,[acgroup],"nhandle",curObj.DataSessionId,.F.)
*!*	If nRet <= 0
*!*		Return .F.
*!*	Endif
_acname=""
_dacname=""
Do Case
Case Inlist(Alltrim(main_vw.againstgs),"SALES","SERVICE INVOICE")
	Do Case
	Case _fldnm="CGST_AMT"
		_dacname="CGST Payable"
	Case _fldnm="SGST_AMT"
		_dacname="SGST Payable"
	Case _fldnm="IGST_AMT"
		_dacname="IGST Payable"
	Endcase

Case Inlist(Alltrim(main_vw.againstgs),"PURCHASES","SERVICE PURCHASE BILL")
	Do Case
	Case _fldnm="CGST_AMT"
		_dacname="CGST Provisional"
	Case _fldnm="SGST_AMT"
		_dacname="SGST Provisional"
	Case _fldnm="IGST_AMT"
		_dacname="IGST Provisional"
	Endcase
Otherwise
	_acname=main_vw.party_nm
Endcase
If !Empty(_dacname)
	_dacname= EtValidFindAcctName(_dacname)
Else
	_dacname=_acname
Endif

Return _dacname

Procedure DebitPosting_GC
Parameters _fldnm

Do Case
Case Type('_Screen.ActiveForm.pcvtype')<>'U'
	curObj=_Screen.ActiveForm
Case Type('_Screen.ActiveForm.objform')<>'U'
	curObj=_Screen.ActiveForm.objform
Case Type('_Screen.ActiveForm.ofrmfrom')<>'U'
	curObj=_Screen.ActiveForm.ofrmfrom
Endcase



*!*	nhandle=0

*!*	lcsqlstr=""
*!*	lcsqlstr="WITH Ac_Group(Gac_id, Ac_group_id,[AcgpName], Level)"
*!*	lcsqlstr=lcsqlstr+"	 AS "
*!*	lcsqlstr=lcsqlstr+"	 ( "
*!*	lcsqlstr=lcsqlstr+"	 SELECT Gac_id, Ac_group_id,[AcgpName]=[GROUP],0 AS Level FROM dbo.ac_Group_Mast AS e WHERE Ac_group_Name =(SELECT TOP 1 [Group] FROM Ac_mast "
*!*	lcsqlstr=lcsqlstr+"	 Where Ac_Name=?Main_vw.Party_nm)"
*!*	lcsqlstr=lcsqlstr+"	 UNION ALL "
*!*	lcsqlstr=lcsqlstr+"	 SELECT e.Gac_id, e.Ac_group_id,[AcgpName]=E.[GROUP],Level + 1 FROM ac_Group_Mast AS e INNER JOIN Ac_Group AS d ON (d.Gac_id = e.Ac_group_id)"
*!*	lcsqlstr=lcsqlstr+"	 Where e.[Group]<>''"
*!*	lcsqlstr=lcsqlstr+"	 )"
*!*	lcsqlstr=lcsqlstr+"	 SELECT Top 1 AcgpName FROM Ac_Group order by Level desc"

*!*	nRet = curObj.sqlconobj.dataconn([EXE],company.dbname,lcsqlstr,[acgroup],"nhandle",curObj.DataSessionId,.F.)

*!*	If nRet <= 0
*!*		Return .F.
*!*	Endif

Do Case
Case Inlist(Alltrim(main_vw.againstgs),"SALES","SERVICE INVOICE")
	Do Case
	Case _fldnm="CGST_AMT"
		_dacname="CGST Payable"
	Case _fldnm="SGST_AMT"
		_dacname="SGST Payable"
	Case _fldnm="IGST_AMT"
		_dacname="IGST Payable"
	Endcase

Case Inlist(Alltrim(main_vw.againstgs),"PURCHASES","SERVICE PURCHASE BILL")
&& Added by Shrikant S. on 20/04/2017 for GST		&& Start
	If IsUnderImport()
		Do Case
		Case _fldnm="CGST_AMT"
			_dacname="Input CGST"
		Case _fldnm="SGST_AMT"
			_dacname="Input SGST"
		Case _fldnm="IGST_AMT"
			_dacname="Input IGST"
		Endcase
	Else
&& Added by Shrikant S. on 20/04/2017 for GST		&& End
		Do Case
		Case _fldnm="CGST_AMT"
			_dacname="CGST Provisional"
		Case _fldnm="SGST_AMT"
			_dacname="SGST Provisional"
		Case _fldnm="IGST_AMT"
			_dacname="IGST Provisional"
		Endcase
	Endif		&& Added by Shrikant S. on 20/04/2017 for GST
Otherwise
	_acname=main_vw.party_nm
Endcase

If Used('Acdetalloc_vw') And main_vw.entry_ty='C6'
	Select acdetalloc_vw
	If Seek(item_vw.itserial,'acdetalloc_vw','itserial')
		_vservCreditAvail = findallocserviceCreditAvail()	&&(No Service Tax Credit for Car and Cleaning Services).
&& (No Service Tax Credit for Car and Cleaning Services).
		If _vservCreditAvail = .F.
&& Commented by Shrikant S. on 10/02/2017 for GST		&& start
			Do Case
			Case _fldnm="CGST_AMT"
				_dacname="Input CGST"
			Case _fldnm="SGST_AMT"
				_dacname="Input SGST"
			Case _fldnm="IGST_AMT"
				_dacname="Input IGST"
			Endcase
&& Commented by Shrikant S. on 10/02/2017 for GST		&& End

&& Added by Shrikant S. on 10/02/2017 for GST		&& start
*!*				Do Case
*!*				Case _fldnm="CGST_AMT"
*!*					_dacname="CGST Provisional"
*!*				Case _fldnm="SGST_AMT"
*!*					_dacname="SGST Provisional"
*!*				Case _fldnm="IGST_AMT"
*!*					_dacname="IGST Provisional"
*!*				Endcase
&& Added by Shrikant S. on 10/02/2017 for GST		&& End

		Endif
&& (No Service Tax Credit for Car and Cleaning Services).

	Endif
Endif
If !Empty(_dacname)
	_dacname= EtValidFindAcctName(_dacname)
Else
	_dacname=_acname
Endif


Return _dacname


Procedure CreditPosting_GC
Parameters _fldnm,_acname


Do Case
Case Type('_Screen.ActiveForm.pcvtype')<>'U'
	curObj=_Screen.ActiveForm
Case Type('_Screen.ActiveForm.objform')<>'U'
	curObj=_Screen.ActiveForm.objform
Case Type('_Screen.ActiveForm.ofrmfrom')<>'U'
	curObj=_Screen.ActiveForm.ofrmfrom
Endcase


nhandle=0
_cacname=""
lcsqlstr="select top 1 st_type,gstin,state from ac_mast where Ac_id=?mac_id"

If Type('main_vw.sac_id')<>'U'
	If main_vw.sac_id >0
		lcsqlstr="select top 1 st_type,gstin,state from shipto where Ac_id=?mac_id and Shipto_id=?main_vw.sac_id"
	Else
		lcsqlstr="select top 1 st_type,gstin,state from ac_mast where Ac_id=?mac_id"
	Endif
Endif

If Type('main_vw.scons_id')<>'U'
	mac_id =main_vw.cons_id
	If main_vw.scons_id >0
		lcsqlstr="select top 1 st_type,gstin,state from shipto where Ac_id=?mac_id and Shipto_id=?main_vw.scons_id"
	Else
		lcsqlstr="select top 1 st_type,gstin,state from ac_mast where Ac_id=?mac_id"
	Endif
Endif

nRet = curObj.sqlconobj.dataconn([EXE],company.dbname,lcsqlstr,[tmptype],"nHandle",curObj.DataSessionId,.F.)
If nRet <= 0
	Return .F.
Endif

If (Upper(Alltrim(tmptype.st_type))="OUT OF COUNTRY" Or Upper(Alltrim(tmptype.gstin))="UNREGISTERED")  And Alltrim(main_vw.againstgs)# "SALES"
	_cacname=_acname
Endif

Return _cacname

Procedure CreditPosting_GD
Parameters _fldnm

Do Case
Case Type('_Screen.ActiveForm.pcvtype')<>'U'
	curObj=_Screen.ActiveForm
Case Type('_Screen.ActiveForm.objform')<>'U'
	curObj=_Screen.ActiveForm.objform
Case Type('_Screen.ActiveForm.ofrmfrom')<>'U'
	curObj=_Screen.ActiveForm.ofrmfrom
Endcase


*!*	nhandle=0
*!*	lcsqlstr=""
*!*	lcsqlstr="WITH Ac_Group(Gac_id, Ac_group_id,[AcgpName], Level)"
*!*	lcsqlstr=lcsqlstr+"	 AS "
*!*	lcsqlstr=lcsqlstr+"	 ( "
*!*	lcsqlstr=lcsqlstr+"	 SELECT Gac_id, Ac_group_id,[AcgpName]=[GROUP],0 AS Level FROM dbo.ac_Group_Mast AS e WHERE Ac_group_Name =(SELECT TOP 1 [Group] FROM Ac_mast "
*!*	lcsqlstr=lcsqlstr+"	 Where Ac_Name=?Main_vw.Party_nm)"
*!*	lcsqlstr=lcsqlstr+"	 UNION ALL "
*!*	lcsqlstr=lcsqlstr+"	 SELECT e.Gac_id, e.Ac_group_id,[AcgpName]=E.[GROUP],Level + 1 FROM ac_Group_Mast AS e INNER JOIN Ac_Group AS d ON (d.Gac_id = e.Ac_group_id)"
*!*	lcsqlstr=lcsqlstr+"	 Where e.[Group]<>''"
*!*	lcsqlstr=lcsqlstr+"	 )"
*!*	lcsqlstr=lcsqlstr+"	 SELECT Top 1 AcgpName FROM Ac_Group order by Level desc"

*!*	nRet = curObj.sqlconobj.dataconn([EXE],company.dbname,lcsqlstr,[acgroup],"nhandle",curObj.DataSessionId,.F.)
*!*	If nRet <= 0
*!*		Return .F.
*!*	Endif
_dacname=""
_acname=""
Do Case
Case  Inlist(Alltrim(main_vw.againstgs),"SALES","SERVICE INVOICE")
	Do Case
	Case _fldnm="CGST_AMT"
		_dacname="CGST Payable"
	Case _fldnm="SGST_AMT"
		_dacname="SGST Payable"
	Case _fldnm="IGST_AMT"
		_dacname="IGST Payable"
	Endcase

Case Inlist(Alltrim(main_vw.againstgs),"PURCHASES","SERVICE PURCHASE BILL")
&& Added by Shrikant S. on 20/04/2017 for GST		&& Start
	If IsUnderImport()
		Do Case
		Case _fldnm="CGST_AMT"
			_dacname="Input CGST"
		Case _fldnm="SGST_AMT"
			_dacname="Input SGST"
		Case _fldnm="IGST_AMT"
			_dacname="Input IGST"
		Endcase
	Else
&& Added by Shrikant S. on 20/04/2017 for GST		&& End
		Do Case
		Case _fldnm="CGST_AMT"
			_dacname="CGST Provisional"
		Case _fldnm="SGST_AMT"
			_dacname="SGST Provisional"
		Case _fldnm="IGST_AMT"
			_dacname="IGST Provisional"
		Endcase
	Endif
Otherwise
	_acname=main_vw.party_nm
Endcase
If !Empty(_dacname)
	_dacname= EtValidFindAcctName(_dacname)
Else
	_dacname=_acname
Endif

Return _dacname

Procedure Calculate_gst
Parameters oobject
lnreturn=0

If item_vw.AMTINCGST>0
	If  Val(oobject.Tag) <>oobject.Value
		lnreturn=Cal_gstexcl()
		Replace AMTINCGST With 0 In item_vw
	Endif
Else
	lnreturn=Cal_gstexcl()
Endif
Return lnreturn

&& Added by Shrikant S. on 04/10/2016 for GST	&& End


Procedure IsUnderImport

sqlconobj1 = Newobject('SqlConnUdObj','SqlConnection',xapps)
_etdatasessionid = _Screen.ActiveForm.DataSessionId
nhandle=0

lcsqlstr=""
llImport=.F.

If Type('main_vw.cons_id')<>'U'
	If Type('main_vw.scons_id')<>'U'
		If main_vw.scons_id >0
			lcsqlstr="select st_type,gstin,state from shipto where Ac_id=?main_vw.cons_id and Shipto_id=?main_vw.scons_id"
		Else
			lcsqlstr="select st_type,gstin,state from ac_mast where Ac_id=?main_vw.cons_id"
		Endif
	Else
		lcsqlstr="select st_type,gstin,state from ac_mast where Ac_id=?main_vw.cons_id"
	Endif
Else
	If Type('main_vw.sac_id')<>'U'
		If main_vw.sac_id >0
			lcsqlstr="select st_type,gstin,state from shipto where Ac_id=?main_vw.Ac_id and Shipto_id=?main_vw.sac_id"
		Else
			lcsqlstr="select st_type,gstin,state from ac_mast where Ac_id=?main_vw.Ac_id"
		Endif
	Else
		lcsqlstr="select st_type,gstin,state from ac_mast where Ac_id=?main_vw.Ac_id"
	Endif
Endif

nRet = sqlconobj1.dataconn([EXE],company.dbname,lcsqlstr,[tmp_type],"nhandle",_etdatasessionid ,.F.)
If nRet <= 0
	Return .F.
Endif

llImport= Iif(Alltrim(tmp_type.st_type)="OUT OF COUNTRY" Or Upper(Alltrim(tmp_type.gstin))="UNREGISTERED",.T.,.F.)		&& Commented by Shrikant S. on 22/02/2017 for GST
*!*	llImport= Iif(Alltrim(tmp_type.st_type)="OUT OF COUNTRY" ,.T.,.F.)			&& Added by Shrikant S. on 22/02/2017 for GST
=sqlconobj1.sqlconnclose("nhandle")

Return llImport


Procedure IsAgainstAdvance
_isadv =.F.
If Type('_mseravail') = 'C'
	If Inlist(_mseravail,"GOODS","SERVICE")
		_isadv = .T.
	Endi
Endif
Return _isadv

Procedure calc_totalgst
Replace gro_amt With main_vw.cgst_amt+main_vw.sgst_amt+main_vw.igst_amt,net_amt With main_vw.cgst_amt+main_vw.sgst_amt+main_vw.igst_amt  In main_vw
_Screen.ActiveForm.accountsposting()
Select Acdet_vw
mrecno=Iif(!Eof(),Recno(),0)

Select Acdet_vw
Locate
Replace ac_id With Acdet_vw.ac_id,party_nm With Acdet_vw.ac_name In main_vw
If mrecno>0
	Select Acdet_vw
	Go mrecno
Endif
Return

Procedure GetPrefix()
PARAMETERS lcEntry_ty

retval=""
curObj=_Screen.ActiveForm
If Type('lcEntry_ty')='C'
	retval=lcEntry_ty+"/"+Transform(Year(company.Sta_dt)-2000)+Transform(Year(company.end_dt)-2000)+"/"
ENDIF
Return retval
*!*	*!*	Procedure DebitPostingReversal
*!*	*!*	Parameters _fldnm
*!*	*!*	_vacnm=''
*!*	*!*	llpostReverse=.F.
*!*	*!*	If Inli(main_vw.entry_ty,"EP","E1","C6","BP","CP","IF","OF","B1")
*!*	*!*		llpostReverse=SerTaxRecApply("REC")
*!*	*!*	ENDIF
*!*	*!*	If llpostReverse=.T.
*!*	*!*		Do Case
*!*	*!*		Case _fldnm="CGSRT_AMT"
*!*	*!*			_vacnm="Input CGST"
*!*	*!*		Case _fldnm="SGSRT_AMT"
*!*	*!*			_vacnm="Input SGST"
*!*	*!*		Case _fldnm="IGSRT_AMT"
*!*	*!*			_vacnm="Input IGST"
*!*	*!*		Endcase
*!*	*!*	ENDIF
*!*	*!*	If !Empty(_vacnm)
*!*	*!*		_vacnm= EtValidFindAcctName(_vacnm)
*!*	*!*	Endif
*!*	*!*	Return _vacnm

*!*	*!*	Procedure CreditPostingReversal
*!*	*!*	Parameters _fldnm

*!*	*!*	_vacnm=''
*!*	*!*	llpostReverse=.F.
*!*	*!*	If Inli(main_vw.entry_ty,"EP","E1","C6","BP","CP","IF","OF","B1")
*!*	*!*		llpostReverse=SerTaxRecApply("REC")
*!*	*!*	Endif

*!*	*!*	If llpostReverse=.T.
*!*	*!*		Do Case
*!*	*!*		Case _fldnm="CGSRT_AMT"
*!*	*!*			_vacnm="CGST Payable"
*!*	*!*		Case _fldnm="SGSRT_AMT"
*!*	*!*			_vacnm="SGST Payable"
*!*	*!*		Case _fldnm="IGSRT_AMT"
*!*	*!*			_vacnm="IGST Payable"
*!*	*!*		Endcase
*!*	*!*	Endif
*!*	*!*	If !Empty(_vacnm)
*!*	*!*		_vacnm= EtValidFindAcctName(_vacnm)
*!*	*!*	Endif
*!*	*!*	Return _vacnm

&& Added by Suraj Kumawat for GST date on 24-04-2017  Start ...
Procedure chkDuplSerialNo_1
Lparameters _txtObj,_valCur,_CurRecond
Set Step On
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
	If (_txtObj.Parent.Parent.Parent.oParForm.IsBarCode =.T.)
		If (Alltrim(_txtObj.Tag) <> Alltrim(_valCur))
			lcfld_nm=""
			lcfld_val=""
			Select BarCodeSrNo_vw
			Locate
			Locate For Alltrim(Inventsrno)==Alltrim(_txtObj.Tag)
			If Found()
				lcfld_nm=Alltrim(BarCodeSrNo_vw.fld_nm)
				_mBCRetValue = _curform.oParForm.BarCodeObj.RemLine('S')
				If _txtObj.Parent.Parent.Parent.oParForm.Addmode
					Delete In BarCodeSrNo_vw
				Endif
			Endif

			Select BarCodeMast_vw
			lnrecno=Iif(!Eof(),Recno(),0)
			If (lnrecno > 0)
				Select BarCodeMast_vw
				Go lnrecno
			Else
				Locate
			Endif
			lcfld_nm="Alltrim("+Alltrim(BarCodeMast_vw.Fld_Nm2)+")"
			If !Empty(lcfld_nm)
				Select BarCodeTran_vw
				Locate
				Locate For &lcfld_nm==Alltrim(_txtObj.Tag)
				If Found()
					If _txtObj.Parent.Parent.Parent.oParForm.Addmode
						Delete In BarCodeTran_vw
					Endif
				Endif
			Endif
		Endif
	Endif
Endif
Return .T.
Endproc
&& Added by Suraj Kumawat for GST date on 24-04-2017  End ...


&& Added by Shrikant S. on 29/04/2017 for GST		&& Start
Procedure checkbatch_Valid
_curform=_screen.ActiveForm

_curretval = _curform.sqlconobj.dataconn([EXE],company.dbname,"select top 1 BATCHVALID from It_mast where It_code=?Item_vw.it_code","_Tmpval","This.Parent.nHandle",_curform.DataSessionId,.F.)
_curform.sqlconobj.sqlconnclose("This.Parent.nHandle")
If _curretval > 0 And Used('_Tmpval')
	Select _Tmpval
	RETURN _Tmpval.batchvalid
Endif
Return .F.
ENDPROC
&& Added by Shrikant S. on 29/04/2017 for GST		&& Edn
