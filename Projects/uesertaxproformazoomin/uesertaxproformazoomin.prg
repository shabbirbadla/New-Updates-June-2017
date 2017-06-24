*:*****************************************************************************
*:        Program: Main.PRG
*:         System: Udyog Software
*:         Author: Shrikant
*:  Last modified: 08/11/2011
*:			AIM  : All Zoom-In Reports
*:*****************************************************************************
Parameters _datasessionid

If Vartype(VuMess) <> [C]
	_Screen.Visible = .F.
	Messagebox("Internal Application Are Not Execute Out-Side ...",16,[])
	Quit
	Return .F.
Endif

nHandle = 0
If Vartype(_datasessionid) = 'N'
	Set DataSession To _datasessionid
Endif

*!*	SqlConObj = Newobject("SqlConNudObj","SqlConnection",xapps)
*!*	nRetval=SqlConObj.sqlconnclose("nHandle")

DO FORM frmzoomin WITH _datasessionid

