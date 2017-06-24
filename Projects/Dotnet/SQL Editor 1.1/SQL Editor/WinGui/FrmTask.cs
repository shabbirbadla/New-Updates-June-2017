
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI;

namespace SQLEditor
{
	/// <summary>
	/// Summary description for DummyPropertyGrid.
	/// </summary>
	public class FrmTask : FrmBaseContent
	{
		private System.Windows.Forms.RichTextBox TxtTasks;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FrmTask(Form mdiParentForm)
		{
			this.MdiParentForm=mdiParentForm;
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.DockableAreas = ((WeifenLuo.WinFormsUI.DockAreas)(((WeifenLuo.WinFormsUI.DockAreas.Float 
				| WeifenLuo.WinFormsUI.DockAreas.DockBottom))));
			
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FrmTask));
			this.TxtTasks = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// TxtTasks
			// 
			this.TxtTasks.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TxtTasks.Location = new System.Drawing.Point(0, 2);
			this.TxtTasks.Name = "TxtTasks";
			this.TxtTasks.Size = new System.Drawing.Size(280, 345);
			this.TxtTasks.TabIndex = 0;
			this.TxtTasks.Text = "";
			// 
			// FrmTask
			// 
			this.DockableAreas = ((WeifenLuo.WinFormsUI.DockAreas)(((((WeifenLuo.WinFormsUI.DockAreas.Float | WeifenLuo.WinFormsUI.DockAreas.DockLeft) 
				| WeifenLuo.WinFormsUI.DockAreas.DockRight) 
				| WeifenLuo.WinFormsUI.DockAreas.DockTop) 
				| WeifenLuo.WinFormsUI.DockAreas.DockBottom)));
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(280, 349);
			this.CloseButton = false;
			this.Controls.Add(this.TxtTasks);
			this.DockPadding.Bottom = 2;
			this.DockPadding.Top = 2;
			this.HideOnClose = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FrmTask";
			this.ShowHint = WeifenLuo.WinFormsUI.DockState.DockBottomAutoHide;
			this.Text = "Task List";
			this.ResumeLayout(false);

		}
		#endregion

		public void ApplyTask(string text)
		{
			TxtTasks.Text = text;
		}
	}
}
