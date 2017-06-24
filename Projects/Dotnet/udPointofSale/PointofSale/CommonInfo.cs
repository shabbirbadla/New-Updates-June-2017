using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointofSale
{
    class CommonInfo
    {
        public static string Server = "", DbName = "", Uid = "", Pwd = "";
        public static string ConnectionString
        {
            get
            {
                return "Data Source=" + Server.ToString() + ";Initial Catalog=" + DbName.ToString() + ";User Id=" + Uid.ToString() + ";Password=" + Pwd.ToString();
            }
        }

        private static string _icoPath = "";
        public static string IconPath
        {
            set { _icoPath = value; }
            get { return _icoPath; }
        }

        private static string _appCaption = "";
        public static string AppCaption
        {
            set { _appCaption = value; }
            get { return _appCaption; }
        }

        private static int _compId = 0;
        public static int CompId
        {
            set { _compId = value; }
            get { return _compId; }
        }

        private static string _userName = "";
        public static string UserName
        {
            set { _userName = value; }
            get { return _userName; }
        }

        private static string _invSeries = "";
        public static string InvSeries
        {
            set { _invSeries = value; }
            get { return _invSeries; }
        }

        private static string _department = "";
        public static string Department
        {
            set { _department = value; }
            get { return _department; }
        }

        private static string _category = "";
        public static string Category
        {
            set { _category = value; }
            get { return _category; }
        }

        private static string _l_yn = "";
        public static string L_yn
        {
            set { _l_yn = value; }
            get { return _l_yn; }
        }

        private static string _startDate = "";
        public static string StartDate
        {
            set { _startDate = value; }
            get { return _startDate; }
        }

        private static string _endDate = "";
        public static string EndDate
        {
            set { _endDate = value; }
            get { return _endDate; }
        }

        private static string _prodCode = "";
        public static string ProdCode
        {
            set { _prodCode = value; }
            get { return _prodCode; }
        }

        private static string _applName = "";
        public static string ApplName
        {
            set { _applName = value; }
            get { return _applName; }
        }

        private static int _applPId = 0;
        public static int ApplPId
        {
            set { _applPId = value; }
            get { return _applPId; }
        }

        private static string _applCode = "";
        public static string ApplCode
        {
            set { _applCode = value; }
            get { return _applCode; }
        }

        //****** Added by Sachin N. S. on 19/05/2013 for Bug-14538 -- Start ******//
        private static int _partyId = 0;
        public static int PartyId
        {
            set { _partyId = value; }
            get { return _partyId; }
        }

        private static string _partyNm = "";
        public static string PartyNm
        {
            set { _partyNm = value; }
            get { return _partyNm; }
        }
        
        private static string _wareHouse = "";
        public static string WareHouse
        {
            set { _wareHouse = value; }
            get { return _wareHouse; }
        }

        private static string _chngdWareHouse = "";
        public static string ChngdWareHouse
        {
            set { _chngdWareHouse = value; }
            get { return _chngdWareHouse; }
        }

        private static string _hdrDiscFlds = "";
        public static string HdrDiscFlds
        {
            set { _hdrDiscFlds = value; }
            get { return _hdrDiscFlds; }
        }

        private static string _dtlDiscFlds = "";
        public static string DtlDiscFlds
        {
            set { _dtlDiscFlds = value; }
            get { return _dtlDiscFlds; }
        }

        private static string _dtlDiscFldsIM = "";
        public static string DtlDiscFldsIM
        {
            set { _dtlDiscFldsIM = value; }
            get { return _dtlDiscFldsIM; }
        }
        //****** Added by Sachin N. S. on 19/05/2013 for Bug-14538 -- End ******//

        //****** Added by Sachin N. S. on 19/05/2013 for Bug-27503 -- Start ******//
        private static string _counterNm = "";
        public static string CounterNm
        {
            set { _counterNm = value; }
            get { return _counterNm; }
        }

        private static Int16 _neg_itBal = 0;
        public static Int16 Neg_itBal
        {
            set { _neg_itBal = value; }
            get { return _neg_itBal; }
        }

        private static Int16 _cashInNo = 0;
        public static Int16 CashInNo
        {
            set { _cashInNo = value; }
            get { return _cashInNo; }
        }
        //****** Added by Sachin N. S. on 19/05/2013 for Bug-27503 -- End ******//
    }
}
