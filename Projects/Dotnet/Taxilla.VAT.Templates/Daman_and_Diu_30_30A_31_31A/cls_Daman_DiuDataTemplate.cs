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

namespace Daman_and_Diu_30_30A_31_31A
{
    public class cls_Daman_DiuDataTemplate
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

        public cls_Daman_DiuDataTemplate(string connectionString)
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

                /*sqlstr = "EXECUTE USP_REP_CUST_DND_VAT_30 '','','','" + FromDate + "','" + ToDate + "','','','','',0,0,'','','','','','','','','" + FinYear + "',''";
                sqlstr = sqlstr + " " + "EXECUTE USP_REP_CUST_DND_VAT_31 '','','','" + FromDate + "','" + ToDate + "','','','','',0,0,'','','','','','','','','" + FinYear + "',''";
                */
                /*sqlstr = sqlstr + " " + "select tran_date,rtrim(tran_no) as tran_no,tran_cate,tran_type,rtrim(seller_nm) as seller_nm,rtrim(seller_tin_no) as seller_tin_no"
                    + ",rtrim(it_desc) as it_desc,vat_per,amt,tax_amt"
                    + " from cust_dnd_vat_30_tbl"
                    + " where vat_type='DamanVAT30'"
                    + " order by tran_no";

                sqlstr = sqlstr + " " + "select tran_date,rtrim(tran_no) as tran_no,tran_cate,tran_type,rtrim(seller_nm) as seller_nm,rtrim(seller_addr) as seller_addr"
                    + ",rtrim(dest_state) as dest_state,rtrim(seller_tin_no) as seller_tin_no,rtrim(it_desc) as it_desc,vat_per,amt"
                    + " from cust_dnd_vat_30_tbl"
                    + " where vat_type='DamanVAT30A'"
                    + " order by tran_no";

                sqlstr = sqlstr + " " + "select tran_date,tran_cate,rtrim(tran_no) as tran_no,rtrim(buyer_nm) as buyer_nm"
                    + ",rtrim(buyer_tin_no) as buyer_tin_no,rtrim(it_desc) as it_desc,sale_type,amt,vat_per,tax_amt"
                    + " from cust_dnd_vat_31_tbl"
                    + " where vat_type='DamanVAT31'"
                    + " order by tran_no";

                sqlstr = sqlstr + " " + "select tran_date,tran_cate,rtrim(tran_no) as tran_no,rtrim(buyer_nm) as buyer_nm"
                    + ",rtrim(buyer_tin_no) as buyer_tin_no,rtrim(dest_state) as dest_state,rtrim(it_desc) as it_desc,sale_type,amt,vat_per,tax_amt"
                    + " from cust_dnd_vat_31_tbl"
                    + " where vat_type='DamanVAT31A'"
                    + " order by tran_no";
                */

