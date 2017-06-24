Lparameters cCompName,cMacRetType
&&Changes done by Vasant on 18/05/2011 as per TKT-6331 (Blank Mac Id) & TKT-7998 (The application should generate default IP(127.0.0.1) if the IP is blank.)
Local cRetVal,_ProcId,_MacId,_MacVolNo,_MacVolNm,_MacId1,_MacIpAdr,_Guid

cRetVal = ''
_ProcId = ''
_MacId  = ''
_MacVolNo = ''
_MacVolNm = ''
_MacId1  = ''
_MacIpAdr = ''
_Guid	= ''

Do Case
	Case cMacRetType = "M"
		Try
			Local lcComputerName, loWMIService, loItems, loItem
			lcComputerName = ALLTRIM(cCompName)
			loWMIService = Getobject("winmgmts:\\" + lcComputerName + "\root\cimv2")
			loItems = loWMIService.ExecQuery("Select * from Win32_NetworkAdapterConfiguration",,48)
			For Each loItem In loItems
*!*					lcMACAddress = loItem.MACAddress
*!*					If !Isnull(lcMACAddress) And loItem.IPEnabled
*!*		*!*					_MacId1 =  "MAC Address 1: " + loItem.MACAddress
*!*						_MacId1 =  loItem.MACAddress
*!*					Endif
				IF loItem.IPEnabled
					_MacId1 =  loItem.MACAddress
				Endif	
				If Isnull(_MacId1)
					_MacId1 = ''
				Endif
				_MacId1 = Transform(_MacId1)
				IF !EMPTY(_MacId1)
					EXIT
				Endif	
			Endfor
			cRetVal = _MacId1
		CATCH TO ErrMsg
			=MESSAGEBOX(ErrMsg.Message,0,"Udyog Administrator")
		Endtry	 
		
	Case cMacRetType = "P"
		Try
			Local lcComputerName, loWMI, lowmiWin32Objects, lowmiWin32Object
			lcComputerName = ALLTRIM(cCompName)
		
			loWMI = Getobject("winmgmts:\\" + lcComputerName)
		
			lowmiWin32Objects = loWMI.InstancesOf("Win32_Processor")
			
			For Each lowmiWin32Object In lowmiWin32Objects
				With lowmiWin32Object
	*!*					_ProcId = "ProcessorId: " + Transform(.ProcessorId)
					&&Changes has been done for changing null value to blank on 24/02/2011 by Vasant
					*_ProcId = Transform(.ProcessorId)
					_ProcId = .ProcessorId
					IF ISNULL(_ProcId)
						_ProcId = ''
					ENDIF
					_ProcId = Transform(_ProcId)
					IF !EMPTY(_ProcId)
						EXIT
					Endif	
					&&Changes has been done for changing null value to blank on 24/02/2011 by Vasant
				Endwith
			Endfor
			cRetVal = _ProcId
		CATCH TO ErrMsg
			=MESSAGEBOX(ErrMsg.Message,0,"Udyog Administrator")
		Endtry	 
		
	Case cMacRetType = "V"
*!* Let's get the Volume Serial Number(s)
		Try
			Local lcComputerName, loWMIService, loItems, loItem
			lcComputerName = ALLTRIM(cCompName)
			loWMIService = Getobject("winmgmts:\\" + lcComputerName + "\root\cimv2")
			loItems = loWMIService.ExecQuery("Select * from Win32_LogicalDisk")
			For Each loItem In loItems
*!*					lcVolumeSerial = loItem.VolumeSerialNumber
*!*					If !Isnull(lcVolumeSerial)
*!*		*!*			        _MacVolNm = "Name: " + loItem.NAME
*!*		*!*			        _MacVolNo =  "Volume Serial Number: " + loItem.VolumeSerialNumber
*!*						_MacVolNm = Alltrim(loItem.Name)
*!*						_MacVolNo = Alltrim(loItem.VolumeSerialNumber)
*!*					Endif
				_MacVolNm = Alltrim(loItem.Name)
				_MacVolNo = Alltrim(loItem.VolumeSerialNumber)
				IF ISNULL(_MacVolNm)
					_MacVolNm = ''
				ENDIF
				IF ISNULL(_MacVolNo)
					_MacVolNo = ''
				ENDIF
				_MacVolNm = Transform(_MacVolNm)
				_MacVolNo = Transform(_MacVolNo)
				IF !EMPTY(_MacVolNm) AND !EMPTY(_MacVolNo)
					EXIT
				Endif	
			Endfor
			cRetVal = _MacVolNm + "," + _MacVolNo
		CATCH TO ErrMsg
			=MESSAGEBOX(ErrMsg.Message,0,"Udyog Administrator")
		Endtry	 

	Case cMacRetType = "I"
*!* Let's get the Current Machine IP Address
		Try
			Local lcComputerName, loWMIService, loItems, loItem
			lcComputerName = ALLTRIM(cCompName)
			loWMIService = Getobject("winmgmts:\\" + lcComputerName + "\root\cimv2")
			loItems = loWMIService.ExecQuery("Select * from Win32_NetworkAdapterConfiguration")
			For Each loItem In loItems
				If loItem.IPEnabled
					_MacIpAdr = loItem.IPAddress[0]
				Endif
				IF ISNULL(_MacIpAdr)
					_MacIpAdr = ''
				ENDIF
				_MacIpAdr = Transform(_MacIpAdr)
				IF !EMPTY(_MacIpAdr)
					EXIT
				Endif	
			ENDFOR
			IF EMPTY(_MacIpAdr)
				_MacIpAdr = '127.0.0.1'
			Endif
			cRetVal = _MacIpAdr
		CATCH TO ErrMsg
			=MESSAGEBOX(ErrMsg.Message,0,"Udyog Administrator")
		Endtry	 

	Case cMacRetType = "H"
