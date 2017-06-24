using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace eMailClient
{
    public partial class frmMain : Form
    {
        #region Define Variables
        public string vumess = "iTAX";
        private bool isExit;

        private Int32 companyID;
        private string connectionstring;//added by satish pal
       

        #endregion

        #region Forms & Control Events
       /// public frmMain(Int32 CompanyID, String mCompanynm)
        public frmMain(Int32 CompanyID,String Connectionstring)   //added by satish pal 
        {
            companyID = CompanyID;
            connectionstring = Connectionstring; ///added  by satish pal
            InitializeComponent();

        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {   
            if (MdiChildren.Length > 0)
            {
                string childformtext = "", msg = "";
                int childcount = 0;
                foreach (Form childform in MdiChildren)
                {
                    childformtext = (childformtext != string.Empty ? childformtext + "," : childformtext) + childform.Text.ToString();
                    childcount++;
                }
                if (childcount == 1)
                    //msg = "The " + childformtext.ToString() + " Form is Open.\nClose the form and then exit.";
                    msg = "The Form is Open.\nClose the form and then exit.";
                else 
                    //msg = "The " + childformtext.ToString() + " Forms are Open.\nClose the forms and then exit.";
                    msg = "The Forms are Open.\nClose the forms and then exit.";
                MessageBox.Show(msg.ToString(), vumess, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                isExit = true;
                Close();
                isExit = false;
            }
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MdiChildren.Length > 0)
            {
                DialogResult nres = MessageBox.Show("Are you sure to close all the open forms?", vumess, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (nres == DialogResult.Yes)
                {
                    foreach (Form childForm in MdiChildren)
                    {
                        childForm.Close();
                    }
                }
                else
                    return;
            }
        }

        private void emailSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //frmEmailSettings m_obj_frmemailsettings = new frmEmailSettings(companyID);
            frmEmailSettings m_obj_frmemailsettings = new frmEmailSettings(companyID,connectionstring);//added by satish pal--hi
            //companynm
            m_obj_frmemailsettings.MdiParent = this;
            m_obj_frmemailsettings.WindowState = FormWindowState.Normal;
            //m_obj_frmemailsettings.CompanyID = companyID;
            m_obj_frmemailsettings.Show();
        }

        private void emailClientWizardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEmailClient m_obj_frmemailclient = new frmEmailClient(companyID,connectionstring);
            m_obj_frmemailclient.MdiParent = this;
            m_obj_frmemailclient.WindowState = FormWindowState.Maximized;
            m_obj_frmemailclient.CompanyID = companyID;
            m_obj_frmemailclient.Show();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (MdiChildren.Length > 0)
                {
                    string childformtext = "", msg = "";
                    int childcount = 0;
                    foreach (Form childform in MdiChildren)
                    {
                        childformtext = (childformtext != string.Empty ? childformtext + "," : childformtext) + childform.Text.ToString();
                        childcount++;
                    }
                    if (childcount == 1)
                        //msg = "The " + childformtext.ToString() + " Form is Open.\nClose the form and then exit.";
                        msg = "The Form is Open.\nClose the form and then exit.";
                    else
                        //msg = "The " + childformtext.ToString() + " Forms are Open.\nClose the forms and then exit.";
                        msg = "The Forms are Open.\nClose the forms and then exit.";
                    MessageBox.Show(msg.ToString(), vumess, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    e.Cancel = true;
                }
                else
                {
                    if (e.CloseReason == CloseReason.UserClosing)
                    {
                        if (isExit || !isExit)
                        {
                            DialogResult res = MessageBox.Show("Are you sure to exit iTAX Email Client?", vumess, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                            if (res == DialogResult.Yes)
                            {
                                Dispose(true);
                                Application.Exit();
                            }
                            else
                                e.Cancel = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        ///satish pal-Satrt

        ///Satish Pal-End
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            ///Satish Pal--Start

            
       
            ///Satish Pal -End
        }

        private void cascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void tileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void tileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }
        #endregion
    }
}
