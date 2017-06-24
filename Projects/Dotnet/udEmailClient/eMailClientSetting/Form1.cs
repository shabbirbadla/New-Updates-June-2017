using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using eMailClient.RPT;
using eMailClient.BLL;


namespace eMailClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //cls_Gen_Mgr_Report GenReport = new cls_Gen_Mgr_Report();
            //GenReport.ReportPath = "\\\\pro_priyanka\\itax2\\Report\\Report1.rpt";
            ////GenReport.SqlSrvName = "pro_priyanka\\avery2008r2";
            ////GenReport.SqlUsrName = "sa";
            ////GenReport.SqlPasswd =  "sa1985";
            //GenReport.DskFileName = "C:\\temp\\Test.txt";
            //GenReport.Sqlstring = " Select [Party Name] = Stmain.party_nm, [Site] = u_mfgsite, [Invoice No.] = Stmain.inv_no, [Invoice Date] = Stmain.[date], [Sales Order no.] = u_sono, [Customer PO No.] = u_pono," +
            //                      " [Item Code] = Stitem.it_code, [Item Description] = it_desc, [Invoice Qty] = qty, UOM = u_uom, [No. of Sheets] = '', [Mode of Shipment] = u_tmode," +
            //                      " [Docket No.] = '', [Driver's Mob No.] = '', [Transporter Name] = u_deli, [Expected Date of Delivery] = ''" +
            //                      " From Stmain" +
            //                      " Inner Join Stitem on (Stmain.entry_ty = Stitem.entry_ty And Stmain.tran_cd = Stitem.tran_cd)" +
            //                      " Inner Join It_mast on (Stitem.it_code = It_Mast.it_code)" +
            //                      " Where Stmain.party_nm='A90868' and Stmain.[date]='01-30-2013'";
            //GenReport.ReportExportType = CrystalDecisions.Shared..ExportFormatType.RichText;

            //GenReport.GenReport();

            //cls_Gen_Mgr_ProcExecute m_obj_ProcExecute = new cls_Gen_Mgr_ProcExecute();
            //m_obj_ProcExecute.LogFilePath = Application.StartupPath;
            //m_obj_ProcExecute.ExecuteProcess();

            
            //LogWriter.WriteLogToTextFile("\\\\pro_priyanka\\itax2\\log\\", "Task has been completed !!!");
            //LogWriter.WriteLogToTextFile("\\\\pro_priyanka\\itax2\\log\\", "Test Task1!!!");
            //LogWriter.WriteLogToTextFile("\\\\pro_priyanka\\itax2\\log\\", "Test Task 2 !!!");
            //cls_Gen_Mgr_MailSender obj_MailSender = new cls_Gen_Mgr_MailSender();
            //obj_MailSender.ToMailAddress = "uday@udyogsoftware.com";
            //obj_MailSender.CcMailAddress = "priyanka_himane@udyogsoftware.com";


            //string logsummery;
            //logsummery = "\n\n-------------------------------------<<< Summary >>>-----------------------------" +
            //             "\n\nTotal Records found : " + Convert.ToString(1).Trim() + " " +
            //             "\n\nTotal Exported Paf : " + Convert.ToString(1).Trim() + " " +
            //             "\n\nTotal Mails sent : " + Convert.ToString(1).Trim() + " " +
            ////             "\n\n---------------------------------------<<< End >>> ------------------------------";

            //LogWriter.WriteLogToTextFile(Application.StartupPath, logsummery,true);

            string LogFilePath = Application.StartupPath;
            LogWriter.WriteLogToTextFile(LogFilePath, "-------------------------------------<<< Summary >>>-----------------------------", true);
            LogWriter.WriteLogToTextFile(LogFilePath, "Total Records found : " + Convert.ToString(2).Trim(), true);
            LogWriter.WriteLogToTextFile(LogFilePath, "Total Exported PDF : " + Convert.ToString(2).Trim(), true);
            LogWriter.WriteLogToTextFile(LogFilePath, "Total Mails sent : " + Convert.ToString(2).Trim(), true);
            LogWriter.WriteLogToTextFile(LogFilePath, "---------------------------------------<<< End >>> ------------------------------", true);

        }
    }
}
