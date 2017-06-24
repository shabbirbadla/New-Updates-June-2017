using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;
using System.Text.RegularExpressions;
using WindowsFormsApplication1;

namespace WindowsFormsApplication1
{
    public partial class FrmOnlineShop : Form
    {
        private SqlConnection SqlConn;
        private string connectionString;
        
        public FrmOnlineShop()
        {
            InitializeComponent();
            //GetInfo.iniFile getIni = new GetInfo.iniFile(Application.StartupPath + "\\Visudyog.ini");
            GetInfo.iniFile getIni = new GetInfo.iniFile("D:\\USQUARE\\Visudyog.ini");
            GetInfo.EncDec getEncDec = new GetInfo.EncDec();
            string Server = getIni.IniReadValue("DataServer", "Name");
            string user = getIni.IniReadValue("DataServer", getEncDec.OnEncrypt("myName", getEncDec.Enc("myName", "User")));
            string pass = getIni.IniReadValue("DataServer", getEncDec.OnEncrypt("myName", getEncDec.Enc("myName", "Pass")));
            connectionString = "Data Source=" + Server + ";Initial Catalog=VasantVudyog;Uid=" + getEncDec.Dec("myName", getEncDec.OnDecrypt("myName", user)) + ";Pwd=" + getEncDec.Dec("myName", getEncDec.OnDecrypt("myName", pass));
        }

        //public void FrmOnlineShop()
        //{
            //connectionString = System.Configuration.ConfigurationSettings.;
            //GetInfo.iniFile getIni = new GetInfo.iniFile(Application.StartupPath+"\\Visudyog.ini");
            //GetInfo.EncDec getEncDec = new GetInfo.EncDec();
            //string Server = getIni.IniReadValue("DataServer", "Name");
            //string user = getIni.IniReadValue("DataServer", getEncDec.OnEncrypt("myName", getEncDec.Enc("myName", "User")));
            //string pass = getIni.IniReadValue("DataServer", getEncDec.OnEncrypt("myName", getEncDec.Enc("myName", "Pass")));
            //connectionString = "Data Source=" + Server + ";Initial Catalog=Master;Uid=" + getEncDec.Dec("myName", getEncDec.OnDecrypt("myName", user)) + ";Pwd=" + getEncDec.Dec("myName", getEncDec.OnDecrypt("myName", pass));
        //}


        public SqlConnection connectionOpen(string conStr)
        { 
            SqlConnection SqlConn1 = new SqlConnection(conStr);
            return SqlConn1;
        }

        public void ConnectionClose()
        { 
            if (SqlConn != null)
            {
                SqlConn.Close();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                if (SqlConn == null)
                {
                    SqlConn = connectionOpen(connectionString);
                    SqlConn.Open();
                }
                else
                {
                    if (SqlConn.State == ConnectionState.Closed)
                    {
                        SqlConn = connectionOpen(connectionString);
                        SqlConn.Open();
                    }
                }

                DataSet ds = new DataSet();
                string sqlStr = "select *, FeatureProd=space(250) from Proddetail";
                //SqlDataAdapter da = new SqlDataAdapter(sqlStr, SqlConn);
                
                //ds.Dispose();

                //sqlStr = "select *,_optionType=space()  from ProdDetail";
                SqlDataAdapter da = new SqlDataAdapter(sqlStr, SqlConn);
                da.Fill(ds, "FeatureDetl_Tbl");
                
                //ds.Dispose();

                SqlConn.Close();
                setBinding_Source(ds);

            }
            catch (SqlException Sqlexcp)
            {
                MessageBox.Show(Sqlexcp.Message.ToString(),"",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }

        private void setBinding_Source(DataSet ds1)
        {
            string _decData;
            EncrytDecrypt ab = new EncrytDecrypt();
            //Regex _regExp0 = new Regex(@"\s*(?[^~]+)\s*~*0*~", System.Text.RegularExpressions.RegexOptions.Compiled);
            //Regex _regExp1 = new Regex(@"~*0*~\s*(?[^~]+)\s*~*1*~", System.Text.RegularExpressions.RegexOptions.Compiled);
            foreach (DataRow dr in ds1.Tables["FeatureDetl_Tbl"].Rows)
            {
                _decData = ab.newDecrypt(dr.ItemArray[0].ToString().Trim(),"Udencyogprod");
                MessageBox.Show(_decData);
            }
            this.dataGridView1.DataSource = ds1.Tables["FeatureDetl_Tbl"];
        }


    }
}
