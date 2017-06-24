using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows.Forms;
using System.Transactions;

namespace DadosReportParameterMaster
{
    public partial class ParameterMast : Form
    {
        //variables of calling Application 
        private string pApplCode = string.Empty;
        private string pApplNm = string.Empty;
        private string pApplId = string.Empty;
        private string pApplDesc = string.Empty;

        //variables of this application
        private string cApplNm = string.Empty;
        private string cApplId = string.Empty;

        public ParameterMast(string[] args)
        {
            InitializeComponent();
            //SQLConnStr = "Data source=" + args[2] + ";Initial Catalog=" + args[1] + ";Uid=" + args[3] + ";Pwd=" + args[4];
            //AppName = args[8].Replace("<*#*>", " ");
            //this.Icon = new System.Drawing.Icon(args[7]);

            //pApplCode = args[11].Replace("<*#*>", " ");
            //pApplNm = args[9];
            //pApplId = args[10];

            cApplNm = typeof(Program).Assembly.GetName().Name + ".exe";
            cApplId = Convert.ToString(Process.GetCurrentProcess().Id);

            SQLConnStr = "Data source=Prod_shrikant\\shree;Initial Catalog=C011213;Uid=sa;Pwd=sa1985";
            this.AppName = "sdk";

        }
        string SQLConnStr = string.Empty;
        string AppName = string.Empty;
        DataTable paratable = new DataTable("Parameter_vw");

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
        private void frmparamaster_Load(object sender, EventArgs e)
        {
            //Filldata();
            this.MoveLast();
            this.SetDataBinding();
            genquerypopup();
            Paratypefill();
            fillgrid();
            CurrentMode = Mode.View;
            ControlState(CurrentMode);
            InsertProcessIdRecord();
            //btnLast.PerformClick();

        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            //paraIDManager.Position = 0;
            // btngenpopup.PerformClick();
            this.MoveFirst();
            genquerypopup();
            Paratypefill();
            fillgrid();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            //paraIDManager.Position--;
            // btngenpopup.PerformClick();
            this.MovePrevious();
            genquerypopup();
            Paratypefill();
            fillgrid();
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            //paraIDManager.Position++;
            //btngenpopup.PerformClick();
            this.MoveNext();
            genquerypopup();
            Paratypefill();
            fillgrid();
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            //paraIDManager.Position = paraIDManager.Count - 1;
            // btngenpopup.PerformClick();
            this.MoveLast();
            genquerypopup();
            Paratypefill();
            fillgrid();
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
            if (rtxtParamQuery.Text.Trim() == "" && cboParamType.Text.Trim() == "VarChar")
            {
                DialogResult save = MessageBox.Show("Please enter the parameter query", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (save == DialogResult.OK)
                {
                    rtxtParamQuery.Focus();
                    return;
                }
            }
            this.Validate();
            if (this.CheckValidation())
            {
                checkgenpoppquery();
                if (strreturn == "True")
                {
                    SaveData();
                    btnLast.PerformClick();
                }
            }

            strreturn = "";

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            CurrentMode = Mode.Edit;

            ControlState(CurrentMode);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // int count = command.ExecuteScalar();


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
                    //Filldata();
                    btnLast.PerformClick();
                }
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //if (rtxtParamQuery.Text != "")
            //{
            //    CurrentMode = Mode.View;
            //    ControlState(CurrentMode);
            //    paratable.Rows[0].CancelEdit();
            //    //paraIDManager.CancelCurrentEdit();
            //    //genquerypopup();
            //    //Paratypefill();
            //}
            //else
            //{
            //    CurrentMode = Mode.DefValue;
            //    ControlState(CurrentMode);
            //}
            //Filldata();

            //fillgrid();
            this.txtParamName.CausesValidation = false;
            this.txtParamCaption.CausesValidation = false;
            CurrentMode = Mode.View;
            ControlState(CurrentMode);
            btnLast.PerformClick();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btngenpopup_Click(object sender, EventArgs e)
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
        private void btnNew_Click(object sender, EventArgs e)
        {
            //txtParamName.Clear();
            //txtParamCaption.Clear();
            //chkTo.Checked = false;
            //rtxtParamQuery.Clear();
            //rtxtSearchFlds.Clear();
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

        private void cboparatype_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cboParamType.Text)
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
            txtparatype.Text = Convert.ToString(paratype);
        }


