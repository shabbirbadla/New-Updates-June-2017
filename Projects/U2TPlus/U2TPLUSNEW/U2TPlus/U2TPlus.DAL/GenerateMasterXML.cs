using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using U2TPlus.DAL;
using U2TPlus.DAL.Interface;
using System.Configuration;

namespace U2TPlus.DAL
{
    public class GenerateMasterXML : IGenerateMasterXML
    {
        #region IGenerateMasterXML Members

        private string mMasterTableName;
        /// <summary>
        /// Get or Set Master Table Name form databse for Selected Company (To Generate XML)
        /// <returns>
        /// string
        /// </returns>
        /// </summary>
        public string MasterTableName
        {
            get { return mMasterTableName; }
            set { mMasterTableName = value; }
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
        private string mAccountGroupParent;
        /// <summary>
        /// Get or Set Parent Name to get the releted Childs
        /// <returns>
        /// string
        /// </returns>
        /// </summary>
        public string AccountGroupParent
        {
            get { return mAccountGroupParent; }
            set { mAccountGroupParent = value; }
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

        #endregion

        #region Constractor
        DBConnections objConnection;
        public GenerateMasterXML()
        {
            objConnection = new DBConnections();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// To Get the Selected Company Master Table Data for Tally Version 9.0 as a DataSet
        /// </summary>
        /// <param name="_iGenerateMasterXML"></param>
        /// <returns>DataSet</returns>
        public DataSet GenerateMaster(IGenerateMasterXML _iGenerateMasterXML)
        {
            object returnVal = null;
            int returnINT = 0;
            string WhereCondition = string.Empty;
            objConnection.AppConnectionString = _iGenerateMasterXML.ConnectionString;
            objConnection.ClearParameters();
            objConnection.CommandTypeToExecute = CommandType.Text;
            if (GeneratingOptions.ToLower() == "Not Generated".ToLower())
            {
                objConnection.CommandText = "(Select [Name] From Syscolumns Where [Name] = 'isU2TExport' And Id = Object_Id('" + this.MasterTableName + "'))";

                returnVal = objConnection.ExecuteScalar();

                if (returnVal == null)
                {
                    objConnection.CommandText = "Alter Table " + this.MasterTableName + " add isU2TExport Bit";

                    returnINT = objConnection.ExecuteNonQuery();

                    if (returnINT == -1)
                    {
                        objConnection.CommandText = "(Select [Name] From Syscolumns Where [Name] = 'isU2TExport' And Id = Object_Id('" + this.MasterTableName + "'))";

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
                    WhereCondition = ConfigurationManager.AppSettings[MasterTableName].ToString();
                    WhereCondition = convertSpecialCharsToNormal(WhereCondition);
                    if (WhereCondition != string.Empty)
                    {
                        WhereCondition = "And " + WhereCondition;
                    }
                    objConnection.CommandText = "select * from " + this.MasterTableName + " Where isU2TExport is null " + WhereCondition;
                    ReturnDataSet = objConnection.GetDataSet("Master");
                }

                if (returnINT > 0)
                {
                    WhereCondition = ConfigurationManager.AppSettings[MasterTableName].ToString();
                    WhereCondition = convertSpecialCharsToNormal(WhereCondition);
                    if (WhereCondition != string.Empty)
                    {
                        WhereCondition = "And " + WhereCondition;
                    }
                    objConnection.CommandText = "select * from " + this.MasterTableName + " Where isU2TExport is null " + WhereCondition;
                    ReturnDataSet = objConnection.GetDataSet("Master");
                }
            }
            if (GeneratingOptions.ToLower() == "Generated".ToLower())
            {
                objConnection.CommandText = "(Select [Name] From Syscolumns Where [Name] = 'isU2TExport' And Id = Object_Id('" + this.MasterTableName + "'))";

                returnVal = objConnection.ExecuteScalar();

                if (returnVal == null)
                {
                    objConnection.CommandText = "Alter Table " + this.MasterTableName + " add isU2TExport Bit";

                    returnINT = objConnection.ExecuteNonQuery();

                    if (returnINT == -1)
                    {
                        objConnection.CommandText = "(Select [Name] From Syscolumns Where [Name] = 'isU2TExport' And Id = Object_Id('" + this.MasterTableName + "'))";

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
                    WhereCondition = ConfigurationManager.AppSettings[MasterTableName].ToString();
                    WhereCondition = convertSpecialCharsToNormal(WhereCondition);
                    if (WhereCondition != string.Empty)
                    {
                        WhereCondition = "And " + WhereCondition;
                    }
                    objConnection.CommandText = "select * from " + this.MasterTableName + " Where isU2TExport is null " + WhereCondition;
                    ReturnDataSet = objConnection.GetDataSet("Master");
                }

                if (returnINT > 0)
                {
                    WhereCondition = ConfigurationManager.AppSettings[MasterTableName].ToString();
                    WhereCondition = convertSpecialCharsToNormal(WhereCondition);
                    if (WhereCondition != string.Empty)
                    {
                        WhereCondition = "And " + WhereCondition;
                    }
                    objConnection.CommandText = "select * from " + this.MasterTableName + " Where isU2TExport is null " + WhereCondition;
                    ReturnDataSet = objConnection.GetDataSet("Master");
                }
            }
            if (GeneratingOptions.ToLower() == "All".ToLower())
            {
                objConnection.CommandText = "(Select [Name] From Syscolumns Where [Name] = 'isU2TExport' And Id = Object_Id('" + this.MasterTableName + "'))";
                returnVal = objConnection.ExecuteScalar();

                if (returnVal == null)
                {
                    objConnection.CommandText = "Alter Table " + this.MasterTableName + " add isU2TExport Bit";

                    returnINT = objConnection.ExecuteNonQuery();

                    if (returnINT == -1)
                    {
                        objConnection.CommandText = "(Select [Name] From Syscolumns Where [Name] = 'isU2TExport' And Id = Object_Id('" + this.MasterTableName + "'))";

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
                    WhereCondition = ConfigurationManager.AppSettings[MasterTableName].ToString();
                    WhereCondition = convertSpecialCharsToNormal(WhereCondition);
                    if (WhereCondition != string.Empty)
                    {
                        WhereCondition = " Where " + WhereCondition;
                    }
                    objConnection.CommandText = "select * from " + this.MasterTableName + WhereCondition;
                    ReturnDataSet = objConnection.GetDataSet("Master");
                }

                if (returnINT > 0)
                {
                    WhereCondition = ConfigurationManager.AppSettings[MasterTableName].ToString();
                    WhereCondition = convertSpecialCharsToNormal(WhereCondition);
                    objConnection.CommandText = "select * from " + this.MasterTableName + " Where " + WhereCondition;
                    ReturnDataSet = objConnection.GetDataSet("Master");
                }
            }

            return ReturnDataSet;
        }

        public int UpdateGenerateMaster(IGenerateMasterXML _iGenerateMasterXML)
        {
            int returnINT = 0;
            objConnection.AppConnectionString = _iGenerateMasterXML.ConnectionString;
            objConnection.ClearParameters();
            objConnection.CommandTypeToExecute = CommandType.Text;

            try
            {
                objConnection.CommandText = "update " + this.MasterTableName + " set isU2TExport = 'true' where isU2TExport is null";

                returnINT = objConnection.ExecuteNonQuery();

                if (returnINT == 0)
                {
                    objConnection.CommandText = "select * from " + this.MasterTableName + " where isU2TExport is null";
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
            catch (Exception ex)
            {
                mdbError = ex.Message;
            }

            return returnINT;
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

        #region Commented Code
        /// <summary>
        /// To Get the Selected Company Account Group Master Table Data with child  for Tally Version 9.0 as a DataSet
        /// </summary>
        /// <param name="_iGenerateMasterXML"></param>
        /// <returns>DataSet</returns>
        //public DataSet GenerateAccountGroupMasterTV90WithChild(IGenerateMasterXML _iGenerateMasterXML)
        //{
        //    object returnVal = null;
        //    int returnINT = 0;
        //    objConnection.AppConnectionString = _iGenerateMasterXML.ConnectionString;
        //    objConnection.ClearParameters();
        //    objConnection.CommandTypeToExecute = CommandType.Text;
        //    if (GeneratingOptions.ToLower() == "Not Generated".ToLower())
        //    {
        //        objConnection.CommandText = "(Select [Name] From Syscolumns Where [Name] = 'isU2TExport' And Id = Object_Id('" + this.MasterTableName + "'))";

        //        returnVal = objConnection.ExecuteScalar();

        //        if (returnVal == null)
        //        {
        //            objConnection.CommandText = "Alter Table " + this.MasterTableName + " add isU2TExport Bit";

        //            returnINT = objConnection.ExecuteNonQuery();

        //            if (returnINT == -1)
        //            {
        //                objConnection.CommandText = "(Select [Name] From Syscolumns Where [Name] = 'isU2TExport' And Id = Object_Id('" + this.MasterTableName + "'))";

        //                returnVal = objConnection.ExecuteScalar();

        //                if (returnVal != null)
        //                {
        //                    returnINT = 1;
        //                }
        //                else
        //                {
        //                    returnINT = 0;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            objConnection.CommandText = "select * from " + this.MasterTableName + " where isU2TExport is null and  (ac_group_name='" + AccountGroupParent + "' or [group]='" + AccountGroupParent + "')";
        //            ReturnDataSet = objConnection.GetDataSet("Master");
        //        }

        //        if (returnINT > 0)
        //        {
        //            objConnection.CommandText = "select * from " + this.MasterTableName + " where isU2TExport is null and  (ac_group_name='" + AccountGroupParent + "' or [group]='" + AccountGroupParent + "')";
        //            ReturnDataSet = objConnection.GetDataSet("Master");
        //        }
        //    }
        //    if (GeneratingOptions.ToLower() == "Generated".ToLower())
        //    {
        //        objConnection.CommandText = "(Select [Name] From Syscolumns Where [Name] = 'isU2TExport' And Id = Object_Id('" + this.MasterTableName + "'))";

        //        returnVal = objConnection.ExecuteScalar();

        //        if (returnVal == null)
        //        {
        //            objConnection.CommandText = "Alter Table " + this.MasterTableName + " add isU2TExport Bit";

        //            returnINT = objConnection.ExecuteNonQuery();

        //            if (returnINT == -1)
        //            {
        //                objConnection.CommandText = "(Select [Name] From Syscolumns Where [Name] = 'isU2TExport' And Id = Object_Id('" + this.MasterTableName + "'))";

        //                returnVal = objConnection.ExecuteScalar();

        //                if (returnVal != null)
        //                {
        //                    returnINT = 1;
        //                }
        //                else
        //                {
        //                    returnINT = 0;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            objConnection.CommandText = "select * from " + this.MasterTableName + " where isU2TExport is null and  (ac_group_name='" + AccountGroupParent + "' or [group]='" + AccountGroupParent + "')";
        //            ReturnDataSet = objConnection.GetDataSet("Master");
        //        }

        //        if (returnINT > 0)
        //        {
        //            objConnection.CommandText = "select * from " + this.MasterTableName + " where isU2TExport is null and  (ac_group_name='" + AccountGroupParent + "' or [group]='" + AccountGroupParent + "')";
        //            ReturnDataSet = objConnection.GetDataSet("Master");
        //        }
        //    }
        //    if (GeneratingOptions.ToLower() == "All".ToLower())
        //    {
        //        objConnection.CommandText = "(Select [Name] From Syscolumns Where [Name] = 'isU2TExport' And Id = Object_Id('" + this.MasterTableName + "'))";

        //        returnVal = objConnection.ExecuteScalar();

        //        if (returnVal == null)
        //        {
        //            objConnection.CommandText = "Alter Table " + this.MasterTableName + " add isU2TExport Bit";

        //            returnINT = objConnection.ExecuteNonQuery();

        //            if (returnINT == -1)
        //            {
        //                objConnection.CommandText = "(Select [Name] From Syscolumns Where [Name] = 'isU2TExport' And Id = Object_Id('" + this.MasterTableName + "'))";

        //                returnVal = objConnection.ExecuteScalar();

        //                if (returnVal != null)
        //                {
        //                    returnINT = 1;
        //                }
        //                else
        //                {
        //                    returnINT = 0;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            objConnection.CommandText = "select * from " + this.MasterTableName + " where ac_group_name='" + AccountGroupParent + "' or [group]='" + AccountGroupParent + "'";
        //            ReturnDataSet = objConnection.GetDataSet("Master");
        //        }

        //        if (returnINT > 0)
        //        {
        //            objConnection.CommandText = "select * from " + this.MasterTableName + " where ac_group_name='" + AccountGroupParent + "' or [group]='" + AccountGroupParent + "'";
        //            ReturnDataSet = objConnection.GetDataSet("Master");
        //        }
        //    }

        //    return ReturnDataSet;
        //}

        ///// <summary>
        ///// To Get the Selected Company Account GroupMaster Table Data for Tally Version 9.0 as a DataSet
        ///// </summary>
        ///// <param name="_iGenerateMasterXML"></param>
        ///// <returns>DataSet</returns>
        //public DataSet GenerateAccountGroupMasterTV90(IGenerateMasterXML _iGenerateMasterXML)
        //{
        //    object returnVal = null;
        //    int returnINT = 0;
        //    objConnection.AppConnectionString = _iGenerateMasterXML.ConnectionString;
        //    objConnection.ClearParameters();
        //    objConnection.CommandTypeToExecute = CommandType.Text;
        //    if (GeneratingOptions.ToLower() == "Not Generated".ToLower())
        //    {

        //        objConnection.CommandText = "(Select [Name] From Syscolumns Where [Name] = 'isU2TExport' And Id = Object_Id('" + this.MasterTableName + "'))";

        //        returnVal = objConnection.ExecuteScalar();

        //        if (returnVal == null)
        //        {
        //            objConnection.CommandText = "Alter Table " + this.MasterTableName + " add isU2TExport Bit";

        //            returnINT = objConnection.ExecuteNonQuery();

        //            if (returnINT == -1)
        //            {
        //                objConnection.CommandText = "(Select [Name] From Syscolumns Where [Name] = 'isU2TExport' And Id = Object_Id('" + this.MasterTableName + "'))";

        //                returnVal = objConnection.ExecuteScalar();

        //                if (returnVal != null)
        //                {
        //                    returnINT = 1;
        //                }
        //                else
        //                {
        //                    returnINT = 0;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            objConnection.CommandText = "select * from  " + this.MasterTableName + " where isU2TExport is null";                    
        //            ReturnDataSet = objConnection.GetDataSet("Master");
        //        }

        //        if (returnINT > 0)
        //        {
        //            objConnection.CommandText = "select * from  " + this.MasterTableName + " where isU2TExport is null";                    
        //            ReturnDataSet = objConnection.GetDataSet("Master");
        //        }
        //    }
        //    if (GeneratingOptions.ToLower() == "Generated".ToLower())
        //    {
        //        objConnection.CommandText = "(Select [Name] From Syscolumns Where [Name] = 'isU2TExport' And Id = Object_Id('" + this.MasterTableName + "'))";

        //        returnVal = objConnection.ExecuteScalar();

        //        if (returnVal == null)
        //        {
        //            objConnection.CommandText = "Alter Table " + this.MasterTableName + " add isU2TExport Bit";

        //            returnINT = objConnection.ExecuteNonQuery();

        //            if (returnINT == -1)
        //            {
        //                objConnection.CommandText = "(Select [Name] From Syscolumns Where [Name] = 'isU2TExport' And Id = Object_Id('" + this.MasterTableName + "'))";

        //                returnVal = objConnection.ExecuteScalar();

        //                if (returnVal != null)
        //                {
        //                    returnINT = 1;
        //                }
        //                else
        //                {
        //                    returnINT = 0;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            objConnection.CommandText = "select * from  " + this.MasterTableName + " where isU2TExport is null";                    
        //            ReturnDataSet = objConnection.GetDataSet("Master");
        //        }

        //        if (returnINT > 0)
        //        {
        //            objConnection.CommandText = "select * from  " + this.MasterTableName + " where isU2TExport is null";                    
        //            ReturnDataSet = objConnection.GetDataSet("Master");
        //        }
        //    }
        //    if (GeneratingOptions.ToLower() == "All".ToLower())
        //    {
        //        objConnection.CommandText = "(Select [Name] From Syscolumns Where [Name] = 'isU2TExport' And Id = Object_Id('" + this.MasterTableName + "'))";

        //        returnVal = objConnection.ExecuteScalar();

        //        if (returnVal == null)
        //        {
        //            objConnection.CommandText = "Alter Table " + this.MasterTableName + " add isU2TExport Bit";

        //            returnINT = objConnection.ExecuteNonQuery();

        //            if (returnINT == -1)
        //            {
        //                objConnection.CommandText = "(Select [Name] From Syscolumns Where [Name] = 'isU2TExport' And Id = Object_Id('" + this.MasterTableName + "'))";

        //                returnVal = objConnection.ExecuteScalar();

        //                if (returnVal != null)
        //                {
        //                    returnINT = 1;
        //                }
        //                else
        //                {
        //                    returnINT = 0;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            objConnection.CommandText = "select * from  " + this.MasterTableName;                   
        //            ReturnDataSet = objConnection.GetDataSet("Master");
        //        }

        //        if (returnINT > 0)
        //        {
        //            objConnection.CommandText = "select * from  " + this.MasterTableName;                    
        //            ReturnDataSet = objConnection.GetDataSet("Master");
        //        }
        //    }

        //    return ReturnDataSet;
        //}

        /// <summary>
        /// To Update the Selected Company Master Table Data in isU2TExport column for Tally Version 9.0
        /// </summary>
        /// <param name="_iGenerateMasterXML"></param>
        /// <returns>int</returns>

        #endregion
    }
}
