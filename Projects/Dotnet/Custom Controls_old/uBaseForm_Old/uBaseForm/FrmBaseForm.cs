using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace uBaseForm
{
    public partial class FrmBaseForm : Form
    {
        uBaseForm.FrmBaseForm vParentForm;
        string vFrmCaption = "";
        string vAppDbnm;
        string vComDbnm;
        string vAppPath;
        string vComPath;
        Boolean vAddMode, vEditMode, vEditButton, vAddButton, vDeleteButton,vPrintButton;
        
        Icon vFrmIcon;
        int vCompId = 0;
        string vServerName = string.Empty;
        string vPassword, vUserId;
        string vAppUerName = string.Empty;
        string vPApplText, vPApplName, vApplCode, vPApplRange;
        int vPApplPID;
        string[] vPara;
        DataSet vDataSet;

        public FrmBaseForm()
        {
            InitializeComponent();
        }

        private void FrmBaseForm_Load(object sender, EventArgs e)
        {
            this.Text = this.pFrmCaption;
            if (this.pFrmIcon != null)
            {
                this.Icon = this.pFrmIcon;
            }
        }

        protected override CreateParams CreateParams  /*Ramya to disable close button*/
        {
            get
            {
                CreateParams param = base.CreateParams;
                param.ClassStyle = param.ClassStyle | 0x200;
                return param;

            }

        }


        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //abc  

        }
        protected override void OnKeyUp(System.Windows.Forms.KeyEventArgs e)
        {
            if (this.ActiveControl == null)
            {
                return;

            }
            if (e.KeyCode == Keys.Enter & (this.ActiveControl.GetType().ToString() == "System.Windows.Forms.TextBox"))
            {
                TextBox tb = (TextBox)this.ActiveControl;
                if (tb.Multiline & tb.AcceptsReturn)
                {
                    e.Handled = false;
                    base.OnKeyUp(e);
                    return;
                }

                e.Handled = true;
                this.ProcessTabKey(!e.Shift);
            }
            else if (e.KeyCode == Keys.Enter & (this.ActiveControl.GetType().ToString() == "System.Windows.Forms.ComboBox"))
            {

                e.Handled = true;
                this.ProcessTabKey(!e.Shift);
            }
            else
            {
                e.Handled = false;
                base.OnKeyUp(e);
            }


        }
        public string pAppDbnm
        {
            get
            {
                return vAppDbnm;
            }
            set
            {
                vAppDbnm = value;
            }
        }
        public string pComDbnm
        {
            get
            {
                return vComDbnm;
            }
            set
            {
                vComDbnm = value;
            }
        }
        public string pAppPath
        {
            get
            {
                return vAppPath;
            }
            set
            {
                vAppPath = value;
            }
        }
        public string pComPath
        {
            get
            {
                return vComPath;
            }
            set
            {
                vComPath = value;
            }
        }
        public string pFrmCaption
        {
            get
            {
                return vFrmCaption;
            }
            set
            {
                vFrmCaption = value;
            }
        }
        public Boolean pAddMode
        {
            get
            {
                return vAddMode;
            }
            set
            {
                vAddMode = value;
            }
        }
        public Boolean pEditMode
        {
            get
            {
                return vEditMode;
            }
            set
            {
                vEditMode = value;
            }
        }
        public Icon pFrmIcon
        {
            get
            {
                return vFrmIcon;
            }
            set
            {
                vFrmIcon = value;
            }
        }
        public int pCompId
        {
            get
            {
                return vCompId;
            }
            set
            {
                vCompId = value;
            }
        }
        public string pServerName
        {
            get
            {
                return vServerName;
            }
            set
            {
                vServerName = value;
            }
        }
        public string pUserId
        {
            get
            {
                return vUserId;
            }
            set
            {
                vUserId = value;
            }
        }
        public string pPassword
        {
            get
            {
                return vPassword;
            }
            set
            {
                vPassword = value;
            }
        }
        public string pAppUerName
        {
            get
            {
                return vAppUerName;
            }
            set
            {
                vAppUerName = value;
            }
        }
        
        public string pPApplText
        {
            get
            {
                return vPApplText;
            }
            set
            {
                vPApplText = value;
            }
        }
        public string pPApplName
        {
            get
            {
                return vPApplName;
            }
            set
            {
                vPApplName = value;
            }
        }
        public string pPApplCode
        {
            get
            {
                return vApplCode;
            }
            set
            {
                vApplCode = value;
            }
        }
        public int pPApplPID
        {
            get
            {
                return vPApplPID;
            }
            set
            {
                vPApplPID = value;
            }
        }
        public string pPApplRange
        {
            get
            {
                return vPApplRange;
            }
            set
            {
                vPApplRange = value;
            }
        }
        public Boolean  pAddButton
        {
            get
            {

                return vAddButton;
            }
            set
            {
                vAddButton = value;
            }
        }
        public Boolean pEditButton
        {
            get
            {

                return vEditButton;
            }
            set
            {
                vEditButton = value;
            }
        }
        public Boolean pDeleteButton
        {
            get
            {

                return vDeleteButton ;
            }
            set
            {
                vDeleteButton = value;
            }
        }
        public Boolean pPrintButton
        {
            get
            {

                return vPrintButton ;
            }
            set
            {
                vPrintButton = value;
            }
        }
        public DataSet pDataSet
        {
            get { return vDataSet; }
            set { vDataSet = value; }
        }
        public string [] pPara
        {
            get { return vPara; }
            set { vPara = value; }
        }
        public uBaseForm.FrmBaseForm pParentForm
        {
            get { return vParentForm; }
            set { vParentForm = value; }
        }
       
    }
}
