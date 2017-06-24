using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using QueryCommander.WinGui.Base;


namespace QueryCommander
{
	public class FrmSplash : FrmPerPixelAlpha 
	{
		private Button btnClose = new Button();
		public bool OpenForDonation;
		private int formWidth = 100;   
		public int FormWidth   
		{
			get 
			{
				return formWidth; 
			}
		}

		private int formHeight = 100;   
		public int FormHeight  
		{
			get 
			{
				return formHeight; 
			}
		}
		
		public FrmSplash() 
		{
			Text = "PerPixelAlpha Layered Form";
			TopMost = true;
			FormBorderStyle = FormBorderStyle.FixedDialog;
			MinimizeBox = false;
			MaximizeBox = false;
			ControlBox = false;
			Width = formWidth;
			Height = formHeight;
			ShowInTaskbar = false;
			this.AcceptButton=this.btnClose;
			this.CancelButton=this.btnClose;

			this.Click+=new EventHandler(FrmSplash_Click);
			this.MouseDown+=new MouseEventHandler(FrmSplash_MouseDown);
			this.btnClose.Click+=new EventHandler(btnClose_Click);
		}


		// Let Windows drag this window for us
		protected override void WndProc(ref Message m) 
		{
			
			if (m.Msg == 0x0084 /*WM_NCHITTEST*/) 
			{
				m.Result= (IntPtr)2;	// HTCLIENT
				
				if( ((Cursor.Position.X - this.Left) > 380 &
					(Cursor.Position.X - this.Left) < 500 &
					(Cursor.Position.Y - this.Top) > 405 &
					(Cursor.Position.Y - this.Top) < 445 ) ||
					((Cursor.Position.X - this.Left) > 165 &
					(Cursor.Position.X - this.Left) < 360 &
					(Cursor.Position.Y - this.Top) > 335 &
					(Cursor.Position.Y - this.Top) < 350 ))
				{
					this.Cursor=Cursors.Hand;
				}
				else
				{
					this.Cursor=Cursors.Default;
					Bitmap btm = new Bitmap(typeof(FrmSplash), "Embedded.qcSplash1.png");
					this.SetBitmap(btm,255);
					Invalidate();
					btm.Dispose();

				}
			}
		

			base.WndProc(ref m);
		}

		private void FrmSplash_Click(object sender, EventArgs e)
		{
			if( (Cursor.Position.X - this.Left) > 380 &
				(Cursor.Position.X - this.Left) < 500 &
				(Cursor.Position.Y - this.Top) > 405 &
				(Cursor.Position.Y - this.Top) < 445 )
			{
				
				this.Close();
				
			}
			if((Cursor.Position.X - this.Left) > 165 &
				(Cursor.Position.X - this.Left) < 360 &
				(Cursor.Position.Y - this.Top) > 335 &
				(Cursor.Position.Y - this.Top) < 350 )
			{
				string url="http://www.plan-international.org/";
				System.Diagnostics.Process.Start("IExplore.exe",url);
				OpenForDonation=true;
				this.Close();
			}
		}

		private void FrmSplash_MouseDown(object sender, MouseEventArgs e)
		{
			if( (Cursor.Position.X - this.Left) > 380 &
				(Cursor.Position.X - this.Left) < 500 &
				(Cursor.Position.Y - this.Top) > 405 &
				(Cursor.Position.Y - this.Top) < 445 )
			{
				Bitmap btm = new Bitmap(typeof(FrmSplash), "Embedded.qcSplash2.png");
				this.SetBitmap(btm,255);
				Invalidate();
				btm.Dispose();
			}
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			Bitmap btm = new Bitmap(typeof(FrmSplash), "Embedded.qcSplash2.png");
			this.SetBitmap(btm,255);
			Invalidate();
			btm.Dispose();
			this.Close();
		}
	}
}


