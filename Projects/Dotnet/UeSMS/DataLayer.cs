using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace UeSMS
{
    class DataLayer
    {
        private SqlConnection Conn;
        string Server;
        string Database;
        string UID;
        string PWD;
        string ConStr;
        char[] ch = { '\"' };
        public DataTable SmsStore;
        SqlConnection  GetConnection()
        {
            if (Conn == null)
                Conn = new SqlConnection(GetConStr());
            OpenConn();
            return Conn;
        }
        void OpenConn()
        {
            if (Conn.State != ConnectionState.Open)
                Conn.Open();
        }
        string GetConStr()
        {
            if (ConStr == null)
                ConStr = "Server=" + Server + ";Database=" + Database + ";user=" + UID + ";password=" + PWD;
            return ConStr;
        }
        public DataLayer(string ServerName, string UserID, string Password, string DatabaseName)
        {
            Server = ServerName;
            Database = DatabaseName;
            UID = UserID;
            PWD = Password;
            GetConnection();
        }
        public void GetSMSs()
        {
            DataSet ds = new DataSet();
            //Conn.Open();
            SqlDataAdapter da = new SqlDataAdapter("Select * from smsstore where smsStatus=0", GetConnection());
            da.Fill(ds);
            SmsStore = ds.Tables[0];
        }
        // Added by Sachin N. S. on 29/09/2014 for Bug-22077 -- Start
        public void GetSMSs(string _smsId)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter("Select * from smsstore where smsStatus=0 and sms_id=" + _smsId, GetConnection());
            da.Fill(ds);
            SmsStore = ds.Tables[0];
        }
        // Added by Sachin N. S. on 29/09/2014 for Bug-22077 -- End

        public void UpdateSendStatus(string Status,string SmsID)
        {

            if (Status.Substring(Status.LastIndexOf(':') + 1).ToUpper().Trim() == "STOP")
            {
                return;
            }
            string[] lsStr = Status.Split(ch);
            if (lsStr.Length > 1)
                updateGUID(lsStr[1], SmsID);
            else
                updateGUID(lsStr[0], SmsID);
        }
        void updateGUID(string Guid, string smsID)
        {
            SqlCommand cmd = new SqlCommand("update smsstore set smsguid='" + Guid.Trim() + "',smsstatus=1 where sms_ID=" + smsID, GetConnection());
            cmd.ExecuteNonQuery();
        }
        public void DeleteUnidentified(string smsID)
        {
            SqlCommand cmd = new SqlCommand("delete smsstore where sms_ID=" + smsID, GetConnection());
            cmd.ExecuteNonQuery();
        }

    }
}
