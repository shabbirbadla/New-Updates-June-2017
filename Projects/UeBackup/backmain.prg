Lparameters tcDateFunc

_Screen.Visible = .F.

Declare Integer GetPrivateProfileString In Win32API As GetPrivStr ;
	STRING cSection, String cKey, String cDefault, String @cBuffer, ;
	INTEGER nBufferSize, String cINIFile

Local iniFilePath,lcExeName,ueapath,uexapps

ueapath = Allt(Fullpath(Curd()))
iniFilePath = ueapath+"visudyog.ini"

If !File(iniFilePath)
	Messagebox("Configuration File Not found",16,'Udyog Admin')
	Retu .F.
Endif

mvu_one = Space(2000)
mvu_two = 0
mvu_two	= GetPrivStr([Settings],"XFile", "", @mvu_one, Len(mvu_one), ueapath + "visudyog.ini")
uexapps = Left(mvu_one,mvu_two)

If Vartype(uexapps) <> 'C' Or Empty(uexapps)
	=Messagebox('In Configuration file Xfile Path cannot be empty',16,[Udyog iTAX])
	Return .F.
Else
	If !File(uexapps)
		=Messagebox('In Configuration file Specify application file is not found',16,[Udyog iTAX])
		Return .F.
	Endif
	lcExeName = Allt(uexapps)
	Do &lcExeName With [B],tcDateFunc
Endif
