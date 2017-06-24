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
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.InteropServices ; 
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using tom;


namespace QueryCommander
{
	public class QCRichEditor : RichTextBox
	{
		protected override void OnPaint(PaintEventArgs e)
		{
			//e.ClipRectangle.

			base.OnPaint (e);
		}

		#region Enums
		enum SearchFor{All, StartComment, EndComment,  LineComment, String};
		enum SearchType{Comment, LineComment, String};
		public enum ParameterType{Open,Close};
		/// <summary>
		/// Specifies the style of underline that should be
		/// applied to the text.
		/// </summary>
		public enum UnderlineStyle
		{
			/// <summary>
			/// No underlining.
			/// </summary>
			None = 0,
			/// <summary>
			/// Standard underlining across all words.
			/// </summary>
			Normal = 1,
			/// <summary>
			/// Standard underlining broken between words.
			/// </summary>
			Word = 2,
			/// <summary>
			/// Double line underlining.
			/// </summary>
			Double = 3,
			/// <summary>
			/// Dotted underlining.
			/// </summary>
			Dotted = 4,
			/// <summary>
			/// Dashed underlining.
			/// </summary>
			Dash = 5,
			/// <summary>
			/// Dash-dot ("-.-.") underlining.
			/// </summary>
			DashDot = 6,
			/// <summary>
			/// Dash-dot-dot ("-..-..") underlining.
			/// </summary>
			DashDotDot = 7,
			/// <summary>
			/// Wave underlining (like spelling mistakes in MS Word).
			/// </summary>
			Wave = 8,
			/// <summary>
			/// Extra thick standard underlining.
			/// </summary>
			Thick = 9,
			/// <summary>
			/// Extra thin standard underlining.
			/// </summary>
			HairLine = 10,
			/// <summary>
			/// Double thickness wave underlining.
			/// </summary>
			DoubleWave = 11,
			/// <summary>
			/// Thick wave underlining.
			/// </summary>
			HeavyWave = 12,
			/// <summary>
			/// Extra long dash underlining.
			/// </summary>
			LongDash = 13
		}
		/// <summary>
		/// Specifies the color of underline that should be
		/// applied to the text.
		/// </summary>
		/// <remarks>
		/// I named these colors by their appearance, so some
		/// of them might not be what you expect. Please email
		/// me if you feel one should be changed.
		/// </remarks>
		public enum UnderlineColor
		{
			/// <summary>Black.</summary>
			Black = 0x00,
			/// <summary>Blue.</summary>
			Blue = 0x10,
			/// <summary>Cyan.</summary>
			Cyan = 0x20,
			/// <summary>LimeGreen.</summary>
			LimeGreen = 0x30,
			/// <summary>Magenta.</summary>
			Magenta = 0x40,
			/// <summary>Red.</summary>
			Red = 0x50,
			/// <summary>Yellow.</summary>
			Yellow = 0x60,
			/// <summary>White.</summary>
			White = 0x70,
			/// <summary>DarkBlue.</summary>
			DarkBlue = 0x80,
			/// <summary>DarkCyan.</summary>
			DarkCyan = 0x90,
			/// <summary>Green.</summary>
			Green = 0xA0,
			/// <summary>DarkMagenta.</summary>
			DarkMagenta = 0xB0,
			/// <summary>Brown.</summary>
			Brown = 0xC0,
			/// <summary>OliveGreen.</summary>
			OliveGreen = 0xD0,
			/// <summary>DarkGray.</summary>
			DarkGray = 0xE0,
			/// <summary>Gray.</summary>
			Gray = 0xF0
		}


		#endregion
		#region DllImport
		[DllImport("user32.dll", CharSet=CharSet.Auto)]
		private static extern bool LockWindowUpdate(IntPtr hWndLock);

