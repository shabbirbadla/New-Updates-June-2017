using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using uBaseForm;
using System.Diagnostics;
using System.Collections;
using DataAccess_Net;
using System.Threading;
using System.Reflection;
using System.Globalization;
using System.Web;
using vunettofx;

namespace udAccountsTreeView
{
    public partial class FrmAccountsTreeView : uBaseForm.FrmBaseForm
    {
        DataAccess_Net.clsDataAccess oDataAccess;
        string SqlStr = "";
        DataTable GrpTbl1, GrpTbl;
        short vTimeOut = 30;
        String cAppPId, cAppName;
        ContextMenuStrip docMenu;
        string vIconPath = "";
        DataTable tblCompany ;
        string appPath;
        public FrmAccountsTreeView(string[] args)
        {
            InitializeComponent();
            this.pPara = args;
            this.pCompId = Convert.ToInt16(args[0]);
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
            this.pFrmCaption = "Chart Of Accounts";
            vIconPath = args[7].Replace("<*#*>", " ");
        }

        private void FrmAccountsTreeView_Load(object sender, EventArgs e)
        {
            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;

            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();

            this.SetFormColor();
            appPath = Application.ExecutablePath;

            appPath = Path.GetDirectoryName(appPath);
            if (string.IsNullOrEmpty(this.pAppPath))
            {
                this.pAppPath = appPath;
            }
            string fName = appPath + @"\bmp\loc-on.gif";
            if (File.Exists(fName) == true)
            {
                this.btnGroup.Image = Image.FromFile(fName);
                this.btnAcNm.Image = Image.FromFile(fName);
            }
            this.mInsertProcessIdRecord();
            this.mthCommon();
            this.mthRefreshTreeView();

        }
        private void mthRefreshTreeView()
        {
            try
            {
                Tvw.Nodes.Clear();
                GrpTbl = new DataTable();
                GrpTbl1 = new DataTable();


                SqlStr = "Select Ac_Group_id,Ac_Group_Name,GAc_Id,[Group],Ledger=cast(0 as Bit),[Type] From Ac_group_Mast";
                SqlStr = SqlStr + " Order by Ac_Group_Name ";
                GrpTbl = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);

                SqlStr = "Select Ac_Group_id,Ac_Group_Name,GAc_Id,[Group],[Type] From Ac_group_Mast Where ac_group_id=Gac_Id ";
                if (this.txtGrp.Text.Trim() != "")
                {
                    SqlStr = "Select Ac_Group_id,Ac_Group_Name,GAc_Id,[Group],[Type] From Ac_group_Mast Where Ac_Group_Name='" + this.txtGrp.Text.Trim() + "'";
                }
               
                SqlStr = SqlStr + " Order by Ac_Group_Name ";
                GrpTbl1 = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);

                string sAcGroupid = "";
                string NodeText = "";
                for (int i = 0; i <= GrpTbl1.Rows.Count - 1; i++)
                {
                    DataRow dr = GrpTbl1.Rows[i];

                    TreeNode tNode = new TreeNode();

                    sAcGroupid = dr["Ac_Group_id"].ToString().Trim();
                    tNode = Tvw.Nodes.Add(sAcGroupid);
                    tNode.Tag = "#GRP##Type=" + dr["Type"].ToString().Trim()+"#";
                    NodeText = dr["Ac_Group_Name"].ToString().Trim();
                    tNode.Text = NodeText;
                    //tNode.Tag = dr["Group"].ToString().Trim();
                    tNode.Name = sAcGroupid;
                    this.mthCheckChildMenu(sAcGroupid, tNode, NodeText);
                

                }//for (int j = 0; j<=dsmain.Tables["com_menu"].Rows.Count - 1; j++)

                this.sblChild.Text = "";
                this.stbMain.Refresh();

            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
            finally { }
        }

