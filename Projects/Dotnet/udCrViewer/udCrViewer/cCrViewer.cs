using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.Drawing;

using System.IO;
using System.Reflection;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Windows.Forms;
using CrystalDecisions.ReportSource;
using Microsoft.Office.Interop.Outlook;

using udDataTableQuery;

namespace udCrViewer
{
    public class cCrViewer
    {
        DataTable vResulSet;
        string[] vPageBreakField, vPara;
        string vPageBreakFieldList, vReportPath, vAppPath, vFrmCaption, vRepHead,
             vUserName = ""; //Added By Kishor A. for bug-28186;
        DataSet vdsCommon;
        Int16 vPrintOption;
        Boolean vWaitPrint;
        public void Main()
        {
            MessageBox.Show("Testing CR Viewer");
            vPageBreakField = this.pPageBreakFieldList.Split(',');
            string vFldList = "";
            string[] vFieldList={};
            string vPdfFileNm = "";
            DataTable tblDistinct = new DataTable();
            if (vPageBreakField.Length > 1)
            {
                foreach (DataColumn dtc in this.pResulSet.Columns)
                {
                    vFldList = vFldList + "," + dtc.ColumnName.Trim();
                }

                vFldList = vFldList.Substring(1, vFldList.Length - 1);
                vFieldList = vFldList.Split(',');
                udDataTableQuery.cSelectDistinct oSelDistinct = new udDataTableQuery.cSelectDistinct();
                tblDistinct = oSelDistinct.SelectDistinct(this.pResulSet, "", this.vPageBreakField);
            }

            MessageBox.Show("Testing CR Viewer -- 1");

            if (vPageBreakField.Length > 1 && tblDistinct.Rows.Count > 1)
            {
                string vFiltCon = "";
                string vvFrmCaption = "";
                foreach (DataRow tDr in tblDistinct.Rows)
                {
                    MessageBox.Show("Testing CR Viewer -- 1.1");
                    vFiltCon = "";
                    vvFrmCaption = "";
                    vPdfFileNm = "";
                    foreach (DataColumn tCol in tblDistinct.Columns)
                    {
                        if (vFiltCon != "")
                        {
                            vFiltCon = vFiltCon + " and ";
                        }
                        vFiltCon = vFiltCon + tCol.ColumnName.Trim() + "=" + ((tCol.DataType.Name.ToLower() == "string" || tCol.DataType.Name.ToLower() == "string") ? "'" : "") + tDr[tCol.ColumnName].ToString().Trim() + ((tCol.DataType.Name.ToLower() == "string" || tCol.DataType.Name.ToLower() == "string") ? "'" : "");
                        vvFrmCaption = vvFrmCaption + "_" + tDr[tCol.ColumnName].ToString().Trim();
                        vPdfFileNm = vPdfFileNm + "_" + tDr[tCol.ColumnName].ToString().Trim();
                    }

                    MessageBox.Show("Testing CR Viewer -- 1.2");
                    DataTable tblRes = new DataTable();
                    udDataTableQuery.cFilterDataTable oFilterDataTable = new udDataTableQuery.cFilterDataTable();
                    tblRes = oFilterDataTable.FilterDataTable(this.pResulSet, vFiltCon, vFieldList);
                    //vvFrmCaption = this.pFrmCaption + vvFrmCaption ;
                    //if (this.pPrintOption == 2)
                    //{
                    //    this.mthViewReport(tblRes, vvFrmCaption);
                        
                    //}
                    //else if (this.pPrintOption == 4 || this.pPrintOption == 7)
                    //{
                    //    this.mthPrintFile("pdf", tblRes, vvFrmCaption, this.pRepHead+vPdfFileNm);
                    //}
                    MessageBox.Show("Testing CR Viewer -- 1.3");
                    this.mthPrintFile("pdf", tblRes, vvFrmCaption, this.pRepHead + vPdfFileNm);
                    MessageBox.Show("Testing CR Viewer -- 2");             
                }
            }
            else
            {
                MessageBox.Show("Testing CR Viewer -- 3");
                this.mthPrintFile("pdf", this.pResulSet, this.pFrmCaption, this.pRepHead + vPdfFileNm);
                MessageBox.Show("Testing CR Viewer -- 4");
                //if (this.pPrintOption == 2)
                //{
                //    this.mthViewReport(this.pResulSet, this.pFrmCaption);

                //}
                //else if (this.pPrintOption == 4 || this.pPrintOption == 7)
                //{
                //    this.mthPrintFile("pdf", this.pResulSet, this.pFrmCaption, this.pFrmCaption);
                //}
            }
        }
      
        
        private void mthPrintFile(string FileType, DataTable tResultSet, string vvFrmCaption, string vPdfFileNm)
        {
            MessageBox.Show("Testing 2");
            ReportDocument cr = new ReportDocument();
            //DataSet dsCrview = new DataSet();
            cr.Load(vReportPath);

            MessageBox.Show("Testing 3");

            //--->Fill DataSet
            //MessageBox.Show(" 1- "+ this.pPrintOption.ToString());
            try
            {
                cr.SetDataSource(tResultSet);
                CrystalDecisions.Shared.ParameterFields parafld1 = new CrystalDecisions.Shared.ParameterFields();
                CrystalDecisions.Shared.ParameterField parafld = new CrystalDecisions.Shared.ParameterField();
                CrystalDecisions.Shared.ParameterValues currValue = new CrystalDecisions.Shared.ParameterValues();

                CrystalDecisions.Shared.ParameterDiscreteValue paramrange = new CrystalDecisions.Shared.ParameterDiscreteValue();
                string tblnm = "", fldnm = "";

                for (int i = 0; i <= cr.ParameterFields.Count - 1; i++)
                {
                    string pstr = cr.ParameterFields[i].Name;
                    try
                    {
                        if (pstr.IndexOf(".") > -1)
                        {
                            tblnm = pstr.Substring(0, pstr.IndexOf("."));
                            fldnm = pstr.Substring(pstr.IndexOf(".") + 1);
                            currValue.AddValue(this.pDsCommon.Tables[tblnm].Rows[0][fldnm]);
                            cr.DataDefinition.ParameterFields[i].ApplyCurrentValues(currValue);
                        }
                    }
                    catch (System.Exception  ex)
                    {
                        //MessageBox.Show(fldnm + " Field Not Found in " + tblnm);
                    }
                }//for (int i = 0; i<=cr.ParameterFields.Count - 1; i++)


                if (this.pPrintOption == 2 || this.pPrintOption == 3)
                {
                    if (this.pPrintOption == 2)     // Added by Sachin N. S. on 25/02/2013 for Bug-7304
                    {
                        //MessageBox.Show(" 2- " + this.pPrintOption.ToString());
                        frmCrViewer ofrmCrViewer = new frmCrViewer();
                        //ofrmCrViewer.pAppPath = this.pAppPath;
                        //ofrmCrViewer.pPara = this.pPara;
                        //ofrmCrViewer.pReportPath = vReportPath;
                        //ofrmCrViewer.pResulSet = tResultSet;
                        //ofrmCrViewer.pdsCommon = this.pDsCommon;
                        //ofrmCrViewer.pPrintOption = this.pPrintOption;
                        //ofrmCrViewer.pWaitPrint = this.pWaitPrint;
                        ofrmCrViewer.pFrmCaption = vvFrmCaption;
                        //ofrmCrViewer.pPageBreakFieldList = this.pPageBreakFieldList;

                        ofrmCrViewer.pPrintOption = this.pPrintOption;
                        ofrmCrViewer.pReportDoc = cr;
                        ofrmCrViewer.Show();
                    }
                    //******** Added by Sachin N. S. on 25/02/2013 for Bug-7304 -- Start ********\\
                    else
                    {
                        cr.PrintToPrinter(1, false, 0, 0);
                    }
                    //******** Added by Sachin N. S. on 25/02/2013 for Bug-7304 -- End ********\\
                }
                else if (this.pPrintOption == 4 || this.pPrintOption == 7)
                {
                    CrystalDecisions.CrystalReports.Engine.ReportDocument oCrReport = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                    ExportOptions crExportOption = new ExportOptions();
                    DiskFileDestinationOptions crFileDestination = new DiskFileDestinationOptions();
                    PdfRtfWordFormatOptions pdfRptOption = new PdfRtfWordFormatOptions();
                    crFileDestination.DiskFileName = this.pDsCommon.Tables["CoAdditional"].Rows[0]["PDF_Path"].ToString().Trim() + vPdfFileNm + ".pdf";
                    //crFileDestination.DiskFileName = @"D:\gabc.pdf";
                    //MessageBox.Show(crFileDestination.DiskFileName);


                    crExportOption.ExportDestinationType = ExportDestinationType.DiskFile;
                    crExportOption.ExportFormatType = ExportFormatType.PortableDocFormat;
                    crExportOption.DestinationOptions = crFileDestination;
                    crExportOption.FormatOptions = pdfRptOption;

                    cr.Export(crExportOption);
                    if (this.pPrintOption == 7)
                    {
                        this.mthSendEmail(crFileDestination.DiskFileName, tResultSet);
                    }
                }
                

            }
            catch (System.Exception   ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            //<---Fill DataSet


        }
        private string ReadSignature()
        {
            string appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\Signatures";
            string signature = string.Empty;
            DirectoryInfo diInfo = new DirectoryInfo(appDataDir);
            if (diInfo.Exists)
            {
                FileInfo[] fiSignature = diInfo.GetFiles("*.txt");
                if (fiSignature.Length > 0)
                {
                    StreamReader sr = new StreamReader(fiSignature[0].FullName, Encoding.Default);
                    signature = sr.ReadToEnd();

                    if (!string.IsNullOrEmpty(signature))
                    {
                        string fileName = fiSignature[0].Name.Replace(fiSignature[0].Extension, string.Empty);
                        signature = signature.Replace(fileName + "_files/", appDataDir + "/" + fileName + "_files/");
                    }
                }
            }

            return signature;
        }
        private void mthSendEmail_1(string vPdfFileNm, DataTable tResultSet)
        {

            string veMailId="rupeshprajapati@udyogsoftware.com";
            veMailId = "vasant@udyogsoftware.com";

            try
            {
                
                Microsoft.Office.Interop.Outlook.Application oOutLookApp = new Microsoft.Office.Interop.Outlook.Application();
                Microsoft.Office.Interop.Outlook.MailItem oMsg;   //                oOutLookApp .CreateItem ( Microsoft.Office.Interop.Outlook.MailItem);
                
                oMsg =(Microsoft .Office .Interop .Outlook .MailItem )oOutLookApp.CreateItem(OlItemType.olMailItem);   // Convert.tooOutLookApp.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
                oMsg.Subject = tResultSet.Rows[0]["EMailSub"].ToString();
                
                oMsg.Body = tResultSet.Rows[0]["EmailBody"].ToString(); 
                //oMsg.To = veMailId;
                oMsg.To = tResultSet.Rows[0]["EmailId"].ToString();
                
                //oMsg.HTMLBody = tResultSet.Rows[0]["EmailBody"].ToString() + oMsg.HTMLBody;
                Microsoft.Office.Interop.Outlook.Inspector insp ;
                insp = oMsg.GetInspector;
                
    
                string sDisplayName="", sBodyLen=oMsg .Body .Length.ToString () ;
                Microsoft.Office.Interop.Outlook.Attachments oAttachs;
                oAttachs = oMsg.Attachments;  // = oMsg.Attachments;
                Microsoft.Office.Interop.Outlook.Attachment oAttach ;
                oAttachs = (Microsoft .Office .Interop .Outlook .Attachments )oMsg.Attachments;
                //oAttach = oAttachs.Add(sSource, , sBodyLen + 1, sDisplayName);
                oAttach = (Microsoft.Office.Interop.Outlook.Attachment )oAttachs.Add(vPdfFileNm, null, sBodyLen + 1, sDisplayName);
                
                oMsg.Send();
                //MessageBox.Show("Mail Sent");
                //oAttach =(Microsoft.Office.Interop.Outlook.MailItem) oAttachs.Add(vPdfFileNm, null, sBodyLen + 1, sDisplayName);
                //oMsg.Send();
            }
            catch (System.Exception  ex)
            {
                MessageBox.Show(ex.Message);
            }



        }
        private void mthSendEmail(string vPdfFileNm, DataTable tResultSet)
        {

            string veMailId = "rupeshprajapati@udyogsoftware.com";
            veMailId = "vasant@udyogsoftware.com";
            //MessageBox.Show("Sending Mail");
            try
            {

                Microsoft.Office.Interop.Outlook.Application oOutLookApp = new Microsoft.Office.Interop.Outlook.Application();
                Microsoft.Office.Interop.Outlook.MailItem oMsg;   //                oOutLookApp .CreateItem ( Microsoft.Office.Interop.Outlook.MailItem);

                oMsg = (Microsoft.Office.Interop.Outlook.MailItem)oOutLookApp.CreateItem(OlItemType.olMailItem);   // Convert.tooOutLookApp.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
                oMsg.Subject = tResultSet.Rows[0]["EMailSub"].ToString();

                //oMsg.Body = tResultSet.Rows[0]["EmailBody"].ToString() +Convert.ToChar((13))+ ReadSignature();
                oMsg.Body = tResultSet.Rows[0]["EmailBody"].ToString() ;
                //oMsg.To = veMailId;
                oMsg.To = tResultSet.Rows[0]["EmailId"].ToString();
                
                //oMsg.HTMLBody = tResultSet.Rows[0]["EmailBody"].ToString() + oMsg.HTMLBody;
                Microsoft.Office.Interop.Outlook.Inspector insp;
                insp = oMsg.GetInspector;
                

                string sDisplayName = "", sBodyLen = oMsg.Body.Length.ToString();
                Microsoft.Office.Interop.Outlook.Attachments oAttachs;
                oAttachs = oMsg.Attachments;  // = oMsg.Attachments;
                Microsoft.Office.Interop.Outlook.Attachment oAttach;
                oAttachs = (Microsoft.Office.Interop.Outlook.Attachments)oMsg.Attachments;
                //oAttach = oAttachs.Add(sSource, , sBodyLen + 1, sDisplayName);
                oAttach = (Microsoft.Office.Interop.Outlook.Attachment)oAttachs.Add(vPdfFileNm, null, sBodyLen + 1, sDisplayName);

                oMsg.Send();
              //  MessageBox.Show("Mail Sent");
                //oAttach =(Microsoft.Office.Interop.Outlook.MailItem) oAttachs.Add(vPdfFileNm, null, sBodyLen + 1, sDisplayName);
                //oMsg.Send();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }

        public DataTable pResulSet
        {
         
            get
            {
                return vResulSet;
            }
            set
            {
                vResulSet = value;
            }
        }
        public string pPageBreakFieldList
        {
            get { return vPageBreakFieldList; }
            set { vPageBreakFieldList = value; }
        }
        public string pReportPath
        {

            get
            {
                return vReportPath;
            }
            set
            {
                vReportPath = value;
            }
        }
        public DataSet pDsCommon
        {
            get
            {
                return vdsCommon;
            }
            set
            {
                vdsCommon = value;
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
        public string[] pPara
        {
            get { return vPara; }
            set { vPara = value; }
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
        public Boolean pWaitPrint
        {
            get { return vWaitPrint; }
            set { vWaitPrint = value; }
        }
        public string pFrmCaption
        {
            get { return vFrmCaption; }
            set { vFrmCaption = value; }
        }
        public string pRepHead
        {
            get { return vRepHead; }
            set { vRepHead = value; }
        }
        //Added By Kishor A. for Bug-28186 Start
        public string pUserName
        {
            get { return vUserName; }
            set { vUserName = value; }
        }
        //Added By Kishor A. for Bug-28186 End

    }

}
