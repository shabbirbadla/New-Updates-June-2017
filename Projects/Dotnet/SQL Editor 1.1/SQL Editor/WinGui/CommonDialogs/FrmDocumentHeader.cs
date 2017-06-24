
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;

namespace SQLEditor
{
	/// <summary>
	/// Summary description for FrmDocumentHeader.
	/// </summary>
	public class FrmDocumentHeader : FrmBaseDialog
	{
		public string Header="";
		XmlDocument _doc = new XmlDocument();
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtSummary;
		private System.Windows.Forms.TextBox txtRevision;
		private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
		public System.Windows.Forms.CheckBox chbShowFrmDocumentHeader;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FrmDocumentHeader(string header)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			_doc.LoadXml(header);
			XmlNodeList nList =  _doc.GetElementsByTagName("summary");
			txtSummary.Text = nList[0].InnerText;
			nList =  _doc.GetElementsByTagName("revision");
			txtRevision.Text = nList[nList.Count-1].InnerText;
			
			
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDocumentHeader));
            this.txtSummary = new System.Windows.Forms.TextBox();
            this.txtRevision = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.chbShowFrmDocumentHeader = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // txtSummary
            // 
            this.txtSummary.Location = new System.Drawing.Point(9, 25);
            this.txtSummary.Multiline = true;
            this.txtSummary.Name = "txtSummary";
            this.txtSummary.Size = new System.Drawing.Size(384, 64);
            this.txtSummary.TabIndex = 1;
            // 
            // txtRevision
            // 
            this.txtRevision.Location = new System.Drawing.Point(9, 121);
            this.txtRevision.Multiline = true;
            this.txtRevision.Name = "txtRevision";
            this.txtRevision.Size = new System.Drawing.Size(384, 72);
            this.txtRevision.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(144, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "Revision";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(9, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "Summary:";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(321, 201);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 24);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Skip";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(233, 201);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 24);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // chbShowFrmDocumentHeader
            // 
            this.chbShowFrmDocumentHeader.Location = new System.Drawing.Point(9, 201);
            this.chbShowFrmDocumentHeader.Name = "chbShowFrmDocumentHeader";
            this.chbShowFrmDocumentHeader.Size = new System.Drawing.Size(200, 24);
            this.chbShowFrmDocumentHeader.TabIndex = 0;
            this.chbShowFrmDocumentHeader.Text = "Don\'t show this window";
            this.chbShowFrmDocumentHeader.CheckedChanged += new System.EventHandler(this.chbShowFrmDocumentHeader_CheckedChanged);
            // 
            // FrmDocumentHeader
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(400, 233);
            this.Controls.Add(this.chbShowFrmDocumentHeader);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtRevision);
            this.Controls.Add(this.txtSummary);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDocumentHeader";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Documentation header";
            this.Load += new System.EventHandler(this.FrmDocumentHeader_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void FrmDocumentHeader_Load(object sender, System.EventArgs e)
		{
		
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.OK;
			XmlNodeList nList =  _doc.GetElementsByTagName("summary");
			nList[0].InnerText = txtSummary.Text;
			nList =  _doc.GetElementsByTagName("revision");
			nList[nList.Count-1].InnerText = txtRevision.Text;
			Header = _doc.InnerXml;
			Header = Header.Replace("<summary>","\n\t<summary>");
			Header = Header.Replace("<revision ","\n\t<revision ");
			Header = Header.Replace("<param ","\n\t<param ");
			Header = Header.Replace("</member>","\n</member>");
			
			SQLEditor.Config.Settings settings = SQLEditor.Config.Settings.Load();
			if(settings.Exists())
			{
				if(chbShowFrmDocumentHeader.Checked)
					settings.ShowFrmDocumentHeader = false;
				else
					settings.ShowFrmDocumentHeader = true;

				settings.Save();
			}

			this.Close();
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void chbShowFrmDocumentHeader_CheckedChanged(object sender, System.EventArgs e)
		{
		
		}
	}
}
