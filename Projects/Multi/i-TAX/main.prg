Lparameters lcmenufind

Set Safety Off
Set Multilocks On
Set Deleted On
Set Century On
Set Date To british
Set Procedure To vu_udfs Additive
Set Procedure To sqlconnection Additive
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

Application.Visible = .F.
_vfp.Visible        = .F.
_Screen.Visible     = .F.

*!*	*!*	IF DATE() <> CTOD('30/09/08')
*!*	*!*		MESSAGEBOX("Demo version...",0,[UDYOG i-TAX])
*!*	*!*		QUIT
*!*	*!*		RETURN .F.
*!*	*!*	ENDIF

Declare Integer GetPrivateProfileString In Win32API As GetPrivStr ;
	STRING cSection, String cKey, String cDefault, String @cBuffer, ;
	INTEGER nBufferSize, String cINIFile
Declare Integer WritePrivateProfileString In Win32API As WritePrivStr ;
	STRING cSection, String cKey, String cValue, String cINIFile
Declare Integer GetProfileString In Win32API As GetProStr ;
	STRING cSection, String cKey, String cDefault, ;
	STRING @cBuffer, Integer nBufferSize
Declare Integer Beep In kernel32 Integer pn_Freq,Integer pn_Duration

Declare Integer GetCurrentProcessId In kernel32  && get Application Process ID

Public mvu_Checkint,mvu_Splittbl,mvu_MenuType 				&& DTS Releated Public Variables

Public apath,mvu_backend,mvu_server,mvu_user,mvu_pass,chqcon,musername,ousername,ocompany_name,ocompany_year,vchkprod,musername,icopath,msgsvr,killapp,ProcessID,;
	varyear,namecomp,vfromwherelogin,loginsuccess,nameuser,co_yr,company,coadditional,passreturn,forceexit,yvariable,;
	tbrdesktop,statdesktop,cmenuname,exitclick,exitonce,rptfilename,vumess,treedesktop,addbutton1,editbutton1,printbutton1,deletebutton1,ctrlkey,xapps,;
	applicationshutoff,appprocid,runinstanceapp,AppSessionId,_StuffObj,nTrypassword,storeusername,;
	OneKey_Server,OneKey_user,OneKey_Pass,mvu_MenuType,_Defaultdb,mvu_Auto_object,mvu_User_Roles,;
	iTaxAppPath,iTaxDbPath,mudProdCode

Public mvu_Backimg,mvu_Position			&& Wallpaper Related

appprocid = GetCurrentProcessId()
AppSessionId = 0

Public vu_s1,vu_p1,vu_sr1,vu_pr1,vu_so1,vu_po1,vu_ar1,vu_dc1,vu_cp1,vu_bp1,vu_jv1,vu_ip1,;
	vu_op1,vu_dn1,vu_cn1,vu_pc1,vu_ir1,vu_ii1,vu_es1,vu_b1,vu_ss1,vu_sq1,vu_br1,vu_cr1,;
	vu_ep1,vu_pl1,vu_sp1,vu_pi1,vu_li1,vu_lr1,vu_i1,vu_r1,vu_gi1,vu_gr1,vu_hi1,vu_hr1,;
	vu_bi1,vu_rp1,vu_fp1,vu_dp1,vu_rr1,vu_fr1,vu_dr1,vu_wi1,vu_wo1,vu_gt1,vu_st1,vu_bo1,;
	vu_ai1,vu_usercnt

Public vu_si1,vu_sd1,vu_sc1,vu_cd1,vu_cc1,vu_ed1,vu_sb1,vu_vi1,vu_ct1,vu_tr1,vu_eq1

Store .F. To vu_si1,vu_sd1,vu_sc1,vu_cd1,vu_cc1,vu_ed1,vu_sb1,vu_vi1,vu_ct1,vu_tr1,vu_eq1

