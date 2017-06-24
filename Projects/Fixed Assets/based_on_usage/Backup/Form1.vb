Imports System.Data.SqlClient
Public Class Form1

    Private Sub Form1_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        CommenceReport.Close()
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        pPApplPID = 0
        'this.pPara = args;
        'this.pFrmCaption = "Grade Master";
        pCompId = Convert.ToInt16(args(0))
        pComDbnm = args(1)
        pServerName = args(2)
        pUserId = args(3)
        pPassword = args(4)
        pPApplRange = args(5)
        pAppUerName = args(6)
        'Icon MainIcon = new System.Drawing.Icon(args[7].Replace("<*#*>", " "));
        ' pFrmIcon = MainIcon;
        pPApplText = args(8).Replace("<*#*>", " ")
        pPApplName = args(9)
        pPApplPID = Convert.ToInt16(args(10))
        pPApplCode = args(11)

        Dim objConnection As SqlConnection = New SqlConnection("Server='" + pServerName + "';Database='" + pComDbnm + "';User ID=sa;Password=sa1985;")
        Dim reader As SqlDataReader
        Dim command As SqlCommand = New SqlCommand
        command.Connection = objConnection
        command.CommandText = "select * from it_mast WHERE TYPE='Machinery/Stores              '"
        objConnection.Open()

        reader = command.ExecuteReader()

        If (reader.HasRows) Then

            While (reader.Read())

                cmbAsset.Items.Add(reader.GetString(0))
            End While

        End If
        txtDate.CustomFormat = "dd/MM/yyyy"
        txtDate.Format = DateTimePickerFormat.Custom
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If txtLessVal.Text = "" Then
            txtLessVal.Text = 0
        End If
        sqlstr = "INSERT INTO addvl (AssetCode,PurchaseDate,PurchaseValue,AddValue,LessValue,CommenceDate,Remarks)VALUES('" + Trim(cmbAsset.Text) + "','" + txtDate.Text + "'," + txtBookVal.Text + "," + txtAddVal.Text + "," + txtLessVal.Text + ",'" + txtCDate.Text + "','" & txtRemark.Text & "')"
        conn(sqlstr)
        MsgBox("Entery Save", MsgBoxStyle.OkOnly, "Save")
        
    End Sub

    Private Sub Locate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Locate.Click
        Locat.Show()
        Me.Hide()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        sqlstr = "delete from addvl where Vno= '" + txtDcNo.Text + "'"
        conn(sqlstr)
        MsgBox("Record Deleted", MsgBoxStyle.OkOnly, "Delete")
        
    End Sub

    Private Sub btnReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReport.Click
        CommenceReport.Show()
    End Sub

    Private Sub cmbAsset_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAsset.SelectedIndexChanged
        Dim objConnection As SqlConnection = New SqlConnection("Server='" + pServerName + "';Database='" + pComDbnm + "';User ID=sa;Password=sa1985;")
        Dim reader As SqlDataReader
        Dim command As SqlCommand = New SqlCommand
        command.Connection = objConnection
        command.CommandText = "select * from it_mast WHERE it_name='" + cmbAsset.Text + "'"
        objConnection.Open()

        reader = command.ExecuteReader()
        If (reader.HasRows) Then
            While (reader.Read())
                txtBookVal.Text = reader.GetDecimal(56)
                txtCDate.Text = reader.GetDateTime(55)
            End While

        End If
    End Sub
End Class
