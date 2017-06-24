using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using udclsDGVNumericColumn;
using System.Windows.Forms;
using System.Data;
using DataAccess_Net;

namespace udEmpColValid
{
    public class cEmpColValid
    {
        string vValidFunNm;
        public void mthNumericColumnCalculate(ref DataGridView dgv, int vRowIndex, DataAccess_Net.clsDataAccess oDataAccess)
        {

            /*==========Customization Part==========*/


            //Decimal ESICEmpRPer = 0, ESICEmpEPer = 0;
            //DataTable dt = new DataTable();
            //string SqlStr = "Select Def_Rate,fld_nm,Def_Amt From Emp_Pay_Head_Master where fld_nm in ('ESICEmpR','ESICEmpE')";
            //dt = oDataAccess.GetDataTable(SqlStr, null, 30);
            //if (dt.Rows.Count > 1)
            //{
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        if (dr["fld_nm"].ToString().Trim() == "ESICEmpR")
            //        {
            //            ESICEmpRPer = (decimal)dr["Def_Rate"];
            //        }
            //        if (dr["fld_nm"].ToString().Trim() == "ESICEmpE")
            //        {
            //            ESICEmpEPer = (decimal)dr["Def_Rate"];
            //        }
            //    }
            //}

            //if (pValidFunNm.ToUpper() == "EXPESIC") /*Here EXPESIC means Net Expression you have given for dependent payheads*/
            //{
            //    Decimal s = (decimal)dgv.Rows[vRowIndex].Cells["Col_BasicAmt"].Value + (decimal)dgv.Rows[vRowIndex].Cells["Col_BonusAmt"].Value + (decimal)dgv.Rows[vRowIndex].Cells["Col_CONWAMT"].Value + (decimal)dgv.Rows[vRowIndex].Cells["Col_DAAMT"].Value + (decimal)dgv.Rows[vRowIndex].Cells["Col_HRAAmt"].Value + (decimal)dgv.Rows[vRowIndex].Cells["Col_MediAmt"].Value + (decimal)dgv.Rows[vRowIndex].Cells["Col_dAllowAmt"].Value + (decimal)dgv.Rows[vRowIndex].Cells["Col_ArrAmt"].Value + (decimal)dgv.Rows[vRowIndex].Cells["Col_oAllowAmt"].Value + (decimal)dgv.Rows[vRowIndex].Cells["Col_OtWagAmt"].Value + (decimal)dgv.Rows[vRowIndex].Cells["Col_HOT_Amt"].Value + (decimal)dgv.Rows[vRowIndex].Cells["Col_JBonusAmt"].Value + (decimal)dgv.Rows[vRowIndex].Cells["Col_attair"].Value + (decimal)dgv.Rows[vRowIndex].Cells["Col_PFPT"].Value + (decimal)dgv.Rows[vRowIndex].Cells["Col_Lunch"].Value + (decimal)dgv.Rows[vRowIndex].Cells["Col_UOTHEAR"].Value + (decimal)dgv.Rows[vRowIndex].Cells["Col_TELALWEAR"].Value + (decimal)dgv.Rows[vRowIndex].Cells["Col_PERALLEAR"].Value + (decimal)dgv.Rows[vRowIndex].Cells["Col_UEAMT"].Value;
            //    if (s <= 15000)
            //    {
            //        if (ESICEmpRPer != 0)  /*If default rate is there then you need to use formula like this*/
            //        {
            //            dgv.Rows[vRowIndex].Cells["Col_ESICEmpR"].Value = Math.Round((ESICEmpRPer * s) / 100);
            //        }
            //        if (ESICEmpEPer != 0)
            //        {
            //            dgv.Rows[vRowIndex].Cells["Col_ESICEmpE"].Value = Math.Round((ESICEmpEPer * s)/ 100);
            //        }
            //    }
            //    else
            //    {
            //         dgv.Rows[vRowIndex].Cells["Col_ESICEmpE"].Value = 0;
            //         dgv.Rows[vRowIndex].Cells["Col_ESICEmpR"].Value=0;
            //    }

            //}


            /*==========Customization Part==========*/

        }
        public string pValidFunNm
        {
            get { return vValidFunNm; }
            set { vValidFunNm = value; }
        }
    }
}
