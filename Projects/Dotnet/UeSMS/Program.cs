using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
//using udyogerp;
using GetInfo;

namespace UeSMS
{
    class Program
    {
        static void Main(string[] args)
        {
            string lsSName = "", lsUser = "", lsPwd = "", lsdbn = "", lsGatwayID = "", lsGatwayPwd = "", lsSenderID = "";//Birendra : Bug-16177 on 27/06/2013 :lsSenderId:
            string lscoDbNm = "";
            string _SmsStoreIdent = "";     // Added by Sachin N. S. on 29/09/2014 for Bug-22077

            GetInfo.iniFile _oIni;
            GetInfo.EncDec _oEncDec = new GetInfo.EncDec();
            //udyogerp.udyogerpClass obj;           // Commented by Sachin N. S. on 28/07/2014 for Bug-22077
            //obj = new udyogerp.udyogerpClass();   // Commented by Sachin N. S. on 28/07/2014 for Bug-22077


            //****** To be used for Testing -- Start
            //if (args.Length < 16)
            //{
            //args = new string[] { "9", "s081415", @"sachin-pc", "sa", "sa1985", "^19003", "admin", @"d:\usquare10\bmp\icon_10usquare.ico", "usquare10", "usquare10.exe", "4764", "udpid4764dtm20111213125821", "", "2" };
            //}

            //if (args.Length < 2)
            //{
            //args = new string[] { "s011415", "30", "17" };
            //args = new string[] { "s081415", "1", "5" };
            //}

            //****** To be used for Testing -- End

            string CurrPath;
            CurrPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            //CurrPath = "D:\\Usquare10";     // To be removed by Sachin N. S.
            //            if (args.Length == 2)
            //if (args.Length == 3) //Birendra : Bug-16177 on 27/06/2013
            if (args.Length == 3) // Changed by Sachin N. S. on 08/05/2014 for Bug-22077
            {
                lscoDbNm = args[0];
                lsGatwayID = args[1];
                _SmsStoreIdent = args[2];   // Added by Sachin N. S. on 29/09/2014 for Bug-22077

                //***** Commented by Sachin N. S. on 08/05/2014 for Bug-22077 -- Start
                //lsGatwayPwd = args[1];
                //lsSenderID = args[2]; //Birendra : Bug-16177 on 27/06/2013
                //***** Commented by Sachin N. S. on 08/05/2014 for Bug-22077 -- End

                //***** Changed by Sachin N. S. on 28/07/2014 for Bug-22077 -- Start
                _oIni = new GetInfo.iniFile(CurrPath + "\\" + "Visudyog.ini");

                lsSName = _oIni.IniReadValue("DataServer", "Name");
                lsUser = _oEncDec.Dec("myName", _oEncDec.OnDecrypt("myName", _oIni.IniReadValue("DataServer", _oEncDec.OnEncrypt("myName", _oEncDec.Enc("myName", "User")))));
                lsPwd = _oEncDec.Dec("myName", _oEncDec.OnDecrypt("myName", _oIni.IniReadValue("DataServer", _oEncDec.OnEncrypt("myName", _oEncDec.Enc("myName", "Pass")))));

                //lsSName = obj.GetVal(CurrPath, "Name").ToString();
                //lsUser = obj.GetVal(CurrPath, "User").ToString();
                //lsPwd = obj.GetVal(CurrPath, "Pass").ToString();
                //***** Changed by Sachin N. S. on 28/07/2014 for Bug-22077 -- End

                lsdbn = "Vudyog";
            }
            else if (args.Length == 8)  // Changed by Sachin N. S. on 08/05/2014 for Bug-22077
            {
                lsGatwayID = args[0];
                lsGatwayPwd = args[1];
                lsSenderID = args[2];  // Added by Sachin N. S. 
                lsSName = args[3];
                lsUser = args[4];
                lsPwd = args[5];
                lsdbn = args[6];
                lscoDbNm = args[7];     // Added by Sachin N. S. on 08/05/2014 for Bug-22077
            }
            else if (args.Length == 14)  // Added by Sachin N. S. on 30/04/2014 for Bug-22077 -- Start
            {
                if (args[13].ToString() == "1")

                    Application.Run(new frmSMSParamMaster(args));
                else
                    Application.Run(new frmSMSSettingMaster(args));

            }                           // Added by Sachin N. S. on 30/04/2014 for Bug-22077 -- End
            else
                return;

            if (args.Length == 3 || args.Length == 8)       // Added by Sachin N. S. on 30/04/2014 for Bug-22077
            {
                //SendSMS sms = new SendSMS();
                SendSMS sms = new SendSMS(lsSName, lsUser, lsPwd, lscoDbNm, lsGatwayID);   // Changed by Sachin N. S. on 30/04/2014 for Bug-22077
                DataLayer dt = new DataLayer(lsSName, lsUser, lsPwd, lsdbn);
                if (_SmsStoreIdent == "0")       // Added by Sachin N. S. on 29/09/2014 for Bug-22077
                {
                    dt.GetSMSs();
                }
                else                            // Added by Sachin N. S. on 29/09/2014 for Bug-22077 -- Start
                {
                    dt.GetSMSs(_SmsStoreIdent);               // Added by Sachin N. S. on 29/09/2014 for Bug-22077 -- End
                }

                Console.Write(dt.SmsStore.Rows.Count.ToString());

                foreach (DataRow dr in dt.SmsStore.Rows)
                {
                    //if (obj.GetVal(dr["smsGUID"].ToString(), "Decrypt").ToString() == "T*u#b#e*B@u#l*b")
                    if (_oEncDec.Dec("myName", _oEncDec.OnDecrypt("myName", dr["smsGUID"].ToString())).ToString() == "T*u#b#e*B@u#l*b")     // Changed by Sachin N. S. on 28/07/2014 for Bug-22077
                    {
                        //dt.UpdateSendStatus(sms.ExecuteSendSms(dr["MessageText"].ToString(), dr["MobileNo"].ToString(), lsGatwayID, lsGatwayPwd, dr["SMS_ID"].ToString());
                        //Birendra : Bug-16177 on 26/06/2013 :added gateway parameter in ExecuteSendSms method:
                        //dt.UpdateSendStatus(sms.ExecuteSendSms(dr["MessageText"].ToString(), dr["MobileNo"].ToString(), lsGatwayID, lsGatwayPwd, dr["Gateway"].ToString(), lsSenderID), dr["SMS_ID"].ToString());
                        dt.UpdateSendStatus(sms.ExecuteSendSms(dr["MessageText"].ToString(), dr["MobileNo"].ToString()), dr["SMS_ID"].ToString());   // Changed by Sachin N. S. on 08/05/2014 for Bug-22077
                    }
                    else
                        dt.DeleteUnidentified(dr["SMS_ID"].ToString());
                }
            }
        }
    }
}