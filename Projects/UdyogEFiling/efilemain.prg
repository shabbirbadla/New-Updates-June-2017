LPARAMETERS _fromsoftware,_warefilt,_range
*LPARAMETERS _fromsoftware,_range

lnparacnt=PARAMETERS()
_Screen.Visible= .F.
_vfp.Visible = .F.

SET CENTURY on
SET DATE BRITISH
SET DELETED ON
SET EXCLUSIVE OFF
SET SAFETY off
SET MEMOWIDTH TO 1000

PUBLIC _RunFromSoftware,efilemainfrmstatus,vumess1,mProduct,mVersion,_LogUserName,mToolVersion,_mDirecteFiling,_mACESFiling,_mforeFiling
*_RunFromSoftware = _fromsoftware

&& Added by Shrikant S. on 14/01/2015 for bug-27523		&& Start
PUBLIC _warenmFilter,_fware,_tware

IF lnparacnt >2
	_warenmFilter=_warefilt
ELSE
	_range=_warefilt
	_warenmFilter=.f.
endif
&& Added by Shrikant S. on 14/01/2015 for bug-27523		&& End

_fware=""
_tware=""

mVersion = ''
=AGETFILEVERSION(_VersionArr,JUSTFNAME(SYS(16,0)))
IF TYPE('_VersionArr') = 'C'
	mVersion = _VersionArr(1,4)
Endif	
IF OCCURS('.',mVersion) = 2
	mVersion = mVersion + '.0'
Endif
mToolVersion = mVersion
mVersion = '1.0.1.0'	
vumess1		= 'Data Extraction Tool For '	
mProduct 	= 'VU9'
_mDirecteFiling = .f.
_mACESFiling = .f.
IF TYPE('_fromsoftware') = 'C'
	IF _fromsoftware = '<~1~>VU8<~1~>'
		PUBLIC apath,vumess,icoPath,reg_value,reg_prods,musername,mDefaultdb,Company,CoAddlInfo,CoAdditional,_tmpvar,;
			mvu_backend,mvu_server,mvu_user,mvu_pass,_mfirstlogin,GlobalObj,ntrypassword,storeusername	
	
		mProduct 	= 'VU8'
		APath 		= STRTRAN(STREXTRACT(_fromsoftware,'<~2~>','<~2~>'),'<~2A2~>',' ')
		mUserName 	= STRTRAN(STREXTRACT(_fromsoftware,'<~3~>','<~3~>'),'<~3A3~>',' ')
		_mcorecno 	= VAL(STREXTRACT(_fromsoftware,'<~4~>','<~4~>'))
		vumess 		= STRTRAN(STREXTRACT(_fromsoftware,'<~5~>','<~5~>'),'<~5A5~>',' ')
		_fromsoftware = .t.
		_mACESFiling  = .t.

		_mcomastname = ADDBS(APath)+ 'Co_Mast'
		IF !USED('Co_Mast')	
			SELECT 0
			USE &_mcomastname AGAIN SHARED ALIAS Co_Mast
		Endif	
		IF USED('Co_Mast') AND _mcorecno > 0
			SELECT Co_Mast
			GO _mcorecno
			Scatter Name company Memo
			If Val(company.vcolor)= 0  And Iscolor() =.T.
				company.vcolor =Str(15460070)
			Endif

			DECLARE integer WriteProfileString IN Win32API ;
				AS WriteWinINI string,string,string

			declare integer GetPrivateProfileString in Win32API as GetPrivStr ;
				string cSection, string cKey, string cDefault, string @cBuffer, ;
				integer nBufferSize, string cINIFile

			lcclass = apath + 'Class'
			lcimage =  apath + 'Image'
			lcreport = apath + 'Report'
			lcBmp = apath + 'Bmp'
			lcpath = Allt(company.dir_nm)
			lcdata = Alltrim(company.dir_nm)

			Set Path To &lcpath,&apath,&lcreport,&lcimage,&lcclass,&lcBmp
			Set Defa To '&lcpath'
			SET RESOURCE TO
		ELSE
			=MESSAGEBOX('Company not found',0,vumess)
			RETURN .f.
		Endif
	Endif	
