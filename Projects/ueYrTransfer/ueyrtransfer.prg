Parameters tcCaption,TransferType,pRange

If Vartype(VuMess) <> [C]
	_Screen.Visible = .F.
	Messagebox("Internal Application does not Execute Out-Side ...",16)
	Return .F.
Endif

Public lMsg
lMsg=""
*!*	****Versioning**** Added By Amrendra On 01/06/2011
*!*		LOCAL _VerValidErr,_VerRetVal,_CurrVerVal
*!*		_VerValidErr = ""
*!*		_VerRetVal  = 'NO'
*!*		_CurrVerVal='10.0.0.0' &&[VERSIONNUMBER]
*!*		TRY
*!*			_VerRetVal = AppVerChk('YEAR TRANSFER',_CurrVerVal,JUSTFNAME(SYS(16)))
*!*		CATCH TO _VerValidErr
*!*			_VerRetVal  = 'NO'
*!*		Endtry
*!*		IF TYPE("_VerRetVal")="L"
*!*			cMsgStr="Version Error occured!"
*!*			cMsgStr=cMsgStr+CHR(13)+"Kindly update latest version of "+GlobalObj.getPropertyval("ProductTitle")
*!*			Messagebox(cMsgStr,64,VuMess)
*!*			Return .F.
*!*		ENDIF
*!*		IF _VerRetVal  = 'NO'
*!*			Return .F.
*!*		Endif
*!*	****Versioning****

If Vartype(tcCaption) <> 'C'
	tcCaption = ""
Endif

cDbName = Alltrim(Company.dbName)
dSta_Dt	= Company.Sta_Dt
dEnd_Dt	= Company.End_Dt

*!*	If "vutex" $ vchkprod				&& 02/03/2011
*!*		Messagebox("This option will not work in Trading company.")
*!*		Return
*!*	Endif

If Empty(Company.Enddir) And (Upper(Alltrim(tcCaption))=="UDYOG" Or Upper(Alltrim(tcCaption))=="")
	Messagebox("Create a new year first or Make sure you have exit from software after New Year creation. "+Chr(13)+"Then run this option.",0,VuMess)
	Return
Endif
If !("CLSYRTLIBS" $ Upper(Set("Classlib")))
	Set Classlib To clsyrtlibs.vcx Additive
Endif

oSession  = Createobject("session")
SqlConObj= Newobject("SqlConNudObj","SqlConnection",xapps)
_etDataSessionId=oSession.DataSessionId
*nHandle=0		&&120414


Public UpdtHandle
UpdtHandle=0


lcSqlstr = "Select Top 1 * From Vudyog..Co_mast Where Sta_dt> (Select sta_dt from Vudyog..Co_mast where compid=?Company.CompId) and co_name=?Company.co_name Order by sta_dt "		&& Added by Shrikant S. on 30/12/2014 for bug-24872
*!*	lcSqlstr =	"Select Top 1 * From Vudyog..Co_mast Where DBName='"+Alltrim(Company.Enddir)+"' "
*!*	nRetval=  SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"_newyeardet","nHandle",_etDataSessionId,.T.)		&&21/12/2013
nRetval=  SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"_newyeardet","UpdtHandle",_etDataSessionId)
If nRetval<0
	Return .F.
Endif


If Used('_newyeardet')
	Select _newyeardet
	nSta_Dt	= _newyeardet.Sta_Dt
	nEnd_Dt	= _newyeardet.End_Dt
	nDbName= Alltrim(_newyeardet.dbName)
	lmulti_cur=_newyeardet.mcur_
	nlyn=Allt(Str(Year(_newyeardet.Sta_Dt)-1))+[-]+Allt(Str(Year(_newyeardet.End_Dt)))
Endif

If !Empty(Alltrim(Company.Enddir)) And Upper(TransferType)<>"CAPITAL GOODS ENTRY"
	If Alltrim(nDbName)=Alltrim(cDbName)
		Messagebox('This option will not work since database is same for current year and Next year.',0+64+256,VuMess)
		Return
	Endif
Endif

Do Case
Case Upper(TransferType)="CLOSING STOCK"			&& Transferrring Closing Stock
	lMsg="Stock Transferred successfully..."
	Ans=6
	recfound=.F.

	If (Upper(Alltrim(tcCaption))=="UDYOG" Or Upper(Alltrim(tcCaption))=="")
		lcSqlstr =	"Select Top 1  * From "+Alltrim(nDbName)+"..OsMain Where Party_nm='OPENING STOCK'"
		nRetval=  SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"_osCnt","UpdtHandle",_etDataSessionId,.T.)
		If nRetval<0
			Return .F.
		Endif
	Endif

	If Used('_osCnt') And (Upper(Alltrim(tcCaption))=="UDYOG" Or Upper(Alltrim(tcCaption))=="")
		Select _osCnt
		If Reccount()>0
*Messagebox('Please Delete the Opening Stock Entry in Next Year and Run this Option',0+64+256,VuMess)
			recfound=.T.
			Ans = Messagebox("Your all entries of opening stock may get deleted from new year."+Chr(13)+"Do you want to transfer closing stock ? ",4+32,VuMess)
			If Ans =7
				Return
			Endif
		Endif
		Use In _osCnt
	Endif
*	Ans = Messagebox("Do you want to transfer closing stock ? ",36,VuMess)
*!*		If ! Ans = 6
*!*			Return .F.
*!*		Endif

	If recfound=.T.
		prodType=0
		lcSqlstr =	"Select Entry_ty,Bcode_nm,ext_vou From "+Alltrim(nDbName)+"..Lcode "
		nRetval=  SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"tmpL","UpdtHandle",_etDataSessionId,.T.)
		If nRetval<0
			Return .F.
		Endif
		Select tmpL
		Scan
