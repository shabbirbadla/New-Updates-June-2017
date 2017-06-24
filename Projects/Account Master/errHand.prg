para  e_error, e_line,  e_prog,e_method,e_message,e_mline
	cMsg="Error Number: " + alltrim(str(e_error))+ chr(13)+ ;
	 "Line Number: "+alltrim(str(e_line)) + chr(13) + e_message+ chr(13)+ ;
	 "Text: "+e_mline +chr(13)+ ;
	 "Program: "+e_prog + "." + e_method
	nAnswer = messagebox(cMsg, 2+48+512, "Error")
	do case
		case nAnswer = 3	&&Abort
			cancel
		case nAnswer = 4	&&Retry
			retry
		otherwise			&&Ignore
			return
	endcase
return
