IF TYPE('_screen.ActiveForm.txtpartyname')=='O' AND TYPE('_screen.ActiveForm.currentpartyname')<>'U' && Birendra : Bug-21315 on 10/01/2014 :Added condition:

mAlias = Alias()

&& Added by Shrikant S. on 21/06/2012 for Bug-4744		&& Start
	If Vartype(oGlblPrdFeat)='O'
		If oGlblPrdFeat.UdChkProd('openord')
			If Type('lcode_vw.allowzeroqty')!='U'
				If lcode_Vw.allowzeroqty=.T.
					If Inlist(Main_Vw.Entry_ty,'OO')
						With _Screen.ActiveForm
							tot_grd_col=.voupage.page1.grditem.ColumnCount
							For i = 1 To tot_grd_col
								Do Case
								Case .voupage.page1.grditem.Columns(i).header1.Caption='Quantity'
									.voupage.page1.grditem.Columns(i).Visible=.F.
									.voupage.page1.grditem.Columns(i).ReadOnly=.T.
								Endcase
							Endfor
						Endwith
					Endif
				Endif
			Endif
		Endif
	Endif
&& Added by Shrikant S. on 21/06/2012 for Bug-4744		&& End

*------------------------------------------FOR DEBIT NOTE --------------------------------------------------
IF USED('main_vw') AND (_screen.ActiveForm.addmode OR _screen.ActiveForm.editmode) && Birendra : Bug-21315 on 10/01/2014 :Added condition:
	IF INLIST(MAIN_VW.ENTRY_tY,'ST','PT') 
	*IF INLIST(MAIN_VW.ENTRY_tY,'ST','PT') 
	SELECT main_vw
		IF TYPE('U_INTR_PER')#'U'
			_curfrmobj = _screen.ActiveForm
			IF TYPE( "_curfrmobj.sqlconobj")='O' 
	*!*				nRet = _curfrmobj.sqlconobj.DataConn([EXE],Company.DbName,"select ac_name,i_rate,i_days,intpay from ac_mast where ac_name='"+main_vw.party_nm+"'",[tmp_acmast],;
	*!*						"_curfrmobj.nHandle",_curfrmobj.DataSessionId,.f.)   && Commented by Archana K. on 2/1/2013 for Bug-7858  
					nRet = _curfrmobj.sqlconobj.DataConn([EXE],Company.DbName,"select ac_name,i_rate,i_days,intpay from ac_mast where ac_name=?main_vw.party_nm",[tmp_acmast],;
						"_curfrmobj.nHandle",_curfrmobj.DataSessionId,.f.)  && Changes by Archana K. on 2/1/2013 for Bug-7858
					If nRet <= 0
					RETURN .f.
					ENDIF 
				IF main_vw.u_intr_per=0
				replace main_vw.U_INTR_PER WITH tmp_acmast.i_rate IN main_vw
				ENDIF 
				IF USED("tmp_acmast")
				USE IN tmp_acmast
				ENDIF 
			ENDIF 
		SELECT main_vw
		ENDIF 
	ENDIF 
ENDIF 
*!*	IF INLIST(MAIN_VW.ENTRY_tY,'D1','D2','D3','D4','D5','C3','C2','C4','C5') AND (_screen.ActiveForm.addmode OR _screen.ActiveForm.editmode)		&& Commented by Shrikant S. on 17/03/2012 for Bug-2276
IF INLIST(MAIN_VW.ENTRY_tY,'D2','D3','D4','D5','C3','C2','C4','C5') AND (_screen.ActiveForm.addmode OR _screen.ActiveForm.editmode)			&& Added by Shrikant S. on 17/03/2012 for Bug-2276
		WITH _screen.ActiveForm 
			tot_grd_col=.voupage.page1.grditem.columncount	
			FOR i = 1 TO tot_grd_col
				DO CASE 
				   CASE .voupage.page1.grditem.columns(i).header1.caption='Ass. Value'
						.voupage.page1.grditem.columns(i).visible=.F.	

		    	   CASE .voupage.page1.grditem.columns(i).header1.caption='Rate'
						.voupage.page1.grditem.columns(i).visible=.F.	
	
				   CASE .voupage.page1.grditem.columns(i).header1.caption='Quantity'
				 		.voupage.page1.grditem.columns(i).visible=IIF(INLIST(main_vw.entry_ty,'D5','D4','C4','C5'),.t.,.f.)		&& Added by Shrikant S. on 17/03/2012 for Bug-2276
