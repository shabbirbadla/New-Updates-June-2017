ldebitaccount=""
lcreditaccount=""

If Tmp_Lcode.v_account
	If !Empty(Tmp_Lcode.defa_db)
		ldebitaccount=Evaluate(Tmp_Lcode.defa_db)
		If Empty(ldebitaccount)
			Return .F.
		Endif
	Endif
	If !Empty(Tmp_Lcode.defa_cr)
		lcreditaccount=Evaluate(Tmp_Lcode.defa_cr)
		If Empty(lcreditaccount)
			Return .F.
		Endif
	Endif
Endif


lnSrno = 0
If !Empty(ldebitaccount)
	lnPrimary = This.Findidbyname("AC_MAST","Ac_id","[Ac_Name] = ?ldebitaccount")
	If Empty(lnPrimary) Or Isnull(lnPrimary)
		Return .F.
	Endif
	lnSrno = 1
	Select Tmp_acdet
	Append Blan
	Replace Entry_ty With Thisform.Entry_ty,Date With Tmp_main.Date, doc_no With Tmp_main.doc_no, inv_no With Tmp_main.inv_no, ;
		l_yn With Tmp_main.l_yn, ac_Name With ldebitaccount;
		,Ac_Id With lnPrimary,Amount With sumNetAmt,Dept With Tmp_main.Dept, Cate With Tmp_main.Cate,amt_ty With "DR";
		,PostOrd With Iif(Tmp_main.Party_nm=ldebitaccount,[A],[B]),acserial With Padl(lnSrno,5,"0"),compid With company.compid In Tmp_acdet
Endif

If !Empty(lcreditaccount)
	lnPrimary = This.Findidbyname("AC_MAST","Ac_id","[Ac_Name] = ?lcreditaccount")
	If Empty(lnPrimary) Or Isnull(lnPrimary)
*!*			This.cError_desc = Alltrim(lcreditaccount)+" : Account Code Not Found In [Account Master]"
*!*			This.make_log_file(This.Errlogfile,Space(2)+Padr(Alltrim(lcreditaccount),35," ")+Space(2)+Padr(Alltrim(This.cError_desc) ,55," "))
		Return .F.
	Endif
	lnSrno = 2
	Select Tmp_acdet
	Append Blan
	Replace Entry_ty With Thisform.Entry_ty,Date With Tmp_main.Date, doc_no With Tmp_main.doc_no, inv_no With Tmp_main.inv_no, ;
		l_yn With Tmp_main.l_yn, ac_Name With lcreditaccount;
		,Ac_Id With lnPrimary,Amount With sumNetAmt,Dept With Tmp_main.Dept, Cate With Tmp_main.Cate,amt_ty With "CR";
		,PostOrd With Iif(Tmp_main.Party_nm=lcreditaccount,[A],[B]),acserial With Padl(lnSrno,5,"0"),compid With company.compid In Tmp_acdet
Endif
If Empty(lcreditaccount) And Empty(ldebitaccount)
	If Used('Tmp_DcMast')
		Select Tmp_DcMast
		If Reccount()>0
			Scan
				recAppend=.T.
				ldebitaccount=Alltrim(Evaluate(Tmp_DcMast.dac_name))
				fldVal="Tmp_Main."+Alltrim(Tmp_DcMast.fld_nm)
				sumNetAmt=&fldVal
				If sumNetAmt <=0
					Loop
				Endif
				If !Empty(ldebitaccount)
					lnPrimary = This.Findidbyname("AC_MAST","Ac_id","[Ac_Name] = ?ldebitaccount")
					If Empty(lnPrimary) Or Isnull(lnPrimary)
						Return .F.
					Endif
					lnSrno =lnSrno + 1
					Select Tmp_acdet
					Scan For Alltrim(ac_Name)==Alltrim(ldebitaccount)
						Replace Amount With Amount+sumNetAmt
						recAppend=.F.
					Endscan
					If recAppend
						Select Tmp_acdet
						Append Blan
						Replace Entry_ty With Thisform.Entry_ty,Date With Tmp_main.Date, doc_no With Tmp_main.doc_no, inv_no With Tmp_main.inv_no, ;
							l_yn With Tmp_main.l_yn, ac_Name With ldebitaccount;
							,Ac_Id With lnPrimary,Amount With sumNetAmt,Dept With Tmp_main.Dept, Cate With Tmp_main.Cate,amt_ty With "DR";
							,PostOrd With Iif(Tmp_main.Party_nm=ldebitaccount,[A],[C0]),acserial With Padl(lnSrno,5,"0"),compid With company.compid In Tmp_acdet
					Endif
				Endif
				recAppend=.T.
				lcreditaccount=Alltrim(Evaluate(Tmp_DcMast.cac_name))
				If !Empty(lcreditaccount)
					lnPrimary = This.Findidbyname("AC_MAST","Ac_id","[Ac_Name] = ?lcreditaccount")
					If Empty(lnPrimary) Or Isnull(lnPrimary)
						Return .F.
					Endif
					lnSrno =lnSrno + 1
					Select Tmp_acdet
					Scan For Alltrim(ac_Name)==Alltr(lcreditaccount)
						Replace Amount With Amount+sumNetAmt
						recAppend=.F.
					Endscan
					If recAppend
						Select Tmp_acdet
						Append Blan
						Replace Entry_ty With Thisform.Entry_ty,Date With Tmp_main.Date, doc_no With Tmp_main.doc_no, inv_no With Tmp_main.inv_no, ;
							l_yn With Tmp_main.l_yn, ac_Name With lcreditaccount;
							,Ac_Id With lnPrimary,Amount With sumNetAmt,Dept With Tmp_main.Dept, Cate With Tmp_main.Cate,amt_ty With "CR";
							,PostOrd With Iif(Tmp_main.Party_nm=lcreditaccount,[A],[CE]),acserial With Padl(lnSrno,5,"0"),compid With company.compid In Tmp_acdet
					Endif
				Endif
			Endscan
		Endif
	Endif
Endif
If !Empty(lcreditaccount) And !Empty(ldebitaccount)
	Local Dbi,Crj,lDbi,lCrj,CrLf  &&
	CrLf=Chr(13)+Chr(10)
	Store 1 To Dbi && Crj,Dbi
	Dime DbArray[Dbi]
	DbArray[Dbi]=[]

	Select Tmp_acdet
	Scan
		Dime DbArray[Dbi]
		DbArray[Dbi]=ac_Name+Allt(Str(Amount,17,2))+amt_ty
		Dbi=Dbi+1
	Endscan

	Select Tmp_acdet
	Scan
		Replace oac_name With [] In Tmp_acdet
		_FirstName = 1
		For lDbi= 1 To Dbi-1
			If Substr(DbArray[lDbi],1,Len(Tmp_acdet.ac_Name))!= Tmp_acdet.ac_Name
				Replace oac_name With Iif(_FirstName = 1,'',oac_name+CrLf)+DbArray[lDbi] In Tmp_acdet
				_FirstName = _FirstName + 1
			Endif
		Endfor
	Endscan
Endif

Return .T.

