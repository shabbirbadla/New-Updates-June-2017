lCcompany="©¸¸Æ¾Á¬r˜Âsz‡ƒu‰‚…u‚| §¨˜—¯b¡Š©u–”z‡ƒu‰‚…u‚| §¨š¥¢_"
SET STEP ON
*If !Empty(_user01.user_roles)
nm1=Padl(Alltr("USER"),Len(Alltrim(LCcompany)),Alltr("USER"))
*Endif
ins = ' '
chk=0
For j = 1 To Len(Alltrim(lccompany))
	N=Asc(Substr(Alltrim(lccompany),j,1)) - Asc(Substr(nm1,j,1))
	If N<=0
		chk=1
		Loop
	Endif
	ins = ins+Chr(Asc(Substr(Alltrim(lccompany),j,1)) - Asc(Substr(nm1,j,1)))
Endfor
WAIT WINDOW 'L'