IF (_SCREEN.ACTIVEFORM.PCVTYPE="DM")

	IF EMPTY(main_vw.u_grosswt)
		MESSAGEBOX("Plz Enter Gross weight First!!!")
		RETURN .f.
	ENDIF
	IF EMPTY(main_vw.U_NETWT)
		MESSAGEBOX("Plz Enter Net weight First!!!")
		RETURN .f.
	ENDIF
	IF EMPTY(main_vw.U_COUNTAIN)
		MESSAGEBOX("Plz Enter Container No. weight First!!!")
		RETURN .f.
	ENDIF
	

ENDIF