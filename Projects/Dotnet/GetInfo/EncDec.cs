using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace GetInfo
{
    public class EncDec
    {
        ///// <summary>
        ///// This method is used in Double Encryption of the string
        ///// </summary>
        public string OnEncrypt(string type,string encStr)
        {
            if (type != "myName")
            {
                return string.Empty;
            }
            string encryptedStr = "";
            for (int i = 0; i < encStr.Length; i++)
            {
                encryptedStr = encryptedStr + Chr(Asc(encStr[i].ToString()) + Asc(encStr[i].ToString()));
            }
            return encryptedStr;
        }
        ///// <summary>
        ///// This method is used in Double Decryption of the string
        ///// </summary>
        public string OnDecrypt(string type,string encStr)
        {
            if (type != "myName")
            {
                return string.Empty;
            }
            string decryptedStr = "";
            for (int i = 0; i < encStr.Length; i++)
            {
                decryptedStr = decryptedStr + Chr(Asc(encStr[i].ToString()) / 2);
            }
            return decryptedStr;
        }

        ///// <summary>
        ///// Encrypt a string using Encryption 
        ///// </summary>
        public string Enc(string type,string text)
        {
            if (type != "myName")
            {
                return string.Empty;
            }
            int D; int rep; int Change;
            int F; string finalString = string.Empty; string R; string two;
            D = 0;
            F = text.Length;
            rep = 0;
            while (F > 0)
            {
                R = text.Substring(D, 1);
                int a = Asc(R);
                string ch = Chr(a);
                Change = a + rep;
                //if (Change > 255)
                //{
                //}
                two = Chr(Change);
                finalString = finalString + two;
                D = D + 1;
                rep = rep + 1;
                F = F - 1;
            }
            return finalString;
        }


        //<summary>
        //Decryption of a string 
        //</summary>
        public string Dec(string type,string text)
        {
            if (type != "myName")
            {
                return string.Empty;
            }
            int D; int rep; int Change;
            int F; string finalString = string.Empty; string R; string two;
            D = 0;
            F = text.Length;
            rep = 0;
            while (F > 0)
            {
                R = text.Substring(D, 1);
                int a = Asc(R);
                string ch = Chr(a);
                Change = a - rep;
                two = Chr(Change);
                finalString = finalString + two;
                D = D + 1;
                rep = rep + 1;
                F = F - 1;
            }
            return finalString;
        }
        public string Chr(int intByte)
        {
            byte[] bytBuffer = new byte[] { (byte)intByte };
            return Encoding.GetEncoding(1252).GetString(bytBuffer);
        }
        private int Asc(string strChar)
        {
            char[] chrBuffer = { Convert.ToChar(strChar) };
            byte[] bytBuffer = Encoding.GetEncoding(1252).GetBytes(chrBuffer);
            return (int)bytBuffer[0];
        }
    }

}
