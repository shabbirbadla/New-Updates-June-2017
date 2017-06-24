Define Class clsConnect As Custom OlePublic

	appname=""
	retValue=""
	aPath=""
	Procedure InitProc(appPath As String,appFile As String) As VOID
	This.appname=Alltrim(appFile)+".EXE"

	Set Default To &appPath
&&Added by Shrikant S. on 01/08/2014		&& Start		
	lcPath=&appPath+"\"+This.appname
	Set Classlib To UdGeneral.vcx In (lcPath) Additive
	Set Classlib To "SqlConnection.vcx" In (lcPath) Additive
	Set Procedure To vu_udfs.prg In (lcPath) Additive
&&Added by Shrikant S. on 01/08/2014		&& End
	
&&Commented by Shrikant S. on 01/08/2014		&& Start	
*!*		Set Classlib To UdGeneral.vcx In (This.appname) Additive
*!*		Set Classlib To "SqlConnection.vcx" In (This.appname) Additive
*!*		Set Procedure To vu_udfs.prg In (This.appname) Additive
&&Commented by Shrikant S. on 01/08/2014		&& End

	Endproc


	Procedure RetAppEnc(lcpara1 As String) As String

*!*		This.retValue=NewENCRY(enc(lcpara1),'Ud*yog+1993')
	This.retValue=""
	vumess=This.appname
	GlobalObj= Createobject("UdGeneral")
	If Type('GlobalObj')='O'
		This.retValue=(GlobalObj.getpropertyval("udprodcode"))
		GlobalObj=Null
	Endif
	Return (This.retValue)
	Endproc

	Procedure RetModuleDec(lcpara1 As String) As String
	This.retValue=NewDecry(lcpara1,'Udyog!Module!Mast')
	Return (This.retValue)
	Endproc

	Procedure RetFeatureDec(lcpara As String) As String
	This.retValue=NewDecry(lcpara,'Udencyogprod')
	Return (This.retValue)
	Endproc

	Procedure RetVersion(lcpara As String) As String
	This.retValue=""
	vumess=This.appname
	GlobalObj= Createobject("UdGeneral")
	If Type('GlobalObj')='O'
		This.retValue=Dec(GlobalObj.getPropertyval(lcpara))
		GlobalObj=Null
	Endif
	Return (This.retValue)
	Endproc

	Procedure RetShortCode(lcpara As String) As String
	This.retValue=""
	vumess=This.appname
	GlobalObj= Createobject("UdGeneral")
	If Type('GlobalObj')='O'
		This.retValue=Dec(NewDecry(GlobalObj.getPropertyval(lcpara),'Udyog_*Prod_Cd'))
		GlobalObj=Null
	Endif
	Return (This.retValue)
	Endproc

	Procedure RetProduct() As String
	This.retValue=""
	vumess=This.appname
	GlobalObj= Createobject("UdGeneral")

	_arrcode = GlobalObj.getPropertyval("MainProduct(1,3)")
	If !Empty(_arrcode)
		This.retValue = Dec(_arrcode)+","
		GlobalObj=Null
	Endif
	Return This.retValue
	Endproc

	Procedure RetEncValue(encString As String,keyValue As String) As String
	Sys(2335,1)
	This.retValue=NewENCRY(encString,keyValue)
