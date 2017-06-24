using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;

namespace UeSMS
{
    public partial class frmParamValue : uBaseForm.FrmBaseForm
    {
        DataTable _dtURLParam;
        string Mode = "";
        int _rowIndex = 0;
        public frmParamValue(string[] args, DataTable _dtURLParam1, int _rwIndex, string _Mode)
        {
            InitializeComponent();
            this.pPApplPID = 0;
            this.pPara = args;
            this.pFrmCaption = "SMS Gateway Master";
            this.pCompId = Convert.ToInt16(args[0]);
            this.pComDbnm = args[1];
            this.pServerName = args[2];
            this.pUserId = args[3];
            this.pPassword = args[4];
            this.pPApplRange = args[5];
            this.pAppUerName = args[6];
            Icon MainIcon = new System.Drawing.Icon(args[7].Replace("<*#*>", " "));
            this.pFrmIcon = MainIcon;
            this.pPApplText = args[8].Replace("<*#*>", " ");
            this.pPApplName = args[9];
            this.pPApplPID = Convert.ToInt16(args[10]);
            this.pPApplCode = args[11];
            this._dtURLParam = _dtURLParam1;
            this.Mode = _Mode;
            this._rowIndex = _rwIndex;
        }

        private void frmParamValue_Load(object sender, EventArgs e)
        {
            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;

            enableDisablecontrol(Mode == "VIEW" ? false : true);
            setDataBinding();
        }

        private void enableDisablecontrol(bool _EnblDisable)
        {
            this.txtParamVal.Enabled = _EnblDisable;
        }

        private void setDataBinding()
        {
            this.txtParamNm.DataBindings.Clear();
            this.txtParamDesc.DataBindings.Clear();
            this.txtParamVal.DataBindings.Clear();

            this.BindingContext[_dtURLParam].Position = _rowIndex;

            this.txtParamNm.DataBindings.Add("Text", _dtURLParam, "ParamName");
            this.txtParamDesc.DataBindings.Add("Text", _dtURLParam, "ParamDesc");
            this.txtParamVal.DataBindings.Add("Text", _dtURLParam, "ParamVal");
            if (_dtURLParam.Rows[this._rowIndex]["ParamEncr"].ToString() == "True")
                this.txtParamVal.PasswordChar = '*';
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
