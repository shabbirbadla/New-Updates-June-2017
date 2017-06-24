using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace uTextBox
{
    public class cTextBox : TextBox
    {
        string vHelpQuery = string.Empty;
        string vCaption = string.Empty;
        string vToolTip = string.Empty;
        string vDataType = string.Empty;
        Boolean  vMandatory = false ;
        Boolean vUnique = false;
        string vInputMask = string.Empty;
       
        //string vRemarks = string.Empty;
        string vReadOnly = string.Empty;
        string vDefaultValue = string.Empty;
        string vValidation = string.Empty;
        string vErrorMsg = string.Empty;
        Boolean vInternalUse = false ;
        
        public string pCaption
        {
            get
            {
                return vCaption;
            }
            set
            {
                vCaption = value;
            }
        }
        public string pToolTip
        {
            get
            {
                return vToolTip;
            }
            set
            {
                vToolTip = value;
            }
        }
        public string pDataType
        {
            get
            {
                return vDataType;
            }
            set
            {
                vDataType = value;
            }
        }
        public Boolean pMandatory 
        {
            get
            {
                return vMandatory;
            }
            set
            {
               vMandatory  = value;
            }
        }
        public Boolean pUnique 
        {
            get
            {
                return vUnique;
            }
            set
            {
               vUnique  = value;
            }
        }
        public string pInputMask
        {
            get
            {
                return vInputMask;
            }
            set
            {
                 vInputMask= value;
            }
        }
        public string pHelpQuery
        {
            get
            {
                return vHelpQuery;
            }
            set
            {
                vHelpQuery = value;
            }
        }
        //public string pRemarks
        //{
        //    get
        //    {
        //        return pRemarks;
        //    }
        //    set
        //    {
        //        pRemarks = value;
        //    }
        //}
        public string pReadOnly
        {
            get
            {
                return vReadOnly;
            }
            set
            {
               vReadOnly  = value;
            }
        }
        public string pDefaultValue
        {
            get
            {
                return vDefaultValue;
            }
            set
            {
                vDefaultValue = value;
            }
        }
        public string pValidation
        {
            get
            {
                return vValidation;
            }
            set
            {
                 vValidation= value;
            }
        }
        public string pErrorMsg
        {
            get
            {
                return vErrorMsg;
            }
            set
            {
                vErrorMsg = value;
            }
        }
        public Boolean pInternalUse
        {
            get
            {
                return vInternalUse;
            }
            set
            {
                vInternalUse = value;
            }
        }
      
       
       

    }
}
