using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataAccess_Net;
using uBaseForm;
using System.Collections;
using System.Data.SqlClient;

namespace UdAlert
{
    public partial class frmAlert : uBaseForm.FrmBaseForm
    {
       
        string alName, alDescription, alTableName, alLastUpdated;
        ArrayList arList = new ArrayList();
        DataAccess_Net.clsDataAccess oDataAccess;
        string vRole;//Rup
        int rCount,rtCount;
        DataSet dsAlert = new DataSet();
        string sqlstr=string.Empty;
        private string vMainField = string.Empty, vMainFldVal = string.Empty, VmainFieldCaption = string.Empty;
        public frmAlert(string[] args)
        {
            
            InitializeComponent();
            this.pFrmCaption = "Alert Form";
            this.pCompId = Convert.ToInt16(args[0]);
            this.pComDbnm = args[1];
            this.pServerName = args[2];
            this.pUserId = args[3];
            this.pPassword = args[4];
            this.pAppUerName = args[5];
            this.vRole = args[11];
        }

        private void frmAlert_Load(object sender, EventArgs e)
        {
            this.vMainField = "Alert_Name";


            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();
            sqlstr = "usp_alert_List '"+this.pAppUerName+"'";
            dsAlert = oDataAccess.GetDataSet(sqlstr, null, 20);

            if(dsAlert.Tables[0].Rows.Count==0)
            {
               // MessageBox.Show("There is no records for alerts");
                this.Close();
                return;

            }
            vMainFldVal = dsAlert.Tables[0].Rows[0]["Alert_Name"].ToString();
            rCount = 0;
            rtCount = dsAlert.Tables[0].Rows.Count - 1;
            this.btnFirst_Click(sender, e);
            
        }
    

        public void LoadData(DataSet dsgetData)
        {

            foreach (DataRow dr in dsgetData.Tables[0].Rows)
            {
                alName = dr["Alert_Name"].ToString();
                alDescription = dr["Alert_Description"].ToString();
                alLastUpdated = dr["Last_Updated"].ToString();
                alTableName = dr["Table_Name"].ToString();
                

            }


            txtAlertName.Text = alName;
            txtAlertDesc.Text = alDescription;

            DateTime dt = Convert.ToDateTime(alLastUpdated);
            txtLastUpdated.Text = dt.Day.ToString() + "/" + dt.Month.ToString() + "/" + dt.Year.ToString() + " " +
                dt.ToString("hh") + ":" + dt.ToString("mm") + ":" + dt.ToString("ss tt");
                
           
       

            string sqlQuery = String.Empty;
            sqlQuery = "select * from " + alTableName;
            DataSet dsDataT = oDataAccess.GetDataSet(sqlQuery, null, 20);
            dataGridView1.DataSource = dsDataT.Tables[0];
                 
            for (int i = 0; i < dsDataT.Tables[0].Columns.Count; i++)
            {
                if (dsDataT.Tables[0].Columns[i].DataType.ToString() == "System.DateTime")
                {
                    this.dataGridView1.Columns[i].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";

                }
            }
           
            

            
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            rCount = rCount + 1;
            vMainFldVal = dsAlert.Tables[0].Rows[rCount]["Alert_Name"].ToString();
            DataSet dsMain = oDataAccess.GetDataSet("select top 1 Alert_Name,Alert_Description,Last_Updated,Table_Name from Alert_Master where [IsActive]=1 and Alert_Name='" + vMainFldVal.Trim() + "'", null, 20);
            LoadData(dsMain);

            this.mthBtnEnabled();
            
        }
        private void mthBtnEnabled()
        {
            this.btnFirst.Enabled = false;
            this.btnBack.Enabled = false;
            this.btnNext.Enabled = false;
            this.btnLast.Enabled = false;
            if (rCount != rtCount)
            {
                this.btnNext.Enabled = true;
                this.btnLast.Enabled = true;
            }
            if (rCount !=0)
            {
                this.btnFirst.Enabled = true ;
                this.btnBack.Enabled = true ;
            }

        }
    
       

        private void btnFirst_Click(object sender, EventArgs e)
        {
            rCount = 0;
            vMainFldVal = dsAlert.Tables[0].Rows[rCount]["Alert_Name"].ToString();
            
            string sqlstr = "select * from Alert_Master where Alert_Name='"+vMainFldVal.Trim()+"'";
            DataSet dsMain = oDataAccess.GetDataSet("select top 1 Alert_Name,Alert_Description,Last_Updated,Table_Name from Alert_Master where [IsActive]=1 and Alert_Name='"+vMainFldVal.Trim()+"'", null, 20);
            LoadData(dsMain);
            this.mthBtnEnabled();
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            rCount = rCount-1;
            vMainFldVal = dsAlert.Tables[0].Rows[rCount]["Alert_Name"].ToString();

            string sqlstr = "select * from Alert_Master where Alert_Name='" + vMainFldVal.Trim() + "'";
            DataSet dsMain = oDataAccess.GetDataSet("select top 1 Alert_Name,Alert_Description,Last_Updated,Table_Name from Alert_Master where [IsActive]=1 and Alert_Name='" + vMainFldVal.Trim() + "'", null, 20);
            LoadData(dsMain);
            this.mthBtnEnabled();
        }
        private void btnLast_Click(object sender, EventArgs e)
        {
            rCount = rtCount;
            vMainFldVal = dsAlert.Tables[0].Rows[rCount]["Alert_Name"].ToString();

            string sqlstr = "select * from Alert_Master where Alert_Name='" + vMainFldVal.Trim() + "'";
            DataSet dsMain = oDataAccess.GetDataSet("select top 1 Alert_Name,Alert_Description,Last_Updated,Table_Name from Alert_Master where [IsActive]=1 and Alert_Name='" + vMainFldVal.Trim() + "'", null, 20);
            LoadData(dsMain);
            this.mthBtnEnabled();
        }

     

        private void frmAlert_FormClosing(object sender, FormClosingEventArgs e)
        {
            string sqlstr;
            string alertnmae = this.alName;
            if (this.chkAlert1.Checked == true)
            {
                sqlstr = "update ALERT_MASTER set UserRoles=replace(UserRoles,'<<" + this.vRole.Trim() + ">>','') where charindex('<<" + this.vRole.Trim() + ">>',userroles)>0";
                oDataAccess.ExecuteSQLStatement(sqlstr, null, 20, true);

            }

        }





    }

}

