LPARAMETERS cOle
SET STEP ON 
LOCAL lRegistered,oTest
IF VARTYPE(cOle) != 'C'
	RETURN .F.
ENDIF

lRegistered = .T.
ON ERROR lRegistered = .F.
oTest = NEWOBJECT(cOle)
ON ERROR
IF VARTYPE(oTest) != 'O'
	lRegistered = .F.
ENDIF
RELEASE oTest
RETURN lRegistered

