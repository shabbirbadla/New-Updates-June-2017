using System;
using System.Collections.Generic;
using System.Text;
using U2TPlus.DAL;
using U2TPlus.DAL.Interface;
using System.Data;

namespace U2TPlus.BAL
{
    public class CompanyInfo : ICompanyInfo
    {
        U2TPlus.DAL.CompanyInfo objCompanyInfo = new U2TPlus.DAL.CompanyInfo();

        #region ICompanyInfo Members
        private string mConnectionString;
        /// <summary>
        /// Get or Set Connection String for Selected Company
        /// <return>
        /// String
        /// </return>
        /// </summary>
        public string ConnectionString
        {
            get { return mConnectionString; }
            set { mConnectionString = value; }
        }
        private string mCompanyDBName;
        /// <summary>
        /// Get or Set Database Name for Selected Company
        /// <return>
        /// string
        /// </return>
        /// </summary>
        public string CompanyDBName
        {
            get { return mCompanyDBName; }
            set { mCompanyDBName = value; }
        }
        private string mCompanyMasterConnectionString;
        /// <summary>
        /// Get or Set Company Master Connection String To Know The Total Company Details
        /// <return>
        /// string
        /// </return>
        /// </summary>
        public string CompanyMasterConnectionString
        {
            get { return mCompanyMasterConnectionString; }
            set { mCompanyMasterConnectionString = value; }
        }
        #endregion

        #region Constractor

        public CompanyInfo()
        {

        }

        #endregion

        #region Common Preoperties

        private DataSet mDS;
        /// <summary>
        /// Get DataSet of the Exicuted Table Values
        /// <return>
        /// DataSet
        /// </return>
        /// </summary>
        public DataSet DS
        {
            get { return mDS; }
        }

        private string mdbError = string.Empty;
        /// <summary>
        /// Get Error Report when Exicuting Functions
        /// <return>
        /// String
        /// </return>
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
        public DataSet GetCompanyMasterTableNames()
        {
            try
            {
                objCompanyInfo.ConnectionString = this.ConnectionString;
                objCompanyInfo.CompanyDBName = this.CompanyDBName;
                mDS = new DataSet();
                mDS = objCompanyInfo.GetCompanyMasterTableNames(this);

                mdbError = objCompanyInfo.DBError;

                return DS;
            }
            catch (Exception ex)
            {
                mdbError = ex.Message;
                Logger.LogInfo(ex);
            }

            return DS;
        }
        /// <summary>
        /// Get all Companies Data from USquare database and co_mast table
        /// </summary>
        /// <param name="_iCompanyInfo"></param>
        /// <returns>DataSet</returns>
        public DataSet CompanyMasterData()
        {
            objCompanyInfo.CompanyMasterConnectionString = ApplicationValues.vUdyogDBConnectionString;
            mDS = objCompanyInfo.CompanyMasterData(this);
            mdbError = objCompanyInfo.DBError;
            Logger.LogInfo(mdbError);
            return DS;
        }
        /// <summary>
        /// Get all Companies Financial years from USquare database and co_mast table
        /// </summary>
        /// <param name="_iCompanyInfo"></param>
        /// <returns>DataSet</returns>
        public DataSet MasterCompanyFinancealYears()
        {
            objCompanyInfo.CompanyMasterConnectionString = ApplicationValues.vUdyogDBConnectionString;
            mDS = objCompanyInfo.MasterCompanyFinancealYears(this);
            mdbError = objCompanyInfo.DBError;
            Logger.LogInfo(mdbError);
            return DS;
        }

        #endregion

        public DataSet GetCompanyTransTableNames()
        {            
            try
            {
                objCompanyInfo.ConnectionString = this.ConnectionString;
                objCompanyInfo.CompanyDBName = this.CompanyDBName;
                mDS = new DataSet();
                mDS = objCompanyInfo.GetCompanyTransTableNames(this);

                mdbError = objCompanyInfo.DBError;

                return DS;
            }
            catch (Exception ex)
            {
                mdbError = ex.Message;
                Logger.LogInfo(ex);
            }

            return DS;
        }

       
    }
}
