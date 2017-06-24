using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ueReadRegisterMe
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("ueReadRegisterMe")]

    public class ueReadRegisterMe
    {
        private string _r_compn = string.Empty;
        public string r_compn
        {
            get 
            {
                return _r_compn;
            }

            set
            {
                if (_r_compn == string.Empty)
                _r_compn = value;
            }
        }

        private string _r_comp = string.Empty;
        public string r_comp
        {
            get 
            {
                return _r_comp;
            }

            set
            {
                if (_r_comp == string.Empty)
                _r_comp = value;
            }
        }  
        
        private string _r_add = string.Empty;
        public string r_add
        {
            get 
            {
                return _r_add;
            }

            set
            {
                if (_r_add == string.Empty)
                _r_add = value;
            }
        }

        private string _r_add1 = string.Empty;
        public string r_add1
        {
            get 
            {
                return _r_add1;
            }

            set
            {
                if (_r_add1 == string.Empty)
                _r_add1 = value;
            }
        }
        
        private string _r_add2 = string.Empty;
        public string r_add2
        {
            get 
            {
                return _r_add2;
            }

            set
            {
                if (_r_add2 == string.Empty)
                _r_add2 = value;
            }
        } 
        
        private string _r_add3 = string.Empty;
        public string r_add3
        {
            get 
            {
                return _r_add3;
            }

            set
            {
                if (_r_add3 == string.Empty)
                _r_add3 = value;
            }
        }

        private string _r_add4 = string.Empty;
        public string r_add4
        {
            get 
            {
                return _r_add4;
            }

            set
            {
                if (_r_add4 == string.Empty)
                _r_add4 = value;
            }
        }
        
        private string _r_location = string.Empty;
        public string r_location
        {
            get 
            {
                return _r_location;
            }

            set
            {
                if (_r_location == string.Empty)
                _r_location = value;
            }
        }
        
        private string _r_Route = string.Empty;
        public string r_Route
        {
            get 
            {
                return _r_Route;
            }

            set
            {
                if (_r_Route == string.Empty)
                _r_Route = value;
            }
        } 
            
        private string _r_zip = string.Empty;
        public string r_zip
        {
            get 
            {
                return _r_zip;
            }

            set
            {
                if (_r_zip == string.Empty)
                _r_zip = value;
            }
        }

        private string _r_contact = string.Empty;
        public string r_contact
        {
            get 
            {
                return _r_contact;
            }

            set
            {
                if (_r_contact == string.Empty)
                _r_contact = value;
            }
        }
        
        private string _r_email = string.Empty;
        public string r_email
        {
            get 
            {
                return _r_email;
            }

            set
            {
                if (_r_email == string.Empty)
                _r_email = value;
            }
        }

        private string _r_tel1 = string.Empty;
        public string r_tel1
        {
            get 
            {
                return _r_tel1;
            }

            set
            {
                if (_r_tel1 == string.Empty)
                _r_tel1 = value;
            }
        }
        
        private string _r_tel2 = string.Empty;
        public string r_tel2
        {
            get 
            {
                return _r_tel2;
            }

            set
            {
                if (_r_tel2 == string.Empty)
                _r_tel2 = value;
            }
        }
        
        private string _r_mob = string.Empty;
        public string r_mob
        {
            get 
            {
                return _r_mob;
            }

            set
            {
                if (_r_mob == string.Empty)
                _r_mob = value;
            }
        }
        
        private string _r_fax = string.Empty;
        public string r_fax
        {
            get 
            {
                return _r_fax;
            }

            set
            {
                if (_r_fax == string.Empty)
                _r_fax = value;
            }
        }

        private string _r_web = string.Empty;
        public string r_web
        {
            get 
            {
                return _r_web;
            }

            set
            {
                if (_r_web == string.Empty)
                _r_web = value;
            }
        }
        
        private string _r_billadd = string.Empty;
        public string r_billadd
        {
            get 
            {
                return _r_billadd;
            }

            set
            {
                if (_r_billadd == string.Empty)
                _r_billadd = value;
            }
        }
        
        private string _r_Billadd1 = string.Empty;
        public string r_Billadd1
        {
            get 
            {
                return _r_Billadd1;
            }

            set
            {
                if (_r_Billadd1 == string.Empty)
                _r_Billadd1 = value;
            }
        }
        
        private string _r_Billadd2 = string.Empty;
        public string r_Billadd2
        {
            get 
            {
                return _r_Billadd2;
            }

            set
            {
                if (_r_Billadd2 == string.Empty)
                _r_Billadd2 = value;
            }
        }
         
        private string _r_Billadd3 = string.Empty;
        public string r_Billadd3
        {
            get 
            {
                return _r_Billadd3;
            }

            set
            {
                if (_r_Billadd3 == string.Empty)
                _r_Billadd3 = value;
            }
        }

        private string _r_Billadd4 = string.Empty;
        public string r_Billadd4
        {
            get 
            {
                return _r_Billadd4;
            }

            set
            {
                if (_r_Billadd4 == string.Empty)
                _r_Billadd4 = value;
            }
        }

        private string _r_Billlocation = string.Empty;
        public string r_Billlocation
        {
            get 
            {
                return _r_Billlocation;
            }

            set
            {
                if (_r_Billlocation == string.Empty)
                _r_Billlocation = value;
            }
        }
        
        private string _r_BillRoute = string.Empty;
        public string r_BillRoute
        {
            get 
            {
                return _r_BillRoute;
            }

            set
            {
                if (_r_BillRoute == string.Empty)
                _r_BillRoute = value;
            }
        }
        
        private string _r_Billzip = string.Empty;
        public string r_Billzip
        {
            get 
            {
                return _r_Billzip;
            }

            set
            {
                if (_r_Billzip == string.Empty)
                _r_Billzip = value;
            }
        }
        
        private string _r_Billcontact = string.Empty;
        public string r_Billcontact
        {
            get 
            {
                return _r_Billcontact;
            }

            set
            {
                if (_r_Billcontact == string.Empty)
                _r_Billcontact = value;
            }
        }
        
        private string _r_Billemail = string.Empty;
        public string r_Billemail
        {
            get 
            {
                return _r_Billemail;
            }

            set
            {
                if (_r_Billemail == string.Empty)
                _r_Billemail = value;
            }
        }
        
        private string _r_Billtel1 = string.Empty;
        public string r_Billtel1
        {
            get 
            {
                return _r_Billtel1;
            }

            set
            {
                if (_r_Billtel1 == string.Empty)
                _r_Billtel1 = value;
            }
        }

        private string _r_Billtel2 = string.Empty;
        public string r_Billtel2
        {
            get 
            {
                return _r_Billtel2;
            }

            set
            {
                if (_r_Billtel2 == string.Empty)
                _r_Billtel2 = value;
            }
        }
        
        private string _r_Billmob = string.Empty;
        public string r_Billmob
        {
            get 
            {
                return _r_Billmob;
            }

            set
            {
                if (_r_Billmob == string.Empty)
                _r_Billmob = value;
            }
        }
        
        private string _r_Billfax = string.Empty;
        public string r_Billfax
        {
            get 
            {
                return _r_Billfax;
            }

            set
            {
                if (_r_Billfax == string.Empty)
                _r_Billfax = value;
            }
        }
        
        private string _r_Billweb = string.Empty;
        public string r_Billweb
        {
            get 
            {
                return _r_Billweb;
            }

            set
            {
                if (_r_Billweb == string.Empty)
                _r_Billweb = value;
            }
        }
        
        private string _r_srvtype = string.Empty;
        public string r_srvtype
        {
            get 
            {
                return _r_srvtype;
            }

            set
            {
                if (_r_srvtype == string.Empty)
                _r_srvtype = value;
            }
        }
        
        private string _r_regtype = string.Empty;
        public string r_regtype
        {
            get 
            {
                return _r_regtype;
            }

            set
            {
                if (_r_regtype == string.Empty)
                _r_regtype = value;
            }
        }
        
        private string _r_instdate = string.Empty;
        public string r_instdate
        {
            get 
            {
                return _r_instdate;
            }

            set
            {
                if (_r_instdate == string.Empty)
                _r_instdate = value;
            }
        }
        
        private string _r_InstTime = string.Empty;
        public string r_InstTime
        {
            get 
            {
                return _r_InstTime;
            }

            set
            {
                if (_r_InstTime == string.Empty)
                _r_InstTime = value;
            }
        }
        
        private string _r_Business = string.Empty;
        public string r_Business
        {
            get 
            {
                return _r_Business;
            }

            set
            {
                if (_r_Business == string.Empty)
                _r_Business = value;
            }
        }
 
        private string _r_ProdManu = string.Empty;
        public string r_ProdManu
        {
            get 
            {
                return _r_ProdManu;
            }

            set
            {
                if (_r_ProdManu == string.Empty)
                _r_ProdManu = value;
            }
        }
        
        private string _xvalue = string.Empty;
        public string xvalue
        {
            get 
            {
                return _xvalue;
            }

            set
            {
                if (_xvalue == string.Empty)
                _xvalue = value;
            }
        }
        
        private string _r_idno = string.Empty;
        public string r_idno
        {
            get 
            {
                return _r_idno;
            }

            set
            {
                if (_r_idno == string.Empty)
                _r_idno = value;
            }
        }
        
        private string _r_clientid = string.Empty;
        public string r_clientid
        {
            get 
            {
                return _r_clientid;
            }

            set
            {
                if (_r_clientid == string.Empty)
                _r_clientid = value;
            }
        }
        
        private string _r_ecode = string.Empty;
        public string r_ecode
        {
            get 
            {
                return _r_ecode;
            }

            set
            {
                if (_r_ecode == string.Empty)
                _r_ecode = value;
            }
        }
        
        private string _r_ename = string.Empty;
        public string r_ename
        {
            get 
            {
                return _r_ename;
            }

            set
            {
                if (_r_ename == string.Empty)
                _r_ename = value;
            }
        }
        
        private string _r_svccode = string.Empty;
        public string r_svccode
        {
            get 
            {
                return _r_svccode;
            }

            set
            {
                if (_r_svccode == string.Empty)
                _r_svccode = value;
            }
        }
        
        private string _r_svcname = string.Empty;
        public string r_svcname
        {
            get 
            {
                return _r_svcname;
            }

            set
            {
                if (_r_svcname == string.Empty)
                _r_svcname = value;
            }
        }
        
        private string _r_pid = string.Empty;
        public string r_pid
        {
            get 
            {
                return _r_pid;
            }

            set
            {
                if (_r_pid == string.Empty)
                _r_pid = value;
            }
        }
        
        private string _r_dbsrvname = string.Empty;
        public string r_dbsrvname
        {
            get 
            {
                return _r_dbsrvname;
            }

            set
            {
                if (_r_dbsrvname == string.Empty)
                _r_dbsrvname = value;
            }
        } 

        private string _r_dbsrvIp = string.Empty;
        public string r_dbsrvIp
        {
            get 
            {
                return _r_dbsrvIp;
            }

            set
            {
                if (_r_dbsrvIp == string.Empty)
                _r_dbsrvIp = value;
            }
        } 
        
        private string _r_Apsrvname = string.Empty;
        public string r_Apsrvname
        {
            get 
            {
                return _r_Apsrvname;
            }

            set
            {
                if (_r_Apsrvname == string.Empty)
                _r_Apsrvname = value;
            }
        }
        
        private string _r_ApsrvIp = string.Empty;
        public string r_ApsrvIp
        {
            get 
            {
                return _r_ApsrvIp;
            }

            set
            {
                if (_r_ApsrvIp == string.Empty)
                _r_ApsrvIp = value;
            }
        }
        
        private string _r_ExpDt = string.Empty;
        public string r_ExpDt
        {
            get 
            {
                return _r_ExpDt;
            }

            set
            {
                if (_r_ExpDt == string.Empty)
                _r_ExpDt = value;
            }
        }
        
        private string _r_MACId = string.Empty;
        public string r_MACId
        {
            get 
            {
                return _r_MACId;
            }

            set
            {
                if (_r_MACId == string.Empty)
                _r_MACId = value;
                char [] A = _r_MACId.ToCharArray();
                Array.Reverse(A);
                _r_MACId = new string(A);
            }
        }
        
        private string _r_AMCStDt = string.Empty;
        public string r_AMCStDt
        {
            get 
            {
                return _r_AMCStDt;
            }

            set
            {
                if (_r_AMCStDt == string.Empty)
                _r_AMCStDt = value;
            }
        }
        
        private string _r_AMCEdDt = string.Empty;
        public string r_AMCEdDt
        {
            get 
            {
                return _r_AMCEdDt;
            }

            set
            {
                if (_r_AMCEdDt == string.Empty)
                _r_AMCEdDt = value;
            }
        }
        
        private string _reg_date = string.Empty;
        public string reg_date
        {
            get 
            {
                return _reg_date;
            }

            set
            {
                if (_reg_date == string.Empty)
                _reg_date = value;
            }
        }

        private string _reg_value = string.Empty;
        public string reg_value
        {
            get 
            {
                return _reg_value;
            }

            set
            {
                if (_reg_value == string.Empty)
                    _reg_value = value;
            }
        }

        private UInt16 _r_coof = 0;
        public UInt16 r_coof
        {
            get
            {
                return _r_coof;
            }

            set
            {
                if (_r_coof == 0)
                    _r_coof = value;
            }
        }

        private UInt16 _r_noof = 0;
        public UInt16 r_noof
        {
            get
            {
                return _r_noof;
            }

            set
            {
                if (_r_noof == 0)
                    _r_noof = value;
            }
        }

        private int _UnqVal = 0;
        public int UnqVal
        {
            get
            {
                return _UnqVal;
            }

            set
            {
                if (_UnqVal == 0)
                    _UnqVal = value;
            }
        }

        public ueReadRegisterMe()
        { 
            //Default Constructor
        }

        public void _ueReadRegisterMe(string cServerPath, string cUnqVal)
        {

            System.IO.DirectoryInfo cFilePath;
            System.IO.FileInfo[] aFileName;
            cFilePath = new System.IO.DirectoryInfo(cServerPath);
            aFileName = cFilePath.GetFiles("*register.me");
            String FileName = "";
            bool llFail = false;

            if (cUnqVal.Trim() == "")
            {
                MessageBox.Show("Unique Value cannot be Blank.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if ((cUnqVal.Trim().Length) % 2 != 0)
            {
                MessageBox.Show("The value should have even no. of numeric characters, cannot continue.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            UnqVal = getUnqValue(cUnqVal);

            foreach (System.IO.FileInfo cFn in aFileName)
            {
                FileName = cFn.Name;
                if (cFn.IsReadOnly == true)
                {
                    MessageBox.Show("Set the property of Register.me to read & write.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    llFail = true;
                    break;
                }
                else
                {
                    break;
                }
            }

            if (llFail == true)
            {
                return;
            }

            if (FileName != "")
            {
                try
                {
                    string _RegMe = "";
                    StreamReader sr = new StreamReader(cServerPath + "\\" + FileName, Encoding.GetEncoding(1252));
                    _RegMe = sr.ReadToEnd();
                    sr.Close();

                    r_compn = dec(_RegMe.Substring(0, 50));
                    r_comp = r_compn;
                    r_add = dec(_RegMe.Substring(50, 200));
                    r_add1 = r_add.Substring(0, 50);
                    r_add2 = r_add.Substring(50, 50);
                    r_add3 = r_add.Substring(100, 50);
                    r_add4 = r_add.Substring(150, 50);
                    r_location = dec(_RegMe.Substring(250, 50));
                    r_Route = dec(_RegMe.Substring(300, 50));
                    r_zip = dec(_RegMe.Substring(350, 50));
                    r_contact = dec(_RegMe.Substring(400, 50));
                    r_email = dec(_RegMe.Substring(450, 100));
                    r_tel1 = dec(_RegMe.Substring(550, 50));
                    r_tel2 = dec(_RegMe.Substring(600, 50));
                    r_mob = dec(_RegMe.Substring(650, 50));
                    r_fax = dec(_RegMe.Substring(700, 50));
                    r_web = dec(_RegMe.Substring(750, 50));
                    r_billadd = dec(_RegMe.Substring(800, 200));
                    r_Billadd1 = r_billadd.Substring(0, 50);
                    r_Billadd2 = r_billadd.Substring(50, 50);
                    r_Billadd3 = r_billadd.Substring(100, 50);
                    r_Billadd4 = r_billadd.Substring(150, 50);
                    r_Billlocation = dec(_RegMe.Substring(1000, 50));
                    r_BillRoute = dec(_RegMe.Substring(1050, 50));
                    r_Billzip = dec(_RegMe.Substring(1100, 50));
                    r_Billcontact = dec(_RegMe.Substring(1150, 50));


                    r_Billemail = dec(_RegMe.Substring(1200, 50));
                    r_Billtel1 = dec(_RegMe.Substring(1250, 50));
                    r_Billtel2 = dec(_RegMe.Substring(1300, 50));
                    r_Billmob = dec(_RegMe.Substring(1350, 50));
                    r_Billfax = dec(_RegMe.Substring(1400, 50));
                    r_Billweb = dec(_RegMe.Substring(1450, 50));

                    r_srvtype = dec(_RegMe.Substring(1500, 50));
                    r_regtype = dec(_RegMe.Substring(1550, 50));
                    r_instdate = dec(_RegMe.Substring(1600, 10));
                    r_InstTime = dec(_RegMe.Substring(1610, 50));
                    r_Business = dec(_RegMe.Substring(1660, 100));
                    r_ProdManu = dec(_RegMe.Substring(1760, 100));


                    xvalue = dec(_RegMe.Substring(1860, 200));
                    r_idno = dec(_RegMe.Substring(2060, 50));
                    r_clientid = dec(_RegMe.Substring(2110, 15));

                    r_ecode = dec(_RegMe.Substring(2125, 50));
                    r_ename = dec(_RegMe.Substring(2175, 50));
                    r_svccode = dec(_RegMe.Substring(2225, 50));
                    r_svcname = dec(_RegMe.Substring(2275, 50));

                    r_coof = Convert.ToUInt16(dec(dec(_RegMe.Substring(2325, 10))).Trim().Replace("COOF", "").Replace(Convert.ToChar(30).ToString(), ""));

                    r_noof = Convert.ToUInt16(dec(dec(_RegMe.Substring(2335, 10))).ToString().Trim().Replace("USOF", ""));

                    r_pid = dec(_RegMe.Substring(2345, 10));

                    r_dbsrvname = dec(_RegMe.Substring(2355, 50));
                    r_dbsrvIp = dec(_RegMe.Substring(2405, 20));
                    r_Apsrvname = dec(_RegMe.Substring(2425, 50));
                    r_ApsrvIp = dec(_RegMe.Substring(2475, 20));
                    r_ExpDt = dec(_RegMe.Substring(2495, 25));
                    r_MACId = dec(_RegMe.Substring(2520, 50));

                    r_AMCStDt = dec(_RegMe.Substring(2570, 25));
                    r_AMCEdDt = dec(_RegMe.Substring(2595, 25));

                    if (_RegMe.Length > 2620)
                    {
                        reg_date = dec(_RegMe.Substring(2620, 10));
                        reg_value = dec(_RegMe.Substring(2630, 8));
                        reg_value = reg_value != "" ? reg_value : "NOT DONE";
                    }

                }
                catch (System.Exception se)
                {
                    MessageBox.Show(se.Message.ToString());
                }
            }
        }

        private string dec(string cValue)
        {
            int D = 0, F = cValue.Length, rep = 0, Change = 0;
            string Repl = "", two = "", R = "";
            while (F > 0)
            {
                R = cValue.Substring(D, 1);
                char[] r = { Convert.ToChar(R) };
                Change = ((int)(Encoding.GetEncoding(1252).GetBytes(r))[0]) - rep;
                if (Change > 0)
                {
                    two = Convert.ToChar(Change).ToString();
                }
                Repl = Repl + two;
                D = D + 1;
                F = F - 1;
                rep = rep + 1;
            }
            return Repl;
        }

        private int getUnqValue(string _cUnqVal)
        {
            int nUnqVal=0;
            string _cUnqVal1 = "";
            char cTmp;
            char[] _chrArr = _cUnqVal.ToString().Trim().ToCharArray();
            for (int i = 0; i < _chrArr.Length; i+=2)
            {
                cTmp = _chrArr[i];
                _chrArr[i] = _chrArr[i + 1];
                _chrArr[i + 1] = cTmp;
            }

            _cUnqVal1 = new string(_chrArr);
            nUnqVal = Convert.ToInt32(_cUnqVal1.Trim()) * 3;

            return nUnqVal;
        }
    }
}

    /* Old Code Using Constructor not working
    public class ueReadRegisterMe
    {
        public readonly string r_compn, r_comp, r_add, r_add1, r_add2, r_add3, r_add4, r_location, r_Route, 
            r_zip, r_contact, r_email, r_tel1, r_tel2, r_mob, r_fax, r_web, r_billadd, r_Billadd1, r_Billadd2, 
            r_Billadd3, r_Billadd4, r_Billlocation, r_BillRoute, r_Billzip, r_Billcontact, r_Billemail, r_Billtel1,
            r_Billtel2, r_Billmob, r_Billfax, r_Billweb, r_srvtype, r_regtype, r_instdate, r_InstTime, r_Business, 
            r_ProdManu, xvalue, r_idno, r_clientid, r_ecode, r_ename, r_svccode, r_svcname, r_pid, r_dbsrvname, 
            r_dbsrvIp, r_Apsrvname, r_ApsrvIp, r_ExpDt, r_MACId, r_AMCStDt, r_AMCEdDt, reg_date, reg_value ;
        public readonly UInt16 r_coof, r_noof ;

        public ueReadRegisterMe(string cServerPath)
        {
            System.IO.DirectoryInfo cFilePath;
            System.IO.FileInfo[] aFileName;
            //System.IO.FileInfo cFn;
            cFilePath = new System.IO.DirectoryInfo(cServerPath);
            aFileName = cFilePath.GetFiles("*register.me");
            String FileName="";
            bool llFail=false;
            
            foreach (System.IO.FileInfo cFn in aFileName)
            {
                FileName = cFn.Name;
                if (cFn.IsReadOnly == true)
                {
                    MessageBox.Show("Set the property of Register.me to read & write.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    llFail = true;
                    break;
                }
            }

            if (llFail == true)
            {
                return;
            }

            if (FileName != "")
            {
                try
                {
                    //FileStream fs = FileName;
                    string _RegMe = "";
                    //StreamReader sr = File.OpenText(cServerPath + "\\" + FileName);
                    StreamReader sr = new StreamReader(cServerPath + "\\" + FileName, Encoding.GetEncoding(1252));
                        //File.ReadAllText(cServerPath + "\\" + FileName, Encoding.GetEncoding(1252));
                    _RegMe = sr.ReadToEnd();

                    r_compn		=	dec(_RegMe.Substring(0,50));
		            r_comp   	=	r_compn;
		            r_add		= 	dec(_RegMe.Substring(50,200));
                    MessageBox.Show(r_add);
		            r_add1		=	r_add.Substring(0,50);
                    MessageBox.Show(r_add1);
                    r_add2      =   r_add.Substring(50, 50);
                    MessageBox.Show(r_add2);
		            r_add3		=	r_add.Substring(100,50);
                    MessageBox.Show(r_add3);
		            r_add4		=	r_add.Substring(150,50);
                    MessageBox.Show(r_add4);
		            r_location	=   dec(_RegMe.Substring(250,50));
                    MessageBox.Show(r_location);
		            r_Route		=   dec(_RegMe.Substring(300,50));
                    MessageBox.Show(r_Route);
		            r_zip		= 	dec(_RegMe.Substring(350,50));
                    MessageBox.Show(r_zip);
		            r_contact	=	dec(_RegMe.Substring(400,50));
                    MessageBox.Show(r_contact);
		            r_email		=	dec(_RegMe.Substring(450,100));
                    MessageBox.Show(r_email);
		            r_tel1		= 	dec(_RegMe.Substring(550,50));
                    MessageBox.Show(r_tel1);
		            r_tel2		= 	dec(_RegMe.Substring(600,50));
                    MessageBox.Show(r_tel2);
		            r_mob		= 	dec(_RegMe.Substring(650,50));
                    MessageBox.Show(r_mob);
		            r_fax		= 	dec(_RegMe.Substring(700,50));
                    MessageBox.Show(r_fax);
		            r_web		= 	dec(_RegMe.Substring(750,50));
                    MessageBox.Show(r_web);

		            r_billadd		= 	dec(_RegMe.Substring(800,200));
                    MessageBox.Show(r_billadd);
		            r_Billadd1		=	r_billadd.Substring(0,50);
                    MessageBox.Show(r_Billadd1);
		            r_Billadd2		=	r_billadd.Substring(50,50);
                    MessageBox.Show(r_Billadd2);
		            r_Billadd3		=	r_billadd.Substring(100,50);
                    MessageBox.Show(r_Billadd3);
		            r_Billadd4		=	r_billadd.Substring(150,50);
                    MessageBox.Show(r_Billadd4);
		            r_Billlocation	=   dec(_RegMe.Substring(1000,50));
                    MessageBox.Show(r_Billlocation);
		            r_BillRoute		=   dec(_RegMe.Substring(1050,50));
                    MessageBox.Show(r_BillRoute);
		            r_Billzip		= 	dec(_RegMe.Substring(1100,50));
                    MessageBox.Show(r_Billzip);
		            r_Billcontact	=	dec(_RegMe.Substring(1150,50));
                    MessageBox.Show(r_Billcontact);
		            r_Billemail		=	dec(_RegMe.Substring(1200,50));
                    MessageBox.Show(r_Billemail);
		            r_Billtel1		= 	dec(_RegMe.Substring(1250,50));
                    MessageBox.Show(r_Billtel1);
		            r_Billtel2		= 	dec(_RegMe.Substring(1300,50));
                    MessageBox.Show(r_Billtel2);
		            r_Billmob		= 	dec(_RegMe.Substring(1350,50));
                    MessageBox.Show(r_Billmob);
		            r_Billfax		= 	dec(_RegMe.Substring(1400,50));
                    MessageBox.Show(r_Billfax);
		            r_Billweb		= 	dec(_RegMe.Substring(1450,50));
                    MessageBox.Show(r_Billweb);

		            r_srvtype		= 	dec(_RegMe.Substring(1500,50));
                    MessageBox.Show(r_srvtype);
		            r_regtype		=	dec(_RegMe.Substring(1550,50));
                    MessageBox.Show(r_regtype);
		            r_instdate		=	dec(_RegMe.Substring(1600,10));
                    MessageBox.Show(r_instdate);
		            r_InstTime		=	dec(_RegMe.Substring(1610,50));
                    MessageBox.Show(r_InstTime);
		            r_Business		=	dec(_RegMe.Substring(1660,100));
                    MessageBox.Show(r_Business);
		            r_ProdManu		=	dec(_RegMe.Substring(1760,100));
                    MessageBox.Show(r_ProdManu);

		            xvalue			=	dec(_RegMe.Substring(1860,200));
                    MessageBox.Show(xvalue);
		            r_idno			=	dec(_RegMe.Substring(2060,50));
                    MessageBox.Show(r_idno);
		            r_clientid		= 	dec(_RegMe.Substring(2110,15));
                    MessageBox.Show(r_clientid);
		            r_ecode			= 	dec(_RegMe.Substring(2125,50));
                    MessageBox.Show(r_ecode);
		            r_ename			=	dec(_RegMe.Substring(2175,50));
                    MessageBox.Show(r_ename);
		            r_svccode		=	dec(_RegMe.Substring(2225,50));
                    MessageBox.Show(r_svccode);
		            r_svcname		=	dec(_RegMe.Substring(2275,50));
                    MessageBox.Show(r_svcname);

                    MessageBox.Show(dec(dec(_RegMe.Substring(2325, 10))).Trim());

		            r_coof			=	Convert.ToUInt16(dec(dec(_RegMe.Substring(2325,10))).Trim().Replace("COOF","").Replace(Convert.ToChar(30).ToString(),""));
                    MessageBox.Show(r_coof.ToString());
		             r_noof			= 	Convert.ToUInt16(dec(dec(_RegMe.Substring(2335,10))).ToString().Trim().Replace("USOF",""));
                    MessageBox.Show(r_noof.ToString());
		            r_pid       	=	dec(_RegMe.Substring(2345,10));
                    MessageBox.Show(r_pid);

		            r_dbsrvname 	=	dec(_RegMe.Substring(2355,50));
                    MessageBox.Show(r_dbsrvname);
		            r_dbsrvIp 		=	dec(_RegMe.Substring(2405,20));
                    MessageBox.Show(r_dbsrvIp);
		            r_Apsrvname 	=	dec(_RegMe.Substring(2425,50));
                    MessageBox.Show(r_Apsrvname);
		            r_ApsrvIp 		=	dec(_RegMe.Substring(2475,20));
                    MessageBox.Show(r_ApsrvIp);
		            r_ExpDt 		=	dec(_RegMe.Substring(2495,25));
                    MessageBox.Show(r_ExpDt);
		            r_MACId			=	dec(_RegMe.Substring(2520,50));
                    MessageBox.Show(r_MACId);

		            r_AMCStDt		=	dec(_RegMe.Substring(2570,25));
                    MessageBox.Show(r_AMCStDt);
		            r_AMCEdDt		=	dec(_RegMe.Substring(2595,25));
                    MessageBox.Show(r_AMCEdDt);


		            reg_date		= 	dec(_RegMe.Substring(2620,10));
                    MessageBox.Show(reg_date);
		            reg_value   	= 	dec(_RegMe.Substring(2630,8));
                    MessageBox.Show(reg_value);
		            reg_value		= 	reg_value!="" ? reg_value : "NOT DONE";
                    MessageBox.Show(reg_value);

                }
                catch (System.Exception se)
                {
                    MessageBox.Show(se.Message.ToString());
                }
            }
               
        }

        private string dec(string cValue)
        {
            int D=0, F=cValue.Length, rep=0, Change=0;
            string Repl="",two="",R="";
            while (F>0)
            {
                R=cValue.Substring(D,1);
                //r = R.ToCharArray(D, 1);
                //Change = (int)Convert.ToChar(R)-rep;
                //Change = Convert.ToInt32(((int)Convert.ToChar(R)).ToString()) - rep;
                //Change = (int)R[0] - rep;
                char[] r = {Convert.ToChar(R)};
                Change = ((int)(Encoding.GetEncoding(1252).GetBytes(r))[0]) - rep;
		        if (Change>0)
                {
			        two = Convert.ToChar(Change).ToString();
                }
		        Repl=Repl+two;
		        D=D+1;
		        F=F-1;
		        rep=rep+1;
            }
            return Repl;
        }
    }
    */ 