ELSE
	IF mProduct = 'VU9'	AND _fromsoftware
		_mACESFiling = .t.
	Endif
Endif
_RunFromSoftware = _fromsoftware		

IF _RunFromSoftware
	_LogUserName = mUserName
	vumess1		 = vumess1 + vumess
	
	DECLARE integer WritePrivateProfileString IN Win32API ;
		AS WritePrivateINI string,string,string,string
			_mACESFiling  = .f.
	IF _mACESFiling  = .t.
		IF mProduct = 'VU8'
			DO FORM eFileAuth
			Read Events
		ELSE
			DO FORM eFileAuth_VU9
		Endif	
	ENDIF
	
	DO FORM eFileWizard
	IF mProduct = 'VU8'
		Read Events
		CLOSE ALL 	
		Quit
	Endif
Else
	PUBLIC apath,vumess,icoPath,reg_value,reg_prods,musername,mDefaultdb,Company,CoAddlInfo,CoAdditional,_tmpvar,;
		mvu_backend,mvu_server,mvu_user,mvu_pass,_mfirstlogin,GlobalObj,ntrypassword,storeusername	&&vasant050411

	_mfirstlogin = .t.	

	apath       =SYS(5)+CURDIR()
	_curdefadir  = apath
	_curdefapath = apath
	DO WHILE .t.
		ntrypassword  = 0
		storeusername = ''
		vumess		= ''
		icoPath		= ''
		musername	= ''
		reg_value	= ''
		reg_prods   = ''
		mDefaultDb   = ''
		mProduct     = ''

		SET path TO (_curdefapath)
		SET DEFAULT TO (_curdefadir)
		SET RESOURCE TO
		
		efilemainfrmstatus = .f.
		DO FORM eFileLogin
		Read Events
		_mfirstlogin = .f.
		IF efilemainfrmstatus = .f.
			*CLOSE TABLES &&vasant251010
			CLOSE ALL 	&&vasant251010
			EXIT
		Else	
			DO FORM eFileCompany
			Read Events
		Endif	
		IF efilemainfrmstatus = .t.
			DO FORM eFileWizard
			Read Events
		ENDIF
		*CLOSE TABLES &&vasant251010
		CLOSE ALL 	&&vasant251010
	ENDDO
Endif	

*******************
Procedure onencrypt
*****************
Lpara lcvariable
lcreturn = ""
For i=1 To Len(lcvariable)
	lcreturn=lcreturn+Chr(Asc(Substr(lcvariable,i,1))+Asc(Substr(lcvariable,i,1)))
Endfor
Return lcreturn

Procedure ondecrypt
*****************
Lpara lcvariable
lcreturn = ""
For i=1 To Len(lcvariable)
	lcreturn=lcreturn+Chr(Asc(Substr(lcvariable,i,1))/2)
Endfor
Return lcreturn


Proc enc
********
Para mcheck
d=1
F=Len(mcheck)
Repl=""
rep=0
Do Whil F > 0
	r=Subs(mcheck,d,1)
	Change = Asc(r)+rep
	If Change>255
		Wait Wind Str(Change)
	Endi
	two = Chr(Change)
	Repl=Repl+two
	d=d+01
	rep=rep+1
	F=F-1
Endd
Retu Repl

Proc dec
********
Para mcheck
d=1
F=Len(mcheck)
Repl=""
rep=0
Do Whil F > 0
	r=Subs(mcheck,d,1)
	Change = Asc(r)-rep
	If Change>0
		two = Chr(Change)
	Endi
	Repl=Repl+two
	d=d+01
	F=F-1
	rep=rep+1
Endd
Retu Repl
