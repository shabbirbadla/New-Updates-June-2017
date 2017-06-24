using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;



namespace eMailClient.MailSender
{
    public class cls_Gen_Mgr_MailSender : cls_Gen_Ent_MailSender
    {
        private SmtpClient obj_SmtpClient;
        

        public cls_Gen_Mgr_MailSender()
        {
        }

        public void ConfigMail()
        {
            Obj_MailMessage = new MailMessage(FromMailAddress,ToMailAddress);
            Obj_MailMessage.Subject = Subject;
            Obj_MailMessage.Body = Body;
            Obj_MailMessage.Priority = MailPriority.Normal;
            Obj_MailMessage.IsBodyHtml = false;
            // Check CC Address if null or empty
            if (CcMailAddress != null)
            {
                if (CcMailAddress.Trim() != string.Empty)
                {
                    Obj_MailMessage.CC.Add(CcMailAddress);
                }
            }
            
            // Check BCC Address if null or empty
            if (BccMailAddress != null)
            {
                if (BccMailAddress.Trim() != string.Empty)
                {
                    Obj_MailMessage.Bcc.Add(BccMailAddress);
                }
            }

            // Check Attachment Path
            if (AttachmentPath != null)
            {
                if (AttachmentPath.Trim() != "") 
                {
                    Obj_MailMessage.Attachments.Add(new Attachment(AttachmentPath));
                }
            }

            // Network Credential
            NetworkCredential basicAuto= new NetworkCredential(MailId, MailPass); 

            // SMTP Client
            obj_SmtpClient = new SmtpClient();
            obj_SmtpClient.Host = SmtpHost;
            obj_SmtpClient.Port = SmtpPort;
            //obj_SmtpClient.EnableSsl = SmtpEnableSSL;
            obj_SmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            obj_SmtpClient.UseDefaultCredentials = false;   // Added by Sachin N. S. on 04/02/2014 for Bug-20211
            obj_SmtpClient.Credentials = basicAuto;
            //obj_SmtpClient.Timeout = 1000000;
            obj_SmtpClient.Timeout = 30000;
        }

        private void CheckConnection()
        {
            try
            {
                System.Net.IPHostEntry i = System.Net.Dns.GetHostEntry("www.google.com");
            }
            catch(WebException ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public bool Send()
        {
            bool isSuccess = false;
            try
            {

                //MessageBox.Show("reach uday");
                //frmStatus obj_form = new frmStatus();
                ////obj_form.ShowDialog();
               // CheckConnection();

                obj_SmtpClient.Send(Obj_MailMessage);
                isSuccess = true;
            }
            catch (SmtpFailedRecipientsException SmtpRecpEx)
            {
                for (int i = 0; i <= SmtpRecpEx.InnerExceptions.Length; i++)
                {
                    SmtpStatusCode smtpStatus = SmtpRecpEx.InnerExceptions[i].StatusCode;
                    if (smtpStatus == SmtpStatusCode.MailboxBusy ||
                        smtpStatus == SmtpStatusCode.MailboxUnavailable)
                    {
                        //throw new Exception("Delivery failed - retrying in 5");
                        //Thread.Sleep(1000);
                        obj_SmtpClient.Send(Obj_MailMessage);
                        isSuccess = true;
                    }
                    else
                    {
                        throw new Exception("Failed to delivery message to " + SmtpRecpEx.InnerExceptions[i].FailedRecipient.Trim());
                    }
                }
            }
            catch (SmtpException SmtpEx)
            {
                throw new SmtpException("Failure Sending Mail.." + SmtpEx.StatusCode.ToString() + ".." + SmtpEx.Message.ToString());

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            finally
            {
                Obj_MailMessage.Dispose();
            }

            return isSuccess;
        }

        public string DecryptData(string encryptpwd)
        {
            string decryptpwd = string.Empty;
            UTF8Encoding encodepwd = new UTF8Encoding();
            Decoder decode = encodepwd.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encryptpwd);
            int charCount = decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            decryptpwd = new String(decoded_char);
            return decryptpwd;
        }
    }
}
