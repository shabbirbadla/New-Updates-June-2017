DEFINE CLASS ClNetToFx As Session OLEPUBLIC
         			
	PROCEDURE PrNetToFx(_mCoDbName as String,_mCoSta_dt as String,_mCoEnd_dt as String,_mLogUserName as String,_mAppDefaPath as String,_mAppPath as String,_mIcoPath as String,_mProdCode as String,_mProgName as String)
		_MainScrnDataSession = Set("Datasession")
		Set DataSession To _MainScrnDataSession 

		Application.Visible = .F.
		_vfp.Visible        = .F.
		_Screen.Visible     = .F.
		Set Safety Off
		Set Multilocks On
		Set Deleted On
		Set Century On
		Set Date To british
		Set Resource Off
		Set Talk Off
		Set Scoreboard Off
		Set Escape Off
		Set Exclusive Off
		Set Exact Off
		Set Clock Status
		Set Multilocks On
		Set Resource Off
		Set Help On

		Public apath,mvu_backend,mvu_server,mvu_user,mvu_pass,chqcon,musername,vchkprod,;
			icopath,company,coadditional,forceexit,tbrdesktop,statdesktop,exitclick,exitonce,vumess,treedesktop,;
			addbutton1,editbutton1,printbutton1,deletebutton1,xapps,mvu_MenuType,_Defaultdb,;
			GlobalObj,_CoDbName,_CoSta_dt,_CoEnd_dt,_CoProgName,UdNewTrigEnbl,oGlblPrdFeat,ueReadRegMe

		apath			= _mAppDefaPath
		mvu_backend		= 0
		mvu_server		= ''
		mvu_user		= ''
		mvu_pass		= ''
		chqcon			= 0
		musername		= _mLogUserName
		vchkprod		= _mProdCode
		icopath			= _mIcoPath
		forceexit		= .f.
		exitclick		= .f.
		exitonce		= .f.
		vumess			= ''
		addbutton1		= .f.
		editbutton1		= .f.
		printbutton1	= .f.
		deletebutton1	= .f.
		xapps			= ''
		mvu_MenuType	= ''
		_Defaultdb		= 'Vudyog'
		_CoDbName		= _mCoDbName
		*_CoSta_dt		= CTOT(_mCoSta_dt)
		*_CoEnd_dt		= CTOT(_mCoEnd_dt)
		IF TYPE('_mCoSta_dt') = 'C'
			_mCoSta_dt	= CTOT(_mCoSta_dt)
			_mCoEnd_dt	= CTOT(_mCoEnd_dt)
		Endif	
		_CoSta_dt		= _mCoSta_dt
		_CoEnd_dt		= _mCoEnd_dt
		_CoProgName		= _mProgName
		UdNewTrigEnbl	= .f.	

		Declare Integer GetPrivateProfileString In Win32API As GetPrivStr ;
			string cSection, String cKey, String cDefault, String @cBuffer, ;
			integer nBufferSize, String cINIFile
		Declare Integer WritePrivateProfileString In Win32API As WritePrivStr ;
			string cSection, String cKey, String cValue, String cINIFile
		Declare Integer GetProfileString In Win32API As GetProStr ;
			string cSection, String cKey, String cDefault, ;
			string @cBuffer, Integer nBufferSize
		Declare Integer Beep In kernel32 Integer pn_Freq,Integer pn_Duration

		Local iniFilePath
		iniFilePath = apath+"visudyog.ini"
		If !File(iniFilePath)
			Messagebox("Configuration file not found",16,vumess)
			Retu .F.
		Endif

		mvu_one     = Space(2000)
		mvu_two     = 0
		mvu_two	    = GetPrivStr([Settings],"Backend", "", @mvu_one, Len(mvu_one), iniFilePath)
		mvu_backend = Left(mvu_one,mvu_two)
		mvu_two     = GetPrivStr([DataServer],"Name", "", @mvu_one, Len(mvu_one), iniFilePath)
		mvu_server  = Left(mvu_one,mvu_two)
		mvu_two     = GetPrivStr([DataServer],onencrypt(enc("User")), "", @mvu_one, Len(mvu_one), iniFilePath)
		mvu_user    = Left(mvu_one,mvu_two)
		mvu_two     = GetPrivStr([DataServer],onencrypt(enc("Pass")), "", @mvu_one, Len(mvu_one), iniFilePath)
		mvu_pass    = Left(mvu_one,mvu_two)
		mvu_backend = Iif(Empty(mvu_backend),"0",mvu_backend)
		mvu_two		= GetPrivStr([Settings],"Title", "", @mvu_one, Len(mvu_one), iniFilePath)
		lcvumess  	= Left(mvu_one,mvu_two)
		vumess      = Iif(Empty(lcvumess),vumess,lcvumess)
		mvu_two		= GetPrivStr([Settings],"XFile", "", @mvu_one, Len(mvu_one), iniFilePath)
		xapps	  	= Left(mvu_one,mvu_two)
				
		SET DEFAULT TO &_mAppDefaPath
		SET PATH TO &_mAppPath
				SET PROCEDURE TO vu_udfs Additive 
		Set Classlib To sqlConnection In &xapps Additive 
		Set Classlib To vutool In &xapps Additive 
		Set Classlib To UdGeneral.vcx  In &xapps Additive 

		GlobalObj = Createobject("UdGeneral")
		If Type('GlobalObj') != 'O'
			Retu .F.
		ENDIF

		ueReadRegMe = Createobject("ueReadRegisterMe")
		If Type('ueReadRegMe') != 'O'
			=Messagebox("Registration details not found.",0+16,vuMess)
			Return .F.
		Endif
		_UnqVal = GlobalObj.getPropertyval('UnqVal')
		_UnqVal = Substr(Dec(_UnqVal),2,8)
		ueReadRegMe._ueReadRegisterMe(apath,_UnqVal)

		oGlblPrdFeat = Createobject("UdProductFeature",vchkprod)
		
		vumess = GlobalObj.getpropertyval("vumess")
		If mvu_backend # "0"
			Do Case
			Case Empty(mvu_server)
				Messagebox("ERROR !!!, Data server not defined",32,vumess)
				Retu .F.
			Case Empty(mvu_user)
				Messagebox("ERROR !!!, User name not defined",32,vumess)
				Retu .F.
			Endcase
		Endif

		DO FORM frmNetToFx.scx WITH _MainScrnDataSession 
		READ Events

	Endproc	
	
Enddefine