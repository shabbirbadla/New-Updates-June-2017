using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using cTipsSqlConn;
using System.Data.SqlClient;
using System.Data.OleDb;

using System.Collections;

using System.Xml;
using ueErrorLoger;

using System.IO;

using System.Net;
using System.Diagnostics;
using System.Threading;

namespace ueST3DataTool
{
    public partial class frm_ST3DataTool : Form
    {
        public String SqlStr;
        public string  pCompId;

        string pCompDb;
        string pServerName;
        string pUserName;
        string pPassword;
        string pPath;
        string pIconPath;
        string LogFileName="";
        string ErrorLogName = "";
        string xmlFileName="";
        string crcFileName = "";
        string FileTag="ST3";
        string crcValue;
        bool xmlfilegenerated;

        DataSet dsComman = new DataSet();
        cTipsSqlConn.SlqDatacon oSqlConn = new cTipsSqlConn.SlqDatacon();
        
        DataView dvwST3 = new DataView();
        DataView dvwCompany = new DataView();
        DataView dvwCoAdditional = new DataView();
        
        bool DataValidation;
        string[] strArgs1;

        TextWriter sw;
        String sdate, edate;

        XmlNode xnCommon1;
        XmlNode xnCommon1n;
        XmlNode xnCommon1_1 ;
        XmlNode xnCommon1_2 ;
        XmlNode xnCommon1_3;
        XmlNode xnCommon1_4;
        XmlNode xnCommon1_5;
        XmlNode xnCommon1_6;
        XmlAttribute xaCommon;
        XmlText xmltext;

        public frm_ST3DataTool(string[] strArgs)
        {
            xmlfilegenerated = true;
            this.crcValue = "";
            strArgs1 = strArgs;

            pCompDb = strArgs[0];
            pServerName = strArgs[1];
            pUserName = strArgs[2];
            pPassword = strArgs[3];
            pPath = strArgs[4];
            pPath = pPath.Replace("<*#*>", " ");
            pCompId =strArgs[5];
            pIconPath = strArgs[6];
            this.Text = "ST-3 XML Generation";
            InitializeComponent();

            oSqlConn.pdbname = pCompDb;
            oSqlConn.pSrvname = pServerName;
            oSqlConn.pSrvusername = pUserName;
            oSqlConn.pSrvuserpassword = pPassword;
            this.DataValidation = true;
            ErrorLogName =Path.GetDirectoryName(Application.ExecutablePath);
            
            ErrorLogName = Path.Combine(ErrorLogName, this.FileTag + "_error.txt");
            
            this.mGenerateDataSet();
            this.mControlSet();
            this.lbl.Text = "";
        }
        private void mFwrite(string vline)
        {
            try
            {
                
                sw.WriteLine("");
                sw.WriteLine(vline);
                sw.Flush();
            }
            catch (Exception ex)
            {
                this.mOnError(ex);
            }
        }
        private void mCheck_Validtions()
        {
            try
            {
                this.lbl.Text = "Cheking Validations";

                string vFldVal;
                this.DataValidation = true;
                //STC No. Validation--->
                vFldVal = dvwCoAdditional.Table.Rows[0]["sregn"].ToString().Trim();
                if (vFldVal.Length != 15)
                {
                    this.mFwrite("Invalid STC No. lenght should be 15");
                    this.DataValidation = false;
                }
                else
                {
                    if ((Char.IsNumber(vFldVal, 0)) || (Char.IsNumber(vFldVal, 1)) || (Char.IsNumber(vFldVal, 2)) || (Char.IsNumber(vFldVal, 3)) || (Char.IsNumber(vFldVal, 4))
                        || (!Char.IsNumber(vFldVal, 5)) || (!Char.IsNumber(vFldVal, 6)) || (!Char.IsNumber(vFldVal, 7)) || (!Char.IsNumber(vFldVal, 8)) || (Char.IsNumber(vFldVal, 9))
                        || !(vFldVal.Substring(10, 2) == "ST" || vFldVal.Substring(10, 2) == "SD") || (!Char.IsNumber(vFldVal, 12)) || (!Char.IsNumber(vFldVal, 13)) || (!Char.IsNumber(vFldVal, 14))
                        )
                    {
                        this.mFwrite("Invalid STC No. foarmat");
                        this.DataValidation = false;
                    }
                }
                //<---STC No. Validation
                //--->ASSeSSee'S Name
                vFldVal = dvwCompany.Table.Rows[0]["co_name"].ToString().Trim();
                if (vFldVal.Length > 60)
                {
                    this.mFwrite("ASSeSSee'S Name not allowed with more than 60 character");
                    this.DataValidation = false;
                }
                //<---ASSeSSee'S Name
                //--->LTU CITY
                if (dvwCoAdditional.Table.Rows[0]["LTU"].ToString() == "True" && dvwCoAdditional.Table.Rows[0]["LTUCITY"].ToString().Trim() == "")
                {
                    this.mFwrite("LTU City Can not be Blank");
                    this.DataValidation = false;
                }
                //<---LTU CITY
                //Premises Code No.--->
                vFldVal = dvwCoAdditional.Table.Rows[0]["premisecd"].ToString().Trim();

                if (vFldVal.Length != 10)
                {
                    this.mFwrite("Premises Code No. lenght should be 10");
                    this.DataValidation = false;
                }
                //<---Premises Code No.
                //Constitution--->
                vFldVal = dvwCoAdditional.Table.Rows[0]["typeorg"].ToString().Trim();
                if (vFldVal != "Proprietorship" && vFldVal != "Partnership" && vFldVal != "Registered Public Limited Company" && vFldVal != "Registered Private Limited Company" && vFldVal != "Registered Trust" && vFldVal != "Society/Cooperative Society" && vFldVal != "Others")
                {
                    this.mFwrite("Invalid ASSeSSee Constitution");
                    this.DataValidation = false;
                }
                //<---Constitution
                //STRP No--->
                vFldVal = dvwCoAdditional.Table.Rows[0]["StrpId"].ToString().Trim();
                if (vFldVal.Length > 40)
                {
                    this.mFwrite("STRP No. lenght should be less than or equal to 40");
                    this.DataValidation = false;
                }
                //<---STRP No
                //STRP Name--->
                vFldVal = dvwCoAdditional.Table.Rows[0]["StrpNM"].ToString().Trim();
                if (vFldVal.Length > 40)
                {
                    this.mFwrite("STRP Name lenght should be less than or equal to 40");
                    this.DataValidation = false;
                }
                //<---STRP Name
                //Name--->
                vFldVal = dvwCoAdditional.Table.Rows[0]["signature"].ToString().Trim();
                if (vFldVal.Length == 0)
                {
                    this.mFwrite("Person Name can not be blank");
                    this.DataValidation = false;
                }
                else
                {
                    if (vFldVal.Length > 40)
                    {
                        this.mFwrite("Person Name lenght should be less than or equal to 40");
                        this.DataValidation = false;
                    }
                }
                //<---Name
                //Place--->
                vFldVal = dvwCompany.Table.Rows[0]["city"].ToString().Trim();
                if (vFldVal.Length == 0)
                {
                    this.mFwrite("Place can not be blank");
                    this.DataValidation = false;
                }
                else
                {
                    if (vFldVal.Length > 40)
                    {
                        this.mFwrite("Place lenght should be less than or equal to 40");
                        this.DataValidation = false;
                    }
                }
                //<---Place

                if (this.DataValidation == false)
                {
                    this.lbl.Text = "Data validation failed...";
                }
            }
            catch (Exception ex)
            {
                this.mOnError(ex);
            }
        }
        private void mGenerateDataSet()
        {
            
            string selectstring;
            long vrcount;
            this.lbl.Text = "Generating Company Addtional Information...";
            selectstring = "Select * from manufact";
            try
            {
               oSqlConn.Selectcommand(null, true, null, selectstring, ref dvwCoAdditional, out vrcount);
            }
            catch (SqlException ex)
            {
                this.mOnError(ex);
                Application.Exit();
            }
            this.lbl.Text = "Generating Company Details...";
            
            selectstring = "Select * from vudyog..co_mast where compid="+this.pCompId;
            try
            {
                oSqlConn.Selectcommand(null, true, null, selectstring, ref dvwCompany, out vrcount);
            }
            catch (SqlException ex)
            {
                this.mOnError(ex);
                Application.Exit();
            }
            this.lbl.Text = "";
        }

        private void mOnError(Exception ex)
        {
            this.lbl.Text = "Generating Error Details";
            
            bool bReturnLog = false;
            ueErrorLoger.ErrorLog.LogFilePath = ErrorLogName;
            bReturnLog = ErrorLog.ErrorRoutine(false, ex);
            if (false == bReturnLog)
            {
                MessageBox.Show("Unable to write a log");
            }
            MessageBox.Show(ex.Message);
            this.lbl.Text = "";
            Application.Exit();
            return;
        }
        
        private void mSetRecordSet()
        {
            this.lbl.Text = "Generating ST-3 Details from Database...";
            string selectstring = "Execute USP_REP_ST3_MULTISERVICE '','','','" + sdate + "','" + edate + "','','','','',0,0,'','','','','','','','','" + this.cmbFYear.Text + "',''";
            long vrcount;
            try
            {
                oSqlConn.Selectcommand(null, true, null, selectstring, ref dvwST3, out vrcount);
            }
            catch (SqlException ex)
            {
                this.mOnError(ex);
                Application.Exit();
                return;
            }
            this.lbl.Text = "";
        }
      
        private void btnFPath_Click(object sender, EventArgs e)
        {
            this.lbl.Text = "Select Folder...";
            folderBrowserDialog1.Description = "Select Folder";
            folderBrowserDialog1.ShowDialog();
            string fname;
            fname = this.FileTag;

            fname = fname + "_" + dvwCoAdditional.Table.Rows[0]["sregn"].ToString().Trim();
            fname = fname + "_" + this.cmbPeriod.Text.Trim().Substring(0,3);
            fname = fname + this.cmbFYear.Text.Trim().Substring(0,4);
            fname = fname + "_" + this.cmbPeriod.Text.Trim().Substring(this.cmbPeriod.Text.IndexOf("-")+1, 3);
            fname = fname + this.cmbFYear.Text.Trim().Substring(this.cmbFYear.Text.IndexOf("-")+1, 4);
            fname = fname +  ".zip";

            if (folderBrowserDialog1.SelectedPath != string.Empty)
            {
                
                this.txtFPath.Text = Path.Combine(folderBrowserDialog1.SelectedPath ,fname);
                this.crcFileName = Path.Combine(folderBrowserDialog1.SelectedPath, "eFileInfo" + ".xml");
                this.xmlFileName = Path.Combine(folderBrowserDialog1.SelectedPath, this.FileTag + ".xml");
            }
            this.lbl.Text = "";
        }

