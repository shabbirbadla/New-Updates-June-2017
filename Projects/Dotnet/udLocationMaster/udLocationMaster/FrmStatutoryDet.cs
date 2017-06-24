using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace udLocationMaster
{
    public partial class FrmStatutoryDet : Form 
    {
        DataSet dsmain_chld;
        Boolean vEnable;
        public FrmStatutoryDet()
        {
            InitializeComponent();
            this.Text = "Statutory Details";
        }

        public FrmStatutoryDet(DataSet dsmain, Boolean vEnabled, Icon MainIcon)
        {
            InitializeComponent();
            this.Text = "Statutory Details";
            dsmain_chld = dsmain;
            vEnable = vEnabled;
            this.Icon = MainIcon;
        }
        private void EnableDisableFormControls()
        {
            grpPFDet.Enabled = vEnable;
            grpMSICDet.Enabled = vEnable;
            grpPTDet.Enabled = vEnable;
        }
        private void BindData()
        {
            this.txtPFcode.DataBindings.Add("Text", dsmain_chld.Tables[0], "PF_Code");
            this.txtPFsign.DataBindings.Add("Text", dsmain_chld.Tables[0], "PF_Sign");
            this.txtESICcode.DataBindings.Add("Text", dsmain_chld.Tables[0], "ESIC_Code");
            this.txtESICsign.DataBindings.Add("Text", dsmain_chld.Tables[0], "ESIC_Sign");
            this.txtPTcode.DataBindings.Add("Text", dsmain_chld.Tables[0], "PT_Code");
            this.txtPTsign.DataBindings.Add("Text", dsmain_chld.Tables[0], "PT_Sign");

            this.txtESICDesig.DataBindings.Add("Text", dsmain_chld.Tables[0], "ESIC_Desig");  /*Ramya for Bug-7617*/
            this.txtESICAdd.DataBindings.Add("Text", dsmain_chld.Tables[0], "ESIC_Add");  /*Ramya for Bug-7617*/
        }
        private void btnDone_Click(object sender, EventArgs e)
        {
            dsmain_chld.AcceptChanges();
            this.Close();
        }

        private void FrmStatutoryDet_Load(object sender, EventArgs e)
        {
            this.BindData();
            this.EnableDisableFormControls();
        }

        private void FrmStatutoryDet_FormClosed(object sender, FormClosedEventArgs e)
        {
            btnDone.PerformClick();
        }

      

    }
}
