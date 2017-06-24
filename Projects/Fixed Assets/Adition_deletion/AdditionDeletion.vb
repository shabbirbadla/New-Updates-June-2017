Imports System.Data.SqlClient
Imports System.Windows.Forms
Imports System.Globalization
Imports System.Threading



Public Class AdditionDeletion
    Public enunit As Integer

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Birendra : Bug-7528 on 02/01/2013 :Start:

        Dim ci As CultureInfo
        ci = New CultureInfo("en-US")
        ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy"
        Thread.CurrentThread.CurrentCulture = ci

        If (My.Application.CommandLineArgs.Count > 0) Then
            pPApplPID = 0
            'pCompId = Convert.ToInt16(My.Application.CommandLineArgs.Item(0).ToString())
            'pComDbnm = My.Application.CommandLineArgs.Item(1).ToString()
            'pServerName = My.Application.CommandLineArgs.Item(2).ToString()
            'pUserId = My.Application.CommandLineArgs.Item(3).ToString()
            'pPassword = My.Application.CommandLineArgs.Item(4).ToString()
            'pPApplRange = My.Application.CommandLineArgs.Item(5).ToString()
            'pAppUerName = My.Application.CommandLineArgs.Item(6).ToString()
            'pPApplText = My.Application.CommandLineArgs.Item(8).Replace("<*#*>", " ")
            'pPApplName = My.Application.CommandLineArgs.Item(9).ToString()
            'pPApplPID = Convert.ToInt16(My.Application.CommandLineArgs.Item(10).ToString())
            'pPApplCode = My.Application.CommandLineArgs.Item(11).ToString()
            'Me.Icon = New Icon(My.Application.CommandLineArgs.Item(7).ToString())

            pCompId = Convert.ToInt16(My.Application.CommandLineArgs.Item(0).ToString())
            pComDbnm = My.Application.CommandLineArgs.Item(1).ToString()
            pServerName = My.Application.CommandLineArgs.Item(2).ToString()
            pUserId = My.Application.CommandLineArgs.Item(3).ToString()
            pPassword = My.Application.CommandLineArgs.Item(4).ToString()
            pPApplRange = My.Application.CommandLineArgs.Item(5).ToString()
            pAppUerName = My.Application.CommandLineArgs.Item(6).ToString()
            pPApplText = My.Application.CommandLineArgs.Item(8).Replace("<*#*>", " ")
            pPApplName = My.Application.CommandLineArgs.Item(8).Replace("<*#*>", " ")
            pPApplPID = Convert.ToInt16(My.Application.CommandLineArgs.Item(10).ToString())
            pPApplCode = My.Application.CommandLineArgs.Item(11).ToString()
            Me.Icon = New Icon(My.Application.CommandLineArgs.Item(7).Replace("<*#*>", " "))
        Else
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
            pPApplPID = Convert.ToInt32(args(10))
            pPApplCode = args(11)
            Me.Icon = New Icon(args(7))
        End If
        'Birendra : Bug-7528 on 02/01/2013 :End:

        'pPApplPID = 0
        'pCompId = Convert.ToInt16(args(0))
        'pComDbnm = args(1)
        'pServerName = args(2)
        'pUserId = args(3)
        'pPassword = args(4)
        'pPApplRange = args(5)
        'pAppUerName = args(6)
        'pPApplText = args(8).Replace("<*#*>", " ")
        'pPApplName = args(9)
        'pPApplPID = Convert.ToInt16(args(10))
        'pPApplCode = args(11)
        mFormload()
        mInsertProcessIdRecord()
    End Sub


    Private Sub cmbAsset_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAsset.SelectedIndexChanged
        Dim objConnection As SqlConnection = New SqlConnection("Server='" + pServerName + "';Database='" + pComDbnm + "';User ID='" + pUserId + "';Password='" + pPassword + "';")
        Dim reader As SqlDataReader
        Dim command As SqlCommand = New SqlCommand
        command.Connection = objConnection
        command.CommandText = "Set DateFormat dmy select COMNCDT,PVALUE from it_mast WHERE it_name='" + cmbAsset.Text + "'"
        objConnection.Open()

        reader = command.ExecuteReader()
        If (reader.HasRows) Then
            While (reader.Read())
                txtCDate.Text = reader.GetDateTime(0)
                txtBookVal.Text = reader.GetDecimal(1)
            End While

        End If
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        enunit = 0
        txtDcNo.Clear()
        cmbAsset.Text = ""
        cmbAsset.Enabled = True
        'cmbAsset.ResetText()
        txtDate.Text = Now
        txtDate.Enabled = True
        txtBookVal.Clear()
        txtAddVal.Clear()
        txtLessVal.Clear()
        txtLessVal.Enabled = True
        txtCDate.Text = ""
        txtRemark.Clear()
        txtRemark.Enabled = True
        txtAddVal.Enabled = True
        ToolStripButton1.Enabled = True
        btnCancel.Enabled = True
        btnFirst.Enabled = False
        btnForward.Enabled = False
        btnLast.Enabled = False
        btnBack.Enabled = False
        btnNew.Enabled = False
        btnEdit.Enabled = False
        BtnDelete.Enabled = False
        btnLocate.Enabled = False
        If (cmbAsset.Items.Count = 0) Then


            Dim objConnection As SqlConnection = New SqlConnection("Server='" + pServerName + "';Database='" + pComDbnm + "';User ID='" + pUserId + "';Password='" + pPassword + "';")
            Dim reader As SqlDataReader
            Dim command As SqlCommand = New SqlCommand
            command.Connection = objConnection
            command.CommandText = "select IT_NAME from it_mast WHERE TYPE='Machinery/Stores              ' and methodnm<>'based on usage'"
            objConnection.Open()

            reader = command.ExecuteReader()
            If (reader.HasRows) Then
                While (reader.Read())
                    cmbAsset.Items.Add(reader.GetString(0))
                End While

            End If
        End If
        txtCDate.CustomFormat = "dd/MM/yyyy"
        txtDate.CustomFormat = "dd/MM/yyyy"
        txtDate.Format = DateTimePickerFormat.Custom
        cmbAsset.Focus()
    End Sub


    Private Sub txtAddVal_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAddVal.TextChanged
        If Val(txtAddVal.Text) > 0 Then
            txtLessVal.Enabled = False
        End If
    End Sub

    Private Sub txtLessVal_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtLessVal.TextChanged
        If Val(txtLessVal.Text) > 0 Then
            txtAddVal.Enabled = False
        End If
    End Sub


    Private Sub btnForward_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnForward.Click
        Dim vnum, cnt1, cnt2 As Integer
        Dim objConnection As SqlConnection = New SqlConnection("Server='" + pServerName + "';Database='" + pComDbnm + "';User ID='" + pUserId + "';Password='" + pPassword + "';")
        Dim reader As SqlDataReader
        Dim command As SqlCommand = New SqlCommand
        command.Connection = objConnection
        command.CommandText = "select Vno from addvl order by vno desc"
        objConnection.Open()
        reader = command.ExecuteReader()
        If (reader.Read()) Then
            cnt2 = reader.GetInt32(0)
        End If
        objConnection.Close()
        vnum = Val(txtDcNo.Text) + 1
        cnt1 = 0
        Do Until cnt1 > 0
            command.CommandText = "Set DateFormat dmy select Vno,AssetCode,PurchaseDate,PurchaseValue,AddValue,LessValue,CommenceDate,Remarks from addvl where Vno=" & vnum & ""
            objConnection.Open()
            reader = command.ExecuteReader()
            'cnt2 = reader.FieldCount
            If (reader.Read) Then
                'While (reader.Read())
                'reader.Read()
                txtDcNo.Text = reader.GetInt32(0)
                cmbAsset.Text = reader.GetString(1)
                txtDate.Text = reader.GetString(2)
                txtBookVal.Text = reader.GetDecimal(3)
                txtAddVal.Text = reader.GetDecimal(4)
                txtLessVal.Text = reader.GetDecimal(5)
                'txtCDate.Text = reader.GetString(6)            'Commented by Shrikant S. on 08/10/2014 for Bug-23861
                txtCDate.Text = reader.GetDateTime(reader.GetOrdinal("CommenceDate")).ToString("dd/MM/yyyy")             'Added by Shrikant S. on 08/10/2014 for Bug-23861
                txtRemark.Text = reader.GetString(7)
                cnt1 = 1
                objConnection.Close()
                'Birendra : Bug-7528 on 06/01/2013 :Start:
                btnBack.Enabled = True
                btnFirst.Enabled = True

                If vnum = cnt2 Then
                    btnForward.Enabled = False
                    btnLast.Enabled = False
                    btnBack.Enabled = True
                    btnFirst.Enabled = True
                End If
                'Birendra : Bug-7528 on 06/01/2013 :end:

            ElseIf (vnum > cnt2) Then
                'MsgBox("last record")
                Exit Do
            Else
                vnum = vnum + 1
                objConnection.Close()
            End If
        Loop
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Dim vnum, cnt1, cnt2 As Integer
        Dim objConnection As SqlConnection = New SqlConnection("Server='" + pServerName + "';Database='" + pComDbnm + "';User ID='" + pUserId + "';Password='" + pPassword + "';")
        Dim reader As SqlDataReader
        Dim command As SqlCommand = New SqlCommand
        command.Connection = objConnection
        command.CommandText = "select Vno from addvl order by vno asc"
        objConnection.Open()
        reader = command.ExecuteReader()
        If (reader.Read()) Then
            cnt2 = reader.GetInt32(0)
        End If
        objConnection.Close()
        vnum = Val(txtDcNo.Text) - 1
        cnt1 = 0
        Do Until cnt1 > 0
            command.CommandText = "select Vno,AssetCode,PurchaseDate,PurchaseValue,AddValue,LessValue,CommenceDate,Remarks from addvl where Vno=" & vnum & ""
            objConnection.Open()
            reader = command.ExecuteReader()
            If (reader.Read = True) Then
                txtDcNo.Text = reader.GetInt32(0)
                cmbAsset.Text = reader.GetString(1)
                txtDate.Text = reader.GetString(2)
                txtBookVal.Text = reader.GetDecimal(3)
                txtAddVal.Text = reader.GetDecimal(4)
                txtLessVal.Text = reader.GetDecimal(5)
                'txtCDate.Text = reader.GetDateTime(6)                  'Added by Shrikant S. on 08/10/2014 for Bug-23861
                txtCDate.Text = reader.GetDateTime(reader.GetOrdinal("CommenceDate")).ToString("dd/MM/yyyy")             'Added by Shrikant S. on 08/10/2014 for Bug-23861
                txtRemark.Text = reader.GetString(7)
                cnt1 = 1
                objConnection.Close()
                'Birendra : Bug-7528 on 06/01/2013 :Start:
                btnForward.Enabled = True
                btnLast.Enabled = True
                If vnum = cnt2 Then
                    'Birendra : Bug-7528 on 06/01/2013 :Start:
                    btnForward.Enabled = True
                    btnLast.Enabled = True
                    btnFirst.Enabled = False
                    btnBack.Enabled = False
                    'Birendra : Bug-7528 on 06/01/2013 :end:

                End If
                'Birendra : Bug-7528 on 06/01/2013 :end:

            ElseIf (vnum < cnt2) Then
                'MsgBox("First record")
                Exit Do
            Else
                vnum = vnum - 1
                objConnection.Close()
                'Birendra : Bug-7528 on 06/01/2013 :Start:
                btnForward.Enabled = True
                btnLast.Enabled = True
                btnFirst.Enabled = False
                btnBack.Enabled = False
                'Birendra : Bug-7528 on 06/01/2013 :end:
            End If
        Loop
    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        Dim objConnection As SqlConnection = New SqlConnection("Server='" + pServerName + "';Database='" + pComDbnm + "';User ID='" + pUserId + "';Password='" + pPassword + "';")
        Dim reader As SqlDataReader
        Dim command As SqlCommand = New SqlCommand
        command.Connection = objConnection
        command.CommandText = "Set DateFormat dmy select Vno,AssetCode,PurchaseDate,PurchaseValue,AddValue,LessValue,CommenceDate,Remarks from addvl order by vno asc"
        objConnection.Open()

        reader = command.ExecuteReader()
        If (reader.HasRows) Then
            reader.Read()
            txtDcNo.Text = reader.GetInt32(0)
            cmbAsset.Text = reader.GetString(1)
            txtDate.Text = reader.GetString(2)
            txtBookVal.Text = reader.GetDecimal(3)
            txtAddVal.Text = reader.GetDecimal(4)
            txtLessVal.Text = reader.GetDecimal(5)
            'txtCDate.Text = reader.GetDateTime(6)              'Commented by Shrikant S. on 08/10/2014 for Bug-23861
            txtCDate.Text = reader.GetDateTime(reader.GetOrdinal("CommenceDate")).ToString("dd/MM/yyyy")             'Added by Shrikant S. on 08/10/2014 for Bug-23861
            txtRemark.Text = reader.GetString(7)
        Else
            '  MsgBox("Last Record", MsgBoxStyle.OkOnly, "Last Record")
        End If
        'Birendra : Bug-7528 on 06/01/2013 :Start:
        btnForward.Enabled = True
        btnLast.Enabled = True
        btnFirst.Enabled = False
        btnBack.Enabled = False
        'Birendra : Bug-7528 on 06/01/2013 :end:

    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        Dim objConnection As SqlConnection = New SqlConnection("Server='" + pServerName + "';Database='" + pComDbnm + "';User ID='" + pUserId + "';Password='" + pPassword + "';")
        Dim reader As SqlDataReader
        Dim command As SqlCommand = New SqlCommand
        command.Connection = objConnection
        command.CommandText = "Set DateFormat dmy select Vno,AssetCode,PurchaseDate,PurchaseValue,AddValue,LessValue,CommenceDate,Remarks from addvl order by vno desc"
        objConnection.Open()

        reader = command.ExecuteReader()
        If (reader.HasRows) Then
            reader.Read()
            txtDcNo.Text = reader.GetInt32(0)
            cmbAsset.Text = reader.GetString(1)
            txtDate.Text = reader.GetString(2)
            txtBookVal.Text = reader.GetDecimal(3)
            txtAddVal.Text = reader.GetDecimal(4)
            txtLessVal.Text = reader.GetDecimal(5)
            'txtCDate.Text = reader.GetDateTime(6)                  'Commented by Shrikant S. on 08/10/2014 for Bug-23861
            txtCDate.Text = reader.GetDateTime(reader.GetOrdinal("CommenceDate")).ToString("dd/MM/yyyy")             'Added by Shrikant S. on 08/10/2014 for Bug-23861
            txtRemark.Text = reader.GetString(7)
        Else
            '            MsgBox("Last Record", MsgBoxStyle.OkOnly, "Last Record")
        End If
        'Birendra : Bug-7528 on 06/01/2013 :Start:
        btnForward.Enabled = False
        btnLast.Enabled = False
        btnBack.Enabled = True
        btnFirst.Enabled = True
        'Birendra : Bug-7528 on 06/01/2013 :end:


    End Sub

    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton1.Click
        'Birendra : bug-7528 on 04/01/2013 :Start:
        If cmbAsset.Text.Length = 0 Then
            ErrorProvider1.SetError(cmbAsset, "Asset Can Not Be Empty!!!")
            cmbAsset.Focus()
            Return
        End If
        'Birendra : bug-7528 on 04/01/2013 :End:
        If Val(txtAddVal.Text) < 0 Then
            ErrorProvider1.SetError(txtAddVal, "Add Value Can Not Be Negative!!!")
            txtAddVal.Focus()
            Return
        End If
        If Val(txtLessVal.Text) < 0 Then
            ErrorProvider1.SetError(txtLessVal, "Less Value Can Not Be Negative!!!")
            txtLessVal.Focus()
            Return
        End If

        If txtLessVal.Text = "" Or Val(txtLessVal.Text) <= 0 Then
            'Birendra : bug-7528 on 04/01/2013 :Start:
            If txtAddVal.Text.Length = 0 Then
                ErrorProvider1.SetError(txtAddVal, "Add Value Can Not Be Empty!!!")
                txtAddVal.Focus()
                Return
            End If
            'Birendra : bug-7528 on 04/01/2013 :End:
            txtLessVal.Text = 0
        Else
            txtAddVal.Text = 0
        End If

        cmbAsset.Enabled = False
        txtDate.Enabled = False
        txtRemark.Enabled = False
        txtAddVal.Enabled = False
        txtLessVal.Enabled = False
        ToolStripButton1.Enabled = False
        btnCancel.Enabled = False
        btnFirst.Enabled = True
        btnForward.Enabled = True
        btnLast.Enabled = True
        btnBack.Enabled = True
        btnNew.Enabled = True
        btnEdit.Enabled = True
        BtnDelete.Enabled = True
        btnLocate.Enabled = True
        ErrorProvider1.Clear() 'Birendra : Bug-7528 on 04/01/2013

        ' Added By Shrikant S. on 14/08/2014 for Bug-23791      Start
        Dim _purvalue, _addvalue, _lessvalue As Double

        If Not (Double.TryParse(txtBookVal.Text, _purvalue)) Then
            _purvalue = 0
        End If
        If Not (Double.TryParse(txtAddVal.Text, _addvalue)) Then
            _addvalue = 0
        End If
        If Not (Double.TryParse(txtLessVal.Text, _lessvalue)) Then
            _lessvalue = 0
        End If
        ' Added By Shrikant S. on 14/08/2014 for Bug-23791      End

        If enunit = 1 Then
            'sqlstr = "Set DateFormat dmy update addvl set assetCode='" + Trim(cmbAsset.Text) + "',PurchaseDate=" + "'" + CDate(txtDate.Text) + "',AddValue=" + txtAddVal.Text + ",LessValue=" + txtLessVal.Text + ",CommenceDate=" + "'" + txtCDate.Text + "',Remarks=" + "'" & txtRemark.Text & "'" + " where vno=" + txtDcNo.Text
            sqlstr = "Set DateFormat dmy update addvl set assetCode='" + Trim(cmbAsset.Text) + "',PurchaseDate=" + "'" + CDate(txtDate.Text) + "',AddValue=" + CStr(_addvalue) + ",LessValue=" + CStr(_lessvalue) + ",CommenceDate=" + "'" + txtCDate.Text + "',Remarks=" + "'" & txtRemark.Text & "'" + " where vno=" + txtDcNo.Text
            conn(sqlstr)
            MsgBox("Record Updated...", MsgBoxStyle.OkOnly, "Update")
            enunit = 0
            Exit Sub
        End If



        Dim pdate As Date
        pdate = CDate(txtDate.Text)
        'sqlstr = "Set DateFormat dmy INSERT INTO addvl (AssetCode,PurchaseDate,PurchaseValue,AddValue,LessValue,CommenceDate,Remarks)VALUES('" + Trim(cmbAsset.Text) + "','" + CDate(txtDate.Text) + "'," + txtBookVal.Text + "," + txtAddVal.Text + "," + txtLessVal.Text + ",'" + txtCDate.Text + "','" & txtRemark.Text & "')"      ' Commented By Shrikant S. on 14/08/2014 for Bug-23791
        sqlstr = "Set DateFormat dmy INSERT INTO addvl (AssetCode,PurchaseDate,PurchaseValue,AddValue,LessValue,CommenceDate,Remarks)VALUES('" + Trim(cmbAsset.Text) + "','" + CDate(txtDate.Text) + "'," + CStr(_purvalue) + "," + CStr(_addvalue) + "," + CStr(_lessvalue) + ",'" + txtCDate.Text + "','" & txtRemark.Text & "')"       ' Added By Shrikant S. on 14/08/2014 for Bug-23791

        conn(sqlstr)
        MsgBox("Entry Saved...", MsgBoxStyle.OkOnly, "Save")
        Dim objConnection As SqlConnection = New SqlConnection("Server='" + pServerName + "';Database='" + pComDbnm + "';User ID='" + pUserId + "';Password='" + pPassword + "';")
        Dim reader As SqlDataReader
        Dim command As SqlCommand = New SqlCommand
        command.Connection = objConnection
        command.CommandText = "select Vno from addvl order by vno desc"
        objConnection.Open()

        reader = command.ExecuteReader()
        If (reader.HasRows) Then
            reader.Read()
            txtDcNo.Text = reader.GetInt32(0)
        End If
    End Sub

    Private Sub BtnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnDelete.Click
        If MessageBox.Show("Are you sure to delete this entry?", "Confirmation", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
            sqlstr = "delete from addvl where Vno= '" + txtDcNo.Text + "'"
            conn(sqlstr)
            MsgBox("Record Deleted...", MsgBoxStyle.OkOnly, "Delete")
            txtDcNo.Clear()
            cmbAsset.Text = ""
            txtDate.Text = Now
            txtBookVal.Clear()
            txtAddVal.Clear()
            txtLessVal.Clear()
            txtCDate.Text = ""
            txtRemark.Clear()
            'Me.Form1_Load(sender, e)
            mFormload()
            Me.Refresh()
        End If
    End Sub

    Private Sub btnLocate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLocate.Click
        Locat.Show()
        Me.Hide()
    End Sub



    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ToolStripButton1.Enabled = False
        btnCancel.Enabled = False
        btnFirst.Enabled = True
        btnForward.Enabled = True
        btnLast.Enabled = True
        btnBack.Enabled = True
        btnNew.Enabled = True
        btnEdit.Enabled = True
        btnLocate.Enabled = True
        BtnDelete.Enabled = True

        mFormload()
        'Me.Form1_Load(sender, e)
    End Sub

    Private Sub btnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        enunit = 1
        ToolStripButton1.Enabled = True
        btnCancel.Enabled = True
        btnFirst.Enabled = False
        btnForward.Enabled = False
        btnLast.Enabled = False
        btnBack.Enabled = False
        btnNew.Enabled = False
        btnEdit.Enabled = False
        BtnDelete.Enabled = False
        btnLocate.Enabled = False
        'Birendra : Bug-7528 on 04/01/2013 :Start:
        txtDate.Enabled = True
        txtLessVal.Enabled = True
        txtRemark.Enabled = True
        txtAddVal.Enabled = True
        cmbAsset.Enabled = True
        'Birendra : Bug-7528 on 04/01/2013 :End:

    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnPreview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPreview.Click

    End Sub

    Private Sub cmbAsset_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAsset.Leave
        ErrorProvider1.Clear() 'Birendra : Bug-7528 on 04/01/2013
    End Sub

    Private Sub txtAddVal_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAddVal.Leave
        ErrorProvider1.Clear() 'Birendra : Bug-7528 on 04/01/2013
    End Sub
    Private Sub mInsertProcessIdRecord()
        Dim sqlstr As String
        Dim pi As Integer

        pi = Process.GetCurrentProcess().Id
        cAppName = "UeChangeAssetValue.exe"
        cAppPId = Convert.ToString(Process.GetCurrentProcess().Id)
        sqlstr = "Set DateFormat dmy insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + pPApplCode + "','" + Now.Date.ToString() + "','" + pPApplName + "'," + pPApplPID.ToString() + ",'" + pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + Text.Trim() + "')"
        conn(sqlstr)
    End Sub
    Private Sub mDeleteProcessIdRecord()

        If (String.IsNullOrEmpty(pPApplName) Or pPApplPID = 0 Or String.IsNullOrEmpty(cAppName) Or String.IsNullOrEmpty(cAppPId)) Then
            Return
        End If
        Dim sqlstr As String
        sqlstr = " Delete from vudyog..ExtApplLog where pApplNm='" + pPApplName + "' and pApplId=" + pPApplPID.ToString() + " and cApplNm= '" + cAppName + "' and cApplId= " + cAppPId
        conn(sqlstr)
    End Sub
    Private Sub mFormload()
        ToolStripButton1.Enabled = False
        btnCancel.Enabled = False
        btnPreview.Enabled = False
        btnPrint.Enabled = False
        txtCDate.CustomFormat = "dd/MM/yyyy"
        txtDate.CustomFormat = "dd/MM/yyyy"
        Dim objConnection As SqlConnection = New SqlConnection("Server='" + pServerName + "';Database='" + pComDbnm + "';User ID='" + pUserId + "';Password='" + pPassword + "';")
        Dim reader As SqlDataReader
        Dim texists As Integer
        Dim command As SqlCommand = New SqlCommand
        command.Connection = objConnection
        command.CommandText = "select Vno,AssetCode,PurchaseDate,PurchaseValue,AddValue,LessValue,CommenceDate,Remarks from addvl"
        objConnection.Open()

        reader = command.ExecuteReader()
        If (reader.HasRows) Then
            ' While (reader.Read())
            reader.Read()
            txtDcNo.Text = reader.GetInt32(0)
            cmbAsset.Text = reader.GetString(1)
            txtDate.Text = reader.GetString(2)
            txtBookVal.Text = reader.GetDecimal(3)
            txtAddVal.Text = reader.GetDecimal(4)
            txtLessVal.Text = reader.GetDecimal(5)
            txtCDate.Text = reader.GetDateTime(6)
            txtRemark.Text = reader.GetString(7)
            'End While

            'Birendra : Bug-7528 on 02/01/2013 :Start:
            btnEmail.Enabled = False
            ToolStripButton1.Enabled = False
            btnCancel.Enabled = False
            btnFirst.Enabled = False
            btnEdit.Enabled = True
            BtnDelete.Enabled = True
            btnLocate.Enabled = True
            btnNew.Enabled = True
            objConnection.Close()
            command.CommandText = "SELECT COUNT(*) from bs_usage"
            objConnection.Open()
            reader = command.ExecuteReader()
            If (reader.Read()) Then
                texists = Val(reader.GetValue(0).ToString())
            End If
            If texists = 1 Then
                btnForward.Enabled = False
                btnLast.Enabled = False
                btnBack.Enabled = False
                btnFirst.Enabled = False
            Else
                btnForward.Enabled = True
                btnLast.Enabled = True
                btnBack.Enabled = False
                btnFirst.Enabled = False
            End If

        Else
            btnEdit.Enabled = False
            BtnDelete.Enabled = False
            btnFirst.Enabled = False
            btnForward.Enabled = False
            btnLast.Enabled = False
            btnBack.Enabled = False
            btnEmail.Enabled = False
            btnLocate.Enabled = False
            txtCDate.CustomFormat = " "
            txtDcNo.Clear()
            cmbAsset.Text = ""
            txtDate.Text = Now
            txtBookVal.Clear()
            txtAddVal.Clear()
            txtLessVal.Clear()
            txtCDate.Text = ""
            txtRemark.Clear()
            txtDate.CustomFormat = " "

            'Birendra : Bug-7528 on 02/01/2013 :End:

        End If

        cmbAsset.Enabled = False
        txtDate.Enabled = False
        txtLessVal.Enabled = False
        txtRemark.Enabled = False
        txtAddVal.Enabled = False
        'txtDate.CustomFormat = "dd/MM/yyyy"
        'txtDate.Format = DateTimePickerFormat.Custom

    End Sub

    Private Sub AdditionDeletion_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        mDeleteProcessIdRecord()
    End Sub
    Private Sub USAGE_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Birendra : Bug-7528 on 14/03/2013 :Start:
        If e.Control = True Then
            Dim exf As System.EventArgs
            'Dim exf = e
            If e.KeyCode = Keys.D Then
                If btnDelete.Enabled Then
                    btnDelete_Click(sender, exf)
                End If
            End If

            'If e.KeyCode = Keys.C Then
            '    If btnCopy.Enabled Then
            '        btnCopy_Click(sender, exf)
            '    End If
            'End If

            If e.KeyCode = Keys.E Then
                If btnEdit.Enabled Then
                    btnEdit_Click(sender, exf)
                End If
            End If

            If e.KeyCode = Keys.N Then
                If btnNew.Enabled Then
                    btnNew_Click(sender, exf)
                End If
            End If

            If e.KeyCode = Keys.Z Then
                If btnCancel.Enabled Then
                    btnCancel_Click(sender, exf)
                End If
            End If

            If e.KeyCode = Keys.S Then
                If ToolStripButton1.Enabled Then
                    ToolStripButton1_Click(sender, exf)
                End If
            End If

            If e.KeyCode = Keys.L Then
                If btnLocate.Enabled Then
                    btnLocate_Click(sender, exf)
                End If
            End If

            If e.KeyCode = Keys.Left Then
                If btnBack.Enabled Then
                    btnBack_Click(sender, exf)
                End If
            End If

            If e.KeyCode = Keys.Right Then
                If btnForward.Enabled Then
                    btnForward_Click(sender, exf)
                End If
            End If


            If e.KeyCode = Keys.F4 Then
                If btnExit.Enabled Then
                    btnExit_Click(sender, exf)
                End If
            End If
        End If
        'Birendra : Bug-7528 on 14/03/2013 :End:
    End Sub

    Private Sub txtDate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDate.ValueChanged

    End Sub
    'Added by Shrikant S. on 14/08/2014 for Bug-23791   Start
    Private Sub txtBookVal_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtLessVal.KeyDown, txtBookVal.KeyDown, txtAddVal.KeyDown
        Dim text As String = CType(sender, TextBox).Text
        If Char.IsDigit(Chr(e.KeyValue)) Or _
           Chr(e.KeyValue) = "¾"c Or _
           e.KeyData = Keys.Delete Or _
           e.KeyData = Keys.Back Or _
            (e.KeyValue >= 96 And e.KeyValue <= 105) Or (e.KeyValue = 110) Then


            If Chr(e.KeyValue) = "¾"c Or (e.KeyValue = 110) Then
                If text.Contains(".") Then
                    e.SuppressKeyPress = True
                Else
                    e.SuppressKeyPress = False
                End If
            End If
        ElseIf e.KeyData = Keys.Enter Then
            'State a call to function for when Enter is pressed`
        Else
            e.SuppressKeyPress = True
        End If

    End Sub
    'Added by Shrikant S. on 14/08/2014 for Bug-23791   End
End Class