        //public void SaveDataGridItems()
        //{
        //    string para_caption = string.Empty;
        //    string para_search = string.Empty;
        //    string para_return = string.Empty;
        //    foreach (DataGridViewRow row in dgvPopupFields.Rows)
        //    {
        //        para_caption = Convert.ToString(row.Cells["Column1"].Value);
        //        para_search = Convert.ToString(row.Cells["Column2"].Value);
        //        para_return = Convert.ToString(row.Cells["Column3"].Value);
        //    }
        //    SqlConnection conn = new SqlConnection(this.SQLConnStr);
        //    string paraid = txtParamId.Text;
        //    string paraquery = rtxtParamQuery.Text;
        //    //For Insert into usqry
        //    string sqlstring = "update para_master set ParaCaption='" + para_caption + "',SearchFlds='" + para_search + "',parareturn='" + para_return + "'";
        //    SqlCommand cmd = new SqlCommand(sqlstring, conn);
        //    cmd.Connection = conn;
        //    //MessageBox.Show(cmd.CommandText);
        //    conn.Open();
        //    cmd.ExecuteNonQuery();
        //    conn.Close();
        //}


        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
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
                cboParamType.Enabled = false;
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
                NavButtonState(true, true, true, true);
            }
            else if (md == Mode.Edit)
            {
                txtParamName.Enabled = true;
                txtParamCaption.Enabled = true;
                cboParamType.Enabled = true;
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
                cboParamType.Enabled = false;
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
                cboParamType.Enabled = true;
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void fillgrid()
        {
            string selectSQL;
            string tmppart1, tmppart2, tmppart3, tmppart4;
            //selectSQL = "SELECT SearchFlds FROM para_master where  ParameterId=" + txtParamId.Text;
            selectSQL = "SELECT SearchFlds FROM para_master where  ParameterId=" + Convert.ToString(paratable.Rows[0]["ParameterId"]);
            SqlConnection con = new SqlConnection(this.SQLConnStr);
            SqlCommand cmd1 = new SqlCommand(selectSQL, con);
            SqlDataReader reader;
            con.Open();
            reader = cmd1.ExecuteReader();
            while (reader.Read())
            {
                tmppart1 = reader["SearchFlds"].ToString();

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
                    char[] delimiters1 = new char[] { ',', '}' };
                    string[] parts1 = tmppart2.Split(delimiters1, StringSplitOptions.RemoveEmptyEntries);
                    //string[] parts1 = tmppart2.Split(delimiters1);
                    for (int y = 0; y < parts1.Length; y++)
                    {
                        string part3 = parts1[y].Substring(parts1[y].LastIndexOf(':') + 1);

                        dgvPopupFields.Rows[y].Cells[1].Value = part3;
                    }
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
            cboParamType.Text = para_type;
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
                else if (this.cboParamType.Text == "Numeric" || this.cboParamType.Text == "DateTime")
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
                if (CurrentMode == Mode.Add)
                {
                    if (this.CheckDuplicate())
                        return;
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
                        if (this.cboParamType.Text == "VarChar")
                        {
                            paratable.Rows[0]["IsQuery"] = true;
                            cmd.ExecuteNonQuery();
                        }
                        if (this.cboParamType.Text == "Numeric" || this.cboParamType.Text == "DateTime")
                        {
                            tmpParam_filtcond = string.Empty;
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
                    cmd.CommandText = "update usqry set RepQry='" + paraquery + "' where qryid='" + Convert.ToString(paratable.Rows[0]["QueryId"]) + "'";
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();
                    try
                    {
                        cmd.Transaction = transaction;
                        cmd.Connection = conn;
                        if (this.cboParamType.Text == "VarChar")
                        {
                            cmd.ExecuteNonQuery();
                        }


                        if (tmpParam_filtcond == "{###}")
                        {
                            tmpParam_filtcond = rtxtSearchFlds.Text;
                        }
                        if (this.cboParamType.Text == "Numeric" || this.cboParamType.Text == "DateTime")
                        {
                            tmpParam_filtcond = string.Empty;
                        }
                        cmd.CommandText = "update para_master set SearchFlds='" + tmpParam_filtcond + "',ParamName='" + txtParamName.Text.Trim() + "',paracaption='" + txtParamName.Text.Trim() + "',paramtype=" + txtparatype.Text.Trim() + ",QueryId=" + Convert.ToString(paratable.Rows[0]["QueryId"]) + ",IsQuery=" + (Convert.ToBoolean(paratable.Rows[0]["IsQuery"]) ? "1" : "0") + " where ParameterId='" + paraid + "'";
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

                }
                //else
                //{
                //    timer1.Enabled = true;
                //    MessageBox.Show("Data updated Successfully", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
                CurrentMode = Mode.View;
                ControlState(CurrentMode);

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
            if (SearchFieldcount == 0 && this.cboParamType.Text == "VarChar")
            {
                MessageBox.Show("Please select atleast one search field.", AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (ReturnFieldCount == 0 && this.cboParamType.Text == "VarChar")
            {
                MessageBox.Show("Please select atleast one return field.", AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else if (ReturnFieldCount > 1 && this.cboParamType.Text == "VarChar")
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
            cboParamType.Text = "";
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


        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (btnNew.Enabled)
            {
                btnNew_Click(this.btnNew, e);
            }

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (btnSave.Enabled)
            {
                btnSave_Click(this.btnSave, e);

            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (btnEdit.Enabled)
            {
                btnEdit_Click(this.btnEdit, e);
            }
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (btnCancel.Enabled)
            {
                btnCancel_Click(this.btnCancel, e);
            }
        }

        private void deletToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (btnDelete.Enabled)
            {
                btnDelete_Click(this.btnDelete, e);
            }
        }
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (btnCopy.Enabled)
            {
                btnCopy_Click(this.btnCopy, e);
            }
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
        private void txtParamName_Validating(object sender, System.ComponentModel.CancelEventArgs e)
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
            if (this.CurrentMode == Mode.Add || this.CurrentMode == Mode.Edit)
            {
                if (this.txtParamName.Text.Trim().Length > 0)
                {
                    if (this.CheckDuplicate())
                        e.Cancel = true;
                }
            }
        }

        private void txtParamCaption_Validating(object sender, System.ComponentModel.CancelEventArgs e)
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
        }

        private void ParameterMast_FormClosing(object sender, FormClosingEventArgs e)
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

        private void ParameterMast_FormClosed(object sender, FormClosedEventArgs e)
        {
            DeleteProcessIdRecord();
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

        private void ParameterMast_KeyDown(object sender, KeyEventArgs e)
        {
            ////MessageBox.Show("s1");
            //if (e.Control)
            //{
            //    if (e.KeyCode == Keys.N)
            //    {
            //        if (this.btnEdit.Enabled)
            //            this.btnEdit.PerformClick();
            //    }
            //}
        }

        private void SetDataBinding()
        {
            txtParamId.DataBindings.Add("Text", paratable, "Parameterid");
            rtxtParamQuery.DataBindings.Add("Text", paratable, "RepQry");
            txtParamName.DataBindings.Add("Text", paratable, "paramName");
            txtParamCaption.DataBindings.Add("Text", paratable, "paraCaption");
            txtparatype.DataBindings.Add("Text", paratable, "ParamType");
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
    }
}

