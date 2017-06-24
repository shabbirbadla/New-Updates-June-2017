using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

using eMailClient.BLL;

namespace eMailClient
{
    public partial class frmEmailClient : Form
    {
        #region Define Variables
        cls_Gen_Mgr_Email_Client m_obj_cls_Gen_Mgr_Email_Client;
       
        private bool AddMode = false;
        private bool EditMode = false;
        public string vumess = "iTAX";
        int trantypecount = 0;

        private Int32 companyID;

        public Int32 CompanyID
        {
            get { return companyID; }
            set { companyID = value; }
        }

        //satish pal-Start
        private string connectionstring;

        public string Connectionstring
        {
            get { return connectionstring; }
            set { connectionstring = value; }
        }
        //satish pal-End
        private List<string> emailId;

        public List<string> EmailId
        {
            get { return emailId; }
            set { emailId = value; }
        }

        #endregion

        #region Enumeration Members
        public enum eMailClient_RowName : int
        {
            id = 0,
            desc,
            tran_typ,
            hasattachment,
            attachment_typ,
            rep_nm,
            to,
            cc,
            bcc,
            subject,
            body,
            query,
            reportquery,
            parameters,
            separator,
            encoding,
            isfirstrow,
            reportquerytype,
            exportpath,
            exportprefixname,
            removefiles,
            emaillogfiles,
            logemailid
        }
        #endregion

        #region Other Methods
        private void BindData()
        {
            txt_Id.Text = m_obj_cls_Gen_Mgr_Email_Client.Id;
            txt_desc.Text = m_obj_cls_Gen_Mgr_Email_Client.Desc;
            TranTypeCheck(m_obj_cls_Gen_Mgr_Email_Client.Tran_typ);
            chk_hasattachment.Checked = m_obj_cls_Gen_Mgr_Email_Client.Hasattachment;
            cmb_attachment_typ.Text = m_obj_cls_Gen_Mgr_Email_Client.Attachment_typ;
            txt_rep_nm.Text = m_obj_cls_Gen_Mgr_Email_Client.Rep_nm;
            txt_to.Text = m_obj_cls_Gen_Mgr_Email_Client.To;
            txt_cc.Text = m_obj_cls_Gen_Mgr_Email_Client.Cc;
            txt_bcc.Text = m_obj_cls_Gen_Mgr_Email_Client.Bcc;
            txt_subject.Text = m_obj_cls_Gen_Mgr_Email_Client.Subject;
            txt_body.Text = m_obj_cls_Gen_Mgr_Email_Client.Body;
            txtExportPath.Text = m_obj_cls_Gen_Mgr_Email_Client.Exportpath;
            txt_filenameprefix.Text = m_obj_cls_Gen_Mgr_Email_Client.Exportprefixname;
            chkremovefiles.Checked = m_obj_cls_Gen_Mgr_Email_Client.Removefiles;
            chkemaillogfiles.Checked = m_obj_cls_Gen_Mgr_Email_Client.Emaillogfiles;
            txtlogemailid.Text = m_obj_cls_Gen_Mgr_Email_Client.Logemailid;
        }

        private void ClearFields()
        {
            txt_Id.Clear();
            txt_desc.Clear();
            for (int i = 0; i < chklst_tran_typ.Items.Count; i++)
            {
                chklst_tran_typ.Items[i].Checked = false;
            }
            chk_hasattachment.CheckState = CheckState.Unchecked;
            txt_rep_nm.Clear();
            txt_to.Clear();
            txt_cc.Clear();
            txt_bcc.Clear();
            txt_subject.Clear();
            txt_body.Clear();
            txtExportPath.Clear();
            txt_filenameprefix.Clear();
            chkremovefiles.CheckState = CheckState.Unchecked;
            chkemaillogfiles.CheckState = CheckState.Unchecked;
            txtlogemailid.Clear();
            if (AddMode == true)
            {
                m_obj_cls_Gen_Mgr_Email_Client.Query = "";
                m_obj_cls_Gen_Mgr_Email_Client.Reportquery = "";
                m_obj_cls_Gen_Mgr_Email_Client.Parameters = "";
                m_obj_cls_Gen_Mgr_Email_Client.Reportquerytype = "";
                m_obj_cls_Gen_Mgr_Email_Client.Separator = "";
                m_obj_cls_Gen_Mgr_Email_Client.Encoding = "";
                m_obj_cls_Gen_Mgr_Email_Client.IsFirstrow = false;
            }
        }

