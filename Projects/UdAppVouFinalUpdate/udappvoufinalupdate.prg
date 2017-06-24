_curvouobj = _Screen.ActiveForm
etsql_con = 1

&& Commented by Shrikant S. on 29/09/2016 for GST		&& Start
*!*	If _curvouobj.itempage Or Inlist(main_vw.entry_ty,"RR","RP","GI","GR","HI","HR") 	&& Changed by Shrikant S. on 29/09/2010 for TKT-4021

*!*	*!*	If _curvouobj.itempage Or Inlist(main_vw.entry_ty,"RR","RP","GI","GR","HI","HR","J2","VR") 	&& Changed by Shrikant S. on 29/09/2010 for TKT-4021	&& Changed by Sachin N. S. on 22/09/2012 for Bug-5164
*!*	*!*		If _curvouobj.addmode Or _curvouobj.editmode
*!*	*!*			If Used('Gen_SrNo_Vw')
*!*	*!*				Select gen_srno_vw
*!*	*!*				Replace All tran_cd With main_vw.tran_cd,entry_ty With main_vw.entry_ty,;
*!*	*!*					DATE With main_vw.Date,compid With main_vw.compid,l_yn With main_vw.l_yn In gen_srno_vw
*!*	*!*	*npgno,cit_code,cware,cgroup,cchapno
*!*	*!*	*itserial

*!*	*!*				etsql_str  = "Select * from Gen_SrNo Where l_yn = ?main_vw.l_yn and Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
*!*	*!*				etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[TmpEt_Vw],;
*!*	*!*					"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
*!*	*!*				If etsql_con > 0 And Used('TmpEt_Vw')
*!*	*!*					Select tmpet_vw
*!*	*!*					Scan
*!*	*!*						metdele = .F.
*!*	*!*						Select item_vw
*!*	*!*						If !Seek(main_vw.entry_ty+Dtos(main_vw.Date)+main_vw.doc_no+tmpet_vw.itserial,'Item_vw','Eddits')
*!*	*!*							metdele = .T.
*!*	*!*						Else
*!*	*!*	*!*							If TmpEt_Vw.Cit_code # Item_vw.it_code &&Or TmpEt_Vw.Cware # Item_vw.Ware_nm  	&& Commented by Shrikant S. on 29/09/2010 for TKT-4021
*!*	*!*	*!*							If tmpet_vw.cit_code # item_vw.it_code  And !Inlist(gen_srno_vw.ctype,"RGAPART2","RGCPART2","PLASRNO")	&& Added by Shrikant S. on 29/09/2010 for TKT-4021
*!*	*!*	*						If tmpet_vw.cit_code # item_vw.it_code  And !Inlist(gen_srno_vw.ctype,"RGAPART2","RGCPART2","PLASRNO","SERVTXSRNO")	&& Added by Shrikant S. on 29/09/2010 for TKT-4021		&& Changed By Sachin N. S. on 18/09/2012 for Bug-5164
*!*	*!*	*Birendra: Bug-19986 on 11/10/2013 :Modified with below one:FORM402SRNO
*!*	*!*	&&						If tmpet_vw.cit_code # item_vw.it_code  And !Inlist(gen_srno_vw.ctype,"RGAPART2","RGCPART2","PLASRNO","SERVTXSRNO","FORM402SRNO") Commented by sandeep for bug-21327 on 08-02-2014
*!*	*!*							If tmpet_vw.cit_code # item_vw.it_code  And !Inlist(gen_srno_vw.ctype,"RGAPART2","RGCPART2","PLASRNO","SERVTXSRNO","FORM402SRNO","FORM403SRNO") &&Added by sandeep for bug-21327 on 08-02-2014

*!*	*!*								metdele = .T.
*!*	*!*							Endif
*!*	*!*	&& Added by Shrikant S. on 01 Feb, 2010  ------------Start
*!*	*!*							If Seek(main_vw.entry_ty+Dtos(main_vw.Date)+main_vw.doc_no+tmpet_vw.itserial,'Item_vw','Eddits')
*!*	*!*								If tmpet_vw.cit_code = item_vw.it_code Or Empty(item_vw.u_pageno)&&Or TmpEt_Vw.Cware # Item_vw.Ware_nm
*!*	*!*									metdele = .T.
*!*	*!*								Endif
*!*	*!*							Endif
*!*	*!*	&& Added by Shrikant S. on 01 Feb, 2010  ------------End
*!*	*!*	&& Added by Shrikant S. on 29/09/2010 for TKT-4021
*!*	*!*	*!*							If	Inlist(gen_srno_vw.ctype,"RGAPART2","RGCPART2","PLASRNO")		&& 280910
*!*	*!*	*						If	Inlist(gen_srno_vw.ctype,"RGAPART2","RGCPART2","PLASRNO","SERVTXSRNO")		&& 280910	&& Changed By Sachin N. S. on 18/09/2012 for Bug-5164
*!*	*!*	*Birendra: Bug-19986 on 11/10/2013 :Modified with below one:FORM402SRNO
*!*	*!*	&&						If	Inlist(gen_srno_vw.ctype,"RGAPART2","RGCPART2","PLASRNO","SERVTXSRNO","FORM402SRNO")  Commented by sandeep for  bug-21327 on 08-02-2014
*!*	*!*							If	Inlist(gen_srno_vw.ctype,"RGAPART2","RGCPART2","PLASRNO","SERVTXSRNO","FORM402SRNO","FORM403SRNO")&&Added by sandeep for bug-21327 on 08-02-2014

*!*	*!*								If	(Type('main_vw.u_rg23no')='C' Or Type('main_vw.u_rg23cno')='C')
*!*	*!*									If	Empty(main_vw.u_rg23no) And Empty(main_vw.u_rg23cno)
*!*	*!*										metdele =.T.
*!*	*!*									Endif
*!*	*!*								Endif
*!*	*!*								If	Type('main_vw.u_plasr')='C'
*!*	*!*									If Empty(main_vw.u_plasr)
*!*	*!*										metdele=.T.
*!*	*!*									Endif
*!*	*!*								Endif
*!*	*!*	****** Added By Sachin N. S. on 18/09/2012 for Bug-5164 ****** Start
*!*	*!*								If	Type('main_vw.SERVTXSRNO')='C'
*!*	*!*									If Empty(main_vw.SERVTXSRNO)
*!*	*!*										metdele=.T.
*!*	*!*									Endif
*!*	*!*								Endif
*!*	*!*	****** Added By Sachin N. S. on 18/09/2012 for Bug-5164 ****** End
*!*	*!*	*Birendra: Bug-19986 on 11/10/2013 : Start:
*!*	*!*								If	Type('main_vw.U_402srno')='C'
*!*	*!*									If Empty(main_vw.U_402srno)
*!*	*!*										metdele=.T.
*!*	*!*									Endif
*!*	*!*								Else
*!*	*!*									If	Type('lmc_vw.U_402srno')='C'
*!*	*!*										If Empty(lmc_vw.U_402srno)
*!*	*!*											metdele=.T.
*!*	*!*										Endif
*!*	*!*									Endif
*!*	*!*								Endif
*!*	*!*	*Birendra: Bug-19986 on 11/10/2013 : End:
*!*	*!*	&&Added by sandeep for bug-21327 on 08-02-2014	--->Start:
*!*	*!*								If	Type('main_vw.U_403srno')='C'
*!*	*!*									If Empty(main_vw.U_403srno)
*!*	*!*										metdele=.T.
*!*	*!*									Endif
*!*	*!*								Else
*!*	*!*									If	Type('lmc_vw.U_403srno')='C'
*!*	*!*										If Empty(lmc_vw.U_403srno)
*!*	*!*											metdele=.T.
*!*	*!*										Endif
*!*	*!*									Endif
*!*	*!*								Endif
*!*	*!*	&&Added by sandeep for bug-21327 on 08-02-2014	<----End


*!*	*!*							Endif
*!*	*!*	&& Added by Shrikant S. on 29/09/2010 for TKT-4021
*!*	*!*						Endif
*!*	*!*						If metdele = .T. And etsql_con > 0
*!*	*!*							etsql_str = _curvouobj.sqlconobj.gendelete("Gen_SrNo","Tran_cd = ?Main_vw.Tran_cd And ;
*!*	*!*									Entry_ty = ?Main_vw.Entry_ty And Itserial = ?TmpEt_Vw.itserial")
*!*	*!*							etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],;
*!*	*!*								"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
*!*	*!*						Else
*!*	*!*							Select gen_srno_vw
*!*	*!*							If Seek(tmpet_vw.itserial,'Gen_SrNo_Vw','ItSerial') And ;
*!*	*!*									gen_srno_vw.cit_code = tmpet_vw.cit_code And  ;
*!*	*!*									gen_srno_vw.cgroup = tmpet_vw.cgroup And gen_srno_vw.cchapno = tmpet_vw.cchapno		&&Gen_SrNo_Vw.Cware = TmpEt_Vw.Cware AND
*!*	*!*								Replace npgno With '' In gen_srno_vw
*!*	*!*							Endif
*!*	*!*						Endif
*!*	*!*						Select tmpet_vw
*!*	*!*					Endscan
*!*	*!*				Else
*!*	*!*					etsql_con = 0
*!*	*!*				Endif

*!*	*!*				Select gen_srno_vw
*!*	*!*				Scan
*!*	*!*					mrecno=Recno()			&& Added by Shrikant S. on 29/09/2010 for TKT-4021
*!*	*!*					Select item_vw
*!*	*!*	*!*					If !Seek(main_vw.entry_ty+Dtos(main_vw.Date)+main_vw.doc_no+gen_srno_vw.itserial,'Item_vw','Eddits') And !Inlist(gen_srno_vw.ctype,"RGAPART2","RGCPART2","PLASRNO") && Changed by Shrikant S. on 29/09/2010 for TKT-4021
*!*	*!*	*				If !Seek(main_vw.entry_ty+Dtos(main_vw.Date)+main_vw.doc_no+gen_srno_vw.itserial,'Item_vw','Eddits') And !Inlist(gen_srno_vw.ctype,"RGAPART2","RGCPART2","PLASRNO","SERVTXSRNO") && Changed by Shrikant S. on 29/09/2010 for TKT-4021	&& Changed by Sachin N. S. on 18/09/2012 for Bug-5164
*!*	*!*	*Birendra: Bug-19986 on 11/10/2013 :Modified with below one:FORM402SRNO
*!*	*!*	&&				If !Seek(main_vw.entry_ty+Dtos(main_vw.Date)+main_vw.doc_no+gen_srno_vw.itserial,'Item_vw','Eddits') And !Inlist(gen_srno_vw.ctype,"RGAPART2","RGCPART2","PLASRNO","SERVTXSRNO","FORM402SRNO")  Commented by sandeep for  bug-21327 on 08-02-2014
*!*	*!*					If !Seek(main_vw.entry_ty+Dtos(main_vw.Date)+main_vw.doc_no+gen_srno_vw.itserial,'Item_vw','Eddits') And !Inlist(gen_srno_vw.ctype,"RGAPART2","RGCPART2","PLASRNO","SERVTXSRNO","FORM402SRNO","FORM403SRNO") &&added by sandeep for  bug-21327 on 08-02-2014

*!*	*!*						Replace npgno With '' In gen_srno_vw
*!*	*!*					Else
*!*	*!*	*!*						If Gen_SrNo_Vw.Cit_code # Item_vw.it_code &&Or Gen_SrNo_Vw.Cware # Item_vw.Ware_nm
*!*	*!*	*!*						If gen_srno_vw.cit_code # item_vw.it_code And !Inlist(gen_srno_vw.ctype,"RGAPART2","RGCPART2","PLASRNO") && Changed by Shrikant S. on 29/09/2010 for TKT-4021
*!*	*!*	*					If gen_srno_vw.cit_code # item_vw.it_code And !Inlist(gen_srno_vw.ctype,"RGAPART2","RGCPART2","PLASRNO","SERVTXSRNO") && Changed by Shrikant S. on 29/09/2010 for TKT-4021		&& Changed by Sachin N. S. on 18/09/2012 for Bug-5164
*!*	*!*	*Birendra: Bug-19986 on 11/10/2013 :Modified with below one:FORM402SRNO
*!*	*!*	&&					If gen_srno_vw.cit_code # item_vw.it_code And !Inlist(gen_srno_vw.ctype,"RGAPART2","RGCPART2","PLASRNO","SERVTXSRNO","FORM402SRNO") &&Commented by sandeep for  bug-21327 on 08-02-2014
*!*	*!*						If gen_srno_vw.cit_code # item_vw.it_code And !Inlist(gen_srno_vw.ctype,"RGAPART2","RGCPART2","PLASRNO","SERVTXSRNO","FORM402SRNO","FORM403SRNO") &&added by sandeep for  bug-21327 on 08-02-2014
*!*	*!*							Replace npgno With '' In gen_srno_vw
*!*	*!*						Endif
*!*	*!*	&&Added by Shrikant S. on 06 Feb, 2010 -- Start
*!*	*!*						If Seek(main_vw.entry_ty+Dtos(main_vw.Date)+main_vw.doc_no+gen_srno_vw.itserial,'Item_vw','Eddits')
*!*	*!*							If (gen_srno_vw.cit_code = item_vw.it_code And Empty(item_vw.u_pageno))
*!*	*!*								Replace npgno With '' In gen_srno_vw
*!*	*!*							Else
*!*	*!*								Replace npgno With item_vw.u_pageno In gen_srno_vw		&& Added by Shrikant S. on 29/09/2010 for TKT-4021
*!*	*!*							Endif
*!*	*!*						Endif
*!*	*!*	&&Added by Shrikant S. on 06 Feb, 2010 -- Start
*!*	*!*	&& Added by Shrikant S. on 29/09/2010 for TKT-4021		--Start
*!*	*!*	*!*						If	Inlist(gen_srno_vw.ctype,"RGAPART2","RGCPART2","PLASRNO")
*!*	*!*	*					If	Inlist(gen_srno_vw.ctype,"RGAPART2","RGCPART2","PLASRNO","SERVTXSRNO")		&& Changed by Sachin N. S. on 18/09/2012 for Bug-5164
*!*	*!*	*Birendra: Bug-19986 on 11/10/2013 :Modified with below one:FORM402SRNO
*!*	*!*	&&					If	Inlist(gen_srno_vw.ctype,"RGAPART2","RGCPART2","PLASRNO","SERVTXSRNO","FORM402SRNO") &&Commented by sandeep for  bug-21327 on 08-02-2014
*!*	*!*						If	Inlist(gen_srno_vw.ctype,"RGAPART2","RGCPART2","PLASRNO","SERVTXSRNO","FORM402SRNO","FORM403SRNO")	&&added by sandeep for  bug-21327 on 08-02-2014

*!*	*!*	*!*							If	(Type('main_vw.u_rg23no')='C' Or Type('main_vw.u_rg23cno')='C')
*!*	*!*							If	(Type('main_vw.u_rg23no')='C' Or Type('main_vw.u_rg23cno')='C') And Inlist(gen_srno_vw.ctype,"RGAPART2","RGCPART2")	&& Changed by Sachin N. S. on 18/09/2012 for Bug-5164
*!*	*!*								If	Empty(main_vw.u_rg23no) And Empty(main_vw.u_rg23cno)
*!*	*!*									Replace npgno With '' In gen_srno_vw
*!*	*!*								Endif
*!*	*!*							Endif
*!*	*!*	*!*							If	Type('main_vw.u_plasr')='C' And !(Type('main_vw.u_rg23no')='C' Or Type('main_vw.u_rg23cno')='C')
*!*	*!*							If	Type('main_vw.u_plasr')='C' And !(Type('main_vw.u_rg23no')='C' Or Type('main_vw.u_rg23cno')='C') And gen_srno_vw.ctype='PLASRNO'	&& Changed by Sachin N. S. on 18/09/2012 for Bug-5164
*!*	*!*								If Empty(main_vw.u_plasr)
*!*	*!*									Replace npgno With '' In gen_srno_vw
*!*	*!*								Endif
*!*	*!*							Endif
*!*	*!*	***** Added by Sachin N. S. on 18/09/2012 for Bug-5164 ***** Start
*!*	*!*							If	Type('main_vw.SERVTXSRNO')='C'	And gen_srno_vw.ctype='SERVTXSRNO'
*!*	*!*								If Empty(main_vw.SERVTXSRNO)
*!*	*!*									Replace npgno With '' In gen_srno_vw
*!*	*!*								Endif
*!*	*!*							Endif
*!*	*!*	***** Added by Sachin N. S. on 18/09/2012 for Bug-5164 ***** End
*!*	*!*	*Birendra: Bug-19986 on 11/10/2013 : Start:
*!*	*!*							If	Type('main_vw.U_402srno')='C' And gen_srno_vw.ctype="FORM402SRNO"
*!*	*!*								If Empty(main_vw.U_402srno)
*!*	*!*									Replace npgno With '' In gen_srno_vw
*!*	*!*								Endif
*!*	*!*							Else
*!*	*!*								If	Type('lmc_vw.U_402srno')='C' And gen_srno_vw.ctype="FORM402SRNO"
*!*	*!*									If Empty(lmc_vw.U_402srno)
*!*	*!*										Replace npgno With '' In gen_srno_vw
*!*	*!*									Endif
*!*	*!*								Endif
*!*	*!*							Endif
*!*	*!*	*Birendra: Bug-19986 on 11/10/2013 : End:
*!*	*!*	&&added by sandeep for  bug-21327 on 08-02-2014	-->Start
*!*	*!*							If	Type('main_vw.U_403srno')='C' And gen_srno_vw.ctype="FORM403SRNO"
*!*	*!*								If Empty(main_vw.U_403srno)
*!*	*!*									Replace npgno With '' In gen_srno_vw
*!*	*!*								Endif
*!*	*!*							Else
*!*	*!*								If	Type('lmc_vw.U_403srno')='C' And gen_srno_vw.ctype="FORM403SRNO"
*!*	*!*									If Empty(lmc_vw.U_403srno)
*!*	*!*										Replace npgno With '' In gen_srno_vw
*!*	*!*									Endif
*!*	*!*								Endif
*!*	*!*							Endif
*!*	*!*	&&added by sandeep for  bug-21327 on 08-02-2014	<--End


