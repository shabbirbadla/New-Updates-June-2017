using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SQLEditor
{
	/// <summary>
	/// Summary description for FrmChooseXMLFile.
	/// </summary>
	public class FrmChooseXMLFile : FrmBaseDialog
	{
		public string FileName="";
		public bool CreateKeys;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.CheckBox chbCreateKeys;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtFileName;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox2;
		public System.Windows.Forms.RadioButton rbStructure;
		public System.Windows.Forms.RadioButton rbData;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FrmChooseXMLFile()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.chbCreateKeys = new System.Windows.Forms.CheckBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbData = new System.Windows.Forms.RadioButton();
            this.rbStructure = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(16, 24);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(320, 20);
            this.txtFileName.TabIndex = 0;
            // 
            // btnBrowse
            // 
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnBrowse.Location = new System.Drawing.Point(344, 24);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(64, 23);
            this.btnBrowse.TabIndex = 3;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // chbCreateKeys
            // 
            this.chbCreateKeys.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chbCreateKeys.Location = new System.Drawing.Point(16, 56);
            this.chbCreateKeys.Name = "chbCreateKeys";
            this.chbCreateKeys.Size = new System.Drawing.Size(232, 24);
            this.chbCreateKeys.TabIndex = 4;
            this.chbCreateKeys.Text = "Create synthetic ID\'s and foreign keys.";
            // 
            // btnOk
            // 
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOk.Location = new System.Drawing.Point(257, 275);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "Ok";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(345, 275);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.chbCreateKeys);
            this.groupBox1.Controls.Add(this.btnBrowse);
            this.groupBox1.Controls.Add(this.txtFileName);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Location = new System.Drawing.Point(4, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(416, 152);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select xml-file";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(40, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(352, 56);
            this.label1.TabIndex = 5;
            this.label1.Text = "If the the xml elements doesn\'t include a unique id and parent id, this can be cr" +
                "eated for you by selecting the \"Create synthetic ID\'s and foreign keys\" checkbox" +
                ".";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbData);
            this.groupBox2.Controls.Add(this.rbStructure);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Location = new System.Drawing.Point(4, 165);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(416, 104);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Action";
            // 
            // rbData
            // 
            this.rbData.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbData.Location = new System.Drawing.Point(24, 56);
            this.rbData.Name = "rbData";
            this.rbData.Size = new System.Drawing.Size(296, 24);
            this.rbData.TabIndex = 1;
            this.rbData.Text = "Create insert script from xml data ";
            // 
            // rbStructure
            // 
            this.rbStructure.Checked = true;
            this.rbStructure.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbStructure.Location = new System.Drawing.Point(24, 24);
            this.rbStructure.Name = "rbStructure";
            this.rbStructure.Size = new System.Drawing.Size(296, 24);
            this.rbStructure.TabIndex = 0;
            this.rbStructure.TabStop = true;
            this.rbStructure.Text = "Create table structure script";
            // 
            // FrmChooseXMLFile
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(425, 305);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Name = "FrmChooseXMLFile";
            this.Text = "Import data";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			this.FileName= this.txtFileName.Text;
			if(chbCreateKeys.Checked)
				CreateKeys=true;

			this.DialogResult=DialogResult.OK;
			this.Close();
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult=DialogResult.Cancel;
			this.Close();
		}

		private void btnBrowse_Click(object sender, System.EventArgs e)
		{
//			string filePath="";
			OpenFileDialog fd = new OpenFileDialog();
			fd.Filter ="(*.xml)|*.xml";
			if(fd.ShowDialog()==DialogResult.OK)
			{
				txtFileName.Text=fd.FileName;	
			}
		}
	}
}
