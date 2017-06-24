using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using eMailClient.DAL;
using eMailClient.BLL;

namespace eMailClient
{
    public partial class frmEmailSettings : Form
    {
        cls_Gen_Mgr_Email_Settings m_obj_cls_Gen_Mgr_Email_Settings;
        public string vumess = AppDetails.Apptitle;
        string cAppPId, cAppName, pPApplText;
        string pPApplCode, pPApplName;
        int pPApplPID;

        private Int32 companyID;
        public Int32 CompanyID
        {
            get { return companyID; }
            set { companyID = value; }
        }
        // <added by satish pal>
        private string connectionstring;
        public string Connectionstring
        {
            get { return connectionstring; }
            set { connectionstring = value; }
        }
       /// </added by satish pal>
        #region Enumeration Members
        public enum eMailClient_RowName : int
        {
            yourname = 0,
            username,
            password,
            host,
            port,
            enablessl
        }
        #endregion

        #region Other Methods
        private void BindData()
        {
            txt_yourname.Text = m_obj_cls_Gen_Mgr_Email_Settings.Yourname;
            txt_username.Text = m_obj_cls_Gen_Mgr_Email_Settings.Username;
            txt_password.Text = m_obj_cls_Gen_Mgr_Email_Settings.Password == null ? "" : m_obj_cls_Gen_Mgr_Email_Settings.DecryptData(m_obj_cls_Gen_Mgr_Email_Settings.Password);
            txt_host.Text = m_obj_cls_Gen_Mgr_Email_Settings.Host;
            txt_port.Text = m_obj_cls_Gen_Mgr_Email_Settings.Port.ToString();
            chk_enablessl.Checked = m_obj_cls_Gen_Mgr_Email_Settings.Enablessl;
        }

        private void Binding()
        {
            m_obj_cls_Gen_Mgr_Email_Settings.Yourname = txt_yourname.Text.ToString().Trim();
            m_obj_cls_Gen_Mgr_Email_Settings.Username = txt_username.Text.ToString().Trim();
            m_obj_cls_Gen_Mgr_Email_Settings.Password = txt_password.Text.ToString().Trim();
            m_obj_cls_Gen_Mgr_Email_Settings.Host = Convert.ToString(txt_host.Text.ToString().Trim());
            m_obj_cls_Gen_Mgr_Email_Settings.Port = Convert.ToInt32(txt_port.Text.ToString().Trim());
            m_obj_cls_Gen_Mgr_Email_Settings.Enablessl = chk_enablessl.Checked;
        }

        private bool EmailValidation(TextBox field)
        {
            bool isvalid = true;
            string value = field.Text.ToString().Trim();
            if (value != string.Empty)
            {
                var result = value.Split(',');
                for (int i = 0; i < result.Length; i++)
                {
                    if (i == 0)
                    {
                        if (result[i] != string.Empty)
                        {
                            string emailpattern = @"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z][a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$";
                            Match emailmatch = Regex.Match(result[i].Trim().ToString(), emailpattern, RegexOptions.IgnoreCase);
                            if (!emailmatch.Success)
                            {
                                errorProvider1.SetError(txt_username, "EmailId is invalid");
                                isvalid = false;
                            }
                            else
                            {
                                errorProvider1.Clear();
                            }
                        }
                    }
                    else
                    {
                        errorProvider1.SetError(txt_username, "Enter only one EmailId");
                        isvalid = false;
                        break;
                    }
                }
            }
            return isvalid;
        }

