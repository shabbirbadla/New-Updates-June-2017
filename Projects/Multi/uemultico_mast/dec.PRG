kk = "לךÊÜטלךׂÜללךÞהָלךטÊנ"
mprod=""
FOR i = 0 TO LEN(kk)
	mprod=mprod+chr(asc(substr(kk,i,1))/2)
NEXT
SET STEP ON
MESSAGEBOX(mprod)