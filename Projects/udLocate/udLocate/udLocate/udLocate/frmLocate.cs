using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataAccess_Net;
using uBaseForm;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraPrinting;
//using Microsoft.Office.Interop.Excel;



namespace udLocate
{
    public partial class frmLocate : uBaseForm.FrmBaseForm
    {
        DataAccess_Net.clsDataAccess oDataAccess;
        System.Data.DataTable tblMain, tblMastCode;
        string SqlStr = string.Empty;
        string vTableNm = string.Empty, vEntry_Ty = string.Empty;
        string vExpression = string.Empty, lFldList = string.Empty;
        string[] arrFldLst;
        string tempTbl = string.Empty;
        //       String cAppPId, cAppName;
        //        Boolean cValid = true;

        public frmLocate(string[] args)
        {
            InitializeComponent();
            this.pPApplPID = 0;
            this.pPara = args;
            this.pFrmCaption = "Locate Records...";
            this.pCompId = Convert.ToInt16(args[0]);
            this.pComDbnm = args[1];
            this.pServerName = args[2];
            this.pUserId = args[3];
            this.pPassword = args[4];
            //this.pPApplRange = args[5];
            //this.pAppUerName = args[6];
            System.Drawing.Icon MainIcon = new System.Drawing.Icon(args[5].Replace("<*#*>", " "));
            this.pFrmIcon = MainIcon;
            this.pPApplText = args[6].Replace("<*#*>", " ");
            this.pPApplName = args[7];
            //this.pPApplPID = Convert.ToInt16(args[10]);
            //this.pPApplCode = args[11];
            tempTbl = args[8];
        }

        private void frmLocate_Load(object sender, EventArgs e)
        {
            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();

            vTableNm = "MastCode";
            vEntry_Ty = "EM";
            SqlStr = "Select * from " + vTableNm + " where code='" + vEntry_Ty + "'";
            tblMastCode = new System.Data.DataTable();
            System.Data.DataTable TemTbl = new System.Data.DataTable();
            tblMastCode = oDataAccess.GetDataTable(SqlStr, null, 20);
            lFldList = tblMastCode.Rows[0]["Locatefld"].ToString().Trim();
            if (String.IsNullOrEmpty(lFldList))
            {
                SqlStr = "Execute Usp_Ent_Get_Column_Description '" + tblMastCode.Rows[0]["FileName"].ToString().Trim() + "'";
                TemTbl = oDataAccess.GetDataTable(SqlStr, null, 20);
                lFldList = TemTbl.Rows[0]["colfilt"].ToString().Trim();
            }
            

            //SqlStr = "Select employeecode,fname,lname from " + tblMastCode.Rows[0]["FileName"].ToString().Trim();
            SqlStr = "Select " + GetFldList(lFldList).Substring(1) + " from " + tblMastCode.Rows[0]["FileName"].ToString().Trim();
            tblMain = new System.Data.DataTable();
            tblMain = oDataAccess.GetDataTable(SqlStr, null, 20);
            dvxGrdLocate.DataSource = tblMain;
            gridview1.OptionsView.ColumnAutoWidth = false;
            gridview1.OptionsPrint.AutoWidth = false;
            gridview1.OptionsBehavior.Editable = false;
            ApplyColCaption(gridview1);
            gridview1.BestFitColumns();
            //dvxGrdLocate.Dock = DockStyle.Fill;
            
        }

        private string GetFldList(string LocateFld)
        {
            if (String.IsNullOrEmpty(LocateFld))
            {
                arrFldLst = null;
                return ",*";
            }
            string[] lfldlst;
            string sFldLst = string.Empty;
            arrFldLst = LocateFld.Split(',');
            foreach (string s in arrFldLst)
            {
                if (s.Contains(':'))
                {
                    lfldlst = s.Split(':');
                    sFldLst = sFldLst + "," + lfldlst[0];
                }
                else
                {
                    sFldLst = sFldLst + "," + s;
                }
            }
            return sFldLst;
        }
        private void ApplyColCaption(GridView gv)
        {
            if (arrFldLst == null)
                return;
            string[] lfldlst;
            foreach (string s in arrFldLst)
            {
                if (s.Contains(':'))
                {
                    lfldlst = s.Split(':');
                    gv.Columns[lfldlst[0].Trim()].Caption = lfldlst[1].ToString();
                }

            }

        }
        private void gridview1_DoubleClick(object sender, EventArgs e)
        {
            int rInd = gridview1.FocusedRowHandle;
            if (rInd >= 0)
            {
                DataRow dr = gridview1.GetDataRow(rInd);
//                MessageBox.Show(dr[tblMastCode.Rows[0]["unique_flds"].ToString().Trim()].ToString());
                cLocate.RetVal = dr[tblMastCode.Rows[0]["unique_flds"].ToString().Trim()].ToString();
                System.Windows.Forms.Application.Exit();
            }
        }

