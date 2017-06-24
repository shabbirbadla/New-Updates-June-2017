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
    public partial class frmEmailList : Form
    {
        cls_Gen_Mgr_Email_Client m_obj_cls_Gen_Mgr_Email_Client;
        public int nPosition;
        public string cEmailList;

        #region Other Methods
        public void DataFill(string custnm)
        {
            lstvw_EmailList.Items.Clear();
            m_obj_cls_Gen_Mgr_Email_Client.EmailIdList(custnm);
            foreach (DataRow m_dr in m_obj_cls_Gen_Mgr_Email_Client.DsSearch.Tables[0].Rows)
            {
                ListViewItem lstviewitem = new ListViewItem();
                string[] strLstEmail = Convert.ToString(m_dr["Email"]).Trim().Split(',');
                foreach (string strLst in strLstEmail)
                {
                    lstviewitem = new ListViewItem();
                    lstviewitem.Text = m_dr["Mailname"].ToString().Trim().ToUpper();
                    lstviewitem.SubItems.Add(strLst.Trim().ToLower());
                    lstvw_EmailList.Items.Add(lstviewitem);
                }
            }
        }

        private void SearchRecords()
        {
            string cFilterVal = txtSearch.Text.ToString().Trim();
            if (cFilterVal != "")
            {
                DataFill(cFilterVal);
            }
            else
            {
                lstvw_EmailList.Items.Clear();
            }
            lstvw_EmailList.Refresh();
        }

        private string TranTypeSelectedList()
        {
            string trantypelist = "";
            for (int i = 0; i < lstvw_EmailList.Items.Count; i++)
            {
                if (lstvw_EmailList.Items[i].Checked)
                {
                    ListViewItem lstvwItem = new ListViewItem();
                    lstvwItem = lstvw_EmailList.Items[i];
                    trantypelist = (trantypelist != string.Empty ? trantypelist + "," : trantypelist) + lstvwItem.SubItems[1].Text.ToString();
                }
            }
            return trantypelist;
        }
        #endregion

        public frmEmailList(Int32 CompanyID, String Connectionstring)
        {
            InitializeComponent();
            m_obj_cls_Gen_Mgr_Email_Client = new cls_Gen_Mgr_Email_Client(CompanyID,Connectionstring);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            nPosition = (-1);
            Close();
        }
        
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            chk_selectall.Checked = false;
            SearchRecords();
            if (lstvw_EmailList.Items.Count > 0)
            {
                chk_selectall.Enabled = true;
            }
            else
            {
                chk_selectall.Enabled = false;
            }
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
            for (int i = 0; i < lstvw_EmailList.Items.Count; i++)
            {
                ListViewItem lstvwItem = new ListViewItem();
                lstvwItem = lstvw_EmailList.Items[i];
                if (lstvwItem.SubItems[1].Text != string.Empty)
                {
                    lstvw_EmailList.Items[i].Checked = isSelect;
                }
            }
        }

        private void frmEmailList_KeyDown(object sender, KeyEventArgs e)
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

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (lstvw_EmailList.Items.Count > 0)
            {
                cEmailList = TranTypeSelectedList();
            }
        }

        private void frmEmailList_Load(object sender, EventArgs e)
        {

        }
    }
}
