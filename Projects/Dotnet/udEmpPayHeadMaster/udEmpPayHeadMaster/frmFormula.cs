using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace udEmpPayHeadMaster
{
    public partial class frmFormula : uBaseForm.FrmBaseForm 
    {
        uBaseForm.FrmBaseForm vParentForm;
        string vFormula, vParaList=string.Empty;
        DataAccess_Net.clsDataAccess oDataAccess;
        String SqlStr = string.Empty;
        DataView dvw;
        public frmFormula()
        {
            InitializeComponent();
        }

        private void frmFormula_Load(object sender, EventArgs e)
        {
            if (this.pAddMode == false && this.pEditMode == false)
            {
                //this.gridTable.Enabled = false;
            }
            string[] args = { };
            if (this.pPara != null)
            {
                args = this.pPara;
            }
            if (args.Length < 1)
            {
                //args = new string[] { "19", "A021112", "desktop246", "sa", "sa@1985", "^18010", "ADMIN", @"D:\Usquare\Bmp\Icon_usquare.ico", "Usquare pack", "Usquare.exe", "1", "udpid6096DTM20110307112715" };/*Rup Add ICO Path,Parent Appl Caption,Parent Appl. Name,Parent Appl PId,Application Log Table*/
                args = new string[] { "19", "A041415", "PRO_PANKAJ\\USQUARE", "sa", "sa1985", "13032", "TUM", "ADMIN", @"D:\USquare\Bmp\Icon_usquare.ico", "Usquare pack", "Usquare.exe", "1", "udpid6096DTM20110307112715" };/*Rup Add ICO Path,Parent Appl Caption,Parent Appl. Name,Parent Appl PId,Application Log Table*/
            }

            this.pFrmCaption = "USer Roles EmpEdMasterils";
            this.pComDbnm = args[1];
            this.pServerName = args[2];
            this.pUserId = args[3];
            this.pPassword = args[4];
            this.pPApplRange = args[5];
            this.pAppUerName = args[6];
            Icon MainIcon = new System.Drawing.Icon(args[7].Replace("<*#*>", " "));
            this.pFrmIcon = MainIcon;
            this.pPApplText = args[8].Replace("<*#*>", " ");
            this.pPApplName = args[9];
            this.pPApplPID = Convert.ToInt16(args[10]);
            this.pPApplCode = args[11];

            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();
            
            this.rtxtExpression.Text = this.pFormula;

            SqlStr = "select t.[Name]+'.'+c.[Name] as [FldName] from sysobjects t inner join SysColumns c on (t.[id]=c.[id]) where t.[Name]='Emp_Pay_Head_Master' and t.xType='U'";
            SqlStr = SqlStr + " union select t.[Name]+'.'+c.[Name] as [FldName] from sysobjects t inner join SysColumns c on (t.[id]=c.[id]) where t.[Name]='EmployeeMast' and t.xType='U'";
            SqlStr = SqlStr + " union select t.[Name]+'.'+c.[Name] as [FldName] from sysobjects t inner join SysColumns c on (t.[id]=c.[id]) where t.[Name]='Emp_ED_EmpEdMasterils' and t.xType='U'";
            SqlStr = SqlStr + " union select t.[Name]+'.'+c.[Name] as [FldName] from sysobjects t inner join SysColumns c on (t.[id]=c.[id]) where t.[Name]='Emp_monthly_Payroll' and t.xType='U'";
            SqlStr = SqlStr + " union select t.[Name]+'.'+c.[Name] as [FldName] from sysobjects t inner join SysColumns c on (t.[id]=c.[id]) where t.[Name]='Emp_Payment_Det' and t.xType='U'";

            DataSet dsEarn = new DataSet();
            dsEarn = oDataAccess.GetDataSet(SqlStr, null, 20);
            if (this.pAddMode==true  && String.IsNullOrEmpty(this.pDataSet.Tables[0].Rows[0]["fld_Nm"].ToString())==false)
            {
                dsEarn.Tables[0].Rows.Add("Emp_Payment_Det." + ((string)this.pDataSet.Tables[0].Rows[0]["fld_Nm"]));
            }
            dvw = new DataView();
            dvw = dsEarn.Tables[0].DefaultView;
            dvw.Sort= "FldName" ;
            lstFldList.DataSource = dvw ;
            lstFldList.DisplayMember = "FldName";

        }
       
        public string  pFormula
        {
            get { return vFormula; }
            set { vFormula = value; }
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            uBaseForm.FrmBaseForm ofrm = new uBaseForm.FrmBaseForm();
            ofrm = this.pParentForm;
            Type tfrm = ofrm.GetType();

            DataSet dsTemp = new DataSet();
            dsTemp = (DataSet)tfrm.GetProperty("pDataSet").GetValue(ofrm, null);
            dsTemp.Tables[0].Rows[0]["Formula"] = this.rtxtExpression.Text;
            tfrm.GetProperty("pDataSet").SetValue(this.pParentForm, dsTemp, null);
            this.Close();
            
        }

        private void lstVeriable_SelectedIndexChanged(object sender, EventArgs e)
        {
            Clipboard.SetDataObject("{" + lstVeriable.SelectedItem.ToString() + "}");
            PasteText();
            
        }
        private void PasteText()
        {
            rtxtExpression.Paste();
            rtxtExpression.Focus();
            SendKeys.SendWait("^c");//Added By pankaj For Bug-25365 on 24-03-2015
            Clipboard.SetDataObject("");
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject("+");
            PasteText();
        }

        private void btnMinus_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject("-");
            PasteText();
        }

        private void btnMulti_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject("*");
            PasteText();
        }

        private void btnbtnDevision_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject("/");
            PasteText();
        }

        private void btnGrater_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(">");
            PasteText();
        }

        private void btnLess_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject("<");
            PasteText();
        }

        private void btnEqual_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject("=");
            PasteText();
        }

        private void btnNotEqual_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject("<>");
            PasteText();
        }

        private void btnLessEqual_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject("<=");
            PasteText();
        }

        private void btnGraterEqual_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(">=");
            PasteText();
        }

        private void btnOpenBrac_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject("(");
            PasteText();
        }

        private void btnCloseBrac_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(")");
            PasteText();
        }

        

        private void lstFldList_DoubleClick(object sender, EventArgs e)
        {
            Clipboard.SetDataObject("{" + lstFldList.Text + "}");
            PasteText();
        }
        public uBaseForm.FrmBaseForm pParentForm
        {
            get { return vParentForm; }
            set { vParentForm = value; }
        }
    }
}