*!*	*!*						Endif
*!*	*!*					Endif
*!*	*!*	&& Added by Shrikant S. on 29/09/2010 for TKT-4021		--End
*!*	*!*					If etsql_con > 0 And !Empty(gen_srno_vw.npgno)
*!*	*!*	&& Added by Shrikant S. on 29/09/2010 for TKT-4021		--Start
*!*	*!*						Do Case
*!*	*!*						Case gen_srno_vw.ctype="RGAPART2"
*!*	*!*							Do While (dup_no("U_rg23no",gen_srno_vw.npgno,"Stkl_vw_Main",.T.,"_curvouobj.nHandle")=.F.)
*!*	*!*								newsrno=gen_no("U_rg23no","Stkl_vw_Main",.T.,"_curvouobj.nHandle")
*!*	*!*								Replace npgno With newsrno In gen_srno_vw
*!*	*!*								Replace u_rg23no With newsrno In main_vw
*!*	*!*							Enddo
*!*	*!*						Case gen_srno_vw.ctype="RGCPART2"
*!*	*!*	*!*							Do While (dup_no("U_rg23cno",main_vw.u_rg23cno,"Stkl_vw_Main",.T.,"_curvouobj.nHandle")=.F.)
*!*	*!*							Do While (dup_no("U_rg23cno",gen_srno_vw.npgno,"Stkl_vw_Main",.T.,"_curvouobj.nHandle")=.F.)	&& Changed by Sachin N. S. on 18/09/2012 for Bug-5164
*!*	*!*								newsrno=gen_no("U_rg23cno","Stkl_vw_Main",.T.,"_curvouobj.nHandle")
*!*	*!*								Replace npgno With newsrno In gen_srno_vw
*!*	*!*								Replace u_rg23cno With newsrno In main_vw
*!*	*!*							Enddo
*!*	*!*	*					Case gen_srno_vw.ctype="RGPART1"
*!*	*!*	*Birendra: Bug-19986 on 11/10/2013 :Modified with below one:FORM402SRNO
*!*	*!*						Case Inlist(gen_srno_vw.ctype,"RGPART1","RGPAGE")
*!*	*!*							Do While (chk_pageno(.T.,"_curvouobj.nHandle")=.F.)
*!*	*!*								Replace u_pageno With '' In item_vw
*!*	*!*								newpgno=gen_pageno()
*!*	*!*								If Betw(mrecno,1,Reccount())
*!*	*!*									Go mrecno
*!*	*!*									Replace npgno With newpgno In gen_srno_vw
*!*	*!*									Replace u_pageno With newpgno In item_vw
*!*	*!*								Endif
*!*	*!*							Enddo
*!*	*!*						Case gen_srno_vw.ctype="PLASRNO"
*!*	*!*	*!*							Do While (dup_no("u_plasr",main_vw.u_plasr,"Stkl_vw_Main",.T.,"_curvouobj.nHandle")=.F.)
*!*	*!*							Do While (dup_no("u_plasr",gen_srno_vw.npgno,"Stkl_vw_Main",.T.,"_curvouobj.nHandle")=.F.)		&& Changed by Sachin N. S. on 18/09/2012 for Bug-5164
*!*	*!*								newsrno=gen_no("u_plasr","Stkl_vw_Main",.T.,"_curvouobj.nHandle")
*!*	*!*								Replace npgno With newsrno In gen_srno_vw
*!*	*!*								Replace u_plasr With newsrno In main_vw
*!*	*!*							Enddo
*!*	*!*	***** Added by Sachin N. S. on 18/09/2012 for Bug-5164 ***** Start
*!*	*!*						Case gen_srno_vw.ctype="SERVTXSRNO"
*!*	*!*							Do While (Dup_ServTxSrNo("SERVTXSRNO",gen_srno_vw.npgno,gen_srno_vw.itserial,gen_srno_vw.SerTy,.T.,"_curvouobj.nHandle")=.F.)		&& Changed by Sachin N. S. on 18/09/2012 for Bug-5164
*!*	*!*								newsrno=Gen_ServTxSrno("SERVTXSRNO",.T.,"_curvouobj.nHandle")
*!*	*!*								Replace npgno With newsrno In gen_srno_vw
*!*	*!*								Replace SERVTXSRNO With newsrno In main_vw
*!*	*!*							Enddo
*!*	*!*	***** Added by Sachin N. S. on 18/09/2012 for Bug-5164 ***** End
*!*	*!*	*Birendra: Bug-19986 on 11/10/2013 : Start:
*!*	*!*						Case gen_srno_vw.ctype="FORM402SRNO"
*!*	*!*							Do While (dup_no("u_402srno",gen_srno_vw.npgno,"Stkl_vw_Main",.T.,"_curvouobj.nHandle")=.F.)		&& Changed by Sachin N. S. on 18/09/2012 for Bug-5164
*!*	*!*								newsrno=gen_no("u_402srno","Stkl_vw_Main",.T.,"_curvouobj.nHandle")
*!*	*!*								Replace npgno With newsrno In gen_srno_vw
*!*	*!*								Replace U_402srno With newsrno In main_vw
*!*	*!*							Enddo
*!*	*!*	*Birendra: Bug-19986 on 11/10/2013 : End:
*!*	*!*	&&added by sandeep for  bug-21327 on 08-02-2014	-->Start
*!*	*!*						Case gen_srno_vw.ctype="FORM403SRNO"
*!*	*!*							Do While (dup_no("u_403srno",gen_srno_vw.npgno,"Stkl_vw_Main",.T.,"_curvouobj.nHandle")=.F.)		&& Changed by Sachin N. S. on 18/09/2012 for Bug-5164
*!*	*!*								newsrno=gen_no("u_403srno","Stkl_vw_Main",.T.,"_curvouobj.nHandle")
*!*	*!*								Replace npgno With newsrno In gen_srno_vw
*!*	*!*								Replace U_403srno With newsrno In main_vw
*!*	*!*							Enddo
*!*	*!*	&&added by sandeep for  bug-21327 on 08-02-2014	<--End


*!*	*!*						Endcase
*!*	*!*	&& Added by Shrikant S. on 29/09/2010 for TKT-4021		--End
*!*	*!*						etsql_str  = _curvouobj.sqlconobj.geninsert("Gen_SrNo","","","Gen_SrNo_Vw",mvu_backend)
*!*	*!*						etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],;
*!*	*!*							"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
*!*	*!*					Endif
*!*	*!*					Select gen_srno_vw
*!*	*!*				Endscan
*!*	*!*			Endif

*!*	*!*		Else
*!*	*!*			etsql_str  = "Select Top 1 Tran_cd from Gen_SrNo Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
*!*	*!*			etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[TmpEt_Vw],;
*!*	*!*				"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
*!*	*!*			If etsql_con > 0 And Used('TmpEt_Vw')
*!*	*!*				Select tmpet_vw
*!*	*!*				If Reccount() > 0
*!*	*!*					etsql_str = _curvouobj.sqlconobj.gendelete("Gen_SrNo","Tran_cd = ?Main_vw.Tran_cd And ;
*!*	*!*							Entry_ty = ?Main_vw.Entry_ty")
*!*	*!*					etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],;
*!*	*!*						"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
*!*	*!*				Endif
*!*	*!*			Endif
*!*	*!*		Endif

*!*	*!*	&& Added by Shrikant S. on 29/09/2010 for TKT-4021		--Start
*!*	*!*		If(!Inlist(main_vw.Rule,"NON-MODVATABLE","ANNEXURE V") Or Inlist(main_vw.entry_ty,"RR","RP","GI","GR","HI","HR"))
*!*	*!*			If	(Type('Main_vw.U_rg23no')='C' Or Type('Main_vw.U_rg23cno')='C')
*!*	*!*				Do Case
*!*	*!*				Case !Empty(main_vw.u_rg23no)
*!*	*!*					etsql_str  = "Update "+_curvouobj.entry_tbl+"Main set U_rg23no='"+main_vw.u_rg23no+"' Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty "
*!*	*!*					etsql_str  = etsql_str +" "+"Update "+_curvouobj.entry_tbl+"Acdet set U_rg23no='"+main_vw.u_rg23no+"' Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty "
*!*	*!*					etsql_con1 = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[_tpageno],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
*!*	*!*				Case !Empty(main_vw.u_rg23cno)
*!*	*!*					etsql_str  = "Update "+_curvouobj.entry_tbl+"Main set U_rg23Cno='"+main_vw.u_rg23cno+"' Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty "
*!*	*!*					etsql_str  = etsql_str +" "+"Update "+_curvouobj.entry_tbl+"Acdet set U_rg23Cno='"+main_vw.u_rg23cno+"' Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty "
*!*	*!*					etsql_con1 = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[_tpageno],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
*!*	*!*				Endcase
*!*	*!*				If Used('_tpageno')
*!*	*!*					Use In _tpageno
*!*	*!*				Endif
*!*	*!*			Endif
*!*	*!*			If Type('Main_vw.u_plasr')='C'
*!*	*!*				If !Empty(main_vw.u_plasr)
*!*	*!*					etsql_str  = "Update "+_curvouobj.entry_tbl+"Main set U_Plasr='"+main_vw.u_plasr+"' Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty "
*!*	*!*					etsql_str  = etsql_str +" "+"Update "+_curvouobj.entry_tbl+"Acdet set U_Plasr='"+main_vw.u_plasr+"' Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty "
*!*	*!*					etsql_con1 = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[_tpageno],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
*!*	*!*					If Used('_tpageno')
*!*	*!*						Use In _tpageno
*!*	*!*					Endif
*!*	*!*				Endif
*!*	*!*			Endif

*!*	*!*	****** Added by Sachin N. S. on 18/09/2012 for Bug-5164 ****** Start
*!*	*!*			If Type('Main_vw.SERVTXSRNO')='C'
*!*	*!*	&&			If !Empty(main_vw.SERVTXSRNO)  &commented by sandeep for the bug-13869 on 20-MAy-2013
*!*	*!*				If !Empty(Nvl((main_vw.SERVTXSRNO),'')) && added by sandeep for bug-13869 on 20-MAy-2013
*!*	*!*					etsql_str  = "Update "+_curvouobj.entry_tbl+"Main set SERVTXSRNO='"+main_vw.SERVTXSRNO+"' Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty "
*!*	*!*					etsql_str  = etsql_str +" "+"Update "+_curvouobj.entry_tbl+"Acdet set SERVTXSRNO='"+main_vw.SERVTXSRNO+"' Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty "
*!*	*!*					etsql_con1 = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[_tpageno],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
*!*	*!*					If Used('_tpageno')
*!*	*!*						Use In _tpageno
*!*	*!*					Endif
*!*	*!*				Endif
*!*	*!*			Endif
*!*	*!*	****** Added by Sachin N. S. on 18/09/2012 for Bug-5164 ****** End

*!*	*!*			Select item_vw

*!*	*!*			Scan
*!*	*!*				If Type('Item_vw.u_pageno')='C'
*!*	*!*					If(!Empty(item_vw.u_pageno))
*!*	*!*						etsql_str  = "Update "+_curvouobj.entry_tbl+"Item set u_pageno='"+item_vw.u_pageno+"' Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty and Itserial=?Item_vw.Itserial"
*!*	*!*						etsql_con1 = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[_temppageno],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
*!*	*!*						If Used('_temppageno')
*!*	*!*							Use In _temppageno
*!*	*!*						Endif
*!*	*!*					Endif
*!*	*!*				Endif
*!*	*!*				Select item_vw
*!*	*!*			Endscan
*!*	*!*		Endif
*!*	*!*	&& Added by Shrikant S. on 29/09/2010 for TKT-4021		--End

*!*	*!*		If Used('TmpEt_Vw')
*!*	*!*			Use In tmpet_vw
*!*	*!*		Endif
*!*	*!*	Endif
&& Commented by Shrikant S. on 29/09/2016 for GST		&& End

&&-->Ipop(Rup)

_malias 	= Alias()
_mrecno	= Recno()

If ([vuexc] $ vchkprod)
	If(_curvouobj.addmode=.T. Or _curvouobj.editmode=.T.)
		If Type('_curvouobj.PCVTYPE')='C'
*			If (Inlist(_curvouobj.pcvtype,'IP','ST','OP','DC')) And Used('projectitref_vw')
*Birendra Bug-4543 on 31/07/2012 : Commented and modified with Below one:
			If (Inlist(_curvouobj.pcvtype,'IP','ST','OP','DC','WI','WO')) And Used('projectitref_vw')
				Set DataSession To _Screen.ActiveForm.DataSessionId
				If (_curvouobj.editmode) &&Delete existing record from projectitref
					etsql_str  = "delete  from projectitref Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
					etsql_con1 = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[_delBom],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
					If Used('_delBom')
						Use In _delbom
					Endif
				Endif

				Select projectitref_vw
				If Reccount()>0 &&Insert records into projectitref
					Replace All tran_cd With main_vw.tran_cd In projectitref_vw
					Scan
						msqlstr  =  _curvouobj.sqlconobj.geninsert ("projectitref","'SERIALNO'","","projectitref_vw",mvu_backend)
						etsql_con1  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,[],;
							"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
					Endscan
				Endif
*Birendra : Bug-8378 on 24/01/2013:Commented:Start:
*!*					IF ! oglblprdfeat.udchkprod('AutoTran') &&Birendra for Bug-4426 on 08/06/2012	&& Commented By Shrikant S. on 29/12/2012 for Bug-2267
*!*					If ! oglblprdfeat.udchkprod('AutoTran') Or  ! oglblprdfeat.udchkprod('stkresrvtn')	&& Added By Shrikant S. on 29/12/2012 for Bug-2267
*!*						If Used('projectitref_vw')			&& Added By Shrikant S. on 18/01/2013 for Bug-8377
*!*							Use In projectitref_vw
*!*						Endif								&& Added By Shrikant S. on 18/01/2013 for Bug-8377
*!*					Endif
*Birendra : Bug-8378 on 24/01/2013:Commented:End:

			Endif
		Endif
&& Added By Kishor A. for Bug-26932 on 10/09/2015 Start
		If Upper(Alltrim(main_vw.party_nm)) = 'CANCELLED'
			etsql_str  = "delete  from projectitref Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
			etsql_con1 = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[_delBom],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
			If Used('_delBom')
				Use In _delbom
			Endif
		Endif
&& Added By Kishor A. for Bug-26932 on 10/09/2015 End

	Else &&Delete Button
		If  (([vuexc] $ vchkprod) Or ([vuinv] $ vchkprod)) And Type('_curvouobj.PCVTYPE')='C' &&Check Existing Records
			etsql_str  = "select top 1 entry_ty from projectitref Where aTran_cd = ?Main_vw.Tran_cd And aEntry_ty = ?Main_vw.Entry_ty"
			etsql_con1 = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[_chkbom],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
			If Used('_chkbom')
				If Reccount()>0
					Select _chkbom
					=Messagebox("Entry Passed Against /"+_chkbom.entry_ty+" Entry Can not be Deleted",16,vumess)
					Use In _chkbom
					Return .F.
				Endif
				Use In _chkbom
			Endif
		Endif
		etsql_str  = "delete  from projectitref Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
		etsql_con1 = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[_delBom],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
		If Used('_delBom')
			Use In _delbom
		Endif
	Endif
Endif

If !Empty(_malias)
	Select &_malias
Endif
If Betw(_mrecno,1,Reccount())
	Go _mrecno
Endif

If _curvouobj.addmode = .T. Or _curvouobj.editmode = .T.
	If Used('_uploadcursor')
		If Reccount('_uploadcursor') > 0
			objupload =Createobject("Udyog.iTAX.FileUpload.Any.Format")
			ServerName=mvu_server
			username  =_curvouobj.sqlconobj.dec(_curvouobj.sqlconobj.ondecrypt(mvu_user))
			spassword =_curvouobj.sqlconobj.dec(_curvouobj.sqlconobj.ondecrypt(mvu_pass))
			connectionstring = "Data Source="+ServerName+";Initial Catalog="+company.dbname+";User ID="+username+";password="+spassword

			Select _uploadcursor
			Replace All tr_id With main_vw.tran_cd In _uploadcursor
			Scan &&While !Eof()
				If _uploadcursor.pwhat = "U" Or _uploadcursor.pwhat = "I"
					If Empty(_uploadcursor.objsave)
						objupload.saveimage(_uploadcursor.tr_type,_uploadcursor.tr_id,_uploadcursor.tr_serial,_uploadcursor.filename,_uploadcursor.extension,_uploadcursor.objpath,connectionstring,_uploadcursor.pwhat,.F.,Null,_uploadcursor.tr_itserial)
					Else
						objupload.saveimage(_uploadcursor.tr_type,_uploadcursor.tr_id,_uploadcursor.tr_serial,_uploadcursor.filename,_uploadcursor.extension,_uploadcursor.objsave,connectionstring,_uploadcursor.pwhat,.T.,Alltrim(_uploadcursor.objsource),_uploadcursor.tr_itserial)
					Endif
				Else
*********** Added By Sachin N. S. on 23/09/2010 for TKT-3982 *********** Start
*!*						If _uploadcursor.pWhat = "D"
*!*							objUpload.DeleteImage(_uploadcursor.tr_type,_uploadcursor.TR_ID,_uploadcursor.tr_serial,ConnectionString,_uploadcursor.DatabaseSave,_uploadcursor.tr_itSerial)		&& Changed By Sachin N. S. on 23/09/2010 for TKT-3982
*!*						Endif
*********** Added By Sachin N. S. on 23/09/2010 for TKT-3982 *********** End
*Birendra : For Bug-661 on 10/11/2011:Start: (Due to above code asking deleted item confirmation)
					If _uploadcursor.pwhat = "D"
						msqlstr = "delete from uploadfiles where tr_type = '" + main_vw.entry_ty + "' and tr_id = " + Str(main_vw.tran_cd)
						msqlstr = msqlstr+" and tr_serial= "+Str(_uploadcursor.tr_serial) + " and tr_itserial = '" + _uploadcursor.tr_itserial+"'"
						nretval = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,"",;
							"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
						If nretval<=0
							Return .F.
						Endif
					Endif
*Birendra : For Bug-661 on 10/11/2011:End:

