DEFINE CLASS WSendMail AS CUSTOM OLEPUBLIC
	NAME = "WSendMail"
	*|-- Source of mail.-
	m_Originante    = ""
	*|-- Addressee of mail, have to be separate by ";".-
	m_Destinatarios = ""
	*|-- Eg. "MANOJ, UDAY" <manoj.jain@globaludyog.com>.-
	*|-- Address of mail (recipient), with complete name for client mail.-
	m_To            = ""
	*|-- Eg. "MANOJ, UDAY" <manoj.jain@globaludyog.com>.-
	*|-- Address of mail (recipient), with complete name for client mail( Carbon copy ).-
	m_Cc            = ""
	*|-- Eg. "MANOJ, UDAY" <manoj.jain@globaludyog.com>.-
	*|-- Address of mail (emitter), with complete name for client mail.-
	m_De            = ""
	*|-- Eg. "MANOJ, UDAY" <manoj.jain@globaludyog.com>.-
	*|-- Address of mail (reply).-
	m_ResponderA    = ""
	*|-- Eg. "MANOJ, UDAY" <manoj.jain@globaludyog.com>.-
	*|-- Address of mail (notification).-
	m_NotificarA    = ""
	*|-- Name of application that send mail.-
	m_xMailer       = "WSendMail (By Manoj Jain - <manoj.jain@globaludyog.com>)"
	*|-- Nombre del X-Sender del mail.-
	m_xSender       = ""
	*|-- Nombre del Sender del mail.-
	m_Sender        = ""
	*|-- Body of mail, in plain text or HTML Text.-
	m_Body     = ""
	*|-- Subject of mail.-
	m_Asunto        = ""
	*|-- Formato del mail. T = Texto
	*|-                    H = HTML
	*|-- Type of Mail.
	m_TipoMail      = "T"
	*|-- Priority of Mail.   A = highs
	*|--                     B = Low
	*|--					 N = Normal
	*|--                    
	m_Prioridad    = "N"

	*|-- Character of mail. N = Normal
	*|-                     P = Personal
	*|-                     V = Private
	*|-                     C = Confidential
	m_TipoCaracter  = "N"	
	*|-- Zona of the mail.-
	m_ZonaHoraria = "-0300" && Argentina
	*|-- User for SMTP connection.-
	m_UsrSMTP  = ""
	*|-- Password for user STMP.-
	m_PassSMTP = ""
	*|-- Type of authentication.
	*|--							0 = Plana
	*|--							1 = base 64 bits
	m_TipoAut = 0
	*|-- Object FoxUtils.-
	PROTECTED m_ObjUtils
	m_ObjUtils = .NULL.
	*|-- Last receives message.-
	m_UltMsg = ""
	
	*|-- Type of YOU GO that it will be generated for the NextPart. -
	*|--							0 = GUID
	*|--							1 = UUID
	m_TipoMailPartID = 0
	*|-- ID for Body Part of mail.-
	m_MailPartID = "000_333d_1506_5025"
	
	*|-- Character of separation of direction. -
	m_SeparadorDir = ";"
	*|-- Conexion to socket.-
	PROTECTED m_SocketConn
	m_SocketConn = .NULL.
	*|-- Host name.-
	m_HostName = ""
	*|-- Port to connect Server Mail.-
	m_HostPort = 25
	*|-- Version of MIME.
	m_MIME_Version = "1.0"
	*|-- Length of line for file attach.
	m_LargoCadenaAdjunto = 76
	*|-- Error
	m_Error = ""
	*|-- Internal property that assign if mail have files attach.-
	PROTECTED ArchivoAdjunto
	ArchivoAdjunto = .F.
	*|-- Internal property with list of files to attach.-
	PROTECTED m_ListaAdjuntos[1]

	PROCEDURE INIT
		#DEFINE ENTER			CHR(13) + CHR(10)
		#DEFINE NO_ERRORS		"OK"
		#DEFINE CHK_220_REPLY	"220"
		#DEFINE CHK_221_REPLY	"221"
		#DEFINE CHK_235_REPLY	"235"
		#DEFINE CHK_250_REPLY	"250"
		#DEFINE CHK_334_REPLY	"334"
		#DEFINE CHK_354_REPLY	"354"

		This.m_SocketConn = CREATEOBJECT( "WSocket" )
		This.m_ObjUtils   = CREATEOBJECT( "FoxUtils" )

		RETURN DODEFAULT()
	ENDPROC


	PROTECTED PROCEDURE SetInfoCon()
		This.m_SocketConn.m_HostPort = This.m_HostPort
		This.m_SocketConn.m_HostName = This.m_HostName		
	ENDPROC

	PROTECTED PROCEDURE Helo
		#DEFINE LOCAL_HOST_NAME_LEN 256
		#DEFINE SOCKET_ERROR         -1
		#DEFINE ENTER		CHR(13) + CHR(10)
		DECLARE LONG gethostname IN "wsock32.dll" STRING @sname, LONG namelen
		LOCAL LocalHost, lnLenHostName
		LOCAL lsResult
		*|-- Get machine name.-
		LocalHost = SPACE(LOCAL_HOST_NAME_LEN)
		IF ( GetHostName( @LocalHost, LOCAL_HOST_NAME_LEN ) == SOCKET_ERROR )
			RETURN .F. && Cant'n get machine name.-
		ENDIF

		lnLenHostName = AT( CHR(0), LocalHost )
		IF( lnLenHostName > 0 )
			LocalHost = SUBSTR( LocalHost, 1, lnLenHostName - 1 )
		ENDIF
		*|-- Send HELO To sever with IP.-
		IF( EMPTY( This.m_UsrSMTP ) AND EMPTY( This.m_PassSMTP ) )
			IF( !THIS.m_SocketConn.Enviar( "HELO " + ALLTRIM( LocalHost ) + ENTER ) )
				RETURN .F.
			ENDIF
			IF( !This.VerificarRespuesta( THIS.m_SocketConn.Recibir(), CHK_250_REPLY ) )
				RETURN .F.
			ENDIF
		ELSE
			IF( !THIS.m_SocketConn.Enviar( "EHLO " + ALLTRIM( This.m_HostName ) + ENTER ) )
				RETURN .F.
			ENDIF
			IF( !This.VerificarRespuesta( THIS.m_SocketConn.Recibir(), CHK_250_REPLY ) )
				RETURN .F.
			ENDIF
		ENDIF
		RETURN .T.
	ENDPROC
	*|-----------------------------------------------------------------------------------------
	*LOGIN USING USERNAME AND PASSWORD OF SMTP (IF THERE IS ANY....) 
	*|-----------------------------------------------------------------------------------------
	PROTECTED PROCEDURE Login
		LOCAL lsPass, lsUsr
		IF( EMPTY( This.m_UsrSMTP ) AND EMPTY( This.m_PassSMTP ) )
			RETURN .T.
		ENDIF
		DO CASE
			CASE This.m_TipoAut == 0
				IF( !THIS.m_SocketConn.Enviar( "AUTH PLAIN" + ENTER ) )
					RETURN .F.
				ENDIF
				IF( !This.VerificarRespuesta( THIS.m_SocketConn.Recibir(), CHK_334_REPLY ) )
					RETURN .F.
				ENDIF
				lsUsr  = This.m_UsrSMTP
				lsPass = This.m_PassSMTP
			CASE This.m_TipoAut == 1
				IF( !THIS.m_SocketConn.Enviar( "AUTH LOGIN" + ENTER ) )
					RETURN .F.
				ENDIF
				IF( !This.VerificarRespuesta( THIS.m_SocketConn.Recibir(), CHK_334_REPLY ) )
					RETURN .F.
				ENDIF
				IF( This.m_ObjUtils.Encode64( This.m_UsrSMTP, @lsUsr ) )
					IF( !This.m_ObjUtils.Encode64( This.m_PassSMTP, @lsPass ) )
						RETURN .F.
					ENDIF
				ELSE
					RETURN .F.
				ENDIF
		ENDCASE
		*|-- Send the user name.-
		IF( !THIS.m_SocketConn.Enviar( lsUsr + ENTER ) )
			RETURN .F.
		ENDIF
		IF( !This.VerificarRespuesta( THIS.m_SocketConn.Recibir(), CHK_334_REPLY ) )
			RETURN .F.
		ENDIF
		*|-- Send the password.-
		IF( !THIS.m_SocketConn.Enviar( lsPass + ENTER ) )
			RETURN .F.
		ENDIF
		
		IF( !This.VerificarRespuesta( THIS.m_SocketConn.Recibir(), CHK_235_REPLY ) )
			RETURN .F.
		ENDIF

		RETURN .T.
	ENDPROC
	*|-----------------------------------------------------------------------------------------
	*|-- EnviarCabeceraMail send message to mail server with all header of mail. ( RFC822 ).-
	*|-----------------------------------------------------------------------------------------
	PROTECTED PROCEDURE EnviarCabeceraMail
		LOCAL nAcounts
		LOCAL _timezone
		LOCAL zoneh
		LOCAL zonem
		LOCAL pbuf 
		LOCAL nPosFind
		LOCAL nOldPosFind
		LOCAL cRCPT
		LOCAL lsResult
		*|-- Begin.....
		IF( !THIS.m_SocketConn.Enviar( "MAIL FROM:<" +  ALLTRIM ( THIS.m_Originante ) + ">" + ENTER ) )
			RETURN .F.
		ENDIF
		IF( !This.VerificarRespuesta( THIS.m_SocketConn.Recibir(), CHK_250_REPLY ) )
			RETURN .F.
		ENDIF
		pbuf        = ""
		nOldPosFind = 1
		nPosFind    = 1
		*|-- Addressee of mail.-
		nAcounts = OCCURS( This.m_SeparadorDir, THIS.m_Destinatarios )
		IF nAcounts == 0
			*|-- Coma not found.-
			cRCPT = "RCPT TO:<" + THIS.m_Destinatarios + ">" + ENTER
			IF( !THIS.m_SocketConn.Enviar( cRCPT ) )
				RETURN .F.
			ENDIF
			IF( !This.VerificarRespuesta( THIS.m_SocketConn.Recibir(), CHK_250_REPLY ) )
				RETURN .F.
			ENDIF
		ELSE
			nAcounts = nAcounts + 1
			FOR i = 1 TO nAcounts
				nPosFind = AT( This.m_SeparadorDir, THIS.m_Destinatarios, i )
				IF ( nPosFind != 0 )
					cRCPT = "RCPT TO:<" + SUBSTR( THIS.m_Destinatarios, nOldPosFind, ;
						nPosFind - nOldPosFind ) + ">" + ENTER
					IF( !THIS.m_SocketConn.Enviar( cRCPT ) )
						RETURN .F.
					ENDIF
					IF( !This.VerificarRespuesta( THIS.m_SocketConn.Recibir(), CHK_250_REPLY ) )
						RETURN .F.
					ENDIF
					nOldPosFind = nPosFind + 1
				ELSE
					cRCPT = "RCPT TO:<" + SUBSTR( THIS.m_Destinatarios, nOldPosFind, ;
						( LEN( THIS.m_Destinatarios ) - nOldPosFind ) + 1 ) + ">" + ENTER
					IF( !THIS.m_SocketConn.Enviar( cRCPT ) )
						RETURN .F.
					ENDIF
					IF( !This.VerificarRespuesta( THIS.m_SocketConn.Recibir(), CHK_250_REPLY ) )
						RETURN .F.
					ENDIF
					nOldPosFind = nPosFind + 1
					EXIT					
				ENDIF
			NEXT
		ENDIF
		*|-- Inform to server that begin send information of email.-
		IF( !THIS.m_SocketConn.Enviar( "DATA" + ENTER ) )
			RETURN .F.
		ENDIF
		IF( !This.VerificarRespuesta( THIS.m_SocketConn.Recibir(), CHK_354_REPLY ) )
			RETURN .F.
		ENDIF
		*|-- Day of mail.-
		LOCAL dtcHora
		LOCAL days( 7 )
		LOCAL months( 12 )
		days[ 1 ] = "Sun"
		days[ 2 ] = "Mon"
		days[ 3 ] = "Tue"
		days[ 4 ] = "Wed"
		days[ 5 ] = "Thu"
		days[ 6 ] = "Fri"
		days[ 7 ] = "Sat"
		
		months[ 1  ] = "Jan"
		months[ 2  ] = "Feb"
		months[ 3  ] = "Mar"
		months[ 4  ] = "Apr"
		months[ 5  ] = "May"
		months[ 6  ] = "Jun"
		months[ 7  ] = "Jul"
		months[ 8  ] = "Aug"
		months[ 9  ] = "Sep"
		months[ 10 ] = "Oct"
		months[ 11 ] = "Nov"
		months[ 12 ] = "Dec"
		
		sTypeDate = SET("Date")
		sTypeHour = SET("Hours")
		sTypeCent = SET("Century")
		SET DATE TO BRITISH
		SET HOURS TO 24
		SET CENTURY ON
		dtcHora = TTOC( DATETIME() )
		SET DATE TO &sTypeDate
		SET HOURS TO &sTypeHour 
		SET CENTURY &sTypeCent 
		*|-- Date: Tue, 26 Dec 2007 13:45:00 -300
		pbuf = pbuf + "Date: " + days[ DOW( DATE() ) ] + ", " +;
			STR( DAY( DATE() ), 2 ) + " " + months[ MONTH( DATE() ) ] + " " +;
			STR( YEAR( DATE() ), 4 ) + " " + SUBSTR( dtcHora, 12, 2 ) + ":" +;
			SUBSTR( dtcHora, 15, 2 ) + ":" + SUBSTR( dtcHora, 18, 2 ) + " " +;
			THIS.m_ZonaHoraria + ENTER
		*|-- "FROM" Specify.-
		pbuf = pbuf + "From: " + THIS.m_De + ENTER
		*|-- "SUBJECT" Specify.-
		pbuf = pbuf + "Subject: " +	IIF(.NOT.EMPTY(NVL(THIS.m_Asunto,.F.)),THIS.m_Asunto,THIS.m_vSubject) + ENTER
		*|-- "SENDER" Specify.-
		IF( !EMPTY( THIS.m_Sender ) )
			pbuf = pbuf + "Sender: " + THIS.m_Sender + ENTER
		ELSE
			IF( !EMPTY( THIS.m_Originante ) )
				pbuf = pbuf + "Sender: " + THIS.m_Originante + ENTER
			ENDIF
		ENDIF
		*|-- Add "REPLY-TO" to header mail.-
		IF( !EMPTY( THIS.m_ResponderA ) )
			pbuf = pbuf + "Reply-To: " + THIS.m_ResponderA + ENTER
		ELSE
			IF( !EMPTY( THIS.m_Originante ) )
				pbuf = pbuf + "Reply-To: " + THIS.m_Originante + ENTER
			ENDIF
		ENDIF
		*|-- "TO" Specify.-
		pbuf = pbuf + "To: " + THIS.m_To + ENTER
		*|-- Add "CC".-
		IF( !EMPTY( THIS.m_Cc ) )
			pbuf = pbuf + "Cc: " + THIS.m_Cc + ENTER
		ENDIF
		*|-- Begin header mail in agreement of RFC822.-
		*|-- Add X-Sender to header of email.-
		IF( !EMPTY( THIS.m_xSender ) )
			pbuf = pbuf + "X-Sender: " + THIS.m_xSender + ENTER
		ENDIF
		*|-- Add X-Mailer to header of email.-
		IF( !EMPTY( THIS.m_xMailer ) )
			pbuf = pbuf + "X-Mailer: " + THIS.m_xMailer + ENTER
		ENDIF
		*|-- Priority of mail.-
		DO CASE
			CASE THIS.m_Prioridad = "A"  && Alta
				pbuf = pbuf + "X-Priority: 1 (High)"   + ENTER ;
							+ "Importance: High"       + ENTER ;
							+ "X-MSMail-priority: High"
			CASE THIS.m_Prioridad = "N"  && Nomal
				pbuf = pbuf + "X-Priority: 3 (Normal)" + ENTER ;
							+ "Importance: Normal"     + ENTER ;
							+ "X-MSMail-priority: Normal"
			CASE THIS.m_Prioridad = "B"  && Baja
				pbuf = pbuf + "X-Priority: 5 (Lowest)" + ENTER ;
							+ "Importance: Low"        + ENTER ;
							+ "X-MSMail-priority: Lowest"
		ENDCASE
		*|-- Character of mail.-
		DO CASE
			CASE THIS.m_TipoCaracter = "P"  && Personal
				pbuf = pbuf + "Sensitivity: Personal"          	  + ENTER
			CASE THIS.m_TipoCaracter = "V"  && Privado
				pbuf = pbuf + "Sensitivity: Private"          	  + ENTER
			CASE THIS.m_TipoCaracter = "C"  && Confidencial
				pbuf = pbuf + "Sensitivity: Company-Confidential" + ENTER
			CASE THIS.m_TipoCaracter = "N"  && Normal
		ENDCASE
		*|-- Send order of read notification.-
		IF !EMPTY(THIS.m_NotificarA)
			pbuf = pbuf + 'Disposition-Notification-To: ' + ENTER
		ENDIF
		*|-- Add MEME-Version.-
		IF( !EMPTY( THIS.m_MIME_Version ) )
			pbuf = pbuf + "MIME-Version: " + THIS.m_MIME_Version + ENTER
		ENDIF
		*|-- Send header of mail.-
		IF( !THIS.m_SocketConn.Enviar( pbuf ) )
			RETURN .F.
		ENDIF
		IF THIS.ArchivoAdjunto = .T.
			IF( !THIS.m_SocketConn.Enviar( 'Content-Type: multipart/mixed; boundary="----=_NextPart_' + This.m_MailPartID + '"' + ENTER ) )
				RETURN .F.
			ENDIF
		ELSE
			*|-- Type of context
			IF This.m_TipoMail = "T" && Texto
				pbuf = "Content-Type: text/plain;charset=iso-8859-1" + ENTER
			ELSE && HTML
				pbuf = "Content-Type: text/html;charset=iso-8859-1" + ENTER
			ENDIF
			
			IF( !THIS.m_SocketConn.Enviar( pbuf ) )
				RETURN .F.
			ENDIF
		ENDIF
		*|-- Send empty line for indicate end of header mail.-
		IF( !THIS.m_SocketConn.Enviar( ENTER ) )
			RETURN .F.
		ENDIF
		
		RETUR .T.
	ENDPROC
	PROTECTED PROCEDURE EnviarCuerpo

		IF THIS.ArchivoAdjunto
			LOCAL lpcsBody

			lpcsBody = ""

			IF THIS.ArchivoAdjunto = .T.
				lpcsBody = "This is a multipart message in MIME format." + ENTER + ENTER
			ENDIF

			lpcsBody = lpcsBody + "------=_NextPart_" + This.m_MailPartID + ENTER

			*|-- Tipo de contexto.-
			*|-- Type of context.-
			IF This.m_TipoMail = "T" && Texto
				lpcsBody = lpcsBody + "Content-Type: text/plain;charset=iso-8859-1" + ENTER
			ELSE && HTML
				lpcsBody = lpcsBody + "Content-Type: text/html;charset=iso-8859-1" + ENTER
			ENDIF
			
			IF( !THIS.m_SocketConn.Enviar( lpcsBody ) )
				RETURN .F.
			ENDIF
			
			IF( !THIS.m_SocketConn.Enviar( "Content-Transfer-Encoding: quoted-printable" + ENTER ) )
				RETURN .F.
			ENDIF
			
			lpcsBody = ENTER + THIS.m_Body + ENTER + ENTER

			IF( !THIS.m_SocketConn.Enviar( lpcsBody ) )
				RETURN .F.
			ENDIF
		ELSE
			RETURN THIS.m_SocketConn.Enviar( THIS.m_Body )
		ENDIF
	
	ENDPROC
	PROTECTED PROCEDURE EnviarFinMail
		*|-- Indicate end of mail.-
		IF( !THIS.m_SocketConn.Enviar( ENTER + "." + ENTER ) )
			RETURN .F.
		ENDIF
		
		RETURN( This.VerificarRespuesta( THIS.m_SocketConn.Recibir(), CHK_250_REPLY ) )
	ENDPROC
	PROCEDURE AdjuntarArchivo( lspFile )
		LOCAL lnPos
		IF !FILE( lspFile )
			RETURN .F.
		ENDIF
		IF !THIS.ArchivoAdjunto
			DIMENSION THIS.m_ListaAdjuntos[ 1 ]
			lnPos = 1
		ELSE
			lnPos = ALEN( THIS.m_ListaAdjuntos ) + 1
			DIMENSION THIS.m_ListaAdjuntos[ lnPos ]
		ENDIF
		THIS.ArchivoAdjunto = .T.
		THIS.m_ListaAdjuntos[ lnPos  ] = lspFile 
		RETURN .T.
	ENDPROC
	PROTECTED PROCEDURE EnviarArchivos
		LOCAL lnLen, i, iFile, lpcsBody, sFile, lsEncodeFile, lsMimeType
		LOCAL lsResult
		lnLen = ALEN( THIS.m_ListaAdjuntos )
		lpcsBody = ""

		FOR iFile = 1 TO lnLen 
			sFile      = THIS.m_ListaAdjuntos[ iFile ]
			lsMimeType = This.m_ObjUtils.ObtenerMimeType( sFile )
			lpcsBody = lpcsBody + '------=_NextPart_' + This.m_MailPartID 							+ ENTER +;
					'Content-Type: ' + lsMimeType +  'name=' 			 + JUSTFNAME(sFile)			+ ENTER +;
					'Content-Transfer-Encoding: base64'												+ ENTER +;
					'Content-Description: '								 + JUSTFNAME(sFile) 		+ ENTER +;
					'Content-Disposition: attachment; filename="'		 + JUSTFNAME(sFile) + '"' 	+ ENTER +;
					ENTER
			IF( This.m_ObjUtils.FileEncode64( sFile, @lsEncodeFile, This.m_LargoCadenaAdjunto ) )
				lpcsBody = lpcsBody + lsEncodeFile + ENTER + ENTER
				IF( !THIS.m_SocketConn.Enviar( lpcsBody ) )
					RETURN .F.
				ENDIF
				IF iFile == lnLen 
					IF( !THIS.m_SocketConn.Enviar( '------=_NextPart_' + This.m_MailPartID + '--' + ENTER + ENTER ) )
						RETURN .F.
					ENDIF
				ENDIF
				lpcsBody = ""
				RELEASE sBodyFile
				RELEASE sEncodeFile
			ELSE
				RETURN .F.
			ENDIF
		NEXT
		RETURN .T.
	ENDPROC
	PROCEDURE SendMail
		LOCAL lbReturn	
		lbReturn = .T.
		*|-- Create Socket in WSocket class.-
		IF ( !THIS.m_SocketConn.CrearSock() )
			This.SetError( "The Socket could not be created!!", .F. )
			RETURN .F.
		ENDIF
		This.SetInfoCon()
		*|-- Connect to server mail.-
		IF ( THIS.m_SocketConn.ConectarServer() )
			IF( This.VerificarRespuesta( THIS.m_SocketConn.Recibir(), CHK_220_REPLY ) )
				This.GenIDMail()
				IF( THIS.Helo() )
					IF( This.Login() )
						IF ( THIS.EnviarCabeceraMail() )
							IF( THIS.EnviarCuerpo() )
								IF THIS.ArchivoAdjunto = .T.
									IF !THIS.EnviarArchivos()
										This.SetError( "It could not send the archives of the mail!!", .T. )
										lbReturn = .F.
									ENDIF
								ENDIF
								IF( lbReturn )
									IF( !THIS.EnviarFinMail() )
										This.SetError( "It could not send end of the mail!!", .T. )
										lbReturn = .F.
									ENDIF
								ENDIF
							ELSE
								This.SetError( "It could not send the body of the mail!!", .T. )
								lbReturn = .F.
							ENDIF
						ELSE
							This.SetError( "It could not send the head of the mail!!", .T. )
							lbReturn = .F.
						ENDIF
					ELSE
						This.SetError( "Invalid Username and Password for Mail server!!!", .T. )
						lbReturn = .F.
					ENDIF
				ELSE
					This.SetError( "It could not send the greeting of the mail!!", .T. )
					lbReturn = .F.
				ENDIF
			ELSE
				This.SetError( "I connected myself with the server, but this not suitable!!", .T. )
				lbReturn = .F.
			ENDIF
			THIS.QuitSMTP()
			THIS.m_SocketConn.Desconectarme()
		ELSE
			This.SetError( "Specified Mail Server not Found or Invalid!!!", .F. )
			RETURN .F.
		ENDIF
		RETURN lbReturn
	ENDPROC
	PROTECTED PROCEDURE GenIDMail()
		DO CASE
			CASE This.m_TipoMailPartID = 0
				This.m_MailPartID = This.m_ObjUtils.ObtenerGuid()
			CASE This.m_TipoMailPartID = 1
				This.m_MailPartID = This.m_ObjUtils.ObtenerUuid()
		ENDCASE		
	ENDPROC
	PROTECTED PROCEDURE SetError( lsMsgErr, lbAgregarUM )
		This.m_Error = lsMsgErr + IIF( lbAgregarUM, " - " + This.m_UltMsg, "" )
	ENDPROC
	PROTECTED PROCEDURE QuitSMTP
		*|-- Send quit menssage to server.-
		IF( !THIS.m_SocketConn.Enviar( "QUIT" + ENTER ) )
			RETURN .F.
		ENDIF
		RETURN( This.VerificarRespuesta( THIS.m_SocketConn.Recibir(), CHK_221_REPLY ) )
	ENDPROC
	PROTECTED PROCEDURE VerificarRespuesta( lsString, lsToVerify )
		This.m_UltMsg = lsString
		RETURN( SUBSTR( lsString, 1, LEN(lsToVerify) ) == lsToVerify )
	ENDPROC
ENDDEFINE