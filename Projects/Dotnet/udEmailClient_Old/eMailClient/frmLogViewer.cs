using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

using eMailClient.BLL;

namespace eMailClient
{
    public partial class frmLogViewer : Form
    {   

        #region Properties
        private string filename;

        public string Filename
        {
            get { return filename; }
            set { filename = value; }
        }
        #endregion

        public frmLogViewer()
        {
            InitializeComponent();
        }
     
        public void ReadLogFile(string filename)
        {
            lblFilename.Text = Path.GetFileName(filename);
            rtbLogFile.LoadFile(filename,RichTextBoxStreamType.PlainText);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
