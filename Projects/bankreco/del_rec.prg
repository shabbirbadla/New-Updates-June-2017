parameters GRD
set step on
*if upper(alia()) = 'BTEMP'
if GRD=1
	return
endif
select trecostat
if dele()
	recall
else
	dele
endif

_screen.activeform.GRID2.refresh
_screen.activeform.onbal()
set step off
retu
