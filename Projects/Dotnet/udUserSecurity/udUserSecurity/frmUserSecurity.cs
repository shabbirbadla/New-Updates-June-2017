using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using System.Collections;
using System.Diagnostics;

namespace udUserSecurity
{
    public partial class frmUserSecurity : uBaseForm.FrmBaseForm
    {
        DataAccess_Net.clsDataAccess oDataAccess;
        String cAppPId, cAppName;

        string SqlStr = string.Empty;
        string vRole, vMenu;
        uBaseForm.FrmBaseForm vParentForm;
        bool cValid = true;
       

        public frmUserSecurity(string[] args)
         {
            this.pDisableCloseBtn = true;  /*close disable*/
            InitializeComponent();
            this.pFrmCaption = "User Security";
            this.pPara = args;
            this.pCompId = Convert.ToInt16(args[0]);
            this.pComDbnm = args[1];
            this.pServerName = args[2];
            this.pUserId = args[3];
            this.pPassword = args[4];
            this.pPApplRange = args[5];
            this.pAppUerName = args[6];
            //Icon MainIcon = new System.Drawing.Icon(args[7].Replace("<*#*>", " "));
            //this.pFrmIcon = MainIcon;
            this.pPApplText = args[8].Replace("<*#*>", " ");
            this.pPApplName = args[9];
            this.pPApplPID = Convert.ToInt16(args[10]);
            this.pPApplCode = args[11];
        }

        private void frmUserSecurity_Load(object sender, EventArgs e)
        {
            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;

            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();


            treeView1.SelectedNode = treeView1.Nodes[0];
            //this.SetMenuRights();
           // this.btnLast_Click(sender, e);
            //this.mInsertProcessIdRecord();
            //this.SetFormColor();
            listView1.Items[0].Selected = true;

        }
        
        private void SetMenuRights() 
        {

            DataSet dsMenu = new DataSet();
            DataSet dsRights = new DataSet();
            this.pPApplRange = this.pPApplRange.Replace("^", "");
            string strSQL = "select padname,barname,range from com_menu where range =" + this.pPApplRange;
            dsMenu = oDataAccess.GetDataSet(strSQL, null, 20);
            if (dsMenu != null)
            {
                if (dsMenu.Tables[0].Rows.Count > 0)
                {
                    string padName = "";
                    string barName = "";
                    padName = dsMenu.Tables[0].Rows[0]["padname"].ToString();
                    barName = dsMenu.Tables[0].Rows[0]["barname"].ToString();
                    strSQL = "select padname,barname,dbo.func_decoder(rights,'F') as rights from ";
                    strSQL += "userrights where padname ='" + padName.Trim() + "' and barname ='" + barName + "' and range = " + this.pPApplRange;
                    strSQL += "and dbo.func_decoder([user],'T') ='" + this.pAppUerName.Trim() + "'";

                }
            }
            dsRights = oDataAccess.GetDataSet(strSQL, null, 20);


            if (dsRights != null)
            {
                string rights = dsRights.Tables[0].Rows[0]["rights"].ToString();
                int len = rights.Length;
                string newString = "";
                ArrayList rArray = new ArrayList();

                while (len > 2)
                {
                    newString = rights.Substring(0, 2);
                    rights = rights.Substring(2);
                    rArray.Add(newString);
                    len = rights.Length;
                }
                rArray.Add(rights);

                this.pAddButton = (rArray[0].ToString().Trim() == "IY" ? true : false);
                this.pEditButton = (rArray[1].ToString().Trim() == "CY" ? true : false);
                this.pDeleteButton = (rArray[2].ToString().Trim() == "DY" ? true : false);
                this.pPrintButton = (rArray[3].ToString().Trim() == "PY" ? true : false);
                ////menuItemRemoveToolbar.Enabled = (myArray[2].ToString().Trim() == "DY" ? true : false);
                //btnPrint.Enabled = (myArray[3].ToString().Trim() == "PY" ? true : false);
            }
        }

