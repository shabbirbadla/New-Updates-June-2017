Para lAboutUs,cRights

&&vasant16/11/2010	Changes done for VU 10 (Standard/Professional/Enterprise)
*!*	NOTE : All product starting with VU has been changed to UV	&&04/05/10

r_coof = 0
r_noof = 0
xvalue		=   ''
reg_prods   =   ''
reg_value	=	''
unreg_msg	=	''
_regform    = 	.F.
r_macid1	= 	''

Local sqlconobj,mudProdCode
sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)
mudProdCode = Dec(NewDecry(GlobalObj.getPropertyval("UdProdCode"),'Ud*yog+1993'))
cPrdVrsn 	= Dec(NewDecry(GlobalObj.getPropertyval("UdProdShortCode"),'Udyog_*Prod_Cd'))

***** Added by Sachin N. S. on 06/12/2016 for GST Vudyog Database Working -- Start
_ProdCode = Cast(GlobalObj.getPropertyval("udProdCode") As Varbinary(250))
lcCondi = ""
lcCondi = lcCondi + Iif(Type("_co_mast.prodCode")="U" And Type("Company.prodCode")="U",""," (prodCode = ?_ProdCode or prodCode is null or prodCode = '' ) ")
***** Added by Sachin N. S. on 06/12/2016 for GST Vudyog Database Working -- End

If Type('apath') = 'U'
	Public apath
	apath = Allt(Sys(5) + Curd())
Endif
If Type('vismainprod') = 'U'
	Public vismainprod

	Declare Integer GetPrivateProfileString In Win32API As GetPrivStr ;
		String cSection, String cKey, String cDefault, String @cBuffer, ;
		Integer nBufferSize, String cINIFile
	Declare Integer WritePrivateProfileString In Win32API As WritePrivStr ;
		String cSection, String cKey, String cValue, String cINIFile
	Declare Integer GetProfileString In Win32API As GetProStr ;
		String cSection, String cKey, String cDefault, ;
		String @cBuffer, Integer nBufferSize
Endif

vismainprod = ''

vufile	= Sys(2000,apath+"*register.me")	&& Added By Sachin N. S. on 19/11/2008
Do Unreg_data

mprod = ''

If File(vufile)
	nFile=Adir(arrFile,apath+vufile)
	If arrFile[1,5]='R'			&& Checking for Register.me File Readonly Attribute
		=Messagebox("Set the property of Register.me to read & write.",0+16,vuMess)
		Return .F.
	Endif
	_regform = .T.
Endif
ueReadRegMe = Createobject("ueReadRegisterMe")
If Type('ueReadRegMe') != 'O'
	=Messagebox("Registration details not found.",0+16,vuMess)
	Return .F.
Endif

_UnqVal = GlobalObj.getPropertyval('UnqVal')
_UnqVal = Substr(Dec(_UnqVal),2,8)
ueReadRegMe._ueReadRegisterMe(apath,_UnqVal)

reg_value		= 	Iif(!Empty(ueReadRegMe.reg_value),ueReadRegMe.reg_value,'NOT DONE')
unreg_msg		=	Icase(Empty(ueReadRegMe.r_srvtype),'Un-registered',;
	Upper(Alltrim(ueReadRegMe.r_srvtype))='PREMIUM','',;
	Upper(Alltrim(ueReadRegMe.r_srvtype))='NORMAL','',;
	Upper(Alltrim(ueReadRegMe.r_srvtype))='SUPPORT','Fictitious - ( Testing Purpose )',;
	Upper(Alltrim(ueReadRegMe.r_srvtype))='MARKETING','Fictitious - ( Demo Purpose )',;
	Upper(Alltrim(ueReadRegMe.r_srvtype))='INSTITUTIONAL','Fictitious - ( Training Purpose )',;
	Upper(Alltrim(ueReadRegMe.r_srvtype))='DEVELOPER',Alltrim(ueReadRegMe.r_svcname)+' Developer Version',;
	Upper(Alltrim(ueReadRegMe.r_srvtype))='VIEWER VERSION','',;  &&Added by Archana K. on 20/03/13 for Bug-7899
'Un-registered')
xvalue = ueReadRegMe.xvalue
r_coof = ueReadRegMe.r_coof
r_noof = ueReadRegMe.r_noof

&& Added By Amrendra for TKT 8121 on 13-06-2011 Start
****Versioning****
Local _VerValidErr,_VerRetVal,_CurrVerVal
_VerValidErr = ""
_VerRetVal  = 'NO'
_CurrVerVal='10.0.0.0' && [VERSIONNUMBER]
Try
	_VerRetVal = AppVerChk('SOFTWARE',_CurrVerVal,Justfname(Sys(16)))
Catch To _VerValidErr
	_VerRetVal  = 'NO'
Endtry

If Type("_VerRetVal")="L"
	cMsgStr="Version Error occured!"
	cMsgStr=cMsgStr+Chr(13)+"Kindly update latest version of "+GlobalObj.getPropertyval("ProductTitle")
	Messagebox(cMsgStr,64,vuMess)
	Return .F.
Endif

If _VerRetVal  = 'NO'
	cMsgStr="Version Error occured!"
	cMsgStr=cMsgStr+Chr(13)+"Kindly update latest version of "+GlobalObj.getPropertyval("ProductTitle")
	Messagebox(cMsgStr,64,vuMess)
	Wait Window '' Nowait
	*!*			READ events
	*!*			CLEAR EVENTS
	Quit
	Return .F.
Endif
****Versioning****

&& Added By Amrendra for TKT 8121 on 13-06-2011 End
&& Commented By Amrendra for TKT 8121 on 13-06-2011 Start
*!*	r_softver		= '1.1.1'
*!*	Local _VerValidErr,_VerRetVal
*!*	_VerValidErr = ""
*!*	_VerRetVal  = .F.
*!*	Try
*!*		_VerRetVal = AppVerChk('SOFTWARE',r_softver)
*!*	Catch To _VerValidErr
*!*		_VerRetVal  = .F.
*!*	Endtry
*!*	If _VerRetVal  = .F.
*!*		=Messagebox("Kindly update latest version of "+GlobalObj.getPropertyval("ProductTitle"),0+16,vuMess)
*!*		Return .F.
*!*	Endif
&& Commented By Amrendra for TKT 8121 on 13-06-2011 End
If File(vufile)
	************ Getting Actual M/c Registration Id ************ Start
	lnCnt = Len(ueReadRegMe.r_MACId)
	cr_MACId = Alltrim(ueReadRegMe.r_MACId)
	r_macid1 = cr_MACId
	r_MACId  = ""
	Do While .T.
		If lnCnt > 0
			r_MACId = Left(cr_MACId,1) + r_MACId
		Else
			Exit
		Endif
		cr_MACId = Substr(cr_MACId,2,Len(cr_MACId)-1)
		lnCnt = lnCnt - 1
	Enddo
	************ Getting Actual M/c Registration Id ************ End
Endif

*!*	If !Inlist(Upper(Alltrim(ueReadRegMe.r_srvtype)),'PREMIUM','NORMAL')  && Commented by Archana K. on 20/2/13 for Bug-7899
If !Inlist(Upper(Alltrim(ueReadRegMe.r_srvtype)),'PREMIUM','NORMAL','VIEWER VERSION') && Changed by Archana K. on 20/2/13 for Bug-7899
	xvalue = ''
	_arrcode = GlobalObj.getPropertyval("MainProduct(1,1)")
	If _arrcode > 0
		_iarrcode = 1
		For _iarrcode = 1 To _arrcode
			_sarrcode = NewDecry(GlobalObj.getPropertyval("MainProddetails("+Alltrim(Str(_iarrcode))+",3)"),'Ud_yog_prod_desc')
			xvalue = xvalue + ','+cPrdVrsn+" "+_sarrcode
		Endfor
	Endif

	_arrcode = GlobalObj.getPropertyval("OptionalProduct(1,1)")
	If _arrcode > 0
		_iarrcode = 1
		For _iarrcode = 1 To _arrcode
			_sarrcode = NewDecry(GlobalObj.getPropertyval("OptionalProddetails("+Alltrim(Str(_iarrcode))+",3)"),'Udyogproddesc')
			xvalue = xvalue + ','+cPrdVrsn+" "+_sarrcode
		Endfor
	Endif

	_arrcode = GlobalObj.getPropertyval("FreeProduct(1,1)")
	If _arrcode > 0
		_iarrcode = 1
		For _iarrcode = 1 To _arrcode
			_sarrcode = NewDecry(GlobalObj.getPropertyval("FreeProddetails("+Alltrim(Str(_iarrcode))+",3)"),'_Udyog*prod_desc')
			xvalue = xvalue + ','+cPrdVrsn+" "+_sarrcode
		Endfor
	Endif

	r_coof = 9999
	***** Added by Sachin N. S. on 11/05/2017 for GST -- Start
	If mudProdCode == 'VudyogGSSDK'
		r_noof = 2
	Else
		***** Added by Sachin N. S. on 11/05/2017 for GST -- End
		r_noof = 9999
	ENDIF
