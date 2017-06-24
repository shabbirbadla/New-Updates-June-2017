using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace udAdditionalInfo
{

    public partial class udFrmAdditionalInfo : Form
    {
        DataTable _dtLother;
        public string _HdrDtl;
        public string _TblName;
        public DataSet _commonDs;
        public bool AddMode=false,EditMode=false;

        public udFrmAdditionalInfo()
        {
            InitializeComponent();
        }

        public udFrmAdditionalInfo(DataTable _dtLother, string _HdrDtl, string _TblName, DataSet _commonDs)
        {
            InitializeComponent();
            this._dtLother = _dtLother;
            this._HdrDtl = _HdrDtl;
            this._TblName = _TblName;
            this._commonDs = _commonDs;
        }

        private void udFrmAdditionalInfo_Load(object sender, EventArgs e)
        {
            int _lblPosLeft=9, _lblPosRight=338, _lblPosTop=15, _cntrlPosLeft=80, _cntrlPosRight=409, _cntrlPosTop = 12;
            int _lblMaxWidth=67, _lblMaxHght=15, _cntrlMaxWidth=215, _cntrlMaxHght=20;
            bool _lChngLine = false, _lLeftSide=true, _lLblHght=false;
            Label _lblCntrl;
            TextBox _txtCntrl;
            uNumericTextBox.cNumericTextBox _cNumericTextBox;

            foreach (DataRow _dr in _dtLother.Rows)
            {
                if (_dr["Inter_Use"].ToString().ToUpper() == ".T.")
                {
                    continue;
                }
                _lblCntrl= new Label();
                _lblCntrl.Location = new System.Drawing.Point(_lLeftSide==true? _lblPosLeft : _lblPosRight, _lblPosTop);
                _lblCntrl.Name = "lbl"+_dr["Fld_Nm"].ToString().Trim();
                _lblCntrl.Text = _dr["Head_Nm"].ToString().Trim();
                _lblCntrl.Width = _lblMaxWidth;
                _lblCntrl.Height = _lblMaxHght;
                if (_lblCntrl.Text.Length > 11)
                {
                    _lblCntrl.Height *= 2;
                    _cntrlPosTop = _lLeftSide == true ? _cntrlPosTop + 7 : _cntrlPosTop;
                    _lLblHght = true;
                }
                _lblCntrl.BackColor = System.Drawing.Color.Transparent;
                this.pnlUpper.Controls.Add(_lblCntrl);

                switch(_dr["Data_Ty"].ToString().Trim())
                {
                    case "Decimal":
                        _cNumericTextBox = new uNumericTextBox.cNumericTextBox();
                        _cNumericTextBox.Location = new System.Drawing.Point(_lLeftSide == true ? _cntrlPosLeft : _cntrlPosRight, _cntrlPosTop);
                        _cNumericTextBox.Name = "txt" + _dr["Fld_Nm"].ToString().Trim();
                        _cNumericTextBox.Width = _cntrlMaxWidth;
                        _cNumericTextBox.DataBindings.Add("Text", _commonDs, _TblName + "." + _dr["Fld_Nm"].ToString().Trim());
                        _cNumericTextBox.pDecimalLength = Convert.ToInt16(_dr["Fld_Dec"]);
                        this.pnlUpper.Controls.Add(_cNumericTextBox);
                        break;
                    default:
                        _txtCntrl = new TextBox();
                        _txtCntrl.Location = new System.Drawing.Point(_lLeftSide == true ? _cntrlPosLeft : _cntrlPosRight, _cntrlPosTop);
                        _txtCntrl.Name = "txt" + _dr["Fld_Nm"].ToString().Trim();
                        _txtCntrl.Width = _cntrlMaxWidth;
                        _txtCntrl.DataBindings.Add("Text", _commonDs, _TblName + "." + _dr["Fld_Nm"].ToString().Trim());
                        this.pnlUpper.Controls.Add(_txtCntrl);
                        break;
                }
                
                if (_lLeftSide == true)
                {
                    _lLeftSide = false;
                    _lChngLine = true;
                }
                else
                {
                    if (_lChngLine == true)
                    {
                        _lLeftSide = true;
                        _lChngLine = false;
                        _lblPosTop += 25;
                        _cntrlPosTop += 23;
                        if (_lLblHght == true)
                        {
                            _lLblHght = false;
                            _lblPosTop += 5;
                        }
                    }
                }
            }
            this.pnlUpper.Height = _cntrlPosTop + _cntrlMaxHght + 10;
            this.pnlLower.Top = this.pnlUpper.Top + this.pnlUpper.Height+2;
            this.Height = this.pnlLower.Top + this.pnlLower.Height + 30;
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _commonDs.Tables[_TblName].RejectChanges();
            this.Close();
        }

    }
}