        private void EnableDisableFields(bool value)
        {
            txt_Id.Enabled = false;
            txt_desc.Enabled = value;
            txt_rep_nm.Enabled = value;
            txt_to.Enabled = value;
            txt_cc.Enabled = value;
            txt_bcc.Enabled = value;
            txt_subject.Enabled = value;
            txt_body.Enabled = value;
            txtExportPath.Enabled = value;
            txt_filenameprefix.Enabled = value;
            txtlogemailid.Enabled = value;

            chklst_tran_typ.Enabled = value;
            chk_hasattachment.Enabled = value;
            chkremovefiles.Enabled = value;
            chkemaillogfiles.Enabled = value;

            cmb_attachment_typ.Enabled = value;

            btn_rep_nm.Enabled = value;
            btn_To.Enabled = value;
            btn_Cc.Enabled = value;
            btn_Bcc.Enabled = value;
            btn_Zoom.Enabled = value;
            btnQueryWin.Enabled = value;
            btnExportPath.Enabled = value;
            if (AddMode == true || EditMode == true)
            {
                tlsbtnSend.Enabled = false;
                tlsbtnPendingMails.Enabled = false;
            }
            else
            {
                if (AddMode == false && EditMode == false)
                {
                    if (txt_Id.Text != string.Empty)
                    {
                        tlsbtnSend.Enabled = true;
                        tlsbtnPendingMails.Enabled = true;
                    }
                    else
                    {
                        tlsbtnSend.Enabled = false;
                        tlsbtnPendingMails.Enabled = false;
                    }
                }
            }
            //foreach (Control ctl in groupBox1.Controls)
            //{
            //    foreach (Control ctl1 in groupBox2.Controls)
            //    {
            //        ctl1.Enabled = value;
            //    }
            //    ctl.Enabled = value;
            //}

            txt_Id.BackColor = Color.White;
            txt_desc.BackColor = Color.White;
            txt_rep_nm.BackColor = Color.White;
            txt_to.BackColor = Color.White;
            txt_cc.BackColor = Color.White;
            txt_bcc.BackColor = Color.White;
            txt_subject.BackColor = Color.White;
            txt_body.BackColor = Color.White;
            txtExportPath.BackColor = Color.White;
            txt_filenameprefix.BackColor = Color.White;
            txtlogemailid.BackColor = Color.White;
        }

        private void AttachmentTypeFill()
        {
            cmb_attachment_typ.Items.Clear();
            cmb_attachment_typ.Items.Add("");
            cmb_attachment_typ.Items.Add("PDF");
            cmb_attachment_typ.Items.Add("Excel");
            cmb_attachment_typ.Items.Add("CSV");
            cmb_attachment_typ.Items.Add("WORD");
        }

        private void HasAttachmentChk()
        {
            if (chk_hasattachment.CheckState == CheckState.Checked && (EditMode == true || AddMode == true))
            {
                cmb_attachment_typ.Enabled = true;
                if (cmb_attachment_typ.Text.Length != 0)
                {
                    if (cmb_attachment_typ.SelectedItem.ToString() == "CSV")
                    {
                        txt_rep_nm.Enabled = false;
                        btn_rep_nm.Enabled = false;
                    }
                }
            }
            else
            {
                cmb_attachment_typ.Enabled = false;
                txt_rep_nm.Enabled = false;
                btn_rep_nm.Enabled = false;
                txtExportPath.Enabled = false;
                btnExportPath.Enabled = false;
                txt_filenameprefix.Enabled = false;
                txt_rep_nm.Clear();
                txtExportPath.Clear();
                cmb_attachment_typ.Items.Clear();
                AttachmentTypeFill();
            }
        }

