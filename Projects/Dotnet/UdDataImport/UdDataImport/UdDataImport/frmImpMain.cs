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
using System.Globalization;     /*Added Amar 20/01/2012*/
using System.Diagnostics;
using System.Xml;
using System.Xml.XPath;         /*Added Archana 30/04/13 */
using System.Data.SqlClient;    /*Added Amar 20/01/2012*/

namespace WindowsFormsApplication1
{
    public partial class frmImpMain : uBaseForm.FrmBaseForm
    {
        DataAccess_Net.clsDataAccess oDataAccess;
        private string vLocCode = string.Empty;
        bool IsExitCalled;
        private String cAppPId,cAppName;    /*Added Amar 20/01/2012*/
        private string Constr=string.Empty; //Added by Archana K. on 06/04/13 for Bug-5837 
        private DataSet  dsImpMaster;
        //const int Timeout = 3000; //Added by Archana K. on 03/06/13 for Bug-5837//commented by Archana K. on 11/12/13 for Bug-20512 
        const int Timeout = 4000;   //Added by Archana K. on 03/06/13 for Bug-5837//Changed by Archana K. on 11/12/13 for Bug-20512 

        public frmImpMain(string[] args)
        {
            InitializeComponent();
            this.pFrmCaption = "Data Import";
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
            
            this.dtpFrmDt.CustomFormat = "dd/MM/yyyy";
            this.dtpFrmDt.Format = DateTimePickerFormat.Custom;
            this.dtpToDt.CustomFormat = "dd/MM/yyyy";
            this.dtpToDt.Format = DateTimePickerFormat.Custom;

            string appPath = Application.ExecutablePath;
            appPath = Path.GetDirectoryName(appPath);
            string fName = appPath + @"\bmp\pickup.gif";

            if (File.Exists(fName) == true)
            {
                this.btnImportFrom.Image = Image.FromFile(fName);
                this.btnPath.Image = Image.FromFile(fName);
            }

            //***** Commented by Sachin N. S. on 13/04/2015 for Bug-25651 -- Start
            //fName = appPath + @"\bmp\save.gif";
            //if (File.Exists(fName) == true)
            //{
            //    this.btnImportFrom.Image = Image.FromFile(fName);
            //    this.btnImportFrom.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            //}
            //***** Commented by Sachin N. S. on 13/04/2015 for Bug-25651 -- End

            fName = appPath + @"\bmp\close2.gif";
            if (File.Exists(fName) == true)
            {
                this.btnCancel.Image = Image.FromFile(fName);
                this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            }
        }
        private void mInsertProcessIdRecord()/*Added Amar 20/01/2012*/
        {
            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            //cAppName = "udDynamicMaster.exe"; //Commented by Archana K. on 05/04/13
            cAppName = "UdDataImport.exe";      //Changed by Archana K. on 05/04/13
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            //sqlstr = " Set DateFormat dmy insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";
            sqlstr = " insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";
            //oDataAccess.ExecuteSQLStatement(sqlstr, null, 20, true);      //Commented by Archana K. on 03/06/13 for Bug-5837
            oDataAccess.ExecuteSQLStatement(sqlstr, null, Timeout, true);   //Changed by Archana K. on 03/06/13 for Bug-5837
        }
        private void mDeleteProcessIdRecord()/*Added Amar 20/01/2012*/
        {
            DataSet dsData = new DataSet();
            string sqlstr;
            sqlstr = " Delete from vudyog..ExtApplLog where pApplNm='" + this.pPApplName + "' and pApplId=" + this.pPApplPID + " and cApplNm= '" + cAppName + "' and cApplId= " + cAppPId;
            oDataAccess.ExecuteSQLStatement(sqlstr, null, Timeout, true);   //Changed by Archana K. on 03/06/13 for Bug-5837
        }
        private void mcheckCallingApplication()/*Added Amar 20/01/2012*/
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
                MessageBox.Show("Can't proceed, Main Application " + this.pPApplText + " is closed", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                IsExitCalled = true;
                Application.Exit();
            }
        }

        private void frmImpMain_Load(object sender, EventArgs e)/*Added Amar 20/01/2012*/
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
            //this.mcheckCallingApplication();
            if (IsExitCalled == true)
            {
                Application.Exit();
                return;
            }
            //<--- Added By Amrendra On 27/02/2012 
           
