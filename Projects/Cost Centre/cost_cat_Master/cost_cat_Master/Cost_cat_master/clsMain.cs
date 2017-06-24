using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Cost_cat_master
{
    class clsMain
    {
        public clsMain(string[] args)
        {
            Cost_Cat_Mast oFrmDynamic = new Cost_Cat_Mast();
            oFrmDynamic.pFrmCaption = "Cost Centre Category Master"; //Birendra : Bug-12497 on 15/04/2013
            oFrmDynamic.pCompId = Convert.ToInt16(args[0]);
            oFrmDynamic.pComDbnm = args[1];
            oFrmDynamic.pServerName = args[2];
            oFrmDynamic.pUserId = args[3];
            oFrmDynamic.pPassword = args[4];
            if (args[5] != "")
            {
                oFrmDynamic.pPApplRange = args[5].ToString().Replace("^", "");
            }
            oFrmDynamic.pPApplRange = args[5];
            oFrmDynamic.pPApplRange = args[5];
            oFrmDynamic.pAppUerName = args[7];
            Icon MainIcon = new System.Drawing.Icon(args[8].Replace("<*#*>", " "));
            oFrmDynamic.pFrmIcon = MainIcon;
            oFrmDynamic.pPApplText = args[9].Replace("<*#*>", " ");
            oFrmDynamic.pPApplName = args[10];
            oFrmDynamic.pPApplPID = Convert.ToInt16(args[11]);
            oFrmDynamic.pPApplCode = args[12];
            oFrmDynamic.PMasterCode = args[6];
            Application.Run(oFrmDynamic);

        }

    }
}