*!*					 		.voupage.page1.grditem.columns(i).visible=IIF(INLIST(main_vw.entry_ty,'D1','D5','D4','C4','C5'),.t.,.f.)	&& Commented by Shrikant S. on 17/03/2012 for Bug-2276
	
		           CASE .voupage.page1.grditem.columns(i).header1.caption='Sale Bill No.'
				  		.voupage.page1.grditem.columns(i).width=70
	
				   CASE .voupage.page1.grditem.columns(i).header1.caption='Item Name'
				 		.voupage.page1.grditem.columns(i).width=120
						.voupage.page1.grditem.Columns(i).Visible=.F.		&& Added by Shrikant S. on 04/01/2013 for Bug-7269
*!*					.voupage.page1.grditem.Columns(i).Visible=Iif(Inlist(_Screen.ActiveForm.pcvtype,'D3','C3'),.T.,.F.)		&& Added by Shrikant S. on 17/03/2012 for Bug-2276	&& Commented by Shrikant S. on 04/01/2013 for Bug-7269
*!*							.voupage.page1.grditem.columns(i).visible=IIF(INLIST(_screen.ActiveForm.pcvtype,'D3','D1','C3'),.t.,.f.)		&& Commented by Shrikant S. on 17/03/2012 for Bug-2276


*				   CASE .voupage.page1.grditem.columns(i).header1.caption='Payment Recd. Date'
				   CASE .voupage.page1.grditem.columns(i).header1.caption=IIF(INLIST(main_vw.entry_ty,'C3'),'Payment Made Date','Payment Recd. Date')
				 		.voupage.page1.grditem.columns(i).header1.caption=IIF(INLIST(main_vw.entry_ty,'C3'),'Payment Made Date','Payment Recd. Date')
				 		.voupage.page1.grditem.columns(i).visible=IIF(INLIST(main_vw.entry_ty,'D3','C3'),.t.,.f.)
		
*				   CASE .voupage.page1.grditem.columns(i).header1.caption='Recd. Amount'
				   CASE .voupage.page1.grditem.columns(i).header1.caption=IIF(INLIST(main_vw.entry_ty,'C3'),'Made Amount','Recd. Amount')
				 		.voupage.page1.grditem.columns(i).header1.caption=IIF(INLIST(main_vw.entry_ty,'C3'),'Made Amount','Recd. Amount')
				 		.voupage.page1.grditem.columns(i).visible=IIF(INLIST(main_vw.entry_ty,'D3','C3'),.t.,.f.)
				 		
				   CASE .voupage.page1.grditem.columns(i).header1.caption='Late Days'
				 		.voupage.page1.grditem.columns(i).visible=IIF(INLIST(main_vw.entry_ty,'D3','C3'),.t.,.f.)
	
				   CASE .voupage.page1.grditem.columns(i).header1.caption='Interest %'
				 		.voupage.page1.grditem.columns(i).visible=IIF(INLIST(main_vw.entry_ty,'D3','C3'),.t.,.f.)
	
				   CASE .voupage.page1.grditem.columns(i).header1.caption='Interest Amount'
				 		.voupage.page1.grditem.columns(i).visible=IIF(INLIST(main_vw.entry_ty,'D3','C3'),.t.,.f.)
					 	.voupage.page1.grditem.refresh	
			    ENDCASE
			ENDFOR
		ENDWITH
			SELECT item_vw
		IF main_vw.entry_ty='D3'   AND _screen.ActiveForm.addmode
			* Check for Interest Pay Party :Start
			_curfrmobj = _screen.ActiveForm 
			nRet = _curfrmobj.sqlconobj.DataConn([EXE],Company.DbName,"select ac_name,i_rate,i_days,intpay from ac_mast where ac_name='"+main_vw.party_nm+"'",[tmp_acmast],;
					"_curfrmobj.nHandle",_curfrmobj.DataSessionId,.f.)
				If nRet <= 0
				RETURN .f.
				ENDIF 
			IF tmp_acmast.intpay

				SELECT tmp_acmast
				_curfrmobj.intbaseday=tmp_acmast.i_days

				IF USED("tmp_acmast")
				USE IN tmp_acmast
				ENDIF 

			* Check for Interest Pay Party :End

				DO FORM frmgetdate WITH DATE()
				nret=0
				mAlias = Alias()
				_curfrmobj = _screen.ActiveForm 
				* Cash Receipt
				sqlstr1="select stmain.inv_no as sbillno,stmain.inv_sr as sinvsr,'Interest' as item ,crmall.new_all as recdamt,stmain.date as sdate,stmain.due_dt as sduedt,crmain.date as brdate,stmain.net_amt as sbillamt ,crmain.net_amt , "
				sqlstr2="ltdays=convert(int,(crmain.date-stmain.due_dt)),stmain.U_INTR_PER as intper from crmain inner join crmall on crmain.tran_cd=crmall.tran_cd "
				sqlstr3="left join stacdet on stacdet.tran_cd=crmall.main_tran and stacdet.acserial=crmall.acseri_all "
				sqlstr4="inner join stmain on stmain.tran_cd=stacdet.tran_cd where  crmain.date BETWEEN  ?brsdate and ?bredate AND CRMAIN.AC_ID = ?MAIN_VW.AC_ID"

				nRet = _curfrmobj.sqlconobj.DataConn([EXE],Company.DbName,sqlstr1+sqlstr2+sqlstr3+sqlstr4,[tmp_sdetail_vw1],;
						"_curfrmobj.nHandle",_curfrmobj.DataSessionId,.f.)
				* Bank Receipt
				sqlstr1="select stmain.inv_no as sbillno,stmain.inv_sr as sinvsr,'Interest' as item ,brmall.new_all as recdamt,stmain.date as sdate,stmain.due_dt as sduedt,brmain.date as brdate,stmain.net_amt as sbillamt ,brmain.net_amt , "
				sqlstr2="ltdays=convert(int,(brmain.date-stmain.due_dt)),stmain.U_INTR_PER as intper from brmain inner join brmall on brmain.tran_cd=brmall.tran_cd "
				sqlstr3="left join stacdet on stacdet.tran_cd=brmall.main_tran and stacdet.acserial=brmall.acseri_all "
				sqlstr4="inner join stmain on stmain.tran_cd=stacdet.tran_cd where  brmain.date BETWEEN  ?brsdate and ?bredate AND BRMAIN.AC_ID = ?MAIN_VW.AC_ID"

