using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using eMailClient.BLL;

namespace eMailClient
{
    public partial class frmProcessWatcher : Form
    {
        
        #region define variables


        cls_Gen_Mgr_ProcExecute m_Obj_ProcExecute;

        private List<string> emailID;
        public List<string> EmailID
        {
            get { return emailID; }
            set { emailID = value; }
        }

        private string logStatusFileName;
        public string LogStatusFileName
        {
            get { return logStatusFileName; }
            set { logStatusFileName = value; }
        }


        private string action;
        public string Action
        {
            get { return action; }
            set { action = value; }
        }

        private Int32 companyID;
        public Int32 CompanyID
        {
            get { return companyID; }
            set { companyID = value; }
        }

        private Int32 connectionString;
        public Int32 ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        private List<string> logEmailID;

        public List<string> LogEmailID
        {
            get { return logEmailID; }
            set { logEmailID = value; }
        }

        private bool executePendingJob;

        public bool ExecutePendingJob
        {
            get { return executePendingJob; }
            set { executePendingJob = value; }
        }

        private bool executeJob;

        public bool ExecuteJob
        {
            get { return executeJob; }
            set { executeJob = value; }
        }
        #endregion


        #region Forms & Controls Event
        public frmProcessWatcher(Int32 CompanyID,String ConnectionString)
        {
            InitializeComponent();
            m_Obj_ProcExecute = new cls_Gen_Mgr_ProcExecute(CompanyID,ConnectionString);

        }

        protected virtual void frmProcessWatcher_Load(object sender, EventArgs e)
        {
            m_Obj_ProcExecute.BgwkProcess = bgwkProcess;
            Init();
        }

        private void Init()
        {
            if (Action == "AUTO")
            {
                notifyIcon.BalloonTipText = AppDetails.Apptitle + " eMail Scheduler running.....";
                notifyIcon.BalloonTipTitle = AppDetails.Apptitle+" eMail Scheduler";
                notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
                notifyIcon.Text = AppDetails.Apptitle+" Email Scheduler";
                notifyIcon.Visible = false;


                ContextMenu obj_ContextMenu = new ContextMenu();

                MenuItem obj_MenuItem = new MenuItem();
                obj_ContextMenu.MenuItems.AddRange(new MenuItem[] { obj_MenuItem });
                obj_MenuItem.Index = 0;
                obj_MenuItem.Text = "Show Process";
                obj_MenuItem.Click += new EventHandler(obj_MenuItem_ShowProcessClick);

                obj_MenuItem = new MenuItem();
                obj_ContextMenu.MenuItems.AddRange(new MenuItem[] { obj_MenuItem });
                obj_MenuItem.Index = 1;
                obj_MenuItem.Text = "Exit";
                obj_MenuItem.Click += new EventHandler(obj_MenuItem_ExitClick);

                notifyIcon.ContextMenu = obj_ContextMenu;
            }
            // Set BackGroundWorker Control Properties
            bgwkProcess.WorkerReportsProgress = true;
            bgwkProcess.WorkerSupportsCancellation = true;
            bgwkProcess.RunWorkerAsync();
        }

        private void Ticks(object sender, EventArgs e)
        {
            Timer tm = (Timer)sender;
            tm.Enabled = false;
            tm.Stop();
            timer.Start();
            timer.Enabled = true;
        }

        private void StartProcess(DoWorkEventArgs e)
        {
            try
            {
                m_Obj_ProcExecute.ApplicationPath = Application.StartupPath;
                m_Obj_ProcExecute.LogFilePath = m_Obj_ProcExecute.CompanyPath + "\\Log\\";      // Commented by Sachin N. S. on 31/01/2014 for Bug-20211
                m_Obj_ProcExecute.EmailID = EmailID;
                m_Obj_ProcExecute.LogEmailID = LogEmailID;
                m_Obj_ProcExecute.ExecutePendingJob = ExecutePendingJob;
                m_Obj_ProcExecute.ExecuteJob = ExecuteJob;
                m_Obj_ProcExecute.ExecuteProcess();
                LogStatusFileName = m_Obj_ProcExecute.LogStatusFileName;
            }
            catch (Exception Ex)
            {
                bgwkProcess.CancelAsync();
                if (bgwkProcess.CancellationPending)
                {
                    e.Cancel = true;
                }
                MessageBox.Show(Ex.Message,
                                AppDetails.Apptitle, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }
        }

        private void bgwkProcess_DoWork(object sender, DoWorkEventArgs e)
        {
            StartProcess(e);
        }

   
        private void bgwkProcess_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblPercentage.Text = e.ProgressPercentage.ToString().Trim() + "%";

            progBar.Value = e.ProgressPercentage;
            progBar.Text = "Please wait, Processing is going on......";
            txtOutput.AppendText(e.UserState.ToString().Trim());
            txtOutput.AppendText(Environment.NewLine);

            if (Action == "AUTO")
            {
                if (WindowState == FormWindowState.Minimized)
                {
                    notifyIcon.BalloonTipText = AppDetails.Apptitle+" eMail Sender running at " + progBar.Value.ToString() + "%,\nClick here for Details.";
                    notifyIcon.ShowBalloonTip(500);
                }
            }
        }

        private void bgwkProcess_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                txtOutput.AppendText(" Process has been cancelled !!!! ");
                txtOutput.AppendText(Environment.NewLine);
            }
            else if (e.Error != null)
            {
                MessageBox.Show("Error. Details: " + (e.Error as Exception).ToString(), AppDetails.Apptitle, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            else
            {
                txtOutput.AppendText(" Task has been completed successfully !!!! ");
                txtOutput.AppendText(Environment.NewLine);
                progBar.Value = 100;
                lblPercentage.Text = 100.ToString().Trim() + "%";
                Timer tmWait = new Timer();
                tmWait.Interval = 600;
                tmWait.Enabled = true;
                tmWait.Start();
                tmWait.Tick += new EventHandler(Ticks);

             }
        }


        private void timer_Tick(object sender, EventArgs e)
        {
            if (this.Opacity > 0.5)
            {
                this.Opacity -= 0.05;
            }
            else
            {
                timer.Enabled = false;
                this.Close();
            } 
        }

        private void cmdProcessCancel_Click(object sender, EventArgs e)
        {
            bgwkProcess.CancelAsync();
        }

        private void frmProcessWatcher_Resize(object sender, EventArgs e)
        {
            if (Action == "AUTO")
            {
                if (FormWindowState.Minimized == WindowState)
                {
                    this.Hide();
                    notifyIcon.Visible = true;
                    notifyIcon.ShowBalloonTip(500);
                }
                else
                {
                    if (FormWindowState.Normal == WindowState)
                    {
                        notifyIcon.Visible = false;
                    }
                }
            }
        }
        #endregion

        #region Notifier Events
        private void notifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            ShowProcess();
        }
        #endregion

        #region Context Menu Events
        private void obj_MenuItem_ExitClick(object Sender, EventArgs e)
        {
            Application.Exit();
        }

        private void obj_MenuItem_ShowProcessClick(object Sender, EventArgs e)
        {
            ShowProcess();
        }
        #endregion

        #region Private Methods
        private void ShowProcess()
        {
            if (FormWindowState.Minimized == WindowState)
            {
                this.Show();
                WindowState = FormWindowState.Normal;
                notifyIcon.Visible = false;
            }
        }
        #endregion
    }
}
