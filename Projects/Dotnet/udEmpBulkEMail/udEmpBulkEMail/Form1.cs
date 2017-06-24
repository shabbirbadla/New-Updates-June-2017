using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using Microsoft.Office.Interop.Outlook;

using System.Threading;
using System.Reflection;
using System.Globalization;

namespace udEmpBulkEMail
{
    public partial class Form1 : Form
    {
        DataSet dsDataSet;
        System.Data.DataTable tResultSet;
        public Form1()
        {
            InitializeComponent();
            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnEMail_Click(object sender, EventArgs e)
        {
            string veMailId = "rupeshprajapati@udyogsoftware.com";
            
            if (!String.IsNullOrEmpty(txtFileName.Text))
            {

                //string myConnection = "Provider='Microsoft.ACE.OLEDB.12.0';Data Source=D:\\SalaryData.xlsx;Extended Properties=Excel 12.0 ";
                string myConnection = "Provider='Microsoft.ACE.OLEDB.12.0';Data Source=" + @txtFileName.Text.Trim() + ";Extended Properties=Excel 12.0 ";
                OleDbConnection conn = new OleDbConnection(myConnection);
                string strSQL = "SELECT * FROM [Sheet1$]";
                OleDbCommand cmd = new OleDbCommand(strSQL, conn);
                dsDataSet = new DataSet();
                OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                adapter.Fill(dsDataSet);
                tResultSet = dsDataSet.Tables[0];
            }
            else
            {
                MessageBox.Show("File Path Not Found");
                return;
            }

            foreach (DataRow dr in tResultSet.Rows)
            {
                if (dr["SendMail"].ToString().Trim().ToLower() == "y")

                {

                    try
                    {
                        

                        Microsoft.Office.Interop.Outlook.Application oOutLookApp = new Microsoft.Office.Interop.Outlook.Application();
                        Microsoft.Office.Interop.Outlook.MailItem oMsg;   //               

                        oMsg = (Microsoft.Office.Interop.Outlook.MailItem)oOutLookApp.CreateItem(OlItemType.olMailItem);   
                        oMsg.Subject = rtxtSubject.Text.Trim();

                        //oMsg.Body = rtxtEmailBody.Text.Replace("@@Name", dr["Name"].ToString()).Trim();//tResultSet.Rows[0]["EmailBody"].ToString();
                        string tMsg;
                        tMsg = rtxtEmailBody.Text.Replace("@@Name", dr["Name"].ToString()).Trim();
                        tMsg = tMsg.Replace("@@Request", dr["Request"].ToString()).Trim();
                        tMsg = tMsg.Replace("@@Amount", dr["Amount"].ToString()).Trim();
                        tMsg = tMsg.Replace("@@FromDate", dr["FromDate"].ToString()).Trim();
                        tMsg = tMsg.Replace("@@ToDate", dr["ToDate"].ToString()).Trim();
                        tMsg = tMsg.Replace("@@ClientName", dr["ClientName"].ToString()).Trim();
                        tMsg = tMsg.Replace("@@BankName", dr["BankName"].ToString()).Trim();
                        oMsg.Body = tMsg.Replace("12:00:00 AM", "").Trim();
                        

                        oMsg.To = dr["To"].ToString();
                        oMsg.CC = dr["CC"].ToString();
                        oMsg.BCC = dr["BCC"].ToString();

                         oMsg.Send();
                         
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    MessageBox.Show("Message Sent to Outlook");
                    System.Windows.Forms.Application.Exit();
                }
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "BullMailFormat";
            openFileDialog1.Filter="Excel Files |*.xlsx";

            if (openFileDialog1.ShowDialog()==DialogResult.OK)
            {
                txtFileName.Text = openFileDialog1.FileName;
                txtFileName.Refresh();

          
            }
          
            
        }
    }
}
