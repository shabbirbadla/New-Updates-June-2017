Public Sub SendMessage(strTo As String, strFrom As String, strBCC As String, strAttachment2 As String)
    'Send using the Port on a SMTP server
    Dim attList() As String
    Dim item As Integer
    Dim iMsg 
    Dim iConf 
    Dim Flds 
    Dim strHTML
    Dim stBody1 As String, stBody2 As String, stSubject As String
    Set iMsg = CreateObject("CDO.Message")
    Set iConf = CreateObject("CDO.configuration")
    Set Flds = iConf.Fields
    'Set lfsFile1 = CreateObject("Scripting.FileSystemObject")
    With Flds
        .item("http://schemas.microsoft.com/cdo/configuration/sendusing") = 2 'cdoSendUsingPort
        .item("http://schemas.microsoft.com/cdo/configuration/smtpserverport") = 25
        .item("http://schemas.microsoft.com/cdo/configuration/smtpserver") = "YOUR.MAIL.SERVER"
        'Use SSL to connect to the SMTP server:
        .item("http://schemas.microsoft.com/cdo/configuration/smtpconnectiontimeout") = 10
        .Update
    End With

    If InStr(strAttachment2, ";") Then attList = Split(strAttachment2, ";")

    With iMsg
        Set .Configuration = iConf
        .To = strTo
        '.CC = strCC
        .FROM = strFrom
        .BCC = strBCC
        '.Subject = strSubject
        '.HTMLBody = strHTML
        '.TextBody = strMessage
        '.Nocoverpage: True
        '.confirmsend: true
        If InStr(strAttachment2, ";") Then
            For item = 1 To UBound(attList)
                .addAttachment (attList(item))
            Next
        Else
            If Not IsNull(strAttachment2) And Len(strAttachment2) > 0 Then .addAttachment strAttachment2
        End If
        .Send
    End With
    
    ' cleanup of variables
    Set iMsg = Nothing
    Set iConf = Nothing
    Set Flds = Nothing
End Sub