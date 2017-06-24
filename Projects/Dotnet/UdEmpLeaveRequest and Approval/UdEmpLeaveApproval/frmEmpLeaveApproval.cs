using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using ueconnect;
using GetInfo;

namespace UdEmpLeaveApproval
{
    public partial class frmEmpLeaveApproval : uBaseForm.FrmBaseForm
    {
        String cAppPId, cAppName;
        string startupPath = string.Empty;
        string ErrorMsg = string.Empty;
        string EmpCode = string.Empty,APPDATE=string.Empty;
        string LeaveBalance = string.Empty, LeaveOpening = string.Empty, LeaveAvail = string.Empty, LeaveNm = string.Empty;
        int rowindex, AppID;
        clsConnect oConnect;
        string ServiceType = string.Empty;
        bool vEnable = false;
        bool IsError = false;
        bool pSave = false;
        string ErrorStr = "";
        string sqlQuery;
        DataSet tDs = new DataSet();
        string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
        DataAccess_Net.clsDataAccess oDataAccess = new DataAccess_Net.clsDataAccess();

        public frmEmpLeaveApproval(string[] args)
        {
            InitializeComponent();
            this.pDisableCloseBtn = false;
            this.pPara = args;
            this.pFrmCaption = "Employee Leave Approval";
            this.pCompId = Convert.ToInt16(args[0]);
            this.pComDbnm = args[1];
            this.pServerName = args[2];
            this.pUserId = args[3];
            this.pPassword = args[4];
            this.pPApplRange = args[5];
            this.pAppUerName = args[6];
            Icon MainIcon = new System.Drawing.Icon(args[7].Replace("<*#*>", " "));
            this.pFrmIcon = MainIcon;
            this.pPApplText = args[8].Replace("<*#*>", " ");
            this.pPApplName = args[9];
            this.pPApplPID = Convert.ToInt16(args[10]);
            this.pPApplCode = args[11];

        }

        #region Form Load
        private void frmEmpLeaveApproval_Load(object sender, EventArgs e)
        {
            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.DateSeparator = "/";
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;

            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();
            string appPath = Application.ExecutablePath;
            appPath = Path.GetDirectoryName(appPath);
            if (string.IsNullOrEmpty(this.pAppPath))
            {
                this.pAppPath = appPath;
            }
            string fName = appPath + @"\bmp\pickup.gif";
            if (File.Exists(fName) == true)
            {
                this.btnAppBy.Image = Image.FromFile(fName);
                this.btnEmpNm.Image = Image.FromFile(fName);
                
            }

            this.mInsertProcessIdRecord();

            startupPath =Application.StartupPath;
            //startupPath = @"D:\VudyogPRO";          // To be Removed Sachin 
            oConnect = new clsConnect();
            GetInfo.iniFile ini = new GetInfo.iniFile(startupPath + "\\" + "Visudyog.ini");
            string appfile = ini.IniReadValue("Settings", "xfile").Substring(0, ini.IniReadValue("Settings", "xfile").Length - 4);
            oConnect.InitProc("'" + startupPath + "'", appfile);
            DirectoryInfo dir = new DirectoryInfo(startupPath);
            Array totalFile = dir.GetFiles();
            string registerMePath = string.Empty;
            for (int i = 0; i < totalFile.Length; i++)
            {
                string fname = Path.GetFileName(((System.IO.FileInfo[])(totalFile))[i].Name);
                if (Path.GetFileName(((System.IO.FileInfo[])(totalFile))[i].Name).ToUpper().Contains("REGISTER.ME"))
                {
                    registerMePath = Path.GetFileName(((System.IO.FileInfo[])(totalFile))[i].Name);
                    break;
                }
            }
            if (registerMePath == string.Empty)
            {
                ServiceType = "";
            }
            else
            {
                string[] objRegisterMe = (oConnect.ReadRegiValue(startupPath)).Split('^');
                ServiceType = objRegisterMe[15].ToString().Trim();
            }
            cmbStatus.Items.Add("Approved");
            cmbStatus.Items.Add("Rejected");
            this.set_grid_structure();
        }
        #endregion

