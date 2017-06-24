&&Changes has been done by vasant on 24/04/2013 as per Bug 11644 (The Updates for 'Service Tax Reverse Mechanism'(BUG-6092) are not working when 'Multi Currency' option is selected for Service Tax Transactions).
&&Changes has been done by vasant on 25/02/2012 as per Bug-6092 (Required "Service Tax REVERSE CHARGE MECHANISM" in our Default Products.)
Lparameters _custvouobj,_udtrigvoutype,_udtrigvoufldnm,_udtrigvoucol
_custalias         = Alias()
_custrecno		   = Recno()
_custdatasessionid = _Screen.ActiveForm.DataSessionId
_custhowtoCalculateExAmt = _custvouobj.howtoCalculateExAmt

fldLen=15		&& Added by Shrikant S. on 14/11/2015 for Swachh Bharat Cess Bug-27242



If _custvouobj.Servicetaxpage = .T. And !Inlist(_custvouobj.pcvtype,'IB')

*!*		IF (_udtrigvoutype = 'ITEM' AND INLIST(UPPER(_udtrigvoufldnm),'ITEM_VW.SERBAMT','ITEM_VW.SERCAMT','ITEM_VW.SERHAMT') AND _udtrigvoucol = 'A' AND _custhowtoCalculateExAmt = 'I')		&& Commented by Shrikant S. on 14/11/2015 for Swachh Bharat Cess Bug-27242
*!*		If (_udtrigvoutype = 'ITEM' And Inlist(Upper(_udtrigvoufldnm),'ITEM_VW.SERBAMT','ITEM_VW.SERCAMT','ITEM_VW.SERHAMT','ITEM_VW.SERBCESS') And _udtrigvoucol = 'A' And _custhowtoCalculateExAmt = 'I')	&& Added by Shrikant S. on 14/11/2015 for Swachh Bharat Cess Bug-27242	&& Commented by Shrikant S. on 24/05/2016 for Bug-28132
*!*		If (_udtrigvoutype = 'ITEM' And Inlist(Upper(_udtrigvoufldnm),'ITEM_VW.SERBAMT','ITEM_VW.SERCAMT','ITEM_VW.SERHAMT','ITEM_VW.SERBCESS','ITEM_VW.SKKCAMT') And _udtrigvoucol = 'A' And _custhowtoCalculateExAmt = 'I')		&& Added by Shrikant S. on 24/05/2016 for Bug-28132	&& Commented by Shrikant S. on 04/10/2016 for GST
	If (_udtrigvoutype = 'ITEM' And Inlist(Upper(_udtrigvoufldnm),'ITEM_VW.SERBAMT','ITEM_VW.SERCAMT','ITEM_VW.SERHAMT','ITEM_VW.SERBCESS','ITEM_VW.SKKCAMT','ITEM_VW.CGST_AMT','ITEM_VW.IGST_AMT','ITEM_VW.SGST_AMT') And _udtrigvoucol = 'A' And _custhowtoCalculateExAmt = 'I')		&& Added by Shrikant S. on 04/10/2016 for GST
		_tmpudtrigvoufldper = 'Item_vw.'+Alltrim(Upper(DcMast_vw.Pert_Name))
		If &_tmpudtrigvoufldper > 0
			custsql_con	= 1
			custsql_str	= ''
			_oldcustconval  = 0
			custnhandle     = 0
			Set DataSession To _custdatasessionid


&& Added by Shrikant S. on 21/12/2016 for GST		&& Start
			If Type('_curvouobj.mainalias') = 'C'
				If Upper(_curvouobj.mainalias) <> 'MAIN_VW'
					Return
				Endif
			Endif
			If !Used("Acdetalloc_vw")
				Return
			Endif

