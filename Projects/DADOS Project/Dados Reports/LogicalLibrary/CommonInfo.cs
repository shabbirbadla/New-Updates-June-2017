using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Net;
using System.Configuration;
using System.Net.Mail;
using System.Net.Mime;
using System.Windows.Forms;


namespace LogicalLibrary
{
    public class CommonInfo
    {
        #region Constructor

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public CommonInfo() { }

        #endregion

        #region Spliting the Query Arguments
        /// <summary>
        /// spiting the arguments([,],=,|)
        /// </summary>
        /// <author>Jayakumar Budha</author>
        /// <param name="Value">string</param>
        /// <returns>array of string</returns>
        public string[] spiltArguments(string Value)
        {
            Value = Value.Replace("][", "|");
            Value = Value.Replace("[", "");
            Value = Value.Replace("]", "");
            Value = Value.Replace("=", "|");


            return Value.Split(new char[] { '|' });

        }
        #endregion

        #region Replacing Special Carectors
        /// <summary>
        /// Replacing Special Charectors (~,",#,%,&,*,:,<,>,{,},?,/,\,|) to (_)
        /// </summary>
        /// <author>Jayakumar Budha</author>
        /// <param name="strOldFileName">string</param>
        /// <param name="strNewFileName">string</param>
        /// <returns>strnewFileName as empty or value</returns>        
        public string replaceSpecialChars(string strOldFileName, string strNewFileName)
        {
            char[] spChar = new char[] { '"' };
            byte[] spByte = ASCIIEncoding.ASCII.GetBytes(spChar);
            string spString = ASCIIEncoding.ASCII.GetString(spByte);

            if (strOldFileName.Contains("~"))
            {
                if (strNewFileName == string.Empty)
                    strNewFileName = strOldFileName.Replace("~", "_");
                else
                    strNewFileName = strNewFileName.Replace("~", "_");
            }

            if (strOldFileName.Contains(spString))
            {
                if (strNewFileName == string.Empty)
                    strNewFileName = strOldFileName.Replace(spString, "_");
                else
                    strNewFileName = strNewFileName.Replace(spString, "_");
            }

            if (strOldFileName.Contains("#"))
            {
                if (strNewFileName == string.Empty)
                    strNewFileName = strOldFileName.Replace("#", "_");
                else
                    strNewFileName = strNewFileName.Replace("#", "_");
            }

            if (strOldFileName.Contains("%"))
            {
                if (strNewFileName == string.Empty)
                    strNewFileName = strOldFileName.Replace("%", "_");
                else
                    strNewFileName = strNewFileName.Replace("%", "_");
            }

            if (strOldFileName.Contains("&"))
            {
                if (strNewFileName == string.Empty)
                    strNewFileName = strOldFileName.Replace("&", "and");
                else
                    strNewFileName = strNewFileName.Replace("&", "and");
            }

            if (strOldFileName.Contains("*"))
            {
                if (strNewFileName == string.Empty)
                    strNewFileName = strOldFileName.Replace("*", "_");
                else
                    strNewFileName = strNewFileName.Replace("*", "_");
            }

            if (strOldFileName.Contains(":"))
            {
                if (strNewFileName == string.Empty)
                    strNewFileName = strOldFileName.Replace(":", "_");
                else
                    strNewFileName = strNewFileName.Replace(":", "_");
            }

            if (strOldFileName.Contains("<"))
            {
                if (strNewFileName == string.Empty)
                    strNewFileName = strOldFileName.Replace("<", "_");
                else
                    strNewFileName = strNewFileName.Replace("<", "_");
            }

            if (strOldFileName.Contains(">"))
            {
                if (strNewFileName == string.Empty)
                    strNewFileName = strOldFileName.Replace(">", "_");
                else
                    strNewFileName = strNewFileName.Replace(">", "_");
            }

            if (strOldFileName.Contains("?"))
            {
                if (strNewFileName == string.Empty)
                    strNewFileName = strOldFileName.Replace("?", "_");
                else
                    strNewFileName = strNewFileName.Replace("?", "_");
            }

            if (strOldFileName.Contains("/"))
            {
                if (strNewFileName == string.Empty)
                    strNewFileName = strOldFileName.Replace("/", "_");
                else
                    strNewFileName = strNewFileName.Replace("/", "_");
            }

            if (strOldFileName.Contains(@"\"))
            {
                if (strNewFileName == string.Empty)
                    strNewFileName = strOldFileName.Replace(@"\", "_");
                else
                    strNewFileName = strNewFileName.Replace(@"\", "_");
            }

            if (strOldFileName.Contains("{"))
            {
                if (strNewFileName == string.Empty)
                    strNewFileName = strOldFileName.Replace("{", "_");
                else
                    strNewFileName = strNewFileName.Replace("{", "_");
            }

            if (strOldFileName.Contains("}"))
            {
                if (strNewFileName == string.Empty)
                    strNewFileName = strOldFileName.Replace("}", "_");
                else
                    strNewFileName = strNewFileName.Replace("}", "_");
            }

            if (strOldFileName.Contains("|"))
            {
                if (strNewFileName == string.Empty)
                    strNewFileName = strOldFileName.Replace("|", "_");
                else
                    strNewFileName = strNewFileName.Replace("|", "_");
            }

            return strNewFileName;
        }
        #endregion

        #region verifying mailid's or valid or not
        /// <summary>
        /// method for determining is the user provided a valid email address
        /// We use regular expressions in this check, as it is a more thorough
        /// way of checking the address provided
        /// </summary>
        /// <author>Jayakumar Budha</author>
        /// <param name="email">email address to validate</param>
        /// <returns>true is valid, false if not valid</returns>
        public bool IsValidEmail(string email)
        {
            //regular expression pattern for valid email
            //addresses, allows for the following domains:
            //com,edu,info,gov,int,mil,net,org,biz,name,museum,coop,aero,pro,tv,co.in,co,in
            string pattern = @"^[-a-zA-Z0-9][-.a-zA-Z0-9]*@[-.a-zA-Z0-9]+(\.[-.a-zA-Z0-9]+)*\.
    (com|edu|info|gov|int|mil|net|org|biz|name|museum|coop|aero|pro|tv|co.in|co|in|[a-zA-Z]{2})$";
            //Regular expression object
            Regex check = new Regex(pattern, RegexOptions.IgnorePatternWhitespace);
            //boolean variable to return to calling method
            bool valid = false;

            //make sure an email address was provided
            if (string.IsNullOrEmpty(email))
            {
                valid = false;
            }
            else
            {
                //use IsMatch to validate the address
                valid = check.IsMatch(email);
            }
            //return the value to the calling method
            return valid;
        }
        #endregion

        #region Sending Mail
        /// <summary>
        /// Sending Notification Mail
        /// </summary>
        /// <author>Jayakumar Budha</author>
        /// <param name="strToMailID">take paramenter as string array</param>
        /// <param name="strFromMailID">take paramenter as string</param>
        /// <param name="strCCMailID">take paramenter as string array, you can send the null value also</param>
        /// <param name="strBCCMailID">take paramenter as string array, you can send the null value also</param>
        /// <param name="strSubject">take paramenter as string, you can send the null value or "" also</param>
        /// <param name="strMailBody">take paramenter as string, you can send the null value or "" also</param>
        /// <param name="AttachmentLocation">take paramenter as string, you con't send null or ""</param>
        /// <param name="XSLAttachmentLocation">take paramenter as string, if it will be mail attachment is XML File Then You will Send this string or you can send as null or ""</param>
        /// <returns>if sucsses true, not sucsses false </returns>
        public bool sendMail(string[] strToMailID, string strFromMailID, string[] strCCMailID, string[] strBCCMailID, string strSubject, string strMailBody, string AttachmentLocation, string XSLAttachmentLocation, string SMPTHost, string SMPTPort, string NetworkCredentialUserName, string NetworkCredentialPassword)
        {
            #region Configring smtpClient

            string strSmptHost = SMPTHost;
            string strSmptPort = SMPTPort;
            string struserName = NetworkCredentialUserName;
            string strPassword = NetworkCredentialPassword;
            string strFromAddress = strFromMailID;
            string strMailSubject = strSubject;
            string stringMailBody = strMailBody;
            string[] ToMailIDs = strToMailID;
            SmtpClient client = new SmtpClient();
            //SmtpClient client = new SmtpClient(Properties.Settings.Default.SMTPAddress);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Host = strSmptHost;
            client.Port = Convert.ToInt32(strSmptPort);
            // setup Smtp authentication
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(struserName, strPassword);
            client.UseDefaultCredentials = false;
            client.UseDefaultCredentials = true;
            client.EnableSsl = true;
            client.Credentials = credentials;
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(strFromAddress);
            if (ToMailIDs != null)
            {
                foreach (string tomailid in ToMailIDs)
                {
                    msg.To.Add(new MailAddress(tomailid));
                }
            }

            if (strCCMailID != null)
            {
                foreach (string ccmailid in strCCMailID)
                {
                    msg.CC.Add(new MailAddress(ccmailid));
                }
            }

            if (strBCCMailID != null)
            {
                foreach (string bccmailid in strBCCMailID)
                {
                    msg.Bcc.Add(new MailAddress(bccmailid));
                }
            }

            #endregion

            #region Adding Mail Subject And Body

            msg.Subject = strMailSubject;
            msg.IsBodyHtml = false;
            msg.Body = strMailBody;

            #endregion

            #region Adding Attachment for mail

            if (XSLAttachmentLocation != "" && XSLAttachmentLocation != null)
            {
                Attachment attachment1 = new Attachment(AttachmentLocation, System.Net.Mime.MediaTypeNames.Application.Octet);
                Attachment attachment2 = new Attachment(XSLAttachmentLocation, System.Net.Mime.MediaTypeNames.Application.Octet);
                msg.Attachments.Add(attachment1);
                msg.Attachments.Add(attachment2);
            }
            else
            {
                Attachment attachment = new Attachment(AttachmentLocation, System.Net.Mime.MediaTypeNames.Application.Octet);
                msg.Attachments.Add(attachment);
            }

            #endregion

            #region Sending Mail
            try
            {
                client.Send(msg);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Mail Not Sent Due to internal Error :  " + "\r\n" + ex.Message + "\r\n" + "please Send once again");
            }
            #endregion
        }
        #endregion
    }
}
