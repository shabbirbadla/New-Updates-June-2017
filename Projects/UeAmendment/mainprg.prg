*!***************************************************************************************************************************************************!*
*!*	PROGRAME NAME						: MAINPRG.PRG																								*!*
*!*	DELEVELOPED BY						: RAJESH DHAVALE																							*!*
*!*	PROGRAME PURPOSE					: MAINTAINED FIELD WISE HISTORY	OF VOUCHER																	*!*
*!*-------------------------------------------------------------------------------------------------------------------------------------------------*!*
*!*	VERSION DETAILS																																	*!*
*!*-------------------------------------------------------------------------------------------------------------------------------------------------*!*
*!*	ASSIGNED DATE						: 11/06/2010																								*!*
*!*	VERSION								: 1.0 	w.e.f DATED 23/10/2010																				*!*
*!*	PURPOSE								: Before edit software altered the existing transaction stores 										    	*!*
*!*										: it in different transaction i.e. Amendment History and also auto generate the Amendment Number			*!*
*!*										: (Note: This version is made according to the SRS details 													*!*
*!*																																					*!*
*!*	ASSIGNED DATE						: 01/07/2010																								*!*
*!*	VERSION								: 2.0 	w.e.f DATED 23/10/2010																				*!*
*!*	PURPOSE								: In this new version we have changed to whole concept from trnsaction wise to fields wise  i.e. 			*!*
*!*										: now we only stores the data in one table and only for amended fields and that also as per user required	*!*
*!*										: (Note: This version is made according to the knowledege received from discussion with Mr. Arun Dixit)		*!*
*!*																																			 		*!*
*!*	ASSIGNED DATE						: 18/11/2010																								*!*
*!*	VERSION								: 3.0 	w.e.f DATED 25/12/2010																				*!*
*!*	PURPOSE								: In this new version we have restricted users to provide rights to changes only few of the fields and 		*!*
*!*										: that also we have provided the control to users itself due to avoide mistakes and improve performance		*!*
*!*																																					*!*
*!*	ASSIGNED DATE						: 05/02/2011																								*!*
*!*	VERSION								: 3.1 	w.e.f DATED 24/02/2011																				*!*
*!*	PURPOSE								: Genaret the new version due to uevoucher.app files changes regarding "LMC" table					 		*!*
*!*																																					*!*
*!*	New Plans for next release			: //2010																									*!*
*!*	VERSION								: 3.2 	w.e.f DATED //2010																					*!*
*!*	PURPOSE								: In our next 3.1 release as below:																			*!*
*!*										: 1) Provide individual userswise data amendment option in current version its comman setting for all users	*!*
*!*										: 2) Considre approval system at the time of amendment														*!*
*!*										: 3) Considre multi-tasking at the time of amendment														*!*
*!*	Modification 						: Birendra on 22 Mar 2011
*!*************************************o**************************************************************************************************************!*

#Define UEAMENFMENT.APP_VERSION	'3.1'	&& STORE THE PROGRAME VERSION

#Define UEAMENFMENT_ERROR_01 "Not able to create the cursor "+mCursorNm1+", Can't proceed ...!"
#Define UEAMENFMENT_ERROR_02 "Not able to create the cursor "+mCursorNm2+", Can't proceed ...!"
#Define UEAMENFMENT_ERROR_03 "Not able to create the cursor "+mCursorNm3+", Can't proceed ...!"
#Define UEAMENFMENT_ERROR_04 "Enabled to insert data in Amend_detail table, Can't proceed ...!"

#Define UEAMENFMENT_MESSAGE_01 "Problem in adding fields because its already exist"

#Define UEAMENFMENT_QUESTION_01 "Activate Amendment Option"+Chr(13)+"Click 'Yes' to 'ACTIVATE'"+Chr(13)+"And"+Chr(13)+"Click 'No' to 'DEACTIVATE'"

******************************************
*  PRCODURE SRNO	: 1
*  PRCODURE NAME	: VouInit()
*  PURPOSE			: Runtime adding two extra fields in lother database and display it in screen (voucherwise)
*  FIRE        		: After "UeTrigVouInit.PRG"
******************************************
Procedure VouInit()
_curvouobj = _Screen.ActiveForm
If 'trnamend' $ vChkprod
	If Alltrim(lcode_vw.Amendment) = "STRICT" Or Alltrim(lcode_vw.Amendment) = "WARNING"
		Select lother_vw
		msr = 0
		Calculate Max(serial) To msr
		msr = msr + 1
		Select lother_vw
		Append Blank
*		Replace e_code With Main_vw.entry_ty, ingrid With .T.,head_nm With 'Amendment No',;
fld_nm With 'Amend_no',data_ty With 'N',fld_wid With 4,;
fld_dec With 0,att_file With .T.,whn_con With '',;
val_con With '',val_err With '',defa_val With '',;
filtcond With '',serial With msr,inter_use With .F.,;
bef_aft With .F.,mandatory With .F.,;
User_Name With 'ADMIN',tbl_nm With 'Main_vw'  In lother_vw		 && Changes done for Version 3.1 i.e. change tbl_nm from "main_vw.entry_ty+'MAIN'" to "Main_vw"
*Birendra : Changes Done for Mandatory and inter_use field: 03/12/2012: :Start:
		Replace e_code With Main_vw.entry_ty, ingrid With .T.,head_nm With 'Amendment No',;
			fld_nm With 'Amend_no',data_ty With 'N',fld_wid With 4,;
			fld_dec With 0,att_file With .T.,whn_con With '',;
			val_con With '',val_err With '',defa_val With '',;
			filtcond With '',serial With msr,inter_use With '.F.',;
			bef_aft With .F.,mandatory With '.F.',;
			User_Name With 'ADMIN',tbl_nm With 'Main_vw'  In lother_vw		 && Changes done for Version 3.1 i.e. change tbl_nm from "main_vw.entry_ty+'MAIN'" to "Main_vw"
*Birendra : Changes Done for Mandatory and inter_use field: 03/12/2012: :End:

		Append Blank
		Select lother_vw
		msr = msr + 1
*		Replace e_code With Main_vw.entry_ty, ingrid With .T.,head_nm With 'Amendment Date :',;
fld_nm With 'Amend_Dt',data_ty With 'D',fld_wid With 11,;
fld_dec With 0,att_file With .T.,whn_con With '',;
val_con With '',val_err With '',defa_val With '',;
filtcond With '',serial With msr,inter_use With .F.,;
bef_aft With .F.,mandatory With .F.,;
User_Name With 'ADMIN',tbl_nm With 'Main_vw'  In lother_vw		 && Changes done for Version 3.1 i.e. change tbl_nm from "main_vw.entry_ty+'MAIN'" to "Main_vw"

*Birendra : Changes Done for Mandatory and inter_use field: 03/12/2012: :Start:
		Replace e_code With Main_vw.entry_ty, ingrid With .T.,head_nm With 'Amendment Date :',;
			fld_nm With 'Amend_Dt',data_ty With 'D',fld_wid With 11,;
			fld_dec With 0,att_file With .T.,whn_con With '',;
			val_con With '',val_err With '',defa_val With '',;
			filtcond With '',serial With msr,inter_use With '.F.',;
			bef_aft With .F.,mandatory With '.F.',;
			User_Name With 'ADMIN',tbl_nm With 'Main_vw'  In lother_vw		 && Changes done for Version 3.1 i.e. change tbl_nm from "main_vw.entry_ty+'MAIN'" to "Main_vw"
*Birendra : Changes Done for Mandatory and inter_use field: 03/12/2012: :End:

	Endif
Endif
Endproc

******************************************
*  PRCODURE SRNO	: 2
*  PRCODURE NAME	: VouAftActivate()
*  PURPOSE			: Change toolbar icon and the tool tip only for amendment transaction whcih we have allowed in transactions setting
*  FIRE      		: After "UeTrigVouAftActivate.PRG"
******************************************
Procedure VouAftActivate()
If 'trnamend' $ vChkprod
	If Alltrim(lcode_vw.Amendment) = "WARNING" Or Alltrim(lcode_vw.Amendment) = "STRICT"	&& Checking Amendment Acticated or not Activated for Voucher
*!*			Release  tmpAcpt_Amend, tmpIgnr_Amend, fldcnt, marray1, txt_IgnrFld									&& 12/12/2010
*!*			Release  tmpAcpt_Amend, tmpIgnr_Amend, fldcnt, marray, txt_AcptFld, txt_IgnrFld, Ifldcnt, Imarray 		&& 12/12/2010
		Public tmpAcpt_Amend, tmpIgnr_Amend, fldcnt, marray, txt_AcptFld, txt_IgnrFld, Ifldcnt, Imarray
		tmpAcpt_Amend=''
		fldcnt=''
		txt_AcptFld=''
*!*			tbrdesktop.btnmodi.DisabledPicture = Alltrim(apath)+'\bmp\Amend_text_off.gif'
*!*			tbrdesktop.btnmodi.ToolTipText = "Amendment (Ctrl+E)"
*!*			tbrdesktop.btnmodi.Picture = Alltrim(apath)+'\bmp\Amend_text.gif'
		Do SeekFlds
	Else
		txt_AcptFld = ""
		txt_IgnrFld = ""
*!*			tbrdesktop.btnmodi.DisabledPicture = Alltrim(apath)+'\bmp\edit-off.gif'
*!*			tbrdesktop.btnmodi.ToolTipText = "Amendment (Ctrl+E)"
*!*			tbrdesktop.btnmodi.Picture = Alltrim(apath)+'\bmp\edit.gif'
	Endif
Endif
Endproc

******************************************
*  PRCODURE SRNO	: 3
*  PRCODURE NAME 	: VouNew()
*  PURPOSE			: In voucher new creation this will ristirct users to altered the amendment number data
*  FIRE      		: After "UeTrigVouNew.PRG"
******************************************
Procedure VouNew()
If 'trnamend' $ vChkprod
	If Alltrim(lcode_vw.Amendment) = "WARNING" Or Alltrim(lcode_vw.Amendment) = "STRICT"
		_curvouobj = _Screen.ActiveForm
		i=1
		For i=1 To _curvouobj.ControlCount
*!*				If _curvouobj.Controls[i].Name != "SQLCONOBJ"		&& Commented By Shrikant S. on 15/04/2013 for Bug-8068
			If !Inlist(_curvouobj.Controls[i].Name,"SQLCONOBJ","_BATCHSERIALSTK") && Added By Shrikant S. on 15/04/2013 for Bug-8068
				If _curvouobj.Controls[i].Visible=.T.
					If Inlist(_curvouobj.Controls[i].BaseClass,'Textbox')
						If Inlist(Upper(_curvouobj.Controls[i].ControlSource),Upper("Main_vw.Amend_No"),Upper("Main_vw.Amend_Dt"))
							_curvouobj.Controls[i].Enabled = .F.		&& Disabled Voucherwise fields
						Endif
					Endif
				Endif
			Endif
		Endfor
	Endif
Endif
Endproc

******************************************
*  PRCODURE SRNO	: 4
*  PRCODURE NAME 	: VouCancel()
*  PURPOSE			: This will again setting-up the toolbar after precessing the cancel button in toolbar
*  FIRE      		: After "UeTrigVouCancel.PRG"
******************************************
Procedure VouCancel()
If 'trnamend' $ vChkprod
	_curvouobj = _Screen.ActiveForm
	If Alltrim(lcode_vw.Amendment) = "WARNING" Or Alltrim(lcode_vw.Amendment) = "STRICT"	&& Checking Amendment Acticated or not Activated for Voucher
		Do Dis_Flds
		If Alltrim(lcode_vw.Amendment) = "WARNING" Or Alltrim(lcode_vw.Amendment) = "STRICT"	&& Checking Amendment Acticated or not Activated for Voucher
