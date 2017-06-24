using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccess_Net;

namespace udAdditionalInfo
{
    public class udAdditionalInfo
    {
        clsDataAccess _oDataAccess;
        DataTable _dtLother;
        public string _HdrDtl;
        public string _EntryTy;
        public string _TblName;
        public DataSet _commonDs;

        public udAdditionalInfo()
        { 
            // Default Constructor
        }

        public void callAdditionalInfo()
        {
            //_oDataAccess = new clsDataAccess();

            _dtLother = new DataTable();
            DataRow[] _dr;
            string cSql;
            //cSql = "Select * from Lother Where E_Code='"+_EntryTy+"' and Att_File="+_HdrDtl=="H" ? "1" : "0" + " order by Formno, Serial, SubSerial ";
            //_dtLother = _oDataAccess.GetDataTable(cSql, null, 25);
            _dr = _commonDs.Tables["Lother"].Select(_HdrDtl=="H"? " Att_file=1 ":"Att_File=0");
            if (_dr.Count() > 0)
                _dtLother = _dr.CopyToDataTable();
            else
                _dtLother = _commonDs.Tables["Lother"].Clone();

            if (_dtLother.Rows.Count > 0)
            {
                udFrmAdditionalInfo _udFrmAdditionalInfo = new udFrmAdditionalInfo(_dtLother,_HdrDtl,_TblName,_commonDs);
                _udFrmAdditionalInfo.Show();
            }
        }
    }
}
