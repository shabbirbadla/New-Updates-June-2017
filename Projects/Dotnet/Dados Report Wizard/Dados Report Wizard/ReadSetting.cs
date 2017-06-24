using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dados_Report_Wizard
{
    class ReadSetting
    {
        public ReadSetting(string appPath,string DBName)
        {
            GetInfo.iniFile ini = new GetInfo.iniFile(appPath + "\\" + "Visudyog.ini");
            GetInfo.EncDec eObject = new GetInfo.EncDec();
            _apptitle = ini.IniReadValue("Settings", "Title");
            _appfile = ini.IniReadValue("Settings", "xfile").Substring(0, ini.IniReadValue("Settings", "xfile").Length - 4);
            _server = ini.IniReadValue("DataServer", "Name");
            _user = ini.IniReadValue("DataServer", eObject.OnEncrypt("myName", eObject.Enc("myName", "User")));
            _pass = ini.IniReadValue("DataServer", eObject.OnEncrypt("myName", eObject.Enc("myName", "Pass")));
            _user = eObject.Dec("myName", eObject.OnDecrypt("myName", _user));
            _pass = eObject.Dec("myName", eObject.OnDecrypt("myName", _pass));
            _DBName = DBName;
        }
        private string _server;
        public string Server
        {
            get { return _server;}
        }
        private string _user;
        public string User
        {
            get { return _user; }
        }
        private string _pass;
        public string Pass
        {
            get { return _pass; }
        }
        private string _apptitle;
        public string AppTitle
        {
            get { return _apptitle; }
        }
        private string _appfile;
        public string AppFile
        {
            get { return _appfile; }
        }
        public string ConnectionString
        {
            get { return "Data Source=" + this.Server + ";Initial Catalog="+this.DBName +";Uid=" + this.User + ";Pwd=" + this.Pass;}
        }
        private string _DBName;
        public string DBName
        {
            get { return _DBName; }
        }

    }
}
