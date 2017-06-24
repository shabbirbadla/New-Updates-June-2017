using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Printing;
using System.Collections;
using System.Data;

namespace SQLEditor.General
{
	/// <summary>
	/// Summary description for GridPrinterClass.
	/// </summary>
	/// <summary>
	/// Summary description for PrinterClass.
	/// </summary>
	public class GridPrinterClass
	{
		//clone of Datagrid
		private Grid PrintGrid;
		
		//printdocument for initial printer settings
		private PrintDocument PrintDoc;

		//defines whether the grid is ordered right to left
		private bool bRightToLeft;
		
		//Current Top
		private float CurrentY = 0;

		//Current Left
		private float CurrentX = 0;

		//CurrentRow to print
		private int CurrentRow = 0;

		//Page Counter
		public int PageCounter=0;

		/// <summary>
		/// Constructor Class
		/// </summary>
		/// <param name="pdocument"></param>
		/// <param name="dgrid"></param>
		public GridPrinterClass(PrintDocument pdocument,DataGrid dgrid)
		{
			PrintGrid = new Grid(dgrid);
			PrintDoc = pdocument;

			//The grid columns are right to left
			bRightToLeft = dgrid.RightToLeft==RightToLeft.Yes;

			//init CurrentX and CurrentY
			CurrentY = pdocument.DefaultPageSettings.Margins.Top;
			CurrentX =  pdocument.DefaultPageSettings.Margins.Left;
			

		}

		public bool Print(Graphics g,ref float currentX,ref float currentY)
		{
			//use predefined area
			CurrentX = currentX;
			CurrentY = currentY;

			PrintHeaders(g);
			bool Morepages = PrintDataGrid(g);

			currentY = CurrentY;
			currentX = CurrentX;

			return Morepages;

			
		}

		public bool Print(Graphics g)
		{
			CurrentX = PrintDoc.DefaultPageSettings.Margins.Left;
			CurrentY = PrintDoc.DefaultPageSettings.Margins.Top;
			PrintHeaders(g);
			return PrintDataGrid(g);
		}

		/// <summary>
		/// Print the Grid Headers
		/// </summary>
		/// <param name="g"></param>
		private void PrintHeaders(Graphics g)
		{
			StringFormat sf = new StringFormat();

			//if we want to print the grid right to left
			if (bRightToLeft)
			{
				CurrentX = PrintDoc.DefaultPageSettings.PaperSize.Width - PrintDoc.DefaultPageSettings.Margins.Right;
				sf.FormatFlags = StringFormatFlags.DirectionRightToLeft;
			}
			else
			{
				CurrentX = PrintDoc.DefaultPageSettings.Margins.Left;
			}

			for (int i=0;i<PrintGrid.Columns;i++)
			{
				//set header alignment
				switch (((Header)PrintGrid.Headers.GetValue(i)).Alignment)
				{
						//left
					case HorizontalAlignment.Left:
						sf.Alignment = StringAlignment.Near;
						break;

						//right
					case HorizontalAlignment.Center:
						sf.Alignment = StringAlignment.Center;
						break;

						//right
					case HorizontalAlignment.Right:
						sf.Alignment = StringAlignment.Far;
						break;
				}

				//advance X according to order
				if (bRightToLeft)
				{
						
					//draw the cell bounds (lines) and back color
					g.FillRectangle(new SolidBrush(PrintGrid.HeaderBackColor),CurrentX - PrintGrid.Headers[i].Width,CurrentY,PrintGrid.Headers[i].Width,PrintGrid.Headers[i].Height);
					g.DrawRectangle(new Pen(PrintGrid.LineColor),CurrentX - PrintGrid.Headers[i].Width,CurrentY,PrintGrid.Headers[i].Width,PrintGrid.Headers[i].Height);
					
					
					//draw the cell text
					g.DrawString(PrintGrid.Headers[i].Text,PrintGrid.Headers[i].Font,new SolidBrush(PrintGrid.HeaderForeColor),new RectangleF(CurrentX - PrintGrid.Headers[i].Width,CurrentY,PrintGrid.Headers[i].Width,PrintGrid.Headers[i].Height),sf);
			
					//next cell
					CurrentX -=PrintGrid.Headers[i].Width;
					
				}
				else
				{
						
					//draw the cell bounds (lines) and back color
					g.FillRectangle(new SolidBrush(PrintGrid.HeaderBackColor),CurrentX,CurrentY,PrintGrid.Headers[i].Width,PrintGrid.Headers[i].Height);
					g.DrawRectangle(new Pen(PrintGrid.LineColor),CurrentX,CurrentY,PrintGrid.Headers[i].Width,PrintGrid.Headers[i].Height);
					
					
					//draw the cell text
					g.DrawString(PrintGrid.Headers[i].Text,PrintGrid.Headers[i].Font,new SolidBrush(PrintGrid.HeaderForeColor),new RectangleF(CurrentX,CurrentY,PrintGrid.Headers[i].Width,PrintGrid.Headers[i].Height),sf);
					
					//next cell
					CurrentX +=PrintGrid.Headers[i].Width;
				}
					
					
			}

			//reset to beginning
			if (bRightToLeft)
			{
				//right align
				CurrentX = PrintDoc.DefaultPageSettings.PaperSize.Width - PrintDoc.DefaultPageSettings.Margins.Right;
			}
			else
			{
				//left align
				CurrentX = PrintDoc.DefaultPageSettings.Margins.Left;
			}

			//advance to next row
			CurrentY = CurrentY + ((Header)(PrintGrid.Headers.GetValue(0))).Height;
			 
		}

