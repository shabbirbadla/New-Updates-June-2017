using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace eMailClient
{
    public partial class frmTranType : Form
    {
        public int nPosition;
        private ListView lstViewRet;

        #region Properties
        public ListView LstViewRet
        {
            get { return lstViewRet; }
            set { lstViewRet = value; }
        }
        #endregion

        #region Other Methods
        public frmTranType(ListView chk_list)
        {
            InitializeComponent();
            foreach (ListViewItem item in chk_list.Items)
            {
                chklst_tran_typ.Items.Add(item.Clone() as ListViewItem);
            }

            bool isselect = false;
            for (int i = 0; i < chklst_tran_typ.Items.Count; i++)
            {
                if (!chklst_tran_typ.Items[i].Checked)
                {
                    isselect = true;
                    break;
                }
            }
            if (isselect == true)
            {
                chk_selectall.Checked = false;
            }
            else
            {
                chk_selectall.Checked = true;
            }
        }
        #endregion
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            nPosition = (-1);
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            LstViewRet = chklst_tran_typ;
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
            for (int i = 0; i < chklst_tran_typ.Items.Count; i++)
            {
                ListViewItem lstvwItem = new ListViewItem();
                lstvwItem = chklst_tran_typ.Items[i];
                if (lstvwItem.SubItems[1].Text != string.Empty)
                {
                    chklst_tran_typ.Items[i].Checked = isSelect;
                }
            }
        }

        private void frmTranType_KeyDown(object sender, KeyEventArgs e)
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
