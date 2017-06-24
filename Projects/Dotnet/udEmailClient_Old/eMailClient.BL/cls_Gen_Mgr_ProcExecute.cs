using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Globalization;

using eMailClient.DAL;
using eMailClient.RPT;
using eMailClient.MailSender;

namespace eMailClient.BLL
{
    public class cls_Gen_Mgr_ProcExecute : cls_Gen_Ent_ProcExecute
    {
        #region variable declaration
        string connectionString;
        SqlConnection sqlconn;
        DataSet m_Obj_DS;
        DataSet m_Obj_logDs;

        cls_Gen_Mgr_Report m_Obj_Report;
        cls_Gen_Mgr_MailSender m_Obj_MailSender = new cls_Gen_Mgr_MailSender();
        cls_Gen_Mgr_Email_LogWriter m_Obj_Email_LogWriter;

        #endregion

        public cls_Gen_Mgr_ProcExecute(Int32 CompanyID, string ConnectionString)
        {
            // connectionString = ConfigurationManager.ConnectionStrings[Convert.ToString(CompanyID).Trim()].ConnectionString;
            connectionString = ConnectionString;
            this.CompanyID = CompanyID;     // Added by Sachin N. S. on 21/01/2014 for Bug-20211
            m_Obj_Report = new cls_Gen_Mgr_Report(CompanyID, ConnectionString);
            m_Obj_Report.CompanyID = CompanyID;
            m_Obj_Email_LogWriter = new cls_Gen_Mgr_Email_LogWriter(CompanyID, ConnectionString);
            m_Obj_Email_LogWriter.CompanyID = CompanyID;
            m_Obj_DS = new DataSet();
            getCompanyDirNm();      // Added by Sachin N. S. on 31/01/2014 for Bug-20211
        }

        public void ExecuteProcess()
        {
            if (ExecutePendingJob == true)
            {
                ExecutePendingProcess();
            }
            if (ExecuteJob == true)
            {
                Int32 Barvalue = 2;
                try
                {
                    // For Main Process
                    BgwkProcess.ReportProgress(Barvalue, "Process Started...");
                    Select(); // Call Main Query..
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error found in the ExecuteProcess Method  \n" +
                                        "Message : " + Ex.Message +
                                        "\nSource : " + Ex.Source +
                                        "\nTargetSite : " + Ex.TargetSite);
                }

                string logString = "Process Started....";
                string outIDString = string.Empty;
                LogWriter.LogFileName = "";     // Added by Sachin N. S. on 21/01/2014 for Bug-20211
                LogWriter.WriteLogToTextFile(LogFilePath, "-------------------------------------<<< START >>>-----------------------------", true, false);
                LogWriter.WriteLogToTextFile(LogFilePath, logString, false, false);
                decimal mainCursPrgsValue = 98.00m / Convert.ToInt32(m_Obj_DS.Tables[0].Rows.Count);

                // Check SMTP Settings

                DataRow smtpDR = m_Obj_DS.Tables[1].Rows[0];
                if (smtpDR == null)
                    throw new Exception("SMTP Setting table found empty...");

                CheckSMTPCredentials(smtpDR);

                foreach (DataRow m_Obj_DR in m_Obj_DS.Tables[0].Rows)
                {
                    try
                    {
                        outIDString = Convert.ToString(m_Obj_DR["id"]).Trim() + " [" + Convert.ToString(m_Obj_DR["Desc"]).Trim() + "] ";
                        LogWriter.WriteLogToTextFile(LogFilePath, "Process Start ID : " + outIDString, false, false);
                        BgwkProcess.ReportProgress(Barvalue, "Process Start ID :" + outIDString);
                        outIDString = "ID :" + Convert.ToString(m_Obj_DR["id"]).Trim();
                        if (BgwkProcess.CancellationPending)
                        {
                            BgwkProcess.ReportProgress(0, "^ Processing has been cancelled...");
                            break;
                        }

                        string tomailaddress = string.Empty;
                        if (Convert.ToString(m_Obj_DR["To"]).Trim() == string.Empty)
                        {
                            if (m_Obj_DS.Tables[0].Columns.Contains("email") == true)
                            {
                                tomailaddress = Convert.ToString(m_Obj_DR["Email"]).Trim();
                            }
                        }
                        else
                            tomailaddress = Convert.ToString(m_Obj_DR["To"]).Trim();

                        switch (Convert.ToString(m_Obj_DR["attachment_typ"]).Trim().ToUpper())
                        {
                            case "CSV":
                                ExportCSV(Convert.ToString(m_Obj_DR["id"]).Trim(),
                                          Convert.ToString(m_Obj_DR["exportpath"]).Trim(),
                                          Convert.ToString(m_Obj_DR["exportprefixname"]).Trim(),
                                          Convert.ToString(m_Obj_DR["parameters"]).Trim(),
                                          Convert.ToBoolean(m_Obj_DR["hasattachment"]),
                                          Convert.ToString(m_Obj_DR["query"]).Trim(),
                                          //Convert.ToString(m_Obj_DR["reportquery"]).Trim(),
                                          //Convert.ToString(m_Obj_DR["reportquerytype"]).Trim(),
                                          Convert.ToString(m_Obj_DR["sqlQuery"]).Trim(),        // Changed by Sachin N. S. on 21/01/2014 for Bug-20211
                                          Convert.ToString(m_Obj_DR["spWhat"]).Trim(),          // Changed by Sachin N. S. on 21/01/2014 for Bug-20211
                                          Convert.ToString(m_Obj_DR["separator"]).Trim(),
                                          Convert.ToString(m_Obj_DR["encoding"]).Trim(),
                                          Convert.ToBoolean(m_Obj_DR["isfirstrow"]),
                                          Convert.ToString(m_Obj_DS.Tables[1].Rows[0]["host"]).Trim(),
                                          Convert.ToInt32(m_Obj_DS.Tables[1].Rows[0]["port"]),
                                          Convert.ToBoolean(m_Obj_DS.Tables[1].Rows[0]["enablessl"]),
                                          tomailaddress,
                                          Convert.ToString(m_Obj_DR["cc"]).Trim(),
                                          Convert.ToString(m_Obj_DR["bcc"]).Trim(),
                                          Convert.ToString(m_Obj_DR["body"]).Trim(),
                                          Convert.ToString(m_Obj_DR["subject"]).Trim(),
                                          Convert.ToString(m_Obj_DS.Tables[1].Rows[0]["username"]).Trim(),
                                          Convert.ToString(m_Obj_DS.Tables[1].Rows[0]["password"]).Trim(),
                                          Convert.ToBoolean(m_Obj_DR["removefiles"]),
                                          mainCursPrgsValue,
                                          ref Barvalue,
                                          outIDString,
                                          Convert.ToBoolean(m_Obj_DR["emaillogfiles"]),
                                          Convert.ToString(m_Obj_DR["logemailid"]).Trim(),
                                          Convert.ToString(m_Obj_DR["spWhat"]).Trim(),      // Added by Sachin N. S. on 21/01/2014 for Bug-20211
                                          Convert.ToString(m_Obj_DR["sqlQuery"]).Trim(),    // Added by Sachin N. S. on 21/01/2014 for Bug-20211
                                          Convert.ToString(m_Obj_DR["qTable"]).Trim());     // Added by Sachin N. S. on 21/01/2014 for Bug-20211

                                break;
                            case "EXCEL":
                            case "WORD":
                            case "PDF":
                                //var directory = Path.GetDirectoryName(Environment.CurrentDirectory.ToString()) + "\\" + getRptFilePath();
                                var directory = AppDetails.AppPath + "\\" + getRptFilePath();
                                //var directory = "D:\\VUDYOGSDK\\"+getRptFilePath();     // To be Changed by Sachin N. S. on 20/01/2014 for Bug-20211

                                ExportCrystal(Convert.ToString(m_Obj_DR["id"]).Trim(),
                                    //Convert.ToString(m_Obj_DR["rep_nm"]).Trim(),    // Changed by Sachin N. S. on 21/01/2014 for Bug-20211
                                              Convert.ToString(directory + "\\" + m_Obj_DR["Reprep_nm"]).Trim() + ".rpt",   // Changed by Sachin N. S. on 21/01/2014 for Bug-20211
                                              Convert.ToString(m_Obj_DR["exportprefixname"]).Trim(),
                                              Convert.ToString(m_Obj_DR["exportpath"]).Trim(),
                                              Convert.ToString(m_Obj_DR["query"]).Trim(),
                                              Convert.ToString(m_Obj_DR["parameters"]).Trim(),
                                              Convert.ToString(m_Obj_DR["attachment_typ"]).Trim(),
                                    //Convert.ToString(m_Obj_DR["reportquery"]).Trim(),         
                                    //Convert.ToString(m_Obj_DR["reportquerytype"]).Trim(),     
                                              Convert.ToString(m_Obj_DR["sqlQuery"]).Trim(),        // Changed by Sachin N. S. on 21/01/2014 for Bug-20211
                                              Convert.ToString(m_Obj_DR["spWhat"]).Trim(),          // Changed by Sachin N. S. on 21/01/2014 for Bug-20211
                                              Convert.ToString(m_Obj_DS.Tables[1].Rows[0]["host"]).Trim(),
                                              Convert.ToInt32(m_Obj_DS.Tables[1].Rows[0]["port"]),
                                              Convert.ToBoolean(m_Obj_DS.Tables[1].Rows[0]["enablessl"]),
                                              tomailaddress,
                                              Convert.ToString(m_Obj_DR["cc"]).Trim(),
                                              Convert.ToString(m_Obj_DR["bcc"]).Trim(),
                                              Convert.ToString(m_Obj_DR["body"]).Trim(),
                                              Convert.ToString(m_Obj_DR["subject"]).Trim(),
                                              Convert.ToString(m_Obj_DS.Tables[1].Rows[0]["username"]).Trim(),
                                              Convert.ToString(m_Obj_DS.Tables[1].Rows[0]["password"]).Trim(),
                                              Convert.ToBoolean(m_Obj_DR["removefiles"]),
                                              mainCursPrgsValue,
                                              ref Barvalue,
                                              outIDString,
                                              Convert.ToBoolean(m_Obj_DR["emaillogfiles"]),
                                              Convert.ToString(m_Obj_DR["logemailid"]).Trim(),
                                              Convert.ToString(m_Obj_DR["spWhat"]).Trim(),      // Added by Sachin N. S. on 21/01/2014 for Bug-20211
                                              Convert.ToString(m_Obj_DR["sqlQuery"]).Trim(),    // Added by Sachin N. S. on 21/01/2014 for Bug-20211
                                              Convert.ToString(m_Obj_DR["qTable"]).Trim());     // Added by Sachin N. S. on 21/01/2014 for Bug-20211

                                break;

                        }
                    }

                    catch (Exception Ex)
                    {
                        throw Ex;
                    }

                    outIDString = "Process End ID : " + Convert.ToString(m_Obj_DR["id"]).Trim();
                    LogWriter.WriteLogToTextFile(LogFilePath, outIDString, false, false);
                    LogWriter.WriteLogToTextFile(LogFilePath, "-------------------------------------<<< END >>>-----------------------------", true, false);
                    LogWriter.WriteLogToTextFile(LogFilePath, string.Empty, true, true);
                    BgwkProcess.ReportProgress(Barvalue, outIDString);
                }
            }
        }

