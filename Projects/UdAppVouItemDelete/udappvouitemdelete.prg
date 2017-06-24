Parameters vbefore
&&-->Ipop(Rup)
_Malias 	= Alias()
_mRecNo	= Recno()
_curvouobj = _Screen.ActiveForm
Set DataSession To _curvouobj.DataSessionId

If Type('_curvouobj.PCVTYPE')='C' And ([vuexc] $ vchkprod)
*	If Inlist(_curvouobj.PCVTYPE,'IP','ST','OP','DC')
*Birendra Bug-4543 on 31/07/2012 : Commented and modified with Below one:
	If Inlist(_curvouobj.PCVTYPE,'IP','ST','OP','DC','WI','WO') OR (Inlist(_curvouobj.PCVTYPE,'PT') AND oglblprdfeat.udchkprod('AutoTran') )&&Birendra: TKT-8452 on 15 oct 2011
		If !Used("projectitref_vw")
			msqlstr="SELECT * FROM projectitref where entry_ty='"+Alltrim(main_vw.entry_ty)+"' and tran_cd="+Str(main_vw.tran_cd)
			nretval = _curvouobj.sqlconobj.dataconn("EXE",company.dbname,msqlstr,"projectitref_vw","_curvouobj.nhandle",_curvouobj.DataSessionId)
		Endif
		Delete From projectitref_vw Where projectitref_vw.it_code=item_vw.it_code And projectitref_vw.itserial=item_vw.itserial
	Endif
ENDIF

&& Added By Shrikant S. on 29/12/2012 for Bug-2267		&& Start 	&&vasant030412
If Type('_curvouobj.PcvType') = 'C' AND vbefore = 'BEFORE'
	_mstkresrvtn = .f.
	_mstkresrvtn = oGlblPrdFeat.UdChkProd('stkresrvtn')
	IF _mstkresrvtn = .t.
		If (lcode_vw.entry_ty ='SO' OR lcode_vw.bcode_nm ='SO')
			_mQty = 0
			mSqlStr="Select Top 1 AllocQty From StkResrvSum Where Entry_ty = ?Main_vw.Entry_ty And Tran_cd = ?Main_vw.Tran_cd ;
				And Itserial = ?Item_vw.Itserial"
			sql_con = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,mSqlStr,[chktbl_vw],"This.Parent.nHandle",_curvouobj.DataSessionId,.F.)
			If sql_con > 0 AND Used('chktbl_vw')
				Select chktbl_vw
				_mQty = Iif(Isnull(chktbl_vw.AllocQty)=.F.,chktbl_vw.AllocQty,0)
				Use In chktbl_vw
			Endif
			Select item_vw
			If _mQty > 0
				.ShowMessageBox("Stock Reservation done"+Chr(13)+"Entry Cannot be Deleted",0+32,vumess)
				If !Empty(_Malias)
					Select &_Malias
				Endif
				If Betw(_mRecNo,1,Reccount())
					Go _mRecNo
				Endif
				Return .F.
			Endif
		ENDIF
		If (INLIST(lcode_vw.entry_ty,'WK','PO') OR INLIST(lcode_vw.bcode_nm,'WK','PO') OR lcode_vw.inv_stk = '+')
			_mQty = 0
			mSqlStr="Select SUM(AllocQty) as AllocQty From StkResrvDet Where Entry_ty = ?Main_vw.Entry_ty And Tran_cd = ?Main_vw.Tran_cd ;
				And Itserial = ?Item_vw.Itserial"
			sql_con = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,mSqlStr,[chktbl_vw],"This.Parent.nHandle",_curvouobj.DataSessionId,.F.)
			If sql_con > 0 AND Used('chktbl_vw')
				Select chktbl_vw
				_mQty = Iif(Isnull(chktbl_vw.AllocQty)=.F.,chktbl_vw.AllocQty,0)
				Use In chktbl_vw
			Endif
			Select item_vw
			If _mQty > 0
				.ShowMessageBox("Stock Reservation done"+Chr(13)+"Entry Cannot be Deleted",0+32,vumess)
				If !Empty(_Malias)
					Select &_Malias
				Endif
				If Betw(_mRecNo,1,Reccount())
					Go _mRecNo
				Endif
				Return .F.
			Endif
		ENDIF
	ENDIF
Endif
&& Added By Shrikant S. on 29/12/2012 for Bug-2267		&& End	 	&&vasant030412

&& Added by Shrikant S. on 29/11/2016 for GST		&& Start
If Type('_curvouobj.PcvType') = 'C' AND vbefore = 'BEFORE'
	IF USED('Acdetalloc_vw')	
		SELECT Acdetalloc_vw
		LOCATE FOR Entry_ty=Main_vw.Entry_ty AND Tran_cd=Main_vw.Tran_cd AND Itserial=Item_vw.Itserial
		IF FOUND()
			DELETE IN Acdetalloc_vw	
		endif
	endif
Endif
&& Added by Shrikant S. on 29/11/2016 for GST		&& End

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

******** Added By Sachin N. S. on 13/02/2012 for TKT-9411 and Bug-660 Batchwise/Serialize Inventory ******** Start
If _curvouobj.itempage And Vartype(_curvouobj._BatchSerialStk)='O'
	etsql_con=_curvouobj._BatchSerialStk._ueTrigVouItemDelete(vbefore)
	Return Iif(etsql_con>0,.T.,.F.)
Endif
******** Added By Sachin N. S. on 13/02/2012 for TKT-9411 and Bug-660 Batchwise/Serialize Inventory ******** End

&& Added by Shrikant S. on 28/06/2014 for Bug-23280			&& Start
If Vartype(oglblindfeat)='O'
	If oglblindfeat.udchkind('pharmaind')
		If Alltrim(vbefore)=="BEFORE"
			If Inli(.PCVTYPE,"AR")
				If Used('BatchTbl_vw')
					Select BatchTbl_vw
					Locate For itserial=item_vw.itserial
					If Found()
						Delete In BatchTbl_vw
					Endif
				Endif
			Endif
		Endif
	Endif
Endif
&& Added by Shrikant S. on 28/06/2014 for Bug-23280			&& End