*!*					nRet = _curfrmobj.sqlconobj.DataConn([EXE],Company.DbName,"select ac_name,i_rate,i_days from ac_mast where ac_name='"+main_vw.party_nm+"'",[tmp_acmast],;
*!*							"_curfrmobj.nHandle",_curfrmobj.DataSessionId,.f.)
*!*						If nRet <= 0
*!*						RETURN .f.
*!*						ENDIF 

*!*					IF USED("tmp_acmast")
*!*					USE IN tmp_acmast
*!*					ENDIF 

				nRet = _curfrmobj.sqlconobj.DataConn([EXE],Company.DbName,"select it_code from it_mast where it_name='Interest'",[tmp_itcode],;
						"_curfrmobj.nHandle",_curfrmobj.DataSessionId,.f.)
					If nRet <= 0
					RETURN .f.
					ENDIF 
				SELECT tmp_itcode
				ztmpitcode =''
				ztmpitcode=tmp_itcode.it_code

				nRet = _curfrmobj.sqlconobj.DataConn([EXE],Company.DbName,sqlstr1+sqlstr2+sqlstr3+sqlstr4,[tmp_sdetail_vw],;
						"_curfrmobj.nHandle",_curfrmobj.DataSessionId,.f.)



					ztmpintbaseday=365
					IF _curfrmobj.intbaseday>0
					ztmpintbaseday=_curfrmobj.intbaseday
					ENDIF 
			
					If nRet > 0 AND USED('tmp_sdetail_vw')
						IF USED('tmp_sdetail_vw1')
						 SELECT tmp_sdetail_vw
						 append FROM  DBF('tmp_sdetail_vw1')
						 SELECT tmp_sdetail_vw1
						 USE IN tmp_sdetail_vw1
						ENDIF 
						SELECT item_vw
						mitem_no=0
						COUNT TO mitem_no
						SELECT item_vw
						DELETE ALL 
						SELECT tmp_sdetail_vw
						DELETE ALL FOR ltdays <= 0
						GO top					
						SCAN
							mitem_no=mitem_no+1
							SELECT item_vw
	*						LOCATE FOR ALLTRIM(item_vw.sinvsr) = ALLTRIM(tmp_sdetail_vw.sinvsr) AND item_vw.sbdate = tmp_sdetail_vw.sdate ;
									 AND ALLTRIM(item_vw.sbillno) = ALLTRIM(tmp_sdetail_vw.sbillno) AND ALLTRIM(item_vw.item) = ALLTRIM(tmp_sdetail_vw.item)
	*						IF NOT FOUND()
								APPEND BLANK
								replace item_vw.sinvsr WITH tmp_sdetail_vw.sinvsr IN item_vw
								replace item_vw.sbdate WITH tmp_sdetail_vw.sdate IN item_vw
								replace item_vw.sbillno WITH tmp_sdetail_vw.sbillno IN item_vw
								replace item_vw.sbillamt WITH tmp_sdetail_vw.sbillamt IN item_vw				
								replace item_vw.ltdays WITH tmp_sdetail_vw.ltdays IN item_vw
								replace item_vw.item WITH tmp_sdetail_vw.item IN item_vw	
								replace item_vw.brdate WITH tmp_sdetail_vw.brdate IN item_vw
								replace item_vw.recdamt WITH tmp_sdetail_vw.recdamt IN item_vw	
								replace item_vw.sduedt WITH tmp_sdetail_vw.sduedt IN item_vw				
								replace item_vw.intper WITH tmp_sdetail_vw.intper IN item_vw
								replace item_no WITH STR(mitem_no,4),itserial WITH PADL(ALLTRIM(STR(mitem_no)),LEN(itserial),'0') IN item_vw
				*			ENDIF 
						ENDSCAN
						SELECT item_vw
						replace ALL item_vw.entry_ty WITH main_vw.entry_ty IN item_vw
						replace ALL item_vw.date WITH main_vw.date IN item_vw
						replace ALL item_vw.doc_no WITH main_vw.doc_no IN item_vw
						replace ALL item_vw.qty WITH 1 IN item_vw
						replace ALL item_vw.intamt WITH ROUND(((item_vw.recdamt*(item_vw.intper/100))/ztmpintbaseday)*item_vw.ltdays,0) IN item_vw
						replace ALL item_vw.rate WITH item_vw.intamt IN item_vw			
						replace ALL item_vw.gro_amt WITH item_vw.rate*item_vw.qty IN item_vw
						replace ALL it_code WITH ztmpitcode
						_Screen.Activeform.Voupage.Page1.Grditem.refresh
					ELSE
						nRet = 0
					Endif	
			ENDIF
		ENDIF 
		
