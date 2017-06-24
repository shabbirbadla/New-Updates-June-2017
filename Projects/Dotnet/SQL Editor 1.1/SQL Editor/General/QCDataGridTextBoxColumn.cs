using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace SQLEditor.General
{
	/// <summary>
	/// Summary description for QCDataGridTextBoxColumn.
	/// </summary>
	public class QCDataGridTextBoxColumn : System.Windows.Forms.DataGridTextBoxColumn 
	{ 
		private bool m_IsRedIfOverDue = false; 

		public QCDataGridTextBoxColumn(string format, string headerText, string mappingName, int width) 
		{ 
			base.Format = format; 
			base.HeaderText = headerText; 
			base.MappingName = mappingName; 
			base.Width = width; 
		} 

		public QCDataGridTextBoxColumn(string format, string headerText, string mappingName, 
			int width, bool isRedIfOverDue) 
		{ 
			base.Format = format; 
			base.HeaderText = headerText; 
			base.MappingName = mappingName; 
			base.Width = width; 
			m_IsRedIfOverDue = isRedIfOverDue; 
		} 

		protected override void Edit(System.Windows.Forms.CurrencyManager source, int rowNum, 
			System.Drawing.Rectangle bounds, bool isReadOnly, string instantText, bool cellIsVisible) 
		{ 
			// Do Nothing 
			// This is a ReadOnly DataGrid 
		} 

		protected override void Paint(System.Drawing.Graphics g, System.Drawing.Rectangle bounds, 
			System.Windows.Forms.CurrencyManager source, int rowNum, System.Drawing.Brush backBrush, 
			System.Drawing.Brush foreBrush, bool alignToRight) 
		{ 
			object bVal = GetColumnValueAtRow(source, rowNum); 

			if (this.Format == "d") 
			{ 
				try 
				{ 
					// globalize for german 
					if (Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName == "de") 
						bVal = String.Format("{0:d}", Convert.ToDateTime(bVal).ToString("dd/MM/yy")); 
					else 
						bVal = String.Format("{0:d}", Convert.ToDateTime(bVal)); 
				} 
				catch 
				{ 
					// ignore an invalid cast 
				} 
			} 

			// if the current row is this row, draw the selection back color 
			if (this.DataGridTableStyle.DataGrid.CurrentRowIndex == rowNum) 
			{ 
				g.FillRectangle(new SolidBrush(this.DataGridTableStyle.SelectionBackColor), bounds); 
				g.DrawString(Convert.ToString(bVal), this.DataGridTableStyle.DataGrid.Font, 
					new SolidBrush(this.DataGridTableStyle.SelectionForeColor), bounds.X + 2, bounds.Y + 2); 
			} 
			else 
			{ 
				g.FillRectangle(backBrush, bounds); 

				if (m_IsRedIfOverDue) 
				{ 
					try 
					{ 
						if (Convert.ToDateTime(bVal).Date < DateTime.Now.Date) 
							g.DrawString(Convert.ToString(bVal), this.DataGridTableStyle.DataGrid.Font, 
								new SolidBrush(Color.Red), bounds.X + 2, bounds.Y + 2); 
						else 
							g.DrawString(Convert.ToString(bVal), this.DataGridTableStyle.DataGrid.Font, foreBrush, 
								bounds.X + 2, bounds.Y + 2); 
					} 
					catch 
					{ 
						g.DrawString(Convert.ToString(bVal), this.DataGridTableStyle.DataGrid.Font, foreBrush, 
							bounds.X + 2, bounds.Y + 2); 
					} 
				} 
				else 
				{ 
					g.DrawString(Convert.ToString(bVal), this.DataGridTableStyle.DataGrid.Font, foreBrush, 
						bounds.X + 2, bounds.Y + 2); 
				} 
			} 
		} 

	}
}
