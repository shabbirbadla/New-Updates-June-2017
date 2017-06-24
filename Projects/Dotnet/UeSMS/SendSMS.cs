using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpRequestUrl;
//using System.Collections.Specialized;
using System.Net;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using DataAccess_Net;       // Added by Sachin N. S. on 09/05/2014 for Bug-22077
using GsmComm.GsmCommunication;
using GsmComm.PduConverter;


namespace UeSMS
{
    class SendSMS
    {
        string GatewayUID;
        string GatewayPWD;
        string SenderID;
        string SendUrl;
        //*****  Added by Sachin N. S. on 08/05/2014 for Bug-22077 -- Start
        string UrlParam;        
        DataRow _dr;

        Int16 Comm_Port = 0;
        Int32 Comm_BaudRate = 0;
        Int32 Comm_TimeOut = 0;
        GsmCommMain comm;
        SmsSubmitPdu pdu1;
        //*****  Added by Sachin N. S. on 08/05/2014 for Bug-22077 -- End

        HttpRequestUrlClass sendsms;
        public SendSMS()
        {
            //GatewayPWD = "551060";
            //GatewayUID = "uDyog";
            //SenderID = "UDYOG";
            GatewayPWD = "PSHPAK";
            GatewayUID = "PSHPAK";
            SenderID = "INFOIN";
            SetSendUrl();
            sendsms = new HttpRequestUrlClass();
        }

        //**** Added by Sachin N. S. on 08/05/2014 for Bug-22077 -- Start
        public SendSMS(string _lsSName, string _lsUser, string _lsPwd, string _lsdbn, string _smsSetId)
        {
            DataAccess_Net.clsDataAccess._databaseName = _lsdbn;
            DataAccess_Net.clsDataAccess._serverName = _lsSName;
            DataAccess_Net.clsDataAccess._userID = _lsUser;
            DataAccess_Net.clsDataAccess._password = _lsPwd;
            clsDataAccess oDataAccess = new clsDataAccess();

            string _sqlStr = "SELECT A.SMSCODE, A.SMSGTWAYID, A.SMSURL, A.SMSPARAM, B.SMSSETID, B.SMSVIA, B.SMSGTWAYNM, B.SMSPARAVL, B.PORTNAME, B.BAUDRATE, B.TIMEOUT, B.SMSSIMNO ";
            _sqlStr += " FROM SMSPARAMMASTER A LEFT JOIN SMSSETTINGMASTER B ON A.SMSCODE=B.SMSCODE WHERE SMSSETID=" + _smsSetId.ToString();
            _dr = oDataAccess.GetDataRow(_sqlStr, null, 50);

            if (_dr["SMSVIA"].ToString() == "1")
            {
                SetSendUrl(_dr["SMSURL"].ToString());

                UrlParam = getParamStr(_dr);

                sendsms = new HttpRequestUrlClass();
            }
            else
            {
                setMobData(_dr);
            }
        }

        private string getParamStr(DataRow _dr1)
        {
            string[] _param, _paramVl;
            string _urlParam = "";
            _param = _dr1["SMSPARAM"].ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            _paramVl = Utilities.dec(_dr1["SMSPARAVL"].ToString()).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < _param.Length; i++)
            {
                _urlParam += (i == 0 ? "" : "&") + _param[i].Substring(0, _param[i].IndexOf(':')).ToString() + "=" + _paramVl[i].ToString();
            }
            return _urlParam;
        }

        private void setMobData(DataRow _dr1)
        {
            Comm_Port = Convert.ToInt16(_dr1["PORTNAME"]);
            Comm_BaudRate = Convert.ToInt16(_dr1["BAUDRATE"]);
            Comm_TimeOut = Convert.ToInt16(_dr1["TIMEOUT"]);
        }
        //**** Added by Sachin N. S. on 08/05/2014 for Bug-22077 -- End

        void SetCredentials()
        {
            GatewayPWD = "551060";
            GatewayUID = "udyog";
            SenderID = "UDYOG";
        }

        void SetCredentials(string GateWayUid, string GateWayPWD, string SenderIDS)
        {
            GatewayPWD = GateWayPWD;
            GatewayUID = GateWayUid;
            SenderID = SenderIDS;
        }

        void SetSendUrl()
        {
            SendUrl = "http://hapi.smsapi.org/SendSMS.aspx?";
        }

        void SetSendUrl(string url)
        {
            SendUrl = url.EndsWith("?") ? url : url.Trim()+'?' ;
        }