**************************************** FORWARDING ***************************************************
*--------------------------------------- FOR DEBIT NOTE -----------------------------------------------
	IF main_vw.entry_ty='D2'   AND _screen.ActiveForm.addmode  &&Birendra
		nret=0
		_curfrmobj = _screen.ActiveForm 

		sqlstr1="select stmain.inv_no as sbillno,stmain.inv_sr as sinvsr,'Interest' as item ,stmain.date as sdate,stmain.due_dt as sduedt,stmain.net_amt as sbillamt, "
		sqlstr2="stmain.U_INTR_PER as intper from stmain where stmain.ac_id = ?MAIN_VW.AC_ID and stmain.date = ?main_vw.date and stmain.frwdn = 'YES'"
		sqlstr3=""
		sqlstr4=""

		nRet = _curfrmobj.sqlconobj.DataConn([EXE],Company.DbName,"select it_code from it_mast where it_name='Interest'",[tmp_itcode],;
				"_curfrmobj.nHandle",_curfrmobj.DataSessionId,.f.)
			If nRet <= 0
			RETURN .f.
			ENDIF 
		SELECT tmp_itcode
		ztmpitcode=tmp_itcode.it_code

		nRet = _curfrmobj.sqlconobj.DataConn([EXE],Company.DbName,sqlstr1+sqlstr2,[tmp_sdetail_vw],;
				"_curfrmobj.nHandle",_curfrmobj.DataSessionId,.f.)
	
		If nRet > 0 AND USED('tmp_sdetail_vw')
			SELECT item_vw
			mitem_no=0
			COUNT TO mitem_no
			SELECT tmp_sdetail_vw
			SCAN
				mitem_no=mitem_no+1
				SELECT item_vw
				APPEND BLANK
				replace item_vw.sinvsr WITH tmp_sdetail_vw.sinvsr IN item_vw
				replace item_vw.sbdate WITH tmp_sdetail_vw.sdate IN item_vw
				replace item_vw.sbillno WITH tmp_sdetail_vw.sbillno IN item_vw
				replace item_vw.sbillamt WITH tmp_sdetail_vw.sbillamt IN item_vw				
				replace item_vw.item WITH tmp_sdetail_vw.item IN item_vw	
				replace item_vw.sduedt WITH tmp_sdetail_vw.sduedt IN item_vw
				replace item_no WITH STR(mitem_no,4),itserial WITH PADL(ALLTRIM(STR(mitem_no)),LEN(itserial),'0') IN item_vw
			ENDSCAN
			SELECT item_vw
			replace ALL item_vw.entry_ty WITH main_vw.entry_ty IN item_vw
			replace ALL item_vw.date WITH main_vw.date IN item_vw
			replace ALL item_vw.doc_no WITH main_vw.doc_no IN item_vw
			replace ALL item_vw.qty WITH 1 IN item_vw
			replace ALL it_code WITH ztmpitcode
			
			_Screen.Activeform.Voupage.Page1.Grditem.refresh
		ELSE
			nRet = 0
		Endif	

	ENDIF

