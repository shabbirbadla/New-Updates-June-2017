
Proc con
Para CBackend
local conyes
Public constr,chqcon
conyes = .t.
on error conyes = .f.
Do case
Case CBackend = "1" && SQL server
constr= "Driver={SQL Server};server=Tushar;database=Neio;uid=sa;pwd=;"
Chqcon = Sqlstringconnect(constr)
If chqcon < 0
		Messagebox("Cannot Establish connection to SQL Server...Please check the Server Name",16,"Visual Udyog")
		nodefa
		retu .f.
		quit
	Endif
Case CBackend="0"
*	constr = 'DRIVER=Microsoft Visual FoxPro Driver;UID=;Deleted=Yes;Null=Yes;Collate=Machine;BackgroundFetch=Yes;Exclusive=No;SourceType=DBC;SourceDB=d:\vUdyog_latest\nxio\Invoice.Dbc'
	cdir=sys(5)+allt(co_mast.dir_nm)+"invoice.dbc"
	constr = 'DRIVER=Microsoft Visual FoxPro Driver;UID=;Deleted=Yes;Null=Yes;Collate=Machine;BackgroundFetch=Yes;Exclusive=No;SourceType=DBC;SourceDB=&cdir'
	chqcon=Sqlstringconnect(constr)
	if chqcon < 0
		Messagebox("Cannot Establish connection ...",16,"Visual Udyog")
		retu .f.
	Endif
Endcase
*!*	public str
*!*	str = ""
*!*	str = CreateObject("Adodb.Connection")
*!*	If Type('Str') = "O"
*!*		Str.Connectionstring = constr
*!*		Str.open() && Opening connection
*!*		if conyes = .f.
*!*			Messagebox("Cannot Establish connection to SQL Server...Please check the Server Name",16,"Visual Udyog")
*!*			rele conyes
*!*			retu .f.
*!*		else
*!*			return .t.	
*!*		Endif	
*!*	Endif