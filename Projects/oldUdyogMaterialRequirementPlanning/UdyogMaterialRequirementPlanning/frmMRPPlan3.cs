using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Base;
using System.Collections;

namespace UdyogMaterialRequirementPlanning
{
    public partial class frmMRPPlan3 : Form
    {
        DataTable FinalData = new DataTable("Data");
        DataTable MainTable;
        DataTable ItemTable;
        DataTable Gen_Inv;
        DataTable Gen_Miss;
        DataTable Gen_doc;
        DataTable Lcode;
        DataTable DcMast;
        DataTable Stax_mas;
        DataTable Lother;
        DataTable Series;
        DataTable Category;
        DataTable FieldListTbl;
        DataTable MrpLog;
        DataTable PartyTable;

        ArrayList arrList = new ArrayList();
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemSeriesComboBox;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemCategoryComboBox;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryPartyNameComboBox;

        string Entry_Ty = string.Empty;
        string ErrorMsg = string.Empty;
        int TranGenType;
        string EntryTbl = string.Empty;
        #region Form Properties
        private bool _IsUpdated = false;
        public bool IsUpdated
        {
            get { return _IsUpdated; }
            set { _IsUpdated = value; }
        }


        #endregion

        public frmMRPPlan3(DataTable finalData, DataTable MRPLogTable, int tranGenType, string entry_ty, string caption)
        {
            InitializeComponent();
            FinalData = finalData;
            TranGenType = tranGenType;      //1-Single Transaction  2-Bomwise Transaction
            Entry_Ty = entry_ty;
            this.Text = caption;
            MrpLog = MRPLogTable;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //Application.Exit();
            this.Close();
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            string RetVal = string.Empty;
            RetVal = this.CheckValidations();
            if (RetVal.Trim().Length > 0)
            {
                MessageBox.Show(RetVal);
                return;
            }
            SqlTransaction tran = null;
            UdyogDataOperation.DBOperation op = new UdyogDataOperation.DBOperation();
            UdyogTranOperation.TranOperation tranOp = new UdyogTranOperation.TranOperation(); ;
            string inv_no = string.Empty;
            string inv_sr = string.Empty;
            string finYear = string.Empty;
            try
            {
                SqlConnection conn = new SqlConnection(clsCommon.ConnStr);
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                double TranId;
                int commitTransaction = 0;
                for (int i = 0; i < MainTable.Rows.Count; i++)
                {
                    try
                    {
                        tran = conn.BeginTransaction();
                        //cmd.Transaction = tran;
                        int tmpTranId = Convert.ToInt32(MainTable.Rows[i]["Tran_cd"]);
                        string docNo = Convert.ToString(MainTable.Rows[i]["Doc_no"]);
                        MainTable.Rows[i]["Inv_no"] = this.GetInvNo(Entry_Ty, DateTime.Today, new SqlConnection(clsCommon.ConnStr), MainTable.Rows[i]["Inv_sr"].ToString(), clsCommon.FinYear);
                        inv_no = tranOp.GenerateInvNo(Entry_Ty, MainTable.Rows[i]["Inv_sr"].ToString()
                                , MainTable.Rows[i]["Inv_no"].ToString(), DateTime.Today, "", "", (int)Convert.ToUInt32(Lcode.Rows[0]["Invno_size"]), new SqlConnection(clsCommon.ConnStr)
                                    , EntryTbl, clsCommon.DbName, clsCommon.FromDt, clsCommon.ToDt);
                        if (inv_no.Trim().Length == 0)
                            continue;
                        inv_sr = MainTable.Rows[i]["Inv_sr"].ToString();
                        MainTable.Rows[i]["Inv_no"] = inv_no;
                        finYear = Convert.ToString(MainTable.Rows[i]["l_yn"]);

                        MainTable.Rows[i]["Doc_no"] = tranOp.GenerateDocNo(Entry_Ty, DateTime.Today, EntryTbl, new SqlConnection(clsCommon.ConnStr), clsCommon.DbName, clsCommon.FromDt, clsCommon.ToDt);

                        MainTable.Rows[i]["Net_amt"] = (decimal)ItemTable.Compute("Sum(Gro_amt)", "Tran_cd=" + tmpTranId);
                        MainTable.Rows[i]["Gro_amt"] = (decimal)ItemTable.Compute("Sum(Gro_amt)", "Tran_cd=" + tmpTranId);
                        MainTable.Rows[i]["Ac_id"] = this.GetPartyId(MainTable.Rows[i]["Party_nm"].ToString());

                        cmd.Parameters.Clear();
                        cmd = op.GenerateInsertString(cmd, MainTable.Rows[i], EntryTbl + "main", new string[] { "Tran_cd" }, null);
                        cmd.Transaction = tran;
                        cmd.Connection = conn;
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            cmd.CommandText = "Select ident_current('" + EntryTbl + "main')";
                            TranId = (double)((decimal)cmd.ExecuteScalar());
                            MainTable.Rows[i]["Tran_cd"] = TranId;
                            int UpdateRec = 0;
                            int ItemRec = (int)ItemTable.Compute("count(Tran_cd)", "Tran_cd=" + tmpTranId.ToString().Trim());
                            for (int j = 0; j < ItemTable.Rows.Count; j++)
                            {
                                if (Convert.ToInt32(ItemTable.Rows[j]["Tran_cd"]) == tmpTranId)
                                {
                                    ItemTable.Rows[j]["Inv_no"] = MainTable.Rows[i]["Inv_no"];
                                    ItemTable.Rows[j]["Doc_no"] = MainTable.Rows[i]["Doc_no"];
                                    ItemTable.Rows[j]["Tran_cd"] = TranId;
                                    ItemTable.Rows[j]["Party_nm"] = MainTable.Rows[i]["Party_nm"];
                                    ItemTable.Rows[j]["Ac_Id"] = MainTable.Rows[i]["Ac_Id"];
                                    cmd.Parameters.Clear();
                                    cmd = op.GenerateInsertString(cmd, ItemTable.Rows[j], EntryTbl + "Item", null, null);
                                    cmd.ExecuteNonQuery();
                                    cmd.Parameters.Clear();

                                    UpdateRec++;
                                }
                            }
                            if (UpdateRec == ItemRec)
                            {
                                for (int l = 0; l < MrpLog.Rows.Count; l++)
                                {
                                    string fldNm = (Entry_Ty == "PD" ? "rIt_code" : "It_code");
                                    DataView itemview = ItemTable.DefaultView;
                                    itemview.RowFilter = "Tran_cd=" + MainTable.Rows[i]["Tran_cd"].ToString() + " and it_code=" + MrpLog.Rows[l][fldNm].ToString();
                                    for (int m = 0; m < itemview.Count; m++)
                                    {

                                        if (Convert.ToDecimal(MrpLog.Rows[l][fldNm]) == Convert.ToDecimal(itemview[m]["it_code"]))
                                        {
                                            MrpLog.Rows[l]["Rentry_ty"] = itemview[m]["Entry_ty"];
                                            MrpLog.Rows[l]["Itref_tran"] = itemview[m]["Tran_cd"];
                                            MrpLog.Rows[l]["RItserial"] = itemview[m]["Itserial"];
                                            MrpLog.Rows[l]["rIt_code"] = itemview[m]["It_code"];
                                            cmd = op.GenerateInsertString(cmd, MrpLog.Rows[l], "MRPLOG", new string[] { "ItemLvl" }, null);
                                            cmd.ExecuteNonQuery();
                                            cmd.Parameters.Clear();
                                        }
                                    }
                                    itemview.RowFilter = "";
                                }
                                tran.Commit();
                                commitTransaction++;
                            }
                        }
                        //ItemTable.AcceptChanges();
                        gridView2.ActiveFilterString = "";
                        if (gridView1.SelectedRowsCount > 0)
                        {
                            gridView2.ActiveFilterString = "Tran_cd=" + gridView1.GetDataRow(gridView1.FocusedRowHandle)["Tran_cd"].ToString();
                            gridView2.RefreshData();
                        }
                        else
                        {
                            gridView2.ActiveFilterString = "Tran_cd=" + gridView1.GetDataRow(0)["Tran_cd"].ToString();
                            gridView2.RefreshData();
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorMsg = ErrorMsg + Environment.NewLine + ex.Message;
                        if (tran != null)
                            tran.Rollback();
                        tranOp.DeleteGeneratedInvNo(Entry_Ty, inv_sr, inv_no, finYear, DateTime.Today, new SqlConnection(clsCommon.ConnStr), Convert.ToBoolean(Lcode.Rows[0]["Auto_inv"]));
                        continue;

                    }

                }
                this.Refresh();
                if (commitTransaction > 0)
                {
                    MessageBox.Show("Transaction(s) generated successfully.");
                    this.btnFinish.Enabled = false;
                    this.IsUpdated = true;
                    this.Refresh();
                }
                else
                {
                    throw new ApplicationException("Transaction not generated.");
                }
                //Application.Exit();
            }

            catch (Exception ex)
            {
                //if (tran != null)
                //    tran.Rollback();
                ErrorMsg = ErrorMsg + Environment.NewLine + ex.Message;
                //tranOp.DeleteGeneratedInvNo(Entry_Ty, inv_sr, inv_no.Trim(), finYear, DateTime.Now, new SqlConnection(connStr),Convert.ToBoolean(Lcode.Rows[0]["Auto_inv"]));
                MessageBox.Show(ErrorMsg);
                ErrorMsg = string.Empty;
                return;
            }

        }

