Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.Threading

Public Class Disposal
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        txtDCNO.Clear()
        TXTDATE.Text = Now
        TXTDATE.Enabled = True
        txtNarr.Enabled = True
        txtValSold.Enabled = True
        cmbAssCode.Text = ""
        txtNarr.Text = ""
        cmbAssCode.Enabled = True
        txtDCNO.Clear()
        txtDepr.Clear()
        txtPurch.Clear()
        txtValSold.Clear()
        btnSave.Enabled = True
        btnCancel.Enabled = True
        btnFirst.Enabled = False
        btnForward.Enabled = False
        btnLast.Enabled = False
        btnBack.Enabled = False
        btnNew.Enabled = False
        btnEdit.Enabled = False
        btnLocate.Enabled = False
        btnDelete.Enabled = False
        cmbAssCode.Items.Clear() 'Birendra : Bug-7528 on 14/03/2013
        Dim objConnection As SqlConnection = New SqlConnection("Server='" + pServerName + "';Database='" + pComDbnm + "';User ID='" + pUserId + "';Password='" + pPassword + "';")
        Dim reader As SqlDataReader
        Dim command As SqlCommand = New SqlCommand
        command.Connection = objConnection
        command.CommandText = "select IT_NAME from it_mast  WHERE TYPE='Machinery/Stores              ' and methodnm<>'' and it_name not in(select assetcode from assetdisposal)"
        objConnection.Open()
        reader = command.ExecuteReader()
        If (cmbAssCode.Items.Count = 0) Then

            If (reader.HasRows) Then
                While (reader.Read())
                    cmbAssCode.Items.Add(reader.GetString(0))
                End While
            End If
        End If
        TXTDATE.CustomFormat = "dd/MM/yyyy"
        objConnection.Close()
        TXTDATE.Focus()
    End Sub

    Private Sub cmbAssCode_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAssCode.SelectedIndexChanged
        Dim objConnection As SqlConnection = New SqlConnection("Server='" + pServerName + "';Database='" + pComDbnm + "';User ID='" + pUserId + "';Password='" + pPassword + "';")
        Dim reader As SqlDataReader
        Dim ds As DataSet
        Dim ad As SqlDataAdapter
        Dim command As SqlCommand = New SqlCommand
        command.Connection = objConnection
        command.CommandText = "select PVALUE from it_mast WHERE it_name='" + cmbAssCode.Text + "'"
        objConnection.Open()
        reader = command.ExecuteReader()
        If (reader.HasRows) Then
            While (reader.Read())
                txtPurch.Text = reader.GetDecimal(0)
            End While
        End If
        objConnection.Close()
        objConnection.Open()
        'Birendra : Bug-7528 on 14/03/2013 :Modified as per below:
        'TXTDATE.CustomFormat = "MM/dd/yyyy"

        'ad = New SqlDataAdapter("EXECUTE USP_REP_DEPRETIATION_SCHEDULE_REPORT'','','','04/01/2012','" + TXTDATE.Text + "','','','','',0,0,'','','','','','','','','2012-2013',''", objConnection)
        ad = New SqlDataAdapter("set dateformat dmy EXECUTE USP_REP_DEPRETIATION_SCHEDULE_REPORT'','','','','" + TXTDATE.Text + "','','','','',0,0,'','','','','','','','','2012-2013',''", objConnection)
        ds = New DataSet
        ad.Fill(ds)
        For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
            If Trim(ds.Tables(0).Rows(i)("it_name1").ToString) = Trim(cmbAssCode.Text) Then
                txtDepr.Text = ds.Tables(0).Rows(i)("depreciation").ToString
                Exit Sub
            Else
                txtDepr.Text = 0
            End If
        Next
        'TXTDATE.CustomFormat = "dd/MM/yyyy"
    End Sub
    Private Sub Disposal_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
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
            pPApplPID = Convert.ToInt16(args(10))
            pPApplCode = args(11)
            Me.Icon = New Icon(args(7))
        End If
        btnBack.Enabled = True
        btnFirst.Enabled = True
        btnNew.Enabled = True
        btnForward.Enabled = True
        btnLast.Enabled = True
        btnSave.Enabled = False
        btnCancel.Enabled = False
        btnEdit.Enabled = False
        btnDelete.Enabled = True
        TXTDATE.Enabled = False
        txtNarr.Enabled = False
        cmbAssCode.Enabled = False
        txtValSold.Enabled = False
        btnLocate.Enabled = True
        mformload()
        mInsertProcessIdRecord()

    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim str1 As String
        If cmbAssCode.Text.Length = 0 Then
            ErrorProvider1.SetError(cmbAssCode, "Asset Code Can Not Be Empty!!!")
            cmbAssCode.Focus()
            Return
        End If
        If txtValSold.Text = "" Then
            txtValSold.Text = 0
        End If
        btnSave.Enabled = False
        btnCancel.Enabled = False
        btnFirst.Enabled = True
        btnForward.Enabled = True
        btnLast.Enabled = True
        btnBack.Enabled = True
        btnNew.Enabled = True
        btnEdit.Enabled = False
        btnDelete.Enabled = True
        btnLocate.Enabled = True
        'Added by Shrikant S. on 14/08/2014 for Bug-23791       Start
        Dim _purch, _depr, _sold As Double
        If Not (Double.TryParse(txtPurch.Text, _purch)) Then
            _purch = 0
        End If
        If Not (Double.TryParse(txtDepr.Text, _depr)) Then
            _depr = 0
        End If
        If Not (Double.TryParse(txtValSold.Text, _sold)) Then
            _sold = 0
        End If
        'Added by Shrikant S. on 14/08/2014 for Bug-23791       End
        str1 = "set dateformat dmy insert into assetdisposal (DisposalDate,Assetcode,PurchaseVal,Depreciation,ValueSold,Narration) values('" + TXTDATE.Text + "','" + cmbAssCode.Text + "'," + CStr(_purch) + "," + CStr(_depr) + "," + CStr(_sold) + ",'" + txtNarr.Text + "')"    'Added by Shrikant S. on 14/08/2014 for Bug-23791   
        'str1 = "set dateformat dmy insert into assetdisposal (DisposalDate,Assetcode,PurchaseVal,Depreciation,ValueSold,Narration) values('" + TXTDATE.Text + "','" + cmbAssCode.Text + "'," + txtPurch.Text + "," + txtDepr.Text + "," + txtValSold.Text + ",'" + txtNarr.Text + "')"      'Added by Shrikant S. on 14/08/2014 for Bug-23791   
        conn(str1)
        btnSave.Enabled = False
        btnNew.Enabled = True
        'Birendra : Bug-7528 on 13/03/2013 :Commented:
        'btnEdit.Enabled = True 
        btnDelete.Enabled = True
        'Me.Disposal_Load(sender, e)

        'Birendra : Bug-7528 on 14/03/2013 :Commented above one :start:
        mformload()
        TXTDATE.Enabled = False
        cmbAssCode.Enabled = False
        txtValSold.Enabled = False
        txtNarr.Enabled = False
        Me.Refresh()

        'Birendra : Bug-7528 on 14/03/2013 :Commented above one : End:


    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        Dim objConnection As SqlConnection = New SqlConnection("Server='" + pServerName + "';Database='" + pComDbnm + "';User ID='" + pUserId + "';Password='" + pPassword + "';")
        Dim reader As SqlDataReader
        Dim ds As DataSet
        Dim ad As SqlDataAdapter
        ad = New SqlDataAdapter("set dateformat dmy select VoucherNo,DisposalDate,Assetcode,PurchaseVal,Depreciation,ValueSold,Narration from assetdisposal order by VoucherNo asc", objConnection)
        ds = New DataSet
        ad.Fill(ds)
        If (ds.Tables(0).Rows.Count <> 0) Then
            txtDCNO.Text = ds.Tables(0).Rows(0)("VoucherNo").ToString
            'Birendra : Bug-7528 on 14/03/2013 :Modified as per below:
            'Dim date1 As String = DateTime.ParseExact(ds.Tables(0).Rows(0)("DisposalDate"), "M/d/yyyy", CultureInfo.InvariantCulture).ToString("d/M/yyyy")
            Dim date1 = ds.Tables(0).Rows(0)("DisposalDate")
            TXTDATE.Text = date1
            cmbAssCode.Text = ds.Tables(0).Rows(0)("Assetcode").ToString
            txtPurch.Text = ds.Tables(0).Rows(0)("PurchaseVal").ToString
            txtDepr.Text = ds.Tables(0).Rows(0)("Depreciation").ToString
            txtValSold.Text = ds.Tables(0).Rows(0)("ValueSold").ToString
            txtNarr.Text = ds.Tables(0).Rows(0)("Narration").ToString
        End If
        btnForward.Enabled = True
        btnLast.Enabled = True
        btnBack.Enabled = False
        btnFirst.Enabled = False
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        Dim objConnection As SqlConnection = New SqlConnection("Server='" + pServerName + "';Database='" + pComDbnm + "';User ID='" + pUserId + "';Password='" + pPassword + "';")
        Dim reader As SqlDataReader
        Dim ds As DataSet
        Dim ad As SqlDataAdapter
        ad = New SqlDataAdapter("set dateformat dmy select VoucherNo,DisposalDate,Assetcode,PurchaseVal,Depreciation,ValueSold,Narration from assetdisposal order by VoucherNo desc", objConnection)
        ds = New DataSet
        ad.Fill(ds)
        If (ds.Tables(0).Rows.Count <> 0) Then
            txtDCNO.Text = ds.Tables(0).Rows(0)("VoucherNo").ToString
            'Birendra : Bug-7528 on 14/03/2013 :Modified as per below:
            Dim date1 = ds.Tables(0).Rows(0)("DisposalDate")
            'Dim date1 As String = DateTime.ParseExact(ds.Tables(0).Rows(0)("DisposalDate"), "M/d/yyyy", CultureInfo.InvariantCulture).ToString("d/M/yyyy")
            TXTDATE.Text = date1
            cmbAssCode.Text = ds.Tables(0).Rows(0)("Assetcode").ToString
            txtPurch.Text = ds.Tables(0).Rows(0)("PurchaseVal").ToString
            txtDepr.Text = ds.Tables(0).Rows(0)("Depreciation").ToString
            txtValSold.Text = ds.Tables(0).Rows(0)("ValueSold").ToString
            txtNarr.Text = ds.Tables(0).Rows(0)("Narration").ToString
        End If
        btnForward.Enabled = False
        btnLast.Enabled = False
        btnBack.Enabled = True
        btnFirst.Enabled = True
    End Sub

    Private Sub btnForward_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnForward.Click
        Dim objConnection As SqlConnection = New SqlConnection("Server='" + pServerName + "';Database='" + pComDbnm + "';User ID='" + pUserId + "';Password='" + pPassword + "';")
        Dim reader As SqlDataReader
        Dim ds As DataSet
        Dim ad As SqlDataAdapter
        Dim num1 As Integer
        num1 = Val(txtDCNO.Text) + 1
        ad = New SqlDataAdapter("set dateformat dmy select VoucherNo,DisposalDate,Assetcode,PurchaseVal,Depreciation,ValueSold,Narration from assetdisposal where VoucherNo>=" & num1 & "", objConnection)
        ds = New DataSet
        ad.Fill(ds)

        If (ds.Tables(0).Rows.Count <> 0) Then
            txtDCNO.Text = ds.Tables(0).Rows(0)("VoucherNo").ToString
            'Birendra : Bug-7528 on 14/03/2013 :Modified as per below:
            Dim date1 = ds.Tables(0).Rows(0)("DisposalDate")
            'Dim date1 As String = DateTime.ParseExact(ds.Tables(0).Rows(0)("DisposalDate"), "M/d/yyyy", CultureInfo.InvariantCulture).ToString("d/M/yyyy")
            TXTDATE.Text = date1
            cmbAssCode.Text = ds.Tables(0).Rows(0)("Assetcode").ToString
            txtPurch.Text = ds.Tables(0).Rows(0)("PurchaseVal").ToString
            txtDepr.Text = ds.Tables(0).Rows(0)("Depreciation").ToString
            txtValSold.Text = ds.Tables(0).Rows(0)("ValueSold").ToString
            txtNarr.Text = ds.Tables(0).Rows(0)("Narration").ToString
        End If
        If Val(txtDCNO.Text) < num1 Then
            btnForward.Enabled = False
            btnLast.Enabled = False
            btnBack.Enabled = True
            btnFirst.Enabled = True
        Else
            btnForward.Enabled = True
            btnLast.Enabled = True
            btnBack.Enabled = True
            btnFirst.Enabled = True
        End If
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Dim objConnection As SqlConnection = New SqlConnection("Server='" + pServerName + "';Database='" + pComDbnm + "';User ID='" + pUserId + "';Password='" + pPassword + "';")
        Dim reader As SqlDataReader
        Dim ds As DataSet
        Dim ad As SqlDataAdapter
        Dim num1 As Integer
        num1 = Val(txtDCNO.Text) - 1
        ad = New SqlDataAdapter("set dateformat dmy select VoucherNo,DisposalDate,Assetcode,PurchaseVal,Depreciation,ValueSold,Narration from assetdisposal where VoucherNo<=" & num1 & " order by voucherno desc", objConnection)
        ds = New DataSet
        ad.Fill(ds)
        If (ds.Tables(0).Rows.Count <> 0) Then
            txtDCNO.Text = ds.Tables(0).Rows(0)("VoucherNo").ToString
            'Birendra : Bug-7528 on 14/03/2013 :Modified as per below:
            'Dim date1 As String = DateTime.ParseExact(ds.Tables(0).Rows(0)("DisposalDate"), "M/d/yyyy", CultureInfo.InvariantCulture).ToString("d/M/yyyy")
            TXTDATE.Text = ds.Tables(0).Rows(0)("DisposalDate")
            cmbAssCode.Text = ds.Tables(0).Rows(0)("Assetcode").ToString
            txtPurch.Text = ds.Tables(0).Rows(0)("PurchaseVal").ToString
            txtDepr.Text = ds.Tables(0).Rows(0)("Depreciation").ToString
            txtValSold.Text = ds.Tables(0).Rows(0)("ValueSold").ToString
            txtNarr.Text = ds.Tables(0).Rows(0)("Narration").ToString
        End If
        If Val(txtDCNO.Text) > num1 Then
            btnForward.Enabled = True
            btnLast.Enabled = True
            btnBack.Enabled = False
            btnFirst.Enabled = False
        Else
            btnForward.Enabled = True
            btnLast.Enabled = True
            btnBack.Enabled = True
            btnFirst.Enabled = True
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim str1, str2 As String
        Dim msg As MsgBoxResult
        If MessageBox.Show("Are you sure to delete this entry?", "Confirmation", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
            str1 = "delete from assetdisposal where voucherno=" + txtDCNO.Text + ""
            conn(str1)
            mformload()
            Me.Refresh()
        End If

    End Sub

    Private Sub btnLocate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLocate.Click
        dispose_locate.Show()
        Me.Hide()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        TXTDATE.Text = ""
        TXTDATE.Enabled = False
        cmbAssCode.Enabled = False
        txtValSold.Enabled = False
        txtNarr.Enabled = False
        'Birendra : Bug-7528 on 12/03/2013 :Start:
        cmbAssCode.Text = ""
        txtNarr.Clear()
        txtValSold.Clear()
        txtPurch.Clear()
        txtDepr.Clear()
        'Birendra : Bug-7528 on 12/03/2013 :End:
        mformload()
        Me.Refresh()
    End Sub

    Private Sub mformload()
        btnEmail.Enabled = False
        btnCancel.Enabled = False
        btnFirst.Enabled = False
        'Birendra : Bug-7528 on 13/03/2013 :Commented:
        'btnEdit.Enabled = True
        btnDelete.Enabled = True
        btnLocate.Enabled = True
        btnNew.Enabled = True
        btnSave.Enabled = False
        TXTDATE.CustomFormat = "dd/MM/yyyy"

        Dim objConnection As SqlConnection = New SqlConnection("Server='" + pServerName + "';Database='" + pComDbnm + "';User ID='" + pUserId + "';Password='" + pPassword + "';")
        Dim reader As SqlDataReader
        Dim ds As DataSet
        Dim ad As SqlDataAdapter
        ad = New SqlDataAdapter("set dateformat dmy select VoucherNo,DisposalDate,Assetcode,PurchaseVal,Depreciation,ValueSold,Narration from assetdisposal", objConnection)
        ds = New DataSet
        Dim date1 As String
        ad.Fill(ds)
        If (ds.Tables(0).Rows.Count <> 0) Then
            txtDCNO.Text = ds.Tables(0).Rows(0)("VoucherNo").ToString
            'date1 = DateTime.ParseExact(ds.Tables(0).Rows(0)("DisposalDate"), "M/d/yyyy", CultureInfo.InvariantCulture).ToString("d/M/yyyy")
            date1 = ds.Tables(0).Rows(0)("DisposalDate")
            TXTDATE.Text = date1
            cmbAssCode.Text = ds.Tables(0).Rows(0)("Assetcode").ToString
            txtPurch.Text = ds.Tables(0).Rows(0)("PurchaseVal").ToString
            txtDepr.Text = ds.Tables(0).Rows(0)("Depreciation").ToString
            txtValSold.Text = ds.Tables(0).Rows(0)("ValueSold").ToString
            txtNarr.Text = ds.Tables(0).Rows(0)("Narration").ToString
        Else
            btnEdit.Enabled = False
            btnDelete.Enabled = False
            btnFirst.Enabled = False
            btnForward.Enabled = False
            btnLast.Enabled = False
            btnBack.Enabled = False
            btnEmail.Enabled = False
            btnLocate.Enabled = False
            'Birendra : Bug-7528 on 14/03/2013 :Start:
            txtDCNO.Clear()
            cmbAssCode.Text = ""
            txtNarr.Clear()
            txtValSold.Clear()
            txtPurch.Clear()
            txtDepr.Clear()
            TXTDATE.Value = Now.Date
            TXTDATE.CustomFormat = " "
            'Birendra : Bug-7528 on 14/03/2013 :End:

        End If
        'Birendra : Bug-7528 on 13/03/2013 :modified condition below one:
        If (ds.Tables(0).Rows.Count <= 1) Then
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
    End Sub
    Private Sub mInsertProcessIdRecord()
        Dim sqlstr As String
        Dim pi As Integer
        pi = Process.GetCurrentProcess().Id
        cAppName = "UeDisposalOfAssets.exe"
        cAppPId = Convert.ToString(Process.GetCurrentProcess().Id)
        sqlstr = "Set DateFormat dmy insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + pPApplCode + "','" + Now.Date.ToString() + "','" + pPApplName + "'," + pPApplPID.ToString() + ",'" + pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + Text.Trim() + "')"
        conn(sqlstr)
    End Sub

    'Added By Pankaj B. on 25.2.2015 for Installer 12.0 start
    Private Sub mDeleteProcessIdRecord()
        If (String.IsNullOrEmpty(pPApplName) Or pPApplPID = 0 Or String.IsNullOrEmpty(cAppName) Or String.IsNullOrEmpty(cAppPId)) Then
            Return
        End If
        Dim sqlstr As String
        sqlstr = " Delete from vudyog..ExtApplLog where pApplNm='" + pPApplName + "' and pApplId=" + pPApplPID.ToString() + " and cApplNm= '" + cAppName + "' and cApplId= " + cAppPId
        conn(sqlstr)
    End Sub
    'Added By Pankaj B. on 25.2.2015 for Installer 12.0 End


    Private Sub cmbAssCode_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAssCode.Leave
        ErrorProvider1.Clear()
    End Sub

    Private Sub GroupBox1_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GroupBox1.Enter

    End Sub

    Private Sub btnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        'Birendra : Bug-7528 on 13/03/2013 :Start:
        'txtNarr.Enabled = True
        'txtValSold.Enabled = True
        'btnSave.Enabled = True
        'btnCancel.Enabled = True
        'btnFirst.Enabled = False
        'btnForward.Enabled = False
        'btnLast.Enabled = False
        'btnBack.Enabled = False
        'btnNew.Enabled = False
        'btnEdit.Enabled = False
        'btnDelete.Enabled = False
        'btnLocate.Enabled = False
        'Birendra : Bug-7528 on 13/03/2013 :End:

    End Sub

    Private Sub Disposal_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
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
                If btnSave.Enabled Then
                    btnSave_Click(sender, exf)
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

    Private Sub Disposal_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
    End Sub
    'Added by Shrikant S. on 14/08/2014 for Bug-23791   Start
    Private Sub txtPurch_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtValSold.KeyDown, txtPurch.KeyDown, txtDepr.KeyDown
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

    Private Sub Disposal_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        mDeleteProcessIdRecord()
    End Sub
End Class