        private void btnFGen_Click(object sender, EventArgs e)
        {

            this.pgb.Visible = true;

            this.lbl.Text = "XML Generation Starts...";
            if (this.txtFPath.Text.Trim() != "")
            {
                //this.xmlFileName = this.txtFPath.Text.Trim().ToUpper().Replace(".ZIP", ".xml");
                this.LogFileName = this.xmlFileName.Replace(".xml", "Log.txt");
            }
            
            try
            {
                if (File.Exists(this.LogFileName) == true)
                {
                    File.Delete(this.LogFileName);
                }
            }
            catch (FileNotFoundException ex)
            {
                this.mOnError(ex);
            }

            sw = new StreamWriter(this.LogFileName);
            sw.Flush();

            this.mFwrite("Date        : " + DateTime.Now.ToLongTimeString());
            this.mFwrite("Time        : " + DateTime.Now.ToShortDateString());
            this.mFwrite("Checking Validations....");
            this.mRefreshPGB(1);

            this.sdate = "04/01/" + this.cmbFYear.Text.Substring(0, 4);
            this.edate = "09/30/" + this.cmbFYear.Text.Substring(0, 4);
            if (this.cmbPeriod.Text == "October-March")
            {
                this.sdate = "10/01/" + this.cmbFYear.Text.Substring(5, 4);
                this.edate = "03/31/" + this.cmbFYear.Text.Substring(5, 4);
            }

            this.mCheck_Validtions();

            this.mRefreshPGB(10);
            this.Visible = true;
            bool xmlfilegenerated = true;

             pgb.Value = 30;
             if (DataValidation == true)
             {
                 this.mFwrite("Data is validated...");
                 if (this.txtFPath.Text != "")
                 {
                     this.mFwrite("Generating RecordSet...");
                     this.mSetRecordSet();
                     this.mFwrite("Record Set generated...");
                     this.mFwrite("XML Generation Starts--->");
                     this.mWriteRawXML();
                     this.mFwrite("<----XML Generation completed");
                     if (xmlfilegenerated == true)
                     {
                         this.mGenerateCRC();
                     }
                 }
                 else
                 {
                     MessageBox.Show("Please Select Target File Path");
                     xmlfilegenerated = false;
                 }
             }
             else
             {
                 xmlfilegenerated = false;
                 this.mRefreshPGB(0);
             }
            sw.Close();

            if (xmlfilegenerated == true)
            {
                try
                {
                    //string vpass;
                    //string vversion;
                    //vversion = "1.0.0.0";

                    //Encoding ascii = Encoding.ASCII;
                    //Encoding unicode = Encoding.Unicode;
                    //// Convert the string into a byte[].
                    //byte[] unicodeBytes = unicode.GetBytes(decryptedVersioNo.Substring(4, 1).ToString());
                    //// Perform the conversion from one encoding to the other.
                    //byte[] asciiBytes = Encoding.Convert(unicode, ascii, unicodeBytes);
                    //string asciiString = asciiBytes[0].ToString();

                    //string vascci = asciiString;
                    //vpass = this.crcValue.Substring(0, 4) + vascci+this.crcValue.Substring(5,this.crcValue.Length-1-5);

                    if (File.Exists(this.txtFPath.Text) == true)
                    {
                        File.Delete(this.txtFPath.Text);
                    }

                    string vpass;
                    string vversion;
                    vversion = "1.0.0.0";
                    
                    if ((File.Exists(this.LogFileName) == true) && (this.xmlfilegenerated = true))
                    {
                        File.Delete(this.LogFileName);
                    }

                    string vascci = this.mAscii(vversion);
                    vpass = this.crcValue.Substring(0, 4) + vascci.PadLeft(4, Convert.ToChar("0")) + this.crcValue.Substring(4, (this.crcValue.Length - 4));
                    vpass = "testing";

                    UdyogZipUnzip.UdyoyZipUnZipUtility UZIP = new UdyogZipUnzip.UdyoyZipUnZipUtility();
                    //UZIP.UdyogZip(@"D:\Temp", this.txtFPath.Text, "test");
                    UZIP.UdyogZip(Path.GetDirectoryName(this.txtFPath.Text), this.txtFPath.Text, vpass);

                    System.Threading.Thread.Sleep(4000);

                   
                    if ((File.Exists(this.xmlFileName) == true) && (this.xmlfilegenerated = true))
                    {
                        File.Delete(this.xmlFileName);
                    }
                    if ((File.Exists(this.crcFileName) == true) && (this.xmlfilegenerated = true))
                    {
                        File.Delete(this.crcFileName);
                    }
                    this.lbl.Text = this.txtFPath.Text.Trim() + " file Successfuly generated";
                    this.mRefreshPGB(0);
                    
                }
                
                catch (FileNotFoundException ex)
                {
                    this.mOnError(ex);
                }
            }
        }
        private void mControlSet()
        {
            this.lbl.Text = "";
            if (this.pIconPath != "")
            {
                Icon MainIcon = new System.Drawing.Icon(this.pIconPath);
                this.Icon = MainIcon;
            }
            if (this.pPath != "")
            {
                this.btnFPath.Image = Image.FromFile(this.pPath + @"\bmp\pickup.gif");
            }
            int syear = ((DateTime) dvwCompany.Table.Rows[0]["sta_dt"]).Year;
            int eyear = ((DateTime)dvwCompany.Table.Rows[0]["end_dt"]).Year;
            this.cmbFYear.Text= (syear.ToString().Trim() + "-" + eyear.ToString().Trim());

            cmbPeriod.Items.Add("April-September");
            cmbPeriod.Items.Add("October-March");
            if (DateTime.Now.Month >= 5 && DateTime.Now.Month <= 10)
            {
                this.cmbPeriod.Text = "April-September";

            }
            else
            {
                this.cmbPeriod.Text = "October-March";
            }

            //this.sdate = "04/01/" + this.cmbFYear.Text.Substring(0, 4);
            //this.edate = "09/30/" + this.cmbFYear.Text.Substring(0, 4);
            //if (this.cmbPeriod.Text == "October-March")
            //{
            //    this.sdate = "10/01/" + this.cmbFYear.Text.Substring(5, 4);
            //    this.edate = "03/31/" + this.cmbFYear.Text.Substring(5, 4);
            //}
            //this.LogFileName = dvwCompany.Table.Rows[0]["dir_nm"].ToString().Trim()+ this.FileTag+"Log" + ".txt";

        }
        