*!*				lcEntry_tbl=Iif(!Empty(tmpL.Bcode_nm),Alltrim(tmpL.Bcode_nm),Alltrim(tmpL.Entry_ty))	&& 20/01/2015
			lcEntry_tbl=Iif(!Empty(tmpL.Bcode_nm),Alltrim(tmpL.Bcode_nm),Iif(tmpL.ext_vou=.T.,"",Alltrim(tmpL.Entry_ty)))		&& 20/01/2015
			lcSqlstr =	"Delete From "+Alltrim(nDbName)+".."+lcEntry_tbl+"ITREF  Where (Entry_ty+STR(Tran_cd)+ITSERIAL "
			lcSqlstr =lcSqlstr +" in (Select Entry_ty+Convert(Varchar(10),Tran_cd)+ITSERIAL From "+Alltrim(nDbName)+"..OSITREF) and Entry_ty='OS') Or "
			lcSqlstr =lcSqlstr +" (Rentry_ty+STR(itref_tran)+ritserial in  (Select Entry_ty+convert(Varchar(10),Tran_cd)+Itserial From "+Alltrim(nDbName)+"..OSITREF) AND Rentry_ty='OS')"
			nRetval=  SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"","UpdtHandle",_etDataSessionId,.T.)
			If nRetval<0
				Return .F.
			Endif
		Endscan
		If nRetval>0
			lcSqlstr =	"Delete From "+Alltrim(nDbName)+"..OSACDET Where Entry_ty ='OS'"
			nRetval=  SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"","UpdtHandle",_etDataSessionId,.T.)
			If nRetval<0
				Return .F.
			Endif
		Endif
		If nRetval>0
			lcSqlstr =	"Delete From "+Alltrim(nDbName)+"..OSITEM Where Entry_ty ='OS'"
			nRetval=  SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"","UpdtHandle",_etDataSessionId,.T.)
			If nRetval<0
				Return .F.
			Endif
		Endif

		If nRetval>0
			lcSqlstr =	"Delete From "+Alltrim(nDbName)+"..OSMAIN Where Entry_ty ='OS'"
			nRetval=  SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"","UpdtHandle",_etDataSessionId,.T.)
			If nRetval<0
				Return .F.
			Endif
		Endif
		If nRetval>0
			lcSqlstr =	"Delete From "+Alltrim(nDbName)+"..GEN_INV Where Entry_ty ='OS'"
			nRetval=  SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"","UpdtHandle",_etDataSessionId,.T.)
			If nRetval<0
				Return .F.
			Endif
		Endif
		If nRetval>0
			lcSqlstr =	"Delete From "+Alltrim(nDbName)+"..GEN_MISS Where Entry_ty ='OS'"
			nRetval=  SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"","UpdtHandle",_etDataSessionId,.T.)
			If nRetval<0
				Return .F.
			Endif
		Endif
		If nRetval>0
			lcSqlstr =	"Delete From "+Alltrim(nDbName)+"..GEN_Doc Where Entry_ty ='OS'"
			nRetval=  SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"","UpdtHandle",_etDataSessionId,.T.)
			If nRetval<0
				Return .F.
			Endif
		Endif
	Endif

	If "vutex" $ vchkprod

		oPBar=Createobject("frmProgress")
		oPBar.Show()
		oPBar.ProgStatus("Processing Stock Valuation: "+ Alltrim(Transform(5))+ "%"  ,5)


		If Company.uom_no >1
			lcSqlstr =	"Execute Usp_Ent_Gen_MultiUnit_Stock '"+Dtos(dEnd_Dt)+"','"+"'"
			noofunit=Company.uom_no
		Else
			lcSqlstr =	"Execute Usp_Ent_GenStock '"+Dtos(dEnd_Dt)+"','"+"'"
			noofunit=1
		Endif

		oPBar.ProgStatus("Processing Stock Entries: "+ Alltrim(Transform(10))+ "%"  ,10)
		nRet=.T.
		nRetval=  SqlConObj.DataConn([EXE],cDbName,lcSqlstr,"_actstk","UpdtHandle",_etDataSessionId)
		If nRetval<0
			Return .F.
		Endif

		If Reccount('_actstk')>0 And (Upper(Alltrim(tcCaption))=="UDYOG" Or Upper(Alltrim(tcCaption))=="")
			valueType=""

			Ans = Messagebox("Want to Transfer Closing Stock Value of Non-Excise Stock?",36,VuMess)
			If Ans = 6

				Do Form uefrm_stkval.scx With _etDataSessionId To valueType

				If Empty(valueType)
					Return
				Endif
				If !Empty(valueType)
					lcSqlstr =	"Select Top 1 It_Name From It_mast Order by It_Name Desc"
					nRetval=SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"_itname","UpdtHandle",_etDataSessionId)
					If nRetval<0
						Return .F.
					Endif
					toItem=""
					If Used('_itname')
						Select _itname
						toItem=_itname.it_Name

						Use In _itname
					Endif
					If nRetval>0
						Wait Window "Generating valuation..." Nowait
						lcSqlstr =	"EXECUTE USP_REP_STKVAL'','','M.[RULE]<>`ANNEXURE V`','"+Dtos(dSta_Dt)+"','"+Dtos(dEnd_Dt)+"','','','','"+Alltrim(toItem)+"',0,0,'','','','','','','','','','"+Alltrim(valueType)+"'"
						nRetval=  SqlConObj.DataConn([EXE],cDbName,lcSqlstr,"_stkval","UpdtHandle",_etDataSessionId)
						If nRetval<0
							Return .F.
						Endif
						If Used('_stkval')
							Select _stkval

							Update _actstk Set rate=Iif(a.rQty>0,a.Ramt/a.rQty,0) From _stkval a inner Join _actstk On(a.it_code=_actstk.it_code)
							Use In _stkval
						Endif
					Endif
				Endif
			Endif

			Wait Window "Processing Non-Excise stock..." Nowait
			nRet=gentrdstock(SqlConObj,_etDataSessionId,"UpdtHandle",cDbName,nDbName,nSta_Dt,nEnd_Dt,noofunit)

		Endif

		If !Empty(tcCaption)		&& 090414
			If Upper(Alltrim(tcCaption))=="UDYOG"
				nRet=gentrdstock2(SqlConObj,_etDataSessionId,"UpdtHandle",cDbName,nDbName,nSta_Dt,nEnd_Dt,noofunit,1)
			Else
				nRet=gentrdstock2(SqlConObj,_etDataSessionId,"UpdtHandle",cDbName,tcCaption,nSta_Dt,nEnd_Dt,noofunit,2)
			Endif
		Else
			If nRet =.T.
				lcSqlstr =	"Select Entries=isnull(substring((select ', '+rtrim(b.code_nm) from "+Alltrim(nDbName)+"..TradeMain a Inner join "+Alltrim(nDbName)+"..Lcode b on (a.Entry_ty=b.Entry_ty) where date >'"+Dtos(dEnd_Dt)+"' group by a.Entry_ty,b.code_nm  For XML Path('')),2,1000),'')"
				lcSqlstr =lcSqlstr+" ,Entry_ty=isnull(substring((select ', '''+rtrim(a.Entry_ty)+'''' from "+Alltrim(nDbName)+"..TradeMain a Inner join "+Alltrim(nDbName)+"..Lcode b on (a.Entry_ty=b.Entry_ty) where date >'"+Dtos(dEnd_Dt)+"' group by a.Entry_ty,b.code_nm  For XML Path('')),2,100),'')"

				nRetval=  SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"tmpcur","UpdtHandle",_etDataSessionId,.T.)
				If nRetval<0
					nRet =.F.
				Endif
				If !Empty(tmpcur.Entries)
					Messagebox(Allt(tmpcur.Entries)+" entries of Excise has been made in new year."+Chr(13)+"Excise stock cann't be transfer again. ",0+16,VuMess)
					nRet =.F.
					nRetval=0
				Endif
				If nRet =.T.
					Wait Window "Processing Excise/Non-Excise Bills..." Nowait		&& Updating item & main
					lcSqlstr =	"Execute Usp_Closing_Stock '"+Alltrim(nDbName)+"','"+Dtos(dSta_Dt)+"','"+Dtos(dEnd_Dt)+"','"+Alltr(Str(Year(dSta_Dt)))+"-"+Alltr(Str(Year(dEnd_Dt)))+"','"+Alltrim(muserName)+"'"

					nRetval=  SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"tmpcur","UpdtHandle",_etDataSessionId,.T.)
					If nRetval<0
						nRet =.F.
					Endif
					If Used('tmpcur')
						If Reccount('tmpcur')>0
							If Allt(tmpcur.Ans)<>""
								nRet =.F.
								nRetval=0
								lMsg="Error occured while updating stock details"+Chr(13)+Alltrim(tmpcur.Ans)
							Endif
						Endif
					Endif
				Endif
			Else
				lMsg="Error occured while transferring non-excise stock"
				nRetval =0
			Endif

			If nRetval >0 And nRet =.T.
				Wait Window "Updating RG page numbers..." Nowait	&& Updating RG pages
				lcSqlstr =	"Execute "+Alltrim(nDbName)+"..Usp_Ent_Gen_RGPAGE '"+Dtos(dSta_Dt)+"','"+Dtos(dEnd_Dt)+"','"+Dtos(nSta_Dt)+"','"+Dtos(nEnd_Dt)+"','"+Alltr(Str(Year(dSta_Dt)))+"-"+Alltr(Str(Year(dEnd_Dt)))+"'"
				nRetval = SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"tmpcur","UpdtHandle",_etDataSessionId,.T.)
				If nRetval<0
					nRet =.F.
				Endif
				If Used('tmpcur')
					If Reccount('tmpcur')>0
						If Allt(tmpcur.Ans)<>""
							nRet =.F.
							nRetval=0
							lMsg="Error occured while updating RG page details"+Chr(13)+Alltrim(tmpcur.Ans)
						Endif
					Endif
				Endif
			Endif

			If nRetval>0 And nRet =.T.
				Wait Window "Updating stock tables..." Nowait
				lcSqlstr =	" Execute "+Alltrim(nDbName)+"..USP_ENT_UPDATE_ITBALW "			&& Updating It_BalW Table
				nRetval = SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"","UpdtHandle",_etDataSessionId,.T.)
				If nRetval <0
					nRet =.F.
				Endif
			Endif
			If nRetval>0 And nRet =.T.
				Wait Window "Updating stock tables..." Nowait
				lcSqlstr =	" Execute "+Alltrim(nDbName)+"..USP_ENT_UPDATE_ITBAL "			&& Updating It_Bal Table
				nRetval = SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"","UpdtHandle",_etDataSessionId,.T.)
				If nRetval <0
					nRet =.F.
				Endif
			Endif
		Endif
		Clear
	Else
		Ans = Messagebox("Want to Transfer Closing Stock Value ? ",36,VuMess)
		oPBar=Createobject("frmProgress")
		valueType=""
		If Ans = 6

			Do Form uefrm_stkval.scx With _etDataSessionId To valueType
			If Empty(valueType)
				Return
			Endif
			If !Empty(valueType)
