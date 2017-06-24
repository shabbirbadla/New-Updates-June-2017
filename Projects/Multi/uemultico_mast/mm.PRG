oval = "!ddd$"
Const="'~','!','@','#','$','%','^','&','*','(',')','-','=','+','\','/','[',']','{','}',' ',':',';','<','>',',','.'"
&&26
SET STEP ON 
For i = 32 To 126
	If Chr(i) $ oVal
		Return .F.
	Endif
Next
Return .T.
