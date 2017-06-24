lparameters lWhat,lcmenufind
set safety off
set multilocks on
set deleted on
set century on
set date to british
set procedure to vu_udfs additive
*!*	set procedure to sqlconn.prg additive
set resource off
set talk off
set scoreboard off
set escape off
set exclusive off
set exact off
set clock status
set multilocks on
set resource off
set help on
on shutdown do onshutdown in vu_udfs.prg


application.visible = .f.
_vfp.visible        = .f.
_screen.visible     = .f.
_screen.caption     = ' Udyog ERP - [ Beta Version ]'

declare integer GetPrivateProfileString in Win32API as GetPrivStr ;
	string cSection, string cKey, string cDefault, string @cBuffer, ;
	integer nBufferSize, string cINIFile
declare integer WritePrivateProfileString in Win32API as WritePrivStr ;
	string cSection, string cKey, string cValue, string cINIFile
declare integer GetProfileString in Win32API as GetProStr ;
	string cSection, string cKey, string cDefault, ;
	string @cBuffer, integer nBufferSize
declare integer Beep in kernel32 integer pn_Freq,integer pn_Duration

declare integer GetCurrentProcessId in kernel32  && get Application Process ID


public apath,mvu_backend,mvu_server,mvu_user,mvu_pass,chqcon,musername,ousername,ocompany_name,ocompany_year,vchkprod,musername,icopath,msgsvr,killapp,processid,;
	varyear,namecomp,vfromwherelogin,loginsuccess,nameuser,co_yr,company,coadditional,passreturn,forceexit,yvariable,;
	tbrdesktop,statdesktop,cmenuname,exitclick,exitonce,rptfilename,vumess,treedesktop,addbutton1,editbutton1,printbutton1,deletebutton1,ctrlkey,xapps

AppProcID = GetCurrentProcessId()


public vu_s1,vu_p1,vu_sr1,vu_pr1,vu_so1,vu_po1,vu_ar1,vu_dc1,vu_cp1,vu_bp1,vu_jv1,vu_ip1,;
	vu_op1,vu_dn1,vu_cn1,vu_pc1,vu_ir1,vu_ii1,vu_es1,vu_b1,vu_ss1,vu_sq1,vu_br1,vu_cr1,;
	vu_ep1,vu_pl1,vu_sp1,vu_pi1,vu_li1,vu_lr1,vu_i1,vu_r1,vu_gi1,vu_gr1,vu_hi1,vu_hr1,;
	vu_bi1,vu_rp1,vu_fp1,vu_dp1,vu_rr1,vu_fr1,vu_dr1,vu_wi1,vu_wo1,vu_gt1,vu_st1,vu_bo1,;
	vu_ai1,vu_usercnt

public vu_si1,vu_sd1,vu_sc1,vu_cd1,vu_cc1,vu_ed1,vu_sb1,vu_vi1,vu_ct1,vu_tr1,vu_eq1

store .f. to vu_si1,vu_sd1,vu_sc1,vu_cd1,vu_cc1,vu_ed1,vu_sb1,vu_vi1,vu_ct1,vu_tr1,vu_eq1

store .f. to vu_s1,vu_p1,vu_sr1,vu_pr1,vu_so1,vu_po1,vu_ar1,vu_dc1,vu_cp1,vu_bp1,vu_jv1,vu_ip1,;
	vu_op1,vu_dn1,vu_cn1,vu_pc1,vu_ir1,vu_ii1,vu_es1,vu_b1,vu_ss1,vu_sq1,vu_br1,vu_cr1,;
	vu_ep1,vu_pl1,vu_sp1,vu_pi1,vu_li1,vu_lr1,vu_i1,vu_r1,vu_gi1,vu_gr1,vu_hi1,vu_hr1,;
	vu_bi1,vu_rp1,vu_fp1,vu_dp1,vu_rr1,vu_fr1,vu_dr1,vu_wi1,vu_wo1,vu_gt1,vu_st1,vu_bo1,;
	vu_ai1

