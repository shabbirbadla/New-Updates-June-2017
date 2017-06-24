_curvouobj = _Screen.ActiveForm

etsql_con = 1
IF _curvouobj.ItemPage
	IF _curvouobj.AddMode OR _curvouobj.EditMode
		If Used('Gen_SrNo_Vw')
			SELECT Gen_SrNo_Vw
			REPLACE ALL Tran_cd WITH Main_Vw.Tran_cd,Entry_ty WITH Main_vw.Entry_ty,;
				Date WITH Main_vw.Date,CompId WITH Main_vw.CompId,l_yn WITH main_vw.l_yn IN Gen_SrNo_Vw
			*npgno,cit_code,cware,cgroup,cchapno
			*itserial
			
			etsql_str  = "Select * from Gen_SrNo Where l_yn = ?main_vw.l_yn and Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
			etsql_con  = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[TmpEt_Vw],;
							"_curvouobj.nHandle",_curvouobj.DataSessionId,.t.)
			IF etsql_con > 0 AND USED('TmpEt_Vw')
				SELECT TmpEt_Vw
				SCAN
					metdele = .f.
					SELECT Item_vw
					IF !SEEK(Main_vw.Entry_ty+DTOS(Main_vw.Date)+Main_vw.Doc_no+TmpEt_Vw.ItSerial,'Item_vw','Eddits')
						metdele = .t.
					ELSE
						IF TmpEt_Vw.Cit_code # Item_vw.it_code &&Or TmpEt_Vw.Cware # Item_vw.Ware_nm
							metdele = .t.						
						Endif 	
					Endif		
					IF metdele = .t. AND etsql_con > 0
						etsql_str = _curvouobj.SqlConObj.GenDelete("Gen_SrNo","Tran_cd = ?Main_vw.Tran_cd And ;
								Entry_ty = ?Main_vw.Entry_ty And Itserial = ?TmpEt_Vw.itserial")
						etsql_con = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[],;
							"_curvouobj.nHandle",_curvouobj.DataSessionId,.t.)
					Else
						SELECT Gen_SrNo_Vw
						IF SEEK(TmpEt_Vw.Itserial,'Gen_SrNo_Vw','ItSerial') AND ;
							Gen_SrNo_Vw.Cit_code = TmpEt_Vw.Cit_code AND  ;
								Gen_SrNo_Vw.Cgroup = TmpEt_Vw.Cgroup AND Gen_SrNo_Vw.Cchapno = TmpEt_Vw.Cchapno		&&Gen_SrNo_Vw.Cware = TmpEt_Vw.Cware AND
							REPLACE NPgNo WITH '' IN Gen_SrNo_Vw		
						Endif			
					Endif	
					SELECT TmpEt_Vw
				Endscan	
			ELSE
				etsql_con = 0
			Endif		
				
			SELECT Gen_SrNo_Vw
			SCAN
				SELECT Item_vw
				IF !SEEK(Main_vw.Entry_ty+DTOS(Main_vw.Date)+Main_vw.Doc_no+Gen_SrNo_Vw.ItSerial,'Item_vw','Eddits')
					REPLACE NPgNo WITH '' IN Gen_SrNo_Vw
				ELSE
					IF Gen_SrNo_Vw.Cit_code # Item_vw.it_code &&Or Gen_SrNo_Vw.Cware # Item_vw.Ware_nm
						REPLACE NPgNo WITH '' IN Gen_SrNo_Vw
					Endif 	
				Endif		
				IF etsql_con > 0 AND !EMPTY(Gen_SrNo_Vw.NPgNo)
					etsql_str  = _curvouobj.SqlConObj.GenInsert("Gen_SrNo","","","Gen_SrNo_Vw",mvu_backend)
					etsql_con  = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[],;
							"_curvouobj.nHandle",_curvouobj.DataSessionId,.t.)
				Endif		
				SELECT Gen_SrNo_Vw
			Endscan
		ENDIF
	ELSE
		etsql_str  = "Select Top 1 Tran_cd from Gen_SrNo Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
		etsql_con  = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[TmpEt_Vw],;
						"_curvouobj.nHandle",_curvouobj.DataSessionId,.t.)
		IF etsql_con > 0 AND USED('TmpEt_Vw')
			SELECT TmpEt_Vw
			IF RECCOUNT() > 0
				etsql_str = _curvouobj.SqlConObj.GenDelete("Gen_SrNo","Tran_cd = ?Main_vw.Tran_cd And ;
						Entry_ty = ?Main_vw.Entry_ty")
				etsql_con = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[],;
					"_curvouobj.nHandle",_curvouobj.DataSessionId,.t.)
			Endif
		ENDIF
	Endif	
	IF USED('TmpEt_Vw')
		USE IN TmpEt_Vw
	Endif	