*!*						objUpload.DeleteImage(_uploadcursor.tr_type,_uploadcursor.TR_ID,_uploadcursor.tr_serial,ConnectionString,Iif(Empty(_uploadcursor.objSave),.F.,.T.),_uploadcursor.tr_itSerial)	&& Commented By Sachin N. S. on 23/09/2010 for TKT-3982

				Endif
				Select _uploadcursor
			Endscan
			objupload.connectionclose()
			objupload.dispose()
		Endif
		Use In _uploadcursor &&Birendra: Bug-661 on 10/12/2011
	Endif
Else
	If _curvouobj.addmode = .F. And _curvouobj.editmode = .F.
		msqlstr = "delete from uploadfiles where tr_type = '" + main_vw.entry_ty + "' and tr_id = " + Str(main_vw.tran_cd)
		nretval = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,"",;
			"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
		If nretval<=0
			Return .F.
		Endif

	Endif
Endif

******** Added By Sachin N. S. on 20/12/2010 for New Installer ******** Start
*Birendra: Bug-19986 on 22/10/2013 : Start::Commented:
*!*	If _curvouobj.itempage	And ([vutex] $ vchkprod) And etsql_con > 0
*!*		If _curvouobj.addmode Or _curvouobj.editmode
*!*			If Used('Gen_SrNo_Vw')
*!*				Select gen_srno_vw
*!*				Replace All tran_cd With main_vw.tran_cd,entry_ty With main_vw.entry_ty,;
*!*					DATE With main_vw.Date,compid With main_vw.compid,l_yn With main_vw.l_yn In gen_srno_vw
*!*	*npgno,cit_code,cware,cgroup,cchapno
*!*	*itserial

*!*				etsql_str  = "Select * from Gen_SrNo Where l_yn = ?main_vw.l_yn and Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
*!*				etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[TmpEt_Vw],;
*!*					"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
*!*				If etsql_con > 0 And Used('TmpEt_Vw')
*!*					Select tmpet_vw
*!*					Scan
*!*						metdele = .F.
*!*						Select item_vw
*!*						If !Seek(main_vw.entry_ty+Dtos(main_vw.Date)+main_vw.doc_no+tmpet_vw.itserial,'Item_vw','Eddits')
*!*							metdele = .T.
*!*						Else
*!*							If tmpet_vw.cit_code # item_vw.it_code &&Or TmpEt_Vw.Cware # Item_vw.Ware_nm
*!*								metdele = .T.
*!*							Endif
*!*						Endif
*!*						If metdele = .T. And etsql_con > 0
*!*							etsql_str = _curvouobj.sqlconobj.gendelete("Gen_SrNo","Tran_cd = ?Main_vw.Tran_cd And ;
*!*									Entry_ty = ?Main_vw.Entry_ty And Itserial = ?TmpEt_Vw.itserial")
*!*							etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],;
*!*								"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
*!*						Else
*!*							Select gen_srno_vw
*!*							If Seek(tmpet_vw.itserial,'Gen_SrNo_Vw','ItSerial') And ;
*!*									gen_srno_vw.cit_code = tmpet_vw.cit_code And  ;
*!*									gen_srno_vw.cgroup = tmpet_vw.cgroup And gen_srno_vw.cchapno = tmpet_vw.cchapno		&&Gen_SrNo_Vw.Cware = TmpEt_Vw.Cware AND
*!*								Replace npgno With '' In gen_srno_vw
*!*							Endif
*!*						Endif
*!*						Select tmpet_vw
*!*					Endscan
*!*				Else
*!*					etsql_con = 0
*!*				Endif

*!*				Select gen_srno_vw
*!*				Scan
*!*					Select item_vw
*!*					If !Seek(main_vw.entry_ty+Dtos(main_vw.Date)+main_vw.doc_no+gen_srno_vw.itserial,'Item_vw','Eddits')
*!*						Replace npgno With '' In gen_srno_vw
*!*					Else
*!*						If gen_srno_vw.cit_code # item_vw.it_code &&Or Gen_SrNo_Vw.Cware # Item_vw.Ware_nm
*!*							Replace npgno With '' In gen_srno_vw
*!*						Endif
*!*					Endif
*!*					If etsql_con > 0 And !Empty(gen_srno_vw.npgno)
*!*						etsql_str  = _curvouobj.sqlconobj.geninsert("Gen_SrNo","","","Gen_SrNo_Vw",mvu_backend)
*!*						etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],;
*!*							"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
*!*					Endif
*!*					Select gen_srno_vw
*!*				Endscan
*!*			Endif
*!*		Else
*!*			etsql_str  = "Select Top 1 Tran_cd from Gen_SrNo Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
*!*			etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[TmpEt_Vw],;
*!*				"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
*!*			If etsql_con > 0 And Used('TmpEt_Vw')
*!*				Select tmpet_vw
*!*				If Reccount() > 0
*!*					etsql_str = _curvouobj.sqlconobj.gendelete("Gen_SrNo","Tran_cd = ?Main_vw.Tran_cd And ;
*!*							Entry_ty = ?Main_vw.Entry_ty")
*!*					etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],;
*!*						"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
*!*				Endif
*!*			Endif
*!*		Endif
*!*		If Used('TmpEt_Vw')
*!*			Use In tmpet_vw
*!*		Endif
*!*	Endif
******** Added By Sachin N. S. on 20/12/2010 for New Installer ******** End
*Birendra: Bug-19986 on 22/10/2013 : End::Commented:

&& Rup--->12/08/2009
If Used('Gen_SrNo_Vw')
	Use In gen_srno_vw
Endif
&&<---Rup 12/08/2009

*!*	IF _curvouobj.Addmode = .t. OR _curvouobj.EditMode = .t.
*!*		IF USED('_uploadcursor')
*!*			IF RECCOUNT('_uploadcursor') > 0
*!*				objUpload =CREATEOBJECT("Udyog.iTAX.FileUpload.Any.Format")
*!*				ServerName=mvu_server
*!*				UserName  =_curvouobj.SqlConObj.dec(_curvouobj.sqlconObj.ondecrypt(mvu_user))
*!*				SPassword =_curvouobj.SqlConObj.dec(_curvouobj.sqlconObj.ondecrypt(mvu_Pass))
*!*				ConnectionString = "Data Source="+ServerName+";Initial Catalog="+Company.dbname+";User ID="+UserName+";password="+SPassWord

*!*				SELECT _uploadcursor
*!*				GO top
*!*				SCAN WHILE !EOF()
*!*					IF _uploadcursor.pWhat = "U" OR _uploadcursor.pWhat = "I"
*!*						IF EMPTY(_uploadcursor.objSave)
*!*							objUpload.SaveImage(_uploadcursor.tr_type,_uploadcursor.tr_id,_uploadcursor.tr_serial,_uploadcursor.filename,_uploadcursor.Extension,_uploadcursor.objPath,ConnectionString,_uploadcursor.pWhat,.f.,null)
*!*						ELSE
*!*							objUpload.SaveImage(_uploadcursor.tr_type,_uploadcursor.tr_id,_uploadcursor.tr_serial,_uploadcursor.filename,_uploadcursor.Extension,_uploadcursor.objSave,ConnectionString,_uploadcursor.pWhat,.t.,ALLTRIM(_uploadcursor.objSource))
*!*						ENDIF
*!*					ENDIF
*!*				ENDSCAN
*!*				objUpload.ConnectionClose()
*!*				objUpload.Dispose()
*!*			ENDIF
*!*		ENDIF
*!*	ELSE
*!*		IF _curvouobj.addmode = .f. AND _curvouobj.editmode = .f.
*!*			mSqlStr = "delete from uploadfiles where tr_type = '" + main_vw.entry_ty + "' and tr_id = " + STR(main_vw.tran_cd)
*!*			nRetval = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,mSqlstr,"",;
*!*								"_curvouobj.nHandle",_curvouobj.DataSessionId,.t.)
*!*			IF nretVal<=0
*!*				RETURN .f.
*!*			ENDIF

*!*		ENDIF
*!*	ENDIF

******By Shrikant S. on  27/01/2011 ---Start for TKT-5814
If _curvouobj.addmode = .T. Or _curvouobj.editmode = .T.
	If Used('ItRef_vw')
		Select itref_vw
		Scan
&&--->Commented by Rup 18/04/2011 TKT-6627,6628,6829
*!*				etsql_str = "Select Top 1 Tran_cd from  "+ItRef_vw.REntry_ty+"item where Tran_cd = ?ItRef_vw.Itref_tran And Itserial = ?ItRef_vw.Ritserial  and re_qty>qty"
*!*				etsql_con1  = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,etsql_str,;
*!*					[tmpreqty_vw],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)

*!*				If etsql_con1 > 0 And Used('tmpreqty_vw')
*!*					Select tmpreqty_vw
*!*					If Reccount()>0
*!*						sql_errmess = Chr(13)+" Unable to Pickup. Since it is already picked."+Chr(13)
*!*						Return .F.
*!*					Endif
*!*					If Used('tmpreqty_vw')
*!*						Select tmpreqty_vw
*!*						Use In tmpreqty_vw
*!*					Endif
*!*				Endif
&&<---Commented by Rup 18/04/2011 TKT-6627,6628,6829
&&--->Added by Rup 18/04/2011 TKT-6627,6628,6829
			etsql_str = "select bcode_nm=case when ext_vou=1 then bcode_nm else entry_ty end,uom_desc  from lcode where entry_ty='"+itref_vw.rentry_ty+"'"
			etsql_con1  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,;
				"llcode","_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
			etsql_str = "Select  * from  "+llcode.bcode_nm+"item where Tran_cd = ?ItRef_vw.Itref_tran And Itserial = ?ItRef_vw.Ritserial "
			etsql_con1  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,;
				[tmpreqty_vw],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
			mansln =0
			Select llcode
			If !Empty(llcode.uom_desc) And !Empty(lcode_vw.uom_desc)
				_muom_desc2 = llcode.uom_desc
				For i1 = 1 To company.uom_no
					If Type('_curvouobj.multi_qty(i1,1)') = 'C'
						vq= Iif(At(';',_muom_desc2)	> 0,Substr(_muom_desc2,1,At(';',_muom_desc2)-1),_muom_desc2)
						vqty= Iif(At(':',vq) > 0,Substr(vq,1,At(':',vq)-1),vq)
						v2 = Iif(At(':',vq) > 0,Substr(vq,At(':',vq)+1),'')
						vsoft = Substr(v2 ,1,3)
						vhard = Substr(v2 ,5,3)
						If vsoft=".f." And vhard =".f."
							vhard =".t."
						Endif
						x="tmpreqty_vw.re_"+vqty+">tmpreqty_vw."+vqty
						If &x
							vx=0
							x="tmpreqty_vw."+vqty
							vx=&x
							mmess =vqty+" Value Exceeds Balance Value for "+Alltrim(Str(vx))
							If 	Lower(vsoft) ='.t.'
								If _curvouobj.showmessagebox(mmess+Chr(13)+'Proceed ?',4,vumess,1) # 6		&&vasant051209
									sql_errmess = Chr(13)+" Unable to Pickup. Since it is already picked."+Chr(13)
									Return .F.
								Endif
							Else
								If 	Lower(vhard) ='.t.'
									sql_errmess = Chr(13)+" Unable to Pickup. Since it is already picked."+Chr(13)
									Return .F.
								Endif
							Endif
						Endif
					Endi
				Endfor
			Else
				If etsql_con1 > 0 And Used('tmpreqty_vw')
					Select tmpreqty_vw
					If Reccount()>0
						If tmpreqty_vw.re_qty>tmpreqty_vw.qty
							sql_errmess = Chr(13)+" Unable to Pickup. Since it is already picked."+Chr(13)
							Return .F.
						Endif
					Endif
					If Used('tmpreqty_vw')
						Select tmpreqty_vw
						Use In tmpreqty_vw
					Endif
				Endif
			Endif
			Select itref_vw
&&<---Added by Rup 18/04/2011 TKT-6627,6628,6829
		Endscan
	Endif
Endif
******By Shrikant S. on  27/01/2011 ---End

If 'trnamend' $ vchkprod
	Do voufinalupdate In mainprg &&Birendra :(TKT-2386) 22 Mar 2011 for Order Amendment
Endif

******** Added By Sachin N. S. on 13/02/2012 for TKT-9411 And BUG-660 Batchwise/Serialize Inventory ******** Start
If _curvouobj.itempage
	If Type('_curvouobj._BatchSerialStk')='O'
		If etsql_con>0
			etsql_con=_curvouobj._batchserialstk._uetrigvoufinalupdate()
&& added by Shrikant S. on 11/07/2015 for Bug-25878			&& Start
			If etsql_con <=0
				Return .F.
			Endif
&& added by Shrikant S. on 11/07/2015 for Bug-25878			&& End
		Endif
	Endif
Endif
******** Added By Sachin N. S. on 13/02/2012 for TKT-9411 And BUG-660 Batchwise/Serialize Inventory ******** End

*!*	** Start : Added by Uday on 26/12/2011 for Exim
_mexim = .F.                              && Added by Ajay Jaiswal on 22/02/2012 for EXIM
_mdbk = .F.                               && Added by Ajay Jaiswal on 03/04/2012 for DBK
_mexim = oglblprdfeat.udchkprod('exim')   && Added by Ajay Jaiswal on 22/02/2012 for EXIM
_mdbk = oglblprdfeat.udchkprod('dbk')     && Added by Ajay Jaiswal on 03/04/2012 for DBK
If _mexim  Or _mdbk                                 && Added by Ajay Jaiswal on 21/02/2012 for EXIM & DBK
	If _curvouobj.pcvtype = "OP"
		If _curvouobj.addmode = .T. Or _curvouobj.editmode = .T.
			If Used("OP_Pttax_vw")
				Update op_pttax_vw Set tran_cd = main_vw.tran_cd ;
					WHERE tran_cd = 0

				msqlstr = "delete from op_pttaxdet where entry_ty = '" + main_vw.entry_ty + "' and tran_cd = " + Str(main_vw.tran_cd)
				nretval = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,"",;
					"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
				If nretval<=0
					Return .F.
				Endif
				Go Top In op_pttax_vw
				Store "" To msqlstr
				Do While !Eof("Op_Pttax_vw")
					msqlstr = _curvouobj.sqlconobj.geninsert("op_pttaxdet","'item'","","op_pttax_vw",mvu_backend)

					nretval = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,"",;
						"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
					If nretval<=0
						Return .F.
					Endif

					Skip In op_pttax_vw
				Enddo

			Endif
		Else
			If Used("OP_Pttax_vw")
				msqlstr = "delete from op_pttaxdet where entry_ty = '" + main_vw.entry_ty + "' and tran_cd = " + Str(main_vw.tran_cd)
				nretval = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,"",;
					"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
				If nretval<=0
					_curvouobj.showmessagebox("Error while Delete Record from Op_PTtaxDet table",32,vumess)
					Return .F.
				Endif

				malias = Alias()
				Select op_pttax_vw
				Delete All
				Select (malias)

				With _curvouobj.voupage.pgappduties.grdappduties
					.Refresh()
				Endwith

			Endif
		Endif
	Endif
Endif
*!*	** End : Added by Uday on 26/12/2011 for Exim

** 	Added By Shrikant S. on 31/05/2012 for Bug-3248		&& Start
If _curvouobj.addmode = .F. And _curvouobj.editmode = .F.
	If Inlist(main_vw.entry_ty,"J2","J3")
		msqlstr = "delete from SertaxCrRef where rentry_ty = '" + main_vw.entry_ty + "' and rtran_cd = " + Str(main_vw.tran_cd)
		nretval = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,"",;
			"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
		If nretval<=0
			Return .F.
		Endif
	Endif
Endif
** 	Added By Shrikant S. on 31/05/2012 for Bug-3248		&& End
** 	Bug-4885 Rup--->
If Inli(main_vw.entry_ty,"PP","TH","FH","EH","RH")
	If _curvouobj.addmode = .F. And _curvouobj.editmode = .F.
		msqlstr = "Update Emp_Monthly_Payroll Set "+main_vw.entry_ty+"_Ent_Ty='',"+main_vw.entry_ty+"_Trn_Cd=0 where "+main_vw.entry_ty+"_Ent_Ty='"+main_vw.entry_ty+"' and "+main_vw.entry_ty+"_Trn_Cd=" + Str(main_vw.tran_cd)
		nretval = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,"",;
			"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
		If nretval<=0
			Return .F.
		Endif
	Else
		If Used("PayRoll_vw")
			Select PayRoll_vw
			Go Top
			Scan For sel
				msqlstr = "Update Emp_Monthly_Payroll Set "+main_vw.entry_ty+"_Ent_Ty='"+main_vw.entry_ty+"',"+main_vw.entry_ty+"_Trn_Cd=" + Alltrim(Str(main_vw.tran_cd))+" Where Pay_Year='"+Alltrim(PayRoll_vw.Pay_Year)+"' and Pay_Month="+Alltrim(Str(PayRoll_vw.Pay_Month))+" and EmployeeCode='"+Alltrim(PayRoll_vw.EmployeeCode)+"'"
				nretval = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,"",;
					"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
				If nretval<=0
					Return .F.
				Endif
			Endscan
			Use In PayRoll_vw
		Endif
	Endif
Endif
**<---Bug-4885 Rup
&&Added By Shrikant S. on 29/12/2012 for Bug-2267 		&& Start &&vasant030412
If _curvouobj.addmode = .T. Or _curvouobj.editmode = .T.
	_mstkresrvtn = .F.
	_mstkresrvtn = oglblprdfeat.udchkprod('stkresrvtn')
	If _mstkresrvtn = .T.
		If etsql_con > 0
			etsql_str  = "Delete from StkResrvDet Where Entry_ty = ?Main_vw.Entry_ty and Tran_cd = ?Main_vw.Tran_cd"
			etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],;
				"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)

			etsql_str  = "Delete from StkResrvDet Where REntry_ty = ?Main_vw.Entry_ty and RTran_cd = ?Main_vw.Tran_cd"
			etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],;
				"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
		Endif

