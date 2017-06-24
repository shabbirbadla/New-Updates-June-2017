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
    public class GenerateMasterXML_OLD : IGenerateMasterXML
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
        public GenerateMasterXML_OLD()
        {

        }
        #endregion

        #region Globle Veriables of the Class

        StringBuilder GenerateMasterXMLBuilder = new StringBuilder();
        DataSet DSMaster = null;
        DataSet DSGroupMaster = null;
        U2TPlus.DAL.GenerateMasterXML objGenerateMasterXML = new U2TPlus.DAL.GenerateMasterXML();
        CommonFunctions CFunctions = new CommonFunctions();
        DataTable dTable = new DataTable();
        bool itgroupname = false;

        #endregion

        #region Public Methods

        /// <summary>
        /// To Get the Selected Company Account GroupMaster Table Data for Tally Version 9.0 as a String Builder
        /// </summary>
        /// <param name="_iGenerateMasterXML"></param>
        /// <returns>StringBuilder</returns>
        public StringBuilder GenerateAccountGroupMasterTV90()
        {
            Dictionary<string, string> agmList = new Dictionary<string, string>();
            string tablename = string.Empty;
            string fieldValue = string.Empty;
            string strTblDtls = string.Empty;
            string[] splitstring = null;
            string[] Fieldvalues = null;
            string[] defaultvalues = null;
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
                    }

                    #endregion
                }
                DSMaster = new DataSet();
                DSGroupMaster = new DataSet();
                objGenerateMasterXML.ConnectionString = ApplicationValues.CompanyDBConnectionString;
                objGenerateMasterXML.CompanyDBName = ApplicationValues.CompanyDBName;
                objGenerateMasterXML.GeneratingOptions = this.GeneratingOptions;

                if (fieldCount != 0)
                {
                    #region At least One Field Value is there in settins File
                    _isNeedToUpdate = true;

                    #region Geting the Data Base Table Name
                    agmList.TryGetValue("Group".ToUpper(), out strTblDtls);
                    splitstring = strTblDtls.Split(new char[] { '.' });
                    if (splitstring.Length > 0)
                    {
                        foreach (string strvalue in splitstring)
                        {
                            if (strvalue.Contains("U2T"))
                            {
                                tablename = strvalue;
                                int num1 = tablename.IndexOf('_', 0);
                                tablename = tablename.Substring(num1 + 1, (tablename.Length - (num1 + 1)));
                                if (tablename != string.Empty)
                                {
                                    ApplicationValues.tableName = tablename;
                                    break;
                                }
                                else
                                {
                                    ApplicationValues.tableName = "AC_GROUP_MAST";
                                    break;
                                }
                            }
                            else
                            {
                                fieldValue = strvalue;
                                ApplicationValues.tableName = "AC_GROUP_MAST";
                                break;
                            }
                        }
                    }
                    else
                    {
                        tablename = "AC_GROUP_MAST";
                        ApplicationValues.tableName = tablename;
                    }
                    #endregion
                    objGenerateMasterXML.MasterTableName = tablename;
                    objGenerateMasterXML.TallyTagsTable = this.TallyTagsTable;
                    DSGroupMaster = objGenerateMasterXML.GenerateAccountGroupMasterTV90(this);
                    if (DSGroupMaster.Tables.Count > 0)
                    {
                        dTable = new DataTable();
                        foreach (DataColumn dCloumn in DSGroupMaster.Tables[0].Columns)
                        {
                            DataColumn dc1 = new DataColumn();
                            dc1.ColumnName = dCloumn.ColumnName;
                            dTable.Columns.Add(dc1);
                        }
                        getAccountGroupMasterChild(DSGroupMaster, string.Empty);
                        string rowValue = string.Empty;
                        GenerateMasterXMLBuilder = new StringBuilder();
                        if (dTable != null)
                        {
                            foreach (DataRow row in dTable.Rows)
                            {
                                string fieldVal = string.Empty;
                                GenerateMasterXMLBuilder.AppendLine("<TALLYMESSAGE xmlns:UDF='TallyUDF'>");
                                #region For Group Tag
                                if (agmList.ContainsKey("Group".ToUpper()))
                                {
                                    if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "Group") == "Field")
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "Group");
                                        if (fieldVal != "")
                                        {
                                            if (row[fieldVal] != null)
                                                rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                        }
                                        GenerateMasterXMLBuilder.AppendLine("<GROUP Name='" + rowValue.Trim() + "' ACTION='Create'>");
                                    }
                                    else
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "Group");
                                        if (fieldVal != "")
                                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                        GenerateMasterXMLBuilder.AppendLine("<GROUP Name='" + fieldVal.Trim() + "' ACTION='Create'>");
                                    }
                                }
                                #endregion
                                #region For NAME.LIST Tag
                                if (agmList.ContainsKey("NAME.LIST".ToUpper()))
                                {
                                    GenerateMasterXMLBuilder.AppendLine("<NAME.LIST>");
                                    #region For Name Tag with in the NAME.LIST
                                    if (agmList.ContainsKey("NAME".ToUpper()))
                                    {
                                        if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "NAME") == "Field")
                                        {
                                            fieldVal = this.GetFieldValue(agmList, "NAME");
                                            if (fieldVal != "")
                                            {
                                                if (row[fieldVal] != null)
                                                    rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                            }
                                            GenerateMasterXMLBuilder.AppendLine("<NAME>" + rowValue.Trim() + "</NAME>");
                                        }
                                        else
                                        {
                                            fieldVal = this.GetFieldValue(agmList, "NAME");
                                            if (fieldVal != "")
                                                fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                            GenerateMasterXMLBuilder.AppendLine("<NAME>" + fieldVal.Trim() + "</NAME>");
                                        }
                                    }
                                    #endregion
                                    GenerateMasterXMLBuilder.AppendLine("</NAME.LIST>");
                                }
                                #endregion
                                #region for PARENT Tag
                                if (agmList.ContainsKey("PARENT".ToUpper()))
                                {
                                    if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "PARENT") == "Field")
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "PARENT");
                                        if (fieldVal != "")
                                        {
                                            if (row[fieldVal] != null)
                                                rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                        }

                                        GenerateMasterXMLBuilder.AppendLine("<PARENT>" + rowValue.Trim() + "</PARENT>");
                                    }
                                    else
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "PARENT");
                                        if (fieldVal != "")
                                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                        GenerateMasterXMLBuilder.AppendLine("<PARENT>" + fieldVal.Trim() + "</PARENT>");
                                    }
                                }
                                #endregion
                                #region For ISSUBLEDGER Tag
                                if (agmList.ContainsKey("ISSUBLEDGER".ToUpper()))
                                {
                                    if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "ISSUBLEDGER") == "Field")
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "ISSUBLEDGER");
                                        if (fieldVal != "")
                                        {
                                            if (row[fieldVal] != null)
                                                rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                        }
                                        GenerateMasterXMLBuilder.AppendLine("<ISSUBLEDGER>" + rowValue.Trim() + "</ISSUBLEDGER>");
                                    }
                                    else
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "ISSUBLEDGER");
                                        if (fieldVal != "")
                                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                        GenerateMasterXMLBuilder.AppendLine("<ISSUBLEDGER>" + fieldVal.Trim() + "</ISSUBLEDGER>");
                                    }
                                }
                                #endregion
                                #region For ISBILLWISEON Tag
                                if (agmList.ContainsKey("ISBILLWISEON".ToUpper()))
                                {
                                    if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "ISBILLWISEON") == "Field")
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "ISBILLWISEON");
                                        if (fieldVal != "")
                                        {
                                            if (row[fieldVal] != null)
                                                rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                        }
                                        GenerateMasterXMLBuilder.AppendLine("<ISBILLWISEON>" + rowValue.Trim() + "</ISBILLWISEON>");
                                    }
                                    else
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "ISBILLWISEON");
                                        if (fieldVal != "")
                                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                        GenerateMasterXMLBuilder.AppendLine("<ISBILLWISEON>" + fieldVal.Trim() + "</ISBILLWISEON>");
                                    }
                                }
                                #endregion
                                #region For ISCOSTCENTRESON Tag
                                if (agmList.ContainsKey("ISCOSTCENTRESON".ToUpper()))
                                {
                                    if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "ISCOSTCENTRESON") == "Field")
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "ISCOSTCENTRESON");
                                        if (fieldVal != "")
                                        {
                                            if (row[fieldVal] != null)
                                                rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                        }
                                        GenerateMasterXMLBuilder.AppendLine("<ISCOSTCENTRESON>" + rowValue.Trim() + "</ISCOSTCENTRESON>");
                                    }
                                    else
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "ISCOSTCENTRESON");
                                        if (fieldVal != "")
                                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                        GenerateMasterXMLBuilder.AppendLine("<ISCOSTCENTRESON>" + fieldVal.Trim() + "</ISCOSTCENTRESON>");
                                    }
                                }
                                #endregion
                                #region For Rest Of Tags
                                if (agmList.Count > 7)
                                {
                                    foreach (string key in agmList.Keys)
                                    {
                                        if (key.ToUpper() != "Group".ToUpper() && key.ToUpper() != "NAME.LIST".ToUpper() && key.ToUpper() != "NAME".ToUpper() && key.ToUpper() != "PARENT".ToUpper() && key.ToUpper() != "ISSUBLEDGER".ToUpper() && key.ToUpper() != "ISBILLWISEON".ToUpper() && key.ToUpper() != "ISCOSTCENTRESON".ToUpper())
                                        {
                                            if (VerifyDefaultOrField(defaultvalues, Fieldvalues, key.ToUpper()) == "Field")
                                            {
                                                fieldVal = this.GetFieldValue(agmList, key.ToUpper());
                                                if (fieldVal != "")
                                                {
                                                    if (row[fieldVal] != null)
                                                        rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                                }
                                                GenerateMasterXMLBuilder.AppendLine("<" + key.ToUpper() + ">" + rowValue.Trim() + "</" + key.ToUpper() + ">");
                                            }
                                            else
                                            {
                                                fieldVal = this.GetFieldValue(agmList, key.ToUpper());
                                                if (fieldVal != "")
                                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                                GenerateMasterXMLBuilder.AppendLine("<" + key.ToUpper() + ">" + fieldVal.Trim() + "</" + key.ToUpper() + ">");
                                            }
                                        }
                                    }
                                }
                                #endregion
                                #region GRUPING END TAGS
                                if (agmList.ContainsKey("GROUP".ToUpper()))
                                {
                                    GenerateMasterXMLBuilder.AppendLine("</GROUP>");
                                }
                                #endregion
                                GenerateMasterXMLBuilder.AppendLine("</TALLYMESSAGE>");
                            }
                        }

                    }
                    else
                    {
                        dbError = objGenerateMasterXML.dbError;
                    }
                    #endregion
                }
                else
                {
                    #region In Settings Page All Values Are Have Default Values
                    _isNeedToUpdate = false;
                    GenerateMasterXMLBuilder.AppendLine("<TALLYMESSAGE xmlns:UDF='TallyUDF'>");
                    string fieldVal = string.Empty;
                    #region For Group Tag
                    if (agmList.ContainsKey("Group".ToUpper()))
                    {
                        fieldVal = this.GetFieldValue(agmList, "Group");
                        if (fieldVal != "")
                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                        GenerateMasterXMLBuilder.AppendLine("<GROUP Name='" + fieldVal.Trim() + "' ACTION='Create'>");

                    }
                    #endregion

                    #region For NAME.LIST Tag
                    if (agmList.ContainsKey("NAME.LIST".ToUpper()))
                    {
                        GenerateMasterXMLBuilder.AppendLine("<NAME.LIST>");

                        #region For Name Tag with in the NAME.LIST
                        if (agmList.ContainsKey("NAME".ToUpper()))
                        {
                            fieldVal = this.GetFieldValue(agmList, "NAME");
                            if (fieldVal != "")
                                fieldVal = CFunctions.convertSpecialChars(fieldVal);
                            GenerateMasterXMLBuilder.AppendLine("<NAME>" + fieldVal.Trim() + "</NAME>");

                        }
                        #endregion

                        GenerateMasterXMLBuilder.AppendLine("</NAME.LIST>");
                    }
                    #endregion

                    #region for PARENT Tag
                    if (agmList.ContainsKey("PARENT".ToUpper()))
                    {
                        fieldVal = this.GetFieldValue(agmList, "PARENT");
                        if (fieldVal != "")
                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                        GenerateMasterXMLBuilder.AppendLine("<PARENT>" + fieldVal.Trim() + "</PARENT>");
                    }
                    #endregion

                    #region For ISSUBLEDGER Tag
                    if (agmList.ContainsKey("ISSUBLEDGER".ToUpper()))
                    {
                        fieldVal = this.GetFieldValue(agmList, "ISSUBLEDGER");
                        if (fieldVal != "")
                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                        GenerateMasterXMLBuilder.AppendLine("<ISSUBLEDGER>" + fieldVal.Trim() + "</ISSUBLEDGER>");
                    }
                    #endregion

                    #region For ISBILLWISEON Tag
                    if (agmList.ContainsKey("ISBILLWISEON".ToUpper()))
                    {
                        fieldVal = this.GetFieldValue(agmList, "ISBILLWISEON");
                        if (fieldVal != "")
                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                        GenerateMasterXMLBuilder.AppendLine("<ISBILLWISEON>" + fieldVal.Trim() + "</ISBILLWISEON>");
                    }
                    #endregion

                    #region For ISCOSTCENTRESON Tag
                    if (agmList.ContainsKey("ISCOSTCENTRESON".ToUpper()))
                    {
                        fieldVal = this.GetFieldValue(agmList, "ISCOSTCENTRESON");
                        if (fieldVal != "")
                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                        GenerateMasterXMLBuilder.AppendLine("<ISCOSTCENTRESON>" + fieldVal.Trim() + "</ISCOSTCENTRESON>");
                    }
                    #endregion

                    #region For Rest Of Tags
                    if (agmList.Count > 7)
                    {
                        foreach (string key in agmList.Keys)
                        {
                            if (key.ToUpper() != "Group".ToUpper() && key.ToUpper() != "NAME.LIST".ToUpper() && key.ToUpper() != "NAME".ToUpper() && key.ToUpper() != "PARENT".ToUpper() && key.ToUpper() != "ISSUBLEDGER".ToUpper() && key.ToUpper() != "ISBILLWISEON".ToUpper() && key.ToUpper() != "ISCOSTCENTRESON".ToUpper())
                            {
                                fieldVal = this.GetFieldValue(agmList, key.ToUpper());
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<" + key.ToUpper() + ">" + fieldVal.Trim() + "</" + key.ToUpper() + ">");
                            }
                        }
                    }
                    #endregion

                    #region GRUPING END TAGS
                    if (agmList.ContainsKey("GROUP".ToUpper()))
                    {
                        GenerateMasterXMLBuilder.AppendLine("</GROUP>");
                    }
                    #endregion

                    GenerateMasterXMLBuilder.AppendLine("</TALLYMESSAGE>");

                    #endregion
                }
            }
            catch (Exception ex)
            {
                dbError = ex.Message;
            }
            return GenerateMasterXMLBuilder;
        }

        /// <summary>
        /// To Update the Selected Company Master Table
        /// </summary>
        /// <param name="_iGenerateMasterXML"></param>
        /// <returns>int</returns>
        public int UpdateGenerateMasterTV90()
        {
            int returnValue = 0;
            objGenerateMasterXML.ConnectionString = ApplicationValues.CompanyDBConnectionString;
            objGenerateMasterXML.CompanyDBName = ApplicationValues.CompanyDBName;
            objGenerateMasterXML.MasterTableName = ApplicationValues.tableName;
            objGenerateMasterXML.TallyTagsTable = this.TallyTagsTable;
            returnValue = objGenerateMasterXML.UpdateGenerateMaster(this);

            return returnValue;
        }

        /// <summary>
        /// To Get the Selected Company Account Master Table Data for Tally Version 9.0 as a String Builder
        /// </summary>
        /// <param name="_iGenerateMasterXML"></param>
        /// <returns>StringBuilder</returns> 
        public StringBuilder GenerateAccountMasterTV90()
        {
            Dictionary<string, string> agmList = new Dictionary<string, string>();
            string tablename = string.Empty;
            string fieldValue = string.Empty;
            string strTblDtls = string.Empty;
            string[] splitstring = null;
            string[] Fieldvalues = null;
            string[] defaultvalues = null;
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
                    if (ApplicationValues.GenaretingMasterName.ToUpper() == "Account Master".ToUpper())
                    {
                        #region At least One Field Value is there in settins File for Account Master
                        #region Geting the Data Base Table Name
                        agmList.TryGetValue("LEDGER".ToUpper(), out strTblDtls);
                        splitstring = strTblDtls.Split(new char[] { '.' });
                        if (splitstring.Length > 0)
                        {
                            foreach (string strvalue in splitstring)
                            {
                                if (strvalue.Contains("U2T"))
                                {
                                    tablename = strvalue;
                                    int num1 = tablename.IndexOf('_', 0);
                                    tablename = tablename.Substring(num1 + 1, (tablename.Length - (num1 + 1)));
                                    if (tablename != string.Empty)
                                    {
                                        ApplicationValues.tableName = tablename;
                                        break;
                                    }
                                    else
                                    {
                                        ApplicationValues.tableName = "AC_MAST";
                                        break;
                                    }
                                }
                                else
                                {
                                    fieldValue = strvalue;
                                    ApplicationValues.tableName = "AC_MAST";
                                    break;
                                }
                            }
                        }
                        else
                        {
                            tablename = "AC_MAST";
                            ApplicationValues.tableName = tablename;
                        }
                        #endregion
                        objGenerateMasterXML.MasterTableName = tablename;
                        objGenerateMasterXML.TallyTagsTable = this.TallyTagsTable;
                        DSMaster = objGenerateMasterXML.GenerateMaster(this);
                        string rowValue = string.Empty;
                        GenerateMasterXMLBuilder = new StringBuilder();
                        foreach (DataRow row in DSMaster.Tables[0].Rows)
                        {
                            GenerateMasterXMLBuilder.AppendLine("<TALLYMESSAGE xmlns:UDF='TallyUDF'>");
                            string fieldVal = string.Empty;
                            #region For LEDGER Tag
                            if (agmList.ContainsKey("LEDGER".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "LEDGER") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "LEDGER");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<LEDGER Name='" + rowValue.Trim() + "' RESERVEDNAME=''  ACTION='Create'>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "LEDGER");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<LEDGER Name='" + fieldVal.Trim() + "' RESERVEDNAME=''  ACTION='Create'>");
                                }
                            }
                            #endregion

                            #region For MAILINGNAME.LIST Tag
                            if (agmList.ContainsKey("MAILINGNAME.LIST".ToUpper()))
                            {
                                GenerateMasterXMLBuilder.AppendLine("<MAILINGNAME.LIST>");
                                #region For MAILINGNAME Tag with in the MAILINGNAME.LIST
                                if (agmList.ContainsKey("MAILINGNAME".ToUpper()))
                                {
                                    if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "MAILINGNAME") == "Field")
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "MAILINGNAME");
                                        if (fieldVal != "")
                                        {
                                            if (row[fieldVal] != null)
                                                rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                        }
                                        GenerateMasterXMLBuilder.AppendLine("<MAILINGNAME>" + rowValue.Trim() + "</MAILINGNAME>");
                                    }
                                    else
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "MAILINGNAME");
                                        if (fieldVal != "")
                                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                        GenerateMasterXMLBuilder.AppendLine("<MAILINGNAME>" + fieldVal.Trim() + "</MAILINGNAME>");
                                    }
                                }
                                #endregion
                                GenerateMasterXMLBuilder.AppendLine("</MAILINGNAME.LIST>");
                            }
                            #endregion

                            #region For CURRENCYNAME Tag
                            if (agmList.ContainsKey("CURRENCYNAME".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "CURRENCYNAME") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "CURRENCYNAME");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<CURRENCYNAME>" + rowValue.Trim() + "</CURRENCYNAME>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "CURRENCYNAME");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<CURRENCYNAME>" + fieldVal.Trim() + "</CURRENCYNAME>");
                                }
                            }
                            #endregion

                            #region for PARENT Tag
                            if (agmList.ContainsKey("PARENT".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "PARENT") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "PARENT");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }

                                    GenerateMasterXMLBuilder.AppendLine("<PARENT>" + rowValue.Trim() + "</PARENT>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "PARENT");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<PARENT>" + fieldVal.Trim() + "</PARENT>");
                                }
                            }
                            #endregion

                            #region For TAXCLASSIFICATIONNAME Tag
                            if (agmList.ContainsKey("TAXCLASSIFICATIONNAME".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "TAXCLASSIFICATIONNAME") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "TAXCLASSIFICATIONNAME");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<TAXCLASSIFICATIONNAME>" + rowValue.Trim() + "</TAXCLASSIFICATIONNAME>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "TAXCLASSIFICATIONNAME");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<TAXCLASSIFICATIONNAME>" + fieldVal.Trim() + "</TAXCLASSIFICATIONNAME>");
                                }
                            }
                            #endregion

                            #region For TAXTYPE Tag
                            if (agmList.ContainsKey("TAXTYPE".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "TAXTYPE") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "TAXTYPE");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<TAXTYPE>" + rowValue.Trim() + "</TAXTYPE>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "TAXTYPE");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<TAXTYPE>" + fieldVal.Trim() + "</TAXTYPE>");
                                }
                            }
                            #endregion

                            #region For GSTTYPE Tag
                            if (agmList.ContainsKey("GSTTYPE".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "GSTTYPE") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "GSTTYPE");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<GSTTYPE>" + rowValue.Trim() + "</GSTTYPE>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "GSTTYPE");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<GSTTYPE>" + fieldVal.Trim() + "</GSTTYPE>");
                                }
                            }
                            #endregion

                            #region For SERVICECATEGORY Tag
                            if (agmList.ContainsKey("SERVICECATEGORY".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "SERVICECATEGORY") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "SERVICECATEGORY");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<SERVICECATEGORY>" + rowValue.Trim() + "</SERVICECATEGORY>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "SERVICECATEGORY");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<SERVICECATEGORY>" + fieldVal.Trim() + "</SERVICECATEGORY>");
                                }
                            }
                            #endregion

                            #region For EXCISEDUTYTYPE Tag
                            if (agmList.ContainsKey("EXCISEDUTYTYPE".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "EXCISEDUTYTYPE") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "EXCISEDUTYTYPE");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<EXCISEDUTYTYPE>" + rowValue.Trim() + "</EXCISEDUTYTYPE>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "EXCISEDUTYTYPE");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<EXCISEDUTYTYPE>" + fieldVal.Trim() + "</EXCISEDUTYTYPE>");
                                }
                            }
                            #endregion

                            #region For TRADERLEDNATUREOFPURCHASE Tag
                            if (agmList.ContainsKey("TRADERLEDNATUREOFPURCHASE".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "TRADERLEDNATUREOFPURCHASE") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "TRADERLEDNATUREOFPURCHASE");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<TRADERLEDNATUREOFPURCHASE>" + rowValue.Trim() + "</TRADERLEDNATUREOFPURCHASE>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "TRADERLEDNATUREOFPURCHASE");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<TRADERLEDNATUREOFPURCHASE>" + fieldVal.Trim() + "</TRADERLEDNATUREOFPURCHASE>");
                                }
                            }
                            #endregion

                            #region For TDSDEDUCTEETYPE Tag
                            if (agmList.ContainsKey("TDSDEDUCTEETYPE".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "TDSDEDUCTEETYPE") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "TDSDEDUCTEETYPE");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<TDSDEDUCTEETYPE>" + rowValue.Trim() + "</TDSDEDUCTEETYPE>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "TDSDEDUCTEETYPE");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<TDSDEDUCTEETYPE>" + fieldVal.Trim() + "</TDSDEDUCTEETYPE>");
                                }
                            }
                            #endregion

                            #region For TDSRATENAME Tag
                            if (agmList.ContainsKey("TDSRATENAME".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "TDSRATENAME") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "TDSRATENAME");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<TDSRATENAME>" + rowValue.Trim() + "</TDSRATENAME>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "TDSRATENAME");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<TDSRATENAME>" + fieldVal.Trim() + "</TDSRATENAME>");
                                }
                            }
                            #endregion

                            #region For LEDGERFBTCATEGORY Tag
                            if (agmList.ContainsKey("LEDGERFBTCATEGORY".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "LEDGERFBTCATEGORY") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "LEDGERFBTCATEGORY");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<LEDGERFBTCATEGORY>" + rowValue.Trim() + "</LEDGERFBTCATEGORY>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "LEDGERFBTCATEGORY");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<LEDGERFBTCATEGORY>" + fieldVal.Trim() + "</LEDGERFBTCATEGORY>");
                                }
                            }
                            #endregion

                            #region For ISBILLWISEON Tag
                            if (agmList.ContainsKey("ISBILLWISEON".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "ISBILLWISEON") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "ISBILLWISEON");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<ISBILLWISEON>" + rowValue.Trim() + "</ISBILLWISEON>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "ISBILLWISEON");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<ISBILLWISEON>" + fieldVal.Trim() + "</ISBILLWISEON>");
                                }
                            }
                            #endregion

                            #region For ISCOSTCENTRESON Tag
                            if (agmList.ContainsKey("ISCOSTCENTRESON".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "ISCOSTCENTRESON") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "ISCOSTCENTRESON");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<ISCOSTCENTRESON>" + rowValue.Trim() + "</ISCOSTCENTRESON>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "ISCOSTCENTRESON");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<ISCOSTCENTRESON>" + fieldVal.Trim() + "</ISCOSTCENTRESON>");
                                }
                            }
                            #endregion

                            #region For ISINTERESTON Tag
                            if (agmList.ContainsKey("ISINTERESTON".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "ISINTERESTON") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "ISINTERESTON");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<ISINTERESTON>" + rowValue.Trim() + "</ISINTERESTON>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "ISINTERESTON");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<ISINTERESTON>" + fieldVal.Trim() + "</ISINTERESTON>");
                                }
                            }
                            #endregion

                            #region For ALLOWINMOBILE Tag
                            if (agmList.ContainsKey("ALLOWINMOBILE".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "ALLOWINMOBILE") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "ALLOWINMOBILE");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<ALLOWINMOBILE>" + rowValue.Trim() + "</ALLOWINMOBILE>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "ALLOWINMOBILE");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<ALLOWINMOBILE>" + fieldVal.Trim() + "</ALLOWINMOBILE>");
                                }
                            }
                            #endregion

                            #region For ISCONDENSED Tag
                            if (agmList.ContainsKey("ISCONDENSED".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "ISCONDENSED") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "ISCONDENSED");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<ISCONDENSED>" + rowValue.Trim() + "</ISCONDENSED>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "ISCONDENSED");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<ISCONDENSED>" + fieldVal.Trim() + "</ISCONDENSED>");
                                }
                            }
                            #endregion

                            #region For AFFECTSSTOCK Tag
                            if (agmList.ContainsKey("AFFECTSSTOCK".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "AFFECTSSTOCK") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "AFFECTSSTOCK");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<AFFECTSSTOCK>" + rowValue.Trim() + "</AFFECTSSTOCK>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "AFFECTSSTOCK");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<AFFECTSSTOCK>" + fieldVal.Trim() + "</AFFECTSSTOCK>");
                                }
                            }
                            #endregion

                            #region For FORPAYROLL Tag
                            if (agmList.ContainsKey("FORPAYROLL".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "FORPAYROLL") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "FORPAYROLL");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<FORPAYROLL>" + rowValue.Trim() + "</FORPAYROLL>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "FORPAYROLL");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<FORPAYROLL>" + fieldVal.Trim() + "</FORPAYROLL>");
                                }
                            }
                            #endregion

                            #region For INTERESTONBILLWISE Tag
                            if (agmList.ContainsKey("INTERESTONBILLWISE".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "INTERESTONBILLWISE") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "INTERESTONBILLWISE");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<INTERESTONBILLWISE>" + rowValue.Trim() + "</INTERESTONBILLWISE>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "INTERESTONBILLWISE");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<INTERESTONBILLWISE>" + fieldVal.Trim() + "</INTERESTONBILLWISE>");
                                }
                            }
                            #endregion

                            #region For OVERRIDEINTEREST Tag
                            if (agmList.ContainsKey("OVERRIDEINTEREST".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "OVERRIDEINTEREST") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "OVERRIDEINTEREST");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<OVERRIDEINTEREST>" + rowValue.Trim() + "</OVERRIDEINTEREST>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "OVERRIDEINTEREST");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<OVERRIDEINTEREST>" + fieldVal.Trim() + "</OVERRIDEINTEREST>");
                                }
                            }
                            #endregion

                            #region For OVERRIDEADVINTEREST Tag
                            if (agmList.ContainsKey("OVERRIDEADVINTEREST".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "OVERRIDEADVINTEREST") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "OVERRIDEADVINTEREST");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<OVERRIDEADVINTEREST>" + rowValue.Trim() + "</OVERRIDEADVINTEREST>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "OVERRIDEADVINTEREST");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<OVERRIDEADVINTEREST>" + fieldVal.Trim() + "</OVERRIDEADVINTEREST>");
                                }
                            }
                            #endregion

                            #region For USEFORVAT Tag
                            if (agmList.ContainsKey("USEFORVAT".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "USEFORVAT") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "USEFORVAT");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<USEFORVAT>" + rowValue.Trim() + "</USEFORVAT>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "USEFORVAT");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<USEFORVAT>" + fieldVal.Trim() + "</USEFORVAT>");
                                }
                            }
                            #endregion

                            #region For IGNORETDSEXEMPT Tag
                            if (agmList.ContainsKey("IGNORETDSEXEMPT".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "IGNORETDSEXEMPT") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "IGNORETDSEXEMPT");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<IGNORETDSEXEMPT>" + rowValue.Trim() + "</IGNORETDSEXEMPT>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "IGNORETDSEXEMPT");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<IGNORETDSEXEMPT>" + fieldVal.Trim() + "</IGNORETDSEXEMPT>");
                                }
                            }
                            #endregion

                            #region For ISTCSAPPLICABLE Tag
                            if (agmList.ContainsKey("ISTCSAPPLICABLE".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "ISTCSAPPLICABLE") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "ISTCSAPPLICABLE");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<ISTCSAPPLICABLE>" + rowValue.Trim() + "</ISTCSAPPLICABLE>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "ISTCSAPPLICABLE");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<ISTCSAPPLICABLE>" + fieldVal.Trim() + "</ISTCSAPPLICABLE>");
                                }
                            }
                            #endregion

                            #region For ISTDSAPPLICABLE Tag
                            if (agmList.ContainsKey("ISTDSAPPLICABLE".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "ISTDSAPPLICABLE") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "ISTDSAPPLICABLE");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<ISTDSAPPLICABLE>" + rowValue.Trim() + "</ISTDSAPPLICABLE>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "ISTDSAPPLICABLE");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<ISTDSAPPLICABLE>" + fieldVal.Trim() + "</ISTDSAPPLICABLE>");
                                }
                            }
                            #endregion

                            #region For ISFBTAPPLICABLE Tag
                            if (agmList.ContainsKey("ISFBTAPPLICABLE".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "ISFBTAPPLICABLE") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "ISFBTAPPLICABLE");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<ISFBTAPPLICABLE>" + rowValue.Trim() + "</ISFBTAPPLICABLE>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "ISFBTAPPLICABLE");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<ISFBTAPPLICABLE>" + fieldVal.Trim() + "</ISFBTAPPLICABLE>");
                                }
                            }
                            #endregion

                            #region For ISGSTAPPLICABLE Tag
                            if (agmList.ContainsKey("ISGSTAPPLICABLE".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "ISGSTAPPLICABLE") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "ISGSTAPPLICABLE");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<ISGSTAPPLICABLE>" + rowValue.Trim() + "</ISGSTAPPLICABLE>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "ISGSTAPPLICABLE");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<ISGSTAPPLICABLE>" + fieldVal.Trim() + "</ISGSTAPPLICABLE>");
                                }
                            }
                            #endregion

                            #region For SHOWINPAYSLIP Tag
                            if (agmList.ContainsKey("SHOWINPAYSLIP".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "SHOWINPAYSLIP") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "SHOWINPAYSLIP");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<SHOWINPAYSLIP>" + rowValue.Trim() + "</SHOWINPAYSLIP>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "SHOWINPAYSLIP");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<SHOWINPAYSLIP>" + fieldVal.Trim() + "</SHOWINPAYSLIP>");
                                }
                            }
                            #endregion

                            #region For USEFORGRATUITY Tag
                            if (agmList.ContainsKey("USEFORGRATUITY".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "USEFORGRATUITY") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "USEFORGRATUITY");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<USEFORGRATUITY>" + rowValue.Trim() + "</USEFORGRATUITY>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "USEFORGRATUITY");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<USEFORGRATUITY>" + fieldVal.Trim() + "</USEFORGRATUITY>");
                                }
                            }
                            #endregion

                            #region For FORSERVICETAX Tag
                            if (agmList.ContainsKey("FORSERVICETAX".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "FORSERVICETAX") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "FORSERVICETAX");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<FORSERVICETAX>" + rowValue.Trim() + "</FORSERVICETAX>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "FORSERVICETAX");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<FORSERVICETAX>" + fieldVal.Trim() + "</FORSERVICETAX>");
                                }
                            }
                            #endregion

                            #region For ISINPUTCREDIT Tag
                            if (agmList.ContainsKey("ISINPUTCREDIT".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "ISINPUTCREDIT") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "ISINPUTCREDIT");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<ISINPUTCREDIT>" + rowValue.Trim() + "</ISINPUTCREDIT>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "ISINPUTCREDIT");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<ISINPUTCREDIT>" + fieldVal.Trim() + "</ISINPUTCREDIT>");
                                }
                            }
                            #endregion

                            #region For ISEXEMPTED Tag
                            if (agmList.ContainsKey("ISEXEMPTED".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "ISEXEMPTED") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "ISEXEMPTED");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<ISEXEMPTED>" + rowValue.Trim() + "</ISEXEMPTED>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "ISEXEMPTED");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<ISEXEMPTED>" + fieldVal.Trim() + "</ISEXEMPTED>");
                                }
                            }
                            #endregion

                            #region For ISABATEMENTAPPLICABLE Tag
                            if (agmList.ContainsKey("ISABATEMENTAPPLICABLE".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "ISABATEMENTAPPLICABLE") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "ISABATEMENTAPPLICABLE");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<ISABATEMENTAPPLICABLE>" + rowValue.Trim() + "</ISABATEMENTAPPLICABLE>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "ISABATEMENTAPPLICABLE");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<ISABATEMENTAPPLICABLE>" + fieldVal.Trim() + "</ISABATEMENTAPPLICABLE>");
                                }
                            }
                            #endregion

                            #region For TDSDEDUCTEEISSPECIALRATE Tag
                            if (agmList.ContainsKey("TDSDEDUCTEEISSPECIALRATE".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "TDSDEDUCTEEISSPECIALRATE") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "TDSDEDUCTEEISSPECIALRATE");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<TDSDEDUCTEEISSPECIALRATE>" + rowValue.Trim() + "</TDSDEDUCTEEISSPECIALRATE>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "TDSDEDUCTEEISSPECIALRATE");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<TDSDEDUCTEEISSPECIALRATE>" + fieldVal.Trim() + "</TDSDEDUCTEEISSPECIALRATE>");
                                }
                            }
                            #endregion

                            #region For AUDITED Tag
                            if (agmList.ContainsKey("AUDITED".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "AUDITED") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "AUDITED");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<AUDITED>" + rowValue.Trim() + "</AUDITED>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "AUDITED");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<AUDITED>" + fieldVal.Trim() + "</AUDITED>");
                                }
                            }
                            #endregion

                            #region For SORTPOSITION Tag
                            if (agmList.ContainsKey("SORTPOSITION".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "SORTPOSITION") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "SORTPOSITION");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<SORTPOSITION>" + rowValue.Trim() + "</SORTPOSITION>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "SORTPOSITION");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<SORTPOSITION>" + fieldVal.Trim() + "</SORTPOSITION>");
                                }
                            }
                            #endregion

                            #region For LANGUAGENAME.LIST Tag
                            if (agmList.ContainsKey("LANGUAGENAME.LIST".ToUpper()))
                            {
                                GenerateMasterXMLBuilder.AppendLine("<LANGUAGENAME.LIST>");
                                #region For NAME.LIST Tag
                                if (agmList.ContainsKey("NAME.LIST".ToUpper()))
                                {
                                    GenerateMasterXMLBuilder.AppendLine("<NAME.LIST>");
                                    #region For Name Tag with in the NAME.LIST
                                    if (agmList.ContainsKey("NAME".ToUpper()))
                                    {
                                        if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "NAME") == "Field")
                                        {
                                            fieldVal = this.GetFieldValue(agmList, "NAME");
                                            if (fieldVal != "")
                                            {
                                                if (row[fieldVal] != null)
                                                    rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                            }
                                            GenerateMasterXMLBuilder.AppendLine("<NAME>" + rowValue.Trim() + "</NAME>");
                                        }
                                        else
                                        {
                                            fieldVal = this.GetFieldValue(agmList, "NAME");
                                            if (fieldVal != "")
                                                fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                            GenerateMasterXMLBuilder.AppendLine("<NAME>" + fieldVal.Trim() + "</NAME>");
                                        }
                                    }
                                    #endregion
                                    GenerateMasterXMLBuilder.AppendLine("</NAME.LIST>");
                                }
                                #endregion
                                #region For LANGUAGEID Tag
                                if (agmList.ContainsKey("LANGUAGEID".ToUpper()))
                                {
                                    if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "LANGUAGEID") == "Field")
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "LANGUAGEID");
                                        if (fieldVal != "")
                                        {
                                            if (row[fieldVal] != null)
                                                rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                        }
                                        GenerateMasterXMLBuilder.AppendLine("<LANGUAGEID>" + rowValue.Trim() + "</LANGUAGEID>");
                                    }
                                    else
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "LANGUAGEID");
                                        if (fieldVal != "")
                                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                        GenerateMasterXMLBuilder.AppendLine("<LANGUAGEID>" + fieldVal.Trim() + "</LANGUAGEID>");
                                    }
                                }
                                #endregion
                                GenerateMasterXMLBuilder.AppendLine("</LANGUAGENAME.LIST>");
                            }
                            #endregion

                            #region For LEDGERAUDITCLASS.LIST Tag
                            if (agmList.ContainsKey("LEDGERAUDITCLASS.LIST".ToUpper()))
                            {
                                GenerateMasterXMLBuilder.AppendLine("<LEDGERAUDITCLASS.LIST>");

                                #region For LEDAUDITPERIOD.LIST  Tag
                                if (agmList.ContainsKey("LEDAUDITPERIOD.LIST".ToUpper()))
                                {
                                    if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "LEDAUDITPERIOD.LIST") == "Field")
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "LEDAUDITPERIOD.LIST");
                                        if (fieldVal != "")
                                        {
                                            if (row[fieldVal] != null)
                                                rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                        }
                                        GenerateMasterXMLBuilder.AppendLine("<LEDAUDITPERIOD.LIST>" + rowValue.Trim() + "</LEDAUDITPERIOD.LIST >");
                                    }
                                    else
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "LEDAUDITPERIOD.LIST");
                                        if (fieldVal != "")
                                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                        GenerateMasterXMLBuilder.AppendLine("<LEDAUDITPERIOD.LIST>" + fieldVal.Trim() + "</LEDAUDITPERIOD.LIST >");
                                    }
                                }
                                #endregion

                                GenerateMasterXMLBuilder.AppendLine("</LEDGERAUDITCLASS.LIST>");
                            }
                            #endregion

                            #region For Rest Of Tags
                            if (agmList.Count > 45)
                            {
                                foreach (string key in agmList.Keys)
                                {
                                    if (key.ToUpper() != "LEDGER".ToUpper() && key.ToUpper() != "NAME.LIST".ToUpper() && key.ToUpper() != "NAME".ToUpper() && key.ToUpper() != "PARENT".ToUpper() && key.ToUpper() != "ISBILLWISEON".ToUpper() && key.ToUpper() != "MAILINGNAME.LIST".ToUpper() && key.ToUpper() != "ISCOSTCENTRESON".ToUpper() && key.ToUpper() != "MAILINGNAME".ToUpper()
                                        && key.ToUpper() != "CURRENCYNAME".ToUpper() && key.ToUpper() != "TAXCLASSIFICATIONNAME".ToUpper() && key.ToUpper() != "TAXTYPE".ToUpper() && key.ToUpper() != "GSTTYPE ".ToUpper() && key.ToUpper() != "SERVICECATEGORY ".ToUpper() && key.ToUpper() != "EXCISEDUTYTYPE".ToUpper() && key.ToUpper() != "TRADERLEDNATUREOFPURCHASE".ToUpper()
                                        && key.ToUpper() != "TDSDEDUCTEETYPE".ToUpper() && key.ToUpper() != "TDSRATENAME".ToUpper() && key.ToUpper() != "LEDGERFBTCATEGORY".ToUpper() && key.ToUpper() != "ISCOSTCENTRESON".ToUpper() && key.ToUpper() != "ISINTERESTON".ToUpper() && key.ToUpper() != "ALLOWINMOBILE".ToUpper() && key.ToUpper() != "ISCONDENSED".ToUpper() && key.ToUpper() != "AFFECTSSTOCK".ToUpper()
                                        && key.ToUpper() != "FORPAYROLL".ToUpper() && key.ToUpper() != "INTERESTONBILLWISE".ToUpper() && key.ToUpper() != "OVERRIDEINTEREST".ToUpper() && key.ToUpper() != "OVERRIDEADVINTEREST".ToUpper() && key.ToUpper() != "USEFORVAT".ToUpper() && key.ToUpper() != "IGNORETDSEXEMPT".ToUpper() && key.ToUpper() != "ISTCSAPPLICABLE".ToUpper() && key.ToUpper() != "ISTDSAPPLICABLE".ToUpper()
                                        && key.ToUpper() != "ISFBTAPPLICABLE".ToUpper() && key.ToUpper() != "ISGSTAPPLICABLE".ToUpper() && key.ToUpper() != "SHOWINPAYSLIP".ToUpper() && key.ToUpper() != "USEFORGRATUITY".ToUpper() && key.ToUpper() != "FORSERVICETAX".ToUpper() && key.ToUpper() != "ISINPUTCREDIT".ToUpper() && key.ToUpper() != "ISEXEMPTED".ToUpper() && key.ToUpper() != "ISABATEMENTAPPLICABLE".ToUpper()
                                        && key.ToUpper() != "TDSDEDUCTEEISSPECIALRATE".ToUpper() && key.ToUpper() != "AUDITED".ToUpper() && key.ToUpper() != "SORTPOSITION".ToUpper() && key.ToUpper() != "LANGUAGENAME.LIST".ToUpper() && key.ToUpper() != "LANGUAGEID".ToUpper() && key.ToUpper() != "LEDGERAUDITCLASS.LIST".ToUpper() && key.ToUpper() != "LEDAUDITPERIOD.LIST".ToUpper())
                                    {
                                        if (VerifyDefaultOrField(defaultvalues, Fieldvalues, key.ToUpper()) == "Field")
                                        {
                                            fieldVal = this.GetFieldValue(agmList, key.ToUpper());
                                            if (fieldVal != "")
                                            {
                                                if (row[fieldVal] != null)
                                                    rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                            }
                                            GenerateMasterXMLBuilder.AppendLine("<" + key.ToUpper() + ">" + rowValue.Trim() + "</" + key.ToUpper() + ">");
                                        }
                                        else
                                        {
                                            fieldVal = this.GetFieldValue(agmList, key.ToUpper());
                                            if (fieldVal != "")
                                                fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                            GenerateMasterXMLBuilder.AppendLine("<" + key.ToUpper() + ">" + fieldVal.Trim() + "</" + key.ToUpper() + ">");
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region GRUPING END TAGS
                            if (agmList.ContainsKey("LEDGER".ToUpper()))
                            {
                                GenerateMasterXMLBuilder.AppendLine("</LEDGER>");
                            }
                            #endregion

                            GenerateMasterXMLBuilder.AppendLine("</TALLYMESSAGE>");
                        }//
                        #endregion
                    }
                    else if (ApplicationValues.GenaretingMasterName.ToUpper() == "Item Master".ToUpper())
                    {
                        #region At least One Field Value is there in settins File for Item Master
                        #region Geting the Data Base Table Name
                        agmList.TryGetValue("STOCKITEM".ToUpper(), out strTblDtls);
                        splitstring = strTblDtls.Split(new char[] { '.' });
                        if (splitstring.Length > 0)
                        {
                            foreach (string strvalue in splitstring)
                            {
                                if (strvalue.Contains("U2T"))
                                {
                                    tablename = strvalue;
                                    int num1 = tablename.IndexOf('_', 0);
                                    tablename = tablename.Substring(num1 + 1, (tablename.Length - (num1 + 1)));
                                    if (tablename != string.Empty)
                                    {
                                        ApplicationValues.tableName = tablename;
                                        break;
                                    }
                                    else
                                    {
                                        ApplicationValues.tableName = "IT_MAST";
                                        break;
                                    }
                                }
                                else
                                {
                                    fieldValue = strvalue;
                                    ApplicationValues.tableName = "IT_MAST";
                                    break;
                                }
                            }
                        }
                        else
                        {
                            tablename = "IT_MAST";
                            ApplicationValues.tableName = tablename;
                        }
                        #endregion
                        objGenerateMasterXML.MasterTableName = tablename;
                        objGenerateMasterXML.TallyTagsTable = this.TallyTagsTable;
                        DSMaster = objGenerateMasterXML.GenerateMaster(this);
                        string rowValue = string.Empty;
                        GenerateMasterXMLBuilder = new StringBuilder();
                        foreach (DataRow row in DSMaster.Tables[0].Rows)
                        {
                            GenerateMasterXMLBuilder.AppendLine("<TALLYMESSAGE xmlns:UDF='TallyUDF'>");
                            string fieldVal = string.Empty;
                            #region For STOCKITEM Tag
                            if (agmList.ContainsKey("STOCKITEM".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "STOCKITEM") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "STOCKITEM");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<STOCKITEM Name='" + rowValue.Trim() + "' RESERVEDNAME=''>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "STOCKITEM");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<STOCKITEM Name='" + fieldVal.Trim() + "' RESERVEDNAME=''>");
                                }
                            }
                            #endregion

                            #region For ADDITIONALNAME.LIST Tag
                            if (agmList.ContainsKey("ADDITIONALNAME.LIST".ToUpper()))
                            {
                                GenerateMasterXMLBuilder.AppendLine("<ADDITIONALNAME.LIST>");
                                #region For MAILINGNAME Tag with in the ADDITIONALNAME
                                if (agmList.ContainsKey("ADDITIONALNAME".ToUpper()))
                                {
                                    if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "ADDITIONALNAME") == "Field")
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "ADDITIONALNAME");
                                        if (fieldVal != "")
                                        {
                                            if (row[fieldVal] != null)
                                                rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                        }
                                        GenerateMasterXMLBuilder.AppendLine("<ADDITIONALNAME>" + rowValue.Trim() + "</ADDITIONALNAME>");
                                    }
                                    else
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "ADDITIONALNAME");
                                        if (fieldVal != "")
                                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                        GenerateMasterXMLBuilder.AppendLine("<ADDITIONALNAME>" + fieldVal.Trim() + "</ADDITIONALNAME>");
                                    }
                                }
                                #endregion
                                GenerateMasterXMLBuilder.AppendLine("</ADDITIONALNAME.LIST>");
                            }
                            #endregion

                            #region for PARENT Tag
                            if (agmList.ContainsKey("PARENT".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "PARENT") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "PARENT");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }

                                    GenerateMasterXMLBuilder.AppendLine("<PARENT>" + rowValue.Trim() + "</PARENT>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "PARENT");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<PARENT>" + fieldVal.Trim() + "</PARENT>");
                                }
                            }
                            #endregion

                            #region For CATEGORY Tag
                            if (agmList.ContainsKey("CATEGORY".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "CATEGORY") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "CATEGORY");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<CATEGORY>" + rowValue.Trim() + "</CATEGORY>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "CATEGORY");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<CATEGORY>" + fieldVal.Trim() + "</CATEGORY>");
                                }
                            }
                            #endregion

                            #region For TAXCLASSIFICATIONNAME Tag
                            if (agmList.ContainsKey("TAXCLASSIFICATIONNAME".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "TAXCLASSIFICATIONNAME") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "TAXCLASSIFICATIONNAME");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<TAXCLASSIFICATIONNAME>" + rowValue.Trim() + "</TAXCLASSIFICATIONNAME>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "TAXCLASSIFICATIONNAME");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<TAXCLASSIFICATIONNAME>" + fieldVal.Trim() + "</TAXCLASSIFICATIONNAME>");
                                }
                            }
                            #endregion

                            #region For BASEUNITS Tag
                            if (agmList.ContainsKey("BASEUNITS".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "BASEUNITS") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "BASEUNITS");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<BASEUNITS>" + rowValue.Trim() + "</BASEUNITS>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "BASEUNITS");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<BASEUNITS>" + fieldVal.Trim() + "</BASEUNITS>");
                                }
                            }
                            #endregion

                            #region For ADDITIONALUNITS Tag
                            if (agmList.ContainsKey("ADDITIONALUNITS".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "ADDITIONALUNITS") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "ADDITIONALUNITS");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<ADDITIONALUNITS>" + rowValue.Trim() + "</ADDITIONALUNITS>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "ADDITIONALUNITS");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<ADDITIONALUNITS>" + fieldVal.Trim() + "</ADDITIONALUNITS>");
                                }
                            }
                            #endregion

                            #region For ISCOSTCENTRESON Tag
                            if (agmList.ContainsKey("ISCOSTCENTRESON".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "ISCOSTCENTRESON") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "ISCOSTCENTRESON");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<ISCOSTCENTRESON>" + rowValue.Trim() + "</ISCOSTCENTRESON>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "ISCOSTCENTRESON");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<ISCOSTCENTRESON>" + fieldVal.Trim() + "</ISCOSTCENTRESON>");
                                }
                            }
                            #endregion

                            #region For ISBATCHWISEON Tag
                            if (agmList.ContainsKey("ISBATCHWISEON".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "ISBATCHWISEON") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "ISBATCHWISEON");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<ISBATCHWISEON>" + rowValue.Trim() + "</ISBATCHWISEON>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "ISBATCHWISEON");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<ISBATCHWISEON>" + fieldVal.Trim() + "</ISBATCHWISEON>");
                                }
                            }
                            #endregion

                            #region For ISPERISHABLEON Tag
                            if (agmList.ContainsKey("ISPERISHABLEON".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "ISPERISHABLEON") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "ISPERISHABLEON");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<ISPERISHABLEON>" + rowValue.Trim() + "</ISPERISHABLEON>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "ISPERISHABLEON");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<ISPERISHABLEON>" + fieldVal.Trim() + "</ISPERISHABLEON>");
                                }
                            }
                            #endregion

                            #region For IGNOREPHYSICALDIFFERENCE Tag
                            if (agmList.ContainsKey("IGNOREPHYSICALDIFFERENCE".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "IGNOREPHYSICALDIFFERENCE") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "IGNOREPHYSICALDIFFERENCE");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<IGNOREPHYSICALDIFFERENCE>" + rowValue.Trim() + "</IGNOREPHYSICALDIFFERENCE>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "IGNOREPHYSICALDIFFERENCE");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<IGNOREPHYSICALDIFFERENCE>" + fieldVal.Trim() + "</IGNOREPHYSICALDIFFERENCE>");
                                }
                            }
                            #endregion

                            #region for IGNORENEGATIVESTOCK Tag
                            if (agmList.ContainsKey("IGNORENEGATIVESTOCK".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "IGNORENEGATIVESTOCK") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "IGNORENEGATIVESTOCK");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }

                                    GenerateMasterXMLBuilder.AppendLine("<IGNORENEGATIVESTOCK>" + rowValue.Trim() + "</IGNORENEGATIVESTOCK>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "IGNORENEGATIVESTOCK");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<IGNORENEGATIVESTOCK>" + fieldVal.Trim() + "</IGNORENEGATIVESTOCK>");
                                }
                            }
                            #endregion

                            #region For TREATSALESASMANUFACTURED Tag
                            if (agmList.ContainsKey("TREATSALESASMANUFACTURED".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "TREATSALESASMANUFACTURED") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "TREATSALESASMANUFACTURED");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<TREATSALESASMANUFACTURED>" + rowValue.Trim() + "</TREATSALESASMANUFACTURED>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "TREATSALESASMANUFACTURED");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<TREATSALESASMANUFACTURED>" + fieldVal.Trim() + "</TREATSALESASMANUFACTURED>");
                                }
                            }
                            #endregion

                            #region For TREATPURCHASESASCONSUMED Tag
                            if (agmList.ContainsKey("TREATPURCHASESASCONSUMED".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "TREATPURCHASESASCONSUMED") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "TREATPURCHASESASCONSUMED");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<TREATPURCHASESASCONSUMED>" + rowValue.Trim() + "</TREATPURCHASESASCONSUMED>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "TREATPURCHASESASCONSUMED");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<TREATPURCHASESASCONSUMED>" + fieldVal.Trim() + "</TREATPURCHASESASCONSUMED>");
                                }
                            }
                            #endregion

                            #region For TREATREJECTSASSCRAP Tag
                            if (agmList.ContainsKey("TREATREJECTSASSCRAP".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "TREATREJECTSASSCRAP") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "TREATREJECTSASSCRAP");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<TREATREJECTSASSCRAP>" + rowValue.Trim() + "</TREATREJECTSASSCRAP>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "TREATREJECTSASSCRAP");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<TREATREJECTSASSCRAP>" + fieldVal.Trim() + "</TREATREJECTSASSCRAP>");
                                }
                            }
                            #endregion

                            #region For HASMFGDATE Tag
                            if (agmList.ContainsKey("HASMFGDATE".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "HASMFGDATE") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "HASMFGDATE");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<HASMFGDATE>" + rowValue.Trim() + "</HASMFGDATE>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "HASMFGDATE");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<HASMFGDATE>" + fieldVal.Trim() + "</HASMFGDATE>");
                                }
                            }
                            #endregion

                            #region For ALLOWUSEOFEXPIREDITEMS Tag
                            if (agmList.ContainsKey("ALLOWUSEOFEXPIREDITEMS".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "ALLOWUSEOFEXPIREDITEMS") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "ALLOWUSEOFEXPIREDITEMS");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<ALLOWUSEOFEXPIREDITEMS>" + rowValue.Trim() + "</ALLOWUSEOFEXPIREDITEMS>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "ALLOWUSEOFEXPIREDITEMS");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<ALLOWUSEOFEXPIREDITEMS>" + fieldVal.Trim() + "</ALLOWUSEOFEXPIREDITEMS>");
                                }
                            }
                            #endregion

                            #region For IGNOREBATCHES Tag
                            if (agmList.ContainsKey("IGNOREBATCHES".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "IGNOREBATCHES") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "IGNOREBATCHES");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<IGNOREBATCHES>" + rowValue.Trim() + "</IGNOREBATCHES>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "IGNOREBATCHES");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<IGNOREBATCHES>" + fieldVal.Trim() + "</IGNOREBATCHES>");
                                }
                            }
                            #endregion

                            #region For IGNOREGODOWNS Tag
                            if (agmList.ContainsKey("IGNOREGODOWNS".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "IGNOREGODOWNS") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "IGNOREGODOWNS");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<IGNOREGODOWNS>" + rowValue.Trim() + "</IGNOREGODOWNS>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "IGNOREGODOWNS");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<IGNOREGODOWNS>" + fieldVal.Trim() + "</IGNOREGODOWNS>");
                                }
                            }
                            #endregion

                            #region For ISMRPINCLOFTAX Tag
                            if (agmList.ContainsKey("ISMRPINCLOFTAX".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "ISMRPINCLOFTAX") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "ISMRPINCLOFTAX");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<ISMRPINCLOFTAX>" + rowValue.Trim() + "</ISMRPINCLOFTAX>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "ISMRPINCLOFTAX");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<ISMRPINCLOFTAX>" + fieldVal.Trim() + "</ISMRPINCLOFTAX>");
                                }
                            }
                            #endregion

                            #region For REORDERASHIGHER Tag
                            if (agmList.ContainsKey("REORDERASHIGHER".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "REORDERASHIGHER") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "REORDERASHIGHER");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<REORDERASHIGHER>" + rowValue.Trim() + "</REORDERASHIGHER>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "REORDERASHIGHER");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<REORDERASHIGHER>" + fieldVal.Trim() + "</REORDERASHIGHER>");
                                }
                            }
                            #endregion

                            #region For MINORDERASHIGHER Tag
                            if (agmList.ContainsKey("MINORDERASHIGHER".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "MINORDERASHIGHER") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "MINORDERASHIGHER");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<MINORDERASHIGHER>" + rowValue.Trim() + "</MINORDERASHIGHER>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "MINORDERASHIGHER");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<MINORDERASHIGHER>" + fieldVal.Trim() + "</MINORDERASHIGHER>");
                                }
                            }
                            #endregion

                            #region For DENOMINATOR Tag
                            if (agmList.ContainsKey("DENOMINATOR".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "DENOMINATOR") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "DENOMINATOR");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<DENOMINATOR>" + rowValue.Trim() + "</DENOMINATOR>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "DENOMINATOR");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<DENOMINATOR>" + fieldVal.Trim() + "</DENOMINATOR>");
                                }
                            }
                            #endregion

                            #region For RATEOFVAT Tag
                            if (agmList.ContainsKey("RATEOFVAT".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "RATEOFVAT") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "RATEOFVAT");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<RATEOFVAT>" + rowValue.Trim() + "</RATEOFVAT>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "RATEOFVAT");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<RATEOFVAT>" + fieldVal.Trim() + "</RATEOFVAT>");
                                }
                            }
                            #endregion

                            #region For LANGUAGENAME.LIST Tag
                            if (agmList.ContainsKey("LANGUAGENAME.LIST".ToUpper()))
                            {
                                GenerateMasterXMLBuilder.AppendLine("<LANGUAGENAME.LIST>");
                                #region For NAME.LIST Tag
                                if (agmList.ContainsKey("NAME.LIST".ToUpper()))
                                {
                                    GenerateMasterXMLBuilder.AppendLine("<NAME.LIST>");
                                    #region For Name Tag with in the NAME.LIST
                                    if (agmList.ContainsKey("NAME".ToUpper()))
                                    {
                                        if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "NAME") == "Field")
                                        {
                                            fieldVal = this.GetFieldValue(agmList, "NAME");
                                            if (fieldVal != "")
                                            {
                                                if (row[fieldVal] != null)
                                                    rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                            }
                                            GenerateMasterXMLBuilder.AppendLine("<NAME>" + rowValue.Trim() + "</NAME>");
                                        }
                                        else
                                        {
                                            fieldVal = this.GetFieldValue(agmList, "NAME");
                                            if (fieldVal != "")
                                                fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                            GenerateMasterXMLBuilder.AppendLine("<NAME>" + fieldVal.Trim() + "</NAME>");
                                        }
                                    }
                                    #endregion
                                    GenerateMasterXMLBuilder.AppendLine("</NAME.LIST>");
                                }
                                #endregion
                                #region For LANGUAGEID Tag
                                if (agmList.ContainsKey("LANGUAGEID".ToUpper()))
                                {
                                    if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "LANGUAGEID") == "Field")
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "LANGUAGEID");
                                        if (fieldVal != "")
                                        {
                                            if (row[fieldVal] != null)
                                                rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                        }
                                        GenerateMasterXMLBuilder.AppendLine("<LANGUAGEID>" + rowValue.Trim() + "</LANGUAGEID>");
                                    }
                                    else
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "LANGUAGEID");
                                        if (fieldVal != "")
                                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                        GenerateMasterXMLBuilder.AppendLine("<LANGUAGEID>" + fieldVal.Trim() + "</LANGUAGEID>");
                                    }
                                }
                                #endregion
                                GenerateMasterXMLBuilder.AppendLine("</LANGUAGENAME.LIST>");
                            }
                            #endregion

                            #region For ADDITIONALLEDGERS.LIST Tag
                            if (agmList.ContainsKey("ADDITIONALLEDGERS.LIST".ToUpper()))
                            {
                                if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "ADDITIONALLEDGERS.LIST") == "Field")
                                {
                                    fieldVal = this.GetFieldValue(agmList, "ADDITIONALLEDGERS.LIST");
                                    if (fieldVal != "")
                                    {
                                        if (row[fieldVal] != null)
                                            rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                    }
                                    GenerateMasterXMLBuilder.AppendLine("<ADDITIONALLEDGERS.LIST>" + rowValue.Trim() + "</ADDITIONALLEDGERS.LIST>");
                                }
                                else
                                {
                                    fieldVal = this.GetFieldValue(agmList, "ADDITIONALLEDGERS.LIST");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<ADDITIONALLEDGERS.LIST>" + fieldVal.Trim() + "</ADDITIONALLEDGERS.LIST>");
                                }
                            }
                            #endregion

                            #region For Rest Of Tags
                            if (agmList.Count > 45)
                            {
                                foreach (string key in agmList.Keys)
                                {
                                    if (key.ToUpper() != "STOCKITEM".ToUpper() && key.ToUpper() != "ADDITIONALNAME.LIST".ToUpper() && key.ToUpper() != "ADDITIONALNAME".ToUpper() && key.ToUpper() != "PARENT".ToUpper() && key.ToUpper() != "CATEGORY".ToUpper() && key.ToUpper() != "TAXCLASSIFICATIONNAME".ToUpper() && key.ToUpper() != "BASEUNITS".ToUpper() && key.ToUpper() != "ADDITIONALUNITS".ToUpper()
                                        && key.ToUpper() != "ISCOSTCENTRESON".ToUpper() && key.ToUpper() != "ISBATCHWISEON".ToUpper() && key.ToUpper() != "ISPERISHABLEON".ToUpper() && key.ToUpper() != "IGNOREPHYSICALDIFFERENCE ".ToUpper() && key.ToUpper() != "IGNORENEGATIVESTOCK ".ToUpper() && key.ToUpper() != "TREATSALESASMANUFACTURED".ToUpper() && key.ToUpper() != "TREATPURCHASESASCONSUMED".ToUpper()
                                        && key.ToUpper() != "TREATREJECTSASSCRAP".ToUpper() && key.ToUpper() != "HASMFGDATE".ToUpper() && key.ToUpper() != "ALLOWUSEOFEXPIREDITEMS".ToUpper() && key.ToUpper() != "IGNOREBATCHES".ToUpper() && key.ToUpper() != "IGNOREGODOWNS".ToUpper() && key.ToUpper() != "ISMRPINCLOFTAX".ToUpper() && key.ToUpper() != "REORDERASHIGHER".ToUpper() && key.ToUpper() != "MINORDERASHIGHER".ToUpper()
                                        && key.ToUpper() != "DENOMINATOR".ToUpper() && key.ToUpper() != "RATEOFVAT".ToUpper() && key.ToUpper() != "LANGUAGENAME.LIST".ToUpper() && key.ToUpper() != "NAME.LIST".ToUpper() && key.ToUpper() != "NAME".ToUpper() && key.ToUpper() != "LANGUAGEID".ToUpper() && key.ToUpper() != "ADDITIONALLEDGERS.LIST".ToUpper())
                                    {
                                        if (VerifyDefaultOrField(defaultvalues, Fieldvalues, key.ToUpper()) == "Field")
                                        {
                                            fieldVal = this.GetFieldValue(agmList, key.ToUpper());
                                            if (fieldVal != "")
                                            {
                                                if (row[fieldVal] != null)
                                                    rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                            }
                                            GenerateMasterXMLBuilder.AppendLine("<" + key.ToUpper() + ">" + rowValue.Trim() + "</" + key.ToUpper() + ">");
                                        }
                                        else
                                        {
                                            fieldVal = this.GetFieldValue(agmList, key.ToUpper());
                                            if (fieldVal != "")
                                                fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                            GenerateMasterXMLBuilder.AppendLine("<" + key.ToUpper() + ">" + fieldVal.Trim() + "</" + key.ToUpper() + ">");
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region STOCKITEM END TAGS
                            if (agmList.ContainsKey("STOCKITEM".ToUpper()))
                            {
                                GenerateMasterXMLBuilder.AppendLine("</STOCKITEM>");
                            }
                            #endregion

                            GenerateMasterXMLBuilder.AppendLine("</TALLYMESSAGE>");
                        }//
                        #endregion
                    }
                }
                else
                {
                    isNeedToUpdate = false;
                    if (ApplicationValues.GenaretingMasterName.ToUpper() == "Account Master".ToUpper())
                    {
                        #region In Settings Page All Values Are Have Default Values for Account Master
                        for (int i = 0; i < agmList.Count; i++)
                        {
                            GenerateMasterXMLBuilder.AppendLine("<TALLYMESSAGE xmlns:UDF='TallyUDF'>");
                            string fieldVal = string.Empty;
                            #region For LEDGER Tag
                            if (agmList.ContainsKey("LEDGER".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "LEDGER");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<LEDGER Name='" + fieldVal.Trim() + "' RESERVEDNAME=''  ACTION='Create'>");
                            }
                            #endregion

                            #region For MAILINGNAME.LIST Tag
                            if (agmList.ContainsKey("MAILINGNAME.LIST".ToUpper()))
                            {
                                GenerateMasterXMLBuilder.AppendLine("<MAILINGNAME.LIST>");
                                #region For MAILINGNAME Tag with in the MAILINGNAME.LIST
                                if (agmList.ContainsKey("MAILINGNAME".ToUpper()))
                                {
                                    fieldVal = this.GetFieldValue(agmList, "MAILINGNAME");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<MAILINGNAME>" + fieldVal.Trim() + "</MAILINGNAME>");
                                }
                                #endregion
                                GenerateMasterXMLBuilder.AppendLine("</MAILINGNAME.LIST>");
                            }
                            #endregion

                            #region For CURRENCYNAME Tag
                            if (agmList.ContainsKey("CURRENCYNAME".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "CURRENCYNAME");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<CURRENCYNAME>" + fieldVal.Trim() + "</CURRENCYNAME>");
                            }
                            #endregion

                            #region for PARENT Tag
                            if (agmList.ContainsKey("PARENT".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "PARENT");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<PARENT>" + fieldVal.Trim() + "</PARENT>");
                            }
                            #endregion

                            #region For TAXCLASSIFICATIONNAME Tag
                            if (agmList.ContainsKey("TAXCLASSIFICATIONNAME".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "TAXCLASSIFICATIONNAME");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<TAXCLASSIFICATIONNAME>" + fieldVal.Trim() + "</TAXCLASSIFICATIONNAME>");
                            }
                            #endregion

                            #region For TAXTYPE Tag
                            if (agmList.ContainsKey("TAXTYPE".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "TAXTYPE");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<TAXTYPE>" + fieldVal.Trim() + "</TAXTYPE>");
                            }
                            #endregion

                            #region For GSTTYPE Tag
                            if (agmList.ContainsKey("GSTTYPE".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "GSTTYPE");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<GSTTYPE>" + fieldVal.Trim() + "</GSTTYPE>");
                            }
                            #endregion

                            #region For SERVICECATEGORY Tag
                            if (agmList.ContainsKey("SERVICECATEGORY".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "SERVICECATEGORY");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<SERVICECATEGORY>" + fieldVal.Trim() + "</SERVICECATEGORY>");
                            }
                            #endregion

                            #region For EXCISEDUTYTYPE Tag
                            if (agmList.ContainsKey("EXCISEDUTYTYPE".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "EXCISEDUTYTYPE");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<EXCISEDUTYTYPE>" + fieldVal.Trim() + "</EXCISEDUTYTYPE>");
                            }
                            #endregion

                            #region For TRADERLEDNATUREOFPURCHASE Tag
                            if (agmList.ContainsKey("TRADERLEDNATUREOFPURCHASE".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "TRADERLEDNATUREOFPURCHASE");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<TRADERLEDNATUREOFPURCHASE>" + fieldVal.Trim() + "</TRADERLEDNATUREOFPURCHASE>");
                            }
                            #endregion

                            #region For TDSDEDUCTEETYPE Tag
                            if (agmList.ContainsKey("TDSDEDUCTEETYPE".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "TDSDEDUCTEETYPE");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<TDSDEDUCTEETYPE>" + fieldVal.Trim() + "</TDSDEDUCTEETYPE>");
                            }
                            #endregion

                            #region For TDSRATENAME Tag
                            if (agmList.ContainsKey("TDSRATENAME".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "TDSRATENAME");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<TDSRATENAME>" + fieldVal.Trim() + "</TDSRATENAME>");
                            }
                            #endregion

                            #region For LEDGERFBTCATEGORY Tag
                            if (agmList.ContainsKey("LEDGERFBTCATEGORY".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "LEDGERFBTCATEGORY");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<LEDGERFBTCATEGORY>" + fieldVal.Trim() + "</LEDGERFBTCATEGORY>");
                            }
                            #endregion

                            #region For ISBILLWISEON Tag
                            if (agmList.ContainsKey("ISBILLWISEON".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "ISBILLWISEON");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<ISBILLWISEON>" + fieldVal.Trim() + "</ISBILLWISEON>");
                            }
                            #endregion

                            #region For ISCOSTCENTRESON Tag
                            if (agmList.ContainsKey("ISCOSTCENTRESON".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "ISCOSTCENTRESON");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<ISCOSTCENTRESON>" + fieldVal.Trim() + "</ISCOSTCENTRESON>");
                            }
                            #endregion

                            #region For ISINTERESTON Tag
                            if (agmList.ContainsKey("ISINTERESTON".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "ISINTERESTON");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<ISINTERESTON>" + fieldVal.Trim() + "</ISINTERESTON>");
                            }
                            #endregion

                            #region For ALLOWINMOBILE Tag
                            if (agmList.ContainsKey("ALLOWINMOBILE".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "ALLOWINMOBILE");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<ALLOWINMOBILE>" + fieldVal.Trim() + "</ALLOWINMOBILE>");
                            }
                            #endregion

                            #region For ISCONDENSED Tag
                            if (agmList.ContainsKey("ISCONDENSED".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "ISCONDENSED");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<ISCONDENSED>" + fieldVal.Trim() + "</ISCONDENSED>");
                            }
                            #endregion

                            #region For AFFECTSSTOCK Tag
                            if (agmList.ContainsKey("AFFECTSSTOCK".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "AFFECTSSTOCK");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<AFFECTSSTOCK>" + fieldVal.Trim() + "</AFFECTSSTOCK>");
                            }
                            #endregion

                            #region For FORPAYROLL Tag
                            if (agmList.ContainsKey("FORPAYROLL".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "FORPAYROLL");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<FORPAYROLL>" + fieldVal.Trim() + "</FORPAYROLL>");
                            }
                            #endregion

                            #region For INTERESTONBILLWISE Tag
                            if (agmList.ContainsKey("INTERESTONBILLWISE".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "INTERESTONBILLWISE");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<INTERESTONBILLWISE>" + fieldVal.Trim() + "</INTERESTONBILLWISE>");
                            }
                            #endregion

                            #region For OVERRIDEINTEREST Tag
                            if (agmList.ContainsKey("OVERRIDEINTEREST".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "OVERRIDEINTEREST");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<OVERRIDEINTEREST>" + fieldVal.Trim() + "</OVERRIDEINTEREST>");
                            }
                            #endregion

                            #region For OVERRIDEADVINTEREST Tag
                            if (agmList.ContainsKey("OVERRIDEADVINTEREST".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "OVERRIDEADVINTEREST");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<OVERRIDEADVINTEREST>" + fieldVal.Trim() + "</OVERRIDEADVINTEREST>");
                            }
                            #endregion

                            #region For USEFORVAT Tag
                            if (agmList.ContainsKey("USEFORVAT".ToUpper()))
                            {

                                fieldVal = this.GetFieldValue(agmList, "USEFORVAT");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<USEFORVAT>" + fieldVal.Trim() + "</USEFORVAT>");
                            }
                            #endregion

                            #region For IGNORETDSEXEMPT Tag
                            if (agmList.ContainsKey("IGNORETDSEXEMPT".ToUpper()))
                            {

                                fieldVal = this.GetFieldValue(agmList, "IGNORETDSEXEMPT");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<IGNORETDSEXEMPT>" + fieldVal.Trim() + "</IGNORETDSEXEMPT>");
                            }
                            #endregion

                            #region For ISTCSAPPLICABLE Tag
                            if (agmList.ContainsKey("ISTCSAPPLICABLE".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "ISTCSAPPLICABLE");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<ISTCSAPPLICABLE>" + fieldVal.Trim() + "</ISTCSAPPLICABLE>");
                            }
                            #endregion

                            #region For ISTDSAPPLICABLE Tag
                            if (agmList.ContainsKey("ISTDSAPPLICABLE".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "ISTDSAPPLICABLE");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<ISTDSAPPLICABLE>" + fieldVal.Trim() + "</ISTDSAPPLICABLE>");
                            }
                            #endregion

                            #region For ISFBTAPPLICABLE Tag
                            if (agmList.ContainsKey("ISFBTAPPLICABLE".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "ISFBTAPPLICABLE");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<ISFBTAPPLICABLE>" + fieldVal.Trim() + "</ISFBTAPPLICABLE>");
                            }
                            #endregion

                            #region For ISGSTAPPLICABLE Tag
                            if (agmList.ContainsKey("ISGSTAPPLICABLE".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "ISGSTAPPLICABLE");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<ISGSTAPPLICABLE>" + fieldVal.Trim() + "</ISGSTAPPLICABLE>");
                            }
                            #endregion

                            #region For SHOWINPAYSLIP Tag
                            if (agmList.ContainsKey("SHOWINPAYSLIP".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "SHOWINPAYSLIP");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<SHOWINPAYSLIP>" + fieldVal.Trim() + "</SHOWINPAYSLIP>");
                            }
                            #endregion

                            #region For USEFORGRATUITY Tag
                            if (agmList.ContainsKey("USEFORGRATUITY".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "USEFORGRATUITY");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<USEFORGRATUITY>" + fieldVal.Trim() + "</USEFORGRATUITY>");
                            }
                            #endregion

                            #region For FORSERVICETAX Tag
                            if (agmList.ContainsKey("FORSERVICETAX".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "FORSERVICETAX");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<FORSERVICETAX>" + fieldVal.Trim() + "</FORSERVICETAX>");
                            }
                            #endregion

                            #region For ISINPUTCREDIT Tag
                            if (agmList.ContainsKey("ISINPUTCREDIT".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "ISINPUTCREDIT");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<ISINPUTCREDIT>" + fieldVal.Trim() + "</ISINPUTCREDIT>");
                            }
                            #endregion

                            #region For ISEXEMPTED Tag
                            if (agmList.ContainsKey("ISEXEMPTED".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "ISEXEMPTED");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<ISEXEMPTED>" + fieldVal.Trim() + "</ISEXEMPTED>");
                            }
                            #endregion

                            #region For ISABATEMENTAPPLICABLE Tag
                            if (agmList.ContainsKey("ISABATEMENTAPPLICABLE".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "ISABATEMENTAPPLICABLE");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<ISABATEMENTAPPLICABLE>" + fieldVal.Trim() + "</ISABATEMENTAPPLICABLE>");
                            }
                            #endregion

                            #region For TDSDEDUCTEEISSPECIALRATE Tag
                            if (agmList.ContainsKey("TDSDEDUCTEEISSPECIALRATE".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "TDSDEDUCTEEISSPECIALRATE");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<TDSDEDUCTEEISSPECIALRATE>" + fieldVal.Trim() + "</TDSDEDUCTEEISSPECIALRATE>");
                            }
                            #endregion

                            #region For AUDITED Tag
                            if (agmList.ContainsKey("AUDITED".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "AUDITED");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<AUDITED>" + fieldVal.Trim() + "</AUDITED>");
                            }
                            #endregion

                            #region For SORTPOSITION Tag
                            if (agmList.ContainsKey("SORTPOSITION".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "SORTPOSITION");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<SORTPOSITION>" + fieldVal.Trim() + "</SORTPOSITION>");
                            }
                            #endregion

                            #region For LANGUAGENAME.LIST Tag
                            if (agmList.ContainsKey("LANGUAGENAME.LIST".ToUpper()))
                            {
                                GenerateMasterXMLBuilder.AppendLine("<LANGUAGENAME.LIST>");
                                #region For NAME.LIST Tag
                                if (agmList.ContainsKey("NAME.LIST".ToUpper()))
                                {
                                    GenerateMasterXMLBuilder.AppendLine("<NAME.LIST>");
                                    #region For Name Tag with in the NAME.LIST
                                    if (agmList.ContainsKey("NAME".ToUpper()))
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "NAME");
                                        if (fieldVal != "")
                                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                        GenerateMasterXMLBuilder.AppendLine("<NAME>" + fieldVal.Trim() + "</NAME>");
                                    }
                                    #endregion
                                    GenerateMasterXMLBuilder.AppendLine("</NAME.LIST>");
                                }
                                #endregion
                                #region For LANGUAGEID Tag
                                if (agmList.ContainsKey("LANGUAGEID".ToUpper()))
                                {
                                    fieldVal = this.GetFieldValue(agmList, "LANGUAGEID");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<LANGUAGEID>" + fieldVal.Trim() + "</LANGUAGEID>");
                                }
                                #endregion
                                GenerateMasterXMLBuilder.AppendLine("</LANGUAGENAME.LIST>");
                            }
                            #endregion

                            #region For LEDGERAUDITCLASS.LIST Tag
                            if (agmList.ContainsKey("LEDGERAUDITCLASS.LIST".ToUpper()))
                            {
                                GenerateMasterXMLBuilder.AppendLine("<LEDGERAUDITCLASS.LIST>");

                                #region For LEDAUDITPERIOD.LIST  Tag
                                if (agmList.ContainsKey("LEDAUDITPERIOD.LIST ".ToUpper()))
                                {
                                    fieldVal = this.GetFieldValue(agmList, "LEDAUDITPERIOD.LIST ");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<LEDAUDITPERIOD.LIST >" + fieldVal.Trim() + "</LEDAUDITPERIOD.LIST >");
                                }
                                #endregion

                                GenerateMasterXMLBuilder.AppendLine("</LEDGERAUDITCLASS.LIST>");
                            }
                            #endregion

                            #region For Rest Of Tags
                            if (agmList.Count > 45)
                            {
                                foreach (string key in agmList.Keys)
                                {
                                    if (key.ToUpper() != "LEDGER".ToUpper() && key.ToUpper() != "NAME.LIST".ToUpper() && key.ToUpper() != "NAME".ToUpper() && key.ToUpper() != "PARENT".ToUpper() && key.ToUpper() != "ISBILLWISEON".ToUpper() && key.ToUpper() != "MAILINGNAME.LIST".ToUpper() && key.ToUpper() != "ISCOSTCENTRESON".ToUpper() && key.ToUpper() != "MAILINGNAME".ToUpper()
                                        && key.ToUpper() != "CURRENCYNAME".ToUpper() && key.ToUpper() != "TAXCLASSIFICATIONNAME".ToUpper() && key.ToUpper() != "TAXTYPE".ToUpper() && key.ToUpper() != "GSTTYPE ".ToUpper() && key.ToUpper() != "SERVICECATEGORY ".ToUpper() && key.ToUpper() != "EXCISEDUTYTYPE".ToUpper() && key.ToUpper() != "TRADERLEDNATUREOFPURCHASE".ToUpper()
                                        && key.ToUpper() != "TDSDEDUCTEETYPE".ToUpper() && key.ToUpper() != "TDSRATENAME".ToUpper() && key.ToUpper() != "LEDGERFBTCATEGORY".ToUpper() && key.ToUpper() != "ISCOSTCENTRESON".ToUpper() && key.ToUpper() != "ISINTERESTON".ToUpper() && key.ToUpper() != "ALLOWINMOBILE".ToUpper() && key.ToUpper() != "ISCONDENSED".ToUpper() && key.ToUpper() != "AFFECTSSTOCK".ToUpper()
                                        && key.ToUpper() != "FORPAYROLL".ToUpper() && key.ToUpper() != "INTERESTONBILLWISE".ToUpper() && key.ToUpper() != "OVERRIDEINTEREST".ToUpper() && key.ToUpper() != "OVERRIDEADVINTEREST".ToUpper() && key.ToUpper() != "USEFORVAT".ToUpper() && key.ToUpper() != "IGNORETDSEXEMPT".ToUpper() && key.ToUpper() != "ISTCSAPPLICABLE".ToUpper() && key.ToUpper() != "ISTDSAPPLICABLE".ToUpper()
                                        && key.ToUpper() != "ISFBTAPPLICABLE".ToUpper() && key.ToUpper() != "ISGSTAPPLICABLE".ToUpper() && key.ToUpper() != "SHOWINPAYSLIP".ToUpper() && key.ToUpper() != "USEFORGRATUITY".ToUpper() && key.ToUpper() != "FORSERVICETAX".ToUpper() && key.ToUpper() != "ISINPUTCREDIT".ToUpper() && key.ToUpper() != "ISEXEMPTED".ToUpper() && key.ToUpper() != "ISABATEMENTAPPLICABLE".ToUpper()
                                        && key.ToUpper() != "TDSDEDUCTEEISSPECIALRATE".ToUpper() && key.ToUpper() != "AUDITED".ToUpper() && key.ToUpper() != "SORTPOSITION".ToUpper() && key.ToUpper() != "LANGUAGENAME.LIST".ToUpper() && key.ToUpper() != "LANGUAGEID".ToUpper() && key.ToUpper() != "LEDGERAUDITCLASS.LIST".ToUpper() && key.ToUpper() != "LEDAUDITPERIOD.LIST".ToUpper())
                                    {
                                        fieldVal = this.GetFieldValue(agmList, key.ToUpper());
                                        if (fieldVal != "")
                                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                        GenerateMasterXMLBuilder.AppendLine("<" + key.ToUpper() + ">" + fieldVal.Trim() + "</" + key.ToUpper() + ">");
                                    }
                                }
                            }
                            #endregion

                            #region GRUPING END TAGS
                            if (agmList.ContainsKey("LEDGER".ToUpper()))
                            {
                                GenerateMasterXMLBuilder.AppendLine("</LEDGER>");
                            }
                            GenerateMasterXMLBuilder.AppendLine("</TALLYMESSAGE>");
                            #endregion

                            GenerateMasterXMLBuilder.AppendLine("</TALLYMESSAGE>");
                        }
                        #endregion
                    }
                    else if (ApplicationValues.GenaretingMasterName.ToUpper() == "Item Master".ToUpper())
                    {
                        #region In Settings Page All Values Are Have Default Values for Item Master
                        foreach (DataRow row in DSMaster.Tables[0].Rows)
                        {
                            GenerateMasterXMLBuilder.AppendLine("<TALLYMESSAGE xmlns:UDF='TallyUDF'>");
                            string fieldVal = string.Empty;
                            #region For STOCKITEM Tag
                            if (agmList.ContainsKey("STOCKITEM".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "STOCKITEM");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<STOCKITEM Name='" + fieldVal.Trim() + "' RESERVEDNAME=''>");

                            }
                            #endregion

                            #region For ADDITIONALNAME.LIST Tag
                            if (agmList.ContainsKey("ADDITIONALNAME.LIST".ToUpper()))
                            {
                                GenerateMasterXMLBuilder.AppendLine("<ADDITIONALNAME.LIST>");
                                #region For MAILINGNAME Tag with in the ADDITIONALNAME
                                if (agmList.ContainsKey("ADDITIONALNAME".ToUpper()))
                                {
                                    fieldVal = this.GetFieldValue(agmList, "ADDITIONALNAME");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<ADDITIONALNAME>" + fieldVal.Trim() + "</ADDITIONALNAME>");
                                }
                                #endregion
                                GenerateMasterXMLBuilder.AppendLine("</ADDITIONALNAME.LIST>");
                            }
                            #endregion

                            #region for PARENT Tag
                            if (agmList.ContainsKey("PARENT".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "PARENT");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<PARENT>" + fieldVal.Trim() + "</PARENT>");
                            }
                            #endregion

                            #region For CATEGORY Tag
                            if (agmList.ContainsKey("CATEGORY".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "CATEGORY");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<CATEGORY>" + fieldVal.Trim() + "</CATEGORY>");
                            }
                            #endregion

                            #region For TAXCLASSIFICATIONNAME Tag
                            if (agmList.ContainsKey("TAXCLASSIFICATIONNAME".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "TAXCLASSIFICATIONNAME");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<TAXCLASSIFICATIONNAME>" + fieldVal.Trim() + "</TAXCLASSIFICATIONNAME>");
                            }
                            #endregion

                            #region For BASEUNITS Tag
                            if (agmList.ContainsKey("BASEUNITS".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "BASEUNITS");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<BASEUNITS>" + fieldVal.Trim() + "</BASEUNITS>");
                            }
                            #endregion

                            #region For ADDITIONALUNITS Tag
                            if (agmList.ContainsKey("ADDITIONALUNITS".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "ADDITIONALUNITS");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<ADDITIONALUNITS>" + fieldVal.Trim() + "</ADDITIONALUNITS>");
                            }
                            #endregion

                            #region For ISCOSTCENTRESON Tag
                            if (agmList.ContainsKey("ISCOSTCENTRESON".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "ISCOSTCENTRESON");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<ISCOSTCENTRESON>" + fieldVal.Trim() + "</ISCOSTCENTRESON>");
                            }
                            #endregion

                            #region For ISBATCHWISEON Tag
                            if (agmList.ContainsKey("ISBATCHWISEON".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "ISBATCHWISEON");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<ISBATCHWISEON>" + fieldVal.Trim() + "</ISBATCHWISEON>");
                            }
                            #endregion

                            #region For ISPERISHABLEON Tag
                            if (agmList.ContainsKey("ISPERISHABLEON".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "ISPERISHABLEON");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<ISPERISHABLEON>" + fieldVal.Trim() + "</ISPERISHABLEON>");
                            }
                            #endregion

                            #region For IGNOREPHYSICALDIFFERENCE Tag
                            if (agmList.ContainsKey("IGNOREPHYSICALDIFFERENCE".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "IGNOREPHYSICALDIFFERENCE");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<IGNOREPHYSICALDIFFERENCE>" + fieldVal.Trim() + "</IGNOREPHYSICALDIFFERENCE>");
                            }
                            #endregion

                            #region for IGNORENEGATIVESTOCK Tag
                            if (agmList.ContainsKey("IGNORENEGATIVESTOCK".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "IGNORENEGATIVESTOCK");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<IGNORENEGATIVESTOCK>" + fieldVal.Trim() + "</IGNORENEGATIVESTOCK>");
                            }
                            #endregion

                            #region For TREATSALESASMANUFACTURED Tag
                            if (agmList.ContainsKey("TREATSALESASMANUFACTURED".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "TREATSALESASMANUFACTURED");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<TREATSALESASMANUFACTURED>" + fieldVal.Trim() + "</TREATSALESASMANUFACTURED>");
                            }
                            #endregion

                            #region For TREATPURCHASESASCONSUMED Tag
                            if (agmList.ContainsKey("TREATPURCHASESASCONSUMED".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "TREATPURCHASESASCONSUMED");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<TREATPURCHASESASCONSUMED>" + fieldVal.Trim() + "</TREATPURCHASESASCONSUMED>");
                            }
                            #endregion

                            #region For TREATREJECTSASSCRAP Tag
                            if (agmList.ContainsKey("TREATREJECTSASSCRAP".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "TREATREJECTSASSCRAP");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<TREATREJECTSASSCRAP>" + fieldVal.Trim() + "</TREATREJECTSASSCRAP>");
                            }
                            #endregion

                            #region For HASMFGDATE Tag
                            if (agmList.ContainsKey("HASMFGDATE".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "HASMFGDATE");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<HASMFGDATE>" + fieldVal.Trim() + "</HASMFGDATE>");
                            }
                            #endregion

                            #region For ALLOWUSEOFEXPIREDITEMS Tag
                            if (agmList.ContainsKey("ALLOWUSEOFEXPIREDITEMS".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "ALLOWUSEOFEXPIREDITEMS");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<ALLOWUSEOFEXPIREDITEMS>" + fieldVal.Trim() + "</ALLOWUSEOFEXPIREDITEMS>");
                            }
                            #endregion

                            #region For IGNOREBATCHES Tag
                            if (agmList.ContainsKey("IGNOREBATCHES".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "IGNOREBATCHES");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<IGNOREBATCHES>" + fieldVal.Trim() + "</IGNOREBATCHES>");
                            }
                            #endregion

                            #region For IGNOREGODOWNS Tag
                            if (agmList.ContainsKey("IGNOREGODOWNS".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "IGNOREGODOWNS");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<IGNOREGODOWNS>" + fieldVal.Trim() + "</IGNOREGODOWNS>");
                            }
                            #endregion

                            #region For ISMRPINCLOFTAX Tag
                            if (agmList.ContainsKey("ISMRPINCLOFTAX".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "ISMRPINCLOFTAX");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<ISMRPINCLOFTAX>" + fieldVal.Trim() + "</ISMRPINCLOFTAX>");
                            }
                            #endregion

                            #region For REORDERASHIGHER Tag
                            if (agmList.ContainsKey("REORDERASHIGHER".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "REORDERASHIGHER");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<REORDERASHIGHER>" + fieldVal.Trim() + "</REORDERASHIGHER>");
                            }
                            #endregion

                            #region For MINORDERASHIGHER Tag
                            if (agmList.ContainsKey("MINORDERASHIGHER".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "MINORDERASHIGHER");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<MINORDERASHIGHER>" + fieldVal.Trim() + "</MINORDERASHIGHER>");
                            }
                            #endregion

                            #region For DENOMINATOR Tag
                            if (agmList.ContainsKey("DENOMINATOR".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "DENOMINATOR");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<DENOMINATOR>" + fieldVal.Trim() + "</DENOMINATOR>");
                            }
                            #endregion

                            #region For RATEOFVAT Tag
                            if (agmList.ContainsKey("RATEOFVAT".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "RATEOFVAT");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<RATEOFVAT>" + fieldVal.Trim() + "</RATEOFVAT>");
                            }
                            #endregion

                            #region For LANGUAGENAME.LIST Tag
                            if (agmList.ContainsKey("LANGUAGENAME.LIST".ToUpper()))
                            {
                                GenerateMasterXMLBuilder.AppendLine("<LANGUAGENAME.LIST>");
                                #region For NAME.LIST Tag
                                if (agmList.ContainsKey("NAME.LIST".ToUpper()))
                                {
                                    GenerateMasterXMLBuilder.AppendLine("<NAME.LIST>");
                                    #region For Name Tag with in the NAME.LIST
                                    if (agmList.ContainsKey("NAME".ToUpper()))
                                    {

                                        fieldVal = this.GetFieldValue(agmList, "NAME");
                                        if (fieldVal != "")
                                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                        GenerateMasterXMLBuilder.AppendLine("<NAME>" + fieldVal.Trim() + "</NAME>");
                                    }
                                    #endregion
                                    GenerateMasterXMLBuilder.AppendLine("</NAME.LIST>");
                                }
                                #endregion
                                #region For LANGUAGEID Tag
                                if (agmList.ContainsKey("LANGUAGEID".ToUpper()))
                                {
                                    fieldVal = this.GetFieldValue(agmList, "LANGUAGEID");
                                    if (fieldVal != "")
                                        fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                    GenerateMasterXMLBuilder.AppendLine("<LANGUAGEID>" + fieldVal.Trim() + "</LANGUAGEID>");
                                }
                                #endregion
                                GenerateMasterXMLBuilder.AppendLine("</LANGUAGENAME.LIST>");
                            }
                            #endregion

                            #region For ADDITIONALLEDGERS.LIST Tag
                            if (agmList.ContainsKey("ADDITIONALLEDGERS.LIST".ToUpper()))
                            {

                                fieldVal = this.GetFieldValue(agmList, "ADDITIONALLEDGERS.LIST");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<ADDITIONALLEDGERS.LIST>" + fieldVal.Trim() + "</ADDITIONALLEDGERS.LIST>");
                            }
                            #endregion

                            #region For Rest Of Tags
                            if (agmList.Count > 29)
                            {
                                foreach (string key in agmList.Keys)
                                {
                                    if (key.ToUpper() != "STOCKITEM".ToUpper() && key.ToUpper() != "ADDITIONALNAME.LIST".ToUpper() && key.ToUpper() != "ADDITIONALNAME".ToUpper() && key.ToUpper() != "PARENT".ToUpper() && key.ToUpper() != "CATEGORY".ToUpper() && key.ToUpper() != "TAXCLASSIFICATIONNAME".ToUpper() && key.ToUpper() != "BASEUNITS".ToUpper() && key.ToUpper() != "ADDITIONALUNITS".ToUpper()
                                        && key.ToUpper() != "ISCOSTCENTRESON".ToUpper() && key.ToUpper() != "ISBATCHWISEON".ToUpper() && key.ToUpper() != "ISPERISHABLEON".ToUpper() && key.ToUpper() != "IGNOREPHYSICALDIFFERENCE ".ToUpper() && key.ToUpper() != "IGNORENEGATIVESTOCK ".ToUpper() && key.ToUpper() != "TREATSALESASMANUFACTURED".ToUpper() && key.ToUpper() != "TREATPURCHASESASCONSUMED".ToUpper()
                                        && key.ToUpper() != "TREATREJECTSASSCRAP".ToUpper() && key.ToUpper() != "HASMFGDATE".ToUpper() && key.ToUpper() != "ALLOWUSEOFEXPIREDITEMS".ToUpper() && key.ToUpper() != "IGNOREBATCHES".ToUpper() && key.ToUpper() != "IGNOREGODOWNS".ToUpper() && key.ToUpper() != "ISMRPINCLOFTAX".ToUpper() && key.ToUpper() != "REORDERASHIGHER".ToUpper() && key.ToUpper() != "MINORDERASHIGHER".ToUpper()
                                        && key.ToUpper() != "DENOMINATOR".ToUpper() && key.ToUpper() != "RATEOFVAT".ToUpper() && key.ToUpper() != "LANGUAGENAME.LIST".ToUpper() && key.ToUpper() != "NAME.LIST".ToUpper() && key.ToUpper() != "NAME".ToUpper() && key.ToUpper() != "LANGUAGEID".ToUpper() && key.ToUpper() != "ADDITIONALLEDGERS.LIST".ToUpper())
                                    {
                                        fieldVal = this.GetFieldValue(agmList, key.ToUpper());
                                        if (fieldVal != "")
                                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                        GenerateMasterXMLBuilder.AppendLine("<" + key.ToUpper() + ">" + fieldVal.Trim() + "</" + key.ToUpper() + ">");
                                    }
                                }
                            }
                            #endregion

                            #region STOCKITEM END TAGS
                            if (agmList.ContainsKey("STOCKITEM".ToUpper()))
                            {
                                GenerateMasterXMLBuilder.AppendLine("</STOCKITEM>");
                            }
                            #endregion

                            GenerateMasterXMLBuilder.AppendLine("</TALLYMESSAGE>");
                        }//
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                dbError = ex.Message;
            }
            return GenerateMasterXMLBuilder;
        }


        /// <summary>
        /// To Get the Selected Company Account Item Master Table Data for Tally Version 9.0 as a String Builder
        /// </summary>
        /// <param name="_iGenerateMasterXML"></param>
        /// <returns>StringBuilder</returns>
        public StringBuilder GenerateItemGroupMasterTV90()
        {
            Dictionary<string, string> agmList = new Dictionary<string, string>();
            string tablename = string.Empty;
            string fieldValue = string.Empty;
            string strTblDtls = string.Empty;
            string[] splitstring = null;
            string[] Fieldvalues = null;
            string[] defaultvalues = null;
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
                    }

                    #endregion
                }
                DSMaster = new DataSet();
                DSGroupMaster = new DataSet();
                objGenerateMasterXML.ConnectionString = ApplicationValues.CompanyDBConnectionString;
                objGenerateMasterXML.CompanyDBName = ApplicationValues.CompanyDBName;
                objGenerateMasterXML.GeneratingOptions = this.GeneratingOptions;

                if (fieldCount != 0)
                {
                    #region At least One Field Value is there in settins File
                    _isNeedToUpdate = true;

                    #region Geting the Data Base Table Name
                    agmList.TryGetValue("STOCKGROUP".ToUpper(), out strTblDtls);
                    splitstring = strTblDtls.Split(new char[] { '.' });
                    if (splitstring.Length > 0)
                    {
                        foreach (string strvalue in splitstring)
                        {
                            if (strvalue.Contains("U2T"))
                            {
                                tablename = strvalue;
                                int num1 = tablename.IndexOf('_', 0);
                                tablename = tablename.Substring(num1 + 1, (tablename.Length - (num1 + 1)));
                                if (tablename != string.Empty)
                                {
                                    ApplicationValues.tableName = tablename;
                                    break;
                                }

                                else
                                {
                                    ApplicationValues.tableName = "ITEM_GROUP";
                                    break;
                                }
                            }
                            else
                            {
                                fieldValue = strvalue;
                                ApplicationValues.tableName = "ITEM_GROUP";
                                break;
                            }
                        }
                    }
                    else
                    {
                        tablename = "ITEM_GROUP";
                        ApplicationValues.tableName = tablename;
                    }
                    #endregion
                    objGenerateMasterXML.MasterTableName = tablename;
                    objGenerateMasterXML.TallyTagsTable = this.TallyTagsTable;
                    DSGroupMaster = objGenerateMasterXML.GenerateAccountGroupMasterTV90(this);
                    if (DSGroupMaster.Tables.Count > 0)
                    {
                        dTable = new DataTable();
                        foreach (DataColumn dCloumn in DSGroupMaster.Tables[0].Columns)
                        {
                            DataColumn dc1 = new DataColumn();
                            dc1.ColumnName = dCloumn.ColumnName;
                            dTable.Columns.Add(dc1);
                        }
                        getItemGroupMasterChild(DSGroupMaster, string.Empty);
                        string rowValue = string.Empty;
                        GenerateMasterXMLBuilder = new StringBuilder();
                        if (dTable != null)
                        {
                            foreach (DataRow row in dTable.Rows)
                            {
                                string fieldVal = string.Empty;
                                GenerateMasterXMLBuilder.AppendLine("<TALLYMESSAGE xmlns:UDF='TallyUDF'>");

                                #region For STOCKGROUP Tag
                                if (agmList.ContainsKey("STOCKGROUP".ToUpper()))
                                {
                                    if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "STOCKGROUP") == "Field")
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "STOCKGROUP");
                                        if (fieldVal != "")
                                        {
                                            if (row[fieldVal] != null)
                                                rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                        }
                                        GenerateMasterXMLBuilder.AppendLine("<STOCKGROUP Name='" + rowValue.Trim() + "' RESERVEDNAME='Create'>");
                                    }
                                    else
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "STOCKGROUP");
                                        if (fieldVal != "")
                                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                        GenerateMasterXMLBuilder.AppendLine("<STOCKGROUP Name='" + fieldVal.Trim() + "' RESERVEDNAME='Create'>");
                                    }
                                }
                                #endregion

                                #region For PARENT Tag
                                if (agmList.ContainsKey("PARENT".ToUpper()))
                                {
                                    if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "PARENT") == "Field")
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "PARENT");
                                        if (fieldVal != "")
                                        {
                                            if (row[fieldVal] != null)
                                                rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                        }

                                        GenerateMasterXMLBuilder.AppendLine("<PARENT>" + rowValue.Trim() + "</PARENT>");
                                    }
                                    else
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "PARENT");
                                        if (fieldVal != "")
                                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                        GenerateMasterXMLBuilder.AppendLine("<PARENT>" + fieldVal.Trim() + "</PARENT>");
                                    }
                                }
                                #endregion

                                #region For BASEUNITS Tag
                                if (agmList.ContainsKey("BASEUNITS".ToUpper()))
                                {
                                    if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "BASEUNITS") == "Field")
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "BASEUNITS");
                                        if (fieldVal != "")
                                        {
                                            if (row[fieldVal] != null)
                                                rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                        }
                                        GenerateMasterXMLBuilder.AppendLine("<BASEUNITS>" + rowValue.Trim() + "</BASEUNITS>");
                                    }
                                    else
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "BASEUNITS");
                                        if (fieldVal != "")
                                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                        GenerateMasterXMLBuilder.AppendLine("<BASEUNITS>" + fieldVal.Trim() + "</BASEUNITS>");
                                    }
                                }
                                #endregion

                                #region For ADDITIONALUNITS Tag
                                if (agmList.ContainsKey("ADDITIONALUNITS".ToUpper()))
                                {
                                    if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "ADDITIONALUNITS") == "Field")
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "ADDITIONALUNITS");
                                        if (fieldVal != "")
                                        {
                                            if (row[fieldVal] != null)
                                                rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                        }
                                        GenerateMasterXMLBuilder.AppendLine("<ADDITIONALUNITS>" + rowValue.Trim() + "</ADDITIONALUNITS>");
                                    }
                                    else
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "ADDITIONALUNITS");
                                        if (fieldVal != "")
                                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                        GenerateMasterXMLBuilder.AppendLine("<ADDITIONALUNITS>" + fieldVal.Trim() + "</ADDITIONALUNITS>");
                                    }
                                }
                                #endregion

                                #region For ISBATCHWISEON Tag
                                if (agmList.ContainsKey("ISBATCHWISEON".ToUpper()))
                                {
                                    if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "ISBATCHWISEON") == "Field")
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "ISBATCHWISEON");
                                        if (fieldVal != "")
                                        {
                                            if (row[fieldVal] != null)
                                                rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                        }
                                        GenerateMasterXMLBuilder.AppendLine("<ISBATCHWISEON>" + rowValue.Trim() + "</ISBATCHWISEON>");
                                    }
                                    else
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "ISBATCHWISEON");
                                        if (fieldVal != "")
                                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                        GenerateMasterXMLBuilder.AppendLine("<ISBATCHWISEON>" + fieldVal.Trim() + "</ISBATCHWISEON>");
                                    }
                                }
                                #endregion

                                #region For ISPERISHABLEON Tag
                                if (agmList.ContainsKey("ISPERISHABLEON".ToUpper()))
                                {
                                    if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "ISPERISHABLEON") == "Field")
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "ISPERISHABLEON");
                                        if (fieldVal != "")
                                        {
                                            if (row[fieldVal] != null)
                                                rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                        }
                                        GenerateMasterXMLBuilder.AppendLine("<ISPERISHABLEON>" + rowValue.Trim() + "</ISPERISHABLEON>");
                                    }
                                    else
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "ISPERISHABLEON");
                                        if (fieldVal != "")
                                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                        GenerateMasterXMLBuilder.AppendLine("<ISPERISHABLEON>" + fieldVal.Trim() + "</ISPERISHABLEON>");
                                    }
                                }
                                #endregion

                                #region For ISADDABLE Tag
                                if (agmList.ContainsKey("ISADDABLE".ToUpper()))
                                {
                                    if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "ISADDABLE") == "Field")
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "ISADDABLE");
                                        if (fieldVal != "")
                                        {
                                            if (row[fieldVal] != null)
                                                rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                        }
                                        GenerateMasterXMLBuilder.AppendLine("<ISADDABLE>" + rowValue.Trim() + "</ISADDABLE>");
                                    }
                                    else
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "ISADDABLE");
                                        if (fieldVal != "")
                                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                        GenerateMasterXMLBuilder.AppendLine("<ISADDABLE>" + fieldVal.Trim() + "</ISADDABLE>");
                                    }
                                }
                                #endregion

                                #region For IGNOREPHYSICALDIFFERENCE Tag
                                if (agmList.ContainsKey("IGNOREPHYSICALDIFFERENCE".ToUpper()))
                                {
                                    if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "IGNOREPHYSICALDIFFERENCE") == "Field")
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "IGNOREPHYSICALDIFFERENCE");
                                        if (fieldVal != "")
                                        {
                                            if (row[fieldVal] != null)
                                                rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                        }
                                        GenerateMasterXMLBuilder.AppendLine("<IGNOREPHYSICALDIFFERENCE>" + rowValue.Trim() + "</IGNOREPHYSICALDIFFERENCE>");
                                    }
                                    else
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "IGNOREPHYSICALDIFFERENCE");
                                        if (fieldVal != "")
                                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                        GenerateMasterXMLBuilder.AppendLine("<IGNOREPHYSICALDIFFERENCE>" + fieldVal.Trim() + "</IGNOREPHYSICALDIFFERENCE>");
                                    }
                                }
                                #endregion

                                #region for IGNORENEGATIVESTOCK Tag
                                if (agmList.ContainsKey("IGNORENEGATIVESTOCK".ToUpper()))
                                {
                                    if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "IGNORENEGATIVESTOCK") == "Field")
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "IGNORENEGATIVESTOCK");
                                        if (fieldVal != "")
                                        {
                                            if (row[fieldVal] != null)
                                                rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                        }

                                        GenerateMasterXMLBuilder.AppendLine("<IGNORENEGATIVESTOCK>" + rowValue.Trim() + "</IGNORENEGATIVESTOCK>");
                                    }
                                    else
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "IGNORENEGATIVESTOCK");
                                        if (fieldVal != "")
                                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                        GenerateMasterXMLBuilder.AppendLine("<IGNORENEGATIVESTOCK>" + fieldVal.Trim() + "</IGNORENEGATIVESTOCK>");
                                    }
                                }
                                #endregion

                                #region For TREATSALESASMANUFACTURED Tag
                                if (agmList.ContainsKey("TREATSALESASMANUFACTURED".ToUpper()))
                                {
                                    if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "TREATSALESASMANUFACTURED") == "Field")
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "TREATSALESASMANUFACTURED");
                                        if (fieldVal != "")
                                        {
                                            if (row[fieldVal] != null)
                                                rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                        }
                                        GenerateMasterXMLBuilder.AppendLine("<TREATSALESASMANUFACTURED>" + rowValue.Trim() + "</TREATSALESASMANUFACTURED>");
                                    }
                                    else
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "TREATSALESASMANUFACTURED");
                                        if (fieldVal != "")
                                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                        GenerateMasterXMLBuilder.AppendLine("<TREATSALESASMANUFACTURED>" + fieldVal.Trim() + "</TREATSALESASMANUFACTURED>");
                                    }
                                }
                                #endregion

                                #region For TREATPURCHASESASCONSUMED Tag
                                if (agmList.ContainsKey("TREATPURCHASESASCONSUMED".ToUpper()))
                                {
                                    if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "TREATPURCHASESASCONSUMED") == "Field")
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "TREATPURCHASESASCONSUMED");
                                        if (fieldVal != "")
                                        {
                                            if (row[fieldVal] != null)
                                                rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                        }
                                        GenerateMasterXMLBuilder.AppendLine("<TREATPURCHASESASCONSUMED>" + rowValue.Trim() + "</TREATPURCHASESASCONSUMED>");
                                    }
                                    else
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "TREATPURCHASESASCONSUMED");
                                        if (fieldVal != "")
                                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                        GenerateMasterXMLBuilder.AppendLine("<TREATPURCHASESASCONSUMED>" + fieldVal.Trim() + "</TREATPURCHASESASCONSUMED>");
                                    }
                                }
                                #endregion

                                #region For TREATREJECTSASSCRAP Tag
                                if (agmList.ContainsKey("TREATREJECTSASSCRAP".ToUpper()))
                                {
                                    if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "TREATREJECTSASSCRAP") == "Field")
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "TREATREJECTSASSCRAP");
                                        if (fieldVal != "")
                                        {
                                            if (row[fieldVal] != null)
                                                rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                        }
                                        GenerateMasterXMLBuilder.AppendLine("<TREATREJECTSASSCRAP>" + rowValue.Trim() + "</TREATREJECTSASSCRAP>");
                                    }
                                    else
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "TREATREJECTSASSCRAP");
                                        if (fieldVal != "")
                                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                        GenerateMasterXMLBuilder.AppendLine("<TREATREJECTSASSCRAP>" + fieldVal.Trim() + "</TREATREJECTSASSCRAP>");
                                    }
                                }
                                #endregion

                                #region For HASMFGDATE Tag
                                if (agmList.ContainsKey("HASMFGDATE".ToUpper()))
                                {
                                    if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "HASMFGDATE") == "Field")
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "HASMFGDATE");
                                        if (fieldVal != "")
                                        {
                                            if (row[fieldVal] != null)
                                                rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                        }
                                        GenerateMasterXMLBuilder.AppendLine("<HASMFGDATE>" + rowValue.Trim() + "</HASMFGDATE>");
                                    }
                                    else
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "HASMFGDATE");
                                        if (fieldVal != "")
                                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                        GenerateMasterXMLBuilder.AppendLine("<HASMFGDATE>" + fieldVal.Trim() + "</HASMFGDATE>");
                                    }
                                }
                                #endregion

                                #region For ALLOWUSEOFEXPIREDITEMS Tag
                                if (agmList.ContainsKey("ALLOWUSEOFEXPIREDITEMS".ToUpper()))
                                {
                                    if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "ALLOWUSEOFEXPIREDITEMS") == "Field")
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "ALLOWUSEOFEXPIREDITEMS");
                                        if (fieldVal != "")
                                        {
                                            if (row[fieldVal] != null)
                                                rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                        }
                                        GenerateMasterXMLBuilder.AppendLine("<ALLOWUSEOFEXPIREDITEMS>" + rowValue.Trim() + "</ALLOWUSEOFEXPIREDITEMS>");
                                    }
                                    else
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "ALLOWUSEOFEXPIREDITEMS");
                                        if (fieldVal != "")
                                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                        GenerateMasterXMLBuilder.AppendLine("<ALLOWUSEOFEXPIREDITEMS>" + fieldVal.Trim() + "</ALLOWUSEOFEXPIREDITEMS>");
                                    }
                                }
                                #endregion

                                #region For IGNOREBATCHES Tag
                                if (agmList.ContainsKey("IGNOREBATCHES".ToUpper()))
                                {
                                    if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "IGNOREBATCHES") == "Field")
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "IGNOREBATCHES");
                                        if (fieldVal != "")
                                        {
                                            if (row[fieldVal] != null)
                                                rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                        }
                                        GenerateMasterXMLBuilder.AppendLine("<IGNOREBATCHES>" + rowValue.Trim() + "</IGNOREBATCHES>");
                                    }
                                    else
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "IGNOREBATCHES");
                                        if (fieldVal != "")
                                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                        GenerateMasterXMLBuilder.AppendLine("<IGNOREBATCHES>" + fieldVal.Trim() + "</IGNOREBATCHES>");
                                    }
                                }
                                #endregion

                                #region For IGNOREGODOWNS Tag
                                if (agmList.ContainsKey("IGNOREGODOWNS".ToUpper()))
                                {
                                    if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "IGNOREGODOWNS") == "Field")
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "IGNOREGODOWNS");
                                        if (fieldVal != "")
                                        {
                                            if (row[fieldVal] != null)
                                                rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                        }
                                        GenerateMasterXMLBuilder.AppendLine("<IGNOREGODOWNS>" + rowValue.Trim() + "</IGNOREGODOWNS>");
                                    }
                                    else
                                    {
                                        fieldVal = this.GetFieldValue(agmList, "IGNOREGODOWNS");
                                        if (fieldVal != "")
                                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                        GenerateMasterXMLBuilder.AppendLine("<IGNOREGODOWNS>" + fieldVal.Trim() + "</IGNOREGODOWNS>");
                                    }
                                }
                                #endregion

                                #region For LANGUAGENAME.LIST Tag
                                if (agmList.ContainsKey("LANGUAGENAME.LIST".ToUpper()))
                                {
                                    GenerateMasterXMLBuilder.AppendLine("<LANGUAGENAME.LIST>");
                                    #region For NAME.LIST Tag
                                    if (agmList.ContainsKey("NAME.LIST".ToUpper()))
                                    {
                                        GenerateMasterXMLBuilder.AppendLine("<NAME.LIST>");
                                        #region For Name Tag with in the NAME.LIST
                                        if (agmList.ContainsKey("NAME".ToUpper()))
                                        {
                                            if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "NAME") == "Field")
                                            {
                                                fieldVal = this.GetFieldValue(agmList, "NAME");
                                                if (fieldVal != "")
                                                {
                                                    if (row[fieldVal] != null)
                                                        rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                                }
                                                GenerateMasterXMLBuilder.AppendLine("<NAME>" + rowValue.Trim() + "</NAME>");
                                            }
                                            else
                                            {
                                                fieldVal = this.GetFieldValue(agmList, "NAME");
                                                if (fieldVal != "")
                                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                                GenerateMasterXMLBuilder.AppendLine("<NAME>" + fieldVal.Trim() + "</NAME>");
                                            }
                                        }
                                        #endregion
                                        GenerateMasterXMLBuilder.AppendLine("</NAME.LIST>");
                                    }
                                    #endregion
                                    #region For LANGUAGEID Tag
                                    if (agmList.ContainsKey("LANGUAGEID".ToUpper()))
                                    {
                                        if (VerifyDefaultOrField(defaultvalues, Fieldvalues, "LANGUAGEID") == "Field")
                                        {
                                            fieldVal = this.GetFieldValue(agmList, "LANGUAGEID");
                                            if (fieldVal != "")
                                            {
                                                if (row[fieldVal] != null)
                                                    rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                            }
                                            GenerateMasterXMLBuilder.AppendLine("<LANGUAGEID>" + rowValue.Trim() + "</LANGUAGEID>");
                                        }
                                        else
                                        {
                                            fieldVal = this.GetFieldValue(agmList, "LANGUAGEID");
                                            if (fieldVal != "")
                                                fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                            GenerateMasterXMLBuilder.AppendLine("<LANGUAGEID>" + fieldVal.Trim() + "</LANGUAGEID>");
                                        }
                                    }
                                    #endregion
                                    GenerateMasterXMLBuilder.AppendLine("</LANGUAGENAME.LIST>");
                                }
                                #endregion

                                #region For Rest Of Tags
                                if (agmList.Count > 20)
                                {
                                    foreach (string key in agmList.Keys)
                                    {
                                        if (key.ToUpper() != "STOCKGROUP".ToUpper() && key.ToUpper() != "PARENT".ToUpper() && key.ToUpper() != "BASEUNITS".ToUpper() && key.ToUpper() != "ADDITIONALUNITS".ToUpper() && key.ToUpper() != "ISBATCHWISEON".ToUpper() && key.ToUpper() != "ISPERISHABLEON".ToUpper() && key.ToUpper() != "ISADDABLE".ToUpper()
                                            && key.ToUpper() != "IGNOREPHYSICALDIFFERENCE".ToUpper() && key.ToUpper() != "IGNORENEGATIVESTOCK".ToUpper() && key.ToUpper() != "TREATSALESASMANUFACTURED".ToUpper() && key.ToUpper() != "TREATPURCHASESASCONSUMED".ToUpper() && key.ToUpper() != "TREATREJECTSASSCRAP".ToUpper() && key.ToUpper() != "HASMFGDATE".ToUpper()
                                            && key.ToUpper() != "ALLOWUSEOFEXPIREDITEMS".ToUpper() && key.ToUpper() != "IGNOREBATCHES".ToUpper() && key.ToUpper() != "IGNOREGODOWNS".ToUpper() && key.ToUpper() != "LANGUAGENAME.LIST".ToUpper() && key.ToUpper() != "NAME.LIST".ToUpper() && key.ToUpper() != "NAME".ToUpper() && key.ToUpper() != "LANGUAGEID".ToUpper())
                                        {
                                            if (VerifyDefaultOrField(defaultvalues, Fieldvalues, key.ToUpper()) == "Field")
                                            {
                                                fieldVal = this.GetFieldValue(agmList, key.ToUpper());
                                                if (fieldVal != "")
                                                {
                                                    if (row[fieldVal] != null)
                                                        rowValue = CFunctions.convertSpecialChars(row[fieldVal].ToString());
                                                }
                                                GenerateMasterXMLBuilder.AppendLine("<" + key.ToUpper() + ">" + rowValue.Trim() + "</" + key.ToUpper() + ">");
                                            }
                                            else
                                            {
                                                fieldVal = this.GetFieldValue(agmList, key.ToUpper());
                                                if (fieldVal != "")
                                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                                GenerateMasterXMLBuilder.AppendLine("<" + key.ToUpper() + ">" + fieldVal.Trim() + "</" + key.ToUpper() + ">");
                                            }
                                        }
                                    }
                                }
                                #endregion

                                #region GRUPING END TAGS
                                if (agmList.ContainsKey("STOCKGROUP".ToUpper()))
                                {
                                    GenerateMasterXMLBuilder.AppendLine("</STOCKGROUP>");
                                }
                                #endregion

                                GenerateMasterXMLBuilder.AppendLine("</TALLYMESSAGE>");
                            }
                        }
                    }
                    else
                    {
                        dbError = objGenerateMasterXML.dbError;
                    }
                    #endregion
                }
                else
                {
                    #region In Settings Page All Values Are Have Default Values
                    _isNeedToUpdate = false;
                    string fieldVal = string.Empty;
                    GenerateMasterXMLBuilder.AppendLine("<TALLYMESSAGE xmlns:UDF='TallyUDF'>");

                    #region For STOCKGROUP Tag
                    if (agmList.ContainsKey("STOCKGROUP".ToUpper()))
                    {
                        fieldVal = this.GetFieldValue(agmList, "STOCKGROUP");
                        if (fieldVal != "")
                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                        GenerateMasterXMLBuilder.AppendLine("<STOCKGROUP Name='" + fieldVal.Trim() + "' RESERVEDNAME='Create'>");
                    }
                    #endregion

                    #region For PARENT Tag
                    if (agmList.ContainsKey("PARENT".ToUpper()))
                    {
                        fieldVal = this.GetFieldValue(agmList, "PARENT");
                        if (fieldVal != "")
                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                        GenerateMasterXMLBuilder.AppendLine("<PARENT>" + fieldVal.Trim() + "</PARENT>");
                    }
                    #endregion

                    #region For BASEUNITS Tag
                    if (agmList.ContainsKey("BASEUNITS".ToUpper()))
                    {
                        fieldVal = this.GetFieldValue(agmList, "BASEUNITS");
                        if (fieldVal != "")
                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                        GenerateMasterXMLBuilder.AppendLine("<BASEUNITS>" + fieldVal.Trim() + "</BASEUNITS>");
                    }
                    #endregion

                    #region For ADDITIONALUNITS Tag
                    if (agmList.ContainsKey("ADDITIONALUNITS".ToUpper()))
                    {
                        fieldVal = this.GetFieldValue(agmList, "ADDITIONALUNITS");
                        if (fieldVal != "")
                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                        GenerateMasterXMLBuilder.AppendLine("<ADDITIONALUNITS>" + fieldVal.Trim() + "</ADDITIONALUNITS>");
                    }
                    #endregion

                    #region For ISBATCHWISEON Tag
                    if (agmList.ContainsKey("ISBATCHWISEON".ToUpper()))
                    {
                        fieldVal = this.GetFieldValue(agmList, "ISBATCHWISEON");
                        if (fieldVal != "")
                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                        GenerateMasterXMLBuilder.AppendLine("<ISBATCHWISEON>" + fieldVal.Trim() + "</ISBATCHWISEON>");
                    }
                    #endregion

                    #region For ISPERISHABLEON Tag
                    if (agmList.ContainsKey("ISPERISHABLEON".ToUpper()))
                    {
                        fieldVal = this.GetFieldValue(agmList, "ISPERISHABLEON");
                        if (fieldVal != "")
                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                        GenerateMasterXMLBuilder.AppendLine("<ISPERISHABLEON>" + fieldVal.Trim() + "</ISPERISHABLEON>");
                    }
                    #endregion

                    #region For ISADDABLE Tag
                    if (agmList.ContainsKey("ISADDABLE".ToUpper()))
                    {
                        fieldVal = this.GetFieldValue(agmList, "ISADDABLE");
                        if (fieldVal != "")
                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                        GenerateMasterXMLBuilder.AppendLine("<ISADDABLE>" + fieldVal.Trim() + "</ISADDABLE>");
                    }
                    #endregion

                    #region For IGNOREPHYSICALDIFFERENCE Tag
                    if (agmList.ContainsKey("IGNOREPHYSICALDIFFERENCE".ToUpper()))
                    {
                        fieldVal = this.GetFieldValue(agmList, "IGNOREPHYSICALDIFFERENCE");
                        if (fieldVal != "")
                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                        GenerateMasterXMLBuilder.AppendLine("<IGNOREPHYSICALDIFFERENCE>" + fieldVal.Trim() + "</IGNOREPHYSICALDIFFERENCE>");
                    }
                    #endregion

                    #region for IGNORENEGATIVESTOCK Tag
                    if (agmList.ContainsKey("IGNORENEGATIVESTOCK".ToUpper()))
                    {
                        fieldVal = this.GetFieldValue(agmList, "IGNORENEGATIVESTOCK");
                        if (fieldVal != "")
                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                        GenerateMasterXMLBuilder.AppendLine("<IGNORENEGATIVESTOCK>" + fieldVal.Trim() + "</IGNORENEGATIVESTOCK>");
                    }
                    #endregion

                    #region For TREATSALESASMANUFACTURED Tag
                    if (agmList.ContainsKey("TREATSALESASMANUFACTURED".ToUpper()))
                    {
                        fieldVal = this.GetFieldValue(agmList, "TREATSALESASMANUFACTURED");
                        if (fieldVal != "")
                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                        GenerateMasterXMLBuilder.AppendLine("<TREATSALESASMANUFACTURED>" + fieldVal.Trim() + "</TREATSALESASMANUFACTURED>");
                    }
                    #endregion

                    #region For TREATPURCHASESASCONSUMED Tag
                    if (agmList.ContainsKey("TREATPURCHASESASCONSUMED".ToUpper()))
                    {
                        fieldVal = this.GetFieldValue(agmList, "TREATPURCHASESASCONSUMED");
                        if (fieldVal != "")
                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                        GenerateMasterXMLBuilder.AppendLine("<TREATPURCHASESASCONSUMED>" + fieldVal.Trim() + "</TREATPURCHASESASCONSUMED>");
                    }
                    #endregion

                    #region For TREATREJECTSASSCRAP Tag
                    if (agmList.ContainsKey("TREATREJECTSASSCRAP".ToUpper()))
                    {
                        fieldVal = this.GetFieldValue(agmList, "TREATREJECTSASSCRAP");
                        if (fieldVal != "")
                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                        GenerateMasterXMLBuilder.AppendLine("<TREATREJECTSASSCRAP>" + fieldVal.Trim() + "</TREATREJECTSASSCRAP>");
                    }
                    #endregion

                    #region For HASMFGDATE Tag
                    if (agmList.ContainsKey("HASMFGDATE".ToUpper()))
                    {
                        fieldVal = this.GetFieldValue(agmList, "HASMFGDATE");
                        if (fieldVal != "")
                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                        GenerateMasterXMLBuilder.AppendLine("<HASMFGDATE>" + fieldVal.Trim() + "</HASMFGDATE>");
                    }
                    #endregion

                    #region For ALLOWUSEOFEXPIREDITEMS Tag
                    if (agmList.ContainsKey("ALLOWUSEOFEXPIREDITEMS".ToUpper()))
                    {
                        fieldVal = this.GetFieldValue(agmList, "ALLOWUSEOFEXPIREDITEMS");
                        if (fieldVal != "")
                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                        GenerateMasterXMLBuilder.AppendLine("<ALLOWUSEOFEXPIREDITEMS>" + fieldVal.Trim() + "</ALLOWUSEOFEXPIREDITEMS>");
                    }
                    #endregion

                    #region For IGNOREBATCHES Tag
                    if (agmList.ContainsKey("IGNOREBATCHES".ToUpper()))
                    {
                        fieldVal = this.GetFieldValue(agmList, "IGNOREBATCHES");
                        if (fieldVal != "")
                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                        GenerateMasterXMLBuilder.AppendLine("<IGNOREBATCHES>" + fieldVal.Trim() + "</IGNOREBATCHES>");
                    }
                    #endregion

                    #region For IGNOREGODOWNS Tag
                    if (agmList.ContainsKey("IGNOREGODOWNS".ToUpper()))
                    {
                        fieldVal = this.GetFieldValue(agmList, "IGNOREGODOWNS");
                        if (fieldVal != "")
                            fieldVal = CFunctions.convertSpecialChars(fieldVal);
                        GenerateMasterXMLBuilder.AppendLine("<IGNOREGODOWNS>" + fieldVal.Trim() + "</IGNOREGODOWNS>");
                    }
                    #endregion

                    #region For LANGUAGENAME.LIST Tag
                    if (agmList.ContainsKey("LANGUAGENAME.LIST".ToUpper()))
                    {
                        GenerateMasterXMLBuilder.AppendLine("<LANGUAGENAME.LIST>");
                        #region For NAME.LIST Tag
                        if (agmList.ContainsKey("NAME.LIST".ToUpper()))
                        {
                            GenerateMasterXMLBuilder.AppendLine("<NAME.LIST>");
                            #region For Name Tag with in the NAME.LIST
                            if (agmList.ContainsKey("NAME".ToUpper()))
                            {
                                fieldVal = this.GetFieldValue(agmList, "NAME");
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<NAME>" + fieldVal.Trim() + "</NAME>");
                            }
                            #endregion
                            GenerateMasterXMLBuilder.AppendLine("</NAME.LIST>");
                        }
                        #endregion
                        #region For LANGUAGEID Tag
                        if (agmList.ContainsKey("LANGUAGEID".ToUpper()))
                        {
                            fieldVal = this.GetFieldValue(agmList, "LANGUAGEID");
                            if (fieldVal != "")
                                fieldVal = CFunctions.convertSpecialChars(fieldVal);
                            GenerateMasterXMLBuilder.AppendLine("<LANGUAGEID>" + fieldVal.Trim() + "</LANGUAGEID>");
                        }
                        #endregion
                        GenerateMasterXMLBuilder.AppendLine("</LANGUAGENAME.LIST>");
                    }
                    #endregion

                    #region For Rest Of Tags
                    if (agmList.Count > 20)
                    {
                        foreach (string key in agmList.Keys)
                        {
                            if (key.ToUpper() != "STOCKGROUP".ToUpper() && key.ToUpper() != "PARENT".ToUpper() && key.ToUpper() != "BASEUNITS".ToUpper() && key.ToUpper() != "ADDITIONALUNITS".ToUpper() && key.ToUpper() != "ISBATCHWISEON".ToUpper() && key.ToUpper() != "ISPERISHABLEON".ToUpper() && key.ToUpper() != "ISADDABLE".ToUpper()
                                && key.ToUpper() != "IGNOREPHYSICALDIFFERENCE".ToUpper() && key.ToUpper() != "IGNORENEGATIVESTOCK".ToUpper() && key.ToUpper() != "TREATSALESASMANUFACTURED".ToUpper() && key.ToUpper() != "TREATPURCHASESASCONSUMED".ToUpper() && key.ToUpper() != "TREATREJECTSASSCRAP".ToUpper() && key.ToUpper() != "HASMFGDATE".ToUpper()
                                && key.ToUpper() != "ALLOWUSEOFEXPIREDITEMS".ToUpper() && key.ToUpper() != "IGNOREBATCHES".ToUpper() && key.ToUpper() != "IGNOREGODOWNS".ToUpper() && key.ToUpper() != "LANGUAGENAME.LIST".ToUpper() && key.ToUpper() != "NAME.LIST".ToUpper() && key.ToUpper() != "NAME".ToUpper() && key.ToUpper() != "LANGUAGEID".ToUpper())
                            {
                                fieldVal = this.GetFieldValue(agmList, key.ToUpper());
                                if (fieldVal != "")
                                    fieldVal = CFunctions.convertSpecialChars(fieldVal);
                                GenerateMasterXMLBuilder.AppendLine("<" + key.ToUpper() + ">" + fieldVal.Trim() + "</" + key.ToUpper() + ">");
                            }
                        }
                    }
                    #endregion

                    #region GRUPING END TAGS
                    if (agmList.ContainsKey("STOCKGROUP".ToUpper()))
                    {
                        GenerateMasterXMLBuilder.AppendLine("</STOCKGROUP>");
                    }
                    #endregion

                    GenerateMasterXMLBuilder.AppendLine("</TALLYMESSAGE>");

                    #endregion
                }
            }
            catch (Exception ex)
            {
                dbError = ex.Message;
            }
            return GenerateMasterXMLBuilder;
        }

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
            return fieldValue;
        }

        /// <summary>
        /// To Get the Selected Company Settings are Default or Field
        /// </summary>
        /// <param name="_iGenerateMasterXML"></param>
        /// <returns>string</returns>
        private string VerifyDefaultOrField(string[] DefaultValue, string[] FieldValue, string verifyString)
        {
            string returnString = string.Empty;
            if (verifyString != "" && verifyString != null)
            {
                foreach (string value in DefaultValue)
                {
                    if (value != null && value != "")
                    {
                        if (value.ToUpper() == verifyString.ToUpper())
                            returnString = "Default";
                    }
                }

                foreach (string value in FieldValue)
                {
                    if (value != null && value != "")
                    {
                        if (value.ToUpper() == verifyString.ToUpper())
                            returnString = "Field";
                    }
                }
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
            }
        }

        /// <summary>
        /// Recurcive mathod for Item Group Master to get data as parent chaild reletion as tree stucture
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="Parent"></param>        
        private void getItemGroupMasterChild(DataSet ds, string Parent)
        {
            string parent = string.Empty;
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
                    if (itgroupname == false && item["it_group_name"].ToString().Trim() == "")
                    {
                        itgroupname = true;
                        getItemGroupMasterChild(ds, item["it_group_name"].ToString().Trim());                       
                    }
                }
            }
            catch (Exception ex)
            {
                mdbError = ex.Message;
            }
        }

        #endregion
    }
}