            //dtpFrmDt.Value = DateTime.Parse("01/" + DateTime.Today.Month.ToString().Trim() + "/" + DateTime.Today.Year.ToString().Trim());//Commented by Archana K. on 31/05/13 for Bug-5837 
            dtpFrmDt.Value = DateTime.Parse(DateTime.Today.Month.ToString().Trim() + "/" + "01/" + DateTime.Today.Year.ToString().Trim());//Changed by Archana K. on 31/05/13 for Bug-5837 
            dtpToDt.Value = DateTime.Now;
          
        }

        private string mGetSQLServerDefaDateFormate()//<--- Added By Amrendra On 21/01/2012 
        {
            return oDataAccess.GetDataTable("select [dateformat] from master..syslanguages where langid=(select [value] from sys.configurations where [name]='default language')", null, 20).Rows[0][0].ToString();
        }

        private void btnImportFrom_Click(object sender, EventArgs e)
        {
            //this.mcheckCallingApplication(); /*Added Amar 20/01/2012*/
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;

            //******* Added by Sachin N. S. on 11/04/2015 for Bug-25651 -- Start
            string vIE_LocCode;
            DataSet dsCompany;
            dsCompany = new DataSet();
            strSQL = "select ie_locCode from vudyog..Co_Mast where compid=" + this.pCompId;
            dsCompany = oDataAccess.GetDataSet(strSQL, null, 20);
            vIE_LocCode = dsCompany.Tables[0].Rows[0]["ie_locCode"].ToString().Trim();
            //******* Added by Sachin N. S. on 11/04/2015 for Bug-25651 -- End

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
                this.txtImportFrom.Text = oSelectPop.pReturnArray[0];
                this.vLocCode = oSelectPop.pReturnArray[1];
            }
            this.dtpFrmDt.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit(); 
        }

        private void btnPath_Click(object sender, EventArgs e)
        {
            //this.mcheckCallingApplication(); /*Added Amar 20/01/2012*/
            Stream myStream = null;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = "D:\\";
            //ofd.Filter = "zip files (*.zip)|*.zip|All files (*.*)|*.*";  //Commented by Archana K. on 12/03/13 for Bug-5837 
            ofd.Filter = "zip files (*.zip)|*.zip";    //Changed by Archana K. on 12/03/13 for Bug-5837
            ofd.FilterIndex = 1;
            ofd.RestoreDirectory = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = ofd.OpenFile()) != null)
                    {
                        if (myStream.CanRead)
                        {
                            this.txtPath.Text = ofd.FileName;  
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message, this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            //<--- Added By Amrendra On 27/02/2012 
            IsExitCalled = false;
            //this.mcheckCallingApplication();
            if (IsExitCalled == true)
            {
                Application.Exit();
                return;
            }
            //<--- Added By Amrendra On 27/02/2012 

            listView1.Items.Clear();
            if (string.IsNullOrEmpty(this.txtImportFrom.Text))
            {
                MessageBox.Show("Import From cannot be empty...!", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.txtImportFrom.Focus();
                return;
            }

            if (string.IsNullOrEmpty(this.txtPath.Text))
            {
                MessageBox.Show("Import Folder path cannot be empty...!", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.txtPath.Focus();
                return;
            }
            this.Text = this.pFrmCaption + " : Importing in progress...";
            string vfpath = string.Empty, fName;
            string SqlStr, vTables, vTable;
            string vImpTblNm = string.Empty;
            //Birendra : Bug-20512 on 6/02/2014:Start:
            //bool transactioncheck;
            //transactioncheck = false;
            //Birendra : Bug-20512 on 6/02/2014:End:

            //Added by Archana K.on 06/04/13 for Bug-5837 start
            StringReader sr;
            string S;
            int index;
            string ELoc_code;
            List<string> delTargetTbl = new List<string>();
            //Added by Archana K.on 06/04/13 for Bug-5837 end

            int pos;
            cudZipUnzip.udZipUnzip UZIP = new cudZipUnzip.udZipUnzip();
            //vfilelist = new string[] { Path.GetDirectoryName(this.txtZipFile.Text) + @"\eFileInfo.xml", Path.GetDirectoryName(this.txtZipFile.Text) + @"\ST3.xml" };
            if (File.Exists(this.txtPath.Text))
            {
                vfpath = Path.GetDirectoryName(this.txtPath.Text);
                UZIP.UdyogUnzip(this.txtPath.Text, vfpath, "");
            }

            SqlStr = "Select a.*,b.Code as Entry_ty from imp_master a "+
                     " Left outer join (Select a1.Entry_ty as Code from Lcode a1 union all select a2.Code from MastCode a2) b " +  // Added by Sachin N. S. on 03/06/2015 for Bug-25651
                     " on a.Code=b.Code " +        // Added by Sachin N. S. on 03/06/2015 for Bug-25651
                     " Where charindex('<<" + this.vLocCode + ">>'" + ",ImpLocDet)>0 and Import=1 order by (Case when Category='Master' then 'a' else 'b' end),sortord"; //Changes by Archana K. on 25/10/12 for Bug-5837
            dsImpMaster = oDataAccess.GetDataSet(SqlStr, null, 20);

            foreach (DataRow dr in dsImpMaster.Tables[0].Rows)
            {
                // Added by Sachin N. S. on 03/06/2015 for Bug-25651 -- Start
                if (dr["Entry_ty"].ToString() == "NULL" || dr["Entry_ty"].ToString() == "")
                {
                    listView1.Items.Add(dr["Description"].ToString(), dr["Description"].ToString());
                    listView1.Items[listView1.Items.Count - 1].SubItems.Add(dr["Description"].ToString() + " is not defined in the " + dr["Category"].ToString() + " master");
                    listView1.Refresh();
                    continue;
                }
                // Added by Sachin N. S. on 03/06/2015 for Bug-25651 -- End

                //oDataAccess.BeginTransaction();//Added by Archana K. on 07/3/14 for Bug-20512 // Commented by Sachin N. S. on 08/04/2015 for Bug-25651
                vTables = dr["tables"].ToString();
                while (vTables.IndexOf("<<") > -1)
                {
                    oDataAccess.BeginTransaction(); // Added by Sachin N. S. on 08/04/2015 for Bug-25651
                    try//Added by Archana K on 07/03/14 for Bug-20512 
                    {
                        pos = vTables.IndexOf("<<");
                        vTables = vTables.Substring(pos + 2, vTables.Length - pos - 2);
                        pos = vTables.IndexOf("##");
                        vTable = vTables.Substring(0, pos);
                        vTable = vTable.ToLower();
                        listView1.Items.Add(vTable, vTable);
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add("Working...");
                        listView1.Refresh();
                        //Added by Archana K. on 12/12/13 for Bug-20512
                        if (vTable.ToUpper() == "GEN_SRNO" || vTable.ToUpper() == "IT_SRSTK" || vTable.ToUpper() == "IT_SRTRN") //modifyde by Kishor A. for bug-26960
                            fName = this.txtPath.Text.Replace(".zip", "") + "\\" + vTable + "_" + dr["code"].ToString().Trim() + ".xml";
                        else
                            //Added by Archana K. on 10/12/13 for Bug-20512 end
                            fName = this.txtPath.Text.Replace(".zip", "") + "\\" + vTable + ".xml";
                        sr = new StringReader(fName); //Added by Archana K. on 06/04/13 for Bug-5837 start
                        vImpTblNm = vTable;
                        pos = vTables.IndexOf("##ImpTbl<");
                        if (pos > 0)
                        {
                            // vImpTblNm = vTables.Substring(pos, vTables.Length - vTables.IndexOf(">"));
                            vImpTblNm = vTables.Substring(pos + 9, vTables.Length - pos - 9);
                            pos = vImpTblNm.IndexOf(">");
                            vImpTblNm = vImpTblNm.Substring(0, pos).ToLower();
                            fName = this.txtPath.Text.Replace(".zip", "") + "\\" + vImpTblNm + ".xml";
                        }

                        //if (File.Exists(fName.Replace(".xml", "_xsd.xml")))//Commented by Archana K. on 03/06/13 for Bug-5837 
                        if (File.Exists(fName))//Changed by Archana K. on 03/06/13 for Bug-5837
                        {
                            DataSet dataSet = new DataSet();
                            //dataSet.ReadXmlSchema(fName.Replace(".xml", "_xsd.xml"));//Commented by Archana K.on 06/04/13 for Bug-5837 
                            // Added by Archana K.on 06/04/13 for Bug-5837 start
                            dataSet.ReadXml(fName);//Changed by Archana K.on 06/04/13 for Bug-5837 
                            if (dataSet.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i <= dataSet.Tables.Count - 1; i++)
                                {
                                    S = dataSet.Tables[0].Rows[i]["DataExport1"].ToString();
                                    index = S.IndexOf("#", StringComparison.CurrentCultureIgnoreCase);
                                    ELoc_code = S.Substring(0, (index));
                                    if (vLocCode != ELoc_code)
                                    {
                                        MessageBox.Show("Import file not from the selected location..", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                        this.btnPath.Focus();
                                        listView1.Items.Clear();
                                        return;
                                    }
                                }
                                //Birendra : Bug-20512 on 6/02/2014:Start:
                                //if (transactioncheck == false && dr["category"].ToString().ToLower().Trim() == "transaction")
                                //    transactioncheck = true;
                                //Birendra : Bug-20512 on 6/02/2014:End:
                            }
                            else
                            {
                                continue;
                            }
                            // Added by Archana K.on 06/04/13 for Bug-5837 end
                            string TargetTableName = vTable + "_" + Path.GetFileName(this.txtPath.Text).Replace(".zip", "").Replace(" ", "").Replace("-", "");
                            delTargetTbl.Add(TargetTableName);
                            string DDL = SqlTableCreator.GetCreateFromDataTableSQL(TargetTableName, dataSet.Tables[0]);
                            string Finalsql = "IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[" + TargetTableName + "]') AND type in (N'U')) \n";
                            Finalsql += "Begin \n";
                            Finalsql += "Drop table " + TargetTableName + " \n";
                            Finalsql += "End \n";
                            //oDataAccess.ExecuteSQLStatement(Finalsql, null, 30, true);//Commented by Archana K. on 31/05/13 for Bug-5837
                            oDataAccess.ExecuteSQLStatement(Finalsql, null, Timeout, true);//changed by Archana K. on 31/05/13 for Bug-5837
                            //oDataAccess.ExecuteSQLStatement(DDL, null, 30, true);//Commented by Archana K. on 31/05/13 for Bug-5837
                            oDataAccess.ExecuteSQLStatement(DDL, null, Timeout, true);//Changed by Archana K. on 31/05/13 for Bug-5837
                            /// Above : Read Schema to create table

                            if (File.Exists(fName))
                            {
                                //SqlStr = "execute Usp_Imp_TempTableCreation '" + fName + "','" + vImpTblNm + "','" + vTable + "_" + Path.GetFileName(this.txtPath.Text).Replace(".zip", "").Replace(" ", "") + "'";
                                //SqlStr = "execute Usp_Imp_TempTableCreation '" + fName + "','" + dataSet.Tables[0].TableName + "','" + dataSet.Tables[0].TableName + "_" + Path.GetFileName(this.txtPath.Text).Replace(".zip", "").Replace(" ", "").Replace("-","") + "'";
                                SqlStr = "execute Usp_DataImport_TempTableCreation'" + fName + "','" + dataSet.Tables[0].TableName + "','" + TargetTableName + "'";
                                //oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);//Commented by Archana K.on 31/05/13 for Bug-5837
                                oDataAccess.ExecuteSQLStatement(SqlStr, null, Timeout, true);//Changed by Archana K. on 31/05/13 for Bug-5837 
                            }
                            ///Here: Call corosponding SP to update actual Tables.

                            SqlStr = " execute " + dr["sp_name"].ToString().Trim() + " '" + dr["code"].ToString().Trim() + "','";
                            SqlStr += Path.GetFileName(this.txtPath.Text).Replace(".zip", "").Replace(" ", "").Replace("-", "").Trim() + "','";

                            SqlStr += vLocCode + "','";

                            SqlStr += vTable + "'";

                            //oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);//Commented by Archana K. on 31/05/13 for Bug-5837
                            oDataAccess.ExecuteSQLStatement(SqlStr, null, Timeout, true);//Changed by Archana K. on 31/05/13 for Bug-5837
                            listView1.Items[listView1.Items.Count - 1].SubItems[1].Text = "Finished.";
                            listView1.TopItem = listView1.Items[listView1.Items.Count - 1];
                            listView1.Refresh();

                            //DataSet ds = new DataSet();
                            //System.IO.FileStream fsReadXml = new System.IO.FileStream(fName, System.IO.FileMode.Open);
                            //string collist = string.Empty;
                            //string ColDataType = string.Empty, vDataType = string.Empty;
                            //ds.ReadXml(fsReadXml);
                            //SqlStr = string.Empty;
                            //this.mthTableScema(ds, vTable, ref SqlStr);
                        }
                        else
                        {
                            listView1.Items[listView1.Items.Count - 1].SubItems[1].Text = "No Records to Import.";
                            listView1.TopItem = listView1.Items[listView1.Items.Count - 1];
                            listView1.Refresh();
                        }

                        //if (File.Exists(fName))
                        //{
                        //    //SqlStr = "execute Usp_Imp_TempTableCreation '" + fName + "','" + vImpTblNm + "','" + vTable + "_" + Path.GetFileName(this.txtPath.Text).Replace(".zip", "").Replace(" ", "") + "'";
                        //    SqlStr = "execute Usp_Imp_TempTableCreation '" + fName + "','" + vImpTblNm + "','" + vTable + "_" + Path.GetFileName(this.txtPath.Text).Replace(".zip", "").Replace(" ", "") + "'";
                        //    oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);
                        //}
                        //Added by Archana K. on 07/03/14 for Bug-20512 start  
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error in inserting records in the database.\n" + ex.Message, this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        oDataAccess.RollbackTransaction();
                        //Birendra: Bug-14505 on 10/04/2014:Start:
                        this.Close();
                        //return;
                        //Birendra: Bug-14505 on 10/04/2014:End:
                    }
                    //Added by Archana K. on 07/03/14 for Bug-20512 end
                    oDataAccess.CommitTransaction();//Added by Archana K. on 07/03/14 for Bug-20512
                }

                //string[] filenames;
                //filenames = Directory.GetFiles(this.txtPath.Text.Trim().Replace(".zip",""));
                //foreach (string Fil in filenames) // for each file, generate a zipentry
                //{
                //    MessageBox.Show(Fil);
                //}

                //Birendra : Bug-20512 on 6/02/2014:Start:
                //if (transactioncheck == true)
                //{
                //    oDataAccess.ExecuteSQLStatement("execute Usp_It_balw_regenerate", null, Timeout, true);
                //    oDataAccess.ExecuteSQLStatement("execute Usp_It_bal_regenerate", null, Timeout, true);
                //}
                //Birendra : Bug-20512 on 6/02/2014:End:
                //oDataAccess.CommitTransaction();//Added by Archana K. on 07/03/14 for Bug-20512 // Commented by Sachin N. S. on 08/04/2015 for Bug-25651
                //Added by Archana K. 31/05/13 for Bug-5837 start
                for (int i = 0; i < delTargetTbl.Count; i++)
                {
                    string Finalsql = "IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[" + delTargetTbl[i] + "]') AND type in (N'U')) \n";
                    Finalsql += "Begin \n";
                    Finalsql += "Drop table " + delTargetTbl[i] + " \n";
                    Finalsql += "End \n";
                    //oDataAccess.ExecuteSQLStatement(Finalsql, null, 30, true);//commented by Archana K. on 31/05/13 for Bug-5837
                    oDataAccess.ExecuteSQLStatement(Finalsql, null, Timeout, true);//changed by Archana K. on 31/05/13 for Bug-5837 
                }
                delTargetTbl.Clear();
                //Added by Archana K. 31/05/13 for Bug-5837 end

                this.Text = this.pFrmCaption + " : Importing Finished";
                //MessageBox.Show("Importing Finished");    // Commented by Sachin N. S. on 08/04/2015 for Bug-25651
            }
            Directory.Delete(this.txtPath.Text.Replace(".zip", ""), true);      // Added by Sachin N. S. on 11/04/2015 for Bug-25651
            MessageBox.Show("Importing Finished", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);      // Added by Sachin N. S. on 08/04/2015 for Bug-25651
        }

        private void mthTableScema(DataSet ds,string TableName,ref String SqlStr)
        { 
           // Byte        SByte  TimeSpan  
            string vType =string.Empty;
            SqlStr = "create table " + TableName+"(";
            foreach(DataColumn dc in ds.Tables[0].Columns)
            {
                vType = dc.DataType.Name;
                SqlStr = SqlStr + dc.ColumnName;
                switch(vType)
                {
                    case "Boolean":
                        SqlStr = SqlStr + " bit ";
                         break;
                    case "Char":
                         SqlStr = SqlStr + " varchar (" + dc.MaxLength.ToString().Trim() + ")";
                         break;   
                    case "String":
                         SqlStr = SqlStr + " varchar (" + dc.MaxLength.ToString().Trim() + ")";
                         break;
                    case "DateTime":
                         SqlStr = SqlStr + " smalldatetime";
                         break;
                    case "Int16":
                         SqlStr = SqlStr + " int";
                         break;
                    case "Int32":
                         SqlStr = SqlStr + " int";
                         break;
                    case "Int64":
                         SqlStr = SqlStr + " int";
                         break;
                    case "Single":
                         SqlStr = SqlStr + " Decimal (18,6)";
                         break;  
                    case "Double":
                         SqlStr = SqlStr + " Decimal (18,6)";
                         break;  
                    case "Decimal":
                         SqlStr = SqlStr + " Decimal (18,6)";
                         break;
                }//switch(vType)
            }
            SqlStr = SqlStr = SqlStr + " )";

        }

        private void frmImpMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            mDeleteProcessIdRecord();
        }
    }
}
