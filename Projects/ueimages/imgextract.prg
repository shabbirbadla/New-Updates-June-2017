*:*****************************************************************************
*:        Program: Imgextract.PRG
*:         System: Udyog Software
*:         Author: RND
*:  Last modified: 05/08/2009
*:			AIM  : It extract the updated file.
*:*****************************************************************************
LPARAMETERS tnStore AS INTEGER,tcFilenm AS STRING,tcPath AS STRING
*!*	tnStore		 :	(1) Update Image Database [For Internal use only] i.e. Application build time.
*!*					(2) File Extract at runtime method
*!*	tcFilenm	 :	Extract file name
*!*	tcPath	 	 :	Location for extract file.
*!*	Usage		 :	Ueimages(1,"E:\Raghu\Vfp9\ueimages\Demo_ITax.Jpg","")
*!*					Ueimages(2,"DEMO_ITAX.JPG","E:\Raghu\VFP9\ueimages\")
*:*****************************************************************************

*!*	*!*	If Vartype(VuMess) <> [C]
*!*	*!*		_Screen.Visible = .F.
*!*	*!*		Messagebox("Internal Application Are Not Execute Out-Side ...",16)
*!*	*!*		Quit
*!*	*!*	Endif

*!*	****Versioning**** Added By Amrendra On 30/06/2011
*!*		LOCAL _VerValidErr,_VerRetVal,_CurrVerVal
*!*		_VerValidErr = ""
*!*		_VerRetVal  = 'NO'
*!*		TRY
*!*			_VerRetVal = AppVerChk('IMAGES',GetFileVersion(),JUSTFNAME(SYS(16)))
*!*		CATCH TO _VerValidErr
*!*			_VerRetVal  = 'NO'
*!*		Endtry	
*!*		IF TYPE("_VerRetVal")="L"
*!*			cMsgStr="Version Error occured!"
*!*			cMsgStr=cMsgStr+CHR(13)+"Kindly update latest version of "+GlobalObj.getPropertyval("ProductTitle")
*!*			Messagebox(cMsgStr,64,VuMess)
*!*			Return .F.
*!*		ENDIF
*!*		IF _VerRetVal  = 'NO' AND tnStore != 1
*!*			Return .F.
*!*		Endif
*!*	****Versioning****

tnStore = IIF(VARTYPE(tnStore) <> "N",1,tnStore)

IF !USED("Images")
	SELECT 0
	USE images
ENDIF

SELECT images
LOCAL liBlob,lcFilenm,llUpdate
DO CASE
CASE tnStore = 1					  && Update Images
	IF EMPTY(tcFilenm)
		tcFilenm = GETPICT()
	ENDIF
	gnFiles = ADIR(gaFiles,tcFilenm)  && Create array
	IF gnFiles <> 0
		liBlob = FILETOSTR(tcFilenm)
		lcFilenm = JUSTFNAME(tcFilenm)
		SELECT images
		IF !SEEK(UPPER(ALLTRIM(lcFilenm)),"Images","CFilenm")
			INSERT INTO images (bImage,cFilenm,nsize) VALUES (liBlob,lcFilenm,gaFiles(1,2))
		ELSE
			lnId = images.nId
			UPDATE images SET bImage = liBlob,nsize = gaFiles(1,2) WHERE nId = lnId
		ENDIF
	ENDIF
CASE tnStore = 2
	IF EMPTY(tcFilenm) OR EMPTY(tcPath)
		RETURN .F.
	ENDIF
	IF !DIRECTORY(tcPath)
		RETURN .F.
	ENDIF
	tcPath = ALLT(FULLPATH(tcPath))
	tcPath = tcPath+IIF(RIGHT(tcPath,1)<>"\","\","")
	SELECT images
	IF SEEK(UPPER(ALLTRIM(tcFilenm)),"Images","CFilenm")
		gnFiles = ADIR(gaFiles,tcPath+ALLTRIM(images.cFilenm))  && Create array
		IF gnFiles = 0
			llUpdate = .T.
		ENDIF
		IF !llUpdate
			IF (images.nsize <> gaFiles(1,2))
				=CreateFile(tcPath)
			ENDIF
		ELSE
			=CreateFile(tcPath)
		ENDIF
	ENDIF
ENDCASE
RETURN UPPER(ALLTRIM(PROGRAM()))

FUNCTION CreateFile
*******************
LPARAMETERS tcPath AS STRING
liBlob = images.bImage
lcFilenm = FULLPATH(tcPath)+PROPER(ALLTRIM(images.cFilenm))
IF FILE(lcFilenm)
	DELETE FILE (lcFilenm)
ENDIF
STRTOFILE(liBlob, lcFilenm)


*>>>***Versioning**** Added By Amrendra On 08/07/2011
FUNCTION GetFileVersion
PARAMETERS lcTable
	_CurrVerVal='10.0.0.0' &&[VERSIONNUMBER]
	IF !EMPTY(lcTable)
		SELECT(lcTable)
		APPEND BLANK 
		replace fVersion WITH JUSTFNAME(SYS(16))+'   '+_CurrVerVal
	ENDIF 
RETURN _CurrVerVal
*<<<***Versioning**** Added By Amrendra On 08/07/2011