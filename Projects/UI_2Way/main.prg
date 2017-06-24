lparameters lcType as string,tnRange as integer

if vartype(VuMess) <> [C]
	_screen.visible = .f.
	messagebox("Internal Application Are Not Execute Out-Side ...",16,"")
	return .f.
endif

do case
case inlist(alltrim(upper(lcType)),"CSV","AUTOCSV")					&& CSV File Generation
	llRunme = .t.
	if alltrim(upper(lcType)) == "AUTOCSV"
		llRunme = Chk_Company()
		if ! llRunme
			messagebox("Company Not Defined In CSV Setting Master",16,VuMess)
		endif
	endif
	if llRunme = .t.
		do form frmgenfilter with lcType,tnRange
	endif
case alltrim(upper(lcType)) = "SET2WAY"							&& 2Way Setting for CSV
	do form frm_set2way with tnRange
endcase


function Chk_Company
nHandle = 0
sqlconobj=newobject('sqlconnudobj',"sqlconnection",xapps)
lcSqlstr = "SELECT Top 1 * FROM Vudyog..Co_Mast WHERE CompId In (SELECT DISTINCT CompId FROM Set_2Way)"
sql_con=sqlconobj.Dataconn("EXE",[Vudyog],lcSqlstr,[Co_Mast_Vw],"nHandle")
if sql_con=< 0
	return .f.
endif
nretval=sqlconobj.sqlconnclose("nHandle") && Connection Close
if nretval<0
	return .f.
endif
if reccount("Co_Mast_Vw") <> 0
	select Co_Mast_Vw
	go top
	scatter name company
else
	return .f.
endif
return .t.
endfunc
