using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using uBaseForm;
using uTextBox;
using uCheckBox;
using uHelpButton;
using System.Data.SqlClient;
using System.Reflection;

namespace uParameterSelection
{
    public partial class FrmMainParameterSelection : uBaseForm.FrmBaseForm 
    {
        DataAccess_Net.clsDataAccess oDataAccess;
        string vSearchFields = string.Empty, vSearchFieldsCap = string.Empty;
       
        uBaseForm.FrmBaseForm  vParentForm;
        string vParaList;
        DataSet vds;
        public FrmMainParameterSelection()
        {
            InitializeComponent();
         
        }
        private void mDisplayControls()
        {
            
            uBaseForm.FrmBaseForm ofrm = new uBaseForm.FrmBaseForm();
            if (this.pParentForm != null)
            {
                ofrm = this.pParentForm;
                DataSet vds = new DataSet();
                vds = (DataSet)ofrm.GetType().GetProperty("pDataSet").GetValue(ofrm, null);
                if ((vds.Tables[0].Rows[0]["paralist"]) != null)
                {
                    this.pParaList = vds.Tables[0].Rows[0]["paralist"].ToString();
                }
            }


            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();

            DataSet dsFieldList = new DataSet();
            string sqlstr;
            try
            {
                sqlstr = "select * from Alert_Para_master order by DispOrder";
                dsFieldList = oDataAccess.GetDataSet(sqlstr, null, 20);


                if (dsFieldList.Tables.Count == 0)
                {
                    return;
                }
                if (dsFieldList.Tables[0].Rows.Count == 0)
                {
                    return;
                }
                string vCaption = string.Empty, vCaption_old = string.Empty, vFieldName = string.Empty, vDataType = string.Empty, vHelpQuery = string.Empty, vDefaulValue = string.Empty,vVal=string.Empty;
                
                Int16 vSize=0,vDecimal=0;
                int MaxCtrlWidth = 150; 
                int MaxTxtWidth = 250;
                int vTop;
                int vLeft = 30;
                int VHeight = 20, vfrmSize = 110;
                int vPos1 = 0, vPos2 = 0;
                vTop = 0;
                vCaption_old = "";
                for (int i = 0; i < dsFieldList.Tables[0].Rows.Count; i++)
                {
                    vPos1 = 0; vPos2 = 0; vVal = "";
                    vCaption = dsFieldList.Tables[0].Rows[i]["paracaption"].ToString().Trim();
                    vFieldName = dsFieldList.Tables[0].Rows[i]["ParaName"].ToString().Trim();
                    vDataType = dsFieldList.Tables[0].Rows[i]["paraDataType"].ToString().Trim();
                    vSize = Convert.ToInt16(dsFieldList.Tables[0].Rows[i]["ParaSize"]);
                    vDecimal = Convert.ToInt16(dsFieldList.Tables[0].Rows[i]["ParaDecimal"]);
                    vHelpQuery = dsFieldList.Tables[0].Rows[i]["HelpQuery"].ToString().Trim();
                    vDefaulValue = dsFieldList.Tables[0].Rows[i]["DefaVal"].ToString().Trim();
                    if (string.IsNullOrEmpty(pParaList) == false)
                    {
                        if (this.pParaList.IndexOf("<<" + vFieldName + "=", 0) > -1)
                        {
                            vPos1 = this.pParaList.IndexOf("<<" + vFieldName + "=", 0);
                            vPos2 = this.pParaList.IndexOf(">>", vPos1);
                            vVal = this.pParaList.Substring(vPos1, vPos2 - vPos1);
                            vVal = vVal.Replace("<<" + vFieldName + "=", "");
                        }
                    }
                    vLeft = 30;
                    if (string.IsNullOrEmpty(vCaption_old) == false)
                    {
                        if (vCaption_old.IndexOf("From ") > -1 && vCaption.IndexOf("To ") > -1)
                        {
                            vLeft = 450;
                        }
                        else
                        {
                            vTop = vTop + 10 + VHeight;
                            vfrmSize = vfrmSize + 10+VHeight;
                        }

                    }
                    else
                    {
                        vTop = vTop + 10 + VHeight;
                        vfrmSize = vfrmSize + 10 + VHeight;
                    }
                    vCaption_old = vCaption;


                    Label labelCaption = new Label();
                    labelCaption.Name = vFieldName + i;
                    labelCaption.Tag = vDataType;
                    labelCaption.AutoSize = true;
                    labelCaption.Font = new Font(labelCaption.Font, FontStyle.Regular);
                    labelCaption.Size = new System.Drawing.Size(MaxCtrlWidth, 22);
                    labelCaption.Top = vTop;
                    labelCaption.Left = vLeft;
                    labelCaption.Text = vCaption;

                    this.Controls.Add(labelCaption);

                    switch (vDataType.ToUpper())
                    {
                        case "VARCHAR":
                            uTextBox.cTextBox txtBox = new uTextBox.cTextBox();
                            txtBox.pCaption = vCaption;
                            txtBox.Name = vFieldName;
                            txtBox.pDataType = vDataType;
                            txtBox.pHelpQuery = vHelpQuery;
                            txtBox.pDefaultValue = vDefaulValue;
                            txtBox.pHelpQuery = vHelpQuery;
                            txtBox.Top = labelCaption.Top;
                            txtBox.Left = labelCaption.Location.X + 100;
                            txtBox.Tag = vDataType;
                            txtBox.Text = vDefaulValue; //???
                            txtBox.Width = MaxTxtWidth;
                            txtBox.ReadOnly = true;
                            txtBox.MaxLength = vSize;
                            txtBox.Leave += new EventHandler(txtBox_Leave);
                            txtBox.KeyUp += new KeyEventHandler(utextBox_KeyDown);
                            txtBox.Validating += new CancelEventHandler(textbox_Validating);
                            txtBox.Text = vVal;
                            this.Controls.Add(txtBox);
                            if (string.IsNullOrEmpty(vHelpQuery) == false)
                            {
                                uHelpButton.cHelpButton HelpBtn = new uHelpButton.cHelpButton();
                                HelpBtn.Visible = true;
                                HelpBtn.Name = "hpc" + vFieldName;
                                HelpBtn.pCaption = vCaption;
                                HelpBtn.Tag = vDataType;
                                string vfname = this.pAppPath + @"\bmp\" + "loc-on.gif";
                                if (File.Exists(vfname))
                                {
                                    HelpBtn.Image = Image.FromFile(vfname);
                                }
                                HelpBtn.Size = new System.Drawing.Size(22, 22);
                                HelpBtn.Click += new EventHandler(HelpBtn_Click);
                                HelpBtn.Location = new Point(txtBox.Location.X + txtBox.Width + 2, txtBox.Location.Y);
                                if (string.IsNullOrEmpty(vHelpQuery) == false)
                                {
                                    HelpBtn.pHelpQuery = vHelpQuery;
                                }
                                this.Controls.Add(HelpBtn);
                            }
                            //
                            break;

                    }//Swith Case
                }
                 vTop = vTop + 10 + VHeight;
                this.btnDone.Top = vTop;
                this.Height = vfrmSize;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void EnableDiableFormControls()
        {

            bool enabled = true;
                foreach (Control childCtrl in this.Controls )
                {

                    switch (childCtrl.GetType().Name.ToLower())
                    {

                        //case "ctextbox":
                        //    TextBox MyTextBox = (TextBox)childCtrl;
                        //    MyTextBox.ReadOnly = !enabled;
                        //    //if (setFocus)
                        //    //    //SetFocusToControl(childCtrl, childCtrl.Name);
                        //    break;
                        //case "maskedtextbox":
                        //    MaskedTextBox MyTextBox1 = (MaskedTextBox)childCtrl;
                        //    if (MyTextBox1.Tag.ToString() == "Datetime")
                        //        MyTextBox1.Mask = "####.##";
                        //    MyTextBox1.ReadOnly = !enabled;
                        //    if (setFocus)
                        //        //SetFocusToControl(childCtrl, childCtrl.Name);
                        //    break;
                        //case "datetimepicker":
                        //    DateTimePicker dtPicker = (DateTimePicker)childCtrl;
                        //    dtPicker.Enabled = enabled;
                        //    if (setFocus)
                        //        //SetFocusToControl(childCtrl, childCtrl.Name);
                        //    break;
                        //case "checkbox":
                        //    childCtrl.Enabled = enabled;
                        //    if (setFocus)
                        //        if (setFocus)
                        //            //SetFocusToControl(childCtrl, childCtrl.Name);
                        //    break;
                        case "combobox":
                            childCtrl.Enabled = enabled;
                           // SetFocusToControl(childCtrl, childCtrl.Name);
                            break;
                        case "button":
                            if (childCtrl.Name == "buttonPopup")
                            {
                                childCtrl.Enabled = !enabled;
                            }
                            break;
                        case "chelpbutton":
                            childCtrl.Enabled = false;
                            if (childCtrl.Name.Substring(0, 3) == "hpc")
                            {
                                if (string.IsNullOrEmpty(((uHelpButton.cHelpButton)childCtrl).pSearchFieldQuery) == false)
                                {
                                    if (this.pAddMode == false && this.pEditMode == false)
                                    {
                                        childCtrl.Enabled = true;
                                    }
                                }
                                if (string.IsNullOrEmpty(((uHelpButton.cHelpButton)childCtrl).pHelpQuery) == false)
                                {
                                    if (this.pAddMode || this.pEditMode)
                                    {
                                        childCtrl.Enabled = true;
                                    }
                                }
                            }
                            break;//DateTimePicker
                    }
                }
         
        }
        void txtBox_Leave(object sender, EventArgs e)
        {
            
        }
        private void HelpBtn_Click(object sender, EventArgs e)
        {
            string vHelpQuery = string.Empty;
            string VForText = string.Empty;
            string cntName = string.Empty;
            if (sender.GetType().ToString().ToLower() == "utextbox.ctextbox")
            {
                vHelpQuery = ((uTextBox.cTextBox)sender).pHelpQuery;
                VForText = "Select " + ((uTextBox.cTextBox)sender).pCaption;
                cntName = ((uTextBox.cTextBox)sender).Name;
            }
            else
            {
                vHelpQuery = ((uHelpButton.cHelpButton)sender).pHelpQuery;
                VForText = "Select " + ((uHelpButton.cHelpButton)sender).pCaption;
                cntName = ((uHelpButton.cHelpButton)sender).Name;
                cntName = cntName.Substring(3, cntName.Length - 3);
            }
            int pos1 = 0, pos2 = 0;
            string vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            if (this.pAddMode || this.pEditMode)
            {

                pos1 = vHelpQuery.IndexOf("{");
                if (pos1 > 1)
                {
                    strSQL = vHelpQuery.Substring(0, pos1 - 1);
                    Vstr = vHelpQuery.Substring(pos1 + 1, vHelpQuery.Length - pos1 - 2);
                    pos2 = Vstr.IndexOf("#");
                    vSearchCol = Vstr.Substring(0, pos2);
                    Vstr = Vstr.Substring(pos2 + 1, Vstr.Length - pos2 - 1);
                    pos2 = vSearchCol.IndexOf("+");
                    if (pos2 > 0)
                    {
                        vSearchCol = vSearchCol.Substring(0, pos2);
                    }


                    pos2 = Vstr.IndexOf("#");
                    vReturnCol = Vstr.Substring(0, pos2);
                    Vstr = Vstr.Substring(pos2 + 1, Vstr.Length - pos2 - 1);
                    pos2 = Vstr.IndexOf("#");
                    vDisplayColumnList = Vstr.Substring(0, pos2);
                    Vstr = Vstr.Substring(pos2 + 1, Vstr.Length - pos2 - 1);
                    vDisplayColumnList = Vstr;
                }
                else
                {
                    strSQL = vHelpQuery;
                    vSearchCol = "";
                    vReturnCol = "";
                    vDisplayColumnList = "";
                }

            }
            else
            {
                //if (string.IsNullOrEmpty(vSearchFields) == false)
                //{
                //    strSQL = "Select " + vSearchFields + " from " + _TableName + " order by " + vSearchFields;
                //    vSearchCol = vSearchFields;
                //    pos2 = vSearchFields.IndexOf(",");
                //    if (pos2 > 0)
                //    {
                //        vSearchCol = vSearchFields.Substring(0, pos2);
                //    }
                //    vReturnCol = vSearchCol;
                //    vDisplayColumnList = vSearchFieldsCap;
                //}

            }

            DataSet dstemp = new DataSet();
            dstemp = oDataAccess.GetDataSet(strSQL, null, 20);
            DataView dvw = dstemp.Tables[0].DefaultView;

            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;
            //string vFieldList = _UniqueColName + ":" + _UniqueColName.ToUpper();
            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            //oSelectPop.pSetCtlRef = this.textBoxCaption;
            oSelectPop.ShowDialog();
            
            if (oSelectPop.pReturnArray != null)
            {
                if (this.pAddMode || this.pEditMode)
                {
                    //((uTextBox.cTextBox)sender).Text = oSelectPop.pReturnArray[0];
                    this.Controls[cntName].Text = oSelectPop.pReturnArray[0];

                    //((uTextBox.cTextBox)cntName).Name="asdaw";
                //    dsMain.Tables[0].Rows[0][cntName] = oSelectPop.pReturnArray[0];
                //    object objects = this;
                //    mthRemoveControlBind(GetTabControl(objects));
                //    mthControlBind(GetTabControl(objects));
                //    this.Refresh();
                //    return;
                }
            //    string vFldVal = string.Empty, sqlstr = string.Empty;
            //    vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString();
            //    sqlstr = "select top 1  " + vMainField + " from " + this._TableName + " where " + vMainField + "='" + vMainFldVal + "' order by  " + vMainField + " desc";
            //    dsMain.Tables.Clear();
            //    dsMain = oDataAccess.GetDataSet(sqlstr, null, 20);

            //    this.mthSubView();
            //    //this.Controls[cntName].Text = oSelectPop.pReturnArray[0];
            //    //Navigation Button//

            //    DataSet dsTemp = new DataSet();

            //    vMainFldVal = oSelectPop.pReturnArray[0];
            //    vFldVal = vMainFldVal;
            //    sqlstr = "select cnt=count(id) from " + this._TableName + " where  " + vMainField + " < '" + vFldVal + "'";
            //    dsTemp = oDataAccess.GetDataSet(sqlstr, null, 20);
            //    if (dsMain.Tables[0].Rows.Count > 0)
            //    {
            //        if (Convert.ToInt32(dsTemp.Tables[0].Rows[0]["cnt"]) > 0)
            //        {
            //            this.btnFirst.Enabled = true;
            //            this.btnBack.Enabled = true;
            //        }
            //        else
            //        {
            //            this.btnFirst.Enabled = false;
            //            this.btnBack.Enabled = false;
            //        }
            //    }
            //    vFldVal = vMainFldVal;
            //    sqlstr = "select cnt=count(id) from " + this._TableName + " where  " + vMainField + " > '" + vFldVal + "'";
            //    dsTemp = oDataAccess.GetDataSet(sqlstr, null, 20);
            //    if (dsMain.Tables[0].Rows.Count > 0)
            //    {
            //        if (Convert.ToInt32(dsTemp.Tables[0].Rows[0]["cnt"]) > 0)
            //        {
            //            this.btnForward.Enabled = true;
            //            this.btnLast.Enabled = true;
            //        }
            //        else
            //        {
            //            this.btnForward.Enabled = false;
            //            this.btnLast.Enabled = false;
            //        }
            //    }
            //    this.mthSubView();



            //    HandleButtons(true, true, false, false, true);



            //    //this.Controls[cntName].Text = oSelectPop.pReturnArray[0];
            }



        }

        
        private void textbox_Validating(object sender, CancelEventArgs e)
        {
            //uTextBox.cTextBox txtName = (uTextBox.cTextBox)sender;
            ////TextBox txtName = (TextBox)sender;
            //if (!txtName.ReadOnly)
            //{

            //    if (txtName.Text.Trim() == "" && txtName.pMandatory == true && txtName.pDataType.ToUpper() == "VARCHAR")
            //    {

            //        ToolTip t = new ToolTip();
            //        string ErrMsg = t.GetToolTip(txtName).ToString();
            //        errorProvider.SetError(txtName, "Could not allow blank " + txtName.pCaption + " value");
            //        txtName.Focus();
            //        vValid = false;
            //        e.Cancel = true;
            //        return;
            //    }
            //    if (txtName.pMandatory == true && txtName.pDataType.ToUpper() == "DECIMAL")
            //    {
            //        if (txtName.Text.Trim() == "")
            //        {
            //            ToolTip t = new ToolTip();
            //            string ErrMsg = t.GetToolTip(txtName).ToString();
            //            errorProvider.SetError(txtName, "Could not allow blank " + txtName.pCaption + " value");
            //            txtName.Focus();
            //            vValid = false;
            //            e.Cancel = true;
            //            return;
            //        }
            //        if (Convert.ToDecimal(txtName.Text) == 0)
            //        {
            //            ToolTip t = new ToolTip();
            //            string ErrMsg = t.GetToolTip(txtName).ToString();
            //            errorProvider.SetError(txtName, "Could not Zero " + txtName.pCaption + " value");
            //            txtName.Focus();
            //            vValid = false;
            //            e.Cancel = true;
            //            return;
            //        }
            //    }
            //    if (txtName.Name == vMainField)
            //    {
            //        DataSet dsData = new DataSet();
            //        string sqlstr;
            //        sqlstr = " select " + vMainField + " from " + _TableName + " where " + vMainField + "='" + txtName.Text.Trim() + "'";
            //        if (this.pEditMode)
            //        {
            //            sqlstr = sqlstr + " and id<>" + dsMain.Tables[0].Rows[0]["id"].ToString();
            //        }
            //        dsData = oDataAccess.GetDataSet(sqlstr, null, 20);
            //        if (dsData.Tables[0].Rows.Count > 0)
            //        {
            //            ToolTip t = new ToolTip();
            //            string ErrMsg = t.GetToolTip(txtName).ToString();
            //            errorProvider.SetError(txtName, "Duplicate " + txtName.pCaption + " value");
            //            txtName.Focus();
            //            vValid = false;
            //            e.Cancel = true;
            //            return;
            //        }
            //    }
            //    errorProvider.SetError(txtName, "");
            //}

        }

        private void utextBox_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            string paralist="";
            if (this.pAddMode == false && this.pEditMode==false )
            {
                this.Close();
                return;
            }
            
            foreach (Control cntrl in this.Controls)
            {

                if (cntrl.GetType().ToString().ToLower() == "utextbox.ctextbox")
                {
                    if (string.IsNullOrEmpty(cntrl.Text) == false)
                    {
                        paralist = paralist + "<<"+ cntrl.Name.Trim()+"="+ cntrl.Text.Trim() + ">>";
                    }
                }
            }
           
            this.pParaList = paralist;
            uBaseForm.FrmBaseForm ofrm = new uBaseForm.FrmBaseForm();
            ofrm = this.pParentForm;
            Type tfrm = ofrm.GetType();
            TabControl tbj = new TabControl();
            this.pDataSet = (DataSet)tfrm.GetProperty("pDataSet").GetValue(ofrm, null);
            this.pDataSet.Tables[0].Rows[0]["paralist"] = this.pParaList;
            tfrm.GetProperty("pDataSet").SetValue(this.pParentForm, this.pDataSet, null);

            object[] objects = new object [] {this.pParentForm};
            
            tbj=(TabControl) tfrm.GetMethod("GetTabControl").Invoke(this.pParentForm, objects);
            object [] obj = new object[] { tbj };
            tfrm.GetMethod("mthRemoveControlBind").Invoke(this.pParentForm, obj);
            tfrm.GetMethod("mthControlBind").Invoke(this.pParentForm, obj);
            this.Close();
        }

        private void FrmMainParameterSelection_Load(object sender, EventArgs e)
        {
            string[] args = { };
            if (this.pPara != null)
            {
                args = this.pPara;
            }
            if (args.Length < 1)
            {
                  args = new string[] { "19", "A021112", "desktop246", "sa", "sa@1985", "13032", "ALM", "ADMIN", @"D:\USquare\Bmp\Icon_usquare.ico", "Usquare pack", "Usquare.exe", "1", "udpid6096DTM20110307112715" };/*Rup Add ICO Path,Parent Appl Caption,Parent Appl. Name,Parent Appl PId,Application Log Table*/
            }


            oDataAccess = new DataAccess_Net.clsDataAccess();
            this.pFrmCaption = "Parameter Details";
            this.pCompId = Convert.ToInt16(args[0]);
            this.pComDbnm = args[1];
            this.pServerName = args[2];
            this.pUserId = args[3];
            this.pPassword = args[4];
            Icon MainIcon = new System.Drawing.Icon(args[8].Replace("<*#*>", " "));
            this.pFrmIcon = MainIcon;
            this.pPApplText = args[9].Replace("<*#*>", " ");
            this.pPApplName = args[10];
            this.pPApplPID = Convert.ToInt16(args[11]);
            this.pPApplCode = args[12];
            this.mDisplayControls();
            this.EnableDiableFormControls();
        }
        public void mthParalist(ref string pList)
        { 
            
        }
     
        public string pParaList
        {
            get { return vParaList; }
            set { vParaList = value; }

        }
        public uBaseForm.FrmBaseForm pParentForm
        {
            get { return vParentForm; }
            set { vParentForm=value;}
        }
       
    }
}