*!*					oPBar=Createobject("frmProgress")
*!*					oPBar.Show()
*!*					oPBar.ProgStatus("Processing Stock Valuation: "+ Alltrim(Transform(5))+ "%"  ,5)
				lcSqlstr =	"Select Top 1 It_Name From It_mast Order by It_Name Desc"
				nRetval=SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"_itname","UpdtHandle",_etDataSessionId,.T.)
				If nRetval<0
					Return .F.
				Endif
				toItem=""
				If Used('_itname')
					Select _itname
					toItem=_itname.it_Name

					Use In _itname
				Endif
				If nRetval>0
					lcSqlstr =	"EXECUTE USP_REP_STKVAL'','','M.[RULE]<>`ANNEXURE V`','"+Dtos(dSta_Dt)+"','"+Dtos(dEnd_Dt)+"','','','','"+Alltrim(toItem)+"',0,0,'','','','','','','','','','"+Alltrim(valueType)+"'"
					nRetval=  SqlConObj.DataConn([EXE],cDbName,lcSqlstr,"_stkval","UpdtHandle",_etDataSessionId,.T.)
					If nRetval<0
						Return .F.
					Endif
				Endif
			Endif
		Endif
		oPBar.Show()
		oPBar.ProgStatus("Processing Stock Valuation: "+ Alltrim(Transform(5))+ "%"  ,5)

		If Company.uom_no >1
			lcSqlstr =	"Execute Usp_Ent_Gen_MultiUnit_Stock '"+Dtos(dEnd_Dt)+"','"+"'"
			noofunit=Company.uom_no
		Else
			lcSqlstr =	"Execute Usp_Ent_GenStock '"+Dtos(dEnd_Dt)+"','"+"'"
			noofunit=1
		Endif
		oPBar.ProgStatus("Processing Stock Entries: "+ Alltrim(Transform(10))+ "%"  ,10)
		nRetval=  SqlConObj.DataConn([EXE],cDbName,lcSqlstr,"_actstk","UpdtHandle",_etDataSessionId,.T.)
		If nRetval<0
			Return .F.
		Endif
		If Used('_stkval')
			Select _stkval

			Update _actstk Set rate=Iif(a.rQty>0,a.Ramt/a.rQty,0) From _stkval a inner Join _actstk On(a.it_code=_actstk.it_code)
			Use In _stkval
		Endif

		If Reccount('_actstk')=0
			lMsg="No records found to transfer the stock..."
			nRet=.F.
		Else
			Select _actstk
			Locate
			nRet=gen_stockentries(SqlConObj,_etDataSessionId,"UpdtHandle",cDbName,nDbName,nSta_Dt,nEnd_Dt,noofunit)
		Endif

*!*			Else
*!*				Return
*!*		Endif

	Endif


&& Added By Kishor A. for Bug-27923 on 03-05-2016 Start

