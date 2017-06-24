Procedure errhandler
*!*		para  e_error, e_line, e_prog,e_method,e_message,e_mline, aa
*!*		If aa==[ZIP]
*!*			insert into vu_error(ERR_NUM,ERR_LINE,ERR_PROG,ERR_METH,ERR_MESS,ERR_MLINE);
*!*				values(e_error, e_line, e_prog,e_method,e_message,e_mline)
*!*		EndIf
*!*		cMsg="Error:" + alltrim(str(e_error)) + chr(13) + e_message +chr(13)+"Program:"+e_prog
*!*		nAnswer = messagebox(cMsg, 2+48+512, "Error")
*!*		do case
*!*			case nAnswer = 3	&&Abort
*!*				cancel
*!*			case nAnswer = 4	&&Retry
*!*				retry
*!*			otherwise			&&Ignore
*!*				return
*!*		endcase
*!*		return
