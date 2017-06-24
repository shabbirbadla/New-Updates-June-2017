using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.ComponentModel;
using System.Xml;
using System.Xml.XPath;
using System.Globalization;

namespace Delhi
{
    public class cls_Delhi_DataTemplate
    {
         #region variable declaration
        SqlConnection sqlconn;
        string Sta_dt = string.Empty, End_dt = string.Empty;
        #endregion

        #region Database related & Other Properties
        private string connString;
        public string ConnString
        {
            get { return connString; }
            set { connString = value; }
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
        private string selectedTemplate;
        public string SelectedTemplate
        {
            get { return selectedTemplate; }
            set { selectedTemplate = value; }
        }
        #endregion

        public cls_Delhi_DataTemplate(string connectionString)
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

                m_DsFinYear = cls_Sqlhelper.ExecuteDataset(sqlconn, CommandType.Text, sqlstr);
                m_DsFinYear.Tables[0].TableName = "Co_mast";

                DataRow drow;

                drow = m_DsFinYear.Tables["Co_mast"].Rows[0];
                Sta_dt = Convert.ToDateTime(drow["sta_dt"]).ToString("dd/MM/yyyy");
                End_dt = Convert.ToDateTime(drow["end_dt"]).ToString("dd/MM/yyyy");

                FinYear = Sta_dt.Substring(Sta_dt.Length - 4, 4) + "-" + End_dt.Substring(End_dt.Length - 4, 4);
                FromDate = Sta_dt;
                ToDate = End_dt;
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
                BgWorkProgress.ReportProgress(5, "Opening DataBase Connection....");

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

                BgWorkProgress.ReportProgress(10, "Executing SQL Procedure....");

                string sqlstr = string.Empty;

                if (SelectedTemplate.Contains("16"))
                {
                    sqlstr = "EXECUTE USP_REP_CUST_DLVAT_16 '','','','" + FromDate + "','" + ToDate + "','','','','',0,0,'','','','','','','','','" + FinYear + "','" + CompId + "'";
                    if (SelectedTemplate == "DVAT 16 2A_DVAT 30_BILL")
                    {
                        sqlstr = sqlstr + " " + "select tran_date,purbill_date,inv_no,seller_tin_no,seller_name,type_of_pur,tran_type,form_type"
                            + ",goods_type,rate_of_tax,pur_amt,input_tax_paid,bill_amt,gr_no,gr_date,po_no,po_date,del_date,movnt_place_name"
                            + ",movnt_state,consign_goods_place_name,consign_goods_state"
                            + " from cust_dlvat_tbl_16"
                            + " where vat_type='DVAT 16 2A_DVAT 30_BILL'";
                    }
                    else if (SelectedTemplate == "DVAT 16 2B_DVAT 31")
                    {
                        sqlstr = sqlstr + " " + "select tran_date,inv_no,seller_tin_no,seller_name,tran_type"
                            + ",goods_type,form_type,type_of_pur,pur_amt,input_tax_paid,rate_of_tax,diesel_petrol_sales"
                            + ",chgs_civil,chgs_land_cost,sales_agnst_hform"
                            + " from cust_dlvat_tbl_16"
                            + " where vat_type='DVAT 16 2B_DVAT 31'";
                    }
                    else if (SelectedTemplate == "DVAT 16 2C2D")
                    {
                        sqlstr = sqlstr + " " + "select year_qtr,seller_tin_no,seller_name,tran_type"
                            + ",pur_amt,input_tax_paid,form_type"
                            + " from cust_dlvat_tbl_16"
                            + " where vat_type='DVAT 16 2C2D'";
                    }
                }
                else if (SelectedTemplate.Contains("17"))
                {
                    sqlstr = "EXECUTE USP_REP_CUST_DLVAT_17 '','','','" + FromDate + "','" + ToDate + "','','','','',0,0,'','','','','','','','','" + FinYear + "',''";
                    if (SelectedTemplate == "DVAT 17 2A")
                    {
                        sqlstr = sqlstr + " " + "select tran_date,seller_tin_no,seller_name,tran_type,form_type,pur_amt,rate_of_tax,tax_amt"
                            + " from cust_dlvat_tbl_17"
                            + " where vat_type='DVAT 17 2A'";
                    }
                    else if (SelectedTemplate == "DVAT 17 2B")
                    {
                        sqlstr = sqlstr + " " + "select tran_date,inv_no,seller_tin_no,seller_name,cate_of_cont,rate_of_comp,pur_amt,comp_tax"
                            + ",form_43_id,form_43_date,sales_price,rate_of_tax,output_tax"
                            + " from cust_dlvat_tbl_17"
                            + " where vat_type='DVAT 17 2B'";
                    }
                }

                m_DsSelect = cls_Sqlhelper.ExecuteDataset(ConnString, CommandType.Text, sqlstr);
                BgWorkProgress.ReportProgress(20, "Execution Completed....");

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
                    string tagName = string.Empty;

                    BgWorkProgress.ReportProgress(30, "XML File generation started...");

                    writer.WriteStartDocument();

                    #region Casewise Data
                    switch (SelectedTemplate)
                    {
                        #region DVAT 16 2A_DVAT 30_BILL
                        case "DVAT 16 2A_DVAT 30_BILL":
                            #region DVAT 16 2A_DVAT 30_BILL
                            //Sales
                            tagName = "DVAT_16_2A_DVAT_30_BILL";
                            BgWorkProgress.ReportProgress(50, "Creating " + tagName + " tag...");
                            writer.WriteStartElement(tagName);

                            rowCount = dTableToExport.Rows.Count;

                            if (rowCount > 0)
                            {
                                for (int i = 0; i < rowCount; i++)
                                {
                                    writer.WriteStartElement(tagName);
                                    writer.WriteElementString("Transaction_Date", !dTableToExport.Rows[i].ItemArray[0].ToString().Contains("1900") ? Convert.ToDateTime(dTableToExport.Rows[i].ItemArray[0]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture).Trim() : "");
                                    writer.WriteElementString("Date_of_Purchase", !dTableToExport.Rows[i].ItemArray[1].ToString().Contains("1900") ? Convert.ToDateTime(dTableToExport.Rows[i].ItemArray[1]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture).Trim() : "");
                                    writer.WriteElementString("Invoice_or_DebitNote_or_CreditNote_No", dTableToExport.Rows[i].ItemArray[2].ToString().Trim());
                                    writer.WriteElementString("Sellers_TIN ", dTableToExport.Rows[i].ItemArray[3].ToString().Trim());
                                    writer.WriteElementString("Sellers_Name", dTableToExport.Rows[i].ItemArray[4].ToString().Trim());
                                    writer.WriteElementString("Type_of_Purchase", dTableToExport.Rows[i].ItemArray[5].ToString().Trim());
                                    writer.WriteElementString("Transaction_Type", dTableToExport.Rows[i].ItemArray[6].ToString().Trim());
                                    writer.WriteElementString("Form_type_Inter_State", dTableToExport.Rows[i].ItemArray[7].ToString().Trim());
                                    writer.WriteElementString("Goods_Type", dTableToExport.Rows[i].ItemArray[8].ToString().Trim());
                                    writer.WriteElementString("Rate_of_Tax", dTableToExport.Rows[i].ItemArray[9].ToString().Trim() != "0.00" ? dTableToExport.Rows[i].ItemArray[9].ToString().Trim() : "");
                                    writer.WriteElementString("Purchase_Amount", dTableToExport.Rows[i].ItemArray[10].ToString().Trim() != "0.00" ? dTableToExport.Rows[i].ItemArray[10].ToString().Trim() : "");
                                    writer.WriteElementString("Input_Tax_Paid", dTableToExport.Rows[i].ItemArray[11].ToString().Trim() != "0.00" ? dTableToExport.Rows[i].ItemArray[11].ToString().Trim() : "");
                                    writer.WriteElementString("Bill_Amount", dTableToExport.Rows[i].ItemArray[12].ToString().Trim() != "0.00" ? dTableToExport.Rows[i].ItemArray[12].ToString().Trim() : "");
                                    writer.WriteElementString("GR_or_RR_No", dTableToExport.Rows[i].ItemArray[13].ToString().Trim());
                                    writer.WriteElementString("GR_or_RR_Date", !dTableToExport.Rows[i].ItemArray[14].ToString().Contains("1900") ? Convert.ToDateTime(dTableToExport.Rows[i].ItemArray[14]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture).Trim() : "");
                                    writer.WriteElementString("Purchase_Order_No", dTableToExport.Rows[i].ItemArray[15].ToString().Trim());
                                    writer.WriteElementString("Purchase_Order_Date", !dTableToExport.Rows[i].ItemArray[16].ToString().Contains("1900") ? Convert.ToDateTime(dTableToExport.Rows[i].ItemArray[16]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture).Trim() : "");
                                    writer.WriteElementString("Date_of_Delivery", !dTableToExport.Rows[i].ItemArray[17].ToString().Contains("1900") ? Convert.ToDateTime(dTableToExport.Rows[i].ItemArray[17]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture).Trim() : "");
                                    writer.WriteElementString("Name_of_place_in_which_movement_commenced", dTableToExport.Rows[i].ItemArray[18].ToString().Trim());
                                    writer.WriteElementString("State_in_which_movement_commenced_State_Code", dTableToExport.Rows[i].ItemArray[19].ToString().Trim());
                                    writer.WriteElementString("Name_of_place_in_which_the_goods_have_been_consigned", dTableToExport.Rows[i].ItemArray[20].ToString().Trim());
                                    writer.WriteElementString("State_in_which_the_goods_have_been_consigned_State_Code", dTableToExport.Rows[i].ItemArray[21].ToString().Trim());
                                    writer.WriteEndElement();
                                }
                            }
                            else
                            {
                                writer.WriteStartElement(tagName);
                                writer.WriteElementString("Transaction_Date", "");
                                writer.WriteElementString("Date_of_Purchase", "");
                                writer.WriteElementString("Invoice_or_DebitNote_or_CreditNote_No", "");
                                writer.WriteElementString("Sellers_TIN ", "");
                                writer.WriteElementString("Sellers_Name", "");
                                writer.WriteElementString("Type_of_Purchase", "");
                                writer.WriteElementString("Transaction_Type", "");
                                writer.WriteElementString("Form_type_Inter_State", "");
                                writer.WriteElementString("Goods_Type", "");
                                writer.WriteElementString("Rate_of_Tax", "");
                                writer.WriteElementString("Purchase_Amount", "");
                                writer.WriteElementString("Input_Tax_Paid", "");
                                writer.WriteElementString("Bill_Amount", "");
                                writer.WriteElementString("GR_or_RR_No", "");
                                writer.WriteElementString("GR_or_RR_Date", "");
                                writer.WriteElementString("Purchase_Order_No", "");
                                writer.WriteElementString("Purchase_Order_Date", "");
                                writer.WriteElementString("Date_of_Delivery", "");
                                writer.WriteElementString("Name_of_place_in_which_movement_commenced", "");
                                writer.WriteElementString("State_in_which_movement_commenced_State_Code", "");
                                writer.WriteElementString("Name_of_place_in_which_the_goods_have_been_consigned", "");
                                writer.WriteElementString("State_in_which_the_goods_have_been_consigned_State_Code", "");
                                writer.WriteEndElement();
                            }
                            #endregion

                            writer.WriteEndElement();
                            break;
                        #endregion

                        #region DVAT 16 2B_DVAT 31
                        case "DVAT 16 2B_DVAT 31":
                            #region DVAT 16 2B_DVAT 31
                            //Sales
                            tagName = "DVAT_16_2B_DVAT_31";
                            BgWorkProgress.ReportProgress(50, "Creating " + tagName + " tag...");
                            writer.WriteStartElement(tagName);

                            rowCount = dTableToExport.Rows.Count;

                            if (rowCount > 0)
                            {
                                for (int i = 0; i < rowCount; i++)
                                {
                                    writer.WriteStartElement(tagName);
                                    writer.WriteElementString("Date_of_sale_or_transfer", !dTableToExport.Rows[i].ItemArray[0].ToString().Contains("1900") ? Convert.ToDateTime(dTableToExport.Rows[i].ItemArray[0]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture).Trim() : "");
                                    writer.WriteElementString("Invoice_or_Delivery_note_No_or_Debit_note_or_Credit", dTableToExport.Rows[i].ItemArray[1].ToString().Trim());
                                    writer.WriteElementString("Buyers_TIN", dTableToExport.Rows[i].ItemArray[2].ToString().Trim());
                                    writer.WriteElementString("Buyers_Name", dTableToExport.Rows[i].ItemArray[3].ToString().Trim());
                                    writer.WriteElementString("Transaction_type", dTableToExport.Rows[i].ItemArray[4].ToString().Trim());
                                    writer.WriteElementString("Goods_Type", dTableToExport.Rows[i].ItemArray[5].ToString().Trim());
                                    writer.WriteElementString("Form_type", dTableToExport.Rows[i].ItemArray[6].ToString().Trim());
                                    writer.WriteElementString("Local_Sale_Type_of_Sale", dTableToExport.Rows[i].ItemArray[7].ToString().Trim());
                                    writer.WriteElementString("Sales_Price_Excluding_Tax", dTableToExport.Rows[i].ItemArray[8].ToString().Trim() != "0.00" ? dTableToExport.Rows[i].ItemArray[8].ToString().Trim() : "");
                                    writer.WriteElementString("Tax_Amount", dTableToExport.Rows[i].ItemArray[9].ToString().Trim() != "0.00" ? dTableToExport.Rows[i].ItemArray[9].ToString().Trim() : "");
                                    writer.WriteElementString("Rate_of_tax", dTableToExport.Rows[i].ItemArray[10].ToString().Trim() != "0.00" ? dTableToExport.Rows[i].ItemArray[10].ToString().Trim() : "");
                                    writer.WriteElementString("Sales_of_Diesel_and_Petrol_of_various_oil_marketing_company_in_Delhi", dTableToExport.Rows[i].ItemArray[11].ToString().Trim() != "0.00" ? dTableToExport.Rows[i].ItemArray[11].ToString().Trim() : "");
                                    writer.WriteElementString("Charges_towards_labour_services_and_civil_works_contracts", dTableToExport.Rows[i].ItemArray[12].ToString().Trim() != "0.00" ? dTableToExport.Rows[i].ItemArray[12].ToString().Trim() : "");
                                    writer.WriteElementString("Charges_towards_cost_of_land_services_civil_works_contracts", dTableToExport.Rows[i].ItemArray[13].ToString().Trim() != "0.00" ? dTableToExport.Rows[i].ItemArray[13].ToString().Trim() : "");
                                    writer.WriteElementString("Sales_Against_H_form_to_Delhi_dealer", dTableToExport.Rows[i].ItemArray[14].ToString().Trim() != "0.00" ? dTableToExport.Rows[i].ItemArray[14].ToString().Trim() : "");
                                    writer.WriteEndElement();
                                }
                            }
                            else
                            {
                                writer.WriteStartElement(tagName);
                                writer.WriteElementString("Date_of_sale_or_transfer", "");
                                writer.WriteElementString("Invoice_or_Delivery_note_No_or_Debit_note_or_Credit", "");
                                writer.WriteElementString("Buyers_TIN", "");
                                writer.WriteElementString("Buyers_Name", "");
                                writer.WriteElementString("Transaction_type", "");
                                writer.WriteElementString("Goods_Type", "");
                                writer.WriteElementString("Form_type", "");
                                writer.WriteElementString("Local_Sale_Type_of_Sale", "");
                                writer.WriteElementString("Sales_Price_Excluding_Tax", "");
                                writer.WriteElementString("Tax_Amount", "");
                                writer.WriteElementString("Rate_of_tax", "");
                                writer.WriteElementString("Sales_of_Diesel_and_Petrol_of_various_oil_marketing_company_in_Delhi", "");
                                writer.WriteElementString("Charges_towards_labour_services_and_civil_works_contracts", "");
                                writer.WriteElementString("Charges_towards_cost_of_land_services_civil_works_contracts", "");
                                writer.WriteElementString("Sales_Against_H_form_to_Delhi_dealer", "");
                                writer.WriteEndElement();
                            }
                            #endregion

                            writer.WriteEndElement();
                            break;
                        #endregion

                        #region DVAT 16 2C2D
                        case "DVAT 16 2C2D":
                            #region DVAT 16 2C2D
                            //Sales
                            tagName = "DVAT_16_2C2D";
                            BgWorkProgress.ReportProgress(50, "Creating " + tagName + " tag...");
                            writer.WriteStartElement(tagName);

                            rowCount = dTableToExport.Rows.Count;

                            if (rowCount > 0)
                            {
                                for (int i = 0; i < rowCount; i++)
                                {
                                    writer.WriteStartElement(tagName);
                                    writer.WriteElementString("Year_and_Quarter", dTableToExport.Rows[i].ItemArray[0].ToString().Trim());
                                    writer.WriteElementString("TIN", dTableToExport.Rows[i].ItemArray[1].ToString().Trim());
                                    writer.WriteElementString("Sellers_or_Buyers_Name", dTableToExport.Rows[i].ItemArray[2].ToString().Trim());
                                    writer.WriteElementString("Type_Debit_or_Credit_Note", dTableToExport.Rows[i].ItemArray[3].ToString().Trim());
                                    writer.WriteElementString("Turnover_Amount", dTableToExport.Rows[i].ItemArray[4].ToString().Trim() != "0.00" ? dTableToExport.Rows[i].ItemArray[4].ToString().Trim() : "");
                                    writer.WriteElementString("Tax_Amount", dTableToExport.Rows[i].ItemArray[5].ToString().Trim() != "0.00" ? dTableToExport.Rows[i].ItemArray[5].ToString().Trim() : "");
                                    writer.WriteElementString("Form_Type_Annexure_2C_or_2D", dTableToExport.Rows[i].ItemArray[6].ToString().Trim());
                                    writer.WriteEndElement();
                                }
                            }
                            else
                            {
                                writer.WriteStartElement(tagName);
                                writer.WriteElementString("Year_and_Quarter", "");
                                writer.WriteElementString("TIN", "");
                                writer.WriteElementString("Sellers_or_Buyers_Name", "");
                                writer.WriteElementString("Type_Debit_or_Credit_Note", "");
                                writer.WriteElementString("Turnover_Amount", "");
                                writer.WriteElementString("Tax_Amount", "");
                                writer.WriteElementString("Form_Type_Annexure_2C_or_2D", "");
                                writer.WriteEndElement();
                            }
                            #endregion

                            writer.WriteEndElement();
                            break;
                        #endregion

                        #region DVAT 17 2A
                        case "DVAT 17 2A":
                            #region DVAT 17 2A
                            //Sales
                            tagName = "DVAT_17_2A";
                            BgWorkProgress.ReportProgress(50, "Creating " + tagName + " tag...");
                            writer.WriteStartElement(tagName);

                            rowCount = dTableToExport.Rows.Count;

                            if (rowCount > 0)
                            {
                                for (int i = 0; i < rowCount; i++)
                                {
                                    writer.WriteStartElement(tagName);
                                    writer.WriteElementString("Transaction_Date", !dTableToExport.Rows[i].ItemArray[0].ToString().Contains("1900") ? Convert.ToDateTime(dTableToExport.Rows[i].ItemArray[0]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture).Trim() : "");
                                    writer.WriteElementString("Sellers_TIN", dTableToExport.Rows[i].ItemArray[1].ToString().Trim());
                                    writer.WriteElementString("Sellers_Name", dTableToExport.Rows[i].ItemArray[2].ToString().Trim());
                                    writer.WriteElementString("Transaction_type", dTableToExport.Rows[i].ItemArray[3].ToString().Trim());
                                    writer.WriteElementString("Form_type", dTableToExport.Rows[i].ItemArray[4].ToString().Trim());
                                    writer.WriteElementString("Purchase_Amount", dTableToExport.Rows[i].ItemArray[5].ToString().Trim() != "0.00" ? dTableToExport.Rows[i].ItemArray[5].ToString().Trim() : "");
                                    writer.WriteElementString("Rate_of_Tax", dTableToExport.Rows[i].ItemArray[6].ToString().Trim() != "0.00" ? dTableToExport.Rows[i].ItemArray[6].ToString().Trim() : "");
                                    writer.WriteElementString("Tax_Amount", dTableToExport.Rows[i].ItemArray[7].ToString().Trim() != "0.00" ? dTableToExport.Rows[i].ItemArray[7].ToString().Trim() : "");
                                    writer.WriteEndElement();
                                }
                            }
                            else
                            {
                                writer.WriteStartElement(tagName);
                                writer.WriteElementString("Transaction_Date", "");
                                writer.WriteElementString("Sellers_TIN", "");
                                writer.WriteElementString("Sellers_Name", "");
                                writer.WriteElementString("Transaction_type", "");
                                writer.WriteElementString("Form_type", "");
                                writer.WriteElementString("Purchase_Amount", "");
                                writer.WriteElementString("Rate_of_Tax", "");
                                writer.WriteElementString("Tax_Amount", "");
                                writer.WriteEndElement();
                            }
                            #endregion

                            writer.WriteEndElement();
                            break;
                        #endregion

                        #region DVAT 17 2B
                        case "DVAT 17 2B":
                            #region DVAT 17 2B
                            //Sales
                            tagName = "DVAT_17_2B";
                            BgWorkProgress.ReportProgress(50, "Creating " + tagName + " tag...");
                            writer.WriteStartElement(tagName);

                            rowCount = dTableToExport.Rows.Count;

                            if (rowCount > 0)
                            {
                                for (int i = 0; i < rowCount; i++)
                                {
                                    writer.WriteStartElement(tagName);
                                    writer.WriteElementString("Date_of_sale_or_transfer", !dTableToExport.Rows[i].ItemArray[0].ToString().Contains("1900") ? Convert.ToDateTime(dTableToExport.Rows[i].ItemArray[0]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture).Trim() : "");
                                    writer.WriteElementString("Invoice_or_Delivery_note_No._or_Debit_note_or_Credit", dTableToExport.Rows[i].ItemArray[1].ToString().Trim());
                                    writer.WriteElementString("Buyers_TIN", dTableToExport.Rows[i].ItemArray[2].ToString().Trim());
                                    writer.WriteElementString("Buyers_Name", dTableToExport.Rows[i].ItemArray[3].ToString().Trim());
                                    writer.WriteElementString("Category_of_Contract_If_Applicable_1A_1B_2A_2B_3A_3B", dTableToExport.Rows[i].ItemArray[4].ToString().Trim());
                                    writer.WriteElementString("Rate_of_Composition", dTableToExport.Rows[i].ItemArray[5].ToString().Trim() != "0.00" ? dTableToExport.Rows[i].ItemArray[5].ToString().Trim() : "");
                                    writer.WriteElementString("Turnover", dTableToExport.Rows[i].ItemArray[6].ToString().Trim() != "0.00" ? dTableToExport.Rows[i].ItemArray[6].ToString().Trim() : "");
                                    writer.WriteElementString("Composition_Tax", dTableToExport.Rows[i].ItemArray[7].ToString().Trim() != "0.00" ? dTableToExport.Rows[i].ItemArray[7].ToString().Trim() : "");
                                    writer.WriteElementString("Form_DVAT_43_ID_No.", dTableToExport.Rows[i].ItemArray[8].ToString().Trim());
                                    writer.WriteElementString("Form_DVAT_43_Date", !dTableToExport.Rows[i].ItemArray[9].ToString().Contains("1900") ? Convert.ToDateTime(dTableToExport.Rows[i].ItemArray[9]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture).Trim() : "");
                                    writer.WriteElementString("Sales_Price_Excluding_VAT", dTableToExport.Rows[i].ItemArray[10].ToString().Trim() != "0.00" ? dTableToExport.Rows[i].ItemArray[10].ToString().Trim() : "");
                                    writer.WriteElementString("Rate_of_Tax", dTableToExport.Rows[i].ItemArray[11].ToString().Trim() != "0.00" ? dTableToExport.Rows[i].ItemArray[11].ToString().Trim() : "");
                                    writer.WriteElementString("Output_Tax", dTableToExport.Rows[i].ItemArray[12].ToString().Trim() != "0.00" ? dTableToExport.Rows[i].ItemArray[12].ToString().Trim() : "");
                                    writer.WriteEndElement();
                                }
                            }
                            else
                            {
                                writer.WriteStartElement(tagName);
                                writer.WriteElementString("Date_of_sale_or_transfer", "");
                                writer.WriteElementString("Invoice_or_Delivery_note_No._or_Debit_note_or_Credit", "");
                                writer.WriteElementString("Buyers_TIN", "");
                                writer.WriteElementString("Buyers_Name", "");
                                writer.WriteElementString("Category_of_Contract_If_Applicable_1A_1B_2A_2B_3A_3B", "");
                                writer.WriteElementString("Rate_of_Composition", "");
                                writer.WriteElementString("Turnover", "");
                                writer.WriteElementString("Composition_Tax", "");
                                writer.WriteElementString("Form_DVAT_43_ID_No.", "");
                                writer.WriteElementString("Form_DVAT_43_Date", "");
                                writer.WriteElementString("Sales_Price_Excluding_VAT", "");
                                writer.WriteElementString("Rate_of_Tax", "");
                                writer.WriteElementString("Output_Tax", "");
                                writer.WriteEndElement();
                            }
                            #endregion

                            writer.WriteEndElement();
                            break;
                        #endregion
                    }
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
