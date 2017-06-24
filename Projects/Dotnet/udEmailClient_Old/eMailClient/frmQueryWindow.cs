using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using eMailClient.BLL;


namespace eMailClient
{
    public partial class frmQueryWindow : Form
    {
        cls_Gen_Mgr_Email_Client m_obj_cls_Gen_Mgr_Email_Client;
        bool lv1_mdown = false;
        bool lv2_mdown = false;
        public string vumess = "iTAX";

        #region Properties
        private string query;
        public string Query
        {
            get { return query; }
            set { query = value; }
        }

        private string reportquery;

        public string Reportquery
        {
            get { return reportquery; }
            set { reportquery = value; }
        }

        private string parameters;

        public string Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }

        private string reportquerytype;

        public string Reportquerytype
        {
            get { return reportquerytype; }
            set { reportquerytype = value; }
        }

        private string action;

        public string Action
        {
            get { return action; }
            set { action = value; }
        }

        private string entry_ty;

        public string Entry_ty
        {
            get { return entry_ty; }
            set { entry_ty = value; }
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
            if (Query == string.Empty)
                txt_query.Clear();
            if (Reportquery == string.Empty)
                txt_reportquery.Clear();
            if (Parameters == string.Empty)
                lstvwParameters.Items.Clear();
            if (Reportquerytype == string.Empty)
            {
                rbtnQuery.Checked = false;
                rbtnSP.Checked = false;
            }
        }

        public void ShowQuery(string entry_ty)
        {
            if (txt_query.Text == string.Empty && Entry_ty != string.Empty && Entry_ty != null)
            {
                txt_query.Text = "SELECT * FROM " + Entry_ty.ToString().Trim() + "MAIN";
            }
        }

        public void BindData()
        {
            txt_query.Text = Query;
            txt_reportquery.Text = Reportquery;
            ParametersList(Parameters);
            if (Reportquerytype == "Q")
                rbtnQuery.Checked = true;
            else
                if (Reportquerytype == "SP")
                    rbtnSP.Checked = true;
        }

        private void ParametersList(string paramlist)
        {
            if (paramlist == string.Empty || paramlist == null)
                return;
            string[] paramlst = paramlist.Split(',');
            foreach (string lst in paramlst)
            {
                lstvwParameters.Items.Add(lst.ToString().Trim());
            }
        }

        private void Binding()
        {
            Query = txt_query.Text.ToString().Trim();
            Reportquery = txt_reportquery.Text.ToString().Trim();
            Parameters = ParametersList();
            if (rbtnQuery.Checked)
                Reportquerytype = "Q";
            else if (rbtnSP.Checked)
            {
                Reportquerytype = "SP";
            }
        }

        private string ParametersList()
        {
            string paramlist = "";
            for (int i = 0; i < lstvwParameters.Items.Count; i++)
            {
                paramlist = (paramlist != string.Empty ? paramlist + "," : paramlist) + lstvwParameters.Items[i].Text.ToString();
            }
            return paramlist;
        }

        public string GetItemText(ListView LVIEW)
        {
            int nTotalSelected = LVIEW.SelectedIndices.Count;
            if (nTotalSelected <= 0) return "";
            IEnumerator selCol = LVIEW.SelectedItems.GetEnumerator();
            selCol.MoveNext();
            ListViewItem lvi = (ListViewItem)selCol.Current;
            string mDir = "";
            for (int i = 0; i < lvi.SubItems.Count; i++)
                mDir += lvi.SubItems[i].Text + ",";
            mDir = mDir.Substring(0, mDir.Length - 1);
            return mDir;
        }

        private void QueryTypeCheck()
        {
            if (rbtnQuery.Checked == false && rbtnSP.Checked == false)
            {
                throw new Exception("Please select the query type!!");
            }
            else if (rbtnQuery.Checked == true || rbtnSP.Checked == true)
            {
                if (rbtnQuery.Checked)
                {
                    if (!txt_reportquery.Text.Contains("SELECT") && !txt_reportquery.Text.Contains("FROM") && !txt_reportquery.Text.Contains("INNER"))
                    {
                        throw new Exception("Please enter the proper query!!");
                    }
                }
                else if (rbtnSP.Checked)
                {
                    if (txt_reportquery.Text.Contains("SELECT"))
                    {
                        throw new Exception("Please enter the proper stored procedure!!");
                    }
                }
            }
        }
        #endregion

