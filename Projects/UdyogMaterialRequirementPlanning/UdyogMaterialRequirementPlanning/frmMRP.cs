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

            //// Execution from project  suraj kumawat

            //Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            //clsCommon.FromDt = Convert.ToDateTime("01/04/2016", new CultureInfo("en-GB"));
            //clsCommon.ToDt = Convert.ToDateTime("31/03/2017", new CultureInfo("en-GB"));
            //clsCommon.DbName = "B051617";
            //clsCommon.ServerName = @"AIPLDTM002\SQLEXPRESS2008";
            //clsCommon.User = "sa";
            //clsCommon.Password = "sa1985";
            //////clsCommon.EntryType = "SO";
            //clsCommon.AppUserName = "ADMIN";
            //ValidInEntries = "SO:DATE";
            //clsCommon.Valid_Trans = "SO:DATE"; // added by suraj Kumawat for Bug-29249 

            ////Execution from Software

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
            clsCommon.Valid_Trans = args[13]; // added by suraj Kumawat for Bug-29249 

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
                //da = new SqlDataAdapter("Select COUNT(IsWareAppl) as WareCnt  from lcode Where IsWareAppl=1", conn); // Commented by Suraj Kumawat for Bug-29249 

                if (clsCommon.Valid_Trans.ToString() == "SP:DATE") // added by Suraj Kumawat for Bug-29249 
                {
                    da = new SqlDataAdapter("Select ISNULL(COUNT(IsWareAppl),0) as WareCnt  from lcode Where  (Entry_ty = 'SP' OR BCODE_NM='SP') AND IsWareAppl=1   and isnull(mrpvalidity,0) = 1 ", conn);
                }
                else {
                    da = new SqlDataAdapter("Select ISNULL(COUNT(IsWareAppl),0) as WareCnt  from lcode Where  (Entry_ty = 'SO' OR BCODE_NM='SO') AND IsWareAppl=1  and isnull(mrpvalidity,0) = 1 ", conn);
                }
                    
                da.Fill(ldt);
                clsCommon.IsWareAppl = Convert.ToBoolean(ldt.Rows[0]["WareCnt"]);
                
                //clsCommon.MRPValidity = Convert.ToString(ldt.Rows[0]["MRPValidity"]);
                //clsCommon.MRPDepOn = (Convert.ToString(ldt.Rows[0]["MRPDepOn"]) != "" ? Convert.ToString(ldt.Rows[0]["MRPDepOn"]) : "DATE");
                //clsCommon.CodeNm = StringExtension.ToTitleCase(Convert.ToString(ldt.Rows[0]["Code_nm"]));

                if (clsCommon.IsWareAppl == true)
                {
                    clsCommon.Wharehouseapplicable = 1 ;
                }
                else
                {
                    this.lblWarehouse.Visible = false;
                    this.txtWarehouse.Visible = false;
                    this.btnWarehouse.Visible = false;
                    clsCommon.Warehouse = string.Empty;
                    clsCommon.Wharehouseapplicable = 0;
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
                ///// Commented by Suraj Kumawat for bug-29249 Start 
                //SqlCommand cmd = new SqlCommand("Execute Get_MRP_PendingData @Entry_ty,@Sdate,@Edate,@DateOn,@Warehouse", conn);
                //cmd.Parameters.Add(new SqlParameter("@Entry_ty", DependsOn[0]));
                //cmd.Parameters.Add(new SqlParameter("@Sdate", clsCommon.FromDt));
                //cmd.Parameters.Add(new SqlParameter("@Edate", clsCommon.ToDt));
                //cmd.Parameters.Add(new SqlParameter("@DateOn", DependsOn[1]));
                //cmd.Parameters.Add(new SqlParameter("@Warehouse", clsCommon.Warehouse));

                SqlCommand cmd = new SqlCommand("Execute Get_MRP_PendingData @Entry_ty,@Sdate,@Edate,@DateOn,@Warehouse,@Wharehouseapplicable", conn);
                cmd.Parameters.Add(new SqlParameter("@Entry_ty", DependsOn[0]));
                cmd.Parameters.Add(new SqlParameter("@Sdate", clsCommon.FromDt));
                cmd.Parameters.Add(new SqlParameter("@Edate", clsCommon.ToDt));
                cmd.Parameters.Add(new SqlParameter("@DateOn", DependsOn[1]));
                cmd.Parameters.Add(new SqlParameter("@Warehouse", clsCommon.Warehouse));
                cmd.Parameters.Add(new SqlParameter("@Wharehouseapplicable", clsCommon.Wharehouseapplicable));
                ///// Commented by Suraj Kumawat for bug-29249 end...
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

            // Added by Suraj Kumawat for Bug-29249  Start :
            if (clsCommon.IsWareAppl == true)
            {
                this.Default_wareHouse_Selection();
            }
            // Added by Suraj Kumawat for Bug-29249  end:
        }


        public void Default_wareHouse_Selection()  // Added by Suraj Kumawat for Bug-29249  
        {
            SqlConnection conn = new SqlConnection(clsCommon.ConnStr);
            //SqlCommand cmd = new SqlCommand("Select Ware_nm From Warehouse", conn); // Commented by Suraj Kumawat for Bug-29249 
             // Added by Suraj Kumawat for Bug - 29249 :
            string sqlstr = string.Empty;
            sqlstr = "";
            if (clsCommon.Valid_Trans.ToString() == "SP:DATE")
            {
                sqlstr = "SELECT distinct B.WARE_NM  FROM LCODE A  LEFT OUTER JOIN WAREHOUSE B ON B.Validity LIKE '%' + A.Entry_ty + '%' OR(B.Validity LIKE '%' + A.BCODE_NM + '%' AND A.BCODE_NM <> '') WHERE(A.BCODE_NM IN('SP') OR A.ENTRY_TY in('SP')) and isnull(B.WARE_NM,'') <> ''";

            }
            else
            {
                sqlstr = "SELECT distinct B.WARE_NM  FROM LCODE A  LEFT OUTER JOIN WAREHOUSE B ON B.Validity LIKE '%' + A.Entry_ty + '%' OR(B.Validity LIKE '%' + A.BCODE_NM + '%' AND A.BCODE_NM <> '') WHERE(A.BCODE_NM IN('SO') OR A.ENTRY_TY in('SO')) and isnull(B.WARE_NM,'') <> ''";
            }
            //SqlCommand cmd = new SqlCommand("SELECT distinct B.WARE_NM  FROM LCODE A  LEFT OUTER JOIN WAREHOUSE B ON B.Validity LIKE '%' + A.Entry_ty + '%' OR(B.Validity LIKE '%' + A.BCODE_NM + '%' AND A.BCODE_NM <> '') WHERE(A.BCODE_NM IN('SP') OR A.ENTRY_TY in('SP')) and isnull(B.WARE_NM,'') <> '' ", conn);   
            SqlCommand cmd = new SqlCommand(sqlstr, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable ldt = new DataTable();
            da.Fill(ldt);
            string result = ldt.Rows[0][0].ToString();

            if (result != "")
            {
                this.txtWarehouse.Text = result;
                clsCommon.Warehouse = result;
                this.txtWarehouse.ReadOnly = true;
            }
        }

        private void btnWarehouse_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(clsCommon.ConnStr);
            //SqlCommand cmd = new SqlCommand("Select Ware_nm From Warehouse", conn); // Commented by Suraj Kumawat for Bug-29249 
            // Added by Suraj Kumawat for Bug-29249 
            string sqstr = string.Empty;
            if (clsCommon.Valid_Trans.ToString() == "SP:DATE")
            {
                sqstr = "SELECT distinct B.WARE_NM  FROM LCODE A  LEFT OUTER JOIN WAREHOUSE B ON B.Validity LIKE '%' + A.Entry_ty + '%' OR(B.Validity LIKE '%' + A.BCODE_NM + '%' AND A.BCODE_NM <> '') WHERE(A.BCODE_NM IN('SP') OR A.ENTRY_TY in('SP')) and isnull(B.WARE_NM,'') <> ''";
            }
            else {
                sqstr = "SELECT distinct B.WARE_NM  FROM LCODE A  LEFT OUTER JOIN WAREHOUSE B ON B.Validity LIKE '%' + A.Entry_ty + '%' OR(B.Validity LIKE '%' + A.BCODE_NM + '%' AND A.BCODE_NM <> '') WHERE(A.BCODE_NM IN('SO') OR A.ENTRY_TY in('SO')) and isnull(B.WARE_NM,'') <> ''";
            }
            // SqlCommand cmd = new SqlCommand("SELECT distinct B.WARE_NM  FROM LCODE A  LEFT OUTER JOIN WAREHOUSE B ON B.Validity LIKE '%' + A.Entry_ty + '%' OR(B.Validity LIKE '%' + A.BCODE_NM + '%' AND A.BCODE_NM <> '') WHERE(A.BCODE_NM IN('SO') OR A.ENTRY_TY in('SO', 'sp')) and isnull(B.WARE_NM,'') <> '' ", conn);  // Added by Suraj Kumawat for Bug-29249 
            SqlCommand cmd = new SqlCommand(sqstr, conn);  
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
