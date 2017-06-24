using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoLib
{
    public class CryptoConn
    {
        public CryptoConn()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public string GetConnString(string strConnName)
        {
            string strEncString = System.Configuration.ConfigurationManager.AppSettings.Get(strConnName);
            return new CryptoLib().Decrypt(strEncString);
        }

        private void SetConnString(string strConnName, string strConnString)
        {
            System.Configuration.ConfigurationManager.AppSettings.Set(strConnName, new CryptoLib().Encrypt(strConnString));
        }
    }
}