        private void Binding()
        {
            m_obj_cls_Gen_Mgr_Email_Client.Id = txt_Id.Text.ToString().Trim();
            m_obj_cls_Gen_Mgr_Email_Client.Desc = txt_desc.Text.ToString().Trim();
            m_obj_cls_Gen_Mgr_Email_Client.Tran_typ = TranTypeSelectedList();
            m_obj_cls_Gen_Mgr_Email_Client.Hasattachment = chk_hasattachment.Checked;
            m_obj_cls_Gen_Mgr_Email_Client.Attachment_typ = cmb_attachment_typ.Text.ToString().Trim();
            m_obj_cls_Gen_Mgr_Email_Client.Rep_nm = txt_rep_nm.Text.ToString().Trim();
            m_obj_cls_Gen_Mgr_Email_Client.To = txt_to.Text.ToString().Trim();
            m_obj_cls_Gen_Mgr_Email_Client.Cc = txt_cc.Text.ToString().Trim();
            m_obj_cls_Gen_Mgr_Email_Client.Bcc = txt_bcc.Text.ToString().Trim();
            m_obj_cls_Gen_Mgr_Email_Client.Subject = txt_subject.Text.ToString().Trim();
            m_obj_cls_Gen_Mgr_Email_Client.Body = txt_body.Text.ToString().Trim();
            if (txtExportPath.Text.EndsWith("\\") == true)
                m_obj_cls_Gen_Mgr_Email_Client.Exportpath = txtExportPath.Text.ToString().Trim();
            else
                m_obj_cls_Gen_Mgr_Email_Client.Exportpath = txtExportPath.Text.ToString().Trim() + "\\";
            m_obj_cls_Gen_Mgr_Email_Client.Exportprefixname = txt_filenameprefix.Text.ToString().Trim();
            m_obj_cls_Gen_Mgr_Email_Client.Removefiles = chkremovefiles.Checked;
            m_obj_cls_Gen_Mgr_Email_Client.Emaillogfiles = chkemaillogfiles.Checked;
            m_obj_cls_Gen_Mgr_Email_Client.Logemailid = txtlogemailid.Text.ToString().Trim();
        }

        public bool validateMultipleEmailIds(TextBox value)
        {
            bool isvalid = true;
            if (value.Text != string.Empty)
            {
                var result = value.Text.Split(',');
                for (int i = 0; i < result.Length; i++)
                {
                    if (!EmailValidation(result[i]))
                    {
                        return false;
                    }
                }
            }
            return isvalid;
        }

        public bool EmailValidation(string field)
        {
            bool isvalid = true;
            string emailpattern = @"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z][a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$";
            Match emailmatch = Regex.Match(field.Trim().ToString(), emailpattern, RegexOptions.IgnoreCase);
            if (!emailmatch.Success)
            {
                isvalid = false;
            }
            return isvalid;
        }

        private bool FieldsValidation()
        {
            bool isvalid = true;

   
            if (validateMultipleEmailIds(txt_to) == false)
            {
                errorProvider1.SetError(txt_to, "Email Id is invalid");
                isvalid = false;
                return false;
            }
            else
                errorProvider1.Clear();

            if (validateMultipleEmailIds(txt_cc) == false && txt_cc.Text != string.Empty)
            {
                errorProvider1.SetError(txt_cc, "Email Id is invalid");
                isvalid = false;
                return false;
            }
            else
                errorProvider1.Clear();

            if (validateMultipleEmailIds(txt_bcc) == false && txt_bcc.Text != string.Empty)
            {
                errorProvider1.SetError(txt_bcc, "Email Id is invalid");
                isvalid = false;
                return false;
            }
            else
                errorProvider1.Clear();

            if (chkemaillogfiles.Checked)
            {
                if (txtlogemailid.Text == string.Empty)
                {
                    errorProvider1.SetError(txtlogemailid, lbllogemailid.Text.ToString() + " cannot be blank");
                    isvalid = false;
                    return false;
                }
                else if (validateMultipleEmailIds(txtlogemailid) == false && txtlogemailid.Text != string.Empty)
                {
                    errorProvider1.SetError(txtlogemailid, "Email Id is invalid");
                    isvalid = false;
                    return false;
                }
                else
                    errorProvider1.Clear();
            }

            if (!Directory.Exists(txtExportPath.Text) && txtExportPath.Text != string.Empty)
            {
                errorProvider1.SetError(txtExportPath, "Invalid Directory Path");
                isvalid = false;
                return false;
            }

            if (!File.Exists(txt_rep_nm.Text) && txt_rep_nm.Text != string.Empty)
            {
                errorProvider1.SetError(txt_rep_nm, "File does not exists");
                isvalid = false;
                return false;
            }
            return isvalid;
        }

        private string TranTypeSelectedList()
        {
            string trantypelist = "";
            for (int i = 0; i < chklst_tran_typ.Items.Count; i++)
            {
                if (chklst_tran_typ.Items[i].Checked)
                {
                    trantypelist = (trantypelist != string.Empty ? trantypelist + "," : trantypelist) + chklst_tran_typ.Items[i].Text.ToString();
                }
            }
            return trantypelist;
        }

        private void TranTypeCheck(string trantype)
        {
            string[] entry_ty = trantype.Split(',');
            for (int i = 0; i < chklst_tran_typ.Items.Count; i++)
            {
                foreach (string type in entry_ty)
                {
                    if (type == chklst_tran_typ.Items[i].Text.ToString().Trim())
                    {
                        chklst_tran_typ.Items[i].Checked = true;
                    }
                }
            }
         }
        #endregion

