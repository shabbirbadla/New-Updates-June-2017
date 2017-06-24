PARA filename,filename2,filename3,relation1,relation2

SET DELETED ON

IF ! USED(filename)
	USE &filename ALIA &filename SHARE IN 0
ENDIF

IF ! USED(filename2)
	USE &filename2 ALIA &filename2 SHARE IN 0
ENDIF

SELECT &filename2
=AFIEL(detail1)

SELECT &filename
SET FILT TO entry_ty='S '

=AFIEL(header1)
CREA CURS field_ac_mast(fldname CHAR(50),TYPE CHAR(15),lenth CHAR(10))
CREA CURS field_det(fldname CHAR(50),TYPE CHAR(15),lenth CHAR(10))

SELECT field_ac_mast
APPE FROM ARRAY header1

SELECT field_det
APPE FROM ARRAY detail1
*index on entry_ty+dtos(date)+doc_no tag edd


SELECT field_ac_mast
GO TOP

SELECT &filename
GO TOP

m.coutfile='SALE_'+DTOS(DATE())+ALLT(STR(RECNO()))+".XML"
m.hFile = FCREATE(m.coutfile)

xml_ver='<?xml version="1.0" ?>'
MAINstart="<"+'SALE'+">"

writefile(m.hFile,xml_ver)
writefile(m.hFile,MAINstart)

SCAN
	hdrstart="<"+'INVOICEHDR'+ALLT(STR(RECNO()))+">"
	writefile(m.hFile,hdrstart)

	SELECT field_ac_mast
	GO TOP
	DO WHILE ! EOF()
		IF m.hFile <= 0
			FatalAlert('INVALID DESTINATION' + m.coutfile, .F.)
		ENDIF
		fldnames="<"+ALLT(field_ac_mast.fldname)+">"
		fldval1="&filename.."+field_ac_mast.fldname

		IF INLI(ALLT(field_ac_mast.TYPE),'C','M')
			fldval=ALLT(STRTRAN(&fldval1,'&','&#38;'))
		ENDIF

		IF INLI(ALLT(field_ac_mast.TYPE),'D')
			fldval=DTOS(&fldval1)
		ENDIF

		IF INLI(ALLT(field_ac_mast.TYPE),'L')
			fldval=STR(IIF((&fldval1)=.F.,0,1))
		ENDIF

		IF INLI(ALLT(field_ac_mast.TYPE),'N')
			fldval=STR(&fldval1)
		ENDIF

		fldnamee="</"+ALLT(field_ac_mast.fldname)+">"

		writefile(m.hFile,fldnames+fldval+fldnamee)
		SELECT field_ac_mast
		IF EOF()
			EXIT
		ENDIF
		SKIP
	ENDDO

	SELECT &filename
	e="&filename"+".Entry_ty"
	d="&filename"+".Date"
	d1="&filename"+".Doc_no"
	edd1=&e+DTOS(&d)+&d1
	SELECT * FROM (filename2) INTO CURS DETAIL WHERE entry_ty+DTOS(DATE)+doc_no=edd1
	SELECT DETAIL
	GO TOP
	SCAN
		hdrstart="<"+'INVOICEDETAIL'+ALLT(STR(RECNO()))+">"
		writefile(m.hFile,hdrstart)

		SELECT field_det
		GO TOP
		DO WHILE ! EOF()
			IF m.hFile <= 0
				FatalAlert('INVALID DESTINATION' + m.coutfile, .F.)
			ENDIF
			fldnames="<"+ALLT(field_det.fldname)+">"
			fldval1="detail."+field_det.fldname

			IF INLI(ALLT(field_det.TYPE),'C','M')
				fldval=ALLT(STRTRAN(&fldval1,'&','&#38;'))
			ENDIF

			IF INLI(ALLT(field_det.TYPE),'D')
				fldval=DTOS(&fldval1)
			ENDIF

			IF INLI(ALLT(field_det.TYPE),'L')
				fldval=STR(IIF((&fldval1)=.F.,0,1))
			ENDIF

			IF INLI(ALLT(field_det.TYPE),'N')
				fldval=STR(&fldval1)
			ENDIF

			fldnamee="</"+ALLT(field_det.fldname)+">"

			writefile(m.hFile,fldnames+fldval+fldnamee)
			SELECT field_det
			IF EOF()
				EXIT
			ENDIF
			SKIP
		ENDDO


		SELECT DETAIL
		hdrend="</"+'INVOICEDETAIL'+ALLT(STR(RECNO()))+">"
		writefile(m.hFile,hdrend)
	ENDSCAN

	SELECT &filename
	hdrend="</"+"INVOICEHDR"+ALLT(STR(RECNO()))+">"
	writefile(m.hFile,hdrend)
ENDSCAN


MAINend="</"+'SALE'+">"
writefile(m.hFile,MAINend)
FCLOSE(m.hFile)
SELECT &filename
USE

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
PROCEDURE writefile
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
CANCEL
RETURN