        #region Other Private Methods
        private void Select()
        {
            try
            {
                if (sqlconn == null)
                {
                    sqlconn = new SqlConnection(connectionString);
                    sqlconn.Open();
                }
                else
                {
                    if (sqlconn.State == ConnectionState.Closed)
                        sqlconn.Open();
                }

                string sqlString = string.Empty;
                if (EmailID != null)
                {
                    if (EmailID.Count > 0)
                    {
                        // ****** Changed by Sachin N. S. on 21/01/2014 for Bug-20211 -- Start ****** //
                        //sqlString = "Select * from emailclient "+
                        //            " where isdeactive = 0 or isdeactive is null " +
                        //                   "and a.id in(";

                        sqlString = "Select a.*, b.spWhat, b.SqlQuery, b.qTable from emailclient a " +
                                    " inner join r_Status b on a.RepGroup=b.[Group] and a.RepDesc=b.[Desc] and a.RepRep_nm=b.Rep_Nm " +
                                    " where (a.isdeactive = 0 or a.isdeactive is null) " +
                                           "and a.id in(";
                        // ****** Changed by Sachin N. S. on 21/01/2014 for Bug-20211 -- End ****** //
                        string idStr = string.Empty;
                        foreach (string emailId in EmailID)
                        {
                            if (idStr != string.Empty)
                                idStr = idStr + ",'" + emailId + "'";
                            else
                                idStr = idStr + "'" + emailId + "'";
                        }

                        sqlString = sqlString + idStr + ") order by a.Id ";
                    }
                }
                else
                {
                    // **** Changed by Sachin N. S. on 21/01/2014 for Bug-20211 -- Start
                    //sqlString = "Select * from emailclient where isdeactive = 0 or isdeactive is null ";
                    sqlString = "Select a.*, b.spWhat, b.SqlQuery, b.qTable from emailclient a " +
                        " inner join r_Status b on a.RepGroup=b.[Group] and a.RepDesc=b.[Desc] and a.RepRep_nm=b.Rep_Nm " +
                        " where (a.isdeactive = 0 or a.isdeactive is null) Order by A.Id ";
                    // **** Changed by Sachin N. S. on 21/01/2014 for Bug-20211 -- End
                }

                sqlString = sqlString + " Select * from emailsettings ";

                m_Obj_DS = cls_Sqlhelper.ExecuteDataset(sqlconn, CommandType.Text, sqlString);
            }
            catch (Exception Ex)
            {
                throw new Exception("found error while fetching records from emailclient table \n" +
                                    "Message : " + Ex.Message +
                                    "\nSource : " + Ex.Source +
                                    "\nTargetSite : " + Ex.TargetSite);
            }
            finally { sqlconn.Close(); }
        }