*!*				tbrdesktop.btnmodi.DisabledPicture = Alltrim(apath)+'\bmp\Amend_text_off.gif'
*!*				tbrdesktop.btnmodi.ToolTipText = "Amendment (Ctrl+E)"
*!*				tbrdesktop.btnmodi.Picture = Alltrim(apath)+'\bmp\Amend_text.gif'
		Else
*!*				tbrdesktop.btnmodi.DisabledPicture = Alltrim(apath)+'\bmp\edit-off.gif'
*!*				tbrdesktop.btnmodi.ToolTipText = "Amendment (Ctrl+E)"
*!*				tbrdesktop.btnmodi.Picture = Alltrim(apath)+'\bmp\edit.gif'
		Endif
	Endif
	Release OldItCnt, NewItCnt, OldAcCnt, NewAcCnt, MsgRlt, OldFld, NewFld, New_aNo, New_aDate, New_aReamrk, OUser, NUser, Ignr_fld, Acpt_fld, rItSr, rAcSr
Endif
Endproc


******************************************
*  PRCODURE SRNO	: 5
*  PRCODURE NAME 	: VouBefEdit()
*  PURPOSE			: This procedure helps to stroes the existing voucher data in cursor and fire Amendment form
*  FIRE      		: After "UeTrigVouBefEdit.PRG"
******************************************
Procedure VouBefEdit()
If 'trnamend' $ vChkprod
	_curvouobj = _Screen.ActiveForm
	If Inlist(Alltrim(lcode_vw.Amendment),"WARNING","STRICT")
		MainAlias = Alias()
*!*	Public OldItCnt, NewItCnt, OldAcCnt, NewAcCnt, MsgRlt, OldFld, NewFld, New_aNo, New_aDate, New_aReamrk, OUser, NUser, Ignr_fld, Acpt_fld
		Public tmpfld, MsgRlt, mAEnt, mANo, mCursorNm, mCursorNm1, mCursorNm2, mCursorNm3, OldItCnt, NewItCnt, OldAcCnt, NewAcCnt, OldFld, NewFld, New_aNo, New_aDate, New_aReamrk, Ignr_fld, Acpt_fld, OUser, NUser, mOldMain, mOldItem, mOldAcdet
		mAEnt		= _curvouobj.PCVTYPE	&& 12/12/2010	'Add this if required
		mANo		= Alltrim(Str(Main_vw.tran_cd,10,0))
		OUser		= Alltrim(Main_vw.User_Name)

		mCursorNm	= mAEnt+mANo+'TblStru'
		mCursorNm1	= mAEnt+mANo+'Temp_Tbl'
		mCursorNm2	= mAEnt+mANo+'NTemp_Tbl'
		mCursorNm3	= mAEnt+mANo+'FinalCur'

		mOldMain	= mAEnt+mANo+'OldMain'
		mOldItem	= mAEnt+mANo+'OldItem'
		mOldAcdet	= mAEnt+mANo+'Oldacdet'

*		SELECT * FROM main_vw INTO CURSOR &mOldMain readwrite
*Birendra : TKT-9757 on 04 oct 2011:Start:
		tmpmain=''
		tmpitem=''
		tmpacdet=''
		ztmpword=''
		If !Empty(lcode_vw.acpt_amend)
			For i = 1 To Getwordcount(Alltrim(lcode_vw.acpt_amend),',')
				ztmpword = Getwordnum(Alltrim(lcode_vw.acpt_amend),i,',')
				ztmpstr = Strtran(ztmpword,"'","",1)
				If Type(&ztmpword)<>'U'
					If 'main_vw' $ Lower(ztmpword)
						tmpmain=Iif(Empty(tmpmain),tmpmain,tmpmain+',')+ztmpstr
					Endif
					If 'item_vw' $ Lower(ztmpword)
						tmpitem=Iif(Empty(tmpitem),tmpitem,tmpitem+',')+ztmpstr
					Endif
					If 'acdet_vw' $ Lower(ztmpword)
						tmpacdet=Iif(Empty(tmpacdet),tmpacdet,tmpacdet+',')+ztmpstr
					Endif
					If 'lmc_vw' $ Lower(ztmpword)
						tmpmain=Iif(Empty(tmpmain),tmpmain,tmpmain+',')+ztmpstr
					Endif
				Else
					ztmpword=Strtran(Alltrim(Upper(ztmpword)),'MAIN_VW','LMC_VW',1,1)
					If Type(&ztmpword)<>'U'
						ztmpstr = Strtran(ztmpword,"'","",1)
						If 'lmc_vw' $ Lower(ztmpword)
							tmpmain=Iif(Empty(tmpmain),tmpmain,tmpmain+',')+ztmpstr
						Endif
					Endif
				Endif
*		ztmpexcldfld = ztmpexcldfld +IIF(EMPTY(ztmpexcldfld),ztmpstr,','+ztmpstr)
			Endfor
		Else
			For i=1 To _curvouobj.ControlCount
*!*				If _curvouobj.Controls[i].Name != "SQLCONOBJ"		&& Commented By Shrikant S. on 15/04/2013 for Bug-8068
				If !Inlist(_curvouobj.Controls[i].Name,"SQLCONOBJ","_BATCHSERIALSTK") && Added By Shrikant S. on 15/04/2013 for Bug-8068
					If _curvouobj.Controls[i].Visible=.T.
						If Inlist(_curvouobj.Controls[i].BaseClass,'Textbox')
							ztmpstr = Alltrim(_curvouobj.Controls[i].ControlSource)
							If !Empty(ztmpstr) And 'LMC_VW' $ Upper(ztmpstr)
								tmpmain=Iif(Empty(tmpmain),tmpmain,tmpmain+',')+ztmpstr
							Endif
						Endif
					Endif
				Endif
			Endfor

		Endif
		If !Empty(tmpmain) And !Empty(lcode_vw.acpt_amend)
			zstrexec=[SELECT main_vw.Entry_ty,main_vw.tran_cd,main_vw.Amend_No,main_vw.Amend_dt,main_vw.amend_remark,]+tmpmain+[ FROM main_vw JOIN lmc_vw ON (main_vw.tran_cd=lmc_vw.tran_cd) INTO CURSOR &mOldMain readwrite]
			&zstrexec
		Else
			If Used('lmc_vw')
				zstrexec=[SELECT main_vw.*]+Iif(Empty(tmpmain),'',','+tmpmain)+[ FROM main_vw JOIN lmc_vw ON (main_vw.tran_cd=lmc_vw.tran_cd) INTO CURSOR &mOldMain readwrite]
				&zstrexec
			Else
				Select * From Main_vw Into Cursor &mOldMain Readwrite
			Endif
		Endif
*Birendra : TKT-9757 on 04 oct 2011:End:

		Select * From item_vw Into Cursor &mOldItem Readwrite
		Select * From acdet_vw Into Cursor &mOldAcdet Readwrite

		Ignr_fld	= "''"
		Acpt_fld	= "''"
		Ignr_fld	= Alltrim(Upper("'main_vw.rule','main_vw.date','item_vw.apgentime','item_vw.qty1','item_vw.Qty2','item_vw.Qty3','item_vw.Qty5','item_vw.l_yn','item_vw.sysdate','item_vw.entry_ty','item_vw.Inv_No','item_vw.doc_no','item_vw.narr','item_vw.item_no'"+txt_IgnrFld))
		Acpt_fld	= txt_AcptFld

		If Used('Amend_Det')=.T.
			Select Amend_Det
			Use
			Select entry_ty,inv_no, Date, Amend_no, Amend_dt, Space(30) As old_user, Space(30) As new_user, Space(20) As tbl_nm, Space(20) As tbl_fld, Space(50) As Old_Value, Space(50) As New_Value, Space(25) As Status, 0 As FileOrd, tran_cd, Space(50) As TRAN_NM,Amend_remark  From Main_vw Where 1=2 Into Cursor Amend_Det Readwrite	&& Create a temparary cursor to store data after amendment field wise
		Else
			Select 0
*!*				Select entry_ty,inv_no, Date, Amend_no, Amend_dt, Space(30) As old_user, Space(30) As new_user, Space(20) As tbl_nm, Space(20) As tbl_fld, Space(50) As Old_Value, Space(50) As New_Value, Space(25) As Status, 0 As FileOrd From Main_vw Where 1=2 Into Cursor Amend_Det Readwrite	&& Create a temparary cursor to store data after amendment field wise
			Select entry_ty,inv_no, Date, Amend_no, Amend_dt, Space(30) As old_user, Space(30) As new_user, Space(20) As tbl_nm, Space(20) As tbl_fld, Space(50) As Old_Value, Space(50) As New_Value, Space(25) As Status, 0 As FileOrd, tran_cd, Space(50) As TRAN_NM,Amend_remark From Main_vw Where 1=2 Into Cursor Amend_Det Readwrite	&& Create a temparary cursor to store data after amendment field wise
		Endif

		New_aNo = Iif(Isnull(Main_vw.Amend_no),0,Main_vw.Amend_no)
		New_aDate = Datetime()
		New_aReamrk =""
		MsgRlt = 0

		If Alltrim(lcode_vw.Amendment) = "STRICT"
			MsgRlt = 6
		Else
			MsgRlt = Messagebox(UEAMENFMENT_QUESTION_01,4,vumess)
		Endif

		If MsgRlt = 6
			Do Form frm_amendment
			Select &mOldMain
			Replace Amend_no With New_aNo In &mOldMain
			Replace Amend_dt With New_aDate In &mOldMain
			Replace Amend_remark With New_aReamrk In &mOldMain
		Endif
		Release rItSr, rAcSr
		If !Empty(MainAlias)
			Select &MainAlias
		Endif
	Endif
Endif
Endproc

******************************************
*  PRCODURE SRNO	: 6
*  PRCODURE NAME 	: VouAftEdit()
*  PURPOSE			: This procedure helps to fire the Ena_Flds() procedure
*  FIRE      		: Fire After "UeTrigVouAftEdit.PRG"
******************************************
Procedure VouAftEdit
If 'trnamend' $ vChkprod
	tmpfld = lcode_vw.acpt_amend
	If Inlist(Alltrim(lcode_vw.Amendment),"WARNING","STRICT")
		If MsgRlt = 6
			Do Ena_Flds
		Else
			_curvouobj = _Screen.ActiveForm
			i=1
			For i=1 To _curvouobj.ControlCount
*!*				If _curvouobj.Controls[i].Name != "SQLCONOBJ"		&& Commented By Shrikant S. on 15/04/2013 for Bug-8068
				If !Inlist(_curvouobj.Controls[i].Name,"SQLCONOBJ","_BATCHSERIALSTK") && Added By Shrikant S. on 15/04/2013 for Bug-8068
					If _curvouobj.Controls[i].Visible=.T. And Inlist(_curvouobj.Controls[i].BaseClass,'Textbox')
						If Inlist(Upper(_curvouobj.Controls[i].ControlSource),Upper("Main_vw.Amend_No"),Upper("Main_vw.Amend_Dt"))
							_curvouobj.Controls[i].Enabled = .F.		&& Disabled Voucherwise fields
						Endif
					Endif
				Endif
			Endfor
		Endif
	Endif
Endif
Endproc