&& Added by Shrikant S. on 21/12/2016 for GST		&& End


			If Type('_custvouobj.sqlconobj') = 'O'
				_custsqlconobj1 = _custvouobj.sqlconobj
			Else
				_custsqlconobj1 = Newobject('SqlConnUdObj','SqlConnection',xapps)
			Endif
			If Type('_custvouobj.nhandle') = 'N'
				_oldcustconval = _custvouobj.nhandle
			Endif
			If _oldcustconval > 0
				custnhandle = _oldcustconval
			Endif
			_custExemptParty = .F.
			custsql_str = "Select Top 1 SerExmptd From Ac_Mast"
			custsql_str = custsql_str + " Where Ac_id = ?Main_vw.Ac_id"
			custsql_con = _custsqlconobj1.dataconn([EXE],company.dbname,custsql_str,"_tmpdata","custnhandle",_custdatasessionid)
			If custsql_con > 0 And Used('_tmpdata')
				_custExemptParty = _tmpdata.serexmptd
			Endif





*lcserty=IIF(INLIST(_custvouobj.pcvtype,"E1","S1"),Item_vw.serty,Acdetalloc_vw.Serty)		&& Added by Shrikant S. on 06/10/2016 for GST
			lcserty=Acdetalloc_vw.Serty
			custsql_str = "Select Top 1 PerPayReceiver From SerTax_Mast"
*!*				custsql_str = custsql_str + " Where Name = ?Acdetalloc_vw.Serty and (?Main_vw.Date Between sdate and edate)"		&& Commented by Shrikant S. on 06/10/2016 for GST
			custsql_str = custsql_str + " Where Name = ?lcserty and (?Main_vw.Date Between sdate and edate)"						&& Added by Shrikant S. on 06/10/2016 for GST

			custsql_con = _custsqlconobj1.dataconn([EXE],company.dbname,custsql_str,"_tmpdata","custnhandle",_custdatasessionid)
			If custsql_con > 0 And Used('_tmpdata')
				_PerPayReceiver = 0
				If !Empty(_tmpdata.PerPayReceiver)
					_PerPayReceiver = Evaluate(_tmpdata.PerPayReceiver)
				Endif
&& Commented by Shrikant S. on 06/10/2016 for GST		&& Start
*!*					If Item_vw.SerBAmt = 0 And Item_vw.SerrBAmt != 0
*!*						_PerPayReceiver = 100
*!*					Endif
&& Commented by Shrikant S. on 06/10/2016 for GST		&& End

&& Commented by Shrikant S. on 09/06/2017 for GST		&& Start
&& Added by Shrikant S. on 22/04/2017 for GST		&& Start					
				IF (INLIST(LOWER(_custvouobj.accregistatus),"unregistered","compounding") ) AND INLIST(main_vw.Entry_ty,"E1","C6","D6","BP","CP")
					_PerPayReceiver = 100
				ENDIF
&& Added by Shrikant S. on 22/04/2017 for GST		&& End
&& Commented by Shrikant S. on 09/06/2017 for GST		&& End

				
				_tmpudtrigvoumulticur = .F.
				If _custvouobj.Multi_Cur   = .T.
					If Upper(Alltrim(main1_vw.Fcname)) != Upper(Alltrim(company.Currency)) And !Empty(main1_vw.Fcname)
						_tmpudtrigvoumulticur = .T.
					Endif
				Endif
				If _tmpudtrigvoumulticur = .T.
					_udtrigvoucalcfldnm = 'Item_vw.'+Alltrim(Upper(DcMast_vw.FcFld_Nm))
				Else
					_udtrigvoucalcfldnm = 'Item_vw.'+Alltrim(Upper(DcMast_vw.Fld_Nm))
				Endif
				_rsertaxableamt = (&_udtrigvoucalcfldnm * _PerPayReceiver)/100


