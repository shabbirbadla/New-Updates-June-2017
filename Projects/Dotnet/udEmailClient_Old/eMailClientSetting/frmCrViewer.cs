using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;


namespace eMailClient
{
    public partial class frmCrViewer : Form
    {
        ReportDocument RepHandle = new ReportDocument();
        private string reportPath;

        public string ReportPath
        {
            get { return reportPath; }
            set { reportPath = value; }
        }

        private DataSet reportDataSet;
        public DataSet ReportDataSet
        {
            get { return reportDataSet; }
            set { reportDataSet = value; }
        }

        private bool isSubReport;

        public bool IsSubReport
        {
            get { return isSubReport; }
            set { isSubReport = value; }
        }
        private string mainQueryString;

        public string MainQueryString
        {
            get { return mainQueryString; }
            set { mainQueryString = value; }
        }
        private string subQueryString;

        public string SubQueryString
        {
            get { return subQueryString; }
            set { subQueryString = value; }
        }

        public frmCrViewer()
        {
            InitializeComponent();
        }

         public void CallReport() 
        {
            //string cFileName = Server.MapPath(".") + "\\Reports\\Mumbai\\St_Billc.Rpt";
            //string cFileName = Server.MapPath(".") + reportPath.Trim();
            string reportPath = ReportPath.Trim(); 
            if (!File.Exists(reportPath.Trim()))
            {
                MessageBox.Show("Report file not found...");
                return;
            }

            DataSet MainReportCommandSet = new DataSet();
            DataSet SubReportCommandSet = new DataSet();
            DataSet MainDataSet = ReportDataSet; 
            if (MainDataSet == null)
                MainDataSet = new DataSet();

            
            RepHandle.Load(reportPath);

            string[] subSqlStr = null;
            string[] mainSqlStr = MainQueryString.Trim().Split(':');
             
            if (IsSubReport == true && RepHandle.Subreports.Count > 0) // Is Subreport is True
            {
                if (SubQueryString.Trim() != null && SubQueryString.Trim()  != "") 
                {
                    subSqlStr = SubQueryString.Trim().Split(':');  
                    SubReportCommandSet = DataAcess.ExecuteDataset(subSqlStr,connHandle);
                    DataAcess.Connclose(connHandle);
                }
            }

            if (SessionProxy.ReportDataSet == null)
            {
                getCoAdditional GetCoAdditional = new getCoAdditional();
                //MainDataSet = GetCompany.Company(MainDataSet, SessionProxy.ReqCode.Trim(), SessionProxy.FinYear.Trim());
                MainDataSet.Tables.Add(SessionProxy.Company.Copy()); 
                MainDataSet = GetCoAdditional.CoAdditional(MainDataSet);
            }
            else
            {
                if (MainDataSet.Tables.Contains("Company") == false)
                {
                    //getCompany GetCompany = new getCompany();
                    //MainDataSet = GetCompany.Company(MainDataSet, SessionProxy.ReqCode.Trim(), SessionProxy.FinYear.Trim());
                    MainDataSet.Tables.Add(SessionProxy.Company.Copy()); 
                }

                if (MainDataSet.Tables.Contains("Manufact") == false)
                {
                    getCoAdditional GetCoAdditional = new getCoAdditional();
                    MainDataSet = GetCoAdditional.CoAdditional(MainDataSet);
                }

            }

            DataTable command = new DataTable();
            int strCnt = 0;
            foreach (string sqlStr in mainSqlStr)
            {   
                if (!string.IsNullOrEmpty(sqlStr))
                {
                    strCnt += 1;
                    try
                    {
                        MainReportCommandSet = DataAcess.ExecuteDataset(MainReportCommandSet, sqlStr, "Command" + strCnt.ToString().Trim(),500,connHandle);
                        DataAcess.Connclose(connHandle); 
                    }
                    catch (Exception Ex)
                    {
                        DisplayMessage(Ex.Message.Trim());
                        return;
                    }
                }
            }
             
                          
            if (IsSubReport == true && RepHandle.IsSubreport == true)
            {
                if (subSqlStr == null)
                {
                    RepHandle.SetDataSource(MainReportCommandSet.Tables[0]);
                }
                else
                {
                    RepHandle.SetDataSource(MainReportCommandSet);
                }
            }
            else
            {
                for (int i = 0; i < RepHandle.Database.Tables.Count; i++)
                {
                    RepHandle.Database.Tables[i].SetDataSource(MainReportCommandSet.Tables[i]);   
                }
            }
            
            ParameterValues ParamValue;
            ParameterFieldDefinitions parameterFieldDefinitions = RepHandle.DataDefinition.ParameterFields;
            ParameterDiscreteValue disCreteValue = new ParameterDiscreteValue();

            string param = "";
            string dataTableName = "";
            string colName = "";
            for (int i = 0; i < parameterFieldDefinitions.Count; i++)
            {
                if (parameterFieldDefinitions[i].ParameterFieldUsage.ToString().ToUpper() == "NOTINUSE")
                    continue;

                param = parameterFieldDefinitions[i].ParameterFieldName;
                if (param.Trim().IndexOf('+') >= 0)
                {
                    string[] paramArr = param.Split('+');
                    string discValue = "";
                    string paramS = "";
                    foreach (string paramA in paramArr)
                    {
                        if (paramA.Trim().IndexOf('.') < 0)
                            continue;

                        paramS = "";

                        if (paramA.Trim().ToUpper().IndexOf("ALLT(") >= 0)
                            paramS = paramA.Trim().Substring(5, (paramA.Trim().Length - 5) - 1);

                        if (paramA.Trim().ToUpper().IndexOf("ALLTRIM(") >= 0)
                            paramS = paramA.Trim().Substring(8, (paramA.Trim().Length - 8) - 1);
                        
                        if (paramS.Trim() != "")
                            dataTableName = paramS.Trim().Substring(0, paramS.Trim().IndexOf('.'));
                        else
                        {
                            int paramAL = (paramA.Trim().IndexOf('.') == -1 || paramA.Trim().Length < paramA.Trim().IndexOf('.') 
                                           ? paramA.Trim().Length :
                                             paramA.Trim().IndexOf('.'));

                            dataTableName = paramA.Trim().Substring(0, paramAL);
                        }

                        if (dataTableName.Trim().ToUpper() == "COADDITIONAL")
                            dataTableName = "MANUFACT";

                        try
                        {
                            if (paramA.Trim().ToUpper().IndexOf("ALLTRIM(") >= 0 ||
                                paramA.Trim().ToUpper().IndexOf("ALLT(") >= 0)
                                colName = paramA.Trim().Substring(paramA.Trim().IndexOf('.') + 1, (paramA.Length - paramA.Trim().IndexOf('.')) - 2);
                            else
                                colName = paramA.Trim().Substring(paramA.Trim().IndexOf('.') + 1, (paramA.Length - paramA.Trim().IndexOf('.')) - 1);

                            discValue += "'" + MainDataSet.Tables[dataTableName].Rows[0][colName] + "'";
                        }
                        catch
                        {

                        }
                    }
                    ParamValue = parameterFieldDefinitions[i].CurrentValues;
                    disCreteValue.Value = discValue.Trim(); 
                    ParamValue.Add(disCreteValue);
                    parameterFieldDefinitions[i].ApplyCurrentValues(ParamValue);
                }
                else
                {
                    if (param.Trim().IndexOf('.') >= 0)
                    {
                        dataTableName = param.Trim().Substring(0, param.Trim().IndexOf('.'));

                        if (dataTableName.Trim().ToUpper() == "COADDITIONAL")
                            dataTableName = "MANUFACT";

                        try
                        {
                            colName = param.Trim().Substring(param.Trim().IndexOf('.') + 1, (param.Length - param.Trim().IndexOf('.')) - 1);
                            ParamValue = parameterFieldDefinitions[i].CurrentValues;
                            switch (MainDataSet.Tables[dataTableName].Rows[0][colName].GetType().ToString().Trim().ToUpper())
                            {
                                case "SYSTEM.STRING":
                                    //disCreteValue.Value = "'" + MainDataSet.Tables[dataTableName].Rows[0][colName] + "'";
                                    disCreteValue.Value = Convert.ToString(MainDataSet.Tables[dataTableName].Rows[0][colName]).Trim();
                                    break;
                                case "SYSTEM.DATETIME":
                                    disCreteValue.Value = dateFormat.TodateTime(MainDataSet.Tables[dataTableName].Rows[0][colName]);
                                    break;
                                case "SYSTEM.BOOLEAN":
                                    disCreteValue.Value = bitFunction.toBoolean(MainDataSet.Tables[dataTableName].Rows[0][colName]);
                                    break;
                                case "SYSTEM.DECIMAL":
                                case "SYSTEM.NUMERIC":
                                case "SYSTEM.INT32":
                                case "SYSTEM.INT16":
                                case "SYSTEM.INT64":
                                    disCreteValue.Value = numFunction.toDecimal(MainDataSet.Tables[dataTableName].Rows[0][colName]);
                                    break;
                                case "SYSTEM.DBNULL":
                                    disCreteValue.Value = "";
                                    break;
                            }
                            
                            ParamValue.Add(disCreteValue);
                            parameterFieldDefinitions[i].ApplyCurrentValues(ParamValue);
                        }
                        catch
                        {

                        }
                    }
                    else
                    {
                        if (param.Trim().ToUpper() == "MUSERNAME")
                        {
                            ParamValue = parameterFieldDefinitions[i].CurrentValues;
                            disCreteValue.Value = SessionProxy.UserID.Trim();
                            ParamValue.Add(disCreteValue);
                            parameterFieldDefinitions[i].ApplyCurrentValues(ParamValue);
                        }
                            
                    }
                }
            }

            if (RepHandle.Subreports.Count > 0 && IsSubReport == true)
            {
                string subParam = "";
                ParameterValues subParamValue;
                ParameterDiscreteValue subDisCreteValue = new ParameterDiscreteValue();
                ParameterFieldDefinitions subParameterFieldDefinitions;

                for (int i=0;i<RepHandle.Subreports.Count;i++)
                {
                    if (IsSubReport == true && RepHandle.Subreports.Count > 0)
                    {
                        if (subSqlStr == null)
                        {
                            RepHandle.Subreports[i].SetDataSource(MainReportCommandSet.Tables[1]);
                        }
                        else
                        {
                            RepHandle.Subreports[i].SetDataSource(SubReportCommandSet.Tables[i]);
                        }
                    }
                    else
                    {
                        RepHandle.SetDataSource(SubReportCommandSet.Tables[i]);
                    }

                    subParameterFieldDefinitions = RepHandle.Subreports[i].DataDefinition.ParameterFields;
                    for (int sParamCnt = 0; sParamCnt < subParameterFieldDefinitions.Count-1; sParamCnt++)
                    {
                        subParam = subParameterFieldDefinitions[sParamCnt].ParameterFieldName;
                         if (subParam.Trim().IndexOf('.') >= 0)
                         {
                            dataTableName = subParam.Trim().Substring(0, subParam.Trim().IndexOf('.'));
                            
                            if (dataTableName.Trim().ToUpper() == "COADDITIONAL")
                                dataTableName = "MANUFACT";

                            colName = subParam.Trim().Substring(subParam.Trim().IndexOf('.')+1,(subParam.Length - subParam.Trim().IndexOf('.'))-1);
                            subParamValue = subParameterFieldDefinitions[sParamCnt].CurrentValues;
                            subDisCreteValue.Value = "'" + MainDataSet.Tables[dataTableName].Rows[0][colName] + "'";
                            subParamValue.Add(subDisCreteValue);
                            subParameterFieldDefinitions[sParamCnt].ApplyCurrentValues(subParamValue);
                         }
                    }
                }
            }

          //  CRViewer.HasCrystalLogo = false;
            CRViewer.ReportSource = RepHandle;

            //ScriptManager.RegisterStartupScript(this, this.GetType(), "init", "init();", true);
        }
    }
}
