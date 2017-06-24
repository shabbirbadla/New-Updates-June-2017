using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TaxillaServiceTax3Template
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        //static void Main(string[] args)           //Commented by Shrikant S. on 01/04/2015 for Bug-25365
        static int Main(string[] args)             //Added by Shrikant S. on 01/04/2015 for Bug-25365
        {
            if (args.Length < 1)
            {
                args = new string[] { "86", "M111415", "shrikant-pc", "sa", "sa1985", "^21537", "ADMIN", @"e:\u3\Usquare\Bmp\ueicon.ico", "USquare", "USquare.exe", "1", "udpid6096DTM20110307112715" };
                //CallMessageBox.Show("Parameters not found...!!!, Please pass the Parameters.", "Empty Parameters");
                //return 0;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmST3Interface(args)); //added by sandeep for bug-18141 on 14-sep-13
            return 1;

            //Commented by Shrikant S. on 01/04/2015 for Bug-25365      //Start
            ////try
            ////{
            ////    string connectionstring = string.Empty, dbname = string.Empty, iconpath = string.Empty, vumess = string.Empty;
            ////    int compid;

            ////    //args = new string[4];
            ////    //args[0] = "S011314";// "M051313";//dbname
            ////    //args[1] = @"E:\VudyogSDK\Bmp\Icon_VudyogSDK.ico";//icon path
            ////    //args[2] = "Visual Udyog SDK";//vumess
            ////    //args[3] = "6";// "14";//compid

            ////    if (args.Length > 0)
            ////    {
            ////        if (string.IsNullOrEmpty(args[0]) || string.IsNullOrEmpty(args[1]) || string.IsNullOrEmpty(args[2]))
            ////        {
            ////            CallMessageBox.Show("Parameters cannot be empty or null..!!!", "Empty Parameters");
            ////            return;
            ////        }

            ////        dbname = args[0].ToString().Trim();
            ////        iconpath = args[1].ToString().Trim();
            ////        vumess = args[2].ToString().Trim();
            ////        compid = Convert.ToInt32(args[3].ToString().Trim());

            ////        try
            ////        {
            ////            connectionstring = EvaluateConnectionString(dbname);
            ////        }
            ////        catch (Exception Ex)
            ////        {
            ////            CallMessageBox.Show("Error found while evaluating connection string from INI  \n" +
            ////                                 "\nMessage : " + Ex.Message +
            ////                                 "\nSource : " + Ex.Source +
            ////                                 "\nTargetSite : " + Ex.TargetSite, vumess);
            ////            /*MessageBox.Show("Error found while evaluating connection string from INI  \n" +
            ////                                 "\nMessage : " + Ex.Message +
            ////                                 "\nSource : " + Ex.Source +
            ////                                 "\nTargetSite : " + Ex.TargetSite, "", MessageBoxButtons.OK, MessageBoxIcon.Error);*/
            ////            return;
            ////        }

            ////        if (connectionstring == string.Empty)
            ////        {
            ////            CallMessageBox.Show("Connection string found empty...!!!, please check INI File", vumess);

            ////            //MessageBox.Show("Connection string found empty...!!!, please check INI File", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

            ////            return;
            ////        }
            ////        try
            ////        {
            ////            Application.Run(new frmST3Interface(connectionstring, dbname, iconpath, vumess, compid));
            ////        }
            ////        catch (Exception Ex)
            ////        {
            ////            CallMessageBox.Show("Message : " + Ex.Message +
            ////                                 "\nSource : " + Ex.Source +
            ////                                 "\nTargetSite : " + Ex.TargetSite, vumess);
            ////            return;
            ////        }
            ////    }
            ////    else
            ////    {
            ////        CallMessageBox.Show("Parameters not found...!!!, Please pass the Parameters.", "Empty Parameters");
            ////        return;
            ////    }
            ////}
            ////catch (Exception ex)
            ////{
            ////    CallMessageBox.Show(ex.Message, "");
            ////    return;
            ////}
            //Commented by Shrikant S. on 01/04/2015 for Bug-25365      //End
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
            //directory = "E:\\VudyogSDK";

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

    public class CallMessageBox
    {
        static public void Show(string ExcepMessage, string Mess)
        {

            const char dbquote = '"';
            MessageBox.Show(ExcepMessage, Mess, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //MessageBox.Show(Mess + ExcepMessage
            //        + "\n\n"
            //        + "Parameters Examples are below :-"
            //        + "\n\n"
            //        + "For Scheduler : " + "\n"
            //        + "eMailClient.exe " + dbquote + "<COMPANY ID>" + dbquote + " " + dbquote + "<AUTO>" + dbquote + " " + dbquote + "<EmailID>" + dbquote
            //        + "\n"
            //        + "AUTO Parameter indicate for ALL IDs" + "\n"
            //        + "\nScheduler for All IDs\n"
            //        + "eMailClient.exe " + dbquote + "COMPANY ID" + dbquote + " " + dbquote + "AUTO" + dbquote
            //        + "\n\n"
            //        + "Scheduler for Selected ID \n"
            //        + "eMailClient.exe " + dbquote + "COMPANY ID" + dbquote + " " + dbquote + "AUTO" + dbquote + " " + dbquote + "1,2" + dbquote
            //        + "\n\n"
            //        + "For Email client Interface : " + "\n"
            //        + "eMailClient.exe " + dbquote + "COMPANY ID" + dbquote,
            //        "ITAX", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
