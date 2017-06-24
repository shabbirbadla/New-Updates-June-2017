using System;
using System.Windows.Forms;
using System.ComponentModel;

namespace PointofSale
{
    public class clsScreenPropEvents
    {
        #region Screen Height and Width Properties
        private static decimal _OrgHgth = 0;
        public static decimal OrgHgth
        {
            set { _OrgHgth = value; }
        }

        private static decimal _OrgWdth = 0;
        public static decimal OrgWdth
        {
            set { _OrgWdth = value; }
        }

        private static decimal _NewHgth = 0;
        public static decimal NewHgth
        {
            set { _NewHgth = value; }
        }

        private static decimal _NewWdth = 0;
        public static decimal NewWdth
        {
            set { _NewWdth = value; }
        }

        private static decimal screenHghtRatio
        {
            get { return _NewHgth / _OrgHgth; }
        }

        private static decimal screenWdthRatio
        {
            get { return _NewWdth / _OrgWdth; }
        }

        public static void storeOrgHgthWdth(Control _control)
        {
            //TypeDescriptor.AddAttributes(_control, new Attribute[] { _OrgHgth, _OrgWdth });

        }
        #endregion

        #region Screen Resizing
        public static void ResizeForm(Control _control)
        {
            foreach (Control _cntrl in _control.Controls)
            {
                if (_cntrl.GetType().ToString() != "Label")
                {
                    _cntrl.Height = (int)((_NewHgth * _cntrl.Height) / _OrgHgth);
                    _cntrl.Width = (int)((_NewWdth * _cntrl.Width) / _OrgWdth);
                    _cntrl.Top = (int)((_NewHgth * _cntrl.Top) / _OrgHgth);
                    _cntrl.Left = (int)((_NewWdth * _cntrl.Left) / _OrgWdth);
                }

                if (_cntrl.HasChildren == true)
                {
                    ResizeForm(_cntrl);
                }
            }
        }
        #endregion
    }
}