		private bool PrintDataGrid(Graphics g)
		{	
			StringFormat sf = new StringFormat();
			PageCounter++;
			
			//if we want to print the grid right to left
			if (bRightToLeft)
			{
				CurrentX = PrintDoc.DefaultPageSettings.PaperSize.Width - PrintDoc.DefaultPageSettings.Margins.Right;
				sf.FormatFlags = StringFormatFlags.DirectionRightToLeft;
			}
			else
			{
				CurrentX = PrintDoc.DefaultPageSettings.Margins.Left;
			}

			for (int i=CurrentRow;i<PrintGrid.Rows;i++)
			{
				for (int j=0;j<PrintGrid.Columns;j++)
				{
					//set cell alignment
					switch (PrintGrid[i,j].Alignment)
					{
							//left
						case HorizontalAlignment.Left:
							sf.Alignment = StringAlignment.Near;
							break;

							//center
						case HorizontalAlignment.Center:
							sf.Alignment = StringAlignment.Center;
							break;

							//right
						case HorizontalAlignment.Right:
							sf.Alignment = StringAlignment.Far;
							break;
					}
					
					//advance X according to order
					if (bRightToLeft)
					{
						
						//draw the cell bounds (lines) and back color
						g.FillRectangle(new SolidBrush(PrintGrid.BackColor),CurrentX - PrintGrid[i,j].Width,CurrentY,PrintGrid[i,j].Width,PrintGrid[i,j].Height);
						g.DrawRectangle(new Pen(PrintGrid.LineColor),CurrentX - PrintGrid[i,j].Width,CurrentY,PrintGrid[i,j].Width,PrintGrid[i,j].Height);
						
					
						//draw the cell text
						g.DrawString(PrintGrid[i,j].Text,PrintGrid[i,j].Font,new SolidBrush(PrintGrid.ForeColor),new RectangleF(CurrentX - PrintGrid[i,j].Width,CurrentY,PrintGrid[i,j].Width,PrintGrid[i,j].Height),sf);

						//next cell
						CurrentX -=PrintGrid[i,j].Width;
					
					}
					else
					{
						
						//draw the cell bounds (lines) and back color
						g.FillRectangle(new SolidBrush(PrintGrid.BackColor) ,CurrentX,CurrentY,PrintGrid[i,j].Width,PrintGrid[i,j].Height);
						g.DrawRectangle(new Pen(PrintGrid.LineColor) ,CurrentX,CurrentY,PrintGrid[i,j].Width,PrintGrid[i,j].Height);
						
					
						//draw the cell text
						//Draw text by alignment
						
							
						g.DrawString(PrintGrid[i,j].Text,PrintGrid[i,j].Font,new SolidBrush(PrintGrid.ForeColor),new RectangleF(CurrentX,CurrentY,PrintGrid[i,j].Width,PrintGrid[i,j].Height),sf);
							
						//next cell
						CurrentX +=PrintGrid[i,j].Width;
					}
					
					
				}

				//reset to beginning
				if (bRightToLeft)
				{
					//right align
					CurrentX = PrintDoc.DefaultPageSettings.PaperSize.Width - PrintDoc.DefaultPageSettings.Margins.Right;
				}
				else
				{
					//left align
					CurrentX = PrintDoc.DefaultPageSettings.Margins.Left;
				}
				
				//advance to next row
				CurrentY = CurrentY + PrintGrid[i,0].Height;
				CurrentRow++;
				//if we are beyond the page margin (bottom) then we need another page,
				//return true
				if (CurrentY > PrintDoc.DefaultPageSettings.PaperSize.Height - PrintDoc.DefaultPageSettings.Margins.Bottom)
				{
					return true;
				}
			}
			return false;
		}

	}
	public class Grid
	{
		