        private void ExportCSV(string id,
                               string csvPath,
                               string csvPrefixName,
                               string parameters,
                               bool hasAttachment,
                               string sqlcommand,
                               string reportcommand,
                               string reportcommandtype,
                               string separator,
                               string encoding,
                               bool isFirstRow,
                               string smtphost,
                               int smtpport,
                               bool smtpenablessl,
                               string tomailaddress,
                               string ccmailaddress,
                               string bccmailaddress,
                               string mailbody,
                               string mailsubject,
                               string mailid,
                               string mailpasswd,
                               bool removefiles,
                               decimal mainPrgsValue,
                               ref int BarValue,
                               string outIDString,
                               bool emaillogfiles,
                               string logemailid,
                               string spwhat,       // Added by Sachin N. S. on 21/01/2014 for Bug-20211
                               string SqlQuery,     // Added by Sachin N. S. on 21/01/2014 for Bug-20211
                               string qTable)       // Added by Sachin N. S. on 21/01/2014 for Bug-20211
        {

            DataSet m_MainDs = new DataSet();

            bool InvPrint = true;       // Added by Sachin N. S. on 17/01/2014 for Bug-20211

            try
            {
                if (sqlconn == null)
                {
                    sqlconn = new SqlConnection(connectionString);
                    sqlconn.Open();
                }
                else
                {
                    if (sqlconn.State == ConnectionState.Closed)
                        sqlconn.Open();
                }

                //***** Added by Sachin N. S. on 16-01-2014 for Bug-20211 -- Start *****//
                string sqlQuery = "";
                sqlQuery = "SET DATEFORMAT DMY EXEC USP_ENT_AUTOEMAIL_FILTER " + sqlcommand;
                m_MainDs = cls_Sqlhelper.ExecuteDataset(sqlconn, CommandType.Text, sqlQuery);
                //***** Added by Sachin N. S. on 16-01-2014 for Bug-20211 -- End *****//
                //m_MainDs = cls_Sqlhelper.ExecuteDataset(sqlconn, CommandType.Text, sqlcommand);
            }
            catch (Exception Ex)
            {
                throw new Exception("found error while executing main sql query \n" +
                                    "Message : " + Ex.Message +
                                    "\nSource : " + Ex.Source +
                                    "\nTargetSite : " + Ex.TargetSite);
            }
            finally { sqlconn.Close(); }

                        int totRec = 0, totExportFiles = 0, totMailSent = 0;
            string outString = string.Empty;
            if (m_MainDs.Tables[0].Rows.Count == 0)
            {
                BgwkProcess.ReportProgress(BarValue, "No records found for sending mail.");
                LogWriter.WriteLogToTextFile(LogFilePath, "No records found for sending mail.", false, false); // if successful create export file then write in log file
            }
            else
            {

                decimal innerProcessPrgsValue = mainPrgsValue / (m_MainDs.Tables[0].Rows.Count > 0 ? m_MainDs.Tables[0].Rows.Count : 1);

                string m_tomailaddress = string.Empty;

                foreach (DataRow m_MainDR in m_MainDs.Tables[0].Rows)
                {
                    try
                    {
                        totRec++;


                        // Find the Parameter details for log
                        string uid = GetUniqueID(csvPrefixName,
                                       m_MainDR);
                        //End 

                        LogWriter.WriteLogToTextFile(LogFilePath, uid, false, false); // Write unique Id in the Logfile
                        outString = outIDString + " - " + uid;
                        BgwkProcess.ReportProgress(BarValue, outString + " - Exporting file in CSV format");
                        if (BgwkProcess.CancellationPending)
                        {
                            BgwkProcess.ReportProgress(0, "^ Processing has been cancelled...");
                            break;
                        }

                        // If tomailaddress found empty then fetch the email details from Account master
                        if (tomailaddress == string.Empty)
                        {
                            m_tomailaddress = string.Empty;
                            m_tomailaddress = GetEmailDetailsFromTable(Convert.ToInt32(m_MainDR["ac_id"]), Convert.ToString(m_MainDR["Party_nm"]), ref BarValue, outString, m_tomailaddress);
                        }
                        else
                            m_tomailaddress = tomailaddress;
                        // end

                        //BgwkProcess.ReportProgress(BarValue, outString + " - Exporting file in CSV format");
                        if (BgwkProcess.CancellationPending)
                        {
                            BgwkProcess.ReportProgress(0, "^ Processing has been cancelled...");
                            break;
                        }


                        // Set Encoding
                        switch (encoding.Trim().ToUpper())
                        {
                            case "UNICODE":
                                m_Obj_Report.EncodingCSV = Encoding.Unicode;
                                break;
                            case "ASCII":
                                m_Obj_Report.EncodingCSV = Encoding.ASCII;
                                break;
                            case "UTF7":
                                m_Obj_Report.EncodingCSV = Encoding.UTF7;
                                break;
                            case "UTF8":
                                m_Obj_Report.EncodingCSV = Encoding.UTF8;
                                break;
                            case "UTF32":
                                m_Obj_Report.EncodingCSV = Encoding.UTF32;
                                break;
                        }

                        bool isSuccess = false;
                        string cParameter = "";     // Added by Sachin N. S. on 17/01/2014 for Bug-20211

                        // Generate Filename Prefix

                        m_Obj_Report.DskFileName = csvPath;
                        m_Obj_Report.Sqlstring = reportcommand;
                        m_Obj_Report.DskFilePrefixName = EvalPrefixName(csvPrefixName, m_MainDR);
                        //m_Obj_Report.SqlParam = EvalSqlParameters(parameters, m_MainDR);  // Generate SQL Parameters      // Commented by Sachin N. S. on 23/01/2014 for Bug-20211
                        //m_Obj_Report.Sqltype = reportcommandtype;                         // Commented by Sachin N. S. on 23/01/2014 for Bug-20211

                        if (InvPrint == false)      // Added by Sachin N. S. on 17/01/2014 for Bug-20211
                        {
                            m_Obj_Report.Sqlstring = reportcommand;
                            m_Obj_Report.SqlParam = EvalSqlParameters(parameters, m_MainDR);  // Generate SQL Parameters
                        }
                        else        // Added by Sachin N. S. on 17/01/2014 for Bug-20211 -- Start
                        {
                            cParameter = EvalSqlParameters(m_MainDR, qTable);
                            m_Obj_Report.Sqlstring = getReportQuery(reportcommand, cParameter);
                        }           // Added by Sachin N. S. on 17/01/2014 for Bug-20211 -- End

                        if (InvPrint == false)      // Added by Sachin N. S. on 17/01/2014 for Bug-20211
                        {
                            m_Obj_Report.Sqltype = reportcommandtype;     // To be Changed afterwards by Sachin N. S. on 20/01/2014 for Bug-20211
                        }
                        else
                            m_Obj_Report.Sqltype = "Q";         // Changed by Sachin N. S. on 20/01/2014 for Bug-20211

                        m_Obj_Report.Separator = separator;
                        m_Obj_Report.FirstRowColumnNames = isFirstRow;
                        isSuccess = m_Obj_Report.ExportCsvFile();

                        if (isSuccess == true)
                        {
                            totExportFiles++;

                            LogWriter.WriteLogToTextFile(LogFilePath, " - Successfully Exported file in CSV format....", false, false); // if successful create export file then write in log file

                            BgwkProcess.ReportProgress(BarValue, outString + " - Configuring mail...");
                            if (BgwkProcess.CancellationPending)
                            {
                                BgwkProcess.ReportProgress(0, "^ Processing has been cancelled...");
                                break;
                            }

                            isSuccess = false;
                            m_Obj_MailSender = new cls_Gen_Mgr_MailSender();
                            m_Obj_MailSender.ToMailAddress = m_tomailaddress;
                            m_Obj_MailSender.CcMailAddress = ccmailaddress;
                            m_Obj_MailSender.BccMailAddress = bccmailaddress;
                            m_Obj_MailSender.Body = mailbody;
                            //m_Obj_MailSender.Subject = mailsubject;
                            m_Obj_MailSender.Subject = EvalSubject(mailsubject, m_MainDR);
                            m_Obj_MailSender.AttachmentPath = m_Obj_Report.DskFileName;

                            m_Obj_MailSender.SmtpHost = smtphost;
                            m_Obj_MailSender.SmtpPort = smtpport;
                            m_Obj_MailSender.SmtpEnableSSL = smtpenablessl;
                            m_Obj_MailSender.MailId = mailid;
                            m_Obj_MailSender.MailPass = m_Obj_MailSender.DecryptData(mailpasswd);
                            m_Obj_MailSender.FromMailAddress = mailid;
                            m_Obj_MailSender.ConfigMail();
                            BgwkProcess.ReportProgress(BarValue, outString + " - Sending mail...");
                            isSuccess = m_Obj_MailSender.Send();

                            if (isSuccess == true)
                            {
                                //Move sent mail attachment in sentbox
                                //MoveInSentBoxDirectory(m_Obj_MailSender.AttachmentPath, ApplicationPath + "\\Sent Mail\\", Path.GetFileName(m_Obj_MailSender.AttachmentPath));
                                MoveInSentBoxDirectory(m_Obj_MailSender.AttachmentPath, CompanyPath + "\\Sent Mail\\", Path.GetFileName(m_Obj_MailSender.AttachmentPath));      // Changed by Sachin N. S. on 31/01/2014 for Bug-20211
                                // if successful sent mail then write in log file
                                BgwkProcess.ReportProgress(BarValue, outString + " - Successfully Mail Sent...");
                                LogWriter.WriteLogToTextFile(LogFilePath, " - Successfully Mail Sent.... ", false, false);
                                LogEmailDelete(id.ToString().Trim(),
                                               Path.GetFileName(m_Obj_MailSender.AttachmentPath).Trim());
                                totMailSent++;
                            }
                        }
                    }
                    catch (SmtpException SmtpEx)
                    {
                        BgwkProcess.ReportProgress(BarValue, outString + " - Mail has not been sent,see the log file for the issue...");
                        LogWriter.WriteLogToTextFile(LogFilePath, "Mail has not been sent..." + SmtpEx.Message.Trim(), false, false); // if successful create export file then write in log file
                        //If Error found while sending mail then log file details will be inserted in eMailLog table
                        LogEmailWriter(id,
                                        m_tomailaddress,
                                        bccmailaddress,
                                        ccmailaddress,
                                        mailsubject,
                                        mailbody,
                                        csvPath,
                                        Path.GetFileName(m_Obj_Report.DskFileName),
                                        removefiles,
                                        "Pending",
                                        SmtpEx.Message.Trim(),
                                        emaillogfiles,
                                        logemailid);
                    }
                    catch (Exception Ex)
                    {
                        BgwkProcess.ReportProgress(BarValue, outString + " - Error found, see log file for the issue...");
                        LogWriter.WriteLogToTextFile(LogFilePath, "Error Found..." + Ex.Message.Trim(), false, false); // if successful create export file then write in log file
                    }
                    //catch (Exception Ex)
                    //{
                    //     throw new Exception("Error found, while calling method \n" +
                    //                    "Message : " + Ex.Message +
                    //                    "\nSource : " + Ex.Source +
                    //                    "\nTargetSite : " + Ex.TargetSite);
                    //}

                    LogWriter.WriteLogToTextFile(LogFilePath, "------------------------------------------------------------------------------------------", true, false);
                    BarValue = BarValue + (Int32)innerProcessPrgsValue;
                }
            }
            // Remove all exported files
            RemoveExportedFiles(removefiles,
                                BarValue,
                                outString,
                                m_MainDs,
                                "CSV",
                                //ApplicationPath + "\\Sent Mail\\",    // Changed by Sachin N. S. on 31/01/2014 for Bug-20211
                                CompanyPath + "\\Sent Mail\\",
                                csvPrefixName,
                                string.Empty);

            //  End

            LogWriter.WriteLogToTextFile(LogFilePath, "-------------------------------------<<< Summary Start >>>-----------------------------", true, false);
            LogWriter.WriteLogToTextFile(LogFilePath, "Total Records found : " + Convert.ToString(totRec).Trim(), true, false);
            LogWriter.WriteLogToTextFile(LogFilePath, "Total Exported CSV files : " + Convert.ToString(totExportFiles).Trim(), true, false);
            LogWriter.WriteLogToTextFile(LogFilePath, "Total Mails sent : " + Convert.ToString(totMailSent).Trim(), true, false);
            string logfilename = LogWriter.WriteLogToTextFile(LogFilePath, "---------------------------------------<<< Summary End >>> ------------------------------", true, false);

            // Sending Log Mail
            SendLogStatus(emaillogfiles,
                          logemailid,
                          logfilename,
                          smtphost,
                          smtpport,
                          smtpenablessl,
                          mailid,
                          m_Obj_MailSender.DecryptData(mailpasswd),
                          BarValue,
                          outString);
            // End

            // Pass LogStatus File to Property
            LogStatusFileName = logfilename;
        }