        private string CheckValidations()
        {
            try
            {
                if (gridView1.RowCount > 0 && gridView1.Columns["inv_sr"].Visible == true)
                {
                    for (int i = 0; i < MainTable.Rows.Count; i++)
                    {
                        if (MainTable.Rows[i]["Inv_sr"].ToString().Trim().Length == 0)
                        {
                            gridView1.Focus();
                            throw new ApplicationException("Please select the series for transaction(s).");
                        }
                    }
                }
                if (gridView1.RowCount > 0 && gridView1.Columns["cate"].Visible == true)
                {
                    for (int i = 0; i < MainTable.Rows.Count; i++)
                    {
                        if (MainTable.Rows[i]["Cate"].ToString().Trim().Length == 0)
                        {
                            gridView1.Focus();
                            throw new ApplicationException("Please select the category for transaction(s).");
                        }
                    }
                }
                //if (gridView1.RowCount > 0 && gridView1.Columns["colparty_nm"].Visible == true)
                //{
                for (int i = 0; i < MainTable.Rows.Count; i++)
                {
                    if (MainTable.Rows[i]["Party_nm"].ToString().Trim().Length == 0)
                    {
                        gridView1.Focus();
                        throw new ApplicationException("Please select the party for transaction(s).");
                    }
                }
                //}
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return string.Empty;
        }
        private void frmMRPPlan3_Load(object sender, EventArgs e)
        {
            string retvalue = string.Empty;
            this.Text = clsCommon.ApplName;

            if (clsCommon.IconFile != null)
                this.Icon = new Icon(clsCommon.IconFile);
            retvalue = this.GenerateHeaderDetail();
            
            if (retvalue != string.Empty)
            {
                MessageBox.Show(retvalue);
                this.Close();
                return;
            }
            this.UpdateNarration();
            this.CreateFieldList();
            this.AssignDataSource();
        }

        private Int32 GetPartyId(string PartyName)
        {
            Int32 retVal = 0;
            string sqlstr = "Select Ac_id From Ac_mast Where Ac_name=@Ac_name";
            SqlConnection con = new SqlConnection(clsCommon.ConnStr);
            SqlCommand cmd = new SqlCommand(sqlstr, con);
            cmd.Parameters.Add(new SqlParameter("@Ac_name", PartyName));
            if (con.State == ConnectionState.Closed)
                con.Open();

            retVal = (Int32)(Convert.ToDecimal(cmd.ExecuteScalar()));

            if (con.State == ConnectionState.Open)
                con.Close();
            return retVal;
        }
        private void AssignDataSource()
        {
            this.gridControl1.DataSource = MainTable;
            gridControl1.MainView = gridView1;
            gridControl1.ForceInitialize();
            gridView1.PopulateColumns();
            gridView1.OptionsView.ShowGroupPanel = false;
            gridView1.OptionsView.ColumnAutoWidth = false;
            gridView1.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never;

            bool colFound;
            for (int i = 0; i < MainTable.Columns.Count; i++)
            {
                colFound = false;
                DataRow ldr = this.GetFieldInfo(1, MainTable.Columns[i].ColumnName.Trim());
                if (ldr == null)
                {
                    gridView1.Columns[i].Visible = colFound;
                    continue;
                }
                string colName = ldr["FldNm"].ToString();
                string colCaption = ldr["caption"].ToString(); ;
                string colWidth = ldr["width"].ToString();
                int colDeci = Convert.ToInt32(ldr["decimals"]);
                bool colEnabled = Convert.ToBoolean(ldr["Enabled"]);
                int colIndex = Convert.ToInt32(ldr["Index"]);
                if (MainTable.Columns[i].ColumnName.Trim().ToUpper() == colName.ToUpper())
                {
                    colFound = true;
                    gridView1.Columns[i].VisibleIndex = colIndex;
                    gridView1.Columns[i].Caption = colCaption;
                    gridView1.Columns[i].OptionsColumn.AllowEdit = colEnabled;
                    gridView1.Columns[i].Width = Convert.ToInt32(colWidth);
                    gridView1.Columns[i].OptionsColumn.AllowSort = DefaultBoolean.False;
                    if (gridView1.Columns[i].ColumnType.ToString() == "System.Decimal" || gridView1.Columns[i].ColumnType.ToString() == "System.Int32" || gridView1.Columns[i].ColumnType.ToString() == "System.Double")
                    {
                        gridView1.Columns[i].DisplayFormat.FormatType = FormatType.Numeric;
                        gridView1.Columns[i].DisplayFormat.FormatString = "0." + new string('0', colDeci);
                    }
                    // break;
                }

                gridView1.Columns[i].Visible = colFound;
            }
            gridView1.RefreshData();
            gridControl1.Refresh();

            this.gridControl2.DataSource = ItemTable;
            gridControl2.ForceInitialize();
            gridView2.PopulateColumns();
            gridView2.OptionsView.ShowGroupPanel = false;
            gridView2.OptionsView.ColumnAutoWidth = false;
            gridView2.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never;

            for (int i = 0; i < ItemTable.Columns.Count; i++)
            {
                colFound = false;
                DataRow ldr = this.GetFieldInfo(2, ItemTable.Columns[i].ColumnName.Trim());
                if (ldr == null)
                {
                    gridView2.Columns[i].Visible = colFound;
                    continue;
                }
                string colName = ldr["FldNm"].ToString();
                string colCaption = ldr["caption"].ToString();
                string colWidth = ldr["width"].ToString();
                int colDeci = Convert.ToInt32(ldr["decimals"]);
                bool colEnabled = Convert.ToBoolean(ldr["Enabled"]);
                int colIndex = Convert.ToInt32(ldr["Index"]);
                if (ItemTable.Columns[i].ColumnName.Trim().ToUpper() == colName.ToUpper())
                {
                    colFound = true;
                    gridView2.Columns[i].VisibleIndex = colIndex;
                    gridView2.Columns[i].Caption = colCaption;
                    gridView2.Columns[i].OptionsColumn.AllowEdit = colEnabled;
                    gridView2.Columns[i].Width = Convert.ToInt32(colWidth);
                    gridView2.Columns[i].OptionsColumn.AllowSort = DefaultBoolean.False;
                    if (gridView2.Columns[i].ColumnType.ToString() == "System.Decimal" || gridView2.Columns[i].ColumnType.ToString() == "System.Int32" || gridView2.Columns[i].ColumnType.ToString() == "System.Double")
                    {
                        gridView2.Columns[i].DisplayFormat.FormatType = FormatType.Numeric;
                        gridView2.Columns[i].DisplayFormat.FormatString = "0." + new string('0', colDeci);
                    }
                    // break;
                }

                gridView2.Columns[i].Visible = colFound;
            }
            gridView2.RefreshData();
            gridControl2.Refresh();
        }

        private DataRow GetFieldInfo(int TblType, string FieldName)
        {
            DataRow ldr = null;
            for (int i = 0; i < FieldListTbl.Rows.Count; i++)
            {
                if (FieldName.ToUpper().Trim() == FieldListTbl.Rows[i]["FldNm"].ToString().Trim().ToUpper() && Convert.ToInt32(FieldListTbl.Rows[i]["tblType"]) == TblType)
                {
                    ldr = FieldListTbl.Rows[i];
                }
            }
            return ldr;
        }

        private void CreateFieldList()
        {
            #region FieldListTbl table structure
            FieldListTbl = new DataTable("FldList");
            DataColumn dcTblType = new DataColumn("tblType", typeof(int));
            FieldListTbl.Columns.Add(dcTblType);
            DataColumn dcFldNm = new DataColumn("FldNm", typeof(string));
            FieldListTbl.Columns.Add(dcFldNm);
            DataColumn dcFldCaption = new DataColumn("Caption", typeof(string));
            FieldListTbl.Columns.Add(dcFldCaption);
            DataColumn dcFldWidth = new DataColumn("Width", typeof(int));
            FieldListTbl.Columns.Add(dcFldWidth);
            DataColumn dcFldIndex = new DataColumn("Index", typeof(int));
            FieldListTbl.Columns.Add(dcFldIndex);
            DataColumn dcFldDecimal = new DataColumn("Decimals", typeof(int));
            FieldListTbl.Columns.Add(dcFldDecimal);
            DataColumn dcFldEnable = new DataColumn("Enabled", typeof(bool));
            FieldListTbl.Columns.Add(dcFldEnable);
            #endregion

            DataRow row;
            string[] fieldList = new string[] { "Item_no:Item No.:50:0:0:0", "Item:Item Name:250:1:0:0", "Qty:Quantity:90:2:3:0", "Rate:Rate:90:3:2:1", "u_asseamt:Ass. Value:90:4:2:0" };

            #region Item columns
            for (int i = 0; i < fieldList.Length; i++)
            {
                string[] cols = fieldList[i].Split(':');
                row = FieldListTbl.NewRow();
                row[0] = 2;             //1 for Main table  2 for Item table
                row[1] = cols[0];
                row[2] = cols[1];
                row[3] = cols[2];
                row[4] = cols[3];
                row[5] = cols[4];
                row[6] = (cols[5] == "1" ? true : false);
                FieldListTbl.Rows.Add(row);
            }
            #endregion

            fieldList = new string[] { "Date:Date:70:1:0:0", "Party_nm:Party Name:150:2:0:0", "Inv_no:Transaction No.:90:4:0:0" };
            #region Main columns
            for (int i = 0; i < fieldList.Length; i++)
            {
                string[] cols = fieldList[i].Split(':');
                row = FieldListTbl.NewRow();
                row[0] = 1;             //1 for Main table  2 for Item table
                row[1] = cols[0];
                row[2] = cols[1];
                row[3] = cols[2];
                row[4] = cols[3];
                row[5] = cols[4];
                row[6] = (cols[5] == "1" ? true : false);
                FieldListTbl.Rows.Add(row);
            }
            #endregion

            #region Series records
            if (Series.Rows.Count > 0)
            {
                repositoryItemSeriesComboBox = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
                for (int i = 0; i < Series.Rows.Count; i++)
                {
                    repositoryItemSeriesComboBox.Items.Add(Series.Rows[i]["Inv_sr"].ToString());
                }
                gridControl1.RepositoryItems.Add(repositoryItemSeriesComboBox);


                row = FieldListTbl.NewRow();
                row[0] = 1;             //1 for Main table  2 for Item table
                row[1] = "Inv_sr";
                row[2] = "Series";
                row[3] = 60;
                row[4] = 3;
                row[5] = 0;
                row[6] = true;
                FieldListTbl.Rows.Add(row);
            }
            #endregion

            #region Category records
            if (Category.Rows.Count > 0)
            {
                if (PartyTable.Rows.Count > 0)
                {

                    repositoryItemCategoryComboBox = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
                    for (int i = 0; i < Series.Rows.Count; i++)
                    {
                        repositoryItemCategoryComboBox.Items.Add(Category.Rows[i]["cate"].ToString());
                    }
                    gridControl1.RepositoryItems.Add(repositoryItemCategoryComboBox);
                }
                row = FieldListTbl.NewRow();
                row[0] = 1;             //1 for Main table  2 for Item table
                row[1] = "Cate";
                row[2] = "Category";
                row[3] = 60;
                row[4] = 5;
                row[5] = 0;
                row[6] = true;
                FieldListTbl.Rows.Add(row);
            }
            #endregion

            #region Party Records
            if (this.Entry_Ty != "WK")
            {
                if (PartyTable.Rows.Count > 0)
                {
                    repositoryPartyNameComboBox = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
                    for (int i = 0; i < PartyTable.Rows.Count; i++)
                    {
                        repositoryPartyNameComboBox.Items.Add(PartyTable.Rows[i]["Party_nm"].ToString());
                    }
                    gridControl1.RepositoryItems.Add(repositoryPartyNameComboBox);

                    row = FieldListTbl.NewRow();
                    row[0] = 1;             //1 for Main table  2 for Item table
                    row[1] = "Party_nm";
                    row[2] = "Party Name";
                    row[3] = 125;
                    row[4] = 2;
                    row[5] = 0;
                    row[6] = true;
                    FieldListTbl.Rows.Add(row);
                }
            }
            #endregion

            #region Dcmast Columns
            int ColIndex = (int)FieldListTbl.Compute("Count(fldnm)", "");
            for (int i = 0; i < DcMast.Rows.Count; i++)
            {
                if (Convert.ToString(DcMast.Rows[i]["pert_name"]).Length > 0)
                {
                    row = FieldListTbl.NewRow();
                    row[0] = (DcMast.Rows[i]["att_file"].ToString() == "True" ? 1 : 2);             //1 for Main table  2 for Item table
                    row[1] = DcMast.Rows[i]["pert_name"].ToString().Trim();
                    row[2] = DcMast.Rows[i]["Disp_sign"].ToString().Trim();
                    row[3] = 80;
                    row[4] = ++ColIndex;
                    row[5] = 2;
                    row[6] = true;
                    FieldListTbl.Rows.Add(row);
                }
                row = FieldListTbl.NewRow();
                row[0] = (DcMast.Rows[i]["att_file"].ToString() == "True" ? 1 : 2);             //1 for Main table  2 for Item table
                row[1] = DcMast.Rows[i]["fld_nm"].ToString().Trim();
                row[2] = DcMast.Rows[i]["head_nm"].ToString().Trim();
                row[3] = 100;
                row[4] = ++ColIndex;
                row[5] = 2;
                row[6] = true;
                FieldListTbl.Rows.Add(row);

            }
            #endregion

            #region Lother columns
            for (int i = 0; i < Lother.Rows.Count; i++)
            {
                row = FieldListTbl.NewRow();
                row[0] = (Lother.Rows[i]["att_file"].ToString() == "True" ? 1 : 2);             //1 for Main table  2 for Item table
                row[1] = Lother.Rows[i]["fld_nm"].ToString().Trim();
                row[2] = Lother.Rows[i]["head_nm"].ToString().Trim();
                row[3] = 100;
                row[4] = ++ColIndex;
                row[5] = Lother.Rows[i]["fld_Dec"].ToString().Trim();
                row[6] = true;
                FieldListTbl.Rows.Add(row);
            }
            #endregion

        }
        private void CheckSeriesCategory()
        {

        }
        private string GenerateHeaderDetail()
        {
            string errmsg = string.Empty;
            errmsg = this.CreateTempCursors();
            if (errmsg != string.Empty)
                return errmsg;
            switch (TranGenType)
            {
                case 1:         //Single Transaction
                    this.GenerateSingleTransaction();
                    break;
                case 2:         //Multiple Transaction
                    this.GenerateMultipleTransaction();
                    break;
                default:
                    break;
            }
            return errmsg;
        }
        private void GenerateSingleTransaction()
        {
            int NoOfTran = 1;
            string partynm = string.Empty;
            string inv_sr = string.Empty;
            string cate = string.Empty;
            int partycode = 0;

            for (int i = 0; i < NoOfTran; i++)
            {
                //DataRow MainRow = MainTable.NewRow();
                #region "Main Header"
                DataRow MainRow = this.GetDataRow(MainTable);
                MainRow["Entry_ty"] = Entry_Ty;
                MainRow["Date"] = DateTime.Today;
                MainRow["Doc_No"] = this.GenerateDocNo(Entry_Ty);
                MainRow["Inv_no"] = string.Empty;
                //MainRow["Party_nm"] = (this.Entry_Ty == "WK" ? "WORK ORDER" : "USE FOR PRODUCTION");
                MainRow["Ac_id"] = this.GetId("Ac_mast", "Ac_name", "Ac_Id", MainRow["Party_nm"].ToString(), new SqlConnection(clsCommon.ConnStr));
                MainRow["l_yn"] = clsCommon.FinYear;
                MainRow["narr"] = "Generated from MRP " + DateTime.Today.ToString("dd-MM-yyyy");
                MainRow["Apledby"] = clsCommon.AppUserName;
                MainRow["Apgenby"] = clsCommon.AppUserName;
                MainRow["Apgen"] = "YES";
                MainRow["Apled"] = "YES";
                MainRow["CompId"] = clsCommon.CompId;
                MainRow["Apledtime"] = clsCommon.AppUserName;
                MainRow["Apgentime"] = clsCommon.AppUserName;
                MainRow["User_name"] = clsCommon.AppUserName;
                MainRow["l_yn"] = clsCommon.FinYear;
                MainRow["due_dt"] = MainRow["Date"];
                partynm = Convert.ToString(MainRow["Party_nm"]);
                partycode = Convert.ToInt32(MainRow["ac_id"]);
                inv_sr = MainRow["Inv_sr"].ToString();
                cate = MainRow["cate"].ToString();
                MainTable.Rows.Add(MainRow);
                #endregion

                DataRow ItemRow;
                #region "Item Detail"
                for (int j = 0; j < FinalData.Rows.Count; j++)
                {
                    //ItemRow = ItemTable.NewRow();
                    ItemRow = this.GetDataRow(ItemTable);
                    ItemRow["Entry_ty"] = MainRow["Entry_ty"];
                    ItemRow["Date"] = MainRow["Date"];
                    ItemRow["Inv_no"] = MainRow["Inv_no"];
                    ItemRow["Doc_no"] = MainRow["Doc_no"];
                    ItemRow["party_nm"] = partynm;
                    ItemRow["ac_id"] = partycode;
                    ItemRow["Inv_sr"] = inv_sr;
                    ItemRow["Cate"] = cate;
                    ItemRow["l_yn"] = clsCommon.FinYear;
                    ItemRow["Item"] = FinalData.Rows[j]["item"];
                    ItemRow["It_code"] = FinalData.Rows[j]["It_code"];
                    ItemRow["Itserial"] = this.GenerateItserial(j + 1);
                    ItemRow["Item_no"] = (j + 1).ToString().PadCenter(5);
                    ItemRow["Rate"] = 0.00;
                    ItemRow["u_asseamt"] = 0.00;
                    ItemRow["CompId"] = clsCommon.CompId;
                    ItemRow["l_yn"] = clsCommon.FinYear;
                    ItemRow["Qty"] = Convert.ToDecimal(FinalData.Rows[j]["Qty"]);
                    if (Entry_Ty == "WK")
                    {
                        this.UpdateWKOrderDetails(ItemRow, FinalData.Rows[j]["It_code"].ToString());
                    }
                    ItemTable.Rows.Add(ItemRow);
                }
                #endregion
            }
        }
        private void GenerateMultipleTransaction()
        {
            int NoOfTran = 1;
            int Itserial = 0;
            DataView view = FinalData.DefaultView;
            DataTable distinctItem = view.ToTable("FinalData", true, new string[] { "Item" });
            NoOfTran = distinctItem.Rows.Count;

            DataRow ItemRow;

            int TranId = 0;
            string tmpValue = "0";
            string tmpDocNo = string.Empty;
            string partynm = string.Empty;
            string inv_sr = string.Empty;
            string cate = string.Empty;
            int partycode = 0;
            string PartyValue = string.Empty;
            DataColumn colParty = new DataColumn("Party_nm", typeof(string));
            FinalData.Columns.Add(colParty);
            DataColumn colPartyId = new DataColumn("Ac_id", typeof(Int32));
            FinalData.Columns.Add(colPartyId);
            DataColumn colRate = new DataColumn("Rate", typeof(decimal));
            FinalData.Columns.Add(colRate);

            for (int x = 0; x < FinalData.Rows.Count; x++)
            {
                FinalData.Rows[x]["Ac_id"] = 0;
                FinalData.Rows[x]["Rate"] = 0.00;
                FinalData.Rows[x]["Party_nm"] = "";
                if (Convert.ToBoolean(Lcode.Rows[0]["It_rate"]))
                {
                    if (!this.GetItemRateAndParty(Convert.ToString(FinalData.Rows[x]["It_code"]), x))
                    {
                        MessageBox.Show("Error occured while updating the rates.");
                        return;
                    }
                }
            }
            view.Sort = "Party_Nm";
            for (int j = 0; j < view.Count; j++)
            {
                PartyValue = (this.Entry_Ty == "WK" ? "WORK ORDER" : view[j]["Party_nm"].ToString().Trim());
                #region "Main Header"
                if (tmpValue != PartyValue)
                {
                    tmpValue = PartyValue;
                    tmpDocNo = (tmpDocNo == string.Empty ? this.GenerateDocNo(Entry_Ty) : (Convert.ToInt32(tmpDocNo) + 1).ToString().PadLeft(5, '0'));
                    //DataRow MainRow = MainTable.NewRow();
                    DataRow MainRow = this.GetDataRow(MainTable);
                    MainRow["Entry_ty"] = Entry_Ty;
                    MainRow["Date"] = DateTime.Today;
                    MainRow["Doc_No"] = tmpDocNo;
                    MainRow["Inv_no"] = string.Empty;
                    //MainRow["Party_nm"] = (this.Entry_Ty == "WK" ? "WORK ORDER" : "USE FOR PRODUCTION");
                    //MainRow["Party_nm"] = (this.Entry_Ty == "WK" ? "WORK ORDER" : Convert.ToString(MainRow["Party_nm"]));
                    //MainRow["Ac_id"] = this.GetId("Ac_mast", "Ac_name", "Ac_Id", MainRow["Party_nm"].ToString(), new SqlConnection(clsCommon.ConnStr));
                    MainRow["Party_nm"] = PartyValue;
                    MainRow["Ac_id"] = this.GetId("Ac_mast", "Ac_name", "Ac_Id", MainRow["Party_nm"].ToString(), new SqlConnection(clsCommon.ConnStr));
                    inv_sr = MainRow["Inv_sr"].ToString();
                    cate = MainRow["cate"].ToString();
                    MainRow["l_yn"] = clsCommon.FinYear;
                    MainRow["narr"] = "Generated from MRP " + DateTime.Today.ToString("dd-MM-yyyy");
                    MainRow["Apledby"] = clsCommon.AppUserName;
                    MainRow["Apgenby"] = clsCommon.AppUserName;
                    MainRow["Apgen"] = "YES";
                    MainRow["Apled"] = "YES";
                    MainRow["CompId"] = clsCommon.CompId;
                    MainRow["l_yn"] = clsCommon.FinYear;
                    MainRow["Apledtime"] = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                    MainRow["Apgentime"] = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                    MainRow["Sysdate"] = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                    MainRow["User_name"] = clsCommon.AppUserName;
                    MainRow["due_dt"] = MainRow["Date"];
                    TranId = TranId + 1;
                    MainRow["Tran_cd"] = TranId;
                    MainTable.Rows.Add(MainRow);

                    partynm = Convert.ToString(MainRow["Party_nm"]);
                    partycode = Convert.ToInt32(MainRow["ac_id"]);
                    Itserial = 1;
                }
                #endregion
                //ItemRow = ItemTable.NewRow();
                #region "Item Detail"
                ItemRow = this.GetDataRow(ItemTable);
                ItemRow["Entry_ty"] = Entry_Ty;
                ItemRow["Date"] = DateTime.Today;
                ItemRow["Inv_no"] = "";
                ItemRow["Doc_no"] = tmpDocNo;
                ItemRow["Item"] = view[j]["item"];
                ItemRow["It_code"] = view[j]["It_code"];
                ItemRow["Itserial"] = this.GenerateItserial(Itserial);
                ItemRow["Item_no"] = Itserial.ToString().PadCenter(5);
                ItemRow["Party_nm"] = partynm;
                ItemRow["Ac_id"] = partycode;
                ItemRow["Inv_sr"] = inv_sr;
                ItemRow["Cate"] = cate;
                ItemRow["l_yn"] = clsCommon.FinYear;
                ItemRow["CompId"] = clsCommon.CompId;
                ItemRow["Rate"] = view[j]["Rate"];
                ItemRow["Qty"] = Convert.ToDecimal(view[j]["Qty"]);
                ItemRow["u_asseamt"] = Convert.ToDecimal(view[j]["Qty"]) * Convert.ToDecimal(view[j]["Rate"]);
                ItemRow["Tran_cd"] = TranId;
                ItemRow["narr"] = DateTime.Today.ToString("dd-MM-yyyy");
                if (Entry_Ty == "WK")
                {
                    this.UpdateWKOrderDetails(ItemRow, view[j]["It_code"].ToString());
                }
                ItemTable.Rows.Add(ItemRow);

                Itserial++;
                #endregion
            }
        }
        private void UpdateNarration()
        {
            string Narr = "Generated from MRP " + DateTime.Today.ToString("dd-MM-yyyy") + " Against ";
            DataTable OrderTbl = MrpLog.DefaultView.ToTable(true, new string[] { "Entry_ty","OrderNo" });
            DataView orderView = OrderTbl.DefaultView;
            orderView.Sort = "Entry_ty,OrderNo";
            string code = "";
            string tmpcode = "0";
            string code_nm = string.Empty;
            foreach (DataRow itemRow in OrderTbl.Rows)
            {
                code =itemRow["Entry_ty"].ToString().Trim();
                if (tmpcode != code)
                {
                    tmpcode = code;
                    try
                    {
                        SqlConnection con = new SqlConnection(clsCommon.ConnStr);
                        string SqlStr = "Select Code_nm From Lcode Where Entry_ty=@Entry_ty";
                        SqlCommand cmd = new SqlCommand(SqlStr, con);
                        cmd.Parameters.Add(new SqlParameter("@Entry_ty", code));
                        con.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            code_nm = dr["Code_nm"].ToString().Trim().ToTitleCase()+" Nos.:";
                        }
                        dr.Close();
                        if (con.State == ConnectionState.Open)
                        {
                            con.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        code_nm = " Order Nos. ";
                    }
                    Narr = Narr + " " + code_nm;
                }
                
                Narr = Narr + " " + itemRow["OrderNo"].ToString().Trim()+",";
            }

            for (int i = 0; i < MainTable.Rows.Count; i++)
            {
                MainTable.Rows[i]["Narr"] = Narr.Substring(0,Narr.Length-1);
            }
        }
        private bool GetItemRateAndParty(string ItemCode, int ItemRow)
        {
            try
            {
                SqlConnection con = new SqlConnection(clsCommon.ConnStr);
                string SqlStr = "Execute USP_ENT_GETITEMDETAILS @It_code";
                SqlCommand cmd = new SqlCommand(SqlStr, con);
                cmd.Parameters.Add(new SqlParameter("@It_code", ItemCode));
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    FinalData.Rows[ItemRow]["Ac_id"] = Convert.ToUInt32(dr["Ac_id"]);
                    FinalData.Rows[ItemRow]["Rate"] = dr["Rate"];
                    FinalData.Rows[ItemRow]["Party_nm"] = dr["Party_nm"];
                }
                dr.Close();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

        }

        private void UpdateWKOrderDetails(DataRow itemRow, string It_code)
        {
            string sqlstr = "Select Top 1 Bomid,Bomlevel From BomHead Where ItemId=" + It_code;
            SqlDataReader r;
            SqlConnection conn = new SqlConnection(clsCommon.ConnStr);
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            SqlCommand cmd = new SqlCommand(sqlstr, conn);
            r = cmd.ExecuteReader();

            while (r.Read())
            {
                itemRow["BomId"] = r["BomId"];
                itemRow["Bomlevel"] = r["Bomlevel"];
            }
            r.Close();
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }
        private int GetId(string tableName, string SearchFld, string ReturnFld, string value, SqlConnection conn)
        {
            int retAcId = 0;

            string sqlstr = "Select Top 1 " + ReturnFld + " From " + tableName + " Where " + SearchFld + "=@Value";
            SqlDataReader r;
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            SqlCommand cmd = new SqlCommand(sqlstr, conn);
            cmd.Parameters.Add(new SqlParameter("@Value", value));
            r = cmd.ExecuteReader();

            // Iterate over the results.

            while (r.Read())
            {
                retAcId = (int)Convert.ToInt32(r[0]);
            }
            return retAcId;
        }


        private string GenerateDocNo(string EntryTy)
        {
            string retVal = string.Empty;
            int docno = 0;
            string sqlstr = "Select Doc_no From Gen_Doc Where Entry_ty='" + Entry_Ty + "' And Date =CONVERT(SmallDateTime,GETDATE())";
            SqlConnection con = new SqlConnection(clsCommon.ConnStr);
            SqlCommand cmd = new SqlCommand(sqlstr, con);
            if (con.State == ConnectionState.Closed)
                con.Open();

            docno = (int)(Convert.ToDecimal(cmd.ExecuteScalar()));
            if (docno == null)
                docno = 0;

            retVal = (docno + 1).ToString().PadLeft(5, '0');
            if (con.State == ConnectionState.Open)
                con.Close();
            return retVal;
        }
        private string GenerateItserial(int LineItemNo)
        {
            string retVal = string.Empty;
            retVal = LineItemNo.ToString().PadLeft(5, '0');
            return retVal;
        }
        private string GetInvNo(string EntryTy, DateTime VentDate, SqlConnection conn, string InvoiceSeries, string finYear)
        {
            string retVal = string.Empty;
            string sqlStr = string.Empty;
            string SeriesType = string.Empty;
            string prefix = string.Empty;
            string suffix = string.Empty;
            string monthFormat = string.Empty;
            string v_i_middle = string.Empty;

            DataTable TmpTbl = new DataTable();
            SqlCommand cmd;

            cmd = new SqlCommand("Select top 1 * From Series Where Inv_sr=@Series", conn);
            cmd.Parameters.Add(new SqlParameter("@Series", InvoiceSeries));
            DataTable Series = new DataTable("Series_vw");
            //cmd.Transaction = tran;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(Series);

            if (Series.Rows.Count > 0)
            {
                SeriesType = (Convert.ToString(Series.Rows[0]["s_Type"])).Trim();
                prefix = (Convert.ToString(Series.Rows[0]["i_prefix"]).Replace('"', ' ').Replace("'", "").Trim()).Trim();
                suffix = (Convert.ToString(Series.Rows[0]["i_suffix"]).Replace('"', ' ').Replace("'", "")).Trim();
                monthFormat = (Convert.ToString(Series.Rows[0]["mnthformat"])).Trim();

                switch (SeriesType)
                {
                    case "DAYWISE":
                        v_i_middle = VentDate.ToString("ddMMyy");
                        break;
                    case "MONTHWISE":
                        string SQL = "SELECT MnthFrmt FROM monthformat Where MnthFrmt ='" + monthFormat + "'";
                        cmd = new SqlCommand(SQL, conn);
                        SqlDataReader r;
                        if (conn.State == ConnectionState.Closed)
                            conn.Open();
                        r = cmd.ExecuteReader();

                        // Iterate over the results.

                        while (r.Read())
                        {
                            v_i_middle = Convert.ToString(r["MnthFrmt"]);
                        }
                        r.Close();
                        if (v_i_middle == string.Empty)
                            v_i_middle = VentDate.ToString("MMdd");
                        break;
                    default:
                        break;
                }
            }

            switch (SeriesType)
            {
                case "DAYWISE":
                    sqlStr = "Select Top 1 Inv_no from Gen_inv with (TABLOCKX) where Entry_ty =@Entry_ty And Inv_sr = @Series "
                            + " And Inv_dt = @Inv_dt";
                    cmd = new SqlCommand(sqlStr, conn);
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new SqlParameter("@Entry_ty", Entry_Ty));
                    cmd.Parameters.Add(new SqlParameter("@Series", InvoiceSeries));
                    cmd.Parameters.Add(new SqlParameter("@Inv_dt", VentDate.ToString("MM/dd/yyyy")));

                    break;
                case "MONTHWISE":
                    sqlStr = "  Select Top 1 Inv_no from Gen_inv with (TABLOCKX) where Entry_ty = @Entry_ty "
                            + " And Inv_sr = @Series And MONTH(Inv_dt) = @Inv_dt_m And Year(Inv_dt) = @Inv_dt_y ";
                    cmd = new SqlCommand(sqlStr, conn);
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new SqlParameter("@Entry_ty", Entry_Ty));
                    cmd.Parameters.Add(new SqlParameter("@Series", InvoiceSeries));
                    cmd.Parameters.Add(new SqlParameter("@Inv_dt_m", VentDate.Month));
                    cmd.Parameters.Add(new SqlParameter("@Inv_dt_y", VentDate.Year));
                    break;
                default:
                    sqlStr = "  Select Top 1 Inv_no from Gen_inv with (TABLOCKX) where Entry_ty =@Entry_ty  "
                                + " And Inv_sr = @Series And L_yn = @L_yn ";
                    cmd = new SqlCommand(sqlStr, conn);
                    cmd.Parameters.Add(new SqlParameter("@Entry_ty", Entry_Ty));
                    cmd.Parameters.Add(new SqlParameter("@Series", InvoiceSeries));
                    cmd.Parameters.Add(new SqlParameter("@L_yn", finYear));
                    break;
            }
            cmd.Connection = conn;
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            int TranNo = (int)(Convert.ToInt32(cmd.ExecuteScalar()));
            if (TranNo == null)
            {
                TranNo = 0;
            }
            TranNo = TranNo + 1;
            retVal = TranNo.ToString().Trim().PadLeft(Convert.ToInt32(Lcode.Rows[0]["Invno_size"]), '0');


