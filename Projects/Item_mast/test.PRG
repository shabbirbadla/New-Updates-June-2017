constr = 'DRIVER=Microsoft Visual FoxPro Driver;UID=;Deleted=Yes;Null=Yes;Collate=Machine;BackgroundFetch=Yes;Exclusive=No;SourceType=DBC;SourceDB=c:\vudyog_tushar\multi\invoice.dbc'
Chqcon = Sqlstringconnect(constr)
ff=SQLSETPROP(chqcon,'transactions',2)
if ff > 0 
	dimen mflds(1)
	local runflds
	runflds=""
	mvalues=""
	if chqcon > 0
		ll = Sqlexec(chqcon,"select top 1 * from it_mast order by it_name desc","_itmast")
		=sqldisconnecT(chqcon)
		sele _itmast
		repl it_alias with 'tushar'
		local am
		am = _itmast.it_name
		totflds=afields(mflds)
 	 	totflds=totflds-1
		chqcon=sqlstringconnecT(constr)
		sele _itmast
		for i = 1 to totflds
			if mflds(i,1)="GROUP"
*				 runflds = runflds+"["+mflds(i,1)+"]"+"="+"?"+field(i)+ iif(totflds=1 or i=totflds,"",",")
				 runflds = runflds+"it_mast."+mflds(i,1)+"="+"?"+"_itmast."+field(i)+ iif(totflds=1 or i=totflds,"",",")
			else
				runflds = runflds+mflds(i,1)+"="+"?"+field(i)+ iif(totflds=1 or i=totflds,"",",")
			endif
		next
		set step on
 		hh = sqlexec(CHQCON,"update it_mast set "+runflds+" where it_name=?am")
		=sqlcommit(chqcon)
		=sqldisconnect(chqcon)
		rele chqcon
	Endif
endif