if type('lcMenufind') = 'L'
	lcmenufind = ''
endif

apath = allt(sys(5) + curd())
icopath= apath + "bmp\vuicon.ico"
vumess = ' Udyog ERP - [ Beta Version ]'
xapps  = 'UdyogErp.exe'
set classlib to sqlconnection in &xapps additive && Setting Class library

store '' to varyear,namecomp,nameuser
store '' to ocompany_name,ousername,killapp,processid
chqcon      = 0
mvu_backend = []
mvu_server  = []
mvu_user    = []
mvu_pass    = []
passreturn  = 1

if !file(apath+"visudyog.ini")
	messagebox("Configuration File Not found",32,vumess)
	retu .f.
endif

mvu_one     = space(2000)
mvu_two     = 0
mvu_two	    = getprivstr([Settings],"Backend", "", @mvu_one, len(mvu_one), apath + "visudyog.ini")
mvu_backend = left(mvu_one,mvu_two)
mvu_two     = getprivstr([Server],"Name", "", @mvu_one, len(mvu_one), apath + "visudyog.ini")
mvu_server  = left(mvu_one,mvu_two)
mvu_two     = getprivstr([Server],onencrypt(enc("User")), "", @mvu_one, len(mvu_one), apath + "visudyog.ini")
mvu_user    = left(mvu_one,mvu_two)
mvu_two     = getprivstr([Server],onencrypt(enc("Pass")), "", @mvu_one, len(mvu_one), apath + "visudyog.ini")
mvu_pass    = left(mvu_one,mvu_two)
mvu_backend = iif(empty(mvu_backend),"0",mvu_backend)

if mvu_backend # "0"
	if empty(mvu_server)
		messagebox("Server Name Not Defined",32,vumess)
		retu .f.
	endif
	if empty(mvu_user)
		messagebox("User Name Not Defined",32,vumess)
		retu .f.
	endif
endif

m_sysvarv1 	= enc(dtoc(date()))
m_sysvar19 	= enc(str(-1))
if file('Vudyog.Mem')
	rest from vudyog addi
endif
m_sysvar19  = enc(str(val(dec(m_sysvar19))+1))
save to vudyog all like m_*

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
if file('register.me')
	_flopen = fopen('register.me',10)
	r_compn		=	fread(_flopen,50)
	r_comp   	=	dec(dec(dec(fread(_flopen,50))))
	r_user   	=	dec(dec(fread(_flopen,50)))
	r_add1		=	dec(fread(_flopen,50))
	r_add2		=	dec(fread(_flopen,50))
	r_add3		=	dec(fread(_flopen,50))
	r_city		=	dec(dec(fread(_flopen,50)))
	r_state		=	dec(dec(fread(_flopen,50)))
	r_location	=   dec(dec(fread(_flopen,50)))
	r_servcent	=	dec(dec(dec(fread(_flopen,50))))
	r_instdate	=	dec(dec(dec(fread(_flopen,10))))
	xvalue		=	dec(fread(_flopen,200))
	r_noof		= 	val(dec(dec(dec(fread(_flopen,50)))))
	r_idno		=	dec(dec(dec(fread(_flopen,50))))
	r_pid		=	dec(dec(dec(fread(_flopen,16))))
	r_servcont	=	dec(fread(_flopen,50))
	r_servadd1	=	dec(fread(_flopen,50))
	r_servadd2	=	dec(fread(_flopen,50))
	r_servadd3	=	dec(fread(_flopen,50))
	r_servcity	=	dec(fread(_flopen,50))
	r_servzip	=	dec(fread(_flopen,50))
	r_servphone	=	dec(fread(_flopen,50))
	r_servemail	=	dec(fread(_flopen,50))
	reg_value   = 'REGISTERED'
	_flopen = fclose(_flopen)
