using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Windows.Forms;
using CrystalDecisions.ReportSource;
using CRAXDRT;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace ueST3DataTool
{
    public partial class FrmCrviewer : Form
    {
        string vReportPath = " ";
        DateTime vsdate, vedate;
        DataView vdvwCompany;
        DataView vdvwCoAdditional;
        cTipsSqlConn.SlqDatacon oSqlConn = new cTipsSqlConn.SlqDatacon();
        DataSet dsComman = new DataSet();
        DataView vResultSet;
        DataSet DsStDataCon = new DataSet();
        ADODB.Recordset ar = new ADODB.Recordset();
        DataSet dsTitles = new DataSet("Authors");
        ADODB.Recordset rs = new ADODB.Recordset();

        public FrmCrviewer(DataView dvwCompany, DataView dvwCoAdditional,DataView ResultSet, DateTime vvsdate, DateTime vvedate)
        {
            
            this.vsdate = vvsdate;
            this.vedate = vvedate;
            this.vdvwCompany = dvwCompany;
            this.vdvwCoAdditional = dvwCoAdditional;
            this.vResultSet = ResultSet;
            InitializeComponent();
            this.mDataset1();

            //vDt = dt;
        }
        private void mDataset1()
        {
            ADODB.Connection conn=new ADODB.Connection();
            //conn.ConnectionString = "Driver={SQL Server};server=udyog11;database=t021011;uid=sa;pwd=sa@1985;";
            String sConnectionString = "Driver={SQL Server};server=udyog11;database=t021011;uid=sa;pwd=sa@1985;";
            conn.Open(sConnectionString, "sa", "sa@1985", 0);
            
            string selectstring = "select serty from bpmain";
            ar.Open(selectstring, conn, ADODB.CursorTypeEnum.adOpenUnspecified , ADODB.LockTypeEnum.adLockUnspecified,0);
           
            //ar.Open(selectstring, conn,ADODB.CursorTypeEnum.adOpenStatic ,ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdUnknown);


        }
        private void mDataset3()
        { 
            string DBConnection = "Provider=SQLOLEDB.1;uid=sa;password=sa@1985;database=t021011;DataSource=udyog11";
            string SQL = "select serty from epacdet";

            ADODB.Connection Conn=new ADODB.Connection();
            OleDbDataAdapter daTitles=new OleDbDataAdapter();
            Conn.Open(DBConnection,"","",-1); 
            daTitles.Fill(dsTitles,rs,"Tiltes"); 
            Conn.Close(); 

        }
        private void mDataset()
        {
            
            string tblnm = "tbl1";
            SqlConnection conn;
            string vcomconstring = "Driver={SQL Server};server=udyog11;uid=sa;pwd=sa@1985;database=t021011;Connect Timeout=300;Integrated Security=false";
            conn=new SqlConnection(vcomconstring);
            string selectstring = "select serty from bpmain";
            int vRowCount = 0;
            try
            {
               if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                SqlDataAdapter da = new SqlDataAdapter(selectstring, conn);
                da.Fill(DsStDataCon, tblnm);
                

            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw e;
            }
            finally
            {
                if ((conn.State == ConnectionState.Open) )
                {
                    //conn.Close();
                }
            }

        }
        private void FrmCrviewer_Load(object sender, EventArgs e)
        {

            vReportPath = @"D:\ST3.rpt";
            ReportDocument cr = new ReportDocument();

            //ReportObject CrApp=new ReportObject();
            //CrApp = "CrystalRuntime.Application.10";
            
            String FName;
            FName = @"D:\x_1.xml";
            CRAXDRT.Application  crApp = new CRAXDRT.Application ();
            CRAXDRT.Report crReport = new CRAXDRT.Report();
            crReport = crApp.OpenReport(vReportPath, 0);
            
            
            
            CRAXDRT.ExportOptions oExport ;
            oExport = crReport.ExportOptions;
            oExport.DestinationType = CRAXDRT.CRExportDestinationType.crEDTDiskFile;
            oExport.FormatType = CRAXDRT.CRExportFormatType.crEFTXML;
            oExport.XMLFileName = FName;
            this.mDataset3();

            //DataTable _tmpvar--->
            DataTable _tmpvar = new DataTable();
            _tmpvar.TableName = "_tmpvar";

            DataColumn sdate = new DataColumn("sdate", typeof(System.DateTime));
            _tmpvar.Columns.Add(sdate);
            DataColumn edate = new DataColumn("edate", typeof(System.DateTime));
            _tmpvar.Columns.Add(edate);

            DataRow drow = _tmpvar.NewRow();
            drow[sdate] = vsdate;
            drow[edate] = vedate;
            _tmpvar.Rows.Add(drow);
            MessageBox.Show(((DateTime)_tmpvar.Rows[0][sdate]).Month.ToString());
            if (dsComman.Tables["_tmpvar"] != null)
            {
                dsComman.Tables.Remove("_tmpvar");
            }
            dsComman.Tables.Add(_tmpvar);

            MessageBox.Show(((DateTime)dsComman.Tables["_tmpvar"].Rows[0][sdate]).Month.ToString());
            //<---DataTable _tmpvar
           
            
            crReport.EnableParameterPrompting = false;
            
            CRAXDRT.ParameterValues currValue = new CRAXDRT.ParameterValues();
            CRAXDRT.ParameterFieldDefinition ParaDef;
            
            for (int i = 1; i <= crReport.ParameterFields.Count ; i++)
            {
                string pstr = crReport.ParameterFields[i].Name;
                CRAXDRT.ParameterValue pavalue;
                
                pstr = pstr.Replace("?", "").Replace("{","").Replace("}","").Trim();

                string tblnm = pstr.Substring(0, pstr.IndexOf("."));
                string fldnm = pstr.Substring(pstr.IndexOf(".") + 1);
                if (tblnm == "_tmpvar")
                {
                    //ParaDef = crReport.ParameterFields[i];
                    //ParaDef.AddCurrentValue(this.dsComman.Tables[tblnm].Rows[0][fldnm]);
                    crReport.ParameterFields[i].AddCurrentValue(this.dsComman.Tables[tblnm].Rows[0][fldnm]);
                }
                if (tblnm.ToUpper() == "COMPANY")
                {
                    //ParaDef = crReport.ParameterFields[i];
                    //ParaDef.AddCurrentValue(this.vdvwCompany.Table.Rows[0][fldnm]);
                    crReport.ParameterFields[i].AddCurrentValue(this.vdvwCompany.Table.Rows[0][fldnm]);
                }
                if (tblnm.ToUpper() == "COADDITIONAL")
                {
                    
                    //ParaDef = crReport.ParameterFields[i];
                    //ParaDef.AddCurrentValue(this.vdvwCoAdditional.Table.Rows[0][fldnm]);
                    crReport.ParameterFields[i].ClearCurrentValueAndRange();
                    crReport.ParameterFields[i].AddDefaultValue(this.vdvwCoAdditional.Table.Rows[0][fldnm]);
                    crReport.ParameterFields[i].SetDefaultValue(this.vdvwCoAdditional.Table.Rows[0][fldnm], crReport.ParameterFields[i].ValueType);
                    crReport.ParameterFields[i].AddCurrentValue(this.vdvwCoAdditional.Table.Rows[0][fldnm]);
                    crReport.ParameterFields[i].SetCurrentValue(this.vdvwCoAdditional.Table.Rows[0][fldnm], crReport.ParameterFields[i].ValueType);
                    crReport.ParameterFields[i].DisallowEditing = true;
                   
                }
                
                //crReport.ParameterFields[i].AddCurrentValue(currValue);
            }
            crReport.DiscardSavedData();

            
            crReport.Database.SetDataSource(rs, 3, 1);

            CRAXDRT.Database crxDatabase;
            CRAXDRT.DatabaseTables crxDatabaseTables;

            crxDatabase = crReport.Database;
            crxDatabaseTables = crxDatabase.Tables;
            foreach (CRAXDRT.DatabaseTable crxDatabaseTable in crxDatabaseTables)
            {
                crxDatabaseTable.SetLogOnInfo("(local)", "t01011", "sa", "sa1985");
            }

            crReport.Export(false);


            
            
           
            
         
            
            
            


           
            
           // CrystalDecisions.Shared.ParameterFields parafld1 = new CrystalDecisions.Shared.ParameterFields();
           // CrystalDecisions.Shared.ParameterField parafld = new CrystalDecisions.Shared.ParameterField();
           // CrystalDecisions.Shared.ParameterValues currValue = new CrystalDecisions.Shared.ParameterValues();
            
            
           // for (int i = 0; i <= cr.ParameterFields.Count - 1; i++)
           // {
           //     string pstr = cr.ParameterFields[i].Name;
           //     string tblnm = pstr.Substring(0, pstr.IndexOf("."));
           //     string fldnm = pstr.Substring(pstr.IndexOf(".") + 1);
           //     if (tblnm == "_tmpvar")
           //     {
           //         currValue.AddValue(this.dsComman.Tables[tblnm].Rows[0][fldnm]);
           //     }
           //     if (tblnm.ToUpper() == "COMPANY")
           //     {
           //         currValue.AddValue(this.vdvwCompany.Table.Rows[0][fldnm]);
           //     }
           //     if (tblnm.ToUpper() == "COADDITIONAL")
           //     {
           //         currValue.AddValue(this.vdvwCoAdditional.Table.Rows[0][fldnm]);//dvwCoAdditional
           //     }
           //     cr.DataDefinition.ParameterFields[i].ApplyCurrentValues(currValue);
           // }//for (int i = 0; i<=cr.ParameterFields.Count - 1; i++)
           // //ExportOptions crExportOption = new ExportOptions();
           // DiskFileDestinationOptions diskFileDestin = new DiskFileDestinationOptions();
            
           ////CrRep.ParameterFields.GetItemByName("City").AddCurrentValue "London"
           ////CrRep.ParameterFields.GetItemByName("City").AddCurrentValue "Sydney"
           ////CrRep.ParameterFields.GetItemByName("City").AddCurrentValue "Hong Kong"


            //diskFileDestin.DiskFileName = FName;
            //crExportOption = cr.ExportOptions;
            //crExportOption.ExportDestinationOptions = diskFileDestin;
            //crExportOption.ExportDestinationType = ExportDestinationType.DiskFile;
            ////crExportOption.ExportFormatType=ExportFormatType.PortableDocFormat;
            //crExportOption.UExportFormatType = 31;
            //crExportOption.UExportDestinationType = 1;
            //cr.Export();
            

            //crExportOption = cr.ExportOptions;
            //crExportOption.UExportDestinationType = 1;
            //crExportOption.UExportFormatType = 31;
            
           
            //diskFileDestin.DiskFileName = FName;
            //crExportOption.ExportDestinationOptions = diskFileDestin;
            //cr.Export(crExportOption);
            
            //this.crviewer.ShowRefreshButton = false;
            //this.crviewer.ReportSource = cr;
            //this.crviewer.Refresh();
            

        }
    }
}