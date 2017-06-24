using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using eMailClient.DAL;
using System.Configuration;
using System.Diagnostics;
using System.IO;

namespace eMailClient.BLL
{
    public class cls_Gen_Mgr_Email_Client : cls_Gen_Ent_Email_Client, ICommonBLL
    {
        #region variable declaration
        string connectionString;
        SqlConnection sqlconn;
        SqlTransaction sqltran;
        #endregion

        public cls_Gen_Mgr_Email_Client(Int32 CompanyID, String Connectionstring)//satish pal
        {
            ///  Satish Pal-Satrt
            //connectionString = ConfigurationManager.ConnectionStrings[Convert.ToString(CompanyID).Trim()].ConnectionString;
            connectionString = Connectionstring;
            this.CompanyID = CompanyID;     // Added by Sachin N. S. on 18/01/2014 for Bug-20211
            DsSearch = new DataSet();
            LstTranTyp = new List<string>();
        }

        #region Enumeration Members
        public enum eMailClient_RowName : int
        {
            id = 0,
            desc,
            tran_typ,
            hasattachment,
            attachment_typ,
            rep_nm,
            to,
            cc,
            bcc,
            subject,
            body,
            custnm,
            query,
            reportquery,
            parameters,
            separator,
            encoding,
            isfirstrow,
            reportquerytype,
            exportpath,
            exportprefixname,
            removefiles,
            emaillogfiles,
            logemailid,
            //**** Added by Sachin N. S. on 15/01/2014 for Bug-20211 ****// -- Start
            repGroup,
            repDesc,
            repRep_Nm
            //**** Added by Sachin N. S. on 15/01/2014 for Bug-20211 ****// -- End
        }
        #endregion

        #region ICommonBLL Members
        public void Select()
        {
            SqlParameter[] m_spParam = { new SqlParameter("@action","Select"),
                                           new SqlParameter("@id",Id),
                                       new SqlParameter("@custnm",eMailClient_RowName.custnm)};
            DataSet m_DsSelect = new DataSet("DseMailClient");
            m_DsSelect = cls_Sqlhelper.ExecuteDataset(connectionString, "USP_ENT_EMAILCLIENT_SELECT", m_spParam);
            DataRow m_rowDsSelect = m_DsSelect.Tables[0].Rows[0];
            Binding(m_rowDsSelect);
        }

