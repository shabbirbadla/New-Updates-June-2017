using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace udUserSecurity
{

    public partial class frmrolepermission : uBaseForm.FrmBaseForm
    {
        private int indexOfItemUnderMouseToDrop;
        private int indexOfItemUnderMouseToDrag;
        string vRole, vParaList = string.Empty;
        string sqlstr;
        DataTable dtcomp, dtcommenu;
        DataAccess_Net.clsDataAccess oDataAccess;
        string lctext;
        int nSelectedindex;
        string[] companyyear;
        string tot_str;
        bool IsAdded = false;
        int mrecno;
        string mlevelc, mlevels, mlevelp, mlevel;


        public frmrolepermission()
        {
            InitializeComponent();

        }

        public string sRole
        {
            get { return vRole; }
            set { vRole = value; }
        }

        private void frmrolepermission_Load(object sender, EventArgs e)
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


            oDataAccess = new DataAccess_Net.clsDataAccess();
            this.Text = this.pFrmCaption = "Role Permissions - " + this.vRole;
            lstAvailcompany.Items.Clear();
            lstAccecompany.Items.Clear();

            sqlstr = "select user_roles,isnull(company,cast('' as varbinary(250))) as company from  vudyog..userroles where user_roles='" + this.vRole + "'";

            // sqlstr = "select user_roles,cast(isnull(company,cast('' as varbinary)) as varchar) as company from  vudyog..userroles where user_roles='" + this.vRole + "'";
            DataTable dtuser1 = oDataAccess.GetDataTable(sqlstr, null, 30);

            //DataRow dr1 = dtuser1.Rows[0];
            byte[] s = (byte[])(dtuser1.Rows[0]["company"]);

            string s1 = "";

            string ins = "";
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



            sqlstr = "select co_name,cast(year(sta_dt) as varchar(05))+'-'+ cast(year(end_dt) as varchar(05)) as co_year from vudyog..co_mast";
            companyyear = ins.Split('\r');
            dtcomp = oDataAccess.GetDataTable(sqlstr, null, 30);

            foreach (DataRow dr in dtcomp.Rows)
            {

                if (ins.Contains(dr["co_name"].ToString().Trim() + "(" + dr["co_year"].ToString().Trim() + ")"))
                {
                    //var item1 = new ListViewItem(new[] { " ", dr["co_name"].ToString(), dr["co_year"].ToString() });
                    //item1.ImageIndex = 1;
                    //lstAccecompany.Items.Add(item1);


                }
                else
                {
                    var item1 = new ListViewItem(new[] { " ", dr["co_name"].ToString(), dr["co_year"].ToString() });
                    item1.ImageIndex = 0;
                    lstAvailcompany.Items.Add(item1);

                }

            }
            for (int i = 0; i < companyyear.Length; i++)
            {
                if (companyyear[i].Trim() != "")
                {
                    var item1 = new ListViewItem(new[] { " ", companyyear[i].Substring(0, companyyear[i].IndexOf('(')), companyyear[i].Substring(companyyear[i].IndexOf('(') + 1, 9) });
                    item1.ImageIndex = 1;
                    lstAccecompany.Items.Add(item1);
                }
            }






            // string s = dr1["company"].ToString();

            //string s ="0h"+ BitConverter.ToString((byte[])dtuser1.Rows[0]["company"]).Replace("-", "");

            //string s1="";
            //for (int k = 0; k < s.Length; k++)
            //{
            //    s1 += this.vRole;
            //}

            //string ins = "";

            //byte[] asciiBytes = Encoding.ASCII.GetBytes(s);

            //byte[] asciiBytes1 = Encoding.ASCII.GetBytes(s1);


            //for (int k = 0; k < s.Length; k++)
            //{

            //    ins = ins + (char)((asciiBytes[k]) - asciiBytes1[k]);
            //}



        }

        private void lstAvailcompany_ItemDrag(object sender, ItemDragEventArgs e)
        {
            //string s = e.Item.ToString();
            //DoDragDrop(s, DragDropEffects.Copy);
        }

        private void lstAccecompany_DragEnter(object sender, DragEventArgs e)
        {
            //if (e.Data.GetDataPresent(DataFormats.Text))
            //    e.Effect = DragDropEffects.Copy;
            //else
            //    e.Effect = DragDropEffects.None;
        }

        private void lstAccecompany_DragDrop(object sender, DragEventArgs e)
        {
            //string typestring = "Type";
            //string s = e.Data.GetData(typestring.GetType()).ToString();
            //string orig_string = s;
            //s = s.Substring(s.IndexOf(":") + 1).Trim();
            //s = s.Substring(1, s.Length - 2);

            //this.lstAccecompany.Items.Add(s);

            //IEnumerator enumerator = lstAvailcompany.Items.GetEnumerator();
            //int whichIdx = -1;
            //int idx = 0;
            //while (enumerator.MoveNext())
            //{
            //    string s2 = enumerator.Current.ToString();
            //    if (s2.Equals(orig_string))
            //    {
            //        whichIdx = idx;
            //        break;
            //    }
            //    idx++;
            //}
            //this.lstAvailcompany.Items.RemoveAt(whichIdx);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (lstAvailcompany.SelectedItems.Count > 0)
            {
                //lstAvailcompany.SelectedItems[0].Text= lstAccecompany.SelectedItems[0];
                //lstAvailcompany.SelectedItems[0].SubItems[1].Text=lstAccecompany.SelectedItems[0].SubItems[1].Text.ToString().Trim();
                //lstAvailcompany.SelectedItems[0].SubItems[2].Text = lstAccecompany.SelectedItems[0].SubItems[2].Text.ToString().Trim();

                var item2 = new ListViewItem(new[] { " ", lstAvailcompany.SelectedItems[0].SubItems[1].Text.ToString().Trim(), lstAvailcompany.SelectedItems[0].SubItems[2].Text.ToString().Trim() });
                item2.ImageIndex = 1;
                lstAccecompany.Items.Add(item2);
                IsAdded = true;

                lstAvailcompany.SelectedItems.Clear();

                //this.listView1.SelectedItems[0].SubItems[1].Text.ToString().Trim()
            }
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {
            if (IsAdded = true)
            {

                if (MessageBox.Show("Save the changes?", this.pPApplText,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    tot_str = "";
                    if (lstAccecompany.Items.Count > 0)
                    {
                        foreach (ListViewItem li in lstAccecompany.Items)
                        {
                            tot_str = tot_str.Trim() + li.SubItems[1].Text.Trim() + '(' + li.SubItems[2].Text.Trim() + ')' + '[' + vRole.ToUpper() + ']';
                        }
                    }
                }
                IsAdded = false;

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
            if (IsAdded == true)
            {

                if (MessageBox.Show("Save the changes?", this.pPApplText,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    tot_str = "";
                    if (lstAccecompany.Items.Count > 0)
                    {
                        foreach (ListViewItem li in lstAccecompany.Items)
                        {
                            // tot_str = tot_str.Trim() + li.SubItems[1].Text.Trim() + '(' + li.SubItems[2].Text.Trim() + ')' + '[' + vRole.ToUpper() + ']'+System.Environment.NewLine;
                            tot_str = tot_str + li.SubItems[1].Text.Trim() + '(' + li.SubItems[2].Text.Trim() + ')' + '[' + vRole.ToUpper() + ']' + "\r\n";
                        }
                        MessageBox.Show(tot_str);

                        string s = tot_str.Replace("\n", String.Empty);

                        MessageBox.Show(s);

                        sqlstr = "use vudyog select dbo. ('" + s + "','" + this.vRole + "')  as company";
                        DataTable dt = oDataAccess.GetDataTable(sqlstr, null, 20);

                        sqlstr = " Update vudyog..userroles set company=convert(varbinary(250),'" + dt.Rows[0]["company"].ToString().Trim() + "') where user_roles='" + this.vRole + "'";
                        oDataAccess.ExecuteSQLStatement(sqlstr, null, 30, true);

                        // if(com

                        #region fromdotnet

                        //if (s != string.Empty)
                        //{


                        //    string nm1 = "";

                        //    nm1 = padLRStr(this.vRole, this.vRole, s.Length);




                        //    byte[] asciiBytes2 = Encoding.ASCII.GetBytes(s);
                        //    byte[] asciiBytes3 = Encoding.ASCII.GetBytes(nm1);
                        //    int a;

                        //    string  outstr=" ";
                        //    int count = 0;
                        //    for (int k = 0; k < s.Length; k++)
                        //    {
                        //        outstr = outstr + (char)((asciiBytes2[k]) + asciiBytes3[k]);
                        //        a = Convert.ToInt16(Convert.ToChar(asciiBytes2[k] + asciiBytes3[k]));
                        //        count += 1;

                        //    }

                        //    MessageBox.Show(outstr.Trim());
                        //    //byte[] arr = System.Text.Encoding.ASCII.GetBytes(outstr);

                        //    sqlstr = " Update vudyog..userroles set company=convert(varbinary(250),'" + outstr.Trim() + "') where user_roles='" + this.vRole + "'";
                        //  //  oDataAccess.ExecuteSQLStatement(sqlstr, null, 30, true);

                        //    sqlstr = "select user_roles,isnull(company,cast('' as varbinary(250))) as company from  vudyog..userroles where user_roles='" + this.vRole + "'";

                        //    // sqlstr = "select user_roles,cast(isnull(company,cast('' as varbinary)) as varchar) as company from  vudyog..userroles where user_roles='" + this.vRole + "'";
                        //    DataTable dtuser1 = oDataAccess.GetDataTable(sqlstr, null, 30);

                        //    //DataRow dr1 = dtuser1.Rows[0];
                        //    byte[] s2 = (byte[])(dtuser1.Rows[0]["company"]);

                        //    string s1 = "";
                        //    for (int k = 0; k < s2.Length; k++)
                        //    {
                        //        s1 += this.vRole;
                        //    }
                        //    string ins = "";

                        //    byte[] asciiBytes1 = Encoding.ASCII.GetBytes(s1);

                        //    for (int k = 0; k < s2.Length; k++)
                        //    {
                        //        ins = ins + (char)((s2[k]) - asciiBytes1[k]);
                        //    }
                        //    MessageBox.Show(ins);

                        //}

                        #endregion
                    }
                }
                IsAdded = false;
            }

            if (companyyear.Length > 0)
            {
                for (int i = 0; i < companyyear.Length; i++)
                {
                    if (companyyear[i].Trim() != "")
                    {
                        // var item1 = new ListViewItem(new[] { " ", companyyear[i].Substring(0, companyyear[i].IndexOf('(')), companyyear[i].Substring(companyyear[i].IndexOf('(') + 1, 9) });
                        //item1.ImageIndex = 1;
                        //lstAccecompany.Items.Add(item1);
                        comboBox1.Items.Add(companyyear[i].Substring(0, companyyear[i].IndexOf('(')).PadRight(60 - companyyear[i].Substring(0, companyyear[i].IndexOf('(')).Trim().Length) + companyyear[i].Substring(companyyear[i].IndexOf('(') + 1, 9));

                    }
                }

                if (comboBox1.Items.Count > 0)
                {
                    this.txtcoyear.Text = comboBox1.Items[0].ToString().Substring(comboBox1.Items[0].ToString().Length - 9, 9).Trim();

                    this.txtCompany.Text = comboBox1.Items[0].ToString().Substring(0, comboBox1.Items[0].ToString().Length - 9).Trim();
                    //this.txtcoyear.Text = comboBox1.Items[0].ToString().Substring(comboBox1.Items[0].ToString().Length - 9, 9).Trim();
                }
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (IsAdded == true)
            {

                if (MessageBox.Show("Save the changes?", this.pPApplText,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    tot_str = "";
                    if (lstAccecompany.Items.Count > 0)
                    {
                        foreach (ListViewItem li in lstAccecompany.Items)
                        {
                            // tot_str = tot_str.Trim() + li.SubItems[1].Text.Trim() + '(' + li.SubItems[2].Text.Trim() + ')' + '[' + vRole.ToUpper() + ']'+System.Environment.NewLine;
                            tot_str = tot_str + li.SubItems[1].Text.Trim() + '(' + li.SubItems[2].Text.Trim() + ')' + '[' + vRole.ToUpper() + ']' + "\r\n";
                        }

                        MessageBox.Show(tot_str);

                        string s = tot_str.Replace("\n", String.Empty);

                        MessageBox.Show(s);

                        if (s != string.Empty)
                        {
                            //string nm1=vRole.ToString().rep

                            //=string.Concat(Enumerable.Repeat(vRole, tot_str.Length));

                            string nm1 = "";
                            //for (int k = 0; k < s.Length; k++)
                            //{
                            //    nm1 += this.vRole;
                            //}
                            nm1 = padLRStr(this.vRole, this.vRole, s.Length);

                            byte[] asciiBytes2 = Encoding.ASCII.GetBytes(s);
                            // byte[] asciiBytes2 = (byte[])(tot_str);
                            byte[] asciiBytes3 = Encoding.ASCII.GetBytes(nm1);


                            string outstr = " ";
                            int count = 0;
                            for (int k = 0; k < s.Length; k++)
                            {
                                outstr = outstr + Convert.ToChar((asciiBytes2[k]) + asciiBytes3[k]);
                                count += 1;

                            }

                            MessageBox.Show(outstr.Trim());
                            //byte[] arr = System.Text.Encoding.ASCII.GetBytes(outstr);

                            sqlstr = " Update vudyog..userroles set company=convert(varbinary(250),'" + outstr.Trim() + "') where user_roles='" + this.vRole + "'";
                            oDataAccess.ExecuteSQLStatement(sqlstr, null, 30, true);

                            sqlstr = "select user_roles,isnull(company,cast('' as varbinary(250))) as company from  vudyog..userroles where user_roles='" + this.vRole + "'";

                            // sqlstr = "select user_roles,cast(isnull(company,cast('' as varbinary)) as varchar) as company from  vudyog..userroles where user_roles='" + this.vRole + "'";
                            DataTable dtuser1 = oDataAccess.GetDataTable(sqlstr, null, 30);

                            //DataRow dr1 = dtuser1.Rows[0];
                            byte[] s2 = (byte[])(dtuser1.Rows[0]["company"]);

                            string s1 = "";
                            for (int k = 0; k < s2.Length; k++)
                            {
                                s1 += this.vRole;
                            }
                            string ins = "";

                            byte[] asciiBytes1 = Encoding.ASCII.GetBytes(s1);

                            for (int k = 0; k < s2.Length; k++)
                            {
                                ins = ins + Convert.ToChar((s2[k]) - asciiBytes1[k]);
                            }
                            MessageBox.Show(ins);


                            //byte[] arr = Encoding.UTF8.GetBytes(outstr);

                            //string theString = Encoding.UTF8.GetString(arr);

                        }
                    }
                }
                IsAdded = false;

            }
        }

        private void txtCompany_TextChanged(object sender, EventArgs e)
        {
            sqlstr = "select co_name,dbname from vudyog..co_mast where co_name='" + this.txtCompany.Text.Trim() + "'";
            dtcomp = oDataAccess.GetDataTable(sqlstr, null, 30);
            DataTable ds, ds1;

            // MessageBox.Show("Please wait, Generating Nodes................");

            sqlstr = "select co_name,dbname from co_mast where LTRIM(RTRIM(co_name)) = '" + this.txtCompany.Text.Trim() + "' and cast(year(sta_dt) as varchar(05))+'-'+ cast(year(end_dt) as varchar(05)) '" + this.txtcoyear.Text.Trim() + "'";

            sqlstr = "if exists(select name from master.dbo.sysdatabases where name ='" + dtcomp.Rows[0]["dbname"].ToString() + "') select 'y' as founddb else select 'N' as founddb ";
            ds = oDataAccess.GetDataTable(sqlstr, null, 30);

            string s = this.pAppUerName;

            sqlstr = "select user_roles from vudyog..[user] where [user] = '" + s + "' ";
            ds1 = oDataAccess.GetDataTable(sqlstr, null, 30);

            if (ds1.Rows[0]["user_roles"].ToString().Trim() == "ADMINISTRATOR")
            {
                treeView1.CheckBoxes = true;
            }
            else
            {
                treeView1.CheckBoxes = false;
            }

            sqlstr = "select padname,barname,range,newrange from " + dtcomp.Rows[0]["dbname"].ToString() + "..com_menu";

            //  sqlstr = " select space(250) as levelp,space(250) as levelc,space(10) as Levela,space(10) as Level1,space(10) as Level2,CAST(' ' as text) as Levelf,a.* from " + dtcomp.Rows[0]["dbname"].ToString() + "..com_menu a order by a.padnum,a.barnum";


            sqlstr = "use " + dtcomp.Rows[0]["dbname"].ToString().Trim() + " execute usp_ent_create_nodes2";
            dtcommenu = oDataAccess.GetDataTable(sqlstr, null, 30);

            // int index = 0;

            //foreach (DataRow dr1 in dtcommenu.Rows)
            //{
            //      TreeNode n = new TreeNode();
            //     // n.Value = d["id"].ToString();
            //      n.Text = dr1["prompname"].ToString().Replace("\\<"," ").Trim();
            //      //n.Text =n.Text.Replace('<', ' '); 
            //    index=index+1;
            //    treeView1.CheckBoxes = true;
            //    if (dr1["levelp"].ToString().Trim() == "")
            //    {
            //        foreach (DataRow dr2 in dtcommenu.Rows)
            //        {
            //            if (dr1["levelc"].ToString() == dr2["levelp"].ToString())
            //            {
            //                TreeNode inner = new TreeNode();
            //                //inner.Value = r["id"].ToString();
            //                inner.Text = dr2["prompname"].ToString().Trim();
            //                //treeView1.Nodes[index].ChildNodes.Add(inner);
            //                // treeView1.Nodes[index].Nodes.Add(inner);
            //                n.Nodes.Add(inner);

            //            }

            //        }
            //        treeView1.Nodes.Add(n);

            //    }
            //    //if (dr1["levelp"].ToString().Trim() == "")
            //    //{

            //    //}


            //}

           
            TreeNode _tn;
            DataRow[] _dr = dtcommenu.Select("levelP=''");
            CreateTreeNodes(null, _dr);  /*To add the nodes*/

            DataTable dtuserrights;

            foreach (DataRow dr1 in dtcommenu.Rows) /*To give the rights to the node*/
            {
                sqlstr = "select user_roles,padname,barname,dbo.func_decoder(rights,'F') as rights from vudyog..rolesrights where dbo.func_decoder(LTRIM(RTRIM(user_roles)),'T') = '" + this.vRole + "'";
                sqlstr = sqlstr + " and padname='" + dr1["padname"].ToString() + "' and barname='" + dr1["barname"].ToString() + "' and dbo.func_decoder(LTRIM(RTRIM(company)),'T') ='" + this.txtCompany.Text.Trim() + "[" + this.txtcoyear.Text.Trim() + "]" + "'";

                dtuserrights = oDataAccess.GetDataTable(sqlstr, null, 30);
                if (dtuserrights.Rows.Count > 0)   
                {  
                    TreeNode[] arr = treeView1.Nodes.Find(dr1["PrompName"].ToString().Replace("\\<", "").Trim(), true);
                  //treeView1.Nodes
                  //  if (arr.Length > 0)
                  //  {
                  //      treeView1.SelectedNode = arr[0];
                  //      //tr.Checked = true;
                  //      treeView1.SelectedNode.Checked = true;
                  //  }

                    for (int i = 0; i < treeView1.Nodes.Count; i++)
                    {
                        if (dr1["PrompName"].ToString().Replace("\\<", "").Trim() == treeView1.Nodes[i].Text.Trim())
                        {
                            treeView1.Nodes[i].Checked = true;

                        }
                    }
                                        
                }
            }






        }

        private void CreateTreeNodes(TreeNode _parentNode, DataRow[] _dr)
        {
            TreeNode _trn;
            foreach (DataRow _dr1 in _dr)
            {
                _trn = new TreeNode();
                _trn.Name = _dr1["Levelc"].ToString();
                _trn.Text = _dr1["PrompName"].ToString().Replace("\\<","").Trim();
                CreateTreeNodes(_trn, dtcommenu.Select("LevelP='" + _dr1["Levelc"] + "'"));
                if (_parentNode == null)
                {
                    this.treeView1.Nodes.Add(_trn);
                }
                else
                {
                    _parentNode.Nodes.Add(_trn);
                }
            }
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            this.txtcoyear.Text = comboBox1.SelectedItem.ToString().Substring(comboBox1.SelectedItem.ToString().Length - 9, 9).Trim();
            this.txtCompany.Text = comboBox1.SelectedItem.ToString().Substring(0, comboBox1.SelectedItem.ToString().Length - 9).Trim();

            //sqlstr = "select co_name,dbname from vudyog..co_mast where co_name='" + this.txtCompany.Text.Trim() + "'";
            //dtcomp = oDataAccess.GetDataTable(sqlstr, null, 30);
            //DataTable ds, ds1;

            //// MessageBox.Show("Please wait, Generating Nodes................");

            //sqlstr = "select co_name,dbname from co_mast where LTRIM(RTRIM(co_name)) = '" + this.txtCompany.Text.Trim() + "' and cast(year(sta_dt) as varchar(05))+'-'+ cast(year(end_dt) as varchar(05)) '" + this.txtcoyear.Text.Trim() + "'";

            //sqlstr = "if exists(select name from master.dbo.sysdatabases where name ='" + dtcomp.Rows[0]["dbname"].ToString() + "') select 'y' as founddb else select 'N' as founddb ";
            //ds = oDataAccess.GetDataTable(sqlstr, null, 30);

            //string s = this.pAppUerName;

            //sqlstr = "select user_roles from vudyog..[user] where [user] = '" + s + "' ";
            //ds1 = oDataAccess.GetDataTable(sqlstr, null, 30);

            //if (ds1.Rows[0]["user_roles"].ToString().Trim() == "ADMINISTRATOR")
            //{
            //    treeView1.CheckBoxes = true;
            //}
            //else
            //{
            //    treeView1.CheckBoxes = false;
            //}

            //sqlstr = "select padname,barname,range,newrange from " + dtcomp.Rows[0]["dbname"].ToString() + "..com_menu";


            //sqlstr = " select space(250) as levelp,space(250) as levelc,space(10) as Levela,space(10) as Level1,space(10) as Level2,CAST(' ' as text) as Levelf,a.* from " + dtcomp.Rows[0]["dbname"].ToString() + "..com_menu a order by a.padnum,a.barnum";


            ////sqlstr = "use " + dtcomp.Rows[0]["dbname"].ToString().Trim() + " execute usp_ent_create_nodes1";
            ////dtcommenu = oDataAccess.GetDataTable(sqlstr, null, 30);


            ////foreach (DataRow dr in dtcommenu.Rows)
            ////{
            ////    mlevels = dr["level1"].ToString();
            ////    mlevelc = dr["level1"].ToString();
            ////    mlevelp = "";

            ////    foreach (DataRow dr1 in dtcommenu.Rows)
            ////    {
            ////        mlevel = dr["level2"].ToString();
            ////        if (dr1["level1"].ToString() == mlevel)
            ////        {
            ////            mlevels = dr["level1"].ToString().Trim() + '~' + dr["level2"].ToString(); ;
            ////            mlevelc = dr["level2"].ToString().Trim() + dr["level1"].ToString().Trim();
            ////            mlevelp = dr["level2"].ToString().Trim() + mlevelp.Trim();
            ////        }
            ////        else
            ////        {
            ////           // break;
            ////        }
            ////    }

            //    //sqlstr = " Update tempcom_menu set levelf='" + mlevels + "' where padname='" + dr["padname"].ToString() + "' and barname='" + dr["barname"].ToString() + "'";
            //    //oDataAccess.ExecuteSQLStatement(sqlstr, null, 30, true);
            //    //sqlstr = " Update tempcom_menu set levelc='" + mlevelc + "',levelp='" + mlevelp + "' where padname='" + dr["padname"].ToString() + "' and barname='" + dr["barname"].ToString() + "'";
            //    //oDataAccess.ExecuteSQLStatement(sqlstr, null, 30, true);

            //sqlstr = "use " + dtcomp.Rows[0]["dbname"].ToString().Trim() + " execute usp_ent_create_nodes2";
            //dtcommenu = oDataAccess.GetDataTable(sqlstr, null, 30);

            // int index = 0;

            //foreach (DataRow dr1 in dtcommenu.Rows)
            //{
            //      TreeNode n = new TreeNode();
            //     // n.Value = d["id"].ToString();
            //      n.Text = dr1["prompname"].ToString().Replace("\\<"," ").Trim();
            //      //n.Text =n.Text.Replace('<', ' '); 
            //    index=index+1;
            //    treeView1.CheckBoxes = true;
            //    if (dr1["levelp"].ToString().Trim() == "")
            //    {
            //        foreach (DataRow dr2 in dtcommenu.Rows)
            //        {
            //            if (dr1["levelc"].ToString() == dr2["levelp"].ToString())
            //            {
            //                TreeNode inner = new TreeNode();
            //                //inner.Value = r["id"].ToString();
            //                inner.Text = dr2["prompname"].ToString().Trim();
            //                //treeView1.Nodes[index].ChildNodes.Add(inner);
            //                // treeView1.Nodes[index].Nodes.Add(inner);
            //                n.Nodes.Add(inner);

            //            }

            //        }
            //        treeView1.Nodes.Add(n);

            //    }
            //    //if (dr1["levelp"].ToString().Trim() == "")
            //    //{

            //    //}


            //}

            //foreach (DataRow dr1 in dtcommenu.Rows)
            //{
            //    sqlstr = "select user_roles,padname,barname,dbo.func_decoder(rights,'F') as rights from rolesrights where dbo.func_decoder(LTRIM(RTRIM(user_roles)),'T') = '" + this.vRole + "'";
            //    sqlstr = sqlstr + " and padname='" + dr1["padname"].ToString() + "' and barname='" + dr1["barname"].ToString() + "' and dbo.func_decoder(LTRIM(RTRIM(company)),'T') ='"+ this.txtCompany.Text.Trim()+"["+this.txtcoyear.Text.Trim()+"]"+"'";
            //}



            //}


        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //DataTable dt = new DataTable();
            //dt.Columns.Add("Permission");
            //dt.Columns.Add("Allow");
            //grdPermission.Rows.Clear();
            //if (treeView1.SelectedNode.Nodes.Count == 0)
            //{
            //    dt.Rows.Add("Insert",0); 
            //    dt.Rows.Add("Change", 0);
            //    dt.Rows.Add("Delete", 0);
            //    dt.Rows.Add("Print", 0);
            //    dt.Rows.Add("Approve", 0);
            //    grdPermission.DataSource = dt;

            //}
            //else
            //{

            //}

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        //private void lstAccecompany_DragDrop(object sender, DragEventArgs e)
        //{
        //    // sender is always button2
        //    lstAvailcompany.Items.Add(e.Data.ToString());
        //}

        //private void lstAvailcompany_ItemDrag(object sender, ItemDragEventArgs e)
        //{
        //    lstAvailcompany.DoDragDrop(lstAvailcompany.SelectedItems, DragDropEffects.Move);
        //}



        //private void lstAvailcompany_MouseDown(object sender, MouseEventArgs e)
        //{

        //    nSelectedindex = lstAvailcompany.SelectedItems[0].Index;
        //    lctext = this.lstAvailcompany.Items[nSelectedindex].Text + ":" + this.lstAvailcompany.Items[nSelectedindex].SubItems[1].Text + ":" + this.lstAvailcompany.Items[nSelectedindex].SubItems[2].Text;

        //    lstAvailcompany.DoDragDrop(lctext, DragDropEffects.Copy);
        //}

        //private void lstAvailcompany_DragEnter(object sender, DragEventArgs e)
        //{
        //    e.Effect = DragDropEffects.Copy;
        //}


        //private void lstAvailcompany_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        //{

        //}

        //private void lstAccecompany_MouseDown(object sender, MouseEventArgs e)
        //{

        //    nSelectedindex = lstAvailcompany.SelectedItems[0].Index;
        //    lctext = this.lstAvailcompany.Items[nSelectedindex].Text + ":" + this.lstAvailcompany.Items[nSelectedindex].SubItems[1].Text + ":" + this.lstAvailcompany.Items[nSelectedindex].SubItems[2].Text;
        //    //listBox2.DoDragDrop(listBox2.Items[indexOfItem], DragDropEffects.Move);
        //    lstAccecompany.DoDragDrop(lctext, DragDropEffects.Move);
        //}

        //private void lstAccecompany_DragEnter(object sender, DragEventArgs e)
        //{
        //    if (e.Data.GetDataPresent(DataFormats.StringFormat) && (e.AllowedEffect == DragDropEffects.Copy))
        //        e.Effect = DragDropEffects.Copy;
        //    else
        //        e.Effect = DragDropEffects.Move;
        //}

        //private void lstAccecompany_DragDrop(object sender, DragEventArgs e)
        //{

        //    if (e.Data.GetDataPresent(DataFormats.StringFormat))
        //    {
        //        if (indexOfItemUnderMouseToDrop >= 0 && indexOfItemUnderMouseToDrop < lstAccecompany.Items.Count)
        //        {
        //            //lstAccecompany.Items.Insert(indexOfItemUnderMouseToDrop, e.Data.GetData(DataFormats.Text));
        //        }
        //        else
        //        {
        //            // add the selected string to bottom of list

        //            //item2 = new ListViewItem(new[] { " ", dr["co_name"].ToString(), dr["co_year"].ToString() });
        //            //lstAvailcompany.Items.Add(item1);
        //            lstAccecompany.Items.Add(sender.ToString());
        //        }


        //    }

        //    //GiveInfoAboutDragDropEvent(eventTime, "listBox2_DragDrop", sender, e);
        //    DateTime date = DateTime.Now;
        //}

        //private void lstAccecompany_DragOver(object sender, DragEventArgs e)
        //{

        //    //lstAccecompany.IndexFromPoint(lstAccecompany.PointToClient(new Point(e.X, e.Y)));

        //    if (indexOfItemUnderMouseToDrop != ListBox.NoMatches)
        //    {

        //        // pass the location back to use in the dragDrop event method.
        //      //  lstAccecompany.SelectedIndex = indexOfItemUnderMouseToDrop;

        //    }
        //    else
        //    {

        //        // save the intended drop location as an index number into the listBox2 Item collection.
        //       // lstAccecompany.SelectedIndex = indexOfItemUnderMouseToDrop;

        //    }
        //    //if (e.Effect == DragDropEffects.Move)  // When moving an item within listBox2
        //    //    lstAccecompany.Items.Remove((string)e.Data.GetData(DataFormats.Text));


        //}



        //private void lstAvailcompany_ItemDrag(object sender, ItemDragEventArgs e)
        //{
        //       //string lctext;
        //       //int nSelectedindex;
        //       //nSelectedindex = lstAvailcompany.SelectedItems[0].Index;
        //       //lctext = this.lstAvailcompany.Items[nSelectedindex].Text + ":" + this.lstAvailcompany.Items[nSelectedindex].SubItems[1].Text + ":" + this.lstAvailcompany.Items[nSelectedindex].SubItems[2].Text;

        //       //lstAvailcompany.DoDragDrop(lctext, DragDropEffects.Move);
        //}


        //private void lstAvailcompany_MouseDown(object sender, MouseEventArgs e)
        //{
        //    this.DoDragDrop(this, DragDropEffects.Copy);
        //}

        //private void lstAccecompany_DragEnter(object sender, DragEventArgs e)
        //{
        //    e.Effect = DragDropEffects.Copy;
        //}



    }
    
}
