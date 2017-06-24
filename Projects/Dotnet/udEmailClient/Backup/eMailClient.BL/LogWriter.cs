using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace eMailClient.BLL
{
    public class LogWriter
    {
        public static string LogFileName = "";

        public static string WriteLogToTextFile(string loggingDirectoryPath, string logMessage, bool excludeDate,bool isWriteFinalTxt)
        {
            bool successStatus = false;

            DateTime currentDateTime = DateTime.Now;

            string currentDateTimeString = currentDateTime.ToString();

            CheckCreateLogDirectory( loggingDirectoryPath );

            string logLine = string.Empty;

            if (excludeDate == true)
                logLine = BuildLogLine(logMessage );
            else
                logLine = BuildLogLine(currentDateTime, logMessage);

            if (LogFileName == "")
                LogFileName = GetLogFileNameDateString(DateTime.Now);

            //loggingDirectoryPath = (loggingDirectoryPath + "Log_" + GetLogFileNameDateString( DateTime.Now ) + ".txt" );
            loggingDirectoryPath = (loggingDirectoryPath + "Log_" + LogFileName.Trim() + ".txt");   //Changed By Sachin N. S. on 20/01/2014 for Bug-20211
            //string tempfile = Path.GetTempFileName();

            // Lock type as this is a static method / class
            lock ( typeof( LogWriter ) )
            {
                StreamWriter m_logSWriter = null;
                //StreamReader m_logSReader = null;
                
                try
                {
                    m_logSWriter = new StreamWriter(loggingDirectoryPath, true);
                   // m_logSWriter = new StreamWriter(tempfile, true);
                    m_logSWriter.WriteLine(logLine);


                    //if (isWriteFinalTxt == true)
                    //{
                    //    m_logSReader = new StreamReader(loggingDirectoryPath);
                    //    while (!m_logSReader.EndOfStream)
                    //        m_logSWriter.WriteLine(m_logSReader.ReadLine());
                    //}

                    successStatus = true;
                }
                catch 
                {
                    // 
                }
                finally
                {
                    if ( m_logSWriter != null )
                    {
                        m_logSWriter.Close();

                        //if (isWriteFinalTxt == true)
                        //{
                        //    //m_logSReader.Close();
                        //    File.Copy(tempfile, loggingDirectoryPath, true);
                        //}
                    }
                }
            }

            //return successStatus;

            return loggingDirectoryPath;
        }


       

        /// <summary>
        /// Checks for the existence of a log file directory, and creates the directory
        /// if it is not found.
        /// </summary>
        /// <param name="logPath"></param>
        private static bool CheckCreateLogDirectory( string logPath )
        {
            bool loggingDirectoryExists = false;

            DirectoryInfo dirInfo = new DirectoryInfo( logPath );

            if ( dirInfo.Exists )
            {
                loggingDirectoryExists = true;
            }
            else
            {
                try
                {
                    Directory.CreateDirectory( logPath );

                    loggingDirectoryExists = true;
                }
                catch 
                {
                    // Logging failure
                }
            }

            return loggingDirectoryExists;
        }


        /// <summary>
        /// Returns a string for insertion into the log. Columns separated by tabs.
        /// </summary>
        /// <param name="currentDateTime"></param>
        /// <param name="logMessage"></param>
        /// <returns></returns>
        private static string BuildLogLine( DateTime currentDateTime, string logMessage )
        {
            StringBuilder loglineStringBuilder = new StringBuilder();

            loglineStringBuilder.Append(logMessage.IndexOf("#") >= 0 ? "\n\n" : "");
            loglineStringBuilder.Append(GetLogFileEntryDateString( currentDateTime ) );
            loglineStringBuilder.Append("\t");
            loglineStringBuilder.Append( logMessage.Replace("#",""));

            return loglineStringBuilder.ToString();
        }

        /// <summary>
        /// Returns a string for insertion into the log. Columns separated by tabs.
        /// </summary>
        /// <param name="logMessage"></param>
        /// <returns></returns>
        private static string BuildLogLine(string logMessage)
        {
            StringBuilder loglineStringBuilder = new StringBuilder();

            loglineStringBuilder.Append(logMessage.IndexOf("#") >= 0 ? "\n\n" : "");
          //  loglineStringBuilder.Append("\t");
            loglineStringBuilder.Append(logMessage.Replace("#", ""));

            return loglineStringBuilder.ToString();
        }


        /// <summary>
        /// Returns a string with the date in the format yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="currentDateTime"></param>
        /// <returns></returns>
        public static string GetLogFileEntryDateString( DateTime currentDateTime )
        {
            return currentDateTime.ToString( "yyyy-MM-dd HH:mm:ss" );
        }

        /// <summary>
        /// Returns a string with the date in the format yyyy_MM_dd
        /// </summary>
        /// <param name="currentDateTime"></param>
        /// <returns></returns>
        private static string GetLogFileNameDateString( DateTime currentDateTime )
        {
            //return currentDateTime.ToString( "yyyy_MM_dd" );
            return currentDateTime.ToString("yyyy_MM_dd_HH_mm_ss");
        }

        /// <summary>
        /// Returns a string with the Time in the format HH_MM_SS
        /// </summary>
        /// <param name="currentDateTime"></param>
        /// <returns></returns>
        private static string GetLogFileNameTimeString(DateTime currentDateTime)
        {
            return currentDateTime.ToString("yyyy_MM_dd");
        }
    }
}

