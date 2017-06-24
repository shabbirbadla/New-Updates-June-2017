using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;

namespace QueryCommander
{
	/// <summary>
	/// Summary description for FrmAddToSnippet.
	/// </summary>
	public class FrmAddToSnippet : FrmBaseDialog
    {
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtCaption;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.TextBox qcEditor;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FrmAddToSnippet(string text)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			qcEditor.Text=text;
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtCaption = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.qcEditor = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(4, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Caption:";
            // 
            // txtCaption
            // 
            this.txtCaption.Location = new System.Drawing.Point(68, 10);
            this.txtCaption.Name = "txtCaption";
            this.txtCaption.Size = new System.Drawing.Size(320, 20);
            this.txtCaption.TabIndex = 2;
            // 
            // btnOk
            // 
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOk.Location = new System.Drawing.Point(235, 288);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(72, 24);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "Ok";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(315, 288);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 24);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            // 
            // qcEditor
            // 
            this.qcEditor.Location = new System.Drawing.Point(4, 34);
            this.qcEditor.Multiline = true;
            this.qcEditor.Name = "qcEditor";
            this.qcEditor.Size = new System.Drawing.Size(384, 248);
            this.qcEditor.TabIndex = 6;
            this.qcEditor.Text = "textBox1";
            // 
            // FrmAddToSnippet
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(390, 318);
            this.Controls.Add(this.qcEditor);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtCaption);
            this.Controls.Add(this.label1);
            this.Name = "FrmAddToSnippet";
            this.Text = "Manage snippets";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			if(txtCaption.Text.Length==0)
				return;

			XmlDocument xmlSnippets = new XmlDocument();
			xmlSnippets.Load(Application.StartupPath+@"\Snippets.xml");
			XmlNodeList xmlNodeList = xmlSnippets.GetElementsByTagName("snippets");

			foreach(XmlNode node in xmlNodeList[0].ChildNodes)
			{
				if(node.Attributes["name"].Value.ToUpper()==txtCaption.Text.ToUpper())
				{
					MessageBox.Show(Localization.GetString("FrmAddToSnippet.btnOk_Click"));
					return;
				}
			}
			
			XmlNode root = xmlSnippets.DocumentElement;

			//Create a new node.
			XmlElement elem = xmlSnippets.CreateElement("snippet");
			elem.InnerText=qcEditor.Text;
			
			XmlAttribute nameAttr = xmlSnippets.CreateAttribute("name");
			nameAttr.Value = txtCaption.Text;

			elem.Attributes.Append(nameAttr);


			//Add the node to the document.
			root.AppendChild(elem);

			xmlSnippets.Save(Application.StartupPath+@"\Snippets.xml");

			this.Close();
		}
	}
}