&& Added by Shrikant S. on 14/11/2015 for Swachh Bharat Cess Bug-27242	&& Start
				lnlen=Len(_udtrigvoucalcfldnm)
				If lnlen > (fldLen+Iif(_tmpudtrigvoumulticur=.T.,2,0))
					_tmpudtrigvoufldnm = Substr(_udtrigvoucalcfldnm,1,Len(_udtrigvoucalcfldnm)-4 - (lnlen-(fldLen+Iif(_tmpudtrigvoumulticur=.T.,2,0))))+'R'+Right(_udtrigvoucalcfldnm,4+(lnlen-(fldLen+Iif(_tmpudtrigvoumulticur=.T.,2,0))))
					If Len(_tmpudtrigvoufldnm )>18
						_tmpudtrigvoufldnm =Left(_tmpudtrigvoufldnm,Len(_tmpudtrigvoufldnm)-1)
					Endif
				Else
&& Added by Shrikant S. on 14/11/2015 for Swachh Bharat Cess Bug-27242	&& End
					_tmpudtrigvoufldnm = Substr(_udtrigvoucalcfldnm,1,Len(_udtrigvoucalcfldnm)-4)+'R'+Right(_udtrigvoucalcfldnm,4)
				Endif				&& Added by Shrikant S. on 14/11/2015 for Swachh Bharat Cess Bug-27242
				Replace &_tmpudtrigvoufldnm With _rsertaxableamt In Item_vw
				If _custExemptParty = .T. Or Inlist(Main_vw.SerRule,'EXEMPT')
					Replace &_udtrigvoucalcfldnm With 0 In Item_vw
				Else
					Replace &_udtrigvoucalcfldnm With &_udtrigvoucalcfldnm - &_tmpudtrigvoufldnm In Item_vw
				Endif

				If _tmpudtrigvoumulticur = .T.
&& Added by Shrikant S. on 14/11/2015 for Swachh Bharat Cess Bug-27242	&& Start
					lnlen=Len(_udtrigvoufldnm)
					If lnlen>fldLen
						_tmpinrudtrigvoufldnm = Substr(_udtrigvoufldnm,1,Len(_udtrigvoufldnm)-4 - (lnlen-fldLen))+'R'+Right(_udtrigvoufldnm,4+(lnlen-fldLen))
					Else
&& Added by Shrikant S. on 14/11/2015 for Swachh Bharat Cess Bug-27242	&& End
						_tmpinrudtrigvoufldnm = Substr(_udtrigvoufldnm,1,Len(_udtrigvoufldnm)-4)+'R'+Right(_udtrigvoufldnm,4)
					Endif			&& Added by Shrikant S. on 14/11/2015 for Swachh Bharat Cess Bug-27242
					Replace &_udtrigvoufldnm With &_udtrigvoucalcfldnm*Main_vw.fcexrate,;
						&_tmpinrudtrigvoufldnm With &_tmpudtrigvoufldnm*Main_vw.fcexrate In Item_vw
					If DcMast_vw.round_off
						Replace &_udtrigvoufldnm With Round(&_udtrigvoufldnm,0),;
							&_tmpinrudtrigvoufldnm With Round(&_tmpinrudtrigvoufldnm,0) In Item_vw
					Endif
					_tmpudtrigvoufldnm = _tmpinrudtrigvoufldnm
				Endif

				_r1sertaxableamt = &_tmpudtrigvoufldnm
				_r2sertaxableamt = &_udtrigvoufldnm
				_tmpudtrigvoufldnm = Strtran(Upper(_tmpudtrigvoufldnm),'ITEM_VW.','ACDETALLOC_VW.')
				_udtrigvoufldnm    = Strtran(Upper(_udtrigvoufldnm),'ITEM_VW.','ACDETALLOC_VW.')
				Select Acdetalloc_vw
				Replace &_tmpudtrigvoufldnm With _r1sertaxableamt,;
					&_udtrigvoufldnm With _r2sertaxableamt In Acdetalloc_vw
			Endif
			If _oldcustconval <= 0
				=_custsqlconobj1.sqlconnclose("custnhandle")
			Endif
			Release _custsqlconobj1
		Endif
	Endif
