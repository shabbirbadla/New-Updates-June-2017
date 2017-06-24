using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;//Regex 
using System.Runtime.InteropServices;
using udclsDGVNumericColumn;//Numeric value checking
using System.Globalization;
using System.Threading;
using System.Diagnostics;
using System.IO;
using DataAccess_Net;       //----- Added by Sachin N. S. on 08/03/2013 

namespace Dados_Report_Wizard
{
    public partial class frmDadosReport : Form
    {
        const int MF_BYPOSITION = 0x400;
        [DllImport("User32")]
        private static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("User32")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("User32")]
        private static extern int GetMenuItemCount(IntPtr hWnd);
        string strrep, strpara, strrlv, strcol, strqry;
        string strParaid = string.Empty;
        string strparamvalue = string.Empty;
        string strQryid = string.Empty;
        string strDisor = string.Empty;
        string filter = string.Empty;
        string filter1 = string.Empty;
        string filter2 = string.Empty;
        string paratype = string.Empty;
        string strRowId = string.Empty;
        string CharacterToFind;
        int maxQryid = 0, lvlcount = 0, maxColid = 0, rowindx, colindx, lvlNum = 1;   //----- Added by Sachin N. S. on 05/08/2013 for Bug-4524
        ReadSetting readIni;
        SqlConnection con;
        DataSet ds = new DataSet();
        DataRow replvl, col, rep, dr1, drqry, drcol;
        DataRow dr;                             //----- Added by Sachin N. S. on 08/03/2013 
        DataTable dt = new DataTable();           //----- Added by Sachin N. S. on 08/03/2013 
        SqlDataAdapter da;
        bool add, addlvl, editlvl, vdgvcol, edit;
        FunClass func = new FunClass();
        decimal OrgHgth = 0, OrgWdth = 0, NewHgth = 0, NewWdth = 0;                //----- Added by Sachin N. S. on 06/06/2013 for Bug-4524
        string _msgCaption = "";            //----- Added by Sachin N. S. on 06/06/2013 for Bug-4524
        int ApplPId = 0;
        string ApplCode, AppCaption, cAppPId;
        string cAppName = ""; //----- Added by Sachin N. S. on 08/07/2013 for Bug-4524

        clsDataAccess _oDataAccess;     //----- Added by Sachin N. S. on 08/03/2013 

        private string _DBName;

        public string DBName
        {
            get { return _DBName; }
        }

        public frmDadosReport()
        {
            InitializeComponent();
            this.OrgHgth = this.Height;     //----- Added by Sachin N. S. on 06/06/2013 for Bug-4524
            this.OrgWdth = this.Width;      //----- Added by Sachin N. S. on 06/06/2013 for Bug-4524
        }

        public frmDadosReport(string DBName)
        {
            InitializeComponent();
            _DBName = DBName;
            this.OrgHgth = this.Height;     //----- Added by Sachin N. S. on 06/06/2013 for Bug-4524
            this.OrgWdth = this.Width;      //----- Added by Sachin N. S. on 06/06/2013 for Bug-4524
        }

        //----- Added by Sachin N. S. on 08/03/2013 -- Start
        public frmDadosReport(string[] _args)
        {
            InitializeComponent();
            clsDataAccess._serverName = _args[2].ToString();
            clsDataAccess._databaseName = _args[1].ToString();
            clsDataAccess._userID = _args[3].ToString();
            clsDataAccess._password = _args[4].ToString();

            _msgCaption = _args[8].ToString().Replace("<*#*>", " ").ToString();

            this.Icon = new Icon(_args[7].Replace("<*#*>", " ").ToString());      //----- Added by Sachin N. S. on 26/06/2013 for Bug-4524
            ApplPId = Convert.ToInt16(_args[10]);
            ApplCode = _args[11].ToString();
            cAppName = _args[9];

            AppCaption = _args[8].ToString().Replace("<*#*>", " ").ToString();

            _DBName = _args[1].ToString();
            this.OrgHgth = this.Height;     //----- Added by Sachin N. S. on 06/06/2013 for Bug-4524
            this.OrgWdth = this.Width;      //----- Added by Sachin N. S. on 06/06/2013 for Bug-4524
        }
        //----- Added by Sachin N. S. on 08/03/2013 -- End

        private void frmDadosReport_Load(object sender, EventArgs e)
        {
            //----- Added by Sachin N. S. on 08/03/2013 -- Start
            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
            //----- Added by Sachin N. S. on 08/03/2013 -- End

            //con = func.getConnection();   //----- Commented by Sachin N. S. on 08/03/2013 
            //readIni = new ReadSetting(Application.StartupPath, DBName);   //----- Commented by Sachin N. S. on 21/06/2013 for Bug-4524

            _oDataAccess = new clsDataAccess();     //----- Added by Sachin N. S. on 08/03/2013 

            this.mInsertProcessIdRecord();      // Added by Sachin N. S. on 08/07/2013 for Bug-4524

            dgvCol.AutoGenerateColumns = false;
            //this.GetHeader("Last",""); 
            this.Navigate("Last");

            this.BindControlsParent();
            this.BindControlsChildlvl();
            this.BindControlsChildpara();

            dgvCol.Refresh();
            Enbldisbl(false);
            add = false;
            edit = false;

            #region Disable Form Close Button
            IntPtr hMenu = GetSystemMenu(this.Handle, false);
            int menuItemCount = GetMenuItemCount(hMenu);
            RemoveMenu(hMenu, menuItemCount - 1, MF_BYPOSITION);
            #endregion

            //***** Added by Sachin N. S. on 06/06/2013 for Bug-4524 -- Start *****//
            this.NewHgth = this.Height;
            this.NewWdth = this.Width;
            this.ResizeForm(this);
            //***** Added by Sachin N. S. on 06/06/2013 for Bug-4524 -- End *****//
        }

        #region "Private Methods"
        private void ResizeForm(Control _control)
        {
            foreach (Control _cntrl in _control.Controls)
            {
                if (_cntrl.GetType().ToString() != "Label")
                {
                    _cntrl.Height = (int)((this.NewHgth * _cntrl.Height) / this.OrgHgth);
                    _cntrl.Width = (int)((this.NewWdth * _cntrl.Width) / this.OrgWdth);
                    _cntrl.Top = (int)((this.NewHgth * _cntrl.Top) / this.OrgHgth);
                    _cntrl.Left = (int)((this.NewWdth * _cntrl.Left) / this.OrgWdth);
                }

                if (_cntrl.HasChildren == true)
                {
                    ResizeForm(_cntrl);
                }
            }
        }

        //private void GetHeader() //----- Changed by Sachin N. S. on 08/03/2013
        private void GetHeader(string _cond, string _repId, ref string _NaviBtn)
        {
            if (ds.Tables["rep_vw"] != null)
            {
                ds.Tables["rep_vw"].Clear();
            }

            //----- Changed by Sachin N. S. on 08/03/2013 -- Start
            //strrep = "select * from usrep order by repid";

            //***** Commented by Sachin N. S. on 03/04/2014 for Bug-4524 -- Start
            //switch (_cond)
            //{
            //    case "First":
            //        strrep = "select Top 1 * from usrep order by repid asc ";
            //        break;
            //    case "Previous":
            //        strrep = "select Top 1 * from usrep where repid< '" + _repId.ToString() + "' order by repid desc ";
            //        break;
            //    case "Next":
            //        strrep = "select Top 1 * from usrep where repid> '" + _repId.ToString() + "' order by repid asc ";
            //        break;
            //    case "Last":
            //        strrep = "select Top 1 * from usrep order by repid desc ";
            //        break;
            //    default:
            //        strrep = "select Top 1 * from usrep where repid='" + _repId.ToString() + "' order by repid desc ";
            //        break;
            //}
            //***** Commented by Sachin N. S. on 03/04/2014 for Bug-4524 -- End

            //***** Added by Sachin N. S. on 03/04/2014 for Bug-4524 -- Start
            DataSet _ds = new DataSet();
            strrep = "EXECUTE NAVIGATE_DADOS_WIZARD_DATA '" + _cond + "', '" + _repId.ToString() + "'";
            _ds = _oDataAccess.GetDataSet(strrep, null, 50);
            dt = _ds.Tables[0].Copy();
            _NaviBtn = _ds.Tables[1].Rows[0][0].ToString();
            //***** Added by Sachin N. S. on 03/04/2014 for Bug-4524 -- End

            //dt = _oDataAccess.GetDataTable(strrep, null, 50);     // Commented by Sachin N. S. on 03/04/2014 for Bug-4524
            dt.TableName = "rep_vw";
            if (ds.Tables["Rep_vw"] == null)
            { ds.Tables.Add(dt); }
            else
            { ds.Merge(dt); }
            //da = new SqlDataAdapter(strrep, con);
            //da.Fill(ds, "rep_vw");
            //----- Changed by Sachin N. S. on 08/03/2013 -- End
        }

        //----- Changed by Sachin N. S. on 08/03/2013 -- Start
        public void Navigate(string pos)
        {
            string repId = this.txtRepId.Text;
            string _naviBtn = "";

            if (ds.Tables["rep_vw"] != null)
            { ds.Tables["rep_vw"].Clear(); }

            switch (pos)
            {
                case "First":
                    this.GetHeader(pos, "", ref _naviBtn);
                    SetButtonVisibility(_naviBtn);      // Added by Sachin N. S. on 03/04/2014 for Bug-4524
                    break;
                case "Previous":
                    this.GetHeader(pos, repId, ref _naviBtn);
                    SetButtonVisibility(_naviBtn);      // Added by Sachin N. S. on 03/04/2014 for Bug-4524
                    break;
                case "Next":
                    this.GetHeader(pos, repId, ref _naviBtn);
                    SetButtonVisibility(_naviBtn);      // Added by Sachin N. S. on 03/04/2014 for Bug-4524
                    break;
                case "Last":
                    this.GetHeader(pos, "", ref _naviBtn);
                    SetButtonVisibility(_naviBtn);      // Added by Sachin N. S. on 03/04/2014 for Bug-4524
                    break;
                default:
                    this.GetHeader(pos, repId, ref _naviBtn);
                    SetButtonVisibility(_naviBtn);      // Added by Sachin N. S. on 03/04/2014 for Bug-4524
                    break;
            }

            if (ds.Tables["rep_vw"].Rows.Count > 0)
            {
                GetDetails("RepId = '" + ds.Tables["Rep_vw"].Rows[0]["RepId"].ToString() + "'");
                dgvCol.Refresh();
            }
            this.NavigateRepLvl("First");
            if (this.tabControl1.SelectedTab != tabPage1)
                this.tabControl1.SelectedTab = tabPage1;
            lvlNum = 1;
            lvlcount = ds.Tables["Replvl_Vw"].Rows.Count + 1;
            this.Refresh();
        }

        //public void Navigate(string pos)
        //{
        //    if (pos != "")
        //    {
        //        string filter = string.Empty;
        //    }
        //    int rows = ds.Tables["rep_vw"].Rows.Count;
        //    if (strRowId != "")
        //    {
        //        this.BindingContext[ds, "rep_vw"].Position = Convert.ToInt32(strRowId);
        //        strRowId = "";
        //    }

        //    switch (pos)
        //    {
        //        case "First":
        //            this.BindingContext[ds, "rep_vw"].Position = 0;

        //            if (this.BindingContext[ds, "rep_vw"].Position == 0)
        //            {
        //                SetButtonVisibility(false, false, true, true, rows);
        //            }
        //            break;
        //        case "Previous":
        //            this.BindingContext[ds, "rep_vw"].Position = this.BindingContext[ds, "rep_vw"].Position - 1;

        //            if (this.BindingContext[ds, "rep_vw"].Position == 0)
        //            {
        //                SetButtonVisibility(false, false, true, true, rows);
        //            }
        //            else
        //            {
        //                SetButtonVisibility(true, true, true, true, rows);
        //            }

        //            break;
        //        case "Next":
        //            this.BindingContext[ds, "rep_vw"].Position = this.BindingContext[ds, "rep_vw"].Position + 1;

        //            if (this.BindingContext[ds, "rep_vw"].Position + 1 == this.BindingContext[ds, "rep_vw"].Count)
        //            {
        //                SetButtonVisibility(true, true, false, false, rows);
        //            }
        //            else
        //            {
        //                SetButtonVisibility(true, true, true, true, rows);
        //            }
        //            break;
        //        case "Last":
        //            this.BindingContext[ds, "rep_vw"].Position = this.BindingContext[ds, "rep_vw"].Count;
        //            if (this.BindingContext[ds, "rep_vw"].Count == 1)
        //            {
        //                SetButtonVisibility(true, true, false, false, rows);
        //                break;
        //            }
        //            if (this.BindingContext[ds, "rep_vw"].Count > 1)
        //            {
        //                SetButtonVisibility(true, true, false, false, rows);
        //            }
        //            else
        //            {
        //                SetButtonVisibility(false, false, false, false, rows);
        //            }
        //            break;
        //        default:

        //            if (this.BindingContext[ds, "rep_vw"].Count >= 1)
        //            {
        //                if (this.BindingContext[ds, "rep_vw"].Position == 0)
        //                {
        //                    SetButtonVisibility(false, false, true, true, rows);
        //                }
        //                if (this.BindingContext[ds, "rep_vw"].Position + 1 == this.BindingContext[ds, "rep_vw"].Count)
        //                {
        //                    SetButtonVisibility(true, true, false, false, rows);
        //                }
        //                else
        //                {
        //                    if (this.BindingContext[ds, "rep_vw"].Position < this.BindingContext[ds, "rep_vw"].Count && (this.BindingContext[ds, "rep_vw"].Position != 0))
        //                    {
        //                        SetButtonVisibility(true, true, true, true, rows);
        //                    }
        //                }
        //                if (this.BindingContext[ds, "rep_vw"].Position == this.BindingContext[ds, "rep_vw"].Count)
        //                {
        //                    SetButtonVisibility(true, true, false, false, rows);
        //                }
        //            }
        //            else
        //            {
        //                SetButtonVisibility(false, false, false, false, rows);
        //            }
        //            break;
        //    }

        //    if (this.BindingContext[ds, "rep_vw"].Count > 0)
        //    {
        //        filter = "Repid='" + (ds.Tables["rep_vw"].Rows[this.BindingContext[ds, "rep_vw"].Position]["Repid"]).ToString() + "'";
        //        ds.Tables["rep_vw"].DefaultView.RowFilter = string.Empty;
        //        ds.Tables["rep_vw"].DefaultView.RowFilter = filter;
        //        GetDetails(filter);
        //        dgvCol.Refresh();
        //    }
        //    this.NavigateRepLvl("First");
        //    this.Refresh();
        //}
        //----- Changed by Sachin N. S. on 08/03/2013 -- End

        private void BindControlsParent()
        {
            this.txtRepId.DataBindings.Add("Text", ds, "rep_vw.repid");
            this.txtRepNm.DataBindings.Add("Text", ds, "rep_vw.repnm");
            this.txtRepTy.DataBindings.Add("Text", ds, "rep_vw.repty");
        }