Case Upper(TransferType)="THIRD PARTY STOCK"  && Transferring Labour Job V Stock

	lMsg="Stock Transferred successfully..."
	Ans=6
	recfound=.F.
	If (Upper(Alltrim(tcCaption))=="UDYOG" Or Upper(Alltrim(tcCaption))=="")
		lcSqlstr =	"Select Top 1  * From "+Alltrim(nDbName)+"..OsMain Where Party_nm='OPENING STOCK' AND [RULE]='ANNEXURE V'"
		nRetval=  SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"_osCnt","UpdtHandle",_etDataSessionId,.T.)
		If nRetval<0
			Return .F.
		Endif
	ENDIF

	If Used('_osCnt') And (Upper(Alltrim(tcCaption))=="UDYOG" Or Upper(Alltrim(tcCaption))=="")
		Select _osCnt
		If Reccount()>0
			recfound=.T.
			Ans = Messagebox("Your all entries of opening stock may get deleted from new year."+Chr(13)+"Do you want to transfer closing stock ? ",4+32,VuMess)
			If Ans =7
				Return
			Endif
		Endif
		Use In _osCnt
	ENDIF
	

		If nRetval>0
			lcSqlstr =	"Delete From "+Alltrim(nDbName)+"..OSITEM Where Entry_ty ='OS' AND Tran_cd IN (SELECT TRAN_CD FROM OSMAIN WHERE Entry_ty ='OS' AND [RULE]='ANNEXURE V')"
			nRetval=  SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"","UpdtHandle",_etDataSessionId,.T.)
			If nRetval<0
				Return .F.
			Endif
		Endif
	
		If nRetval>0
			lcSqlstr =	"Delete From "+Alltrim(nDbName)+"..OSMAIN Where Entry_ty ='OS' AND [RULE]='ANNEXURE V'"
			nRetval=  SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"","UpdtHandle",_etDataSessionId,.T.)
			If nRetval<0
				Return .F.
			Endif
		Endif
		
		Ans = Messagebox("Want to Transfer Third Party Closing Stock? ",36,VuMess)
		oPBar=Createobject("frmProgress")
		If Ans = 6
			Do Form uefrm_stkval.scx With _etDataSessionId To valueType
			If Empty(valueType)
				Return
			Endif
			If !Empty(valueType)
				lcSqlstr =	"Select Top 1 It_Name From It_mast Order by It_Name Desc"
				nRetval=SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"_itname","UpdtHandle",_etDataSessionId,.T.)
				If nRetval<0
					Return .F.
				Endif
				toItem=""
				If Used('_itname')
					Select _itname
					toItem=_itname.it_Name

					Use In _itname
				Endif
				If nRetval>0
						lcSqlstr =	"EXECUTE USP_REP_STKVAL'','','M.[RULE]=`ANNEXURE V`','"+Dtos(dSta_Dt)+"','"+Dtos(dEnd_Dt)+"','','','','"+Alltrim(toItem)+"',0,0,'','','','','','','','','','"+Alltrim(valueType)+"'"
					nRetval=  SqlConObj.DataConn([EXE],cDbName,lcSqlstr,"_stkval","UpdtHandle",_etDataSessionId,.T.)
					If nRetval<0
						Return .F.
					Endif
				Endif
			Endif
		Endif
		oPBar.Show()
		oPBar.ProgStatus("Processing Stock Valuation: "+ Alltrim(Transform(5))+ "%"  ,5)

		If Company.uom_no >0
			lcSqlstr =	"Execute Usp_Ent_Gen_MultiUnit_Stock '"+Dtos(dEnd_Dt)+"','"+"'"
			noofunit=Company.uom_no
		Else
			lcSqlstr =	"Execute Usp_Ent_Annexure5_GenStock '"+Dtos(dEnd_Dt)+"','"+"'"
			noofunit=1
		Endif
		oPBar.ProgStatus("Processing Stock Entries: "+ Alltrim(Transform(10))+ "%"  ,10)
		nRetval=  SqlConObj.DataConn([EXE],cDbName,lcSqlstr,"_actstk","UpdtHandle",_etDataSessionId,.T.)
		If nRetval<0
			Return .F.
		Endif
		If Used('_stkval')
			Select _stkval

			Update _actstk Set rate=Iif(a.rQty>0,a.Ramt/a.rQty,0) From _stkval a inner Join _actstk On(a.it_code=_actstk.it_code)
			Use In _stkval
		Endif

		If Reccount('_actstk')=0
			lMsg="No records found to transfer the stock..."
			nRet=.F.
		Else
			Select _actstk
			Locate
			nRet=gen_stockentries(SqlConObj,_etDataSessionId,"UpdtHandle",cDbName,nDbName,nSta_Dt,nEnd_Dt,noofunit)
		Endif

&& Added By Kishor A. for Bug-27923 on 03-05-2016 End

&& Added By Kishor A. for Bug-27923 on 22-03-2016 Start
Case Upper(TransferType)="LABOUR JOB IV"  && Transferring Labour Job IV
	lMsg="Labour Job IV entries transferred successfully..."
	Ans = Messagebox("Do you want to Transfer Pending LABOUR JOB IV Entries? ",4+32,VuMess)
	nRet =.T.
	If Ans = 6
		Wait Window "Generating Pending LABOUR JOB IV Entries..., Please wait... " Timeout 1
		lcSqlstr =	"Execute USP_ENT_LabourJob_IV_Pending'"+Alltrim(nDbName)+"','"+Dtos(dEnd_Dt)+"'"
		nRetval=  SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"_LJobIV","UpdtHandle",_etDataSessionId,.T.)
		If nRetval<0
			Return .F.
		Endif
	Else
		Return .F.
	Endif

Case Upper(TransferType)="LABOUR JOB V"  && Transferring Labour Job V
	lMsg="Labour Job V entries transferred successfully..."
	Ans = Messagebox("Do you want to Transfer Pending LABOUR JOB V Entries? ",4+32,VuMess)
	nRet =.T.
	If Ans = 6
		Wait Window "Generating Pending LABOUR JOB V Entries..., Please wait... " Timeout 1
		lcSqlstr =	"Execute USP_ENT_LabourJob_V_Pending'"+Alltrim(nDbName)+"','"+Dtos(dEnd_Dt)+"'"
		nRetval=  SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"_LJobV","UpdtHandle",_etDataSessionId,.T.)
		If nRetval<0
			Return .F.
		Endif
	Else
		Return .F.
	Endif
&& Added By Kishor A. for Bug-27923 on 22-03-2016 End

&& Commented By Kishor A. for Bug-27923 on 11-04-2016 Start
*!*	*!*	Case Upper(TransferType)="LABOUR JOB IV"			&& Transferring Labour Job IV
*!*	*!*		lMsg="Labour Job IV entries transferred successfully..."

