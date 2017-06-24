using System;
using System.Collections.Generic;
using System.Text;

namespace CUDF
{
    public class UDF
    {
        public bool fIsDate(string  vDate)
        {
            bool result;
            try
            {
                DateTime myDT = DateTime.Parse(vDate);
                result = true;
            }
            catch (FormatException e)
            {
                result = false;
            }
                return result;
       }


        public bool fIsNumber(string sNumber)
        {
            bool Result = true;
            for (int i = 0; i < sNumber.Length; i++)
            {
                if (!Char.IsNumber(sNumber, i))
                {
                    Result = false;
                }
            }
            return Result;
        }



    }
}
