
IF !FILE('co_mast.dbf')
	MESSAGEBOX("Please run the file from Vudyog folder.")
	RETURN
ENDIF

IF !used('co_mast')
	USE co_mast again shared in 0

	SELECT co_mast
	SCAN for !delete() 	and empty(enddir)
		lcpath= ADDBS(allt(co_mast.dir_nm))
		lcr_status=allt(lcpath)+"r_status"
		lcaddins=allt(lcpath)+"addins"
		IF !used(lcr_status)
			USE &lcr_status again shared in 0 alias r_status

			SELECT r_status
			LOCATE for ALLT(rep_nm)=="FORM2"
			IF found()
				IF !used(lcaddins)
					USE &lcaddins again shared in 0 alias addins
					SELECT addins
					LOCATE for allt(prompt)=="Form 2 Efiling" and !deleted()
					IF !found()
						APPE blan
						REPLACE enabled with .t., prompt with "Form 2 Efiling",action with "DO VU8EFILING.EXE"
						MESSAGEBOX("Menu inserted successfully in addins for company-"+alltr(co_mast.co_name))
					ELSE
						IF allt(action)=="DO EFILING.APP"
							REPLACE enabled with .t., prompt with "Form 2 Efiling",action with "DO VU8EFILING.EXE"
							MESSAGEBOX("Menu inserted successfully in addins for company-"+alltr(co_mast.co_name))
						ELSE 	
							MESSAGEBOX("Menu already exist for company-"+alltr(co_mast.co_name))
						ENDIF	
					ENDIF
				ENDIF
			ENDIF
		ENDIF
		IF used('r_status')
			USE in r_status
		ENDIF
		IF used('addins')
			USE in addins
		ENDIF
	ENDSCAN
ENDIF
Clos all
Clea all
Release all
