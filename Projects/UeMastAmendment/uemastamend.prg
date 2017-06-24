
#Define UEAMENFMENT.APP_VERSION	'3.1'	&& STORE THE PROGRAME VERSION

#Define UEAMENFMENT_ERROR_01 "Not able to create the cursor "+mCursorNm1+", Can't proceed ...!"
#Define UEAMENFMENT_ERROR_02 "Not able to create the cursor "+mCursorNm2+", Can't proceed ...!"
#Define UEAMENFMENT_ERROR_03 "Not able to create the cursor "+mCursorNm3+", Can't proceed ...!"
#Define UEAMENFMENT_ERROR_04 "Enabled to insert data in Amend_detail table, Can't proceed ...!"

#Define UEAMENFMENT_MESSAGE_01 "Problem in adding fields because its already exist"

#Define UEAMENFMENT_QUESTION_01 "Activate Amendment Option"+Chr(13)+"Click 'Yes' to 'ACTIVATE'"+Chr(13)+"And"+Chr(13)+"Click 'No' to 'DEACTIVATE'"


******************************************
*  PRCODURE SRNO	: 1
*  PRCODURE NAME 	: VouBefEdit()
*  PURPOSE			: This procedure helps to stroes the existing voucher data in cursor and fire Amendment form
*  FIRE      		: After "UeTrigMastBefEdit.PRG"
******************************************
Procedure VouBefEdit()
SET DATASESSION TO _Screen.ActiveForm.DataSessionId
Public New_aNo, New_aDate, New_aReamrk, mOldMain,mOldCurVw,MsgRlt,mNewMain,mNewCurVw,mMapCurVw
*!*	IF 'trnamend' $ vChkprod  && Commented by Suraj K. for Bug-27251 date on 04-12-2015
IF ('trnamend' $ vChkprod ) OR ('HRPayHRPay' $ vChkprod ) && Added by Suraj K. for Bug-27251 date on 04-12-2015

	_curvouobj = _Screen.ActiveForm
	IF TYPE('MastCode_vw.Amendment')='U'
		RETURN
	ENDIF
	If INLIST(Alltrim(MastCode_vw.Amendment),"WARNING","STRICT")
		MainAlias = Alias()
		tmpAliasList=chrtran(ALLTRIM(MastCode_vw.TblAlias),',',CHR(13))
		tmpFldList=chrtran(ALLTRIM(MastCode_vw.fldlst),',',CHR(13))
		tmpMapFldList=chrtran(ALLTRIM(MastCode_vw.MapFldLst),',',CHR(13))
		ALINES(tempArray,tmpAliasList)
		ALINES(tempFldArray,tmpFldList)
		ALINES(tempMapFld,tmpMapFldList)
		PUBLIC ARRAY AmendALias (ALEN(tempArray))
		PUBLIC ARRAY AmendFldLst(ALEN(tempArray),2)
		ACOPY(tempArray,AmendALias)
		i=0
		FOR i=1 TO ALEN(AmendALias)
			AmendFldLst(i,1)='' && Stores Fields List
			AmendFldLst(i,2)='' && Stores Fields Mapping for Amendment Details
&&---> Finding Fields List
			FOR j=1 TO ALEN(tempFldArray)
				IF ALLTRIM(AmendALias(i)) $ tempFldArray(j)
					AmendFldLst(i,1)=AmendFldLst(i,1)+','+JUSTEXT(tempFldArray(j))
				ENDIF
			ENDFOR
			AmendFldLst(i,1)=IIF(LEN(ALLTRIM(AmendFldLst(i,1)))>=2,SUBSTR(AmendFldLst(i,1),2),'')
&&<--- Finding Fields List

&&---> Finding Mapping Fields List
			FOR k=1 TO ALEN(tempMapFld)
				IF ALLTRIM(AmendALias(i)) $ tempMapFld(k)
					AmendFldLst(i,2)=AmendFldLst(i,2)+','+JUSTEXT(tempMapFld(k))
				ENDIF
			ENDFOR
			AmendFldLst(i,2)=IIF(LEN(ALLTRIM(AmendFldLst(i,2)))>=2,SUBSTR(AmendFldLst(i,2),2),'')
