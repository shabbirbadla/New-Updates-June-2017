using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace ueProductUpgrade
{
    class UtilityUpdate
    {
        SqlConnection conn;
        string connStr = string.Empty;
        public UtilityUpdate(string ConnStr)
        {
            connStr = ConnStr;
        }
        public void ExecuteSystemUpdates(Stream UpdatesStream)
        {
            SqlCommand lcmd;
            DataSet lds = new DataSet();
            string sqlStr = string.Empty;
            lds.ReadXml(UpdatesStream, XmlReadMode.Auto);
            DataView ldv = lds.Tables[0].DefaultView;
            ldv.Sort = "UpdatesOrder";
            ldv.RowFilter = "Database='VUDYOG' and IsActive=1";
            if (ldv.Count > 0)
            {
                for (int i = 0; i < ldv.Count; i++)
                {
                    if ((int)ldv[i]["ChkExists"] == 1)
                    {
                        this.ConnOpen();
                        sqlStr = "Select [Name] From VUdyog..SysObjects Where xType='" + ldv[i]["UpdatesType"].ToString().Trim() + "' and [Name]='" + ldv[i]["UpdatesName"].ToString().Trim() + "'";
                        lcmd = new SqlCommand(sqlStr, conn);
                        string objEx = (string)lcmd.ExecuteScalar();
                        if (objEx == null)
                        {
                            sqlStr = ldv[i]["SqlQuery"].ToString().Trim();
                            lcmd = new SqlCommand(sqlStr, conn);
                            lcmd.ExecuteNonQuery();
                        }
                        this.ConnClose();
                    }
                    else
                    {
                        this.ConnOpen();
                        sqlStr = ldv[i]["SqlQuery"].ToString().Trim();
                        lcmd = new SqlCommand(sqlStr, conn);
                        lcmd.ExecuteNonQuery();
                        this.ConnClose();
                    }
                }
            }
            lds = null;
        }
        public void ExecuteCompanyUpdates(string companyDB, Stream UpdatesStream)
        {
            SqlCommand lcmd;
            DataSet lds = new DataSet();
            string sqlStr = string.Empty;
            lds.ReadXml(UpdatesStream, XmlReadMode.Auto);
            DataView ldv = lds.Tables[0].DefaultView;

            ldv.Sort = "UpdatesOrder";
            ldv.RowFilter = "Database='Company' and IsActive=1";
            connStr = connStr.Replace("VUDYOG", companyDB);
            if (ldv.Count > 0)
            {
                for (int i = 0; i < ldv.Count; i++)
                {
                    if ((int)ldv[i]["ChkExists"] == 1)
                    {
                        sqlStr = "Select [Name] From " + companyDB + "..SysObjects Where xType='" + ldv[i]["UpdatesType"].ToString().Trim() + "' and [Name]='" + ldv[i]["UpdatesName"].ToString().Trim() + "'";
                        lcmd = new SqlCommand(sqlStr, conn);
                        this.ConnOpen();
                        string objEx = (string)lcmd.ExecuteScalar();
                        if (objEx == null)
                        {
                            sqlStr = ldv[i]["SqlQuery"].ToString().Trim();
                            lcmd = new SqlCommand(sqlStr, conn);
                            lcmd.ExecuteNonQuery();
                        }
                        this.ConnClose();
                    }
                    else
                    {
                        sqlStr = ldv[i]["SqlQuery"].ToString().Trim();
                        lcmd = new SqlCommand(sqlStr, conn);
                        this.ConnOpen();
                        lcmd.ExecuteNonQuery();
                        this.ConnClose();
                    }
                }
            }
            lds = null;
        }
        private void ConnOpen()
        {
            if (conn != null)
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
            }
            else
            {
                conn = new SqlConnection(connStr);
                conn.Open();
            }

        }
        private void ConnClose()
        {
            if (conn != null)
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
    }
}
