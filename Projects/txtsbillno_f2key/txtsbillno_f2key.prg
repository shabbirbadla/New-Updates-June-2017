Parameters oControl

sql_con = 0
oactive=_Screen.ActiveForm
If Inlist(oactive.pcvtype,"GC","GD" )
	lcfld=Justext(oControl.ControlSource)
	If (oactive.addmode Or oactive.editmode) And Upper(Alltrim(lcfld))=="SBILLNO"
		Do Case
		Case oactive.pcvtype="GC"
			msqlstr="Execute USP_GET_PURCHASE_INVOICES"
			lcCaption = 'Select Bill No.'
			lcFieldValue = oControl.Value
			lcField = 'u_pinvno'
			lcFields = 'u_pinvno,u_pinvdt,Code_nm'
			lcExfld = ''
			lcFldsCaption = [u_pinvno:Bill No.,u_pinvdt:Bill Date,Code_nm:Transaction]
			lcFldRtrn =[u_pinvno,u_pinvdt]

		Case oactive.pcvtype="GD"
			msqlstr="Execute USP_GET_SALES_INVOICES"
			lcCaption = 'Select Bill No.'
			lcFieldValue = oControl.Value
			lcField = 'Inv_no'
			lcFields = 'Inv_no,Date,Code_nm'
			lcExfld = ''
			lcFldsCaption = [Inv_no:Sale Bill No.,Date:Sale Bill Date,Code_nm:Transaction]
			lcFldRtrn =[Inv_no,Date]

		Endcase


		sql_con = oactive.sqlconobj.dataconn([EXE],company.dbname,msqlstr,[_lxtrtbl],"oactive.nHandle",oactive.DataSessionId)
		If sql_con > 0 And Used('_lxtrtbl')
			Select _lxtrtbl
			macname = []

			lcGPopVal=Uegetpop('_lxtrtbl',lcCaption,lcField,lcFields,lcFieldValue,'','','',.T.,lcFldRtrn,lcFields,lcFldsCaption)
			If Vartype(lcGPopVal)="O"
				Do Case
				Case oactive.pcvtype="GC"
					oControl.Value = lcGPopVal.u_pinvno
					replace sbillno WITH lcGPopVal.u_pinvno,sbdate WITH lcGPopVal.u_pinvdt IN item_vw
				Case oactive.pcvtype="GD"
					oControl.Value = lcGPopVal.Inv_no
					replace sbillno WITH lcGPopVal.Inv_no,sbdate WITH lcGPopVal.date IN item_vw
				Endcase
			Endif

		Else
			oactive.showmessagebox("No Records Found!",0+32,vumess)
		Endif

		sql_con = oactive.sqlconobj.sqlconnclose("oactive.nHandle")
		If Used('_lxtrtbl')
			Use In _lxtrtbl
		Endif
	Endif
Endif
