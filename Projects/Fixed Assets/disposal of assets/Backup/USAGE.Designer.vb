<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class USAGE
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.txtDCNO = New System.Windows.Forms.TextBox
        Me.TXTDATE = New System.Windows.Forms.DateTimePicker
        Me.txtUnit = New System.Windows.Forms.TextBox
        Me.txtPunit = New System.Windows.Forms.TextBox
        Me.txtNunit = New System.Windows.Forms.TextBox
        Me.btnsave = New System.Windows.Forms.Button
        Me.cmbAssCode = New System.Windows.Forms.ComboBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.txtlife = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Times New Roman", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(51, 45)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(91, 17)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Document No"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Times New Roman", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(51, 78)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(37, 17)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Date"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Times New Roman", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(51, 111)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(77, 17)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Asset Code"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Times New Roman", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(51, 144)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(75, 17)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Usage Unit"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Times New Roman", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(51, 209)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(88, 17)
        Me.Label6.TabIndex = 8
        Me.Label6.Text = "Previous Unit"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Times New Roman", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(53, 245)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(66, 17)
        Me.Label7.TabIndex = 9
        Me.Label7.Text = "New Unit"
        '
        'txtDCNO
        '
        Me.txtDCNO.Enabled = False
        Me.txtDCNO.Location = New System.Drawing.Point(197, 44)
        Me.txtDCNO.Name = "txtDCNO"
        Me.txtDCNO.Size = New System.Drawing.Size(135, 20)
        Me.txtDCNO.TabIndex = 10
        '
        'TXTDATE
        '
        Me.TXTDATE.CustomFormat = "dd/MM/yyyy"
        Me.TXTDATE.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.TXTDATE.Location = New System.Drawing.Point(197, 76)
        Me.TXTDATE.Name = "TXTDATE"
        Me.TXTDATE.Size = New System.Drawing.Size(134, 20)
        Me.TXTDATE.TabIndex = 11
        '
        'txtUnit
        '
        Me.txtUnit.Location = New System.Drawing.Point(197, 142)
        Me.txtUnit.Name = "txtUnit"
        Me.txtUnit.Size = New System.Drawing.Size(183, 20)
        Me.txtUnit.TabIndex = 13
        '
        'txtPunit
        '
        Me.txtPunit.Enabled = False
        Me.txtPunit.Location = New System.Drawing.Point(197, 208)
        Me.txtPunit.Name = "txtPunit"
        Me.txtPunit.Size = New System.Drawing.Size(100, 20)
        Me.txtPunit.TabIndex = 14
        '
        'txtNunit
        '
        Me.txtNunit.Location = New System.Drawing.Point(197, 244)
        Me.txtNunit.Name = "txtNunit"
        Me.txtNunit.Size = New System.Drawing.Size(100, 20)
        Me.txtNunit.TabIndex = 15
        '
        'btnsave
        '
        Me.btnsave.Location = New System.Drawing.Point(150, 277)
        Me.btnsave.Name = "btnsave"
        Me.btnsave.Size = New System.Drawing.Size(75, 23)
        Me.btnsave.TabIndex = 16
        Me.btnsave.Text = "Save"
        Me.btnsave.UseVisualStyleBackColor = True
        '
        'cmbAssCode
        '
        Me.cmbAssCode.FormattingEnabled = True
        Me.cmbAssCode.Location = New System.Drawing.Point(197, 109)
        Me.cmbAssCode.Name = "cmbAssCode"
        Me.cmbAssCode.Size = New System.Drawing.Size(183, 21)
        Me.cmbAssCode.TabIndex = 17
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Times New Roman", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(51, 177)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(140, 17)
        Me.Label5.TabIndex = 18
        Me.Label5.Text = "Estimated Usefull Life"
        '
        'txtlife
        '
        Me.txtlife.Location = New System.Drawing.Point(197, 176)
        Me.txtlife.Name = "txtlife"
        Me.txtlife.Size = New System.Drawing.Size(100, 20)
        Me.txtlife.TabIndex = 19
        '
        'USAGE
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(390, 312)
        Me.Controls.Add(Me.txtlife)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.cmbAssCode)
        Me.Controls.Add(Me.btnsave)
        Me.Controls.Add(Me.txtNunit)
        Me.Controls.Add(Me.txtPunit)
        Me.Controls.Add(Me.txtUnit)
        Me.Controls.Add(Me.TXTDATE)
        Me.Controls.Add(Me.txtDCNO)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Name = "USAGE"
        Me.Text = "Based On Usage"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtDCNO As System.Windows.Forms.TextBox
    Friend WithEvents TXTDATE As System.Windows.Forms.DateTimePicker
    Friend WithEvents txtUnit As System.Windows.Forms.TextBox
    Friend WithEvents txtPunit As System.Windows.Forms.TextBox
    Friend WithEvents txtNunit As System.Windows.Forms.TextBox
    Friend WithEvents btnsave As System.Windows.Forms.Button
    Friend WithEvents cmbAssCode As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtlife As System.Windows.Forms.TextBox
End Class