******************************************
*  PRCODURE SRNO	: 7
*  PRCODURE NAME 	: VouFinalUpdate()
*  PURPOSE			: In this procedure we actually stores the all amended data in database
*  FIRE      		: After "UeTrigVouFinalUpdate.PRG"
******************************************
Procedure VouFinalUpdate
If 'trnamend' $ vChkprod
	If Type('msgrlt')='U'
		MsgRlt=6
	Endif
	If Used('lcode_vw') And MsgRlt=6
		If ((Alltrim(lcode_vw.Amendment) = "WARNING" And _curvouobj.EditMode = .T.) Or (Alltrim(lcode_vw.Amendment) = "STRICT" And _curvouobj.EditMode = .T.))
			_curvouobj = _Screen.ActiveForm
			MsgRlt = 0
			mAEnt		= _curvouobj.PCVTYPE
			mANo		= Alltrim(Str(Main_vw.tran_cd,10,0))
			mCursorNm	= mAEnt+'TblStru'
			mCursorNm1	= mAEnt+'Temp_Tbl'
			mCursorNm2	= mAEnt+'NTemp_Tbl'
			mCursorNm3	= mAEnt+'FinalCur'
			NUser = Main_vw.User_Name

			mOldMain	= mAEnt+mANo+'OldMain'
			mOldItem	= mAEnt+mANo+'OldItem'
			mOldAcdet	= mAEnt+mANo+'Oldacdet'

			Select &mOldMain
			New_aNo		= Amend_no
			New_aDate 	= Amend_dt
			New_aReamrk	= Amend_remark

			Select Count(*) From &mOldItem Into Cursor cnt1				&& Count Item Records
			OldItCnt = Cnt
			If Used('cnt1')
				Select cnt1
				Use In cnt1
			Endif

			Select Count(*) From &mOldAcdet Into Cursor cnt1				&& Count Acdet Records
			OldAcCnt = Cnt
			If Used('cnt1')
				Select cnt1
				Use In cnt1
			Endif

			Select 0 As TblSr,Space(50) As TblNm, Space(50) As Ofld1, Space(50) As Nfld1, Space(50) As Ofld, Space(50) As Nfld From item_vw Where 1=2 Order By Ofld Into Cursor &mCursorNm Readwrite	&& Create a temparary cursor to store before & after amendment fields
			Index On TblSr Tag TblSr
			Index On Ofld Tag Ofld
			Index On Nfld Tag Nfld

			Select &mCursorNm

			Append Blank
			Replace TblNm With 'Main'									&& Store Main Table Name
			Replace Ofld1 With 'MnRec1'									&& Store Main Table records counter on TblStru
			Replace Nfld1 With 'MnRec1'									&& Store Main Table records counter on TblStru
			Replace Ofld With 'MnRec1'									&& Store Main Table records counter on TblStru
			Replace TblSr With 1										&& Store Table Serial Order

			If OldItCnt >= 1
				Select &mOldItem
				Go Top
				Do While !Eof()
					rItSr = Val(ItSerial)
					Select &mCursorNm
					Set Order To Ofld
					Append Blank
					Replace TblNm With 'Item'						&& Store Item Table Name
					Replace Ofld1 With 'ItRec'+Allt(Str(rItSr,10))	&& Store Item Table records counter on TblStru
					Replace Nfld1 With 'ItRec'+Allt(Str(rItSr,10))	&& Store Item Table records counter on TblStru
					Replace Ofld With 'ItRec'+Allt(Str(rItSr,10))	&& Store Item Table records counter on TblStru
					Replace TblSr With 2							&& Store Table Serial Order
					Select &mOldItem
					Skip
				Enddo
			Endif

			If OldAcCnt >= 1
				Select &mOldAcdet
				Go Top
				Do While !Eof()
					rAcSr = Val(AcSerial)
					Select &mCursorNm
					Set Order To Ofld
					Append Blank
					Replace TblNm With 'Acdet'						&& Store Item Table Name
					Replace Ofld1 With 'AcRec'+Allt(Str(rAcSr,10))	&& Store Item Table records counter on TblStru
					Replace Nfld1 With 'AcRec'+Allt(Str(rAcSr,10))	&& Store Item Table records counter on TblStru
					Replace Ofld With 'AcRec'+Allt(Str(rAcSr,10))	&& Store Item Table records counter on TblStru
					Replace TblSr With 3							&& Store Table Serial Order
					Select &mOldAcdet
					Skip
				Enddo
			Endif
			Select &mCursorNm
			Set Order To TblSr
			Go Top
			OldFld1 = ""
			OldFld = ""
			Do While !Eof()
				OldFld1 = OldFld1 + ', SPACE(100) as '+Alltrim(Ofld)
				OldFld = OldFld + ', a.'+Alltrim(Ofld)+' as Old'+Alltrim(Ofld)
				Select &mCursorNm
				Skip
			Enddo

			msqlstr = ""
			mCond = ""
			mCond1=""
			mCond2=""

			mCond1=Iif(!Empty(txt_AcptFld),"and column_name in ("+Upper(txt_AcptFld)+")",'')
			mCond2=Iif(!!Isnull(txt_AcptFld),"and column_name in ("+Upper(txt_AcptFld)+")",'')
			mCond=mCond1+mCond2
*			msqlstr = "Select distinct column_name as Col_Nm, 0 as fileord, Table_Name as Table_Name "+OldFld1+" from Information_schema.Columns with (nolock) where table_name in ('"+Main_vw.entry_ty+"Main', '"+Main_vw.entry_ty+"Item' , '"+Main_vw.entry_ty+"Acdet') and column_name not in("+Ignr_fld+") "+mCond+" order by table_name"
*Birendra : for Bug-1138 on 16Jan2012:Commented above line and place below line:
			msqlstr = "Select distinct column_name as Col_Nm, 0 as fileord, Table_Name as Table_Name "+OldFld1+" from Information_schema.Columns with (nolock) where table_name in ('"+Iif(!Empty(lcode_vw.BCODE_NM),lcode_vw.BCODE_NM,Iif(!lcode_vw.ext_vou,lcode_vw.entry_ty,""))+"Main', '"+Iif(!Empty(lcode_vw.BCODE_NM),lcode_vw.BCODE_NM,Iif(!lcode_vw.ext_vou,lcode_vw.entry_ty,""))+"Item' , '"+Iif(!Empty(lcode_vw.BCODE_NM),lcode_vw.BCODE_NM,Iif(!lcode_vw.ext_vou,lcode_vw.entry_ty,""))+"Acdet') and column_name not in("+Ignr_fld+") "+mCond+" order by table_name"
*Iif(!Empty(lcode_vw.BCODE_NM),lcode_vw.BCODE_NM,IIF(!lcode_vw.ext_vou,lcode_vw.entry_ty,""))+"main',"+
			nretval = _curvouobj.SqlConObj.DataConn("EXE",Company.DbName,msqlstr,mCursorNm1,"_curvouobj.nhandle",_curvouobj.DataSessionId)
			If nretval <1
				Messagebox(UEAMENFMENT_ERROR_01,16,vumess)
				Err_Found='YES'
				Return .F.
			Endif