        private void mWriteRawXML()
        {
            this.lbl.Text = "XML Generation Starts...";
            try
            {
                XmlDocument doc = new XmlDocument();
                xnCommon1 = doc.CreateElement("aa");
                xnCommon1_1 = doc.CreateElement("aa");
                xnCommon1_2 = doc.CreateElement("aa");
                xnCommon1_3 = doc.CreateElement("aa");
                xnCommon1_4 = doc.CreateElement("aa");
                xnCommon1_5 = doc.CreateElement("aa");
                xnCommon1_6 = doc.CreateElement("aa");

                doc.CreateXmlDeclaration("1.0", "utf-8", "");

                XmlNode xnACES = doc.CreateElement("UDACESST3");
                doc.AppendChild(xnACES);

                XmlNode xnRETURN = doc.CreateElement("UDRETURNST3");
                XmlAttribute xaReturnType = doc.CreateAttribute("UDReturnTypeST3");
                xaReturnType.Value = "ST3";
                xnRETURN.Attributes.Append(xaReturnType);
                xnACES.AppendChild(xnRETURN);
                XmlAttribute xaReturnType1 = doc.CreateAttribute("ACESToolVerACES");
                xaReturnType1.Value = "5.1";
                xnRETURN.Attributes.Append(xaReturnType1);

                xaReturnType1 = doc.CreateAttribute("UDToolVerST3");
                xaReturnType1.Value = "1.0";
                xnRETURN.Attributes.Append(xaReturnType1);
                this.lbl.Text = "Generating Header Details...";
                //--->Header Data
                XmlNode xnHEADERDATA = doc.CreateElement("UDHEADER-DATAST3");
                this.mWriteRawXML_Header(ref doc, ref xnHEADERDATA);
                xnRETURN.AppendChild(xnHEADERDATA);
                this.mRefreshPGB(20);
                //<---Header Data

                //--->TAXABLE-SERVICES 3F
                this.lbl.Text = "Generating Taxable Service Details...";
                XmlNode xnTAXABLESERVICES = doc.CreateElement("UDTAXABLE-SERVICESST3");
                this.mWriteRawXML_TaxableService(ref doc, ref xnTAXABLESERVICES);
                xnRETURN.AppendChild(xnTAXABLESERVICES);
                this.mRefreshPGB(40);
                //<---TAXABLE-SERVICES

                //--->ADVANCE-PAYMENT 4 (1 & 2)  
                this.lbl.Text = "Generating Advance Payment Details...";
                XmlNode xnADVANCEPAYMENT = doc.CreateElement("UDADVANCE-PAYMENTST3");
                this.mWriteRawXML_AdvancePayment(ref doc, ref xnADVANCEPAYMENT);
                xnRETURN.AppendChild(xnADVANCEPAYMENT);
                this.mRefreshPGB(45);
                //<---ADVANCE-PAYMENT 4 (1 & 2)

                //--->PAID-SERVICE 4A1
                this.lbl.Text = "Generating Paid Service Details...";
                XmlNode xnPaidService = doc.CreateElement("UDPAID-SERVICEST3");
                this.mWriteRawXML_PaidService(ref doc, ref xnPaidService);
                xnRETURN.AppendChild(xnPaidService);
                this.mRefreshPGB(50);
                this.Refresh();
                //<---PAID-SERVICE 4A1

                //--->Challan_Numbers 4A2
                this.lbl.Text = "Generating Challan Details...";
                XmlNode xnChallanNumbers = doc.CreateElement("UDCHALLAN-NUMBERSST3");
                this.mWriteRawXML_ChallanNumbers(ref doc, ref xnChallanNumbers);
                xnRETURN.AppendChild(xnChallanNumbers);
                this.mRefreshPGB(55);
                //<---PAID-SERVICE 4A2

                //--->Source_Document_Details 4B
                this.lbl.Text = "Generating Source Document Details";
                XmlNode xnSourceDocumentDetails = doc.CreateElement("UDSOURCE-DOCUMENT-DETAILSST3");
                this.mWriteRawXML_SourceDocumentDetails(ref doc, ref xnSourceDocumentDetails);
                xnRETURN.AppendChild(xnSourceDocumentDetails);
                this.mRefreshPGB(60);
                //<---Source_Document_Details 4B

                //--->Source_Document_Details 4C
                this.lbl.Text = "Generating Service Tax Payable But Not Paid Details....";
                XmlNode xnStPayableNotPaid = doc.CreateElement("UDST-PAYABLE-NOT-PAIDST3");
                this.mWriteRawXML_StPayableNotPaid(ref doc, ref xnStPayableNotPaid);
                xnRETURN.AppendChild(xnStPayableNotPaid);
                this.mRefreshPGB(65);
                //<---Source_Document_Details 4C

                //--->Source_Document_Details 5A & 5AA
                this.lbl.Text = "Generating EXEMPTED/NON TAXABLE SERVICE OR EXEMPTED GOODS Details...";
                XmlNode xnCenvat = doc.CreateElement("UDCENVATST3");
                this.mWriteRawXML_5A(ref doc, ref xnCenvat);
                xnRETURN.AppendChild(xnCenvat);
                this.mRefreshPGB(75);
                //<---Source_Document_Details 5A & 5AA

                //--->CenvatChallanNumbers 5AA-F
                this.lbl.Text = "Generating Cenvat Challan Number Details...";
                XmlNode xnCENVATCHALLANNUMBERS = doc.CreateElement("UDCENVAT-CHALLAN-NUMBERSST3");
                this.mWriteRawXML_CenvatChallanNumbers(ref doc, ref xnCENVATCHALLANNUMBERS);
                xnCenvat.AppendChild(xnCENVATCHALLANNUMBERS);
                this.mRefreshPGB(80);
                //<---CenvatChallanNumbers 5AA-F
               
                //--->CenvatCrStCe 5B
                this.lbl.Text = "Generating Cenvat Credit Taken and Utilized Details...";
                this.mWriteRawXML_5B(ref doc, ref xnCenvat);
                this.mRefreshPGB(90);
                //<---CenvatCrStCe
                
                //--->Distributor 6
                this.lbl.Text = "Generating ISD Details....";
                XmlNode xnDistributor = doc.CreateElement("UDDISTRIBUTORST3");
                this.mWriteRawXML_Distributor(ref doc, ref xnDistributor);
                xnRETURN.AppendChild(xnDistributor);
                this.mRefreshPGB(99);
                //<---Distributor 6
                
                //--->SelfAssessmentMemorendum
                this.lbl.Text = "Self Assessment Memorendum Details...";
                XmlNode xnSELFASSESSMENTMEMORANDUM = doc.CreateElement("UDSELF-ASSESSMENT-MEMORANDUMST3");
                this.mWriteRawXML_SelfAssessmentMemorendum(ref doc, ref xnSELFASSESSMENTMEMORANDUM);
                xnRETURN.AppendChild(xnSELFASSESSMENTMEMORANDUM);
                this.mRefreshPGB(100);
                //<---SelfAssessmentMemorendum


                xnACES.AppendChild(xnRETURN);

                try
                {
                    if (this.xmlfilegenerated ==true )
                    {
                        this.lbl.Text = "Saving XML file...";
                        //doc.CreateXmlDeclaration("1.0", "utf-8", "");

                        //XmlTextWriter WRITER = new XmlTextWriter(this.xmlFileName, Encoding.UTF8);
                        //WRITER.WriteStartDocument();
                        //WRITER.Close();
                        //doc.Save(this.xmlFileName);
                        //doc.Load(this.xmlFileName);
                        //doc.CreateXmlDeclaration("1.0", "utf-8", "");
                        doc.Save(this.xmlFileName);

                        this.lbl.Text = this.xmlFileName + " file Successfuly generated";
                        this.mFwrite(this.xmlFileName + " file Successfuly generated");
                        //MessageBox.Show(this.xmlFileName  +" file Successfuly generated");
                    }
                    else
                    {
                        this.lbl.Text = this.xmlFileName + " file not generated";
                        this.mFwrite(this.xmlFileName + " file not generated");
                    }
                    
                }
                catch (Exception e)
                {
                    this.xmlfilegenerated = false;
                    this.mOnError(e);
                    Application.Exit();
                }

            }
            catch (Exception ex)
            {
                this.xmlfilegenerated = false;
                this.mOnError(ex);
                Application.Exit();
            }
          


        }
        private void mWriteRawXML_SelfAssessmentMemorendum(ref XmlDocument vdoc, ref XmlNode pNode)
        {
            try
            {
                xnCommon1 = vdoc.CreateElement("DECLARATION");
                pNode.AppendChild(xnCommon1);

                xnCommon1 = vdoc.CreateElement("NAME");
                xmltext = vdoc.CreateTextNode(this.dvwCoAdditional.Table.Rows[0]["signature"].ToString().Trim());
                xnCommon1.AppendChild(xmltext);
                pNode.AppendChild(xnCommon1);

                xnCommon1 = vdoc.CreateElement("PLACE");
                xmltext = vdoc.CreateTextNode((string)this.dvwCompany.Table.Rows[0]["city"]);
                xnCommon1.AppendChild(xmltext);
                pNode.AppendChild(xnCommon1);

                xnCommon1 = vdoc.CreateElement("DATE");
                xmltext = vdoc.CreateTextNode(DateTime.Now.Date.ToString());
                xnCommon1.AppendChild(xmltext);
                pNode.AppendChild(xnCommon1);

                xnCommon1 = vdoc.CreateElement("STRP-IDNO");
                xmltext = vdoc.CreateTextNode((string)this.dvwCoAdditional.Table.Rows[0]["StrpId"]);
                xnCommon1.AppendChild(xmltext);
                pNode.AppendChild(xnCommon1);

                xnCommon1 = vdoc.CreateElement("STRP-NAME");
                xmltext = vdoc.CreateTextNode((string)this.dvwCoAdditional.Table.Rows[0]["StrpNm"]);
                xnCommon1.AppendChild(xmltext);
                pNode.AppendChild(xnCommon1);
                this.mFwrite("Self Assessment Memorendum Details generated");
            }
            catch (Exception ex)
            {
                this.mFwrite("Self Assessment Memorendum Details not generated");
                this.xmlfilegenerated = false;
                this.mOnError(ex);
                Application.Exit();
            }
        }
        private void mWriteRawXML_Distributor(ref XmlDocument vdoc, ref XmlNode pNode)
        {
            try
            {
                XmlNode xn61 = vdoc.CreateElement("ST-CE-CENVAT-CRS");
                XmlNode xn62 = vdoc.CreateElement("ED-CESS-HIGH-ED-CESS-CENVAT-CRS");

                for (int i = 0; i <= dvwST3.Table.Rows.Count - 1; i++)
                {
                    String vSrNo = dvwST3.Table.Rows[i]["srno"].ToString().Trim();
                    switch (vSrNo)
                    {
                        //Service Tax-->

                        case "601A0":
                            pNode.AppendChild(xn61);
                            this.mGenerateNode(ref xn61, "OP-BAL-CENVAT-CRS", "OP-BAL-CENVAT-CR", ref vdoc, i);
                            break;
                        case "601B0":
                            this.mGenerateNode(ref xn61, "INPUT-SERVICE-CRS", "INPUT-SERVICE-CR", ref vdoc, i);
                            break;
                        case "601C0":
                            this.mGenerateNode(ref xn61, "DITRIBUTED-CREDITS", "DITRIBUTED-CREDIT", ref vdoc, i);
                            break;
                        case "601D0":
                            this.mGenerateNode(ref xn61, "CREDIT-NOT-ELIGIBLE-DISTRIBUTIONS", "CREDIT-NOT-ELIGIBLE-DISTRIBUTION", ref vdoc, i);
                            break;
                        case "601E0":
                            this.mGenerateNode(ref xn61, "CLOSING-BALANCES", "CLOSING-BALANCES", ref vdoc, i);
                            break;
                        //<---Service
                        //--->Edu. Cess
                        case "602A0":
                            pNode.AppendChild(xn62);
                            this.mGenerateNode(ref xn62, "OP-BAL-ED-CESS-CRS", "OP-BAL-ED-CESS-CR", ref vdoc, i);
                            break;
                        case "602B0":
                            this.mGenerateNode(ref xn62, "INPUT-SERVICE-ED-CESS-CRS", "INPUT-SERVICE-ED-CESS-CR", ref vdoc, i);
                            break;
                        case "602C0":
                            this.mGenerateNode(ref xn62, "ED-CESS-DSTRBTD-CRS", "ED-CESS-DSTRBTD-CR", ref vdoc, i);
                            break;
                        case "602D0":
                            this.mGenerateNode(ref xn62, "ED-CESS-NO-DSTRBTD-CRS", "ED-CESS-NO-DSTRBTD-CR", ref vdoc, i);
                            break;
                        case "602E0":
                            this.mGenerateNode(ref xn62, "CLOSING-BALANCES", "CLOSING-BALANCE", ref vdoc, i);
                            break;
                        //<---Edu. Cess
                        default:
                            break;
                    }
                }
                this.mFwrite("ISD Details generated");
            }
            catch (Exception ex)
            {
                this.mFwrite("ISD Details not generated");
                this.xmlfilegenerated = false;
                this.mOnError(ex);
                Application.Exit();
            }
        }
        private void mWriteRawXML_5B(ref XmlDocument vdoc, ref XmlNode pNode)
        {
            try
            {
                XmlNode xn5B1 = vdoc.CreateElement("CENVAT-CR-ST-CE");
                //pNode.AppendChild(xn5B1);
                XmlNode xn5B2 = vdoc.CreateElement("CENVAT-CR-ED-HIGHED-CESS");
                for (int i = 0; i <= dvwST3.Table.Rows.Count - 1; i++)
                {
                    String vSrNo = dvwST3.Table.Rows[i]["srno"].ToString().Trim();

                    switch (vSrNo)
                    {
                        //Service Tax-->
                        case "5B1A0":
                            pNode.AppendChild(xn5B1);
                            this.mGenerateNode(ref xn5B1, "OPENING-BALANCES", "OPENING-BALANCE", ref vdoc, i);
                            break;
                        case "5B1B0":
                            xnCommon1 = vdoc.CreateElement("CREDIT-TAKEN-ON");
                            xn5B1.AppendChild(xnCommon1);
                            break;
                        case "5B1B1":
                            this.mGenerateNode(ref xnCommon1, "INPUTS", "INPUT", ref vdoc, i);
                            break;
                        case "5B1B2":
                            this.mGenerateNode(ref xnCommon1, "CAPITAL-GOODS", "CAPITAL-GOOD", ref vdoc, i);
                            break;
                        case "5B1B3":
                            this.mGenerateNode(ref xnCommon1, "DIRECT-INPUT-SERVICES-RECDS", "DIRECT-INPUT-SERVICES-RECD", ref vdoc, i);
                            break;
                        case "5B1B4":
                            this.mGenerateNode(ref xnCommon1, "INPUT-SERVICE-DISTRIBUTOR-RECDS", "INPUT-SERVICE-DISTRIBUTOR-RECD", ref vdoc, i);
                            break;
                        case "5B1B5":
                            this.mGenerateNode(ref xnCommon1, "LTU-INTER-UNIT-TRANSFERS", "LTU-INTER-UNIT-TRANSFER", ref vdoc, i);
                            break;
                        case "5B1B6":
                            this.mGenerateNode(ref xnCommon1, "TOTALS", "TOTAL", ref vdoc, i);
                            break;

                        case "5B1C0":
                            xnCommon1 = vdoc.CreateElement("CREDIT-UTILIZED");
                            xn5B1.AppendChild(xnCommon1);
                            break;
                        case "5B1C1":
                            this.mGenerateNode(ref xnCommon1, "ST-PYMNTS", "ST-PYMNT", ref vdoc, i);
                            break;
                        case "5B1C2":
                            this.mGenerateNode(ref xnCommon1, "ED-CESS-PYMNTS", "ED-CESS-PYMNT", ref vdoc, i);
                            break;
                        case "5B1C3":
                            this.mGenerateNode(ref xnCommon1, "EXCISE-PYMNTS", "EXCISE-PYMNT", ref vdoc, i);
                            break;
                        case "5B1C4":
                            this.mGenerateNode(ref xnCommon1, "IP-CAPITAL-GOODS-REM-CLEARANCES", "IP-CAPITAL-GOODS-REM-CLEARANCE", ref vdoc, i);
                            break;
                        case "5B1C5":
                            this.mGenerateNode(ref xnCommon1, "INTER-UNIT-TRANSFER-LTUS", "INTER-UNIT-TRANSFER-LTU", ref vdoc, i);
                            break;
                        case "5B1C6":
                            this.mGenerateNode(ref xnCommon1, "FIVE-B-PAYMENT-RULE-SIXS", "FIVE-B-PAYMENT-RULE-SIX", ref vdoc, i);
                            break;
                        case "5B1C7":
                            this.mGenerateNode(ref xnCommon1, "TOTAL-UT-CRS", "TOTAL-UT-CR", ref vdoc, i);
                            break;

                        case "5B1D0":
                            this.mGenerateNode(ref xn5B1, "CENVAT-CLO-BALS", "CENVAT-CLO-BAL", ref vdoc, i);
                            break;
                        //<---Service Tax
                        //--->Edu. Cess.
                        case "5B2A0":
                            pNode.AppendChild(xn5B2);
                            this.mGenerateNode(ref xn5B2, "OPENING-BALANCES", "OPENING-BALANCE", ref vdoc, i);
                            break;
                        case "5B2B0":
                            xnCommon1 = vdoc.CreateElement("CREDIT-TAKEN-ON");
                            xn5B2.AppendChild(xnCommon1);
                            break;
                        case "5B2B1":
                            this.mGenerateNode(ref xnCommon1, "INPUTS", "INPUT", ref vdoc, i);
                            break;
                        case "5B2B2":
                            this.mGenerateNode(ref xnCommon1, "CAPITAL-GOODS", "CAPITAL-GOOD", ref vdoc, i);
                            break;
                        case "5B2B3":
                            this.mGenerateNode(ref xnCommon1, "DIRECT-INPUT-SERVICES-RECDS", "DIRECT-INPUT-SERVICES-RECD", ref vdoc, i);
                            break;
                        case "5B2B4":
                            this.mGenerateNode(ref xnCommon1, "INPUT-SERVICE-DISTRIBUTOR-RECDS", "INPUT-SERVICE-DISTRIBUTOR-RECDS", ref vdoc, i);
                            break;
                        case "5B2B5":
                            this.mGenerateNode(ref xnCommon1, "LTU-INTER-UNIT-TRANSFERS", "LTU-INTER-UNIT-TRANSFERS", ref vdoc, i);
                            break;
                        case "5B2B6":
                            this.mGenerateNode(ref xnCommon1, "TOTALS", "TOTAL", ref vdoc, i);
                            break;

                        case "5B2C0":
                            xnCommon1 = vdoc.CreateElement("CREDIT-UTILIZED");
                            xn5B2.AppendChild(xnCommon1);
                            break;
                        case "5B2C1":
                            this.mGenerateNode(ref xnCommon1, "PYMNT-ED-CESS-SERVICES", "PYMNT-ED-CESS-SERVICE", ref vdoc, i);
                            break;
                        case "5B2C2":
                            this.mGenerateNode(ref xnCommon1, "PYMNT-ED-CESS-GOODS", "PYMNT-ED-CESS-GOOD", ref vdoc, i);
                            break;
                        case "5B2C3":
                            this.mGenerateNode(ref xnCommon1, "IP-CAPITAL-GOODS-REM-CLEARANCES", "IP-CAPITAL-GOODS-REM-CLEARANCE", ref vdoc, i);
                            break;
                        case "5B2C4":
                            this.mGenerateNode(ref xnCommon1, "INTER-UNIT-TRANSFER-LTUS", "INTER-UNIT-TRANSFER-LTU", ref vdoc, i);
                            break;
                        case "5B2C5":
                            this.mGenerateNode(ref xnCommon1, "TOTAL-UT-CRS", "TOTAL-UT-CR", ref vdoc, i);
                            break;

                        case "5B2D0":
                            this.mGenerateNode(ref xn5B2, "ED-HIGHED-CESS-CLO-BALS", "ED-HIGHED-CESS-CLO-BAL", ref vdoc, i);
                            break;
                        //<---Edu. Cess
                        default:
                            break;
                    }
                }
                this.mFwrite("Cenvat Credit Taken and Utilized Details generated");
            }
            catch (Exception ex)
            {
                this.mFwrite("Cenvat Credit Taken and Utilized Details not generated");
                this.xmlfilegenerated = false;
                this.mOnError(ex);
                Application.Exit();
            }
        }
        private void mWriteRawXML_CenvatChallanNumbers(ref XmlDocument vdoc, ref XmlNode pNode)
        {
            try
            {
                for (int i = 0; i <= dvwST3.Table.Rows.Count - 1; i++)
                {

                    String vSrNo = dvwST3.Table.Rows[i]["srno"].ToString().Trim();// + dvwST3.Table.Rows[i]["srno2"].ToString().Trim() + dvwST3.Table.Rows[i]["srno3"].ToString().Trim();
                    if (vSrNo != "5AZF")
                    {
                        continue;
                    }
                    //--->Service Provider
                    xnCommon1 = vdoc.CreateElement("CHALLAN-NUMBER-CENVAT");
                    pNode.AppendChild(xnCommon1);
                    xaCommon = vdoc.CreateAttribute("Month");

                    if (this.cmbPeriod.Text == "April-September")
                    {
                        if (Convert.ToString(dvwST3.Table.Rows[i]["tChalNo1"]).Trim() != "")
                        {
                            xaCommon.Value = "Apr_Jun";
                        }
                        else
                        {
                            xaCommon.Value = "Jul_Sep";

                        }
                    }
                    if (this.cmbPeriod.Text == "October-March")
                    {
                        if (Convert.ToString(dvwST3.Table.Rows[i]["tChalNo1"]).Trim() != "")
                        {

                            xaCommon.Value = "Oct_Dec";
                        }
                        else
                        {
                            xaCommon.Value = "Jan_Mar";
                        }
                    }
                    if (Convert.ToString(dvwST3.Table.Rows[i]["tChalNo1"]).Trim() + Convert.ToString(dvwST3.Table.Rows[i]["tChalNo2"]).Trim() == "")
                    {
                        xaCommon.Value = "";
                    }
                    xnCommon1.Attributes.Append(xaCommon);
                    xmltext = vdoc.CreateTextNode(Convert.ToString(dvwST3.Table.Rows[i]["tChalNo1"]).Trim() + Convert.ToString(dvwST3.Table.Rows[i]["tChalNo2"]).Trim());
                    xnCommon1.AppendChild(xmltext);
                    pNode.AppendChild(xnCommon1);

                }//for (int i = 0; i <= dvwST3.Table.Rows.Count - 1; i++)
                this.mFwrite("Cenvat Challan Number Details generated");
            }
            catch (Exception ex)
            {
                this.mFwrite("Cenvat Challan Number Details not generated");
                this.xmlfilegenerated = false;
                this.mOnError(ex);
                Application.Exit();
            }
        }
        private void mWriteRawXML_5A(ref XmlDocument vdoc, ref XmlNode pNode)
        {
            try
            {
                for (int i = 0; i <= dvwST3.Table.Rows.Count - 1; i++)
                {
                    String vSrNo = dvwST3.Table.Rows[i]["srno"].ToString().Trim();
                    switch (vSrNo)
                    {
                        case "5AA":
                            xnCommon1 = vdoc.CreateElement("PROVIDING-EXEMPTED-NON-TAXABLE-SERVICE");
                            xmltext = vdoc.CreateTextNode("No");
                            xnCommon1.AppendChild(xmltext);
                            pNode.AppendChild(xnCommon1);
                            break;
                        case "5AB":
                            xnCommon1 = vdoc.CreateElement("MANUFACTURING-EXEMPTED-GOODS");
                            xmltext = vdoc.CreateTextNode("No");
                            xnCommon1.AppendChild(xmltext);
                            pNode.AppendChild(xnCommon1);
                            break;
                        case "5AC":
                            xnCommon1 = vdoc.CreateElement("MNTNG-SEP-RECPT-CNSMPTN-ACCT");
                            xmltext = vdoc.CreateTextNode("No");
                            xnCommon1.AppendChild(xmltext);
                            pNode.AppendChild(xnCommon1);
                            break;
                        case "5AD1":
                            xnCommon1 = vdoc.CreateElement("FIVE-A-D-ONE");
                            xmltext = vdoc.CreateTextNode("No");
                            xnCommon1.AppendChild(xmltext);
                            pNode.AppendChild(xnCommon1);
                            break;
                        case "5AD2":
                            xnCommon1 = vdoc.CreateElement("FIVE-A-D-TWO");
                            xmltext = vdoc.CreateTextNode("No");
                            xnCommon1.AppendChild(xmltext);
                            pNode.AppendChild(xnCommon1);
                            break;
                        case "5AZ0":
                            xnCommon1 = vdoc.CreateElement("AMOUNT-PAYABLE-CENVAT-CREDIT-63A");
                            pNode.AppendChild(xnCommon1);
                            break;
                        case "5AZA":
                            this.mGenerateNode(ref xnCommon1, "VALUE-EXEMPTED-GOODS-CLEAREDS", "VALUE-EXEMPTED-GOODS-CLEARED", ref vdoc, i);
                            break;
                        case "5AZB":
                            this.mGenerateNode(ref xnCommon1, "VALUE-EXEMPTED-SERVICES-PROVIDEDS", "VALUE-EXEMPTED-SERVICES-PROVIDED", ref vdoc, i);
                            break;
                        case "5AZC":
                            this.mGenerateNode(ref xnCommon1, "AMOUNT-PAID-RULE-SIX-CENVAT-CREDITS", "AMOUNT-PAID-RULE-SIX-CENVAT-CREDIT", ref vdoc, i);
                            break;
                        case "5AZD":
                            this.mGenerateNode(ref xnCommon1, "AMOUNT-PAID-RULE-SIX-CASHS", "AMOUNT-PAID-RULE-SIX-CASH", ref vdoc, i);
                            break;
                        case "5AZE":
                            this.mGenerateNode(ref xnCommon1, "TOTAL-AMOUNT-RULE-CENVATS", "TOTAL-AMOUNT-RULE-CENVAT", ref vdoc, i);
                            break;
                        case "5AZF":
                            //this.mGenerateNode(ref xnCommon1, "", "", ref vdoc, i); Pending from Stored Procedure
                            break;
                        default:
                            break;

                    }

                } //for (int i = 0; i <= dvwST3.Table.Rows.Count - 1; i++)
                this.mFwrite("EXEMPTED/NON TAXABLE SERVICE OR EXEMPTED GOODS Details is Generated");
            }
            catch(Exception ex)
            {
                this.mFwrite("EXEMPTED/NON TAXABLE SERVICE OR EXEMPTED GOODS Details not Generated");
                this.xmlfilegenerated = false;
                this.mOnError(ex);
                Application.Exit();
            }
        }
        private void mWriteRawXML_StPayableNotPaid(ref XmlDocument vdoc, ref XmlNode pNode)
        {
            try
            {
                for (int i = 0; i <= dvwST3.Table.Rows.Count - 1; i++)
                {
                    String vSrNo = dvwST3.Table.Rows[i]["srno1"].ToString().Trim() + dvwST3.Table.Rows[i]["srno2"].ToString().Trim();
                    if (vSrNo == "4C")
                    {

                        xmltext = vdoc.CreateTextNode((Convert.ToDecimal(dvwST3.Table.Rows[i]["tamt1"]) + Convert.ToDecimal(dvwST3.Table.Rows[i]["tamt2"])).ToString().Trim());
                        pNode.AppendChild(xmltext);
                    }
                }
                this.mFwrite("Service Tax Payable But Not Paid Details generated");
            }
            catch(Exception ex)
            {
                this.mFwrite("Service Tax Payable But Not Paid Details generated");
                this.xmlfilegenerated = false;
                this.mOnError(ex);
                Application.Exit();
            }
        }
        private void mWriteRawXML_SourceDocumentDetails(ref XmlDocument vdoc, ref XmlNode pNode)
        {
            try
            {
                for (int i = 0; i <= dvwST3.Table.Rows.Count - 1; i++)
                {

                    String vSrNo = dvwST3.Table.Rows[i]["srno1"].ToString().Trim() + dvwST3.Table.Rows[i]["srno2"].ToString().Trim();
                    if (vSrNo != "4B")
                    {
                        continue;
                    }
                    if (dvwST3.Table.Rows[i]["srno3"].ToString().Trim() == "0")
                    {
                        
                    }
                    else
                    {
                        xnCommon1 = vdoc.CreateElement("SOURCE-DOCUMENT-DETAIL");
                        pNode.AppendChild(xnCommon1);

                        xnCommon1_1 = vdoc.CreateElement("PAYMENT-TYPE");
                        xmltext = vdoc.CreateTextNode(dvwST3.Table.Rows[i]["particulars"].ToString().Trim());
                        xnCommon1_1.AppendChild(xmltext);
                        xnCommon1.AppendChild(xnCommon1_1);

                        xnCommon1_1 = vdoc.CreateElement("MONTH_QUARTER");
                        if (dvwST3.Table.Rows[i]["tchalno1"].ToString().Trim() != "")
                        {
                            if (this.cmbPeriod.Text == "April-September")
                            {
                                xmltext = vdoc.CreateTextNode("Apr-Jun");
                            }
                            else
                            {
                                xmltext = vdoc.CreateTextNode("Jul-Sept");
                            }
                        }
                        else
                        {
                            if (this.cmbPeriod.Text == "April-September")
                            {
                                xmltext = vdoc.CreateTextNode("Oct-Dec");
                            }
                            else
                            {
                                xmltext = vdoc.CreateTextNode("Jan-March");
                            }
                        }

                        xnCommon1_1.AppendChild(xmltext);
                        xnCommon1.AppendChild(xnCommon1_1);

                        xnCommon1_1 = vdoc.CreateElement("NO_PERIOD");
                        xmltext = vdoc.CreateTextNode(dvwST3.Table.Rows[i]["sdocno"].ToString().Trim());
                        xnCommon1_1.AppendChild(xmltext);
                        xnCommon1.AppendChild(xnCommon1_1);

                        xnCommon1_1 = vdoc.CreateElement("DATE");
                        if (((DateTime)dvwST3.Table.Rows[i]["sdocdt"]).Year > 1900)
                        {
                            xmltext = vdoc.CreateTextNode(dvwST3.Table.Rows[i]["sdocdt"].ToString().Trim());
                            xnCommon1_1.AppendChild(xmltext);
                        }

                        xnCommon1.AppendChild(xnCommon1_1);

                    }

                }
                this.mFwrite("Source Document Details generated");
            }
            catch(Exception ex)
            {
                this.mFwrite("Source Document Details not generated");
                this.xmlfilegenerated = false;
                this.mOnError(ex);
                Application.Exit();
            }
        }
        private void mWriteRawXML_PaidService(ref XmlDocument vdoc, ref XmlNode pNode)
        {
            try
            {
                for (int i = 0; i <= dvwST3.Table.Rows.Count - 1; i++)
                {
                    if ((string)dvwST3.Table.Rows[i]["srno1"] + (string)dvwST3.Table.Rows[i]["srno2"] + (string)dvwST3.Table.Rows[i]["srno3"] != "4A1")
                    {
                        continue;
                    }
                    String vSrNo = dvwST3.Table.Rows[i]["srno"].ToString().Trim().ToUpper();
                    //--->Service Provider
                    switch (vSrNo)
                    {
                        case "4A1A0":
                            xnCommon1 = vdoc.CreateElement("ST-PAID");
                            pNode.AppendChild(xnCommon1);
                            break;
                        case "4A1A1":
                            this.mGenerateNode(ref xnCommon1, "CASHS", "CASH", ref vdoc, i);
                            break;
                        case "4A1A2":
                            this.mGenerateNode(ref xnCommon1, "CENVAT-CRS", "CENVAT-CR", ref vdoc, i);
                            break;
                        case "4A1A2A":
                            this.mGenerateNode(ref xnCommon1, "TAX-RULE-SIX-ONE-AS", "TAX-RULE-SIX-ONE-A", ref vdoc, i);
                            break;
                        case "4A1A3":
                            this.mGenerateNode(ref xnCommon1, "RULE-6-3-EXCESS-PAID-EARLIER-ADJS", "RULE-6-3-EXCESS-PAID-EARLIER-ADJ", ref vdoc, i);
                            break;
                        case "4A1A4":
                            this.mGenerateNode(ref xnCommon1, "RULE-6-4A-EXCESS-PAID-EARLIER-ADJS", "RULE-6-4A-EXCESS-PAID-EARLIER-ADJ", ref vdoc, i);
                            break;

                        case "4A1B0":
                            xnCommon1 = vdoc.CreateElement("ED-CESS-PAID");
                            pNode.AppendChild(xnCommon1);
                            break;
                        case "4A1B1":
                            this.mGenerateNode(ref xnCommon1, "CASHS", "CASH", ref vdoc, i);
                            break;
                        case "4A1B2":
                            this.mGenerateNode(ref xnCommon1, "CENVAT-CRS", "CENVAT-CR", ref vdoc, i);
                            break;
                        case "4A1B2A":
                            this.mGenerateNode(ref xnCommon1, "EDU-CESS-RULE-SIX-ONE-AS", "EDU-CESS-RULE-SIX-ONE-A", ref vdoc, i);
                            break;
                        case "4A1B3":
                            this.mGenerateNode(ref xnCommon1, "RULE-6-3-EXCESS-PAID-EARLIER-ADJS", "RULE-6-3-EXCESS-PAID-EARLIER-ADJ", ref vdoc, i);
                            break;
                        case "4A1B4":
                            this.mGenerateNode(ref xnCommon1, "RULE-6-4A-EXCESS-PAID-EARLIER-ADJS", "RULE-6-4A-EXCESS-PAID-EARLIER-ADJ", ref vdoc, i);
                            break;

                        case "4A1C0":
                            xnCommon1 = vdoc.CreateElement("HIGHER-ED-CESS-PAID");
                            pNode.AppendChild(xnCommon1);
                            break;
                        case "4A1C1":
                            this.mGenerateNode(ref xnCommon1, "CASHS", "CASH", ref vdoc, i);
                            break;
                        case "4A1C2":
                            this.mGenerateNode(ref xnCommon1, "CENVAT-CRS", "CENVAT-CR", ref vdoc, i);
                            break;
                        case "4A1C2A":
                            this.mGenerateNode(ref xnCommon1, "SEC-EDU-CESS-RULE-SIX-ONE-AS", "SEC-EDU-CESS-RULE-SIX-ONE-A", ref vdoc, i);
                            break;
                        case "4A1C3":
                            this.mGenerateNode(ref xnCommon1, "RULE-6-3-EXCESS-PAID-EARLIER-ADJS", "RULE-6-3-EXCESS-PAID-EARLIER-ADJ", ref vdoc, i);
                            break;
                        case "4A1C4":
                            this.mGenerateNode(ref xnCommon1, "RULE-6-4A-EXCESS-PAID-EARLIER-ADJS", "RULE-6-4A-EXCESS-PAID-EARLIER-ADJ", ref vdoc, i);
                            break;

                        case "4A1D0":
                            xnCommon1 = vdoc.CreateElement("OTHER-AMTS-PAID");
                            pNode.AppendChild(xnCommon1);
                            break;
                        case "4A1D1":
                            this.mGenerateNode(ref xnCommon1, "REV-CASH-ARREARS", "REV-CASH-ARREAR", ref vdoc, i);
                            break;
                        case "4A1D2":
                            this.mGenerateNode(ref xnCommon1, "REV-CR-ARREARS", "REV-CR-ARREAR", ref vdoc, i);
                            break;
                        case "4A1D3":
                            this.mGenerateNode(ref xnCommon1, "ED-CESS-CASH-ARREARS", "ED-CESS-CASH-ARREAR", ref vdoc, i);
                            break;
                        case "4A1D4":
                            this.mGenerateNode(ref xnCommon1, "ED-CESS-CR-ARREARS", "ED-CESS-CR-ARREAR", ref vdoc, i);
                            break;
                        case "4A1D5":
                            this.mGenerateNode(ref xnCommon1, "HIGHER-ED-CESS-CASH-ARREARS", "HIGHER-ED-CESS-CASH-ARREAR", ref vdoc, i);
                            break;
                        case "4A1D6":
                            this.mGenerateNode(ref xnCommon1, "HIGHER-ED-CESS-CR-ARREARS", "HIGHER-ED-CESS-CR-ARREAR", ref vdoc, i);
                            break;
                        case "4A1D7":
                            this.mGenerateNode(ref xnCommon1, "INTEREST-PAIDS", "INTEREST-PAID", ref vdoc, i);
                            break;
                        case "4A1D8":
                            this.mGenerateNode(ref xnCommon1, "PENALTY-PAIDS", "PENALTY-PAID", ref vdoc, i);
                            break;
                        case "4A1D9":
                            this.mGenerateNode(ref xnCommon1, "AMOUNT-73A-PAIDS", "AMOUNT-73A-PAID", ref vdoc, i);
                            break;
                        case "4A1D10":
                            this.mGenerateNode(ref xnCommon1, "SPECIFIED-AMOUNT-PAIDS", "SPECIFIED-AMOUNT-PAID", ref vdoc, i);
                            break;
                        default:
                            break;
                    }
                }
                this.mFwrite("Paid Service Details generated");        
            }
            catch (Exception ex)
            {
                this.mFwrite("Paid Service Details not generated");
                this.xmlfilegenerated = false;
                this.mOnError(ex);
                Application.Exit();
            }

        }
        private void mWriteRawXML_ChallanNumbers(ref XmlDocument vdoc, ref XmlNode pNode)
        {
            try
            {
                for (int i = 0; i <= dvwST3.Table.Rows.Count - 1; i++)
                {

                    String vSrNo = dvwST3.Table.Rows[i]["srno1"].ToString().Trim() + dvwST3.Table.Rows[i]["srno2"].ToString().Trim() + dvwST3.Table.Rows[i]["srno3"].ToString().Trim()+ dvwST3.Table.Rows[i]["srno4"].ToString().Trim();
                    if (vSrNo != "4A2A")
                    {
                        continue;
                    }
                    //--->Service Provider
                    switch (vSrNo)
                    {

                        case "4A2A":
                            xnCommon1_1 = vdoc.CreateElement("CHALLAN-NUMBER");
                            xnCommon1_2 = vdoc.CreateElement("CHALLAN-NUMBER");
                            string vChalNo = Convert.ToString(dvwST3.Table.Rows[i]["tChalNo1"]);
                            if (vChalNo != "")
                            {
                                xaCommon = vdoc.CreateAttribute("Month");
                                if (this.cmbPeriod.Text == "April-September")
                                {
                                    xaCommon.Value = "Apr_Jun";
                                    xnCommon1_1.Attributes.Append(xaCommon);
                                    xmltext = vdoc.CreateTextNode(vChalNo);
                                    xnCommon1_1.AppendChild(xmltext);
                                }
                                else
                                {
                                    xaCommon.Value = "Oct_Dec";
                                    xnCommon1_1.Attributes.Append(xaCommon);
                                    xmltext = vdoc.CreateTextNode(vChalNo);
                                    xnCommon1_1.AppendChild(xmltext);
                                }
                                pNode.AppendChild(xnCommon1_1);
                                //pNode.AppendChild(xnCommon1_2);
                            }


                            vChalNo = Convert.ToString(dvwST3.Table.Rows[i]["tChalNo2"]);
                            if (vChalNo != "")
                            {
                                xaCommon = vdoc.CreateAttribute("Month");
                                if (this.cmbPeriod.Text == "April-September")
                                {
                                    xaCommon.Value = "Jul_Sep";
                                    xnCommon1_2.Attributes.Append(xaCommon);
                                    xmltext = vdoc.CreateTextNode(vChalNo);
                                    xnCommon1_2.AppendChild(xmltext);
                                }
                                else
                                {
                                    xaCommon.Value = "Jan_Mar";
                                    xnCommon1_2.Attributes.Append(xaCommon);
                                    xmltext = vdoc.CreateTextNode(vChalNo);
                                    xnCommon1_2.AppendChild(xmltext);
                                }
  
                                //pNode.AppendChild(xnCommon1_1);
                                pNode.AppendChild(xnCommon1_2);
                            }
                            
                            break;
                        default:
                            break;

                    }
                }//for (int i = 0; i <= dvwST3.Table.Rows.Count - 1; i++)
                this.mFwrite("Challan Details generated");
            }
            catch (Exception ex)
            {
                this.mFwrite("Challan Details not generated");
                this.xmlfilegenerated = false;
                this.mOnError(ex);
                Application.Exit();
            }

        }
        private void mWriteRawXML_AdvancePayment(ref XmlDocument vdoc, ref XmlNode pNode)
        {
            try
            {
                for (int i = 0; i <= dvwST3.Table.Rows.Count - 1; i++)
                {
                    String vSrNo = dvwST3.Table.Rows[i]["srno1"].ToString().Trim() + dvwST3.Table.Rows[i]["srno2"].ToString().Trim() + dvwST3.Table.Rows[i]["srno3"].ToString().Trim();
                    //--->Service Provider
                    switch (vSrNo)
                    {
                        case "41":
                            this.mGenerateNode(ref pNode, "ADVANCE-PAYMENT-AMOUNT-DEPOSITEDS", "ADVANCE-PAYMENT-AMOUNT-DEPOSITED", ref vdoc, i);
                            
                            xnCommon1_3 = vdoc.CreateElement("ADVANCE-PAYMENT-CHALLAN-NUMBERS");
                            pNode.AppendChild(xnCommon1_3);
                            break;
                        case "42":
                            //xnCommon1_3 = vdoc.CreateElement("ADVANCE-PAYMENT-CHALLAN-NUMBERS");
                            //pNode.AppendChild(xnCommon1_3);

                            xnCommon1_4 = vdoc.CreateElement("ADVANCE-PAYMENT-CHALLAN-NUMBER");
                            xnCommon1_3.AppendChild(xnCommon1_4);
                            xaCommon = vdoc.CreateAttribute("Month");

                            if (this.cmbPeriod.Text == "April-September")
                            {
                                xaCommon.Value = "Apr_Jun";
                                xnCommon1_4.Attributes.Append(xaCommon);
                                xmltext = vdoc.CreateTextNode(Convert.ToString(dvwST3.Table.Rows[i]["tChalNo1"]));
                                xnCommon1_4.AppendChild(xmltext);
                            }
                            else
                            {
                                xaCommon.Value = "Oct_Dec";
                                xnCommon1_4.Attributes.Append(xaCommon);
                                xmltext = vdoc.CreateTextNode(Convert.ToString(dvwST3.Table.Rows[i]["tChalNo1"]));
                                xnCommon1_4.AppendChild(xmltext);
                            }

                            if (this.cmbPeriod.Text == "April-September")
                            {
                                xaCommon.Value = "Jul_Sep";
                                xnCommon1_4.Attributes.Append(xaCommon);
                                xmltext = vdoc.CreateTextNode(Convert.ToString(dvwST3.Table.Rows[i]["tChalNo2"]));
                                xnCommon1_4.AppendChild(xmltext);
                            }
                            else
                            {
                                xaCommon.Value = "Jan_Mar";
                                xnCommon1_4.Attributes.Append(xaCommon);
                                xmltext = vdoc.CreateTextNode(Convert.ToString(dvwST3.Table.Rows[i]["tChalNo2"]));
                                xnCommon1_4.AppendChild(xmltext);
                            }
                            break;
                        default:
                            break;

                    }
                }//for (int i = 0; i <= dvwST3.Table.Rows.Count - 1; i++)
                this.mFwrite("Advance Payment Details generated");
            }
            catch (Exception ex)
            {
                this.mFwrite("Advance Payment Details not generated");
                this.xmlfilegenerated = false;
                this.mOnError(ex);
                Application.Exit();
            }


        }
        private void mWriteRawXML_TaxableService(ref XmlDocument vdoc, ref XmlNode Pnode)
        {
            try
            {
                String mServiceCode = "";
                if (dvwST3.Table.Rows.Count > 0)
                {
                    //mServiceCode = (string)dvwST3.Table.Rows[0]["code"];
                }





                XmlNode xnTAXABLESERVICE = vdoc.CreateElement("TAXABLE-SERVICE");
                XmlNode xnPAYABLESERVICE = vdoc.CreateElement("PAYABLE-SERVICE");

                for (int i = 0; i <= dvwST3.Table.Rows.Count - 1; i++)
                {
                   

                    if ((string)dvwST3.Table.Rows[i]["srno1"] != "3")
                    {
                        continue;
                    }

                    
                    if (mServiceCode != (string)dvwST3.Table.Rows[i]["code"])
                    {
                       xnTAXABLESERVICE = vdoc.CreateElement("TAXABLE-SERVICE");
                       xnPAYABLESERVICE = vdoc.CreateElement("PAYABLE-SERVICE");
                        mServiceCode = (string)dvwST3.Table.Rows[i]["code"];
                    }
                    else
                    {

                    }

                    String vSrNo = dvwST3.Table.Rows[i]["srno"].ToString().Trim() ;
                    vSrNo = vSrNo.ToUpper();
                    switch (vSrNo)
                    {
                        case "3A10":
                            xnTAXABLESERVICE = vdoc.CreateElement("TAXABLE-SERVICE");

                            XmlAttribute xaServiceProvided = vdoc.CreateAttribute("ServiceProvided");
                            xaServiceProvided.Value = (string)dvwST3.Table.Rows[i]["code"];
                            xnTAXABLESERVICE.Attributes.Append(xaServiceProvided);
                            break;
                        case "3A21":
                            XmlNode xnSERVICEPROVIDERFLG = vdoc.CreateElement("SERVICE-PROVIDER-FLG");
                            xmltext = vdoc.CreateTextNode((string)dvwST3.Table.Rows[i]["sprovider"]);
                            xnSERVICEPROVIDERFLG.AppendChild(xmltext);
                            xnPAYABLESERVICE.AppendChild(xnSERVICEPROVIDERFLG);
                            break;
                        case "3A22":
                            XmlNode xnSERVICERECIEVERFLG = vdoc.CreateElement("SERVICE-RECIEVER-FLG");
                            xmltext = vdoc.CreateTextNode((string)dvwST3.Table.Rows[i]["sreceiver"]);
                            xnSERVICERECIEVERFLG.AppendChild(xmltext);
                            xnPAYABLESERVICE.AppendChild(xnSERVICERECIEVERFLG);
                            break;
                        case "3B0":
                            xnCommon1 = vdoc.CreateElement("SUB-CLAUSE-NUMBER");
                            xmltext = vdoc.CreateTextNode((string)dvwST3.Table.Rows[i]["ssubcls"]);
                            xnCommon1.AppendChild(xmltext);
                            xnPAYABLESERVICE.AppendChild(xnCommon1);
                            break;
                        case "3C1":
                            xnCommon1 = vdoc.CreateElement("BENEFIT-EXEMPTION-NOTIFICATION");
                            xmltext = vdoc.CreateTextNode(((string)dvwST3.Table.Rows[i]["sexnoti"]).Trim());
                            xnCommon1.AppendChild(xmltext);
                            xnPAYABLESERVICE.AppendChild(xnCommon1);

                            xnCommon1 = vdoc.CreateElement("NOTIFICATION-NUMBERS");
                            xnPAYABLESERVICE.AppendChild(xnCommon1);
                            break;
                        case "3C2":
                            xnCommon1_1 = vdoc.CreateElement("NOTIFICATION-NUMBER");
                            xmltext = vdoc.CreateTextNode((string)dvwST3.Table.Rows[i]["sexnoti"]);         
                            xnCommon1_1.AppendChild(xmltext);
                            xnCommon1.AppendChild(xnCommon1_1);
                            break;
                        case "3D0":
                            xnCommon1 = vdoc.CreateElement("NOTIFICATION-SNO");
                            xmltext = vdoc.CreateTextNode((string)dvwST3.Table.Rows[i]["SabtSr"]); 
                            xnCommon1.AppendChild(xmltext);
                            xnPAYABLESERVICE.AppendChild(xnCommon1);
                            break;
                        case "3E1":
                            xnCommon1 = vdoc.CreateElement("PROVISIONALLY-ASSESSED");
                            xmltext = vdoc.CreateTextNode((string)dvwST3.Table.Rows[i]["sexnoti"]);
                            xnCommon1.AppendChild(xmltext);
                            xnPAYABLESERVICE.AppendChild(xnCommon1);
                            break;
                        case "3E2":
                            xnCommon1 = vdoc.CreateElement("PA-ORDER-NO");
                            xmltext = vdoc.CreateTextNode((string)dvwST3.Table.Rows[i]["sexnoti"]); 
                            xnCommon1.AppendChild(xmltext);
                            xnPAYABLESERVICE.AppendChild(xnCommon1);
                            break;

                        default:
                            break;
                    }
                    vSrNo = dvwST3.Table.Rows[i]["srno"].ToString().Trim() + (string)dvwST3.Table.Rows[i]["sprovider"];
                    vSrNo = vSrNo.ToUpper();
                    
                   switch (vSrNo)
                   {
                       //--->Service Provider 
                       case "3F1A0YES":
                           xnCommon1 = vdoc.CreateElement("SERVICE-PROVIDER");
                           xnPAYABLESERVICE.AppendChild(xnCommon1);
                           xnCommon1_1 = vdoc.CreateElement("SERVICE-TAX-PAYABLE");
                           xnCommon1.AppendChild(xnCommon1_1);
                           xnCommon1_2 = vdoc.CreateElement("GR-AMT-RECD-MNY");
                           xnCommon1_1.AppendChild(xnCommon1_2);
                           break;
                       case "3F1A1YES":
                           this.mGenerateNode(ref xnCommon1_2, "SRV-PROVIDEDS", "SRV-PROVIDED", ref vdoc, i);
                           break;
                       case "3F1A2YES":
                           this.mGenerateNode(ref xnCommon1_2, "SRV-TOBE-PROVIDEDS", "SRV-TOBE-PROVIDED", ref vdoc, i);
                           break;
                       case "3F1B0YES":
                           this.mGenerateNode(ref xnCommon1_1, "MNY-EQUIV-CONS-RECVD-OTH-THAN-MNYS", "MNY-EQUIV-CONS-RECVD-OTH-THAN-MNY", ref vdoc, i);
                           break;
                       case "3F1C0YES":
                           xnCommon1_2 = vdoc.CreateElement("VAL-ST-EXEMPT");
                           xnCommon1_1.AppendChild(xnCommon1_2);
                           break;
                       case "3F1C1YES":
                           this.mGenerateNode(ref xnCommon1_2, "AMT-RECD-EXP-SRVS", "AMT-RECD-EXP-SRV", ref vdoc, i);
                           break;
                       case "3F1C2YES":
                           this.mGenerateNode(ref xnCommon1_2, "AMT-RECD-EXEMPT-SRVS", "AMT-RECD-EXEMPT-SRV", ref vdoc, i);
                           break;
                       case "3F1C3YES":
                           this.mGenerateNode(ref xnCommon1_2, "AMT-RECD-PURE-AGENTS", "AMT-RECD-PURE-AGENT", ref vdoc, i);
                           break;
                       case "3F1D0YES":
                           this.mGenerateNode(ref xnCommon1_1, "ABTMT-AMT-CLAIMEDS", "ABTMT-AMT-CLAIMED", ref vdoc, i);
                           break;
                       case "3F1E0YES":
                           this.mGenerateNode(ref xnCommon1_1, "TAXABLE-VALUES", "TAXABLE-VALUE", ref vdoc, i);
                           break;
                       case "3F1F0YES":

                           xnCommon1_2 = vdoc.CreateElement("ST-RATEWISE-BREAKUPS");
                           xnCommon1_1.AppendChild(xnCommon1_2);
                           break;

                       case "3F1FYES":
                           if ((((decimal)dvwST3.Table.Rows[i]["tamt1"] + (decimal)dvwST3.Table.Rows[i]["tamt2"]) != 0) || ((decimal)dvwST3.Table.Rows[i]["serbper"]==99))
                           {
                            this.mGenerateNode(ref xnCommon1_2, "ST-RATEWISE-BREAKUP", "VALUE", ref vdoc, i);

                           }
                           break;

                       case "3F1G0YES":
                           this.mGenerateNode(ref xnCommon1_1, "TAX-PAYABLES", "TAX-PAYABLE", ref vdoc, i);
                           break;
                       case "3F1H0YES":
                           this.mGenerateNode(ref xnCommon1_1, "ED-CESS-PAYABLES", "ED-CESS-PAYABLE", ref vdoc, i);
                           break;
                       case "3F1I0YES":
                           this.mGenerateNode(ref xnCommon1_1, "HIGHER-ED-CESS-PAYABLES", "HIGHER-ED-CESS-PAYABLE", ref vdoc, i);
                           break;
                       case "3F2J0YES":
                           xnCommon1_1 = vdoc.CreateElement("TAX-AMT-CHGD");
                           xnCommon1.AppendChild(xnCommon1_1);
                           this.mGenerateNode(ref xnCommon1_1, "GR-AMT-SRV-PROVIDEDS", "GR-AMT-SRV-PROVIDED", ref vdoc, i);
                           break;
                       case "3F2K0YES":
                           this.mGenerateNode(ref xnCommon1_1, "MONEY-EQUIV-CHGD-AS-OTHR-MONEYS", "MONEY-EQUIV-CHGD-AS-OTHR-MONEY", ref vdoc, i);
                           break;
                       case "3F2L0YES":
                           this.mGenerateNode(ref xnCommon1_1, "AMT-CHGD-EXP-SRVCS-PROVIDEDS", "AMT-CHGD-EXP-SRVCS-PROVIDED", ref vdoc, i);
                           break;
                       case "3F2M0YES":
                           this.mGenerateNode(ref xnCommon1_1, "AMT-CHGD-EXMPTD-SRVCS-PROVIDEDS", "AMT-CHGD-EXMPTD-SRVCS-PROVIDED", ref vdoc, i);
                           break;
                       case "3F2N0YES":
                           this.mGenerateNode(ref xnCommon1_1, "AMT-CHGD-PURE-AGENTS", "AMT-CHGD-PURE-AGENT", ref vdoc, i);
                           break;
                       case "3F2O0YES":
                           this.mGenerateNode(ref xnCommon1_1, "AMT-CLMD-ABTMNTS", "AMT-CLMD-ABTMNT", ref vdoc, i);
                           break;
                       case "3F2P0YES":
                           this.mGenerateNode(ref xnCommon1_1, "NET-TAX-AMT-CHGDS", "NET-TAX-AMT-CHGD", ref vdoc, i);
                           //xnPAYABLESERVICE.AppendChild(xnCommon1);
                           break;
                       //<---Service Provider 
                       //--->Service Service Receiver
                       case "3F1A0NO":
                           xnCommon1n = vdoc.CreateElement("SERVICE-RECEPIENT");
                           xnPAYABLESERVICE.AppendChild(xnCommon1n);
                           xnCommon1_1 = vdoc.CreateElement("SERVICE-TAX-PAYABLE");
                           xnCommon1n.AppendChild(xnCommon1_1);
                           xnCommon1_2 = vdoc.CreateElement("GR-AMT-RECD-MNY");
                           xnCommon1_1.AppendChild(xnCommon1_2);
                           break;

                       case "3F1A1NO":
                           this.mGenerateNode(ref xnCommon1_2, "SRV-PROVIDEDS", "SRV-PROVIDED", ref vdoc, i);
                           break;
                       case "3F1A2NO":
                           this.mGenerateNode(ref xnCommon1_2, "SRV-TOBE-PROVIDEDS", "SRV-TOBE-PROVIDED", ref vdoc, i);
                           break;
                       case "3F1B0NO":
                           this.mGenerateNode(ref xnCommon1_1, "MNY-EQUIV-CONS-RECVD-OTH-THAN-MNYS", "MNY-EQUIV-CONS-RECVD-OTH-THAN-MNY", ref vdoc, i);
                           break;
                       case "3F1C0NO":
                           xnCommon1_2 = vdoc.CreateElement("VAL-ST-EXEMPT");
                           xnCommon1_1.AppendChild(xnCommon1_2);
                           break;

                       case "3F1C1NO":
                           this.mGenerateNode(ref xnCommon1_2, "AMT-PAID-EXEMPT-SRVS", "AMT-PAID-EXEMPT-SRV", ref vdoc, i);
                           break;
                       case "3F1C2NO":
                           this.mGenerateNode(ref xnCommon1_2, "AMT-PAID-PURE-AGENTS", "AMT-PAID-PURE-AGENT", ref vdoc, i);
                           break;
                       case "3F1D0NO":
                           this.mGenerateNode(ref xnCommon1_1, "ABTMT-AMT-CLAIMEDS", "ABTMT-AMT-CLAIMED", ref vdoc, i);
                           break;
                       case "3F1E0NO":
                           this.mGenerateNode(ref xnCommon1_1, "TAXABLE-VALUES", "TAXABLE-VALUE", ref vdoc, i);
                           break;
                       case "3F1F0NO":
                           xnCommon1_2 = vdoc.CreateElement("ST-RATEWISE-BREAKUPS");
                           xnCommon1_1.AppendChild(xnCommon1_2);
                           break;
                       case "3F1FNO":
                           if ((((decimal)dvwST3.Table.Rows[i]["tamt1"] + (decimal)dvwST3.Table.Rows[i]["tamt2"]) != 0) || ((decimal)dvwST3.Table.Rows[i]["serbper"] == 99))
                           {
                               this.mGenerateNode(ref xnCommon1_2, "ST-RATEWISE-BREAKUP", "VALUE", ref vdoc, i);
                           }
                           //this.mGenerateNode(ref xnCommon1_2, "ST-RATEWISE-BREAKUP", "VALUE", ref vdoc, i);
                           break;

                       case "3F1G0NO":
                           this.mGenerateNode(ref xnCommon1_1, "TAX-PAYABLES", "TAX-PAYABLE", ref vdoc, i);
                           break;
                       case "3F1H0NO":
                           this.mGenerateNode(ref xnCommon1_1, "ED-CESS-PAYABLES", "ED-CESS-PAYABLE", ref vdoc, i);
                           break;
                       case "3F1I0NO":
                           this.mGenerateNode(ref xnCommon1_1, "HIGHER-ED-CESS-PAYABLES", "HIGHER-ED-CESS-PAYABLE", ref vdoc, i);
                           break;
                       case "3F2J0NO":
                           xnCommon1_1 = vdoc.CreateElement("TAX-AMT-PAID");
                           xnCommon1n.AppendChild(xnCommon1_1);
                           this.mGenerateNode(ref xnCommon1_1, "GR-AMT-SRV-RECEIVEDS", "GR-AMT-SRV-RECEIVED", ref vdoc, i);
                           break;
                       case "3F2K0NO":
                           this.mGenerateNode(ref xnCommon1_1, "MONEY-EQUIV-PAID-AS-OTHR-MONEYS", "MONEY-EQUIV-PAID-AS-OTHR-MONEY", ref vdoc, i);
                           break;
                       //case "3F2L0NO":
                       //    this.mGenerateNode(ref xnCommon1_1, "AMT-CHGD-EXP-SRVCS-PROVIDEDS", "AMT-CHGD-EXP-SRVCS-PROVIDED", ref vdoc, i);
                       //    break;
                       case "3F2L0NO":
                           this.mGenerateNode(ref xnCommon1_1, "AMT-PAID-EXMPTD-SRVCS-RECEIVEDS", "AMT-PAID-EXMPTD-SRVCS-RECEIVED", ref vdoc, i);
                           break;
                       case "3F2M0NO":
                           this.mGenerateNode(ref xnCommon1_1, "AMT-PAID-PURE-AGENTS", "AMT-PAID-PURE-AGENT", ref vdoc, i);
                           break;
                       case "3F2N0NO":
                           this.mGenerateNode(ref xnCommon1_1, "AMT-CLMD-ABTMNTS", "AMT-CLMD-ABTMNT", ref vdoc, i);
                           break;
                       case "3F2O0NO":
                           this.mGenerateNode(ref xnCommon1_1, "NET-TAX-AMT-PAIDS", "NET-TAX-AMT-PAID", ref vdoc, i);

                           xnTAXABLESERVICE.AppendChild(xnPAYABLESERVICE);
                           Pnode.AppendChild(xnTAXABLESERVICE);
                           break;

                       //<---Service Service Receiver
                       default:
                           break;

                   } //switch (vSrNo)

                    ////<---Service Receiver 
                }//for (int i = 0; i <= dvwST3.Table.Rows.Count - 1; i++)
                this.mFwrite("Taxable Service Details Generated");
            }
            catch (Exception ex)
            {
                this.mFwrite("Taxable Service Details not Generated");
                this.xmlfilegenerated = false;
                this.mOnError(ex);
                Application.Exit();
            }

        }
        private void mGenerateNode(ref XmlNode pNode, string NodeName, string SubNodeName, ref XmlDocument vdoc, int i)
        {
            try
            {
                xnCommon1_3 = vdoc.CreateElement(NodeName);
                if (dvwST3.Table.Rows[i]["srno"].ToString().Trim().ToUpper() == "4A1D10")
                {
                    xaCommon = vdoc.CreateAttribute("Details");
                    xaCommon.Value = "Others";
                    xnCommon1_3.Attributes.Append(xaCommon);
                }

                if (dvwST3.Table.Rows[i]["srno"].ToString().Trim().IndexOf("3F1F") > -1 && ((decimal)dvwST3.Table.Rows[i]["serbper"] != 99))
                {
                    
                    string vrate = Convert.ToString(dvwST3.Table.Rows[i]["serbper"]);
                    //if (vrate == "0.00")
                    //{
                    //    vrate = "";
                    //}
                    xaCommon = vdoc.CreateAttribute("Rate");
                    xaCommon.Value = vrate;
                    xnCommon1_3.Attributes.Append(xaCommon);
                    pNode.AppendChild(xnCommon1_3);

                    vrate = Convert.ToString(dvwST3.Table.Rows[i]["sercper"]);
                    xaCommon = vdoc.CreateAttribute("EduCess");
                    xaCommon.Value = vrate;
                    xnCommon1_3.Attributes.Append(xaCommon);
                    pNode.AppendChild(xnCommon1_3);

                    vrate = Convert.ToString(dvwST3.Table.Rows[i]["serhper"]);
                    xaCommon = vdoc.CreateAttribute("SecEduCess");
                    xaCommon.Value = vrate;
                    xnCommon1_3.Attributes.Append(xaCommon);

                    //if (vrate == "")
                    //{
                    //    xaCommon = vdoc.CreateAttribute("Specified");
                    //    xaCommon.Value = "No";

                    //}
                    //xnCommon1_3.Attributes.Append(xaCommon);
                }

                if (dvwST3.Table.Rows[i]["srno"].ToString().Trim().IndexOf("3F1F") > -1 && ((decimal)dvwST3.Table.Rows[i]["serbper"] == 99))
                {
                    xaCommon = vdoc.CreateAttribute("Rate");
                    xaCommon.Value = "";
                    xnCommon1_3.Attributes.Append(xaCommon);
                    pNode.AppendChild(xnCommon1_3);

                    xaCommon = vdoc.CreateAttribute("EduCess");
                    xaCommon.Value = "";
                    xnCommon1_3.Attributes.Append(xaCommon);
                    pNode.AppendChild(xnCommon1_3);

                    xaCommon = vdoc.CreateAttribute("SecEduCess");
                    xaCommon.Value = "";
                    xnCommon1_3.Attributes.Append(xaCommon);

                }
                pNode.AppendChild(xnCommon1_3);
                if (dvwST3.Table.Rows[i]["srno"].ToString().Trim().IndexOf("3F1F") > -1 )
                {
                    if (((decimal)dvwST3.Table.Rows[i]["serbper"] == 99) || (Convert.ToDecimal(dvwST3.Table.Rows[i]["tamt1"]) + Convert.ToDecimal(dvwST3.Table.Rows[i]["tamt2"]) == 0))
                    {
                        return;
                    }
                }
                xnCommon1_4 = vdoc.CreateElement(SubNodeName);


                xnCommon1_3.AppendChild(xnCommon1_4);
                xaCommon = vdoc.CreateAttribute("Duration");

                if (this.cmbPeriod.Text == "April-September")
                {
                    xaCommon.Value = "Apr_Jun";
                    xnCommon1_4.Attributes.Append(xaCommon);
                    xmltext = vdoc.CreateTextNode(Convert.ToString(dvwST3.Table.Rows[i]["tamt1"]));
                    xnCommon1_4.AppendChild(xmltext);
                }
                else
                {
                    xaCommon.Value = "Oct_Dec";
                    xnCommon1_4.Attributes.Append(xaCommon);
                    xmltext = vdoc.CreateTextNode(Convert.ToString(dvwST3.Table.Rows[i]["tamt1"]));
                    xnCommon1_4.AppendChild(xmltext);
                }
                xnCommon1_4 = vdoc.CreateElement(SubNodeName);
                xnCommon1_3.AppendChild(xnCommon1_4);

                xaCommon = vdoc.CreateAttribute("Duration");
                if (this.cmbPeriod.Text == "April-September")
                {
                    xaCommon.Value = "Jul_Sep";
                    xnCommon1_4.Attributes.Append(xaCommon);
                    xmltext = vdoc.CreateTextNode(Convert.ToString(dvwST3.Table.Rows[i]["tamt2"]));
                    xnCommon1_4.AppendChild(xmltext);
                }
                else
                {
                    xaCommon.Value = "Jan_Mar";
                    xnCommon1_4.Attributes.Append(xaCommon);
                    xmltext = vdoc.CreateTextNode(Convert.ToString(dvwST3.Table.Rows[i]["tamt2"]));
                    xnCommon1_4.AppendChild(xmltext);
                }

                xnCommon1_4 = vdoc.CreateElement(SubNodeName);
                xaCommon = vdoc.CreateAttribute("Description");
                xaCommon.Value = "Total";
                xnCommon1_4.Attributes.Append(xaCommon);


                xmltext = vdoc.CreateTextNode(Convert.ToString((Decimal)dvwST3.Table.Rows[i]["tamt1"] + (Decimal)dvwST3.Table.Rows[i]["tamt2"]));
                xnCommon1_4.AppendChild(xmltext);
                xnCommon1_3.AppendChild(xnCommon1_4);
            }
            catch (Exception ex)
            {
                this.mFwrite("Error found in Node Generation for" + NodeName + "--->" + SubNodeName);
                this.xmlfilegenerated = false;
                this.mOnError(ex);
                Application.Exit();
            }

        }
        private void mWriteRawXML_Header(ref XmlDocument vdoc, ref XmlNode vxnHEADERDATA)
        {
            try
            {

                XmlNode xnSTCNUMBER = vdoc.CreateElement("STC-NUMBER");
                xmltext = vdoc.CreateTextNode(dvwCoAdditional.Table.Rows[0]["sregn"].ToString().Trim());
                xnSTCNUMBER.AppendChild(xmltext);
                vxnHEADERDATA.AppendChild(xnSTCNUMBER);
                XmlNode xnASSESSEENAME = vdoc.CreateElement("ASSESSEE-NAME");
                xmltext = vdoc.CreateTextNode(dvwCompany.Table.Rows[0]["co_name"].ToString().Trim());
                xnASSESSEENAME.AppendChild(xmltext);
                vxnHEADERDATA.AppendChild(xnASSESSEENAME);
                XmlNode xnFINANCIALYEAR = vdoc.CreateElement("FINANCIAL-YEAR");
                xmltext = vdoc.CreateTextNode(this.cmbFYear.Text);
                xnFINANCIALYEAR.AppendChild(xmltext);
                vxnHEADERDATA.AppendChild(xnFINANCIALYEAR);
                XmlNode xnRETURNPERIOD = vdoc.CreateElement("RETURN-PERIOD");
                xmltext = vdoc.CreateTextNode(this.cmbPeriod.Text);
                xnRETURNPERIOD.AppendChild(xmltext);
                vxnHEADERDATA.AppendChild(xnRETURNPERIOD);
                XmlNode xnSINGLRETIND = vdoc.CreateElement("SINGL-RET-IND");
                xmltext = vdoc.CreateTextNode("Yes");
                xnSINGLRETIND.AppendChild(xmltext);
                vxnHEADERDATA.AppendChild(xnSINGLRETIND);
                XmlNode xnLARGETAXPAYER = vdoc.CreateElement("LARGE-TAXPAYER");
                xmltext = vdoc.CreateTextNode((dvwCoAdditional.Table.Rows[0]["LTU"].ToString() == "True" ? "Yes" : "No"));
                xnLARGETAXPAYER.AppendChild(xmltext);
                vxnHEADERDATA.AppendChild(xnLARGETAXPAYER);
                XmlNode xnLTUCITY = vdoc.CreateElement("LTU-CITY");
                xmltext = vdoc.CreateTextNode((dvwCoAdditional.Table.Rows[0]["LTU"].ToString() == "True" ? dvwCoAdditional.Table.Rows[0]["LTUCITY"].ToString() : ""));
                xnLTUCITY.AppendChild(xmltext);
                vxnHEADERDATA.AppendChild(xnLTUCITY);
                XmlNode xnPREMISESCODE = vdoc.CreateElement("PREMISES-CODE");
                xmltext = vdoc.CreateTextNode(dvwCoAdditional.Table.Rows[0]["premisecd"].ToString().Trim());
                xnPREMISESCODE.AppendChild(xmltext);
                vxnHEADERDATA.AppendChild(xnPREMISESCODE);
                XmlNode xnASSESSEECONSTITUTION = vdoc.CreateElement("ASSESSEE-CONSTITUTION");
                xmltext = vdoc.CreateTextNode(dvwCoAdditional.Table.Rows[0]["typeorg"].ToString().Trim());
                xnASSESSEECONSTITUTION.AppendChild(xmltext);
                vxnHEADERDATA.AppendChild(xnASSESSEECONSTITUTION);
                this.mFwrite("Header Details generated");
            }
            catch (Exception ex)
            {
                this.mFwrite("Header Details not generated");
                this.xmlfilegenerated = false;
                this.mOnError(ex);
                Application.Exit();
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void mGenerateCRC()
        {
            //vFileNM = @"D:\ST-3 Efilling\ACES-EFiling-ST3\ACES-EFiling-ST3\AAACU1234DST001_8262010102747AM.xml";

            try
            {
                
                string vFileNM = this.xmlFileName;
                Crc32.Crc32 crc32 = new Crc32.Crc32();
                String hash = String.Empty;
                using (FileStream fs = File.Open(vFileNM, FileMode.Open))
                    foreach (byte b in crc32.ComputeHash(fs)) hash += b.ToString("x2").ToUpper();
                
                if (hash == "")
                {
                    return;
                }
                this.crcValue = hash;
                
                this.lbl.Text = "Generating Company Details...";
                this.mFwrite("Generating Company Details...");

                XmlDocument vdoc1 = new XmlDocument();


                XmlNode xnInfoFile = vdoc1.CreateElement("InfoFile");
                XmlNode xnClientInfo = vdoc1.CreateElement("ClientInfo");
                xnInfoFile.AppendChild(xnClientInfo);
                vdoc1.AppendChild(xnInfoFile);

                XmlNode xnCompName = vdoc1.CreateElement("CompName");
                xmltext = vdoc1.CreateTextNode(this.dvwCompany.Table.Rows[0]["co_name"].ToString());
                xnCompName.AppendChild(xmltext);
                xnClientInfo.AppendChild(xnCompName);

                XmlNode xnReportName = vdoc1.CreateElement("ReportName");
                xmltext = vdoc1.CreateTextNode("ST3");
                xnReportName.AppendChild(xmltext);
                xnClientInfo.AppendChild(xnReportName);

                XmlNode xnFINANCIALYEAR = vdoc1.CreateElement("FinancialYear");
                xmltext = vdoc1.CreateTextNode(this.cmbFYear.Text);
                xnFINANCIALYEAR.AppendChild(xmltext);
                xnClientInfo.AppendChild(xnFINANCIALYEAR);

                XmlNode xnRETURNPERIOD = vdoc1.CreateElement("ReturnPeriod");
                xmltext = vdoc1.CreateTextNode(this.cmbPeriod.Text);
                xnRETURNPERIOD.AppendChild(xmltext);
                xnClientInfo.AppendChild(xnRETURNPERIOD);

                XmlNode xnCRCValue = vdoc1.CreateElement("CRCValue");
                xmltext = vdoc1.CreateTextNode(this.crcValue);
                xnCRCValue.AppendChild(xmltext);
                xnClientInfo.AppendChild(xnCRCValue);


                XmlNode xnVersion = vdoc1.CreateElement("Version");
                xmltext = vdoc1.CreateTextNode("1.0.1.0");
                xnVersion.AppendChild(xmltext);
                xnClientInfo.AppendChild(xnVersion);

                XmlNode xneFileName = vdoc1.CreateElement("eFileName");
                xmltext = vdoc1.CreateTextNode(this.FileTag.Trim() + ".xml");
                xneFileName.AppendChild(xmltext);
                xnClientInfo.AppendChild(xneFileName);

                XmlNode xnRepoGenDate = vdoc1.CreateElement("RepoGenDate");
                xmltext = vdoc1.CreateTextNode(DateTime.Now.Date.Day.ToString() + "/" + DateTime.Now.Date.Month.ToString() + "/" + DateTime.Now.Date.Year.ToString());
                xnRepoGenDate.AppendChild(xmltext);
                xnClientInfo.AppendChild(xnRepoGenDate);
            
                this.lbl.Text = "Generating Company Details...";
                
                this.lbl.Text = this.crcFileName +" Company Details file Successfuly generated";
                this.mFwrite(this.crcFileName + " Company Details file Successfuly generated");
                vdoc1.Save(this.crcFileName);
                
              

            }
            catch (Exception e)
            {
                this.xmlfilegenerated  = false;
                this.mOnError(e);
                Application.Exit();
            }
            
           

            
        }

        private void mRefreshPGB(int pgbval)
        {
            this.pgb.Value = pgbval;
        }
        private string mAscii(string vVal)
        {
            Encoding ascii = Encoding.ASCII;
            Encoding unicode = Encoding.Unicode;
            // Convert the string into a byte[].
            byte[] unicodeBytes = unicode.GetBytes(vVal.Substring(4, 1).ToString());
            // Perform the conversion from one encoding to the other.
            byte[] asciiBytes = Encoding.Convert(unicode, ascii, unicodeBytes);
            string asciiString = asciiBytes[0].ToString();
            return asciiString;
        }
       
    }
}