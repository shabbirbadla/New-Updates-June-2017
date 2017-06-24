If Type('item_vw.u_pageno')="C" And [vuexc] $ vchkprod
	Replace u_pageno With "" For !Empty(dc_no) In item_vw
Endif

&& Sandeep s.---->Start for TKT-4596
If  main_Vw.entry_ty="SR"  And  [vuexc] $ vchkprod
	If Type('item_vw.u_pageno')="C"
		Replace All u_pageno With ""  In item_vw
	Endif
Endif
&& Sandeep s.---->End for TKT-4596

&& Added By Shrikant S. on 05/11/2011 for Bug-259	&&Start
If Inlist(main_Vw.entry_ty,"S1")
	oFrm=_Screen.ActiveForm
	_datasessionid=_Screen.ActiveForm.DataSessionId
	If oFrm.Addmode Or oFrm.Editmode

		sql_con = oFrm.SqlConObj.DataConn([EXE],Company.DbName,[select * from AcdetAlloc where Entry_ty =?Main_vw.Entry_ty and Tran_cd=?Main_vw.Tran_cd ],[AcdetAlloc_vw],;
			"This.Parent.nHandle",_datasessionid,.F.)
		If sql_con<0
			Return .F.
		Endif
		Select AcdetAlloc_vw
		Index On ItSerial Tag ItSerial AddI
		Index On serty Tag serty AddI
		If Used('Itref_vw')
			Select Itref_vw
			mrecno=Iif(!Eof(),Recno(),0)
			Scan
				If Used('Acalloc_vw')
					Use In Acalloc_vw
				Endif
				sql_con = oFrm.SqlConObj.DataConn([EXE],Company.DbName,[select * from AcdetAlloc where Entry_ty =?Itref_vw.rEntry_ty and Tran_cd=?Itref_vw.itref_tran and itserial=?Itref_vw.ritserial],[Acalloc_vw],;
					"This.Parent.nHandle",_datasessionid,.F.)

				Select Acalloc_vw
				Replace entry_ty With Itref_vw.entry_ty,tran_cd With Itref_vw.tran_cd,ItSerial With Itref_vw.ItSerial In Acalloc_vw

				morg_qty=0
				mqty=0

				Select org_qty,qty From Detail Where entry_ty=Itref_vw.rentry_ty And tran_cd=Itref_vw.itref_tran And ItSerial=Itref_vw.rItSerial Into Array qtyarr
				morg_qty=qtyarr[1]
				mqty=qtyarr[2]
				qtyarr=Null

				Select item_vw
				Scan For ItSerial==Itref_vw.ItSerial
					Replace sTaxAmt With (Acalloc_vw.Amount/morg_qty)*mqty,SerbAmt With (Acalloc_vw.SerbAmt/morg_qty)*mqty;
						,SercAmt With (Acalloc_vw.SercAmt/morg_qty)*mqty,SerhAmt With (Acalloc_vw.SerhAmt/morg_qty)*mqty In item_vw
					Exit
				Endscan

				Select AcdetAlloc_vw
				Locate For entry_ty=Itref_vw.entry_ty And tran_cd=Itref_vw.tran_cd And ItSerial=Itref_vw.ItSerial
				If !Found()
					Select AcdetAlloc_vw
					Append Blank In AcdetAlloc_vw
					Replace entry_ty With Acalloc_vw.entry_ty, tran_cd With Acalloc_vw.tran_cd,serty With Acalloc_vw.serty,Amount With (Acalloc_vw.Amount/morg_qty)*mqty;
						sAbtPer With Acalloc_vw.sAbtPer, sAbtAmt With Acalloc_vw.sAbtAmt,sTaxable With (Acalloc_vw.sTaxable/morg_qty)*mqty, serAvail With Acalloc_vw.serAvail;
						SerbAmt With item_vw.SerbAmt, SercAmt With item_vw.SercAmt,SerhAmt With item_vw.SerhAmt,Sexpamt With Acalloc_vw.Sexpamt;
						SabtSr With Acalloc_vw.SabtSr, SsubCls With Acalloc_vw.SsubCls,SExNoti With Acalloc_vw.SExNoti,ItSerial With Acalloc_vw.ItSerial In AcdetAlloc_vw
				Endif

				Select Itref_vw
			Endscan
			If mrecno>0
				Select Itref_vw
				Go mrecno
			Endif
		Endif
		Select item_vw
		Go Top
		Select AcdetAlloc_vw
		Go Top

	Endif
