using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace DadosReports
{
    public partial class Loading : Form
    {
        private static Loading loading;
        private static Thread _splashLauncher;

        public Loading()
        {
            InitializeComponent();
        }


        static public void ShowSplashScreen()
        {
            //Show the form in a new thread
            _splashLauncher = new Thread(new ThreadStart(ShowForm));
            _splashLauncher.IsBackground = true;
            _splashLauncher.Start();

        }
        static private void ShowForm()
        {
            loading = new Loading();
            Application.Run(loading);
        }

        private static void CloseSplashDown()
        {
            Application.ExitThread();
            loading.Hide();
        }

        public static void CloseSplash()
        {
            //Need to get the thread that launched the form, so
            //we need to use invoke.
            MethodInvoker mi = new MethodInvoker(CloseSplashDown);
            loading.Invoke(mi);
        }

        private void Loading_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "Escape")
                this.Hide();
        }


    }
}