using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Security.Cryptography;
using uegencomfunc;

namespace WindowsFormsApplication1
{
    public class EncrytDecrypt
    {
        public string newEncrypt(string _EncryptVal, string _EncryptPass)
        {
            string lcVFPEncryptionFile="", _FileValidErr="", _EncRetValue="*1*";
            StreamReader sr = new StreamReader("D:\\Usquare\\uecon.exe", Encoding.GetEncoding(1252));
            lcVFPEncryptionFile = sr.ReadToEnd();
            uegencomfunc.uegencomfuncClass GenComFun = new uegencomfunc.uegencomfuncClass();
            int nobj = 5;
            string cChkVal1 = (string)GenComFun.gethashconvcode("D:\\Usquare\\", lcVFPEncryptionFile, nobj);
            nobj = 4;
            string cChkVal2 = (string)GenComFun.gethashconvcode("D:\\Usquare\\", lcVFPEncryptionFile, nobj);

	        if (lcVFPEncryptionFile.ToString().Length == 122880 && cChkVal1 == "20A7D4AF02E62A362CE44FCBAB6EB5FE" 
                    && cChkVal2 == "DBD1EC320842ED0DB1A4DAE26F0513E4DF1F9D1C65FB93D896E553940ACFA408479EDFBE78FCEA9901915384E5FBC1C50CCD00709C0CD870CA8A4C8BC40676EF")
            {}
	        else 
            {
	   	        _FileValidErr = "Error";
	        }
	        if (_FileValidErr=="")
            {
                _EncRetValue = (string)GenComFun.getencryptcode("D:\\Usquare\\", _EncryptVal, _EncryptPass);
            }
            else
            {
                _EncRetValue = "*1*";
            }
            return _EncRetValue;
        }


        public string newDecrypt(string _DecryptVal, string _DecryptPass)
        {
            string lcVFPEncryptionFile="", _FileValidErr="", _DecRetValue="+1+";
            StreamReader sr = new StreamReader("D:\\Usquare\\uecon.exe", Encoding.GetEncoding(1252));
            lcVFPEncryptionFile = sr.ReadToEnd();
            uegencomfunc.uegencomfuncClass GenComFun = new uegencomfunc.uegencomfuncClass();
            int nobj = 5;
            string cChkVal1 = (string)GenComFun.gethashconvcode("D:\\Usquare\\", @""+lcVFPEncryptionFile, nobj);
            nobj = 4;
            string cChkVal2 = (string)GenComFun.gethashconvcode("D:\\Usquare\\", @""+lcVFPEncryptionFile, nobj);

	        if (lcVFPEncryptionFile.ToString().Length == 122880 && cChkVal1 == "20A7D4AF02E62A362CE44FCBAB6EB5FE" 
                    && cChkVal2 == "DBD1EC320842ED0DB1A4DAE26F0513E4DF1F9D1C65FB93D896E553940ACFA408479EDFBE78FCEA9901915384E5FBC1C50CCD00709C0CD870CA8A4C8BC40676EF")
            {}
	        else 
            {
	   	        _FileValidErr = "Error";
	        }
	        if (_FileValidErr=="")
            {
                _DecRetValue = (string)GenComFun.getdecryptcode("D:\\Usquare\\", _DecryptVal, _DecryptPass);
            }
            else
            {
                _DecRetValue = "+2+";
            }
            return _DecRetValue;
        }

        public string Decrypt(string _decValue)
        {
            int D = 0, F = _decValue.Length, rep = 0, Change = 0;
            string Repl = "", two = "", R = "";
            while (F > 0)
            {
                R = _decValue.Substring(D, 1);
                char[] r = { Convert.ToChar(R) };
                Change = ((int)(Encoding.GetEncoding(1252).GetBytes(r))[0]) - rep;
                if (Change > 0)
                {
                    two = Convert.ToChar(Change).ToString();
                }
                Repl += two;
                D += 1;
                F -= 1;
                rep += 1;
            }
            return Repl;
        }

        public string Encrypt(string _decValue)
        {
            int D = 0, F = _decValue.Length, rep = 0, Change = 0;
            string Repl = "", two = "", R = "";
            while (F > 0)
            {
                R = _decValue.Substring(D, 1);
                char[] r = { Convert.ToChar(R) };
                Change = ((int)(Encoding.GetEncoding(1252).GetBytes(r))[0]) + rep;
                if (Change > 0)
                {
                    two = Convert.ToChar(Change).ToString();
                }
                Repl += two;
                D += 1;
                F -= 1;
                rep += 1;
            }
            return Repl;
        }
    }
}