ELSE

	********** Added By Sachin N. S. on 17/02/2012 for Bug-2289 ********** Start
	*!*		If Inlist(mudProdCode,'VudyogSTD','VudyogPRO','VudyogENT','10USquare','10iTax')
	If Inlist(mudProdCode,'VudyogSTD','VudyogPRO','VudyogENT','10USquare','10iTax','VudyogGST')		&& Changed by Sachin N. S. on 09/11/2016 for GST
		vuFile1 = vufile
		cProd=""
		cInfFile = Sys(2000,apath+"*info.inf")
		If !Empty(cInfFile)
			Do readinffile
			If Used('custinfo_vw')
				Select custinfo_vw
				Scan
					Select custinfo_vw
					cProd = cProd + Iif(!Empty(cProd),',','') + Alltrim(custinfo_vw.prodcd)
					Select custinfo_vw
				Endscan
				xvalue = ''
			Endif
		Else
			=Messagebox("Company Information File(.Inf) is missing. Cannot continue...!!!"+Chr(13)+;
				"Please contact your service center for upgrading.",0+64,vuMess)		&& Changed By Sachin N. S. on 23/02/2012 for Bug-2289
			ExitClick = .T.
			Return .F.
		Endif
		vufile=vuFile1

		reg_prods = Strtran(cProd,',','","')
		reg_prods = '"'+reg_prods+'"'
	Endif
	********** Added By Sachin N. S. on 17/02/2012 for Bug-2289 ********** End
Endif
xvalue = Uppe(xvalue)
xvalue1 = xvalue

If !Empty(xvalue)
	*For i = 1 To Len(xvalue) 						&&step 6
	_arrcode = GlobalObj.getPropertyval("MainProduct(1,1)")
	If _arrcode > 0
		_iarrcode = 1
		For _iarrcode = 1 To _arrcode
			_sarrcode = NewDecry(GlobalObj.getPropertyval("MainProddetails("+Alltrim(Str(_iarrcode))+",3)"),'Ud_yog_prod_desc')
			If cPrdVrsn+" "+_sarrcode $ xvalue
				reg_prods = reg_prods + ',"'+NewDecry(GlobalObj.getPropertyval("MainProddetails("+Alltrim(Str(_iarrcode))+",2)")+'"','Ud_yog_prod_desc')
				xvalue = Strtran(xvalue,cPrdVrsn+" "+_sarrcode,"")
			Endif
		Endfor
	Endif

	_arrcode = GlobalObj.getPropertyval("OptionalProduct(1,1)")
	If _arrcode > 0
		_iarrcode = 1
		For _iarrcode = 1 To _arrcode
			_sarrcode = NewDecry(GlobalObj.getPropertyval("OptionalProddetails("+Alltrim(Str(_iarrcode))+",3)"),'Udyogproddesc')
			If cPrdVrsn+" "+_sarrcode $ xvalue
				reg_prods = reg_prods + ',"'+NewDecry(GlobalObj.getPropertyval("OptionalProddetails("+Alltrim(Str(_iarrcode))+",2)")+'"','Udyogproddesc')
				xvalue = Strtran(xvalue,cPrdVrsn+" "+_sarrcode,"")
			Endif
		Endfor
	Endif

	_arrcode = GlobalObj.getPropertyval("FreeProduct(1,1)")
	If _arrcode > 0
		_iarrcode = 1
		For _iarrcode = 1 To _arrcode
			_sarrcode = NewDecry(GlobalObj.getPropertyval("FreeProddetails("+Alltrim(Str(_iarrcode))+",3)"),'_Udyog*prod_desc')
			If cPrdVrsn+" "+_sarrcode $ xvalue
				reg_prods = reg_prods + ',"'+NewDecry(GlobalObj.getPropertyval("FreeProddetails("+Alltrim(Str(_iarrcode))+",2)")+'"')
				xvalue = Strtran(xvalue,cPrdVrsn+" "+_sarrcode,"")
			Endif
		Endfor
	Endif

	If "IT SPBILL" $ xvalue
		reg_prods = reg_prods + ',"vubil"'
		xvalue = Strtran(xvalue,"SPECIAL BILLING","")
		xvalue = Strtran(xvalue,cPrdVrsn+" "+"SPBILL","")
	Endif

	If "U-REPORTER" $ xvalue
		reg_prods = reg_prods + ',"u-reporter"'
		xvalue = Strtran(xvalue,"U-REPORTER","")
	Endif

	*Endfor
Endif

*!*	If ((mudProdCode = 'iTax' And ('US ' $ xvalue1 Or 'UV ' $ xvalue1 Or 'VS ' $ xvalue1 )) Or (mudProdCode = 'USquare' And ('IT ' $ xvalue1 Or 'UV ' $ xvalue1 Or 'VS ' $ xvalue1)) Or (Inlist(mudProdCode,'VudyogMFG','VudyogTRD') And ('IT ' $ xvalue1 Or 'US ' $ xvalue1 Or 'VS ' $ xvalue1)) Or (mudProdCode = 'VudyogServiceTax' And ('IT ' $ xvalue1 Or 'UV ' $ xvalue1 Or 'US ' $ xvalue1)) Or !(Upper(Left(ueReadRegMe.xvalue,Len(cPrdVrsn))) == Upper(cPrdVrsn))) And Inlist(Upper(Alltrim(ueReadRegMe.r_srvtype)),'PREMIUM','NORMAL') && Commented by Archana K. on 20/2/13 for Bug-7899
If ((mudProdCode = 'iTax' And ('US ' $ xvalue1 Or 'UV ' $ xvalue1 Or 'VS ' $ xvalue1 )) Or (mudProdCode = 'USquare' And ('IT ' $ xvalue1 Or 'UV ' $ xvalue1 Or 'VS ' $ xvalue1)) Or (Inlist(mudProdCode,'VudyogMFG','VudyogTRD') And ('IT ' $ xvalue1 Or 'US ' $ xvalue1 Or 'VS ' $ xvalue1)) Or (mudProdCode = 'VudyogServiceTax' And ('IT ' $ xvalue1 Or 'UV ' $ xvalue1 Or 'US ' $ xvalue1)) Or !(Upper(Left(ueReadRegMe.xvalue,Len(cPrdVrsn))) == Upper(cPrdVrsn))) And Inlist(Upper(Alltrim(ueReadRegMe.r_srvtype)),'PREMIUM','NORMAL','VIEWER VERSION')&& Changed by Archana K. on 20/2/13 for Bug-7899

	vumessr = Iif((mudProdCode = 'iTax' And ('US ' $ xvalue1 Or 'UV ' $ xvalue1 Or 'VS ' $ xvalue1)),'iTax',Iif((mudProdCode = 'USquare' And ('IT ' $ xvalue1 Or 'UV ' $ xvalue1 Or 'VS ' $ xvalue1)),'USquare',Iif((mudProdCode = 'VudyogServiceTax' And ('IT ' $ xvalue1 Or 'UV ' $ xvalue1 Or 'US ' $ xvalue1)),'VudyogServiceTax',mudProdCode)))
	Do Unreg_data
	If _regform = .T.
		=Messagebox("Registration file not of "+vumessr,32,vuMess)
		If !lAboutUs
			ExitClick = .T.
			Return .F.
		Endif
	ENDIF