*!*		IF (_udtrigvoutype = 'TRAN' AND INLIST(UPPER(_udtrigvoufldnm),'MAIN_VW.SERBAMT','MAIN_VW.SERCAMT','MAIN_VW.SERHAMT') And INLIST(_custhowtoCalculateExAmt,'V','F'))		&& Commented by Shrikant S. on 14/11/2015 for Swachh Bharat Cess Bug-27242
*!*		If (_udtrigvoutype = 'TRAN' And Inlist(Upper(_udtrigvoufldnm),'MAIN_VW.SERBAMT','MAIN_VW.SERCAMT','MAIN_VW.SERHAMT','MAIN_VW.SERBCESS') And Inlist(_custhowtoCalculateExAmt,'V','F'))	&& added by Shrikant S. on 14/11/2015 for Swachh Bharat Cess Bug-27242	&& Commented by Shrikant S. on 24/05/2016 for Bug-28132
*!*		If (_udtrigvoutype = 'TRAN' And Inlist(Upper(_udtrigvoufldnm),'MAIN_VW.SERBAMT','MAIN_VW.SERCAMT','MAIN_VW.SERHAMT','MAIN_VW.SERBCESS','MAIN_VW.SKKCAMT') And Inlist(_custhowtoCalculateExAmt,'V','F'))	&& Added by Shrikant S. on 24/05/2016 for Bug-28132		&& Commented by Shrikant S. on 04/10/2016 for GST
	If (_udtrigvoutype = 'TRAN' And Inlist(Upper(_udtrigvoufldnm),'MAIN_VW.SERBAMT','MAIN_VW.SERCAMT','MAIN_VW.SERHAMT','MAIN_VW.SERBCESS','MAIN_VW.SKKCAMT','MAIN_VW.CGST_AMT','MAIN_VW.IGST_AMT','MAIN_VW.SGST_AMT') And Inlist(_custhowtoCalculateExAmt,'V','F'))	&& Added by Shrikant S. on 04/10/2016 for GST
		_tmpudtrigvoufldper = Juststem(_udtrigvoufldnm)+'.'+Alltrim(Upper(Tax_vw.Pert_Name))
		If &_tmpudtrigvoufldper > 0
			custsql_con	= 1
			custsql_str	= ''
			_oldcustconval  = 0
			custnhandle     = 0
			Set DataSession To _custdatasessionid

&& Added by Shrikant S. on 21/12/2016 for GST		&& Start
			If Type('_curvouobj.mainalias') = 'C'
				If Upper(_curvouobj.mainalias) <> 'MAIN_VW'
					Return
				Endif
			Endif
			If !Used("Acdetalloc_vw")
				Return
			Endif

&& Added by Shrikant S. on 21/12/2016 for GST		&& End

			If Type('_custvouobj.sqlconobj') = 'O'
				_custsqlconobj1 = _custvouobj.sqlconobj
			Else
				_custsqlconobj1 = Newobject('SqlConnUdObj','SqlConnection',xapps)
			Endif
			If Type('_custvouobj.nhandle') = 'N'
				_oldcustconval = _custvouobj.nhandle
			Endif
			If _oldcustconval > 0
				custnhandle = _oldcustconval
			Endif
			_custmfld_nm  = Upper(Justext(_udtrigvoufldnm))

&& Added by Shrikant S. on 14/11/2015 for Swachh Bharat Cess Bug-27242	&& Start

			lnlen=Len(_custmfld_nm)
			If lnlen>7
				_custsfld_nm  = Substr(_custmfld_nm,1,Len(_custmfld_nm)-4 - (lnlen-7))+'R'+Right(_custmfld_nm,4+(lnlen-7))
			Else
