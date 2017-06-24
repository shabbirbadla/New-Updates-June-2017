PARAMETERS tnRange
*!*	tcEntry_ty 		: Transaction Type
*!*	tcType 			: Type i.e. [M-Masters/T-Transactions]
*!*	Company.Exppath	: Export File Path

IF VARTYPE(VuMess) <> 'C'
	_SCREEN.VISIBLE = .F.
	=MESSAGEBOX("Internal Files not execute out-side...",16,"Warning")
	QUIT
	RETURN .F.
ENDIF

DO FORM XML_Main WITH tnRange

*!*	DO CASE
*!*	CASE tcType = [M]	&& Masters
*!*	CASE tcType = [T]	&& Transactions
*!*	ENDCASE
