using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
/// <summary>
/// A parameter for query execution.
/// </summary>
/// <remarks></remarks>
/// 

namespace DataAccess_Net
{
    public class clsParam
    {

        #region "Fields"

        /// <summary>
        /// The name of the parameter
        /// </summary>
        /// <remarks></remarks>

        public string Name;
        /// <summary>
        /// The value
        /// </summary>
        /// <remarks></remarks>

        public object Value;
        /// <summary>
        /// Should we pass a DBNull instead of value if Value = NullValue
        /// </summary>
        /// <remarks></remarks>

        public bool WrapNull;
        /// <summary>
        /// Value that should be considered a null, for a long this might be 0, for a string maybe ""
        /// </summary>
        /// <remarks></remarks>

        public object NullValue;
        /// <summary>
        /// The basic datatype of the parameter
        /// </summary>
        /// <remarks></remarks>

        public pType ParamType;
        /// <summary>
        /// The direction of the parameter.
        /// </summary>
        /// <remarks></remarks>

        public pInOut InOrOut;

        /// <summary>
        /// Table name that this parameter applies to.
        /// </summary>
        /// <remarks></remarks>

        public string TableName;
        /// <summary>
        /// Field name that this parameter applies to.
        /// </summary>
        /// <remarks></remarks>

        public string FieldName;

        #endregion

        #region "Enums"

        /// <summary>
        /// Parameter Datatype.
        /// </summary>
        /// <remarks></remarks>
        public enum pType
        {
            pLong = 1,
            pDate = 2,
            pString = 3,
            pFloat = 4,
            pOracleCLOB = 5
        }

        /// <summary>
        /// The direction of the parameter
        /// </summary>
        /// <remarks></remarks>
        public enum pInOut
        {
            pIn = 1,
            pOut = 2
        }

        #endregion

        #region "Construction"


        public clsParam()
        {
        }


        public clsParam(string strName, object objValue, pType pParamType, List<clsParam> colParams, bool bWrapNull, object objNullValue, pInOut pInOrOut)
        {
            if (colParams == null)
            {
                colParams = new List<clsParam>();
            }

            //Always append the number so that we dont run into issue with oracle param names
            Name = strName;

            Value = objValue;
            ParamType = pParamType;
            WrapNull = bWrapNull;

            if (pParamType == pType.pLong & !string.IsNullOrEmpty(Convert.ToString(objNullValue)))
            {
                NullValue = Convert.ToInt64(objNullValue);
            }
            else
            {
                NullValue = objNullValue;
            }

            InOrOut = pInOrOut;

            colParams.Add(this);

        }

        public clsParam(string strName, object objValue, pType pParamType, List<clsParam> colParams, string strTableName, string strFieldName, bool bWrapNull, object objNullValue, pInOut pInOrOut)
        {
            if (colParams == null)
            {
                colParams = new List<clsParam>();
            }

            //Always append the number so that we dont run into issue with oracle param names
            Name = strName;

            Value = objValue;
            ParamType = pParamType;
            WrapNull = bWrapNull;

            if (pParamType == pType.pLong & !string.IsNullOrEmpty(Convert.ToString(objNullValue)))
            {
                NullValue = Convert.ToInt64(objNullValue);
            }
            else
            {
                NullValue = objNullValue;
            }

            InOrOut = pInOrOut;

            TableName = strTableName;
            FieldName = strFieldName;

            colParams.Add(this);
        }

        #endregion

    }    
}