        private void ExportCrystal(string id,
                                   string reportPath,
                                   string outPrefixName,
                                   string exportpath,
                                   string sqlcommand,
                                   string parameters,
                                   string exportformattype,
                                   string reportcommand,
                                   string reportcommandtype,
                                   string smtphost,
                                   int smtpport,
                                   bool smtpenablessl,
                                   string tomailaddress,
                                   string ccmailaddress,
                                   string bccmailaddress,
                                   string mailbody,
                                   string mailsubject,
                                   string mailid,
                                   string mailpasswd,
                                   bool removefiles,
                                   decimal mainPrgsValue,
                                   ref int BarValue,
                                   string outIDString,
                                   bool emaillogfiles,
                                   string logemailid,
                                   string spwhat,       // Added by Sachin N. S. on 21/01/2014 for Bug-20211
                                   string SqlQuery,     // Added by Sachin N. S. on 21/01/2014 for Bug-20211
                                   string qTable)       // Added by Sachin N. S. on 21/01/2014 for Bug-20211
        {
            DataSet m_MainDs = new DataSet();

            bool InvPrint = true;       // Added by Sachin N. S. on 17/01/2014 for Bug-20211

            try
            {
                if (sqlconn == null)
                {
                    sqlconn = new SqlConnection(connectionString);
                    sqlconn.Open();
                }
                else
                {
                    if (sqlconn.State == ConnectionState.Closed)
                        sqlconn.Open();
                }

                //***** Added by Sachin N. S. on 16-01-2014 for Bug-20211 -- Start *****//
                string sqlQuery = "";
                sqlQuery = "SET DATEFORMAT DMY EXEC USP_ENT_AUTOEMAIL_FILTER " + sqlcommand;
                //***** Added by Sachin N. S. on 16-01-2014 for Bug-20211 -- End *****//

                m_MainDs = cls_Sqlhelper.ExecuteDataset(sqlconn, CommandType.Text, sqlQuery);

            }
            catch (Exception Ex)
            {
                //throw new Exception("found error in execute main query \n" +
                //                                        "Message : " + Ex.Message +
                //                                        "\nSource : " + Ex.Source +
                //                                        "\nTargetSite : " + Ex.TargetSite);

                BgwkProcess.ReportProgress(BarValue, " Found error in execute main query...");
                LogWriter.WriteLogToTextFile(LogFilePath, "Error Found..." + Ex.Message.Trim(), false, false); // if successful create export file then write in log file
            }
            finally { sqlconn.Close(); }

            string outString = string.Empty;
            int totRec = 0, totExportFiles = 0, totMailSent = 0;
            if (m_MainDs.Tables[0].Rows.Count == 0)
            {
                BgwkProcess.ReportProgress(BarValue, "No records found for sending mail.");
                LogWriter.WriteLogToTextFile(LogFilePath, "No records found for sending mail.", false, false); // if successful create export file then write in log file
            }
            else
            {
                decimal innerProcessPrgsValue = mainPrgsValue / (m_MainDs.Tables[0].Rows.Count > 0 ? m_MainDs.Tables[0].Rows.Count : 1);
                string m_tomailaddress = string.Empty;
                foreach (DataRow m_MainDR in m_MainDs.Tables[0].Rows)
                {

                    try
                    {
                        totRec++;

                        // Find the Parameter details for log
                        string uid = GetUniqueID(outPrefixName,
                                       m_MainDR);
                        //End 

                        LogWriter.WriteLogToTextFile(LogFilePath, uid, false, false); // Write unique Id in the Logfile
                        outString = outIDString + " - " + uid;
                        BgwkProcess.ReportProgress(BarValue, outString + " - Exporting file in " + exportformattype.Trim() + " format");
                        if (BgwkProcess.CancellationPending)
                        {
                            BgwkProcess.ReportProgress(0, "^ Processing has been cancelled...");
                            break;
                        }

                        // If tomailaddress found empty then fetch the email details from Account master

                        if (tomailaddress == string.Empty)
                        {
                            m_tomailaddress = string.Empty;
                            m_tomailaddress = GetEmailDetailsFromTable(Convert.ToInt32(m_MainDR["ac_id"]), Convert.ToString(m_MainDR["Party_nm"]), ref BarValue, outString, m_tomailaddress);
                        }
                        else
                            m_tomailaddress = tomailaddress;

                        // end

                        bool isSuccess = false;
                        string cParameter = "";     // Added by Sachin N. S. on 17/01/2014 for Bug-20211

                        m_Obj_Report.DskFileName = exportpath;
                        m_Obj_Report.DskFilePrefixName = EvalPrefixName(outPrefixName, m_MainDR);  // Generate Filename Prefix
                        m_Obj_Report.ReportPath = reportPath;
                        m_Obj_Report.ReportExportType = exportformattype;

                        if (InvPrint == false)      // Added by Sachin N. S. on 17/01/2014 for Bug-20211
                        {
                            m_Obj_Report.Sqlstring = reportcommand;
                            m_Obj_Report.SqlParam = EvalSqlParameters(parameters, m_MainDR);  // Generate SQL Parameters
                        }
                        else        // Added by Sachin N. S. on 17/01/2014 for Bug-20211 -- Start
                        {
                            cParameter = EvalSqlParameters(m_MainDR, qTable);
                            m_Obj_Report.Sqlstring = getReportQuery(reportcommand, cParameter);
                        }           // Added by Sachin N. S. on 17/01/2014 for Bug-20211 -- End

                        if (InvPrint == false)      // Added by Sachin N. S. on 17/01/2014 for Bug-20211
                        {
                            m_Obj_Report.Sqltype = reportcommandtype;     // To be Changed afterwards by Sachin N. S. on 20/01/2014 for Bug-20211
                        }
                        else
                            m_Obj_Report.Sqltype = "Q";         // Changed by Sachin N. S. on 20/01/2014 for Bug-20211

                        isSuccess = m_Obj_Report.ExportCrystalReport();

                        BgwkProcess.ReportProgress(BarValue, outString + " - Successfully Exported file in " + exportformattype.Trim() + " format...");
                        if (BgwkProcess.CancellationPending)
                        {
                            BgwkProcess.ReportProgress(0, "^ Processing has been cancelled...");
                            break;
                        }

                        if (isSuccess == true)
                        {
                            totExportFiles++;

                            LogWriter.WriteLogToTextFile(LogFilePath, "Successfully Exported file in " + exportformattype + " format....", false, false); // if successful create export file then write in log file

                            BgwkProcess.ReportProgress(BarValue, outString + " - Configuring mail...");
                            if (BgwkProcess.CancellationPending)
                            {
                                BgwkProcess.ReportProgress(0, "^ Processing has been cancelled...");
                                break;
                            }

                            isSuccess = false;

                            m_Obj_MailSender = new cls_Gen_Mgr_MailSender();
                            m_Obj_MailSender.ToMailAddress = m_tomailaddress;
                            m_Obj_MailSender.CcMailAddress = ccmailaddress;
                            m_Obj_MailSender.BccMailAddress = bccmailaddress;
                            m_Obj_MailSender.Body = mailbody;
                            //m_Obj_MailSender.Subject = mailsubject;
                            m_Obj_MailSender.Subject = EvalSubject(mailsubject, m_MainDR);
                            m_Obj_MailSender.AttachmentPath = m_Obj_Report.DskFileName;

                            m_Obj_MailSender.SmtpHost = smtphost;
                            m_Obj_MailSender.SmtpPort = smtpport;
                            m_Obj_MailSender.SmtpEnableSSL = smtpenablessl;
                            m_Obj_MailSender.MailId = mailid;
                            m_Obj_MailSender.MailPass = m_Obj_MailSender.DecryptData(mailpasswd);
                            m_Obj_MailSender.FromMailAddress = mailid;
                            m_Obj_MailSender.ConfigMail();
                            BgwkProcess.ReportProgress(BarValue, outString + " - Sending mail...");
                            isSuccess = m_Obj_MailSender.Send();
                            if (isSuccess == true)
                            {
                                //Move sent mail attachment in sentbox
                                //MoveInSentBoxDirectory(m_Obj_MailSender.AttachmentPath, ApplicationPath + "\\Sent Mail\\", Path.GetFileName(m_Obj_MailSender.AttachmentPath));
                                MoveInSentBoxDirectory(m_Obj_MailSender.AttachmentPath, CompanyPath + "\\Sent Mail\\", Path.GetFileName(m_Obj_MailSender.AttachmentPath));      // Changed by Sachin N. S. on 31/01/2014 for Bug-20211
                                BgwkProcess.ReportProgress(BarValue, outString + " - Successfully Mail Sent...");
                                LogWriter.WriteLogToTextFile(LogFilePath, " - Successfully Mail Sent.... ", false, false);
                                LogEmailDelete(id.ToString().Trim(),
                                               Path.GetFileName(m_Obj_MailSender.AttachmentPath).Trim());
                                totMailSent++;
                            }
                            else
                            {
                                if (isSuccess == false)
                                {
                                    BgwkProcess.ReportProgress(BarValue, outString + " - Mail has not been sent,see the log file for the issue...");
                                    LogWriter.WriteLogToTextFile(LogFilePath, " - Mail has not been sent..", false, false);
                                }
                            }

                        }
                        else
                        {
                            if (isSuccess == false)
                            {
                                BgwkProcess.ReportProgress(BarValue, outString + " - Export file has not been created, see log the file for the issue...");
                                LogWriter.WriteLogToTextFile(LogFilePath, " - Export file has not been created..", false, false);
                            }
                        }
                    }

                    catch (SmtpException SmtpEx)
                    {
                        BgwkProcess.ReportProgress(BarValue, outString + " - Mail has not been sent,see the log file for the issue...");
                        LogWriter.WriteLogToTextFile(LogFilePath, "Mail has not been sent..." + SmtpEx.Message.Trim(), false, false); // if successful create export file then write in log file
                        //If Error found while sending mail then log file details will be inserted in eMailLog table
                        LogEmailWriter(id,
                                        m_tomailaddress,
                                        bccmailaddress,
                                        ccmailaddress,
                                        mailsubject,
                                        mailbody,
                                        exportpath,
                                        Path.GetFileName(m_Obj_Report.DskFileName),
                                        removefiles,
                                        "Pending",
                                        SmtpEx.Message.Trim(),
                                        emaillogfiles,
                                        logemailid);
                    }
                    catch (Exception Ex)
                    {
                        BgwkProcess.ReportProgress(BarValue, outString + " - Error found, see log file for the issue...");
                        LogWriter.WriteLogToTextFile(LogFilePath, "Error Found..." + Ex.Message.Trim(), false, false); // if successful create export file then write in log file

                    }
                    LogWriter.WriteLogToTextFile(LogFilePath, "---------------------------------------------------------------------------------", true, false);
                    BarValue = BarValue + (Int32)innerProcessPrgsValue;
                }
            }


            // Remove all exported files
            RemoveExportedFiles(removefiles,
                                BarValue,
                                outString,
                                m_MainDs,
                                exportformattype,
                                //ApplicationPath + "\\Sent Mail\\",    // Changed by Sachin N. S. on 31/01/2014 for Bug-20211
                                CompanyPath + "\\Sent Mail\\",
                                outPrefixName,
                                string.Empty);

            //  End

            // Write Final Summary 

            LogWriter.WriteLogToTextFile(LogFilePath, "-------------------------------------<<< Summary >>>-----------------------------", true, false);
            LogWriter.WriteLogToTextFile(LogFilePath, "Total Records found : " + Convert.ToString(totRec).Trim(), true, false);
            LogWriter.WriteLogToTextFile(LogFilePath, "Total Exported " + exportformattype.Trim() + " files : " + Convert.ToString(totExportFiles).Trim(), true, false);
            LogWriter.WriteLogToTextFile(LogFilePath, "Total Mails sent : " + Convert.ToString(totMailSent).Trim(), true, false);
            string logfilename = LogWriter.WriteLogToTextFile(LogFilePath, "---------------------------------------<<< End >>> ------------------------------", true, false);

            // End

            // Sending Log Mail
            SendLogStatus(emaillogfiles,
                          logemailid,
                          logfilename,
                          smtphost,
                          smtpport,
                          smtpenablessl,
                          mailid,
                          m_Obj_MailSender.DecryptData(mailpasswd),
                          BarValue,
                          outString);
            // End

            // Pass LogStatus File to Property
            LogStatusFileName = logfilename;

        }

