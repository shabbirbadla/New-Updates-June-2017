PARAMETERS GRD
IF grd=1
	RETURN
endif

SELECT TRECOSTAT
APPE BLANK
repl ac_name with snam
repl dept with sdept
repl cate with scat

_screen.ActiveForm.GRID2.refresh
RETU