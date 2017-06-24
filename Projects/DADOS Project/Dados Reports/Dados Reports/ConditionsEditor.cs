using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Xml;
using System.IO;

namespace DadosReports
{
    public partial class ConditionsEditor : DevExpress.XtraEditors.XtraForm
    {
        #region Veriables

        int ConditionsIndex = 0;

        #endregion

        #region Properties

        private String strConditionFieldText;

        public String ConditionFieldText
        {
            get { return strConditionFieldText; }
            set { strConditionFieldText = value; }
        }
        private string[] strControlValues;//= new string[12];
        public string[] ControlValues
        {
            get { return strControlValues; }
            set { strControlValues = value; }
        }

        private string[] strListControlTextValues;//= new string[12];
        public string[] ListControlTextValues
        {
            get { return strListControlTextValues; }
            set { strListControlTextValues = value; }
        }

        private string[] strFieldControlValues;//= new string[12];
        public string[] FieldControlValues
        {
            get { return strFieldControlValues; }
            set { strFieldControlValues = value; }
        }

        private XmlDocument varConditionsXMLDocument = new XmlDocument();
        public XmlDocument ConditionsXMLDocument
        {
            get { return varConditionsXMLDocument; }
            set { varConditionsXMLDocument = value; }
        }

        private string[] strCondition;
        public string[] Condition
        {
            get { return strCondition; }
            set { strCondition = value; }
        }
        private string[] strConditionValue1;
        public string[] ConditionValue1
        {
            get { return strConditionValue1; }
            set { strConditionValue1 = value; }
        }
        private string[] strConditionValue2;
        public string[] ConditionValue2
        {
            get { return strConditionValue2; }
            set { strConditionValue2 = value; }
        }
        private string[] strApplyToRow;
        public string[] ApplyToRow
        {
            get { return strApplyToRow; }
            set { strApplyToRow = value; }
        }
        private string[] strConditionBackColor;
        public string[] ConditionBackColor
        {
            get { return strConditionBackColor; }
            set { strConditionBackColor = value; }
        }
        private string[] strConditionForeColor;
        public string[] ConditionForeColor
        {
            get { return strConditionForeColor; }
            set { strConditionForeColor = value; }
        }

        #endregion

        #region constructor
        /// <summary>
        /// Required designer variable.
        /// </summary>
        public ConditionsEditor()
        {
            // Required for Windows Form Designer support
            InitializeComponent();

            // TODO: Add any constructor code after InitializeComponent call
        }
        #endregion

        #region Button Exit Functionality
        private void simpleButton_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Create New Style Condition Functionality
        private void btnNewStyleCondition_Click(object sender, EventArgs e)
        {
            lbcConditionName.Items.Add("Condition Item - Index" + ConditionsIndex);
            ConditionsIndex = ConditionsIndex + 1;
            btnRemove.Enabled = true;
            cbeCondition.Enabled = true;
        }
        #endregion