*!*	*!*		lcSqlstr =	"Select Top 1  * From "+Alltrim(Company.Enddir)+"..IIMAIN Where Entry_ty ='LI'"
*!*	*!*		nRetval=  SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"_osCnt","UpdtHandle",_etDataSessionId,.T.)
*!*	*!*		If nRetval<0
*!*	*!*			Return .F.
*!*	*!*		Endif
*!*	*!*		If Used('_osCnt')
*!*	*!*			Select _osCnt
*!*	*!*			If Reccount()>0
*!*	*!*				Messagebox('Please Delete the LABOUR JOB IV Entries in Next Year and Run this Option',0+64+256,VuMess)
*!*	*!*				Return
*!*	*!*			Endif
*!*	*!*			Use In _osCnt
*!*	*!*		Endif

*!*	*!*		Ans = Messagebox("Want to Transfer LABOUR JOB IV Entries ? ",36,VuMess)
*!*	*!*		If Ans = 6
*!*	*!*			oPBar=Createobject("frmProgress")
*!*	*!*			oPBar.Show()
*!*	*!*			oPBar.ProgStatus("Processing Labour job IV Entries: "+ Alltrim(Transform(5))+ "%"  ,5)

*!*	*!*	*!*			Do Form uefrm_stkval.scx With _etDataSessionId To valueType
*!*	*!*	*!*			If Empty(valueType)
*!*	*!*	*!*				Return
*!*	*!*	*!*			Endif

*!*	*!*	*!*			If !Empty(valueType)
*!*	*!*	*!*				oPBar=Createobject("frmProgress")
*!*	*!*	*!*				oPBar.Show()
*!*	*!*	*!*				oPBar.ProgStatus("Processing Stock Valuation: "+ Alltrim(Transform(5))+ "%"  ,5)

*!*	*!*	*!*				lcSqlstr =	"Select Top 1 It_Name From It_mast Order by It_Name Desc"
*!*	*!*	*!*				nRetval=SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"_itname","nHandle",_etDataSessionId,.T.)
*!*	*!*	*!*				If nRetval<0
*!*	*!*	*!*					Return .F.
*!*	*!*	*!*				Endif
*!*	*!*	*!*				toItem=""
*!*	*!*	*!*				If Used('_itname')
*!*	*!*	*!*					Select _itname
*!*	*!*	*!*					toItem=_itname.it_Name

*!*	*!*	*!*					Use In _itname
*!*	*!*	*!*				Endif
*!*	*!*	*!*				If nRetval>0
*!*	*!*	*!*					lcSqlstr =	"EXECUTE USP_REP_STKVAL'','','M.[RULE]<>`ANNEXURE V`','"+Dtos(dSta_Dt)+"','"+Dtos(dEnd_Dt)+"','','','','"+Alltrim(toItem)+"',0,0,'','','','','','','','','','"+Alltrim(valueType)+"'"
*!*	*!*	*!*					nRetval=  SqlConObj.DataConn([EXE],cDbName,lcSqlstr,"_stkval","nHandle",_etDataSessionId,.T.)
*!*	*!*	*!*					If nRetval<0
*!*	*!*	*!*						Return .F.
*!*	*!*	*!*					Endif
*!*	*!*	*!*				Endif
*!*	*!*	*!*			Endif
*!*	*!*			lcSqlstr =	"Execute USP_Ent_Labour_Job_IV_Pending '"+Dtos(dEnd_Dt)+"'"
*!*	*!*			nRetval=  SqlConObj.DataConn([EXE],cDbName,lcSqlstr,"_actstk","UpdtHandle",_etDataSessionId,.T.)
*!*	*!*			If nRetval<0
*!*	*!*				Return .F.
*!*	*!*			Endif


*!*	*!*	*!*			Select _actstk
*!*	*!*	*!*			If Used('_stkval')
*!*	*!*	*!*				Select _stkval

*!*	*!*	*!*				Update _actstk Set rate=Iif(a.rQty>0,a.Ramt/a.rQty,0) From _stkval a inner Join _actstk On(a.it_code=_actstk.it_code)
*!*	*!*	*!*				Use In _stkval
*!*	*!*	*!*			Endif
*!*	*!*			If Reccount('_actstk')=0
*!*	*!*				lMsg="No records found to transfer LABOUR JOB IV entries..."
*!*	*!*				nRet=.F.
*!*	*!*			Else
*!*	*!*				Select _actstk
*!*	*!*				Locate
*!*	*!*				oPBar.ProgStatus("Processing Labour job IV Entries: "+ Alltrim(Transform(10))+ "%"  ,10)
*!*	*!*				nRet=generate_labourjob("LI",SqlConObj,_etDataSessionId,"UpdtHandle",cDbName,nDbName,nSta_Dt,nEnd_Dt)
*!*	*!*			Endif
*!*	*!*		Else
*!*	*!*			Return
*!*	*!*		Endif


*!*	*!*	Case Upper(TransferType)="LABOUR JOB V"						&& Transferring Labour Job V
*!*	*!*		lMsg="Labour Job V entries transferred successfully..."
*!*	*!*		lcSqlstr =	"Select Top 1  * From "+Alltrim(Company.Enddir)+"..IRMAIN Where Entry_ty ='RL'"
*!*	*!*		nRetval=  SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"_osCnt","UpdtHandle",_etDataSessionId,.T.)
*!*	*!*		If nRetval<0
*!*	*!*			Return .F.
*!*	*!*		Endif
*!*	*!*		If Used('_osCnt')
*!*	*!*			Select _osCnt
*!*	*!*			If Reccount()>0
*!*	*!*				Messagebox('Please Delete the LABOUR JOB V Entries in Next Year and Run this Option',0+64+256,VuMess)
*!*	*!*				Return
*!*	*!*			Endif
*!*	*!*			Use In _osCnt
*!*	*!*		Endif

*!*	*!*		Ans = Messagebox("Want to Transfer LABOUR JOB V Entries ? ",36,VuMess)
*!*	*!*		If Ans = 6
*!*	*!*			oPBar=Createobject("frmProgress")
*!*	*!*			oPBar.Show()
*!*	*!*			oPBar.ProgStatus("Processing Labour job IV Entries: "+ Alltrim(Transform(5))+ "%"  ,5)

*!*	*!*	*!*			Do Form uefrm_stkval.scx With _etDataSessionId To valueType
*!*	*!*	*!*			If Empty(valueType)
*!*	*!*	*!*				Return
*!*	*!*	*!*			Endif

*!*	*!*	*!*			If !Empty(valueType)
*!*	*!*	*!*				lcSqlstr =	"Select Top 1 It_Name From It_mast Order by It_Name Desc"
*!*	*!*	*!*				nRetval=SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"_itname","nHandle",_etDataSessionId,.T.)
*!*	*!*	*!*				If nRetval<0
*!*	*!*	*!*					Return .F.
*!*	*!*	*!*				Endif
*!*	*!*	*!*				toItem=""
*!*	*!*	*!*				If Used('_itname')
*!*	*!*	*!*					Select _itname
*!*	*!*	*!*					toItem=_itname.it_Name

