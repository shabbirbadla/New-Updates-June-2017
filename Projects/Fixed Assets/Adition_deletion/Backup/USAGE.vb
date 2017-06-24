Imports System.Data.SqlClient
Public Class USAGE
    Public it_pval, bs_pval, total_unit, salvage, estimate As Integer
    Public depr As Double
    Private Sub USAGE_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        pPApplPID = 0
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
                cmbAssCode.Items.Add(reader.GetString(0))
            End While
        End If
    End Sub

    Private Sub cmbAssCode_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAssCode.SelectedIndexChanged
        Dim objConnection As SqlConnection = New SqlConnection("Server='" + pServerName + "';Database='" + pComDbnm + "';User ID=sa;Password=sa1985;")
        Dim reader As SqlDataReader
        Dim command As SqlCommand = New SqlCommand
        command.Connection = objConnection
        command.CommandText = "select * from it_mast WHERE it_name='" + cmbAssCode.Text + "'"
        objConnection.Open()

        reader = command.ExecuteReader()

        If (reader.HasRows) Then
            While (reader.Read())
                txtUnit.Text = reader.GetString(57)
                it_pval = reader.GetDecimal(56)
                salvage = reader.GetDecimal(58)
                estimate = reader.GetDecimal(59)
                txtlife.Text = estimate
            End While
        End If
        objConnection.Close()

        command.CommandText = "select * from bs_usage WHERE assetcode='" + cmbAssCode.Text + "'"
        objConnection.Open()
        reader = command.ExecuteReader()
        If (reader.HasRows) Then
            While (reader.Read())
                txtPunit.Text = reader.GetDecimal(7)
            End While
        End If
        If (reader.HasRows = False) Then
            txtPunit.Text = 0
        End If
    End Sub

    Private Sub btnsave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnsave.Click
        depr = (((it_pval - salvage) / estimate) * txtNunit.Text)
        Call calculate_usage()
      
    End Sub
    Sub calculate_usage()
        total_unit = Val(txtPunit.Text) + Val(txtNunit.Text)
        sqlstr = "insert into bs_usage (U_DATE,ASSETCODE,USAGE_UNIT,EXP_LIFE,PRE_UNIT,NEW_UNIT,TOT_UNIT,DEPR)values('" + TXTDATE.Text + "','" + cmbAssCode.Text + "','" + txtUnit.Text + "'," + txtlife.Text + "," + txtPunit.Text + "," + txtNunit.Text + "," & total_unit & "," & depr & ")"
        conn(sqlstr)
        MsgBox("Record Save", MsgBoxStyle.OkOnly, "Save")
    End Sub
End Class