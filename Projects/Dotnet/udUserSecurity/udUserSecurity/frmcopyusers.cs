using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace udUserSecurity
{
    public partial class frmcopyusers : uBaseForm.FrmBaseForm
    {
        DataAccess_Net.clsDataAccess oDataAccess;
        uBaseForm.FrmBaseForm vParentForm;
        string uname, i_pass, lcpassword;
        string vRole, vUser, vMenu, vParaList = string.Empty; bool cValid;
        string sqlstr;
        DataTable dtuser;
        DataView dvw;

        public frmcopyusers()
        {
            InitializeComponent();
        }
        public uBaseForm.FrmBaseForm pParentForm
        {
            get { return vParentForm; }
            set { vParentForm = value; }
        }
        public string pRole
        {
            get { return vRole; }
            set { vRole = value; }
        }

        public string pUser
        {
            get { return vUser; }
            set { vUser= value; }
        }
        public string pMenu
        {
            get { return vMenu; }
            set { vMenu = value; }
        }

        private void frmcopyusers_Load(object sender, EventArgs e)
        {
            string[] args = { };
            if (this.pPara != null)
            {
                args = this.pPara;
            }
            if (args.Length < 1)
            {
                //args = new string[] { "19", "A021112", "desktop246", "sa", "sa@1985", "^18010", "ADMIN", @"D:\Usquare\Bmp\Icon_usquare.ico", "Usquare pack", "Usquare.exe", "1", "udpid6096DTM20110307112715" };/*Rup Add ICO Path,Parent Appl Caption,Parent Appl. Name,Parent Appl PId,Application Log Table*/
                args = new string[] { "1", "P011213", "UDYOG5-PC\\Vudyogsdk", "sa", "sa1985", "^14207", "ADMIN", @"D:\USQUARE\Bmp\icon_10USquare.ico", "USquare", "USQUARE.EXE", "4764", "udPID4764DTM20111213125821" };
            }

            this.Text = this.pFrmCaption = "Copying user - " + this.vUser.Trim();
        }

        private void txtUserId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) || char.IsNumber(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void txtUserName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) || char.IsNumber(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
           // MessageBox.Show(txtPwd.Text);
            cValid = true;
            mthValidation();
            if (cValid == false)
            {
                return;
            }
            uname = this.txtUserId.Text.Trim().ToUpper();
            if (uname.Length < 10)
            {
               uname=uname.PadRight(10, ' ');                
            }
            i_pass = " ";
            lcpassword = this.txtPwd.Text.Trim();
            for (int i = 0; i < lcpassword.Length; i++)
            {
                //i_pass=i_pass+char(ASCIIEncoding((lcpassword,i,1)) + asc(substr(uname,i,1)))
                //i_pass=i_pass+chr(asc(substr(lcpassword,i,1)) + asc(substr(uname,i,1)))

               //i_pass=i_pass+Convert.ToChar(Convert.ToInt32(lcpassword.Substring(i,1)))+ Convert.ToInt32(uname.Substring(i,1));

                //int j=0;
                //int.TryParse(lcpassword.Substring(i, 1), out j);

               //string s= (System.Text.Encoding.UTF8.GetBytes(Convert.ToChar(lcpassword.Substring(i, 1)))).ToString();

            

                i_pass = i_pass + Convert.ToChar((int)Convert.ToChar(lcpassword.Substring(i, 1)) + (int)Convert.ToChar(uname.Substring(i, 1)));
               //MessageBox.Show(i_pass);
                
            }
            
            sqlstr = "select * from  vudyog..[user] where upper([user])='" + this.pUser.Trim().ToUpper() + "'";
            dtuser = oDataAccess.GetDataTable(sqlstr, null, 30);

            

            if(dtuser.Rows.Count>0)
            {
                string myhexstr1 = BitConverter.ToString((byte[])dtuser.Rows[0]["company"]).Replace("-", "");
                string myhexstr2 = BitConverter.ToString((byte[])dtuser.Rows[0]["passlog"]).Replace("-", "");

                sqlstr = " insert into vudyog..[user] ([user],password,log_exit,log_company,log_year ,log_machine,user_name,user_roles,company,passlog,tooldock) values('";
                //sqlstr = sqlstr + this.txtUserId.Text.Trim() + "',cast('" + i_pass.Trim() + "' as varbinary(250)),'.f.','','','','" + this.txtUserName.Text.Trim() + "','" + this.pRole.Trim() + "','" + "0x" + BitConverter.ToString((byte[])dtuser.Rows[0]["company"]).Replace("-", "") + "','" + "0x" + BitConverter.ToString((byte[])dtuser.Rows[0]["passlog"]).Replace("-", "") + "','" + dtuser.Rows[0]["tooldock"].ToString().Trim() + "')";
                sqlstr = sqlstr + this.txtUserId.Text.Trim() + "',cast('" + i_pass.Trim() + "' as varbinary(250)),0,'','','','" + this.txtUserName.Text.Trim() + "','" + this.pRole.Trim() + "',0x" + "" + myhexstr1 + ",0x" + myhexstr2 + ",'" + dtuser.Rows[0]["tooldock"].ToString().Trim() + "')";
                try
                {
                    oDataAccess.ExecuteSQLStatement(sqlstr, null, 30, true);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                               

                timer1.Enabled = true;
                timer1.Interval = 1000;
                MessageBox.Show("Successfully Updated!!", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);                                   
                this.Close();

               
            }
          

        }

        private void mthValidation()
        {
            if (txtUserId.Text.Trim() == "")
            {
                MessageBox.Show("UserID Cannot be empty!");
                cValid = false;
                return;
            }
            if (txtUserName.Text.Trim() == "")
            {
                MessageBox.Show("User Name Cannot be empty!");
                cValid = false;
                return;
            }
            if (txtPwd.Text.Trim() == "")
            {
                MessageBox.Show("Password Cannot be empty!");
                cValid = false;
                return;
            }
            if (txtCPwd.Text.Trim() == "")
            {
                MessageBox.Show("Confirm Password Cannot be empty!");
                cValid = false;
                return;
            }
            if(txtPwd.Text.Trim()!=txtCPwd.Text.Trim())
            {
                MessageBox.Show("Confirm password does not match with new password!");
                cValid = false;
                return;
            }
            sqlstr = "select * from vudyog..[user] where upper([user])='" + this.txtUserId.Text.Trim().ToUpper() + "'";

            oDataAccess = new DataAccess_Net.clsDataAccess();

            DataTable dt1 = oDataAccess.GetDataTable(sqlstr, null, 30);
            if (dt1.Rows.Count > 0)
            {
                MessageBox.Show(" UserID already exist.");
                cValid = false;
                return;
            }
          
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            SendKeys.Send("{ENTER}");
            timer1.Enabled = false;
        }

    }
}
