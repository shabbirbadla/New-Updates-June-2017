vDataSessionId=_screen.ActiveForm.DataSessionId
Set DataSession To vDataSessionId
*REPLACE xTraParam WITH  vchkprod IN _rstatusclonesex
REPLACE xTraParam WITH IIF("VUISD"$UPPER(vchkprod),"VUISD","VUSER" ) &&Rup TKT-8171
