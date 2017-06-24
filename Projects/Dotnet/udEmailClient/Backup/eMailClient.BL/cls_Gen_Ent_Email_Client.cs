using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace eMailClient.BLL
{
    public class cls_Gen_Ent_Email_Client
    {
        public cls_Gen_Ent_Email_Client()
        { }

        #region Properties
        private DataSet dsSearch;
        public DataSet DsSearch
        {
            get { return dsSearch; }
            set { dsSearch = value; }
        }

        private List<string> lstTranTyp;
        public List<string> LstTranTyp
        {
            get { return lstTranTyp; }
            set { lstTranTyp = value; }
        }

        private string id;
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        private string desc;
        public string Desc
        {
            get { return desc; }
            set { desc = value; }
        }

        private string tran_typ;
        public string Tran_typ
        {
            get { return tran_typ; }
            set { tran_typ = value; }
        }

        private bool hasattachment;
        public bool Hasattachment
        {
            get { return hasattachment; }
            set { hasattachment = value; }
        }

        private string attachment_typ;
        public string Attachment_typ
        {
            get { return attachment_typ; }
            set { attachment_typ = value; }
        }

        private string rep_nm;
        public string Rep_nm
        {
            get { return rep_nm; }
            set { rep_nm = value; }
        }

        private string exportpath;
        public string Exportpath
        {
            get { return exportpath; }
            set { exportpath = value; }
        }

        private string exportprefixname;
        public string Exportprefixname
        {
            get { return exportprefixname; }
            set { exportprefixname = value; }
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

        private bool removefiles;
        public bool Removefiles
        {
            get { return removefiles; }
            set { removefiles = value; }
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

        private DataSet dsQuery;
        public DataSet DsQuery
        {
            get { return dsQuery; }
            set { dsQuery = value; }
        }

        private string query;
        public string Query
        {
            get { return query; }
            set { query = value; }
        }

        private string reportquery;
        public string Reportquery
        {
            get { return reportquery; }
            set { reportquery = value; }
        }

        private string parameters;
        public string Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }

        private string separator;
        public string Separator
        {
            get { return separator; }
            set { separator = value; }
        }

        private string encoding;
        public string Encoding
        {
            get { return encoding; }
            set { encoding = value; }
        }

        private bool isFirstrow;
        public bool IsFirstrow
        {
            get { return isFirstrow; }
            set { isFirstrow = value; }
        }

        private string reportquerytype;
        public string Reportquerytype
        {
            get { return reportquerytype; }
            set { reportquerytype = value; }
        }

        private Int32 companyID;
        public Int32 CompanyID
        {
            get { return companyID; }
            set { companyID = value; }
        }

        //**** Added by Sachin N. S. on 20/01/2014 for Bug-20211 -- Start ****//
        private string repGroup;
        public string RepGroup
        {
            get { return repGroup; }
            set { repGroup = value; }
        }

        private string repDesc;
        public string RepDesc
        {
            get { return repDesc; }
            set { repDesc = value; }
        }

        private string repRep_Nm;
        public string RepRep_Nm
        {
            get { return repRep_Nm; }
            set { repRep_Nm = value; }
        }

        private string sqlQuery;
        public string SqlQuery
        {
            get { return sqlQuery; }
            set { sqlQuery = value; }
        }

        private string spWhat;
        public string SpWhat
        {
            get { return spWhat; }
            set { spWhat = value; }
        }
        
        private string qTable;
        public string QTable
        {
            get { return qTable; }
            set { qTable = value; }
        }
        //**** Added by Sachin N. S. on 20/01/2014 for Bug-20211 -- End ****//

        //Added By Pankaj B. on 14-04-2015 for Digital Signature Start
        private bool DigitalSign;
        public bool digitalSign
        {
            get { return DigitalSign; }
            set { DigitalSign =value; }
        }
        //Added By Pankaj B. on 14-04-2015 for Digital Signature End


        #endregion
    }
}
