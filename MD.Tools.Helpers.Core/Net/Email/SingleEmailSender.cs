using System;
using System.Net.Mail;
using System.Text;

namespace MD.Tools.Helpers.Core.Net.Email
{
    /// <summary>
    /// Sends single emails for lightweight implementations such as order confirmations and password resets
    /// </summary>
    /// <remarks>
    /// <para>
    /// To route an application's email traffic through this class, inject an instance of it into the EmailService class's EmailSender property.
    /// </para>
    /// <example>
    /// <code>
    /// &lt;system.net&gt;
    ///     &lt;mailSettings&gt;
    ///         &lt;smtp deliveryMethod="PickupDirectoryFromIis" from="no-reply@example.com"&gt;
    ///             &lt;network host="localhost"  /&gt;
    ///         &lt;/smtp&gt;
    ///     &lt;/mailSettings&gt;
    /// &lt;/system.net&gt;
    /// </code>
    /// </example>
    /// <para>See http://msdn.microsoft.com/en-us/library/w355a94k.aspx for more information</para>
    /// </remarks>
    public sealed class SingleEmailSender : IEmailSender
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleEmailSender"/> class.
        /// </summary>
        /// <remarks><para>Internaly creates a new <see ref="SmtpClient" /> instance</para></remarks>
        public SingleEmailSender()
        {
            Client = new SmtpClient();
        }

        /// <summary>
        /// Gets the current <see ref="System.Net.Mail.SmtpClient" />
        /// </summary>
        private SmtpClient Client { get; set; }

        #region IEmailSender Members


        
#pragma warning disable CA1200 // Avoid using cref tags with a prefix
        /// <summary>
        /// Sends an email
        /// </summary>
        /// <remarks>
        /// <para>Use this method to send an email using the full power of the .NET Framework <see ref="MailMessage" /> class</para>
        /// <para>It is the Callers responsibility to call the <see cref="M:MailMessage.Dispose"/> method to release resources</para>
        /// </remarks>
        /// <param name="message">An instance of <see ref="MailMessage" /> containing the email to send.</param>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="message"/> is null</exception>
        void IEmailSender.Send(MailMessage message)
#pragma warning restore CA1200 // Avoid using cref tags with a prefix
        {
            EmailService.ValidateParameters(message);
            Client.Send(message);
            Logging.Logger.LogInformation(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Sent email {0} to {1}", message.Subject, message.To.ToString()));
        }

        #endregion


    }

}
