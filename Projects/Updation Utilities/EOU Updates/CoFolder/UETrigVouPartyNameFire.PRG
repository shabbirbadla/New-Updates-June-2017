_curvouobj=_screen.ActiveForm 
IF(_SCREEN.ActiveForm.PCVTYPE="PO")
mAlias = ALIAS()
		date1= Item_vw.DATE
		SqlConObj=NEWOBJECT('sqlconnudobj',"sqlconnection",xapps)
		nHandle=0
		sq1="SELECT u_dispat,u_refno,u_bank1,u_bank2,u_supref,u_otrefe,u_dest,u_pname,u_padd,u_pono,u_deliv,u_warre,u_indent,u_excomm,u_note,u_terms FROM ac_mast WHERE AC_ID ="+STR(MAIN_VW.AC_ID)
		nRetval = SqlConObj.DataConn([EXE],Company.DbName,sq1,"ITRATEVW","nHandle",_SCREEN.ACTIVEFORM.DATASESSIONID)
		IF nRetval<0
			RETURN .F.
		ENDIF
		SELECT ITRATEVW
		IF _screen.ActiveForm.editmode=.f.
			REPLACE u_disp WITH ITRATEVW.u_dispat IN main_vw
			REPLACE u_ref WITH ITRATEVW.u_refno IN main_vw
			REPLACE u_bank1 WITH ITRATEVW.u_bank1 IN main_vw
			REPLACE u_bank2 WITH ITRATEVW.u_bank2 IN main_vw
			REPLACE u_suppref WITH ITRATEVW.u_supref IN main_vw
			REPLACE u_othref WITH ITRATEVW.u_otrefe IN main_vw
			REPLACE u_dest WITH ITRATEVW.u_dest IN main_vw
			REPLACE u_party WITH ITRATEVW.u_pname IN main_vw
			REPLACE u_partyadd WITH ITRATEVW.u_padd IN main_vw
			REPLACE u_pon WITH ITRATEVW.u_pono IN main_vw
			REPLACE u_delivery WITH ITRATEVW.u_deliv IN main_vw
			REPLACE u_warr WITH ITRATEVW.u_warre IN main_vw
			REPLACE u_indenter WITH ITRATEVW.u_indent IN main_vw
			REPLACE u_comment WITH ITRATEVW.u_excomm IN main_vw
			REPLACE u_note WITH ITRATEVW.u_note IN main_vw
			REPLACE u_terms WITH ITRATEVW.u_terms IN main_vw
		ENDIF
		SELE(mAlias)
ENDIF
 