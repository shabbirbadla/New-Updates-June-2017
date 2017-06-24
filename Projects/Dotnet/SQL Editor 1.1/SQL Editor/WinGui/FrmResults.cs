
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI;

namespace SQLEditor
{
	/// <summary>
	/// Summary description for FrmResults
	/// </summary>
	public class FrmResults : FrmBaseContent
	{
		public string ReferencedObject = "";
//		private System.Windows.Forms.Button btnOK;
//		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.ListView lstObject;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.ComponentModel.IContainer components;

		public FrmResults(Form mdiParentForm)
		{
			this.MdiParentForm=mdiParentForm;
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			this.DockableAreas = ((WeifenLuo.WinFormsUI.DockAreas)(((WeifenLuo.WinFormsUI.DockAreas.Float 
				| WeifenLuo.WinFormsUI.DockAreas.DockBottom))));

			this.DefaultHelpUrl="::/Goto%20reference.htm";

		}

		public void ShowResults( ArrayList objects, string objectName )
		{
			//
			// Required for Windows Form Designer support
			//
			
			this.columnHeader1.Text = "Objects using [" + objectName + "]";

			lstObject.Items.Clear();

			foreach(Database.DBObject dbObject in objects)
			{
				switch(dbObject.Type.ToUpper())
				{
					case "V ": //Tables
						lstObject.Items.Add(dbObject.Name,4);
						break;
					case "U ": //Tables
						lstObject.Items.Add(dbObject.Name,4);
						break;
					case "P ": //Stored Procedures
						lstObject.Items.Add(dbObject.Name,1);
						break;
					case "FN": //Functions
						lstObject.Items.Add(dbObject.Name,0);
						break;
					case "TF": //Functions
						lstObject.Items.Add(dbObject.Name,0);
						break;
					default:
						lstObject.Items.Add(dbObject.Name,2);
						break;
				}
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
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FrmResults));
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.lstObject = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.SuspendLayout();
			// 
			// imageList1
			// 
			this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// lstObject
			// 
			this.lstObject.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						this.columnHeader1});
			this.lstObject.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstObject.LargeImageList = this.imageList1;
			this.lstObject.Location = new System.Drawing.Point(0, 0);
			this.lstObject.MultiSelect = false;
			this.lstObject.Name = "lstObject";
			this.lstObject.Size = new System.Drawing.Size(352, 304);
			this.lstObject.SmallImageList = this.imageList1;
			this.lstObject.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.lstObject.StateImageList = this.imageList1;
			this.lstObject.TabIndex = 7;
			this.lstObject.View = System.Windows.Forms.View.Details;
			this.lstObject.DoubleClick += new System.EventHandler(this.lstObject_DoubleClick);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Referenced object";
			this.columnHeader1.Width = 333;
			// 
			// FrmResults
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(352, 304);
			this.Controls.Add(this.lstObject);
			this.HideOnClose = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FrmResults";
			this.ShowInTaskbar = false;
			this.Text = "Go to reference";
			this.ResumeLayout(false);

		}
		#endregion



		private void lstObject_DoubleClick(object sender, System.EventArgs e)
		{
			ReferencedObject = lstObject.SelectedItems[0].Text;
			DialogResult=DialogResult.OK;
			MainForm frm = (MainForm)MdiParentForm;
			frm.m_FrmDBObjects.CreateConstructorString(this.ReferencedObject);
			
		}

	}
}
