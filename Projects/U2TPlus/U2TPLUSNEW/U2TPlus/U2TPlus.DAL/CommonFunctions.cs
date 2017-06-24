using System;
using System.Collections.Generic;
using System.Text;
using CryptoLib;


namespace U2TPlus.DAL
{
    public class CommonFunctions
    {
        /// <summary>
        /// Encript the given String, Returns Encripted string
        /// </summary>
        /// <param name="StrValue"></param>
        /// <returns>string</returns>
        public string EncriptValue(string StrValue)
        {
            string ReturnValue = string.Empty;

            ReturnValue = new CryptoLib.CryptoLib().Encrypt(StrValue);

            return ReturnValue;
        }
        /// <summary>
        /// Decript the given String, Returns string
        /// </summary>
        /// <param name="EncriptedValue"></param>
        /// <returns>string</returns>
        public string DecriptValue(string EncriptedValue)
        {
            string ReturnValue = new CryptoLib.CryptoLib().Decrypt(EncriptedValue);

            return ReturnValue;
        }
    }
}
