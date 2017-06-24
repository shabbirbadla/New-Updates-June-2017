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
using System.Windows.Forms;
 
namespace Karnataka
{
    public class cls_Karnataka_DataTemplate
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

        public cls_Karnataka_DataTemplate(string connectionString)
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

                sqlstr = "EXECUTE USP_REP_CUST_KVAT '','','','" + FromDate + "','" + ToDate + "','','','','',0,0,'','','','','','','','','" + FinYear + "','" + CompId + "'";

                if (SelectedTemplate == "LOCAL SALES")
                {
                    sqlstr = sqlstr + " " + "select return_period_end,rtrim(cust_name) as cust_name,rtrim(cust_tin_no) as cust_tin_no,inv_no,inv_date"
                        + ",net_value,tax_amt"
                        + " from cust_kvat_tbl"
                        + " where vat_type='Sales'"
                        + "order by inv_no";
                }
                else if (SelectedTemplate == "LOCAL PURCHASES")
                {
                    sqlstr = sqlstr + " " + "select return_period_end,rtrim(cust_name) as cust_name,rtrim(cust_tin_no) as cust_tin_no,inv_no,inv_date"
                        + ",net_value,tax_amt"
                        + " from cust_kvat_tbl"
                        + " where vat_type='Purchases'"
                        + "order by inv_no";
                }
                else if (SelectedTemplate == "INTERSTATE SALES")
                {
                    sqlstr = sqlstr + " " + "select return_period_end,rtrim(cust_name) as cust_name,rtrim(cust_tin_no) as cust_tin_no,inv_no,inv_date"
                        + ",net_value,tax_amt,sugam_number"
                        + " from cust_kvat_tbl"
                        + " where vat_type='Interstatesales'"
                        + " order by inv_no";
                }
                else if (SelectedTemplate == "INTERSTATE PURCHASE")
                {
                    sqlstr = sqlstr + " " + "select return_period_end,rtrim(cust_name) as cust_name,rtrim(cust_tin_no) as cust_tin_no,inv_no,inv_date"
                        + ",net_value,tax_amt,sugam_number"
                        + " from cust_kvat_tbl"
                        + " where vat_type='Interstatepurchase'"
                        + " order by inv_no";
                }
                else if (SelectedTemplate == "VAT EXPORT INVOICE")
                {
                    sqlstr = sqlstr + " " + "select return_period_end,inv_no,inv_date,rtrim(cust_name) as cust_name,bill_lad_no,bill_lad_date"
                        + " ,rtrim(carrier_name) as carrier_name,rtrim(goods) as goods,inv_curr,inv_amt_rs"
                        + " from cust_kvat_tbl"
                        + " where vat_type='VATExportInvoice'"
                        + " order by inv_no";
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
                    DataRow[] dr1;
                    string tagName = string.Empty;

                    BgWorkProgress.ReportProgress(30, "XML File generation started...");

                    writer.WriteStartDocument();

                    #region Casewise Data
                    switch (SelectedTemplate)
                    {
                        #region Sales
                        case "LOCAL SALES":
                            #region Sales
                            //Sales
                            tagName = "Sales";
                            BgWorkProgress.ReportProgress(50, "Creating " + tagName + " tag...");
                            writer.WriteStartElement(tagName);

                            rowCount = dTableToExport.Rows.Count;

                            if (rowCount > 0)
                            {
                                for (int i = 0; i < rowCount; i++)
                                {
                                    writer.WriteStartElement(tagName);
                                    writer.WriteElementString("Return_period_end", dTableToExport.Rows[i].ItemArray[0].ToString().Trim());
                                    writer.WriteElementString("Buyers_name", dTableToExport.Rows[i].ItemArray[1].ToString().Trim());
                                    writer.WriteElementString("Buyers_TIN", dTableToExport.Rows[i].ItemArray[2].ToString().Trim());
                                    writer.WriteElementString("Invoice_Number", dTableToExport.Rows[i].ItemArray[3].ToString().Trim());
                                    writer.WriteElementString("Invoice_Date", Convert.ToDateTime(dTableToExport.Rows[i].ItemArray[4]).ToString("dd/MM/yyyy",CultureInfo.InvariantCulture).Trim());
                                    writer.WriteElementString("Net_Value", dTableToExport.Rows[i].ItemArray[5].ToString().Trim() != "0.00" ? dTableToExport.Rows[i].ItemArray[5].ToString().Trim() : "");
                                    writer.WriteElementString("Tax_Amount", dTableToExport.Rows[i].ItemArray[6].ToString().Trim() != "0.00" ? dTableToExport.Rows[i].ItemArray[6].ToString().Trim() : "");
                                    writer.WriteEndElement();
                                }
                            }
                            else
                            {
                                writer.WriteStartElement(tagName);
                                writer.WriteElementString("Return_period_end", "");
                                writer.WriteElementString("Buyers_name", "");
                                writer.WriteElementString("Buyers_TIN", "");
                                writer.WriteElementString("Invoice_Number", "");
                                writer.WriteElementString("Invoice_Date", "");
                                writer.WriteElementString("Net_value", "");
                                writer.WriteElementString("Tax_Amount", "");
                                writer.WriteEndElement();
                            }
                            #endregion

                            writer.WriteEndElement();
                            break;
                        #endregion

                        #region Purchases
                        case "LOCAL PURCHASES":
                            #region Purchases
                            //Sales
                            tagName = "Purchases";
                            BgWorkProgress.ReportProgress(50, "Creating " + tagName + " tag...");
                            writer.WriteStartElement(tagName);

                            rowCount = dTableToExport.Rows.Count;

                            if (rowCount > 0)
                            {
                                for (int i = 0; i < rowCount; i++)
                                {
                                    writer.WriteStartElement(tagName);
                                    writer.WriteElementString("Return_period_end", dTableToExport.Rows[i].ItemArray[0].ToString().Trim());
                                    writer.WriteElementString("Sellers_name", dTableToExport.Rows[i].ItemArray[1].ToString().Trim());
                                    writer.WriteElementString("Sellers_TIN", dTableToExport.Rows[i].ItemArray[2].ToString().Trim());
                                    writer.WriteElementString("Invoice_Number", dTableToExport.Rows[i].ItemArray[3].ToString().Trim());
                                    writer.WriteElementString("Invoice_Date", Convert.ToDateTime(dTableToExport.Rows[i].ItemArray[4]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture).Trim());
                                    writer.WriteElementString("Net_Value", dTableToExport.Rows[i].ItemArray[5].ToString().Trim() != "0.00" ? dTableToExport.Rows[i].ItemArray[5].ToString().Trim() : "");
                                    writer.WriteElementString("Tax_Amount", dTableToExport.Rows[i].ItemArray[6].ToString().Trim() != "0.00" ? dTableToExport.Rows[i].ItemArray[6].ToString().Trim() : "");
                                    writer.WriteEndElement();
                                }
                            }
                            else
                            {
                                writer.WriteStartElement(tagName);
                                writer.WriteElementString("Return_period_end", "");
                                writer.WriteElementString("Sellers_name", "");
                                writer.WriteElementString("Sellers_TIN", "");
                                writer.WriteElementString("Invoice_Number", "");
                                writer.WriteElementString("Invoice_Date", "");
                                writer.WriteElementString("Net_value", "");
                                writer.WriteElementString("Tax_Amount", "");
                                writer.WriteEndElement();
                            }
                            #endregion

                            writer.WriteEndElement();
                            break;
                        #endregion

                        #region Interstatesales
                        case "INTERSTATE SALES":
                            #region Interstatesales
                            //Sales
                            tagName = "Inter_State_sales";
                            BgWorkProgress.ReportProgress(50, "Creating " + tagName + " tag...");
                            writer.WriteStartElement(tagName);

                            rowCount = dTableToExport.Rows.Count;

                            if (rowCount > 0)
                            {
                                for (int i = 0; i < rowCount; i++)
                                {
                                    writer.WriteStartElement(tagName);
                                    writer.WriteElementString("Return_period_end", dTableToExport.Rows[i].ItemArray[0].ToString().Trim());
                                    writer.WriteElementString("Buyers_name", dTableToExport.Rows[i].ItemArray[1].ToString().Trim());
                                    writer.WriteElementString("Buyers_TIN", dTableToExport.Rows[i].ItemArray[2].ToString().Trim());
                                    writer.WriteElementString("Invoice_Number", dTableToExport.Rows[i].ItemArray[3].ToString().Trim());
                                    writer.WriteElementString("Invoice_Date", Convert.ToDateTime(dTableToExport.Rows[i].ItemArray[4]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture).Trim());
                                    writer.WriteElementString("Net_Value", dTableToExport.Rows[i].ItemArray[5].ToString().Trim() != "0.00" ? dTableToExport.Rows[i].ItemArray[5].ToString().Trim() : "");
                                    writer.WriteElementString("Tax_Amount", dTableToExport.Rows[i].ItemArray[6].ToString().Trim() != "0.00" ? dTableToExport.Rows[i].ItemArray[6].ToString().Trim() : "");
                                    writer.WriteElementString("Sugam_number", dTableToExport.Rows[i].ItemArray[7].ToString().Trim());
                                    writer.WriteEndElement();
                                }
                            }
                            else
                            {
                                writer.WriteStartElement(tagName);
                                writer.WriteElementString("Return_period_end", "");
                                writer.WriteElementString("Buyers_name", "");
                                writer.WriteElementString("Buyers_TIN", "");
                                writer.WriteElementString("Invoice_Number", "");
                                writer.WriteElementString("Invoice_Date", "");
                                writer.WriteElementString("Net_value", "");
                                writer.WriteElementString("Tax_Amount", "");
                                writer.WriteElementString("Sugam_number", "");
                                writer.WriteEndElement();
                            }
                            #endregion

                            writer.WriteEndElement();
                            break;
                        #endregion

                        #region Interstatepurchase
                        case "INTERSTATE PURCHASE":
                            #region Interstatepurchase
                            //Sales
                            tagName = "Inter_State_Purchases";
                            BgWorkProgress.ReportProgress(50, "Creating " + tagName + " tag...");
                            writer.WriteStartElement(tagName);

                            rowCount = dTableToExport.Rows.Count;

                            if (rowCount > 0)
                            {
                                for (int i = 0; i < rowCount; i++)
                                {
                                    writer.WriteStartElement(tagName);
                                    writer.WriteElementString("Return_period_end", dTableToExport.Rows[i].ItemArray[0].ToString().Trim());
                                    writer.WriteElementString("Sellers_name", dTableToExport.Rows[i].ItemArray[1].ToString().Trim());
                                    writer.WriteElementString("Sellers_TIN", dTableToExport.Rows[i].ItemArray[2].ToString().Trim());
                                    writer.WriteElementString("Invoice_Number", dTableToExport.Rows[i].ItemArray[3].ToString().Trim());
                                    writer.WriteElementString("Invoice_Date", Convert.ToDateTime(dTableToExport.Rows[i].ItemArray[4]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture).Trim());
                                    writer.WriteElementString("Net_Value", dTableToExport.Rows[i].ItemArray[5].ToString().Trim() != "0.00" ? dTableToExport.Rows[i].ItemArray[5].ToString().Trim() : "");
                                    writer.WriteElementString("Tax_Amount", dTableToExport.Rows[i].ItemArray[6].ToString().Trim() != "0.00" ? dTableToExport.Rows[i].ItemArray[6].ToString().Trim() : "");
                                    writer.WriteElementString("Sugam_number", dTableToExport.Rows[i].ItemArray[7].ToString().Trim());
                                    writer.WriteEndElement();
                                }
                            }
                            else
                            {
                                writer.WriteStartElement(tagName);
                                writer.WriteElementString("Return_period_end", "");
                                writer.WriteElementString("Sellers_name", "");
                                writer.WriteElementString("Sellers_TIN", "");
                                writer.WriteElementString("Invoice_Number", "");
                                writer.WriteElementString("Invoice_Date", "");
                                writer.WriteElementString("Net_value", "");
                                writer.WriteElementString("Tax_Amount", "");
                                writer.WriteElementString("Sugam_number", "");
                                writer.WriteEndElement();
                            }
                            #endregion

                            writer.WriteEndElement();
                            break;
                        #endregion

                        #region VATExportinvoice
                        case "VAT EXPORT INVOICE":
                            #region VATExportinvoice
                            //Sales
                            tagName = "VAT_export_Invoice";
                            BgWorkProgress.ReportProgress(50, "Creating " + tagName + " tag...");
                            writer.WriteStartElement(tagName);

                            rowCount = dTableToExport.Rows.Count;

                            if (rowCount > 0)
                            {
                                for (int i = 0; i < rowCount; i++)
                                {
                                    writer.WriteStartElement(tagName);
                                    writer.WriteElementString("Return_period_end", dTableToExport.Rows[i].ItemArray[0].ToString().Trim());
                                    writer.WriteElementString("Invoice_No", dTableToExport.Rows[i].ItemArray[1].ToString().Trim());
                                    writer.WriteElementString("Invoice_Date", Convert.ToDateTime(dTableToExport.Rows[i].ItemArray[2]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture).Trim());
                                    writer.WriteElementString("Customer_name", dTableToExport.Rows[i].ItemArray[3].ToString().Trim());
                                    writer.WriteElementString("Bill_of_Lading_number", dTableToExport.Rows[i].ItemArray[4].ToString().Trim());
                                    writer.WriteElementString("Bill_of_Lading_Date", dTableToExport.Rows[i].ItemArray[5].ToString().Trim().Contains("1900")
                                        ? "" : Convert.ToDateTime(dTableToExport.Rows[i].ItemArray[5]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture).Trim());
                                    writer.WriteElementString("Carrier_name", dTableToExport.Rows[i].ItemArray[6].ToString().Trim());
                                    writer.WriteElementString("Goods", dTableToExport.Rows[i].ItemArray[7].ToString().Trim());
                                    writer.WriteElementString("Invoice_currency", dTableToExport.Rows[i].ItemArray[8].ToString().Trim());
                                    writer.WriteElementString("Invoice_amount_in_rupees", dTableToExport.Rows[i].ItemArray[9].ToString().Trim() != "0.00" ? dTableToExport.Rows[i].ItemArray[9].ToString().Trim() : "");
                                    writer.WriteEndElement();
                                }
                            }
                            else
                            {
                                writer.WriteStartElement(tagName);
                                writer.WriteElementString("Return_period_end", "");
                                writer.WriteElementString("Invoice_No", "");
                                writer.WriteElementString("Invoice_Date", "");
                                writer.WriteElementString("Customer_name", "");
                                writer.WriteElementString("Bill_of_Lading_number", "");
                                writer.WriteElementString("Bill_of_Lading_Date", "");
                                writer.WriteElementString("Carrier_name", "");
                                writer.WriteElementString("Goods", "");
                                writer.WriteElementString("Invoice_currency", "");
                                writer.WriteElementString("Invoice_amount_in_rupees", "");
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