		private Font fontGridFont;
		private Font fontGridHeadersFont;
		
		public Color HeaderBackColor;
		public Color HeaderForeColor;
		public Color LineColor;
		public Color ForeColor;
		public Color BackColor;
		
		private int rows;
		private int columns;

		public Cell[,] Cells;
		
		public Header[] Headers;

		public Header this[int Column]
		{
			get {return Headers[columns];}
			
		}

		/// <summary>
		/// The Font of the text in the cells
		/// </summary>
		public Font Font
		{
			get {return fontGridFont;}
			set {fontGridFont = value;}
		}

		/// <summary>
		/// The Font of the text in the header cells
		/// </summary>
		public Font HeaderFont
		{
			get {return fontGridHeadersFont;}
			set {fontGridHeadersFont = value;}
		}

		public int Rows
		{
			get {return rows;}
		}

		public int Columns
		{
			get {return columns;}
		}

		/// <summary>
		/// Gets or Sets a Cell
		/// </summary>
		public Cell this[int RowNumber,int ColumnNumber]
		{
			get 
			{
				//check to see if the cell exists
				if (RowNumber>=0 && ColumnNumber>=0 && RowNumber<=Cells.GetUpperBound(0) && ColumnNumber<=Cells.GetUpperBound(1))
				{
					//return found cell
					return Cells[RowNumber,ColumnNumber];
				}
				else
				{
					//error - no cell found
					return null;
					//throw new NoCellException
				}
			}
			set
			{
				//Check the number of Cell to exist
				if (RowNumber>=0 && ColumnNumber>=0 && RowNumber<=Cells.GetUpperBound(0) && ColumnNumber<=Cells.GetUpperBound(1))
				{
					//set value
					Cells[RowNumber,ColumnNumber]=value;
				}
				else
				{
					//throw new NoCellException
				}
			}
		}

		/// <summary>
		/// Set a new value for a cell
		/// </summary>
		public Cell this[Cell cell]
		{
			set {Cells[cell.RowNumber,cell.ColumnNumber]=value;}
		}

