using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data.SqlClient;
using DataAccess_Net;       // Added by Sachin N. S. on 04/04/2014 for Bug-4524

namespace DadosReportParameterMaster
{
    public partial class ParameterMaster : Form
    {
        //variables of calling Application 
        private string pApplCode = string.Empty;
        private string pApplNm = string.Empty;
        private string pApplId = string.Empty;
        private string pApplDesc = string.Empty;

        //variables of this application
        private string cApplNm = string.Empty;
        private string cApplId = string.Empty;

        string SQLConnStr = string.Empty;
        string AppName = string.Empty;
        DataTable paratable = new DataTable("Parameter_vw");

        clsDataAccess _oDataAccess;     //----- Added by Sachin N. S. on 04/04/2014 for Bug-4524

        //CurrencyManager paraIDManager;
        int paratype;
        string para_type;
        string text1, text2;
        string tmpfieldcaption, tmpsearch, tmpreturn, tmpParam_filtcond;
        string tmp;
        string paraid;
        string tmpparaquery;
        int tmpdata;
        enum Mode
        {
            Add, Edit, View, DefValue
        }
        Mode CurrentMode;
        string strreturn;

        public ParameterMaster(string[] args)
        {
            InitializeComponent();
            //MessageBox.Show("Test - a");
            SQLConnStr = "Data source=" + args[2] + ";Initial Catalog=" + args[1] + ";Uid=" + args[3] + ";Pwd=" + args[4];

            //MessageBox.Show("Test - b");
            // ***** Added by Sachin N. S. on 04/04/2014 for Bug-4524 -- Start
            clsDataAccess._serverName = args[2].ToString();
            clsDataAccess._databaseName = args[1].ToString();
            clsDataAccess._userID = args[3].ToString();
            clsDataAccess._password = args[4].ToString();
            // ***** Added by Sachin N. S. on 04/04/2014 for Bug-4524 -- End

            //MessageBox.Show("Test - c");

            //MessageBox.Show(args[7].ToString());

            AppName = args[8].Replace("<*#*>", " ");
            this.Icon = new System.Drawing.Icon(args[7].Replace("<*#*>", " "));

            //MessageBox.Show("Test - d");

            pApplCode = args[11].Replace("<*#*>", " ");
            pApplNm = args[9];
            pApplId = args[10];

            //MessageBox.Show("Test - e");

            cApplNm = typeof(Program).Assembly.GetName().Name + ".exe";
            cApplId = Convert.ToString(Process.GetCurrentProcess().Id);

            //MessageBox.Show("Test - f");

            //SQLConnStr = "Data source=Prod_shrikant\\shree;Initial Catalog=C011213;Uid=sa;Pwd=sa1985";
            //this.AppName = "sdk";
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case (Keys.Control | Keys.N):
                    if (btnNew.Enabled)
                    {
                        btnNew.PerformClick();
                    }
                    break;
                case (Keys.Control | Keys.E):
                    if (btnEdit.Enabled)
                    {
                        btnEdit.PerformClick();
                    }
                    break;
                case (Keys.Control | Keys.S):
                    if (btnSave.Enabled)
                    {
                        btnSave.PerformClick();

                    }
                    break;
                case (Keys.Control | Keys.Z):
                    if (btnCancel.Enabled)
                    {
                        btnCancel.PerformClick();
                    }
                    break;
                case (Keys.Control | Keys.D):
                    if (btnDelete.Enabled)
                    {
                        btnDelete.PerformClick();
                    }
                    break;
                case (Keys.Control | Keys.C):
                    if (btnCopy.Enabled)
                    {
                        btnCopy.PerformClick();
                    }
                    break;
                case (Keys.Control | Keys.F4):
                    this.Close();
                    break;
                default:
                    return base.ProcessCmdKey(ref msg, keyData);
            }
            return true;
        }

        private void ParameterMaster_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void ParameterMaster_Load(object sender, EventArgs e)
        {
            //MessageBox.Show("Test - 1");
            _oDataAccess = new clsDataAccess();     //----- Added by Sachin N. S. on 04/04/2014 for Bug-4524

            //MessageBox.Show("Test - 2");

            //this.MoveLast();        // Commented by Sachin N. S. on 04/04/2014 for Bug-4524
            this.NavigateData("LAST");       // Added by Sachin N. S. on 04/04/2014 for Bug-4524

            //MessageBox.Show("Test - 3");

            //this.SetDataBinding();        // Commented by Sachin N. S. on 04/04/2014 for Bug-4524
            //genquerypopup();              // Commented by Sachin N. S. on 04/04/2014 for Bug-4524

            //MessageBox.Show("Test - 4");

            Paratypefill();
            fillgrid();

            //MessageBox.Show("Test - 5");

            CurrentMode = Mode.View;

            //MessageBox.Show("Test - 6");

            ControlState(CurrentMode);
            InsertProcessIdRecord();

            //MessageBox.Show("Test - 7");
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            //this.MoveFirst();     // Commented by Sachin N. S. on 04/04/2014 for Bug-4524
            this.NavigateData("FIRST");  // Added by Sachin N. S. on 04/04/2014 for Bug-4524
            genquerypopup();
            Paratypefill();
            fillgrid();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            //this.MovePrevious();    // Commented by Sachin N. S. on 04/04/2014 for Bug-4524
            this.NavigateData("PREVIOUS");  // Added by Sachin N. S. on 04/04/2014 for Bug-4524
            genquerypopup();
            Paratypefill();
            fillgrid();
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            //this.MoveNext();        // Commented by Sachin N. S. on 04/04/2014 for Bug-4524
            this.NavigateData("NEXT");  // Added by Sachin N. S. on 04/04/2014 for Bug-4524
            genquerypopup();
            Paratypefill();
            fillgrid();
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            //this.MoveLast();        // Commented by Sachin N. S. on 04/04/2014 for Bug-4524
            this.NavigateData("LAST");  // Added by Sachin N. S. on 04/04/2014 for Bug-4524
            genquerypopup();
            Paratypefill();
            fillgrid();
        }

