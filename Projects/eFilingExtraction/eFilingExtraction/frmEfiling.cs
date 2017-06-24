using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Globalization;
using System.Diagnostics;

namespace eFillingExtraction
{
    public partial class frmEfiling : Form
    {
        private string _Sta_dt = string.Empty;
        private string _End_dt = string.Empty;
        private string FinYear = string.Empty;
        private string _ConnStr = string.Empty;
        private string appName = string.Empty;
        private string compId = string.Empty;
        private ReturnType eFileType;
        private FormType formType;
        private string Quarter = string.Empty;
        private DataSet _MainSet;
        private string TDSAcknowledgeNo = string.Empty;
        private string TANNo = string.Empty;

        public static string pApplCode = string.Empty;
        public static string pApplNm = string.Empty;
        public static string pApplId = string.Empty;
        public static string pApplDesc = string.Empty;

        //variables of this application
        public static string cApplNm = string.Empty;
        public static string cApplId = string.Empty;
        public string ConnStr = string.Empty;

        public frmEfiling(string[] args)
        {

            InitializeComponent();
            //Execution from Software
            compId = args[0];
            _ConnStr = "Data Source=" + args[2] + ";Initial Catalog=" + args[1] + ";Uid=" + args[3] + ";Pwd=" + args[4];
            this.Icon = new Icon(args[7].Replace("<*#*>", " "));
            eFileType = (Convert.ToInt32(args[13]) == 2 ? ReturnType.eTCS : ReturnType.eTDS);
            this.GetFinYear();
            this.LoadForms();
            this.Text = (Convert.ToInt32(args[13]) == 2 ? "TCS" : "TDS") + " Extraction";
            pApplDesc = args[8].Replace("<*#*>", " ");
            pApplNm = args[9].Replace("<*#*>", " ");
            pApplId = args[10];
            pApplCode = args[11].Replace("<*#*>", " ");
            cApplNm = typeof(Program).Assembly.GetName().Name + ".exe";
            cApplId = Convert.ToString(Process.GetCurrentProcess().Id);
            appName = this.Text.Trim();

            //Execution from Project
            //_ConnStr = "Data Source=Prod_shrikant\\shree;Initial Catalog=A031314;Uid=sa;Pwd=sa1985";
            //eFileType = ReturnType.eTDS;
            //compId = "123";
            //this.GetFinYear();
            //LoadForms();
        }