*!*	*!*	*!*					Use In _itname
*!*	*!*	*!*				Endif
*!*	*!*	*!*				If nRetval>0
*!*	*!*	*!*					lcSqlstr =	"EXECUTE USP_REP_STKVAL'','','M.[RULE]=`ANNEXURE V`','"+Dtos(dSta_Dt)+"','"+Dtos(dEnd_Dt)+"','','','','"+Alltrim(toItem)+"',0,0,'','','','','','','','','','"+Alltrim(valueType)+"'"
*!*	*!*	*!*					nRetval=  SqlConObj.DataConn([EXE],cDbName,lcSqlstr,"_stkval","nHandle",_etDataSessionId,.T.)
*!*	*!*	*!*					If nRetval<0
*!*	*!*	*!*						Return .F.
*!*	*!*	*!*					Endif
*!*	*!*	*!*				Endif
*!*	*!*	*!*			Endif
*!*	*!*			lcSqlstr =	"Execute USP_Ent_Labour_Job_V_Pending '"+Dtos(dEnd_Dt)+"'"
*!*	*!*			nRetval=  SqlConObj.DataConn([EXE],cDbName,lcSqlstr,"_actstk","UpdtHandle",_etDataSessionId,.T.)
*!*	*!*			If nRetval<0
*!*	*!*				Return .F.
*!*	*!*			Endif

*!*	*!*	*!*			Select _actstk
*!*	*!*	*!*			If Used('_stkval')
*!*	*!*	*!*				Select _stkval

*!*	*!*	*!*				Update _actstk Set rate=Iif(a.rQty>0,a.Ramt/a.rQty,0) From _stkval a inner Join _actstk On(a.it_code=_actstk.it_code)
*!*	*!*	*!*				Use In _stkval
*!*	*!*	*!*			Endif

*!*	*!*			If Reccount('_actstk')=0
*!*	*!*				lMsg="No records found to transfer LABOUR JOB V entries..."
*!*	*!*				nRet=.F.
*!*	*!*			Else
*!*	*!*				Select _actstk
*!*	*!*				Locate
*!*	*!*				oPBar.ProgStatus("Processing Labour job IV Entries: "+ Alltrim(Transform(10))+ "%"  ,10)
*!*	*!*				nRet=generate_labourjob("RL",SqlConObj,_etDataSessionId,"UpdtHandle",cDbName,nDbName,nSta_Dt,nEnd_Dt)
*!*	*!*			Endif
*!*	*!*		Else
*!*	*!*			Return
*!*	*!*		Endif
*!*&& Commented By Kishor A. for Bug-27923 on 11-04-2016 End

Case Upper(TransferType)="CAPITAL GOODS ENTRY"				&& Transferring Capital Goods Duty Credit Entry
	lMsg="Capital Goods Duty Credit entries transferred successfully..."
*!*		If Empty(Company.Enddir)
*!*			Messagebox("Create a new year first then run this option",0,VuMess)
*!*			Return
*!*		Endif
	lcSqlstr =	"Select Top 1  * From "+Alltrim(Company.Enddir)+"..IRMAIN Where Entry_ty IN ('HR','GR') AND L_YN ='"+Alltr(Str(Year(nSta_Dt)))+"-"+Alltr(Str(Year(nEnd_Dt)))+"' "
	nRetval=  SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"_osCnt","UpdtHandle",_etDataSessionId,.T.)
	If nRetval<0
		Return .F.
	Endif
	If Used('_osCnt')
		Select _osCnt
		If Reccount()>0
			Messagebox('Please Delete the RG PART-II Receipt Entries in Next Year and Run this Option',0+64+256,VuMess)
			Return
		Endif
		Use In _osCnt
	Endif
	oPBar=Createobject("frmProgress")
	oPBar.Show()
	oPBar.ProgStatus("Processing Capital goods Entries: "+ Alltrim(Transform(5))+ "%"  ,5)
	lcSqlstr =	" SELECT DISTINCT ENTRY_TY,TRAN_CD FROM PTACDET WHERE AC_NAME like "+;
		" '%CAPITAL GOODS PAYABLE A/C%' AND ENTRY_TY IN ('PT','P1') "+;
		" AND L_YN <> '"+Alltr(Str(Year(nSta_Dt)))+"-"+Alltr(Str(Year(nEnd_Dt)))+"' "+;
		" AND ENTRY_TY+CAST(TRAN_CD AS VARCHAR(10)) NOT IN (SELECT ENTRY_TY+CAST(TRAN_CD AS VARCHAR(10)) FROM NEWYEARTRAN ) "
*!*			IIF(Alltrim(nDbName)=Alltrim(cDbName),""," AND L_YN <> '"+Alltr(Str(Year(nSta_Dt)))+"-"+Alltr(Str(Year(nEnd_Dt)))+"' ")

	nRetval=  SqlConObj.DataConn([EXE],cDbName,lcSqlstr,"_tmpAc","UpdtHandle",_etDataSessionId,.T.)
	If nRetval<0
		Return .F.
	Endif
	oPBar.ProgStatus("Processing Capital goods Entries: "+ Alltrim(Transform(10))+ "%"  ,10)
	nRet=.T.

	Select _tmpAc
	reccnt=Reccount()
	If reccnt <> 0
		oPBar.Show()
		lnIncVal = 90 / reccnt
		lninc=0
	Endif
	If reccnt=0
		lMsg="No records found to transfer Capital Goods Duty Credit entries..."
		nRet=.F.
	Endif
	Scan
		lninc=lninc+1
		oPBar.ProgStatus("Processing Capital goods Entries: "+ Alltrim(Transform(Round((lninc /reccnt) * 100,0)))+ "%"  ,(lninc * lnIncVal)+10)

		nRet=gen_capitalacc(_tmpAc.Entry_ty,_tmpAc.Tran_Cd,SqlConObj,_etDataSessionId,"UpdtHandle",nDbName,nSta_Dt,nEnd_Dt,cDbName )
		If !nRet
			Exit
		Endif
		Select _tmpAc
	Endscan
Case Upper(TransferType)="CLOSING BALANCE"					&& Transferring Closing Balance
	lMsg="Closing Balance Transferred successfully..."
	Ans=6
	recfound=.F.
	lcSqlstr =	"Select Top 1  * From "+Alltrim(Company.Enddir)+"..OBMAIN Where Entry_ty ='OB'"
	nRetval=  SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"_osCnt","UpdtHandle",_etDataSessionId,.T.)
	If nRetval<0
		Return .F.
	Endif
	If Used('_osCnt')
		Select _osCnt
		If Reccount()>0
