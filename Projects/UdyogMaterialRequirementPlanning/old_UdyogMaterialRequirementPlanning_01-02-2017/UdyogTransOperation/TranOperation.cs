using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using UdyogDataOperation;


namespace UdyogTranOperation
{
    public class TranOperation
    {
        public string GenerateInvNo(string ventryType, string vInvoiceSeries, string vInvoiceNo, DateTime VentDate, string voldInvoiceSeries, string voldInvoiceNo, int vinv_size, SqlConnection oSqlConObj, string Entry_Tbl, string vcDbName, DateTime vdSta_Dt, DateTime vdEnd_Dt)
        {
            string ReturnInvNo = string.Empty;
            string SeriesType = string.Empty;
            string prefix = string.Empty;
            string suffix = string.Empty;
            string monthFormat = string.Empty;
            string _vInvoiceEs = voldInvoiceSeries;
            string _vInvoiceEn = voldInvoiceNo;
            string v_i_middle = string.Empty;
            string cond = string.Empty;

            SqlTransaction SqlTran = null;
            int rowsAffected = 0;
            SqlCommand cmd = new SqlCommand("Select top 1 * From Series Where Inv_sr=@Series", oSqlConObj);
            cmd.Parameters.Add(new SqlParameter("@Series", vInvoiceSeries));
            DataTable Series = new DataTable("Series_vw");
            //cmd.Transaction = SqlTran;
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(Series);

            if (oSqlConObj.State == ConnectionState.Closed)
                oSqlConObj.Open();

            if (Series.Rows.Count > 0)
            {
                SeriesType = (Convert.ToString(Series.Rows[0]["s_Type"])).Trim();
                prefix = (Convert.ToString(Series.Rows[0]["i_prefix"]).Replace('"', ' ').Replace("'", "").Trim()).Trim();
                suffix = (Convert.ToString(Series.Rows[0]["i_suffix"]).Replace('"', ' ').Replace("'", "")).Trim();
                monthFormat = (Convert.ToString(Series.Rows[0]["mnthformat"])).Trim();

                switch (SeriesType)
                {
                    case "DAYWISE":
                        v_i_middle = VentDate.ToString("ddMMyy");
                        break;
                    case "MONTHWISE":
                        string SQL = "SELECT MnthFrmt FROM monthformat Where MnthFrmt ='" + monthFormat + "'";
                        cmd = new SqlCommand(SQL, oSqlConObj);
                        SqlDataReader r;
                        if (oSqlConObj.State == ConnectionState.Closed)
                            oSqlConObj.Open();
                        r = cmd.ExecuteReader();

                        // Iterate over the results.

                        while (r.Read())
                        {
                            v_i_middle = Convert.ToString(r["MnthFrmt"]);
                        }
                        r.Close();
                        if (v_i_middle == string.Empty)
                            v_i_middle = VentDate.ToString("MMdd");
                        break;
                    default:
                        break;
                }
                if (vInvoiceNo.Length > 0)
                {
                    vInvoiceNo = vInvoiceNo.Substring(prefix.Length + v_i_middle.Length);
                    vInvoiceNo = vInvoiceNo.Substring(0, vInvoiceNo.Length - suffix.Length);
                }

            }
            vInvoiceNo = vInvoiceNo.Trim() == "" ? "0" : Convert.ToInt32(vInvoiceNo).ToString();
            vInvoiceNo = (vInvoiceNo.Trim() == "0" ? "1" : vInvoiceNo);

            if (SeriesType == "MONTHWISE")
            {
                VentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            }

            string vctrYear = string.Empty;
            if (VentDate >= vdSta_Dt && VentDate <= vdEnd_Dt)
            {
                vctrYear = vdSta_Dt.Year.ToString() + "-" + vdEnd_Dt.Year.ToString();
            }
            else
            {
                List<int> endDate = new List<int>();
                for (int i = 1; i <= vdEnd_Dt.Month; i++)
                {
                    endDate.Add(i);
                }

                List<int> startDate = new List<int>();
                for (int i = vdSta_Dt.Month; i <= 12; i++)
                {
                    startDate.Add(i);
                }
                if (endDate.Contains(VentDate.Month))
                    vctrYear = (VentDate.Year - 1).ToString().Trim() + "-" + (VentDate.Year).ToString().Trim();
                else
                    if (startDate.Contains(VentDate.Month))
                        vctrYear = (VentDate.Year).ToString().Trim() + "-" + (VentDate.Year + 1).ToString().Trim();
            }

            DataTable GenInv = new DataTable("Gen_Inv_vw");
            DataTable GenMiss = new DataTable("Gen_Miss_vw");
            try
            {
                SqlTran = oSqlConObj.BeginTransaction();
                cmd = new SqlCommand(" Select * from Gen_inv with (NOLOCK) where 1=0 ", oSqlConObj);
                cmd.Transaction = SqlTran;
                da.SelectCommand = cmd;
                da.Fill(GenInv);

                cmd.CommandText = " Select * from Gen_miss with (NOLOCK) where 1=0 ";
                da.Fill(GenMiss);

                DataRow InvRow = GenInv.NewRow();
                InvRow["Entry_ty"] = ventryType;
                InvRow["Inv_dt"] = VentDate;
                InvRow["Inv_sr"] = vInvoiceSeries;
                InvRow["Inv_no"] = vInvoiceNo;
                InvRow["L_yn"] = vctrYear;
                GenInv.Rows.Add(InvRow);


                DataRow MissRow = GenMiss.NewRow();
                MissRow["Entry_ty"] = ventryType;
                MissRow["Inv_dt"] = VentDate;
                MissRow["Inv_sr"] = vInvoiceSeries;
                MissRow["Inv_no"] = vInvoiceNo;
                MissRow["L_yn"] = vctrYear;
                MissRow["Flag"] = "Y";
                GenMiss.Rows.Add(MissRow);

                bool IsRollBack = true;
                //DataTable TmpTbl = new DataTable();
                DataTable TmpTbl;

                string sqlStr = string.Empty;
                while (true)
                {
                    TmpTbl = new DataTable();
                    //TmpTbl = null;
                    //if (TmpTbl != null)
                    //{
                    //    //TmpTbl.Rows.Clear();
                    //    TmpTbl.Clear();
                    //    TmpTbl = null;
                    //}
                    switch (SeriesType)
                    {
                        case "DAYWISE":
                            sqlStr = "Select Top 1 Inv_no from Gen_inv with (TABLOCKX) where Entry_ty =@Entry_ty And Inv_sr = @Series "
                                    + " And Inv_dt = @Inv_dt";
                            cmd = new SqlCommand(sqlStr, oSqlConObj);
                            cmd.Parameters.Clear();
                            cmd.Parameters.Add(new SqlParameter("@Entry_ty", ventryType));
                            cmd.Parameters.Add(new SqlParameter("@Series", vInvoiceSeries));
                            cmd.Parameters.Add(new SqlParameter("@Inv_dt", VentDate.ToString("MM/dd/yyyy")));

                            break;
                        case "MONTHWISE":
                            sqlStr = "  Select Top 1 Inv_no from Gen_inv with (TABLOCKX) where Entry_ty = @Entry_ty "
                                    + " And Inv_sr = @Series And MONTH(Inv_dt) = @Inv_dt_m And Year(Inv_dt) = @Inv_dt_y ";
                            cmd = new SqlCommand(sqlStr, oSqlConObj);
                            cmd.Parameters.Clear();
                            cmd.Parameters.Add(new SqlParameter("@Entry_ty", ventryType));
                            cmd.Parameters.Add(new SqlParameter("@Series", vInvoiceSeries));
                            cmd.Parameters.Add(new SqlParameter("@Inv_dt_m", VentDate.Month));
                            cmd.Parameters.Add(new SqlParameter("@Inv_dt_y", VentDate.Year));
                            break;
                        default:
                            sqlStr = "  Select Top 1 Inv_no from Gen_inv with (TABLOCKX) where Entry_ty =@Entry_ty  "
                                        + " And Inv_sr = @Series And L_yn = @L_yn ";
                            cmd = new SqlCommand(sqlStr, oSqlConObj);
                            cmd.Parameters.Add(new SqlParameter("@Entry_ty", ventryType));
                            cmd.Parameters.Add(new SqlParameter("@Series", vInvoiceSeries));
                            cmd.Parameters.Add(new SqlParameter("@L_yn", vctrYear));
                            break;
                    }
                    cmd.Transaction = SqlTran;
                    da = new SqlDataAdapter(cmd);
                    da.Fill(TmpTbl);

                    DBOperation op = new DBOperation();
                    if (TmpTbl.Rows.Count <= 0)
                    {
                        cmd = new SqlCommand();
                        cmd.Transaction = SqlTran;
                        cmd = op.GenerateInsertString(cmd, GenInv.Rows[0], "Gen_Inv", null, null);
                        cmd.Connection = oSqlConObj;
                        rowsAffected = cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        if (Convert.ToInt32(TmpTbl.Rows[0]["Inv_no"]) < Convert.ToInt32(GenInv.Rows[0]["Inv_no"]))
                        {
                            cmd = new SqlCommand();
                            cmd.Connection = oSqlConObj;
                            cmd.Parameters.Clear();
                            switch (SeriesType)
                            {
                                case "DAYWISE":
                                    cond = " Entry_ty = @Entry_ty And Inv_sr = @Series And Inv_dt = @Inv_dt";
                                    cmd = op.GenerateUpdateString(cmd, GenInv.Rows[0], "Gen_Inv", null, new string[] { "Inv_no" }, cond, null);
                                    //                                    cmd.Parameters.Clear();
                                    cmd.Parameters.Add(new SqlParameter("@Entry_ty", ventryType));
                                    cmd.Parameters.Add(new SqlParameter("@Series", vInvoiceSeries));
                                    cmd.Parameters.Add(new SqlParameter("@Inv_dt", VentDate.ToString("MM/dd/yyyy")));
                                    break;
                                case "MONTHWISE":
                                    cond = " Entry_ty = @Entry_ty  And Inv_sr = @Series And MONTH(Inv_dt) = @Inv_dt_m And Year(Inv_dt) = @Inv_dt_y ";
                                    cmd = op.GenerateUpdateString(cmd, GenInv.Rows[0], "Gen_Inv", null, new string[] { "Inv_no" }, cond, null);
                                    cmd.Parameters.Add(new SqlParameter("@Entry_ty", ventryType));
                                    cmd.Parameters.Add(new SqlParameter("@Series", vInvoiceSeries));
                                    cmd.Parameters.Add(new SqlParameter("@Inv_dt_m", VentDate.Month));
                                    cmd.Parameters.Add(new SqlParameter("@Inv_dt_y", VentDate.Year));
                                    break;
                                default:
                                    cond = " Entry_ty =@Entry_ty  And Inv_sr = @Series And L_yn = @L_yn ";
                                    cmd = op.GenerateUpdateString(cmd, GenInv.Rows[0], "Gen_Inv", null, new string[] { "Inv_no" }, cond, null);
                                    cmd.Parameters.Add(new SqlParameter("@Entry_ty", ventryType));
                                    cmd.Parameters.Add(new SqlParameter("@Series", vInvoiceSeries));
                                    cmd.Parameters.Add(new SqlParameter("@L_yn", vctrYear));
                                    break;
                            }
                            cmd.Transaction = SqlTran;
                            rowsAffected = cmd.ExecuteNonQuery();
                        }
                    }
                    if (rowsAffected > 0)
                    {
                        switch (SeriesType)
                        {
                            case "DAYWISE":
                                sqlStr = "Select Top 1 Flag from Gen_Miss Where Entry_ty =@Entry_ty And Inv_sr = @Series And Inv_no=@Inv_no And Inv_dt = @Inv_dt";
                                cmd = new SqlCommand(sqlStr, oSqlConObj);
                                cmd.Parameters.Clear();
                                cmd.Parameters.Add(new SqlParameter("@Entry_ty", ventryType));
                                cmd.Parameters.Add(new SqlParameter("@Series", vInvoiceSeries));
                                cmd.Parameters.Add(new SqlParameter("@Inv_no", (int)Convert.ToDouble(GenMiss.Rows[0]["Inv_no"])));
                                cmd.Parameters.Add(new SqlParameter("@Inv_dt", VentDate.ToString("MM/dd/yyyy")));
                                break;
                            case "MONTHWISE":
                                sqlStr = "Select Top 1 Flag from Gen_Miss Where Entry_ty = @Entry_ty And Inv_sr = @Series And Inv_no=@Inv_no And MONTH(Inv_dt) = @Inv_dt_m And Year(Inv_dt) = @Inv_dt_y ";
                                cmd = new SqlCommand(sqlStr, oSqlConObj);
                                cmd.Parameters.Clear();
                                cmd.Parameters.Add(new SqlParameter("@Entry_ty", ventryType));
                                cmd.Parameters.Add(new SqlParameter("@Series", vInvoiceSeries));
                                cmd.Parameters.Add(new SqlParameter("@Inv_no", (int)Convert.ToDouble(GenMiss.Rows[0]["Inv_no"])));
                                cmd.Parameters.Add(new SqlParameter("@Inv_dt_m", VentDate.Month));
                                cmd.Parameters.Add(new SqlParameter("@Inv_dt_y", VentDate.Year));
                                break;
                            default:
                                sqlStr = "Select Top 1 Flag from Gen_Miss Where Entry_ty =@Entry_ty  And Inv_sr = @Series And Inv_no=@Inv_no And L_yn = @L_yn ";
                                cmd = new SqlCommand(sqlStr, oSqlConObj);
                                cmd.Parameters.Clear();
                                cmd.Parameters.Add(new SqlParameter("@Entry_ty", ventryType));
                                cmd.Parameters.Add(new SqlParameter("@Series", vInvoiceSeries));
                                cmd.Parameters.Add(new SqlParameter("@Inv_no", (int)Convert.ToDouble(GenMiss.Rows[0]["Inv_no"])));
                                cmd.Parameters.Add(new SqlParameter("@L_yn", vctrYear));
                                break;
                        }
                        //TmpTbl.Clear();
                        //TmpTbl = null;
                        TmpTbl = new DataTable();
                        cmd.Transaction = SqlTran;
                        //da.SelectCommand = cmd;
                        da = new SqlDataAdapter(cmd);
                        da.Fill(TmpTbl);
                        cmd.Parameters.Clear();
                        string vFoundInMiss = "Y";
                        if (TmpTbl.Rows.Count <= 0)
                        {
                            vFoundInMiss = "N";
                            cmd = new SqlCommand();
                            cmd.Transaction = SqlTran;
                            cmd.Connection = oSqlConObj;
                            cmd = op.GenerateInsertString(cmd, GenMiss.Rows[0], "Gen_Miss", null, null);
                            rowsAffected = cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            vFoundInMiss = TmpTbl.Rows[0]["Flag"].ToString().Trim();
                            if (vFoundInMiss == "N")
                            {
                                GenMiss.Rows[0]["Flag"] = "Y";        //17/10/2013
                                switch (SeriesType)
                                {
                                    case "DAYWISE":
                                        cond = " Entry_ty = @Entry_ty And Inv_sr = @Series And Inv_no=@Inv_no And Inv_dt = @Inv_dt";
                                        cmd = op.GenerateUpdateString(cmd, GenMiss.Rows[0], "Gen_Miss", null, null, cond, null);
                                        //                                    cmd.Parameters.Clear();
                                        cmd.Parameters.Add(new SqlParameter("@Entry_ty", ventryType));
                                        cmd.Parameters.Add(new SqlParameter("@Series", vInvoiceSeries));
                                        cmd.Parameters.Add(new SqlParameter("@Inv_no", (int)Convert.ToDouble(GenMiss.Rows[0]["Inv_no"])));
                                        cmd.Parameters.Add(new SqlParameter("@Inv_dt", VentDate.ToString("MM/dd/yyyy")));
                                        break;
                                    case "MONTHWISE":
                                        cond = " Entry_ty = @Entry_ty And Inv_sr = @Series And Inv_no=@Inv_no And MONTH(Inv_dt) = @Inv_dt_m And Year(Inv_dt) = @Inv_dt_y ";
                                        cmd = op.GenerateUpdateString(cmd, GenMiss.Rows[0], "Gen_Miss", null, null, cond, null);
                                        cmd.Parameters.Add(new SqlParameter("@Entry_ty", ventryType));
                                        cmd.Parameters.Add(new SqlParameter("@Series", vInvoiceSeries));
                                        cmd.Parameters.Add(new SqlParameter("@Inv_no", (int)Convert.ToDouble(GenMiss.Rows[0]["Inv_no"])));
                                        cmd.Parameters.Add(new SqlParameter("@Inv_dt_m", VentDate.Month));
                                        cmd.Parameters.Add(new SqlParameter("@Inv_dt_y", VentDate.Year));
                                        break;
                                    default:
                                        cond = " Entry_ty =@Entry_ty  And Inv_sr = @Series And Inv_no=@Inv_no And L_yn = @L_yn ";
                                        cmd = op.GenerateUpdateString(cmd, GenMiss.Rows[0], "Gen_Miss", null, null, cond, null);
                                        cmd.Parameters.Add(new SqlParameter("@Entry_ty", ventryType));
                                        cmd.Parameters.Add(new SqlParameter("@Series", vInvoiceSeries));
                                        cmd.Parameters.Add(new SqlParameter("@Inv_no", (int)Convert.ToDouble(GenMiss.Rows[0]["Inv_no"])));
                                        cmd.Parameters.Add(new SqlParameter("@L_yn", vctrYear));
                                        break;
                                }
                                cmd.Transaction = SqlTran;
                                cmd.Connection = oSqlConObj;
                                rowsAffected = cmd.ExecuteNonQuery();
                            }
                        }
                        if (vFoundInMiss == "N")
                        {
                            SqlTran.Commit();

                            string sql_main = Entry_Tbl + "main";
                            cmd = new SqlCommand("Select [length] from Syscolumns where [Name]='Inv_no' and Id=Object_Id('" + sql_main + "')", oSqlConObj);
                            int Inv_Size = (int)(Convert.ToDouble(cmd.ExecuteScalar()));
                            ReturnInvNo = GenInv.Rows[0]["inv_no"].ToString().PadLeft(vinv_size, '0');
                            ReturnInvNo = (prefix.Trim() + v_i_middle.Trim() + ReturnInvNo.Trim() + suffix.Trim()).PadRight(Inv_Size, ' ');
                            sqlStr = "Select Top 1 Entry_ty from " + sql_main + " Where Entry_ty =@Entry_ty  And Inv_sr = @Series And Inv_no=@Inv_no And L_yn = @L_yn ";
                            cmd = new SqlCommand(sqlStr, oSqlConObj);
                            cmd.Parameters.Clear();
                            cmd.Parameters.Add(new SqlParameter("@Entry_ty", ventryType));
                            cmd.Parameters.Add(new SqlParameter("@Series", vInvoiceSeries));
                            cmd.Parameters.Add(new SqlParameter("@Inv_no", ReturnInvNo));
                            cmd.Parameters.Add(new SqlParameter("@L_yn", vctrYear));

                            //TmpTbl.Clear();
                            //TmpTbl = null;
                            TmpTbl = new DataTable();
                            //cmd.Transaction = SqlTran;
                            //da.SelectCommand = cmd;
                            da = new SqlDataAdapter(cmd);
                            da.Fill(TmpTbl);
                            if (TmpTbl.Rows.Count <= 0)
                            {
                                IsRollBack = false;
                                _vInvoiceEn = _vInvoiceEn.Trim().Length == 0 ? GenInv.Rows[0]["inv_no"].ToString() : _vInvoiceEn;
                                break;
                            }
                            else
                            {
                                vFoundInMiss = "Y";
                            }
                        }
                        if (vFoundInMiss == "Y")
                        {
                            GenInv.Rows[0]["Inv_no"] = Convert.ToInt32(GenInv.Rows[0]["Inv_no"]) + 1;
                            GenMiss.Rows[0]["Inv_no"] = GenInv.Rows[0]["Inv_no"];
                        }
                    }

                }
                if (IsRollBack == false)
                {
                    GenInv.Rows[0]["Entry_ty"] = ventryType;
                    GenInv.Rows[0]["Inv_dt"] = VentDate;
                    GenInv.Rows[0]["Inv_sr"] = _vInvoiceEs;
                    GenInv.Rows[0]["Inv_No"] = Convert.ToInt32(_vInvoiceEn);
                    GenInv.Rows[0]["l_yn"] = vctrYear;

                    GenMiss.Rows[0]["Entry_ty"] = ventryType;
                    GenMiss.Rows[0]["Inv_dt"] = VentDate;
                    GenMiss.Rows[0]["Inv_sr"] = _vInvoiceEs;
                    GenMiss.Rows[0]["Inv_No"] = Convert.ToInt32(_vInvoiceEn);
                    GenMiss.Rows[0]["L_yn"] = vctrYear;
                    GenMiss.Rows[0]["Flag"] = "N";

                    //string cond = string.Empty;

                    DBOperation op = new DBOperation();
                    cmd.Parameters.Clear();
                    switch (SeriesType)
                    {
                        case "DAYWISE":
                            cond = " Entry_ty = @Entry_ty And Inv_sr = @Series And Inv_no=@Inv_no And Inv_dt = @Inv_dt";
                            cmd = op.GenerateUpdateString(cmd, GenMiss.Rows[0], "Gen_Miss", new string[] { "flag" }, null, cond, null);
                            //                                    cmd.Parameters.Clear();
                            cmd.Parameters.Add(new SqlParameter("@Entry_ty", ventryType));
                            cmd.Parameters.Add(new SqlParameter("@Series", vInvoiceSeries));
                            cmd.Parameters.Add(new SqlParameter("@Inv_no", (int)Convert.ToDouble(GenMiss.Rows[0]["Inv_no"])));
                            cmd.Parameters.Add(new SqlParameter("@Inv_dt", VentDate.ToString("MM/dd/yyyy")));
                            break;
                        case "MONTHWISE":
                            cond = "Entry_ty=@Entry_ty And Inv_sr = @Series And Inv_no=@Inv_no And MONTH(Inv_dt) = @Inv_dt_m And Year(Inv_dt) = @Inv_dt_y ";
                            cmd = op.GenerateUpdateString(cmd, GenMiss.Rows[0], "Gen_Miss", new string[] { "flag" }, null, cond, null);
                            cmd.Parameters.Add(new SqlParameter("@Entry_ty", ventryType));
                            cmd.Parameters.Add(new SqlParameter("@Series", vInvoiceSeries));
                            cmd.Parameters.Add(new SqlParameter("@Inv_no", (int)Convert.ToDouble(GenMiss.Rows[0]["Inv_no"])));
                            cmd.Parameters.Add(new SqlParameter("@Inv_dt_m", VentDate.Month));
                            cmd.Parameters.Add(new SqlParameter("@Inv_dt_y", VentDate.Year));
                            break;
                        default:
                            cond = "Entry_ty=@Entry_ty And Inv_sr = @Series And Inv_no=@Inv_no And L_yn=@L_yn ";
                            cmd = op.GenerateUpdateString(cmd, GenMiss.Rows[0], "Gen_Miss", new string[] { "flag" }, null, cond, null);
                            cmd.Parameters.Add(new SqlParameter("@Entry_ty", ventryType));
                            cmd.Parameters.Add(new SqlParameter("@Series", vInvoiceSeries));
                            cmd.Parameters.Add(new SqlParameter("@Inv_no", (int)Convert.ToDouble(GenMiss.Rows[0]["Inv_no"])));
                            cmd.Parameters.Add(new SqlParameter("@L_yn", vctrYear));
                            break;
                    }
                    //cmd.Transaction = SqlTran;
                    cmd.Connection = oSqlConObj;
                    rowsAffected = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                if (SqlTran != null)
                {
                    SqlTran.Rollback();
                    SqlTran = null;

                }
                return ReturnInvNo;
            }
            finally
            {
                if (oSqlConObj != null)
                {
                    if (oSqlConObj.State == ConnectionState.Open)
                    {
                        oSqlConObj.Close();
                    }
                }
            }
            GenInv = null;
            GenMiss = null;
            return ReturnInvNo;
        }

        public void DeleteGeneratedInvNo(string ventryType, string vInvoiceSeries, string vInvoiceNo, string VentYear, DateTime VentDate, SqlConnection oSqlConObj, bool AutoTransaction)
        {
            string ReturnInvNo = string.Empty;
            string SeriesType = string.Empty;
            string prefix = string.Empty;
            string suffix = string.Empty;
            string monthFormat = string.Empty;
            string cond = string.Empty;
            DBOperation op = new DBOperation();

            string v_i_middle = string.Empty;
            int rowsAffected;
            SqlCommand cmd = new SqlCommand("Select top 1 * From Series Where Inv_sr=@Series", oSqlConObj);
            cmd.Parameters.Add(new SqlParameter("@Series", vInvoiceSeries));
            DataTable Series = new DataTable("Series_vw");

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            SqlTransaction tran = null;
            da.Fill(Series);

            if (oSqlConObj.State == ConnectionState.Closed)
                oSqlConObj.Open();

            if (Series.Rows.Count > 0)
            {
                SeriesType = (Convert.ToString(Series.Rows[0]["s_Type"])).Trim();
                prefix = (Convert.ToString(Series.Rows[0]["i_prefix"]).Replace('"', ' ').Replace("'", "").Trim()).Trim();
                suffix = (Convert.ToString(Series.Rows[0]["i_suffix"]).Replace('"', ' ').Replace("'", "")).Trim();
                monthFormat = (Convert.ToString(Series.Rows[0]["mnthformat"])).Trim();

                switch (SeriesType)
                {
                    case "DAYWISE":
                        v_i_middle = VentDate.ToString("ddMMyy");
                        break;
                    case "MONTHWISE":
                        string SQL = "SELECT MnthFrmt FROM monthformat Where MnthFrmt ='" + monthFormat + "'";
                        cmd = new SqlCommand(SQL, oSqlConObj);
                        SqlDataReader r;
                        if (oSqlConObj.State == ConnectionState.Closed)
                            oSqlConObj.Open();
                        r = cmd.ExecuteReader();

                        while (r.Read())
                        {
                            v_i_middle = Convert.ToString(r["MnthFrmt"]);
                        }
                        r.Close();
                        if (v_i_middle == string.Empty)
                            v_i_middle = VentDate.ToString("MMdd");
                        break;
                    default:
                        break;
                }
                if (vInvoiceNo.Length > 0)
                {
                    vInvoiceNo = vInvoiceNo.Substring(prefix.Length + v_i_middle.Length);
                    vInvoiceNo = vInvoiceNo.Substring(0, vInvoiceNo.Length - suffix.Length);
                }

            }
            vInvoiceNo = vInvoiceNo.Trim() == "" ? "0" : Convert.ToInt32(vInvoiceNo).ToString();
            vInvoiceNo = (vInvoiceNo.Trim() == "0" ? "1" : vInvoiceNo);

            int _vInvoiceEn = (Convert.ToInt32(vInvoiceNo) <= 0 ? 0 : (Convert.ToInt32(vInvoiceNo) - 1));

            DataTable GenInv = new DataTable("Gen_Inv_vw");
            DataTable GenMiss = new DataTable("Gen_Miss_vw");
            try
            {
                cmd = new SqlCommand(" Select * from Gen_inv with (NOLOCK) where 1=0 ", oSqlConObj);
                da = new SqlDataAdapter(cmd);
                da.Fill(GenInv);
                cmd.CommandText = " Select * from Gen_miss with (NOLOCK) where 1=0 ";
                da.Fill(GenMiss);

                DataRow InvRow = GenInv.NewRow();
                InvRow["Entry_ty"] = ventryType;
                InvRow["Inv_dt"] = VentDate;
                InvRow["Inv_sr"] = Series;
                InvRow["Inv_no"] = vInvoiceNo;
                InvRow["L_yn"] = VentYear;
                GenInv.Rows.Add(InvRow);


                DataRow MissRow = GenMiss.NewRow();
                MissRow["Entry_ty"] = ventryType;
                MissRow["Inv_dt"] = VentDate;
                MissRow["Inv_sr"] = Series;
                MissRow["Inv_no"] = vInvoiceNo;
                MissRow["L_yn"] = VentYear;
                MissRow["Flag"] = "Y";
                GenMiss.Rows.Add(MissRow);

                DataTable TmpTbl;
                //DataTable TmpTbl = new DataTable();

                string sqlStr = string.Empty;
                TmpTbl = new DataTable();
                //TmpTbl = null;
                //if (TmpTbl != null)
                //{
                //    TmpTbl.Rows.Clear();
                //}
                switch (SeriesType)
                {
                    case "DAYWISE":
                        sqlStr = "Select Top 1 Inv_no from Gen_inv with (TABLOCKX) where Entry_ty =@Entry_ty And Inv_sr = @Series "
                                + " And Inv_dt = @Inv_dt";
                        cmd = new SqlCommand(sqlStr, oSqlConObj);
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add(new SqlParameter("@Entry_ty", ventryType));
                        cmd.Parameters.Add(new SqlParameter("@Series", vInvoiceSeries));
                        cmd.Parameters.Add(new SqlParameter("@Inv_dt", VentDate.ToString("MM/dd/yyyy")));

                        break;
                    case "MONTHWISE":
                        sqlStr = "  Select Top 1 Inv_no from Gen_inv with (TABLOCKX) where Entry_ty = @Entry_ty "
                                + " And Inv_sr = @Series And MONTH(Inv_dt) = @Inv_dt_m And Year(Inv_dt) = @Inv_dt_y ";
                        cmd = new SqlCommand(sqlStr, oSqlConObj);
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add(new SqlParameter("@Entry_ty", ventryType));
                        cmd.Parameters.Add(new SqlParameter("@Series", vInvoiceSeries));
                        cmd.Parameters.Add(new SqlParameter("@Inv_dt_m", VentDate.Month));
                        cmd.Parameters.Add(new SqlParameter("@Inv_dt_y", VentDate.Year));
                        break;
                    default:
                        sqlStr = "  Select Top 1 Inv_no from Gen_inv with (TABLOCKX) where Entry_ty =@Entry_ty  "
                                    + " And Inv_sr = @Series And L_yn = @L_yn ";
                        cmd = new SqlCommand(sqlStr, oSqlConObj);
                        cmd.Parameters.Add(new SqlParameter("@Entry_ty", ventryType));
                        cmd.Parameters.Add(new SqlParameter("@Series", vInvoiceSeries));
                        cmd.Parameters.Add(new SqlParameter("@L_yn", VentYear));
                        break;
                }
                da = new SqlDataAdapter(cmd);
                tran = oSqlConObj.BeginTransaction();
                cmd.Transaction = tran;
                da.Fill(TmpTbl);


                if (TmpTbl.Rows.Count <= 0)
                {
                    cmd = new SqlCommand();
                    cmd = op.GenerateInsertString(cmd, GenInv.Rows[0], "Gen_Inv", null, null);
                    cmd.Transaction = tran;
                    cmd.Connection = oSqlConObj;
                    rowsAffected = cmd.ExecuteNonQuery();
                }
                else
                {
                    _vInvoiceEn = Convert.ToInt32(GenInv.Rows[0]["Inv_no"]) == Convert.ToInt32(vInvoiceNo) ? _vInvoiceEn : Convert.ToInt32(GenInv.Rows[0]["Inv_no"]);
                    GenInv.Rows[0]["Inv_no"] = _vInvoiceEn.ToString();
                    cmd = new SqlCommand();
                    switch (SeriesType)
                    {
                        case "DAYWISE":
                            cond = " Entry_ty = @Entry_ty And Inv_sr = @Series And Inv_dt = @Inv_dt";
                            cmd = op.GenerateUpdateString(cmd, GenInv.Rows[0], "Gen_Inv", null, null, cond, null);
                            cmd.Parameters.Add(new SqlParameter("@Entry_ty", ventryType));
                            cmd.Parameters.Add(new SqlParameter("@Series", vInvoiceSeries));
                            cmd.Parameters.Add(new SqlParameter("@Inv_dt", VentDate.ToString("MM/dd/yyyy")));
                            break;
                        case "MONTHWISE":
                            cond = " Entry_ty = @Entry_ty  And Inv_sr = @Series And MONTH(Inv_dt) = @Inv_dt_m And Year(Inv_dt) = @Inv_dt_y ";
                            cmd = op.GenerateUpdateString(cmd, GenInv.Rows[0], "Gen_Inv", null, null, cond, null);
                            cmd.Parameters.Add(new SqlParameter("@Entry_ty", ventryType));
                            cmd.Parameters.Add(new SqlParameter("@Series", vInvoiceSeries));
                            cmd.Parameters.Add(new SqlParameter("@Inv_dt_m", VentDate.Month));
                            cmd.Parameters.Add(new SqlParameter("@Inv_dt_y", VentDate.Year));
                            break;
                        default:
                            cond = " Entry_ty =@Entry_ty  And Inv_sr = @Series And L_yn = @L_yn ";
                            cmd = op.GenerateUpdateString(cmd, GenInv.Rows[0], "Gen_Inv", null, null, cond, null);
                            cmd.Parameters.Add(new SqlParameter("@Entry_ty", ventryType));
                            cmd.Parameters.Add(new SqlParameter("@Series", vInvoiceSeries));
                            cmd.Parameters.Add(new SqlParameter("@L_yn", VentYear));
                            break;
                    }
                    cmd.Transaction = tran;
                    cmd.Connection = oSqlConObj;
                    rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        GenInv.Rows[0]["Inv_no"] = vInvoiceNo;
                        GenMiss.Rows[0]["Inv_no"] = vInvoiceNo;
                        switch (SeriesType)
                        {
                            case "DAYWISE":
                                sqlStr = "Select Top 1 Inv_no from Gen_Miss Where Entry_ty =@Entry_ty And Inv_sr = @Series And Inv_no=@Inv_no And Inv_dt = @Inv_dt";
                                cmd = new SqlCommand(sqlStr, oSqlConObj);
                                cmd.Parameters.Clear();
                                cmd.Parameters.Add(new SqlParameter("@Entry_ty", ventryType));
                                cmd.Parameters.Add(new SqlParameter("@Series", vInvoiceSeries));
                                cmd.Parameters.Add(new SqlParameter("@Inv_no", GenInv.Rows[0]["Inv_no"]).ToString());
                                cmd.Parameters.Add(new SqlParameter("@Inv_dt", VentDate.ToString("MM/dd/yyyy")));
                                break;
                            case "MONTHWISE":
                                sqlStr = "Select Top 1 Inv_no from Gen_Miss Where Entry_ty = @Entry_ty  And Inv_sr = @Series And Inv_no=@Inv_no And MONTH(Inv_dt) = @Inv_dt_m And Year(Inv_dt) = @Inv_dt_y ";
                                cmd = new SqlCommand(sqlStr, oSqlConObj);
                                cmd.Parameters.Clear();
                                cmd.Parameters.Add(new SqlParameter("@Entry_ty", ventryType));
                                cmd.Parameters.Add(new SqlParameter("@Series", vInvoiceSeries));
                                cmd.Parameters.Add(new SqlParameter("@Inv_no", GenInv.Rows[0]["Inv_no"]).ToString());
                                cmd.Parameters.Add(new SqlParameter("@Inv_dt_m", VentDate.Month));
                                cmd.Parameters.Add(new SqlParameter("@Inv_dt_y", VentDate.Year));
                                break;
                            default:
                                sqlStr = "Select Top 1 Inv_no from Gen_Miss Where Entry_ty =@Entry_ty  And Inv_sr = @Series And Inv_no=@Inv_no And L_yn = @L_yn ";
                                cmd = new SqlCommand(sqlStr, oSqlConObj);
                                cmd.Parameters.Clear();
                                cmd.Parameters.Add(new SqlParameter("@Entry_ty", ventryType));
                                cmd.Parameters.Add(new SqlParameter("@Series", vInvoiceSeries));
                                cmd.Parameters.Add(new SqlParameter("@Inv_no", GenInv.Rows[0]["Inv_no"]).ToString());
                                cmd.Parameters.Add(new SqlParameter("@L_yn", VentYear));
                                break;
                        }
                        //TmpTbl.Clear();
                        TmpTbl = new DataTable();
                        cmd.Transaction = tran;
                        da = new SqlDataAdapter(cmd);
                        da.Fill(TmpTbl);

                        if (TmpTbl.Rows.Count <= 0)
                        {
                            cmd = new SqlCommand();
                            cmd = op.GenerateInsertString(cmd, GenMiss.Rows[0], "Gen_Miss", null, null);
                            cmd.Transaction = tran;
                            cmd.Connection = oSqlConObj;
                            rowsAffected = cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            switch (SeriesType)
                            {
                                case "DAYWISE":
                                    cond = " Entry_ty = @Entry_ty And Inv_sr = @Series And Inv_no=@Inv_no And Inv_dt = @Inv_dt";
                                    cmd = op.GenerateUpdateString(cmd, GenMiss.Rows[0], "Gen_Miss", new string[] { "Inv_no", "flag" }, null, cond, null);
                                    //                                    cmd.Parameters.Clear();
                                    cmd.Parameters.Add(new SqlParameter("@Entry_ty", ventryType));
                                    cmd.Parameters.Add(new SqlParameter("@Series", vInvoiceSeries));
                                    cmd.Parameters.Add(new SqlParameter("@Inv_no", GenMiss.Rows[0]["Inv_no"]).ToString());
                                    cmd.Parameters.Add(new SqlParameter("@Inv_dt", VentDate.ToString("MM/dd/yyyy")));
                                    break;
                                case "MONTHWISE":
                                    cond = " Entry_ty = @Entry_ty And Inv_sr = @Series And Inv_no=@Inv_no And MONTH(Inv_dt) = @Inv_dt_m And Year(Inv_dt) = @Inv_dt_y ";
                                    cmd = op.GenerateUpdateString(cmd, GenMiss.Rows[0], "Gen_Miss", new string[] { "Inv_no", "flag" }, null, cond, null);
                                    cmd.Parameters.Add(new SqlParameter("@Entry_ty", ventryType));
                                    cmd.Parameters.Add(new SqlParameter("@Series", vInvoiceSeries));
                                    cmd.Parameters.Add(new SqlParameter("@Inv_no", GenMiss.Rows[0]["Inv_no"]).ToString());
                                    cmd.Parameters.Add(new SqlParameter("@Inv_dt_m", VentDate.Month));
                                    cmd.Parameters.Add(new SqlParameter("@Inv_dt_y", VentDate.Year));
                                    break;
                                default:
                                    cond = " Entry_ty =@Entry_ty  And Inv_sr = @Series And Inv_no=@Inv_no And L_yn = @L_yn ";
                                    cmd = op.GenerateUpdateString(cmd, GenMiss.Rows[0], "Gen_Miss", new string[] { "Inv_no", "flag" }, null, cond, null);
                                    cmd.Parameters.Add(new SqlParameter("@Entry_ty", ventryType));
                                    cmd.Parameters.Add(new SqlParameter("@Series", vInvoiceSeries));
                                    cmd.Parameters.Add(new SqlParameter("@Inv_no", GenMiss.Rows[0]["Inv_no"]).ToString());
                                    cmd.Parameters.Add(new SqlParameter("@L_yn", VentYear));
                                    break;
                            }
                            cmd.Transaction = tran;
                            cmd.Connection = oSqlConObj;
                            rowsAffected = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();

                        }
                    }

                }



                switch (SeriesType)
                {
                    case "DAYWISE":
                        cond = " Where Entry_ty = @Entry_ty And Inv_sr = @Series And Inv_no>@Inv_no And Inv_dt = @Inv_dt and flag='Y'";
                        cmd.CommandText = "Select Select Top 1 Inv_no From Gen_miss Where " + cond;
                        cmd.Parameters.Add(new SqlParameter("@Entry_ty", ventryType));
                        cmd.Parameters.Add(new SqlParameter("@Series", vInvoiceSeries));
                        cmd.Parameters.Add(new SqlParameter("@Inv_no", GenMiss.Rows[0]["Inv_no"]).ToString());
                        cmd.Parameters.Add(new SqlParameter("@Inv_dt", VentDate.ToString("MM/dd/yyyy")));
                        break;
                    case "MONTHWISE":
                        cond = " Where Entry_ty = @Entry_ty And Inv_sr = @Series And Inv_no>@Inv_no And MONTH(Inv_dt) = @Inv_dt_m And Year(Inv_dt) = @Inv_dt_y and flag='Y'";
                        cmd.CommandText = "Select Select Top 1 Inv_no From Gen_miss Where " + cond;
                        cmd.Parameters.Add(new SqlParameter("@Entry_ty", ventryType));
                        cmd.Parameters.Add(new SqlParameter("@Series", vInvoiceSeries));
                        cmd.Parameters.Add(new SqlParameter("@Inv_no", GenMiss.Rows[0]["Inv_no"]).ToString());
                        cmd.Parameters.Add(new SqlParameter("@Inv_dt_m", VentDate.Month));
                        cmd.Parameters.Add(new SqlParameter("@Inv_dt_y", VentDate.Year));
                        break;
                    default:
                        cond = " Where Entry_ty =@Entry_ty  And Inv_sr = @Series And Inv_no>@Inv_no And L_yn = @L_yn and flag='Y'";
                        cmd.CommandText = "Select Select Top 1 Inv_no From Gen_miss Where " + cond;
                        cmd.Parameters.Add(new SqlParameter("@Entry_ty", ventryType));
                        cmd.Parameters.Add(new SqlParameter("@Series", vInvoiceSeries));
                        cmd.Parameters.Add(new SqlParameter("@Inv_no", GenMiss.Rows[0]["Inv_no"]).ToString());
                        cmd.Parameters.Add(new SqlParameter("@L_yn", VentYear));
                        break;
                }
                //rowsAffected = cmd.ExecuteNonQuery();
                //TmpTbl.Clear();
                TmpTbl = new DataTable();
                cmd.Transaction = tran;
                cmd.Connection = oSqlConObj;
                da = new SqlDataAdapter(cmd);
                da.Fill(TmpTbl);
                if (TmpTbl != null)
                {
                    if (TmpTbl.Rows.Count <= 0)
                    {
                        switch (SeriesType)
                        {
                            case "DAYWISE":
                                cond = " Where Entry_ty = @Entry_ty And Inv_sr = @Series And Inv_no<@Inv_no And Inv_dt = @Inv_dt and flag='Y' Order by Inv_no ";
                                cmd.CommandText = "Select Top 1 Inv_no From Gen_miss Where " + cond;
                                cmd.Parameters.Add(new SqlParameter("@Entry_ty", ventryType));
                                cmd.Parameters.Add(new SqlParameter("@Series", vInvoiceSeries));
                                cmd.Parameters.Add(new SqlParameter("@Inv_no", GenMiss.Rows[0]["Inv_no"]).ToString());
                                cmd.Parameters.Add(new SqlParameter("@Inv_dt", VentDate.ToString("MM/dd/yyyy")));
                                break;
                            case "MONTHWISE":
                                cond = " Where Entry_ty = @Entry_ty And Inv_sr = @Series And Inv_no>@Inv_no And MONTH(Inv_dt) = @Inv_dt_m And Year(Inv_dt) = @Inv_dt_y and flag='Y' Order by Inv_no";
                                cmd.CommandText = "Select Top 1 Inv_no From Gen_miss Where " + cond;
                                cmd.Parameters.Add(new SqlParameter("@Entry_ty", ventryType));
                                cmd.Parameters.Add(new SqlParameter("@Series", vInvoiceSeries));
                                cmd.Parameters.Add(new SqlParameter("@Inv_no", GenMiss.Rows[0]["Inv_no"]).ToString());
                                cmd.Parameters.Add(new SqlParameter("@Inv_dt_m", VentDate.Month));
                                cmd.Parameters.Add(new SqlParameter("@Inv_dt_y", VentDate.Year));
                                break;
                            default:
                                cond = " Where Entry_ty =@Entry_ty  And Inv_sr = @Series And Inv_no>@Inv_no And L_yn = @L_yn and flag='Y' Order by Inv_no";
                                cmd.CommandText = "Select Top 1 Inv_no From Gen_miss Where " + cond;
                                cmd.Parameters.Add(new SqlParameter("@Entry_ty", ventryType));
                                cmd.Parameters.Add(new SqlParameter("@Series", vInvoiceSeries));
                                cmd.Parameters.Add(new SqlParameter("@Inv_no", GenMiss.Rows[0]["Inv_no"]).ToString());
                                cmd.Parameters.Add(new SqlParameter("@L_yn", VentYear));
                                break;
                        }
                        //TmpTbl.Clear();
                        TmpTbl = new DataTable();
                        //da.SelectCommand = cmd;
                        cmd.Connection = oSqlConObj;
                        cmd.Transaction = tran;
                        da = new SqlDataAdapter(cmd);
                        da.Fill(TmpTbl);
                        if (TmpTbl != null)
                        {
                            if (TmpTbl.Rows.Count > 0)
                            {
                                vInvoiceNo = Convert.ToString(TmpTbl.Rows[0][0]);
                            }
                            GenInv.Rows[0]["Inv_no"] = vInvoiceNo;
                            switch (SeriesType)
                            {
                                case "DAYWISE":
                                    cond = " Entry_ty = @Entry_ty And Inv_sr = @Series And Inv_dt = @Inv_dt";
                                    cmd = op.GenerateUpdateString(cmd, GenInv.Rows[0], "Gen_Inv", new string[] { "Inv_no" }, null, cond, null);
                                    cmd.Parameters.Add(new SqlParameter("@Entry_ty", ventryType));
                                    cmd.Parameters.Add(new SqlParameter("@Series", vInvoiceSeries));
                                    cmd.Parameters.Add(new SqlParameter("@Inv_dt", VentDate.ToString("MM/dd/yyyy")));
                                    break;
                                case "MONTHWISE":
                                    cond = " Entry_ty = @Entry_ty  And Inv_sr = @Series And MONTH(Inv_dt) = @Inv_dt_m And Year(Inv_dt) = @Inv_dt_y ";
                                    cmd = op.GenerateUpdateString(cmd, GenInv.Rows[0], "Gen_Inv", new string[] { "Inv_no" }, null, cond, null);
                                    cmd.Parameters.Add(new SqlParameter("@Entry_ty", ventryType));
                                    cmd.Parameters.Add(new SqlParameter("@Series", vInvoiceSeries));
                                    cmd.Parameters.Add(new SqlParameter("@Inv_dt_m", VentDate.Month));
                                    cmd.Parameters.Add(new SqlParameter("@Inv_dt_y", VentDate.Year));
                                    break;
                                default:
                                    cond = " Entry_ty =@Entry_ty  And Inv_sr = @Series And L_yn = @L_yn ";
                                    cmd = op.GenerateUpdateString(cmd, GenInv.Rows[0], "Gen_Inv", new string[] { "Inv_no" }, null, cond, null);
                                    cmd.Parameters.Add(new SqlParameter("@Entry_ty", ventryType));
                                    cmd.Parameters.Add(new SqlParameter("@Series", vInvoiceSeries));
                                    cmd.Parameters.Add(new SqlParameter("@L_yn", VentYear));
                                    break;
                            }
                            cmd.Connection = oSqlConObj;
                            cmd.Transaction = tran;
                            rowsAffected = cmd.ExecuteNonQuery();
                        }
                    }

                }
                if (rowsAffected > 0)
                {
                    tran.Commit();
                }
                else
                {
                    tran.Rollback();
                }
            }
            catch (Exception ex)
            {
                //return ReturnInvNo;
                tran.Rollback();
            }
            GenInv = null;
            GenMiss = null;
            return;
        }
        public string GenerateDocNo(string ventryType, DateTime VentDate, string Entry_Tbl, SqlConnection oSqlConObj, string vcDbName, DateTime vdSta_Dt, DateTime vdEnd_Dt)
        {
            DataTable GenDoc = new DataTable("Gen_Inv_vw");
            SqlTransaction SqlTran = null;
            string ReturnDocNo = string.Empty;
            bool IsRollBack = true;


            try
            {
                if (oSqlConObj.State == ConnectionState.Closed)
                    oSqlConObj.Open();

                DBOperation op = new DBOperation();
                int rowsAffected;
                DataTable TmpTbl = new DataTable();

                SqlCommand cmd = new SqlCommand(" Select * from Gen_doc with (NOLOCK) where 1=0 ", oSqlConObj);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(GenDoc);
                cmd.Parameters.Clear();

                DataRow DocRow = GenDoc.NewRow();
                DocRow["Entry_ty"] = ventryType;
                DocRow["Date"] = VentDate;
                DocRow["Doc_no"] = 0;
                DocRow["CompId"] = 0;
                GenDoc.Rows.Add(DocRow);
                while (true)
                {
                    string sqlStr = "Select Top 1 Doc_No from Gen_doc with (TABLOCKX) Where Entry_ty =@Entry_ty and Date = @Date";
                    SqlTran = oSqlConObj.BeginTransaction();
                    cmd = new SqlCommand(sqlStr, oSqlConObj);

                    cmd.Parameters.Add(new SqlParameter("@Entry_ty", ventryType));
                    cmd.Parameters.Add(new SqlParameter("@Date", VentDate.ToString("MM/dd/yyyy")));
                    cmd.Transaction = SqlTran;
                    TmpTbl.Clear();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(TmpTbl);

                    //rowsAffected = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    if (TmpTbl.Rows.Count <= 0)
                    {
                        GenDoc.Rows[0]["Doc_No"] = 1;
                        cmd = op.GenerateInsertString(cmd, GenDoc.Rows[0], "Gen_Doc", null, null);
                    }
                    else
                    {
                        GenDoc.Rows[0]["Doc_No"] = Convert.ToInt32(TmpTbl.Rows[0]["Doc_No"]) + 1;

                        string cond = string.Empty;
                        cond = " Entry_ty=@Entry_ty and Date=@Date ";
                        cmd.Parameters.Add(new SqlParameter("@Entry_ty", ventryType));
                        cmd.Parameters.Add(new SqlParameter("@Date", VentDate.ToString("MM/dd/yyyy")));
                        cmd = op.GenerateUpdateString(cmd, GenDoc.Rows[0], "Gen_Doc", null, null, cond, null);
                    }
                    rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        SqlTran.Commit();
                        string sql_main = Entry_Tbl + "main";
                        cmd = new SqlCommand("Select [length] from Syscolumns where [Name]='Doc_no' and Id=Object_Id('" + sql_main + "')", oSqlConObj);
                        int Docno_Size = (int)(Convert.ToDouble(cmd.ExecuteScalar()));
                        ReturnDocNo = GenDoc.Rows[0]["Doc_no"].ToString().Trim().PadLeft(Docno_Size, '0');

                        sqlStr = "Select Top 1 Entry_ty from " + sql_main + " Where Entry_ty =@Entry_ty  And Date=@Date And Doc_no=@Doc_no ";
                        cmd = new SqlCommand(sqlStr, oSqlConObj);
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add(new SqlParameter("@Entry_ty", ventryType));
                        cmd.Parameters.Add(new SqlParameter("@Date", VentDate.ToString("MM/dd/yyyy")));
                        cmd.Parameters.Add(new SqlParameter("@Doc_no", ReturnDocNo));
                        TmpTbl.Clear();
                        //cmd.Transaction = SqlTran;
                        //da.SelectCommand = cmd;
                        da = new SqlDataAdapter(cmd);
                        da.Fill(TmpTbl);
                        if (TmpTbl.Rows.Count <= 0)
                        {
                            IsRollBack = false;
                            break;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                SqlTran.Rollback();
                return ReturnDocNo;
            }
            return ReturnDocNo;
        }

    }
}
