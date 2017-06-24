DEFINE CLASS WSocket AS CUSTOM
	NAME = "WSocket"
	PROTECTED m_HandleSocket
	m_HandleSocket = NULL
	PROTECTED m_ObjUtils
	m_ObjUtils = NULL
	m_HostName = NULL
	m_HostPort = NULL
	m_SockFamily = 0x2						&& AF_INET
	m_SockType   = 0x1						&& SOCK_STREAM
	SockSendCallMode = 0x0
	CheckBeforeRec  = .F.
	CheckBeforeSend = .F.
	PaqueteEnvio    = -1
	PROCEDURE INIT
		#DEFINE AF_INET						0x2
		#DEFINE PF_INET						0x2
		#DEFINE SOCK_STREAM					0x1
		#DEFINE INVALID_SOCKET 				0xFFFF  	&& 65535
		#DEFINE SOL_SOCKET					0x0000FFFF
		#DEFINE SO_OPENTYPE					0x00007008
		#DEFINE SO_SYNCHRONOUS_NONALERT		0x20
		#DEFINE SOCKET_ERROR 				0xFFFFFFFF
		#DEFINE INADDR_NONE					0xFFFFFFFF
		#DEFINE FIONBIO						0x8004667E
		#DEFINE SIOCATMARK					0x40047307
		#DEFINE FIONREAD					0x4004667F
		DECLARE LONG socket IN "wsock32.dll" LONG af, ;
				LONG prototype, LONG protocol
		DECLARE LONG WSAStartup IN "wsock32.dll" INTEGER wVersionRequested, ;
				STRING @lpWSAData
		DECLARE LONG WSACleanup IN "wsock32.dll"
		DECLARE LONG connect IN "wsock32.dll" LONG s, STRING @Name, LONG namelen
		DECLARE LONG inet_addr IN "wsock32.dll" STRING cp
		DECLARE INTEGER htons IN "wsock32.dll" INTEGER hostshort
		DECLARE LONG recv IN "wsock32.dll" LONG s, STRING @buf, ;
			LONG length, LONG flags
		DECLARE LONG ioctlsocket IN "wsock32.dll" LONG s, LONG cmd, LONG @argp
		DECLARE LONG closesocket IN "wsock32.dll" LONG s
		DECLARE LONG send IN "wsock32.dll" LONG s, STRING buf, ;
			LONG length, LONG flags
		DECLARE Sleep IN "kernel32" Long dwMilliseconds
		DECLARE INTEGER setsockopt IN "wsock32.dll" ;
			LONG s, INTEGER level, INTEGER optname, ;
			INTEGER optval, INTEGER optlen
		DECLARE INTEGER WSAGetLastError IN "wsock32.dll"
		DECLARE INTEGER shutdown IN "wsock32.dll" AS SocketShutDown ;
			LONG s, INTEGER how
		This.m_ObjUtils = CREATEOBJECT( "FoxUtils" )
	ENDPROC
	PROCEDURE CrearSock
		LOCAL wSpaceData
		LOCAL optionValue
		wSpaceData = SPACE( 400 ) && Space for the WSAData structure
		*|-- It initializes the WS2_32.DLL to use the Socket.
		IF ( WSAStartup( THIS.m_ObjUtils.MAKEWORD(1, 1), @wSpaceData) != 0 )
			RETURN .F. && The S.O. could not initialize socket
		ENDIF 
		optionValue = SO_SYNCHRONOUS_NONALERT
		IF( setsockopt( 			;
		        INVALID_SOCKET, 	;
		        SOL_SOCKET, 		;
		        SO_OPENTYPE,   		;
		        @optionValue,		;
		        4 					; && sizeof(int)
		        ) == SOCKET_ERROR )
			RETURN .F.
		ENDIF
		*|-- It obtains the Handle of the Socket of the S.O.
		THIS.m_HandleSocket = socket( This.m_SockFamily, This.m_SockType, 0 )
		IF THIS.m_HandleSocket == INVALID_SOCKET
			RETURN .F. && The S.O. could not obtain handle of socket
		ENDIF
	ENDPROC
	PROTECTED PROCEDURE ResolverNombre
		DECLARE RtlMoveMemory IN Win32API AS CopyMemory STRING@, INTEGER, INTEGER
		DECLARE LONG gethostbyname IN "wsock32.dll" STRING pname
		LOCAL hostent, pHostinfo, ipAddress, ipAddress2, h_addrtype 
		hostent = GetHostByName( THIS.m_HostName )
		IF hostent == 0
			RETURN -1
		ENDIF
		pHostinfo = SPACE( 16 ) && Struct hostent 16 Bytes Alloc
		ipAddress = ""
		*|-- Copy structure
		CopyMemory ( @pHostinfo, hostent, LEN( pHostinfo ) )
		h_addrtype = SUBSTR( pHostinfo, 9, 2 )	&& Position of h_addrtype property
		IF h_addrtype != THIS.m_ObjUtils.Short2ToString( AF_INET )
			RETURN -1
		ENDIF
		*|-- Position of property h_addr_list on hostent struct.-
		ipAddress = SUBST( pHostinfo, 13, 4 )
		ipAddress = INT( THIS.m_ObjUtils.StringToLong( ipAddress ) )
		*|-- Search direction of memory of list server.-
		ipAddress2 = SPACE( 4 ) 
		CopyMemory( @ipAddress2, ipAddress, 4 )
		ipAddress2 = INT( THIS.m_ObjUtils.StringToLong( ipAddress2 ) )
		*|-- Copy direction of server from direction of memory of the list.-
		ipAddress = SPACE( 4 )
		CopyMemory( @ipAddress, ipAddress2, 4 )
		ipAddress = INT( THIS.m_ObjUtils.StringToLong( ipAddress ) )
		RETURN ipAddress
	ENDPROC
	PROCEDURE ConectarServer
		LOCAL lsipAddress, lsStructConn, lnReturn
		lsipAddress = inet_addr( THIS.m_HostName )
		IF ( lsipAddress == INADDR_NONE OR lsipAddress <= 0 )
			*|-- Can´t find direction IP, is possible that string is name of server.-
			lsipAddress = This.ResolverNombre()
			IF lsipAddress == -1
				RETURN .F.
			ENDIF
		ENDIF
		*|-----
		*|--	typedef struct SOCKADDR{
		*|--		short 		sin_family		// 2 Bytes
		*|--		unsigned short	sin_port		// 2 Bytes
		*|--		long 			sin_addr		// 4 Bytes
		*|--		char 			sin_zero[8]		// 8 Bytes
		*|--	};
		*|-----
		lsStructConn = ""
		lsStructConn = lsStructConn + THIS.m_ObjUtils.Short2ToString( AF_INET )
		lsStructConn = lsStructConn + THIS.m_ObjUtils.Short2ToString( ;
						htons( THIS.m_HostPort ) )
		lsStructConn = lsStructConn + THIS.m_ObjUtils.Long2ToString( lsipAddress )
		lsStructConn = lsStructConn + SPACE( 8 )
		lnReturn = connect( THIS.m_HandleSocket, @lsStructConn,;
			LEN( lsStructConn ) ) && sizeof(SOCKADDR) -> 16 Bytes
		RETURN IIF( lnReturn == -1, .F., .T. )
	ENDPROC
	PROCEDURE Enviar( msg )
		LOCAL slen, nlenMsg, nindex, smsgAux, bWait, nCountWait
		LOCAL lnControl
		bWait      = .T.
		nCountWait = 0
		nlenMsg    = LEN( msg )
		nindex     = 0
		slen       = 0
		IF( This.CheckBeforeSend )
			nRead = 0x1
			DO WHILE( bWait )
				slen  = ioctlsocket( THIS.m_HandleSocket, FIONBIO, @nRead )
				IF ( slen != 0 ) && SOCKET_ERROR
					IF( nCountWait > 5 )
						RETURN .F.
					ELSE
						*|-- The server is busy.-
						nCountWait = nCountWait + 1
						Sleep( 50 )
					ENDIF
				ELSE
					bWait = .F.
				ENDIF
			ENDDO
		ENDIF
		smsgAux       = msg
		nlenMsgGlobal = nlenMsg
		nlenMsg2      = nlenMsg
		Vuelta        = 0
		PaqEnv        = 512
		IF( This.PaqueteEnvio <= 0 )
			PaqEnv = nlenMsgGlobal
		ELSE
			PaqEnv = This.PaqueteEnvio
		ENDIF
		DO WHILE( nlenMsgGlobal > 0 )
			smsgAux  = SUBSTR( msg, ( Vuelta * PaqEnv ) + 1, PaqEnv )
			nlenMsg  = LEN( smsgAux )
			nlenMsg2 = nlenMsg
			DO WHILE( nlenMsg > 0 )
				slen = send( THIS.m_HandleSocket, smsgAux, nlenMsg, This.SockSendCallMode )
				IF ( slen < 1 )
					RETURN .F. && Fail the send of server.-
				ENDIF
				nlenMsg = nlenMsg - slen
				nindex  = nindex + slen
				smsgAux = SUBSTR( msg, nindex, LEN( msg ) - nindex )
			ENDDO
			nlenMsgGlobal = nlenMsgGlobal - nlenMsg2
			Vuelta        = Vuelta + 1
		ENDDO
		RETURN .T.
	ENDPROC
	PROCEDURE Recibir()
		LOCAL buffer, retval, reply, nRead, slen, bWait, nCountWait
		bWait      = .T.
		nCountWait = 0
		IF( This.CheckBeforeRec )
			nRead      = 0x1
			DO WHILE( bWait )
				slen  = ioctlsocket( THIS.m_HandleSocket, FIONREAD, @nRead )
				IF ( slen == SOCKET_ERROR OR nRead <= 0 )
					IF( nCountWait > 5 )
						RETURN ""
					ELSE
						*|-- The server is busy.-
						nCountWait = nCountWait + 1
						Sleep( 50 )
					ENDIF
				ELSE
					bWait = .F.
				ENDIF
			ENDDO
		ENDIF
		reply  = ""
		buffer = ""
		retval = 0
		DO WHILE( retval = 0 )
			buffer = SPACE( 4096 )
			retval = recv( THIS.m_HandleSocket, @buffer, LEN( buffer ), 0 )
			IF retval <> 0 AND retval <> SOCKET_ERROR
				reply = reply + LEFT( buffer, retval )
			ENDIF
		ENDDO
		RETURN reply
	ENDPROC
	PROCEDURE Desconectarme()
		LOCAL nClose
		*|-- Authenticate the socket.-
		IF THIS.m_HandleSocket == INVALID_SOCKET
			RETURN .F.
		ENDIF
		SocketShutDown( THIS.m_HandleSocket, 2 )
		nClose = closesocket( THIS.m_HandleSocket )
		WSACleanup()
		RETURN IIF ( nClose != SOCKET_ERROR, .T., .F. )
	ENDPROC
ENDDEFINE