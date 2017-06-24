If Used('Gen_SrNo_Vw')
	USE IN Gen_SrNo_Vw
Endif	

If (_Screen.ActiveForm.PCVTYPE="BC" OR _Screen.ActiveForm.PCVTYPE="BD")
	replace main_vw.party_nm WITH "BALANCE WITH B17-BOND              " IN main_vw
	.txtpartyname.readonly=.t.
*!*		.txtpartyname.enabled=.f.
	*!*		replace main_vw.party_nm WITH "BALANCE WITH B17-BOND              " IN main_vw
endif	