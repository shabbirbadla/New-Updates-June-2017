
*!*	*:**********************************************************************************
*!*	*:        Program: STK_VAL.PRG
*!*	*:         System: Udyog Software
*!*	*:         Author: RUPESH PRAJAPATI
*!*	*:  Last modified: 15/09/2006
*!*	*:			AIM  : To chek Nill Balance Printing in Stock List/StocK Ledger Reports
*!*	*:*********************************************************************************
PARAMETERS vDataSessionId 
nilbal= MESSAGEBOX("Items's Having NIL Balance  Should Appear ?",36,vumess)
IF nilbal=6
	REPLACE _rstatusclonesex.xTraParam WITH "'NillBal=YES'"
ELSE
	REPLACE _rstatusclonesex.xTraParam WITH "'NillBal=NO'"
ENDIF




