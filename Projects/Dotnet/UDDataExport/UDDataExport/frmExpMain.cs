using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using uBaseForm;
using udSelectPop;
using DataAccess_Net;
using System.IO;
using cudZipUnzip;
using System.Threading;
using System.Reflection;
using System.Globalization;
using System.Diagnostics;/*Added Rup 07/03/2011*/


namespace UDDataExport
{
    public partial class frmExpMain : uBaseForm.FrmBaseForm
    {
        private string vLocCode = string.Empty;
        private DataSet dsCompany;
        DataAccess_Net.clsDataAccess oDataAccess;
        private String cAppPId, cAppName; /*Added Rup 07/03/2011*/
        int vFileNo;
        bool IsExitCalled;
        string vIE_LocCode;
        int ListViewItemCount;
        //const int Timeout = 3000;//commented by Archana K. on 11/12/13 for Bug-20512 
        const int Timeout = 4000;//Added by Archana K. on 03/06/13 for Bug-5837//Changed by Archana K. on 11/12/13 for Bug-20512 

        public frmExpMain(string[] args)
        {
            InitializeComponent();
            ListViewItemCount = 0;
            this.pFrmCaption = "Data Export";
            this.Text = this.pFrmCaption; //Rup
            this.pCompId = Convert.ToInt16(args[0]);
            this.pComDbnm = args[1];
            this.pServerName = args[2];
            this.pUserId = args[3];
            this.pPassword = args[4];

            if (args[5] != "")
            {
                this.pPApplRange = args[5].ToString().Replace("^", "");
            }
            this.pAppUerName = args[6];
            Icon MainIcon = new System.Drawing.Icon(args[7].Replace("<*#*>", " "));
            this.pFrmIcon = MainIcon;
            this.pPApplText = args[8].Replace("<*#*>", " ");
            this.pPApplName = args[9];
            this.pPApplPID = Convert.ToInt16(args[10]);
            this.pPApplCode = args[11];

            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();

            dsCompany = new DataSet();
            string strSQL = "select * from vudyog..Co_Mast where compid=" + this.pCompId;
            dsCompany = oDataAccess.GetDataSet(strSQL, null, 20);
            vIE_LocCode = dsCompany.Tables[0].Rows[0]["ie_locCode"].ToString().Trim();      // Added by Sachin N. S. on 09/04/2015 for Bug-25651
            this.lblFrom.Text = "Data Export From '" + dsCompany.Tables[0].Rows[0]["co_name"].ToString().Trim() + "'";
            this.dtpFrmDt.CustomFormat = "dd/MM/yyyy";
            this.dtpFrmDt.Format = DateTimePickerFormat.Custom;
            this.dtpToDt.CustomFormat = "dd/MM/yyyy";
            this.dtpToDt.Format = DateTimePickerFormat.Custom;
            //this.mInsertProcessIdRecord(); //Rup
            //this.SetFormColor();//Rup
            //string appPath = @"F:\Installer12.0\";
            string appPath = Application.ExecutablePath;
            appPath = Path.GetDirectoryName(appPath);
            string fName = appPath + @"\bmp\pickup.gif"; //Rup
            if (File.Exists(fName) == true)
            {
                this.btnSendTo.Image = Image.FromFile(fName);
                this.btnPath.Image = Image.FromFile(fName); //Added By Kishor A.
                
            }
            fName = appPath + @"\bmp\save.gif";
            if (File.Exists(fName) == true)
            {
                this.btnExport.Image = Image.FromFile(fName);
                this.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            }
            fName = appPath + @"\bmp\close2.gif";
            if (File.Exists(fName) == true)
            {
                this.btnCancel.Image = Image.FromFile(fName);
                this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            }

            this.txtSendTo.Enabled = true;
        }
        private void mInsertProcessIdRecord()/*Added Rup 07/03/2011*/
        {
            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            //cAppName = "udDynamicMaster.exe";//Commented by Archana K. on 05/04/13 for Bug-5837
            cAppName = "uddataexport.exe";//Changed by Archana K. on 05/04/13 for Bug-5837
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            //sqlstr = " Set DateFormat dmy insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";
            sqlstr = " insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";
            //oDataAccess.ExecuteSQLStatement(sqlstr, null, 20, true);//Commented by Archana K. on 03/06/13 for Bug-5837
            oDataAccess.ExecuteSQLStatement(sqlstr, null, Timeout, true);//Changed by Archana K. on 03/06/13 for Bug-5837
        }
        
