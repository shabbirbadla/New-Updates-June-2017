using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace Udyog.Application.License
{
    public class Logger
    {
        private static string m_logFileName = "StartLogger.log";
        private static FileStream m_logFileStream = null;
        private static StreamWriter m_logWriter = null;

        static Logger()
        {
            string folder = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            string logfilePath = Path.Combine(folder, m_logFileName);

            m_logFileStream = File.Open(logfilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
            m_logWriter = new StreamWriter(m_logFileStream);
        }

        public static void Close()
        {
            if (m_logFileStream != null)
            {
                m_logWriter.Close();
                m_logFileStream.Close();

                m_logFileStream.Dispose();
                m_logFileStream = null;
            }

            if (m_logWriter != null)
            {
                m_logWriter.Dispose();
                m_logWriter = null;
            }
        }

        public static void LogMessage(string message)
        {
            if (m_logFileStream == null)
                return;

            m_logWriter.Write(DateTime.Now.ToString() + ": ");
            m_logWriter.WriteLine(message);
            m_logWriter.Flush();
        }

        public static void LogMessage(string message, params object[] args)
        {
            if (m_logFileStream == null)
                return;

            m_logWriter.Write(DateTime.Now.ToString() + ": ");
            m_logWriter.WriteLine(message, args);
            m_logWriter.Flush();
        }
    }
}