        public frmEmailClient(Int32 CompanyID, String Connectionstring)//satish pal
        {

            InitializeComponent();
            m_obj_cls_Gen_Mgr_Email_Client = new cls_Gen_Mgr_Email_Client(CompanyID, Connectionstring);//satish pal
        }

        private void frmEmailClient_Load(object sender, EventArgs e)
        {
            m_obj_cls_Gen_Mgr_Email_Client.FillTranType();
            foreach (DataRow m_dr in m_obj_cls_Gen_Mgr_Email_Client.DsSearch.Tables[0].Rows)
            {
                ListViewItem lstviewitem = new ListViewItem();
                lstviewitem.Text = m_dr["Entry_ty"].ToString().Trim().ToUpper();
                lstviewitem.SubItems.Add(m_dr["Code_nm"].ToString().Trim().ToUpper());
                chklst_tran_typ.Items.Add(lstviewitem);
            }
            cmb_attachment_typ.Items.Clear();
            AttachmentTypeFill();
            tlsbtnAdd.Enabled = true;
            EnableDisableFields(false);
            ClearFields();
            cmb_attachment_typ.Enabled = false;
            btn_rep_nm.Enabled = false;
            btnExportPath.Enabled = false;
            groupBox2.ForeColor = Color.Tomato;
            foreach (Control ctl in groupBox2.Controls)
            {
                ctl.ForeColor = SystemColors.ControlText;
            }
            groupBox3.ForeColor = Color.Tomato;
            foreach (Control ctl in groupBox3.Controls)
            {
                ctl.ForeColor = SystemColors.ControlText;
            }
        }

        private void tlsbtnAdd_Click(object sender, EventArgs e)
        {
            AddMode = true;
            EditMode = false;
            tlsbtnAdd.Enabled = false;
            tlsbtnEdit.Enabled = false;
            tlsbtnDelete.Enabled = false;
            tlsbtnSave.Enabled = true;
            tlsbtnCancel.Enabled = true;
            tlsbtnExit.Enabled = false;
            tlsbtnSearch.Enabled = false;
            ClearFields();
            cmb_attachment_typ.Items.Clear();
            EnableDisableFields(true);
            AttachmentTypeFill();
            txt_desc.Focus();
            cmb_attachment_typ.Enabled = false;
            txt_rep_nm.Enabled = false;
            btn_rep_nm.Enabled = false;
            txtExportPath.Enabled = false;
            btnExportPath.Enabled = false;
            txt_filenameprefix.Enabled = false;
            txtlogemailid.Enabled = false;
            m_obj_cls_Gen_Mgr_Email_Client.AutoId();
            foreach (DataRow m_dr in m_obj_cls_Gen_Mgr_Email_Client.DsSearch.Tables[0].Rows)
            {
                txt_Id.Text = m_dr[0].ToString().Trim();
            }
        }

        private void tlsbtnExit_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Are you sure to exit?", vumess, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (res == DialogResult.Yes)
            {
                Dispose(true);
                Close();
            }
        }

        private void tlsbtnEdit_Click(object sender, EventArgs e)
        {
            AddMode = false;
            EditMode = true;
            tlsbtnAdd.Enabled = false;
            tlsbtnEdit.Enabled = false;
            tlsbtnDelete.Enabled = false;
            tlsbtnSave.Enabled = true;
            tlsbtnCancel.Enabled = true;
            tlsbtnExit.Enabled = false;
            tlsbtnSearch.Enabled = false;
            EnableDisableFields(true);
            HasAttachmentChk();
        }

        private void tlsbtnCancel_Click(object sender, EventArgs e)
        {
            bool value;
            if (AddMode == true)
            {
                value = false;
                ClearFields();
                cmb_attachment_typ.Items.Clear();
                AttachmentTypeFill();
            }
            else
            {
                value = true;
                BindData();
            }
            AddMode = false;
            EditMode = false;
            tlsbtnAdd.Enabled = true;
            tlsbtnEdit.Enabled = value;
            tlsbtnDelete.Enabled = value;
            tlsbtnSave.Enabled = false;
            tlsbtnCancel.Enabled = false;
            tlsbtnExit.Enabled = true;
            tlsbtnSearch.Enabled = true;
            //ClearFields();
            //cmb_attachment_typ.Items.Clear();
            EnableDisableFields(false);
            //AttachmentTypeFill();
            errorProvider1.Clear();
        }

        private void tlsbtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (FieldsValidation() == false)
                    return;
                else
                    errorProvider1.Clear();

