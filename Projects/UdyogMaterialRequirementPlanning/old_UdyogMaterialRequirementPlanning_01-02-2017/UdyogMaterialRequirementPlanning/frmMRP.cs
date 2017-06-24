using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using System.Diagnostics;

namespace UdyogMaterialRequirementPlanning
{
    public partial class frmMRP :Form
    {

        string ValidInEntries = string.Empty;

        #region Constructor
        public frmMRP(string[] args)
        {
            InitializeComponent();
            
            // Execution from project
//            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            //clsCommon.FromDt = Convert.ToDateTime("01/04/2013", new CultureInfo("en-GB"));
            //clsCommon.ToDt = Convert.ToDateTime("31/03/2014", new CultureInfo("en-GB"));
            //clsCommon.DbName = "D031314";
            //clsCommon.ServerName = @"Prod_Shrikant\shree";
            //clsCommon.User = "sa";
            //clsCommon.Password = "sa1985";
            ////clsCommon.EntryType = "SO";
            //clsCommon.AppUserName = "ADMIN";
            //ValidInEntries = "SO:DATE";

            //Execution from Software

            clsCommon.DbName = args[1];
            clsCommon.ServerName = args[2];
            clsCommon.User = args[3];
            clsCommon.Password = args[4];
            clsCommon.AppUserName = args[6].Replace("<*#*>", " ");
            clsCommon.IconFile = args[7].Replace("<*#*>", " ");
            clsCommon.pApplDesc = args[8].Replace("<*#*>", " ");
            clsCommon.pApplNm = args[9].Replace("<*#*>", " ");
            clsCommon.pApplId = args[10];
            clsCommon.pApplCode = args[11].Replace("<*#*>", " ");

            ValidInEntries = args[13];
            clsCommon.FromDt = Convert.ToDateTime(args[14].Replace("<*#*>", " "), new CultureInfo("en-GB"));
            clsCommon.ToDt = Convert.ToDateTime(args[15].Replace("<*#*>", " "), new CultureInfo("en-GB"));

            clsCommon.FinYear = clsCommon.FromDt.Year.ToString() + "-" + clsCommon.ToDt.Year.ToString();
            clsCommon.cApplNm = typeof(Program).Assembly.GetName().Name + ".exe";
            clsCommon.cApplId = Convert.ToString(Process.GetCurrentProcess().Id);
            clsCommon.ApplName = "Process Material Planning";


            clsCommon.ConnStr = "Data Source=" + clsCommon.ServerName + ";Initial Catalog=" + clsCommon.DbName + ";Uid=" + clsCommon.User + ";Pwd=" + clsCommon.Password;
            this.dtEditFrom.EditValue = clsCommon.FromDt.Date;
            this.dtEditTo.EditValue = clsCommon.ToDt.Date;
            clsCommon.FinYear = clsCommon.FromDt.Year.ToString() + "-" + clsCommon.ToDt.Year.ToString();

        }
        #endregion