        private static bool MoveInSentBoxDirectory(string sourceFileName, string destFileName, string filename)
        {
            bool loggingDirectoryExists = false;

            DirectoryInfo dirInfo = new DirectoryInfo(destFileName);

            if (dirInfo.Exists)
            {
                loggingDirectoryExists = true;
                if (File.Exists(destFileName + filename))
                {
                    File.Delete(destFileName + filename);
                }
                File.Move(sourceFileName, destFileName + filename);
            }
            else
            {
                try
                {
                    Directory.CreateDirectory(destFileName);
                    File.Move(sourceFileName, destFileName + filename);
                    loggingDirectoryExists = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return loggingDirectoryExists;
        }

        private void LogEmailWriter(string Id,
                                    string To,
                                    string Bcc,
                                    string Cc,
                                    string Subject,
                                    string Body,
                                    string filepath,
                                    string filename,
                                    bool removefiles,
                                    string Status,
                                    string Remarks,
                                    bool emaillogfiles,
                                    string logemailid)
        {
            m_Obj_Email_LogWriter.Id = Id;
            m_Obj_Email_LogWriter.To = To;
            m_Obj_Email_LogWriter.Bcc = Bcc;
            m_Obj_Email_LogWriter.Cc = Cc;
            m_Obj_Email_LogWriter.Subject = Subject;
            m_Obj_Email_LogWriter.Body = Body;
            m_Obj_Email_LogWriter.Filepath = filepath;
            m_Obj_Email_LogWriter.Filename = filename;
            m_Obj_Email_LogWriter.Removefiles = removefiles;
            m_Obj_Email_LogWriter.Status = Status;
            m_Obj_Email_LogWriter.Remarks = Remarks;
            m_Obj_Email_LogWriter.Emaillogfiles = emaillogfiles;
            m_Obj_Email_LogWriter.Logemailid = logemailid;
            m_Obj_Email_LogWriter.UpdateEmailLog();
        }

        private void LogEmailDelete(string Id,
                                    string filename)
        {
            m_Obj_Email_LogWriter.Id = Id;
            m_Obj_Email_LogWriter.Filename = filename;
            m_Obj_Email_LogWriter.Delete();
        }

        private void RemoveExportedFiles(bool removefiles,
                                        int BarValue,
                                        string outString,
                                        DataSet m_MainDs,
                                        string exportformattype,
                                        string exportpath,
                                        string outPrefixName,
                                        string filename)
        {
            if (removefiles == true)
            {
                BgwkProcess.ReportProgress(BarValue, outString + " - Deleting exported files...");
                try
                {
                    string outextension = string.Empty;
                    string exportfilename = string.Empty;
                    foreach (DataRow m_MainDR in m_MainDs.Tables[0].Rows)
                    {
                        switch (exportformattype.Trim().ToUpper())
                        {
                            case "PDF":
                                outextension = ".PDF";
                                break;
                            case "EXCEL":
                                outextension = ".XLS";
                                break;
                            case "WORD":
                                outextension = ".DOC";
                                break;
                            case "CSV":
                                outextension = ".CSV";
                                break;
                        }

                        // Generate File name according current Date
                        if (filename == string.Empty)
                        {
                            exportfilename = exportpath.Trim() + EvalPrefixName(outPrefixName, m_MainDR).Trim() +
                                         DateTime.Now.ToString("yyyy_MM_dd") + outextension.Trim();
                        }
                        else
                        {
                            exportfilename = filename.Trim().ToString();
                        }
                        if (File.Exists(exportfilename) == true)
                        {
                            File.Delete(exportfilename);
                            LogWriter.WriteLogToTextFile(LogFilePath, " - Successfully deleted " + Path.GetFileName(exportfilename).ToString().Trim() + " Exported file...", false, false);
                            BgwkProcess.ReportProgress(BarValue, outString + " - Deleted " + Path.GetFileName(exportfilename).ToString().Trim() + " Exported file...");
                        }
                        else
                        {
                            LogWriter.WriteLogToTextFile(LogFilePath, " - File " + Path.GetFileName(exportfilename).ToString().Trim() + " does not found for deleting...", false, false);
                            BgwkProcess.ReportProgress(BarValue, outString + " - File " + Path.GetFileName(exportfilename).ToString().Trim() + " does not exists...");
                        }
                    }
                }
                catch (Exception Ex)
                {
                    LogWriter.WriteLogToTextFile(LogFilePath, "Error Found..." + Ex.Message.Trim(), false, false);
                }
            }
        }

        private string EvalPrefixName(string FileName,
                                      DataRow dr)
        {
            string cFileName = "";
            if (FileName.Trim().IndexOf(",") >= 0)
            {
                string[] aFileName = FileName.Split(',');
                int i = 0;
                foreach (string str in aFileName)
                {
                    if (i == 0 || str.Contains('"')==true)
                        cFileName = cFileName + cFileName!="" ? "_" : "" + str.Trim().Replace('"'.ToString(), "");
                    else
                    {
                        cFileName = cFileName + "_" + Convert.ToString(dr[str]).Trim();
                    }
                    i++;
                }
            }

            return cFileName + "_";

        }

        private string EvalSubject(string subject,
                                      DataRow dr)
        {
            string s = "", sub = "";
            if (subject.Trim().IndexOf("/") >= 0)
            {
                string[] aFileName = subject.Split('/');
                foreach (string str in aFileName)
                {
                    if (str.StartsWith("@"))
                    {
                        s = str.Remove(0, 1);
                        if (dr.Table.Columns[s].DataType.Name == "DateTime")
                        {
                            DateTime d = Convert.ToDateTime(dr[s]);
                            sub = !string.IsNullOrEmpty(sub)
                            ? sub + " " + d.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture).Trim()
                            : d.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture).Trim();
                        }
                        else
                        {
                            sub = !string.IsNullOrEmpty(sub)
                                ? sub + " " + Convert.ToString(dr[s]).Trim()
                                : Convert.ToString(dr[s]).Trim();
                        }
                    }
                    else
                    {
                        sub = !string.IsNullOrEmpty(sub)
                            ? sub + " " + str.Trim()
                            : str.Trim();
                    }
                }
            }

            return sub;

        }

