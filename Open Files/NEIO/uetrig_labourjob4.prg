PARAMETERS vDataSessionId 
LOCAL EXPARA
EXPARA=' '
yesno = 0
yesno = MESSAGEBOX ('Do you want to show previous pending bills?',4+32,vumess)

IF yesno = 6
	EXPARA='YES'
ELSE
	EXPARA='NO'
ENDIF

REPLACE _rstatusclonesex.xTraParam WITH "'"+EXPARA+"'"