&& Added by Shrikant S. on 14/11/2015 for Swachh Bharat Cess Bug-27242	&& End
				_custsfld_nm  = Substr(_custmfld_nm,1,Len(_custmfld_nm)-4)+'R'+Right(_custmfld_nm,4)
			Endif			&& Added by Shrikant S. on 14/11/2015 for Swachh Bharat Cess Bug-27242


			m_serviceamt = 0
			Select Sum(staxable) As serviceamt From Acdetalloc_vw With (Buffering = .T.) Into Cursor tmptbl_vw Where !Empty(Serty) And !Inlist(Serty,'OTHERS')
			If Used('tmptbl_vw')
				If Isnull(tmptbl_vw.serviceamt) = .F.
					m_serviceamt = tmptbl_vw.serviceamt
				Endif
				Use In tmptbl_vw
			Endif

			If m_serviceamt > 0
				_custfldtotamt = 0
				Select Acdetalloc_vw
				Scan
					If !Empty(Acdetalloc_vw.Serty) And !Inlist(Acdetalloc_vw.Serty,'OTHERS')
						_custfldamt    = (Acdetalloc_vw.staxable * Main_vw.&_custmfld_nm)/m_serviceamt
						Replace &_custmfld_nm With _custfldamt In Acdetalloc_vw
						_custfldtotamt = _custfldtotamt + Acdetalloc_vw.&_custmfld_nm
					Endi
				Endscan
				Select Acdetalloc_vw
				Scan
					If !Empty(Acdetalloc_vw.Serty) And !Inlist(Acdetalloc_vw.Serty,'OTHERS')
						_custfldamt    = (Main_vw.&_custmfld_nm - _custfldtotamt)
						Replace &_custmfld_nm With &_custmfld_nm + _custfldamt In Acdetalloc_vw
						Exit
					Endi
				Endscan
			Endif
			_custExemptParty = .F.
			custsql_str = "Select Top 1 SerExmptd From Ac_Mast"
			custsql_str = custsql_str + " Where Ac_id = ?Main_vw.Ac_id"
			custsql_con = _custsqlconobj1.dataconn([EXE],company.dbname,custsql_str,"_tmpdata","custnhandle",_custdatasessionid)
			If custsql_con > 0 And Used('_tmpdata')
				_custExemptParty = _tmpdata.serexmptd
			Endif

			_rsertaxableamt1 = 0
			_rsertaxableamt2 = 0

			Select Acdetalloc_vw
			Scan
				If !Empty(Acdetalloc_vw.Serty) And !Inlist(Acdetalloc_vw.Serty,'OTHERS')
					custsql_str = "Select Top 1 PerPayReceiver From SerTax_Mast"
					custsql_str = custsql_str + " Where Name = ?Acdetalloc_vw.Serty and (?Main_vw.Date Between sdate and edate)"
					custsql_con = _custsqlconobj1.dataconn([EXE],company.dbname,custsql_str,"_tmpdata","custnhandle",_custdatasessionid)
					If custsql_con > 0 And Used('_tmpdata')
						_PerPayReceiver = 0
						If !Empty(_tmpdata.PerPayReceiver)
							_PerPayReceiver = Evaluate(_tmpdata.PerPayReceiver)
						Endif
&& Commented by Shrikant S. on 06/10/2016 for GST		&& Start
*!*							If Main_vw.SerBAmt = 0 And Main_vw.SerrBAmt != 0
*!*								_PerPayReceiver = 100
*!*							Endif
&& Commented by Shrikant S. on 06/10/2016 for GST		&& End
						Replace &_custsfld_nm With 	(&_custmfld_nm * _PerPayReceiver)/100 In Acdetalloc_vw
						If _custExemptParty = .T. Or Inlist(Main_vw.SerRule,'EXEMPT')
							Replace &_custmfld_nm With 	0 In Acdetalloc_vw
						Else
							Replace &_custmfld_nm With 	&_custmfld_nm - &_custsfld_nm In Acdetalloc_vw
						Endif
						_rsertaxableamt1 = _rsertaxableamt1 + Acdetalloc_vw.&_custmfld_nm
						_rsertaxableamt2 = _rsertaxableamt2 + Acdetalloc_vw.&_custsfld_nm
					Endif
				Endif
				Select Acdetalloc_vw
			Endscan

			_tmpudtrigvoumulticur = .F.
			If _custvouobj.Multi_Cur   = .T.
				If Upper(Alltrim(main1_vw.Fcname)) != Upper(Alltrim(company.Currency)) And !Empty(main1_vw.Fcname)
					_tmpudtrigvoumulticur = .T.
				Endif
			Endif
			Select Main_vw
			Replace &_custmfld_nm With _rsertaxableamt1,&_custsfld_nm With _rsertaxableamt2 In Main_vw
			If _tmpudtrigvoumulticur = .T.
				_custmfcfld_nm  = 'Fc'+Upper(Justext(_udtrigvoufldnm))

