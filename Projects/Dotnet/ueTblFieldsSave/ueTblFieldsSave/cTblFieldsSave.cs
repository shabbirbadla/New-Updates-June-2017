using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using uBaseForm;
using System.Data;
using DataAccess_Net;
namespace ueTblFieldsSave
{
    public class cTblFieldsSave
    {
        Int16 vCompId;
        DataAccess_Net.clsDataAccess oDataAccess;

        string vComDbnm, vServerName, vUserId, vPassword ;
        string SqlStr;
        public void mthBtnUpdate(Form pForm, object sender, EventArgs e, DataSet ds, string[] args,String ModuleName)
        {
            uBaseForm.FrmBaseForm oParentForm = new uBaseForm.FrmBaseForm();
            oParentForm = (uBaseForm.FrmBaseForm)pForm;

            if (args.Length < 1)
            {
                args = new string[] { "19", "A021112", "udyog65", "sa", "sa@1985", "13032", "IEM", "ADMIN", @"D:\USquare\Bmp\Icon_usquare.ico", "Usquare pack", "Usquare.exe", "1", "udpid6096DTM20110307112715" };
            }
   
            this.vCompId = Convert.ToInt16(args[0]);
            this.vComDbnm = args[1];
            this.vServerName = args[2];
            this.vUserId = args[3];
            this.vPassword = args[4];
            
            

            DataAccess_Net.clsDataAccess._databaseName = this.vComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.vServerName;
            DataAccess_Net.clsDataAccess._userID = this.vUserId;
            DataAccess_Net.clsDataAccess._password = this.vPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();
            this.mthModUpdt(ModuleName);
                       
        }
        private void mthModUpdt(string ModuleName)
        {

            switch (ModuleName.Trim())
            {
                   
                case "Stock Valuation Rate Update Utility":
                    SqlStr = "update aritem set aaa=qty*bbb where bbb<>0 and aaa=0";
                    break;
                case "Transporter Details Updation In Sales Transaction":
                     SqlStr = "update stmain set u_status=case when (ISNULL(u_lrno,'') ='' or  YEAR(u_lrdt)='1900' ) then 'HOLD' else 'DONE' end ";
                     oDataAccess.ExecuteSQLStatement(SqlStr, null, 30, true);
                     break;

            }
        }
          //this.pDataSet = dsMain;
          //  ueDynamicMasterProcedures.cDynamicMasterProcedures oDynamicproc = new ueDynamicMasterProcedures.cDynamicMasterProcedures();
          //  oDynamicproc.mthBtnClick(this,sender, e,dsMain );
         //Type t = extform.GetType();
           //     t.GetProperty("pAddMode").SetValue(extform, oParentForm.pAddMode, null);
    }
}
