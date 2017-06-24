Lparameter ventryType, vInvoiceSeries, vInvoiceNo,VentDate, voldInvoiceSeries, voldInvoiceNo,vinv_size,oSqlConObj, nDataSessionId, Entry_Tbl, vnHandle,vcDbName,vdSta_Dt,vdEnd_Dt

If Empty(ventryType)
	Retu ''
Endif
If Len(ventryType)>2
	Retu ''
Endif
If Len(ventryType)=1
	ventryType=ventryType+[ ]
Endif
If Empty(vInvoiceSeries)
	vInvoiceSeries=Space(Len(Tmp_Main.Inv_sr))
Endif
If Empty(voldInvoiceSeries)
	voldInvoiceSeries=Space(Len(Tmp_Main.Inv_sr))
Endif

If Type('vinv_size') # 'N'
	vinv_size = 1
Endif
*!*	If !Empty(Thisform.JustInvoice)
*!*		If Thisform.JustSeries+Thisform.JustInvoice==vInvoiceSeries+vInvoiceNo
*!*			Retu Thisform.JustInvoice
*!*		Endif
*!*	Endif

minv_no     = ''
v_i_s_type  = ''
v_i_prefix  = ''
v_i_suffix  = ''
v_i_middle  = ''
_vInvoiceEs = voldInvoiceSeries
_vInvoiceEn = voldInvoiceNo

v_i_prefix  = ''
v_i_suffix  = ''
v_i_s_type  = ''
v_i_middle  = ''
_vInvoiceSr = vInvoiceSeries
_vInvoiceNo = Allt(vInvoiceNo)
*!*	nHandle = 0 && Commented by Shrikant S. on 20/05/2010 for TKT-1476
sql_con = oSqlConObj.DataConn([EXE],vcDbName,[ Select Top 1 * from Series where Inv_sr = ?_vInvoiceSr],;
	[tmptbl_vw],vnHandle,nDataSessionId,.F.)
If sql_con > 0 And Used('tmptbl_vw')
	Select tmptbl_vw
	If Reccount() > 0
		v_i_s_type = tmptbl_vw.S_type
		v_i_MnthFormat = tmptbl_vw.MnthFormat
		If !Empty(tmptbl_vw.I_prefix)
			v_i_prefix = Allt(Eval(tmptbl_vw.I_prefix))
		Endif
		If !Empty(tmptbl_vw.I_suffix)
			v_i_suffix = Allt(Eval(tmptbl_vw.I_suffix))
		Endif
		Do Case
		Case v_i_s_type  = 'DAYWISE'
			v_i_middle   = Dtos(VentDate)
			v_i_middle   = Subs(v_i_middle,7,2)+Subs(v_i_middle,5,2)+Subs(v_i_middle,3,2)
		Case v_i_s_type  = 'MONTHWISE'
			v_i_middle   = genMnthWiseFormat(VentDate,v_i_MnthFormat)
			v_i_middle   = v_i_middle
		Endcase
		_vInvoiceNo = Subs(_vInvoiceNo,Len(v_i_prefix+v_i_middle)+1)
		_vInvoiceNo = Subs(_vInvoiceNo,1,Len(_vInvoiceNo)-Len(v_i_suffix))
	Endif
Endif
_vInvoiceNo = Val(_vInvoiceNo)
_vInvoiceNo = Iif(_vInvoiceNo = 0,1,_vInvoiceNo)

If v_i_s_type = 'MONTHWISE'
	VentDate1 = '01/'+Str(Month(VentDate),2)+'/'+Str(Year(VentDate),4)
	VentDate  = Ctod(VentDate1)
Endif

vctrYear=[]
If Between(VentDate,vdsta_dt,vdend_dt)
	vctrYear=Allt(Str(Year(vdsta_dt)))+[-]+Allt(Str(Year(vdend_dt)))
Else
	endDate=[]
	For i = 1 To Month(vdend_dt)
		If Empty(endDate)
			endDate=Allt(Str(i))
		Else
			endDate=endDate+[,]+Allt(Str(i))
		Endif
	Endfor

	startDate=[]
	For i = Month(vdsta_dt) To 12
		If Empty(startDate)
			startDate=Allt(Str(i))
		Else
			startDate=startDate+[,]+Allt(Str(i))
		Endif
	Endfor
	Do Case
	Case Inlist(Month(VentDate),&endDate)
		vctrYear=Allt(Str(Year(VentDate)-1))+[-]+Allt(Str(Year(VentDate)))
	Case Inlist(Month(VentDate),&startDate)
		vctrYear=Allt(Str(Year(VentDate)))+[-]+Allt(Str(Year(VentDate)+1))
	Endcase
