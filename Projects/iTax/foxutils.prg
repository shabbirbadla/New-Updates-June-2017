DEFINE CLASS FoxUtils AS Custom
	*|-- Name of the class. -
	Name = "FoxUtils"
	*|-- Table for encode. -
	DIMENSION m_base64Table[64]
	m_base64List = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/"

	*|-- Path where is the FoxUtils.fll bookstore, 
	*|-- by defect is the same directory where this the FoxUtil.prg class
	PROTECTED m_LibPath
	m_LibPath = ""
	PROCEDURE Init
		LOCAL i, lnLen, lsLibFile
		*|-- Directory where this the FoxUtils.FLL. 
		This.m_LibPath = ADDBS( JUSTPATH( SYS(16) ) )
		This.m_LibPath = ALLTRIM( SUBSTR( This.m_LibPath, AT( "FOXUTILS.INIT", This.m_LibPath ) + 13, LEN(This.m_LibPath) - 13 ) )
		*|-- Full the Array of encode the value of m_base64List. -
		lnLen = LEN( This.m_base64List )
		DIMENSION This.m_base64Table[ lnLen ]
		FOR i = 1 TO lnLen
			This.m_base64Table[i] = SUBSTR( This.m_base64List, i, 1 )
		NEXT
		IF( !EMPTY( This.m_LibPath ) )
			lsLibFile = ADDBS(This.m_LibPath) + "FoxUtils.fll"
		ELSE
			lsLibFile = "FoxUtils.fll"
		ENDIF
		IF( FILE(lsLibFile) )
			SET LIBRARY TO &lsLibFile
		ENDIF
		*|--------------------------------------------------------------------------------------------------
		*|-- Functions for the GUID
		*|--------------------------------------------------------------------------------------------------
		DECLARE INTEGER CoCreateGuid IN "ole32" 		;
			STRING @pGuid
		DECLARE INTEGER StringFromGUID2 IN "ole32" 		;
			STRING rGuid, STRING @lpsz, INTEGER cchMax

		DECLARE INTEGER UuidCreate IN "RPCRT4.DLL" 		;
			STRING @ UUID

		*|--------------------------------------------------------------------------------------------------
		*|-- Functions for the access to the Registry. -
		*|--------------------------------------------------------------------------------------------------
		DECLARE INTEGER RegOpenKey IN "Win32API" 					;
		   INTEGER nHKey, STRIN @cSubKey, INTEGER @nResult
		DECLARE INTEGER RegCloseKey IN "Win32API" 					;
			INTEGER nHKey
		DECLARE INTEGER RegQueryValueEx IN "Win32API" 				;
		   INTEGER nHKey, STRING lpszValueName, INTEGER dwReserved,	;
		   INTEGER @lpdwType, STRIN @lpbData, INTEGER @lpcbData

		DECLARE INTEGER RegEnumKeyEx IN WIN32API 					;
		  INTEGER nKey,			INTEGER nIndex, 					;
		  STRING  @cName,		INTEGER @nSizeName, 				;
		  INTEGER nReserved,	STRING  @cClass, 					;
		  INTEGER @nSizeClass,	STRING  @cLastWriteTime

		*|--------------------------------------------------------------------------------------------------
		*|-- Constants for the access to the Registry. -
		*|--------------------------------------------------------------------------------------------------
		#DEFINE HKEY_CLASSES_ROOT 	0x80000000
		#DEFINE HKEY_CURRENT_USER 	0x80000001

		#DEFINE REG_SZ 				0x00000001	&& Cadena Unicode terminada en valor nulo

		
	ENDPROC
	
	*|----------------------------------------------------------------------------------------------------------
	*|----------------------------------------- Funciones para MIME ------------------------------------------
	*|----------------------------------------------------------------------------------------------------------
	PROCEDURE ObtenerMimeType( lsFileName )
		LOCAL lsReturn, lsFileExt
		lsReturn  = "text/plain;charset=iso-8859-1"
		lsFileExt = JUSTEXT( lsFileName )

		IF( !This.ObtenerHardCodeMime( lsFileExt, @lsReturn ) )
			This.ObtenerMimeDinamico( lsFileExt, @lsReturn )
		ENDIF
		
		lsReturn = lsReturn + ";"
		RETURN lsReturn
	ENDPROC
	
	PROTECTED PROCEDURE ObtenerMimeDinamico( lsFileExt, lsMimeType )
		LOCAL lnErrCode, lsKeyPath, lnSubBranch
		LOCAL lnKeyEntry, lsNewKey, lnKeySize, lsBuf, lnBufLen, lsRetTime
		LOCAL lnSubKey, lnType, lsData, lnDataLen
		LOCAL lbReturn, lbExit
		
		lsFileExt   = "." + LOWER( lsFileExt )
		lsKeyPath   = "MIME\Database\Content Type"
		lnSubBranch = 0
		lbReturn    = .T.
		lbExit      = .F.
		
		lnErrCode = RegOpenKey( HKEY_CLASSES_ROOT, lsKeyPath, @lnSubBranch )

		IF( lnErrCode = 0 )
			lnKeyEntry = 0
			
			*|-- I cross all the branches of the carpera "Content Type". -
			DO WHILE .T.
				lnKeySize = 100
				lnBufLen  = 100
				lsNewKey  = SPACE( lnKeySize )
				lsBuf     = SPACE( lnBufLen )
				lsRetTime = SPACE( 100 )
				
				lnErrCode = RegEnumKeyEx( lnSubBranch, lnKeyEntry, @lsNewKey, @lnKeySize, 0, @lsBuf, @lnBufLen, @lsRetTime )

				IF( lnErrCode = 259 OR lnErrCode != 0 )
					lbReturn = .F.
					EXIT	&&It leaves. -
				ENDIF
				
				*|--Acquittal the information that this of more. -
				lsNewKey = ALLTRIM( lsNewKey )
				lsNewKey = LEFT( lsNewKey, LEN( lsNewKey ) - 1 )
				
				*|-- Now which I have the name of the branch of the carpera "Content Type" I open for looks for it the extension.
				lnErrCode = RegOpenKey( HKEY_CLASSES_ROOT, lsKeyPath + "\" + lsNewKey, @lnSubKey )
				IF( lnErrCode = 0 )
					lnType    = 0
					lnDataLen = 256
					lsData    = SPACE( lnDataLen )
					
					RegQueryValueEx( lnSubKey, "Extension", 0, @lnType, @lsData, @lnDataLen )

					IF( lnType = REG_SZ )
						lsData = LEFT( lsData, lnDataLen -1 )

						IF( lsData == lsFileExt )
							lsMimeType = lsNewKey
							lbReturn   = .T.
							lbExit     = .T.
						ENDIF
					ENDIF

					*|-- It closes SubRama. -
					RegCloseKey( lnSubKey )
					
					*|-- It leaves the cycle. -
					IF( lbExit == .T. )
						EXIT
					ENDIF
				ELSE
					lbReturn = .F.
					EXIT	&&It leaves. -
				ENDIF
				
				lnKeyEntry = lnKeyEntry + 1
			ENDDO

			*|-- Cierra la rama principal.-
			RegCloseKey( lnSubBranch )
		ELSE
			lbReturn = .F.
		ENDIF
		
		RETURN lbReturn
	ENDPROC
	
	PROTECTED PROCEDURE ObtenerHardCodeMime( lsFileExt, lsMimeType )
		LOCAL lsReturn

		lsReturn  = .T.
		lsFileExt = LOWER( lsFileExt )
		DO CASE
			CASE lsFileExt = "xls"
				lsMimeType = "application/x-msexcel"
			CASE lsFileExt = "doc"
				lsMimeType = "application/msword"
			CASE lsFileExt = "rtf"
				lsMimeType = "text/richtext"
			CASE lsFileExt = "htm"
				lsMimeType = "text/html"
			CASE lsFileExt = "aiff"
				lsMimeType = "audio/x-aiff"
			CASE lsFileExt = "au"
				lsMimeType = "audio/basic"
			CASE lsFileExt = "wav"
				lsMimeType = "audio/wav"
			CASE lsFileExt = "gif"
				lsMimeType = "image/gif"
			CASE lsFileExt = "jpg"
				lsMimeType = "image/jpeg"
			CASE lsFileExt = "pjpg"
				lsMimeType = "image/pjpeg"
			CASE lsFileExt = "tif"
				lsMimeType = "image/tiff"
			CASE lsFileExt = "png"
				lsMimeType = "image/x-png"
			CASE lsFileExt = "xbm"
				lsMimeType = "image/x-xbitmap"
			CASE lsFileExt = "bmp"
				lsMimeType = "image/bmp"
			CASE lsFileExt = "avi"
				lsMimeType = "video/avi"
			CASE lsFileExt = "mpeg"
				lsMimeType = "video/mpeg"
			CASE lsFileExt = "ps"
				lsMimeType = "application/postscript"
			CASE lsFileExt = "hqx"
				lsMimeType = "application/macbinhex40"
			CASE lsFileExt = "pdf"
				lsMimeType = "application/pdf"
			CASE lsFileExt = "tgz"
				lsMimeType = "application/x-compressed"
			CASE lsFileExt = "zip"
				lsMimeType = "application/x-zip-compressed"
			CASE lsFileExt = "gz"
				lsMimeType = "application/x-gzip-compressed"
			OTHERWISE
				lsReturn = .F.
		ENDCASE
		
		RETURN lsReturn
	ENDPROC
	
	*|----------------------------------------------------------------------------------------------------------
	*|----------------------------------------- Functions para Base64 ------------------------------------------
	*|----------------------------------------------------------------------------------------------------------
	PROCEDURE Encode64( lsIn, lsOut )
		LOCAL inlen, lsOut, lnInRel, oval

		inlen   = LEN( lsIn )
		olen    = (inlen + 2) / 3 * 4
		outmax  = inlen * 2
		lsOut   = ""
		lnInRel = 1
		
		IF( outmax < olen ) 
			RETURN .F.
		ENDIF

		DO WHILE( inlen >= 3 )
			lsOut = lsOut + This.m_base64Table[ (BITRSHIFT( ASC(SUBSTR( lsIn, lnInRel, 1 )), 2 )) + 1 ]
			lsOut = lsOut + This.m_base64Table[ (BITOR( BITAND( (BITLSHIFT( ASC(SUBSTR( lsIn, lnInRel, 1 )), 4 )), 0x30 ), BITRSHIFT( ASC(SUBSTR( lsIn, lnInRel + 1, 1 )), 4 ) )) + 1]
			lsOut = lsOut + This.m_base64Table[ (BITOR( BITAND( (BITLSHIFT( ASC(SUBSTR( lsIn, lnInRel + 1, 1 )), 2 )), 0x3C ), BITRSHIFT( ASC(SUBSTR( lsIn, lnInRel + 2, 1 )), 6 ) )) + 1]
			lsOut = lsOut + This.m_base64Table[ (BITAND( ASC(SUBSTR( lsIn, lnInRel + 2, 1 )), 0x3F )) + 1 ]

			lnInRel = lnInRel + 3
			inlen   = inlen - 3

		ENDDO

		IF( inlen > 0 )
			lsOut = lsOut + This.m_base64Table[ (BITRSHIFT( ASC(SUBSTR( lsIn, lnInRel, 1 )), 2 )) + 1 ]
			oval = BITAND( BITLSHIFT( ASC(SUBSTR( lsIn, lnInRel, 1 )), 4 ), 0x30 )

			IF( inlen > 1 )
				oval = BITOR( BITRSHIFT( ASC(SUBSTR( lsIn, lnInRel + 1, 1 )), 4 ), oval )
			ENDIF

			lsOut = lsOut + This.m_base64Table[ oval + 1 ]
			lsOut = lsOut + IIF( (inlen < 2), '=', This.m_base64Table[ (BITAND( BITLSHIFT( ASC(SUBSTR( lsIn, lnInRel + 1, 1 )), 2 ), 0x3C )) + 1 ] )
			lsOut = lsOut + '='			
		ENDIF

		RETURN .T.
	ENDPROC
	
	PROCEDURE FileEncode64( sFileName, lsEncodeFile, lnLargoCadenaAdjunto )
		lsEncodeFile = FileEncode64( sFileName, lnLargoCadenaAdjunto )
		RETURN ( lsEncodeFile != "-" )
	ENDPROC

	*|----------------------------------------------------------------------------------------------------------
	*|---------------------------------------------- GUID ------------------------------------------------------
	*|----------------------------------------------------------------------------------------------------------
	*|-- Ref.: http://www.opengroup.org/dce/info/draft-leach-uuids-guids-01.txt --------------------------------
	
	PROTECTED PROCEDURE CrearGuid
		LOCAL lsGuid, lsGGuid

		lsGuid = SPACE(16)
		lsGGuid = REPLICATE( CHR(0), 76 )

		*|-- To create the GUID and turns it to string. -
		CoCreateGuid( @lsGuid )
		StringFromGUID2( lsGuid, @lsGGuid, 76 )
		
		lsGGuid = STRTRAN( lsGGuid, CHR(0) )
		lsGGuid = SUBSTR( lsGGuid, 2, 36 )

		RETURN lsGGuid
	ENDPROC

	PROTECTED PROCEDURE CrearUuid( lsUIDPart1, lsUIDPart2, lsUIDPart3, lsUIDPart4 )
		LOCAL lsUUID

		lsUUID = SPACE( 16 )

		*|-- It generates the UUID. -
		UUIDCreate( @lsUUID )

		*|-- It turns the values of binary to hex. -
		lsUIDPart1 = This.BinTOHex( SUBSTR( lsUUID,  1, 4 ) )
		lsUIDPart2 = This.BinTOHex( SUBSTR( lsUUID,  5, 4 ) )
		lsUIDPart3 = This.BinTOHex( SUBSTR( lsUUID,  8, 4 ) )
		lsUIDPart4 = This.BinTOHex( SUBSTR( lsUUID, 13, 4 ) )

	ENDPROC

	PROCEDURE ObtenerUuid
		LOCAL lsUIDPart1, lsUIDPart2, lsUIDPart3, lsUIDPart4
		This.CrearUuid( @lsUIDPart1, @lsUIDPart2, @lsUIDPart3, @lsUIDPart4 )
		
		RETURN lsUIDPart1 + "_" + lsUIDPart2 + "_" + lsUIDPart3 + "_" + lsUIDPart4
	ENDPROC

	PROCEDURE ObtenerGuid
		LOCAL lsGuid
		lsGuid = This.CrearGuid()
		
		RETURN STRTRAN( lsGuid, "-", "_" )
	ENDPROC

	*|----------------------------------------------------------------------------------------------------------
	*|------------------------------------- Transformation of values ------------------------------------------
	*|----------------------------------------------------------------------------------------------------------
	PROCEDURE BinTOHex( lsVal )
		LOCAL lsReturn, lnBin

		lsReturn = ""
		FOR I = 1 TO LEN( lsVal )
		   lnBin    = ASC( SUBSTR( lsVal, I, 1) )
		   lsReturn = lsReturn + This.HexToChar( INT( lnBin/16 ) ) + This.HexToChar( MOD( lnBin, 16 ) )
		ENDFOR

		RETURN( lsReturn )
	ENDPROC

	PROCEDURE HexToChar( lnHexVal )
		LOCAL lnAsc

		DO CASE
			CASE BETWEEN( lnHexVal, 0, 9 )
			   lnAsc = 48 + lnHexVal
			 CASE BETWEEN( lnHexVal, 10, 15 )
			   lnAsc = 65 + lnHexVal - 10
		ENDCASE
		
		RETURN( CHR( lnAsc ) )
	ENDPROC

	PROCEDURE MakeWord( Val1, Val2 )
		*|------------------------------------------------------
		*| ((WORD) (((BYTE) (a)) | ((WORD) ((BYTE) (b))) << 8))
		*|------------------------------------------------------
		RETURN BITOR( Val1, BITLSHIFT( Val2, 8 ) )
	ENDPROC

	PROCEDURE Long2ToString( m.Valor )

		LOCAL I1, m.StringARetornar

		m.StringARetornar = ""

		FOR I1 = 24 TO 0 STEP -8
		   m.StringARetornar = CHR(INT(m.Valor/(2^I1))) + m.StringARetornar
		   m.Valor = MOD(m.Valor, (2^I1))
		NEXT

		RETURN m.StringARetornar

	ENDPROC

	PROCEDURE Short2ToString( m.Valor )

		RETURN CHR( BITAND( m.Valor, 0xFF ) ) + CHR( BITRSHIFT( m.Valor, 8 ) )

	ENDPROC

	PROCEDURE StringToLong( m.StrAConvertir )

		LOCAL I1, m.NroARetornar

		m.NroARetornar = 0
		FOR I1 = 0 TO 24 STEP 8
		   m.NroARetornar = m.NroARetornar + (ASC(m.StrAConvertir) * (2^I1))
		   m.StrAConvertir = RIGHT(m.StrAConvertir, LEN(m.StrAConvertir) - 1)
		NEXT

		RETURN m.NroARetornar

	ENDPROC

ENDDEFINE