        private void SetFormColor() 
        {
            DataSet dsColor = new DataSet();
            Color myColor = Color.Coral;
            string strSQL;
            string colorCode = string.Empty;
            strSQL = "select vcolor from Vudyog..co_mast where compid =" + this.pCompId;
            dsColor = oDataAccess.GetDataSet(strSQL, null, 20);
            if (dsColor != null)
            {
                if (dsColor.Tables.Count > 0)
                {
                    dsColor.Tables[0].TableName = "ColorInfo";
                    colorCode = dsColor.Tables["ColorInfo"].Rows[0]["vcolor"].ToString();
                }
            }

            if (!string.IsNullOrEmpty(colorCode))
            {
                this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                myColor = Color.FromArgb(Convert.ToInt32(colorCode.Trim()));
            }
            this.BackColor = Color.FromArgb(myColor.R, myColor.R, myColor.G, myColor.B);

        }

        private void mInsertProcessIdRecord()
        {

            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "udUserSecurity.exe";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);

            sqlstr = "Set DateFormat dmy insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";
            oDataAccess.ExecuteSQLStatement(sqlstr, null, 20, true);
        }

        private void mDeleteProcessIdRecord()
        {
            if (string.IsNullOrEmpty(this.pPApplName) || this.pPApplPID == 0 || string.IsNullOrEmpty(this.cAppName) || string.IsNullOrEmpty(this.cAppPId))
            {
                return;
            }
            DataSet dsData = new DataSet();
            string sqlstr;
            sqlstr = " Delete from vudyog..ExtApplLog where pApplNm='" + this.pPApplName + "' and pApplId=" + this.pPApplPID + " and cApplNm= '" + cAppName + "' and cApplId= " + cAppPId;
            oDataAccess.ExecuteSQLStatement(sqlstr, null, 20, true);
        }

