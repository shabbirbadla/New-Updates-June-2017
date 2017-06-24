using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace U2TPlus
{
    public partial class HelpForm : Form
    {
        public HelpForm()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo(linkLabel1.Text);
            Process.Start(sInfo);
            this.Close();
        }

        private void HelpForm_Load(object sender, EventArgs e)
        {
            this.Focus();
            StringBuilder displayText = new StringBuilder();
            displayText.AppendLine("Please Mail Me regarding your Issues/queries");
            displayText.AppendLine("And Also Send Me Your Suggestions");
            displayText.AppendLine("Mail To  :  u2tplushelp@gmail.com");
            label1.Text = displayText.ToString();
        }
    }
}