using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Configuration;
using System.Security;
using System.Diagnostics;
using System.Reflection;
using U2TPlus.BAL;
using System.Net;

namespace U2TPlus.BAL
{
    public class Logger
    {
        private static DirectoryInfo directoryInfo;
        private static FileStream fileStream;
        private static StreamWriter streamWriter;
        private static StackTrace stackTrace;
        private static MethodBase methodBase;
        private static void Info(Object info)
        {
            //Gets folder & file information of the log file
            string folderName = ApplicationValues.ApplicationPath + @"\Log";
            string fileName = folderName + @"\U2TPlus_Logger.txt";
            string user = string.Empty;
            directoryInfo = new DirectoryInfo(folderName);
            //Check for existence of logger file
            if (File.Exists(fileName))
            {
                try
                {
                    fileStream = new FileStream(fileName, FileMode.Append, FileAccess.Write);
                    streamWriter = new StreamWriter(fileStream);
                    string val = DateTime.Now.ToString() + " " + info.ToString();
                    streamWriter.WriteLine(val);
                }
                catch (DirectoryNotFoundException ex)
                {
                    LogInfo(ex);
                }
                catch (FileNotFoundException ex)
                {
                    LogInfo(ex);
                }
                catch (PathTooLongException ex)
                {
                    LogInfo(ex);
                }
                catch (ArgumentException ex)
                {
                    LogInfo(ex);
                }
                catch (SecurityException ex)
                {
                    LogInfo(ex);
                }
                catch (Exception Ex)
                {
                    LogInfo(Ex);
                }
                finally
                {
                    Dispose();
                }
            }
            else
            {
                //If file doesn't exist create one
                try
                {
                    directoryInfo = Directory.CreateDirectory(directoryInfo.FullName);
                    fileStream = File.Create(fileName);
                    streamWriter = new StreamWriter(fileStream);
                    String val1 = DateTime.Now.ToString() + " " + info.ToString();
                    streamWriter.WriteLine(val1);
                    streamWriter.Close();
                    fileStream.Close();
                }
                catch (FileNotFoundException fileEx)
                {
                    LogInfo(fileEx);
                }
                catch (DirectoryNotFoundException dirEx)
                {
                    LogInfo(dirEx);
                }
                catch (Exception ex)
                {
                    LogInfo(ex);
                }
                finally
                {
                    Dispose();
                }
            }
        }


        public static void LogInfo(Exception ex)
        {
            try
            {
                //Writes error information to the log file including name of the file, line number & error message description
                stackTrace = new StackTrace(ex, true);
                string fileNames = stackTrace.GetFrame((stackTrace.FrameCount - 1)).GetFileName();
                fileNames = fileNames.Substring(fileNames.LastIndexOf(ApplicationValues.ApplicationName));
                Int32 lineNumber = stackTrace.GetFrame((stackTrace.FrameCount - 1)).GetFileLineNumber();
                methodBase = stackTrace.GetFrame((stackTrace.FrameCount - 1)).GetMethod();    //These two lines are respnsible to find out name of the method
                String methodName = methodBase.Name;
                Info("Error in " + fileNames + ", Method name is " + methodName + ", at line number " + lineNumber.ToString() + " ,Error Message," + ex.Message);
            }
            catch (Exception genEx)
            {
                Info(ex.Message);
                Logger.LogInfo(genEx);
            }
            finally
            {
                Dispose();
            }
        }

        public static void LogInfo(string message)
        {
            try
            {
                //Write general message to the log file
                Info("Message-----" + message);
            }
            catch (Exception genEx)
            {
                Info(genEx.Message);
            }
        }

        private static void Dispose()
        {
            if (directoryInfo != null)
                directoryInfo = null;
            if (streamWriter != null)
            {
                streamWriter.Close();
                streamWriter.Dispose();
                streamWriter = null;
            }
            if (fileStream != null)
            {
                fileStream.Dispose();
                fileStream = null;
            }
            if (stackTrace != null)
                stackTrace = null;
            if (methodBase != null)
                methodBase = null;
        }
    }
}
