using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Udyog.Application.License
{
    [DataContract(Namespace = "www.udyogsoftware.com/Registration/2010/06")]
    public struct RegisterMeInfo
    {
        public string CompanyName;
        public AddressInfo InstallationAddress;
        public AddressInfo BillingAddress;
        public string ServiceType;
        public string RegistrationType;
        public string InstallDate;
        public string InstallTime;
        public string Business;
        public string ClientProductList;

        public string UdyogProductList;
        public string IdNumber;
        public string ClientId;
        public string ECode;
        public string EName;
        public string ServiceCenterCode;
        public string ServiceCenterName;

        public string MaximumNumberOfCompaniesAllowed;
        public string MaximumNumberOfUsersAllowed;
        public string ProductId;

        public string DBServerName;
        public string DBServerIP;
        public string ApplicationServerName;
        public string ApplicationServerIP;
        public string ExpiryDate;
        public string MACId;

        public string AMCStartDate;
        public string AMCEndDate;

        public string RegistrationDate;
        public string RegistrationValue;
    }

    [DataContract(Namespace = "www.udyogsoftware.com/Registration/2010/06")]
    public struct AddressInfo
    {
        public string Line1;
        public string Line2;
        public string Line3;
        public string Line4;

        public string Location;
        public string Route;
        public string Zip;
        public string Contact;
        public string EMail;

        public string Tel1;
        public string Tel2;

        public string Mobile;
        public string Fax;
        public string Web;
    }
}
