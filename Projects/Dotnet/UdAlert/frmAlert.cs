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
        List<int> idList; int CurrentAlerID;
        string alName, alDescription, alTableName, alLastUpdated, alIsActive;
        ArrayList arList = new ArrayList();
        DataAccess_Net.clsDataAccess oDataAccess;
        string vRole;//Rup
        int Count, LengthOfList;
        public frmAlert(string[] args)
        {
            
            InitializeComponent();
            this.pFrmCaption = "Alert Form";
            this.pCompId = Convert.ToInt16(args[0]);
            this.pComDbnm = args[1];
            this.pServerName = args[2];
            this.pUserId = args[3];
            this.pPassword = args[4];
            this.vRole = args[11];
        }

        private void frmAlert_Load(object sender, EventArgs e)
        {
            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();
            getAlertId();
            //DataSet dsMain = oDataAccess.GetDataSet("select top 1 Alert_Name,Alert_Description,Last_Updated,Table_Name,IsActive from Alert_Master where [IsActive]=1", null, 20);
            DataSet dsMain = oDataAccess.GetDataSet("execute usp_alert_List", null, 20);
            
            LoadData(dsMain);

            Count++;
        }
       // public virtual bool ColumnAutoWidth { get; set; }
       // public UnboundColumnType unboundtype { get; set; }

        public void LoadData(DataSet dsgetData)
        {
            foreach (DataRow dr in dsgetData.Tables[0].Rows)
            {
                alName = dr["Alert_Name"].ToString();
                alDescription = dr["Alert_Description"].ToString();
                alLastUpdated = dr["Last_Updated"].ToString();
                alTableName = dr["Table_Name"].ToString();
                //alIsActive = dr["IsActive"].ToString();

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
            if (dsDataT.Tables[0].Rows.Count == 0)
            {
                
                MessageBox.Show("There is no records for this alert");
                return;
            }
            
            for (int i = 0; i < dsDataT.Tables[0].Columns.Count; i++)
            {
                if (dsDataT.Tables[0].Columns[i].DataType.ToString() == "System.DateTime")
                {
                    this.dataGridView1.Columns[i].DefaultCellStyle.Format = "dd/MM/yyyy hh:mm:ss";

                }
            }
           
            
            #region binding tables to Grid Control

            //if (dsDataT.Tables[0].Columns.Count > 6)
            //{
            
            //}
            //else
            //{
            //    gridView1.OptionsView.ColumnAutoWidth = true;
            //    gridView1.OptionsView.RowAutoHeight = true;
            //}

            //dataGridView1.DataSource = dsDataT.Tables[0];
            //dataGridView1.Columns[0].Width = 150;
            //dataGridView1.Columns[1].Width = 75;
            //dataGridView1.Columns[2].Width = 50;
            //dataGridView1.Columns[2].Width = 50;

            #endregion
            
        }
      public List<int> getAlertId() // AMAR 
        //public void getAlertId()
        {

            DataSet dsData = oDataAccess.GetDataSet("select id from Alert_Master", null, 20);
            //List<int> idList = new List<int>(dsData.Tables[0].Rows.Count);  //Amar
            idList = new List<int>(dsData.Tables[0].Rows.Count);  //Amar
            foreach (DataRow dr in dsData.Tables[0].Rows)
            {
                int i = 0;
                idList.Add(Convert.ToInt32(dr[i]));
                i++;

            }
            return idList;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
           List<int> alIdList = getAlertId(); 
            this.btnFirst.Enabled = true;
           LengthOfList = alIdList.Count;
            //MessageBox.Show("This is ArrayList"+alIdList[0].ToString ());
            int alId = Convert.ToInt32(alIdList[0]);
            if (Count < LengthOfList && Count >= 0)
            {
                
                this.btnBack.Enabled = true;

                string sqlQuery = "select top 1 Alert_Name,Alert_Description,Last_Updated,Table_Name from Alert_Master where [IsActive]=1 and id=" + alIdList[Count].ToString().ToString();
                DataSet dsalData = oDataAccess.GetDataSet(sqlQuery, null, 20);
                LoadData(dsalData);
                //LoadData(getnextalert(CurrentAlerID,1));
                Count++;
            }
            else
            {
                this.btnNext.Enabled = false;
                Count--;
            }

        }
        //public DataSet getnextalert(int AlertID,int Direction)
        //{
        //    string sqlQuery = "select top 1 Alert_Name,Alert_Description,Last_Updated,Table_Name from Alert_Master where [IsActive]=1 and id=" + AlertID.ToString();
        //    DataSet dsalData = oDataAccess.GetDataSet(sqlQuery, null, 20);
        //    switch (Direction)
        //    {
        //        case 1:
                    
        //                if (dsalData.Tables[0].Rows.Count > 0)
        //                 return dsalData; 
        //                else
        //                    getnextalert(AlertID + 1, 1);
        //                break;
                    
        //        case 2:
        //            if (dsalData.Tables[0].Rows.Count > 0)
        //                return dsalData;
        //            else
        //                getnextalert(AlertID - 1, 2);
        //            break;
        //    }

        //    return null;

        //}

        private void btnBack_Click(object sender, EventArgs e)
        {

            --Count;
            List<int> alIdList = getAlertId();
            LengthOfList = alIdList.Count;
            if (Count >= 0)
            {
                this.btnNext.Enabled = true;
                string sqlQuery = "select top 1 Alert_Name,Alert_Description,Last_Updated,Table_Name from Alert_Master where [IsActive]=1 and  id=" + alIdList[Count].ToString() + "order by id desc";
                DataSet dsalData = oDataAccess.GetDataSet(sqlQuery, null, 20);
                LoadData(dsalData);
            }
            else
            {
                this.btnBack.Enabled = false;
                Count = Count + 2;
            }



        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            this.btnLast.Enabled = true;
            DataSet dsMain = oDataAccess.GetDataSet("select top 1 Alert_Name,Alert_Description,Last_Updated,Table_Name from Alert_Master where [IsActive]=1", null, 20);
            LoadData(dsMain);
            Count = 1;
            this.btnNext.Enabled = true;
            this.btnFirst.Enabled = false;

        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            this.btnBack.Enabled = true;
            this.btnFirst.Enabled = true;
            DataSet dsMain = oDataAccess.GetDataSet("select top 1 Alert_Name,Alert_Description,Last_Updated,Table_Name from Alert_Master where [IsActive]=1 order by id desc", null, 20);
            LoadData(dsMain);
            List<int> alIdList = getAlertId();
            Count = alIdList.Count - 1;
            this.btnLast.Enabled = false;

        }

     

        private void frmAlert_FormClosing(object sender, FormClosingEventArgs e)
        {
            string sqlstr;
            string alertnmae = this.alName;
            if (this.chkAlert1.Checked == true)
            {
                sqlstr = "update ALERT_MASTER set UserRoles=replace(UserRoles,'<<" + this.vRole.Trim() + ">>','') where charindex('<<" + this.vRole.Trim() + ">>',userroles)>0";
                oDataAccess.ExecuteSQLStatement(sqlstr, null, 20, true);
                //sqlstr = "update alert_master set [IsActive]=0 where alert_name='" + this.alName+"'";
                //oDataAccess.ExecuteSQLStatement(sqlstr, null, 20, true);

            }

        }





    }

}