Endif
&& Added By Shrikant S. on 05/11/2011 for Bug-259	&&End



&& Added by Shrikant S. on 20/12/2014 for Bug-24961		&& Start
If main_Vw.entry_ty='ST' And 'vutex' $ vchkprod
	If Used('Detail')
		If Type('Detail.cvdpass')!='U'
			Select Detail
			mrecno=Iif(!Eof(),Recno(),0)

			llcvdpass=.T.
			Locate
			Scan For Detail.cvdpass=.F. And Detail.u_cvdamt>0 And Detail.ticked=.T. And Detail.entry_ty="DC"
				Replace u_cvdamt With 0 In Detail
			Endscan

			If mrecno>0
				Select Detail
				Go mrecno
			Endif
		Endif
	Endif
Endif
&& Added by Shrikant S. on 20/12/2014 for Bug-24961		&& End

&& Added by Pankaj B. on 01-04-2015 for Bug-25746 Start
If ("exim" $ vchkprod) Or ("dbk" $ vchkprod) Then
	oFrm=_Screen.ActiveForm
	_datasessionid=_Screen.ActiveForm.DataSessionId
	If oFrm.Addmode Or oFrm.Editmode
		sql_con = oFrm.SqlConObj.DataConn([EXE],Company.DbName,[select * from Container_det where Entry_ty =?header.Entry_ty and Tran_cd=?header.Tran_cd],[Container_vw],;
			"This.Parent.nHandle",_datasessionid,.F.)
		If sql_con<0
			Return .F.
		Endif
		_mrecno=Iif(!Eof(),Recno(),0)

		If 	_mrecno!=0 Then
			Replace All tran_cd With main_Vw.tran_cd In Container_vw
			Replace All entry_ty With main_Vw.entry_ty In Container_vw
		Endif
	Endif
Endif

&& Added by Pankaj B. on 01-04-2015 for Bug-25746 End


&& Added By Shrikant S. on 28/11/2016 for GST	&&Start
If Inlist(_Screen.ActiveForm.pcvtype,"SR","PR","GC","GD","C6","D6") And ("vugst" $ vchkprod)
	oFrm=_Screen.ActiveForm
	_datasessionid=_Screen.ActiveForm.DataSessionId
	If (oFrm.Addmode Or oFrm.Editmode) And Type('Item_vw.sbillno')<>'U'

		If Used('Itref_vw')
			Select Itref_vw
			mrecno=Iif(!Eof(),Recno(),0)
			Scan