&& Added by Shrikant S. on 14/11/2015 for Swachh Bharat Cess Bug-27242	&& Start
				_custfldlen=9
				lnlen=Len(_custmfcfld_nm)
				If lnlen>_custfldlen
					_custsfcfld_nm  = Substr(_custmfcfld_nm,1,Len(_custmfcfld_nm)-4 - (lnlen-_custfldlen))+'R'+Right(_custmfcfld_nm,4+(lnlen-_custfldlen))
*					_custsfcfld_nm=LEFT(_custsfcfld_nm,10)
					If Len(_custsfcfld_nm)>10
						_custsfcfld_nm =Left(_custsfcfld_nm,Len(_custsfcfld_nm)-1)
					Endif
				Else
&& Added by Shrikant S. on 14/11/2015 for Swachh Bharat Cess Bug-27242	&& End

					_custsfcfld_nm  = Substr(_custmfcfld_nm,1,Len(_custmfcfld_nm)-4)+'R'+Right(_custmfcfld_nm,4)
				Endif		&& Added by Shrikant S. on 14/11/2015 for Swachh Bharat Cess Bug-27242
				Replace &_custmfcfld_nm With &_custmfld_nm/Main_vw.fcexrate,;
					&_custsfcfld_nm With &_custsfld_nm/Main_vw.fcexrate In Main_vw
			Endif
			If Used('Tax_vw')
				If Upper(_custmfld_nm) == Upper(Alltrim(Tax_vw.Fld_Nm))
					Select Tax_vw
					_udappvouTax_vwRecNo = Iif(!Eof(),Recno(),0)

					_rsertaxableamt1 = Main_vw.&_custmfld_nm
					Update Tax_vw Set Def_amt = _rsertaxableamt1 Where Upper(Alltrim(Tax_vw.Fld_Nm)) = Upper(_custmfld_nm)
					_rsertaxableamt2 = Main_vw.&_custsfld_nm
					Update Tax_vw  Set Def_amt =  _rsertaxableamt2 Where Upper(Alltrim(Tax_vw.Fld_Nm)) = Upper(_custsfld_nm)
					If _tmpudtrigvoumulticur = .T.
						_rsertaxableamt1 = Main_vw.&_custmfcfld_nm
						Update Tax_vw Set FcDef_amt = _rsertaxableamt1 Where Upper(Alltrim(Tax_vw.Fld_Nm)) = Upper(_custmfld_nm)
						_rsertaxableamt2 = Main_vw.&_custsfcfld_nm
						Update Tax_vw  Set FcDef_amt =  _rsertaxableamt2 Where Upper(Alltrim(Tax_vw.Fld_Nm)) = Upper(_custsfld_nm)
					Endif
					Select Tax_vw
					If _udappvouTax_vwRecNo > 0
						Go _udappvouTax_vwRecNo
					Endif
				Endif
			Endif
			If _oldcustconval <= 0
				=_custsqlconobj1.sqlconnclose("custnhandle")
			Endif
			Release _custsqlconobj1
		Endif
	Endif
Endif

If !Empty(_custalias)
	Select &_custalias
Endif
If Betw(_custrecno,1,Reccount())
	Go _custrecno
Endif
&&Changes has been done by vasant on 25/02/2012 as per Bug-6092 (Required "Service Tax REVERSE CHARGE MECHANISM" in our Default Products.)
&&Changes has been done by vasant on 24/04/2013 as per Bug 11644 (The Updates for 'Service Tax Reverse Mechanism'(BUG-6092) are not working when 'Multi Currency' option is selected for Service Tax Transactions).
