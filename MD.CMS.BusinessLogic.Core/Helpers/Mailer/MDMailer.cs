using MD.Tools.Helpers.Core.Net.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;
using MD.CMS.BusinessLogic.Core.Properties;
using System.Web;
using System.Net.Mime;
namespace MD.CMS.BusinessLogic.Core.Helpers.Mailer
{
    public class MDMailer : IEmailSender
    {
        #region Attributes
        private int _smtpPort;
        private string _smtpServer;
        private bool _enableTls;
        private string _senderAddress;
        private string _senderPassword;
        #endregion

        #region Properties
        public int SmtpPort
        {
            get { return _smtpPort; }
            set { _smtpPort = value; }
        }
        public string SmtpServer
        {
            get { return _smtpServer; }
            set { _smtpServer = value; }
        }

        public bool EnableTls
        {
            get { return _enableTls; }
            set { _enableTls = value; }
        }

        public string SenderAddress
        {
            get { return _senderAddress; }
            set { _senderAddress = value; }
        }
        public string SenderPassword
        {
            get { return _senderPassword; }
            set { _senderPassword = value; }
        }
        #endregion


        #region Methods
        public void Send(System.Net.Mail.MailMessage message)
        {
            SmtpClient client = new SmtpClient
            {
                Port = _smtpPort,
                Host = _smtpServer,
                EnableSsl = _enableTls,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_senderAddress, _senderPassword),
                Timeout = 20000
            };
            client.Send(message);


        }
        #endregion




      

       
    }
}
