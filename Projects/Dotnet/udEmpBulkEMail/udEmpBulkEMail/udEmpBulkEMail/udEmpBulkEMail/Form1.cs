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

namespace udEmpBulkEMail
{
    public partial class Form1 : Form
    {
        DataSet dsDataSet;
        System.Data.DataTable tResultSet;
        public Form1()
        {
            InitializeComponent();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnEMail_Click(object sender, EventArgs e)
        {
            string veMailId = "rupeshprajapati@udyogsoftware.com";
            veMailId = "vasant@udyogsoftware.com";
            int mailCount;
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
            mailCount = 0;
            foreach (DataRow dr in tResultSet.Rows)
            {
                if (dr["SendMail"].ToString().Trim().ToLower() == "y")

                {

                    try
                    {
                        

                        Microsoft.Office.Interop.Outlook.Application oOutLookApp = new Microsoft.Office.Interop.Outlook.Application();
                        Microsoft.Office.Interop.Outlook.MailItem oMsg;   //                oOutLookApp .CreateItem ( Microsoft.Office.Interop.Outlook.MailItem);

                        oMsg = (Microsoft.Office.Interop.Outlook.MailItem)oOutLookApp.CreateItem(OlItemType.olMailItem);   // Convert.tooOutLookApp.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
                        oMsg.Subject = rtxtSubject.Text.Trim();//tResultSet.Rows[0]["EMailSub"].ToString();

                        oMsg.Body = rtxtEmailBody.Text.Replace("@@Name", dr["Name"].ToString()).Trim();//tResultSet.Rows[0]["EmailBody"].ToString();

                        oMsg.To = dr["To"].ToString();
                        oMsg.CC = dr["CC"].ToString();
                        oMsg.BCC = dr["BCC"].ToString();

                        //   Microsoft.Office.Interop.Outlook.Inspector insp;
                        //    insp = oMsg.GetInspector;


                        //    string sDisplayName = "", sBodyLen = oMsg.Body.Length.ToString();
                        //    Microsoft.Office.Interop.Outlook.Attachments oAttachs;
                        //    oAttachs = oMsg.Attachments;  // = oMsg.Attachments;
                        //    Microsoft.Office.Interop.Outlook.Attachment oAttach;
                        //    oAttachs = (Microsoft.Office.Interop.Outlook.Attachments)oMsg.Attachments;
                        //    //oAttach = oAttachs.Add(sSource, , sBodyLen + 1, sDisplayName);
                        //    oAttach = (Microsoft.Office.Interop.Outlook.Attachment)oAttachs.Add(vPdfFileNm, null, sBodyLen + 1, sDisplayName);

                            oMsg.Send();
                        //    //MessageBox.Show("Mail Sent");
                        //    //oAttach =(Microsoft.Office.Interop.Outlook.MailItem) oAttachs.Add(vPdfFileNm, null, sBodyLen + 1, sDisplayName);
                        //    //oMsg.Send();
                            mailCount++;
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            MessageBox.Show(mailCount.ToString().Trim() + " Email(s) Send");
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "BullMailFormat";
            openFileDialog1.Filter="Excel Files |*.xlsx";

            if (openFileDialog1.ShowDialog()==DialogResult.OK)
            {
                txtFileName.Text = openFileDialog1.FileName;
                txtFileName.Refresh();

            ////string myConnection = "Provider='Microsoft.ACE.OLEDB.12.0';Data Source=D:\\SalaryData.xlsx;Extended Properties=Excel 12.0 ";
            //string myConnection = "Provider='Microsoft.ACE.OLEDB.12.0';Data Source="+@txtFileName.Text.Trim()+ ";Extended Properties=Excel 12.0 ";
            //OleDbConnection conn = new OleDbConnection(myConnection);
            //string strSQL = "SELECT * FROM [Sheet1$]";
            //OleDbCommand cmd = new OleDbCommand(strSQL, conn);
            //dsDataSet = new DataSet(); 
            //OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
            //adapter.Fill(dsDataSet);
            //tResultSet = dsDataSet.Tables[0];
            }
            //for (int rowcount = 2; rowcount < dt.Rows.Count; rowcount++)
            
        }
    }
}
