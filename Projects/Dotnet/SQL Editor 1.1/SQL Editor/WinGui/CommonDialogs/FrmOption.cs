using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using SQLEditor.WinGui.UserControls;
using SQLEditor.Editor;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using SQLEditor.Config;

namespace SQLEditor
{
	public class FrmOption : FrmBaseDialog
	{
        //private UcOptionsEnviroments	optionsEnviroments = new UcOptionsEnviroments();
		private UcOptionEditor			optionEditor = new UcOptionEditor();
		private UcOptionsQuerySettings	optionsQuerySettings = new UcOptionsQuerySettings();
        ////private UcOptionStartUp			optionStartUp = new UcOptionStartUp();

		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Panel UserControlContainer;
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Button btnHelp;
		private MainForm mainForm;
		public FrmOption(MainForm mainForm)
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			this.mainForm=mainForm;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmOption));
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.UserControlContainer = new System.Windows.Forms.Panel();
            this.btnHelp = new System.Windows.Forms.Button();
            ////this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.Location = new System.Drawing.Point(3, 4);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.ShowLines = false;
            this.treeView1.ShowPlusMinus = false;
            this.treeView1.ShowRootLines = false;
            this.treeView1.Size = new System.Drawing.Size(152, 288);
            this.treeView1.TabIndex = 0;
            this.treeView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseUp);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "");
            this.imageList1.Images.SetKeyName(1, "");
            this.imageList1.Images.SetKeyName(2, "");
            this.imageList1.Images.SetKeyName(3, "");
            // 
            // btnOk
            // 
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOk.Location = new System.Drawing.Point(339, 296);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(72, 24);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Ok";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(419, 296);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 24);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            // 
            // UserControlContainer
            // 
            this.UserControlContainer.Location = new System.Drawing.Point(163, 4);
            this.UserControlContainer.Name = "UserControlContainer";
            this.UserControlContainer.Size = new System.Drawing.Size(328, 288);
            this.UserControlContainer.TabIndex = 5;
            // 
            // btnHelp
            // 
            this.btnHelp.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnHelp.Location = new System.Drawing.Point(163, 296);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(72, 24);
            this.btnHelp.TabIndex = 6;
            this.btnHelp.Text = "Help";
            this.btnHelp.Visible = false;
            ////////this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // FrmOption
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(493, 321);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.UserControlContainer);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.treeView1);
            this.Name = "FrmOption";
            this.Text = "Options";
            this.Load += new System.EventHandler(this.FrmOption_Load);
            this.ResumeLayout(false);

		}
		#endregion

		private void FrmOption_Load(object sender, System.EventArgs e)
		{
			InitTreeView();

		}
		private void InitTreeView()
		{
			// Top
			TreeNode tnEnviroment = new TreeNode("Enviroment", 0, 1);
			// Childs
			TreeNode tnEditor = new TreeNode("Editor", 2, 3);
			TreeNode tnQuerySettings = new TreeNode("QuerySettings", 2, 3);
            //TreeNode StartPage = new TreeNode("StartPage", 2, 3);

			tnEnviroment.Nodes.Add(tnEditor);
			tnEnviroment.Nodes.Add(tnQuerySettings);
            //tnEnviroment.Nodes.Add(StartPage);

			this.treeView1.Nodes.Add(tnEnviroment);
			tnEnviroment.Expand();

          //this.UserControlContainer.Controls.Add(optionsEnviroments);
			this.UserControlContainer.Controls.Add(optionEditor);
			this.UserControlContainer.Controls.Add(optionsQuerySettings);
            //this.UserControlContainer.Controls.Add(optionStartUp);

            //ActivateOptionControl(optionsEnviroments);
            ActivateOptionControl(optionEditor);

		}
		private void ActivateOptionControl(System.Windows.Forms.UserControl optionControl)
		{
			foreach(UserControl uc in this.UserControlContainer.Controls)
				uc.Hide();

			optionControl.Show();

//			if(activeOptionControl!=null){
//			activeOptionControl.Hide();}
//			activeOptionControl=optionControl;
//			activeOptionControl.Show();
		}
		private void treeView1_Click(object sender, System.EventArgs e)
		{
			TreeNode selectedNode=treeView1.SelectedNode;
			
			switch(selectedNode.Text)
			{
                ////case "Enviroment":
                ////    ActivateOptionControl(optionsEnviroments);
                ////    break;
				case "Editor":
					ActivateOptionControl(optionEditor);
					break;
				case "QuerySettings":
					ActivateOptionControl(optionsQuerySettings);
					break;
                ////case "StartPage":
                ////    ActivateOptionControl(optionStartUp);
                ////    break;
			}

//			UcOptionsEnviroments optionsEnviroments = new UcOptionsEnviroments();
//			this.UserControlContainer.Controls.Add(optionsEnviroments);
//			optionsEnviroments.Show();
			
		}

		private void treeView1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Left)
			{
				TreeNode selectedNode = this.treeView1.GetNodeAt(e.X,e.Y);
				if(selectedNode != null)
				{
					switch(selectedNode.Text)
					{
                        //case "Enviroment":
                        //    ActivateOptionControl(optionsEnviroments);
                        //    break;
						case "Editor":
							ActivateOptionControl(optionEditor);
							break;
						case "QuerySettings":
							ActivateOptionControl(optionsQuerySettings);
							break;
                        //case "StartPage":
                        //    ActivateOptionControl(optionStartUp);
                        //    break;
					}
				}
			}
		}

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			try
			{
				//Editor settings
				SQLEditor.Config.Settings settings = SQLEditor.Config.Settings.Load();
				settings.ShowEOLMarkers=optionEditor.chbShowEOLMarkers.Checked;
				settings.ShowSpaces=optionEditor.chbShowSpaces.Checked;
				settings.ShowTabs=optionEditor.chbShowTabs.Checked;
				settings.ShowLineNumbers=optionEditor.chbShowLineNumbers.Checked;
				settings.ShowMatchingBracket=optionEditor.chbShowMatchingBrackets.Checked;
				settings.fontFamily=optionEditor.font.FontFamily.Name;
				settings.fontGraphicsUnit=optionEditor.font.Unit;
				settings.fontSize=optionEditor.font.Size;
				settings.fontStyle=optionEditor.font.Style;
				settings.TabIndent= Convert.ToInt16( optionEditor.txtTabIndent.Text);

				//Startup
                ////settings.ShowStartPage=optionStartUp.chbShowStartPage.Checked;
                //////settings.ShowSplash=optionStartUp.chbShowSplash.Checked;

				//QuerySettings
				settings.ReadOnlyOutput=optionsQuerySettings.chbReadOnlyOutput.Checked;
				settings.RunWithIOStatistics=optionsQuerySettings.chbRunWithIOStat.Checked;
				settings.ShowFrmDocumentHeader=optionsQuerySettings.chbShowCommentHeader.Checked;

				settings.Save();
				mainForm.UpdateEditorSettings();
			}
			catch{}
			this.Close();
		}

        ////private void btnHelp_Click(object sender, System.EventArgs e)
        ////{
        ////    mainForm.ShowSQLEditorHelpContext("","::/Options.htm",this.Text,true);
        ////}
	}
}

