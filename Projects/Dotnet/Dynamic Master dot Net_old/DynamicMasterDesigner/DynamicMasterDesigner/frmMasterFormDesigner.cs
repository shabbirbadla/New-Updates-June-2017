using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;
using System.Text.RegularExpressions;
using System.Globalization;
using cST3SqlConn;
using udSelectPop;
using System.Data.SqlClient;
using DynamicMaster;
using DynamicFormClass;

namespace DynamicMasterDesigner
{
    public partial class frmMasterFormDesigner : Form, IDisposable
    {
        private DataTable dtTabs = new DataTable();
        private DataTable dtRemovedTabs = new DataTable();
        private DataTable dtFields = new DataTable();
        private DataTable dtFieldsOriginal = new DataTable();
        private DataTable dtRemovedFields = new DataTable();
        private DataTable dtDatatypes = new DataTable();
        private DataSet dsForms = new DataSet();
        DataAccess_Net.clsDataAccess oDataAccess;// = new DataAccess_Net.clsDataAccess();
        clsDynamicForm oForm = new clsDynamicForm();
        private FormMode _mode;
        private string _ErrorMessage;
        private DataSet dsTempData = new DataSet();
        private string _TabCode;
        private string _CompId;
        private int _Range;
        private string _UserName;

        private enum FormMode
        {
            Add, Edit, View, Cancel
        };

        public enum Mask
        {
            None, DateOnly, PhoneWithArea, IpAddress, SSN, Decimal, Digit
        };

        private Mask m_mask;

        public Mask Maked
        {
            get { return m_mask; }
            set
            {
                m_mask = value;
                this.Text = "";
            }
        }

        public frmMasterFormDesigner(string[] param)
        {
            InitializeComponent();

            //Bind Datatypes dropdown
            this.dtDatatypes.Columns.Add("Datatype", typeof(string));
            this.dtDatatypes.Rows.Add(new object[] { "Varchar" });
            this.dtDatatypes.Rows.Add(new object[] { "Decimal" });
            this.dtDatatypes.Rows.Add(new object[] { "Bit" });
            this.dtDatatypes.Rows.Add(new object[] { "Datetime" });
            this.dtDatatypes.Rows.Add(new object[] { "Text" });
            repositoryItemDatatypeLookup.DataSource = dtDatatypes;

            _CompId = param[0];
            DataAccess_Net.clsDataAccess._databaseName = param[1];
            DataAccess_Net.clsDataAccess._serverName = param[2];
            DataAccess_Net.clsDataAccess._userID = param[3];
            DataAccess_Net.clsDataAccess._password = param[4];
            oDataAccess = new DataAccess_Net.clsDataAccess();
            _Range = Convert.ToInt32(param[5].ToString().Replace("^", ""));
            _UserName = param[6];
        }

        private void InitializeDataTables()
        {
            this.dtTabs.Columns.Add("CAPTION", typeof(string));
            this.dtTabs.Columns.Add("CODE", typeof(string));
            this.dtTabs.Columns.Add("TAB_ORDER", typeof(int));
            this.dtTabs.Columns.Add("FORM_CODE", typeof(string));
            this.dtTabs.Columns.Add("ID", typeof(int));

            //string rowNumber = Convert.ToString(1);
            //this.dtTabs.Rows.Add(new object[] { "General" + rowNumber, "Gen" + rowNumber, rowNumber });
            //this.gridTabControl.DataSource = this.dtTabs;
            //this.gridViewTabControls.Columns["FORM_ID"].Visible = false;
            //this.gridViewTabControls.Columns["ID"].Visible = false;

            this.dtFields.Columns.Add("ID", typeof(int));
            this.dtFields.Columns.Add("SELECTEDTAB", typeof(string));
            this.dtFields.Columns.Add("FIELD_ORDER", typeof(int));
            this.dtFields.Columns.Add("CAPTION", typeof(string));
            this.dtFields.Columns.Add("TOOLTIP", typeof(string));
            this.dtFields.Columns.Add("MANDATORY", typeof(bool));
            this.dtFields.Columns.Add("FIELDNAME", typeof(string));
            this.dtFields.Columns.Add("DATATYPE", typeof(string));
            this.dtFields.Columns.Add("SIZE", typeof(int));
            this.dtFields.Columns.Add("DECIMAL", typeof(int));
            this.dtFields.Columns.Add("UNIQUE", typeof(bool));
            this.dtFields.Columns.Add("INPUTMASK", typeof(string));
            this.dtFields.Columns.Add("HELPQUERY", typeof(string));
            this.dtFields.Columns.Add("REMARKS", typeof(string));
            this.dtFields.Columns.Add("WHENCONDITION", typeof(string));
            this.dtFields.Columns.Add("DEFAULTVALUE", typeof(string));
            this.dtFields.Columns.Add("VALIDATION", typeof(string));
            this.dtFields.Columns.Add("INTERNALUSE", typeof(bool));
            this.dtFields.Columns.Add("FORM_CODE", typeof(string));
            this.dtFields.Columns.Add("TAB_CODE", typeof(string));
            this.dtFields.Columns.Add("STATE", typeof(string));

            this.dtRemovedTabs.Columns.Add("ID", typeof(int));
            this.dtRemovedTabs.Columns.Add("ROW_INDEX", typeof(int));

            this.dtRemovedFields.Columns.Add("ID", typeof(int));
            this.dtRemovedFields.Columns.Add("ROW_INDEX", typeof(int));
            this.dtRemovedFields.Columns.Add("TABLE_NAME", typeof(string));
            this.dtRemovedFields.Columns.Add("FIELD_NAME", typeof(string));
            //this.dtFields.Rows.Add(new object[] { "Gen1", rowNumber, "", "", 0, "", "Varchar", 
            //20, 0, 0, "", "", "", "", "", "", 0, 0});
            //this.gridFields.DataSource = this.dtFields;   
        }

        private void frmMasterFormDesigner_Load(object sender, EventArgs e)
        {
            InitializeDataTables();
            this.WindowState = FormWindowState.Maximized;
            _mode = FormMode.View;
            clsWaitDialog.CreateWaitDialog("Building data", "Please wait while data is built.");
            BindData1(string.Empty);
            //ToggleButtons();
            //SetBindingContextPosition("Last", 0);
            clsWaitDialog.SetWaitDialogCaption("Setting appearances..");
            Color c1 = GetFormColor();
            this.BackColor = Color.FromArgb(c1.R, c1.R, c1.G, c1.B); //SetFormColor();
            //SetMenuRights();
            labelCaption.BackColor = Color.Transparent;
            labelCode.BackColor = Color.Transparent;
            labelTableName.BackColor = Color.Transparent;
            clsWaitDialog.CloseWaitDialog();
        }

        private void SetMenuRights()
        {
            btnNew.Enabled = false;
            btnEdit.Enabled = false;
            menuItemRemoveToolbar.Enabled = false;
            btnPrint.Enabled = false;

            DataSet dsRights = new DataSet();

            dsRights = oForm.GetUserRightsForMenu(_Range, _UserName, oDataAccess);
            if (dsRights != null)
            {
                if (dsRights.Tables.Count > 0)
                {
                    string rights = dsRights.Tables[0].Rows[0]["rights"].ToString();
                    int len = rights.Length;
                    string newString = "";
                    ArrayList myArray = new ArrayList();

                    while (len > 2)
                    {
                        newString = rights.Substring(0, 2);
                        rights = rights.Substring(2);
                        myArray.Add(newString);
                        len = rights.Length;
                    }
                    myArray.Add(rights);

                    btnNew.Enabled = (myArray[0].ToString().Trim() == "IY" ? true : false);
                    btnNew.Enabled = false;
                    btnEdit.Enabled = (myArray[1].ToString().Trim() == "CY" ? true : false);
                    menuItemRemoveToolbar.Enabled = (myArray[2].ToString().Trim() == "DY" ? true : false);
                    btnPrint.Enabled = (myArray[3].ToString().Trim() == "PY" ? true : false);
                }
                else
                {
                    MessageBox.Show("Menu Rights not found", "Info");
                }
            }
        }

        private Color GetFormColor()
        {
            string colorCode = string.Empty;
            Color myColor = Color.Coral;
            colorCode = oForm.GetColorCode(Convert.ToInt32(_CompId), oDataAccess);
            if (!string.IsNullOrEmpty(colorCode))
            {
                this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                myColor = Color.FromArgb(Convert.ToInt32(colorCode.Trim()));
            }
            return myColor;
        }

        #region GridView Events

        private void gridViewTabControls_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            GridView view = sender as GridView;
            DataRowView row = e.Row as DataRowView;
            int currentRowHandle = gridViewTabControls.GetVisibleRowHandle(e.RowHandle);

            ValidateGridRow("gridViewTabControls", currentRowHandle, view, row, e);
        }

