Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Public Class Form2

    Private Sub Form2_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        'CommenceReport.Close()
        CommenceReport.ToDate.CustomFormat = "dd/MM/yyyy"
        CommenceReport.ToDate.Format = DateTimePickerFormat.Custom
        CommenceReport.Refresh()
        CommenceReport.Show()

    End Sub

    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CommenceReport.Hide()
        pPApplPID = 0
        pCompId = Convert.ToInt16(args(0))
        pComDbnm = args(1)
        pServerName = args(2)
        pUserId = args(3)
        pPassword = args(4)
        pPApplRange = args(5)
        pAppUerName = args(6)
        pPApplText = args(8).Replace("<*#*>", " ")
        pPApplName = args(9)
        pPApplPID = Convert.ToInt16(args(10))
        pPApplCode = args(11)
        Dim myConnectionInfo As New ConnectionInfo()
        myConnectionInfo.ServerName = pServerName
        myConnectionInfo.DatabaseName = pComDbnm
        myConnectionInfo.UserID = pUserId
        myConnectionInfo.Password = pPassword
        setDBLOGONforREPORT(myConnectionInfo)
        Label2.Text = CommenceReport.Frmdate.Text
        Label4.Text = CommenceReport.ToDate.Text
    End Sub
    Private Sub setDBLOGONforREPORT(ByVal myconnectioninfo As ConnectionInfo)
        Dim mytableloginfos As New TableLogOnInfos()
        mytableloginfos = CrystalReportViewer1.LogOnInfo
        For Each myTableLogOnInfo As TableLogOnInfo In mytableloginfos
            myTableLogOnInfo.ConnectionInfo = myconnectioninfo
        Next
    End Sub

   
End Class