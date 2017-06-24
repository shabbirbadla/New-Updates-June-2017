PARAMETERS altd,grd
IF grd=2
	RETURN
endif
SELECT tbreco
currdate=tbreco.DATE + altd
repl cl_date with currdate IN tbreco

_screen.ActiveForm.grid1.refresh
return