*!* Let's get the GUID from Current Hardware Profile
		Try
			DECLARE INTEGER GetCurrentHwProfile IN advapi32 STRING @lpHwInfo
			 
			#DEFINE HW_PROFILE_GUIDLEN      39
			#DEFINE MAX_PROFILE_LEN         80
			#DEFINE DOCKINFO_UNDOCKED       1
			#DEFINE DOCKINFO_DOCKED         2
			#DEFINE DOCKINFO_USER_SUPPLIED  4
			#DEFINE DOCKINFO_USER_UNDOCKED  DOCKINFO_USER_SUPPLIED + DOCKINFO_UNDOCKED
			#DEFINE DOCKINFO_USER_DOCKED    DOCKINFO_USER_SUPPLIED + DOCKINFO_DOCKED
		 
			LOCAL cBuffer
			cBuffer = Repli(Chr(0),;
			    4+HW_PROFILE_GUIDLEN + MAX_PROFILE_LEN)

			IF GetCurrentHwProfile (@cBuffer) != 0
				_Guid = SUBSTR(cBuffer,10,30)
			ENDIF
			_guid = SUBSTR(_guid,1,1)+PADL(ASC(SUBSTR(_guid,2,1)),3,'0')+SUBSTR(_guid,3,5)+PADL(ASC(SUBSTR(_guid,8,1)),3,'0')+;
				PADL(ASC(SUBSTR(_guid,9,1)),3,'0')+SUBSTR(_guid,10,3)+PADL(ASC(SUBSTR(_guid,13,1)),3,'0')+;
				PADL(ASC(SUBSTR(_guid,14,1)),3,'0')+SUBSTR(_guid,15,9)+PADL(ASC(SUBSTR(_guid,24,1)),3,'0')+SUBSTR(_guid,25)
			_guid = STRTRAN(_guid,'-','')
			
			cRetVal = _Guid
		CATCH TO ErrMsg
			=MESSAGEBOX(ErrMsg.Message,0,"Udyog Administrator")
		Endtry	 
Endcase
&&Changes done by Vasant on 18/05/2011 as per TKT-6331 (Blank Mac Id) & TKT-7998 (The application should generate default IP(127.0.0.1) if the IP is blank.)
Return cRetVal


*!*	*!* Let's get the MAC Address(es)
*!*	LOCAL lcComputerName, loWMIService, loItems, loItem, lcMACAddress
*!*	lcComputerName = "."
*!*	loWMIService = GETOBJECT("winmgmts:\\" + lcComputerName + "\root\cimv2")
*!*	loItems = loWMIService.ExecQuery("Select * from Win32_NetworkAdapter",,48)
*!*	FOR EACH loItem IN loItems
*!*	    lcMACAddress = loItem.MACAddress
*!*	    IF !ISNULL(lcMACAddress)
*!*	        _MacId =  "MAC Address: " + loItem.MACAddress
*!*	    ENDIF
*!*	ENDFOR


*!*	?_ProcId
*!*	?_MacId
*!*	?_MacVolNo
*!*	?_MacVolNm
*!*	?_MacId1



************************************************************************

*!*	PUBLIC lcServer,lcBranchCode
*!*	LOCAL oServer, oAdapters, oAdapter1
*!*	SET PATH TO SYS(5)+SYS(2003);sysForms;sysPrgs;sysFiles


*!*	oServer = GetObject("winmgmts:\\" + "." + "\root\cimv2")
*!*	oAdapters = oServer.ExecQuery(;
*!*	"SELECT * FROM Win32_NetworkAdapterConfiguration")

*!*	CREATE CURSOR csResult (;
*!*	ObjectCaption C(50),;
*!*	Gateway C(50),;
*!*	IPAddress C(20),;
*!*	MACAddress C(20),;
*!*	IPXAddress C(20),;
*!*	ServiceName C(50),;
*!*	DHCPServer C(50),;
*!*	DNSHostName C(50);
*!*	)

*!*	LOCAL cGateway

*!*	FOR EACH oAdapter IN oAdapters
*!*	WITH oAdapter
*!*	IF .IPEnabled

*!*	cGateway = IIF(ISNULL(.DefaultIPGateway), "",;
*!*	.DefaultIPGateway[0])

*!*	INSERT INTO csResult VALUES (;
*!*	NVL(.Caption, ""),;
*!*	NVL(cGateway, ""),;
*!*	NVL(.IPAddress[0], ""),;
*!*	NVL(.MACAddress, ""),;
*!*	NVL(.IPXAddress, ""),;
*!*	NVL(.ServiceName, ""),;
*!*	NVL(.DHCPServer, ""),;
*!*	NVL(.DNSHostName, "");
*!*	)

*!*	ENDIF
*!*	ENDWITH
*!*	NEXT
*!*	SET STEP ON
*!*	SELECT csResult
*!*	GO TOP

*!*	SELECT csResult
*!*	BROWSE








