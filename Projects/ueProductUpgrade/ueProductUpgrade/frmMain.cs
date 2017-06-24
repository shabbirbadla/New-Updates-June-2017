using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Xml;
using System.Threading;
using System.Reflection;
using System.Globalization;
using System.Diagnostics; //Added by sandeep for bug-18141 on 14-SEP-13
using System.IO;
using DataAccess_Net; //Added by sandeep for bug-18141 on 14-SEP-13
using uBaseForm;  //Added by sandeep for bug-18141 on 14-SEP-13
using ueconnect;


namespace ueProductUpgrade
{
    //public partial class frmMain : Form //commented by sandeep for bug-18141 on 14-SEP-13 
    public partial class frmMain : uBaseForm.FrmBaseForm //Added by sandeep for bug-18141 on 14-SEP-13 
    {
        ReadSetting readIni;
        SqlConnection conn;
        DataSet ds = new DataSet();
        clsConnect oConnect;
        string[] vu10Prod = new string[] { "VudyogSDK", "VudyogPRO", "VudyogENT", "VudyogSTD", "10USquare", "10iTAX", "USquare10", "iTAX10" };
        ArrayList stringList = new ArrayList();
        string appPath = string.Empty;
        string MacId = string.Empty;
        string ErrorMsg = string.Empty;
        string RegProd = string.Empty;
        DataTable UpdateTbl;
        string LogFile = string.Empty;
        DataTable LogTable;
        ProgressBar pbar;
        Int32 totRecords = 0;
        Int32 unFetRecords = 0;
        float ratio;
        string vuMess = string.Empty;
        string ProcessStart = string.Empty;
        string ProcessEnd = string.Empty;
        //Added by sandeep for bug-18141 on 14-SEP-13-->Start 
        private string vLocCode = string.Empty;
        private DataSet dsCompany;
        DataAccess_Net.clsDataAccess oDataAccess;
        private String cAppPId, cAppName;
        int vFileNo;
        bool IsExitCalled;
        string vIE_LocCode;
        int ListViewItemCount;
        const int Timeout = 5000;
        //Added by sandeep for bug-18141 on 14-SEP-13-->End 
        #region Properties
        private string _CurrentApplication = string.Empty;
        public string CurrentApplication
        {
            get { return _CurrentApplication; }
            set { _CurrentApplication = value; }
        }
        private string _CurrentAppFile;
        public string CurrentAppFile
        {
            get { return _CurrentAppFile; }
        }
        private string _UpgradeAppFile;
        public string UpgradeAppFile
        {
            get { return _UpgradeAppFile; }
        }
        private string _UpgradeApplication = string.Empty;
        public string UpgradeApplication
        {
            get { return _UpgradeApplication; }
            set { _UpgradeApplication = value; }
        }
       