*Messagebox('Please Delete the Opening Balance Entries in Next Year and Run this Option',0+64+256,VuMess)
			recfound=.T.
			Ans = Messagebox("Your all opening balances entries may get deleted from new year."+Chr(13)+"Do you want to Transfer closing balance? ",4+32,VuMess)
			If Ans=7
				Return
			Endif

		Endif
		Use In _osCnt
	Endif

	lcSqlstr =	"Select Top 1 Entry_ty from Lcode Where Entry_ty ='OB'"
	nRetval=  SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"_EntryCnt","UpdtHandle",_etDataSessionId,.T.)
	If nRetval<0
		Return .F.
	Endif
	If Reccount('_EntryCnt') <=0
		Messagebox("Balance cannot be transferred since Opening balance transaction not found in Transaction Setting",0,VuMess)
		Return
	Endif

	If Ans = 6
		If recfound=.T.
			prodType=0

			lcSqlstr =	"Select Entry_ty,Bcode_nm,ext_vou From "+Alltrim(nDbName)+"..Lcode "
			nRetval=  SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"tmpL","UpdtHandle",_etDataSessionId,.T.)
			If nRetval<0
				Return .F.
			Endif
			Select tmpL
			Scan
				tblName=Iif(!Empty(tmpL.Bcode_nm),Alltrim(tmpL.Bcode_nm),Iif(tmpL.ext_vou=.T.,"",Alltrim(tmpL.Entry_ty)))		&& 20/01/2015
				lcSqlstr =	"Delete From "+Alltrim(nDbName)+".."+tblName+"MALL Where (Entry_ty+Convert(Varchar(10),Tran_cd)+Acserial in (Select Entry_ty+Convert(Varchar(10),Tran_cd)+Acserial From "+nDbName+"..Obacdet) and Entry_ty='OB') Or  (Entry_all+Convert(Varchar(10),main_tran)+acseri_all in  (Select Entry_ty+Convert(Varchar(10),Tran_cd)+Acserial From "+nDbName+"..Obacdet) AND Entry_all='OB')"
				nRetval=  SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"","UpdtHandle",_etDataSessionId,.T.)
				If nRetval<0
					Return .F.
				Endif
			Endscan
			If nRetval>0
				lcSqlstr =	"Delete From "+Alltrim(nDbName)+"..OBACDET Where Entry_ty ='OB'"
				nRetval=  SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"","UpdtHandle",_etDataSessionId,.T.)
				If nRetval<0
					Return .F.
				Endif
			Endif
			If nRetval>0
				lcSqlstr =	"Delete From "+Alltrim(nDbName)+"..OBMAIN Where Entry_ty ='OB'"
				nRetval=  SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"","UpdtHandle",_etDataSessionId,.T.)
				If nRetval<0
					Return .F.
				Endif
			Endif
			If nRetval>0
				lcSqlstr =	"Delete From "+Alltrim(nDbName)+"..GEN_INV Where Entry_ty ='OB'"
				nRetval=  SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"","UpdtHandle",_etDataSessionId,.T.)
				If nRetval<0
					Return .F.
				Endif
			Endif
			If nRetval>0
				lcSqlstr =	"Delete From "+Alltrim(nDbName)+"..GEN_MISS Where Entry_ty ='OB'"
				nRetval=  SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"","UpdtHandle",_etDataSessionId,.T.)
				If nRetval<0
					Return .F.
				Endif
			Endif

		Endif
*!*				DO FORM uefrm_billwise With _etDataSessionId TO ans
		Do Case
		Case vchkprod==[vuexc]					&& If Product is Only Manufacturing
			prodType=1
			billAns=0
		Otherwise								&& If Product is not Only Manufacturing
			prodType=2
			billAns=1
		Endcase
		Wait Window "Generating Closing Balance" Nowait
*lcSqlstr =	"Execute USP_ENT_GETCLOSING '"+Dtos(dEnd_Dt)+"',' AND AC_MAST.TYP<>''BANK'' ',"+Str(billAns)+","+Str(prodType)
		lcSqlstr =	"Execute USP_ENT_GETCLOSING '"+Dtos(dEnd_Dt)+"',' ',"+Str(billAns)+","+Str(prodType)
		nRetval=SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"_balacdet","UpdtHandle",_etDataSessionId,.T.)
		If nRetval<0
			Return .F.
		Endif

		If Reccount('_balacdet')=0
			lMsg="No records found to transfer Closing Balance..."
			nRet=.F.
		Else
			Select _balacdet
			Locate
			nRet=generate_closingBal("OB",SqlConObj,_etDataSessionId,"UpdtHandle",cDbName,Alltrim(nDbName),nSta_Dt,nEnd_Dt)
		Endif
	Else
		Return
	Endif
Case Upper(TransferType)="RECO STATEMENT"					&& Transferring Reco Statement
	lMsg="Reco Statement Transferred successfully..."
	Ans = Messagebox("Do you want to Transfer Reco Statement? ",4+32,VuMess)
	If Ans = 6
*!*			lcSqlstr =	"Select Top 1  * From "+Alltrim(Company.Enddir)+"..Recostat "
*!*			nRetval=  SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"_osCnt","UpdtHandle",_etDataSessionId,.T.)
*!*			If nRetval<0
*!*				Return .F.
*!*			Endif
*!*			If Used('_osCnt')
*!*				Select _osCnt
*!*				If Reccount()>0
*!*					Messagebox('Please Delete the Entries from Recostat Table in Next Year and Run this Option',0+64+256,VuMess)
*!*					Return
*!*				Endif
*!*				Use In _osCnt
*!*			Endif

		oPBar=Createobject("frmProgress")
		oPBar.Show()
		oPBar.ProgStatus("Processing Reco Statements: "+ Alltrim(Transform(0))+ "%"  ,0)
