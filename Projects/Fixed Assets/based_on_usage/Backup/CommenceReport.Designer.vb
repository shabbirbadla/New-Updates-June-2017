<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CommenceReport
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
        Me.Frmdate = New System.Windows.Forms.DateTimePicker
        Me.ToDate = New System.Windows.Forms.DateTimePicker
        Me.btnProceed = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Times New Roman", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(22, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 17)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "From Date"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Times New Roman", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(268, 19)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 17)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "To Date"
        '
        'Frmdate
        '
        Me.Frmdate.CustomFormat = ""
        Me.Frmdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.Frmdate.Location = New System.Drawing.Point(112, 19)
        Me.Frmdate.Name = "Frmdate"
        Me.Frmdate.Size = New System.Drawing.Size(131, 20)
        Me.Frmdate.TabIndex = 2
        Me.Frmdate.Value = New Date(2012, 4, 1, 0, 0, 0, 0)
        '
        'ToDate
        '
        Me.ToDate.CustomFormat = ""
        Me.ToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.ToDate.Location = New System.Drawing.Point(335, 19)
        Me.ToDate.Name = "ToDate"
        Me.ToDate.Size = New System.Drawing.Size(130, 20)
        Me.ToDate.TabIndex = 3
        Me.ToDate.Value = New Date(2012, 4, 30, 0, 0, 0, 0)
        '
        'btnProceed
        '
        Me.btnProceed.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnProceed.Location = New System.Drawing.Point(378, 60)
        Me.btnProceed.Name = "btnProceed"
        Me.btnProceed.Size = New System.Drawing.Size(87, 35)
        Me.btnProceed.TabIndex = 4
        Me.btnProceed.Text = "Proceed"
        Me.btnProceed.UseVisualStyleBackColor = True
        '
        'CommenceReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(490, 115)
        Me.Controls.Add(Me.btnProceed)
        Me.Controls.Add(Me.ToDate)
        Me.Controls.Add(Me.Frmdate)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Name = "CommenceReport"
        Me.Text = "Depreciation Schedule Report"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Frmdate As System.Windows.Forms.DateTimePicker
    Friend WithEvents ToDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents btnProceed As System.Windows.Forms.Button
End Class
