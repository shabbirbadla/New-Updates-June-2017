using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using ueconnect;
using GetInfo;
using System.IO;
using System.Globalization;
using System.Threading;


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

        DataSet tDs;
        clsConnect oConnect;
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
            oConnect = new clsConnect();
            GetInfo.iniFile ini = new GetInfo.iniFile(startupPath + "\\" + "Visudyog.ini");
            string appfile = ini.IniReadValue("Settings", "xfile").Substring(0, ini.IniReadValue("Settings", "xfile").Length - 4);
            oConnect.InitProc("'" + startupPath + "'", appfile);
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
                string[] objRegisterMe = (oConnect.ReadRegiValue(startupPath)).Split('^');
                ServiceType = objRegisterMe[15].ToString().Trim();
            }
            #endregion
            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.DateSeparator = "/";
            ci.DateTimeFormat.ShortDatePattern = "MM/dd/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;

            this.digitalsign();

            Application.Exit();
        }

        #region Sign pdf with digital signature method //digitalsign()
        private void digitalsign()
        {
            DateTime date = DateTime.ParseExact(invdt, "dd/MM/yyyy", CultureInfo.InvariantCulture);//Convert.ToDateTime(invdt.ToString("MM/dd/yyyy"));
            Stream Path = null;
            sqlQuery = "select FileData,Passw from DigitalSign where dept='" + dept + "' and cate='" + cate + "' and invsr='" + inv_sr + "' and validity like '%" + entry_ty + "%' and ('" + date.ToString("yyyy/MM/dd") + "'>=convert(smalldatetime,sdate) and '" + date.ToString("yyyy/MM/dd") + "'<=convert(smalldatetime,edate))";

            tDs = oDataAccess.GetDataSet(sqlQuery, null, 25);
            if (tDs.Tables[0].Rows.Count == 0)
            {
                return;
            }
            Path = new MemoryStream((byte[])tDs.Tables[0].Rows[0]["FileData"]);
            Pass = CryptorEngine.Decrypt(tDs.Tables[0].Rows[0]["Passw"].ToString(), true);

            OutPdfPath = InPdfPath.ToString().Replace(".PDF", "_digital.pdf");

            //Sign Pdf with digital signature
            PDFSigner pdfs = new PDFSigner();
            pdfs.processCert(Path, Pass);
            pdfs.Sign(InPdfPath, OutPdfPath, "",true);

            /*Delete file without having digital signature and rename digital signature 
            file with old file name*/
            File.Delete(InPdfPath);
            FileInfo info = new FileInfo(OutPdfPath);
            info.MoveTo(InPdfPath);
        }
        #endregion

    }
}