        private void MoveFirst()
        {
            SqlConnection conn = new SqlConnection(this.SQLConnStr);
            string SqlQuery = "Select Top 1 a.*,RepQry=IsNull(b.RepQry,'') From Para_Master a Left Join usqry b on (a.QueryId=b.QryID) Order by ParameterId Asc";
            SqlCommand cmd = new SqlCommand(SqlQuery, conn);
            try
            {

                conn.Open();
                object ueqry = cmd.ExecuteScalar();
                SqlDataAdapter sqldata = new SqlDataAdapter();
                sqldata.SelectCommand = cmd;

                if (paratable != null)
                {
                    paratable.Rows.Clear();
                }
                sqldata.Fill(paratable);
                conn.Close();
                Paratypefill();
                if (rtxtParamQuery.Text != "")
                {
                    genquerypopup();
                }

                ControlState(CurrentMode);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void MovePrevious()
        {
            SqlConnection conn = new SqlConnection(this.SQLConnStr);

            string SqlQuery = "Select Top 1 ParameterId From Para_master Order By ParameterId Asc";
            SqlCommand cmd = new SqlCommand(SqlQuery, conn);
            Int32 ParamId;
            try
            {
                conn.Open();
                ParamId = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (ParamId == Convert.ToInt32(paratable.Rows[0]["ParameterId"]))
            {
                //MessageBox.Show("First record reached.", AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            SqlQuery = "Select Top 1 a.*,RepQry=IsNull(b.RepQry,'') From Para_Master a Left Join usqry b on (a.QueryId=b.QryID) "
                                + " Where ParameterId < " + Convert.ToString(paratable.Rows[0]["ParameterId"]) + " Order by ParameterId Desc ";
            cmd = new SqlCommand(SqlQuery, conn);
            try
            {

                conn.Open();
                object ueqry = cmd.ExecuteScalar();
                SqlDataAdapter sqldata = new SqlDataAdapter();
                sqldata.SelectCommand = cmd;

                if (paratable != null)
                {
                    paratable.Rows.Clear();
                }
                sqldata.Fill(paratable);
                conn.Close();
                Paratypefill();
                if (rtxtParamQuery.Text != "")
                {
                    genquerypopup();
                }

                ControlState(CurrentMode);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void MoveNext()
        {
            SqlConnection conn = new SqlConnection(this.SQLConnStr);

            string SqlQuery = "Select Top 1 ParameterId From Para_master Order By ParameterId Desc";
            SqlCommand cmd = new SqlCommand(SqlQuery, conn);
            Int32 ParamId;
            try
            {
                conn.Open();
                ParamId = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (ParamId == Convert.ToInt32(paratable.Rows[0]["ParameterId"]))
            {
                //MessageBox.Show("Last record reached.", AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            SqlQuery = "Select Top 1 a.*,RepQry=IsNull(b.RepQry,'') From Para_Master a Left Join usqry b on (a.QueryId=b.QryID) "
                                + " Where ParameterId > " + Convert.ToString(paratable.Rows[0]["ParameterId"]) + " Order by ParameterId Asc ";
            cmd = new SqlCommand(SqlQuery, conn);
            try
            {

                conn.Open();
                object ueqry = cmd.ExecuteScalar();
                SqlDataAdapter sqldata = new SqlDataAdapter();
                sqldata.SelectCommand = cmd;

                if (paratable != null)
                {
                    paratable.Rows.Clear();
                }
                sqldata.Fill(paratable);
                conn.Close();
                Paratypefill();
                if (rtxtParamQuery.Text != "")
                {
                    genquerypopup();
                }

                ControlState(CurrentMode);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void MoveLast()
        {
            SqlConnection conn = new SqlConnection(this.SQLConnStr);
            string SqlQuery = "Select Top 1 a.*,RepQry=IsNull(b.RepQry,'') From Para_Master a Left Join usqry b on (a.QueryId=b.QryID) Order by ParameterId Desc";
            SqlCommand cmd = new SqlCommand(SqlQuery, conn);
            try
            {

                conn.Open();
                object ueqry = cmd.ExecuteScalar();
                SqlDataAdapter sqldata = new SqlDataAdapter();
                sqldata.SelectCommand = cmd;

                if (paratable != null)
                {
                    paratable.Rows.Clear();
                }
                sqldata.Fill(paratable);
                conn.Close();
                Paratypefill();
                if (rtxtParamQuery.Text != "")
                {
                    genquerypopup();
                }
                if (paratable.Rows.Count > 0)
                    CurrentMode = Mode.View;
                else
                    CurrentMode = Mode.DefValue;

                ControlState(CurrentMode);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void SetDataBinding()
        {
            //***** Added by Sachin N. S. on 29/04/2014 for Bug-4524 -- Start
            txtParamId.DataBindings.Clear();
            rtxtParamQuery.DataBindings.Clear();
            txtParamName.DataBindings.Clear();
            txtParamCaption.DataBindings.Clear();
            txtparatype.DataBindings.Clear();
            //***** Added by Sachin N. S. on 29/04/2014 for Bug-4524 -- End

            txtParamId.DataBindings.Add("Text", paratable, "Parameterid");
            rtxtParamQuery.DataBindings.Add("Text", paratable, "RepQry");
            txtParamName.DataBindings.Add("Text", paratable, "paramName");
            txtParamCaption.DataBindings.Add("Text", paratable, "paraCaption");
            txtparatype.DataBindings.Add("Text", paratable, "ParamType");
            //txtParamType.DataBindings.Add("Text", paratable, "ParamType");  // Added by Sachin N. S. on 05/04/2014 for Bug-4524
        }
        private void Filldata()
        {
            //SqlConnection conn = new SqlConnection(this.SQLConnStr);
            ////SqlCommand cmd = new SqlCommand("select us.qryid,isnull(us.RepQry,0) RepQry,isnull(pa.ParaCaption,0) ParaCaption,isnull(pa.param_to,0) param_to,isnull(pa.ParamType,0) ParamType,isnull(pa.Param_filtcond,0) Param_filtcond from usqry us left join para_master pa on (us.QryID=pa.QueryId ) where us.RepID='' ", conn);
            //string SqlQuery = "Select a.*,RepQry=IsNull(b.RepQry,'') From Para_Master a Left Join usqry b on (a.QueryId=b.QryID)";
            //SqlCommand cmd = new SqlCommand(SqlQuery, conn);
            ////DataTable paratable;
            //try
            //{

            //    conn.Open();
            //    object ueqry = cmd.ExecuteScalar();
            //    SqlDataAdapter sqldata = new SqlDataAdapter();
            //    sqldata.SelectCommand = cmd;

            //    //paratable = new DataTable();
            //    if (paratable != null)
            //    {
            //        paratable.Rows.Clear();
            //    }
            //    sqldata.Fill(paratable);

            //    conn.Close();
            //    txtParamId.DataBindings.Clear();
            //    rtxtParamQuery.DataBindings.Clear();
            //    txtParamName.DataBindings.Clear();
            //    txtParamCaption.DataBindings.Clear();
            //    //chkTo.DataBindings.Clear();
            //    txtparatype.DataBindings.Clear();
            //    txtParamId.DataBindings.Add("Text", paratable, "Parameterid");
            //    rtxtParamQuery.DataBindings.Add("Text", paratable, "RepQry");
            //    txtParamName.DataBindings.Add("Text", paratable, "paramName");
            //    txtParamCaption.DataBindings.Add("Text", paratable, "paraCaption");
            //    //chkto.DataBindings.Add("Checked", paratable, "param_to");
            //    txtparatype.DataBindings.Add("Text", paratable, "ParamType");
            //    Paratypefill();
            //    if (rtxtParamQuery.Text != "")
            //    {
            //        genquerypopup();
            //    }
            //    // fillgrid();
            //    paraIDManager = (CurrencyManager)this.BindingContext[paratable];

            //    if (paratable.Rows.Count > 0)
            //        CurrentMode = Mode.View;
            //    else
            //        CurrentMode = Mode.DefValue;

            //    ControlState(CurrentMode);

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtParamName.Text.Trim() == "")
            {
                DialogResult save = MessageBox.Show("Please enter the parameter name.", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (save == DialogResult.OK)
                {
                    //MessageBox.Show("Please enter the parameter Caption");
                    txtParamName.Focus();
                    return;
                }
            }

            if (txtParamCaption.Text.Trim() == "")
            {
                DialogResult save = MessageBox.Show("Please enter the parameter caption.", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (save == DialogResult.OK)
                {
                    txtParamCaption.Focus();
                    return;
                }
            }
            //if (rtxtParamQuery.Text.Trim() == "" && cboParamType1.Text.Trim() == "VarChar")
            if (rtxtParamQuery.Text.Trim() == "" && txtParamType.Text.Trim() == "VarChar")     // Changed by Sachin N. S. on 05/04/2014 for Bug-4524
            {
                DialogResult save = MessageBox.Show("Please enter the parameter query", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (save == DialogResult.OK)
                {
                    rtxtParamQuery.Focus();
                    return;
                }
            }

            this.Validate();
            string paramName = this.txtParamName.Text;
            string paraType = this.txtparatype.Text;
            string paraCaption = this.txtParamCaption.Text;
            paratable.Rows[0]["ParamType"] = paraType;
            paratable.Rows[0]["ParamName"] = paramName;
            paratable.Rows[0]["ParaCaption"] = paraCaption;


            if (this.CheckValidation())
            {
                //checkgenpoppquery();
                //if (strreturn == "True")
                //{
                //    SaveData();
                //    if (strreturn == "")
                //        btnLast.PerformClick();
                //}
                SaveData();
                if (strreturn == "")
                    //btnLast.PerformClick();       // Commented by Sachin N. S. on 29/04/2014 for Bug-22077
                    this.NavigateData("Last");      // Added by Sachin N. S. on 29/04/2014 for Bug-22077
            }

            strreturn = "";
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            CurrentMode = Mode.Edit;
            ControlState(CurrentMode);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.txtParamName.CausesValidation = false;
            this.txtParamCaption.CausesValidation = false;
            CurrentMode = Mode.View;
            ControlState(CurrentMode);
            //btnLast.PerformClick();       // Commented by Sachin N. S. on 04/04/2014 for Bug-4524
            NavigateData("LAST");           // Added by Sachin N. S. on 04/04/2014 for Bug-4524
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection conn = new SqlConnection(this.SQLConnStr);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataSet ds = new DataSet();
                string paraid = txtParamId.Text;
                string paraquery = rtxtParamQuery.Text;
                tmpdata = 0;
                conn.Open();
                //cmd.CommandText = "select qryid from usrlv where qryid=" + paraid + "";
                cmd.CommandText = "select Top 1 ParameterId,RepId from Para_Query_master where ParameterId=" + paraid + "";
                adapter.SelectCommand = cmd;
                adapter.Fill(ds);
                adapter.Dispose();
                if (ds.Tables[0].Rows.Count < 1)
                {
                    DialogResult del = MessageBox.Show("Are you sure want to delete this record?", this.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (del == DialogResult.Yes)
                    {
                        SqlTransaction transaction = conn.BeginTransaction();
                        DataRow[] ldrs = paratable.Select("Parameterid=" + paraid);
                        try
                        {
                            cmd.Transaction = transaction;
                            cmd.Connection = conn;
                            cmd.CommandText = "Delete from usqry where qryid=" + Convert.ToString(ldrs[0]["QueryId"]);
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "Delete from para_master where ParameterId=" + paraid;
                            cmd.ExecuteNonQuery();
                            transaction.Commit();
                            conn.Close();
                        }
                        catch (SqlException sqlError)
                        {
                            MessageBox.Show(sqlError.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            transaction.Rollback();
                            conn.Close();
                        }
                    }
                    else
                    {
                        conn.Close();
                        tmpdata = 1;
                    }
                }
                else
                {

                    MessageBox.Show("Parameter has been used for the report Id: " + ds.Tables[0].Rows[0]["RepId"].ToString() + "\nEntry cannot be deleted ", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Question);
                    conn.Close();
                    tmpdata = 1;
                }


            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {

                if (tmpdata == 0)
                {
                    timer1.Enabled = true;
                    MessageBox.Show("Data deleted Successfully.", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DATACLEAR();
                    CurrentMode = Mode.View;
                    ControlState(CurrentMode);
                    //                    Filldata();
                    //btnLast.PerformClick();     // Commented by Sachin N. S. on 04/04/2014 for Bug-4524
                    NavigateData("LAST");           // Added by Sachin N. S. on 04/04/2014 for Bug-4524
                }
            }

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            dgvPopupFields.Rows.Clear();
            if (paratable != null)
                paratable.Rows.Clear();
            DataRow newRow = paratable.NewRow();
            newRow["ParamType"] = 0;
            paratable.Rows.Add(newRow);

            CurrentMode = Mode.Add;
            ControlState(CurrentMode);
            Paratypefill();
            txtParamName.Focus();
            try
            {
                SqlConnection conn = new SqlConnection(this.SQLConnStr);
                SqlCommand cmd = new SqlCommand("select max(ParameterId)+1 from Para_Master", conn);
                conn.Open();
                object ueqry = cmd.ExecuteScalar();
                txtParamId.Text = ueqry.ToString();
                paratable.Rows[0]["ParameterId"] = int.Parse(this.txtParamId.Text);
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            this.rtxtSearchFlds.Text = string.Empty;
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            CurrentMode = Mode.Add;
            ControlState(CurrentMode);
            txtParamName.Focus();
            try
            {
                SqlConnection conn = new SqlConnection(this.SQLConnStr);
                SqlCommand cmd = new SqlCommand("select max(ParameterId)+1 from Para_Master", conn);
                conn.Open();
                object ueqry = cmd.ExecuteScalar();
                txtParamId.Text = ueqry.ToString();
                paratable.Rows[0]["ParameterId"] = Convert.ToInt32(txtParamId.Text);
                paratable.Rows[0]["ParamName"] = string.Empty;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGeneratePopupFields_Click(object sender, EventArgs e)
        {
            if (this.CurrentMode == Mode.Add || this.CurrentMode == Mode.Edit)
            {
                if (rtxtParamQuery.Text.Trim() != "")
                {
                    tmpparaquery = rtxtSearchFlds.Text;
                    genquerypopup();
                    tmpfillgrid();
                    tmpparaquery = "";

                }
                else
                {
                    MessageBox.Show("Please enter the parameter query.", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    rtxtParamQuery.Focus();
                    return;
                }
            }
        }

        private void cboParamType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dgvPopupFields_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            GenerateSearchCondition();
        }
        private void GenerateSearchCondition()
        {
            text2 = "";
            text1 = "";
            tmpsearch = "";
            tmpfieldcaption = "";
            tmpreturn = "";

            foreach (DataGridViewRow row in dgvPopupFields.Rows)
            {
                if (text1 != "")
                {
                    //tmp = "+";
                    tmp = ",";
                }
                else
                {
                    tmp = "";
                }

                text1 = row.Cells["Col_Fieldname"].FormattedValue.ToString();
                text2 = row.Cells["Col_FieldCaption"].FormattedValue.ToString();
                if (text2 != "")
                {
                    tmpfieldcaption += tmp + text1.ToUpper() + ":" + text2;
                    text2 = "";
                    //text1 = "";

                }
            }
            text1 = "";
            foreach (DataGridViewRow row in dgvPopupFields.Rows)
            {
                if (text1 != "")
                {
                    tmp = "+";
                }
                else
                {
                    tmp = "";
                }

                text1 = row.Cells["Col_Fieldname"].FormattedValue.ToString();
                text2 = row.Cells["Col_search"].FormattedValue.ToString();
                if (text2 != "False")
                {
                    string fldVal = string.Empty;
                    switch (row.Cells["colType"].Value.ToString())
                    {
                        case "System.Int16":
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Double":
                        case "System.Decimal":
                            fldVal = "str(" + text1 + ")";
                            break;
                        case "System.DateTime":
                            fldVal = "Ttos(" + text1 + ")";
                            break;
                        default:
                            fldVal = text1;
                            break;
                    }
                    tmpsearch += tmp + fldVal;
                    text2 = "";
                    //text1 = "";
                }
            }

            foreach (DataGridViewRow row in dgvPopupFields.Rows)
            {
                text1 = row.Cells["Col_Fieldname"].FormattedValue.ToString();
                text2 = row.Cells["Col_return"].FormattedValue.ToString();
                if (text2 != "False")
                {
                    tmpreturn = text1;
                    text2 = "";
                    text1 = "";
                }
            }


            if (tmpfieldcaption != null || tmpsearch != null || tmpreturn != null)
            {
                tmpParam_filtcond = "{" + tmpsearch.ToUpper() + "#" + tmpreturn.ToUpper() + "##" + tmpfieldcaption + "}";
                rtxtSearchFlds.Text = tmpParam_filtcond;
            }
        }

        private void ControlState(Mode md)
        {
            if (md == Mode.View)
            {
                txtParamName.Enabled = false;
                txtParamCaption.Enabled = false;
                //cboParamType1.Enabled = false;        // Commented by Sachin N. S. on 05/04/2014 for Bug-4524
                btnParamType.Enabled = false;           // Added by Sachin N. S. on 05/04/2014 for Bug-4524
                chkTo.Enabled = false;
                rtxtParamQuery.Enabled = false;
                btnGeneratePopupFields.Enabled = false;
                dgvPopupFields.Enabled = false;
                btnSave.Enabled = false;
                btnNew.Enabled = true;
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
                btnCancel.Enabled = false;
                btnCopy.Enabled = true;
                //NavButtonState(true, true, true, true);
            }
            else if (md == Mode.Edit)
            {
                txtParamName.Enabled = true;
                txtParamCaption.Enabled = true;
                //cboParamType1.Enabled = true;     // Commented by Sachin N. S. on 05/04/2014 for Bug-4524
                btnParamType.Enabled = true;           // Added by Sachin N. S. on 05/04/2014 for Bug-4524
                chkTo.Enabled = true;
                rtxtParamQuery.Enabled = true;
                btnGeneratePopupFields.Enabled = true;
                dgvPopupFields.Enabled = true;
                btnSave.Enabled = true;
                btnNew.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnCancel.Enabled = true;
                btnCopy.Enabled = false;
                NavButtonState(false, false, false, false);
            }
            else if (md == Mode.DefValue)
            {
                txtParamName.Enabled = false;
                txtParamCaption.Enabled = false;
                //cboParamType1.Enabled = false;        // Commented by Sachin N. S. on 05/04/2014 for Bug-4524
                btnParamType.Enabled = false;           // Added by Sachin N. S. on 05/04/2014 for Bug-4524
                chkTo.Enabled = false;
                rtxtParamQuery.Enabled = false;
                btnGeneratePopupFields.Enabled = false;
                dgvPopupFields.Enabled = false;
                btnSave.Enabled = false;
                btnNew.Enabled = true;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnCancel.Enabled = false;
                btnCopy.Enabled = false;
                NavButtonState(false, false, false, false);
            }
            else
            {
                txtParamName.Enabled = true;
                txtParamCaption.Enabled = true;
                //cboParamType1.Enabled = true;     // Commented by Sachin N. S. on 05/04/2014 for Bug-4524
                btnParamType.Enabled = true;        // Added by Sachin N. S. on 05/04/2014 for Bug-4524
                chkTo.Enabled = true;
                rtxtParamQuery.Enabled = true;
                btnGeneratePopupFields.Enabled = true;
                dgvPopupFields.Enabled = true;
                btnSave.Enabled = true;
                btnNew.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnCancel.Enabled = true;
                btnCopy.Enabled = false;
                NavButtonState(false, false, false, false); ;
            }
        }
        private void NavButtonState(bool First, bool Prev, bool Next, bool Last)
        {
            btnFirst.Enabled = First;
            btnBack.Enabled = Prev;
            btnForward.Enabled = Next;
            btnLast.Enabled = Last;

        }
        private void fillgrid()
        {
            string selectSQL;
            string tmppart1, tmppart2, tmppart3, tmppart4;
            //selectSQL = "SELECT SearchFlds FROM para_master where  ParameterId=" + txtParamId.Text;
            selectSQL = "SELECT SearchFlds FROM para_master where  ParameterId=" + Convert.ToString(paratable.Rows[0]["ParameterId"]);
            
            //MessageBox.Show(selectSQL);
            SqlConnection con = new SqlConnection(this.SQLConnStr);
            SqlCommand cmd1 = new SqlCommand(selectSQL, con);
            SqlDataReader reader;
            con.Open();
            reader = cmd1.ExecuteReader();
            while (reader.Read())
            {
                tmppart1 = reader["SearchFlds"].ToString();

                //MessageBox.Show(tmppart1);

                if (CurrentMode == Mode.Add)
                {
                    //tmppart1 = tmpparaquery;
                    tmppart1 = rtxtParamQuery.Text;
                }
                rtxtSearchFlds.Text = tmppart1;
                if (tmppart1 != "{###}" && tmppart1 != "")
                {
                    int i;
                    char[] delimiters = new char[] { '#' };
                    string[] parts = tmppart1.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    tmppart3 = "";
                    tmppart4 = "";
                    tmppart2 = "";
                    for (i = 0; i < parts.Length; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                tmppart3 = parts[0];
                                break;
                            case 1:
                                tmppart4 = parts[1];
                                break;
                            case 2:
                                tmppart2 = parts[2];
                                break;
                            default:
                                break;
                        }
                        //tmppart3 = parts[0];
                        //tmppart4 = parts[1];
                        //tmppart2 = parts[2];
                    }
                    //char[] delimiters1 = new char[] { '+', '}' };

                    //MessageBox.Show("asasas - 1");

                    char[] delimiters1 = new char[] { ',', '}' };
                    string[] parts1 = tmppart2.Split(delimiters1, StringSplitOptions.RemoveEmptyEntries);
                    //string[] parts1 = tmppart2.Split(delimiters1);
                    for (int y = 0; y < parts1.Length; y++)
                    {
                        string part3 = parts1[y].Substring(parts1[y].LastIndexOf(':') + 1);

                        dgvPopupFields.Rows[y].Cells[1].Value = part3;
                    }

                    //MessageBox.Show("asasas - 2");

                    char[] delimiters2 = new char[] { ',', '+', '{' };
                    string[] parts2 = tmppart3.Split(delimiters2, StringSplitOptions.RemoveEmptyEntries);
                    for (int y = 0; y < parts2.Length; y++)
                    {
                        tmppart3 = parts2[y];
                        foreach (DataGridViewRow row in dgvPopupFields.Rows)
                        {
                            text1 = row.Cells["Col_Fieldname"].FormattedValue.ToString();

                            string fldVal = string.Empty;
                            switch (row.Cells["colType"].Value.ToString())
                            {
                                case "System.Int16":
                                case "System.Int32":
                                case "System.Int64":
                                case "System.Double":
                                case "System.Decimal":
                                    fldVal = "str(" + text1 + ")";
                                    break;
                                case "System.DateTime":
                                    fldVal = "Ttos(" + text1 + ")";
                                    break;
                                default:
                                    fldVal = text1;
                                    break;
                            }
                            if (fldVal.ToUpper() == tmppart3.ToUpper())
                            {
                                y = row.Index;
                                dgvPopupFields.Rows[y].Cells[2].Value = true;
                            }
                        }

                        foreach (DataGridViewRow row in dgvPopupFields.Rows)
                        {
                            text1 = row.Cells["Col_Fieldname"].FormattedValue.ToString();
                            if (text1.ToUpper() == tmppart4.ToUpper())
                            {
                                int z = row.Index;
                                dgvPopupFields.Rows[z].Cells[3].Value = true;
                            }
                        }
                    }

                    //MessageBox.Show("asasas - 3");
                }
            }
            reader.Close();
            con.Close();
        }

        private void Paratypefill()
        {
            switch (txtparatype.Text)
            {
                case "0":
                    para_type = "VarChar";
                    break;
                case "1":
                    para_type = "Numeric";
                    break;
                case "2":
                    para_type = "DateTime";
                    break;
                default:
                    break;
            }
            //cboParamType1.Text = para_type;
            txtParamType.Text = para_type;      // Changed by Sachin N. S. on 05/04/2014 for Bug-4524
        }

        private void genquerypopup()
        {
            try
            {
                //dgvPopupFields.Rows.Clear();
                if (rtxtParamQuery.TextLength > 0)
                {
                    SqlConnection conn = new SqlConnection(this.SQLConnStr);
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.CommandText = rtxtParamQuery.Text;


                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(sqlCommand.CommandText, conn);
                    da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                    try
                    {
                        da.Fill(ds);
                        dgvPopupFields.Rows.Clear();
                    }
                    catch (SqlException sqlError)
                    {
                        MessageBox.Show(sqlError.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    foreach (DataColumn dc in ds.Tables[0].Columns)
                    {
                        dgvPopupFields.Rows.Add(new object[] { dc.ColumnName, string.Empty, false, false, dc.DataType.ToString() });
                    }
                }

            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


        }
        private void checkgenpoppquery()
        {
            try
            {
                if (rtxtParamQuery.TextLength > 0)
                {
                    dgvPopupFields.Rows.Clear();

                    SqlConnection conn = new SqlConnection();
                    conn.ConnectionString = this.SQLConnStr;
                    SqlCommand sqlCommand = new SqlCommand();
                    {
                        sqlCommand.CommandText = rtxtParamQuery.Text;
                        DataSet ds = new DataSet();
                        SqlDataAdapter da = new SqlDataAdapter(sqlCommand.CommandText, conn);
                        da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                        da.Fill(ds);
                        strreturn = "True";

                    }
                }
                //else if (this.cboParamType1.Text == "Numeric" || this.cboParamType1.Text == "DateTime")
                else if (this.txtParamType.Text == "Numeric" || this.txtParamType.Text == "DateTime")       // Changed by Sachin N. S. on 05/04/2014 for Bug-4524
                {
                    strreturn = "True";
                }

            }
            catch (SqlException ex)
            {

                DialogResult save = MessageBox.Show(ex.Message, this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (save == DialogResult.OK)
                {
                    //MessageBox.Show(ex.Message, "Problem in genarated Query");
                    rtxtParamQuery.Focus();
                    strreturn = "False";
                    return;
                }

            }
        }
        private void SaveData()
        {
            bool RecUpdated = false;

            try
            {
                strreturn = "";     // Added by Sachin N. S. on 29/04/2014 for Bug-4524
                if (CurrentMode == Mode.Add)
                {
                    if (this.CheckDuplicate())
                    {
                        strreturn = "";
                        return;
                    }
                    SqlConnection conn = new SqlConnection(this.SQLConnStr);
                    paraid = txtParamId.Text;
                    string paraquery = rtxtParamQuery.Text;

                    conn = new SqlConnection(this.SQLConnStr);
                    SqlCommand cmd = new SqlCommand("select max(QryId)+1 from UsQry", conn);
                    conn.Open();
                    object ueqry = cmd.ExecuteScalar();
                    string QueryId = ueqry.ToString();
                    conn.Close();

                    string sqlstring = "insert into usqry(QRYID,RepQry,RepID,RepLvlID) values(@QueryId,@RepQry,'','')";
                    cmd = new SqlCommand(sqlstring, conn);
                    cmd.Parameters.Add(new SqlParameter("@QueryId", QueryId));
                    cmd.Parameters.Add(new SqlParameter("@RepQry", paraquery));
                    cmd.Connection = conn;
                    conn.Open();

                    paraid = txtParamId.Text;
                    para_type = txtparatype.Text;
                    string para_caption = txtParamName.Text;
                    paratable.Rows[0]["QueryId"] = QueryId;

                    SqlTransaction transaction = conn.BeginTransaction();
                    try
                    {
                        cmd.Transaction = transaction;
                        cmd.Connection = conn;
                        //if (this.cboParamType1.Text == "VarChar")
                        if (this.txtParamType.Text == "VarChar")       // Changed by Sachin N. S. on 05/04/2014 for Bug-4524
                        {
                            paratable.Rows[0]["IsQuery"] = true;
                            cmd.ExecuteNonQuery();
                            txtparatype.Text = "0";                       // Added by Sachin N. S. on 05/04/2014 for Bug-4524
                        }
                        //if (this.cboParamType1.Text == "Numeric" || this.cboParamType1.Text == "DateTime")
                        if (this.txtParamType.Text == "Numeric" || this.txtParamType.Text == "DateTime")  // Changed by Sachin N. S. on 05/04/2014 for Bug-4524
                        {
                            tmpParam_filtcond = string.Empty;
                            txtparatype.Text = this.txtParamType.Text == "Numeric" ? "1" : "2";     // Added by Sachin N. S. on 05/04/2014 for Bug-4524
                        }
                        sqlstring = "insert into Para_Master(ParameterId,ParamName,ParaCaption,QueryId,IsQuery,ParamType,SearchFlds) values (" + txtParamId.Text.Trim() + ",'" + txtParamName.Text.Trim() + "','" + txtParamCaption.Text.Trim() + "'," + QueryId + ",1," + txtparatype.Text.Trim() + ",'" + tmpParam_filtcond + "')";
                        cmd.CommandText = sqlstring;
                        cmd.Connection = conn;
                        cmd.ExecuteNonQuery();
                        transaction.Commit();
                        RecUpdated = true;
                        conn.Close();
                        paratable.Rows[0].AcceptChanges();
                    }
                    catch (SqlException sqlError)
                    {
                        MessageBox.Show(sqlError.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        transaction.Rollback();
                        RecUpdated = false;
                    }
                    conn.Close();
                    genquerypopup();
                    fillgrid();
                }

                else if (CurrentMode == Mode.Edit)
                {
                    SqlConnection conn = new SqlConnection(this.SQLConnStr);
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    paraid = txtParamId.Text;
                    string paraquery = rtxtParamQuery.Text;
                    if (Convert.ToInt32(paratable.Rows[0]["QueryId"]) > 0)
                        cmd.CommandText = "update usqry set RepQry='" + paraquery + "' where qryid='" + Convert.ToString(paratable.Rows[0]["QueryId"]) + "'";
                    else
                    {
                        //if (this.cboParamType1.Text == "VarChar")
                        if (this.txtParamType.Text == "VarChar")       // Changed by Sachin N. S. on 05/04/2014 for Bug-4524
                        {
                            cmd.CommandText = "select max(QryId)+1 from UsQry";
                            conn.Open();
                            object ueqry = cmd.ExecuteScalar();
                            string QueryId = ueqry.ToString();
                            conn.Close();

                            cmd.CommandText = "insert into usqry(QRYID,RepQry,RepID,RepLvlID) values(@QueryId,@RepQry,'','')";
                            cmd.Parameters.Add(new SqlParameter("@QueryId", QueryId));
                            cmd.Parameters.Add(new SqlParameter("@RepQry", paraquery));
                            paratable.Rows[0]["QueryId"] = Convert.ToInt32(QueryId);
                            paratable.Rows[0]["IsQuery"] = true;
                        }
                    }
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();
                    try
                    {
                        cmd.Transaction = transaction;
                        cmd.Connection = conn;
                        //if (this.cboParamType1.Text == "VarChar")
                        if (this.txtParamType.Text == "VarChar")       // Changed by Sachin N. S. on 05/04/2014 for Bug-4524
                        {
                            cmd.ExecuteNonQuery();
                        }

                        if (tmpParam_filtcond == "{###}")
                        {
                            tmpParam_filtcond = rtxtSearchFlds.Text;
                        }
                        //if (this.cboParamType1.Text == "Numeric" || this.cboParamType1.Text == "DateTime")
                        if (this.txtParamType.Text == "Numeric" || this.txtParamType.Text == "DateTime")      // Changed by Sachin N. S. on 05/04/2014 for Bug-4524
                        {
                            tmpParam_filtcond = string.Empty;
                        }
                        cmd.CommandText = "update para_master set SearchFlds='" + tmpParam_filtcond + "',ParamName='" + txtParamName.Text.Trim() + "',paracaption='" + this.txtParamCaption.Text.Trim() + "',paramtype=" + txtparatype.Text.Trim() + ",QueryId=" + Convert.ToString(paratable.Rows[0]["QueryId"]) + ",IsQuery=" + (Convert.ToBoolean(paratable.Rows[0]["IsQuery"]) ? "1" : "0") + " where ParameterId='" + paraid + "'";
                        cmd.ExecuteNonQuery();
                        transaction.Commit();
                        conn.Close();
                        RecUpdated = true;
                        paratable.Rows[0].AcceptChanges();
                    }
                    catch (SqlException sqlError)
                    {
                        MessageBox.Show(sqlError.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        transaction.Rollback();
                        RecUpdated = false;
                    }
                    genquerypopup();
                    fillgrid();
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                if (RecUpdated)
                {
                    timer1.Enabled = true;
                    MessageBox.Show("Data saved Successfully", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CurrentMode = Mode.View;
                    ControlState(CurrentMode);
                }
                //else
                //{
                //    timer1.Enabled = true;
                //    MessageBox.Show("Data updated Successfully", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}


            }

        }
        private bool CheckValidation()
        {
            int ReturnFieldCount = 0;
            int SearchFieldcount = 0;
            foreach (DataGridViewRow row in dgvPopupFields.Rows)
            {
                if (Convert.ToBoolean(row.Cells["Col_return"].Value) == true)
                    ReturnFieldCount++;
                if (Convert.ToBoolean(row.Cells["Col_Search"].Value) == true)
                    SearchFieldcount++;
            }
            //if (SearchFieldcount == 0 && this.cboParamType1.Text == "VarChar")
            if (SearchFieldcount == 0 && this.txtParamType.Text == "VarChar")      // Changed by Sachin N. S. on 05/04/2014 for Bug-4524
            {
                MessageBox.Show("Please select atleast one search field.", AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            //if (ReturnFieldCount == 0 && this.cboParamType1.Text == "VarChar")
            if (ReturnFieldCount == 0 && this.txtParamType.Text == "VarChar")      // Changed by Sachin N. S. on 05/04/2014 for Bug-4524
            {
                MessageBox.Show("Please select atleast one return field.", AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            //else if (ReturnFieldCount > 1 && this.cboParamType1.Text == "VarChar")
            else if (ReturnFieldCount > 1 && this.txtParamType.Text == "VarChar")      // Changed by Sachin N. S. on 05/04/2014 for Bug-4524
            {
                MessageBox.Show("Please select only one return field.", AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }
        private void checkgriddate()
        {
            string checkgrid_value;
            checkgrid_value = "";
            int returnCount = 0;
            foreach (DataGridViewRow row in dgvPopupFields.Rows)
            {
                text1 = row.Cells["Col_return"].FormattedValue.ToString();
                if (text1 == "True")
                {
                    checkgrid_value = "1";
                    strreturn = "True";
                    returnCount++;
                }

            }
            if (checkgrid_value != "1")
            {
                DialogResult save = MessageBox.Show("Please select any one Return Value", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (save == DialogResult.OK)
                {
                    // MessageBox.Show("Please Select any one Return Value");
                    dgvPopupFields.Focus();
                    strreturn = "False";
                    return;
                }
            }
            checkgrid_value = "";
            foreach (DataGridViewRow row in dgvPopupFields.Rows)
            {
                text1 = row.Cells["Col_Search"].FormattedValue.ToString();
                if (text1 == "True")
                {
                    checkgrid_value = "1";
                    strreturn = "True";
                }

            }
            if (checkgrid_value != "1")
            {
                DialogResult save = MessageBox.Show("Please select atleast one search value", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (save == DialogResult.OK)
                {
                    //  MessageBox.Show("Please Select any one Search Value");
                    dgvPopupFields.Focus();
                    strreturn = "False";
                    return;
                }
            }

            checkgrid_value = "";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            SendKeys.Send("{ENTER}");
            timer1.Enabled = false;
        }
        private void DATACLEAR()
        {
            txtParamId.Text = "";
            rtxtParamQuery.Text = "";
            txtParamName.Text = "";
            chkTo.Checked = false;
            txtparatype.Text = "";
            txtParamId.Text = "";
            rtxtParamQuery.Text = "";
            txtParamName.Text = "";
            txtparatype.Text = "";
            //cboParamType1.Text = "";
            txtParamType.Text = "";        // Changed by Sachin N. S. on 05/04/2014 for Bug-4524
            chkTo.Checked = false;
            dgvPopupFields.Rows.Clear();
            rtxtSearchFlds.Text = "";
        }

        private void tmpfillgrid()
        {
            string tmppart1 = tmpparaquery;


            if (CurrentMode == Mode.Add)
            {
                tmppart1 = tmpparaquery;
            }
            rtxtSearchFlds.Text = tmppart1;
            if (tmppart1 != "{###}" && tmppart1 != "")
            {
                int i;
                char[] delimiters = new char[] { '#' };
                string[] parts = tmppart1.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                //string[] parts = tmppart1.Split(delimiters);
                string tmppart3 = "";
                string tmppart4 = "";
                string tmppart2 = "";
                for (i = 0; i < parts.Length; i++)
                {
                    switch (i)
                    {
                        case 0:
                            tmppart3 = parts[0];
                            break;
                        case 1:
                            tmppart4 = parts[1];
                            break;
                        case 2:
                            tmppart2 = parts[2];
                            break;
                        default:
                            break;
                    }
                }

                char[] delimiters2 = new char[] { ',', '+', '{' };
                string[] parts2 = tmppart3.Split(delimiters2, StringSplitOptions.RemoveEmptyEntries);

                for (int yy = 0; yy < parts2.Length; yy++)
                {
                    tmppart3 = parts2[yy];
                    foreach (DataGridViewRow row in dgvPopupFields.Rows)
                    {

                        text1 = row.Cells["Col_Fieldname"].FormattedValue.ToString();
                        string fldVal = string.Empty;
                        switch (row.Cells["colType"].Value.ToString())
                        {
                            case "System.Int16":
                            case "System.Int32":
                            case "System.Int64":
                            case "System.Double":
                            case "System.Decimal":
                                fldVal = "str(" + text1 + ")";
                                break;
                            case "System.DateTime":
                                fldVal = "Ttos(" + text1 + ")";
                                break;
                            default:
                                fldVal = text1;
                                break;
                        }
                        if (fldVal.ToUpper() == tmppart3.ToUpper())
                        {
                            int y = row.Index;
                            dgvPopupFields.Rows[y].Cells[2].Value = true;
                        }

                    }

                    foreach (DataGridViewRow row in dgvPopupFields.Rows)
                    {
                        text1 = row.Cells["Col_Fieldname"].FormattedValue.ToString();
                        if (text1.ToUpper() == tmppart4.ToUpper())
                        {
                            int z = row.Index;
                            dgvPopupFields.Rows[z].Cells[3].Value = true;
                        }

                    }
                }

                char[] delimiters1 = new char[] { ',', '}' };
                string[] parts1 = tmppart2.Split(delimiters1, StringSplitOptions.RemoveEmptyEntries);
                for (int y = 0; y < parts1.Length; y++)
                {

                    delimiters1 = new char[] { ':' };
                    string[] parts8 = parts1[y].Split(delimiters1, StringSplitOptions.RemoveEmptyEntries);
                    text1 = "";
                    int aa = 00;
                    foreach (DataGridViewRow row in dgvPopupFields.Rows)
                    {

                        text1 = row.Cells["Col_Fieldname"].FormattedValue.ToString();

                        if (text1.ToUpper() == parts8[0].ToUpper())
                        {
                            dgvPopupFields.Rows[aa].Cells[1].Value = parts8[1];
                        }
                        aa = aa + 1;
                    }
                }


            }
        }

        private void dgvPopupFields_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                dgvPopupFields.ClearSelection();
                dgvPopupFields.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvPopupFields.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue;
                if (CurrentMode == Mode.Add || CurrentMode == Mode.Edit)
                {
                    int ReturnFieldCount = 0;
                    foreach (DataGridViewRow row in dgvPopupFields.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells["Col_return"].Value) == true)
                            ReturnFieldCount++;
                    }
                    if (ReturnFieldCount > 1)
                    {
                        MessageBox.Show("Please select only one return field.", AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        dgvPopupFields.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = false;
                        dgvPopupFields.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
                        dgvPopupFields.RefreshEdit();
                        e.Cancel = true;
                        return;
                    }
                }
            }

            this.GenerateSearchCondition();
        }
        private bool CheckDuplicate()
        {
            try
            {
                SqlConnection conn = new SqlConnection(this.SQLConnStr);
                SqlCommand cmd = new SqlCommand("select Top 1 ParamName from Para_Master Where ParamName=@ParamName", conn);
                cmd.Parameters.Add(new SqlParameter("@ParamName", this.txtParamName.Text));
                conn.Open();
                string result = Convert.ToString(cmd.ExecuteScalar());
                if (result.Length > 0)
                {
                    MessageBox.Show("Parameter name already exists.", AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return true;
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return false;
        }

        private void txtParamName_Validating(object sender, CancelEventArgs e)
        {
            //if (this.CurrentMode == Mode.Add || this.CurrentMode == Mode.Edit)
            //{
            //    if (this.txtParamName.Text.Trim().Length == 0)
            //    {
            //        DialogResult save = MessageBox.Show("Please enter the parameter name.", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        if (save == DialogResult.OK)
            //        {
            //            txtParamName.Focus();
            //            return;
            //        }
            //        e.Cancel = true;
            //        return;
            //    }
            //    if (this.CheckDuplicate())
            //        e.Cancel = true;
            //}
            if (this.CurrentMode == Mode.Add)
            {
                paratable.Rows[0]["paramName"] = this.txtParamName.Text;
                //if (this.txtParamName.Text.Trim().Length > 0)
                //{
                //    if (this.CheckDuplicate())
                //        e.Cancel = true;
                //}
            }
        }

        private void txtParamCaption_Validating(object sender, CancelEventArgs e)
        {
            //if (this.CurrentMode == Mode.Add || this.CurrentMode == Mode.Edit)
            //{
            //    if (this.txtParamCaption.Text.Trim().Length == 0)
            //    {
            //        DialogResult save = MessageBox.Show("Please enter the parameter caption.", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        if (save == DialogResult.OK)
            //        {
            //            txtParamCaption.Focus();
            //            return;
            //        }
            //        e.Cancel = true;
            //        return;
            //    }
            //}
            if (this.CurrentMode == Mode.Add || this.CurrentMode == Mode.Edit)
            {
                paratable.Rows[0]["paraCaption"] = this.txtParamCaption.Text;
            }
        }

        private void ParameterMaster_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.CausesValidation = false;
            this.txtParamName.CausesValidation = false;
            this.txtParamCaption.CausesValidation = false;
            if (this.CurrentMode == Mode.Add || this.CurrentMode == Mode.Edit)
            {
                DialogResult Ans = MessageBox.Show("Do you want to save the changes first?", this.AppName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                switch (Ans)
                {
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                    case DialogResult.No:
                        e.Cancel = false;
                        break;
                    case DialogResult.Yes:
                        this.txtParamName.CausesValidation = true;
                        this.txtParamCaption.CausesValidation = true;
                        this.btnSave.PerformClick();
                        if (this.CurrentMode == Mode.View)
                            e.Cancel = false;
                        else
                            e.Cancel = true;
                        break;
                }

            }
        }
        private void InsertProcessIdRecord()
        {
            string sqlstr = string.Empty;
            sqlstr = " insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values(" +
                        "@pApplCode,@CallDate,@pApplNm,@pApplId,@pApplDesc,@cApplNm,@cApplId,@cApplDesc)";

            SqlConnection con = new SqlConnection(this.SQLConnStr);
            SqlCommand cmd = new SqlCommand(sqlstr, con);
            cmd.Parameters.Add(new SqlParameter("@pApplCode", pApplCode));
            cmd.Parameters.Add(new SqlParameter("@CallDate", DateTime.Now.ToString("MM/dd/yyyy")));
            cmd.Parameters.Add(new SqlParameter("@pApplNm", pApplNm));
            cmd.Parameters.Add(new SqlParameter("@pApplId", pApplId));
            cmd.Parameters.Add(new SqlParameter("@pApplDesc", AppName));
            cmd.Parameters.Add(new SqlParameter("@cApplNm", cApplNm));
            cmd.Parameters.Add(new SqlParameter("@cApplId", cApplId));
            cmd.Parameters.Add(new SqlParameter("@cApplDesc", this.Text));
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DeleteProcessIdRecord()
        {
            string sqlstr = string.Empty;
            sqlstr = " Delete from vudyog..ExtApplLog where pApplNm=@pApplNm and pApplId=@pApplId and cApplNm= @cApplNm and cApplId= @cApplId";
            SqlConnection con = new SqlConnection(this.SQLConnStr);
            SqlCommand cmd = new SqlCommand(sqlstr, con);
            cmd.Parameters.Add(new SqlParameter("@pApplNm", pApplNm));
            cmd.Parameters.Add(new SqlParameter("@pApplId", pApplId));
            cmd.Parameters.Add(new SqlParameter("@cApplNm", cApplNm));
            cmd.Parameters.Add(new SqlParameter("@cApplId", cApplId));
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ParameterMaster_FormClosed(object sender, FormClosedEventArgs e)
        {
            DeleteProcessIdRecord();
        }

        //****** Added by Sachin N. S. on 04/04/2014 for Bug-4524 -- Start
        private void dgvPopupFields_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvPopupFields.Columns["Col_Search"].Index && e.RowIndex >= 0)
            {
                dgvPopupFields.ClearSelection();
                dgvPopupFields.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvPopupFields.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue;
                if (CurrentMode == Mode.Add || CurrentMode == Mode.Edit)
                {
                    GenerateSearchCondition();
                }
            }

            if (e.ColumnIndex == 3 && e.RowIndex >= 0)
            {
                dgvPopupFields.ClearSelection();
                dgvPopupFields.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvPopupFields.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue;
                if (CurrentMode == Mode.Add || CurrentMode == Mode.Edit)
                {
                    int ReturnFieldCount = 0;
                    foreach (DataGridViewRow row in dgvPopupFields.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells["Col_return"].Value) == true)
                            ReturnFieldCount++;
                    }
                    if (ReturnFieldCount > 1)
                    {
                        MessageBox.Show("Please select only one return field.", AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        dgvPopupFields.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = false;
                        dgvPopupFields.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
                        dgvPopupFields.RefreshEdit();
                        return;
                    }
                }
            }
        }

        private void NavigateData(string _naviPos)
        {
            string _paraId = this.txtParamId.Text;

            //MessageBox.Show("Test - aa - 1");

            DataSet _ds = new DataSet();
            DataTable dt;
            string _NaviBtn = "0000", strrep = "";
            strrep = "EXECUTE NAVIGATE_DADOS_PARA_MAST_DATA '" + _naviPos + "', " + (_paraId == "" ? "0" : _paraId.ToString()) + "";
            _ds = _oDataAccess.GetDataSet(strrep, null, 50);
            dt = _ds.Tables[0];
            _NaviBtn = _ds.Tables[1].Rows[0][0].ToString();

            //MessageBox.Show("Test - aa - 2");

            if (paratable != null)
            {
                paratable.Rows.Clear();
            }

            //MessageBox.Show("Test - aa - 3");

            paratable.Merge(dt);

            //MessageBox.Show("Test - aa - 4");

            Paratypefill();

            //MessageBox.Show("Test - aa - 5");
            
            this.SetDataBinding();

            if (rtxtParamQuery.Text != "")
            {
                genquerypopup();
            }

            //MessageBox.Show("Test - aa - 6");

            if (paratable.Rows.Count > 0)
                CurrentMode = Mode.View;
            else
                CurrentMode = Mode.DefValue;

            ControlState(CurrentMode);

            //MessageBox.Show("Test - aa - 7");

            fillgrid();

            //MessageBox.Show("Test - aa - 8");
            NavButtonState(_NaviBtn.Substring(0, 1) == "1" ? true : false, _NaviBtn.Substring(1, 1) == "1" ? true : false, _NaviBtn.Substring(2, 1) == "1" ? true : false, _NaviBtn.Substring(3, 1) == "1" ? true : false);

            //MessageBox.Show("Test - aa - 9");
        }

        private void btnParamType_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataTable _dt = new DataTable();
            DataRow _dr;
            _dt.Columns.Add(new DataColumn("ParamType", typeof(string)));
            _dr = _dt.NewRow();
            _dr[0] = "VarChar";
            _dt.Rows.Add(_dr);
            _dr = _dt.NewRow();
            _dr[0] = "Numeric";
            _dt.Rows.Add(_dr);
            _dr = _dt.NewRow();
            _dr[0] = "DateTime";
            _dt.Rows.Add(_dr);

            DataView dvw = _dt.DefaultView;

            VForText = "Select Parameter Type";
            vSearchCol = "ParamType";
            vDisplayColumnList = "ParamType:Parameter Type";
            vReturnCol = "ParamType";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                txtParamType.Text = oSelectPop.pReturnArray[0];
                //if (this.pAddMode == false && this.pEditMode == false)
                //{
                switch (txtParamType.Text)
                {
                    case "VarChar":
                        paratype = 0;
                        rtxtParamQuery.Enabled = true;
                        btnGeneratePopupFields.Enabled = true;
                        break;
                    case "Numeric":
                        paratype = 1;
                        rtxtParamQuery.Text = string.Empty;
                        rtxtParamQuery.Enabled = false;
                        btnGeneratePopupFields.Enabled = false;
                        dgvPopupFields.Rows.Clear();
                        rtxtSearchFlds.Text = string.Empty;
                        paratable.Rows[0]["searchflds"] = string.Empty;
                        paratable.Rows[0]["repqry"] = string.Empty;
                        paratable.Rows[0]["QueryId"] = 0;
                        paratable.Rows[0]["IsQuery"] = 0;
                        rtxtSearchFlds.Refresh();
                        break;
                    case "DateTime":
                        paratype = 2;
                        rtxtParamQuery.Text = string.Empty;
                        rtxtParamQuery.Enabled = false;
                        btnGeneratePopupFields.Enabled = false;
                        dgvPopupFields.Rows.Clear();
                        rtxtSearchFlds.Text = string.Empty;
                        paratable.Rows[0]["searchflds"] = string.Empty;
                        paratable.Rows[0]["repqry"] = string.Empty;
                        paratable.Rows[0]["QueryId"] = 0;
                        paratable.Rows[0]["IsQuery"] = 0;
                        rtxtSearchFlds.Refresh();
                        break;
                    default:
                        break;
                }
                //txtparatype.Text = Convert.ToString(paratype);
                //}
            }
        }
        //****** Added by Sachin N. S. on 04/04/2014 for Bug-4524 -- End
    }
}
