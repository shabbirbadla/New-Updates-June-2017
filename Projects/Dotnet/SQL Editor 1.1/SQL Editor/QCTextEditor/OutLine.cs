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
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Text.RegularExpressions;

namespace QueryCommander
{
	
	public class OutLine
	{
		#region Constants
		const string expandIconPath= @"C:\Documents and Settings\WMMIHAA\My Documents\Visual Studio Projects\WindowsApplication7\collapse.gif";
		const string collapseIconPath= @"C:\Documents and Settings\WMMIHAA\My Documents\Visual Studio Projects\WindowsApplication7\expand.gif";
		const string expandLineIconPath= @"C:\Documents and Settings\WMMIHAA\My Documents\Visual Studio Projects\WindowsApplication7\ExpandedLine.gif";
		const string expandLineEndIconPath= @"C:\Documents and Settings\WMMIHAA\My Documents\Visual Studio Projects\WindowsApplication7\ExpandedLineEnd.gif";
		#endregion
		#region Enums
		public enum States
		{
			Collapsed, Expanded
		};
		#endregion
		#region Private members
		OutlineManager _manager = null;
		States _state;
		string _header;
		string _story;
		int _startPos;
		int _endPos;
		int _startLine;
		int _endLine;
		int _numberOfLines;
		bool _isInitiated=false;
		PictureBox _picture = new PictureBox();
		PictureBox _expandPicture = new PictureBox();
		PictureBox _expandPictureEnd = new PictureBox();
		Panel _panel = null;
		#endregion
		#region Public Members
		public OutlineManager Manager
		{
			get{return _manager;}
		}
		public PictureBox Picture
		{
			get{return _picture;}
		}
		public PictureBox ExpandPicture
		{
			get{return _expandPicture;}
		}
		public PictureBox ExpandPictureEnd
		{
			get{return _expandPictureEnd;}
		}
		public Panel panel
		{
			get{return _panel;}
		}
		public bool IsExpanded
		{
			get
			{
				if(_state==States.Expanded)
					return true;
				else
					return false;
			}
		}
		public bool IsCollapsed
		{
			get
			{
				if(_state==States.Collapsed)
					return true;
				else
					return false;
			}
		}

		public int StartPos
		{
			get{return _startPos;}
			set{_startPos=value;}
		}
		public int EndPos
		{
			get
			{
				if(IsExpanded)
					return _endPos;
				else
					return _startPos + _header.Length;
			}
			set{_endPos=value;}
		}
		public int StartLine
		{
			get{return _startLine;}
			set{_startLine=value;}
		} 
		public int EndLine
		{
			get
			{
				if(IsExpanded)
					return _endLine;
				else
					return _startLine;
			}
			//get{return _endLine;}
			set{_endLine=value;}
		} 
		public int NumberOfLines
		{
			get
			{
				if(IsCollapsed)	
					return _numberOfLines;
				else
				{
					int startL=_manager.textControl.GetLineFromCharIndex(_startPos);
					int endL=_manager.textControl.GetLineFromCharIndex(_endPos);

					return endL-startL;
				}
			
			}
			set{_numberOfLines=value;}
		}
		public string Story
		{
			get{return _story;}
			set{_story=value;}
		}

		public string Header
		{
			get{return _header;}
			set{_header=value;}
		}

		public string Text
		{
			get
			{
				if(_state == States.Expanded)
					return _story;
				else
					return _header;
			}
		}
		#endregion
		#region Operators
		public static bool operator ==(OutLine x, OutLine y)
		{
			if(x.StartPos==y.StartPos && x.EndPos==y.EndPos)
				return true;
			else
				return false;
		}
		public static bool operator !=(OutLine x, OutLine y)
		{
			if(x.StartPos==y.StartPos && x.EndPos==y.EndPos)
				return false;
			else
				return true;
		}

		public override bool Equals(object o)
		{
			if(!(o is OutLine))
				return false;

			return this==(OutLine)o;
		}
		public override int GetHashCode()
		{
			return base.GetHashCode ();
		}

