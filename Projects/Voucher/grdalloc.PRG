PARAMETERS _allobj,_allcond,_allep,_alldp
IF TYPE(_allobj) = 'O'
	IF &_allcond
		IF SUBSTR(UPPER(&_allobj..Picture),RAT('\',UPPER(&_allobj..Picture))) != SUBSTR(UPPER(&_allep),RAT('\',UPPER(&_allep)))		&&vasant060609
			&_allobj..Picture = &_allep
		Endif	&&vasant060609
	ELSE
		IF SUBSTR(UPPER(&_allobj..Picture),RAT('\',UPPER(&_allobj..Picture))) != SUBSTR(UPPER(&_alldp),RAT('\',UPPER(&_alldp)))		&&vasant060609
			&_allobj..Picture = &_alldp
		Endif	&&vasant060609
	endif	
Endif	
RETURN .f.