            cmd = new SqlCommand("Select [length] from Syscolumns where [Name]='Inv_no' and Id=Object_Id('" + EntryTbl + "main')", conn);
            //cmd.Transaction = tran;
            cmd.Connection = conn;
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            int Inv_Size = (int)(Convert.ToDouble(cmd.ExecuteScalar()));
            retVal = (prefix + v_i_middle + retVal + suffix).PadRight(Inv_Size, ' ');
            return retVal;
        }

        private void FormSetting()
        {

        }
        private string CreateTempCursors()
        {
            string SqlStr = string.Empty;
            string errmsg = string.Empty;
            SqlConnection con = new SqlConnection(clsCommon.ConnStr);
            SqlStr = "Select * From Lcode Where Entry_ty=@Entry_ty";
            SqlCommand cmd = new SqlCommand(SqlStr, con);
            cmd.Parameters.Add(new SqlParameter("@Entry_ty", Entry_Ty));
            SqlDataAdapter lda = new SqlDataAdapter(cmd);
            Lcode = new DataTable("Lcode_vw");
            lda.Fill(Lcode);
            if (Lcode.Rows.Count <= 0)
            {
                errmsg = Entry_Ty + " Transaction not found in Transaction setting.";
                return errmsg;
            }

            SqlStr = "Select * From DcMast Where Entry_ty=@Entry_ty";
            cmd.CommandText = SqlStr;
            DcMast = new DataTable("DcMast_vw");
            lda.Fill(DcMast);
            SqlStr = "Select * From Lother Where E_code=@Entry_ty";
            cmd.CommandText = SqlStr;
            Lother = new DataTable("Lother_vw");
            lda.Fill(Lother);

            cmd.Parameters.Clear();
            SqlStr = "Select * From stax_mas Where Entry_ty like '%" + Entry_Ty + "%'";
            cmd.CommandText = SqlStr;
            Stax_mas = new DataTable("Stax_mas_vw");
            lda.Fill(Stax_mas);

            SqlStr = "Select * From Series Where Validity like '%" + Entry_Ty + "%'";
            cmd.CommandText = SqlStr;
            Series = new DataTable("Series_vw");
            lda.Fill(Series);

            SqlStr = "Select * From Category Where Validity like '%" + Entry_Ty + "%'";
            cmd.CommandText = SqlStr;
            Category = new DataTable("Category_vw");
            lda.Fill(Category);

            string Groups = Convert.ToString(Lcode.Rows[0]["Defa_group"]).Trim();

            string[] SearchGroups = Groups.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string groupList = string.Empty;
            foreach (string gp in SearchGroups)
            {
                groupList = groupList + "'" + gp + "',";
            }

            groupList = (groupList.Trim().Length > 3 ? groupList.Substring(0, groupList.Length - 1) : "'SUNDRY DEBTORS','SUNDRY CREDITORS'");

            SqlStr = "Select ac_name as Party_nm From Ac_mast Where [Group] in (" + groupList + ")";
            cmd.CommandText = SqlStr;
            PartyTable = new DataTable("Party_vw");
            lda.Fill(PartyTable);

            EntryTbl = (Convert.ToString(Lcode.Rows[0]["Bcode_nm"]).Trim() != "" ? Convert.ToString(Lcode.Rows[0]["Bcode_nm"]).Trim() : (Convert.ToBoolean(Lcode.Rows[0]["ext_vou"]) ? "" : Convert.ToString(Lcode.Rows[0]["Entry_ty"]).Trim()));

            SqlStr = "Select * From " + EntryTbl + "Main Where 1=2";
            cmd.CommandText = SqlStr;
            MainTable = new DataTable("Main_vw");
            lda.Fill(MainTable);

            SqlStr = "Select * From " + EntryTbl + "Item Where 1=2";
            cmd.CommandText = SqlStr;
            ItemTable = new DataTable("Item_vw");
            lda.Fill(ItemTable);

            con = null;
            cmd = null;
            lda = null;
            return errmsg;
        }
        private DataRow GetDataRow(DataTable dt)
        {
            DataRow row = dt.NewRow();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                switch (dt.Columns[i].DataType.ToString())
                {
                    case "System.Int16":
                        row[i] = Convert.ToInt16(0);
                        break;
                    case "System.Int32":
                        row[i] = Convert.ToInt32(0.00);
                        break;
                    case "System.Int64":
                        row[i] = Convert.ToInt64(0.00);
                        break;
                    case "System.Decimal":
                        row[i] = Convert.ToDecimal(0.00);
                        break;
                    case "System.Double":
                        row[i] = Convert.ToDouble(0.00);
                        break;
                    case "System.String":
                        row[i] = "";
                        break;
                    case "System.Boolean":
                        row[i] = Convert.ToBoolean(false);
                        break;
                    case "System.DateTime":
                        row[i] = Convert.ToDateTime("01/01/1900");
                        break;
                    default:
                        break;
                }
            }
            return row;
        }

        private void gridView1_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            switch (e.Column.FieldName.ToUpper().Trim())
            {
                case "INV_SR":
                    e.RepositoryItem = repositoryItemSeriesComboBox;
                    break;
                case "CATE":
                    e.RepositoryItem = repositoryItemCategoryComboBox;
                    break;
                case "PARTY_NM":
                    e.RepositoryItem = repositoryPartyNameComboBox;
                    break;
                default:
                    break;
            }
        }

        private void gridView2_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName.ToUpper().Trim() == "RATE")
            {
                decimal Qty = Convert.ToDecimal(gridView2.GetRowCellValue(e.RowHandle, gridView2.Columns["qty"]));
                gridView2.SetRowCellValue(e.RowHandle, gridView2.Columns["u_asseamt"], Qty * Convert.ToDecimal(e.Value));
                gridView2.SetRowCellValue(e.RowHandle, gridView2.Columns["gro_amt"], Convert.ToDecimal(gridView2.GetRowCellValue(e.RowHandle, gridView2.Columns["u_asseamt"])));
            }
        }

        private void gridView1_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            if (gridView1.SelectedRowsCount > 0)
            {
                gridView2.ActiveFilterString = "Tran_cd=" + gridView1.GetDataRow(gridView1.FocusedRowHandle)["Tran_cd"].ToString().Trim();
                gridView2.RefreshData();
            }
            else
            {
                gridView2.ActiveFilterString = "Tran_cd=" + gridView1.GetDataRow(0)["Tran_cd"].ToString().Trim();
                gridView2.RefreshData();
            }
        }

    }
}