        private SqlParameter[] EvalSqlParameters(string parameters,
                                               DataRow dr)
        {
            // Generate SQL Parameters
            string[] sqlParam = parameters.Split(',');
            SqlParameter[] spParam = new SqlParameter[sqlParam.Length];
            int x = 0;
            foreach (string str in sqlParam)
            {
                spParam[x] = new SqlParameter(str, dr[str.Trim()]);
                x++;
            }

            return spParam;
        }

        //****** Added by Sachin N. S. on 17/01/2014 for Bug-20211 -- Start ******//
        private string EvalSqlParameters(DataRow dr, string qTable)
        {
            string spParam = "";
            spParam = " " + qTable.Trim() + ".Entry_ty = '" + dr["Entry_ty"].ToString() + "' and " + qTable.Trim() + ".Tran_Cd = " + dr["Tran_Cd"].ToString();
            return spParam;
        }
        //****** Added by Sachin N. S. on 17/01/2014 for Bug-20211 -- End ******//

        private string GetUniqueID(string outPrefixName,
                                   DataRow dr)
        {
            string uid = string.Empty;
            if (outPrefixName.Trim().IndexOf(",") >= 0)
            {
                string[] aFileName = outPrefixName.Split(',');
                int i = 0;
                foreach (string str in aFileName)
                {
                    if (i > 0 && str.Contains('"')==false)
                    {
                        uid = str.Trim() + " : " + Convert.ToString(dr[str]).Trim();
                    }
                    i++;
                }
            }

            return uid;
        }

        private string GetEmailDetailsFromTable(int acid, string acname,
                                             ref int BarValue,
                                             string outString,
                                             string tomailaddress)
        {
            try
            {
                if (sqlconn == null)
                {
                    sqlconn = new SqlConnection(connectionString);
                    sqlconn.Open();
                }
                else
                {
                    if (sqlconn.State == ConnectionState.Closed)
                        sqlconn.Open();
                }

                BgwkProcess.ReportProgress(BarValue, outString + " - Getting Email Details from Account Master");

                SqlParameter[] m_spParam = { new SqlParameter("@acid", acid),
                                               new SqlParameter("@acname", acname)};

                DataSet m_EmailDS = cls_Sqlhelper.ExecuteDataset(sqlconn, CommandType.StoredProcedure, "USP_GetEmailDetails", m_spParam);

                if (m_EmailDS.Tables[0].Rows.Count > 0)
                    tomailaddress = Convert.ToString(m_EmailDS.Tables[0].Rows[0]["Email"]).Trim();
                else
                    throw new Exception("Account ID.: " + Convert.ToString(acid).Trim() + " not found in Account Master...");

                m_EmailDS.Dispose();

                if (tomailaddress == string.Empty)
                    throw new Exception("To Mail address found empty...");

            }
            catch (Exception Ex)
            {
                throw new Exception("found error while getting emails details \n" +
                                    "Message : " + Ex.Message +
                                    "\nSource : " + Ex.Source +
                                    "\nTargetSite : " + Ex.TargetSite);
            }
            finally { sqlconn.Close(); }

            return tomailaddress;
        }

        private void CheckSMTPCredentials(DataRow dr)
        {
            if (Convert.ToString(dr["host"]).Trim() == string.Empty)
                throw new Exception("SMTP Host found empty...");

            if (Convert.ToInt32(dr["port"]) == 0)
                throw new Exception("SMTP Port found empty...");

            if (Convert.ToString(dr["username"]).Trim() == string.Empty)
                throw new Exception("SMTP Username found empty...");

            if (Convert.ToString(dr["password"]).Trim() == string.Empty)
                throw new Exception("SMTP Password found empty...");

        }

        private void SendLogStatus(bool emaillogfiles,
                                   string logemailid,
                                   string logfilename,
                                   string smtphost,
                                   int smtpport,
                                   bool smtpenablessl,
                                   string mailid,
                                   string mailpasswd,
                                   int BarValue,
                                   string outString)
        {
            if (emaillogfiles == true)
            {
                if (logemailid != string.Empty)
                {
                    try
                    {
                        m_Obj_MailSender = new cls_Gen_Mgr_MailSender();

                        m_Obj_MailSender.ToMailAddress = logemailid;
                        m_Obj_MailSender.Body = "Dear Sir/Madam,\n\nPlease find " + AppDetails.Apptitle + " Email status log file, \nfor your reference.\nAny issue found please contact " + AppDetails.Apptitle + " team. \n\nThank you\nRegards\n" + AppDetails.Apptitle + " Team";
                        m_Obj_MailSender.Subject = AppDetails.Apptitle + " Email Status Log file...";
                        m_Obj_MailSender.AttachmentPath = logfilename;

                        m_Obj_MailSender.SmtpHost = smtphost;
                        m_Obj_MailSender.SmtpPort = smtpport;
                        m_Obj_MailSender.SmtpEnableSSL = smtpenablessl;
                        m_Obj_MailSender.MailId = mailid;
                        m_Obj_MailSender.MailPass = mailpasswd;
                        m_Obj_MailSender.FromMailAddress = mailid;
                        m_Obj_MailSender.ConfigMail();

                        BgwkProcess.ReportProgress(BarValue, outString + " - Sending Email Log Status mail...");

                        if (m_Obj_MailSender.Send() == true)
                        {
                            BgwkProcess.ReportProgress(BarValue, outString + " Successfully Email Log Status Mail Sent...");
                            LogWriter.WriteLogToTextFile(LogFilePath, "Successfully Email Log Status Mail Sent.... ", false, false);
                        }
                        else
                        {
                            BgwkProcess.ReportProgress(BarValue, outString + " Mail has not been sent, see the log file for the issue..");
                            LogWriter.WriteLogToTextFile(LogFilePath, "Mail has not been sent.... ", false, false);
                        }
                    }
                    catch (Exception Ex)
                    {
                        LogWriter.WriteLogToTextFile(LogFilePath, "Error Found..." + Ex.Message.Trim(), false, false);
                    }
                }
            }

        }