*!*			lcSqlstr =	"Execute USP_ENT_GETCLOSING '"+Dtos(dEnd_Dt)+"',' AND AC_MAST.TYP=''BANK'' ',"+Str(0)+","+Str(2)
*!*			nRetval=SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"_balacdet","nHandle",_etDataSessionId,.T.)
*!*			If nRetval<0
*!*				Return .F.
*!*			Endif
*!*			nRet=generate_closingBal("OB",SqlConObj,_etDataSessionId,"nHandle1",nDbName,nSta_Dt,nEnd_Dt)
*!*			If nRet
*!*			lcSqlstr =	" Select bankreco.* from bankreco Inner Join Ac_Mast On (Ac_Mast.Ac_Name=bankreco.Ac_Name) "+;
*!*				" Where Ac_Mast.[Group]='CASH & BANK BALANCES' And (BankReco.Cl_Date=0 or BankReco.Cl_Date between '"+Dtos(nSta_Dt)+"' and '"+Dtos(nEnd_Dt)+"' )"

		lcSqlstr ="Execute USP_ENT_Gen_BankReco '"+Alltrim(nDbName)+"','"+Dtos(nSta_Dt)+"','"+Dtos(nEnd_Dt)+"'"
		nRetval=SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"_breco","UpdtHandle",_etDataSessionId,.T.)
		If nRetval<0
			Return .F.
		Endif
		If Reccount('_breco')=0
			lMsg="No records found to transfer Reco Statement..."
			nRet=.F.
		Else
			Select _breco
			Locate
			oPBar.ProgStatus("Processing Reco Statements: "+ Alltrim(Transform(10))+ "%"  ,10)
			nRet=generate_recostatement(SqlConObj,_etDataSessionId,"UpdtHandle",cDbName,nDbName,nSta_Dt,nEnd_Dt)
		Endif
*!*			Endif

	Else
		Return
	Endif
Case Upper(TransferType)="PENDING BILLS TRANSFER"					&& Transferring Pending Bills
	lMsg="Pending Bills Transferred successfully..."
	Ans = Messagebox("Do you want to Transfer Pending Bills? ",4+32,VuMess)
	If Ans = 6
		Wait Window "Please wait..." Timeout 1
		lcSqlstr =	"Select Top 1  * From "+Alltrim(Company.Enddir)+"..OBMAIN Where Entry_ty ='OB'"
		nRetval=  SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"_osCnt","UpdtHandle",_etDataSessionId,.T.)
		If nRetval<0
			Return .F.
		Endif
		If Used('_osCnt')
			Select _osCnt
			If Reccount()<=0
				Messagebox('Please transfer Closing Balance first.',0+64+256,VuMess)
				Return
			Endif
			Use In _osCnt
		Endif
*Do Form uefrm_selectentry With _etDataSessionId To Entrylist

		If lmulti_cur=.T.
			lcSqlstr =	"Execute USP_GEN_MULTICUR_PENDING_BILLS '"+nDbName+"','"+Dtos(dEnd_Dt)+"','"+Alltr(Str(Year(nSta_Dt)))+"-"+Alltr(Str(Year(nEnd_Dt)))+"'"
		Else
			lcSqlstr =	"Execute USP_ENT_TRANSFER_PENDING_BILLS '"+nDbName+"','"+Dtos(dEnd_Dt)+"','"+Alltr(Str(Year(nSta_Dt)))+"-"+Alltr(Str(Year(nEnd_Dt)))+"'"
		Endif

		nRetval=  SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"","UpdtHandle",_etDataSessionId,.T.)
		If nRetval<0
			Return .F.
		Endif
		nRet =.T.
		Wait Clear
	Else
		Return
	Endif
Case Upper(TransferType)="PENDING ORDER TRANSFER"					&& Transferring Pending Orders
	lMsg="Pending Orders Transferred successfully..."
	Ans = Messagebox("Do you want to Transfer Pending Orders? ",4+32,VuMess)
	nRet =.T.
	If Ans = 6
		Wait Window "Please wait..." Timeout 1
		lcSqlstr =	"Execute "+Alltrim(Company.Enddir)+"..USP_ENT_GetOrderEntry "
		If nRetval<0
			Return .F.
		Endif
		If Used('_osCnt')
			lEntry_ty=""
			Select _osCnt
			If Reccount()>0
				Scan
					lEntry_ty=lEntry_ty+" "+_osCnt.Entry_ty
				Endscan
				Ans =Messagebox(Iif(!Empty(lEntry_ty),"("+lEntry_ty+")","")+" order entries has been already made/transfered in new year."+Chr(13)+"Still you want to continue?",4+32,VuMess)
				If Ans<>6
					nRet =.F.
				Endif
			Endif
			Use In _osCnt
		Endif
*Do Form uefrm_selectentry With _etDataSessionId To Entrylist
		If nRet =.T.
			Wait Window "Generating Pending Order entries..., Please wait... " Timeout 1
			lcSqlstr =	"Execute USP_ENT_Get_Order_Records '"+Alltrim(nDbName)+"','"+Dtos(dEnd_Dt)+"'"
			nRetval=  SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"_Order","UpdtHandle",_etDataSessionId,.T.)
			If nRetval<0
				Return .F.
			Endif

			If Used('_Order')
				Select _Order
				If _Order.Ans=0
					=Messagebox(Alltrim(_Order.ErrorMsg),16,VuMess)
					nRet =.F.
				Endif
			Endif
		Endif
		Wait Clear
	Else
		Return
	Endif
&& Added By Kishor A. for Bug-27793 on 22-03-2016 Start
Case Upper(TransferType)="PENDING WORKORDER TRANSFER"  && PENDING WORKORDER TRANSFER
	lMsg="Pending Work Orders Transferred successfully..."
	Ans = Messagebox("Do you want to Transfer Pending Work Orders? ",4+32,VuMess)
	nRet =.T.
	If Ans = 6
		Wait Window "Generating Pending Work Order entries..., Please wait... " Timeout 1
		lcSqlstr =	"Execute USP_ENT_WorkOrder_Pending '"+Alltrim(nDbName)+"','"+Dtos(dEnd_Dt)+"'"
		nRetval=  SqlConObj.DataConn([EXE],cDbName ,lcSqlstr,"_Order","UpdtHandle",_etDataSessionId,.T.)
		If nRetval<0
			Return .F.
		Endif
	Endif
&& Added By Kishor A. for Bug-27793 on 22-03-2016 End

Endcase

If nRet
	sql_con = SqlConObj._SqlCommit("UpdtHandle")
	If sql_con < 0
		sql_con = SqlConObj._SqlRollback("UpdtHandle")
	Endif
	If (!Empty(lMsg))
		=Messagebox(lMsg,0+64,VuMess)
	Endif
Else
	If (!Empty(lMsg))
		=Messagebox(lMsg,16,VuMess)
	Endif
	If UpdtHandle > 0
		sql_con = SqlConObj._SqlRollback("UpdtHandle")
	Endif
Endif
=SqlConObj.sqlconnclose("UpdtHandle")
*=SqlConObj.sqlconnclose("nHandle")

Return




*>>>***Versioning**** Added By Amrendra On 05/07/2011
Function GetFileVersion
Parameters lcTable
_CurrVerVal='10.2.0.0' &&[VERSIONNUMBER]
If !Empty(lcTable)
	Select(lcTable)
	Append Blank
	Replace fVersion With Justfname(Sys(16))+'   '+_CurrVerVal
Endif
Return _CurrVerVal
*<<<***Versioning**** Added By Amrendra On 05/07/2011

