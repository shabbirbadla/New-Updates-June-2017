using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using System.Data.OleDb;
namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        Microsoft.Office.Interop.Excel.Application xlApp;
        Microsoft.Office.Interop.Excel.Workbook xlWorkbook;
        Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
        object misValue = System.Reflection.Missing.Value;
        string paramExportFilePath = String.Empty;
        public Form1()
        {
            InitializeComponent();
        }

        
        private void SaveToPDf()
        {
            //string paramExportFilePath = @"C:\Temp\Test.pdf";
            XlFixedFormatType paramExportFormat = XlFixedFormatType.xlTypePDF;
            XlFixedFormatQuality paramExportQuality =
                XlFixedFormatQuality.xlQualityStandard;
            bool paramOpenAfterPublish = false;
            bool paramIncludeDocProps = true;
            bool paramIgnorePrintAreas = true;
            object paramFromPage = Type.Missing;
            object paramToPage = Type.Missing;


            try
            {


                // Save it in the target format.
                if (xlWorkbook != null)
                    xlWorkbook.ExportAsFixedFormat(paramExportFormat,
                        paramExportFilePath, paramExportQuality,
                        paramIncludeDocProps, paramIgnorePrintAreas, paramFromPage,
                        paramToPage, paramOpenAfterPublish,
                        misValue);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                // Close the workbook object, Quit Excel and release the ApplicationClass object.
                if (xlWorkbook != null)
                {
                    xlWorkbook.Close(false, misValue, misValue);
                    xlWorkbook = null;
                    xlApp.Quit();
                    xlApp = null;
                }
                               
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (File.Exists(@"D:\SalaryData.xlsx") & File.Exists(@"D:\Form16.xls"))
            {
                label1.Text = @"D:\SalaryData.xlsx";
                label2.Text = @"D:\Form16.xls";
                label3.Text = "Idle";
            }
            else
            {
                label1.Text = @"File Not found 'D:\SalaryData.xlsx'";
                label2.Text = @"File Not Found 'D:\Form16.xls'";
                button3.Enabled = false;
                label3.Text = "Either or both above file are missing. \r\rCan't Process...";
                label3.ForeColor = Color.Red; 
                return;
            }
            if (!Directory.Exists(@"D:\Form16"))
            {
                label3.Text = "PDF Location Not Found. \r\rCan't Process...";
                label3.ForeColor = Color.Red; 
                return;
            }
            else
                label3.ForeColor = Color.Black; 
            string myConnection = "Provider='Microsoft.ACE.OLEDB.12.0';Data Source=D:\\SalaryData.xlsx;Extended Properties=Excel 12.0 ";
            OleDbConnection conn = new OleDbConnection(myConnection);
            string strSQL = "SELECT * FROM [Tax$]";
            OleDbCommand cmd = new OleDbCommand(strSQL, conn);   
            DataSet dataset = new DataSet(); 
            OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
            adapter.Fill(dataset);
            System.Data.DataTable dt = dataset.Tables[0];
            for (int rowcount = 2; rowcount < dt.Rows.Count; rowcount++)
            {
                xlApp = new Microsoft.Office.Interop.Excel.ApplicationClass();
                xlWorkbook = xlApp.Workbooks.Open(@"D:\Form16.xls", 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.get_Item(1);
                label3.Text = "Generating for " + dataset.Tables[0].Rows[rowcount][0].ToString().Trim() + " (" + (rowcount+1).ToString().Trim()+"/"+dataset.Tables[0].Rows.Count.ToString().Trim()+")";
                label3.Refresh();
                paramExportFilePath = @"D:\Form16\Form16_" + dataset.Tables[0].Rows[rowcount][1].ToString().Trim() + "_" + dataset.Tables[0].Rows[rowcount][0].ToString().Trim() + ".pdf";
                
                    if (!String.IsNullOrEmpty(dataset.Tables[0].Rows[rowcount][0].ToString()))
                    {
                        xlWorkSheet.Cells[7, 5] = dataset.Tables[0].Rows[rowcount][0].ToString();
                        xlWorkSheet.Cells[11, 5] = dataset.Tables[0].Rows[rowcount][4].ToString();
                        xlWorkSheet.Cells[27, 5] = dataset.Tables[0].Rows[rowcount][5].ToString();
                        xlWorkSheet.Cells[32, 5] = dataset.Tables[0].Rows[rowcount][6].ToString();
                        xlWorkSheet.Cells[33, 5] = dataset.Tables[0].Rows[rowcount][7].ToString();
                        xlWorkSheet.Cells[34, 5] = dataset.Tables[0].Rows[rowcount][8].ToString();
                        xlWorkSheet.Cells[38, 5] = dataset.Tables[0].Rows[rowcount][11].ToString();
                        xlWorkSheet.Cells[40, 5] = dataset.Tables[0].Rows[rowcount][12].ToString();
                        xlWorkSheet.Cells[50, 5] = dataset.Tables[0].Rows[rowcount][15].ToString();
                        xlWorkSheet.Cells[51, 5] = dataset.Tables[0].Rows[rowcount][16].ToString();
                        xlWorkSheet.Cells[52, 5] = dataset.Tables[0].Rows[rowcount][17].ToString();
                        xlWorkSheet.Cells[53, 5] = dataset.Tables[0].Rows[rowcount][18].ToString();
                        xlWorkSheet.Cells[54, 5] = dataset.Tables[0].Rows[rowcount][19].ToString();
                        xlWorkSheet.Cells[55, 5] = dataset.Tables[0].Rows[rowcount][20].ToString();
                        xlWorkSheet.Cells[56, 5] = dataset.Tables[0].Rows[rowcount][21].ToString();
                        xlWorkSheet.Cells[57, 5] = dataset.Tables[0].Rows[rowcount][22].ToString();
                        xlWorkSheet.Cells[58, 5] = dataset.Tables[0].Rows[rowcount][23].ToString();
                        xlWorkSheet.Cells[71, 5] = dataset.Tables[0].Rows[rowcount][26].ToString();
                        SaveToPDf();
                    }
            }
            label3.Text = label3.Text + "  Finished Processing.";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(@"D:\SalaryData.xlsx") & File.Exists(@"D:\Form16.xls"))
            {
                label1.Text = @"D:\SalaryData.xlsx";
                label2.Text = @"D:\Form16.xls";
                label3.Text = "Idle";
            }
            else
            {
                label1.Text = @"File Not found 'D:\SalaryData.xlsx'";
                label2.Text = @"File Not Found 'D:\Form16.xls'";
                button3.Enabled = false;
                label3.Text = "Either or both above file are missing. \r\rCan't Process...";
            }
            
        } 
    }
}
