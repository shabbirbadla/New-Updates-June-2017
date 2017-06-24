using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;


namespace ueProductUpgrade
{
    class Utilities
    {
        /// <summary>
        /// This method is used for getting product code from Company Master
        /// </summary>
        /// <param name="code">String to decrypt</param>
        /// <returns></returns>
        public static string GetDecProductCode(string code)
        {
            string decryptedStr = string.Empty;
            for (int i = 0; i < code.Length; i++)
            {
                decryptedStr = decryptedStr + Chr(Asc(code[i].ToString()) / 2);
            }
            return decryptedStr;
        }

        public static string GetEncProductCode(string ProdCode)
        {
            //ProdCode = ProdCode.Replace(",", "");
            string retProd = string.Empty;
            for (int i = 0; i < ProdCode.Length; i++)
            {
                retProd = retProd + Chr(Asc(ProdCode[i].ToString()) * 2);
            }
            return retProd;
        }

        public static string GetDecoder(string value,bool flag)
        {
           // string mVal = "IYCYDYPYVY";
            string finalCode = string.Empty;
            for (int i = 0; i < value.Length; i++)
            {
                if (flag!=true)
                    finalCode = finalCode + Chr(Asc(value[i].ToString()) - 4);
                else
                    finalCode = finalCode + Chr(Asc(value[i].ToString()) + 4);
            }
            return finalCode;
        }

        //public static string GetRights()
        //{
        //    string mVal="IYCYDYPYVY";
        //    string finalCode=string.Empty;
        //    for (int i = 0; i < mVal.Length; i++)
        //    {
        //        finalCode =finalCode+ Chr(Asc(mVal[i].ToString()) - 4);
        //    }
        //    return finalCode;
        //}

        //public static string GetRoles()
        //{
        //    string mVal = "ADMINISTRATOR";
        //    string finalCode = string.Empty;
        //    for (int i = 0; i < mVal.Length; i++)
        //    {
        //        finalCode = finalCode + Chr(Asc(mVal[i].ToString()) + 4);
        //    }
        //    return finalCode;
        //}

        /// <summary>
        /// This method is used for getting the character of the value
        /// </summary>
        /// <param name="intByte"></param>
        /// <returns></returns>
        public static string Chr(int intByte)
        {
            byte[] bytBuffer = new byte[] { (byte)intByte };
            return Encoding.GetEncoding(1252).GetString(bytBuffer);
        }
        /// <summary>
        /// This method is used for getting the ascii value of the character
        /// </summary>
        /// <param name="strChar"></param>
        /// <returns></returns>
        public static int Asc(string strChar)
        {
            char[] chrBuffer = { Convert.ToChar(strChar) };
            byte[] bytBuffer = Encoding.GetEncoding(1252).GetBytes(chrBuffer);
            return (int)bytBuffer[0];
        }
        public static string enc(string strToenc)
        {
            int d = 1;
            int F = strToenc.Length;
            string Repl = string.Empty;
            string r, two;
            int rep = 0, Change;
            while (F > 0)
            {
                r = strToenc.Substring(d, 1);
                Change = Asc(r) + rep;
                two = Chr(Change);
                Repl = Repl + two;
                d = d + 01;
                rep = rep + 1;
                F = F - 1;
            }
            return Repl;
        }
        public static string dec(string strTodec)
        {
            int d = 1;
            int F = strTodec.Length;
            string Repl = string.Empty;
            string r, two = "";
            int rep = 0, Change;
            while (F > 0)
            {
                r = strTodec.Substring(d, 1);
                Change = Asc(r) - rep;
                if (Change > 0)
                    two = Chr(Change);
                Repl = Repl + two;
                d = d + 01;
                F = F - 1;
                rep = rep + 1;
            }
            return Repl;
        }
        public static string onencrypt(string strToOnEnc)
        {
            string lcreturn = string.Empty;
            for (int i = 0; i < strToOnEnc.Length; i++)
            {
                lcreturn = lcreturn + Chr(Asc(strToOnEnc[i].ToString())) + Asc(strToOnEnc[i].ToString());
            }
            return lcreturn;
        }
        public static string ondecrypt(string strToOnDec)
        {
            string lcreturn = string.Empty;
            for (int i = 0; i < strToOnDec.Length; i++)
            {
                lcreturn = lcreturn + Chr(Asc(strToOnDec[i].ToString()) / 2);
            }
            return lcreturn;
        }
        public static string ReverseString(string s)
        {
            char[] arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }
        public static bool InList(string searchStr, string[] strList)
        {
            for (int i = 0; i < strList.Length; i++)
            {
                if (searchStr == strList[i])
                {
                    return true;
                }
            }
            return false;
        }

    }
}
