Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.Reporting.WinForms

Public Class CommenceReport
    Public it_name, method_nm As String
    Public noofyr, u_deprper, pval, depr, add1, del1, ads, balpval, stdays As Integer
    Public sperdep As Double
    Public cmdate As Date
    Private Sub btnProceed_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProceed.Click

        Dim objConnection As SqlConnection = New SqlConnection("Server='" + pServerName + "';Database='" + pComDbnm + "';User ID=sa;Password=sa1985;")
        Dim command As SqlCommand = New SqlCommand()
        command.Connection = objConnection

        Dim objConnection1 As SqlConnection = New SqlConnection("Server='" + pServerName + "';Database='" + pComDbnm + "';User ID=sa;Password=sa1985;")
        Dim command1 As SqlCommand = New SqlCommand()
        command1.Connection = objConnection1
        sqlstr = "delete from temp1"
        conn(sqlstr)
        Dim diffdays(0 To 2), totaldays(0 To 2), noyear As Long
        Dim cnt, fval As Integer
        ' Dim DAA As Date
        Dim i As Integer
        Dim forma As String
        Dim date1(0 To 3) As Date
        ' Dim str1 As String
        Dim reader, reader1 As SqlDataReader
        command.Connection = objConnection
        command.CommandText = "select * from it_mast"
        objConnection.Open()
        reader = command.ExecuteReader()
       

        i = 0
        cnt = 0
        'DAA = "/4/"
        If (reader.HasRows) Then
            
            While (reader.Read())
                forma = reader.GetString(52)
                If ((Trim(reader.GetString(52)) = "WDV")) Then
                    totaldays(i) = DateDiff(DateInterval.Day, CDate(reader.GetDateTime(55)), CDate(ToDate.Text), FirstDayOfWeek.Sunday)
                    diffdays(i) = DateDiff(DateInterval.Day, CDate(Frmdate.Text), CDate(ToDate.Text), FirstDayOfWeek.Sunday)
                    'MsgBox("total days" & totaldays(i))
                    cnt = 0
                    noyear = totaldays(i)
                    Do Until noyear <= 0
                        noyear = noyear - 365
                        If (noyear < 0 And cnt = 0) Then
                            cnt = 1
                        Else
                            cnt = cnt + 1
                        End If
                    Loop
                    noyear = DateDiff(DateInterval.Year, CDate(reader.GetDateTime(55)), CDate(ToDate.Text), FirstDayOfWeek.Sunday)
                    'MsgBox("no of year" & cnt)
                    pval = reader.GetDecimal(56)
                    u_deprper = reader.GetDecimal(54)
                    If (cnt > 1) Then
                        totaldays(i) = totaldays(i) - (365 * (cnt - 1))
                        '    diffdays(i) = diffdays(i) - (365 * (cnt - 1))
                    End If
                    fval = 0
                    Do Until cnt <= 0
                        pval = pval - fval
                        fval = pval * (u_deprper / 100)
                        cnt = cnt - 1
                    Loop
                    'MsgBox("pval " & pval)
                    'MsgBox("fval " & fval)
                    'ComboBox1.Items.Add(totaldays(i))
                    it_name = reader.GetString(0)
                    method_nm = reader.GetString(52)
                    noofyr = reader.GetDecimal(53)
                    u_deprper = reader.GetDecimal(54)
                    cmdate = CDate(reader.GetDateTime(55))
                    'pval = reader.GetDecimal(56)
                    ''MsgBox(totaldays(i))
                    ''MsgBox(diffdays(i))
                    If (diffdays(i) <= 0) Then
                        Exit While
                    End If
                    If (totaldays(i) <= 182) Then
                        depr = (((pval * u_deprper) / 100) / 2)
                    Else
                        depr = ((pval * u_deprper) / 100)
                    End If
                    Call depr_insert()
                    'objConnection.Close()
                End If

                If ((Trim(reader.GetString(52)) = "STRAIGHT LINE")) Then
                    totaldays(i) = DateDiff(DateInterval.Day, CDate(reader.GetDateTime(55)), CDate(ToDate.Text), FirstDayOfWeek.Sunday)
                    diffdays(i) = DateDiff(DateInterval.Day, CDate(Frmdate.Text), CDate(ToDate.Text), FirstDayOfWeek.Sunday)
                    'MsgBox("total days" & totaldays(i))
                    noyear = totaldays(i)
                    Do Until noyear <= 0
                        noyear = noyear - 365
                        If (noyear < 0 And cnt = 0) Then
                            cnt = 1
                        Else
                            cnt = cnt + 1
                        End If
                    Loop
                    pval = reader.GetDecimal(56)
                    u_deprper = reader.GetDecimal(54)
                    'If (cnt > 1) Then
                    '    totaldays(i) = totaldays(i) - (365 * (cnt - 1))
                    '    '    diffdays(i) = diffdays(i) - (365 * (cnt - 1))
                    'End If
                    'fval = 0
                    'Do Until cnt <= 0
                    '    pval = pval - fval
                    '    fval = pval * (u_deprper / 100)
                    '    cnt = cnt - 1
                    'Loop
                    'MsgBox("pval " & pval)
                    'MsgBox("fval " & fval)
                    'ComboBox1.Items.Add(totaldays(i))
                    it_name = reader.GetString(0)
                    method_nm = reader.GetString(52)
                    noofyr = reader.GetDecimal(53)
                    u_deprper = reader.GetDecimal(54)
                    cmdate = CDate(reader.GetDateTime(55))
                    pval = reader.GetDecimal(56)
                    ''MsgBox(totaldays(i))
                    ''MsgBox(diffdays(i))
                    If (diffdays(i) <= 0) Then
                        Exit While
                    End If
                    If (totaldays(i) < diffdays(i)) Then
                        stdays = 365 * noofyr
                        sperdep = pval / stdays
                        ' MsgBox(sperdep)
                        depr = totaldays(i) * sperdep
                    Else
                        stdays = 365 * noofyr
                        sperdep = pval / stdays
                        ' MsgBox(sperdep)
                        depr = diffdays(i) * sperdep
                    End If
                    Call depr_insert()
                End If
                ' MsgBox(Trim(reader.GetString(52)))
                If ((Trim(reader.GetString(52)) = "BASED ON USAGE")) Then
                    depr = 0
                    it_name = reader.GetString(0)
                    method_nm = reader.GetString(52)
                    noofyr = reader.GetDecimal(53)
                    u_deprper = reader.GetDecimal(54)
                    cmdate = CDate(reader.GetDateTime(55))
                    pval = reader.GetDecimal(56)
                    'reader.Cl
                    ' objConnection.Close()
                    ToDate.CustomFormat = "yyyy/MM/dd"
                    ToDate.Format = DateTimePickerFormat.Custom
                    'cmdate = String.Format("{0,yyyy/MM/dd}", ToDate.Text)
                    command1.CommandText = "select sum(depr) as depr from bs_usage where assetcode='" & it_name & "' and u_date <= '" + ToDate.Text + "'"
                    objConnection1.Open()
                    reader1 = command1.ExecuteReader()
                    'If (reader.HasRows) Then
                    ' MsgBox("reader.GetDateTime(1)" & reader.GetDateTime(1))
                    'If (reader.GetDateTime(1) <= CDate(ToDate.Text)) Then
                    reader1.Read()
                    depr = reader1.GetDecimal(0)
                    Call depr_insert()
                    reader1.Close()
                    objConnection1.Close()
                End If


            End While
        End If

        Form2.Show()
        'Me.Close()

    End Sub

    'Public Function depr_insert()
    'End Function
    Sub depr_insert()
        sqlstr = "insert into temp1(IT_NAME1,U_METHODNM,U_NOOFYR,U_DEPRPER,U_PVALUE,Depreciation,BALPVAL) values('" + it_name + "','" + method_nm + "'," & noofyr & "," & u_deprper & "," & pval & "," & depr & "," & balpval & ")"
        conn(sqlstr)

    End Sub

    Private Sub CommenceReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
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
    End Sub
End Class