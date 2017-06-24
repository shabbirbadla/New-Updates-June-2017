using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using eMailClient.BLL;

namespace eMailClient
{
    public partial class frmPendingMail : Form
    {
        #region define variables
        cls_Gen_Mgr_PendingMail m_Obj_PendingMail;
        public string vumess = "iTAX";

        private Int32 companyID;

        public Int32 CompanyID
        {
            get { return companyID; }
            set { companyID = value; }
        }

        private string id;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        private List<string> emailId;

        public List<string> EmailId
        {
            get { return emailId; }
            set { emailId = value; }
        }

        private DataSet obj_DsSelect;

        public DataSet Obj_DsSelect
        {
            get { return obj_DsSelect; }
            set { obj_DsSelect = value; }
        }

        
        #endregion

        public frmPendingMail(Int32 CompanyID)
        {
            InitializeComponent();
            m_Obj_PendingMail = new cls_Gen_Mgr_PendingMail(CompanyID);
        }

        private void frmPendingMail_Load(object sender, EventArgs e)
        {
            dgvPendingMail.DataSource = Obj_DsSelect.Tables[0];
            for (int i = 0; i < dgvPendingMail.Rows.Count; i++)
            {
                Image img;
                string colValue = dgvPendingMail.Rows[i].Cells["Filename"].Value.ToString();
                string str = colValue.Substring(colValue.IndexOf('.') + 1);
                switch (str.ToUpper().ToString().Trim())
                {
                    case "XLS":
                        img = Properties.Resources.excel;
                        dgvPendingMail["colFilename", i].Value = img;
                        break;
                    case "CSV":
                        img = Properties.Resources.csv_text;
                        dgvPendingMail["colFilename", i].Value = img;
                        break;
                    case "PDF":
                        img = Properties.Resources.pdf1;
                        dgvPendingMail["colFilename", i].Value = img;
                        break;
                    case "DOC":
                        img = Properties.Resources.doc_file;
                        dgvPendingMail["colFilename", i].Value = img;
                        break;
                }
                dgvPendingMail.Rows[i].Cells["colFilename"].ToolTipText = dgvPendingMail.Rows[i].Cells["Filename"].Value.ToString();
            }
            dgvPendingMail.Columns["AutoId"].Visible = false;
            dgvPendingMail.Columns["Filename"].Visible = false;
            dgvPendingMail.Columns["Filepath"].Visible = false;
            dgvPendingMail.Columns["colFilename"].DisplayIndex = dgvPendingMail.Columns["Subject"].DisplayIndex + 1;
        }

        private void chk_selectall_CheckedChanged(object sender, EventArgs e)
        {
            bool isSelect = false;
            if (chk_selectall.Checked)
            {
                isSelect = true;
            }
            else
            {
                isSelect = false;
            }
            for (int i = 0; i < dgvPendingMail.Rows.Count; i++)
            {
                dgvPendingMail.Rows[i].Cells["colSelect"].Value = isSelect;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            EmailId = new List<string>();
            for (int i = 0; i < dgvPendingMail.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dgvPendingMail.Rows[i].Cells["colSelect"].Value) == true)
                {
                    EmailId.Add(dgvPendingMail.Rows[i].Cells["AutoId"].Value.ToString());
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dgvPendingMail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvPendingMail.Columns["colFilename"].Index)
            {
                string filename = dgvPendingMail["Filepath", e.RowIndex].Value.ToString() + dgvPendingMail["Filename", e.RowIndex].Value.ToString();
                Process.Start(filename.ToString().Trim());
            }
        }

        private void frmPendingMail_KeyDown(object sender, KeyEventArgs e)
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
    }
}