		#endregion
		#region Constructor
		public OutLine(Panel panel, Point point, string Story, int StartPos, int EndPos, int startLine, int endLine, int NumberOfLines, OutlineManager manager)
		{
			_story=Story;
			_startPos=StartPos;
			_endPos=EndPos;
			_numberOfLines=NumberOfLines;
			_header = "[COMMENT...]";
			_state= States.Expanded;
			_startLine=startLine;
			_endLine=endLine;
			_panel=panel;
			_manager = manager;

			point.X=9;
			_picture.Image = System.Drawing.Image.FromFile(expandIconPath);
			_picture.BackColor = System.Drawing.Color.Transparent;
			_picture.Location = point;
			_picture.Size = new System.Drawing.Size(16, 16);
			_picture.TabStop = false;
			_picture.Click += new System.EventHandler(Click);
			_picture.Move += new System.EventHandler(Move);

			_expandPicture.Image= System.Drawing.Image.FromFile(expandLineIconPath);
			_expandPicture.BackColor = System.Drawing.Color.Transparent;
			_expandPicture.TabStop = false;
			_expandPicture.Size = new System.Drawing.Size(16, 1);
			_expandPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			
			_expandPictureEnd.Image= System.Drawing.Image.FromFile(expandLineEndIconPath);
			_expandPictureEnd.BackColor = System.Drawing.Color.Transparent;
			_expandPictureEnd.TabStop = false;
			_expandPictureEnd.Size = new System.Drawing.Size(16, 1);
			_expandPictureEnd.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;

			SetExpandLine();
			
		}
		#endregion
		#region Events
		private void Move(object sender, System.EventArgs e)
		{
		
		}
		private void Click(object sender, System.EventArgs e)
		{
			Console.WriteLine("Click...");
			if(IsExpanded)
				Collapse();
			else
				Expand();

			_manager.AppendOutline();
		}
		
		#endregion
		#region Private Methods
		private void Expand()
		{
			_state=States.Expanded;
			_picture.Image = System.Drawing.Image.FromFile(expandIconPath);
			_manager.UpdateOutLine(_startPos,_startPos + _header.Length, _story,false);
			
			Point lowerP = _manager.textControl.GetPositionFromCharIndex(_story.Length+_startPos);
			int diff = lowerP.Y-_picture.Location.Y;
			int diffText = _story.Length-_header.Length;
			foreach(OutLine ol in _manager.outLineCollection)
			{
				if(ol.IsCollapsed && ol.StartPos>this.EndPos )
				{
					Point p = ol.Picture.Location;
					p.Y+=diff;
					ol.Picture.Location=p;
					ol.StartPos+=diffText;
					ol.EndPos+=diffText;
				}
			}
			SetExpandLine();
			
			_manager.textControl.SetLineRangeColor(_startLine,_endLine);
			
		}
		private void Collapse()
		{
			ArrayList arr = new ArrayList();
			_state=States.Collapsed;
			_picture.Image = System.Drawing.Image.FromFile(collapseIconPath);

			Point lowerP	= _manager.textControl.GetPositionFromCharIndex(_endPos);
			Point llp		= _manager.textControl.GetPositionFromCharIndex(9);
			int diff		= _picture.Location.Y-lowerP.Y;
			int diffText	= _manager.textControl.GetPositionFromCharIndex(_startPos).Y-_manager.textControl.GetPositionFromCharIndex(_endPos).Y;
			
			foreach(OutLine ol in _manager.outLineCollection)
			{
				//if(ol.IsCollapsed && ol.StartPos  >this.EndPos )
				if(ol.IsCollapsed && ol.Picture.Location.Y  > this.Picture.Location.Y )
				{
					Point p = ol.Picture.Location;
					p.Y+=diff;
					ol.Picture.Location=p;
					
					string ssss = _manager.textControl.Text.Substring(ol.StartPos,ol.EndPos-ol.StartPos);
					Point ppp = _manager.textControl.GetPositionFromCharIndex(ol.StartPos);
					int NewY = ppp.Y+=diffText;
					
					arr.Add(new OutLinePosition( ol,new Point(0,NewY) ));
				}
			}
			_manager.UpdateOutLine(_startPos, _endPos, _header, true);

			for(int i=0;i<arr.Count;i++)
			{
				OutLinePosition olp = (OutLinePosition)arr[i];
				OutLine ol = olp.outLine;

				ol.StartPos = _manager.textControl.GetCharIndexFromPosition(olp.point);
				ol.EndPos	= ol.StartPos + ol.Header.Length;
				string ssss = _manager.textControl.Text.Substring(ol.StartPos,ol.EndPos-ol.StartPos);
				ssss="";
			}
			SetExpandLine();
			
		}

