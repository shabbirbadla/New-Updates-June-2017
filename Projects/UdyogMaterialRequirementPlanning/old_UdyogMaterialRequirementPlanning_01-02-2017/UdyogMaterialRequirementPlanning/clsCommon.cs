using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace UdyogMaterialRequirementPlanning
{
    class clsCommon
    {
        private static DateTime _FromDt;
        public static DateTime FromDt
        {
            get { return _FromDt; }
            set { _FromDt = value; }
        }
        private static DateTime _ToDt;
        public static DateTime ToDt
        {
            get { return _ToDt; }
            set { _ToDt = value; }
        }
        private static string _MRPDepOn;
        public static string MRPDepOn
        {
            get { return _MRPDepOn; }
            set { _MRPDepOn = value; }
        }
        private static bool _IsWareAppl;
        public static bool IsWareAppl
        {
            get { return _IsWareAppl; }
            set { _IsWareAppl = value; }
        }
        private static bool _IsWKOrdAppl;
        public static bool IsWKOrdAppl
        {
            get { return _IsWKOrdAppl; }
            set { _IsWKOrdAppl = value; }
        }
        private static string _Warehouse=string.Empty;
        public static string Warehouse
        {
            get { return _Warehouse; }
            set { _Warehouse = value; }
        }
        //private static string _EntryType;
        //public static string EntryType
        //{
        //    get { return _EntryType; }
        //    set { _EntryType = value; }
        //}
        private static string _MRPValidity;
        public static string MRPValidity
        {
            get { return _MRPValidity; }
            set { _MRPValidity = value; }
        }
        //private static string _CodeNm;
        //public static string CodeNm
        //{
        //    get { return _CodeNm; }
        //    set { _CodeNm = value; }
        //}

        private static int _CompId;
        public static int CompId
        {
            get{return _CompId;}
            set{_CompId=value;}
        }
        private static string _DbName;
        public static string DbName
        {
            get{return _DbName;}
            set{_DbName=value;}
        }
        private static string _ServerName;
        public static string ServerName
        {
            get{return _ServerName;}
            set{_ServerName=value;}
        }
        private static string _User;
        public static string User
        {
            get{return _User;}
            set{_User=value;}
        }
        private static string _Password;
        public static string Password
        {
            get{return _Password;}
            set{_Password=value;}
        }

        private static string _ApplText;
        public static string ApplText
        {
            get{return _ApplText;}
            set{_ApplText=value;}
        }
        private static string _ApplName;
        public static string ApplName
        {
            get { return _ApplName; }
            set { _ApplName = value; }
        }

        private static string _ApplID;
        public static string ApplID
        {
          get { return _ApplID; }
          set { _ApplID = value; }
        }

        private static string _ApplCode;
        public static string ApplCode
        {
          get { return _ApplCode; }
          set { _ApplCode = value; }
        }

        private static string _FrmCaption;
        public static string FrmCaption
        {
            get{return _FrmCaption;}
            set{_FrmCaption=value;}
        }

        private static string _FinYear;
        public static string FinYear
        {
            get { return _FinYear; }
            set { _FinYear = value; }
        }

        private static string _AppUserName;
        public static string AppUserName
        {
            get { return _AppUserName; }
            set { _AppUserName = value; }
        }
        private static String _ConnStr;
        public static string ConnStr
        {
            get { return _ConnStr; }
            set { _ConnStr = value; }
        }
        private static string _IconFile;
        public static string IconFile
        {
            get { return _IconFile; }
            set { _IconFile = value; }
        }


        //variables of calling Application          
        public static string pApplCode = string.Empty;
        public static string pApplNm = string.Empty;
        public static string pApplId = string.Empty;
        public static string pApplDesc = string.Empty;

        //variables of this application
        public static string cApplNm = string.Empty;
        public static string cApplId = string.Empty;


        public static void InsertProcessIdRecord()
        {
            string sqlstr = string.Empty;
            sqlstr = " insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values(" +
                        "@pApplCode,@CallDate,@pApplNm,@pApplId,@pApplDesc,@cApplNm,@cApplId,@cApplDesc)";

            SqlConnection con = new SqlConnection(clsCommon.ConnStr);
            SqlCommand cmd = new SqlCommand(sqlstr, con);
            cmd.Parameters.Add(new SqlParameter("@pApplCode", clsCommon.pApplCode));
            cmd.Parameters.Add(new SqlParameter("@CallDate", DateTime.Now.ToString("MM/dd/yyyy")));
            cmd.Parameters.Add(new SqlParameter("@pApplNm", clsCommon.pApplNm));
            cmd.Parameters.Add(new SqlParameter("@pApplId", clsCommon.pApplId));
            cmd.Parameters.Add(new SqlParameter("@pApplDesc", clsCommon.pApplNm));
            cmd.Parameters.Add(new SqlParameter("@cApplNm", clsCommon.cApplNm));
            cmd.Parameters.Add(new SqlParameter("@cApplId", clsCommon.cApplId));
            cmd.Parameters.Add(new SqlParameter("@cApplDesc", clsCommon.ApplName));
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, clsCommon.ApplName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static void DeleteProcessIdRecord()
        {
            string sqlstr = string.Empty;
            sqlstr = " Delete from vudyog..ExtApplLog where pApplNm=@pApplNm and pApplId=@pApplId and cApplNm= @cApplNm and cApplId= @cApplId";
            SqlConnection con = new SqlConnection(clsCommon.ConnStr);
            SqlCommand cmd = new SqlCommand(sqlstr, con);
            cmd.Parameters.Add(new SqlParameter("@pApplNm", clsCommon.pApplNm));
            cmd.Parameters.Add(new SqlParameter("@pApplId", clsCommon.pApplId));
            cmd.Parameters.Add(new SqlParameter("@cApplNm", clsCommon.cApplNm));
            cmd.Parameters.Add(new SqlParameter("@cApplId", clsCommon.cApplId));
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, clsCommon.pApplNm, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }


    public static class StringExtension
    {
        public static string ToTitleCase(this string str)
        {
            var cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            return cultureInfo.TextInfo.ToTitleCase(str.ToLower());
        }
        public static string PadCenter(this string str, int length)
        {
            int spaces = length - str.Length;
            int padLeft = spaces / 2 + str.Length;
            return str.PadLeft(padLeft).PadRight(length);
        }
    }
}
