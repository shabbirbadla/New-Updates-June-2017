using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI;

namespace SQLEditor
{
	/// <summary>
	/// Summary description for FrmBaseContent.
	/// </summary>
	public class FrmBaseContent : DockContent
	{
		public Form MdiParentForm = null;
		private string _defaultHelpUrl = "";
        private string _helpConnectingUrl = "";
    
		public string DefaultHelpUrl
		{
			get
			{
				if(_helpConnectingUrl.Length==0)
					_helpConnectingUrl= "";

				return String.Format(_defaultHelpUrl,Application.StartupPath)+_helpConnectingUrl;
			}
			set {_helpConnectingUrl=value;}
		}

		#region Default
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FrmBaseContent()
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
            this.SuspendLayout();
            // 
            // FrmBaseContent
            // 
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Name = "FrmBaseContent";
            this.Text = "FrmBaseContent";
            this.ResumeLayout(false);

		}
		#endregion
		#endregion
	}
}