        private void SelectPending()
        {
            try
            {
                if (sqlconn == null)
                {
                    sqlconn = new SqlConnection(connectionString);
                    sqlconn.Open();
                }
                else
                {
                    if (sqlconn.State == ConnectionState.Closed)
                        sqlconn.Open();
                }

                string sqlString = string.Empty;
                if (LogEmailID != null)
                {
                    if (LogEmailID.Count > 0)
                    {
                        sqlString = "Select * from emaillog where status = 'Pending' " +
                                           "and autoid in(";
                        string idStr = string.Empty;
                        foreach (string emailId in LogEmailID)
                        {
                            if (idStr != string.Empty)
                                idStr = idStr + ",'" + emailId + "'";
                            else
                                idStr = idStr + "'" + emailId + "'";
                        }

                        sqlString = sqlString + idStr + ")";
                    }
                }
                else
                    sqlString = "Select * from emaillog where status = 'Pending' ";

                sqlString = sqlString + " Select * from emailsettings ";

                m_Obj_logDs = new DataSet();
                m_Obj_logDs = cls_Sqlhelper.ExecuteDataset(sqlconn, CommandType.Text, sqlString);
            }
            catch (Exception Ex)
            {
                throw new Exception("found error while fetching records from emailclient table \n" +
                                    "Message : " + Ex.Message +
                                    "\nSource : " + Ex.Source +
                                    "\nTargetSite : " + Ex.TargetSite);
            }
            finally { sqlconn.Close(); }
        }

        private void ExecutePendingProcess()
        {
            Int32 Barvalue = 2;
            try
            {
                // For Main Process
                BgwkProcess.ReportProgress(Barvalue, "Pending Process Started...");
                SelectPending(); // Call Main Query..
            }
            catch (Exception Ex)
            {
                throw new Exception("Error found in the ExecuteProcess Method  \n" +
                                    "Message : " + Ex.Message +
                                    "\nSource : " + Ex.Source +
                                    "\nTargetSite : " + Ex.TargetSite);
            }
            if (m_Obj_logDs.Tables[0].Rows.Count == 0)
            {
                LogWriter.WriteLogToTextFile(LogFilePath, "Pending Process Started...", false, false);
                LogWriter.WriteLogToTextFile(LogFilePath, "Pending Mails not found...", false, false);
                BgwkProcess.ReportProgress(Barvalue, "Pending Mails not found...");
                BgwkProcess.ReportProgress(Barvalue, "Pending Process Ended...");
                BgwkProcess.ReportProgress(Barvalue, string.Empty);
                return;
            }

            string logString = "Pending Process Started...";
            string outIDString = string.Empty;
            LogWriter.WriteLogToTextFile(LogFilePath, "-------------------------------------<<< START >>>-----------------------------", true, false);
            LogWriter.WriteLogToTextFile(LogFilePath, logString, false, false);
            decimal mainCursPrgsValue = 98.00m / (Convert.ToInt32(m_Obj_logDs.Tables[0].Rows.Count) > 0 ? Convert.ToInt32(m_Obj_logDs.Tables[0].Rows.Count) : 1);

            // Check SMTP Settings

            DataRow smtpDR = m_Obj_logDs.Tables[1].Rows[0];
            if (smtpDR == null)
                throw new Exception("SMTP Setting table found empty...");

            CheckSMTPCredentials(smtpDR);

            bool isSuccess = false;
            int totRec = 0, totMailSent = 0;
            decimal innerProcessPrgsValue = mainCursPrgsValue / (m_Obj_logDs.Tables[0].Rows.Count > 0 ? m_Obj_logDs.Tables[0].Rows.Count : 1);

            foreach (DataRow m_Obj_DR in m_Obj_logDs.Tables[0].Rows)
            {
                try
                {
                    totRec++;
                    outIDString = "ID : " + Convert.ToString(m_Obj_DR["id"]).Trim();
                    LogWriter.WriteLogToTextFile(LogFilePath, "Process Start " + outIDString, false, false);
                    BgwkProcess.ReportProgress(Barvalue, "Process Start " + outIDString);

                    if (BgwkProcess.CancellationPending)
                    {
                        BgwkProcess.ReportProgress(0, "^ Processing has been cancelled...");
                        break;
                    }

                    isSuccess = false;
                    m_Obj_MailSender = new cls_Gen_Mgr_MailSender();
                    m_Obj_MailSender.ToMailAddress = Convert.ToString(m_Obj_DR["to"]).Trim();
                    m_Obj_MailSender.BccMailAddress = Convert.ToString(m_Obj_DR["bcc"]).Trim();
                    m_Obj_MailSender.CcMailAddress = Convert.ToString(m_Obj_DR["cc"]).Trim();
                    m_Obj_MailSender.Subject = Convert.ToString(m_Obj_DR["subject"]).Trim();
                    m_Obj_MailSender.Body = Convert.ToString(m_Obj_DR["body"]).Trim();
                    m_Obj_MailSender.AttachmentPath = Convert.ToString(m_Obj_DR["filepath"]).Trim() + Convert.ToString(m_Obj_DR["filename"]).Trim();
                    m_Obj_Email_LogWriter.Removefiles = Convert.ToBoolean(m_Obj_DR["removefiles"]);
                    m_Obj_Email_LogWriter.Emaillogfiles = Convert.ToBoolean(m_Obj_DR["emaillogfiles"]);
                    m_Obj_Email_LogWriter.Logemailid = Convert.ToString(m_Obj_DR["logemailid"]).Trim();

                    m_Obj_MailSender.SmtpHost = Convert.ToString(m_Obj_logDs.Tables[1].Rows[0]["host"]);
                    m_Obj_MailSender.SmtpPort = Convert.ToInt32(m_Obj_logDs.Tables[1].Rows[0]["port"]);
                    m_Obj_MailSender.SmtpEnableSSL = Convert.ToBoolean(m_Obj_logDs.Tables[1].Rows[0]["enablessl"]);
                    m_Obj_MailSender.MailId = Convert.ToString(m_Obj_logDs.Tables[1].Rows[0]["username"]);
                    m_Obj_MailSender.MailPass = m_Obj_MailSender.DecryptData(Convert.ToString(m_Obj_logDs.Tables[1].Rows[0]["password"]));
                    m_Obj_MailSender.FromMailAddress = Convert.ToString(m_Obj_logDs.Tables[1].Rows[0]["username"]);
                    m_Obj_MailSender.ConfigMail();
                    BgwkProcess.ReportProgress(Barvalue, outIDString + " - Sending mail...");
                    isSuccess = m_Obj_MailSender.Send();
                    if (isSuccess == true)
                    {
                        //Move sent mail attachment in sentbox
                        //MoveInSentBoxDirectory(m_Obj_MailSender.AttachmentPath, ApplicationPath + "\\Sent Mail\\", Path.GetFileName(m_Obj_MailSender.AttachmentPath));
                        MoveInSentBoxDirectory(m_Obj_MailSender.AttachmentPath, CompanyPath + "\\Sent Mail\\", Path.GetFileName(m_Obj_MailSender.AttachmentPath));      // Changed by Sachin N. S. on 31/01/2014 for Bug-20211
                        // if successful sent mail then write in log file
                        BgwkProcess.ReportProgress(Barvalue, outIDString + " - Successfully Mail Sent...");
                        LogWriter.WriteLogToTextFile(LogFilePath, " - Successfully Mail Sent.... ", false, false);
                        // Remove all exported files
                        if (m_Obj_Email_LogWriter.Removefiles == true)
                        {
                            //string exportfilename = ApplicationPath + "\\Sent Mail\\" + Convert.ToString(m_Obj_DR["filename"]).Trim();
                            string exportfilename = CompanyPath + "\\Sent Mail\\" + Convert.ToString(m_Obj_DR["filename"]).Trim();      // Changed by Sachin N. S. on 31/01/2014 for Bug-20211
                            if (File.Exists(exportfilename))
                            {
                                File.Delete(exportfilename);
                                LogWriter.WriteLogToTextFile(LogFilePath, " - Successfully deleted " + Path.GetFileName(exportfilename).ToString().Trim() + " Exported file...", false, false);
                                BgwkProcess.ReportProgress(Barvalue, outIDString + " - Deleted " + Path.GetFileName(exportfilename).ToString().Trim() + " Exported file...");
                            }
                            else
                            {
                                LogWriter.WriteLogToTextFile(LogFilePath, " - File " + Path.GetFileName(exportfilename).ToString().Trim() + " not found for deleting...", false, false);
                                BgwkProcess.ReportProgress(Barvalue, outIDString + " - File " + Path.GetFileName(exportfilename).ToString().Trim() + " does not exists...");
                            }
                        }
                        LogEmailDelete(Convert.ToString(m_Obj_DR["id"]).Trim(),
                                       Convert.ToString(m_Obj_DR["filename"]).Trim());       
                        totMailSent++;
                    }

                }
                catch (SmtpException SmtpEx)
                {
                    BgwkProcess.ReportProgress(Barvalue, outIDString + " - Mail has not been sent,see the log file for the issue...");
                    LogWriter.WriteLogToTextFile(LogFilePath, "Mail has not been sent..." + SmtpEx.Message.Trim(), false, false); // if successful create export file then write in log file
                    //If Error found while sending mail then log file details will be inserted in eMailLog table
                    LogEmailWriter(Convert.ToString(m_Obj_DR["id"]).Trim(),
                                    m_Obj_MailSender.ToMailAddress,
                                    m_Obj_MailSender.BccMailAddress,
                                    m_Obj_MailSender.CcMailAddress,
                                    m_Obj_MailSender.Subject,
                                    m_Obj_MailSender.Body,
                                    Convert.ToString(m_Obj_DR["filepath"]).Trim(),
                                    Convert.ToString(m_Obj_DR["filename"]).Trim(),
                                    m_Obj_Email_LogWriter.Removefiles,
                                    "Pending",
                                    SmtpEx.Message.Trim(),
                                    m_Obj_Email_LogWriter.Emaillogfiles,
                                    m_Obj_Email_LogWriter.Logemailid);
                }
                catch (Exception Ex)
                {
                    BgwkProcess.ReportProgress(Barvalue, outIDString + " - Error found, see log file for the issue...");
                    LogWriter.WriteLogToTextFile(LogFilePath, "Error Found..." + Ex.Message.Trim(), false, false); // if successful create export file then write in log file
                }
                LogWriter.WriteLogToTextFile(LogFilePath, "Process End " + outIDString, false, false);
                BgwkProcess.ReportProgress(Barvalue, "Process End " + outIDString);
                LogWriter.WriteLogToTextFile(LogFilePath, "------------------------------------------------------------------------------------------", true, false);
                Barvalue = Barvalue + (Int32)mainCursPrgsValue;
                //Barvalue = Barvalue + (Int32)innerProcessPrgsValue;
            }

            LogWriter.WriteLogToTextFile(LogFilePath, "-------------------------------------<<< Summary Start >>>-----------------------------", true, false);
            LogWriter.WriteLogToTextFile(LogFilePath, "Total Records found : " + Convert.ToString(totRec).Trim(), true, false);
            //LogWriter.WriteLogToTextFile(LogFilePath, "Total Exported CSV files : " + Convert.ToString(totExportFiles).Trim(), true);
            LogWriter.WriteLogToTextFile(LogFilePath, "Total Mails sent : " + Convert.ToString(totMailSent).Trim(), true, false);
            string logfilename = LogWriter.WriteLogToTextFile(LogFilePath, "---------------------------------------<<< Summary End >>> ------------------------------", true, false);

            // Sending Log Mail
            SendLogStatus(m_Obj_Email_LogWriter.Emaillogfiles,
                          m_Obj_Email_LogWriter.Logemailid,
                          logfilename,
                          m_Obj_MailSender.SmtpHost,
                          m_Obj_MailSender.SmtpPort,
                          m_Obj_MailSender.SmtpEnableSSL,
                          m_Obj_MailSender.MailId,
                          m_Obj_MailSender.MailPass,
                          Barvalue,
                          outIDString);
            // End

            // Pass LogStatus File to Property
            LogStatusFileName = logfilename;

            outIDString = "Pending Process Ended...";
            LogWriter.WriteLogToTextFile(LogFilePath, outIDString, false, false);
            LogWriter.WriteLogToTextFile(LogFilePath, "-------------------------------------<<< END >>>-----------------------------", true, false);
            LogWriter.WriteLogToTextFile(LogFilePath, string.Empty, true, true);
            BgwkProcess.ReportProgress(Barvalue, outIDString);
        }

