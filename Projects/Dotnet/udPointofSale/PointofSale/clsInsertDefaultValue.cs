using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Linq;
using System.Data.Linq;
using DataAccess_Net;


namespace PointofSale
{
    public class clsInsertDefaultValue
    {
        static clsDataAccess _oDataAccess;

        private static DataSet _dataSet;

        public static DataSet _DataSet
        {
            set { _dataSet = value; }
        }

        public static void InsertDefVal_Main(int _recNo)
        {
            _oDataAccess = new clsDataAccess();

            DataTable _dt = new DataTable();
            string cSql = "";
            cSql = "EXECUTE USP_MAIN_STANDARD_FIELDS '" + _dataSet.Tables["LCode"].Rows[0]["Entry_ty"] + "'";
            _dt = _oDataAccess.GetDataTable(cSql, null, 50);

            _dataSet.Tables["Main"].Rows[_recNo]["Entry_ty"] = _dataSet.Tables["LCode"].Rows[0]["Entry_ty"];
            _dataSet.Tables["Main"].Rows[_recNo]["date"] = _dt.Rows[0]["CURDATE"];
            _dataSet.Tables["Main"].Rows[_recNo]["doc_no"] = _dt.Rows[0]["DOCNO"];
            _dataSet.Tables["Main"].Rows[_recNo]["dept"] = CommonInfo.Department;
            _dataSet.Tables["Main"].Rows[_recNo]["cate"] = CommonInfo.Category;
            //_dataSet.Tables["Main"].Rows[_recNo]["party_nm"] = "CASH SALES";
            _dataSet.Tables["Main"].Rows[_recNo]["party_nm"] = CommonInfo.PartyNm;    // Changed by Sachin N. S. on 19/06/2013 for Bug-14538
            _dataSet.Tables["Main"].Rows[_recNo]["inv_no"] = "";
            _dataSet.Tables["Main"].Rows[_recNo]["inv_sr"] = CommonInfo.InvSeries;
            _dataSet.Tables["Main"].Rows[_recNo]["l_yn"] = CommonInfo.L_yn;
            _dataSet.Tables["Main"].Rows[_recNo]["user_name"] = CommonInfo.UserName;
            _dataSet.Tables["Main"].Rows[_recNo]["sysdate"] = _dt.Rows[0]["SYSDATE"].ToString();
            _dataSet.Tables["Main"].Rows[_recNo]["tran_cd"] = 0;
            //_dataSet.Tables["Main"].Rows[_recNo]["ac_id"] = (int)_dt.Rows[0]["AC_ID"];
            _dataSet.Tables["Main"].Rows[_recNo]["ac_id"] = Convert.ToInt16(CommonInfo.PartyId);  // Changed by Sachin N. S. on 19/06/2013 for Bug-14538
            _dataSet.Tables["Main"].Rows[_recNo]["compid"] = CommonInfo.CompId;
            _dataSet.Tables["Main"].Rows[_recNo]["ApGen"] = "YES";
            _dataSet.Tables["Main"].Rows[_recNo]["ApGenBy"] = CommonInfo.UserName;
            _dataSet.Tables["Main"].Rows[_recNo]["ApLed"] = "YES";
            _dataSet.Tables["Main"].Rows[_recNo]["ApLedBy"] = CommonInfo.UserName;
            _dataSet.Tables["Main"].Rows[_recNo]["CntrCode"] = CommonInfo.CounterNm;      // Added by Sachin N. S. on 14/01/2016 for Bug-27503

            //***** Added by Sachin N. S. on 22/06/2013 for Bug-14538 -- Start *****//
            CommonInfo.ChngdWareHouse = CommonInfo.WareHouse;
            replDefaDiscChrgsValue("H", _recNo);
            if (Convert.ToInt16(CommonInfo.PartyId) > 0 && (Convert.ToBoolean(_dataSet.Tables["Lcode"].Rows[0]["Stax_Tran"]) == true || CommonInfo.HdrDiscFlds != ""))
            {
                GetPartyDefaDiscChrgsValue(CommonInfo.PartyId);
            }
            //***** Added by Sachin N. S. on 22/06/2013 for Bug-14538 -- End *****//
        }