&&<--- Finding Mapping Fields List
			mXMain=ALLTRIM(AmendALias(1))
			mOldMain='old'+ALLTRIM(AmendALias(1))
			mOldCurVw='old'+ALLTRIM(AmendALias(i))
			mMapCurVw='map'+ALLTRIM(AmendALias(i))
			sql_str=''
			IF USED(mOldCurVw)
				USE IN &mOldCurVw
			ENDIF
			xsdfcral=ALLTRIM(AmendALias(i))
			IF USED(xsdfcral)
				SELECT &xsdfcral
				=TABLEUPDATE(.T.)
			ENDIF 
&& Mapping Cursor Creation ---->
			IF i=1
				sql_str=''
				IF !EMPTY(AmendFldLst(i,2))
					sql_str=[select ]+STRTRAN(AmendFldLst(i,2),'#',' as ')+[ from ]+AmendALias(i)+[ INTO CURSOR &mMapCurVw readwrite]
					&sql_str
				ENDIF
			ENDIF
&& Mapping Cursor Creation <----


&& Tables Cursor Creation ---->
			IF USED(AmendALias(i))
				sql_str=''
				IF !EMPTY(AmendFldLst(i,1)) AND USED(AmendALias(i))
*				sql_str='select Amend_No,Amend_dt,amend_remark,'+AmendFldLst(i,1)+ ' from '+AmendALias(i)+' into cursor &mOldCurVw readwrite'
					sql_str='select '+AmendFldLst(i,1)+ ' from '+AmendALias(i)+' into cursor &mOldCurVw readwrite'
					&sql_str
				ENDIF
			ENDIF
&& Tables Cursor Creation <----

		ENDFOR
		lcx=AmendALias(1)+'.Amend_no'
*		mOldMain='old'+ALLTRIM(AmendAlias(1))
		New_aNo = Iif(Isnull(&lcx),0,&lcx)


		New_aDate = Datetime()
		New_aReamrk =""
		MsgRlt = 0

		If Alltrim(MastCode_vw.Amendment) = "STRICT"
			MsgRlt = 6
		Else
			MsgRlt = Messagebox(UEAMENFMENT_QUESTION_01,4,vumess)
		Endif

		If MsgRlt = 6
			Do Form frm_amendment
			SELECT &mXMain
			replace Amend_No WITH New_aNo &&IN &mOldMain
			replace Amend_Dt WITH New_aDate &&IN &mOldMain
			replace Amend_Remark WITH New_aReamrk &&IN &mOldMain
		ENDIF

		If !Empty(MainAlias)
			Select &MainAlias
		Endif
	ENDIF
ENDIF
RETURN .T.
Endproc


******************************************
*  PRCODURE SRNO	: 7
*  PRCODURE NAME 	: VouFinalUpdate()
*  PURPOSE			: In this procedure we actually stores the all amended data in database
*  FIRE      		: After "UeTrigMastFinalUpdate.PRG"
******************************************
Procedure VouFinalUpdate
SET DATASESSION TO _Screen.ActiveForm.DataSessionId
*!*	IF 'trnamend' $ vChkprod && Commented By Suraj K. for Bug-27251 Date on 04-12-2015 
IF ('trnamend' $ vChkprod)  OR ('HRPayHRPay' $ vChkprod) && Added by Suraj K. for Bug-27251 date on 04-12-2015
	IF TYPE('msgrlt')='U'
		MsgRlt=6
	ENDIF
	_curvouobj = _Screen.ActiveForm

	IF USED('MastCode_vw') AND MsgRlt=6
		If ((Alltrim(MastCode_vw.Amendment) = "WARNING" And _curvouobj.EditMode = .T.) Or (Alltrim(MastCode_vw.Amendment) = "STRICT" And _curvouobj.EditMode = .T.))
			MsgRlt = 0

			mxsqlx = ' Select entry_ty,inv_no,Date,tran_cd,Amend_No,Amend_Dt,old_user,new_user,tbl_nm,tbl_fld,Old_Value,New_Value,TRAN_NM,Status,FileOrd,amend_remark from Amend_detail where 1 = 2 '
			nretval = _curvouobj.SqlConObj.DataConn("EXE",Company.DbName,mxsqlx,"Amend_det","_curvouobj.nhandle",_curvouobj.DataSessionId)

			If nretval <1
				Messagebox("Amendment Details Table Error",16,vumess)
				Err_Found='YES'
				Return .F.
			Endif

