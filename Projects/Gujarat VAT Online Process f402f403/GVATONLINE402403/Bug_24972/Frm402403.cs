using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Data.OleDb;
using System.Text.RegularExpressions;


namespace Bug_24972
{
    public partial class Frm402403 : Form
    {
        private string _Sta_dt = string.Empty;
        private string _End_dt = string.Empty;
        private string FinYear = string.Empty;
        private string _ConnStr = string.Empty;
        private string appName = string.Empty;
        private string compId = string.Empty;
        private string query = string.Empty;       
        DataSet MainSet;
        public static string _strCity = string.Empty;
        public static string _strInvNo = string.Empty;
        public static string _strGVType = string.Empty;
        public static Int32 eFileType = 0;        //1-402     2-403


        public string cFileName = string.Empty;
        public string cFinalPath = string.Empty;
            //cFileName = System.IO.Path.Combine(this.txtOutputPath.Text, "Gis_Form_402_m.xls");
                         
        public Frm402403(string[] args)
        {
            InitializeComponent();

            //MessageBox.Show(Convert.ToString(args.Length));

            //Execution from Software
            compId = args[0];
            //appName = args[8].Replace("<*#*>", " ");   //0-compId
            appName = "VUdyog";

            _ConnStr = "Data Source=" + args[2] + ";Initial Catalog=" + args[1] + ";Uid=" + args[3] + ";Pwd=" + args[4];
            //this.Icon = new Icon(args[7]);
            eFileType = Convert.ToInt32(args[5]);
            //this.cboGVType.Text = "NonSpecified";
            this.cboGVType.Text = "";
            
            this.GetInvData();
        
            this.LoadForms();

            //this.cboGVType.Focus();
            //this.cboInvNo.Enabled = false;

            //this.LoadForms();
            this.Text = (eFileType == 2 ? "Form 403" : "Form 402") + " Extraction to Excel";

          

            //Execution from Project
            //_ConnStr = "Data Source=Prod_shrikant\\shri;Initial Catalog=T011314;Uid=sa;Pwd=sa1985";
            //eFileType = 1;
            //compId = "30";
            //this.GetFinYear();
            //LoadForms();
        }

