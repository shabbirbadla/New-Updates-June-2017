using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace eMailClient.MailSender
{
    public class cls_Gen_Ent_MailSender
    {
        private string fromMailAddress;
        public string FromMailAddress
        {
            get { return fromMailAddress; }
            set { fromMailAddress = value; }
        }
        private string toMailAddress;
        public string ToMailAddress
        {
            get { return toMailAddress; }
            set { toMailAddress = value; }
        }

        private string bccMailAddress;
        public string BccMailAddress
        {
            get { return bccMailAddress; }
            set { bccMailAddress = value; }
        }

        private string ccMailAddress;
        public string CcMailAddress
        {
            get { return ccMailAddress; }
            set { ccMailAddress = value; }
        }

        private MailMessage obj_MailMessage;
        public MailMessage Obj_MailMessage
        {
            get { return obj_MailMessage; }
            set { obj_MailMessage = value; }
        }

        private string subject;
        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        private string attachmentPath;
        public string AttachmentPath
        {
            get { return attachmentPath; }
            set { attachmentPath = value; }
        }
        private string body;
        public string Body
        {
            get { return body; }
            set { body = value; }
        }

        private string smtpHost;
        public string SmtpHost
        {
            get { return smtpHost; }
            set { smtpHost = value; }
        }

        private int smtpPort;
        public int SmtpPort
        {
            get { return smtpPort; }
            set { smtpPort = value; }
        }

        private bool smtpEnableSSL;
        public bool SmtpEnableSSL
        {
            get { return smtpEnableSSL; }
            set { smtpEnableSSL = value; }
        }

        private string mailId;
        public string MailId
        {
            get { return mailId; }
            set { mailId = value; }
        }
        private string mailPass;
        public string MailPass
        {
            get { return mailPass; }
            set { mailPass = value; }
        }

        private bool isError;
        public bool IsError
        {
            get { return isError; }
            set { isError = value; }
        }

        private string errorMsg;
        public string ErrorMsg
        {
            get { return errorMsg; }
            set { errorMsg = value; }
        }
    }
}