        public bool validation()
        {
            bool isvalid = true;

            if (txt_yourname.Text == string.Empty)
            {
                errorProvider1.SetError(txt_yourname, lbl_yourname.Text.ToString() + " cannot be blank");
                isvalid = false;
            }

            if (txt_username.Text == string.Empty)
            {
                errorProvider1.SetError(txt_username, lbl_username.Text.ToString() + " cannot be blank");
                isvalid = false;
            }
            else if (EmailValidation(txt_username) == false && txt_username.Text != string.Empty)
            {
                isvalid = false;
            }

            if (txt_password.Text == string.Empty)
            {
                errorProvider1.SetError(txt_password, lbl_password.Text.ToString() + " cannot be blank");
                isvalid = false;
            }

            if (txt_host.Text == "0".ToString())
            {
                errorProvider1.SetError(txt_host, lbl_host.Text.ToString() + " cannot be blank");
                isvalid = false;
            }

            if (txt_port.Text == "0".ToString())
            {
                errorProvider1.SetError(txt_port, lbl_port.Text.ToString() + " cannot be blank");
                isvalid = false;
            }
            return isvalid;
        }
        #endregion

        public string _menuType = string.Empty;
        public frmEmailSettings(Int32 CompanyID, String Connectionstring, string _pPApplText, string _pPApplName, string _pPApplPID, string _pPApplCode)
        {
            InitializeComponent();
            this.pPApplCode = _pPApplCode;
            this.pPApplName = _pPApplName;
            this.pPApplPID = Convert.ToInt16(_pPApplPID);
            this.pPApplText = _pPApplText;
            this.Connectionstring = Connectionstring;
            m_obj_cls_Gen_Mgr_Email_Settings = new cls_Gen_Mgr_Email_Settings (CompanyID,Connectionstring);//--hi
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (validation() == false)
                    return;
                errorProvider1.Clear();
                Binding();
                m_obj_cls_Gen_Mgr_Email_Settings.RecordCount();
                if (m_obj_cls_Gen_Mgr_Email_Settings.DsSearch.Tables[0].Rows.Count == 0)
                {
                    m_obj_cls_Gen_Mgr_Email_Settings.Insert();
                }
                else
                {
                    m_obj_cls_Gen_Mgr_Email_Settings.Update();
                }
                MessageBox.Show("Entry Saved", vumess, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                Close();
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + " Entry cannot be Saved", vumess, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }
        }

        private void frmEmailSettings_Load(object sender, EventArgs e)
        {
            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;

            this.Icon = new Icon(AppDetails.IcoPath);   // Added by Sachin N. S. on 22/01/2014 for Bug-20211
            //MessageBox.Show(connectionstring);  
            m_obj_cls_Gen_Mgr_Email_Settings.Select();
            BindData();
            groupBox1.ForeColor = Color.Tomato;
            groupBox2.ForeColor = Color.Tomato;
            foreach (Control ctl in groupBox1.Controls)
            {
                ctl.ForeColor = SystemColors.ControlText;
            }
            foreach (Control ctl in groupBox2.Controls)
            {
                ctl.ForeColor = SystemColors.ControlText;
            }

            this.mInsertProcessIdRecord();      // Added by Sachin N. S. on 31/01/2014 for Bug-20211
        }

        private void frmEmailSettings_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.O)
            {
                btnOk.PerformClick();
            }
            if (e.Control && e.KeyCode == Keys.Z)
            {
                btnCancel.PerformClick();
            }
        }

        private void mInsertProcessIdRecord()
        {
            int m_execute = 0;
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "eMailClient.exe";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            sqlstr = "Set DateFormat dmy insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";

            m_execute = cls_Sqlhelper.ExecuteNonQuery(Connectionstring, CommandType.Text, sqlstr);
        }

        private void mDeleteProcessIdRecord()/*Added Rup 07/03/2011*/
        {
            int m_execute = 0;
            if (string.IsNullOrEmpty(this.pPApplName) || this.pPApplPID == 0 || string.IsNullOrEmpty(this.cAppName) || string.IsNullOrEmpty(this.cAppPId))
            {
                return;
            }
            string sqlstr;
            sqlstr = " Delete from vudyog..ExtApplLog where pApplNm='" + this.pPApplName + "' and pApplId=" + this.pPApplPID + " and cApplNm= '" + cAppName + "' and cApplId= " + cAppPId;
            m_execute = cls_Sqlhelper.ExecuteNonQuery(Connectionstring, CommandType.Text, sqlstr);
        }

        private void frmEmailSettings_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.mDeleteProcessIdRecord();
        }
    }
}
