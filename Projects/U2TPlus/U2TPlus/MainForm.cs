using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using U2TPlus.BAL;
using System.IO;
using System.Runtime.InteropServices;

namespace U2TPlus
{
    public partial class MainForm : Form
    {
        bool configValue = false;

        //const int MF_BYPOSITION = 0x400;
        //[DllImport("User32")]
        //private static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);
        //[DllImport("User32")]
        //private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        //[DllImport("User32")]
        //private static extern int GetMenuItemCount(IntPtr hWnd);



        public MainForm()
        {
            InitializeComponent();

            #region Assigning Application Values

            ApplicationValues.ApplicationPath = Application.StartupPath;
            ApplicationValues.ConfigXMLFileName = "Config.xml";
            ApplicationValues.ConfigFolderPath = ApplicationValues.ApplicationPath + @"\Config";
            ApplicationValues.ConfigFilePath = ApplicationValues.ConfigFolderPath + @"\" + ApplicationValues.ConfigXMLFileName;
            ApplicationValues.ApplicationName = Application.ProductName;
            ApplicationValues.U2TMarkupsFolderPath = Application.StartupPath + @"\U2TMarkups";
            ApplicationValues.DefaultMasterSettingsXMLName = Application.StartupPath + @"\U2TMarkups\SettingsMaster.xml";

            Logger.LogInfo("Insialized Application Values");

            #endregion

            #region Disableing the Close button in the window

            //IntPtr hMenu = GetSystemMenu(this.Handle, false);
            //int menuItemCount = GetMenuItemCount(hMenu);
            //RemoveMenu(hMenu, menuItemCount - 1, MF_BYPOSITION);

            #endregion
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (Directory.Exists(ApplicationValues.ConfigFolderPath))
            {
                string[] files = Directory.GetFiles(ApplicationValues.ConfigFolderPath);

                if (files.Length > 0)
                {
                    foreach (string filename in files)
                    {
                        FileInfo fInfo = new FileInfo(filename);
                        if (fInfo.Name == ApplicationValues.ConfigXMLFileName)
                        {
                            configValue = true;
                            DataSet DS = new DataSet();
                            DS.ReadXml(ApplicationValues.ConfigFilePath);
                            ApplicationValues.ConfigFileValues = DS;

                            if (DS!=null)
                            {
                                if (ApplicationValues.IsEnvironmentSetup)
                                {
                                    OptionsForm OPF = new OptionsForm();
                                    OPF.Show(this);
                                    Logger.LogInfo("Colling Options Form With Setup Environment");
                                }
                                else
                                {
                                    SetupEnvironment SE = new SetupEnvironment();
                                    SE.Show(this);
                                    Logger.LogInfo("Colling Options Form With out Setup Environment");
                                } 
                            }
                        }
                    }
                }
                if (configValue == false)
                {
                    ConfigurationsForm CF = new ConfigurationsForm();
                    CF.Show(this);
                    Logger.LogInfo("Config.xml file is not there");
                }
            }
            else
            {
                ConfigurationsForm CF = new ConfigurationsForm();
                CF.Show(this);
                Logger.LogInfo("Config folder is not there");
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(ApplicationValues.ConfigFolderPath))
            {
                string[] files = Directory.GetFiles(ApplicationValues.ConfigFolderPath);

                if (files.Length > 0)
                {
                    foreach (string filename in files)
                    {
                        FileInfo fInfo = new FileInfo(filename);
                        if (fInfo.Name == ApplicationValues.ConfigXMLFileName)
                        {
                            configValue = true;
                            DataSet DS = new DataSet();
                            DS.ReadXml(ApplicationValues.ConfigFilePath);
                            if (DS != null)
                            {
                                ApplicationValues.ConfigFileValues = DS;

                                if (ApplicationValues.IsEnvironmentSetup)
                                {
                                    OptionsForm OPF = new OptionsForm();
                                    OPF.Show(this);
                                    Logger.LogInfo("Colling Options Form With Setup Environment");
                                }
                                else
                                {
                                    SetupEnvironment SE = new SetupEnvironment();
                                    SE.Show(this);
                                    Logger.LogInfo("Colling Options Form With out Setup Environment");
                                }
                            }
                        }
                    }
                }
                if (configValue == false)
                {
                    ConfigurationsForm CF = new ConfigurationsForm();
                    CF.Show(this);
                    Logger.LogInfo("Config.xml file is not there");
                }
            }
            else
            {
                ConfigurationsForm CF = new ConfigurationsForm();
                CF.Show(this);
                Logger.LogInfo("Config folder is not there");
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "You Need to Exit The Application", "Exit Application", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                Logger.LogInfo("Config folder is not there");
                Application.Exit();
            }
        }
    }
}