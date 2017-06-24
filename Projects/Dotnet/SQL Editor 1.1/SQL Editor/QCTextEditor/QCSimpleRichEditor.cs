// *******************************************************************************
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// *******************************************************************************
using System;
using System.Windows.Forms;
using tom;
using System.Runtime.InteropServices ; 

namespace QueryCommander
{
	public class QCSimpleRichEditor : RichTextBox
	{
		protected ITextDocument _iTextDocument = null; // OLE interface used to manipulate the control thow method witch has not been exposed in the .Net RichEditControl
		protected IntPtr IRichEditOlePtr = IntPtr.Zero;
		private void GetTextDocumentInterface()
		{
			if (_iTextDocument == null)
			{
				IntPtr ptr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(IntPtr)));	// Alloc the ptr.
				Marshal.WriteIntPtr(ptr, IntPtr.Zero);
				try
				{
					if (0 != API.SendMessage(this.Handle, Messages.EM_GETOLEINTERFACE, IntPtr.Zero, ptr))
					{
						// Read the returned pointer.
						IntPtr pRichEdit = Marshal.ReadIntPtr(ptr);
						try
						{
							if (pRichEdit != IntPtr.Zero)
							{
								// Query for the ITextDocument interface.
								//Guid guid = new Guid("8CC497C0-A1DF-11CE-8098-00AA0047BE5D");
								Guid guid = new Guid("8CC497C0-A1DF-11CE-8098-00AA0047BE5D");
								Marshal.QueryInterface(pRichEdit, ref guid, out this.IRichEditOlePtr);

								_iTextDocument = (ITextDocument)Marshal.GetTypedObjectForIUnknown(this.IRichEditOlePtr, typeof(ITextDocument));
								if (_iTextDocument == null)
								{
									throw new Exception("Failed to get the object wrapper for the interface.");
								}
							}
							else
							{
								throw new Exception("Failed to get the pointer.");
							}
						}
						finally
						{
							Marshal.Release(pRichEdit);
						}
					}
					else
					{
						throw new Exception("EM_GETOLEINTERFACE failed.");
					}
				}
				catch 
				{
					this.ReleaseTextDocumentInterface();
				}
				finally
				{
					// Free the ptr memory.
					Marshal.FreeCoTaskMem(ptr);
				}
			}
		}

		private void ReleaseTextDocumentInterface()
		{
			if (this.IRichEditOlePtr != IntPtr.Zero)
			{
				Marshal.Release(this.IRichEditOlePtr);
			}

			this.IRichEditOlePtr = IntPtr.Zero;
			_iTextDocument = null;
		}

		public int GetOleObjectForeColor()
		{
			if(_iTextDocument==null)
				GetTextDocumentInterface();

			return _iTextDocument.Range(0,this.Text.Length).Font.ForeColor;
		}
	}
}
