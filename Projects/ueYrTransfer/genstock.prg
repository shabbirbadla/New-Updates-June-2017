Lparameters SqlConObj,_ndatasessionid,oHandle,ccDbName,cdSta_Dt,cdEnd_Dt

lcSqlStr =	" SELECT * FROM Stkl_vw_Item "
nRetVal= SqlConObj.DataConn([EXE],ccDbName,lcSqlStr,[Tmp_Lcode],oHandle,_ndatasessionid,.T.)
If nRetVal<0
	Return .F.
Endif

