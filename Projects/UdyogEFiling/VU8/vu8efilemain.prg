SELECT co_mast
LOCATE FOR co_name = company.co_name AND sta_dt = company.sta_dt AND end_dt = company.end_dt
IF FOUND()
	_vu8efilepara = ''
	_vu8efilepara = _vu8efilepara + '<~1~>' + 'VU8' + '<~1~>'
	_vu8efilepara = _vu8efilepara + '<~2~>' + STRTRAN(apath,' ','<~2A2~>') + '<~2~>'
	_vu8efilepara = _vu8efilepara + '<~3~>' + STRTRAN(ALLTRIM(musername),' ','<~3A3~>') + '<~3~>'
	_vu8efilepara = _vu8efilepara + '<~4~>' + ALLTRIM(STR(RECNO('co_mast'))) + '<~4~>'
	_vu8efilepara = _vu8efilepara + '<~5~>' + STRTRAN(ALLTRIM(vumess),' ','<~5A5~>') + '<~5~>'
	_vu8efilename = ''
	_vu8efilename = ADDBS(apath)+'udyogefiling.exe '+_vu8efilepara

	_SCREEN.WindowState = 1
	! /N &_vu8efilename 
ELSE
	=MESSAGEBOX('Company not found in Company Master',0,vumess)
Endif

*! /N3 udyogefiling.exe 

*Value Application attributes 
*1 Active and normal size 
*2 Active and minimized 
*3 Active and maximized 
*4 Inactive and normal size 
*7 Inactive and minimized 
