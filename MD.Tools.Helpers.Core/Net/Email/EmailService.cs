using System;
using System.Globalization;
using System.Net.Mail;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MD.Tools.Helpers.Core.TypeConversion;

namespace MD.Tools.Helpers.Core.Net.Email
{
    /// <summary>
    /// A static class that provides persistent and extensible email client functionality to MD-CMS projects
    /// </summary>
    /// <remarks>
    /// <para>IMPORTANT: This class must be configured within a web Application Start 
    /// event to avoid threading issues when setting the <see ref="IEmailSender" /></para>
    /// <para>This class is intended to be used whenever emails need to be sent from 
    /// a MD-CMS application.</para>
    /// <para>EmailService utilises dependency injection to allow pluggability of email senders.</para>
    /// <para>In the future, further senders for testing and bulk email may be supported.
    /// To implement your own sender, simply implement the <see ref="IEmailSender" /> 
    /// interface and inject an instance of your class into <see ref="P:EmailService.EmailSender" />.</para>
    /// </remarks>
    /// <example>
    /// <para>On Application start you need to configure the IEmailSender implementation to
    /// use within the application</para>
    /// <code>
    /// //Assign a new SingleEmailSender to the EmailService.
    /// //You could use a NullEmailSender or BulkEmailSender here instead.
    /// EmailService.EmailSender = new SingleEmailSender();
    /// </code>
    /// <para>To send an email</para>
    /// <code>
    /// // Configure Mail Message to Send
    /// MailAddress from = null;
    /// MailAddress to = null;
    /// EmailContent[] emailContent = null;
    /// 
    /// //Create an array of content consisting of the MIME content you wish to send
    /// emailContent = new EmailContent[2];
    /// emailContent[0] = new PlainTextContent("Hello World!", Encoding.ASCII);
    /// emailContent[1] = new HtmlContent("&lt;html&gt;&lt;body&gt;&lt;h1&gt;Hello World!&lt;/h1&gt;&lt;/body&gt;&lt;/html&gt;", Encoding.ASCII);
    /// 
    /// //Send the email
    /// EmailService.Send(from, to, "Hello World!", emailContent);
    /// </code>
    /// </example>
    public static class EmailService
    {
        private static volatile IEmailSender _emailSender;
        private static object _syncRoot = new Object();
        private static Regex _testEmail = new Regex(MD.Tools.Helpers.Core.Properties.HelperSettings.Default.TestEmailRegex, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Injected dependency for sending email.
        /// </summary>
        /// <remarks>
        /// The SingleEmailSender class implements the IEmailSender interface.
        /// Use this class to send single emails.
        /// </remarks>
        /// <example>
        /// <code>
        /// SmtpClient client = null;
        /// 
        /// client = new SmtpClient();
        /// EmailService.EmailSender = SingleEmailSender.Create(client);
        /// </code>
        /// </example>
        public static IEmailSender EmailSender
        {
            get
            {
                lock (_syncRoot)
                {
                    if (_emailSender == null) _emailSender = new SingleEmailSender();
                    return _emailSender;
                }
            }
            set
            {
                lock (_syncRoot)
                {
                    _emailSender = value;
                }
            }
        }

        /// <summary>
        /// Sends an email using the injected IEmailSender
        /// </summary>
        /// <remarks>
        /// Use this method to send an email with a plain text body and default encoding
        /// </remarks>
        /// <param name="from">An instance of <see ref="MailAddress" /> containing the sender's display name and address.</param>
        /// <param name="to">An instance of <see ref="MailAddress" /> containing the recipient's display name and address.</param>
        /// <param name="subject">A string containing the email subject line.</param>
        /// <param name="body">A string containing the email body</param>
        /// <exception cref="System.ArgumentNullException">Throw when <paramref name="from"/>, <paramref name="to"/>, <paramref name="subject"/>, <paramref name="body"/> or the underlying <see ref="IEmailSender" /> is null</exception>
        public static void Send(MailAddress from, MailAddress to, string subject, string body)
        {
            ValidateParameters(from, to, subject, body);
            lock (_syncRoot)
            {
                using (MailMessage msg = new MailMessage(from, to))
                {
                    msg.Subject = subject;
                    msg.Body = body;
                    Send(msg);
                }
            }
        }

        /// <summary>
        /// Sends an email using the injected <see ref="IEmailSender" />
        /// </summary>
        /// <remarks>
        /// Use this method to send an email with one or more bodies each with its own encoding and/or MIME type
        /// </remarks>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="emailContent"></param>
        /// <param name="attachment"></param>
        public static void Send(MailAddress from, MailAddress to, string subject, EmailContent[] emailContent, Attachment attachment = null)
        {
            ValidateParameters(from, to, subject, emailContent);
            lock (_syncRoot)
            {
                using (MailMessage mailMessage = new MailMessage(from, to))
                {
                    mailMessage.Subject = subject;
                    if (attachment != null)
                    {
                        mailMessage.Attachments.Add(attachment);
                    }
                    foreach (EmailContent item in emailContent)
                    {
                        mailMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(item.Body, item.Encoding, item.ContentType.MediaType));
                    }

                    Send(mailMessage);
                }
            }
        }

