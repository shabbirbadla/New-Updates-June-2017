PARAMETERS mfile,memt

PRIVATE mhandle,FSIZE

IF FILE(mfile)					&& Does file exist?
	mhandle = FOPEN(mfile,12)	&& If so, open read/write
ELSE
	mhandle = FCREATE(mfile)	&& If not create it
ENDIF

IF mhandle < 0
	DO CASE						&& Unable to open the temporary file.
	CASE FERROR( ) = 4
		reason = 'Too many files open (out of handles)'
		=MESSAGEBOX(reason)
		RETURN .F.
	CASE FERROR( ) = 5
		reason = 'Out of memory'
		=MESSAGEBOX(reason)
		RETURN .F.
	CASE FERROR( ) = 29
		reason = 'Disk full'
		=MESSAGEBOX(reason)
		RETURN .F.
	CASE FERROR( ) = 31
		reason = 'General Failure'
		=MESSAGEBOX(reason)
		RETURN .F.
	ENDCASE
ENDIF
FSIZE = FSEEK(mhandle,0,2)

IF !EMPTY(memt)
	=FPUTS(mhandle,ALLT(memt))						&& Table Name
ENDIF

=FCLOSE(mhandle)  && Close the file