ELSE
	&&Changes has been done as per TKT-9721 by Vasant on 30/09/2011
	If _regform = .T.
		*!*			If !Upper(Alltrim(ueReadRegMe.r_srvtype))='DEVELOPER' And Inlist(mudProdCode,'VudyogSDK')
		If !Upper(Alltrim(ueReadRegMe.r_srvtype))='DEVELOPER' And Inlist(mudProdCode,'VudyogSDK','VudyogGSSDK')		&& Changed by Sachin N. S. on 11/05/2017 for GST
			=Messagebox("For this Product, Only Developer Version Registration is allowed",32,vuMess)
			ExitClick = .T.
			Return .F.
		Endif
	Endif
	&&Changes has been done as per TKT-9721 by Vasant on 30/09/2011
	
	*********** Company Check ************* Start
	nHandle = 0
	mSqlStr = " If Not Exists(select [Name] from vudyog..sysobjects where xtype = 'U' and [name] = 'register') "+;
		" Begin "+;
		" create table vudyog..Register ( appsrvno numeric(4), userno numeric(4), dbsrvnm varChar(20), dbsrvid varChar(20), "+;
		"      appsrvnm varChar(20), appsrvid varChar(20), regd varChar(15), Status varChar(10), UProduct Varchar(25), MacRegId Varchar(25)) "+;
		" End "
	nretval = sqlconobj.dataconn("EXE","vUdyog",mSqlStr,"_sysobjects","nHandle")
	If nretval<0
		Return .F.
	Endif
	mRet=sqlconobj.sqlconnclose("nhandle")		&& Added By Sachin N. S. on 08/07/2010 for TKT-1473
	If mRet< 0
		Return .F.
	Endif

	*********** Company Check ************* End
	If (reg_value # 'DONE' And _regform = .T.) And (lAboutUs=.F.)	&& Changed By Sachin N. S. on 04/01/2009
		Do Form frmRegistration1
		Read Events
	Endif
	If lAboutUs=.T.				&& Added By Sachin N. S. on 09/07/2009
		If File(vufile)			&& Added By Sachin N. S. on 26/05/2011 for TKT-8128 Multi-Company
			Do Form frmRegistration2
		Else					&& Added By Sachin N. S. on 26/05/2011 for TKT-8128 Multi-Company Start
			=Messagebox("Register.me file not found.",0+64,vuMess)
		Endif					&& Added By Sachin N. S. on 26/05/2011 for TKT-8128 Multi-Company End
	ENDIF
Endif

If File(vufile) And reg_value # 'DONE'
	r_instdate1 = ueReadRegMe.r_instdate		&& Commented By SAchin N. S. on 15/09/2009

	nHandle = 0
	mSqlStr = " select getDate() as CurSysDate "
	nretval = sqlconobj.dataconn("EXE","vUdyog",mSqlStr,"_CurSysDate","nHandle")
	If nretval<0
		Return .F.
	Endif
	dCurDate = enc(Alltrim(Dtoc(Ttod(_CurSysDate.CurSysDate))))

	****** Changed by Sachin N. S. on 06/12/2016 for GST Vudyog Database Working -- Start
	*!*		mSqlStr = "	update vudyog..co_mast set coFormdt=ISNULL(coFormdt,'') "+;
	*!*			" If Not Exists(select top 1 [coformdt] from vudyog..co_mast where coformdt<>'') "+;
	*!*			" Begin "+;
	*!*			"    update vudyog..co_mast set coFormDt = '"+dCurDate+"' where coformdt='' "+;
	*!*			" End "+;
	*!*			" select top 1 [coformdt] from vudyog..co_mast where coformdt<>'' "

	mSqlStr = "	update vudyog..co_mast set coFormdt=ISNULL(coFormdt,'') "+Iif(!Empty(lcCondi)," Where "+lcCondi,"")+;
		" If Not Exists(select top 1 [coformdt] from vudyog..co_mast where coformdt<>'' "++Iif(!Empty(lcCondi)," and "+lcCondi,"")+") "+;
		" Begin "+;
		"    update vudyog..co_mast set coFormDt = '"+dCurDate+"' where coformdt='' "+Iif(!Empty(lcCondi)," and "+lcCondi,"")+;
		" End "+;
		" select top 1 [coformdt] from vudyog..co_mast where coformdt<>'' "+Iif(!Empty(lcCondi)," and "+lcCondi,"")
	****** Changed by Sachin N. S. on 06/12/2016 for GST Vudyog Database Working -- End
	nretval = sqlconobj.dataconn("EXE","vudyog",mSqlStr,"_coformdt","nHandle")
	If nretval<0
		Return .F.
	Endif
	mRet=sqlconobj.sqlconnclose("nhandle")		&& Added By Sachin N. S. on 08/07/2010 for TKT-1473
	If mRet< 0
		Return .F.
	Endif
	r_instdate1 = Iif(Type("_coFormDt.coFormDt")="T",Dtoc(Ttod(_coFormDt.coFormDt)),Dec(Alltrim(_coFormDt.coFormDt)))
	Do Unreg_data
Else
	nHandle = 0
	mSqlStr = " if Not Exists(select name from syscolumns where id = object_id('vudyog..co_mast') and name = 'coFormdt') "+;
		" Begin "+;
		" 	 Alter table vudyog..Co_Mast Add coFormdt varchar(25) "+;
		" End "
	nretval = sqlconobj.dataconn("EXE","vudyog",mSqlStr,"","nHandle")
	If nretval<0
		Return .F.
	Endif

	*!*		nHandle = 0
	mSqlStr = " select getDate() as CurSysDate "
	nretval = sqlconobj.dataconn("EXE","vUdyog",mSqlStr,"_CurSysDate","nHandle")
	If nretval<0
		Return .F.
	Endif
	dCurDate = enc(Alltrim(Dtoc(Ttod(_CurSysDate.CurSysDate))))
	*!*		nHandle = 0
	****** Changed by Sachin N. S. on 06/12/2016 for GST Vudyog Database Working -- Start
	*!*		mSqlStr = "	 update vudyog..co_mast set coFormdt=ISNULL(coFormdt,'') "+;
	*!*			" If Not Exists(select top 1 [coformdt] from vudyog..co_mast where coformdt<>'') "+;
	*!*			" Begin "+;
	*!*			"    update vudyog..co_mast set coFormDt = '"+dCurDate+"' where coformdt='' "+;
	*!*			" End "+;
	*!*			" select top 1 [coformdt] from vudyog..co_mast where coformdt<>'' "
	mSqlStr = "	 update vudyog..co_mast set coFormdt=ISNULL(coFormdt,'') "+Iif(!Empty(lcCondi)," Where "+lcCondi,"")+;
		" If Not Exists(select top 1 [coformdt] from vudyog..co_mast where coformdt<>'' "+Iif(!Empty(lcCondi)," and "+lcCondi,"")+") "+;
		" Begin "+;
		"    update vudyog..co_mast set coFormDt = '"+dCurDate+"' where coformdt='' "+Iif(!Empty(lcCondi)," and "+lcCondi,"")+;
		" End "+;
		" select top 1 [coformdt] from vudyog..co_mast where coformdt<>'' "+Iif(!Empty(lcCondi)," and "+lcCondi,"")
	****** Changed by Sachin N. S. on 06/12/2016 for GST Vudyog Database Working -- End

	nretval = sqlconobj.dataconn("EXE","vudyog",mSqlStr,"_coformdt","nHandle")
	If nretval<0
		Return .F.
	Endif
	mRet=sqlconobj.sqlconnclose("nhandle")		&& Added By Sachin N. S. on 08/07/2010 for TKT-1473
	If mRet< 0
		Return .F.
	Endif
	r_instdate1 = Iif(Type("_coFormDt.coFormDt")="T",Dtoc(Ttod(_coFormDt.coFormDt)),Dec(Alltrim(_coFormDt.coFormDt)))
Endif


Proc Unreg_data
	r_coof = 9999
	r_noof = 9999
	xvalue		=   ''
	reg_prods   =   ''
	reg_value	=	'NOT DONE'
	unreg_msg	=	'Un-registered'


	*!*	*!*	LPARAMETERS cRights

	*!*	*!*	DO FORM frmRegistration WITH cRights
	*!*	*!*	NOTE : All product starting with VU has been changed to UV	&&04/05/10
	*!*	**************** vurInfo **************** Start

	*!*	Para lAboutUs,cRights
	*!*	r_compn     =   ''
	*!*	r_comp		=	''
	*!*	r_user		=	''
	*!*	r_add1		=	''
	*!*	r_add2		=	''
	*!*	r_add3		=	''
	*!*	r_add4		=	''
	*!*	r_city		=	''
	*!*	r_state		=	''
	*!*	r_location	=	''
	*!*	r_phone     =   ''
	*!*	r_servcent	=	''
	*!*	r_instdate	=	''
	*!*	xvalue		=   ''
	*!*	r_coof		=	0
	*!*	r_noof		=	0
	*!*	r_idno		=	''
	*!*	r_pid		=   ''
	*!*	r_servcont	=	''
	*!*	r_servadd1	=	''
	*!*	r_servadd2	=	''
	*!*	r_servadd3	=	''
	*!*	r_servcity	=	''
	*!*	r_servzip	=	''
	*!*	r_servphone	=	''
	*!*	r_servemail	=	''
	*!*	reg_value	=	''
	*!*	reg_prods   =   ''
	*!*	_regform	= 	.F.


	*!*	r_Route		=   ''
	*!*	r_zip		= 	''
	*!*	r_contact	=	''
	*!*	r_email		=	''
	*!*	r_tel1		= 	''
	*!*	r_tel2		= 	''
	*!*	r_mob		= 	''
	*!*	r_fax		= 	''
	*!*	r_web		= 	''
	*!*	r_billadd	= 	''
	*!*	r_Billadd1	=	''
	*!*	r_Billadd2	=	''
	*!*	r_Billadd3	=	''
	*!*	r_Billadd4	=	''
	*!*	r_Billlocation	=   ''
	*!*	r_BillRoute		=   ''
	*!*	r_Billzip		= 	''
	*!*	r_Billcontact	=	''
	*!*	r_Billemail	=	''
	*!*	r_Billtel1	= 	''
	*!*	r_Billtel2	= 	''
	*!*	r_Billmob	= 	''
	*!*	r_Billfax	= 	''
	*!*	r_Billweb	= 	''
	*!*	r_srvtype	= 	''
	*!*	r_regtype	=	''
	*!*	r_InstTime	=	''
	*!*	r_Business	=	''
	*!*	r_ProdManu	=	''
	*!*	r_clientid	= 	''
	*!*	r_ecode		= 	''
	*!*	r_ename		=	''
	*!*	r_svccode	=	''
	*!*	r_svcname	=	''
	*!*	r_Appcnt	=	0
	*!*	r_dbsrvname 	=	''
	*!*	r_dbsrvIp 		=	''
	*!*	r_Apsrvname 	=	''
	*!*	r_ApsrvIp 	=	''
	*!*	r_MACId		=	''
	*!*	r_MACId1	=	''
	*!*	r_ExpDt 	=	''
	*!*	r_AmcStDt 	=	''
	*!*	r_AmcEdDt 	=	''
	*!*	reg_date	= 	''
	*!*	unreg_msg	=	'Un-registered'



	*!*	Local sqlconobj,mudProdCode
	*!*	sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)
	*!*	mudProdCode=dec(GlobalObj.getpropertyval("UdProdCode"))

	*!*	If Type('apath') = 'U'
	*!*		Public apath
	*!*		apath = Allt(Sys(5) + Curd())
	*!*	Endif
	*!*	If Type('vismainprod') = 'U'
	*!*		Public vismainprod

	*!*		Declare Integer GetPrivateProfileString In Win32API As GetPrivStr ;
	*!*			String cSection, String cKey, String cDefault, String @cBuffer, ;
	*!*			Integer nBufferSize, String cINIFile
	*!*		Declare Integer WritePrivateProfileString In Win32API As WritePrivStr ;
	*!*			String cSection, String cKey, String cValue, String cINIFile
	*!*		Declare Integer GetProfileString In Win32API As GetProStr ;
	*!*			String cSection, String cKey, String cDefault, ;
	*!*			String @cBuffer, Integer nBufferSize
	*!*	Endif

	*!*	*!*	vumess		= [Visual Udyog]
	*!*	*!*	vumessr		= vumess
	*!*	vismainprod = ''
	*!*	*!*	vufile      = 'register.me'
	*!*	*vufile      = 'uregister.me'

	*!*	vufile	= Sys(2000,apath+"*register.me")	&& Added By Sachin N. S. on 19/11/2008
	*!*	Do Unreg_data

	*!*	mprod = ''

	*!*	If File(vufile)
	*!*		nFile=Adir(arrFile,apath+vufile)
	*!*		If arrFile[1,5]='R'			&& Checking for Register.me File Readonly Attribute
	*!*			=Messagebox("Set the property of Register.me to read & write.",0+16,vuMess)
	*!*			Return .F.
	*!*		Endif

	*!*		_regform = .T.


	*!*	*********** To be Changed By Sachin N. S.*********** Start
	*!*	*!*		_flopen = Fopen(vufile,10)

	*!*	*!*		If _flopen > 0

	*!*	*!*			r_compn		=	dec((Fread(_flopen,50)))
	*!*	*!*			r_comp   	=	r_compn

	*!*	*!*		********* Installation
	*!*	*!*			r_add		= 	Alltrim(dec((Fread(_flopen,200))))
	*!*	*!*			r_add1		=	Substr(r_add,1,50)
	*!*	*!*			r_add2		=	Substr(r_add,51,100)
	*!*	*!*			r_add3		=	Substr(r_add,101,150)
	*!*	*!*			r_add4		=	Substr(r_add,151,200)
	*!*	*!*			r_location	=   dec((Fread(_flopen,50)))
	*!*	*!*			r_Route		=   dec((Fread(_flopen,50)))				&& New
	*!*	*!*			r_zip		= 	dec((Fread(_flopen,50)))				&& New
	*!*	*!*			r_contact	=	dec((Fread(_flopen,50)))				&& New
	*!*	*!*			r_email		=	dec((Fread(_flopen,100)))				&& New
	*!*	*!*			r_tel1		= 	dec((Fread(_flopen,50)))				&& New
	*!*	*!*			r_tel2		= 	dec((Fread(_flopen,50)))				&& New
	*!*	*!*			r_mob		= 	dec((Fread(_flopen,50)))				&& New
	*!*	*!*			r_fax		= 	dec((Fread(_flopen,50)))				&& New
	*!*	*!*			r_web		= 	dec((Fread(_flopen,50)))				&& New

	*!*	*!*		********* Billing
	*!*	*!*			r_billadd		= 	Alltrim(dec((Fread(_flopen,200))))
	*!*	*!*			r_Billadd1		=	Substr(r_billadd,1,50)
	*!*	*!*			r_Billadd2		=	Substr(r_billadd,51,100)
	*!*	*!*			r_Billadd3		=	Substr(r_billadd,101,150)
	*!*	*!*			r_Billadd4		=	Substr(r_billadd,151,200)
	*!*	*!*			r_Billlocation	=   dec((Fread(_flopen,50)))
	*!*	*!*			r_BillRoute		=   dec((Fread(_flopen,50)))			&& New
	*!*	*!*			r_Billzip		= 	dec((Fread(_flopen,50)))			&& New
	*!*	*!*			r_Billcontact	=	dec((Fread(_flopen,50)))			&& New
	*!*	*!*			r_Billemail		=	dec((Fread(_flopen,50)))			&& New
	*!*	*!*			r_Billtel1		= 	dec((Fread(_flopen,50)))			&& New
	*!*	*!*			r_Billtel2		= 	dec((Fread(_flopen,50)))			&& New
	*!*	*!*			r_Billmob		= 	dec((Fread(_flopen,50)))			&& New
	*!*	*!*			r_Billfax		= 	dec((Fread(_flopen,50)))			&& New
	*!*	*!*			r_Billweb		= 	dec((Fread(_flopen,50)))			&& New

	*!*	*!*			r_srvtype		= 	dec((Fread(_flopen,50)))			&& New
	*!*	*!*			r_regtype		=	dec((Fread(_flopen,50)))			&& New
	*!*	*!*			r_instdate		=	dec((Fread(_flopen,10)))
	*!*	*!*			r_InstTime		=	dec((Fread(_flopen,50)))			&& New
	*!*	*!*			r_Business		=	dec((Fread(_flopen,100)))			&& New
	*!*	*!*			r_ProdManu		=	dec((Fread(_flopen,100)))			&& New

	*!*	*!*			xvalue			=	dec((Fread(_flopen,200)))
	*!*	*!*			r_idno			=	dec((Fread(_flopen,50)))
	*!*	*!*			r_clientid		= 	dec((Fread(_flopen,15)))
	*!*	*!*			r_ecode			= 	dec((Fread(_flopen,50)))
	*!*	*!*			r_ename			=	dec((Fread(_flopen,50)))
	*!*	*!*			r_svccode		=	dec((Fread(_flopen,50)))
	*!*	*!*			r_svcname		=	dec((Fread(_flopen,50)))

	*!*	*!*			r_coof			=	Val(dec((Fread(_flopen,10))))
	*!*	*!*			r_noof			= 	Val(dec((Fread(_flopen,10))))
	*!*	*!*			r_Appcnt		=	Val(dec((Fread(_flopen,10))))		&& New
	*!*	*!*			r_pid       	=	dec((Fread(_flopen,10)))

	*!*	*!*			r_dbsrvname 	=	dec((Fread(_flopen,50)))			&& New
	*!*	*!*			r_dbsrvIp 		=	dec((Fread(_flopen,20)))			&& New
	*!*	*!*			r_Apsrvname 	=	dec((Fread(_flopen,50)))			&& New
	*!*	*!*			r_ApsrvIp 		=	dec((Fread(_flopen,20)))			&& New
	*!*	*!*			r_ExpDt 		=	dec((Fread(_flopen,25)))			&& New
	*!*	*!*			r_MACId			=	dec((Fread(_flopen,50)))			&& Added By Sachin N. S. on 04/07/2009
	*!*	*!*
	*!*	*!*			r_AMCStDt		=	dec((Fread(_flopen,25)))
	*!*	*!*			r_AMCEdDt		=	dec((Fread(_flopen,25)))
	*!*	*!*
	*!*	*!*			reg_date		= 	dec((Fread(_flopen,10)))			&& New
	*!*	*!*			reg_value   	= 	dec((Fread(_flopen,8)))				&& New
	*!*	*!*			reg_value		= 	Iif(!Empty(reg_value),reg_value,'NOT DONE')
	*!*	*!*			_flopen 		= 	Fclose(_flopen)
	*!*	*********** To be Changed By Sachin N. S.*********** End

	*!*			****** New Code ****** Start
	*!*
	*!*			cString = Filetostr(vuFile)

	*!*			r_compn		=	dec(Substr(cString,1,50))
	*!*			r_comp   	=	r_compn

	*!*			********* Installation
	*!*			r_add		= 	Alltrim(dec(Substr(cString,51,200)))
	*!*			r_add1		=	Substr(r_add,1,50)
	*!*			r_add2		=	Substr(r_add,51,100)
	*!*			r_add3		=	Substr(r_add,101,150)
	*!*			r_add4		=	Substr(r_add,151,200)
	*!*			r_location	=   dec(Substr(cString,251,50))
	*!*			r_Route		=   dec(Substr(cString,301,50))				&& New
	*!*			r_zip		= 	dec(Substr(cString,351,50))				&& New
	*!*			r_contact	=	dec(Substr(cString,401,50))				&& New
	*!*			r_email		=	dec(Substr(cString,451,100))				&& New
	*!*			r_tel1		= 	dec(Substr(cString,551,50))				&& New
	*!*			r_tel2		= 	dec(Substr(cString,601,50))				&& New
	*!*			r_mob		= 	dec(Substr(cString,651,50))				&& New
	*!*			r_fax		= 	dec(Substr(cString,701,50))				&& New
	*!*			r_web		= 	dec(Substr(cString,751,50))				&& New

	*!*			********* Billing
	*!*			r_billadd		= 	Alltrim(dec(Substr(cString,801,200)))
	*!*			r_Billadd1		=	Substr(r_billadd,1,50)
	*!*			r_Billadd2		=	Substr(r_billadd,51,100)
	*!*			r_Billadd3		=	Substr(r_billadd,101,150)
	*!*			r_Billadd4		=	Substr(r_billadd,151,200)
	*!*			r_Billlocation	=   dec(Substr(cString,1001,50))
	*!*			r_BillRoute		=   dec(Substr(cString,1051,50))			&& New
	*!*			r_Billzip		= 	dec(Substr(cString,1101,50))			&& New
	*!*			r_Billcontact	=	dec(Substr(cString,1151,50))			&& New
	*!*			r_Billemail		=	dec(Substr(cString,1201,50))			&& New
	*!*			r_Billtel1		= 	dec(Substr(cString,1251,50))			&& New
	*!*			r_Billtel2		= 	dec(Substr(cString,1301,50))			&& New
	*!*			r_Billmob		= 	dec(Substr(cString,1351,50))			&& New
	*!*			r_Billfax		= 	dec(Substr(cString,1401,50))			&& New
	*!*			r_Billweb		= 	dec(Substr(cString,1451,50))			&& New

	*!*			r_srvtype		= 	dec(Substr(cString,1501,50))			&& New
	*!*			r_regtype		=	dec(Substr(cString,1551,50))			&& New
	*!*			r_instdate		=	dec(Substr(cString,1601,10))
	*!*			r_InstTime		=	dec(Substr(cString,1611,50))			&& New
	*!*			r_Business		=	dec(Substr(cString,1661,100))			&& New
	*!*			r_ProdManu		=	dec(Substr(cString,1761,100))			&& New

	*!*			xvalue			=	dec(Substr(cString,1861,200))
	*!*			r_idno			=	dec(Substr(cString,2061,50))
	*!*			r_clientid		= 	dec(Substr(cString,2111,15))
	*!*			r_ecode			= 	dec(Substr(cString,2126,50))
	*!*			r_ename			=	dec(Substr(cString,2176,50))
	*!*			r_svccode		=	dec(Substr(cString,2226,50))
	*!*			r_svcname		=	dec(Substr(cString,2276,50))

	*!*			&&04/05/10
	*!*	*!*			r_coof			=	Val(dec(Substr(cString,2326,10)))
	*!*	*!*			r_noof			= 	Val(dec(Substr(cString,2336,10)))
	*!*			r_coof			=	Val(STRTRAN(dec(dec(Substr(cString,2326,10))),'COOF',''))
	*!*			r_noof			= 	Val(STRTRAN(dec(dec(Substr(cString,2336,10))),'USOF',''))
	*!*			&&04/05/10
	*!*			*!*	r_Appcnt		=	Val(dec((Fread(_flopen,10))))		&& New
	*!*			r_pid       	=	dec(Substr(cString,2346,10))

	*!*			r_dbsrvname 	=	dec(Substr(cString,2356,50))			&& New
	*!*			r_dbsrvIp 		=	dec(Substr(cString,2406,20))			&& New
	*!*			r_Apsrvname 	=	dec(Substr(cString,2426,50))			&& New
	*!*			r_ApsrvIp 		=	dec(Substr(cString,2476,20))			&& New
	*!*			r_ExpDt 		=	dec(Substr(cString,2496,25))			&& New
	*!*			r_MACId			=	dec(Substr(cString,2521,50))			&& Added By Sachin N. S. on 04/07/2009

	*!*			r_AMCStDt		=	dec(Substr(cString,2571,25))
	*!*			r_AMCEdDt		=	dec(Substr(cString,2596,25))


	*!*			reg_date		= 	dec(Substr(cString,2621,10))			&& New
	*!*			reg_value   	= 	dec(Substr(cString,2631,8))				&& New
	*!*			reg_value		= 	Iif(!Empty(reg_value),reg_value,'NOT DONE')

	*!*			****** New Code ****** End

	*!*			unreg_msg		=	Icase(Upper(Alltrim(r_srvtype))='PREMIUM','Un-registered',Upper(Alltrim(r_srvtype))='NORMAL','Un-registered',Upper(Alltrim(r_srvtype))='SUPPORT','Fictitious - ( Testing Purpose )',Upper(Alltrim(r_srvtype))='MARKETING','Fictitious - ( Demo Purpose )',Upper(Alltrim(r_srvtype))='INSTITUTIONAL','Fictitious - ( Training Purpose )','Un-registered')


	*!*			************ Getting Actual M/c Registration Id ************ Start
	*!*			lnCnt = Len(r_MACId)
	*!*			cr_MACId = Alltrim(r_MACId)
	*!*			r_MACId1 = cr_MACId
	*!*			r_MACId  = ""
	*!*			Do While .T.
	*!*				If lnCnt > 0
	*!*					r_MACId = Left(cr_MACId,1) + r_MACId
	*!*				Else
	*!*					Exit
	*!*				Endif
	*!*				cr_MACId = Substr(cr_MACId,2,Len(cr_MACId)-1)
	*!*				lnCnt = lnCnt - 1
	*!*			Enddo
	*!*			************ Getting Actual M/c Registration Id ************ End

	*!*	*!*		Else		&& If there is any problem in opening the file.
	*!*	*!*			Do Unreg_data
	*!*	*!*			=Messagebox("Cannot open the company registration file.",0+16,vuMess)
	*!*	*!*		Endif

	*!*	Endif

	*!*	*!*	=MESSAGEBOX("Product List : "+xvalue)
	*!*	*!*	=Messagebox("No of Company : "+Transform(r_coof))


	*!*	*!*	r_MACId			=	"001676671503"
	*!*	*!*	r_Apsrvname		=	"Sachin"
	*!*	*!*	reg_value		= 	"DONE"
	*!*	*!*	r_srvtype		=	"NORMAL"
	*!*	*!*	r_coof			=	999
	*!*	*!*	r_MacId			= "BFEBFBFF00000F49"


	*!*	*!*	*!*	If UPPER(mprod) = 'VUEDUVER' And Uppe(xvalue) = 'EDUCATIONAL VERSION'
	*!*	*!*	If Uppe(xvalue) = 'EDUCATIONAL VERSION'
	*!*	If !Inlist(Upper(Alltrim(r_srvtype)),'PREMIUM','NORMAL')
	*!*		xvalue = 'UV EXMFG,UV EXPORT,UV INVENT,UV ORDPROC,UV PROACCT,UV SPBILL,UV FA,UV EXTRD,UV ISD,UV STAX,U-REPORTER,UV TDS'
	*!*		xvalue = xvalue + ',IT CUR,IT EOU,IT EXMFG,IT EXTRD,IT SEZD,IT SEZU,IT STAX,IT VAT,IT XDOC,IT TDS'
	*!*		xvalue = xvalue + ',US CUR,US EOU,US EXMFG,US EXTRD,US FA,US INVENT,US ORDPROC,US SEZD,US SEZU,US STAX,US VAT,US XDOC,US TDS,VS STAX,VS TDS'
	*!*		r_coof = 9999
	*!*		r_noof = 9999
	*!*	Endif
	*!*	xvalue = Uppe(xvalue)
	*!*	xvalue1 = xvalue

	*!*	If !Empty(xvalue)
	*!*		For i = 1 To Len(xvalue) 						&&step 6
	*!*			If "IT EXMFG" $ xvalue Or "US EXMFG" $ xvalue Or "UV EXMFG" $ xvalue	&& Or "EV EXMFG" $ xvalue Or "MV EXMFG" $ xvalue Or "SV EXMFG" $ xvalue 						&&and "VISUAL" $ xvalue
	*!*				reg_prods = reg_prods + ',"vuexc"'
	*!*				xvalue = Strtran(xvalue,"EXCISE MANUFACTURING","")
	*!*				xvalue = Strtran(xvalue,"IT EXMFG","")
	*!*				xvalue = Strtran(xvalue,"US EXMFG","")
	*!*				xvalue = Strtran(xvalue,"EV EXMFG","")
	*!*				xvalue = Strtran(xvalue,"MV EXMFG","")
	*!*				xvalue = Strtran(xvalue,"SV EXMFG","")
	*!*				xvalue = Strtran(xvalue,"UV EXMFG","")
	*!*			Endi
	*!*			If "IT XDOC" $ xvalue Or "US XDOC" $ xvalue 	&& Or "EV XDOC" $ xvalue Or "MV XDOC" $ xvalue Or "SV XDOC" $ xvalue 						&&and "VISUAL" $ xvalue
	*!*				reg_prods = reg_prods + ',"vuexp"'
	*!*				xvalue = Strtran(xvalue,"EXPORT","")
	*!*				xvalue = Strtran(xvalue,"IT XDOC","")
	*!*				xvalue = Strtran(xvalue,"US XDOC","")
	*!*				xvalue = Strtran(xvalue,"EV XDOC","")
	*!*				xvalue = Strtran(xvalue,"MV XDOC","")
	*!*				xvalue = Strtran(xvalue,"SV XDOC","")
	*!*			Endi
	*!*			If "IT INVENT" $ xvalue Or "US INVENT" $ xvalue 		&& Or "EV INVENT" $ xvalue Or "MV INVENT" $ xvalue Or "SV INVENT" $ xvalue					&&and "VISUAL" $ xvalue
	*!*				reg_prods = reg_prods + ',"vuinv"'
	*!*				xvalue = Strtran(xvalue,"INVENTORY","")
	*!*				xvalue = Strtran(xvalue,"IT INVENT","")
	*!*				xvalue = Strtran(xvalue,"US INVENT","")
	*!*				xvalue = Strtran(xvalue,"EV INVENT","")
	*!*				xvalue = Strtran(xvalue,"MV INVENT","")
	*!*				xvalue = Strtran(xvalue,"SV INVENT","")
	*!*			Endi
	*!*			If "IT ORDPROC" $ xvalue Or "US ORDPROC" $ xvalue 	&& Or "EV ORDPROC" $ xvalue Or "MV ORDPROC" $ xvalue Or "SV ORDPROC" $ xvalue					&&and "VISUAL" $ xvalue
	*!*				reg_prods = reg_prods + ',"vuord"'
	*!*				xvalue = Strtran(xvalue,"ORDER PROCESSING","")
	*!*				xvalue = Strtran(xvalue,"IT ORDPROC","")
	*!*				xvalue = Strtran(xvalue,"US ORDPROC","")
	*!*				xvalue = Strtran(xvalue,"EV ORDPROC","")
	*!*				xvalue = Strtran(xvalue,"MV ORDPROC","")
	*!*				xvalue = Strtran(xvalue,"SV ORDPROC","")
	*!*			Endi
	*!*			If "IT VAT" $ xvalue Or "US VAT" $ xvalue Or "UV VAT" $ xvalue		&& Or "EV VAT" $ xvalue Or "MV VAT" $ xvalue	Or "SV VAT" $ xvalue 						&&and "VISUAL" $ xvalue
	*!*				reg_prods = reg_prods + ',"vupro"'
	*!*				xvalue = Strtran(xvalue,"PROFESSIONAL ACCOUNTING","")
	*!*				xvalue = Strtran(xvalue,"IT VAT","")
	*!*				xvalue = Strtran(xvalue,"US VAT","")
	*!*				xvalue = Strtran(xvalue,"EV VAT","")
	*!*				xvalue = Strtran(xvalue,"MV VAT","")
	*!*				xvalue = Strtran(xvalue,"SV VAT","")
	*!*			Endi
	*!*			If "IT SPBILL" $ xvalue 											&&and "VISUAL" $ xvalue
	*!*				reg_prods = reg_prods + ',"vubil"'
	*!*				xvalue = Strtran(xvalue,"SPECIAL BILLING","")
	*!*				xvalue = Strtran(xvalue,"IT SPBILL","")
	*!*			Endi
	*!*			If "US FA" $ xvalue Or "UV FA" $ xvalue		&& Or "EV FA" $ xvalue Or "MV FA" $ xvalue	Or "SV FA" $ xvalue 											&&and "VISUAL" $ xvalue
	*!*				reg_prods = reg_prods + ',"vuent"'
	*!*				xvalue = Strtran(xvalue,"ENTERPRISE ACCOUNTING","")
	*!*				xvalue = Strtran(xvalue,"US FA","")
	*!*				xvalue = Strtran(xvalue,"EV FA","")
	*!*				xvalue = Strtran(xvalue,"MV FA","")
	*!*				xvalue = Strtran(xvalue,"SV FA","")
	*!*				xvalue = Strtran(xvalue,"UV FA","")
	*!*			Endi
	*!*			If "IT EXTRD" $ xvalue Or "US EXTRD" $ xvalue Or "UV EXTRD" $ xvalue 		&& Or "EV EXTRD" $ xvalue Or "MV EXTRD" $ xvalue	Or "SV EXTRD" $ xvalue 					&&and "VISUAL" $ xvalue
	*!*				reg_prods = reg_prods + ',"vutex"'
	*!*				xvalue = Strtran(xvalue,"EXCISE TRADING","")
	*!*				xvalue = Strtran(xvalue,"IT EXTRD","")
	*!*				xvalue = Strtran(xvalue,"US EXTRD","")
	*!*				xvalue = Strtran(xvalue,"EV EXTRD","")
	*!*				xvalue = Strtran(xvalue,"MV EXTRD","")
	*!*				xvalue = Strtran(xvalue,"SV EXTRD","")
	*!*				xvalue = Strtran(xvalue,"UV EXTRD","")
	*!*			Endi
	*!*			If "IT ISD" $ xvalue Or "US ISD" $ xvalue 	&& Or "EV ISD" $ xvalue Or "MV ISD" $ xvalue	Or "SV ISD" $ xvalue						&&and "VISUAL" $ xvalue
	*!*				reg_prods = reg_prods + ',"vuisd"'
	*!*				xvalue = Strtran(xvalue,"INPUT SERVICE DISTRIBUTOR","")
	*!*				xvalue = Strtran(xvalue,"IT ISD","")
	*!*				xvalue = Strtran(xvalue,"US ISD","")
	*!*				xvalue = Strtran(xvalue,"EV ISD","")
	*!*				xvalue = Strtran(xvalue,"MV ISD","")
	*!*				xvalue = Strtran(xvalue,"SV ISD","")
	*!*			Endi
	*!*			If "IT STAX" $ xvalue Or "US STAX" $ xvalue Or "UV STAX" $ xvalue Or "VS STAX" $ xvalue		&&	Or "EV STAX" $ xvalue Or "MV STAX" $ xvalue	Or "SV STAX" $ xvalue  				&&and "VISUAL" $ xvalue
	*!*				reg_prods = reg_prods + ',"vuser"'
	*!*				xvalue = Strtran(xvalue,"SERVICE TAX","")
	*!*				xvalue = Strtran(xvalue,"IT STAX","")
	*!*				xvalue = Strtran(xvalue,"US STAX","")
	*!*				xvalue = Strtran(xvalue,"EV STAX","")
	*!*				xvalue = Strtran(xvalue,"MV STAX","")
	*!*				xvalue = Strtran(xvalue,"SV STAX","")
	*!*				xvalue = Strtran(xvalue,"UV STAX","")
	*!*				xvalue = Strtran(xvalue,"VS STAX","")
	*!*			Endi
	*!*			If "U-REPORTER" $ xvalue
	*!*				reg_prods = reg_prods + ',"u-reporter"'
	*!*				xvalue = Strtran(xvalue,"U-REPORTER","")
	*!*			Endi
	*!*
	*!*			If "IT CUR" $ xvalue Or "US CUR" $ xvalue 		&& Or "EV CUR" $ xvalue Or "MV CUR" $ xvalue Or "SV CUR" $ xvalue
	*!*				reg_prods = reg_prods + ',"vumcu"'
	*!*				xvalue = Strtran(xvalue,"MULTI-CURRENCY","")
	*!*				xvalue = Strtran(xvalue,"IT CUR","")
	*!*				xvalue = Strtran(xvalue,"US CUR","")
	*!*				xvalue = Strtran(xvalue,"EV CUR","")
	*!*				xvalue = Strtran(xvalue,"MV CUR","")
	*!*				xvalue = Strtran(xvalue,"SV CUR","")
	*!*			Endi
	*!*			If "IT TDS" $ xvalue Or "US TDS" $ xvalue Or "UV TDS" $ xvalue	Or "VS TDS" $ xvalue	&& Or "EV TDS" $ xvalue Or "MV TDS" $ xvalue	Or "SV TDS" $ xvalue  				&&and "VISUAL" $ xvalue
	*!*				reg_prods = reg_prods + ',"vutds"'
	*!*				xvalue = Strtran(xvalue,"TDS","")
	*!*				xvalue = Strtran(xvalue,"IT TDS","")
	*!*				xvalue = Strtran(xvalue,"US TDS","")
	*!*				xvalue = Strtran(xvalue,"EV TDS","")
	*!*				xvalue = Strtran(xvalue,"MV TDS","")
	*!*				xvalue = Strtran(xvalue,"SV TDS","")
	*!*				xvalue = Strtran(xvalue,"UV TDS","")
	*!*				xvalue = Strtran(xvalue,"VS TDS","")
	*!*			Endi

	*!*		Endfor
	*!*	Endif

	*!*	*!*	If ((mudProdCode = 'iTax' And ('US ' $ xvalue1 Or 'UV ' $ xvalue1 Or 'VS ' $ xvalue1)) (mudProdCode = 'VudyogServiceTax' And ('US ' $ xvalue1 Or 'UV ' $ xvalue1 Or 'IT ' $ xvalue1)) Or (mudProdCode = 'USquare' And ('IT ' $ xvalue1 Or 'UV ' $ xvalue1 Or 'VS ' $ xvalue1)) Or (Inlist(mudProdCode,'VudyogMFG','VudyogTRD') And ('IT ' $ xvalue1 Or 'US ' $ xvalue1 Or 'VS ' $ xvalue1)) ) And Inlist(Upper(Alltrim(r_srvtype)),'PREMIUM','NORMAL')
	*!*	If ((mudProdCode = 'iTax' And ('US ' $ xvalue1 Or 'UV ' $ xvalue1 OR 'VS ' $ xvalue1 )) Or (mudProdCode = 'USquare' And ('IT ' $ xvalue1 Or 'UV ' $ xvalue1 OR 'VS ' $ xvalue1)) Or (Inlist(mudProdCode,'VudyogMFG','VudyogTRD') And ('IT ' $ xvalue1 Or 'US ' $ xvalue1 OR 'VS ' $ xvalue1)) Or (mudProdCode = 'VudyogServiceTax' And ('IT ' $ xvalue1 Or 'UV ' $ xvalue1 OR 'US ' $ xvalue1)) ) And Inlist(Upper(Alltrim(r_srvtype)),'PREMIUM','NORMAL')
	*!*		vumessr = Iif((mudProdCode = 'iTax' And ('US ' $ xvalue1 Or 'UV ' $ xvalue1 Or 'VS ' $ xvalue1)),'iTax',Iif((mudProdCode = 'USquare' And ('IT ' $ xvalue1 Or 'UV ' $ xvalue1 Or 'VS ' $ xvalue1)),'USquare',Iif((mudProdCode = 'VudyogServiceTax' And ('IT ' $ xvalue1 Or 'UV ' $ xvalue1 Or 'US ' $ xvalue1)),'VudyogServiceTax',mudProdCode)))
	*!*		Do Unreg_data
	*!*		If _regform = .T.
	*!*			=Messagebox("Registration file not of "+vumessr,32,vuMess)
	*!*			If !lAboutUs
	*!*				ExitClick = .T.
	*!*			Endif
	*!*		Endif
	*!*	Else

	*!*	*********** Company Check ************* Start
	*!*		nHandle = 0
	*!*		mSqlStr = " If Not Exists(select [Name] from vudyog..sysobjects where xtype = 'U' and [name] = 'register') "+;
	*!*			" Begin "+;
	*!*			" create table vudyog..Register ( appsrvno numeric(4), userno numeric(4), dbsrvnm varChar(20), dbsrvid varChar(20), "+;
	*!*			"      appsrvnm varChar(20), appsrvid varChar(20), regd varChar(15), Status varChar(10), UProduct Varchar(25), MacRegId Varchar(25)) "+;
	*!*			" End "
	*!*		nretval = sqlconobj.dataconn("EXE","vUdyog",mSqlStr,"_sysobjects","nHandle")
	*!*		If nretval<0
	*!*			Return .F.
	*!*		ENDIF
	*!*		mRet=sqlconobj.sqlconnclose("nhandle")		&& Added By Sachin N. S. on 08/07/2010 for TKT-1473
	*!*		If mRet< 0
	*!*			Return .F.
	*!*		Endif

	*!*	*********** Company Check ************* End
	*!*		If (reg_value # 'DONE' And _regform = .T.) and (lAboutUs=.F.)	&& Changed By Sachin N. S. on 04/01/2009
	*!*			Do Form frmRegistration1
	*!*			Read Events
	*!*		Endif
	*!*		If lAboutUs=.T.				&& Added By Sachin N. S. on 09/07/2009
	*!*			Do Form frmRegistration2
	*!*		Endif
	*!*	Endif

	*!*	If File(vufile) And reg_value # 'DONE'
	*!*		r_instdate1 = r_instdate		&& Commented By SAchin N. S. on 15/09/2009

	*!*		nHandle = 0
	*!*		mSqlStr = " select getDate() as CurSysDate "
	*!*		nretval = sqlconobj.dataconn("EXE","vUdyog",mSqlStr,"_CurSysDate","nHandle")
	*!*		If nretval<0
	*!*			Return .F.
	*!*		Endif
	*!*		dCurDate = enc(Alltrim(Dtoc(Ttod(_CurSysDate.CurSysDate))))

	*!*		mSqlStr = "	update vudyog..co_mast set coFormdt=ISNULL(coFormdt,'') "+;
	*!*			" If Not Exists(select top 1 [coformdt] from vudyog..co_mast where coformdt<>'') "+;
	*!*			" Begin "+;
	*!*			"    update vudyog..co_mast set coFormDt = '"+dCurDate+"' where coformdt='' "+;
	*!*			" End "+;
	*!*			" select top 1 [coformdt] from vudyog..co_mast where coformdt<>'' "
	*!*		nretval = sqlconobj.dataconn("EXE","vudyog",mSqlStr,"_coformdt","nHandle")
	*!*		If nretval<0
	*!*			Return .F.
	*!*		Endif
	*!*		mRet=sqlconobj.sqlconnclose("nhandle")		&& Added By Sachin N. S. on 08/07/2010 for TKT-1473
	*!*		If mRet< 0
	*!*			Return .F.
	*!*		Endif
	*!*		r_instdate1 = IIF(TYPE("_coFormDt.coFormDt")="T",Dtoc(Ttod(_coFormDt.coFormDt)),dec(ALLTRIM(_coFormDt.coFormDt)))
	*!*		Do Unreg_data
	*!*
	*!*	Else
	*!*		nHandle = 0
	*!*		mSqlStr = " if Not Exists(select name from syscolumns where id = object_id('vudyog..co_mast') and name = 'coFormdt') "+;
	*!*			" Begin "+;
	*!*			" 	 Alter table vudyog..Co_Mast Add coFormdt varchar(25) "+;
	*!*			" End "
	*!*		nretval = sqlconobj.dataconn("EXE","vudyog",mSqlStr,"","nHandle")
	*!*		If nretval<0
	*!*			Return .F.
	*!*		Endif

	*!*	*!*		nHandle = 0
	*!*		mSqlStr = " select getDate() as CurSysDate "
	*!*		nretval = sqlconobj.dataconn("EXE","vUdyog",mSqlStr,"_CurSysDate","nHandle")
	*!*		If nretval<0
	*!*			Return .F.
	*!*		Endif
	*!*		dCurDate = enc(Alltrim(Dtoc(Ttod(_CurSysDate.CurSysDate))))
	*!*	*!*		nHandle = 0
	*!*		mSqlStr = "	 update vudyog..co_mast set coFormdt=ISNULL(coFormdt,'') "+;
	*!*			" If Not Exists(select top 1 [coformdt] from vudyog..co_mast where coformdt<>'') "+;
	*!*			" Begin "+;
	*!*			"    update vudyog..co_mast set coFormDt = '"+dCurDate+"' where coformdt='' "+;
	*!*			" End "+;
	*!*			" select top 1 [coformdt] from vudyog..co_mast where coformdt<>'' "
	*!*		nretval = sqlconobj.dataconn("EXE","vudyog",mSqlStr,"_coformdt","nHandle")
	*!*		If nretval<0
	*!*			Return .F.
	*!*		Endif
	*!*	*!*		r_instdate1 = Dtoc(Ttod(_coFormDt.coFormDt))
	*!*		mRet=sqlconobj.sqlconnclose("nhandle")		&& Added By Sachin N. S. on 08/07/2010 for TKT-1473
	*!*		If mRet< 0
	*!*			Return .F.
	*!*		Endif
	*!*		r_instdate1 = IIF(TYPE("_coFormDt.coFormDt")="T",Dtoc(Ttod(_coFormDt.coFormDt)),dec(ALLTRIM(_coFormDt.coFormDt)))
	*!*
	*!*	Endif



	*!*	Proc co_regupdt
	*!*		Select co_mast
	*!*		Go Top
	*!*		mcompname = co_mast.Co_Name

	*!*		Select User
	*!*		Loca
	*!*		Do While !Eof()

	*!*			ins = ''
	*!*			out = User.company
	*!*			nm1 = Padl(Alltr(User.User),Len(User.company),Alltr(User.User))
	*!*			chk = 0
	*!*			For j = 1 To Len(out)
	*!*				N = Asc(Substr(out,j,1)) - Asc(Substr(nm1,j,1))
	*!*				If N<=0
	*!*					chk = 1
	*!*					Loop
	*!*				Else
	*!*					ins = ins+Chr(Asc(Substr(out,j,1)) - Asc(Substr(nm1,j,1)))
	*!*				Endif
	*!*			Endfor
	*!*			ins = Strtran(ins,Padr(mcompname,Len(mcompname),' '),Padr(Allt(r_compn),Len(mcompname),' '))
	*!*			nm=Alltr(User.User)
	*!*			out=' '
	*!*			If !Empty(ins)
	*!*				nm1=Padl(nm,Len(ins),nm)
	*!*				For i = 1 To Len(ins)
	*!*					out = out+ Chr(Asc(Substr(ins,i,1)) + Asc(Substr(nm1,i,1)))
	*!*				Endfor
	*!*			Endif
	*!*			out=Alltr(out)
	*!*			Repl company With out In User

	*!*			Select User
	*!*			If !Eof()
	*!*				Skip
	*!*			Endif
	*!*		Enddo
	*!*		Select co_mast
	*!*		Repl Co_Name With r_compn In co_mast

	*!*	Proc Unreg_data
	*!*		r_compn     =   'UDYOG SOFTWARE INDIA LTD.'
	*!*		r_comp		=	'UN - REGISTERED'					&& 'VIEWER'
	*!*		r_user		=	''
	*!*		r_add1		=	'203, Jhalawar, E.S.Patanwala Estate,'
	*!*		r_add2		=	'L.B.S. Marg, Opp. Shreyas Cinema,'
	*!*		r_add3		=	'Ghatkopar (West),'
	*!*		r_city		=	'Mumbai.'
	*!*		r_state		=	'MAHARASHTRA'
	*!*		r_location	=	'MUMBAI'
	*!*		r_phone     =   ''
	*!*		r_servcent	=	'UDYOG SOFTWARE INDIA LTD.'
	*!*		r_instdate	=	''
	*!*		xvalue		=   ''
	*!*		r_coof		=	1
	*!*		r_noof		=	1
	*!*		r_idno		=	''
	*!*		r_pid		=   ''
	*!*		r_servcont	=	''
	*!*		r_servadd1	=	'203, Jhalawar, E.S.Patanwala Estate,'
	*!*		r_servadd2	=	'L.B.S. Marg, Opp. Shreyas Cinema,'
	*!*		r_servadd3	=	'Ghatkopar (West),'
	*!*		r_servcity	=	'Mumbai.'
	*!*		r_servzip	=	'MAHARASHTRA'
	*!*		r_servphone	=	'022 - 6799 3535'
	*!*		r_servemail	=	'support@udyogsoftware.com'
	*!*		reg_value	=	'NOT DONE'				&& 'VIEWER'
	*!*		reg_prods   = ''


	*!*	Proc enc
	*!*		Para mcheck
	*!*		D=1
	*!*		F=Len(mcheck)
	*!*		Repl=""
	*!*		rep=0
	*!*		Do Whil F > 0
	*!*			R=Subs(mcheck,D,1)
	*!*			Change = Asc(R)+rep
	*!*			If Change>255
	*!*				Wait Wind Str(Change)
	*!*			Endi
	*!*			two = Chr(Change)
	*!*			Repl=Repl+two
	*!*			D=D+01
	*!*			rep=rep+1
	*!*			F=F-1
	*!*	Endd
	*!*	Retu Repl

	*!*	Proc dec
	*!*		Para mcheck
	*!*		D=1
	*!*		F=Len(mcheck)
	*!*		Repl=""
	*!*		rep=0
	*!*		Do Whil F > 0
	*!*			R=Subs(mcheck,D,1)
	*!*			Change = Asc(R)-rep
	*!*			If Change>0
	*!*				two = Chr(Change)
	*!*			Endi
	*!*			Repl=Repl+two
	*!*			D=D+01
	*!*			F=F-1
	*!*			rep=rep+1
	*!*	Endd
	*!*	Retu Repl


	*!*	**************** vurInfo **************** End
	&&vasant16/11/2010	Changes done for VU 10 (Standard/Professional/Enterprise)
