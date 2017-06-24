using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace eFillingExtraction
{
    class ReturnVer1
    {
        private ReturnType _ReturnType;
        private FormType _FormType;
        private string _Quarter;

        //public static int NoofTabs = 2;
        //public static string SheetNames = "Challan,Deductee";
        public DataSet MainSet = new DataSet();

        public ReturnVer1(ReturnType returnType, FormType formType, string quarter)
        {
            _ReturnType = returnType;
            _FormType = formType;
            _Quarter = quarter;
        }

        public void GetTempDataTables()
        {

            #region Company Details
            //if (_FormType == FormType.Form24Q) Commented By Kishor A. for Bug-26391 on 06/07/2015
            //{
                if (MainSet.Tables.Contains("General"))
                    MainSet.Tables.Remove("General");
                if (!MainSet.Tables.Contains("General"))
                {
                    DataTable CompanyTable = new DataTable("General");
                    CompanyTable.Columns.Add(new DataColumn("TAN", typeof(string)));
                    CompanyTable.Columns.Add(new DataColumn("AddressChange", typeof(string)));
                    CompanyTable.Columns.Add(new DataColumn("RAddressChange", typeof(string)));
                    CompanyTable.Columns.Add(new DataColumn("IsReturnFiled", typeof(string)));
                    CompanyTable.Columns.Add(new DataColumn("TDSAcknowNo", typeof(string)));
                    MainSet.Tables.Add(CompanyTable);
                }
            //}
            #endregion

            switch (_ReturnType)
            {
                case ReturnType.eTDS:
                    #region Challan Table
                    if (MainSet.Tables.Contains("Challan"))
                        MainSet.Tables.Remove("Challan");
                    if (!MainSet.Tables.Contains("Challan"))
                    {
                        DataTable ChallanTable = new DataTable("Challan");
                        ChallanTable.Columns.Add(new DataColumn("csrno", typeof(string)));
                        ChallanTable.Columns.Add(new DataColumn("atdsamt", typeof(decimal)));
                        ChallanTable.Columns.Add(new DataColumn("ascamt", typeof(decimal)));
                        ChallanTable.Columns.Add(new DataColumn("aecamt", typeof(decimal)));
                        ChallanTable.Columns.Add(new DataColumn("interest", typeof(decimal)));
                        ChallanTable.Columns.Add(new DataColumn("fees", typeof(decimal)));
                        ChallanTable.Columns.Add(new DataColumn("othersamt", typeof(decimal)));
                        //ChallanTable.Columns.Add(new DataColumn("atotamt", typeof(decimal)));
                        ChallanTable.Columns.Add(new DataColumn("bsrcode", typeof(string)));
                        ChallanTable.Columns.Add(new DataColumn("u_chaldt", typeof(string)));
                        ChallanTable.Columns.Add(new DataColumn("u_chalno", typeof(string)));
                        ChallanTable.Columns.Add(new DataColumn("paidbybook", typeof(string)));
                        ChallanTable.Columns.Add(new DataColumn("interest_alloc", typeof(decimal)));
                        ChallanTable.Columns.Add(new DataColumn("othersamt_alloc", typeof(decimal)));
                        ChallanTable.Columns.Add(new DataColumn("minor_head", typeof(string)));
                        ChallanTable.Columns.Add(new DataColumn("NilIndicator", typeof(string)));
                        ChallanTable.Columns.Add(new DataColumn("remarks", typeof(string)));
                        MainSet.Tables.Add(ChallanTable);
                    }
                    #endregion

                    #region Deductee Table
                    if (MainSet.Tables.Contains("Deductee"))
                        MainSet.Tables.Remove("Deductee");
                    if (!MainSet.Tables.Contains("Deductee"))
                    {
                        DataTable DeducteeTable = new DataTable("Deductee");
                        DeducteeTable.Columns.Add(new DataColumn("dsrno", typeof(string)));
                        DeducteeTable.Columns.Add(new DataColumn("ded_refno", typeof(string)));
                        DeducteeTable.Columns.Add(new DataColumn("csrno", typeof(string)));
                        DeducteeTable.Columns.Add(new DataColumn("section", typeof(string)));
                        DeducteeTable.Columns.Add(new DataColumn("i_tax", typeof(string)));
                        DeducteeTable.Columns.Add(new DataColumn("party_nm", typeof(string)));
                        DeducteeTable.Columns.Add(new DataColumn("date", typeof(string)));
                        DeducteeTable.Columns.Add(new DataColumn("tdsonamt", typeof(decimal)));
                        DeducteeTable.Columns.Add(new DataColumn("tdsamt", typeof(decimal)));
                        DeducteeTable.Columns.Add(new DataColumn("scamt", typeof(decimal)));
                        DeducteeTable.Columns.Add(new DataColumn("ecamt", typeof(decimal)));
                        DeducteeTable.Columns.Add(new DataColumn("tottds", typeof(decimal)));
                        DeducteeTable.Columns.Add(new DataColumn("ddate", typeof(string)));
                        DeducteeTable.Columns.Add(new DataColumn("reasonfor", typeof(string)));
                        DeducteeTable.Columns.Add(new DataColumn("ded_code", typeof(string)));
                        DeducteeTable.Columns.Add(new DataColumn("ded_rate", typeof(decimal)));
                        DeducteeTable.Columns.Add(new DataColumn("certino", typeof(string)));

                        if (this._FormType == FormType.Form27Q)
                        {
                            DeducteeTable.Columns.Add(new DataColumn("tdsaper", typeof(string)));
                            DeducteeTable.Columns.Add(new DataColumn("nature_rem", typeof(string)));
                            DeducteeTable.Columns.Add(new DataColumn("acknowledge", typeof(string)));
                            DeducteeTable.Columns.Add(new DataColumn("country_code", typeof(string)));
                            DeducteeTable.Columns.Add(new DataColumn("grossingUp", typeof(string)));
                        }

                        MainSet.Tables.Add(DeducteeTable);
                    }

                    
                    #endregion

                    #region Salary Details
                    if (this._FormType == FormType.Form24Q && _Quarter=="Q4" )
                    {
                        if (MainSet.Tables.Contains("Salary"))
                            MainSet.Tables.Remove("Salary");

                        DataTable SalDetailTable = new DataTable("Salary");
                        SalDetailTable.Columns.Add(new DataColumn("srno", typeof(string)));
                        SalDetailTable.Columns.Add(new DataColumn("PAN", typeof(string)));
                        SalDetailTable.Columns.Add(new DataColumn("EmployeeName", typeof(string)));
                        SalDetailTable.Columns.Add(new DataColumn("DeducteeType", typeof(string)));
                        SalDetailTable.Columns.Add(new DataColumn("EStartDt", typeof(string)));
                        SalDetailTable.Columns.Add(new DataColumn("EEndDt", typeof(string)));
                        SalDetailTable.Columns.Add(new DataColumn("NetPayment", typeof(decimal)));
                        SalDetailTable.Columns.Add(new DataColumn("EnterAmt", typeof(decimal)));
                        SalDetailTable.Columns.Add(new DataColumn("pTaxAmt", typeof(decimal)));
                        SalDetailTable.Columns.Add(new DataColumn("Tot_deduc", typeof(decimal)));
                        SalDetailTable.Columns.Add(new DataColumn("TotChgIncome", typeof(decimal)));
                        SalDetailTable.Columns.Add(new DataColumn("LossFromProp", typeof(decimal)));

                        SalDetailTable.Columns.Add(new DataColumn("GrossInc", typeof(decimal)));
                        SalDetailTable.Columns.Add(new DataColumn("u_s_80C_80CCD", typeof(decimal)));
                        SalDetailTable.Columns.Add(new DataColumn("u_s_80CCG", typeof(decimal)));
                        SalDetailTable.Columns.Add(new DataColumn("u_s_VIA", typeof(decimal)));
                        SalDetailTable.Columns.Add(new DataColumn("Tot_deducVIA", typeof(decimal)));
                        SalDetailTable.Columns.Add(new DataColumn("TotTaxableInc", typeof(decimal)));
                        SalDetailTable.Columns.Add(new DataColumn("IncomeTax", typeof(decimal)));
                        SalDetailTable.Columns.Add(new DataColumn("Surcharge", typeof(decimal)));
                        SalDetailTable.Columns.Add(new DataColumn("EduCess", typeof(decimal)));
                        SalDetailTable.Columns.Add(new DataColumn("TaxRelief", typeof(decimal)));
                        SalDetailTable.Columns.Add(new DataColumn("NetTax", typeof(decimal)));
                        SalDetailTable.Columns.Add(new DataColumn("TotalTDS", typeof(decimal)));
                        SalDetailTable.Columns.Add(new DataColumn("TaxShortFall", typeof(decimal)));

                        MainSet.Tables.Add(SalDetailTable);

                    }
                    #endregion
                    break;
                case ReturnType.eTCS:
                    #region Challan Table
                    if (MainSet.Tables.Contains("Challan"))
                        MainSet.Tables.Remove("Challan");
                    if (!MainSet.Tables.Contains("Challan"))
                    {
                        DataTable ChallanTable = new DataTable("Challan");
                        ChallanTable.Columns.Add(new DataColumn("csrno", typeof(string)));
                        ChallanTable.Columns.Add(new DataColumn("atdsamt", typeof(decimal)));
                        ChallanTable.Columns.Add(new DataColumn("ascamt", typeof(decimal)));
                        ChallanTable.Columns.Add(new DataColumn("aecamt", typeof(decimal)));
                        ChallanTable.Columns.Add(new DataColumn("interest", typeof(decimal)));
                        ChallanTable.Columns.Add(new DataColumn("fees", typeof(decimal)));
                        ChallanTable.Columns.Add(new DataColumn("othersamt", typeof(decimal)));
                        //ChallanTable.Columns.Add(new DataColumn("atotamt", typeof(decimal)));
                        ChallanTable.Columns.Add(new DataColumn("bsrcode", typeof(string)));
                        ChallanTable.Columns.Add(new DataColumn("u_chaldt", typeof(string)));
                        ChallanTable.Columns.Add(new DataColumn("u_chalno", typeof(string)));
                        ChallanTable.Columns.Add(new DataColumn("paidbybook", typeof(string)));
                        ChallanTable.Columns.Add(new DataColumn("interest_alloc", typeof(decimal)));
                        ChallanTable.Columns.Add(new DataColumn("othersamt_alloc", typeof(decimal)));
                        ChallanTable.Columns.Add(new DataColumn("minor_head", typeof(string)));
                        ChallanTable.Columns.Add(new DataColumn("NilIndicator", typeof(string)));
                        ChallanTable.Columns.Add(new DataColumn("remarks", typeof(string)));
                        MainSet.Tables.Add(ChallanTable);
                    }
                    #endregion

                    #region Party Table
                    if (MainSet.Tables.Contains("Party"))
                        MainSet.Tables.Remove("Party");
                    if (!MainSet.Tables.Contains("Party"))
                    {
                        DataTable PartyTable = new DataTable("Party");
                        PartyTable.Columns.Add(new DataColumn("dsrno", typeof(string)));
                        PartyTable.Columns.Add(new DataColumn("section", typeof(string)));
                        PartyTable.Columns.Add(new DataColumn("csrno", typeof(string)));

                        PartyTable.Columns.Add(new DataColumn("i_tax", typeof(string)));
                        PartyTable.Columns.Add(new DataColumn("ded_refno", typeof(string)));
                        PartyTable.Columns.Add(new DataColumn("party_nm", typeof(string)));
                        PartyTable.Columns.Add(new DataColumn("date", typeof(string)));
                        PartyTable.Columns.Add(new DataColumn("net_amt", typeof(decimal)));
                        PartyTable.Columns.Add(new DataColumn("tdsamt", typeof(decimal)));
                        PartyTable.Columns.Add(new DataColumn("scamt", typeof(decimal)));
                        PartyTable.Columns.Add(new DataColumn("ecamt", typeof(decimal)));
                        PartyTable.Columns.Add(new DataColumn("tottds", typeof(decimal)));
                        PartyTable.Columns.Add(new DataColumn("ddate", typeof(string)));
                        PartyTable.Columns.Add(new DataColumn("reasonfor", typeof(string)));
                        PartyTable.Columns.Add(new DataColumn("ded_code", typeof(string)));
                        PartyTable.Columns.Add(new DataColumn("tdsonamt", typeof(decimal)));
                        PartyTable.Columns.Add(new DataColumn("ded_rate", typeof(decimal)));
                        PartyTable.Columns.Add(new DataColumn("certino", typeof(string)));

                        //DeducteeTable.Columns.Add(new DataColumn("Mode", typeof(string)));
                        //DeducteeTable.Columns.Add(new DataColumn("atotamt", typeof(decimal)));

                        MainSet.Tables.Add(PartyTable);
                    }
                    #endregion
                    break;
                default:
                    break;
            }

        }
        public string GetFinalData(DataTable Data, DataTable SalDetails)
        {
            this.GetFinalData(Data);

            #region Salary Details
            if (MainSet.Tables["Salary"] != null)
            {
                MainSet.Tables["Salary"].Rows.Clear();
            }
            try
            {
                for (int i = 0; i < SalDetails.Rows.Count; i++)
                {

                    #region Transforming Data to Salary Table
                    DataRow SalaryRow = MainSet.Tables["Salary"].NewRow();
                    SalaryRow["srno"] = Convert.ToString(i + 1).Trim();
                    SalaryRow["PAN"] = Convert.ToString(SalDetails.Rows[i]["PAN"]).Trim();
                    SalaryRow["EmployeeName"] = Convert.ToString(SalDetails.Rows[i]["EmployeeName"]).Trim();
                    SalaryRow["DeducteeType"] = Convert.ToString(SalDetails.Rows[i]["DeducteeType"]).Trim();
                    SalaryRow["EStartDt"] = Convert.ToDateTime(SalDetails.Rows[i]["EStartDt"]).ToString("dd/MM/yyyy").Trim();
                    SalaryRow["EEndDt"] = Convert.ToDateTime(SalDetails.Rows[i]["EEndDt"]).ToString("dd/MM/yyyy").Trim();
                    SalaryRow["NetPayment"] = Convert.ToDecimal(SalDetails.Rows[i]["NetPayment"]);
                    SalaryRow["EnterAmt"] = Convert.ToDecimal(SalDetails.Rows[i]["EnterAmt"]);
                    SalaryRow["pTaxAmt"] = Convert.ToDecimal(SalDetails.Rows[i]["pTaxAmt"]);
                    SalaryRow["Tot_deduc"] = Convert.ToDecimal(SalDetails.Rows[i]["Tot_deduc"]);
                    SalaryRow["TotChgIncome"] = Convert.ToDecimal(SalDetails.Rows[i]["TotChgIncome"]);
                    SalaryRow["LossFromProp"] = Convert.ToDecimal(SalDetails.Rows[i]["LossFromProp"]);
                    SalaryRow["GrossInc"] = Convert.ToDecimal(SalDetails.Rows[i]["GrossInc"]);
                    SalaryRow["u_s_80C_80CCD"] = Convert.ToDecimal(SalDetails.Rows[i]["u_s_80C_80CCD"]);
                    SalaryRow["u_s_80CCG"] = Convert.ToDecimal(SalDetails.Rows[i]["u_s_80CCG"]);
                    SalaryRow["u_s_VIA"] = Convert.ToDecimal(SalDetails.Rows[i]["u_s_VIA"]);
                    SalaryRow["Tot_deducVIA"] = Convert.ToDecimal(SalDetails.Rows[i]["Tot_deducVIA"]);
                    SalaryRow["TotTaxableInc"] = Convert.ToDecimal(SalDetails.Rows[i]["TotTaxableInc"]);
                    SalaryRow["IncomeTax"] = Convert.ToDecimal(SalDetails.Rows[i]["IncomeTax"]);
                    SalaryRow["Surcharge"] = Convert.ToDecimal(SalDetails.Rows[i]["Surcharge"]);
                    SalaryRow["EduCess"] = Convert.ToDecimal(SalDetails.Rows[i]["EduCess"]);
                    SalaryRow["TaxRelief"] = Convert.ToDecimal(SalDetails.Rows[i]["TaxRelief"]);
                    SalaryRow["NetTax"] = Convert.ToDecimal(SalDetails.Rows[i]["NetTax"]);
                    SalaryRow["TotalTDS"] = Convert.ToDecimal(SalDetails.Rows[i]["TotalTDS"]);
                    SalaryRow["TaxShortFall"] = Convert.ToDecimal(SalDetails.Rows[i]["TaxShortFall"]);

                    MainSet.Tables["Salary"].Rows.Add(SalaryRow);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            #endregion
            return string.Empty;
        }
        public string GetFinalData(DataTable Data)
        {

            switch (_ReturnType)
            {
                case ReturnType.eTDS:
                    #region TDS
                    //Added Commented By Kishor A. for Bug-26391 on 06/07/2015 Start..
                    if (MainSet.Tables["General"] != null) 
                    {
                        MainSet.Tables["General"].Rows.Clear();
                    }
                    //Added Commented By Kishor A. for Bug-26391 on 06/07/2015 End.

                    if (MainSet.Tables["Challan"] != null)
                    {
                        MainSet.Tables["Challan"].Rows.Clear();
                    }
                    if (MainSet.Tables["Deductee"] != null)
                    {
                        MainSet.Tables["Deductee"].Rows.Clear();
                    }
                    try
                    {
                        string tmpGenNo = "0"; //Added Commented By Kishor A. for Bug-26391 on 06/07/2015
                        string tmpChalno = "0";
                        for (int i = 0; i < Data.Rows.Count; i++)
                        {
                            string remark = Convert.ToString(Data.Rows[i]["reasonfor"]).Trim();
                            #region Transforming Raw Data to Deductee Table
                            DataRow dedRow = MainSet.Tables["Deductee"].NewRow();
                            dedRow["dsrno"] = Convert.ToString(Data.Rows[i]["dsrno"]).Trim();
                            dedRow["csrno"] = Convert.ToString(Data.Rows[i]["csrno"]).Trim();
                            dedRow["ded_refno"] = Convert.ToString(Data.Rows[i]["ded_refno"]).Trim();
                            dedRow["ded_code"] = Convert.ToString(Data.Rows[i]["ded_code"]).Trim();

                            dedRow["i_tax"] = Convert.ToString(Data.Rows[i]["i_tax"]).Trim();
                            dedRow["party_nm"] = Convert.ToString(Data.Rows[i]["party_nm"]).Trim();
                            dedRow["section"] = Convert.ToString(Data.Rows[i]["section"]).Trim();
                            dedRow["date"] = (Convert.ToDateTime(Data.Rows[i]["date"])).ToString("dd/MM/yyyy").Trim();
                            dedRow["tdsonamt"] = Convert.ToDecimal(Data.Rows[i]["tdsonamt"]);
                            dedRow["tdsamt"] = Convert.ToDecimal(Data.Rows[i]["tdsamt"]);
                            dedRow["scamt"] = Convert.ToDecimal(Data.Rows[i]["scamt"]);
                            dedRow["ecamt"] = Convert.ToDecimal(Data.Rows[i]["ecamt"]) + Convert.ToDecimal(Data.Rows[i]["hcamt"]);

                            dedRow["tottds"] = Convert.ToDecimal(Data.Rows[i]["tdsamt"]) + Convert.ToDecimal(Data.Rows[i]["scamt"]) + Convert.ToDecimal(Data.Rows[i]["ecamt"]) + Convert.ToDecimal(Data.Rows[i]["hcamt"]);
                            //dedRow["atotamt"] = Convert.ToDecimal(Data.Rows[i]["atotamt"]);

                            dedRow["ddate"] = (remark == "T" || remark == "Y" ? "" : (Convert.ToDateTime(Data.Rows[i]["ddate"])).ToString("dd/MM/yyyy").Trim());
                            dedRow["ded_rate"] = Convert.ToDecimal(Data.Rows[i]["ded_rate"]);
                            dedRow["reasonfor"] = Convert.ToString(Data.Rows[i]["reasonfor"]).Trim();
                            dedRow["certino"] = Convert.ToString(Data.Rows[i]["certino"]).Trim();

                            if (_FormType == FormType.Form27Q)
                            {
                                dedRow["tdsaper"] = Convert.ToString(Data.Rows[i]["tdsrateact"]).Trim();
                                dedRow["nature_rem"] = Convert.ToString(Data.Rows[i]["natrem_code"]).Trim();
                                dedRow["acknowledge"] = Convert.ToString(Data.Rows[i]["ack15ca"]).Trim();
                                dedRow["country_code"] = Convert.ToString(Data.Rows[i]["tds_code"]).Trim();
                                dedRow["grossingUp"] = string.Empty;
                            }

                            MainSet.Tables["Deductee"].Rows.Add(dedRow);
                            dedRow = null;
                            #endregion

                            if (tmpChalno != Convert.ToString(Data.Rows[i]["csrno"]).Trim())
                            {
                                #region Transforming Raw Data to Challan Table
                                tmpChalno = Convert.ToString(Data.Rows[i]["csrno"]).Trim();
                                DataRow chalRow = MainSet.Tables["Challan"].NewRow();
                                chalRow["csrno"] = Convert.ToString(Data.Rows[i]["csrno"]).Trim();

                                chalRow["atdsamt"] = (decimal)Data.Compute("sum(atdsamt)", "csrno=" + tmpChalno);
                                chalRow["ascamt"] = (decimal)Data.Compute("sum(ascamt)", "csrno=" + tmpChalno);
                                chalRow["aecamt"] = (decimal)Data.Compute("sum(aecamt)", "csrno=" + tmpChalno);

                                chalRow["interest"] = (decimal)Data.Compute("sum(interest)", "csrno=" + tmpChalno + " and  dsrno=1");
                                chalRow["fees"] = (decimal)Data.Compute("sum(fees)", "csrno=" + tmpChalno + " and  dsrno=1");
                                chalRow["othersamt"] = (decimal)Data.Compute("sum(othersamt)", "csrno=" + tmpChalno + " and  dsrno=1");

                                //Commented By Shrikant S. on 22/08/2013    
                                //chalRow["interest"] = (decimal)Data.Compute("sum(interest)", "csrno=" + tmpChalno);
                                //chalRow["fees"] = (decimal)Data.Compute("sum(fees)", "csrno=" + tmpChalno);
                                //chalRow["othersamt"] = (decimal)Data.Compute("sum(othersamt)", "csrno=" + tmpChalno);

                                //chalRow["atotamt"] = (decimal)Data.Compute("sum(atotamt)", "Trim(u_chalno)='" + tmpChalno + "'");
                                chalRow["paidbybook"] = Convert.ToString(Data.Rows[i]["paidbybook"]).Trim();
                                chalRow["bsrcode"] = Convert.ToString(Data.Rows[i]["bsrcode"]).Trim();
                                chalRow["u_chalno"] = Convert.ToString(Data.Rows[i]["u_chalno"]).Trim();
                                chalRow["u_chaldt"] = (Convert.ToDateTime(Data.Rows[i]["u_chaldt"])).ToString("dd/MM/yyyy").Trim();
                                //chalRow["minor_head"] = (remark == "T" || remark == "Y" ? "" : Convert.ToString(Data.Rows[i]["minor_head"]).Trim());  //Commented 19/10/2013 
                                Decimal Tdsamt = 0;
                                Tdsamt = Convert.ToDecimal(chalRow["atdsamt"]) + Convert.ToDecimal(chalRow["ascamt"]) + Convert.ToDecimal(chalRow["aecamt"]) + Convert.ToDecimal(chalRow["interest"])+Convert.ToDecimal(chalRow["fees"])+Convert.ToDecimal(chalRow["othersamt"]);
                                chalRow["minor_head"] = (Tdsamt<=0 ? "" : Convert.ToString(Data.Rows[i]["minor_head"]).Trim());    //Added on 19/10/2013
                                chalRow["interest_alloc"] = Convert.ToDecimal(0.00);
                                chalRow["othersamt_alloc"] = Convert.ToDecimal(0.00);
                                chalRow["NilIndicator"] = (remark == "T" || remark == "Y" || (Convert.ToDecimal(chalRow["atdsamt"]) == 0) ? "Y" : "N");
                                chalRow["remarks"] = string.Empty;

                                MainSet.Tables["Challan"].Rows.Add(chalRow);
                                chalRow = null;
                                #endregion
                            }
                            //Added Commented By Kishor A. for Bug-26391 on 06/07/2015 Start..
                            if (tmpGenNo != Convert.ToString(Data.Rows[i]["TAN"]).Trim())
                            {
                                #region Transforming Raw Data to General Table
                                tmpGenNo = Convert.ToString(Data.Rows[i]["TAN"]).Trim();
                                DataRow GenRow = MainSet.Tables["General"].NewRow();
                                GenRow["TAN"] = Convert.ToString(Data.Rows[i]["TAN"]).Trim();
                                GenRow["AddressChange"] = Convert.ToString(Data.Rows[i]["AddressChange"]).Trim();
                                GenRow["RAddressChange"] = Convert.ToString(Data.Rows[i]["RAddressChange"]).Trim();
                                GenRow["IsReturnFiled"] = Convert.ToString(Data.Rows[i]["IsReturnFiled"]).Trim();
                                GenRow["TDSAcknowNo"] = Convert.ToString(Data.Rows[i]["TDSAcknowNo"]).Trim();

                                MainSet.Tables["General"].Rows.Add(GenRow);
                                GenRow = null;
                                #endregion
                                //Added Commented By Kishor A. for Bug-26391 on 06/07/2015 End..
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                    #endregion

                    break;
                case ReturnType.eTCS:
                    #region TCS
                    if (MainSet.Tables["Challan"] != null)
                    {
                        MainSet.Tables["Challan"].Rows.Clear();
                    }
                    if (MainSet.Tables["Party"] != null)
                    {
                        MainSet.Tables["Party"].Rows.Clear();
                    }
                    try
                    {
                        string tmpChalno = "0";
                        for (int i = 0; i < Data.Rows.Count; i++)
                        {
                            string remark = Convert.ToString(Data.Rows[i]["reasonfor"]).Trim();
                            #region Transforming Raw Data to Party Table
                            DataRow dedRow = MainSet.Tables["Party"].NewRow();
                            dedRow["dsrno"] = Convert.ToString(Data.Rows[i]["dsrno"]).Trim();
                            dedRow["section"] = Convert.ToString(Data.Rows[i]["section"]).Trim();
                            dedRow["csrno"] = Convert.ToString(Data.Rows[i]["csrno"]).Trim();
                            dedRow["i_tax"] = Convert.ToString(Data.Rows[i]["i_tax"]).Trim();
                            dedRow["ded_refno"] = Convert.ToString(Data.Rows[i]["ded_refno"]).Trim();
                            dedRow["party_nm"] = Convert.ToString(Data.Rows[i]["party_nm"]).Trim();
                            dedRow["date"] = Convert.ToDateTime(Data.Rows[i]["date"]).ToString("dd/MM/yyyy").Trim();
                            dedRow["net_amt"] = Convert.ToDecimal(Data.Rows[i]["net_amt"]);
                            dedRow["tdsamt"] = Convert.ToDecimal(Data.Rows[i]["tdsamt"]);
                            dedRow["scamt"] = Convert.ToDecimal(Data.Rows[i]["scamt"]);
                            dedRow["ecamt"] = Convert.ToDecimal(Data.Rows[i]["ecamt"]) + Convert.ToDecimal(Data.Rows[i]["hcamt"]);
                            dedRow["tottds"] = Convert.ToDecimal(Data.Rows[i]["tdsamt"]) + Convert.ToDecimal(Data.Rows[i]["scamt"]) + Convert.ToDecimal(Data.Rows[i]["ecamt"]) + Convert.ToDecimal(Data.Rows[i]["hcamt"]);
                            dedRow["ddate"] = (remark == "T" || remark == "Y" ? "" : Convert.ToDateTime(Data.Rows[i]["ddate"]).ToString("dd/MM/yyyy").Trim());
                            dedRow["reasonfor"] = Convert.ToString(Data.Rows[i]["reasonfor"]).Trim();
                            dedRow["ded_code"] = Convert.ToString(Data.Rows[i]["ded_code"]).Trim();

                            dedRow["tdsonamt"] = Convert.ToDecimal(Data.Rows[i]["tdsonamt"]);
                            //dedRow["atotamt"] = Convert.ToDecimal(Data.Rows[i]["atotamt"]);
                            dedRow["ded_rate"] = Convert.ToDecimal(Data.Rows[i]["ded_rate"]);
                            dedRow["certino"] = Convert.ToString(Data.Rows[i]["certino"]).Trim();
                            MainSet.Tables["Party"].Rows.Add(dedRow);
                            dedRow = null;
                            #endregion

                            if (tmpChalno != Convert.ToString(Data.Rows[i]["u_chalno"]).Trim())
                            {
                                #region Transforming Raw Data to Challan Table
                                tmpChalno = Convert.ToString(Data.Rows[i]["csrno"]).Trim();
                                DataRow chalRow = MainSet.Tables["Challan"].NewRow();
                                chalRow["csrno"] = Convert.ToString(Data.Rows[i]["csrno"]).Trim();

                                chalRow["atdsamt"] = (decimal)Data.Compute("sum(atdsamt)", "csrno=" + tmpChalno);
                                chalRow["ascamt"] = (decimal)Data.Compute("sum(ascamt)", "csrno=" + tmpChalno);
                                chalRow["aecamt"] = (decimal)Data.Compute("sum(aecamt)", "csrno=" + tmpChalno);

                                //Added By Shrikant S. on 22/08/2013 
                                chalRow["interest"] = (decimal)Data.Compute("sum(interest)", "csrno=" + tmpChalno + " and  dsrno=1");
                                chalRow["fees"] = (decimal)Data.Compute("sum(fees)", "csrno=" + tmpChalno + " and  dsrno=1");
                                chalRow["othersamt"] = (decimal)Data.Compute("sum(othersamt)", "csrno=" + tmpChalno + " and  dsrno=1");
                                //Commented By Shrikant S. on 22/08/2013 
                                //chalRow["interest"] = (decimal)Data.Compute("sum(interest)", "csrno=" + tmpChalno);
                                //chalRow["fees"] = (decimal)Data.Compute("sum(fees)", "csrno=" + tmpChalno);
                                //chalRow["othersamt"] = (decimal)Data.Compute("sum(othersamt)", "csrno=" + tmpChalno);

                                //chalRow["atotamt"] = (decimal)Data.Compute("sum(atotamt)", "Trim(u_chalno)='" + tmpChalno + "'");
                                chalRow["paidbybook"] = Convert.ToString(Data.Rows[i]["paidbybook"]).Trim();
                                chalRow["bsrcode"] = Convert.ToString(Data.Rows[i]["bsrcode"]).Trim();
                                chalRow["u_chalno"] = tmpChalno;
                                chalRow["u_chaldt"] = Convert.ToDateTime(Data.Rows[i]["u_chaldt"]).ToString("dd/MM/yyyy").Trim();
                                chalRow["minor_head"] = (remark == "T" || remark == "Y" ? "" : Convert.ToString(Data.Rows[i]["minor_head"]).Trim());
                                chalRow["interest_alloc"] = Convert.ToDecimal(0.00);
                                chalRow["othersamt_alloc"] = Convert.ToDecimal(0.00);
                                chalRow["NilIndicator"] = (remark == "T" || remark == "Y" || (Convert.ToDecimal(chalRow["atdsamt"]) == 0) ? "Y" : "N");
                                chalRow["remarks"] = string.Empty;
                                MainSet.Tables["Challan"].Rows.Add(chalRow);
                                chalRow = null;
                                #endregion
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                    #endregion
                    break;
                default:
                    break;
            }

            return string.Empty;
        }

    }
}