*!*			If (lcode_vw.entry_ty ='OP' Or lcode_vw.bcode_nm ='OP') 							&& Commented by Shrikant S. on 18/01/2013 for Bug-8377
		If (lcode_vw.entry_ty ='OP' Or lcode_vw.bcode_nm ='OP') And Used('projectitref_vw')		&& Added by Shrikant S. on 18/01/2013 for Bug-8377
			Select projectitref_vw
			Scan
				If etsql_con > 0
					_mQty1 = projectitref_vw.qty
					_mQty2 = 0
					_mAllocQty = 0

					etsql_str  = "Select SUM(Qty) As Qty from ProjectItRef Where AEntry_ty = ?ProjectItRef_vw.AEntry_ty and ATran_cd = ?ProjectItRef_vw.ATran_cd and AItSerial = ?ProjectItRef_vw.AItSerial"
					etsql_str  = etsql_str + " and Entry_ty = 'OP' "
					etsql_str  = etsql_str + " and Entry_ty + CAST(Tran_cd as varchar(10)) + ItSerial != ?ProjectItRef_vw.Entry_ty + CAST(?ProjectItRef_vw.Tran_cd as varchar(10)) + ?ProjectItRef_vw.ItSerial "
					etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[chktbl_vw],;
						"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
					If etsql_con > 0 And Used('chktbl_vw')
						_mQty2 = Iif(Isnull(chktbl_vw.qty)=.F.,chktbl_vw.qty,0)
					Endif

					etsql_str  = "Select * from StkResrvDet Where Entry_ty = ?ProjectItRef_vw.AEntry_ty and Tran_cd = ?ProjectItRef_vw.ATran_cd and ItSerial = ?ProjectItRef_vw.AItSerial"
					etsql_str  = etsql_str + " And REntry_ty = 'SO'"
					etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[chktbl_vw],;
						"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
					If etsql_con > 0 And Used('chktbl_vw')
						Select chktbl_vw
						Scan
							If _mQty2 > chktbl_vw.AllocQty
								Replace AllocQty With 0 In chktbl_vw
								_mQty2 = _mQty2 - chktbl_vw.AllocQty
							Else
								Replace AllocQty With AllocQty - _mQty2 In chktbl_vw
								_mQty2 = 0
							Endif
							If _mQty2 = 0
								If _mQty1 > chktbl_vw.AllocQty
									_mAllocQty = chktbl_vw.AllocQty
									_mQty1 = _mQty1 - chktbl_vw.AllocQty
								Else
									_mAllocQty = _mQty1
									Replace AllocQty With _mQty1 In chktbl_vw
									_mQty1 = 0
								Endif
								If _mAllocQty > 0
									etsql_str  = "Insert Into StkResrvDet (Entry_ty,Tran_cd,ItSerial,It_code,AllocQty,REntry_ty,RTran_cd,RItSerial,RCTran_cd)"
									etsql_str  = etsql_str + "Values (?ProjectItRef_vw.Entry_ty,?ProjectItRef_vw.Tran_cd,?ProjectItRef_vw.ItSerial,?ProjectItRef_vw.It_code,?_mAllocQty,?chktbl_vw.REntry_ty,?chktbl_vw.RTran_cd,?chktbl_vw.RItSerial,?chktbl_vw.CTran_cd)"
									etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],;
										"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)

									etsql_str  = "Insert Into StkResrvDet (Entry_ty,Tran_cd,ItSerial,It_code,AllocQty,REntry_ty,RTran_cd,RItSerial,RCTran_cd)"
									etsql_str  = etsql_str + "Values (?ProjectItRef_vw.AEntry_ty,?ProjectItRef_vw.ATran_cd,?ProjectItRef_vw.AItSerial,?ProjectItRef_vw.It_code,?_mAllocQty,?ProjectItRef_vw.Entry_ty,?ProjectItRef_vw.Tran_cd,?ProjectItRef_vw.ItSerial,?chktbl_vw.CTran_cd)"
									etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],;
										"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
								Endif
							Endif
							Select chktbl_vw
						Endscan
					Endif
				Endif
				Select projectitref_vw
			Endscan
		Endif

		If Used('ItRef_vw')
			Select itref_vw
			Scan
				If etsql_con > 0
					_mQty1 = itref_vw.RQty
					_mQty2 = 0
					_mAllocQty = 0
*					etsql_str  = "Select SUM(Qty) As Qty from "+_curvouobj.entry_tbl+"ItRef Where REntry_ty = ?ItRef_vw.REntry_ty and ItRef_Tran = ?ItRef_vw.ItRef_Tran and RItSerial = ?ItRef_vw.RItSerial"
*Birendra : Bug-8378 on 24/01/2013 :Commented above changes:
					etsql_str  = "Select SUM(RQty) As Qty from "+_curvouobj.entry_tbl+"ItRef Where REntry_ty = ?ItRef_vw.REntry_ty and ItRef_Tran = ?ItRef_vw.ItRef_Tran and RItSerial = ?ItRef_vw.RItSerial"
					etsql_str  = etsql_str + " and Entry_ty + CAST(Tran_cd as varchar(10)) + ItSerial != ?ItRef_vw.Entry_ty + CAST(?ItRef_vw.Tran_cd as varchar(10)) + ?ItRef_vw.ItSerial "
					etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[chktbl_vw],;
						"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
					If etsql_con > 0 And Used('chktbl_vw')
						_mQty2 = Iif(Isnull(chktbl_vw.qty)=.F.,chktbl_vw.qty,0)
					Endif
					etsql_str  = "Select * from StkResrvDet Where Entry_ty = ?ItRef_vw.REntry_ty and Tran_cd = ?ItRef_vw.ItRef_Tran and ItSerial = ?ItRef_vw.RItSerial"
					etsql_str  = etsql_str + " And REntry_ty = 'SO'"
					etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[chktbl_vw],;
						"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
					If etsql_con > 0 And Used('chktbl_vw')
						Select chktbl_vw
						Scan
							If _mQty2 > chktbl_vw.AllocQty
								Replace AllocQty With 0 In chktbl_vw
								_mQty2 = _mQty2 - chktbl_vw.AllocQty
							Else
								Replace AllocQty With AllocQty - _mQty2 In chktbl_vw
								_mQty2 = 0
							Endif
							If _mQty2 = 0
								If _mQty1 > chktbl_vw.AllocQty
									_mAllocQty = chktbl_vw.AllocQty
									_mQty1 = _mQty1 - chktbl_vw.AllocQty
								Else
									_mAllocQty = _mQty1
									Replace AllocQty With _mQty1 In chktbl_vw
									_mQty1 = 0
								Endif
								If _mAllocQty > 0
									etsql_str  = "Insert Into StkResrvDet (Entry_ty,Tran_cd,ItSerial,It_code,AllocQty,REntry_ty,RTran_cd,RItSerial,RCTran_cd)"
									etsql_str  = etsql_str + "Values (?ItRef_vw.Entry_ty,?ItRef_vw.Tran_cd,?ItRef_vw.ItSerial,?ItRef_vw.It_code,?_mAllocQty,?chktbl_vw.REntry_ty,?chktbl_vw.RTran_cd,?chktbl_vw.RItSerial,?chktbl_vw.CTran_cd)"
									etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],;
										"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)

									etsql_str  = "Insert Into StkResrvDet (Entry_ty,Tran_cd,ItSerial,It_code,AllocQty,REntry_ty,RTran_cd,RItSerial,RCTran_cd)"
									etsql_str  = etsql_str + "Values (?ItRef_vw.REntry_ty,?ItRef_vw.ItRef_Tran,?ItRef_vw.RItSerial,?ItRef_vw.It_code,?_mAllocQty,?ItRef_vw.Entry_ty,?ItRef_vw.Tran_cd,?ItRef_vw.ItSerial,?chktbl_vw.CTran_cd)"
									etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],;
										"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
								Endif
							Endif
							Select chktbl_vw
						Endscan
					Endif
				Endif
				Select itref_vw
			Endscan
		Endif
	Endif
Endif

&& Added By Shrikant S. on 08/12/2012 for Bug-2267			&& Start
If _curvouobj.addmode = .F. And _curvouobj.editmode = .F.
	_mstkresrvtn = .F.
	_mstkresrvtn = oglblprdfeat.udchkprod('stkresrvtn')
	If _mstkresrvtn = .T.
		If (lcode_vw.entry_ty ='SO' Or lcode_vw.bcode_nm ='SO')
			If etsql_con >0
				etsql_str  = "Delete from Stkresrvsum Where Entry_ty = ?Main_vw.Entry_ty and Tran_cd = ?Main_vw.Tran_cd"
				etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
			Endif
		Endif
	Endif
Endif
&& Added By Shrikant S. on 08/12/2012 for Bug-2267			&& End
&&Added By Shrikant S. on 29/12/2012 for Bug-2267 		&& End &&vasant030412
************ Added By Shrikant S. on 30/10/2012	FOR BUG-6867	****************** Start
If !_curvouobj.addmode And !_curvouobj.editmode
	If Vartype(oglblprdfeat)='O'
		If oglblprdfeat.udchkprod('openord')
			If Used("SpDiff_vw")
				etsql_str = _curvouobj.sqlconobj.gendelete("SpDiff","Tran_cd = ?Main_vw.Tran_cd And ;
						Entry_ty = ?Main_vw.Entry_ty")
				etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],;
					"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
				If etsql_con<0
					Return .F.
				Endif
			Endif
			If _curvouobj.pcvtype="OO"
				etsql_str = _curvouobj.sqlconobj.gendelete("Amend_Detail","Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty")
				etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],;
					"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
				If etsql_con<0
					Return .F.
				Endif
			Endif
			If _curvouobj.pcvtype="ST"
				etsql_str = _curvouobj.sqlconobj.gendelete("SpDiff","Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty")
				etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],;
					"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
				If etsql_con<0
					Return .F.
				Endif
			Endif

		Endif
	Endif
Endif
************ Added By Shrikant S. on 30/10/2012	FOR BUG-6867	****************** Start
&& Added By Shrikant S. on 01/02/2014 for Bug-13926		&& Start
If !_curvouobj.addmode And !_curvouobj.editmode
	If Vartype(oglblprdfeat)='O'
		If oglblprdfeat.udchkprod('mrp')
			etsql_str = "Select [Name] From SysObjects Where Xtype='U' and [Name]='MRPLOG'"
			etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,"tmpCur",;
				"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
			If etsql_con<0
				Return .F.
			Endif
			If Reccount('tmpCur')>0
				etsql_str = _curvouobj.sqlconobj.gendelete("MRPLOG","ItRef_Tran= ?Main_vw.Tran_cd And ;
						REntry_ty = ?Main_vw.Entry_ty")
				etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],;
					"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
				If etsql_con<0
					Return .F.
				Endif
			Endif
			If Used('tmpCur')
				Use In tmpCur
			Endif
		Endif
	Endif
Endif
&& Added By Shrikant S. on 01/02/2014 for Bug-13926		&& End

&& Added by Shrikant S. on 29/09/2014 for Bug-23879		&& Start
If _curvouobj.addmode Or _curvouobj.editmode
	If Vartype(oglblprdfeat)='O'
		If oglblprdfeat.udchkprod('qctrl')
			If _curvouobj.pcvtype='OS'
				Select item_vw
				lnrecno=Iif(!Eof(),Recno(),0)
				Scan
					Replace qcaceptqty With qty, qcholdqty With 0 In item_vw

					etsql_str  = "Update "+_curvouobj.entry_tbl+"Item set qcaceptqty=qty,qcholdqty=0 Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty and Itserial=?Item_vw.Itserial"
					etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)

					Select item_vw
				Endscan
				If lnrecno>0
					Select item_vw
					Go lnrecno
				Endif
			Endif
		Endif
	Endif
Else
	If Vartype(oglblprdfeat)='O'
		If oglblprdfeat.udchkprod('qctrl') And Inlist(lcode_vw.inv_stk,'+')
			etsql_str="Select Insp_id From qc_inspection_master Where Entry_ty=?Main_vw.Entry_ty and Tran_cd=?Main_vw.Tran_cd "
			etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[qcvw],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
			If etsql_con>0 And Reccount('qcvw')>0
				Select qcvw
				Locate
				etsql_str  = "Delete From qc_inspection_item Where Insp_Id=?qcvw.insp_Id "
				etsql_str  = etsql_str  +" Delete from QC_INSPECTION_PARAMETER Where Insp_Id=?qcvw.insp_Id "
				etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
			Endif
		Endif
	Endif
Endif
&& Added by Shrikant S. on 29/09/2014 for Bug-23879		&& End

&& Added by Shrikant S. on 03/06/2017 for GST		&& Start
If Vartype(oglblindfeat)='O'
	If oglblindfeat.udchkind('vugst')
		If Inlist(_curvouobj.pcvtype,"PT","P1","E1","GC","GD","C6","D6") And Type('main_vw.u_cldt')<>'U'
			Do Case
			Case _curvouobj.pcvtype="P1"
				Replace u_cldt With pinvdt,tranStatus WITH .t. In main_vw
*!*				Case Inlist(_curvouobj.pcvtype,"PT","E1") And (_curvouobj.taxapplarea="OUT OF COUNTRY" Or _curvouobj.accregistatus="UNREGISTERED")		&& Commented by Shrikant S. on 12/06/2017 for GST
			Case Inlist(_curvouobj.pcvtype,"PT","E1") And _curvouobj.taxapplarea="OUT OF COUNTRY"			&& Added by Shrikant S. on 12/06/2017 for GST
				Replace u_cldt With pinvdt,tranStatus WITH .t. In main_vw
*!*				Case Inlist(_curvouobj.pcvtype,"C6","D6","GC","GD") And Inlist(main_vw.againstgs,"PURCHASES","SERVICE PURCHASE BILL") And (_curvouobj.taxapplarea="OUT OF COUNTRY" Or _curvouobj.accregistatus="UNREGISTERED")		&& Commented by Shrikant S. on 12/06/2017 for GST
			Case Inlist(_curvouobj.pcvtype,"C6","D6","GC","GD") And Inlist(main_vw.againstgs,"PURCHASES","SERVICE PURCHASE BILL") And _curvouobj.taxapplarea="OUT OF COUNTRY"		&& Added by Shrikant S. on 12/06/2017 for GST	
				Replace u_cldt With pinvdt,tranStatus WITH .t. In main_vw
			Endcase
		Endif
	Endif
Endif
&& Added by Shrikant S. on 03/06/2017 for GST		&& End

&& Added by Shrikant S. on 30/06/2014 for Bug-23280		&& Start
If Vartype(oglblindfeat)='O'
	If oglblindfeat.udchkind('pharmaind')
		If _curvouobj.itempage And Inlist(main_vw.entry_ty,"AR","WK","OS")
			If _curvouobj.addmode Or _curvouobj.editmode
				If Used('BatchTbl_Vw')
					Select BatchTbl_Vw
					Replace All tran_cd With main_vw.tran_cd,entry_ty With main_vw.entry_ty,;
						DATE With main_vw.Date,compid With main_vw.compid,l_yn With main_vw.l_yn In BatchTbl_Vw

					etsql_str  = "Select * from BatchGenTbl Where l_yn = ?main_vw.l_yn and Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
					etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[tmprunno_vw],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
					If etsql_con > 0 And Used('tmprunno_vw')
						Select tmprunno_vw
						Scan
							metdele = .F.
							Select item_vw
							If !Seek(main_vw.entry_ty+Dtos(main_vw.Date)+main_vw.doc_no+tmprunno_vw.itserial,'Item_vw','Eddits')
								metdele = .T.
							Else
								If Seek(main_vw.entry_ty+Dtos(main_vw.Date)+main_vw.doc_no+tmprunno_vw.itserial,'Item_vw','Eddits')
									If Empty(item_vw.batchno)
										metdele = .T.
									Endif
								Endif
							Endif
							If metdele = .T. And etsql_con > 0
								etsql_str = _curvouobj.sqlconobj.gendelete("BatchGenTbl","Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty And Itserial = ?tmprunno_vw.itserial")
								etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
							Else
								Select BatchTbl_Vw
								If Seek(tmprunno_vw.itserial,'BatchTbl_Vw','ItSerial')
									Replace batchno With '',runno With '' In BatchTbl_Vw
								Endif
							Endif
							Select tmprunno_vw
						Endscan
					Else
						etsql_con = 0
					Endif

					Select BatchTbl_Vw
					Scan
						mrecno=Recno()
						Select item_vw
						If !Seek(main_vw.entry_ty+Dtos(main_vw.Date)+main_vw.doc_no+BatchTbl_Vw.itserial,'Item_vw','Eddits')
							Replace batchno With '',runno With '' In BatchTbl_Vw
						Else
							If Seek(main_vw.entry_ty+Dtos(main_vw.Date)+main_vw.doc_no+BatchTbl_Vw.itserial,'Item_vw','Eddits')
								If Empty(item_vw.batchno)
									Replace batchno With '',runno With '' In BatchTbl_Vw
								Else
									Replace batchno With item_vw.batchno In BatchTbl_Vw
								Endif
							Endif
						Endif
						If etsql_con > 0 And !Empty(BatchTbl_Vw.runno)
							Do While (CheckBatchNo(.T.,"_curvouobj.nHandle")=.F.)
								Replace batchno With '' In item_vw
								newbatchno=GenBatchNo()
								If Betw(mrecno,1,Reccount())
									Go mrecno
									If _item.IsRunning=.T.
										If Alltrim(_item.BatchSfix)<>""
											_mrunno=Val(Left(Right(Alltrim(newbatchno),6+Len(Alltrim(BatchSfix))),6))
										Else
											_mrunno=Val(Right(Alltrim(newbatchno),6))
										Endif
									Else
										_mrunno=0
									Endif


									Replace batchno With newbatchno,runno With Str(_mrunno) In BatchTbl_Vw
									Replace batchno With newbatchno In item_vw
								Endif
							Enddo
							etsql_str  = _curvouobj.sqlconobj.geninsert("BatchGenTbl","","","BatchTbl_Vw",mvu_backend)
							etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
						Endif
						Select BatchTbl_Vw
					Endscan
					Use In BatchTbl_Vw
				Endif
			Else
				etsql_str  = "Select Top 1 Tran_cd from BatchGenTbl Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
				etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[tmprunno_vw],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
				If etsql_con > 0 And Used('tmprunno_vw')
					Select tmprunno_vw
					If Reccount() > 0
						etsql_str = _curvouobj.sqlconobj.gendelete("BatchGenTbl","Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty")
						etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
					Endif
				Endif
			Endif


			Select item_vw
			Scan
				If Type('Item_vw.batchno')='C'
					If(!Empty(item_vw.batchno))
						etsql_str  = "Update "+_curvouobj.entry_tbl+"Item set batchno=?item_vw.batchno Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty and Itserial=?Item_vw.Itserial"
						etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[_temppageno],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
						If Used('_temppageno')
							Use In _temppageno
						Endif
					Endif
				Endif
				Select item_vw
			Endscan

			If Used('tmprunno_vw')
				Use In tmprunno_vw
			Endif

			If (_curvouobj.pcvtype="WK")
				If (_curvouobj.addmode Or _curvouobj.editmode) And Used("wkrmdet_vw")
					If (_curvouobj.editmode) &&Delete existing record from projectitref
						etsql_str  = "delete from WKRMDET Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
						etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,"","_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
						If etsql_con<=0
							Return .F.
						Endif
					Endif

					Select wkrmdet_vw
					Count For !Deleted() To wkcnt

					If wkcnt>0 &&Insert records into projectitref
						Replace All tran_cd With main_vw.tran_cd In wkrmdet_vw
						Scan
							msqlstr  =  _curvouobj.sqlconobj.geninsert ("WKRMDET","'ID'","","wkrmdet_vw",mvu_backend)
							etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
							If etsql_con<=0
								Return .F.
							Endif
						Endscan
						Use In wkrmdet_vw
					Else
						_curvouobj.showmessagebox("Work order cannot save without BOM allocation",32,vumess)
						Return .F.
					Endif
				Else
					etsql_str  = "delete  from WKRMDET Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
					etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,"","_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
					If etsql_con<=0
						Return .F.
					Endif
				Endif
			Endif
		Endif

		_malias 	= Alias()
		_mrecno	= Recno()

		If ([vuexc] $ vchkprod)
			If(_curvouobj.addmode=.T. Or _curvouobj.editmode=.T.)
				If Type('_curvouobj.PCVTYPE')='C'
					If (Inlist(_curvouobj.pcvtype,'SS')) And Used('batchalloc_vw')
						Set DataSession To _Screen.ActiveForm.DataSessionId
						If (_curvouobj.editmode) &&Delete existing record from projectitref
							etsql_str  = "delete  from batch_alloc Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
							etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,"","_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
							If etsql_con <=0
								Return .F.
							Endif
						Endif

						Select batchalloc_vw
						If Reccount()>0 &&Insert records into batch_alloc
							Replace All tran_cd With main_vw.tran_cd In batchalloc_vw
							Scan
								msqlstr  =  _curvouobj.sqlconobj.geninsert ("batch_alloc","","","batchalloc_vw",mvu_backend)
								etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
							Endscan
						Endif

					Endif
				Endif
			Else &&Delete Button
				etsql_str  = "delete  from batch_alloc Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
				etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,"","_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
				If etsql_con <=0
					Return .F.
				Endif
			Endif
		Endif

		If ([vuexc] $ vchkprod) And Type('_curvouobj.PCVTYPE')='C'
			If(!_curvouobj.addmode And !_curvouobj.editmode)
				If _curvouobj.pcvtype="AR"
					etsql_str  = "select top 1 entry_ty from wkrmdet Where rTran_cd = ?Main_vw.Tran_cd And rEntry_ty = ?Main_vw.Entry_ty"
					etsql_con1 = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[_chkbom],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
					If Used('_chkbom')
						If Reccount()>0
							Select _chkbom
							=Messagebox("Entry Passed Against work order. Entry Can not be Deleted",16,vumess)
							Use In _chkbom
							Return .F.
						Endif
						Use In _chkbom
					Endif
				Endif
