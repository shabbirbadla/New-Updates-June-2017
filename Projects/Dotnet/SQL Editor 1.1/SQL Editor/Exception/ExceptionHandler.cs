using System;
using System.Diagnostics;
using SQLEditor.WinGui;

namespace SQLEditor
{
	/// <summary>
	/// Summary description for Exception.
	/// </summary>
	/// 
	/// <revision author="wmmabet" date="2004-01-13">Removed warning/fixed xml-comments.</revision>
	public abstract class ExceptionHandler
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ExceptionHandler()
		{
		}

		/// <summary>
		/// Error hanlding function.
		/// </summary>
		/// <param name="currentForm">Current form</param>
		/// <param name="e">Exception</param>
		public static void ErrorHandler(System.Windows.Forms.Form currentForm, Exception e)
		{
			if(e is System.Reflection.TargetInvocationException)
				e = e.InnerException;
                

			FrmExceptionMessage frm = new FrmExceptionMessage(currentForm, e);
			frm.ShowDialog();
		}
	}
}
