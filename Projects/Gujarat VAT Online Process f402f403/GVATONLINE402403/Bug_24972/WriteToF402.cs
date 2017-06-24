using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Reflection; 
using MSExcel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading;
using Bug_24972;
using EXCEL = Microsoft.Office.Interop.Excel;

namespace Bug_24972
{
    class WriteToF402
    {
        object missing = Type.Missing;
        MSExcel.Application oXL;
        private string _ExcelFilName;
        private string finalFileNameWithPath = string.Empty;
        bool lExist = false;


        public WriteToF402(string ExcelFileName)
        {
            string currentDirectorypath = Environment.CurrentDirectory;
            _ExcelFilName = System.IO.Path.Combine(currentDirectorypath, ExcelFileName);

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
           
           //finalFileNameWithPath = "Gis_Form_402_m_" + DateTime.Now.ToString("dd-MM-yyyy")+".xls";


           if (Frm402403.eFileType == 1)
           {
               if (Frm402403._strGVType == "NonSpecified")
               {
                   finalFileNameWithPath = "Gis_Form_402_m_" + Frm402403._strInvNo + ".xls";
               }
               else
               {
                   finalFileNameWithPath = "Gis_Form_402_specified_m_" + Frm402403._strInvNo + ".xls";
               }               
           }
           else
           {
               if (Frm402403._strGVType == "NonSpecified")
               {
                   finalFileNameWithPath = "Gis_Form_403_m_" + Frm402403._strInvNo + ".xls";
               }
               else
               {
                   finalFileNameWithPath = "Gis_Form_403_specified_m_" + Frm402403._strInvNo + ".xls";
               }   
           }
   

           finalFileNameWithPath = System.IO.Path.Combine(currentDirectorypath, finalFileNameWithPath);

           //finalFileNameWithPath = string.Format("{0}_{1}", "Gis_Form_402_m", DateTime.Now.ToString("dd-MM-yyyy"));
           //finalFileNameWithPath = System.IO.Path.Combine(finalFileNameWithPath, ".xls");
            //finalFileNameWithPath = string.Format("{0}\\{1}.xlsx", currentDirectorypath, _ExcelFilName);

                       
           try
           {
               if (File.Exists(finalFileNameWithPath))
               {
                   File.Delete(finalFileNameWithPath);
               }
               System.IO.File.Copy(_ExcelFilName, finalFileNameWithPath);
               lExist = false;
           }
           catch (Exception ex)
           {
               string error = "File already opened.";

               MessageBox.Show(error, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
               lExist = true;
               
           }
           finally
           {
               //Application.Exit();
               //object fileName = _ExcelFilName;
               //MSExcel.Application oXL = new MSExcel.Application();
               //oXL.Visible = true;
               //oXL.DisplayAlerts = false;

               //MSExcel.Workbook oWB = oXL.Workbooks.get_Item(fileName);
               //oWB.Close(false, _ExcelFilName, oWB);
           }

            
           //finalFileNameWithPath = string.Format("{0}\\{1}.xlsx", currentDirectorypath, _ExcelFilName);
 
           //Delete existing file with same file name.
           
           //var newFile = new FileInfo(finalFileNameWithPath);
           _ExcelFilName = finalFileNameWithPath;
           
           
         }

        public string WriteToFile(DataSet WriteDataSet)
        {
        
            string ReturnValue = string.Empty;
            int nR = 0;

            if (Frm402403.eFileType == 2)
            {
                nR = 1;
            }


            if (lExist == false)
            {
                try
                {
                    MSExcel.Application oXL = new MSExcel.Application();
                    oXL.Visible = true;
                    oXL.DisplayAlerts = false;


                    //Writing Deductee Heading
                    oXL.EnableEvents = false;       //Added by Shrikant S. on 16/10/2015 
                    MSExcel.Workbook oWB = oXL.Workbooks.Open(_ExcelFilName, 0, false, 5, "", "", false, MSExcel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
                    oXL.EnableEvents = true;       //Added by Shrikant S. on 16/10/2015 
                    MSExcel.Sheets oWorkSheets = oWB.Worksheets;


                    MSExcel.Worksheet oSheet = (MSExcel.Worksheet)oWorkSheets.get_Item("Basic Info");

                    //oSheet.Cells[9, 2] = 11111111111;

                    //Regex.Replace(your_String, @"[^0-9a-zA-Z]+", ",");
                    //string replacestr= Regex.Replace(str, "[^a-zA-Z0-9_]+", " ");

                    if (Frm402403.eFileType == 2)
                    {
                        oSheet.Cells[3, 2] = Convert.ToDecimal(WriteDataSet.Tables["Data"].Rows[0]["U_403srno"]);


                        oSheet.Cells[4, 2] = Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["U_CHKPST"]).Trim().ToUpper();

                        if (Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["ST_TYPE"]).Trim() == "OUT OF STATE")
                        {
                            //oSheet.Cells[ + nR6, 2] = Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["STATE"]).Trim();
                            oSheet.Cells[5, 2] = GetValidState(Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["STATE"]).Trim());
                        }
                        else
                        {
                            oSheet.Cells[5, 2] = "Outside India";
                        }

                        oSheet.Cells[6, 2] = Regex.Replace(Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["CITY"]).Trim().ToUpper(), @"[^0-9a-zA-Z]", " ");
                        oSheet.Cells[8, 2] = Frm402403._strCity.Trim().ToUpper();
                    }
                    else
                    {

                        oSheet.Cells[3 + nR, 2] = Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["U_CHKPST"]).Trim().ToUpper();
                        oSheet.Cells[5 + nR, 2] = Frm402403._strCity.Trim().ToUpper();

                        if (Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["ST_TYPE"]).Trim() == "OUT OF STATE")
                        {
                            //oSheet.Cells[ + nR6, 2] = Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["STATE"]).Trim();
                            oSheet.Cells[6 + nR, 2] = GetValidState(Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["STATE"]).Trim());
                        }
                        else
                        {
                            oSheet.Cells[6 + nR, 2] = "Outside India";
                        }

