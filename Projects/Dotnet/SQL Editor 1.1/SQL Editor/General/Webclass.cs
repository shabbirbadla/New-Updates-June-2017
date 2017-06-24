using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SQLEditor.USILWeb
{
	/// <summary>
	/// Summary description for Settings.
	/// </summary>
    [Serializable]
    public class USILWeb : System.Net.WebClient
    {
        public bool lSuccuss;
        public USILWeb()
        {
            this.lSuccuss = false;
        }

        public int DownloadFile(string UriAddress, string FilePath,char cNew) 
        {
            try
            {
                DownloadFile(UriAddress, FilePath);
                this.lSuccuss = true;
                return 1;
            }
            catch
            {
                this.lSuccuss = false;
                return -1;
            }
        }
    }
}
