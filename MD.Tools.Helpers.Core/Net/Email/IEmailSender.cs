using System.Net.Mail;


namespace MD.Tools.Helpers.Core.Net.Email
{
    /// <summary>
    /// Defines the behaviour of an email sender
    /// </summary>
    /// <remarks>
    /// Implement <see cref="IEmailSender"/> if your class implements email sending functionality.
    /// To route an application's email traffic through your class, inject an instance into the EmailService class's EmailSender property.
    /// </remarks>
    public interface IEmailSender
    {

        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="message">The message.</param>
        /// <remarks>
        /// Use this method to send an email using the full power of the .NET Framework MailMessage class
        /// </remarks>
        void Send(MailMessage message);
    }


}
