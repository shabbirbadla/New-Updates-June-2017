using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
static class clsWaitDialog
{
	private static DevExpress.Utils.WaitDialogForm Dlg = null;
	public static void CreateWaitDialog(string Caption, string Title)
	{
		if (string.IsNullOrEmpty(Title)) {
			Dlg = new DevExpress.Utils.WaitDialogForm(Caption);
		} else {
			Dlg = new DevExpress.Utils.WaitDialogForm(Caption, Title);
		}
	}
	public static void CloseWaitDialog()
	{
		Dlg.Close();
	}
	public static void SetWaitDialogCaption(string fCaption)
	{
		if (Dlg != null) {
			Dlg.Caption = fCaption;
		}
	}
}
