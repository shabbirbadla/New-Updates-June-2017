using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using uBaseForm;
using  DataAccess_Net;
using udclsUDF;

using DevExpress.Data;
using DevExpress.LookAndFeel;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Data.SqlClient;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Design;
using DevExpress.XtraGrid.Views;


namespace udUserRolesSelection
{
    public partial class frmMainUserRoles : uBaseForm.FrmBaseForm
    {
        DataAccess_Net.clsDataAccess oDataAccess;
        udclsUDF.udclsUDF oudclsUDF = new udclsUDF.udclsUDF();
        uBaseForm.FrmBaseForm vParentForm;
        DataTable dt;

        public frmMainUserRoles()
        {
            InitializeComponent();
           

        }

        private void frmUserRoleSelection_Load(object sender, EventArgs e)
        {
            if (this.pAddMode == false && this.pEditMode == false)
            {
                this.grdMain.Enabled = false;
            }
            string[] args={};
            if (this.pPara != null)
            {
                args = this.pPara;
            }
            if (args.Length < 1)
            {
                args = new string[] { "19", "A021112", "desktop246", "sa", "sa@1985", "^18010", "ADMIN", @"D:\Usquare\Bmp\Icon_usquare.ico", "Usquare pack", "Usquare.exe", "1", "udpid6096DTM20110307112715" };/*Rup Add ICO Path,Parent Appl Caption,Parent Appl. Name,Parent Appl PId,Application Log Table*/
            }

            this.pFrmCaption = "USer Roles Details";
            this.pCompId = Convert.ToInt16(args[0]);
            this.pComDbnm = args[1];
            this.pServerName = args[2];
            this.pUserId = args[3];
            this.pPassword = args[4];
            Icon MainIcon = new System.Drawing.Icon(args[8].Replace("<*#*>", " "));
            this.pFrmIcon = MainIcon;
            this.pPApplText = args[9].Replace("<*#*>", " ");
            this.pPApplName = args[10];
            this.pPApplPID = Convert.ToInt16(args[11]);
            this.pPApplCode = args[12];

            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();
            this.mthGridFill();
            this.SetFormColor();
        }
        private void mthGridFill()
        {
            string vvRoles=string.Empty;
            uBaseForm.FrmBaseForm ofrm = new uBaseForm.FrmBaseForm();
            if (this.pParentForm != null)
            {
                ofrm = this.pParentForm;
                DataSet vds = new DataSet();
                vds = (DataSet)ofrm.GetType().GetProperty("pDataSet").GetValue(ofrm, null);
                if ((vds.Tables[0].Rows[0]["paralist"]) != null)
                {
                    vvRoles = vds.Tables[0].Rows[0]["userroles"].ToString();
                }
            }

            dt = new DataTable();
            DataColumn dcol1 = new DataColumn();
            DataColumn dcol2 = new DataColumn();
            dcol1.ColumnName = "sel";
            dcol1.DataType = System.Type.GetType("System.Boolean");
            dt.Columns.Add(dcol1);

            dcol2.ColumnName = "roles";
            dcol2.DataType = System.Type.GetType("System.String");
            dt.Columns.Add(dcol2);

            DataSet dsRoles = new DataSet();
            DataSet dsCoMast = new DataSet();
            string strSQL = "select * from vudyog..co_mast where compid=" + this.pCompId;
            dsCoMast = oDataAccess.GetDataSet(strSQL, null, 20);
            string vmRoles = string.Empty,vCompany=string.Empty,vRoles=string.Empty,vCompanyList=string.Empty,vCompName=string.Empty;
            strSQL = "select user_roles,cast(company as varchar(8000)) as company from vudyog..userroles";
            vCompName = dsCoMast.Tables[0].Rows[0]["co_name"].ToString().Trim() + "(" + ((DateTime)dsCoMast.Tables[0].Rows[0]["sta_dt"]).Year.ToString().Trim() + "-" + ((DateTime)dsCoMast.Tables[0].Rows[0]["end_dt"]).Year.ToString().Trim() + ")";
            dsRoles = oDataAccess.GetDataSet(strSQL, null, 20);
            foreach (DataRow dr in dsRoles.Tables[0].Rows)
            {
                vmRoles = dr["user_roles"].ToString();
                vCompany = dr["company"].ToString();
                int len = vCompany.Length;
                int n;
                vRoles = oudclsUDF.padRStr(vmRoles.Trim(), vmRoles.Trim(), len); 
                vCompanyList = "";
                for (int j = 0; j <= len - 1; j++)
                {
                    n = oudclsUDF.fascii(vCompany.Substring(j, 1)) - oudclsUDF.fascii(vRoles.Substring(j, 1));
                    if (n > 0)
                    {
                        vCompanyList = vCompanyList + Convert.ToChar(n).ToString();
                    }
                    
                }
                if (vCompanyList.IndexOf(vCompName) > -1)
                {
                    DataRow dr1=dt.NewRow();
                    dr1["sel"] = false;
                    if (!string.IsNullOrEmpty(vvRoles))
                    {
                        if (vvRoles.IndexOf("<<"+vmRoles.Trim()+">>") > -1)
                        {
                            dr1["sel"] = true ;
                        }
                    }
                    dr1["roles"] = vmRoles.Trim();
                    dt.Rows.Add(dr1);

                }
            }
            

            grdMainvw.OptionsView.ColumnAutoWidth = false;
            grdMainvw.OptionsView.RowAutoHeight = false;
            grdMainvw.OptionsView.ShowIndicator = false;

            grdMain.DataSource = dt;
            grdMainvw.Columns[0].Caption = "Select";
            grdMainvw.Columns[1].Caption = "Roles";
            grdMainvw.Columns[1].Width = grdMain.Width - grdMainvw.Columns[0].Width;

        }
        private void SetFormColor() //Rup
        {
            DataSet dsColor = new DataSet();
            Color myColor = Color.Coral;
            string strSQL;
            string colorCode = string.Empty;
            strSQL = "select vcolor from Vudyog..co_mast where compid =" + this.pCompId;
            dsColor = oDataAccess.GetDataSet(strSQL, null, 20);
            if (dsColor != null)
            {
                if (dsColor.Tables.Count > 0)
                {
                    dsColor.Tables[0].TableName = "ColorInfo";
                    colorCode = dsColor.Tables["ColorInfo"].Rows[0]["vcolor"].ToString();
                }
            }

            if (!string.IsNullOrEmpty(colorCode))
            {
                this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                myColor = Color.FromArgb(Convert.ToInt32(colorCode.Trim()));
            }
            this.BackColor = Color.FromArgb(myColor.R, myColor.R, myColor.G, myColor.B);
            this.grdMain.BackColor = Color.FromArgb(myColor.R, myColor.R, myColor.G, myColor.B);
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            string vRoleList = string.Empty;
            this.pEditMode = true;
            if (this.pAddMode == false && this.pEditMode == false)
            {
                this.Close();
                return;
            }
            foreach (DataRow dr in dt.Rows )
            { 
                if((bool)dr["sel"]==true)
                {
                     vRoleList = vRoleList + "<<"+ ((string)dr["roles"]).Trim()+">>";
                }
            }
            uBaseForm.FrmBaseForm ofrm = new uBaseForm.FrmBaseForm();
            
            ofrm = this.pParentForm;
            Type tfrm = ofrm.GetType();
            this.pDataSet = (DataSet)tfrm.GetProperty("pDataSet").GetValue(ofrm, null);
            this.pDataSet.Tables[0].Rows[0]["UserRoles"] = vRoleList;
            tfrm.GetProperty("pDataSet").SetValue(this.pParentForm, this.pDataSet, null);
            this.Close();
            return;

        }
        public uBaseForm.FrmBaseForm pParentForm
        {
            get { return vParentForm; }
            set { vParentForm = value; }
        }
    }
}
