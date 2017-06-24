using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DataAccess_Net;

namespace DadosReports
{
    public partial class frmLayOut : Form
    {
        //public frmLayOut(string ConnString, string Username, string ReportId)//Commented by Archana K. on 21/03/14 for Bug-22080
        public frmLayOut(string ConnString, string Username, string ReportId, Boolean flag)//Changed by Archana K. on 21/03/14 for Bug-22080
        {
            InitializeComponent();
            _layoutName = string.Empty;
            _isDefault = true;
            connString = ConnString;
            userName = Username;
            reportId = ReportId;
            flag1 = flag;
        }
        private string _layoutName;
        public string LayoutName
        {
            get { return _layoutName; }
            set { _layoutName = value; }
        }
        private bool _isDefault;
        public bool IsDefault
        {
            get { return _isDefault; }
            set { _isDefault = value; }
        }
        private bool _isView;
        public bool IsView
        {
            get { return _isView; }
            set { _isView = value; }
        }
        private int _LayOutId;
        public int LayOutId
        {
            get { return _LayOutId; }
            set { _LayOutId = value; }
        }
        DataAccess_Net.clsDataAccess oDataAccess;
        private string connString;
        private string userName;
        private string reportId;
        string sqlStr = string.Empty;
        Boolean flag1;
        private void btnCancel_Click(object sender, EventArgs e)
        {
            _layoutName = string.Empty;
            this.Close();
        }
        //public int Status = 0;    // Insert LayOut//Commented by Archana K. on 21/03/14 for Bug-22080
        public int Status=-1; //Changes by Archana K. on 21/03/14 for Bug-22080   
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (btnSave.Text == "&Save")
            {
                _layoutName = txtLayoutName.Text.Trim();
                _isDefault = chkIsDefault.Checked;
                if (CheckValidation() == false)
                {
                    return;
                }
                if (CheckExistence() == true)
                {
                    if (DialogResult.No == MessageBox.Show("Layout name already exists." + Environment.NewLine + " Do you want to update the layout?", "Dados Reports", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                        Status = 0;
                        return;
                    }
                        //Added by Archana K. on 24/03/14 for Bug-22080 start
                    else
                    {
                        Status = 1;
                    }
                        //Added by Archana K. on 24/03/14 for Bug-22080 end
                      // Update LayOut
                }
                this.Close();
            }
            else
            {
                _layoutName = txtLayoutName.Text;
                IsView = true;
                this.Close();
            }

        }
        private void EblDbl(Boolean vEnabled)
        {
            //Added by Archana K. on 22/03/14 for Bug-22080
            btnNew.Enabled=vEnabled;
            btnEdit.Enabled = vEnabled;
            btnSave.Enabled=!vEnabled;
            txtLayoutName.ReadOnly = vEnabled;
           // btnCancel.Enabled = !vEnabled;
            btnDelete.Enabled = vEnabled;
            btnLayout.Enabled = !vEnabled;
            chkIsDefault.Visible = !vEnabled;
            lblIsDefault.Visible = !vEnabled;
        }
        private void clear()
        {
            txtLayoutName.Text = "";
            chkIsDefault.Checked = false;
        }
        private bool CheckValidation()
        {
            if (txtLayoutName.Text.Trim().Length == 0)
            {
                MessageBox.Show("Layout name cannot be empty.", "Dados Reports", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtLayoutName.Focus();
                return false;
            }

            return true;
        }
        private bool CheckExistence()
        {
            bool RetValue=true;
            SqlConnection cn = new SqlConnection(connString);
            sqlStr = "If Exists(select Top 1 LayoutName from reportlayout Where userName=@userName and Repid=@Repid and layOutname=@LayoutName)";
            sqlStr = sqlStr + " " + "select convert(bit,1) as ret";
            sqlStr=sqlStr+" "+"else";
            sqlStr = sqlStr + " " + "select convert(bit,0) as ret";
            SqlCommand lcmd = new SqlCommand(sqlStr, cn);
            cn.Open();
            lcmd.Parameters.Add("@UserName", SqlDbType.VarChar);
            lcmd.Parameters["@UserName"].Value = userName;

            lcmd.Parameters.Add("@RepId", SqlDbType.VarChar);
            lcmd.Parameters["@RepId"].Value = reportId;

            lcmd.Parameters.Add("@LayoutName", SqlDbType.VarChar);
            lcmd.Parameters["@LayoutName"].Value = this.LayoutName;

            SqlDataReader dr = lcmd.ExecuteReader();
            while (dr.Read())
            {
                RetValue = (bool)dr["ret"];
            }
            dr.Close();
            cn.Close();

            return RetValue;

            
        }

        private void frmLayOut_Load(object sender, EventArgs e)
        {
            oDataAccess = new clsDataAccess();
            this.chkIsDefault.Checked = true;
            //Added by Archana K. on 15/03/14 for Bug-22080 start
            EblDbl(flag1);
            if (flag1 == true)
            {
                btnSave.Text = "&Save";
                btnLayout.Enabled = flag1;
            }
            else
            {
                btnSave.Text = "&View";
                chkIsDefault.Visible = flag1;
                lblIsDefault.Visible = flag1;
                txtLayoutName.ReadOnly = !flag1;
            }
            BindData();
            //Added by Archana K. on 15/03/14 for Bug-22080 end
           //this.Activate();
        }
        private void BindData()
        {
            txtLayoutName.Text=LayoutName;
            chkIsDefault.Checked = IsDefault;
        }
        private void btnLayout_Click(object sender, EventArgs e)
        {
            //Added by Archana K. on 15/03/14 for Bug-22080
            string VForText = string.Empty, vSearchCol = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            sqlStr = "Select LayOutName,LayOutId from reportlayout where Repid="+reportId+" order by LayOutName";
            tDs = oDataAccess.GetDataSet(sqlStr, null, 20);

            DataView dvw = tDs.Tables[0].DefaultView;
            if ((dvw.Table.Rows.Count <= 0))
            {
                MessageBox.Show("No Records Found !!!", "Dados Reports", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            VForText = "Select Report Layout";
            vSearchCol = "LayOutName";
            vDisplayColumnList = "LayOutName:Report Layout";
            vReturnCol = "LayOutName,LayOutId";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;
            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtLayoutName.Text = oSelectPop.pReturnArray[0];
                LayOutId = Convert.ToInt32(oSelectPop.pReturnArray[1]);//Added by Archana K. on 21/03/14 for Bug-22080
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            //Added by Archana K. on 20/03/14 for Bug-22080
            Status = 0;
            EblDbl(false);
            btnLayout.Enabled = false;
            clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //Added by Archana K. on 20/03/14 for Bug-22080
            Status = 2;
           _layoutName = txtLayoutName.Text.Trim();
           //_LayOutId = LayOutId;
           this.Close();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            //Added by Archana K. on 20/03/14 for Bug-22080
            Status = 1;
            EblDbl(false);
        }

        private void frmLayOut_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();//Added by Archana K. on 19/03/14 for Bug-22080
        }

       

    }
}
