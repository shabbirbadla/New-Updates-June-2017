using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml.xmp;
using System.Security.Cryptography.Xml;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using Udyog.Library.Common;     //Added by Shrikant S. on 16/09/2015 for Bug-26664
using uBaseForm;
using GetInfo;
//using ueconnect;          //Commented by Shrikant S. on 16/09/2015 for Bug-26664

namespace udDigitalSigSetting
{
    public partial class frmDigitalSigSetting : uBaseForm.FrmBaseForm
    {
        DataAccess_Net.clsDataAccess oDataAccess = new DataAccess_Net.clsDataAccess();
        //clsConnect oConnect;
        String cAppPId, cAppName;
        string startupPath = string.Empty, ServiceType = string.Empty, sqlQuery = string.Empty, certtxt = string.Empty, Pass = string.Empty, id = string.Empty;
        string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;

        SqlConnection conn = new SqlConnection();

        DataSet tDs, DS1, dsMain;
        private RegisterMeInfo m_info;           //Added by Shrikant S. on 16/09/2015 for Bug-26664
        #region form constructor

        public frmDigitalSigSetting(string[] args)
        {
            InitializeComponent();
            this.pDisableCloseBtn = true;
            this.pPara = args;
            this.pFrmCaption = "Digital Signature";
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


            grpView.Location = new Point(7, 32);
            this.StartPosition = FormStartPosition.CenterScreen;

            this.StartPosition = FormStartPosition.Manual;
            this.Top = (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2;
            this.Left = (Screen.PrimaryScreen.Bounds.Width - this.Width) / 2;

        }

        #endregion

        #region Form load
        private void frmDigitalSigSetting_Load(object sender, EventArgs e)
        {
            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.DateSeparator = "/";
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();

            string appPath = Application.ExecutablePath;
            appPath = Path.GetDirectoryName(appPath);
            //appPath = @"E:\U3\USquare";//Pankaj
            if (string.IsNullOrEmpty(this.pAppPath))
            {
                this.pAppPath = appPath;
            }
            string fName = appPath + @"\bmp\pickup.gif";
            if (File.Exists(fName) == true)
            {
                this.btnPickUp.Image = System.Drawing.Image.FromFile(fName);
                this.btnDept.Image = System.Drawing.Image.FromFile(fName);
                this.btnCate.Image = System.Drawing.Image.FromFile(fName);
                this.btnInvSr.Image = System.Drawing.Image.FromFile(fName);
                this.btnValidIn.Image = System.Drawing.Image.FromFile(fName);
            }

            startupPath = Application.StartupPath;
            //startupPath = @"E:\U3\VudyogGST";          // To be Removed Sachin 
            //oConnect = new clsConnect();          //Commented by Shrikant S. on 16/09/2015 for Bug-26664
            GetInfo.iniFile ini = new GetInfo.iniFile(startupPath + "\\" + "Visudyog.ini");
            string appfile = ini.IniReadValue("Settings", "xfile").Substring(0, ini.IniReadValue("Settings", "xfile").Length - 4);
            //oConnect.InitProc("'" + startupPath + "'", appfile);          //Commented by Shrikant S. on 16/09/2015 for Bug-26664
            DirectoryInfo dir = new DirectoryInfo(startupPath);
            Array totalFile = dir.GetFiles();
            string registerMePath = string.Empty;
            for (int i = 0; i < totalFile.Length; i++)
            {
                string fname = Path.GetFileName(((System.IO.FileInfo[])(totalFile))[i].Name);
                if (Path.GetFileName(((System.IO.FileInfo[])(totalFile))[i].Name).ToUpper().Contains("REGISTER.ME"))
                {
                    registerMePath = Path.GetFileName(((System.IO.FileInfo[])(totalFile))[i].Name);
                    break;
                }
            }
            if (registerMePath == string.Empty)
            {
                ServiceType = "";
            }
            else
            {
                //string[] objRegisterMe = (oConnect.ReadRegiValue(startupPath)).Split('^');        //Commented by Shrikant S. on 16/09/2015 for Bug-26664
                string[] objRegisterMe = (this.GetRegisterDetails()).Split('^');            //Added by Shrikant S. on 16/09/2015 for Bug-26664

                ServiceType = objRegisterMe[15].ToString().Trim();
            }
            if (ServiceType.ToUpper() == "VIEWER VERSION")
            {
                this.btnNew.Enabled = false;
                this.btnEdit.Enabled = false;
                this.btnCancel.Enabled = false;
                this.btnDelete.Enabled = false;
            }
            else
            {
                this.pAddMode = false;
                this.pEditMode = false;
                ControlsEnable();
            }
            conn.ConnectionString = "data source='" + pServerName + "';initial catalog='" + pComDbnm + "';user id='" + pUserId + "';password='" + pPassword + "'";

            mInsertProcessIdRecord();
            this.btnLast_Click(sender, e);

        }
        #endregion

        #region Controls Enable Desable
        private void ControlsEnable()
        {
            if (this.pEditMode)
            {
                this.ActionButtonState(false, false, true, true, false);
                this.MOVEBTNSTATE(false, false, false, false);

                foreach (Control c in grpSelect.Controls)
                {
                    if (c is TextBox)
                    {
                        c.Enabled = true;
                    }
                }

                foreach (Control c in grpSelect.Controls)
                {
                    if (c is Button)
                    {
                        c.Enabled = true;
                    }
                }
                tlsbtnSearch.Enabled = false;
                btnExit.Enabled = false;

                //Added by Shrikant S. on 10/09/2015 for Bug-26664      //Start
                this.txtLeft.Enabled = true;
                this.txtBottom.Enabled = true;
                this.txtWidth.Enabled = true;
                this.txtHeight.Enabled = true;
                this.txtLocation.Enabled = true;
                this.txtReason.Enabled = true;

                if (this.txtLocation.Text.Length > 0)
                    this.chkLocation.Enabled = true;
                else
                    this.chkLocation.Enabled = false;

                if (this.txtReason.Text.Length > 0)
                    this.chkReason.Enabled = true;
                else
                    this.chkReason.Enabled = false;

                //Added by Shrikant S. on 10/09/2015 for Bug-26664      //End
            }
            else if (this.pAddMode)
            {
                this.ActionButtonState(false, false, true, true, false);
                this.MOVEBTNSTATE(false, false, false, false);

                txtCert.Text = "";
                txtPass.Text = "";
                foreach (Control c in grpSelect.Controls)
                {
                    if (c is TextBox)
                    {
                        c.Text = String.Empty;
                        c.Enabled = true;
                    }
                }

                foreach (Control c in grpSelect.Controls)
                {
                    if (c is Button)
                    {
                        c.Text = String.Empty;
                        c.Enabled = true;
                    }
                }

                tlsbtnSearch.Enabled = false;
                btnExit.Enabled = false;
                grpView.Location = new Point(9, 146);
                this.Size = new Size(488, 565);
                //Added by Shrikant S. on 10/09/2015 for Bug-26664      //Start
                this.txtLeft.Enabled = true;
                this.txtBottom.Enabled = true;
                this.txtWidth.Enabled = true;
                this.txtHeight.Enabled = true;
                this.txtLocation.Enabled = true;
                this.txtReason.Enabled = true;
                //this.chkLocation.Enabled = true;
                //this.chkReason.Enabled = true;
                //Added by Shrikant S. on 10/09/2015 for Bug-26664      //End
            }
            else
            {
                this.ActionButtonState(true, true, false, false, true);
                this.MOVEBTNSTATE(true, true, true, true);
                foreach (Control c in grpView.Controls)
                {
                    if (c is TextBox)
                    {
                        c.Enabled = false;
                    }
                    ////Added by Shrikant S. on 10/09/2015 for Bug-26664      //Start
                    if (c is CheckBox)
                    {
                        c.Enabled = false;
                    }
                    ////Added by Shrikant S. on 10/09/2015 for Bug-26664      //End
                }
                foreach (Control c in grpSelect.Controls)
                {
                    if (c is TextBox)
                    {
                        c.Enabled = false;
                    }
                }

                foreach (Control c in grpSelect.Controls)
                {
                    if (c is Button)
                    {
                        c.Enabled = false;
                    }
                }
                tlsbtnSearch.Enabled = true;
                btnExit.Enabled = true;
                grpView.Location = new Point(7, 32);
                this.Size = new Size(488, 450);


            }
        }
        #endregion

        #region Enable disable [Action and move] menu
        private void MOVEBTNSTATE(bool first, bool back, bool next, bool last)
        {
            btnFirst.Enabled = first;
            btnBack.Enabled = back;
            btnForward.Enabled = next;
            btnLast.Enabled = last;
        }

        private void ActionButtonState(bool New, bool Edit, bool Save, bool Cancel, bool Delete)
        {
            btnNew.Enabled = New;
            btnEdit.Enabled = Edit;
            btnSave.Enabled = Save;
            btnCancel.Enabled = Cancel;
            btnDelete.Enabled = Delete;
        }
        #endregion

        #region Add/Edit/Delete/Cancel/Exit button event
        private void btnNew_Click(object sender, EventArgs e)
        {
            this.rdoUSBToken.Checked = true;        //Added by Shrikant S. on 10/09/2015 for Bug-26664
            this.pAddMode = true;
            this.ControlsEnable();
            if (tDs != null)
                tDs.Clear();
            DataRow dr1;
            dr1 = tDs.Tables[0].NewRow();

            foreach (DataColumn dc in tDs.Tables[0].Columns)
            {
                switch (dc.DataType.ToString())
                {
                    case "System.String":
                        dr1[dc] = "";
                        break;
                    case "System.DateTime":
                        dr1[dc] = DateTime.Now.Date;
                        break;
                    case "System.Int32":
                        dr1[dc] = 0;
                        break;
                    case "System.Decimal":
                        dr1[dc] = 0.00;
                        break;
                    case "System.Boolean":              //Added by Shrikant S. on 10/09/2015 for Bug-26664
                        dr1[dc] = false;                //Added by Shrikant S. on 10/09/2015 for Bug-26664
                        break;                          //Added by Shrikant S. on 10/09/2015 for Bug-26664
                    default:
                        break;
                }

            }
            //Added by Shrikant S. on 10/09/2015 for Bug-26664      //Start
            dr1["signLeft"] = 375;
            dr1["signBottom"] = 30;
            dr1["signWidth"] = 200;
            dr1["signHeight"] = 70;
            //Added by Shrikant S. on 10/09/2015 for Bug-26664      //End
            tDs.Tables[0].Rows.Add(dr1);

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (this.pAddMode)
            {

                this.pAddMode = false;
                this.pEditMode = false;
                ControlsEnable();
                btnLast_Click(sender, e);
            }
            else
            {
                this.pAddMode = false;
                this.pEditMode = false;
                ControlsEnable();
                selectrecord();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            this.pEditMode = true;
            this.ControlsEnable();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            mDeleteProcessIdRecord();
            this.Close();
        }

        #endregion



        private void Controlset()
        {
            txtSignBy.DataBindings.Clear();
            txtSignBy.Text = "";
            txtSignBy.DataBindings.Add("Text", tDs.Tables[0], "SignBy");
            txtOrg.DataBindings.Clear();
            txtOrg.Text = "";
            txtOrg.DataBindings.Add("Text", tDs.Tables[0], "OrgName");
            txtOrgUnit.DataBindings.Clear();
            txtOrgUnit.Text = "";
            txtOrgUnit.DataBindings.Add("Text", tDs.Tables[0], "OrgUnitNm");
            txtCountry.DataBindings.Clear();
            txtCountry.Text = "";
            txtCountry.DataBindings.Add("Text", tDs.Tables[0], "Country");
            txtState.DataBindings.Clear();
            txtState.Text = "";
            txtState.DataBindings.Add("Text", tDs.Tables[0], "State");

            txtLocation.DataBindings.Clear();
            txtLocation.Text = "";
            txtLocation.DataBindings.Add("Text", tDs.Tables[0], "Location");
            txtEmail.DataBindings.Clear();
            txtEmail.Text = "";
            txtEmail.DataBindings.Add("Text", tDs.Tables[0], "Email");
            txtValidAfter.DataBindings.Clear();
            txtValidAfter.Text = "";
            txtValidAfter.DataBindings.Add("Text", tDs.Tables[0], "Sdate");
            txtValidBefore.DataBindings.Clear();
            txtValidBefore.Text = "";
            txtValidBefore.DataBindings.Add("Text", tDs.Tables[0], "Edate");
            txtDept.DataBindings.Clear();
            txtDept.Text = "";
            txtDept.DataBindings.Add("Text", tDs.Tables[0], "Dept");
            txtCate.DataBindings.Clear();
            txtCate.Text = "";
            txtCate.DataBindings.Add("Text", tDs.Tables[0], "Cate");
            txtInvSr.DataBindings.Clear();
            txtInvSr.Text = "";
            txtInvSr.DataBindings.Add("Text", tDs.Tables[0], "InvSr");
            txtValid.DataBindings.Clear();
            txtValid.Text = "";
            txtValid.DataBindings.Add("Text", tDs.Tables[0], "Validity");

            //Added by Shrikant S. on 09/09/2015        //Start
            txtReason.DataBindings.Clear();
            txtReason.Text = "";
            txtReason.DataBindings.Add("Text", tDs.Tables[0], "Reason");


            txtLeft.DataBindings.Clear();
            txtLeft.Text = "";
            Binding txtLeftBind = new Binding("Text", tDs.Tables[0], "signLeft");
            txtLeftBind.Format += new ConvertEventHandler(IntToString);
            txtLeftBind.Parse += new ConvertEventHandler(StringToInt);
            txtLeft.DataBindings.Add(txtLeftBind);

            txtBottom.DataBindings.Clear();
            txtBottom.Text = "";
            Binding txtBottomBind = new Binding("Text", tDs.Tables[0], "signBottom");
            txtBottomBind.Format += new ConvertEventHandler(IntToString);
            txtBottomBind.Parse += new ConvertEventHandler(StringToInt);
            txtBottom.DataBindings.Add(txtBottomBind);


            txtWidth.DataBindings.Clear();
            txtWidth.Text = "";
            Binding txtWidthBind = new Binding("Text", tDs.Tables[0], "signWidth");
            txtWidthBind.Format += new ConvertEventHandler(IntToString);
            txtWidthBind.Parse += new ConvertEventHandler(StringToInt);
            txtWidth.DataBindings.Add(txtWidthBind);


            txtHeight.DataBindings.Clear();
            txtHeight.Text = "";
            Binding txtHeightBind = new Binding("Text", tDs.Tables[0], "signHeight");
            txtHeightBind.Format += new ConvertEventHandler(IntToString);
            txtHeightBind.Parse += new ConvertEventHandler(StringToInt);
            txtHeight.DataBindings.Add(txtHeightBind);

            chkLocation.DataBindings.Clear();
            chkLocation.Checked = false;
            chkLocation.DataBindings.Add(new Binding("Checked", tDs.Tables[0], "ShowLocation"));

            //Binding chkLocationBind = new Binding("Checked", tDs.Tables[0], "ShowLocation");
            //chkLocationBind.Format += new ConvertEventHandler(BoolToString);
            //chkLocationBind.Parse += new ConvertEventHandler(StringToBool);
            //chkLocation.DataBindings.Add(chkLocationBind);

            chkReason.DataBindings.Clear();
            chkReason.Checked = false;
            chkReason.DataBindings.Add(new Binding("Checked", tDs.Tables[0], "ShowReason"));

            //Binding chkReasonBind = new Binding("Checked", tDs.Tables[0], "ShowReason");
            //chkReasonBind.Format += new ConvertEventHandler(BoolToString);
            //chkReasonBind.Parse += new ConvertEventHandler(StringToBool);
            //chkReason.DataBindings.Add(chkReasonBind);

            //Added by Shrikant S. on 09/09/2015        //End

        }

        public X509Certificate2 GetCertificate()
        {

            X509Store st = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            st.Open(OpenFlags.ReadOnly);

            X509Certificate2Collection col = st.Certificates;
            X509Certificate2 card = null;


            X509Certificate2Collection fcollection = (X509Certificate2Collection)col.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
            X509Certificate2Enumerator en2 = fcollection.GetEnumerator();
            en2.MoveNext();

            X509Certificate2Collection sel = X509Certificate2UI.SelectFromCollection(fcollection, "Certificates", "Select one to sign", X509SelectionFlag.SingleSelection);
            if (sel.Count > 0)
            {
                X509Certificate2Enumerator en = sel.GetEnumerator();
                en.MoveNext();
                card = en.Current;
            }
            st.Close();
            return card;
        }

        private void btnPickUp_Click(object sender, EventArgs e)
        {
            //Added by Shrikant S. on 09/09/2015        //Start
            if (rdoUSBToken.Checked == true)
            {
                X509Certificate2 certificate = this.GetCertificate();
                if (certificate == null)
                    return;
                org.bouncycastle.x509.X509Certificate certAux = new org.bouncycastle.x509.X509Certificate(certificate.GetRawCertData());
                org.bouncycastle.x509.X509Certificate[] chain = new org.bouncycastle.x509.X509Certificate[] { certAux };
                if (chain.Length > 0)
                {

                    Hashtable certDetail = new Hashtable();
                    certDetail = PdfPKCS7.GetSubjectFields(chain[0]).GetFields();

                    if (!certDetail.ContainsKey("O"))
                    {
                        MessageBox.Show("Please check the proper Certificate is selected or not/USB Token is connected.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (certDetail.ContainsKey("E") || certDetail.ContainsKey("email"))
                        tDs.Tables[0].Rows[0]["Email"] = PdfPKCS7.GetSubjectFields(chain[0]).GetField("E").ToString();

                    tDs.Tables[0].Rows[0]["SignBy"] = PdfPKCS7.GetSubjectFields(chain[0]).GetField("CN").ToString();
                    tDs.Tables[0].Rows[0]["OrgName"] = PdfPKCS7.GetSubjectFields(chain[0]).GetField("O").ToString();

                    if (certDetail.ContainsKey("OU"))
                        tDs.Tables[0].Rows[0]["OrgUnitNm"] = PdfPKCS7.GetSubjectFields(chain[0]).GetField("OU").ToString();

                    if (certDetail.ContainsKey("L"))
                        tDs.Tables[0].Rows[0]["Location"] = PdfPKCS7.GetSubjectFields(chain[0]).GetField("L").ToString();

                    tDs.Tables[0].Rows[0]["State"] = PdfPKCS7.GetSubjectFields(chain[0]).GetField("ST").ToString();
                    tDs.Tables[0].Rows[0]["Country"] = PdfPKCS7.GetSubjectFields(chain[0]).GetField("C").ToString();
                    tDs.Tables[0].Rows[0]["Sdate"] = certificate.NotBefore.ToString("dd/MM/yyyy");
                    tDs.Tables[0].Rows[0]["Edate"] = certificate.NotAfter.ToString("dd/MM/yyyy");

                }
                this.txtLocation.Focus();
            }
            else
            {
                //Added by Shrikant S. on 09/09/2015        //End
                filedialog.Title = "Browse Pfx files";
                filedialog.FileName = "";
                filedialog.CheckFileExists = true;
                filedialog.CheckPathExists = true;
                filedialog.Filter = "Pfx files (*.pfx)|*.pfx";

                if (filedialog.ShowDialog() == DialogResult.OK)
                {
                    txtCert.Text = filedialog.FileName;
                }
            }                   //Added by Shrikant S. on 09/09/2015       
        }

        private void btnValidateSign_Click(object sender, EventArgs e)
        {
            //Added by Shrikant S. on 09/09/2015        //Start
            if (rdoUSBToken.Checked == true)
            {
                return;
            }
            //Added by Shrikant S. on 09/09/2015        //End
            X509Certificate2Collection collection = new X509Certificate2Collection();
            try
            {
                collection.Import(txtCert.Text.ToString().Trim(), txtPass.Text.ToString().Trim(), X509KeyStorageFlags.PersistKeySet);
                foreach (X509Certificate2 x509 in collection)
                {
                    certtxt = x509.Subject.Substring(x509.Subject.IndexOf("E=") + 2, x509.Subject.IndexOf(",") - 2);
                    tDs.Tables[0].Rows[0]["Email"] = certtxt;
                    certtxt = x509.Subject.Substring(x509.Subject.IndexOf("CN="), x509.Subject.Length - x509.Subject.IndexOf("CN="));
                    tDs.Tables[0].Rows[0]["SignBy"] = certtxt.Substring(certtxt.IndexOf("CN=") + 3, certtxt.IndexOf(",") - 3);
                    certtxt = x509.Subject.Substring(x509.Subject.IndexOf("O="), x509.Subject.Length - x509.Subject.IndexOf("O="));
                    tDs.Tables[0].Rows[0]["OrgName"] = certtxt.Substring(certtxt.IndexOf("O=") + 2, certtxt.IndexOf(",") - 2);
                    certtxt = x509.Subject.Substring(x509.Subject.IndexOf("OU="), x509.Subject.Length - x509.Subject.IndexOf("OU="));
                    tDs.Tables[0].Rows[0]["OrgUnitNm"] = certtxt.Substring(certtxt.IndexOf("OU=") + 3, certtxt.IndexOf(",") - 3);
                    certtxt = x509.Subject.Substring(x509.Subject.IndexOf("L="), x509.Subject.Length - x509.Subject.IndexOf("L="));
                    tDs.Tables[0].Rows[0]["Location"] = certtxt.Substring(certtxt.IndexOf("L=") + 2, certtxt.IndexOf(",") - 2);
                    certtxt = x509.Subject.Substring(x509.Subject.IndexOf("S="), x509.Subject.Length - x509.Subject.IndexOf("S="));
                    tDs.Tables[0].Rows[0]["State"] = certtxt.Substring(certtxt.IndexOf("S=") + 2, certtxt.IndexOf(",") - 2);
                    certtxt = x509.Subject.Substring(x509.Subject.IndexOf("C="), x509.Subject.Length - x509.Subject.IndexOf("C="));
                    tDs.Tables[0].Rows[0]["Country"] = certtxt.Substring(certtxt.IndexOf("C=") + 2, certtxt.Length - 2);
                    tDs.Tables[0].Rows[0]["Sdate"] = x509.NotBefore.ToString().Substring(0, 10);
                    tDs.Tables[0].Rows[0]["Edate"] = x509.NotAfter.ToString().Substring(0, 10);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnDept_Click(object sender, EventArgs e)
        {

            sqlQuery = "select dept from department";
            DS1 = oDataAccess.GetDataSet(sqlQuery, null, 25);
            DataView dvw = DS1.Tables[0].DefaultView;

            VForText = "Select Department";
            vSearchCol = "dept";
            vDisplayColumnList = "Dept:Department";
            vReturnCol = "Dept";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                tDs.Tables[0].Rows[0]["Dept"] = oSelectPop.pReturnArray[0];
                this.txtDept.Text = oSelectPop.pReturnArray[0];     //Added by Shrikant S. on 15/09/2015 for Bug-26664
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtSignBy.Text == "")
            {
                MessageBox.Show("Please Verify signature...!!! ", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (this.pAddMode)
            {
                sqlQuery = "select signby from DigitalSign where signby='" + tDs.Tables[0].Rows[0]["SignBy"] + "'";
                sqlQuery += " and dept='" + tDs.Tables[0].Rows[0]["Dept"] + "' and cate='" + tDs.Tables[0].Rows[0]["Cate"] + "'";
                sqlQuery += " and invsr='" + tDs.Tables[0].Rows[0]["InvSr"] + "'";
                DS1 = oDataAccess.GetDataSet(sqlQuery, null, 25);
                if (DS1.Tables[0].Rows.Count != 0)
                {
                    MessageBox.Show("Record already exists...!!! ", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }


            try
            {
                oDataAccess.BeginTransaction();
                if (this.pAddMode)
                {
                    //Added by Shrikant S. on 10/09/2015 for Bug-26664      //Start
                    byte[] data = null;
                    this.Validate();
                    if (!this.rdoUSBToken.Checked)
                    {
                        //Added by Shrikant S. on 10/09/2015 for Bug-26664      //End
                        FileInfo finfo = new FileInfo(filedialog.FileName);
                        data = File.ReadAllBytes(filedialog.FileName);
                        Pass = CryptorEngine.Encrypt(txtPass.Text, true);
                    }       //Added by Shrikant S. on 10/09/2015 for Bug-26664   

                    foreach (DataRow dr in tDs.Tables[0].Rows)
                    {

                        sqlQuery = "";
                        sqlQuery = "set dateformat mdy Insert into DigitalSign ([FileType],[FileData],[Passw],[SignBy],[OrgName],[OrgUnitNm],[Country],[State],";
                        //sqlQuery += "[Location],[Email],[Dept],[Cate],[InvSr],[Validity],[Sdate],[Edate]) values ";       //Commented by Shrikant S. on 09/09/2015 
                        sqlQuery += "[Location],[Email],[Dept],[Cate],[InvSr],[Validity],[Reason],[ShowReason],[Showlocation],[signLeft],[signBottom],[signWidth],[signHeight],[Sdate],[Edate]) values ";         //Added by Shrikant S. on 09/09/2015 
                        sqlQuery += "('" + (this.rdoUSBToken.Checked ? "USBTO" : "pfx") + "',";        //Changed by Shrikant S. on 09/09/2015 
                        sqlQuery += "@data,";
                        sqlQuery += "'" + Pass + "',";
                        sqlQuery += "'" + dr["SignBy"] + "',";
                        sqlQuery += "'" + dr["OrgName"] + "',";
                        sqlQuery += "'" + dr["OrgUnitNm"] + "',";
                        sqlQuery += "'" + dr["Country"] + "',";
                        sqlQuery += "'" + dr["State"] + "',";
                        sqlQuery += "'" + dr["Location"] + "',";
                        sqlQuery += "'" + dr["Email"] + "',";
                        sqlQuery += "'" + dr["Dept"] + "',";
                        sqlQuery += "'" + dr["Cate"] + "',";
                        sqlQuery += "'" + dr["InvSr"] + "',";
                        sqlQuery += "'" + dr["Validity"] + "',";

                        //Added by Shrikant S. on 09/09/2015        //Start
                        sqlQuery += "'" + dr["Reason"] + "',";
                        sqlQuery += (Convert.ToBoolean(dr["ShowReason"]) == true ? "1" : "0") + ",";
                        sqlQuery += (Convert.ToBoolean(dr["ShowLocation"]) == true ? "1" : "0") + ",";
                        sqlQuery += Convert.ToString(dr["signLeft"]) + ",";
                        sqlQuery += Convert.ToString(dr["signBottom"]) + ",";
                        sqlQuery += Convert.ToString(dr["signWidth"]) + ",";
                        sqlQuery += Convert.ToString(dr["signHeight"]) + ",";
                        //Added by Shrikant S. on 09/09/2015        //End

                        sqlQuery += "'" + Convert.ToDateTime(dr["Sdate"]).ToString("MM/dd/yyyy") + "',";
                        sqlQuery += "'" + Convert.ToDateTime(dr["Edate"]).ToString("MM/dd/yyyy") + "')";

                        conn.Open();
                        SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                        //Added by Shrikant S. on 10/09/2015 for Bug-26664          //Start
                        if (data == null)
                        {
                            cmd.CommandText = cmd.CommandText.Replace("@data,", "");
                            cmd.CommandText = cmd.CommandText.Replace("[FileData],", "");
                        }
                        else
                        //Added by Shrikant S. on 10/09/2015 for Bug-26664          //End
                        {
                            cmd.Parameters.AddWithValue("@data", data);
                        }
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                if (this.pEditMode)
                {
                    foreach (DataRow dr in tDs.Tables[0].Rows)
                    {
                        sqlQuery = "";
                        sqlQuery = "update DigitalSign set ";
                        sqlQuery += " [Dept]='" + dr["Dept"] + "',";
                        sqlQuery += " [Cate]='" + dr["Cate"] + "',";
                        sqlQuery += " [InvSr]='" + dr["InvSr"] + "',";
                        sqlQuery += " [Validity]='" + dr["Validity"] + "',";
                        //Added by Shrikant S. on 10/09/2015 for Bug-26664      //Start    
                        sqlQuery += " [Location]='" + dr["Location"] + "',";
                        sqlQuery += " [Reason]='" + dr["Reason"] + "',";
                        sqlQuery += " [ShowReason]=" + (this.chkReason.Checked ? 1 : 0).ToString() + ",";
                        sqlQuery += " [ShowLocation]=" + (this.chkLocation.Checked ? 1 : 0).ToString() + ",";
                        sqlQuery += " [signLeft]=" + Convert.ToString(dr["signLeft"]) + ",";
                        sqlQuery += " [signBottom]=" + Convert.ToString(dr["signBottom"]) + ",";
                        sqlQuery += " [signWidth]=" + Convert.ToString(dr["signWidth"]) + ",";
                        sqlQuery += " [signHeight]=" + Convert.ToString(dr["signHeight"]);
                        //Added by Shrikant S. on 10/09/2015 for Bug-26664      //End
                        sqlQuery += " Where id='" + dr["ID"] + "'";
                        oDataAccess.ExecuteSQLStatement(sqlQuery, null, 25, true);
                    }
                }

                if (this.pAddMode)
                {

                    this.pAddMode = false;
                    this.pEditMode = false;
                    ControlsEnable();
                    btnLast_Click(sender, e);
                }
                else
                {
                    this.pAddMode = false;
                    this.pEditMode = false;
                    ControlsEnable();
                    selectrecord();
                }

                MessageBox.Show("Records saved successfully...!!! ", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                oDataAccess.CommitTransaction();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Records could not be saved...!!! " + ex.Message, this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                oDataAccess.RollbackTransaction();
                return;
            }

        }


        #region move First/Next/Previous/Last Button Event
        private void btnLast_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.pAddMode == false && this.pEditMode == false)
                {
                    sqlQuery = "SELECT top 1 [ID],[SignBy],[OrgName],[OrgUnitNm],[Country],[State],";
                    sqlQuery += " [Location],[Email],[Dept],[Cate],[InvSr],[Validity],[Sdate],[Edate]";
                    sqlQuery += ",Reason,showReason,showLocation,signLeft,signBottom,signWidth,signHeight";     //Added by Shrikant S. on 10/09/2015 for Bug-26664
                    sqlQuery += " FROM [DigitalSign]";
                    sqlQuery += " order by ID desc";
                    tDs = oDataAccess.GetDataSet(sqlQuery, null, 25);
                }
                else
                {
                    sqlQuery = "SELECT top 1 [ID],[SignBy],[OrgName],[OrgUnitNm],[Country],[State],";
                    sqlQuery += " [Location],[Email],[Dept],[Cate],[InvSr],[Validity],[Sdate],[Edate]";
                    sqlQuery += ",Reason,showReason,showLocation,signLeft,signBottom,signWidth,signHeight";         //Added by Shrikant S. on 10/09/2015 for Bug-26664
                    sqlQuery += " FROM [DigitalSign]";
                    sqlQuery += " where id='" + tDs.Tables[0].Rows[0]["ID"] + "'";
                    tDs = oDataAccess.GetDataSet(sqlQuery, null, 25);
                }
                if (tDs.Tables[0].Rows.Count == 0)
                {
                    this.MOVEBTNSTATE(false, false, false, false);
                    this.ActionButtonState(true, false, false, false, false);
                    tlsbtnSearch.Enabled = false;
                    sqlQuery = "SELECT [ID],[SignBy],[OrgName],[OrgUnitNm],[Country],[State],";
                    sqlQuery += " [Location],[Email],[Dept],[Cate],[InvSr],[Validity],[Sdate],[Edate]";
                    sqlQuery += ",Reason,showReason,showLocation,signLeft,signBottom,signWidth,signHeight";             //Added by Shrikant S. on 10/09/2015 for Bug-26664
                    sqlQuery += " FROM [DigitalSign] where 1=2";
                    tDs = oDataAccess.GetDataSet(sqlQuery, null, 25);
                    Controlset();
                }
                else
                {
                    this.MOVEBTNSTATE(true, true, false, false);
                    tlsbtnSearch.Enabled = true;
                    Controlset();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("DigitalSign Table missing", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }



        }
        private void btnBack_Click(object sender, EventArgs e)
        {

            id = tDs.Tables[0].Rows[0]["ID"].ToString();
            sqlQuery = "SELECT top 1 [ID],[SignBy],[OrgName],[OrgUnitNm],[Country],[State],";
            sqlQuery += " [Location],[Email],Dept,[Cate],[InvSr],[Validity],[Sdate],[Edate]";
            sqlQuery += ",Reason,showLocation,showReason,signLeft,signBottom,signWidth,signHeight";     //Added by Shrikant S. on 10/09/2015 for Bug-26664
            sqlQuery += " FROM [DigitalSign]";
            sqlQuery += " where id=(select top 1 id from DigitalSign where id<'" + tDs.Tables[0].Rows[0]["ID"] + "'  order by ID desc)";

            tDs = oDataAccess.GetDataSet(sqlQuery, null, 25);
            MOVEBTNSTATE(true, true, true, true);
            if (tDs.Tables[0].Rows.Count == 0)
            {
                sqlQuery = "SELECT top 1 [ID],[SignBy],[OrgName],[OrgUnitNm],[Country],[State],";
                sqlQuery += " [Location],[Email],Dept,[Cate],[InvSr],[Validity],[Sdate],[Edate]";
                sqlQuery += ",Reason,showLocation,showReason,signLeft,signBottom,signWidth,signHeight";     //Added by Shrikant S. on 10/09/2015 for Bug-26664
                sqlQuery += " FROM [DigitalSign]";
                sqlQuery += " where id=(select top 1 id from DigitalSign where id='" + id + "'  order by ID desc)";

                tDs = oDataAccess.GetDataSet(sqlQuery, null, 25);
                MOVEBTNSTATE(false, false, true, true);
            }

            Controlset();
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            if (tDs.Tables[0].Rows.Count != 0)
            {
                sqlQuery = "SELECT top 1 [ID],[SignBy],[OrgName],[OrgUnitNm],[Country],[State],";
                sqlQuery += " [Location],[Email],[Dept],[Cate],[InvSr],[Validity],[Sdate],[Edate]";
                sqlQuery += ",Reason,showLocation,showReason,signLeft,signBottom,signWidth,signHeight";     //Added by Shrikant S. on 10/09/2015 for Bug-26664
                sqlQuery += " FROM [DigitalSign]";
                sqlQuery += " order by ID";
                tDs = oDataAccess.GetDataSet(sqlQuery, null, 25);
                Controlset();
                MOVEBTNSTATE(false, false, true, true);
            }
            else
            {
                MOVEBTNSTATE(false, false, true, true);
            }

        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            id = tDs.Tables[0].Rows[0]["ID"].ToString();
            sqlQuery = "SELECT  [ID],[SignBy],[OrgName],[OrgUnitNm],[Country],[State],";
            sqlQuery += " [Location],[Email],[Dept],[Cate],[InvSr],[Validity],[Sdate],[Edate]";
            sqlQuery += ",Reason,showLocation,showReason,signLeft,signBottom,signWidth,signHeight";     //Added by Shrikant S. on 10/09/2015 for Bug-26664
            sqlQuery += " FROM [DigitalSign]";
            sqlQuery += " where id=(select top 1 id from DigitalSign where id>'" + tDs.Tables[0].Rows[0]["ID"] + "'  order by ID)";
            tDs = oDataAccess.GetDataSet(sqlQuery, null, 25);
            MOVEBTNSTATE(true, true, true, true);
            if (tDs.Tables[0].Rows.Count == 0)
            {
                sqlQuery = "SELECT  [ID],[SignBy],[OrgName],[OrgUnitNm],[Country],[State],";
                sqlQuery += " [Location],[Email],[Dept],[Cate],[InvSr],[Validity],[Sdate],[Edate]";
                sqlQuery += ",Reason,showLocation,showReason,signLeft,signBottom,signWidth,signHeight";     //Added by Shrikant S. on 10/09/2015 for Bug-26664
                sqlQuery += " FROM [DigitalSign]";
                sqlQuery += " where id=(select top 1 id from DigitalSign where id='" + id + "'  order by ID)";
                tDs = oDataAccess.GetDataSet(sqlQuery, null, 25);
                MOVEBTNSTATE(true, true, false, false);
            }
            Controlset();
        }
        #endregion

        private void btnValidIn_Click(object sender, EventArgs e)
        {
            sqlQuery = "select code_nm,entry_ty from lcode";
            DS1 = oDataAccess.GetDataSet(sqlQuery, null, 25);
            frmValidIn frmvalid = new frmValidIn();

            frmvalid.ds = DS1;
            frmvalid.getds(DS1, txtValid.Text.ToString().Trim());

            frmvalid.ShowDialog();
            tDs.Tables[0].Rows[0]["Validity"] = frmvalid.validdata;
            this.txtValid.Text = frmvalid.validdata;        //Added by Shrikant S. on 15/09/2015 for Bug-26664
        }

        private void btnCate_Click(object sender, EventArgs e)
        {
            sqlQuery = "select cate from category";
            DS1 = oDataAccess.GetDataSet(sqlQuery, null, 25);
            DataView dvw = DS1.Tables[0].DefaultView;

            VForText = "Select Category";
            vSearchCol = "cate";
            vDisplayColumnList = "cate:Category";
            vReturnCol = "cate";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                tDs.Tables[0].Rows[0]["Cate"] = oSelectPop.pReturnArray[0];
                this.txtCate.Text = oSelectPop.pReturnArray[0];     //added by Shrikant S. on 15/09/2015 for Bug-26664
                this.txtCate.Refresh();     //added by Shrikant S. on 15/09/2015 for Bug-26664
            }
        }

        private void btnInvSr_Click(object sender, EventArgs e)
        {
            sqlQuery = "select inv_sr from series";
            DS1 = oDataAccess.GetDataSet(sqlQuery, null, 25);
            DataView dvw = DS1.Tables[0].DefaultView;

            VForText = "Select Series";
            vSearchCol = "inv_sr";
            vDisplayColumnList = "inv_sr:Series";
            vReturnCol = "inv_sr";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                tDs.Tables[0].Rows[0]["InvSr"] = oSelectPop.pReturnArray[0];
                this.txtInvSr.Text = oSelectPop.pReturnArray[0];             //added by Shrikant S. on 15/09/2015 for Bug-26664
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                oDataAccess.BeginTransaction();
                if (MessageBox.Show("Do you want to delete record ?", this.pPApplText, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    sqlQuery = " delete from DigitalSign where id='" + tDs.Tables[0].Rows[0]["ID"].ToString() + "'";
                    oDataAccess.ExecuteSQLStatement(sqlQuery, null, 25, true);

                    this.pAddMode = false;
                    this.pEditMode = false;
                    btnLast_Click(sender, e);

                    ControlsEnable();
                    MessageBox.Show("Records deleted successfully...!!! ", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                oDataAccess.CommitTransaction();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Records could not be delete...!!! " + ex.Message, this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                oDataAccess.RollbackTransaction();
                return;
            }

        }

        private void tlsbtnSearch_Click(object sender, EventArgs e)
        {
            sqlQuery = "select cast(id as varchar) as id,signby,dept,cate,invsr from DigitalSign";
            DS1 = oDataAccess.GetDataSet(sqlQuery, null, 25);
            DataView dvw = DS1.Tables[0].DefaultView;

            VForText = "Select Digital Signature";
            vSearchCol = "id";
            vDisplayColumnList = "signby:Signed By,dept:Department,cate:Category,invsr:Invoice Series";
            vReturnCol = "id";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                tDs.Tables[0].Rows[0]["id"] = oSelectPop.pReturnArray[0];
            }
            selectrecord();
        }

        private void selectrecord()
        {
            sqlQuery = "SELECT top 1 [ID],[SignBy],[OrgName],[OrgUnitNm],[Country],[State],";
            sqlQuery += " [Location],[Email],[Dept],[Cate],[InvSr],[Validity],[Sdate],[Edate]";
            sqlQuery += ",Reason,showLocation,showReason,signLeft,signBottom,signWidth,signHeight";     //Added by Shrikant S. on 10/09/2015 for Bug-26664
            sqlQuery += " FROM [DigitalSign]";
            sqlQuery += " where id='" + tDs.Tables[0].Rows[0]["ID"] + "'";
            tDs = oDataAccess.GetDataSet(sqlQuery, null, 25);
            if (tDs.Tables[0].Rows.Count == 0)
            {
                this.MOVEBTNSTATE(false, false, false, false);
                this.ActionButtonState(true, false, false, false, false);
            }
            Controlset();
        }

        private void mInsertProcessIdRecord()
        {

            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "udDigitalSigSetting.exe";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            sqlstr = "Set DateFormat dmy  insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";
            oDataAccess.ExecuteSQLStatement(sqlstr, null, 25, true);
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
            oDataAccess.ExecuteSQLStatement(sqlstr, null, 25, true);
        }

        private void frmDigitalSigSetting_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.N:
                        if (this.pAddMode == false && this.pEditMode == false)
                        {
                            this.btnNew_Click(sender, e);
                        }
                        break;
                    case Keys.Z:
                        if (this.pAddMode || this.pEditMode)
                        {
                            this.btnCancel_Click(sender, e);
                        }
                        break;
                    case Keys.S:
                        if (this.pAddMode || this.pEditMode)
                        {
                            this.btnSave_Click(sender, e);
                        }
                        break;
                    case Keys.D:
                        if (this.pAddMode == false && this.pEditMode == false)
                        {
                            this.btnDelete_Click(sender, e);
                        }
                        break;
                    case Keys.E:
                        if (this.pAddMode == false && this.pEditMode == false)
                        {
                            this.btnEdit_Click(sender, e);
                        }
                        break;
                    default:
                        break;
                }

            }
        }
        //  Added by Shrikant S. on 09/09/2015 for      //Start
        private void txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            int asciiCode = Convert.ToInt32(e.KeyChar);
            if ((asciiCode >= 48 && asciiCode <= 57) || asciiCode == 8)
            {
            }
            else
                e.Handled = true;
        }

        private void BoolToString(object sender, ConvertEventArgs e)
        {
            if (e.Value.ToString() == "1")
            {
                e.Value = true;
            }
            else
            {
                e.Value = false;
            }
        }
        private void StringToBool(object sender, ConvertEventArgs e)
        {
            if ((bool)e.Value == true)
            {
                e.Value = "1";
            }
            else
            {
                e.Value = "0";
            }
        }
        private void IntToString(object sender, ConvertEventArgs e)
        {
            if (e.DesiredType == typeof(string))
            {
                // Use the ToString method to format the value as string .
                if (e.Value.ToString() != "")
                {
                    e.Value = ((int)e.Value).ToString();
                }
                else
                {
                    e.Value = "0";
                }
            }
        }
        private void StringToInt(object sender, ConvertEventArgs e)
        {
            if (e.DesiredType == typeof(int))
            {
                //Convert the string back to Date using the static Parse() method.
                if (e.Value.ToString() != "")
                {
                    e.Value = int.Parse(e.Value.ToString(), System.Globalization.NumberStyles.Integer);
                }
                else
                {
                    e.Value = int.Parse("0", System.Globalization.NumberStyles.Integer);
                }
            }
        }

        private void rdoUSBToken_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdoUSBToken.Checked)
            {
                this.txtPass.Enabled = false;
                this.btnValidateSign.Enabled = false;

            }
            else
            {
                this.txtPass.Enabled = true;
                this.btnValidateSign.Enabled = true;
            }
        }

        private void chkLocation_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkReason_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtLocation_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtLocation_Validated(object sender, EventArgs e)
        {
            if (txtLocation.Text.Length > 0)
                chkLocation.Enabled = true;
            else
                chkLocation.Enabled = false;
        }

        private void txtReason_Validated(object sender, EventArgs e)
        {
            if (txtReason.Text.Length > 0)
                chkReason.Enabled = true;
            else
                chkReason.Enabled = false;
        }

        private void txtLeft_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private string GetRegisterDetails()
        {
            string retvalue = string.Empty;

            UdyogRegister reg = new UdyogRegister();
            m_info = reg.RegistrationInfo;

            retvalue = m_info.CompanyName;
            retvalue = retvalue + "^" + m_info.InstallationAddress.Line1;
            retvalue = retvalue + "^" + m_info.InstallationAddress.Line2;
            retvalue = retvalue + "^" + m_info.InstallationAddress.Line3;
            retvalue = retvalue + "^" + m_info.InstallationAddress.Location;
            retvalue = retvalue + "^" + m_info.InstallationAddress.Zip;
            retvalue = retvalue + "^" + m_info.InstallationAddress.EMail;
            retvalue = retvalue + "^" + m_info.ServiceCenterCode;
            retvalue = retvalue + "^" + m_info.DBServerIP;
            retvalue = retvalue + "^" + m_info.DBServerName;
            retvalue = retvalue + "^" + m_info.ApplicationServerIP;
            retvalue = retvalue + "^" + m_info.ApplicationServerName;
            retvalue = retvalue + "^" + m_info.MACId;
            retvalue = retvalue + "^" + m_info.MaximumNumberOfCompaniesAllowed;
            retvalue = retvalue + "^" + m_info.MaximumNumberOfUsersAllowed;
            retvalue = retvalue + "^" + m_info.ServiceType;
            retvalue = retvalue + "^" + m_info.ProductId;

            return retvalue;
        }

        private void txtDept_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                btnDept.PerformClick();
            }
        }

        private void txtCate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                btnCate.PerformClick();
            }
        }

        private void txtInvSr_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                btnInvSr.PerformClick();
            }
        }

        private void txtValid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                btnValidIn.PerformClick();
            }
        }
        //  Added by Shrikant S. on 09/09/2015 for      //End

    }
}
