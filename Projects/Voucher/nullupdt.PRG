&&Changes done by Vasant on 31/12/2011 as per Bug 1348 - RG Page No. should generate from start (i.e.) for each financial year
LPARAMETERS _NullUpdtTblNm
IF USED(_NullUpdtTblNm)
	SELECT (_NullUpdtTblNm)
	SCATTER MEMVAR Blank Memo
	Scan
		FOR i1 = 1 TO fcount(_NullUpdtTblNm)
			i3 = FIELD(i1,_NullUpdtTblNm)
			i4 = 'm.'+i3
			IF ISNULL(&i3)
				REPLACE &i3 WITH &i4 IN (_NullUpdtTblNm)
			Endif
		Endfor
	ENDSCAN
	GO Top
Endif	
&&Changes done by Vasant on 31/12/2011 as per Bug 1348 - RG Page No. should generate from start (i.e.) for each financial year