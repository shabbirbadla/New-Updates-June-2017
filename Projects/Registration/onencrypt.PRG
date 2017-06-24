Lpara lcvariable
lcreturn = ""
For i=1 TO LEN(lcvariable)
	lcreturn=lcreturn+CHR(ASC(SUBSTR(lcvariable,i,1))+ASC(SUBSTR(lcvariable,i,1)))
Endfor
Return lcreturn