***credit note late payment***

	IF INLIST(MAIN_VW.ENTRY_tY,'C3') AND _screen.ActiveForm.addmode 
			* Check for Interest Pay Party :Start
			_curfrmobj = _screen.ActiveForm 
			nRet = _curfrmobj.sqlconobj.DataConn([EXE],Company.DbName,"select ac_name,i_rate,i_days,intpay from ac_mast where ac_name='"+main_vw.party_nm+"'",[tmp_acmast],;
					"_curfrmobj.nHandle",_curfrmobj.DataSessionId,.f.)
				If nRet <= 0
				RETURN .f.
				ENDIF 
			IF tmp_acmast.intpay

				SELECT tmp_acmast
				_curfrmobj.intbaseday=tmp_acmast.i_days

				IF USED("tmp_acmast")
				USE IN tmp_acmast
				ENDIF 

			* Check for Interest Pay Party :End

		DO FORM frmgetdate WITH DATE()
		nret=0
		mAlias = Alias()
		_curfrmobj = _screen.ActiveForm 
		* Cash payment
		sqlstr1="select ptmain.u_pinvno as sbillno,'Interest' as item ,cpmall.new_all as recdamt,ptmain.date as sdate,ptmain.due_dt as sduedt,cpmain.date as brdate,ptmain.net_amt as sbillamt ,cpmain.net_amt , "
		sqlstr2="ltdays=convert(int,(cpmain.date-ptmain.due_dt)),ptmain.U_INTR_PER as intper from cpmain inner join cpmall on cpmain.tran_cd=cpmall.tran_cd "
		sqlstr3="left join ptacdet on ptacdet.tran_cd=cpmall.main_tran and ptacdet.acserial=cpmall.acseri_all "
		sqlstr4="inner join ptmain on ptmain.tran_cd=ptacdet.tran_cd where  cpmain.date BETWEEN  ?brsdate and ?bredate AND CPMAIN.AC_ID = ?MAIN_VW.AC_ID"

		nRet = _curfrmobj.sqlconobj.DataConn([EXE],Company.DbName,sqlstr1+sqlstr2+sqlstr3+sqlstr4,[tmp_sdetail_vw1],;
				"_curfrmobj.nHandle",_curfrmobj.DataSessionId,.f.)
		* Bank Payment

		sqlstr1="select ptmain.u_pinvno as sbillno,'Interest' as item ,bpmall.new_all as recdamt,ptmain.date as sdate,ptmain.due_dt as sduedt,bpmain.date as brdate,ptmain.net_amt as sbillamt ,bpmain.net_amt , "
		sqlstr2="ltdays=convert(int,(bpmain.date-ptmain.due_dt)),ptmain.U_INTR_PER as intper from bpmain inner join bpmall on bpmain.tran_cd=bpmall.tran_cd "
		sqlstr3="left join ptacdet on ptacdet.tran_cd=bpmall.main_tran and ptacdet.acserial=bpmall.acseri_all "
		sqlstr4="inner join ptmain on ptmain.tran_cd=ptacdet.tran_cd where bpmain.date BETWEEN  ?brsdate and ?bredate AND BpMAIN.AC_ID = ?MAIN_VW.AC_ID"

*!*			nRet = _curfrmobj.sqlconobj.DataConn([EXE],Company.DbName,"select ac_name,i_rate,i_days from ac_mast where ac_name='"+main_vw.party_nm+"'",[tmp_acmast],;
*!*					"_curfrmobj.nHandle",_curfrmobj.DataSessionId,.f.)
*!*				If nRet <= 0
*!*				RETURN .f.
*!*				ENDIF 
*!*			SELECT tmp_acmast
*!*			_curfrmobj.intbaseday=tmp_acmast.i_days

*!*			IF USED("tmp_acmast")
*!*			USE IN tmp_acmast
*!*			ENDIF 

		nRet = _curfrmobj.sqlconobj.DataConn([EXE],Company.DbName,"select it_code from it_mast where it_name='Interest'",[tmp_itcode],;
				"_curfrmobj.nHandle",_curfrmobj.DataSessionId,.f.)
			If nRet <= 0
			RETURN .f.
			ENDIF 

			SELECT tmp_itcode
			ztmpitcode =''
			ztmpitcode=tmp_itcode.it_code

		nRet = _curfrmobj.sqlconobj.DataConn([EXE],Company.DbName,sqlstr1+sqlstr2+sqlstr3+sqlstr4,[tmp_sdetail_vw],;
				"_curfrmobj.nHandle",_curfrmobj.DataSessionId,.f.)

				ztmpintbaseday=365
				IF _curfrmobj.intbaseday>0
				ztmpintbaseday=_curfrmobj.intbaseday
				ENDIF 


			If nRet > 0 AND USED('tmp_sdetail_vw')
				IF USED('tmp_sdetail_vw1')
				 SELECT tmp_sdetail_vw
				 append FROM  DBF('tmp_sdetail_vw1')
				 SELECT tmp_sdetail_vw1
				 USE IN tmp_sdetail_vw1
				ENDIF 

				SELECT item_vw
				mitem_no=0
				COUNT TO mitem_no
				SELECT item_vw
				DELETE ALL 
				SELECT tmp_sdetail_vw
				DELETE ALL FOR ltdays <= 0
				GO top
				SCAN
					mitem_no=mitem_no+1
					SELECT item_vw
