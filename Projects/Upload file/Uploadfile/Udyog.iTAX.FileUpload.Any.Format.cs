using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Web; 
using System.Web.UI;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;
using System.Threading; 
using Microsoft.Office;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;
using Microsoft.Office.Interop.PowerPoint;


namespace Udyog.iTAX.FileUpload.Any.Format
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("Udyog.iTAX.FileUpload.Any.Format")]     

    public class Getfiles
    {
        private static SqlConnection conn1;
        private static SqlTransaction tran;

        protected SqlConnection ConnectionOpen(string connString) 
        {
            SqlConnection conn = new SqlConnection();
            conn = new SqlConnection(connString);
            return conn;
        }

        public void ConnectionClose()
        {
            if (conn1 != null)
            {
                conn1.Close();
            }
        }

        protected  void SaveImageProc(string trtype,int trid,int trserial,string filename,string extension,string filePath,byte[] fileObject,string connString,string pWhat,string fileSource,string pItSerial)
        {

            ShowWindow ss = new ShowWindow();
            ss.Show(); 

            if (conn1 == null)
            {
                conn1 = ConnectionOpen(connString);
                conn1.Open();
            }
            else
            {
                if (conn1.State == ConnectionState.Closed)
                {
                    conn1 = ConnectionOpen(connString);
                    conn1.Open();
                }
            }

            try
            {
                SqlCommand cmd = new SqlCommand("USP_UPLOADFILE_ANY_FORMAT", conn1);
                tran = conn1.BeginTransaction();
                cmd.Transaction = tran; 
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@TrType", trtype));
                cmd.Parameters.Add(new SqlParameter("@TrId", trid));
                cmd.Parameters.Add(new SqlParameter("@TrSerial", trserial));
                cmd.Parameters.Add(new SqlParameter("@Filename", filename.ToString().Trim()));
                cmd.Parameters.Add(new SqlParameter("@Extension", extension.ToString().Trim()));
                cmd.Parameters.Add(new SqlParameter("@ObjImage", fileObject));
                cmd.Parameters.Add(new SqlParameter("@ObjPath", filePath.ToString().Trim()));
                cmd.Parameters.Add(new SqlParameter("@pWhat", pWhat));
                cmd.Parameters.Add(new SqlParameter("@pItSerial", pItSerial));
                cmd.ExecuteNonQuery();
            }
            catch(SqlException SQ)
            {
                tran.Rollback();
                ss.Hide();
                MessageBox.Show(SQ.Message.ToString(),"",MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;

            }

            try
            {
                if (fileSource != null && filePath != null)
                {
                    string FileObject;
                    FileObject = filePath.ToString().Trim() + "\\" + filename.ToString().Trim() + "."+ extension.ToString().Trim();
                    File.Copy(fileSource, FileObject,true);
                }

                ss.Hide(); 
                tran.Commit();
            }
            catch(System.Exception Ex)
            {
                ss.Hide();
                MessageBox.Show(Ex.Message.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
         }

        protected  byte[] getByteArray(string XLSPath)
        {
            //Streams the generated XLS file to the user
            byte[] Buffer = null;
            using (FileStream MyFileStream = new FileStream(XLSPath, FileMode.Open))
            {
                // Total bytes to read: 
                long size;
                size = MyFileStream.Length;
                Buffer = new byte[size];
                MyFileStream.Read(Buffer, 0, int.Parse(MyFileStream.Length.ToString()));
            }
            return Buffer;
        }

        public void DeleteImage(string trtype, int trid, int trserial, string connString, bool DatabaseSave, string pItSerial)
        {
          

            if (conn1 == null)
            {
                conn1 = ConnectionOpen(connString);
                conn1.Open();
            }
            else
            {
                if (conn1.State == ConnectionState.Closed)
                {
                    conn1 = ConnectionOpen(connString);
                    conn1.Open();
                }
            }

            string SqlStr = "select tr_type,tr_id,tr_serial,filename,Extension,ObjPath,tr_itserial from uploadfiles where tr_type = '" + trtype.ToString().Trim() + "' and tr_id = " + trid.ToString().Trim() + " and tr_serial = " + trserial.ToString().Trim() + " and tr_itserial = '" + pItSerial.Trim() + "' and [filename] is not null and extension is not null";

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(SqlStr, conn1);
            da.Fill(ds, "_getData");
            conn1.Close(); 

            string Primary = null;
            string Extension = null;
            string FilePath = null;

            if (ds.Tables[0].Rows.Count > 0)
            {
                Primary = ds.Tables[0].Rows[0]["filename"].ToString();
                Extension = ds.Tables[0].Rows[0]["Extension"].ToString();
                FilePath = ds.Tables[0].Rows[0]["ObjPath"].ToString();
            }
            else
            {
                MessageBox.Show("File not found for Delete..!!!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult dlgResult = MessageBox.Show("Want to Delete ?", "Continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlgResult == DialogResult.No)
            {
                return;
            }

            try
            {
                if (conn1 == null)
                {
                    conn1 = ConnectionOpen(connString);
                    conn1.Open();
                }
                else
                {
                    if (conn1.State == ConnectionState.Closed)
                    {
                        conn1 = ConnectionOpen(connString);
                        conn1.Open();
                    }
                }


                string mSqlstr = "update uploadfiles set objImage = null,filename = null,Extension=null,objpath=null where tr_type = '" + trtype.ToString().Trim() + "' and tr_id = " + trid.ToString().Trim() + " and tr_serial = " + trserial.ToString().Trim() + " and tr_itserial = '" + pItSerial.Trim() + "' ";
                SqlCommand cmd = new SqlCommand(mSqlstr, conn1);
                tran = conn1.BeginTransaction();
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (SqlException Sq)
            {
                MessageBox.Show(Sq.Message.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                conn1.Close(); 
                return;
            }

            try
            {
                if (DatabaseSave == false)
                {
                    if ((Primary != null && Extension != null) || (Primary.ToString().Trim()  != "" && Extension.ToString().Trim() != ""))
                    {
                        string FileObject = FilePath.ToString().Trim() + "\\" + Primary.ToString().Trim() + "." + Extension.ToString().Trim();
                        MessageBox.Show(FileObject.ToString());
                        File.Delete(FileObject);

                    }
                }
            }
            catch(System.Exception Ex)
            {
                tran.Rollback();
                MessageBox.Show(Ex.Message.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                conn1.Close();
                return;
            }

            MessageBox.Show("Deleted..!!!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            tran.Commit();
            conn1.Close();
        }

        public void SaveImage(string trtype,int trid,int trserial,string filename,string extension,string filePath,string connectionString,string pWhat,bool  saveInFolder,string fileSource,string pItSerial)
        {
            if (saveInFolder == false)
            {
                SaveImageProc(trtype, trid, trserial, filename, extension, filePath, getByteArray(filePath), connectionString, pWhat, null, pItSerial);
            }
            else
            {
                SaveImageProc(trtype, trid, trserial, filename, extension, filePath, null, connectionString, pWhat, fileSource, pItSerial);
            }
        }

        public void RetriveImage(string trtype, int trid, int trserial, string connString, string pItSerial)
        {

            Thread RetriveImageThread = new Thread(delegate()
            {

                ShowWindow ss = new ShowWindow();
                ss.Show();

                if (conn1 == null)
                {
                    conn1 = ConnectionOpen(connString);
                    conn1.Open();
                }
                else
                {
                    if (conn1.State == ConnectionState.Closed)
                    {
                        conn1 = ConnectionOpen(connString);
                        conn1.Open();
                    }
                }


                DataSet ds = new DataSet();
                string SqlStr = "select tr_type,tr_id,tr_serial,filename,Extension,ObjPath,objImage,tr_itSerial from uploadfiles where tr_type = '" + trtype.ToString().Trim() + "' and tr_id = " + trid.ToString().Trim() + " and tr_serial = " + trserial.ToString().Trim() + " and tr_itserial = '" + pItSerial.Trim() + "' and objImage is not null and [filename] is not null and extension is not null";
                SqlDataAdapter da = new SqlDataAdapter(SqlStr, conn1);
                da.Fill(ds, "_uploadfile");
                conn1.Close();

                string Primary = "";
                string Extension = "";
                byte[] bytefile = null;
                string strfn = "";

                if (ds.Tables[0].Rows.Count > 0)
                {
                    Primary = ds.Tables[0].Rows[0]["filename"].ToString();
                    Extension = ds.Tables[0].Rows[0]["Extension"].ToString();

                    try
                    {
                        bytefile = (byte[])ds.Tables[0].Rows[0]["ObjImage"];
                    }
                    catch (System.Exception EQ)
                    {
                        // MessageBox.Show(EQ.Message.ToString()); 
                    }

                    if ((Primary == null && Extension == null) || (Primary.ToString() == "" && Extension.ToString() == ""))
                    {
                        ss.Hide();
                        MessageBox.Show("No file found for View", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else
                    {
                        strfn = System.IO.Path.GetTempPath() + Convert.ToString(DateTime.Now.ToFileTime()) + '.' + Extension.ToString().Trim();
                        FileStream fs = new FileStream(strfn, FileMode.CreateNew, FileAccess.Write);
                        fs.Write(bytefile, 0, bytefile.Length);
                        fs.Flush();
                        fs.Close();
                    }

                }
                else
                {
                    ss.Hide();
                    MessageBox.Show("File not found for View..!!!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                ss.Hide();

                try
                {
                    switch (Extension)
                    {
                        case "XLS":
                            Worksheet ExcelWorkSheet = new Worksheet();
                            Sheets ExcelSheet;
                            Workbook ExcelWorkBook;
                            Microsoft.Office.Interop.Excel.Application ExcelApps = new Microsoft.Office.Interop.Excel.Application();
                            ExcelWorkBook = ExcelApps.Workbooks.Open(strfn, 0, false, 5, "", "", false, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", true, true, 0, true, 0, 0);
                            ExcelSheet = ExcelWorkBook.Worksheets;
                            ExcelApps.Visible = true;
                            break;
                        case "BMP":
                        case "JPG":
                        case "GIF":
                        case "PNG":
                        case "TIF":
                            System.Diagnostics.Process.Start(strfn);
                            break;
                        case "DOC":
                            object filename = strfn;
                            object missing = System.Reflection.Missing.Value;
                            object readOnly = false;
                            object isVisible = true;
                            Microsoft.Office.Interop.Word.ApplicationClass WordApp = new Microsoft.Office.Interop.Word.ApplicationClass();
                            Document WordDoc = new Document();
                            WordApp.Visible = true;
                            WordDoc = WordApp.Documents.Open(ref filename, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);
                            WordDoc.Activate();
                            break;
                        case "PPT":
                        case "PPS":
                            Microsoft.Office.Interop.PowerPoint.Application PowerPointApp = new Microsoft.Office.Interop.PowerPoint.Application();
                            Presentation PowerPointPresentation;
                            PowerPointApp.Visible = Microsoft.Office.Core.MsoTriState.msoTrue;
                            PowerPointApp.WindowState = PpWindowState.ppWindowMaximized;
                            PowerPointPresentation = PowerPointApp.Presentations.Open(strfn, 0, 0, 0);
                            SlideShowSettings _SlideshowSetting;
                            _SlideshowSetting = PowerPointPresentation.SlideShowSettings;
                            _SlideshowSetting.Run();
                            break;
                        case "PDF":
                            System.Diagnostics.Process.Start(strfn);
                            break;
                        default:
                            System.Diagnostics.Process.Start(strfn);
                            break;
                    }
                }
                catch
                {
                    System.Diagnostics.Process.Start(strfn);

                }
            });

            RetriveImageThread.Name = "RetriveImage Thread";
            RetriveImageThread.Start(); 
            

        }

        public void RetrivePath(string trtype, int trid, int trserial, string connString, string pItSerial)
        {
            //Thread RetrivePathThread = new Thread(delegate()
            //{
                ShowWindow ss = new ShowWindow();
                ss.Show();

                if (conn1 == null)
                {
                    conn1 = ConnectionOpen(connString);
                    conn1.Open();
                }
                else
                {
                    if (conn1.State == ConnectionState.Closed)
                    {
                        conn1 = ConnectionOpen(connString);
                        conn1.Open();
                    }
                }

                DataSet ds = new DataSet();
                string SqlStr = "select tr_type,tr_id,tr_serial,filename,Extension,ObjPath,tr_itserial from uploadfiles where tr_type = '" + trtype.ToString().Trim() + "' and tr_id = " + trid.ToString().Trim() + " and tr_serial = " + trserial.ToString().Trim() + " and tr_itserial = '" + pItSerial.Trim() + "' and [filename] is not null and extension is not null";

                SqlDataAdapter da = new SqlDataAdapter(SqlStr, conn1);
                da.Fill(ds, "_uploadfile");
                conn1.Close();

                string Primary = "";
                string Extension = "";
                string strfn = "";

                if (ds.Tables[0].Rows.Count > 0)
                {
                    string filePath = "";
                    Primary = ds.Tables[0].Rows[0]["filename"].ToString();
                    Extension = ds.Tables[0].Rows[0]["Extension"].ToString();
                    filePath = ds.Tables[0].Rows[0]["ObjPath"].ToString();
                    strfn = filePath.ToString().Trim() + "\\" + Primary.ToString().Trim() + "." + Extension.ToString().Trim();
                }
                else
                {
                    ss.Hide();
                    MessageBox.Show("File not found for View..!!!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                ss.Hide();
                if (strfn != "" || strfn != null)
                {
                    System.Diagnostics.Process.Start(strfn);
                }
            //});

            /*RetrivePathThread.Name = "RetrivePath Thread";
            RetrivePathThread.Start(); */

        }

        /* Added By Sachin N. S. -- Works In Add & Edit Mode -- Start */
        public void RetrivePathEdit(string filename, string Extension1, string ObjPath)
        {
            ShowWindow ss = new ShowWindow();
            ss.Show();
            string Primary = "";
            string Extension = "";
            string strfn = "";

            if (filename != "" && Extension1 != "" && ObjPath != "")
            {
                string filePath = "";
                Primary = filename.Trim();
                Extension = Extension1.Trim();
                filePath = ObjPath.Trim();
                strfn = filePath.ToString().Trim() + "\\" + Primary.ToString().Trim() + "." + Extension.ToString().Trim();
            }
            else
            {
                ss.Hide();
                MessageBox.Show("File not found for View..!!!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ss.Hide();
            if (strfn != "" || strfn != null)
            {
                System.Diagnostics.Process.Start(strfn);
            }
        }
        /* Added By Sachin N. S. -- Works In Add & Edit Mode -- End */


        public void Dispose()
        {
            System.ComponentModel.Component destroyobj = new System.ComponentModel.Component();
            destroyobj.Dispose(); 
        }

        public void OpenImage(byte[] ObjByte,string PrimaryName,string ExtensionName)
        {

            string Primary = "";
            string Extension = "";
            byte[] bytefile = null;
            string strfn = "";

            Primary = PrimaryName.ToString().Trim();
            Extension = ExtensionName.ToString().Trim();
            bytefile = ObjByte; 

            strfn = System.IO.Path.GetTempPath() + Convert.ToString(DateTime.Now.ToFileTime()) + '.' + Extension.ToString().Trim();
            FileStream fs = new FileStream(strfn, FileMode.CreateNew, FileAccess.Write);
            fs.Write(bytefile, 0, bytefile.Length);
            fs.Flush();
            fs.Close();
            
            switch (Extension)
            {
                case "XLS":
                    Worksheet ExcelWorkSheet = new Worksheet();
                    Sheets ExcelSheet;
                    Workbook ExcelWorkBook;
                    Microsoft.Office.Interop.Excel.Application ExcelApps = new Microsoft.Office.Interop.Excel.Application();
                    ExcelWorkBook = ExcelApps.Workbooks.Open(strfn, 0, false, 5, "", "", false, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", true, true, 0, true, 0, 0);
                    ExcelSheet = ExcelWorkBook.Worksheets;
                    ExcelApps.Visible = true;
                    break;
                case "BMP":
                case "JPG":
                case "GIF":
                case "PNG":
                case "TIF":
              
                case "DOC":
                    object filename = strfn;
                    object missing = System.Reflection.Missing.Value;
                    object readOnly = false;
                    object isVisible = true;
                    Microsoft.Office.Interop.Word.ApplicationClass WordApp = new Microsoft.Office.Interop.Word.ApplicationClass();
                    Document WordDoc = new Document();
                    WordApp.Visible = true;
                    WordDoc = WordApp.Documents.Open(ref filename, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);
                    WordDoc.Activate();
                    break;
                case "PPT":
                case "PPS":
                    Microsoft.Office.Interop.PowerPoint.Application PowerPointApp = new Microsoft.Office.Interop.PowerPoint.Application();
                    Presentation PowerPointPresentation;
                    PowerPointApp.Visible = Microsoft.Office.Core.MsoTriState.msoTrue;
                    PowerPointApp.WindowState = PpWindowState.ppWindowMaximized;
                    PowerPointPresentation = PowerPointApp.Presentations.Open(strfn, 0, 0, 0);
                    SlideShowSettings _SlideshowSetting;
                    _SlideshowSetting = PowerPointPresentation.SlideShowSettings;
                    _SlideshowSetting.Run();
                    break;
                case "PDF":
                    System.Diagnostics.Process.Start(strfn);
                    break;
            }
        }
        //public void RetriveImage(string Sqlstr,string connectionString  )
        //{
        //    SqlConnection conn = new SqlConnection();
        //    conn.ConnectionString = connectionString; 
        //    //"Data Source=uday;Initial Catalog=A010708;User ID=sa;password=sa1985";
        //    conn.Open();
        //    DataSet ds = new DataSet();
        //    SqlDataAdapter da = new SqlDataAdapter(Sqlstr, conn);
        //    da.Fill(ds, "_uploadfile");
        //    conn.Close(); 


        //    string Primary = "";
        //    string Extension = "";
        //    byte[] bytefile;

        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        Primary = ds.Tables[0].Rows[0]["filePname"].ToString();
        //        Extension = ds.Tables[0].Rows[0]["fileEname"].ToString();
        //        bytefile = (byte[])ds.Tables[0].Rows[0]["fileobj"];
        //    }
        //    else
        //    {
        //        return;
        //    }

        //    string fileObject = Primary.ToString().Trim() + Extension.ToString().Trim();   
        //    System.Web.UI.Page _page = new Page();   // Inherit Web Page
        //    _page.Response.Clear();  

        //    switch (Extension)
        //    {
        //        case  "XLS" :
        //              _page.Response.ContentType = "application/vnd.ms-excel";
        //              _page.Response.AddHeader("content-disposition", "attachment;filename=" + fileObject.ToString().Trim() );
        //              break;
        //        case "BMP" :
        //        case "JPG" :
        //        case "GIF" :
        //        case "PNG" :
        //        case "TIF" :
        //            _page.Response.ContentType = "application/vnd.pbrush";
        //            _page.Response.AddHeader("content-disposition", "attachment;filename=" + fileObject.ToString().Trim());
        //            break;
        //        case "DOC" :
        //            _page.Response.ContentType = "application/vnd.ms-word";
        //            _page.Response.AddHeader("content-disposition", "attachment;filename=" + fileObject.ToString().Trim());
        //            break;
        //        case "PPT" :
        //        case "PPS" :
        //            _page.Response.ContentType = "application/vnd.ms-powerpoint";
        //            _page.Response.AddHeader("content-disposition", "attachment;filename=" + fileObject.ToString().Trim());
        //            break;
        //    }

        //    _page.Response.Charset = "";
        //    _page.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    _page.Response.BinaryWrite(bytefile);
        //    _page.Response.End();

        //}
    }
}
