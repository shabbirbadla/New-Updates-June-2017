using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eMailClient.DAL;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace eMailClient.BLL
{
    public class cls_Gen_Mgr_Email_Settings : cls_Gen_Ent_Email_Settings,ICommonBLL
    {
        #region variable declaration
        string connectionString;
        SqlConnection sqlconn;
        SqlTransaction sqltran;
        #endregion

       /// public cls_Gen_Mgr_Email_Settings(Int32 CompanyID, string Server, string User, string Pass)
        // public cls_Gen_Mgr_Email_Settings(Int32 CompanyID)
        public cls_Gen_Mgr_Email_Settings(Int32 CompanyID,string Connectionstring)//added by satish pal
        {
            

        // connectionString = ConfigurationManager.ConnectionStrings[Convert.ToString(CompanyID).Trim()].ConnectionString;
            connectionString = Connectionstring;// added by satish pal

            
           DsSearch = new DataSet();
        }

        #region Enumeration Members
        public enum eMailClient_RowName : int
        {
            yourname = 0,
            username,
            password,
            host,
            port,
            enablessl
        }
        #endregion

        #region ICommonBLL Members
        public void Select()
        {
            try
            {
                // Commented by Sachin N. S. on 
                //if (sqlconn == null)
                //{
                //    sqlconn = new SqlConnection(connectionString);
                //    sqlconn.Open();
                //}
                //else
                //{
                //    if (sqlconn.State == ConnectionState.Closed)
                //        sqlconn.Open();
                //}

                DataSet m_DsSelect = new DataSet();
                m_DsSelect = cls_Sqlhelper.ExecuteDataset(connectionString, CommandType.Text, "Select * From eMailSettings");
                DsSearch = m_DsSelect;
                m_DsSelect.Dispose();
                if (DsSearch.Tables[0].Rows.Count > 0)
                {
                    DataRow m_rowDsSelect = DsSearch.Tables[0].Rows[0];
                    Binding(m_rowDsSelect);
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                //sqlconn.Close(); 
            }
        }

        public void Insert()
        {
            int m_execute = 0;
            try
            {   
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

                string pwd = EncryptData(Password.Trim());

                m_execute = cls_Sqlhelper.ExecuteNonQuery(sqltran, CommandType.Text, "Insert into eMailSettings(Yourname,Username,Password,Host,Port,"+
                    "EnableSSL) values('" + Yourname.Trim() + "','" + Username.Trim() + "','" + pwd + "','" + Host.Trim() + "'," + 
                    Convert.ToString(Port) + ",'" + Convert.ToBoolean(Enablessl) + "')");

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

        public void Update()
        {
            int m_execute = 0;
            try
            {
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

                string pwd = EncryptData(Password.Trim());

                m_execute = cls_Sqlhelper.ExecuteNonQuery(sqltran, CommandType.Text, "Update eMailSettings set Yourname='" + Yourname.Trim() + "',Username='" +
                    Username.Trim() + "',Password='" + pwd + "',Host='" + Host.Trim() + "',Port=" + Convert.ToInt32(Port) + ",EnableSSL='" + 
                    Convert.ToBoolean(Enablessl) + "'");

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
        }
        #endregion

        #region Other Methods
        private void Binding(DataRow m_rowDsSelect)
        {
            Yourname = Convert.ToString(m_rowDsSelect["yourname"]);
            Username = Convert.ToString(m_rowDsSelect["username"]);
            Password = Convert.ToString(m_rowDsSelect["password"]);
            Host = Convert.ToString(m_rowDsSelect["host"]);
            Port = Convert.ToInt32(m_rowDsSelect["port"]);
            Enablessl = Convert.ToBoolean(m_rowDsSelect["enablessl"]);
        }

        public void RecordCount()
        {
            DataSet m_DsSelect = new DataSet();
            m_DsSelect = cls_Sqlhelper.ExecuteDataset(connectionString, CommandType.Text, "Select * From eMailSettings");
            DsSearch = m_DsSelect;
            m_DsSelect.Dispose();
        }

        public string EncryptData(string password)
        {
            string pwd = string.Empty;
            byte[] encode = new byte[password.Length];
            encode = System.Text.Encoding.UTF8.GetBytes(password);
            pwd = Convert.ToBase64String(encode);
            return pwd;
        }

        public string DecryptData(string encryptpwd)
        {
            string decryptpwd = string.Empty;
            UTF8Encoding encodepwd = new UTF8Encoding();
            Decoder decode = encodepwd.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encryptpwd);
            int charCount = decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            decryptpwd = new String(decoded_char);
            return decryptpwd;
        }
        #endregion
    }
}
