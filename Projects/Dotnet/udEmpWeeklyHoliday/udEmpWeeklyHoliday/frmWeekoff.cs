using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;

namespace udEmpWeeklyHoliday
{
    public partial class frmWeekoff : uBaseForm.FrmBaseForm
    {

       // DataAccess_Net.clsDataAccess oDataAccess;

        string weekoffval;
        //string Setweekoff;
        char[] weekdayval;

        string Pweeekoff;
       // string str11;
        public string Cweekoff
        {
            get { return Pweeekoff; }
            set { Pweeekoff = value; }
        }
        string PweekVal;

        public string CweekVal
        {
            set { PweekVal = value; }
            get { return PweekVal; }
        }


        public string ST = string.Empty;//Added by Archana K. on 21/05/13 for Bug-7899
        //public frmWeekoff(string[] args)//Commented by Archana K. on 21/05/13 for Bug-7899
        public frmWeekoff(string[] args, string ServiceType)//Changed Archana K. on 21/05/13 for Bug-7899
        {
            this.pDisableCloseBtn = true;  /* close disable  */
            InitializeComponent();
            this.pPApplPID = 0;
            this.pPara = args;
            this.pFrmCaption = "Week Days";
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
            this.ST = ServiceType;//Added by Archana K. on 21/05/13 for Bug-7899
        }

        private void frmWeekoff_Load(object sender, EventArgs e)
        {
            CultureInfo ci = new CultureInfo("en-US");   /*Ramya*/
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;

            weekdayval = CweekVal.ToCharArray();
            for (int i = 0; i < weekdayval.Length; i++)
            {
                CheckBox ch = new CheckBox();
                //CheckBox ch1 = new CheckBox();
                Panel p = new Panel();
                p = (Panel)this.Controls["panel" + Convert.ToString(i)];
                switch (weekdayval[i])
                {

                    case 'F':
                        //p = (Panel)("panel" +" Convert.ToString(i)");

                        //p.Name= this.Controls["panel" + Convert.ToString(i)].Name;


                        ch = (CheckBox)(p.Controls[1]);
                        ch.Checked = true;
                        break;
                    case 'H':
                        ch = (CheckBox)(p.Controls[0]);
                        ch.Checked = true;
                        break;
                    //case 'W':
                    //    ch = (CheckBox)(p.Controls[1]);
                    //    ch1 = (CheckBox)(p.Controls[0]);
                    //    ch.Checked = false;
                    //    ch1.Checked = false;
                    //    break;


                }
                
            }

            //Added by Archana K. on 17/05/13 for Bug-7899 start
            if (ST.ToUpper() == "VIEWER VERSION")
            {
                panel0.Enabled = false;
                panel1.Enabled = false;
                panel2.Enabled = false;
                panel3.Enabled = false;
                panel4.Enabled = false;
                panel5.Enabled = false;
                panel6.Enabled = false;
                panel8.Enabled = false;
            }
            else
            {
                panel0.Enabled = true;
                panel1.Enabled = true;
                panel2.Enabled = true;
                panel3.Enabled = true;
                panel4.Enabled = true;
                panel5.Enabled = true;
                panel6.Enabled = true;
                panel8.Enabled = true;
            }
            //Added by Archana K. on 17/05/13 for Bug-7899 end


        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (chkFSun.Checked == false && chkHSun.Checked == false)
            {
                weekoffval = "W";
            }
            else
            {
                if (chkFSun.Checked)
                {
                    weekoffval = "F";
                }
                else if (chkHSun.Checked)
                {
                    weekoffval = "H";
                }
            }

            if (chkFMon.Checked == false && chkHMon.Checked == false)
            {
                weekoffval += "W";
            }
            else
            {
                if (chkFMon.Checked)
                {
                    weekoffval += "F";
                }
                else if (chkHMon.Checked)
                {
                    weekoffval += "H";
                }
            }

            if (chkFTues.Checked == false && chkHTues.Checked == false)
            {
                weekoffval += "W";
            }
            else
            {
                if (chkFTues.Checked)
                {
                    weekoffval += "F";
                }
                else if (chkHTues.Checked)
                {
                    weekoffval += "H";
                }
            }

            if (chkFWed.Checked == false && chkHWed.Checked == false)
            {
                weekoffval += "W";
            }
            else
            {
                if (chkFWed.Checked)
                {
                    weekoffval += "F";
                }
                else if (chkHWed.Checked)
                {
                    weekoffval += "H";
                }
            }

            if (chkFThurs.Checked == false && chkHThurs.Checked == false)
            {
                weekoffval += "W";
            }
            else
            {
                if (chkFThurs.Checked)
                {
                    weekoffval += "F";
                }
                else if (chkHThurs.Checked)
                {
                    weekoffval += "H";
                }
            }

            if (chkFFri.Checked == false && chkHFri.Checked == false)
            {
                weekoffval += "W";
            }
            else
            {
                if (chkFFri.Checked)
                {
                    weekoffval += "F";
                }
                else if (chkHFri.Checked)
                {
                    weekoffval += "H";
                }
            }

            if (chkFSat.Checked == false && chkHSat.Checked == false)
            {
                weekoffval += "W";
            }
            else
            {
                if (chkFSat.Checked)
                {
                    weekoffval += "F";
                }
                else if (chkHSat.Checked)
                {
                    weekoffval += "H";
                }
            }

            Pweeekoff = weekoffval;

            this.Close();
        }


        private void chkFSun_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFSun.Checked)
            {
                chkHSun.Checked = false;
            }
        }

        private void chkHSun_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHSun.Checked)
            {
                chkFSun.Checked = false;
            }
        }

        private void chkFMon_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFMon.Checked)
            {
                chkHMon.Checked = false;
            }

        }

        private void chkHMon_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHMon.Checked)
            {
                chkFMon.Checked = false;
            }
        }

        private void chkFTues_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFTues.Checked)
            {
                chkHTues.Checked = false;
            }
        }

        private void chkHTues_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHTues.Checked)
            {
                chkFTues.Checked = false;
            }

        }

        private void chkHWed_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHWed.Checked)
            {
                chkFWed.Checked = false;
            }

        }

        private void chkFWed_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFWed.Checked)
            {
                chkHWed.Checked = false;
            }
        }

        private void chkHThurs_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHThurs.Checked)
            {
                chkFThurs.Checked = false;
            }
        }

        private void chkFThurs_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFThurs.Checked)
            {
                chkHThurs.Checked = false;
            }
        }

        private void chkFFri_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFFri.Checked)
            {
                chkHFri.Checked = false;
            }
        }

        private void chkHFri_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHFri.Checked)
            {
                chkFFri.Checked = false;
            }
        }

        private void chkFSat_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFSat.Checked)
            {
                chkHSat.Checked = false;
            }
        }

        private void chkHSat_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHSat.Checked)
            {
                chkFSat.Checked = false;
            }
        }


    }
}