*	This.retValue=NewENCRY(encString,'Udencyogprod')
	Return (This.retValue)
	Endproc

	Procedure ReadRegiValue(aPath As String) As String
	This.retValue=""
	vumess=This.appname
	GlobalObj= Createobject("UdGeneral")

	If Type('ueReadRegMe')!='O'
		ueReadRegMe = Createobject("ueReadRegisterMe")
	Endif
	If Type('ueReadRegMe') = 'O'
		_UnqVal = GlobalObj.getPropertyval('UnqVal')
		_UnqVal = Substr(Dec(_UnqVal),2,8)
		ueReadRegMe._ueReadRegisterMe(aPath,_UnqVal)

		This.retValue=Alltrim(ueReadRegMe.r_comp)
		This.retValue=This.retValue+"^"+Alltrim(ueReadRegMe.r_add1)
		This.retValue=This.retValue+"^"+Alltrim(ueReadRegMe.r_add2)
		This.retValue=This.retValue+"^"+Alltrim(ueReadRegMe.r_add3)
		This.retValue=This.retValue+"^"+Alltrim(ueReadRegMe.r_location)
		This.retValue=This.retValue+"^"+Alltrim(ueReadRegMe.r_zip)
		This.retValue=This.retValue+"^"+Alltrim(ueReadRegMe.r_email)
		This.retValue=This.retValue+"^"+Alltrim(ueReadRegMe.r_svccode)
		This.retValue=This.retValue+"^"+Alltrim(ueReadRegMe.r_dbsrvIp)
		This.retValue=This.retValue+"^"+Alltrim(ueReadRegMe.r_dbsrvname)
		This.retValue=This.retValue+"^"+Alltrim(ueReadRegMe.r_ApsrvIp)
		This.retValue=This.retValue+"^"+Alltrim(ueReadRegMe.r_Apsrvname)
		This.retValue=This.retValue+"^"+Alltrim(ueReadRegMe.r_MACId)
		This.retValue=This.retValue+"^"+Alltrim(Str(ueReadRegMe.r_coof))
		This.retValue=This.retValue+"^"+Alltrim(Str(ueReadRegMe.r_noof))
		This.retValue=This.retValue+"^"+Alltrim(ueReadRegMe.r_srvtype)
		This.retValue=This.retValue+"^"+Alltrim(ueReadRegMe.xvalue)
	Endif
	ueReadRegMe=Null
	GlobalObj=Null
	Return This.retValue
	Endproc

	Procedure RetIniTable(appPath As String) As String

	vumess=This.appname
	GlobalObj= Createobject("UdGeneral")
	This.retValue=""
	aPath=appPath+Iif(Right(appPath,1)=="\","","\")

	Do ReadInfFile

	Select custinfo_vw
*	Cursortoxml("custinfo_vw","xmlString",1)
	Cursortoxml("custinfo_vw","xmlString",1,0,0,"1")
*	xmlString=Strtran(xmlString,[encoding="Windows-1252" standalone="yes"],"")
*	Strtofile(xmlString,"e:\abcd.txt",0)
	This.retValue=xmlString
	GlobalObj=Null
	Use In custinfo_vw
	Return This.retValue
	Endproc


	Procedure GetCompany (cValue As String) As String
	This.retValue=""
	nm="ADMINISTRATOR"
	If !Empty(cValue )
		nm1=Padl(nm,Len(cValue ),nm)
		For i = 1 To Len(cValue )
			This.retValue = This.retValue+ Chr(Asc(Substr(cValue ,i,1)) + Asc(Substr(nm1,i,1)))
		Endfor
	Endif
	Return This.retValue
	Endproc

	Procedure GetAppName As String
	Return (This.appname)
	Endproc

*!*		Procedure ReadInfFile
*!*		Set Step On
*!*		Select 0
*!*		Create Cursor custinfo_vw (clientnm Varchar(100),macid Varchar(50),prodnm Varchar(50),prodcd Varchar(250),zip Varchar(100),featureid Memo,vatstates Memo, AddProdCd Varchar(250))

*!*		Select 0
*!*		Create Cursor custinfofile_vw (clientdet m,dec_cldet m)

*!*		custinfosrno = 1
*!*		Do While custinfosrno <= 2
*!*			custinfofilenm = ''
*!*			Do Case
*!*			Case custinfosrno = 1
*!*				custinfofilenm = aPath
*!*			Case custinfosrno = 2
*!*				custinfofilenm = aPath+'Database\'
*!*			Endcase
*!*			vufile	= Sys(2000,custinfofilenm+"*info.inf")
*!*			nFile	= Adir(arrFile,custinfofilenm+vufile)
*!*			_lens = 1
*!*			For _lens = 1 To nFile
*!*				Select custinfofile_vw
*!*				Append Blank In custinfofile_vw
*!*				Replace clientdet With Filetostr(custinfofilenm+arrFile(_lens,1)) In custinfofile_vw
*!*			Endfor
*!*			custinfosrno = custinfosrno + 1
*!*		Enddo

*!*		Select custinfofile_vw
*!*		Scan

*!*			nFile	 = Len(custinfofile_vw.clientdet)
*!*			_partynm = Alltrim(Substr(custinfofile_vw.clientdet,1,50))
*!*			Replace dec_cldet With _partynm In custinfofile_vw

*!*			_lens = 1
*!*			For _lens = 51 To nFile Step 50
*!*				_mname = NewDecry(Substr(custinfofile_vw.clientdet,_lens,50),_partynm)
*!*				Replace dec_cldet With custinfofile_vw.dec_cldet +  _mname In custinfofile_vw
*!*			Endfor

*!*			_partyz = custinfofile_vw.dec_cldet
*!*			Do While .T.
*!*				_party1 = At('<<~0s>>',_partyz)
*!*				_party2 = At('<<e0~>>',_partyz)
*!*				If _party1 > 0 And _party2 > 0
*!*					_partya = Substr(_partyz,_party1,_party2-_party1)
*!*					_macid  = ''
*!*					Select custinfo_vw
*!*					Append Blank In custinfo_vw
*!*					Do While .T.
*!*						_party3 = At('<<~1s>>',_partya)
*!*						_party4 = At('<<e1~>>',_partya)
*!*						If _party3 > 0 And _party4 > 0
*!*							_partyd = Substr(_partya,_party3,_party4-_party3)
*!*							Do Case
*!*							Case Substr(_partyd,8,2) = 'CN'
*!*								Replace clientnm With Dec(NewDecry(Substr(_partyd,11),_partynm)) In custinfo_vw
*!*							Case Substr(_partyd,8,2) = 'MI'
*!*								Replace macid With Dec(NewDecry(Substr(_partyd,11),_partynm)) In custinfo_vw
*!*								_macid = Alltrim(custinfo_vw.macid)
*!*							Case Substr(_partyd,8,2) = 'PV'
*!*								Replace prodnm With Dec(NewDecry(Substr(_partyd,11),_macid)) In custinfo_vw
*!*							Case Substr(_partyd,8,2) = 'PC'
*!*	*!*							REPLACE prodcd WITH NewDECRY(SUBSTR(_partyd,11),_macid) IN custinfo_vw
*!*	****** Changed By Sachin N. S. on 21/04/2011 for TKT-6920 and TKT-2386 ****** Start
*!*								_cPrdCode1=''
*!*								_cPrdCode2=''
*!*								_cPrdCode3=''
*!*								_cPrdCode = Alltrim(NewDecry(Substr(_partyd,11),_macid))+','
*!*								Do While .T.
*!*									_cPrdCode1 = Left(_cPrdCode,At(',',_cPrdCode)-1)

*!*									If Inlist(_cPrdCode1,"vuent" ,"vupro","vuexc","vuexp","vuinv","vuord","vubil","vutex","vuser","vuisd","vumcu","vutds")
*!*										_cPrdCode2 = _cPrdCode2 + Iif(Empty(_cPrdCode2),"",",") + _cPrdCode1
*!*									Else
*!*										_cPrdCode3 = _cPrdCode3 + Iif(Empty(_cPrdCode3),"",",") + _cPrdCode1
*!*									Endif
*!*									_cPrdCode = Strtran(_cPrdCode,_cPrdCode1+',','')

*!*									If Empty(_cPrdCode)
*!*										Exit
*!*									Endif
*!*								Enddo
*!*								Replace prodcd With _cPrdCode2, AddProdCd With _cPrdCode3 In custinfo_vw

*!*	****** Changed By Sachin N. S. on 21/04/2011 for TKT-6920 and TKT-2386 ****** End
*!*							Case Substr(_partyd,8,2) = 'ZP'
*!*								Replace zip With NewDecry(Substr(_partyd,11),_macid) In custinfo_vw
*!*							Case Substr(_partyd,8,2) = 'FI'
*!*								Replace featureid With NewDecry(Substr(_partyd,11),_partynm+_macid) In custinfo_vw
*!*							Case Substr(_partyd,8,2) = 'VS'
*!*								Replace vatstates With NewDecry(Substr(_partyd,11),_macid) In custinfo_vw
*!*							Endcase

*!*							_partya = Substr(_partya,_party4+7)
*!*						Else
*!*							_partya = ''
*!*						Endif

*!*						If Empty(_partya)
*!*							Exit
*!*						Endif
*!*					Enddo
*!*					_partyz = Substr(_partyz,_party2+7)
*!*				Else
*!*					_partyz = ''
*!*				Endif

*!*				If Empty(_partyz)
*!*					Exit
*!*				Endif
*!*			Enddo

*!*			Select custinfofile_vw
*!*		Endscan

*!*		If Used('custinfofile_vw')
*!*			Use In custinfofile_vw
*!*		Endif
*!*		Select custinfo_vw
*!*		Endproc

Enddefine