        private void InitializeMenuTreeView()
        {
            //docMenu = new ContextMenuStrip();
            //ToolStripMenuItem openLabel = new ToolStripMenuItem();
            //openLabel.Text = "Open";
            //ToolStripMenuItem deleteLabel = new ToolStripMenuItem();
            //deleteLabel.Text = "Delete";
            //ToolStripMenuItem renameLabel = new ToolStripMenuItem();
            //renameLabel.Text = "Rename";

            //docMenu.Items.AddRange(new ToolStripMenuItem[]{openLabel, deleteLabel, renameLabel});
            //docNode.ContextMenuStrip = docMenu;
    
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                this.mthRefreshTreeView();
                if (chkLedger.Checked)
                {
                    foreach (TreeNode vNode in this.Tvw.Nodes)
                    {
                        DataTable tblMailNm = new DataTable();
                        SqlStr = "Select MailName=Ac_Name,Ac_Id,[Type] From Ac_Mast Where  isnull(Ac_Group_Id,0)<>0  and Ac_group_Id=" + vNode.Name;
                        if (this.chlDeAct.Checked == false)
                        {
                            SqlStr = SqlStr + " and lDeActive=0";
                        }
                        SqlStr = SqlStr + " Order By Ac_Name";

                        tblMailNm = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
                        foreach (DataRow dr in tblMailNm.Rows)
                        {

                            TreeNode tSubNode = new TreeNode();
                            tSubNode = vNode.Nodes.Add("Led_" + dr["Ac_Id"].ToString().Trim());
                            tSubNode.Tag = "#LED##Type=" + dr["Type"].ToString().Trim()+"#";
                            tSubNode.ForeColor = Color.Blue;
                            tSubNode.Text = dr["MailName"].ToString().Trim();
                            tSubNode.Name = "Led_" + dr["Ac_Id"].ToString().Trim();
                            tSubNode.ImageIndex = 2;



                            this.sblChild.Text = "Generating Record:-" + tSubNode.Text;
                            this.stbMain.Refresh();

                        }

                        this.mthAddLedgers(vNode);
                    }
                }
                Tvw.Visible = true;
                Tvw.Focus(); //Birendra : Bug-21095 on 17/12/2013

            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
            finally 
            {
                this.sblChild.Text = "";
                this.stbMain.Refresh();
            }
        }

        private void mthCheckChildMenu(string vAcGroupid, TreeNode mtNode, string vNodeText)
        {
   
            string subAcGroupid;
            string tNodeText = "";
            for (int j = 0; j <= GrpTbl.Rows.Count - 1; j++)
            {
                try
                {
                    DataRow dr = GrpTbl.Rows[j];
                    tNodeText = dr["Ac_Group_Name"].ToString().Trim();
                    if ((dr["GAc_Id"].ToString().Trim() == vAcGroupid.Trim()) && (vNodeText != tNodeText))
                    {
                        TreeNode tSubNode = new TreeNode();
                        subAcGroupid = dr["Ac_Group_id"].ToString().Trim();
                        tSubNode = mtNode.Nodes.Add(subAcGroupid);
                        tSubNode.Tag = "#GRP##Type=" + dr["Type"].ToString().Trim() + "#";
                        tSubNode.Text = tNodeText;
                        tSubNode.ImageIndex = 1;
                        tSubNode.Name = subAcGroupid;
                      

                        this.sblChild.Text = "Generating Record:-" + tSubNode.Text;
                        this.stbMain.Refresh();

                        this.mthCheckChildMenu(subAcGroupid, tSubNode, tNodeText);
                    }
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
                finally { }
            }
        }

        private void btnAcNm_Click(object sender, EventArgs e)
        {
            DataTable tblAcPop = new DataTable();
            Boolean vNodeFound = false;
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            SqlStr = "Select ac_name ,(case when ISNULL(MailName,'')='' then ac_name else mailname end) as mailname From Ac_Mast";
            SqlStr = SqlStr + " union ";
            SqlStr = SqlStr + " Select ac_group_name ,ac_group_name as mailname From Ac_Group_Mast";
            SqlStr = SqlStr + " Order By ac_name";
            tblAcPop = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
            DataView dvw = new DataView();
            dvw = tblAcPop.DefaultView;

            VForText = "Select Description";
            vSearchCol = "ac_name";
            vDisplayColumnList = "ac_name:Description,mailname:Mail Name";
            vReturnCol = "ac_name,mailname";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtAcNm.Text = oSelectPop.pReturnArray[1].Trim();
                vNodeFound = false;

                foreach (TreeNode vNode in this.Tvw.Nodes)
                {
                    mthSearch(vNode, vNodeFound);
                }
            }
        }

        private void mthSearch(TreeNode vNode, Boolean NodeFound)
        {

            foreach (TreeNode t1Node in vNode.Nodes)
            {
                if ((t1Node.Text.Trim() == this.txtAcNm.Text.Trim()))
                {
                    t1Node.Parent.Expand();
                    Tvw.SelectedNode = t1Node;
                    t1Node.BackColor = Color.Yellow;
                    NodeFound = true;
                }
                else
                {
                    t1Node.BackColor = Color.White;
                    mthSearch(t1Node, NodeFound);
                }

            }
            
        }


