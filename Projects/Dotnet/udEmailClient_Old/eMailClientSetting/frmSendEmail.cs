using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

using eMailClient.BLL;

namespace eMailClient
{
    public partial class frmSendEmail : Form
    {
        cls_Gen_Mgr_Email_Client m_obj_cls_Gen_Mgr_Email_Client = new cls_Gen_Mgr_Email_Client();
        public string vumess = "iTAX";
        public frmSendEmail()
        {
            InitializeComponent();
        }

        public void ShowEmailList(DataSet mDsEmailList)
        {
            foreach (DataRow m_dr in mDsEmailList.Tables[0].Rows)
            {
                ListViewItem lstviewitem = new ListViewItem();
                lstviewitem.Text = m_dr["Id"].ToString().Trim().ToUpper();
                lstviewitem.SubItems.Add(m_dr["Desc"].ToString().Trim().ToUpper());
                chklst_email.Items.Add(lstviewitem);
            }
        }
        private void frmSendEmail_Load(object sender, EventArgs e)
        {
            //m_obj_cls_Gen_Mgr_Email_Client.FillEmailDetails();
            //if (m_obj_cls_Gen_Mgr_Email_Client.DsSearch.Tables[0].Rows.Count > 0)
            //{
            //    foreach (DataRow m_dr in m_obj_cls_Gen_Mgr_Email_Client.DsSearch.Tables[0].Rows)
            //    {
            //        ListViewItem lstviewitem = new ListViewItem();
            //        lstviewitem.Text = m_dr["Id"].ToString().Trim().ToUpper();
            //        lstviewitem.SubItems.Add(m_dr["Desc"].ToString().Trim().ToUpper());
            //        chklst_email.Items.Add(lstviewitem);
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("No emails found to send!!", vumess, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void chk_selectall_CheckedChanged(object sender, EventArgs e)
        {
            bool isSelect;
            if (chk_selectall.Checked)
            {
                isSelect = true;
            }
            else
            {
                isSelect = false;
            }
            for (int i = 0; i < chklst_email.Items.Count; i++)
            {
                ListViewItem lstvwItem = new ListViewItem();
                lstvwItem = chklst_email.Items[i];
                if (lstvwItem.SubItems[1].Text != string.Empty)
                {
                    chklst_email.Items[i].Checked = isSelect;
                }
            }
        }

        private void frmSendEmail_KeyDown(object sender, KeyEventArgs e)
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
    }
}