        //----- Changed by Sachin N. S. on 08/03/2013 -- Start
        private void GetDetails(string inc)
        {
            try
            {
                if (ds.Tables["para_vw"] != null)
                { ds.Tables["para_vw"].Clear(); }
                if (ds.Tables["replvl_vw"] != null)
                { ds.Tables["replvl_vw"].Clear(); }
                if (ds.Tables["para_vw"] != null)
                { ds.Tables["para_vw"].Clear(); }
                if (ds.Tables["col_vw"] != null)
                { ds.Tables["col_vw"].Clear(); }

                if (ds.Tables["rep_vw"].Rows.Count > 0)
                {
                    strpara = "select pm.parameterID,pm.ParaCaption,pm.ParamName,pm.QueryId,pqm.Para_order,pqm.repid,pqm.paramvalue,qry.repqry,pm.paramtype from Para_master pm Inner Join Para_query_master pqm on (pqm.parameterId=pm.parameterId) Inner Join usrep rp on(rp.repid=pqm.repid) left Join usqry qry on(qry.qryid=pm.queryid) where pqm." + inc + "";
                    dt = _oDataAccess.GetDataTable(strpara, null, 50);
                    dt.TableName = "para_vw";
                    if (ds.Tables["para_vw"] == null)
                    { ds.Tables.Add(dt); }
                    else
                    { ds.Merge(dt); }

                    strrlv = "select qry.replvlid,lty.lvlty,lty.lvlnm,qry.repqry,qry.qryid,rlv.prycl,rlv.seccl,qry.repid From Uslty lty Inner Join usrlv rlv on (rlv.lvlid=lty.lvltid) Inner join usqry qry on (qry.qryid=rlv.qryid) where qry." + inc + " order by qry.RepLvlId";
                    dt = _oDataAccess.GetDataTable(strrlv, null, 50);
                    dt.TableName = "replvl_vw";
                    if (ds.Tables["replvl_vw"] == null)
                    { ds.Tables.Add(dt); }
                    else
                    { ds.Merge(dt); }

                    strcol = "select usrl.colid,usc.columnnames,usc.columncaption,usc.columndatatype,usc.columnorder,usrl.isgrouped,usrl.isfreezing,usrl.colwidth,usrl.isdisplayed,usrl.grouporder,usrl.issummury,usrl.qryid,prycl=(case when usc.columnnames=rlv.prycl then rlv.prycl else '' end ),seccl=(case when usc.columnnames=rlv.prycl then 1 else 0 end ),usc.precision,usrl.repid, usrl.QryId, usrl.ReplvlId from uscol usc inner join uscrl usrl on(usc.colid=usrl.colid) left join usrlv rlv on(rlv.qryid=usrl.qryid) where usrl." + inc + "";
                    dt = _oDataAccess.GetDataTable(strcol, null, 50);
                    dt.TableName = "col_vw";
                    if (ds.Tables["col_vw"] == null)
                    { ds.Tables.Add(dt); }
                    else
                    { ds.Merge(dt); }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //private void GetDetails(string inc)
        //{
        //    try
        //    {
        //        if (ds.Tables["para_vw"] != null)
        //        { ds.Tables["para_vw"].Clear(); }
        //        if (ds.Tables["replvl_vw"] != null)
        //        { ds.Tables["replvl_vw"].Clear(); }
        //        if (ds.Tables["para_vw"] != null)
        //        { ds.Tables["para_vw"].Clear(); }
        //        if (ds.Tables["col_vw"] != null)
        //        { ds.Tables["col_vw"].Clear(); }

        //        if (ds.Tables["rep_vw"].Rows.Count > 0)
        //        {
        //            strpara = "select pm.parameterID,pm.ParaCaption,pm.QueryId,pqm.DisplayOrder,pqm.repid,pqm.paramvalue,qry.repqry,pm.paramtype,pm.ParamName from Para_master pm Inner Join Para_query_master pqm on (pqm.parameterId=pm.parameterId) Inner Join usrep rp on(rp.repid=pqm.repid) left Join usqry qry on(qry.qryid=pm.queryid) where pqm." + inc + "";
        //            da = new SqlDataAdapter(strpara, con);
        //            da.Fill(ds, "para_vw");
        //            strrlv = "select qry.replvlid,lty.lvlty,lty.lvlnm,qry.repqry,qry.qryid,rlv.prycl,rlv.seccl,qry.repid From Uslty lty Inner Join usrlv rlv on (rlv.lvlid=lty.lvltid) Inner join usqry qry on (qry.qryid=rlv.qryid) where qry." + inc + " order by qry.RepLvlId";
        //            da = new SqlDataAdapter(strrlv, con);
        //            da.Fill(ds, "replvl_vw");
        //            strcol = "select usrl.colid,usc.columnnames,usc.columncaption,usc.columndatatype,usc.columnorder,usrl.isgrouped,usrl.isfreezing,usrl.colwidth,usrl.isdisplayed,usrl.grouporder,usrl.issummury,usrl.qryid,prycl=(case when usc.columnnames=rlv.prycl then rlv.prycl else '' end ),seccl=(case when usc.columnnames=rlv.prycl then 1 else 0 end ),usc.precision,usrl.repid from uscol usc inner join uscrl usrl on(usc.colid=usrl.colid) left join usrlv rlv on(rlv.qryid=usrl.qryid) where usrl." + inc + "";
        //            da = new SqlDataAdapter(strcol, con);
        //            da.Fill(ds, "col_vw");
        //            con.Close();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}
        //----- Changed by Sachin N. S. on 08/03/2013 -- End

        private void AutoLvl(DataTable _dtRepLevel)
        {
            // Changed By Sachin N. S. on 09/03/2013 -- Start
            //if (ds.Tables["lvl"] != null)
            //{
            //    ds.Tables["lvl"].Clear();
            //}
            strqry = "Select max(lvltid) as lvltid from uslty";
            dr1 = _oDataAccess.GetDataRow(strqry, null, 50);


            //da = new SqlDataAdapter(strqry, con);
            //da.Fill(ds, "lvl");
            //dr1 = ds.Tables["lvl"].Rows[0];
            if (_dtRepLevel.Rows.Count > 0)
                txtQryLvl.Text = ((int.Parse(_dtRepLevel.Compute("max(replvlid)", string.Empty).ToString()) > int.Parse(dr1.ItemArray.GetValue(0).ToString()) ? int.Parse(_dtRepLevel.Compute("max(RepLvlid)", string.Empty).ToString()) : int.Parse(dr1.ItemArray.GetValue(0).ToString())) + 1).ToString();
            else
                txtQryLvl.Text = (int.Parse(dr1.ItemArray.GetValue(0).ToString()) + 1).ToString();
            // Changed By Sachin N. S. on 09/03/2013 -- End
        }

        private int AutoQry(DataTable _dtqryid)
        {
            // Changed By Sachin N. S. on 09/03/2013 -- Start
            //if (ds.Tables["qry"] != null)
            //{
            //    ds.Tables["qry"].Clear();
            //}
            strqry = "Select max(qryid)as qryid from usqry";
            drqry = _oDataAccess.GetDataRow(strqry, null, 50);

            //da = new SqlDataAdapter(strqry, con);
            //da.Fill(ds, "qry");
            //drqry = ds.Tables["qry"].Rows[0];

            if (_dtqryid.Rows.Count > 0)
                maxQryid = (int.Parse(_dtqryid.Compute("max(qryid)", string.Empty).ToString()) > int.Parse(drqry.ItemArray.GetValue(0).ToString()) ? int.Parse(_dtqryid.Compute("max(qryid)", string.Empty).ToString()) : int.Parse(drqry.ItemArray.GetValue(0).ToString())) + 1;
            else
                maxQryid = int.Parse(drqry.ItemArray.GetValue(0).ToString()) + 1;
            // Changed By Sachin N. S. on 09/03/2013 -- End
            return maxQryid;
        }

        private int AutoCol(DataTable _dtcolid)
        {
            // Changed By Sachin N. S. on 09/03/2013 -- Start
            //if (ds.Tables["crl"] != null)
            //{
            //    ds.Tables["crl"].Clear();
            //}

            strqry = "Select max(colid)as colid from uscrl";
            drcol = _oDataAccess.GetDataRow(strqry, null, 50);

            //da = new SqlDataAdapter(strqry, con);
            //da.Fill(ds, "crl");
            //drcol = ds.Tables["crl"].Rows[0];

            if (_dtcolid.Rows.Count > 0)
                maxColid = (int.Parse(_dtcolid.Compute("max(colid)", string.Empty).ToString()) > int.Parse(drcol.ItemArray.GetValue(0).ToString()) ? int.Parse(_dtcolid.Compute("max(colid)", string.Empty).ToString()) : int.Parse(drcol.ItemArray.GetValue(0).ToString())) + 1;
            else
                maxColid = int.Parse(drcol.ItemArray.GetValue(0).ToString()) + 1;
            return maxColid;
            // Changed By Sachin N. S. on 09/03/2013 -- End
        }

        private void AutoId()
        {
            string cSql;    // Added by Sachin N. S. on 09/03/2013
            //dr = new DataRow();
            cSql = "select max(cast(substring(repid,2,len(repid))as int)+1) from usrep";
            dr = _oDataAccess.GetDataRow(cSql, null, 50);

            //da = new SqlDataAdapter("select max(cast(substring(repid,2,len(repid))as int)+1) from usrep", con);
            //DataTable dt = new DataTable();
            //da.Fill(dt);
            txtRepId.Text = (dr[0].ToString().PadLeft(6, '0'));
        }

        private void clear()
        {
            Control[] ctrlclr = { txtRepId, txtRepNm, rtbsqlqry };
            for (int i = 0; i < ctrlclr.Length; i++)
                ctrlclr[i].Text = "";
            ds.Tables["para_vw"].Clear();
            ds.Tables["col_vw"].Clear();
            ds.Tables["replvl_vw"].Clear();
            ds.Tables["rep_vw"].Clear();
            dgvParaRep.Refresh();
            dgvCol.Refresh();
            txtRepTy.Text = "View";
            txtRepId.Focus();
            txtLvlNm.ReadOnly = true;
            rtbQry.ReadOnly = true;
            dgvCol.ReadOnly = true;
        }

        private void clearlvl()
        {
            Control[] ctrlTxtNull = { txtLvlNm, txtLvlTyp, txtQryLvl, rtbQry };
            for (int i = 0; i < ctrlTxtNull.Length; i++)
                ctrlTxtNull[i].Text = "";
        }

        private void ctrlBindclr()
        {
            Control[] ctrbndclrl = { txtRepId, txtRepNm, txtRepTy };
            for (int i = 0; i < ctrbndclrl.Length; i++)
                ctrbndclrl[i].DataBindings.Clear();
        }

        private void ctrlBindclrChild()
        {
            Control[] ctrbndclrlchld = { txtQryLvl, txtLvlNm, txtLvlTyp, rtbQry };
            for (int i = 0; i < ctrbndclrlchld.Length; i++)
                ctrbndclrlchld[i].DataBindings.Clear();
        }

        private void BindControlsChildpara()
        {
            this.dgvParaRep.DataSource = ds.Tables["para_vw"];
            dgvParaRep.Columns[0].Visible = false;
            dgvParaRep.Columns[5].Visible = false;
            dgvParaRep.Columns[6].Visible = false;
            dgvParaRep.Columns[7].Visible = false;
            dgvParaRep.Columns[8].Visible = false;

            dgvParaRep.Columns[1].ReadOnly = true;
            dgvParaRep.Columns[2].ReadOnly = true;
            dgvParaRep.Columns[3].ReadOnly = true;

            dgvParaRep.Columns[1].HeaderText = "Parameter Caption";
            dgvParaRep.Columns[1].Width = 500;
            dgvParaRep.Columns[2].HeaderText = "Parameter Field";
            dgvParaRep.Columns[2].Width = 500;
            dgvParaRep.Columns[3].HeaderText = "Query Id";
            dgvParaRep.Columns[3].Width = 100;
            dgvParaRep.Columns[4].HeaderText = "Display Order";
            dgvParaRep.Columns[4].Width = 100;
        }

        private void BindControlsChildlvl()
        {
            txtQryLvl.DataBindings.Add("text", ds, "replvl_vw.replvlid");
            txtLvlNm.DataBindings.Add("text", ds, "replvl_vw.lvlnm");
            txtLvlTyp.DataBindings.Add("text", ds, "replvl_vw.lvlty");
            rtbQry.DataBindings.Add("text", ds, "replvl_vw.repqry");
            dgvCol.DataSource = ds.Tables["col_vw"];
            dgvCol.Columns[0].ReadOnly = true;
            dgvCol.Columns[0].DataPropertyName = "columnnames";
            dgvCol.Columns[1].DataPropertyName = "columncaption";
            dgvCol.Columns[2].DataPropertyName = "columnDataType";
            dgvCol.Columns[3].DataPropertyName = "columnorder";
            dgvCol.Columns[4].DataPropertyName = "isdisplayed";
            dgvCol.Columns[5].DataPropertyName = "isfreezing";
            dgvCol.Columns[6].DataPropertyName = "colwidth";
            dgvCol.Columns[7].DataPropertyName = "isgrouped";
            dgvCol.Columns[8].DataPropertyName = "grouporder";
            dgvCol.Columns[9].DataPropertyName = "issummury";
            dgvCol.Columns[10].DataPropertyName = "prycl";
            dgvCol.Columns[11].DataPropertyName = "seccl";
            dgvCol.Columns[12].DataPropertyName = "Precision";
            dgvCol.Columns[13].DataPropertyName = "colid";
            dgvCol.Columns[13].Visible = false;
        }

        private void Enbldisbl(Boolean _Enbldisbl)
        {
            grpLvl.Enabled = btnSave.Enabled = btnCancel.Enabled = txtRepNm.Enabled = cmdRepTy.Enabled = cmdAddParam.Enabled = cmdRemoveParam.Enabled = _Enbldisbl;
            btnNew.Enabled = btnEdit.Enabled = btnDelete.Enabled = rtbQry.ReadOnly = dgvCol.ReadOnly = dgvParaRep.Columns[4].ReadOnly = dgvCol.ReadOnly = cmdReportNm.Enabled = txtRepNm.ReadOnly = !_Enbldisbl;    // Changed by Sachin N. S. on 07/08/2013 for Bug-4524
            txtRepId.Enabled = txtRepTy.Enabled = txtLvlTyp.Enabled = txtQryLvl.Enabled = cmdGenerate.Enabled = false;
            cmdAddlvl.Enabled = cmdRemovelvl.Enabled = cmdEditlvl.Enabled = _Enbldisbl;
            cmdGenScript.Enabled = !_Enbldisbl;

            if (add == true)
                cmdEditlvl.Enabled = false;
            //cmdFirst.Enabled = cmdPrev.Enabled = cmdPrev.Enabled = cmdNext.Enabled = cmdLast.Enabled // Changed by Sachin N. S. on 06/06/2013 for Bug-4524
            cmdParacap.Visible = false;
            cboPk.Visible = false;
        }

        public void NavigateRepLvl(string pos)
        {
            string filter1 = string.Empty;
            string filter2 = string.Empty;
            int rows = ds.Tables["replvl_vw"].Rows.Count;
            switch (pos)
            {
                case "First":
                    this.BindingContext[ds, "replvl_vw"].Position = 0;
                    if (this.BindingContext[ds, "replvl_vw"].Position == 0)
                    {
                        SetButtonVisibility1(false, false, true, true, rows);
                    }
                    lvlNum = 1;
                    break;
                case "Previous":
                    this.BindingContext[ds, "replvl_vw"].Position = this.BindingContext[ds, "replvl_vw"].Position - 1;
                    if (this.BindingContext[ds, "replvl_vw"].Position == 0)
                    {
                        SetButtonVisibility1(false, false, true, true, rows);
                    }
                    else
                    {
                        SetButtonVisibility1(true, true, true, true, rows);
                    }
                    lvlNum = lvlNum == 1 ? 1 : lvlNum - 1;
                    break;
                case "Next":
                    this.BindingContext[ds, "replvl_vw"].Position = this.BindingContext[ds, "replvl_vw"].Position + 1;
                    if (this.BindingContext[ds, "replvl_vw"].Position + 1 == this.BindingContext[ds, "replvl_vw"].Count)
                    {
                        SetButtonVisibility1(true, true, false, false, rows);
                    }
                    else
                    {
                        SetButtonVisibility1(true, true, true, true, rows);
                    }
                    lvlNum += 1;
                    break;
                case "Last":
                    this.BindingContext[ds, "replvl_vw"].Position = this.BindingContext[ds, "replvl_vw"].Count;
                    if (this.BindingContext[ds, "replvl_vw"].Count == 1)
                    {
                        SetButtonVisibility1(true, true, false, false, rows);
                        break;
                    }
                    if (this.BindingContext[ds, "replvl_vw"].Count > 1)
                    {
                        SetButtonVisibility1(true, true, false, false, rows);
                    }
                    else
                    {
                        SetButtonVisibility1(false, false, false, false, rows);
                    }
                    lvlNum = ds.Tables["RepLvl_Vw"].Rows.Count + 1;
                    break;
                default:
                    if (this.BindingContext[ds, "replvl_vw"].Count >= 1)
                    {
                        if (this.BindingContext[ds, "replvl_vw"].Position == 0)
                        {
                            SetButtonVisibility1(false, false, true, true, rows);
                        }
                        if (this.BindingContext[ds, "replvl_vw"].Position + 1 == this.BindingContext[ds, "replvl_vw"].Count)
                        {
                            SetButtonVisibility1(true, true, false, false, rows);
                        }
                        else
                        {
                            if (this.BindingContext[ds, "replvl_vw"].Position < this.BindingContext[ds, "replvl_vw"].Count && (this.BindingContext[ds, "replvl_vw"].Position != 0))
                            {
                                SetButtonVisibility1(true, true, true, true, rows);
                            }
                        }
                        if (this.BindingContext[ds, "replvl_vw"].Position == this.BindingContext[ds, "replvl_vw"].Count)
                        {
                            SetButtonVisibility1(true, true, false, false, rows);
                        }
                    }
                    else
                    {
                        SetButtonVisibility1(false, false, false, false, rows);
                    }
                    lvlNum = 1;
                    break;
            }
            if (this.BindingContext[ds, "replvl_vw"].Count > 0)
            {
                filter1 = "qryid='" + (ds.Tables["replvl_vw"].Rows[this.BindingContext[ds, "replvl_vw"].Position]["qryid"]).ToString() + "'";
                ds.Tables["replvl_vw"].DefaultView.RowFilter = string.Empty;
                ds.Tables["replvl_vw"].DefaultView.RowFilter = filter1;
                filter2 = "qryid='" + (ds.Tables["replvl_vw"].Rows[this.BindingContext[ds, "replvl_vw"].Position]["qryid"]).ToString() + "'";
                ds.Tables["col_vw"].DefaultView.RowFilter = string.Empty;
                ds.Tables["col_vw"].DefaultView.RowFilter = filter2;
            }
            this.Refresh();
        }

        private void SetButtonVisibility(bool FirstButton, bool PreviousButton, bool NextButton, bool LastButton, int intRowCount)
        {
            if (intRowCount > 1)
            {
                this.btnFirst.Enabled = FirstButton;
                this.btnBack.Enabled = PreviousButton;
                this.btnForward.Enabled = NextButton;
                this.btnLast.Enabled = LastButton;
            }
            else
            {
                this.btnFirst.Enabled = false;
                this.btnBack.Enabled = false;
                this.btnForward.Enabled = false;
                this.btnLast.Enabled = false;
            }
        }

        private void SetButtonVisibility(string _sNaviBtn)
        {
            this.btnFirst.Enabled = _sNaviBtn.Substring(0, 1) == "1" ? true : false;
            this.btnBack.Enabled = _sNaviBtn.Substring(1, 1) == "1" ? true : false;
            this.btnForward.Enabled = _sNaviBtn.Substring(2, 1) == "1" ? true : false;
            this.btnLast.Enabled = _sNaviBtn.Substring(3, 1) == "1" ? true : false;
        }

        private void SetButtonVisibility1(bool FirstButton, bool PreviousButton, bool NextButton, bool LastButton, int intRowCount)
        {
            if (intRowCount > 1)
            {
                this.cmdFirst.Enabled = FirstButton;
                this.cmdPrev.Enabled = PreviousButton;
                this.cmdNext.Enabled = NextButton;
                this.cmdLast.Enabled = LastButton;
            }
            else
            {
                this.cmdFirst.Enabled = false;
                this.cmdPrev.Enabled = false;
                this.cmdNext.Enabled = false;
                this.cmdLast.Enabled = false;
            }

        }

        private void refesh_rtbsqlqry()
        {
            //if (add == false && edit==false)  // Commented by Sachin N. S. on 07/08/2013 for Bug-4524
            //{                                 // Commented by Sachin N. S. on 07/08/2013 for Bug-4524
            //if ((int)dgvParaRep.CurrentRow.Cells["queryid"].Value == 0)
            if (dgvParaRep.CurrentRow != null && dgvParaRep.CurrentRow.Cells["queryid"].Value != null && dgvParaRep.CurrentRow.Cells["queryid"].Value.ToString() != "")     // Added by Sachin N. S. on 21/06/2013 for Bug-4524
            {
                if (Convert.ToInt16(dgvParaRep.CurrentRow.Cells["queryid"].Value) == 0)
                {
                    rtbsqlqry.Text = "";
                    rtbsqlqry.Refresh();
                }
                else
                {
                    DataRow[] drqry = ds.Tables["para_vw"].Select("parameterID='" + dgvParaRep.CurrentRow.Cells["ParameterId"].Value.ToString() + "'");
                    //rtbsqlqry.Text = drqry[0].ItemArray.GetValue(6).ToString();   // Changed by Sachin N. S. on 21/06/2013 for Bug-4524
                    if (drqry.Count() > 0)
                    {
                        rtbsqlqry.Text = drqry[0]["RepQry"].ToString();
                        rtbsqlqry.Refresh();
                    }
                }
            }
            //}     // Commented by Sachin N. S. on 07/08/2013 for Bug-4524
        }

        private Type Contains(string p)
        {
            throw new NotImplementedException();
        }

        #endregion "Private Methods"

        #region "Main Tool bar Events"

        private void btnFirst_Click(object sender, EventArgs e)
        {
            this.Navigate("First");
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Navigate("Previous");
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            this.Navigate("Next");
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            this.Navigate("Last");
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            add = true;
            Enbldisbl(true);
            SetButtonVisibility(false, false, false, false, 0);     // Added by Sachin N. S. on 02/062014 for Bug-4524
            clear();
            clearlvl();
            ctrlBindclr();
            AutoId();
            txtRepNm.Focus();
            lvlcount = 0;
            lvlNum = 0;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            Enbldisbl(true);
            SetButtonVisibility(false, false, false, false, 0);     // Added by Sachin N. S. on 02/062014 for Bug-4524
            edit = true;
            txtRepId.Enabled = false;
            //cmdAddlvl.Text = "&Edit Level";
            this.ctrlBindclr();
            txtLvlNm.ReadOnly = true;
            rtbQry.ReadOnly = true;
            dgvCol.ReadOnly = true;
            txtRepNm.ReadOnly = true;
            //cmdParacap.Visible = true;        //Commented by Shrikant S. on 27/04/2015 for Bug-25963
            cmdParacap.Visible = false;         //Added by Shrikant S. on 27/04/2015 for Bug-25963
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult x = MessageBox.Show("Do you really want to delete the record?", _msgCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (x == DialogResult.Yes)
                {
                    int _retVal = 0;    // Added by Sachin N. S. on 05/06/2013 for Bug-4524
                    //***** Changed by Sachin N. S. on 05/06/2013 for Bug-4524 -- Start *****//
                    //con = func.getConnection();
                    //string deleteSql = "delete from para_query_master where repid=" + txtRepId.Text;
                    //func.GetRecord(deleteSql, con);
                    //string deleteSql1 = "delete from uslty where lvltid in(select lvlid from usrlv where qryid in(select qryid from usqry where repid=" + txtRepId.Text + "))";
                    //func.GetRecord(deleteSql1, con);
                    //string deleteSql2 = "delete from usrlv where qryid in(select qryid from usqry where repid=" + txtRepId.Text + ")";
                    //func.GetRecord(deleteSql2, con);
                    //string deleteSql3 = "delete from usqry where repid=" + txtRepId.Text;
                    //func.GetRecord(deleteSql3, con);
                    //string deleteSql4 = "delete from uscol where colid in(select colid from uscrl where repid=" + txtRepId.Text + ")";
                    //func.GetRecord(deleteSql4, con);
                    //string deleteSql5 = "delete from uscrl where repid=" + txtRepId.Text;
                    //func.GetRecord(deleteSql5, con);
                    //string deleteSql6 = "delete from usrep where repid=" + txtRepId.Text;
                    //func.GetRecord(deleteSql6, con);

                    _oDataAccess.BeginTransaction();
                    string deleteSql = "delete from para_query_master where repid=" + txtRepId.Text;
                    _retVal = _oDataAccess.ExecuteSQLStatement(deleteSql, null, 20, true);
                    if (_retVal > 0)
                    {
                        deleteSql = "delete from usrlv where qryid in(select qryid from usqry where repid=" + txtRepId.Text + ")";
                        _retVal = _oDataAccess.ExecuteSQLStatement(deleteSql, null, 20, true);
                    }
                    if (_retVal > 0)
                    {
                        deleteSql = "delete from uslty where lvltid in(select replvlid from usqry where repid=" + txtRepId.Text + ")";
                        _retVal = _oDataAccess.ExecuteSQLStatement(deleteSql, null, 20, true);
                    }
                    if (_retVal > 0)
                    {
                        deleteSql = "delete from uscol where colid in(select colid from uscrl where repid=" + txtRepId.Text + ")";
                        _retVal = _oDataAccess.ExecuteSQLStatement(deleteSql, null, 20, true);
                    }
                    if (_retVal > 0)
                    {
                        deleteSql = "delete from uscrl where repid=" + txtRepId.Text;
                        _retVal = _oDataAccess.ExecuteSQLStatement(deleteSql, null, 20, true);
                    }
                    if (_retVal > 0)
                    {
                        deleteSql = "delete from usqry where repid=" + txtRepId.Text;
                        _retVal = _oDataAccess.ExecuteSQLStatement(deleteSql, null, 20, true);
                    }
                    if (_retVal > 0)
                    {
                        deleteSql = "delete from usrep where repid=" + txtRepId.Text;
                        _retVal = _oDataAccess.ExecuteSQLStatement(deleteSql, null, 20, true);
                    }
                    if (_retVal > 0)
                    {
                        _oDataAccess.CommitTransaction();
                        MessageBox.Show("Record deleted successfully", _msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                    else
                    {
                        _oDataAccess.RollbackTransaction();
                        MessageBox.Show("Cannot delete the record due to some problem.", _msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                    //***** Changed by Sachin N. S. on 05/06/2013 for Bug-4524 -- End *****//

                    Enbldisbl(false);
                    ctrlBindclr();
                    //***** Changed by Sachin N. S. on 05/06/2013 for Bug-4524 -- Start *****//
                    //if (ds.Tables["rep_vw"].Rows.Count > 0)
                    //{
                    //    DataRow[] row = ds.Tables["rep_vw"].Select("RepId=" + txtRepId.Text);
                    //    ds.Tables["rep_vw"].Rows.Remove(row[0]);
                    //}
                    //if (ds.Tables["para_vw"].Rows.Count > 0)
                    //{
                    //    DataRow[] row2 = ds.Tables["para_vw"].Select("RepId=" + txtRepId.Text);
                    //    ds.Tables["para_vw"].Rows.Remove(row2[0]);
                    //}
                    //if (ds.Tables["replvl_vw"].Rows.Count > 0)
                    //{
                    //    DataRow[] row3 = ds.Tables["replvl_vw"].Select("RepId=" + txtRepId.Text);
                    //    ds.Tables["replvl_vw"].Rows.Remove(row3[0]);
                    //}
                    //if (ds.Tables["col_vw"].Rows.Count > 0)
                    //{
                    //    DataRow[] row4 = ds.Tables["col_vw"].Select("RepId=" + txtRepId.Text);
                    //    ds.Tables["col_vw"].Rows.Remove(row4[0]);
                    //}

                    if (ds.Tables["rep_vw"].Rows.Count > 0)
                    {
                        ds.Tables["rep_vw"].Clear();
                    }
                    if (ds.Tables["para_vw"].Rows.Count > 0)
                    {
                        ds.Tables["para_vw"].Clear();
                    }
                    if (ds.Tables["replvl_vw"].Rows.Count > 0)
                    {
                        ds.Tables["replvl_vw"].Clear();
                    }
                    if (ds.Tables["col_vw"].Rows.Count > 0)
                    {
                        ds.Tables["col_vw"].Clear();
                    }
                    string _navBtn = "";
                    //***** Changed by Sachin N. S. on 05/06/2013 for Bug-4524 -- End *****//

                    this.BindControlsParent();
                    ds.Tables["rep_vw"].AcceptChanges();
                    ds.Tables["para_vw"].AcceptChanges();
                    ds.Tables["replvl_vw"].AcceptChanges();
                    ds.Tables["col_vw"].AcceptChanges();
                    GetHeader("Last", "", ref _navBtn);   // ------- Changed By Sachin N. S. on 08/03/2013
                    this.Navigate("Last");
                    this.Refresh();
                    add = false;
                    //con.Close();      // Commented by Sachin N. S. on 06/06/2013 for Bug-4524
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, _msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //con = func.getConnection();   // Commented by Sachin N. S. on 05/06/2013 for Bug-4524
            if (txtRepNm.Text != "" && cmdAddlvl.Text != "S&ave Level")
            {
                if (add == true)
                {
                    //con = func.getConnection();     // Commented by Sachin N. S. on 05/06/2013 for Bug-4524
                    int _nretVal = 0;     // Added by Sachin N. S. on 05/06/2013 for Bug-4524
                    rep = ds.Tables["rep_vw"].NewRow();
                    rep["repid"] = txtRepId.Text;
                    rep["repnm"] = txtRepNm.Text;
                    rep["repty"] = txtRepTy.Text;
                    ds.Tables["rep_vw"].Rows.Add(rep);
                    string strqr = "Insert into usrep values('" + ds.Tables["rep_vw"].Rows[0]["repid"] + "','" + ds.Tables["rep_vw"].Rows[0]["repnm"] + "','" + ds.Tables["rep_vw"].Rows[0]["repty"] + "')";
                    _oDataAccess.BeginTransaction();

                    _nretVal = _oDataAccess.ExecuteSQLStatement(strqr, null, 20, true);
                    //func.GetRecord(strqr, con);       // Commented by Sachin N. S. on 05/06/2013 for Bug-4524
                    if (ds.Tables["replvl_vw"].Rows.Count > 0)
                    {
                        if (_nretVal > 0)   // Added by Sachin N. S. on 05/06/2013 for Bug-4524
                        {
                            for (int i = 0; i < dgvParaRep.Rows.Count; i++)
                            {
                                if (dgvParaRep.Rows[i].Cells["ParameterID"].Value == null)
                                    strParaid = Convert.ToString(DBNull.Value);
                                else
                                    strParaid = dgvParaRep.Rows[i].Cells["ParameterID"].Value.ToString();
                                if (dgvParaRep.Rows[i].Cells["QueryId"].Value == null)
                                    strQryid = Convert.ToString(DBNull.Value);
                                else
                                    strQryid = dgvParaRep.Rows[i].Cells["QueryId"].Value.ToString();
                                if (dgvParaRep.Rows[i].Cells["Para_Order"].Value == null)
                                    strDisor = Convert.ToString(DBNull.Value);
                                else
                                    strDisor = dgvParaRep.Rows[i].Cells["Para_Order"].Value.ToString();
                                if (dgvParaRep.Rows[i].Cells["ParamType"].Value == null)
                                    paratype = Convert.ToString(DBNull.Value);
                                else
                                    paratype = dgvParaRep.Rows[i].Cells["ParamType"].Value.ToString();
                                if (dgvParaRep.Rows[i].Cells["Paramvalue"].Value == null)
                                    strparamvalue = Convert.ToString(DBNull.Value);
                                else
                                    strparamvalue = dgvParaRep.Rows[i].Cells["Paramvalue"].Value.ToString();
                                string strqr1 = "Insert into para_query_master(parameterid,queryid,paramvalue,repid,Para_Order) values('" + strParaid + "','" + strQryid + "','" + strparamvalue + "','" + txtRepId.Text + "'," + strDisor + ")";
                                //func.GetRecord(strqr1, con);        // Commented by Sachin N. S. on 05/06/2013 for Bug-4524
                                _nretVal = _oDataAccess.ExecuteSQLStatement(strqr1, null, 20, true);
                            }
                        }
                        if (_nretVal > 0)   // Added by Sachin N. S. on 05/06/2013 for Bug-4524
                        {
                            for (int i = 0; i < ds.Tables["replvl_vw"].Rows.Count; i++)
                            {
                                string strqr2 = "Insert into usqry values(" + ds.Tables["replvl_vw"].Rows[i]["qryid"] + ",'" + txtRepId.Text + "'," + ds.Tables["replvl_vw"].Rows[i]["replvlid"] + ",'" + ds.Tables["replvl_vw"].Rows[i]["repqry"] + "')";
                                _nretVal = _oDataAccess.ExecuteSQLStatement(strqr2, null, 20, true);
                                //func.GetRecord(strqr2, con);        // Commented by Sachin N. S. on 05/06/2013 for Bug-4524
                                if (_nretVal > 0)
                                {
                                    string strqr4 = "Insert into uslty values('" + ds.Tables["replvl_vw"].Rows[i]["lvlty"] + "','" + ds.Tables["replvl_vw"].Rows[i]["lvlnm"] + "'," + ds.Tables["replvl_vw"].Rows[i]["RepLvlId"] + ")";
                                    _nretVal = _oDataAccess.ExecuteSQLStatement(strqr4, null, 20, true);
                                }
                                if (_nretVal > 0)
                                {
                                    string strqr3 = "Insert into usrlv values(" + ds.Tables["replvl_vw"].Rows[i]["replvlid"] + "," + ds.Tables["replvl_vw"].Rows[i]["qryid"] + "," + ds.Tables["replvl_vw"].Rows[i]["RepLvlId"] + ",'" + ds.Tables["replvl_vw"].Rows[i]["prycl"] + "','" + ds.Tables["replvl_vw"].Rows[i]["seccl"] + "')";
                                    _nretVal = _oDataAccess.ExecuteSQLStatement(strqr3, null, 20, true);
                                }
                                //func.GetRecord(strqr3, con);        // Commented by Sachin N. S. on 05/06/2013 for Bug-4524
                                //func.GetRecord(strqr4, con);        // Commented by Sachin N. S. on 05/06/2013 for Bug-4524
                                if (_nretVal > 0)
                                {
                                    for (int j = 0; j < ds.Tables["col_vw"].Rows.Count; j++)
                                    {
                                        if (Convert.ToInt32(ds.Tables["replvl_vw"].Rows[i]["qryid"]) == Convert.ToInt32(ds.Tables["col_vw"].Rows[j]["qryid"]))
                                        {
                                            if (_nretVal > 0)
                                            {
                                                string strqr5 = "Insert into uscrl values(" + ds.Tables["col_vw"].Rows[j]["colid"] + "," + ds.Tables["replvl_vw"].Rows[i]["qryid"] + ",'" + txtRepId.Text + "'," + ds.Tables["replvl_vw"].Rows[i]["replvlid"] + ",'" + ds.Tables["col_vw"].Rows[j]["isgrouped"] + "','" + ds.Tables["col_vw"].Rows[j]["isfreezing"] + "'," + ds.Tables["col_vw"].Rows[j]["colwidth"] + ",'" + ds.Tables["col_vw"].Rows[j]["issummury"] + "','" + ds.Tables["col_vw"].Rows[j]["isdisplayed"] + "'," + ds.Tables["col_vw"].Rows[j]["grouporder"] + ")";
                                                _nretVal = _oDataAccess.ExecuteSQLStatement(strqr5, null, 20, true);
                                            }
                                            //func.GetRecord(strqr5, con);    // Commented by Sachin N. S. on 05/06/2013 for Bug-4524
                                            if (_nretVal > 0)
                                            {
                                                string strqr6 = "Insert into uscol values(" + ds.Tables["col_vw"].Rows[j]["colid"] + ",'" + ds.Tables["col_vw"].Rows[j]["columnnames"] + "','" + ds.Tables["col_vw"].Rows[j]["columncaption"] + "','" + ds.Tables["col_vw"].Rows[j]["columndatatype"] + "'," + ds.Tables["col_vw"].Rows[j]["columnorder"] + ",'" + ds.Tables["col_vw"].Rows[j]["Precision"] + "')";
                                                _nretVal = _oDataAccess.ExecuteSQLStatement(strqr6, null, 20, true);
                                            }
                                            //func.GetRecord(strqr6, con);    // Commented by Sachin N. S. on 05/06/2013 for Bug-4524
                                        }
                                    }
                                }
                                if (_nretVal <= 0)
                                {
                                    break;
                                }
                            }
                        }

                        if (_nretVal > 0)
                        {
                            _oDataAccess.CommitTransaction();
                            MessageBox.Show("Record Added successfully", _msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            //return;
                        }
                        else
                        {
                            _oDataAccess.RollbackTransaction();
                            MessageBox.Show("Record cannot be added properly. Please try again...!!!", _msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            return;
                        }
                    }
                    else
                    {
                        _oDataAccess.RollbackTransaction();
                        MessageBox.Show("Query Level is not defined.", _msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        return;
                    }

                }
                else if (edit == true)
                {
                    //******* Added by Sachin N. S. on 12/07/2013 for Bug-4524 -- Start *******//
                    if (ds.Tables["replvl_vw"].Rows.Count <= 0)
                    {
                        MessageBox.Show("Query Level is not defined.", _msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        return;
                    }

                    //******* Added by Sachin N. S. on 12/07/2013 for Bug-4524 -- End *******//
                    int _nretVal = 0;
                    _nretVal = this.SaveEditRecords();
                    if (_nretVal <= 0)
                        return;
                    ////_oDataAccess.BeginTransaction();    // Added by Sachin N. S. on 05/06/2013 for Bug-4524
                    ////int _nretVal = 0;                     // Added by Sachin N. S. on 05/06/2013 for Bug-4524

                    //////----Deleting Records from Database ** Start ----//
                    ////string _colid = string.Join(",", ds.Tables["col_vw"].Rows.OfType<DataRow>().Select(rw => rw["Colid"].ToString()).ToArray());
                    ////string updateSql = "Delete from uscol From uscol a inner join uscrl b on a.colid=b.Colid where b.repid='" + txtRepId.Text + "' and b.colid not in (" + _colid + ")";
                    ////_nretVal = _oDataAccess.ExecuteSQLStatement(updateSql, null, 20, true);
                    ////if (_nretVal <= 0)
                    ////{
                    ////    _oDataAccess.RollbackTransaction();
                    ////    MessageBox.Show("Deleting from Column Settings table failed. Cannot continue...!!", _msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    ////    return;
                    ////}

                    ////updateSql = "Delete from uscrl where repid='" + txtRepId.Text + "' and colid not in (" + _colid + ")";
                    ////_nretVal = _oDataAccess.ExecuteSQLStatement(updateSql, null, 20, true);
                    ////if (_nretVal <= 0)
                    ////{
                    ////    _oDataAccess.RollbackTransaction();
                    ////    MessageBox.Show("Deleting from column settings table failed. Cannot continue...!!", _msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    ////    return;
                    ////}

                    ////DataTable _dt=new DataTable();
                    ////_colid = string.Join(",", ds.Tables["replvl_vw"].Rows.OfType<DataRow>().Select(rw => rw["QryId"].ToString()).ToArray());
                    ////updateSql = "select b.lvltyp from usrlv b inner join usqry c on b.qryid=c.qryid where c.repid='" + txtRepId.Text + "' and b.qryid not in (" + _colid + ")";
                    ////_dt = _oDataAccess.GetDataTable(updateSql, null, 20);
                    ////if (_nretVal <= 0)
                    ////{
                    ////    _oDataAccess.RollbackTransaction();
                    ////    MessageBox.Show("Accessing the relational table records failed. Cannot continue...!!", _msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    ////    return;
                    ////}

                    ////updateSql = "Delete from usrlv from usrlv a inner join usqry b on a.qryid=b.qryid where b.repid='" + txtRepId.Text + "' and b.qryid not in (" + _colid + ")";
                    ////_nretVal = _oDataAccess.ExecuteSQLStatement(updateSql, null, 20, true);
                    ////if (_nretVal <= 0)
                    ////{
                    ////    _oDataAccess.RollbackTransaction();
                    ////    MessageBox.Show("Deleting from level relation table failed. Cannot continue...!!", _msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    ////    return;
                    ////}
                    ////string _lvlid;
                    ////_lvlid = string.Join(",", _dt.Rows.OfType<DataRow>().Select(rw => rw["LvlTyp"].ToString()).ToArray());
                    ////if (_lvlid != "")
                    ////{
                    ////    updateSql = "Delete from uslty where lvltid in (" + _lvlid + ") and lvltid not in (select b.lvltyp from usrlv b inner join usqry c on b.qryid=c.qryid where c.repid!='" + txtRepId.Text + "')";
                    ////    _nretVal = _oDataAccess.ExecuteSQLStatement(updateSql, null, 20, true);
                    ////    if (_nretVal <= 0)
                    ////    {
                    ////        _oDataAccess.RollbackTransaction();
                    ////        MessageBox.Show("Deleting from report level table failed. Cannot continue...!!", _msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    ////        return;
                    ////    }
                    ////}
                    ////updateSql = "Delete from usqry where Repid = '" + txtRepId.Text + "' and QryId not in (" + _colid + ") ";
                    ////_nretVal = _oDataAccess.ExecuteSQLStatement(updateSql, null, 20, true);
                    ////if (_nretVal <= 0)
                    ////{
                    ////    _oDataAccess.RollbackTransaction();
                    ////    MessageBox.Show("Deleting from report level table failed. Cannot continue...!!", _msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    ////    return;
                    ////}

                    //////----Deleting Records from Database ** End ----//

                    ////if (ds.Tables["rep_vw"].Rows.Count > 0)
                    ////{
                    ////    DataRow[] rep = ds.Tables["rep_vw"].Select("Repid ='" + txtRepId.Text+"'");
                    ////    rep[0]["repnm"] = txtRepNm.Text;
                    ////    rep[0]["repty"] = txtRepTy.Text;
                    ////}
                    ////else  //******* Added by Sachin N. S. on 12/07/2013 for Bug-4524 -- Start *******//
                    ////{
                    ////    updateSql = "Delete from usrep WHERE repid ='" + txtRepId.Text + "'";
                    ////    _nretVal = _oDataAccess.ExecuteSQLStatement(updateSql, null, 20, true);
                    ////}   //******* Added by Sachin N. S. on 12/07/2013 for Bug-4524 -- End *******//
                    ////ds.Tables["rep_vw"].AcceptChanges();
                    ////for (int i = 0; i < ds.Tables["rep_vw"].Rows.Count; i++)
                    ////{
                    ////    updateSql = "UPDATE usrep SET repnm ='" + ds.Tables["rep_vw"].Rows[i]["repnm"] + "',repty='" + ds.Tables["rep_vw"].Rows[i]["repty"] + "' WHERE repid ='" + ds.Tables["rep_vw"].Rows[i]["repid"] + "'";
                    ////    //func.GetRecord(updateSql, con);   // Commented by Sachin N. S. on 05/06/2013 for Bug-4524
                    ////    //******* Added by Sachin N. S. on 05/06/2013 for Bug-4524 -- Start *******//
                    ////    _nretVal = _oDataAccess.ExecuteSQLStatement(updateSql, null, 20, true);
                    ////    if (_nretVal <= 0)
                    ////        break;
                    ////    //******* Added by Sachin N. S. on 05/06/2013 for Bug-4524 -- End *******//
                    ////}
                    ////if (_nretVal > 0)       //******* Added by Sachin N. S. on 05/06/2013 for Bug-4524
                    ////{
                    ////    string _paramId = string.Join(",", ds.Tables["Para_vw"].Rows.OfType<DataRow>().Select(rw => rw["ParameterId"].ToString()).ToArray());
                    ////    if (_paramId != "")
                    ////    {
                    ////        updateSql = "Delete from Para_query_master WHERE repid ='" + txtRepId.Text + "' and Parameterid not in (" + _paramId + ")";
                    ////        _nretVal = _oDataAccess.ExecuteSQLStatement(updateSql, null, 20, true);
                    ////    }
                    ////    if (_nretVal > 0)
                    ////    {

                    ////        for (int i = 0; i < dgvParaRep.Rows.Count; i++)
                    ////        {
                    ////            if (dgvParaRep.Rows[i].Cells["ParameterID"].Value == null)
                    ////                strParaid = Convert.ToString(DBNull.Value);
                    ////            else
                    ////                strParaid = dgvParaRep.Rows[i].Cells["ParameterID"].Value.ToString();
                    ////            if (dgvParaRep.Rows[i].Cells["QueryId"].Value == null)
                    ////                strQryid = Convert.ToString(DBNull.Value);
                    ////            else
                    ////                strQryid = dgvParaRep.Rows[i].Cells["QueryId"].Value.ToString();
                    ////            if (dgvParaRep.Rows[i].Cells["DisplayOrder"].Value == null)
                    ////                strDisor = Convert.ToString(DBNull.Value);
                    ////            else
                    ////                strDisor = dgvParaRep.Rows[i].Cells["DisplayOrder"].Value.ToString();

                    ////            string updateSql1 = "UPDATE para_query_master SET parameterID='" + strParaid + "',queryid='" + strQryid + "',displayorder ='" + strDisor + "' WHERE repid ='" + txtRepId.Text + "' and Parameterid=" + dgvParaRep.Rows[i].Cells["ParameterID"].Value.ToString();
                    ////            //func.GetRecord(updateSql1, con);
                    ////            //******* Added by Sachin N. S. on 05/06/2013 for Bug-4524 -- Start *******//
                    ////            _nretVal = _oDataAccess.ExecuteSQLStatement(updateSql1, null, 20, true);
                    ////            if (_nretVal <= 0)
                    ////                break;
                    ////            //******* Added by Sachin N. S. on 05/06/2013 for Bug-4524 -- End *******//
                    ////        }
                    ////    }
                    ////}
                    ////if (_nretVal > 0)   //******* Added by Sachin N. S. on 05/06/2013 for Bug-4524
                    ////{
                    ////    //string _paramId = string.Join(",", ds.Tables["replvl_vw"].Rows.OfType<DataRow>().Select(rw => rw["ParameterId"].ToString()).ToArray());
                    ////    //updateSql = "Delete from usrlv WHERE LVLID=" + ds.Tables["replvl_vw"].Rows[i]["replvlid"] + " and QRYID=" + ds.Tables["replvl_vw"].Rows[i]["qryid"] + " ";
                    ////    //_nretVal = _oDataAccess.ExecuteSQLStatement(updateSql, null, 20, true);

                    ////    for (int i = 0; i < ds.Tables["replvl_vw"].Rows.Count; i++)
                    ////    {
                    ////        string updateSql2 = "UPDATE usrlv SET  LVLTYP=" + ds.Tables["replvl_vw"].Rows[i]["RepLvlId"] + ",PRYCL='" + ds.Tables["replvl_vw"].Rows[i]["prycl"] + "',SECCL='" + ds.Tables["replvl_vw"].Rows[i]["seccl"] + "' WHERE LVLID=" + ds.Tables["replvl_vw"].Rows[i]["replvlid"] + " and QRYID=" + ds.Tables["replvl_vw"].Rows[i]["qryid"] + " ";
                    ////        //func.GetRecord(updateSql2, con);
                    ////        //******* Added by Sachin N. S. on 05/06/2013 for Bug-4524 -- Start *******//
                    ////        _nretVal = _oDataAccess.ExecuteSQLStatement(updateSql2, null, 20, true);
                    ////        if (_nretVal <= 0)
                    ////            break;
                    ////        //******* Added by Sachin N. S. on 05/06/2013 for Bug-4524 -- End *******//
                    ////        if (_nretVal > 0)
                    ////        {
                    ////            string updateSql3 = "UPDATE uslty SET lvlty='" + ds.Tables["replvl_vw"].Rows[i]["lvlty"] + "',lvlnm='" + ds.Tables["replvl_vw"].Rows[i]["lvlnm"] + "' where lvltid=" + ds.Tables["replvl_vw"].Rows[i]["RepLvlId"] + "";
                    ////            //func.GetRecord(updateSql3, con);
                    ////            //******* Added by Sachin N. S. on 05/06/2013 for Bug-4524 -- Start *******//
                    ////            _nretVal = _oDataAccess.ExecuteSQLStatement(updateSql3, null, 20, true);
                    ////            if (_nretVal <= 0)
                    ////                break;
                    ////            //******* Added by Sachin N. S. on 05/06/2013 for Bug-4524 -- End *******//
                    ////        }
                    ////        if (_nretVal > 0)
                    ////        {
                    ////            string _repqry = ds.Tables["replvl_vw"].Rows[i]["repqry"].ToString().Replace("'", "''");
                    ////            //string updateSql4 = "UPDATE usqry SET repqry='" + ds.Tables["replvl_vw"].Rows[i]["repqry"] + "' where qryid=" + ds.Tables["replvl_vw"].Rows[i]["qryid"] + " and repid='" + txtRepId.Text + "' and replvlid=" + ds.Tables["replvl_vw"].Rows[i]["replvlid"] + "";
                    ////            string updateSql4 = "UPDATE usqry SET repqry='" + _repqry + "' where qryid=" + ds.Tables["replvl_vw"].Rows[i]["qryid"] + " and repid='" + txtRepId.Text + "' and replvlid=" + ds.Tables["replvl_vw"].Rows[i]["replvlid"] + "";
                    ////            //func.GetRecord(updateSql4, con);
                    ////            //******* Added by Sachin N. S. on 05/06/2013 for Bug-4524 -- Start *******//
                    ////            _nretVal = _oDataAccess.ExecuteSQLStatement(updateSql4, null, 20, true);
                    ////            if (_nretVal <= 0)
                    ////                break;
                    ////            //******* Added by Sachin N. S. on 05/06/2013 for Bug-4524 -- End *******//
                    ////        }
                    ////        if (_nretVal > 0)       //******* Added by Sachin N. S. on 05/06/2013 for Bug-4524
                    ////        {
                    ////            for (int j = 0; j < ds.Tables["col_vw"].Rows.Count; j++)
                    ////            {
                    ////                if (Convert.ToInt32(ds.Tables["replvl_vw"].Rows[i]["qryid"]) == Convert.ToInt32(ds.Tables["col_vw"].Rows[j]["qryid"]))
                    ////                {
                    ////                    string updateSql5 = "UPDATE uscrl set isgrouped='" + ds.Tables["col_vw"].Rows[j]["isgrouped"] + "',isfreezing='" + ds.Tables["col_vw"].Rows[j]["isfreezing"] + "',colwidth=" + ds.Tables["col_vw"].Rows[j]["colwidth"] + ",issummury='" + ds.Tables["col_vw"].Rows[j]["issummury"] + "',isdisplayed='" + ds.Tables["col_vw"].Rows[j]["isdisplayed"] + "',grouporder=" + ds.Tables["col_vw"].Rows[j]["grouporder"] + " where colid=" + ds.Tables["col_vw"].Rows[j]["colid"] + " and qryid=" + ds.Tables["replvl_vw"].Rows[i]["qryid"] + " and repid='" + txtRepId.Text + "' and replvlid=" + ds.Tables["replvl_vw"].Rows[i]["replvlid"] + "";
                    ////                    //func.GetRecord(updateSql5, con);
                    ////                    //******* Added by Sachin N. S. on 05/06/2013 for Bug-4524 -- Start *******//
                    ////                    _nretVal = _oDataAccess.ExecuteSQLStatement(updateSql5, null, 20, true);
                    ////                    if (_nretVal <= 0)
                    ////                        break;
                    ////                    //******* Added by Sachin N. S. on 05/06/2013 for Bug-4524 -- End *******//
                    ////                    if (_nretVal > 0)       //******* Added by Sachin N. S. on 05/06/2013 for Bug-4524
                    ////                    {
                    ////                        string updateSql6 = "UPDATE uscol set columncaption='" + ds.Tables["col_vw"].Rows[j]["columncaption"] + "',columndatatype='" + ds.Tables["col_vw"].Rows[j]["columndatatype"] + "',columnorder=" + ds.Tables["col_vw"].Rows[j]["columnorder"] + ",precision='" + ds.Tables["col_vw"].Rows[j]["Precision"] + "' where colid= " + ds.Tables["col_vw"].Rows[j]["colid"] + " ";
                    ////                        //func.GetRecord(updateSql6, con);
                    ////                        //******* Added by Sachin N. S. on 05/06/2013 for Bug-4524 -- Start *******//
                    ////                        _nretVal = _oDataAccess.ExecuteSQLStatement(updateSql6, null, 20, true);
                    ////                        if (_nretVal <= 0)
                    ////                            break;
                    ////                        //******* Added by Sachin N. S. on 05/06/2013 for Bug-4524 -- End *******//
                    ////                    }
                    ////                }
                    ////            }
                    ////        }
                    ////    }
                    ////}
                    ////if (_nretVal > 0)       //******* Added by Sachin N. S. on 05/06/2013 for Bug-4524 -- Start
                    ////{
                    ////    _oDataAccess.CommitTransaction();
                    ////    MessageBox.Show("Record Updated successfully", _msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    ////    //return;
                    ////}
                    ////else
                    ////{
                    ////    _oDataAccess.RollbackTransaction();
                    ////    MessageBox.Show("Record are not updated properly.", _msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    ////    return;
                    ////}
                    //////******* Added by Sachin N. S. on 05/06/2013 for Bug-4524 -- End
                }
            }
            else
            {
                MessageBox.Show("Report Name or Query Level is not saved properly.", _msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                txtRepNm.Focus();
                return;
            }
            add = false;
            edit = false;
            Enbldisbl(false);
            this.BindControlsParent();
            ds.Tables["rep_vw"].AcceptChanges();
            ds.Tables["para_vw"].AcceptChanges();
            ds.Tables["rep_vw"].AcceptChanges();
            ds.Tables["col_vw"].AcceptChanges();
            // con.Close();         // Commented by Sachin N. S. on 06/06/2013 for Bug-4524
            string _navBtn = "";
            GetHeader("Last", "", ref _navBtn);   // ------- Changed By Sachin N. S. on 08/03/2013
            this.Navigate("Last");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Enbldisbl(false);
            clear();
            clearlvl();
            ctrlBindclr();
            ctrlBindclrChild();
            this.BindControlsParent();
            this.BindControlsChildlvl();
            this.BindControlsChildpara();
            string _navBtn = "";
            this.GetHeader("Last", "", ref _navBtn);   // ------- Changed By Sachin N. S. on 08/03/2013
            cmdAddlvl.Text = "&Add Level";
            cmdRemovelvl.Text = "&Remove Level";
            this.Navigate("Last");
            dgvCol.Refresh();
            Enbldisbl(false);
            add = false;
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult x = MessageBox.Show("Do you really want to exit?", _msgCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (x == DialogResult.Yes)
                this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
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

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnDelete.Enabled)
                btnDelete_Click(this.btnDelete, e);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnExit_Click(this.btnExit, e);
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (this.btnSave.Enabled)
                btnSave_Click(this.btnSave, e);
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnCancel.Enabled)
                btnCancel_Click(this.btnCancel, e);
        }

        #endregion "Main Tool bar Events"

        #region "Header Control Events"

        private void cmdRepTy_Click(object sender, EventArgs e)
        {
            // Commented by Sachin N. S. on 09/03/2013 for Bug-4524 --- Start
            //frmSearchForm ft = new frmSearchForm(func.getConnection(), "Select Report Type", "usrep", "repty", "repty:Report Type", "", "repty");
            //DialogResult res = ft.ShowDialog();
            //if (res == DialogResult.OK)
            //{
            //    txtRepTy.Text = ft.ReturnString[0];
            //}
            //else
            //{
            //    txtRepTy.Text = ft.ReturnString[0];
            //}
            // Commented by Sachin N. S. on 09/03/2013 for Bug-4524 --- End

            // Added by Sachin N. S. on 09/03/2013 for Bug-4524 --- Start
            string cFrmCap = "", cSrchCol = "", cDispCol = "", cRetCol = "";
            DataTable _dt = new DataTable();
            _dt.Columns.Add(new DataColumn("RepTy", typeof(string)));

            DataRow _dr;
            _dr = _dt.NewRow();
            _dr["RepTy"] = "View";

            _dr = _dt.NewRow();
            _dr["RepTy"] = "Chart";

            udSelectPop.SELECTPOPUP _oSelPop = new udSelectPop.SELECTPOPUP();
            cFrmCap = "Select Report Type";
            cSrchCol = "RepTy";
            cDispCol = "RepTy:Report Type";
            cRetCol = "RepTy";
            DataView _dvw = _dt.DefaultView;

            _oSelPop.pdataview = _dvw;
            _oSelPop.pformtext = cFrmCap;
            _oSelPop.psearchcol = cSrchCol;
            _oSelPop.pDisplayColumnList = cDispCol;
            _oSelPop.pRetcolList = cRetCol;
            //_oSelPop.Icon = new Icon();
            _oSelPop.ShowDialog();
            if (_oSelPop.pReturnArray != null)
            {
                txtRepTy.Text = _oSelPop.pReturnArray[0].ToString();
            }
            // Added by Sachin N. S. on 09/03/2013 for Bug-4524 --- End
        }

        private void cmdReportNm_Click(object sender, EventArgs e)
        {
            //***** Commented by Sachin N. S. on 06/06/2013 for Bug-4524 -- Start *****//
            //frmSearchForm ft = new frmSearchForm(func.getConnection(), "Select Report Name", "usrep", "repid,repnm,repty", "repnm:Report Name", "repty", "repid,repnm,repty");
            //DialogResult res = ft.ShowDialog();
            //if (res == DialogResult.OK)
            //{
            //    strRowId = ft.strRepid;
            //    Navigate("");
            //}
            //else
            //{
            //    strRowId = ft.strRepid;
            //    this.Navigate("");
            //}
            //***** Commented by Sachin N. S. on 06/06/2013 for Bug-4524 -- Start *****//

            // Added by Sachin N. S. on 06/06/2013 for Bug-4524 --- Start
            string cSql = "Select RepId, RepNm, RepTy from usRep Order by RepId";
            string cFrmCap = "", cSrchCol = "", cDispCol = "", cRetCol = "";
            DataTable _dt = new DataTable();
            _dt = _oDataAccess.GetDataTable(cSql, null, 50);

            udSelectPop.SELECTPOPUP _oSelPop = new udSelectPop.SELECTPOPUP();
            cFrmCap = "Select Report";
            cSrchCol = "RepNm";
            cDispCol = "RepId:Report Id,RepNm:Report Name,RepTy:Report Type";
            cRetCol = "RepId";
            DataView _dvw = _dt.DefaultView;

            _oSelPop.pdataview = _dvw;
            _oSelPop.pformtext = cFrmCap;
            _oSelPop.psearchcol = cSrchCol;
            _oSelPop.pDisplayColumnList = cDispCol;
            _oSelPop.pRetcolList = cRetCol;
            //_oSelPop.Icon = new Icon();
            _oSelPop.ShowDialog();
            if (_oSelPop.pReturnArray != null)
            {
                strRowId = _oSelPop.pReturnArray[0].ToString();
                this.txtRepId.Text = strRowId;
                this.Navigate("");
            }
            // Added by Sachin N. S. on 06/06/2013 for Bug-4524 --- End
        }

        private void txtRepTy_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                cmdRepTy.PerformClick();
            }
        }

        private void txtRepNm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                cmdReportNm.PerformClick();
            }
        }

        #endregion "Header Control Events"

        #region "Parameter Tab -- Control Events"

        private void cmdAddParam_Click(object sender, EventArgs e)
        {
            DataRow dr = ds.Tables["para_vw"].NewRow();
            ds.Tables["para_vw"].Rows.Add(dr);
            this.BindingContext[ds, "para_vw"].Position = ds.Tables["para_vw"].Rows.Count;
            if (dgvParaRep.CurrentRow.Index < this.dgvParaRep.Rows.Count && dgvParaRep.Rows.Count > 1)
            {
                //int i = dgvParaRep.CurrentRow.Index + 1;
                int i = this.BindingContext[ds, "para_vw"].Position;
                dgvParaRep.CurrentCell = dgvParaRep.Rows[i].Cells[1];
            }
            //cmdParacap.PerformClick();    // Commented by Sachin N. S. on 28/06/2013 for Bug-4324
            this.getParameterFld();
        }

        private void cmdRemoveParam_Click(object sender, EventArgs e)
        {
            //foreach (DataGridViewRow row in dgvParaRep.SelectedRows)
            //    if (!row.IsNewRow)
            //        dgvParaRep.Rows.Remove(row);
            ////DataRow[] row = ds.Tables["para_vw"].Select("ParameterId=" + dgvParaRep.Rows[0].Cells["ParameterID"].ToString());
            ////ds.Tables["para_vw"].Rows.Remove(row[0]);

            //if (dgvParaRep.CurrentRow != null)
            //    dgvParaRep.Rows.Remove(dgvParaRep.CurrentRow);

            if (dgvParaRep.CurrentRow != null)
            {
                DataRow[] row = ds.Tables["para_vw"].Select("ParameterId=" + dgvParaRep.CurrentRow.Cells["ParameterID"].Value.ToString());
                ds.Tables["Para_vw"].Rows.Remove(row[0]);
                ds.Tables["Para_vw"].AcceptChanges();
            }
        }

        private void dgvParaRep_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
            }
        }

        private void cmdParacap_Click(object sender, EventArgs e)
        {
            // Changed by Sachin N. S. on 09/03/2013 --- Start
            //frmSearchForm ft = new frmSearchForm(func.getConnection(), "Select Parameter Caption", "para_master", "parameterID,paramname,queryid,ParamType", "paramname:Parameter Caption", "parameterID,queryid,ParamType", "parameterID,paramname,queryid,ParamType,paramname");
            //DialogResult res = ft.ShowDialog();
            string cSql = "";
            string cFrmCap = "", cSrchCol = "", cDispCol = "", cRetCol = "";
            DataTable _dt = new DataTable();

            udSelectPop.SELECTPOPUP _oSelPop = new udSelectPop.SELECTPOPUP();
            DataTable _tmpTbl = ds.Tables["Para_Vw"];
            DataColumn _dc = _tmpTbl.Columns["parameterId"];
            string _paramId = string.Join(",", _tmpTbl.Rows.OfType<DataRow>().Select(row => row[_dc].ToString()).ToArray());
            _paramId = _paramId.Length > 0 ? _paramId.Substring(0, _paramId.Length - 1) : _paramId;

            string _ccond = _paramId.ToString() == "" ? "" : "where parameterid not in (" + _paramId + ")";
            cSql = "Select distinct parameterID,paramname,queryid,ParamType,paracaption From para_master " + _ccond;
            cFrmCap = "Select Parameter Field";
            cSrchCol = "paracaption";
            cDispCol = "paramname:Parameter Name,paracaption:Parameter Caption";
            cRetCol = "parameterID,paramname,queryid,ParamType,paracaption";
            _dt = _oDataAccess.GetDataTable(cSql, null, 50);
            DataView _dvw = _dt.DefaultView;

            _oSelPop.pdataview = _dvw;
            _oSelPop.pformtext = cFrmCap;
            _oSelPop.psearchcol = cSrchCol;
            _oSelPop.pDisplayColumnList = cDispCol;
            _oSelPop.pRetcolList = cRetCol;
            //_oSelPop.Icon = new Icon();
            _oSelPop.ShowDialog();

            // Changed by Sachin N. S. on 09/03/2013 --- End

            try
            {
                // Changed by Sachin N. S. on 09/03/2013 --- Start
                //if (res == DialogResult.OK)       
                if (_oSelPop.pReturnArray != null)
                // Changed by Sachin N. S. on 09/03/2013 --- End
                {
                    if (dgvParaRep.Rows.Count > 0)
                    {
                        bool lAdd = true;
                        for (int i = 0; i < dgvParaRep.Rows.Count; i++)
                        {
                            if (dgvParaRep.Rows[i].Cells["paramname"].Value.ToString() == _oSelPop.pReturnArray[1].ToString())
                            {
                                MessageBox.Show("Duplicate Parameter Caption \n.... could not save", _msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                                cmdParacap.Focus();
                                lAdd = false;
                                return;
                            }
                        }
                        if (lAdd == true)
                        {
                            dgvParaRep.CurrentRow.Cells["parameterID"].Value = _oSelPop.pReturnArray[0].ToString();
                            dgvParaRep.CurrentRow.Cells["paracaption"].Value = _oSelPop.pReturnArray[4].ToString();
                            dgvParaRep.CurrentRow.Cells["queryid"].Value = _oSelPop.pReturnArray[2].ToString();
                            dgvParaRep.CurrentRow.Cells["paramtype"].Value = _oSelPop.pReturnArray[3].ToString();
                            dgvParaRep.CurrentRow.Cells["paramname"].Value = _oSelPop.pReturnArray[1].ToString();
                        }
                    }
                    else
                    {
                        dgvParaRep.CurrentRow.Cells["parameterID"].Value = _oSelPop.pReturnArray[0].ToString();
                        dgvParaRep.CurrentRow.Cells["paracaption"].Value = _oSelPop.pReturnArray[4].ToString();
                        dgvParaRep.CurrentRow.Cells["queryid"].Value = _oSelPop.pReturnArray[2].ToString();
                        dgvParaRep.CurrentRow.Cells["paramtype"].Value = _oSelPop.pReturnArray[3].ToString();
                        dgvParaRep.CurrentRow.Cells["paramname"].Value = _oSelPop.pReturnArray[1].ToString();
                    }
                    string ch = dgvParaRep.CurrentRow.Cells["paracaption"].Value.ToString();
                    if (dgvParaRep.CurrentRow.Cells["Paramtype"].Value.ToString() == "2")
                    {
                        if (ch.StartsWith("F") || ch.StartsWith("A"))
                        {
                            dgvParaRep.CurrentRow.Cells["paramvalue"].Value = "01/01/1900";
                        }
                        else
                        {
                            dgvParaRep.CurrentRow.Cells["paramvalue"].Value = "01/01/2015";
                        }
                    }
                    else
                    {
                        if (ch.StartsWith("F"))
                        {
                            dgvParaRep.CurrentRow.Cells["paramvalue"].Value = "a";
                        }
                        else
                        {
                            dgvParaRep.CurrentRow.Cells["paramvalue"].Value = "z";
                        }
                    }
                    SendKeys.Send("{tab}");
                    cmdParacap.Visible = false;
                    dgvParaRep.Focus();
                    this.Refresh();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void dgvParaRep_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            this.refesh_rtbsqlqry();
        }

        private void dgvParaRep_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.refesh_rtbsqlqry();
            //******* Commented by Sachin N. S. on 28/06/2013 for Bug-4324 -- Start *******//
            //if (add == true || edit == true)
            //{
            //    if (e.ColumnIndex == 1)
            //    {
            //        cmdParacap.Visible = true;
            //        Point _point;
            //        //int x = dgvParaRep.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false).Right + dgvParaRep.Left - 20;
            //        //int y = dgvParaRep.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false).Top + dgvParaRep.Bottom - 187;
            //        int x = this.Location.X + dgvParaRep.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex , false).X + dgvParaRep.Location.X; // -20;
            //        int y = this.Location.Y + dgvParaRep.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex , false).Y + dgvParaRep.Location.Y; // -187;

            //        //_point = dgvParaRep.GetCellDisplayRectangle(e.ColumnIndex, dgvParaRep.CurrentCellAddress.Y, true).Location + dgvParaRep.Location;

            //        this.cmdParacap.Location = new Point(x, y);
            //        //this.cmdParacap.Location = _point;
            //    }
            //    else
            //    {
            //        cmdParacap.Visible = false;
            //    }
            //}
            //else
            //    cmdParacap.Visible = false;
            //******* Commented by Sachin N. S. on 28/06/2013 for Bug-4324 -- End *******//
        }

        private void dgvParaRep_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox txtEdit = (TextBox)e.Control;
            txtEdit.KeyPress += txtEdit_KeyPress;
            vdgvcol = false;
        }

        private void getParameterFld()
        {
            // Changed by Sachin N. S. on 09/03/2013 --- Start
            //frmSearchForm ft = new frmSearchForm(func.getConnection(), "Select Parameter Caption", "para_master", "parameterID,paramname,queryid,ParamType", "paramname:Parameter Caption", "parameterID,queryid,ParamType", "parameterID,paramname,queryid,ParamType,paramname");
            //DialogResult res = ft.ShowDialog();
            string cSql = "";
            string cFrmCap = "", cSrchCol = "", cDispCol = "", cRetCol = "";
            DataTable _dt = new DataTable();

            udSelectPop.SELECTPOPUP _oSelPop = new udSelectPop.SELECTPOPUP();
            DataTable _tmpTbl = ds.Tables["Para_Vw"];
            DataColumn _dc = _tmpTbl.Columns["parameterId"];
            DataRow[] __datarow1 = _tmpTbl.Select("ParameterId<>0");
            string _paramId = string.Join(",", _tmpTbl.Rows.OfType<DataRow>().Select(row => row[_dc].ToString()).ToArray());

            //.AsEnumerable().Where(row => !string.IsNullOrEmpty(row.Field<int>(_dc).ToString()))
            //string _paramId = string.Join(",", _tmpTbl.AsEnumerable().Where(row => row.Field<string>("ParameterId")!=null && !string.IsNullOrEmpty(Convert.ToString(row.Field<int>("ParameterId")))).Select(row => row[_dc].ToString()).ToArray());
            //    .Rows.OfType<DataRow>().Select(row => row[_dc].ToString()).ToArray());

            _paramId = _paramId.Length > 0 ? _paramId.Substring(0, _paramId.Length - 1) : _paramId;

            string _ccond = _paramId.ToString() == "" ? "" : "where a.parameterid not in (" + _paramId + ")";
            cSql = "Select distinct a.parameterID,a.paramname,a.queryid,a.ParamType,a.paracaption, isnull(b.Repqry, '') as RepQry From para_master a Left join usqry b on a.queryid=b.qryid " + _ccond;
            cFrmCap = "Select Parameter Field";
            cSrchCol = "paracaption";
            cDispCol = "paramname:Parameter Name,paracaption:Parameter Caption";
            cRetCol = "parameterID,paramname,queryid,ParamType,paracaption,RepQry";
            _dt = _oDataAccess.GetDataTable(cSql, null, 50);
            DataView _dvw = _dt.DefaultView;

            _oSelPop.pdataview = _dvw;
            _oSelPop.pformtext = cFrmCap;
            _oSelPop.psearchcol = cSrchCol;
            _oSelPop.pDisplayColumnList = cDispCol;
            _oSelPop.pRetcolList = cRetCol;
            //_oSelPop.Icon = new Icon();
            _oSelPop.ShowDialog();

            // Changed by Sachin N. S. on 09/03/2013 --- End

            try
            {
                // Changed by Sachin N. S. on 09/03/2013 --- Start
                //if (res == DialogResult.OK)       
                if (_oSelPop.pReturnArray != null)
                // Changed by Sachin N. S. on 09/03/2013 --- End
                {
                    if (dgvParaRep.Rows.Count > 0)
                    {
                        bool lAdd = true;
                        for (int i = 0; i < dgvParaRep.Rows.Count; i++)
                        {
                            if (dgvParaRep.Rows[i].Cells["paramname"].Value.ToString() == _oSelPop.pReturnArray[1].ToString())
                            {
                                MessageBox.Show("Duplicate Parameter Caption \n.... could not save", _msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                                cmdParacap.Focus();
                                lAdd = false;
                                return;
                            }
                        }
                        if (lAdd == true)
                        {
                            dgvParaRep.CurrentRow.Cells["parameterID"].Value = _oSelPop.pReturnArray[0].ToString();
                            dgvParaRep.CurrentRow.Cells["paracaption"].Value = _oSelPop.pReturnArray[4].ToString();
                            dgvParaRep.CurrentRow.Cells["queryid"].Value = _oSelPop.pReturnArray[2].ToString();
                            dgvParaRep.CurrentRow.Cells["paramtype"].Value = _oSelPop.pReturnArray[3].ToString();
                            dgvParaRep.CurrentRow.Cells["paramname"].Value = _oSelPop.pReturnArray[1].ToString();
                            dgvParaRep.CurrentRow.Cells["Para_Order"].Value = 0;  // Added by Sachin N. S. on 01/07/2013 for Bug-4524
                            ds.Tables["Para_Vw"].Rows[this.BindingContext[ds, "Para_vw"].Position]["RepQry"] = _oSelPop.pReturnArray[5].ToString();  // Added by Sachin N. S. on 01/07/2013 for Bug-4524
                            rtbsqlqry.Text = _oSelPop.pReturnArray[5].ToString();  // Added by Sachin N. S. on 01/07/2013 for Bug-4524
                        }
                    }
                    else
                    {
                        dgvParaRep.CurrentRow.Cells["parameterID"].Value = _oSelPop.pReturnArray[0].ToString();
                        dgvParaRep.CurrentRow.Cells["paracaption"].Value = _oSelPop.pReturnArray[4].ToString();
                        dgvParaRep.CurrentRow.Cells["queryid"].Value = _oSelPop.pReturnArray[2].ToString();
                        dgvParaRep.CurrentRow.Cells["paramtype"].Value = _oSelPop.pReturnArray[3].ToString();
                        dgvParaRep.CurrentRow.Cells["paramname"].Value = _oSelPop.pReturnArray[1].ToString();
                        dgvParaRep.CurrentRow.Cells["Para_Order"].Value = 0;  // Added by Sachin N. S. on 01/07/2013 for Bug-4524
                        ds.Tables["Para_Vw"].Rows[this.BindingContext[ds, "Para_vw"].Position]["RepQry"] = _oSelPop.pReturnArray[5].ToString();  // Added by Sachin N. S. on 01/07/2013 for Bug-4524
                        rtbsqlqry.Text = _oSelPop.pReturnArray[5].ToString();  // Added by Sachin N. S. on 01/07/2013 for Bug-4524
                    }
                    string ch = dgvParaRep.CurrentRow.Cells["paracaption"].Value.ToString();
                    if (dgvParaRep.CurrentRow.Cells["Paramtype"].Value.ToString() == "2")
                    {
                        if (ch.StartsWith("F") || ch.StartsWith("A"))
                        {
                            dgvParaRep.CurrentRow.Cells["paramvalue"].Value = "01/01/1900";
                        }
                        else
                        {
                            dgvParaRep.CurrentRow.Cells["paramvalue"].Value = "01/01/2015";
                        }
                    }
                    else
                    {
                        if (ch.StartsWith("F"))
                        {
                            dgvParaRep.CurrentRow.Cells["paramvalue"].Value = "a";
                        }
                        else
                        {
                            dgvParaRep.CurrentRow.Cells["paramvalue"].Value = "z";
                        }
                    }
                    SendKeys.Send("{tab}");
                    cmdParacap.Visible = false;
                    dgvParaRep.Focus();
                    this.Refresh();
                }
                else        // Added by Sachin N. S. on 04/06/2013 for Bug-4524 -- Start
                {
                    dgvParaRep.Rows.Remove(dgvParaRep.CurrentRow);
                }           // Added by Sachin N. S. on 04/06/2013 for Bug-4524 -- End
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion "Parameter Tab -- Control Events"

        #region "Report Level Tab -- Control Events"

        private void cmdLvlTy_Click(object sender, EventArgs e)
        {
            //**** Commented by Sachin N. S. on 31/05/2013 for Bug-4524 -- Start ****//
            //frmSearchForm ft = new frmSearchForm(func.getConnection(), "Select Report Level Type", "uslty", "lvlty", "lvlty:Report Level Type", "", "lvlty");
            //DialogResult res = ft.ShowDialog();
            //if (res == DialogResult.OK)
            //{
            //    txtLvlTyp.Text = ft.ReturnString[0];
            //}
            //else
            //{
            //    txtLvlTyp.Text = ft.ReturnString[0];
            //}
            //**** Commented by Sachin N. S. on 31/05/2013 for Bug-4524 -- End ****//

            //**** Added by Sachin N. S. on 31/05/2013 for Bug-4524 -- Start ****//
            string cSql = "";
            string cFrmCap = "", cSrchCol = "", cDispCol = "", cRetCol = "";
            DataTable _dt = new DataTable();

            udSelectPop.SELECTPOPUP _oSelPop = new udSelectPop.SELECTPOPUP();
            cSql = "Select distinct lvlty From uslty ";
            cFrmCap = "Select Report Level Type";
            cSrchCol = "lvlty";
            cDispCol = "lvlty:Report Level Type";
            cRetCol = "lvlty";
            _dt = _oDataAccess.GetDataTable(cSql, null, 50);
            DataView _dvw = _dt.DefaultView;

            _oSelPop.pdataview = _dvw;
            _oSelPop.pformtext = cFrmCap;
            _oSelPop.psearchcol = cSrchCol;
            _oSelPop.pDisplayColumnList = cDispCol;
            _oSelPop.pRetcolList = cRetCol;
            _oSelPop.ShowDialog();
            try
            {
                if (_oSelPop.pReturnArray != null)
                {
                    txtLvlTyp.Text = _oSelPop.pReturnArray[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //**** Added by Sachin N. S. on 31/05/2013 for Bug-4524 -- End ****//
        }

        private void cmdAddlvl_Click(object sender, EventArgs e)
        {
            if (cmdAddlvl.Text == "&Add Level")
            {
                cmdAddlvl.Text = "S&ave Level";
                cmdRemovelvl.Text = "Ca&ncel Level";
                cmdEditlvl.Enabled = false;
                AutoLvl(ds.Tables["replvl_vw"]);
                AutoQry(ds.Tables["replvl_vw"]);
                AutoCol(ds.Tables["col_vw"]);
                txtLvlNm.Text = string.Empty;
                txtLvlTyp.Text = "Grid View";
                rtbQry.Text = string.Empty;
                cmdGenerate.Enabled = true;
                ctrlBindclrChild();
                dgvCol.DataSource = null;
                txtLvlNm.Focus();
                addlvl = true;
                editlvl = false;
                txtLvlNm.ReadOnly = false;
                rtbQry.ReadOnly = false;
                dgvCol.ReadOnly = false;
                cmdLvlTy.Enabled = true;    // Added by Sachin N. S. on 05/06/2013 for Bug-4524
                lvlNum += 1;            // Added by Sachin N. S. on 05/08/2013 for Bug-4524
                vdgvcol = false;
            }
            else
            {
                //func.getConnection();     // Commented by Sachin N. S. on 09/03/2013
                try
                {
                    if (txtLvlNm.Text == "" || rtbQry.Text == "")
                    {
                        MessageBox.Show("Query level name or the Sql query is not defined.", _msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        return;
                        this.rtbQry.Focus();
                    }
                    else
                    {
                        if (addlvl == true)
                        {
                            addlvl = false;     // ****** Added by Sachin N. S. on 05/06/2013 for Bug-4524
                            replvl = ds.Tables["replvl_vw"].NewRow();
                            replvl["RepLvlId"] = txtQryLvl.Text;
                            replvl["lvlty"] = txtLvlTyp.Text;
                            replvl["lvlnm"] = txtLvlNm.Text;
                            if (lvlcount > 0)
                            {
                                for (int i = 0; i < ds.Tables["replvl_vw"].Rows.Count; i++)
                                {
                                    if (ds.Tables["replvl_vw"].Rows[i]["lvlnm"].ToString() != txtLvlNm.Text.ToString())
                                    {
                                        replvl["lvlnm"] = txtLvlNm.Text;
                                    }
                                    else
                                    {
                                        MessageBox.Show("Duplicate Level Name \n.... could Not save", _msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                                        addlvl = true;     // ****** Added by Sachin N. S. on 08/08/2013 for Bug-4524
                                        txtLvlNm.Focus();
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                replvl["lvlnm"] = txtLvlNm.Text;
                            }
                            if (rtbQry.Text.Contains("'"))
                            {
                                StringBuilder B = new StringBuilder(rtbQry.Text);
                                B.Replace("'", "''");
                                replvl["repqry"] = B.ToString();
                            }
                            else
                                replvl["repqry"] = rtbQry.Text;

                            replvl["qryid"] = maxQryid;
                            replvl["repid"] = txtRepId.Text;
                            bool lDefinePriKey = false;
                            if (dgvCol.Rows.Count > 0)
                            {
                                for (int i = 0; i < dgvCol.Rows.Count; i++)
                                {
                                    AutoCol(ds.Tables["col_vw"]);
                                    col = ds.Tables["col_vw"].NewRow();
                                    col["columnnames"] = dgvCol.Rows[i].Cells["colname"].Value.ToString();
                                    col["columncaption"] = dgvCol.Rows[i].Cells["colcaption"].Value.ToString();
                                    col["columndatatype"] = dgvCol.Rows[i].Cells["coldatatype"].Value.ToString();
                                    col["columnorder"] = dgvCol.Rows[i].Cells["colorder"].Value.ToString();
                                    col["isdisplayed"] = dgvCol.Rows[i].Cells["display"].Value;
                                    if (dgvCol.Rows[i].Cells["freeze"].Value == null)
                                        col["isfreezing"] = false;
                                    else
                                        col["isfreezing"] = dgvCol.Rows[i].Cells["freeze"].Value;
                                    col["colwidth"] = dgvCol.Rows[i].Cells["colwidth"].Value.ToString();
                                    if (dgvCol.Rows[i].Cells["group"].Value == null)
                                        col["isgrouped"] = false;
                                    else
                                        col["isgrouped"] = dgvCol.Rows[i].Cells["group"].Value;
                                    if (dgvCol.Rows[i].Cells["grporder"].Value == null)
                                        col["grouporder"] = 0;
                                    else
                                        col["grouporder"] = dgvCol.Rows[i].Cells["grporder"].Value;
                                    if (dgvCol.Rows[i].Cells["summary"].Value == null)
                                        col["issummury"] = false;
                                    else
                                        col["issummury"] = dgvCol.Rows[i].Cells["summary"].Value;
                                    if (dgvCol.Rows[i].Cells["Primarykey"].Value == null)
                                        col["Prycl"] = DBNull.Value;
                                    else
                                        col["Prycl"] = dgvCol.Rows[i].Cells["Primarykey"].Value;
                                    if (dgvCol.Rows[i].Cells["foreginkey"].Value == null)
                                        col["seccl"] = false;
                                    else
                                        col["seccl"] = dgvCol.Rows[i].Cells["foreginkey"].Value;
                                    col["qryid"] = maxQryid;
                                    col["colid"] = maxColid;
                                    col["repid"] = txtRepId.Text;
                                    col["RepLvlId"] = txtQryLvl.Text;   // Added by Sachin N. S. 0n 10/07/2013 for Bug-4524
                                    if (lvlcount == 0)
                                    {
                                        replvl["prycl"] = "";
                                        replvl["seccl"] = false;
                                    }
                                    else if (lvlcount > 0 && dgvCol.Rows[i].Cells["Primarykey"].Value != null && dgvCol.Rows[i].Cells["Primarykey"].Value.ToString() != "")
                                    {
                                        if (lDefinePriKey == false)
                                        {
                                            replvl["prycl"] = dgvCol.Rows[i].Cells["Primarykey"].Value;
                                            replvl["seccl"] = dgvCol.Rows[i].Cells["colname"].Value;
                                            lDefinePriKey = true;
                                        }
                                        else
                                        {
                                            replvl["prycl"] = dgvCol.Rows[i].Cells["Primarykey"].Value;
                                            replvl["seccl"] = dgvCol.Rows[i].Cells["colname"].Value;
                                            MessageBox.Show("Only one Primary Key can be defined for a relation", _msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                                            ds.Tables["col_vw"].RejectChanges();
                                            addlvl = true;     // ****** Added by Sachin N. S. on 08/08/2013 for Bug-4524
                                            return;
                                        }
                                    }
                                    ds.Tables["col_vw"].Rows.Add(col);
                                }
                                if (lDefinePriKey == false && lvlcount > 0)
                                {
                                    ds.Tables["col_vw"].RejectChanges();
                                    MessageBox.Show("Please Enter Primary Key.....", _msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                                    addlvl = true;     // ****** Added by Sachin N. S. on 08/08/2013 for Bug-4524
                                    return;
                                }
                                ds.Tables["replvl_vw"].Rows.Add(replvl);
                                addlvl = false;     // ****** Added by Sachin N. S. on 05/06/2013 for Bug-4524
                            }
                            else
                            {
                                addlvl = true;     // ****** Added by Sachin N. S. on 05/06/2013 for Bug-4524
                                MessageBox.Show("Please Generate the Columns.....", _msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                                cmdGenerate.Focus();
                                return;
                            }
                        }
                        else
                        {
                            if (ds.Tables["replvl_vw"].Rows.Count > 0)
                            {
                                DataRow[] replvl = ds.Tables["replvl_vw"].Select("RepId=" + txtRepId.Text + " And RepLvlId=" + txtQryLvl.Text);
                                replvl[0]["lvlty"] = txtLvlTyp.Text;
                                replvl[0]["lvlnm"] = txtLvlNm.Text;
                                replvl[0]["repqry"] = rtbQry.Text;
                                if (ds.Tables["col_vw"].Rows.Count > 0)
                                {
                                    for (int i = 0; i < dgvCol.Rows.Count; i++)
                                    {
                                        DataRow[] col = ds.Tables["col_vw"].Select("RepId=" + txtRepId.Text + " And colid=" + dgvCol.Rows[i].Cells["colid"].Value);
                                        col[0]["columnnames"] = dgvCol.Rows[i].Cells["colname"].Value.ToString();
                                        col[0]["columncaption"] = dgvCol.Rows[i].Cells["colcaption"].Value.ToString();
                                        col[0]["columndatatype"] = dgvCol.Rows[i].Cells["coldatatype"].Value.ToString();
                                        col[0]["columnorder"] = dgvCol.Rows[i].Cells["colorder"].Value.ToString();
                                        col[0]["isdisplayed"] = dgvCol.Rows[i].Cells["display"].Value;
                                        if (dgvCol.Rows[i].Cells["freeze"].Value == null)
                                            col[0]["isfreezing"] = false;
                                        else
                                            col[0]["isfreezing"] = dgvCol.Rows[i].Cells["freeze"].Value;
                                        col[0]["colwidth"] = dgvCol.Rows[i].Cells["colwidth"].Value.ToString();
                                        if (dgvCol.Rows[i].Cells["group"].Value == null)
                                            col[0]["isgrouped"] = false;
                                        else
                                            col[0]["isgrouped"] = dgvCol.Rows[i].Cells["group"].Value;
                                        if (dgvCol.Rows[i].Cells["grporder"].Value == null)
                                            col[0]["grouporder"] = 0;
                                        else
                                            col[0]["grouporder"] = dgvCol.Rows[i].Cells["grporder"].Value;
                                        if (dgvCol.Rows[i].Cells["summary"].Value == null)
                                            col[0]["issummury"] = false;
                                        else
                                            col[0]["issummury"] = dgvCol.Rows[i].Cells["summary"].Value;
                                        if (dgvCol.Rows[i].Cells["Primarykey"].Value == null)
                                            col[0]["Prycl"] = DBNull.Value;
                                        else
                                            col[0]["Prycl"] = dgvCol.Rows[i].Cells["Primarykey"].Value;
                                        if (dgvCol.Rows[i].Cells["foreginkey"].Value == null)
                                            col[0]["seccl"] = false;
                                        else
                                            col[0]["seccl"] = dgvCol.Rows[i].Cells["foreginkey"].Value;

                                        if (col[0]["Prycl"] != null && col[0]["Prycl"].ToString() != "")
                                        {
                                            replvl[0]["prycl"] = dgvCol.Rows[i].Cells["Primarykey"].Value;
                                            replvl[0]["seccl"] = dgvCol.Rows[i].Cells["colname"].Value;
                                        }
                                    }
                                }
                            }
                        }
                        cmdGenerate.Enabled = false;
                        cmdEditlvl.Enabled = true;
                        ds.Tables["col_vw"].AcceptChanges();
                        ds.Tables["replvl_vw"].AcceptChanges();
                        cmdAddlvl.Text = "&Add Level";
                        cmdRemovelvl.Text = "&Remove Level";
                        BindControlsChildlvl();
                        dgvCol.DataSource = ds.Tables["col_vw"];
                        dgvCol.Columns[0].ReadOnly = true;
                        dgvCol.Columns[0].DataPropertyName = "columnnames";
                        dgvCol.Columns[1].DataPropertyName = "columncaption";
                        dgvCol.Columns[2].DataPropertyName = "columnDataType";
                        dgvCol.Columns[3].DataPropertyName = "columnorder";
                        dgvCol.Columns[4].DataPropertyName = "isdisplayed";
                        dgvCol.Columns[5].DataPropertyName = "isfreezing";
                        dgvCol.Columns[6].DataPropertyName = "colwidth";
                        dgvCol.Columns[7].DataPropertyName = "isgrouped";
                        dgvCol.Columns[8].DataPropertyName = "grouporder";
                        dgvCol.Columns[9].DataPropertyName = "issummury";
                        dgvCol.Columns[10].DataPropertyName = "prycl";
                        dgvCol.Columns[11].DataPropertyName = "seccl";
                        dgvCol.Columns[12].DataPropertyName = "Precision";
                        dgvCol.RefreshEdit();
                        dgvCol.Refresh();
                        this.NavigateRepLvl("first");
                        lvlcount = lvlcount + 1;
                        dgvCol.ReadOnly = true;
                    }
                }
                catch (Exception ex)
                {
                    ds.Tables["col_vw"].RejectChanges();
                    ds.Tables["replvl_vw"].AcceptChanges();
                    throw ex;
                }
                //****** Added by Sachin N. S. on 05/06/2013 for Bug-4524 -- Start ******//
                addlvl = false;
                editlvl = false;
                txtLvlNm.ReadOnly = true;
                rtbQry.ReadOnly = true;
                dgvCol.ReadOnly = true;
                cmdLvlTy.Enabled = false;
                //****** Added by Sachin N. S. on 05/06/2013 for Bug-4524 -- End ******//
            }
        }

        private void cmdEditlvl_Click(object sender, EventArgs e)
        {
            cmdAddlvl.Text = "S&ave Level";
            cmdRemovelvl.Text = "Ca&ncel Level";
            addlvl = false;
            editlvl = true;
            cmdGenerate.Enabled = true;
            ctrlBindclrChild();
            txtLvlNm.ReadOnly = false;
            rtbQry.ReadOnly = false;
            dgvCol.ReadOnly = false;
            cmdEditlvl.Enabled = false;
        }

        private void cmdRemovelvl_Click(object sender, EventArgs e)
        {
            if (cmdRemovelvl.Text == "&Remove Level")
            {
                if (txtQryLvl.Text != "")
                {
                    DialogResult x = MessageBox.Show("Do you really want to remove the record?", _msgCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (x == DialogResult.Yes)
                    {
                        ds.Tables["Col_vw"].Rows.Cast<DataRow>().Where(r => r["RepLvlId"].ToString().Trim() == txtQryLvl.Text.Trim()).ToList().ForEach(r => r.Delete());    // Added by Sachin N. S. on 04/06/2013 for Bug-4524
                        DataRow[] row = ds.Tables["replvl_vw"].Select("RepLvlId=" + txtQryLvl.Text);
                        ds.Tables["replvl_vw"].Rows.Remove(row[0]);
                        cmdGenerate.Enabled = false;
                        ds.Tables["replvl_vw"].AcceptChanges();
                        ds.Tables["Col_vw"].AcceptChanges();     // Added by Sachin N. S. on 04/06/2013 for Bug-4524
                        lvlcount -= 1;      // Added by Sachin N. S. on 10/07/2013 for Bug-4524
                        lvlNum -= 1;
                        this.dgvCol.Refresh();
                        this.NavigateRepLvl("Previous");
                    }
                }
            }
            else
            {
                cmdAddlvl.Text = "&Add Level";
                cmdRemovelvl.Text = "&Remove Level";

                clearlvl();
                ctrlBindclrChild();
                this.BindControlsChildlvl();
                this.NavigateRepLvl("First");
                cmdGenerate.Enabled = false;
                cmdEditlvl.Enabled = true;
                lvlNum -= 1;            // Added by Sachin N. S. on 05/08/2013 for Bug-4524
            }
        }

        #region "Report Level Navigation -- Button Events"

        private void cmdFirst_Click(object sender, EventArgs e)
        {
            this.NavigateRepLvl("First");
        }

        private void cmdPrev_Click(object sender, EventArgs e)
        {
            this.NavigateRepLvl("Previous");
        }

        private void cmdNext_Click(object sender, EventArgs e)
        {
            this.NavigateRepLvl("Next");
        }

        private void cmdLast_Click(object sender, EventArgs e)
        {
            this.NavigateRepLvl("Last");
        }

        #endregion"Report Level Navigation -- Button Events"

        private void cmdGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                // Changed by Sachin N. S. 09/03/2013 -- Start
                string csql = "";
                List<clsParam> _clParam = new List<clsParam>();
                clsParam _objParam;
                csql = rtbQry.Text;
                if (csql.ToString() == "")
                {
                    MessageBox.Show("To generate columns proper Sql Query/Stored Procedure is required. Cannot continue...!!!", _msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    return;
                }

                if (csql.Contains("@"))
                {
                    clsParam.pType paramType = clsParam.pType.pString;

                    int DeleteValue = csql.ToUpper().IndexOf("where".ToUpper());
                    DeleteValue = DeleteValue >= 0 ? DeleteValue : csql.ToUpper().IndexOf("EXECUTE") >= 0 ? csql.ToUpper().IndexOf("EXECUTE") : csql.ToUpper().IndexOf("EXEC") >= 0 ? csql.ToUpper().IndexOf("EXEC") : 0;   // Added by Sachin N. S. on 07/08/2013 for Bug-4524
                    string para = string.Empty;
                    if (DeleteValue >= 0)
                    {
                        DeleteValue = csql.ToUpper().IndexOf("@") - 1;
                        for (int i = 0; i < ds.Tables["para_vw"].Rows.Count; i++)
                        {
                            dr = ds.Tables["para_vw"].Rows[i];
                            csql = csql.Replace("@" + ds.Tables["para_vw"].Rows[i]["ParamName"].ToString(), "?");       // Added by Sachin N. S. on 07/08/2013 for Bug-4524
                            _objParam = new clsParam(ds.Tables["para_vw"].Rows[i]["ParamName"].ToString(), dr["ParamValue"], paramType, _clParam, false, null, clsParam.pInOut.pIn);
                            //                            sqlCommand.Parameters.AddWithValue("@" + dgvParaRep.Rows[i].Cells["ParamName"].Value, dgvParaRep.Rows[i].Cells["paramvalue"].Value);
                        }
                    }
                }
                dt = new DataTable();
                //CharacterToFind = "@";
                CharacterToFind = "\\?";  // Changed by Sachin N. S. on 07/08/2013 for Bug-4524
                Regex exp = new Regex(CharacterToFind, RegexOptions.IgnoreCase);
                int noOfOccurrences = exp.Matches(csql.ToString()).Count;
                int NoParamteresCount = _clParam.Count;
                try
                {
                    if (noOfOccurrences == NoParamteresCount || (noOfOccurrences == 0 && NoParamteresCount == 0))
                    {
                        try
                        {
                            //dt = _oDataAccess.GetDataTable(csql, _clParam, 25);
                            dt = _oDataAccess.GetDataTable(csql, _clParam, 25);     // Changed by Sachin N. S. on 07/08/2013 for Bug-4524
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, _msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            return;
                        }
                        //da = new SqlDataAdapter(sqlCommand);
                        //da.Fill(ds2);
                    }
                    else
                    {
                        MessageBox.Show("Number of parameters are not matching.", _msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Please enter proper sql query.. \n \n Error : " + ex.Message, _msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return;
                }

                DataColumnCollection column = dt.Columns;
                var numericTypes = new[] {"Byte", "Decimal", "Double",
                     "Int16", "Int64", "SByte",
                     "Single", "UInt16", "UInt32", "UInt64"};

                // ****** Added by Sachin N. S. on 03/06/2013 for Bug-4524 -- Start ****** //
                List<DataGridViewRow> _rowNum = new List<DataGridViewRow>();
                for (int i = 0; i < dgvCol.Rows.Count; i++)
                {
                    #region Remove field
                    bool lltrue = false;
                    lltrue = false;
                    for (int k = 0; k < dt.Columns.Count; k++)
                    {
                        if (column[k].ColumnName.ToString().ToUpper() == dgvCol.Rows[i].Cells["colname"].Value.ToString().ToUpper())
                        {
                            lltrue = true;
                            break;
                        }
                    }
                    if (lltrue == false)
                    {
                        _rowNum.Add(dgvCol.Rows[i]);
                        //dgvCol.Rows.RemoveAt(i);
                    }
                    #endregion
                }
                foreach (DataGridViewRow _dr in _rowNum)
                {
                    dgvCol.Rows.Remove(_dr);
                }
                // ****** Added by Sachin N. S. on 03/06/2013 for Bug-4524 -- End ****** //

                for (int i = 0; i < dt.Columns.Count; i++)
                {

                    // ****** Commented by Sachin N. S. on 03/06/2013 for Bug-4524 -- Start ****** //
                    //10/2/13 start
                    //DataTable _dtcolvw = ds.Tables["col_vw"];
                    //#region Remove field
                    bool lltrue = false;
                    //lltrue = false;
                    //for (int k = 0; k < dgvCol.Rows.Count; k++)
                    //{
                    //    if (column[i].ColumnName.ToString() == dgvCol.Rows[k].Cells["colname"].Value.ToString())
                    //    {
                    //        lltrue = true;
                    //        break;
                    //    }
                    //}
                    //if (lltrue == false)
                    //{
                    //    DataRow[] row = ds.Tables["col_vw"].Select("columnnames='" + column[i].ColumnName.ToString() + "'");
                    //    if (row.Count() > 0)
                    //    {
                    //        ds.Tables["col_vw"].Rows.Remove(row[0]);
                    //    }
                    //}
                    //#endregion
                    // ****** Added by Sachin N. S. on 03/06/2013 for Bug-4524 -- End ****** //

                    #region Add New field
                    lltrue = false;
                    for (int l = 0; l < dgvCol.Rows.Count; l++)
                    {
                        if (column[i].ColumnName.ToString().ToUpper() == dgvCol.Rows[l].Cells["colname"].Value.ToString().ToUpper())
                        {
                            lltrue = true;
                            break;
                        }
                    }
                    if (lltrue == false)
                    {
                        dgvCol.Rows.Add();
                        int _nRw = dgvCol.Rows.Count - 1;
                        dgvCol.Rows[_nRw].Cells["colName"].Value = column[i].ColumnName;
                        dgvCol.Rows[_nRw].Cells["colcaption"].Value = column[i].Caption;

                        if (column[i].DataType.Name == "String")
                        {
                            dgvCol.Rows[_nRw].Cells["colDatatype"].Value = "Varchar";
                        }
                        else if (column[i].DataType.Name == "Int32")
                        {
                            dgvCol.Rows[_nRw].Cells["colDatatype"].Value = "Int";
                        }
                        else if (column[i].DataType.Name == "Boolean")
                        {
                            dgvCol.Rows[_nRw].Cells["colDatatype"].Value = "Bit";
                        }
                        else if (column[i].DataType.Name == "DateTime")
                        {
                            dgvCol.Rows[_nRw].Cells["colDatatype"].Value = "DateTime";
                        }
                        else if (column[i].DataType.Name == "Byte[]")      //**** Added by Sachin N. S. on 04/06/2013 for Bug-4524 -- Start ****//
                        {
                            dgvCol.Rows[_nRw].Cells["colDatatype"].Value = "Varbinary";
                        }                                                       //**** Added by Sachin N. S. on 04/06/2013 for Bug-4524 -- End ****//
                        else
                        {
                            for (int num = 0; num < numericTypes.Length; num++)
                            {
                                if (column[i].DataType.Name == numericTypes[num].ToString())
                                {
                                    dgvCol.Rows[_nRw].Cells["colDatatype"].Value = "Numeric";
                                }
                            }
                        }
                        dgvCol.Rows[_nRw].Cells["colorder"].Value = _nRw + 1;
                        dgvCol.Rows[_nRw].Cells["Display"].Value = 1;
                        dgvCol.Rows[_nRw].Cells["colwidth"].Value = 250;
                    }
                    #endregion
                }

                dgvCol.Refresh();
                if (lvlNum == 0)        // Changed by Sachin N. S. on 05/08/2013 for Bug-4524
                {
                    dgvCol.Columns[11].ReadOnly = true;
                }
                //addlvl = false; // ****** Commented by Sachin N. S. on 05/06/2013 for bug-4524
            }

            catch (Exception ex)
            {
                MessageBox.Show("Please enter proper sql query.. \n \n Error : " + ex.Message, _msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void dgvCol_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((add == true || edit == true) && (addlvl == true || editlvl == true))
            {
                if (e.ColumnIndex == 10)
                {
                    if (lvlNum > 1)     // Changed by Sachin N. S. on 05/08/2013 for Bug-4524
                    {
                        cboPk.Visible = true;
                        int x = dgvCol.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false).Right + dgvCol.Left - 95;
                        int y = dgvCol.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false).Top + dgvCol.Bottom - 10;
                        this.cboPk.Location = new Point(x, y);
                        rowindx = e.RowIndex;
                        colindx = e.ColumnIndex;

                        //***** Changed by Sachin N. S. on 05/08/2013 for Bug-4524 -- Start *****//
                        string _replvlId = ds.Tables["RepLvl_vw"].AsEnumerable().Min(row => row["ReplvlId"]).ToString();
                        DataRow[] _dr = ds.Tables["Col_vw"].Select("ReplvlId = " + _replvlId.ToString());
                        foreach (DataRow _dr1 in _dr)
                        {
                            if (dgvCol.CurrentRow.Cells["ColDatatype"].Value.ToString() == _dr1["ColumnDatatype"].ToString())
                            {
                                cboPk.Items.Add(_dr1["columnNames"].ToString());
                            }
                        }
                        //***** Changed by Sachin N. S. on 05/08/2013 for Bug-4524 -- End *****//
                    }
                }
                else
                {
                    cboPk.Visible = false;
                }
                //***** Changed by Sachin N. S. on 05/08/2013 for Bug-4524 -- Start *****//
                //for (int i = 0; i < dgvCol.Rows.Count; i++)
                //{
                //    if (dgvCol.CurrentRow.Cells["ColDatatype"].Value.ToString() == dgvCol.Rows[i].Cells["ColDatatype"].Value.ToString())
                //    {
                //        cboPk.Items.Add(dgvCol.Rows[i].Cells["colName"].Value.ToString());
                //    }
                //}
                //***** Changed by Sachin N. S. on 05/08/2013 for Bug-4524 -- End *****//
            }
        }

        private void dgvCol_CurrentCellChanged(object sender, EventArgs e)
        {
            cboPk.Items.Clear();
        }

        #endregion "Report Level Tab -- Control Events"

        #region Numeric value Checked
        private void dgvCol_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            //TextBox txtEdit = (TextBox)e.Control;
            //txtEdit.KeyPress += txtEdit_KeyPress;
            //vdgvcol = true;
        }

        private void txtEdit_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (vdgvcol == true)
            {
                if (dgvCol.CurrentCell.ColumnIndex == 4 || dgvCol.CurrentCell.ColumnIndex == 6 || dgvCol.CurrentCell.ColumnIndex == 8)
                {
                    int result;
                    if (int.TryParse(e.KeyChar.ToString(), out result) || e.KeyChar == (char)Keys.Back)
                    {
                        e.Handled = false;
                        return;
                    }
                    else
                    {
                        //  MessageBox.Show("Enter only numbers");
                        e.Handled = true;
                    }
                }
            }
            else
            {
                if (dgvParaRep.CurrentCell.ColumnIndex == 4)
                {
                    int result;
                    if (int.TryParse(e.KeyChar.ToString(), out result) || e.KeyChar == (char)Keys.Back)
                    {
                        e.Handled = false;
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Enter only numbers");
                        e.Handled = true;
                    }
                }
            }

        }

        #endregion

        #region Generate Process Id's       // Added by Sachin N. S. on 08/07/2013 for Bug-4524 -- Start
        private void mInsertProcessIdRecord()/*Added Rup 07/03/2011*/
        {
            _oDataAccess = new clsDataAccess();
            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "udDadosRepWizard.exe";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            sqlstr = "Set DateFormat dmy insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + ApplCode + "','" + DateTime.Now.Date.ToString() + "','" + cAppName + "'," + ApplPId + ",'" + AppCaption + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";
            _oDataAccess.ExecuteSQLStatement(sqlstr, null, 20, true);
        }
        private void mDeleteProcessIdRecord()/*Added Rup 07/03/2011*/
        {
            _oDataAccess = new clsDataAccess();
            if (string.IsNullOrEmpty(cAppName) || ApplPId == 0 || string.IsNullOrEmpty(this.cAppName) || string.IsNullOrEmpty(this.cAppPId))
            {
                return;
            }
            DataSet dsData = new DataSet();
            string sqlstr;
            sqlstr = " Delete from vudyog..ExtApplLog where pApplNm='" + cAppName + "' and pApplId=" + ApplPId + " and cApplNm= '" + cAppName + "' and cApplId= " + cAppPId;
            _oDataAccess.ExecuteSQLStatement(sqlstr, null, 20, true);
        }
        #endregion Generate Process Id's    // Added by Sachin N. S. on 08/07/2013 for Bug-4524 -- End


        private void cboPk_SelectionChangeCommitted(object sender, EventArgs e)
        {
            dgvCol.Rows[rowindx].Cells[colindx].Value = cboPk.Text.ToString();
            dgvCol.RefreshEdit();
            SendKeys.Send("{tab}");
            cboPk.Visible = false;
        }

        private void frmDadosReport_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.mDeleteProcessIdRecord();
        }

        //****** Added by Sachin N. S. on 05/08/2013 for Bug-4524 -- Start ******//
        private int SaveEditRecords()
        {
            int _retVal = 0;
            _oDataAccess.BeginTransaction();
            string deleteSql = "delete from para_query_master where repid=" + txtRepId.Text;
            _retVal = _oDataAccess.ExecuteSQLStatement(deleteSql, null, 20, true);

            DataTable _dt = new DataTable();
            deleteSql = "select b.lvltyp from usrlv b inner join usqry c on b.qryid=c.qryid where c.repid='" + txtRepId.Text + "'";
            _dt = _oDataAccess.GetDataTable(deleteSql, null, 20);

            if (_retVal > 0)
            {
                deleteSql = "delete from usrlv where qryid in(select qryid from usqry where repid=" + txtRepId.Text + ")";
                _retVal = _oDataAccess.ExecuteSQLStatement(deleteSql, null, 20, true);
            }
            string _lvlid = string.Join(",", _dt.Rows.OfType<DataRow>().Select(rw => rw["lvltyp"].ToString()).ToArray());
            if (_retVal > 0 && _lvlid != "")
            {
                deleteSql = "delete from uslty where lvltid in(" + _lvlid + ") and lvltid not in (select b.lvltyp from usrlv b inner join usqry c on b.qryid=c.qryid where c.repid!='" + txtRepId.Text + "')";
                _retVal = _oDataAccess.ExecuteSQLStatement(deleteSql, null, 20, true);
            }
            if (_retVal > 0)
            {
                deleteSql = "delete from uscol where colid in(select colid from uscrl where repid=" + txtRepId.Text + ")";
                _retVal = _oDataAccess.ExecuteSQLStatement(deleteSql, null, 20, true);
            }
            if (_retVal > 0)
            {
                deleteSql = "delete from uscrl where repid=" + txtRepId.Text;
                _retVal = _oDataAccess.ExecuteSQLStatement(deleteSql, null, 20, true);
            }
            if (_retVal > 0)
            {
                deleteSql = "delete from usqry where repid=" + txtRepId.Text;
                _retVal = _oDataAccess.ExecuteSQLStatement(deleteSql, null, 20, true);
            }

            if (_retVal > 0)
            {
                for (int i = 0; i < ds.Tables["Para_vw"].Rows.Count; i++)
                {
                    if (ds.Tables["Para_vw"].Rows[i]["ParameterID"] == null)
                        strParaid = Convert.ToString(DBNull.Value);
                    else
                        strParaid = ds.Tables["Para_vw"].Rows[i]["ParameterID"].ToString();
                    if (ds.Tables["Para_vw"].Rows[i]["QueryId"] == null)
                        strQryid = Convert.ToString(DBNull.Value);
                    else
                        strQryid = ds.Tables["Para_vw"].Rows[i]["QueryId"].ToString();
                    if (ds.Tables["Para_vw"].Rows[i]["Para_order"] == null)
                        strDisor = Convert.ToString(DBNull.Value);
                    else
                        strDisor = ds.Tables["Para_vw"].Rows[i]["Para_Order"].ToString();
                    if (ds.Tables["Para_vw"].Rows[i]["ParamType"] == null)
                        paratype = Convert.ToString(DBNull.Value);
                    else
                        paratype = ds.Tables["Para_vw"].Rows[i]["ParamType"].ToString();
                    if (ds.Tables["Para_vw"].Rows[i]["Paramvalue"] == null)
                        strparamvalue = Convert.ToString(DBNull.Value);
                    else
                        strparamvalue = ds.Tables["Para_vw"].Rows[i]["Paramvalue"].ToString();
                    string strqr1 = "Insert into para_query_master(parameterid,queryid,paramvalue,repid,Para_Order) values('" + strParaid + "','" + strQryid + "','" + strparamvalue + "','" + txtRepId.Text + "'," + strDisor + ")";
                    _retVal = _oDataAccess.ExecuteSQLStatement(strqr1, null, 20, true);
                }
            }

            if (_retVal > 0)
            {
                for (int i = 0; i < ds.Tables["replvl_vw"].Rows.Count; i++)
                {
                    string strqr2 = "Insert into usqry values(" + ds.Tables["replvl_vw"].Rows[i]["qryid"] + ",'" + txtRepId.Text + "'," + ds.Tables["replvl_vw"].Rows[i]["replvlid"] + ",'" + ds.Tables["replvl_vw"].Rows[i]["repqry"] + "')";
                    _retVal = _oDataAccess.ExecuteSQLStatement(strqr2, null, 20, true);
                    if (_retVal > 0)
                    {
                        string strqr4 = "Insert into uslty values('" + ds.Tables["replvl_vw"].Rows[i]["lvlty"] + "','" + ds.Tables["replvl_vw"].Rows[i]["lvlnm"] + "'," + ds.Tables["replvl_vw"].Rows[i]["RepLvlId"] + ")";
                        _retVal = _oDataAccess.ExecuteSQLStatement(strqr4, null, 20, true);
                    }
                    if (_retVal > 0)
                    {
                        string strqr3 = "Insert into usrlv values(" + ds.Tables["replvl_vw"].Rows[i]["replvlid"] + "," + ds.Tables["replvl_vw"].Rows[i]["qryid"] + "," + ds.Tables["replvl_vw"].Rows[i]["RepLvlId"] + ",'" + ds.Tables["replvl_vw"].Rows[i]["prycl"] + "','" + ds.Tables["replvl_vw"].Rows[i]["seccl"] + "')";
                        _retVal = _oDataAccess.ExecuteSQLStatement(strqr3, null, 20, true);
                    }
                    if (_retVal > 0)
                    {
                        for (int j = 0; j < ds.Tables["col_vw"].Rows.Count; j++)
                        {
                            if (Convert.ToInt32(ds.Tables["replvl_vw"].Rows[i]["qryid"]) == Convert.ToInt32(ds.Tables["col_vw"].Rows[j]["qryid"]))
                            {
                                if (_retVal > 0)
                                {
                                    string strqr5 = "Insert into uscrl values(" + ds.Tables["col_vw"].Rows[j]["colid"] + "," + ds.Tables["replvl_vw"].Rows[i]["qryid"] + ",'" + txtRepId.Text + "'," + ds.Tables["replvl_vw"].Rows[i]["replvlid"] + ",'" + ds.Tables["col_vw"].Rows[j]["isgrouped"] + "','" + ds.Tables["col_vw"].Rows[j]["isfreezing"] + "'," + ds.Tables["col_vw"].Rows[j]["colwidth"] + ",'" + ds.Tables["col_vw"].Rows[j]["issummury"] + "','" + ds.Tables["col_vw"].Rows[j]["isdisplayed"] + "'," + ds.Tables["col_vw"].Rows[j]["grouporder"] + ")";
                                    _retVal = _oDataAccess.ExecuteSQLStatement(strqr5, null, 20, true);
                                }
                                if (_retVal > 0)
                                {
                                    string strqr6 = "Insert into uscol values(" + ds.Tables["col_vw"].Rows[j]["colid"] + ",'" + ds.Tables["col_vw"].Rows[j]["columnnames"] + "','" + ds.Tables["col_vw"].Rows[j]["columncaption"] + "','" + ds.Tables["col_vw"].Rows[j]["columndatatype"] + "'," + ds.Tables["col_vw"].Rows[j]["columnorder"] + ",'" + ds.Tables["col_vw"].Rows[j]["Precision"] + "')";
                                    _retVal = _oDataAccess.ExecuteSQLStatement(strqr6, null, 20, true);
                                }
                            }
                        }
                    }
                    if (_retVal <= 0)
                    {
                        break;
                    }
                }
            }

            if (_retVal > 0)
            {
                string updateSql;
                for (int i = 0; i < ds.Tables["rep_vw"].Rows.Count; i++)
                {
                    updateSql = "UPDATE usrep SET repnm ='" + ds.Tables["rep_vw"].Rows[i]["repnm"] + "',repty='" + ds.Tables["rep_vw"].Rows[i]["repty"] + "' WHERE repid ='" + ds.Tables["rep_vw"].Rows[i]["repid"] + "'";
                    _retVal = _oDataAccess.ExecuteSQLStatement(updateSql, null, 20, true);
                    if (_retVal <= 0)
                        break;
                }
            }
            if (_retVal > 0)
            {
                _oDataAccess.CommitTransaction();
                MessageBox.Show("Record updated successfully", _msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            else
            {
                _oDataAccess.RollbackTransaction();
                MessageBox.Show("Cannot update the record due to some problem.", _msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            return _retVal;
        }

        private void dgvCol_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (addlvl == true || editlvl == true)
            {
                if (lvlNum == 1)
                {
                    if (dgvCol.Columns[e.ColumnIndex].Name.ToUpper() == "PRIMARYKEY" || dgvCol.Columns[e.ColumnIndex].Name.ToUpper() == "FOREGINKEY")
                    {
                        dgvCol.Columns[e.ColumnIndex].ReadOnly = true;
                    }
                    else
                    {
                        dgvCol.Columns[e.ColumnIndex].ReadOnly = false;
                    }
                }
                else
                {
                    dgvCol.Columns[e.ColumnIndex].ReadOnly = false;
                }
            }
        }

        private void cmdGenScript_Click(object sender, EventArgs e)
        {
            string updateSql, _sqlScript;
            DataTable _dt = new DataTable();
            updateSql = "EXECUTE CREATE_DADOS_REP_SCRIPT_GENERATOR '" + ds.Tables["rep_vw"].Rows[0]["repid"] + "'";
            _dt = _oDataAccess.GetDataTable(updateSql, null, 3000);
            _sqlScript = _dt.Rows[0][0].ToString().Replace("\\n", "\n").Replace("\\t", "\t");
            string[] _sqlScript1 = _sqlScript.Split(new[] { "/n" }, StringSplitOptions.None);

            if (File.Exists(Application.StartupPath + "\\" + ds.Tables["rep_vw"].Rows[0]["repnm"].ToString().Trim() + ".sql") == true)
                File.Delete(Application.StartupPath + "\\" + ds.Tables["rep_vw"].Rows[0]["repnm"].ToString().Trim() + ".sql");

            using (StreamWriter sw = new StreamWriter(Application.StartupPath + "\\" + ds.Tables["rep_vw"].Rows[0]["repnm"].ToString().Trim() + ".sql"))
            {
                //foreach(string _str in _sqlScript1)
                sw.WriteLine(_sqlScript);
            }
            MessageBox.Show("File generated at " + Application.StartupPath + "\\" + ds.Tables["rep_vw"].Rows[0]["repnm"].ToString().Trim() + ".sql", _msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        //****** Added by Sachin N. S. on 05/08/2013 for Bug-4524 -- End ******//
    }
}



