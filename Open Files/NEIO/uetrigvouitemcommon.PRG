para colno
_curvouobj = _Screen.ActiveForm
&&vasant061009
IF TYPE('_curvouobj.mainalias') = 'C'
	IF UPPER(_curvouobj.mainalias) <> 'MAIN_VW'
		RETURN 
	Endif	
ENDIF
&&vasant061009
*SET datasession to _curvouobj.datasessionid	&&vasant071009
IF TYPE('_curvouobj.PcvType') = 'C'
	*!*----->IP & OP FOR BatchProcess(Rup)
	if main_vw.entry_ty='IP' and inlist(colno,3) and USED('projectitref_vw') AND ([vuexc] $ vchkprod)
		_Malias 	= Alias()
		_mRecNo	= Recno()

		SELECT aentry_ty,atran_cd,aitserial,qty FROM projectitref_vw WHERE entry_ty=main_vw.entry_ty AND tran_cd=main_vw.tran_cd AND itserial=item_vw.itserial INTO CURSOR tibl
		IF used ('tibl')
			if(tibl.qty=0)
				RETURN .t.
			ENDIF 
			IF item_vw.qty<tibl.qty
				replace qty WITH item_vw.qty,qty WITH item_vw.qty IN projectitref_vw FOR entry_ty=main_vw.entry_ty AND tran_cd=main_vw.tran_cd AND itserial=item_vw.itserial
			ENDIF 
			IF item_vw.qty>tibl.qty
				
				etsql_con	= 1
				nHandle     = 0
				_etDataSessionId = _Screen.Activeform.DataSessionId
				SET DATASESSION TO _etDataSessionId
				SqlConObj = NEWOBJECT('SqlConnUdObj','SqlConnection',xapps)
				
				etsql_str = "EXECUTE USP_ENT_BOMDET_CHK_IP '"+item_vw.entry_ty+"',"+alltrim(STR(item_vw.tran_cd))+",'"+ALLTRIM(main_vw.rule);
								+"','"+ALLTRIM(main_vw.inv_sr)+"','"+ALLTRIM(main_vw.cate)+"','"+ALLTRIM(main_vw.dept);
								+"','"+tibl.aentry_ty+"',"+ALLTRIM(STR(tibl.atran_cd))+",'"+tibl.aitserial+"',"+ALLTRIM(STR(item_vw.it_code))
				
					etsql_con = SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[tibl_1],;
				"nHandle",_etDataSessionId,.f.)
				If Used('tibl_1')
					select sum(qty) as tqy from projectitref_vw where aentry_ty=tibl.aentry_ty and atran_cd=tibl.atran_cd and aitserial=tibl.aitserial AND ait_code=item_vw.it_code	AND itserial<>item_vw.itserial INTO CURSOR tibl_2 
					If Used('tibl_2')
						SELECT 	tibl_2
						replace balqty WITH balqty-IIF(!ISNULL(tibl_2.tqy),tibl_2.tqy,0) IN tibl_1
						*balitem_vw.qty)>balitem_vw.balqty+balitem_vw.tlissqtyp
						IF item_vw.qty>tibl_1.balqty+tibl_1.tlissqtyp
							=messagebox('Qty. Could not be grater then '+ALLTRIM(str(tibl_1.balqty+tibl_1.tlissqtyp,14,3)),0+64,VuMess)
							replace qty WITH tibl.qty IN item_vw
							_Screen.Activeform.Voupage.Page1.Grditem.columns(colno-1).text1.setfocus()
							RETURN .f.
						ELSE
							replace qty WITH item_vw.qty,qty WITH item_vw.qty IN projectitref_vw FOR entry_ty=main_vw.entry_ty AND tran_cd=main_vw.tran_cd AND itserial=item_vw.itserial
						ENDIF 
						USE IN tibl_2
					ENDIF 
					USE IN tibl_1
				ENDIF 	
			ENDIF 
			USE IN tibl
		ENDIF 	
		If !Empty(_Malias)
			Select &_Malias
		ENDIF
		If Betw(_mRecNo,1,Reccount())
			Go _mRecNo
		ENDIF
	endif

	*-----------------
	if main_vw.entry_ty='OP' and inlist(colno,3) and USED('projectitref_vw') AND ([vuexc] $ vchkprod)
		_Malias 	= Alias()
		_mRecNo	= Recno()
		SELECT aentry_ty,atran_cd,aitserial,qty FROM projectitref_vw WHERE entry_ty=main_vw.entry_ty AND tran_cd=main_vw.tran_cd AND itserial=item_vw.itserial INTO CURSOR tibl
		IF used ('tibl')
			SELECT tibl
			IF RECCOUNT()>0
					
				IF item_vw.qty<tibl.qty
					replace qty WITH item_vw.qty,qty WITH item_vw.qty IN projectitref_vw FOR entry_ty=main_vw.entry_ty AND tran_cd=main_vw.tran_cd AND itserial=item_vw.itserial
				ENDIF 
				IF item_vw.qty>tibl.qty
					etsql_con	= 1
					nHandle     = 0
					_etDataSessionId = _Screen.Activeform.DataSessionId
					SET DATASESSION TO _etDataSessionId
					SqlConObj = NEWOBJECT('SqlConnUdObj','SqlConnection',xapps)
				
					etsql_str = "USP_ENT_CHK_OP_ALLOCATION '"+item_vw.entry_ty+"',"+alltrim(STR(item_vw.tran_cd))+","+alltrim(STR(item_vw.IT_CODE))+",'"+item_vw.itserial+"','";
								+tibl.Aentry_ty+"',"+ALLTRIM(STR(tibl.atran_cd))+",'"+tibl.aitserial+"'"
				
					etsql_con = SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[tibl_1],"nHandle",_etDataSessionId,.f.)
					If Used('tibl_1')
						select sum(qty) as tqy from projectitref_vw where aentry_ty=tibl.aentry_ty and atran_cd=tibl.atran_cd and aitserial=tibl.aitserial AND ait_code=item_vw.it_code	AND itserial<>item_vw.itserial INTO CURSOR tibl_2 
						If Used('tibl_2')
							SELECT 	tibl_2
							replace balqty WITH balqty-IIF(!ISNULL(tibl_2.tqy),tibl_2.tqy,0),wipqty WITH wipqty-IIF(!ISNULL(tibl_2.tqy),tibl_2.tqy,0) IN tibl_1
							IF ((item_vw.qty>tibl_1.wipqty) AND tibl_1.wipqty<>0)
								=messagebox('Qty. could not be grater then '+ALLTRIM(str(tibl_1.wipqty,14,3)),0+64,VuMess)
								replace qty WITH tibl.qty IN item_vw
								_Screen.Activeform.Voupage.Page1.Grditem.columns(colno-1).text1.setfocus()
								RETURN .f.
							ELSE
								replace qty WITH item_vw.qty IN projectitref_vw FOR entry_ty=main_vw.entry_ty AND tran_cd=main_vw.tran_cd AND itserial=item_vw.itserial
							ENDIF 
							USE IN tibl_2
						ENDIF 
						USE IN tibl_1
					ENDIF 	
				ENDIF 
			ENDIF 	&&IF RECCOUNT>0	
			USE IN tibl
		ENDIF 		&&IF used ('tibl')
		If !Empty(_Malias)
			Select &_Malias
		ENDIF
		If Betw(_mRecNo,1,Reccount())
			Go _mRecNo
		ENDIF
	endif
	*-----------------
	*if main_vw.entry_ty ='ST' and inlist(colno,3) and USED('projectitref_vw') AND (([vuexc] $ vchkprod) Or ([vuinv] $ vchkprod))
	if INLIST(main_vw.entry_ty,'DC','ST') and inlist(colno,2) and USED('projectitref_vw') AND ([vuexc] $ vchkprod)
		_Malias = Alias()
		_mRecNo	= Recno()
		SELECT projectitref_vw
		LOCATE 
		paqty=projectitref_vw.aqty
		SELECT sum(qty) as qty FROM projectitref_vw WHERE (entry_ty=main_vw.entry_ty AND tran_cd=main_vw.tran_cd AND itserial=item_vw.itserial) INTO CURSOR tibl &&aentry_ty,atran_cd,aitserial,
		IF used ('tibl')
			IF (item_vw.qty=tibl.qty)
				USE IN tibl
				RETURN .t.
			ENDIF
			SELECT aentry_ty,atran_cd,aitserial,qty FROM projectitref_vw WHERE entry_ty=main_vw.entry_ty AND tran_cd=main_vw.tran_cd AND itserial=item_vw.itserial INTO CURSOR tibl1
			SELECT tibl1
			IF (RECCOUNT()<=0)
				USE IN tibl
				USE IN tibl1
				RETURN .t.
			ENDIF 
			IF (RECCOUNT()>1) AND (item_vw.qty<>tibl.qty)
				=messagebox('Please delete this item then make new entry with New Allocation...!'),0+64,VuMess)
				replace qty WITH tibl.qty IN item_vw
				_Screen.Activeform.Voupage.Page1.Grditem.columns(colno-1).text1.setfocus()
				USE IN tibl
				USE IN tibl1
				RETURN .f.
			ENDIF

			IF item_vw.qty<tibl.qty
				replace qty WITH item_vw.qty IN projectitref_vw FOR entry_ty=main_vw.entry_ty AND tran_cd=main_vw.tran_cd AND itserial=item_vw.itserial			
			ENDIF 

			IF item_vw.qty>tibl.qty
				etsql_con	= 1
				nHandle     = 0

				_etDataSessionId = _Screen.Activeform.DataSessionId
				SET DATASESSION TO _etDataSessionId
				SqlConObj = NEWOBJECT('SqlConnUdObj','SqlConnection',xapps)
				etsql_str = "USP_ENT_CHK_ST_ALLOCATION '"+item_vw.entry_ty+"',"+alltrim(STR(item_vw.tran_cd))+",'"+item_vw.itserial+"',"+alltrim(STR(item_vw.IT_CODE))+",'";
								+tibl1.Aentry_ty+"',"+ALLTRIM(STR(tibl1.atran_cd))+",'"+tibl1.aitserial+"'"

				etsql_con = SqlConObj.DataConn([EXE],Company.DbName,etsql_str,[tibl_1],"nHandle",_etDataSessionId,.f.)
				If Used('tibl_1')
	*!*					SELECT 	projectitref_vw
					IF (RECCOUNT()=0) AND (item_vw.qty>paqty)
						=messagebox('Qty. could not be grater then '+ALLTRIM(str(paqty,14,3)),0+64,VuMess)
						replace qty WITH tibl.qty IN item_vw
						RETURN .f.
					ENDIF 
					select sum(qty) as tqy from projectitref_vw where aentry_ty=tibl1.aentry_ty and atran_cd=tibl1.atran_cd and aitserial=tibl1.aitserial AND ait_code=item_vw.it_code	AND itserial<>item_vw.itserial INTO CURSOR tibl_2 

					If Used('tibl_2')

						replace balqty WITH balqty-IIF(!ISNULL(tibl_2.tqy),tibl_2.tqy,0) IN tibl_1
						IF item_vw.qty>tibl_1.balqty AND tibl_1.balqty!=null AND tibl_1.balqty!=0
							=messagebox('Qty. could not be grater then '+ALLTRIM(str(tibl_1.balqty,14,3)),0+64,VuMess)
							replace qty WITH tibl.qty IN item_vw
							_Screen.Activeform.Voupage.Page1.Grditem.columns(colno-1).text1.setfocus()
							RETURN .f.
						ELSE 

							replace qty WITH item_vw.qty IN projectitref_vw FOR entry_ty=main_vw.entry_ty AND tran_cd=main_vw.tran_cd AND itserial=item_vw.itserial
						ENDIF 
						USE IN tibl_2
					ENDIF 

					USE IN tibl_1
				ENDIF 	
			ENDIF 
			USE IN tibl
		ENDIF 	
		If !Empty(_Malias)
			Select &_Malias
		ENDIF
		If Betw(_mRecNo,1,Reccount())
			Go _mRecNo
		ENDIF
	ENDIF
*!*<-----IP & OP FOR BatchProcess(Rup)
Endif
