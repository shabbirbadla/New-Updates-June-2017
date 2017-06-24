using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.IO;
namespace cTipsSqlConn
{
    public class SlqDatacon
    {
        private string vdbname;
        private string vcomconstring = " ";
        
        private Boolean vInsert=false ;
        private string vInsertFldInclude=" ";
        private string vInsertFldExclude = " ";
        private string vInsertWhnCon = " ";
        private string vSrvuserpassword= " ";
        private string vSrvname = " ";
        private string vSrvusername= " ";

        private Boolean vEdit = false;
        private string vEditFldInclude = " ";
        private string vEditFldExclude = " ";
        private string vEditWhnCon = " ";

        private string vDeleteWhnCon = " ";

        public void mthGetConn(ref SqlConnection vconn)
        {
            this.mthSetCon(ref vconn);
        }
        private void mthSetCon(ref SqlConnection vconn)
        {
            if (vcomconstring == " ")
            {
                this.genconnstr();
            }
            vconn = new SqlConnection(vcomconstring + ";Connect Timeout=300;database=" + this.pdbname.ToString());
        }
        //public void Selectcommand(SqlConnection conn, Boolean conClose, SqlTransaction trans,string selectstring, ref DataSet DsStDataCon, string tblnm, out Int64 vRowCount)
        public void Selectcommand(SqlConnection conn, Boolean conClose, SqlTransaction trans, string selectstring, ref DataView sdvw,  out Int64 vRowCount)
        {
            DataSet DsStDataCon = new DataSet();
            string tblnm = "tbl1";
            vRowCount = 0;
            try
            {
                if (conn == null)
                {
                    this.mthGetConn(ref conn);
                }
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                SqlDataAdapter da = new SqlDataAdapter(selectstring, conn);
                da.Fill(DsStDataCon, tblnm);
                sdvw = DsStDataCon.Tables[tblnm].DefaultView;

            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw e;
            }
            finally
            {
                if ((conn.State == ConnectionState.Open) && (conClose==true))
                {
                    conn.Close();
                }
            }
        }
        public void Executecommand(SqlConnection conn, Boolean conClose,SqlTransaction trans,string ExString, Int64 RowCount)
        {
            try
            {
                if (conn == null)
                {
                    this.mthGetConn(ref conn);
                }
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                SqlCommand ExCommand = new SqlCommand();
                ExCommand.Connection = conn;
                ExCommand.CommandText = ExString;
                ExCommand.CommandType = CommandType.Text;
                ExCommand.ExecuteNonQuery();

            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw e;
            }
            finally
            {
                if ((conn.State == ConnectionState.Open) && (conClose==true))
                {
                    conn.Close();
                }
            }

        }
        //public void InsertCommand(string Tbl_Nm,ref DataTable Tbl, Boolean Conn_Close, string Flds_Exclude, string Flds_Include, string Trans_Name)


