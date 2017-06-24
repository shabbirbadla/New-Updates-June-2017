using System;
using System.IO; 
using System.Security.Cryptography;
using System.Text;
namespace SQLEditor.Database
{
	/// <summary>
	/// Summary description for Security.
	/// </summary>
	public class Security
	{
		private const string PASSWORD = "{1FCC37D8-E00B-4bef-99C3-529DC051082B}";
		public static string Encrypt(string clearText) 
		{ 
			string Password = PASSWORD;
			byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(clearText); 
			PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password, 
				new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76}); 

            MemoryStream ms = new MemoryStream(); 

			Rijndael alg = Rijndael.Create(); 

			alg.Key = pdb.GetBytes(32); 
			alg.IV = pdb.GetBytes(16); 

			CryptoStream cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write); 

			cs.Write(clearBytes, 0, clearBytes.Length); 

			cs.Close(); 

			byte[] encryptedData = ms.ToArray();

			return Convert.ToBase64String(encryptedData); 
		}
		public static string Decrypt(string cipherText) 
		{ 
			string Password = PASSWORD;
			byte[] cipherBytes = Convert.FromBase64String(cipherText); 

			PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password, 
				new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76}); 

            MemoryStream ms = new MemoryStream(); 

			Rijndael alg = Rijndael.Create(); 
			alg.Key = pdb.GetBytes(32); 
			alg.IV = pdb.GetBytes(16); 

			CryptoStream cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write); 

			cs.Write(cipherBytes, 0, cipherBytes.Length); 
			cs.Close(); 
			byte[] decryptedData = ms.ToArray(); 

			return System.Text.Encoding.Unicode.GetString(decryptedData); 
		}
        public static int Asc(string p_strChar)
        {
            if ((p_strChar.Length == 0) || (p_strChar.Length > 1))
            {
                throw new ArgumentOutOfRangeException("p_strChar", p_strChar, "Must be a single character.");
            }
            char[] chrBuffer = { Convert.ToChar(p_strChar) };
            byte[] bytBuffer = Encoding.GetEncoding(1252).GetBytes(chrBuffer);
            return (int)bytBuffer[0];
        }
        public static string Chr(int p_intByte)
        {
            if ((p_intByte < 0) || (p_intByte > 255))
            {
                throw new ArgumentOutOfRangeException("p_intByte", p_intByte, "Must be between 1 and 255.");
            }
            byte[] bytBuffer = new byte[] { (byte)p_intByte };
            return Encoding.GetEncoding(1252).GetString(bytBuffer);
        }
	}
}