        public static void InsertDefVal_Item(int _recNo)
        {
            _dataSet.Tables["Item"].Rows[_recNo]["Entry_ty"] = _dataSet.Tables["LCode"].Rows[0]["Entry_ty"];
            _dataSet.Tables["Item"].Rows[_recNo]["date"] = _dataSet.Tables["Main"].Rows[0]["Date"];
            _dataSet.Tables["Item"].Rows[_recNo]["doc_no"] = _dataSet.Tables["Main"].Rows[0]["Doc_no"];
            _dataSet.Tables["Item"].Rows[_recNo]["party_nm"] = _dataSet.Tables["Main"].Rows[0]["Party_nm"];
            _dataSet.Tables["Item"].Rows[_recNo]["ac_id"] = _dataSet.Tables["Main"].Rows[0]["AC_ID"];
            _dataSet.Tables["Item"].Rows[_recNo]["dept"] = CommonInfo.Department;
            _dataSet.Tables["Item"].Rows[_recNo]["cate"] = CommonInfo.Category;
            _dataSet.Tables["Item"].Rows[_recNo]["inv_sr"] = CommonInfo.InvSeries;
            _dataSet.Tables["Item"].Rows[_recNo]["l_yn"] = CommonInfo.L_yn;
            _dataSet.Tables["Item"].Rows[_recNo]["compid"] = CommonInfo.CompId;
            _dataSet.Tables["Item"].Rows[_recNo]["pmkey"] = _dataSet.Tables["LCode"].Rows[0]["Inv_Stk"].ToString();
            _dataSet.Tables["Item"].Rows[_recNo]["Ware_nm"] = CommonInfo.ChngdWareHouse;     // Changed by Sachin N. S. on 19/06/2013 for Bug-14538
            _dataSet.Tables["Item"].Rows[_recNo]["CntrCode"] = CommonInfo.CounterNm;        // Added by Sachin N. S. on 14/01/2016 for Bug-27503

            var MaxDoc = Convert.ToInt16(_dataSet.Tables["Item"].AsEnumerable().Where(row => row["Itserial"] != null && row["Itserial"].ToString() != "").Count()) + 1;
            _dataSet.Tables["Item"].Rows[_recNo]["itserial"] = MaxDoc.ToString().PadLeft(5, '0');
            MaxDoc = Convert.ToInt16(_dataSet.Tables["Item"].AsEnumerable().Where(row => row["Item_no"] != null && row["Item_no"].ToString() != "").DefaultIfEmpty().Max(row => row == null ? 0 : Convert.ToInt16(row["Item_no"]))) + 1;
            _dataSet.Tables["Item"].Rows[_recNo]["item_no"] = MaxDoc.ToString();

            _dataSet.Tables["Item"].Rows[_recNo]["inv_no"] = "";
            _dataSet.Tables["Item"].Rows[_recNo]["re_qty"] = 0;
            _dataSet.Tables["Item"].Rows[_recNo]["tran_cd"] = 0;
            _dataSet.Tables["Item"].Rows[_recNo]["it_code"] = 0;

            //***** Added by Sachin N. S. on 23/07/2013 for Bug-14538 -- Start *****//
            if (Convert.ToBoolean(_dataSet.Tables["Lcode"].Rows[0]["Stax_Item"]) == true)
            {
                _dataSet.Tables["Item"].Rows[_recNo]["Tax_Name"] = "NO-TAX";
                _dataSet.Tables["Item"].Rows[_recNo]["TaxAmt"] = 0;
            }
            replDefaDiscChrgsValue("D", _recNo);
            //***** Added by Sachin N. S. on 23/07/2013 for Bug-14538 -- End *****//
        }

        public static void InsertDefVal_PayDetail(int _recNo, string _paymode)
        {
            _dataSet.Tables["PSPayDetail"].Rows[_recNo]["Entry_ty"] = _dataSet.Tables["Main"].Rows[0]["Entry_ty"];
            _dataSet.Tables["PSPayDetail"].Rows[_recNo]["date"] = _dataSet.Tables["Main"].Rows[0]["Date"];
            _dataSet.Tables["PSPayDetail"].Rows[_recNo]["dept"] = _dataSet.Tables["Main"].Rows[0]["Dept"];
            _dataSet.Tables["PSPayDetail"].Rows[_recNo]["cate"] = _dataSet.Tables["Main"].Rows[0]["Cate"];
            _dataSet.Tables["PSPayDetail"].Rows[_recNo]["party_nm"] = _dataSet.Tables["Main"].Rows[0]["Party_nm"];
            _dataSet.Tables["PSPayDetail"].Rows[_recNo]["inv_no"] = "";
            _dataSet.Tables["PSPayDetail"].Rows[_recNo]["inv_sr"] = _dataSet.Tables["Main"].Rows[0]["Inv_sr"];
            _dataSet.Tables["PSPayDetail"].Rows[_recNo]["user_name"] = _dataSet.Tables["Main"].Rows[0]["User_name"];
            _dataSet.Tables["PSPayDetail"].Rows[_recNo]["sysdate"] = _dataSet.Tables["Main"].Rows[0]["SYSDATE"].ToString();
            _dataSet.Tables["PSPayDetail"].Rows[_recNo]["tran_cd"] = 0;
            _dataSet.Tables["PSPayDetail"].Rows[_recNo]["ac_id"] = (int)_dataSet.Tables["Main"].Rows[0]["AC_ID"];

            _dataSet.Tables["PSPayDetail"].Rows[_recNo]["PayMode"] = _paymode.ToString();
        }

