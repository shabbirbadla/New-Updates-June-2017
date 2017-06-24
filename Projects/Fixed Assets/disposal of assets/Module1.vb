Imports System.Data.SqlClient
Module Module1
    Public pPApplPID, pCompId As Integer
    Public pComDbnm, pServerName, pPApplRange, pUserId, pPassword, pAppUerName, pPApplText, pPApplName, pPApplCode As String
    Public frmDate1, todate1, sqlstr, cAppName, cAppPId As String
    'Public args() As String = {"1", "B121213", "Desktop7", "sa", "sa@1985", "^13048", "ADMIN", "D:\testing\VudyogSDK\Bmp\Icon_VudyogPRO.ico", "SDK", "VudyogSDK.exe", "4764", "udPID4764DTM20111213125821"}
    '    Public args() As String = {"1", "B121213", "Desktop7", "sa", "sa@1985", "^13048", "ADMIN", "D:\Usquare\Bmp\Icon_10USquare.ico", "Usquare", "Usquare10.exe", "4764", "udPID4764DTM20111213125821"}
    Public args() As String = {"1", "B081213", "Desktop7", "sa", "sa@1985", "^13048", "ADMIN", "D:\Usquare\Bmp\Icon_10USquare.ico", "Usquare", "Usquare10.exe", "4764", "udPID4764DTM20111213125821"}

    
    Public Sub conn(ByVal str As String)
        Dim objConnection As SqlConnection = New SqlConnection("Server='" + pServerName + "';Database='" + pComDbnm + "';User ID='" + pUserId + "';Password='" + pPassword + "';")
        Dim objCommand As SqlCommand = New SqlCommand()
        objCommand.Connection = objConnection
        objCommand.CommandText = str
        objConnection.Open()
        objCommand.ExecuteNonQuery()
        objConnection.Close()
    End Sub

End Module
