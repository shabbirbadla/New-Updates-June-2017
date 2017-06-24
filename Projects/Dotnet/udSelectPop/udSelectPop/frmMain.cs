using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace udSelectPop
{
    public partial class SELECTPOPUP : Form
    {
        object txtRef;
        string compath, vformtext = " ", vsearchcol = "", vselstmt = "", vretcol = "", vretcolval = "", vRetColList = "";
        public string vSearchImg = "";  /* Added by pratap for Bug 2128: 27-06-2012 */
        Boolean vSearchOpt = false;   /* Added by pratap for Bug 2128 : 27-06-2012 */
        int vsearchcolind = 0, vretcolind;
        DataGridViewRow vRetRow;
        DataView vdataview;
        string fName = string.Empty;
        int strIndex = 0;
        string vDisplayColumnList;
        Binding bd;
        string[] vReturnArray;
        string vfilt = null;//Bug-3023 Payroll
        public SELECTPOPUP()
        {
            InitializeComponent();


        }

        private void controlset()
        {
            long fwidth = 0;
            this.mBindColumns();
            for (int i = 0; i <= this.dgrSelect.Columns.Count - 1; i++)
            {
                if (this.dgrSelect.Columns[i].Visible == true)
                {
                    fwidth = fwidth + this.dgrSelect.Columns[i].Width;
                }
            }

            if (fwidth < 723)
            {
                //if (fwidth < 300) //Bug-3023 Payroll
                if (fwidth < 450)
                {
                    //this.Width = 300; //Bug-3023 Payroll
                    this.Width = 450;//Bug-3023 Payroll
                    //this.dgrSelect.Width = 280; //Bug-3023 Payroll
                    this.dgrSelect.Width = 430; //Bug-3023 Payroll
                }
                else
                {

                    this.Width = (int)fwidth + 20;
                    
                    this.dgrSelect.Width = (int)fwidth;
                    
                }
            }
            else
            {
                this.Width = 723;
            }

            //this.txtSelect.Width = this.Width-this.txtSelect.Left-20;//Bug-3023 Payroll
            this.txtSelect.Width = this.Width - this.txtSelect.Left - 60;//Bug-3023 Payroll
            this.btnSearchOpt.Left = this.txtSelect.Left + this.txtSelect.Width + 10;//Bug-3023 Payroll
            this.Text = vformtext;
            this.Left = (Screen.PrimaryScreen.Bounds.Width - this.Width) / 2;           //Added by Shrikant S. on 17/11/2016 for GST
            this.Refresh();
            //this.Text=this.met
        }//controlset()
        private void fsearchcolind()
        {
            try
            {
                string str;
                for (int i = 0; i <= this.dgrSelect.ColumnCount - 1; i++)
                {
                    str = (string)this.dgrSelect.Columns[i].Name.Trim();
                    if (vsearchcol != " " & (this.dgrSelect.Columns[i].Name.Trim() == vsearchcol.Trim()))
                    {
                        this.dgrSelect.Columns[i].DefaultCellStyle.ForeColor = Color.Blue;
                        vsearchcolind = i;
                    }
                    else
                    {
                        this.dgrSelect.Columns[i].DefaultCellStyle.ForeColor = Color.Black;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }//fsearchcolind()
        private void retcolind()
        {
            string str;
            if (vdataview.Count <= 1000)
                this.dgrSelect.AutoResizeColumns();       //Commented by Shrikant S. on 17/11/2016 for GST


            for (int i = 0; i <= this.dgrSelect.ColumnCount - 1; i++)
            {
                str = (string)this.dgrSelect.Columns[i].Name.Trim();

                if (vretcol != " " & (this.dgrSelect.Columns[i].Name.Trim() == vretcol.Trim()))
                {
                    vretcolind = i;

                }
            }

        }//retcolind()
        private void mReturnArray()
        {
            if (vRetRow == null)
            {
                this.Close();
                return;
            }
            string vColNm = string.Empty;
            string[] Cols = this.vRetColList.Split(',');
            vReturnArray = new string[Cols.Length];
            for (int i = 0; i <= Cols.Length - 1; i++)
            {
                vColNm = Cols[i];
                this.vReturnArray[i] = vRetRow.Cells[vColNm].Value.ToString();
            }
            if (txtRef != null)
            {
                if (txtRef.GetType().ToString() == "System.Windows.Forms.TextBox")
                {
                    ((TextBox)txtRef).Text = this.vReturnArray[0];
                }
            }
            this.Close();
        }


        private void SELECTPOPUP_Load(object sender, EventArgs e)
        {
            //MessageBox.Show("I1");
            compath = Directory.GetCurrentDirectory();


            this.dgrSelect.DataSource = vdataview;
            if (vdataview.Count <=1000)
                this.dgrSelect.AutoResizeColumns();       //Commented by Shrikant S. on 17/11/2016 for GST

            this.fsearchcolind();
            this.retcolind();
            this.controlset();

            this.dgrSelect.Visible = true;
            this.txtSelect.Visible = true;
            this.btnSearchOpt.Visible = true;   /*Pratap 28-06-2012*/
            fName = vSearchImg + @"\bmp\Searchany.bmp"; /* Added by pratap for Bug 2128 :27-06-2012 */

            if (File.Exists(fName) == true)
            {

                this.btnSearchOpt.Image = Image.FromFile(fName);
                this.toolTipButton.SetToolTip(this.btnSearchOpt, "Search Anywhere");
                fName = string.Empty;

            }
        }
        private void mBindColumns1()
        {
            string vColNm = string.Empty, vColCap = string.Empty;
            dgrSelect.DataSource = this.vdataview;
            dgrSelect.AutoGenerateColumns = false;
            dgrSelect.Columns.Clear();

            string[] Cols = this.vDisplayColumnList.Split(',');
            for (int i = 0; i <= Cols.Length - 1; i++)
            {
                string[] ColParts = Cols[i].Trim().Split(':');
                switch (ColParts.Length)
                {
                    case 1:
                        vColCap = ColParts[0];
                        break;
                    case 2:
                        vColCap = ColParts[1];
                        break;
                    default:
                        throw new ArgumentException("Too many spaces in field definition: '" + Cols[i] + "'.");
                }
                vColNm = ColParts[0];
                DataGridViewTextBoxColumn col1 = new DataGridViewTextBoxColumn();
                this.dgrSelect.Columns.Add(vColNm, vColCap);
                this.dgrSelect.Columns[vColNm].DataPropertyName = vColNm;

            }
        }
        private void mBindColumns()
        {
            string vColNm = string.Empty, vColCap = string.Empty;
            Boolean vColFound = false;
            dgrSelect.DataSource = this.vdataview;
            for (int i = 0; i <= dgrSelect.Columns.Count - 1; i++)
            {
                vColFound = false;
                vColNm = dgrSelect.Columns[i].Name;
                string[] Cols = this.vDisplayColumnList.Split(',');
                for (int j = 0; j <= Cols.Length - 1; j++)
                {
                    string[] ColParts = Cols[j].Trim().Split(':');
                    switch (ColParts.Length)
                    {
                        case 1:
                            vColCap = ColParts[0];
                            break;
                        case 2:
                            vColCap = ColParts[1];
                            break;
                        default:
                            throw new ArgumentException("Too many spaces in field definition: '" + Cols[i] + "'.");
                    }

                    if (vColNm.ToUpper() == ColParts[0].ToUpper())
                    {
                        vColFound = true;
                        this.dgrSelect.Columns[vColNm].HeaderText = vColCap;
                    }
                    this.dgrSelect.Columns[vColNm].Visible = vColFound;
                }

            }
        }

        private void txtSelect_TextChanged(object sender, EventArgs e)
        {
            //string vfilt = null; //Bug-3023 Payroll


            if (this.txtSelect.Focus() == true & (this.txtSelect.Text != ""))
            {
                vfilt = null;//Bug-3023 Payroll
                if (vSearchOpt == false)  /* Added by pratap for Bug 2128 :27-06-2012 */
                {
                    vfilt = "[" + vsearchcol.Trim() + "]" + " like '%" + this.txtSelect.Text.Trim() + "%'";
                }
                else
                {
                    vfilt = "[" + vsearchcol.Trim() + "]" + " like '" + this.txtSelect.Text.Trim() + "%'";  /*Pratap 25-06-2012 for Bug -2128 */
                }
            }

            if (txtSelect.Text.Trim() == "")  /*Ramya 09/03/2013 for Bug-9607*/
            {
                vfilt = "";
            }


            if (vfilt != null)
            {
                vdataview.RowFilter = vfilt;
            }
            if (vfilt == null || vfilt == "")
            {
                vdataview.RowFilter = "";
            }
            this.dgrSelect.DataSource = vdataview;
        }

        private void dgrSelect_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)  /*Ramya Bug-2125*/
            {
                this.Close();
            }
            if ((int)e.KeyCode == 13)
            {
                strIndex = this.dgrSelect.CurrentRow.Index;
                //this.txtSelect.Text = (string)this.dgrSelect.CurrentRow.Cells[vsearchcolind].Value;//Bug-3023 Payroll         // Commented by Sachin N. S. on 23/02/2016 for Bug-27503
                strIndex = this.dgrSelect.CurrentRow.Index;     // Added by Sachin N. S. on 31/05/2013 for Bug-4524
                vRetRow = this.dgrSelect.CurrentRow;//Bug-3023 Payroll
                //this.txtSelect.Focus();         // Commented by Sachin N. S. on 31/05/2013 for Bug-4524
                this.mReturnArray();
                this.txtSelect.Visible = false;
                this.dgrSelect.Visible = false;
                return;
            }

        }

        private void txtSelect_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)  /*Ramya Bug-2125*/
            {
                this.Close();
            }
            if ((int)e.KeyCode == 40)
            {
                int vrind, vcolind;
                this.dgrSelect.Focus();
                if (this.dgrSelect.CurrentRow != null)
                {
                    //this.txtSelect.Text = (string)this.dgrSelect.CurrentRow.Cells[vsearchcolind].Value;//Bug-3023 Payroll
                    //vretcolval = (string)this.dgrSelect.CurrentRow.Cells[vretcolind].Value;
                    vretcolval = (string)this.dgrSelect.CurrentRow.Cells[vretcolind].Value.ToString();      // Added by Sachin N. S. on 10/11/2014 for Bug-22077
                    vRetRow = this.dgrSelect.CurrentRow;
                    strIndex = this.dgrSelect.CurrentRow.Index;  // Added by Sachin N. S. on 31/05/2013 for Bug-4524                    
                }
                this.dgrSelect.Focus();//Bug-3023 Payroll

            }//if ((int) e.KeyCode == 40)
            if ((int)e.KeyCode == 13)
            {
                if (this.dgrSelect.CurrentRow != null)
                {
                    this.txtSelect.Text = (string)this.dgrSelect.CurrentRow.Cells[vsearchcolind].Value;
                    //vretcolval = (string)this.dgrSelect.CurrentRow.Cells[vretcolind].Value;
                    vretcolval = (string)this.dgrSelect.CurrentRow.Cells[vretcolind].Value.ToString();      // Added by Sachin N. S. on 10/11/2014 for Bug-22077

                    vRetRow = new DataGridViewRow();
                    vRetRow = this.dgrSelect.CurrentRow;
                    strIndex = this.dgrSelect.CurrentRow.Index;  // Added by Sachin N. S. on 31/05/2013 for Bug-4524                    
                }
                else
                {
                    this.txtSelect.Text = " ";
                }
                this.mReturnArray();
                //retval();
                this.txtSelect.Visible = false;
                this.dgrSelect.Visible = false;

            }//((int)e.KeyCode == 13)





        }

        private void dgrSelect_Leave(object sender, EventArgs e)
        {
            try   /* Added by pratap for Bug 2128 :27-06-2012 */
            {
                this.dgrSelect.Rows[strIndex].Selected = true;
            }
            catch (Exception ex1)
            {
            }
        }

        private void dgrSelect_SelectionChanged(object sender, EventArgs e)
        {
            if (this.dgrSelect.Focus() == true & (this.dgrSelect.CurrentRow != null))
            {
                //this.txtSelect.Text = this.dgrSelect.CurrentRow.Cells[vsearchcolind].Value.ToString();
                try
                {
                    vRetRow = this.dgrSelect.CurrentRow;
                }
                catch (Exception ex)
                {
                }
            }
        }
        private void dgrSelect_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.vsearchcol = this.dgrSelect.Columns[e.ColumnIndex].Name;
            this.fsearchcolind();
            for (int i = 0; i <= this.dgrSelect.ColumnCount - 1; i++)
            {
                if (vsearchcolind == i)
                {
                    this.dgrSelect.Columns[i].DefaultCellStyle.ForeColor = Color.Blue;
                    //this.Text = "Select " + this.dgrSelect.Columns[i].Name;  /*Ramya Bug-12038*/
                    this.Text = "Select " + this.dgrSelect.Columns[i].HeaderText;   // Changed by Sachin N. S. on 04/04/2013 for Bug-7304
                }
                else
                {
                    this.dgrSelect.Columns[i].DefaultCellStyle.ForeColor = Color.Black;
                }
            }
            if (this.dgrSelect.CurrentRow == null)
            {
                return;
            }
            this.txtSelect.Text = this.dgrSelect.CurrentRow.Cells[vsearchcolind].Value.ToString();



        }
        private void dgrSelect_Click(object sender, EventArgs e)
        {
        }

        private void dgrSelect_DoubleClick(object sender, EventArgs e)
        {
            //Added By kishor Agarwal for Bug  -27503 on 02-02-2016 Start.
            if (this.dgrSelect.CurrentRow == null)
            {
                return;
            }
            //Added By kishor Agarwal for Bug  -27503 on 02-02-2016 Start.
            
            vSearchOpt = true;
            /*---> Added By Amrendra for Bug-3996 on 23-5-2012*/
            if (this.dgrSelect.Rows.Count <= 0)
                return;
            /*<--- Added By Amrendra for Bug-3996 on 23-5-2012*/

            //******* Commented by Sachin N. S. on 29/01/2014 for Bug-20211 -- Start
            ////strIndex = this.dgrSelect.CurrentRow.Index;  /*Ramya Bug-2125*/     // Commented by Sachin N. S. on 31/05/2013 for Bug-4524
            //this.txtSelect.Focus();/*Ramya Bug-2125*/
            ///*---> Added By Ramya for Bug-2126 on 28-5-2012*/
            ////this.txtSelect.Text = (string)this.dgrSelect.CurrentRow.Cells[vsearchcolind].Value;
            //this.txtSelect.Text = Convert.ToString(dgrSelect.CurrentRow.Cells[vsearchcolind].Value);
            ///*<--- Added By  Ramya for Bug-2126 on 28-5-2012*/
            //******* Commented by Sachin N. S. on 29/01/2014 for Bug-20211 -- End

            strIndex = this.dgrSelect.CurrentRow.Index;  // Added by Sachin N. S. on 31/05/2013 for Bug-4524

            vRetRow = this.dgrSelect.CurrentRow;
            this.mReturnArray();
            this.txtSelect.Visible = false;
            this.dgrSelect.Visible = false;
            return;
        }

        private void dgrSelect_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            return;
        }

        public string pDisplayColumnList
        {
            set
            {
                vDisplayColumnList = value;
            }
        }
        public object pSetCtlRef
        {
            get
            {
                return txtRef;
            }
            set
            {
                txtRef = value;
            }
        }
        public string pformtext
        {
            set
            {
                vformtext = value;
            }
        }
        public string pselstmt
        {
            set
            {
                vselstmt = value;
            }
        }
        public string psearchcol
        {
            set
            {
                vsearchcol = value;
            }
        }
        public string pRetcol
        {
            set
            {
                vretcol = value;
            }
        }
        public string pRetcolList
        {
            get { return vRetColList; }
            set { vRetColList = value; }
        }
        public string[] pReturnArray
        {
            get { return vReturnArray; }
            set { vReturnArray = value; }
        }
        public DataView pdataview
        {
            get { return vdataview; }
            set
            {
                vdataview = value;
            }
        }
        public string pSearchImg  /* Added by pratap for Bug 2128 on 27-06-2012 */
        {
            get
            {
                return vSearchImg;
            }
            set
            {
                vSearchImg = value;
            }
        }
        private void SELECTPOPUP_KeyDown(object sender, KeyEventArgs e)   /*Ramya Bug-2125*/
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void btnSearchOpt_Click(object sender, EventArgs e)  /* Added by pratap for Bug 2128 on 27-06-2012 */
        {
            try
            {

                if (vSearchOpt == false)
                {
                    fName = vSearchImg + @"\bmp\Incsearch.bmp";
                    vSearchOpt = true;


                    if (File.Exists(fName) == true)
                    {
                        this.btnSearchOpt.Image = Image.FromFile(fName);
                        this.toolTipButton.SetToolTip(this.btnSearchOpt, "Incremental Search");

                    }
                    this.txtSelect_TextChanged(sender, e);
                    //this.txtSelect.Text = Convert.ToString(dgrSelect.CurrentRow.Cells[vsearchcolind].Value);
                    fName = string.Empty;
                }
                else
                {
                    fName = vSearchImg + @"\bmp\Searchany.bmp";
                    vSearchOpt = false;


                    if (File.Exists(fName) == true)
                    {
                        this.btnSearchOpt.Image = Image.FromFile(fName);
                        this.toolTipButton.SetToolTip(this.btnSearchOpt, "Search Anywhere");

                    }
                    this.txtSelect_TextChanged(sender, e);
                    fName = string.Empty;
                    //this.txtSelect.Text = Convert.ToString(dgrSelect.CurrentRow.Cells[vsearchcolind].Value);
                }
            }
            catch (Exception ex2)
            {
                MessageBox.Show(ex2.Message);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }




    }
}