Store .F. To vu_s1,vu_p1,vu_sr1,vu_pr1,vu_so1,vu_po1,vu_ar1,vu_dc1,vu_cp1,vu_bp1,vu_jv1,vu_ip1,;
	vu_op1,vu_dn1,vu_cn1,vu_pc1,vu_ir1,vu_ii1,vu_es1,vu_b1,vu_ss1,vu_sq1,vu_br1,vu_cr1,;
	vu_ep1,vu_pl1,vu_sp1,vu_pi1,vu_li1,vu_lr1,vu_i1,vu_r1,vu_gi1,vu_gr1,vu_hi1,vu_hr1,;
	vu_bi1,vu_rp1,vu_fp1,vu_dp1,vu_rr1,vu_fr1,vu_dr1,vu_wi1,vu_wo1,vu_gt1,vu_st1,vu_bo1,;
	vu_ai1

mvu_MenuType = Iif(Type('lcMenufind') = 'L',[M],lcmenufind)		&& Menu Type
mudProdCode = 'iTax'
*mudProdCode = 'USqaure'
apath = Allt(Fullpath(Curd()))
icopath= apath + "bmp\ueicon.ico"
SET STEP ON 
Store '' To varyear,namecomp,nameuser
Store '' To ocompany_name,ousername,killapp,ProcessID
chqcon      = 0
mvu_backend = []
mvu_server  = []
mvu_user    = []
mvu_pass    = []
passreturn  = 1
vumess = 'UDYOG i-TAX'

