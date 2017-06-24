using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSExcel=Microsoft.Office.Interop.Excel;
using System.IO;

namespace eFillingExtraction
{
    class WriteTDSExcel
    {
        object missing = Type.Missing;
        MSExcel.Application oXL;
        private string _ExcelFilName;

        public WriteTDSExcel(string ExcelFileName)
        {
            _ExcelFilName = ExcelFileName;
        }
        
        public string WriteToFile(DataSet WriteDataSet)
        {
            string ReturnValue = string.Empty;
           
            try
            {
                MSExcel.Application oXL = new MSExcel.Application();
                oXL.Visible = false;
                oXL.DisplayAlerts = false;

                //Writing Deductee Heading
                MSExcel.Workbook oWB = oXL.Workbooks.Open(_ExcelFilName, 0, false, 5, "", "", false, MSExcel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
                MSExcel.Sheets oWorkSheets = oWB.Worksheets;

                MSExcel.Worksheet oSheet = (MSExcel.Worksheet)oWorkSheets.get_Item("Deductee");

                //for (int j = 0; j < WriteDataSet.Tables["Deductee"].Columns.Count; j++)
                //{
                //    MSExcel.Range CellRange = oSheet.get_Range(oSheet.Cells[2, j + 1], oSheet.Cells[WriteDataSet.Tables["Deductee"].Rows.Count + 1, j + 1]);
                //    switch (WriteDataSet.Tables["Deductee"].Columns[j].GetType().ToString())
                //    {
                //        case "System.String":
                //            CellRange.HorizontalAlignment = MSExcel.XlHAlign.xlHAlignLeft;
                //            break;
                //        case "System.Decimal":
                //            CellRange.NumberFormat = "0.00";
                //            CellRange.HorizontalAlignment = MSExcel.XlHAlign.xlHAlignRight;
                //            break;
                //        default:
                //            break;
                //    }
                //    CellRange.VerticalAlignment = MSExcel.XlVAlign.xlVAlignCenter;
                //    CellRange.Font.Size = 9;
                //    CellRange.Font.Name = "Calibri";
                //}

                //Writing Deductee Values
                for (int i = 1; i <= WriteDataSet.Tables["Deductee"].Rows.Count; i++)
                {
                    for (int j = 1; j <= WriteDataSet.Tables["Deductee"].Columns.Count; j++)
                    {
                        MSExcel.Range CellRange = oSheet.get_Range(oSheet.Cells[i + 1, j], oSheet.Cells[i + 1, j]);
                        switch (WriteDataSet.Tables["Deductee"].Rows[i-1][j-1].GetType().ToString())
                        {
                            case "System.String":
                                oSheet.Cells[i + 1, j] ="'"+Convert.ToString( WriteDataSet.Tables["Deductee"].Rows[i - 1][j - 1]);
                                CellRange.HorizontalAlignment = MSExcel.XlHAlign.xlHAlignLeft;
                                break;
                            case "System.Decimal":
                                CellRange.NumberFormat="0.00";
                                oSheet.Cells[i + 1, j] = Convert.ToDecimal(WriteDataSet.Tables["Deductee"].Rows[i - 1][j - 1]);
                                CellRange.HorizontalAlignment = MSExcel.XlHAlign.xlHAlignRight;
                                break;
                            default:
                                break;
                        }
                        CellRange.VerticalAlignment = MSExcel.XlVAlign.xlVAlignCenter;
                        CellRange.Font.Size = 9;
                        CellRange.Font.Name = "Calibri";
                    }
                }

                //Writing Challan heading
                MSExcel.Worksheet oSheet2 = (MSExcel.Worksheet)oWorkSheets.get_Item("Challan");

                //for (int j = 0; j < WriteDataSet.Tables["Challan"].Columns.Count; j++)
                //{
                //    MSExcel.Range CellRange = oSheet2.get_Range(oSheet2.Cells[2, j+1], oSheet2.Cells[WriteDataSet.Tables["Challan"].Rows.Count+1, j+1]);
                //    switch (WriteDataSet.Tables["Challan"].Columns[j].GetType().ToString())
                //    {
                //        case "System.String":
                //            CellRange.HorizontalAlignment = MSExcel.XlHAlign.xlHAlignLeft;
                //            break;
                //        case "System.Decimal":
                //            CellRange.NumberFormat = "0.00";
                //            CellRange.HorizontalAlignment = MSExcel.XlHAlign.xlHAlignRight;
                //            break;
                //        default:
                //            break;
                //    }
                //    CellRange.VerticalAlignment = MSExcel.XlVAlign.xlVAlignCenter;
                //    CellRange.Font.Size = 9;
                //    CellRange.Font.Name = "Calibri";
                //}
                //Writing Challan values
                for (int i = 1; i <= WriteDataSet.Tables["Challan"].Rows.Count; i++)
                {
                    for (int j = 1; j <= WriteDataSet.Tables["Challan"].Columns.Count; j++)
                    {
                        MSExcel.Range CellRange = oSheet2.get_Range(oSheet2.Cells[i + 1, j], oSheet2.Cells[i + 1, j]);
                        switch (WriteDataSet.Tables["Challan"].Rows[i - 1][j - 1].GetType().ToString())
                        {
                            case "System.String":
                                oSheet2.Cells[i + 1, j] = "'" + Convert.ToString(WriteDataSet.Tables["Challan"].Rows[i - 1][j - 1]);
                                CellRange.HorizontalAlignment = MSExcel.XlHAlign.xlHAlignLeft;
                                break;
                            case "System.Decimal":
                                CellRange.NumberFormat = "0.00";
                                oSheet2.Cells[i + 1, j] = Convert.ToDecimal(WriteDataSet.Tables["Challan"].Rows[i - 1][j - 1]);
                                CellRange.HorizontalAlignment = MSExcel.XlHAlign.xlHAlignRight;
                                break;
                            default:
                                break;
                        }
                        CellRange.VerticalAlignment = MSExcel.XlVAlign.xlVAlignCenter;
                        CellRange.Font.Size = 9;
                        CellRange.Font.Name = "Calibri";
                    }
                }

                //Writing Company heading
                bool GeneralTab = false;
                foreach (MSExcel.Worksheet sheet in oWB.Worksheets)
                {
                    if (sheet.Name.Trim() == "General")
                    {
                        GeneralTab = true;
                        break;
                    }
                }


                if (GeneralTab == true)
                {
                    MSExcel.Worksheet oSheet3 = (MSExcel.Worksheet)oWorkSheets.get_Item("General");
                    //oSheet3.Name = "General";

                    //Commented By Kishor A. for Bug-26391 on 06/07/2015 Start

                    //Writing Company values
                    //for (int i = 1; i <= WriteDataSet.Tables["Company"].Rows.Count; i++)
                    //{
                    //    for (int j = 1; j <= WriteDataSet.Tables["Company"].Columns.Count; j++)
                    //    {
                    //        MSExcel.Range CellRange = oSheet3.get_Range(oSheet3.Cells[i + 1, j], oSheet3.Cells[i + 1, j]);
                    //        switch (WriteDataSet.Tables["Company"].Rows[i - 1][j - 1].GetType().ToString())
                    //        {
                    //            case "System.String":
                    //                oSheet3.Cells[i + 1, j] = "'" + Convert.ToString(WriteDataSet.Tables["Company"].Rows[i - 1][j - 1]);
                    //                CellRange.HorizontalAlignment = MSExcel.XlHAlign.xlHAlignLeft;
                    //                break;
                    //            case "System.Decimal":
                    //                CellRange.NumberFormat = "0.00";
                    //                oSheet3.Cells[i + 1, j] = Convert.ToDecimal(WriteDataSet.Tables["Company"].Rows[i - 1][j - 1]);
                    //                CellRange.HorizontalAlignment = MSExcel.XlHAlign.xlHAlignRight;
                    //                break;
                    //            default:
                    //                break;
                    //        }
                    //        CellRange.VerticalAlignment = MSExcel.XlVAlign.xlVAlignCenter;
                    //        CellRange.Font.Size = 9;
                    //        CellRange.Font.Name = "Calibri";
                    //    }
                    //}

                    //Commented By Kishor A. for Bug-26391 on 06/07/2015 End

                    //Added Commented By Kishor A. for Bug-26391 on 06/07/2015 Start..
                    for (int i = 1; i <= WriteDataSet.Tables["General"].Rows.Count; i++)
                    {
                        for (int j = 1; j <= WriteDataSet.Tables["General"].Columns.Count; j++)
                        {
                            MSExcel.Range CellRange = oSheet3.get_Range(oSheet3.Cells[i + 1, j], oSheet3.Cells[i + 1, j]);
                            switch (WriteDataSet.Tables["General"].Rows[i - 1][j - 1].GetType().ToString())
                            {
                                case "System.String":
                                    oSheet3.Cells[i + 1, j] = "'" + Convert.ToString(WriteDataSet.Tables["General"].Rows[i - 1][j - 1]);
                                    CellRange.HorizontalAlignment = MSExcel.XlHAlign.xlHAlignLeft;
                                    break;
                                case "System.Decimal":
                                    CellRange.NumberFormat = "0.00";
                                    oSheet3.Cells[i + 1, j] = Convert.ToDecimal(WriteDataSet.Tables["General"].Rows[i - 1][j - 1]);
                                    CellRange.HorizontalAlignment = MSExcel.XlHAlign.xlHAlignRight;
                                    break;
                                default:
                                    break;
                            }
                            CellRange.VerticalAlignment = MSExcel.XlVAlign.xlVAlignCenter;
                            CellRange.Font.Size = 9;
                            CellRange.Font.Name = "Calibri";
                        }
                    }
                    //Added Commented By Kishor A. for Bug-26391 on 06/07/2015 End..
                }

              
                //foreach (Worksheet sheet in wrkBook.Sheets)
                //{
                //    if (sheet.Name.ToLower() == "sheet1")
                //    {
                //        sheet.Delete();
                //        break;
                //    }
                //}

                //Releasing the Excel object
                oWB.SaveAs(_ExcelFilName, MSExcel.XlFileFormat.xlWorkbookNormal,
                    missing, missing, missing, missing,
                    MSExcel.XlSaveAsAccessMode.xlNoChange,
                    missing, missing, missing, missing, missing);
                oWB.Close(missing, missing, missing);
                oXL.UserControl = true;
                oXL.DisplayAlerts = true;
                
                oXL.Quit();
            }
            catch (Exception ex)
            {
                ReturnValue = ex.Message;
            }
            finally
            {
                if (oXL != null)
                    oXL.Quit();
                GC.Collect();
            }
            return ReturnValue;
        }

    }
}
