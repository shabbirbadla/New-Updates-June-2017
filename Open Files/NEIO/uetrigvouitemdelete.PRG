Parameters vbefore
&&-->Ipop(Rup)
_Malias 	= Alias()
_mRecNo	= Recno()
_curvouobj = _Screen.ActiveForm
Set DataSession To _curvouobj.DataSessionId

If Type('_curvouobj.PCVTYPE')='C' And ([vuexc] $ vchkprod)
	If Inlist(_curvouobj.PCVTYPE,'IP','ST','OP','DC')
		If !Used("projectitref_vw")
			msqlstr="SELECT * FROM projectitref where entry_ty='"+Alltrim(main_vw.entry_ty)+"' and tran_cd="+Str(main_vw.tran_cd)
			nretval = _curvouobj.sqlconobj.dataconn("EXE",company.dbname,msqlstr,"projectitref_vw","_curvouobj.nhandle",_curvouobj.DataSessionId)
		Endif
		Delete From projectitref_vw Where projectitref_vw.it_code=item_vw.it_code And projectitref_vw.itserial=item_vw.itserial
	Endif
Endif
If !Empty(_Malias)
	Select &_Malias
Endif
If Betw(_mRecNo,1,Reccount())
	Go _mRecNo
Endif
&&<--Ipop(Rup)
&&--->Rup 12/08/2009
If Used('Gen_SrNo_Vw')
	Delete From Gen_SrNo_Vw Where Gen_SrNo_Vw.itserial=item_vw.itserial
Endif
&&<---Rup 12/08/2009

******** Added By Sachin N. S. on 15/07/2011 for Batchwise/Serialize Inventory ******** Start
If _curvouobj.itempage And Vartype(_curvouobj._BatchSerialStk)='O'
	etsql_con=_curvouobj._BatchSerialStk._ueTrigVouItemDelete(vbefore)
	Return Iif(etsql_con>0,.T.,.F.)
Endif
******** Added By Sachin N. S. on 15/07/2011 for Batchwise/Serialize Inventory ******** End
