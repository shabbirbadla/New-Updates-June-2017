IF USED('mall_vw')
*!*	WAIT WINDOW "uday reach"
	SELECT mall_vw
	IF tagcount < 1
		INDEX on party_nm TAG party_nm
	ENDIF
ENDIF
