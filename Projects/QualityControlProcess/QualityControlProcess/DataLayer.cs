using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QualityControlProcess
{
    class DataLayer
    {
        //private static string Conn = "server=udyog3\\vudyogsdk;database=m011112;user id=sa;password=sa@1985";
        private static string Conn = String.Empty;
        public static void CreateConnection(string Server, string Database, string uid, string pwd)
        {
            if (Conn == String.Empty)
                Conn = "server=" + Server + ";database=" + Database + ";user id=" + uid + ";password=" + pwd;
        }
        public static DataSet GetDataSet()
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter("select ITEM_ID,qc_para,[Desc],std_value,Low_Tol,Up_Tol,is_tol_in_percent from QC_Process_Item", Conn);
            da.Fill(ds, "QC_Process_Item");
            return ds;
        }
        public static DataSet GetDataSet(string cmd)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd , Conn);
            da.Fill(ds);
            return ds;
        }
        public static void GetMasterDataSet(DataSet ds, string tbl, string cond)//Cond parameter added By Pankaj On 23/06/2014 for Bug-22827
        {

            SqlDataAdapter da = new SqlDataAdapter("select * from " + tbl + cond, Conn);

            da.Fill(ds, tbl);
        }



        public static DataTable GetDataTable(string SqlCmd)
        {
            DataTable dt=new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(SqlCmd, Conn);
            da.Fill(dt);
            return dt;
        }
        public static void GetItemDataSet(DataSet ds, string tbl, string cond)
        {
            
            SqlDataAdapter da = new SqlDataAdapter("select qc_para,[Desc],std_value,Low_Tol,Up_Tol,is_tol_in_percent,DEF_VAL from " + tbl + " Where " + cond, Conn);
            if (ds.Tables.Contains(tbl))
                ds.Tables.Remove(tbl);
            da.Fill(ds, tbl);
        }
        public static DataSet GetParameters(DataSet ds)
        {
            //DataSet ds = new DataSet();
            string sqlstr="select qc_para,Para_Desc,Datatype,id from QC_PARAMETER_MASTER ";
            sqlstr += " union ";
            sqlstr += "select space(1),space(1),space(1),space(1)";
            //SqlDataAdapter da = new SqlDataAdapter("select qc_para,Para_Desc,Datatype,id from QC_PARAMETER_MASTER", Conn);
            SqlDataAdapter da = new SqlDataAdapter(sqlstr, Conn);
            da.Fill(ds, "QC_Parameter_Master");
            return ds;
        }
        public static int InsertInMaster(string cmd)
        {
            SqlConnection cn = new SqlConnection(Conn);
            cn.Open();
            SqlCommand SqlCmd = new SqlCommand(cmd, cn);
            SqlCmd.ExecuteNonQuery();
            SqlCmd = new SqlCommand("Select max(QC_PROCESS_ID) id from QC_PROCESS_MASTER", cn);
            SqlDataReader dr = SqlCmd.ExecuteReader();
            if (dr.Read())
                return Convert.ToInt32(dr[0]);
            return 1;
        }
        public static void Save(DataTable dt, int Id)
        {
            string ColName = String.Empty;
            string ColValue = String.Empty;
            DataColumn colx = new DataColumn();
            colx.ColumnName = "DataType";
            colx.DataType = System.Type.GetType("System.String");
            dt.Columns.Add(colx);
            DataSet xt = new DataSet();
            xt = GetParameters(xt);
            
            foreach (DataRow rw in dt.Rows)
            {
                foreach (DataRow srw in xt.Tables[0].Rows)
                {
                    if (rw["Qc_Para"].ToString() == srw["Qc_Para"].ToString())
                    {
                        rw["DataType"] = srw["Datatype"];
                        break;
                    }                    
                }
                if (String.IsNullOrEmpty(rw["STD_VALUE"].ToString()))
                //{
                    //MessageBox.Show("Please enter standerd Value !");

                    break;
                    //return;
                //}
                ColName = "QC_PROCESS_ID,";
                ColValue = Id.ToString() + ",";
                foreach (DataColumn col in dt.Columns)
                {
                    switch (col.DataType.Name)
                    {
                        case "String":
                            ColValue = ColValue + "'" + rw[col].ToString() + "',";
                            break;
                        case "Decimal":
                            ColValue = ColValue + (String.IsNullOrEmpty(rw[col].ToString()) ? "null" : rw[col].ToString()) + ",";
                            break;
                        case "Boolean":
                            ColValue = ColValue + (rw[col].ToString() == "True" ? 1 : 0) + ",";
                            break;
                    }
                    
                    ColName = ColName + "[" + col.ColumnName + "],";

                }

                if (!String.IsNullOrEmpty(ColName))
                    ColName = ColName.Remove(ColName.Length - 1, 1);
                if (!String.IsNullOrEmpty(ColValue))
                    ColValue = ColValue.Remove(ColValue.Length - 1, 1);
                string cmd = "insert into QC_PROCESS_ITEM (" + ColName + ") Values (" + ColValue + ")";
                SqlConnection cn = new SqlConnection(Conn);
                cn.Open();
                SqlCommand SqlCmd = new SqlCommand(cmd, cn);
                SqlCmd.ExecuteNonQuery();
            }
            //return ColName + ":" + ColValue;
        }
        public static void RemoveItem(string cmd)
        {
            //string cmd = "delete from QC_PROCESS_ITEM where QC_PROCESS_ID=id";
            SqlConnection cn = new SqlConnection(Conn);
            cn.Open();
            SqlCommand SqlCmd = new SqlCommand(cmd, cn);
            SqlCmd.ExecuteNonQuery();
        }
        public static void ExecuteSQLnonQuery(string SQL)
        {
            SqlConnection cn = new SqlConnection(Conn);
            cn.Open();
            SqlCommand SqlCmd = new SqlCommand(SQL, cn);
            SqlCmd.ExecuteNonQuery();
        }
        public static void Update(DataTable dt, int Id)
        {

            string ColName = String.Empty;
            string ColValue = String.Empty;
            string cmd = "delete from QC_PROCESS_ITEM where QC_PROCESS_ID=" + Id;
            RemoveItem(cmd);
            DataColumn colx = new DataColumn();
            colx.ColumnName = "DataType";
            colx.DataType = System.Type.GetType("System.String");
            dt.Columns.Add(colx);
            DataSet xt = new DataSet();
            xt = GetParameters(xt);

            foreach (DataRow rw in dt.Rows)
            {
                foreach (DataRow srw in xt.Tables[0].Rows)
                {
                    if (rw["Qc_Para"].ToString() == srw["Qc_Para"].ToString())
                    {
                        rw["DataType"] = srw["Datatype"];
                        break;
                    }
                }
                if (String.IsNullOrEmpty(rw["STD_VALUE"].ToString()))
                {
                    dt.Rows.Remove(rw);
                    break;
                }
                ColName = "QC_PROCESS_ID,";
                ColValue = Id.ToString() + ",";
                foreach (DataColumn col in dt.Columns)
                {
                    
                    switch (col.DataType.Name)
                    {
                        case "String":
                            ColValue = ColValue + "'" + rw[col].ToString() + "',";
                            break;
                        case "Decimal":
                            ColValue = ColValue + (String.IsNullOrEmpty(rw[col].ToString()) ? "null" : rw[col].ToString()) + ",";
                            break;
                        case "Boolean":
                            ColValue = ColValue + (rw[col].ToString() == "True" ? 1 : 0) + ",";
                            break;
                    }

                    ColName = ColName + "[" + col.ColumnName + "],";

                }

                if (!String.IsNullOrEmpty(ColName))
                    ColName = ColName.Remove(ColName.Length - 1, 1);
                if (!String.IsNullOrEmpty(ColValue))
                    ColValue = ColValue.Remove(ColValue.Length - 1, 1);
 
                cmd = "insert into QC_PROCESS_ITEM (" + ColName + ") Values (" + ColValue + ")";
                SqlConnection cn = new SqlConnection(Conn);
                cn.Open();
                SqlCommand SqlCmd = new SqlCommand(cmd, cn);
                SqlCmd.ExecuteNonQuery();
            }
            //return ColName + ":" + ColValue;
        }
    }
}
