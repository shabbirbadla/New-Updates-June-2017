using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using System.Windows.Forms;
using uButton;
using System.Reflection;
using uBaseForm;
using System.Drawing;
using uParameterSelection;
namespace ueDynamicMasterProcedures
{
    public class cDynamicMasterProcedures 
    {
        string vParaList;

        public void mthBtnClick(Form pForm, object sender, EventArgs e,DataSet ds)
        {
            uBaseForm.FrmBaseForm oParentForm = new uBaseForm.FrmBaseForm();
            oParentForm = (uBaseForm.FrmBaseForm)pForm ;
            string cntName = string.Empty;
            cntName = ((uButton.cButton)sender).Name.ToLower();
            string appPath=string.Empty , appName=string.Empty;
            if (cntName == "cbtnbtnpara")
            {
                appPath = oParentForm.pAppPath+@"\";
                appName = "uParameterSelection.exe";
                appPath = @appPath.Trim() + appName.Trim();
                Assembly ass = Assembly.LoadFrom(appPath);
                Form extform = new Form();
                appName = appName.Substring(0, appName.IndexOf("."));

                extform = (Form)ass.CreateInstance(appName.Trim() + ".FrmMainParameterSelection", true);
                Type t = extform.GetType();
                t.GetProperty("pAddMode").SetValue(extform, oParentForm.pAddMode, null);
                t.GetProperty("pEditMode").SetValue(extform, oParentForm.pEditMode, null);
                t.GetProperty("pAppPath").SetValue(extform, oParentForm.pAppPath, null);
                t.GetProperty("pParentForm").SetValue(extform, pForm, null);
                t.GetProperty("pPara").SetValue(extform, oParentForm.pPara, null);
                extform.Show();
            }
            if (cntName == "cbtnbtnuserroles")
            {
                appPath = oParentForm.pAppPath + @"\";
                appName = "udUserRolesSelection.exe";
                appPath = @appPath.Trim() + appName.Trim();
                Assembly ass = Assembly.LoadFrom(appPath);
                Form extform = new Form();
                appName = appName.Substring(0, appName.IndexOf("."));
                extform = (Form)ass.CreateInstance(appName.Trim() + ".frmMainUserRoles", true);
                Type t = extform.GetType();
                t.GetProperty("pAddMode").SetValue(extform, oParentForm.pAddMode, null);
                t.GetProperty("pEditMode").SetValue(extform, oParentForm.pEditMode, null);
                t.GetProperty("pAppPath").SetValue(extform, oParentForm.pAppPath, null);
                t.GetProperty("pPara").SetValue(extform, oParentForm.pPara, null);
                t.GetProperty("pParentForm").SetValue(extform, pForm, null);
                extform.Show();
            }
            if (cntName == "cbtnbtntblfld")
            {
                appPath = oParentForm.pAppPath + @"\";
                appName = "ueFieldUpdate.exe";
                appPath = @appPath.Trim() + appName.Trim();
                Assembly ass = Assembly.LoadFrom(appPath);
                Form extform = new Form();
                appName = appName.Substring(0, appName.IndexOf("."));
                extform = (Form)ass.CreateInstance(appName.Trim() + ".frmTable", true);
                Type t = extform.GetType();
                t.GetProperty("pAddMode").SetValue(extform, oParentForm.pAddMode, null);
                t.GetProperty("pEditMode").SetValue(extform, oParentForm.pEditMode, null);
                t.GetProperty("pAppPath").SetValue(extform, oParentForm.pAppPath, null);
                t.GetProperty("pPara").SetValue(extform, oParentForm.pPara, null);
                t.GetProperty("pParentForm").SetValue(extform, pForm, null);
                extform.Show();
            }
            //Birendra : Bug-22262 on 07/04/2014:Start:
            if (cntName == "cbtnexpbtnloc" || cntName == "cbtnimpbtnloc")
            {
                appPath = oParentForm.pAppPath + @"\";
                appName = "udImpExpLocationSelection.exe";
                appPath = @appPath.Trim() + appName.Trim();
                Assembly ass = Assembly.LoadFrom(appPath);
                Form extform = new Form();
                appName = appName.Substring(0, appName.IndexOf("."));
                extform = (Form)ass.CreateInstance(appName.Trim() + ".frmIELocSelection", true);
                Type t = extform.GetType();
                t.GetProperty("pAddMode").SetValue(extform, oParentForm.pAddMode, null);
                t.GetProperty("pEditMode").SetValue(extform, oParentForm.pEditMode, null);
                t.GetProperty("pAppPath").SetValue(extform, oParentForm.pAppPath, null);
                t.GetProperty("pPara").SetValue(extform, oParentForm.pPara, null);
                t.GetProperty("pParentForm").SetValue(extform, pForm, null);
                extform.Show();
            }
            //Birendra : Bug-22262 on 07/04/2014:End:

            
        }
        public string pParaList
        {
            get { return vParaList; }
            set { vParaList = value; }
        }
    }

 }
