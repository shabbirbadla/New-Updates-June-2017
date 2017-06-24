using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraPrinting;
using System.IO;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.Printing.Core;
using DevExpress.Utils.Serializing.Helpers;


namespace UdyogMaterialRequirementPlanning
{
    public partial class frmMRPPlan1 : Form
    {
        private DataTable PendingData;
        //DataAccess_Net.clsDataAccess oDataAccess;
        string BomNotDefined ;
        string BommID; //ruchit

        public frmMRPPlan1(DataTable pendingData)
        {
            InitializeComponent();
            PendingData = pendingData;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            this.label2.Visible = true;
            this.label2.Text = "Please wait...";
            this.Refresh();
            this.UpdateBomDetails();
            
        //    if (BomNotDefined.Trim().Length > 0)
        //    {
        //        DialogResult result = MessageBox.Show("BOM not attached to the item(s): " + BomNotDefined.Trim().Substring(1).Trim() +Environment.NewLine+ "Do you want to continue?", "", MessageBoxButtons.YesNo);
        //        if (result == DialogResult.No)
        //        {
        //            return;
        //        }
        //    }
        //    int Selected = 0;
        //    for (int i = 0; i < PendingData.Rows.Count; i++)
        //    {
        //        if (Convert.ToBoolean(PendingData.Rows[i]["Sel"]) == true)
        //            Selected++;
        //    }
        //    if (Selected == 0)
        //    {
        //        MessageBox.Show("Please select the entries.");
        //        return;
        //    }
        //    foreach (DataRow dr in PendingData.Rows)
        //    {
        //        if (Convert.ToBoolean(dr["Sel"]) == false)
        //        {
        //            dr.Delete();
        //        }
        //    }
        //    PendingData.AcceptChanges();
        //    frmMRPPlan2 f = new frmMRPPlan2(PendingData);
        //    f.Show();
        //    this.label2.Visible = false;
        //    this.Hide();
        //}

            //Ruchit Start 26974 Bug 26/10/2015
            if (BomNotDefined.Trim().Length > 0 && BommID.Trim().Length == 0) // no bom id items
            {
                DialogResult result = MessageBox.Show("BOM not attached to the item(s): " + BomNotDefined.Trim().Substring(1).Trim() + Environment.NewLine + "Need BOM to Generate Indent or Workorder", "", MessageBoxButtons.OK);
                if (result == DialogResult.OK)
                {
                    //Application.Exit();
                    return;
                }
            }
            else if (BomNotDefined.Trim().Length > 0 && BommID.Trim().Length != 0) //bom id + no bom id items
            {
                DialogResult result = MessageBox.Show("BOM not attached to the item(s): " + BomNotDefined.Trim().Substring(1).Trim() + Environment.NewLine + "Do you want to continue with selected item(s)?", "", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    //Application.Exit();
                    return;
                }
                else
                {
                    int Selected = 0;
                    for (int i = 0; i < PendingData.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(PendingData.Rows[i]["Sel"]) == true)
                            Selected++;
                    }
                    if (Selected == 0)
                    {
                        MessageBox.Show("Please select the entries.");
                        return;
                    }
                    foreach (DataRow dr in PendingData.Rows)
                    {
                        if (Convert.ToBoolean(dr["Sel"]) == false)
                        {
                            dr.Delete();
                        }
                    }
                    PendingData.AcceptChanges();
                    frmMRPPlan2 f = new frmMRPPlan2(PendingData);
                    f.Show();
                    this.label2.Visible = false;
                    this.Hide();
                }
            }
            else if (BomNotDefined.Trim().Length == 0 && BommID.Trim().Length != 0) // only bom id items
            {
                int Selected = 0;
                for (int i = 0; i < PendingData.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(PendingData.Rows[i]["Sel"]) == true)
                        Selected++;
                }
                if (Selected == 0)
                {
                    MessageBox.Show("Please select the entries.");
                    return;
                }
                foreach (DataRow dr in PendingData.Rows)
                {
                    if (Convert.ToBoolean(dr["Sel"]) == false)
                    {
                        dr.Delete();
                    }
                }
                PendingData.AcceptChanges();
                frmMRPPlan2 f = new frmMRPPlan2(PendingData);
                f.Show();
                this.label2.Visible = false;
                this.Hide();
            }
            else                                            //no entries    
            {
                int Selected = 0;
                for (int i = 0; i < PendingData.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(PendingData.Rows[i]["Sel"]) == true)
                        Selected++;
                }
                if (Selected == 0)
                {
                    MessageBox.Show("Please select the entries.");
                    return;
                }
            }
        }
        //Ruchit End 26974 Bug 26/10/2015

        private void btnCancel_Click(object sender, EventArgs e)
        {
            clsCommon.DeleteProcessIdRecord();
            Application.Exit();
        }

        private void frmMRPPlan1_Load(object sender, EventArgs e)
        {
            this.Text = clsCommon.ApplName;
            if (clsCommon.IconFile!=null)
                this.Icon = new Icon(clsCommon.IconFile);

            this.label1.Text = "Order Entries";
            this.label2.Visible = false;
            //oDataAccess = new DataAccess_Net.clsDataAccess();     
            this.GridSetting();
        }

        private void gridView1_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (gridView1.FocusedColumn.Name.ToUpper() == "COLSEL" || gridView1.FocusedColumn.Name.ToUpper() == "COLADJUSTQTY")
                e.Cancel = false;
            else
                e.Cancel = true;
        }

        private void UpdateBomDetails()
        { 
             DataView view = PendingData.DefaultView;
             string SqlStr = string.Empty;
             BomNotDefined = string.Empty;
             BommID = string.Empty; //Ruchit
            view.RowFilter = "Sel = 1";
            // then get the distinct table...
            DataTable distinctItem = view.ToTable("PendData", true,new string[]{"Item","It_code"});


            for (int i = 0; i < distinctItem.Rows.Count; i++)
            {
                SqlStr = "Select Isnull(u_bomid1,'') as u_bomid1 From It_mast Where It_code=@It_code";
                SqlConnection con = new SqlConnection(clsCommon.ConnStr);
                SqlCommand cmd = new SqlCommand(SqlStr, con);
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter("@It_code",Convert.ToString(distinctItem.Rows[i]["It_code"]).Trim()));
                if (con.State == ConnectionState.Closed)
                    con.Open();
                cmd.CommandTimeout = 60000;
                string BomId = Convert.ToString(cmd.ExecuteScalar()).Trim();
                if (BomId == string.Empty)
                {
                    BomNotDefined += " ," + Convert.ToString(distinctItem.Rows[i]["Item"]).Trim();
                    DataRow[] rows = PendingData.Select("It_code=" + Convert.ToString(distinctItem.Rows[i]["It_code"]).Trim());
                    foreach (DataRow dr in rows)
                    {
                        dr["Sel"] = false;
                    }
                }
                //Ruchit Start 26974 Bug 26/10/2015   
                if (BomId.Trim().Length != 0)
                {
                    BommID += " ," + Convert.ToString(distinctItem.Rows[i]["Item"]).Trim();
                }
                //Ruchit End 26974 Bug 26/10/2015
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
            view.RowFilter = "";
        }

        private void GridSetting()
        {
            gridControl1.DataSource = PendingData;
            gridControl1.ForceInitialize();
            gridView1.PopulateColumns();
            gridView1.OptionsPrint.AutoWidth = false;
            gridControl1.MainView = gridView1;
            gridView1.OptionsView.ShowGroupPanel = false;

            gridView1.Columns[0].Width = 30;
            gridView1.Columns[0].Caption= "Select";
            gridView1.Columns[1].Caption = "Order No.";
            gridView1.Columns[1].Width = 50;
            gridView1.Columns[2].Caption = "Date";
            gridView1.Columns[2].Width = 40;

            gridView1.Columns[3].Caption = "Due Date";
            gridView1.Columns[3].Width = 40;
            gridView1.Columns[4].Caption = "Item";
            gridView1.Columns[4].Width = 120;
            gridView1.Columns[5].Caption = "Quantity";
            gridView1.Columns[5].Width = 60;
            gridView1.Columns[6].Caption = "Adjust Qty";
            gridView1.Columns[6].Width = 60;
            gridView1.Columns[6].Visible = false;
            gridView1.Columns[7].Caption = "Warehouse";
            gridView1.Columns[8].Visible = false;
            gridView1.Columns[9].Visible = false;
            gridView1.Columns[10].Visible = false;
            gridView1.Columns[11].Visible = false;
            if (clsCommon.IsWareAppl == false)
                gridView1.Columns[7].Visible = false;

        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (this.cboExport.Text == "")
            {
                MessageBox.Show("Please select file type.");
                this.cboExport.Focus();
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog();
            switch (cboExport.Text)
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

            sfd.FileName = "ExportDataGrid";

            if (sfd.ShowDialog() != DialogResult.OK )
                return;


            switch (cboExport.Text)
            {
                case "EXCEL(XLS)":
                    XlsExportOptions _Options1 = new XlsExportOptions();
                    _Options1.SheetName = sfd.FileName;
                    _Options1.ExportMode = XlsExportMode.SingleFile;
                    gridControl1.ExportToXls(sfd.FileName, _Options1);
                    break;
                case "EXCEL(XLSX)":
                    XlsxExportOptions _Options2 = new XlsxExportOptions();
                    _Options2.SheetName = sfd.FileName;
                    _Options2.ExportMode = XlsxExportMode.SingleFile;
                    gridControl1.ExportToXlsx(sfd.FileName, _Options2);
                    break;
                case "PDF":
                    PdfExportOptions _Options3 = new PdfExportOptions();
                    gridControl1.ExportToPdf(sfd.FileName);
                    break;
                case "RTF":
                    RtfExportOptions _Options4 = new RtfExportOptions();
                    _Options4.ExportMode = RtfExportMode.SingleFile;
                    gridControl1.ExportToRtf(sfd.FileName);
                    break;
                case "HTML":
                    RtfExportOptions _Options5 = new RtfExportOptions();
                    _Options5.ExportMode = RtfExportMode.SingleFilePageByPage;
                    gridControl1.ExportToHtml(sfd.FileName);
                    break;
            }
        }

        private void frmMRPPlan1_FormClosed(object sender, FormClosedEventArgs e)
        {
           
        }

        private void frmMRPPlan1_FormClosing(object sender, FormClosingEventArgs e)
        {
            clsCommon.DeleteProcessIdRecord();
        }


    }
}
