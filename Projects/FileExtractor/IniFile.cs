using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

namespace GetInfo
{
    /// <summary>
    /// Create a New INI file to store or load data
    /// </summary>
    public class IniFile
    {
        public string path;

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        /// <summary>
        /// INIFile Constructor.
        /// </summary>
        /// <param name="INIPath"></param>
        public IniFile(string INIPath)
        {
            path = INIPath;
        }
        /// <summary>
        /// Write Data to the INI File
        /// </summary>
        /// <param name="Section"></param>
        /// Section name
        /// <param name="Key"></param>
        /// Key Name
        /// <param name="Value"></param>
        /// Value Name
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.path);
        }

        /// <summary>
        /// Read Data Value From the Ini File
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <param name="Path"></param>
        /// <returns></returns>
        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, this.path);
            return temp.ToString();

        }
        public string enc(string mcheck)
        {
            int d=0,lcChange;
            int f=mcheck.Length;
            string lcRepl="";
            int rep=0;
            char two,r;
            while(f>0)
            {
                r=Convert.ToChar(mcheck.Substring(d,1));
                lcChange=Convert.ToInt16(r)+rep;
                two=Convert.ToChar(lcChange);
                lcRepl=lcRepl+two;
                d++;
                rep++;
                f--;
            }
            return lcRepl;
        }
        public string onencrypt(string lcvariable)
        {
            string lcreturn = "";
            foreach (char ch in lcvariable)
                lcreturn = lcreturn + ((int)ch+(int)ch);
            return lcreturn;
        }
        
    }
}