        public void Insert()
        {
            int m_execute = 0;
            try
            {
                validation();
                //***** Added by Sachin N. S. on 21/01/2014 for Bug-20211 -- Start *****//
                Rep_nm = "";
                Reportquery = "";
                Reportquerytype = "";
                //***** Added by Sachin N. S. on 21/01/2014 for Bug-20211 -- End *****//

                SqlParameter[] m_spParam = { new SqlParameter("@action","Insert"),
                                       new SqlParameter("@id",Id),
                                       new SqlParameter("@desc",Desc),
                                       new SqlParameter("@tran_typ",Tran_typ),
                                       new SqlParameter("@hasattachment",Hasattachment),
                                       new SqlParameter("@attachment_typ",Attachment_typ),
                                       new SqlParameter("@rep_nm",Rep_nm),
                                       new SqlParameter("@to",To),
                                       new SqlParameter("@cc",Cc),
                                       new SqlParameter("@bcc",Bcc),
                                       new SqlParameter("@subject",Subject),
                                       new SqlParameter("@body",Body),
                                       new SqlParameter("@query",Query),
                                       new SqlParameter("@reportquery",Reportquery),
                                       new SqlParameter("@parameters",Parameters),
                                       new SqlParameter("@separator",Separator),
                                       new SqlParameter("@encoding",Encoding),
                                       new SqlParameter("@isFrstrow",IsFirstrow),
                                       new SqlParameter("@reportquerytype",Reportquerytype),
                                       new SqlParameter("@exportpath",Exportpath),
                                       new SqlParameter("@exportprefixname",Exportprefixname),
                                       new SqlParameter("@removefiles",Removefiles),
                                       new SqlParameter("@emaillogfiles",Emaillogfiles),
                                       new SqlParameter("@logemailid",Logemailid),
                                       //**** Added by Sachin N. S. on 15/01/2014 for Bug-20211 ****// -- Start
                                       new SqlParameter("@repgroup",RepGroup),
                                           new SqlParameter("@repDesc",RepDesc),
                                           new SqlParameter("@repRep_Nm",RepRep_Nm)};
                //**** Added by Sachin N. S. on 15/01/2014 for Bug-20211 ****// -- End
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

                m_execute = cls_Sqlhelper.ExecuteNonQuery(sqltran, "USP_ENT_EMAILCLIENT_INSERTUPDATE", m_spParam);

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
                validation();
                //***** Added by Sachin N. S. on 21/01/2014 for Bug-20211 -- Start *****//
                Rep_nm = "";
                Reportquery = "";
                Reportquerytype = "";
                //***** Added by Sachin N. S. on 21/01/2014 for Bug-20211 -- End *****//

                SqlParameter[] m_spParam = { new SqlParameter("@action","Update"),
                                       new SqlParameter("@id",Id),
                                       new SqlParameter("@desc",Desc),
                                       new SqlParameter("@tran_typ",Tran_typ),
                                       new SqlParameter("@hasattachment",Hasattachment),
                                       new SqlParameter("@attachment_typ",Attachment_typ),
                                       new SqlParameter("@rep_nm",Rep_nm),
                                       new SqlParameter("@to",To),
                                       new SqlParameter("@cc",Cc),
                                       new SqlParameter("@bcc",Bcc),
                                       new SqlParameter("@subject",Subject),
                                       new SqlParameter("@body",Body),
                                       new SqlParameter("@query",Query),
                                       new SqlParameter("@reportquery",Reportquery),
                                       new SqlParameter("@parameters",Parameters),
                                       new SqlParameter("@separator",Separator),
                                       new SqlParameter("@encoding",Encoding),
                                       new SqlParameter("@isFrstrow",IsFirstrow),
                                       new SqlParameter("@reportquerytype",Reportquerytype),
                                       new SqlParameter("@exportpath",Exportpath),
                                       new SqlParameter("@exportprefixname",Exportprefixname),
                                       new SqlParameter("removefiles",Removefiles),
                                       new SqlParameter("@emaillogfiles",Emaillogfiles),
                                       new SqlParameter("@logemailid",Logemailid),
                                       //**** Added by Sachin N. S. on 15/01/2014 for Bug-20211 ****// -- Start
                                       new SqlParameter("@repgroup",RepGroup),
                                           new SqlParameter("@repDesc",RepDesc),
                                           new SqlParameter("@repRep_Nm",RepRep_Nm)};
                //**** Added by Sachin N. S. on 15/01/2014 for Bug-20211 ****// -- End

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

                m_execute = cls_Sqlhelper.ExecuteNonQuery(sqltran, "USP_ENT_EMAILCLIENT_INSERTUPDATE", m_spParam);

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
                validation();
                SqlParameter[] m_spParam = { new SqlParameter("@id", Id) };

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

                m_execute = cls_Sqlhelper.ExecuteNonQuery(sqltran, "USP_ENT_EMAILCLIENT_DELETE", m_spParam);

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

        #region Other Methods
        private void Binding(DataRow m_rowDsSelect)
        {
            Id = Convert.ToString(m_rowDsSelect["id"]);
            Desc = Convert.ToString(m_rowDsSelect["desc"]);
            Tran_typ = Convert.ToString(m_rowDsSelect["tran_typ"]);
            if (DBNull.Value == m_rowDsSelect["hasattachment"])
                Hasattachment = false;
            else
                Hasattachment = Convert.ToBoolean(m_rowDsSelect["hasattachment"]);
            Attachment_typ = Convert.ToString(m_rowDsSelect["attachment_typ"]);
            //Rep_nm = Convert.ToString(m_rowDsSelect["rep_nm"]);
            Rep_nm = Convert.ToString(m_rowDsSelect["Reprep_nm"]);      // Changed by Sachin N. S. on 20/01/2014 for Bug-20211
            To = Convert.ToString(m_rowDsSelect["to"]);
            Cc = Convert.ToString(m_rowDsSelect["cc"]);
            Bcc = Convert.ToString(m_rowDsSelect["bcc"]);
            Subject = Convert.ToString(m_rowDsSelect["subject"]);
            Body = Convert.ToString(m_rowDsSelect["body"]);
            Query = Convert.ToString(m_rowDsSelect["query"]);
            //Reportquery = Convert.ToString(m_rowDsSelect["reportquery"]);
            Reportquery = Convert.ToString(m_rowDsSelect["sqlquery"]);       // Changed by Sachin N. S. on 20/01/2014 for Bug-20211
            Parameters = Convert.ToString(m_rowDsSelect["parameters"]);
            Separator = Convert.ToString(m_rowDsSelect["separator"]);
            Encoding = Convert.ToString(m_rowDsSelect["encoding"]);
            if (DBNull.Value == m_rowDsSelect["isFirstrow"])
                IsFirstrow = false;
            else
                IsFirstrow = Convert.ToBoolean(m_rowDsSelect["isfirstrow"]);
            //Reportquerytype = Convert.ToString(m_rowDsSelect["reportquerytype"]);
            Reportquerytype = Convert.ToString(m_rowDsSelect["spWhat"]);       // Changed by Sachin N. S. on 20/01/2014 for Bug-20211
            Exportpath = Convert.ToString(m_rowDsSelect["exportpath"]);
            Exportprefixname = Convert.ToString(m_rowDsSelect["exportprefixname"]);
            if (DBNull.Value == m_rowDsSelect["removefiles"])
                Removefiles = false;
            else
                Removefiles = Convert.ToBoolean(m_rowDsSelect["removefiles"]);
            if (DBNull.Value == m_rowDsSelect["emaillogfiles"])
                Emaillogfiles = false;
            else
                Emaillogfiles = Convert.ToBoolean(m_rowDsSelect["emaillogfiles"]);
            Logemailid = Convert.ToString(m_rowDsSelect["logemailid"]);

            //**** Added by Sachin N. S. on 20/01/2014 for Bug-20211 -- Start ****//
            RepGroup = Convert.ToString(m_rowDsSelect["RepGroup"]);
            RepDesc = Convert.ToString(m_rowDsSelect["RepDesc"]);
            RepRep_Nm = Convert.ToString(m_rowDsSelect["RepRep_Nm"]);
            SqlQuery = Convert.ToString(m_rowDsSelect["SqlQuery"]);
            SpWhat = Convert.ToString(m_rowDsSelect["SpWhat"]);
            QTable = Convert.ToString(m_rowDsSelect["qTable"]);
            //**** Added by Sachin N. S. on 20/01/2014 for Bug-20211 -- End ****//
        }

        public void SearchById()
        {
            DataSet m_DsSearch = new DataSet();
            m_DsSearch = cls_Sqlhelper.ExecuteDataset(connectionString, "USP_ENT_EMAILCLIENT_SELECT_SEARCH");
            DsSearch = m_DsSearch;
            m_DsSearch.Dispose();
        }

        public void FillTranType()
        {
            DataSet m_DsFill = new DataSet();
            m_DsFill = cls_Sqlhelper.ExecuteDataset(connectionString, CommandType.Text, "Select rtrim(Code_nm) as Code_nm,rtrim(Entry_ty) as Entry_ty From Lcode Order by Code_nm");
            DsSearch = m_DsFill;
            m_DsFill.Dispose();
        }

        private void validation()
        {
            //if (Tran_typ == string.Empty)
            //    throw new Exception("Please select the Transaction Type");
            //if (To == string.Empty && To == null)
            //    throw new Exception("There must be atleast one or multiple emailid(s) in the To, Cc or Bcc box.");            
        }

        public void AutoId()
        {
            DataSet m_DsAutoId = new DataSet();
            m_DsAutoId = cls_Sqlhelper.ExecuteDataset(connectionString, CommandType.Text, "Select 'EM' + Case When count(*) = 0 then cast(1 as varchar(1)) else convert(varchar(20), max(convert(int, substring(id, 3, 100))) + 1) end From eMailClient");
            DsSearch = m_DsAutoId;
            m_DsAutoId.Dispose();
        }

        public void EmailIdList(string custnm)
        {
            DataSet m_DsEmailList = new DataSet();
            SqlParameter[] m_spParam = { new SqlParameter("@action","Ac_mast"),
                                           new SqlParameter("@id",Id),
                                           new SqlParameter("@custnm",custnm) };
            m_DsEmailList = cls_Sqlhelper.ExecuteDataset(connectionString, "USP_ENT_EMAILCLIENT_SELECT", m_spParam);
            DsSearch = m_DsEmailList;
            m_DsEmailList.Dispose();
        }

        public string QueryResult(string query)
        {
            string retResult;
            try
            {
                DataSet m_DsQueryResult = new DataSet();
                m_DsQueryResult = cls_Sqlhelper.ExecuteDataset(connectionString, CommandType.Text, query);
                DsQuery = m_DsQueryResult;
                m_DsQueryResult.Dispose();

                retResult = "Query Executed Successfully";
            }
            catch (SqlException sqlex)
            {
                retResult = sqlex.Message.Trim();
            }
            catch (Exception ex)
            {
                retResult = ex.Message.Trim();
            }

            return retResult;
        }

        public void FillEmailDetails()
        {
            DataSet m_DsFillDtls = new DataSet();
            m_DsFillDtls = cls_Sqlhelper.ExecuteDataset(connectionString, CommandType.Text, "Select rtrim(Id) as Id,rtrim([Desc]) as [Desc] From eMailClient");
            DsSearch = m_DsFillDtls;
            m_DsFillDtls.Dispose();
        }

        //***** Added by Sachin N. S. on 14/01/2014 for Bug-20211 *****// -- Start
        public void SearchReportWizard()
        {
            SqlParameter[] m_spParam = { new SqlParameter("@action","REPORTWIZARD"),
                                           new SqlParameter("@id",""),
                                       new SqlParameter("@custnm","")};

            DataSet m_DsSearch = new DataSet();
            m_DsSearch = cls_Sqlhelper.ExecuteDataset(connectionString, "USP_ENT_EMAILCLIENT_SELECT", m_spParam);
            DsSearch = m_DsSearch;
            m_DsSearch.Dispose();
        }

        public string getRptFilePath()
        {
            DataSet m_DsSelect = new DataSet("DsCompanyRec");
            m_DsSelect = cls_Sqlhelper.ExecuteDataset(connectionString,CommandType.Text, "SELECT [DbName] FROM VUDYOG..CO_MAST WHERE COMPID="+CompanyID.ToString());
            return m_DsSelect.Tables[0].Rows[0]["DbName"].ToString().Trim();
        }
        //***** Added by Sachin N. S. on 14/01/2014 for Bug-20211 *****// -- End
        #endregion
    }
}
