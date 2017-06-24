* server adress
ftp_ip="111.22.333.44"
* username
ftp_user="username"
* password
ftp_pass="password"
* local directory
vcLocalDir="C:\TEST\"
* FTP server directory
vcServerDir="/docs/http/transfer/test/"
* file name
vcFilename="samples.vct"

 

#DEFINE GENERIC_READ     2147483648
#DEFINE GENERIC_WRITE     1073741824
#DEFINE INTERNET_INVALID_PORT_NUMBER 0
#DEFINE INTERNET_OPEN_TYPE_DIRECT  1
#DEFINE INTERNET_OPEN_TYPE_PROXY  3
#DEFINE INTERNET_DEFAULT_FTP_PORT  21
#DEFINE INTERNET_FLAG_ASYNC    268435456
#DEFINE INTERNET_FLAG_FROM_CACHE  16777216
#DEFINE INTERNET_FLAG_OFFLINE   16777216
#DEFINE INTERNET_FLAG_CACHE_IF_NET_FAIL 65536
#DEFINE INTERNET_OPEN_TYPE_PRECONFIG 0
#DEFINE FTP_TRANSFER_TYPE_ASCII   1
#DEFINE FTP_TRANSFER_TYPE_BINARY  2
#DEFINE INTERNET_SERVICE_FTP   1
#DEFINE INTERNET_SERVICE_GOPHER   2
#DEFINE INTERNET_SERVICE_HTTP   3
#DEFINE FILE_ATTRIBUTE_NORMAL   128
*
PUBLIC hOpen, hFtpSession
*
DECLARE INTEGER InternetOpen IN WININET.DLL;
 STRING  sAgent,;
 INTEGER lAccessType,;
 STRING  sProxyName,;
 STRING  sProxyBypass,;
 STRING  lFlags
*
DECLARE INTEGER InternetCloseHandle IN WININET.DLL;
 INTEGER hInet
*
DECLARE INTEGER InternetConnect IN WININET.DLL;
 INTEGER hInternetSession,;
 STRING  sServerName,;
 INTEGER nServerPort,;
 STRING  sUsername,;
 STRING  sPassword,;
 INTEGER lService,;
 INTEGER lFlags,;
 INTEGER lContext
*
DECLARE INTEGER FtpGetFile IN WININET.DLL;
 INTEGER hFtpSession,;
 STRING  lpszRemoteFile,;
 STRING  lpszNewFile,;
 INTEGER fFailIfExists,;
 INTEGER dwFlagsAndAttributes,;
 INTEGER dwFlags,;
 INTEGER dwContext
*
DECLARE INTEGER FtpDeleteFile IN WININET.DLL ;
 INTEGER nFile,;
 STRING  lpszFileName
*
DECLARE INTEGER InternetWriteFile IN WININET.DLL;
 INTEGER   hFile,;
 STRING  @ sBuffer,;
 INTEGER   lNumBytesToWrite,;
 INTEGER @ dwNumberOfBytesWritten
*
DECLARE INTEGER FtpOpenFile IN WININET.DLL;
 INTEGER hFtpSession,;
 STRING  sFileName,;
 INTEGER lAccess,;
 INTEGER lFlags,;
 INTEGER lContext

 
sAgent = "vfp6"
sProxyName = CHR(0)
sProxyBypass = CHR(0)
lFlags = 0
hOpen = InternetOpen (sAgent, INTERNET_OPEN_TYPE_DIRECT, sProxyName, sProxyBypass, lFlags)
IF hOpen = 0
 RETURN
ENDIF
ftp_ip = "141.150.196.241"
ftp_user = "globaludyog"
ftp_pass = "smadSMAD"
hFtpSession = InternetConnect (hOpen, ftp_ip, INTERNET_INVALID_PORT_NUMBER, ftp_user, ftp_pass, INTERNET_SERVICE_FTP, 0, 0)
IF hFtpSession = 0
 = InternetCloseHandle (hOpen)
 RETURN
ENDIF
lpszRemoteFile = "index*.htm"	&&vcServerDir+vcFilename
lpszNewFile    = "c:\index*.htm" &&vcLocalDir+vcFilename
fFailIfExists  = 0
dwContext      = 0
set step on
lnResult = FtpGetFile (hFtpSession, lpszRemoteFile, lpszNewFile, fFailIfExists, FILE_ATTRIBUTE_NORMAL, FTP_TRANSFER_TYPE_ASCII, dwContext)
= InternetCloseHandle (hFtpSession)
= InternetCloseHandle (hOpen)
RETURN



FUNCTION connect2ftp (strHost, strUser, strPwd)
hOpen = InternetOpen ("vfp", 1, 0, 0, 0)
IF hOpen = 0
 ? "Sem acceso a WinInet.Dll"
 RETURN .F.
ENDIF
hFtpSession = InternetConnect (hOpen, strHost, 0, strUser, strPwd, 1,0,0)
IF hFtpSession = 0
 = InternetCloseHandle (hOpen)
 RETURN .F.
ENDIF
RETURN .T.

FUNCTION local2ftp (hConnect, lcSource, lcTarget)
hSource = FOPEN (lcSource)
IF (hSource = -1)
 RETURN -1
ENDIF
hTarget = FtpOpenFile(hConnect, lcTarget, GENERIC_WRITE, 2, 0)
IF hTarget = 0
 = FCLOSE (hSource)
 RETURN -2
ENDIF
lnBytesWritten = 0
lnChunkSize =  128
DO WHILE !FEOF(hSource)
 lcBuffer = FREAD (hSource, lnChunkSize)
 lnLength = Len(lcBuffer)
 IF lnLength<>0
  IF InternetWriteFile (hTarget, @lcBuffer, lnLength, @lnLength) = 1
   lnBytesWritten = lnBytesWritten + lnLength
  ELSE
   EXIT
  ENDIF
 ELSE
  EXIT
 ENDIF
ENDDO
= InternetCloseHandle (hTarget)
= FCLOSE (hSource)
RETURN lnBytesWritten

