using System;
using System.Collections.Generic;
using System.Text;
using U2TPlus.DAL.Interface;
using U2TPlus.DAL;
using System.Data;
using System.Configuration;

namespace U2TPlus.DAL
{
    public class GenerateTransXML : IGenerateTransXML
    {
        #region IGenerateTransXML Members

        private string mPresentTransTableName;
        /// <summary>
        /// Get Or Set Present Exicutable Table Name of the Transctions.
        /// </summary>
        public string PresentTransTableName
        {
            get { return mPresentTransTableName; }
            set { mPresentTransTableName = value; }
        }
        /// <summary>
        /// Enumeration values of the Transaction Tables
        /// </summary>
        public enum TransTableNames
        {
            MAIN, ITEM, ACDET, ITREF, MALL
        }
        private string mdbError = string.Empty;
        /// <summary>
        /// Get Error Report when Exicuting Functions
        /// <returns>
        /// String
        /// </returns>
        /// </summary>
        public string dbError
        {
            get
            {
                return mdbError;
            }
            set
            {
                mdbError = value;
            }
        }
        private DataTable mTallyTagsTable;
        /// <summary>
        /// Get or Set The Datatable of tally tags based on the master id in generating xml
        /// <returns>
        /// String
        /// </returns>
        /// </summary>
        public DataTable TallyTagsTable
        {
            get
            {
                return mTallyTagsTable;
            }
            set
            {
                mTallyTagsTable = value;
            }
        }
        private string mConnectionString;
        /// <summary>
        /// Get or Set the Selected Company Database Connection String
        /// <returns>
        /// string
        /// </returns>
        /// </summary>
        public string ConnectionString
        {
            get { return mConnectionString; }
            set { mConnectionString = value; }
        }
        private string mCompanyDBName;
        /// <summary>
        /// Get or Set the Selected Company Database Name
        /// <returns>
        /// string
        /// </returns>
        /// </summary>
        public string CompanyDBName
        {
            get { return mCompanyDBName; }
            set { mCompanyDBName = value; }
        }
        private string mGeneratingOptions;
        /// <summary>
        /// Get or Set the Selected Generating Options as Not Generated, Generated, All.
        /// <returns>
        /// string
        /// </returns>
        /// </summary>
        public string GeneratingOptions
        {
            get { return mGeneratingOptions; }
            set { mGeneratingOptions = value; }
        }
        private DataSet mReturnDataSet;
        /// <summary>
        /// Get the After get the data based on the Parent Group 
        /// from Account Group Master returns chailds data as a DataSet
        /// <returns>
        /// DataSet
        /// </returns>
        /// </summary>
        public DataSet ReturnDataSet
        {
            get { return mReturnDataSet; }
            set { mReturnDataSet = value; }
        }
        private string mConfiguration_ID;
        /// <summary>
        /// Get or Set the Selected Company ID from Congif.xml
        /// <returns>
        /// string
        /// </returns>
        /// </summary>
        public string Configuration_ID
        {
            get { return mConfiguration_ID; }
            set { mConfiguration_ID = value; }
        }
        public bool _isNeedToUpdate = false;
        /// <summary>
        /// Get or Set Selected Company Master Tables is need to be update or not
        /// <returns>
        /// bool
        /// </returns>
        /// </summary>
        public bool isNeedToUpdate
        {
            get { return _isNeedToUpdate; }
            set { _isNeedToUpdate = value; }
        }
        private bool mIsGeneratesDefaultSettings;
        /// <summary>
        /// Get Or Set Generating XML from default settings XML file or master table settings xml
        /// <returns>
        /// bool
        /// </returns>
        /// </summary>
        public bool isGeneratesDefaultSettings
        {
            get { return mIsGeneratesDefaultSettings; }
            set { mIsGeneratesDefaultSettings = value; }
        }
        private string mGenerateFromDate;
        /// <summary>
        /// Get Or Set Generating XML from From Date
        /// </summary>
        public string GenerateFromDate
        {
            get { return mGenerateFromDate; }
            set { mGenerateFromDate = value; }
        }
        private string mGenerateToDate;
        /// <summary>
        /// Get Or Set Generating XML from To Date
        /// </summary>
        public string GenerateToDate
        {
            get { return mGenerateToDate; }
            set { mGenerateToDate = value; }
        }
        private string mGenaretingTransName;
        /// <summary>
        /// Get Or Set Genareting Transction Name
        /// </summary>
        public string GenaretingTransName
        {
            get { return mGenaretingTransName; }
            set { mGenaretingTransName = value; }
        }
        private string[] mTableNames = new string[5];
        /// <summary>
        /// Get Or Set Transaction Table Names
        /// </summary>
        public string[] tablename
        {
            get { return mTableNames; }
            set { mTableNames = value; }
        }
        private string mTableEntryType;
        /// <summary>
        /// Get Or Set Table Entry Type
        /// </summary>
        public string TableEntryType
        {
            get { return mTableEntryType; }
            set { mTableEntryType = value; }
        }

