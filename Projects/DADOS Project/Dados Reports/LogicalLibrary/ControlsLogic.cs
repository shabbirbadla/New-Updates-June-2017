using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.XtraEditors.Controls;
using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.XtraEditors;
using DevExpress.XtraNavBar;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;

namespace LogicalLibrary
{
    public class ControlsLogic
    {
        public ControlsLogic()
        {

        }

        public int IndexOf(ListBoxItemCollection itemsCollection, string s)
        {
            for (int i = 0; i < itemsCollection.Count; i++)
                if (itemsCollection[i].ToString() == s) return i;
            return -1;
        }


        public ComboBoxEdit InitSkinNames(UserLookAndFeel lookAndFeel, ComboBoxEdit comboBoxEdit)
        {
            foreach (SkinContainer cnt in SkinManager.Default.Skins)
                comboBoxEdit.Properties.Items.Add(cnt.SkinName);
            comboBoxEdit.EditValue = lookAndFeel.SkinName;

            return comboBoxEdit;
        }

        public ListBoxControl bindListBox(ListBoxControl listBoxTheme, NavBarControl navBarControl)
        {
            listBoxTheme.Items.AddRange(navBarControl.AvailableNavBarViews.ToArray(typeof(object)) as object[]);
            listBoxTheme.SelectedIndex = IndexOf(listBoxTheme.Items, navBarControl.View.ToString());

            return listBoxTheme;
        }

        public string GetCellHintText(GridView view, int rowHandle, DevExpress.XtraGrid.Columns.GridColumn column)
        {
            string ret = view.GetRowCellDisplayText(rowHandle, column);
            foreach (DevExpress.XtraGrid.Columns.GridColumn col in view.Columns)
                if (col.VisibleIndex < 0)
                    ret += string.Format("\r\n {0}: {1}", col.Caption, view.GetRowCellDisplayText(rowHandle, col));
            return ret;
        }

    }
}
