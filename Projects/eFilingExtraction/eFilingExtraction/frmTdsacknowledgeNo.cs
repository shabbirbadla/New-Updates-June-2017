using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace eFillingExtraction
{
    public partial class frmTdsacknowledgeNo : Form
    {
        private string _AcknowledgeNo = string.Empty;
        public string AcknowledgeNo
        {
            get { return _AcknowledgeNo; }
            set 
            {
                if (value.Length < 15)
                {
                    throw new Exception("Please enter 15 digit No.");
                }
                _AcknowledgeNo = value; 
            }
        }
        public frmTdsacknowledgeNo()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                this.AcknowledgeNo = this.txtAcknowledgeno.Text;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        //    this.Hide();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
        
    }
}