                        oSheet.Cells[7 + nR, 2] = Regex.Replace(Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["CITY"]).Trim().ToUpper(), @"[^0-9a-zA-Z]", " ");
                    }


                    oSheet.Cells[9 + nR, 2] = Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["C_TAX"]).Trim();
                    oSheet.Cells[10 + nR, 2] = Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["U_NT"]).Trim();
                    oSheet.Cells[12 + nR, 2] = Regex.Replace(Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["MAILNAME1"]).Trim(), @"[^0-9a-zA-Z]", " ");
                    oSheet.Cells[13 + nR, 2] = Regex.Replace(Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["ADD11"]).Trim() + Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["ADD22"]).Trim() + Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["ADD33"]).Trim(), @"[^0-9a-zA-Z]", " ");
                    oSheet.Cells[14 + nR, 2] = Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["S_TAX1"]).Trim();

                    if (string.IsNullOrEmpty(Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["C_TAX1"]).Trim()) == false)
                    {
                        oSheet.Cells[16 + nR, 2] = "Yes";
                        oSheet.Cells[16 + nR, 3] = Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["C_TAX1"]).Trim();
                    }
                    else
                    {
                        oSheet.Cells[16 + nR, 2] = "No";
                    }
                    

                    oSheet.Cells[20 + nR, 2] = Math.Round(Convert.ToDouble(WriteDataSet.Tables["Data"].Rows[0]["NET_AMT"]));


                    //if (Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["INV_SR"]).Trim() == "")
                    //{
                    //    oSheet.Cells[22 + nR, 1] = Convert.ToDecimal(WriteDataSet.Tables["Data"].Rows[0]["INV_NO"]);
                    //}
                    //else
                    //{

                    //    oSheet.Cells[22 + nR, 1] = Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["INV_SR"]).Trim() + " " + Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["INV_NO"]).Trim();

                    //}
                    oSheet.Cells[31 + nR, 1] = Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["INV_NO"]); //ruchit 05/01/2016
                    //DateTime dt = Convert.ToDateTime(WriteDataSet.Tables["Data"].Rows[0]["DATE"]);

                    //oSheet.Cells[22 + nR, 2] = String.Format(new CultureInfo("en-US"), "{0:dd/MM/yyyy}", WriteDataSet.Tables["Data"].Rows[0]["DATE"]);

                    //oSheet.Cells[22 + nR, 2] = Convert.ToDateTime(WriteDataSet.Tables["Data"].Rows[0]["DATE"]).ToString("dd/MM/yyyy");
                    oSheet.Cells[31 + nR, 2] = Convert.ToDateTime(WriteDataSet.Tables["Data"].Rows[0]["DATE"]).ToString("dd/MM/yyyy"); //ruchit 05/01/2016

                    MSExcel.Worksheet oSheet2 = (MSExcel.Worksheet)oWorkSheets.get_Item("Transport Detail");
                                  
                    if (!DBNull.Value.Equals(WriteDataSet.Tables["Data"].Rows[0]["U_TMODE"]))
                    {
                        oSheet2.Cells[3, 2] = Regex.Replace(Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["U_TMODE"]).Trim(), @"[^0-9a-zA-Z]", " ");
                    }

                    if (!DBNull.Value.Equals(WriteDataSet.Tables["Data"].Rows[0]["TRANDT"]))
                    {
                        if ((WriteDataSet.Tables["Data"].Rows[0]["TRANDT"]).ToString().Length > 0)       //added by Shrikant S. on 16/10/2015
                        {
                            oSheet2.Cells[6, 2] = Regex.Replace(Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["TRANDT"]).Substring(0, 15).Trim(), @"[^0-9a-zA-Z]", " ");
                            oSheet2.Cells[7, 2] = Regex.Replace(Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["TRANDT"]).Substring(15, 15).Trim(), @"[^0-9a-zA-Z]", " ");
                            oSheet2.Cells[8, 2] = Regex.Replace(Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["TRANDT"]).Substring(31, 19).Trim(), @"[^0-9a-zA-Z]", " ");
                        }
                    }

                    //oSheet2.Cells[5, 2] = Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["TRANDT"]).Substring(0, 15).Trim();
                  
                    if (!DBNull.Value.Equals(WriteDataSet.Tables["Data"].Rows[0]["U_OWNERNM"]))
                    {
                        oSheet2.Cells[15, 2] = Regex.Replace(Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["U_OWNERNM"]).Trim(), @"[^0-9a-zA-Z]", " ");
                    }
                   
                    if (!DBNull.Value.Equals(WriteDataSet.Tables["Data"].Rows[0]["U_VEHNO"]))
                    {
                        if (Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["U_VEHNO"]).Length > 10)
                        {
                            oSheet2.Cells[16, 2] = Regex.Replace(Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["U_VEHNO"]).Substring(0, 10).Trim(), @"[^0-9a-zA-Z]", " ");
                        }
                        else
                        {
                            oSheet2.Cells[16, 2] = Regex.Replace(Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["U_VEHNO"]).Trim(), @"[^0-9a-zA-Z]", " ");
                        }

                        //oSheet2.Cells[16, 2] = Regex.Replace(Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["U_VEHNO"]).Substring(0, 10).Trim(), @"[^0-9a-zA-Z]", " ");
                        //oSheet2.Cells[16, 2] = Regex.Replace(Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["U_VEHNO"]).Trim(), @"[^0-9a-zA-Z]", " ");
                        
                    }

                    if (!DBNull.Value.Equals(WriteDataSet.Tables["Data"].Rows[0]["U_LRNO"]))
                    {
                        oSheet2.Cells[18, 2] = Regex.Replace(Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["U_LRNO"]).Trim(), @"[^0-9a-zA-Z]", " ");
                    }
                    if (!DBNull.Value.Equals(WriteDataSet.Tables["Data"].Rows[0]["U_LRDT"]))
                    {
                        if (Convert.ToDateTime(WriteDataSet.Tables["Data"].Rows[0]["U_LRDT"]).ToString("dd/MM/yyyy") != "01/01/1900")
                        {
                            oSheet2.Cells[19, 2] = Convert.ToDateTime(WriteDataSet.Tables["Data"].Rows[0]["U_LRDT"]).ToString("dd/MM/yyyy");
                        }
                    }

                    if (!DBNull.Value.Equals(WriteDataSet.Tables["Data"].Rows[0]["U_DRVNAME"]))
                    {
                        oSheet2.Cells[21, 2] = Regex.Replace(Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["U_DRVNAME"]).Trim(), @"[^0-9a-zA-Z]", " ");
                    }

                    //MessageBox.Show(Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["INV_SR"]).Trim() + " " + Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["INV_NO"]).Trim());

                    if (!DBNull.Value.Equals(WriteDataSet.Tables["Data"].Rows[0]["U_DRVADD"]))
                    {
                        oSheet2.Cells[22, 2] = Regex.Replace(Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["U_DRVADD"]).Trim(), @"[^0-9a-zA-Z]", " ");
                    }

                    if (!DBNull.Value.Equals(WriteDataSet.Tables["Data"].Rows[0]["U_DRVLICEN"]))
                    {
                        oSheet2.Cells[23, 2] = Regex.Replace(Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["U_DRVLICEN"]).Trim(), @"[^0-9a-zA-Z]", " ");
                    }

                    //oSheet2.Cells[24, 2] = Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["U_LICSTATE"]).Trim();

                    oSheet2.Cells[24, 2] = GetValidState(Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["U_LICSTATE"]).Trim());

                    //Writing Item details

                    MSExcel.Worksheet oSheet1 = (MSExcel.Worksheet)oWorkSheets.get_Item("Commodity Details");

                    //MSExcel._Worksheet workSheet = (MSExcel.Worksheet)oXL.ActiveSheet;



                    //for (int i = 0; i <= WriteDataSet.Tables["Data"].Rows.Count; i++) //Commented by Shrikant S. on 16/10/2015 
                    for (int i = 0; i < WriteDataSet.Tables["Data"].Rows.Count; i++)
                    {

                        if (i < 25)
                        {

                            if (Convert.ToString(WriteDataSet.Tables["Data"].Rows[i]["EIT_NAME"]).Trim() == "")
                            {
                                oSheet1.Cells[2 + i, 1] = Convert.ToString(WriteDataSet.Tables["Data"].Rows[i]["IT_DESC"]).Trim();
                            }
                            else
                            {
                                oSheet1.Cells[2 + i, 1] = Convert.ToString(WriteDataSet.Tables["Data"].Rows[i]["EIT_NAME"]).Trim();
                            }


                            oSheet1.Cells[2 + i, 2] = Convert.ToDecimal(WriteDataSet.Tables["Data"].Rows[i]["QTY"]);
                            //oSheet1.Cells[2+i, 3] = Convert.ToString(WriteDataSet.Tables["Data"].Rows[i]["RATEUNIT"]).Trim().ToUpperInvariant();
                            oSheet1.Cells[2 + i, 3] = GetValidUnit(Convert.ToString(WriteDataSet.Tables["Data"].Rows[i]["RATEUNIT"]).Trim());

                            oSheet1.Cells[2 + i, 4] = Convert.ToDecimal(WriteDataSet.Tables["Data"].Rows[i]["TAXRATE"]);
                            oSheet1.Cells[2 + i, 5] = Convert.ToDecimal(WriteDataSet.Tables["Data"].Rows[i]["TAXAMT"]);
                            //MessageBox.Show(Convert.ToString(Frm402403._strInvNo));

                            //oSheet1.Cells.NumberFormat = "@";

                            //oSheet1.Cells[2 + i, 6] = " "+Convert.ToString(Frm402403._strInvNo);

                            // commented by Ruchit for 27116 START
                            //if (Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["INV_SR"]).Trim() == "")
                            //{
                            //    oSheet1.Cells[2 + i, 6] = Convert.ToDecimal(WriteDataSet.Tables["Data"].Rows[0]["INV_NO"]);

                            //}
                            //else
                            //{
                            //    oSheet1.Cells[2 + i, 6] = Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["INV_SR"]).Trim() + " " + Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["INV_NO"]).Trim();

                            //}
                            // commented by Ruchit for 27116 END
                            oSheet1.Cells[2 + i, 6] = Convert.ToString(WriteDataSet.Tables["Data"].Rows[0]["INV_NO"]).Trim(); //ruchit 05/01/2016

                            //oSheet1.Cells.NumberFormat = "@";
                            /*
                            Microsoft.Office.Interop.Excel._Worksheet _Worksheet = (MSExcel._Worksheet)oWB.ActiveSheet;

                            Microsoft.Office.Interop.Excel.Range headerRg_first = (MSExcel.Range)_Worksheet.Cells[2, 1];
                            Microsoft.Office.Interop.Excel.Range headerRg_last = (MSExcel.Range)_Worksheet.Rows.Cells[2, 6];

                            Microsoft.Office.Interop.Excel.Range headerRange = _Worksheet.get_Range(headerRg_first, headerRg_last);

                            headerRange.NumberFormat = "@";
                            */


                            //oSheet1.Cells[2 + i, 6] = @"000002".ToString();
                        }

                    }

                    //MessageBox.Show("Excel file has been generated at " + finalFileNameWithPath);

                    //Releasing the Excel object
                    /*
                    oWB.SaveAs(_ExcelFilName, MSExcel.XlFileFormat.xlWorkbookNormal,
                        missing, missing, missing, missing,
                        MSExcel.XlSaveAsAccessMode.xlNoChange,
                        missing, missing, missing, missing, missing);
                    oWB.Close(missing, missing, missing);
                    oXL.UserControl = true;
                    oXL.DisplayAlerts = true;
                    */
                    //oXL.Quit();                           

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

            }
            else
            {
                Application.Exit();
            }
            return ReturnValue;
        }

        /*
        public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int ProcessId);
        private static void KillExcel(EXCEL.Application theApp)
        {
            int id = 0;
            IntPtr intptr = new IntPtr(theApp.Hwnd);
            System.Diagnostics.Process p = null;
            try
            {
                GetWindowThreadProcessId(intptr, out id);
                p = System.Diagnostics.Process.GetProcessById(id);
                if (p != null)
                {
                    p.Kill();
                    p.Dispose();
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("KillExcel:" + ex.Message);
            }
        }
        */

        public string GetValidState(string cStateName)
        {
           switch (cStateName)
            {
                case "Andaman And Nicobar Island":
                   return "Andaman and Nicobar Islands";
          
                case "Dadra & Nagar Haveli":
                   return "Dadra and Nagar Haveli";

                case "Daman & DIU":
                   return "Daman And  Diu";

                case "Jammu & Kashmir":
                   return "Jammu And Kashmir";

                case "Lakshwadeep":
                   return "Lakshadweep";

                case "Odisha":
                   return "Orissa";

                case "Pondicherry":
                   return "Pondicherry or Pudducherry";
                   
                case "Tamil Nadu":
                   return "Tamilnadu";

               default:
                   return cStateName;
            }
          
        }


        public string GetValidUnit(string cUnit)
        {
            switch (cUnit)
            {
                case "kgs":
                    return "Kgs";

                case "ltr":
                    return "Ltrs";

                case "mts":
                    return "Mtrs";

                case "sqm":
                    return "Mtrs";

                case "nos":
                    return "Nos";

                case "pcs":
                    return "Nos";

                case "PCS":
                    return "Nos";

                case "ton":
                    return "Tons";
                    
                default:
                    return cUnit;
            }

        }
    }
}
