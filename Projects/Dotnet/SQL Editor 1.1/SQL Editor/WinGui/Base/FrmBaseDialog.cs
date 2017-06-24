using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SQLEditor
{
	/// <summary>
	/// Summary description for FrmBaseDialog.
	/// </summary>
	public class FrmBaseDialog : System.Windows.Forms.Form
	{
		#region Default
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FrmBaseDialog()
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
			this.components = new System.ComponentModel.Container();
			this.Size = new System.Drawing.Size(300,300);
			this.Text = "FrmBaseDialog";
		}
		#endregion
		#endregion

		public DialogResult ShowDialogWindow(System.Windows.Forms.Form parent)
		{
			DialogResult result = DialogResult.Cancel;
			
			this.Menu = null;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.SizeGripStyle = SizeGripStyle.Hide;
			this.SizeGripStyle = SizeGripStyle.Hide;
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.ShowInTaskbar = false;
			this.StartPosition = FormStartPosition.CenterParent;
			this.ControlBox = false;
			result = this.ShowDialog(parent);
			

			return result;
		}
	}
}