*					LOCATE FOR ALLTRIM(item_vw.sinvsr) = ALLTRIM(tmp_sdetail_vw.sinvsr) AND item_vw.sbdate = tmp_sdetail_vw.sdate ;
							 AND ALLTRIM(item_vw.sbillno) = ALLTRIM(tmp_sdetail_vw.sbillno) AND ALLTRIM(item_vw.item) = ALLTRIM(tmp_sdetail_vw.item)
*					IF NOT FOUND()
						APPEND BLANK
						replace item_vw.sbdate WITH tmp_sdetail_vw.sdate IN item_vw
						replace item_vw.sbillno WITH tmp_sdetail_vw.sbillno IN item_vw
						replace item_vw.sbillamt WITH tmp_sdetail_vw.sbillamt IN item_vw				
						replace item_vw.ltdays WITH tmp_sdetail_vw.ltdays IN item_vw
						replace item_vw.item WITH tmp_sdetail_vw.item IN item_vw	
						replace item_vw.brdate WITH tmp_sdetail_vw.brdate IN item_vw
						replace item_vw.recdamt WITH tmp_sdetail_vw.recdamt IN item_vw	
						replace item_vw.sduedt WITH tmp_sdetail_vw.sduedt IN item_vw				
						replace item_vw.intper WITH tmp_sdetail_vw.intper IN item_vw
						replace item_no WITH STR(mitem_no,4),itserial WITH PADL(ALLTRIM(STR(mitem_no)),LEN(itserial),'0') IN item_vw
*					ENDIF 
				ENDSCAN
				SELECT item_vw
				replace ALL item_vw.entry_ty WITH main_vw.entry_ty IN item_vw
				replace ALL item_vw.date WITH main_vw.date IN item_vw
				replace ALL item_vw.doc_no WITH main_vw.doc_no IN item_vw
				replace ALL item_vw.qty WITH 1 IN item_vw
				replace ALL item_vw.intamt WITH ROUND(((item_vw.recdamt*(item_vw.intper/100))/ztmpintbaseday)*item_vw.ltdays,0) IN item_vw
				replace ALL item_vw.rate WITH item_vw.intamt IN item_vw			
				replace ALL item_vw.gro_amt WITH item_vw.rate*item_vw.qty IN item_vw
				replace ALL it_code WITH ztmpitcode

				_Screen.Activeform.Voupage.Page1.Grditem.refresh
			ELSE
				nRet = 0
			ENDIF
		ENDIF 	
	ENDIF
*end credit note late payment

***for forwarding credit note***

	IF main_vw.entry_ty='C2'   AND _screen.ActiveForm.addmode  &&Birendra

			nret=0
			_curfrmobj = _screen.ActiveForm 

			sqlstr1="select ptmain.u_pinvno as sbillno,'Interest' as item ,ptmain.date as sdate,ptmain.due_dt as sduedt,ptmain.net_amt as sbillamt, "
			sqlstr2="ptmain.U_INTR_PER as intper from ptmain where ptmain.ac_id = ?MAIN_VW.AC_ID and ptmain.date = ?main_vw.date and ptmain.frwdn = 'YES'"
			sqlstr3=""
			sqlstr4=""

			nRet = _curfrmobj.sqlconobj.DataConn([EXE],Company.DbName,"select it_code from it_mast where it_name='Interest'",[tmp_itcode],;
					"_curfrmobj.nHandle",_curfrmobj.DataSessionId,.f.)
				If nRet <= 0
				RETURN .f.
				ENDIF 

			SELECT tmp_itcode
			ztmpitcode =''
			ztmpitcode=tmp_itcode.it_code

		
			nRet = _curfrmobj.sqlconobj.DataConn([EXE],Company.DbName,sqlstr1+sqlstr2,[tmp_sdetail_vw],;
					"_curfrmobj.nHandle",_curfrmobj.DataSessionId,.f.)
		
			If nRet > 0 AND USED('tmp_sdetail_vw')
				SELECT item_vw
				mitem_no=0
				COUNT TO mitem_no
				SELECT tmp_sdetail_vw

			SCAN
				mitem_no=mitem_no+1
				SELECT item_vw
				APPEND BLANK
				replace item_vw.sbdate WITH tmp_sdetail_vw.sdate IN item_vw
				replace item_vw.sbillno WITH tmp_sdetail_vw.sbillno IN item_vw
				replace item_vw.sbillamt WITH tmp_sdetail_vw.sbillamt IN item_vw				
				replace item_vw.item WITH tmp_sdetail_vw.item IN item_vw	
				replace item_vw.sduedt WITH tmp_sdetail_vw.sduedt IN item_vw
				replace item_no WITH STR(mitem_no,4),itserial WITH PADL(ALLTRIM(STR(mitem_no)),LEN(itserial),'0') IN item_vw
			ENDSCAN
			SELECT item_vw
			replace ALL item_vw.entry_ty WITH main_vw.entry_ty IN item_vw
			replace ALL item_vw.date WITH main_vw.date IN item_vw
			replace ALL item_vw.doc_no WITH main_vw.doc_no IN item_vw
			replace ALL item_vw.qty WITH 1 IN item_vw
			replace ALL item_vw.it_code WITH ztmpitcode IN item_vw
				_Screen.Activeform.Voupage.Page1.Grditem.refresh
			ELSE
				nRet = 0
			Endif	
	ENDIF

