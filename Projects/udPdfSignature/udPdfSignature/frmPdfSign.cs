using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iTextSharp.text.pdf;
//using ueconnect;      //Commented by Shrikant S. on 16/09/2015 for Bug-26664
using GetInfo;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Diagnostics;
using Udyog.Library.Common;     //Added by Shrikant S. on 16/09/2015 for Bug-26664

namespace udPdfSignature
{
    public partial class frmPdfSign : uBaseForm.FrmBaseForm
    {
        #region Variable declaration
        string startupPath = string.Empty;
        string ServiceType = string.Empty;
        string sqlQuery = string.Empty;
        string Pass = string.Empty, dept = string.Empty, cate = string.Empty, inv_sr = string.Empty, entry_ty = string.Empty, invdt = string.Empty;
        string InPdfPath = string.Empty, OutPdfPath = string.Empty;
        // Added by Sachin N. S. on 18/09/2015 for Bug-26664 -- Start
        int ApplPId = 0;
        string ApplCode="", cAppPId="";
        string cAppName = "", AppCaption=""; 
        // Added by Sachin N. S. on 18/09/2015 for Bug-26664 -- End

        private RegisterMeInfo m_info;           //Added by Shrikant S. on 16/09/2015 for Bug-26664

        DataSet tDs;
        //clsConnect oConnect;      //Commented by Shrikant S. on 16/09/2015 for Bug-26664
        DataAccess_Net.clsDataAccess oDataAccess = new DataAccess_Net.clsDataAccess();
        #endregion

        public frmPdfSign(string[] args)
        {
            InitializeComponent();

            this.pPara = args;
            this.InPdfPath = args[0];
            this.pCompId = Convert.ToInt16(args[1]);
            this.pComDbnm = args[2];
            this.pServerName = args[3];
            this.pUserId = args[4];
            this.pPassword = args[5];
            this.dept = args[6].Replace("<*#*>", " ").Trim();//args[6] == "<#>" ? "" : args[6];
            this.cate = args[7].Replace("<*#*>", " ").Trim();// == "<#>" ? "" : args[7];
            this.inv_sr = args[8].Replace("<*#*>", " ").Trim();// == "<#>" ? "" : args[8];
            this.entry_ty = args[9];
            this.invdt = args[10];
            // Added by Sachin N. S. on 18/09/2015 for Bug-26664 -- Start

            this.AppCaption = args[11].ToString().Replace("<*#*>", " ").ToString();
            this.cAppName = args[12].ToString();
            this.ApplPId = Convert.ToInt16(args[13]);
            this.ApplCode = args[14].ToString();
            // Added by Sachin N. S. on 18/09/2015 for Bug-26664 -- End
        }

        private void frmPdfSign_Load(object sender, EventArgs e)
        {
            #region Connect To database
            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();

            startupPath = Application.StartupPath;
            //startupPath = @"D:\VudyogSDK";          // To be Removed Sachin 
            //oConnect = new clsConnect();      //Commented by Shrikant S. on 16/09/2015 for Bug-26664

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
            }
            #endregion

            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.DateSeparator = "/";
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;

            this.mInsertProcessIdRecord();      // Added by Sachin N. S. on 18/09/2015 for Bug-26664

            this.digitalsign();

            Application.Exit();
        }

        //Added by Shrikant S. on 16/09/2015 for Bug-26664          //Start
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
        //Added by Shrikant S. on 16/09/2015 for Bug-26664          //End
        #region Sign pdf with digital signature method //digitalsign()