        private void mDeleteProcessIdRecord()/*Added Rup 07/03/2011*/
        {
            DataSet dsData = new DataSet();
            string sqlstr;
            sqlstr = " Delete from vudyog..ExtApplLog where pApplNm='" + this.pPApplName + "' and pApplId=" + this.pPApplPID + " and cApplNm= '" + cAppName + "' and cApplId= " + cAppPId;
            //oDataAccess.ExecuteSQLStatement(sqlstr, null, 20, true);//Commented by Archana K. on 03/06/13 for Bug-5837
            oDataAccess.ExecuteSQLStatement(sqlstr, null, Timeout, true);//Changed by Archana K. on 03/06/13 for Bug-5837
        }
        
        private void mcheckCallingApplication()/*Added Rup 07/03/2011*/
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
                IsExitCalled = true;
                Application.Exit();
            }
        }

        //private void SetMenuRights()
        //{
        //    btnNew.Enabled = false;
        //    btnEdit.Enabled = false;
        //    //menuItemRemoveToolbar.Enabled = false;
        //    btnPrint.Enabled = false;

        //    DataSet dsRights = new DataSet();

        //    dsRights = oForm.GetUserRightsForMenu(_Range, _UserName, oDataAccess);
        //    if (dsRights != null)
        //    {
        //        string rights = dsRights.Tables[0].Rows[0]["rights"].ToString();
        //        //MessageBox.Show("Rights: " + rights);
        //        int len = rights.Length;
        //        string newString = "";
        //        ArrayList myArray = new ArrayList();

        //        while (len > 2)
        //        {
        //            newString = rights.Substring(0, 2);
        //            rights = rights.Substring(2);
        //            myArray.Add(newString);
        //            len = rights.Length;
        //        }
        //        myArray.Add(rights);

        //        btnNew.Enabled = (myArray[0].ToString().Trim() == "IY" ? true : false);
        //        btnEdit.Enabled = (myArray[1].ToString().Trim() == "CY" ? true : false);
        //        //menuItemRemoveToolbar.Enabled = (myArray[2].ToString().Trim() == "DY" ? true : false);
        //        btnPrint.Enabled = (myArray[3].ToString().Trim() == "PY" ? true : false);
        //    }
        //}


        private void btnSendTo_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet dstemp = new DataSet();
            strSQL = "select Loc_Desc,Loc_Code from loc_Master where Loc_Code<>'" + vIE_LocCode + "'";
            dstemp = oDataAccess.GetDataSet(strSQL, null, 20);
            DataView dvw = dstemp.Tables[0].DefaultView;

            VForText = "Select Location Name";
            vSearchCol = "Loc_Desc";
            vDisplayColumnList = "Loc_Desc:Location Name";
            vReturnCol = "Loc_Desc,Loc_Code";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtSendTo.Text = oSelectPop.pReturnArray[0];
                this.vLocCode = oSelectPop.pReturnArray[1];
            }
            //this.mthRefreshGrid(strcol);
            this.dtpFrmDt.Focus();//Added by Archana K. on 31/05/13 for Bug-5837
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit(); //Rup
        }

        private void btnPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.Description = "Select Folder";
            fbd.ShowDialog();
            if (fbd.SelectedPath != string.Empty)
            {
                this.txtPath.Text = fbd.SelectedPath;
                vIE_LocCode = dsCompany.Tables[0].Rows[0]["ie_locCode"].ToString().Trim();
                this.txtPath.Text = this.txtPath.Text.Trim() + "\\" + vIE_LocCode + DateTime.Now.ToString().Replace(":", "").Replace(" ", "").Replace("/", "") + ".zip";
            }
            //this.txtPath.Text + "\\" + vCode + "_" + vTable + "_" + vCategory + "_" + vvCode + "_" + vTgLocCode + "_" + vTgEntTY + "_" + DateTime.Now.ToString().Replace(":", "").Replace(" ", "").Replace("/", "") + ".xml";
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            //<--- Added By Amrendra On 27/02/2012 
            //Added By Archana K. on 26/10/12 for Bug-5837 start
            if (vIE_LocCode == "")
            {
                MessageBox.Show("Location code not defined in control centre..", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Application.Exit();
            }
            else
            {
                //Added By Archana K. on 26/10/12 for Bug-5837 end
                IsExitCalled = false;
                this.mcheckCallingApplication();      

                if (IsExitCalled == true)
                {
                    Application.Exit();
                    return;
                }
                //<--- Added By Amrendra On 27/02/2012 

                //string[] vFileList = new string[90];//Commenetd by Archana K. on 13/05/13 for Bug-5837
                List<string> vFileList = new List<string>();//Added by Archaan K. on 13/05/13 for Bug-5837
                listView1.Items.Clear();
                if (string.IsNullOrEmpty(this.txtSendTo.Text))
                {
                    MessageBox.Show("Export Data to could not empty...!", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    this.txtSendTo.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(this.txtPath.Text))
                {
                    MessageBox.Show("Export Folder path could not empty...!", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    this.txtPath.Focus();
                    return;
                }
                //MessageBox.Show("Basic Validation Crossed");

                DataSet dsExpMaster = new DataSet();
                string strSQL = "select * from Exp_Master where charindex('<<" + this.vLocCode + ">>'" + ",ExpLocDet)>0 and Export=1";
                dsExpMaster = oDataAccess.GetDataSet(strSQL, null, 20);
                //MessageBox.Show("Exp_Master Crossed Toatal Recort Fornd : " + dsExpMaster.Tables[0].Rows.Count.ToString());
                string vDirName = Path.GetDirectoryName(this.txtPath.Text);

                vFileNo = 0;
                foreach (DataRow dr in dsExpMaster.Tables[0].Rows)
                {
                    this.mGenerateXml(dr, vDirName, ref vFileList);
                }
                string[] filenames;
                //MessageBox.Show("Xml Generation Done.");

                filenames = Directory.GetFiles(vDirName, "*.xml", SearchOption.TopDirectoryOnly);
                cudZipUnzip.udZipUnzip oZip = new cudZipUnzip.udZipUnzip();

                //oZip.UdyogZip(Path.GetDirectoryName(this.txtPath.Text), this.txtPath.Text, "", vFileList);// commented By Archana on 05/10/2012 for Bug-5837
                oZip.UdyogZip(Path.GetDirectoryName(this.txtPath.Text) + "\\", this.txtPath.Text, "", vFileList.ToArray());     // Changed By Archana on 05/10/2012 for Bug-5837

                //MessageBox.Show("Zip Generation Done.");
                foreach (string Fil in vFileList) // for each file, generate a zipentry
                {
                    if (Fil != null)
                    {
                        File.Delete(Fil);
                    }
                }

                MessageBox.Show("Exporting Finished", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
            } //Added By Archana K. on 26/10/12 for Bug-5837
        }

        //private void mGenerateXml(DataRow vRow, string vDirName,ref  string[] vFileList)//Commented by Archana K. on 13/05/13 For Bug-5837
        private void mGenerateXml(DataRow vRow, string vDirName, ref  List<string> vFileList)//Changed by Archana K. on 13/05/13 for Bug-5837 
        {
            DataSet tDs = new DataSet();
            string strSQL = string.Empty, vFileName = string.Empty, vvCode, VExpDataVol = string.Empty, vTgLocCode = string.Empty, vTgEntTY = string.Empty;
            string vDtdFilt = string.Empty;
            string vCategory = String.Empty, vDataExport = string.Empty, vSpName = string.Empty;
            string vCommonPara;

            vvCode = dsCompany.Tables[0].Rows[0]["ie_locCode"].ToString().Trim();
            string vCode = string.Empty, vTables = string.Empty, vTable = string.Empty, vExpLocDet = string.Empty, vLocList = string.Empty;
            int pos = 0;
            vCode = vRow["Code"].ToString();
            vTables = vRow["Tables"].ToString();
            vCategory = vRow["Category"].ToString();
            VExpDataVol = vRow["ExpDataVol"].ToString();
            vSpName = vRow["Sp_Name"].ToString();
            //vExpLocDet = vRow["ExpLocDet"].ToString();            

            oDataAccess.BeginTransaction();//Added by Archana K. on 07/3/14 for Bug-20512 

            while (vTables.Length > 0 && vTables.IndexOf("<<") > -1)
            {
                vDtdFilt = string.Empty;
                vDataExport = "";
                pos = vTables.IndexOf("<<");
                vTables = vTables.Substring(pos + 2, vTables.Length - pos - 2);
                pos = vTables.IndexOf(">>");
                vTable = vTables.Substring(0, pos);
                if (vTable.IndexOf("##DtdFilt") > -1)
                {
                    vDtdFilt = "Date";
                }

             
                //pos = vTable.IndexOf("##");
                //vTable = vTable.Substring(0, pos);
                vExpLocDet = vRow["ExpLocDet"].ToString();

                pos = vTables.IndexOf("##ExpCode");
                vDataExport = vTable.Substring(pos, vTable.Length - pos);
                vDataExport = vDataExport.Replace("##ExpCode<", "");
                vDataExport = "''" + vIE_LocCode + "''+" + vDataExport;
                vDataExport = vDataExport.Replace("`", "''");
                pos = vTable.IndexOf("##");
                vTable = vTable.Substring(0, pos);
                vTable = vTable.ToLower();
                pos = vExpLocDet.IndexOf("<<" + this.vLocCode);
                vExpLocDet = vExpLocDet.Substring(pos + 2, vExpLocDet.Length - pos - 2);
                listView1.Items.Add(vTable, vTable); /*xxxxxxxxxxxx*/
                listView1.Items[listView1.Items.Count - 1].SubItems.Add("Working...");
                //listView1.Items[ListViewItemCount].SubItems.Add("Working...");
                listView1.Refresh();
                //Added by Archana K. on 10/12/13 for Bug-20512 start
                if (vTable.ToUpper() == "GEN_SRNO" || vTable.ToUpper() == "IT_SRSTK" || vTable.ToUpper() == "IT_SRTRN") // Added By Kishor A. For Bug-26960
                    vFileName = vDirName + "\\" + vTable + "_" + vCode + ".xml";
                else
                    //Added by Archana K. on 10/12/13 for Bug-20512 end
                    vFileName = vDirName + "\\" + vTable + ".xml";
                TextWriter tw; //Added by Archana K. on 03/06/13 for Bug-5837 
                //Commented by Archana K. on 03/06/13 for Bug-5837 start
                //vCommonPara = "<<TablNm=" + vTable + ">>";
                //vCommonPara = vCommonPara + "<<Cate=" + vCategory + ">>";
                //vCommonPara = vCommonPara + "<<FileType=xsd>>";

                //vCommonPara = vCommonPara + "<<ExpCode=" + vDataExport + ">>";
                //vCommonPara = vCommonPara + "<<ExpDataVol=" + VExpDataVol + ">>";
                //vCommonPara = vCommonPara + "<<DtFld=" + vDtdFilt + ">>";
                //vCommonPara = vCommonPara + "<<sDate=" + this.dtpFrmDt.Value.ToShortDateString() + ">>";
                //vCommonPara = vCommonPara + "<<eDate=" + this.dtpToDt.Value.ToShortDateString() + ">>";
                ////strSQL = "Set DateFormat dmy execute " + vSpName + " '" + vCommonPara + "'";
                //strSQL = "execute " + vSpName + " '" + vCommonPara + "'";

                //tDs = oDataAccess.GetDataSet(strSQL, null, 20);

                //TextWriter tw;
                //tw = new StreamWriter(vFileName.Replace(".xml", "_xsd.xml"));
                //if (tDs.Tables[0].Rows.Count > 0)
                //{
                //    tw.Write(tDs.Tables[0].Rows[0]["cxml"].ToString());
                //}
                //tw.Close();
                //vFileList[vFileNo] = vFileName.Replace(".xml", "_xsd.xml");
                //vFileNo = vFileNo + 1;
                //Commented by Archana K. on 03/06/13 for Bug-5837 end
                try //Added by Archana K. on 07/03/14 for Bug-20512  
                {
                    vCommonPara = "<<TablNm=" + vTable + ">>";
                    vCommonPara = vCommonPara + "<<Cate=" + vCategory + ">>";
                    vCommonPara = vCommonPara + "<<FileType=xml>>";
                    vCommonPara = vCommonPara + "<<ExpCode=" + vDataExport + ">>";
                    vCommonPara = vCommonPara + "<<ExpDataVol=" + VExpDataVol + ">>";
                    vCommonPara = vCommonPara + "<<DtFld=" + vDtdFilt + ">>";
                    vCommonPara = vCommonPara + "<<sDate=" + this.dtpFrmDt.Value.ToShortDateString() + ">>";
                    vCommonPara = vCommonPara + "<<eDate=" + this.dtpToDt.Value.ToShortDateString() + ">>";
                    //strSQL = "set dateformat dmy execute " + vSpName + " '" + vCommonPara + "'";
                    strSQL = "execute " + vSpName + " '" + vCommonPara + "'";
                    //tDs = oDataAccess.GetDataSet(strSQL, null,20);//Commented by Archana K. on 07/03/14 for Bug-20512
                    tDs = oDataAccess.GetDataSet(strSQL, null, 100);//Commented by Archana K. on 07/03/14 for Bug-20512
                    //tw = new StreamWriter(vFileName);//Commented by Archana K. on 09/05/13 for Bug-5837
                    //Added by Archana K. on 09/05/13 for Bug-5837 start

                    if (tDs.Tables.Count > 0)
                    {
                        if (tDs.Tables[0].Rows.Count > 0)
                        {

                            if (tDs.Tables[0].Rows[0]["cxml"].ToString() != "")
                            {
                                tw = new StreamWriter(vFileName);
                                //Added by Archana K. on 09/05/13 for Bug-5837 end
                                if (tDs.Tables[0].Rows.Count > 0)
                                {
                                    tw.Write(tDs.Tables[0].Rows[0]["cxml"].ToString());
                                }
                                tw.Close();
                                //Added by Archana K. on 09/05/13 for Bug-5837 start
                            }
                            else
                            {
                                string fnm = vFileName.Replace(".xml", "_xsd.xml");
                                if (File.Exists(Path.Combine(vDirName, fnm)))
                                {
                                    File.Delete(Path.Combine(vDirName, fnm));
                                    vFileList.Remove(fnm);
                                }
                                //****** Added by Sachin N. S. on 09/04/2015 for Bug-25651 -- Start
                                listView1.Items[listView1.Items.Count - 1].SubItems[1].Text = "No data to export...";
                                listView1.Refresh();
                                listView1.TopItem = listView1.Items[listView1.Items.Count - 1];
                                //****** Added by Sachin N. S. on 09/04/2015 for Bug-25651 -- End
                                continue;
                            }
                        }
                    }
                    else
                    {
                        //Birendra: Bug-14505 on 10/04/2014:Start:
                        throw new System.ArgumentException(strSQL);
                        //return;
                        //Birendra: Bug-14505 on 10/04/2014:End:
                    }
                    //Added by Archana K. on 09/05/13 for Bug-5837 end
                    //Commented by Archana K. on 09/05/13 for Bug-5837 start
                    //vFileList[vFileNo] = vFileName;
                    //vFileNo = vFileNo + 1;
                    //Commented by Archana K. on 09/05/13 for Bug-5837 end
                    vFileList.Add(vFileName);//Added by Archana K. on 13/05/13 for Bug-5837
                    
                    //if (vCategory=="Transaction")
                    //{
                    //    strSQL = strSQL + ",sEntry_Ty=Entry_ty";
                    //}
                    //strSQL = strSQL + ",* FROM " + vTable;
                    //strSQL = strSQL + " where 1=2 FOR XML auto,XMLSCHEMA,ROOT('" + vTable.ToLower() + "'))  as cxml";
                    //tDs = oDataAccess.GetDataSet(strSQL, null, 20);

                    //TextWriter tw;
                    //tw = new StreamWriter(vFileName.Replace(".xml", "_xsd.xml"));
                    //if (tDs.Tables[0].Rows.Count > 0)
                    //{
                    //    tw.Write(tDs.Tables[0].Rows[0]["cxml"].ToString());
                    //}
                    //tw.Close();

                    //vFileList[vFileNo] = vFileName.Replace(".xml", "_xsd.xml");
                    //vFileNo = vFileNo + 1;

                    //strSQL = "set dateformat dmy Select (SELECT Dataexport1="+vDataExport.Replace("''","'");
                    //if (vCategory == "Transaction")
                    //{
                    //    strSQL = strSQL + ",sEntry_Ty=Entry_ty";
                    //}
                    //strSQL = strSQL + ",* FROM " + vTable;


                    //strSQL = strSQL + (VExpDataVol == "Updated" ? " where isnull(DataExport,'')=''" : " where 1=1");
                    //if (vDtdFilt)
                    //{
                    //    strSQL = strSQL + " and (Date between '"+this.dtpFrmDt.Value.ToShortDateString()+"' and '"+this.dtpToDt.Value.ToShortDateString()+"')";
                    //}
                    //strSQL = strSQL + " FOR XML auto, ROOT('" + vTable.ToLower() + "'))  as cxml";

                    //tDs = oDataAccess.GetDataSet(strSQL, null, 20);



                    //tw = new StreamWriter(vFileName);
                    ////tw.WriteLine("<" + vTable + ">");
                    //if (tDs.Tables[0].Rows.Count > 0)
                    //{
                    //    tw.Write(tDs.Tables[0].Rows[0]["cxml"].ToString());
                    //}
                    ////tw.WriteLine("</" + vTable + ">");
                    //tw.Close();
                    //vFileList[vFileNo] = vFileName;
                    //vFileNo = vFileNo + 1;

                    

                    if (vTable.ToUpper() == "LITEMALL" & vDtdFilt != string.Empty)
                    {
                        //strSQL = "set dateformat dmy if exists (select c.[name] from syscolumns c  inner join sysobjects o on (c.id=o.id) where o.[Name]='" + vTable + "' and c.[Name]='DataExport')";
                        strSQL = "if exists (select c.[name] from syscolumns c  inner join sysobjects o on (c.id=o.id) where o.[Name]='" + vTable + "' and c.[Name]='DataExport')";
                        strSQL = strSQL + " begin";
                        strSQL = strSQL + " Declare @SqlCommand nvarchar(2000)";
                        //strSQL = strSQL + " set @SqlCommand='update " + vTable + " set DataExport=" + vDataExport + " where isnull(DataExport,'''')='''' ";
                        strSQL = strSQL + " set @SqlCommand='update a set a.DataExport=" + vDataExport.Replace("Tran_cd", "a.Tran_cd").Replace("Entry_ty", "a.Entry_ty") + " from " + vTable + " a  ";
                        strSQL = strSQL + " inner join dcmain b on (a.tran_cd=b.Tran_cd) ";
                        strSQL = strSQL + "  where isnull(a.DataExport,'''')='''' ";
                        
                        
                        //   if (vDtdFilt != string.Empty)
                        //  {
                        strSQL = strSQL + " and (b.Date between ''" + this.dtpFrmDt.Value.ToShortDateString() + "'' and ''" + this.dtpToDt.Value.ToShortDateString() + "'') ";
                        // }
                        strSQL = strSQL + "'" + " execute sp_executesql  @SqlCommand";
                        strSQL = strSQL + " end";
                        //oDataAccess.ExecuteSQLStatement(strSQL, null, 20, true);//Commented by Archana K. on 03/06/13 for Bug-5837
                        oDataAccess.ExecuteSQLStatement(strSQL, null, Timeout, true);//Changed by Archana K. on 03/06/13 for Bug-5837
                    }
                    else
                    {
                        //strSQL = "set dateformat dmy if exists (select c.[name] from syscolumns c  inner join sysobjects o on (c.id=o.id) where o.[Name]='" + vTable + "' and c.[Name]='DataExport')";
                        strSQL = "if exists (select c.[name] from syscolumns c  inner join sysobjects o on (c.id=o.id) where o.[Name]='" + vTable + "' and c.[Name]='DataExport')";
                        strSQL = strSQL + " begin";
                        strSQL = strSQL + " Declare @SqlCommand nvarchar(2000)";
                        //strSQL = strSQL + " set @SqlCommand='update " + vTable + " set DataExport=rtrim(isnull(DataExport,''''))+''" + this.txtSendTo.Text + "'' where charindex(''<<" + this.txtSendTo.Text + ">>'',DataExport)=0'";
                        //strSQL = strSQL + " set @SqlCommand='update " + vTable + " set DataExport=" + vDataExport + " where isnull(DataExport,'''')=''''";

                        if (vTable.ToUpper() != "IT_SRSTK")
                        {
                            strSQL = strSQL + " set @SqlCommand='update " + vTable + " set DataExport=" + vDataExport + " where isnull(DataExport,'''')=''''";
                        }
                        //if (vDtdFilt != string.Empty) Commnted By Kishor A. for bug-26960 on 21/09/2015
                        else
                        {
                            strSQL = strSQL + " set @SqlCommand='update " + vTable + " set DataExport=" + vDataExport + " where isnull(DataExport,'''')='''' and InEntry_ty =''" + vCode + "''";
                            
                        }
                        //Added By Kishor A. for bug - 26960 on 21 / 09 / 2015 Start..
                        if (vDtdFilt != string.Empty && vTable.ToUpper() != "IT_SRSTK")
                        {
                            strSQL = strSQL + " and (Date between ''" + this.dtpFrmDt.Value.ToShortDateString() + "'' and ''" + this.dtpToDt.Value.ToShortDateString() + "'') ";
                        }

                        if (vTable.ToUpper() == "IT_SRSTK")
                        {
                           strSQL = strSQL + " and (InDate between ''" + this.dtpFrmDt.Value.ToShortDateString() + "'' and ''" + this.dtpToDt.Value.ToShortDateString() + "'') ";
                        }
                        //Added By Kishor A. for bug - 26960 on 21 / 09 / 2015 End..

                        if (vTable.ToUpper() == "GEN_SRNO" || vTable.ToUpper() == "IT_SRTRN")
                        {
                            strSQL = strSQL + " and entry_ty=''" + vCode + "''";
                        }
                        strSQL = strSQL + "'" + " execute sp_executesql  @SqlCommand";
                        strSQL = strSQL + " end";
                        //oDataAccess.ExecuteSQLStatement(strSQL, null, 20, true); //Commented by Archana K. on 03/06/13 for Bug-5837
                        oDataAccess.ExecuteSQLStatement(strSQL, null, Timeout, true); //Changed by Archana K. on 03/06/13 for Bug-5837           
                    }
                    //Added by Archana K. on 07/03/14 for Bug-20512 start  
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in exporting records in the database.\n" + ex.Message, this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    oDataAccess.RollbackTransaction();
                    //Birendra: Bug-14505 on 10/04/2014:Start:
                    this.Close();
                    //return;
                    //Birendra: Bug-14505 on 10/04/2014:End:
                                    }
                //Added by Archana K. on 07/03/14 for Bug-20512 end

            listView1.Items[listView1.Items.Count - 1].SubItems[1].Text = "Finished...";
                listView1.Refresh();
                listView1.TopItem = listView1.Items[listView1.Items.Count - 1];
            }
            oDataAccess.CommitTransaction();//Added by Archana K. on 07/03/14 for Bug-20512
        }

        private void frmExpMain_Load(object sender, EventArgs e)
        {
            //Added By Amrendra On 27/02/2012  --->
            string _SqlDefaultDateFormate = mGetSQLServerDefaDateFormate();
            CultureInfo ci = new CultureInfo("en-US");
            //ci.DateTimeFormat.ShortDatePattern = "MM/dd/yyyy";
            switch (_SqlDefaultDateFormate)
            {
                case "mdy":
                    ci.DateTimeFormat.ShortDatePattern = "MM/dd/yyyy";
                    break;
                case "dmy":
                    ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
                    break;
                case "ymd":
                    ci.DateTimeFormat.ShortDatePattern = "yyyy/MM/dd";
                    break;
            }
            Thread.CurrentThread.CurrentCulture = ci;
            this.mInsertProcessIdRecord();
            IsExitCalled = false;
            this.mcheckCallingApplication();  
            if (IsExitCalled == true)
            {
                Application.Exit();
                return;
            }
            //<--- Added By Amrendra On 27/02/2012 

            //dtpFrmDt.Value = DateTime.Parse("01/"+DateTime.Today.Month.ToString().Trim()+"/"+DateTime.Today.Year.ToString().Trim())  ;//commented by Archana K. on 31/05/13 for Bug-5837
            dtpFrmDt.Value = DateTime.Parse(DateTime.Today.Month.ToString().Trim() + "/" + "01/" + DateTime.Today.Year.ToString().Trim());//changed by Archana K. on 31/05/13 for Bug-5837
            dtpToDt.Value = DateTime.Now;
        }

        private string mGetSQLServerDefaDateFormate()//<--- Added By Amrendra On 21/01/2012 
        {
            return oDataAccess.GetDataTable("select [dateformat] from master..syslanguages where langid=(select [value] from sys.configurations where [name]='default language')", null, 20).Rows[0][0].ToString();
        }

        private void frmExpMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            mDeleteProcessIdRecord();
        }


    }

}
