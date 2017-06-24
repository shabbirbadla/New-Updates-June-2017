using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Security.Principal;

namespace WindowsFormsApplication5
{
    public partial class frmRegister_dll : Form
    {
        Icon MainIcon;
        string startupPath = string.Empty;
        string ErrorMsg = string.Empty;
        DataTable _dtFileList;

        public frmRegister_dll()
        {
            InitializeComponent();
        }
        public static void Registar_Dlls(string filePath)
        {
            try
            {
                string arg_fileinfo = filePath;
                Process reg = new Process();
                reg.StartInfo.FileName = "regsvr32.exe";
                reg.StartInfo.Arguments = arg_fileinfo;
                reg.StartInfo.UseShellExecute = false;
                reg.StartInfo.CreateNoWindow = true;
                reg.StartInfo.RedirectStandardOutput = true;
                reg.Start();
                reg.WaitForExit();

                //MessageBox.Show("DLL registration done successfully.");
                reg.Close();
                //Application.Exit();

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }
        public void Checkdll(string ApplicationPath)
        {
            //****** Changed by Sachin N. S. on 17/04/2014 -- Start
            //if (File.Exists(Path.Combine(ApplicationPath, "FATHZIP.DLL")))
            //{
            //    string dllnm = startupPath + "\\FATHZIP.DLL /s";
            //    Registar_Dlls(dllnm);
            //}
            //else
            //{
            //    MessageBox.Show("File not found for View..!!!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}
            
            DataRow _dr;
            for(int i=0; i<_dtFileList.Rows.Count;i++)
            {
                _dr = _dtFileList.Rows[i];
                if (Convert.ToBoolean(_dr[0]) == true)
                {
                    dgvFileList.FirstDisplayedScrollingRowIndex = i;
                    if (i > 0)
                    {
                        dgvFileList.Rows[i - 1].Selected = false;
                    }
                    dgvFileList.Rows[i].Selected = true;
                    //dgvFileList.Rows[i].Cells[0].Selected = true;
                    dgvFileList.PerformLayout();
                    try
                    {
                        if (File.Exists(Path.Combine(ApplicationPath, _dr[1].ToString().Trim())))
                        {
                            string dllnm = startupPath + "\\" + _dr[1].ToString().Trim() + " /s";
                            Registar_Dlls(dllnm);
                            _dr[2] = "Registered";
                            dgvFileList.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                        }
                        else
                        {
                            _dr[2] = "File not found for View..!!!";
                            dgvFileList.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                            //return;
                        }
                    }
                    catch (Exception ex)
                    {
                        _dr[2] = "Not Registered";
                        dgvFileList.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    }
                }
            }
            //******* Changed by Sachin N. S. on 17/04/2014 -- End
        }

        private void btnregister_dll_Click(object sender, EventArgs e)
        {

            if (IsUserAdministrator())
            {
                //Checkdll(startupPath);
                Checkdll(this.txtSelPath.Text.Trim());      // Changed by Sachin N. S. on 17/04/2014
                MessageBox.Show("File registration process is comleted.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DialogResult x = MessageBox.Show("DLL registration should be run with Administrator Rights.\n If you are using Windows Vista or Later Version,\n please exit and select Run as Administrator after Right Clicking the Exe.\n If you continue, the DLL may not be installed successfully.\nContinue ?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (x == DialogResult.Yes)
                {
                    //Checkdll(startupPath);
                    Checkdll(this.txtSelPath.Text.Trim());      // Changed by Sachin N. S. on 17/04/2014
                }
                else
                {
                    Application.Exit();
                }
            }
        }

        private void frmRegister_dll_Load(object sender, EventArgs e)
        {
            startupPath = Application.StartupPath;

            //****** Added by Sachin N. S. on 17/04/2014 for Bug- -- Start
            _dtFileList = new DataTable();
            _dtFileList.Columns.Add(new DataColumn("cSel", typeof(bool)));
            _dtFileList.Columns.Add(new DataColumn("cFileName", typeof(string)));
            _dtFileList.Columns.Add(new DataColumn("cStatus", typeof(string)));

            dgvFileList.AutoGenerateColumns = false;
            dgvFileList.DataSource = _dtFileList;
            dgvFileList.Columns[0].DataPropertyName = "cSel";
            dgvFileList.Columns[1].DataPropertyName = "cFileName";
            dgvFileList.Columns[2].DataPropertyName = "cStatus";
            //****** Added by Sachin N. S. on 17/04/2014 for Bug- -- End
        }

        public bool IsUserAdministrator()
        {
            bool isAdmin;
            try
            {
                WindowsIdentity user = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(user);
                isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (UnauthorizedAccessException ex)
            {
                isAdmin = false;
            }
            catch (Exception ex)
            {
                isAdmin = false;
            }
            return isAdmin;
        }

        //****** Added by Sachin N. S. on 17/04/2014 for Bug- -- Start
        private void btnSelPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog _fd = new FolderBrowserDialog();
            _fd.ShowDialog();
            if (_fd.SelectedPath != null && _fd.SelectedPath != "")
            {
                _dtFileList.Clear();
                this.txtSelPath.Text = _fd.SelectedPath;
                DataRow _dr;
                string[] _fileLst = Directory.GetFiles(_fd.SelectedPath, "*.dll");
                foreach (string _fileNm in _fileLst)
                {
                    _dr = _dtFileList.NewRow();
                    _dr[0] = false;
                    _dr[1] = Path.GetFileName(_fileNm);
                    _dr[2] = "";
                    _dtFileList.Rows.Add(_dr);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkSel_CheckedChanged(object sender, EventArgs e)
        {
            if (_dtFileList.Rows.Count > 0)
            {
                if (chkSel.Checked == true)
                {
                    foreach (DataRow _dr in _dtFileList.Rows)
                    {
                        _dr[0] = true;
                    }
                }
                else
                {
                    foreach (DataRow _dr in _dtFileList.Rows)
                    {
                        _dr[0] = false;
                    }
                }
            }
            else
            {
                this.chkSel.Checked = false;
            }
        }
        //****** Added by Sachin N. S. on 17/04/2014 for Bug- -- End
    }
}
