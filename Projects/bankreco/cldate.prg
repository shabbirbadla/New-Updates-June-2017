SELECT lac_det
SELECT sum(amount) opbal from lac_det where entry_ty=[B ] and allt(ac_name)=snam ;
	into cursor bankop
SELECT brtemp
nrec=recno()
cdate=date
cl_amt=bankop.opbal
openamt=bankop.opbal
IF empty(cl_date)
	WAIT window "Clear date " + dtoc(date) + " : " + str(amount) nowait
ENDIF
GO top
SCAN
	IF cl_date>=date and date<(cdate+1)
		cl_amt=cl_amt+iif(amt_ty=[DB],amount,-amount)
	ENDIF
ENDSCAN
*select sum(iif(amt_ty='DB',amount,-amount)) BalAmount from brtemp into cursor bAmtTemp && Prasad 21032005
SELECT sum(iif(amt_ty='DB',amount,-amount)) BalAmount from brtemp where empty(cl_date) into cursor bATemp && Prasad 21032005 to be removed
SELECT brtemp
GO nrec
IF not eof()
	Skip
ENDIF
DispAmount = RecAmount + bATemp.BalAmount  - mclbal

IF eof()
	IF wontop()="clawind"
	ELSE
	*@ 30, 3 say "Bank Statement Amount of "  + allt(snam) + " :" + transform(DispAmount,"@CX")+space(20) && remove later
	deact window clawind
	@ 30, 3 say "Bank Statement Amount of "  + allt(snam) + " :" + transform(DispAmount,"@CX")+space(20) && remove later
	ENDIF
	GO nrec-1 && if eof, the cursor should shift to other record to refresh the table
*	keyboard '{UPARROW}'
ELSE
	deact window clawind
	@ 30, 3 say "Bank Statement Amount of "  + allt(snam) + " :" + transform(DispAmount,"@CX")+space(20) && remove later
ENDIF

RETURN .t.
