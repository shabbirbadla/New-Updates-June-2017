_curvouobj = _Screen.ActiveForm 
etsql_con = 1
If _curvouobj.ItemPage Or Inlist(Main_Vw.Entry_ty,"RR","RP","GI","GR","HI","HR") 	&& Changed by Shrikant S. on 29/09/2010 for TKT-4021
	If _curvouobj.AddMode Or _curvouobj.EditMode
		If Used('Gen_SrNo_Vw')
			Select Gen_SrNo_Vw
			Replace All Tran_cd With Main_Vw.Tran_cd,Entry_ty With Main_Vw.Entry_ty,;
				Date With Main_Vw.Date,CompId With Main_Vw.CompId,l_yn With Main_Vw.l_yn In Gen_SrNo_Vw
*npgno,cit_code,cware,cgroup,cchapno
*itserial

			etsql_str  = "Select * from Gen_SrNo Where l_yn = ?main_vw.l_yn and Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
			etsql_con  = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[TmpEt_Vw],;
				"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
			If etsql_con > 0 And Used('TmpEt_Vw')
				Select TmpEt_Vw
				Scan
					metdele = .F.
					Select Item_vw
					If !Seek(Main_Vw.Entry_ty+Dtos(Main_Vw.Date)+Main_Vw.Doc_no+TmpEt_Vw.ItSerial,'Item_vw','Eddits')
						metdele = .T.
					Else
*!*							If TmpEt_Vw.Cit_code # Item_vw.it_code &&Or TmpEt_Vw.Cware # Item_vw.Ware_nm  	&& Commented by Shrikant S. on 29/09/2010 for TKT-4021
						If TmpEt_Vw.Cit_code # Item_vw.it_code  And !Inlist(Gen_SrNo_Vw.ctype,"RGAPART2","RGCPART2","PLASRNO")	&& Added by Shrikant S. on 29/09/2010 for TKT-4021
							metdele = .T.
						Endif
&& Added by Shrikant S. on 01 Feb, 2010  ------------Start
						If Seek(Main_Vw.Entry_ty+Dtos(Main_Vw.Date)+Main_Vw.Doc_no+TmpEt_Vw.ItSerial,'Item_vw','Eddits')
							If TmpEt_Vw.Cit_code = Item_vw.it_code Or Empty(Item_vw.u_pageno)&&Or TmpEt_Vw.Cware # Item_vw.Ware_nm
								metdele = .T.
							Endif
						Endif
&& Added by Shrikant S. on 01 Feb, 2010  ------------End
&& Added by Shrikant S. on 29/09/2010 for TKT-4021
						If	Inlist(Gen_SrNo_Vw.ctype,"RGAPART2","RGCPART2","PLASRNO")		&& 280910
							If	(Type('main_vw.u_rg23no')='C' Or Type('main_vw.u_rg23cno')='C')
								If	Empty(Main_Vw.u_rg23no) And Empty(Main_Vw.u_rg23cno)
									metdele =.T.
								Endif
							Endif
							If	Type('main_vw.u_plasr')='C'
								If Empty(Main_Vw.u_plasr)
									metdele=.T.
								Endif
							Endif
						Endif
