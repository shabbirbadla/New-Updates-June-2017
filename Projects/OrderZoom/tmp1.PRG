SET EXACT OFF
CLEA
mpickfrom = []
mpickfromv = [SO/PO/IP/JV/SO/PT/IP/JV/SO/PO/JV/PT/ST]

DO WHILE !EMPTY(mpickfromv)
	IF BETW(ASC(LEFT(mpickfromv,1)),65,89)
		IF LEFT(mpickfromv,2) $ mpickfrom = .f.
			mpickfrom = mpickfrom + IIF(EMPTY(mpickfrom),'"'+LEFT(mpickfromv,2)+'"',',"'+LEFT(mpickfromv,2)+'"')
		ENDIF
		mpickfromv = SUBSTR(mpickfromv,3)
	ELSE
		mpickfromv = SUBSTR(mpickfromv,2)
	ENDIF
ENDDO

?mpickfrom
