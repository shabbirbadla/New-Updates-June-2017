using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
//using CrystalDecisions.Shared;

namespace eMailClient.RPT
{
    public class cls_Gen_Ent_Report
    {
        private string dskFileName;

        public string DskFileName
        {
            get { return dskFileName; }
            set { dskFileName = value; }
        }

        private string dskFilePrefixName;
        public string DskFilePrefixName
        {
            get { return dskFilePrefixName; }
            set { dskFilePrefixName = value; }
        }

        private string reportPath;

        public string ReportPath
        {
            get { return reportPath; }
            set { reportPath = value; }
        }

        private string sqlSrvName;

        public string SqlSrvName
        {
            get { return sqlSrvName; }
            set { sqlSrvName = value; }
        }
        private string sqlUsrName;

        public string SqlUsrName
        {
            get { return sqlUsrName; }
            set { sqlUsrName = value; }
        }
        private string sqlPasswd;

        public string SqlPasswd
        {
            get { return sqlPasswd; }
            set { sqlPasswd = value; }
        }

        private DataSet dsReportSource;
        public DataSet DsReportSource
        {
            get { return dsReportSource; }
            set { dsReportSource = value; }
        }

        private string userID;
        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        private string sqlstring;
        public string Sqlstring
        {
            get { return sqlstring; }
            set { sqlstring = value; }
        }

        private string sqltype;
        public string Sqltype
        {
            get { return sqltype; }
            set { sqltype = value; }
        }

        private SqlParameter[] sqlParam;
        public SqlParameter[] SqlParam
        {
            get { return sqlParam; }
            set { sqlParam = value; }
        }

        private string reportExportType;

        public string ReportExportType
        {
            get { return reportExportType; }
            set { reportExportType = value; }
        }

        private Encoding encodingCSV;

        public Encoding EncodingCSV
        {
            get { return encodingCSV; }
            set { encodingCSV = value; }
        }

        private string separator;
        public string Separator
        {
            get { return separator; }
            set { separator = value; }
        }

        private bool firstRowColumnNames;
        public bool FirstRowColumnNames
        {
            get { return firstRowColumnNames; }
            set { firstRowColumnNames = value; }
        }

        private Int32 companyID;

        public Int32 CompanyID
        {
            get { return companyID; }
            set { companyID = value; }
        }

      //Pankaj Start
        private bool isdigsign;
        public bool Isdigsign
        {
            get { return isdigsign; }
            set { isdigsign =value; }
        }

        private string dept;
        public string Dept
        {
            get { return dept; }
            set { dept = value; }
        }

        private string cate;
        public string Cate
        {
            get { return cate; }
            set { cate = value; }
        }

        private string inv_sr;
        public string Inv_sr
        {
            get { return inv_sr; }
            set { inv_sr = value; }
        }

        private string entry_ty;
        public string Entry_ty
        {
            get { return entry_ty; }
            set { entry_ty = value; }
        }

        private DateTime inv_dt;
        public DateTime Inv_dt
        {
            get { return inv_dt; }
            set { inv_dt = value; }
        }
        //Pankaj End
    }
}
