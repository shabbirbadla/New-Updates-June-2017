*!*	Find the current IP address, MAC or Subnet 
*!*	faq184-3356
*!*	Posted: 20 Mar 03 

=getNicinfo('NONE')

*************************************************************************
* From Rob444 on tek-tips.com Jan 16, 2003
* wgcs converted to a callable function to return a specific data item.
* 
* Available data items:
*   MAC, Name, Desc, IP, Index, Type, Gateway, DHCPON, DHCPServer
*
* Library IPHLPAPI.DLL requires Win98/2000
*************************************************************************
PROCEDURE GetNicInfo
LPARAMETER pcItem
LOCAL lcItem
lcItem = iif( vartype(pcItem)='C', upper(pcItem), 'NONE' )
#DEFINE ERROR_SUCCESS           0 
#DEFINE ERROR_NOT_SUPPORTED     50 
#DEFINE ERROR_INVALID_PARAMETER 87 
#DEFINE ERROR_BUFFER_OVERFLOW   111 
#DEFINE ERROR_NO_DATA           232 

DECLARE INTEGER GetAdaptersInfo IN iphlpapi; 
    STRING @pAdapterInfo, LONG @pOutBufLen 

LOCAL lcBuffer, lnBufsize 
lnBufsize = 0 
lcBuffer = "" 

* this call usually returns the ERROR_BUFFER_OVERFLOW 
* with lnBufsize set to the required amount of memory 
= GetAdaptersInfo(@lcBuffer, @lnBufsize) 

lcBuffer = Repli(Chr(0), lnBufsize) 
IF GetAdaptersInfo(@lcBuffer, @lnBufsize) <> ERROR_SUCCESS 
  * still something is wrong 
  RETURN ""
ENDIF 

*|typedef struct _IP_ADAPTER_INFO { 
*|  struct _IP_ADAPTER_INFO* Next;         0:4 
*|  DWORD ComboIndex;                      4:4 
*|  char AdapterName[MAX_ADAPTER_NAME_LENGTH + 4];          8:260 
*|  char Description[MAX_ADAPTER_DESCRIPTION_LENGTH + 4]; 268:132 
*|  UINT AddressLength;                       400:4 
*|  BYTE Address[MAX_ADAPTER_ADDRESS_LENGTH]; 404:8 
*|  DWORD Index;                              412:4 
*|  UINT Type;                                416:4 
*|  UINT DhcpEnabled;                         420:2
*|  PIP_ADDR_STRING CurrentIpAddress;         ?? 4 byte pointer ?? 
*|  IP_ADDR_STRING IpAddressList;             433:15
*|  IP_ADDR_STRING GatewayList;               
*|  IP_ADDR_STRING DhcpServer;                
*|  BOOL HaveWins; 
*|  IP_ADDR_STRING PrimaryWinsServer; 
*|  IP_ADDR_STRING SecondaryWinsServer; 
*|  time_t LeaseObtained; 
*|  time_t LeaseExpires; 
*|} IP_ADAPTER_INFO, *PIP_ADAPTER_INFO; 

* wgcs: Experimentally found indices:
*   NetMask       449:15
*   Gateway       473:15
*   dhcp          513:15
*   Wins Server   557:15
*   

#DEFINE MAX_ADAPTER_NAME_LENGTH        256 
#DEFINE MAX_ADAPTER_DESCRIPTION_LENGTH 128 
#DEFINE MAX_ADAPTER_ADDRESS_LENGTH     8 

do case
  case lcItem='NONE'
    ? "Current IP address:", STRTRAN(SUBSTR(lcBuffer, 433,15), Chr(0),"") 
  case lcItem='IP'
    RETURN STRTRAN(SUBSTR(lcBuffer, 433,15), Chr(0),"") 
endcase

do case
  case lcItem='NONE'
    ? "Adapter name:",STRTRAN(SUBSTR(lcBuffer, 9, 260), Chr(0), "") 
  case lcItem='NAME'
    RETURN STRTRAN(SUBSTR(lcBuffer, 9, 260), Chr(0), "") 
endcase
do case
  case lcItem='NONE'
    ? "Description:",STRTRAN(SUBSTR(lcBuffer, 269, 132), Chr(0), "") 
  case lcItem='DESC'
    RETURN STRTRAN(SUBSTR(lcBuffer, 269, 132), Chr(0), "") 
endcase
do case
  case lcItem='NONE'
    ? "Net Mask:",STRTRAN(SUBSTR(lcBuffer, 449, 15), Chr(0), "") 
  case lcItem='NETMASK'
    RETURN STRTRAN(SUBSTR(lcBuffer, 449, 15), Chr(0), "") 
endcase
do case
  case lcItem='NONE'
    ? "Gateway:",STRTRAN(SUBSTR(lcBuffer, 473, 15), Chr(0), "") 
  case lcItem='GATEWAY'
    RETURN STRTRAN(SUBSTR(lcBuffer, 473, 15), Chr(0), "") 
endcase
do case
  case lcItem='NONE'
    ? "DHCP enabled:",asc(SUBSTR(lcBuffer, 421, 1))<>0
  case lcItem='DHCPON'
    RETURN asc(SUBSTR(lcBuffer, 421, 1))<>0
endcase
do case
  case lcItem='NONE'
    ? "DHCP Server:",STRTRAN(SUBSTR(lcBuffer, 513, 15), Chr(0), "") 
  case lcItem='DHCPSERVER'
    RETURN STRTRAN(SUBSTR(lcBuffer, 513, 15), Chr(0), "") 
endcase
do case
  case lcItem='NONE'
    ? "Wins Server:",STRTRAN(SUBSTR(lcBuffer, 557, 15), Chr(0), "") 
  case lcItem='WINSSERVER'
    RETURN STRTRAN(SUBSTR(lcBuffer, 557, 15), Chr(0), "") 
endcase

LOCAL lnAddrlen, lcAddress, ii, ch, lcMacAddr
lnAddrlen = Asc(SUBSTR(lcBuffer, 401, 1)) 
lcAddress = SUBSTR(lcBuffer, 405, lnAddrlen) 

do case
  case lcItem='NONE'
    ? "MAC Address: " 
    FOR ii=1 TO lnAddrlen 
        ?? Asc(SUBSTR(lcAddress, ii,1)), " " 
    ENDFOR 
  case lcItem='MAC'
    lcMacAddr = ''
    FOR ii=1 TO lnAddrlen 
      lcMacAddr = lcMacAddr + PadL(Int2Hex(Asc(SUBSTR(lcAddress, ii,1))),2,'0')+ " " 
    ENDFOR 
    RETURN Alltrim(lcMacAddr)
endcase
  
do case
  case lcItem='NONE'
    ? "Index?:", Asc(SUBSTR(lcBuffer, 413,1)) 
  case lcItem='INDEX'
    return Asc(SUBSTR(lcBuffer, 413,1)) 
endcase
do case
  case lcItem='NONE'
    ? "Type:", Asc(SUBSTR(lcBuffer, 417,1)) 
  case lcItem='TYPE'
    RETURN Asc(SUBSTR(lcBuffer, 417,1)) 
endcase

    * storing the buffer to a cursor, so you can review each byte of it 
    CREATE CURSOR cs (asc I, ch C(1)) 
    FOR ii=1 TO lnBufsize 
        ch = SUBSTR(lcBuffer, ii,1) 
        INSERT INTO cs VALUES (Asc(m.ch), m.ch) 
    ENDFOR 
    brow last fields x=recno(),asc,ch
    
RETURN lcBuffer
 