                if (SelectedTemplate.Contains("30"))
                {
                    //sqlstr = "EXECUTE USP_REP_CUST_DND_VAT_30 '','','','" + FromDate + "','" + ToDate + "','','','','',0,0,'','','','','','','','','" + FinYear + "',''"; // Commented By Pankaj B on 26-08-2014 for Bug-22611
                    sqlstr = "EXECUTE USP_REP_CUST_DND_VAT_30 '','','','" + FromDate + "','" + ToDate + "','','','','',0,0,'','','','','','','','','" + FinYear + "','" + compId + "'"; // Added By Pankaj B on 26-08-2014 for Bug-22611
                    if (SelectedTemplate == "DAMAN VAT 30")
                    {
                        sqlstr = sqlstr + " " + "select tran_date,rtrim(tran_no) as tran_no,tran_cate,tran_type,rtrim(seller_nm) as seller_nm,rtrim(seller_tin_no) as seller_tin_no"
                            + ",rtrim(it_desc) as it_desc,vat_per,amt,tax_amt"
                            + " from cust_dnd_vat_30_tbl"
                            + " where vat_type='DamanVAT30'"
                            + " order by tran_no";
                    }
                    else if (SelectedTemplate == "DAMAN VAT 30A")
                    {
                        sqlstr = sqlstr + " " + "select tran_date,rtrim(tran_no) as tran_no,tran_cate,tran_type,rtrim(seller_nm) as seller_nm,rtrim(seller_addr) as seller_addr"
                            + ",rtrim(dest_state) as dest_state,rtrim(seller_tin_no) as seller_tin_no,rtrim(it_desc) as it_desc,vat_per,amt"
                            + " from cust_dnd_vat_30_tbl"
                            + " where vat_type='DamanVAT30A'"
                            + " order by tran_no";
                    }
                }
                else if (SelectedTemplate.Contains("31"))
                {
                    sqlstr = "EXECUTE USP_REP_CUST_DND_VAT_31 '','','','" + FromDate + "','" + ToDate + "','','','','',0,0,'','','','','','','','','" + FinYear + "',''";
                    if (SelectedTemplate == "DAMAN VAT 31")
                    {
                        sqlstr = sqlstr + " " + "select tran_date,tran_cate,rtrim(tran_no) as tran_no,rtrim(buyer_nm) as buyer_nm"
                            + ",rtrim(buyer_tin_no) as buyer_tin_no,rtrim(it_desc) as it_desc,sale_type,amt,vat_per,tax_amt"
                            + " from cust_dnd_vat_31_tbl"
                            + " where vat_type='DamanVAT31'"
                            + " order by tran_no";
                    }
                    else if (SelectedTemplate == "DAMAN VAT 31A")
                    {
                        sqlstr = sqlstr + " " + "select tran_date,tran_cate,rtrim(tran_no) as tran_no,rtrim(buyer_nm) as buyer_nm"
                            + ",rtrim(buyer_tin_no) as buyer_tin_no,rtrim(dest_state) as dest_state,rtrim(it_desc) as it_desc,sale_type,amt,vat_per,tax_amt"
                            + " from cust_dnd_vat_31_tbl"
                            + " where vat_type='DamanVAT31A'"
                            + " order by tran_no";
                    }
                }

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
                    DataRow[] dr1;
                    string tagName = string.Empty;
                    //DateTime date;
                    //string[] FDate, TDate;
                    //string d1 = string.Empty, d2 = string.Empty;

                    BgWorkProgress.ReportProgress(8, "XML File generation started...");

                    writer.WriteStartDocument();

