using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace eMailClient.BLL
{
    public class cls_Gen_Ent_Email_LogWriter
    {
        #region Properties
        private DataSet dsSelect;

        public DataSet DsSelect
        {
            get { return dsSelect; }
            set { dsSelect = value; }
        }

        private string id;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        private string to;

        public string To
        {
            get { return to; }
            set { to = value; }
        }

        private string cc;

        public string Cc
        {
            get { return cc; }
            set { cc = value; }
        }

        private string bcc;

        public string Bcc
        {
            get { return bcc; }
            set { bcc = value; }
        }

        private string subject;

        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        private string body;

        public string Body
        {
            get { return body; }
            set { body = value; }
        }

        private string filepath;

        public string Filepath
        {
            get { return filepath; }
            set { filepath = value; }
        }

        private string filename;

        public string Filename
        {
            get { return filename; }
            set { filename = value; }
        }

        private bool removefiles;

        public bool Removefiles
        {
            get { return removefiles; }
            set { removefiles = value; }
        }

        private string status;

        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        private string remarks;

        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }

        private bool emaillogfiles;

        public bool Emaillogfiles
        {
            get { return emaillogfiles; }
            set { emaillogfiles = value; }
        }

        private string logemailid;

        public string Logemailid
        {
            get { return logemailid; }
            set { logemailid = value; }
        }

        private Int32 companyID;

        public Int32 CompanyID
        {
            get { return companyID; }
            set { companyID = value; }
        }
        #endregion
    }
}
