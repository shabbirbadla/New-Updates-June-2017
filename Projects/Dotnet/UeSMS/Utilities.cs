using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UeSMS
{
    class Utilities
    {
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
            int d = 0;
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
            int d = 0;
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
    }
}
