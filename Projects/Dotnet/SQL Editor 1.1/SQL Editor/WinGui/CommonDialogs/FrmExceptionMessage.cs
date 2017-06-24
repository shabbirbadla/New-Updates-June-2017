using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SQLEditor
{

	/// <summary>
	///  Exception dialog
	/// </summary>
	/// <revision author="wmmabet" date="2004-01-13">Removed warning/fixed xml-comments.</revision>
	public class FrmExceptionMessage :  System.Windows.Forms.Form
	{
		private object currentForm = null;
		private Exception exception = null;
		private bool _continue;

		private System.Windows.Forms.TextBox TxtDetail;
		private System.Windows.Forms.Button BtnQuit;
		private System.Windows.Forms.Button BtnContinue;
		private System.Windows.Forms.Button BtnDetails;
		private System.Windows.Forms.Label LblMessage;
        private System.Windows.Forms.ImageList ImgList1;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.ComponentModel.IContainer components;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="currentForm">Current form</param>
		/// <param name="e">Exception</param>
		public FrmExceptionMessage(object currentForm, Exception e)
		{
			this.currentForm=currentForm;
			this.exception=e;

			InitializeComponent();
			this.Height=280;
			displayError();

			
			BtnContinue.Focus();
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
            this.TxtDetail = new System.Windows.Forms.TextBox();
            this.BtnQuit = new System.Windows.Forms.Button();
            this.BtnContinue = new System.Windows.Forms.Button();
            this.BtnDetails = new System.Windows.Forms.Button();
            this.LblMessage = new System.Windows.Forms.Label();
            this.ImgList1 = new System.Windows.Forms.ImageList(this.components);
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // TxtDetail
            // 
            this.TxtDetail.Location = new System.Drawing.Point(8, 84);
            this.TxtDetail.Multiline = true;
            this.TxtDetail.Name = "TxtDetail";
            this.TxtDetail.ReadOnly = true;
            this.TxtDetail.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TxtDetail.Size = new System.Drawing.Size(424, 192);
            this.TxtDetail.TabIndex = 11;
            this.TxtDetail.Text = "textBox1";
            this.TxtDetail.WordWrap = false;
            // 
            // BtnQuit
            // 
            this.BtnQuit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BtnQuit.Location = new System.Drawing.Point(328, 12);
            this.BtnQuit.Name = "BtnQuit";
            this.BtnQuit.Size = new System.Drawing.Size(96, 24);
            this.BtnQuit.TabIndex = 2;
            this.BtnQuit.Text = "Close";
            this.BtnQuit.Click += new System.EventHandler(this.BtnQuit_Click);
            // 
            // BtnContinue
            // 
            this.BtnContinue.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BtnContinue.Location = new System.Drawing.Point(216, 12);
            this.BtnContinue.Name = "BtnContinue";
            this.BtnContinue.Size = new System.Drawing.Size(96, 24);
            this.BtnContinue.TabIndex = 0;
            this.BtnContinue.Text = "Continue";
            this.BtnContinue.Click += new System.EventHandler(this.BtnContinue_Click);
            // 
            // BtnDetails
            // 
            this.BtnDetails.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BtnDetails.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnDetails.Location = new System.Drawing.Point(8, 12);
            this.BtnDetails.Name = "BtnDetails";
            this.BtnDetails.Size = new System.Drawing.Size(96, 24);
            this.BtnDetails.TabIndex = 1;
            this.BtnDetails.Text = "Details";
            this.BtnDetails.Click += new System.EventHandler(this.BtnDetails_Click);
            // 
            // LblMessage
            // 
            this.LblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblMessage.Location = new System.Drawing.Point(8, 87);
            this.LblMessage.Name = "LblMessage";
            this.LblMessage.Size = new System.Drawing.Size(424, 64);
            this.LblMessage.TabIndex = 0;
            this.LblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ImgList1
            // 
            this.ImgList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.ImgList1.ImageSize = new System.Drawing.Size(9, 6);
            this.ImgList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // linkLabel1
            // 
            this.linkLabel1.Location = new System.Drawing.Point(5, 48);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(280, 16);
            this.linkLabel1.TabIndex = 18;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Please report this bug";
            this.linkLabel1.Visible = false;
            // 
            // FrmExceptionMessage
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(439, 284);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.TxtDetail);
            this.Controls.Add(this.BtnQuit);
            this.Controls.Add(this.BtnContinue);
            this.Controls.Add(this.BtnDetails);
            this.Controls.Add(this.LblMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmExceptionMessage";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SQLEditor";
            this.Load += new System.EventHandler(this.FrmException_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		
		
		#endregion


		private void BtnDetails_Click(object sender, System.EventArgs e)
		{
			this.Height=(this.Height==496)?280:496;
		}

		private void BtnQuit_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}

		private void FrmException_Load(object sender, System.EventArgs e)
		{
			if(this.exception is System.Data.SqlClient.SqlException ||
				this.exception is SQLEditor.Database.MinorException)
			{
				_continue=true;
				BtnQuit.Visible=false;
				BtnContinue.Location = BtnQuit.Location;
				linkLabel1.Visible=false;
				this.AcceptButton=BtnContinue;
				this.CancelButton=BtnContinue;
			}
			this.Refresh();
		}
		private void displayError()
		{
			string Message="";
			string Detail="";
			Cursor.Current = Cursors.Default;
			
			//BtnDetails.Image=ImgList1.Images[0];
			Detail = (exception.InnerException==null)?exception.StackTrace.ToString():exception.InnerException.ToString();
			Message= exception.Message;
			
			
			TxtDetail.Lines=formatMessage(Detail);
			int numberOfBreaks = Message.Split('\n').Length-1;
			
			LblMessage.Text=String.Format(Message);

			LblMessage.Height = LblMessage.Height + (numberOfBreaks * 5);
			this.Height = this.Height + (numberOfBreaks * 5);
			TxtDetail.Top = TxtDetail.Top + (numberOfBreaks * 5);
			//PnlButtons.Top = PnlButtons.Top + (numberOfBreaks * 5);
			
			this.Focus();
		}

			
		private void SetCaption(string text)
		{
			this.Text = text + " - " + exception.GetType().Name;
		}

		private void BtnContinue_Click(object sender, System.EventArgs e)
		{
			if(!_continue)
				((Form)currentForm).Close();
	
			this.Close();
		}

		private string[] formatMessage(string message)
		{
			// Formatts an string, and returns a string array.
			// Delimiter=\n
			return message.Split('\n');
		}

		private void Cancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

//////        private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
//////        {
//////            try
//////            {
//////                string targetUriFormat="https://sourceforge.net/tracker/index.php?group_id=123942&group_id=123942&atid=744782&func=postadd&category_id=100&artifact_group_id=100&assigned_to=100&priority=6&summary={0}&details={1}";
//////                string msgFormat = "DateTime: {0}\nSQLEditor Version: {1}\nUser message:\n{2}\n\nStack trace:{3}";
//////                FrmExtendExceptionMessage msgFrm = new FrmExtendExceptionMessage();
//////                msgFrm.ShowDialogWindow(this);
//////                if(msgFrm.txtMessage.Text.Length==0)
//////                {
//////                    MessageBox.Show(Localization.GetString("FrmExceptionMessage.NoMessage"),"SQLEditor");
//////                    return;
//////                }
//////                string detail = String.Format(msgFormat,
//////                    DateTime.Now,
//////                    System.Windows.Forms.Application.ProductVersion,
//////                    msgFrm.txtMessage.Text,
//////                    TxtDetail.Text);

//////                string url = String.Format(targetUriFormat,LblMessage.Text,detail);
//////                System.Net.WebClient webClient = new System.Net.WebClient();
//////                webClient.DownloadData(url);
//////            }
//////            catch(Exception ex)
//////            {
//////                ////string body = "Exception report:" + DateTime.Now + "%0D%0APlease add some comments to this exception.%0D%0A%0D%0ASQLEditor Version:%0D%0A"+System.Windows.Forms.Application.ProductVersion+"%0D%0A%0D%0AMessage:%0D%0A" + LblMessage.Text+"%0D%0A%0D%0AStack trace:" +TxtDetail.Text.Replace("at ","%0D%0Aat ");
//////                ////string target = "mailto:qcsupport@rockwolf.com?subject=SQLEditor Bugreport&body=" + body;//"http://workspaces.gotdotnet.com/SQLEditor";
//////                ////System.Diagnostics.Process.Start(target);
//////            }
////////			string body = "Exception report:" + DateTime.Now + "%0D%0APlease add some comments to this exception.%0D%0A%0D%0ASQLEditor Version:%0D%0A"+System.Windows.Forms.Application.ProductVersion+"%0D%0A%0D%0AMessage:%0D%0A" + LblMessage.Text+"%0D%0A%0D%0AStack trace:" +TxtDetail.Text.Replace("at ","%0D%0Aat ");
////////			string target = "mailto:qcsupport@rockwolf.com?subject=SQLEditor Bugreport&body=" + body;//"http://workspaces.gotdotnet.com/SQLEditor";
////////			System.Diagnostics.Process.Start(target);
//////        }

	}
}
