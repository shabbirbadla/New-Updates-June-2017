using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using SQLEditor.Database;
using WeifenLuo.WinFormsUI;

namespace SQLEditor
{
    /// <summary>
    /// Summary description for FrmSearch.
    /// </summary>
    public class FrmDatabase : FrmBaseContent ///System.Windows.Forms.Form
    {
        private System.Windows.Forms.Label LblCompany;
        public System.Windows.Forms.Button btn_Use;
        private System.Windows.Forms.ComboBox cboCompany;
        //MdiParentForm;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        public FrmDatabase(Form MdiParentForm)
        {
            this.MdiParentForm = MdiParentForm;
            //MdiParentForm = MdiParentForm;
            //Form MdiParentForm
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            //MdiParentForm = MdiParentForm;
            ////mainForm = (MainForm)frmQuery.MdiParentForm;
            ////mainForm.menuItemFindNext.Visible = true;
            AddCombo();
            this.Focus();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        private void AddCombo()
        {
            //MainForm frm = (MainForm);
            MainForm frm = (MainForm)MdiParentForm;
            string dbName = string.Empty;
            MainForm.DBConnection dbConnection = null;
            ArrayList DatabaseObjects = new ArrayList();
            Cursor.Current = Cursors.WaitCursor;
            foreach (MainForm.DBConnection c in frm.DBConnections)
            {
                dbConnection = c;
                break;
            }
            if (dbConnection == null)
                return; 
            IDatabaseManager db = DatabaseFactory.CreateNew(dbConnection.Connection);
            //ArrayList allDatabases = new ArrayList();

            ArrayList dbArr = db.GetDatabasesObjects(dbConnection.ConnectionName, dbConnection.Connection);
            foreach (SQLEditor.Database.DB database in dbArr)
            {
                DatabaseObjects.Add(database);
                //allDatabases.Add(database);
            }

            cboCompany.DataSource = DatabaseObjects;

        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.LblCompany = new System.Windows.Forms.Label();
            this.btn_Use = new System.Windows.Forms.Button();
            this.cboCompany = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // LblCompany
            // 
            this.LblCompany.Location = new System.Drawing.Point(16, 14);
            this.LblCompany.Name = "LblCompany";
            this.LblCompany.Size = new System.Drawing.Size(56, 16);
            this.LblCompany.TabIndex = 0;
            this.LblCompany.Text = "Company";
            // 
            // btn_Use
            // 
            this.btn_Use.Enabled = false;
            this.btn_Use.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btn_Use.Location = new System.Drawing.Point(372, 11);
            this.btn_Use.Name = "btn_Use";
            this.btn_Use.Size = new System.Drawing.Size(80, 21);
            this.btn_Use.TabIndex = 2;
            this.btn_Use.Text = "&Use ";
            // 
            // cboCompany
            // 
            this.cboCompany.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCompany.Location = new System.Drawing.Point(78, 11);
            this.cboCompany.Name = "cboCompany";
            this.cboCompany.Size = new System.Drawing.Size(288, 21);
            this.cboCompany.TabIndex = 0;
            // 
            // FrmDatabase
            // 
            this.AcceptButton = this.btn_Use;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(464, 44);
            this.ControlBox = false;
            this.Controls.Add(this.cboCompany);
            this.Controls.Add(this.btn_Use);
            this.Controls.Add(this.LblCompany);
            this.Name = "FrmDatabase";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Company Database";
            this.TopMost = true;
            this.ResumeLayout(false);

        }
        #endregion
    }
}

