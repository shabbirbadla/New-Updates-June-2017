Parameter ventryType, vInvoiceDate,Tbl_entry,ooSqlConObj,nDataSessionId,vnHandle,vcDbName,vdSta_Dt,vdEnd_Dt

If Empty(ventryType)
	Retu ''
Endif
If Len(ventryType)>2
	Retu ''
Endif
If Len(ventryType)=1
	ventryType=ventryType+[ ]
Endif
If Empty(vInvoiceDate)
	Retu ''
Endif
If Used('Gen_doc_vw')
	Select Gen_doc_vw
	Use In Gen_doc_vw
Endif
mdoc_no = ''

sql_con = ooSqlConObj.DataConn([EXE],vcDbName,[ Select * from Gen_doc with (NOLOCK) where 1=0 ],[Gen_doc_vw],;
	vnHandle,nDataSessionId,.F.)
If sql_con > 0 And Used('Gen_doc_vw')
	Select Gen_doc_vw
	Append Blank In Gen_doc_vw
	Replace Entry_ty With ventryType,Date With vInvoiceDate In Gen_doc_vw
	Wait Clear 		&&vasu
	mrollback = .T.
	Do While .T.
		sql_con = ooSqlConObj.DataConn([EXE],vcDbName,[ Select Top 1 Doc_no from Gen_doc with (TABLOCKX) ;
			where Entry_ty = ?Gen_doc_vw.Entry_ty And Date = ?Gen_doc_vw.Date ],[tmptbl_vw],;
			vnHandle,nDataSessionId,.T.)
		mSqlStr = []
		If sql_con > 0 And Used('tmptbl_vw')
			Select tmptbl_vw
			If Reccount() <= 0
				Select Gen_doc_vw
				Replace doc_no With 1 In Gen_doc_vw
				mSqlStr = ooSqlConObj.GenInsert("gen_doc","","","Gen_doc_vw",mvu_backend)
			Else
				Select Gen_doc_vw
				Replace doc_no With tmptbl_vw.doc_no + 1 In Gen_doc_vw
				mCond = "Entry_ty = ?Gen_doc_vw.Entry_ty And Date = ?Gen_doc_vw.Date"
				mSqlStr = ooSqlConObj.GenUpdate("gen_doc","","","Gen_doc_vw",mvu_backend,mCond,"doc_no")
			Endif
			sql_con = ooSqlConObj.DataConn([EXE],vcDbName,mSqlStr,[],;
				vnHandle,nDataSessionId,.T.)
			If sql_con > 0
				sql_con = ooSqlConObj._SqlCommit(vnHandle)
				If sql_con > 0
					sql_main = Tbl_entry+'Main'
					Select Gen_doc_vw
					mdoc_no = Padl(Alltrim(Str(Gen_doc_vw.doc_no)),Len(Main_vw.doc_no),"0")
					sql_con = ooSqlConObj.DataConn([EXE],vcDbName,[ Select Top 1 Entry_ty from ]+sql_main+;
						[ where Entry_ty = ?Gen_doc_vw.Entry_ty And Date = ?Gen_doc_vw.Date And Doc_no = ?mdoc_no],;
						[tmptbl_vw],vnHandle,nDataSessionId,.F.)
					If sql_con > 0 And Used('tmptbl_vw')
						Select tmptbl_vw
						If Reccount() <= 0
							mrollback = .F.
							Exit
						Endif
					Endif
				Endif
			Endif
		Endif
	Enddo
	If mrollback = .T.
		sql_con = ooSqlConObj._SqlRollback(vnHandle)
	Endif
Endif
If Used('Gen_doc_vw')
	Select Gen_doc_vw
	Use In Gen_doc_vw
Endif
Wait Clear
Return mdoc_no

