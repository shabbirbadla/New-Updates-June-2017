Lparameters cCompName,cMacRetType
&&Changes done by Vasant on 18/05/2011 as per TKT-6331 (Blank Mac Id) & TKT-7997 (In the "Company Information Page" the partner code and the no.of users should be blank.)
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
			lcComputerName = Alltrim(cCompName)
			loWMIService = Getobject("winmgmts:\\" + lcComputerName + "\root\cimv2")
			loItems = loWMIService.ExecQuery("Select * from Win32_NetworkAdapterConfiguration",,48)
			For Each loItem In loItems
				*!*					lcMACAddress = loItem.MACAddress
				*!*					If !Isnull(lcMACAddress) And loItem.IPEnabled
				*!*		*!*					_MacId1 =  "MAC Address 1: " + loItem.MACAddress
				*!*						_MacId1 =  loItem.MACAddress
				*!*					Endif
				If loItem.IPEnabled
					_MacId1 =  loItem.MACAddress
				Endif
				If Isnull(_MacId1)
					_MacId1 = ''
				Endif
				_MacId1 = Transform(_MacId1)
				If !Empty(_MacId1)
					Exit
				Endif
			Endfor
			cRetVal = _MacId1
		Catch To ErrMsg
			=Messagebox(ErrMsg.Message,0,vumess)
		Endtry

	Case cMacRetType = "P"
		Try
			Local lcComputerName, loWMI, lowmiWin32Objects, lowmiWin32Object
			lcComputerName = Alltrim(cCompName)

			loWMI = Getobject("winmgmts:\\" + lcComputerName)

			lowmiWin32Objects = loWMI.InstancesOf("Win32_Processor")

			For Each lowmiWin32Object In lowmiWin32Objects
				With lowmiWin32Object
					*!*					_ProcId = "ProcessorId: " + Transform(.ProcessorId)
					&&Changes has been done for changing null value to blank on 24/02/2011 by Vasant
					*_ProcId = Transform(.ProcessorId)
					_ProcId = .ProcessorId
					If Isnull(_ProcId)
						_ProcId = ''
					Endif
					_ProcId = Transform(_ProcId)
					If !Empty(_ProcId)
						Exit
					Endif
					&&Changes has been done for changing null value to blank on 24/02/2011 by Vasant
				Endwith
			Endfor
			cRetVal = _ProcId
		Catch To ErrMsg
			=Messagebox(ErrMsg.Message,0,vumess)
		Endtry

	Case cMacRetType = "V"
		*!* Let's get the Volume Serial Number(s)
		Try
			Local lcComputerName, loWMIService, loItems, loItem
			lcComputerName = Alltrim(cCompName)
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
				If Isnull(_MacVolNm)
					_MacVolNm = ''
				Endif
				If Isnull(_MacVolNo)
					_MacVolNo = ''
				Endif
				_MacVolNm = Transform(_MacVolNm)
				_MacVolNo = Transform(_MacVolNo)
				If !Empty(_MacVolNm) And !Empty(_MacVolNo)
					Exit
				Endif
			Endfor
			cRetVal = _MacVolNm + "," + _MacVolNo
		Catch To ErrMsg
			=Messagebox(ErrMsg.Message,0,vumess)
		Endtry

	Case cMacRetType = "I"
		*!* Let's get the Current Machine IP Address
		Try
			Local lcComputerName, loWMIService, loItems, loItem
			lcComputerName = Alltrim(cCompName)
			loWMIService = Getobject("winmgmts:\\" + lcComputerName + "\root\cimv2")
			loItems = loWMIService.ExecQuery("Select * from Win32_NetworkAdapterConfiguration")
			For Each loItem In loItems
				If loItem.IPEnabled
					_MacIpAdr = loItem.IPAddress[0]
				Endif
				If Isnull(_MacIpAdr)
					_MacIpAdr = ''
				Endif
				_MacIpAdr = Transform(_MacIpAdr)
				If !Empty(_MacIpAdr)
					Exit
				Endif
			Endfor
			If Empty(_MacIpAdr)
				_MacIpAdr = '127.0.0.1'
			Endif
			cRetVal = _MacIpAdr
		Catch To ErrMsg
			=Messagebox(ErrMsg.Message,0,vumess)
		Endtry

	Case cMacRetType = "H"
		*!* Let's get the GUID from Current Hardware Profile
		Try
			Declare Integer GetCurrentHwProfile In advapi32 String @lpHwInfo

			#Define HW_PROFILE_GUIDLEN      39
			#Define MAX_PROFILE_LEN         80
			#Define DOCKINFO_UNDOCKED       1
			#Define DOCKINFO_DOCKED         2
			#Define DOCKINFO_USER_SUPPLIED  4
			#Define DOCKINFO_USER_UNDOCKED  DOCKINFO_USER_SUPPLIED + DOCKINFO_UNDOCKED
			#Define DOCKINFO_USER_DOCKED    DOCKINFO_USER_SUPPLIED + DOCKINFO_DOCKED

			Local cBuffer
			cBuffer = Repli(Chr(0),;
				4+HW_PROFILE_GUIDLEN + MAX_PROFILE_LEN)

			If GetCurrentHwProfile (@cBuffer) != 0
				_Guid = Substr(cBuffer,10,30)
			Endif
			_Guid = Substr(_Guid,1,1)+Padl(Asc(Substr(_Guid,2,1)),3,'0')+Substr(_Guid,3,5)+Padl(Asc(Substr(_Guid,8,1)),3,'0')+;
				PADL(Asc(Substr(_Guid,9,1)),3,'0')+Substr(_Guid,10,3)+Padl(Asc(Substr(_Guid,13,1)),3,'0')+;
				PADL(Asc(Substr(_Guid,14,1)),3,'0')+Substr(_Guid,15,9)+Padl(Asc(Substr(_Guid,24,1)),3,'0')+Substr(_Guid,25)
			_Guid = Strtran(_Guid,'-','')

			cRetVal = _Guid
		Catch To ErrMsg
			=Messagebox(ErrMsg.Message,0,vumess)
		Endtry
Endcase
&&Changes done by Vasant on 18/05/2011 as per TKT-6331 (Blank Mac Id) & TKT-7997 (In the "Company Information Page" the partner code and the no.of users should be blank.)
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