        private void mthAddLedgers(TreeNode vNode)
        {
            try{
                if (vNode.Nodes.Count > 0)
                {
                    foreach (TreeNode ttNode in vNode.Nodes)
                    {
                        if (ttNode.Name.IndexOf("Led_") == -1)
                        {
                            DataTable ttblMailNm = new DataTable();
                            //SqlStr = "Select MailName=(case when isnull(MailName,'')='' Then Ac_Name Else MailName End),Ac_Id,[Group] From Ac_Mast Where isnull(Ac_Group_Id,0)<>0  and Ac_group_Id=" + ttNode.Name + " Order By (case when isnull(MailName,'')='' Then Ac_Name Else MailName End)  ";
                            SqlStr = "Select MailName=Ac_Name,Ac_Id,[Type] From Ac_Mast Where  isnull(Ac_Group_Id,0)<>0  and Ac_group_Id=" + ttNode.Name;
                            if (this.chlDeAct.Checked == false)
                            {
                                SqlStr = SqlStr + " and lDeActive=0";
                            }
                            SqlStr = SqlStr + " Order By Ac_Name";

                            ttblMailNm = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
                            foreach (DataRow dr in ttblMailNm.Rows)
                            {
                               
                                TreeNode tSubNode = new TreeNode();
                                tSubNode = ttNode.Nodes.Add("Led_" + dr["Ac_Id"].ToString().Trim());
                                tSubNode.Tag = "#LED##Type=" + dr["Type"].ToString().Trim() + "#";
                                tSubNode.ForeColor = Color.Blue;
                                tSubNode.Text = dr["MailName"].ToString().Trim();
                                tSubNode.Name = "Led_" + dr["Ac_Id"].ToString().Trim();
                                tSubNode.ImageIndex = 2;
                                this.sblChild.Text = "Generating Record:-" + tSubNode.Text;
                                this.stbMain.Refresh();
                            }
                        }
                        this.mthAddLedgers(ttNode);
                    }
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
            finally { }
        }
        //private void mthRefreshLable(string str1,string str2)
        //{
        //    this.sblChild.Text = "Generating Record:-" + str2;
        //    this.stbMain.Refresh();
        //}
        
        #region Security Checking and Common Methods
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
            //foreach (Control ctrl in this.Controls)
            //{
            //    ctrl.BackColor = Color.FromArgb(myColor.R, myColor.R, myColor.G, myColor.B);
            //}


        }

        private void mInsertProcessIdRecord()
        {
            try
            {
                DataSet dsData = new DataSet();
                string sqlstr;
                int pi;
                pi = Process.GetCurrentProcess().Id;
                cAppName = "udAccountsTreeView.exe";
                cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
                sqlstr = " Set DateFormat dmy insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";
                oDataAccess.ExecuteSQLStatement(sqlstr, null, 20, true);
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
            finally { }
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
                MessageBox.Show("Can't proceed,Main Application " + this.pPApplText + " is closed", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Application.Exit();
            }
        }
        private void mthCommon()
        {
            tblCompany = new DataTable();
            SqlStr = "Select * From vudyog..Co_Mast where CompId="+this.pCompId;
            tblCompany = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
        }

        private void FrmAccountsTreeView_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.mDeleteProcessIdRecord();
        }
        #endregion

        private void btnGroup_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataTable tblAcPop = new DataTable();
            SqlStr = "Select Ac_Group_Name From Ac_group_Mast union Select ''";
            SqlStr = SqlStr + " Order by Ac_Group_Name ";
            tblAcPop = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);

            DataView dvw = new DataView();
            dvw = tblAcPop.DefaultView;