                if (txt_subject.Text == string.Empty && txt_body.Text == string.Empty)
                {
                    DialogResult res = MessageBox.Show("Save this message without a subject or text in the body?", vumess, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (res == DialogResult.Cancel)
                    {
                        return;
                    }
                }

                Binding();

                if (AddMode == true)
                {
                    m_obj_cls_Gen_Mgr_Email_Client.Insert();
                }
                else
                    if (EditMode == true)
                    {
                        m_obj_cls_Gen_Mgr_Email_Client.Update();
                    }

                MessageBox.Show("Entry Saved", vumess, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " Entry cannot be Saved", vumess, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }

            AddMode = false;
            EditMode = false;
            tlsbtnAdd.Enabled = true;
            tlsbtnEdit.Enabled = true;
            tlsbtnDelete.Enabled = true;
            tlsbtnSave.Enabled = false;
            tlsbtnCancel.Enabled = false;
            tlsbtnExit.Enabled = true;
            tlsbtnSearch.Enabled = true;
            BindData();
            //ClearFields();
            EnableDisableFields(false);
            //AttachmentTypeFill();
            errorProvider1.Clear();
        }

        private void tlsbtnSearch_Click(object sender, EventArgs e)
        {
            AddMode = false;
            EditMode = false;
            m_obj_cls_Gen_Mgr_Email_Client.SearchById();
            frmSearch m_obj_frmsearch = new frmSearch(CompanyID, connectionstring);
            m_obj_frmsearch.SeachValidate(m_obj_cls_Gen_Mgr_Email_Client.DsSearch.Tables[0], "Id+Desc+Subject", "Id", "Id:Id,Desc:Email Description,Subject:Email Subject");
            if (m_obj_cls_Gen_Mgr_Email_Client.DsSearch.Tables[0].Rows.Count > 0)
            {
                m_obj_frmsearch.ShowDialog();
                if (m_obj_frmsearch.DialogResult == DialogResult.OK || m_obj_frmsearch.oSelectedRow != null)
                {
                    m_obj_cls_Gen_Mgr_Email_Client.Id = (string)m_obj_frmsearch.oSelectedRow["Id"];
                    m_obj_cls_Gen_Mgr_Email_Client.Select();

                    ClearFields();
                    BindData();
                    tlsbtnAdd.Enabled = true;
                    tlsbtnEdit.Enabled = true;
                    tlsbtnDelete.Enabled = true;
                    tlsbtnSave.Enabled = false;
                    tlsbtnCancel.Enabled = false;
                    EnableDisableFields(false);
                }
            }
            else
            {
                MessageBox.Show("No Mails found!!", vumess, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void tlsbtnDelete_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Are you sure to delete this email?", vumess, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (res == DialogResult.Yes)
            {
                try
                {
                    m_obj_cls_Gen_Mgr_Email_Client.Delete();
                    MessageBox.Show("Entry Deleted", vumess, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " Entry cannot be Deleted", vumess, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    return;
                }
                AddMode = false;
                EditMode = false;
                tlsbtnAdd.Enabled = true;
                tlsbtnEdit.Enabled = false;
                tlsbtnDelete.Enabled = false;
                tlsbtnSave.Enabled = false;
                tlsbtnCancel.Enabled = false;
                tlsbtnExit.Enabled = true;
                tlsbtnSearch.Enabled = true;
                ClearFields();
                EnableDisableFields(false);
                AttachmentTypeFill();
                errorProvider1.Clear();
            }
        }

        private void btn_To_Click(object sender, EventArgs e)
        {
            frmEmailList m_obj_frmemaillist = new frmEmailList(CompanyID, Connectionstring);
            m_obj_frmemaillist.ShowDialog();
            if (m_obj_frmemaillist.DialogResult == DialogResult.OK && m_obj_frmemaillist.cEmailList != null)
            {
                m_obj_cls_Gen_Mgr_Email_Client.To = m_obj_frmemaillist.cEmailList;
                txt_to.Text = m_obj_cls_Gen_Mgr_Email_Client.To;
            }
        }

        private void btn_Cc_Click(object sender, EventArgs e)
        {
            frmEmailList m_obj_frmemaillist = new frmEmailList(CompanyID, Connectionstring);
            m_obj_frmemaillist.ShowDialog();
            if (m_obj_frmemaillist.DialogResult == DialogResult.OK && m_obj_frmemaillist.cEmailList != null)
            {
                m_obj_cls_Gen_Mgr_Email_Client.Cc = m_obj_frmemaillist.cEmailList;
                txt_cc.Text = m_obj_cls_Gen_Mgr_Email_Client.Cc;
            }
        }

        private void btn_Bcc_Click(object sender, EventArgs e)
        {
            frmEmailList m_obj_frmemaillist = new frmEmailList(CompanyID, Connectionstring);
            m_obj_frmemaillist.ShowDialog();
            if (m_obj_frmemaillist.DialogResult == DialogResult.OK && m_obj_frmemaillist.cEmailList != null)
            {
                m_obj_cls_Gen_Mgr_Email_Client.Bcc = m_obj_frmemaillist.cEmailList;
                txt_bcc.Text = m_obj_cls_Gen_Mgr_Email_Client.Bcc;
            }
        }

        private void btn_Zoom_Click(object sender, EventArgs e)
        {
            frmTranType m_obj_frmtrantype = new frmTranType(chklst_tran_typ);
            m_obj_frmtrantype.ShowDialog();
            if (m_obj_frmtrantype.DialogResult == DialogResult.OK && m_obj_frmtrantype.LstViewRet != null)
            {
                for (int i = 0; i < chklst_tran_typ.Items.Count; i++)
                {
                    foreach (ListViewItem item in m_obj_frmtrantype.LstViewRet.Items)
                    {
                        if (item.SubItems[0].Text == chklst_tran_typ.Items[i].SubItems[0].Text)
                        {
                            if (item.Checked == true)
                            {
                                chklst_tran_typ.Items[i].Checked = true;
                            }
                            else
                            {
                                chklst_tran_typ.Items[i].Checked = false;
                            }
                        }
                    }
                }
            }
        }

        private void frmEmailClient_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                tlsbtnAdd.PerformClick();
            }
            if (e.Control && e.KeyCode == Keys.E)
            {
                tlsbtnEdit.PerformClick();
            }
            if (e.Control && e.KeyCode == Keys.D)
            {
                tlsbtnDelete.PerformClick();
            }
            if (e.Control && e.KeyCode == Keys.S)
            {
                tlsbtnSave.PerformClick();
            }
            if (e.Control && e.KeyCode == Keys.Z)
            {
                tlsbtnCancel.PerformClick();
            }
            if (e.Alt && e.KeyCode == Keys.F4)
            {
                tlsbtnExit.PerformClick();
            }
            if (e.KeyCode == Keys.F2)
            {
                tlsbtnSearch.PerformClick();
            }
            if (e.Alt && e.KeyCode == Keys.S)
            {
                tlsbtnSend.PerformClick();
            }
            if (e.Alt && e.KeyCode == Keys.P)
            {
                tlsbtnPendingMails.PerformClick();
            }
        }

        private void txt_to_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                frmEmailList m_obj_frmemaillist = new frmEmailList(CompanyID, Connectionstring);//satish pal
                m_obj_frmemaillist.ShowDialog();
            }
        }

        private void txt_cc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                frmEmailList m_obj_frmemaillist = new frmEmailList(CompanyID, Connectionstring);//satish pal
                m_obj_frmemaillist.ShowDialog();
            }
        }

        private void txt_bcc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                frmEmailList m_obj_frmemaillist = new frmEmailList(CompanyID, Connectionstring);//satish pal
                m_obj_frmemaillist.ShowDialog();
            }
        }

        private void chklst_tran_typ_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (chk_hasattachment.Checked)
            {
                if (e.Item.Checked)
                {
                    trantypecount++;
                }
                else
                {
                    trantypecount--;
                }
                if (trantypecount == 2 && trantypecount < 3)
                {
                    MessageBox.Show("Transaction type selected is more than one, so please make sure that the attachment " +
                        "should be compatible with all the selected transactions!!", vumess, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                }
            }
        }

        private void btnQueryWin_Click(object sender, EventArgs e)
        {
            frmQueryWindow m_obj_frmquerywindow = new frmQueryWindow(CompanyID, connectionstring);
            if (AddMode == true)
            {
                string entry_ty = "";
                int trancount = 0;
                for (int i = 0; i < chklst_tran_typ.Items.Count; i++)
                {
                    if (chklst_tran_typ.Items[i].Checked)
                    {
                        entry_ty = chklst_tran_typ.Items[i].Text.ToString().Trim();
                        trancount++;
                    }
                    if (trancount > 1)
                    {
                        trancount = 0;
                        break;
                    }
                }
                if (trancount == 1)
                {
                    m_obj_frmquerywindow.Entry_ty = entry_ty.ToString();
                }
                m_obj_frmquerywindow.Action = "AddMode";
            }
            else
            {
                if (EditMode == true)
                {
                    m_obj_frmquerywindow.Action = "EditMode";
                }
            }
            m_obj_frmquerywindow.Query = m_obj_cls_Gen_Mgr_Email_Client.Query;
            m_obj_frmquerywindow.Reportquery = m_obj_cls_Gen_Mgr_Email_Client.Reportquery;
            m_obj_frmquerywindow.Parameters = m_obj_cls_Gen_Mgr_Email_Client.Parameters;
            m_obj_frmquerywindow.Reportquerytype = m_obj_cls_Gen_Mgr_Email_Client.Reportquerytype;
             
            m_obj_frmquerywindow.ShowDialog();

            m_obj_cls_Gen_Mgr_Email_Client.Query = m_obj_frmquerywindow.Query;
            m_obj_cls_Gen_Mgr_Email_Client.Reportquery = m_obj_frmquerywindow.Reportquery;
            m_obj_cls_Gen_Mgr_Email_Client.Parameters = m_obj_frmquerywindow.Parameters;
            m_obj_cls_Gen_Mgr_Email_Client.Reportquerytype = m_obj_frmquerywindow.Reportquerytype;
        }

        private void btn_rep_nm_Click(object sender, EventArgs e)
        {
            openFD.Title = "Select Report";
            openFD.FileName = "";
            openFD.Filter = "RPT|*.rpt";

            if (openFD.ShowDialog() == DialogResult.Cancel)
            {
                txt_rep_nm.Text = "";
            }
            else
            {
                txt_rep_nm.Text = openFD.FileName;
            }
        }

        private void chk_hasattachment_CheckedChanged(object sender, EventArgs e)
        {
            if (AddMode != false || EditMode != false)
            {
                HasAttachmentChk();
                if (chk_hasattachment.Checked)
                {
                    for (int i = 0; i < chklst_tran_typ.Items.Count; i++)
                    {
                        if (chklst_tran_typ.Items[i].Checked)
                        {
                            trantypecount++;
                        }
                        if (trantypecount == 2 && trantypecount < 3)
                        {
                            MessageBox.Show("Transaction Type selected is more than one, so please make sure that the attachment " +
                                "should be compatible with all the selected transactions!!", vumess, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                            trantypecount = 0;
                            break;
                        }
                    }
                }
                else
                {
                    m_obj_cls_Gen_Mgr_Email_Client.Separator = "";
                    m_obj_cls_Gen_Mgr_Email_Client.Encoding = "";
                    m_obj_cls_Gen_Mgr_Email_Client.IsFirstrow = false;
                }
            }
        }

        private void cmb_attachment_typ_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AddMode != false || EditMode != false)
            {
                if (cmb_attachment_typ.SelectedItem.ToString() != string.Empty)
                {
                    if (cmb_attachment_typ.SelectedItem.ToString() == "CSV")
                    {
                        frmExportCSV m_obj_frmexportcsv = new frmExportCSV(CompanyID, connectionstring);//satish pal
                        if (AddMode == true)
                            m_obj_frmexportcsv.Action = "AddMode";
                        else
                            if (EditMode == true)
                                m_obj_frmexportcsv.Action = "EditMode";
                        m_obj_frmexportcsv.Separator = m_obj_cls_Gen_Mgr_Email_Client.Separator == null ? "" : m_obj_cls_Gen_Mgr_Email_Client.Separator;
                        m_obj_frmexportcsv.Encoding = m_obj_cls_Gen_Mgr_Email_Client.Encoding == null ? "" : m_obj_cls_Gen_Mgr_Email_Client.Encoding;
                        m_obj_frmexportcsv.IsFirstrow = m_obj_cls_Gen_Mgr_Email_Client.IsFirstrow;

                        m_obj_frmexportcsv.ShowDialog();

                        m_obj_cls_Gen_Mgr_Email_Client.Separator = m_obj_frmexportcsv.Separator;
                        m_obj_cls_Gen_Mgr_Email_Client.Encoding = m_obj_frmexportcsv.Encoding;
                        m_obj_cls_Gen_Mgr_Email_Client.IsFirstrow = m_obj_frmexportcsv.IsFirstrow;
                        txt_rep_nm.Clear();
                        txt_rep_nm.Enabled = false;
                        btn_rep_nm.Enabled = false;
                        txtExportPath.Enabled = true;
                        btnExportPath.Enabled = true;
                        txt_filenameprefix.Enabled = true;
                    }
                    else
                    {
                        m_obj_cls_Gen_Mgr_Email_Client.Separator = "";
                        m_obj_cls_Gen_Mgr_Email_Client.Encoding = "";
                        m_obj_cls_Gen_Mgr_Email_Client.IsFirstrow = false;
                    }
                }
            }
            if (cmb_attachment_typ.SelectedItem.ToString() != string.Empty && cmb_attachment_typ.SelectedItem.ToString() != "CSV")
            {
                txtExportPath.Enabled = true;
                btnExportPath.Enabled = true;
                txt_rep_nm.Enabled = true;
                btn_rep_nm.Enabled = true;
                txt_filenameprefix.Enabled = true;
            }
            else
            {
                if (cmb_attachment_typ.SelectedItem.ToString() != "CSV")
                {
                    txtExportPath.Enabled = false;
                    btnExportPath.Enabled = false;
                    txt_rep_nm.Enabled = false;
                    btn_rep_nm.Enabled = false;
                    txt_filenameprefix.Enabled = false;
                }
            }
        }

        private void btnExportPath_Click(object sender, EventArgs e)
        {
            fbd1.ShowNewFolderButton = true;
            if (fbd1.ShowDialog() == DialogResult.Cancel)
                txtExportPath.Text = "";
            else
                txtExportPath.Text = fbd1.SelectedPath;
        }

        private void chkemaillogfiles_CheckedChanged(object sender, EventArgs e)
        {
            if (chkemaillogfiles.Checked)
            {
                txtlogemailid.Enabled = true;
                txtlogemailid.Focus();
            }
            else
            {
                txtlogemailid.Enabled = false;
                errorProvider1.Clear();
            }
        }

        private void tlsbtnSend_Click(object sender, EventArgs e)
        {
            if (txt_Id.Text != string.Empty)
            {
                if (txt_subject.Text == string.Empty && txt_body.Text == string.Empty)
                {
                    DialogResult res = MessageBox.Show("Send this message without a subject or text in the body?", vumess, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (res == DialogResult.Cancel)
                        return;
                }
 
                frmProcessWatcher obj_ProcessWatcher = new frmProcessWatcher(CompanyID);
                obj_ProcessWatcher.EmailID = new List<string>();
                obj_ProcessWatcher.EmailID.Add(txt_Id.Text.ToString().Trim());
                obj_ProcessWatcher.CompanyID = CompanyID;
                obj_ProcessWatcher.ExecuteJob = true;
                obj_ProcessWatcher.ShowDialog();
                if (obj_ProcessWatcher.LogStatusFileName != null)
                {
                    DialogResult nres = MessageBox.Show("Want to see email status log file?", vumess, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (nres == DialogResult.Yes)
                    {
                        frmLogViewer m_obj_frmlogviewer = new frmLogViewer();
                        m_obj_frmlogviewer.ReadLogFile(obj_ProcessWatcher.LogStatusFileName);
                        m_obj_frmlogviewer.ShowDialog();
                    }
                }
            }
        }

        private void tlsbtnPendingMails_Click(object sender, EventArgs e)
        {
            if (txt_Id.Text != string.Empty)
            {
                cls_Gen_Mgr_PendingMail m_Obj_PendingMail = new cls_Gen_Mgr_PendingMail(CompanyID);
                m_Obj_PendingMail.Id = txt_Id.Text.ToString().Trim();
                m_Obj_PendingMail.Select();

                if (m_Obj_PendingMail.DsSelect.Tables[0].Rows.Count > 0)
                {
                    frmPendingMail Obj_PendingMail = new frmPendingMail(CompanyID);
                    Obj_PendingMail.CompanyID = CompanyID;
                    Obj_PendingMail.Id = txt_Id.Text.ToString().Trim();
                    Obj_PendingMail.Obj_DsSelect = m_Obj_PendingMail.DsSelect;
                    Obj_PendingMail.ShowDialog();
                    if (Obj_PendingMail.DialogResult == DialogResult.OK)
                    {
                        frmProcessWatcher obj_ProcessWatcher = new frmProcessWatcher(CompanyID);
                        obj_ProcessWatcher.LogEmailID = new List<string>();
                        obj_ProcessWatcher.LogEmailID = Obj_PendingMail.EmailId;
                        obj_ProcessWatcher.CompanyID = CompanyID;
                        obj_ProcessWatcher.ExecutePendingJob = true;
                        obj_ProcessWatcher.ShowDialog();
                        if (obj_ProcessWatcher.LogStatusFileName != null)
                        {
                            DialogResult nres = MessageBox.Show("Want to see email status log file?", vumess, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                            if (nres == DialogResult.Yes)
                            {
                                frmLogViewer m_obj_frmlogviewer = new frmLogViewer();
                                m_obj_frmlogviewer.ReadLogFile(obj_ProcessWatcher.LogStatusFileName);
                                m_obj_frmlogviewer.ShowDialog();
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No Mails found Pending!!", vumess, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
        }
    }
}
