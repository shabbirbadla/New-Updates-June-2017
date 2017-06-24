*****lparameters tcDateFunc
_screen.visible = .f.

declare integer GetPrivateProfileString in Win32API as GetPrivStr ;
	string cSection, string cKey, string cDefault, string @cBuffer, ;
	integer nBufferSize, string cINIFile

local iniFilePath,lcExeName,ueapath,uexapps

ueapath = allt(fullpath(curd()))
iniFilePath = ueapath+"visudyog.ini"

if !file(iniFilePath)
	messagebox("Configuration File Not found",16,'Udyog Admin')
	retu .f.
endif

mvu_one = space(2000)
mvu_two = 0
mvu_two	= GetPrivStr([Settings],"XFile", "", @mvu_one, len(mvu_one), ueapath + "visudyog.ini")
uexapps = left(mvu_one,mvu_two)

if vartype(uexapps) <> 'C' or empty(uexapps)
	=messagebox('In Configuration file Xfile Path cannot be empty',16,[Udyog iTAX])
	return .f.
else
	if !file(uexapps)
		=messagebox('In Configuration file Specify application file is not found',16,[Udyog iTAX])
		return .f.
	endif
	lcExeName = allt(uexapps)
	do &lcExeName with [CSV],"",.f.
endif