        private void LoadForms()
        {
           
            this.cboInvNo.Enabled = false;
            this.cboGVType.Focus();            
            this.cboInvNo.Items.Clear();

            //this.cboGVType.SelectedItem = "NonSpecified";
            this.cboGVType.SelectedItem = "";
            this.cboGVType.Select();
            //this.cboGVType.ForeColor = Color.FromArgb(185, 205, 165);
            //switch (eFileType)
            //{
            //    case 1:             //for form 402

            //        MainSet = new DataSet();

            //        query = "select Distinct Tran_cd, (ltrim(rtrim(inv_sr))+ltrim(rtrim(inv_no))) As inv_no from STMAIN s inner join AC_MAST a ON a.Ac_id = s.Ac_id where a.st_type in ('OUT OF STATE', 'OUT OF COUNTRY') and s.entry_ty = 'ST' ORDER BY INV_NO";

            //        //EXECUTE USP_REP_GJFORM402'','','','04/01/2014','03/31/2015','','','','',0,0,'','','','','','','','','2014-2015','And U_402srno = 2'

            //        try
            //        {
            //            SqlConnection cn1 = new SqlConnection(_ConnStr);
            //            SqlDataAdapter da1 = new SqlDataAdapter(query, cn1);
            //            da1.Fill(MainSet, "SInvNo");
            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show(ex.ToString(), appName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        this.cboInvNo.Items.Clear();
            //        this.cboInvNo.DataSource = MainSet.Tables["SInvNo"].DefaultView;
            //        this.cboInvNo.DisplayMember = "Inv_No";
            //        this.cboInvNo.ValueMember = "Tran_cd";

            //        MainSet = null;

            //        break;
            //    case 2:             //for form 403
            //        MainSet = new DataSet();

            //        query = "select Distinct Tran_cd, (ltrim(rtrim(inv_sr))+ltrim(rtrim(inv_no))) As inv_no from PTMAIN s inner join AC_MAST a ON a.Ac_id = s.Ac_id where a.st_type in ('OUT OF STATE', 'OUT OF COUNTRY') and s.entry_ty = 'PT' ORDER BY INV_NO";

            //        //EXECUTE USP_REP_GJFORM402'','','','04/01/2014','03/31/2015','','','','',0,0,'','','','','','','','','2014-2015','And U_402srno = 2'

            //        try
            //        {
            //            SqlConnection cn1 = new SqlConnection(_ConnStr);
            //            SqlDataAdapter da1 = new SqlDataAdapter(query, cn1);
            //            da1.Fill(MainSet, "PInvNo");
            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show(ex.ToString(), appName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        this.cboInvNo.Items.Clear();
            //        this.cboInvNo.DataSource = MainSet.Tables["PInvNo"].DefaultView;
            //        this.cboInvNo.DisplayMember = "Inv_No";
            //        this.cboInvNo.ValueMember = "Tran_cd";

            //        MainSet =null;

            //        break;
            //    default:
            //        break;
            //}
        }

        private void GetInvData()
        {
            
            //MainSet = new DataSet();
            
            //string SQL = "SELECT sta_dt,End_dt,City FROM Vudyog..Co_mast Where CompId=" + compId;
            string SQL = "SELECT sta_dt,End_dt,DISTRICT FROM Vudyog..Co_mast Where CompId=" + compId; //ruchit 05/01/2016
            SqlConnection con = new SqlConnection(_ConnStr);
            SqlCommand cmd = new SqlCommand(SQL, con);
            SqlDataReader r = null;

           try
            {
                con.Open();
                r = cmd.ExecuteReader();
             
                while (r.Read())
                {
                    _Sta_dt = Convert.ToDateTime(r["Sta_dt"]).ToString("MM/dd/yyyy");
                    _End_dt = Convert.ToDateTime(r["End_dt"]).ToString("MM/dd/yyyy");
                    //_strCity = Convert.ToString(r["City"]);
                    _strCity = Convert.ToString(r["DISTRICT"]); // ruchit 05/01/2016
                }
                FinYear = _Sta_dt.Substring(_Sta_dt.Length - 4, 4) + "-" + _End_dt.Substring(_End_dt.Length - 4, 4);

          
            }
            catch (Exception err)
            {
               
                MessageBox.Show(err.ToString(), appName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                if (r != null) r.Close();
                con.Close();
            }

            
        }


        private void btnExportData_Click(object sender, EventArgs e)
        {
            string RetVal = string.Empty;
            MainSet = new DataSet();

            if (this.cboInvNo.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please select the Invoice.");
                this.cboInvNo.Focus();
                return;
            }

            if (this.cboGVType.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please select the Goods Type.");
                this.cboGVType.Focus();
                return;
            }

            _strGVType = this.cboGVType.Text.Trim();
          
            if (MainSet.Tables["Data"] != null)
            {
                MainSet.Tables["Data"].Rows.Clear();
            }

            
  
            //query = "EXECUTE USP_REP_GJFORM402'','','','" + _Sta_dt + "','" + _End_dt + "','','','','',0,0,'','','','','','','','','" + FinYear + "','And U_402srno = "+eFileType+ "'";
            
            switch (eFileType)
            {
                case 1:             //for form 402
                    
                    if (this.cboGVType.Text.Trim() == "NonSpecified")
                    {
                        cFileName = "Gis_Form_402_m.xls";
                        cFinalPath = "Gis_Form_402_m_" ;
                        //query = "EXECUTE USP_REP_GJFORM402'','','IT_MAST.U_GVTYPE=`NONSPECIFIED`','" + _Sta_dt + "','" + _End_dt + "','','','','',0,0,'','','','','','','','','" + FinYear + "','And STMAIN.Tran_cd = " + cboInvNo.SelectedValue.ToString() + "'";
                        query = "EXECUTE USP_REP_GJFORM402403'','','IT_MAST.U_GVTYPE=`NONSPECIFIED`','" + _Sta_dt + "','" + _End_dt + "','','','','',0,0,'','','','','','','','','" + FinYear + "','And STMAIN.Tran_cd = " + cboInvNo.SelectedValue.ToString() + "'"; //RUCHIT 05/01/2016
                    }
                    else
                    {
                        cFileName = "Gis_Form_402_specified_m.xls";
                        cFinalPath = "Gis_Form_402_specified_m";
                        //query = "EXECUTE USP_REP_GJFORM402'','','IT_MAST.U_GVTYPE=`SPECIFIED`','" + _Sta_dt + "','" + _End_dt + "','','','','',0,0,'','','','','','','','','" + FinYear + "','And STMAIN.Tran_cd = " + cboInvNo.SelectedValue.ToString() + "'";
                        query = "EXECUTE USP_REP_GJFORM402403'','','IT_MAST.U_GVTYPE=`SPECIFIED`','" + _Sta_dt + "','" + _End_dt + "','','','','',0,0,'','','','','','','','','" + FinYear + "','And STMAIN.Tran_cd = " + cboInvNo.SelectedValue.ToString() + "'"; //RUCHIT
                    }

                    break;
                case 2:             //for form 403

                    
                    if (this.cboGVType.Text.Trim() == "NonSpecified")
                    {
                        cFileName = "Gis_Form_403_m.xls";
                        cFinalPath = "Gis_Form_403_m" ;
                        query = "EXECUTE USP_REP_GJFORM403'','','IT_MAST.U_GVTYPE=`NONSPECIFIED`','" + _Sta_dt + "','" + _End_dt + "','','','','',0,0,'','','','','','','','','" + FinYear + "','And PTMAIN.Tran_cd = " + cboInvNo.SelectedValue.ToString() + "'";
                    }
                    else
                    {
                        cFileName = "Gis_Form_403_specified_m.xls";
                        cFinalPath = "Gis_Form_403_specified_m" ;
                        query = "EXECUTE USP_REP_GJFORM403'','','IT_MAST.U_GVTYPE=`SPECIFIED`','" + _Sta_dt + "','" + _End_dt + "','','','','',0,0,'','','','','','','','','" + FinYear + "','And PTMAIN.Tran_cd = " + cboInvNo.SelectedValue.ToString() + "'";
                    }
                    break;
            }

            SqlConnection cn = new SqlConnection(_ConnStr);
            SqlDataReader rs = null;

            try
            {
               
                SqlDataAdapter da = new SqlDataAdapter(query, cn);
                da.Fill(MainSet, "Data");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), appName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                if (rs != null) rs.Close();
                cn.Close();
            }


            if (MainSet.Tables["Data"].Rows.Count > 0)
            {
                this.toolStripStatusLabel1.Text = "Writing excel file...";

                _strInvNo = this.cboInvNo.Text.Trim();
                cFinalPath = cFinalPath + _strInvNo + ".xls";

                 //this.SendToBack();
                WriteToF402 oExcel = new WriteToF402(cFileName);
                RetVal = oExcel.WriteToFile(MainSet);

                string currentDirectorypath = Environment.CurrentDirectory;
                cFinalPath = System.IO.Path.Combine(currentDirectorypath, cFinalPath);
                if (RetVal.Length <= 0)          //Added by Shrikant S. on 16/10/2015 
                {
                MessageBox.Show("Excel file has been generated at " + cFinalPath);
                //this.Close();
                }
                else
                {
                    MessageBox.Show("Issue occured while generating the file " + cFinalPath);       //Added by Shrikant S. on 16/10/2015 
                }
            }
            else
            {
                MessageBox.Show("No records found for this invoice and goods type.");
            }

            MainSet = null;
            this.toolStripStatusLabel1.Text = "";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboGVType_Validating(object sender, CancelEventArgs e)
        {
            this.getinvno();
        }

        private void getinvno()
        {
            if (this.cboGVType.Text.Trim() != "")
            {
                this.cboInvNo.Enabled = true;
                this.cboInvNo.Focus();
                MainSet = null;

                switch (eFileType)
                {
                    case 1:             //for form 402

                        MainSet = new DataSet();

                        if (this.cboGVType.Text.Trim() == "NonSpecified")
                        {
                            //query = "select DISTINCT Tran_cd, (ltrim(rtrim(inv_sr))+ltrim(rtrim(inv_no))) As inv_no from STITEM s inner join AC_MAST a ON a.Ac_id = s.Ac_id inner join it_mast I ON I.IT_CODE = s.IT_CODE where a.st_type in ('OUT OF STATE', 'OUT OF COUNTRY') and s.entry_ty = 'ST' and i.u_gvtype = 'NONSPECIFIED' ORDER BY INV_NO";
                            query = "select DISTINCT Tran_cd, (ltrim(rtrim(inv_no))) As inv_no from STITEM s inner join AC_MAST a ON a.Ac_id = s.Ac_id inner join it_mast I ON I.IT_CODE = s.IT_CODE where a.st_type in ('OUT OF STATE', 'OUT OF COUNTRY') and s.entry_ty = 'ST' and i.u_gvtype = 'NONSPECIFIED' and s.l_yn='" + FinYear + "' ORDER BY INV_NO"; //ruchit 05/01/2016
                        }
                        else
                        {
                            //query = "select DISTINCT Tran_cd, (ltrim(rtrim(inv_sr))+ltrim(rtrim(inv_no))) As inv_no from STITEM s inner join AC_MAST a ON a.Ac_id = s.Ac_id inner join it_mast I ON I.IT_CODE = s.IT_CODE where a.st_type in ('OUT OF STATE', 'OUT OF COUNTRY') and s.entry_ty = 'ST' and i.u_gvtype = 'SPECIFIED' ORDER BY INV_NO";
                            query = "select DISTINCT Tran_cd, (ltrim(rtrim(inv_no))) As inv_no from STITEM s inner join AC_MAST a ON a.Ac_id = s.Ac_id inner join it_mast I ON I.IT_CODE = s.IT_CODE where a.st_type in ('OUT OF STATE', 'OUT OF COUNTRY') and s.entry_ty = 'ST' and i.u_gvtype = 'SPECIFIED' and s.l_yn='" + FinYear + "' ORDER BY INV_NO"; //ruchit 05/01/2016
                        }

                        try
                        {
                            SqlConnection cn1 = new SqlConnection(_ConnStr);
                            SqlDataAdapter da1 = new SqlDataAdapter(query, cn1);
                            da1.Fill(MainSet, "SInvNo");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString(), appName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                        //this.cboInvNo.Items.Clear();
                        this.cboInvNo.DataSource = MainSet.Tables["SInvNo"].DefaultView;
                        this.cboInvNo.DisplayMember = "Inv_No";
                        this.cboInvNo.ValueMember = "Tran_cd";
                        this.cboInvNo.DisplayMember = "Year";
                        MainSet = null;

                        break;
                    case 2:             //for form 403
                        MainSet = new DataSet();

                        if (this.cboGVType.Text.Trim() == "NonSpecified")
                        {
                            query = "select DISTINCT Tran_cd, (ltrim(rtrim(inv_sr))+ltrim(rtrim(inv_no))) As inv_no from PTITEM s inner join AC_MAST a ON a.Ac_id = s.Ac_id inner join it_mast I ON I.IT_CODE = s.IT_CODE where a.st_type in ('OUT OF STATE', 'OUT OF COUNTRY') and s.entry_ty = 'PT' and i.u_gvtype = 'NONSPECIFIED' ORDER BY INV_NO";
                        }
                        else
                        {
                            query = "select DISTINCT Tran_cd, (ltrim(rtrim(inv_sr))+ltrim(rtrim(inv_no))) As inv_no from PTITEM s inner join AC_MAST a ON a.Ac_id = s.Ac_id inner join it_mast I ON I.IT_CODE = s.IT_CODE where a.st_type in ('OUT OF STATE', 'OUT OF COUNTRY') and s.entry_ty = 'PT' and i.u_gvtype = 'SPECIFIED' ORDER BY INV_NO";
                        }
                        try
                        {
                            SqlConnection cn1 = new SqlConnection(_ConnStr);
                            SqlDataAdapter da1 = new SqlDataAdapter(query, cn1);
                            da1.Fill(MainSet, "PInvNo");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString(), appName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                        //this.cboInvNo.Items.Clear();
                        this.cboInvNo.DataSource = MainSet.Tables["PInvNo"].DefaultView;
                        this.cboInvNo.DisplayMember = "Inv_No";
                        this.cboInvNo.ValueMember = "Tran_cd";

                        MainSet = null;

                        break;
                    default:
                        break;
                }
            }

        }

       
        private void cboGVType_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.getinvno();
            }
        }

        private void cboGVType_Leave(object sender, EventArgs e)
        {

        }

        private void cboGVType_Click(object sender, EventArgs e)
        {

        }

       

               

        /*
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
            folderBrowserDialog.Description = "Select Output path to save excel file ";
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                this.txtOutputPath.Text = folderBrowserDialog.SelectedPath.ToString();
            }
        }
        */
       
    }
}