*!*				Else
*!*					If _curvouobj.pcvtype="IP"
*!*						If !Used("ProjectItRef_vw")
*!*							=Messagebox("Please select the batch from pickup button of the Item.",16,vumess)
*!*							etsql_con =0
*!*						Endif
*!*						If Used("ProjectItRef_vw")
*!*							Select item_vw
*!*							Scan
*!*								If Type('Item_vw.batchno')='C'
*!*									If(!Empty(item_vw.batchno))
*!*										Select projectitref_vw
*!*										Locate For entry_ty=item_vw.entry_ty And tran_cd=item_vw.tran_cd And itserial=item_vw.itserial
*!*										If !Found()
*!*											=Messagebox("Please select the batch from pickup button of the Item."+(item_vw.item),16,vumess)
*!*											etsql_con =0
*!*											exit
*!*										Endi
*!*									Endif
*!*								Endif
*!*								Select item_vw
*!*							Endscan
*!*						Endif
*!*					Endif
			Endif
		Endif

		If !Empty(_malias)
			Select &_malias
		Endif
		If Betw(_mrecno,1,Reccount())
			Go _mrecno
		Endif

	Endif
Else
&&Added & Commented By Kishor A to resolved whether Container_det tables Exsits Start	&& Shrikant S. on 25/01/2017 for GST
	etsql_str  = "SELECT * FROM SYS.objects WHERE NAME='BatchGenTbl'"
	etsql_con1 = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[_TblExists],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
	Select _TblExists
	If Reccount() >0
		etsql_str  = "delete from BatchGenTbl Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
		etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
	Endif
	If Used('_TblExists')
		Use In _TblExists
	Endif
&&Added & Commented By Kishor A to resolved whether Container_det tables Exsits End			&& Shrikant S. on 25/01/2017 for GST
Endif
&& Added by Shrikant S. on 30/06/2014 for Bug-23280		&& End

&& changes by EBS team on 07/03/14 for Bug-21466,21467,21468 Start
* Changes done as per --> CR_KOEL_0002_Export_Sales_Transaction_&_Export_LC_Master_Cross_Reference
* Changes done by EBS Product Team
_mexim = oglblprdfeat.udchkprod('exim')
_currObj = _Screen.ActiveForm
If _mexim
	If main_vw.entry_ty='SI'
		lOldArea = Select()
		Store .T. To isCheck
		Set Step On
		Do Case
		Case !Empty(main_vw.lc_no) And Upper(Alltrim(main_vw.party_nm)) = 'CANCELLED'
			isCheck = CancelPartyProc() && Executes When Transaction is Cancelled.
		Case !Empty(main_vw.lc_no) And (_currObj.addmode = .T. Or _currObj.editmode = .T.)
			isCheck = LCBalUpdt_AddEdtProc() && Updates LC Balance Amount in Add or Edit Mode
		Case !Empty(main_vw.lc_no) And (_currObj.addmode = .F. And _currObj.editmode = .F.)
			isCheck = LCBalUpdt_DeleteProc() && Updates LC Balance Amount in Delete Mode
		Case Empty(main_vw.lc_no) And (_currObj.addmode = .F. And _currObj.editmode = .T.)
			isCheck = RemoveLCProc() && Updates LC Balance Amount after Removing LC No.
		Endcase

		If isCheck = .F.
			Messagebox("Issues found in customization " +Chr(13) +;
				"Please refer Customization related this CR_KOEL_0002_Export_Sales_Transaction_&_Export_LC_Master_Cross_Reference",16,vumess)
		Endif

		If Used("cur_lcno")
			Use In cur_lcno
		Endif
		Wait Clear
* End --> CR_KOEL_0002_Export_Sales_Transaction_&_Export_LC_Master_Cross_Reference


* Changes done as per --> CR_KOEL_0005A_Form_To_Record_Pre_Shipment_Info
* Date : 08/11/2012
* Changes done by EBS Product Team


		Wait Window + 'Updating Pre Shipment Info. details, Please wait....' Nowait
		Store .T. To isSave
		Do Case
		Case _currObj.addmode = .T. Or _currObj.editmode = .T.
			isSave = ProcSave()
		Case _currObj.addmode = .F. And _currObj.editmode = .F.
			isSave = ProcDelete()
		Endcase
		Wait Clear

		If isSave = .F.
			Messagebox("Issues found in customization " +Chr(13) +;
				"Please refer Customization related this CR_KOEL_0005A_Form_To_Record_Pre_Shipment_Info",16,vumess)
		Endif

		If isSave = .T. And isCheck = .T.
&&Return .T. &&&& Added By Pankaj B. on 01-04-2015 for Bug-25746
		Else
			Return .F.
		Endif

		Select(lOldArea)
		Wait Clear
&&Return isCheck && && Commented By Pankaj B. on 01-04-2015 for Bug-25746
*End --> CR_KOEL_0005A_Form_To_Record_Pre_Shipment_Info
	Endif
* End ------->* Changes done by EBS Product Team
Endif
* End --> CR_KOEL_0002_Export_Sales_Transaction_&_Export_LC_Master_Cross_Reference

* EPCG Changes done as per --> PR_EPCG_00001_Import_Purchase_Duty_Saved_EO
* Changes done by EBS Product Team on 05022013
_mepcg = oglblprdfeat.udchkprod('epcg')
_meximaa = oglblprdfeat.udchkprod('exim_aa')
_curscrobj= _Screen.ActiveForm
If _mepcg
	If main_vw.entry_ty = 'P1' And !Empty(Iif(Type("main_vw.licen_no")<>"U",Alltrim(main_vw.licen_no),Alltrim(lmc_vw.licen_no)))
		lOldArea = Select()
		Store .T. To isCheck

		If _curscrobj.addmode = .T. Or _curscrobj.editmode = .T.
			If main_vw.Date < curepcgmast.issue_dt
				Messagebox("Selected license no. issue date should not be less than transaction date...",16,vumess)
				_curscrobj.EPCGctrlObjName1.Value = ""
				_curscrobj.EPCGctrlObjName1.SetFocus()
				Return .F.
			Endif

			If main_vw.Date > curepcgmast.expiry_dt
				Messagebox("Selected License no. expiry date cannot be Less than Transaction date..!!",16,vumess)
				_curscrobj.EPCGctrlObjName1.Value = ""
				_curscrobj.EPCGctrlObjName1.SetFocus()
				Return .F.
			Endif
			isCheck = EPCGAddEditProc()
		Else
			isCheck = EPCGDeleteProc()
		Endif

		If isCheck = .F.
			Return
		Endif
	Endif
Endif
* End ---> PR_EPCG_00001_Import_Purchase_Duty_Saved_EO

* AA Changes done as per --> PR_AA_00001_Import_Purchase_AA
* Changes done by EBS Product Team on 01012013
If _meximaa
	If Inlist(main_vw.entry_ty,"P1","EI") And !Empty(Iif(Type("main_vw.aalic_no")<>"U",Alltrim(main_vw.aalic_no),Alltrim(lmc_vw.aalic_no)))
		_curscrobj = _Screen.ActiveForm

*!*		MESSAGEBOX("main_vw.tot_examt = "+STR(main_vw.tot_examt,12,2)+CHR(13)+"_curscrobj.oTranItemDtySaved = "+STR(_curscrobj.oTranItemDtySaved,12,2)+;
*!*			CHR(13)+"_curscrobj.oTranDtySaved = "+STR(_curscrobj.oTranDtySaved,12,2)+;
*!*			CHR(13)+"main_vw.aa_duty = "+STR(IIF(TYPE("main_vw.aa_duty")<>"U",main_vw.aa_duty,lmc_vw.aa_duty),12,2))

*!*		MESSAGEBOX("main_vw.net_amt = "+STR(main_vw.net_amt,12,2)+CHR(13)+"_curscrobj.oTranItemInvAmt = "+STR(_curscrobj.oTranItemInvAmt,12,2)+;
*!*			CHR(13)+"_curscrobj.oTranTotInvAmt = "+STR(_curscrobj.oTranTotInvAmt,12,2)+;
*!*			CHR(13)+"main_vw.AA_INVAMT = "+STR(IIF(TYPE("main_vw.AA_INVAMT")<>"U",main_vw.aa_invamt,lmc_vw.aa_invamt),12,2)+;
*!*			CHR(13)+"_curscrobj.oAAEOInvAmt = "+STR(_curscrobj.oAAEOInvAmt,12,2)+;
*!*			CHR(13)+"curaamast.act_invamt = "+STR(curaamast.act_invamt,12,2))

*!*		CHR(13)+"EOInvAmt = "+STR(EOInvAmt,12,2)+
		lOldArea = Select()
		Store .T. To isCheck

		If _curscrobj.addmode = .T. Or _curscrobj.editmode = .T.
			If main_vw.Date < curaamast.issue_dt
				Messagebox("Selected License No. issue date cannot be greater than transaction date...!!",0+16,vumess)
				_curscrobj.AActrlObjName1.Value = ""
				_curscrobj.AActrlObjName1.SetFocus()
				Return .F.
			Endif

			If main_vw.Date > curaamast.expiry_dt
				Messagebox("Selected License No. expiry date cannot be less than transaction date...!!",0+16,vumess)
				_curscrobj.AActrlObjName1.Value = ""
				_curscrobj.AActrlObjName1.SetFocus()
				Return .F.
			Endif
			isCheck = AAAddEditProc()
		Else
			isCheck = AADeleteProc()
		Endif
		If isCheck = .F.
			Return
		Endif
	Endif
Endif
* End ---> PR_AA_00001_Import_Purchase_AA

&& Added by Shrikant S. on 13/01/2015 for Bug-24872		&& Start
_malias 	= Alias()
_mrecno	= Recno()

If  (([vuexc] $ vchkprod) Or ([vuinv] $ vchkprod)) And Type('_curvouobj.PCVTYPE')='C' &&Check Existing Records
	etsql_str  = "Select Top 1 dbname From Vudyog..Co_mast Where Sta_dt< (Select sta_dt from Vudyog..Co_mast where compid=?Company.CompId) and co_name=?Company.co_name and dbname<>?Company.dbname Order by sta_dt desc"
	etsql_con1 = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[chkprevrec],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
	If Used('chkprevrec')
		If Reccount('chkprevrec')>0
			Locate
			etsql_str  = "Delete From "+Alltrim(chkprevrec.dbname)+"..newyeartran Where nTran_cd = ?Main_vw.Tran_cd And nEntry_ty = ?Main_vw.Entry_ty"
			etsql_con1 = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
		Endif
		Use In chkprevrec
	Endif
Endif

If !Empty(_malias)
	Select &_malias
Endif
If Betw(_mrecno,1,Reccount())
	Go _mrecno
Endif
&& Added by Shrikant S. on 13/01/2015 for Bug-24872		&& End

&& Added By Pankaj B. on 01-04-2015 for Bug-25746 Start
_malias 	= Alias()
_mrecno	= Recno()

If(_curvouobj.addmode=.T. Or _curvouobj.editmode=.T.)
	If Type('_curvouobj.PCVTYPE')='C'
		If Used('Container_vw')
*Set DataSession To _curvouobj.DataSessionId
			If (_curvouobj.editmode) &&Delete existing record from udcost_center
				etsql_str  = "delete from Container_det Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
				etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
			Endif

			Select Container_vw
			If etsql_con > 0 &&Insert records into udcost_center
				Replace All tran_cd With main_vw.tran_cd In Container_vw
				Replace All entry_ty With main_vw.entry_ty In Container_vw
				Scan
					If etsql_con > 0
						etsql_str  = _curvouobj.sqlconobj.geninsert ("Container_det","","","Container_vw","")
						etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],;
							"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
						Select Container_vw
					Endif
				Endscan
			Endif
			Use In Container_vw
		Endif
	Endif
Else &&Delete Button
	etsql_str  = "delete from Container_det Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
	etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
Endif

If !Empty(_malias)
	Select &_malias
Endif
If Betw(_mrecno,1,Reccount())
	Go _mrecno
Endif
&& Added By Pankaj B. on 01-04-2015 for Bug-25746 End

&& Added By Kishor A. For Bug-26677 On 06/08/2015 Start.
_malias 	= Alias()
_mrecno	= Recno()

If(_curvouobj.addmode=.T. Or _curvouobj.editmode=.T.)
	If Type('_curvouobj.PCVTYPE')='C'
		If Used('PayTermsdet_vw')
			If (_curvouobj.editmode) &&Delete existing record from PayTermsdet
				etsql_str  = "delete from PayTermsdet Where tran_cd = ?Main_vw.tran_cd And Entry_ty = ?Main_vw.Entry_ty"
				etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
			Endif

			Select PayTermsdet_vw
			Replace All tran_cd With main_vw.tran_cd,entry_ty With main_vw.entry_ty In PayTermsdet_vw

			Select PayTermsdet_vw
			Scan
				If etsql_con > 0 &&Insert records into PayTermsdet
					etsql_str  = _curvouobj.sqlconobj.geninsert ("PayTermsdet","","","PayTermsdet_vw","")
					etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],;
						"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
				Endif
			Endscan
			Use In PayTermsdet_vw
		Endif
	Endif
Else &&Delete Button
	etsql_str  = "delete from PayTermsdet Where tran_cd = ?Main_vw.tran_cd And Entry_ty = ?Main_vw.Entry_ty"
	etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
Endif

If !Empty(_malias)
	Select &_malias
Endif
If Betw(_mrecno,1,Reccount())
	Go _mrecno
Endif

If Used("PayTerms_vw")
	Use In PayTerms_vw
Endif

If Used("PayTermsdet_vw")
	Use In PayTermsdet_vw
Endif

&& Added By Kishor A. For Bug-26677 On 06/08/2015 End.


&& Added by Shrikant S. on 05/10/2016 for GST 		&&Start
_malias 	= Alias()
_mrecno	= Recno()

_curvouobj = _Screen.ActiveForm
If(_curvouobj.addmode=.T. Or _curvouobj.editmode=.T.)
	If Used("GridCurs")
		Select entry_ty,Date,tran_cd,CGST_Act As ACGST,SGST_Act As ASGST,IGST_Act As AIGST,main_vw.entry_ty As Refentry_ty,main_vw.tran_cd As Reftran_cd From GridCurs ;
			INTO Cursor FinalGstCurs Readwrite Where Date #{} && And CGST_Act > 0 Or SGST_Act > 0 Or IGST_Act > 0

		If (_curvouobj.editmode)=.T.
			etsql_str  = "delete  from gstalloc Where RefTran_cd = ?Main_vw.Tran_cd And RefEntry_ty = ?Main_vw.Entry_ty"
			nretval = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,"","_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
			If nretval <=0
				Return .F.
			Endif
		Endif

		Select FinalGstCurs
		Go Top
		Scan
			Select FinalGstCurs
			lcSqlStr = _curvouobj .sqlconobj.geninsert([GSTALLOC],[],[],[FinalGstCurs],mvu_backend)
			lcSqlStr="set dateformat dmy "+lcSqlStr
			nretval = _curvouobj .sqlconobj.dataconn("EXE",company.dbname,lcSqlStr,"","thisform.nHandle",_curvouobj.DataSessionId,.T.)
			If nretval<=0 Or Empty(FinalGstCurs.Date)
				=Messagebox("Error in Update Statement. Please retry later",64,vumess)
				nRollback=_curvouobj .sqlconobj._sqlrollback("thisform.nHandle")
				If nRollback<=0
					=Messagebox("Not saved properly. Delete this entry and re-enter",64,vumess)
				Endif
				nretval=Thisform.sqlconobj.sqlconnclose("thisform.nHandle")
				Exit
			Endif
			Select FinalGstCurs
		Endscan
		If Used('FinalGstCurs')
			Use In FinalGstCurs
		Endif
	Endif