*!*				Select Main_vw
			Select &mCursorNm1
			Go Top
			Do While !Eof()
				tmpfld = mAEnt+mANo+'Old'+Allt(Substr(table_name,3,50))+'.'+Col_Nm
				If Type('&tmpfld')<>'U' &&Birendra : TKT-9757 on 10 Oct. 2011
					tmpfld1 = &tmpfld
					If Type('&tmpfld')='T'												&& Convert DateTime to String
						tmpfld1 = Ttoc(&tmpfld)
					Endif
					If Type('&tmpfld')='M'												&& Convert Memo to String
						tmpfld1  = Chrtranc(&tmpfld,&tmpfld,&tmpfld)
					Endif
					If Type('&tmpfld')='N'												&& Convert Numeric to String
						tmpfld1 = Str(&tmpfld,20,2)
					Endif
					If Type('&tmpfld')='L'												&& Convert Logical to String
						tmpfld1 = Iif(&tmpfld=.T.,'.T.','.F.')
					Endif
					If Allt(Substr(table_name,3,50))="MAIN"					&& replace data fro main table
						rplfld = 'MnRec1'
						Replace &rplfld With tmpfld1 In &mCursorNm1
					Endif
					Replace FileOrd With Iif(Allt(Substr(table_name,3,50))="MAIN",1,Iif(Allt(Substr(table_name,3,50))="ITEM",2,Iif(Allt(Substr(table_name,3,50))="ACDET",3,0))) In &mCursorNm1
				Endif
				Select &mCursorNm1
				Skip
			Enddo


			Select &mOldItem
			Go Top
			Do While !Eof()
				T = Val(ItSerial)
				Select &mCursorNm1
				Go Top
				Do While !Eof()
					tmpfld = mAEnt+mANo+'Old'+Allt(Substr(table_name,3,50))+'.'+Col_Nm
					If Type('&tmpfld')<>'U' &&Birendra : TKT-9757 on 10 Oct. 2011
						tmpfld1 = &tmpfld
						If Type('&tmpfld')='T'												&& Convert DateTime to String
							tmpfld1 = Ttoc(&tmpfld)
						Endif
						If Type('&tmpfld')='M'												&& Convert Memo to String
							tmpfld1  = Chrtranc(&tmpfld,&tmpfld,&tmpfld)
						Endif
						If Type('&tmpfld')='N'												&& Convert Numeric to String
							tmpfld1 = Str(&tmpfld,20,2)
						Endif
						If Type('&tmpfld')='L'												&& Convert Logical to String
							tmpfld1 = Iif(&tmpfld=.T.,'.T.','.F.')
						Endif
						If OldItCnt >= 1 And Allt(Substr(table_name,3,50))="ITEM"	&& replace data fro Item table
							rplfld = 'ItRec'+Allt(Str(T,10))
							Replace &rplfld With tmpfld1 In &mCursorNm1
						Endif
						Replace FileOrd With Iif(Allt(Substr(table_name,3,50))="MAIN",1,Iif(Allt(Substr(table_name,3,50))="ITEM",2,Iif(Allt(Substr(table_name,3,50))="ACDET",3,0))) In &mCursorNm1
					Endif
					Select &mCursorNm1
					Skip
				Enddo
				Select &mOldItem
				Skip
				T=T+1
			Enddo

			Select &mOldAcdet
			Go Top
			Do While !Eof()
				T = Val(AcSerial)
				Select &mCursorNm1
				Go Top
				Do While !Eof()
					tmpfld = mAEnt+mANo+'Old'+Allt(Substr(table_name,3,50))+'.'+Col_Nm
					If Type('&tmpfld')<>'U' &&Birendra : TKT-9757 on 10 Oct. 2011
						tmpfld1 = &tmpfld
						If Type('&tmpfld')='T'												&& Convert DateTime to String
							tmpfld1 = Ttoc(&tmpfld)
						Endif
						If Type('&tmpfld')='M'												&& Convert Memo to String
							tmpfld1  = Chrtranc(&tmpfld,&tmpfld,&tmpfld)
						Endif
						If Type('&tmpfld')='N'												&& Convert Numeric to String
							tmpfld1 = Str(&tmpfld,20,2)
						Endif
						If Type('&tmpfld')='L'												&& Convert Logical to String
							tmpfld1 = Iif(&tmpfld=.T.,'.T.','.F.')
						Endif
						If OldAcCnt >= 1 And Allt(Substr(table_name,3,50))="ACDET"	&& replace data fro Acdet table
							rplfld = 'AcRec'+Allt(Str(T,10))
							Replace &rplfld With tmpfld1 In &mCursorNm1
						Endif
						Replace FileOrd With Iif(Allt(Substr(table_name,3,50))="MAIN",1,Iif(Allt(Substr(table_name,3,50))="ITEM",2,Iif(Allt(Substr(table_name,3,50))="ACDET",3,0))) In &mCursorNm1
					Endif
					Select &mCursorNm1
					Skip
				Enddo
				Select &mOldAcdet
				Skip
				T=T+1
			Enddo

			Select Count(*) From item_vw Into Cursor cnt1				&& Count Item Records
			NewItCnt = Cnt
			If Used('cnt1')
				Select cnt1
				Use In cnt1
			Endif

			Select Count(*) From acdet_vw Into Cursor cnt1				&& Count Acdet Records
			NewAcCnt = Cnt
			If Used('cnt1')
				Select cnt1
				Use In cnt1
			Endif

			Select &mCursorNm
			Set Order To Ofld
			If Seek('MnRec1')=.T.
				Replace Nfld With 'MnRec1'												&& Store Main Table records counter on TblStru
			Endif
			If NewItCnt >= 1
				Select item_vw
				Go Top
				Do While !Eof()
					rItSr = Val(ItSerial)
					If NewItCnt >= OldItCnt
						Select &mCursorNm
						Set Order To Ofld
						If Seek('ItRec'+Allt(Str(rItSr,10)))=.T.
							Replace Nfld With 'ItRec'+Allt(Str(rItSr,10))				&& Store Item Table records counter on TblStru
						Else
							Append Blank												&& Add new Row in TblStru
							Replace Nfld With 'ItRec'+Allt(Str(rItSr,10))				&& Store Item Table records counter on TblStru
							Replace Nfld1 With 'ItRec'+Allt(Str(rItSr,10))				&& Store Item Table records counter on TblStru
							Replace Ofld1 With 'ItRec'+Allt(Str(rItSr,10))				&& Store Item Table records counter on TblStru
							Replace Ofld With 'Space (5) OldItRec'+Allt(Str(rItSr,10))  && Store Item Table records counter on TblStru
							Replace TblNm With 'Item'									&& Store Item Table Name
							Replace TblSr With 2										&& Store Table Serial Order
						Endif
					Endif
					If NewItCnt < OldItCnt
						Select &mCursorNm
						Set Order To Ofld
						If Seek('ItRec'+Allt(Str(rItSr,10)))=.T.
							If Alltrim(Ofld) = 'ItRec'+Allt(Str(rItSr,10))
								Replace Nfld With 'ItRec'+Allt(Str(rItSr,10))				&& Store Item Table records counter on TblStru
							Endif
						Endif
					Endif
					Select item_vw
					Skip
				Enddo
				Select &mCursorNm
				Set Order To Ofld
				Scan For Empty(Alltrim(Nfld))
					Replace Nfld With 'Space (5) '+Allt(Nfld1)  && Find Out the deleted record
				Endscan
			Endif
			If NewAcCnt >= 1
				Select acdet_vw
				Go Top
				Do While !Eof()
					rAcSr = Val(AcSerial)
					If NewAcCnt >= OldAcCnt
						Select &mCursorNm
						Set Order To Ofld
						If Seek('AcRec'+Allt(Str(rAcSr,10)))=.T.
							Replace Nfld With 'AcRec'+Allt(Str(rAcSr,10))				&& Store Item Table records counter on TblStru
						Else
							Append Blank												&& Add new Row in TblStru
							Replace Nfld With 'AcRec'+Allt(Str(rAcSr,10))				&& Store Item Table records counter on TblStru
							Replace Nfld1 With 'AcRec'+Allt(Str(rAcSr,10))				&& Store Item Table records counter on TblStru
							Replace Ofld1 With 'AcRec'+Allt(Str(rAcSr,10))				&& Store Item Table records counter on TblStru
							Replace Ofld With 'Space (5) OldAcRec'+Allt(Str(rAcSr,10))  && Store Item Table records counter on TblStru
							Replace TblNm With 'Acdet'									&& Store Item Table Name
							Replace TblSr With 3										&& Store Table Serial Order
						Endif
					Endif
					If NewAcCnt < OldAcCnt
						Select &mCursorNm
						Set Order To Ofld
						If Seek('AcRec'+Allt(Str(rAcSr,10)))=.T.
							If Alltrim(Ofld) = 'AcRec'+Allt(Str(rAcSr,10))
								Replace Nfld With 'AcRec'+Allt(Str(rAcSr,10))				&& Store Item Table records counter on TblStru
							Endif
						Endif
					Endif
					Select acdet_vw
					Skip
				Enddo
				Select &mCursorNm
				Set Order To Ofld
				Scan For Empty(Alltrim(Nfld))
					Replace Nfld With 'Space (5) '+Allt(Nfld1)  && Find Out the deleted record
				Endscan
			Endif
			Select &mCursorNm
			Set Order To TblSr
			Go Top
			NewFld = ""
			NewFld1 = ""
			flt = ""
			Do While !Eof()
				NewFld1 = NewFld1 + Iif(Alltrim(Nfld1)=Alltrim(Nfld),', SPACE(100) as '+Alltrim(Nfld),Iif(Alltrim(Nfld1)!=Alltrim(Nfld),', '+Alltrim(Nfld),''))
				NewFld = NewFld + Iif(Alltrim(Ofld1)=Alltrim(Ofld),', b.'+Alltrim(Nfld1)+' as New'+Alltrim(Nfld1), ', '+Alltrim(Ofld)+', b.'+Alltrim(Nfld1)+' as New'+Alltrim(Nfld1))
				flt = flt + Iif(Alltrim(Ofld1)=Alltrim(Ofld),' Or a.'+Alltrim(Ofld1)+' <> '+'b.'+Alltrim(Nfld1),' Or !EMPTY('+Alltrim(Nfld1)+')')
				Select &mCursorNm
				Skip
			Enddo
			msqlstr = ""
			mCond=""
			mCond=Iif(!Empty(txt_AcptFld),"and column_name in ("+Upper(txt_AcptFld)+")",'')
*		msqlstr = "Select distinct column_name as Col_Nm, 0 as fileord, Table_Name as Table_Name "+NewFld1+" from Information_schema.Columns with (nolock) where table_name in ('"+Main_vw.entry_ty+"Main', '"+Main_vw.entry_ty+"Item' , '"+Main_vw.entry_ty+"Acdet')  and column_name not in("+Ignr_fld+")"+mCond+" order by table_name"
*Birendra: for Bug-1138 on 16jan2012:Commented Above line modify below one:
			msqlstr = "Select distinct column_name as Col_Nm, 0 as fileord, Table_Name as Table_Name "+NewFld1+" from Information_schema.Columns with (nolock) where table_name in ('"+Iif(!Empty(lcode_vw.BCODE_NM),lcode_vw.BCODE_NM,Iif(!lcode_vw.ext_vou,lcode_vw.entry_ty,""))+"Main', '"+Iif(!Empty(lcode_vw.BCODE_NM),lcode_vw.BCODE_NM,Iif(!lcode_vw.ext_vou,lcode_vw.entry_ty,""))+"Item' , '"+Iif(!Empty(lcode_vw.BCODE_NM),lcode_vw.BCODE_NM,Iif(!lcode_vw.ext_vou,lcode_vw.entry_ty,""))+"Acdet')  and column_name not in("+Ignr_fld+")"+mCond+" order by table_name"

			nretval = _curvouobj.SqlConObj.DataConn("EXE",Company.DbName,msqlstr,mCursorNm2,"_curvouobj.nhandle",_curvouobj.DataSessionId)
			If nretval <1
				Messagebox(UEAMENFMENT_ERROR_02,16,vumess)
				Err_Found='YES'
				Return .F.
			Endif
*!*			Select Main_vw
			Select &mCursorNm2
			Go Top
			Do While !Eof()
				If Allt(Substr(table_name,3,50))="MAIN"					&& replace data fro main table
					tmpfld = Allt(Substr(table_name,3,50))+'_vw.'+Alltrim(Col_Nm)
*birendra : TKT-9757 on 04 oct 2011:Start:
					ztmpword="'"+tmpfld+"'"
					If Type(&ztmpword)='U'
						ztmpword=Strtran(Alltrim(Upper(ztmpword)),'MAIN_VW','LMC_VW',1,1)
						If Type(&ztmpword)<>'U'
							tmpfld=Strtran(ztmpword,"'","",1)
						Endif
					Endif
*birendra : TKT-9757 on 04 oct 2011:Start:

					tmpfld1 = &tmpfld
					If Type('&tmpfld')='T'												&& Convert DateTime to String
						tmpfld1 = Ttoc(&tmpfld)
					Endif
					If Type('&tmpfld')='M'												&& Convert Memo to String
						tmpfld1  = Chrtranc(&tmpfld,&tmpfld,&tmpfld)
					Endif
					If Type('&tmpfld')='N'												&& Convert Numeric to String
						tmpfld1 = Str(&tmpfld,20,2)
					Endif
					If Type('&tmpfld')='L'												&& Convert Logical to String
						tmpfld1 = Iif(&tmpfld=.T.,'.T.','.F.')
					Endif
					rplfld = 'MnRec1'
					Replace &rplfld With tmpfld1 In &mCursorNm2
					Replace FileOrd With Iif(Allt(Substr(table_name,3,50))="MAIN",1,Iif(Allt(Substr(table_name,3,50))="ITEM",2,Iif(Allt(Substr(table_name,3,50))="ACDET",3,0))) In &mCursorNm2
				Endif
				Select &mCursorNm2
				Skip
			Enddo

			Select item_vw
			Go Top
			Do While !Eof()
				T = Val(ItSerial)
				Select &mCursorNm2
				Go Top
				Do While !Eof()
					If OldItCnt >= 1 And Allt(Substr(table_name,3,50))="ITEM"	&& replace data fro Item table
						tmpfld = Allt(Substr(table_name,3,50))+'_vw.'+Alltrim(Col_Nm)
						tmpfld1 = &tmpfld
						If Type('&tmpfld')='T'												&& Convert DateTime to String
							tmpfld1 = Ttoc(&tmpfld)
						Endif
						If Type('&tmpfld')='M'												&& Convert Memo to String
							tmpfld1  = Chrtranc(&tmpfld,&tmpfld,&tmpfld)
						Endif
						If Type('&tmpfld')='N'												&& Convert Numeric to String
							tmpfld1 = Str(&tmpfld,20,2)
						Endif
						If Type('&tmpfld')='L'												&& Convert Logical to String
							tmpfld1 = Iif(&tmpfld=.T.,'.T.','.F.')
						Endif
						rplfld = 'ItRec'+Allt(Str(T,10))
						Replace &rplfld With tmpfld1 In &mCursorNm2
						Replace FileOrd With Iif(Allt(Substr(table_name,3,50))="MAIN",1,Iif(Allt(Substr(table_name,3,50))="ITEM",2,Iif(Allt(Substr(table_name,3,50))="ACDET",3,0))) In &mCursorNm2
					Endif
					Select &mCursorNm2
					Skip
				Enddo
				Select item_vw
				Skip
				T=T+1
			Enddo

			Select acdet_vw
			Go Top
			Do While !Eof()
				T = Val(AcSerial)
				Select &mCursorNm2
				Go Top
				Do While !Eof()
					If OldAcCnt >= 1 And Allt(Substr(table_name,3,50))="ACDET"	&& replace data fro Acdet table
						tmpfld = Allt(Substr(table_name,3,50))+'_vw.'+Alltrim(Col_Nm)
						tmpfld1 = &tmpfld
						If Type('&tmpfld')='T'												&& Convert DateTime to String
							tmpfld1 = Ttoc(&tmpfld)
						Endif
						If Type('&tmpfld')='M'												&& Convert Memo to String
							tmpfld1  = Chrtranc(&tmpfld,&tmpfld,&tmpfld)
						Endif
						If Type('&tmpfld')='N'												&& Convert Numeric to String
							tmpfld1 = Str(&tmpfld,20,2)
						Endif
						If Type('&tmpfld')='L'												&& Convert Logical to String
							tmpfld1 = Iif(&tmpfld=.T.,'.T.','.F.')
						Endif
						rplfld = 'AcRec'+Allt(Str(T,10))
						Replace &rplfld With tmpfld1 In &mCursorNm2
						Replace FileOrd With Iif(Allt(Substr(table_name,3,50))="MAIN",1,Iif(Allt(Substr(table_name,3,50))="ITEM",2,Iif(Allt(Substr(table_name,3,50))="ACDET",3,0))) In &mCursorNm2
					Endif
					Select &mCursorNm2
					Skip
				Enddo
				Select acdet_vw
				Skip
				T=T+1
			Enddo

