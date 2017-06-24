&& 17/02/2014 sandeep/shrikant for bug-21254 on 17-02-2014		&& Start
_oForm = _Screen.ActiveForm
If ([vuexc] $ vchkprod)
	If Uppe(Allt(wTable))=Upper([Main_Vw])
		Select lother
		_nrecno=Iif(!Eof(),Recno(),0)
		Locate For Upper(Alltrim(fld_nm))='U_ARREARS'
		If Found()
*!*			lcfiltcond=ALLTRIM(lother.filtcond)
*!*			MESSAGEBOX(&lcfiltcond)
*!*				Replace lother.FILTCOND With &lcfiltcond In lother	
			
			If Alltrim(MAIN_VW.PARTY_NM)=="BALANCE WITH EXCISE MISC PLA" AND inlist(main_vw.entry_ty ,'RR','RP','OB','GI','GR','HR','HR','HI')
				Replace lother.FILTCOND With ',Misc. payments - Fine,Misc. payments - Penalty,Misc. payments - Others' In lother
			Else
&&				Replace lother.FILTCOND With ',ARREARS OF DUTY UNDER RULE 8,ARREARS OF DUTY UNDER Section 11A,ARREARS OF DUTY UNDER Section 11D,OTHERS ARREARS OF DUTY,INTEREST PAYMENT UNDER RULE 8,INTEREST PAYMENT UNDER Section 11A,INTEREST PAYMENT UNDER Section 11D,OTHERS INTEREST PAYMENTS' In lother
				Replace lother.FILTCOND With ',ARREARS OF DUTY UNDER RULE 8' additive
				Replace lother.FILTCOND With ',ARREARS OF DUTY UNDER Section 11A,ARREARS OF DUTY UNDER Section 11A2[B],ARREARS OF DUTY UNDER Section 11D,OTHERS ARREARS OF DUTY,INTEREST PAYMENT UNDER RULE 8,INTEREST PAYMENT UNDER Section 11A2[B],INTEREST PAYMENT UNDER Section 11A' In lother
			Endif

		Endif

		Select lother
		If _nrecno!=0
			Go _nrecno
		Endif
	Endif
Endif
&& 17/02/2014 sandeep/shrikant for bug-21254 on 17-02-2014		&& End