Endif

If Used('Gen_inv_vw')
	Select Gen_inv_vw
	Use In Gen_inv_vw
Endif
If Used('Gen_miss_vw')
	Select Gen_miss_vw
	Use In Gen_miss_vw
Endif
sql_con = oSqlConObj.DataConn([EXE],vcDbName,[ Select * from Gen_inv with (NOLOCK) where 1=0 ],;
	[Gen_inv_vw],vnHandle,nDataSessionId,.F.)
If sql_con > 0
	sql_con = oSqlConObj.DataConn([EXE],vcDbName,[ Select * from Gen_miss with (NOLOCK) where 1=0 ],;
		[Gen_Miss_vw],vnHandle,nDataSessionId,.F.)
Endif
If sql_con > 0 And Used('Gen_inv_vw') And Used('Gen_miss_vw')
	Select Gen_inv_vw
	Append Blank In Gen_inv_vw
	Replace Entry_ty With ventryType,Inv_dt With VentDate,Inv_sr With vInvoiceSeries,;
		Inv_no With _vInvoiceNo,L_yn With vctrYear In Gen_inv_vw

	Select Gen_miss_vw
	Append Blank In Gen_miss_vw
	Replace Entry_ty With Gen_inv_vw.Entry_ty,Inv_dt With Gen_inv_vw.Inv_dt,Inv_sr With Gen_inv_vw.Inv_sr,;
		Inv_no With Gen_inv_vw.Inv_no,L_yn With Gen_inv_vw.L_yn,Flag With 'Y' In Gen_miss_vw

	Wait Clear
	mrollback = .T.
	Do While .T.
		Wait Window "Generating.., Please wait... " Nowait
		Do Case
		Case v_i_s_type = 'DAYWISE'
			sql_con = oSqlConObj.DataConn([EXE],vcDbName,[ Select Top 1 Inv_no from Gen_inv with (TABLOCKX) ;
				where Entry_ty = ?Gen_inv_vw.Entry_ty And Inv_sr = ?Gen_inv_vw.Inv_sr And Inv_dt = ?Gen_inv_vw.Inv_dt ],;
				[tmptbl_vw],vnHandle,nDataSessionId,.T.)
		Case v_i_s_type = 'MONTHWISE'
			sql_con = oSqlConObj.DataConn([EXE],vcDbName,[ Select Top 1 Inv_no from Gen_inv with (TABLOCKX) ;
				where Entry_ty = ?Gen_inv_vw.Entry_ty And Inv_sr = ?Gen_inv_vw.Inv_sr And ;
				MONTH(Inv_dt) = ?MONTH(Gen_inv_vw.Inv_dt) And Year(Inv_dt) = ?Year(Gen_inv_vw.Inv_dt) ],[tmptbl_vw],;
				vnHandle,nDataSessionId,.T.)
		Otherwise
			sql_con = oSqlConObj.DataConn([EXE],vcDbName,[ Select Top 1 Inv_no from Gen_inv with (TABLOCKX) ;
				where Entry_ty = ?Gen_inv_vw.Entry_ty And Inv_sr = ?Gen_inv_vw.Inv_sr And L_yn = ?Gen_inv_vw.L_yn ],;
				[tmptbl_vw],vnHandle,nDataSessionId,.T.)
		Endcase
		mSqlStr = []
		If sql_con > 0 And Used('tmptbl_vw')
			mSqlStr = ' '
			Select tmptbl_vw
			If Reccount() <= 0
				Select Gen_inv_vw
				mSqlStr = oSqlConObj.GenInsert("gen_inv","","","Gen_inv_vw",mvu_backend)
			Else
				If tmptbl_vw.Inv_no < Gen_inv_vw.Inv_no
					Select Gen_inv_vw
					Do Case
					Case v_i_s_type = 'DAYWISE'
						mCond = "Entry_ty = ?Gen_inv_vw.Entry_ty And Inv_sr = ?Gen_inv_vw.Inv_sr And Inv_dt = ?Gen_inv_vw.Inv_dt"
					Case v_i_s_type = 'MONTHWISE'
						mCond = "Entry_ty = ?Gen_inv_vw.Entry_ty And Inv_sr = ?Gen_inv_vw.Inv_sr And MONTH(Inv_dt) = ?MONTH(Gen_inv_vw.Inv_dt) And Year(Inv_dt) = ?Year(Gen_inv_vw.Inv_dt)"
					Otherwise
						mCond = "Entry_ty = ?Gen_inv_vw.Entry_ty And Inv_sr = ?Gen_inv_vw.Inv_sr And L_yn = ?Gen_inv_vw.L_yn"
					Endcase
					mSqlStr = oSqlConObj.GenUpdate("gen_inv","","","Gen_inv_vw",mvu_backend,mCond,"inv_no")
				Endif
			Endif
			If !Empty(mSqlStr)
				sql_con = oSqlConObj.DataConn([EXE],vcDbName,mSqlStr,[],;
					vnHandle,nDataSessionId,.T.)
			Endif
			If sql_con > 0
				Do Case
				Case v_i_s_type = 'DAYWISE'
					sql_con = oSqlConObj.DataConn([EXE],vcDbName,[ Select Top 1 Flag from Gen_miss where ;
						Entry_ty = ?Gen_inv_vw.Entry_ty And Inv_sr = ?Gen_inv_vw.Inv_sr And ;
						Inv_no = ?Gen_inv_vw.Inv_no And Inv_dt = ?Gen_inv_vw.Inv_dt ],[tmptbl_vw],;
						vnHandle,nDataSessionId,.T.)
				Case v_i_s_type = 'MONTHWISE'
					sql_con = oSqlConObj.DataConn([EXE],vcDbName,[ Select Top 1 Flag from Gen_miss where ;
						Entry_ty = ?Gen_inv_vw.Entry_ty And Inv_sr = ?Gen_inv_vw.Inv_sr And ;
						Inv_no = ?Gen_inv_vw.Inv_no And MONTH(Inv_dt) = ?MONTH(Gen_inv_vw.Inv_dt) And ;
						Year(Inv_dt) = ?Year(Gen_inv_vw.Inv_dt) ],[tmptbl_vw],;
						vnHandle,nDataSessionId,.T.)
				Otherwise
					sql_con = oSqlConObj.DataConn([EXE],vcDbName,[ Select Top 1 Flag from Gen_miss where ;
						Entry_ty = ?Gen_inv_vw.Entry_ty And Inv_sr = ?Gen_inv_vw.Inv_sr And ;
						Inv_no = ?Gen_inv_vw.Inv_no And L_yn = ?Gen_inv_vw.L_yn ],[tmptbl_vw],;
						vnHandle,nDataSessionId,.T.)
				Endcase
				vFoundInMiss='Y'
				mSqlStr = []
				If sql_con > 0 And Used('tmptbl_vw')
					Select tmptbl_vw
					If Reccount() <= 0
						vFoundInMiss='N'
						Select Gen_miss_vw
						mSqlStr = oSqlConObj.GenInsert("gen_miss","","","Gen_miss_vw",mvu_backend)
						sql_con = oSqlConObj.DataConn([EXE],vcDbName,mSqlStr,[],;
							vnHandle,nDataSessionId,.T.)
					Else
						vFoundInMiss=tmptbl_vw.Flag
						If vFoundInMiss = 'N'
							Select Gen_miss_vw
							Do Case
							Case v_i_s_type = 'DAYWISE'
								mCond = "Entry_ty = ?Gen_miss_vw.Entry_ty And Inv_sr = ?Gen_miss_vw.Inv_sr And ;
									Inv_no = ?Gen_miss_vw.Inv_no And Inv_dt = ?Gen_miss_vw.Inv_dt"
							Case v_i_s_type = 'MONTHWISE'
								mCond = "Entry_ty = ?Gen_miss_vw.Entry_ty And Inv_sr = ?Gen_miss_vw.Inv_sr And ;
									Inv_no = ?Gen_miss_vw.Inv_no And MONTH(Inv_dt) = ?MONTH(Gen_miss_vw.Inv_dt) And ;
									Year(Inv_dt) = ?Year(Gen_miss_vw.Inv_dt)"
							Otherwise
								mCond = "Entry_ty = ?Gen_miss_vw.Entry_ty And Inv_sr = ?Gen_miss_vw.Inv_sr And ;
									Inv_no = ?Gen_miss_vw.Inv_no And L_yn = ?Gen_miss_vw.L_yn"
							Endcase
							mSqlStr = oSqlConObj.GenUpdate("gen_miss","","","Gen_miss_vw",;
								mvu_backend,mCond,"inv_no,flag")		&&03/01/2008
							sql_con = oSqlConObj.DataConn([EXE],vcDbName,mSqlStr,[],;
								vnHandle,nDataSessionId,.T.)
						Endif
					Endif
				Endif
				If vFoundInMiss='N'
					sql_con = oSqlConObj._SqlCommit(vnHandle)
					If sql_con > 0
						sql_main = Entry_tbl+'Main'
						Select Gen_inv_vw
						minv_no = Padl(Alltrim(Str(Gen_inv_vw.Inv_no)),vinv_size,"0")	&&test_z vinv_size
						minv_no = Padr(v_i_prefix+v_i_middle+minv_no+v_i_suffix,Len(Tmp_Main.Inv_no),' ')
						sql_con = oSqlConObj.DataConn([EXE],vcDbName,[ Select Top 1 Entry_ty from ]+;
							sql_main+[ where Entry_ty = ?Gen_inv_vw.Entry_ty And Inv_sr = ?Gen_inv_vw.Inv_sr And ;
							Inv_no = ?minv_no And L_yn = ?Gen_inv_vw.L_yn ],[tmptbl_vw],;
							vnHandle,nDataSessionId,.T.)
						If sql_con > 0 And Used('tmptbl_vw')
							Select tmptbl_vw
							If Reccount() <= 0
								mrollback = .F.
								Exit
							Else
								vFoundInMiss='Y'
							Endif
						Endif
					Endif
				Endif
				If vFoundInMiss='Y'
					Select Gen_inv_vw
					Replace Inv_no With Inv_no + 1 In Gen_inv_vw

					Select Gen_miss_vw
					Replace Inv_no With Gen_inv_vw.Inv_no In Gen_miss_vw
				Endif
			Endif
		Endif
	Enddo
	mSqlStr = []
	If mrollback = .F.
		_vInvoiceEn = Iif(Type('_vInvoiceEn') # 'N',Val(_vInvoiceEn),_vInvoiceEn)
		Select Gen_inv_vw
		Replace Entry_ty With ventryType,Inv_dt With VentDate,Inv_sr With _vInvoiceEs,;
			Inv_no With _vInvoiceEn,L_yn With vctrYear In Gen_inv_vw
		Select Gen_miss_vw
		Replace Entry_ty With Gen_inv_vw.Entry_ty,Inv_dt With Gen_inv_vw.Inv_dt,Inv_sr With Gen_inv_vw.Inv_sr,;
			Inv_no With Gen_inv_vw.Inv_no,L_yn With Gen_inv_vw.L_yn,Flag With 'N' In Gen_miss_vw

		Do Case
		Case v_i_s_type = 'daywise'
			mCond = "entry_ty = ?gen_miss_vw.entry_ty and inv_sr = ?gen_miss_vw.inv_sr and ;
				inv_no = ?gen_miss_vw.inv_no and inv_dt = ?gen_miss_vw.inv_dt"
		Case v_i_s_type = 'monthwise'
			mCond = "entry_ty = ?gen_miss_vw.entry_ty and inv_sr = ?gen_miss_vw.inv_sr and ;
				inv_no = ?gen_miss_vw.inv_no and month(inv_dt) = ?month(gen_miss_vw.inv_dt) and ;
				year(inv_dt) = ?year(gen_miss_vw.inv_dt)"
		Otherwise
			mCond = "entry_ty = ?gen_miss_vw.entry_ty and inv_sr = ?gen_miss_vw.inv_sr and ;
				inv_no = ?gen_miss_vw.inv_no and l_yn = ?gen_miss_vw.l_yn"
		Endcase
		mSqlStr = oSqlConObj.GenUpdate("gen_miss","","","gen_miss_vw",mvu_backend,mCond,"flag")
		sql_con = oSqlConObj.DataConn([exe],vcDbName,mSqlStr,[],;
			vnHandle,nDataSessionId,.T.)
		If sql_con < 0
			mrollback = .T.
		Endif
	Endif
	If mrollback = .T.
		sql_con = oSqlConObj._SqlRollback(vnHandle)
	Endif
Endif
If Used('Gen_inv_vw')
	Select Gen_inv_vw
	Use In Gen_inv_vw
Endif
If Used('Gen_miss_vw')
	Select Gen_miss_vw
	Use In Gen_miss_vw
Endif
*!*	nRetval= oSqlConObj.sqlconnclose("nHandle") && Commented by Shrikant S. on 20/05/2010 for TKT-1476
Wait Clear
Return minv_no
