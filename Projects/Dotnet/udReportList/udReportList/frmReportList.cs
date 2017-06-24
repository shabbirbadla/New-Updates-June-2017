using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using System.IO;
using System.Reflection;

using udCrViewer;
namespace udReportList
{
    public partial class frmReportList : uBaseForm.FrmBaseForm
    {
        DataAccess_Net.clsDataAccess oDataAccess;
        string SqlStr = "", vRepGroup = "", vEntry_Ty ;
        Boolean vWaitPrint = false, vBtnCancelPress = false;
        DataTable tblRepList = new DataTable();
        DataTable vTblRepList = new DataTable();

        //DataSet dsCommon = new DataSet();
        Int16 vTran_Cd, vPreviewOption;
        string appPath = "", appName = "", vReportPath = "", vSpPara="";
        public frmReportList()
        {
            InitializeComponent();
            ////1- Preview with Print Button [Default]
            ////2- Preview with out Print Button
            ////3- Print with Preview
            //this.pPreviewOption = 1;
            //this.pTran_Cd = 116;
            //this.pEntry_Ty = "BP";
            //this.pAppPath = Application.ExecutablePath;
        }
        private void frmReportList_Load(object sender, EventArgs e)
        {

            mthMainGrdRefresh();

            if (this.pPrintOption != 3)
            {
                this.chkWait.Visible = false;
            }
            else
            {
                this.chkWait.Visible = true ;
            }

        }
        private void mthMainGrdRefresh()
        {
            
            this.dgvMain.Columns.Clear();
            this.dgvMain.DataSource = this.pTblRepList ;
            this.dgvMain.Columns.Clear();

            System.Windows.Forms.DataGridViewCheckBoxColumn ColSel = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ColSel.HeaderText = "Select";
            ColSel.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            ColSel.Name = "ColSel";
            ColSel.Width = 40;
            this.dgvMain.Columns.Add(ColSel);

            System.Windows.Forms.DataGridViewTextBoxColumn colDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colDesc.HeaderText = "Report Description";
            colDesc.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colDesc.Name = "colDesc";
            colDesc.Width = 280;
            colDesc.ReadOnly = true;
            this.dgvMain.Columns.Add(colDesc);


            System.Windows.Forms.DataGridViewTextBoxColumn colRepNm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colRepNm.HeaderText = "Rep_Nm";
            colRepNm.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colRepNm.Name = "colRepNm";
            colRepNm.Width = 45;
            this.dgvMain.Columns.Add(colRepNm);


            this.dgvMain.Columns["colRepNm"].Visible = false;
            dgvMain.Columns["colSel"].DataPropertyName = "Sel";
            dgvMain.Columns["colDesc"].DataPropertyName = "Desc";
            dgvMain.Columns["colRepNm"].DataPropertyName = "Rep_Nm";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.vBtnCancelPress = true;
            this.Close();
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            this.vBtnCancelPress = false ;
            this.pWaitPrint=this.chkWait.Checked;

            this.Close();
        }
        private void mthCallCrViewer123(DataRow dr)
        {
            int pos = -1;
            if ((Boolean)dr["Sel"])
            {
                vReportPath =this.pdsCommon.Tables["company"].Rows[0]["Dir_Nm"].ToString().Trim() + dr["Rep_Nm"].ToString().Trim() + ".rpt";

                SqlStr = dr["SqlQuery"].ToString();
                pos = SqlStr.IndexOf(";");
                if (SqlStr.ToLower().IndexOf("execute") == -1)
                {
                    MessageBox.Show(" Please use Strored Procedure insetead of Query ''" + SqlStr + "''", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    SqlStr = "";
                }
                else
                {
                    if (pos > 0)
                    {
                        SqlStr = SqlStr.Substring(0, pos);
                        //SqlStr = SqlStr + " ' A.Entry_ty=''" + this.pEntry_Ty + "'' AND A.TRAN_CD =" + this.pTran_Cd.ToString().Trim() + "'";
                        SqlStr = SqlStr + vSpPara;
                    }
                    else
                    {
                        MessageBox.Show(" Invalid SQl Statement " + SqlStr, this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    if (File.Exists(vReportPath) == false)
                    {
                        MessageBox.Show(vReportPath + " File not found", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }

                if (File.Exists(vReportPath) && SqlStr != "")
                {

                    DataTable vResulSet = new DataTable();
                    vResulSet = oDataAccess.GetDataTable(SqlStr, null, 80);

                    appPath = this.pAppPath;
                    appPath = Path.GetDirectoryName(appPath);

                    appPath = appPath + @"\";
                    appName = "udCrViewer.exe";
                    appPath = @appPath.Trim() + appName.Trim();

                    udCrViewer.frmCrViewer oCrvForm = new udCrViewer.frmCrViewer();
                    oCrvForm.pPara = this.pPara;
                    oCrvForm.pAppPath = this.pAppPath;
                    //oCrvForm.pReportPath = vReportPath; //Rup
                    oCrvForm.pParentForm  = this;
                    //oCrvForm.pResulSet = vResulSet; //Rup
                    oCrvForm.pdsCommon =this.pdsCommon;
                    oCrvForm.Show();
                    //Assembly ass = Assembly.LoadFrom(appPath);
                    //Form extform = new Form();
                    //appName = appName.Substring(0, appName.IndexOf("."));
                    //extform = (Form)ass.CreateInstance(appName.Trim() + ".frmCrViewer", true);

                    //Type t = extform.GetType();
                    //t.GetProperty("pAppPath").SetValue(extform, this.pAppPath, null);
                    //t.GetProperty("pPara").SetValue(extform, this.pPara, null);
                    //t.GetProperty("pParentForm").SetValue(extform, this, null);
                    //t.GetProperty("pReportPath").SetValue(extform, vReportPath, null);

                }

            }
        }
        public string  pEntry_Ty
        {

            get
            {
                return vEntry_Ty;
            }
            set
            {
                vEntry_Ty = value;
            }
        }
        //public Int16 pTran_Cd
        //{

        //    get
        //    {
        //        return vTran_Cd;
        //    }
        //    set
        //    {
        //        vTran_Cd = value;
        //    }
        //}
        public string pRepGroup
        {

            get
            {
                return vRepGroup;
            }
            set
            {
                vRepGroup = value;
            }
        }
        //public string pSpPara
        //{

        //    get
        //    {
        //        return vSpPara;
        //    }
        //    set
        //    {
        //        vSpPara = value;
        //    }
        //}
        public Int16 pPrintOption
        {
            get
            {
                return vPreviewOption;
            }
            set
            {
                vPreviewOption = value;
            }
        }
        public DataTable pTblRepList
        {
            get { return vTblRepList; }
            set { vTblRepList = value; }
        }
        public Boolean  pWaitPrint
        {
            get { return vWaitPrint; }
            set { vWaitPrint = value; }
        }
        public Boolean pBtnCancelPress
        {
            get { return vBtnCancelPress; }
            set { vBtnCancelPress = value; }
        }
    }
}
