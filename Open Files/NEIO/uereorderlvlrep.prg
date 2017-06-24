Parameters vDataSessionId
Local EXPARA
**************Added by Archana K. on 3/12/12 for Bug-6547(Issue in Date Filter in Re-Order Level Report)
EXPARA=' '
yesno = 0
yesno = Messagebox ('Do you want to show Nil quantity to be ordered?',4+32,vumess)

If yesno = 6
	EXPARA=EXPARA+'$'+'ANS=YES'
Else
	EXPARA=EXPARA+'$'+'ANS=NO'
Endif
x=""

Replace _rstatusclonesex.xTraParam With "'"+EXPARA+"'"

