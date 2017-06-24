using System;
using System.Collections.Generic;
using System.Text;

using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Base;
using System.IO;
using DevExpress.XtraExport;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Export;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors;
using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.XtraGrid.Columns;
using DevExpress.Utils.Menu;
using  System.Net;
using System.Drawing;

namespace LogicalLibrary
{
    public class MenuInfo
    {
        public MenuInfo()
        { 
        
        }
        public MenuInfo(GridColumn column, FixedStyle style)
        {
            this.Column = column;
            this.Style = style;
        }
        public FixedStyle Style;
        public GridColumn Column;


        public DevExpress.Utils.Menu.DXMenuItem CreateCheckItem(string caption, GridColumn column, FixedStyle style,Image image)
        {
            DXMenuCheckItem item = new DXMenuCheckItem(caption, column.Fixed == style, image, new EventHandler(OnFixedClick));
            item.Tag = new MenuInfo(column, style);
            return item;
        }

        public void OnFixedClick(object sender, EventArgs e)
        {
            DXMenuItem item = sender as DXMenuItem;
            MenuInfo info = item.Tag as MenuInfo;
            if (info == null) return;
            info.Column.Fixed = info.Style;
        }
    }
}