        #endregion

        #region Constractor

        DBConnections objConnection;
        public GenerateTransXML()
        {
            objConnection = new DBConnections();
        }

        #endregion

        #region Public Methods

        public DataSet GenerateTransactionXML(IGenerateTransXML _iGenerateTransXML)
        {
            string[] str = Enum.GetNames(typeof(TransTableNames));

            object returnVal = null;
            int returnINT = 0;
            string WhereCondition = string.Empty;
            objConnection.AppConnectionString = _iGenerateTransXML.ConnectionString;
            objConnection.ClearParameters();
            objConnection.CommandTypeToExecute = CommandType.Text;
            if (!string.IsNullOrEmpty(GenerateFromDate))
            {
                DateTime d = DateTime.Parse(GenerateFromDate);
                GenerateFromDate = d.ToString("MM/dd/yyyy");
            }

            if (!string.IsNullOrEmpty(GenerateToDate))
            {
                DateTime d = DateTime.Parse(GenerateToDate);
                GenerateToDate = d.ToString("MM/dd/yyyy");
            }

            if (GeneratingOptions.ToLower() == "Not Generated".ToLower())
            {
                objConnection.CommandText = "(Select [Name] From Syscolumns Where [Name] = 'isU2TExport' And Id = Object_Id('" + this.PresentTransTableName + "'))";

                returnVal = objConnection.ExecuteScalar();

                if (returnVal == null)
                {
                    objConnection.CommandText = "Alter Table " + this.PresentTransTableName + " add isU2TExport Bit";

                    returnINT = objConnection.ExecuteNonQuery();

                    if (returnINT == -1)
                    {
                        objConnection.CommandText = "(Select [Name] From Syscolumns Where [Name] = 'isU2TExport' And Id = Object_Id('" + this.PresentTransTableName + "'))";

                        returnVal = objConnection.ExecuteScalar();

                        if (returnVal != null)
                        {
                            returnINT = 1;
                        }
                        else
                        {
                            returnINT = 0;
                        }
                    }
                }
                else
                {
                    //WhereCondition = ConfigurationManager.AppSettings[PresentTransTableName].ToString();
                    //WhereCondition = convertSpecialCharsToNormal(WhereCondition)
                    

                    if (WhereCondition != string.Empty)
                    {
                        WhereCondition = "And " + WhereCondition + "And DATE between '" + this.GenerateFromDate + "'and '" + this.GenerateToDate + " '";
                    }
                    else
                    {
                        WhereCondition = "And DATE between '" + this.GenerateFromDate + "'and '" + this.GenerateToDate + " '";
                    }
                    objConnection.CommandText = "select * from " + this.PresentTransTableName + " Where isU2TExport is null " + WhereCondition;
                    ReturnDataSet = objConnection.GetDataSet(this.PresentTransTableName);
                }

                if (returnINT > 0)
                {
                    //WhereCondition = ConfigurationManager.AppSettings[PresentTransTableName].ToString();
                    //WhereCondition = convertSpecialCharsToNormal(WhereCondition);
                    if (WhereCondition != string.Empty)
                    {
                        WhereCondition = "And " + WhereCondition + "And DATE between '" + this.GenerateFromDate + "'and '" + this.GenerateToDate + " '";
                    }
                    else
                    {
                        WhereCondition = "And DATE between '" + this.GenerateFromDate + "'and '" + this.GenerateToDate + " '";
                    }
                    objConnection.CommandText = "select * from " + this.PresentTransTableName + " Where isU2TExport is null " + WhereCondition;
                    ReturnDataSet = objConnection.GetDataSet(this.PresentTransTableName);
                }
            }
            if (GeneratingOptions.ToLower() == "Generated".ToLower())
            {
                objConnection.CommandText = "(Select [Name] From Syscolumns Where [Name] = 'isU2TExport' And Id = Object_Id('" + this.PresentTransTableName + "'))";

                returnVal = objConnection.ExecuteScalar();

                if (returnVal == null)
                {
                    objConnection.CommandText = "Alter Table " + this.PresentTransTableName + " add isU2TExport Bit";

                    returnINT = objConnection.ExecuteNonQuery();

                    if (returnINT == -1)
                    {
                        objConnection.CommandText = "(Select [Name] From Syscolumns Where [Name] = 'isU2TExport' And Id = Object_Id('" + this.PresentTransTableName + "'))";

                        returnVal = objConnection.ExecuteScalar();

                        if (returnVal != null)
                        {
                            returnINT = 1;
                        }
                        else
                        {
                            returnINT = 0;
                        }
                    }
                }
                else
                {
                    //WhereCondition = ConfigurationManager.AppSettings[PresentTransTableName].ToString();
                    //WhereCondition = convertSpecialCharsToNormal(WhereCondition);
                    if (WhereCondition != string.Empty)
                    {
                        WhereCondition = "And " + WhereCondition + "And DATE between '" + this.GenerateFromDate + "'and '" + this.GenerateToDate + " '";
                    }
                    else
                    {
                        WhereCondition = "And DATE between '" + this.GenerateFromDate + "'and '" + this.GenerateToDate + " '";
                    }
                    objConnection.CommandText = "select * from " + this.PresentTransTableName + " Where isU2TExport is null " + WhereCondition;
                    ReturnDataSet = objConnection.GetDataSet(this.PresentTransTableName);
                }

                if (returnINT > 0)
                {
                    //WhereCondition = ConfigurationManager.AppSettings[PresentTransTableName].ToString();
                    //WhereCondition = convertSpecialCharsToNormal(WhereCondition);
                    if (WhereCondition != string.Empty)
                    {
                        WhereCondition = "And " + WhereCondition + "And DATE between '" + this.GenerateFromDate + "'and '" + this.GenerateToDate + " '";
                    }
                    else
                    {
                        WhereCondition = "And DATE between '" + this.GenerateFromDate + "'and '" + this.GenerateToDate + " '";
                    }
                    objConnection.CommandText = "select * from " + this.PresentTransTableName + " Where isU2TExport is null " + WhereCondition;
                    ReturnDataSet = objConnection.GetDataSet(this.PresentTransTableName);
                }
            }
            if (GeneratingOptions.ToLower() == "All".ToLower())
            {
                objConnection.CommandText = "(Select [Name] From Syscolumns Where [Name] = 'isU2TExport' And Id = Object_Id('" + this.PresentTransTableName + "'))";
                returnVal = objConnection.ExecuteScalar();

                if (returnVal == null)
                {
                    objConnection.CommandText = "Alter Table " + this.PresentTransTableName + " add isU2TExport Bit";

                    returnINT = objConnection.ExecuteNonQuery();

                    if (returnINT == -1)
                    {
                        objConnection.CommandText = "(Select [Name] From Syscolumns Where [Name] = 'isU2TExport' And Id = Object_Id('" + this.PresentTransTableName + "'))";

                        returnVal = objConnection.ExecuteScalar();

                        if (returnVal != null)
                        {
                            returnINT = 1;
                        }
                        else
                        {
                            returnINT = 0;
                        }
                    }
                }
                else
                {
                    //WhereCondition = ConfigurationManager.AppSettings[PresentTransTableName].ToString();
                    //WhereCondition = convertSpecialCharsToNormal(WhereCondition);
                    if (WhereCondition != string.Empty)
                    {
                        WhereCondition = " where " + WhereCondition + "And DATE between '" + this.GenerateFromDate + "'and '" + this.GenerateToDate + " '";
                    }
                    else
                    {
                        WhereCondition = " where   DATE between '" + this.GenerateFromDate + "'and '" + this.GenerateToDate + " '";
                    }
                    objConnection.CommandText = "select * from " + this.PresentTransTableName + WhereCondition;
                    ReturnDataSet = objConnection.GetDataSet(this.PresentTransTableName);
                }

                if (returnINT > 0)
                {
                    //WhereCondition = ConfigurationManager.AppSettings[PresentTransTableName].ToString();
                    //WhereCondition = convertSpecialCharsToNormal(WhereCondition);
                    if (WhereCondition != string.Empty)
                    {
                        WhereCondition = " where  " + WhereCondition + "And DATE between '" + this.GenerateFromDate + "'and '" + this.GenerateToDate + " '";
                    }
                    else
                    {
                        WhereCondition = " where DATE between '" + this.GenerateFromDate + "'and '" + this.GenerateToDate + " '";
                    }
                    objConnection.CommandText = "select * from " + this.PresentTransTableName + WhereCondition;
                    ReturnDataSet = objConnection.GetDataSet(this.PresentTransTableName);
                }
            }

            return ReturnDataSet;


        }

