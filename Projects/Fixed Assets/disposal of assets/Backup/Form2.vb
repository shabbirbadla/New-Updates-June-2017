Public Class Form2

    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'TODO: This line of code loads data into the 'DataSet1.TEMP1' table. You can move, or remove it, as needed.
        Me.TEMP1TableAdapter.Fill(Me.DataSet1.TEMP1) 
        Me.ReportViewer1.RefreshReport()
    End Sub
End Class