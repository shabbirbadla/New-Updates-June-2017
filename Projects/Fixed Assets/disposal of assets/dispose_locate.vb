Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Public Class dispose_locate

    Private Sub dispose_locate_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        Disposal.Show()
        Me.Close()
    End Sub

    Private Sub dispose_locate_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim objConnection As SqlConnection = New SqlConnection("Server='" + pServerName + "';Database='" + pComDbnm + "';User ID='" + pUserId + "';Password='" + pPassword + "';")
        Dim objAdapter As New SqlDataAdapter
        Dim objDataset As DataSet = New DataSet
        objAdapter.SelectCommand = New SqlCommand
        objAdapter.SelectCommand.Connection = objConnection
        objAdapter.SelectCommand.CommandText = "select VoucherNo,convert(datetime,DisposalDate,103),Assetcode,PurchaseVal,Depreciation,ValueSold,Narration from assetdisposal"
        objConnection.Open()
        objAdapter.Fill(objDataset, "assetdisposal")
        objConnection.Close()
        DataGridView1.DataSource = objDataset
        DataGridView1.DataMember = "assetdisposal"
        DataGridView1.Columns(0).HeaderText = "Document No"
        DataGridView1.Columns(1).HeaderText = "Date"
        DataGridView1.Columns(2).HeaderText = "Asset Code"
        DataGridView1.Columns(3).HeaderText = "Purchase Value"
        DataGridView1.Columns(4).HeaderText = "Depreciation"
        DataGridView1.Columns(5).HeaderText = "Value Sold"
        DataGridView1.Columns(6).HeaderText = "Narration"
        If (My.Application.CommandLineArgs.Count > 0) Then
            Me.Icon = New Icon(My.Application.CommandLineArgs.Item(7).ToString())
        Else
            Me.Icon = New Icon(args(7)) ' Birendra : 7528 on 07/01/2013
        End If


    End Sub

    Private Sub DataGridView1_CellClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        DataGridView1.CurrentRow.Selected = True
    End Sub

    Private Sub DataGridView1_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        Dim i As Integer
        i = DataGridView1.CurrentRow.Index
        Disposal.txtDCNO.Text = DataGridView1.Item(0, i).Value
        Disposal.TXTDATE.Text = DataGridView1.Item(1, i).Value
        Disposal.cmbAssCode.Text = DataGridView1.Item(2, i).Value
        Disposal.txtPurch.Text = DataGridView1.Item(3, i).Value
        Disposal.txtDepr.Text = DataGridView1.Item(4, i).Value
        Disposal.txtValSold.Text = DataGridView1.Item(5, i).Value
        Disposal.txtNarr.Text = DataGridView1.Item(6, i).Value
        Disposal.Show()
        Me.Close()
    End Sub

    Private Sub DataGridView1_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DataGridView1.KeyDown
        Dim i As Integer
        i = DataGridView1.CurrentRow.Index
        Disposal.txtDCNO.Text = DataGridView1.Item(0, i).Value
        Disposal.TXTDATE.Text = DataGridView1.Item(1, i).Value
        Disposal.cmbAssCode.Text = DataGridView1.Item(2, i).Value
        Disposal.txtPurch.Text = DataGridView1.Item(3, i).Value
        Disposal.txtDepr.Text = DataGridView1.Item(4, i).Value
        Disposal.txtValSold.Text = DataGridView1.Item(5, i).Value
        Disposal.txtNarr.Text = DataGridView1.Item(6, i).Value
        Disposal.Show()
        Me.Close()
    End Sub
End Class