		public Grid(DataGrid TheGrid)
		{
			try
			{
				//get the Data in the grid
				DataTable TableGrid = null;

				if (TheGrid.DataSource.GetType()==typeof(DataView))
				{
					DataView ViewGrid = (DataView)TheGrid.DataSource;
					TableGrid = ViewGrid.Table;
					
				}
				else
				{
					if(TheGrid.DataSource is DataSet)
						TableGrid = ((DataSet) TheGrid.DataSource).Tables[0];
					else
						TableGrid = (DataTable)TheGrid.DataSource;
				}
				//set number of rows
				rows = TableGrid.Rows.Count;
			
				//set number of columns
				//first check if the grid has tablestyle and columnstyle
				
				//check for table styles
				if (TheGrid.TableStyles.Count==0)
				{
					//create table style and column style
					CreateColumnStyles(TheGrid,TableGrid);
				}
				else
				{
					//create column styles if there are none
					if (TheGrid.TableStyles[TableGrid.TableName].GridColumnStyles.Count==0)
						CreateColumnStyles(TheGrid,TableGrid);
				}
			
				//set number of columns
				columns = TheGrid.TableStyles[TableGrid.TableName].GridColumnStyles.Count;

				

				

				Cells = new Cell[Rows,Columns];

				//Copy Cells
				for (int i=0;i<Rows;i++)
				{
					for (int j=0;j<Columns;j++)
					{
						Cells[i,j] = new Cell(i,j,TheGrid.Font,TheGrid.TableStyles[TableGrid.TableName].GridColumnStyles[j].Alignment,TheGrid.GetCellBounds(i,j),TheGrid[i,j].ToString());
					
					
					}
				}

				//init number of columns to headers
				Headers = new Header[Columns];
				SetHeaders(TheGrid,TableGrid);
				
				//define grid colors
				SetColors(TheGrid);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		private void CreateColumnStyles(DataGrid TheGrid,DataTable TableGrid)
		{
			// Define new table style.
			DataGridTableStyle tableStyle = new DataGridTableStyle();
			
			Graphics g = TheGrid.CreateGraphics();

			try
			{
            
				// Clear any existing table styles.
				TheGrid.TableStyles.Clear();
  
				// Use mapping name that is defined in the data source.
				tableStyle.MappingName = TableGrid.TableName;
  
				// Now create the column styles within the table style.
				DataGridTextBoxColumn columnStyle;
				
				
				for (int iCurrCol = 0; iCurrCol < TableGrid.Columns.Count; 
					iCurrCol++)
				{
					DataColumn dataColumn = TableGrid.Columns[iCurrCol];
    
					columnStyle = new DataGridTextBoxColumn();

					
					columnStyle.HeaderText = dataColumn.ColumnName;
					columnStyle.MappingName = dataColumn.ColumnName; 


					
		
					
					columnStyle.TextBox.Width = TheGrid.GetCellBounds(0,iCurrCol).Width;  

					columnStyle.TextBox.Height = (int)g.MeasureString(columnStyle.HeaderText,TheGrid.HeaderFont).Height + 10;

					
					// Add the new column style to the table style.
					tableStyle.GridColumnStyles.Add(columnStyle);
				}    

				
				// Add the new table style to the data grid.
			
				TheGrid.TableStyles.Add(tableStyle);
			}
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
			}
			finally
			{
				g.Dispose();
			}
			
		}
		private void SetHeaders(DataGrid TheGrid,DataTable TableGrid)
		{
			try
			{
				//Check if there are styles
				if (TheGrid.TableStyles.Count>0)
				{
					if (TheGrid.TableStyles[TableGrid.TableName].GridColumnStyles.Count>0)	 
					{
						for (int i=0;i<=Headers.GetUpperBound(0);i++)
						{
							//Known bug - when there are no rows headers are not displayed properly
							DataGridTextBoxColumn columnStyle = (DataGridTextBoxColumn)TheGrid.TableStyles[TableGrid.TableName].GridColumnStyles[i];
							if (Cells.GetUpperBound(0)>0)
							{
								Headers[i] = new Header(i,TheGrid.HeaderFont,columnStyle.Alignment,new RectangleF(Cells[0,i].Location.X,columnStyle.TextBox.Bounds.Y,Cells[0,i].Location.Width,((DataGridTextBoxColumn)TheGrid.TableStyles[TableGrid.TableName].GridColumnStyles[0]).TextBox.Height),columnStyle.HeaderText);
							}
							else
							{
								Headers[i] = new Header(i,TheGrid.HeaderFont,columnStyle.Alignment,columnStyle.TextBox.Bounds,columnStyle.HeaderText);
							}
						}

					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		private void SetColors(DataGrid TheGrid)
		{
			HeaderBackColor = TheGrid.HeaderBackColor;
			HeaderForeColor = TheGrid.HeaderForeColor;
			LineColor = TheGrid.GridLineColor;
			ForeColor = TheGrid.ForeColor;
			BackColor = TheGrid.BackColor;
		}

	}
	public class Cell
	{
		private RectangleF rectLocation;

		private float fCellHeight;
		private float fCellWidth;
		private float fCellX;
		private float fCellY;

		private Font fontCellFont;
		private HorizontalAlignment haAlignment;
		private string sText;

		private int iColumnNumber;

		private int iRowNumber;

		/// <summary>
		/// The Font of the text in the cell
		/// </summary>
		public string Text
		{
			get {return sText;}
			set {sText = value;}
		}

		/// <summary>
		/// The Font of the text in the cell
		/// </summary>
		public Font Font
		{
			get {return fontCellFont;}
			set {fontCellFont = value;}
		}

		/// <summary>
		/// The Location of the cell
		/// </summary>
		public RectangleF Location
		{
			get {return rectLocation;}
		}


		/// <summary>
		/// Set The location of the Cell
		/// </summary>
		protected RectangleF RectLocation
		{
			set 
			{
				rectLocation = value;
				fCellWidth = value.Width;
				fCellHeight = value.Height;
				fCellX = value.X;
				fCellY = value.Y;

			}
		}

		/// <summary>
		/// Get the Height of the cell
		/// </summary>
		public float Height
		{
			get {return fCellHeight;}
		}

		/// <summary>
		/// Get the Height of the cell
		/// </summary>
		public float Width
		{
			get {return fCellWidth;}
		}

		/// <summary>
		/// Get the Height of the cell
		/// </summary>
		public float X
		{
			get {return fCellX;}
		}

		/// <summary>
		/// Get the Height of the cell
		/// </summary>
		public float Y
		{
			get {return fCellY;}
		}

		/// <summary>
		/// The Column number of the Cell
		/// </summary>
		public virtual int ColumnNumber
		{
			get {return iColumnNumber;}
		}

		/// <summary>
		/// The Row number of the cell
		/// </summary>
		public int RowNumber
		{
			get {return iRowNumber;}
		}

		/// <summary>
		/// The Horizonal Alignment cell
		/// </summary>
		public HorizontalAlignment Alignment
		{
			get {return haAlignment;}
			set {haAlignment = value;}
		}

		public Cell()
		{
			
		}

		/// <summary>
		/// Create New Cell
		/// </summary>
		/// <param name="RowNumber"></param>
		/// <param name="ColumnNumber"></param>
		/// <param name="cellfont"></param>
		/// <param name="align"></param>
		/// <param name="location"></param>
		/// <param name="text"></param>
		public Cell(int RowNumber,int ColumnNumber,Font cellfont,HorizontalAlignment align,RectangleF location,string text)
		{
			iRowNumber = RowNumber;
			iColumnNumber = ColumnNumber;
			fontCellFont = cellfont;
			haAlignment = align;
			RectLocation = location;
			sText = text;

		}

		/// <summary>
		/// Create new cell with default Text
		/// </summary>
		/// <param name="RowNumber"></param>
		/// <param name="ColumnNumber"></param>
		/// <param name="cellfont"></param>
		/// <param name="align"></param>
		/// <param name="location"></param>
		public Cell(int RowNumber,int ColumnNumber,Font cellfont,HorizontalAlignment align,RectangleF location)
		{
			iRowNumber = RowNumber;
			iColumnNumber = ColumnNumber;
			fontCellFont = cellfont;
			haAlignment = align;
			RectLocation = location;
			sText = "";
			
		}

		/// <summary>
		/// Create new cell with default Text and alignment
		/// </summary>
		/// <param name="RowNumber"></param>
		/// <param name="ColumnNumber"></param>
		/// <param name="cellfont"></param>
		/// <param name="location"></param>
		public Cell(int RowNumber,int ColumnNumber,Font cellfont,RectangleF location)
		{
			iRowNumber = RowNumber;
			iColumnNumber = ColumnNumber;
			fontCellFont = cellfont;
			haAlignment = HorizontalAlignment.Left;
			RectLocation = location;
			sText = "";

		}

		/// <summary>
		/// Create new cell with default alignment
		/// </summary>
		/// <param name="RowNumber"></param>
		/// <param name="ColumnNumber"></param>
		/// <param name="cellfont"></param>
		/// <param name="location"></param>
		public Cell(int RowNumber,int ColumnNumber,Font cellfont,RectangleF location,string text)
		{
			iRowNumber = RowNumber;
			iColumnNumber = ColumnNumber;
			fontCellFont = cellfont;
			haAlignment = HorizontalAlignment.Left;
			RectLocation = location;
			sText = text;
			
			

		}
	}
	public class Header : Cell
	{
		private int iColumnNumber;

		public override int ColumnNumber
		{
			get {return iColumnNumber;}
		}

		/// <summary>
		/// Create New Header
		/// </summary>
		/// <param name="RowNumber"></param>
		/// <param name="ColumnNumber"></param>
		/// <param name="cellfont"></param>
		/// <param name="align"></param>
		/// <param name="location"></param>
		/// <param name="text"></param>
		public Header(int ColumnNumber,Font Headerfont,HorizontalAlignment align,RectangleF location,string text)
		{
			iColumnNumber = ColumnNumber;
			Font = Headerfont;
			Alignment = align;
			RectLocation  = location;
			Text = text;
		}

		/// <summary>
		/// Create New Header with default alignment
		/// </summary>
		/// <param name="RowNumber"></param>
		/// <param name="ColumnNumber"></param>
		/// <param name="cellfont"></param>
		/// <param name="align"></param>
		/// <param name="location"></param>
		/// <param name="text"></param>
		public Header(int ColumnNumber,Font Headerfont,RectangleF location,string text)
		{
			iColumnNumber = ColumnNumber;
			Font = Headerfont;
			Alignment = HorizontalAlignment.Left;
			RectLocation  = location;
			Text = text;
		}
	}
}