        private void digitalsign()
        {
            DateTime date = DateTime.ParseExact(invdt, "dd/MM/yyyy", CultureInfo.InvariantCulture);//Convert.ToDateTime(invdt.ToString("MM/dd/yyyy"));
            Stream Path = null;
            //sqlQuery = "select FileData,Passw from DigitalSign where dept='" + dept + "' and cate='" + cate + "' and invsr='" + inv_sr + "' and validity like '%" + entry_ty + "%' and ('" + date.ToString("yyyy/MM/dd") + "'>=convert(smalldatetime,sdate) and '" + date.ToString("yyyy/MM/dd") + "'<=convert(smalldatetime,edate))";        //Commented by Shrikant S. on 10/09/2015 for Bug-26664
            sqlQuery = "set dateformat mdy select * from DigitalSign where dept='" + dept + "' and cate='" + cate + "' and invsr='" + inv_sr + "' and validity like '%" + entry_ty + "%' and ('" + date.ToString("yyyy/MM/dd") + "'>=convert(smalldatetime,sdate) and '" + date.ToString("yyyy/MM/dd") + "'<=convert(smalldatetime,edate))";          //Added by Shrikant S. on 10/09/2015 for Bug-26664

            tDs = oDataAccess.GetDataSet(sqlQuery, null, 25);
            if (tDs.Tables[0].Rows.Count == 0)
            {
                return;
            }
            if (tDs.Tables[0].Rows.Count > 1)
            {
                PDFSigner pdfsign = new PDFSigner();
                pdfsign.appCap = this.AppCaption;
                string certName = pdfsign.GetCertificateName();
                if (certName == string.Empty)
                    return;

                tDs.Tables[0].Rows.Clear();
                tDs.Tables.RemoveAt(0);

                sqlQuery = "set dateformat mdy select * from DigitalSign where dept='" + dept + "' and cate='" + cate + "' and invsr='" + inv_sr + "' and validity like '%" + entry_ty + "%' and ('" + date.ToString("yyyy/MM/dd") + "'>=convert(smalldatetime,sdate) and '" + date.ToString("yyyy/MM/dd") + "'<=convert(smalldatetime,edate)) and SignBy='" + certName + "'";          //Added by Shrikant S. on 10/09/2015 for Bug-26664
                tDs = oDataAccess.GetDataSet(sqlQuery, null, 25);
                pdfsign = null;
            }
            //Added by Shrikant S. on 10/09/2015 for Bug-26664  //Start
            Int32 left = Convert.ToInt32(tDs.Tables[0].Rows[0]["signLeft"]);
            Int32 bottom = Convert.ToInt32(tDs.Tables[0].Rows[0]["signBottom"]);
            Int32 width = left + Convert.ToInt32(tDs.Tables[0].Rows[0]["signWidth"]);
            Int32 height = bottom + Convert.ToInt32(tDs.Tables[0].Rows[0]["signHeight"]);

            string reason = (Convert.ToString(tDs.Tables[0].Rows[0]["Reason"]).Length > 0 && Convert.ToBoolean(tDs.Tables[0].Rows[0]["ShowReason"]) ? Convert.ToString(tDs.Tables[0].Rows[0]["Reason"]) : string.Empty);
            string location = (Convert.ToString(tDs.Tables[0].Rows[0]["Location"]).Length > 0 && Convert.ToBoolean(tDs.Tables[0].Rows[0]["ShowLocation"]) ? Convert.ToString(tDs.Tables[0].Rows[0]["Location"]) : string.Empty);
            string SignBy = Convert.ToString(tDs.Tables[0].Rows[0]["SignBy"]);

            iTextSharp.text.Rectangle rect = new iTextSharp.text.Rectangle(left, bottom, width, height);

            InPdfPath = InPdfPath.ToLower();            
            OutPdfPath = InPdfPath.ToString().Replace(".pdf", "_digital.pdf");      

            if (Convert.ToString(tDs.Tables[0].Rows[0]["FileType"]) == "USBTO")
            {
                PDFSigner pdfs = new PDFSigner();
                pdfs.appCap = this.AppCaption;
                if (!pdfs.processCertificate(InPdfPath, OutPdfPath, SignBy, rect, reason, location))
                {
                    return;
                }
            }
            else
            {
                //Added by Shrikant S. on 10/09/2015 for Bug-26664  //End
                Path = new MemoryStream((byte[])tDs.Tables[0].Rows[0]["FileData"]);
                Pass = CryptorEngine.Decrypt(tDs.Tables[0].Rows[0]["Passw"].ToString(), true);

                //OutPdfPath = InPdfPath.ToString().Replace(".pdf", "_digital.pdf");      //Commented by Shrikant S. on 10/09/2015 for Bug-26664
                //Sign Pdf with digital signature
                PDFSigner pdfs = new PDFSigner();
                pdfs.appCap = this.AppCaption;
                pdfs.processCert(Path, Pass);
                pdfs.Sign(InPdfPath, OutPdfPath, "", true,rect,reason,location);
            }               //Added by Shrikant S. on 10/09/2015 for Bug-26664  
            /*Delete file without having digital signature and rename digital signature 
			file with old file name*/
            File.Delete(InPdfPath);
            FileInfo info = new FileInfo(OutPdfPath);
            info.MoveTo(InPdfPath);
        }
        #endregion


        #region Generate Process Id's       // Added by Sachin N. S. on 18/09/2015 for Bug-26664 -- Start
        private void mInsertProcessIdRecord()
        {
            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "udPdfSignature.exe";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            this.Text = "PDF Signature";
            sqlstr = "Set DateFormat dmy insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + ApplCode + "','" + DateTime.Now.Date.ToString() + "','" + cAppName + "'," + ApplPId + ",'" + AppCaption + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";
            oDataAccess.ExecuteSQLStatement(sqlstr, null, 20, true);
        }
        private void mDeleteProcessIdRecord()
        {
            if (string.IsNullOrEmpty(cAppName) || ApplPId == 0 || string.IsNullOrEmpty(this.cAppName) || string.IsNullOrEmpty(this.cAppPId))
            {
                return;
            }
            DataSet dsData = new DataSet();
            string sqlstr;
            sqlstr = " Delete from vudyog..ExtApplLog where pApplNm='" + cAppName + "' and pApplId=" + ApplPId + " and cApplNm= '" + cAppName + "' and cApplId= " + cAppPId;
            oDataAccess.ExecuteSQLStatement(sqlstr, null, 20, true);
        }
        #endregion Generate Process Id's    
        
        private void frmPdfSign_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.mDeleteProcessIdRecord();
        }
        // Added by Sachin N. S. on 18/09/2015 for Bug-26664 -- End

    }
}
