Imports System.Data.SqlClient
Public Class usage_locate

    Private Sub usage_locate_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim objConnection As SqlConnection = New SqlConnection("Server='" + pServerName + "';Database='" + pComDbnm + "';User ID='" + pUserId + "';Password='" + pPassword + "';")
        Dim objAdapter As New SqlDataAdapter
        Dim objDataset As DataSet = New DataSet
        objAdapter.SelectCommand = New SqlCommand
        objAdapter.SelectCommand.Connection = objConnection
        objAdapter.SelectCommand.CommandText = "select DCNO,DATE1,ASSETCODE,USAGE_UNIT,EXP_LIFE,PRE_UNIT,NEW_UNIT,TOT_UNIT,DEPR,WORK_NO from bs_usage"
        objConnection.Open()
        objAdapter.Fill(objDataset, "bs_usage")
        objConnection.Close()
        DataGridView1.DataSource = objDataset
        DataGridView1.DataMember = "bs_usage"
        DataGridView1.Columns(0).HeaderText = "Document No"
        DataGridView1.Columns(1).HeaderText = "Date"
        DataGridView1.Columns(2).HeaderText = "Asset Code"
        DataGridView1.Columns(3).HeaderText = "Usage Unit"
        DataGridView1.Columns(4).HeaderText = "Estimated Usefull Life"
        DataGridView1.Columns(5).HeaderText = "Previous Unit"
        DataGridView1.Columns(6).HeaderText = "New Unit"
        DataGridView1.Columns(7).HeaderText = "Work Order No."
        DataGridView1.Columns(7).HeaderText = "Depreciation"
        If (My.Application.CommandLineArgs.Count > 0) Then
            Me.Icon = New Icon(My.Application.CommandLineArgs.Item(7).ToString())
        Else
            Me.Icon = New Icon(args(7)) ' Birendra : 7528 on 07/01/2013
        End If



    End Sub

    Private Sub DataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
    End Sub
    Private Sub DataGridView1_CellContentDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentDoubleClick
        Dim i As Integer
        i = DataGridView1.CurrentRow.Index
        USAGE.txtDCNO.Text = DataGridView1.Item(0, i).Value
        USAGE.TXTDATE.Text = DataGridView1.Item(1, i).Value
        USAGE.cmbAssCode.Text = DataGridView1.Item(2, i).Value
        USAGE.txtUnit.Text = DataGridView1.Item(3, i).Value
        USAGE.txtlife.Text = DataGridView1.Item(4, i).Value
        USAGE.txtPunit.Text = DataGridView1.Item(5, i).Value
        USAGE.txtNunit.Text = DataGridView1.Item(6, i).Value
        USAGE.cmbWorkOrder.Text = DataGridView1.Item(7, i).Value
        USAGE.txtDepr.Text = DataGridView1.Item(8, i).Value

        USAGE.Show()
        Me.Close()

    End Sub


    Private Sub DataGridView1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DataGridView1.KeyDown
        Dim i As Integer
        i = DataGridView1.CurrentRow.Index
        USAGE.txtDCNO.Text = DataGridView1.Item(0, i).Value
        USAGE.TXTDATE.Text = DataGridView1.Item(1, i).Value
        USAGE.cmbAssCode.Text = DataGridView1.Item(2, i).Value
        USAGE.txtUnit.Text = DataGridView1.Item(3, i).Value
        USAGE.txtlife.Text = DataGridView1.Item(4, i).Value
        USAGE.txtPunit.Text = DataGridView1.Item(5, i).Value
        USAGE.txtNunit.Text = DataGridView1.Item(6, i).Value
        USAGE.cmbWorkOrder.Text = DataGridView1.Item(7, i).Value
        USAGE.txtDepr.Text = DataGridView1.Item(8, i).Value
        USAGE.Show()
        Me.Close()
    End Sub

    Private Sub DataGridView1_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        DataGridView1_CellContentDoubleClick(sender, e)
    End Sub

    Private Sub usage_locate_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Leave
        '        USAGE.Show()
    End Sub

    Private Sub usage_locate_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        USAGE.Show()

    End Sub
End Class