        private void mcheckCallingApplication()
        {
            Process pProc;
            Boolean procExists = true;
            try
            {
                pProc = Process.GetProcessById(Convert.ToInt16(this.pPApplPID));
                String pName = pProc.ProcessName;
                string pName1 = this.pPApplName.Substring(0, this.pPApplName.IndexOf("."));
                if (pName.ToUpper() != pName1.ToUpper())
                {
                    procExists = false;
                }
            }
            catch (Exception)
            {
                procExists = false;
            }
            if (procExists == false)
            {
                //MessageBox.Show("Can't proceed,Main Application " + this.pPApplText + " is closed", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //Application.Exit();
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //MessageBox.Show(treeView1.SelectedNode.Text);

            //if (treeView1.SelectedNode == treeView1.Nodes[0])
            //{
            //    //treeView2.Nodes[0].Text = "ADMINISTRATOR";
            //    //treeView2.Nodes[0].ImageIndex = 2;

            //    //treeView2.Nodes[0].Text = "ADMIN";
            //    //treeView2.Nodes[0].ImageIndex = 3;
            //    //treeView2.Nodes[0].SelectedImageIndex = 3;
            //    listView1.Items.Clear();
            //    listView1.Columns.Clear();
            //         listView1.View = View.Details;
            //         listView1.Columns.Add("Roles", -2, HorizontalAlignment.Left);

                
            //    listView1.Items.Add("ADMINISTRATOR");
            //    listView1.Items[0].ImageIndex=2;
            

            //}
            //else if (treeView1.SelectedNode == treeView1.Nodes[1])
            //{
            //    listView1.Items.Clear();
            //    listView1.Columns.Clear();
            //    listView1.View = View.Details;
            //    listView1.Columns.Add("UserId",150, HorizontalAlignment.Left);
            //    listView1.Columns.Add("Role", 150, HorizontalAlignment.Left);

            //    var item1 = new ListViewItem(new[] { "ADMIN", "ADMINISTRATOR" });
            //    listView1.Items.Add(item1);
            //    listView1.Items[0].ImageIndex = 3;
            //  //  listView1.Items.Add("ADMINISTRATOR");
            //    //treeView2.Nodes[0].Text = "ADMIN";
            //    //treeView2.Nodes[0].ImageIndex = 3;

            //    //treeView2.Nodes[0].Text = "ADMINISTRATOR";
            //    //treeView2.Nodes[0].ImageIndex = 2;
            //    //treeView2.Nodes[0].SelectedImageIndex = 2;
            //}

        }

        //private void treeView2_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        //{

        //    if (treeView2.Nodes[0].Text == "ADMINISTRATOR")
        //    {
        //        uBaseForm.FrmBaseForm oForm = new frmAdministrator();
        //        Type tfrm = oForm.GetType();
        //        oForm.ShowDialog();

        //    }
        //    else if (treeView2.Nodes[0].Text == "ADMIN")
        //    {
        //        uBaseForm.FrmBaseForm oForm = new frmAdmin();
        //        Type tfrm = oForm.GetType();


        //        oForm.ShowDialog();




        //    }

        //}

        

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            listView1.Items.Clear();

            
            
            if (treeView1.SelectedNode == treeView1.Nodes[0])
            {         
                listView1.Columns.Clear();
                listView1.View = View.Details;
                listView1.Columns.Add("Roles", -2, HorizontalAlignment.Left);

                
                string sqls1 = "select [user_roles] from vudyog..[userroles]";

                DataTable dt = oDataAccess.GetDataTable(sqls1, null, 40);
                foreach (DataRow dr in dt.Rows)
                {
                    var item1 = new ListViewItem(new[] { dr["user_roles"].ToString() });
                    
                    if (dr["user_roles"].ToString().ToUpper().Trim() == "ADMINISTRATOR")
                    {
                        item1.ImageIndex = 2;
                        item1.ForeColor = Color.Red;
                    }
                    else
                    {
                        item1.ImageIndex = 4;
                        item1.ForeColor = Color.LightSkyBlue;
                    }
                    listView1.Items.Add(item1);
                  
                }
                listView1.Items[0].Selected = true;
                this.detailsradioButton.Select();


            }
            else if (treeView1.SelectedNode == treeView1.Nodes[1])
            {
              
                listView1.Columns.Clear();
                listView1.View = View.Details;
                listView1.Columns.Add("User ID", 200, HorizontalAlignment.Left);
                listView1.Columns.Add("Role", 200, HorizontalAlignment.Left);

                string sqls1 = "select [user],user_roles from vudyog..[user]";

                DataTable dt = oDataAccess.GetDataTable(sqls1, null, 30);
                foreach(DataRow dr in dt.Rows)
                {
                    var item1 = new ListViewItem(new[] { dr["user"].ToString().ToUpper(), dr["user_roles"].ToString() });
                   

                    if (dr["user_roles"].ToString().ToUpper().Trim() == "ADMINISTRATOR")
                    {
                        item1.ImageIndex = 3;
                        item1.ForeColor = Color.Red;
                    }
                    else
                    {
                        item1.ImageIndex = 5;
                        item1.ForeColor = Color.LightSkyBlue;
                    }
                    listView1.Items.Add(item1);
                    //listView1.Items[0].ImageIndex = 3;
                }

                listView1.Items[0].Selected = true;
                this.detailsradioButton.Select();
                //var item1 = new ListViewItem(new[] { "ADMIN", "ADMINISTRATOR" });
                //listView1.Items.Add(item1);
                //listView1.Items[0].ImageIndex = 3;



                //  listView1.Items.Add("ADMINISTRATOR");
                //treeView2.Nodes[0].Text = "ADMIN";
                //treeView2.Nodes[0].ImageIndex = 3;

                //treeView2.Nodes[0].Text = "ADMINISTRATOR";
                //treeView2.Nodes[0].ImageIndex = 2;
                //treeView2.Nodes[0].SelectedImageIndex = 2;
            }
        }

        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (treeView1.SelectedNode == treeView1.Nodes[0])
                {
                    listView1.ContextMenuStrip = contextMenuStrip1;
                }
                else if (treeView1.SelectedNode == treeView1.Nodes[1])
                {
                listView1.ContextMenuStrip = contextMenuStrip2;
                }
     
            }
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {

            listView1.ContextMenuStrip = null;
        }

        private void newroleMenuItem_Click(object sender, EventArgs e)
        {

            uBaseForm.FrmBaseForm oForm = new frmroleaccess();
          //  uBaseForm.FrmBaseForm oForm = new frmroleaccess(this.pAppUerName.Trim());
            //uBaseForm.FrmBaseForm oForm = new frmroleaccess(args1);
            Type tfrm = oForm.GetType();
            tfrm.GetProperty("pMenu").SetValue(oForm, "New Role", null);
            tfrm.GetProperty("pRole").SetValue(oForm, "New Role", null);
            //tfrm.GetProperty("pRole").SetValue(oForm, this.listView1.SelectedItems[0].SubItems[1].Text.ToString().Trim(), null);
           

            tfrm.GetProperty("pPara").SetValue(oForm, this.pPara, null);
            //tfrm.GetProperty("pDataSet").SetValue(oForm, dsMain, null);
            //tfrm.GetProperty("pMenu").SetValue(oForm, this.propMenuItem.Text.ToString().Trim(), null);

            tfrm.GetProperty("pParentForm").SetValue(oForm, this, null);
            oForm.ShowDialog();
            treeView1.SelectedNode = treeView1.Nodes[1];
            treeView1.SelectedNode = treeView1.Nodes[0];

        }

        private void deleteroleMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                listView1.Items[0].Selected = true;
            }

            SqlStr="select * from vudyog..[user] where user_roles='"+this.listView1.SelectedItems[0].Text.ToString().Trim()+"'";

            DataTable dt = oDataAccess.GetDataTable(SqlStr, null, 30);
            if (dt.Rows.Count>0)
            {
                MessageBox.Show("Role is assigned to some Users!!!" + Environment.NewLine + "Cannot proceed without deleteting the Users with same Role.", this.pPApplText, MessageBoxButtons.OK);
                return;
            }
            else
            {
                if (MessageBox.Show("Are you sure you wish to delete this Role ?", this.pPApplText,
     MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    oDataAccess.BeginTransaction();
                    try
                    {
                        SqlStr = "delete FROM vudyog..rolesrights where dbo.func_decoder(LTRIM(RTRIM(user_roles)),'T') = '" + this.listView1.SelectedItems[0].Text.ToString().Trim() + "'";
                        oDataAccess.ExecuteSQLStatement(SqlStr, null, 30, true);

                        SqlStr = "delete from vudyog..[userroles] where [user_roles]='" + this.listView1.SelectedItems[0].Text.ToString().Trim() + "'";
                        oDataAccess.ExecuteSQLStatement(SqlStr, null, 30, true);

                        SqlStr = "delete FROM " + pComDbnm + "..userrights where dbo.func_decoder(LTRIM(RTRIM(user_roles)),'T') = '" + this.listView1.SelectedItems[0].Text.ToString().Trim() + "'";
                        oDataAccess.ExecuteSQLStatement(SqlStr, null, 30, true);
                        
                        oDataAccess.CommitTransaction();

                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message, this.pPApplName);
                        oDataAccess.RollbackTransaction();
                        return;
                    }
                        timer1.Enabled = true;
                        timer1.Interval = 1000;
                        MessageBox.Show("Successfully Updated!!", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);

                        treeView1.SelectedNode = treeView1.Nodes[1];
                        treeView1.SelectedNode = treeView1.Nodes[0];
                    

                }
                else
                {
                    return;
                }

            }
        }

        private void copyroleMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                listView1.Items[0].Selected = true;
            }
            if (this.pAppUerName.ToUpper().Trim() != "ADMIN")
            {
                MessageBox.Show("Cannot Proceed!!!" + Environment.NewLine + "Only ADMIN user can Copy.");
                return;
            }
            else
            {
               // uBaseForm.FrmBaseForm oForm = new frmroleaccess(this.pAppUerName.Trim());
              //  uBaseForm.FrmBaseForm oForm = new frmroleaccess(args1);
                uBaseForm.FrmBaseForm oForm = new frmroleaccess();
                Type tfrm = oForm.GetType();
                tfrm.GetProperty("pRole").SetValue(oForm, this.listView1.SelectedItems[0].Text.ToString().Trim(), null);
                //tfrm.GetProperty("pRole").SetValue(oForm, this.listView1.SelectedItems[0].SubItems[1].Text.ToString().Trim(), null);
                tfrm.GetProperty("pMenu").SetValue(oForm, "Copy Role", null);
                oForm.ShowDialog();
                treeView1.SelectedNode = treeView1.Nodes[1];
                treeView1.SelectedNode = treeView1.Nodes[0];
            }

        }

        private void largeMenuItem_Click(object sender, EventArgs e)
        {
            listView1.View = View.LargeIcon;

        }

        private void smallMenuItem_Click(object sender, EventArgs e)
        {
            listView1.View = View.SmallIcon;
        }

        private void listMenuItem_Click(object sender, EventArgs e)
        {
            listView1.View = View.List;
        }

        private void detailsMenuItem_Click(object sender, EventArgs e)
        {
            listView1.View = View.Details;
        }

        private void refreshMenuItem_Click(object sender, EventArgs e)
        {
            listView1.Refresh();
        }

        private void prop1MenuItem_Click(object sender, EventArgs e)
        {
            //uBaseForm.FrmBaseForm oForm = new frmroleaccess(this.pAppUerName.Trim());

           // uBaseForm.FrmBaseForm oForm = new frmroleaccess(args1);
            uBaseForm.FrmBaseForm oForm = new frmroleaccess();
            Type tfrm = oForm.GetType();
            tfrm.GetProperty("pRole").SetValue(oForm, this.listView1.SelectedItems[0].Text.ToString().Trim(), null);
            tfrm.GetProperty("pMenu").SetValue(oForm, this.prop1MenuItem.Text.ToString().Trim(), null);
            oForm.ShowDialog();
        }

        private void largeradioButton_CheckedChanged(object sender, EventArgs e)
        {
            listView1.View = View.LargeIcon;
        }

        private void smallradioButton_CheckedChanged(object sender, EventArgs e)
        {
            listView1.View = View.SmallIcon;
        }
        private void listradioButton_CheckedChanged(object sender, EventArgs e)
        {
            listView1.View = View.List;
        }

        private void detailsradioButton_CheckedChanged(object sender, EventArgs e)
        {
            listView1.View = View.Details;
        }
        private void newuserMenuItem_Click(object sender, EventArgs e)
        {
            if (this.pAppUerName.ToUpper().Trim() != "ADMIN")
            {
                MessageBox.Show("Cannot Proceed!!!" + Environment.NewLine + "Only ADMIN user can Create.");
                return;
            }

             uBaseForm.FrmBaseForm oForm = new frmuseraccess();
            Type tfrm = oForm.GetType();
            tfrm.GetProperty("pUser").SetValue(oForm, this.listView1.SelectedItems[0].Text.ToString().Trim(), null);
            tfrm.GetProperty("pRole").SetValue(oForm, this.listView1.SelectedItems[0].SubItems[1].Text.ToString().Trim(), null);
            tfrm.GetProperty("pMenu").SetValue(oForm, "New User", null);
            oForm.ShowDialog();
            treeView1.SelectedNode = treeView1.Nodes[0];
            treeView1.SelectedNode = treeView1.Nodes[1];

        }

        private void deleteuserMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                listView1.Items[0].Selected = true;
            }
            if (this.pAppUerName.ToUpper().Trim() != "ADMIN")
            {
                MessageBox.Show("Cannot Proceed!!!" + Environment.NewLine + "Only ADMIN user can Delete.");
                return;
            }
            if (this.listView1.SelectedItems[0].Text.ToString().ToUpper().Trim() == this.pAppUerName.ToUpper().Trim())
            {
                MessageBox.Show("Cannot proceed!!!" + Environment.NewLine + "You have Logged in with same user.", this.pPApplText, MessageBoxButtons.OK);
                return;
            }
            else
            {
                if (MessageBox.Show("Are you sure you wish to delete this User ?", this.pPApplText,
     MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    oDataAccess.BeginTransaction();
                    try
                    {
                        SqlStr = "delete FROM " + pComDbnm + "..userrights where dbo.func_decoder(LTRIM(RTRIM(user)),'T') = '" + this.listView1.SelectedItems[0].Text.ToString().Trim() + "'";
                        oDataAccess.ExecuteSQLStatement(SqlStr, null, 30, true);

                        SqlStr = "delete from vudyog..[user] where [user]='" + this.listView1.SelectedItems[0].Text.ToString().Trim() + "'";
                        oDataAccess.ExecuteSQLStatement(SqlStr, null, 30, true);
                        oDataAccess.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        oDataAccess.RollbackTransaction();
                        return;
                    }
                    timer1.Enabled = true; 
                    timer1.Interval = 1000;
                    MessageBox.Show("Successfully Updated!!", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                    treeView1.SelectedNode = treeView1.Nodes[0];
                    treeView1.SelectedNode = treeView1.Nodes[1];


                }
                else
                {
                    return;
                }
                
            }
        }

        private void copyuserMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                listView1.Items[0].Selected = true;
            }
            if (this.pAppUerName.ToUpper().Trim() != "ADMIN")
            {
                MessageBox.Show("Cannot Proceed!!!" + Environment.NewLine + "Only ADMIN user can Copy.");
                return;
            }
            else
            {
                uBaseForm.FrmBaseForm oForm = new frmcopyusers();
                Type tfrm = oForm.GetType();
                tfrm.GetProperty("pUser").SetValue(oForm, this.listView1.SelectedItems[0].Text.ToString().Trim(), null);
                tfrm.GetProperty("pRole").SetValue(oForm, this.listView1.SelectedItems[0].SubItems[1].Text.ToString().Trim(), null);
                //tfrm.GetProperty("pMenu").SetValue(oForm, this.propMenuItem.Text.ToString().Trim(), null);
                oForm.ShowDialog();
                treeView1.SelectedNode = treeView1.Nodes[0];
                treeView1.SelectedNode = treeView1.Nodes[1];
            }



        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            listView1.Refresh();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            SendKeys.Send("{ENTER}");
            timer1.Enabled = false;
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

        private void prop2MenuItem_Click(object sender, EventArgs e)
        {
            uBaseForm.FrmBaseForm oForm = new frmuseraccess();
            Type tfrm = oForm.GetType();
            tfrm.GetProperty("pUser").SetValue(oForm, this.listView1.SelectedItems[0].Text.ToString().Trim(), null);
            tfrm.GetProperty("pRole").SetValue(oForm, this.listView1.SelectedItems[0].SubItems[1].Text.ToString().Trim(), null);
            tfrm.GetProperty("pMenu").SetValue(oForm, "Properties", null);
            oForm.ShowDialog();
            treeView1.SelectedNode = treeView1.Nodes[0];
            treeView1.SelectedNode = treeView1.Nodes[1];

        }

        

      

        

        

       

      

       

        
    }
}
