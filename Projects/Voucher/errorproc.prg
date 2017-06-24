******************************************************************************
* FUNCTION ERROR(cError) - Returns Error No. and Messages .
* This function provides the continues processing of the program w/o exiting 
* a particular module during execution. this routine must be place on the
* ErrorEvent of a particular form(s).
* RETURN - ERROR(),MESSAGE()
* SYNTAX - ERRORPROC()
****************************************************************************
nError  = ERROR()
cMethod = MESSAGE()
nLineNo = LineNo()
mPrg    = Program()
IF nerror # 0 
	nAnsWer = MESSAGEBOX('Warning No.: '+ALLTRIM(STR(nError))+CHR(13)+"Warning Message: "+cMethod,64,'Warning Messages'+;
	'Program Name '+mPrg+' Line No '+str(nLineNo))
ENDIF
RETURN