*//\\\\///\\\/////////
			mMapCurVw='map'+ALLTRIM(AmendALias(1))
			xEntryTy=Alltrim(MastCode_vw.Code)
			xTranNm=ALLTRIM(_curvouobj.caption)
			xTranCd=IIF(TYPE(mMapCurVw+'.Tran_cd')<>'U',mMapCurVw+'.Tran_cd','')
			xInvNo=IIF(TYPE(mMapCurVw+'.Inv_no')<>'U', mMapCurVw+'.Inv_no','')
			xDate=''
			xxtd=mMapCurVw+'.Date'
			xDate=IIF(TYPE('&xxtd')<>'U',xxtd,"CTOT('01-01-1900')")


			FOR amend_i=1 TO ALEN(AmendALias)
*				mOldMain='old'+ALLTRIM(AmendALias(1))
*				mNewMain='New'+ALLTRIM(AmendALias(1))
				mOldCurVw='old'+ALLTRIM(AmendALias(amend_i))
				mNewCurVw=ALLTRIM(AmendALias(amend_i))

				tbl_nm=''
				tbl_fld=''
				old_value=''
				New_value=''
				xtran_nm=''
				xoldfld=''
				xnewfld=''

				IF !USED(mOldCurVw)
					LOOP
				ENDIF
				IF USED(mNewCurVw)
						SELECT &mNewCurVw
						TABLEUPDATE(.t.)
					mNewCurVw='New'+ALLTRIM(AmendALias(amend_i))
				ELSE
					loop
				ENDIF

&& Tables Cursor Creation <----
				IF USED(AmendALias(amend_i))
					sql_str=''
					IF !EMPTY(AmendFldLst(amend_i,1))
						sql_str='select '+AmendFldLst(amend_i,1)+ ' from '+AmendALias(amend_i)+' into cursor &mNewCurVw readwrite'
						&sql_str
					ENDIF
&& Tables Cursor Creation <----

					xAmendNo=New_aNo
					xAmendDt=New_aDate
					xfileOrd=amend_i
					SELECT &mNewCurVw
					GO TOP
					SELECT &mOldCurVw
					GO TOP
					DO WHILE !EOF()
						xStatus='Record Altered No : '+ALLTRIM(STR(RECNO()))
						xoUser=''
						xnUser=''
						FOR k=1 TO FCOUNT(mOldCurVw)
							SELECT &mOldCurVw

							xoldfld=mOldCurVw+'.'+FIELD(k)
							xnewfld=mNewCurVw+'.'+FIELD(k)
							xtbl_fld=FIELD(k)

							xOld_value=''
							xNew_value=''

							xOld_value=&xoldfld
							xNew_value=&xnewfld

							IF TYPE('&xoldfld')<>'U'
								DO CASE
								CASE TYPE('&xoldfld')='T'
									xOld_value=Ttoc(&xoldfld)
									xNew_value=Ttoc(&xnewfld)
								CASE TYPE('&xoldfld')='M'
									xOld_value=CHRTRANC(&xoldfld,&xoldfld,&xoldfld)
									xNew_value=CHRTRANC(&xnewfld,&xnewfld,&xnewfld)
								CASE TYPE('&xoldfld')='N'
									xOld_value=Str(&xoldfld,20,2)
									xNew_value=Str(&xnewfld,20,2)
								CASE TYPE('&xoldfld')='L'
									xOld_value=Iif(&xoldfld=.T.,'.T.','.F.')
									xNew_value=Iif(&xnewfld=.T.,'.T.','.F.')
								ENDCASE
							ENDIF
							xtran_nm=ALLTRIM(_curvouobj.caption)
							SELECT amend_det
							APPEND BLANK
							Replace entry_ty With xEntryTy In amend_det