        public static DataRow AddNewRow(DataTable _dt)
        {
            DataRow _dr = _dt.NewRow();
            for (int i = 0; i < _dt.Columns.Count; i++)
            {
                switch (_dt.Columns[i].DataType.ToString())
                {
                    case "System.Int16":
                        _dr[i] = Convert.ToInt16(0);
                        break;
                    case "System.Decimal":
                        _dr[i] = Convert.ToDecimal(0.00);
                        break;
                    case "System.Double":
                        _dr[i] = Convert.ToDouble(0.00);
                        break;
                    case "System.String":
                        _dr[i] = Convert.ToString("");
                        break;
                    case "System.Boolean":
                        _dr[i] = Convert.ToBoolean(false);
                        break;
                    case "System.DateTime":
                        _dr[i] = Convert.ToDateTime("01/01/1900");
                        break;
                }
            }
            return _dr;
        }

        //***** Added by Sachin N. S. on 22/06/2013 for Bug-14538 -- Start *****//
        public static void replNewvalueinChldTable(string tblName, string colName, object objValue)
        {
            foreach (DataRow _dr in _dataSet.Tables[tblName].Rows)
            {
                _dr[colName] = objValue;
            }
        }

        public static void replDefaDiscChrgsValue(string _hdrDtl,int _rowNo)
        {
            string _tblName = _hdrDtl == "H" ? "Main" : "Item";
            string _cCond = "Att_File = " + (_hdrDtl == "H" ? "1" : "0");

            DataRow[] _dr = _dataSet.Tables["DcMast"].Select(_cCond);
            foreach (DataRow _dr1 in _dr)
            {
                if (Convert.ToDecimal(_dr1["Def_Pert"]) > 0)
                {
                    _dataSet.Tables[_tblName].Rows[_rowNo][_dr1["Pert_Name"].ToString().Trim()] = Convert.ToDecimal(_dr1["Def_Pert"]);
                }
                if (Convert.ToDecimal(_dr1["Def_Amt"]) > 0)
                {
                    _dataSet.Tables[_tblName].Rows[_rowNo][_dr1["Fld_Nm"].ToString().Trim()] = Convert.ToDecimal(_dr1["Def_Amt"]);
                }
            }
        }

        public static void GetPartyDefaDiscChrgsValue(int _partyId)
        {
            string cSql;
            DataTable _dt = new DataTable();
            cSql = "Select " + CommonInfo.HdrDiscFlds + " from ac_mast where ac_id = " + _partyId.ToString();
            _dt = _oDataAccess.GetDataTable(cSql, null, 25);

            _dt.TableName = "PartyDiscChrgs";
            if (_dataSet.Tables.Contains("PartyDiscChrgs"))
            {
                _dataSet.Tables["PartyDiscChrgs"].Clear();
                _dataSet.Merge(_dt);
            }
            else
            {
                _dataSet.Tables.Add(_dt);
            }
            //return _dt;
        }

        public static DataTable GetItemDefaDiscChrgsValue(int _partyId, int _itemId, decimal _rateLevel)
        {
            string cSql = "", cCond = "";
            bool _dtCrtd = false;
            DataTable _dt = new DataTable();
            if (CommonInfo.DtlDiscFlds != "")
            {
                DataRow _dr;
                cSql = "Select Top 1 Rate_Level from ac_mast where ac_id = " + _partyId.ToString();
                _dr = _oDataAccess.GetDataRow(cSql, null, 25);

                cSql = "Select " + CommonInfo.DtlDiscFlds + " from it_Rate ";
                cCond = " where It_code= " + _itemId.ToString();

                if (_dr[0].ToString() != "")
                {
                    cCond += " and (It_Rate.RLevel=" + _rateLevel.ToString() + " or It_Rate.Ac_id=" + _partyId.ToString() + ") ";
                }
                else
                {
                    cCond += " and It_Rate.Ac_id=" + _partyId.ToString();
                }
                cSql += cCond;
                _dt = _oDataAccess.GetDataTable(cSql, null, 25);
                _dtCrtd = true;
            }
            if (CommonInfo.DtlDiscFldsIM != "")
            {
                if (_dt.Rows.Count == 0 || _dtCrtd == false)
                {
                    cSql = "Select " + CommonInfo.DtlDiscFldsIM + " from it_Mast ";
                    cCond = " where It_code= " + _itemId.ToString();
                    cSql += cCond;
                    _dt = _oDataAccess.GetDataTable(cSql, null, 25);
                }
            }

            return _dt;
        }
        //***** Added by Sachin N. S. on 22/06/2013 for Bug-14538 -- End *****//
    }
}
