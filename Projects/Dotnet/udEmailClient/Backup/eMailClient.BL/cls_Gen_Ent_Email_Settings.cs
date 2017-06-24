using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace eMailClient.BLL
{
    public class cls_Gen_Ent_Email_Settings
    {
        public cls_Gen_Ent_Email_Settings()
        {
        }

        #region Properties
        private DataSet dsSearch;

        public DataSet DsSearch
        {
            get { return dsSearch; }
            set { dsSearch = value; }
        }

        private string yourname;

        public string Yourname
        {
            get { return yourname; }
            set { yourname = value; }
        }

        private string username;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private string host;

        public string Host
        {
            get { return host; }
            set { host = value; }
        }

        private int port;

        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        private bool enablessl;

        public bool Enablessl
        {
            get { return enablessl; }
            set { enablessl = value; }
        }
        #endregion
    }
}
