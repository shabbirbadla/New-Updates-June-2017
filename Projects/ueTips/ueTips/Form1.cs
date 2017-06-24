using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using cTipsSqlConn;
namespace ueTips
{
    public partial class frmTips : Form
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileString")]
        private static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);
        
        public String SqlStr;
        public int pCompId;
        
        string pCompDb;
        string pServerName;
        string pUserName;
        string pPassword;
        string pPath;
        string pvicon;

        string[] strArgs1;
        String FileName="";
        DataSet dsTipsDb=new DataSet();
        cTipsSqlConn.SlqDatacon oSqlConn = new cTipsSqlConn.SlqDatacon();
        DataView dvw = new DataView();

        BindingManagerBase bm;
        Binding bd;

        public frmTips(string[] strArgs)
        {
           

            strArgs1 = strArgs;
          
            pCompDb = strArgs[0];
            pServerName = strArgs[1];
            pUserName = strArgs[2];
            pPassword = strArgs[3];
            pPath   = strArgs[4];
            pPath = pPath.Replace("<*#*>", " ");
            pvicon = strArgs[5];
            pvicon = pvicon.Replace("<*#*>", " ");

            InitializeComponent();
           
            //Icon MainIcon = new System.Drawing.Icon(pPath+@"bmp\Icon_usquare.ico");
            //this.Icon = MainIcon;
            FileName = pPath + "bmp\\vutips.jpg";
            this.lblMain.Image = Image.FromFile(FileName);

            Icon MainIcon = new System.Drawing.Icon(pvicon);
            this.Icon = MainIcon;

            oSqlConn.pdbname = pCompDb;
            oSqlConn.pSrvname = pServerName;
            oSqlConn.pSrvusername = pUserName;
            oSqlConn.pSrvuserpassword=pPassword;
            SqlStr = "Select * from TipsMaster ";
            string selectstring;
            long vrcount;
            selectstring = "Select * from TipsMaster ";
            try
            {
               
                
                oSqlConn.Selectcommand(null, true, null, selectstring, ref dvw, out vrcount);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            
            bd = new Binding("text", dvw, "tip");
            bm = this.BindingContext[dvw];

            this.lblMain.DataBindings.Add(bd);
            if (dvw.Count > 0)
            {
                bm.Position = this.dvw.Table.Rows.Count - 1;

            }

                       
         
        }
        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        

        private void cmdClose_Click(object sender, EventArgs e)
        {
            
            if (this.checkBox1.Checked == true)
            {
                FileName = pPath + "Visudyog.ini";    
                //WritePrivateProfileString("Defaults", "ShowIntroForm", "1", @"C:\usqare\Visudyog.ini");
                WritePrivateProfileString("Defaults", "ShowIntroForm", "1", FileName);
            }
            Application.Exit();
        }

        private void cmdNext_Click(object sender, EventArgs e)
        {
            int rno = 0;
            if (this.dvw.Table.Rows.Count > 1)
            {
                rno = this.RandomNumber(1, dvw.Table.Rows.Count);
                bm.Position = rno - 1;

            }

        }
        
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            
            
        }
    }
}