        private void frmLocate_FormClosing(object sender, FormClosingEventArgs e)
        {
            //"if exists (select * from ADMINEMPMST0131052012) begin drop table ADMINEMPMST0131052012 end "
            if (cLocate.RetVal != null)
            {
                SqlStr = "if exists (select * from sysobjects where [name]='" + tempTbl + "') begin drop table ADMINEMPMST0131052012 end ";
                oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);
                SqlStr = " create table " + tempTbl + " (RetValue varchar(100))";
                oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);
                SqlStr = "Insert " + tempTbl + " values('" + cLocate.RetVal.Trim() + "')";
                oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int xs = ((System.Windows.Forms.Button)sender).Location.X;
            if (((System.Windows.Forms.Button)sender).Location.X + contextMenuStrip1.Width > this.Width)
                xs = this.Width - (contextMenuStrip1.Width+10);
            contextMenuStrip1.Show(this, xs, ((System.Windows.Forms.Button)sender).Location.Y + ((System.Windows.Forms.Button)sender).Height);
        }

        private void excelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            switch (((ToolStripItem)sender).Text)
            {
                case "EXCEL(XLS)":
                    sfd.Title = "Save as Excel";
                    sfd.DefaultExt = "xls";
                    sfd.Filter = "*.xls|*.xls";
                    break;
                case "EXCEL(XLSX)":
                    sfd.Title = "Save as Excel";
                    sfd.DefaultExt = "xlsx";
                    sfd.Filter = "*.xlsx|*.xlsx";
                    break;
                case "PDF":
                    sfd.Title = "Save as PDF";
                    sfd.DefaultExt = "pdf";
                    sfd.Filter = "*.pdf|*.pdf";
                    break;
                case "RTF":
                    sfd.Title = "Save as rtf";
                    sfd.DefaultExt = "rtf";
                    sfd.Filter = "*.rtf|*.rtf";
                    break;
                case "HTML":
                    sfd.Title = "Save as html";
                    sfd.DefaultExt = "html";
                    sfd.Filter = "*.html|*.html";
                    break;
            }

            //sfd.DefaultExt = "xls";
            //sfd.Filter = "*.xls|*.xls|*.xlsx|*.xlsx|*.pdf|*.pdf|*.rtf|*.rtf";
            sfd.FileName = "ExportData";

            if (sfd.ShowDialog() != DialogResult.OK && sfd.FileName != "")
                return;

            //PrintableComponentLink link = new PrintableComponentLink(new PrintingSystem());
            //link.PaperKind = System.Drawing.Printing.PaperKind.A4;
            //link.Component = gridControl1;
            //link.Landscape = true;

            switch (((ToolStripItem)sender).Text)
            {
                case "EXCEL(XLS)":
                    XlsExportOptions _Options1 = new XlsExportOptions();
                    _Options1.SheetName = sfd.FileName;
                    _Options1.ExportMode = XlsExportMode.SingleFile;
                    dvxGrdLocate.ExportToXls(sfd.FileName, _Options1);
                    //Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                    //app.Visible = true;
                    //File.OpenWrite(sfd.FileName);
                    

                    break;
                case "EXCEL(XLSX)":
                    XlsxExportOptions _Options2 = new XlsxExportOptions();
                    _Options2.SheetName = sfd.FileName;
                    _Options2.ExportMode = XlsxExportMode.SingleFile;
                    _Options2.TextExportMode = TextExportMode.Text;
                    dvxGrdLocate.ExportToXlsx(sfd.FileName, _Options2);

                    //dvxGrdLocate.ExportToXlsx(sfd.FileName);
                    //Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                    //app.Visible = true;
                    //app.Workbooks.Open(Path.GetFullPath(sfd.FileName), 0, false, 5, string.Empty, string.Empty, true, XlPlatform.xlWindows, "\t",
                    //               true, false, 0, false, false, XlCorruptLoad.xlNormalLoad);
                    break;
                case "PDF":
                    PdfExportOptions _Options3 = new PdfExportOptions();
                    dvxGrdLocate.ExportToPdf(sfd.FileName);
                    break;
                case "RTF":
                    RtfExportOptions _Options4 = new RtfExportOptions();
                    _Options4.ExportMode = RtfExportMode.SingleFile;
                    dvxGrdLocate.ExportToRtf(sfd.FileName);
                    break;
                case "HTML":
                    RtfExportOptions _Options5 = new RtfExportOptions();
                    _Options5.ExportMode = RtfExportMode.SingleFile;
                    dvxGrdLocate.ExportToHtml(sfd.FileName);
                    break;
            }

        }


        private void gridview1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                gridview1_DoubleClick(sender, new EventArgs());
        }


    }
}