*!*	*end forwarding credit note

	
ENDIF 
ENDIF 


*-------------------------------------------------------------------------------------------------------------


*!*	* -----------------------------------------------------------------------------------------------------------------

*!*	*credit note discount
*!*		IF MAIN_VW.DEPT = 'DISCOUNT' AND main_vw.entry_ty='CI' 
*!*			WITH _screen.ActiveForm 
*!*				tot_grd_col=.voupage.page1.grditem.columncount	
*!*					FOR i = 1 TO tot_grd_col
*!*				DO CASE 
*!*			       CASE .voupage.page1.grditem.columns(i).header1.caption='Rate'
*!*						.voupage.page1.grditem.columns(i).visible=.F.	
*!*		
*!*				   CASE .voupage.page1.grditem.columns(i).header1.caption='Quantity'
*!*				 		.voupage.page1.grditem.columns(i).visible=.t.
*!*		
*!*		           CASE .voupage.page1.grditem.columns(i).header1.caption='Purchase Bill No.'
*!*				  		.voupage.page1.grditem.columns(i).width=80
*!*		
*!*				   CASE .voupage.page1.grditem.columns(i).header1.caption='Item Name'
*!*				 		.voupage.page1.grditem.columns(i).width=120
*!*			
*!*				   CASE .voupage.page1.grditem.columns(i).header1.caption='Payment Paid Date'
*!*				 		.voupage.page1.grditem.columns(i).visible=.f.
*!*		
*!*				   CASE .voupage.page1.grditem.columns(i).header1.caption='Paid Amount'
*!*				 		.voupage.page1.grditem.columns(i).visible=.f.
*!*				 		
*!*				   CASE .voupage.page1.grditem.columns(i).header1.caption='Late Days'
*!*				 		.voupage.page1.grditem.columns(i).visible=.f.

*!*				   CASE .voupage.page1.grditem.columns(i).header1.caption='Interest %'
*!*				 		.voupage.page1.grditem.columns(i).visible=.f.

*!*				   CASE .voupage.page1.grditem.columns(i).header1.caption='Interest Amount'
*!*				 		.voupage.page1.grditem.columns(i).visible=.f.
*!*		
*!*					 	.voupage.page1.grditem.refresh	
*!*		
*!*				   ENDCASE
*!*				ENDFOR
*!*			ENDWITH
*!*		ENDIF

*!*	*end credit note discount

*!*	*----------------------------------------------------------------------------------------------------------------------

*!*	*credit note rate difference

*!*		IF MAIN_VW.DEPT = 'RATE DIFFERENCE' AND main_vw.entry_ty='CI' 
*!*				WITH _screen.ActiveForm 
*!*					tot_grd_col=.voupage.page1.grditem.columncount	
*!*					FOR i = 1 TO tot_grd_col
*!*					DO CASE 
*!*				       CASE .voupage.page1.grditem.columns(i).header1.caption='Rate'
*!*							.voupage.page1.grditem.columns(i).visible=.F.	
*!*			
*!*					   CASE .voupage.page1.grditem.columns(i).header1.caption='Quantity'
*!*					 		.voupage.page1.grditem.columns(i).visible=.T.	
*!*			
*!*			           CASE .voupage.page1.grditem.columns(i).header1.caption='Purchase Bill No.'
*!*					  		.voupage.page1.grditem.columns(i).width=70
*!*			
*!*					   CASE .voupage.page1.grditem.columns(i).header1.caption='Item Name'
*!*					 		.voupage.page1.grditem.columns(i).width=120
*!*				
*!*					   CASE .voupage.page1.grditem.columns(i).header1.caption='Payment Paid Date'
*!*					 		.voupage.page1.grditem.columns(i).visible=.f.
*!*			
*!*					   CASE .voupage.page1.grditem.columns(i).header1.caption='Paid Amount'
*!*					 		.voupage.page1.grditem.columns(i).visible=.f.
*!*					 		
*!*					   CASE .voupage.page1.grditem.columns(i).header1.caption='Late Days'
*!*					 		.voupage.page1.grditem.columns(i).visible=.f.
*!*			
*!*					   CASE .voupage.page1.grditem.columns(i).header1.caption='Interest %'
*!*					 		.voupage.page1.grditem.columns(i).visible=.f.
*!*			
*!*					   CASE .voupage.page1.grditem.columns(i).header1.caption='Interest Amount'
*!*					 		.voupage.page1.grditem.columns(i).visible=.f.

