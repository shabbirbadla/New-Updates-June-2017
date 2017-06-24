*Birendra : 22 mar 2011 for Order Amendment
If 'trnamend' $ vChkprod
	Do VouNew In MainPrg
Endif

**** Added By Sachin N. S. on 12/12/2011 for BUG-921 and 3158 on dt.27/04/2012**** Start
If 'vuexc' $ vChkprod
	If Used('dcmast_vw')
		Select dcmast_vw
		Replace All Code With "A" For Inlist(fld_nm,'CCDAMT','HCDAMT') And Entry_ty='P1'
	Endif
Endif
**** Added By Sachin N. S. on 12/12/2011 for BUG-921 and 3158 on dt.27/04/2012**** End