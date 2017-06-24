using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DataAccess_Net;
using System.Text.RegularExpressions;
using udEvaluate;    // Added by Sachin N. S. on 09/03/2016 for Bug-27503


namespace PointofSale
{
    public class clsInsUpdDelPrint : iEvalFunctions
    {
        private static clsDataAccess _oDataAccess;
        public static DataSet _commonDs;
        public static string _retMsg = "";
        private static Evaluator ev;        // Added by Sachin N. S. on 09/03/2016 for Bug-27503
        //private static DataTable _dtTaxChrgs;

        #region Default Constructor
        public clsInsUpdDelPrint()
        {
            // Default Constructor
        }

        #endregion Default Constructor

        #region Private Methods

        public iEvalFunctions InheritedFunctions()
        {
            return null;
        }
        
        public static bool InsertRecords()
        {
            _retMsg = "Record cannot be saved.";
            int _tran_cd;
            string _invNo;
            try
            {
                _oDataAccess = new clsDataAccess();
                _oDataAccess.BeginTransaction();

                _invNo = clsInsUpdDelPrint.GenerateInvNo(_commonDs.Tables["Lcode"].Rows[0]["Entry_ty"].ToString(), CommonInfo.InvSeries, Convert.ToDateTime(_commonDs.Tables["Main"].Rows[0]["Date"]));
                UpdateSecondaryKey(_commonDs.Tables["Main"], "Inv_no", _invNo);
                UpdateSecondaryKey(_commonDs.Tables["Item"], "Inv_no", _invNo);
                UpdateSecondaryKey(_commonDs.Tables["PSPAYDETAIL"], "Inv_no", _invNo);

                _tran_cd = InsertIntoTable(_commonDs.Tables["Main"], "DCMAIN", false, "TRAN_CD");
                UpdateSecondaryKey(_commonDs.Tables["Main"], "Tran_cd", _tran_cd);
                if (_tran_cd > 0)
                {
                    UpdateSecondaryKey(_commonDs.Tables["Item"], "Tran_cd", _tran_cd);
                    //if (InsertIntoTable(_commonDs.Tables["Item"], "DCITEM", true,"") > 0)
                    if (InsertIntoTable(_commonDs.Tables["Item"], "DCITEM", true, "TAXPER") > 0)
                    {
                        UpdateSecondaryKey(_commonDs.Tables["PSPAYDETAIL"], "Tran_cd", _tran_cd);
                        if (InsertIntoTable(_commonDs.Tables["PSPAYDETAIL"], "PSPAYDETAIL", true, "") > 0)
                        {
                            _oDataAccess.CommitTransaction();
                            _retMsg = "Record saved successfully...!!!";
                            _retMsg += "\nPrinting bill : " + _invNo.ToString();
                        }
                        else
                        {
                            _oDataAccess.RollbackTransaction();
                            _retMsg = "Cannot save the record.";
                            return false;
                        }
                    }
                    else
                    {
                        _oDataAccess.RollbackTransaction();
                        _retMsg = "Cannot save the record.";
                        return false;
                    }
                }
                else
                {
                    _oDataAccess.RollbackTransaction();
                    _retMsg = "Cannot save the record.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                _oDataAccess.RollbackTransaction();
                _retMsg = ex.Message;
                return false;
            }
            return true;
        }

        private static int InsertIntoTable(DataTable _dsTable, string _actTblName, bool lUpdate, string _excludeFld)
        {
            string _csql;
            int _retInt = 0;
            List<clsParam> _clsParam;
            foreach (DataRow _dr in _dsTable.Rows)
            {
                if (_dsTable.TableName == "Item")
                {
                    _retInt = UpdateItBalandItBalW(Convert.ToInt16(_dr["It_Code"]), _dr["Item"].ToString(), Convert.ToDecimal(_dr["Qty"]));
                }
                else
                {
                    _retInt = 1;
                }
                if (_retInt > 0)
                {
                    _clsParam = new List<clsParam>();
                    _csql = GetInsertString(_dr, _actTblName, _clsParam, _excludeFld);
                    _csql = "Set DateFormat DMY " + _csql;
                    _retInt = _oDataAccess.ExecuteSQLStatement(_csql, _clsParam, 25, lUpdate);
                }
            }
            return _retInt;
        }

        private static void UpdateSecondaryKey(DataTable _dt, string _DetRefKey, int _value)
        {
            foreach (DataRow _dr in _dt.Rows)
            {
                _dr[_DetRefKey] = _value;
            }
        }

        private static void UpdateSecondaryKey(DataTable _dt, string _DetRefKey, string _value)
        {
            foreach (DataRow _dr in _dt.Rows)
            {
                _dr[_DetRefKey] = _value;
            }
        }

        //***** Added by Sachin N. S. on 20/01/2016 for Bug-27503 -- Start ******//
        public static bool CancelRecords(int _iTranCd)
        {
            _retMsg = "Record cannot be saved.";
            bool _lCancel = true;
            try
            {
                _oDataAccess = new clsDataAccess();

                string _csql = "Select Ac_id from Ac_mast where Ac_name='CANCELLED.'";
                DataRow _dr = _oDataAccess.GetDataRow(_csql, null, 25);

                if (_dr != null)
                {
                    _csql = "Update DcMain set ";
                    _csql += "Party_nm= 'CANCELLED.', ";
                    _csql += "Ac_Id = "+_dr["Ac_Id"].ToString()+", ";
                    _csql += "Gro_Amt = 0, ";
                    _csql += "Tot_dedUC = 0, ";
                    _csql += "Tot_Add = 0, ";
                    _csql += "TaxAmt = 0, ";
                    _csql += "Tot_Tax = 0, ";
                    _csql += "Net_amt = 0, ";
                    _csql += "Tot_NonTax = 0, ";
                    _csql += "Tot_FDisc = 0, ";
                    _csql += "BalAmt = 0, ";
                    _csql += "TotalPaid = 0, ";
                    _csql += "FDiscAmt = 0 ";
                    _csql += "where Entry_ty='PS' and Tran_cd=" + _iTranCd.ToString();
                }
                else
                {
                    _retMsg = "'CANCELLED.' Party not defined in the Account master. ";
                    return false;
                }

                _oDataAccess.BeginTransaction();

                _lCancel = UpdateIntoTable(_csql);
                if (_lCancel == true)
                {
                    _lCancel = DeleteFromTable("DCITEM", _iTranCd);
                    if (_lCancel == true)
                    {
                        _lCancel = DeleteFromTable("PSPAYDETAIL", _iTranCd);
                    }
                }

                if (_lCancel == true)
                {
                    _oDataAccess.CommitTransaction();
                    _retMsg = "Record Cancelled successfully...!!!";
                }
                else
                {
                    _oDataAccess.RollbackTransaction();
                    _retMsg = "Cannot save the record.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                _oDataAccess.RollbackTransaction();
                _retMsg = ex.Message;
                return false;
            }
            return true;
        }

        private static bool UpdateIntoTable(string _csql)
        {
            int _retInt = 0;
            _csql = "Set DateFormat DMY " + _csql;
            _retInt = _oDataAccess.ExecuteSQLStatement(_csql, null, 25, true);
            return (_retInt > 0 ? true : false);
        }

        private static bool DeleteFromTable(string _actTblName, int _iTranCd)
        {
            string _csql;
            int _retInt = 0;
            _csql = "Delete from " + _actTblName + " Where Entry_ty='PS' and Tran_cd=" + _iTranCd.ToString();
            _csql = "Set DateFormat DMY " + _csql;
            _retInt = _oDataAccess.ExecuteSQLStatement(_csql, null, 25, true);
            return (_retInt > 0 ? true : false);
        }

        private static string GetUpdateString(DataRow _dr, string _tblName, List<clsParam> _clparam, string _xcludeFld)
        {
            DataTable _dt = _dr.Table;
            string _sql = BuildUpdateSQL(_dt, _tblName, _xcludeFld);

            clsParam _objparam;
            clsParam.pType paramType = clsParam.pType.pString;
            string _fldNm = "";
            foreach (DataColumn _dc in _dt.Columns)
            {
                _fldNm = _dc.ColumnName;
                if (_xcludeFld.IndexOf(_fldNm.ToUpper()) != -1)
                {
                    continue;
                }

                switch (_dc.DataType.ToString().ToLower())
                {
                    case "varchar":
                        paramType = clsParam.pType.pString;
                        break;
                    case "decimal":
                        paramType = clsParam.pType.pFloat;
                        break;
                    case "bit":
                        paramType = clsParam.pType.pLong;
                        break;
                    case "datetime":
                        paramType = clsParam.pType.pDate;
                        break;
                    case "text":
                        paramType = clsParam.pType.pString;
                        break;
                    default:
                        paramType = clsParam.pType.pString;
                        break;
                }
                _objparam = new clsParam(_dc.ColumnName, _dr[_fldNm], paramType, _clparam, false, null, clsParam.pInOut.pIn);
            }
            return _sql;
        }

        private static string BuildUpdateSQL(DataTable _dt, string _tblName, string _xcludeFld)
        {
            StringBuilder _InsSql = new StringBuilder("Update " + _tblName + " ");
            StringBuilder _ValSql = new StringBuilder(" Set ");
            bool _FirstRec = true;
            string _colName = "";
            foreach (DataColumn _dc in _dt.Columns)
            {
                _colName = _dc.ColumnName.ToUpper();
                if (_xcludeFld.IndexOf(_colName) != -1)
                {
                    continue;
                }

                if (_FirstRec)
                {
                    _FirstRec = false;
                }
                else
                {
                    _InsSql.Append(", ");
                    _ValSql.Append(", ");
                }

                _ValSql.Append("[" + _dc.ColumnName + "] = ?");
            }

            _InsSql.Append(_ValSql.ToString());
            _InsSql.Append("Where Tran_cd = " + _dt.Rows[0]["Tran_Cd"].ToString());

            return _InsSql.ToString();
        }
        //***** Added by Sachin N. S. on 20/01/2016 for Bug-27503 -- End ******//

        private static string BuildInsertSQL(DataTable _dt, string _tblName, string _xcludeFld)
        {
            StringBuilder _InsSql = new StringBuilder("Insert into " + _tblName + " ( ");
            StringBuilder _ValSql = new StringBuilder(" Values (");
            bool _FirstRec = true;
            bool _isIdentity = false;
            string _IdentityType = "";
            string _colName = "";
            foreach (DataColumn _dc in _dt.Columns)
            {
                _colName = _dc.ColumnName.ToUpper();
                if (_xcludeFld.IndexOf(_colName) != -1)
                {
                    continue;
                }

                if (_dc.AutoIncrement)
                {
                    _isIdentity = true;

                    switch (_dc.DataType.Name)
                    {
                        case "Int16":
                            _IdentityType = "smallint";
                            break;
                        case "SByte":
                            _IdentityType = "tinyint";
                            break;
                        case "Int64":
                            _IdentityType = "bigint";
                            break;
                        case "Decimal":
                            _IdentityType = "decimal";
                            break;
                        default:
                            _IdentityType = "int";
                            break;
                    }
                }
                else
                {
                    if (_FirstRec)
                    {
                        _FirstRec = false;
                    }
                    else
                    {
                        _InsSql.Append(", ");
                        _ValSql.Append(", ");
                    }

                    _InsSql.Append("[" + _dc.ColumnName + "]");
                    _ValSql.Append("?");
                    //_ValSql.Append("@");
                    //_ValSql.Append(_dc.ColumnName);
                }
            }

            _InsSql.Append(")");
            _ValSql.Append(")");
            _InsSql.Append(_ValSql.ToString());

            if (_isIdentity)
            {
                _InsSql.Append("; Select cast(scope_identity() as ");
                _InsSql.Append(_IdentityType);
                _InsSql.Append(")");
            }

            return _InsSql.ToString();
        }

        private void AddParameter(SqlCommand _SqlCmd, string paramName, string fieldName, object value)
        {
            SqlParameter _sqlPara = new SqlParameter(paramName, value);

            _sqlPara.Direction = ParameterDirection.Input;
            _sqlPara.ParameterName = paramName;
            _sqlPara.SourceColumn = fieldName;
            _sqlPara.SourceVersion = DataRowVersion.Current;

            _SqlCmd.Parameters.Add(_sqlPara);
        }

        private static string GetInsertString(DataRow _dr, string _tblName, List<clsParam> _clparam, string _xcludeFld)
        {
            DataTable _dt = _dr.Table;
            string _sql = BuildInsertSQL(_dt, _tblName, _xcludeFld);

            clsParam _objparam;
            clsParam.pType paramType = clsParam.pType.pString;
            string _fldNm = "";
            foreach (DataColumn _dc in _dt.Columns)
            {
                _fldNm = _dc.ColumnName;
                if (_xcludeFld.IndexOf(_fldNm.ToUpper()) != -1)
                {
                    continue;
                }

                switch (_dc.DataType.ToString().ToLower())
                {
                    case "varchar":
                        paramType = clsParam.pType.pString;
                        break;
                    case "decimal":
                        paramType = clsParam.pType.pFloat;
                        break;
                    case "bit":
                        paramType = clsParam.pType.pLong;
                        break;
                    case "datetime":
                        paramType = clsParam.pType.pDate;
                        break;
                    case "text":
                        paramType = clsParam.pType.pString;
                        break;
                    default:
                        paramType = clsParam.pType.pString;
                        break;
                }
                _objparam = new clsParam(_dc.ColumnName, _dr[_fldNm], paramType, _clparam, false, null, clsParam.pInOut.pIn);
            }

            //_clparam.Add(new clsParam { FieldName = "@Entry_ty1", Value = _dc });
            //_clparam.Add(new clsParam { FieldName = "@Tran_cd", Value = 0 });

            return _sql;
        }

        private static string GenerateInvNo(string _EntType, string _InvSr, DateTime _InvDt)
        {
            //Parameter ventryType, vInvoiceSeries, vInvoiceNo,VentDate, voldInvoiceSeries, voldInvoiceNo,vinv_size	
            int _InvNo = 1;
            string v_i_s_type = "";
            string v_i_prefix = "";
            string v_i_suffix = "";
            string v_i_middle = "";
            string vctrYear = "";
            string csql = "", cCond = "";
            string minv_no = "";
            string v_i_MnthFormat = "";

            //****** Added by Sachin N. S. on 09/03/2016 for Bug-27503 -- Start
            ev = new Evaluator(udEvaluate.eParserSyntax.cSharp, false);
            //ev.AddEnvironmentFunctions(this);
            ev.AddEnvironmentFunctions(new EvalFunctions());
            //****** Added by Sachin N. S. on 09/03/2016 for Bug-27503 -- End

            DataRow _dr = _oDataAccess.GetDataRow("Select Top 1 * from Series where Inv_sr = '" + _InvSr + "' ", null, 20);
            if (_dr.Table.Rows.Count > 0)
            {
                opCode lcode = null;        // Added by Sachin N. S. on 09/03/2016 for Bug-27503
                object res;                 // Added by Sachin N. S. on 09/03/2016 for Bug-27503
                v_i_s_type = _dr["S_Type"].ToString().TrimEnd();
                v_i_MnthFormat = _dr["MnthFormat"].ToString().TrimEnd();
                if (_dr["I_prefix"].ToString().TrimEnd() != "")
                {
                    //*****  Added by Sachin N. S. on 09/03/2016 for Bug-27503 ******* -- Start
                    lcode = ev.Parse(_dr["I_prefix"].ToString().TrimEnd());     
                    res = lcode.value;
                    if (res != null)
                    {
                        v_i_prefix = Evaluator.ConvertToString(res);
                    }
                    //v_i_prefix = _dr["I_prefix"].ToString().TrimEnd();
                    //*****  Added by Sachin N. S. on 09/03/2016 for Bug-27503 ******* -- End
                }
                if (_dr["I_suffix"].ToString().TrimEnd() != "")
                {
                    //*****  Added by Sachin N. S. on 09/03/2016 for Bug-27503 ******* -- Start
                    lcode = null;
                    res = null;
                    lcode = ev.Parse(_dr["I_suffix"].ToString().TrimEnd());
                    res = lcode.value;
                    if (res != null)
                    {
                        v_i_suffix = Evaluator.ConvertToString(res);
                    }
                    //v_i_suffix = _dr["I_suffix"].ToString().TrimEnd();
                    //*****  Added by Sachin N. S. on 09/03/2016 for Bug-27503 ******* -- End
                    
                }

                switch (v_i_s_type)
                {
                    case "DAYWISE":
                        v_i_middle = _InvDt.ToString("ddMMyy");
                            //_InvDt.Day.ToString().PadLeft(2, '0') + _InvDt.Month.ToString().PadLeft(2, '0') + _InvDt.Year.ToString().Substring(3);
                        break;
                    case "MONTHWISE":
                        v_i_middle = genMnthWiseFormat(_InvDt,v_i_MnthFormat);
                        break;
                }
            }

            if (_InvDt >= Convert.ToDateTime(CommonInfo.StartDate) && _InvDt <= Convert.ToDateTime(CommonInfo.EndDate))
            {
                vctrYear = CommonInfo.L_yn;
            }

            string cInsGen_Inv = "Set dateformat dmy;";
            cInsGen_Inv += "Insert into Gen_Inv (Entry_ty, Inv_Sr, Inv_No, l_yn, Inv_dt, CompId) ";
            cInsGen_Inv += "values ('" + _EntType + "', '" + _InvSr + "', '" + _InvNo + "', '" + CommonInfo.L_yn + "', '" + _InvDt + "', " + CommonInfo.CompId.ToString() + ")";

            string cInsGen_Miss = "Set dateformat dmy;";
            cInsGen_Miss += "Insert into Gen_Miss (Entry_ty, Inv_Sr, Inv_No, Flag, l_yn, Inv_dt, user_name, CompId) ";
            cInsGen_Miss += "values ('" + _EntType + "', '" + _InvSr + "', '" + _InvNo + "', 'Y', '" + CommonInfo.L_yn + "', '" + _InvDt + "', '', " + CommonInfo.CompId.ToString() + ")";

            bool _loop = true;
            do
            {
                switch (v_i_s_type)
                {
                    case "DAYWISE":
                        csql = "Set dateformat dmy;";
                        csql += "Select Top 1 Inv_no from Gen_inv with (TABLOCKX) ";
                        cCond = " where Entry_ty = '" + _EntType + "' And Inv_sr = '" + _InvSr + "' And Inv_dt = '" + _InvDt + "' ";
                        break;
                    case "MONTHWISE":
                        csql = "Set dateformat dmy;";
                        csql += "Select Top 1 Inv_no from Gen_inv with (TABLOCKX) ";
                        cCond = " where Entry_ty = '" + _EntType + "' And Inv_sr = '" + _InvSr + "' And MONTH(Inv_dt) = " + _InvDt.Month.ToString() + " And Year(Inv_dt) = " + _InvDt.Year.ToString();
                        break;
                    default:
                        csql = "Set dateformat dmy;";
                        csql = "Select Top 1 Inv_no from Gen_inv with (TABLOCKX) ";
                        cCond = " where Entry_ty = '" + _EntType + "' And Inv_sr = '" + _InvSr + "' And L_yn = '" + CommonInfo.L_yn + "' ";
                        break;
                }
                csql += cCond;

                _dr = _oDataAccess.GetDataRow(csql, null, 20);

                if (_dr == null)
                {
                    _oDataAccess.ExecuteSQLStatement(cInsGen_Inv, null, 25, true);
                }
                else
                {
                    if (_InvNo <= Convert.ToInt16(_dr["Inv_no"]))
                    {
                        _InvNo = Convert.ToInt16(_dr["Inv_no"]) + 1;
                        continue;
                    }
                    else
                    {
                        csql = "Update Gen_Inv set Inv_no = " + _InvNo.ToString() + " ";
                        csql += cCond;
                        _oDataAccess.ExecuteSQLStatement(csql, null, 25, true);
                    }
                }

                switch (v_i_s_type)
                {
                    case "DAYWISE":
                        csql = "Set dateformat dmy;";
                        csql += " Select Top 1 Flag from Gen_miss ";
                        cCond = " where Entry_ty = '" + _EntType + "' And Inv_sr = '" + _InvSr + "' And Inv_no = '" + _InvNo + "' And Inv_dt = '" + Convert.ToString(_InvDt) + "' ";
                        break;
                    case "MONTHWISE":
                        csql = "Set dateformat dmy;";
                        csql += " Select Top 1 Flag from Gen_miss ";
                        cCond = " where Entry_ty = '" + _EntType + "' And Inv_sr = '" + _InvSr + "' And Inv_no = '" + _InvNo + "' And MONTH(Inv_dt) = " + _InvDt.Month.ToString() + " And Year(Inv_dt) = " + _InvDt.Year.ToString() + " ";
                        break;
                    default:
                        csql = "Set dateformat dmy;";
                        csql += " Select Top 1 Flag from Gen_miss ";
                        cCond = " where Entry_ty = '" + _EntType + "' And Inv_sr = '" + _InvSr + "' And Inv_no = '" + _InvNo + "' And L_yn = '" + CommonInfo.L_yn + "' ";
                        break;
                }
                csql += cCond;
                _dr = _oDataAccess.GetDataRow(csql, null, 20);

                if (_dr == null)
                {
                    cInsGen_Inv = "set dateformat dmy;";
                    cInsGen_Inv += "Insert into Gen_Miss (Entry_ty, Inv_sr, Inv_no, flag, l_yn, Inv_dt, user_name, Compid) ";
                    cInsGen_Inv += " values('" + _EntType + "', '" + _InvSr + "', '" + _InvNo + "', 'Y', '" + CommonInfo.L_yn + "', '" + _InvDt.ToString() + "', '', '" + CommonInfo.CompId + "' ) ";
                    _oDataAccess.ExecuteSQLStatement(cInsGen_Inv, null, 25, true);
                }
                else
                {
                    if (_dr["Flag"].ToString() == "N")
                    {
                        cInsGen_Inv = "set dateformat dmy;";
                        cInsGen_Inv += "Update Gen_Miss set Inv_no='" + _InvNo + "', flag='Y', Inv_dt='" + _InvDt + "' ";
                        cInsGen_Inv += cCond;
                        _oDataAccess.ExecuteSQLStatement(cInsGen_Inv, null, 25, true);
                    }
                }
                int invSize = Convert.ToInt16(_commonDs.Tables["Lcode"].Rows[0]["InvNo_Size"]);
                minv_no = _InvNo.ToString().PadLeft(invSize, '0');
                minv_no = (v_i_prefix + v_i_middle + minv_no + v_i_suffix).PadRight(invSize, ' ');
                csql = "Select Top 1 Entry_ty from DCMAIN where Entry_ty = '" + _EntType + "' And Inv_sr = '" + _InvSr + "' And Inv_no = '" + _InvNo + "' And L_yn = '" + CommonInfo.L_yn + "' ";
                _dr = _oDataAccess.GetDataRow(csql, null, 20);

                if (_dr == null)
                {
                    _loop = false;
                    break;
                }
                else
                {
                    _InvNo += 1;
                }

            } while (_loop == true);

            return minv_no;
        }

        private static int UpdateItBalandItBalW(int _It_Code, string _It_Name, decimal _qty)
        {
            string _csql;
            DataRow _dr;
            DataTable _dt;
            int _retVal = 0;
            List<clsParam> _clsParam = new List<clsParam>();

            // Updating It_Bal -- Start
            _csql = "Select top 1 It_code,It_name From It_bal where It_code = " + _It_Code.ToString() + "";
            _dr = _oDataAccess.GetDataRow(_csql, null, 25);
            if (_dr == null)    // Insert in It_Bal
            {
                _csql = "Select top 1 * From It_bal where 1=2";
                _dt = _oDataAccess.GetDataTable(_csql, null, 25);
                _dr = clsInsertDefaultValue.AddNewRow(_dt);
                _dt.Rows.Add(_dr);
                _dt.Rows[0]["It_code"] = _It_Code.ToString();
                _dt.Rows[0]["It_Name"] = _It_Name.ToString();
                _dt.Rows[0]["DCQTY"] = _qty.ToString();

                if (InsertIntoTable(_dt, "IT_BAL", true, "") > 0)
                {
                    _retVal = 1;
                }
            }
            else   // Update It_Bal
            {
                _csql = "Update It_Bal set DCQTY=ISNULL(DCQTY,0)+" + _qty.ToString();
                _csql += " where It_code = " + _It_Code.ToString();
                _retVal = _oDataAccess.ExecuteSQLStatement(_csql, null, 25, true);
            }
            // Updating It_Bal -- End

            //Updating It_BalW -- Start
            if (_retVal > 0)
            {
                _csql = "Set Dateformat DMY;";
                _csql += "Select top 1 It_code From It_balW ";
                _csql += " where It_code = " + _It_Code.ToString();
                _csql += " and Entry_ty = '" + _commonDs.Tables["Lcode"].Rows[0]["Entry_Ty"].ToString() + "'";
                _csql += " and Date = '" + _commonDs.Tables["Main"].Rows[0]["Date"].ToString() + "'";
                _csql += " and [Rule] = 'MODVATABLE'";
                _dr = _oDataAccess.GetDataRow(_csql, null, 25);
                if (_dr == null)    // Insert in It_Bal
                {
                    _csql = "Select top 1 * From It_balW where 1=2";
                    _dt = _oDataAccess.GetDataTable(_csql, null, 25);
                    _dr = clsInsertDefaultValue.AddNewRow(_dt);
                    _dt.Rows.Add(_dr);
                    _dt.Rows[0]["It_code"] = _It_Code.ToString();
                    _dt.Rows[0]["Entry_ty"] = _commonDs.Tables["Lcode"].Rows[0]["Entry_Ty"].ToString();
                    _dt.Rows[0]["Date"] = _commonDs.Tables["Main"].Rows[0]["Date"].ToString();
                    _dt.Rows[0]["Rule"] = "MODVATABLE";
                    _dt.Rows[0]["QTY"] = _qty.ToString();

                    if (InsertIntoTable(_dt, "IT_BALW", true, "") > 0)
                    {
                        _retVal = 1;
                    }
                }
                else   // Update It_Bal
                {
                    _csql = "Set Dateformat DMY;";
                    _csql += "Update It_BalW set QTY=ISNULL(QTY,0)+" + _qty.ToString();
                    _csql += " where It_code = " + _It_Code.ToString();
                    _csql += " and Entry_ty = '" + _commonDs.Tables["Lcode"].Rows[0]["Entry_Ty"].ToString() + "'";
                    _csql += " and Date = '" + _commonDs.Tables["Main"].Rows[0]["Date"].ToString() + "'";
                    _csql += " and [Rule] = 'MODVATABLE'";
                    _retVal = _oDataAccess.ExecuteSQLStatement(_csql, null, 25, true);
                }
            }
            //Updating It_BalW -- End
            return _retVal;
        }

        private static string genMnthWiseFormat(DateTime _EntDate,string cMnthFormat)
        {
            string cRetVal="";
            cRetVal = _EntDate.ToString("MMyy");
            cMnthFormat = cMnthFormat.TrimEnd();

            cMnthFormat = cMnthFormat.Replace("YYYY", "yyyy").Replace("YY", "yy").Replace("Mon", "MMM");

            cRetVal = cMnthFormat != "" ? _EntDate.ToString(cMnthFormat) : cRetVal;

            return cRetVal;
        }

        // ******** Added by Sachin N. S. on 09/08/2016 for Bug-27503 -- Start
        public static Boolean CheckInvNoLength(string _EntType, string _InvSr)
        {
            _oDataAccess = new clsDataAccess();
            int _InvNo = 1;
            string v_i_s_type = "";
            string v_i_prefix = "";
            string v_i_suffix = "";
            string v_i_middle = "";
            string vctrYear = "";
            string csql = "", cCond = "";
            string minv_no = "";
            string v_i_MnthFormat = "";

            ev = new Evaluator(udEvaluate.eParserSyntax.cSharp, false);
            ev.AddEnvironmentFunctions(new EvalFunctions());

            DataRow _dr = _oDataAccess.GetDataRow("Select Top 1 * from Series where Inv_sr = '" + _InvSr + "' ", null, 20);
            if (_dr.Table.Rows.Count > 0)
            {
                opCode lcode = null;        
                object res;                 
                v_i_s_type = _dr["S_Type"].ToString().TrimEnd();
                v_i_MnthFormat = _dr["MnthFormat"].ToString().TrimEnd();
                if (_dr["I_prefix"].ToString().TrimEnd() != "")
                {
                    lcode = ev.Parse(_dr["I_prefix"].ToString().TrimEnd());
                    res = lcode.value;
                    if (res != null)
                    {
                        v_i_prefix = Evaluator.ConvertToString(res);
                    }
                }
                if (_dr["I_suffix"].ToString().TrimEnd() != "")
                {
                    lcode = null;
                    res = null;
                    lcode = ev.Parse(_dr["I_suffix"].ToString().TrimEnd());
                    res = lcode.value;
                    if (res != null)
                    {
                        v_i_suffix = Evaluator.ConvertToString(res);
                    }
                }

                switch (v_i_s_type)
                {
                    case "DAYWISE":
                        v_i_middle = DateTime.Now.ToString("ddMMyy");
                        break;
                    case "MONTHWISE":
                        v_i_middle = genMnthWiseFormat(DateTime.Now, v_i_MnthFormat);
                        break;
                }
            }

            DataRow dr = _oDataAccess.GetDataRow("Select InvNo_Size from Lcode where Entry_ty='PS'", null, 20);

            int invSize = Convert.ToInt16(dr[0].ToString());
            minv_no = _InvNo.ToString().PadLeft(invSize, '0');
            invSize += (v_i_prefix.Length + v_i_middle.Length  + v_i_suffix.Length);

            dr = _oDataAccess.GetDataRow("select MAX_LENGTH from sys.columns where object_id=object_id('DCMAIN') AND [NAME]='INV_NO'", null, 20);

            int maxlength = Convert.ToInt16(dr[0]);
            return invSize > maxlength ? false : true;
        }
        // ******** Added by Sachin N. S. on 09/08/2016 for Bug-27503 -- End
        #endregion Private Methods

    }
}