ENDIF

&&-->Ipop(Rup)

_Malias 	= Alias()
_mRecNo	= Recno()	

IF (([vuexc] $ vchkprod) Or ([vuinv] $ vchkprod))
	if(_curvouobj.addmode=.t. or _curvouobj.editmode=.t.) 
		If Type('_curvouobj.PCVTYPE')='C'
			If (inlist(_curvouobj.PCVTYPE,'IP','ST','OP','DC')) AND USED('projectitref_vw') 
					Set DataSession To _Screen.ActiveForm.DataSessionId
					IF (_curvouobj.editmode) &&Delete existing record from projectitref 
						etsql_str  = "delete  from projectitref Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
						etsql_con1 = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[_delBom],"_curvouobj.nHandle",_curvouobj.DataSessionId,.t.)
						IF USED('_delBom')			
							USE IN _delBom
						ENDIF
					ENDIF 
		
					SELECT projectitref_vw	
					IF RECCOUNT()>0 &&Insert records into projectitref
						REPLACE ALL TRAN_CD WITH MAIN_VW.TRAN_CD IN projectitref_vw		
						SCAN
							mSqlStr  =  _curvouobj.SqlConObj.GenInsert ("projectitref","'SERIALNO'","","projectitref_vw",mvu_backend)	
							etsql_con1  = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,mSqlStr,[],;
									"_curvouobj.nHandle",_curvouobj.DataSessionId,.t.)
						ENDSCAN 
					ENDIF
					USE IN projectitref_vw	
			ENDIF
		ENDIF 
	ELSE &&Delete Button
		IF  (([vuexc] $ vchkprod) Or ([vuinv] $ vchkprod)) AND Type('_curvouobj.PCVTYPE')='C' &&Check Existing Records
			etsql_str  = "select top 1 entry_ty from projectitref Where aTran_cd = ?Main_vw.Tran_cd And aEntry_ty = ?Main_vw.Entry_ty"
			etsql_con1 = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[_chkbom],"_curvouobj.nHandle",_curvouobj.DataSessionId,.t.)
			IF USED('_chkbom')
				IF RECCOUNT()>0
					SELECT _chkbom
					=MESSAGEBOX("Entry Passed Against /"+_chkBom.entry_ty+" Entry Can not be Deleted",16,vumess)
					USE IN _chkbom
					RETURN .F. 	
				ENDIF 			
				USE IN _chkbom
			ENDIF
		ENDIF 
		
		etsql_str  = "delete  from projectitref Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
		etsql_con1 = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[_delBom],"_curvouobj.nHandle",_curvouobj.DataSessionId,.t.)
		IF USED('_delBom')			
			USE IN _delBom
		ENDIF
	ENDIF 	
ENDIF 	
If !Empty(_Malias)
	Select &_Malias
   ENDIF
If Betw(_mRecNo,1,Reccount())
	Go _mRecNo
ENDIF


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
					ENDIF
				ELSE
					objUpload.DeleteImage(_uploadcursor.tr_type,_uploadcursor.TR_ID,_uploadcursor.tr_serial,ConnectionString,IIF(EMPTY(_uploadcursor.objSave),.f.,.t.),_uploadcursor.tr_itserial)
				Endif
				Select _uploadcursor
			Endscan
			objUpload.ConnectionClose()
			objUpload.Dispose()
		Endif
	Endif
Else
	If _curvouobj.AddMode = .F. And _curvouobj.EditMode = .F.
		mSqlStr = "delete from uploadfiles where tr_type = '" + Main_Vw.Entry_ty + "' and tr_id = " + Str(Main_Vw.Tran_cd)
		nRetval = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,mSqlStr,"",;
			"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
		If nRetval<=0
			Return .F.
		Endif

	Endif
Endif

&& Rup--->12/08/2009
If Used('Gen_SrNo_Vw')
	USE IN Gen_SrNo_Vw
ENDIF
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




RETURN IIF(etsql_con < 1,.f.,.t.)

