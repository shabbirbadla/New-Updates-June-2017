using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace UdyogDataOperation
{
    public class DBOperation
    {
        public SqlCommand GenerateInsertString(SqlCommand sqlCmd, DataRow SourceRow, string TargetTable, string[] ExcludeFields, string[] IncludeFields)
        {
            /// GenerateInsertString(sourceRow, MainTable, null, new string[] { "transId" });  


            string insSqlStr = "";
            string insFlds = "";
            int parameterCount = 0;

            for (int i = 0; i <= SourceRow.Table.Columns.Count - 1; i++)
            {
                bool xflag = false;

                // exclude parameters blank
                if (ExcludeFields != null)
                {
                    foreach (string exField in ExcludeFields)
                    {
                        if (exField != null)
                        {
                            if (exField.Trim().ToUpper() == SourceRow.Table.Columns[i].ColumnName.Trim().ToUpper())
                            {
                                xflag = true;
                                break;
                            }
                        }
                    }
                }
                // end

                // include parameters blank
                if (IncludeFields != null)
                {
                    foreach (string inField in IncludeFields)
                    {
                        if (inField != null)
                        {
                            if (inField.Trim().ToUpper() == SourceRow.Table.Columns[i].ColumnName.Trim().ToUpper())
                            {
                                xflag = false;
                                break;
                            }
                            else
                            {
                                xflag = true;
                            }
                        }
                    }
                }
                // end

                if (xflag == false)
                {
                    if (insFlds.Trim() == "")
                        insFlds += "[" + SourceRow.Table.Columns[i].ColumnName.Trim() + "]";
                    else
                        insFlds += ",[" + SourceRow.Table.Columns[i].ColumnName.Trim() + "]";
                    parameterCount++;
                }
            }  // end generate Field string


            insSqlStr += "insert into " + TargetTable.Trim() + "(" + insFlds + ") values (";
            int paramNumber = 0;
            string paramName = "";
            insFlds = "";

            // Start extract field value string
            for (int i = 0; i <= SourceRow.Table.Columns.Count - 1; i++)
            {
                bool xflag = false;

                // exclude parameters blank
                if (ExcludeFields != null)
                {
                    foreach (string exField in ExcludeFields)
                    {
                        if (exField != null)
                        {
                            if (exField.Trim().ToUpper() == SourceRow.Table.Columns[i].ColumnName.Trim().ToUpper())
                            {
                                xflag = true;
                                break;
                            }
                        }
                    }
                }
                // end

                // include parameters blank
                if (IncludeFields != null)
                {
                    foreach (string inField in IncludeFields)
                    {
                        if (inField != null)
                        {
                            if (inField.Trim().ToUpper() == SourceRow.Table.Columns[i].ColumnName.Trim().ToUpper())
                            {
                                xflag = false;
                                break;
                            }
                            else
                            {
                                xflag = true;
                            }
                        }
                    }
                }
                // end

                if (xflag == false)
                {
                    if (insFlds.Trim() != "")
                        insFlds += ",";

                    paramNumber++;
                    paramName = "@P" + paramNumber.ToString().Trim();
                    switch (SourceRow.Table.Columns[i].DataType.ToString().Trim().ToUpper())
                    {

                        case "SYSTEM.STRING":
                            insFlds += paramName;
                            sqlCmd.Parameters.Add(new SqlParameter(paramName, SqlDbType.VarChar));
                            sqlCmd.Parameters[paramName].Value = !DBNull.Value.Equals(SourceRow[SourceRow.Table.Columns[i]]) ? Convert.ToString(SourceRow[i]) : string.Empty;

                            break;
                        case "SYSTEM.DECIMAL":
                            insFlds += paramName;
                            sqlCmd.Parameters.Add(new SqlParameter(paramName, SqlDbType.Decimal));
                            sqlCmd.Parameters[paramName].Value = (!DBNull.Value.Equals(SourceRow[SourceRow.Table.Columns[i]]) ?
                                    Convert.ToDecimal(SourceRow[SourceRow.Table.Columns[i]]) :Convert.ToDecimal(0.00));
                            break;
                        case "SYSTEM.SINGLE":
                        case "SYSTEM.INT16":
                            insFlds += paramName;
                            sqlCmd.Parameters.Add(new SqlParameter(paramName, SqlDbType.Int));
                            sqlCmd.Parameters[paramName].Value = (!DBNull.Value.Equals(SourceRow[SourceRow.Table.Columns[i]]) ?
                                       Convert.ToInt16(SourceRow[SourceRow.Table.Columns[i]]) : 0);
                            break;
                        case "SYSTEM.INT32":
                            insFlds += paramName;
                            sqlCmd.Parameters.Add(new SqlParameter(paramName, SqlDbType.Int));
                            sqlCmd.Parameters[paramName].Value = (!DBNull.Value.Equals(SourceRow[SourceRow.Table.Columns[i]]) ?
                                       Convert.ToInt32(SourceRow[SourceRow.Table.Columns[i]]) : 0);
                            break;
                        case "SYSTEM.INT64":
                            insFlds += paramName;
                            sqlCmd.Parameters.Add(new SqlParameter(paramName, SqlDbType.Decimal));
                            sqlCmd.Parameters[paramName].Value = (!DBNull.Value.Equals(SourceRow[SourceRow.Table.Columns[i]]) ?
                                       Convert.ToInt64(SourceRow[SourceRow.Table.Columns[i]]) : 0.00);

                            break;
                        case "SYSTEM.DATETIME":
                            insFlds += paramName;
                            sqlCmd.Parameters.Add(new SqlParameter(paramName, SqlDbType.DateTime));
                            sqlCmd.Parameters[paramName].Value =!DBNull.Value.Equals(SourceRow[SourceRow.Table.Columns[i]])? Convert.ToDateTime(SourceRow[SourceRow.Table.Columns[i]]):Convert.ToDateTime("01/01/1900");
                            break;
                        case "SYSTEM.BOOLEAN":
                            insFlds += paramName;
                            sqlCmd.Parameters.Add(new SqlParameter(paramName, SqlDbType.Bit));
                            sqlCmd.Parameters[paramName].Value = (!DBNull.Value.Equals(SourceRow[SourceRow.Table.Columns[i]]) ?
                                (Convert.ToBoolean(SourceRow[SourceRow.Table.Columns[i]]) == true ? 1 : 0) : 0);
                            break;
                    }
                }
            }

            insSqlStr += insFlds + ")";

            sqlCmd.CommandText = insSqlStr;
            return sqlCmd;

        }
        public SqlCommand GenerateUpdateString(SqlCommand sqlCmd, DataRow SourceRow, string TargetTable, string[] ExcludeFields, string[] IncludeFields, string cond, string[] keyFields)
        {
            ///  GenerateUpdateString(MainView, MainTable, new string[] { "transId" }, "", null, new string[] { "transId"});
            string upSqlStr = "";
            string upFlds = "";
            //int parameterCount = 0;
            int paramNumber = 0;
            string paramName = "";


            for (int i = 0; i <= SourceRow.Table.Columns.Count - 1; i++)
            {
                bool xflag = false;

                // exclude parameters blank
                if (ExcludeFields != null)
                {
                    foreach (string exField in ExcludeFields)
                    {
                        if (exField != null)
                        {
                            if (exField.Trim().ToUpper() == SourceRow.Table.Columns[i].ColumnName.Trim().ToUpper())
                            {
                                xflag = true;
                                break;
                            }
                        }
                    }
                }
                // end

                // include parameters blank
                if (IncludeFields != null)
                {
                    foreach (string inField in IncludeFields)
                    {
                        if (inField != null)
                        {
                            if (inField.Trim().ToUpper() == SourceRow.Table.Columns[i].ColumnName.Trim().ToUpper())
                            {
                                xflag = false;
                                break;
                            }
                            else
                            {
                                xflag = true;
                            }
                        }
                    }
                }
                // end

                if (xflag == false)
                {
                    paramNumber++;
                    paramName = "@P" + paramNumber.ToString().Trim();

                    if (upFlds.Trim() == "")
                        upFlds += "[" + SourceRow.Table.Columns[i].ColumnName.Trim() + "] = ";
                    else
                        upFlds += ",[" + SourceRow.Table.Columns[i].ColumnName.Trim() + "] = ";

                    switch (SourceRow.Table.Columns[i].DataType.ToString().Trim().ToUpper())
                    {
                        case "SYSTEM.STRING":
                            upFlds += paramName;
                            sqlCmd.Parameters.Add(new SqlParameter(paramName, SqlDbType.VarChar));
                            sqlCmd.Parameters[paramName].Value = Convert.ToString(SourceRow[SourceRow.Table.Columns[i]]).Trim();

                            break;
                        case "SYSTEM.DECIMAL":
                            upFlds += paramName;
                            sqlCmd.Parameters.Add(new SqlParameter(paramName, SqlDbType.Decimal));
                            sqlCmd.Parameters[paramName].Value = (!DBNull.Value.Equals(SourceRow[SourceRow.Table.Columns[i]]) ?
                                       Convert.ToDecimal(SourceRow[SourceRow.Table.Columns[i]]) : 0);
                            break;
                        case "SYSTEM.SINGLE":
                        case "SYSTEM.INT16":
                            upFlds += paramName;
                            sqlCmd.Parameters.Add(new SqlParameter(paramName, SqlDbType.Int));
                            sqlCmd.Parameters[paramName].Value = (!DBNull.Value.Equals(SourceRow[SourceRow.Table.Columns[i]]) ?
                                       Convert.ToInt16(SourceRow[SourceRow.Table.Columns[i]]) : 0);
                            break;
                        case "SYSTEM.INT32":
                            upFlds += paramName;
                            sqlCmd.Parameters.Add(new SqlParameter(paramName, SqlDbType.Int));
                            sqlCmd.Parameters[paramName].Value = (!DBNull.Value.Equals(SourceRow[SourceRow.Table.Columns[i]]) ?
                                       Convert.ToInt32(SourceRow[SourceRow.Table.Columns[i]]) : 0);
                            break;
                        case "SYSTEM.INT64":
                            upFlds += paramName;
                            sqlCmd.Parameters.Add(new SqlParameter(paramName, SqlDbType.Decimal));
                            sqlCmd.Parameters[paramName].Value = (!DBNull.Value.Equals(SourceRow[SourceRow.Table.Columns[i]]) ?
                                       Convert.ToInt64(SourceRow[SourceRow.Table.Columns[i]]) : 0);

                            break;
                        case "SYSTEM.DATETIME":
                            upFlds += paramName;
                            sqlCmd.Parameters.Add(new SqlParameter(paramName, SqlDbType.DateTime));
                            sqlCmd.Parameters[paramName].Value = (!DBNull.Value.Equals(SourceRow[SourceRow.Table.Columns[i]]) ? Convert.ToDateTime(SourceRow[SourceRow.Table.Columns[i]]) : Convert.ToDateTime("01/01/1900"));
                            break;
                        case "SYSTEM.BOOLEAN":
                            upFlds += paramName;
                            sqlCmd.Parameters.Add(new SqlParameter(paramName, SqlDbType.Bit));
                            sqlCmd.Parameters[paramName].Value = (!DBNull.Value.Equals(SourceRow[SourceRow.Table.Columns[i]]) ?
                                (Convert.ToBoolean(SourceRow[SourceRow.Table.Columns[i]]) == true ? 1 : 0) : 0);
                            break;
                    }
                } // end (xFlag == false)
            } // end For endfor
            if (keyFields != null)
            {
                string condFld = "";
                upSqlStr += "update " + TargetTable.Trim() + " set " + upFlds.Trim() +
                            " where ";

                foreach (string keyField in keyFields)
                {
                    if (keyField != null)
                    {
                        if (condFld == "")
                            condFld = keyField.Trim().ToUpper() + " = ";
                        else
                            condFld += " and " + keyField.Trim().ToUpper() + " = ";

                        switch (SourceRow.Table.Columns[keyField.Trim()].DataType.ToString().Trim().ToUpper())
                        {
                            case "SYSTEM.STRING":
                                condFld += "'" + Convert.ToString(SourceRow[keyField.Trim()]).Trim() + "'";
                                break;
                            case "SYSTEM.DECIMAL":
                                condFld += Convert.ToDecimal(SourceRow[keyField.Trim()]);
                                break;
                            case "SYSTEM.INT16":
                                condFld += Convert.ToInt16(SourceRow[keyField.Trim()]);
                                break;
                            case "SYSTEM.INT32":
                                condFld += Convert.ToInt32(SourceRow[keyField.Trim()]);
                                break;
                            case "SYSTEM.INT64":
                                condFld += Convert.ToInt64(SourceRow[keyField.Trim()]);
                                break;
                            case "SYSTEM.DATETIME":
                                condFld += "'" + Convert.ToDateTime(SourceRow[keyField.Trim()]) + "'";
                                break;
                            case "SYSTEM.BOOLEAN":
                                condFld += Convert.ToBoolean(SourceRow[keyField.Trim()]) == true ?
                                    1 : 0;
                                break;
                        }
                    }
                }

                upSqlStr += condFld.Trim();
            }
            else
            {
                upSqlStr += "update " + TargetTable.Trim() + " set " + upFlds.Trim() +
                (cond.Trim() != "" ? " where " + cond.Trim() : "");
            }
            sqlCmd.CommandText = upSqlStr;
            return sqlCmd;

        }

    }

}