Else
	etsql_str  = "delete  from gstalloc Where RefTran_cd = ?Main_vw.Tran_cd And RefEntry_ty = ?Main_vw.Entry_ty"
	nretval = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
Endif
etsql_con =nretval

If !Empty(_malias)
	Select &_malias
Endif
If Betw(_mrecno,1,Reccount())
	Go _mrecno
Endif

&& Added by Shrikant S. on 05/10/2016 for GST 		&&End


&& Added by Kishor A. for Bug-28461 on 02-09-2016 Start


If _curvouobj.addmode Or _curvouobj.editmode
	_malias 	= Alias()
	_mrecno	= Recno()
	If _curvouobj.itempage And Inlist(main_vw.entry_ty,"AR","OS","PT","OP","ES")
		If !oglblindfeat.udchkind('pharmaind')
			If oglblprdfeat.udchkprod('exmfgbp')
				If Used('BatchTbl_Vw')
					Select BatchTbl_Vw
					Replace All tran_cd With main_vw.tran_cd,entry_ty With main_vw.entry_ty,;
						DATE With main_vw.Date,compid With main_vw.compid,l_yn With main_vw.l_yn In BatchTbl_Vw

					etsql_str  = "Select * from BatchGenTbl Where l_yn = ?main_vw.l_yn and Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
					etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[tmprunno_vw],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
					If etsql_con > 0 And Used('tmprunno_vw')
						Select tmprunno_vw
						Scan
							metdele = .F.
							Select item_vw
							If !Seek(main_vw.entry_ty+Dtos(main_vw.Date)+main_vw.doc_no+tmprunno_vw.itserial,'Item_vw','Eddits')
								metdele = .T.
							Else
								If Seek(main_vw.entry_ty+Dtos(main_vw.Date)+main_vw.doc_no+tmprunno_vw.itserial,'Item_vw','Eddits')
									If Empty(item_vw.batchno)
										metdele = .T.
									Endif
								Endif
							Endif
							If metdele = .T. And etsql_con > 0
								etsql_str = _curvouobj.sqlconobj.gendelete("BatchGenTbl","Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty And Itserial = ?tmprunno_vw.itserial")
								etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
							Else
								Select BatchTbl_Vw
								If Seek(tmprunno_vw.itserial,'BatchTbl_Vw','ItSerial')
									Replace batchno With '',runno With '' In BatchTbl_Vw
								Endif
							Endif
							Select tmprunno_vw
						Endscan
					Else
						etsql_con = 0
					Endif

					Select BatchTbl_Vw
					Scan
						mrecno=Recno()
						Select item_vw
						If !Seek(main_vw.entry_ty+Dtos(main_vw.Date)+main_vw.doc_no+BatchTbl_Vw.itserial,'Item_vw','Eddits')
							Replace batchno With '',runno With '' In BatchTbl_Vw
						Else
							If Seek(main_vw.entry_ty+Dtos(main_vw.Date)+main_vw.doc_no+BatchTbl_Vw.itserial,'Item_vw','Eddits')
								If Empty(item_vw.batchno)
									Replace batchno With '',runno With '' In BatchTbl_Vw
								Else
									Replace batchno With item_vw.batchno In BatchTbl_Vw
								Endif
							Endif
						Endif
						If etsql_con > 0 And !Empty(BatchTbl_Vw.runno)
*!*								Do While (CheckBatchNo(.T.,"_curvouobj.nHandle",_curvouobj)=.F.)		&& Commented by Shrikant S.on 17/05/2017 for GST
							Do While (CheckBatchNo(.T.,"_curvouobj.nHandle")=.F.)			&& Added by Shrikant S.on 17/05/2017 for GST
								Replace batchno With '' In item_vw
								newbatchno=GenBatchNo()
								If Betw(mrecno,1,Reccount())
									Go mrecno
									If _item.IsRunning=.T.
										If Alltrim(_item.BatchSfix)<>""
											_mrunno=Val(Left(Right(Alltrim(newbatchno),6+Len(Alltrim(BatchSfix))),6))
										Else
											_mrunno=Val(Right(Alltrim(newbatchno),6))
										Endif
									Else
										_mrunno=0
									Endif
									Replace batchno With newbatchno,runno With Str(_mrunno) In BatchTbl_Vw
									Replace batchno With newbatchno In item_vw
								Endif
							Enddo
							etsql_str  = _curvouobj.sqlconobj.geninsert("BatchGenTbl","","","BatchTbl_Vw",mvu_backend)
							etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
						Endif
						Select BatchTbl_Vw
					Endscan
*!*						Use In BatchTbl_Vw

					etsql_str  = "SELECT * FROM BatchGenTbl Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
					etsql_con1 = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[_TblBatchGen],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
					Select	_TblBatchGen
					Index On itserial+Str(it_code) Tag _ItserCode

					Select * From item_vw Into Cursor tmpItemvw_Vw
					Index On itserial+Str(it_code) Tag _ItserCode

					Select _TblBatchGen
					Scan
						Select _TblBatchGen
						lcitserial= itserial
						lcTran_cd=tran_cd
						lcEntry_ty=entry_ty
						lcIt_Code=Str(it_code)
						lcBatchNo=batchno
						Select tmpItemvw_Vw
						If Seek(lcitserial+lcIt_Code)
*!*								etsql_str  = "Update batchgentbl set batchno=?tmpItemvw_Vw.batchno Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty and Itserial=?Item_vw.Itserial and IT_code=?Item_vw.It_Code"		&& Commented by Shrikant S. on 17/05/2017 for GST
							etsql_str  = "Update "+_curvouobj.entry_tbl+"Item set batchno=?tmpItemvw_Vw.batchno Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty and Itserial=?Item_vw.Itserial and IT_code=?Item_vw.It_Code"			&& Added by Shrikant S. on 17/05/2017 for GST
							etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[_temppageno],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
						Endif
						Select _TblBatchGen
					Endscan

					If Used('tmpItemvw_Vw')
						Use In tmpItemvw_Vw
					Endif
					If Used('_TblBatchGen')
						Use In _TblBatchGen
					Endif
					If Used('_TblExists')
						Use In _TblExists
					Endif
				Else
					etsql_str  = "SELECT * FROM SYS.objects WHERE NAME='BatchGenTbl'"
					etsql_con1 = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[_TblExists],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
					Select _TblExists
					If Reccount() >0
						etsql_str  = "SELECT top 1 * FROM BatchGenTbl Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
						etsql_con1 = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[_TblBatchGen],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
						Select	_TblBatchGen
						Index On itserial+Str(it_code) Tag _ItserCode

&& Added by Shrikant S. on 17/05/2017 for GST		&& Start
						If etsql_con1 > 0 And Used('_TblBatchGen')
							Select _TblBatchGen
							If Reccount() > 0
								etsql_str = _curvouobj.sqlconobj.gendelete("BatchGenTbl","Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty")
								etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
							Endif
						Endif
&& Added by Shrikant S. on 17/05/2017 for GST		&& End

&& Commented by Shrikant S. on 17/05/2017 for GST 	&& Start
*!*							Select * From item_vw Into Cursor tmpItemvw_Vw
*!*							Index On itserial+Str(it_code) Tag _ItserCode
*!*							Select _TblBatchGen
*!*							Scan
*!*								Select _TblBatchGen
*!*								lcitserial= itserial
*!*								lcTran_cd=tran_cd
*!*								lcEntry_ty=entry_ty
*!*								lcIt_Code=Str(it_code)
*!*								Select tmpItemvw_Vw
*!*								If !Seek(lcitserial+lcIt_Code)
*!*									etsql_str  = "delete from BatchGenTbl Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty And ItSerial= ?lcitserial"
*!*									etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
*!*								Endif
*!*								Select _TblBatchGen
*!*							Endscan
&& Commented by Shrikant S. on 17/05/2017 for GST 	&& End

						If Used('tmpItemvw_Vw')
							Use In tmpItemvw_Vw
						Endif
						If Used('_TblBatchGen')
							Use In _TblBatchGen
						Endif
						If Used('_TblExists')
							Use In _TblExists
						Endif

					Endif
				Endif
			Endif
		Endif
	Endif
	If !Empty(_malias)
		Select &_malias
	Endif
	If Betw(_mrecno,1,Reccount())
		Go _mrecno
	Endif
	If Used('BatchTbl_Vw')
		Use In BatchTbl_Vw
	Endif

Else
	etsql_str  = "SELECT * FROM SYS.objects WHERE NAME='BatchGenTbl'"
	etsql_con1 = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[_TblExists],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
	Select _TblExists
	If Reccount() >0
		etsql_str  = "delete from BatchGenTbl Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
		etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
	Endif
	If Used('_TblExists')
		Use In _TblExists
	Endif
Endif
&& Added by Kishor A. for Bug-28461 on 02-09-2016 End





Return Iif(etsql_con < 1,.F.,.T.)			&& Added By Shrikant S. on 27/06/2014 for Bug-23280 && Please don't comment or remov the line and paste your code in above this line











* AA Changes done as per --> PR_AA_00001_Import_Purchase_AA
* Changes done by EBS Product Team on 01012013
Procedure AAAddEditProc
_curscrobj = _Screen.ActiveForm

If _curscrobj.addmode = .T. Or _curscrobj.editmode = .T.
	lcSqlStr = ""
	EOInvAmt = 0

* AA Changes done as per --> PR_AA_00001_Import_Purchase_AA
* Changes done by EBS Product Team on 29122012

	If main_vw.entry_ty = "P1"
* Changes done by EBS Product Team on 27022013
		Do Case
		Case (_curscrobj.addmode = .T. And _curscrobj.editmode = .F.) Or ((_curscrobj.addmode = .F. And _curscrobj.editmode = .T.);
				And (_curscrobj.oAATranDtySaved = 0 Or _curscrobj.oAATranTotInvAmt = 0))
			lcSqlStr = "Update aa_mast set act_duty = act_duty + " + Alltrim(Str(lmc_vw.aa_duty,12,2)) + ",act_invamt = act_invamt + " + Alltrim(Str(main_vw.aa_invamt,12,2)) +;
				" Where licen_no = '" + Alltrim(lmc_vw.aalic_no) + "'"

		Case (_curscrobj.addmode = .F. And _curscrobj.editmode = .T.) And (_curscrobj.oAATranDtySaved > 0 Or _curscrobj.oAATranTotInvAmt > 0)
&& Update old LC Balance amount, when user changes LC no. in Edit mode.

			Do Case
			Case !Empty(_curscrobj.oAALicenseNo) And Alltrim(_curscrobj.oAALicenseNo) <> Alltrim(lmc_vw.aalic_no);
					And  (_curscrobj.oAATranDtySaved = main_vw.tot_examt Or _curscrobj.oAATranTotInvAmt = main_vw.net_amt)
				lcSqlStr = " Update aa_mast set act_duty = act_duty - " + Alltrim(Str(main_vw.tot_examt,12,2)) + ",act_invamt = act_invamt - " + Alltrim(Str(main_vw.net_amt,12,2)) +;
					" Where licen_no = '" + Alltrim(_curscrobj.oAALicenseNo) + "'"

				lcSqlStr = lcSqlStr + " Update aa_mast set act_duty = act_duty + " + Alltrim(Str(main_vw.tot_examt,12,2)) + ",act_invamt = act_invamt + " + Alltrim(Str(main_vw.net_amt,12,2)) +;
					" Where licen_no = '" + Alltrim(lmc_vw.aalic_no) + "'"

			Case !Empty(_curscrobj.oAALicenseNo) And Alltrim(_curscrobj.oAALicenseNo) = Alltrim(lmc_vw.aalic_no) And (_curscrobj.oAATranItemDtySaved <> main_vw.tot_examt Or _curscrobj.oAATranItemInvAmt <> main_vw.net_amt)
				lcSqlStr = " Update aa_mast set act_duty = act_duty - " + Alltrim(Str(_curscrobj.oAATranItemDtySaved,12,2)) + ",act_invamt = act_invamt - " + Alltrim(Str(_curscrobj.oAATranItemInvAmt,12,2)) +;
					" Where licen_no = '" + Alltrim(lmc_vw.aalic_no) + "'"

				lcSqlStr = lcSqlStr + " Update aa_mast set act_duty = act_duty + " + Alltrim(Str(main_vw.tot_examt,11,2)) + ",act_invamt = act_invamt + " + Alltrim(Str(main_vw.net_amt,11,2)) +;
					" Where licen_no = '" + Alltrim(lmc_vw.aalic_no) + "'"

			Case !Empty(_curscrobj.oAALicenseNo) And Alltrim(_curscrobj.oAALicenseNo) <> Alltrim(lmc_vw.aalic_no) And (_curscrobj.oAATranDtySaved <> main_vw.tot_examt Or _curscrobj.oAATranTotInvAmt <> main_vw.net_amt)
				lcSqlStr = " Update aa_mast set act_duty = act_duty - " + Alltrim(Str(_curscrobj.oAATranDtySaved,12,2)) + ",act_invamt = act_invamt - " + Alltrim(Str(_curscrobj.oAATranTotInvAmt,12,2)) +;
					" Where licen_no = '" + Alltrim(_curscrobj.oAALicenseNo) + "'"

				lcSqlStr = lcSqlStr + " Update aa_mast set act_duty = act_duty + " + Alltrim(Str(main_vw.tot_examt,12,2)) + ",act_invamt = act_invamt + " + Alltrim(Str(main_vw.net_amt,12,2)) +;
					" Where licen_no = '" + Alltrim(lmc_vw.aalic_no) + "'"

			Endcase
		Endcase

*!*			Messagebox("_curscrobj.oAAEOInvAmt = "+Str(_curscrobj.oAAEOInvAmt,12,2)+;
*!*				CHR(13)+"main_vw.aa_eoinvamt = "+Str(main_vw.aa_eoinvamt,12,2)+;
*!*				CHR(13)+"curaamast.eo_invamt = "+Str(curaamast.eo_invamt,12,2))
		Do Case
		Case (_curscrobj.addmode = .T. And _curscrobj.editmode = .F.) Or ((_curscrobj.addmode = .F. And _curscrobj.editmode = .T.) And _curscrobj.oAAEOInvAmt = 0)
*!*				Wait Window "Case 1"
			EOInvAmt = curaamast.eo_invamt + main_vw.gro_amt + (main_vw.gro_amt * curaamast.va_per / 100)
			lcSqlStr = lcSqlStr + " Update aa_mast set eo_invamt = " + Alltrim(Str(EOInvAmt,12,2)) + ",act_duty_fc = act_duty * " + Alltrim(Str(main_vw.fcexrate,12,2)) + ;
				", fcid = " + Alltrim(Str(main_vw.fcid)) + " Where licen_no = '" + Alltrim(lmc_vw.aalic_no) + "'"

		Case (_curscrobj.addmode = .F. And _curscrobj.editmode = .T.) And _curscrobj.oAAEOInvAmt > 0
*!*				Wait Window "Case 2"
			Do Case
			Case !Empty(_curscrobj.oAALicenseNo) And Alltrim(_curscrobj.oAALicenseNo) <> Alltrim(lmc_vw.aalic_no) And  _curscrobj.oAAEOInvAmt = main_vw.aa_eoinvamt
*!*					Wait Window "Case 2.1"
				lcSqlStr = lcSqlStr + " Update aa_mast set eo_invamt = eo_invamt - " + Alltrim(Str(main_vw.aa_eoinvamt,12,2)) + ",act_duty_fc = act_duty * " + Alltrim(Str(main_vw.fcexrate,12,2)) + ;
					", fcid = " + Alltrim(Str(main_vw.fcid)) + " Where licen_no = '" + Alltrim(_curscrobj.oAALicenseNo) + "'"

				lcSqlStr = lcSqlStr + " Update aa_mast set eo_invamt = eo_invamt + " + Alltrim(Str(main_vw.aa_eoinvamt,12,2)) + ",act_duty_fc = act_duty * " + Alltrim(Str(main_vw.fcexrate,12,2)) + ;
					", fcid = " + Alltrim(Str(main_vw.fcid)) + " Where licen_no = '" + Alltrim(lmc_vw.aalic_no) + "'"

			Case !Empty(_curscrobj.oAALicenseNo) And Alltrim(_curscrobj.oAALicenseNo) <> Alltrim(lmc_vw.aalic_no) And  _curscrobj.oAAEOInvAmt <> main_vw.aa_eoinvamt
*!*					Wait Window "Case 2.2"
				lcSqlStr = lcSqlStr + " Update aa_mast set eo_invamt = eo_invamt - " + Alltrim(Str(_curscrobj.oAAEOInvAmt,12,2)) + ",act_duty_fc = act_duty * " + Alltrim(Str(main_vw.fcexrate,12,2)) + ;
					", fcid = " + Alltrim(Str(main_vw.fcid)) + " Where licen_no = '" + Alltrim(_curscrobj.oAALicenseNo) + "'"

				lcSqlStr = lcSqlStr + " Update aa_mast set eo_invamt = eo_invamt + " + Alltrim(Str(main_vw.aa_eoinvamt,12,2)) + ",act_duty_fc = act_duty * " + Alltrim(Str(main_vw.fcexrate,12,2)) + ;
					", fcid = " + Alltrim(Str(main_vw.fcid)) + " Where licen_no = '" + Alltrim(lmc_vw.aalic_no) + "'"

			Case !Empty(_curscrobj.oAALicenseNo) And Alltrim(_curscrobj.oAALicenseNo) = Alltrim(lmc_vw.aalic_no) And  _curscrobj.oAAEOInvAmt <> main_vw.aa_eoinvamt
