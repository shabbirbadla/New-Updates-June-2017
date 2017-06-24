using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using System.Windows.Forms;

namespace udDnMastBefDel
{
    public class udDnMastBefDel
    {
        string vErrMsg = string.Empty;
        DataTable dtTemp;
        
        public string pErrMsg
        {
            set
            {
                vErrMsg = value;
            }
            get
            {
                return vErrMsg;
            }
        }
        
        public void CheckBefore(string MasterCode,DataTable Main_vw, DataAccess_Net.clsDataAccess oDataAccess)
        {
            string sqlstr;
            if (MasterCode == "QPR" && Main_vw.Rows.Count > 0)
            {
                sqlstr = "select qc_para from Qc_process_item where qc_para='"+Main_vw.Rows[0]["QC_Para"].ToString() +"'";
                dtTemp = oDataAccess.GetDataTable(sqlstr, null, 20);
                if (dtTemp.Rows.Count >0)
                pErrMsg = "Parameter used Can't Delete.";
            }
        }
    }
}
