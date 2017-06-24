para filename,filename2,filename3,relation1,relation2


if ! used(filename)
	use &filename alia &filename share in 0
endif
select &filename

=afiel(a)
crea curs field_ac_mast(fldname char(50),type char(15),lenth char(10))

select field_ac_mast
appe from array a
select field_ac_mast
go top

select &filename
scan
	m.coutfile=Alias()+dtos(date())+allt(str(recno()))+".XML"
	m.hFile = FCREATE(m.cOutFile)
	
	xml_ver='<?xml version="1.0" ?>'
	hdrstart="<"+allt(alia())+">"
	writefile(m.hfile,xml_ver)	
	writefile(m.hfile,hdrstart)
	
	select field_ac_mast
	go top
		do while ! eof()
			IF m.hFile <= 0
				FatalAlert(INVALID_DESTINATION_LOC + m.cOutFile, .F.)
			ENDIF
				fldnames="<"+allt(field_ac_mast.fldname)+">"
				fldval1="&filename.."+field_ac_mast.fldname
		
				if inli(allt(field_ac_mast.type),'C','M')
					fldval=allt(&fldval1)
				endif
			
				if inli(allt(field_ac_mast.type),'D')
					fldval=dtos(&fldval1)
				endif
	
				if inli(allt(field_ac_mast.type),'L')
					fldval=str(iif((&fldval1)=.F.,0,1))
				endif

				if inli(allt(field_ac_mast.type),'N')
					fldval=str(&fldval1)
				endif
					
				fldnamee="</"+allt(field_ac_mast.fldname)+">"
		
				writefile(m.hfile,fldnames+fldval+fldnamee)
				select field_ac_mast
				if eof()
					exit
				endif
			skip
		enddo
	select &filename		
	hdrend="</"+allt(alia())+">"
	writefile(m.hfile,hdrend)
	FCLOSE(m.hFile)

endscan

select &filename
use

*!*	para coutfile

*!*	m.hFile = FCREATE(m.cOutFile)
*!*	IF m.hFile <= 0
*!*		FatalAlert(INVALID_DESTINATION_LOC + m.cOutFile, .F.)
*!*	ENDIF
*!*	writefile(m.hfile,"AMAR YADAV ABCL")

*!*	FCLOSE(m.hFile)



**************************************************************************
**
** Function Name: WRITEFILE(<ExpN>, <ExpC>)
** Creation Date: 1994.12.02
** Purpose        :
**
**              Centralized file output routine to check for proper output
**
** Parameters:
**
**      hFileHandle - Handle of output file
**      cText       - Contents to write to file
**
**************************************************************************
procedure writefile
LPARAMETERS hFileHandle, cText
	m.nBytesSent = FPUTS(m.hFileHandle, m.cText)
	IF  m.nBytesSent < LEN(m.cText)
		FatalAlert('asdasd', .T.)
	ENDIF
RETURN
**************************************************************************
PROCEDURE FatalAlert
	LPARAMETERS cAlert_Message, lCleanup

	MESSAGEBOX(m.cAlert_Message, 16)

*	MESSAGEBOX(m.cAlert_Message, 16, ERROR_TITLE_LOC)

	CANCEL
RETURN

