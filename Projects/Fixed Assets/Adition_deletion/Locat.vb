Imports System.Data
Imports System.Data.SqlClient
Public Class Locat

    Private Sub Locate_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim objConnection As SqlConnection = New SqlConnection("Server='" + pServerName + "';Database='" + pComDbnm + "';User ID='" + pUserId + "';Password='" + pPassword + "';")
        Dim objAdapter As New SqlDataAdapter
        Dim objDataset As DataSet = New DataSet
        objAdapter.SelectCommand = New SqlCommand
        objAdapter.SelectCommand.Connection = objConnection
        objAdapter.SelectCommand.CommandText = "select * from addvl"
        objConnection.Open()
        objAdapter.Fill(objDataset, "addvl")
        objConnection.Close()
        DataGridView1.DataSource = objDataset
        DataGridView1.DataMember = "addvl"
        DataGridView1.Columns(0).HeaderText = "Document No"
        DataGridView1.Columns(1).HeaderText = "Asset Code"
        DataGridView1.Columns(2).HeaderText = "Asset Date"
        DataGridView1.Columns(3).HeaderText = "Book value"
        DataGridView1.Columns(4).HeaderText = "Add value"
        DataGridView1.Columns(5).HeaderText = "Less Value"
        DataGridView1.Columns(6).HeaderText = "Commencement Date"
        DataGridView1.Columns(7).HeaderText = "Remark"

        If (My.Application.CommandLineArgs.Count > 0) Then
            Me.Icon = New Icon(My.Application.CommandLineArgs.Item(7).ToString())
        Else
            Me.Icon = New Icon(args(7)) ' Birendra : 7528 on 07/01/2013
        End If
    End Sub

    Private Sub DataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick, DataGridView1.CellDoubleClick
        DataGridView1.CurrentRow.Selected = True
        Me.DataGridView1_CellContentDoubleClick(sender, e)
    End Sub

    Private Sub DataGridView1_CellContentDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentDoubleClick
        Dim i As Integer
        i = DataGridView1.CurrentRow.Index
        AdditionDeletion.txtDcNo.Text = DataGridView1.Item(0, i).Value
        AdditionDeletion.cmbAsset.Text = DataGridView1.Item(1, i).Value
        AdditionDeletion.txtDate.Text = DataGridView1.Item(2, i).Value
        AdditionDeletion.txtBookVal.Text = DataGridView1.Item(3, i).Value
        AdditionDeletion.txtAddVal.Text = DataGridView1.Item(4, i).Value
        AdditionDeletion.txtLessVal.Text = DataGridView1.Item(5, i).Value
        AdditionDeletion.txtCDate.Text = DataGridView1.Item(6, i).Value
        AdditionDeletion.txtRemark.Text = DataGridView1.Item(7, i).Value
        AdditionDeletion.Show()
        Me.Close()

    End Sub


    Private Sub DataGridView1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DataGridView1.KeyDown
        'Birendra : Bug-7528 on 07/01/2013 :Start: commented
        'Dim i As Integer
        'i = DataGridView1.CurrentRow.Index
        'AdditionDeletion.txtDcNo.Text = DataGridView1.Item(0, i).Value
        'AdditionDeletion.cmbAsset.Text = DataGridView1.Item(1, i).Value
        'AdditionDeletion.txtDate.Text = DataGridView1.Item(2, i).Value
        'AdditionDeletion.txtBookVal.Text = DataGridView1.Item(3, i).Value
        'AdditionDeletion.txtAddVal.Text = DataGridView1.Item(4, i).Value
        'AdditionDeletion.txtLessVal.Text = DataGridView1.Item(5, i).Value
        'AdditionDeletion.txtCDate.Text = DataGridView1.Item(6, i).Value
        'AdditionDeletion.txtRemark.Text = DataGridView1.Item(7, i).Value
        'AdditionDeletion.Show()
        'Me.Close()
        'Birendra : Bug-7828 on 07/01/2013 :End: commented

    End Sub
End Class