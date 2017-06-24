using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace uHelpButton
{
    public class cHelpButton : Button
    {
        string vHelpQuery = string.Empty;
        string vSearchFieldQuery=string.Empty;
        string vCaption = string.Empty;
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
        public String  pSearchFieldQuery
        {
            get
            {
                return vSearchFieldQuery;
            }
            set
            {
                vSearchFieldQuery = value;
            }
        }
        public String pCaption
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

    }
}