        #region Set datagrid structure
        public void set_grid_structure()
        {
            dgvApproval.ColumnCount = 11;

            DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();
            chk.HeaderText = "Select";
            chk.Name = "chk";
            dgvApproval.Columns.Insert(0, chk);
            DataGridViewComboBoxColumn cmb = new DataGridViewComboBoxColumn();
            cmb.Items.Add("Approved");
            cmb.Items.Add("Rejected");
            cmb.HeaderText = "Status";
            cmb.Name = "cmb";
            dgvApproval.Columns.Insert(1, cmb);
            
            dgvApproval.Columns[2].HeaderText = "Approval Remarks";
            dgvApproval.Columns[2].DataPropertyName = "APPR_REMARKS";
            dgvApproval.Columns[2].Name = "APPR_REMARKS";
            dgvApproval.Columns[3].HeaderText = "Application ID";
            dgvApproval.Columns[3].DataPropertyName = "APPID";
            dgvApproval.Columns[3].Name = "APPID";
            dgvApproval.Columns[4].HeaderText = "DATE";
            dgvApproval.Columns[4].DataPropertyName = "DATE";
            dgvApproval.Columns[4].Name = "DATE";
            dgvApproval.Columns[5].HeaderText = "Employee Name";
            dgvApproval.Columns[5].DataPropertyName = "EMPNM";
            dgvApproval.Columns[5].Name = "EMPNM";
            dgvApproval.Columns[6].HeaderText = "Employee Code";
            dgvApproval.Columns[6].DataPropertyName = "EMPCODE";
            dgvApproval.Columns[6].Name = "EMPCODE";
            dgvApproval.Columns[7].HeaderText = "Leave Type";
            dgvApproval.Columns[7].DataPropertyName = "LEAVETYPE";
            dgvApproval.Columns[7].Name = "LEAVETYPE";
            dgvApproval.Columns[8].HeaderText = "Leave Name";
            dgvApproval.Columns[8].DataPropertyName = "LEAVENM";
            dgvApproval.Columns[8].Name = "LEAVENM";
            dgvApproval.Columns[9].HeaderText = "Requested Leave";
            dgvApproval.Columns[9].DataPropertyName = "REQ_LEAVE";
            dgvApproval.Columns[9].Name = "REQ_LEAVE";
            dgvApproval.Columns[10].HeaderText = "Balance Leave";
            dgvApproval.Columns[10].DataPropertyName = "BAL_LEAVE";
            dgvApproval.Columns[10].Name = "BAL_LEAVE";
            dgvApproval.Columns[11].HeaderText = "Start Date";
            dgvApproval.Columns[11].DataPropertyName = "SDATE";
            dgvApproval.Columns[11].Name = "SDATE";
            dgvApproval.Columns[12].HeaderText = "End Date";
            dgvApproval.Columns[12].DataPropertyName = "EDATE";
            dgvApproval.Columns[12].Name = "EDATE";

            dgvApproval.Columns[0].Width = 60;
            dgvApproval.Columns[2].Width = 80;
            dgvApproval.Columns[3].Width = 80;
            dgvApproval.Columns[5].Width = 80;
            dgvApproval.Columns[6].Width = 80;
            dgvApproval.Columns[10].Width = 80;
        }
        #endregion

        #region insert and delete ecord from ExtApplLog table
        private void mInsertProcessIdRecord()
        {

            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "UdEmpLeaveApproval.exe";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            sqlstr = "Set DateFormat dmy  insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";
            oDataAccess.ExecuteSQLStatement(sqlstr, null, 25, true);
        }

