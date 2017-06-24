using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using U2TPlus.DAL;
using System.Configuration;

namespace U2TPlus.BAL
{
    public static class ApplicationValues
    {
        private static string mUserName;
        /// <summary>
        /// Get or Set the Login User
        /// <returns>
        /// string
        /// </returns>
        /// </summary>
        public static string UserName
        {
            get
            {
                return mUserName;
            }
            set
            {
                mUserName = value;
            }
        }
        private static string mApplicationPath;
        /// <summary>
        /// Get or Set the Application Path
        /// <returns>
        /// string
        /// </returns>
        /// </summary>
        public static string ApplicationPath
        {
            get
            {
                return mApplicationPath;
            }
            set
            {
                mApplicationPath = value;
            }
        }
        private static string mConfigXMLFileName;
        /// <summary>
        /// Get or Set the Config XML File Name
        /// <returns>
        /// string
        /// </returns>
        /// </summary>
        public static string ConfigXMLFileName
        {
            get
            {
                return mConfigXMLFileName;
            }
            set
            {
                mConfigXMLFileName = value;
            }
        }
        private static string mvUdyogDBConnectionString;
        /// <summary>
        /// Get or Set the vUdyog Database Connection String
        /// <returns>
        /// string
        /// </returns>
        /// </summary>
        public static string vUdyogDBConnectionString
        {
            get
            {
                return mvUdyogDBConnectionString;
            }
            set
            {
                mvUdyogDBConnectionString = value;
            }
        }
        private static string mCompanyDBConnectionString;
        /// <summary>
        /// Get or Set the Selected Company Database Connection String
        /// <returns>
        /// string
        /// </returns>
        /// </summary>
        public static string CompanyDBConnectionString
        {
            get
            {
                return mCompanyDBConnectionString;
            }
            set
            {
                mCompanyDBConnectionString = value;
            }
        }
        private static string mConfigFolderPath;
        /// <summary>
        /// Get or Set the Config Folder Path (To Store the Config XML File)
        /// <returns>
        /// string
        /// </returns>
        /// </summary>
        public static string ConfigFolderPath
        {
            get
            {
                return mConfigFolderPath;
            }
            set
            {
                mConfigFolderPath = value;
            }
        }
        private static string mConfigFilePath;
        /// <summary>
        /// Get or Set the Config File Path (For Config XML File)
        /// <returns>
        /// string
        /// </returns>
        /// </summary>
        public static string ConfigFilePath
        {
            get
            {
                return mConfigFilePath;
            }
            set
            {
                mConfigFilePath = value;
            }
        }
        private static string mINIFilePath;
        /// <summary>
        /// Get or Set the INI File Path (For Get the Database Crediencials)
        /// <returns>
        /// string
        /// </returns>
        /// </summary>
        public static string INIFilePath
        {
            get
            {
                return mINIFilePath;
            }
            set
            {
                mINIFilePath = value;
            }
        }
        private static string mEncriptedSUPValues;
        /// <summary>
        /// Get or Set the Encripted Server Name, UserID, Password Values
        /// <returns>
        /// string
        /// </returns>
        /// </summary>
        public static string EncriptedSUPValues
        {

            get { return Encript(mEncriptedSUPValues); }
            set { mEncriptedSUPValues = value; }
        }
        private static string mDecriptSUPValues;
        /// <summary>
        /// Get or Set the Decripted Server Name, UserID, Password Values
        /// <returns>
        /// string
        /// </returns>
        /// </summary>
        public static string DecriptSUPValues
        {
            get { return mDecriptSUPValues; }
            set { mDecriptSUPValues = Decript(value); }
        }
        private static DataSet mCompanyMaster;
        /// <summary>
        /// Get or Set to Store the Company Master Details (Company Name,Company ID,DatBase Name,Financial Years)
        /// <returns>
        /// DAtaSet
        /// </returns>
        /// </summary>
        /// 
        public static DataSet CompanyMaster
        {
            get { return mCompanyMaster; }
            set { mCompanyMaster = value; }
        }
        /// <summary>
        /// Return Encripted String Value 
        /// <returns>
        /// string
        /// </returns>
        /// </summary>
        public static string Encript(string strValue)
        {
            U2TPlus.DAL.CommonFunctions CFunctions = new U2TPlus.DAL.CommonFunctions();
            return CFunctions.EncriptValue(strValue);
        }
        /// <summary>
        /// Return Decripted String Value 
        /// <returns>
        /// string
        /// </returns>
        /// </summary>
        public static string Decript(string strValue)
        {
            U2TPlus.DAL.CommonFunctions CFunctions = new U2TPlus.DAL.CommonFunctions();
            return CFunctions.DecriptValue(strValue);
        }
        private static string mDBServerName;
        /// <summary>
        /// Get or Set Database Server Name
        /// <returns>
        /// string
        /// </returns>
        /// </summary>
        public static string DBServerName
        {
            get { return mDBServerName; }
            set { mDBServerName = value; }
        }
        private static string mDBUserID;
        /// <summary>
        /// Get or Set Database User ID
        /// <returns>
        /// string
        /// </returns>
        /// </summary>
        public static string DBUserID
        {
            get { return mDBUserID; }
            set { mDBUserID = value; }
        }
        private static string mDBPassword;
        /// <summary>
        /// Get or Set Database Password
        /// <returns>
        /// string
        /// </returns>
        /// </summary>
        public static string DBPassword
        {
            get { return mDBPassword; }
            set { mDBPassword = value; }
        }
        private static DataSet mConfigFileValues;
        /// <summary>
        /// Get or Set to store the Config XML file values
        /// </summary>
        /// <returns>
        /// DataSet
        /// </returns>
        public static DataSet ConfigFileValues
        {
            get { return mConfigFileValues; }
            set { mConfigFileValues = value; }
        }
        private static string mApplicationName;
        /// <summary>
        /// Get or Set Application Name
        /// <returns>
        /// string
        /// </returns>
        /// </summary>
        public static string ApplicationName
        {
            get { return mApplicationName; }
            set { mApplicationName = value; }
        }
        private static bool mIsEnvironmentSetup = false;
        /// <summary>
        /// Get or Set Is Environment Setup as bool 
        /// <returns>
        /// bool
        /// </returns>
        /// </summary>
        public static bool IsEnvironmentSetup
        {
            get { return mIsEnvironmentSetup; }
            set { mIsEnvironmentSetup = value; }
        }
        private static string mCompanyDBName;
        /// <summary>
        /// Get or Set the Selected Company Database Name
        /// <returns>
        /// string
        /// </returns>
        /// </summary>
        public static string CompanyDBName
        {
            get { return mCompanyDBName; }
            set { mCompanyDBName = value; }
        }
        private static string mCompanyName;
        /// <summary>
        /// Get or Set to Genarete the XML file for Selected Company
        /// <returns>
        /// string
        /// </returns>
        /// </summary>
        public static string CompanyName
        {
            get { return mCompanyName; }
            set { mCompanyName = value; }
        }
        private static string mGeneratCompanyXMLFilePath;
        /// <summary>
        /// Get or Set where need to save the genareted XML file for selected company.
        /// <returns>
        /// string
        /// </returns>
        /// </summary>
        public static string GeneratCompanyXMLFilePath
        {
            get { return mGeneratCompanyXMLFilePath; }
            set { mGeneratCompanyXMLFilePath = value; }
        }
        private static string mGeneratingProductPath;
        /// <summary>
        /// Get or Set Generating Product Path (VUdyog Product Instaled path)
        /// <returns>
        /// string
        /// </returns>
        /// </summary>
        public static string GeneratingProductPath
        {
            get { return mGeneratingProductPath; }
            set { mGeneratingProductPath = value; }
        }
        private static string mTallyVersion;
        /// <summary>
        /// Get or Set Tally Verion of the Selected Company
        /// <returns>
        /// string
        /// </returns>
        /// </summary>
        public static string TallyVersion
        {
            get { return mTallyVersion; }
            set { mTallyVersion = value; }
        }
        private static string mComanyFinancealYear;
        /// <summary>
        /// Get or Set Financial Year of the Selected Company
        /// <returns>
        /// string
        /// </returns>
        /// </summary>
        public static string ComanyFinancealYear
        {
            get { return mComanyFinancealYear; }
            set { mComanyFinancealYear = value; }
        }
        private static StringBuilder mGeneratingXMLHeader;
        /// <summary>
        /// Get or Set Genareting Tally Version Master XML Header
        /// <returns>
        /// string
        /// </returns>
        /// </summary>
        public static StringBuilder GenaretingXMLHeader
        {
            get { return mGeneratingXMLHeader; }
            set { mGeneratingXMLHeader = value; }
        }
        private static StringBuilder mGeneratingXMLFooter;
        /// <summary>
        /// Get or Set Genareting Tally Version MasterXML Footer
        /// <returns>
        /// string
        /// </returns>
        /// </summary>
        public static StringBuilder GeneratingXMLFooter
        {
            get { return mGeneratingXMLFooter; }
            set { mGeneratingXMLFooter = value; }
        }
        //private static StringBuilder mGeneratingXMLMessage;
        //public static StringBuilder GeneratingXMLMessage
        //{
        //    get { return mGeneratingXMLMessage; }
        //    set { mGeneratingXMLMessage = value; }
        //}
        //private static StringBuilder mGeneratingXMLMessageData;
        //public static StringBuilder GeneratingXMLMessageData
        //{
        //    get { return mGeneratingXMLMessageData; }
        //    set { mGeneratingXMLMessageData = value; }
        //}
        private static string mU2TMarkupsFolderPath;
        /// <summary>
        /// Get or Set U2T Markups Folder Path to Get the Settings XML's
        /// <returns>
        /// string
        /// </returns>
        /// </summary>
        public static string U2TMarkupsFolderPath
        {
            get { return mU2TMarkupsFolderPath; }
            set { mU2TMarkupsFolderPath = value; }
        }
        private static string mCompanyConfigID;
        /// <summary>
        /// Get or Set Company Configuretion ID from Config.xml
        /// <returns>
        /// string
        /// </returns>
        /// </summary>
        public static string CompanyConfigID
        {
            get { return mCompanyConfigID; }
            set { mCompanyConfigID = value; }
        }
        private static string mtableName;
        /// <summary>
        /// Get or Set Table Name form databse for Selected Company (To Generate XML)
        /// <returns>
        /// string
        /// </returns>
        /// </summary>
        public static string tableName
        {
            get { return mtableName; }
            set { mtableName = value; }
        }
        private static string mDefaultMasterSettingsXMLName;
        /// <summary>
        /// Get or Set Default Master Settings XML file name from markups folder
        /// <returns>
        /// string
        /// </returns>
        /// </summary>
        public static string DefaultMasterSettingsXMLName
        {
            get { return mDefaultMasterSettingsXMLName; }
            set { mDefaultMasterSettingsXMLName = value; }
        }
        private static string mGenaretingMasterName;
        /// <summary>
        /// Get or Set Genareting Master Name to format the XML tags
        /// </summary>
        public static string GenaretingMasterName
        {
            get { return mGenaretingMasterName; }
            set { mGenaretingMasterName = value; }
        }
        private static string mTitleMasterName;
        /// <summary>
        /// Get or Set Genareting Master Name
        /// </summary>
        public static string TitleMasterName
        {
            get { return mTitleMasterName; }
            set { mTitleMasterName = value; }
        }
        private static StringBuilder mTranXMLHeader;
        /// <summary>
        /// Get or Set Genareting Tally Version Transaction XML Header
        /// <returns>
        /// string
        /// </returns>
        /// </summary>
        public static StringBuilder TranXMLHeader
        {
            get { return mTranXMLHeader; }
            set { mTranXMLHeader = value; }
        }
        private static StringBuilder mTranXMLFooter;
        /// <summary>
        /// Get or Set Genareting Tally Version Transaction XML Footer
        /// <returns>
        /// string
        /// </returns>
        /// </summary>
        public static StringBuilder TranXMLFooter
        {
            get { return mTranXMLFooter; }
            set { mTranXMLFooter = value; }
        }
        private static string mTransEntryType;
        /// <summary>
        /// Get or Set the Entry Type of the Transaction
        /// </summary>
        public static string TransEntryType
        {
            get { return mTransEntryType; }
            set { mTransEntryType = value; }
        }
        private static bool mMainFormIsActiveted = false;
        public static bool MainFormIsActiveted
        {
            get { return mMainFormIsActiveted; }
            set { mMainFormIsActiveted = value; }
        }
        private static string mDataMappingFolderName;
        public static string DataMappingFolderName
        {
            get { return mDataMappingFolderName; }
            set { mDataMappingFolderName = value; }
        }
        private static string mDataMappingFileName;
        public static string DataMappingFileName
        {
            get { return mDataMappingFileName; }
            set { mDataMappingFileName = value; }
        }
        private static string[] mAccountSwappingDataTables = null;

        public static string[] AccountSwappingDataTables
        {
            get { return mAccountSwappingDataTables; }
            set { mAccountSwappingDataTables = value; }
        }

        private static string[] mAccountGroupSwappingDataTables = null;
        public static string[] AccountGroupSwappingDataTables
        {
            get { return mAccountGroupSwappingDataTables; }
            set { mAccountGroupSwappingDataTables = value; }
        }

    }
}
