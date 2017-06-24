using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Xml.XPath;
using System.Globalization;
using System.ComponentModel;

namespace Form_231
{
    public class cls_FORM231_DataTemplate
    {
        #region variable declaration
        SqlConnection sqlconn;
        string Sta_dt, End_dt, City = string.Empty;
        string Stax_no = string.Empty, Vcst_no = string.Empty, Stax_loc = string.Empty
            , Co_email = string.Empty, Co_phone = string.Empty, Cst_No = string.Empty
            , Vatpname = string.Empty, Vatpdesig = string.Empty, Vatpmobileno = string.Empty
            , Vatpemail = string.Empty, Vatpremarks = string.Empty, Vatpretdate = string.Empty;
        //bool FirstReturn, LastReturn = false;
        #endregion

        #region Database related & Other Properties
        private string connString;
        public string ConnString
        {
            get { return connString; }
            set { connString = value; }
        }        
        private DataRow drRow1;
        public DataRow DrRow1
        {
            get { return drRow1; }
            set { drRow1 = value; }
        }
        private DataSet dsMain;
        public DataSet DsMain
        {
            get { return dsMain; }
            set { dsMain = value; }
        }        
        private BackgroundWorker bgWorkProgress;
        public BackgroundWorker BgWorkProgress
        {
            get { return bgWorkProgress; }
            set { bgWorkProgress = value; }
        }
        #endregion

        #region Field Properties
        private string fromDate;
        public string FromDate
        {
            get { return fromDate; }
            set { fromDate = value; }
        }
        private string toDate;
        public string ToDate
        {
            get { return toDate; }
            set { toDate = value; }
        }
        private int compId;
        public int CompId
        {
            get { return compId; }
            set { compId = value; }
        }
        private string finYear;
        public string FinYear
        {
            get { return finYear; }
            set { finYear = value; }
        }
        private string return_Type;
        public string Return_Type
        {
            get { return return_Type; }
            set { return_Type = value; }
        }
        private string first_Return;
        public string First_Return
        {
            get { return first_Return; }
            set { first_Return = value; }
        }
        private string last_Return;
        public string Last_Return
        {
            get { return last_Return; }
            set { last_Return = value; }
        }
        private string isEligibleFor704;
        public string IsEligibleFor704
        {
            get { return isEligibleFor704; }
            set { isEligibleFor704 = value; }
        }
        private string periodicityOfReturn;
        public string PeriodicityOfReturn
        {
            get { return periodicityOfReturn; }
            set { periodicityOfReturn = value; }
        }
        private DateTime dateOfFilingRet;
        public DateTime DateOfFilingRet
        {
            get { return dateOfFilingRet; }
            set { dateOfFilingRet = value; }
        }
        private string remarks;
        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }
        #endregion

        public cls_FORM231_DataTemplate(string connectionString)
        {
            ConnString = connectionString;
        }

        #region Methods
        public void GetCompanyData()
        {
            try
            {
                if (sqlconn == null)
                {
                    sqlconn = new SqlConnection(ConnString);
                    sqlconn.Open();
                }
                else
                {
                    if (sqlconn.State == ConnectionState.Closed)
                        sqlconn.Open();
                }

                DataSet m_DsFinYear = new DataSet();

                string sqlstr = "Select * From Vudyog..Co_mast Where CompId=" + CompId;
                sqlstr = sqlstr + " " + "Select * From ManuFact";

                m_DsFinYear = cls_Sqlhelper.ExecuteDataset(sqlconn, CommandType.Text, sqlstr);
                m_DsFinYear.Tables[0].TableName = "Co_mast";
                m_DsFinYear.Tables[1].TableName = "ManuFact";

                DataRow drow;

                drow = m_DsFinYear.Tables["Co_mast"].Rows[0];

                Stax_no = drow["stax_no"].ToString().Trim();
                Co_email = drow["email"].ToString().Trim();
                Co_phone = drow["phone"].ToString().Trim();
                Sta_dt = Convert.ToDateTime(drow["sta_dt"]).ToString("dd/MM/yyyy");
                End_dt = Convert.ToDateTime(drow["end_dt"]).ToString("dd/MM/yyyy");
                
                City = drow["city"].ToString().Trim();
                Cst_No = drow["cst_no"].ToString().Trim();

                FinYear = Sta_dt.Substring(Sta_dt.Length - 4, 4) + "-" + End_dt.Substring(End_dt.Length - 4, 4);
                FromDate = Sta_dt;
                ToDate = End_dt;
                
                drow = m_DsFinYear.Tables["ManuFact"].Rows[0];
                Vcst_no = !string.IsNullOrEmpty(Cst_No) ? drow["vcst_no"].ToString().Trim() : "";
                Stax_loc = drow["stax_loc"].ToString().Trim();
                Vatpname = drow["vatpname"].ToString().Trim();
                Vatpdesig = drow["vatpdesig"].ToString().Trim();
                Vatpmobileno = drow["vatpmobileno"].ToString().Trim();
                Vatpemail = drow["vatpemail"].ToString().Trim();
                Vatpremarks = drow["vatpremarks"].ToString().Trim();
                Vatpretdate = drow["vatpretdate"].ToString().Trim();
                DateOfFilingRet = Convert.ToDateTime(Vatpretdate, CultureInfo.CurrentCulture);// DateTime.ParseExact(Vatpretdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                sqlconn.Close();
            }
        }

        public void GetProcData()
        {
            try
            {
                BgWorkProgress.ReportProgress(2, "Opening DataBase Connection....");

                if (sqlconn == null)
                {
                    sqlconn = new SqlConnection(ConnString);
                    sqlconn.Open();
                }
                else
                {
                    if (sqlconn.State == ConnectionState.Closed)
                        sqlconn.Open();
                }

                DataSet m_DsSelect = new DataSet();

                BgWorkProgress.ReportProgress(3, "Executing SQL Procedure....");

                string sqlstr = string.Empty;
                //sqlstr = "EXECUTE USP_REP_MHFORM231_TEMPLATE_MH '','','','" + FromDate + "','" + ToDate + "','','','','',0,0,'','','','','','','','','" + FinYear + "',''";
                sqlstr = "EXECUTE USP_REP_MHFORM231 '','','','" + FromDate + "','" + ToDate + "','','','','',0,0,'','','','','','','','','" + FinYear + "',''";

                m_DsSelect = cls_Sqlhelper.ExecuteDataset(ConnString, CommandType.Text, sqlstr);
                BgWorkProgress.ReportProgress(5, "Execution Completed....");

                DsMain = m_DsSelect;
                m_DsSelect.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                sqlconn.Close();
            }
        }