        #region Private Methods
        private void FormSetting()
        {
            try
            {
                SqlConnection conn = new SqlConnection(clsCommon.ConnStr);

                SqlDataAdapter da = new SqlDataAdapter("Select COUNT(IsWkOrdAppl) as WkOrderCnt  From lcode Where IsWkOrdAppl=1", conn);
                DataTable ldt = new DataTable();
                da.Fill(ldt);
                clsCommon.IsWKOrdAppl = Convert.ToBoolean(ldt.Rows[0]["WkOrderCnt"]);
                ldt.Clear();

                da = new SqlDataAdapter("Select COUNT(IsWareAppl) as WareCnt  from lcode Where IsWareAppl=1", conn);
                da.Fill(ldt);
                clsCommon.IsWareAppl = Convert.ToBoolean(ldt.Rows[0]["WareCnt"]);
                
                //clsCommon.MRPValidity = Convert.ToString(ldt.Rows[0]["MRPValidity"]);
                //clsCommon.MRPDepOn = (Convert.ToString(ldt.Rows[0]["MRPDepOn"]) != "" ? Convert.ToString(ldt.Rows[0]["MRPDepOn"]) : "DATE");
                //clsCommon.CodeNm = StringExtension.ToTitleCase(Convert.ToString(ldt.Rows[0]["Code_nm"]));

                if (clsCommon.IsWareAppl == true)
                {

                }
                else
                {
                    this.lblWarehouse.Visible = false;
                    this.txtWarehouse.Visible = false;
                    this.btnWarehouse.Visible = false;
                    clsCommon.Warehouse = string.Empty;
                }
                ldt = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private DataTable GetPendingData()
        { 
            SqlConnection conn = new SqlConnection(clsCommon.ConnStr);
            DataTable ldt = new DataTable();
            string[] Entries = ValidInEntries.Split(',');
            for (int i = 0; i < Entries.Length; i++)
            {
                string[] DependsOn = Entries[i].Split(':');
                SqlCommand cmd = new SqlCommand("Execute Get_MRP_PendingData @Entry_ty,@Sdate,@Edate,@DateOn,@Warehouse", conn);
                cmd.Parameters.Add(new SqlParameter("@Entry_ty", DependsOn[0]));
                cmd.Parameters.Add(new SqlParameter("@Sdate", clsCommon.FromDt));
                cmd.Parameters.Add(new SqlParameter("@Edate", clsCommon.ToDt));
                cmd.Parameters.Add(new SqlParameter("@DateOn", DependsOn[1]));
                cmd.Parameters.Add(new SqlParameter("@Warehouse", clsCommon.Warehouse));
                cmd.CommandTimeout = 60000;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ldt);
            }
            
            
            return ldt;
        }
        private bool CheckValidation()
        { 
            bool RetVal=true;
            if (this.dtEditFrom.Text == string.Empty)
            {
                MessageBox.Show("Please select from date.");
                this.dtEditFrom.Focus();
                return false;
            }
            if (this.dtEditTo.Text == string.Empty)
            {
                MessageBox.Show("Please select to date.");
                this.dtEditTo.Focus();
                return false;
            }
            if (Convert.ToDateTime(this.dtEditTo.EditValue) < Convert.ToDateTime(this.dtEditFrom.EditValue))
            {
                MessageBox.Show("From date can not be greater than to date.");
                this.dtEditFrom.Focus();
                return false;
            }
            return RetVal;
        }
        #endregion


        private void btnCancel_Click(object sender, EventArgs e)
        {
            clsCommon.DeleteProcessIdRecord();
            Application.Exit();
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            clsCommon.FromDt =Convert.ToDateTime(this.dtEditFrom.EditValue);
            clsCommon.ToDt= Convert.ToDateTime(this.dtEditTo.EditValue);
            if (this.CheckValidation() == false)
                return;

            DataTable ldt = this.GetPendingData();
            if (ldt == null)
                return;

            if (ldt.Rows.Count == 0)
            {
                MessageBox.Show("No orders found for planning.");
                return;
            }
            frmMRPPlan1 f = new frmMRPPlan1(ldt);
            f.Show();
            this.Hide();
        }

        private void frmMRP_Load(object sender, EventArgs e)
        {
            this.Text = clsCommon.ApplName;
            if (clsCommon.IconFile!=null)
                this.Icon = new Icon(clsCommon.IconFile);
            this.FormSetting();
            clsCommon.InsertProcessIdRecord();
        }
        

        private void btnWarehouse_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(clsCommon.ConnStr);
            SqlCommand cmd = new SqlCommand("Select Ware_nm From Warehouse", conn);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable ldt = new DataTable();
            da.Fill(ldt);
            
            DataView ldv = ldt.DefaultView;
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = ldv;
            oSelectPop.pformtext = "Select Warehouse";
            oSelectPop.psearchcol = "Ware_nm";

            oSelectPop.pDisplayColumnList = "Ware_nm:Warehouse";
            oSelectPop.pRetcolList = "Ware_nm";
            oSelectPop.ShowDialog();

            if (oSelectPop.pReturnArray != null)
            {
                this.txtWarehouse.Text = oSelectPop.pReturnArray[0];
                clsCommon.Warehouse = oSelectPop.pReturnArray[0];
            }
        }

        private void frmMRP_FormClosing(object sender, FormClosingEventArgs e)
        {
            clsCommon.DeleteProcessIdRecord();
        }
      
    }
}