        public string ExecuteSendSms(string Message, string MobileNos)
        {
            // Commented by Sachin N. S. on 08/05/2014 for Bug-22077 -- Start
            //if (String.IsNullOrEmpty(GatewayUID) | String.IsNullOrEmpty(GatewayPWD)|String.IsNullOrEmpty(SenderID))
            //    return "Error In UserID or Password or SenderID : Stop";
            //string url = SendUrl + "UserName=" + GatewayUID + "&password=" + GatewayPWD + "&MobileNo=" + MobileNos + "&SenderID=" + SenderID + "&CDMAHeader=919920894440&Message="+Message;
            //try
            //{
            //    //return "\"Worked\"" +"\"Done\""+ url;
            //    return sendsms.MakeWebRequestGET(url);
            //}
            //catch
            //{
            //    return "Error In Sending SMS ";
            //}
            // Commented by Sachin N. S. on 08/05/2014 for Bug-22077 -- End

            if (_dr["SMSVIA"].ToString() == "1")
                return sendSMSThruWeb(MobileNos,Message);
            else
                return sendSMSThruMob(MobileNos, Message);

            return "";
        }

        public string ExecuteSendSms(string Message, string MobileNos, string lGatewayUID, string lGatewayPWD, string lGateway, string lSenderID)
        {
            //if (String.IsNullOrEmpty(GatewayUID) | String.IsNullOrEmpty(GatewayPWD) | String.IsNullOrEmpty(lGateway) | String.IsNullOrEmpty(lSenderID))
            //    return "Error In UserID or Password or Gateway or  SenderID : Stop";
            //Birendra : Bug-16177 on 26/06/2013 :Start:
            string url;
            url = "";
            switch (lGateway.Trim().ToUpper())
            {
                case "VIVA":
                    SendUrl = "http://hapi.smsapi.org/SendSMS.aspx?";
                    url = SendUrl + "UserName=" + lGatewayUID.Trim() + "&password=" + lGatewayPWD.Trim() + "&MobileNo=" + MobileNos + "&SenderID=" + lSenderID.Trim() + "&CDMAHeader=919920894440&Message=" + Message;
                    break;
                case "WEBSMS":
                    SendUrl = "http://dndopen.dove-sms.com/TransSMS/SMSAPI.jsp?";    //vasant1  viva after mobile,other before mobile &sendername  //&CDMAHeader=919920894440
                    url = SendUrl + "username=" + GatewayUID.Trim() + "&password=" + lGatewayPWD.Trim() + "&sendername=" + lSenderID.Trim() + "&mobileno=" + MobileNos.Trim() + "&message=" + Message;
                    break;
            }
            //Birendra : Bug-16177 on 26/06/2013 :End:

            //            string url = SendUrl + "UserName=" + lGatewayUID.Trim() + "&password=" + lGatewayPWD.Trim() + "&MobileNo=" + MobileNos + "&SenderID=" + SenderID + "&CDMAHeader=919920894440&Message=" + Message;
            try
            {
                //return url.Trim();
                return sendsms.MakeWebRequestGET(url);
            }
            catch
            {
                return "Error In Sending SMS ";
            }
        }

        //****** Added by Sachin N. S. on 09/05/2014 for Bug-22077 -- Start
        private string sendSMSThruWeb(string _mobileNo, string _message)
        {
            UrlParam = UrlParam.Replace("<<MobileNo>>", _mobileNo.Trim());
            UrlParam = UrlParam.Replace("<<Message>>", _message.Trim());

            string url = SendUrl + UrlParam;
            //url = "http://mainadmin.dove-sms.com/sendsms.jsp?user=PSHPAK&password=D@ve34&mobiles=9867556339&sms=Your Order Dispatched Through Bill No-0001 dated-01/04/13 Through Vehicle-MH-15/AG/8699. Bill Amount-477837.00 Make Payment Before-03/04/13- Pushpak Steel Pune&senderid=PSHPAK";
            //StreamWriter fs = File.CreateText("E:\\Test.txt");
            //fs.WriteLine(url);
            //fs.Close();

            try
            {
                return sendsms.MakeWebRequestGET(url);
            }
            catch
            {
                return "Error In Sending SMS ";
            }
        }

        private string sendSMSThruMob(string _mobileNo, string _message)
        {
            comm = new GsmCommMain(Comm_Port, Comm_BaudRate, Comm_TimeOut);
            try
            {
                comm.Open();
                if (comm.IsConnected())
                {
                    try
                    {
                        pdu1 = new SmsSubmitPdu(_message, _mobileNo, "");

                        comm.SendMessage(pdu1);
                       
                        return "Message sent successfully";
                    }
                    catch (Exception ex1)
                    {
                        comm.Close();
                        return "Error sending SMS to Destination address";
                    }
                }
            }
            catch (Exception excomm)
            {
                return "Error while connecting to GSM Phone / Modem";
            }
            return "";
        }
        //****** Added by Sachin N. S. on 09/05/2014 for Bug-22077 -- End
    }
}
