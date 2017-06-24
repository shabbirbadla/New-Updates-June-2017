using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using SQLEditor.General;

namespace SQLEditor.WinGui.Base
{
	/// <summary>
	/// Summary description for FrmPerPixelAlpha.
	/// </summary>
	public class FrmPerPixelAlpha : System.Windows.Forms.Form
	{
		/// <para>Changes the current bitmap.</para>
		public void SetBitmap(Bitmap bitmap) 
		{
			SetBitmap(bitmap, 255);
		}

		/// <para>Changes the current bitmap with a custom opacity level.  Here is where all happens!</para>
		public void SetBitmap(Bitmap bitmap, byte opacity) 
		{
			if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
				throw new ApplicationException("The bitmap must be 32ppp with alpha-channel.");

			
			IntPtr screenDc = Win32.GetDC(IntPtr.Zero);
			IntPtr memDc = Win32.CreateCompatibleDC(screenDc);
			IntPtr hBitmap = IntPtr.Zero;
			IntPtr oldBitmap = IntPtr.Zero;

			try 
			{
				hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));  // grab a GDI handle from this GDI+ bitmap
				oldBitmap = Win32.SelectObject(memDc, hBitmap);

				Win32.Size size				= new Win32.Size(bitmap.Width, bitmap.Height);
				Win32.Point pointSource		= new Win32.Point(0, 0);
				Win32.Point topPos			= new Win32.Point(Left, Top);
				Win32.BLENDFUNCTION blend	= new Win32.BLENDFUNCTION();
				blend.BlendOp				= Win32.AC_SRC_OVER;
				blend.BlendFlags			= 0;
				blend.SourceConstantAlpha	= opacity;
				blend.AlphaFormat			= Win32.AC_SRC_ALPHA;

				Win32.UpdateLayeredWindow(Handle, screenDc, ref topPos, ref size, memDc, ref pointSource, 0, ref blend, Win32.ULW_ALPHA);
			}
			finally 
			{
				Win32.ReleaseDC(IntPtr.Zero, screenDc);
				if (hBitmap != IntPtr.Zero) 
				{
					Win32.SelectObject(memDc, oldBitmap);
					//Windows.DeleteObject(hBitmap); // The documentation says that we have to use the Windows.DeleteObject... but since there is no such method I use the normal DeleteObject from Win32 GDI and it's working fine without any resource leak.
					Win32.DeleteObject(hBitmap);
				}
				Win32.DeleteDC(memDc);
			}
		}

		protected override CreateParams CreateParams	
		{
			get 
			{
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= 0x00080000; // This form has to have the WS_EX_LAYERED extended style
				return cp;
			}
		}
	}

}
