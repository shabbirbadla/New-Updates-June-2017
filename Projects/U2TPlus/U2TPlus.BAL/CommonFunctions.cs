using System;
using System.Collections.Generic;
using System.Text;

namespace U2TPlus.BAL
{
    public class CommonFunctions
    {
        /// <summary>
        /// Convert Special Carectors in to XML format like & to &amp; ect..
        /// </summary>
        /// <param name="ConvertingValue"></param>
        /// <returns>string</returns>
        public string convertSpecialChars(string ConvertingValue)
        {
            try
            {
                if (ConvertingValue.Contains("&"))
                    ConvertingValue = ConvertingValue.Replace("&", "&amp;");
                if (ConvertingValue.Contains("<"))
                    ConvertingValue = ConvertingValue.Replace("<", "&lt;");
                if (ConvertingValue.Contains(">"))
                    ConvertingValue = ConvertingValue.Replace(">", "&gt;");
                if (ConvertingValue.Contains('"'.ToString()))
                    ConvertingValue = ConvertingValue.Replace('"'.ToString(), "&quot;");
                if (ConvertingValue.Contains("'"))
                    ConvertingValue = ConvertingValue.Replace("'", "&apos;");
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex);
            }

            return ConvertingValue;
        }
    }
}
