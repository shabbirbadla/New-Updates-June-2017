using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ueProductUpgrade
{
    public partial class ProgressBar : Form
    {
        public ProgressBar()
        {
            InitializeComponent();
        }
        public void ShowProgress(string lcaption, int lvalue)
        {
            if (lvalue <= 0)
            {
                lblStatus.Text = lcaption;
                pBar.Visible = false;    
                return;
            }
            pBar.Visible = true;    
            lblStatus.Text = lcaption;
            pBar.Value = (lvalue>100?100:lvalue);

            this.Refresh();
        }

        private void ProgressBar_Load(object sender, EventArgs e)
        {

        }

    }

}