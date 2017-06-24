Lparameters lcEntry_ty,lnTran_Cd,SqlConObj,_ndatasessionid,oHandle,lcDbName,cdSta_Dt,cdEnd_Dt

Local nHandle, lcSqlStr, nRetVal

lcSqlStr =	" SELECT * FROM LCODE WHERE ENTRY_TY = ?lcEntry_ty "
nRetVal= SqlConObj.DataConn([EXE],lcDbName,lcSqlStr,[Lcode_vw],oHandle,_ndatasessionid,.T.)
If nRetVal<0
	Return .F.
Endif

Select Tmp_Lcode
mEnTry_ty=Iif(!Empty(Tmp_Lcode.Bcode_nm),Tmp_Lcode.Bcode_nm,lcEntry_ty)
lcSqlStr =	" SELECT * FROM "+oldDbName+".."+mEnTry_ty+"MAIN WHERE ENTRY_TY = ?lcEntry_ty AND  TRAN_CD = "+Transform(nTran_Cd)
nRetVal= SqlConObj.DataConn([EXE],lcDbName,lcSqlStr,[Main_vw],oHandle,_ndatasessionid,.T.)
If nRetVal<0
	Return .F.
Endif

lcSqlStr =	" SELECT * FROM "+oldDbName+".."+mEnTry_ty+"ITEM WHERE ENTRY_TY = ?lcEntry_ty AND  TRAN_CD = "+Transform(nTran_Cd)
nRetVal= SqlConObj.DataConn([EXE],lcDbName,lcSqlStr,[Item_vw],oHandle,_ndatasessionid,.T.)
If nRetVal<0
	Return .F.
Endif

lcSqlStr =	" SELECT * FROM "+oldDbName+".."+mEnTry_ty+"ACDET WHERE ENTRY_TY = ?lcEntry_ty AND  TRAN_CD = "+Transform(nTran_Cd)
nRetVal= SqlConObj.DataConn([EXE],lcDbName,lcSqlStr,[Acdet_vw],oHandle,_ndatasessionid,.T.)
If nRetVal<0
	Return .F.
Endif

lcSqlStr =	" SELECT * FROM DCMAST WHERE ENTRY_TY = ?lcEntry_ty "
nRetVal= SqlConObj.DataConn([EXE],lcDbName,lcSqlStr,[DcMast_vw],oHandle,_ndatasessionid,.T.)
If nRetVal<0
	Return .F.
Endif

lcSqlStr =	" SELECT * FROM STAX_MAS WHERE ENTRY_TY = ?lcEntry_ty "
nRetVal= SqlConObj.DataConn([EXE],lcDbName,lcSqlStr,[Tax_vw],oHandle,_ndatasessionid,.T.)
If nRetVal<0
	Return .F.
Endif

Return .T.