Local iniFilePath
iniFilePath = apath+"visudyog.ini"
If !File(iniFilePath)
	Messagebox("Configuration File Not found",16,vumess)
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
mvu_two     = GetPrivStr([1KeyServer],"Name", "", @mvu_one, Len(mvu_one), iniFilePath)
OneKey_Server = Left(mvu_one,mvu_two)
mvu_two     = GetPrivStr([1KeyServer],onencrypt(enc("User")), "", @mvu_one, Len(mvu_one), iniFilePath)
OneKey_user = Left(mvu_one,mvu_two)
mvu_two     = GetPrivStr([1KeyServer],onencrypt(enc("Pass")), "", @mvu_one, Len(mvu_one), iniFilePath)
OneKey_Pass = Left(mvu_one,mvu_two)
mvu_two		= GetPrivStr([Settings],"Title", "", @mvu_one, Len(mvu_one), iniFilePath)
lcvumess  	= Left(mvu_one,mvu_two)
vumess      = Iif(Empty(lcvumess),vumess,lcvumess)
mvu_two		= GetPrivStr([Settings],"XFile", "", @mvu_one, Len(mvu_one), iniFilePath)
xapps	  	= Left(mvu_one,mvu_two)
mvu_two = GetPrivStr([Settings],"iTaxAppPath", "", @mvu_one, Len(mvu_one), iniFilePath)
iTaxAppPath = Iif(Empty(Left(mvu_one,mvu_two)),apath,Left(mvu_one,mvu_two))
iTaxAppPath = Iif(Right(iTaxAppPath,1)='\',iTaxAppPath,iTaxAppPath+"\")
mvu_two = GetPrivStr([Settings],"iTaxDbPath", "", @mvu_one, Len(mvu_one), iniFilePath)
iTaxDbPath = Iif(Empty(Left(mvu_one,mvu_two)),apath+"Database\",Left(mvu_one,mvu_two))
iTaxDbPath = Iif(Right(iTaxDbPath,1)='\',iTaxDbPath,iTaxDbPath+"\")
mvu_Splittbl = ""
mvu_Checkint = "NO"

mvu_two		= GetPrivStr([Settings],"Backimage", "", @mvu_one, Len(mvu_one), iniFilePath)
lcBackimg  	= Left(mvu_one,mvu_two)
mvu_Backimg  = Iif(Empty(lcBackimg),'',lcBackimg)

mvu_two		 = GetPrivStr([Settings],"Position", "", @mvu_one, Len(mvu_one), iniFilePath)
lnPosition   = Left(mvu_one,mvu_two)
mvu_Position = Iif(Empty(lcBackimg),1,Val(lnPosition))

*!*	Backimage = Itaxback.Tif
*!*	Position = 1

_Screen.Caption  = vumess

If Empty(xapps)
	=Messagebox('In Configuration file Xfile Path cannot be empty',16,vumess)
	Return .F.
Else
	If !File(xapps)
		=Messagebox('In Configuration file Specify application file is not found',16,vumess)
		Return .F.
	Endif
Endif

On Shutdown applicationshutoff.appshutoff
On Key Label Shift+f12 applicationshutoff.appshutoff

Set Classlib To sqlconnection In &xapps Additive && Setting Class library

applicationshutoff = Createobject("Onshutdown")
_StuffObj = Createobject("_stuff")

If mvu_backend # "0"
	Do Case
		Case Empty(mvu_server)
			Messagebox("ERROR !!!, Data Server Not Defined",32,vumess)
			Retu .F.
		Case Empty(mvu_user)
			Messagebox("ERROR !!!, User Name Not Defined",32,vumess)
			Retu .F.
		Case Empty(OneKey_Server)
			Messagebox("ERROR !!!, 1Key Server Not Defined",32,vumess)
			Retu .F.
		Case Empty(OneKey_user)
			Messagebox("ERROR !!!, 1Key User Not Defined",32,vumess)
			Retu .F.
	Endcase
Endif

m_sysvarv1 	= enc(Dtoc(Date()))
m_sysvar19 	= enc(Str(-1))
If File('Vudyog.Mem')
	Rest From vudyog AddI
Endif
m_sysvar19  = enc(Str(Val(dec(m_sysvar19))+1))
Save To vudyog All Like m_*

r_compn     =   'UDYOG SOFTWARE INDIA LTD.'
r_comp		=	'UN - REGISTERED'
r_user		=	''
r_add1		=	'203, Jhalawar, E.S.Patanwala Estate,'
r_add2		=	'L.B.S. Marg, Opp. Shreyas Cinema,'
r_add3		=	'Ghatkopar (West),'
r_city		=	'Mumbai.'
r_state		=	'MAHARASHTRA'
r_location	=	'MUMBAI'
r_servcent	=	'UDYOG SOFTWARE INDIA LTD.'
r_instdate	=	''
xvalue		=   ''
r_noof		=	1
r_idno		=	''
r_pid		=   ''
r_servcont	=	''
r_servadd1	=	'203, Jhalawar, E.S.Patanwala Estate,'
r_servadd2	=	'L.B.S. Marg, Opp. Shreyas Cinema,'
r_servadd3	=	'Ghatkopar (West),'
r_servcity	=	'Mumbai.'
r_servzip	=	'MAHARASHTRA'
r_servphone	=	'022 - 5599 3535'
r_servemail	=	'support@udyogsoftware.com'
reg_value	=	'UN-REGISTERED'
reg_prods   = ''

If File('register.me')
	_flopen = Fopen('register.me',10)
	r_compn		=	Fread(_flopen,50)
	r_comp   	=	dec(dec(dec(Fread(_flopen,50))))
	r_user   	=	dec(dec(Fread(_flopen,50)))
	r_add1		=	dec(Fread(_flopen,50))
	r_add2		=	dec(Fread(_flopen,50))
	r_add3		=	dec(Fread(_flopen,50))
	r_city		=	dec(dec(Fread(_flopen,50)))
	r_state		=	dec(dec(Fread(_flopen,50)))
	r_location	=   dec(dec(Fread(_flopen,50)))
	r_servcent	=	dec(dec(dec(Fread(_flopen,50))))
	r_instdate	=	dec(dec(dec(Fread(_flopen,10))))
	xvalue		=	dec(Fread(_flopen,200))
	r_noof		= 	Val(dec(dec(dec(Fread(_flopen,50)))))
	r_idno		=	dec(dec(dec(Fread(_flopen,50))))
	r_pid		=	dec(dec(dec(Fread(_flopen,16))))
	r_servcont	=	dec(Fread(_flopen,50))
	r_servadd1	=	dec(Fread(_flopen,50))
	r_servadd2	=	dec(Fread(_flopen,50))
	r_servadd3	=	dec(Fread(_flopen,50))
	r_servcity	=	dec(Fread(_flopen,50))
	r_servzip	=	dec(Fread(_flopen,50))
	r_servphone	=	dec(Fread(_flopen,50))
	r_servemail	=	dec(Fread(_flopen,50))
	reg_value   = 'REGISTERED'
	_flopen = Fclose(_flopen)
Endi
xvalue = Uppe(xvalue)

If !Empty(xvalue)
	For i = 1 To Len(xvalue) 						&&step 6
		If "EXCISE MANUFACTURING" $ xvalue And "VISUAL" $ xvalue
			reg_prods = reg_prods + ',"vuexc"'
			xvalue = Strtran(xvalue,"EXCISE MANUFACTURING","")
		Endi
		If "EXPORT" $ xvalue And "VISUAL" $ xvalue
			reg_prods = reg_prods + ',"vuexp"'
			xvalue = Strtran(xvalue,"EXPORT","")
		Endi
		If "INVENTORY" $ xvalue And "VISUAL" $ xvalue
			reg_prods = reg_prods + ',"vuinv"'
			xvalue = Strtran(xvalue,"INVENTORY","")
		Endi
		If "ORDER PROCESSING" $ xvalue And "VISUAL" $ xvalue
			reg_prods = reg_prods + ',"vuord"'
			xvalue = Strtran(xvalue,"ORDER PROCESSING","")
		Endi
		If "PROFESSIONAL ACCOUNTING" $ xvalue And "VISUAL" $ xvalue
			reg_prods = reg_prods + ',"vupro"'
			xvalue = Strtran(xvalue,"PROFESSIONAL ACCOUNTING","")
		Endi
		If "SPECIAL BILLING" $ xvalue And "VISUAL" $ xvalue
			reg_prods = reg_prods + ',"vubil"'
			xvalue = Strtran(xvalue,"SPECIAL BILLING","")
		Endi
		If "ENTERPRISE ACCOUNTING" $ xvalue And "VISUAL" $ xvalue
			reg_prods = reg_prods + ',"vuent"'
			xvalue = Strtran(xvalue,"ENTERPRISE ACCOUNTING","")
		Endi
		If "EXCISE TRADING" $ xvalue And "VISUAL" $ xvalue
			reg_prods = reg_prods + ',"vutex"'
			xvalue = Strtran(xvalue,"EXCISE TRADING","")
		Endi
		If "INPUT SERVICE DISTRIBUTOR" $ xvalue And "VISUAL" $ xvalue
			reg_prods = reg_prods + ',"vuisd"'
			xvalue = Strtran(xvalue,"INPUT SERVICE DISTRIBUTOR","")
		Endi
		If "SERVICE TAX" $ xvalue And "VISUAL" $ xvalue
			reg_prods = reg_prods + ',"vuser"'
			xvalue = Strtran(xvalue,"SERVICE TAX","")
		Endi
	Endfor
Endif

Close Data All	&& closing database invoice which is automatically opened with lmain

On Error Do errorprocformain With Error()
Local sqlconobj
sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)
applicationshutoff = Createobject("Onshutdown")
_StuffObj = Createobject("_stuff")

On Error
traperror = Createobject("traperrormsg")
On Error traperror.errorroutine(Error(), Message( ), Message(1), Program( ), Lineno( ))

lcdisplayvalue=[ ]

Do Form spl  &&ash Rpp
Read Events

If _StuffObj.GetPropAPI()=0
	=_StuffObj.appmutex() && Check Session and Set Property (API)
Else
	=_StuffObj.SetPropAPI() && Set Property ( API )
Endif

Local lcbuffer,llshowintro
lcbuffer = " " + Chr(0)
llshowintro = .T.
If GetPrivStr("Defaults", "ShowIntroForm", "", @lcbuffer, Len(lcbuffer), Curdir() + "visudyog.ini") > 0
	llshowintro = (Val(lcbuffer) = 0)
Endif
Local lcontinue, lresume
lcontinue=.T.		&& its default must be .T.

_Defaultdb = "Vudyog"
nhandle =0
nhandle1 =0
msqlstr="SELECT [Name] FROM Master..sysdatabases WHERE [Name] = ?_Defaultdb"
nretval=sqlconobj.dataconn('EXE','master',msqlstr,"_co_mast","nHandle")
If nretval<0
	Return .F.
Endif

If Reccount('_co_mast') = 0
	mvu_MenuType = Iif(mvu_MenuType = [L],[M],mvu_MenuType)
	Do Form defafrmdb
Else
	If mvu_MenuType = [L]			&& DTS Main
		lcSqlstr = "SELECT Co_Name FROM Vudyog..Co_Mast WHERE Import = 1"
		nretval = sqlconobj.dataconn('EXE',_Defaultdb,lcSqlstr,"_Import_cur","nhandle1")
		If nretval<0
			Return .F.
		Endif
		nretval=sqlconobj.sqlconnclose("nHandle1") && Connection Close
		Select _Import_cur
		If Reccount("_Import_cur") = 0
			exitclick = .T.
			Messagebox("Please Create Company Before DTS...",64,vumess)
			Return
		Endif
		msqlstr = "SELECT [Name] FROM Master..sysdatabases WHERE [Name] = 'ITaxdts'"
		nretval = sqlconobj.dataconn('EXE','master',msqlstr,"_ITAX_DTS","nHandle")
		If nretval<0
			Return .F.
		Endif
		If Reccount("_ITAX_DTS") = 0
			Do Form defafrmdb
		Endif
	Endif
Endif

nretval=sqlconobj.sqlconnclose("nHandle") && Connection Close
If nretval<0
	Return .F.
Endif

Do While .T.
	nhandle=0
	IF !INLIST(mvu_MenuType,[C]) &&Rup
		nretval=sqlconobj.dataconn('EXE',_Defaultdb,'select * from co_mast',"_co_mast","nHandle")
	ELSE
		nretval=sqlconobj.dataconn('EXE',_Defaultdb,"select * from co_mast where com_type='M'","_co_mast","nHandle")
	ENDIF 
	If nretval<0
		Return .F.
	Endif

	nretval=sqlconobj.sqlconnclose("nHandle") && Connection Close
	If nretval<0
		Return .F.
	Endif
	SET STEP ON 
	If Eof('_co_mast') Or Bof('_co_mast')
		Do Form frmconndatabase
		Do Form frmudyogsdi With .T.
		Read Events
		Exit
	Else
		If Not Inlist(mvu_MenuType,[B]) And exitclick <> .T.
			Do Form frmpassword
			Read Events
		Endif
		If exitclick = .F.
			Do Form frmconndatabase
			If AppSessionId = 1 And mvu_MenuType <> [B]
				Public ObjLoggedUser,MsgWind
				Try
					ObjLoggedUser=Newobject('frmloginusers',"frmloginusers.Vcx")
					With ObjLoggedUser
						If Type("ObjLoggedUser") = "O"
							.Systray1.cleariconlist()
							.Systray1.iconfile = apath+'Bmp\ueicon.ico'
							.Systray1.addicontosystray()
							.Systray1.tiptext = vumess+" - Logon User Info"
						Endif
					Endwith
				Catch To errObj

				Endtry
				MsgWind=Newobject('frmMsgWindow',"frmMsgWindow.vcx")
			Endif

			Do Case
				Case mvu_MenuType = [B]			&& backup
					pbar.cleaprogressbar()
					pbar = Null
					Release pbar
					mvu_Auto_object = Createobject("Empty")
					Set Path To (apath)
					Do uebackup.App With [B],[]
				Case mvu_MenuType = [R]			&& Restore
					pbar.cleaprogressbar()
					pbar = Null
					Release pbar
					Do uebackup.App With [R],[]
				Case mvu_MenuType = [C]			&&Rup Multi Company Creation
					Do Form frmudyogsdi With .F.
				Case mvu_MenuType = [L]			&& DTS Main
					pbar.cleaprogressbar()
					pbar = Null
					Release pbar
					Do DTS.App
				Case mvu_MenuType = [D]
					Do Form frmudyogsdi With .F.
				Case mvu_MenuType = [I]			&& Integration Utility
					pbar.cleaprogressbar()
					pbar = Null
					Release pbar
					Do Integration.App
				Case mvu_MenuType = [M]			&& i-Tax MDI Window
					Do Form frmudyogsdi With .F.
				Otherwise
					=Messagebox("Please Pass Valid Parameter...",64,vumess)
					exitclick = .T.
			Endcase
			If exitclick = .T.
				Exit
			Else
				Read Events
			Endif
			If exitclick <> .T.
				Loop
			Endif
		Else
			Exit
		Endif
	Endif
Enddo

On Shutdown
Close All
Clear All
Clear Dlls
Quit
Return

Procedure errorprocformain
	Lparameters lerror
	If lerror = 1
		=Messagebox("Application Name has been changed!! Application Name should be Udyog i-Tax.Exe",64,vumess)
		Quit
	Endif
	Return