                    #region Single Data File
                    /*writer.WriteStartElement("DVAT");
                    
                    #region DVAT30
                    writer.WriteStartElement("DVAT30");
                    #region Data
                    //Data
                    dr1 = dSetToExport.Tables[0].Select("vat_type='DamanVAT30'", "tran_no asc");
                    tagName = "Data";
                    BgWorkProgress.ReportProgress(10, "Creating " + tagName + " tag...");
                    rowCount = dr1.Length;

                    if (rowCount > 0)
                    {
                        for (int i = 0; i < rowCount; i++)
                        {
                            writer.WriteStartElement(tagName);
                            writer.WriteElementString("Transaction_Date", dr1.Length > 0 ? Convert.ToDateTime(dr1[i].ItemArray[1]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture).Trim() : "");
                            writer.WriteElementString("Transaction_No", dr1.Length > 0 ? dr1[i].ItemArray[2].ToString().Trim() : "");
                            writer.WriteElementString("Category_of_Transaction", dr1.Length > 0 ? dr1[i].ItemArray[3].ToString().Trim() : "");
                            writer.WriteElementString("Transaction_Type", dr1.Length > 0 ? dr1[i].ItemArray[4].ToString().Trim() : "");
                            writer.WriteElementString("Seller_Name", dr1.Length > 0 ? dr1[i].ItemArray[5].ToString().Trim() : "");
                            writer.WriteElementString("Seller_TIN_Number", dr1.Length > 0 ? dr1[i].ItemArray[8].ToString().Trim() : "");
                            writer.WriteElementString("Short_Description_of_Goods", dr1.Length > 0 ? dr1[i].ItemArray[9].ToString().Trim() : "");
                            writer.WriteElementString("VAT_Percentage", dr1.Length > 0 ? dr1[i].ItemArray[10].ToString().Trim() : "");
                            writer.WriteElementString("Taxable_Amount", dr1.Length > 0 ? dr1[i].ItemArray[11].ToString().Trim() : "");
                            //writer.WriteElementString("Purchases_Amount", dr1.Length > 0 ? dr1[i].ItemArray[11].ToString().Trim() : "");
                            writer.WriteElementString("Tax_Amount", dr1.Length > 0 ? dr1[i].ItemArray[12].ToString().Trim() : "");
                            writer.WriteEndElement();
                        }
                    }
                    else
                    {
                        writer.WriteStartElement(tagName);
                        writer.WriteElementString("Transaction_Date", "");
                        writer.WriteElementString("Transaction_No", "");
                        writer.WriteElementString("Category_of_Transaction", "");
                        writer.WriteElementString("Transaction_Type", "");
                        writer.WriteElementString("Seller_Name", "");
                        writer.WriteElementString("Seller_TIN_Number", "");
                        writer.WriteElementString("Short_Description_of_Goods", "");
                        writer.WriteElementString("VAT_Percentage", "");
                        writer.WriteElementString("Taxable_Amount", "");
                        //writer.WriteElementString("Purchases_Amount", "");
                        writer.WriteElementString("Tax_Amount", "");
                        writer.WriteEndElement();
                    }
                    #endregion

                    writer.WriteEndElement();
                    #endregion

                    #region DVAT30A
                    writer.WriteStartElement("DVAT30A");
                    #region Data
                    //Data
                    dr1 = dSetToExport.Tables[0].Select("vat_type='DamanVAT30A'", "tran_no asc");
                    tagName = "Data";
                    BgWorkProgress.ReportProgress(10, "Creating " + tagName + " tag...");
                    rowCount = dr1.Length;

                    if (rowCount > 0)
                    {
                        for (int i = 0; i < rowCount; i++)
                        {
                            writer.WriteStartElement(tagName);
                            writer.WriteElementString("Transaction_Date", dr1.Length > 0 ? Convert.ToDateTime(dr1[i].ItemArray[1]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture).Trim() : "");
                            writer.WriteElementString("Transaction_No", dr1.Length > 0 ? dr1[i].ItemArray[2].ToString().Trim() : "");
                            writer.WriteElementString("Category_of_Transaction", dr1.Length > 0 ? dr1[i].ItemArray[3].ToString().Trim() : "");
                            writer.WriteElementString("Transaction_Type", dr1.Length > 0 ? dr1[i].ItemArray[4].ToString().Trim() : "");
                            writer.WriteElementString("Seller_Name", dr1.Length > 0 ? dr1[i].ItemArray[5].ToString().Trim() : "");
                            writer.WriteElementString("Seller_Address", dr1.Length > 0 ? dr1[i].ItemArray[6].ToString().Trim() : "");
                            writer.WriteElementString("Destination_Name_of_the_State", dr1.Length > 0 ? dr1[i].ItemArray[7].ToString().Trim() : "");
                            writer.WriteElementString("Seller_TIN_Number", dr1.Length > 0 ? dr1[i].ItemArray[8].ToString().Trim() : "");
                            writer.WriteElementString("Short_Description_of_Goods", dr1.Length > 0 ? dr1[i].ItemArray[9].ToString().Trim() : "");
                            writer.WriteElementString("Tax_Percentage", dr1.Length > 0 ? dr1[i].ItemArray[10].ToString().Trim() : "");
                            writer.WriteElementString("Taxable_Amount", dr1.Length > 0 ? dr1[i].ItemArray[11].ToString().Trim() : "");
                            writer.WriteEndElement();
                        }
                    }
                    else
                    {
                        writer.WriteStartElement(tagName);
                        writer.WriteElementString("Transaction_Date", "");
                        writer.WriteElementString("Transaction_No", "");
                        writer.WriteElementString("Category_of_Transaction", "");
                        writer.WriteElementString("Transaction_Type", "");
                        writer.WriteElementString("Seller_Name", "");
                        writer.WriteElementString("Seller_Address", "");
                        writer.WriteElementString("Destination_Name_of_the_State", "");
                        writer.WriteElementString("Seller_TIN_Number", "");
                        writer.WriteElementString("Short_Description_of_Goods", "");
                        writer.WriteElementString("Tax_Percentage", "");
                        writer.WriteElementString("Taxable_Amount", "");
                        writer.WriteEndElement();
                    }
                    #endregion

                    writer.WriteEndElement();
                    #endregion

                    #region DVAT31
                    writer.WriteStartElement("DVAT31");
                    #region Data
                    //Data
                    dr1 = dSetToExport.Tables[1].Select("vat_type='DamanVAT31'", "tran_no asc");
                    tagName = "Data";
                    BgWorkProgress.ReportProgress(10, "Creating " + tagName + " tag...");
                    rowCount = dr1.Length;

                    if (rowCount > 0)
                    {
                        for (int i = 0; i < rowCount; i++)
                        {
                            writer.WriteStartElement(tagName);
                            writer.WriteElementString("Date_of_Transaction", dr1.Length > 0 ? Convert.ToDateTime(dr1[i].ItemArray[1]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture).Trim() : "");
                            writer.WriteElementString("Category_of_Transaction", dr1.Length > 0 ? dr1[i].ItemArray[2].ToString().Trim() : "");
                            writer.WriteElementString("Invoice_No", dr1.Length > 0 ? dr1[i].ItemArray[3].ToString().Trim() : "");
                            writer.WriteElementString("Buyers_Name", dr1.Length > 0 ? dr1[i].ItemArray[4].ToString().Trim() : "");
                            writer.WriteElementString("Buyers_TIN", dr1.Length > 0 ? dr1[i].ItemArray[5].ToString().Trim() : "");
                            writer.WriteElementString("Description_of_Goods", dr1.Length > 0 ? dr1[i].ItemArray[7].ToString().Trim() : "");
                            writer.WriteElementString("Type_of_Sale", dr1.Length > 0 ? dr1[i].ItemArray[8].ToString().Trim() : "");
                            writer.WriteElementString("Sales_Amount", dr1.Length > 0 ? dr1[i].ItemArray[9].ToString().Trim() : "");
                            writer.WriteElementString("Tax_Rate", dr1.Length > 0 ? dr1[i].ItemArray[10].ToString().Trim() : "");
                            writer.WriteElementString("Tax_Amount", dr1.Length > 0 ? dr1[i].ItemArray[11].ToString().Trim() : "");
                            writer.WriteEndElement();
                        }
                    }
                    else
                    {
                        writer.WriteStartElement(tagName);
                        writer.WriteElementString("Date_of_Transaction", "");
                        writer.WriteElementString("Category_of_Transaction", "");
                        writer.WriteElementString("Invoice_No", "");
                        writer.WriteElementString("Buyers_Name", "");
                        writer.WriteElementString("Buyers_TIN", "");
                        writer.WriteElementString("Description_of_Goods", "");
                        writer.WriteElementString("Type_of_Sale", "");
                        writer.WriteElementString("Sales_Amount", "");
                        writer.WriteElementString("Tax_Rate", "");
                        writer.WriteElementString("Tax_Amount", "");
                        writer.WriteEndElement();
                    }
                    #endregion

                    writer.WriteEndElement();
                    #endregion

                    #region DVAT31A
                    writer.WriteStartElement("DVAT31A");
                    #region Data
                    //Data
                    dr1 = dSetToExport.Tables[1].Select("vat_type='DamanVAT31A'", "tran_no asc");
                    tagName = "Data";
                    BgWorkProgress.ReportProgress(10, "Creating " + tagName + " tag...");
                    rowCount = dr1.Length;

                    if (rowCount > 0)
                    {
                        for (int i = 0; i < rowCount; i++)
                        {
                            writer.WriteStartElement(tagName);
                            writer.WriteElementString("Date_of_Sale", dr1.Length > 0 ? Convert.ToDateTime(dr1[i].ItemArray[1]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture).Trim() : "");
                            writer.WriteElementString("Category_of_Transaction", dr1.Length > 0 ? dr1[i].ItemArray[2].ToString().Trim() : "");
                            writer.WriteElementString("Invoice_Number", dr1.Length > 0 ? dr1[i].ItemArray[3].ToString().Trim() : "");
                            writer.WriteElementString("Buyers_Name", dr1.Length > 0 ? dr1[i].ItemArray[4].ToString().Trim() : "");
                            writer.WriteElementString("Buyers_TIN", dr1.Length > 0 ? dr1[i].ItemArray[5].ToString().Trim() : "");
                            writer.WriteElementString("Destination_State", dr1.Length > 0 ? dr1[i].ItemArray[6].ToString().Trim() : "");
                            writer.WriteElementString("Description_of_Goods", dr1.Length > 0 ? dr1[i].ItemArray[7].ToString().Trim() : "");
                            writer.WriteElementString("Type_of_Sales", dr1.Length > 0 ? dr1[i].ItemArray[8].ToString().Trim() : "");
                            writer.WriteElementString("Sales_Amount", dr1.Length > 0 ? dr1[i].ItemArray[9].ToString().Trim() : "");
                            writer.WriteElementString("Rate_of_Tax", dr1.Length > 0 ? dr1[i].ItemArray[10].ToString().Trim() : "");
                            writer.WriteElementString("Tax_Amount", dr1.Length > 0 ? dr1[i].ItemArray[11].ToString().Trim() : "");
                            writer.WriteEndElement();
                        }
                    }
                    else
                    {
                        writer.WriteStartElement(tagName);
                        writer.WriteElementString("Date_of_Transaction", "");
                        writer.WriteElementString("Category_of_Transaction", "");
                        writer.WriteElementString("Invoice_No", "");
                        writer.WriteElementString("Buyers_Name", "");
                        writer.WriteElementString("Buyers_TIN", "");
                        writer.WriteElementString("Description_of_Goods", "");
                        writer.WriteElementString("Type_of_Sale", "");
                        writer.WriteElementString("Sales_Amount", "");
                        writer.WriteElementString("Tax_Rate", "");
                        writer.WriteElementString("Tax_Amount", "");
                        writer.WriteEndElement();
                    }
                    #endregion

                    writer.WriteEndElement();
                    #endregion
                    */
#endregion