*!*					Wait Window "Case 2.3"
				lcSqlStr = lcSqlStr + " Update aa_mast set eo_invamt = eo_invamt - " + Alltrim(Str(_curscrobj.oAAEOInvAmt,12,2)) + ",act_duty_fc = act_duty * " + Alltrim(Str(main_vw.fcexrate,12,2)) + ;
					", fcid = " + Alltrim(Str(main_vw.fcid)) + " Where licen_no = '" + Alltrim(_curscrobj.oAALicenseNo) + "'"

				lcSqlStr = lcSqlStr + " Update aa_mast set eo_invamt = eo_invamt + " + Alltrim(Str(main_vw.aa_eoinvamt,12,2)) + ",act_duty_fc = act_duty * " + Alltrim(Str(main_vw.fcexrate,12,2)) + ;
					", fcid = " + Alltrim(Str(main_vw.fcid)) + " Where licen_no = '" + Alltrim(lmc_vw.aalic_no) + "'"

			Case !Empty(_curscrobj.oAALicenseNo) And Alltrim(_curscrobj.oAALicenseNo) = Alltrim(lmc_vw.aalic_no) And  _curscrobj.oAAEOInvAmt = main_vw.aa_eoinvamt
*!*					Wait Window "Case 2.4"
				lcSqlStr = lcSqlStr + " Update aa_mast set eo_invamt = " + Alltrim(Str(main_vw.aa_eoinvamt,12,2)) + ",act_duty_fc = act_duty * " + Alltrim(Str(main_vw.fcexrate,12,2)) + ;
					", fcid = " + Alltrim(Str(main_vw.fcid)) + " Where licen_no = '" + Alltrim(lmc_vw.aalic_no) + "'"

			Endcase
		Endcase
* End ---> PR_AA_00001_Import_Purchase_AA
	Else
		If main_vw.entry_ty = "EI"
			Do Case
			Case (_curscrobj.addmode = .T. And _curscrobj.editmode = .F.) Or ((_curscrobj.addmode = .F. And _curscrobj.editmode = .T.) And _curscrobj.oAATranTotInvAmt = 0)
				lcSqlStr = "Update aa_mast set exp_invamt = exp_invamt + " + Alltrim(Str(lmc_vw.aa_invamt,12,2)) +;
					" Where licen_no = '" + Alltrim(lmc_vw.aalic_no) + "'"

			Case (_curscrobj.addmode = .F. And _curscrobj.editmode = .T.) And _curscrobj.oAATranTotInvAmt > 0
&& Update old LC Balance amount, when user changes LC no. in Edit mode.

				Do Case
				Case !Empty(_curscrobj.oAALicenseNo) And Alltrim(_curscrobj.oAALicenseNo) <> Alltrim(lmc_vw.aalic_no) And  _curscrobj.oAATranTotInvAmt <> main_vw.net_amt
					lcSqlStr = " Update aa_mast set exp_invamt = exp_invamt - " + Alltrim(Str(_curscrobj.oAATranTotInvAmt,12,2)) +;
						" Where licen_no = '" + Alltrim(_curscrobj.oAALicenseNo) + "'"

					lcSqlStr = lcSqlStr + " Update aa_mast set exp_invamt = exp_invamt + " + Alltrim(Str(main_vw.net_amt,12,2)) +;
						" Where licen_no = '" + Alltrim(lmc_vw.aalic_no) + "'"

				Case !Empty(_curscrobj.oAALicenseNo) And Alltrim(_curscrobj.oAALicenseNo) = Alltrim(lmc_vw.aalic_no) And  _curscrobj.oAATranItemInvAmt <> main_vw.net_amt
					lcSqlStr = " Update aa_mast set exp_invamt = exp_invamt - " + Alltrim(Str(_curscrobj.oAATranItemInvAmt,12,2)) +;
						" Where licen_no = '" + Alltrim(lmc_vw.aalic_no) + "'"

					lcSqlStr = lcSqlStr + " Update aa_mast set exp_invamt = exp_invamt + " + Alltrim(Str(main_vw.net_amt,11,2)) +;
						" Where licen_no = '" + Alltrim(lmc_vw.aalic_no) + "'"

				Case !Empty(_curscrobj.oAALicenseNo) And Alltrim(_curscrobj.oAALicenseNo) <> Alltrim(lmc_vw.aalic_no) And  _curscrobj.oAATranTotInvAmt = main_vw.net_amt
					lcSqlStr = " Update aa_mast set exp_invamt = exp_invamt - " + Alltrim(Str(main_vw.net_amt,12,2)) +;
						" Where licen_no = '" + Alltrim(_curscrobj.oAALicenseNo) + "'"

					lcSqlStr = lcSqlStr + " Update aa_mast set exp_invamt = exp_invamt + " + Alltrim(Str(main_vw.net_amt,12,2)) +;
						" Where licen_no = '" + Alltrim(lmc_vw.aalic_no) + "'"
				Endcase
			Endcase
		Endif
	Endif
* Changes done by EBS Product Team on 27022013
* End ---> PR_AA_00002_Export_Sales_EO_AA

	If !Empty(lcSqlStr)
*!*			IF main_vw.entry_ty = "P1"
*!*				lcSqlStr = lcSqlStr + " Update aa_mast set act_duty_fc = act_duty * " + STR(main_vw.fcexrate,12,2) + ;
*!*					", fcid = " + STR(main_vw.fcid) + " where licen_no = '" + IIF(TYPE("main_vw.aalic_no")<>"U",ALLTRIM(main_vw.aalic_no),ALLTRIM(lmc_vw.aalic_no)) + "'"
*!*			ENDIF
		nretval = _curscrobj.sqlconobj.dataconn([EXE],company.dbname,lcSqlStr,"","_curscrobj.nHandle",_curscrobj.DataSessionId,.T.)
		If nretval < 0
			nretval = _curscrobj.sqlconobj.sqlconnclose("_curscrobj.nHandle")
			Select(lOldArea)
			Return .F.
		Endif
	Endif
Endif
Return .T.
Endproc

Procedure AADeleteProc
_curscrobj = _Screen.ActiveForm

lcSqlStr = ""
* AA Changes done as per --> PR_AA_00001_Import_Purchase_AA
* Changes done by EBS Product Team on 29122012

If main_vw.entry_ty = "P1"
* Changes done by EBS Product Team on 27022013
	If Type("main_vw.aa_duty") <> 'U' And Type("main_vw.aalic_no") <> 'U' And Type("main_vw.aa_invamt") <> 'U'
		lcSqlStr = "Update aa_mast set act_duty = act_duty - " + Alltrim(Str(main_vw.tot_examt,12,2)) + ;
			",act_invamt = act_invamt - " + Alltrim(Str(main_vw.net_amt,12,2)) + ", eo_invamt = eo_invamt - " + Alltrim(Str(main_vw.aa_eoinvamt,12,2))+;
			" where licen_no = '" + Alltrim(main_vw.aalic_no) + "'"
	Else
		If Type("lmc_vw.aa_duty") <> 'U' And Type("lmc_vw.aalic_no") <> 'U' And Type("main_vw.aa_invamt") <> 'U'
			lcSqlStr = "Update aa_mast set act_duty = act_duty - " + Alltrim(Str(main_vw.tot_examt,12,2)) + ;
				",act_invamt = act_invamt - " + Alltrim(Str(main_vw.net_amt,12,2)) + ", eo_invamt = eo_invamt - " + Alltrim(Str(main_vw.aa_eoinvamt,12,2))+;
				" where licen_no = '" + Alltrim(lmc_vw.aalic_no) + "'"
		Endif
	Endif
* Changes done by EBS Product Team on 27022013
* End ---> PR_AA_00001_Import_Purchase_AA
Else

* AA Changes done as per --> PR_AA_00002_Export_Sales_EO_AA
* Changes done by EBS Product Team on 04022013

	If main_vw.entry_ty = "EI"
* Changes done by EBS Product Team on 27022013
		If Type("main_vw.aa_invamt") <> 'U' And Type("main_vw.aalic_no") <> 'U'
			lcSqlStr = "Update aa_mast set exp_invamt = exp_invamt - " + Alltrim(Str(main_vw.net_amt,12,2)) +;
				" where licen_no = '" + Alltrim(main_vw.aalic_no) + "'"
		Else
			If Type("lmc_vw.aa_invamt") <> 'U' And Type("lmc_vw.aalic_no") <> 'U'
				lcSqlStr = "Update aa_mast set exp_invamt = exp_invamt - " + Alltrim(Str(main_vw.net_amt,12,2)) +;
					" where licen_no = '" + Alltrim(lmc_vw.aalic_no) + "'"
			Endif
		Endif
	Endif
* Changes done by EBS Product Team on 27022013
* End ---> PR_AA_00002_Export_Sales_EO_AA
Endif
If !Empty(lcSqlStr)
	nretval = _curscrobj.sqlconobj.dataconn([EXE],company.dbname,lcSqlStr,"","_curscrobj.nHandle",_curscrobj.DataSessionId,.T.)
	If nretval < 0
		nretval = _curscrobj.sqlconobj.sqlconnclose("_curscrobj.nHandle")
		Select(lOldArea)
		Return .F.
	Endif
Endif
Return .T.
Endproc

* End ---> PR_AA_00001_Import_Purchase_AA

* EPCG Changes done as per --> PR_EPCG_00001_Import_Purchase_Duty_Saved_EO
* Changes done by EBS Product Team on 05022013
Procedure EPCGAddEditProc
_curscrobj = _Screen.ActiveForm
If _curscrobj.addmode = .T. Or _curscrobj.editmode = .T.
	lcSqlStr = ""
	Store 0 To mDutySaved,mTotEO
	Select Sum(basic_eo) As sumbasiceo,Sum(duty_saved) As sumdutysaved From item_vw With (Buffering = .T.) ;
		INTO Cursor cursumitem_vw

*!*		MESSAGEBOX("sumbasiceo = "+STR(cursumitem_vw.sumbasiceo,12,2)+CHR(13)+"sumdutysaved = "+STR(cursumitem_vw.sumdutysaved,12,2)+;
*!*			CHR(13)+"_curscrobj.oTranDtySaved = "+STR(_curscrobj.oTranDtySaved,12,2)+;
*!*			CHR(13)+"lmc_vw.duty_saved = "+STR(IIF(TYPE("main_vw.duty_saved")<>"U",main_vw.duty_saved,lmc_vw.duty_saved),12,2)+;
*!*			CHR(13)+"lmc_vw.tot_eo = "+STR(IIF(TYPE("main_vw.tot_eo")<>"U",main_vw.tot_eo,lmc_vw.tot_eo),12,2)+;
*!*			CHR(13)+"_curscrobj.oTranItemDtySaved = "+STR(_curscrobj.oTranItemDtySaved,12,2)+;
*!*			CHR(13)+"_curscrobj.oTranItemTotEO = "+STR(_curscrobj.oTranItemTotEO,12,2)+;
*!*			CHR(13)+"_curscrobj.oTranTotEO = "+STR(_curscrobj.oTranTotEO,12,2))

	Do Case
	Case (_curscrobj.addmode = .T. And _curscrobj.editmode = .F.) Or ((_curscrobj.addmode = .F. And _curscrobj.editmode = .T.) ;
			AND _curscrobj.oEPCGTranDtySaved = 0 And _curscrobj.oEPCGTranTotEO = 0)

		lcSqlStr = "Update epcg_mast set duty_saved = duty_saved + " + Alltrim(Str(lmc_vw.duty_saved,12,2)) +;
			", tot_eo = tot_eo + " + Alltrim(Str(lmc_vw.tot_eo,12,2)) + " Where licen_no = '" + Alltrim(lmc_vw.licen_no) + "'"

	Case (_curscrobj.addmode = .F. And _curscrobj.editmode = .T.) And _curscrobj.oEPCGTranDtySaved > 0 And _curscrobj.oEPCGTranTotEO > 0

		Do Case
		Case !Empty(_curscrobj.oEPCGLicenseNo) And Alltrim(_curscrobj.oEPCGLicenseNo) <> Alltrim(lmc_vw.licen_no) ;
				AND _curscrobj.oEPCGTranDtySaved = cursumitem_vw.sumdutysaved And _curscrobj.oEPCGTranTotEO = cursumitem_vw.sumbasiceo

			lcSqlStr = " Update epcg_mast set duty_saved = duty_saved - " + Alltrim(Str(lmc_vw.duty_saved,12,2)) +;
				", tot_eo = tot_eo - " + Alltrim(Str(lmc_vw.tot_eo,12,2)) + " Where licen_no = '" + Alltrim(_curscrobj.oEPCGLicenseNo) + "'"

			lcSqlStr = lcSqlStr + " Update epcg_mast set duty_saved = duty_saved + " + Alltrim(Str(lmc_vw.duty_saved,12,2)) +;
				", tot_eo = tot_eo + " + Alltrim(Str(lmc_vw.tot_eo,12,2)) + " Where licen_no = '" + Alltrim(lmc_vw.licen_no) + "'"

		Case !Empty(_curscrobj.oEPCGLicenseNo) And Alltrim(_curscrobj.oEPCGLicenseNo) = Alltrim(lmc_vw.licen_no) ;
				AND _curscrobj.oEPCGTranItemDtySaved <> cursumitem_vw.sumdutysaved And _curscrobj.oEPCGTranItemTotEO <> cursumitem_vw.sumbasiceo

			lcSqlStr = " Update epcg_mast set duty_saved = duty_saved - " + Alltrim(Str(_curscrobj.oEPCGTranItemDtySaved,12,2)) +;
				", tot_eo = tot_eo - " + Alltrim(Str(_curscrobj.oEPCGTranItemTotEO,12,2)) + " Where licen_no = '" + Alltrim(lmc_vw.licen_no) + "'"

			lcSqlStr = lcSqlStr + " Update epcg_mast set duty_saved = duty_saved + " + Alltrim(Str(lmc_vw.duty_saved,11,2)) +;
				", tot_eo = tot_eo + " + Alltrim(Str(lmc_vw.tot_eo,12,2)) + " Where licen_no = '" + Alltrim(lmc_vw.licen_no) + "'"

		Case !Empty(_curscrobj.oEPCGLicenseNo) And Alltrim(_curscrobj.oEPCGLicenseNo) <> Alltrim(lmc_vw.licen_no) ;
				AND _curscrobj.oEPCGTranDtySaved <> cursumitem_vw.sumdutysaved And _curscrobj.oEPCGTranTotEO <> cursumitem_vw.sumbasiceo

			lcSqlStr = " Update epcg_mast set duty_saved = duty_saved - " + Alltrim(Str(_curscrobj.oEPCGTranDtySaved,12,2)) +;
				", tot_eo = tot_eo - " + Alltrim(Str(_curscrobj.oEPCGTranTotEO,12,2)) + " Where licen_no = '" + Alltrim(_curscrobj.oEPCGLicenseNo) + "'"

			lcSqlStr = lcSqlStr + " Update epcg_mast set duty_saved = duty_saved + " + Alltrim(Str(lmc_vw.duty_saved,12,2)) +;
				", tot_eo = tot_eo + " + Alltrim(Str(lmc_vw.tot_eo,12,2)) + " Where licen_no = '" + Alltrim(lmc_vw.licen_no) + "'"

		Endcase
	Endcase

	If !Empty(lcSqlStr)
		lcSqlStr = lcSqlStr + " Update epcg_mast set duty_saved_fc = duty_saved * " + Alltrim(Str(main_vw.fcexrate,12,2)) + ;
			", tot_eo_fc = tot_eo * " + Alltrim(Str(main_vw.fcexrate,12,2)) + ", fcid = " + Alltrim(Str(main_vw.fcid)) +;
			" where licen_no = '" + Alltrim(lmc_vw.licen_no) + "'"
	Endif

	nretval = _curscrobj.sqlconobj.dataconn([EXE],company.dbname,lcSqlStr,"","_curscrobj.nHandle",_curscrobj.DataSessionId,.T.)
	If nretval < 0
		nretval = _curscrobj.sqlconobj.sqlconnclose("_curscrobj.nHandle")
		Select(lOldArea)
		Return .F.
	Endif
Endif

Return .T.
Endproc

Procedure EPCGDeleteProc
_curscrobj = _Screen.ActiveForm
lcSqlStr = ""

If !Used("cursumitem_vw")
	Select Sum(basic_eo) As sumbasiceo,Sum(duty_saved) As sumdutysaved From item_vw With (Buffering = .T.) ;
		INTO Cursor cursumitem_vw
Endif

If Type("main_vw.tot_eo") <> 'U' And Type("main_vw.duty_saved") <> 'U' And Type("main_vw.licen_no") <> 'U'
	lcSqlStr = "update epcg_mast set duty_saved = duty_saved - " + Alltrim(Str(cursumitem_vw.sumdutysaved,12,2)) +;
		", tot_eo = tot_eo - " + Alltrim(Str(cursumitem_vw.sumbasiceo,12,2)) + " where licen_no = '" + Alltrim(main_vw.licen_no) + "'"
Else
	If Type("lmc_vw.tot_eo") <> 'U' And Type("lmc_vw.duty_saved") <> 'U' And Type("lmc_vw.licen_no") <> 'U'
		lcSqlStr = "update epcg_mast set duty_saved = duty_saved -" + Alltrim(Str(cursumitem_vw.sumdutysaved,12,2)) +;
			", tot_eo = tot_eo -" + Alltrim(Str(cursumitem_vw.sumbasiceo,12,2)) + " where licen_no = '" + Alltrim(lmc_vw.licen_no) + "'"
	Endif
Endif

nretval = _curscrobj.sqlconobj.dataconn([EXE],company.dbname,lcSqlStr,"","_curscrobj.nHandle",_curscrobj.DataSessionId,.T.)
If nretval < 0
	nretval = _curscrobj.sqlconobj.sqlconnclose("_curscrobj.nHandle")
	Select(lOldArea)
	Return .F.
Endif
Return .T.
Endproc

* End ---> PR_EPCG_00001_Import_Purchase_Duty_Saved_EO

* Changes done as per --> CR_KOEL_0002_Export_Sales_Transaction_&_Export_LC_Master_Cross_Reference
* Changes done by EBS Product Team
Procedure CancelPartyProc
_currObj = _Screen.ActiveForm
lcSqlStr = "Update Export_LC_Mast set LC_BalAmt = LC_BalAmt + " + Alltrim(Str(_currObj.oPrevInvAmt,11,2)) +;
	" Where LC_No = '" + Alltrim(_currObj.oLCNo) + "'"