*!*			If Used('FinalCur')
*!*				Select FinalCur
*!*				Use
*!*			Endif

*!*			Messagebox(OldFld,64,'OldFld')
*!*			Messagebox(NewFld,64,'NewFld')

			Select Distinct Upper(a.Col_Nm) As Col_Nm, a.FileOrd, Upper(a.table_name) As table_name &OldFld &NewFld From &mCursorNm1 a Left Join &mCursorNm2 b On a.table_name=b.table_name And a.Col_Nm=b.Col_Nm Where a.Col_Nm<>b.Col_Nm &flt Order By a.FileOrd Into Cursor &mCursorNm3 Readwrite
			If !Used('&mCursorNm3')
*!*				Messagebox("TmpFile '+&mCursorNm3+'not created, can't Proceed...!",16,vumess)
				Messagebox(UEAMENFMENT_ERROR_03,16,vumess)
				Err_Found='YES'
				Return .F.
			Endif
			Select &mCursorNm
			Go Top
			Do While !Eof()
				tmptblnam = ''
				tmptblfld = ''
				chktblnm = Alltrim(TblNm)
				chkOldfld = ""
				chkNewfld = ""
				chkOldfld = Alltrim(Ofld)
				chkNewfld = Alltrim(Nfld)
				rold = Alltrim(Ofld1)
				rnew = Alltrim(Nfld1)
				tmpOfld = mCursorNm3+'.Old'+rold
				tmpNfld = mCursorNm3+'.New'+rnew
				Select &mCursorNm3
				Go Top
				Do While !Eof()
					tmptblnam = ''
					tmptblfld = ''
					tmpFileOrd = ''
					tmptblnam = table_name
					tmptblfld = Col_Nm
					tmpFileOrd = FileOrd
					Select Amend_Det
					Append Blank
					Replace entry_ty With Main_vw.entry_ty In Amend_Det
					Replace inv_no With Main_vw.inv_no In Amend_Det
					Replace Date With Main_vw.Date In Amend_Det
					Replace tran_cd With Main_vw.tran_cd In Amend_Det
					Replace Amend_no With New_aNo In Amend_Det
					Replace Amend_dt With New_aDate In Amend_Det
					Replace old_user With OUser In Amend_Det
					Replace new_user With NUser In Amend_Det
					Replace tbl_nm With tmptblnam In Amend_Det
					Replace tbl_fld With tmptblfld In Amend_Det
					Replace Old_Value With &tmpOfld In Amend_Det
					Replace New_Value With &tmpNfld In Amend_Det
*Birendra :  Bug-3443 on 14/04/2012 :Start:
					Replace Amend_remark With Main_vw.Amend_remark In Amend_Det
*Birendra :  Bug-3443 on 14/04/2012 :End:
					Replace TRAN_NM With Alltrim(_curvouobj.Caption) In Amend_Det
					If chkOldfld = chkNewfld
						Replace Status With "Record Altered No : "+Alltrim(Subs(tmpNfld,20,10)) In Amend_Det
					Endif
					If Substr(chkNewfld,1,5)="Space"
						Replace Status With "Record Deleted No : "+Alltrim(Subs(tmpNfld,20,10)) In Amend_Det
					Endif
					If Substr(chkOldfld,1,5)="Space"
						Replace Status With "Record Added No   : "+Alltrim(Subs(tmpNfld,20,10)) In Amend_Det
					Endif
					Replace FileOrd With tmpFileOrd In Amend_Det
					Select &mCursorNm3
					Skip
				Enddo
				Select &mCursorNm
				Skip
			Enddo

			Select Amend_Det
			Delete For Old_Value = New_Value
			Select Count(*) As rcount From Amend_Det Into Cursor TmpAmendCur
			If rcount = 0
				Select Amend_Det
				Append Blank
				Replace entry_ty With Main_vw.entry_ty In Amend_Det
				Replace inv_no With Main_vw.inv_no In Amend_Det
				Replace Date With Main_vw.Date In Amend_Det
				Replace tran_cd With Main_vw.tran_cd In Amend_Det
				Replace Amend_no With New_aNo In Amend_Det
				Replace Amend_dt With New_aDate In Amend_Det
				Replace old_user With OUser In Amend_Det
				Replace new_user With NUser In Amend_Det
				Replace tbl_nm With '' In Amend_Det
				Replace tbl_fld With '' In Amend_Det
				Replace Old_Value With '' In Amend_Det
				Replace New_Value With '' In Amend_Det
				Replace TRAN_NM With Alltrim(_curvouobj.Caption) In Amend_Det
				Replace Status With 'NO CHANGES DONE' In Amend_Det
				Replace FileOrd With 0 In Amend_Det
			Endif
			If Used('TmpAmendCur')
				Select TmpAmendCur
				Use
			Endif
			Select Amend_Det

			If Used('Amend_det')
				Select Amend_Det
				Go Top
				Do While !Eof()
					msqlstr =""
					msqlstr = _curvouobj.SqlConObj.GenInsert(Company.DbName+"..Amend_Detail","","","Amend_det",mvu_backend)
					etsql_con = 0
					etsql_con = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,msqlstr,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
					If etsql_con  < 1
*!*						Messagebox(msqlstr,16,"Error Found in insert AMENDMENT statement ")
						Messagebox(UEAMENFMENT_ERROR_04,16,vumess)
						Err_Found='YES'
						Return .F.
					Endif
					Select Amend_Det
					Skip
				Enddo
			Endif
			Do Dis_Flds

			If Used('&mCursorNm')
				Select &mCursorNm
				Use In &mCursorNm
			Endif
			If Used('&mCursorNm1')
				Select &mCursorNm1
				Use In &mCursorNm1
			Endif
			If Used('&mCursorNm2')
				Select &mCursorNm2
				Use In &mCursorNm2
			Endif
			If Used('&mCursorNm3')
				Select &mCursorNm3
				Use In &mCursorNm3
			Endif
			If Used('Amend_Det')
				Select Amend_Det
				Use In Amend_Det
			Endif
		Endif
	Endif
	Release OldItCnt, NewItCnt, OldAcCnt, NewAcCnt, MsgRlt, OldFld, NewFld, New_aNo, New_aDate, New_aReamrk, OUser, NUser, Ignr_fld, Acpt_fld, rItSr, rAcSr
Endif
Endproc


******************************************
*  PRCODURE SRNO	: 8
*  PRCODURE NAME 	: VouToolSave()
*  PURPOSE			: This procedure we used for 2 major works i.e. as below
*					: 1) To store the Amendmend details in views and
*					: 2) To store the data at runtime in database for transaction setting
*  FIRE      		: After "UeTrigVouToolSave.PRG"
******************************************
Procedure VouToolSave
If 'trnamend' $ vChkprod
	_curvouobj = _Screen.ActiveForm
	If _curvouobj.EditMode = .T. And _curvouobj.Caption <> "Transaction Setting"
		If Used('lcode_vw')
			If Alltrim(lcode_vw.Amendment) = "WARNING" Or Alltrim(lcode_vw.Amendment) = "STRICT"
				Select Main_vw											&& 12/12/2010
				Replace Amend_no With New_aNo In Main_vw
				Replace Amend_dt With New_aDate In Main_vw
				Replace Amend_remark With New_aReamrk In Main_vw
			Endif
		Endif
	Endif
* Commented By Birendra
*!*		If _curvouobj.Caption = "Transaction Setting"
*!*	*!*			PgFound=.F.
*!*	*!*			i=1
*!*	*!*			For i=1 To _curvouobj.ControlCount
*!*	*!*				If _curvouobj.Controls[i].BaseClass="Pageframe"
*!*	*!*					j=1
*!*	*!*					For j=1 To _curvouobj.Controls[i].PageCount
*!*	*!*						If _curvouobj.Controls[i].Pages[j].Caption="A\<mendment Details"
*!*	*!*							PgFound=.T.
*!*	*!*							pgcnt = j
*!*	*!*							PgFrm = i
*!*	*!*							EXIT && Birendra
*!*	*!*						Endif
*!*	*!*					Endfor
*!*	*!*				Endif
*!*	*!*			Endfor
*!*	*!*			_curvouobj.Controls[PgFrm].PageCount=pgcnt
*!*	*!*			_Amend_Page = _curvouobj.Controls[PgFrm].Pages[pgcnt]

*!*			If "OrdAmend" $ vchkprod
*!*	*!*				_Amend_Page.oTxt1.Enabled=.F.  &&Birendra
*!*	*!*				_Amend_Page.oETxt1.Enabled=.F.
*!*				If Inlist(Alltrim(Upper(_lcode.Amendment)),'WARNING','STRICT')

*!*					msqlstr =""
*!*					etsql_con = 0
*!*					msqlstr = "select column_name from INFORMATION_SCHEMA.columns where table_name = '"+Alltrim(Iif(!Empty(_LCODE.BCODE_NM),_LCODE.BCODE_NM,_LCODE.entry_ty))+"MAIN' and column_name in('Amend_No','Amend_Dt','Amend_Remark')"
*!*					etsql_con = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,msqlstr,[FldChk_Vw],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
*!*					If etsql_con  < 1
*!*	*!*						Messagebox("Probelm in adding fields because its already exist",64,vumess)
*!*						Messagebox(UEAMENFMENT_MESSAGE_01,64,vumess)
*!*					Endif

*!*					Select FldChk_Vw
*!*					Locate For ALLTRIM(UPPER(column_name)) = UPPER('Amend_No') &&Birendra
*!*					If !Found()
*!*						msqlstr =''
*!*						etsql_con = 0
*!*						msqlstr = 'Alter table '+Iif(!Empty(_LCODE.BCODE_NM),_LCODE.BCODE_NM,_LCODE.entry_ty)+'main add [Amend_No] [int] '
*!*						etsql_con = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,msqlstr,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
*!*					Endif

*!*					Select FldChk_Vw
*!*					Locate For ALLTRIM(UPPER(column_name)) = UPPER('Amend_Dt') &&Birendra
*!*					If !Found()
*!*						msqlstr =''
*!*						etsql_con = 0
*!*						msqlstr = 'Alter table '+Iif(!Empty(_LCODE.BCODE_NM),_LCODE.BCODE_NM,_LCODE.entry_ty)+'main add [Amend_Dt] [datetime]'
*!*						etsql_con = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,msqlstr,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
*!*					Endif

