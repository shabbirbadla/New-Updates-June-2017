using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

using uBaseForm;
using udCrViewer;

namespace udReportList
{
    public class cReportList
    {
        //string pEntry_Ty = "BP";
        Int16 vTran_Cd;// = 116;

        string[] vPara;
        string SqlStr = "", vRepGroup = "", vAppPath = "", vPApplText = "";
        DataAccess_Net.clsDataAccess oDataAccess;
        DataTable vTblRepList;
        DataSet vDsCommon;
        Boolean vWaitPrint = false, vBtnCancelPress = false;
        Int16 vPrintOption;
        string vComDbnm = "", vServerName = "", vUserId = "", vPassword = "", vSpPara="";

        public void Main()
        {
            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();



            SqlStr = "Select Sel=cast(0 as bit),* From R_Status where [group]='" + this.pRepGroup + "'";
            DataTable vTblRepList = new DataTable();
            this.pTblRepList = oDataAccess.GetDataTable(SqlStr, null, 25);

            if (this.pTblRepList.Rows.Count == 1)
            {
                this.pTblRepList.Rows[0]["sel"] = true;
            }
            else if (this.pTblRepList.Rows.Count == 0)
            {
                MessageBox.Show(" No Record found in Report Wizards for Report Group " + this.pRepGroup , this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;    
            }
            else
            {
                frmReportList oForm = new frmReportList();
                oForm.pTblRepList = this.pTblRepList;
                oForm.pPrintOption = this.pPrintOption; 
                oForm.ShowDialog();
                this.pWaitPrint = oForm.pWaitPrint;
                this.pBtnCancelPress = oForm.pBtnCancelPress;
            }
            if (this.pBtnCancelPress == false)
            {
                this.mthPrint();
            }
        }
        private void mthPrint()
        {
            
            string vReportPath = "";
            string appPath = "";
            string appName = "";
            int cnt=0;
            
            string vPDF_Path = "";
            vPDF_Path = this.pDsCommon.Tables["CoAdditional"].Rows[0]["PDF_Path"].ToString().Trim();

            if ((this.pPrintOption == 4 || this.pPrintOption == 7) && string.IsNullOrEmpty(vPDF_Path))
            {
                vPDF_Path= Path.GetDirectoryName(Path.GetFullPath(Application.ExecutablePath)) + (this.pAppPath.Trim().Substring(this.pAppPath.Trim().Length - 1, 1) != @"\" ? @"\" : "") + @"PDF_Files\";
                SqlStr = "update Manufact Set PDF_Path='"+vPDF_Path+"'";
                oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);
                Directory.CreateDirectory(vPDF_Path);
                MessageBox.Show("Folder Created " + vPDF_Path + " to Store pdf Files", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information); 
            }
            
            foreach (DataRow dr in vTblRepList.Rows)
            {

                int pos = -1;
                if ((Boolean)dr["Sel"])
                {
                    vReportPath = pDsCommon.Tables["company"].Rows[0]["Dir_Nm"].ToString().Trim() + (pDsCommon.Tables["company"].Rows[0]["Dir_Nm"].ToString().Trim().Substring((pDsCommon.Tables["company"].Rows[0]["Dir_Nm"].ToString().Trim()).Length - 1, 1) != @"\" ? @"\" : "") + dr["Rep_Nm"].ToString().Trim() + ".rpt";

                    SqlStr = dr["SqlQuery"].ToString();
                    pos = SqlStr.IndexOf(";");
                    if (SqlStr.ToLower().IndexOf("execute") == -1)
                    {
                        MessageBox.Show(" Please use Strored Procedure insetead of Query ''" + SqlStr + "''", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        SqlStr = "";
                    }
                    else
                    {
                        if (pos > 0)
                        {
                            SqlStr = SqlStr.Substring(0, pos);
                            //SqlStr = SqlStr + " ' A.Entry_ty=''" + this.pEntry_Ty + "'' AND A.TRAN_CD =" + this.pTran_Cd.ToString().Trim() + "'";
                            SqlStr = SqlStr+" "+ this.pSpPara.Trim();
                        }
                        else
                        {
                            MessageBox.Show(" Invalid SQl Statement " + SqlStr, this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                        if (File.Exists(vReportPath) == false)
                        {
                            MessageBox.Show(vReportPath + " File not found", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                    }
                    cnt=cnt+1;
                    if (this.pWaitPrint && cnt > 1)
                    {
                        MessageBox.Show("Printing Continue...", dr["Desc"].ToString(), MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                    }
                    if (File.Exists(vReportPath) && SqlStr != "")
                    {

                        DataTable vResulSet = new DataTable();
                        vResulSet = oDataAccess.GetDataTable(SqlStr, null, 80);
                        //MessageBox.Show(SqlStr);
                        appPath = this.pAppPath;
                        appPath = Path.GetDirectoryName(appPath);
                        udCrViewer.cCrViewer oCrViewer = new udCrViewer.cCrViewer();
                        oCrViewer.pAppPath = this.pAppPath;
                        //oCrViewer.pPara = this.pPara;
                        oCrViewer.pReportPath = vReportPath;
                        oCrViewer.pResulSet = vResulSet;
                        oCrViewer.pDsCommon = this.pDsCommon;
                        oCrViewer.pPrintOption = this.pPrintOption;
                        oCrViewer.pWaitPrint = this.pWaitPrint;
                        oCrViewer.pFrmCaption = dr["Desc"].ToString().Trim() + " (" + dr["Rep_Nm"].ToString().Trim() + ".rpt)";
                        oCrViewer.pPageBreakFieldList = dr["PgBrakFld"].ToString();
                        oCrViewer.pRepHead = dr["ExpFileNm"].ToString().Trim();
                        oCrViewer.Main();
                        //appPath = appPath + @"\";
                        //appName = "udCrViewer.exe";
                        //appPath = @appPath.Trim() + appName.Trim();

                        //udCrViewer.frmCrViewer oCrvForm = new frmCrViewer();
                        //oCrvForm.pAppPath = this.pAppPath;
                        //oCrvForm.pPara = this.pPara;
                        //oCrvForm.pReportPath = vReportPath;
                        //oCrvForm.pResulSet = vResulSet;
                        //oCrvForm.pdsCommon = this.pDsCommon;
                        //oCrvForm.pPrintOption = this.pPrintOption;
                        //oCrvForm.pWaitPrint = this.pWaitPrint;
                        //oCrvForm.pFrmCaption = dr["Desc"].ToString().Trim() + " (" + dr["Rep_Nm"].ToString().Trim() + ".rpt)";
                        //oCrvForm.pPageBreakFieldList = dr["PgBrakFld"].ToString();
                        //oCrvForm.Show();

                  

                    }

                }

            }//For

        }
        public string[] pPara
        {
            get { return vPara; }
            set { vPara = value; }
        }
        public string[] pRepList
        {
            get { return pRepList; }
            set { pRepList = value; }
        }
        public string pRepGroup
        {
            get { return vRepGroup; }
            set { vRepGroup = value; }
        }
        public DataTable pTblRepList
        {
            get { return vTblRepList; }
            set { vTblRepList = value; }
        }
        public DataSet pDsCommon
        {
            get { return vDsCommon; }
            set { vDsCommon = value; }
        }
        public Boolean pWaitPrint
        {
            get { return vWaitPrint; }
            set { vWaitPrint = value; }
        }
        public Boolean pBtnCancelPress
        {
            get { return vBtnCancelPress; }
            set { vBtnCancelPress = value; }
        }
        public string pComDbnm
        {
            get
            {
                return vComDbnm;
            }
            set
            {
                vComDbnm = value;
            }
        }
        public string pServerName
        {
            get
            {
                return vServerName;
            }
            set
            {
                vServerName = value;
            }
        }
        public string pUserId
        {
            get
            {
                return vUserId;
            }
            set
            {
                vUserId = value;
            }
        }
        public string pPassword
        {
            get
            {
                return vPassword;
            }
            set
            {
                vPassword = value;
            }
        }
        public string pAppPath
        {
            get
            {
                return vAppPath;
            }
            set
            {
                vAppPath = value;
            }
        }
        public string pPApplText
        {
            get
            {
                return vPApplText;
            }
            set
            {
                vPApplText = value;
            }
        }
        public Int16 pPrintOption
        {

            get
            {
                return vPrintOption;
            }
            set
            {
                vPrintOption = value;
            }
        }
        //public Int16 pTran_Cd
        //{

        //    get
        //    {
        //        return vTran_Cd;
        //    }
        //    set
        //    {
        //        vTran_Cd = value;
        //    }
        //}
        public string pSpPara
        {

            get
            {
                return vSpPara;
            }
            set
            {
                vSpPara = value;
            }
        }

    }
}