        #endregion
        //commented by sandeep for bug-18141 on 14-SEP-13-->S 
        //public frmMain()
        //{
        //    InitializeComponent();
        //}
        //commented by sandeep for bug-18141 on 14-SEP-13-->E 
        //added by sandeep for bug-18141 on 14-SEP-13---->start      
        public frmMain(string[] args)
        {
            InitializeComponent();

            ListViewItemCount = 0;
            this.pFrmCaption = "Product Upgrade";
            this.Text = this.pFrmCaption;

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
            this.pPApplPID = Convert.ToInt32(args[10]);
         
            this.pPApplCode = args[11];


            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();

            dsCompany = new DataSet();

            string strSQL = "select * from vudyog..Co_Mast where compid=" + this.pCompId;
            
            dsCompany = oDataAccess.GetDataSet(strSQL, null, 20);

            this.mInsertProcessIdRecord();

        }
        private void mInsertProcessIdRecord()
        {
            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "ueProductUpgrade.EXE";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            sqlstr = " insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString("MM/dd/yyyy") + "','" + this.pPApplName + "'," + this.pPApplPID.ToString() + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";        
            
            oDataAccess.ExecuteSQLStatement(sqlstr, null, Timeout, true);
        }
        private void mDeleteProcessIdRecord()
        {
            DataSet dsData = new DataSet();
            string sqlstr;
            sqlstr = " Delete from vudyog..ExtApplLog where pApplNm='" + this.pPApplName + "' and pApplId=" + this.pPApplPID + " and cApplNm= '" + cAppName + "' and cApplId= " + cAppPId;            
            oDataAccess.ExecuteSQLStatement(sqlstr, null, Timeout, true);
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
                IsExitCalled = true;
                Application.Exit();
            }
        }




        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.mDeleteProcessIdRecord();
        }

        //added by sandeep for bug-18141 on 14-SEP-13-->End


        #region Method to Implement
        public void SetNavigationVisibility()
        {
            if (this.tabControl1.SelectedIndex == 0)
            {
                this.btnBack.Enabled = false;
                this.btnNext.Enabled = true;
                this.btnFinish.Enabled = false;
            }
            else if (this.tabControl1.SelectedIndex == (this.tabControl1.TabCount - 1))
            {
                this.btnBack.Enabled = true;
                this.btnNext.Enabled = false;
                this.btnFinish.Enabled = true;
            }
            else
            {
                this.btnBack.Enabled = true;
                this.btnNext.Enabled = true;
                this.btnFinish.Enabled = false;
            }
            this.Refresh();
        }
        public void SetImages()
        {
            readIni = new ReadSetting(appPath);
            this.Icon = new Icon(appPath + "\\BMP\\UEICON.ICO");
            foreach (string fileName in Directory.GetFiles(appPath + "\\BMP\\"))
            {
                FileInfo f = new FileInfo(fileName);
                if (f.Name.ToUpper().IndexOf("_" + readIni.AppFile.ToUpper() + ".JPG") > 0)
                    this.picImage.Image = Image.FromFile(f.FullName);
            }
            //_CurrentApplication = readIni.AppTitle;       //Commented By Shrikant S. on 18/10/2012 
            _CurrentAppFile = readIni.AppFile;
            _CurrentApplication = readIni.AppTitle;
        }
        public string GetCurrentAppName(string appName)      //Added By Shrikant S. on 18/10/2012 
        {
            string result = string.Empty;
            try
            {
                conn = new SqlConnection(readIni.ConnectionString);
                conn.Open();
                SqlCommand lcmd = new SqlCommand("Select Top 1 AppDesc From ApplicationDet Where LTrim(RTrim(AppName))='" + appName.Trim() + "'",conn);
                result = (string)lcmd.ExecuteScalar();
                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show(ex.Message, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }
        public void SetStyleToGrid()
        {
            dgvComp1.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dgvComp2.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dgvProduct.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dgvProductUpgrade.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        public void BindData()
        {
            conn = new SqlConnection(readIni.ConnectionString);
            string sqlStr = string.Empty;
            sqlStr = "Select CompId,Co_Name,Passroute=rtrim(convert(varchar(250),Passroute)),Passroute1=rtrim(convert(varchar(250),Passroute1)),add1,add2,add3,area,zone,city,zip,state,country,email,dbname,foldername,sta_dt,end_dt From vudyog..Co_Mast Where Enddir='' and com_type=''";
            SqlDataAdapter da = new SqlDataAdapter(sqlStr, conn);
            ds = new DataSet();
            da.Fill(ds, "Co_Mast_vw");

            dgvComp1.DataSource = ds.Tables["Co_Mast_vw"];
            dgvComp1.Columns[0].DataPropertyName = "Co_Name";
            dgvComp2.DataSource = ds.Tables["Co_Mast_vw"];
            dgvComp2.Columns[0].DataPropertyName = "Co_Name";
            GetProd();
            GetCompanyProducts();

            dgvProduct.DataSource = ds.Tables["cProduct_vw"];
            dgvProduct.Columns[0].DataPropertyName = "Sel";
            dgvProduct.Columns[1].DataPropertyName = "CProductName";

            dgvProductUpgrade.DataSource = ds.Tables["Product_vw"];
            dgvProductUpgrade.Columns[0].DataPropertyName = "Sel";
            dgvProductUpgrade.Columns[1].DataPropertyName = "cProdName";
            SetDefaultProduct();

        }

        public void SetDefaultProduct()
        {
            for (int i = 0; i < ds.Tables["Product_vw"].Rows.Count; i++)
            {
                for (int j = 0; j < ds.Tables["cProduct_vw"].Rows.Count; j++)
                {
                    if (ds.Tables["Product_vw"].Rows[i]["cProdCode"].ToString() == ds.Tables["cProduct_vw"].Rows[j]["cProduct"].ToString() && ds.Tables["Product_vw"].Rows[i]["CompId"].ToString() == ds.Tables["cProduct_vw"].Rows[j]["CompId"].ToString())
                    {
                        ds.Tables["Product_vw"].Rows[i]["Sel"] = true;
                    }
                }
            }
        }
        public string GetProductDescription(string Prodcode)
        {

            string retValue = string.Empty;
            
            if (vu10Prod.Contains(this.CurrentAppFile))
            {
                for (int i = 0; i < ds.Tables["Product_vw"].Rows.Count; i++)
                {
                    if (ds.Tables["Product_vw"].Rows[i]["cProdCode"].ToString().Trim() == Prodcode.Trim())      // 26/04/2014 trim added to condition by Shrikant S.
                    {
                        retValue = ds.Tables["Product_vw"].Rows[i]["cProdName"].ToString();
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < ds.Tables["Product_vw"].Rows.Count; i++)
                {
                    if (ds.Tables["Product_vw"].Rows[i]["cMainProdCode"].ToString() == Prodcode)
                    {
                        retValue = ds.Tables["Product_vw"].Rows[i]["cProdCode"].ToString();
                        break;
                    }
                }
            }
            return retValue;
        }
        public void GetProd()
        {
            conn = new SqlConnection(readIni.ConnectionString);
            string sqlStr = string.Empty;
            sqlStr = "Select Sel=convert(bit,0),a.Enc1,a.Enc2,EValue=space(2000),cProdName=space(100),cProdCode=space(10),cCmbNotAlwd=space(250),cModDep=space(250),cMainProdCode=space(250),nProdType =convert(bit,0),b.CompId  From Vudyog..ModuleMast a, Vudyog..co_Mast b Where a.Enc1=@pEnc and b.enddir='' ";       

            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand(sqlStr, conn);
            da.SelectCommand = cmd;
            cmd.Parameters.Add("@pEnc", SqlDbType.VarChar).Value = oConnect.RetAppEnc(this.CurrentAppFile);

            da.Fill(ds, "Product_vw");
            for (int i = 0; i < ds.Tables["Product_vw"].Rows.Count; i++)
            {
                ds.Tables["Product_vw"].Rows[i]["Evalue"] = oConnect.RetModuleDec(ds.Tables["Product_vw"].Rows[i]["Enc2"].ToString());
                for (int j = 1; j <= 5; j++)
                {
                    int firstKeyIndex = ds.Tables["Product_vw"].Rows[i]["Evalue"].ToString().IndexOf("~*" + j.ToString() + "*~");
                    int secondKeyIndex = ds.Tables["Product_vw"].Rows[i]["Evalue"].ToString().IndexOf("~*" + j.ToString() + "*~", firstKeyIndex + 1);
                    string lstring = ds.Tables["Product_vw"].Rows[i]["Evalue"].ToString().Substring(firstKeyIndex + 5, secondKeyIndex - firstKeyIndex - 5);
                    switch (j)
                    {
                        case 1:
                            ds.Tables["Product_vw"].Rows[i]["cProdCode"] = lstring.Trim();
                            break;
                        case 2:
                            ds.Tables["Product_vw"].Rows[i]["cProdName"] = lstring.Trim();
                            break;
                        case 3:
                            ds.Tables["Product_vw"].Rows[i]["cModDep"] = lstring.Trim();
                            break;
                        case 4:
                            ds.Tables["Product_vw"].Rows[i]["cCmbNotAlwd"] = lstring.Trim();
                            break;
                        case 5:
                            ds.Tables["Product_vw"].Rows[i]["cMainProdCode"] = lstring.Trim();
                            break;
                    }
                }
            }
        }
        private void GetCompanyProducts()
        {

            DataTable dtcProducts = new DataTable();
            dtcProducts.TableName = "cProduct_vw";
            ds.Tables.Add(dtcProducts);
            DataColumn sel = dtcProducts.Columns.Add("Sel", typeof(System.Boolean));
            DataColumn dccompId = dtcProducts.Columns.Add("CompId", typeof(int));
            DataColumn dcProduct = dtcProducts.Columns.Add("CProduct", typeof(System.String));
            DataColumn dcProductName = dtcProducts.Columns.Add("CProductName", typeof(System.String));
            DataRow dr;

            string cProduct = string.Empty;
            string cprod = string.Empty;
            string productName = string.Empty;

            for (int i = 0; i < ds.Tables["Co_mast_vw"].Rows.Count; i++)
            {
                if (!vu10Prod.Contains(this.CurrentAppFile))
                {
                    cProduct = Utilities.GetDecProductCode(ds.Tables["Co_mast_vw"].Rows[i]["Passroute"].ToString());
                    int j = 0;
                    while (j + 5 <= cProduct.Length)
                    {

                        cprod = cProduct.Substring(j, 5);
                        dr = dtcProducts.NewRow();
                        dr["Sel"] = true;
                        dr["Compid"] = ds.Tables["Co_mast_vw"].Rows[i]["CompId"];
                        dr["cProduct"] = cprod;
                        switch (cprod)
                        {

                            case "vuent":
                                productName = "Financial A/c";
                                break;
                            case "vupro":
                                productName = "Vat";
                                break;
                            case "vuexc":
                                productName = "Excise Manufacturing";
                                break;
                            case "vuexp":
                                productName = "Export";
                                break;
                            case "vuinv":
                                productName = "Inventory";
                                break;
                            case "vuord":
                                productName = "Order Processing";
                                break;
                            case "vubil":
                                productName = "Special Billing";
                                break;
                            case "vutex":
                                productName = "Excise Trading";
                                break;
                            case "vuser":
                                productName = "Service Tax";
                                break;
                            case "vuisd":
                                productName = "Input Service Distributor";
                                break;
                            case "vumcu":
                                productName = "Multi-Currency";
                                break;
                            case "vutds":
                                productName = "TDS";
                                break;
                        }
                        dr["cProductName"] = productName;
                        dtcProducts.Rows.Add(dr);
                        j = j + 5;
                    }
                }
                else
                {
                    cProduct = Utilities.GetDecProductCode(ds.Tables["Co_mast_vw"].Rows[i]["Passroute1"].ToString());
                    //cProduct = Utilities.GetDecProductCode(ds.Tables["Co_mast_vw"].Rows[i]["Passroute"].ToString() + (ds.Tables["Co_mast_vw"].Rows[i]["Passroute1"].ToString().Length>0?"," :"") + ds.Tables["Co_mast_vw"].Rows[i]["Passroute1"].ToString());
                    
                    if (cProduct.Trim().Length != 0)
                    {
                        string[] prod = cProduct.Split(',');
                        foreach (string xprod in prod)
                        {
                            string ProdDesc = string.Empty;
                            ProdDesc=GetProductDescription(xprod);
                            if (ProdDesc.Length != 0)
                            {
                                dr = dtcProducts.NewRow();
                                dr["Sel"] = true;
                                dr["Compid"] = ds.Tables["Co_mast_vw"].Rows[i]["CompId"];
                                dr["cProduct"] = xprod;
                                dr["cProductName"] = GetProductDescription(xprod);
                                dtcProducts.Rows.Add(dr);
                            }
                        }
                        
                    }
                }
            }


        }
        private string GenearateProductXml()
        {
            DataView dv = new DataView();
            string filename = txtCo_name.Text.Trim() + "-" + txtPartnerCode.Text.Trim() + "-UpgradeProducts.xml";

            using (XmlWriter writer = XmlWriter.Create(filename))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("CompanyData");
                writer.WriteElementString("UpgradeFrom", CurrentApplication);
                writer.WriteElementString("UpgradeTo", UpgradeApplication);
                for (int i = 0; i < ds.Tables["Co_mast_vw"].Rows.Count; i++)
                {
                    writer.WriteStartElement("Company" + (i + 1).ToString());
                    writer.WriteElementString("Name", ds.Tables["Co_mast_vw"].Rows[i]["Co_name"].ToString().Trim());
                    writer.WriteStartElement("Products");
                    dv = ds.Tables["Product_vw"].DefaultView;
                    dv.RowFilter = "Sel=1 and CompId=" + ds.Tables["Co_mast_vw"].Rows[i]["CompId"].ToString().Trim();
                    for (int j = 0; j < dv.Count; j++)
                    {
                        DataRow[] drs = ds.Tables["cProduct_vw"].Select("CompId=" + ds.Tables["Co_mast_vw"].Rows[i]["CompId"].ToString().Trim() + " and cProduct='" + dv[j]["cProdCode"].ToString().Trim() + "'");
                        if (drs.Length==0)
                            writer.WriteElementString("AddProduct", dv[j]["cProdName"].ToString().Trim());
                        else
                            writer.WriteElementString("Product", dv[j]["cProdName"].ToString().Trim());
                    }
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
                btnFinish.Enabled = false;
                btnBack.Enabled = false;
                //MessageBox.Show("Xml generated successfully....", CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //this.Close();
            }
            return filename;
        }
        private bool CheckRegisterMe()
        {
            DirectoryInfo dir = new DirectoryInfo(appPath);
            Array totalFile = dir.GetFiles();
            string m_registerMePath = string.Empty;
            for (int i = 0; i < totalFile.Length; i++)
            {
                string fname = Path.GetFileName(((System.IO.FileInfo[])(totalFile))[i].Name);
                if (Path.GetFileName(((System.IO.FileInfo[])(totalFile))[i].Name).ToUpper().Contains("REGISTER.ME"))
                {
                    m_registerMePath = Path.GetFileName(((System.IO.FileInfo[])(totalFile))[i].Name);
                    break;
                }
                //if (Path.GetExtension(((System.IO.FileInfo[])(totalFile))[i].Name).ToUpper() == ".ME")
                //{
                //    m_registerMePath = Path.GetFileName(((System.IO.FileInfo[])(totalFile))[i].Name);
                //    break;
                //}
            }
            if (m_registerMePath == string.Empty)
            {
                ErrorMsg = "Regiser.me file not found.";
                return false;
            }

            this.ReadRegisterMe(m_registerMePath);
            return true;
        }
        private void GenerateDeaultValue()
        {
            txtDBIP.Text = GetMachineDetails.IpAddress();
            txtDBName.Text = GetMachineDetails.MachineName();
            txtAppIP.Text = txtDBIP.Text;
            txtAppName.Text = txtDBName.Text;
            txtProductVer.Text = this.UpgradeAppFile;
            MacId = Utilities.ReverseString(GetMachineDetails.ProcessorId().Trim());
            if (this.CurrentAppFile == "VudyogSDK")
            {
                cboServiceVer.Items.Add("Developer Version");
            }
            else
            {
                cboServiceVer.Items.Add("Client Version");
                cboServiceVer.Items.Add("Support Version");
                cboServiceVer.Items.Add("Marketing Version");
                cboServiceVer.Items.Add("Educational Version");
            }
            txtNoComp.Text = ds.Tables["Co_mast_vw"].Rows.Count.ToString();
            cboServiceVer.SelectedIndex = 0;
            if (cboServiceVer.Text != "Client Version")
            {
                txtUsers.Text = "1";
                txtNoComp.Text = "1";
            }
        }
        private bool CheckValidation()
        {
            if (tabControl1.TabPages.Count == 4)
            {
                if (txtCo_name.TextLength == 0)
                {
                    MessageBox.Show("Please select the main company.", CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtCo_name.Focus();
                    return false;
                }
                if (txtPartnerCode.TextLength == 0)
                {
                    MessageBox.Show("Please fill partner code.", CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtPartnerCode.Focus();
                    return false;
                }
                if (txtEmail.Text.Length != 0)
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmail.Text.Trim(), "^([a-zA-Z0-9_\\-\\.]+)@([a-zA-Z0-9_\\-\\.]+)\\.([a-zA-Z]{2,5})$"))
                    {
                        MessageBox.Show("Email field is not in correct format!", CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtEmail.Focus();
                        return false;
                    }
                }
                if (txtUsers.TextLength == 0)
                {
                    MessageBox.Show("Please fill no. of users.", CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtUsers.Focus();
                    return false;
                }
                if (txtNoComp.TextLength == 0)
                {
                    MessageBox.Show("Please fill no. of companies.", CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtNoComp.Focus();
                    return false;
                }
            }
            return true;
        }
        private string GenerateCompanyXml()
        {
            string filename = string.Empty;
            filename = txtCo_name.Text.Trim() + "-" + txtPartnerCode.Text.Trim() + ".xml";
            using (XmlWriter writer = XmlWriter.Create(filename))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("VFPData");
                writer.WriteStartElement("curtemp");
                writer.WriteElementString("co_name", stringList[0].ToString());
                writer.WriteElementString("add1", stringList[1].ToString());
                writer.WriteElementString("add2", stringList[2].ToString());
                writer.WriteElementString("add3", stringList[3].ToString());
                writer.WriteElementString("area", stringList[4].ToString());
                writer.WriteElementString("zone", stringList[5].ToString());
                writer.WriteElementString("city", stringList[6].ToString());
                writer.WriteElementString("zip", stringList[7].ToString());

                writer.WriteElementString("state", stringList[8].ToString());
                writer.WriteElementString("country", stringList[9].ToString());
                writer.WriteElementString("servicetype", cboServiceVer.Text.Trim());
                writer.WriteElementString("noofuser", txtUsers.Text.Trim());
                writer.WriteElementString("noofco", txtNoComp.Text.Trim());
                writer.WriteElementString("contact", txtContact.Text.Trim());
                writer.WriteElementString("email", txtEmail.Text.Trim());
                writer.WriteElementString("product", oConnect.RetProduct().Trim());

                writer.WriteElementString("version", oConnect.RetVersion("UdProdId").Trim());
                int j = 1;
                for (int i = 0; i < ds.Tables["Co_mast_vw"].Rows.Count; i++)
                {
                    if (ds.Tables["Co_mast_vw"].Rows[i]["Co_name"].ToString().Trim().ToUpper() != stringList[0].ToString().ToUpper())
                    {
                        writer.WriteElementString("company" + (j++).ToString(), ds.Tables["Co_mast_vw"].Rows[i]["Co_name"].ToString().Trim());
                    }
                }
                writer.WriteElementString("prtnrcd", txtPartnerCode.Text.Trim());
                writer.WriteElementString("macid", MacId);
                writer.WriteElementString("dbsrvip", txtDBIP.Text.Trim());
                writer.WriteElementString("dbsrvnm", txtDBName.Text.Trim());
                writer.WriteElementString("apsrvip", txtAppIP.Text.Trim());
                writer.WriteElementString("apsrvnm", txtAppName.Text.Trim());
                writer.WriteElementString("prodversn", oConnect.RetShortCode("UdProdShortCode").Trim());
                writer.WriteElementString("prodsrno", "");
                writer.WriteElementString("lickeyno", "");
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
            return filename;
        }
        private void ReadRegisterMe(string fileName)
        {
            string[] objRegisterMe = (oConnect.ReadRegiValue(appPath)).Split('^');
            //Co_Name,Add1,Add2,Add3,area,zone,city,zip,state,country,email
            stringList.Add(objRegisterMe[0].ToString().Trim());
            stringList.Add(objRegisterMe[1].ToString().Trim());
            stringList.Add(objRegisterMe[2].ToString().Trim());
            stringList.Add(objRegisterMe[3].ToString().Trim());
            stringList.Add("");
            stringList.Add("");
            stringList.Add(objRegisterMe[4].ToString().Trim());
            stringList.Add(objRegisterMe[5].ToString().Trim());
            stringList.Add("");
            stringList.Add("");
            stringList.Add(objRegisterMe[6].ToString().Trim());
            stringList.Add("");
            stringList.Add("");

            txtCo_name.Text = objRegisterMe[0].ToString().Trim();
            txtPartnerCode.Text = objRegisterMe[7].ToString().Trim();
            txtDBIP.Text = objRegisterMe[8].ToString().Trim();
            txtDBName.Text = objRegisterMe[9].ToString().Trim();
            txtAppIP.Text = objRegisterMe[10].ToString().Trim();
            txtAppName.Text = objRegisterMe[11].ToString().Trim();
            MacId = Utilities.ReverseString(objRegisterMe[12].ToString());
            txtNoComp.Text = objRegisterMe[13].ToString().Trim();
            txtUsers.Text = objRegisterMe[14].ToString().Trim();
            cboServiceVer.Text = objRegisterMe[15].ToString().Trim();
            if (objRegisterMe[16].ToString().IndexOf("PRO") > 0)
                RegProd = "VudyogPRO";
            else if (objRegisterMe[16].ToString().IndexOf("STD") > 0)
                RegProd = "VudyogSTD";
            else if (objRegisterMe[16].ToString().IndexOf("ENT") > 0)
                RegProd = "VudyogENT";
            else if (objRegisterMe[16].ToString().IndexOf("SDK") > 0)
                RegProd = "VudyogSDK";
            else if (objRegisterMe[16].ToString().IndexOf("10US") > 0)
                RegProd = "10USQUARE";
            else if (objRegisterMe[16].ToString().IndexOf("10IT") > 0)
                RegProd = "10ITAX";
        }

        private bool ExtractZipFiles()
        {
            if (Directory.Exists(appPath + "\\Database\\"))
            {
                //pbar.Show();
                //pbar.ShowProgress("Extracting Zip Files...", 0);
                UdyogZipUnzip.UdyoyZipUnZipUtility file = new UdyogZipUnzip.UdyoyZipUnZipUtility();
                if (File.Exists(Path.Combine(appPath + "\\Database\\", "Neio.Zip")))
                {
                    file.UdyogUnzip(Path.Combine(appPath + "\\Database\\", "Neio.Zip"), Path.Combine(appPath + "\\Database\\", "Neio"), "Neio.Dat", "");
                    if (!RestoreSqlData(appPath + "\\Database\\Neio\\", "Neio"))        //Added by Shrikant S. on 26/08/2014 for Bug-23814  
                    //if (!RestoreSqlData(Path.Combine(appPath + "\\Database\\Neio\\", "Neio.Dat"), "Neio"))    ////Commented by Shrikant S. on 26/08/2014 for Bug-23814
                    {
                        ErrorMsg = "Error occured while restoring Database Neio";
                        //MessageBox.Show("Error occured while restoring Database Neio", CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                }
                if (File.Exists(Path.Combine(appPath + "\\Database\\", "Nxio.Zip")))
                {
                    file.UdyogUnzip(Path.Combine(appPath + "\\Database\\", "Nxio.Zip"), Path.Combine(appPath + "\\Database\\", "Nxio"), "Nxio.Dat", "");
                    if (!RestoreSqlData(appPath + "\\Database\\Nxio\\", "Nxio"))             //Added by Shrikant S. on 26/08/2014 for Bug-23814
                    //if (!RestoreSqlData(Path.Combine(appPath + "\\Database\\Nxio\\", "Nxio.Dat"), "Nxio"))    ////Commented by Shrikant S. on 26/08/2014 for Bug-23814
                    {
                        //MessageBox.Show("Error occured while restoring Database Nxio", CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ErrorMsg = "Error occured while restoring Database Nxio";
                        return false;
                    }
                }
            }
            return true;
        }
        private bool RestoreSqlData(string fileFullPath, string dbName)
        {
            // Commented by Shrikant S. on 26/08/2014 for Bug-23814     //Start
            //try
            //{
            //    conn = new SqlConnection(readIni.ConnectionString);
            //    conn.Open();
            //    string strText = string.Empty;
            //    strText = "If Not Exists( Select [Name] From Master..SysDatabases Where [Name]='" + dbName + "') Begin Create Database " + dbName + "  ";
            //    strText = strText + " Restore Database " + dbName + " From Disk=N'" + fileFullPath + "' With Move '" + dbName + "' To '" + appPath + "\\Database\\" + dbName + ".mdf" + "', Move '" + dbName + "_Log' To '" + appPath + "\\Database\\" + dbName + ".ldf" + "',replace  End";
            //    SqlCommand cmd = new SqlCommand(strText, conn);
            //    cmd.CommandTimeout = 1500;
            //    cmd.ExecuteNonQuery();
            //    conn.Close();
            //}
            // Commented by Shrikant S. on 26/08/2014 for Bug-23814     //End
            // Added by Shrikant S. on 26/08/2014 for Bug-23814     //Start
            try
            {
                string mdfFilenm = string.Empty;
                string ldfFilenm = string.Empty;
                string restorePath = string.Empty;

                conn = new SqlConnection(readIni.ConnectionString);
                conn.Open();
                string strText = string.Empty;
                strText = "If Not Exists( Select [Name] From Master..SysDatabases Where [Name]='" + dbName + "') Begin Create Database " + dbName + " End  ";
                SqlCommand cmd = new SqlCommand(strText, conn);
                cmd.ExecuteNonQuery();
                restorePath = (readIni.ItaxPath.Length > 0 ? readIni.ItaxPath : fileFullPath);

                strText = "RESTORE FILELISTONLY FROM DISK = N'"+Path.Combine(restorePath,dbName)+".Dat'";
                cmd.CommandText = strText;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt=new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    mdfFilenm = dt.Rows[0]["LogicalName"].ToString();
                    ldfFilenm = dt.Rows[1]["LogicalName"].ToString();
                }
                strText = "RESTORE DATABASE " + dbName + " From Disk=N'" + Path.Combine(restorePath, dbName) + ".Dat' with Move N'" + mdfFilenm + "' To N'" + Path.Combine(restorePath, dbName) + ".mdf',";
                strText = strText + " " + "Move N'" + ldfFilenm + "' To N'" + Path.Combine(restorePath, dbName) + ".ldf',replace";
                cmd.CommandText = strText;
                //strText = strText + " Restore Database " + dbName + " From Disk=N'" + fileFullPath + "' With Move '" + dbName + "' To '" + appPath + "\\Database\\" + dbName + ".mdf" + "', Move '" + dbName + "_Log' To '" + appPath + "\\Database\\" + dbName + ".ldf" + "',replace  End";
                cmd.CommandTimeout = 1500;
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            // Added by Shrikant S. on 26/08/2014 for Bug-23814     //End
            catch (Exception ex)
            {
                conn.Close();
                return false;
            }
            return true;
        }
        private void GetDefaultTables()
        {
            Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream("ueProductUpgrade.BasicTable.xml");
            ds.ReadXml(s, XmlReadMode.Auto);
        }
        private void ReadInfFile()
        {
            this.ShowProcess("Reading Inf File..");
            StringReader sr = new StringReader(oConnect.RetIniTable(appPath));
            ds.ReadXml(sr);
        }
        public bool GetFeatures()
        {
            string sqlStr = string.Empty;
            this.ShowProcess("Genearating Features..");
            try
            {
                sqlStr = "Select CompId=Convert(int,0), Enc=Convert(Varchar(254),Enc),PEnc=Convert(Varchar(254),PEnc),vcEnc=convert(Varchar(5000),'') "
                + ",OptionType=Convert(Varchar(254),''),featureid=Convert(Varchar(254),''),sfeatureid=Convert(Varchar(254),'') "
                + " ,ProdCode=Convert(Varchar(254),''),prodver=Convert(Varchar(254),''),servicever=Convert(Varchar(254),'') "
                + " ,fType=Convert(Varchar(254),''),OptionName=Convert(Varchar(254),''),IntFeatureId=Convert(Int,0) From Vudyog..ProdDetail ";
                SqlDataAdapter lda = new SqlDataAdapter();
                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                lda.SelectCommand = cmd;
                lda.Fill(ds, "FeatureDet_vw");

                for (int i = 0; i < ds.Tables["FeatureDet_vw"].Rows.Count; i++)
                {
                    ds.Tables["FeatureDet_vw"].Rows[i]["vcEnc"] = oConnect.RetFeatureDec(ds.Tables["FeatureDet_vw"].Rows[i]["Enc"].ToString());
                    for (int j = 0; j < 8; j++)
                    {
                        int firstKeyIndex = (j == 0 ? 0 : ds.Tables["FeatureDet_vw"].Rows[i]["vcEnc"].ToString().IndexOf("~*" + (j - 1).ToString() + "*~"));
                        int secondKeyIndex = ds.Tables["FeatureDet_vw"].Rows[i]["vcEnc"].ToString().IndexOf("~*" + j.ToString() + "*~", firstKeyIndex + 1);
                        string lstring = string.Empty;
                        if (j == 0)
                            lstring = ds.Tables["FeatureDet_vw"].Rows[i]["vcEnc"].ToString().Substring(firstKeyIndex, secondKeyIndex);
                        else
                            lstring = ds.Tables["FeatureDet_vw"].Rows[i]["vcEnc"].ToString().Substring(firstKeyIndex + 5, secondKeyIndex - firstKeyIndex - 5);
                        switch (j)
                        {
                            case 0:
                                ds.Tables["FeatureDet_vw"].Rows[i]["OptionType"] = lstring;
                                break;
                            case 1:
                                ds.Tables["FeatureDet_vw"].Rows[i]["featureid"] = lstring;
                                ds.Tables["FeatureDet_vw"].Rows[i]["IntFeatureId"] = Convert.ToInt32(lstring);
                                break;
                            case 2:
                                ds.Tables["FeatureDet_vw"].Rows[i]["sfeatureid"] = lstring;
                                break;
                            case 3:
                                ds.Tables["FeatureDet_vw"].Rows[i]["ProdCode"] = lstring;
                                break;
                            case 4:
                                ds.Tables["FeatureDet_vw"].Rows[i]["prodver"] = lstring;
                                break;
                            case 5:
                                ds.Tables["FeatureDet_vw"].Rows[i]["servicever"] = lstring;
                                break;
                            case 6:
                                ds.Tables["FeatureDet_vw"].Rows[i]["fType"] = lstring;
                                break;
                            case 7:
                                ds.Tables["FeatureDet_vw"].Rows[i]["OptionName"] = lstring;
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMsg = "Error Occured while retrieving features." + Environment.NewLine + ex.Message;
                return false;
            }
            return true;
        }

        private bool CheckRegisterMeValidation()
        {
            this.CheckRegisterMe();
            if (Utilities.ReverseString(MacId) != GetMachineDetails.ProcessorId())        
            {
                ErrorMsg = "Please check, invalid Register.me file. ";
                return false;
            }
            return true;
        }
        private bool CheckIniValidation()
        {
            if (ds.Tables["CustInfo_vw"].Rows.Count > 0)                  
            {
                if (ds.Tables["CustInfo_vw"].Rows[0]["macid"].ToString() != GetMachineDetails.ProcessorId())
                {
                    ErrorMsg = "Please check, invalid Info.inf file. ";
                    return false;
                }
            }
            else
            {
                ErrorMsg = "Info.inf file not found...!!! ";
                return false;
            }
            return true;
        }
        private bool DoProductUpgrade()
        {
            UpdateTbl = new DataTable("Update_vw");
            DataRow[] ldrs;
            DataRow updateRow;
            UpdateTbl.Columns.Add("CompId", typeof(System.Int32));
            UpdateTbl.Columns.Add("Co_name", typeof(System.String));
            UpdateTbl.Columns.Add("OptType", typeof(System.String));
            UpdateTbl.Columns.Add("SqlQuery", typeof(System.String));
            UpdateTbl.Columns.Add("encValue", typeof(System.String));
            UpdateTbl.Columns.Add("OptName", typeof(System.String));

            if (ds.Tables["Product_vw"] != null)
            {
                ds.Tables["Product_vw"].Clear();
            }
            if (ds.Tables["cProduct_vw"] != null)
            {
                ds.Tables["cProduct_vw"].Clear();
            }
            string[] lProducts;
            try
            {
                //GetFeatures();

                //pbar.Show();
                this.ShowProcess("Genearating Scripts..");
                for (int k = 0; k < ds.Tables["Co_mast_vw"].Rows.Count; k++)
                {
                    for (int i = 0; i < ds.Tables["CustInfo_vw"].Rows.Count; i++)
                    {
                        string defaData = (ds.Tables["CustInfo_vw"].Rows[i]["Prodcd"].ToString().IndexOf("vutex") >= 0 ? "NXIO" : "NEIO");
                        string compRights = ds.Tables["Co_mast_vw"].Rows[k]["Co_name"].ToString().ToUpper().Trim() + "(" + Convert.ToDateTime(ds.Tables["Co_mast_vw"].Rows[k]["sta_dt"]).Year.ToString() + "-" + Convert.ToDateTime(ds.Tables["Co_mast_vw"].Rows[k]["end_dt"]).Year.ToString() + ")[ADMINISTRATOR]";
                        if (ds.Tables["Co_mast_vw"].Rows[k]["Co_name"].ToString().ToUpper().Trim() == ds.Tables["CustInfo_vw"].Rows[i]["clientnm"].ToString().ToUpper().Trim())
                        {
    
                            //lProducts = ds.Tables["CustInfo_vw"].Rows[i]["ProdCd"].ToString().Split(',');         //Commented by Shrikant S. on 11/08/2014 for Bug-23814
                            string prodlist = ds.Tables["CustInfo_vw"].Rows[i]["ProdCd"].ToString() + "," + ds.Tables["CustInfo_vw"].Rows[i]["addProdCd"].ToString();       //Added by Shrikant S. on 11/08/2014 for Bug-23814
                            lProducts = prodlist.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);              //Added by Shrikant S. on 11/08/2014 for Bug-23814

                            string lstProducts = string.Empty;
                            for (int l = 0; l < lProducts.Count(); l++)
                            {
                                lstProducts = lstProducts + "'" + lProducts[l] + "',";
                            }
                            lstProducts = (lstProducts.Length != 0 ? lstProducts.Substring(0, lstProducts.Length - 1) : "");
                            ldrs = ds.Tables["FeatureDet_vw"].Select(" ProdCode In (" + lstProducts + ") and Prodver='" + RegProd + "' and servicever='" + cboServiceVer.Text.ToUpper().Trim() + "'");
                            foreach (DataRow ldr in ldrs)
                            {
                                updateRow = UpdateTbl.NewRow();
                                updateRow["CompId"] = ds.Tables["Co_mast_vw"].Rows[k]["CompId"];
                                updateRow["Co_Name"] = ds.Tables["Co_mast_vw"].Rows[k]["Co_Name"];
                                updateRow["OptType"] = ldr["OptionType"];
                                updateRow["OptName"] = ldr["OptionName"];
                                updateRow["encValue"] = oConnect.RetEncValue(ldr["FeatureId"].ToString().Trim() + "~*0*~" + ldr["sfeatureid"].ToString().Trim() + "~*1*~" + ldr["OptionName"].ToString().Trim() + "~*2*~", "Udencyogprod");
                                updateRow["SqlQuery"] = this.GenInsertUpdate(ds.Tables["Co_mast_vw"].Rows[k]["dbname"].ToString().Trim(), ldr[4].ToString(), ldr[11].ToString(), defaData, compRights);
                                UpdateTbl.Rows.Add(updateRow);
                            }
                        }
                    }
                }
                //pbar.Dispose();
            }
            catch (Exception ex)
            {
                //pbar.Dispose();
                this.ErrorMsg = ex.Message;
                return false;
            }
            return true;
        }
        private string GenInsertUpdate(string companyDB, string optType, string optValue, string defaultDataNm, string compforRights)
        {
            string retStr = string.Empty;
            string cRights = string.Empty;
            string cRoles = string.Empty;
            switch (optType)
            {

                case "MENU":
                    cRights = Utilities.GetDecoder("IYCYDYPYVY", false).Trim();
                    cRoles = Utilities.GetDecoder("ADMINISTRATOR", true).Trim();
                    retStr = "Execute Usp_Int_GenFeature '" + companyDB + "','" + defaultDataNm + "','" + optType + "','" + optValue + "',@encValue,'" + compforRights + "','" + cRights + "','" + cRoles + "'";
                    break;
                case "TRANSACTION":
                    retStr = "Execute Usp_Int_GenFeature '" + companyDB + "','" + defaultDataNm + "','" + optType + "','" + optValue + "',@encValue,'" + compforRights + "','" + cRights + "','" + cRoles + "'";
                    break;
                case "REPORT":
                    retStr = "Execute Usp_Int_GenFeature '" + companyDB + "','" + defaultDataNm + "','" + optType + "','" + optValue + "',@encValue,'" + compforRights + "','" + cRights + "','" + cRoles + "'";
                    break;
            }
            return retStr;
        }

        private bool ExecuteScripts()
        {
            string companyFolder = string.Empty;
            string LogFile = string.Empty;
            SqlParameter param;
            SqlCommand lcmd;
            string SQL = string.Empty;
            int result = 0;
            LogTable = new DataTable("Log_vw");

            LogTable.Columns.Add("Co_name", typeof(System.String));
            LogTable.Columns.Add("OptType", typeof(System.String));
            LogTable.Columns.Add("OptName", typeof(System.String));
            LogTable.Columns.Add("LogDesc", typeof(System.String));
            //UpdateTbl.WriteXml("d:\\abcd.xml");
            DataView ldv = UpdateTbl.DefaultView;
            if (UpdateTbl != null)
            {
                UpdateTbl = null;
            }

            //pbar.Show();
            try
            {
                for (int i = 0; i < ds.Tables["CO_mast_vw"].Rows.Count; i++)
                {
                    for (int k = 0; k < ds.Tables["CustInfo_vw"].Rows.Count; k++)
                    {
                        if (ds.Tables["CO_mast_vw"].Rows[i]["Co_name"].ToString().ToUpper().Trim() == ds.Tables["CustInfo_vw"].Rows[k]["clientnm"].ToString().ToUpper().Trim())
                        {
                            totRecords = 0;
                            unFetRecords = 0;

                            ProcessStart = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                            companyFolder = ds.Tables["CO_mast_vw"].Rows[i]["foldername"].ToString().Trim();
                            CreateLogFile(ds.Tables["CO_mast_vw"].Rows[i]["co_name"].ToString().Trim(), companyFolder);

                            ldv.RowFilter = "Co_name='" + ds.Tables["CO_mast_vw"].Rows[i]["Co_Name"].ToString().Trim() + "'";
                            ldv.Sort = "Co_name,OptType";
                            totRecords = ldv.Table.Rows.Count;

                            //pbar.ShowProgress("Executing Features for company: " + ds.Tables["CO_mast_vw"].Rows[i]["Co_Name"].ToString().Trim(), 0);
                            this.ShowProcess("Updating features for :" + ds.Tables["CO_mast_vw"].Rows[i]["Co_Name"].ToString().Trim());
                            for (int j = 0; j < ldv.Table.Rows.Count; j++)
                            {
                                try
                                {
                                    //pbar.ShowProgress("Executing Features for company: " + ds.Tables["CO_mast_vw"].Rows[i]["Co_Name"].ToString().Trim(), Convert.ToInt32(j * ratio));
                                    SQL = ldv.Table.Rows[j]["SqlQuery"].ToString();
                                    lcmd = new SqlCommand(SQL, conn);
                                    param = lcmd.Parameters.Add("@encValue", SqlDbType.VarChar);
                                    param.Value = ldv.Table.Rows[j]["encValue"].ToString().Trim();

                                    this.ConnOpen();
                                    //int result = (int)lcmd.ExecuteScalar();
                                    SqlDataReader dr = lcmd.ExecuteReader();
                                    int resultID = dr.GetOrdinal("ret");
                                    int ErrorMsgID = dr.GetOrdinal("ErrorMsg");
                                    while (dr.Read())
                                    {
                                        result = (int)dr[resultID];
                                        string ErrorMsg = (string)dr[ErrorMsgID];
                                    }
                                    dr.Close();
                                    this.ConnClose();
                                    switch (result)
                                    {
                                        case 0:
                                            unFetRecords = unFetRecords + 1;
                                            throw new Exception("Option already exists.");
                                            break;
                                        case 1:
                                            unFetRecords = unFetRecords + 1;
                                            throw new Exception("Option not available in current Zip file. Kindly get new Neio/Nxio zip files.");
                                            break;
                                        case 2:
                                            throw new Exception("Feature Updated Sucessfully.");
                                            break;
                                        case 4:
                                            throw new Exception("Feature Updated Sucessfully.");
                                            break;
                                        case 5:
                                            unFetRecords = unFetRecords + 1;
                                            throw new Exception("Issue occured in execution." + Environment.NewLine + ErrorMsg);
                                        default:
                                            break;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //unFetRecords = unFetRecords + 1;
                                    if (result == 5)
                                    {
                                        //dr.Close();
                                        this.ConnClose();
                                    }
                                    DataRow ldr = LogTable.NewRow();
                                    ldr["co_name"] = ds.Tables["CO_mast_vw"].Rows[i]["co_name"].ToString().Trim();
                                    ldr["OptType"] = ldv.Table.Rows[j]["OptType"].ToString();
                                    ldr["OptName"] = ldv.Table.Rows[j]["OptName"].ToString();
                                    ldr["LogDesc"] = ex.Message;
                                    LogTable.Rows.Add(ldr);
                                    ldr = null;
                                }
                            }
                            this.ConnClose();
                            ldv.RowFilter = "";
                            ProcessEnd = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                            WriteLog(ds.Tables["CO_mast_vw"].Rows[i]["co_name"].ToString().Trim(), 2);
                            ClearTables();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                ErrorMsg="Error occured while updating features."+Environment.NewLine+ex.Message;
                return false;
            }
            return true;
        }
        private void CreateLogFile(string companyName, string companyFolder)
        {
            if (!System.IO.Directory.Exists(appPath + "\\" + companyFolder + "\\" + "Upgrade_Log"))        // Added By Shrikant S. on 31/03/2012 for Bug-3228
            {
                System.IO.Directory.CreateDirectory(appPath + "\\" + companyFolder + "\\" + "Upgrade_Log");
            }
            LogFile = appPath + "\\" + companyFolder + "\\Upgrade_Log\\Upgrade_Log_" + companyName + "_" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + ".uLog";
            if (!File.Exists(LogFile))
            {
                WriteLog(companyName, 1);
            }
        }
        private void WriteLog(string company, int type)
        {
            int xWidth = 200;
            //this.ShowProcess("Writing Log for " + company);

            switch (type)
            {
                case 1:
                    using (FileStream file = new FileStream(LogFile, FileMode.Append, FileAccess.Write))
                    {
                        StreamWriter streamWriter = new StreamWriter(file);
                        streamWriter.WriteLine("".PadRight(xWidth, '='));
                        int x = (xWidth - ("Auto Generated Log For:").Length - company.Length) / 2;
                        streamWriter.WriteLine(" ".PadLeft(x) + "Auto Generated Log For:" + company);
                        streamWriter.Close();
                    }
                    break;
                case 2:
                    using (FileStream file = new FileStream(LogFile, FileMode.Append, FileAccess.Write))
                    {
                        StreamWriter streamWriter = new StreamWriter(file);
                        streamWriter.WriteLine("".PadRight(xWidth, '='));
                        streamWriter.WriteLine(" ".PadLeft((xWidth - ("Log Summary").Length) / 2) + "Log Summary");
                        streamWriter.WriteLine("".PadRight(xWidth, '='));

                        streamWriter.WriteLine("Process Start:" + ProcessStart);
                        streamWriter.WriteLine("Total Features:" + totRecords.ToString());
                        streamWriter.WriteLine("Total Updated:" + (totRecords - unFetRecords).ToString());
                        streamWriter.WriteLine("Total Not Updated:" + unFetRecords.ToString());
                        streamWriter.WriteLine("Process End:" + ProcessEnd);

                        if (LogTable.Rows.Count > 0)
                        {
                            DataView ldv = LogTable.DefaultView;
                            ldv.Sort = "LogDesc";
                            streamWriter.WriteLine("".PadRight(xWidth, '='));
                            streamWriter.WriteLine(" ".PadLeft((xWidth - ("Log Details").Length) / 2) + "Log Details");
                            streamWriter.WriteLine("".PadRight(xWidth, '='));
                            streamWriter.WriteLine(("Option Type").PadRight(15, ' ') + ("Option Name").PadRight(125, ' ') + ("Description").PadRight(55, ' '));
                            for (int i = 0; i < ldv.Count; i++)
                            {
                                streamWriter.WriteLine((ldv[i]["OptType"].ToString().Trim()).PadRight(15, ' ') + (ldv[i]["OptName"].ToString().Trim()).PadRight(125, ' ') + (ldv[i]["LogDesc"].ToString().Trim()).PadRight(55, ' '));
                                //streamWriter.WriteLine((LogTable.Rows[i]["OptType"].ToString().Trim()).PadRight(15, ' ') + (LogTable.Rows[i]["OptName"].ToString().Trim()).PadRight(55, ' ') + (LogTable.Rows[i]["LogDesc"].ToString().Trim()).PadRight(55, ' '));
                            }
                        }
                        streamWriter.Close();
                    }
                    break;
            }
        }
        private void ClearTables()
        {
            if (LogTable != null)
                LogTable.Clear();
        }
        private bool InsertFeature()
        {
            SqlCommand lcmd;
            string sqlStr = string.Empty;
            //pbar.Show();
            try
            {
                for (int k = 0; k < ds.Tables["Co_mast_vw"].Rows.Count; k++)
                {
                    string companyName = ds.Tables["Co_mast_vw"].Rows[k]["Co_name"].ToString().Trim();
                    string companyDB = ds.Tables["Co_mast_vw"].Rows[k]["DBName"].ToString().ToUpper().Trim();
                    for (int i = 0; i < ds.Tables["CustInfo_vw"].Rows.Count; i++)
                    {
                        if (companyName.ToUpper() == ds.Tables["CustInfo_vw"].Rows[i]["clientnm"].ToString().ToUpper().Trim())
                        {
                            //pbar.ShowProgress("Inserting Features for company:" + companyName, 0);
                            this.ShowProcess("Updating feature for:" + companyName);
                            sqlStr = "Delete From " + companyDB + "..ClientFeature";
                            lcmd = new SqlCommand(sqlStr, conn);
                            this.ConnOpen();
                            lcmd.ExecuteNonQuery();
                            this.ConnClose();

                            string[] lfeatureIds = ds.Tables["CustInfo_vw"].Rows[i]["FeatureId"].ToString().Split(',');

                            for (int j = 0; j < lfeatureIds.Length; j++)
                            {
                                //pbar.ShowProgress("Inserting Features for company:" + companyName, Convert.ToInt32(j * ratio)); 
                                sqlStr = "If Not Exists (Select top 1 enc From " + companyDB + "..Clientfeature Where Enc=@encValue)  Begin Insert Into " + companyDB + "..ClientFeature (enc) values (@encValue) End";
                                lcmd = new SqlCommand(sqlStr, conn);
                                SqlParameter param = lcmd.Parameters.Add("@encValue", SqlDbType.NVarChar);
                                param.Value = oConnect.RetEncValue(txtCo_name.Text.Trim() + "~*0*~" + ds.Tables["CustInfo_vw"].Rows[i]["macid"].ToString().Trim() + "~*1*~" + lfeatureIds[j].Trim().PadLeft(10, '0') + "~*2*~" + companyName, ds.Tables["CustInfo_vw"].Rows[i]["macid"].ToString().Trim());

                                this.ConnOpen();
                                lcmd.ExecuteNonQuery();
                                this.ConnClose();

                            }
                            DoCompanyProductUpgrade(ds.Tables["CustInfo_vw"].Rows[i]["Prodcd"].ToString().Trim(), ds.Tables["CustInfo_vw"].Rows[i]["AddProdCd"].ToString().Trim(), ds.Tables["CustInfo_vw"].Rows[i]["VatStates"].ToString().Trim(), " Compid= " + ds.Tables["Co_mast_vw"].Rows[k]["CompId"].ToString());

                        }
                    }
                }
            }
            catch(Exception ex)
            { 
                ErrorMsg="Error occured while updating features."+Environment.NewLine+ex.Message;
                return false;
            }
            //pbar.Dispose();
            return true;
        }
        private void ConnOpen()
        {
            if (this.conn != null)
            {
                if (this.conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
            }
            else
            {
                this.conn = new SqlConnection(readIni.ConnectionString);
                this.conn.Open();
            }
        }
        private void ConnClose()
        {
            if (this.conn != null)
            {
                if (this.conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
        private void DoCompanyProductUpgrade(string companyProd, string compAddProd,string compVatStates, string cond)
        {
            SqlCommand lcmd;
            string sqlStr = string.Empty;
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            this.ShowProcess("Updating products...");
            sqlStr = "Update Co_mast set Passroute=Convert(Varbinary(250),@Pass), Passroute1=Convert(Varbinary(250),@Pass1),Prodcode=Convert(Varbinary(250),@Prodcode),Vatstates=@vatStates Where " + cond;
            lcmd = new SqlCommand(sqlStr, conn);
            SqlParameter param1 = lcmd.Parameters.Add("@Pass", SqlDbType.VarChar);
            param1.Value = Utilities.GetEncProductCode(companyProd);

            SqlParameter param2 = lcmd.Parameters.Add("@Pass1", SqlDbType.VarChar);
            param2.Value = Utilities.GetEncProductCode(compAddProd);

            SqlParameter param3 = lcmd.Parameters.Add("@Prodcode", SqlDbType.VarChar);
            param3.Value = string.Empty;

            SqlParameter param4 = lcmd.Parameters.Add("@vatStates", SqlDbType.VarChar);
            param4.Value = textInfo.ToTitleCase(compVatStates).ToString();

            this.ConnOpen();
            lcmd.ExecuteNonQuery();
            this.ConnClose();
        }
        private bool DoMenuUpdate()
        {
            SqlCommand lcmd;
            string sqlStr = string.Empty;
            string[] lProducts;
            DataRow[] ldrs;
            try
            {
                for (int k = 0; k < ds.Tables["Co_mast_vw"].Rows.Count; k++)
                {
                    string companyName = ds.Tables["Co_mast_vw"].Rows[k]["Co_name"].ToString().Trim();
                    string companyDB = ds.Tables["Co_mast_vw"].Rows[k]["DBName"].ToString().ToUpper().Trim();
                    for (int i = 0; i < ds.Tables["CustInfo_vw"].Rows.Count; i++)
                    {
                        if (companyName.ToUpper() == ds.Tables["CustInfo_vw"].Rows[i]["clientnm"].ToString().ToUpper().Trim())
                        {
                            //pbar.ShowProgress("Inserting Features for company:" + companyName, 0);
                            this.ShowProcess("Updating menus...");
                            try
                            {
                                sqlStr = "Update " + companyDB + "..Com_menu Set LabKey=''";
                                lcmd = new SqlCommand(sqlStr, conn);
                                this.ConnOpen();
                                lcmd.ExecuteNonQuery();
                                this.ConnClose();
                            }
                            catch
                            {
                                MessageBox.Show("Unable to update Menu Table." + Environment.NewLine + "Please contact your software vendor.", CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                            lProducts = ds.Tables["CustInfo_vw"].Rows[i]["ProdCd"].ToString().Split(',');
                            string lstProducts = string.Empty;
                            for (int l = 0; l < lProducts.Count(); l++)
                            {
                                lstProducts = lstProducts + "'" + lProducts[l] + "',";
                            }
                            lstProducts = (lstProducts.Length != 0 ? lstProducts.Substring(0, lstProducts.Length - 1) : "");
                            ldrs = ds.Tables["FeatureDet_vw"].Select(" ProdCode In (" + lstProducts + ") and Prodver='" + RegProd + "' and servicever='NORMAL' and fType='PREMIUM' and OptionType='MENU'");

                            foreach (DataRow ldr in ldrs)
                            {
                                //pbar.ShowProgress("Inserting Features for company:" + companyName, Convert.ToInt32(j * ratio)); 
                                sqlStr = "If Exists (Select Top 1 enc From " + companyDB + "..Clientfeature Where Enc=@encValue) ";
                                sqlStr = sqlStr + " " + "Begin";
                                sqlStr = sqlStr + " " + "Update " + companyDB + "..Com_Menu set LabKey='SPREMIUM' Where ltrim(rtrim(Padname))+ltrim(rtrim(Barname))='" + ldr["OptionName"].ToString().Trim() + "'";
                                sqlStr = sqlStr + " " + "End";
                                sqlStr = sqlStr + " " + "Else";
                                sqlStr = sqlStr + " " + "Begin";
                                sqlStr = sqlStr + " " + "Update " + companyDB + "..Com_Menu set LabKey='UPREMIUM' Where ltrim(rtrim(Padname))+ltrim(rtrim(Barname))='" + ldr["OptionName"].ToString().Trim() + "'";
                                sqlStr = sqlStr + " " + "End";

                                lcmd = new SqlCommand(sqlStr, conn);
                                SqlParameter param = lcmd.Parameters.Add("@encValue", SqlDbType.NVarChar);
                                param.Value = oConnect.RetEncValue(txtCo_name.Text.Trim() + "~*0*~" + ds.Tables["CustInfo_vw"].Rows[i]["macid"].ToString().Trim() + "~*1*~" + ldr["FeatureId"].ToString().Trim() + "~*2*~" + companyName, ds.Tables["CustInfo_vw"].Rows[i]["macid"].ToString().Trim());

                                this.ConnOpen();
                                lcmd.ExecuteNonQuery();
                                this.ConnClose();

                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                ErrorMsg = "Error occured while updating menu." + Environment.NewLine + ex.Message;
                return false;
            }
            return true;
        }
        public bool ExecuteUpdates()
        {

            UtilityUpdate runUpdates = new UtilityUpdate(readIni.ConnectionString);
            runUpdates.ExecuteSystemUpdates(Assembly.GetExecutingAssembly().GetManifestResourceStream("ueProductUpgrade.Updates.xml"));
            this.ShowProcess("Executing system updates...");
            for (int i = 0; i < ds.Tables["Co_Mast_vw"].Rows.Count; i++)
            {
                this.ShowProcess("Executing Company updates for :" + ds.Tables["Co_Mast_vw"].Rows[i]["Co_Name"].ToString().Trim());
                runUpdates.ExecuteCompanyUpdates(ds.Tables["Co_Mast_vw"].Rows[i]["Dbname"].ToString().Trim(), Assembly.GetExecutingAssembly().GetManifestResourceStream("ueProductUpgrade.Updates.xml"));
            }
            return true;
        }
        private void ShowProcess(string processMsg)
        {
            lblStatus.Text = "Please wait..." + Environment.NewLine + processMsg;
            lblStatus.Refresh();
        }
        #endregion
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.tabControl1.SelectedIndex = this.tabControl1.SelectedIndex - 1;
            SetNavigationVisibility();
            this.tabControl1.Refresh();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            this.tabControl1.SelectedIndex = this.tabControl1.SelectedIndex + 1;
            SetNavigationVisibility();
            this.tabControl1.Refresh();
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            this.btnFinish.Enabled = false;
            if (rdoProductUpgrade.Checked == true)
            {
                lblStatus.Visible = true;
                this.ShowProcess("Extracting Zip files");
                ExtractZipFiles();
                if (this.CheckRegisterMe())
                {
                    if (!CheckRegisterMeValidation())
                    {
                        lblStatus.Visible = false;
                        MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        return;
                    }
                }
                else
                {
                    MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }
                this.ReadInfFile();
                if (!this.CheckIniValidation())
                {
                    lblStatus.Visible = false;
                    MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }
                //if (!this.ExecuteUpdates())
                //{
                //    lblStatus.Visible = false;
                //    MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    this.Close();
                //    return;
                //}
                if (!this.GetFeatures())
                {
                    lblStatus.Visible = false;
                    MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }
                if (!this.DoProductUpgrade())
                {
                    lblStatus.Visible = false;
                    MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }
                if (!this.ExecuteScripts())
                {
                    lblStatus.Visible = false;
                    MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }
                if (!this.InsertFeature())
                {
                    lblStatus.Visible = false;
                    MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }
                if (!this.DoMenuUpdate())
                {
                    lblStatus.Visible = false;
                    MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }
                MessageBox.Show("Upgradation done ..."+Environment.NewLine+"Please check the log file in Companyfolder under Upgrade_Log folder" , CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.None);
                this.Close();
            }
            else
            {
                if (this.CheckValidation())
                {
                    string fileNames = string.Empty;
                    fileNames = GenerateCompanyXml();
                    fileNames = fileNames + Environment.NewLine + GenearateProductXml();
                    fileNames = fileNames + Environment.NewLine + "These Xml files generated successfully in main folder.";
                    MessageBox.Show(fileNames, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    this.btnFinish.Enabled = true;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //DialogResult ans = new DialogResult();
            //ans = MessageBox.Show("Do you really want to Cancel?", this.CurrentApplication, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (ans == DialogResult.Yes)
            //    Application.Exit();
            this.Dispose();
            Application.Exit();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            //appPath = "e:\\U3\\VudyogSdk";
            
            appPath = Application.StartupPath;
            SetStyleToGrid();
            SetImages();
            SetNavigationVisibility();
            lblCurrApp.Text = _CurrentApplication;
            lblUpgradeApp1.Text = _CurrentApplication;
            _UpgradeAppFile = this.CurrentAppFile;

            if (!Utilities.InList(this.CurrentAppFile, vu10Prod))
            {
                MessageBox.Show("This application will only run in VU-10 products.", CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                return;
            }
            oConnect = new clsConnect();
            //oConnect.InitProc(appPath, this.CurrentAppFile);          //Commented by Shrikant S. on 13/08/2014 for Bug-23814  
            oConnect.InitProc("'" + appPath + "'", this.CurrentAppFile);    //Added by Shrikant S. on 13/08/2014 for Bug-23814  
            BindData();
            tabControl1.TabPages.Remove(tabPage4);
            if (!this.CheckRegisterMe())
            {
                tabControl1.TabPages.Add(tabPage4);
                GenerateDeaultValue();
            }
            else
            {
                if (!CheckRegisterMe())
                {
                    MessageBox.Show("Error occured while reading Register.me file.", CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            lblStatus.Visible = true;
            lblStatus.Text = "Creating System Objects...";
            this.Refresh();
            if (!this.ExecuteUpdates())
            {

                lblStatus.Visible = false;
                MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            lblStatus.Visible = false;
            //Added by Shrikant S. on 26/10/2012 for Bug-5849       //Start
            _CurrentApplication = this.GetCurrentAppName(_CurrentAppFile);      
            lblCurrApp.Text = _CurrentApplication;
            lblUpgradeApp1.Text = _CurrentApplication;
            _UpgradeAppFile = this.CurrentAppFile;
            //Added by Shrikant S. on 26/10/2012 for Bug-5849       //End
            this.Refresh();
        }

        private void btnApplication_Click(object sender, EventArgs e)
        {
            conn = new SqlConnection(readIni.ConnectionString);
            frmSearchForm objSearch = new frmSearchForm(conn, "Select Application", "ApplicationDet", "AppName,AppDesc", "AppName:Name,AppDesc:Description", "", "AppName,AppDesc");
            if (objSearch.ShowDialog() == DialogResult.OK)
            {
                lblUpgradeApp1.Text = objSearch.ReturnString[1].ToString();
                _UpgradeApplication = objSearch.ReturnString[1].ToString();
                _UpgradeAppFile = objSearch.ReturnString[0].ToString();
                objSearch = null;
            }
        }


        private void dgvComp1_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvComp1.SelectedRows.Count > 0)
            {
                ds.Tables["cProduct_vw"].DefaultView.RowFilter = "";
                ds.Tables["cProduct_vw"].DefaultView.RowFilter = "CompId=" + dgvComp1.SelectedRows[0].Cells["CompId"].Value.ToString();
                dgvProduct.Refresh();
            }
            else
            {
                ds.Tables["cProduct_vw"].DefaultView.RowFilter = "";
                ds.Tables["cProduct_vw"].DefaultView.RowFilter = "CompId=" + dgvComp1.Rows[0].Cells["CompId"].Value.ToString();
                dgvProduct.Refresh();
            }
        }

        private void dgvComp2_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvComp2.SelectedRows.Count > 0)
            {
                ds.Tables["Product_vw"].DefaultView.RowFilter = "";
                ds.Tables["Product_vw"].DefaultView.RowFilter = "CompId=" + dgvComp2.SelectedRows[0].Cells["CompId"].Value.ToString();
                for (int i = 0; i < dgvProductUpgrade.Rows.Count; i++)
                {
                    if (dgvProductUpgrade.Rows[i].Cells["Sel"].Value.ToString() == "True")
                    {
                        dgvProductUpgrade.Rows[i].ReadOnly = true;
                        dgvProductUpgrade.Rows[i].DefaultCellStyle.BackColor = Color.Cyan;
                    }
                }
                dgvProductUpgrade.Sort(this.dgvProductUpgrade.Columns["cModDep"], ListSortDirection.Ascending);
                dgvProductUpgrade.Rows[0].Selected = true;
                dgvProductUpgrade.FirstDisplayedScrollingRowIndex = 0;
                dgvProductUpgrade.Refresh();
            }
            else
            {
                ds.Tables["Product_vw"].DefaultView.RowFilter = "";
                ds.Tables["Product_vw"].DefaultView.RowFilter = "CompId=" + dgvComp2.Rows[0].Cells["CompId"].Value.ToString();
                for (int i = 0; i < dgvProductUpgrade.Rows.Count; i++)
                {
                    if (dgvProductUpgrade.Rows[i].Cells["Sel"].Value.ToString() == "True")
                    {
                        dgvProductUpgrade.Rows[i].ReadOnly = true;
                    }
                }
                dgvProductUpgrade.Sort(this.dgvProductUpgrade.Columns["cModDep"], ListSortDirection.Ascending);
                dgvProductUpgrade.Rows[0].Selected = true;
                dgvProductUpgrade.FirstDisplayedScrollingRowIndex = 0;
                dgvProductUpgrade.Refresh();
            }
        }
        private void dgvProductUpgrade_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (dgvProductUpgrade.Columns[e.ColumnIndex].Name == "chkSel2")
            {
                if (dgvProductUpgrade.CurrentRow.Cells["Sel"].ReadOnly == false)
                {
                    DataRow[] dr = ds.Tables["Product_vw"].Select("CompId=" + dgvComp2.SelectedRows[0].Cells["CompId"].Value.ToString() + " and cProdCode='" + dgvProductUpgrade.CurrentRow.Cells["cProdCode"].Value.ToString() + "'");
                    string moduleDep = dgvProductUpgrade.CurrentRow.Cells["cModDep"].Value.ToString();
                    string moduleNotAllowed = dgvProductUpgrade.CurrentRow.Cells["cCmbNotAlwd"].Value.ToString();
                    string currProd = dgvProductUpgrade.CurrentRow.Cells["cProdName"].Value.ToString();
                    int currIndex = dgvProductUpgrade.CurrentRow.Index;
                    string productName = string.Empty;
                    string prodNotAllowed = string.Empty;
                    bool setValue = true;
                    switch (dgvProductUpgrade.CurrentRow.Cells["Sel"].Value.ToString())
                    {
                        case "True":

                            if (moduleDep.Trim().Length == 0)
                            {
                                string unselectProd = dr.ElementAt(0)["cProdCode"].ToString();
                                DialogResult ans = new DialogResult();
                                string Msg = "some of the dependent modules are selected." + Environment.NewLine;
                                Msg = Msg + "Un-selecting the main module will unselect the depending modules." + Environment.NewLine;
                                Msg = Msg + "Would you like to continue ?";
                                ans = MessageBox.Show(Msg, this.CurrentApplication, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (ans == DialogResult.Yes)
                                {
                                    for (int i = 0; i < dgvProductUpgrade.RowCount; i++)
                                    {
                                        if (dgvProductUpgrade.Rows[i].Cells["Sel"].Value.ToString() == "True")
                                        {
                                            string DependencyMod = dgvProductUpgrade.Rows[i].Cells["cModDep"].Value.ToString();
                                            if (DependencyMod.Length != 0)
                                            {
                                                if (DependencyMod.Contains(unselectProd))
                                                {
                                                    dgvProductUpgrade.Rows[i].Cells["Sel"].Value = false;
                                                }
                                            }
                                        }
                                    }
                                    setValue = false;
                                }
                                dr.ElementAt(0)["Sel"] = setValue;
                                dgvProductUpgrade.Rows[currIndex].Selected = true;
                                dgvProductUpgrade.CurrentRow.Cells["Sel"].Value = setValue;
                            }
                            break;
                        case "False":
                            if (moduleDep.Trim().Length != 0)
                            {
                                int cnt = 0;
                                for (int i = 0; i < dgvProductUpgrade.Rows.Count; i++)
                                {
                                    if (moduleDep.Contains(dgvProductUpgrade.Rows[i].Cells["cProdCode"].Value.ToString()))
                                    {
                                        if (dgvProductUpgrade.Rows[i].Cells["Sel"].Value.ToString() == "True")
                                        {
                                            productName = string.Empty;
                                            break;
                                        }
                                        cnt = cnt + 1;
                                        productName = productName + (cnt == 1 ? "" : ", " + Environment.NewLine) + dgvProductUpgrade.Rows[i].Cells["cProdName"].Value.ToString();
                                    }
                                }
                                if (productName.Trim().Length != 0)
                                {
                                    string Msg = currProd + " product can be selected only if";
                                    Msg = Msg + (cnt == 1 ? " the following module is selected: " : " either of the following modules are selected: ") + Environment.NewLine + productName;
                                    MessageBox.Show(Msg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    setValue = false;
                                }
                                dr.ElementAt(0)["Sel"] = setValue;
                                dgvProductUpgrade.Rows[currIndex].Selected = true;
                                dgvProductUpgrade.CurrentRow.Cells["Sel"].Value = setValue;
                            }
                            if (moduleNotAllowed.Trim().Length != 0 && setValue == true)
                            {
                                for (int i = 0; i < dgvProductUpgrade.Rows.Count; i++)
                                {
                                    if (moduleNotAllowed.Contains(dgvProductUpgrade.Rows[i].Cells["cMainProdCode"].Value.ToString()) && dgvProductUpgrade.Rows[i].Cells["Sel"].Value.ToString() == "True")
                                    {
                                        prodNotAllowed = prodNotAllowed + ", " + dgvProductUpgrade.Rows[i].Cells["cProdName"].Value.ToString();
                                    }
                                }
                                if (prodNotAllowed.Trim().Length != 0)
                                {
                                    string Msg = "'" + currProd + "' product not allowed with selected combination.";
                                    MessageBox.Show(Msg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    setValue = false;
                                }
                                dr.ElementAt(0)["Sel"] = setValue;
                                dgvProductUpgrade.Rows[currIndex].Selected = true;
                                dgvProductUpgrade.CurrentRow.Cells["Sel"].Value = setValue;
                            }
                            break;
                    }
                    dgvProductUpgrade.RefreshEdit();
                }
            }
        }



        private void btnCompany_Click(object sender, EventArgs e)
        {
            conn = new SqlConnection(readIni.ConnectionString);
            frmSearchForm objSearch = new frmSearchForm(ds.Tables["Co_Mast_vw"], "Select company", "Co_Name", "Co_Name:Company Name", "Compid,passroute,passroute1,Add1,Add2,Add3,area,zone,city,zip,state,country,email,sta_dt,end_dt,dbname,foldername", "Co_Name,Add1,Add2,Add3,area,zone,city,zip,state,country,email");
            if (objSearch.ShowDialog() == DialogResult.OK)
            {

                txtCo_name.Text = objSearch.ReturnString[0].ToString().Trim();
                txtContact.Text = objSearch.ReturnString[9].ToString().Trim();
                txtEmail.Text = objSearch.ReturnString[10].ToString().Trim();

                //Co_Name,Add1,Add2,Add3,area,zone,city,zip,state,country,email
                stringList.Add(objSearch.ReturnString[0].ToString().Trim());
                stringList.Add(objSearch.ReturnString[1].ToString().Trim());
                stringList.Add(objSearch.ReturnString[2].ToString().Trim());
                stringList.Add(objSearch.ReturnString[3].ToString().Trim());
                stringList.Add(objSearch.ReturnString[4].ToString().Trim());
                stringList.Add(objSearch.ReturnString[5].ToString().Trim());
                stringList.Add(objSearch.ReturnString[6].ToString().Trim());
                stringList.Add(objSearch.ReturnString[7].ToString().Trim());
                stringList.Add(objSearch.ReturnString[8].ToString().Trim());
                stringList.Add(objSearch.ReturnString[9].ToString().Trim());
                stringList.Add("");
                stringList.Add("");
                stringList.Add("");
                stringList.Add("");
                stringList.Add(objSearch.ReturnString[10].ToString().Trim());
                stringList.Add("");
                stringList.Add("");
            }

            objSearch = null;
        }

        private void cboServiceVer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboServiceVer.Text != "Client Version")
            {
                txtUsers.Text = "1";
                txtNoComp.Text = "1";
            }
        }

        private void rdoProductUpgrade_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoProductUpgrade.Checked == true)
            {
                tabControl1.TabPages.Remove(tabPage2);
                tabControl1.TabPages.Remove(tabPage3);
                if (!this.CheckRegisterMe())
                    tabControl1.TabPages.Remove(tabPage4);

                this.btnBack.Enabled = false;
                this.btnNext.Enabled = false;
                this.btnFinish.Enabled = true;

            }
            else
            {
                tabControl1.TabPages.Add(tabPage2);
                tabControl1.TabPages.Add(tabPage3);

                if (!this.CheckRegisterMe())
                {
                    tabControl1.TabPages.Add(tabPage4);
                    GenerateDeaultValue();
                }
                this.SetNavigationVisibility();
            }
        }

        private void txtUsers_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Delete);
        }

        private void txtNoComp_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Delete);
        }


    }
}