		private void SetExpandLine()
		{
			if(IsExpanded)
			{
				Point p = _picture.Location;
				int fontHeight=_manager.textControl.Font.Height;
				p.Y +=5;
				
				
				_expandPicture.Location=p;
				_expandPicture.Height=NumberOfLines*fontHeight;
				_expandPicture.SendToBack();
				_expandPicture.Visible=true;
		
				p.Y+=_expandPicture.Height;
				p.X-=1;
				_expandPictureEnd.Location=p;

				_expandPicture.SendToBack();
				_expandPictureEnd.SendToBack();
				
				_expandPictureEnd.Visible=true;
				_expandPicture.Visible=true;
			}
			else
			{
				_expandPicture.Visible=false;
				_expandPictureEnd.Visible=false;
				
			}
		}
//		void test()
//		{
//			Graphics g = Graphics.fr
//		}
//		float PaintFoldingText(Graphics g, 
//			int lineNumber, 
//			float physicalXPos, 
//			Rectangle lineRectangle, 
//			string text, bool drawSelected)
//		{
//			// TODO: get font and color from the highlighting file
//			HighlightColor      selectionColor  = textArea.Document.HighlightingStrategy.GetColorFor("Selection");
//			Brush               bgColorBrush    = drawSelected ? BrushRegistry.GetBrush(selectionColor.BackgroundColor) : GetBgColorBrush(lineNumber);
//			Brush               backgroundBrush = textArea.Enabled ? bgColorBrush : SystemBrushes.InactiveBorder;
//			
//			float wordWidth = g.MeasureString(text, textArea.Font, Int32.MaxValue, measureStringFormat).Width;
//			RectangleF rect = new RectangleF(physicalXPos, lineRectangle.Y, wordWidth, lineRectangle.Height - 1);
//			
//			g.FillRectangle(backgroundBrush, rect);
//			
//			g.DrawRectangle(BrushRegistry.GetPen(drawSelected ? Color.DarkGray : Color.Gray), rect.X, rect.Y, rect.Width, rect.Height);
//			
//			physicalColumn += text.Length;
//			g.DrawString(text,
//				textArea.Font,
//				BrushRegistry.GetBrush(drawSelected ? selectionColor.Color : Color.Gray),
//				rect, 
//				measureStringFormat);
//			
//			return (float)Math.Ceiling(rect.Right);
//		}
		#endregion
		#region Public Methods
		public void Init()
		{
			_panel.Controls.Add(_picture);
			_panel.Controls.Add(_expandPicture);
			_panel.Controls.Add(_expandPictureEnd);
			_isInitiated=true;
		}
		public bool IsAffected(int CursorPosition)
		{
			if(_startPos>=CursorPosition && _endPos<=CursorPosition)
				return true;
			else
				return false;
		}

		public bool IsValid(string text, int startLine, int endLine)
		{
			if( (this.Text==text.Replace("\r","\n")) && 
				this.StartLine==startLine &&
				this.EndLine==endLine)
				return true;
			else
				return false;
		}
		#endregion
	}

	public class OutLineCollection :CollectionBase	
	{
		public virtual int Add(OutLine newOutLine)
		{
			if((newOutLine.StartLine==newOutLine.EndLine) && newOutLine.IsExpanded)
				return 0;
			foreach(OutLine ol in this)
			{
				if(ol==newOutLine)
					return 0;
			}
			newOutLine.Init();
			return this.List.Add(newOutLine);  
			Console.WriteLine("Added:[" + newOutLine.Text + " ]");
		}
		public virtual OutLine this[int Index]
		{
			get
			{
				return (OutLine)this.List[Index];        
			}
		}
		protected override void OnClear()
		{
			foreach(OutLine ol in this)
			{
				ol.panel.Controls.Remove(ol.Picture);
			}

			base.OnClear ();
		}
		public void Delete(int index)
		{
			OutLine ol = this[index];
			string text = ol.Manager.textControl.GetText(ol.StartPos, ol.EndPos);// .SelectedText;

			if(text==ol.Header)
				return;

			ol.panel.Controls.Remove(ol.Picture);
			ol.panel.Controls.Remove(ol.ExpandPicture);
			ol.panel.Controls.Remove(ol.ExpandPictureEnd);
			
			this.RemoveAt(index);
			Console.WriteLine("Remove[ " + ol.Text.Replace("\n"," ") + " ] picCount:" + ol.Manager.panel.Controls.Count.ToString() + " OLCount:" +ol.Manager.outLineCollection.Count.ToString());

		}
	}

	public class OutLinePosition
	{
		public OutLinePosition(OutLine ol, Point p)
		{
			this.outLine=ol;
			this.point=p;
		}
		public OutLine outLine;
		public Point point;
	}
}