            VForText = "Select Group";
            vSearchCol = "Ac_Group_Name";
            vDisplayColumnList = "Ac_Group_Name:Group Name";
            vReturnCol = "Ac_Group_Name";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtGrp.Text = oSelectPop.pReturnArray[0].Trim();
            }
        }

        private void Tvw_DoubleClick(object sender, EventArgs e)
        {
           
            //MessageBox.Show(Tvw.SelectedNode.Name );
            if (Tvw.SelectedNode.Tag.ToString().Trim() == "")
            {
                return;
            }
            //MessageBox.Show(Tvw.SelectedNode.Tag.ToString());
            this.pAppPath = Application.StartupPath + @"\";
            vunettofx.ClNetToFx oVfpCls = new vunettofx.ClNetToFx();
            
            string pPara = "DO UEACMAST WITH ";
            if (Tvw.SelectedNode.Tag.ToString().IndexOf("#GRP#") > -1)
            {

                pPara = pPara + @"""";
                pPara = pPara + @"GROUP";
                pPara = pPara + @"""";
                pPara = pPara + "," + @"""";
                pPara = pPara + "#pid="+Tvw.SelectedNode.Name.Trim()+"#";
                pPara = pPara + @"""";
                SqlStr = "Select [Range] From Com_Menu Where PadName='MAINMASTERS' and BarName='ACCOUNTGROUPMASTER'";
            }
            else if (Tvw.SelectedNode.Tag.ToString().IndexOf("#Type=T") > -1 || Tvw.SelectedNode.Tag.ToString().IndexOf("#Type=P") > -1)
            {
                pPara = pPara + @"""";
                pPara = pPara + @"GL";
                pPara = pPara + @"""";
                pPara = pPara + "," + @"""";
                pPara = pPara + @"'T','P'";
                pPara = pPara + "#pid=" + Tvw.SelectedNode.Name.Trim().Replace("Led_","") + "#";
                pPara = pPara + @"""";
                SqlStr = "Select [Range] From Com_Menu Where PadName='MAINMASTERS' and BarName='GENERALLEDGERMASTER'";
            }
            else if (Tvw.SelectedNode.Tag.ToString().IndexOf("#Type=B") > -1 )
            {
                pPara = pPara + @"""";
                pPara = pPara + @"";
                pPara = pPara + @"""";
                pPara = pPara + "," + @"""";
                pPara = pPara + @"'B'";
                pPara = pPara + "#pid=" + Tvw.SelectedNode.Name.Trim().Replace("Led_", "") + "#";
                pPara = pPara + @"""";
                SqlStr = "Select [Range] From Com_Menu Where PadName='MAINMASTERS' and BarName='ACCOUNTMASTER'";
            }
            DataTable tComMenu = new DataTable();
            tComMenu = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
            if (tComMenu.Rows.Count > 0)
            {
                pPara = pPara + "," + @"""";
                pPara = pPara +"^"+ tComMenu.Rows[0]["Range"].ToString() + "";
                pPara = pPara + @"""";
               
            }
            String  sDate, eDate;
            //sDate = ((DateTime)tblCompany.Rows[0]["sta_dt"]).Year.ToString()+"/"+((DateTime)tblCompany.Rows[0]["sta_dt"]).Month.ToString()+"/"+((DateTime)tblCompany.Rows[0]["sta_dt"]).Day.ToString();
            //eDate = ((DateTime)tblCompany.Rows[0]["end_dt"]).Year.ToString() + "/" + ((DateTime)tblCompany.Rows[0]["end_dt"]).Month.ToString() + "/" + ((DateTime)tblCompany.Rows[0]["end_dt"]).Day.ToString();
            sDate = ((DateTime)tblCompany.Rows[0]["sta_dt"]).Day.ToString() + "/" + ((DateTime)tblCompany.Rows[0]["sta_dt"]).Month.ToString() + "/" + ((DateTime)tblCompany.Rows[0]["sta_dt"]).Year.ToString();
            eDate = ((DateTime)tblCompany.Rows[0]["end_dt"]).Day.ToString() + "/" + ((DateTime)tblCompany.Rows[0]["end_dt"]).Month.ToString() + "/" + ((DateTime)tblCompany.Rows[0]["end_dt"]).Year.ToString();
            oVfpCls.PrNetToFx(this.pComDbnm, sDate, eDate, this.pAppUerName, this.pAppPath, this.pAppPath, vIconPath, "", pPara);
        }

        private void button1_Click111(object sender, EventArgs e)
        {
            this.pAppPath = Application.StartupPath +@"\";
            vunettofx.ClNetToFx oVfpCls = new vunettofx.ClNetToFx();
            
            string pPara="DO UEACMAST WITH ";
            
            pPara = pPara + @"""";
            pPara = pPara + @"""";
            pPara=pPara+","+@"""";
            pPara=pPara+@"'";
            pPara=pPara+@"B'";
            pPara = pPara +  @"""";
            pPara = pPara + "," + @"""";
            pPara=pPara+"^13007";
            pPara = pPara + @"""";
            MessageBox.Show(this.pAppPath);
            oVfpCls.PrNetToFx(this.pComDbnm, "2012-04-01", "2013-03-31", this.pAppUerName, this.pAppPath, this.pAppPath, vIconPath, "", pPara);
            //this.pPara = args;
            //this.pCompId = Convert.ToInt16(args[0]);
            //this.pComDbnm = args[1];
            //this.pServerName = args[2];
            //this.pUserId = args[3];
            //this.pPassword = args[4];
            //this.pPApplRange = args[5];
            //this.pAppUerName = args[6];
            //Icon MainIcon = new System.Drawing.Icon(args[7].R                                 eplace("<*#*>", " "));
            //this.pFrmIcon = MainIcon;
            //this.pPApplText = args[8].Replace("<*#*>", " ");
            //this.pPApplName = args[9];
            //this.pPApplPID = Convert.ToInt16(args[10]);
            //this.pPApplCode = args[11];
            //this.pFrmCaption = "Chart Of Accounts";
            //http://msdn.microsoft.com/en-us/library/ms171707.aspx
        }

    }
}
