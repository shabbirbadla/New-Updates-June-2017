Imports System.Data.SqlClient
Module Module1
    Public pPApplPID, pCompId As Integer
    Public pComDbnm, pServerName, pPApplRange, pUserId, pPassword, pAppUerName, pPApplText, pPApplName, pPApplCode As String
    Public frmDate1, todate1, sqlstr As String
    Public args() As String = {"1", "A011213", "LENOVO-PC\PROJECT", "sa", "sa1985", "^13048", "ADMIN", "D:\VudyogPRO\Bmp\Icon_VudyogPRO.ico", "Pro", "VudyogPRO.exe", "4764", "udPID4764DTM20111213125821"}
    
    Public Sub conn(ByVal str As String)
        Dim objConnection As SqlConnection = New SqlConnection("Server='" + pServerName + "';Database='" + pComDbnm + "';User ID=sa;Password=sa1985;")
        Dim objCommand As SqlCommand = New SqlCommand()
        objCommand.Connection = objConnection
        objCommand.CommandText = str
        objConnection.Open()
        objCommand.ExecuteNonQuery()
        objConnection.Close()
    End Sub

End Module
