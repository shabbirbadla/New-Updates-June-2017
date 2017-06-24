using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Collections;
using DataAccess_Net;

namespace DynamicFormClass
{
    public class clsDynamicForm
    {
        /// <summary>
        /// Save Master form Details
        /// </summary>
        /// <param name="strCaption"></param>
        /// <param name="strCode"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public int SaveMasterFormDetails(string strCaption, string strCode, string tableName,
                                         DataAccess_Net.clsDataAccess oDataAccess)
        {
            List<clsParam> colParams;
            clsParam objParam;
            int masterFormId = 0;

            string strSQL;

            try
            {
                strSQL = "INSERT INTO [DBO].[FORM_MASTER](CAPTION, CODE, TABLE_NAME)";
                strSQL += "VALUES(?, ?, ?);";

                colParams = new List<clsParam>();
                objParam = new clsParam("caption", strCaption, clsParam.pType.pString, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("code", strCode, clsParam.pType.pString, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("tableName", tableName, clsParam.pType.pString, colParams, false, null,
                                        clsParam.pInOut.pIn);

                masterFormId = oDataAccess.ExecuteSQLStatement(strSQL, colParams, 20, false);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objParam = null;
                colParams = null;
                ////oDataAccess = null;
            }
            return masterFormId;
        }

        public int UpdateMasterFormDetails(int intMasterFormID, string strCaption, string strCode, string tableName,
                                           DataAccess_Net.clsDataAccess oDataAccess)
        {
            List<clsParam> colParams;
            clsParam objParam;
            int masterFormId = 0;

            string strSQL;

            try
            {
                strSQL = "UPDATE [TEST].[DBO].[FORM_MASTER] SET CAPTION = ?";
                strSQL += " WHERE ID = ? AND CODE = ?";

                colParams = new List<clsParam>();
                objParam = new clsParam("caption", strCaption, clsParam.pType.pString, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("id", intMasterFormID, clsParam.pType.pLong, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("code", strCode, clsParam.pType.pString, colParams, false, null,
                                        clsParam.pInOut.pIn);
                //objParam = new clsParam("tableName", tableName, clsParam.pType.pString, colParams, false, null,
                //                        clsParam.pInOut.pIn);                

                oDataAccess.ExecuteSQLStatement(strSQL, colParams, 20, true);
                masterFormId = 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objParam = null;
                colParams = null;
                //oDataAccess = null;
            }
            return masterFormId;
        }

        public int SaveTabControlDetails(string strFormCode, string strCaption, string strCode, int tabOrder,
                                         DataAccess_Net.clsDataAccess oDataAccess)
        {
            //oDataAccess = new DataAccess_Net.clsDataAccess();
            List<clsParam> colParams;
            clsParam objParam;
            int tabControlId = 0;

            string strSQL;

            try
            {
                strSQL = "INSERT INTO [DBO].[TAB_CONTROLS](CAPTION, CODE, TAB_ORDER, FORM_CODE)";
                strSQL += "VALUES(?, ?, ?, ?)";

                colParams = new List<clsParam>();
                objParam = new clsParam("caption", strCaption, clsParam.pType.pString, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("code", strCode, clsParam.pType.pString, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("order", tabOrder, clsParam.pType.pLong, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("form_code", strFormCode, clsParam.pType.pString, colParams, false, null,
                                        clsParam.pInOut.pIn);

                tabControlId = oDataAccess.ExecuteSQLStatement(strSQL, colParams, 20, false);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objParam = null;
                colParams = null;
                //oDataAccess = null;
            }
            return tabControlId;
        }

        public int SaveFieldDetails(string strTabCode, int fieldOrder, string strCaption, string tooltip, int mandatory,
            string fieldname, string datatype, int size, int isDecimal, int isSearchField, string inputMask,
            string helpQuery, string remarks, string whenCondition, string defaultValue, string validation,
            int internalUse, string strFormCode, DataAccess_Net.clsDataAccess oDataAccess)
        {
            //oDataAccess = new DataAccess_Net.clsDataAccess();
            List<clsParam> colParams;
            clsParam objParam;
            int field_Id = 0;

            string strSQL;

            try
            {
                strSQL = "INSERT INTO [DBO].[FIELD_MASTER](SELECTEDTAB, FIELD_ORDER, CAPTION, TOOLTIP, MANDATORY, ";
                strSQL += "FIELDNAME, DATATYPE, SIZE, [DECIMAL], [SearchField], INPUTMASK, ";
                strSQL += "HELPQUERY, REMARKS, WHENCONDITION, DEFAULTVALUE, [VALIDATION], ";
                strSQL += "INTERNALUSE, FORM_CODE, TAB_CODE) ";
                strSQL += "VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

                colParams = new List<clsParam>();
                objParam = new clsParam("selectedtab", strTabCode, clsParam.pType.pString, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("order", fieldOrder, clsParam.pType.pLong, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("caption", strCaption, clsParam.pType.pString, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("tooltip", tooltip, clsParam.pType.pString, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("mandatory", (mandatory == 0 ? 0 : mandatory), clsParam.pType.pLong, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("fieldname", fieldname, clsParam.pType.pString, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("datatype", datatype, clsParam.pType.pString, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("size", size, clsParam.pType.pLong, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("decimal", isDecimal, clsParam.pType.pLong, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("SearchField", (isSearchField == 0 ? 0 : isSearchField), clsParam.pType.pLong, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("inputmask", inputMask, clsParam.pType.pString, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("helpquery", helpQuery, clsParam.pType.pString, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("remarks", remarks, clsParam.pType.pString, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("whencondition", whenCondition, clsParam.pType.pString, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("defaultvalue", defaultValue, clsParam.pType.pString, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("validation", validation, clsParam.pType.pString, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("internaluse", (internalUse == 0 ? 0 : internalUse), clsParam.pType.pLong, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("form_code", strFormCode, clsParam.pType.pString, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("tab_code", strTabCode, clsParam.pType.pString, colParams, false, null,
                                        clsParam.pInOut.pIn);

                field_Id = oDataAccess.ExecuteSQLStatement(strSQL, colParams, 20, false);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objParam = null;
                colParams = null;
                ////oDataAccess = null;
            }
            return field_Id;
        }

        public int UpdateFieldDetails(string strTabCode, int fieldOrder, string strCaption, string tooltip, int mandatory,
            string fieldname, string datatype, int size, int isDecimal, int isSearchField, string inputMask,
            string helpQuery, string remarks, string whenCondition, string defaultValue, string validation,
            int internalUse, string strFormCode, int intFieldId, DataAccess_Net.clsDataAccess oDataAccess)
        {
            //oDataAccess = new DataAccess_Net.clsDataAccess();
            List<clsParam> colParams;
            clsParam objParam;
            int field_Id = 0;

            string strSQL;

            try
            {
                strSQL = "UPDATE FIELD_MASTER SET FIELD_ORDER = ?, CAPTION = ?, ";
                strSQL += "TOOLTIP = ?, MANDATORY = ?, FIELDNAME = ?, DATATYPE = ?, SIZE = ?, ";
                strSQL += "[DECIMAL] = ?, [SearchField] = ?, INPUTMASK = ?, HELPQUERY = ?, REMARKS = ?, ";
                strSQL += "WHENCONDITION = ?, DEFAULTVALUE = ?, [VALIDATION] = ?, INTERNALUSE = ? ";
                strSQL += "WHERE ID = ? AND SELECTEDTAB = ? "; // and tabcontrol_id=?

                colParams = new List<clsParam>();

                objParam = new clsParam("order", fieldOrder, clsParam.pType.pLong, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("caption", strCaption, clsParam.pType.pString, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("tooltip", tooltip, clsParam.pType.pString, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("mandatory", (mandatory == 0 ? 0 : mandatory), clsParam.pType.pLong, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("fieldname", fieldname, clsParam.pType.pString, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("datatype", datatype, clsParam.pType.pString, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("size", size, clsParam.pType.pLong, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("decimal", isDecimal, clsParam.pType.pLong, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("SearchField", (isSearchField == 0 ? 0 : isSearchField), clsParam.pType.pLong, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("inputmask", inputMask, clsParam.pType.pString, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("helpquery", helpQuery, clsParam.pType.pString, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("remarks", remarks, clsParam.pType.pString, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("whencondition", whenCondition, clsParam.pType.pString, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("defaultvalue", defaultValue, clsParam.pType.pString, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("validation", validation, clsParam.pType.pString, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("internaluse", (internalUse == 0 ? 0 : internalUse), clsParam.pType.pLong, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("field_id", (intFieldId == 0 ? 0 : intFieldId), clsParam.pType.pLong, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("tab_code", strTabCode, clsParam.pType.pString, colParams, false, null,
                                        clsParam.pInOut.pIn);

                field_Id = oDataAccess.ExecuteSQLStatement(strSQL, colParams, 20, true);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objParam = null;
                colParams = null;
                ////oDataAccess = null;
            }
            return field_Id;
        }

        public int UpdateTabControlDetails(int intTabControlID, string strFormCode, string strCaption, int tabOrder,
                                           DataAccess_Net.clsDataAccess oDataAccess)
        {
            oDataAccess = new DataAccess_Net.clsDataAccess();
            List<clsParam> colParams;
            clsParam objParam;
            int tabControlId = 0;

            string strSQL;

            try
            {
                strSQL = "UPDATE [DBO].[TAB_CONTROLS] SET CAPTION = ?, TAB_ORDER = ? WHERE ID = ? AND FORM_CODE = ?";

                colParams = new List<clsParam>();
                objParam = new clsParam("caption", strCaption, clsParam.pType.pString, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("order", tabOrder, clsParam.pType.pLong, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("id", intTabControlID, clsParam.pType.pLong, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("form_code", strFormCode, clsParam.pType.pString, colParams, false, null,
                                        clsParam.pInOut.pIn);

                oDataAccess.ExecuteSQLStatement(strSQL, colParams, 20, true);
                tabControlId = 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objParam = null;
                colParams = null;
                ////oDataAccess = null;
            }
            return tabControlId;
        }

        public DataSet GetFormList(string strFormCode, DataAccess_Net.clsDataAccess oDataAccess)
        {
            List<clsParam> colParams;
            clsParam objParam;
            DataSet dsFormList = new DataSet();
            string strSQL;

            try
            {
                colParams = new List<clsParam>();
                strSQL = "SELECT ID, CAPTION, CODE, TABLE_NAME FROM FORM_MASTER";

                if (!string.IsNullOrEmpty(strFormCode))
                {
                    strSQL += " WHERE UPPER(CODE) = ?";
                    objParam = new clsParam("CODE", strFormCode.ToUpper(), clsParam.pType.pString, colParams,
                                            false, null, clsParam.pInOut.pIn);
                }

                dsFormList = oDataAccess.GetDataSet(strSQL, colParams, 20);
                dsFormList.Tables[0].TableName = "Forms";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objParam = null;
                colParams = null;
                //oDataAccess = null;
            }
            return dsFormList;
        }

        public DataSet GetEmptyFormList(DataAccess_Net.clsDataAccess oDataAccess)
        {
            List<clsParam> colParams;
            clsParam objParam;
            DataSet dsFormList = new DataSet();
            string strSQL;

            try
            {
                colParams = new List<clsParam>();
                strSQL = "SELECT ID, CAPTION, CODE, TABLE_NAME FROM FORM_MASTER WHERE CODE = ?";
                objParam = new clsParam("CODE", "", clsParam.pType.pString, colParams,
                                                            false, null, clsParam.pInOut.pIn);

                dsFormList = oDataAccess.GetDataSet(strSQL, colParams, 20);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objParam = null;
                colParams = null;
                //oDataAccess = null;
            }
            return dsFormList;
        }

        public DataSet GetFormList1(string strFormCode, string[] arFormCode, DataAccess_Net.clsDataAccess oDataAccess)
        {
            List<clsParam> colParams;
            DataSet dsFormList = new DataSet();
            string strSQL;

            try
            {
                colParams = new List<clsParam>();
                strSQL = "SELECT ID, CAPTION, CODE, TABLE_NAME FROM FORM_MASTER WHERE ";

                for (int i = 0; i < arFormCode.Length; i++)
                {
                    strSQL += arFormCode[i].ToString() + " LIKE '%" + strFormCode + "%' OR ";
                }
                strSQL = strSQL.Substring(0, strSQL.Length - 3);
                //WHERE CAPTION LIKE '%vij%' OR CODE LIKE '%vij%' OR TABLE_NAME LIKE '%vij%'
                //if (!string.IsNullOrEmpty(strFormCode))
                //{
                //    strSQL += " WHERE UPPER(CODE) = ?";
                //    objParam = new clsParam("CODE", strFormCode.ToUpper(), clsParam.pType.pString, colParams,
                //                            false, null, clsParam.pInOut.pIn);
                //}

                dsFormList = oDataAccess.GetDataSet(strSQL, colParams, 20);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //objParam = null;
                colParams = null;
                //oDataAccess = null;
            }
            return dsFormList;
        }

        public DataSet GetTabControlList(string strFormCode, DataAccess_Net.clsDataAccess oDataAccess)
        {
            List<clsParam> colParams;
            clsParam objParam;
            DataSet dsTabList = new DataSet();
            string strSQL;

            try
            {
                strSQL = "SELECT CAPTION, CODE, TAB_ORDER, FORM_CODE, ID FROM TAB_CONTROLS WHERE FORM_CODE = ? ORDER BY TAB_ORDER";
                //, ID AS TABCONTROL_ID
                colParams = new List<clsParam>();
                objParam = new clsParam("form_id", strFormCode, clsParam.pType.pString, colParams, false, null,
                                        clsParam.pInOut.pIn);

                dsTabList = oDataAccess.GetDataSet(strSQL, colParams, 20);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objParam = null;
                colParams = null;
                //oDataAccess = null;
            }
            return dsTabList;
        }

        public DataSet GetFieldList(string strTabCode, string formCode, DataAccess_Net.clsDataAccess oDataAccess)
        {
            oDataAccess = new DataAccess_Net.clsDataAccess();
            List<clsParam> colParams;
            clsParam objParam;
            DataSet dsFieldList = new DataSet();
            string strSQL;

            try
            {
                //strSQL = "SELECT ID, '' AS SELECTEDTAB, FIELD_ORDER, CAPTION, TOOLTIP, MANDATORY,";
                //strSQL += " FIELDNAME, DATATYPE, SIZE, [DECIMAL], [SearchField],	INPUTMASK,";
                //strSQL += " HELPQUERY, REMARKS, WHENCONDITION, DEFAULTVALUE, [VALIDATION],";
                //strSQL += " INTERNALUSE, FORM_ID, TABCONTROL_ID FROM FIELD_MASTER WHERE TABCONTROL_ID = ?";

                strSQL = "SELECT F.ID, T.CODE AS SELECTEDTAB, F.FIELD_ORDER, F.CAPTION, F.TOOLTIP, F.MANDATORY, ";
                strSQL += "F.FIELDNAME, F.DATATYPE, F.SIZE, F.[DECIMAL], F.[SEARCHFIELD], F.INPUTMASK, F.HELPQUERY, ";
                strSQL += "F.REMARKS, F.WHENCONDITION, F.DEFAULTVALUE, F.[VALIDATION], ";
                strSQL += "F.INTERNALUSE, F.FORM_CODE, F.TAB_CODE,F.Val_Error,F.LineCount,F.MainField ";
                strSQL += "FROM TAB_CONTROLS T ";
                strSQL += "INNER JOIN FIELD_MASTER F ON (T.FORM_CODE = F.FORM_CODE AND T.CODE = F.TAB_CODE) ";
                strSQL += "WHERE f.TAB_CODE = ? and f.form_code = ? ORDER BY F.FIELD_ORDER";

                colParams = new List<clsParam>();
                objParam = new clsParam("tab_id", strTabCode, clsParam.pType.pString, colParams, false, null,
                                        clsParam.pInOut.pIn);
                objParam = new clsParam("form_id", formCode, clsParam.pType.pString, colParams, false, null,
                                        clsParam.pInOut.pIn);

                dsFieldList = oDataAccess.GetDataSet(strSQL, colParams, 20);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objParam = null;
                colParams = null;
                //oDataAccess = null;
            }
            return dsFieldList;
        }

        public DataSet GetAllFields(string strFormCode, DataAccess_Net.clsDataAccess oDataAccess)
        {
            //oDataAccess = new DataAccess_Net.clsDataAccess();
            List<clsParam> colParams;
            clsParam objParam;
            DataSet dsFieldList = new DataSet();
            string strSQL;

            try
            {
                //strSQL = "SELECT ID, '' AS SELECTEDTAB, FIELD_ORDER, CAPTION, TOOLTIP, MANDATORY,";
                //strSQL += " FIELDNAME, DATATYPE, SIZE, [DECIMAL], [SearchField],	INPUTMASK,";
                //strSQL += " HELPQUERY, REMARKS, WHENCONDITION, DEFAULTVALUE, [VALIDATION],";
                //strSQL += " INTERNALUSE, FORM_ID, TABCONTROL_ID FROM FIELD_MASTER WHERE TABCONTROL_ID = ?";

                strSQL = "SELECT F.ID, T.CODE AS SELECTEDTAB, F.FIELD_ORDER, F.CAPTION, F.TOOLTIP, F.MANDATORY, ";
                strSQL += "F.FIELDNAME, F.DATATYPE, F.SIZE, F.[DECIMAL], F.[SearchField], F.INPUTMASK, F.HELPQUERY, ";
                strSQL += "F.REMARKS, F.WHENCONDITION, F.DEFAULTVALUE, F.[VALIDATION], ";
                strSQL += "F.INTERNALUSE, F.FORM_CODE, F.TAB_CODE AS TAB_CODE, 'Unchanged' AS STATE ";
                strSQL += "FROM TAB_CONTROLS T ";
                strSQL += "INNER JOIN FIELD_MASTER F ON t.form_code = f.form_code and T.CODE = F.TAB_CODE ";
                strSQL += "WHERE F.FORM_CODE = ? ";

                colParams = new List<clsParam>();
                objParam = new clsParam("tab_code", strFormCode, clsParam.pType.pString, colParams, false, null,
                                        clsParam.pInOut.pIn);

                dsFieldList = oDataAccess.GetDataSet(strSQL, colParams, 0);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objParam = null;
                colParams = null;
                //oDataAccess = null;
            }
            return dsFieldList;
        }

        //public DataSet GetFormInfo(string strFormCode, DataAccess_Net.clsDataAccess oDataAccess)
        //{
        //    oDataAccess = new DataAccess_Net.clsDataAccess();
        //    List<clsParam> colParams;
        //    clsParam objParam;
        //    DataSet dsFormList = new DataSet();
        //    int formId = 0;
        //    int tabId = 0;
        //    string strSQL;

        //    try
        //    {
        //        strSQL = "SELECT ID, CAPTION, CODE, TABLE_NAME FROM FORM_MASTER WHERE CODE = ?";

        //        colParams = new List<clsParam>();
        //        objParam = new clsParam("CODE", strFormCode, clsParam.pType.pString, colParams, false, null,
        //                                clsParam.pInOut.pIn);

        //        dsFormList = oDataAccess.GetDataSet(strSQL, colParams, 20);

        //        if (dsFormList.Tables.Count > 0)
        //        {
        //            dsFormList.Tables[0].TableName = "Forms";
        //            if (dsFormList.Tables[0].Rows.Count > 0)
        //            {
        //                formId = Convert.ToInt32(dsFormList.Tables[0].Rows[0]["id"]);
        //            }
        //        }
        //        if (formId != 0)
        //        {
        //            DataSet dsTabList = new DataSet();
        //            dsTabList = GetTabControlList(formId, oDataAccess);
        //            if (dsTabList.Tables.Count > 0)
        //            {
        //                dsTabList.Tables[0].TableName = "Tabs";
        //                if (dsTabList.Tables[0].Rows.Count > 0)
        //                {
        //                    tabId = Convert.ToInt32(dsTabList.Tables["Tabs"].Rows[0]["id"]);
        //                    DataTable dtTabs = new DataTable();
        //                    dtTabs = dsTabList.Tables["Tabs"].Copy();
        //                    dsFormList.Tables.Add(dtTabs);
        //                }
        //            }
        //        }
        //        if (tabId != 0)
        //        {
        //            DataSet dsFieldList = new DataSet();
        //            dsFieldList = GetFieldList(tabId, oDataAccess);
        //            if (dsFieldList.Tables.Count > 0)
        //            {
        //                dsFieldList.Tables[0].TableName = "Fields";
        //                DataTable dtFields = new DataTable();
        //                dtFields = dsFieldList.Tables["Fields"].Copy();
        //                dsFormList.Tables.Add(dtFields);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        objParam = null;
        //        colParams = null;
        //        //oDataAccess = null;
        //    }
        //    return dsFormList;
        //}

        public int DeleteSelectedTab(int tabID, DataAccess_Net.clsDataAccess oDataAccess)
        {
            List<clsParam> colParams;
            clsParam objParam;
            int Id = 0;

            string strSQL;

            try
            {
                strSQL = "DELETE FROM TAB_CONTROLS WHERE ID = ?;";

                colParams = new List<clsParam>();
                objParam = new clsParam("ID", tabID, clsParam.pType.pLong, colParams, false, null,
                                        clsParam.pInOut.pIn);

                Id = oDataAccess.ExecuteSQLStatement(strSQL, colParams, 20, true);

            }
            catch (Exception ex)
            {
                Id = 0;
                throw ex;
            }
            finally
            {
                objParam = null;
                colParams = null;
                ////oDataAccess = null;
            }
            return Id;
        }

        public int DeleteFields(int tabID, DataAccess_Net.clsDataAccess oDataAccess)
        {
            List<clsParam> colParams;
            clsParam objParam;
            int Id = 0;

            string strSQL;

            try
            {
                strSQL = "DELETE FROM FIELD_MASTER WHERE TABCONTROL_ID = ?;";

                colParams = new List<clsParam>();
                objParam = new clsParam("TAB_ID", tabID, clsParam.pType.pLong, colParams, false, null,
                                        clsParam.pInOut.pIn);

                Id = oDataAccess.ExecuteSQLStatement(strSQL, colParams, 20, true);

            }
            catch (Exception ex)
            {
                Id = 0;
                throw ex;
            }
            finally
            {
                objParam = null;
                colParams = null;
                ////oDataAccess = null;
            }
            return Id;
        }

        public int DeleteSelectedField(int fieldID, DataAccess_Net.clsDataAccess oDataAccess)
        {
            List<clsParam> colParams;
            clsParam objParam;
            int Id = 0;

            string strSQL;

            try
            {
                strSQL = "DELETE FROM FIELD_MASTER WHERE ID = ?;";

                colParams = new List<clsParam>();
                objParam = new clsParam("TAB_ID", fieldID, clsParam.pType.pLong, colParams, false, null,
                                        clsParam.pInOut.pIn);

                Id = oDataAccess.ExecuteSQLStatement(strSQL, colParams, 20, true);

            }
            catch (Exception ex)
            {
                Id = 0;
                throw ex;
            }
            finally
            {
                objParam = null;
                colParams = null;
                ////oDataAccess = null;
            }
            return Id;
        }

        public int DeleteSelectedEntry(int rowId, string tableName, DataAccess_Net.clsDataAccess oDataAccess)
        {
            List<clsParam> colParams;
            clsParam objParam;
            int Id = 0;

            string strSQL;

            try
            {
                strSQL = "DELETE FROM " + tableName + " WHERE ID = ?;";

                colParams = new List<clsParam>();
                objParam = new clsParam("ROW_ID", rowId, clsParam.pType.pLong, colParams, false, null,
                                        clsParam.pInOut.pIn);

                Id = oDataAccess.ExecuteSQLStatement(strSQL, colParams, 20, true);

            }
            catch (Exception ex)
            {
                Id = 0;
                throw ex;
            }
            finally
            {
                objParam = null;
                colParams = null;
                ////oDataAccess = null;
            }
            return Id;
        }

        public DataSet GetDataFromSelectedTable(string tableName, DataAccess_Net.clsDataAccess oDataAccess)
        {
            DataSet dsData = new DataSet();
            string strSQL;

            try
            {
                strSQL = "SELECT * FROM " + tableName;

                dsData = oDataAccess.GetDataSet(strSQL, null, 20);
                dsData.Tables[0].TableName = "FormInfo";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //objParam = null;
                //colParams = null;
                //oDataAccess = null;
            }
            return dsData;
        }

        public int SaveDynamicFormData(string tableName, List<clsParam> colParams,
            DataAccess_Net.clsDataAccess oDataAccess)
        {
            int id = 0;

            string strSQL;
            StringBuilder strFieldNames = new StringBuilder();
            StringBuilder strFieldValues = new StringBuilder();

            try
            {
                strSQL = "INSERT INTO [DBO]." + tableName + "(";

                foreach (clsParam oParam in colParams)
                {
                    strFieldNames.Append(oParam.Name + ",");
                    strFieldValues.Append("?,");
                }
                strFieldNames = strFieldNames.Remove(strFieldNames.Length - 1, 1);
                strFieldValues = strFieldValues.Remove(strFieldValues.Length - 1, 1);
                strFieldNames.Append(") VALUES(" + strFieldValues + ")");

                strSQL += strFieldNames.ToString();

                id = oDataAccess.ExecuteSQLStatement(strSQL, colParams, 20, true);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                colParams = null;
                ////oDataAccess = null;
            }
            return id;
        }

        public int UpdateDynamicFormData(string tableName, List<clsParam> colParams,
            DataAccess_Net.clsDataAccess oDataAccess, int FieldId)
        {
            int id = 0;
            clsParam objParam;
            string strSQL;
            StringBuilder strFieldNames = new StringBuilder();
            StringBuilder strFieldValues = new StringBuilder();

            try
            {
                strSQL = "UPDATE [DBO]." + tableName + " SET ";

                foreach (clsParam oParam in colParams)
                {
                    strFieldNames.Append(oParam.Name + " =?,");
                    strFieldValues.Append("?,");
                }
                strFieldNames = strFieldNames.Remove(strFieldNames.Length - 1, 1);
                strFieldValues = strFieldValues.Remove(strFieldValues.Length - 1, 1);
                strFieldNames.Append(" WHERE ID = ? ");

                strSQL += strFieldNames.ToString();
                objParam = new clsParam("id", FieldId, clsParam.pType.pLong, colParams,
                                        false, null, clsParam.pInOut.pIn);
                id = oDataAccess.ExecuteSQLStatement(strSQL, colParams, 20, true);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                colParams = null;
                ////oDataAccess = null;
            }
            return id;
        }

        public string GetColorCode(int companyID, DataAccess_Net.clsDataAccess oDataAccess)
        {
            DataSet dsData = new DataSet();
            string strSQL;
            string colorCode = string.Empty;
            clsParam objParam;
            List<clsParam> colParams = new List<clsParam>();

            try
            {
                strSQL = "select vcolor from Vudyog..co_mast where compid = ?";

                objParam = new clsParam("id", companyID, clsParam.pType.pLong, colParams,
                                        false, null, clsParam.pInOut.pIn);

                dsData = oDataAccess.GetDataSet(strSQL, colParams, 20);

                if (dsData != null)
                {
                    if (dsData.Tables.Count > 0)
                    {
                        dsData.Tables[0].TableName = "ColorInfo";
                        colorCode = dsData.Tables["ColorInfo"].Rows[0]["vcolor"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objParam = null;
                colParams = null;
                //oDataAccess = null;
            }
            return colorCode;
        }

        public DataSet GetUserRightsForMenu(int range, string userName, DataAccess_Net.clsDataAccess oDataAccess)
        {
            List<clsParam> colParams;
            clsParam objParam;
            DataSet dsMenu = new DataSet();
            DataSet dsRights = new DataSet();
            string strSQL;

            try
            {
                colParams = new List<clsParam>();
                strSQL = "select padname,barname,range from com_menu where range = ?";

                objParam = new clsParam("range", range, clsParam.pType.pLong, colParams,
                                        false, null, clsParam.pInOut.pIn);

                dsMenu = oDataAccess.GetDataSet(strSQL, colParams, 20);
                dsMenu.Tables[0].TableName = "UserRights";

                if (dsMenu != null)
                {
                    if (dsMenu.Tables[0].Rows.Count > 0)
                    {
                        string padName = "";
                        string barName = "";
                        padName = dsMenu.Tables[0].Rows[0]["padname"].ToString();
                        barName = dsMenu.Tables[0].Rows[0]["barname"].ToString();

                        strSQL = "select padname,barname,dbo.func_decoder(rights,'F') as rights from ";
                        strSQL += "userrights where padname = ? and barname = ? and range = ? ";
                        strSQL += "and dbo.func_decoder([user],'T') = ?";

                        colParams = new List<clsParam>();
                        objParam = new clsParam("padname", padName, clsParam.pType.pString, colParams,
                                        false, null, clsParam.pInOut.pIn);
                        objParam = new clsParam("barname", barName, clsParam.pType.pString, colParams,
                                        false, null, clsParam.pInOut.pIn);
                        objParam = new clsParam("range", range, clsParam.pType.pLong, colParams,
                                        false, null, clsParam.pInOut.pIn);
                        objParam = new clsParam("user", userName, clsParam.pType.pString, colParams,
                                        false, null, clsParam.pInOut.pIn);

                        dsRights = oDataAccess.GetDataSet(strSQL, colParams, 20);
                        //if (dsRights != null)
                        //{ }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objParam = null;
                colParams = null;
                //oDataAccess = null;
            }
            return dsRights;
        }

        public bool CheckTableExists(string tableName, DataAccess_Net.clsDataAccess oDataAccess)
        {
            List<clsParam> colParams;
            clsParam objParam;
            DataRow dr;
            string strSQL;
            bool bExists = false;
            try
            {
                colParams = new List<clsParam>();
                strSQL = "select * from sys.tables where type = 'u' and name = ?";

                objParam = new clsParam("tabelname", tableName, clsParam.pType.pString, 
                                        colParams, false, null, clsParam.pInOut.pIn);

                dr = oDataAccess.GetDataRow(strSQL, colParams, 20);
                if (dr != null)
                    bExists = true;
                else
                    bExists = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objParam = null;
                colParams = null;
                //oDataAccess = null;
            }
            return bExists;
        }

    }
}
