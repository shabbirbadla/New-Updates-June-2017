using System;
using System.Collections.Generic;
using System.Text;
using CryptoLib;

namespace LogicalLibrary
{
    public class EncriptionValues
    {
        CryptoLib.CryptoLib cryptolib = new CryptoLib.CryptoLib(); 
        public string EncriptValue(string Encriptvalue)
        {
            return cryptolib.Encrypt(Encriptvalue);
        }

    }
}
