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
    public partial class frmroleaccess : uBaseForm.FrmBaseForm 
    {
        uBaseForm.FrmBaseForm vParentForm;
        string vRole,vMenu, vParaList = string.Empty,sqlstr;
        DataAccess_Net.clsDataAccess oDataAccess;
        bool cValid;
        DataTable dtuserrole;
        string[] args1;

        public frmroleaccess()
        {
            InitializeComponent();
            //this.pAppUerName = s;

        }

        //public frmroleaccess(string[] args)
        //        {
        //            args1 = args;

        //    this.pDisableCloseBtn = true;  /*close disable*/
        //    InitializeComponent();
        //    this.pFrmCaption = "User Security";
        //    this.pPara = args;
        //    this.pCompId = Convert.ToInt16(args[0]);
        //    this.pComDbnm = args[1];
        //    this.pServerName = args[2];
        //    this.pUserId = args[3];
        //    this.pPassword = args[4];
        //    this.pPApplRange = args[5];
        //    this.pAppUerName = args[6];
        //    //Icon MainIcon = new System.Drawing.Icon(args[7].Replace("<*#*>", " "));
        //    //this.pFrmIcon = MainIcon;
        //    this.pPApplText = args[8].Replace("<*#*>", " ");
        //    this.pPApplName = args[9];
        //    this.pPApplPID = Convert.ToInt16(args[10]);
        //    this.pPApplCode = args[11];
        //}

        private void frmroleaccess_Load(object sender, EventArgs e)
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

            if (this.pMenu.Trim() == "Copy Role")
            {
                this.Text= this.pFrmCaption = "Role Properties - Copying Role " + this.vRole;
            }
            else
            {
                this.Text= this.pFrmCaption = "Role Properties - " + this.vRole;
            }
           // this.Text = this.pFrmCaption = "Role Properties - " + this.vRole;
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
            txtRole.Focus();

            if (this.vMenu == "Properties")
            {
                txtRole.Text = vRole.Trim();
                txtRole.Enabled = false;
                btnPermission.Enabled = true;
            }
            else
            {
                txtRole.Enabled = true; 
                btnPermission.Enabled = false;
            }
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

        public uBaseForm.FrmBaseForm pParentForm
        {
            get { return vParentForm; }
            set { vParentForm = value; }
        }

        private void btnPermission_Click(object sender, EventArgs e)
        {
            //uBaseForm.FrmBaseForm oForm = new frmrolepermission(args1);
            uBaseForm.FrmBaseForm oForm = new frmrolepermission();
            Type tfrm = oForm.GetType();
            tfrm.GetProperty("sRole").SetValue(oForm, this.vRole, null);
            oForm.ShowDialog();

        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            cValid = true;
            string strcomp;
            mthValidation();

            if (cValid == false)
            {
                return;
            }

            if (this.vMenu.Trim()== "New Role")
            {
                strcomp=string.Empty;
                sqlstr = " insert into vudyog..userroles (user_roles,company) values ('" + this.txtRole.Text.Trim() + "',cast('" + strcomp.Trim() + "' as varbinary(250))) ";
                oDataAccess.ExecuteSQLStatement(sqlstr, null, 30, true);

                timer1.Enabled = true;
                timer1.Interval = 1000;
                MessageBox.Show("Successfully Updated!!", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                this.Close();
                return;
            }
            else if (this.vMenu.Trim() == "Copy Role")
            {
                sqlstr = "select * from  vudyog..[userroles] where [user_roles]='" + this.vRole.Trim() + "'";
                dtuserrole = oDataAccess.GetDataTable(sqlstr, null, 30);
                if (dtuserrole.Rows.Count > 0)
                {                    
                    string myhexstr1 = BitConverter.ToString((byte[])dtuserrole.Rows[0]["company"]).Replace("-", "");
                    sqlstr = " insert into vudyog..userroles (user_roles,company) values ('" + this.txtRole.Text.Trim() + "',0x" + myhexstr1.Trim() + " ) ";
                    oDataAccess.ExecuteSQLStatement(sqlstr, null, 30, true);

                    sqlstr = "select *,dbo.func_decoder(COMPANY,'T') as mcomp from vudyog..rolesrights where dbo.func_decoder(LTRIM(RTRIM(user_roles)),'T') ='" + this.vRole.Trim() + "'";
                    DataTable dtuserrights = oDataAccess.GetDataTable(sqlstr, null, 30);

                    foreach (DataRow dr in dtuserrights.Rows)
                    {
                        sqlstr = "insert into vudyog..rolesrights (user_roles,padname,barname,rights,company,range) values ";
                        sqlstr = sqlstr + "(dbo.func_decoder('" + this.txtRole.Text.Trim() + "','F'),'" + dr["padname"].ToString().Trim() + "','";
                        sqlstr = sqlstr + dr["barname"].ToString().Trim() + "', isnull('" + dr["rights"].ToString().Trim() + "',''),";
                        sqlstr = sqlstr + "(dbo.func_decoder('" + dr["mcomp"].ToString().Trim() + "','F')),'" + dr["range"].ToString().Trim() + "')";

                        oDataAccess.ExecuteSQLStatement(sqlstr, null, 30, true);
                    }

                    timer1.Enabled = true;
                    timer1.Interval = 1000;
                    MessageBox.Show("Successfully Updated!!", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                    this.Close();
                    return;

                }
            }


        }
          private void mthValidation()
          {
              if (this.txtRole.Text.Trim() == "")
              {
                  MessageBox.Show("ROLES Cannot be empty!", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                  cValid = false;
                  return;
              }

              sqlstr = "select * from vudyog..[userroles] where [user_roles]='" + this.txtRole.Text.Trim() + "'";

              oDataAccess = new DataAccess_Net.clsDataAccess();

              DataTable dt1 = oDataAccess.GetDataTable(sqlstr, null, 30);
              if (dt1.Rows.Count > 0)
              {
                  MessageBox.Show("Role already exist!");
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