        /// <summary>
        /// Sends an email using the injected <see ref="IEmailSender" />
        /// </summary>
        /// <remarks>
        /// Use this method to send an email using the full power of the .NET Framework <see ref="MailMessage" /> class
        /// </remarks>
        /// <param name="message">An instance of <see ref="MailMessage" /> containing the email to send.</param>
        /// <exception cref="System.ArgumentNullException">Throw when <paramref name="message"/> or the underlying <see ref="IEmailSender" /> is null</exception>
        public static void Send(MailMessage message)
        {
            ValidateParameters(message);
            lock (_syncRoot)
            {
                if (MD.Tools.Helpers.Core.Properties.HelperSettings.Default.RedirectAllEmail)
                {
                    RedirectEmail(message.To);
                    RedirectEmail(message.CC);
                    RedirectEmail(message.Bcc);
                }
                else
                {
                    ProcessTestEmailAddresses(message.To);
                    ProcessTestEmailAddresses(message.CC);
                    ProcessTestEmailAddresses(message.Bcc);
                }
                EmailSender.Send(message);
            }
        }

        private static void RedirectEmail(MailAddressCollection mac)
        {
            if (mac == null || mac.Count == 0) return;
            List<string> addresses = new List<string>();
            foreach (MailAddress ma in mac)
            {
                addresses.Add(ma.Address);
            }
            MailAddress mared = new MailAddress(MD.Tools.Helpers.Core.Properties.HelperSettings.Default.RedirectEmailAddress, string.Format(CultureInfo.InvariantCulture, MD.Tools.Helpers.Core.Properties.HelperSettings.Default.RedirectDisplayNameTemplate, string.Join(", ", addresses.ToArray())));
            mac.Clear();
            mac.Add(mared);
        }

        private static void ProcessTestEmailAddresses(MailAddressCollection mac)
        {
            if (mac == null || mac.Count == 0) return;
            for (int i = mac.Count - 1; i >= 0; i--)
            {
                MailAddress ma = mac[i];
                Match m = _testEmail.Match(ma.Address);
                while (m != null && m.Success)
                {
                    mac.Add(ma.Address.Replace(m.Groups[1].Value, "@", StringComparison.OrdinalIgnoreCase));
                    m = m.NextMatch();
                }
            }
        }

        #region Parameter Validation Methods

        /// <summary>
        /// Validates the parameters.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="subject">The subject.</param>
        internal static void ValidateParameters(MailAddress from, MailAddress to, string subject)
        {

            if (null == from)
            {
                throw new ArgumentNullException(nameof(from));
            }

            if (null == to)
            {
                throw new ArgumentNullException(nameof(to));
            }

            if (null == subject)
            {
                throw new ArgumentNullException(nameof(subject));
            }
        }

        /// <summary>
        /// Validates the parameters.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        internal static void ValidateParameters(MailAddress from, MailAddress to, string subject, string body)
        {
            ValidateParameters(from, to, subject);
            if (null == body)
            {
                throw new ArgumentNullException(nameof(body));
            }
        }

        /// <summary>
        /// Validates the parameters.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="emailContent">Content of the email.</param>
        internal static void ValidateParameters(MailAddress from, MailAddress to, string subject, EmailContent[] emailContent)
        {
            ValidateParameters(from, to, subject);
            if (null == emailContent)
            {
                throw new ArgumentNullException(nameof(emailContent));
            }
            if (emailContent.Length == 0) throw new NotSupportedException("You must provide at least one Email Content item");
            foreach (EmailContent content in emailContent)
            {
                ValidateEmailContent(content);
            }
        }

        /// <summary>
        /// Validates the parameters.
        /// </summary>
        /// <param name="message">The message.</param>
        internal static void ValidateParameters(MailMessage message)
        {
            if (null == message)
            {
                throw new ArgumentNullException(nameof(message));
            }
        }

        /// <summary>
        /// Validates that the <see ref="EmailContent" /> implementation is correctly populated
        /// </summary>
        /// <param name="content">The content.</param>
        private static void ValidateEmailContent(EmailContent content)
        {
            if (null == content)
            {
                throw new ArgumentNullException(nameof(content));
            }
            if (string.IsNullOrEmpty(content.Body))
            {
                throw new NotSupportedException("You must provide a body text for the {0} Email Content".ToFormattedString( content.ContentType));
            }
        }

        #endregion

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static void Send(MailAddress mailAddress1, MailAddress mailAddress2, HtmlContent[] htmlContent)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            throw new NotImplementedException();
        }
    }

}
