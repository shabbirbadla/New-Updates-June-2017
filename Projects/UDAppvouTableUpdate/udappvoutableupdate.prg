***************************************************Auto Transaction*************************************

*Birendra : On 5july2011 TKT - 8452 :Start:
_curvouobj = _Screen.ActiveForm
*Birendra : on 23 Sept 2011 for Auto IP/OP BOM :Start:
ztmpalias = ''
ztmpalias = ALIAS()
*Birendra : on 23 Sept 2011 for Auto IP/OP BOM :End:

IF (oglblprdfeat.udchkprod('AutoTran') OR oglblprdfeat.udchkprod('procinv')) AND !lcode_vw.QC_Module
IF File('uedataexport.app')
	lcSqlstr = "select * from sysobjects where [name] = 'Tbl_DataExport_Mast'"
	mretval = _curvouobj.SqlConObj.dataconn("EXE",company.dbname,lcSqlstr,[Tbl_DbExport_vw],"_curvouobj.nhandle",_curvouobj.DataSessionId,.t.)
	If mretval <= 0
		Return .F.
	Endif
	IF USED('Tbl_DbExport_vw')
		lcSqlstr = "SELECT TOP 1 * FROM Tbl_DataExport_Mast WHERE cMastcode = '"+Main_Vw.Entry_ty+"' AND cType = 'T'"
		mretval = _curvouobj.sqlconobj.dataconn("EXE",company.dbname,lcSqlstr,[Tbl_DbExport_vw],"_curvouobj.nhandle",_curvouobj.DataSessionId,.T.)
		If mretval <= 0
			Return .F.
		Endif
		If Reccount("Tbl_DbExport_vw") = 0
			SELECT Tbl_DbExport_vw
			USE IN Tbl_DbExport_vw
		ELSE
			SELECT Tbl_DbExport_vw
			USE IN Tbl_DbExport_vw
			If Type("_Screen.ActiveForm.Mainalias")<>"U"			
					=uedataexport("INIT","T",Main_Vw.Entry_ty)
					=uedataexport("PROCESS")
			Endif													
		Endif
	ENDIF 
ENDIF 
ENDIF 
*Birendra : on 23 Sept 2011 for Auto IP/OP BOM :Start:
IF USED("projectitref_vw")
	Use In projectitref_vw 
ENDIF 
IF NOT EMPTY(ztmpalias)
SELECT &ztmpalias
ENDIF 
*Birendra : on 23 Sept 2011 for Auto IP/OP BOM :End:

*Birendra : On 5july2011 TKT - 8452 :End:
*****************************************************************************************