        private void mDeleteProcessIdRecord()
        {
            if (string.IsNullOrEmpty(this.pPApplName) || this.pPApplPID == 0 || string.IsNullOrEmpty(this.cAppName) || string.IsNullOrEmpty(this.cAppPId))
            {
                return;
            }
            DataSet dsData = new DataSet();
            string sqlstr;
            sqlstr = " Delete from vudyog..ExtApplLog where pApplNm='" + this.pPApplName + "' and pApplId=" + this.pPApplPID + " and cApplNm= '" + cAppName + "' and cApplId= " + cAppPId;
            oDataAccess.ExecuteSQLStatement(sqlstr, null, 25, true);
        }
        #endregion

        #region show record in grid
        private void btnShowRec_Click(object sender, EventArgs e)
        {
            int cnt;
            DialogResult res;
            res = DialogResult.Yes;
            cnt = 0;
            if (txtApprNm.Text == "")
            {
                MessageBox.Show("Please select Approvar Name...!!! ", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (pSave == false)
            {
                foreach (DataGridViewRow dr in dgvApproval.Rows)
                {
                    if (dr.Cells[1].EditedFormattedValue.ToString().Trim() != "")
                    {
                        res = MessageBox.Show("Do you want to discard the changes...!!! ", this.pPApplText, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        break;
                    }
                    if (dr.Cells[2].EditedFormattedValue.ToString().Trim() != "")
                    {
                        res = MessageBox.Show("Do you want to discard the changes...!!! ", this.pPApplText, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        break;
                    }
                    if (Convert.ToBoolean(dr.Cells[0].EditedFormattedValue.ToString()) == true)
                    {
                        res = MessageBox.Show("Do you want to discard the changes...!!! ", this.pPApplText, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        break;
                    }
                }

            }
            if (res == DialogResult.Yes)
            {
                sqlQuery = "";
                sqlQuery = "with empcte as ( ";
                sqlQuery += "select EmployeeCode,EmployeeName,supervisor from employeemast where supervisor='" + txtApprNm.Text.Trim() + "' union all ";
                sqlQuery += " select b.EmployeeCode,b.EmployeeName,b.supervisor from employeemast b";
                sqlQuery += " inner join empcte c on (c.EmployeeName=b.supervisor) )";
                sqlQuery += " select B.APPR_REMARKS,A.APPID,A.DATE,A.EMPNM,A.EMPCODE,B.LEAVETYPE,B.LEAVENM,B.REQ_LEAVE,B.BAL_LEAVE,B.SDATE,B.EDATE,B.LEAVENO";
                sqlQuery += " FROM Emp_Leave_Request_Head A";
                sqlQuery += " INNER JOIN Emp_Leave_Request_Item B ON (A.APPID=B.APPID)";
                sqlQuery += " WHERE B.STATUS='PENDING' AND A.EMPNM<>'" + txtApprNm.Text + "'";

                if (txtEmpNm.Text.Trim().Length != 0)
                {
                    sqlQuery += " AND A.EMPNM='" + txtEmpNm.Text + "'";
                }
                else
                {
                    sqlQuery += " AND A.EMPNM IN  (select EmployeeName from empcte)";
                }

                sqlQuery += " order by a.appid";
                tDs = oDataAccess.GetDataSet(sqlQuery, null, 25);
                dgvApproval.DataSource = tDs.Tables[0].DefaultView;

                if (tDs.Tables[0].Rows.Count != 0)
                {
                    dgvApproval.Columns["APPR_REMARKS"].Width = 200;
                    dgvApproval.Columns["APPID"].ReadOnly = true;
                    dgvApproval.Columns["DATE"].ReadOnly = true;
                    dgvApproval.Columns["DATE"].Width = 70;
                    dgvApproval.Columns["EMPNM"].ReadOnly = true;
                    dgvApproval.Columns["EMPNM"].Width = 200;
                    dgvApproval.Columns["EMPCODE"].ReadOnly = true;
                    dgvApproval.Columns["EMPCODE"].Width = 80;
                    dgvApproval.Columns["LEAVETYPE"].ReadOnly = true;
                    dgvApproval.Columns["LEAVETYPE"].Width = 50;
                    dgvApproval.Columns["LEAVENM"].ReadOnly = true;
                    dgvApproval.Columns["LEAVENM"].Width = 100;
                    dgvApproval.Columns["REQ_LEAVE"].ReadOnly = true;
                    dgvApproval.Columns["REQ_LEAVE"].Width = 50;
                    dgvApproval.Columns["BAL_LEAVE"].ReadOnly = true;
                    dgvApproval.Columns["BAL_LEAVE"].Width = 50;
                    dgvApproval.Columns["SDATE"].ReadOnly = true;
                    dgvApproval.Columns["SDATE"].Width = 70;
                    dgvApproval.Columns["EDATE"].ReadOnly = true;
                    dgvApproval.Columns["EDATE"].Width = 70;
                    dgvApproval.Columns["LEAVENO"].Visible = false;

                    chkSelectAll.Enabled = true;
                    btnUpdate.Enabled = true;
                    btnStatusUp.Enabled = true;
                    cmbStatus.Enabled = true;
                    cmbStatus.Text = "";
                    chkSelectAll.Checked = false;
                }
                else
                {
                    MessageBox.Show("No records found for approval...!!! ", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            pSave = false;
        }
        #endregion

        #region select Approval and employee popup 
        private void btnAppBy_Click_1(object sender, EventArgs e)
        {
            EmpCode = "";
            sqlQuery = "select EmployeeCode,EmployeeName from employeemast where employeename in (select supervisor from employeemast group by supervisor)";
            tDs = oDataAccess.GetDataSet(sqlQuery, null, 25);
            DataView dvw = tDs.Tables[0].DefaultView;

            VForText = "Select Employee";
            vSearchCol = "EmployeeName";
            vDisplayColumnList = "EmployeeCode:Employee Code,EmployeeName:Employee Name";
            vReturnCol = "EmployeeName,EmployeeCode";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtApprNm.Text = oSelectPop.pReturnArray[0];
                EmpCode = oSelectPop.pReturnArray[1];
            }
        }

        private void btnEmpNm_Click_1(object sender, EventArgs e)
        {
            EmpCode = "";
            sqlQuery = "with empcte as ( ";
            sqlQuery += "select EmployeeCode,EmployeeName,supervisor from employeemast where supervisor='"+txtApprNm.Text.Trim()+"' union all ";
            sqlQuery += " select b.EmployeeCode,b.EmployeeName,b.supervisor from employeemast b";
            sqlQuery += " inner join empcte c on (c.EmployeeName=b.supervisor) )";
            sqlQuery += " select SPACE(1) AS EmployeeCode,SPACE(1) AS EmployeeName union all";
            sqlQuery += " select EmployeeCode,EmployeeName from empcte";
            tDs = oDataAccess.GetDataSet(sqlQuery, null, 25);
            DataView dvw = tDs.Tables[0].DefaultView;

            VForText = "Select Employee";
            vSearchCol = "EmployeeName";
            vDisplayColumnList = "EmployeeCode:Employee Code,EmployeeName:Employee Name";
            vReturnCol = "EmployeeName,EmployeeCode";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtEmpNm.Text = oSelectPop.pReturnArray[0];
                EmpCode = oSelectPop.pReturnArray[1];
            }
        }
        #endregion

        #region save record
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int cnt=0,day1=0,month1=0,year1=0,lastday=0;
            DateTime date1;
            sqlQuery = "select ident_current('Emp_Leave_Request_Head') AS APPID,GETDATE() AS CURRDATE";
            tDs = oDataAccess.GetDataSet(sqlQuery, null, 25);
            APPDATE = tDs.Tables[0].Rows[0]["CURRDATE"].ToString().Substring(0,10);
            foreach (DataGridViewRow row in dgvApproval.Rows)
            {

                if (Convert.ToBoolean(row.Cells[0].Value) == true)
                {
                    if (this.update_validation(row.Index)==false)
                    {
                        return;
                    }
                    else
                    {
                        try
                        {
                            oDataAccess.BeginTransaction();
                            if (Convert.ToString(row.Cells["cmb"].EditedFormattedValue.ToString().Trim()) != "Rejected")
                            {
                                    sqlQuery = "execute usp_Ent_Emp_Update_Leave_Approv '" + row.Cells["LEAVETYPE"].Value.ToString().Trim() + "'," + row.Cells["REQ_LEAVE"].Value.ToString().Trim() + ",'" + row.Cells["EMPCODE"].Value.ToString().Trim() + "','" + row.Cells["REQ_LEAVE"].Value.ToString().Trim() + "','" + row.Cells["SDATE"].Value.ToString() + "','" + row.Cells["EDATE"].Value.ToString() + "'";
                                    oDataAccess.ExecuteSQLStatement(sqlQuery, null, 25, true);

                            }


                                sqlQuery = "";
                                sqlQuery = "update Emp_Leave_Request_ITEM ";
                                sqlQuery += " SET STATUS='" + row.Cells["cmb"].Value.ToString() + "',APPR_BY='" + txtApprNm.Text + "',APPR_DT='" + APPDATE + "',APPR_REMARKS='" + row.Cells[2].Value + "'";
                                sqlQuery += " WHERE APPID=" + row.Cells[3].Value + " AND LEAVETYPE='" + row.Cells[7].Value + "' AND LEAVENO=" + row.Cells["LEAVENO"].Value + "";
                                oDataAccess.ExecuteSQLStatement(sqlQuery, null, 25, true);
                                oDataAccess.CommitTransaction();
                                cnt = 1;
                            
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Records could not be update...!!! " + ex.Message, this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            oDataAccess.RollbackTransaction();
                            return;
                        }
                    }

                }
            }

            if (cnt == 1)
            {
                MessageBox.Show("Records updated successfully...!!! ", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                pSave = true;
                this.btnShowRec_Click(sender, e);
            }
            else
            {
                MessageBox.Show("Please select record to update..!!! ", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }
        #endregion

        private bool update_validation(int rindex)
        {
            bool error=true;

            if (Convert.ToString(dgvApproval.Rows[rindex].Cells["cmb"].Value)=="")
            {
                MessageBox.Show("Please select status!!! ", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvApproval.Rows[rindex].Cells["cmb"].Selected = true;
                error=false;
            }
            if (Convert.ToString(dgvApproval.Rows[rindex].Cells["cmb"].Value) == "Rejected" && dgvApproval.Rows[rindex].Cells["APPR_REMARKS"].Value.ToString()=="")
            {
                MessageBox.Show("Please enter the reason for rejection of Leave ", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvApproval.Rows[rindex].Cells["APPR_REMARKS"].Selected = true;
                error = false;
            }

            return error;
        }



        private void frmEmpLeaveApproval_FormClosed(object sender, FormClosedEventArgs e)
        {
            mDeleteProcessIdRecord();
        }

        #region bulk update (select all record and update all status)
        private void chkSelectAll_Click(object sender, EventArgs e)
        {

            if (chkSelectAll.Checked && dgvApproval.Rows.Count != 0)
            {
                foreach (DataGridViewRow row in dgvApproval.Rows)
                {
                    row.Cells["chk"].Value = true;
                }
            }
            else
            {
                foreach (DataGridViewRow row in dgvApproval.Rows)
                {
                    row.Cells["chk"].Value = false;
                }
            }


        }

        private void btnStatusUp_Click(object sender, EventArgs e)
        {
            if (cmbStatus.Text.ToString().Trim() != "" && dgvApproval.Rows.Count != 0)
            {

                foreach (DataGridViewRow row in dgvApproval.Rows)
                {
                    row.Cells["cmb"].Value = cmbStatus.Text.ToString().Trim();
                }
            }
            else
            {
                foreach (DataGridViewRow row in dgvApproval.Rows)
                {
                    row.Cells["cmb"].Value = "";
                }
            }

        }
        #endregion

    }
}
