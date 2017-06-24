using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using eMailClient.BLL;

namespace eMailClient
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //string apppath;
            //apppath = Application.StartupPath;
            //Application.Run(new frmDocViewer());
            //args = new string[1];
            //args[0] = "23";
            //args[3] = "Auto";
            //args[2] = "EM2";

            // *** Added by Sachin N. S. on 23/01/2014 for Bug-20211 -- Start -- To be Commented
            //args = new string[9];
            //args[8] = "udpid6096dtm20110307112715";
            //args[7] = "1";
            //args[6] = "usquare.exe";
            //args[5] = "usquare pack";
            //args[4] = "";
            //args[3] = "B071314";    // "m051313";//dbname
            //args[2] = "no";
            //args[1] = "no";         //compid
            //args[0] = "1";          //compid
            // *** Added by Sachin N. S. on 23/01/2014 for Bug-20211 -- End -- To be Commented

            try
            {
                AppDetails.AppPath = Path.GetDirectoryName(Application.ExecutablePath).Trim();
                //AppDetails.AppPath = "D:\\USQUARE10";
                int mCompanyId = 0;
                string mCompanynm = "";
                string mconnectionstring = string.Empty;
                if (args.Length > 0)
                {
                    // Read Company Parameter
                    if (args[0] != null)
                    {
                        if (Convert.ToInt16(args[0]) != 0)
                        {
                            mCompanyId = Convert.ToInt16(args[0]);
                        }
                        else
                            CallMessageBox.Show(string.Empty, "Company ID Parameter cannot be empty...");
                    }

                    // Satish Pal -Start
                    if (Convert.ToString(args[3]).ToUpper().Trim() != "")
                    {
                        if (Convert.ToString(args[3]).ToUpper().Trim() != "")
                        {
                            mCompanynm = Convert.ToString(args[3]).ToUpper().Trim();
                            try
                            {
                                mconnectionstring = EvaluateConnectionString(mCompanynm);
                            }
                            catch (Exception Ex)
                            {
                                MessageBox.Show("Error found while evaluating connection string from INI  \n" +
                                                     "\nMessage : " + Ex.Message +
                                                     "\nSource : " + Ex.Source +
                                                     "\nTargetSite : " + Ex.TargetSite, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        else
                            CallMessageBox.Show(string.Empty, "Company name Parameter cannot be empty...");
                    }

                    string[] iconFiles = Directory.GetFiles(AppDetails.AppPath + "\\Bmp\\", "Icon_*.ico");
                    string icoPath = args[4].ToString() != "" ? args[4].ToString() : iconFiles.Length > 0 ? iconFiles[0].ToString().Trim() : AppDetails.AppPath + "\\Bmp\\ueIcon.ico";    // Added by Sachin N. S. on 22/01/2014 for Bug-20211
                    AppDetails.IcoPath = icoPath;    // Added by Sachin N. S. on 22/01/2014 for Bug-20211

                    // Satish Pal -End
                    // Auto Scheduler
                    ///if (args.Length > 1)//COMMENTED BY SATISH PAL
                    if (Convert.ToString(args[1]).ToUpper().Trim() == "AUTO") //ADDED BY SATISH PAL
                    {
                        if (Convert.ToString(args[1]).ToUpper().Trim() == "AUTO")
                        {
                            // Check any specific email id parameter has paased.
                            if (args.Length > 2)
                            {
                                // if (args[2] != null)
                                frmProcessWatcher obj_ProcessWatcher = new frmProcessWatcher(mCompanyId, mconnectionstring);
                                if (args[2] != "NO")
                                {
                                    foreach (string id in Convert.ToString(args[2]).Split(','))
                                    {
                                        obj_ProcessWatcher.EmailID = new List<string>();
                                        obj_ProcessWatcher.EmailID.Add(id);
                                    }
                                }
                                obj_ProcessWatcher.CompanyID = mCompanyId;
                                obj_ProcessWatcher.ExecutePendingJob = true;
                                obj_ProcessWatcher.ExecuteJob = true;
                                obj_ProcessWatcher.Action = "AUTO";
                                Application.Run(obj_ProcessWatcher);
                            }
                            else
                            {
                                frmProcessWatcher obj_ProcessWatcher = new frmProcessWatcher(mCompanyId, mconnectionstring);
                                obj_ProcessWatcher.CompanyID = mCompanyId;
                                obj_ProcessWatcher.ExecutePendingJob = true;
                                obj_ProcessWatcher.ExecuteJob = true;
                                obj_ProcessWatcher.Action = "AUTO";
                                Application.Run(obj_ProcessWatcher);
                            }
                        }
                        else
                            CallMessageBox.Show(string.Empty, "AUTO Parameter not found or may not Proper..");
                    }

                    // Only Passing Company Parameter and call main form
                    ///if (args.Length > 1)//COMMENTED BY SATISH PAL
                    if (Convert.ToString(args[1]).ToUpper().Trim() == "NO") //ADDED BY SATISH PAL
                    {
                        // Application.Run(new frmMain(mCompanyId, mconnectionstring));//ADDED BY SATISH PAL
                        Application.Run(new frmEmailClient(mCompanyId, mconnectionstring, args[5].ToString(), args[6].ToString(), args[7].ToString(), args[8].ToString()));
                    }
                }
                else
                {
                    CallMessageBox.Show(string.Empty, "Parameters are not found or may not Proper..");
                }
            }
            catch (Exception Ex)
            {
                CallMessageBox.Show(string.Empty, Ex.Message.Trim());
            }
            return;     // Added by Sachin N. S. on 22/01/2014 for Bug-20211

        }
        //added by satish pal-satrt
        private static string EvaluateConnectionString(string dbname)
        {
            string apptitle = string.Empty;
            string appfile = string.Empty;
            string server = string.Empty;
            string user = string.Empty;
            string pass = string.Empty;
            string connectionstring = string.Empty;

            var directory = AppDetails.AppPath;
            //var directory = "D:\\VUDYOGSDK";     // To be Changed by Sachin N. S. on 20/01/2014 for Bug-20211

            if (!File.Exists(directory + "\\Visudyog.ini"))
                directory = Environment.CurrentDirectory.ToString();

            if (File.Exists(directory + "\\Visudyog.ini"))
            {
                GetInfo.iniFile ini = new GetInfo.iniFile(directory + "\\Visudyog.ini");

                GetInfo.EncDec eObject = new GetInfo.EncDec();

                //****** Changed by Sachin N. S. on 20/01/2014 for Bug-20211 -- Start ******//
                //apptitle = ini.IniReadValue("Settings", "Title");
                //appfile = ini.IniReadValue("Settings", "xfile").Substring(0, ini.IniReadValue("Settings", "xfile").Length - 4);
                //server = ini.IniReadValue("DataServer", "Name");
                //user = ini.IniReadValue("DataServer", eObject.OnEncrypt("myName", eObject.Enc("myName", "User")));
                //pass = ini.IniReadValue("DataServer", eObject.OnEncrypt("myName", eObject.Enc("myName", "Pass")));

                //user = eObject.Dec("myName", eObject.OnDecrypt("myName", user));
                //pass = eObject.Dec("myName", eObject.OnDecrypt("myName", pass));
                //connectionstring = "Data Source=" + server.Trim() +
                //                   ";Initial Catalog=" + dbname.Trim() +
                //                   ";UID=" + user.Trim() +
                //                   ";Pwd=" + pass.Trim() +
                //                   ";Min Pool Size=5;Max Pool Size=60;Connect Timeout=10";

                AppDetails.Apptitle = ini.IniReadValue("Settings", "Title");
                AppDetails.Appfile = ini.IniReadValue("Settings", "xfile").Substring(0, ini.IniReadValue("Settings", "xfile").Length - 4);
                AppDetails.Server = ini.IniReadValue("DataServer", "Name");
                user = ini.IniReadValue("DataServer", eObject.OnEncrypt("myName", eObject.Enc("myName", "User")));
                pass = ini.IniReadValue("DataServer", eObject.OnEncrypt("myName", eObject.Enc("myName", "Pass")));

                AppDetails.User = eObject.Dec("myName", eObject.OnDecrypt("myName", user));
                AppDetails.Pass = eObject.Dec("myName", eObject.OnDecrypt("myName", pass));

                connectionstring = "Data Source=" + AppDetails.Server.Trim() +
                                   ";Initial Catalog=" + dbname.Trim() +
                                   ";UID=" + AppDetails.User.Trim() +
                                   ";Pwd=" + AppDetails.Pass.Trim() +
                                   ";Min Pool Size=5;Max Pool Size=60;Connect Timeout=100";
                //****** Changed by Sachin N. S. on 20/01/2014 for Bug-20211 -- End ******//
            }
            else
            {
                throw new Exception("Visudyog.ini file does not exists in " + directory.ToString() + " path.\n");
            }
            return connectionstring;
        }
        //added by satish pal -Ebd
    }

    public class CallMessageBox
    {
        static public void Show(string ExcepMessage, string Mess)
        {

            const char dbquote = '"';
            MessageBox.Show(Mess + ExcepMessage
                    + "\n\n"
                    + "Parameters Examples are below :-"
                    + "\n\n"
                    + "For Scheduler : " + "\n"
                    + "eMailClient.exe " + dbquote + "<COMPANY ID>" + dbquote + " " + dbquote + "<AUTO>" + dbquote + " " + dbquote + "<EmailID>" + dbquote
                    + "\n"
                    + "AUTO Parameter indicate for ALL IDs" + "\n"
                    + "\nScheduler for All IDs\n"
                    + "eMailClient.exe " + dbquote + "COMPANY ID" + dbquote + " " + dbquote + "AUTO" + dbquote
                    + "\n\n"
                    + "Scheduler for Selected ID \n"
                    + "eMailClient.exe " + dbquote + "COMPANY ID" + dbquote + " " + dbquote + "AUTO" + dbquote + " " + dbquote + "1,2" + dbquote
                    + "\n\n"
                    + "For Email client Interface : " + "\n"
                    + "eMailClient.exe " + dbquote + "COMPANY ID" + dbquote,
                    AppDetails.Apptitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