                    #region Casewise Data
                    switch (SelectedTemplate)
                    {
                        #region DVAT30
                        case "DAMAN VAT 30":
                            writer.WriteStartElement("DVAT30");

                            #region Data
                            //Data
                            tagName = "Data";
                            BgWorkProgress.ReportProgress(10, "Creating " + tagName + " tag...");
                            rowCount = dTableToExport.Rows.Count;

                            if (rowCount > 0)
                            {
                                for (int i = 0; i < rowCount; i++)
                                {
                                    writer.WriteStartElement(tagName);
                                    writer.WriteElementString("Transaction_Date", Convert.ToDateTime(dTableToExport.Rows[i].ItemArray[0]).ToString("dd/MM/yyyy",CultureInfo.InvariantCulture).Trim());
                                    writer.WriteElementString("Transaction_No", dTableToExport.Rows[i].ItemArray[1].ToString().Trim());
                                    writer.WriteElementString("Category_of_Transaction", dTableToExport.Rows[i].ItemArray[2].ToString().Trim());
                                    writer.WriteElementString("Transaction_Type", dTableToExport.Rows[i].ItemArray[3].ToString().Trim());
                                    writer.WriteElementString("Seller_Name", dTableToExport.Rows[i].ItemArray[4].ToString().Trim());
                                    writer.WriteElementString("Seller_TIN_Number", dTableToExport.Rows[i].ItemArray[5].ToString().Trim());
                                    writer.WriteElementString("Short_Description_of_Goods", dTableToExport.Rows[i].ItemArray[6].ToString().Trim());
                                    writer.WriteElementString("VAT_Percentage", dTableToExport.Rows[i].ItemArray[7].ToString().Trim() != "0.00" ? dTableToExport.Rows[i].ItemArray[7].ToString().Trim() : "");
                                    writer.WriteElementString("Taxable_Amount", dTableToExport.Rows[i].ItemArray[8].ToString().Trim() != "0.00" ? dTableToExport.Rows[i].ItemArray[8].ToString().Trim() : "");
                                    writer.WriteElementString("Tax_Amount", dTableToExport.Rows[i].ItemArray[9].ToString().Trim() != "0.00" ? dTableToExport.Rows[i].ItemArray[9].ToString().Trim() : "");
                                    writer.WriteEndElement();
                                }
                            }
                            else
                            {
                                writer.WriteStartElement(tagName);
                                writer.WriteElementString("Transaction_Date", "");
                                writer.WriteElementString("Transaction_No", "");
                                writer.WriteElementString("Category_of_Transaction", "");
                                writer.WriteElementString("Transaction_Type", "");
                                writer.WriteElementString("Seller_Name", "");
                                writer.WriteElementString("Seller_TIN_Number", "");
                                writer.WriteElementString("Short_Description_of_Goods", "");
                                writer.WriteElementString("VAT_Percentage", "");
                                writer.WriteElementString("Taxable_Amount", "");
                                writer.WriteElementString("Tax_Amount", "");
                                writer.WriteEndElement();
                            }
                            #endregion

                            writer.WriteEndElement();
                            break;
                        #endregion

                        #region DVAT30A
                        case "DAMAN VAT 30A":
                            writer.WriteStartElement("DVAT30A");
                            #region Data
                            //Data
                            tagName = "Data";
                            BgWorkProgress.ReportProgress(10, "Creating " + tagName + " tag...");
                            rowCount = dTableToExport.Rows.Count;

                            if (rowCount > 0)
                            {
                                for (int i = 0; i < rowCount; i++)
                                {
                                    writer.WriteStartElement(tagName);
                                    writer.WriteElementString("Transaction_Date", Convert.ToDateTime(dTableToExport.Rows[i].ItemArray[0]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture).Trim());
                                    writer.WriteElementString("Transaction_No", dTableToExport.Rows[i].ItemArray[1].ToString().Trim());
                                    writer.WriteElementString("Category_of_Transaction", dTableToExport.Rows[i].ItemArray[2].ToString().Trim());
                                    writer.WriteElementString("Transaction_Type", dTableToExport.Rows[i].ItemArray[3].ToString().Trim());
                                    writer.WriteElementString("Seller_Name", dTableToExport.Rows[i].ItemArray[4].ToString().Trim());
                                    writer.WriteElementString("Seller_Address", dTableToExport.Rows[i].ItemArray[5].ToString().Trim());
                                    writer.WriteElementString("Destination_Name_of_the_State", dTableToExport.Rows[i].ItemArray[6].ToString().Trim());
                                    writer.WriteElementString("Seller_TIN_Number", dTableToExport.Rows[i].ItemArray[7].ToString().Trim());
                                    writer.WriteElementString("Short_Description_of_Goods", dTableToExport.Rows[i].ItemArray[8].ToString().Trim());
                                    writer.WriteElementString("Tax_Percentage", dTableToExport.Rows[i].ItemArray[9].ToString().Trim() != "0.00" ? dTableToExport.Rows[i].ItemArray[9].ToString().Trim() : "");
                                    writer.WriteElementString("Taxable_Amount", dTableToExport.Rows[i].ItemArray[10].ToString().Trim() != "0.00" ? dTableToExport.Rows[i].ItemArray[10].ToString().Trim() : "");
                                    writer.WriteEndElement();
                                }
                            }
                            else
                            {
                                writer.WriteStartElement(tagName);
                                writer.WriteElementString("Transaction_Date", "");
                                writer.WriteElementString("Transaction_No", "");
                                writer.WriteElementString("Category_of_Transaction", "");
                                writer.WriteElementString("Transaction_Type", "");
                                writer.WriteElementString("Seller_Name", "");
                                writer.WriteElementString("Seller_Address", "");
                                writer.WriteElementString("Destination_Name_of_the_State", "");
                                writer.WriteElementString("Seller_TIN_Number", "");
                                writer.WriteElementString("Short_Description_of_Goods", "");
                                writer.WriteElementString("Tax_Percentage", "");
                                writer.WriteElementString("Taxable_Amount", "");
                                writer.WriteEndElement();
                            }
                            #endregion

                            writer.WriteEndElement();
                            break;
                        #endregion

                        #region DVAT31
                        case "DAMAN VAT 31":
                            writer.WriteStartElement("DVAT31");
                            #region Data
                            //Data
                            tagName = "Data";
                            BgWorkProgress.ReportProgress(10, "Creating " + tagName + " tag...");
                            rowCount = dTableToExport.Rows.Count;

                            if (rowCount > 0)
                            {
                                for (int i = 0; i < rowCount; i++)
                                {
                                    writer.WriteStartElement(tagName);
                                    writer.WriteElementString("Date_of_Transaction", Convert.ToDateTime(dTableToExport.Rows[i].ItemArray[0]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture).Trim());
                                    writer.WriteElementString("Category_of_Transaction", dTableToExport.Rows[i].ItemArray[1].ToString().Trim());
                                    writer.WriteElementString("Invoice_No", dTableToExport.Rows[i].ItemArray[2].ToString().Trim());
                                    writer.WriteElementString("Buyers_Name", dTableToExport.Rows[i].ItemArray[3].ToString().Trim());
                                    writer.WriteElementString("Buyers_TIN", dTableToExport.Rows[i].ItemArray[4].ToString().Trim());
                                    writer.WriteElementString("Description_of_Goods", dTableToExport.Rows[i].ItemArray[5].ToString().Trim());
                                    writer.WriteElementString("Type_of_Sale", dTableToExport.Rows[i].ItemArray[6].ToString().Trim());
                                    writer.WriteElementString("Sales_Amount", dTableToExport.Rows[i].ItemArray[7].ToString().Trim());
                                    writer.WriteElementString("Tax_Rate", dTableToExport.Rows[i].ItemArray[8].ToString().Trim() != "0.00" ? dTableToExport.Rows[i].ItemArray[8].ToString().Trim() : "");
                                    writer.WriteElementString("Tax_Amount", dTableToExport.Rows[i].ItemArray[9].ToString().Trim() != "0.00" ? dTableToExport.Rows[i].ItemArray[9].ToString().Trim() : "");
                                    writer.WriteEndElement();
                                }
                            }
                            else
                            {
                                writer.WriteStartElement(tagName);
                                writer.WriteElementString("Date_of_Transaction", "");
                                writer.WriteElementString("Category_of_Transaction", "");
                                writer.WriteElementString("Invoice_No", "");
                                writer.WriteElementString("Buyers_Name", "");
                                writer.WriteElementString("Buyers_TIN", "");
                                writer.WriteElementString("Description_of_Goods", "");
                                writer.WriteElementString("Type_of_Sale", "");
                                writer.WriteElementString("Sales_Amount", "");
                                writer.WriteElementString("Tax_Rate", "");
                                writer.WriteElementString("Tax_Amount", "");
                                writer.WriteEndElement();
                            }
                            #endregion

                            writer.WriteEndElement();
                            break;
                        #endregion

                        #region DVAT31A
                        case "DAMAN VAT 31A":
                            writer.WriteStartElement("DVAT31A");
                            #region Data
                            //Data
                            tagName = "Data";
                            BgWorkProgress.ReportProgress(10, "Creating " + tagName + " tag...");
                            rowCount = dTableToExport.Rows.Count;

                            if (rowCount > 0)
                            {
                                for (int i = 0; i < rowCount; i++)
                                {
                                    writer.WriteStartElement(tagName);
                                    writer.WriteElementString("Date_of_Sale", Convert.ToDateTime(dTableToExport.Rows[i].ItemArray[0]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture).Trim());
                                    writer.WriteElementString("Category_of_Transaction", dTableToExport.Rows[i].ItemArray[1].ToString().Trim());
                                    writer.WriteElementString("Invoice_Number", dTableToExport.Rows[i].ItemArray[2].ToString().Trim());
                                    writer.WriteElementString("Buyers_Name", dTableToExport.Rows[i].ItemArray[3].ToString().Trim());
                                    writer.WriteElementString("Buyers_TIN", dTableToExport.Rows[i].ItemArray[4].ToString().Trim());
                                    writer.WriteElementString("Destination_State", dTableToExport.Rows[i].ItemArray[5].ToString().Trim());
                                    writer.WriteElementString("Description_of_Goods", dTableToExport.Rows[i].ItemArray[6].ToString().Trim());
                                    writer.WriteElementString("Type_of_Sales", dTableToExport.Rows[i].ItemArray[7].ToString().Trim());
                                    writer.WriteElementString("Sales_Amount", dTableToExport.Rows[i].ItemArray[8].ToString().Trim() != "0.00" ? dTableToExport.Rows[i].ItemArray[8].ToString().Trim() : "");
                                    writer.WriteElementString("Rate_of_Tax", dTableToExport.Rows[i].ItemArray[9].ToString().Trim() != "0.00" ? dTableToExport.Rows[i].ItemArray[9].ToString().Trim() : "");
                                    writer.WriteElementString("Tax_Amount", dTableToExport.Rows[i].ItemArray[10].ToString().Trim() != "0.00" ? dTableToExport.Rows[i].ItemArray[10].ToString().Trim() : "");
                                    writer.WriteEndElement();
                                }
                            }
                            else
                            {
                                writer.WriteStartElement(tagName);
                                writer.WriteElementString("Date_of_Transaction", "");
                                writer.WriteElementString("Category_of_Transaction", "");
                                writer.WriteElementString("Invoice_No", "");
                                writer.WriteElementString("Buyers_Name", "");
                                writer.WriteElementString("Buyers_TIN", "");
                                writer.WriteElementString("Description_of_Goods", "");
                                writer.WriteElementString("Type_of_Sale", "");
                                writer.WriteElementString("Sales_Amount", "");
                                writer.WriteElementString("Tax_Rate", "");
                                writer.WriteElementString("Tax_Amount", "");
                                writer.WriteEndElement();
                            }
                            #endregion

                            writer.WriteEndElement();
                            break;
                        #endregion
                    }
                    #endregion

                    //writer.WriteEndElement();
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