        public frmQueryWindow(Int32 CompanyID, String Connectionstring)//satish pal
        {
            InitializeComponent();
            m_obj_cls_Gen_Mgr_Email_Client = new cls_Gen_Mgr_Email_Client(CompanyID, Connectionstring);//satish pal
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Binding();
            try
            {
                QueryTypeCheck();
                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), vumess, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmQueryWindow_Load(object sender, EventArgs e)
        {
            lblQryP1.Text = "";
            lblQryP2.Text = "";
            CallMethod(Action);
            ShowQuery(entry_ty);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            if (txt_query.Focus() && txt_query.Text != string.Empty)
            {
                lblQryP1.Text = m_obj_cls_Gen_Mgr_Email_Client.QueryResult(txt_query.Text.Trim().ToString());
                if (lblQryP1.Text == "Query Executed Successfully")
                {
                    lstvwFields.Items.Clear();
                    foreach (DataColumn m_col in m_obj_cls_Gen_Mgr_Email_Client.DsQuery.Tables[0].Columns)
                    {
                        ListViewItem lstviewitem = new ListViewItem();
                        lstviewitem.Text = m_col.ColumnName.ToUpper().ToString().Trim();
                        lstvwFields.Items.Add(lstviewitem);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please enter the query!!", vumess, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_query.Focus();
            }
        }

        private void txt_query_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                btnExecute.PerformClick();
            }
            if (e.Control && e.Alt && e.KeyCode == Keys.Z)
            {
                Undo1.PerformClick();
            }
            if (e.Control && e.KeyCode == Keys.X)
            {
                Cut1.PerformClick();
            }
            if (e.Control && e.KeyCode == Keys.C)
            {
                Copy1.PerformClick();
            }
            if (e.Control && e.KeyCode == Keys.V)
            {
                Paste1.PerformClick();
            }
            if (e.KeyCode == Keys.Delete)
            {
                Delete1.PerformClick();
            }
            if (e.Control && e.KeyCode == Keys.A)
            {
                SelectAll1.PerformClick();
            }
        }

        private void txt_reportquery_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.Alt && e.KeyCode == Keys.Z)
            {
                Undo.PerformClick();
            }
            if (e.Control && e.KeyCode == Keys.X)
            {
                Cut.PerformClick();
            }
            if (e.Control && e.KeyCode == Keys.C)
            {
                Copy.PerformClick();
            }
            if (e.Control && e.KeyCode == Keys.V)
            {
                Paste.PerformClick();
            }
            if (e.KeyCode == Keys.Delete)
            {
                Delete.PerformClick();
            }
            if (e.Control && e.KeyCode == Keys.A)
            {
                SelectAll.PerformClick();
            }
        }

        private void frmQueryWindow_KeyDown(object sender, KeyEventArgs e)
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

        private void lstvwParameters_DragDrop(object sender, DragEventArgs e)
        {
            string item1 = e.Data.GetData(DataFormats.Text).ToString();
            string[] item2 = item1.Split(',');
            lstvwParameters.Items.Add(new ListViewItem(item2, 0));
            lv1_mdown = false;
            lv2_mdown = false;
        }

        private void lstvwParameters_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void lstvwFields_MouseMove(object sender, MouseEventArgs e)
        {
            if (!lv1_mdown) return;
            if (e.Button == MouseButtons.Right) return;
            string str = GetItemText(lstvwFields);
            if (str == "") return;
            if (lstvwParameters.Items.Count > 0)
            {
                for (int i = 0; i < lstvwParameters.Items.Count; i++)
                {
                    if (str == lstvwParameters.Items[i].Text)
                    {
                        return;
                    }
                }
            }
            lstvwFields.DoDragDrop(str, DragDropEffects.Copy | DragDropEffects.Move);
        }

        private void lstvwFields_MouseDown(object sender, MouseEventArgs e)
        {
            lv1_mdown = true;
        }

        private void lstvwParameters_MouseDown(object sender, MouseEventArgs e)
        {
            lv2_mdown = true;
        }

        private void lstvwParameters_KeyDown(object sender, KeyEventArgs e)
        {
            if (lstvwParameters.Items.Count > 0)
            {
                if (e.KeyCode == Keys.Delete)
                {
                    for (int i = 0; i < lstvwParameters.Items.Count; i++)
                    {
                        if (lstvwParameters.Items[i].Selected)
                        {
                            lstvwParameters.Items[i].Remove();
                        }
                    }
                }
            }
        }

        private void btnMoveFldOne_Click(object sender, EventArgs e)
        {
            if (lstvwFields.Items.Count > 0)
            {
                if (lstvwFields.SelectedItems.Count == 0)
                {
                    MessageBox.Show("Please select the item!!", vumess, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    for (int i = 0; i < lstvwFields.Items.Count; i++)
                    {
                        if (lstvwFields.Items[i].Selected)
                        {
                            for (int j = 0; j < lstvwParameters.Items.Count; j++)
                            {
                                if (lstvwFields.Items[i].Text == lstvwParameters.Items[j].Text)
                                {
                                    MessageBox.Show("Selected Field already exists in Parameters List!!", vumess, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }
                            lstvwParameters.Items.Add(lstvwFields.Items[i].Text.ToString().Trim());
                        }
                    }
                }
            }
        }

        private void btnMoveFldAll_Click(object sender, EventArgs e)
        {
            if (lstvwFields.Items.Count > 0)
            {
                if (lstvwParameters.Items.Count > 0 && lstvwParameters.Items.Count != lstvwFields.Items.Count)
                {
                    lstvwParameters.Items.Clear();
                }
                for (int i = 0; i < lstvwFields.Items.Count; i++)
                {
                    for (int j = 0; j < lstvwParameters.Items.Count; j++)
                    {
                        if (lstvwFields.Items[i].Text == lstvwParameters.Items[j].Text)
                        {
                            MessageBox.Show("Fields already exists in Parameters List!!", vumess, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    lstvwParameters.Items.Add(lstvwFields.Items[i].Text.ToString().Trim());
                }
            }
        }

        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {
            if (txt_reportquery.CanUndo)
                contextMenuStrip2.Items["Undo"].Enabled = true;
            else
                contextMenuStrip2.Items["Undo"].Enabled = true;
            if (txt_reportquery.SelectedText.Length == 0)
            {
                contextMenuStrip2.Items["Cut"].Enabled = false;
                contextMenuStrip2.Items["Copy"].Enabled = false;
                contextMenuStrip2.Items["Delete"].Enabled = false;
            }
            else
            {
                contextMenuStrip2.Items["Cut"].Enabled = true;
                contextMenuStrip2.Items["Copy"].Enabled = true;
                contextMenuStrip2.Items["Delete"].Enabled = true;
            }
            if (Clipboard.ContainsText())
                contextMenuStrip2.Items["Paste"].Enabled = true;
            else
                contextMenuStrip2.Items["Paste"].Enabled = false;
            if(txt_reportquery.Text.Length == 0)
                contextMenuStrip2.Items["SelectAll"].Enabled = false;
            else
                contextMenuStrip2.Items["SelectAll"].Enabled = true;
            if(txt_reportquery.Text.Length == 0)
                contextMenuStrip2.Items["Execute"].Enabled = false;
            else
                contextMenuStrip2.Items["Execute"].Enabled = true;
        }

        private void Undo_Click(object sender, EventArgs e)
        {
            txt_reportquery.Undo();
        }

        private void Cut_Click(object sender, EventArgs e)
        {
            txt_reportquery.Cut();
        }

        private void Copy_Click(object sender, EventArgs e)
        {
            txt_reportquery.Copy();
        }

        private void Paste_Click(object sender, EventArgs e)
        {
            txt_reportquery.Paste();
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            int SelectionIndex = txt_reportquery.SelectionStart;
            int SelectionCount = txt_reportquery.SelectionLength;
            if (SelectionCount == 0)
                SelectionCount = 1;
            txt_reportquery.Text = txt_reportquery.Text.Remove(SelectionIndex, SelectionCount);
            txt_reportquery.SelectionStart = SelectionIndex;
        }

        private void SelectAll_Click(object sender, EventArgs e)
        {
            txt_reportquery.SelectAll();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (txt_query.CanUndo)
                contextMenuStrip1.Items["Undo1"].Enabled = true;
            else
                contextMenuStrip1.Items["Undo1"].Enabled = true;
            if (txt_query.SelectedText.Length == 0)
            {
                contextMenuStrip1.Items["Cut1"].Enabled = false;
                contextMenuStrip1.Items["Copy1"].Enabled = false;
                contextMenuStrip1.Items["Delete1"].Enabled = false;
            }
            else
            {
                contextMenuStrip1.Items["Cut1"].Enabled = true;
                contextMenuStrip1.Items["Copy1"].Enabled = true;
                contextMenuStrip1.Items["Delete1"].Enabled = true;
            }
            if (Clipboard.ContainsText())
                contextMenuStrip1.Items["Paste1"].Enabled = true;
            else
                contextMenuStrip1.Items["Paste1"].Enabled = false;
            if (txt_query.Text.Length == 0)
                contextMenuStrip1.Items["SelectAll1"].Enabled = false;
            else
                contextMenuStrip1.Items["SelectAll1"].Enabled = true;
            if (txt_query.Text.Length == 0)
                contextMenuStrip1.Items["Execute1"].Enabled = false;
            else
                contextMenuStrip1.Items["Execute1"].Enabled = true;
        }

        private void Undo1_Click(object sender, EventArgs e)
        {
            txt_query.Undo();
        }

        private void Cut1_Click(object sender, EventArgs e)
        {
            txt_query.Cut();
        }

        private void Copy1_Click(object sender, EventArgs e)
        {
            txt_query.Copy();
        }

        private void Paste1_Click(object sender, EventArgs e)
        {
            txt_query.Paste();
        }

        private void Delete1_Click(object sender, EventArgs e)
        {
            int SelectionIndex = txt_query.SelectionStart;
            int SelectionCount = txt_query.SelectionLength;
            if (SelectionCount == 0)
                SelectionCount = 1;
            txt_query.Text = txt_query.Text.Remove(SelectionIndex, SelectionCount);
            txt_query.SelectionStart = SelectionIndex;
        }

        private void SelectAll1_Click(object sender, EventArgs e)
        {
            txt_query.SelectAll();
        }

        private void Execute1_Click(object sender, EventArgs e)
        {
            btnExecute.PerformClick();
        }
    }
}
