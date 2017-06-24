using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace Udyog.Application.License
{
    //[ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(ILicenseCallback))]
    [ServiceContract()]
    public interface ILicenseContract
    {
        [OperationContract]
        string ValidateLicense(string securityCode);

        [OperationContract]
        string GetConnection(string securityCode);

        [OperationContract]
        void CloseConnection(string securityCode);

        [OperationContract]
        string ExtendConnection(string securityCode);

        [OperationContract]
        int GetTotalConnectionsPermitted();

        [OperationContract]
        int GetNumberOfConnectedClients();

        [OperationContract]
        void NotifyClientCountChange(int clientCount);

        [OperationContract]
        string CheckConnection();

        [OperationContract]
        RegisterMeInfo GetInfo();

        //Added by Shrikant S. on 09/07/2013 for  Bug-16557     //Start    
        [OperationContract]
        bool IsUserAlreadyLoggedIn(string userName, string code);
        //Added by Shrikant S. on 09/07/2013 for  Bug-16557     //End
    }

    //public interface ILicenseCallback
    //{
    //    [OperationContract(IsOneWay = true)]
    //    void NotifyNewClientsConnection(int totalClientCount);

    //    [OperationContract(IsOneWay = true)]
    //    void NofityClientForSessionRenewal();
    //}
}
