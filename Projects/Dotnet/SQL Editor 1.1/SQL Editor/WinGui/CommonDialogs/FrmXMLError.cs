
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SQLEditor
{
	/// <summary>
	/// Summary description for FrmXMLError.
	/// </summary>
	public class FrmXMLError : System.Windows.Forms.Form
	{

		private System.Windows.Forms.RichTextBox txtXML;
		private System.Windows.Forms.Label lblError;
		private System.Windows.Forms.Button btnOk;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FrmXMLError(string errorText, string xmlText, int line)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			lblError.Text = errorText;
			txtXML.Text = xmlText;
			try
			{
				GoToLine(line);
			}
			catch
			{
				return;
			}
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
            this.txtXML = new System.Windows.Forms.RichTextBox();
            this.lblError = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtXML
            // 
            this.txtXML.Location = new System.Drawing.Point(8, 39);
            this.txtXML.Name = "txtXML";
            this.txtXML.Size = new System.Drawing.Size(648, 242);
            this.txtXML.TabIndex = 0;
            this.txtXML.Text = "richTextBox1";
            // 
            // lblError
            // 
            this.lblError.Location = new System.Drawing.Point(8, 8);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(648, 28);
            this.lblError.TabIndex = 1;
            this.lblError.Text = "label1";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(576, 287);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 24);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "OK";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // FrmXMLError
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(664, 318);
            this.ControlBox = false;
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.txtXML);
            this.Name = "FrmXMLError";
            this.Text = "XML Error";
            this.ResumeLayout(false);

		}
		#endregion
		public void GoToLine(int line)
		{
			int pos=0;
			
			for(int i=0;i<txtXML.Lines.Length;i++)
			{
				if(i==line)
					break;
				pos+=txtXML.Lines[i].Length;
				
			}
			Point pt = txtXML.GetPositionFromCharIndex(pos);
			pt.X = 0;
			int startPos	=   txtXML.GetCharIndexFromPosition(pt);
			txtXML.SelectionStart = startPos;
		}

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}
	}
}
