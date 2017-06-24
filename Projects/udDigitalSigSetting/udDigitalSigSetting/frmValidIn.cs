using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace udDigitalSigSetting
{
    public partial class frmValidIn : Form
    {
        public DataSet ds;
        public string validdata = string.Empty,actstr=string.Empty;
        public string[] InpValid, asd1;
        public frmValidIn()
        {
            InitializeComponent();
        }

        public void getds(DataSet ds2,string InValid)
        {
            InpValid = InValid.Split(' ');
            validdata = InValid;
            int i=0;
            foreach (DataRow dr in ds2.Tables[0].Rows)
            {
               listView1.Items.Add(dr["code_nm"].ToString());
               asd1 = InpValid.Where(n => n == dr["entry_ty"].ToString()).ToArray();
               if (asd1.Length != 0)
               {
                   listView1.Items[i].Checked=true;
               }
                i=i+1;
            }

        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            string data=string.Empty;
            foreach (ListViewItem it in listView1.Items)
            {
                if (it.Checked)
                {
                    DataRow[] drflt =ds.Tables[0].Select("code_nm='"+it.Text+"'");
                    data = data +" "+ drflt[0].ItemArray[1].ToString();
                }
            }
            validdata= data;
            this.Close();
        }


    }
}