nretval = _currObj.sqlconobj.dataconn([EXE],company.dbname,lcSqlStr,"","_currObj.nHandle",_currObj.DataSessionId,.T.)
If nretval < 0
	nretval = _currObj.sqlconobj.sqlconnclose("_currObj.nHandle")
	Select(lOldArea)
	Return .F.
Endif
Return .T.
Endproc

Procedure LCBalUpdt_AddEdtProc
_currObj = _Screen.ActiveForm
If _currObj.addmode = .T. Or _currObj.editmode = .T.
&& Get LC Balance Details for checking
	Wait Window "Checking LC Balance Amount..." Nowait
	If Type("main_vw.LC_No") <> 'U'
		lcSqlStr = "Select LC_No, LC_BalAmt from export_lc_mast where lc_no=?main_vw.lc_no"
	Else
		If Type("lmc_vw.LC_No") <> 'U'
			lcSqlStr = "Select LC_No, LC_BalAmt from export_lc_mast where lc_no=?lmc_vw.lc_no"
		Endif
	Endif

	nretval = _currObj.sqlconobj.dataconn([EXE],company.dbname,lcSqlStr,"cur_lcno","_currObj.nHandle",_currObj.DataSessionId)
	If nretval < 0
		nretval = _currObj.sqlconobj.sqlconnclose("_currObj.nHandle")
		Select(lOldArea)
		Return .F.
	Endif

	Store .T. To lcheck
	If _currObj.addmode = .T.
		If main_vw.fcnet_amt > 0
			If (cur_lcno.LC_BalAmt < main_vw.fcnet_amt)
				lcheck = .F.
			Endif
		Endif
	Else
		If _currObj.editmode = .T.
			Store 0 To totBal
			If _currObj.oPrevInvAmt > 0
				Do Case
				Case !Empty(_currObj.oLCNo) And Alltrim(_currObj.oLCNo) <> Alltrim(main_vw.lc_no)
					totBal = cur_lcno.LC_BalAmt

				Case Empty(_currObj.oLCNo) And Alltrim(_currObj.oLCNo) <> Alltrim(main_vw.lc_no)
					totBal = cur_lcno.LC_BalAmt
				Otherwise
					totBal = cur_lcno.LC_BalAmt + _currObj.oPrevInvAmt
				Endcase
				If main_vw.fcnet_amt <> _currObj.oPrevInvAmt
					If (totBal < main_vw.fcnet_amt)
						lcheck = .F.
					Endif
				Else
					If (totBal < main_vw.fcnet_amt)
						lcheck = .F.
					Endif
				Endif
			Else
				If (cur_lcno.LC_BalAmt < main_vw.fcnet_amt)
					lcheck = .F.
				Endif
			Endif
		Endif
	Endif

	If lcheck = .F.
		Messagebox("Invoice Amount found more than LC Balance Amount."+Chr(13)+"Transaction could not be saved....",0+64,vumess)
		Select(lOldArea)
		Return .F.
	Endif

	Wait Window "Updating LC Balance Amount..." Nowait
	lcSqlStr = ""
	Do Case
	Case (_currObj.addmode = .T. And _currObj.editmode = .F.) Or ;
			((_currObj.addmode = .F. And _currObj.editmode = .T.) And _currObj.oPrevInvAmt = 0)
		lcSqlStr = " Update Export_LC_Mast set LC_BalAmt = LC_BalAmt - " + Alltrim(Str(main_vw.fcnet_amt,11,2)) +;
			" Where LC_No = '" + Alltrim(main_vw.lc_no) + "'"

	Case (_currObj.addmode = .F. And _currObj.editmode = .T.) And _currObj.oPrevInvAmt > 0
&& Update old LC Balance amount, when user changes LC no. in Edit mode.
		Do Case
		Case !Empty(_currObj.oLCNo) And Alltrim(_currObj.oLCNo) <> Alltrim(main_vw.lc_no) And  _currObj.oPrevInvAmt <> main_vw.fcnet_amt
			lcSqlStr = " Update Export_Lc_Mast set LC_BalAmt = LC_BalAmt + " + Alltrim(Str(_currObj.oPrevInvAmt,11,2)) +;
				" Where LC_No = '" + Alltrim(_currObj.oLCNo) + "'"

			lcSqlStr = lcSqlStr + " Update Export_Lc_Mast set LC_BalAmt = LC_BalAmt - " + Alltrim(Str(main_vw.fcnet_amt,11,2)) +;
				" Where LC_No = '" + Alltrim(main_vw.lc_no) + "'"

		Case !Empty(_currObj.oLCNo) And Alltrim(_currObj.oLCNo) = Alltrim(main_vw.lc_no) And  _currObj.oPrevInvAmt <> main_vw.fcnet_amt

			lcSqlStr = " Update Export_Lc_Mast set LC_BalAmt = LC_BalAmt + " + Alltrim(Str(_currObj.oPrevInvAmt,11,2)) +;
				" Where LC_No = '" + Alltrim(main_vw.lc_no) + "'"

			lcSqlStr = lcSqlStr + " Update Export_Lc_Mast set LC_BalAmt = LC_BalAmt - " + Alltrim(Str(main_vw.fcnet_amt,11,2)) +;
				" Where LC_No = '" + Alltrim(main_vw.lc_no) + "'"

		Case Empty(_currObj.oLCNo) And (_currObj.oPrevInvAmt = main_vw.fcnet_amt Or _currObj.oPrevInvAmt <> main_vw.fcnet_amt)
			lcSqlStr = lcSqlStr + " Update Export_Lc_Mast set LC_BalAmt = LC_BalAmt - " + Alltrim(Str(main_vw.fcnet_amt,11,2)) +;
				" Where LC_No = '" + Alltrim(main_vw.lc_no) + "'"

		Case !Empty(_currObj.oLCNo) And Alltrim(_currObj.oLCNo) <> Alltrim(main_vw.lc_no) And  _currObj.oPrevInvAmt = main_vw.fcnet_amt
			lcSqlStr = " Update Export_Lc_Mast set LC_BalAmt = LC_BalAmt + " + Alltrim(Str(_currObj.oPrevInvAmt,11,2)) +;
				" Where LC_No = '" + Alltrim(_currObj.oLCNo) + "'"

			lcSqlStr = lcSqlStr + " Update Export_Lc_Mast set LC_BalAmt = LC_BalAmt - " + Alltrim(Str(main_vw.fcnet_amt,11,2)) +;
				" Where LC_No = '" + Alltrim(main_vw.lc_no) + "'"

		Otherwise
			lcSqlStr = " Update Export_Lc_Mast set LC_BalAmt = LC_BalAmt + " + Alltrim(Str(_currObj.oPrevInvAmt,11,2)) +;
				" Where LC_No = '" + Alltrim(main_vw.lc_no) + "'"

			lcSqlStr = lcSqlStr + " Update Export_Lc_Mast set LC_BalAmt = LC_BalAmt - " + Alltrim(Str(main_vw.fcnet_amt,11,2)) +;
				" Where LC_No = '" + Alltrim(main_vw.lc_no) + "'"
		Endcase

	Endcase

	nretval = _currObj.sqlconobj.dataconn([EXE],company.dbname,lcSqlStr,"","_currObj.nHandle",_currObj.DataSessionId,.T.)
	If nretval < 0
		nretval = _currObj.sqlconobj.sqlconnclose("_currObj.nHandle")
		Select(lOldArea)
		Return .F.
	Endif
Endif
Return .T.
Endproc

Procedure LCBalUpdt_DeleteProc
_currObj = _Screen.ActiveForm
lcSqlStr = "Update Export_LC_Mast set LC_BalAmt = LC_BalAmt + " + Alltrim(Str(main_vw.fcnet_amt,11,2)) +;
	" Where LC_No = '" + Alltrim(main_vw.lc_no) + "'"

nretval = _currObj.sqlconobj.dataconn([EXE],company.dbname,lcSqlStr,"","_currObj.nHandle",_currObj.DataSessionId,.T.)
If nretval < 0
	nretval = _currObj.sqlconobj.sqlconnclose("_currObj.nHandle")
	Select(lOldArea)
	Return .F.
Endif
Return .T.
Endproc

Procedure RemoveLCProc
_currObj = _Screen.ActiveForm
If !Empty(_currObj.oLCNo)
	lcSqlStr = "Update Export_LC_Mast set LC_BalAmt = LC_BalAmt + " + Alltrim(Str(_currObj.oPrevInvAmt,11,2)) +;
		" Where LC_No = '" + Alltrim(_currObj.oLCNo) + "'"

	nretval = _currObj.sqlconobj.dataconn([EXE],company.dbname,lcSqlStr,"","_currObj.nHandle",_currObj.DataSessionId,.T.)
	If nretval < 0
		nretval = _currObj.sqlconobj.sqlconnclose("_currObj.nHandle")
		Select(lOldArea)
		Return .F.
	Endif

	If Type("main_vw.FrmDocCr") <> 'U'
		Replace main_vw.frmdoccr With '' In main_vw
	Else
		If Type("lmc_vw.FrmDocCr") <> 'U'
			Replace lmc_vw.frmdoccr With '' In lmc_vw
		Endif
	Endif

	If Type("main_vw.DocCrNo") <> 'U'
		Replace main_vw.doccrno With '' In main_vw
	Else
		If Type("lmc_vw.DocCrNo") <> 'U'
			Replace lmc_vw.doccrno With '' In lmc_vw
		Endif
	Endif

	If Type("main_vw.PerInv_No") <> 'U'
		Replace main_vw.perinv_no With '' In main_vw
	Else
		If Type("lmc_vw.PerInv_No") <> 'U'
			Replace lmc_vw.perinv_no With '' In lmc_vw
		Endif
	Endif

	If Type("main_vw.PerInv_Dt") <> 'U'
		Replace main_vw.perinv_dt With {} In main_vw
	Else
		If Type("lmc_vw.PerInv_Dt") <> 'U'
			Replace lmc_vw.perinv_dt With {} In lmc_vw
		Endif
	Endif

	If Type("main_vw.DraftsAt") <> 'U'
		Replace main_vw.draftsat With '' In main_vw
	Else
		If Type("lmc_vw.DraftsAt") <> 'U'
			Replace lmc_vw.draftsat With '' In lmc_vw
		Endif
	Endif

	If Type("main_vw.u_bank") <> 'U'
		Replace main_vw.u_bank With '' In main_vw
	Else
		If Type("lmc_vw.u_bank") <> 'U'
			Replace lmc_vw.u_bank With '' In lmc_vw
		Endif
	Endif

	If Type("main_vw.u_throu1") <> 'U'
		Replace main_vw.u_throu1 With '' In main_vw
	Else
		If Type("lmc_vw.u_throu1") <> 'U'
			Replace lmc_vw.u_throu1 With '' In lmc_vw
		Endif
	Endif

	If Type("main_vw.u_throu2") <> 'U'
		Replace main_vw.u_throu2 With '' In main_vw
	Else
		If Type("lmc_vw.u_throu2") <> 'U'
			Replace lmc_vw.u_throu2 With '' In lmc_vw
		Endif
	Endif

	If Type("main_vw.u_throu3") <> 'U'
		Replace main_vw.u_throu3 With '' In main_vw
	Else
		If Type("lmc_vw.u_throu3") <> 'U'
			Replace lmc_vw.u_throu3 With '' In lmc_vw
		Endif
	Endif

&&added by priyanka_28052013
	If Type("main_vw.u_terms") <> 'U'
		Replace main_vw.u_terms With '' In main_vw
	Else
		If Type("lmc_vw.u_terms") <> 'U'
			Replace lmc_vw.u_terms With '' In lmc_vw
		Endif
	Endif
&&added by priyanka_28052013
Endif
Return .T.
Endproc
* End --> CR_KOEL_0002_Export_Sales_Transaction_&_Export_LC_Master_Cross_Reference


* Changes done as per --> CR_KOEL_0005A_Form_To_Record_Pre_Shipment_Info
* Date : 08/11/2012
* Changes done by EBS Product Team

Procedure ProcDelete
_cursobj = _Screen.ActiveForm
Wait Window + 'Checking existing Pre Shipment Information Details.' Nowait
Select main_vw
Go Top
Sq1= " Delete From PreShipInfo Where Entry_Ty = ?Main_Vw.Entry_Ty and Tran_cd = ?Main_Vw.Tran_Cd " &&and itserial in ("+List_Itsr+")"
nretval = 1
nretval = _cursobj.sqlconobj.dataconn([EXE],company.dbname,Sq1,"","_cursobj.nHandle",_cursobj.DataSessionId)
If nretval<=0
	Wait Clear
	Messagebox("Unable to Delete Pre Shipment Information Details from Database..!!",16,vumess)
	Return .F.
Endif
Wait Clear
Return .T.
Endproc

Procedure ProcSave
_cursobj = _Screen.ActiveForm
Wait Window + 'Updating Pre shipment Info. Details, Please wait' Nowait
If Used('Tbl_PreShipment_Vw')
	_Tally=0
	Select * From Tbl_PreShipment_Vw Into Cursor cursRec
	If _Tally > 0
		Replace All entry_ty With main_vw.entry_ty,;
			tran_cd	With main_vw.tran_cd In Tbl_PreShipment_Vw

		Create Cursor Tbl_SqlString(cmdInsStr Memo,cmdDelStr Memo,cmdFinStr Memo)   && Create temporary cursor for string
		Append Blank In Tbl_SqlString

		Store "" To InsStr,DelStr

		Select item_vw
		Go Top
		Do While !Eof("Item_vw")
			Ds_Itsr = item_vw.itserial

			Replace cmdDelStr With cmdDelStr +;
				" Delete from PreShipInfo where entry_ty ='" + Alltrim(item_vw.entry_ty) +;
				"' and tran_cd =" + Alltrim(Str(item_vw.tran_cd)) + " and itserial ='" + Alltrim(item_vw.itserial) +"'" ;
				IN Tbl_SqlString


			Select Tbl_PreShipment_Vw
			Go Top
			Scan For itserial = Ds_Itsr
				Wait Window + "Updating data for Serial:- "+Transform(Tbl_PreShipment_Vw.Serial) Nowait

				For fieldcount = 1 To Fcount("Tbl_PreShipment_Vw") Step 1
					If Inlist(Field(fieldcount,"Tbl_PreShipment_Vw"),"SRNO","KVA","ENGINECD","CASENO","DIMENSION","LORRYNO","CONTAINNO") ;
							OR ;
							INLIST(Field(fieldcount,"Tbl_PreShipment_Vw"),"CONTAINTYPE","LINESEALNO","EXCSEALNO")
						fxStr = "Replace " + Field(fieldcount,"Tbl_PreShipment_Vw") + " WITH [" + RepQuotes(Evaluate(Field(fieldcount,"Tbl_PreShipment_Vw"))) + ;
							"] IN Tbl_PreShipment_Vw "

						&fxStr
					Endif
				Endfor


				Replace cmdInsStr With cmdInsStr + " Insert into PreShipInfo (entry_ty,tran_cd,itserial,srno,kva,enginecd,caseno,pckqty,nocase,Dimension,grsweight,netweight,lorryno,containno,containtype,linesealno,excsealno,dispadvno,remark) values('" +;
					ALLTRIM(Tbl_PreShipment_Vw.entry_ty) + "',"+;
					ALLTRIM(Str(Tbl_PreShipment_Vw.tran_cd)) + ",'"+;
					ALLTRIM(Tbl_PreShipment_Vw.itserial) + "','"+;
					ALLTRIM(Tbl_PreShipment_Vw.srno) + "','"+;
					ALLTRIM(Tbl_PreShipment_Vw.kva) + "','"+;
					ALLTRIM(Tbl_PreShipment_Vw.enginecd) + "','"+;
					ALLTRIM(Tbl_PreShipment_Vw.caseno) + "',"+;
					ALLTRIM(Str(Tbl_PreShipment_Vw.pckqty,18,3))+","+;
					ALLTRIM(Str(Tbl_PreShipment_Vw.nocase))+",'"+;
					ALLTRIM(Tbl_PreShipment_Vw.Dimension)+"',"+;
					ALLTRIM(Str(Tbl_PreShipment_Vw.grsweight,18,3))+","+;
					ALLTRIM(Str(Tbl_PreShipment_Vw.netweight,18,3))+",'"+;
					ALLTRIM(Tbl_PreShipment_Vw.lorryno) + "','"+;
					ALLTRIM(Tbl_PreShipment_Vw.containno) + "','"+;
					ALLTRIM(Tbl_PreShipment_Vw.containtype) + "','"+;
					ALLTRIM(Tbl_PreShipment_Vw.linesealno) + "','"+;
					ALLTRIM(Tbl_PreShipment_Vw.excsealno) + "','"+;
					ALLTRIM(Tbl_PreShipment_Vw.dispadvno) + "','"+;
					ALLTRIM(Tbl_PreShipment_Vw.remark)+"')";   && Changes done as per --> CR_KOEL_00005A_Form_To_Record_Pre_Shipment_Info_1.2 on Dated 06/03/2013
				In Tbl_SqlString

			Endscan
			Select item_vw
			Skip
		Enddo

		Replace cmdFinStr With cmdDelStr + cmdInsStr In Tbl_SqlString

		nretval = _cursobj.sqlconobj.dataconn([EXE],company.dbname,Tbl_SqlString.cmdFinStr,"","_cursobj.nHandle",_cursobj.DataSessionId,.T.)
		If nretval<=0
			Wait Clear
			Messagebox("Unable to update Pre shipment information details in database..!!",16,vumess)
			Return .F.
		Endif
	Endif
	Use In Tbl_PreShipment_Vw

	If Used("Tbl_SqlString")
		Use In  Tbl_SqlString
	Endif

	If Used("cursRec")
		Use In cursRec
	Endif
	Wait Clear
	Return .T.
Endif
Endproc

Procedure RepQuotes
Parameters sString
sString = Iif(Isnull(sString),"",sString)
Return Alltrim(Strtran(sString,"'","''"))
Endproc
*End --> CR_KOEL_0005A_Form_To_Record_Pre_Shipment_Info
&& changes by EBS team on 07/03/14 for Bug-21466,21467,21468 end

*!*	Return Iif(etsql_con < 1,.F.,.T.)			&& Commented by shrikant S. on 29/09/2014 for Bug-23879