*!*					Select FldChk_Vw
*!*					Locate For ALLTRIM(UPPER(column_name)) = UPPER('Amend_Remark')
*!*					If !Found()
*!*						msqlstr =''
*!*						etsql_con = 0
*!*						msqlstr = 'Alter table '+Iif(!Empty(_LCODE.BCODE_NM),_LCODE.BCODE_NM,_LCODE.entry_ty)+'main add [Amend_Remark] [Varchar] (150)'
*!*						etsql_con = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,msqlstr,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
*!*					Endif

*!*				Endif

*!*			Endif


*!*			If File('IgFldLst.dbf')
*!*				If Used('IgFldLst')
*!*					Select IgFldLst
*!*					Use In IgFldLst
*!*				Endif
*!*			Endif

*!*			If File('AcFldLst.dbf')
*!*				If Used('AcFldLst')
*!*					Select AcFldLst
*!*					Use In AcFldLst
*!*				Endif
*!*			Endif

*!*			If Used('TmpLst')
*!*				Select TmpLst
*!*				Use
*!*			Endif
*!*			If Used('TmpLst1')
*!*				Select TmpLst1
*!*				Use
*!*			Endif

*!*		Endif


*!*		If Used('FldChk_Vw')
*!*			Select FldChk_Vw
*!*			Use In FldChk_Vw
*!*		ENDIF
Endif
Endproc

******************************************
*  PRCODURE SRNO	: 9
*  PRCODURE NAME 	: VouToolEdit()
*  PURPOSE			: This procedure we used for 2 major works i.e. as below
*					: 1) To store the Amendmend details in views and
*					: 2) To store the data at runtime in database for transaction setting
*  FIRE      		: After "UeTrigVouToolEdit.PRG"
****************************************** Birendra Commented
Procedure VouToolEdit
*!*	  IF 'OrdAmend' $ vChkprod
*!*		_curvouobj = _Screen.ActiveForm
*!*		If _curvouobj.Caption = "Transaction Setting"		&& Addng New Tab in Transaction Setting
*!*			Public Olst1_status, Olst2_status, CheckIcon, Uncheckicon, tmpAcpt_Amend, tmpIgnr_Amend, marray, FieldsData
*!*			Olst1_status=.T.
*!*			Olst2_status=.T.
*!*			MainAlias = Alias()

*!*			If "_Amendment" $ Upper(Set('classlib'))

*!*			Else
*!*				Set Classlib To _Amendment Additive
*!*			Endif

*!*			PgFound=.F.
*!*			i=1
*!*			For i=1 To _curvouobj.ControlCount
*!*				If _curvouobj.Controls[i].BaseClass="Pageframe"
*!*					j=1
*!*					For j=1 To _curvouobj.Controls[i].PageCount
*!*						If _curvouobj.Controls[i].Pages[j].Caption="A\<mendment Details"
*!*							PgFound=.T.
*!*							pgcnt = _curvouobj.Controls[i].PageCount
*!*							PgFrm = i
*!*						Else
*!*							PgFound=.F.
*!*							pgcnt = _curvouobj.Controls[i].PageCount + 1
*!*							PgFrm = i
*!*						Endif
*!*					Endfor
*!*				Endif
*!*			Endfor
*!*			_curvouobj.Controls[PgFrm].PageCount=pgcnt
*!*			_Amend_Page = _curvouobj.Controls[PgFrm].Pages[pgcnt]

*!*			If PgFound=.F.
*!*				_curvouobj.Controls[PgFrm].PageCount=pgcnt
*!*				_Amend_Page = _curvouobj.Controls[PgFrm].Pages[pgcnt]
*!*				_Amend_Page.Caption="A\<mendment Details"	&& Assign Caption to New Generated Tab
*!*				_Amend_Page.FontBold = .F.					&& Assign Font to New Generated Tab
*!*				_Amend_Page.ColorSource = 4 				&& Assign Color Source to New Generated Tab
*!*				_Amend_Page.FontSize = 8					&& Assign Font Size to New Generated Tab
*!*				_Amend_Page.BackColor = Rgb(240,240,240) 	&& Assign Back Color to New Generated Tab
*!*				_Amend_Page.PageOrder = pgcnt 				&& Assign Page Orderto New Generated Tab

*!*	*Birendra  : commented not required
*!*	*			oShp1 = Createobject("Shape")				&& Create a Object for Label
*!*				_Amend_Page.AddObject( 'oShp1', '_Shape' )
*!*				_Amend_Page.oShp1.Top=4
*!*				_Amend_Page.oShp1.Left=4
*!*				_Amend_Page.oShp1.Height=329
*!*				_Amend_Page.oShp1.Width=562
*!*				_Amend_Page.oShp1.BackStyle=0
*!*				_Amend_Page.oShp1.SpecialEffect = 0
*!*				_Amend_Page.oShp1.Visible=.T.

*!*	*Birendra  : commented not required
*!*	*			oLbl1 = Createobject("label")				&& Create a Object for Label
*!*				_Amend_Page.AddObject( 'oLbl1', '_label' )
*!*				_Amend_Page.oLbl1.Caption='Amendment Style'
*!*				_Amend_Page.oLbl1.AutoSize=.T.
*!*				_Amend_Page.oLbl1.Top=20
*!*				_Amend_Page.oLbl1.Left=20
*!*				_Amend_Page.oLbl1.FontSize = 8
*!*				_Amend_Page.oLbl1.Visible=.T.

*!*	*Birendra  : commented not required
*!*	*			oTxt1 = Createobject("TextBox")				&& Create a Object for Label
*!*				_Amend_Page.AddObject( 'oTxt1', '_TextBox' )
*!*				_Amend_Page.oTxt1.FontSize = 8
*!*				_Amend_Page.oTxt1.Top=20
*!*				_Amend_Page.oTxt1.Left=	150	&&300
*!*				_Amend_Page.oTxt1.Width= 150	&&300
*!*				_Amend_Page.oTxt1.MaxLength= 8
*!*				_Amend_Page.oTxt1.ControlSource = "_lcode.Amendment"
*!*				_Amend_Page.oTxt1.format = 'M'  && Birendra
*!*				_Amend_Page.oTxt1.inputmask = "NO,STRICT,WARNING" && Birendra
*!*				_Amend_Page.oTxt1.Visible=.T.
*!*				_Amend_Page.oTxt1.Enabled=.T.
*!*	*Birendra  : commented not required
*!*	*			oLbl2 = Createobject("label")				&& Create a Object for Label
*!*				_Amend_Page.AddObject( 'oLbl2', '_label' )
*!*				_Amend_Page.oLbl2.Caption='Accept Amendment only for below fields :'
*!*				_Amend_Page.oLbl2.AutoSize=.T.
*!*				_Amend_Page.oLbl2.Top=60
*!*				_Amend_Page.oLbl2.Left=20
*!*				_Amend_Page.oLbl2.FontSize = 8
*!*				_Amend_Page.oLbl2.Visible=.T.
*!*	*Birendra  : commented not required
*!*	*			oETxt1 = Createobject("EditBox")				&& Create a Object for Label
*!*				_Amend_Page.AddObject( 'oETxt1', '_EditBox' )
*!*				_Amend_Page.oETxt1.FontSize = 8
*!*				_Amend_Page.oETxt1.Top=80
*!*				_Amend_Page.oETxt1.Left= 20	&&300
*!*				_Amend_Page.oETxt1.Width= 500
*!*				_Amend_Page.oETxt1.ControlSource = "_lcode.Acpt_Amend"
*!*				_Amend_Page.oETxt1.Visible=.T.
*!*				_Amend_Page.oETxt1.Enabled=.T.

*!*			Else
*!*				_Amend_Page.oTxt1.Enabled=.T.
*!*				_Amend_Page.oETxt1.Enabled=.T.
*!*				_Amend_Page.oTxt1.ControlSource = "_lcode.Amendment"
*!*				_Amend_Page.oETxt1.ControlSource = "_lcode.Acpt_Amend"
*!*			Endif
*!*		Endif
*!*	  ENDIF
Endproc


******************************************
*  PRCODURE SRNO	: 9
*  PRCODURE NAME 	: SeekFlds()
*  PURPOSE			: This procedure used to find the fields name as per per the transaction setting and store it in multi-dimentional array
*  FIRE      		: This will not fired in any of the udyog standard programe
******************************************
Procedure SeekFlds
If 'trnamend' $ vChkprod
	tmpAcpt_Amend = ""
	tmpIgnr_Amend = ""
	tmpAcpt_Amend = Iif(!Empty(Alltrim(Upper(lcode_vw.acpt_amend))),Alltrim(Upper(lcode_vw.acpt_amend))+",",'')		&& store fields string in tmpAcpt_Amend
	tmpIgnr_Amend = Iif(!Empty(Alltrim(Upper(lcode_vw.Ignr_Amend))),Alltrim(Upper(lcode_vw.Ignr_Amend))+",",'')		&& store fields string in tmpIgnr_Amend
	fldloop=1				&& variable for 'FOR Looping'
	fldcnt=0				&& variable for Fields Counter
	scnt =2
	ecnt =1
	txt_AcptFld = ""
	fldcnt1=0				&& variable for Fields Counter
	scnt1 =1
	ecnt1 =1
	fldcnt2=0				&& variable for Fields Counter
	scnt2 =1
	ecnt2 =1
	If Len(Allt(tmpAcpt_Amend)) > 0
		For fldloop=1 To Len(Allt(tmpAcpt_Amend))
			If Subs(tmpAcpt_Amend,fldloop,fldloop)=","
				fldcnt = fldcnt +1
				ecnt=fldloop-scnt-1
				Dimension marray(fldcnt)
				Store Substr(tmpAcpt_Amend,scnt,ecnt) To marray(fldcnt)
				scnt=fldloop+2
			Endif
			If Subs(tmpAcpt_Amend,fldloop,fldloop)="."
				scnt1=fldloop+1
			Endif
			If Subs(tmpAcpt_Amend,fldloop,fldloop)=","
				ecnt1=fldloop-scnt1-1
				txt_AcptFld=txt_AcptFld+"'"+Substr(tmpAcpt_Amend,scnt1,ecnt1)+"',"
				fldcnt1 = fldcnt1 +1
			Endif
*!*				MESSAGEBOX(txt_AcptFld,64,ALLTRIM(STR(fldloop,5,0))+'rajesh2')
		Endfor
		txt_AcptFld = Substr(txt_AcptFld,1,Len(txt_AcptFld)-1) && Birendra
*!*					MESSAGEBOX(txt_AcptFld,64,'rajesh3')
	Endif

	fldloop=1				&& variable for 'FOR Looping'
	Ifldcnt=0				&& variable for Fields Counter
	scnt =2
	ecnt =1
	txt_IgnrFld = ""
	fldcnt1=0				&& variable for Fields Counter
	scnt1 =1
	ecnt1 =1
	fldcnt2=0				&& variable for Fields Counter
	scnt2 =1
	ecnt2 =1
	If Len(Allt(tmpIgnr_Amend)) > 0
*!*				MESSAGEBOX('entered in loop2')
		For fldloop=1 To Len(Allt(tmpIgnr_Amend))
			If Subs(tmpIgnr_Amend,fldloop,fldloop)=","
				Ifldcnt = Ifldcnt +1
				ecnt=fldloop-scnt-1
				Dimension marray(Ifldcnt)
				Store Substr(tmpIgnr_Amend,scnt,ecnt) To marray(Ifldcnt)
				scnt=fldloop+2
			Endif
			If Subs(tmpIgnr_Amend,fldloop,fldloop)="."
				scnt1=fldloop+1
			Endif
			If Subs(tmpIgnr_Amend,fldloop,fldloop)=","
				ecnt1=fldloop-scnt1-1
				txt_IgnrFld=txt_IgnrFld+"'"+Substr(tmpIgnr_Amend,scnt1,ecnt1)+"',"
				fldcnt1 = fldcnt1 +1
			Endif
		Endfor
		txt_IgnrFld = Substr(txt_IgnrFld,1,Len(txt_IgnrFld)-1)
	Endif
