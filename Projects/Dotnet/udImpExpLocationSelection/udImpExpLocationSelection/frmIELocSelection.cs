using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace udImpExpLocationSelection
{
    public partial class frmIELocSelection : uBaseForm.FrmBaseForm
    {
        DataAccess_Net.clsDataAccess oDataAccess;
        uBaseForm.FrmBaseForm vParentForm;
        DataTable dt;
        string vImpExp;
        public frmIELocSelection()
        {
            InitializeComponent();
        }

        private void frmIELocSelection_Load(object sender, EventArgs e)
        {
            
            string[] args = { };
            if (this.pPara != null)
            {
                args = this.pPara;
            }
            if (args.Length < 1)
            {
                //args = new string[] { "19", "A021112", "udyog65", "sa", "sa@1985", "13032", "IEM", "ADMIN", @"D:\USquare\Bmp\Icon_usquare.ico", "Usquare pack", "Usquare.exe", "1", "udpid6096DTM20110307112715" };
                //args = new string[] { "11", "D011314", "desktop7\\desktop7", "sa", "sa@1985", "^14017", "ADMIN", @"D:\vudyogsdk\Bmp\Icon_VudyogSDK.ico", "VudyogSdk", "VudyogSdk.EXE", "4764", "udPID4764DTM20111213125821" };
                args = new string[] { "02", "X011516", "PRO_PANKAJ\\SQLEXPRESS", "sa", "sa1985", "^13074", "ADMIN", @"F:\Installer12.0\Bmp\icon_VudyogSDK.ico", "VudyogSDK", "VudyogSDK.exe", "4360", "udPID4360DTM20150921100032" };
            }
          
            this.pFrmCaption = "Location Details";
            this.pCompId = Convert.ToInt16(args[0]);
            this.pComDbnm = args[1];
            this.pServerName = args[2];
            this.pUserId = args[3];
            this.pPassword = args[4];
            //Icon MainIcon = new System.Drawing.Icon(args[8].Replace("<*#*>", " "));
            //this.pFrmIcon = MainIcon;

            this.pPApplText = args[9].Replace("<*#*>", " ");
            this.pPApplName = args[10];
            this.pPApplPID = Convert.ToInt16(args[11]);
            this.pPApplCode = args[12];

            //this.pPApplText = args[8].Replace("<*#*>", " ");
            //this.pPApplName = args[9];
            //this.pPApplPID = Convert.ToInt16(args[10]);
            //this.pPApplCode = args[11];



            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();
            this.mthGridFill();
            this.SetFormColor(); 
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            

            string vExpLocDet = string.Empty;
            this.pEditMode = true;
            if (this.pAddMode == false && this.pEditMode == false)
            {
                    this.Close();
                    return;
            }
            foreach (DataRow dr in dt.Rows)
            {
                if ((bool)dr["sel"] == true)
                {
                    vExpLocDet = vExpLocDet + "<<" + ((string)dr["Loc_Code"]).Trim() + ">>";
                }
            }
            uBaseForm.FrmBaseForm ofrm = new uBaseForm.FrmBaseForm();
            ofrm = this.pParentForm;
            Type tfrm = ofrm.GetType();
            this.pDataSet = (DataSet)tfrm.GetProperty("pDataSet").GetValue(ofrm, null);
            this.pImpExp = (string)tfrm.GetProperty("PMasterCode").GetValue(ofrm, null); //Birendra : Bug-22262 on 10/04/2014
            if (this.pImpExp.ToLower() == "imp")
            {
                this.pDataSet.Tables[0].Rows[0]["ImpLocDet"] = vExpLocDet;
            }
            else
            {
                this.pDataSet.Tables[0].Rows[0]["ExpLocDet"] = vExpLocDet;
            }
            
            tfrm.GetProperty("pDataSet").SetValue(this.pParentForm, this.pDataSet, null);
            this.Close();
            return;
        }

        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void mthGridFill()
        {
            string vExpLocDet = string.Empty, vCode = string.Empty, vTranType = string.Empty;
            uBaseForm.FrmBaseForm ofrm = new uBaseForm.FrmBaseForm();
            if (this.pParentForm != null)
            {
                ofrm = this.pParentForm;
                DataSet vds = new DataSet();
                vds = (DataSet)ofrm.GetType().GetProperty("pDataSet").GetValue(ofrm, null);
                this.pImpExp = (string)ofrm.GetType().GetProperty("PMasterCode").GetValue(ofrm, null); //Birendra : Bug-22262 on 10/04/2014
                
//                if (this.pImpExp == "imp")
                if (this.pImpExp.ToLower() == "imp")  //Birendra :  Bug-22262 on 22/04/2014
                {
                    if ((vds.Tables[0].Rows[0]["ImpLocDet"]) != null)
                    {
                        vExpLocDet = vds.Tables[0].Rows[0]["ImpLocDet"].ToString();
                    }
                }
                else
                {
                    if ((vds.Tables[0].Rows[0]["ExpLocDet"]) != null)
                    {
                        vExpLocDet = vds.Tables[0].Rows[0]["ExpLocDet"].ToString();
                    }
                }
            }

            dt = new DataTable();
            DataColumn dcol1 = new DataColumn();
            DataColumn dcol2 = new DataColumn();
            DataColumn dcol3 = new DataColumn();
            //DataColumn dcol4 = new DataColumn();
            dcol1.ColumnName = "sel";
            dcol1.DataType = System.Type.GetType("System.Boolean");
            dt.Columns.Add(dcol1);

            dcol2.ColumnName = "loc_desc";
            dcol2.DataType = System.Type.GetType("System.String");
            dt.Columns.Add(dcol2);

            dcol3.ColumnName = "loc_code";
            dcol3.DataType = System.Type.GetType("System.String");
            dt.Columns.Add(dcol3);

            //dcol4.ColumnName = "TranType";
            //dcol4.DataType = System.Type.GetType("System.String");
            //dt.Columns.Add(dcol4);

            DataSet dsLocation = new DataSet();
            string strSQL = string.Empty;
            
            //Added By Kishor A. for Bug-27080 on 09/10/2015 Start..
            DataSet dsLoc = new DataSet();
            string strSQLloc = "Select IE_LOCCODE From vudyog..co_mast where COMPID= "+this.pCompId;
            dsLoc = oDataAccess.GetDataSet(strSQLloc, null, 20);
            
            if (this.pParentForm.Tag.ToString()=="Tag1")
            {
                strSQL = "select *  from loc_master where Loc_Code <>"+"'"+dsLoc.Tables[0].Rows[0][0].ToString()+ "'";
            }
            else
            {
                strSQL = "select *  from loc_master";
            }
            //Added By Kishor A. for Bug-27080 on 09/10/2015 End..
            //string strSQL = "select *  from loc_master"; // Commented By Kishor A. for Bug-27080 on 09/10/2015

            dsLocation = oDataAccess.GetDataSet(strSQL, null, 20);
            int pos = 0;
            //vExpLocDet = "<<AV1>><<AV2>>";
            foreach (DataRow dr in dsLocation.Tables[0].Rows)
            {
                DataRow dr1 = dt.NewRow();
                dr1["sel"] = false;
                vCode = dr["loc_code"].ToString();
                
                if (!string.IsNullOrEmpty(vExpLocDet))
                {
                    if (vExpLocDet.IndexOf("<<" + vCode.Trim() + ">>") > -1)
                    {
                        dr1["sel"] = true;
                       // pos = vExpLocDet.IndexOf("<<" + vCode.Trim() + "##") + ("<<" + vCode.Trim() + "##").Length;
                    }
                }

                dr1["loc_desc"] = dr["loc_desc"].ToString();
                dr1["loc_code"] = vCode;
                dt.Rows.Add(dr1);

            }
            grdMain.AutoSize = false;
            //grdMain.AutoResizeRow = false;
            grdMain.RowHeadersVisible = false;
            grdMain.DataSource = dt;
            grdMain.Columns[0].HeaderText = "Select";
            grdMain.Columns[1].HeaderText = "Location Details";
            grdMain.Columns[2].HeaderText = "Location Code";
            //grdMain.Columns[3].HeaderText = "Transaction Type";
            if (this.pAddMode == false && this.pEditMode == false)
            {
                grdMain.Columns[0].ReadOnly = true;
                grdMain.Columns[1].ReadOnly = true;
                grdMain.Columns[2].ReadOnly = true;
                //grdMain.Columns[3].ReadOnly = true;
            }
            else
            {
                grdMain.Columns[0].ReadOnly = false;
                grdMain.Columns[1].ReadOnly = true;
                grdMain.Columns[2].ReadOnly = true;
                //grdMain.Columns[3].ReadOnly = false;
            }
            //grdMain.Columns[1].Width = grdMain.Width - grdMainvw.Columns[0].Width;

            //DataSet dsRoles = new DataSet();
            //DataSet dsCoMast = new DataSet();
            //string strSQL = "select * from vudyog..co_mast where compid=" + this.pCompId;
            //dsCoMast = oDataAccess.GetDataSet(strSQL, null, 20);
            //string vmRoles = string.Empty, vCompany = string.Empty, vRoles = string.Empty, vCompanyList = string.Empty, vCompName = string.Empty;
            //strSQL = "select user_roles,cast(company as varchar(8000)) as company from vudyog..userroles";
            //vCompName = dsCoMast.Tables[0].Rows[0]["co_name"].ToString().Trim() + "(" + ((DateTime)dsCoMast.Tables[0].Rows[0]["sta_dt"]).Year.ToString().Trim() + "-" + ((DateTime)dsCoMast.Tables[0].Rows[0]["end_dt"]).Year.ToString().Trim() + ")";
            //dsRoles = oDataAccess.GetDataSet(strSQL, null, 20);
            //foreach (DataRow dr in dsRoles.Tables[0].Rows)
            //{
            //    vmRoles = dr["user_roles"].ToString();
            //    vCompany = dr["company"].ToString();
            //    int len = vCompany.Length;
            //    int n;
            //    vRoles = oudclsUDF.padRStr(vmRoles.Trim(), vmRoles.Trim(), len);
            //    vCompanyList = "";
            //    for (int j = 0; j <= len - 1; j++)
            //    {
            //        n = oudclsUDF.fascii(vCompany.Substring(j, 1)) - oudclsUDF.fascii(vRoles.Substring(j, 1));
            //        if (n > 0)
            //        {
            //            vCompanyList = vCompanyList + Convert.ToChar(n).ToString();
            //        }

            //    }
            //    if (vCompanyList.IndexOf(vCompName) > -1)
            //    {
            //        DataRow dr1 = dt.NewRow();
            //        dr1["sel"] = false;
            //        if (!string.IsNullOrEmpty(vvRoles))
            //        {
            //            if (vvRoles.IndexOf("<<" + vmRoles.Trim() + ">>") > -1)
            //            {
            //                dr1["sel"] = true;
            //            }
            //        }
            //        dr1["roles"] = vmRoles.Trim();
            //        dt.Rows.Add(dr1);

            //    }
            //}


            //grdMainvw.OptionsView.ColumnAutoWidth = false;
            //grdMainvw.OptionsView.RowAutoHeight = false;
            //grdMainvw.OptionsView.ShowIndicator = false;

            //grdMain.DataSource = dt;
            //grdMainvw.Columns[0].Caption = "Select";
            //grdMainvw.Columns[1].Caption = "Roles";
            //grdMainvw.Columns[1].Width = grdMain.Width - grdMainvw.Columns[0].Width;

        }
        public uBaseForm.FrmBaseForm pParentForm
        {
            get { return vParentForm; }
            set { vParentForm = value; }
        }
        public string pImpExp
        {
            get { return vImpExp; }
            set { vImpExp = value; }
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
            //this.grdMain.BackColor = Color.FromArgb(myColor.R, myColor.R, myColor.G, myColor.B);

        }
    }
}
