using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using U2TPlus.BAL;

namespace U2TPlus
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            this.Focus();
            StringBuilder sbText = new StringBuilder();

            sbText.AppendLine("U2T Plus");
            sbText.AppendLine("Version : 1.0.0");
            label1.Text = sbText.ToString();

            //sbText = new StringBuilder();
            //sbText.AppendLine("2009,Adaequare Info Pvt Ltd");
            //sbText.AppendLine("All Rights Reserved");
            //label2.Text = sbText.ToString();

            sbText = new StringBuilder();
            sbText.AppendLine("Warning: This computer program is protected by copyright law and international");
            sbText.AppendLine("treaties. Unauthorized reproduction or distribution of this program, or any portion of");
            sbText.AppendLine("it, may result in severe civil and criminal penalties, and will be prosecuted  to the");
            sbText.AppendLine("maximum extent possible under the law.");
            lblNote.Text = sbText.ToString();
            Logger.LogInfo("Acsessed About Us");
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }        
    }
}