        //****** Added by Sachin N. S. on 17/01/2014 for Bug-20211 -- Start ******//
        public string getReportQuery(string _reportcommand, string _csqlCond)
        {
            string _sqlqry1 = _reportcommand, _sqlqry2 = "", _retQry = "";
            do
            {
                _sqlqry2 = _sqlqry1.IndexOf(":") >= 0 ? _sqlqry1.Substring(0, _sqlqry1.IndexOf(":")) : _sqlqry1;
                _sqlqry1 = _sqlqry1.IndexOf(":") >= 0 ? _sqlqry1.Substring(_sqlqry1.IndexOf(":") + 1) : "";

                if (_sqlqry2.ToUpper().Contains("EXECUTE") == true)
                {
                    _retQry = _retQry + _sqlqry2.Substring(0, _sqlqry2.IndexOf(";")) + " '" + _csqlCond.Trim().Replace("'", "''") + "' ";
                }
                else
                {
                    if (_sqlqry2.ToUpper().Contains("WHERE") == true)
                    {
                        _retQry = _retQry + _sqlqry2.Substring(0, _sqlqry2.ToUpper().IndexOf(" WHERE") + 6) + " " + _csqlCond;
                    }
                    else
                    {
                        _retQry = _retQry + _sqlqry2 + "WHERE" + " " + _csqlCond;
                    }
                }
                if (_sqlqry1 != "")
                    _retQry += ":";

            } while (_sqlqry1 != "");

            return _retQry;
        }

        public string getRptFilePath()
        {
            DataSet m_DsSelect = new DataSet("DsCompanyRec");
            m_DsSelect = cls_Sqlhelper.ExecuteDataset(connectionString, CommandType.Text, "SELECT [DbName] FROM VUDYOG..CO_MAST WHERE COMPID=" + CompanyID.ToString());
            return m_DsSelect.Tables[0].Rows[0][0].ToString().Trim();
        }

        private void getCompanyDirNm()
        {
            try
            {
                if (sqlconn == null)
                {
                    sqlconn = new SqlConnection(connectionString);
                    sqlconn.Open();
                }
                else
                {
                    if (sqlconn.State == ConnectionState.Closed)
                        sqlconn.Open();
                }

                string sqlString = string.Empty;
                if (CompanyID != null)
                {
                    if (CompanyID > 0)
                    {
                        sqlString = "Select FolderName from Vudyog..Co_mast a where Compid = "+CompanyID.ToString();
                    }
                }

                m_Obj_DS = cls_Sqlhelper.ExecuteDataset(sqlconn, CommandType.Text, sqlString);
                CompanyPath = AppDetails.AppPath + "\\"+m_Obj_DS.Tables[0].Rows[0][0].ToString().Trim();
            }
            catch (Exception Ex)
            {
                throw new Exception("found error while fetching records from emailclient table \n" +
                                    "Message : " + Ex.Message +
                                    "\nSource : " + Ex.Source +
                                    "\nTargetSite : " + Ex.TargetSite);
            }
            finally { sqlconn.Close(); }
        }
        //****** Added by Sachin N. S. on 17/01/2014 for Bug-20211 -- End ******//
        #endregion
    }
}
