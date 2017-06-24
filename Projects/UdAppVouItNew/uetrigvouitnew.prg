&&--->TKT-8320 GTA
If Inlist(.pcVtype,'IF','OF') &&And (([vuexc] $ vchkprod) Or ([vuinv] $ vchkprod))
	replace qty WITH 1 IN item_vw
ENDIF 
&&<---TKT-8320 GTA