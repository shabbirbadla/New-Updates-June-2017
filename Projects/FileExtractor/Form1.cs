using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace FileExtractor
{
    public class ZipMessage
    {
        public string ErrMessage;
        public ZipMessage()
        {
            ErrMessage = "";
        }
    }
    public partial class Form1 : Form
    {
        string mainEXE, sqlUID, sqlPWD,CurrPath,sqlSever, sucMessage;

        public Form1()
        {
            InitializeComponent();
            CurrPath = Application.ExecutablePath;
            CurrPath = CurrPath.Substring(0, CurrPath.LastIndexOf("\\"));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBox1.Text = folderBrowserDialog1.SelectedPath;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true ;
        }

        private void button2_Click(object sender, EventArgs e)
        {

            button3.Enabled = false;

            udyogerp.udyogerpClass  obj = new udyogerp.udyogerpClass();
            sqlUID = obj.GetVal(CurrPath, "User").ToString();
            sqlPWD = obj.GetVal(CurrPath, "Pass").ToString();
            sqlSever = obj.GetVal(CurrPath, "Name").ToString();
            mainEXE = obj.GetVal(CurrPath, "XFile").ToString();
            sucMessage = "";
            if (sqlSever.ToUpper() == "File Not Found".ToUpper() | sqlPWD.ToUpper() == "File Not Found".ToUpper() | sqlUID.ToUpper() == "File Not Found".ToUpper())
            {
                MessageBox.Show("Not A Valid Folder");
                button3.Enabled = true;
                return;
            }
            if (String.IsNullOrEmpty(sqlSever) & String.IsNullOrEmpty(sqlPWD))
            {
                if (!IsCompanyExist(sqlUID, sqlPWD, sqlSever))
                {
                    UdyogZipUnzip.UdyoyZipUnZipUtility xx = new UdyogZipUnzip.UdyoyZipUnZipUtility();
                    if (!String.IsNullOrEmpty(mainEXE))
                        xx.UdyogUnzip("MainExe.zip", mainEXE, textBox1.Text, "");
                    if (xx.zm.ErrMessage != "")
                    {
                        sucMessage=sucMessage+"Error Occured while extracting MainExe.Zip\n";
                    }
                    xx.UdyogUnzip("Database.zip", textBox1.Text + "\\Database", "");
                    if (xx.zm.ErrMessage != "")
                    {
                        sucMessage = sucMessage + "Error Occured while extracting Database.zip\n";
                    }
                    xx.UdyogUnzip("Class.zip", textBox1.Text + "\\Class", "");
                    if (xx.zm.ErrMessage != "")
                    {
                        sucMessage = sucMessage + "Error Occured while extracting Class.zip\n";
                    }
                    xx.UdyogUnzip("Vudyog.zip", textBox1.Text, "");
                    if (xx.zm.ErrMessage != "")
                    {
                        sucMessage = sucMessage + xx.zm.ErrMessage+"\n";
                    }
                    sucMessage = sucMessage + "Opereation Finished...";
                    MessageBox.Show(sucMessage);
                }
                else
                    MessageBox.Show("It's Not a Fresh Installation, Companies Exists...");
            }
            else
            {
                MessageBox.Show("It's Not a Fresh Installation...");
            }
            button3.Enabled = true;
        }
        public bool IsCompanyExist(string uid, string pass,string sever)
        {
           // string conn = "server=" + sever + ";database=vudyog" + ";user id=" + uid + ";password=" + pass;
            string conn = "server=" + sever + ";database=vudyog" + ";user id=sa;password=" + pass;
            SqlConnection cn = new SqlConnection(conn);
            try
            {
                cn.Open();
                SqlCommand cmb = new SqlCommand("select * from co_mast", cn);
                SqlDataReader dr = cmb.ExecuteReader();
                if (dr.HasRows)
                    return true;
            }
            catch
            {
                if (String.IsNullOrEmpty(sever) | String.IsNullOrEmpty(pass))
                    return false;
                else
                    return true;
            }            
            return false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();            
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = CurrPath;
        }
    }
}
