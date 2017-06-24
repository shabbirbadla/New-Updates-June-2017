using System;
using System.Collections.Generic;
using System.Text;
using U2TPlus.DAL;
using U2TPlus.DAL.Interface;
using U2TPlus.BAL;
using System.Data;
using System.IO;
using System.Xml;

namespace U2TPlus.BAL
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

        public GenerateTransXML()
        {

        }
        #endregion

        #region Globle Veriables of the Class

        StringBuilder GenerateTransXMLBuilder = new StringBuilder();
        DataSet DSMaster = null;
        DAL.GenerateTransXML objGenerateTransXML = new U2TPlus.DAL.GenerateTransXML();
        CommonFunctions CFunctions = new CommonFunctions();
        DataTable dTable = new DataTable();
        int CloseIndex = 0;
        int StartIndex = 0;
        string[] _LedgerEntriesList = null;
        string[] _AllInventoryEntriesList = null;
        string[] _LedgerEntriesOrderList = null;
        string[] _AllInventoryEntriesOrderList = null;
        StringBuilder RecursionBuilder = new StringBuilder();
        string GlobleGUIDString = string.Empty;
        bool needToProced = false;

        #endregion

        #region Public Methods

        public StringBuilder GenerateTransactionXML()
        {
            string[] TableNamesList = Enum.GetNames(typeof(TransTableNames));

            Dictionary<string, string> TagsList = new Dictionary<string, string>();
            Dictionary<string, string> ValuesList = new Dictionary<string, string>();
            string[] allTags = null;
            string[] allTagsOrders = null;
            string tablename = string.Empty;
            string fieldValue = string.Empty;
            string strTblDtls = string.Empty;
            string[] Fieldvalues = null;
            string[] defaultvalues = null;
            dbError = string.Empty;

            try
            {
                int fieldCount = 0;
                int defaultCount = 0;

                if (TallyTagsTable != null)
                {
                    Fieldvalues = new string[TallyTagsTable.Rows.Count];
                    defaultvalues = new string[TallyTagsTable.Rows.Count];

                    //For Exicuting with isGeneratesDefaultSettings as false
                    #region For Exicuting with isGeneratesDefaultSettings as false
                    if (isGeneratesDefaultSettings == false)
                    {
                        DataRow[] rows = TallyTagsTable.Select("CONFIGURATION_id=" + Configuration_ID);
                        TagsList = new Dictionary<string, string>();

                        foreach (DataRow row in rows)
                        {
                            if (row["FIELDVALUE"].ToString() != "" && row["FIELDVALUE"] != null)
                            {
                                TagsList.Add(row["Order"].ToString().ToUpper(), row["Name"].ToString().ToUpper());
                                ValuesList.Add(row["Order"].ToString().ToUpper(), row["FIELDVALUE"].ToString().ToUpper());
                                Fieldvalues[fieldCount] = row["Name"].ToString().ToUpper();
                                fieldCount = fieldCount + 1;
                            }
                            else
                            {
                                TagsList.Add(row["Order"].ToString().ToUpper(), row["Name"].ToString().ToUpper());
                                ValuesList.Add(row["Order"].ToString().ToUpper(), row["DefaultValues"].ToString().ToUpper());
                                defaultvalues[defaultCount] = row["Name"].ToString().ToUpper();
                                defaultCount = defaultCount + 1;
                            }
                        }

                        allTags = new string[TagsList.Count];
                        allTagsOrders = new string[TagsList.Count];
                        TagsList.Values.CopyTo(allTags, 0);
                        TagsList.Keys.CopyTo(allTagsOrders, 0);
                    }
                    #endregion
                    //For Exicuting with isGeneratesDefaultSettings as true
                    #region For Exicuting with isGeneratesDefaultSettings as true
                    else if (isGeneratesDefaultSettings == true)
                    {
                        DataRow[] rows = TallyTagsTable.Select("MASTERTYPE_id=" + Configuration_ID);
                        TagsList = new Dictionary<string, string>();
                        foreach (DataRow row in rows)
                        {
                            if (row["FIELDVALUE"].ToString() != "" && row["FIELDVALUE"] != null)
                            {
                                TagsList.Add(row["Order"].ToString().ToUpper(), row["Name"].ToString().ToUpper());
                                ValuesList.Add(row["Order"].ToString().ToUpper(), row["FIELDVALUE"].ToString().ToUpper());
                                Fieldvalues[fieldCount] = row["Name"].ToString().ToUpper();
                                fieldCount = fieldCount + 1;
                            }
                            else
                            {
                                TagsList.Add(row["Order"].ToString().ToUpper(), row["Name"].ToString());
                                ValuesList.Add(row["Order"].ToString().ToUpper(), row["DefaultValues"].ToString());
                                defaultvalues[defaultCount] = row["Name"].ToString().ToUpper();
                                defaultCount = defaultCount + 1;
                            }
                        }

                        allTags = new string[TagsList.Count];
                        allTagsOrders = new string[TagsList.Count];
                        TagsList.Values.CopyTo(allTags, 0);
                        TagsList.Keys.CopyTo(allTagsOrders, 0);
                    }
                    #endregion

                    DSMaster = new DataSet();
                    objGenerateTransXML.ConnectionString = ApplicationValues.CompanyDBConnectionString;
                    objGenerateTransXML.CompanyDBName = ApplicationValues.CompanyDBName;
                    objGenerateTransXML.GeneratingOptions = this.GeneratingOptions;
                    objGenerateTransXML.GenerateFromDate = this.GenerateFromDate;
                    objGenerateTransXML.GenerateToDate = this.GenerateToDate;

                    if (fieldCount != 0)
                    {
                        isNeedToUpdate = true;
                        int tableNumber = 0;
                        foreach (string tblName in TableNamesList)
                        {
                            objGenerateTransXML.PresentTransTableName = ApplicationValues.TransEntryType + tblName;
                            objGenerateTransXML.TallyTagsTable = this.TallyTagsTable;
                            objGenerateTransXML.GenerateFromDate = this.GenerateFromDate;
                            objGenerateTransXML.GenerateToDate = this.GenerateToDate;
                            DSMaster.Tables.Add(objGenerateTransXML.GenerateTransactionXML(this).Tables[objGenerateTransXML.PresentTransTableName].Copy());
                            mTableNames[tableNumber] = objGenerateTransXML.PresentTransTableName;
                            tableNumber = tableNumber + 1;
                        }
                        objGenerateTransXML.tablename = mTableNames;
                        string rowValue = string.Empty;
                        string fieldVal = string.Empty;
                        GenerateTransXMLBuilder = new StringBuilder();
                        bool breakloop = true;
                        if (DSMaster != null)
                        {
                            string _proced = string.Empty;
                            _proced = allTablesHaveData(DSMaster, ValuesList);
                            if (needToProced)
                            {
                                foreach (DataRow row in DSMaster.Tables[0].Rows)
                                {
                                    GenerateTransXMLBuilder.AppendLine("<TALLYMESSAGE xmlns:UDF='TallyUDF'>");
                                    for (int i = 0; i < allTags.Length; i++)
                                    {
                                        if (dbError != null && dbError != "")
                                        {
                                            breakloop = false;
                                            break;
                                        }
                                        else
                                        {
                                            #region For LEDGERENTRIES.LIST
                                            if (allTags[i].ToString().ToUpper() == "LEDGERENTRIES.LIST".ToString().ToUpper())
                                            {
                                                int _CloseIndex = 0;
                                                int _StartIndex = 0;
                                                for (int _count = i; _count < allTags.Length; _count++)
                                                {
                                                    if (allTags[_count].Contains("/" + "LEDGERENTRIES.LIST".ToUpper()))
                                                    {
                                                        _CloseIndex = _count;
                                                        break;
                                                    }
                                                }

                                                for (int _count = i; _count < allTags.Length; _count++)
                                                {
                                                    if (allTags[_count].Contains("LEDGERENTRIES.LIST".ToUpper()))
                                                    {
                                                        if ("/" + "LEDGERENTRIES.LIST".ToUpper() == allTags[_CloseIndex])
                                                        {
                                                            _StartIndex = _count;
                                                            break;
                                                        }
                                                    }
                                                }

                                                if (_StartIndex != 0)
                                                {
                                                    int diff = _CloseIndex - _StartIndex;
                                                    _LedgerEntriesList = new string[diff + 1];
                                                    _LedgerEntriesOrderList = new string[diff + 1];
                                                    for (int _count = 0; _count <= diff; _count++)
                                                    {
                                                        if (_StartIndex <= _CloseIndex)
                                                        {
                                                            _LedgerEntriesList[_count] = allTags[_StartIndex];
                                                            _LedgerEntriesOrderList[_count] = allTagsOrders[_StartIndex];
                                                            _StartIndex = _StartIndex + 1;
                                                        }
                                                    }
                                                }

                                                DataRow[] acdetrows = DSMaster.Tables[ApplicationValues.TransEntryType + "ACDET"].Select("tran_cd=" + row["tran_cd"]);

                                                foreach (DataRow _row in acdetrows)
                                                {
                                                    if (_row["postord"].ToString().ToUpper().Trim() == "A".ToUpper())
                                                    {
                                                        for (int LedgerCount = 0; LedgerCount < _LedgerEntriesList.Length; LedgerCount++)
                                                        {
                                                            buildLedgerEntries(TagsList, ValuesList, Fieldvalues, defaultvalues, _row, allTags, _LedgerEntriesList[LedgerCount].ToString(), _LedgerEntriesOrderList[LedgerCount].ToString());
                                                        }
                                                    }
                                                    else if (_row["postord"].ToString().Contains("C".ToUpper()))
                                                    {
                                                        int startTagNumber = 0;
                                                        int EndTagNumber = 0;
                                                        for (int LedgerCount = 0; LedgerCount < _LedgerEntriesList.Length; LedgerCount++)
                                                        {
                                                            if (_LedgerEntriesList[LedgerCount].ToString() == "BILLALLOCATIONS.LIST")
                                                            {
                                                                startTagNumber = LedgerCount;

                                                                for (int _Count = LedgerCount; _Count < _LedgerEntriesList.Length; _Count++)
                                                                {
                                                                    if (_LedgerEntriesList[_Count].Contains("/BILLALLOCATIONS.LIST"))
                                                                    {
                                                                        EndTagNumber = _Count;
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                            if (startTagNumber == 0 && EndTagNumber == 0)
                                                            {
                                                                buildLedgerEntries(TagsList, ValuesList, Fieldvalues, defaultvalues, _row, allTags, _LedgerEntriesList[LedgerCount].ToString(), _LedgerEntriesOrderList[LedgerCount].ToString());
                                                                if (_LedgerEntriesList[LedgerCount].ToString() == "LEDGERENTRIES.LIST")
                                                                {
                                                                    GenerateTransXMLBuilder.AppendLine("<BASICRATEOFINVOICETAX.LIST>");
                                                                    GenerateTransXMLBuilder.AppendLine("<BASICRATEOFINVOICETAX>10</BASICRATEOFINVOICETAX>");
                                                                    GenerateTransXMLBuilder.AppendLine("</BASICRATEOFINVOICETAX.LIST>");
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (startTagNumber <= LedgerCount)
                                                                {
                                                                    if (EndTagNumber >= LedgerCount)
                                                                    {
                                                                        if (EndTagNumber == LedgerCount)
                                                                        {
                                                                            startTagNumber = 0;
                                                                            EndTagNumber = 0;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                i = _CloseIndex;
                                            }
                                            #endregion
                                            #region for ALLINVENTORYENTRIES.LIST
                                            else if (allTags[i].ToString().ToUpper() == "ALLINVENTORYENTRIES.LIST".ToString().ToUpper())
                                            {
                                                int _CloseIndex = 0;
                                                int _StartIndex = 0;
                                                for (int _count = i; _count < allTags.Length; _count++)
                                                {
                                                    if (allTags[_count].Contains("/" + "ALLINVENTORYENTRIES.LIST".ToUpper()))
                                                    {
                                                        _CloseIndex = _count;
                                                        break;
                                                    }
                                                }

                                                for (int _count = i; _count < allTags.Length; _count++)
                                                {
                                                    if (allTags[_count].Contains("ALLINVENTORYENTRIES.LIST".ToUpper()))
                                                    {
                                                        if ("/" + "ALLINVENTORYENTRIES.LIST".ToUpper() == allTags[_CloseIndex])
                                                        {
                                                            _StartIndex = _count;
                                                            break;
                                                        }
                                                    }
                                                }

                                                if (_StartIndex != 0)
                                                {
                                                    int diff = _CloseIndex - _StartIndex;
                                                    _AllInventoryEntriesList = new string[diff + 1];
                                                    _AllInventoryEntriesOrderList = new string[diff + 1];
                                                    for (int _count = 0; _count <= diff; _count++)
                                                    {
                                                        if (_StartIndex <= _CloseIndex)
                                                        {
                                                            _AllInventoryEntriesList[_count] = allTags[_StartIndex];
                                                            _AllInventoryEntriesOrderList[_count] = allTagsOrders[_StartIndex];
                                                            _StartIndex = _StartIndex + 1;
                                                        }
                                                    }
                                                }

                                                DataRow[] acdetrows = DSMaster.Tables[ApplicationValues.TransEntryType + "ACDET"].Select("tran_cd=" + row["tran_cd"]);
                                                DataRow[] itemrows = DSMaster.Tables[ApplicationValues.TransEntryType + "ITEM"].Select("tran_cd=" + row["tran_cd"]);
                                                foreach (DataRow _acdetrow in acdetrows)
                                                {
                                                    if (_acdetrow["postord"].ToString().ToUpper().Trim() == "B".ToUpper())
                                                    {
                                                        foreach (DataRow _itemRow in itemrows)
                                                        {
                                                            for (int InventoryCount = 0; InventoryCount < _AllInventoryEntriesList.Length; InventoryCount++)
                                                            {
                                                                buildInventoryEntries(TagsList, ValuesList, Fieldvalues, defaultvalues, _acdetrow, _itemRow, allTags, _AllInventoryEntriesList[InventoryCount].ToString(), _AllInventoryEntriesOrderList[InventoryCount].ToString());
                                                            }
                                                        }
                                                    }
                                                }

                                                i = _CloseIndex;
                                            }
                                            #endregion
                                            #region For Rest of the Tags
                                            else
                                            {
                                                getDinamicXMLString(TagsList, ValuesList, Fieldvalues, defaultvalues, row, allTags, allTags[i].ToString(), allTagsOrders[i].ToString());
                                            }
                                            #endregion
                                        }
                                    }
                                    if (breakloop == true)
                                        GenerateTransXMLBuilder.AppendLine("</TALLYMESSAGE>");
                                    else
                                    {
                                        breakloop = true;
                                        break;
                                    }

                                }// 
                            }
                            else
                            {
                                dbError = "the " + _proced.ToLower() + " data table will not have sufcient data, please verify database";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dbError = ex.Message;
                Logger.LogInfo(ex);
            }

            return GenerateTransXMLBuilder;
        }

        private string allTablesHaveData(DataSet DSMaster, Dictionary<string, string> ValuesList)
        {
            string _pro = string.Empty;
            for (int i = 0; i < DSMaster.Tables.Count; i++)
            {
                string datasetTableName = string.Empty;
                datasetTableName = DSMaster.Tables[i].TableName.Replace(ApplicationValues.TransEntryType, "");
                if (datasetTableName == TransTableNames.ACDET.ToString() || datasetTableName == TransTableNames.MAIN.ToString() || datasetTableName == TransTableNames.ITEM.ToString())
                {
                    if (DSMaster.Tables[i].Rows.Count > 0)
                    {
                        needToProced = true;
                    }
                    else
                    {
                        needToProced = false;
                        _pro = DSMaster.Tables[i].TableName;
                        break;
                    }
                }
            }
            return _pro;
        }

        public int UpdateGenerateTrans()
        {
            int returnValue = 0;
            objGenerateTransXML.ConnectionString = ApplicationValues.CompanyDBConnectionString;
            objGenerateTransXML.CompanyDBName = ApplicationValues.CompanyDBName;
            objGenerateTransXML.PresentTransTableName = ApplicationValues.tableName;
            objGenerateTransXML.TallyTagsTable = this.TallyTagsTable;
            returnValue = objGenerateTransXML.UpdateGenerateTrans(this);
            Logger.LogInfo(mdbError);
            return returnValue;
        }

        public string GetEntryType()
        {
            string Entry_Type = string.Empty;
            try
            {
                objGenerateTransXML.ConnectionString = ApplicationValues.CompanyDBConnectionString;
                objGenerateTransXML.GenaretingTransName = this.GenaretingTransName;
                Entry_Type = objGenerateTransXML.GetEntryType(this);
            }
            catch (Exception ex)
            {
                dbError = ex.Message;
                Logger.LogInfo(ex);
            }
            return Entry_Type;
        }

        #endregion

        #region Private Methods

        private void buildInventoryEntries(Dictionary<string, string> TagsList, Dictionary<string, string> ValuesList, string[] Fieldvalues, string[] defaultvalues, DataRow _acdetrow, DataRow _itemRow, string[] allTags, string key, string keyOrder)
        {
            string fieldVal = string.Empty;
            string rowValue = string.Empty;
            StringBuilder strKey = new StringBuilder();
            Dictionary<string, string> tabAndCol = new Dictionary<string, string>();
            string tableName = string.Empty;
            string columnName = string.Empty;
            string strSwappingDataValue = string.Empty;
            try
            {
                strKey.Append(key.ToUpper());
                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, strKey) == "Field")
                {
                    tabAndCol = this.getTableAndColumnName(TagsList, ValuesList, keyOrder);
                    this.getTableAndColumnName(TagsList, ValuesList, keyOrder).TryGetValue("table", out tableName);
                    this.getTableAndColumnName(TagsList, ValuesList, keyOrder).TryGetValue("column", out columnName);
                    ValuesList.TryGetValue(keyOrder, out strSwappingDataValue);
                    if (columnName != "")
                    {
                        if (tableName.ToLower() == ("ITEM").ToString().ToLower())
                        {
                            if (_itemRow[columnName] != null)
                                rowValue = CFunctions.convertSpecialChars(_itemRow[columnName].ToString());
                            if (columnName.ToUpper() == "U_ASSEAMT".ToUpper())
                            {
                                string amt_ty = _acdetrow["amt_ty"].ToString();
                                if (amt_ty.ToUpper() == "CR".ToUpper())
                                {
                                    rowValue = CFunctions.convertSpecialChars(_itemRow[columnName].ToString());
                                }
                                else
                                {
                                    rowValue = "-" + CFunctions.convertSpecialChars(_itemRow[columnName].ToString());
                                }
                            }
                        }
                        else if (tableName.ToLower() == ("ACDET").ToString().ToLower())
                        {
                            if (_acdetrow[columnName] != null)
                            {
                                rowValue = CFunctions.convertSpecialChars(_acdetrow[columnName].ToString());

                                if (columnName.ToUpper() == "AMOUNT".ToUpper())
                                {
                                    string amt_ty = _acdetrow["amt_ty"].ToString();
                                    if (amt_ty.ToUpper() == "CR".ToUpper())
                                    {
                                        rowValue = CFunctions.convertSpecialChars(_acdetrow[columnName].ToString());
                                    }
                                    else
                                    {
                                        rowValue = "-" + CFunctions.convertSpecialChars(_acdetrow[columnName].ToString());
                                    }
                                }
                            }
                        }
                    }
                    foreach (string Swapp in ApplicationValues.AccountGroupSwappingDataTables)
                    {
                        if (Swapp.ToUpper().Trim() == strSwappingDataValue.ToUpper().Trim())
                        {
                            rowValue = DataMemberSwapping(rowValue, strSwappingDataValue, "Accounts Group");
                            break;
                        }
                    }
                    foreach (string Swapp in ApplicationValues.AccountSwappingDataTables)
                    {
                        if (Swapp.ToUpper().Trim() == strSwappingDataValue.ToUpper().Trim())
                        {
                            rowValue = DataMemberSwapping(rowValue, strSwappingDataValue, "Accounts");
                            break;
                        }
                    }
                    GenerateTransXMLBuilder.AppendLine("<" + key.ToUpper() + ">" + rowValue.Trim() + "</" + key.ToUpper() + ">");
                }
                else
                {
                    fieldVal = this.GetFieldValue(TagsList, ValuesList, key.ToUpper(), keyOrder);
                    if (fieldVal != "")
                        fieldVal = CFunctions.convertSpecialChars(fieldVal);

                    if (fieldVal == "" && key.Contains("U2T"))//tag have attributies
                    {
                        fieldVal = this.getfieldValString(key.ToUpper());
                        strSwappingDataValue = getDataMemberFieldValue(key.ToUpper());
                        if (fieldVal != "")
                        {
                            if (_itemRow[fieldVal] != null)
                            {
                                rowValue = rowValue.ToUpper();
                                rowValue = CFunctions.convertSpecialChars(_itemRow[fieldVal].ToString());
                                foreach (string Swapp in ApplicationValues.AccountGroupSwappingDataTables)
                                {
                                    if (Swapp.ToUpper().Trim() == strSwappingDataValue.ToUpper().Trim())
                                    {
                                        rowValue = DataMemberSwapping(rowValue, strSwappingDataValue, "Accounts Group");
                                        break;
                                    }
                                }
                                foreach (string Swapp in ApplicationValues.AccountSwappingDataTables)
                                {
                                    if (Swapp.ToUpper().Trim() == strSwappingDataValue.ToUpper().Trim())
                                    {
                                        rowValue = DataMemberSwapping(rowValue, strSwappingDataValue, "Accounts");
                                        break;
                                    }
                                }
                            }
                        }
                        string NewKey = ReplaceRowValue(key, rowValue.Trim(), ApplicationValues.tableName, fieldVal);
                        GenerateTransXMLBuilder.AppendLine("<" + NewKey + ">");
                    }
                    else
                    {
                        Recreasion(TagsList, ValuesList, allTags, key, Fieldvalues, defaultvalues, keyOrder, _acdetrow);
                    }
                }
            }
            catch (Exception ex)
            {
                dbError = ex.Message;
                Logger.LogInfo(ex);
            }
        }

        private void buildLedgerEntries(Dictionary<string, string> TagsList, Dictionary<string, string> ValuesList, string[] Fieldvalues, string[] defaultvalues, DataRow row, string[] allTags, string key, string keyOrder)
        {
            string fieldVal = string.Empty;
            string rowValue = string.Empty;
            StringBuilder strKey = new StringBuilder();
            Dictionary<string, string> tabAndCol = new Dictionary<string, string>();
            string tableName = string.Empty;
            string columnName = string.Empty;
            string strSwappingDataValue = string.Empty;
            try
            {
                strKey.Append(key.ToUpper());
                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, strKey) == "Field")
                {
                    tabAndCol = this.getTableAndColumnName(TagsList, ValuesList, keyOrder);
                    this.getTableAndColumnName(TagsList, ValuesList, keyOrder).TryGetValue("table", out tableName);
                    this.getTableAndColumnName(TagsList, ValuesList, keyOrder).TryGetValue("column", out columnName);

                    ValuesList.TryGetValue(keyOrder, out strSwappingDataValue);
                    if (columnName != "")
                    {
                        if (tableName.ToLower() == ("ACDET").ToString().ToLower())
                        {
                            if (row[columnName] != null)
                            {
                                rowValue = CFunctions.convertSpecialChars(row[columnName].ToString());
                                if (columnName.ToUpper() == "AMOUNT".ToUpper())
                                {
                                    string amt_ty = row["amt_ty"].ToString();
                                    if (amt_ty.ToUpper() == "CR".ToUpper())
                                    {
                                        rowValue = CFunctions.convertSpecialChars(row[columnName].ToString());
                                    }
                                    else
                                    {
                                        rowValue = "-" + CFunctions.convertSpecialChars(row[columnName].ToString());
                                    }
                                }

                            }
                        }
                    }
                    foreach (string Swapp in ApplicationValues.AccountGroupSwappingDataTables)
                    {
                        if (Swapp.ToUpper().Trim() == strSwappingDataValue.ToUpper().Trim())
                        {
                            rowValue = DataMemberSwapping(rowValue, strSwappingDataValue, "Accounts Group");
                            break;
                        }
                    }
                    foreach (string Swapp in ApplicationValues.AccountSwappingDataTables)
                    {
                        if (Swapp.ToUpper().Trim() == strSwappingDataValue.ToUpper().Trim())
                        {
                            rowValue = DataMemberSwapping(rowValue, strSwappingDataValue, "Accounts");
                            break;
                        }
                    }
                    GenerateTransXMLBuilder.AppendLine("<" + key.ToUpper() + ">" + rowValue.Trim() + "</" + key.ToUpper() + ">");
                }
                else
                {
                    fieldVal = this.GetFieldValue(TagsList, ValuesList, key.ToUpper(), keyOrder);
                    if (fieldVal != "")
                        fieldVal = CFunctions.convertSpecialChars(fieldVal);

                    if (fieldVal == "" && key.Contains("U2T"))//tag have attributies
                    {
                        fieldVal = this.getfieldValString(key.ToUpper());
                        strSwappingDataValue = getDataMemberFieldValue(key.ToUpper());
                        if (fieldVal != "")
                        {
                            if (row[fieldVal] != null)
                            {
                                rowValue = rowValue.ToUpper();
                                rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                foreach (string Swapp in ApplicationValues.AccountGroupSwappingDataTables)
                                {
                                    if (Swapp.ToUpper().Trim() == strSwappingDataValue.ToUpper().Trim())
                                    {
                                        rowValue = DataMemberSwapping(rowValue, strSwappingDataValue, "Accounts Group");
                                        break;
                                    }
                                }
                                foreach (string Swapp in ApplicationValues.AccountSwappingDataTables)
                                {
                                    if (Swapp.ToUpper().Trim() == strSwappingDataValue.ToUpper().Trim())
                                    {
                                        rowValue = DataMemberSwapping(rowValue, strSwappingDataValue, "Accounts");
                                        break;
                                    }
                                }
                            }
                        }
                        string NewKey = ReplaceRowValue(key, rowValue.Trim(), ApplicationValues.tableName, fieldVal);
                        GenerateTransXMLBuilder.AppendLine("<" + NewKey + ">");
                    }
                    else
                    {
                        Recreasion(TagsList, ValuesList, allTags, key, Fieldvalues, defaultvalues, keyOrder, row);
                    }
                }
            }
            catch (Exception ex)
            {
                dbError = ex.Message;
                Logger.LogInfo(ex);
            }
        }

        private string getFieldValue(string strTblDtls, string[] tableNames)
        {
            string _fieldvalue = string.Empty;
            string[] splitstring = null;

            try
            {
                if (strTblDtls != "")
                {
                    splitstring = strTblDtls.Split('.');
                    if (splitstring.Length > 0)
                    {
                        foreach (string strvalue in splitstring)
                        {
                            if (strvalue.Contains("U2T"))
                            {
                                _fieldvalue = strvalue.Replace("U2T_", ApplicationValues.TransEntryType);
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dbError = ex.Message;
                Logger.LogInfo(ex);
            }

            return _fieldvalue;
        }

        private void Recreasion(Dictionary<string, string> Tagsdictionary, Dictionary<string, string> Valuesdictionary, string[] tagslsit, string tallyKey, string[] Fieldvalues, string[] defaultvalues, string tagOrderNumber, DataRow _itemsRow)
        {
            bool needtocall = false;
            StringBuilder strtallyKey = new StringBuilder();
            try
            {
                string[] firstvalue = tallyKey.Split(new char[] { ' ' });
                strtallyKey.Append(firstvalue[0].ToString());
                if (Tagsdictionary.ContainsValue("/" + strtallyKey.ToString().ToUpper()))
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
                        if (tagslsit[i].Contains(tallyKey.ToString().ToUpper()))
                        {
                            if ("/" + strtallyKey.ToString().ToUpper() == tagslsit[CloseIndex])
                            {
                                StartIndex = i;
                                break;
                            }

                        }
                    }
                    if (StartIndex != 0)
                    {
                        GenerateTransXMLBuilder.AppendLine("<" + tagslsit[StartIndex].ToUpper() + ">");// + fieldVal.Trim() + "</" + key.ToUpper() + ">");                                      
                    }
                }
                else
                {
                    if (needtocall)
                    {
                        if (tallyKey.ToUpper().Contains("/"))
                        {
                            tallyKey = tallyKey.Replace("/", "");
                            GenerateTransXMLBuilder.AppendLine("</" + tallyKey.ToUpper() + ">");
                        }
                        else
                        {
                            GenerateTransXMLBuilder.AppendLine("</" + tallyKey.ToUpper() + ">");
                        }
                    }
                    else
                    {
                        if (!tallyKey.ToUpper().Contains("/"))
                        {
                            string fieldVal = string.Empty;
                            string[] newTallyKey = tallyKey.Split(new char[] { ' ' });
                            if (newTallyKey.Length > 1)
                            {
                                fieldVal = this.GetFieldValue(Tagsdictionary, Valuesdictionary, tallyKey.ToUpper(), tagOrderNumber);
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                if (_itemsRow.Table.TableName.Contains("ACDET".ToUpper()))
                                {
                                    if (tallyKey.ToString().ToUpper() == "ISDEEMEDPOSITIVE".ToString().ToUpper())
                                    {
                                        string amt_ty = _itemsRow["amt_ty"].ToString();
                                        if (amt_ty.ToUpper() == "CR".ToUpper())
                                        {
                                            fieldVal = CFunctions.convertSpecialChars("NO");
                                        }
                                        else
                                        {
                                            fieldVal = CFunctions.convertSpecialChars("YES");
                                        }
                                    }
                                }

                                GenerateTransXMLBuilder.AppendLine("<" + tallyKey.ToUpper() + ">" + fieldVal + "</" + newTallyKey[0].ToUpper() + ">");
                            }
                            else
                            {
                                fieldVal = this.GetFieldValue(Tagsdictionary, Valuesdictionary, tallyKey.ToUpper(), tagOrderNumber);
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                if (_itemsRow.Table.TableName.Contains("ACDET".ToUpper()))
                                {
                                    if (tallyKey.ToString().ToUpper() == "ISDEEMEDPOSITIVE".ToString().ToUpper())
                                    {
                                        string amt_ty = _itemsRow["amt_ty"].ToString();
                                        if (amt_ty.ToUpper() == "CR".ToUpper())
                                        {
                                            fieldVal = CFunctions.convertSpecialChars("NO");
                                        }
                                        else
                                        {
                                            fieldVal = CFunctions.convertSpecialChars("YES");
                                        }
                                    }
                                }
                                GenerateTransXMLBuilder.AppendLine("<" + tallyKey.ToUpper() + ">" + fieldVal + "</" + tallyKey.ToUpper() + ">");
                            }

                        }
                        else
                        {
                            tallyKey = tallyKey.Replace("/", "");
                            GenerateTransXMLBuilder.AppendLine("</" + tallyKey.ToUpper() + ">");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dbError = ex.Message;
                Logger.LogInfo(ex);
            }
        }

        private void getDinamicXMLString(Dictionary<string, string> TagsList, Dictionary<string, string> ValuesList, string[] Fieldvalues, string[] defaultvalues, DataRow row, string[] allTags, string key, string OrderNumber)
        {
            string fieldVal = string.Empty;
            string rowValue = string.Empty;
            StringBuilder strKey = new StringBuilder();
            Dictionary<string, string> tabAndCol = new Dictionary<string, string>();
            string tableName = string.Empty;
            string columnName = string.Empty;
            string strSwappingDataValue = string.Empty;
            try
            {
                strKey.Append(key.ToUpper());
                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, strKey) == "Field")
                {
                    tabAndCol = this.getTableAndColumnName(TagsList, ValuesList, OrderNumber);
                    this.getTableAndColumnName(TagsList, ValuesList, OrderNumber).TryGetValue("table", out tableName);
                    this.getTableAndColumnName(TagsList, ValuesList, OrderNumber).TryGetValue("column", out columnName);

                    ValuesList.TryGetValue(OrderNumber, out strSwappingDataValue);

                    if (columnName != "")
                    {
                        if (tableName.ToLower() == ("MAIN").ToString().ToLower())
                        {
                            if (row[columnName] != null)
                            {
                                string DataValue = string.Empty;
                                DataValue = row[columnName].ToString();
                                if ("Date".ToUpper() == columnName.ToUpper() && key.ToUpper() == "Date".ToUpper())
                                {
                                    DateTime date = Convert.ToDateTime(DataValue.ToString());
                                    string year = date.Year.ToString();
                                    string month = date.Month.ToString();
                                    if (month.Length == 1)
                                        month = "0" + month;
                                    string day = date.Day.ToString();
                                    if (day.Length == 1)
                                        day = "0" + day;
                                    DataValue = year + month + day;
                                }
                                else if ("Date".ToUpper() == columnName.ToUpper() && key.ToUpper() == "EFFECTIVEDATE".ToUpper())
                                {
                                    DateTime date = Convert.ToDateTime(DataValue.ToString());
                                    string year = date.Year.ToString();
                                    string month = date.Month.ToString();
                                    if (month.Length == 1)
                                        month = "0" + month;
                                    string day = date.Day.ToString();
                                    if (day.Length == 1)
                                        day = "0" + day;
                                    DataValue = year + month + day;
                                }
                                else if ("Date".ToUpper() == columnName.ToUpper() && key.ToUpper() == "BASICDATETIMEOFINVOICE".ToUpper())
                                {
                                    DateTime date = Convert.ToDateTime(DataValue.ToString());
                                    string year = date.Year.ToString();
                                    string month = Enum.GetName(typeof(MonthName), Convert.ToInt32(date.Month));
                                    string day = date.Day.ToString();
                                    string time = date.Hour + ":" + date.Minute;
                                    DataValue = day + "-" + month + "-" + year + " at " + time;
                                }
                                else if ("Date".ToUpper() == columnName.ToUpper() && key.ToUpper() == "BASICDATETIMEOFREMOVAL".ToUpper())
                                {
                                    DateTime date = Convert.ToDateTime(DataValue.ToString());
                                    string year = date.Year.ToString();
                                    string month = Enum.GetName(typeof(MonthName), Convert.ToInt32(date.Month));
                                    string day = date.Day.ToString();
                                    string time = date.Hour + ":" + date.Minute;
                                    DataValue = day + "-" + month + "-" + year + " at " + time;
                                }
                                rowValue = CFunctions.convertSpecialChars(DataValue.ToString());

                                foreach (string Swapp in ApplicationValues.AccountGroupSwappingDataTables)
                                {
                                    if (Swapp.ToUpper().Trim() == strSwappingDataValue.ToUpper().Trim())
                                    {
                                        rowValue = DataMemberSwapping(rowValue, strSwappingDataValue, "Accounts Group");
                                        break;
                                    }
                                }
                                foreach (string Swapp in ApplicationValues.AccountSwappingDataTables)
                                {
                                    if (Swapp.ToUpper().Trim() == strSwappingDataValue.ToUpper().Trim())
                                    {
                                        rowValue = DataMemberSwapping(rowValue, strSwappingDataValue, "Accounts");
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    GenerateTransXMLBuilder.AppendLine("<" + key.ToUpper() + ">" + rowValue.Trim() + "</" + key.ToUpper() + ">");
                }
                else
                {
                    fieldVal = this.GetFieldValue(TagsList, ValuesList, key.ToUpper(), OrderNumber);
                    if (fieldVal != "")
                        fieldVal = CFunctions.convertSpecialChars(fieldVal);

                    if (fieldVal == "" && key.Contains("U2T"))//tag have attributies
                    {
                        fieldVal = this.getfieldValString(key.ToUpper());
                        strSwappingDataValue = getDataMemberFieldValue(key.ToUpper());
                        if (fieldVal != "")
                        {
                            if (row[fieldVal] != null)
                            {
                                rowValue = rowValue.ToUpper();
                                rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                foreach (string Swapp in ApplicationValues.AccountGroupSwappingDataTables)
                                {
                                    if (Swapp.ToUpper().Trim() == strSwappingDataValue.ToUpper().Trim())
                                    {
                                        rowValue = DataMemberSwapping(rowValue, strSwappingDataValue, "Accounts Group");
                                        break;
                                    }
                                }
                                foreach (string Swapp in ApplicationValues.AccountSwappingDataTables)
                                {
                                    if (Swapp.ToUpper().Trim() == strSwappingDataValue.ToUpper().Trim())
                                    {
                                        rowValue = DataMemberSwapping(rowValue, strSwappingDataValue, "Accounts");
                                        break;
                                    }
                                }
                            }
                        }
                        string NewKey = ReplaceRowValue(key, rowValue.Trim(), ApplicationValues.tableName, fieldVal);
                        GenerateTransXMLBuilder.AppendLine("<" + NewKey + ">");
                    }
                    else if (fieldVal != "" && key.Contains("NewGUID"))
                    {
                        fieldVal = replaceGUID(key, row["tran_cd"].ToString(), ApplicationValues.TransEntryType);
                        GenerateTransXMLBuilder.AppendLine("<" + fieldVal + ">");
                    }
                    else if (fieldVal != "" && fieldVal.Contains("NewGUID"))
                    {
                        GlobleGUIDString = GlobleGUIDString.Replace("'", "");
                        fieldVal = GlobleGUIDString;
                        GenerateTransXMLBuilder.AppendLine("<" + key + ">" + fieldVal + "</" + key + ">");
                        GlobleGUIDString = string.Empty;
                    }
                    else if (fieldVal != "" && fieldVal.Contains("SelectedCompanyName"))
                    {
                        fieldVal = ApplicationValues.CompanyName;
                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                        GenerateTransXMLBuilder.AppendLine("<" + key + ">" + fieldVal + "</" + key + ">");
                    }
                    else
                    {
                        Recreasion(TagsList, ValuesList, allTags, key, Fieldvalues, defaultvalues, OrderNumber, row);
                    }

                }
            }
            catch (Exception ex)
            {
                dbError = ex.Message;
                Logger.LogInfo(ex);
            }
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
                            fieldVal = collname.Substring(val + 1, (val1 - val) - 1);
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

        enum MonthName
        {
            jan = 1, feb, mar, apr, may, jun, jul, aug, sep, oct, nov, dec
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

        private string GetNewGUID(string Entry_Ty, string tran_cd)
        {
            string newGUID = Guid.NewGuid().ToString();
            string newAutoString = getString(Entry_Ty, tran_cd);
            newGUID = "'" + newGUID + "-" + newAutoString + "'";
            return newGUID;
        }

        private string replaceGUID(string key, string tran_cd, string entry_ty)
        {
            string Strreturn = string.Empty;
            GlobleGUIDString = GetNewGUID(entry_ty, tran_cd);
            key = key.Replace("NewGUID", GlobleGUIDString);
            return key;
        }

        private string getString(string suffix, string prifix)
        {
            string resultValue = string.Empty;
            int lenOfZero = 8 - (suffix.Length + prifix.Length);
            for (int i = 0; i < lenOfZero; i++)
            {
                resultValue = resultValue + "0";
            }
            resultValue = suffix + resultValue + prifix;
            return resultValue;
        }

        private Dictionary<string, string> getTableAndColumnName(Dictionary<string, string> TagsDictionary, Dictionary<string, string> ValuesDictionary, string OrderNumber)
        {
            Dictionary<string, string> TableAndColumn = new Dictionary<string, string>();
            string[] splitstring = null;
            string strTagDtls = string.Empty;
            string ColumnValue = string.Empty;
            string fieldVal = string.Empty;
            string tableName = string.Empty;

            try
            {
                TagsDictionary.TryGetValue(OrderNumber, out strTagDtls);
                ValuesDictionary.TryGetValue(OrderNumber, out ColumnValue);

                if (ColumnValue != "")
                {
                    splitstring = ColumnValue.Split(new char[] { '.' });
                    if (splitstring.Length > 0)
                    {
                        foreach (string strvalue in splitstring)
                        {
                            if (strvalue.Contains("U2T"))
                            {
                                int num1 = strvalue.IndexOf('_', 0);
                                tableName = strvalue.Substring(num1 + 1, (strvalue.Length - (num1 + 1)));
                            }
                            else
                            {
                                fieldVal = strvalue;
                            }
                        }
                    }
                }
                TableAndColumn.Add("table", tableName);
                TableAndColumn.Add("column", fieldVal);
            }
            catch (Exception ex)
            {
                dbError = ex.Message;
                Logger.LogInfo(ex);
            }

            return TableAndColumn;
        }

        /// <summary>
        /// To Get the Selected Company Account Group Master Table Field Value
        /// </summary>
        /// <param name="_iGenerateMasterXML"></param>
        /// <returns>string</returns>
        private string GetFieldValue(Dictionary<string, string> TagsDictionary, Dictionary<string, string> ValuesDictionary, string KeyToGetValue, string tagOrderNumber)
        {
            string strTblDtls = string.Empty;
            string strFieldValue = string.Empty;
            string[] splitstring = null;
            string fieldValue = string.Empty;
            try
            {
                TagsDictionary.TryGetValue(tagOrderNumber, out strTblDtls);
                ValuesDictionary.TryGetValue(tagOrderNumber, out strFieldValue);
                if (strFieldValue == "")
                {
                    splitstring = strTblDtls.Split(new char[] { '.' });
                    if (splitstring.Length > 0)
                    {
                        foreach (string strvalue in splitstring)
                        {
                            if (strvalue.Contains("U2T"))
                            {
                            }
                            else if (strvalue.Contains("NewGUID"))
                            {
                                fieldValue = strvalue;
                            }
                            else
                            {
                                fieldValue = "";
                            }

                        }

                    }
                }
                else
                {
                    splitstring = strFieldValue.Split(new char[] { '.' });
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
                dbError = ex.Message;
                Logger.LogInfo(ex);
            }
            return returnString;
        }

        #endregion

    }
}
