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
    public class cls_Gen_Mgr_PendingMail:cls_Gen_Ent_PendingMail
    {
        #region variable declaration
        string connectionString;
        #endregion

        //public cls_Gen_Mgr_PendingMail(Int32 CompanyID)
        public cls_Gen_Mgr_PendingMail(Int32 CompanyID, string Connectionstring)    // Changed by Sachin N. S. on 24/01/2014 for Bug-20211
        {
           //connectionString = ConfigurationManager.ConnectionStrings[Convert.ToString(CompanyID).Trim()].ConnectionString;
            connectionString = Connectionstring;    // Changed by Sachin N. S. on 24/01/2014 for Bug-20211
            DsSelect = new DataSet();
        }

        #region Other Methods
        public void Select()
        {
            try
            {
                DataSet m_DsSelect = new DataSet();
                m_DsSelect = cls_Sqlhelper.ExecuteDataset(connectionString, CommandType.Text, "Select rtrim(autoId) as AutoId,rtrim(Id) as ID,rtrim([to]) as [TO],rtrim(cc) as CC," +
                                            " rtrim(bcc) as BCC,rtrim([subject]) as [SUBJECT],rtrim([filename]) as [FILENAME],rtrim(remarks) as REMARKS,rtrim(filepath) as FILEPATH " +
                                            " From eMailLog Where id='" + Id.ToString().Trim() + "'");
                DsSelect = m_DsSelect;
                m_DsSelect.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        #endregion
    }
}
