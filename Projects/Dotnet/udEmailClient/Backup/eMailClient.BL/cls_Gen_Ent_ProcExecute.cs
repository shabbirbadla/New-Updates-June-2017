using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
//using System.Windows.Forms;

namespace eMailClient.BLL
{
    public class cls_Gen_Ent_ProcExecute
    {
        #region Properties
        private BackgroundWorker bgwkProcess;
        public BackgroundWorker BgwkProcess
        {
            get { return bgwkProcess; }
            set { bgwkProcess = value; }
        }

        private string logStatusFileName;

        public string LogStatusFileName
        {
            get { return logStatusFileName; }
            set { logStatusFileName = value; }
        }

        private string logFilePath;

        public string LogFilePath
        {
            get { return logFilePath; }
            set { logFilePath = value; }
        }

        private List<string> emailID;

        public List<string> EmailID
        {
            get { return emailID; }
            set { emailID = value; }
        }

        private Int32 companyID;
        public Int32 CompanyID
        {
            get { return companyID; }
            set { companyID = value; }
        }

        private string applicationPath;

        public string ApplicationPath
        {
            get { return applicationPath; }
            set { applicationPath = value; }
        }

        private bool executePendingJob;

        public bool ExecutePendingJob
        {
            get { return executePendingJob; }
            set { executePendingJob = value; }
        }

        private List<string> logEmailID;

        public List<string> LogEmailID
        {
            get { return logEmailID; }
            set { logEmailID = value; }
        }

        private bool executeJob;

        public bool ExecuteJob
        {
            get { return executeJob; }
            set { executeJob = value; }
        }

        //**** Added by Sachin N. S. on 31/01/2014 for Bug-20211 -- Start
        private string companyPath;

        public string CompanyPath
        {
            get { return companyPath; }
            set { companyPath = value; }
        }
        //**** Added by Sachin N. S. on 31/01/2014 for Bug-20211 -- End
        #endregion
    }
}
