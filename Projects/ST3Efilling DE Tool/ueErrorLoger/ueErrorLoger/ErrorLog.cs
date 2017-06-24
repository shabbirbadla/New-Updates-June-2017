using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Net;
using System.Diagnostics; 

namespace ueErrorLoger
{
    public class ErrorLog
    {
        public static string strLogFilePath = string.Empty;
        private static StreamWriter sw = null;

        private static bool CustomErrorRoutine(Exception objException)
        {
            string strPathName = string.Empty;

            if (strLogFilePath.Equals(string.Empty))
            {
                //Get Default log file path "LogFile.txt"
                strPathName = GetLogFilePath();
            }
            else
            {
                //If the log file path is not empty but the file
                //is not available it will create it
                if (true != File.Exists(strLogFilePath))
                {
                    if (false == CheckDirectory(strLogFilePath))
                        return false;

                    FileStream fs = new FileStream(strLogFilePath,
                            FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.Close();
                }
                strPathName = strLogFilePath;
            }
            bool bReturn = true;

            // write the error log to that text file
            if (true != WriteErrorLog(strPathName, objException))
            {
                bReturn = false;
            }
            return bReturn;
        }

        public static string LogFilePath
        {
            get
            {
                return strLogFilePath;
            }
            set
            {
                strLogFilePath = value;
            }
           
        }
        public static bool ErrorRoutine(bool bLogType, Exception objException)
        {
            try
            {
                //Check whether logging is enabled or not
                bool bLoggingEnabled = false;

                bLoggingEnabled = true;

                //Don't process more if the logging 
                if (false == bLoggingEnabled)
                    return true;

                //Write to Windows event log
                if (true == bLogType)
                {
                    string EventLogName = "ErrorLog";

                    if (!EventLog.SourceExists(EventLogName))
                        EventLog.CreateEventSource(objException.Message,
                        EventLogName);

                    // Inserting into event log
                    EventLog Log = new EventLog();
                    Log.Source = EventLogName;
                    Log.WriteEntry(objException.Message,EventLogEntryType.Error);
                }
                //Custom text-based event log
                else
                {
                    if (false == CustomErrorRoutine(objException))
                        return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message); 
                return false;
            }
        }

       


        private static bool WriteErrorLog(string strPathName,Exception  objException)
        {
            bool bReturn        = false;
            string strException    = string.Empty;
            try
            {
                sw = new StreamWriter(strPathName,true);
                sw.WriteLine("Source        : " + 
                        objException.Source.ToString().Trim());
                sw.WriteLine("Method        : " + 
                        objException.TargetSite.Name.ToString());
                sw.WriteLine("Date        : " + 
                        DateTime.Now.ToLongTimeString());
                sw.WriteLine("Time        : " + 
                        DateTime.Now.ToShortDateString());
                sw.WriteLine("Computer    : " + 
                        Dns.GetHostName().ToString());
                sw.WriteLine("Error        : " +  
                        objException.Message.ToString().Trim());
                sw.WriteLine("Stack Trace    : " + 
                        objException.StackTrace.ToString().Trim());
                sw.WriteLine("^^===================================================================^^");
                sw.Flush();
                sw.Close();
                bReturn    = true;
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message); 
                bReturn    = false;
            }
            return bReturn;
        }


        private static string GetLogFilePath()
        {
            try
            {
                // get the base directory
                string baseDir =  AppDomain.CurrentDomain.BaseDirectory +
                               AppDomain.CurrentDomain.RelativeSearchPath;

                // search the file below the current directory
                string retFilePath = baseDir + "//" + "LogFile.txt";

                // if exists, return the path
                if (File.Exists(retFilePath) == true)
                    return retFilePath;
                    //create a text file
                else
                {
                        if (false == CheckDirectory(strLogFilePath))
                             return  string.Empty;

                        FileStream fs = new FileStream(retFilePath,
                              FileMode.OpenOrCreate, FileAccess.ReadWrite);
                        fs.Close();
                }

                return retFilePath;
            }
            catch(Exception)
            {
                return string.Empty;
            }
        }

        private static bool CheckDirectory(string strLogPath)
        {
            try
            {
                int nFindSlashPos  = strLogPath.Trim().LastIndexOf("\\");
                string strDirectoryname = 
                           strLogPath.Trim().Substring(0,nFindSlashPos);

                if (false == Directory.Exists(strDirectoryname))
                    Directory.CreateDirectory(strDirectoryname);
                return true;
            }
            catch(Exception)
            {
                return false;

            }
        }


    }
}