        private void LoadForms()
        {
            switch (eFileType)
            {
                case ReturnType.eTDS:             //for TDS
                    this.cboFormNo.Items.Add(FormType.Form24Q);
                    this.cboFormNo.Items.Add(FormType.Form26Q);
                    this.cboFormNo.Items.Add(FormType.Form27Q);
                    break;
                case ReturnType.eTCS:             //for TCS
                    this.cboFormNo.Items.Add(FormType.Form27EQ);
                    break;
                default:
                    break;
            }
        }
        private void GetFinYear()
        {
            string SQL = "SELECT sta_dt,End_dt,tds_no FROM Vudyog..Co_mast Where CompId=" + compId;
            SqlConnection con = new SqlConnection(_ConnStr);
            SqlCommand cmd = new SqlCommand(SQL, con);
            SqlDataReader r = null;

            try
            {
                con.Open();
                r = cmd.ExecuteReader();

                while (r.Read())
                {
                    _Sta_dt = Convert.ToDateTime(r["Sta_dt"]).ToString("dd/MM/yyyy");
                    _End_dt = Convert.ToDateTime(r["End_dt"]).ToString("dd/MM/yyyy");
                    TANNo = Convert.ToString(r["tds_no"]);
                }
                FinYear = _Sta_dt.Substring(_Sta_dt.Length - 4, 4) + "-" + _End_dt.Substring(_End_dt.Length - 4, 4);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString(), appName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                if (r != null) r.Close();
                con.Close();
            }
        }
        private bool CheckValidation()
        {
            string RetVal = string.Empty;
            if (this.cboFormNo.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please select the form.");
                this.cboFormNo.Focus();
                return false;
            }
            if (this.cboQuarter.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please select the quarter.");
                this.cboQuarter.Focus();
                return false;
            }
            if (this.txtOutputPath.Text.Trim().Length == 0)
            {
                MessageBox.Show("Output path cannot be empty.");
                return false;
            }
            return true;
        }
        private void btnGenerateExcel_Click(object sender, EventArgs e)
        {
            string RetVal = string.Empty;
            Quarter = this.cboQuarter.Text;
            this.toolStripStatuslbl.Text = "Validating Values";
            this.Refresh();
            if (!this.CheckValidation())
            {
                this.toolStripStatuslbl.Text = "Ready";
                this.Refresh();
                return;
            }

            this.formType = (FormType)this.cboFormNo.SelectedItem;

            if (!System.IO.Directory.Exists(this.txtOutputPath.Text))
            {
                System.IO.Directory.CreateDirectory(this.txtOutputPath.Text);
            }
            DataTable dt;
            this.toolStripStatuslbl.Text = "Generating data...";
            dt = GetChallanDetailsFromDatabase();
            
            if (dt == null)
            {
                this.toolStripStatuslbl.Text = "Ready";
                this.Refresh();
                return;
            }
            int Version = CheckVersioning();
            switch (Version)
            {
                case 1:
                    #region Using Version 1
                    ReturnVer1 ver1 = new ReturnVer1(eFileType,formType,Quarter);
                    this.toolStripStatuslbl.Text = "Generating temporary details...";
                    this.Refresh();
                    ver1.GetTempDataTables();

                    this.toolStripStatuslbl.Text = "Generating Final Data...";
                    this.Refresh();
                    if (formType == FormType.Form24Q && Quarter=="Q4" )
                    {
                        this.toolStripStatuslbl.Text = "Generating Salary Data...";
                        this.Refresh();
                        DataTable dtSalary = this.GetSalaryDetails();
                        if (dtSalary == null)
                        {
                            return;
                        }
                        RetVal = ver1.GetFinalData(dt, dtSalary);
                    }
                    else
                    {
                        RetVal = ver1.GetFinalData(dt);
                    }
                    if (RetVal != "")
                    {
                        MessageBox.Show(RetVal, appName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        ver1 = null;
                        return;
                    }
                    _MainSet = ver1.MainSet;
                    //if (formType == FormType.Form24Q)//Commented By Kishor A. for Bug-26391 on 06/07/2015
                    //{//Commented By Kishor A. for Bug-26391 on 06/07/2015

                        this.toolStripStatuslbl.Text = "Generating Company details...";
                        this.GetCompanyDetails();
                        if (RetVal != "")
                        {
                            MessageBox.Show(RetVal, appName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;
                        }
                        //}//Commented By Kishor A. for Bug-26391 on 06/07/2015

                    ver1 = null;
                    #endregion

                    if (_MainSet.Tables[0].Rows.Count <= 0)
                    {
                        this.toolStripStatuslbl.Text = "Ready";
                        this.Refresh();
                        MessageBox.Show("No challans found to generate the file.", appName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    break;

                case 2:
                    #region Using Version 2
                    RetVal = this.CheckPreviousAcknowledgeNo();
                    if (RetVal != "")
                    {
                        MessageBox.Show(RetVal, appName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    if (TDSAcknowledgeNo.Trim().Length == 0)
                    {
                        DialogResult ans = MessageBox.Show("Have you filed return before this ?", appName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (ans == DialogResult.Yes)
                        {
                            frmTdsacknowledgeNo frm = new frmTdsacknowledgeNo();
                            ans = frm.ShowDialog();
                            if (ans == DialogResult.OK)
                            {
                                TDSAcknowledgeNo = frm.AcknowledgeNo;
                            }
                            frm = null;
                        }
                    }
                    ReturnVer2 ver2 = new ReturnVer2(eFileType, formType,Quarter);
                    this.toolStripStatuslbl.Text = "Generating temporary details...";
                    this.Refresh();
                    ver2.GetTempDataTables();

                    this.toolStripStatuslbl.Text = "Generating Final Data...";
                    this.Refresh();
                    if (formType == FormType.Form24Q && Quarter == "Q4")
                    {
                        this.toolStripStatuslbl.Text = "Generating Salary Data...";
                        this.Refresh();
                        DataTable dtSalary = this.GetSalaryDetails();
                        if (dtSalary == null)
                        {
                            return;
                        }
                        RetVal = ver2.GetFinalData(dt, dtSalary);
                    }
                    else
                    {
                        RetVal = ver2.GetFinalData(dt);
                    }
                    if (RetVal != "")
                    {
                        MessageBox.Show(RetVal, appName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        ver1 = null;
                        return;
                    }
                    _MainSet = ver2.MainSet;
                    this.toolStripStatuslbl.Text = "Generating Company details...";
                    this.GetCompanyDetails();
                    if (RetVal != "")
                    {
                        MessageBox.Show(RetVal, appName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    ver2 = null;
                    #endregion

                    if (_MainSet.Tables[1].Rows.Count <= 0)
                    {
                        this.toolStripStatuslbl.Text = "Ready";
                        this.Refresh();
                        MessageBox.Show("No challans found to generate the file.", appName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    break;
                default:
                    break;
            }
            string cFileName = string.Empty;
            if (Version == 1)
            {
                cFileName = System.IO.Path.Combine(this.txtOutputPath.Text, "tds" + this.cboFormNo.Text.Trim() + this.cboQuarter.Text.Trim() + ".xls");
                this.toolStripStatuslbl.Text = "Writing excel file...";
            }
            else
            {
                cFileName = System.IO.Path.Combine(this.txtOutputPath.Text, "tds" + this.cboFormNo.Text.Trim() + this.cboQuarter.Text.Trim() + ".xml");
                this.toolStripStatuslbl.Text = "Generating xml file...";
            }

            if (System.IO.File.Exists(cFileName))
            {
                try
                {
                    System.IO.File.Delete(cFileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, appName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }
            if (Version == 1)
            {
                string resourceName = Path.GetFileName(cFileName);
                string xmlResource = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ".tds" + this.cboFormNo.Text.Trim() + ".xls";
                try
                {
                    using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(xmlResource))
                    {
                        using (System.IO.FileStream fileStream = new System.IO.FileStream(cFileName, System.IO.FileMode.Create))
                        {
                            for (int i = 0; i < stream.Length; i++)
                            {
                                fileStream.WriteByte((byte)stream.ReadByte());
                            }
                            fileStream.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, appName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                if (eFileType == ReturnType.eTDS)
                {
                    WriteTDSExcel oExcel = new WriteTDSExcel(cFileName);
                    RetVal = oExcel.WriteToFile(_MainSet);
                }
                if (eFileType == ReturnType.eTCS)
                {
                    WriteTCSExcel oExcel = new WriteTCSExcel(cFileName);
                    RetVal = oExcel.WriteToFile(_MainSet);
                }
                this.toolStripStatuslbl.Text = "file generated...";
                if (RetVal.Trim().Length == 0)
                {
                    MessageBox.Show(cFileName + " file generated successfully.", appName, MessageBoxButtons.OK, MessageBoxIcon.None);
                    this.toolStripStatuslbl.Text = "Ready";
                }
                else
                {
                    MessageBox.Show(RetVal, appName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            else
            {
                try
                {

                    WriteXML.WriteXmlToFile(_MainSet, cFileName);
                    MessageBox.Show(cFileName + " file generated successfully.", appName, MessageBoxButtons.OK, MessageBoxIcon.None);
                    this.toolStripStatuslbl.Text = "Ready";
                    this.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, appName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }
            _MainSet = null;
        }

        private int CheckVersioning()
        {
            DateTime StartDt = Convert.ToDateTime(this.txtFrmDate.Text, new CultureInfo("en-GB"));
            DateTime EndDt = Convert.ToDateTime(this.txtToDate.Text, new CultureInfo("en-GB"));

            if (StartDt >= Convert.ToDateTime("01/04/2013", new CultureInfo("en-GB")) && formType != FormType.Form24Q) //Added By Kishor A. for Bug-26391
            //  if (StartDt >= Convert.ToDateTime("01/04/2013", new CultureInfo("en-GB")) && EndDt <= Convert.ToDateTime("30/09/2013", new CultureInfo("en-GB")) && formType!=FormType.Form24Q) //Commented By Kishor A. for Bug-26391
            {
                return 1;
            }
            else if (formType == FormType.Form24Q) //Added By Kishor A. for Bug-26391
            //else if (StartDt >= Convert.ToDateTime("01/10/2013", new CultureInfo("en-GB")) || formType == FormType.Form24Q) //Commented By Kishor A. for Bug-26391
            {
                return 2;
            }
            return 0;
        }
        private string CheckPreviousAcknowledgeNo()
        {
            try
            {
                string query = " Select Top 1 Acknow_no From TdsAcknow Where Acknow_no<>'' Order By L_yn desc,[Quarter] desc  ";
                SqlConnection cn = new SqlConnection(_ConnStr);
                cn.Open();
                SqlCommand cmd = new SqlCommand(query, cn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    TDSAcknowledgeNo = Convert.ToString(dr["Acknow_no"]);
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return string.Empty;
        }
        private string GetCompanyDetails()
        {
            if (_MainSet.Tables["General"] != null)
            {
                _MainSet.Tables["General"].Rows.Clear();
            }
            try
            {
                DataRow Row = _MainSet.Tables["General"].NewRow();
                Row["TAN"] = TANNo.Trim();
                Row["IsReturnFiled"] = (TDSAcknowledgeNo.Trim().Length > 0 ? "Y" : "N");
                Row["TDSAcknowNo"] = TDSAcknowledgeNo.Trim();
                Row["AddressChange"] = (this.chkAddChange.Checked ? "Y" : "N");
                Row["RAddressChange"] = (this.chkRAddChange.Checked ? "Y" : "N");
                _MainSet.Tables["General"].Rows.Add(Row);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return string.Empty;
        }
        private DataTable GetChallanDetailsFromDatabase()
        {
            string query = string.Empty;
            string[] d1 = this.txtFrmDate.Text.Trim().Split('/');
            string[] d2 = this.txtToDate.Text.Trim().Split('/');
            string FromDate = d1[1] + "/" + d1[0] + "/" + d1[2];
            string ToDate = d2[1] + "/" + d2[0] + "/" + d2[2];
            DataTable ldt = new DataTable("Data");

            switch (this.cboFormNo.Text.Trim())
            {
                case "Form24Q":
                    query = "EXECUTE USP_REP_EMP_FORM24Q_1stJul2013 '','','','" + FromDate + "','" + ToDate + "','','','','',0,0,'','','','','','','','','" + FinYear + "',''";
                    break;
                case "Form26Q":
                    query = "EXECUTE USP_REP_FORM26Q_1STJUL2013'','','','" + FromDate + "','" + ToDate + "','','','','',0,0,'','','','','','','','','" + FinYear + "',''";
                    break;
                case "Form27Q":
                    query = "EXECUTE USP_REP_FORM27Q_1STJUL2013'','','','" + FromDate + "','" + ToDate + "','','','','',0,0,'','','','','','','','','" + FinYear + "',''";
                    break;
                case "Form27EQ":
                    query = "EXECUTE USP_REP_FORM27EQ_1STJUL2013'','','','" + FromDate + "','" + ToDate + "','','','','',0,0,'','','','','','','','','" + FinYear + "',''";
                    break;
                default:
                    break;
            }
            try
            {
                SqlConnection cn = new SqlConnection(_ConnStr);
                SqlDataAdapter da = new SqlDataAdapter(query, cn);
                da.SelectCommand.CommandTimeout = 0;
                da.Fill(ldt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            return ldt;

        }
        private DataTable GetSalaryDetails()
        {
            DataTable ldt = new DataTable("Salary");
            string query = "execute USP_REP_FORM24Q_SAL_DET '" + Convert.ToDateTime(this._Sta_dt, new CultureInfo("en-GB")).ToString("MM/dd/yyyy") + "','" + Convert.ToDateTime(this._End_dt, new CultureInfo("en-GB")).ToString("MM/dd/yyyy") + "'";
            try
            {
                SqlConnection cn = new SqlConnection(_ConnStr);
                SqlDataAdapter da = new SqlDataAdapter(query, cn);
                da.SelectCommand.CommandTimeout = 0;
                da.Fill(ldt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            return ldt;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DeleteProcessIdRecord();
            Application.Exit();
        }

        private void cboQuarter_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.cboQuarter.SelectedItem.ToString())
            {
                case "Q1":
                    this.txtFrmDate.Text = "01/04/" + _Sta_dt.Substring(_Sta_dt.Length - 4, 4);
                    this.txtToDate.Text = "30/06/" + _Sta_dt.Substring(_Sta_dt.Length - 4, 4);
                    break;
                case "Q2":
                    this.txtFrmDate.Text = "01/07/" + _Sta_dt.Substring(_Sta_dt.Length - 4, 4);
                    this.txtToDate.Text = "30/09/" + _Sta_dt.Substring(_Sta_dt.Length - 4, 4);
                    break;
                case "Q3":
                    this.txtFrmDate.Text = "01/10/" + _Sta_dt.Substring(_Sta_dt.Length - 4, 4);
                    this.txtToDate.Text = "31/12/" + _Sta_dt.Substring(_Sta_dt.Length - 4, 4);
                    break;
                case "Q4":
                    this.txtFrmDate.Text = "01/01/" + _End_dt.Substring(_End_dt.Length - 4, 4);
                    this.txtToDate.Text = "31/03/" + _End_dt.Substring(_End_dt.Length - 4, 4);
                    break;
                default:
                    break;
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
            folderBrowserDialog.Description = "Select Output path to save excel file ";
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                this.txtOutputPath.Text = folderBrowserDialog.SelectedPath.ToString();
            }
        }

        private void frmEfiling_Load(object sender, EventArgs e)
        {
            this.InsertProcessIdRecord();
        }
        public void InsertProcessIdRecord()
        {
            string sqlstr = string.Empty;
            sqlstr = " insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values(" +
                        "@pApplCode,@CallDate,@pApplNm,@pApplId,@pApplDesc,@cApplNm,@cApplId,@cApplDesc)";

            SqlConnection con = new SqlConnection(_ConnStr);
            SqlCommand cmd = new SqlCommand(sqlstr, con);
            cmd.Parameters.Add(new SqlParameter("@pApplCode", pApplCode));
            cmd.Parameters.Add(new SqlParameter("@CallDate", DateTime.Now.ToString("MM/dd/yyyy")));
            cmd.Parameters.Add(new SqlParameter("@pApplNm", pApplNm));
            cmd.Parameters.Add(new SqlParameter("@pApplId", pApplId));
            cmd.Parameters.Add(new SqlParameter("@pApplDesc", pApplDesc));
            cmd.Parameters.Add(new SqlParameter("@cApplNm", cApplNm));
            cmd.Parameters.Add(new SqlParameter("@cApplId", cApplId));
            cmd.Parameters.Add(new SqlParameter("@cApplDesc", appName));
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, appName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void DeleteProcessIdRecord()
        {
            string sqlstr = string.Empty;
            sqlstr = " Delete from vudyog..ExtApplLog where pApplNm=@pApplNm and pApplId=@pApplId and cApplNm= @cApplNm and cApplId= @cApplId";
            SqlConnection con = new SqlConnection(_ConnStr);
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
                MessageBox.Show(ex.Message, appName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
