using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using eMailClient.BLL;


namespace eMailClient
{
    public partial class frmExportCSV : Form
    {
        cls_Gen_Mgr_Email_Client m_obj_cls_Gen_Mgr_Email_Client;
        public string vumess = AppDetails.Apptitle;

        #region Properties
        private string separator;

        public string Separator
        {
            get { return separator; }
            set { separator = value; }
        }

        private string encoding;

        public string Encoding
        {
            get { return encoding; }
            set { encoding = value; }
        }

        private bool isFirstrow;

        public bool IsFirstrow
        {
            get { return isFirstrow; }
            set { isFirstrow = value; }
        }

        private string action;

        public string Action
        {
            get { return action; }
            set { action = value; }
        }
        #endregion

        #region Other Methods
        public void CallMethod(string value)
        {

            if (value == "AddMode")
            {
                ClearFields();
                BindData();
            }
            else
            {
                if (value == "EditMode")
                {
                    BindData();
                }
            }
        }

        private void ClearFields()
        {
            if (Separator == string.Empty)
            {
                rbtnSemicolon.Checked = false;
                rbtnTab.Checked = false;
                rbtnOther.Checked = false;
                txtOther.Clear();
            }
            if (Encoding == string.Empty)
            {
                rbtnUnicode.Checked = false;
                rbtnAscii.Checked = false;
                rbtnUtf7.Checked = false;
                rbtnUtf8.Checked = false;
                rbtnUtf32.Checked = false;
            }

            if (IsFirstrow == bool.Parse("false"))
                chkFirstrow.CheckState = CheckState.Unchecked;
        }

        public void BindData()
        {
            switch (Separator)
            {
                case ";":
                    rbtnSemicolon.Checked = true;
                    break;
                case "\\t":
                    rbtnTab.Checked = true;
                    break;
                default:
                    if (Separator != ";" && Separator != "\\t" && Separator != string.Empty && Separator != null)
                    {
                        rbtnOther.Checked = true;
                        txtOther.Text = Separator;
                    }
                    break;
            }

            switch (Encoding.ToUpper().ToString().Trim())
            {
                case "UNICODE":
                    rbtnUnicode.Checked = true;
                    break;
                case "ASCII":
                    rbtnAscii.Checked = true;
                    break;
                case "UTF7":
                    rbtnUtf7.Checked = true;
                    break;
                case "UTF8":
                    rbtnUtf8.Checked = true;
                    break;
                case "UTF32":
                    rbtnUtf32.Checked = true;
                    break;
            }
            if (IsFirstrow == bool.Parse("false"))
                chkFirstrow.CheckState = CheckState.Unchecked;
            else
                chkFirstrow.CheckState = CheckState.Checked;
        }

        private void Binding()
        {
            if (rbtnSemicolon.Checked)
                Separator = ";";
            if (rbtnTab.Checked)
                Separator = "\\t";
            if (rbtnOther.Checked)
                Separator = txtOther.Text.ToString().Trim();
            if (rbtnUnicode.Checked)
                Encoding = "UNICODE";
            if (rbtnAscii.Checked)
                Encoding = "ASCII";
            if (rbtnUtf7.Checked)
                Encoding = "UTF7";
            if (rbtnUtf8.Checked)
                Encoding = "UTF8";
            if (rbtnUtf32.Checked)
                Encoding = "UTF32";
            if (chkFirstrow.CheckState == CheckState.Checked)
                IsFirstrow = true;
            else
                IsFirstrow = false;
        }
        #endregion

        public frmExportCSV(Int32 CompanyID, String Connectionstring)//satish pal
        {
            InitializeComponent();
            m_obj_cls_Gen_Mgr_Email_Client = new cls_Gen_Mgr_Email_Client(CompanyID,Connectionstring);//satish pal
        }

        private void rbtnOther_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnOther.Checked)
            {
                txtOther.ReadOnly = false;
                txtOther.Focus();
            }
            else
            {
                txtOther.ReadOnly = true;
                txtOther.BackColor = Color.White;
                txtOther.Clear();
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Binding();
        }

        private void frmExportCSV_Load(object sender, EventArgs e)
        {
            this.Icon = new Icon(AppDetails.IcoPath);   // Added by Sachin N. S. on 22/01/2014 for Bug-20211
            rbtnSemicolon.Checked = false;
            rbtnTab.Checked = false;
            rbtnOther.Checked = false;
            rbtnUnicode.Checked = false;
            rbtnAscii.Checked = false;
            rbtnUtf7.Checked = false;
            rbtnUtf8.Checked = false;
            rbtnUtf32.Checked = false;
            CallMethod(Action);
        }

        private void frmExportCSV_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.O)
            {
                btnOk.PerformClick();
            }
            if (e.Control && e.KeyCode == Keys.Z)
            {
                btnCancel.PerformClick();
            }
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txtOther_TextChanged(object sender, EventArgs e)
        {
            if (txtOther.Text == ";")
            {
                MessageBox.Show("Please enter different separator other than 'semicolon' and 'tab'.", vumess, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtOther.Text = "";
                txtOther.Focus();
            }
        }
    }
}
