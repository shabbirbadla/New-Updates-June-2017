// ===============================================
// | Downloaded From                             |
// | Visual C# Kicks - http://www.vcskicks.com/  |
// ===============================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace AlignUI
{
    public class UI
    {
        public class Align
        {
            public static void Lefts(Control control1, Control control2)
            {
                control2.Left = control1.Left;
            }

            public static void Centers(Control control1, Control control2)
            {
                int center = control1.Left + (control1.Width / 2);
                control2.Left = center - (control2.Width / 2);
            }

            public static void Rights(Control control1, Control control2)
            {
                int right = control1.Left + control1.Width;
                control2.Left = right - control2.Width;
            }

            public static void Tops(Control control1, Control control2)
            {
                control2.Top = control1.Top;
            }

            public static void Middles(Control control1, Control control2)
            {
                int middle = control1.Top + (control1.Height / 2);
                control2.Top = middle - (control2.Height / 2);
            }

            public static void Bottoms(Control control1, Control control2)
            {
                int bottom = control1.Top + control1.Height;
                control2.Top = bottom - control2.Height;
            }
        }

        public class MakeSameSize
        {
            public static void Width(Control control1, Control control2)
            {
                control2.Width = control1.Width;
            }

            public static void Height(Control control1, Control control2)
            {
                control2.Height = control1.Height;
            }

            public static void Both(Control control1, Control control2)
            {
                control2.Width = control1.Width;
                control2.Height = control1.Height;
            }
        }

        public class CenterInForm
        {
            public static void Horizontally(Form parentForm, Control control)
            {
                Rectangle surfaceRect = parentForm.ClientRectangle;

                control.Left = (surfaceRect.Width / 2) - (control.Width / 2);
            }

            public static void Vertically(Form parentForm, Control control)
            {
                Rectangle surfaceRect = parentForm.ClientRectangle;

                control.Top = (surfaceRect.Height / 2) - (control.Height / 2);
            }
        }
    }
}