*!*						 	.voupage.page1.grditem.refresh	
*!*			    	ENDCASE
*!*				ENDFOR
*!*			ENDWITH
*!*		ENDIF

*!*	*end credit note rate difference

*!*	*-------------------------------------------------------------------------------------------------------------------

*!*	*credit note weight difference

*!*		IF MAIN_VW.DEPT = 'WEIGHT DIFFERENCE'  AND main_vw.entry_ty='CI' 

*!*			WITH _screen.ActiveForm 
*!*				tot_grd_col=.voupage.page1.grditem.columncount	
*!*				FOR i = 1 TO tot_grd_col
*!*		
*!*				DO CASE 
*!*			       CASE .voupage.page1.grditem.columns(i).header1.caption='Rate'
*!*						.voupage.page1.grditem.columns(i).visible=.F.	
*!*		
*!*				   CASE .voupage.page1.grditem.columns(i).header1.caption='Quantity'
*!*				 		.voupage.page1.grditem.columns(i).visible=.T.	
*!*		
*!*		           CASE .voupage.page1.grditem.columns(i).header1.caption='Purchase Bill No.'
*!*				  		.voupage.page1.grditem.columns(i).width=80
*!*		
*!*				   CASE .voupage.page1.grditem.columns(i).header1.caption='Item Name'
*!*				 		.voupage.page1.grditem.columns(i).width=120
*!*			
*!*				   CASE .voupage.page1.grditem.columns(i).header1.caption='Payment Paid Date'
*!*				 		.voupage.page1.grditem.columns(i).visible=.f.
*!*		
*!*				   CASE .voupage.page1.grditem.columns(i).header1.caption='Paid Amount'
*!*				 		.voupage.page1.grditem.columns(i).visible=.f.
*!*				 		
*!*				   CASE .voupage.page1.grditem.columns(i).header1.caption='Late Days'
*!*				 		.voupage.page1.grditem.columns(i).visible=.f.
*!*		
*!*				   CASE .voupage.page1.grditem.columns(i).header1.caption='Interest %'
*!*				 		.voupage.page1.grditem.columns(i).visible=.f.
*!*		
*!*				   CASE .voupage.page1.grditem.columns(i).header1.caption='Interest Amount'
*!*				 		.voupage.page1.grditem.columns(i).visible=.f.

*!*					 	.voupage.page1.grditem.refresh	
*!*		
*!*		    	ENDCASE
*!*		
*!*			ENDFOR
*!*		ENDWITH
*!*		ENDIF

*!*	**end credit note weight difference
*!*	endif
*!*	SELECT (mAlias)         




*!*	**--** (Start) Added patch for "Grace Period Checking for Sales Transaction" = (12 01 2010)

*!*	*!*	If (Type('_screen.activeform.PCVTYPE')='C' And Used('MAIN_VW'))
*!*	*!*		If MAIN_VW.ENTRY_TY='ST'
*!*	*!*			_curvouobj = _Screen.ActiveForm
*!*	*!*			sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)
*!*	*!*			nHandle=0
*!*	*!*			mAllow = .F.
*!*	*!*			sq1="select u_grpd from Ac_Mast where Ac_name='"+ MAIN_VW.party_nm +"'"
*!*	*!*			nRetval = sqlconobj.dataconn([EXE],company.dbname,sq1,"AC","nHandle",_Screen.ActiveForm.DataSessionId)
*!*	*!*			If EMPTY(nRetval)
*!*	*!*				Return .F.
*!*	*!*			Endif
*!*	*!*			If Used("AC")
*!*	*!*				Select AC
*!*	*!*				Go Top
*!*	*!*				If Reccount() > 0
*!*	*!*					If !empty(AC.u_grpd) Then
*!*	*!*						Messagebox(" "+ Alltrim(MAIN_VW.party_nm) +" has "+Alltrim(AC.U_GRPD)+" days Grace Period.",64+0,"Grace Period Alert")
*!*	*!*					Endif
*!*	*!*				Endif
*!*	*!*			ENDIF
*!*	*!*		ENDIF
*!*	*!*	ENDIF

*!*	**--** (End)