*!*					sql_con = oFrm.SqlConObj.DataConn([EXE],Company.DbName,[select u_pinvno,u_pinvdt from Lmain_vw where Entry_ty =?Itref_vw.rEntry_ty and Tran_cd=?Itref_vw.itref_tran ],[tmplmain],		&& Commented by Shrikant S. on 19/01/2017 for GST
				sql_con = oFrm.SqlConObj.DataConn([EXE],Company.DbName,[select pinvno,pinvdt from Lmain_vw where Entry_ty =?Itref_vw.rEntry_ty and Tran_cd=?Itref_vw.itref_tran ],[tmplmain],;		&& Added by Shrikant S. on 19/01/2017 for GST
				"This.Parent.nHandle",_datasessionid,.F.)


				Select item_vw
				Scan For ItSerial==Itref_vw.ItSerial
*!*						Replace sbillno With tmplmain.u_pinvno,sbdate With tmplmain.u_pinvdt In item_vw		&& Commented by Shrikant S. on 19/01/2017 for GST
					Replace sbillno With tmplmain.pinvno,sbdate With tmplmain.pinvdt In item_vw			&& Added by Shrikant S. on 19/01/2017 for GST
					Exit
				Endscan


				Select Itref_vw
			Endscan
			If mrecno>0
				Select Itref_vw
				Go mrecno
			Endif
		Endif

		If Used('OthItref_vw')
			Select OthItref_vw
			mrecno=Iif(!Eof(),Recno(),0)
			Scan
*!*					sql_con = oFrm.SqlConObj.DataConn([EXE],Company.DbName,[select u_pinvno,u_pinvdt from Lmain_vw where Entry_ty =?Itref_vw.rEntry_ty and Tran_cd=?Itref_vw.itref_tran ],[tmplmain],		&& Commented by Shrikant S. on 19/01/2017 for GST
				sql_con = oFrm.SqlConObj.DataConn([EXE],Company.DbName,[select pinvno,pinvdt from Lmain_vw where Entry_ty =?OthItref_vw.rEntry_ty and Tran_cd=?OthItref_vw.itref_tran ],[tmplmain],;		&& Added by Shrikant S. on 19/01/2017 for GST
				"This.Parent.nHandle",_datasessionid,.F.)


				Select item_vw
				Scan For ItSerial==OthItref_vw.ItSerial
*!*						Replace sbillno With tmplmain.u_pinvno,sbdate With tmplmain.u_pinvdt In item_vw		&& Commented by Shrikant S. on 19/01/2017 for GST
					Replace sbillno With tmplmain.pinvno,sbdate With tmplmain.pinvdt In item_vw			&& Added by Shrikant S. on 19/01/2017 for GST
					Exit
				Endscan


				Select OthItref_vw
			Endscan
			If mrecno>0
				Select OthItref_vw
				Go mrecno
			Endif
		Endif

		Select item_vw
		Go Top

	Endif
Endif

&& Added by Shrikant S. on 07/02/2017 for GST		&& Start
*!*	If Inlist(_Screen.ActiveForm.pcvtype,"GC","GD","C6","D6") And ("vugst" $ vchkprod)
*!*		If (_Screen.ActiveForm.Addmode Or _Screen.ActiveForm.Editmode) And Allt(main_Vw.againstgs)=="SERVICES"
*!*			If !Used('Acdetalloc_Vw')
*!*				msqlstr="select * from Acdetalloc where Entry_ty =?Main_vw.Entry_ty and Tran_cd=?Main_Vw.Tran_cd "
*!*				sql_con = oFrm.SqlConObj.DataConn([EXE],Company.DbName,msqlstr,[Acdetalloc_Vw],"This.Parent.nHandle",_datasessionid,.F.)

*!*				Select AcdetAlloc_vw
*!*				Index On ItSerial Tag ItSerial
*!*				Index On serty Tag serty
*!*			Endif


*!*			If Used("Acdetalloc_vw")

*!*				Select item_vw
*!*				COPY TO "d:\item.dbf"
*!*				Scan

*!*					Select AcdetAlloc_vw
*!*					Set Order To
*!*					Locate For entry_ty=main_Vw.entry_ty And tran_cd=main_Vw.tran_cd And ItSerial=item_vw.ItSerial
*!*					If !Found()
*!*						Append Blank In AcdetAlloc_vw
*!*						Replace entry_ty With main_Vw.entry_ty, tran_cd With main_Vw.tran_cd,ItSerial With item_vw.ItSerial,serty With item_vw.serty,Amount With item_vw.u_asseamt ;
*!*							,sAbtPer With item_vw.sAbtPer,sAbtAmt With item_vw.sAbtAmt,sTaxable With item_vw.sTaxable;
*!*							,Sexpamt With item_vw.Sexpamt,SabtSr With item_vw.SabtSr,SsubCls With item_vw.SsubCls,SExNoti With item_vw.SExNoti;
*!*							,Amountin With 0,SEXNOTISL With item_vw.SEXNOTISL,SABTSRSL With item_vw.SABTSRSL;
*!*							,cgst_amt With item_vw.cgst_amt,sgst_amt With item_vw.sgst_amt,igst_amt With item_vw.igst_amt;
*!*							,cgsrt_amt With item_vw.cgsrt_amt,sgsrt_amt With item_vw.sgsrt_amt,igsrt_amt With item_vw.igsrt_amt;
*!*							In AcdetAlloc_vw
*!*					Else
*!*						Replace Amount With item_vw.u_asseamt ;
*!*							,sAbtPer With item_vw.sAbtPer,sAbtAmt With item_vw.sAbtAmt,sTaxable With item_vw.u_asseamt,Amountin With 0 ;
*!*							,cgst_amt With item_vw.cgst_amt,sgst_amt With item_vw.sgst_amt,igst_amt With item_vw.igst_amt;
*!*							,cgsrt_amt With item_vw.cgsrt_amt,sgsrt_amt With item_vw.sgsrt_amt,igsrt_amt With item_vw.igsrt_amt;
*!*							In AcdetAlloc_vw

*!*					ENDIF
*!*					Select item_vw
*!*				Endscan
*!*			Endif

*!*		Endif
*!*	Endif
&& Added by Shrikant S. on 07/02/2017 for GST		&& End

If Inlist(_Screen.ActiveForm.pcvtype,"SO","PO") And ("vugst" $ vchkprod)
	oFrm=_Screen.ActiveForm
	_datasessionid=_Screen.ActiveForm.DataSessionId
	If oFrm.Addmode Or oFrm.Editmode

&& Added by Shrikant S. on 04/04/2017 for GST		&& Start
		If Type('Main_vw.gstscode')<>'U' And Type('Header.gstscode')='U'
			Select Header
			Scan
				If Header.ticked = .T.
					If Type('Main_vw.Cons_id') = 'N'
						Replace Cons_id With Iif(Header.Cons_id > 0,Header.Cons_id,main_Vw.Ac_id) In main_Vw

						If Type('Main_vw.Scons_id') = 'N'
							Replace Scons_id With Iif(Header.Scons_id > 0,Header.Scons_id,main_Vw.Sac_id) In main_Vw
						Endif
					Endif
				Endif
				Select Header
			Endscan

			lnconsid=0
			lnsconsid=0
			If Type('Main_vw.Cons_id')<>'U'
				lnconsid=main_Vw.Cons_id
			Endif
			If Type('Main_vw.sCons_id')<>'U'
				lnsconsid=main_Vw.Scons_id
			Endif
			If lnsconsid>0
				lcstr="Select Top 1 State,Statecode From Shipto Where Ac_id=?lnconsid and Shipto_Id=?lnsconsid"
			Else
				lcstr="Select Top 1 State,Statecode From Shipto Where Ac_id=?lnconsid "
			Endif
			sql_con = oFrm.SqlConObj.DataConn([EXE],Company.DbName,lcstr,[statedet],"This.Parent.nHandle",_datasessionid,.F.)
			If sql_con<0
				Return .F.
			Endif
			Replace gstscode With statedet.Statecode,gststate With statedet.State In main_Vw
			IF TYPE('oFrm.txtState')='O'
				oFrm.txtState.Value=statedet.State
			endif
		Endif
&& Added by Shrikant S. on 04/04/2017 for GST		&& End

		If Used('Itref_vw')
			Select Itref_vw
			mrecno=Iif(!Eof(),Recno(),0)

			Scan For Inlist(Itref_vw.rentry_ty,"SQ","PD","EQ")		&& Commented by Shrikant S. on 14/06/2017 for GST
*!*				Scan 
				Select item_vw
				Scan For ItSerial==Itref_vw.ItSerial
				
					oFrm.get_gst_rate_statewise()

*!*						mac_id =main_Vw.ac_id

*!*						lcsqlstr="select top 1 state,gstin,st_type from ac_mast where Ac_id=?mac_id"

*!*						If Type('main_vw.sac_id')<>'U'
*!*							If main_Vw.sac_id >0
*!*								lcsqlstr="select top 1 state,gstin,st_type from shipto where Ac_id=?mac_id and Shipto_id=?main_vw.sac_id"
*!*							Else
*!*								lcsqlstr="select top 1 state,gstin,st_type from ac_mast where Ac_id=?mac_id"
*!*							Endif
*!*						Endif

*!*						If Type('main_vw.scons_id')<>'U'
*!*							mac_id =main_Vw.cons_id
*!*							If main_Vw.scons_id >0
*!*								lcsqlstr="select top 1 state,gstin,st_type from shipto where Ac_id=?mac_id and Shipto_id=?main_vw.scons_id"
*!*							Else
*!*								lcsqlstr="select top 1 state,gstin,st_type from ac_mast where Ac_id=?mac_id"
*!*							Endif
*!*						Endif

*!*						nRet = oFrm.SqlConObj.DataConn([EXE],Company.DbName,lcsqlstr,[tmpst],;
*!*							"oFrm.nHandle",oFrm.DataSessionId,.F.)
*!*						If nRet <= 0
*!*							Return .F.
*!*						Endif

*!*						lcsqlstr="select gst_state_code as state_code from state where state=?tmpst.state"
*!*						nRet = oFrm.SqlConObj.DataConn([EXE],Company.DbName,lcsqlstr,[tmp_state],;
*!*							"oFrm.nHandle",oFrm.DataSessionId,.F.)
*!*						If nRet <= 0
*!*							Return .F.
*!*						Endif

*!*						lcsqlstr="Select top 1 hsncode,isservice,hsn_id,serty from it_mast where it_name=?item_vw.item"
*!*						nRet = oFrm.SqlConObj.DataConn([EXE],Company.DbName,lcsqlstr,[tmp],;
*!*							"oFrm.nHandle",oFrm.DataSessionId,.F.)
*!*						If nRet <= 0
*!*							Return .F.
*!*						Endif

*!*						If !Empty(tmp.hsncode) And tmp.isservice<>.T.
*!*							Select tmp_state
*!*							lcsqlstr="Select sgstper,cgstper,igstper from hsncodemast Where hsnid=?tmp.hsn_id and state_code=?tmp_state.state_code and Activefrom < ?Main_vw.Date+1 order by Activefrom desc"
*!*							nRet = oFrm.SqlConObj.DataConn([EXE],Company.DbName,lcsqlstr,[tmphsn],;
*!*								"oFrm.nHandle",oFrm.DataSessionId,.F.)
*!*							If nRet <= 0
*!*								Return .F.
*!*							Endif

*!*							If Reccount('tmphsn')>0
*!*								Replace sgst_per With tmphsn.sgstper, cgst_per With tmphsn.cgstper,igst_per With tmphsn.igstper In item_vw

*!*								If Type('item_vw.Exemptype')<>'U'
*!*									If (tmphsn.sgstper+tmphsn.cgstper+tmphsn.igstper)<=0
*!*										Replace Exemptype With "Exempted" In item_vw
*!*									Endif
*!*								Endif
*!*							Endif
*!*						Endif

*!*						If !Empty(tmp.hsncode) And tmp.isservice=.T.
*!*							msqlstr="select Name,Abt_per,CGST_PER,IGST_PER,SGST_PER From SerTax_Mast where [name]=?tmp.serty and (?main_vw.date between sdate and edate) and charindex(?main_vw.entry_ty,validity)>0 "
*!*							nRet = oFrm.SqlConObj.DataConn([EXE],Company.DbName,msqlstr,[tmphsn],"oFrm.nHandle",oFrm.DataSessionId,.F.)
*!*							If nRet <= 0
*!*								Return .F.
*!*							Endif

*!*							If Reccount('tmphsn')>0
*!*								Replace sgst_per With tmphsn.sgst_per, cgst_per With tmphsn.cgst_per,igst_per With tmphsn.igst_per In item_vw

*!*								If Type('item_vw.Exemptype')<>'U'
*!*									If (tmphsn.sgst_per+tmphsn.cgst_per +tmphsn.igst_per)<=0
*!*										Replace Exemptype With "Exempted" In item_vw
*!*									Endif
*!*								Endif
*!*								If Type('item_vw.serty')<>'U'
*!*									Replace serty With tmphsn.Name In item_vw
*!*								Endif
*!*							Endif
*!*						Endif


*!*						Do Case
*!*						Case Inlist(Alltrim(tmpst.st_type),"INTRASTATE") Or Empty(tmpst.gstin)
*!*							If Type('item_vw.IGST_AMT')<>'U'
*!*								Replace IGST_AMT With 0 In item_vw
*!*							Endif
*!*							If Type('item_vw.IGST_PER')<>'U'
*!*								Replace igst_per With 0 In item_vw
*!*							Endif
*!*							lnCase=1
*!*						Case Inlist(Alltrim(tmpst.st_type),"INTERSTATE","OUT OF COUNTRY")

*!*							If Type('item_vw.CGST_AMT')<>'U'
*!*								Replace CGST_AMT With 0 In item_vw
*!*							Endif
*!*							If Type('item_vw.CGST_PER')<>'U'
*!*								Replace cgst_per With 0 In item_vw
*!*							Endif
*!*							If Type('item_vw.SGST_AMT')<>'U'
*!*								Replace SGST_AMT With 0 In item_vw
*!*							Endif
*!*							If Type('item_vw.SGST_PER')<>'U'
*!*								Replace sgst_per With 0 In item_vw
*!*							Endif
*!*							lnCase=2
*!*						Endcase

*!*						If Upper(Alltrim(tmpst.gstin))="UNREGISTERED"
*!*							Replace IGST_AMT With 0,igst_per With 0,CGST_AMT With 0,cgst_per With 0,SGST_AMT With 0,sgst_per With 0 In item_vw
*!*							If Type('item_vw.IGST_AMT')<>'U'
*!*								Replace IGST_AMT With 0 In item_vw
*!*							Endif
*!*							If Type('item_vw.IGST_PER')<>'U'
*!*								Replace igst_per With 0 In item_vw
*!*							Endif
*!*							If Type('item_vw.CGST_AMT')<>'U'
*!*								Replace CGST_AMT With 0 In item_vw
*!*							Endif
*!*							If Type('item_vw.CGST_PER')<>'U'
*!*								Replace cgst_per With 0 In item_vw
*!*							Endif
*!*							If Type('item_vw.SGST_AMT')<>'U'
*!*								Replace SGST_AMT With 0 In item_vw
*!*							Endif
*!*							If Type('item_vw.SGST_PER')<>'U'
*!*								Replace sgst_per With 0 In item_vw
*!*							Endif
*!*						Endif


				Endscan
				Select Itref_vw
			Endscan
			If mrecno>0
				Select Itref_vw
				Go mrecno
			Endif
		Endif

		Select item_vw
		Go Top

	Endif
Endif
&& Added By Shrikant S. on 28/11/2016 for GST	&&End

&& Added by Shrikant S. on 23/02/2017 for GST		&& Start
If Inlist(_Screen.ActiveForm.pcvtype,"GC","GD") And ("vugst" $ vchkprod)
	If Type('Item_vw.actRate')<>'U'
		Select item_vw
		mrecno=Iif(!Eof(),Recno(),0)
		Replace All ActRate With Rate In item_vw

		If mrecno >0
			Select item_vw
			Go mrecno
		Endif

	Endif
Endif
If Inlist(_Screen.ActiveForm.pcvtype,"C6","D6") And ("vugst" $ vchkprod)
	If Type('Item_vw.ACTTAXAVAL')<>'U'
		Select item_vw
		mrecno=Iif(!Eof(),Recno(),0)
		Replace All ACTTAXAVAL With Rate In item_vw

		If mrecno >0
			Select item_vw
			Go mrecno
		Endif

	Endif
Endif

&& Added by Shrikant S. on 23/02/2017 for GST		&& End