*							MESSAGEBOX(IIF(EMPTY(xInvNo),'',&xInvNo))
							Replace inv_no With IIF(EMPTY(xInvNo),'',&xInvNo) In amend_det
							Replace Date With IIF(EMPTY(xDate),CTOT('01-01-1900'),&xDate) In amend_det
							Replace tran_cd With IIF(EMPTY(xTranCd),'',&xTranCd) In amend_det
							Replace Amend_No With New_aNo In amend_det
							Replace Amend_Dt With New_aDate In amend_det
							replace amend_remark WITH New_aReamrk IN amend_det && Added By Amrendra For Bug-8506 on 28-01-2013
							Replace old_user With xoUser In amend_det
							Replace new_user With musername In amend_det
							Replace tbl_nm With ALLTRIM(AmendALias(amend_i)) In amend_det
							Replace tbl_fld With xtbl_fld In amend_det
							Replace old_value With xOld_value In amend_det
							Replace New_value With xNew_value In amend_det
							Replace TRAN_NM With ALLTRIM(_curvouobj.caption) In amend_det
							Replace Status With xStatus In amend_det
							Replace fileOrd With amend_i In amend_det

						ENDFOR
						SKIP IN &mNewCurVw
						SELECT &mOldCurVw
						SKIP
					ENDDO
					USE IN &mOldCurVw
				ENDIF

			ENDFOR
			USE IN &mMapCurVw
			Err_Found=''
			If Used('Amend_det')
				Select amend_det
				Go Top
				xxa=SET("Exact" )
				SET EXACT ON
				DELETE FOR UPPER(ALLTRIM(old_value))=UPPER(ALLTRIM(New_value))
				SET EXACT &xxa
				GO TOP
				Do While !Eof()
					msqlstr =""
					msqlstr = _curvouobj.SqlConObj.GenInsert(Company.DbName+"..Amend_Detail","","","Amend_det",mvu_backend)
					etsql_con = 0
					etsql_con = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,msqlstr,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
					If etsql_con  < 1
						Messagebox(UEAMENFMENT_ERROR_04,16,vumess)
						Err_Found='YES'
						Return .F.
					Endif
					Select amend_det
					Skip
				ENDDO

				IF Err_Found!='YES'
					nCommit=_curvouobj.SqlConObj._sqlcommit("thisform.nHandle")
					If nCommit<=0
						=Messagebox("Entry not updated. Re-enter the same",64,vumess)
						nRollback=_curvouobj.SqlConObj._sqlrollback("thisform.nHandle")
						If nRollback<=0
							Return .F.
						Endif
						Return .F.
					ENDIF
				ELSE
					nRollback=_curvouobj.SqlConObj._sqlrollback("thisform.nHandle")
					If nRollback<=0
						Return .F.
					ENDIF
				ENDIF
			ENDIF
			USE IN amend_det
*			USE IN &mMapCurVw
			RELEASE AmendALias,AmendFldLst
			RELEASE New_aNo, New_aDate, New_aReamrk, mOldMain,mOldCurVw,MsgRlt,mNewMain,mNewCurVw
		Endif
	ENDIF
ENDIF
RETURN .T.
Endproc


*>>>***Versioning**** Added By Amrendra On 05/07/2011 TKT 8543
FUNCTION GetFileVersion
PARAMETERS lcTable
_CurrVerVal='10.0.0.0' &&[VERSIONNUMBER]
IF !EMPTY(lcTable)
	SELECT(lcTable)
	APPEND BLANK
	replace fVersion WITH JUSTFNAME(SYS(16))+'   '+_CurrVerVal
ENDIF
RETURN _CurrVerVal
*<<<***Versioning**** Added By Amrendra On 05/07/2011  TKT 8543
