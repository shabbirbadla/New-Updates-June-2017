using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Xml;
using U2TPlus.DAL;
using U2TPlus.DAL.Interface;
using U2TPlus.BAL;

namespace U2TPlus.BAL
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

        #endregion

        #region Constractor
        public GenerateMasterXML()
        {

        }
        #endregion

        #region Globle Veriables of the Class

        StringBuilder GenerateMasterXMLBuilder = new StringBuilder();
        DataSet DSMaster = null;
        //DataSet DSGroupMaster = null;
        U2TPlus.DAL.GenerateMasterXML objGenerateMasterXML = new U2TPlus.DAL.GenerateMasterXML();
        CommonFunctions CFunctions = new CommonFunctions();
        DataTable dTable = new DataTable();
        int CloseIndex = 0;
        int StartIndex = 0;
        StringBuilder RecursionBuilder = new StringBuilder();

        #endregion

        #region Private Methods

        /// <summary>
        /// To Get the Selected Company Account Group Master Table Field Value
        /// </summary>
        /// <param name="_iGenerateMasterXML"></param>
        /// <returns>string</returns>
        private string GetFieldValue(Dictionary<string, string> agmDictionary, string KeyToGetValue)
        {
            string strTblDtls = string.Empty;
            string[] splitstring = null;
            string fieldValue = string.Empty;
            try
            {
                agmDictionary.TryGetValue(KeyToGetValue.ToUpper(), out strTblDtls);
                splitstring = strTblDtls.Split(new char[] { '.' });
                if (splitstring.Length > 0)
                {
                    foreach (string strvalue in splitstring)
                    {
                        if (strvalue.Contains("U2T"))
                        {
                        }
                        else
                            fieldValue = strvalue;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
            return fieldValue;
        }

        /// <summary>
        /// To Get the Selected Company Settings are Default or Field
        /// </summary>
        /// <param name="_iGenerateMasterXML"></param>
        /// <returns>string</returns>
        private string VerifyDefaultOrField(string[] DefaultValue, string[] FieldValue, StringBuilder verifyString)
        {
            string returnString = string.Empty;
            try
            {
                if (Convert.ToString(verifyString) != "" && Convert.ToString(verifyString) != null)
                {
                    foreach (string value in DefaultValue)
                    {
                        if (value != null && value != "")
                        {
                            if (value.ToUpper() == verifyString.ToString().ToUpper())
                            {
                                returnString = "Default";
                                break;
                            }
                        }
                    }

                    foreach (string value in FieldValue)
                    {
                        if (value != null && value != "")
                        {
                            if (value.ToUpper() == verifyString.ToString().ToUpper())
                            {
                                returnString = "Field";
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
            return returnString;
        }

        /// <summary>
        /// Recurcive mathod for Acount Group Master to get data as parent chaild reletion as tree stucture
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="Parent"></param>        
        private void getAccountGroupMasterChild(DataSet ds, string Parent)
        {
            try
            {
                DataTable dt = new DataTable();
                foreach (DataColumn dCloumn in ds.Tables[0].Columns)
                {
                    DataColumn dc1 = new DataColumn();
                    dc1.ColumnName = dCloumn.ColumnName;
                    dt.Columns.Add(dc1);
                }
                foreach (DataRow drows in ds.Tables[0].Rows)
                {
                    if (drows["group"].ToString().Trim() == Parent)
                    {
                        DataRow dRow = dt.NewRow();
                        for (int i = 0; i < drows.ItemArray.Length; i++)
                        {
                            dRow[i] = drows[i];
                        }
                        dt.Rows.Add(dRow);
                    }
                }
                foreach (DataRow item in dt.Rows)
                {
                    DataRow dRow = dTable.NewRow();
                    for (int i = 0; i < item.ItemArray.Length; i++)
                    {
                        dRow[i] = item[i];
                    }
                    dTable.Rows.Add(dRow);
                    getAccountGroupMasterChild(ds, item["ac_group_name"].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                mdbError = ex.Message;
                Logger.LogInfo(ex);
            }
        }

        /// <summary>
        /// Recurcive mathod for Item Group Master to get data as parent chaild reletion as tree stucture
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="Parent"></param>        
        private void getItemGroupMasterChild(DataSet ds, string Parent)
        {
            try
            {
                DataTable dt = new DataTable();
                foreach (DataColumn dCloumn in ds.Tables[0].Columns)
                {
                    DataColumn dc1 = new DataColumn();
                    dc1.ColumnName = dCloumn.ColumnName;
                    dt.Columns.Add(dc1);
                }
                foreach (DataRow drows in ds.Tables[0].Rows)
                {
                    if (drows["group"].ToString().Trim() == Parent)
                    {
                        DataRow dRow = dt.NewRow();
                        for (int i = 0; i < drows.ItemArray.Length; i++)
                        {
                            dRow[i] = drows[i];
                        }
                        dt.Rows.Add(dRow);
                    }
                }

                foreach (DataRow item in dt.Rows)
                {
                    if (dTable.Rows.Count < ds.Tables[0].Rows.Count)
                    {
                        DataRow dRow = dTable.NewRow();
                        for (int i = 0; i < item.ItemArray.Length; i++)
                        {
                            dRow[i] = item[i];
                        }
                        dTable.Rows.Add(dRow);
                    }
                }
            }

            catch (Exception ex)
            {
                mdbError = ex.Message;
                Logger.LogInfo(ex);
            }
        }

        private string getfieldValString(string agmDictionary)
        {
            string[] _splitString = null;
            string fieldVal = string.Empty;
            List<string> firstTag = new List<string>();
            try
            {
                _splitString = agmDictionary.Split(new char[] { ' ' });
                foreach (string sstring in _splitString)
                {
                    firstTag.Add(sstring);
                }

                if (firstTag.Capacity > 0)
                {
                    foreach (string collname in firstTag)
                    {
                        if (collname.Contains("U2T"))
                        {
                            int _ind = collname.IndexOf("U2T");
                            int _sInd = collname.IndexOf('.');

                            fieldVal = collname.Substring(_sInd + 1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
            return fieldVal;
        }

        private string ReplaceRowValue(string Value, string replaceValue, string tableName, string columnName)
        {
            string returnValue = string.Empty;
            try
            {

                returnValue = Value.Replace("U2T_" + tableName + "." + columnName, "'" + replaceValue + "'");
                return returnValue;
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
            return returnValue;
        }

        private void Recreasion(Dictionary<string, string> dictionary, string[] tagslsit, string tallyKey, string[] Fieldvalues, string[] defaultvalues, DataRow row)
        {
            bool needtocall = false;
            StringBuilder strtallyKey = new StringBuilder();
            try
            {
                strtallyKey.Append(tallyKey);
                if (dictionary.ContainsKey("/" + strtallyKey.ToString().ToUpper()))
                {
                    for (int i = 0; i < tagslsit.Length; i++)
                    {
                        if (tagslsit[i].Contains("/" + strtallyKey.ToString().ToUpper()))
                        {
                            CloseIndex = i;
                            needtocall = true;
                            break;
                        }
                    }

                    for (int i = 0; i < tagslsit.Length; i++)
                    {
                        if (tagslsit[i].Contains(strtallyKey.ToString().ToUpper()))
                        {
                            if ("/" + tagslsit[i] == tagslsit[CloseIndex])
                            {
                                StartIndex = i;
                                break;
                            }

                        }
                    }
                    if (StartIndex != 0)
                    {
                        GenerateMasterXMLBuilder.AppendLine("<" + tagslsit[StartIndex].ToUpper() + ">");// + fieldVal.Trim() + "</" + key.ToUpper() + ">");                                      
                    }
                }
                else
                {
                    if (needtocall)
                    {
                        if (tallyKey.ToUpper().Contains("/"))
                        {
                            tallyKey = tallyKey.Replace("/", "");
                            GenerateMasterXMLBuilder.AppendLine("</" + tallyKey.ToUpper() + ">");
                        }
                        else
                        {
                            GenerateMasterXMLBuilder.AppendLine("</" + tallyKey.ToUpper() + ">");
                        }
                    }
                    else
                    {
                        if (!tallyKey.ToUpper().Contains("/"))
                        {
                            string fieldVal = string.Empty;
                            fieldVal = this.GetFieldValue(dictionary, tallyKey.ToUpper());
                            if (fieldVal != "")
                                fieldVal = CFunctions.convertSpecialChars(fieldVal);
                            GenerateMasterXMLBuilder.AppendLine("<" + tallyKey.ToUpper() + ">" + fieldVal + "</" + tallyKey.ToUpper() + ">");
                        }
                        else
                        {
                            tallyKey = tallyKey.Replace("/", "");
                            GenerateMasterXMLBuilder.AppendLine("</" + tallyKey.ToUpper() + ">");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
        }

        private void getDinamicXMLString(Dictionary<string, string> agmList, string[] Fieldvalues, string[] defaultvalues, DataRow row, string[] allTags, string key)
        {
            string fieldVal = string.Empty;
            string rowValue = string.Empty;
            StringBuilder strKey = new StringBuilder();
            string strSwappingFieldValue = string.Empty;
            try
            {
                strKey.Append(key.ToUpper());
                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, strKey) == "Field")
                {
                    fieldVal = this.GetFieldValue(agmList, key.ToUpper());
                    agmList.TryGetValue(key.ToUpper(), out strSwappingFieldValue);

                    if (fieldVal != "")
                    {
                        if (row[fieldVal] != null)
                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                        foreach (string Swapp in ApplicationValues.AccountGroupSwappingDataTables)
                        {
                            if (Swapp.ToUpper().Trim() == strSwappingFieldValue.ToUpper().Trim())
                            {
                                rowValue = DataMemberSwapping(rowValue, strSwappingFieldValue, "Accounts Group");
                                break;
                            }
                        }
                        foreach (string Swapp in ApplicationValues.AccountSwappingDataTables)
                        {
                            if (Swapp.ToUpper().Trim() == strSwappingFieldValue.ToUpper().Trim())
                            {
                                rowValue = DataMemberSwapping(rowValue, strSwappingFieldValue, "Accounts");
                                break;
                            }
                        }
                    }
                    GenerateMasterXMLBuilder.AppendLine("<" + key.ToUpper() + ">" + rowValue.Trim() + "</" + key.ToUpper() + ">");
                }
                else
                {
                    fieldVal = this.GetFieldValue(agmList, key.ToUpper());
                    if (fieldVal != "")
                        fieldVal = CFunctions.convertSpecialChars(fieldVal);

                    if (fieldVal == "" && key.Contains("U2T"))//tag have attributies
                    {
                        fieldVal = this.getfieldValString(key.ToUpper());
                        strSwappingFieldValue = getDataMemberFieldValue(key.ToUpper());
                        if (fieldVal != "")
                        {
                            if (row[fieldVal] != null)
                            {
                                rowValue = rowValue.ToUpper();
                                rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                foreach (string Swapp in ApplicationValues.AccountGroupSwappingDataTables)
                                {
                                    if (Swapp.ToUpper().Trim() == strSwappingFieldValue.ToUpper().Trim())
                                    {
                                        rowValue = DataMemberSwapping(rowValue, strSwappingFieldValue, "Accounts Group");
                                        break;
                                    }
                                }
                                foreach (string Swapp in ApplicationValues.AccountSwappingDataTables)
                                {
                                    if (Swapp.ToUpper().Trim() == strSwappingFieldValue.ToUpper().Trim())
                                    {
                                        rowValue = DataMemberSwapping(rowValue, strSwappingFieldValue, "Accounts");
                                        break;
                                    }
                                }
                            }
                        }
                        string NewKey = ReplaceRowValue(key, rowValue.Trim(), ApplicationValues.tableName, fieldVal);
                        GenerateMasterXMLBuilder.AppendLine("<" + NewKey + ">");
                    }
                    else if (key.Split(new char[] { ' ' }).Length > 1 && isNeedToUpdate == false)
                        GenerateMasterXMLBuilder.AppendLine("<" + key + ">");
                    else
                        Recreasion(agmList, allTags, key, Fieldvalues, defaultvalues, row);
                }
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
        }

        private string getDataMemberFieldValue(string DatMemberFieldValue)
        {

            string[] _splitString = null;
            string fieldVal = string.Empty;
            List<string> firstTag = new List<string>();
            try
            {
                _splitString = DatMemberFieldValue.Split(new char[] { ' ' });
                foreach (string sstring in _splitString)
                {
                    firstTag.Add(sstring);
                }

                if (firstTag.Capacity > 0)
                {
                    foreach (string collname in firstTag)
                    {
                        if (collname.Contains("U2T"))
                        {
                            int val = collname.IndexOf("=");
                            int val1 = collname.Length;
                            fieldVal = collname.Substring(val + 1, (val1 - val)-1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }
            return fieldVal;
        }

        private string DataMemberSwapping(string rowValue, string strSwappingFieldValue, string DataMappingTableName)
        {
            try
            {
                if (Directory.Exists(ApplicationValues.DataMappingFolderName))
                {
                    FileInfo fInfo = new FileInfo(ApplicationValues.DataMappingFolderName + @"\" + ApplicationValues.DataMappingFileName);

                    if (fInfo.Exists)
                    {
                        DataSet DSDataMenber = new DataSet();
                        DSDataMenber.ReadXml(fInfo.FullName);
                        string MasterID = string.Empty;
                        if (DSDataMenber != null)
                        {

                            foreach (DataRow MASTERROW in DSDataMenber.Tables["MASTERTYPE"].Rows)
                            {
                                if (MASTERROW["Name"].ToString().ToUpper().Trim() == DataMappingTableName.ToUpper().Trim())
                                    MasterID = MASTERROW["MASTERTYPE_id"].ToString();
                            }
                            if (MasterID != "")
                            {
                                string filter = "MASTERTYPE_id=" + MasterID;
                                DataRow[] rows = DSDataMenber.Tables["DATAMAPPING"].Select(filter);

                                foreach (DataRow row in rows)
                                {
                                    if (row["UdyogValue"].ToString().ToUpper().Trim() == rowValue.ToUpper().Trim())
                                    {
                                        rowValue = row["TallyValue"].ToString().Trim();
                                        rowValue = CFunctions.convertSpecialChars(rowValue);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }

            return rowValue;
        }

        private string getFieldValue(string Value)
        {
            string _fieldvalue = string.Empty;
            string[] splitstring = null;

            try
            {
                if (Value != "")
                {
                    splitstring = Value.Split(new char[] { ' ' });
                    if (splitstring.Length > 0)
                    {
                        foreach (string strvalue in splitstring)
                        {
                            if (strvalue.Contains("U2T"))
                            {
                                string[] newString = new string[splitstring.Length + 1];
                                newString = strvalue.Split(new char[] { '"' });
                                foreach (string getTable in newString)
                                {
                                    if (getTable.Contains("U2T"))
                                    {
                                        string[] getMainTable = new string[newString.Length + 1];
                                        getMainTable = getTable.Split(new char[] { '.' });
                                        foreach (string getNewTable in getMainTable)
                                        {
                                            _fieldvalue = getNewTable;
                                            int num1 = _fieldvalue.IndexOf('_', 0);
                                            _fieldvalue = _fieldvalue.Substring(num1 + 1, (_fieldvalue.Length - (num1 + 1)));
                                            break;
                                        }
                                    }
                                    if (_fieldvalue != string.Empty)
                                    {
                                        break;
                                    }
                                }
                                if (_fieldvalue != string.Empty)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }


            return _fieldvalue;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// To Get the Selected Company Account Master Table Data for Tally Version 9.0 as a String Builder
        /// </summary>
        /// <param name="_iGenerateMasterXML"></param>
        /// <returns>StringBuilder</returns> 
        public StringBuilder GenareteMaster()
        {
            Dictionary<string, string> agmList = new Dictionary<string, string>();
            string[] allTags = null;
            string tablename = string.Empty;
            string fieldValue = string.Empty;
            string strTblDtls = string.Empty;
            string[] Fieldvalues = null;
            string[] defaultvalues = null;
            mdbError = string.Empty;
            try
            {
                int fieldCount = 0;
                int defaultCount = 0;
                if (TallyTagsTable != null)
                {
                    Fieldvalues = new string[TallyTagsTable.Rows.Count];
                    defaultvalues = new string[TallyTagsTable.Rows.Count];
                    #region For Exicuting with isGeneratesDefaultSettings as false

                    if (isGeneratesDefaultSettings == false)
                    {
                        DataRow[] rows = TallyTagsTable.Select("CONFIGURATION_id=" + Configuration_ID);
                        agmList = new Dictionary<string, string>();
                        foreach (DataRow row in rows)
                        {
                            if (row["FIELDVALUE"].ToString() != "" && row["FIELDVALUE"] != null)
                            {
                                agmList.Add(row["Name"].ToString().ToUpper(), row["FIELDVALUE"].ToString().ToUpper());
                                Fieldvalues[fieldCount] = row["Name"].ToString().ToUpper();
                                fieldCount = fieldCount + 1;
                            }
                            else
                            {
                                agmList.Add(row["Name"].ToString().ToUpper(), row["DefaultValues"].ToString().ToUpper());
                                defaultvalues[defaultCount] = row["Name"].ToString().ToUpper();
                                defaultCount = defaultCount + 1;
                            }
                        }

                        allTags = new string[agmList.Count];
                        agmList.Keys.CopyTo(allTags, 0);
                    }

                    #endregion
                    #region For Exicuting with isGeneratesDefaultSettings as true

                    else if (isGeneratesDefaultSettings == true)
                    {
                        DataRow[] rows = TallyTagsTable.Select("MASTERTYPE_id=" + Configuration_ID);
                        agmList = new Dictionary<string, string>();
                        foreach (DataRow row in rows)
                        {
                            if (row["FIELDVALUE"].ToString() != "" && row["FIELDVALUE"] != null)
                            {
                                agmList.Add(row["Name"].ToString().ToUpper(), row["FIELDVALUE"].ToString().ToUpper());
                                Fieldvalues[fieldCount] = row["Name"].ToString().ToUpper();
                                fieldCount = fieldCount + 1;
                            }
                            else
                            {
                                agmList.Add(row["Name"].ToString().ToUpper(), row["DefaultValues"].ToString().ToUpper());
                                defaultvalues[defaultCount] = row["Name"].ToString().ToUpper();
                                defaultCount = defaultCount + 1;
                            }
                        }

                        allTags = new string[agmList.Count];
                        agmList.Keys.CopyTo(allTags, 0);
                    }

                    #endregion
                }

                DSMaster = new DataSet();
                objGenerateMasterXML.ConnectionString = ApplicationValues.CompanyDBConnectionString;
                objGenerateMasterXML.CompanyDBName = ApplicationValues.CompanyDBName;
                objGenerateMasterXML.GeneratingOptions = this.GeneratingOptions;

                if (fieldCount != 0)
                {
                    isNeedToUpdate = true;

                    #region Geting database Table Name based on the Fist Tally tag in the XML
                    if (isGeneratesDefaultSettings == true)
                    {
                        DataRow[] rows = TallyTagsTable.Select("MASTERTYPE_id=" + Configuration_ID);
                        foreach (DataRow row in rows)
                        {
                            strTblDtls = row["Name"].ToString();
                            break;
                        }
                        if (strTblDtls != "")
                        {
                            ApplicationValues.tableName = getFieldValue(strTblDtls);
                        }
                    }
                    else
                    {
                        DataRow[] rows = TallyTagsTable.Select("Configuration_ID=" + Configuration_ID);
                        foreach (DataRow row in rows)
                        {
                            strTblDtls = row["Name"].ToString();
                            break;
                        }
                        if (strTblDtls != "")
                        {
                            ApplicationValues.tableName = getFieldValue(strTblDtls);
                        }
                    }
                    #endregion

                    if (ApplicationValues.tableName != "" && ApplicationValues.tableName != null)
                    {
                        objGenerateMasterXML.MasterTableName = ApplicationValues.tableName;
                        objGenerateMasterXML.TallyTagsTable = this.TallyTagsTable;
                        DSMaster = objGenerateMasterXML.GenerateMaster(this);
                        string rowValue = string.Empty;
                        string fieldVal = string.Empty;
                        GenerateMasterXMLBuilder = new StringBuilder();
                        #region for Account Group Master
                        if (ApplicationValues.GenaretingMasterName.ToUpper() == "Account Group Master".ToUpper())
                        {
                            if (DSMaster != null && DSMaster.Tables["Master"].Rows.Count > 0)
                            {
                                if (DSMaster.Tables.Count > 0)
                                {
                                    dTable = new DataTable();
                                    foreach (DataColumn dCloumn in DSMaster.Tables[0].Columns)
                                    {
                                        DataColumn dc1 = new DataColumn();
                                        dc1.ColumnName = dCloumn.ColumnName;
                                        dTable.Columns.Add(dc1);
                                    }
                                    getAccountGroupMasterChild(DSMaster, string.Empty);
                                }
                                if (dTable != null)
                                {
                                    foreach (DataRow row in dTable.Rows)
                                    {
                                        GenerateMasterXMLBuilder.AppendLine("<TALLYMESSAGE xmlns:UDF='TallyUDF'>");
                                        for (int i = 0; i < allTags.Length; i++)
                                        {
                                            getDinamicXMLString(agmList, Fieldvalues, defaultvalues, row, allTags, allTags[i].ToString());
                                        }
                                        GenerateMasterXMLBuilder.AppendLine("</TALLYMESSAGE>");
                                    }
                                }
                            }
                            else
                            {
                                Logger.LogInfo("the Account Group Master table will not have sufficient data, please check the data base table and try once againe.");
                                dbError = "the Account Group Master table will not have sufficient data, please check the data base table and try once againe.";
                            }
                        }
                        #endregion
                        #region For Item Group Master
                        else if (ApplicationValues.GenaretingMasterName.ToUpper() == "Item Group Master".ToUpper())
                        {
                            if (DSMaster != null && DSMaster.Tables["Master"].Rows.Count > 0)
                            {
                                if (DSMaster.Tables.Count > 0)
                                {
                                    dTable = new DataTable();
                                    foreach (DataColumn dCloumn in DSMaster.Tables[0].Columns)
                                    {
                                        DataColumn dc1 = new DataColumn();
                                        dc1.ColumnName = dCloumn.ColumnName;
                                        dTable.Columns.Add(dc1);
                                    }
                                    getItemGroupMasterChild(DSMaster, string.Empty);
                                }
                                if (dTable != null)
                                {
                                    foreach (DataRow row in dTable.Rows)
                                    {
                                        GenerateMasterXMLBuilder.AppendLine("<TALLYMESSAGE xmlns:UDF='TallyUDF'>");
                                        for (int i = 0; i < allTags.Length; i++)
                                        {
                                            getDinamicXMLString(agmList, Fieldvalues, defaultvalues, row, allTags, allTags[i].ToString());
                                        }
                                        GenerateMasterXMLBuilder.AppendLine("</TALLYMESSAGE>");
                                    }
                                }
                            }
                            else
                            {
                                Logger.LogInfo("the Item Group Master table will not have sufficient data, please check the data base table and try once againe.");
                                dbError = "the Item Group Master table will not have sufficient data, please check the data base table and try once againe.";
                            }
                        }
                        #endregion
                        #region For Rest Of The Masters that is : Account Master, Item Master
                        else
                        {
                            if (DSMaster != null && DSMaster.Tables["Master"].Rows.Count > 0)
                            {
                                foreach (DataRow row in DSMaster.Tables[0].Rows)
                                {
                                    GenerateMasterXMLBuilder.AppendLine("<TALLYMESSAGE xmlns:UDF='TallyUDF'>");
                                    for (int i = 0; i < allTags.Length; i++)
                                    {
                                        getDinamicXMLString(agmList, Fieldvalues, defaultvalues, row, allTags, allTags[i].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("</TALLYMESSAGE>");
                                }
                            }
                            else
                            {
                                Logger.LogInfo("the " + ApplicationValues.GenaretingMasterName + " table will not have sufficient data, please check the data base table and try once againe.");
                                dbError = "the " + ApplicationValues.GenaretingMasterName + " table will not have sufficient data, please check the data base table and try once againe.";
                            }
                        }
                        #endregion
                    }
                }
                else
                {
                    isNeedToUpdate = false;
                    #region For All The Masters with Default Settings
                    if (DSMaster != null)
                    {
                        DataRow row = null;
                        GenerateMasterXMLBuilder.AppendLine("<TALLYMESSAGE xmlns:UDF='TallyUDF'>");
                        for (int i = 0; i < allTags.Length; i++)
                        {
                            getDinamicXMLString(agmList, Fieldvalues, defaultvalues, row, allTags, allTags[i].ToString());
                        }
                        GenerateMasterXMLBuilder.AppendLine("</TALLYMESSAGE>");

                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                dbError = ex.Message;
                Logger.LogInfo(ex);
            }

            return GenerateMasterXMLBuilder;

        }

        /// <summary>
        /// To Update the Selected Company Master Table
        /// </summary>
        /// <param name="_iGenerateMasterXML"></param>
        /// <returns>int</returns>
        public int UpdateGenerateMaster()
        {
            int returnValue = 0;
            objGenerateMasterXML.ConnectionString = ApplicationValues.CompanyDBConnectionString;
            objGenerateMasterXML.CompanyDBName = ApplicationValues.CompanyDBName;
            objGenerateMasterXML.MasterTableName = ApplicationValues.tableName;
            objGenerateMasterXML.TallyTagsTable = this.TallyTagsTable;
            returnValue = objGenerateMasterXML.UpdateGenerateMaster(this);
            Logger.LogInfo(mdbError);
            return returnValue;
        }

        #endregion
    }
}
