using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ueReadRegisterMe
{
    class Class1
    {
        /*    if (File.Exists(m_registerMePath[0]))
               {
                   FileStream stream = File.Open(m_registerMePath[0], FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                   StreamReader reader = new StreamReader(stream, Encoding.GetEncoding(1252));
                   fileText = reader.ReadToEnd();
                   reader.Close();
                   stream.Close();
                   /// <summary>
                   ///Sequencing the substringing of Register.me file array of data.
                   /// </summary>                
                   CompanyName = EncryptionDecryptionTask.Dec(fileText.Substring(0, 50));
                   Address1 = EncryptionDecryptionTask.Dec(fileText.Substring(50, 50));
                   Address2 = EncryptionDecryptionTask.Dec(fileText.Substring(100, 50));
                   Address3 = EncryptionDecryptionTask.Dec(fileText.Substring(150, 50));
                   Area = EncryptionDecryptionTask.Dec(fileText.Substring(200, 30));
                   City = EncryptionDecryptionTask.Dec(fileText.Substring(230, 30));
                   State = EncryptionDecryptionTask.Dec(fileText.Substring(260, 30));
                   PinCode = EncryptionDecryptionTask.Dec(fileText.Substring(290, 10));
                   Country = EncryptionDecryptionTask.Dec(fileText.Substring(300, 30));
                   PhoneNo = EncryptionDecryptionTask.Dec(fileText.Substring(330, 30));
                   MobileNo = EncryptionDecryptionTask.Dec(fileText.Substring(360, 30));
                   MacId = EncryptionDecryptionTask.Dec(fileText.Substring(390, 15));
                   Product = EncryptionDecryptionTask.Dec(fileText.Substring(405, 200));
                   MaxUsage = EncryptionDecryptionTask.Dec(fileText.Substring(605, 100));
                   RegistrationDate = EncryptionDecryptionTask.Dec(fileText.Substring(705, 10));
                   NextRunDate = EncryptionDecryptionTask.Dec(fileText.Substring(715, 10));
                   ValidUpto = EncryptionDecryptionTask.Dec(fileText.Substring(725, 10));
                   AddlComp1 = EncryptionDecryptionTask.Dec(fileText.Substring(735, 200));
                   AddlComp2 = EncryptionDecryptionTask.Dec(fileText.Substring(935, 200));       
                }
      
         public static string Dec(string text)
           {            
               int D; int rep; int Change;
               int F; string finalString = string.Empty; string R; string two;
               D = 0;
               F = text.Length;
               rep = 0;
               while (F > 0)
               {
                   R = text.Substring(D, 1);
                   int a = Asc(R,1133);
                   string ch = Chr(a,1134);
                   Change = a - rep;
                   if (Change > 255)
                   {
                       MessageBox.Show("Error Found: " + 1134 + "! Register.Me file is currupt!");
                   }
                   two = Chr(Change,1134);
                   finalString = finalString + two;
                   D = D + 1;
                   rep = rep + 1;
                   F = F - 1;
               }
               return finalString;
           }

         /// <summary>
        /// Encrypt a string using Encryption Udyog Register.Me file ecncryption Logic.
        /// </summary>
        /// <param name="toEncrypt">string to be encrypted</param>        
        /// <returns></returns>
        public static string Enc(string text)
        {
            int D; int rep; int Change;
            int F; string finalString = string.Empty; string R; string two;
            D = 0;
            F = text.Length;
            rep = 0;
            while (F>0)
	        {
	            R = text.Substring(D,1);
                int a = Asc(R,1132);
                string ch = Chr(a,1131);
                Change = a + rep;	        
                if (Change > 255)
	            {
                    MessageBox.Show("Error Found: " + 1131 + "! Register.Me file is currupt!"); 
	            }
                two = Chr(Change,1131);
                finalString = finalString + two;
                D = D + 1;
                rep = rep + 1;
                F = F - 1;
            }
            Dec(finalString);
            return finalString;
        }

        /// <summary>
        /// Decryption of a string using Decryption Udyog Register.Me file Decryption Logic.
        /// </summary>
        /// <param name="toEncrypt">string to be Decryption</param>        
        /// <returns></returns>
        public static string Dec(string text)
        {            
            int D; int rep; int Change;
            int F; string finalString = string.Empty; string R; string two;
            D = 0;
            F = text.Length;
            rep = 0;
            while (F > 0)
            {
                R = text.Substring(D, 1);
                int a = Asc(R,1133);
                string ch = Chr(a,1134);
                Change = a - rep;
                if (Change > 255)
                {
                    MessageBox.Show("Error Found: " + 1134 + "! Register.Me file is currupt!");
                }
                two = Chr(Change,1134);
                finalString = finalString + two;
                D = D + 1;
                rep = rep + 1;
                F = F - 1;
            }
            return finalString;
        }

        /// <summary>
        /// Chr method is to read any charter's ascii value should not goes down in minus.
        /// </summary>
        /// <param name="intBye">integer value(ascii value)</param>        
        /// <param name="errorcode">error code no define into program to hide the actual error 
        /// from the cutomer.</param>        
        /// <returns></returns>
        public static string Chr(int intByte,int errorCode)
        {
            if ((intByte < 0) || (intByte > 255))
            {
                MessageBox.Show("Error Found: "+errorCode+"! Register.Me file is currupt!");
                RegisterMeCheck.ProcessDetection();
                //throw new Exception(intByte+ "Must be between 1 and 255.");
            }

            byte[] bytBuffer = new byte[] { (byte)intByte };
            return Encoding.GetEncoding(1252).GetString(bytBuffer);
        }

        /// <summary>
        /// Chr method is to read any charter's ascii value should not goes down in minus.
        /// </summary>
        /// <param name="strChar">to get char value fro acsii value of character</param>        
        /// <param name="errorcode">error code no define into program to hide the actual error 
        /// from the cutomer.</param>        
        /// <returns></returns>
        private static int Asc(string strChar,int errorCode)
        {
            if ((strChar.Length == 0) || (strChar.Length > 1))
            {
                MessageBox.Show("Error Found: " + errorCode + "!Register.Me file is currupt!");
                RegisterMeCheck.ProcessDetection();
                //throw new ArgumentOutOfRangeException("p_strChar", strChar, "Must be a single character.");
            }

            char[] chrBuffer = { Convert.ToChar(strChar) };
            byte[] bytBuffer = Encoding.GetEncoding(1252).GetBytes(chrBuffer);
            return (int)bytBuffer[0];
        }
       
           */
    }
}