endi
xvalue = uppe(xvalue)
if !empty(xvalue)
	for i = 1 to len(xvalue) 						&&step 6
		if "EXCISE MANUFACTURING" $ xvalue and "VISUAL" $ xvalue
			reg_prods = reg_prods + ',"vuexc"'
			xvalue = strtran(xvalue,"EXCISE MANUFACTURING","")
		endi
		if "EXPORT" $ xvalue and "VISUAL" $ xvalue
			reg_prods = reg_prods + ',"vuexp"'
			xvalue = strtran(xvalue,"EXPORT","")
		endi
		if "INVENTORY" $ xvalue and "VISUAL" $ xvalue
			reg_prods = reg_prods + ',"vuinv"'
			xvalue = strtran(xvalue,"INVENTORY","")
		endi
		if "ORDER PROCESSING" $ xvalue and "VISUAL" $ xvalue
			reg_prods = reg_prods + ',"vuord"'
			xvalue = strtran(xvalue,"ORDER PROCESSING","")
		endi
		if "PROFESSIONAL ACCOUNTING" $ xvalue and "VISUAL" $ xvalue
			reg_prods = reg_prods + ',"vupro"'
			xvalue = strtran(xvalue,"PROFESSIONAL ACCOUNTING","")
		endi
		if "SPECIAL BILLING" $ xvalue and "VISUAL" $ xvalue
			reg_prods = reg_prods + ',"vubil"'
			xvalue = strtran(xvalue,"SPECIAL BILLING","")
		endi
		if "ENTERPRISE ACCOUNTING" $ xvalue and "VISUAL" $ xvalue
			reg_prods = reg_prods + ',"vuent"'
			xvalue = strtran(xvalue,"ENTERPRISE ACCOUNTING","")
		endi
		if "EXCISE TRADING" $ xvalue and "VISUAL" $ xvalue
			reg_prods = reg_prods + ',"vutex"'
			xvalue = strtran(xvalue,"EXCISE TRADING","")
		endi
		if "INPUT SERVICE DISTRIBUTOR" $ xvalue and "VISUAL" $ xvalue
			reg_prods = reg_prods + ',"vuisd"'
			xvalue = strtran(xvalue,"INPUT SERVICE DISTRIBUTOR","")
		endi
		if "SERVICE TAX" $ xvalue and "VISUAL" $ xvalue
			reg_prods = reg_prods + ',"vuser"'
			xvalue = strtran(xvalue,"SERVICE TAX","")
		endi
	endfor
endif

clos data all	&& closing database invoice which is automatically opened with lmain

on error do errorprocformain with error()
local sqlconobj
sqlconobj=newobject('sqlconnudobj',"sqlconnection",xapps)
on error

lcdisplayvalue=[ ]

*!*	do form spl  &&ash Rpp
*!*	read events

local lcbuffer,llshowintro
lcbuffer = " " + chr(0)
llshowintro = .t.
if getprivstr("Defaults", "ShowIntroForm", "", @lcbuffer, len(lcbuffer), curdir() + "visudyog.ini") > 0
	llshowintro = (val(lcbuffer) = 0)
endif
local lcontinue, lresume
lcontinue=.t.		&& its default must be .T.

nhandle =0
msqlstr="select * from master..sysdatabases where name = 'Vudyog'"
nretval=sqlconobj.dataconn('EXE','master',msqlstr,"_co_mast","nHandle")
if nretval<0
	return .f.
endif
nretval=sqlconobj.sqlconnclose("nHandle") && Connection Close
if nretval<0
	return .f.
endif
if reccount('_co_mast') = 0
	=messagebox("ERROR !!! System database not found......",64,vumess)
	return .f.
endif

do form frmutilpassword with lWhat
read events

close all
clear all
clear dlls 
quit
return

*!*	Messagebox("QUIT")
*!*	Return

*!*	CLEAR events
*!*	READ events
*!*	QUIT
*!*	RETURN

procedure errorprocformain
lparameters lerror
if lerror = 1
	=messagebox("Application Name has been rename, Application Name should be UDYOGERP.EXE",64,vumess)
	quit
endif
return