&& Added by Shrikant S. on 29/09/2010 for TKT-4021
					Endif
					If metdele = .T. And etsql_con > 0
						etsql_str = _curvouobj.SqlConObj.GenDelete("Gen_SrNo","Tran_cd = ?Main_vw.Tran_cd And ;
								Entry_ty = ?Main_vw.Entry_ty And Itserial = ?TmpEt_Vw.itserial")
						etsql_con = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[],;
							"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
					Else
						Select Gen_SrNo_Vw
						If Seek(TmpEt_Vw.ItSerial,'Gen_SrNo_Vw','ItSerial') And ;
								Gen_SrNo_Vw.Cit_code = TmpEt_Vw.Cit_code And  ;
								Gen_SrNo_Vw.Cgroup = TmpEt_Vw.Cgroup And Gen_SrNo_Vw.Cchapno = TmpEt_Vw.Cchapno		&&Gen_SrNo_Vw.Cware = TmpEt_Vw.Cware AND
							Replace NPgNo With '' In Gen_SrNo_Vw
						Endif
					Endif
					Select TmpEt_Vw
				Endscan
			ELSE
				etsql_con = 0
			Endif

			Select Gen_SrNo_Vw
			Scan
				mrecno=Recno()			&& Added by Shrikant S. on 29/09/2010 for TKT-4021
				Select Item_vw
				If !Seek(Main_Vw.Entry_ty+Dtos(Main_Vw.Date)+Main_Vw.Doc_no+Gen_SrNo_Vw.ItSerial,'Item_vw','Eddits') And !Inlist(Gen_SrNo_Vw.ctype,"RGAPART2","RGCPART2","PLASRNO") && Changed by Shrikant S. on 29/09/2010 for TKT-4021
					Replace NPgNo With '' In Gen_SrNo_Vw
				Else
*!*						If Gen_SrNo_Vw.Cit_code # Item_vw.it_code &&Or Gen_SrNo_Vw.Cware # Item_vw.Ware_nm
					If Gen_SrNo_Vw.Cit_code # Item_vw.it_code And !Inlist(Gen_SrNo_Vw.ctype,"RGAPART2","RGCPART2","PLASRNO") && Changed by Shrikant S. on 29/09/2010 for TKT-4021
						Replace NPgNo With '' In Gen_SrNo_Vw
					Endif
&&Added by Shrikant S. on 06 Feb, 2010 -- Start
					If Seek(Main_Vw.Entry_ty+Dtos(Main_Vw.Date)+Main_Vw.Doc_no+Gen_SrNo_Vw.ItSerial,'Item_vw','Eddits')
						If (Gen_SrNo_Vw.Cit_code = Item_vw.it_code And Empty(Item_vw.u_pageno))
							Replace NPgNo With '' In Gen_SrNo_Vw
						Else
							Replace NPgNo With Item_vw.u_pageno In Gen_SrNo_Vw		&& Added by Shrikant S. on 29/09/2010 for TKT-4021
						Endif
					Endif
&&Added by Shrikant S. on 06 Feb, 2010 -- Start
&& Added by Shrikant S. on 29/09/2010 for TKT-4021		--Start
					If	Inlist(Gen_SrNo_Vw.ctype,"RGAPART2","RGCPART2","PLASRNO")
						If	(Type('main_vw.u_rg23no')='C' Or Type('main_vw.u_rg23cno')='C')
							If	Empty(Main_Vw.u_rg23no) And Empty(Main_Vw.u_rg23cno)
								Replace NPgNo With '' In Gen_SrNo_Vw
							Endif
						Endif
						If	Type('main_vw.u_plasr')='C' And !(Type('main_vw.u_rg23no')='C' Or Type('main_vw.u_rg23cno')='C')
							If Empty(Main_Vw.u_plasr)
								Replace NPgNo With '' In Gen_SrNo_Vw
							Endif
						Endif
					Endif
				Endif
&& Added by Shrikant S. on 29/09/2010 for TKT-4021		--End
				If etsql_con > 0 And !Empty(Gen_SrNo_Vw.NPgNo)
&& Added by Shrikant S. on 29/09/2010 for TKT-4021		--Start
					Do Case
						Case Gen_SrNo_Vw.ctype="RGAPART2"
							Do While (Dup_No("U_rg23no",Gen_SrNo_Vw.NPgNo,"Stkl_vw_Main",.T.,"_curvouobj.nHandle")=.F.)
								NewSrno=Gen_no("U_rg23no","Stkl_vw_Main",.T.,"_curvouobj.nHandle")
								Replace NPgNo With NewSrno In Gen_SrNo_Vw
								Replace u_rg23no With NewSrno In Main_Vw
							Enddo
						Case Gen_SrNo_Vw.ctype="RGCPART2"
							Do While (Dup_No("U_rg23cno",Main_Vw.u_rg23cno,"Stkl_vw_Main",.T.,"_curvouobj.nHandle")=.F.)
								NewSrno=Gen_no("U_rg23cno","Stkl_vw_Main",.T.,"_curvouobj.nHandle")
								Replace NPgNo With NewSrno In Gen_SrNo_Vw
								Replace u_rg23cno With NewSrno In Main_Vw
							Enddo
						Case Gen_SrNo_Vw.ctype="RGPART1"
							Do While (CHK_PAGENO(.T.,"_curvouobj.nHandle")=.F.)
								Replace u_pageno With '' In Item_vw
								Newpgno=Gen_Pageno()
								If Betw(mrecno,1,Reccount())
									Go mrecno
									Replace NPgNo With Newpgno In Gen_SrNo_Vw
									Replace u_pageno With Newpgno In Item_vw
								Endif
							Enddo
						Case Gen_SrNo_Vw.ctype="PLASRNO"
							Do While (Dup_No("u_plasr",Main_Vw.u_plasr,"Stkl_vw_Main",.T.,"_curvouobj.nHandle")=.F.)
								NewSrno=Gen_no("u_plasr","Stkl_vw_Main",.T.,"_curvouobj.nHandle")
								Replace NPgNo With NewSrno In Gen_SrNo_Vw
								Replace u_plasr With NewSrno In Main_Vw
							Enddo
					Endcase
&& Added by Shrikant S. on 29/09/2010 for TKT-4021		--End
					etsql_str  = _curvouobj.SqlConObj.GenInsert("Gen_SrNo","","","Gen_SrNo_Vw",mvu_backend)
					etsql_con  = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[],;
						"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
				Endif
				Select Gen_SrNo_Vw
			Endscan
		Endif
	ELSE
		etsql_str  = "Select Top 1 Tran_cd from Gen_SrNo Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
		etsql_con  = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[TmpEt_Vw],;
			"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
		If etsql_con > 0 And Used('TmpEt_Vw')
			Select TmpEt_Vw
			If Reccount() > 0
				etsql_str = _curvouobj.SqlConObj.GenDelete("Gen_SrNo","Tran_cd = ?Main_vw.Tran_cd And ;
						Entry_ty = ?Main_vw.Entry_ty")
				etsql_con = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[],;
					"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
			Endif
		Endif
	Endif
&& Added by Shrikant S. on 29/09/2010 for TKT-4021		--Start
	If(!Inlist(Main_Vw.Rule,"NON-MODVATABLE","ANNEXURE V") Or Inlist(Main_Vw.Entry_ty,"RR","RP","GI","GR","HI","HR"))
		If	(Type('Main_vw.U_rg23no')='C' Or Type('Main_vw.U_rg23cno')='C')
			Do Case
				Case !Empty(Main_Vw.u_rg23no)
					etsql_str  = "Update "+_curvouobj.Entry_tbl+"Main set U_rg23no='"+Main_Vw.u_rg23no+"' Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty "
					etsql_str  = etsql_str +" "+"Update "+_curvouobj.Entry_tbl+"Acdet set U_rg23no='"+Main_Vw.u_rg23no+"' Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty "
					etsql_con1 = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[_tpageno],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
				Case !Empty(Main_Vw.u_rg23cno)
					etsql_str  = "Update "+_curvouobj.Entry_tbl+"Main set U_rg23Cno='"+Main_Vw.u_rg23cno+"' Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty "
					etsql_str  = etsql_str +" "+"Update "+_curvouobj.Entry_tbl+"Acdet set U_rg23Cno='"+Main_Vw.u_rg23cno+"' Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty "
					etsql_con1 = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[_tpageno],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
			Endcase
			If Used('_tpageno')
				Use In _tpageno
			Endif
		Endif
		If Type('Main_vw.u_plasr')='C'
			If !Empty(Main_Vw.u_plasr)
			MESSAGEBOX(_curvouobj.Entry_tbl)
				etsql_str  = "Update "+_curvouobj.Entry_tbl+"Main set U_Plasr='"+Main_Vw.u_plasr+"' Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty "
				etsql_str  = etsql_str +" "+"Update "+_curvouobj.Entry_tbl+"Acdet set U_Plasr='"+Main_Vw.u_plasr+"' Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty "
				MESSAGEBOX(etsql_str)
				etsql_con1 = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[_tpageno],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
				If Used('_tpageno')
					Use In _tpageno
				Endif
			Endif
		Endif
		Select Item_vw

		Scan
			If Type('Item_vw.u_pageno')='C'
				If(!Empty(Item_vw.u_pageno))
					etsql_str  = "Update "+_curvouobj.Entry_tbl+"Item set u_pageno='"+Item_vw.u_pageno+"' Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty and Itserial=?Item_vw.Itserial"
					etsql_con1 = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[_temppageno],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
					If Used('_temppageno')
						Use In _temppageno
					Endif
				Endif
			Endif
			Select Item_vw
		Endscan
	Endif
&& Added by Shrikant S. on 29/09/2010 for TKT-4021		--End

	If Used('TmpEt_Vw')
		Use In TmpEt_Vw
	Endif
Endif

&&-->Ipop(Rup)

_Malias 	= Alias()
_mRecNo	= Recno()

If ([vuexc] $ vchkprod)
	If(_curvouobj.AddMode=.T. Or _curvouobj.EditMode=.T.)
		If Type('_curvouobj.PCVTYPE')='C'
			If (Inlist(_curvouobj.PCVTYPE,'IP','ST','OP','DC')) And Used('projectitref_vw')
				Set DataSession To _Screen.ActiveForm.DataSessionId
				If (_curvouobj.EditMode) &&Delete existing record from projectitref
					etsql_str  = "delete  from projectitref Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
					etsql_con1 = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[_delBom],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
					If Used('_delBom')
						Use In _delBom
					Endif
				Endif

				Select projectitref_vw
				If Reccount()>0 &&Insert records into projectitref
					Replace All Tran_cd With Main_Vw.Tran_cd In projectitref_vw
					Scan
						mSqlStr  =  _curvouobj.SqlConObj.GenInsert ("projectitref","'SERIALNO'","","projectitref_vw",mvu_backend)
						etsql_con1  = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,mSqlStr,[],;
							"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
					Endscan
				Endif
				Use In projectitref_vw
			Endif
		Endif
	Else &&Delete Button
		If  (([vuexc] $ vchkprod) Or ([vuinv] $ vchkprod)) And Type('_curvouobj.PCVTYPE')='C' &&Check Existing Records
			etsql_str  = "select top 1 entry_ty from projectitref Where aTran_cd = ?Main_vw.Tran_cd And aEntry_ty = ?Main_vw.Entry_ty"
			etsql_con1 = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[_chkbom],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
			If Used('_chkbom')
				If Reccount()>0
					Select _chkbom
					=Messagebox("Entry Passed Against /"+_chkbom.Entry_ty+" Entry Can not be Deleted",16,vumess)
					Use In _chkbom
					Return .F.
				Endif
				Use In _chkbom
			Endif
		Endif
		etsql_str  = "delete  from projectitref Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
		etsql_con1 = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[_delBom],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
		If Used('_delBom')
			Use In _delBom
		Endif
	Endif
Endif
If !Empty(_Malias)
	Select &_Malias
Endif
If Betw(_mRecNo,1,Reccount())
	Go _mRecNo
Endif


If _curvouobj.AddMode = .T. Or _curvouobj.EditMode = .T.
	If Used('_uploadcursor')
		If Reccount('_uploadcursor') > 0
			objUpload =Createobject("Udyog.iTAX.FileUpload.Any.Format")
			ServerName=mvu_server
			UserName  =_curvouobj.SqlConObj.dec(_curvouobj.SqlConObj.ondecrypt(mvu_user))
			SPassword =_curvouobj.SqlConObj.dec(_curvouobj.SqlConObj.ondecrypt(mvu_Pass))
			ConnectionString = "Data Source="+ServerName+";Initial Catalog="+Company.DbName+";User ID="+UserName+";password="+SPassword

			Select _uploadcursor
			Replace All TR_ID With Main_Vw.Tran_cd In _uploadcursor
			Scan &&While !Eof()
				If _uploadcursor.pWhat = "U" Or _uploadcursor.pWhat = "I"
					If Empty(_uploadcursor.objSave)
						objUpload.SaveImage(_uploadcursor.tr_type,_uploadcursor.TR_ID,_uploadcursor.tr_serial,_uploadcursor.filename,_uploadcursor.Extension,_uploadcursor.objPath,ConnectionString,_uploadcursor.pWhat,.F.,Null,_uploadcursor.tr_itSerial)
					Else
						objUpload.SaveImage(_uploadcursor.tr_type,_uploadcursor.TR_ID,_uploadcursor.tr_serial,_uploadcursor.filename,_uploadcursor.Extension,_uploadcursor.objSave,ConnectionString,_uploadcursor.pWhat,.T.,Alltrim(_uploadcursor.objSource),_uploadcursor.tr_itSerial)
					Endif
				Else
*********** Added By Sachin N. S. on 23/09/2010 for TKT-3982 *********** Start
					If _uploadcursor.pWhat = "D"
						objUpload.DeleteImage(_uploadcursor.tr_type,_uploadcursor.TR_ID,_uploadcursor.tr_serial,ConnectionString,_uploadcursor.DatabaseSave,_uploadcursor.tr_itSerial)		&& Changed By Sachin N. S. on 23/09/2010 for TKT-3982
					Endif
*********** Added By Sachin N. S. on 23/09/2010 for TKT-3982 *********** End
*!*						objUpload.DeleteImage(_uploadcursor.tr_type,_uploadcursor.TR_ID,_uploadcursor.tr_serial,ConnectionString,Iif(Empty(_uploadcursor.objSave),.F.,.T.),_uploadcursor.tr_itSerial)	&& Commented By Sachin N. S. on 23/09/2010 for TKT-3982

				Endif
				Select _uploadcursor
			Endscan
			objUpload.ConnectionClose()
			objUpload.Dispose()
		Endif
	Endif
ELSE
	If _curvouobj.AddMode = .F. And _curvouobj.EditMode = .F.
		mSqlStr = "delete from uploadfiles where tr_type = '" + Main_Vw.Entry_ty + "' and tr_id = " + Str(Main_Vw.Tran_cd)
		nRetval = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,mSqlStr,"",;
			"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
		If nRetval<=0
			Return .F.
		Endif

	Endif
Endif


******** Added By Sachin N. S. on 20/12/2010 for New Installer ******** Start

If _curvouobj.ItemPage	And ([vutex] $ vchkprod) And etsql_con > 0
	If _curvouobj.AddMode Or _curvouobj.EditMode
		If Used('Gen_SrNo_Vw')
			Select Gen_SrNo_Vw
			Replace All Tran_cd With Main_Vw.Tran_cd,Entry_ty With Main_Vw.Entry_ty,;
				Date With Main_Vw.Date,CompId With Main_Vw.CompId,l_yn With Main_Vw.l_yn In Gen_SrNo_Vw
*npgno,cit_code,cware,cgroup,cchapno
*itserial

			etsql_str  = "Select * from Gen_SrNo Where l_yn = ?main_vw.l_yn and Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
			etsql_con  = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[TmpEt_Vw],;
				"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
			If etsql_con > 0 And Used('TmpEt_Vw')
				Select TmpEt_Vw
				Scan
					metdele = .F.
					Select Item_vw
					If !Seek(Main_Vw.Entry_ty+Dtos(Main_Vw.Date)+Main_Vw.Doc_no+TmpEt_Vw.ItSerial,'Item_vw','Eddits')
						metdele = .T.
					Else
						If TmpEt_Vw.Cit_code # Item_vw.it_code &&Or TmpEt_Vw.Cware # Item_vw.Ware_nm
							metdele = .T.
						Endif
					Endif
					If metdele = .T. And etsql_con > 0
						etsql_str = _curvouobj.SqlConObj.GenDelete("Gen_SrNo","Tran_cd = ?Main_vw.Tran_cd And ;
								Entry_ty = ?Main_vw.Entry_ty And Itserial = ?TmpEt_Vw.itserial")
						etsql_con = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[],;
							"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
					Else
						Select Gen_SrNo_Vw
						If Seek(TmpEt_Vw.ItSerial,'Gen_SrNo_Vw','ItSerial') And ;
								Gen_SrNo_Vw.Cit_code = TmpEt_Vw.Cit_code And  ;
								Gen_SrNo_Vw.Cgroup = TmpEt_Vw.Cgroup And Gen_SrNo_Vw.Cchapno = TmpEt_Vw.Cchapno		&&Gen_SrNo_Vw.Cware = TmpEt_Vw.Cware AND
							Replace NPgNo With '' In Gen_SrNo_Vw
						Endif
					Endif
					Select TmpEt_Vw
				Endscan
			Else
				etsql_con = 0
			Endif

			Select Gen_SrNo_Vw
			Scan
				Select Item_vw
				If !Seek(Main_Vw.Entry_ty+Dtos(Main_Vw.Date)+Main_Vw.Doc_no+Gen_SrNo_Vw.ItSerial,'Item_vw','Eddits')
					Replace NPgNo With '' In Gen_SrNo_Vw
				Else
					If Gen_SrNo_Vw.Cit_code # Item_vw.it_code &&Or Gen_SrNo_Vw.Cware # Item_vw.Ware_nm
						Replace NPgNo With '' In Gen_SrNo_Vw
					Endif
				Endif
				If etsql_con > 0 And !Empty(Gen_SrNo_Vw.NPgNo)
					etsql_str  = _curvouobj.SqlConObj.GenInsert("Gen_SrNo","","","Gen_SrNo_Vw",mvu_backend)
					etsql_con  = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[],;
						"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
				Endif
				Select Gen_SrNo_Vw
			Endscan
		Endif
	Else
		etsql_str  = "Select Top 1 Tran_cd from Gen_SrNo Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
		etsql_con  = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[TmpEt_Vw],;
			"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
		If etsql_con > 0 And Used('TmpEt_Vw')
			Select TmpEt_Vw
			If Reccount() > 0
				etsql_str = _curvouobj.SqlConObj.GenDelete("Gen_SrNo","Tran_cd = ?Main_vw.Tran_cd And ;
						Entry_ty = ?Main_vw.Entry_ty")
				etsql_con = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[],;
					"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
			Endif
		Endif
	Endif
	If Used('TmpEt_Vw')
		Use In TmpEt_Vw
	Endif
Endif
******** Added By Sachin N. S. on 20/12/2010 for New Installer ******** End



&& Rup--->12/08/2009
If Used('Gen_SrNo_Vw')
	Use In Gen_SrNo_Vw
Endif
&&<---Rup 12/08/2009

*!*	IF _curvouobj.Addmode = .t. OR _curvouobj.EditMode = .t.
*!*		IF USED('_uploadcursor')
*!*			IF RECCOUNT('_uploadcursor') > 0
*!*				objUpload =CREATEOBJECT("Udyog.iTAX.FileUpload.Any.Format")
*!*				ServerName=mvu_server
*!*				UserName  =_curvouobj.SqlConObj.dec(_curvouobj.sqlconObj.ondecrypt(mvu_user))
*!*				SPassword =_curvouobj.SqlConObj.dec(_curvouobj.sqlconObj.ondecrypt(mvu_Pass))
*!*				ConnectionString = "Data Source="+ServerName+";Initial Catalog="+Company.dbname+";User ID="+UserName+";password="+SPassWord

*!*				SELECT _uploadcursor
*!*				GO top
*!*				SCAN WHILE !EOF()
*!*					IF _uploadcursor.pWhat = "U" OR _uploadcursor.pWhat = "I"
*!*						IF EMPTY(_uploadcursor.objSave)
*!*							objUpload.SaveImage(_uploadcursor.tr_type,_uploadcursor.tr_id,_uploadcursor.tr_serial,_uploadcursor.filename,_uploadcursor.Extension,_uploadcursor.objPath,ConnectionString,_uploadcursor.pWhat,.f.,null)
*!*						ELSE
*!*							objUpload.SaveImage(_uploadcursor.tr_type,_uploadcursor.tr_id,_uploadcursor.tr_serial,_uploadcursor.filename,_uploadcursor.Extension,_uploadcursor.objSave,ConnectionString,_uploadcursor.pWhat,.t.,ALLTRIM(_uploadcursor.objSource))
*!*						ENDIF
*!*					ENDIF
*!*				ENDSCAN
*!*				objUpload.ConnectionClose()
*!*				objUpload.Dispose()
*!*			ENDIF
*!*		ENDIF
*!*	ELSE
*!*		IF _curvouobj.addmode = .f. AND _curvouobj.editmode = .f.
*!*			mSqlStr = "delete from uploadfiles where tr_type = '" + main_vw.entry_ty + "' and tr_id = " + STR(main_vw.tran_cd)
*!*			nRetval = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,mSqlstr,"",;
*!*								"_curvouobj.nHandle",_curvouobj.DataSessionId,.t.)
*!*			IF nretVal<=0
*!*				RETURN .f.
*!*			ENDIF

*!*		ENDIF
*!*	ENDIF

******By Shrikant S. on  27/01/2011 ---Start for TKT-5814
If _curvouobj.AddMode = .T. Or _curvouobj.EditMode = .T.
	If Used('ItRef_vw')
		Select ItRef_vw
		Scan
		&&--->Commented by Rup 18/04/2011 TKT-6627,6628,6829
*!*				etsql_str = "Select Top 1 Tran_cd from  "+ItRef_vw.REntry_ty+"item where Tran_cd = ?ItRef_vw.Itref_tran And Itserial = ?ItRef_vw.Ritserial  and re_qty>qty"
*!*				etsql_con1  = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,etsql_str,;
*!*					[tmpreqty_vw],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)

*!*				If etsql_con1 > 0 And Used('tmpreqty_vw')
*!*					Select tmpreqty_vw
*!*					If Reccount()>0
*!*						sql_errmess = Chr(13)+" Unable to Pickup. Since it is already picked."+Chr(13)
*!*						Return .F.
*!*					Endif
*!*					If Used('tmpreqty_vw')
*!*						Select tmpreqty_vw
*!*						Use In tmpreqty_vw
*!*					Endif
*!*				Endif
		&&<---Commented by Rup 18/04/2011 TKT-6627,6628,6829
		&&--->Added by Rup 18/04/2011 TKT-6627,6628,6829
			etsql_str = "select bcode_nm=case when ext_vou=1 then bcode_nm else entry_ty end,uom_desc  from lcode where entry_ty='"+ItRef_vw.REntry_ty+"'"
			etsql_con1  = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,etsql_str,;
				"llcode","_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
			etsql_str = "Select  * from  "+llcode.bcode_nm+"item where Tran_cd = ?ItRef_vw.Itref_tran And Itserial = ?ItRef_vw.Ritserial "
			etsql_con1  = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,etsql_str,;
				[tmpreqty_vw],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
			mAnsln =0	
			SELECT llcode
			IF !EMPTY(llcode.uom_desc) AND !EMPTY(lcode_vw.uom_desc) 
				_muom_desc2 = llcode.uom_desc
				FOR i1 = 1 TO Company.Uom_no
					IF TYPE('_curvouobj.multi_qty(i1,1)') = 'C'
						vQ= IIF(AT(';',_muom_desc2)	> 0,SUBSTR(_muom_desc2,1,AT(';',_muom_desc2)-1),_muom_desc2)
						vQty= IIF(AT(':',vQ) > 0,SUBSTR(vQ,1,AT(':',vQ)-1),vQ)	
						v2 = IIF(AT(':',vQ) > 0,SUBSTR(vQ,AT(':',vQ)+1),'')
						vSoft = SUBSTR(v2 ,1,3)	
						vHard = SUBSTR(v2 ,5,3)
						IF vSoft=".f." AND vHard =".f."
							vHard =".t."
						ENDIF 
						x="tmpreqty_vw.re_"+vQty+">tmpreqty_vw."+vQty
						IF &x
							vx=0
							x="tmpreqty_vw."+vQty
							vx=&x
							mmess =vQty+" Value Exceeds Balance Value for "+ALLTRIM(str(vx))
							IF 	lower(vSoft) ='.t.'
								if _curvouobj.ShowMessageBox(mmess+chr(13)+'Proceed ?',4,vumess,1) # 6		&&vasant051209
									sql_errmess = Chr(13)+" Unable to Pickup. Since it is already picked."+Chr(13)
									RETURN .f.
								ENDIF
							ELSE
								IF 	lower(vHard) ='.t.'
									sql_errmess = Chr(13)+" Unable to Pickup. Since it is already picked."+Chr(13)
									RETURN .f.							
								ENDIF 
							ENDIF 
						ENDIF 
					endi	
				Endfor
			ELSE
				If etsql_con1 > 0 And Used('tmpreqty_vw')
					Select tmpreqty_vw
					If Reccount()>0
						IF tmpreqty_vw.re_qty>tmpreqty_vw.qty
							sql_errmess = Chr(13)+" Unable to Pickup. Since it is already picked."+Chr(13)
							Return .F.
						ENDIF 
					Endif
					If Used('tmpreqty_vw')
						Select tmpreqty_vw
						Use In tmpreqty_vw
					ENDIF
				ENDIF 
			ENDIF
			Select ItRef_vw
		&&<---Added by Rup 18/04/2011 TKT-6627,6628,6829
		Endscan
	Endif
Endif
******By Shrikant S. on  27/01/2011 ---End


IF 'trnamend' $ vChkprod
	DO VouFinalUpdate IN MainPrg &&Birendra :(TKT-2386) 22 Mar 2011 for Order Amendment
ENDIF 

******** Added By Sachin N. S. on 15/07/2011 for Batchwise/Serialize Inventory ******** Start
If _curvouobj.itempage
	If etsql_con>0
		etsql_con=_curvouobj._BatchSerialStk._ueTrigVouFinalUpdate()
	Endif
Endif
******** Added By Sachin N. S. on 15/07/2011 for Batchwise/Serialize Inventory ******** End

Return Iif(etsql_con < 1,.F.,.T.)