		[DllImport("user32.dll", EntryPoint="SendMessage", CharSet=CharSet.Auto )]
		public static extern int SendMessage( IntPtr hWnd, int Msg,ref GETTEXTLENGTHEX wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, 
			UIntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", CharSet=CharSet.Auto)]
		static internal extern int GetScrollInfo(IntPtr hwnd, int bar, ref SCROLLINFO si);

		[DllImport("User32.dll",CharSet = CharSet.Auto,SetLastError=true)] 
		public static extern int GetScrollPos(IntPtr hWnd, int nBar);

		[DllImport("User32.dll",CharSet = CharSet.Auto,SetLastError=true)] 
		public static extern int SetScrollPos(IntPtr hWnd,int nBar,int nPos,bool nRedraw);
 
		[DllImport("User32.dll")]
		public static extern bool PostMessage(IntPtr hWnd,int msg,int wParam,int lParam);
		#endregion
		#region STRUCTS
		public struct GETTEXTLENGTHEX
		{
			public Int32 uiFlags;
			public Int32 uiCodePage;
		}

		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
		internal struct SCROLLINFO
		{
			internal 	int   cbSize;
			internal 	int   fMask;
			internal 	int    nMin;
			internal 	int    nMax;
			internal 	int   nPage;
			internal 	int    nPos;
			internal 	int    nTrackPos;
		}
		#endregion
		#region Const

		private const int SB_LINEUP = 0;
		private const int SB_LINEDOWN = 1; 
		private const int SB_PAGEUP = 2; 
		private const int SB_PAGEDOWN = 3;
		private const int SB_THUMBPOSITION = 4; 
		private const int SB_THUMBTRACK = 5; 
		private const int SB_TOP = 6; 
		private const int SB_BOTTOM = 7; 
		private const int SB_ENDSCROLL = 8; 

		private const int WM_HSCROLL = 0x114;
		private const int WM_VSCROLL = 0x115;
		private const int WM_MOUSEWHEEL = 0x020A;
		private const int WM_NCCALCSIZE =0x0083;
		private const int WM_PAINT =0x000F;
		private const int WM_SIZE =0x0005;

		private const int SB_HORZ = 0; 
		private const int SB_VERT = 1; 
		private const int SB_CTL = 2; 
		private const int SB_BOTH = 3; 

		private const int  ESB_DISABLE_BOTH = 0x3;
		private const int  ESB_ENABLE_BOTH = 0x0;

		private const int  MK_LBUTTON = 0x01;
		private const int  MK_RBUTTON = 0x02;
		private const int  MK_SHIFT = 0x04;
		private const int  MK_CONTROL = 0x08;
		private const int  MK_MBUTTON = 0x10; 
		private const int  MK_XBUTTON1 = 0x0020;
		private const int  MK_XBUTTON2 = 0x0040;
		

		#endregion
		#region private members
		int _numberOfQuotesAndComments=0;
		bool _isUpdatingColors;
		int _lastLength = 0;
		bool _isPasting;
		int _lastScrollPos=0;
		/// <summary>
		/// Gets or sets the underline style to apply to the
		/// current selection or insertion point.
		/// </summary>
		/// <remarks>
		/// Underline styles can be set to any value of the
		/// <see cref="UnderlineStyle"/> enumeration.
		/// </remarks>
		UnderlineStyle _selectionUnderlineStyle
		{
			get
			{
				Messages.CHARFORMAT fmt = new Messages.CHARFORMAT();
				fmt.cbSize = Marshal.SizeOf( fmt );
            
				// Get the underline style
				API.SendMessage( new HandleRef( this, Handle ),
					Messages.EM_GETCHARFORMAT,
					Messages.SCF_SELECTION, ref fmt );
            
				// Default to none
				if ( ( fmt.dwMask & Messages.CFM_UNDERLINETYPE ) == 0 )
					return UnderlineStyle.None;
            
				byte style = ( byte )( fmt.bUnderlineType & 0x0F );
            
				return ( UnderlineStyle )style;
			}
			set
			{
				// Ensure we don't alter the color
				UnderlineColor color = _selectionUnderlineColor;
            
				// Ensure we don't show it if it shouldn't be shown
				if ( value == UnderlineStyle.None )
					color = UnderlineColor.Black;
            
				Messages.CHARFORMAT fmt = new Messages.CHARFORMAT();
				fmt.cbSize = Marshal.SizeOf( fmt );
				fmt.dwMask = Messages.CFM_UNDERLINETYPE;
				fmt.bUnderlineType = ( byte )( ( byte )value |
					( byte )color );
            
				// Set the underline type
				API.SendMessage( new HandleRef( this, Handle ),
					Messages.EM_SETCHARFORMAT,
					Messages.SCF_SELECTION, ref fmt );
			}
		}

		/// <summary>
		/// Gets or sets the underline color to apply to the
		/// current selection or insertion point.
		/// </summary>
		/// <remarks>
		/// Underline colors can be set to any value of the
		/// <see cref="UnderlineColor"/> enumeration.
		/// </remarks>
		UnderlineColor _selectionUnderlineColor
		{
			get
			{
				Messages.CHARFORMAT fmt = new Messages.CHARFORMAT();
				fmt.cbSize = Marshal.SizeOf( fmt );
            
				// Get the underline color
				API.SendMessage( new HandleRef( this, Handle ),
					Messages.EM_GETCHARFORMAT,
					Messages.SCF_SELECTION, ref fmt );
            
				// Default to black
				if ( ( fmt.dwMask & Messages.CFM_UNDERLINETYPE ) == 0 )
					return UnderlineColor.Black;
            
				byte style = ( byte )( fmt.bUnderlineType & 0xF0 );
            
				return ( UnderlineColor )style;
			}
			set
			{
				try
				{
					// Ensure we don't alter the style
					UnderlineStyle style = _selectionUnderlineStyle;
            
					// Ensure we don't show it if it shouldn't be shown
					if ( style == UnderlineStyle.None )
						value = UnderlineColor.Black;
            
					Messages.CHARFORMAT fmt = new Messages.CHARFORMAT();
					fmt.cbSize = Marshal.SizeOf( fmt );
					fmt.dwMask = Messages.CFM_UNDERLINETYPE;
					fmt.bUnderlineType = ( byte )( ( byte )style |
						( byte )value );
            
					// Set the underline color
					API.SendMessage( new HandleRef( this, Handle ),
						Messages.EM_SETCHARFORMAT,
						Messages.SCF_SELECTION, ref fmt );
				}
				catch(Exception ex)
				{
					return;
				}
			}
		}

		bool _checkForCommentsAndStrings;
		SyntaxReader _syntaxReader = new SyntaxReader();
		ColorPositionCollection _colorPositions = new ColorPositionCollection();
		WordAndPosition[] _buffer = new WordAndPosition[200000];
		public InfoMessageCollection _infoMessages = new InfoMessageCollection();
		protected ITextDocument _iTextDocument = null; // OLE interface used to manipulate the control thow method witch has not been exposed in the .Net RichEditControl
		protected IntPtr IRichEditOlePtr = IntPtr.Zero;
		ArrayList _undoBuffer = new ArrayList();
		ITextFont _defaultFont;
		#endregion
		#region public members
		public static bool Paint = true;
		public bool IsPasting
		{
			get
			{
				if((GetTextLength()-_lastLength)>2)
					return true;

				return false;;
			}
			set{_isPasting=value;}
		}
		public bool CheckForCommentsAndStrings
		{
			get{ return _checkForCommentsAndStrings;}
			set{ _checkForCommentsAndStrings = value;}
		}

		public int LastLength
		{
			get{return _lastLength;}
			set{_lastLength=value;}
		}
		
		public int GetTextLength()
		{
			IntPtr hControl = this.Handle;
			GETTEXTLENGTHEX lpGTL = new GETTEXTLENGTHEX();
			lpGTL.uiCodePage = 0;
			lpGTL.uiCodePage = 1200; // Unicode
			return SendMessage(hControl, Messages.EM_GETTEXTLENGTHEX ,ref lpGTL, IntPtr.Zero);
		}

		public ScrollablePanel MyPanel = null;
		public bool IsUpdatingColors;
		#endregion
		#region events
		public delegate void DebugEventHandler(object sender, DebugEventArgs args);
		public event DebugEventHandler DebugEvent;
		protected override void InitLayout()
		{
			base.InitLayout ();
			try
			{
				if(_iTextDocument==null)
					GetTextDocumentInterface();
				ITextRange range = _iTextDocument.Range(0,0);
				_defaultFont = range.Font;
			}
			catch(Exception ex)
			{
				return;
			}
		}

		protected override void OnSelectionChanged(EventArgs e)
		{
#if LOGEVENTS
			RaiseDebugEvent(new System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name + "[" + this.Text + "]");
#endif
			try
			{
				SetBackGroudForRange(0,this.Text.Length,true);
				if(this.SelectionStart-1<this.Text.Length)
				{
					string s = this.Text.Substring(this.SelectionStart-1,1);

					if(s=="(")
						CheckForParameters(ParameterType.Open,this.SelectionStart-1);
					else if(s==")")
						CheckForParameters(ParameterType.Close,this.SelectionStart);
					else
					{
						s = this.Text.Substring(this.SelectionStart,1);
						if(s=="(")
							CheckForParameters(ParameterType.Open,this.SelectionStart);
						else if(s==")")
							CheckForParameters(ParameterType.Close,this.SelectionStart+1);
					}
				}
			}
			catch
			{
				SetBackGroudForRange(0,this.Text.Length,true);
			}
			base.OnSelectionChanged (e);
		}

		protected override void OnTextChanged(EventArgs e)
		{
#if LOGEVENTS
//			RaiseDebugEvent(new System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name + "[" + this.Text + "]");
#endif
			return;
			try
			{
				SetBackGroudForRange(0,this.Text.Length,true);
				if((this.SelectionStart-1<this.Text.Length)&& this.SelectionStart!=0)
				{
					string s = this.Text.Substring(this.SelectionStart-1,1);
					
					if(s=="(")
						CheckForParameters(ParameterType.Open,this.SelectionStart-1);
					else if(s==")")
						CheckForParameters(ParameterType.Close,this.SelectionStart);
					else
					{
						s = this.Text.Substring(this.SelectionStart,1);
						if(s=="(")
							CheckForParameters(ParameterType.Open,this.SelectionStart);
						else if(s==")")
							CheckForParameters(ParameterType.Close,this.SelectionStart+1);
					}
				}
			}
			catch
			{
				SetBackGroudForRange(0,this.Text.Length,true);
			}
			Paint=false;
			base.OnTextChanged (e);
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
#if LOGEVENTS
			RaiseDebugEvent(new System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name + "1 [" + this.Text + "]");
#endif
			if(e.KeyCode.ToString().Length==1 
				|| e.KeyCode.ToString() ==  "OemQuestion" 
				|| e.KeyCode.ToString() ==  "OemMinus"
				|| e.KeyCode.ToString() ==  "D7" 
				|| e.KeyCode.ToString() ==  "Space")

			{
#if LOGEVENTS
				RaiseDebugEvent(new System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name );
#endif
				Paint=false;
				SetCurrentLineColor();
				//SetCommentAndStringColor();
			}


			base.OnKeyUp (e);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{	
#if LOGEVENTS
			RaiseDebugEvent(new System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name + "1 [" + this.Text + "]");
#endif
			base.OnKeyDown (e);
#if LOGEVENTS
			RaiseDebugEvent(new System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name + "2 [" + this.Text + "]");
#endif
		}
		protected override void OnKeyPress(KeyPressEventArgs e)
		{	
#if LOGEVENTS
			RaiseDebugEvent(new System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name + "1 [" + this.Text + "]");
#endif

			base.OnKeyPress (e);
		}

		public override void Refresh()
		{
#if LOGEVENTS
			RaiseDebugEvent(new System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name );
#endif
			Paint =false;
			SetColorSyntaxForAllText();
			CheckForCommentsAndStrings=true;
			base.Refresh ();
		}

		protected override void WndProc(ref System.Windows.Forms.Message m) 
		{
			if (m.Msg == Messages.WM_PAINT) 
			{
				if (Paint)// || _isWorking)
				{
					base.WndProc(ref m);
				}
				else
				{
#if LOGEVENTS
	RaiseDebugEvent(new System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name);
#endif
					ClearInfoMessages();
					Paint = true;

					//Pasting?
					if((GetTextLength()-_lastLength>2) )//&& this.CheckForCommentsAndStrings==false)
					{
#if LOGEVENTS
	RaiseDebugEvent(new System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name + "PASTING");
#endif
						SetColorSyntaxForAllText();
						this.CheckForCommentsAndStrings=true;
					}
					_lastLength = GetTextLength();

					m.Result = IntPtr.Zero;
				}
			}
			else
			{
				base.WndProc (ref m);
				int postion= GetScrollPos ( this.Handle,SB_VERT ) ;  //getting the scroll postion
				if(_lastScrollPos!=postion)
				{
					_lastScrollPos=postion;

					if(SetScrollPos (MyPanel.Handle,SB_VERT,postion ,true) != -1)
					{ 
						//this statement is scrolling the other panel control
						PostMessage (MyPanel.Handle,WM_VSCROLL ,SB_THUMBPOSITION + 0x10000 * postion,0);
					}
				}
			}
		}

		public override bool PreProcessMessage(ref Message msg)
		{
			if(msg.Msg==  Messages.WM_KEYDOWN)
			{
				Keys keydata = ((Keys)(int)msg.WParam)|ModifierKeys;
				Keys keycode = ((Keys)(int)msg.WParam);
#if LOGEVENTS
	RaiseDebugEvent(new System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name + " (" + keycode.ToString() + ")" );
#endif

				if(keycode==Keys.Tab)
				{
					if((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
					{
//						Point pt = this.GetPositionFromCharIndex(this.SelectionStart);
//						pt.X=0;
//						int newStartPosition = this.GetCharIndexFromPosition(pt);
//						string newText = this.Text.Substring(newStartPosition,this.SelectionLength);
//						this.Select(newStartPosition,this.SelectionLength);
//						newText = newText.Replace("\n\t","\n");
//						this.SelectedText = newText;
						return true;
					}
					else
					{
						if( this.SelectionLength>0)
						{
							Point pt = this.GetPositionFromCharIndex(this.SelectionStart);
							pt.X=0;
							int newStartPosition = this.GetCharIndexFromPosition(pt);
							string newText = this.Text.Substring(newStartPosition,this.SelectionLength);
							this.Select(newStartPosition,this.SelectionLength);
							newText = newText.Replace("\n","\n\t");
							this.SelectedText = "\t" + newText;

							return true;
						}
					}
				}
			}
			return base.PreProcessMessage (ref msg);
		}


		#endregion
		#region private methods 
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
				catch (Exception err)
				{
					Trace.WriteLine(err.ToString());
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
	
		private void SetCurrentLineColor()
		{
			try
			{
				if(_isUpdatingColors)
					return;

				_isUpdatingColors = true;
				Point pt = this.GetPositionFromCharIndex(this.SelectionStart);
				pt.X=0;
				int startPos =   this.GetCharIndexFromPosition(pt);
				int linePosition = startPos - this.GetCharIndexFromPosition(pt);
				string lineText = this.Lines[this.GetLineFromCharIndex(startPos)];

				int count = 0;
				WordAndPosition[] buffer = ParseLineWithNoValidation(lineText,ref count);
						
				for (int i = 0; i < count; i++)
				{
					_iTextDocument.Range(	startPos+buffer[i].Position,startPos+buffer[i].Position + buffer[i].Length).Font.ForeColor = _syntaxReader.GetColorRef(buffer[i].Word.ToUpper());
				}
				_isUpdatingColors=false;
			}
			catch(Exception ex)
			{
				_isUpdatingColors=false;
				return;
			}
		}
		private void SetCurrentLineColor(int line)
		{
			try
			{
				if(_isUpdatingColors)
					return;

				_isUpdatingColors = true;

				int pos=0;
				for(int i=0;i<this.Lines.Length;i++)
				{
					if(i==line)
						break;
					pos+=this.Lines[i].Length;
				
				}
				Point pt = this.GetPositionFromCharIndex(pos);
				pt.X = 0;
				int startPos =   this.GetCharIndexFromPosition(pt);
				int endPos = this.Text.IndexOf("\n",startPos);
				if(endPos==-1)
					endPos=this.Text.Length;

				
//				Point pt = this.GetPositionFromCharIndex(this.SelectionStart);
//				pt.X=0;

				//int startPos =   this.GetCharIndexFromPosition(pt);
				int linePosition = startPos - this.GetCharIndexFromPosition(pt);
				string lineText = this.Lines[this.GetLineFromCharIndex(startPos)];

				int count = 0;
				WordAndPosition[] buffer = ParseLineWithNoValidation(lineText,ref count);
						
				for (int i = 0; i < count; i++)
				{
					_iTextDocument.Range(	startPos+buffer[i].Position,startPos+buffer[i].Position + buffer[i].Length).Font.ForeColor = _syntaxReader.GetColorRef(buffer[i].Word.ToUpper());
				}
				_isUpdatingColors=false;
			}
			catch(Exception ex)
			{
				_isUpdatingColors=false;
				return;
			}
		}
		private void SetColorForRange(int startPos, int endPos, int colorRef)
		{
#if LOGEVENTS
			RaiseDebugEvent(new System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name);
#endif	
			
			Application.DoEvents();
			Paint=false;
			
			if(_iTextDocument==null)
				GetTextDocumentInterface();
			
			ITextRange range = _iTextDocument.Range(startPos,endPos);
			ITextFont font = range.Font;

			if(range.Font.ForeColor!=colorRef)
			{
				API.SendMessage(this.Handle, Messages.EM_STOPGROUPTYPING, IntPtr.Zero,IntPtr.Zero);
				Paint=true;
				font.ForeColor = colorRef;
			}

			font=null;
			range=null;

//			_isWorking = false;
		}
	
		private int SetCommentColor(int startPos)
		{

			if(startPos==-1)
				return -1;

			int endPos=this.Text.IndexOf("*/",startPos+1) +2 ;

			if(endPos<=GetTextLength() && endPos>0)
			{
				SetColorForRange(startPos,endPos,_syntaxReader.color_comment);
			}
			return (endPos);
		}
		private int SetStringColor(int startPos)
		{
			if(startPos==-1)
				return -1;
			int endPos=this.Text.IndexOf("'",startPos+1) +1;
			
			if(endPos<=GetTextLength() && endPos>0)
			{
				SetColorForRange(startPos,endPos,_syntaxReader.color_string);
			}
			return (endPos+1);
		}

		private int GetCommentColor(int startPos, string text)
		{
			if(startPos==-1)
				return -1;

			int endPos=text.IndexOf("*/",startPos+1) +2 ;

			if(endPos<=text.Length && endPos>0)
			{
				_colorPositions.Add(new ColorPosition(QueryCommander.ColorPosition.ColorType.Comment,startPos,endPos));
			}
			return (endPos);
		}
		
		private int GetStringColor(int startPos, string text)
		{
			if(startPos==-1)
				return -1;

#if LOGEVENTS
			RaiseDebugEvent(new System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name);
#endif
			int endPos=text.IndexOf("'",startPos+1) +1 ;

//			if(endPos<=text.Length && endPos>0)
//			{
//				Point pt = this.GetPositionFromCharIndex(startPos);
//				pt.X=0;
//				int linePosition = startPos - this.GetCharIndexFromPosition(pt);
//				string lineText = this.Lines[this.GetLineFromCharIndex(startPos)];
//				int commentPosition = lineText.IndexOf("--");	
//					
//				if(commentPosition<0 || commentPosition > linePosition)
//					_colorPositions.Add(new ColorPosition(QueryCommander.ColorPosition.ColorType.String,startPos,endPos));
//			}
			return (endPos);
		}
	
		private int GetFirstPosition(int currentPosition)
		{

			if(currentPosition==0)
				return 0;
			Point pt;
			pt = this.GetPositionFromCharIndex(currentPosition);
			pt.X = 0;
			int linseStartPos =   this.GetCharIndexFromPosition(pt);
			int pos = linseStartPos;
			string s= this.Lines[ this.GetLineFromCharIndex(currentPosition) ];
			currentPosition = currentPosition-linseStartPos;
			Regex r = new Regex(@"[(). \f\t\v\n]", RegexOptions.RightToLeft);

			MatchCollection mCol = r.Matches(s);

			for(int i=0;i<mCol.Count;i++)
			{
				if(mCol[i].Index<currentPosition-1)
				{
					pos = linseStartPos + mCol[i].Index;
					break;
				}
			}
			
			return pos;
		}
		private int GetLastPosition(int currentPosition)
		{

			if(currentPosition==0)
				return 0;
		
			Point pt;
			pt = this.GetPositionFromCharIndex(currentPosition);
			pt.X = 0;
			int linseStartPos =   this.GetCharIndexFromPosition(pt);
			string s= this.Lines[ this.GetLineFromCharIndex(currentPosition) ];
			int pos = linseStartPos + s.Length;
			currentPosition = currentPosition-linseStartPos;

			Regex r = new Regex(@"[(). \f\t\v\n]");

			MatchCollection mCol = r.Matches(s);

			for(int i=0;i<mCol.Count;i++)
			{
				if(mCol[i].Index>currentPosition-1)
				{
					pos = linseStartPos + mCol[i].Index;
					break;
				}
			}
			
			return pos;
		}

		private int PreviusIndexOf(string character, int currentPosition)
		{
			int line = this.GetLineFromCharIndex(currentPosition);

			for(int i=currentPosition;i>0;i--)
			{
				if(this.GetLineFromCharIndex(i) != line)
					return -1;
				if(this.Text.Substring(i-1,1)==character)
				{
					return i;
				}
			}
			return 0;
		}

		private int ParseLine(string s)
		{
			_buffer.Initialize();
			int count = 0;
			Regex r = new Regex(@"\w+|[^A-Za-z0-9_ \f\t\v]", RegexOptions.IgnoreCase|RegexOptions.Compiled);
			Match m;

			for (m = r.Match(s); m.Success ; m = m.NextMatch()) 
			{
				if(_syntaxReader.IsReservedWord(m.Value.ToUpper()))
				{
					_buffer[count].Word = m.Value;
					_buffer[count].Position = m.Index;
					_buffer[count].Length = m.Length;
					count++;
				}
				
			}
			return count;
		}
		private WordAndPosition[] ParseLineWithNoValidation(string s, ref int count)
		{
			WordAndPosition[] buffer = new WordAndPosition[200000];
			buffer.Initialize();
			count = 0;
			Regex r = new Regex(@"\w+|[^A-Za-z0-9_ \f\t\v]", RegexOptions.IgnoreCase|RegexOptions.Compiled);
			Match m;

			for (m = r.Match(s); m.Success ; m = m.NextMatch()) 
			{
				buffer[count].Word = m.Value;
				buffer[count].Position = m.Index;
				buffer[count].Length = m.Length;
				count++;
			}
			return buffer;
		}
		
		private int ParseLineWithNoValidation(string s)
		{
			_buffer.Initialize();
			int count = 0;
			Regex r = new Regex(@"\w+|[^A-Za-z0-9_ \f\t\v]", RegexOptions.IgnoreCase|RegexOptions.Compiled);
			Match m;

			for (m = r.Match(s); m.Success ; m = m.NextMatch()) 
			{
				_buffer[count].Word = m.Value;
				_buffer[count].Position = m.Index;
				_buffer[count].Length = m.Length;
				count++;
			}
			return count;
		}
		private void GetCommentAndStringColor_NEW(int startPos, int endPos)
		{
			Console.WriteLine("Start...");
			if(_iTextDocument==null)
				GetTextDocumentInterface();

			int Position = 0;
			int searchFor = (int)SearchFor.All;
			string s = this.Text;
			string pat = @"\x2F+\x2A|\x2A+\x2F|\x27";
			Regex r = new Regex(pat, RegexOptions.IgnoreCase|RegexOptions.Compiled);
			Match m;

			for (m = r.Match(s); m.Success ; m = m.NextMatch()) 
			{
				switch(searchFor)
				{
					case (int)SearchFor.All:
						Position= m.Index;
						switch(m.Value)
						{
							case "/*":
								searchFor=(int)SearchFor.EndComment;
								break;
							case "'":
								searchFor=(int)SearchFor.String;
								break;
							default:
								break;
						}
						
						break;
					case (int)SearchFor.EndComment:
						if(m.Value.IndexOf("*/")<0)
							continue;
						_iTextDocument.Range(Position,m.Index+m.Value.Length).Font.ForeColor = _syntaxReader.color_comment;
						searchFor=(int)SearchFor.All;
						break;
					case (int)SearchFor.String:
						if(m.Value.IndexOf("'")<0)
							continue;
						_iTextDocument.Range(Position,m.Index+m.Value.Length).Font.ForeColor = _syntaxReader.color_string;
						searchFor=(int)SearchFor.All;
						break;
				}
			}
			Console.WriteLine("Klar!");
		}
		private void GetCommentAndStringColor(int startPos, int endPos)
		{
			Application.DoEvents();
			string text = this.Text;
			if(text.Length == 0 || startPos<0)
				return;
			if(startPos>=text.Length)
				return;

			int CommentstartPos=text.IndexOf("/*",startPos);
			int StringstartPos=text.IndexOf("'",startPos);
			
			//No Comments or strings
			if(CommentstartPos<0 && StringstartPos<0)
				return;

			if((CommentstartPos>=startPos && CommentstartPos<StringstartPos) || CommentstartPos>-1 && StringstartPos<=startPos)
			{		
				if(text.IndexOf("*/",CommentstartPos+1)>0)
				{
					startPos=GetCommentColor(CommentstartPos,text);
					GetCommentAndStringColor(startPos, endPos);
					
				}
			}
			else
			{
				if(text.IndexOf("'",StringstartPos+1)>0)
				{
                    startPos=GetStringColor(StringstartPos, text);
					GetCommentAndStringColor(startPos, endPos);
				}
			}

			return;
		}
		
		private void ApplyCommentAndStringColor()
		{
#if LOGEVENTS
			RaiseDebugEvent(new System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name);
#endif
			LockWindowUpdate(this.Handle);
			foreach(ColorPosition c in _colorPositions)
			{
				if(c.Type==ColorPosition.ColorType.String)
					SetColorForRange(c.StartPosition,c.EndPosition,_syntaxReader.color_string);
				else
					SetColorForRange(c.StartPosition,c.EndPosition,_syntaxReader.color_comment);
			}
			_colorPositions.Clear();
			LockWindowUpdate(IntPtr.Zero);
		}
		
		private void RaiseDebugEvent(string method) 
		{
			DebugEventArgs d = new DebugEventArgs(method);

			// Now, raise the event by invoking the delegate. Pass in 
			// the object that initated the event (this) as well as FireEventArgs. 
			// The call must match the signature of FireEventHandler.
			DebugEvent(this,d); 
		}

		private int GetConnectedClosingParamether(int pos)
		{
			string s = this.Text.Substring(pos, this.Text.Length-pos);
			string pat = @"\x28|\x29";
			int next = -1;
			int ret = -1;
			Regex r = new Regex(pat, RegexOptions.IgnoreCase|RegexOptions.Compiled);
			MatchCollection mCol = r.Matches(s);

			for(int i=0;i<=mCol.Count-1;i++)
			{
				int t = mCol[i].Value.IndexOf("(");

//				if(mCol[i].Index<=pos)
//					continue;
				if(mCol[i].Value.IndexOf("(")==0)
					next++;
				else
				{
					if(next==0)
					{
						ret = mCol[i].Index+mCol[i].Value.Length -1;
						break;
					}
					else
						next--;
				}
			}
			return ret+pos;
		}
		private int GetConnectedOpeningParamether(int pos)
		{
			string s = this.Text.Substring(0,pos);
			string pat = @"\x28|\x29";
			int next = -1;
			int ret = -1;
			Regex r = new Regex(pat, RegexOptions.IgnoreCase|RegexOptions.Compiled);
			MatchCollection mCol = r.Matches(s);

			for(int i=mCol.Count-1;i>=0;i--)
			{
				int t = mCol[i].Value.IndexOf(")");

				if(mCol[i].Index>=pos)
					continue;
				if(mCol[i].Value.IndexOf(")")==0)
					next++;
				else
				{
					if(next==0)
					{
						ret = mCol[i].Index+mCol[i].Value.Length -1;
						break;
					}
					else
						next--;
				}

			}

			return ret;
		}
		
		private void SetBoldForRange(int startPos, int endPos)
		{
			if(_iTextDocument==null)
				GetTextDocumentInterface();
			
			ITextRange range = _iTextDocument.Range(startPos,endPos);
			ITextFont font = range.Font;

			API.SendMessage(this.Handle, Messages.EM_STOPGROUPTYPING, IntPtr.Zero,IntPtr.Zero);
			Paint=true;
			font.Bold = 0;
			
			font=null;
			range=null;
		}
		private void SetBackGroudForRange(int startPos, int endPos, bool reset)
		{
			if(_iTextDocument==null)
				GetTextDocumentInterface();
			
			_iTextDocument.Freeze();
			ITextRange range = _iTextDocument.Range(startPos,endPos);
			ITextFont font = range.Font;

			API.SendMessage(this.Handle, Messages.EM_STOPGROUPTYPING, IntPtr.Zero,IntPtr.Zero);
			Paint=false;
			if(reset)
				font.BackColor= -9999997;
			else
			{
				font.BackColor = 8454143;
			}
			_iTextDocument.Unfreeze();
			font=null;
			range=null;
		}
		#endregion
		#region public methods
		public void SetLineRangeColor(int firstLine, int toLine)
		{
			for(int i=firstLine;i<toLine;i++)
			{
				SetCurrentLineColor(i);
			}
		}
		public void CheckForParameters(ParameterType type, int pos1)
		{
			//int pos1 = this.SelectionStart;
			int pos2 = 0;

			if(type==ParameterType.Open)
			{
				pos2 = GetConnectedClosingParamether(pos1);
				if(pos2>=0)
					SetBackGroudForRange(pos1,pos2+1,false);
			}
			else
			{
				pos2 = GetConnectedOpeningParamether(pos1);
				if(pos2>=0)
					SetBackGroudForRange(pos2,pos1,false);
			}
			//SetBoldForRange(pos2,pos1+1);
			//SetBoldForRange(pos2,pos2+1);
			
		}
		public int GetCharIndexForTableDefenition(string tableName)
		{
			_buffer.Initialize();
			int count = 0;
			string s = this.Text;
			Regex r = new Regex(@"\w+|[^A-Za-z0-9_ \f\t\v]", RegexOptions.IgnoreCase|RegexOptions.Compiled);
			Match m;

			for (m = r.Match(s); m.Success ; m = m.NextMatch()) 
			{
				if( (m.Value.ToUpper()==tableName.ToUpper() && _buffer[count-1].Word.ToUpper() == "FROM") ||
					(m.Value.ToUpper()==tableName.ToUpper() && _buffer[count-1].Word.ToUpper() == "JOIN"))
				{
					return m.Index;
				}
				_buffer[count].Word = m.Value;
				_buffer[count].Position = m.Index;
				_buffer[count].Length = m.Length;
				count++;
				
				
			}
			return -1;
		}
		public void SetCommentAndStringColor()
		{
			try
			{
				API.SendMessage(this.Handle, Messages.EM_STOPGROUPTYPING, IntPtr.Zero,IntPtr.Zero);
				string pat=@"\x27(.|[\r\n])*?\x27 | /\*(.|[\r\n])*?\*/ |--.*";
				
				Regex r = new Regex(pat,RegexOptions.IgnorePatternWhitespace);

				int c1 = this.Text.Length - this.Text.Replace("'","").Length;
				int c2 = (this.Text.Length - this.Text.Replace(@"/*","").Length)/2;
				int numberOfQuotesAndComments = c1+c2;

				//int numberOfQuotesAndComments = r.Matches(this.Text).Count;
				if(numberOfQuotesAndComments==0)
					return;

				int diff=_numberOfQuotesAndComments - numberOfQuotesAndComments;
				if(diff>-2 && diff<+1 )
					return;

				Match m;
				
				for (m = r.Match(this.Text); m.Success ; m = m.NextMatch())
				{
					if(m.Value.IndexOf("'")==0)
						SetColorForRange(m.Index,m.Index+m.Length,_syntaxReader.color_string);
					else
						SetColorForRange(m.Index,m.Index+m.Length,_syntaxReader.color_comment);
				}
				_numberOfQuotesAndComments = numberOfQuotesAndComments;
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		public void SetCommentAndStringColor(int startPos, int endPos)
		{
//			if(!_checkForCommentsAndStrings)
//				return;
#if LOGEVENTS
			RaiseDebugEvent(new System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name);
#endif
			_checkForCommentsAndStrings=false;
			//SetColorSyntaxForAllText();
			GetCommentAndStringColor_NEW(startPos,endPos);
//			GetCommentAndStringColor(startPos,endPos);
//			ApplyCommentAndStringColor();
			_lastLength = GetTextLength();
		
		}
		public void UndoAction()
		{	
			int numberOfUndo=0;
			while(this.UndoActionName=="Unknown" && this.CanUndo)
			{
				this.Undo();
				numberOfUndo++;
			}
			
			string r = this.UndoActionName;
			Console.WriteLine(r);
			if(this.CanUndo)
				this.Undo();
		}
		public void SetCurrentWordColor()
		{	
			try
			{
				int lastPos		= GetLastPosition( this.SelectionStart);
				int firstPos	= GetFirstPosition(lastPos);
				if(lastPos == firstPos)
					return;

				string word		= this.Text.Substring(firstPos ,lastPos-firstPos).Trim();

#if LOGEVENTS
	RaiseDebugEvent(new System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name +" (" + word + ")");
#endif

				string lineText = this.Lines[this.GetLineFromCharIndex(this.SelectionStart)];

				if(word.Length==1)
				{
					SetColorForRange(firstPos,lastPos,_syntaxReader.color_default );
					return;
				}

				int pos = word.IndexOf("(",0);
				if(pos<0)
					pos=lastPos;
				else if(pos>0)
				{
					SetColorForRange(firstPos,lastPos,_syntaxReader.color_default );
					lastPos = lastPos -(word.Length-pos);
					word=word.Substring(0,pos);
				}
				
				string sss = word.ToUpper().Substring(0,word.Length);
				if(pos==0)
					SetColorForRange(firstPos+1,lastPos,_syntaxReader.GetColorRef(word.ToUpper().Substring(1,word.Length-1) ));
				else
					SetColorForRange(firstPos,lastPos,_syntaxReader.GetColorRef(word.ToUpper()));

			}
			catch
			{
				//hhthrow;
			}
		}
		public string GetCurrentWord()
		{
			int lastPos		= GetLastPosition( this.SelectionStart);
			int firstPos	= GetFirstPosition(lastPos);
			string word		= this.Text.Substring(firstPos ,lastPos-firstPos).Trim();
			
			word = word.Replace("(","");
			word = word.Replace(")","");
			word = word.Replace(".","");
			word = word.Replace("'","");
			word = word.Replace("--","");
			word = word.Replace("/*","");
			word = word.Replace("*/","");
			word = word.Replace(" ","");
			word = word.Replace("[","");
			word = word.Replace("]","");

			return word;
		}
		
		public void SetReseveredWordsToUpperCase()
		{
			int count = ParseLine(this.Text);
			if(_iTextDocument==null)
				GetTextDocumentInterface();

			for (int i = 0; i < count; i++)
			{
				if( _syntaxReader.IsReservedWord(_buffer[i].Word.ToUpper()))
				{
					string CharToUpper = _buffer[i].Word.ToUpper();
					
					tom.ITextRange range = _iTextDocument.Range(_buffer[i].Position,_buffer[i].Position+_buffer[i].Length);
					if(range.Font.ForeColor != _syntaxReader.color_comment)
						range.Text = CharToUpper;
				}
			}
		}

		public void SetColorSyntaxForAllText_XXX()
		{
			for(int line=0;line<this.Lines.Length;line++)
			{
				SetCurrentLineColor(line);
			}
		}
		public void SetColorSyntaxForAllText()
		{
#if LOGEVENTS
			RaiseDebugEvent(new System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name);
#endif
			try
			{
				this.Cursor = Cursors.WaitCursor;
				LockWindowUpdate(this.Handle);

				int l = GetTextLength();
				
				int count = ParseLine(this.Text);

				// Reset colors
				_iTextDocument.Range(0,this.Text.Length).Font.ForeColor=_syntaxReader.color_default;

				if(_iTextDocument==null)
					GetTextDocumentInterface();

				for (int i = 0; i < count; i++)
				{
					_iTextDocument.Range(_buffer[i].Position,_buffer[i].Position + _buffer[i].Length).Font.ForeColor = _syntaxReader.GetColorRef(_buffer[i].Word.ToUpper());
				}
				string text=this.Text;

				_isPasting=false;
				SetCommentAndStringColor();
				
				LockWindowUpdate(IntPtr.Zero);
				this.Cursor = Cursors.Default;
			}
			catch(Exception ex)
			{
				_isPasting=false;
				LockWindowUpdate(IntPtr.Zero);
				this.Cursor=Cursors.Default;
				return;
			}
		}
		
		public void GoToLine(int line)
		{
			int pos=0;
			for(int i=0;i<this.Lines.Length;i++)
			{
				if(i==line)
					break;
			
				pos+=this.Lines[i].Length;
				
			}
			Point pt = this.GetPositionFromCharIndex(pos);
			pt.X = 0;
			int startPos =   this.GetCharIndexFromPosition(pt);
			this.SelectionStart=startPos;	
		}
		public void SelectLine(int line)
		{
			int pos=0;
			for(int i=0;i<this.Lines.Length;i++)
			{
				if(i==line)
					break;
				pos+=this.Lines[i].Length;
				
			}
			Point pt = this.GetPositionFromCharIndex(pos);
			pt.X = 0;
			int startPos =   this.GetCharIndexFromPosition(pt);
			int endPos = this.Text.IndexOf("\n",startPos);
			if(endPos==-1)
				endPos=this.Text.Length;

			this.Select(startPos, endPos-startPos);
			
		}
		public void CommentSection()
		{
			bool IsInSelection = true;
			int startPos = this.SelectionStart;
			int lastPos = this.SelectionLength + startPos;
			int currentPosition = startPos;
			while(IsInSelection)
			{
				Point pt;
				pt = this.GetPositionFromCharIndex(currentPosition);
				pt.X = 0;
				currentPosition = this.GetCharIndexFromPosition(pt);
				string lineText = this.Lines[this.GetLineFromCharIndex(currentPosition)];
				int endPos = currentPosition + lineText.Length;
				_iTextDocument.Range(currentPosition,endPos).Text = "-- " + lineText;
			}
			
		
		}

		public ArrayList GetVariables(string Stringmatch)
		{
			ArrayList arrList = new ArrayList();
			string s = this.Text;
			string pat = @"\100+\w+";
			Regex r = new Regex(pat, RegexOptions.IgnoreCase|RegexOptions.Compiled);
			Match m;

			for (m = r.Match(s); m.Success ; m = m.NextMatch())
			{
				if(!arrList.Contains(m.Value))
				arrList.Add(m.Value);
				
			}
			return arrList;
		}
		public void ClearInfoMessages()
		{
			_infoMessages.Clear();
			this.Controls.Clear();
		}
		public void AddInfoMessages(Point StartPoint,QueryCommander.InfoMessage.MessageType MessageType, double PercentPositionFromTop, string Message)
		{
			int i = _infoMessages.Add(new InfoMessage(StartPoint,MessageType,PercentPositionFromTop,Message));
			this.Controls.Add(_infoMessages[i].Picture);
		}
		
		public void Collapse()
		{
			return;

			int WM_USER = 0x0400;
			int EM_OUTLINE = WM_USER + 220;
			int EMO_EXPAND = 3;

			API.SendMessage(this.Handle,EM_OUTLINE,1,0);

			API.SendMessage(this.Handle,EM_OUTLINE,EMO_EXPAND,1);

			API.SendMessage(this.Handle,EM_OUTLINE,EMO_EXPAND,-1);


		}
		public void SetSelectionUnderlineStyle(UnderlineStyle underlineStyle)
		{
			_selectionUnderlineStyle = underlineStyle;
		}
		public void SetSelectionUnderlineColor(UnderlineColor underlineColor)
		{
			_selectionUnderlineColor = underlineColor;
		}

		public void SetText(int startPos, int endPos, string text)
		{
			try
			{
				if(_iTextDocument==null)
					GetTextDocumentInterface();

				_iTextDocument.Range(startPos,endPos).Text=text;

			}
			catch(Exception ex)
			{
				return;
			}
		}
		public string GetText(int startPos, int endPos)
		{
			try
			{
				if(_iTextDocument==null)
					GetTextDocumentInterface();

				return _iTextDocument.Range(startPos,endPos).Text.Replace("\r","\n");

			}
			catch(Exception ex)
			{
				return "";;
			}
		}
		
		#endregion
	}
	#region classes
	public class InfoMessageCollection :CollectionBase	
	{
		public virtual int Add(InfoMessage newInfoMessage)
		{
			return this.List.Add(newInfoMessage);        
		}
		public virtual InfoMessage this[int Index]
		{
			get
			{
				return (InfoMessage)this.List[Index];        
			}
		}
	}
	public class InfoMessage
	{
		public enum MessageType {Info, Warning, Exception};
		public System.Windows.Forms.PictureBox Picture = new PictureBox();
		public Point StartPoint;		
		public double PercentPositionFormTop;
		public InfoMessage(Point startPoint, MessageType messageType, double percentPositionFormTop, string Message)
		{
			this.StartPoint=startPoint;
			this.Picture.Location=startPoint;
			this.Picture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.Picture.Size = new System.Drawing.Size(15, 15);
			this.Picture.BackColor = System.Drawing.Color.Transparent;
			this.PercentPositionFormTop=percentPositionFormTop;
			ToolTip toolTip1 = new ToolTip();
//			toolTip1.AutoPopDelay = 1000;
//			toolTip1.InitialDelay = 1000;
//			toolTip1.ReshowDelay = 500;
			toolTip1.ShowAlways = true;
      		toolTip1.SetToolTip(this.Picture,Message);


			switch(messageType)
			{
				case MessageType.Info:
					this.Picture.Image=System.Drawing.Image.FromStream(CopyEmbeddedResource("QueryCommander.Embedded.infomessage.gif"));
					break;
				case MessageType.Warning:
					this.Picture.Image=System.Drawing.Image.FromStream(CopyEmbeddedResource("QueryCommander.Embedded.infowarning.gif"));
					break;
				case MessageType.Exception:
					this.Picture.Image=System.Drawing.Image.FromStream(CopyEmbeddedResource("QueryCommander.Embedded.infowarning.gif"));
					break;
				default:
					this.Picture.Image=System.Drawing.Image.FromStream(CopyEmbeddedResource("QueryCommander.Embedded.infomessage.gif"));
					break;
			}
		}
		private System.IO.Stream CopyEmbeddedResource(string resource)
		{
			System.Reflection.Assembly a = System.Reflection.Assembly.GetEntryAssembly();
			return a.GetManifestResourceStream(resource);
		}
		 
	
	}
	public class DebugEventArgs: EventArgs 
	{
		public DebugEventArgs(string method) 
		{
			this.method = method;
		}
		public string method;
	}

//	struct CommentsAndStrings
//	{
//		public int Type;
//		public int Position;
//		public int Length;
//	}
	struct WordAndPosition
	{
		public string Word;
		public int Position;
		public int Length;
		public override string ToString()
		{
			string s = "Word = " + Word + ", Position = " + Position + ", Length = " + Length + "\n";
			return s;
		}
	};
	/// <summary>
	/// Used as storage of all Comments and Strings
	/// </summary>
	class ColorPosition
	{
		public ColorPosition(ColorType type, int startPosition, int endPosition)
		{
			StartPosition = startPosition;
			EndPosition = endPosition;
			Type = type;
		}
		public enum ColorType{String, Comment}
		public int StartPosition;
		public int EndPosition;
		public ColorPosition.ColorType Type;
		
	};
	class ColorPositionCollection :ArrayList
	{
		public int Add(ColorPosition colorPosition)
		{
			return base.Add (colorPosition);
		}

	}
	public class API
	{
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
		public static extern int SendMessage(IntPtr hWnd, int message, IntPtr wParam, IntPtr lParam);
		[DllImport( "user32", CharSet = CharSet.Auto )]
		public static extern int SendMessage( HandleRef hWnd, int msg, int wParam, ref Messages.PARAFORMAT lp );
		[DllImport( "user32", CharSet = CharSet.Auto )]
		public static extern int SendMessage( HandleRef hWnd, int msg, int wParam,	ref Messages.CHARFORMAT lp );
		
		[DllImport( "user32", CharSet = CharSet.Auto )]
		public static extern int SendMessage( IntPtr hWnd, 
			int wmsg,
			int wParam,
			int lParam);
//		(ByVal hwnd As Long, 
//		ByVal wMsg As Long, 
//		ByVal wParam As Long, 
//		ByVal lParam As Long)
	}
	
	/// <summary>
	/// Definition of message constats
	/// </summary>
	public class Messages
	{
		#region Enums
		// It makes no difference if we use PARAFORMAT or
		// PARAFORMAT2 here, so I have opted for PARAFORMAT2
		[StructLayout( LayoutKind.Sequential )]
			public struct PARAFORMAT
		{
			public int cbSize;
			public uint dwMask;
			public short wNumbering;
			public short wReserved;
			public int dxStartIndent;
			public int dxRightIndent;
			public int dxOffset;
			public short wAlignment;
			public short cTabCount;
			[MarshalAs( UnmanagedType.ByValArray, SizeConst = 32 )]
			public int[] rgxTabs;
        
			// PARAFORMAT2 from here onwards
			public int dySpaceBefore;
			public int dySpaceAfter;
			public int dyLineSpacing;
			public short sStyle;
			public byte bLineSpacingRule;
			public byte bOutlineLevel;
			public short wShadingWeight;
			public short wShadingStyle;
			public short wNumberingStart;
			public short wNumberingStyle;
			public short wNumberingTab;
			public short wBorderSpace;
			public short wBorderWidth;
			public short wBorders;
		}
		[StructLayout( LayoutKind.Sequential )]
			public struct CHARFORMAT
		{
			public int cbSize;
			public uint dwMask;
			public uint dwEffects;
			public int yHeight;
			public int yOffset;
			public int crTextColor;
			public byte bCharSet;
			public byte bPitchAndFamily;
			[MarshalAs( UnmanagedType.ByValArray, SizeConst = 32 )]
			public char[] szFaceName;
        
			// CHARFORMAT2 from here onwards
			public short wWeight;
			public short sSpacing;
			public int crBackColor;
			public int LCID;
			public uint dwReserved;
			public short sStyle;
			public short wKerning;
			public byte bUnderlineType;
			public byte bAnimation;
			public byte bRevAuthor;
		}

		#endregion
		#region Constants
		public const int WM_USER = 0x0400;
		public const int EM_STOPGROUPTYPING = WM_USER + 88;
		public const int EM_GETOLEINTERFACE = WM_USER + 60;
		public const short  WM_PAINT = 0x00f;
		public const short  WM_KEYDOWN = 0x100;
		public const int EM_GETTEXTLENGTHEX = WM_USER + 95;
		public const int WM_VSCROLL = 0x115;
		public const int EM_UNDO = 0x304;

		public const int	EM_SETEVENTMASK = 1073;
		public const int	EM_GETPARAFORMAT = 1085;
		public const int	EM_SETPARAFORMAT = 1095;
		public const int	EM_SETTYPOGRAPHYOPTIONS = 1226;
		public const int	WM_SETREDRAW = 11;
		public const int	TO_ADVANCEDTYPOGRAPHY = 1;
		public const int	PFM_ALIGNMENT = 8;
		public const int	SCF_SELECTION = 1;

		public const int CFM_UNDERLINETYPE = 8388608;
		public const int EM_SETCHARFORMAT = 1092;
		public const int EM_GETCHARFORMAT = 1082;
		public const int EM_OUTLINE = WM_USER + 220;
		#endregion
	}
	
	public class Action
	{
		public Action(int Position, string Value, string SelectedText)
		{
			this.Position=Position;
			this.Value=Value;
			this.SelectedText=SelectedText;
		}
		public int Position;
		public string Value ;
		public string SelectedText;
	}
	#endregion
}