Endif
Endproc

******************************************
*  PRCODURE SRNO	: 10
*  PRCODURE NAME 	: Dis_Flds()
*  PURPOSE			: This procedure used to disabled the fields
*  FIRE      		: This will not fired in any of the udyog standard programe
******************************************
Procedure Dis_Flds
If 'trnamend' $ vChkprod
	_curvouobj = _Screen.ActiveForm
	i=1
	For i=1 To _curvouobj.ControlCount
*!*				If _curvouobj.Controls[i].Name != "SQLCONOBJ"		&& Commented By Shrikant S. on 15/04/2013 for Bug-8068
		If !Inlist(_curvouobj.Controls[i].Name,"SQLCONOBJ","_BATCHSERIALSTK") && Added By Shrikant S. on 15/04/2013 for Bug-8068
			If _curvouobj.Controls[i].Visible=.T.
				If Inlist(_curvouobj.Controls[i].BaseClass,'Commandbutton')
*!*						MESSAGEBOX(_curvouobj.Controls[i].name)
				Endif

				If Inlist(_curvouobj.Controls[i].BaseClass,'Textbox')
*!*						If Empty(tmpfld)
*!*							_curvouobj.Controls[i].Enabled = .T.		&& Enabled Voucherwise fields
					_curvouobj.Controls[i].BackColor = Rgb(255,255,255)
					_curvouobj.Controls[i].ForeColor = Rgb(0,0,0)
*!*						Else
*!*							SrchFld=1
*!*							For SrchFld=1 To fldcnt
*!*								If (Alltrim(Upper(_curvouobj.Controls[i].ControlSource)) = Alltrim(Upper(marray(SrchFld)))) And !Empty(Alltrim(Upper(marray(SrchFld))))
*!*									*!*	MESSAGEBOX(UPPER(_curvouobj.Controls[i].controlsource) +' = '+UPPER(marray(SrchFld)))
*!*	*!*									_curvouobj.Controls[i].Enabled = .T.		&& Enabled Voucherwise fields
*!*									_curvouobj.Controls[i].BackColor = Rgb(255,255,255)
*!*									_curvouobj.Controls[i].ForeColor = Rgb(0,0,0)
*!*								Else
*!*									If (Alltrim(Upper(_curvouobj.Controls[i].ControlSource)) = Alltrim(Upper(marray(SrchFld)))) And _curvouobj.Controls[i].BackColor <> Rgb(245,253,153)
*!*										_curvouobj.Controls[i].Enabled = .F.		&& Disabled Voucherwise fields
*!*									Endif
*!*								Endif
*!*							Endfor
*!*						Endif
					If Inlist(Upper(_curvouobj.Controls[i].ControlSource),Upper("Main_vw.Amend_No"),Upper("Main_vw.Amend_Dt"))
						_curvouobj.Controls[i].Enabled = .F.		&& Disabled Voucherwise fields
					Endif
				Endif
				If _curvouobj.Controls[i].Name = "VouPage"
					If Inlist(_curvouobj.Controls[i].BaseClass,'Textbox')
*!*									_curvouobj.Controls[i].Enabled = .T.		&& Enabled Voucherwise fields
*!*									_curvouobj.Controls[i].backcolor = RGB(245,253,153)
*!*									_curvouobj.Controls[i].forecolor = RGB(0,0,0)
					Endif
					j=1
					For j=1 To _curvouobj.Controls[i].PageCount
						k=1
						For k=1 To _curvouobj.Controls[i].Pages[j].ControlCount
							If Substr(_curvouobj.Controls[i].Pages[j].Controls[k].Name,1,3)="grd"
*!*										MESSAGEBOX(_curvouobj.Controls[i].pages[j].controls[k].columncount,64,_curvouobj.Controls[i].pages[j].controls[k].name)
								l=1
								For l=1 To _curvouobj.Controls[i].Pages[j].Controls[k].ColumnCount
									If _curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].Visible = .T.
*!*												MESSAGEBOX(UPPER(_curvouobj.Controls[i].pages[j].controls[k].columns[l].controlsource),64,_curvouobj.Controls[i].pages[j].name)
										If !Empty(tmpAcpt_Amend)
											SrchFld=1
											For SrchFld=1 To fldcnt
*!*														MESSAGEBOX(STR(l,2,0)+"   => "+_curvouobj.Controls[i].pages[j].controls[k].columns[l].header1.caption,"Grid column header",0.01)
*!*															MESSAGEBOX(UPPER(marray(SrchFld)),64,'rajesh1 '+ALLTRIM(STR(SrchFld,10,2)))

*Birendra : TKT-9757 :Start:
												txtname='txt'+Allt(Str(l)) && Birendra :TKT-9757

												cntname='cnt'+Allt(Str(l)) && Added By Shrikant S. on 26/06/2012 for Bug-4744
*!*													If inlist(_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].CurrentControl, 'Text1',txtname)		&& Commented By Shrikant S. on 26/06/2012 for Bug-4744
												If Inlist(_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].CurrentControl, 'Text1',txtname,cntname)		&& Added By Shrikant S. on 26/06/2012 for Bug-4744


*												If _curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].CurrentControl = 'Text1'
*	tmpraj = Upper(marray(SrchFld))

													Tmpcond=''
&& Added By Shrikant S. on 26/06/2012 for Bug-4744		&& Start
													Tmpcond=Iif(_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].CurrentControl='Text1', ;
														"Upper(_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].text1.ControlSource) = Upper(marray(SrchFld))",;
														IIF(_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].CurrentControl=cntname,;
														"Upper(_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l]."+cntname+".dpk1.uControlSource) = Upper(marray(SrchFld))",;
														"Upper(_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l]."+txtname+".ControlSource) = Upper(marray(SrchFld))"))
&& Added By Shrikant S. on 26/06/2012 for Bug-4744		&& End

&& Commented By Shrikant S. on 26/06/2012 for Bug-4744		&& Start
*!*														tmpcond=IIF(_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].CurrentControl='Text1', ;
*!*																"Upper(_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].text1.ControlSource) = Upper(marray(SrchFld))",;
*!*															"Upper(_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l]."+txtname+".ControlSource) = Upper(marray(SrchFld))")
&& Commented By Shrikant S. on 26/06/2012 for Bug-4744		&& End
													If &Tmpcond

*Birendra : TKT-9757 :End:
*												If _curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].CurrentControl = 'Text1'
*													tmpraj = Upper(marray(SrchFld))
*													If Upper(_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].text1.ControlSource) = Upper(marray(SrchFld))
														If !Empty(Upper(marray(SrchFld)))
*!*																	MESSAGEBOX(UPPER(_curvouobj.Controls[i].pages[j].controls[k].columns[l].text1.controlsource)+"="+UPPER(marray(SrchFld)),64,"CtrSrc = tmpraj")
*!*																_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].Enabled = .T.				&& Enabled Voupage fields
															_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].BackColor = Rgb(255,255,255)
															_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].ForeColor = Rgb(0,0,0)
														Endif
													Else
														If (_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].text1.ControlSource)<>Upper(marray(SrchFld)) And _curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].BackColor <> Rgb(245,253,153)
*!*																MESSAGEBOX(UPPER(_curvouobj.Controls[i].pages[j].controls[k].columns[l].text1.controlsource)+"="+UPPER(marray(SrchFld)),64,"CtrSrc != SrchFld")
															_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].Enabled = .F.				&& Disabled Voupage fields
															_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].ForeColor = Rgb(0,0,0)
														Endif
													Endif
												Else
													_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].Enabled=.F.
												Endif
											Endfor
										Else
*!*												_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].Enabled = .T.				&& Enabled Voupage fields
											_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].BackColor =  Rgb(255,255,255)
											_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].ForeColor = Rgb(0,0,0)
										Endif
									Endif
								Endfor
							Else
								If _curvouobj.Controls[i].Pages[j].Controls[k].Visible = .T.
*!*											MESSAGEBOX(_curvouobj.Controls[i].pages[j].controls[k].name)
									_curvouobj.Controls[i].Pages[j].Controls[k].Enabled = .F.
								Endif
							Endif
						Endfor
					Endfor
				Else
*!*							IF _curvouobj.Controls[i].baseclass = 'Textbox'
*!*								_curvouobj.Controls[i].Enabled = .F.
*!*							ENDIF
				Endif
			Endif
		Endif
	Endfor
*!*		_curvouobj = _Screen.ActiveForm
*!*		i=1
*!*		For i=1 To _curvouobj.ControlCount
*!*			If _curvouobj.Controls[i].Name != "SQLCONOBJ"
*!*				If _curvouobj.Controls[i].Visible=.T.
*!*					If Inlist(_curvouobj.Controls[i].BaseClass,'Textbox')
*!*						If Inlist(Upper(_curvouobj.Controls[i].ControlSource),Upper("Main_vw.Amend_No"),Upper("Main_vw.Amend_Dt"))
*!*							_curvouobj.Controls[i].Enabled = .F.		&& Disabled Voucherwise fields
*!*						Else
*!*							If Empty(txt_AcptFld)
*!*								_curvouobj.Controls[i].BackColor = Rgb(255,255,255)
*!*								_curvouobj.Controls[i].ForeColor = Rgb(0,0,0)
*!*							Else
*!*								_curvouobj.Controls[i].Enabled = .F.		&& Disabled Voucherwise fields
*!*							Endif
*!*						Endif
*!*					Endif
*!*					If _curvouobj.Controls[i].Name = "VouPage"
*!*						If Inlist(_curvouobj.Controls[i].BaseClass,'Textbox')
*!*							_curvouobj.Controls[i].BackColor = Rgb(255,255,255)
*!*							_curvouobj.Controls[i].ForeColor = Rgb(0,0,0)
*!*						Endif
*!*						j=1
*!*						For j=1 To _curvouobj.Controls[i].PageCount
*!*							k=1
*!*							For k=1 To _curvouobj.Controls[i].Pages[j].ControlCount
*!*								If _curvouobj.Controls[i].Pages[j].Controls[k].Name="grd"
*!*									l=1
*!*									For l=1 To _curvouobj.Controls[i].Pages[j].Controls[k].ColumnCount
*!*										If _curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].Visible = .T.
*!*											If !Empty(tmpAcpt_Amend)
*!*												SrchFld=1
*!*												For SrchFld=1 To fldcnt
*!*													If _curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].CurrentControl = 'Text1'
*!*													MESSAGEBOX(_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].CurrentControl)
*!*														If Upper(_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].text1.ControlSource) = Upper(marray(SrchFld))
*!*															_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].BackColor = Rgb(255,255,255)
*!*															_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].ForeColor = Rgb(0,0,0)
*!*														Else
*!*															_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].BackColor = Rgb(255,255,255)
*!*															_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].ForeColor = Rgb(0,0,0)
*!*															_curvouobj.Controls[i].Pages[j].Controls[k].Refresh()
*!*														Endif
*!*													Endif
*!*												Endfor
*!*											Else
*!*												_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].BackColor = Rgb(255,255,255)
*!*												_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].ForeColor = Rgb(0,0,0)
*!*											Endif
*!*										Endif
*!*									Endfor
*!*								Else
*!*									If _curvouobj.Controls[i].Pages[j].Controls[k].Visible = .T.
*!*									Endif
*!*								Endif
*!*							Endfor
*!*						Endfor
*!*					Else
*!*					Endif
*!*				Endif
*!*			Endif
*!*		Endfor
	If Alltrim(lcode_vw.Amendment) = "WARNING" Or Alltrim(lcode_vw.Amendment) = "STRICT"	&& Checking Amendment Acticated or not Activated for Voucher
