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
    public partial class frmuseraccess : uBaseForm.FrmBaseForm 
    {
        uBaseForm.FrmBaseForm vParentForm;
        string vRole, vMenu, vUser, vParaList = string.Empty, sqlstr, uname, i_pass, lcpassword;
        string o_pass;
        DataAccess_Net.clsDataAccess oDataAccess;
        bool cValid=true;
        DataTable dtUser, dtUserRole, dt, dtcomp;
        string ins = "";

        public frmuseraccess()
        {
            InitializeComponent();
        }

        public string pRole
        {
            get { return vRole; }
            set { vRole = value; }
        }

        public string pMenu
        {
            get { return vMenu; }
            set { vMenu = value; }
        }

        public string pUser
        {
            get { return vUser; }
            set { vUser = value; }
        }

        private void rbtnChangePwd_Click(object sender, EventArgs e)
        {

            //if (rbtnChangePwd.Checked)
            //{
            //    if (this.vMenu != "New User")
            //    {
            //        this.txtOldPass.Enabled = false;
            //        if (this.vUser == "ADMIN")
            //        {
            //            txtOldPass.Enabled = true;
            //        }

            //    }
            //    this.txtConfpass.Enabled = true;
            //    this.txtNewpass.Enabled = true;

            //}
            //else
            //{
            //    this.txtOldPass.Enabled = false;
            //    this.txtOldPass.Text = "";
            //    this.txtConfpass.Enabled = false;
            //    this.txtConfpass.Text = "";
            //    this.txtNewpass.Enabled = false;
            //    this.txtNewpass.Text = "";

            //}

          
            
        }

        private void frmuseraccess_Load(object sender, EventArgs e)
        {
            oDataAccess = new DataAccess_Net.clsDataAccess();
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

           
            this.Text = this.pFrmCaption = "User Properties " + this.vRole;
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
            //txtRole.Focus();
            comboBox1.Focus();

            sqlstr = "select * from vudyog..[user] where upper([user])='"+this.vUser+"'";
            dtUser = oDataAccess.GetDataTable(sqlstr, null, 30);


            sqlstr = "select distinct user_roles from vudyog..userroles";
            DataTable dtroles;
            dtroles = oDataAccess.GetDataTable(sqlstr, null, 30);
            foreach (DataRow dr in dtroles.Rows)
            {
                comboBox1.Items.Add(dr["user_roles"].ToString().Trim());
            }
            

            if (this.vMenu == "Properties")
            {
                //txtRole.Text = vRole.Trim();
                comboBox1.Text = vRole.Trim();
                //comboBox1.Items[0].ToString() = vRole.Trim();
               

                if (this.vUser == "ADMIN")
                {
                    comboBox1.Items.Clear();
                    comboBox1.Items.Add(vRole.Trim());
                    comboBox1.SelectedIndex=0;
                    //comboBox1.SelectedText = this.vRole.Trim();
                    comboBox1.Enabled = false;
                    this.txtUserName.Enabled = false;
                   
                }
                else
                {
                    //comboBox1.Items.Clear();
                    this.comboBox1.Enabled = true;
                    //sqlstr = "select distinct user_roles from vudyog..userroles";
                    //DataTable dtroles;
                    //dtroles = oDataAccess.GetDataTable(sqlstr, null, 30);
                    //foreach (DataRow dr in dtroles.Rows)
                    //{
                    //    comboBox1.Items.Add(dr["user_roles"].ToString().Trim());
                    //}

                    comboBox1.SelectedText = this.vRole.Trim();

                    comboBox1.Enabled = true;
                    //comboBox1.DisplayMember = this.vRole.Trim();
                    //comboBox1.SelectedItem = this.vRole.Trim();
                    //comboBox1.SelectedIndex = comboBox1.FindStringExact(this.vRole.Trim());

                    comboBox1.SelectedIndex = comboBox1.Items.IndexOf(this.vRole.Trim());
                    this.txtUserName.Enabled = true;
                }

                this.txtUserId.Text = this.vUser;
                this.txtUserId.Enabled = false;
                this.txtUserName.Text = dtUser.Rows[0]["user_name"].ToString().Trim();
            }
            else
            {
                comboBox1.Enabled = true;
                chkChangePwd.Checked = true;

                //sqlstr = "select distinct user_roles from vudyog..userroles";
                //DataTable dtroles;
                //dtroles = oDataAccess.GetDataTable(sqlstr, null, 30);
                //foreach (DataRow dr in dtroles.Rows)
                //{
                //    comboBox1.Items.Add(dr["user_roles"].ToString());
                //}

                //btnPermission.Enabled = false;
            }
            this.txtOldPass.Enabled = false;

            sqlstr = "select user_roles,isnull(company,cast('' as varbinary(250))) as company from  vudyog..userroles where user_roles='" + this.vRole + "'";

            // sqlstr = "select user_roles,cast(isnull(company,cast('' as varbinary)) as varchar) as company from  vudyog..userroles where user_roles='" + this.vRole + "'";
            DataTable dtuser1 = oDataAccess.GetDataTable(sqlstr, null, 30);

            //DataRow dr1 = dtuser1.Rows[0];
            byte[] s = (byte[])(dtuser1.Rows[0]["company"]);

            string s1 = "";


            if (s.Length > 0)
            {
                s1 = padLRStr(this.vRole, this.vRole, s.Length);


                byte[] asciiBytes1 = Encoding.ASCII.GetBytes(s1);

                for (int k = 0; k < s.Length; k++)
                {
                    //ins = ins + (char)((s[k]) - asciiBytes1[k]);
                    ins = ins + Convert.ToChar((s[k]) - asciiBytes1[k]);

                }
            }

        }

        private void chkChangePwd_CheckedChanged(object sender, EventArgs e)
        {
            if (chkChangePwd.Checked)
            {
                if (this.vMenu != "New User")
                {
                    this.txtOldPass.Enabled = false;
                   if(this.vUser == "ADMIN")
                    {
                    txtOldPass.Enabled = true;
                    }
                    
                }
                this.txtConfpass.Enabled = true;
                this.txtNewpass.Enabled = true;

            }
            else
            {
                this.txtOldPass.Enabled = false;
                this.txtOldPass.Text = "";
                this.txtConfpass.Enabled = false;
                this.txtConfpass.Text = "";
                this.txtNewpass.Enabled = false;
                this.txtNewpass.Text = "";

            }

        }

        private void butApply_Click(object sender, EventArgs e)
        {
            cValid = true;
            if (cValid == false)
            {
                cValid = true;
                return;
            }
            
            //sqlstr = "Select cast(company as varbinary(250))  as company from vudyog..userroles where user_roles='" + this.comboBox1.Text.Trim() + "'";
            //dtUserRole = oDataAccess.GetDataTable(sqlstr, null, 30);

            //byte[] s2 = (byte[])(dtUserRole.Rows[0]["company"]);

          

            sqlstr = "Select cast(company as varchar) as company1,* from vudyog..userroles where user_roles='" + this.comboBox1.Text.Trim() + "'";
            dtUserRole = oDataAccess.GetDataTable(sqlstr, null, 30);

            if(dtUserRole.Rows[0]["company1"].ToString()== string.Empty)
            {
                    MessageBox.Show("Cannot Assign selected role!!! Role is Empty");
                    cValid = false;
                    return;
            }

            mthchkValidation();
            if (cValid == false)
            {
                cValid = true;
                return;
            }

            uname = this.txtUserId.Text.Trim().ToUpper();
            if (uname.Length < 10)
            {
                uname = uname.PadRight(10, ' ');
            }
            i_pass = " ";
            lcpassword = this.txtNewpass.Text.Trim();
            for (int i = 0; i < lcpassword.Length; i++)
            {
               //string s= (System.Text.Encoding.UTF8.GetBytes(Convert.ToChar(lcpassword.Substring(i, 1)))).ToString();
                i_pass = i_pass + Convert.ToChar((int)Convert.ToChar(lcpassword.Substring(i, 1)) + (int)Convert.ToChar(uname.Substring(i, 1)));

            }
            if (this.vMenu == "New User")
            {
                if (dtUserRole.Rows.Count > 0)
                {
                    string myhexstr1 = BitConverter.ToString((byte[])dtUserRole.Rows[0]["company"]).Replace("-", "");
                    // string myhexstr2 = BitConverter.ToString((byte[])dtuser.Rows[0]["passlog"]).Replace("-", "");
                    sqlstr = " ";

                    sqlstr = "insert into vudyog..[user]([user],[user_name],[password],[user_roles],[company],[passlog]) values ('" + this.txtUserId.Text.Trim() + "','" + this.txtUserName.Text.Trim() + "',";
                    sqlstr = sqlstr + "cast('" + i_pass.Trim() + "' as varbinary(250)),'" + this.comboBox1.Text.Trim() + "',0x" + myhexstr1 + ",cast(" + "''" + " as varbinary(250)))";
                    oDataAccess.ExecuteSQLStatement(sqlstr, null, 30, true);
                }
            }
        }
        
        private void mthchkValidation()
        {
            if (this.vMenu == "New User")
            {
                if (this.txtUserId.Text.Trim() == "")
                {
                    MessageBox.Show("User ID cannot be empty!");
                    cValid = false;
                    return;
                }
                if (this.txtUserName.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("User Name cannot be empty!");
                    cValid = false;
                    return;
                }
                if (this.txtNewpass.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("New Password cannot be empty!");
                    cValid = false;
                    return;
                }
                if (this.txtConfpass.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Confirm Password cannot be empty!");
                    cValid = false;
                    return;
                }
                if (this.txtNewpass.Text.Trim() != this.txtConfpass.Text.Trim())
                {
                    MessageBox.Show("New Password doesnot match with the Confirm Password");
                    cValid = false;
                    return;

                }
                if (this.comboBox1.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("User Roles Cannot be empty!");
                    cValid = false;
                    return;
                }

                sqlstr = "Select [user] from vudyog..[user] where [user]='" + this.txtUserId.Text.Trim() + "'";
                dt = oDataAccess.GetDataTable(sqlstr, null, 30);
               

               if(dt.Rows.Count>0)
                {
                    MessageBox.Show("UserID already exist");
                    cValid = false;
                    return;
                }

            }
            else
            {
                if (this.txtNewpass.Text.Trim() != this.txtConfpass.Text.Trim())
                {
                    MessageBox.Show("New Password doesnot match with the Confirm Password");
                    cValid = false;
                    return;
                }

            }
        }
            
        private void txtNewpass_Enter(object sender, EventArgs e)
        {
            if (txtOldPass.Enabled == true)
            {
                if (this.txtOldPass.Text.ToString().Trim() == "")
                {
                    MessageBox.Show("Old password cannot be empty");
                    this.txtOldPass.Focus();
                    cValid = false;
                    return;
                }

                string o_pass = "";
                uname = this.txtUserId.Text.Trim().ToUpper();

                if (uname.Trim() == "ADMIN")
                {
                    sqlstr = "Select cast(password as varchar) as password  from vudyog..[user] where [user]='" + this.txtUserId.Text.Trim() + "'";
                    dt = oDataAccess.GetDataTable(sqlstr, null, 30);
                    i_pass = " ";
                    if (dt.Rows.Count > 0)
                    {
                        i_pass = dt.Rows[0]["password"].ToString();
                    }

                    if (uname.Length < 10)
                    {
                        uname = uname.PadRight(10, ' ');
                    }
                    for (int j = 0; j < i_pass.Length; j++)
                    {
                        //o_pass = o_pass + Chr(Asc(Substr(i_pass, j, 1)) - Asc(Substr(uname, j, 1)));
                        o_pass = o_pass + Convert.ToChar((int)Convert.ToChar(i_pass.Substring(j, 1)) - (int)Convert.ToChar(uname.Substring(j, 1)));

                    }
                    if (o_pass.Trim() != this.txtOldPass.Text.Trim())
                    {
                        MessageBox.Show("Incorrect password! ");
                        cValid = false;
                        this.txtOldPass.Focus();
                        return;
                    }
                }

            }

        }

        private void txtConfpass_Enter(object sender, EventArgs e)
        {
            if (txtOldPass.Enabled == true)
            {
                if (this.txtOldPass.Text.ToString().Trim() == "")
                {
                    MessageBox.Show("Old password cannot be empty");
                    this.txtOldPass.Focus();
                    cValid = false;
                    return;
                }

                string o_pass = "";
                uname = this.txtUserId.Text.Trim().ToUpper();

                if (uname.Trim() == "ADMIN")
                {
                    sqlstr = "Select cast(password as varchar) as password  from vudyog..[user] where [user]='" + this.txtUserId.Text.Trim() + "'";
                    dt = oDataAccess.GetDataTable(sqlstr, null, 30);
                    i_pass = " ";
                    if (dt.Rows.Count > 0)
                    {
                        i_pass = dt.Rows[0]["password"].ToString();
                    }

                    if (uname.Length < 10)
                    {
                        uname = uname.PadRight(10, ' ');
                    }
                    for (int j = 0; j < i_pass.Length; j++)
                    {
                        //o_pass = o_pass + Chr(Asc(Substr(i_pass, j, 1)) - Asc(Substr(uname, j, 1)));
                        o_pass = o_pass + Convert.ToChar((int)Convert.ToChar(i_pass.Substring(j, 1)) - (int)Convert.ToChar(uname.Substring(j, 1)));

                    }
                    if (o_pass.Trim() != this.txtOldPass.Text.Trim())
                    {
                        MessageBox.Show("Incorrect password! ");
                        cValid = false;
                        this.txtOldPass.Focus();
                        return;
                    }
                }

            }
            if (this.txtNewpass.Text.ToString().Trim() == "")
            {
                MessageBox.Show("New password cannot be empty");
                this.txtNewpass.Focus();
                cValid = false;
                return;
            }
        }
        
        public string padLRStr(string vMainStr, string vStrAddStr, int len)
        {
            while (vMainStr.Length <= len)
            {
                vMainStr = vMainStr.Trim() + vStrAddStr.Trim();
            }
            vMainStr = vMainStr.Substring(0, len - vStrAddStr.Length) + vStrAddStr;

            return vMainStr;
        }
        
        private void tabPage2_Enter(object sender, EventArgs e)
        {
            oDataAccess = new DataAccess_Net.clsDataAccess();
            //this.Text = this.pFrmCaption = "Role Permissions - " + this.vRole;
            lstAvailcompany.Items.Clear();


            //sqlstr = "select user_roles,isnull(company,cast('' as varbinary(250))) as company from  vudyog..userroles where user_roles='" + this.vRole + "'";

            //// sqlstr = "select user_roles,cast(isnull(company,cast('' as varbinary)) as varchar) as company from  vudyog..userroles where user_roles='" + this.vRole + "'";
            //DataTable dtuser1 = oDataAccess.GetDataTable(sqlstr, null, 30);

            ////DataRow dr1 = dtuser1.Rows[0];
            //byte[] s = (byte[])(dtuser1.Rows[0]["company"]);

            //string s1 = "";

            
            //if (s.Length > 0)
            //{
            //    s1 = padLRStr(this.vRole, this.vRole, s.Length);


            //    byte[] asciiBytes1 = Encoding.ASCII.GetBytes(s1);

            //    for (int k = 0; k < s.Length; k++)
            //    {
            //        //ins = ins + (char)((s[k]) - asciiBytes1[k]);
            //        ins = ins + Convert.ToChar((s[k]) - asciiBytes1[k]);

            //    }
            //}

            sqlstr = "select co_name,cast(year(sta_dt) as varchar(05))+'-'+ cast(year(end_dt) as varchar(05)) as co_year from vudyog..co_mast";
            dtcomp = oDataAccess.GetDataTable(sqlstr, null, 30);

            foreach (DataRow dr in dtcomp.Rows)
            {
                //var item1 = new ListViewItem(new[] { " ", dr["co_name"].ToString(), dr["co_year"].ToString() });
                //lstAvailcompany.Items.Add(item1);

                if (ins.Contains(dr["co_name"].ToString().Trim() + "(" + dr["co_year"].ToString().Trim() + ")"))
                {
                    var item1 = new ListViewItem(new[] { " ", dr["co_name"].ToString(), dr["co_year"].ToString() });
                    item1.ImageIndex = 1;
                    lstAccecompany.Items.Add(item1);
                }
                else
                {
                    var item1 = new ListViewItem(new[] { " ", dr["co_name"].ToString(), dr["co_year"].ToString() });
                    item1.ImageIndex = 0;
                    lstAvailcompany.Items.Add(item1);

                }

            }
        }

        private void tabPage3_Enter(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();

            sqlstr = "select co_name,cast(year(sta_dt) as varchar(05))+'-'+ cast(year(end_dt) as varchar(05)) as co_year from vudyog..co_mast";
            dtcomp = oDataAccess.GetDataTable(sqlstr, null, 30);

            foreach (DataRow dr in dtcomp.Rows)
            {
                if (ins.Contains(dr["co_name"].ToString().Trim() + "(" + dr["co_year"].ToString().Trim() + ")"))
                {
                    comboBox2.Items.Add(dr["co_name"].ToString().PadRight(60-dr["co_name"].ToString().Trim().Length)+ dr["co_year"].ToString().Trim());
                }            
            }
        }

        private void comboBox2_DisplayMemberChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            this.txtCompany.Text = comboBox2.SelectedItem.ToString().Substring(0, comboBox2.SelectedItem.ToString().Length - 9).Trim();
            this.txtcoyear.Text = comboBox2.SelectedItem.ToString().Substring(comboBox2.SelectedItem.ToString().Length - 9, 9);

        }       
              
    }
}