        private void gridViewFields_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            GridView view = sender as GridView;
            DataRowView row = e.Row as DataRowView;
            int currentRowHandle = gridViewFields.GetVisibleRowHandle(e.RowHandle);
            //gridViewFields.PostEditor();
            //gridViewFields.UpdateCurrentRow();
            ValidateGridRow("gridViewFields", currentRowHandle, view, row, e);
        }

        private void ValidateGridRow(string gridName, int currentRowHandle, GridView view, DataRowView row,
                                     DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            switch (gridName)
            {
                case "gridViewTabControls":
                    //Validate Caption
                    if (string.IsNullOrEmpty(row["CAPTION"].ToString()))
                    {
                        SetGridViewRowError(view, "CAPTION", e, "Caption cannot be empty.");
                    }
                    else
                    {
                        if (searchForDuplicates(gridViewTabControls, view.Columns["CAPTION"].Name,
                                                row["CAPTION"].ToString(), currentRowHandle))
                        {
                            SetGridViewRowError(view, "CAPTION", e, "Caption already exists. Choose another caption.");
                        }
                    }
                    //Validate Code
                    if (string.IsNullOrEmpty(row["CODE"].ToString()))
                    {
                        SetGridViewRowError(view, "CODE", e, "Code cannot be empty.");
                    }
                    else
                    {
                        if (searchForDuplicates(gridViewTabControls, view.Columns["CODE"].Name,
                                                row["code"].ToString(), currentRowHandle))
                        {
                            SetGridViewRowError(view, "CODE", e, "Code already exists. Choose another code.");
                        }
                        else if (e.Valid)
                        {
                            SetDataSourceForRepositoryLookupTab(view.Columns["CODE"].Name,
                                                                row["CODE"].ToString(), currentRowHandle);
                        }
                    }
                    //Validate Order
                    if (string.IsNullOrEmpty(row["TAB_ORDER"].ToString()))
                    {
                        SetGridViewRowError(view, "TAB_ORDER", e, "Order cannot be empty.");
                    }
                    else
                    {
                        if (searchForDuplicateOrder(gridViewTabControls, view.Columns["TAB_ORDER"].Name,
                                                    Convert.ToInt32(row["TAB_ORDER"]), currentRowHandle))
                        {
                            SetGridViewRowError(view, "TAB_ORDER", e, "Order already exists. Choose another.");
                        }
                    }
                    break;
                case "gridViewFields":

                    //Validate Selected Tab
                    if (string.IsNullOrEmpty(row["SELECTEDTAB"].ToString()))
                    {
                        SetGridViewRowError(view, "SELECTEDTAB", e, "Please select a Tab Name.");
                    }
                    else { e.ErrorText = ""; }

                    //Validate Order
                    if (string.IsNullOrEmpty(row["FIELD_ORDER"].ToString()))
                    {
                        SetGridViewRowError(view, "FIELD_ORDER", e, "Please select a Order.");
                    }
                    else if (searchForDuplicateOrder(gridViewFields, view.Columns["FIELD_ORDER"].Name,
                             Convert.ToInt32(row["FIELD_ORDER"]), currentRowHandle))
                    {
                        SetGridViewRowError(view, "FIELD_ORDER", e, "Order already exists. Choose another Order.");
                    }
                    else { e.ErrorText = ""; }

                    //Validate Caption
                    if (string.IsNullOrEmpty(row["CAPTION"].ToString()))
                    {
                        SetGridViewRowError(view, "CAPTION", e, "Please select a Caption.");
                    }
                    else if (searchForDuplicates(gridViewFields, view.Columns["CAPTION"].Name,
                             row["CAPTION"].ToString(), currentRowHandle))
                    {
                        SetGridViewRowError(view, "CAPTION", e, "Caption already exists. Choose another caption.");
                    }
                    else { e.ErrorText = ""; }

                    //Validate FieldName
                    if (string.IsNullOrEmpty(row["FIELDNAME"].ToString()))
                    {
                        SetGridViewRowError(view, "FIELDNAME", e, "Please select a Field Name.");
                    }
                    else if (row["FIELDNAME"].ToString().Contains(" "))
                    {
                        SetGridViewRowError(view, "FIELDNAME", e, "Field Name cannot have blank space.");
                    }
                    else if (searchForDuplicates(gridViewFields, view.Columns["FIELDNAME"].Name,
                             row["FIELDNAME"].ToString(), currentRowHandle))
                    {
                        SetGridViewRowError(view, "FIELDNAME", e, "Field Name already exists. Choose another Field Name.");
                    }
                    else { e.ErrorText = ""; }

                    //Validate DataType
                    if (string.IsNullOrEmpty(row["DATATYPE"].ToString()))
                    {
                        SetGridViewRowError(view, "DATATYPE", e, "Please select a DataType.");
                    }
                    break;
                default:
                    break;
            }
        }

        private void gridViewTabControls_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            string formCode = string.Empty;
            if (gridViewTabControls.GetRowCellValue(e.FocusedRowHandle, "ID") != DBNull.Value)
            {
                if (Convert.ToInt32(gridViewTabControls.GetRowCellValue(e.FocusedRowHandle, "ID")) > 0)
                {
                    _TabCode = gridViewTabControls.GetFocusedRowCellValue("CODE").ToString();
                    formCode = gridViewTabControls.GetRowCellValue(e.FocusedRowHandle, "FORM_CODE").ToString();

                    this.gridFields.DataSource = this.dtFields;
                    this.gridViewFields.Columns["SELECTEDTAB"].FilterInfo =
                        new DevExpress.XtraGrid.Columns.ColumnFilterInfo("TAB_CODE = '" + _TabCode + "'");
                    //new DevExpress.XtraGrid.Columns.ColumnFilterInfo("SELECTEDTAB = '" + _TabCode + "'");
                }
                else
                {
                    //Tab is added on the fly.
                    if (_mode == FormMode.Add || _mode == FormMode.Edit)
                    {
                        if (gridViewTabControls.GetFocusedRowCellValue("CODE") != null)
                        {
                            _TabCode = gridViewTabControls.GetFocusedRowCellValue("CODE").ToString();

                            if (_mode == FormMode.Edit)
                                formCode = gridViewTabControls.GetRowCellValue(e.FocusedRowHandle, "FORM_CODE").ToString();
                            GetDataFromTempTable(_TabCode, formCode);
                            //gridViewFields.Columns["SELECTEDTAB"].FilterInfo = null;
                            gridViewFields.ClearColumnsFilter();
                        }
                        else
                        {
                            //GetDataFromTempTable("Gen1", gridViewTabControls.GetRowCellValue(e.FocusedRowHandle, "FORM_CODE").ToString());
                            GetDataFromTempTable("Gen1", textBoxCode.Text);
                            _TabCode = "Gen1";
                        }
                    }
                    else
                    {
                        this.dtFields.Rows.Clear();
                        if (gridViewTabControls.GetFocusedRowCellValue("CODE") != null)
                        {
                            _TabCode = gridViewTabControls.GetFocusedRowCellValue("CODE").ToString();
                            AddEmptyGridRow(gridViewFields.Name, _TabCode, 0, formCode);
                        }
                    }
                }
            }
        }

        private DataView GetFilteredData(ColumnView view)
        {
            if (view == null) return null;
            if (view.ActiveFilter == null || !view.ActiveFilterEnabled
                || view.ActiveFilter.Expression == "")
                return view.DataSource as DataView;

            DataTable table = ((DataView)view.DataSource).Table;
            DataView filteredDataView = new DataView(table);
            filteredDataView.RowFilter = view.ActiveFilter.Expression;
            return filteredDataView;
        }

        private void GetDataFromTempTable(string tabName, string formCode)
        {
            string rowFilter = "SELECTEDTAB='" + tabName + "'";
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataView dv = new DataView();

            dt = this.dtFields.Copy();
            dv = dt.DefaultView;
            dv.RowFilter = rowFilter;
            dt1 = dv.ToTable();
            if (dt1.Rows.Count > 0)
            {
                this.gridFields.DataSource = dt1;
            }
            else
            {
                AddEmptyGridRow("gridViewFields", tabName, 0, formCode);
            }
        }

        private void gridViewTabControls_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (_mode == FormMode.Edit)
            {
                int tabID = 0;
                GridView view = sender as GridView;
                tabID = Convert.ToInt32(gridViewTabControls.GetRowCellValue(view.FocusedRowHandle, "ID"));
                if (view.FocusedColumn.FieldName == "CODE" && tabID != 0)
                {
                    e.Cancel = true;
                }
            }
        }

        private void gridViewFields_ShowingEditor(object sender, CancelEventArgs e)
        {
            GridView view = (GridView)sender;
            if (_mode == FormMode.Edit)
            {
                int fieldID = 0;
                fieldID = Convert.ToInt32(gridViewFields.GetRowCellValue(view.FocusedRowHandle, "ID"));
                if ((view.FocusedColumn.FieldName == "FIELDNAME" || view.FocusedColumn.FieldName == "DATATYPE") && fieldID != 0)
                    e.Cancel = true;
            }
            ////Enable or Disable Decimal column based on the datatype selected.
            object cellValue = gridViewFields.GetRowCellValue(view.FocusedRowHandle, "DATATYPE");
            string columnName = view.FocusedColumn.FieldName;

            if (columnName != "DECIMAL")
            {
                if (columnName == "SIZE")
                {
                    switch (cellValue.ToString())
                    {
                        case "Bit":
                            e.Cancel = true;
                            break;
                        case "Datetime":
                            e.Cancel = true;
                            break;
                        case "Text":
                            e.Cancel = true;
                            break;
                        case "Varchar":
                            break;
                        case "Decimal":
                            break;
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                if (!cellValue.ToString().Equals("Decimal"))
                {
                    e.Cancel = true;
                }
            }
        }

        private void SetGridViewRowError(GridView view, string columnName,
                              DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e,
                              string errorMessage)
        {
            e.Valid = false;
            e.ErrorText = errorMessage;
            view.SetColumnError(view.Columns[columnName], errorMessage,
                                DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            view.FocusedColumn = view.Columns[columnName];
        }

        private bool searchForDuplicates(DevExpress.XtraGrid.Views.Grid.GridView oGrid,
                                         string colName, string colValue, int currentRowIndex)
        {
            bool bExists = false;
            for (int i = 0; i < oGrid.DataRowCount; i++)
            {
                if (i != currentRowIndex)
                {
                    if (colValue.ToString().ToUpper() == oGrid.GetRowCellValue(i, colName).ToString().ToUpper())
                        bExists = true;
                }
            }
            return bExists;
        }

        private bool searchForDuplicateOrder(DevExpress.XtraGrid.Views.Grid.GridView oGrid,
                                             string colName, int colValue, int currentRowIndex)
        {
            bool bExists = false;
            for (int i = 0; i < oGrid.DataRowCount; i++)
            {
                if (i != currentRowIndex)
                {
                    if (colValue == Convert.ToInt32(oGrid.GetRowCellValue(i, colName)))
                        bExists = true;
                }
                //object b = gridView1.GetRowCellValue(i, colName);
                //if (b != null && b.Equals(colValue))
                //{
                //    gridView1.FocusedRowHandle = i;
                //    bExists = true;
                //}
            }
            return bExists;
        }

        private bool CheckDuplicates(string strValue, string colData, int rowIndex)
        {
            bool bExists = false;

            int count = gridViewTabControls.DataRowCount;
            for (int i = count - 1; i >= 0; i--)
            {
                GridView childView = gridViewTabControls.GetDetailView(i, 0) as GridView;
                if (childView == null) continue;
                object v1 = gridViewTabControls.GetRowCellValue(i, "Column1");
                if (childView.DataRowCount < 1) continue;
                object vc = childView.GetRowCellValue(0, "Column1");
                //compare values and perform an action
            }
            return bExists;
        }

        private void gridViewFields_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            //Validate the row and Add it to dtFields
        }

        private void gridViewFields_FocusedColumnChanged(object sender, FocusedColumnChangedEventArgs e)
        {
            //Validate the column

        }

        #endregion

        #region MenuItem Events

        private void menuItemAddToolbar_Click(object sender, EventArgs e)
        {
            string rowNumber = Convert.ToString(this.dtTabs.Rows.Count + 1);

            if (_mode == FormMode.Edit)
            {
                AddEmptyGridRow(gridViewTabControls.Name, "Gen" + rowNumber, 0, textBoxFormID.Text);
            }
            else if (_mode == FormMode.Add)
            {
                AddEmptyGridRow(gridViewTabControls.Name, "Gen" + rowNumber, 0, textBoxFormID.Text);
            }
            SetDataSourceForRepositoryLookupTab("General" + rowNumber, "Gen" + rowNumber, Convert.ToInt32(rowNumber));
        }

        private void menuItemAddField_Click(object sender, EventArgs e)
        {
            string rowNumber = Convert.ToString(this.dtFields.Rows.Count + 1);
            string formCode = string.Empty;

            if (_mode == FormMode.Edit)
            {
                formCode = textBoxFormID.Text;
                AddEmptyGridRow("gridViewFields", _TabCode, GetSelectedTabID(), formCode);
            }
            else if (_mode == FormMode.Add)
                AddEmptyGridRow("gridViewFields", _TabCode, 0, formCode);
        }

        private void AddEmptyGridRow(string gridName, string tabCode, int tabID, string formCode)
        {
            string rowNumber;
            switch (gridName.ToUpper())
            {
                case "GRIDVIEWTABCONTROLS":
                    rowNumber = Convert.ToString(this.dtTabs.Rows.Count + 1);
                    this.dtTabs.Rows.Add(new object[] { "General" + rowNumber, tabCode, rowNumber, formCode, 0 });
                    this.gridTabControl.DataSource = this.dtTabs;
                    break;
                case "GRIDVIEWFIELDS":
                    rowNumber = Convert.ToString(this.dtFields.Rows.Count + 1);
                    DataRow drFields = this.dtFields.NewRow();
                    drFields["ID"] = 0;
                    drFields["SELECTEDTAB"] = tabCode;
                    drFields["FIELD_ORDER"] = rowNumber;
                    drFields["CAPTION"] = "";
                    drFields["TOOLTIP"] = "";
                    drFields["MANDATORY"] = 0;
                    drFields["FIELDNAME"] = "";
                    drFields["DATATYPE"] = "Varchar";
                    drFields["SIZE"] = 20;
                    drFields["DECIMAL"] = 0;
                    drFields["UNIQUE"] = 0;
                    drFields["INPUTMASK"] = "";
                    drFields["HELPQUERY"] = "";
                    drFields["REMARKS"] = "";
                    drFields["WHENCONDITION"] = "";
                    drFields["DEFAULTVALUE"] = "";
                    drFields["VALIDATION"] = "";
                    drFields["INTERNALUSE"] = 0;
                    drFields["FORM_CODE"] = formCode;
                    drFields["TAB_CODE"] = tabCode;
                    drFields["STATE"] = "Added";
                    this.dtFields.Rows.Add(drFields);
                    //do not remove the following line(VIJAYMESA)
                    this.gridFields.DataSource = this.dtFields;
                    break;
            }
        }

        private void menuItemRemoveToolbar_Click(object sender, EventArgs e)
        {
            if (gridViewTabControls.RowCount == 1)
            { MessageBox.Show("Atleast one tab should be existing"); return; }

            DialogResult confirmDelete = MessageBox.Show("Are you sure you want to delete selected tab & it's Fields?",
                                                         "Confirm Delete?", MessageBoxButtons.YesNo);
            if (confirmDelete == DialogResult.Yes)
            {
                //Code for Deleting selected Toolbar & it's Fields.
                int selectedID = Convert.ToInt32(gridViewTabControls.GetFocusedRowCellValue("ID"));
                if (selectedID > 0)
                {
                    AddDeletedTabEntry(gridViewTabControls.Name, selectedID, gridViewTabControls.FocusedRowHandle,
                                       textBoxTableName.Text, "");
                    dtTabs.Rows[gridViewTabControls.FocusedRowHandle].Delete();
                }
                else if (selectedID == 0)
                {
                    dtTabs.Rows[gridViewTabControls.FocusedRowHandle].Delete();
                }
            }
        }

        private void menuItemRemoveField_Click(object sender, EventArgs e)
        {
            if (gridViewFields.RowCount == 1)
            { MessageBox.Show("Atleast one field should be existing"); return; }

            DialogResult confirmDelete = MessageBox.Show("Are you sure you want to delete selected field.",
                                                         "Confirm Delete?", MessageBoxButtons.YesNo);
            if (confirmDelete == DialogResult.Yes)
            {
                //if (gridViewTabControls.GetRowCellValue(e.FocusedRowHandle, "ID") != DBNull.Value)
                int selectedID = Convert.ToInt32(gridViewFields.GetFocusedRowCellValue("ID"));

                if (selectedID > 0)
                {
                    //Remove the row from the DataTable.
                    //GetFilteredFieldRecords("FIELD_ORDER=" + gridViewFields.GetFocusedRowCellValue("FIELD_ORDER") +
                    //                        " AND TAB_ID=" + gridViewFields.GetFocusedRowCellValue("TAB_ID"));
                    AddDeletedTabEntry(gridViewFields.Name, selectedID, gridViewFields.FocusedRowHandle,
                        textBoxTableName.Text, gridViewFields.GetFocusedRowCellValue("FIELDNAME").ToString());
                    dtFields.Rows[gridViewFields.FocusedRowHandle].Delete();
                }
                else if (selectedID == 0)
                {
                    dtFields.Rows[gridViewFields.FocusedRowHandle].Delete();
                }
            }
        }

        #endregion

        private void SetDataSourceForRepositoryLookupTab(string colName, string colValue, int currentRowIndex)
        {
            BaseView tempView = gridTabControl.MainView;
            DataView dv = new DataView();
            DataTable dt = new DataTable();
            if (tempView != null)
            {
                dv = (DataView)tempView.DataSource;
                dt = dv.Table;
                repositoryItemTabLookUp.DataSource = dt;
                gridViewFields.Columns["SELECTEDTAB"].ColumnEdit = repositoryItemTabLookUp;
            }
        }

        private int GetSelectedTabID()
        {
            return Convert.ToInt32(gridViewTabControls.GetFocusedRowCellValue("ID"));
        }

        private void AddDeletedTabEntry(string gridName, int itemID, int rowIndex, string tableName, string fieldName)
        {
            switch (gridName)
            {
                case "gridViewTabControls":
                    DataRow drTab = dtRemovedTabs.NewRow();
                    drTab["ID"] = itemID;
                    drTab["ROW_INDEX"] = rowIndex;
                    this.dtRemovedTabs.Rows.Add(drTab);
                    break;
                case "gridViewFields":
                    DataRow drField = dtRemovedFields.NewRow();
                    drField["ID"] = itemID;
                    drField["ROW_INDEX"] = rowIndex;
                    drField["TABLE_NAME"] = tableName;
                    drField["FIELD_NAME"] = fieldName;
                    this.dtRemovedFields.Rows.Add(drField);
                    break;
            }
        }

        private void repositoryItemTabLookUp_EditValueChanged(object sender, EventArgs e)
        {
            //string selectedTab = (sender as DevExpress.XtraEditors.LookUpEdit).Text.ToString();
            //MessageBox.Show(selectedTab + " " + GetTabId(selectedTab));
            ////gridViewFields.SetRowCellValue(gridViewFields.GetFocusedDataSourceRowIndex(), "TAB_ID",
            ////                               GetTabId(selectedTab));
        }

        private int GetTabId(string tabName)
        {
            //DataView dv = new DataView();
            //dv = this.dtTabs.DefaultView;
            //dv.RowFilter = "CODE = '" + tabName + "'";
            //if (dv.Count > 0)
            //    return Convert.ToInt32(dv.Table.Rows[0]["ID"].ToString());
            //else
            //    return 0;
            int tabID = 0;
            for (int i = 0; i < dtTabs.Rows.Count; i++)
            {
                if (dtTabs.Rows[i]["CODE"].ToString() == tabName)
                    tabID = Convert.ToInt32(dtTabs.Rows[i]["ID"].ToString());
                else
                    tabID = 0;
            }
            return tabID;
        }

        private void repositoryItemDatatypeLookup_EditValueChanged(object sender, EventArgs e)
        {
            //If selected datatype is not Decimal, then set 0 in Decimal column.
            string text = (sender as DevExpress.XtraEditors.LookUpEdit).Text.ToUpper();
            switch (text)
            {
                case "VARCHAR":
                    //gridViewFields.Columns["Size"].OptionsColumn.ReadOnly = false;
                    gridViewFields.SetRowCellValue(gridViewFields.GetFocusedDataSourceRowIndex(), "SIZE", 20);
                    gridViewFields.SetRowCellValue(gridViewFields.GetFocusedDataSourceRowIndex(), "DECIMAL", 0);
                    break;
                case "DECIMAL":
                    //gridViewFields.Columns["Size"].OptionsColumn.ReadOnly = false;
                    break;
                case "BIT":
                    //gridViewFields.Columns["Size"].OptionsColumn.ReadOnly = true;
                    gridViewFields.SetRowCellValue(gridViewFields.GetFocusedDataSourceRowIndex(), "SIZE", 1);
                    gridViewFields.SetRowCellValue(gridViewFields.GetFocusedDataSourceRowIndex(), "DECIMAL", 0);
                    break;
                case "DATETIME":
                    //gridViewFields.Columns["Size"].OptionsColumn.ReadOnly = true;
                    gridViewFields.SetRowCellValue(gridViewFields.GetFocusedDataSourceRowIndex(), "SIZE", 7);
                    gridViewFields.SetRowCellValue(gridViewFields.GetFocusedDataSourceRowIndex(), "DECIMAL", 0);
                    break;
                case "TEXT":
                    //gridViewFields.Columns["Size"].OptionsColumn.ReadOnly = true;
                    gridViewFields.SetRowCellValue(gridViewFields.GetFocusedDataSourceRowIndex(), "SIZE", 8000);
                    gridViewFields.SetRowCellValue(gridViewFields.GetFocusedDataSourceRowIndex(), "DECIMAL", 0);
                    break;
                default:
                    MessageBox.Show("Invalid selection.");
                    break;
            }
            //if (!text.Equals("Decimal"))
            //{
            //    gridViewFields.SetRowCellValue(gridViewFields.GetFocusedDataSourceRowIndex(), "Decimal", 0);
            //}
            //else if (text.Equals("Bit") || text.Equals("DateTime") || text.Equals("Text"))
            //{
            //    gridViewFields.Columns["Size"].OptionsColumn.ReadOnly = true;
            //}
        }

        #region Save/Update/Delete

        private void btnNew_Click(object sender, EventArgs e)
        {
            _mode = FormMode.Add;
            ToggleButtons();

            this.dtTabs.Rows.Clear();
            this.dtFields.Rows.Clear();
            this.BindingContext[dsForms, "Forms"].AddNew();
            AddEmptyGridRow(gridViewTabControls.Name, "Gen1", 0, "");
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            _mode = FormMode.Edit;
            ToggleButtons();
            //gridFields.DataSource = this.dtFields;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (textBoxCaption.Text.Length == 0) { MessageBox.Show("Please enter caption", "Caption"); return; }

            if (textBoxCode.Text.Length == 0) { MessageBox.Show("Please enter code", "Code"); return; }

            if (textBoxTableName.Text.Length == 0) { MessageBox.Show("Please enter table name", "Table Name"); return; }

            int intMasterFormId = 0;
            int intTabControlId = 0;
            string strCaption = "";
            string strTabCode = "";
            int tabOrder = 0;
            int fieldId = 0;
            bool bTableCreated = false;
            StringBuilder queryString = new StringBuilder();

            try
            {
                oDataAccess.BeginTransaction();                
                if (_mode == FormMode.Add)
                {
                    //clsWaitDialog.CreateWaitDialog("Saving data", "Saving Form Details.");
                    intMasterFormId = oForm.SaveMasterFormDetails(textBoxCaption.Text, textBoxCode.Text,
                                                                      textBoxTableName.Text, oDataAccess);
                    if (intMasterFormId == 0)
                    {
                        _ErrorMessage = "Error saving Form Details";
                        ShowErrorMessage(oDataAccess, _ErrorMessage);
                        return;
                    }
                    if (dtTabs == null)
                    {
                        ShowErrorMessage(oDataAccess, _ErrorMessage);
                        return;
                    }

                    //clsWaitDialog.CreateWaitDialog("Saving data", "Checking Table Existence.");
                    bTableCreated = oForm.CheckTableExists(textBoxTableName.Text, oDataAccess);
                    if (bTableCreated)
                    {
                        _ErrorMessage = "Table Name already exists.\n" +
                                        "Please choose a different name";
                        ShowErrorMessage(oDataAccess, _ErrorMessage);
                        return;
                    }
                    //clsWaitDialog.CreateWaitDialog("Saving data", "Creating Table.");
                    queryString.Append("CREATE TABLE " + textBoxTableName.Text + " ( ");
                    queryString.Append("ID INT IDENTITY, ");
                    for (int i = 0; i < dtTabs.Rows.Count; i++)
                    {
                        strCaption = dtTabs.Rows[i]["CAPTION"].ToString();
                        strTabCode = dtTabs.Rows[i]["CODE"].ToString();
                        tabOrder = Convert.ToInt32(dtTabs.Rows[i]["TAB_ORDER"].ToString());

                        //clsWaitDialog.CreateWaitDialog("Saving data", "Saving Tab information.");
                        intTabControlId = oForm.SaveTabControlDetails(textBoxCode.Text, strCaption, strTabCode, tabOrder, oDataAccess);

                        if (intTabControlId == 0)
                        {
                            _ErrorMessage = "Error saving Tab Details.\nTab Code: " + strTabCode;
                            ShowErrorMessage(oDataAccess, _ErrorMessage);
                            return;
                        }

                        //Get Fields added under each tab and save them.
                        DataTable tabFields = new DataTable();
                        tabFields = GetFilteredFieldRecords("SELECTEDTAB='" + strTabCode + "'");

                        //clsWaitDialog.CreateWaitDialog("Saving data", "Saving Field Info.");
                        fieldId = SaveFieldInfo(oForm, textBoxCode.Text, strTabCode, oDataAccess, tabFields, queryString, bTableCreated);
                        if (fieldId == 0)
                        {
                            _ErrorMessage = "Error saving Field Details.\nTab Code: " + strTabCode;
                            ShowErrorMessage(oDataAccess, _ErrorMessage);
                            return;
                        }
                    }
                    queryString = queryString.Remove(queryString.Length - 2, 1);
                    queryString.Append(")");
                    //Create Table in the Database

                    //clsWaitDialog.CreateWaitDialog("Saving complete", "Finalizing saving.");
                    BindData1(string.Empty);
                    SetBindingContextPosition("Last", 0);
                    oDataAccess.ExecuteSQLStatement(queryString.ToString(), null, 20, true);
                    //clsWaitDialog.CloseWaitDialog();
                }
                else if (_mode == FormMode.Edit)
                {
                    int formID = 0;
                    int tabID = 0;
                    if (textBoxFormID.Text != string.Empty)
                        formID = GetFormID(textBoxFormID.Text);

                    //Get Form ID

                    if (txtCaption.Text != textBoxCaption.Text)
                    {
                        //Update Form details(Only if the caption is changed)
                        //clsWaitDialog.CreateWaitDialog("Saving...", "Updating Form Info.");
                        formID = oForm.UpdateMasterFormDetails(formID, textBoxCaption.Text,
                                                               textBoxCode.Text, textBoxTableName.Text, oDataAccess);
                    }
                    //Save or Update Tab details
                    if (formID != 0)
                    {
                        gridViewTabControls.PostEditor();
                        gridViewTabControls.UpdateCurrentRow();
                        DataTable changedTabRows = new DataTable();
                        changedTabRows = this.dtTabs.GetChanges();

                        if (changedTabRows != null)
                        {
                            //clsWaitDialog.CreateWaitDialog("Saving...", "Updating Tab Info.");
                            tabID = UpdateTabControlDetails(changedTabRows, queryString);
                            if (tabID == 0)
                            {
                                ShowErrorMessage(oDataAccess, _ErrorMessage);
                                return;
                            }
                        }

                        if (this.dtRemovedTabs.Rows.Count > 0)
                        {
                            //Rows r deleted from dtTabs. So remove them from DB also.
                            int deletedTabId = 0;
                            for (int i = 0; i < dtRemovedTabs.Rows.Count; i++)
                            {
                                //clsWaitDialog.CreateWaitDialog("Saving...", "Deleting fields.");
                                oForm.DeleteFields(Convert.ToInt32(dtRemovedTabs.Rows[i]["ID"]), oDataAccess);
                                deletedTabId = oForm.DeleteSelectedTab(Convert.ToInt32(dtRemovedTabs.Rows[i]["ID"]),
                                                                       oDataAccess);
                                if (deletedTabId == 0)
                                {
                                    ShowErrorMessage(oDataAccess, _ErrorMessage);
                                    return;
                                }
                            }
                        }
                        
                        gridViewFields.PostEditor();
                        gridViewFields.UpdateCurrentRow();
                        DataTable changedFieldRows = new DataTable();
                        changedFieldRows = this.dtFields.GetChanges();

                        if (changedFieldRows != null)
                        {
                            bTableCreated = true;
                            //clsWaitDialog.CreateWaitDialog("Saving...", "Saving Field Info.");
                            fieldId = SaveFieldInfo(oForm, textBoxCode.Text, strTabCode, oDataAccess, changedFieldRows, queryString, bTableCreated);
                            if (fieldId == 0)
                            {
                                //_ErrorMessage = "Error saving Field Details.\nTab Code: " + strCode;
                                ShowErrorMessage(oDataAccess, _ErrorMessage);
                                return;
                            }
                        }
                        if (this.dtRemovedFields.Rows.Count > 0)
                        {
                            int deletedFieldId = 0;
                            for (int j = 0; j < dtRemovedFields.Rows.Count; j++)
                            {
                                StringBuilder strDeleteSQL = new StringBuilder(); ;
                                strDeleteSQL.Append("alter table ");
                                strDeleteSQL.Append(dtRemovedFields.Rows[j]["TABLE_NAME"]);
                                strDeleteSQL.Append(" drop column ");
                                strDeleteSQL.Append(dtRemovedFields.Rows[j]["FIELD_NAME"]);
                                deletedFieldId = oDataAccess.ExecuteSQLStatement(strDeleteSQL.ToString(), null, 20, true);
                                deletedFieldId = oForm.DeleteSelectedField(Convert.ToInt32(dtRemovedFields.Rows[j]["ID"]),
                                                                           oDataAccess);
                            }
                        }
                        this.dtFields.AcceptChanges();
                    }
                    else
                    {
                        ShowErrorMessage(oDataAccess, _ErrorMessage);
                        return;
                    }
                    BindLastRecord(textBoxFormID.Text);
                }
                oDataAccess.CommitTransaction();
                _mode = FormMode.View;
                ToggleButtons();
                MessageBox.Show("Details Saved Successfully.", "Success");
            }
            catch (Exception ex)
            {
                //clsWaitDialog.CloseWaitDialog();
                ShowErrorMessage(oDataAccess, ex.Message + Environment.NewLine + ex.StackTrace);
            }
            finally
            {
                dtRemovedTabs.Rows.Clear();
                dtRemovedFields.Rows.Clear();
                //clsWaitDialog.CloseWaitDialog();
            }
        }

        private int GetFormID(string formCode)
        {
            DataView dv;
            int formId = 0;

            dv = dsForms.Tables[0].DefaultView;
            dv.RowFilter = "code = '" + formCode + "'";
            if (dv.Count > 0)
            {
                formId = Convert.ToInt32(dv[0]["id"].ToString());
            }
            return formId;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.dtRemovedTabs.Rows.Clear();
            this.dtRemovedFields.Rows.Clear();
            this.BindingContext[dsForms, "Forms"].CancelCurrentEdit();
            this.BindingContext[dsForms, "Forms"].EndCurrentEdit();
            if (_mode == FormMode.Edit)
            {
                if (textBoxFormID.Text != "")
                    BindLastRecord(textBoxFormID.Text);
            }
            else if (_mode == FormMode.Add)
            {
                if (this.dsForms.Tables[0].Rows.Count > 0)
                {
                    string formCode = this.dsForms.Tables[0].Rows[this.dsForms.Tables[0].Rows.Count - 1]["code"].ToString();
                    //BindLastRecord(GetRowIndex(formID));
                    BindLastRecord(formCode);
                    this.BindingContext[dsForms, "Forms"].Position = this.BindingContext[dsForms, "Forms"].Count;
                }
                else BindData1(string.Empty);
            }
            _mode = FormMode.View;
            ToggleButtons();
        }

        private int UpdateTabControlDetails(DataTable changedRows, StringBuilder queryString)
        {
            int tabID = 0;
            int actualTabID = 0;
            string strFormCode = string.Empty;
            //string strCaption = "";
            string strCode = "";
            //int tabOrder = 0;

            for (int i = 0; i < changedRows.Rows.Count; i++)
            {
                //DataTable dtTemp = new DataTable();
                //dtTemp = GetFilteredFieldRecords("SELECTEDTAB='" + changedRows.Rows[i]["CODE"].ToString() + "'");
                //if (dtTemp.Rows.Count <= 0)
                //{
                //    _ErrorMessage = "Tab " + changedRows.Rows[i]["CODE"].ToString() +
                //                    " should have atleast one field added under it.";
                //    return 0;
                //}

                switch (changedRows.Rows[i].RowState)
                {
                    case DataRowState.Added:
                        strFormCode = changedRows.Rows[i]["FORM_ID"].ToString();
                        strCode = changedRows.Rows[i]["CODE"].ToString();
                        tabID = oForm.SaveTabControlDetails(changedRows.Rows[i]["FORM_CODE"].ToString(),
                                                            changedRows.Rows[i]["CAPTION"].ToString(),
                                                            changedRows.Rows[i]["CODE"].ToString(),
                                                            Convert.ToInt32(changedRows.Rows[i]["TAB_ORDER"]),
                                                            oDataAccess);
                        actualTabID = tabID;
                        break;
                    case DataRowState.Modified:
                        strFormCode = changedRows.Rows[i]["FORM_CODE"].ToString();
                        strCode = changedRows.Rows[i]["CODE"].ToString();
                        tabID = oForm.UpdateTabControlDetails(Convert.ToInt32(changedRows.Rows[i]["ID"]),
                                                              changedRows.Rows[i]["FORM_CODE"].ToString(),
                                                              changedRows.Rows[i]["CAPTION"].ToString(),
                                                              Convert.ToInt32(changedRows.Rows[i]["TAB_ORDER"]),
                                                              oDataAccess);
                        break;
                    case DataRowState.Deleted:
                        tabID = 1;
                        //DataTable dtFieldList = new DataTable();
                        //dtFieldList = GetFilteredFieldRecords("TAB_ID=" + dictDeletedTabs[i]);
                        //tabID = oForm.DeleteFields(dictDeletedTabs[i], oDataAccess);
                        //tabID = oForm.DeleteSelectedTab(dictDeletedTabs[i], oDataAccess);
                        break;
                }

                gridViewFields.PostEditor();
                gridViewFields.UpdateCurrentRow();

                ////Need to remove the below code for Updating Field Info. It's handled above.
                //DataTable changedFieldRows = new DataTable();
                //changedFieldRows = this.dtFields.GetChanges();
                ////Get Changes Specific to the Tab and apply changes.
                //if (changedFieldRows != null)
                //{
                //    if (changedFieldRows.Rows.Count > 0)
                //    {
                //        fieldID = SaveFieldInfo(oForm, strCode, intMasterFormId, actualTabID, oDataAccess, 
                //                                changedFieldRows, queryString);
                //        if (fieldID == 0)
                //            return 0;
                //    }
                //}
            }
            return tabID;
        }

        private DataTable GetFilteredFieldRecords(string filterString)
        {
            DataTable dtTemp = new DataTable();
            dtTemp = dtFields.Copy();
            DataView dv = dtTemp.DefaultView;
            //dv.RowFilter = "SELECTEDTAB='" + tabCode + "'";
            dv.RowFilter = filterString;
            dtTemp = dv.ToTable();

            return dtTemp;
        }

        private int SaveFieldInfo(clsDynamicForm oDynamicForm, string strFormCode, string strTabCode,
                                  DataAccess_Net.clsDataAccess oDataAccess, DataTable dtTemp,
                                  StringBuilder queryString, bool bTableCreated)
        {
            string selectedTab = string.Empty;
            int order = 0;
            string caption = string.Empty;
            string tooltip = string.Empty;
            int mandatory = 0;
            string isMandatory = string.Empty;
            string fieldName = string.Empty;
            string datatype = string.Empty;
            int size = 0;
            int decimalVal = 0;
            int uniqueVal = 0;
            string inputMask = string.Empty;
            string helpQuery = string.Empty;
            string remarks = string.Empty;
            string whenCondition = string.Empty;
            string defaultValue = string.Empty;
            string validation = string.Empty;
            int internalUse = 0;
            int fieldId = 0;

            try
            {
                if (dtTemp.Rows.Count > 0)
                {
                    for (int i = 0; i < dtTemp.Rows.Count; i++)
                    {
                        switch (dtTemp.Rows[i].RowState)
                        {
                            case DataRowState.Added:
                                selectedTab = dtTemp.Rows[i]["SELECTEDTAB"].ToString();
                                order = Convert.ToInt16(dtTemp.Rows[i]["FIELD_ORDER"]);
                                caption = dtTemp.Rows[i]["CAPTION"].ToString();
                                tooltip = dtTemp.Rows[i]["TOOLTIP"].ToString();
                                mandatory = Convert.ToInt32(dtTemp.Rows[i]["MANDATORY"] == DBNull.Value ? 0 : dtTemp.Rows[i]["Mandatory"]);
                                fieldName = dtTemp.Rows[i]["FIELDNAME"].ToString();
                                datatype = dtTemp.Rows[i]["DATATYPE"].ToString();
                                size = Convert.ToInt32(dtTemp.Rows[i]["SIZE"] == DBNull.Value ? 0 : dtTemp.Rows[i]["Size"]);
                                decimalVal = Convert.ToInt32(dtTemp.Rows[i]["DECIMAL"] == DBNull.Value ? 0 : dtTemp.Rows[i]["Decimal"]);
                                uniqueVal = Convert.ToInt32(dtTemp.Rows[i]["UNIQUE"] == DBNull.Value ? 0 : dtTemp.Rows[i]["Unique"]);
                                inputMask = dtTemp.Rows[i]["INPUTMASK"].ToString();
                                helpQuery = dtTemp.Rows[i]["HELPQUERY"].ToString();
                                remarks = dtTemp.Rows[i]["REMARKS"].ToString();
                                whenCondition = dtTemp.Rows[i]["WHENCONDITION"].ToString();
                                defaultValue = dtTemp.Rows[i]["DEFAULTVALUE"].ToString();
                                validation = dtTemp.Rows[i]["VALIDATION"].ToString();
                                internalUse = Convert.ToInt16(dtTemp.Rows[i]["INTERNALUSE"] == DBNull.Value ? 0 : dtTemp.Rows[i]["InternalUse"]);
                                fieldId = Convert.ToInt16(dtTemp.Rows[i]["ID"] == DBNull.Value ? 0 : dtTemp.Rows[i]["ID"]);
                                isMandatory = (mandatory == 0 ? "NULL" : "NOT NULL");
                                if (dtTemp.Rows[i]["TAB_CODE"].ToString() != "")
                                    strTabCode = dtTemp.Rows[i]["TAB_CODE"].ToString();

                                //10/2/2011
                                if (bTableCreated)
                                {
                                    if (AddColumnToTable(dtTemp.Rows[i], "Add") == 0)
                                    {
                                        _ErrorMessage = "Error while adding field.";
                                        ShowErrorMessage(oDataAccess, _ErrorMessage);
                                        return 0;
                                    }
                                }

                                switch (datatype.ToLower())
                                {
                                    case "varchar":
                                        queryString.Append(fieldName + " " + datatype + " (" + size + ") " + isMandatory + ", ");
                                        break;
                                    case "decimal":
                                        queryString.Append(fieldName + " " + datatype + " (" + size + ", " + decimalVal + ") " + isMandatory + ", ");
                                        break;
                                    case "bit":
                                    case "datetime":
                                    case "text":
                                        queryString.Append(fieldName + " " + datatype + " " + isMandatory + ", ");
                                        break;
                                }
                                fieldId = oDynamicForm.SaveFieldDetails(strTabCode, order, caption, tooltip, mandatory, fieldName, datatype,
                                        size, decimalVal, uniqueVal, inputMask, helpQuery, remarks, whenCondition, defaultValue,
                                        validation, internalUse, strFormCode, oDataAccess);
                                break;
                            case DataRowState.Modified:
                                selectedTab = dtTemp.Rows[i]["SELECTEDTAB"].ToString();
                                order = Convert.ToInt16(dtTemp.Rows[i]["FIELD_ORDER"]);
                                caption = dtTemp.Rows[i]["CAPTION"].ToString();
                                tooltip = dtTemp.Rows[i]["TOOLTIP"].ToString();
                                mandatory = Convert.ToInt32(dtTemp.Rows[i]["MANDATORY"] == DBNull.Value ? 0 : dtTemp.Rows[i]["Mandatory"]);
                                fieldName = dtTemp.Rows[i]["FIELDNAME"].ToString();
                                datatype = dtTemp.Rows[i]["DATATYPE"].ToString();
                                size = Convert.ToInt32(dtTemp.Rows[i]["SIZE"] == DBNull.Value ? 0 : dtTemp.Rows[i]["Size"]);
                                decimalVal = Convert.ToInt32(dtTemp.Rows[i]["DECIMAL"] == DBNull.Value ? 0 : dtTemp.Rows[i]["Decimal"]);
                                uniqueVal = Convert.ToInt32(dtTemp.Rows[i]["UNIQUE"] == DBNull.Value ? 0 : dtTemp.Rows[i]["Unique"]);
                                inputMask = dtTemp.Rows[i]["INPUTMASK"].ToString();
                                helpQuery = dtTemp.Rows[i]["HELPQUERY"].ToString();
                                remarks = dtTemp.Rows[i]["REMARKS"].ToString();
                                whenCondition = dtTemp.Rows[i]["WHENCONDITION"].ToString();
                                defaultValue = dtTemp.Rows[i]["DEFAULTVALUE"].ToString();
                                validation = dtTemp.Rows[i]["VALIDATION"].ToString();
                                internalUse = Convert.ToInt32(dtTemp.Rows[i]["INTERNALUSE"] == DBNull.Value ? 0 : dtTemp.Rows[i]["InternalUse"]);
                                fieldId = Convert.ToInt16(dtTemp.Rows[i]["ID"] == DBNull.Value ? 0 : dtTemp.Rows[i]["ID"]);
                                isMandatory = (mandatory == 0 ? "NULL" : "NOT NULL");
                                strTabCode = dtTemp.Rows[i]["TAB_CODE"].ToString();
                                //MessageBox.Show("alter table " + textBoxTableName.Text +
                                //    " alter " + fieldName + "(" + size + ");");

                                //10/2/2011
                                //Update only if the Field's metadata is modified
                                //Compare with the actual datatable and apply changes.
                                if (bTableCreated && IsFieldInfoModified(fieldName, dtTemp.Rows[i]))
                                {
                                    if (AddColumnToTable(dtTemp.Rows[i], "Modify") == 0)
                                    {
                                        _ErrorMessage = "Error while adding field.";
                                        ShowErrorMessage(oDataAccess, _ErrorMessage);
                                        return 0;
                                    }
                                }

                                switch (datatype.ToLower())
                                {
                                    case "varchar":
                                        queryString.Append(fieldName + " " + datatype + " (" + size + ") " + isMandatory + ", ");
                                        break;
                                    case "decimal":
                                        queryString.Append(fieldName + " " + datatype + " (" + size + ", " + decimalVal + ") " + isMandatory + ", ");
                                        break;
                                    case "bit":
                                    case "datetime":
                                    case "text":
                                        queryString.Append(fieldName + " " + datatype + " " + isMandatory + ", ");
                                        break;
                                }
                                fieldId = oDynamicForm.UpdateFieldDetails(strTabCode, order, caption, tooltip, mandatory, fieldName, datatype,
                                        size, decimalVal, uniqueVal, inputMask, helpQuery, remarks, whenCondition, defaultValue,
                                        validation, internalUse, strFormCode, fieldId, oDataAccess);
                                break;
                            case DataRowState.Deleted:
                                fieldId = 1;
                                //fieldId = oDynamicForm.DeleteSelectedField(dictDeletedFields[i], oDataAccess);
                                break;
                        }
                    }
                }
                else
                {
                    _ErrorMessage = "Atleast one field should be added under tab: " + strTabCode;
                    return 0;
                }
            }
            catch (Exception ex)
            {
                _ErrorMessage = ex.Message;
                return 0;
            }
            return fieldId;
        }

        private bool IsFieldInfoModified(string fieldName, DataRow changedRow)
        {
            bool IsModified = false;
            DataView dv = new DataView();

            dv = dtFieldsOriginal.DefaultView;
            dv.RowFilter = "FIELDNAME = '" + fieldName + "'";
            if (dv.Count > 0)
            {
                if (changedRow["MANDATORY"] != dv[0]["MANDATORY"])
                { IsModified = true; }
                if (changedRow["SIZE"] != dv[0]["SIZE"])
                { IsModified = true; }
                if (changedRow["DECIMAL"] != dv[0]["DECIMAL"])
                { IsModified = true; }
            }

            return IsModified;
        }

        private int AddColumnToTable(DataRow columnInfo, string changeType)
        {
            int result = 0;
            StringBuilder strSQL = new StringBuilder();
            string isMandatory = (Convert.ToInt32(columnInfo["MANDATORY"]) == 0 ? "NULL" : "NOT NULL");
            strSQL.Append("alter table ");
            strSQL.Append(textBoxTableName.Text);
            if (changeType == "Add")
                strSQL.Append(" add ");
            else if (changeType == "Modify")
                strSQL.Append(" alter column ");

            switch (columnInfo["DATATYPE"].ToString().ToLower())
            {
                case "varchar":
                    strSQL.Append(columnInfo["FIELDNAME"] + " " + columnInfo["DATATYPE"] + " (" + columnInfo["SIZE"] + ") " + isMandatory + ";");
                    break;
                case "decimal":
                    strSQL.Append(columnInfo["FIELDNAME"] + " " + columnInfo["DATATYPE"] + " (" + columnInfo["SIZE"] + ", " + columnInfo["DECIMAL"] + ") " + isMandatory + ";");
                    break;
                case "bit":
                case "datetime":
                case "text":
                    strSQL.Append(columnInfo["FIELDNAME"] + " " + columnInfo["DATATYPE"] + " " + isMandatory + ";");
                    break;
            }

            result = oDataAccess.ExecuteSQLStatement(strSQL.ToString(), null, 20, true);
            return result;
        }

        private void ShowErrorMessage(DataAccess_Net.clsDataAccess oDataAccess, string ErrorMessage)
        {
            if (oDataAccess.InTransaction)
            {
                oDataAccess.RollbackTransaction();
            }
            MessageBox.Show(ErrorMessage);
        }

        private void buttonLookup_Click(object sender, EventArgs e)
        {
            string selectstring = "select * from [FORM_MASTER]";
            DataSet dsData = new DataSet();
            DataView dvw = new DataView();
            dsData = oDataAccess.GetDataSet(selectstring, null, 20);
            dvw = dsData.Tables[0].DefaultView;

            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = "Select Master Name";
            oSelectPop.psearchcol = "CAPTION";
            string vFieldList = "CAPTION:Caption,CODE:Code";
            oSelectPop.pDisplayColumnList = vFieldList;
            oSelectPop.pRetcolList = "CAPTION,CODE,TABLE_NAME";
            //oSelectPop.pSetCtlRef = this.textBoxCaption;
            oSelectPop.ShowDialog();

            if (oSelectPop.pReturnArray != null)
            {
                //this.textBoxCaption.Text = oSelectPop.pReturnArray[0];
                //this.textBoxCode.Text = oSelectPop.pReturnArray[1];
                //this.textBoxCode.Text = oSelectPop.pReturnArray[2];
                SetBindingContextPosition("SelectedRecord", GetSelectedRowIndex(oSelectPop.pReturnArray[1]));
                BindTabControls(textBoxFormID.Text, oForm);
            }
            else
            {
                MessageBox.Show("No Recored Selected.");
            }
        }

        #endregion

        private int GetRowIndex(int FormId)
        {
            int id = 0;
            for (int i = 0; i < this.dsForms.Tables[0].Rows.Count; i++)
            {
                if (FormId == Convert.ToInt32(dsForms.Tables[0].Rows[i]["id"]))
                {
                    id = i;
                }
            }
            return id;
        }

        private void textBoxCode_Leave(object sender, EventArgs e)
        {
            if (textBoxCode.Text.Length != 0)
            {
                textBoxTableName.Text = textBoxCode.Text.ToUpper() + "_MAST";
            }
            else
            {
                textBoxTableName.Text = "";
                //gridTabControl.Enabled = false;
                //gridFields.Enabled = false;
            }
            if (string.IsNullOrEmpty(textBoxTableName.Text) == false)
            {
                //gridTabControl.Enabled = true;
                //gridFields.Enabled = true;
            }
        }

        //private void buttonLookup_Click(object sender, EventArgs e)
        //{
        //    DataSet dsFormList = new DataSet();
        //    DataSet dsTabList = new DataSet();
        //    DataSet dsFieldList = new DataSet();

        //    try
        //    {
        //        frmSearch frm = new frmSearch(textBoxCode.Text);
        //        frm.ShowDialog();
        //        //textBoxCode.Text = frm._formCode;
        //        BindData(frm._formCode);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //    finally
        //    {
        //        dsFormList = null;
        //        dsTabList = null;
        //        dsFieldList = null;
        //    }
        //}

        private int GetSelectedRowIndex(string formCode)
        {
            string popupValue = "";
            for (int i = 0; i < dsForms.Tables[0].Rows.Count; i++)
            {
                popupValue = dsForms.Tables[0].Rows[i]["CODE"].ToString();
                if (popupValue == formCode)
                    return i;
            }
            return 0;
        }

        #region Data Bindings

        private void BindData(string formCode)
        {
            DataSet dsFormData = new DataSet();
            DataSet dsTabData = new DataSet();
            try
            {
                dsFormData = oForm.GetFormList(formCode, oDataAccess);
                dsForms = dsFormData;

                if (dsFormData.Tables[0].Rows.Count == 0)
                {
                    btnEdit.Visible = false;
                    return;
                }
                else btnEdit.Visible = true;

                textBoxCaption.DataBindings.Clear();
                textBoxCode.DataBindings.Clear();
                textBoxTableName.DataBindings.Clear();
                textBoxFormID.DataBindings.Clear();

                textBoxFormID.DataBindings.Add("Text", dsFormData, "Forms.ID");
                textBoxCaption.DataBindings.Add("Text", dsFormData, "Forms.Caption");
                textBoxCode.DataBindings.Add("Text", dsFormData, "Forms.Code");
                textBoxTableName.DataBindings.Add("Text", dsFormData, "Forms.Table_Name");

                BindTabControls(dsFormData.Tables[0].Rows[0]["code"].ToString(), oForm);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BindData1(string formCode)
        {
            DataSet dsFormData = new DataSet();
            DataSet dsTabData = new DataSet();
            try
            {
                dsFormData = oForm.GetFormList(formCode, oDataAccess);
                dsForms = dsFormData;

                if (dsFormData.Tables[0].Rows.Count == 0)
                {
                    btnNew.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnEdit.Enabled = false;
                    gridTabControl.DataSource = this.dtTabs;
                    gridFields.DataSource = this.dtFields;
                    gridViewFields.Columns["DATATYPE"].ColumnEdit = repositoryItemDatatypeLookup;
                    ToggleNavigationButtons(false, false, false, false, false, false);

                    textBoxCaption.ReadOnly = true;
                    txtCaption.ReadOnly = true;
                    textBoxCode.ReadOnly = true;
                    textBoxTableName.ReadOnly = true; /*Rup 07/03/2011*/
                    textBoxCaption.Text = string.Empty;
                    txtCaption.Text = string.Empty;
                    textBoxCode.Text = string.Empty;
                    textBoxTableName.Text = string.Empty;
                    textBoxFormID.Text = string.Empty;

                    contextMenuStrip1.Enabled = false;
                    contextMenuStrip2.Enabled = false;

                    SetGridColumnWidths();
                    SetGridColumnCaptions(gridViewTabControls.Name);
                    SetGridColumnCaptions(gridViewFields.Name);
                    HideGridViewColumns(gridViewTabControls.Name);
                    HideGridViewColumns(gridViewFields.Name);
                    return;
                }
                else
                {
                    ToggleButtons();
                    SetBindingContextPosition("Last", 0);
                    btnEdit.Visible = true;
                }

                textBoxCaption.DataBindings.Clear();
                txtCaption.DataBindings.Clear();
                textBoxCode.DataBindings.Clear();
                textBoxTableName.DataBindings.Clear();
                textBoxFormID.DataBindings.Clear();
                this.BindingContext[dsForms, "Forms"].Position = this.BindingContext[dsForms, "Forms"].Count;

                textBoxFormID.DataBindings.Add("Text", dsFormData, "Forms.CODE");
                textBoxCaption.DataBindings.Add("Text", dsFormData, "Forms.Caption");
                txtCaption.DataBindings.Add("Text", dsFormData, "Forms.Caption");
                //txtCaption.Text = textBoxCaption.Text;
                textBoxCode.DataBindings.Add("Text", dsFormData, "Forms.Code");
                textBoxTableName.DataBindings.Add("Text", dsFormData, "Forms.Table_Name");

                BindTabControls(textBoxFormID.Text, oForm);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BindTabControls(string formCode, clsDynamicForm oForm)
        {
            DataSet dsTabData = new DataSet();

            dsTabData = oForm.GetTabControlList(formCode, oDataAccess);
            gridTabControl.DataSource = dsTabData.Tables[0];
            HideGridViewColumns(gridViewTabControls.Name);

            if (dsTabData.Tables[0].Rows.Count > 0)
            {
                this.dtTabs = dsTabData.Tables[0];
                _TabCode = gridViewTabControls.GetFocusedRowCellValue("CODE").ToString();
                BindFields(Convert.ToInt32(dsTabData.Tables[0].Rows[0]["id"]), formCode, oForm, oDataAccess);
                this.repositoryItemTabLookUp.DataSource = dsTabData.Tables[0];
                gridViewFields.Columns["SELECTEDTAB"].ColumnEdit = repositoryItemTabLookUp;
                gridViewFields.RefreshData();
                SetGridColumnWidths();
                SetGridColumnCaptions(gridViewTabControls.Name);
                SetGridColumnCaptions(gridViewFields.Name);
            }
        }

        private void BindFields(int tabID, string formCode, clsDynamicForm oForm, DataAccess_Net.clsDataAccess oDataAccess)
        {
            DataSet dsFieldData = new DataSet();

            if (formCode != "")
                dsFieldData = oForm.GetAllFields(formCode, oDataAccess);
            else
                MessageBox.Show("Not Implemented");
            //dsFieldData = oForm.GetFieldList(tabID, oDataAccess);

            gridFields.DataSource = dsFieldData.Tables[0];
            gridViewFields.Columns["DATATYPE"].ColumnEdit = repositoryItemDatatypeLookup;
            HideGridViewColumns(gridViewFields.Name);
            this.dtFields = dsFieldData.Tables[0];
            this.dtFieldsOriginal = dsFieldData.Tables[0];
        }

        private void BindLastRecord(string formCode)
        {
            DataSet dsTabData = new DataSet();
            try
            {
                dsTabData = oForm.GetTabControlList(formCode, oDataAccess);
                gridTabControl.DataSource = dsTabData.Tables[0];
                this.dtTabs = dsTabData.Tables[0];

                if (dsTabData.Tables[0].Rows.Count > 0)
                {
                    BindFields(Convert.ToInt32(dsTabData.Tables[0].Rows[0]["id"]), formCode, oForm, oDataAccess);
                    this.repositoryItemTabLookUp.DataSource = dsTabData.Tables[0];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        private void textBoxCaption_Validated(object sender, EventArgs e)
        {
            errorProvider1.SetError(textBoxCaption, "");
        }

        #region Preview Form

        private void buttonPreviewForm_Click(object sender, EventArgs e)
        {
            //PreviewDynamicMasterForm();
            //"Test", "Udyog12\\SQLEXPRESS", "sa", "sa1985", "", "195", ""
            string[] strParams = new string[] { "1", DataAccess_Net.clsDataAccess._databaseName, 
                                                DataAccess_Net.clsDataAccess._serverName, 
                                                DataAccess_Net.clsDataAccess._userID, 
                                                DataAccess_Net.clsDataAccess._password, 
                                                "12005", textBoxCode.Text, _UserName };
            DynamicMaster.MasterForm oMaster = new DynamicMaster.MasterForm(strParams);
            oMaster.StartPosition = FormStartPosition.CenterScreen;
            oMaster.Show();
        }

        #endregion

        #region Navigation Buttons

        private void btnFirst_Click(object sender, EventArgs e)
        {
            SetBindingContextPosition("First", 0);
            BindLastRecord(textBoxFormID.Text);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            SetBindingContextPosition("Previous", 0);
            BindLastRecord(textBoxFormID.Text);
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            SetBindingContextPosition("Next", 0);
            BindLastRecord(textBoxFormID.Text);
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            SetBindingContextPosition("Last", 0);
            BindLastRecord(textBoxFormID.Text);
        }

        private void ToggleButtons()
        {
            switch (_mode)
            {
                case FormMode.Add:
                    textBoxCaption.Focus();
                    textBoxCaption.ReadOnly = false;
                    textBoxCode.ReadOnly = false;
                    textBoxTableName.ReadOnly = false; /*Rup 07/03/2011*/
                    buttonLookup.Enabled = false;
                    //buttonPreviewForm.Enabled = false;

                    btnNew.Enabled = false;
                    btnEdit.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;

                    textBoxFormID.Text = string.Empty;
                    textBoxCaption.Text = string.Empty;
                    textBoxCode.Text = string.Empty;
                    textBoxTableName.Text = string.Empty;
                    txtCaption.Text = string.Empty;

                    DevExpress.Utils.SetOptions.SetOptionValueByString("Editable", gridViewTabControls.OptionsBehavior, true);
                    DevExpress.Utils.SetOptions.SetOptionValueByString("Editable", gridViewFields.OptionsBehavior, true);
                    contextMenuStrip1.Enabled = true;
                    contextMenuStrip2.Enabled = true;

                    ToggleNavigationButtons(false, false, false, false, false, false);
                    break;
                case FormMode.Edit:
                    btnNew.Enabled = false;
                    btnEdit.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;

                    textBoxCaption.ReadOnly = false;
                    gridTabControl.Enabled = true;

                    DevExpress.Utils.SetOptions.SetOptionValueByString("Editable", gridViewTabControls.OptionsBehavior, true);
                    DevExpress.Utils.SetOptions.SetOptionValueByString("Editable", gridViewFields.OptionsBehavior, true);
                    contextMenuStrip1.Enabled = true;
                    contextMenuStrip2.Enabled = true;

                    ToggleNavigationButtons(false, false, false, false, false, false);
                    break;
                case FormMode.View:
                    btnNew.Enabled = true;
                    if (this.BindingContext[dsForms, "Forms"].Count > 0)
                        btnEdit.Enabled = true;
                    else
                        btnEdit.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;

                    textBoxCaption.ReadOnly = true;
                    textBoxCode.ReadOnly = true;
                    textBoxTableName.ReadOnly = true; /*Rup 07/03/2011*/
                    textBoxTableName.ReadOnly = true;
                    DevExpress.Utils.SetOptions.SetOptionValueByString("Editable", gridViewTabControls.OptionsBehavior, false);
                    DevExpress.Utils.SetOptions.SetOptionValueByString("Editable", gridViewFields.OptionsBehavior, false);
                    contextMenuStrip1.Enabled = false;
                    contextMenuStrip2.Enabled = false;

                    if (this.BindingContext[dsForms, "Forms"].Position == -1)
                        ToggleNavigationButtons(false, false, false, false, false, false);
                    else if (this.BindingContext[dsForms, "Forms"].Position == 0)
                        ToggleNavigationButtons(false, false, true, true, true, true);
                    else if (this.BindingContext[dsForms, "Forms"].Position == this.BindingContext[dsForms, "Forms"].Count - 1)
                        ToggleNavigationButtons(true, true, false, false, true, true);
                    else
                        ToggleNavigationButtons(true, true, true, true, true, true);

                    break;
                case FormMode.Cancel:
                    btnNew.Enabled = true;
                    btnEdit.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;

                    textBoxCaption.ReadOnly = true;
                    textBoxCode.ReadOnly = true;
                    textBoxTableName.ReadOnly = true; /*Rup 07/03/2011*/
                    //Clear out the Datasource for RepositoryItemTabLookup control.
                    this.repositoryItemTabLookUp.DataSource = null;
                    ToggleNavigationButtons(true, true, true, true, true, true);
                    break;
            }
        }

        private void ToggleNavigationButtons(bool First, bool Previous, bool Next, bool Last,
                                              bool Lookup, bool PreviewForm)
        {
            btnFirst.Enabled = First;
            btnBack.Enabled = Previous;
            btnForward.Enabled = Next;
            btnLast.Enabled = Last;
            btnLocate.Enabled = PreviewForm;

            buttonLookup.Enabled = Lookup;
            buttonPreviewForm.Enabled = PreviewForm;
        }

        private void SetBindingContextPosition(string bindingMode, int newPosition)
        {
            int recCount = this.BindingContext[dsForms, "Forms"].Count;
            int currentPos = this.BindingContext[dsForms, "Forms"].Position;

            switch (bindingMode)
            {
                case "First":
                    this.BindingContext[dsForms, "Forms"].Position = 0;
                    btnFirst.Enabled = false;
                    btnBack.Enabled = false;
                    btnLast.Enabled = true;
                    btnForward.Enabled = true;
                    break;
                case "Previous":
                    btnLast.Enabled = true;
                    btnForward.Enabled = true;
                    this.BindingContext[dsForms, "Forms"].Position = this.BindingContext[dsForms, "Forms"].Position - 1;
                    if (this.BindingContext[dsForms, "Forms"].Position == 0)
                    {
                        btnFirst.Enabled = false;
                        btnBack.Enabled = false;
                    }
                    break;
                case "Next":
                    btnFirst.Enabled = true;
                    btnBack.Enabled = true;
                    this.BindingContext[dsForms, "Forms"].Position = this.BindingContext[dsForms, "Forms"].Position + 1;
                    if (this.BindingContext[dsForms, "Forms"].Position == this.BindingContext[dsForms, "Forms"].Count - 1)
                    {
                        btnLast.Enabled = false;
                        btnForward.Enabled = false;
                    }
                    break;
                case "Last":
                    if (this.BindingContext[dsForms, "Forms"].Count == 0 || this.BindingContext[dsForms, "Forms"].Count == 1)
                    {
                        btnFirst.Enabled = false;
                        btnBack.Enabled = false;
                        btnLast.Enabled = false;
                        btnForward.Enabled = false;
                    }
                    else
                    {
                        btnFirst.Enabled = true;
                        btnBack.Enabled = true;
                        btnLast.Enabled = false;
                        btnForward.Enabled = false;
                    }
                    this.BindingContext[dsForms, "Forms"].Position = this.BindingContext[dsForms, "Forms"].Count;
                    break;
                case "SelectedRecord":
                    if (newPosition == 0)
                        SetBindingContextPosition("First", 0);
                    else if (newPosition + 1 == recCount)
                        SetBindingContextPosition("Last", 0);
                    else
                    {
                        this.BindingContext[dsForms, "Forms"].Position = newPosition;
                        btnFirst.Enabled = true;
                        btnBack.Enabled = true;
                        btnLast.Enabled = true;
                        btnForward.Enabled = true;
                    }
                    break;
            }
        }

        #endregion

        private void frmMasterFormDesigner_FormClosing(object sender, FormClosingEventArgs e)
        {
            oDataAccess = null;
            oForm = null;
            dsForms = null;
            dtDatatypes = null;
        }

        #region GridView Column Names, Column Widths

        private void SetGridColumnCaptions(string gridName)
        {
            switch (gridName)
            {
                case "gridViewTabControls":
                    gridViewTabControls.Columns["CAPTION"].Caption = "Caption";
                    gridViewTabControls.Columns["CAPTION"].Name = "CAPTION";
                    gridViewTabControls.Columns["CODE"].Caption = "Code";
                    gridViewTabControls.Columns["CODE"].Name = "CODE";
                    gridViewTabControls.Columns["TAB_ORDER"].Caption = "Order";
                    gridViewTabControls.Columns["TAB_ORDER"].Name = "TAB_ORDER";
                    gridViewTabControls.Columns["FORM_CODE"].Caption = "FORM_CODE";
                    gridViewTabControls.Columns["FORM_CODE"].Name = "FORM_CODE";
                    break;
                case "gridViewFields":
                    gridViewFields.Columns["SELECTEDTAB"].Caption = "Selected Tab";
                    gridViewFields.Columns["SELECTEDTAB"].Name = "SELECTEDTAB";
                    gridViewFields.Columns["FIELD_ORDER"].Caption = "Field Order";
                    gridViewFields.Columns["FIELD_ORDER"].Name = "FIELD_ORDER Order";
                    gridViewFields.Columns["CAPTION"].Caption = "Caption";
                    gridViewFields.Columns["CAPTION"].Name = "CAPTION";
                    gridViewFields.Columns["TOOLTIP"].Caption = "Tooltip";
                    gridViewFields.Columns["TOOLTIP"].Name = "TOOLTIP";
                    gridViewFields.Columns["MANDATORY"].Caption = "Mandatory";
                    gridViewFields.Columns["MANDATORY"].Name = "MANDATORY";
                    gridViewFields.Columns["FIELDNAME"].Caption = "Field Name";
                    gridViewFields.Columns["FIELDNAME"].Name = "FIELDNAME";
                    gridViewFields.Columns["DATATYPE"].Caption = "Datatype";
                    gridViewFields.Columns["DATATYPE"].Name = "DATATYPE";
                    gridViewFields.Columns["SIZE"].Caption = "Size";
                    gridViewFields.Columns["SIZE"].Name = "SIZE";
                    gridViewFields.Columns["DECIMAL"].Caption = "Decimal";
                    gridViewFields.Columns["DECIMAL"].Name = "DECIMAL";
                    gridViewFields.Columns["UNIQUE"].Caption = "Unique";
                    gridViewFields.Columns["UNIQUE"].Name = "UNIQUE";
                    gridViewFields.Columns["INPUTMASK"].Caption = "Input Mask";
                    gridViewFields.Columns["INPUTMASK"].Name = "INPUTMASK";
                    gridViewFields.Columns["HELPQUERY"].Caption = "Help Query";
                    gridViewFields.Columns["HELPQUERY"].Name = "HELPQUERY";
                    gridViewFields.Columns["REMARKS"].Caption = "Remarks";
                    gridViewFields.Columns["REMARKS"].Name = "REMARKS";
                    gridViewFields.Columns["WHENCONDITION"].Caption = "When Condition";
                    gridViewFields.Columns["WHENCONDITION"].Name = "WHENCONDITION";
                    gridViewFields.Columns["DEFAULTVALUE"].Caption = "Default Value";
                    gridViewFields.Columns["DEFAULTVALUE"].Name = "DEFAULTVALUE";
                    gridViewFields.Columns["VALIDATION"].Caption = "Validation";
                    gridViewFields.Columns["VALIDATION"].Name = "VALIDATION";
                    gridViewFields.Columns["INTERNALUSE"].Caption = "Internal Use";
                    gridViewFields.Columns["INTERNALUSE"].Name = "INTERNALUSE";
                    break;
            }
        }

        private void HideGridViewColumns(string gridName)
        {
            switch (gridName)
            {
                case "gridViewTabControls":
                    this.gridViewTabControls.Columns["FORM_CODE"].Visible = false;
                    this.gridViewTabControls.Columns["ID"].Visible = false;
                    break;
                case "gridViewFields":
                    this.gridViewFields.Columns["ID"].Visible = false;
                    this.gridViewFields.Columns["FORM_CODE"].Visible = false;
                    this.gridViewFields.Columns["TAB_CODE"].Visible = false;
                    this.gridViewFields.Columns["STATE"].Visible = false;
                    break;
            }
        }

        private void SetGridColumnWidths()
        {
            gridViewFields.Columns["SELECTEDTAB"].Width = 100;
            gridViewFields.Columns["FIELD_ORDER"].Width = 100;
            gridViewFields.Columns["MANDATORY"].Width = 100;
            gridViewFields.Columns["FIELDNAME"].Width = 100;
            gridViewFields.Columns["INPUTMASK"].Width = 100;
            gridViewFields.Columns["HELPQUERY"].Width = 100;
            gridViewFields.Columns["WHENCONDITION"].Width = 120;
            gridViewFields.Columns["DEFAULTVALUE"].Width = 100;
            gridViewFields.Columns["VALIDATION"].Width = 100;
            gridViewFields.Columns["INTERNALUSE"].Width = 100;
        }

        #endregion

        #region Shortcut Keys

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit?", "Logout", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
                Application.Exit();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
                this.Close();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnNew.Enabled)
                btnNew_Click(this.btnNew, e);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnEdit.Enabled)
                btnEdit_Click(this.btnEdit, e);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnSave.Enabled)
                btnSave_Click(this.btnSave, e);
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnCancel.Enabled)
                btnCancel_Click(this.btnCancel, e);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnExit_Click(this.btnExit, e);
        }

        #endregion

    }
}