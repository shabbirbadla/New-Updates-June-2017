Select test
SET STEP ON 
lccompany=Alltrim(test.company)
nm1=Padl("ADMINISTRATOR",Len(test.company),"ADMINISTRATOR")
INS = ' '
chk=0
For J = 1 To Len(lccompany)
	N=Asc(Substr(lccompany,J,1)) - Asc(Substr(nm1,J,1))
	If N<=0
		chk=1
		Loop
	Endif
	INS = INS+Chr(Asc(Substr(lccompany,J,1)) - Asc(Substr(nm1,J,1)))
Endfor
MESSAGEBOX("xx")