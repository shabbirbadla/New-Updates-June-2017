using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

using eMailClient.DAL;

namespace eMailClient.BLL
{
    public class cls_Gen_Mgr_Email_LogWriter : cls_Gen_Ent_Email_LogWriter
    {
        #region variable declaration
        string connectionString;
        SqlConnection sqlconn;
        SqlTransaction sqltran;
        #endregion

        public cls_Gen_Mgr_Email_LogWriter(Int32 CompanyID,String  ConnectionString)
        {
           // connectionString = ConfigurationManager.ConnectionStrings[Convert.ToString(CompanyID).Trim()].ConnectionString;
            connectionString = ConnectionString;
        }

        #region Other Methods
        public void UpdateEmailLog()
        {
            int m_execute = 0;
            try
            {
                SqlParameter[] m_spParam = {new SqlParameter("@id",Id),
                                       new SqlParameter("@to",To),
                                       new SqlParameter("@cc",Cc),
                                       new SqlParameter("@bcc",Bcc),
                                       new SqlParameter("@subject",Subject),
                                       new SqlParameter("@body",Body),
                                       new SqlParameter("@filepath",Filepath),
                                       new SqlParameter("@filename",Filename),
                                       new SqlParameter("@removefiles",Removefiles),
                                       new SqlParameter("@status",Status),
                                       new SqlParameter("@remarks",Remarks),
                                       new SqlParameter("emaillogfiles",Emaillogfiles),
                                       new SqlParameter("@logemailid",Logemailid)};

                if (sqlconn == null)
                {
                    sqlconn = new SqlConnection(connectionString);
                    sqlconn.Open();
                }
                else
                {
                    if (sqlconn.State == ConnectionState.Closed)
                        sqlconn.Open();
                }

                sqltran = sqlconn.BeginTransaction();

                m_execute = cls_Sqlhelper.ExecuteNonQuery(sqltran, "USP_ENT_EMAILLOG_INSERTUPDATE", m_spParam);

                sqltran.Commit();
            }
            catch (Exception ex)
            {
                if (sqltran != null)
                {
                    sqltran.Rollback();
                }
                throw ex;
            }
            finally
            {
                sqlconn.Close();
            }
        }

        public void Delete()
        {
            int m_execute = 0;
            try
            {
                SqlParameter[] m_spParam = { new SqlParameter("@id", Id),
                                           new SqlParameter("@filename",Filename) };

                if (sqlconn == null)
                {
                    sqlconn = new SqlConnection(connectionString);
                    sqlconn.Open();
                }
                else
                {
                    if (sqlconn.State == ConnectionState.Closed)
                        sqlconn.Open();
                }

                sqltran = sqlconn.BeginTransaction();

                m_execute = cls_Sqlhelper.ExecuteNonQuery(sqltran, "USP_ENT_EMAILLOG_DELETE", m_spParam);

                sqltran.Commit();
            }
            catch (Exception ex)
            {
                if (sqltran != null)
                {
                    sqltran.Rollback();
                }
                throw ex;
            }
            finally
            {
                sqlconn.Close();
            }
        }
        #endregion
    }
}
