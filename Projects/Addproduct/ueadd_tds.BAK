_SCREEN.VISIBLE = .F.

DECLARE INTEGER GetPrivateProfileString IN Win32API AS GetPrivStr ;
	STRING cSection, STRING cKey, STRING cDefault, STRING @cBuffer, ;
	INTEGER nBufferSize, STRING cINIFile

LOCAL iniFilePath,lcExeName,ueapath

ueapath = ALLT(FULLPATH(CURD()))
iniFilePath = ueapath+"visudyog.ini"

IF !FILE(iniFilePath)
	MESSAGEBOX("Configuration File Not found",16,'Udyog Admin')
	RETU .F.
ENDIF

PUBLIC uexapps,nHandle,gcConnstr,mvu_server,mvu_user,mvu_pass

mvu_one = SPACE(2000)
mvu_two = 0
mvu_two	= GetPrivStr([Settings],"XFile", "", @mvu_one, LEN(mvu_one), ueapath + "visudyog.ini")
uexapps = LEFT(mvu_one,mvu_two)
mvu_two    = GetPrivStr([DataServer],"Name", "", @mvu_one, LEN(mvu_one), iniFilePath)
mvu_server = LEFT(mvu_one,mvu_two)
mvu_two    = GetPrivStr([DataServer],onencrypt(enc("User")), "", @mvu_one, LEN(mvu_one), iniFilePath)
mvu_user   = LEFT(mvu_one,mvu_two)
mvu_two    = GetPrivStr([DataServer],onencrypt(enc("Pass")), "", @mvu_one, LEN(mvu_one), iniFilePath)
mvu_pass   = LEFT(mvu_one,mvu_two)

IF VARTYPE(uexapps) <> 'C' OR EMPTY(uexapps)
	=MESSAGEBOX('In Configuration file Xfile Path cannot be empty',16,[Udyog iTAX])
	RETURN .F.
ELSE
	IF !FILE(uexapps)
		=MESSAGEBOX('In Configuration file Specify application file is not found',16,[Udyog])
		RETURN .F.
	ENDIF
ENDIF

_mvu_user1 = dec(ondecrypt(mvu_user))
_mvu_pass1 = dec(ondecrypt(mvu_pass))

nHandle = 0
gcConnstr = "Driver={SQL Server};Server="+ALLTRIM(mvu_server)+";Database=Vudyog;Uid="+ALLTRIM(_mvu_user1)+";Pwd="+ALLTRIM(_mvu_pass1)
nHandle=SQLSTRINGCONNECT(gcConnstr)
IF nHandle < 0
	RETURN .F.
ENDIF

lnReturn = SQLEXEC(nHandle,"SELECT CompId,Co_Name,Passroute From Vudyog..Co_Mast","Company")
IF lnReturn < 0
	RETURN .F.
ENDIF

oComp = CREATEOBJECT("Empty")
ADDPROPERTY(oComp,"Co_Name","")
SELECT Company
SCAN
	isUpdatePRoute = .F.
	cCurrentProd = chkprod()
	IF "vuser" $ cCurrentProd		&& Searching service tax product
		IF ! "vutds" $ cCurrentProd	&& Searching TDS product
			isUpdatePRoute = .T.
			cCurrentProd = ALLTRIM(cCurrentProd)+"vutds"
			REPLACE Passroute WITH EncryptPassroute(cCurrentProd) IN Company
		ENDIF
	ENDIF
	IF isUpdatePRoute	&& Updating TDS product
		oComp.Co_Name = oComp.Co_Name+ALLTRIM(Company.Co_Name)+CHR(13)
		lnReturn = SQLEXEC(nHandle,"Update Vudyog..Co_Mast SET Passroute = ?Company.Passroute Where CompId = ?Company.CompId")
		IF lnReturn < 0
			LOOP
		ENDIF
	ENDIF				&& Updating TDS product
	SELECT Company
ENDSCAN
SQLDISCONNECT(nHandle)
IF !EMPTY(oComp.Co_Name)
	MESSAGEBOX("Product <TDS> is added successfully in below companies."+CHR(13)+oComp.Co_Name,64,[Udyog])
ELSE
	MESSAGEBOX("Service Tax product is not found in existing companies."+CHR(13)+oComp.Co_Name,64,[Udyog])
ENDIF

*!*	vuser			&& Service Tax
*!*	vutds			&& TDS


FUNCTION onencrypt
******************
LPARA lcvariable
lcreturn = ""
FOR i=1 TO LEN(lcvariable)
	lcreturn=lcreturn+CHR(ASC(SUBSTR(lcvariable,i,1))+ASC(SUBSTR(lcvariable,i,1)))
ENDFOR
RETURN lcreturn
ENDFUNC

FUNCTION ondecrypt
*****************
LPARA lcvariable
lcreturn = ""
FOR i=1 TO LEN(lcvariable)
	lcreturn=lcreturn+CHR(ASC(SUBSTR(lcvariable,i,1))/2)
ENDFOR
RETURN lcreturn
ENDFUNC

FUNCTION decoder
*****************
PARAMETERS thispass,passflag
STORE "" TO finecode,mycoder
FOR i = 1 TO LEN(thispass)
	IF !passflag
		mycoder = CHR(ASC(SUBSTR(thispass,i,1))-4) &&+7-11)
	ELSE
		mycoder = CHR(ASC(SUBSTR(thispass,i,1))+4) &&-7+11)
	ENDIF (!passflag)
	finecode = finecode+mycoder
NEXT (i)
RETURN finecode
ENDFUNC

FUNCTION enc
************
PARA mcheck
d=1
F=LEN(mcheck)
REPL=""
rep=0
DO WHIL F > 0
	r=SUBS(mcheck,d,1)
	CHANGE = ASC(r)+rep
	IF CHANGE>255
		WAIT WIND STR(CHANGE)
	ENDI
	two = CHR(CHANGE)
	REPL=REPL+two
	d=d+01
	rep=rep+1
	F=F-1
ENDD
RETU REPL
ENDFUNC

FUNCTION dec
************
PARA mcheck
d=1
F=LEN(mcheck)
REPL=""
rep=0
DO WHIL F > 0
	r=SUBS(mcheck,d,1)
	CHANGE = ASC(r)-rep
	IF CHANGE>0
		two = CHR(CHANGE)
	ENDI
	REPL=REPL+two
	d=d+01
	F=F-1
	rep=rep+1
ENDD
RETU REPL
ENDFUNC

FUNCTION chkprod
****************
BUFFER=[]
IF !EMPTY(ALLT(Company.Passroute))
	FOR x = 1 TO LEN(ALLT(Company.Passroute))
		BUFFER = BUFFER + CHR(ASC(SUBSTR(Company.Passroute,x,1))/2)
	NEXT x
ENDIF
vchkprod=BUFFER
RETURN vchkprod
ENDFUNC


FUNCTION EncryptPassroute
*************************
LPARAMETERS cProd AS STRING
LOCAL finalprod,mprod
finalprod=""
mprod="cProd"
mprod=mprod
FOR j=1 TO LEN(&mprod)
	finalprod=finalprod+CHR(ASC(SUBSTR(&mprod,j,1))*2)
ENDFOR
RETURN finalprod
ENDFUNC
