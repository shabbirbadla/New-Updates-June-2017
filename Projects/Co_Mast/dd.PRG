o=''
FOR i=65 TO 122
  IF !BETWEEN(i,90,96)
    o =o+','+CHR(i)
	?CHR(i)
  Endif	
NEXT
MESSAGEBOX(o)
