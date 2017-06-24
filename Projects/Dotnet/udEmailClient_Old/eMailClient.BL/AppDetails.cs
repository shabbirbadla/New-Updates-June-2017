using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eMailClient.BLL
{
    public class AppDetails
    {
        private static string _apptitle;
        public static string Apptitle
        {
            get { return _apptitle; }
            set { _apptitle = value; }
        }

        private static string _appfile;
        public static string Appfile
        {
            get { return _appfile; }
            set { _appfile = value; }
        }

        private static string _server;
        public static string Server
        {
            get { return _server; }
            set { _server = value; }
        }

        private static string _user;
        public static string User
        {
            get { return _user; }
            set { _user = value; }
        }

        private static string _pass;
        public static string Pass
        {
            get { return _pass; }
            set { _pass = value; }
        }

        private static string _icoPath;
        public static string IcoPath
        {
            get { return _icoPath; }
            set { _icoPath = value; }
        }

        private static string _appPath;
        public static string AppPath
        {
            get { return _appPath; }
            set { _appPath = value; }
        }
    }
}
