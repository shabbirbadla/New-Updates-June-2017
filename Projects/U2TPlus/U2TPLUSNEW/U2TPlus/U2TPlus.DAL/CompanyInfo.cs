using System;
using System.Collections.Generic;
using System.Text;
using U2TPlus.DAL.Interface;
using System.Data;
using System.Configuration;

namespace U2TPlus.DAL
{
    public class CompanyInfo : ICompanyInfo
    {
        #region ICompanyInfo Members
        private string mConnectionString;
        /// <summary>
        /// Get or Set Connection String for Selected Company
        /// <returns>
        /// String
        /// </returns>
        /// </summary>
        public string ConnectionString
        {
            get { return mConnectionString; }
            set { mConnectionString = value; }
        }
        private string mCompanyDBName;
        /// <summary>
        /// Get or Set Database Name for Selected Company
        /// <returns>
        /// string
        /// </returns>
        /// </summary>
        public string CompanyDBName
        {
            get { return mCompanyDBName; }
            set { mCompanyDBName = value; }
        }
        private string mCompanyMasterConnectionString;
        /// <summary>
        /// Get or Set Company Master Connection String To Know The Total Company Details
        /// <returns>
        /// string
        /// </returns>
        /// </summary>
        public string CompanyMasterConnectionString
        {
            get { return mCompanyMasterConnectionString; }
            set { mCompanyMasterConnectionString = value; }
        }
        #endregion

        #region Constractor

        DBConnections objConnection;

        public CompanyInfo()
        {
            objConnection = new DBConnections();
        }

        #endregion

        #region Common Preoperties

        private DataSet mDS;
        /// <summary>
        /// Get DataSet of the Exicuted Table Values
        /// <returns>
        /// DataSet
        /// </returns>
        /// </summary>
        public DataSet DS
        {
            get { return mDS; }
        }

        private string mdbError = string.Empty;
        /// <summary>
        /// Get Error Report when Exicuting Functions
        /// <returns>
        /// String
        /// </returns>
        /// </summary>
        public string DBError
        {
            get { return mdbError; }
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Get the all Master table names from selected Company
        /// </summary>
        /// <param name="_iCompanyInfo"></param>
        /// <returns>DataSet</returns>
        public DataSet GetCompanyMasterTableNames(ICompanyInfo _iCompanyInfo)
        {
            try
            {
                objConnection.AppConnectionString = _iCompanyInfo.ConnectionString;
                objConnection.ClearParameters();
                objConnection.CommandTypeToExecute = CommandType.Text;
                string gMasters = ConfigurationManager.AppSettings["Masters"].ToString();
                objConnection.CommandText = "Select * From " + CompanyDBName + "..Mastcode where code in (" + gMasters + ")";
                mDS = new DataSet();
                mDS = objConnection.GetDataSet("CompanyMasterTableNames");
                mdbError = objConnection.DBError;
            }
            catch (Exception ex)
            {
                mdbError = ex.Message;
            }

            return DS;
        }
        /// <summary>
        /// Get all Companies Data from USquare database and co_mast table
        /// </summary>
        /// <param name="_iCompanyInfo"></param>
        /// <returns>DataSet</returns>
        public DataSet CompanyMasterData(ICompanyInfo _iCompanyInfo)
        {
            try
            {
                objConnection.AppConnectionString = _iCompanyInfo.CompanyMasterConnectionString;
                objConnection.ClearParameters();
                objConnection.CommandTypeToExecute = CommandType.Text;
                objConnection.CommandText = "select [CompId] as [CompanyId],LTRIM(RTRIM([co_name])) as [CompanyName], LTRIM(RTRIM([dbname])) as [DataBaseName], (Convert(varchar,year([sta_dt])) + '-' + Convert(varchar,year([end_dt]))) as [FinancialYear] from [co_mast]";
                mDS = new DataSet();
                mDS = objConnection.GetDataSet("CompanyMaster");
            }
            catch (Exception ex)
            {
                mdbError = ex.Message;
            }

            return DS;
        }
        /// <summary>
        /// Get all Companies Financial years from USquare database and co_mast table
        /// </summary>
        /// <param name="_iCompanyInfo"></param>
        /// <returns>DataSet</returns>
        public DataSet MasterCompanyFinancealYears(ICompanyInfo _iCompanyInfo)
        {
            try
            {
                objConnection.AppConnectionString = _iCompanyInfo.CompanyMasterConnectionString;
                objConnection.ClearParameters();
                objConnection.CommandTypeToExecute = CommandType.Text;
                objConnection.CommandText = "select distinct((Convert(varchar,year([sta_dt])) + '-' + Convert(varchar,year([end_dt])))) as [FinancialYear] from [co_mast]";
                mDS = new DataSet();
                mDS = objConnection.GetDataSet("FinancealYears");
            }
            catch (Exception ex)
            {
                mdbError = ex.Message;
            }
            return DS;
        }

        #endregion

        public DataSet GetCompanyTransTableNames(ICompanyInfo _iCompanyInfo)
        {
            try
            {
                objConnection.AppConnectionString = _iCompanyInfo.ConnectionString;
                objConnection.ClearParameters();
                objConnection.CommandTypeToExecute = CommandType.Text;
                string gMasters = ConfigurationManager.AppSettings["Transactions"].ToString();
                objConnection.CommandText = "Select * From " + CompanyDBName + "..lcode where Entry_Ty in (" + gMasters + ")";
                mDS = new DataSet();
                mDS = objConnection.GetDataSet("CompanyTransTableNames");
                mdbError = objConnection.DBError;
            }
            catch (Exception ex)
            {
                mdbError = ex.Message;
            }

            return DS;
        }
    }
}
