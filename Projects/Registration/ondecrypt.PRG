Lpara lcvariable
lcreturn = ""
For i=1 TO LEN(lcvariable)
	lcreturn=lcreturn+chr(asc(substr(lcvariable,i,1))/2)
Endfor
Return lcreturn
