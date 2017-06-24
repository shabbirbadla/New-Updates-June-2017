
If Type("_Screen.ActiveForm.Mainalias")<>"U"
	If oglblprdfeat.udchkprod('AutoTran')
		If Inlist(Upper(Alltrim(_Screen.ActiveForm.Mainalias)),"IT_MAST_VW","AC_MAST_VW","ITEM_GROUP_VW","AC_GROUP_MAST_VW","DEPARTMENT_VW","CATEGORY_VW","WAREHOUSE_VW","SERIES_VW","_STMAST","_DCMAST","_LOTHER")
			=uedataexport("INIT","M",Upper(Alltrim(_Screen.ActiveForm.maintbl)))
			=uedataexport("PROCESS")
		Endif
	Endif
Endif
