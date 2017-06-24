using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace ExportDataToFile
{
    public partial class Dialog : uBaseForm.FrmBaseForm
    {
        public Dialog()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1(1);
            this.Close();
        }
        public void demo(DataTable dt,string val,Icon pfrmicon)
        {
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.MultiSelect = false;
         
            dataGridView1.DataSource = dt;
            dataGridView1.Columns[0].DataPropertyName = "Field Name";
            dataGridView1.Columns[0].HeaderText = "Field Name";
            dataGridView1.Columns[0].ReadOnly = true;
            label2.Text = val.ToString();
            this.Icon = pfrmicon;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1(33);
            this.Close();
        }
    }
}