        #region Based on the Selected Condition  To Display Controls in the Form
        private void cbeCondition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbcConditionName.SelectedItem != null || lbcConditionName.SelectedItem.ToString() != "")
            {
                if (cbeCondition.SelectedItem.ToString() == "None")
                {
                    spinEdit_Value1.Enabled = false;
                    spinEdit_Value2.Enabled = false;

                    colorEdit_BackColor.Enabled = false;
                    colorEdit_ForeColor.Enabled = false;

                    simpleButton_Apply.Enabled = false;
                    //Condition[lbcConditionName.SelectedIndex] = cbeCondition.SelectedItem.ToString();
                }
                else
                {
                    string SelectedCondition = cbeCondition.SelectedItem.ToString();
                    switch (SelectedCondition)
                    {
                        case "Equal":
                            spinEdit_Value1.Enabled = true;
                            spinEdit_Value2.Enabled = false;

                            colorEdit_BackColor.Enabled = true;
                            colorEdit_ForeColor.Enabled = true;

                            simpleButton_Apply.Enabled = true;
                            //Condition[lbcConditionName.SelectedIndex] = cbeCondition.SelectedItem.ToString();
                            break;
                        case "NotEqual":
                            spinEdit_Value1.Enabled = true;
                            spinEdit_Value2.Enabled = false;

                            colorEdit_BackColor.Enabled = true;
                            colorEdit_ForeColor.Enabled = true;

                            simpleButton_Apply.Enabled = true;
                            //Condition[lbcConditionName.SelectedIndex] = cbeCondition.SelectedItem.ToString();
                            break;
                        case "Between":
                            spinEdit_Value1.Enabled = true;
                            spinEdit_Value2.Enabled = true;

                            colorEdit_BackColor.Enabled = true;
                            colorEdit_ForeColor.Enabled = true;

                            simpleButton_Apply.Enabled = true;
                            //Condition[lbcConditionName.SelectedIndex] = cbeCondition.SelectedItem.ToString();
                            break;
                        case "NotBetween":
                            spinEdit_Value1.Enabled = true;
                            spinEdit_Value2.Enabled = true;

                            colorEdit_BackColor.Enabled = true;
                            colorEdit_ForeColor.Enabled = true;

                            simpleButton_Apply.Enabled = true;
                            //Condition[lbcConditionName.SelectedIndex] = cbeCondition.SelectedItem.ToString();
                            break;
                        case "Less":
                            spinEdit_Value1.Enabled = true;
                            spinEdit_Value2.Enabled = false;

                            colorEdit_BackColor.Enabled = true;
                            colorEdit_ForeColor.Enabled = true;

                            simpleButton_Apply.Enabled = true;
                            //Condition[lbcConditionName.SelectedIndex] = cbeCondition.SelectedItem.ToString();
                            break;
                        case "Greater":
                            spinEdit_Value1.Enabled = true;
                            spinEdit_Value2.Enabled = false;

                            colorEdit_BackColor.Enabled = true;
                            colorEdit_ForeColor.Enabled = true;

                            simpleButton_Apply.Enabled = true;
                            //Condition[lbcConditionName.SelectedIndex] = cbeCondition.SelectedItem.ToString();
                            break;
                        case "GreaterOrEqual":
                            spinEdit_Value1.Enabled = true;
                            spinEdit_Value2.Enabled = false;

                            colorEdit_BackColor.Enabled = true;
                            colorEdit_ForeColor.Enabled = true;

                            simpleButton_Apply.Enabled = true;
                            //Condition[lbcConditionName.SelectedIndex] = cbeCondition.SelectedItem.ToString();
                            break;
                        case "LessOrEqual":
                            spinEdit_Value1.Enabled = true;
                            spinEdit_Value2.Enabled = false;

                            colorEdit_BackColor.Enabled = true;
                            colorEdit_ForeColor.Enabled = true;

                            simpleButton_Apply.Enabled = true;
                            //Condition[lbcConditionName.SelectedIndex] = cbeCondition.SelectedItem.ToString();
                            break;
                        default:
                            break;

                    }
                }
            }
            else
            {
                MessageBox.Show(this, "please Select The Condition to Asign Value !!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }

        }
        #endregion

        #region The Form Loading Functionality
        private void ConditionsEditor_Load(object sender, EventArgs e)
        {

            groupControl1.Text = "Conditions Editor for   [" + ConditionFieldText + "]   Field";



            //colorEdit_BackColor.Color = Color.FromArgb(255, 225, 0);

            //XmlDocument xmldocument = new XmlDocument();
            //XmlTextReader xmltextreader = new XmlTextReader(Environment.CurrentDirectory + "/Conditions Editor/ConditionsEditor.xml");
            //xmltextreader.Read();

            //while (xmltextreader.Read())
            //{
            //    xmltextreader.MoveToElement();

            //    if (xmltextreader.GetAttribute("name") == groupControl1.Name)
            //    {
            //        groupControl1.Text = xmltextreader.ReadElementContentAsString();
            //    }
            //    else if (xmltextreader.GetAttribute("name") == lbcConditionName.Name)
            //    {
            //        //xmltextreader.MoveToElement();
            //        int depth = xmltextreader.Depth;

            //        XmlReader newreader = xmltextreader.ReadSubtree();

            //        while (newreader.Read())
            //        {
            //            newreader.MoveToElement();
            //            if (newreader.Name == "items")
            //            {
            //                lbcConditionName.Items.Add(newreader.GetAttribute("text"));
            //            }
            //            else if (newreader.GetAttribute("name") == cbeCondition.Name)
            //            {
            //                cbeCondition.SelectedText = newreader.GetAttribute("text");
            //            }
            //        }



            //    }
            //}
        }
        #endregion

        #region Deleting the Selected Condition the List Control (Remove Condtion Button) Functionality
        private void btnRemove_Click(object sender, EventArgs e)
        {

            if (lbcConditionName.SelectedItem != null)
            {
                lbcConditionName.Items.RemoveAt(lbcConditionName.SelectedIndex);
                if (lbcConditionName.Items.Count == 0)
                {
                    cbeCondition.Enabled = false;
                    btnRemove.Enabled = false;
                    spinEdit_Value1.Enabled = false;
                    spinEdit_Value2.Enabled = false;

                    colorEdit_BackColor.Enabled = false;
                    colorEdit_ForeColor.Enabled = false;

                    simpleButton_Apply.Enabled = false;
                }
            }
            else
            {
                MessageBox.Show(this, "Please Select The Condition witch you need to Remove", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
        }
        #endregion

        #region To Apply Condition to Grid (Apply Button)Functionality
        private void simpleButton_Apply_Click(object sender, EventArgs e)
        {
            //if (lbcConditionName.Items.Count > 0)
            //{
            //    int i = 0;
            //    strListControlTextValues = new string[lbcConditionName.Items.Count];

            //    foreach (string strListControlItems in lbcConditionName.Items)
            //    {
            //        strListControlTextValues[i] = strListControlItems;
            //        i++;
            //    }
            //}
            //else
            //{
            //    MessageBox.Show(this, "Please set the Condition First !!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            //}


            //XmlTextWriter xmltextwriter = new XmlTextWriter(Environment.CurrentDirectory + "/Conditions Editor/ConditionsEditor.xml", null);

        }
        #endregion





        private void lbcConditionName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string strlistinedexText = lbcConditionName.SelectedValue.ToString();

            //if (strlistinedexText != "" && strlistinedexText != null)
            //{
            //    cbeCondition.SelectedItem = Condition[lbcConditionName.SelectedIndex];
            //}

        }
    }
}