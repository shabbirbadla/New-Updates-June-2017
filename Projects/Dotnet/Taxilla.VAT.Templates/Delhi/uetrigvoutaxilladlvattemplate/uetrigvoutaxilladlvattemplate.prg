Parameters nRange

Declare Integer ShellExecute In shell32.Dll;
	INTEGER hndwin,;
	STRING cAction,;
	STRING cFileName,;
	STRING cParams,;
	STRING cDir,;
	INTEGER nShowWin

cAction = "open"
cFileName = apath + "Delhi.exe"
cParams = '"'+Alltrim(Company.dbname)+'" "'+Alltrim(icopath)+'" "'+Alltrim(vumess)+'" "'+Alltrim(Str(Company.compid))+'"'
cDir = ""
mRtrn = ShellExecute(0,cAction,cFileName,cParams,cDir,1)
Do Case
Case mRtrn = 2
	mMsg = "Bad Association"
Case mRtrn = 29
	mMsg = "Failure to load appication"
Case mRtrn = 30
	mMsg = "Application is busy"
Case mRtrn = 31
	mMsg = "No application association"
Otherwise
	mMsg = ""
Endcase

If !Empty(mMsg)
	Messagebox(mMsg,48,vumess)
Endif

Clear Dlls ShellExecute
