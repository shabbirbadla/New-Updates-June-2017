using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using DynamicFormClass;

namespace DynamicMasterDesigner
{
    public partial class frmSearch : Form
    {
        public string _formCode;

        //public frmSearch()
        //{
        //    InitializeComponent();
        //}
        string[] criteria = new string[] { "caption", "code", "table_name" };

        public frmSearch(string formCode)
        {
            _formCode = formCode;
            InitializeComponent();
        }

        private void frmSearch_Load(object sender, EventArgs e)
        {
            textBoxSearch.Text = _formCode;
            BindData(textBoxSearch.Text);
            statusLabelFilterColumn.Text += "CAPTION, CODE, TABLE_NAME";
        }

        private void textBoxSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                e.Handled = true; //I handle it... Don't pass it to other windows
                //this.SelectNextControl(textBox1, true, true, true, true); // goto next control
                //if (textBoxSearch.Text != _formCode)
                BindData(textBoxSearch.Text);
            }
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            BindData(textBoxSearch.Text);
        }

        private void BindData(string formCode)
        {
            try
            {
                DataSet dsFormList = new DataSet();
                clsDynamicForm oForm = new clsDynamicForm();
                DataAccess_Net.clsDataAccess oDataAccess = new DataAccess_Net.clsDataAccess();
                //dsFormList = oForm.GetFormList(textBoxSearch.Text);
                //criteria
                dsFormList = oForm.GetFormList1(textBoxSearch.Text, criteria, oDataAccess);
                gridFormList.DataSource = dsFormList.Tables[0];
                gridViewFormList.Columns["ID"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void gridViewFormList_DoubleClick(object sender, EventArgs e)
        {
            object formCode = "";
            int rowIndex = ((DevExpress.XtraGrid.Views.Base.ColumnView)(sender)).FocusedRowHandle;
            formCode = gridViewFormList.GetRowCellValue(rowIndex, "CODE");
            textBoxSearch.Text = formCode.ToString();
            _formCode = formCode.ToString();
            this.Close();
        }

        private void gridFormList_Click(object sender, EventArgs e)
        {
            //gridViewFormList.FocusedColumn.Name.ToString()
            checkBoxDefaultSearch.Enabled = true;
            if (!checkBoxDefaultSearch.Checked)
            {
                switch (gridViewFormList.FocusedColumn.Name.ToUpper())
                {
                    case "COLCAPTION":
                        statusLabelFilterColumn.Text = "Filter Column: CAPTION";
                        criteria = new string[] { "CAPTION" };
                        break;
                    case "COLCODE":
                        statusLabelFilterColumn.Text = "Filter Column: CODE";
                        criteria = new string[] { "CODE" };
                        break;
                    case "COLTABLE_NAME":
                        statusLabelFilterColumn.Text = "Filter Column: TABLE_NAME";
                        criteria = new string[] { "TABLE_NAME" };
                        break;
                }
            }
            //else
            //{
            //    statusLabelFilterColumn.Text = "Filter Column: CAPTION, CODE, TABLE_NAME";
            //    criteria = new string[] { "CAPTION", "CODE", "TABLE_NAME" };
            //}
        }

        private void checkBoxDefaultSearch_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxDefaultSearch.Checked)
            {
                statusLabelFilterColumn.Text = "Filter Column: CAPTION, CODE, TABLE_NAME";
                criteria = new string[] { "CAPTION", "CODE", "TABLE_NAME" };
            }
        }

    }
}