        //public void UpdateCommand(SqlConnection conn, Boolean conClose, SqlTransaction trans,string TargetTbl, ref DataSet ds, string DtTbl, string PrimaryKeys)//,string tmptblnm,
        public void UpdateCommand(SqlConnection conn, Boolean conClose, SqlTransaction trans, string TargetTbl, ref DataView udvw, string PrimaryKeys)
        {
            
    
        }
        //private void mthGenUpdateCommand(string comType, string TargetTbl, ref DataSet tds, string tDtTbl, string tPrimaryKeys, string tFldInclude, string tFldExclude, ref SqlCommand updCommand)
        private void mthGenUpdateCommand(string comType, string TargetTbl, ref DataView udvw, string tPrimaryKeys, string tFldInclude, string tFldExclude, ref SqlCommand updCommand)
        {
    

        }
        private void mthaddPara(SqlCommand updcmd, ref DataView  pdvwupd, string fldnm, out Boolean vretval, ref DataView pdvw)
        {
            vretval = false;
            
        }
        private void genconnstr()
        {
            string l;
            string dsrvname = " ";
            string dsrvusername = " ";
            string dsrvuserpassword = " ";
            if (this.pdbname != null && this.pSrvname !=null  && this.pSrvusername != null && this.pSrvuserpassword!=null)
            {
                dsrvname = this.pSrvname;
                dsrvusername = this.pSrvusername;
                dsrvuserpassword = this.pSrvuserpassword;
            }
            else
            {
                FileStream fs = new FileStream("visudyog.ini", FileMode.Open, FileAccess.Read);
                StreamReader dr = new StreamReader(fs);
                dr.BaseStream.Seek(0, SeekOrigin.Begin);
                while (dr.Peek() > -1)
                {
                    l = dr.ReadLine().Trim();
                    if (l.ToUpper().Contains("DATASERVERNAME="))
                    {
                        dsrvname = l.Substring(l.IndexOf("DATASERVERNAME=") + ("DATASERVERNAME=").Length + 1);
                    }
                    if (l.ToUpper().Contains("DATASERVERUSERNAME="))
                    {
                        dsrvusername = l.Substring(l.IndexOf("DATASERVERUSERNAME=") + ("DATASERVERUSERNAME=").Length + 1);
                    }
                    if (l.ToUpper().Contains("DATASERVERPASSWORD="))
                    {
                        dsrvuserpassword = l.Substring(l.IndexOf("DATASERVERPASSWORD=") + ("DATASERVERPASSWORD=").Length + 1);
                    }
                }
                dr.Close();
            }
            vcomconstring = "server=" + dsrvname.Trim() + ";uid=" + dsrvusername.Trim() + ";pwd=" + dsrvuserpassword.Trim();//+ ";database=" + "vudyog";

        }
        //public string pcomconstring
        //{
        //    set
        //    {
        //        vcomconstringt = value;
        //    }
        //}
        public string pdbname
        {
            get
            {
                return vdbname;
            }
            set
            {
                vdbname = value;
            }
        }
        public Boolean pInsert
        {
            get
            {
                return vInsert;
            }
            set
            {
                vInsert = value;
            }
        }
        public string pInsertFldInclude
        {
            get
            {
                return vInsertFldInclude;
            }
            set
            {
                vInsertFldInclude = value;
            }
        }
        public string pInsertFldExclude
        {
            get
            {
                return vInsertFldExclude;
            }
            set
            {
                vInsertFldExclude = value;
            }
        }
        public string pInsertWhnCon
        {
            get
            {
                return vInsertWhnCon;
            }
            set
            {
                vInsertWhnCon = value;
            }
        }


        public Boolean pEdit
        {
            get
            {
                return vEdit;
            }
            set
            {
                vEdit = value;
            }
        }
        public string pEditFldInclude
        {
            get
            {
                return vEditFldInclude;
            }
            set
            {
                vEditFldInclude = value;
            }
        }
        public string pEditFldExclude
        {
            get
            {
                return vEditFldExclude;
            }
            set
            {
                vEditFldExclude = value;
            }
        }
        public string pEditWhnCon
        {
            get
            {
                return vEditWhnCon;
            }
            set
            {
                vEditWhnCon = value;
            }
        }
        public string pDeleteWhnCon
        {
            get
            {
                return vDeleteWhnCon;
            }
            set
            {
                vDeleteWhnCon = value;
            }
        }

        public string pSrvuserpassword
        {
            get
            {
                return vSrvuserpassword;
            }
            set
            {
                vSrvuserpassword = value;
            }
        }
        public string pSrvname
        {
            get
            {
                return vSrvname;
            }
            set
            {
                vSrvname = value;
            }
        }
        public string pSrvusername
        {
            get
            {
                return vSrvusername;
            }
            set
            {
                vSrvusername = value;
            }
        }

    }//public class SlqDatacon

}//namespace SqlConn
