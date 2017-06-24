using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace Karnataka
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
            try
            {
                string connectionstring = string.Empty, dbname = string.Empty, iconpath = string.Empty, vumess = string.Empty;
                int compid;

                //args = new string[4];
                ////args[0] = "S011314";
                //args[0] = "U011415";// "M051313";//dbname
                //args[1] = @"D:\VudyogSDK\Bmp\Icon_VudyogSDK.ico";//icon path
                //args[2] = "Visual Udyog SDK";//vumess
                //args[3] = "1";
                ////args[3] = "14";

                if (args.Length > 0)
                {
                    if (string.IsNullOrEmpty(args[0]) || string.IsNullOrEmpty(args[1]) || string.IsNullOrEmpty(args[2]) || string.IsNullOrEmpty(args[3]))
                    {
                        MessageBox.Show("Parameters cannot be empty or null..!!!", "Empty Parameters", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    dbname = args[0].ToString().Trim();
                    iconpath = args[1].ToString().Trim();
                    vumess = args[2].ToString().Trim();
                    compid = Convert.ToInt32(args[3].ToString().Trim());

                    try
                    {
                        connectionstring = EvaluateConnectionString(dbname);
                    }
                    catch (Exception Ex)
                    {
                        MessageBox.Show("Error found while evaluating connection string from INI  \n" +
                                             "\nMessage : " + Ex.Message +
                                             "\nSource : " + Ex.Source +
                                             "\nTargetSite : " + Ex.TargetSite, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (connectionstring == string.Empty)
                    {
                        MessageBox.Show("Connection string found empty...!!!, please check INI File", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    try
                    {
                        Application.Run(new frmKarnataka(connectionstring, dbname, iconpath, vumess, compid));
                    }
                    catch (Exception Ex)
                    {
                        MessageBox.Show("Message : " + Ex.Message +
                                             "\nSource : " + Ex.Source +
                                             "\nTargetSite : " + Ex.TargetSite, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Parameters not found...!!!, Please pass the Parameters.", "Empty Parameters", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private static string EvaluateConnectionString(string dbname)
        {
            string apptitle = string.Empty;
            string appfile = string.Empty;
            string server = string.Empty;
            string user = string.Empty;
            string pass = string.Empty;
            string connectionstring = string.Empty;

            var directory = Path.GetDirectoryName(Environment.CurrentDirectory.ToString());

            if (!File.Exists(directory + "\\Visudyog.ini"))
                directory = Environment.CurrentDirectory.ToString();

            if (File.Exists(directory + "\\Visudyog.ini"))
            {
                GetInfo.iniFile ini = new GetInfo.iniFile(directory + "\\Visudyog.ini");

                GetInfo.EncDec eObject = new GetInfo.EncDec();
                apptitle = ini.IniReadValue("Settings", "Title");
                appfile = ini.IniReadValue("Settings", "xfile").Substring(0, ini.IniReadValue("Settings", "xfile").Length - 4);
                server = ini.IniReadValue("DataServer", "Name");
                user = ini.IniReadValue("DataServer", eObject.OnEncrypt("myName", eObject.Enc("myName", "User")));
                pass = ini.IniReadValue("DataServer", eObject.OnEncrypt("myName", eObject.Enc("myName", "Pass")));

                user = eObject.Dec("myName", eObject.OnDecrypt("myName", user));
                pass = eObject.Dec("myName", eObject.OnDecrypt("myName", pass));

                connectionstring = "Data Source=" + server.Trim() +
                                   ";Initial Catalog=" + dbname.Trim() +
                                   ";UID=" + user.Trim() +
                                   ";Pwd=" + pass.Trim() +
                                   ";Min Pool Size=5;Max Pool Size=60;Connect Timeout=10";
            }
            else
            {
                throw new Exception("Visudyog.ini file does not exists in " + directory.ToString() + " path.\n");
            }
            return connectionstring;
        }
    }
}