        public int UpdateGenerateTrans(IGenerateTransXML _iGenerateTransXML)
        {
            int returnINT = 0;
            objConnection.AppConnectionString = _iGenerateTransXML.ConnectionString;
            objConnection.ClearParameters();
            objConnection.CommandTypeToExecute = CommandType.Text;

            try
            {
                foreach (string PTblName in tablename)
                {
                    if (!string.IsNullOrEmpty(GenerateFromDate))
                    {
                        DateTime d = DateTime.Parse(GenerateFromDate);
                        GenerateFromDate = d.ToString("MM/dd/YYYY");
                    }

                    if (!string.IsNullOrEmpty(GenerateToDate))
                    {
                        DateTime d = DateTime.Parse(GenerateToDate);
                        GenerateToDate = d.ToString("MM/dd/YYYY");
                    }

                    objConnection.CommandText = "update " + PTblName + " set isU2TExport = 'true' where isU2TExport is null And DATE between '" + this.GenerateFromDate + "'and '" + this.GenerateToDate + " '";

                    returnINT = objConnection.ExecuteNonQuery();

                    if (returnINT == 0)
                    {
                        objConnection.CommandText = "select * from " + PTblName + " where isU2TExport is null And DATE between '" + this.GenerateFromDate + "'and '" + this.GenerateToDate + " '";
                        DataSet UpdateVerifyDS = new DataSet();
                        UpdateVerifyDS = objConnection.GetDataSet("UpdateVerify");

                        if (UpdateVerifyDS != null)
                        {
                            if (UpdateVerifyDS.Tables[0].Rows.Count > 0)
                            {
                                returnINT = 0;
                            }
                            else
                            {
                                returnINT = 1;
                            }
                        }
                        else
                        {
                            returnINT = 0;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                mdbError = ex.Message;
            }

            return returnINT;
        }

        public string GetEntryType(IGenerateTransXML _iGenerateTransXML)
        {
            string Entry_Type = string.Empty;
            try
            {
                objConnection.AppConnectionString = _iGenerateTransXML.ConnectionString;
                objConnection.ClearParameters();
                objConnection.CommandTypeToExecute = CommandType.Text;

                objConnection.CommandText = "select entry_ty from lcode where code_nm='" + this.GenaretingTransName + "'";

                Entry_Type = objConnection.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                dbError = ex.Message;
            }
            return Entry_Type;
        }

        #endregion

        #region private Methods

        /// <summary>
        /// Convert Special Carectors in to XML format like & to &amp; ect..
        /// </summary>
        /// <param name="ConvertingValue"></param>
        /// <returns>string</returns>
        public string convertSpecialChars(string ConvertingValue)
        {
            try
            {
                if (ConvertingValue.Contains("&"))
                    ConvertingValue = ConvertingValue.Replace("&", "&amp;");
                if (ConvertingValue.Contains("<"))
                    ConvertingValue = ConvertingValue.Replace("<", "&lt;");
                if (ConvertingValue.Contains(">"))
                    ConvertingValue = ConvertingValue.Replace(">", "&gt;");
                if (ConvertingValue.Contains('"'.ToString()))
                    ConvertingValue = ConvertingValue.Replace('"'.ToString(), "&quot;");
                if (ConvertingValue.Contains("'"))
                    ConvertingValue = ConvertingValue.Replace("'", "&apos;");
            }
            catch (Exception ex)
            {
                mdbError = ex.Message;
            }

            return ConvertingValue;
        }

        /// <summary>
        /// Convert Special xml Carectors in to noprmal format like &amp; to & ect..
        /// </summary>
        /// <param name="ConvertingValue"></param>
        /// <returns>string</returns>
        public string convertSpecialCharsToNormal(string ConvertingValue)
        {
            try
            {
                if (ConvertingValue.Contains("&"))
                    ConvertingValue = ConvertingValue.Replace("&amp;", "&");
                if (ConvertingValue.Contains("<"))
                    ConvertingValue = ConvertingValue.Replace("&ly;", "<");
                if (ConvertingValue.Contains(">"))
                    ConvertingValue = ConvertingValue.Replace("&gt;", ">");
                if (ConvertingValue.Contains('"'.ToString()))
                    ConvertingValue = ConvertingValue.Replace("&quot;", '"'.ToString());
                if (ConvertingValue.Contains("'"))
                    ConvertingValue = ConvertingValue.Replace("&apos;", "'");
            }
            catch (Exception ex)
            {
                mdbError = ex.Message;
            }

            return ConvertingValue;
        }

        #endregion

    }
}