        public XmlTextWriter GenerateXML(DataTable dTableToExport, string xmlPath)
        {
            try
            {
                XmlDocument document = new XmlDocument();
                XPathNavigator navigator = document.CreateNavigator();
                XmlTextWriter writer;
                int rowCount = 0;
                using (writer = new XmlTextWriter(xmlPath.ToString().Trim(), Encoding.UTF8))
                {
                    DataRow[] dr1, dr2, dr3, dr4, dr5, dr6, dr7, dr8, dr9, dr10, dr11, dr12, dr13;
                    string tagName = string.Empty;
                    DateTime date;
                    string[] FDate, TDate;
                    string d1 = string.Empty, d2 = string.Empty;

                    BgWorkProgress.ReportProgress(8, "XML File generation started...");

                    writer.WriteStartDocument();
                    #region ST3
                    writer.WriteStartElement("MH_FORM_231");

                    #region Dealer Details
                    //Dealer Details
                    tagName = "Dealer_Details";
                    BgWorkProgress.ReportProgress(10, "Creating " + tagName + " tag...");
                    writer.WriteStartElement(tagName);
                    writer.WriteElementString("MVAT_RC_No", string.IsNullOrEmpty(Stax_no) ? "" : Stax_no);
                    writer.WriteElementString("If_Holding_CST_RC", string.IsNullOrEmpty(Vcst_no) ? "No" : "Yes");
                    writer.WriteElementString("Eligible_for_704", string.IsNullOrEmpty(IsEligibleFor704) ? "" : IsEligibleFor704); 
                    writer.WriteElementString("Location_of_Sales_Tax_Officer_having_jurisdiction_over_principal_place_of_business", string.IsNullOrEmpty(Stax_loc) ? "" : Stax_loc);
                    writer.WriteElementString("E_Mail_id", string.IsNullOrEmpty(Co_email) ? "" : Co_email);
                    writer.WriteElementString("Phone_No", string.IsNullOrEmpty(Co_phone) ? "" : Co_phone);
                    writer.WriteElementString("Type_of_Return", string.IsNullOrEmpty(Return_Type) ? "" : Return_Type);
                    writer.WriteElementString("Whether_First_return", string.IsNullOrEmpty(First_Return) ? "" : First_Return);
                    writer.WriteElementString("Periodicity_of_Return", string.IsNullOrEmpty(PeriodicityOfReturn) ? "" : PeriodicityOfReturn);
                    writer.WriteElementString("Whether_Last_Return", string.IsNullOrEmpty(Last_Return) ? "" : Last_Return);
                    FDate = FromDate.Split('/');
                    TDate = ToDate.Split('/');
                    d1 = FDate[1].ToString() + "/" + FDate[0].ToString() + "/" + FDate[2].ToString();
                    d2 = TDate[1].ToString() + "/" + TDate[0].ToString() + "/" + TDate[2].ToString();
                    writer.WriteElementString("Period_Covered_by_Return_From", d1.Length > 0 ? d1.ToString() : "");
                    writer.WriteElementString("Period_Covered_by_Return_To", d2.Length > 0 ? d2.ToString() : "");
                    writer.WriteEndElement();
                    #endregion

                    #region Net Turnover of Sales
                    //Net Turnover of Sales
                    DataTable dt1 =
                        dTableToExport.DefaultView.ToTable(true, "Partsr", "SrNo", "Amt1");

                    dr1 = dt1.Select("Partsr='5' and SrNo='A'");
                    dr2 = dt1.Select("Partsr='5' and SrNo='B'");
                    dr3 = dt1.Select("Partsr='5' and SrNo='C'");
                    dr4 = dt1.Select("Partsr='5' and SrNo='D'");
                    dr5 = dt1.Select("Partsr='5' and SrNo='DA'");
                    dr6 = dt1.Select("Partsr='5' and SrNo='DB'");
                    dr7 = dt1.Select("Partsr='5' and SrNo='E'");
                    dr8 = dt1.Select("Partsr='5' and SrNo='F'");
                    dr9 = dt1.Select("Partsr='5' and SrNo='G'");
                    dr10 = dt1.Select("Partsr='5' and SrNo='H'");
                    dr11 = dt1.Select("Partsr='5' and SrNo='I'");
                    dr12 = dt1.Select("Partsr='5' and SrNo='J'");
                    
                    tagName = "Net_Turnover_of_Sales";
                    BgWorkProgress.ReportProgress(10, "Creating " + tagName + " tag...");
                    writer.WriteStartElement(tagName);
                    writer.WriteElementString("Gross_turnover_of_Sales_5_a", dr1.Length > 0 ? (dr1[0].ItemArray[2].ToString().Trim() != "0.00" ? dr1[0].ItemArray[2].ToString().Trim() : "") : "");
                    writer.WriteElementString("Goods_returned_including_redcution_of_Sales_Price_on_account_of_Rate_Difference_and_rate_discount_5_b", dr2.Length > 0 ? (dr2[0].ItemArray[2].ToString().Trim() != "0.00" ? dr2[0].ItemArray[2].ToString().Trim() : "") : "");
                    writer.WriteElementString("Net_Tax_Amount_5_c", dr3.Length > 0 ? (dr3[0].ItemArray[2].ToString().Trim() != "0.00" ? dr3[0].ItemArray[2].ToString().Trim() : "") : "");
                    writer.WriteElementString("Branch_Transfer_or_Consignment_transfer_within_the_State_if_tax_paid_by_Agent_5_d", dr4.Length > 0 ? (dr4[0].ItemArray[2].ToString().Trim() != "0.00" ? dr4[0].ItemArray[2].ToString().Trim() : "") : "");
                    writer.WriteElementString("Sales_under_section_8_1_5_e_1", dr5.Length > 0 ? (dr5[0].ItemArray[2].ToString().Trim() != "0.00" ? dr5[0].ItemArray[2].ToString().Trim() : "") : "");
                    writer.WriteElementString("Turnover_of_Export_Sales_under_section_5_1_and_5_3_of_the_CST_act_1956_5_e_2", dr6.Length > 0 ? (dr6[0].ItemArray[2].ToString().Trim() != "0.00" ? dr6[0].ItemArray[2].ToString().Trim() : "") : "");
                    writer.WriteElementString("Turnover_of_Sales_in_course_of_import_under_section_5_2_of_the_CST_act_1956_5_e_3", dr7.Length > 0 ? (dr7[0].ItemArray[2].ToString().Trim() != "0.00" ? dr7[0].ItemArray[2].ToString().Trim() : "") : "");
                    writer.WriteElementString("Sales_of_Tax_Free_Goods_specified_in_Schedule_A_of_MVAT_5_f", dr8.Length > 0 ? (dr8[0].ItemArray[2].ToString().Trim() != "0.00" ? dr8[0].ItemArray[2].ToString().Trim() : "") : "");
                    writer.WriteElementString("Sales_of_Taxable_Goods_fully_exempted_under_section_41_and_under_section_8_other_than_under_section_8_1_5_g", dr9.Length > 0 ? (dr9[0].ItemArray[2].ToString().Trim() != "0.00" ? dr9[0].ItemArray[2].ToString().Trim() : "") : "");
                    writer.WriteElementString("Labour_Charges_or_job_Work_Charges_5_h", dr10.Length > 0 ? (dr10[0].ItemArray[2].ToString().Trim() != "0.00" ? dr10[0].ItemArray[2].ToString().Trim() : "") : "");
                    writer.WriteElementString("Other_Allowable_Deduction_in_Sales_turnover_5_i", dr11.Length > 0 ? (dr11[0].ItemArray[2].ToString().Trim() != "0.00" ? dr11[0].ItemArray[2].ToString().Trim() : "") : "");
                    writer.WriteElementString("Balance_Net_turnover_of_sales_liable_to_tax", dr12.Length > 0 ? (dr12[0].ItemArray[2].ToString().Trim() != "0.00" ? dr12[0].ItemArray[2].ToString().Trim() : "") : "");
                    writer.WriteEndElement();
                    #endregion

                    #region Computation_Sales Tax Payable
                    //Computation_Sales Tax Payable
                    dt1 = dTableToExport.DefaultView.ToTable(true, "Partsr", "SrNo", "Rate", "Amt1");

                    dr1 = dt1.Select("Partsr='6' and SrNo not in ('AA','DA','Z','CA','YA','ZA','BA','EA','EB')");
                    dr2 = dt1.Select("Partsr='6' and SrNo in ('Z')");
                    
                    rowCount = dt1.Select("Partsr='6' and SrNo not in ('AA','DA','Z','CA','YA','ZA','BA','EA','EB')").Length;

                    tagName = "Computation_Sales_Tax_Payable";
                    BgWorkProgress.ReportProgress(10, "Creating " + tagName + " tag...");
                    string srno = string.Empty;

                    if (dr1.Length > 0)
                    {
                        writer.WriteStartElement(tagName);
                        for (int i = 0; i < rowCount; i++)
                        {
                            //writer.WriteElementString("Sl_No", dr1.Length > 0 ? dr1[i].ItemArray[1].ToString().Trim().ToLower() + ")" : "");// Commented By Pankaj B. on 09-07-2014 for Bug-22610
                            writer.WriteElementString("Sl_No", dr1.Length > 0 ? Convert.ToChar(97 + i) + ")" : "");// Added By Pankaj B. on 09-07-2014 for Bug-22610
                            writer.WriteElementString("Rate_of_Tax", dr1.Length > 0 ? Convert.ToDecimal(string.Format("{0:0.00}", Convert.ToDecimal(dr1[i].ItemArray[2]))).ToString().Trim() : "");
                            writer.WriteElementString("turnover_of_Sales_liable_to_Tax", dr1.Length > 0 ? dr1[i].ItemArray[3].ToString().Trim() : "");
                        }
                        if (rowCount < 6)
                        {
                            for (int i = rowCount + 1; i <= 6; i++)
                            {
                                if (i == 2)
                                    srno = "b)";
                                else if (i == 3)
                                    srno = "c)";
                                else if (i == 4)
                                    srno = "d)";
                                else if (i == 5)
                                    srno = "e)";
                                else if (i == 6)
                                    srno = "f)";
                                writer.WriteElementString("Sl_No", srno.Length > 0 ? srno : "");
                                writer.WriteElementString("Rate_of_Tax", "");
                                writer.WriteElementString("turnover_of_Sales_liable_to_Tax", "");
                            }
                        }
                        writer.WriteElementString("Total", dr2.Length > 0 ? dr2[0].ItemArray[3].ToString().Trim() : "");
                        writer.WriteEndElement();
                    }
                    else
                    {
                        writer.WriteStartElement(tagName);
                        for (int i = 1; i <= 6; i++)
                        {
                            if (i == 1)
                                srno = "a)";
                            else if (i == 2)
                                srno = "b)";
                            else if (i == 3)
                                srno = "c)";
                            else if (i == 4)
                                srno = "d)";
                            else if (i == 5)
                                srno = "e)";
                            else if (i == 6)
                                srno = "f)";
                            writer.WriteElementString("Sl_No", srno.Length > 0 ? srno : "");
                            writer.WriteElementString("Rate_of_Tax", "");
                            writer.WriteElementString("turnover_of_Sales_liable_to_Tax", "");
                        }
                        writer.WriteElementString("Total", "");
                        writer.WriteEndElement();
                    }
                    #endregion

                    #region Purchases Eligible for Sett_Off
                    //Purchases Eligible for Sett_Off
                    dt1 = dTableToExport.DefaultView.ToTable(true, "Partsr", "SrNo", "Amt1");

                    dr1 = dt1.Select("Partsr='7' and SrNo='A'");
                    dr2 = dt1.Select("Partsr='7' and SrNo='B'");
                    dr3 = dt1.Select("Partsr='7' and SrNo='C'");
                    dr4 = dt1.Select("Partsr='7' and SrNo='D'");
                    dr5 = dt1.Select("Partsr='7' and SrNo='E'");
                    dr6 = dt1.Select("Partsr='7' and SrNo='E1'");
                    dr7 = dt1.Select("Partsr='7' and SrNo='F'");
                    dr8 = dt1.Select("Partsr='7' and SrNo='G'");
                    dr9 = dt1.Select("Partsr='7' and SrNo='H'");
                    dr10 = dt1.Select("Partsr='7' and SrNo='I'");
                    dr11 = dt1.Select("Partsr='7' and SrNo='J'");
                    dr12 = dt1.Select("Partsr='7' and SrNo='K'");
                    dr13 = dt1.Select("Partsr='7' and SrNo='L'");
                    

                    tagName = "Purchases_Eligible_for_Sett_Off";
                    BgWorkProgress.ReportProgress(10, "Creating " + tagName + " tag...");
                    writer.WriteStartElement(tagName);
                    writer.WriteElementString("Total_turnover_of_Purchase_7_a", dr1.Length > 0 ? (dr1[0].ItemArray[2].ToString().Trim().ToLower() != "0.00" ? dr1[0].ItemArray[2].ToString().Trim().ToLower() : "") : "");
                    writer.WriteElementString("Value_of_Goods_Returned_7_b", dr2.Length > 0 ? (dr2[0].ItemArray[2].ToString().Trim().ToLower() != "0.00" ? dr2[0].ItemArray[2].ToString().Trim().ToLower() : "") : "");
                    writer.WriteElementString("Direct_Imports_7_c", dr3.Length > 0 ? (dr3[0].ItemArray[2].ToString().Trim().ToLower() != "0.00" ? dr3[0].ItemArray[2].ToString().Trim().ToLower() : "") : "");
                    writer.WriteElementString("Import_high_Seas_Purchase_7_d", dr4.Length > 0 ? (dr4[0].ItemArray[2].ToString().Trim().ToLower() != "0.00" ? dr4[0].ItemArray[2].ToString().Trim().ToLower() : "")  : "");
                    writer.WriteElementString("Inter_State_Purchases_excluding_Form_H_7_e", dr5.Length > 0 ? (dr5[0].ItemArray[2].ToString().Trim().ToLower() != "0.00" ? dr5[0].ItemArray[2].ToString().Trim().ToLower() : "") : "");
                    writer.WriteElementString("Purchase_Against_Form_H_7_e_1", dr6.Length > 0 ? (dr6[0].ItemArray[2].ToString().Trim().ToLower() != "0.00" ? dr6[0].ItemArray[2].ToString().Trim().ToLower() : "") : "");
                    writer.WriteElementString("Inter_State_Branch_or_Consignment_transfer_Received_7_f", dr7.Length > 0 ? (dr7[0].ItemArray[2].ToString().Trim().ToLower() != "0.00" ? dr7[0].ItemArray[2].ToString().Trim().ToLower() : "") : "");
                    writer.WriteElementString("Within_the_State_Branch_or_Consignment_transfer_Received_where_tax_to_be_paid_by_an_Agent_7_g", dr8.Length > 0 ? (dr8[0].ItemArray[2].ToString().Trim().ToLower() != "0.00" ? dr8[0].ItemArray[2].ToString().Trim().ToLower() : "") : "");
                    writer.WriteElementString("Within_State_Purchases_from_unregistered_Delaer_7_h", dr9.Length > 0 ? (dr9[0].ItemArray[2].ToString().Trim().ToLower() != "0.00" ? dr9[0].ItemArray[2].ToString().Trim().ToLower() : "") : "");
                    writer.WriteElementString("Purchase_from_MVAT_Registered_Delaer_7_i", dr10.Length > 0 ? (dr10[0].ItemArray[2].ToString().Trim().ToLower() != "0.00" ? dr10[0].ItemArray[2].ToString().Trim().ToLower() : "") : "");
                    writer.WriteElementString("State_Purchase_fully_exempted_under_section_41_and_u_8_but_not_covered_under_section_8_7_j", dr11.Length > 0 ? (dr11[0].ItemArray[2].ToString().Trim().ToLower() != "0.00" ? dr11[0].ItemArray[2].ToString().Trim().ToLower() : "") : "");
                    writer.WriteElementString("within_the_State_purchase_of_Tax_Free_goods_specified_in_schedule_A_7_k", dr12.Length > 0 ? (dr12[0].ItemArray[2].ToString().Trim().ToLower() != "0.00" ? dr12[0].ItemArray[2].ToString().Trim().ToLower() : "") : "");
                    writer.WriteElementString("Other_Allowable_Deduction_in_Purchase_turnover_7_l", dr13.Length > 0 ? (dr13[0].ItemArray[2].ToString().Trim().ToLower() != "0.00" ? dr13[0].ItemArray[2].ToString().Trim().ToLower() : "") : "");
                    writer.WriteEndElement();
                    #endregion

                    #region Purchase Tax payable
                    //Purchase Tax payable
                    dt1 = dTableToExport.DefaultView.ToTable(true, "Partsr", "SrNo", "Rate", "Amt1");

                    // Commented By Pankaj B. on 09-07-2014 for Bug-22610 Start
                    //dr1 = dt1.Select("Partsr='10' and SrNo not in ('AA','DA','Z','CA','YA','ZA','BA','EA','EB')");
                    //rowCount = dt1.Select("Partsr='10' and SrNo not in ('AA','DA','Z','CA','YA','ZA','BA','EA','EB')").Length;
                    // Commented By Pankaj B. on 09-07-2014 for Bug-22610 End

                    // Added By Pankaj B. on 09-07-2014 for Bug-22610 Start
                    dr1 = dt1.Select("Partsr='7A' and SrNo<>'Z'");
                    rowCount = dt1.Select("Partsr='7A' and SrNo<>'Z'").Length;
                    // Added By Pankaj B. on 09-07-2014 for Bug-22610 End

                    tagName = "Purchase_Tax_payable";
                    BgWorkProgress.ReportProgress(10, "Creating " + tagName + " tag...");
                    if (dr1.Length > 0)
                    {
                        for (int i = 0; i < rowCount; i++)
                        {
                            writer.WriteStartElement(tagName);
                            //writer.WriteElementString("Sl_No", dr1.Length > 0 ? dr1[i].ItemArray[1].ToString().Trim().ToLower() + ")" : "");// Commented By Pankaj B. on 09-07-2014 for Bug-22610
                            writer.WriteElementString("Sl_No", dr1.Length > 0 ? Convert.ToChar(97 + i) + ")" : "");// Added By Pankaj B. on 09-07-2014 for Bug-22610
                            writer.WriteElementString("Rate_of_Tax", dr1.Length > 0 ? Convert.ToDecimal(string.Format("{0:0.00}", Convert.ToDecimal(dr1[i].ItemArray[2]))).ToString().Trim() : "");
                            writer.WriteElementString("Turnover_of_Purchases_liable_to_Tax", dr1.Length > 0 ? (dr1[i].ItemArray[3].ToString().Trim() != "0.00" ? dr1[i].ItemArray[3].ToString().Trim() : "") : "");
                            writer.WriteEndElement();
                        }
                        if (rowCount < 5)
                        {
                            for (int i = rowCount + 1; i <= 5; i++)
                            {
                                if (i == 2)
                                    srno = "b)";
                                else if (i == 3)
                                    srno = "c)";
                                else if (i == 4)
                                    srno = "d)";
                                else if (i == 5)
                                    srno = "e)";
                                writer.WriteStartElement(tagName);
                                writer.WriteElementString("Sl_No", srno.Length > 0 ? srno : "");
                                writer.WriteElementString("Rate_of_Tax", "");
                                writer.WriteElementString("Turnover_of_Purchases_liable_to_Tax", "");
                                writer.WriteEndElement();
                            }
                        }
                    }
                    else
                    {
                        for (int i = 1; i <= 5; i++)
                        {
                            if (i == 1)
                                srno = "a)";
                            else if (i == 2)
                                srno = "b)";
                            else if (i == 3)
                                srno = "c)";
                            else if (i == 4)
                                srno = "d)";
                            else if (i == 5)
                                srno = "e)";
                            writer.WriteStartElement(tagName);
                            writer.WriteElementString("Sl_No", srno.Length > 0 ? srno : "");
                            writer.WriteElementString("Rate_of_Tax", "");
                            writer.WriteElementString("Turnover_of_Purchases_liable_to_Tax", "");
                            writer.WriteEndElement();
                        }
                    }
                    #endregion

                    #region Tax Rate Breakup for SetOff
                    //Tax Rate Breakup for SetOff
                    dt1 = dTableToExport.DefaultView.ToTable(true, "Partsr", "SrNo", "Rate", "Amt1", "Amt2");

                    dr1 = dt1.Select("Partsr='8' and SrNo not in ('AA','DA','Z','CA','YA','ZA','BA','EA','EB')");

                    rowCount = dt1.Select("Partsr='8' and SrNo not in ('AA','DA','Z','CA','YA','ZA','BA','EA','EB')").Length;

                    tagName = "Tax_Rate_Breakup_for_SetOff";
                    BgWorkProgress.ReportProgress(10, "Creating " + tagName + " tag...");
                    if (dr1.Length > 0)
                    {
                        for (int i = 0; i < rowCount; i++)
                        {
                            writer.WriteStartElement(tagName);
                            //writer.WriteElementString("Sl_No", dr1.Length > 0 ? dr1[i].ItemArray[1].ToString().Trim().ToLower() + ")" : "");// Commented By Pankaj B. on 09-07-2014 for Bug-22610
                            writer.WriteElementString("Sl_No", dr1.Length > 0 ? Convert.ToChar(97 + i) + ")" : "");// Added By Pankaj B. on 09-07-2014 for Bug-22610
 
                            if (dr1.Length > 0)
                            {
                                /*if (Convert.ToInt32(dr1[i].ItemArray[2]) == 0 && dr1[i].ItemArray[1].ToString() == "A")
                                    writer.WriteElementString("Rate_of_Tax", "Marginal VAT");
                                else if (Convert.ToInt32(dr1[i].ItemArray[2]) != 0 && dr1[i].ItemArray[1].ToString() != "A")
                                    writer.WriteElementString("Rate_of_Tax", "");
                                else
                                    writer.WriteElementString("Rate_of_Tax", dr1.Length > 0 ? dr1[i].ItemArray[2].ToString() : "");*/
                                writer.WriteElementString("Rate_of_Tax", dr1.Length > 0 ? Convert.ToDecimal(string.Format("{0:0.00}", Convert.ToDecimal(dr1[i].ItemArray[2]))).ToString().Trim() : "");
                            }
                            writer.WriteElementString("Net_turnover_of_Purchases", dr1.Length > 0 ? (dr1[i].ItemArray[3].ToString().Trim() != "0.00" ? dr1[i].ItemArray[3].ToString().Trim() : "") : "");
                            writer.WriteElementString("Tax_Amount", dr1.Length > 0 ? (dr1[i].ItemArray[4].ToString().Trim() != "0.00" ? dr1[i].ItemArray[4].ToString().Trim() : "") : "");
                            writer.WriteEndElement();
                        }
                        if (rowCount < 5)
                        {
                            for (int i = rowCount + 1; i <= 5; i++)
                            {
                                if (i == 2)
                                    srno = "b)";
                                else if (i == 3)
                                    srno = "c)";
                                else if (i == 4)
                                    srno = "d)";
                                else if (i == 5)
                                    srno = "e)";
                                writer.WriteStartElement(tagName);
                                writer.WriteElementString("Sl_No", srno.Length > 0 ? srno : "");
                                writer.WriteElementString("Rate_of_Tax", "");
                                writer.WriteElementString("Net_turnover_of_Purchases", "");
                                writer.WriteElementString("Tax_Amount", "");
                                writer.WriteEndElement();
                            }
                        }
                    }
                    else
                    {
                        for (int i = rowCount + 1; i <= 5; i++)
                        {
                            if (i == 1)
                                srno = "a)";
                            else if (i == 2)
                                srno = "b)";
                            else if (i == 3)
                                srno = "c)";
                            else if (i == 4)
                                srno = "d)";
                            else if (i == 5)
                                srno = "e)";
                            writer.WriteStartElement(tagName);
                            writer.WriteElementString("Sl_No", srno.Length > 0 ? srno : "");
                            writer.WriteElementString("Rate_of_Tax", "");
                            writer.WriteElementString("Net_turnover_of_Purchases", "");
                            writer.WriteElementString("Tax_Amount", "");
                            writer.WriteEndElement();
                        }
                    }
                    #endregion

                    #region Set Off Claimed
                    //Set Off Claimed
                    dt1 = dTableToExport.DefaultView.ToTable(true, "Partsr", "SrNo", "Amt1", "Amt2");

                    dr1 = dt1.Select("Partsr='9' and SrNo='B'");
                    dr2 = dt1.Select("Partsr='9' and SrNo='BA'");
                    dr3 = dt1.Select("Partsr='9' and SrNo='C'");
                    dr4 = dt1.Select("Partsr='9' and SrNo='F'");
                    dr5 = dt1.Select("Partsr='9' and SrNo='G'");
                    //dr6 = dt1.Select("Partsr='9' and SrNo='G'");

                    tagName = "Set_Off_Claimed";
                    BgWorkProgress.ReportProgress(10, "Creating " + tagName + " tag...");
                    writer.WriteStartElement(tagName);
                    writer.WriteElementString("Purchase_Value_Reduction_in_the_Amount_of_Set_off_u_r_53_1_of_the_corresponding_Purchase_Price_of_Sch_C_D_and_E_goods_9_b_1", dr1.Length > 0 ? (dr1[0].ItemArray[2].ToString().Trim() != "0.00" ? dr1[0].ItemArray[2].ToString().Trim() : "") : "");
                    writer.WriteElementString("Tax_Amount_Reduction_in_the_Amount_of_Set_off_u_r_53_1_of_the_corresponding_Purchase_Price_of_Sch_C_D_and_E_goods_9_b_1", dr1.Length > 0 ? (dr1[0].ItemArray[3].ToString().Trim() != "0.00" ? dr1[0].ItemArray[3].ToString().Trim() : "") : "");
                    writer.WriteElementString("Purchase_Value_Reduction_in_the_Amount_of_Set_off_u_r_53_2_of_the_corresponding_Purchase_Price_of_Sch_B_goods_9_b_2", dr2.Length > 0 ? (dr2[0].ItemArray[2].ToString().Trim() != "0.00" ? dr2[0].ItemArray[2].ToString().Trim() : "") : "");
                    writer.WriteElementString("Tax_Amount_Reduction_in_the_Amount_of_Set_off_u_r_53_2_of_the_corresponding_Purchase_Price_of_Sch_B_goods_9_b_2", dr2.Length > 0 ? (dr2[0].ItemArray[3].ToString().Trim() != "0.00" ? dr2[0].ItemArray[3].ToString().Trim() : "") : "");
                    writer.WriteElementString("Purchase_Value_Reduction_in_the_Amount_of_Set_off_under_any_other_sub_Rule_of_Rule_53_9_c", dr3.Length > 0 ? (dr3[0].ItemArray[2].ToString().Trim() != "0.00" ? dr3[0].ItemArray[2].ToString().Trim() : "") : "");
                    writer.WriteElementString("Tax_Amount_Reduction_in_the_Amount_of_Set_off_under_any_other_sub_Rule_of_Rule_53_9_c", dr3.Length > 0 ? (dr3[0].ItemArray[3].ToString().Trim() != "0.00" ? dr3[0].ItemArray[3].ToString().Trim() : "") : "");
                    writer.WriteElementString("Tax_Amount_Adjustment_on_Account_off_Sett_off_claimed_short_in_earlier_Return_9_d", dr4.Length > 0 ? (dr4[0].ItemArray[3].ToString().Trim() != "0.00" ? dr4[0].ItemArray[3].ToString().Trim() : "") : "");
                    writer.WriteElementString("Tax_Amount_Adjustment_on_Account_of_Sett_off_claimed_Excess_in_earlier_Return_9_e", dr5.Length > 0 ? (dr5[0].ItemArray[3].ToString().Trim() != "0.00" ? dr5[0].ItemArray[3].ToString().Trim() : "") : "");
                    writer.WriteEndElement();
                    #endregion

                    #region Tax Payable and Return
                    //Tax Payable and Return
                    dt1 = dTableToExport.DefaultView.ToTable(true, "Partsr", "SrNo", "Amt1");

                    dr1 = dt1.Select("Partsr='10A' and SrNo='B'");
                    dr2 = dt1.Select("Partsr='10A' and SrNo='C'");
                    dr3 = dt1.Select("Partsr='10A' and SrNo='D'");
                    dr4 = dt1.Select("Partsr='10A' and SrNo='E'");
                    //dr5 = dt1.Select("Partsr='10A' and SrNo='E'");
                    dr6 = dt1.Select("Partsr='10A' and SrNo='F'");
                    dr7 = dt1.Select("Partsr='10B' and SrNo='B'");
                    dr8 = dt1.Select("Partsr='10B' and SrNo='C'");
                    dr9 = dt1.Select("Partsr='10B' and SrNo='D'");
                    dr10 = dt1.Select("Partsr='10B' and SrNo='F'");
                    dr11 = dt1.Select("Partsr='10B' and SrNo='F1'");
                    dr12 = dt1.Select("Partsr='10C' and SrNo='A'");

                    tagName = "Tax_Payable_and_Return";
                    BgWorkProgress.ReportProgress(10, "Creating " + tagName + " tag...");

                    writer.WriteStartElement(tagName);
                    writer.WriteElementString("Excess_credit_brought_forward_from_previous_return_10_A_b", dr1.Length > 0 ? (dr1[0].ItemArray[2].ToString().Trim() != "0.00" ? dr1[0].ItemArray[2].ToString().Trim() : "") : "");
                    writer.WriteElementString("Amount_already_Paid_10_A_c", dr2.Length > 0 ? (dr2[0].ItemArray[2].ToString().Trim() != "0.00" ? dr2[0].ItemArray[2].ToString().Trim() : "") : "");
                    writer.WriteElementString("Excess_credit_as_per_Form_234_to_be_adjusted_against_the_liability_as_per_Form_231_10_A_d", dr3.Length > 0 ? (dr3[0].ItemArray[2].ToString().Trim() != "0.00" ? dr3[0].ItemArray[2].ToString().Trim() : "") : "");
                    writer.WriteElementString("Adjustment_of_ET_paid_under_Maharashtra_Tax_Entry_of_Goods_10_A_e", dr4.Length > 0 ? (dr4[0].ItemArray[2].ToString().Trim() != "0.00" ? dr4[0].ItemArray[2].ToString().Trim() : "") : "");
                    //pending
                    writer.WriteElementString("Amount_of_Tax_Collect_at_Source_under_section_31A_10_A_e1", "");//dr1.Length > 0 ? dr1[0].ItemArray[2].ToString().Trim() : "");
                    //pending
                    writer.WriteElementString("Refund_Adjustment_Order_No_10_A_f", dr6.Length > 0 ? (dr6[0].ItemArray[2].ToString().Trim() != "0.00" ? dr6[0].ItemArray[2].ToString().Trim() : "") : "");
                    writer.WriteElementString("Adjustment_on_account_of_MVAT_payable_if_any_as_per_Return_Form_234_against_the_excess_credit_as_per_Form_231_10_B_b", dr7.Length > 0 ? (dr7[0].ItemArray[2].ToString().Trim() != "0.00" ? dr7[0].ItemArray[2].ToString().Trim() : "") : "");
                    writer.WriteElementString("Adjustment_on_account_of_CST_payable_as_per_return_for_this_period_10_B_c", dr8.Length > 0 ? (dr8[0].ItemArray[2].ToString().Trim() != "0.00" ? dr8[0].ItemArray[2].ToString().Trim() : "") : "");
                    writer.WriteElementString("Adjustment_on_account_of_ET_payable_under_Maharashtra_tax_on_Entry_of_Goods_10_b_d", dr9.Length > 0 ? (dr9[0].ItemArray[2].ToString().Trim() != "0.00" ? dr9[0].ItemArray[2].ToString().Trim() : "") : "");
                    writer.WriteElementString("Interest_Payable_10_b_f", dr10.Length > 0 ? (dr10[0].ItemArray[2].ToString().Trim() != "0.00" ? dr10[0].ItemArray[2].ToString().Trim() : "") : "");
                    writer.WriteElementString("Late_Fee_Payable_10_b_f1", dr11.Length > 0 ? (dr11[0].ItemArray[2].ToString().Trim() != "0.00" ? dr11[0].ItemArray[2].ToString().Trim() : "") : "");
                    writer.WriteElementString("Excess_credit_carried_forward_to_subsequent_tax_period_10_c_a", dr12.Length > 0 ? (dr12[0].ItemArray[2].ToString().Trim() != "0.00" ? dr12[0].ItemArray[2].ToString().Trim() : "") : "");
                    writer.WriteEndElement();

                    for (int i = 2; i <= 11; i++)
                    {
                        writer.WriteStartElement(tagName);
                        writer.WriteElementString("Excess_credit_brought_forward_from_previous_return_10_A_b", "");
                        writer.WriteElementString("Amount_already_Paid_10_A_c", "");
                        writer.WriteElementString("Excess_credit_as_per_Form_234_to_be_adjusted_against_the_liability_as_per_Form_231_10_A_d", "");
                        writer.WriteElementString("Adjustment_of_ET_paid_under_Maharashtra_Tax_Entry_of_Goods_10_A_e", "");
                        writer.WriteElementString("Amount_of_Tax_Collect_at_Source_under_section_31A_10_A_e1", "");
                        writer.WriteElementString("Refund_Adjustment_Order_No_10_A_f", "");
                        writer.WriteElementString("Adjustment_on_account_of_MVAT_payable_if_any_as_per_Return_Form_234_against_the_excess_credit_as_per_Form_231_10_B_b", "");
                        writer.WriteElementString("Adjustment_on_account_of_CST_payable_as_per_return_for_this_period_10_B_c", "");
                        writer.WriteElementString("Adjustment_on_account_of_ET_payable_under_Maharashtra_tax_on_Entry_of_Goods_10_b_d", "");
                        writer.WriteElementString("Interest_Payable_10_b_f", "");
                        writer.WriteElementString("Late_Fee_Payable_10_b_f1", "");
                        writer.WriteElementString("Excess_credit_carried_forward_to_subsequent_tax_period_10_c_a", "");
                        writer.WriteEndElement();
                    }
                    #endregion

                    #region Challan Details
                    //Challan Details
                    dt1 = dTableToExport.DefaultView.ToTable(true, "Partsr", "SrNo", "Inv_no", "Amt1", "Date", "Party_nm", "Address");

                    dr1 = dt1.Select("Partsr='10E' and SrNo not in ('Z')");

                    rowCount = dt1.Select("Partsr='10E' and SrNo not in ('Z')").Length;

                    tagName = "Challan_Details";
                    BgWorkProgress.ReportProgress(10, "Creating " + tagName + " tag...");
                    if (dr1.Length > 0)
                    {
                        for (int i = 0; i < rowCount; i++)
                        {
                            writer.WriteStartElement(tagName);
                            writer.WriteElementString("SL_No", (i + 1).ToString().Trim());
                            writer.WriteElementString("Challan_CIN_No", dr1.Length > 0 ? dr1[i].ItemArray[2].ToString().Trim() : "");
                            writer.WriteElementString("Amount", dr1.Length > 0 ? (dr1[i].ItemArray[3].ToString().Trim() != "0.00" ? dr1[i].ItemArray[3].ToString().Trim() : "") : "");

                            date = dr1.Length > 0 ? Convert.ToDateTime(dr1[i].ItemArray[4].ToString().Trim()) : Convert.ToDateTime("");
                            writer.WriteElementString("Payment_Date", date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture).Trim());

                            writer.WriteElementString("Name_of_The_Bank", dr1.Length > 0 ? dr1[i].ItemArray[5].ToString().Trim() : "");
                            writer.WriteElementString("Branch_Name", dr1.Length > 0 ? dr1[i].ItemArray[6].ToString().Trim() : "");
                            writer.WriteEndElement();
                        }
                        
                    }
                    else
                    {
                        writer.WriteStartElement(tagName);
                        writer.WriteElementString("SL_No", "");
                        writer.WriteElementString("Challan_CIN_No", "");
                        writer.WriteElementString("Amount", "");
                        writer.WriteElementString("Payment_Date", "");
                        writer.WriteElementString("Name_of_The_Bank", "");
                        writer.WriteElementString("Branch_Name", "");
                        writer.WriteEndElement();
                    }
                    #endregion

                    #region Details of the RAO
                    //Details of the RAO
                    dt1 = dTableToExport.DefaultView.ToTable(true, "Partsr", "SrNo", "RAOSNO", "Amt1", "DATE");

                    dr1 = dt1.Select("Partsr='10F' and SrNo not in ('Z')");

                    rowCount = dt1.Select("Partsr='10F' and SrNo not in ('Z')").Length;

                    tagName = "Details_of_the_RAO";
                    BgWorkProgress.ReportProgress(10, "Creating " + tagName + " tag...");
                    if (dr1.Length > 0)
                    {
                        for (int i = 0; i < rowCount; i++)
                        {
                            writer.WriteStartElement(tagName);
                            writer.WriteElementString("SL_No", (i + 1).ToString().Trim());
                            writer.WriteElementString("RAO_No", dr1.Length > 0 ? dr1[i].ItemArray[2].ToString().Trim() : "");
                            writer.WriteElementString("Amount", dr1.Length > 0 ? (dr1[i].ItemArray[3].ToString().Trim() != "0.00" ? dr1[i].ItemArray[3].ToString().Trim() : "") : "");
                            
                            date = dr1.Length > 0 ? Convert.ToDateTime(dr1[i].ItemArray[4].ToString().Trim()) : Convert.ToDateTime("");
                            writer.WriteElementString("Date_of_RAO", date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture).Trim());
                            writer.WriteEndElement();
                        }
                        if (rowCount < 5)
                        {
                            for (int i = rowCount + 1; i <= 5; i++)
                            {
                                if (i == 2)
                                    srno = "2";
                                else if (i == 3)
                                    srno = "3";
                                else if (i == 4)
                                    srno = "4";
                                else if (i == 5)
                                    srno = "5";
                                writer.WriteStartElement(tagName);
                                writer.WriteElementString("SL_No", srno.Length > 0 ? srno : "");
                                writer.WriteElementString("RAO_No", srno.Length > 0 ? srno : "");
                                writer.WriteElementString("Amount", "");

                                date = DateTime.Now;
                                writer.WriteElementString("Date_of_RAO", date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture).Trim());
                                writer.WriteEndElement();
                            }
                        }
                    }
                    else
                    {
                        for (int i = rowCount + 1; i <= 5; i++)
                        {
                            if (i == 1)
                                srno = "1";
                            else if (i == 2)
                                srno = "2";
                            else if (i == 3)
                                srno = "3";
                            else if (i == 4)
                                srno = "4";
                            else if (i == 5)
                                srno = "5";
                            writer.WriteStartElement(tagName);
                            writer.WriteElementString("SL_No", srno.Length > 0 ? srno : "");
                            writer.WriteElementString("RAO_No", srno.Length > 0 ? srno : "");
                            writer.WriteElementString("Amount", "");
                            writer.WriteElementString("Date_of_RAO", DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture).Trim());
                            writer.WriteEndElement();
                        }
                    }
                    #endregion

                    #region Declaration
                    //Declaration
                    tagName = "Declaration";
                    BgWorkProgress.ReportProgress(10, "Creating " + tagName + " tag...");
                    writer.WriteStartElement(tagName);
                    writer.WriteElementString("Date_of_filing_return", DateOfFilingRet.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture).Trim());
                    writer.WriteElementString("Place", string.IsNullOrEmpty(City) ? "" : City);
                    writer.WriteElementString("Name_of_Authorized_Person", string.IsNullOrEmpty(Vatpname) ? "" : Vatpname);
                    writer.WriteElementString("Designation", string.IsNullOrEmpty(Vatpdesig) ? "" : Vatpdesig);
                    writer.WriteElementString("Mobile_No", string.IsNullOrEmpty(Vatpmobileno) ? "" : Vatpmobileno);
                    writer.WriteElementString("Email_id", string.IsNullOrEmpty(Vatpemail) ? "" : Vatpemail);
                    writer.WriteElementString("Remarks", string.IsNullOrEmpty(Vatpremarks) ? "" : Vatpremarks);
                    writer.WriteEndElement();
                    #endregion

                    writer.WriteEndElement();
                    #endregion

                    writer.WriteEndDocument();
                    writer.Close();
                }
                return writer;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
