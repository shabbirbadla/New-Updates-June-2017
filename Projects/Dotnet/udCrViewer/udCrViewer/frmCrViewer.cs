using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using uBaseForm;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Windows.Forms;
using CrystalDecisions.ReportSource;
using System.Data.SqlClient;

using udDataTableQuery;
namespace udCrViewer
{
    public partial class frmCrViewer :   uBaseForm.FrmBaseForm
    {
        //string vReportPath = "", vSqlStmt = "", vPageBreakFieldList="";
        Int16 vPrintOption = 1;
        //DataTable vResulSet;
        //Boolean vWaitPrint;
        //string [] vPageBreakField;
        ReportDocument vReportDoc;

        public frmCrViewer()
        {
            this.pDisableCloseBtn = false;
            InitializeComponent();

        }

        private void crystalReportViewer_Load(object sender, EventArgs e)
        {
             //this.mthViewReport(this.pResulSet);
             this.mthViewReport();
        }
       //private void mthViewReport(DataTable tResultSet)
       private void mthViewReport()
       {

            //ReportDocument cr = new ReportDocument();
            ////DataSet dsCrview = new DataSet();
            //cr.Load(vReportPath);

            ////--->Fill DataSet
            
            try
            {
            //    cr.SetDataSource(tResultSet);
            //    CrystalDecisions.Shared.ParameterFields parafld1 = new CrystalDecisions.Shared.ParameterFields();
            //    CrystalDecisions.Shared.ParameterField parafld = new CrystalDecisions.Shared.ParameterField();
            //    CrystalDecisions.Shared.ParameterValues currValue = new CrystalDecisions.Shared.ParameterValues();

            //    CrystalDecisions.Shared.ParameterDiscreteValue paramrange = new CrystalDecisions.Shared.ParameterDiscreteValue();
            //    string tblnm="", fldnm="";
                
            //    for (int i = 0; i <= cr.ParameterFields.Count - 1; i++)
            //    {
            //        string pstr = cr.ParameterFields[i].Name;
            //        try
            //        {
            //           if (pstr.IndexOf(".") > -1)
            //            {
            //                tblnm = pstr.Substring(0, pstr.IndexOf("."));
            //                fldnm = pstr.Substring(pstr.IndexOf(".") + 1);
            //                currValue.AddValue(this.pdsCommon.Tables[tblnm].Rows[0][fldnm]);
            //                cr.DataDefinition.ParameterFields[i].ApplyCurrentValues(currValue);
            //            }
            //            else if (pstr=="musername")
            //            {
            //                currValue.AddValue(this.pAppUerName);
            //                cr.DataDefinition.ParameterFields[i].ApplyCurrentValues(currValue);
            //            }
                        
            //        }
            //        catch (Exception ex)
            //        {
            //            //MessageBox.Show(fldnm + " Field Not Found in " + tblnm);
            //        }
            //    }//for (int i = 0; i<=cr.ParameterFields.Count - 1; i++)

                CrViewer.EnableDrillDown = false;
                CrViewer.ShowRefreshButton = false;
                CrViewer.ShowGroupTreeButton = false;
                this.CrViewer.ReportSource =this.pReportDoc;
                //this.CrViewer.ReportSource = cr;
                this.CrViewer.Refresh();

                if (this.pPrintOption == 3)
                {
                    this.CrViewer.PrintReport();
                }
                else if (this.pPrintOption == 2)
                {
                    CrViewer.ShowExportButton = false;
                    CrViewer.ShowPrintButton = false;
                }
                //else if (this.pPrintOption == 4)
                //{

                    
                //    this.CrViewer.ExportReport();
                //}
                
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            //<---Fill DataSet

        }
        //public string pReportPath
        //{

        //    get
        //    {
        //        return vReportPath;
        //    }
        //    set
        //    {
        //        vReportPath = value;
        //    }
        //}
        //public string pSqlStmt
        //{

        //    get
        //    {
        //        return vSqlStmt;
        //    }
        //    set
        //    {
        //        vSqlStmt = value;
        //    }
        //}

     
        //public DataTable  pResulSet
        //{

        //    get
        //    {
        //        return vResulSet;
        //    }
        //    set
        //    {
        //        vResulSet = value;
        //    }
        //}
       public Int16 pPrintOption
       {

           get
           {
               return vPrintOption;
           }
           set
           {
               vPrintOption = value;
           }
       }
        public ReportDocument pReportDoc
        {
            get { return vReportDoc; }
            set { vReportDoc = value; }
        }
        //public Boolean pWaitPrint
        //{
        //    get { return vWaitPrint; }
        //    set { vWaitPrint = value; }
        //}
        //public  string pPageBreakFieldList
        //{
        //    get { return vPageBreakFieldList; }
        //    set { vPageBreakFieldList = value; }
        //}
    }
}