*!*			tbrdesktop.btnmodi.DisabledPicture = Alltrim(apath)+'\bmp\Amend_text_off.gif'
*!*			tbrdesktop.btnmodi.ToolTipText = "Amendment (Ctrl+E)"
*!*			tbrdesktop.btnmodi.Picture = Alltrim(apath)+'\bmp\Amend_text.gif'
	Else
*!*			tbrdesktop.btnmodi.DisabledPicture = Alltrim(apath)+'\bmp\edit-off.gif'
*!*			tbrdesktop.btnmodi.ToolTipText = "Amendment (Ctrl+E)"
*!*			tbrdesktop.btnmodi.Picture = Alltrim(apath)+'\bmp\edit.gif'
	Endif
Endif
Endproc


******************************************
*  PRCODURE SRNO	: 11
*  PRCODURE NAME 	: Ena_Flds()
*  PURPOSE			: This procedure used to enabled the fields
*  FIRE      		: This will not fired in any of the udyog standard programe
******************************************
Procedure Ena_Flds
If 'trnamend' $ vChkprod
	_curvouobj = _Screen.ActiveForm
	i=1
	For i=1 To _curvouobj.ControlCount
*!*				If _curvouobj.Controls[i].Name != "SQLCONOBJ"		&& Commented By Shrikant S. on 15/04/2013 for Bug-8068
		If !Inlist(_curvouobj.Controls[i].Name,"SQLCONOBJ","_BATCHSERIALSTK") && Added By Shrikant S. on 15/04/2013 for Bug-8068
			If _curvouobj.Controls[i].Visible=.T.
				If Inlist(_curvouobj.Controls[i].BaseClass,'Commandbutton')
*!*						MESSAGEBOX(_curvouobj.Controls[i].name)
				Endif

				If Inlist(_curvouobj.Controls[i].BaseClass,'Textbox')
					If Empty(tmpfld)
						_curvouobj.Controls[i].Enabled = .T.		&& Enabled Voucherwise fields
						_curvouobj.Controls[i].BackColor = Rgb(245,253,153)
						_curvouobj.Controls[i].ForeColor = Rgb(0,0,0)
					Else
						SrchFld=1
						For SrchFld=1 To fldcnt
*Birendra : TKT-9757 :Start:
							zcontsource=''
							If "LMC_VW" $ Alltrim(Upper(_curvouobj.Controls[i].ControlSource))
								zcontsource=Strtran(Alltrim(Upper(_curvouobj.Controls[i].ControlSource)),'LMC_VW','MAIN_VW',1,1)
							Else
								zcontsource=_curvouobj.Controls[i].ControlSource
							Endif

							If (Alltrim(Upper(zcontsource)) == Alltrim(Upper(marray(SrchFld)))) And !Empty(Alltrim(Upper(marray(SrchFld))))
*Birendra : TKT-9757 :End:

*							If (Alltrim(Upper(_curvouobj.Controls[i].ControlSource)) = Alltrim(Upper(marray(SrchFld)))) And !Empty(Alltrim(Upper(marray(SrchFld))))
*!*	MESSAGEBOX(UPPER(_curvouobj.Controls[i].controlsource) +' = '+UPPER(marray(SrchFld)))
								_curvouobj.Controls[i].Enabled = .T.		&& Enabled Voucherwise fields
								_curvouobj.Controls[i].BackColor = Rgb(245,253,153)
								_curvouobj.Controls[i].ForeColor = Rgb(0,0,0)
							Else
								If (Alltrim(Upper(_curvouobj.Controls[i].ControlSource)) <> Alltrim(Upper(marray(SrchFld)))) And _curvouobj.Controls[i].BackColor <> Rgb(245,253,153)
									_curvouobj.Controls[i].Enabled = .F.		&& Disabled Voucherwise fields
								Endif
							Endif
						Endfor
					Endif
					If Inlist(Upper(_curvouobj.Controls[i].ControlSource),Upper("Main_vw.Amend_No"),Upper("Main_vw.Amend_Dt"))
						_curvouobj.Controls[i].Enabled = .F.		&& Disabled Voucherwise fields
					Endif
				Endif
				If _curvouobj.Controls[i].Name = "VouPage"
					If Inlist(_curvouobj.Controls[i].BaseClass,'Textbox')
*!*									_curvouobj.Controls[i].Enabled = .T.		&& Enabled Voucherwise fields
*!*									_curvouobj.Controls[i].backcolor = RGB(245,253,153)
*!*									_curvouobj.Controls[i].forecolor = RGB(0,0,0)
					Endif
					j=1
					For j=1 To _curvouobj.Controls[i].PageCount
						k=1
						For k=1 To _curvouobj.Controls[i].Pages[j].ControlCount
							If Substr(_curvouobj.Controls[i].Pages[j].Controls[k].Name,1,3)="grd"
*!*										MESSAGEBOX(_curvouobj.Controls[i].pages[j].controls[k].columncount,64,_curvouobj.Controls[i].pages[j].controls[k].name)
								l=1
								For l=1 To _curvouobj.Controls[i].Pages[j].Controls[k].ColumnCount
									If _curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].Visible = .T.
*!*												MESSAGEBOX(UPPER(_curvouobj.Controls[i].pages[j].controls[k].columns[l].controlsource),64,_curvouobj.Controls[i].pages[j].name)
										If !Empty(tmpAcpt_Amend)
											SrchFld=1
											For SrchFld=1 To fldcnt
*!*														MESSAGEBOX(STR(l,2,0)+"   => "+_curvouobj.Controls[i].pages[j].controls[k].columns[l].header1.caption,"Grid column header",0.01)
*!*															MESSAGEBOX(UPPER(marray(SrchFld)),64,'rajesh1 '+ALLTRIM(STR(SrchFld,10,2)))

*												If _curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].CurrentControl = 'Text1'
*													tmpraj = Upper(marray(SrchFld))
*													If Upper(_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].text1.ControlSource) = Upper(marray(SrchFld))
*Birendra : TKT-9757 on 04 oct 2011 :Start:
												txtname='txt'+Allt(Str(l)) && Birendra :TKT-9757

												cntname='cnt'+Allt(Str(l)) && Added By Shrikant S. on 26/06/2012 for Bug-4744
*!*													If inlist(_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].CurrentControl, 'Text1') 	&& Commented By Shrikant S. on 26/06/2012 for Bug-4744
												If Inlist(_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].CurrentControl, 'Text1',txtname,cntname) && Added By Shrikant S. on 26/06/2012 for Bug-4744

													Tmpcond=''
&& Added By Shrikant S. on 26/06/2012 for Bug-4744		&& Start
													Tmpcond=Iif(_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].CurrentControl='Text1', ;
														"Upper(_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].text1.ControlSource) = Upper(marray(SrchFld))",;
														IIF(_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].CurrentControl=cntname,;
														"Upper(_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l]."+cntname+".dpk1.uControlSource) = Upper(marray(SrchFld))",;
														"Upper(_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l]."+txtname+".ControlSource) = Upper(marray(SrchFld))"))
&& Added By Shrikant S. on 26/06/2012 for Bug-4744		&& End
&& Commented By Shrikant S. on 26/06/2012 for Bug-4744		&& Start
*!*														tmpcond=IIF(_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].CurrentControl='Text1', ;
*!*																"Upper(_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].text1.ControlSource) = Upper(marray(SrchFld))",;
*!*																"Upper(_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l]."+txtname+".ControlSource) = Upper(marray(SrchFld))")
&& Commented By Shrikant S. on 26/06/2012 for Bug-4744		&& End
													If &Tmpcond
*Birendra : TKT-9757 :End:

														If !Empty(Upper(marray(SrchFld)))
*!*																	MESSAGEBOX(UPPER(_curvouobj.Controls[i].pages[j].controls[k].columns[l].text1.controlsource)+"="+UPPER(marray(SrchFld)),64,"CtrSrc = tmpraj")
															_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].Enabled = .T.				&& Enabled Voupage fields
															_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].BackColor = Rgb(245,253,153)
															_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].ForeColor = Rgb(0,0,0)
														Endif
													Else
														If (_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].text1.ControlSource)<>Upper(marray(SrchFld)) And _curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].BackColor <> Rgb(245,253,153)
*!*																MESSAGEBOX(UPPER(_curvouobj.Controls[i].pages[j].controls[k].columns[l].text1.controlsource)+"="+UPPER(marray(SrchFld)),64,"CtrSrc != SrchFld")
															_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].Enabled = .F.				&& Disabled Voupage fields
															_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].ForeColor = Rgb(0,0,0)
														Endif
													Endif
												Else
*													_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].Enabled=.F.
												Endif
											Endfor
										Else
											_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].Enabled = .T.				&& Enabled Voupage fields
											_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].BackColor =  Rgb(245,253,153)
											_curvouobj.Controls[i].Pages[j].Controls[k].Columns[l].ForeColor = Rgb(0,0,0)
										Endif
									Endif
								Endfor
							Else
								If _curvouobj.Controls[i].Pages[j].Controls[k].Visible = .T.
*!*											MESSAGEBOX(_curvouobj.Controls[i].pages[j].controls[k].name)
									_curvouobj.Controls[i].Pages[j].Controls[k].Enabled = .F.
								Endif
							Endif
						Endfor
					Endfor
				Else
*!*							IF _curvouobj.Controls[i].baseclass = 'Textbox'
*!*								_curvouobj.Controls[i].Enabled = .F.
*!*							ENDIF
				Endif
			Endif
		Endif
	Endfor
Endif
Endproc


******************************************Birendra
*  PROCEDURE SRNO	: 11
*  PROCEDURE NAME 	: VouToolCancel()
*  PURPOSE			: This procedure used to disabled the fields
*  FIRE      		: ToolCancel Button
******************************************
Procedure VouToolCancel
*!*		If _curvouobj.Caption = "Transaction Setting"
*!*			PgFound=.F.
*!*			zmflag=.f.
*!*			i=1
*!*			For i=1 To _curvouobj.ControlCount
*!*				If _curvouobj.Controls[i].BaseClass="Pageframe"
*!*					j=1
*!*					For j=1 To _curvouobj.Controls[i].PageCount
*!*						If _curvouobj.Controls[i].Pages[j].Caption="A\<mendment Details"
*!*							PgFound=.T.
*!*							pgcnt = j
*!*							PgFrm = i
*!*							EXIT && Birendra
*!*						Endif
*!*					Endfor
*!*				Endif
*!*			Endfor
*!*			_curvouobj.Controls[PgFrm].PageCount=pgcnt
*!*			_Amend_Page = _curvouobj.Controls[PgFrm].Pages[pgcnt]

*!*			If PgFound=.T.
*!*	*!*			 WITH _Amend_Page
*!*	*!*				.SetAll('enabled',zmflag,'checkbox')
*!*	*!*				.SetAll('enabled',zmflag,'Combobox')
*!*	*!*				.SetAll('enabled',zmflag,'textbox')
*!*	*!*				.SetAll('enabled',zmflag,'listbox')
*!*	*!*				.SetAll('enabled',zmflag,'EditBox')
*!*	*!*			ENDWITH
*!*				_Amend_Page.oTxt1.Enabled=.F.
*!*				_Amend_Page.oETxt1.Enabled=.F.
*!*			ENDIF
*!*		ENDIF

