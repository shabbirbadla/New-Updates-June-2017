using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace Udyog.Application.License
{
	public class UdyogRegister
	{
        private RegisterMeInfo m_info;

        public UdyogRegister()
        {
            GetRegisterMe();
        }

        public RegisterMeInfo RegistrationInfo
        {
            get { return m_info; }
        }

        #region Private Methods

        private void GetRegisterMe()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            string[] registerMeFile = Directory.GetFiles(path, "*Register.Me");
            if (registerMeFile.Length == 0)
                return;

            path = registerMeFile[0];
            if (!File.Exists(path))
            {
                return;
            }

            FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader reader = new StreamReader(stream, Encoding.GetEncoding(1252));

            //byte[] content = System.Text.Encoding.GetEncoding(437).GetBytes(reader.ReadToEnd());
            string fileText = reader.ReadToEnd();
            reader.Close();
            stream.Close();

            string companyName = Decrypt(fileText.Substring(0, 50));
            string installation = Decrypt(fileText.Substring(50, 200));
            string billing = Decrypt(fileText.Substring(800, 200));

            m_info = new RegisterMeInfo();
            m_info.InstallationAddress.Line1 = installation.Substring(0, 50);
            m_info.InstallationAddress.Line2 = installation.Substring(50, 50);
            m_info.InstallationAddress.Line3 = installation.Substring(100, 50);
            m_info.InstallationAddress.Line4 = installation.Substring(150, 50);
            m_info.InstallationAddress.Location = Decrypt(fileText.Substring(250, 50));
            m_info.InstallationAddress.Route = Decrypt(fileText.Substring(300, 50));
            m_info.InstallationAddress.Zip = Decrypt(fileText.Substring(350, 50));
            m_info.InstallationAddress.Contact = Decrypt(fileText.Substring(400, 50));
            m_info.InstallationAddress.EMail = Decrypt(fileText.Substring(450, 50));
            m_info.InstallationAddress.Tel1 = Decrypt(fileText.Substring(550, 50));
            m_info.InstallationAddress.Tel2 = Decrypt(fileText.Substring(600, 50));
            m_info.InstallationAddress.Mobile = Decrypt(fileText.Substring(650, 50));
            m_info.InstallationAddress.Fax = Decrypt(fileText.Substring(700, 50));
            m_info.InstallationAddress.Web = Decrypt(fileText.Substring(751, 50));

            m_info.BillingAddress.Line1 = billing.Substring(0, 50);
            m_info.BillingAddress.Line2 = billing.Substring(50, 50);
            m_info.BillingAddress.Line3 = billing.Substring(100, 50);
            m_info.BillingAddress.Line4 = billing.Substring(150, 50);
            m_info.BillingAddress.Location = Decrypt(fileText.Substring(1000, 50));
            m_info.BillingAddress.Route = Decrypt(fileText.Substring(1050, 50));
            m_info.BillingAddress.Zip = Decrypt(fileText.Substring(1100, 50));
            m_info.BillingAddress.Contact = Decrypt(fileText.Substring(1150, 50));
            m_info.BillingAddress.EMail = Decrypt(fileText.Substring(1200, 50));
            m_info.BillingAddress.Tel1 = Decrypt(fileText.Substring(1250, 50));
            m_info.BillingAddress.Tel2 = Decrypt(fileText.Substring(1300, 50));
            m_info.BillingAddress.Mobile = Decrypt(fileText.Substring(1350, 50));
            m_info.BillingAddress.Fax = Decrypt(fileText.Substring(1400, 50));
            m_info.BillingAddress.Web = Decrypt(fileText.Substring(1450, 50));

            m_info.ServiceType = Decrypt(fileText.Substring(1500,50));
            m_info.RegistrationType = Decrypt(fileText.Substring(1550,50));
            m_info.InstallDate = Decrypt(fileText.Substring(1600,10));
            m_info.InstallTime = Decrypt(fileText.Substring(1610,50));
            m_info.Business = Decrypt(fileText.Substring(1660,100));
            m_info.ClientProductList = Decrypt(fileText.Substring(1760,100));

            m_info.UdyogProductList = Decrypt(fileText.Substring(1860,200));
            m_info.IdNumber = Decrypt(fileText.Substring(2060,50));
            m_info.ClientId = Decrypt(fileText.Substring(2110,15));
            m_info.ECode = Decrypt(fileText.Substring(2125,50));
            m_info.EName = Decrypt(fileText.Substring(2175,50));
            m_info.ServiceCenterCode = Decrypt(fileText.Substring(2225,50));
            m_info.ServiceCenterName = Decrypt(fileText.Substring(2275,50));

            m_info.MaximumNumberOfCompaniesAllowed = Decrypt(Decrypt(fileText.Substring(2325,10))).Substring(4);
            m_info.MaximumNumberOfUsersAllowed = Decrypt(Decrypt(fileText.Substring(2335,10))).Substring(4);
            m_info.ProductId = Decrypt(fileText.Substring(2345, 10));

            m_info.DBServerName = Decrypt(fileText.Substring(2355,50));
            m_info.DBServerIP = Decrypt(fileText.Substring(2405, 20));
            m_info.ApplicationServerName = Decrypt(fileText.Substring(2425, 50));
            m_info.ApplicationServerIP = Decrypt(fileText.Substring(2475, 20));
            m_info.ExpiryDate = Decrypt(fileText.Substring(2495, 25));
            m_info.MACId = Decrypt(fileText.Substring(2520, 50));

            m_info.AMCStartDate = Decrypt(fileText.Substring(2570, 25));
            m_info.AMCEndDate = Decrypt(fileText.Substring(2595, 25));

            if (fileText.Length > 2620)
            {
                m_info.RegistrationDate = Decrypt(fileText.Substring(2620, 10));
                m_info.RegistrationValue = Decrypt(fileText.Substring(2630, 8));
            }
        }

        private string Decrypt(string mcheck)
        {
            int D = 0;
            int F = mcheck.Length;
            string repl = string.Empty;
            int rep = 0;
            string two = string.Empty;

            while (F > 0)
            {
                string R = mcheck.Substring(D, 1);
                int change = Asc(R) - rep;
                if (change > 0)
                {
                    two = Chr(change);
                }
                repl += two;
                D++;
                F--;
                rep++;
            }

            return repl;
        }

        public static string Chr(int intByte)
        {
            if ((intByte < 0) || (intByte > 255))
            {
                throw new ArgumentOutOfRangeException("p_intByte", intByte, "Must be between 1 and 255.");
            }

            byte[] bytBuffer = new byte[] { (byte)intByte };
            return Encoding.GetEncoding(1252).GetString(bytBuffer);
        }

        private static int Asc(string strChar)
        {
            if ((strChar.Length == 0) || (strChar.Length > 1))
            {
                throw new ArgumentOutOfRangeException("p_strChar", strChar, "Must be a single character.");
            }

            char[] chrBuffer = { Convert.ToChar(strChar) };
            byte[] bytBuffer = Encoding.GetEncoding(1252).GetBytes(chrBuffer);
            return (int)bytBuffer[0];
        }

        #endregion Private Methods
    }

    //public struct RegisterMeInfo
    //{
    //    public string CompanyName;
    //    public AddressInfo InstallationAddress;
    //    public AddressInfo BillingAddress;
    //    public string ServiceType; //?
    //    public string RegistrationType; // ?
    //    public string InstallDate;
    //    public string InstallTime;
    //    public string Business;
    //    public string ClientProductList;

    //    public string UdyogProductList;
    //    public string IdNumber; //?
    //    public string ClientId;
    //    public string ECode; //?
    //    public string EName; //?
    //    public string ServiceCenterCode; 
    //    public string ServiceCenterName; 

    //    public string MaximumNumberOfCompaniesAllowed; 
    //    public string MaximumNumberOfUsersAllowed; 
    //    public string ProductId; //?

    //    public string DBServerName;
    //    public string DBServerIP;
    //    public string ApplicationServerName;
    //    public string ApplicationServerIP;
    //    public string ExpiryDate; //?
    //    public string MACId; //Machine Processor Id in reverse

    //    public string AMCStartDate;
    //    public string AMCEndDate;

    //    public string RegistrationDate;
    //    public string RegistrationValue;
    //}

    //public struct AddressInfo
    //{
    //    public string Line1;
    //    public string Line2;
    //    public string Line3;
    //    public string Line4;

    //    public string Location;
    //    public string Route;
    //    public string Zip;
    //    public string Contact;
    //    public string EMail;

    //    public string Tel1;
    //    public string Tel2;

    //    public string Mobile;
    //    public string Fax;
    //    public string Web;
    //}
}
