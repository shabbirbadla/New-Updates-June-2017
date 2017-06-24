using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Form_231
{
    [ToolboxItem(false)]
    public partial class UCCustomToolTip : UserControl
    {
        #region Control Properties
        private Button btn_Done;
        public Button Btn_Done
        {
            get { return btn_Done; }
            set { btn_Done = value; }
        }
        private ComboBox cbo_Select;
        public ComboBox Cbo_Select
        {
            get { return cbo_Select; }
            set { cbo_Select = value; }
        }
        #endregion

        public UCCustomToolTip()
        {
            InitializeComponent();
            Btn_Done = btnDone;
            Cbo_Select = cboSelect;
        }
    }
}
