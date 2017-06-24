Imports System.Data.SqlClient
Imports System.Globalization
Imports System.Threading

Public Class USAGE
    Public it_pval, bs_pval, total_unit, salvage, estimate, enunit As Integer
    Public depr As Double

    Private Sub USAGE_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Birendra : Bug-7528 on 02/01/2013 :Start:
        Dim ci As CultureInfo
        ci = New CultureInfo("en-US")
        ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy"
        Thread.CurrentThread.CurrentCulture = ci

        If (My.Application.CommandLineArgs.Count > 0) Then
            pPApplPID = 0
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
            pPApplPID = Convert.ToInt16(args(10))
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
        mformload()
        mInsertProcessIdRecord()

    End Sub

    Private Sub cmbAssCode_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim objConnection As SqlConnection = New SqlConnection("Server='" + pServerName + "';Database='" + pComDbnm + "';User ID='" + pUserId + "';Password='" + pPassword + "';")
        Dim reader As SqlDataReader
        Dim command As SqlCommand = New SqlCommand
        command.Connection = objConnection
        command.CommandText = "select USGUNIT,PVALUE,SALVAGE,ESTIMATE from it_mast WHERE it_name='" + cmbAssCode.Text + "'"
        objConnection.Open()

        reader = command.ExecuteReader()

        If (reader.HasRows) Then
            While (reader.Read())
                txtUnit.Text = reader.GetString(0)
                it_pval = reader.GetDecimal(1)
                salvage = reader.GetDecimal(2)
                estimate = reader.GetDecimal(3)
                txtlife.Text = estimate
            End While
        End If
        objConnection.Close()

        command.CommandText = "select TOT_UNIT from bs_usage WHERE assetcode='" + cmbAssCode.Text + "'"
        objConnection.Open()
        reader = command.ExecuteReader()
        If (reader.HasRows) Then
            While (reader.Read())
                txtPunit.Text = reader.GetDecimal(0)
            End While
        End If
        If (reader.HasRows = False) Then
            txtPunit.Text = 0
        End If
    End Sub

    Private Sub btnsave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        depr = (((it_pval - salvage) / estimate) * txtNunit.Text)
        Call calculate_usage()

    End Sub
    Sub calculate_usage()
        'Added by Shrikant S. on 13/08/2014 for Bug-23791           Start
        Dim _life, _punit, _nunit, _depr As Double
        If Not (Double.TryParse(txtlife.Text, _life)) Then
            _life = 0
        End If
        If Not (Double.TryParse(txtPunit.Text, _punit)) Then
            _punit = 0
        End If
        If Not (Double.TryParse(txtNunit.Text, _nunit)) Then
            _nunit = 0
        End If
        If Not (Double.TryParse(txtDepr.Text, _depr)) Then
            _depr = 0
        End If
        total_unit = _punit + _nunit
        'Added by Shrikant S. on 13/08/2014 for Bug-23791           End

        If enunit = 1 Then
            '            sqlstr = "update bs_usage set new_unit=" + txtNunit.Text + " where dcno=" + txtDCNO.Text + " "
            'Birendra :  Bug-7528 on 02/07/2013
            sqlstr = "update bs_usage set new_unit=" + CStr(_nunit) + ",depr=" + CStr(_depr) + " where dcno=" + txtDCNO.Text + " "        'Added By Shrikant S. on 14/08/2014 for Bug-23791
            'sqlstr = "update bs_usage set new_unit=" + txtNunit.Text + ",depr=" + txtDepr.Text + " where dcno=" + txtDCNO.Text + " "   'Commented By Shrikant S. on 14/08/2014 for Bug-23791
            conn(sqlstr)
            MsgBox("Record Updated...", MsgBoxStyle.OkOnly, "Update")
            txtNunit.Enabled = False
            enunit = 0
            Exit Sub
        End If

        Dim ndate As Date
        Dim cng As Long
        cng = CLng(Val(TXTDATE.Text))
        ndate = New Date(cng)
        'total_unit = Val(txtPunit.Text) + Val(txtNunit.Text)
        If (cmbWorkOrder.Text = "") Then
            cmbWorkOrder.Text = 0
        End If
        txtNunit.Text = CStr(_nunit)
        sqlstr = "insert into bs_usage (DATE1,ASSETCODE,USAGE_UNIT,EXP_LIFE,PRE_UNIT,NEW_UNIT,TOT_UNIT,DEPR,WORK_NO)values('" + CDate(TXTDATE.Text) + "','" + cmbAssCode.Text + "','" + txtUnit.Text + "'," + CStr(_life) + "," + CStr(_punit) + "," + CStr(_nunit) + "," & CStr(total_unit) & "," & CStr(_depr) & ",'" + cmbWorkOrder.Text + "')"        ' Added by Shrikant S. on 13/08/2014 for Bug-23791
        'sqlstr = "insert into bs_usage (DATE1,ASSETCODE,USAGE_UNIT,EXP_LIFE,PRE_UNIT,NEW_UNIT,TOT_UNIT,DEPR,WORK_NO)values('" + CDate(TXTDATE.Text) + "','" + cmbAssCode.Text + "','" + txtUnit.Text + "'," + txtlife.Text + "," + txtPunit.Text + "," + txtNunit.Text + "," & total_unit & "," & depr & ",'" + cmbWorkOrder.Text + "')"       ' Commented by Shrikant S. on 13/08/2014 for Bug-23791
        conn(sqlstr)

        MsgBox("Record Saved...", MsgBoxStyle.OkOnly, "Save")

        Dim objConnection As SqlConnection = New SqlConnection("Server='" + pServerName + "';Database='" + pComDbnm + "';User ID='" + pUserId + "';Password='" + pPassword + "';")
        Dim reader As SqlDataReader
        Dim command As SqlCommand = New SqlCommand
        command.Connection = objConnection
        command.CommandText = "select dcno from bs_usage order by dcno desc"
        objConnection.Open()
        reader = command.ExecuteReader()

        If (reader.HasRows) Then
            reader.Read()
            txtDCNO.Text = reader.GetInt32(0)
        End If

        cmbWorkOrder.Enabled = False
        txtDepr.Text = depr
        txtDepr.Enabled = False
        TXTDATE.Enabled = False
        cmbAssCode.Enabled = False
        txtNunit.Enabled = False
        ToolStripButton1.Enabled = False
        btnCancel.Enabled = False
        btnFirst.Enabled = True
        btnForward.Enabled = True
        btnLast.Enabled = True
        btnBack.Enabled = True
        btnNew.Enabled = True
        btnEdit.Enabled = True
        'btnEdit.Enabled = False  'Birendra :  Bug-7528 on 02/07/2013
        btnDelete.Enabled = True
        btnLocate.Enabled = True

    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        enunit = 0
        txtDCNO.Clear()
        TXTDATE.CustomFormat = "dd/MM/yyyy"
        TXTDATE.Text = Now
        TXTDATE.Enabled = True
        cmbAssCode.Text = ""
        cmbAssCode.Enabled = True
        txtlife.Clear()
        txtUnit.Clear()
        txtPunit.Clear()
        txtNunit.Clear()
        cmbWorkOrder.Text = ""
        txtDepr.Clear()
        cmbWorkOrder.Enabled = True
        'txtDepr.Enabled = True
        txtNunit.Enabled = True
        ToolStripButton1.Enabled = True
        btnCancel.Enabled = True
        btnFirst.Enabled = False
        btnForward.Enabled = False
        btnLast.Enabled = False
        btnBack.Enabled = False
        btnNew.Enabled = False
        btnEdit.Enabled = False
        btnLocate.Enabled = False
        btnDelete.Enabled = False
        Dim objConnection As SqlConnection = New SqlConnection("Server='" + pServerName + "';Database='" + pComDbnm + "';User ID='" + pUserId + "';Password='" + pPassword + "';")
        Dim reader As SqlDataReader
        Dim command As SqlCommand = New SqlCommand
        command.Connection = objConnection
        command.CommandText = "select IT_NAME from it_mast WHERE TYPE='Machinery/Stores              ' and methodnm='BASED ON USAGE'"
        objConnection.Open()

        reader = command.ExecuteReader()
        If (cmbAssCode.Items.Count = 0) Then

            If (reader.HasRows) Then
                While (reader.Read())
                    cmbAssCode.Items.Add(reader.GetString(0))
                End While
            End If
        End If

        objConnection.Close()

        command.CommandText = "select distinct inv_no from main"
        objConnection.Open()
        reader = command.ExecuteReader()
        If (reader.HasRows) Then
            While (reader.Read())
                cmbWorkOrder.Items.Add(reader.GetString(0))
            End While
        End If
        objConnection.Close()
        TXTDATE.Focus()
    End Sub

    Private Sub cmbAssCode_SelectedIndexChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAssCode.SelectedIndexChanged
        Dim objConnection As SqlConnection = New SqlConnection("Server='" + pServerName + "';Database='" + pComDbnm + "';User ID='" + pUserId + "';Password='" + pPassword + "';")
        Dim reader As SqlDataReader
        Dim command As SqlCommand = New SqlCommand
        command.Connection = objConnection
        command.CommandText = "select USGUNIT,PVALUE,SALVAGE,ESTIMATE from it_mast WHERE it_name='" + cmbAssCode.Text + "'"
        objConnection.Open()

        reader = command.ExecuteReader()

        If (reader.HasRows) Then
            While (reader.Read())
                txtUnit.Text = reader.GetString(0)
                it_pval = reader.GetDecimal(1)
                salvage = reader.GetDecimal(2)
                estimate = reader.GetDecimal(3)
                txtlife.Text = estimate
            End While
        End If
        objConnection.Close()

        command.CommandText = "select TOT_UNIT from bs_usage WHERE assetcode='" + cmbAssCode.Text + "'"
        objConnection.Open()
        reader = command.ExecuteReader()
        If (reader.HasRows) Then
            While (reader.Read())
                txtPunit.Text = reader.GetDecimal(0)
            End While
        End If
        If (reader.HasRows = False) Then
            txtPunit.Text = 0
        End If
    End Sub

    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton1.Click
        'Birendra : bug-7528 on 04/01/2013 :Start:
        If cmbAssCode.Text.Length = 0 Then
            ErrorProvider1.SetError(cmbAssCode, "Asset Code Can Not Be Empty!!!")
            cmbAssCode.Focus()
            Return
        End If
        If txtNunit.Text = "" Then
            ErrorProvider1.SetError(txtNunit, "New Unit Can Not Be Empty!!!")
            txtNunit.Focus()
            Return
        End If

        ToolStripButton1.Enabled = False
        btnCancel.Enabled = False
        btnFirst.Enabled = True
        btnForward.Enabled = True
        btnLast.Enabled = True
        btnBack.Enabled = True
        btnNew.Enabled = True
        btnEdit.Enabled = True
        'btnEdit.Enabled = False  'Birendra :  Bug-7528 on 02/07/2013
        btnDelete.Enabled = True
        btnLocate.Enabled = True
        'Birendra : bug-7528 on 04/01/2013 :End:
        calculate_depr() 'Birendra : Bug-7528 on 02/07/2013
        'Added By Shrikant S. on 13/08/2014 for Bug-23791       Start
        Dim _newunit As Double
        If Not (Double.TryParse(txtNunit.Text, _newunit)) Then
            _newunit = 0
        End If
        If estimate > 0 Then
            depr = (((it_pval - salvage) / estimate) * _newunit)
        End If
        'Added By Shrikant S. on 13/08/2014 for Bug-23791       End

        'depr = (((it_pval - salvage) / estimate) * txtNunit.Text)      'Commented By Shrikant S. on 13/08/2014 for Bug-23791
        txtDepr.Text = CStr(depr)
        Call calculate_usage()

    End Sub

    Private Sub btnForward_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnForward.Click
        Dim vnum, cnt1, cnt2 As Integer
        vnum = Val(txtDCNO.Text) + 1
        Dim objConnection As SqlConnection = New SqlConnection("Server='" + pServerName + "';Database='" + pComDbnm + "';User ID='" + pUserId + "';Password='" + pPassword + "';")
        Dim reader As SqlDataReader
        Dim command As SqlCommand = New SqlCommand
        command.Connection = objConnection
        command.CommandText = "select dcno from bs_usage order by dcno desc"
        objConnection.Open()
        reader = command.ExecuteReader()
        If (reader.Read()) Then
            cnt2 = reader.GetInt32(0)
        End If

        objConnection.Close()
        cnt1 = 0
        Do Until cnt1 > 0
            'command.CommandText = "select DCNO,DATE1,ASSETCODE,USAGE_UNIT,EXP_LIFE,PRE_UNIT,NEW_UNIT,TOT_UNIT,DEPR from bs_usage where dcno=" & vnum & ""
            'Birendra : Bug-7528 on 02/07/2013
            command.CommandText = "select DCNO,DATE1,ASSETCODE,USAGE_UNIT,EXP_LIFE,PRE_UNIT,NEW_UNIT,TOT_UNIT,DEPR,Work_no from bs_usage where dcno=" & vnum & ""
            objConnection.Open()
            reader = command.ExecuteReader()

            If (reader.Read = True) Then
                txtDCNO.Text = reader.GetInt32(0)
                TXTDATE.Text = reader.GetString(1)
                cmbAssCode.Text = reader.GetString(2)
                txtUnit.Text = reader.GetString(3)
                txtlife.Text = reader.GetDecimal(4)
                txtPunit.Text = reader.GetDecimal(5)
                txtNunit.Text = reader.GetDecimal(6)
                txtDepr.Text = reader.GetDecimal(8) 'Birendra : Bug-7528 on 02/07/2013
                cmbWorkOrder.Text = reader.GetString(9)  'Birendra : Bug-7528 on 02/07/2013
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
                'MsgBox("Last record")
                btnForward.Enabled = False
                btnLast.Enabled = False
                btnBack.Enabled = True
                btnFirst.Enabled = True

                Exit Do
            Else
                vnum = vnum + 1
                objConnection.Close()
            End If
        Loop
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Dim vnum, cnt1, cnt2 As Integer
        vnum = Val(txtDCNO.Text) - 1
        Dim objConnection As SqlConnection = New SqlConnection("Server='" + pServerName + "';Database='" + pComDbnm + "';User ID='" + pUserId + "';Password='" + pPassword + "';")
        Dim reader As SqlDataReader
        Dim command As SqlCommand = New SqlCommand
        command.Connection = objConnection
        command.CommandText = "select dcno from bs_usage order by dcno asc"
        objConnection.Open()
        reader = command.ExecuteReader()
        If (reader.Read()) Then
            cnt2 = reader.GetInt32(0)
        End If
        objConnection.Close()
        Do Until cnt1 > 0
            'command.CommandText = "select DCNO,DATE1,ASSETCODE,USAGE_UNIT,EXP_LIFE,PRE_UNIT,NEW_UNIT,TOT_UNIT,DEPR from bs_usage where dcno=" & vnum & ""
            'Birendra : Bug-7528 on 02/07/2013
            command.CommandText = "select DCNO,DATE1,ASSETCODE,USAGE_UNIT,EXP_LIFE,PRE_UNIT,NEW_UNIT,TOT_UNIT,DEPR,Work_no from bs_usage where dcno=" & vnum & ""
            objConnection.Open()
            reader = command.ExecuteReader()
            If (reader.Read = True) Then
                txtDCNO.Text = reader.GetInt32(0)
                TXTDATE.Text = reader.GetString(1)
                cmbAssCode.Text = reader.GetString(2)
                txtUnit.Text = reader.GetString(3)
                txtlife.Text = reader.GetDecimal(4)
                txtPunit.Text = reader.GetDecimal(5)
                txtNunit.Text = reader.GetDecimal(6)
                txtDepr.Text = reader.GetDecimal(8) 'Birendra : Bug-7528 on 02/07/2013
                cmbWorkOrder.Text = reader.GetString(9) 'Birendra : Bug-7528 on 02/07/2013

                cnt1 = 1
                objConnection.Close()
                'Birendra : Bug-7528 on 06/01/2013 :Start:
                btnForward.Enabled = True
                btnLast.Enabled = True
                'Birendra : Bug-7528 on 06/01/2013 :end:
                If (vnum = cnt2) Then
                    'Birendra : Bug-7528 on 06/01/2013 :Start:
                    btnForward.Enabled = True
                    btnLast.Enabled = True
                    btnFirst.Enabled = False
                    btnBack.Enabled = False
                    'Birendra : Bug-7528 on 06/01/2013 :end:
                End If

            ElseIf (vnum < cnt2) Then
                'MsgBox("First record")
                'Birendra : Bug-7528 on 06/01/2013 :Start:
                btnForward.Enabled = True
                btnLast.Enabled = True
                btnFirst.Enabled = False
                btnBack.Enabled = False
                'Birendra : Bug-7528 on 06/01/2013 :end:
                Exit Do
            Else
                vnum = vnum - 1
                objConnection.Close()

            End If
        Loop
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        Dim objConnection As SqlConnection = New SqlConnection("Server='" + pServerName + "';Database='" + pComDbnm + "';User ID='" + pUserId + "';Password='" + pPassword + "';")
        Dim reader As SqlDataReader
        Dim command As SqlCommand = New SqlCommand
        command.Connection = objConnection
        'command.CommandText = "select DCNO,DATE1,ASSETCODE,USAGE_UNIT,EXP_LIFE,PRE_UNIT,NEW_UNIT,TOT_UNIT,DEPR from bs_usage order by dcno desc"
        'Birendra : Bug-7528 on 02/07/2013
        command.CommandText = "select DCNO,DATE1,ASSETCODE,USAGE_UNIT,EXP_LIFE,PRE_UNIT,NEW_UNIT,TOT_UNIT,DEPR,work_no from bs_usage order by dcno desc"
        objConnection.Open()
        reader = command.ExecuteReader()

        If (reader.HasRows) Then
            'While (reader.Read())
            reader.Read()
            txtDCNO.Text = reader.GetInt32(0)
            TXTDATE.Text = reader.GetString(1)
            cmbAssCode.Text = reader.GetString(2)
            txtUnit.Text = reader.GetString(3)
            txtlife.Text = reader.GetDecimal(4)
            txtPunit.Text = reader.GetDecimal(5)
            txtNunit.Text = reader.GetDecimal(6)
            txtDepr.Text = reader.GetDecimal(8) 'Birendra : Bug-7528 on 02/07/2013
            cmbWorkOrder.Text = reader.GetString(9) 'Birendra : Bug-7528 on 02/07/2013



        End If
        'Birendra : Bug-7528 on 06/01/2013 :Start:
        btnForward.Enabled = False
        btnLast.Enabled = False
        btnBack.Enabled = True
        btnFirst.Enabled = True
        'Birendra : Bug-7528 on 06/01/2013 :end:


    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        Dim objConnection As SqlConnection = New SqlConnection("Server='" + pServerName + "';Database='" + pComDbnm + "';User ID='" + pUserId + "';Password='" + pPassword + "';")
        Dim reader As SqlDataReader
        Dim command As SqlCommand = New SqlCommand
        command.Connection = objConnection
        'command.CommandText = "select DCNO,DATE1,ASSETCODE,USAGE_UNIT,EXP_LIFE,PRE_UNIT,NEW_UNIT,TOT_UNIT,DEPR from bs_usage order by dcno asc"
        'Birendra : Bug-7528 on 02/07/2013
        command.CommandText = "select DCNO,DATE1,ASSETCODE,USAGE_UNIT,EXP_LIFE,PRE_UNIT,NEW_UNIT,TOT_UNIT,DEPR,Work_no from bs_usage order by dcno asc"
        objConnection.Open()
        reader = command.ExecuteReader()

        If (reader.HasRows) Then
            'While (reader.Read())
            reader.Read()
            txtDCNO.Text = reader.GetInt32(0)
            TXTDATE.Text = reader.GetString(1)
            cmbAssCode.Text = reader.GetString(2)
            txtUnit.Text = reader.GetString(3)
            txtlife.Text = reader.GetDecimal(4)
            txtPunit.Text = reader.GetDecimal(5)
            txtNunit.Text = reader.GetDecimal(6)
            txtDepr.Text = reader.GetDecimal(8) 'Birendra : Bug-7528 on 02/07/2013
            cmbWorkOrder.Text = reader.GetString(9) 'Birendra : Bug-7528 on 02/07/2013
        End If
        'Birendra : Bug-7528 on 06/01/2013 :Start:
        btnForward.Enabled = True
        btnLast.Enabled = True
        btnFirst.Enabled = False
        btnBack.Enabled = False
        'Birendra : Bug-7528 on 06/01/2013 :end:

    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If MessageBox.Show("Are you sure to delete this entry?", "Confirmation", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
            sqlstr = "delete from bs_usage where dcno= '" + txtDCNO.Text + "'"
            conn(sqlstr)
            MsgBox("Record Deleted...", MsgBoxStyle.OkOnly, "Delete")
            txtDCNO.Clear()
            cmbAssCode.Text = ""
            TXTDATE.Text = Now
            txtUnit.Clear()
            txtlife.Clear()
            txtPunit.Clear()
            txtNunit.Clear()
            cmbWorkOrder.Text = ""
            txtDepr.Clear()
            'Me.USAGE_Load(sender, e)
            mformload()
            Me.Refresh()
        End If

    End Sub

    Private Sub btnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        txtNunit.Enabled = True
        enunit = 1
        'Birendra : Bug-7528 on 04/01/2013 :Start:
        ToolStripButton1.Enabled = True
        btnCancel.Enabled = True
        btnFirst.Enabled = False
        btnForward.Enabled = False
        btnLast.Enabled = False
        btnBack.Enabled = False
        btnNew.Enabled = False
        btnEdit.Enabled = False
        btnDelete.Enabled = False
        btnLocate.Enabled = False
        'Birendra : Bug-7528 on 04/01/2013 :End:

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        txtDCNO.Clear()
        cmbAssCode.Text = ""
        TXTDATE.Text = Now
        txtUnit.Clear()
        txtlife.Clear()
        txtPunit.Clear()
        txtNunit.Clear()
        cmbWorkOrder.Text = ""
        '        Me.USAGE_Load(sender, e)
        'Birendra : Bug-7528 on 04/01/2013 :Start:
        ToolStripButton1.Enabled = False
        btnCancel.Enabled = False
        btnFirst.Enabled = False
        btnForward.Enabled = False
        btnLast.Enabled = False
        btnBack.Enabled = False
        btnNew.Enabled = True
        btnEdit.Enabled = False
        btnDelete.Enabled = False
        btnLocate.Enabled = False
        'Birendra : Bug-7528 on 04/01/2013 :End:

        mformload()

    End Sub


    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnLocate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLocate.Click
        usage_locate.Show()
        Me.Hide()
    End Sub

    Private Sub btnPreview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPreview.Click

    End Sub

    Private Sub cmbWorkOrder_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbWorkOrder.SelectedIndexChanged

    End Sub

    Private Sub cmbAssCode_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAssCode.Leave
        ErrorProvider1.Clear() 'Birendra : Bug-7528 on 04/01/2013
    End Sub

    Private Sub txtNunit_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNunit.Leave
        ErrorProvider1.Clear() 'Birendra : Bug-7528 on 04/01/2013
    End Sub
    Private Sub mformload()
        Dim objConnection As SqlConnection = New SqlConnection("Server='" + pServerName + "';Database='" + pComDbnm + "';User ID='" + pUserId + "';Password='" + pPassword + "';")
        Dim reader As SqlDataReader
        Dim texists As Integer
        Dim command As SqlCommand = New SqlCommand
        TXTDATE.CustomFormat = "dd/MM/yyyy"
        command.Connection = objConnection
        command.CommandText = "select DCNO,DATE1,ASSETCODE,USAGE_UNIT,EXP_LIFE,PRE_UNIT,NEW_UNIT,TOT_UNIT,DEPR,WORK_NO from bs_usage"
        objConnection.Open()
        reader = command.ExecuteReader()
        texists = 0
        If (reader.Read()) Then
            'While (reader.Read())

            txtDCNO.Text = reader.GetInt32(0)
            TXTDATE.Text = reader.GetString(1)
            cmbAssCode.Text = reader.GetString(2)
            txtUnit.Text = reader.GetString(3)
            txtlife.Text = reader.GetDecimal(4)
            txtPunit.Text = reader.GetDecimal(5)
            txtNunit.Text = reader.GetDecimal(6)
            txtDepr.Text = reader.GetDecimal(8)
            cmbWorkOrder.Text = reader.GetString(9)
            'Birendra : Bug-7528 on 02/01/2013 :Start:
            btnEmail.Enabled = False
            ToolStripButton1.Enabled = False
            btnCancel.Enabled = False
            btnFirst.Enabled = False
            btnEdit.Enabled = True
            'btnEdit.Enabled = False  'Birendra :  Bug-7528 on 02/07/2013
            btnDelete.Enabled = True
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
            btnDelete.Enabled = False
            btnFirst.Enabled = False
            btnForward.Enabled = False
            btnLast.Enabled = False
            btnBack.Enabled = False
            btnEmail.Enabled = False
            btnLocate.Enabled = False
            TXTDATE.CustomFormat = " "
            'Birendra : Bug-7528 on 02/01/2013 :End:

        End If
        TXTDATE.Enabled = False
        cmbAssCode.Enabled = False
        txtNunit.Enabled = False
        cmbWorkOrder.Enabled = False
    End Sub
    Private Sub mInsertProcessIdRecord()
        Dim sqlstr As String
        Dim pi As Integer
        pi = Process.GetCurrentProcess().Id
        cAppName = "UeBasedOnUsages.exe"
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

    Private Sub USAGE_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
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


    Private Sub txtNunit_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNunit.TextChanged
        'Birendra : Bug-7528 on 01/07/2013 :Start:
        'calculate_depr()
        'depr = (((it_pval - salvage) / estimate) * txtNunit.Text)
        'Call calculate_usage()
        'Birendra : Bug-7528 on 01/07/2013 :End:
    End Sub
    Private Sub calculate_depr()
        'Birendra : Bug-7528 on 01/07/2013 :Start:
        Dim objConnection As SqlConnection = New SqlConnection("Server='" + pServerName + "';Database='" + pComDbnm + "';User ID='" + pUserId + "';Password='" + pPassword + "';")
        Dim reader As SqlDataReader
        Dim command As SqlCommand = New SqlCommand
        command.Connection = objConnection
        command.CommandText = "select USGUNIT,PVALUE,SALVAGE,ESTIMATE from it_mast WHERE it_name='" + cmbAssCode.Text + "'"
        objConnection.Open()

        reader = command.ExecuteReader()

        If (reader.HasRows) Then
            While (reader.Read())
                txtUnit.Text = reader.GetString(0)
                it_pval = reader.GetDecimal(1)
                salvage = reader.GetDecimal(2)
                estimate = reader.GetDecimal(3)
                txtlife.Text = estimate
            End While
        End If
        objConnection.Close()
        'Birendra : Bug-7528 on 01/07/2013 :End:
    End Sub

    'Added by Shrikant S. on 14/08/2014 for Bug-23791   Start
    Private Sub txtlife_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPunit.KeyDown, txtNunit.KeyDown, txtlife.KeyDown, txtDepr.KeyDown
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